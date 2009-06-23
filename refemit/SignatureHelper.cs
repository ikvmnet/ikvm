/*
  Copyright (C) 2008 Jeroen Frijters

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
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using IKVM.Reflection.Emit.Writer;

namespace IKVM.Reflection.Emit
{
	public class SignatureHelper
	{
		internal const byte DEFAULT = 0x00;
		internal const byte GENERIC = 0x10;
		internal const byte HASTHIS = 0x20;
		internal const byte FIELD = 0x06;
		internal const byte PROPERTY = 0x08;
		internal const byte ELEMENT_TYPE_VOID = 0x01;
		internal const byte ELEMENT_TYPE_BOOLEAN = 0x02;
		internal const byte ELEMENT_TYPE_CHAR = 0x03;
		internal const byte ELEMENT_TYPE_I1 = 0x04;
		internal const byte ELEMENT_TYPE_U1 = 0x05;
		internal const byte ELEMENT_TYPE_I2 = 0x06;
		internal const byte ELEMENT_TYPE_U2 = 0x07;
		internal const byte ELEMENT_TYPE_I4 = 0x08;
		internal const byte ELEMENT_TYPE_U4 = 0x09;
		internal const byte ELEMENT_TYPE_I8 = 0x0a;
		internal const byte ELEMENT_TYPE_U8 = 0x0b;
		internal const byte ELEMENT_TYPE_R4 = 0x0c;
		internal const byte ELEMENT_TYPE_R8 = 0x0d;
		internal const byte ELEMENT_TYPE_STRING = 0x0e;
		internal const byte ELEMENT_TYPE_PTR = 0x0f;
		internal const byte ELEMENT_TYPE_BYREF = 0x10;
		internal const byte ELEMENT_TYPE_VALUETYPE = 0x11;
		internal const byte ELEMENT_TYPE_CLASS = 0x12;
		internal const byte ELEMENT_TYPE_VAR = 0x13;
		internal const byte ELEMENT_TYPE_ARRAY = 0x14;
		internal const byte ELEMENT_TYPE_GENERICINST = 0x15;
		internal const byte ELEMENT_TYPE_TYPEDBYREF = 0x16;
		internal const byte ELEMENT_TYPE_I = 0x18;
		internal const byte ELEMENT_TYPE_U = 0x19;
		internal const byte ELEMENT_TYPE_OBJECT = 0x1c;
		internal const byte ELEMENT_TYPE_SZARRAY = 0x1d;
		internal const byte ELEMENT_TYPE_MVAR = 0x1e;
		internal const byte ELEMENT_TYPE_CMOD_REQD = 0x1f;
		internal const byte ELEMENT_TYPE_CMOD_OPT = 0x020;

		internal static void WriteType(ModuleBuilder moduleBuilder, ByteBuffer bb, Type type)
		{
			while (type.HasElementType)
			{
				if (type.IsArray)
				{
					// we look at the Type.Name property, because for arrays of generic parameters (T[]) the FullName returns null
					if (type.Name.EndsWith("[]", StringComparison.Ordinal))
					{
						bb.Write(ELEMENT_TYPE_SZARRAY);
					}
					else
					{
						int rank = type.GetArrayRank();
						bb.Write(ELEMENT_TYPE_ARRAY);
						WriteType(moduleBuilder, bb, type.GetElementType());
						bb.WriteCompressedInt(rank);
						// since a Type doesn't contain the lower/upper bounds
						// (they act like a custom modifier, so they are part of the signature, but not of the Type),
						// we set them to the C# compatible values and hope for the best
						bb.WriteCompressedInt(0);	// boundsCount
						bb.WriteCompressedInt(rank);	// loCount
						for (int i = 0; i < rank; i++)
						{
							bb.WriteCompressedInt(0);
						}
						return;
					}
				}
				else if (type.IsByRef)
				{
					bb.Write(ELEMENT_TYPE_BYREF);
				}
				else if (type.IsPointer)
				{
					bb.Write(ELEMENT_TYPE_PTR);
				}
				type = type.GetElementType();
			}
			if (type == typeof(void))
			{
				bb.Write(ELEMENT_TYPE_VOID);
			}
			else if (type == typeof(bool))
			{
				bb.Write(ELEMENT_TYPE_BOOLEAN);
			}
			else if (type == typeof(char))
			{
				bb.Write(ELEMENT_TYPE_CHAR);
			}
			else if (type == typeof(sbyte))
			{
				bb.Write(ELEMENT_TYPE_I1);
			}
			else if (type == typeof(byte))
			{
				bb.Write(ELEMENT_TYPE_U1);
			}
			else if (type == typeof(short))
			{
				bb.Write(ELEMENT_TYPE_I2);
			}
			else if (type == typeof(ushort))
			{
				bb.Write(ELEMENT_TYPE_U2);
			}
			else if (type == typeof(int))
			{
				bb.Write(ELEMENT_TYPE_I4);
			}
			else if (type == typeof(uint))
			{
				bb.Write(ELEMENT_TYPE_U4);
			}
			else if (type == typeof(long))
			{
				bb.Write(ELEMENT_TYPE_I8);
			}
			else if (type == typeof(ulong))
			{
				bb.Write(ELEMENT_TYPE_U8);
			}
			else if (type == typeof(float))
			{
				bb.Write(ELEMENT_TYPE_R4);
			}
			else if (type == typeof(double))
			{
				bb.Write(ELEMENT_TYPE_R8);
			}
			else if (type == typeof(string))
			{
				bb.Write(ELEMENT_TYPE_STRING);
			}
			else if (type == typeof(IntPtr))
			{
				bb.Write(ELEMENT_TYPE_I);
			}
			else if (type == typeof(UIntPtr))
			{
				bb.Write(ELEMENT_TYPE_U);
			}
			else if (type == typeof(TypedReference))
			{
				bb.Write(ELEMENT_TYPE_TYPEDBYREF);
			}
			else if (type == typeof(object))
			{
				bb.Write(ELEMENT_TYPE_OBJECT);
			}
			else if (type.IsGenericParameter)
			{
				if (type.DeclaringMethod != null)
				{
					bb.Write(ELEMENT_TYPE_MVAR);
				}
				else
				{
					bb.Write(ELEMENT_TYPE_VAR);
				}
				bb.WriteCompressedInt(type.GenericParameterPosition);
			}
			else if (type.IsGenericType && !type.IsGenericTypeDefinition)
			{
				WriteGenericSignature(moduleBuilder, bb, type.GetGenericTypeDefinition(), type.GetGenericArguments());
			}
			else if (!type.IsPrimitive)
			{
				if (type.IsValueType)
				{
					bb.Write(ELEMENT_TYPE_VALUETYPE);
				}
				else
				{
					bb.Write(ELEMENT_TYPE_CLASS);
				}
				bb.WriteTypeDefOrRefEncoded(moduleBuilder.GetTypeToken(type).Token);
			}
			else
			{
				throw new NotImplementedException(type.FullName);
			}
		}

		private static void WriteCustomModifiers(ModuleBuilder moduleBuilder, ByteBuffer bb, byte mod, Type[][] modifiers, int modifiersIndex)
		{
			if (modifiers != null && modifiersIndex < modifiers.Length)
			{
				WriteCustomModifiers(moduleBuilder, bb, mod, modifiers[modifiersIndex]);
			}
		}

		internal static void WriteCustomModifiers(ModuleBuilder moduleBuilder, ByteBuffer bb, byte mod, Type[] modifiers)
		{
			if (modifiers != null)
			{
				foreach (Type type in modifiers)
				{
					bb.Write(mod);
					bb.WriteTypeDefOrRefEncoded(moduleBuilder.GetTypeToken(type).Token);
				}
			}
		}

		internal static void WriteFieldSig(ModuleBuilder moduleBuilder, ByteBuffer bb, Type fieldType, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers)
		{
			bb.Write(FIELD);
			WriteCustomModifiers(moduleBuilder, bb, ELEMENT_TYPE_CMOD_REQD, requiredCustomModifiers);
			WriteCustomModifiers(moduleBuilder, bb, ELEMENT_TYPE_CMOD_OPT, optionalCustomModifiers);
			WriteType(moduleBuilder, bb, fieldType);
		}

		internal static void WriteMethodSig(ModuleBuilder moduleBuilder, ByteBuffer bb, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers, int genericParamCount)
		{
			if ((callingConvention & ~CallingConventions.HasThis) != CallingConventions.Standard)
			{
				throw new NotImplementedException();
			}
			byte first = DEFAULT;
			if ((callingConvention & CallingConventions.HasThis) != 0)
			{
				first |= HASTHIS;
			}
			if (genericParamCount > 0)
			{
				first |= GENERIC;
			}
			parameterTypes = parameterTypes ?? Type.EmptyTypes;
			bb.Write(first);
			if (genericParamCount > 0)
			{
				bb.WriteCompressedInt(genericParamCount);
			}
			bb.WriteCompressedInt(parameterTypes.Length);
			// RetType
			WriteCustomModifiers(moduleBuilder, bb, ELEMENT_TYPE_CMOD_REQD, requiredCustomModifiers, parameterTypes.Length);
			WriteCustomModifiers(moduleBuilder, bb, ELEMENT_TYPE_CMOD_OPT, optionalCustomModifiers, parameterTypes.Length);
			WriteType(moduleBuilder, bb, returnType ?? typeof(void));
			// Param
			for (int i = 0; i < parameterTypes.Length; i++)
			{
				WriteCustomModifiers(moduleBuilder, bb, ELEMENT_TYPE_CMOD_REQD, requiredCustomModifiers, i);
				WriteCustomModifiers(moduleBuilder, bb, ELEMENT_TYPE_CMOD_OPT, optionalCustomModifiers, i);
				WriteType(moduleBuilder, bb, parameterTypes[i]);
			}
		}

		private static MethodBase GetMethodOnTypeDefinition(MethodBase method)
		{
			Type type = method.DeclaringType;
			if (type != null && type.IsGenericType && !type.IsGenericTypeDefinition)
			{
				IMethodInstance methodInstance = method as IMethodInstance;
				if (methodInstance != null)
				{
					method = methodInstance.GetMethodOnTypeDefinition();
				}
				else
				{
					// this trick allows us to go from the method on the generic type instance, to the equivalent method on the generic type definition
					method = method.Module.ResolveMethod(method.MetadataToken);
				}
			}
			return method;
		}

		internal static void WriteMethodSig(ModuleBuilder moduleBuilder, ByteBuffer bb, MethodBase methodOnTypeInstance)
		{
			Debug.Assert(!methodOnTypeInstance.IsGenericMethod || methodOnTypeInstance.IsGenericMethodDefinition);
			MethodBase method = GetMethodOnTypeDefinition(methodOnTypeInstance);
			ParameterInfo returnParameter = null;
			ParameterInfo[] parameters = method.GetParameters();
			if (method is MethodInfo)
			{
				returnParameter = ((MethodInfo)method).ReturnParameter;
			}
			byte first = DEFAULT;
			if (!method.IsStatic)
			{
				first |= HASTHIS;
			}
			if (method.IsGenericMethod)
			{
				first |= GENERIC;
			}
			bb.Write(first);
			if (method.IsGenericMethod)
			{
				bb.WriteCompressedInt(method.GetGenericArguments().Length);
			}
			bb.WriteCompressedInt(parameters.Length);
			// RetType
			if (returnParameter == null)
			{
				WriteType(moduleBuilder, bb, typeof(void));
			}
			else
			{
				WriteCustomModifiers(moduleBuilder, bb, ELEMENT_TYPE_CMOD_REQD, returnParameter.GetRequiredCustomModifiers());
				WriteCustomModifiers(moduleBuilder, bb, ELEMENT_TYPE_CMOD_OPT, returnParameter.GetOptionalCustomModifiers());
				WriteType(moduleBuilder, bb, returnParameter.ParameterType);
			}
			// Param
			for (int i = 0; i < parameters.Length; i++)
			{
				WriteCustomModifiers(moduleBuilder, bb, ELEMENT_TYPE_CMOD_REQD, parameters[i].GetRequiredCustomModifiers());
				WriteCustomModifiers(moduleBuilder, bb, ELEMENT_TYPE_CMOD_OPT, parameters[i].GetOptionalCustomModifiers());
				WriteType(moduleBuilder, bb, parameters[i].ParameterType);
			}
		}

		private static bool IsGenericParameter(Type type)
		{
			while (type.HasElementType)
			{
				type = type.GetElementType();
			}
			return type.IsGenericParameter;
		}

		internal static void WriteGenericSignature(ModuleBuilder moduleBuilder, ByteBuffer bb, Type type, Type[] typeArguments)
		{
			Debug.Assert(type.IsGenericTypeDefinition);
			bb.Write(ELEMENT_TYPE_GENERICINST);
			WriteType(moduleBuilder, bb, type);
			bb.WriteCompressedInt(typeArguments.Length);
			foreach (Type t in typeArguments)
			{
				WriteType(moduleBuilder, bb, t);
			}
		}
	}
}
