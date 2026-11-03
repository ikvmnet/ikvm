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

using IKVM.CoreLib.Linking;

namespace IKVM.CoreLib.Jvm
{

    struct StackState<TTypeSystem, TType, TMember, TField, TMethod>
        where TTypeSystem : struct, IJvmTypeSystem<TType, TMember, TField, TMethod>
        where TType : class
        where TMember : class
        where TField : class, TMember
        where TMethod : class, TMember
    {

        readonly InstructionState<TTypeSystem, TType, TMember, TField, TMethod>[] _state;
        readonly int _pc;
        int _sp;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="pc"></param>
        internal StackState(InstructionState<TTypeSystem, TType, TMember, TField, TMethod>[] state, int pc)
        {
            _state = state;
            _pc = pc;
            _sp = state[_pc].GetStackHeight();
        }

        /// <summary>
        /// Gets the type system.
        /// </summary>
        readonly TTypeSystem TypeSystem => _state[_pc].TypeSystem;

        internal VerificationType<TType> PeekType()
        {
            if (_sp == 0)
                throw new VerifyException("Unable to pop operand off an empty stack");

            var type = _state[_pc].GetStackByIndex(_sp - 1);
            if (type.IsThis)
                type = type.WithoutFlags();

            return type;
        }

        internal VerificationType<TType> PopAnyType()
        {
            if (_sp == 0)
                throw new VerifyException("Unable to pop operand off an empty stack");

            return InstructionState<TTypeSystem, TType, TMember, TField, TMethod>.Unwrap(TypeSystem, _state, _state[_pc].GetStackByIndex(--_sp));
        }

        internal VerificationType<TType> PopType(TType baseType)
        {
            return InstructionState<TTypeSystem, TType, TMember, TField, TMethod>.PopTypeImpl(TypeSystem, baseType, PopAnyType());
        }

        // NOTE this can *not* be used to pop double or long
        internal VerificationType<TType> PopType()
        {
            return InstructionState<TTypeSystem, TType, TMember, TField, TMethod>.PopTypeImpl(PopAnyType());
        }

        internal void PopInt()
        {
            InstructionState<TTypeSystem, TType, TMember, TField, TMethod>.PopIntImpl(PopAnyType());
        }

        internal void PopFloat()
        {
            InstructionState<TTypeSystem, TType, TMember, TField, TMethod>.PopFloatImpl(PopAnyType());
        }

        internal void PopDouble()
        {
            InstructionState<TTypeSystem, TType, TMember, TField, TMethod>.PopDoubleImpl(PopAnyType());
        }

        internal void PopLong()
        {
            InstructionState<TTypeSystem, TType, TMember, TField, TMethod>.PopLongImpl(PopAnyType());
        }

        internal VerificationType<TType> PopArrayType()
        {
            return InstructionState<TTypeSystem, TType, TMember, TField, TMethod>.PopArrayTypeImpl(TypeSystem, PopAnyType());
        }

        /// <summary>
        /// Null or an initialized object reference.
        /// </summary>
        /// <returns></returns>
        internal VerificationType<TType> PopObjectType()
        {
            return InstructionState<TTypeSystem, TType, TMember, TField, TMethod>.PopObjectTypeImpl(PopAnyType());
        }

        /// <summary>
        /// Null or an initialized object reference derived from <paramref name="baseType"/>.
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        internal VerificationType<TType> PopObjectType(TType baseType)
        {
            return InstructionState<TTypeSystem, TType, TMember, TField, TMethod>.PopObjectTypeImpl(TypeSystem, baseType, PopObjectType());
        }

    }

}
