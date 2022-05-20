/*
  Copyright (C) 2010 Jeroen Frijters

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
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using Type = IKVM.Reflection.Type;

class GenerateSystemCore
{
	static void Main(string[] args)
	{
		Universe universe = new Universe();
		AssemblyName name = new AssemblyName("System.Core");
		name.Version = new Version(3, 5);
		name.SetPublicKey(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
		AssemblyBuilder ab = universe.DefineDynamicAssembly(name, AssemblyBuilderAccess.Save);
		ModuleBuilder modb = ab.DefineDynamicModule("System.Core", "System.Core.dll");
		TypeBuilder tb = modb.DefineType("System.Runtime.CompilerServices.ExtensionAttribute", TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed, universe.Import(typeof(Attribute)));
		tb.DefineDefaultConstructor(MethodAttributes.Public);
		tb.CreateType();
		ab.Save("System.Core.dll");
	}
}
