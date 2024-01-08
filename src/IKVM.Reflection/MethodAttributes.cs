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
	public enum MethodAttributes
	{
		MemberAccessMask		= 0x0007,
		PrivateScope			= 0x0000,
		Private					= 0x0001,
		FamANDAssem				= 0x0002,
		Assembly				= 0x0003,
		Family					= 0x0004,
		FamORAssem				= 0x0005,
		Public					= 0x0006,
		Static					= 0x0010,
		Final					= 0x0020,
		Virtual					= 0x0040,
		HideBySig				= 0x0080,
		VtableLayoutMask		= 0x0100,
		ReuseSlot				= 0x0000,
		NewSlot					= 0x0100,
		CheckAccessOnOverride	= 0x0200,
		Abstract				= 0x0400,
		SpecialName				= 0x0800,

		PinvokeImpl				= 0x2000,
		UnmanagedExport			= 0x0008,

		RTSpecialName			= 0x1000,
		HasSecurity				= 0x4000,
		RequireSecObject		= 0x8000,

		ReservedMask			= 0xd000,
	}
}
