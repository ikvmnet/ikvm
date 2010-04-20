/*
  Copyright (C) 2009 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using IKVM.Reflection.Reader;
using IKVM.Reflection.Writer;
using IKVM.Reflection.Emit;

namespace IKVM.Reflection
{
	sealed class MethodSignature : Signature
	{
		// as an optimization, we could pack the custom modifiers (like in MethodBuilder)
		private readonly Type returnType;
		private readonly Type[] returnTypeOptionalCustomModifiers;
		private readonly Type[] returnTypeRequiredCustomModifiers;
		private readonly Type[] parameterTypes;
		private readonly Type[][] parameterOptionalCustomModifiers;
		private readonly Type[][] parameterRequiredCustomModifiers;
		private readonly CallingConventions callingConvention;
		private readonly int genericParamCount;

		private MethodSignature(Type returnType, Type[] returnTypeOptionalCustomModifiers, Type[] returnTypeRequiredCustomModifiers,
			Type[] parameterTypes, Type[][] parameterOptionalCustomModifiers, Type[][] parameterRequiredCustomModifiers,
			CallingConventions callingConvention, int genericParamCount)
		{
			this.returnType = returnType;
			this.returnTypeOptionalCustomModifiers = returnTypeOptionalCustomModifiers;
			this.returnTypeRequiredCustomModifiers = returnTypeRequiredCustomModifiers;
			this.parameterTypes = parameterTypes;
			this.parameterOptionalCustomModifiers = parameterOptionalCustomModifiers;
			this.parameterRequiredCustomModifiers = parameterRequiredCustomModifiers;
			this.callingConvention = callingConvention;
			this.genericParamCount = genericParamCount;
		}

		public override bool Equals(object obj)
		{
			MethodSignature other = obj as MethodSignature;
			return other != null
				&& other.callingConvention == callingConvention
				&& other.genericParamCount == genericParamCount
				&& other.returnType.Equals(returnType)
				&& Util.ArrayEquals(other.returnTypeOptionalCustomModifiers, returnTypeOptionalCustomModifiers)
				&& Util.ArrayEquals(other.returnTypeRequiredCustomModifiers, returnTypeRequiredCustomModifiers)
				&& Util.ArrayEquals(other.parameterTypes, parameterTypes)
				&& Util.ArrayEquals(other.parameterOptionalCustomModifiers, parameterOptionalCustomModifiers)
				&& Util.ArrayEquals(other.parameterRequiredCustomModifiers, parameterRequiredCustomModifiers);
		}

		public override int GetHashCode()
		{
			return genericParamCount ^ 77 * (int)callingConvention
				^ 3 * returnType.GetHashCode()
				^ Util.GetHashCode(returnTypeOptionalCustomModifiers) * 33
				^ Util.GetHashCode(returnTypeRequiredCustomModifiers) * 55
				^ Util.GetHashCode(parameterTypes) * 5
				^ Util.GetHashCode(parameterOptionalCustomModifiers)
				^ Util.GetHashCode(parameterRequiredCustomModifiers);
		}

		private sealed class UnboundGenericMethodContext : IGenericContext
		{
			private readonly IGenericContext original;

			internal UnboundGenericMethodContext(IGenericContext original)
			{
				this.original = original;
			}

			public Type GetGenericTypeArgument(int index)
			{
				return original.GetGenericTypeArgument(index);
			}

			public Type GetGenericMethodArgument(int index)
			{
				return UnboundGenericMethodParameter.Make(index);
			}
		}

		internal static MethodSignature ReadSig(ModuleReader module, ByteReader br, IGenericContext context)
		{
			CallingConventions callingConvention;
			int genericParamCount;
			Type returnType;
			Type[] returnTypeOptionalCustomModifiers;
			Type[] returnTypeRequiredCustomModifiers;
			Type[] parameterTypes;
			Type[][] parameterOptionalCustomModifiers;
			Type[][] parameterRequiredCustomModifiers;
			byte flags = br.ReadByte();
			switch (flags & 7)
			{
				case DEFAULT:
					callingConvention = CallingConventions.Standard;
					break;
				case VARARG:
					callingConvention = CallingConventions.VarArgs;
					break;
				default:
					throw new BadImageFormatException();
			}
			if ((flags & HASTHIS) != 0)
			{
				callingConvention |= CallingConventions.HasThis;
			}
			if ((flags & EXPLICITTHIS) != 0)
			{
				callingConvention |= CallingConventions.ExplicitThis;
			}
			genericParamCount = 0;
			if ((flags & GENERIC) != 0)
			{
				genericParamCount = br.ReadCompressedInt();
				context = new UnboundGenericMethodContext(context);
			}
			int paramCount = br.ReadCompressedInt();
			ReadCustomModifiers(module, br, context, out returnTypeRequiredCustomModifiers, out returnTypeOptionalCustomModifiers);
			returnType = ReadRetType(module, br, context);
			parameterTypes = new Type[paramCount];
			parameterOptionalCustomModifiers = null;
			parameterRequiredCustomModifiers = null;
			for (int i = 0; i < parameterTypes.Length; i++)
			{
				if ((callingConvention & CallingConventions.VarArgs) != 0 && br.PeekByte() == SENTINEL)
				{
					Array.Resize(ref parameterTypes, i);
					if (parameterOptionalCustomModifiers != null)
					{
						Array.Resize(ref parameterOptionalCustomModifiers, i);
					}
					if (parameterRequiredCustomModifiers != null)
					{
						Array.Resize(ref parameterRequiredCustomModifiers, i);
					}
					break;
				}
				if (IsCustomModifier(br.PeekByte()))
				{
					if (parameterOptionalCustomModifiers == null)
					{
						parameterOptionalCustomModifiers = new Type[parameterTypes.Length][];
						parameterRequiredCustomModifiers = new Type[parameterTypes.Length][];
					}
					ReadCustomModifiers(module, br, context, out parameterRequiredCustomModifiers[i], out parameterOptionalCustomModifiers[i]);
				}
				parameterTypes[i] = ReadParam(module, br, context);
			}
			return new MethodSignature(returnType, returnTypeOptionalCustomModifiers, returnTypeRequiredCustomModifiers,
				parameterTypes, parameterOptionalCustomModifiers, parameterRequiredCustomModifiers, callingConvention, genericParamCount);
		}

		internal static __StandAloneMethodSig ReadStandAloneMethodSig(ModuleReader module, ByteReader br, IGenericContext context)
		{
			CallingConventions callingConvention = 0;
			System.Runtime.InteropServices.CallingConvention unmanagedCallingConvention = 0;
			bool unmanaged;
			byte flags = br.ReadByte();
			switch (flags & 7)
			{
				case DEFAULT:
					callingConvention = CallingConventions.Standard;
					unmanaged = false;
					break;
				case 0x01:	// C
					unmanagedCallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl;
					unmanaged = true;
					break;
				case 0x02:	// STDCALL
					unmanagedCallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall;
					unmanaged = true;
					break;
				case 0x03:	// THISCALL
					unmanagedCallingConvention = System.Runtime.InteropServices.CallingConvention.ThisCall;
					unmanaged = true;
					break;
				case 0x04:	// FASTCALL
					unmanagedCallingConvention = System.Runtime.InteropServices.CallingConvention.FastCall;
					unmanaged = true;
					break;
				case VARARG:
					callingConvention = CallingConventions.VarArgs;
					unmanaged = false;
					break;
				default:
					throw new BadImageFormatException();
			}
			if ((flags & HASTHIS) != 0)
			{
				callingConvention |= CallingConventions.HasThis;
			}
			if ((flags & EXPLICITTHIS) != 0)
			{
				callingConvention |= CallingConventions.ExplicitThis;
			}
			if ((flags & GENERIC) != 0)
			{
				throw new BadImageFormatException();
			}
			int paramCount = br.ReadCompressedInt();
			SkipCustomModifiers(br);
			Type returnType = ReadRetType(module, br, context);
			List<Type> parameterTypes = new List<Type>();
			List<Type> optionalParameterTypes = new List<Type>();
			List<Type> curr = parameterTypes;
			for (int i = 0; i < paramCount; i++)
			{
				if (br.PeekByte() == SENTINEL)
				{
					br.ReadByte();
					curr = optionalParameterTypes;
				}
				SkipCustomModifiers(br);
				curr.Add(ReadParam(module, br, context));
			}
			return new __StandAloneMethodSig(unmanaged, unmanagedCallingConvention, callingConvention, returnType, parameterTypes.ToArray(), optionalParameterTypes.ToArray());
		}

		internal int GetParameterCount()
		{
			return parameterTypes.Length;
		}

		internal Type GetParameterType(int index)
		{
			return parameterTypes[index];
		}

		internal Type GetReturnType(IGenericBinder binder)
		{
			return returnType.BindTypeParameters(binder);
		}

		internal Type[] GetReturnTypeOptionalCustomModifiers(IGenericBinder binder)
		{
			return BindTypeParameters(binder, returnTypeOptionalCustomModifiers);
		}

		internal Type[] GetReturnTypeRequiredCustomModifiers(IGenericBinder binder)
		{
			return BindTypeParameters(binder, returnTypeRequiredCustomModifiers);
		}

		internal Type GetParameterType(IGenericBinder binder, int index)
		{
			return parameterTypes[index].BindTypeParameters(binder);
		}

		internal Type[] GetParameterOptionalCustomModifiers(IGenericBinder binder, int index)
		{
			if (parameterOptionalCustomModifiers == null)
			{
				return null;
			}
			return BindTypeParameters(binder, parameterOptionalCustomModifiers[index]);
		}

		internal Type[] GetParameterRequiredCustomModifiers(IGenericBinder binder, int index)
		{
			if (parameterRequiredCustomModifiers == null)
			{
				return null;
			}
			return BindTypeParameters(binder, parameterRequiredCustomModifiers[index]);
		}

		internal CallingConventions CallingConvention
		{
			get { return callingConvention; }
		}

		private sealed class Binder : IGenericBinder
		{
			private readonly Type declaringType;
			private readonly Type[] methodArgs;

			internal Binder(Type declaringType, Type[] methodArgs)
			{
				this.declaringType = declaringType;
				this.methodArgs = methodArgs;
			}

			public Type BindTypeParameter(Type type)
			{
				return declaringType.GetGenericTypeArgument(type.GenericParameterPosition);
			}

			public Type BindMethodParameter(Type type)
			{
				if (methodArgs == null)
				{
					return type;
				}
				return methodArgs[type.GenericParameterPosition];
			}
		}

		internal MethodSignature Bind(Type type, Type[] methodArgs)
		{
			Binder binder = new Binder(type, methodArgs);
			return new MethodSignature(returnType.BindTypeParameters(binder),
				BindTypeParameters(binder, returnTypeOptionalCustomModifiers),
				BindTypeParameters(binder, returnTypeRequiredCustomModifiers),
				BindTypeParameters(binder, parameterTypes),
				BindTypeParameters(binder, parameterOptionalCustomModifiers),
				BindTypeParameters(binder, parameterRequiredCustomModifiers),
				callingConvention, genericParamCount);
		}

		private sealed class Unbinder : IGenericBinder
		{
			public Type BindTypeParameter(Type type)
			{
				return type;
			}

			public Type BindMethodParameter(Type type)
			{
				return UnboundGenericMethodParameter.Make(type.GenericParameterPosition);
			}
		}

		internal static MethodSignature MakeFromBuilder(Type returnType, Type[] parameterTypes,
			Type[][] optionalCustomModifiers, Type[][] requiredCustomModifiers, CallingConventions callingConvention, int genericParamCount)
		{
			Type[] returnTypeOptionalCustomModifiers = UnpackReturnTypeModifiers(optionalCustomModifiers);
			Type[] returnTypeRequiredCustomModifiers = UnpackReturnTypeModifiers(requiredCustomModifiers);
			Type[][] parameterOptionalCustomModifiers = UnpackParameterModifiers(optionalCustomModifiers);
			Type[][] parameterRequiredCustomModifiers = UnpackParameterModifiers(requiredCustomModifiers);

			if (genericParamCount > 0)
			{
				Unbinder unbinder = new Unbinder();
				returnType = returnType.BindTypeParameters(unbinder);
				Type.InplaceBindTypeParameters(unbinder, returnTypeOptionalCustomModifiers);
				Type.InplaceBindTypeParameters(unbinder, returnTypeRequiredCustomModifiers);
				parameterTypes = BindTypeParameters(unbinder, parameterTypes);
				parameterOptionalCustomModifiers = BindTypeParameters(unbinder, parameterOptionalCustomModifiers);
				parameterRequiredCustomModifiers = BindTypeParameters(unbinder, parameterRequiredCustomModifiers);
			}

			return new MethodSignature(returnType, returnTypeOptionalCustomModifiers, returnTypeRequiredCustomModifiers,
					parameterTypes, parameterOptionalCustomModifiers, parameterRequiredCustomModifiers, callingConvention, genericParamCount);
		}

		private static Type[] UnpackReturnTypeModifiers(Type[][] types)
		{
			if (types == null)
			{
				return Type.EmptyTypes;
			}
			return types[types.Length - 1];
		}

		private static Type[][] UnpackParameterModifiers(Type[][] types)
		{
			if (types == null)
			{
				return null;
			}
			Type[][] unpacked = new Type[types.Length - 1][];
			Array.Copy(types, unpacked, unpacked.Length);
			return unpacked;
		}

		internal bool MatchParameterTypes(Type[] types)
		{
			if (types == parameterTypes)
			{
				return true;
			}
			if (types == null)
			{
				return parameterTypes.Length == 0;
			}
			if (parameterTypes == null)
			{
				return types.Length == 0;
			}
			if (types.Length == parameterTypes.Length)
			{
				for (int i = 0; i < types.Length; i++)
				{
					if (!Util.TypeEquals(types[i], parameterTypes[i]))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		internal override void WriteSig(ModuleBuilder module, ByteBuffer bb)
		{
			WriteSigImpl(module, bb, parameterTypes.Length);
		}

		internal void WriteMethodRefSig(ModuleBuilder module, ByteBuffer bb, Type[] optionalParameterTypes)
		{
			WriteSigImpl(module, bb, parameterTypes.Length + optionalParameterTypes.Length);
			if (optionalParameterTypes.Length > 0)
			{
				bb.Write(SENTINEL);
				foreach (Type type in optionalParameterTypes)
				{
					WriteType(module, bb, type);
				}
			}
		}

		private void WriteSigImpl(ModuleBuilder module, ByteBuffer bb, int parameterCount)
		{
			byte first;
			if ((callingConvention & CallingConventions.Any) == CallingConventions.VarArgs)
			{
				Debug.Assert(genericParamCount == 0);
				first = VARARG;
			}
			else if (genericParamCount > 0)
			{
				first = GENERIC;
			}
			else
			{
				first = DEFAULT;
			}
			if ((callingConvention & CallingConventions.HasThis) != 0)
			{
				first |= HASTHIS;
			}
			if ((callingConvention & CallingConventions.ExplicitThis) != 0)
			{
				first |= EXPLICITTHIS;
			}
			bb.Write(first);
			if (genericParamCount > 0)
			{
				bb.WriteCompressedInt(genericParamCount);
			}
			bb.WriteCompressedInt(parameterCount);
			// RetType
			WriteCustomModifiers(module, bb, ELEMENT_TYPE_CMOD_OPT, returnTypeOptionalCustomModifiers);
			WriteCustomModifiers(module, bb, ELEMENT_TYPE_CMOD_REQD, returnTypeRequiredCustomModifiers);
			WriteType(module, bb, returnType ?? module.universe.System_Void);
			// Param
			for (int i = 0; i < parameterTypes.Length; i++)
			{
				if (parameterOptionalCustomModifiers != null)
				{
					WriteCustomModifiers(module, bb, ELEMENT_TYPE_CMOD_OPT, parameterOptionalCustomModifiers[i]);
				}
				if (parameterRequiredCustomModifiers != null)
				{
					WriteCustomModifiers(module, bb, ELEMENT_TYPE_CMOD_REQD, parameterRequiredCustomModifiers[i]);
				}
				WriteType(module, bb, parameterTypes[i]);
			}
		}
	}
}
