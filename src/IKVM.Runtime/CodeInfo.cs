/*
  Copyright (C) 2002-2014 Jeroen Frijters

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

namespace IKVM.Runtime
{

    readonly struct CodeInfo
    {

        private readonly InstructionState[] state;

        internal CodeInfo(InstructionState[] state)
        {
            this.state = state;
        }

        internal bool HasState(int index)
        {
            return state[index] != null;
        }

        internal int GetStackHeight(int index)
        {
            return state[index].GetStackHeight();
        }

        internal TypeWrapper GetStackTypeWrapper(int index, int pos)
        {
            TypeWrapper type = state[index].GetStackSlot(pos);
            if (VerifierTypeWrapper.IsThis(type))
            {
                type = ((VerifierTypeWrapper)type).UnderlyingType;
            }
            return type;
        }

        internal TypeWrapper GetRawStackTypeWrapper(int index, int pos)
        {
            return state[index].GetStackSlot(pos);
        }

        internal bool IsStackTypeExtendedDouble(int index, int pos)
        {
            return state[index].GetStackSlotEx(pos) == VerifierTypeWrapper.ExtendedDouble;
        }

        internal TypeWrapper GetLocalTypeWrapper(int index, int local)
        {
            return state[index].GetLocalTypeEx(local);
        }

    }

}
