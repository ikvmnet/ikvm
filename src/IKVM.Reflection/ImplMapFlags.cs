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

    // IKVM.Reflection specific type
    [Flags]
	public enum ImplMapFlags
	{

		NoMangle = 0x0001,
		CharSetMask = 0x0006,
		CharSetNotSpec = 0x0000,
		CharSetAnsi = 0x0002,
		CharSetUnicode = 0x0004,
		CharSetAuto = 0x0006,
		SupportsLastError = 0x0040,
		CallConvMask = 0x0700,
		CallConvWinapi = 0x0100,
		CallConvCdecl = 0x0200,
		CallConvStdcall = 0x0300,
		CallConvThiscall = 0x0400,
		CallConvFastcall = 0x0500,
		// non-standard flags (i.e. CLR specific)
		BestFitOn = 0x0010,
		BestFitOff = 0x0020,
		CharMapErrorOn = 0x1000,
		CharMapErrorOff = 0x2000,

	}

}
