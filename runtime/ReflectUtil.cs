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
using System.Reflection;
#if IKVM_REF_EMIT
using IKVM.Reflection.Emit;
#else
using System.Reflection.Emit;
#endif

namespace IKVM.Internal
{
	static class ReflectUtil
	{
		private static readonly bool clr_v4 = Environment.Version.Major >= 4;

		internal static bool IsSameAssembly(Type type1, Type type2)
		{
#if IKVM_REF_EMIT && !NET_4_0
			return IkvmAssembly.GetAssembly(type1) == IkvmAssembly.GetAssembly(type2);
#else
			return type1.Assembly.Equals(type2.Assembly);
#endif
		}

#if IKVM_REF_EMIT && !NET_4_0
		internal static bool IsFromAssembly(Type type, IkvmAssembly assembly)
		{
			return IkvmAssembly.GetAssembly(type) == assembly;
		}
#else
		internal static bool IsFromAssembly(Type type, Assembly assembly)
		{
			return type.Assembly.Equals(assembly);
		}
#endif

#if IKVM_REF_EMIT && !NET_4_0
		internal static IkvmAssembly GetAssembly(Type type)
		{
			return IkvmAssembly.GetAssembly(type);
		}
#else
		internal static Assembly GetAssembly(Type type)
		{
			return type.Assembly;
		}
#endif

		internal static bool IsDynamicAssembly(Assembly asm)
		{
#if NET_4_0
			return asm.IsDynamic();
#else
			if (clr_v4)
			{
				// on .NET 4.0 dynamic assemblies have a non-AssemblyBuilder derived peer, so we have to call IsDynamic
				return (bool)typeof(Assembly).InvokeMember("IsDynamic", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, null, asm, null);
			}
			return asm is AssemblyBuilder;
#endif
		}
	}
}
