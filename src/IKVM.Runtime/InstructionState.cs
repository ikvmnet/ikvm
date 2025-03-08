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

    sealed class InstructionState
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
        RuntimeJavaType[] _stack;
        int _stackLen;
        int _stackEnd;
        RuntimeJavaType[] _locals;
        bool _uninitializedThis;
        internal bool _changed = true;

        ShareFlags _flags;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="stack"></param>
        /// <param name="stackLen"></param>
        /// <param name="stackEnd"></param>
        /// <param name="locals"></param>
        /// <param name="uninitializedThis"></param>
        /// <exception cref="ArgumentNullException"></exception>
        InstructionState(RuntimeContext context, RuntimeJavaType[] stack, int stackLen, int stackEnd, RuntimeJavaType[] locals, bool uninitializedThis)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _flags = ShareFlags.All;
            _stack = stack;
            _stackLen = stackLen;
            _stackEnd = stackEnd;
            _locals = locals;
            _uninitializedThis = uninitializedThis;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="maxLocals"></param>
        /// <param name="maxStack"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal InstructionState(RuntimeContext context, int maxLocals, int maxStack)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _flags = ShareFlags.None;
            _stack = new RuntimeJavaType[maxStack];
            _stackEnd = maxStack;
            _locals = new RuntimeJavaType[maxLocals];
        }

        /// <summary>
        /// Creates a copy of this instance.
        /// </summary>
        /// <returns></returns>
        internal InstructionState Copy()
        {
            return new InstructionState(_context, _stack, _stackLen, _stackEnd, _locals, _uninitializedThis);
        }

        /// <summary>
        /// Copies this instruction state to the specified instruction state.
        /// </summary>
        /// <param name="target"></param>
        internal void CopyTo(InstructionState target)
        {
            target._flags = ShareFlags.All;
            target._stack = _stack;
            target._stackLen = _stackLen;
            target._stackEnd = _stackEnd;
            target._locals = _locals;
            target._uninitializedThis = _uninitializedThis;
            target._changed = true;
        }

        /// <summary>
        /// Creates a new instance with the same local variables.
        /// </summary>
        /// <returns></returns>
        internal InstructionState CopyLocals()
        {
            var copy = new InstructionState(_context, new RuntimeJavaType[_stack.Length], 0, _stack.Length, _locals, _uninitializedThis);
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
            if (s1 == null)
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
                else if ((type1 == s1._context.VerifierJavaTypeFactory.ExtendedDouble && type2 == s1._context.PrimitiveJavaTypeFactory.DOUBLE) || (type2 == s1._context.VerifierJavaTypeFactory.ExtendedDouble && type1 == s1._context.PrimitiveJavaTypeFactory.DOUBLE))
                {
                    if (type1 != s1._context.VerifierJavaTypeFactory.ExtendedDouble)
                    {
                        s.StackCopyOnWrite();
                        s._stack[i] = s1._context.VerifierJavaTypeFactory.ExtendedDouble;
                        s._changed = true;
                    }
                }
                else if ((type1 == s1._context.VerifierJavaTypeFactory.ExtendedFloat && type2 == s1._context.PrimitiveJavaTypeFactory.FLOAT) || (type2 == s1._context.VerifierJavaTypeFactory.ExtendedFloat && type1 == s1._context.PrimitiveJavaTypeFactory.FLOAT))
                {
                    if (type1 != s1._context.VerifierJavaTypeFactory.ExtendedFloat)
                    {
                        s.StackCopyOnWrite();
                        s._stack[i] = s1._context.VerifierJavaTypeFactory.ExtendedFloat;
                        s._changed = true;
                    }
                }
                else if (!type1.IsPrimitive)
                {
                    var baseType = FindCommonBaseType(s1._context, type1, type2);
                    if (baseType == s1._context.VerifierJavaTypeFactory.Invalid)
                        throw new VerifyError(string.Format("cannot merge {0} and {1}", type1.Name, type2.Name));

                    if (type1 != baseType)
                    {
                        s.StackCopyOnWrite();
                        s._stack[i] = baseType;
                        s._changed = true;
                    }
                }
                else
                {
                    throw new VerifyError(string.Format("cannot merge {0} and {1}", type1.Name, type2.Name));
                }
            }

            for (int i = 0; i < s._locals.Length; i++)
            {
                var type = s._locals[i];
                var type2 = s2._locals[i];
                var baseType = FindCommonBaseType(s1._context, type, type2);
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

        internal static RuntimeJavaType FindCommonBaseType(RuntimeContext context, RuntimeJavaType type1, RuntimeJavaType type2)
        {
            // types are equal
            if (type1 == type2)
                return type1;

            // merging java.lang.Object with anything can only result in java.lang.Object
            if (type1 == context.JavaBase.TypeOfJavaLangObject || type2 == context.JavaBase.TypeOfJavaLangObject)
                return context.JavaBase.TypeOfJavaLangObject;

            // verifier invalid
            if (type1 == context.VerifierJavaTypeFactory.Invalid || type2 == context.VerifierJavaTypeFactory.Invalid)
                return context.VerifierJavaTypeFactory.Invalid;

            // first type is null, return second
            if (type1 == context.VerifierJavaTypeFactory.Null)
                return type2;

            // second type is null, return first
            if (type2 == context.VerifierJavaTypeFactory.Null)
                return type1;

            if (RuntimeVerifierJavaType.IsFaultBlockException(type1))
            {
                RuntimeVerifierJavaType.ClearFaultBlockException(type1);
                return FindCommonBaseType(context, context.JavaBase.TypeOfjavaLangThrowable, type2);
            }

            if (RuntimeVerifierJavaType.IsFaultBlockException(type2))
            {
                RuntimeVerifierJavaType.ClearFaultBlockException(type2);
                return FindCommonBaseType(context, type1, context.JavaBase.TypeOfjavaLangThrowable);
            }

            // primitive types cannot be merged
            if (type1.IsPrimitive || type2.IsPrimitive)
                return context.VerifierJavaTypeFactory.Invalid;

            // this type without init being run cannot be merged
            if (type1 == context.VerifierJavaTypeFactory.UninitializedThis || type2 == context.VerifierJavaTypeFactory.UninitializedThis)
                return context.VerifierJavaTypeFactory.Invalid;

            // new verifier type cannot be merged
            if (RuntimeVerifierJavaType.IsNew(type1) || RuntimeVerifierJavaType.IsNew(type2))
                return context.VerifierJavaTypeFactory.Invalid;

            // this type can only be its own underlying
            if (RuntimeVerifierJavaType.IsThis(type1))
                type1 = ((RuntimeVerifierJavaType)type1).UnderlyingType;

            // this type can only be its own underlying
            if (RuntimeVerifierJavaType.IsThis(type2))
                type2 = ((RuntimeVerifierJavaType)type2).UnderlyingType;

            // unloadable types cannot be merged
            if (type1.IsUnloadable || type2.IsUnloadable)
                return context.VerifierJavaTypeFactory.Unloadable;

            // unpack array types into underlying element
            if (type1.ArrayRank > 0 && type2.ArrayRank > 0)
            {
                int rank = 1;
                int rank1 = type1.ArrayRank - 1;
                int rank2 = type2.ArrayRank - 1;
                var elem1 = type1.ElementTypeWrapper;
                var elem2 = type2.ElementTypeWrapper;
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
                        return baseType;
                }
                else
                {
                    baseType = FindCommonBaseTypeHelper(context, elem1, elem2);
                }

                // rebuild array type
                return baseType.MakeArrayType(rank);
            }

            return FindCommonBaseTypeHelper(context, type1, type2);
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

        void SetLocal1(int index, RuntimeJavaType type)
        {
            try
            {
                LocalsCopyOnWrite();
                if (index > 0 && _locals[index - 1] != _context.VerifierJavaTypeFactory.Invalid && _locals[index - 1].IsWidePrimitive)
                    _locals[index - 1] = _context.VerifierJavaTypeFactory.Invalid;

                _locals[index] = type;
            }
            catch (IndexOutOfRangeException)
            {
                throw new VerifyError("Illegal local variable number");
            }
        }

        void SetLocal2(int index, RuntimeJavaType type)
        {
            try
            {
                LocalsCopyOnWrite();
                if (index > 0 && _locals[index - 1] != _context.VerifierJavaTypeFactory.Invalid && _locals[index - 1].IsWidePrimitive)
                    _locals[index - 1] = _context.VerifierJavaTypeFactory.Invalid;

                _locals[index] = type;
                _locals[index + 1] = _context.VerifierJavaTypeFactory.Invalid;
            }
            catch (IndexOutOfRangeException)
            {
                throw new VerifyError("Illegal local variable number");
            }
        }

        internal void GetLocalInt(int index)
        {
            if (GetLocalType(index) != _context.PrimitiveJavaTypeFactory.INT)
                throw new VerifyError("Invalid local type");
        }

        internal void SetLocalInt(int index, int instructionIndex)
        {
            SetLocal1(index, _context.PrimitiveJavaTypeFactory.INT);
        }

        internal void GetLocalLong(int index)
        {
            if (GetLocalType(index) != _context.PrimitiveJavaTypeFactory.LONG)
                throw new VerifyError("incorrect local type, not long");
        }

        internal void SetLocalLong(int index, int instructionIndex)
        {
            SetLocal2(index, _context.PrimitiveJavaTypeFactory.LONG);
        }

        internal void GetLocalFloat(int index)
        {
            if (GetLocalType(index) != _context.PrimitiveJavaTypeFactory.FLOAT)
                throw new VerifyError("incorrect local type, not float");
        }

        internal void SetLocalFloat(int index, int instructionIndex)
        {
            SetLocal1(index, _context.PrimitiveJavaTypeFactory.FLOAT);
        }

        internal void GetLocalDouble(int index)
        {
            if (GetLocalType(index) != _context.PrimitiveJavaTypeFactory.DOUBLE)
                throw new VerifyError("incorrect local type, not double");
        }

        internal void SetLocalDouble(int index, int instructionIndex)
        {
            SetLocal2(index, _context.PrimitiveJavaTypeFactory.DOUBLE);
        }

        internal RuntimeJavaType GetLocalType(int index)
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

        // this is used by the compiler (indirectly, through MethodAnalyzer.GetLocalTypeWrapper),
        // we've already verified the code so we know we won't run outside the array boundary,
        // and we don't need to record the fact that we're reading the local.
        internal RuntimeJavaType GetLocalTypeEx(int index)
        {
            return _locals[index];
        }

        internal void SetLocalType(int index, RuntimeJavaType type, int instructionIndex)
        {
            if (type.IsWidePrimitive)
                SetLocal2(index, type);
            else
                SetLocal1(index, type);
        }

        internal void PushType(RuntimeJavaType type)
        {
            if (type.IsIntOnStackPrimitive)
                type = _context.PrimitiveJavaTypeFactory.INT;

            PushHelper(type);
        }

        internal void PushInt()
        {
            PushHelper(_context.PrimitiveJavaTypeFactory.INT);
        }

        internal void PushLong()
        {
            PushHelper(_context.PrimitiveJavaTypeFactory.LONG);
        }

        internal void PushFloat()
        {
            PushHelper(_context.PrimitiveJavaTypeFactory.FLOAT);
        }

        internal void PushExtendedFloat()
        {
            PushHelper(_context.VerifierJavaTypeFactory.ExtendedFloat);
        }

        internal void PushDouble()
        {
            PushHelper(_context.PrimitiveJavaTypeFactory.DOUBLE);
        }

        internal void PushExtendedDouble()
        {
            PushHelper(_context.VerifierJavaTypeFactory.ExtendedDouble);
        }

        internal void PopInt()
        {
            PopIntImpl(PopAnyType());
        }

        internal static void PopIntImpl(RuntimeJavaType type)
        {
            if (type != type.Context.PrimitiveJavaTypeFactory.INT)
                throw new VerifyError("Int expected on stack");
        }

        internal bool PopFloat()
        {
            var tw = PopAnyType();
            PopFloatImpl(tw);
            return tw == _context.VerifierJavaTypeFactory.ExtendedFloat;
        }

        internal static void PopFloatImpl(RuntimeJavaType tw)
        {
            if (tw != tw.Context.PrimitiveJavaTypeFactory.FLOAT && tw != tw.Context.VerifierJavaTypeFactory.ExtendedFloat)
                throw new VerifyError("Float expected on stack");
        }

        internal bool PopDouble()
        {
            var tw = PopAnyType();
            PopDoubleImpl(tw);
            return tw == _context.VerifierJavaTypeFactory.ExtendedDouble;
        }

        internal static void PopDoubleImpl(RuntimeJavaType tw)
        {
            if (tw != tw.Context.PrimitiveJavaTypeFactory.DOUBLE && tw != tw.Context.VerifierJavaTypeFactory.ExtendedDouble)
                throw new VerifyError("Double expected on stack");
        }

        internal void PopLong()
        {
            PopLongImpl(PopAnyType());
        }

        internal static void PopLongImpl(RuntimeJavaType tw)
        {
            if (tw != tw.Context.PrimitiveJavaTypeFactory.LONG)
            {
                throw new VerifyError("Long expected on stack");
            }
        }

        internal RuntimeJavaType PopArrayType()
        {
            return PopArrayTypeImpl(PopAnyType());
        }

        internal static RuntimeJavaType PopArrayTypeImpl(RuntimeJavaType type)
        {
            if (!RuntimeVerifierJavaType.IsNullOrUnloadable(type) && type.ArrayRank == 0)
                throw new VerifyError("Array reference expected on stack");

            return type;
        }

        // null or an initialized object reference
        internal RuntimeJavaType PopObjectType()
        {
            return PopObjectTypeImpl(PopType());
        }

        internal static RuntimeJavaType PopObjectTypeImpl(RuntimeJavaType type)
        {
            if (type.IsPrimitive || RuntimeVerifierJavaType.IsNew(type) || type == type.Context.VerifierJavaTypeFactory.UninitializedThis)
                throw new VerifyError("Expected object reference on stack");

            return type;
        }

        // null or an initialized object reference derived from baseType (or baseType)
        internal RuntimeJavaType PopObjectType(RuntimeJavaType baseType)
        {
            return PopObjectTypeImpl(baseType, PopObjectType());
        }

        internal static RuntimeJavaType PopObjectTypeImpl(RuntimeJavaType baseType, RuntimeJavaType type)
        {
            // HACK because of the way interfaces references works, if baseType
            // is an interface or array of interfaces, any reference will be accepted
            if (!baseType.IsUnloadable && !baseType.IsInterfaceOrInterfaceArray && !(type.IsUnloadable || type.IsAssignableTo(baseType)))
                throw new VerifyError("Unexpected type " + type.Name + " where " + baseType.Name + " was expected");

            return type;
        }

        internal RuntimeJavaType PeekType()
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

        internal RuntimeJavaType PopFaultBlockException()
        {
            return _stack[--_stackLen];
        }

        internal RuntimeJavaType PopAnyType()
        {
            if (_stackLen == 0)
                throw new VerifyError("Unable to pop operand off an empty stack");

            var type = _stack[--_stackLen];
            if (type.IsWidePrimitive || type == _context.VerifierJavaTypeFactory.ExtendedDouble)
                _stackEnd++;

            if (RuntimeVerifierJavaType.IsThis(type))
                type = ((RuntimeVerifierJavaType)type).UnderlyingType;

            if (RuntimeVerifierJavaType.IsFaultBlockException(type))
            {
                RuntimeVerifierJavaType.ClearFaultBlockException(type);
                type = _context.JavaBase.TypeOfjavaLangThrowable;
            }

            return type;
        }

        // NOTE this can *not* be used to pop double or long
        internal RuntimeJavaType PopType()
        {
            return PopTypeImpl(PopAnyType());
        }

        internal static RuntimeJavaType PopTypeImpl(RuntimeJavaType type)
        {
            if (type.IsWidePrimitive || type == type.Context.VerifierJavaTypeFactory.ExtendedDouble)
                throw new VerifyError("Attempt to split long or double on the stack");

            return type;
        }

        // this will accept null, a primitive type of the specified type or an initialized reference of the
        // specified type or derived from it
        // NOTE this can also be used to pop double or long
        internal RuntimeJavaType PopType(RuntimeJavaType baseType)
        {
            return PopTypeImpl(baseType, PopAnyType());
        }

        internal static RuntimeJavaType PopTypeImpl(RuntimeJavaType baseType, RuntimeJavaType type)
        {
            if (baseType.IsIntOnStackPrimitive)
                baseType = baseType.Context.PrimitiveJavaTypeFactory.INT;

            if (RuntimeVerifierJavaType.IsNew(type) || type == type.Context.VerifierJavaTypeFactory.UninitializedThis)
                throw new VerifyError("Expecting to find object/array on stack");

            if (type == baseType)
                return type;

            if (type == baseType.Context.VerifierJavaTypeFactory.ExtendedDouble && baseType == baseType.Context.PrimitiveJavaTypeFactory.DOUBLE)
                return type;

            if (type == baseType.Context.VerifierJavaTypeFactory.ExtendedFloat && baseType == baseType.Context.PrimitiveJavaTypeFactory.FLOAT)
                return type;

            if (type.IsPrimitive == false && baseType.IsPrimitive == false)
            {
                if (baseType == baseType.Context.JavaBase.TypeOfJavaLangObject)
                    return type;

                if (type.IsUnloadable || baseType.IsUnloadable)
                    return type;

                // because of the way interfaces references works, if baseType
                // is an interface or array of interfaces, any reference will be accepted
                if (baseType.IsInterfaceOrInterfaceArray)
                    return type;

                if (type.IsAssignableTo(baseType))
                    return type;

                if (HasMissingBaseType(type) || HasMissingBaseType(baseType))
                    return type;
            }

            throw new VerifyError("Unexpected type " + type.Name + " where " + baseType.Name + " was expected");
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

        internal RuntimeJavaType GetStackSlot(int pos)
        {
            var tw = _stack[_stackLen - 1 - pos];
            if (tw == _context.VerifierJavaTypeFactory.ExtendedDouble)
                tw = _context.PrimitiveJavaTypeFactory.DOUBLE;
            else if (tw == _context.VerifierJavaTypeFactory.ExtendedFloat)
                tw = _context.PrimitiveJavaTypeFactory.FLOAT;

            return tw;
        }

        internal RuntimeJavaType GetStackSlotEx(int pos)
        {
            return _stack[_stackLen - 1 - pos];
        }

        internal RuntimeJavaType GetStackByIndex(int index)
        {
            return _stack[index];
        }

        void PushHelper(RuntimeJavaType type)
        {
            if (type.IsWidePrimitive || type == _context.VerifierJavaTypeFactory.ExtendedDouble)
                _stackEnd--;

            if (_stackLen >= _stackEnd)
                throw new VerifyError("Stack overflow");

            StackCopyOnWrite();
            _stack[_stackLen++] = type;
        }

        internal void MarkInitialized(RuntimeJavaType type, RuntimeJavaType initType, int instructionIndex)
        {
            System.Diagnostics.Debug.Assert(type != null && initType != null);

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
                _stack = (RuntimeJavaType[])_stack.Clone();
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
                _locals = (RuntimeJavaType[])_locals.Clone();
            }
        }

        internal void ClearFaultBlockException()
        {
            if (RuntimeVerifierJavaType.IsFaultBlockException(_stack[0]))
            {
                StackCopyOnWrite();
                _changed = true;
                _stack[0] = _context.JavaBase.TypeOfjavaLangThrowable;
            }
        }

    }

}
