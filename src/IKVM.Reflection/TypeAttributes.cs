/*
  Copyright (C) 2009-2012 Jeroen Frijters

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

namespace IKVM.Reflection
{

    [Flags]
	public enum TypeAttributes
	{

		AnsiClass = 0,
		Class = 0,
		AutoLayout = 0,
		NotPublic = 0,
		Public = 1,
		NestedPublic = 2,
		NestedPrivate = 3,
		NestedFamily = 4,
		NestedAssembly = 5,
		NestedFamANDAssem = 6,
		VisibilityMask = 7,
		NestedFamORAssem = 7,
		SequentialLayout = 8,
		ExplicitLayout = 16,
		LayoutMask = 24,
		ClassSemanticsMask = 32,
		Interface = 32,
		Abstract = 128,
		Sealed = 256,
		SpecialName = 1024,
		RTSpecialName = 2048,
		Import = 4096,
		Serializable = 8192,
		WindowsRuntime = 16384,
		UnicodeClass = 65536,
		AutoClass = 131072,
		CustomFormatClass = 196608,
		StringFormatMask = 196608,
		HasSecurity = 262144,
		ReservedMask = 264192,
		BeforeFieldInit = 1048576,
		CustomFormatMask = 12582912,

	}

}
