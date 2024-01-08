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
	public enum MethodImplAttributes
	{
		CodeTypeMask		= 0x0003,
		IL					= 0x0000,
		Native				= 0x0001,
		OPTIL				= 0x0002,
		Runtime				= 0x0003,
		ManagedMask			= 0x0004,
		Unmanaged			= 0x0004,
		Managed				= 0x0000,

		ForwardRef			= 0x0010,
		PreserveSig			= 0x0080,
		InternalCall		= 0x1000,
		Synchronized		= 0x0020,
		NoInlining			= 0x0008,
		NoOptimization		= 0x0040,
		AggressiveInlining  = 0x0100,

		MaxMethodImplVal	= 0xffff,
	}
}
