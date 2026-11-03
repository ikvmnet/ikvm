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

namespace IKVM.CoreLib.Jvm
{

    readonly struct CodeInfo<TTypeSystem, TType, TMember, TField, TMethod>
        where TTypeSystem : struct, IJvmTypeSystem<TType, TMember, TField, TMethod>
        where TType : class
        where TMember : class
        where TField : class, TMember
        where TMethod : class, TMember
    {

        readonly TTypeSystem _typeSystem;
        readonly InstructionState<TTypeSystem, TType, TMember, TField, TMethod>[] _state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeSystem"></param>
        /// <param name="state"></param>
        internal CodeInfo(TTypeSystem typeSystem, InstructionState<TTypeSystem, TType, TMember, TField, TMethod>[] state)
        {
            _typeSystem = typeSystem;
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

        /// <summary>
        /// Gets the type at the given position from the top of the stack at the given instruction, without extended
        /// precision, and with the 'this' reference as a plain reference.
        /// </summary>
        /// <param name="pc"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        internal VerificationType<TType> GetStackType(int pc, int pos)
        {
            var type = _state[pc].GetStackSlot(pos);
            if (type.IsThis)
                type = type.WithoutFlags();

            return type;
        }

        /// <summary>
        /// Gets the type at the given position from the top of the stack at the given instruction, without extended
        /// precision.
        /// </summary>
        /// <param name="pc"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        internal VerificationType<TType> GetRawStackType(int pc, int pos)
        {
            return _state[pc].GetStackSlot(pos);
        }

        internal bool IsStackTypeExtendedDouble(int pc, int pos)
        {
            var type = _state[pc].GetStackSlotEx(pos);
            return type.IsDouble && type.IsExtended;
        }

        internal bool IsStackTypeExtendedFloat(int pc, int pos)
        {
            var type = _state[pc].GetStackSlotEx(pos);
            return type.IsFloat && type.IsExtended;
        }

        internal VerificationType<TType> GetLocalType(int pc, int local)
        {
            return _state[pc].GetLocalTypeEx(local);
        }

        /// <summary>
        /// Returns the type that values of both of the given types can be merged to, or top if there is none.
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        internal VerificationType<TType> FindCommonBaseType(VerificationType<TType> type1, VerificationType<TType> type2)
        {
            return InstructionState<TTypeSystem, TType, TMember, TField, TMethod>.FindCommonBaseType(_typeSystem, _state, type1, type2);
        }

        /// <summary>
        /// Clears the fault block exception at the start of the handler at the given instruction, because the handler
        /// is not a fault block.
        /// </summary>
        /// <param name="handlerIndex"></param>
        internal void ClearFaultBlockException(int handlerIndex)
        {
            _state[handlerIndex].ClearFaultBlockException();
        }

    }

}
