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
using System.Reflection;
using IKVM.Reflection.Emit.Impl;

namespace IKVM.Reflection.Emit
{
	public abstract class IkvmAssembly
	{
		private static readonly Dictionary<Assembly, IkvmAssembly> assemblies = new Dictionary<Assembly, IkvmAssembly>();

		internal IkvmAssembly() { }

		private class AssemblyWrapper : IkvmAssembly
		{
			private readonly Assembly asm;

			internal AssemblyWrapper(Assembly asm)
			{
				this.asm = asm;
			}

			public override Type GetType(string typeName)
			{
				return asm.GetType(typeName);
			}
		}

		public static IkvmAssembly GetAssembly(Type type)
		{
			TypeBase tb = type as TypeBase;
			if (tb != null)
			{
				return tb.ModuleBuilder.Assembly;
			}
			IkvmAssembly ikvmAssembly;
			if (!assemblies.TryGetValue(type.Assembly, out ikvmAssembly))
			{
				ikvmAssembly = new AssemblyWrapper(type.Assembly);
				assemblies.Add(type.Assembly, ikvmAssembly);
			}
			return ikvmAssembly;
		}

		public abstract Type GetType(string typeName);
	}
}
