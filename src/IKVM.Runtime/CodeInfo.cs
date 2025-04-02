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

using System;

namespace IKVM.Runtime
{

    readonly struct CodeInfo
    {

        readonly RuntimeContext _context;
        readonly InstructionState[] _state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="state"></param>
        internal CodeInfo(RuntimeContext context, InstructionState[] state)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _state = state ?? throw new ArgumentNullException(nameof(state));
        }

        internal bool HasState(int pc)
        {
            return _state[pc]._initialized;
        }

        internal int GetStackHeight(int pc)
        {
            return _state[pc].GetStackHeight();
        }

        internal RuntimeJavaType GetStackTypeWrapper(int pc, int pos)
        {
            var type = _state[pc].GetStackSlot(pos);
            if (RuntimeVerifierJavaType.IsThis(type))
                type = ((RuntimeVerifierJavaType)type).UnderlyingType;

            return type;
        }

        internal RuntimeJavaType GetRawStackTypeWrapper(int pc, int pos)
        {
            return _state[pc].GetStackSlot(pos);
        }

        internal bool IsStackTypeExtendedDouble(int pc, int pos)
        {
            return _state[pc].GetStackSlotEx(pos) == _context.VerifierJavaTypeFactory.ExtendedDouble;
        }

        internal RuntimeJavaType GetLocalTypeWrapper(int pc, int local)
        {
            return _state[pc].GetLocalTypeEx(local);
        }

    }

}
