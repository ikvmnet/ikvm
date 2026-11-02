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
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace IKVM.Runtime
{

    struct InstructionState
    {

        struct LocalStoreSites
        {

            int[] data;
            int count;
            bool shared;

            internal LocalStoreSites Copy()
            {
                var n = new LocalStoreSites();
                n.data = data;
                n.count = count;
                n.shared = true;
                return n;
            }

            internal static LocalStoreSites Alloc()
            {
                var n = new LocalStoreSites();
                n.data = new int[4];
                return n;
            }

            internal void Add(int store)
            {
                for (int i = 0; i < count; i++)
                    if (data[i] == store)
                        return;

                if (count == data.Length)
                {
                    var newarray = new int[data.Length * 2];
                    Buffer.BlockCopy(data, 0, newarray, 0, data.Length * 4);
                    data = newarray;
                    shared = false;
                }

                if (shared)
                {
                    shared = false;
                    data = (int[])data.Clone();
                }

                data[count++] = store;
            }

            internal int this[int index] => data[index];

            internal int Count => count;

            internal static void MarkShared(LocalStoreSites[] localStoreSites)
            {
                for (int i = 0; i < localStoreSites.Length; i++)
                    localStoreSites[i].shared = true;
            }

        }

        enum ShareFlags : byte
        {
            None = 0,
            Stack = 1,
            Locals = 2,
            All = Stack | Locals
        }

        readonly RuntimeContext _context;
        readonly InstructionState[] _states;
        VerificationType[] _stack;
        int _stackLen;
        int _stackEnd;
        VerificationType[] _locals;
        bool _uninitializedThis;
        internal bool _changed = true;
        ShareFlags _flags;
        internal bool _initialized = false;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="states"></param>
        /// <param name="stack"></param>
        /// <param name="stackLen"></param>
        /// <param name="stackEnd"></param>
        /// <param name="locals"></param>
        /// <param name="uninitializedThis"></param>
        /// <exception cref="ArgumentNullException"></exception>
        InstructionState(RuntimeContext context, InstructionState[] states, VerificationType[] stack, int stackLen, int stackEnd, VerificationType[] locals, bool uninitializedThis)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _states = states ?? throw new ArgumentNullException(nameof(states));
            _flags = ShareFlags.All;
            _stack = stack;
            _stackLen = stackLen;
            _stackEnd = stackEnd;
            _locals = locals;
            _uninitializedThis = uninitializedThis;
            _initialized = true;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="states">The states of every instruction of the method, through which a fault block exception is cleared once it is known not to belong to a fault block.</param>
        /// <param name="maxLocals"></param>
        /// <param name="maxStack"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal InstructionState(RuntimeContext context, InstructionState[] states, int maxLocals, int maxStack)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _states = states ?? throw new ArgumentNullException(nameof(states));
            _flags = ShareFlags.None;
            _stack = new VerificationType[maxStack];
            _stackEnd = maxStack;
            _locals = new VerificationType[maxLocals];
            _initialized = true;
        }

        /// <summary>
        /// Gets the runtime context.
        /// </summary>
        internal readonly RuntimeContext Context => _context;

        /// <summary>
        /// Creates a copy of this instance.
        /// </summary>
        /// <returns></returns>
        internal InstructionState Copy()
        {
            return new InstructionState(_context, _states, _stack, _stackLen, _stackEnd, _locals, _uninitializedThis);
        }

        /// <summary>
        /// Copies this instruction state to the specified instruction state.
        /// </summary>
        /// <param name="target"></param>
        internal void CopyTo(ref InstructionState target)
        {
            if (_initialized == false)
                throw new InvalidOperationException();

            target._flags = ShareFlags.All;
            target._stack = _stack;
            target._stackLen = _stackLen;
            target._stackEnd = _stackEnd;
            target._locals = _locals;
            target._uninitializedThis = _uninitializedThis;
            target._changed = true;
            target._initialized = _initialized;
        }

        /// <summary>
        /// Creates a new instance with the same local variables.
        /// </summary>
        /// <returns></returns>
        internal InstructionState CopyLocals()
        {
            var copy = new InstructionState(_context, _states, new VerificationType[_stack.Length], 0, _stack.Length, _locals, _uninitializedThis);
            copy._flags &= ~ShareFlags.Stack;
            return copy;
        }

        /// <summary>
        /// Creates a new state which is the merged combination of the two specified states.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        /// <exception cref="VerifyError"></exception>
        public static InstructionState operator +(InstructionState s1, InstructionState s2)
        {
            if (s1._initialized == false)
                return s2.Copy();

            if (s1._stackLen != s2._stackLen || s1._stackEnd != s2._stackEnd)
                throw new VerifyError($"Inconsistent stack height: {s1._stackLen + s1._stack.Length - s1._stackEnd} != {s2._stackLen + s2._stack.Length - s2._stackEnd}");

            var s = s1.Copy();
            s._changed = s1._changed;

            for (int i = 0; i < s._stackLen; i++)
            {
                var type1 = s._stack[i];
                var type2 = s2._stack[i];
                if (type1 == type2)
                {
                    // perfect match, nothing to do
                }
                else if ((type1.IsDouble && type2.IsDouble) || (type1.IsFloat && type2.IsFloat))
                {
                    // the types differ only in precision, so the result is the extended one
                    if (type1.IsExtended == false)
                    {
                        s.StackCopyOnWrite();
                        s._stack[i] = type2;
                        s._changed = true;
                    }
                }
                else if (!type1.IsPrimitive)
                {
                    var baseType = FindCommonBaseType(s1._context, s1._states, type1, type2);
                    if (baseType.IsTop)
                        throw new VerifyError(string.Format("cannot merge {0} and {1}", type1, type2));

                    if (type1 != baseType)
                    {
                        s.StackCopyOnWrite();
                        s._stack[i] = baseType;
                        s._changed = true;
                    }
                }
                else
                {
                    throw new VerifyError(string.Format("cannot merge {0} and {1}", type1, type2));
                }
            }

            for (int i = 0; i < s._locals.Length; i++)
            {
                var type = s._locals[i];
                var type2 = s2._locals[i];
                var baseType = FindCommonBaseType(s1._context, s1._states, type, type2);
                if (type != baseType)
                {
                    s.LocalsCopyOnWrite();
                    s._locals[i] = baseType;
                    s._changed = true;
                }
            }

            if (!s._uninitializedThis && s2._uninitializedThis)
            {
                s._uninitializedThis = true;
                s._changed = true;
            }

            return s;
        }

        internal void SetUnitializedThis(bool state)
        {
            _uninitializedThis = state;
        }

        internal void CheckUninitializedThis()
        {
            if (_uninitializedThis)
                throw new VerifyError("Base class constructor wasn't called");
        }

        /// <summary>
        /// Clears the fault block exception at the start of the handler at the given instruction index, because the
        /// handler turned out not to be a fault block.
        /// </summary>
        /// <param name="states"></param>
        /// <param name="handlerIndex"></param>
        static void ClearFaultBlockException(InstructionState[] states, int handlerIndex)
        {
            states[handlerIndex].ClearFaultBlockException();
        }

        internal static VerificationType FindCommonBaseType(RuntimeContext context, InstructionState[] states, VerificationType type1, VerificationType type2)
        {
            // types are equal
            if (type1 == type2)
                return type1;

            // merging java.lang.Object with anything can only result in java.lang.Object
            var obj = VerificationType.Reference(context.JavaBase.TypeOfJavaLangObject);
            if (type1 == obj || type2 == obj)
                return obj;

            // first type is null, return second
            if (type1.IsNull)
                return type2;

            // second type is null, return first
            if (type2.IsNull)
                return type1;

            // verifier invalid
            if (type1.IsTop || type2.IsTop)
                return VerificationType.Top;

            if (type1.IsFaultBlockException)
            {
                ClearFaultBlockException(states, type1.Index);
                return FindCommonBaseType(context, states, VerificationType.Reference(context.JavaBase.TypeOfjavaLangThrowable), type2);
            }

            if (type2.IsFaultBlockException)
            {
                ClearFaultBlockException(states, type2.Index);
                return FindCommonBaseType(context, states, type1, VerificationType.Reference(context.JavaBase.TypeOfjavaLangThrowable));
            }

            // primitive types cannot be merged
            if (type1.IsPrimitive || type2.IsPrimitive)
                return VerificationType.Top;

            // an object whose constructor has not run cannot be merged
            if (type1.IsUninitialized || type2.IsUninitialized)
                return VerificationType.Top;

            // this type can only be its own underlying
            var tw1 = type1.Type;
            var tw2 = type2.Type;

            // unloadable types cannot be merged
            if (tw1.IsUnloadable || tw2.IsUnloadable)
                return VerificationType.Reference(context.VerifierJavaTypeFactory.Unloadable);

            // unpack array types into underlying element
            if (tw1.ArrayRank > 0 && tw2.ArrayRank > 0)
            {
                int rank = 1;
                int rank1 = tw1.ArrayRank - 1;
                int rank2 = tw2.ArrayRank - 1;
                var elem1 = tw1.ElementTypeWrapper;
                var elem2 = tw2.ElementTypeWrapper;
                while (rank1 != 0 && rank2 != 0)
                {
                    elem1 = elem1.ElementTypeWrapper;
                    elem2 = elem2.ElementTypeWrapper;
                    rank++;
                    rank1--;
                    rank2--;
                }

                // NOTE arrays of value types have special merging semantics!
                // NOTE we don't have to test for the case where the element types are the same, because that
                // is only relevant if the ranks are the same, but if that is the case the types are completely
                // identical, in which case the identity test at the top of this method already returned.
                RuntimeJavaType baseType;
                if (elem1.IsPrimitive || elem2.IsPrimitive || elem1.IsNonPrimitiveValueType || elem2.IsNonPrimitiveValueType)
                {
                    baseType = context.JavaBase.TypeOfJavaLangObject;
                    rank--;
                    if (rank == 0)
                        return VerificationType.Reference(baseType);
                }
                else
                {
                    baseType = FindCommonBaseTypeHelper(context, elem1, elem2);
                }

                // rebuild array type
                return VerificationType.Reference(baseType.MakeArrayType(rank));
            }

            return VerificationType.Reference(FindCommonBaseTypeHelper(context, tw1, tw2));
        }

        static RuntimeJavaType FindCommonBaseTypeHelper(RuntimeContext context, RuntimeJavaType type1, RuntimeJavaType type2)
        {
            if (type1 == type2)
                return type1;

            // NOTE according to a paper by Alessandro Coglio & Allen Goldberg titled
            // "Type Safety in the JVM: Some Problems in Java 2 SDK 1.2 and Proposed Solutions"
            // the common base of two interfaces is java.lang.Object, and there is special
            // treatment for java.lang.Object types that allow it to be assigned to any interface
            // type, the JVM's typesafety then depends on the invokeinterface instruction to make
            // sure that the reference actually implements the interface.
            // NOTE the ECMA CLI spec also specifies this interface merging algorithm, so we can't
            // really do anything more clever than this.
            if (type1.IsInterface && type2.IsInterface)
                return context.JavaBase.TypeOfJavaLangObject;

            // t1 is an interface and is implemented by t2, common base type is t1
            if (type1.IsInterface)
                return type2.ImplementsInterface(type1) ? type1 : context.JavaBase.TypeOfJavaLangObject;

            // t2 is an interface and is implemented by t1, common base type is t2
            if (type2.IsInterface)
                return type1.ImplementsInterface(type2) ? type2 : context.JavaBase.TypeOfJavaLangObject;

            var st1 = new Stack<RuntimeJavaType>();
            while (type1 != null)
            {
                st1.Push(type1);
                type1 = type1.BaseTypeWrapper;
            }

            var st2 = new Stack<RuntimeJavaType>();
            while (type2 != null)
            {
                st2.Push(type2);
                type2 = type2.BaseTypeWrapper;
            }

            // pop each item off of the stacks until they do not match
            var type = context.JavaBase.TypeOfJavaLangObject;
            for (; ; )
            {
                type1 = st1.Count > 0 ? st1.Pop() : null;
                type2 = st2.Count > 0 ? st2.Pop() : null;
                if (type1 != type2)
                    return type;

                type = type1;
            }
        }

        void SetLocal1(int index, VerificationType type)
        {
            try
            {
                LocalsCopyOnWrite();
                if (index > 0 && _locals[index - 1].IsCategory2)
                    _locals[index - 1] = VerificationType.Top;

                _locals[index] = type;
            }
            catch (IndexOutOfRangeException)
            {
                throw new VerifyError("Illegal local variable number");
            }
        }

        void SetLocal2(int index, VerificationType type)
        {
            try
            {
                LocalsCopyOnWrite();
                if (index > 0 && _locals[index - 1].IsCategory2)
                    _locals[index - 1] = VerificationType.Top;

                _locals[index] = type;
                _locals[index + 1] = VerificationType.Top;
            }
            catch (IndexOutOfRangeException)
            {
                throw new VerifyError("Illegal local variable number");
            }
        }

        internal void GetLocalInt(int index)
        {
            if (GetLocalType(index) != VerificationType.Int)
                throw new VerifyError("Invalid local type");
        }

        internal void SetLocalInt(int index, int instructionIndex)
        {
            SetLocal1(index, VerificationType.Int);
        }

        internal void GetLocalLong(int index)
        {
            if (GetLocalType(index) != VerificationType.Long)
                throw new VerifyError("incorrect local type, not long");
        }

        internal void SetLocalLong(int index, int instructionIndex)
        {
            SetLocal2(index, VerificationType.Long);
        }

        internal void GetLocalFloat(int index)
        {
            if (GetLocalType(index) != VerificationType.Float)
                throw new VerifyError("incorrect local type, not float");
        }

        internal void SetLocalFloat(int index, int instructionIndex)
        {
            SetLocal1(index, VerificationType.Float);
        }

        internal void GetLocalDouble(int index)
        {
            if (GetLocalType(index) != VerificationType.Double)
                throw new VerifyError("incorrect local type, not double");
        }

        internal void SetLocalDouble(int index, int instructionIndex)
        {
            SetLocal2(index, VerificationType.Double);
        }

        internal VerificationType GetLocalType(int index)
        {
            try
            {
                return _locals[index];
            }
            catch (IndexOutOfRangeException)
            {
                throw new VerifyError("Illegal local variable number");
            }
        }

        // this is used by the compiler (indirectly, through CodeInfo.GetLocalType),
        // we've already verified the code so we know we won't run outside the array boundary,
        // and we don't need to record the fact that we're reading the local.
        internal VerificationType GetLocalTypeEx(int index)
        {
            return _locals[index];
        }

        internal void SetLocalType(int index, VerificationType type, int instructionIndex)
        {
            if (type.IsCategory2)
                SetLocal2(index, type);
            else
                SetLocal1(index, type);
        }

        internal void PushType(RuntimeJavaType type)
        {
            PushHelper(type.ToVerificationType());
        }

        internal void PushType(VerificationType type)
        {
            PushHelper(type);
        }

        internal void PushInt()
        {
            PushHelper(VerificationType.Int);
        }

        internal void PushLong()
        {
            PushHelper(VerificationType.Long);
        }

        internal void PushFloat()
        {
            PushHelper(VerificationType.Float);
        }

        internal void PushExtendedFloat()
        {
            PushHelper(VerificationType.ExtendedFloat);
        }

        internal void PushDouble()
        {
            PushHelper(VerificationType.Double);
        }

        internal void PushExtendedDouble()
        {
            PushHelper(VerificationType.ExtendedDouble);
        }

        internal void PopInt()
        {
            PopIntImpl(PopAnyType());
        }

        internal static void PopIntImpl(VerificationType type)
        {
            if (!type.IsInt)
                throw new VerifyError("Int expected on stack");
        }

        internal bool PopFloat()
        {
            var tw = PopAnyType();
            PopFloatImpl(tw);
            return tw.IsExtended;
        }

        internal static void PopFloatImpl(VerificationType tw)
        {
            if (!tw.IsFloat)
                throw new VerifyError("Float expected on stack");
        }

        internal bool PopDouble()
        {
            var tw = PopAnyType();
            PopDoubleImpl(tw);
            return tw.IsExtended;
        }

        internal static void PopDoubleImpl(VerificationType tw)
        {
            if (!tw.IsDouble)
                throw new VerifyError("Double expected on stack");
        }

        internal void PopLong()
        {
            PopLongImpl(PopAnyType());
        }

        internal static void PopLongImpl(VerificationType tw)
        {
            if (!tw.IsLong)
            {
                throw new VerifyError("Long expected on stack");
            }
        }

        internal VerificationType PopArrayType()
        {
            return PopArrayTypeImpl(PopAnyType());
        }

        internal static VerificationType PopArrayTypeImpl(VerificationType type)
        {
            if (!type.IsNullOrUnloadable() && !(type.IsReference && type.Type.ArrayRank > 0))
                throw new VerifyError("Array reference expected on stack");

            return type;
        }

        // null or an initialized object reference
        internal VerificationType PopObjectType()
        {
            return PopObjectTypeImpl(PopType());
        }

        internal static VerificationType PopObjectTypeImpl(VerificationType type)
        {
            if (!type.IsReference && !type.IsNull)
                throw new VerifyError("Expected object reference on stack");

            return type;
        }

        // null or an initialized object reference derived from baseType (or baseType)
        internal VerificationType PopObjectType(RuntimeJavaType baseType)
        {
            return PopObjectTypeImpl(baseType, PopObjectType());
        }

        internal static VerificationType PopObjectTypeImpl(RuntimeJavaType baseType, VerificationType type)
        {
            // HACK because of the way interfaces references works, if baseType
            // is an interface or array of interfaces, any reference will be accepted
            if (!baseType.IsUnloadable && !baseType.IsInterfaceOrInterfaceArray && !(type.IsUnloadable() || IsAssignableTo(type, baseType)))
                throw new VerifyError("Unexpected type " + type + " where " + baseType.Name + " was expected");

            return type;
        }

        /// <summary>
        /// Returns whether a value of the given type, which is null or an initialized reference, can be assigned to <paramref name="baseType"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
        static bool IsAssignableTo(VerificationType type, RuntimeJavaType baseType)
        {
            // verifier null can be assigned to anything
            if (type.IsNull)
                return true;

            return type.IsReference && type.Type.IsAssignableTo(baseType);
        }

        internal VerificationType PeekType()
        {
            if (_stackLen == 0)
                throw new VerifyError("Unable to pop operand off an empty stack");

            return _stack[_stackLen - 1];
        }

        internal void MultiPopAnyType(int count)
        {
            while (count-- != 0)
                PopAnyType();
        }

        internal VerificationType PopFaultBlockException()
        {
            return _stack[--_stackLen];
        }

        internal VerificationType PopAnyType()
        {
            if (_stackLen == 0)
                throw new VerifyError("Unable to pop operand off an empty stack");

            var type = _stack[--_stackLen];
            if (type.IsCategory2)
                _stackEnd++;

            return Unwrap(_context, _states, type);
        }

        /// <summary>
        /// Returns the type of a value as it is once popped off the stack: 'this' becomes a plain reference, and a fault
        /// block exception becomes a plain 'java.lang.Throwable', which means its handler is not a fault block.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="states"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static VerificationType Unwrap(RuntimeContext context, InstructionState[] states, VerificationType type)
        {
            if (type.IsThis)
                type = type.WithoutFlags();

            if (type.IsFaultBlockException)
            {
                ClearFaultBlockException(states, type.Index);
                type = VerificationType.Reference(context.JavaBase.TypeOfjavaLangThrowable);
            }

            return type;
        }

        // NOTE this can *not* be used to pop double or long
        internal VerificationType PopType()
        {
            return PopTypeImpl(PopAnyType());
        }

        internal static VerificationType PopTypeImpl(VerificationType type)
        {
            if (type.IsCategory2)
                throw new VerifyError("Attempt to split long or double on the stack");

            return type;
        }

        // this will accept null, a primitive type of the specified type or an initialized reference of the
        // specified type or derived from it
        // NOTE this can also be used to pop double or long
        internal VerificationType PopType(RuntimeJavaType baseType)
        {
            return PopTypeImpl(baseType, PopAnyType());
        }

        internal static VerificationType PopTypeImpl(RuntimeJavaType baseType, VerificationType type)
        {
            if (type.IsUninitialized)
                throw new VerifyError("Expecting to find object/array on stack");

            if (baseType.IsPrimitive)
            {
                // a float or double of extended precision is accepted where a plain one is expected
                if (type.WithoutFlags() == baseType.ToVerificationType())
                    return type;
            }
            else if (type.IsPrimitive == false)
            {
                if (type.IsReference && type.Type == baseType)
                    return type;

                if (baseType == baseType.Context.JavaBase.TypeOfJavaLangObject)
                    return type;

                if (type.IsUnloadable() || baseType.IsUnloadable)
                    return type;

                // because of the way interfaces references works, if baseType
                // is an interface or array of interfaces, any reference will be accepted
                if (baseType.IsInterfaceOrInterfaceArray)
                    return type;

                if (IsAssignableTo(type, baseType))
                    return type;

                if ((type.IsReference && HasMissingBaseType(type.Type)) || HasMissingBaseType(baseType))
                    return type;
            }

            throw new VerifyError("Unexpected type " + type + " where " + baseType.Name + " was expected");
        }

        static bool HasMissingBaseType(RuntimeJavaType tw)
        {
#if IMPORTER
            for (RuntimeJavaType baseTypeWrapper; (baseTypeWrapper = tw.BaseTypeWrapper) != null; tw = baseTypeWrapper)
            {
                if (baseTypeWrapper.IsUnloadable)
                {
                    tw.Context.StaticCompiler.IssueMissingTypeMessage(tw.TypeAsBaseType.BaseType);
                    return true;
                }
            }
#endif
            return false;
        }

        internal int GetStackHeight()
        {
            return _stackLen;
        }

        /// <summary>
        /// Gets the type at the given position from the top of the stack, without extended precision.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        internal VerificationType GetStackSlot(int pos)
        {
            var tw = _stack[_stackLen - 1 - pos];
            if (tw.IsExtended)
                tw = tw.WithoutFlags();

            return tw;
        }

        internal VerificationType GetStackSlotEx(int pos)
        {
            return _stack[_stackLen - 1 - pos];
        }

        internal VerificationType GetStackByIndex(int index)
        {
            return _stack[index];
        }

        void PushHelper(VerificationType type)
        {
            if (type.IsCategory2)
                _stackEnd--;

            if (_stackLen >= _stackEnd)
                throw new VerifyError("Stack overflow");

            StackCopyOnWrite();
            _stack[_stackLen++] = type;
        }

        internal void MarkInitialized(VerificationType type, VerificationType initType, int instructionIndex)
        {
            System.Diagnostics.Debug.Assert(type.IsUninitialized && initType.IsReference);

            for (int i = 0; i < _locals.Length; i++)
            {
                if (_locals[i] == type)
                {
                    LocalsCopyOnWrite();
                    _locals[i] = initType;
                }
            }

            for (int i = 0; i < _stackLen; i++)
            {
                if (_stack[i] == type)
                {
                    StackCopyOnWrite();
                    _stack[i] = initType;
                }
            }
        }

        /// <summary>
        /// Copies the stack for future modification.
        /// </summary>
        void StackCopyOnWrite()
        {
            if ((_flags & ShareFlags.Stack) != 0)
            {
                _flags &= ~ShareFlags.Stack;
                _stack = (VerificationType[])_stack.Clone();
            }
        }

        /// <summary>
        /// Copies the locals for future modification.
        /// </summary>
        void LocalsCopyOnWrite()
        {
            if ((_flags & ShareFlags.Locals) != 0)
            {
                _flags &= ~ShareFlags.Locals;
                _locals = (VerificationType[])_locals.Clone();
            }
        }

        internal void ClearFaultBlockException()
        {
            if (_stack[0].IsFaultBlockException)
            {
                StackCopyOnWrite();
                _changed = true;
                _stack[0] = VerificationType.Reference(_context.JavaBase.TypeOfjavaLangThrowable);
            }
        }

    }

}
