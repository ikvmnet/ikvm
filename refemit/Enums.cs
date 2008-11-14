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

namespace IKVM.Reflection.Emit
{
	public enum AssemblyBuilderAccess
	{
		Save = 2,
		ReflectionOnly = 6,
	}

	public enum OperandType
	{
		InlineBrTarget = 0,
		InlineField = 1,
		InlineI = 2,
		InlineI8 = 3,
		InlineMethod = 4,
		InlineNone = 5,
		InlinePhi = 6,
		InlineR = 7,
		InlineSig = 9,
		InlineString = 10,
		InlineSwitch = 11,
		InlineTok = 12,
		InlineType = 13,
		InlineVar = 14,
		ShortInlineBrTarget = 15,
		ShortInlineI = 16,
		ShortInlineR = 17,
		ShortInlineVar = 18,
	}

	public enum FlowControl
	{
		Branch = 0,
		Break = 1,
		Call = 2,
		Cond_Branch = 3,
		Meta = 4,
		Next = 5,
		Return = 7,
		Throw = 8,
	}

	public enum PackingSize
	{
		Unspecified = 0,
		Size1 = 1,
	}

	public enum PEFileKinds
	{
		Dll = 1,
		ConsoleApplication = 2,
		WindowApplication = 3,
	}
}
