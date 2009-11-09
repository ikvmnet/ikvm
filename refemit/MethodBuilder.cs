/*
  Copyright (C) 2008, 2009 Jeroen Frijters

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
using System.Reflection;
using System.IO;
using System.Diagnostics;
using IKVM.Reflection.Emit.Writer;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Diagnostics.SymbolStore;

namespace IKVM.Reflection.Emit
{
	public sealed class MethodBuilder : MethodInfo
	{
		private readonly TypeBuilder typeBuilder;
		private readonly string name;
		private readonly int nameIndex;
		private readonly int pseudoToken;
		private int signature;
		private Type returnType;
		private Type[] parameterTypes;
		private Type[][] requiredCustomModifiers;	// last element is for the return type
		private Type[][] optionalCustomModifiers;
		private MethodAttributes attributes;
		private MethodImplAttributes implFlags;
		private ILGenerator ilgen;
		private int rva;
		private readonly CallingConventions callingConvention;
		private List<ParameterBuilder> parameters;
		private GenericTypeParameterBuilder[] gtpb;
		private List<CustomAttributeBuilder> declarativeSecurity;

		internal MethodBuilder(TypeBuilder typeBuilder, string name, MethodAttributes attributes, CallingConventions callingConvention)
		{
			this.typeBuilder = typeBuilder;
			this.name = name;
			this.pseudoToken = typeBuilder.ModuleBuilder.AllocPseudoToken();
			this.nameIndex = typeBuilder.ModuleBuilder.Strings.Add(name);
			this.attributes = attributes;
			if ((attributes & MethodAttributes.Static) == 0)
			{
				callingConvention |= CallingConventions.HasThis;
			}
			this.callingConvention = callingConvention;
		}

		internal static Type[] Copy(Type[] array)
		{
			if (array == null || array.Length == 0)
			{
				return Type.EmptyTypes;
			}
			Type[] newArray = new Type[array.Length];
			Array.Copy(array, newArray, array.Length);
			return newArray;
		}

		private static Type[][] PackCustomModifiers(Type[] returnTypeCustomModifiers, Type[][] parameterTypeCustomModifiers, int parameterCount)
		{
			if (returnTypeCustomModifiers == null && parameterTypeCustomModifiers == null)
			{
				return null;
			}
			Type[][] newArray = new Type[parameterCount + 1][];
			newArray[parameterCount] = Copy(returnTypeCustomModifiers);
			if (parameterTypeCustomModifiers != null)
			{
				for (int i = 0; i < parameterCount; i++)
				{
					newArray[i] = Copy(parameterTypeCustomModifiers[i]);
				}
			}
			else
			{
				for (int i = 0; i < parameterCount; i++)
				{
					newArray[i] = Type.EmptyTypes;
				}
			}
			return newArray;
		}

		public ILGenerator GetILGenerator()
		{
			if (ilgen == null)
			{
				ilgen = new ILGenerator(typeBuilder.ModuleBuilder);
			}
			return ilgen;
		}

		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
		}

		private void SetDllImportPseudoCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (customBuilder.IsBlob)
			{
				throw new NotImplementedException();
			}
			const short NoMangle = 0x0001;
			const short CharSetMask = 0x0006;
			const short CharSetNotSpec = 0x0000;
			const short CharSetAnsi = 0x0002;
			const short CharSetUnicode = 0x0004;
			const short CharSetAuto = 0x0006;
			const short SupportsLastError = 0x0040;
			const short CallConvMask = 0x0700;
			const short CallConvWinapi = 0x0100;
			const short CallConvCdecl = 0x0200;
			const short CallConvStdcall = 0x0300;
			const short CallConvThiscall = 0x0400;
			const short CallConvFastcall = 0x0500;
			// non-standard flags
			const short BestFitOn = 0x0010;
			const short BestFitOff = 0x0020;
			const short CharMapErrorOn = 0x1000;
			const short CharMapErrorOff = 0x2000;
			int name = this.nameIndex;
			short flags = CharSetNotSpec | CallConvWinapi;
			bool? bestFitMapping = (bool?)customBuilder.GetFieldValue("BestFitMapping");
			if (bestFitMapping.HasValue)
			{
				flags |= bestFitMapping.Value ? BestFitOn : BestFitOff;
			}
			bool? throwOnUnmappableChar = (bool?)customBuilder.GetFieldValue("ThrowOnUnmappableChar");
			if (throwOnUnmappableChar.HasValue)
			{
				flags |= throwOnUnmappableChar.Value ? CharMapErrorOn : CharMapErrorOff;
			}
			CallingConvention? callingConvention = (CallingConvention?)customBuilder.GetFieldValue("CallingConvention");
			if (callingConvention.HasValue)
			{
				flags &= ~CallConvMask;
				switch (callingConvention.Value)
				{
					case System.Runtime.InteropServices.CallingConvention.Cdecl:
						flags |= CallConvCdecl;
						break;
					case System.Runtime.InteropServices.CallingConvention.FastCall:
						flags |= CallConvFastcall;
						break;
					case System.Runtime.InteropServices.CallingConvention.StdCall:
						flags |= CallConvStdcall;
						break;
					case System.Runtime.InteropServices.CallingConvention.ThisCall:
						flags |= CallConvThiscall;
						break;
					case System.Runtime.InteropServices.CallingConvention.Winapi:
						flags |= CallConvWinapi;
						break;
				}
			}
			CharSet? charSet = (CharSet?)customBuilder.GetFieldValue("CharSet");
			if (charSet.HasValue)
			{
				flags &= ~CharSetMask;
				switch (charSet.Value)
				{
					case CharSet.Ansi:
					case CharSet.None:
						flags |= CharSetAnsi;
						break;
					case CharSet.Auto:
						flags |= CharSetAuto;
						break;
					case CharSet.Unicode:
						flags |= CharSetUnicode;
						break;
				}
			}
			string entryPoint = (string)customBuilder.GetFieldValue("EntryPoint");
			if (entryPoint != null)
			{
				name = this.ModuleBuilder.Strings.Add(entryPoint);
			}
			bool? exactSpelling = (bool?)customBuilder.GetFieldValue("ExactSpelling");
			if (exactSpelling.HasValue && exactSpelling.Value)
			{
				flags |= NoMangle;
			}
			bool? preserveSig = (bool?)customBuilder.GetFieldValue("PreserveSig");
			if (!preserveSig.HasValue || preserveSig.Value)
			{
				implFlags |= MethodImplAttributes.PreserveSig;
			}
			bool? setLastError = (bool?)customBuilder.GetFieldValue("SetLastError");
			if (setLastError.HasValue && setLastError.Value)
			{
				flags |= SupportsLastError;
			}
			TableHeap.ImplMapTable.Record rec = new TableHeap.ImplMapTable.Record();
			rec.MappingFlags = flags;
			rec.MemberForwarded = pseudoToken;
			rec.ImportName = name;
			rec.ImportScope = this.ModuleBuilder.Tables.ModuleRef.Add(this.ModuleBuilder.Strings.Add((string)customBuilder.GetConstructorArgument(0)));
			this.ModuleBuilder.Tables.ImplMap.AddRecord(rec);
		}

		private void SetMethodImplAttribute(CustomAttributeBuilder customBuilder)
		{
			MethodImplOptions opt;
			switch (customBuilder.Constructor.GetParameters().Length)
			{
				case 0:
					opt = 0;
					break;
				case 1:
					{
						object val = customBuilder.GetConstructorArgument(0);
						if (val is short)
						{
							opt = (MethodImplOptions)(short)val;
						}
						else
						{
							opt = (MethodImplOptions)val;
						}
						break;
					}
				default:
					throw new NotSupportedException();
			}
			MethodCodeType? type = (MethodCodeType?)customBuilder.GetFieldValue("MethodCodeType");
			implFlags = (MethodImplAttributes)opt;
			if (type.HasValue)
			{
				implFlags |= (MethodImplAttributes)type;
			}
		}

		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			Type type = customBuilder.Constructor.DeclaringType;
			if (type == typeof(DllImportAttribute))
			{
				SetDllImportPseudoCustomAttribute(customBuilder);
			}
			else if (type == typeof(MethodImplAttribute))
			{
				SetMethodImplAttribute(customBuilder);
			}
			else if (type == typeof(PreserveSigAttribute))
			{
				implFlags |= MethodImplAttributes.PreserveSig;
			}
			else if (type == typeof(SpecialNameAttribute))
			{
				attributes |= MethodAttributes.SpecialName;
			}
			else
			{
				if (type == typeof(System.Security.SuppressUnmanagedCodeSecurityAttribute))
				{
					attributes |= MethodAttributes.HasSecurity;
				}
				this.ModuleBuilder.SetCustomAttribute(pseudoToken, customBuilder);
			}
		}

		public void __AddDeclarativeSecurity(CustomAttributeBuilder customBuilder)
		{
			attributes |= MethodAttributes.HasSecurity;
			if (declarativeSecurity == null)
			{
				declarativeSecurity = new List<CustomAttributeBuilder>();
			}
			declarativeSecurity.Add(customBuilder);
		}

		public void AddDeclarativeSecurity(System.Security.Permissions.SecurityAction securityAction, System.Security.PermissionSet permissionSet)
		{
			this.ModuleBuilder.AddDeclaritiveSecurity(pseudoToken, securityAction, permissionSet);
			this.attributes |= MethodAttributes.HasSecurity;
		}

		public void SetImplementationFlags(MethodImplAttributes attributes)
		{
			implFlags = attributes;
		}

		public ParameterBuilder DefineParameter(int position, ParameterAttributes attributes, string strParamName)
		{
			if (parameters == null)
			{
				parameters = new List<ParameterBuilder>();
			}
			this.ModuleBuilder.Tables.Param.AddRow();
			ParameterBuilder pb = new ParameterBuilder(this.ModuleBuilder, position, attributes, strParamName);
			parameters.Add(pb);
			return pb;
		}

		public void SetSignature(Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
		{
			this.returnType = returnType ?? typeof(void);
			this.parameterTypes = Copy(parameterTypes);
			this.requiredCustomModifiers = PackCustomModifiers(returnTypeRequiredCustomModifiers, parameterTypeRequiredCustomModifiers, this.parameterTypes.Length);
			this.optionalCustomModifiers = PackCustomModifiers(returnTypeOptionalCustomModifiers, parameterTypeOptionalCustomModifiers, this.parameterTypes.Length);
		}

		public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names)
		{
			gtpb = new GenericTypeParameterBuilder[names.Length];
			for (int i = 0; i < names.Length; i++)
			{
				TableHeap.GenericParamTable.Record rec = new TableHeap.GenericParamTable.Record();
				rec.Number = (short)i;
				rec.Flags = 0;
				rec.Owner = pseudoToken;
				rec.Name = this.ModuleBuilder.Strings.Add(names[i]);
				gtpb[i] = new GenericTypeParameterBuilder(this.ModuleBuilder, names[i], null, this, this.ModuleBuilder.Tables.GenericParam.AddRecord(rec), i);
			}
			return (GenericTypeParameterBuilder[])gtpb.Clone();
		}

		public override MethodInfo MakeGenericMethod(params Type[] typeArguments)
		{
			return new GenericMethodInstance(this, typeArguments);
		}

		public override Type[] GetGenericArguments()
		{
			return gtpb == null ? Type.EmptyTypes : (Type[])gtpb.Clone();
		}

		public override MethodInfo GetBaseDefinition()
		{
			throw new NotSupportedException();
		}

		public override Type ReturnType
		{
			get { return returnType; }
		}

		public override ParameterInfo ReturnParameter
		{
			get { return new ParameterInfoImpl(this, -1); }
		}

		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get { throw new NotImplementedException(); }
		}

		public override MethodAttributes Attributes
		{
			get { return attributes; }
		}

		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return implFlags;
		}

		private class ParameterInfoImpl : ParameterInfo
		{
			private readonly MethodBuilder method;
			private readonly int parameter;

			internal ParameterInfoImpl(MethodBuilder method, int parameter)
			{
				this.method = method;
				this.parameter = parameter;
			}

			public override Type ParameterType
			{
				get
				{
					return parameter == -1 ? method.returnType : method.parameterTypes[parameter];
				}
			}

			public override Type[] GetOptionalCustomModifiers()
			{
				if (method.optionalCustomModifiers == null)
				{
					return Type.EmptyTypes;
				}
				int index = parameter == -1 ? method.optionalCustomModifiers.Length - 1 : parameter;
				return Copy(method.optionalCustomModifiers[index]);
			}

			public override Type[] GetRequiredCustomModifiers()
			{
				if (method.requiredCustomModifiers == null)
				{
					return Type.EmptyTypes;
				}
				int index = parameter == -1 ? method.requiredCustomModifiers.Length - 1 : parameter;
				return Copy(method.requiredCustomModifiers[index]);
			}
		}

		public override ParameterInfo[] GetParameters()
		{
			ParameterInfo[] parameters = new ParameterInfo[parameterTypes.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				parameters[i] = new ParameterInfoImpl(this, i);
			}
			return parameters;
		}

		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public override RuntimeMethodHandle MethodHandle
		{
			get { throw new NotImplementedException(); }
		}

		public override Type DeclaringType
		{
			get { return this.ModuleBuilder.IsModuleType(typeBuilder) ? null : typeBuilder; }
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public override string Name
		{
			get { return name; }
		}

		public override Type ReflectedType
		{
			get { return this.DeclaringType; }
		}

		public override CallingConventions CallingConvention
		{
			get { throw new NotImplementedException(); }
		}

		public override int MetadataToken
		{
			get { return pseudoToken; }
		}

		public override bool IsGenericMethod
		{
			get { return gtpb != null; }
		}

		public override bool IsGenericMethodDefinition
		{
			get { return gtpb != null; }
		}

#if NET_4_0
		public override Module Module
		{
			get { return typeBuilder.Module; }
		}
#endif

		internal void Bake()
		{
			ByteBuffer signature = new ByteBuffer(16);
			SignatureHelper.WriteMethodSig(this.ModuleBuilder, signature, callingConvention, returnType, parameterTypes, requiredCustomModifiers, optionalCustomModifiers, gtpb == null ? 0 : gtpb.Length);
			this.signature = this.ModuleBuilder.Blobs.Add(signature);

			if (ilgen != null)
			{
				if (this.ModuleBuilder.symbolWriter != null)
				{
					this.ModuleBuilder.symbolWriter.OpenMethod(new SymbolToken(-pseudoToken | 0x06000000));
				}
				rva = ilgen.WriteBody();
				if (this.ModuleBuilder.symbolWriter != null)
				{
					this.ModuleBuilder.symbolWriter.CloseMethod();
				}
				ilgen = null;
			}
			else
			{
				rva = -1;
			}

			if (declarativeSecurity != null)
			{
				this.ModuleBuilder.AddDeclarativeSecurity(pseudoToken, declarativeSecurity);
			}
		}

		internal ModuleBuilder ModuleBuilder
		{
			get { return typeBuilder.ModuleBuilder; }
		}

		internal void WriteMethodDefRecord(int baseRVA, MetadataWriter mw, ref int paramList)
		{
			if (rva != -1)
			{
				mw.Write(rva + baseRVA);
			}
			else
			{
				mw.Write(0);
			}
			mw.Write((short)implFlags);
			mw.Write((short)attributes);
			mw.WriteStringIndex(nameIndex);
			mw.WriteBlobIndex(signature);
			mw.WriteParam(paramList);
			if (parameters != null)
			{
				paramList += parameters.Count;
			}
		}

		internal void WriteParamRecords(MetadataWriter mw)
		{
			if (parameters != null)
			{
				foreach (ParameterBuilder pb in parameters)
				{
					pb.WriteParamRecord(mw);
				}
			}
		}

		internal void FixupToken(int token, ref int parameterToken)
		{
			typeBuilder.ModuleBuilder.RegisterTokenFixup(this.pseudoToken, token);
			if (parameters != null)
			{
				foreach (ParameterBuilder pb in parameters)
				{
					pb.FixupToken(parameterToken++);
				}
			}
		}

		internal bool MatchParameters(Type[] types)
		{
			if (types.Length == parameterTypes.Length)
			{
				for (int i = 0; i < types.Length; i++)
				{
					if (!types[i].Equals(parameterTypes[i]))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
	}

	interface IMethodInstance
	{
		MethodBase GetMethodOnTypeDefinition();
	}

	sealed class MethodInstance : MethodInfo, IMethodInstance
	{
		private readonly Type type;
		private readonly MethodInfo method;

		internal MethodInstance(Type type, MethodInfo method)
		{
			Debug.Assert(method.DeclaringType.IsGenericTypeDefinition);
			this.type = type;
			this.method = method;
		}

		public override bool Equals(object obj)
		{
			MethodInstance other = obj as MethodInstance;
			return other != null && other.type == type && other.method == method;
		}

		public override int GetHashCode()
		{
			return type.GetHashCode() * 23 + method.GetHashCode();
		}

		public override MethodAttributes Attributes
		{
			get { return method.Attributes; }
		}

		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			throw new NotImplementedException();
		}

		public override ParameterInfo[] GetParameters()
		{
			return ReplaceGenericParameters(type, method.GetParameters());
		}

		public override ParameterInfo ReturnParameter
		{
			get { return ReplaceGenericParameter(type, method.ReturnParameter); }
		}

		public override Type ReturnType
		{
			get { return FieldInstance.ReplaceGenericParameter(type, method.ReturnType); }
		}

		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public override RuntimeMethodHandle MethodHandle
		{
			get { throw new NotImplementedException(); }
		}

		public override Type DeclaringType
		{
			get { return type; }
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public override string Name
		{
			get { return method.Name; }
		}

		public override Type ReflectedType
		{
			get { return type; }
		}

		public override MethodInfo GetBaseDefinition()
		{
			throw new NotImplementedException();
		}

		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get { throw new NotImplementedException(); }
		}

		MethodBase IMethodInstance.GetMethodOnTypeDefinition()
		{
			return method;
		}

		private sealed class GenericInstanceParameterInfo : ParameterInfo
		{
			private readonly Type container;
			private readonly ParameterInfo parameterInfo;

			internal GenericInstanceParameterInfo(Type container, ParameterInfo parameterInfo)
			{
				this.container = container;
				this.parameterInfo = parameterInfo;
			}

			public override Type ParameterType
			{
				get { return FieldInstance.ReplaceGenericParameter(container, parameterInfo.ParameterType); }
			}

			public override Type[] GetOptionalCustomModifiers()
			{
				return FieldInstance.ReplaceGenericParameters(container, parameterInfo.GetOptionalCustomModifiers());
			}

			public override Type[] GetRequiredCustomModifiers()
			{
				return FieldInstance.ReplaceGenericParameters(container, parameterInfo.GetRequiredCustomModifiers());
			}
		}

		private static ParameterInfo ReplaceGenericParameter(Type type, ParameterInfo parameterInfo)
		{
			return new GenericInstanceParameterInfo(type, parameterInfo);
		}

		internal static ParameterInfo[] ReplaceGenericParameters(Type type, ParameterInfo[] parameterInfo)
		{
			ParameterInfo[] newArray = new ParameterInfo[parameterInfo.Length];
			for (int i = 0; i < newArray.Length; i++)
			{
				newArray[i] = ReplaceGenericParameter(type, parameterInfo[i]);
			}
			return newArray;
		}
	}

	sealed class GenericMethodInstance : MethodInfo
	{
		private readonly MethodBuilder method;
		private readonly Type[] args;

		internal GenericMethodInstance(MethodBuilder method, Type[] args)
		{
			this.method = method;
			this.args = args;
		}

		public override bool Equals(object obj)
		{
			GenericMethodInstance other = obj as GenericMethodInstance;
			if (other != null && other.method == method && other.args.Length == args.Length)
			{
				for (int i = 0; i < args.Length; i++)
				{
					if (!args[i].Equals(other.args[i]))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public override int GetHashCode()
		{
			int h = method.GetHashCode();
			foreach (Type type in args)
			{
				h = h * 27 + type.GetHashCode();
			}
			return h;
		}

		public override MethodInfo GetBaseDefinition()
		{
			throw new NotImplementedException();
		}

		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get { throw new NotImplementedException(); }
		}

		public override MethodAttributes Attributes
		{
			get { return method.Attributes; }
		}

		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			throw new NotImplementedException();
		}

		private sealed class GenericInstanceParameterInfo : ParameterInfo
		{
			private readonly GenericMethodInstance container;
			private readonly ParameterInfo parameterInfo;

			internal GenericInstanceParameterInfo(GenericMethodInstance container, ParameterInfo parameterInfo)
			{
				this.container = container;
				this.parameterInfo = parameterInfo;
			}

			public override Type ParameterType
			{
				get { return ReplaceGenericParameter(container, parameterInfo.ParameterType); }
			}

			public override Type[] GetOptionalCustomModifiers()
			{
				return ReplaceGenericParameters(container, parameterInfo.GetOptionalCustomModifiers());
			}

			public override Type[] GetRequiredCustomModifiers()
			{
				return ReplaceGenericParameters(container, parameterInfo.GetRequiredCustomModifiers());
			}
		}

		public override ParameterInfo[] GetParameters()
		{
			ParameterInfo[] param = method.GetParameters();
			for (int i = 0; i < param.Length; i++)
			{
				param[i] = new GenericInstanceParameterInfo(this, param[i]);
			}
			return param;
		}

		public override ParameterInfo ReturnParameter
		{
			get { return new GenericInstanceParameterInfo(this, method.ReturnParameter); }
		}

		public override Type ReturnType
		{
			get { return ReplaceGenericParameter(this, method.ReturnType); }
		}

		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public override RuntimeMethodHandle MethodHandle
		{
			get { throw new NotImplementedException(); }
		}

		public override Type DeclaringType
		{
			get { return method.DeclaringType; }
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public override string Name
		{
			get { return method.Name; }
		}

		public override Type ReflectedType
		{
			get { return this.DeclaringType; }
		}

		public override bool IsGenericMethod
		{
			get { return true; }
		}

		public override MethodInfo GetGenericMethodDefinition()
		{
			return method;
		}

		public override Type[] GetGenericArguments()
		{
			return (Type[])args.Clone();
		}

		private static Type ReplaceGenericParameter(GenericMethodInstance container, Type type)
		{
			if (type.IsGenericParameter && type.DeclaringMethod == container)
			{
				return container.args[type.GenericParameterPosition];
			}
			return type;
		}

		private static Type[] ReplaceGenericParameters(GenericMethodInstance container, Type[] types)
		{
			Type[] newarray = new Type[types.Length];
			for (int i = 0; i < newarray.Length; i++)
			{
				newarray[i] = ReplaceGenericParameter(container, types[i]);
			}
			return newarray;
		}
	}
}
