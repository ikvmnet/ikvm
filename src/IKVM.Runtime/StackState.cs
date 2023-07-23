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

        private InstructionState state;
        private int sp;

        internal StackState(InstructionState state)
        {
            this.state = state;
            sp = state.GetStackHeight();
        }

        internal RuntimeJavaType PeekType()
        {
            if (sp == 0)
            {
                throw new VerifyError("Unable to pop operand off an empty stack");
            }
            RuntimeJavaType type = state.GetStackByIndex(sp - 1);
            if (RuntimeVerifierJavaType.IsThis(type))
            {
                type = ((RuntimeVerifierJavaType)type).UnderlyingType;
            }
            return type;
        }

        internal RuntimeJavaType PopAnyType()
        {
            if (sp == 0)
            {
                throw new VerifyError("Unable to pop operand off an empty stack");
            }

            var type = state.GetStackByIndex(--sp);
            if (RuntimeVerifierJavaType.IsThis(type))
            {
                type = ((RuntimeVerifierJavaType)type).UnderlyingType;
            }
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

        // either null or an initialized object reference
        internal RuntimeJavaType PopObjectType()
        {
            return InstructionState.PopObjectTypeImpl(PopAnyType());
        }

        // null or an initialized object reference derived from baseType (or baseType)
        internal RuntimeJavaType PopObjectType(RuntimeJavaType baseType)
        {
            return InstructionState.PopObjectTypeImpl(baseType, PopObjectType());
        }
    }

}
