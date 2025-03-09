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

    struct StackState
    {

        readonly InstructionState[] _state;
        readonly int _pc;
        int _sp;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="pc"></param>
        internal StackState(InstructionState[] state, int pc)
        {
            _state = state;
            _pc = pc;
            _sp = state[_pc].GetStackHeight();
        }

        internal RuntimeJavaType PeekType()
        {
            if (_sp == 0)
                throw new VerifyError("Unable to pop operand off an empty stack");

            var type = _state[_pc].GetStackByIndex(_sp - 1);
            if (RuntimeVerifierJavaType.IsThis(type))
                type = ((RuntimeVerifierJavaType)type).UnderlyingType;

            return type;
        }

        internal RuntimeJavaType PopAnyType()
        {
            if (_sp == 0)
                throw new VerifyError("Unable to pop operand off an empty stack");

            var type = _state[_pc].GetStackByIndex(--_sp);

            if (RuntimeVerifierJavaType.IsThis(type))
                type = ((RuntimeVerifierJavaType)type).UnderlyingType;

            if (RuntimeVerifierJavaType.IsFaultBlockException(type))
            {
                RuntimeVerifierJavaType.ClearFaultBlockException(type);
                type = type.Context.JavaBase.TypeOfjavaLangThrowable;
            }

            return type;
        }

        internal RuntimeJavaType PopType(RuntimeJavaType baseType)
        {
            return InstructionState.PopTypeImpl(baseType, PopAnyType());
        }

        // NOTE this can *not* be used to pop double or long
        internal RuntimeJavaType PopType()
        {
            return InstructionState.PopTypeImpl(PopAnyType());
        }

        internal void PopInt()
        {
            InstructionState.PopIntImpl(PopAnyType());
        }

        internal void PopFloat()
        {
            InstructionState.PopFloatImpl(PopAnyType());
        }

        internal void PopDouble()
        {
            InstructionState.PopDoubleImpl(PopAnyType());
        }

        internal void PopLong()
        {
            InstructionState.PopLongImpl(PopAnyType());
        }

        internal RuntimeJavaType PopArrayType()
        {
            return InstructionState.PopArrayTypeImpl(PopAnyType());
        }

        /// <summary>
        /// Null or an initialized object reference.
        /// </summary>
        /// <returns></returns>
        internal RuntimeJavaType PopObjectType()
        {
            return InstructionState.PopObjectTypeImpl(PopAnyType());
        }

        /// <summary>
        /// Null or an initialized object reference derived from <paramref name="baseType"/>.
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        internal RuntimeJavaType PopObjectType(RuntimeJavaType baseType)
        {
            return InstructionState.PopObjectTypeImpl(baseType, PopObjectType());
        }

    }

}
