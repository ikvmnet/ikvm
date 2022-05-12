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
#if STATIC_COMPILER || STUB_GENERATOR
using Type = IKVM.Reflection.Type;
#endif

namespace IKVM.Internal
{
	static class Types
	{
		internal static readonly Type Object = JVM.Import(typeof(System.Object));
		internal static readonly Type ValueType = JVM.Import(typeof(System.ValueType));
		internal static readonly Type Enum = JVM.Import(typeof(System.Enum));
		internal static readonly Type Type = JVM.Import(typeof(System.Type));
		internal static readonly Type String = JVM.Import(typeof(System.String));
		internal static readonly Type Exception = JVM.Import(typeof(System.Exception));
		internal static readonly Type Array = JVM.Import(typeof(System.Array));
		internal static readonly Type Attribute = JVM.Import(typeof(System.Attribute));
		internal static readonly Type Delegate = JVM.Import(typeof(System.Delegate));
		internal static readonly Type MulticastDelegate = JVM.Import(typeof(System.MulticastDelegate));
		internal static readonly Type RuntimeTypeHandle = JVM.Import(typeof(System.RuntimeTypeHandle));

		internal static readonly Type IntPtr = JVM.Import(typeof(System.IntPtr));
		internal static readonly Type Void = JVM.Import(typeof(void));
		internal static readonly Type Boolean = JVM.Import(typeof(System.Boolean));
		internal static readonly Type Byte = JVM.Import(typeof(System.Byte));
		internal static readonly Type SByte = JVM.Import(typeof(System.SByte));
		internal static readonly Type Char = JVM.Import(typeof(System.Char));
		internal static readonly Type Int16 = JVM.Import(typeof(System.Int16));
		internal static readonly Type UInt16 = JVM.Import(typeof(System.UInt16));
		internal static readonly Type Int32 = JVM.Import(typeof(System.Int32));
		internal static readonly Type UInt32 = JVM.Import(typeof(System.UInt32));
		internal static readonly Type Int64 = JVM.Import(typeof(System.Int64));
		internal static readonly Type UInt64 = JVM.Import(typeof(System.UInt64));
		internal static readonly Type Single = JVM.Import(typeof(System.Single));
		internal static readonly Type Double = JVM.Import(typeof(System.Double));

		internal static readonly Type IsVolatile = JVM.Import(typeof(System.Runtime.CompilerServices.IsVolatile));
		internal static readonly Type SecurityAttribute = JVM.Import(typeof(System.Security.Permissions.SecurityAttribute));

		// we want deterministics initialization
		static Types() { }
	}
}
