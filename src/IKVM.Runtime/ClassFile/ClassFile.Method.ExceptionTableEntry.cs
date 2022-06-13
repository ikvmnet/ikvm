/*
  Copyright (C) 2002-2015 Jeroen Frijters

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

namespace IKVM.Internal
{

    sealed partial class ClassFile
	{
        internal sealed partial class Method
        {
            internal sealed class ExceptionTableEntry
			{
				internal readonly int startIndex;
				internal readonly int endIndex;
				internal readonly int handlerIndex;
				internal readonly ushort catch_type;
				internal readonly int ordinal;
				internal readonly bool isFinally;

				internal ExceptionTableEntry(int startIndex, int endIndex, int handlerIndex, ushort catch_type, int ordinal)
					: this(startIndex, endIndex, handlerIndex, catch_type, ordinal, false)
				{
				}

				internal ExceptionTableEntry(int startIndex, int endIndex, int handlerIndex, ushort catch_type, int ordinal, bool isFinally)
				{
					this.startIndex = startIndex;
					this.endIndex = endIndex;
					this.handlerIndex = handlerIndex;
					this.catch_type = catch_type;
					this.ordinal = ordinal;
					this.isFinally = isFinally;
				}
			}
		}
	}

}
