/*
  Copyright (C) 2008-2012 Jeroen Frijters

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
#if STATIC_COMPILER || STUB_GENERATOR
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace IKVM.Internal
{
	static class ReflectUtil
	{
#if !NET_4_0 && !STATIC_COMPILER && !STUB_GENERATOR
		private static readonly bool clr_v4 = Environment.Version.Major >= 4;
		private static Predicate<Assembly> get_IsDynamic;
#endif

		internal static bool IsSameAssembly(Type type1, Type type2)
		{
			return type1.Assembly.Equals(type2.Assembly);
		}

		internal static bool IsFromAssembly(Type type, Assembly assembly)
		{
			return type.Assembly.Equals(assembly);
		}

		internal static Assembly GetAssembly(Type type)
		{
			return type.Assembly;
		}

		internal static bool IsDynamicAssembly(Assembly asm)
		{
#if STATIC_COMPILER || STUB_GENERATOR
			return false;
#elif NET_4_0
			return asm.IsDynamic;
#else
			if (clr_v4)
			{
				// on .NET 4.0 dynamic assemblies have a non-AssemblyBuilder derived peer, so we have to call IsDynamic
				if (get_IsDynamic == null)
				{
					get_IsDynamic = (Predicate<Assembly>)Delegate.CreateDelegate(typeof(Predicate<Assembly>), typeof(Assembly).GetMethod("get_IsDynamic"));
				}
				return get_IsDynamic(asm);
			}
			return asm is AssemblyBuilder;
#endif
		}

		internal static bool IsReflectionOnly(Type type)
		{
			while (type.HasElementType)
			{
				type = type.GetElementType();
			}
			Assembly asm = type.Assembly;
			if (asm != null && asm.ReflectionOnly)
			{
				return true;
			}
			if (!type.IsGenericType || type.IsGenericTypeDefinition)
			{
				return false;
			}
			// we have a generic type instantiation, it might have ReflectionOnly type arguments
			foreach (Type arg in type.GetGenericArguments())
			{
				if (IsReflectionOnly(arg))
				{
					return true;
				}
			}
			return false;
		}

		internal static bool ContainsTypeBuilder(Type type)
		{
			while (type.HasElementType)
			{
				type = type.GetElementType();
			}
			if (!type.IsGenericType || type.IsGenericTypeDefinition)
			{
				return type is TypeBuilder;
			}
			foreach (Type arg in type.GetGenericArguments())
			{
				if (ContainsTypeBuilder(arg))
				{
					return true;
				}
			}
			return type.GetGenericTypeDefinition() is TypeBuilder;
		}

		internal static bool IsVector(Type type)
		{
#if STATIC_COMPILER || STUB_GENERATOR
			return type.__IsVector;
#else
			// there's no API to distinguish an array of rank 1 from a vector,
			// so we check if the type name ends in [], which indicates it's a vector
			// (non-vectors will have [*] or [,]).
			return type.IsArray && type.Name.EndsWith("[]");
#endif
		}

		internal static bool IsDynamicMethod(MethodInfo method)
		{
			// there's no way to distinguish a baked DynamicMethod from a RuntimeMethodInfo and
			// on top of that Mono behaves completely different from .NET
			try
			{
				// on Mono 2.10 the MetadataToken property returns zero instead of throwing InvalidOperationException
				return method.MetadataToken == 0;
			}
			catch (InvalidOperationException)
			{
				return true;
			}
		}

		internal static MethodBuilder DefineTypeInitializer(TypeBuilder typeBuilder, ClassLoaderWrapper loader)
		{
			MethodAttributes attr = MethodAttributes.Static | MethodAttributes.RTSpecialName | MethodAttributes.SpecialName;
			if (typeBuilder.IsInterface && loader.WorkaroundInterfacePrivateMethods)
			{
				// LAMESPEC the ECMA spec says (part. I, sect. 8.5.3.2) that all interface members must be public, so we make
				// the class constructor public.
				// NOTE it turns out that on .NET 2.0 this isn't necessary anymore (neither Ref.Emit nor the CLR verifier complain about it),
				// but the C# compiler still considers interfaces with non-public methods to be invalid, so to keep interop with C# we have
				// to keep making the .cctor method public.
				attr |= MethodAttributes.Public;
			}
			else
			{
				attr |= MethodAttributes.Private;
			}
			return typeBuilder.DefineMethod(ConstructorInfo.TypeConstructorName, attr, null, Type.EmptyTypes);
		}

		internal static bool MatchNameAndPublicKeyToken(AssemblyName name1, AssemblyName name2)
		{
			return name1.Name.Equals(name2.Name, StringComparison.OrdinalIgnoreCase)
				&& CompareKeys(name1.GetPublicKeyToken(), name2.GetPublicKeyToken());
		}

		private static bool CompareKeys(byte[] b1, byte[] b2)
		{
			int len1 = b1 == null ? 0 : b1.Length;
			int len2 = b2 == null ? 0 : b2.Length;
			if (len1 != len2)
			{
				return false;
			}
			for (int i = 0; i < len1; i++)
			{
				if (b1[i] != b2[i])
				{
					return false;
				}
			}
			return true;
		}

		internal static bool IsConstructor(MethodBase method)
		{
			return method.IsSpecialName && method.Name == ConstructorInfo.ConstructorName;
		}

		internal static MethodBuilder DefineConstructor(TypeBuilder tb, MethodAttributes attribs, Type[] parameterTypes)
		{
			return tb.DefineMethod(ConstructorInfo.ConstructorName, attribs | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, null, parameterTypes);
		}

		internal static bool CanOwnDynamicMethod(Type type)
		{
			return type != null
				&& !type.IsInterface
				&& !type.HasElementType
				&& !type.IsGenericTypeDefinition
				&& !type.IsGenericParameter;
		}

		internal static bool MatchParameterInfos(ParameterInfo p1, ParameterInfo p2)
		{
			if (p1.ParameterType != p2.ParameterType)
			{
				return false;
			}
			if (!MatchTypes(p1.GetOptionalCustomModifiers(), p2.GetOptionalCustomModifiers()))
			{
				return false;
			}
			if (!MatchTypes(p1.GetRequiredCustomModifiers(), p2.GetRequiredCustomModifiers()))
			{
				return false;
			}
			return true;
		}

		private static bool MatchTypes(Type[] t1, Type[] t2)
		{
			if (t1.Length == t2.Length)
			{
				for (int i = 0; i < t1.Length; i++)
				{
					if (t1[i] != t2[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

#if STATIC_COMPILER
		internal static Type GetMissingType(Type type)
		{
			while (type.HasElementType)
			{
				type = type.GetElementType();
			}
			if (type.__IsMissing)
			{
				return type;
			}
			else if (type.__ContainsMissingType)
			{
				if (type.IsGenericType)
				{
					foreach (Type arg in type.GetGenericArguments())
					{
						Type t1 = GetMissingType(arg);
						if (t1.__IsMissing)
						{
							return t1;
						}
					}
				}
				throw new NotImplementedException(type.FullName);
			}
			else
			{
				return type;
			}
		}
#endif
	}
}
