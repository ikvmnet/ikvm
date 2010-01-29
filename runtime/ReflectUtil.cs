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
#if NET_4_0
			return asm.IsDynamic;
#elif STATIC_COMPILER || STUB_GENERATOR
			return false;
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
	}
}
