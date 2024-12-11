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

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Runtime
{

    sealed class InstructionState
    {

        private struct LocalStoreSites
        {

            private int[] data;
            private int count;
            private bool shared;

            internal LocalStoreSites Copy()
            {
                LocalStoreSites n = new LocalStoreSites();
                n.data = data;
                n.count = count;
                n.shared = true;
                return n;
            }

            internal static LocalStoreSites Alloc()
            {
                LocalStoreSites n = new LocalStoreSites();
                n.data = new int[4];
                return n;
            }

            internal void Add(int store)
            {
                for (int i = 0; i < count; i++)
                {
                    if (data[i] == store)
                    {
                        return;
                    }
                }
                if (count == data.Length)
                {
                    int[] newarray = new int[data.Length * 2];
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

            internal int this[int index]
            {
                get
                {
                    return data[index];
                }
            }

            internal int Count
            {
                get
                {
                    return count;
                }
            }

            internal static void MarkShared(LocalStoreSites[] localStoreSites)
            {
                for (int i = 0; i < localStoreSites.Length; i++)
                {
                    localStoreSites[i].shared = true;
                }
            }
        }

        readonly RuntimeContext context;
        RuntimeJavaType[] stack;
        int stackSize;
        int stackEnd;
        RuntimeJavaType[] locals;
        bool unitializedThis;
        internal bool changed = true;

        private enum ShareFlags : byte
        {
            None = 0,
            Stack = 1,
            Locals = 2,
            All = Stack | Locals
        }
        private ShareFlags flags;

        private InstructionState(RuntimeContext context, RuntimeJavaType[] stack, int stackSize, int stackEnd, RuntimeJavaType[] locals, bool unitializedThis)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.flags = ShareFlags.All;
            this.stack = stack;
            this.stackSize = stackSize;
            this.stackEnd = stackEnd;
            this.locals = locals;
            this.unitializedThis = unitializedThis;
        }

        internal InstructionState(RuntimeContext context, int maxLocals, int maxStack)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.flags = ShareFlags.None;
            this.stack = new RuntimeJavaType[maxStack];
            this.stackEnd = maxStack;
            this.locals = new RuntimeJavaType[maxLocals];
        }

        internal InstructionState Copy()
        {
            return new InstructionState(context, stack, stackSize, stackEnd, locals, unitializedThis);
        }

        internal void CopyTo(InstructionState target)
        {
            target.flags = ShareFlags.All;
            target.stack = stack;
            target.stackSize = stackSize;
            target.stackEnd = stackEnd;
            target.locals = locals;
            target.unitializedThis = unitializedThis;
            target.changed = true;
        }

        internal InstructionState CopyLocals()
        {
            InstructionState copy = new InstructionState(context, new RuntimeJavaType[stack.Length], 0, stack.Length, locals, unitializedThis);
            copy.flags &= ~ShareFlags.Stack;
            return copy;
        }

        public static InstructionState operator +(InstructionState s1, InstructionState s2)
        {
            if (s1 == null)
                return s2.Copy();

            if (s1.stackSize != s2.stackSize || s1.stackEnd != s2.stackEnd)
                throw new VerifyError(string.Format("Inconsistent stack height: {0} != {1}",
                    s1.stackSize + s1.stack.Length - s1.stackEnd,
                    s2.stackSize + s2.stack.Length - s2.stackEnd));

            var s = s1.Copy();
            s.changed = s1.changed;
            for (int i = 0; i < s.stackSize; i++)
            {
                var type = s.stack[i];
                var type2 = s2.stack[i];
                if (type == type2)
                {
                    // perfect match, nothing to do
                }
                else if ((type == s1.context.VerifierJavaTypeFactory.ExtendedDouble && type2 == s1.context.PrimitiveJavaTypeFactory.DOUBLE) || (type2 == s1.context.VerifierJavaTypeFactory.ExtendedDouble && type == s1.context.PrimitiveJavaTypeFactory.DOUBLE))
                {
                    if (type != s1.context.VerifierJavaTypeFactory.ExtendedDouble)
                    {
                        s.StackCopyOnWrite();
                        s.stack[i] = s1.context.VerifierJavaTypeFactory.ExtendedDouble;
                        s.changed = true;
                    }
                }
                else if ((type == s1.context.VerifierJavaTypeFactory.ExtendedFloat && type2 == s1.context.PrimitiveJavaTypeFactory.FLOAT) || (type2 == s1.context.VerifierJavaTypeFactory.ExtendedFloat && type == s1.context.PrimitiveJavaTypeFactory.FLOAT))
                {
                    if (type != s1.context.VerifierJavaTypeFactory.ExtendedFloat)
                    {
                        s.StackCopyOnWrite();
                        s.stack[i] = s1.context.VerifierJavaTypeFactory.ExtendedFloat;
                        s.changed = true;
                    }
                }
                else if (!type.IsPrimitive)
                {
                    var baseType = FindCommonBaseType(s1.context, type, type2);
                    if (baseType == s1.context.VerifierJavaTypeFactory.Invalid)
                        throw new VerifyError(string.Format("cannot merge {0} and {1}", type.Name, type2.Name));

                    if (type != baseType)
                    {
                        s.StackCopyOnWrite();
                        s.stack[i] = baseType;
                        s.changed = true;
                    }
                }
                else
                {
                    throw new VerifyError(string.Format("cannot merge {0} and {1}", type.Name, type2.Name));
                }
            }

            for (int i = 0; i < s.locals.Length; i++)
            {
                var type = s.locals[i];
                var type2 = s2.locals[i];
                var baseType = FindCommonBaseType(s1.context, type, type2);
                if (type != baseType)
                {
                    s.LocalsCopyOnWrite();
                    s.locals[i] = baseType;
                    s.changed = true;
                }
            }

            if (!s.unitializedThis && s2.unitializedThis)
            {
                s.unitializedThis = true;
                s.changed = true;
            }

            return s;
        }

        private static LocalStoreSites MergeStoreSites(LocalStoreSites h1, LocalStoreSites h2)
        {
            if (h1.Count == 0)
            {
                return h2.Copy();
            }
            if (h2.Count == 0)
            {
                return h1.Copy();
            }
            LocalStoreSites h = h1.Copy();
            for (int i = 0; i < h2.Count; i++)
            {
                h.Add(h2[i]);
            }
            return h;
        }

        internal void SetUnitializedThis(bool state)
        {
            unitializedThis = state;
        }

        internal void CheckUninitializedThis()
        {
            if (unitializedThis)
            {
                throw new VerifyError("Base class constructor wasn't called");
            }
        }

        internal static RuntimeJavaType FindCommonBaseType(RuntimeContext context, RuntimeJavaType type1, RuntimeJavaType type2)
        {
            if (type1 == type2)
            {
                return type1;
            }
            if (type1 == context.VerifierJavaTypeFactory.Null)
            {
                return type2;
            }
            if (type2 == context.VerifierJavaTypeFactory.Null)
            {
                return type1;
            }
            if (type1 == context.VerifierJavaTypeFactory.Invalid || type2 == context.VerifierJavaTypeFactory.Invalid)
            {
                return context.VerifierJavaTypeFactory.Invalid;
            }
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
            if (type1.IsPrimitive || type2.IsPrimitive)
            {
                return context.VerifierJavaTypeFactory.Invalid;
            }
            if (type1 == context.VerifierJavaTypeFactory.UninitializedThis || type2 == context.VerifierJavaTypeFactory.UninitializedThis)
            {
                return context.VerifierJavaTypeFactory.Invalid;
            }
            if (RuntimeVerifierJavaType.IsNew(type1) || RuntimeVerifierJavaType.IsNew(type2))
            {
                return context.VerifierJavaTypeFactory.Invalid;
            }
            if (RuntimeVerifierJavaType.IsThis(type1))
            {
                type1 = ((RuntimeVerifierJavaType)type1).UnderlyingType;
            }
            if (RuntimeVerifierJavaType.IsThis(type2))
            {
                type2 = ((RuntimeVerifierJavaType)type2).UnderlyingType;
            }
            if (type1.IsUnloadable || type2.IsUnloadable)
            {
                return context.VerifierJavaTypeFactory.Unloadable;
            }
            if (type1.ArrayRank > 0 && type2.ArrayRank > 0)
            {
                int rank = 1;
                int rank1 = type1.ArrayRank - 1;
                int rank2 = type2.ArrayRank - 1;
                RuntimeJavaType elem1 = type1.ElementTypeWrapper;
                RuntimeJavaType elem2 = type2.ElementTypeWrapper;
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
                    {
                        return baseType;
                    }
                }
                else
                {
                    baseType = FindCommonBaseTypeHelper(context, elem1, elem2);
                }
                return baseType.MakeArrayType(rank);
            }
            return FindCommonBaseTypeHelper(context, type1, type2);
        }

        private static RuntimeJavaType FindCommonBaseTypeHelper(RuntimeContext context, RuntimeJavaType t1, RuntimeJavaType t2)
        {
            if (t1 == t2)
            {
                return t1;
            }
            if (t1.IsInterface || t2.IsInterface)
            {
                // NOTE according to a paper by Alessandro Coglio & Allen Goldberg titled
                // "Type Safety in the JVM: Some Problems in Java 2 SDK 1.2 and Proposed Solutions"
                // the common base of two interfaces is java.lang.Object, and there is special
                // treatment for java.lang.Object types that allow it to be assigned to any interface
                // type, the JVM's typesafety then depends on the invokeinterface instruction to make
                // sure that the reference actually implements the interface.
                // NOTE the ECMA CLI spec also specifies this interface merging algorithm, so we can't
                // really do anything more clever than this.
                return context.JavaBase.TypeOfJavaLangObject;
            }
            Stack<RuntimeJavaType> st1 = new Stack<RuntimeJavaType>();
            Stack<RuntimeJavaType> st2 = new Stack<RuntimeJavaType>();
            while (t1 != null)
            {
                st1.Push(t1);
                t1 = t1.BaseTypeWrapper;
            }
            while (t2 != null)
            {
                st2.Push(t2);
                t2 = t2.BaseTypeWrapper;
            }
            if (HasMissingBaseType(context, st1) || HasMissingBaseType(context, st2))
            {
                return context.VerifierJavaTypeFactory.Unloadable;
            }
            RuntimeJavaType type = context.JavaBase.TypeOfJavaLangObject;
            for (; ; )
            {
                t1 = st1.Count > 0 ? st1.Pop() : null;
                t2 = st2.Count > 0 ? st2.Pop() : null;
                if (t1 != t2)
                {
                    return type;
                }
                type = t1;
            }
        }

        private static bool HasMissingBaseType(RuntimeContext context, Stack<RuntimeJavaType> st)
        {
#if IMPORTER
            if (st.Pop().IsUnloadable)
            {
                // we have a missing type in base class hierarchy
                context.StaticCompiler.IssueMissingTypeMessage(st.Pop().TypeAsBaseType.BaseType);
                return true;
            }
#endif
            return false;
        }

        private void SetLocal1(int index, RuntimeJavaType type)
        {
            try
            {
                LocalsCopyOnWrite();
                if (index > 0 && locals[index - 1] != context.VerifierJavaTypeFactory.Invalid && locals[index - 1].IsWidePrimitive)
                {
                    locals[index - 1] = context.VerifierJavaTypeFactory.Invalid;
                }
                locals[index] = type;
            }
            catch (IndexOutOfRangeException)
            {
                throw new VerifyError("Illegal local variable number");
            }
        }

        private void SetLocal2(int index, RuntimeJavaType type)
        {
            try
            {
                LocalsCopyOnWrite();
                if (index > 0 && locals[index - 1] != context.VerifierJavaTypeFactory.Invalid && locals[index - 1].IsWidePrimitive)
                {
                    locals[index - 1] = context.VerifierJavaTypeFactory.Invalid;
                }
                locals[index] = type;
                locals[index + 1] = context.VerifierJavaTypeFactory.Invalid;
            }
            catch (IndexOutOfRangeException)
            {
                throw new VerifyError("Illegal local variable number");
            }
        }

        internal void GetLocalInt(int index)
        {
            if (GetLocalType(index) != context.PrimitiveJavaTypeFactory.INT)
            {
                throw new VerifyError("Invalid local type");
            }
        }

        internal void SetLocalInt(int index, int instructionIndex)
        {
            SetLocal1(index, context.PrimitiveJavaTypeFactory.INT);
        }

        internal void GetLocalLong(int index)
        {
            if (GetLocalType(index) != context.PrimitiveJavaTypeFactory.LONG)
            {
                throw new VerifyError("incorrect local type, not long");
            }
        }

        internal void SetLocalLong(int index, int instructionIndex)
        {
            SetLocal2(index, context.PrimitiveJavaTypeFactory.LONG);
        }

        internal void GetLocalFloat(int index)
        {
            if (GetLocalType(index) != context.PrimitiveJavaTypeFactory.FLOAT)
            {
                throw new VerifyError("incorrect local type, not float");
            }
        }

        internal void SetLocalFloat(int index, int instructionIndex)
        {
            SetLocal1(index, context.PrimitiveJavaTypeFactory.FLOAT);
        }

        internal void GetLocalDouble(int index)
        {
            if (GetLocalType(index) != context.PrimitiveJavaTypeFactory.DOUBLE)
            {
                throw new VerifyError("incorrect local type, not double");
            }
        }

        internal void SetLocalDouble(int index, int instructionIndex)
        {
            SetLocal2(index, context.PrimitiveJavaTypeFactory.DOUBLE);
        }

        internal RuntimeJavaType GetLocalType(int index)
        {
            try
            {
                return locals[index];
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
            return locals[index];
        }

        internal void SetLocalType(int index, RuntimeJavaType type, int instructionIndex)
        {
            if (type.IsWidePrimitive)
            {
                SetLocal2(index, type);
            }
            else
            {
                SetLocal1(index, type);
            }
        }

        internal void PushType(RuntimeJavaType type)
        {
            if (type.IsIntOnStackPrimitive)
            {
                type = context.PrimitiveJavaTypeFactory.INT;
            }
            PushHelper(type);
        }

        internal void PushInt()
        {
            PushHelper(context.PrimitiveJavaTypeFactory.INT);
        }

        internal void PushLong()
        {
            PushHelper(context.PrimitiveJavaTypeFactory.LONG);
        }

        internal void PushFloat()
        {
            PushHelper(context.PrimitiveJavaTypeFactory.FLOAT);
        }

        internal void PushExtendedFloat()
        {
            PushHelper(context.VerifierJavaTypeFactory.ExtendedFloat);
        }

        internal void PushDouble()
        {
            PushHelper(context.PrimitiveJavaTypeFactory.DOUBLE);
        }

        internal void PushExtendedDouble()
        {
            PushHelper(context.VerifierJavaTypeFactory.ExtendedDouble);
        }

        internal void PopInt()
        {
            PopIntImpl(PopAnyType());
        }

        internal static void PopIntImpl(RuntimeJavaType type)
        {
            if (type != type.Context.PrimitiveJavaTypeFactory.INT)
            {
                throw new VerifyError("Int expected on stack");
            }
        }

        internal bool PopFloat()
        {
            RuntimeJavaType tw = PopAnyType();
            PopFloatImpl(tw);
            return tw == context.VerifierJavaTypeFactory.ExtendedFloat;
        }

        internal static void PopFloatImpl(RuntimeJavaType tw)
        {
            if (tw != tw.Context.PrimitiveJavaTypeFactory.FLOAT && tw != tw.Context.VerifierJavaTypeFactory.ExtendedFloat)
            {
                throw new VerifyError("Float expected on stack");
            }
        }

        internal bool PopDouble()
        {
            RuntimeJavaType tw = PopAnyType();
            PopDoubleImpl(tw);
            return tw == context.VerifierJavaTypeFactory.ExtendedDouble;
        }

        internal static void PopDoubleImpl(RuntimeJavaType tw)
        {
            if (tw != tw.Context.PrimitiveJavaTypeFactory.DOUBLE && tw != tw.Context.VerifierJavaTypeFactory.ExtendedDouble)
            {
                throw new VerifyError("Double expected on stack");
            }
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
            {
                throw new VerifyError("Array reference expected on stack");
            }
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
            {
                throw new VerifyError("Expected object reference on stack");
            }
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
            {
                throw new VerifyError("Unexpected type " + type.Name + " where " + baseType.Name + " was expected");
            }
            return type;
        }

        internal RuntimeJavaType PeekType()
        {
            if (stackSize == 0)
            {
                throw new VerifyError("Unable to pop operand off an empty stack");
            }
            return stack[stackSize - 1];
        }

        internal void MultiPopAnyType(int count)
        {
            while (count-- != 0)
            {
                PopAnyType();
            }
        }

        internal RuntimeJavaType PopFaultBlockException()
        {
            return stack[--stackSize];
        }

        internal RuntimeJavaType PopAnyType()
        {
            if (stackSize == 0)
            {
                throw new VerifyError("Unable to pop operand off an empty stack");
            }
            RuntimeJavaType type = stack[--stackSize];
            if (type.IsWidePrimitive || type == context.VerifierJavaTypeFactory.ExtendedDouble)
            {
                stackEnd++;
            }
            if (RuntimeVerifierJavaType.IsThis(type))
            {
                type = ((RuntimeVerifierJavaType)type).UnderlyingType;
            }
            if (RuntimeVerifierJavaType.IsFaultBlockException(type))
            {
                RuntimeVerifierJavaType.ClearFaultBlockException(type);
                type = context.JavaBase.TypeOfjavaLangThrowable;
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
            {
                throw new VerifyError("Attempt to split long or double on the stack");
            }
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
            {
                baseType = baseType.Context.PrimitiveJavaTypeFactory.INT;
            }
            if (RuntimeVerifierJavaType.IsNew(type) || type == type.Context.VerifierJavaTypeFactory.UninitializedThis)
            {
                throw new VerifyError("Expecting to find object/array on stack");
            }
            if (type == baseType)
            {
                return type;
            }
            else if (type == baseType.Context.VerifierJavaTypeFactory.ExtendedDouble && baseType == baseType.Context.PrimitiveJavaTypeFactory.DOUBLE)
            {
                return type;
            }
            else if (type == baseType.Context.VerifierJavaTypeFactory.ExtendedFloat && baseType == baseType.Context.PrimitiveJavaTypeFactory.FLOAT)
            {
                return type;
            }
            else if (type.IsPrimitive || baseType.IsPrimitive)
            {
                // throw at the end of the method
            }
            else if (baseType == baseType.Context.JavaBase.TypeOfJavaLangObject)
            {
                return type;
            }
            else if (type.IsUnloadable || baseType.IsUnloadable)
            {
                return type;
            }
            else if (baseType.IsInterfaceOrInterfaceArray)
            {
                // because of the way interfaces references works, if baseType
                // is an interface or array of interfaces, any reference will be accepted
                return type;
            }
            else if (type.IsAssignableTo(baseType))
            {
                return type;
            }
            else if (HasMissingBaseType(type) || HasMissingBaseType(baseType))
            {
                return type;
            }
            throw new VerifyError("Unexpected type " + type.Name + " where " + baseType.Name + " was expected");
        }

        private static bool HasMissingBaseType(RuntimeJavaType tw)
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
            return stackSize;
        }

        internal RuntimeJavaType GetStackSlot(int pos)
        {
            RuntimeJavaType tw = stack[stackSize - 1 - pos];
            if (tw == context.VerifierJavaTypeFactory.ExtendedDouble)
            {
                tw = context.PrimitiveJavaTypeFactory.DOUBLE;
            }
            else if (tw == context.VerifierJavaTypeFactory.ExtendedFloat)
            {
                tw = context.PrimitiveJavaTypeFactory.FLOAT;
            }
            return tw;
        }

        internal RuntimeJavaType GetStackSlotEx(int pos)
        {
            return stack[stackSize - 1 - pos];
        }

        internal RuntimeJavaType GetStackByIndex(int index)
        {
            return stack[index];
        }

        private void PushHelper(RuntimeJavaType type)
        {
            if (type.IsWidePrimitive || type == context.VerifierJavaTypeFactory.ExtendedDouble)
            {
                stackEnd--;
            }
            if (stackSize >= stackEnd)
            {
                throw new VerifyError("Stack overflow");
            }
            StackCopyOnWrite();
            stack[stackSize++] = type;
        }

        internal void MarkInitialized(RuntimeJavaType type, RuntimeJavaType initType, int instructionIndex)
        {
            System.Diagnostics.Debug.Assert(type != null && initType != null);

            for (int i = 0; i < locals.Length; i++)
            {
                if (locals[i] == type)
                {
                    LocalsCopyOnWrite();
                    locals[i] = initType;
                }
            }
            for (int i = 0; i < stackSize; i++)
            {
                if (stack[i] == type)
                {
                    StackCopyOnWrite();
                    stack[i] = initType;
                }
            }
        }

        private void StackCopyOnWrite()
        {
            if ((flags & ShareFlags.Stack) != 0)
            {
                flags &= ~ShareFlags.Stack;
                stack = (RuntimeJavaType[])stack.Clone();
            }
        }

        private void LocalsCopyOnWrite()
        {
            if ((flags & ShareFlags.Locals) != 0)
            {
                flags &= ~ShareFlags.Locals;
                locals = (RuntimeJavaType[])locals.Clone();
            }
        }

        internal void DumpLocals()
        {
            Console.Write("// ");
            string sep = "";
            for (int i = 0; i < locals.Length; i++)
            {
                Console.Write(sep);
                Console.Write(locals[i]);
                sep = ", ";
            }
            Console.WriteLine();
        }

        internal void DumpStack()
        {
            Console.Write("// ");
            string sep = "";
            for (int i = 0; i < stackSize; i++)
            {
                Console.Write(sep);
                Console.Write(stack[i]);
                sep = ", ";
            }
            Console.WriteLine();
        }

        internal void ClearFaultBlockException()
        {
            if (RuntimeVerifierJavaType.IsFaultBlockException(stack[0]))
            {
                StackCopyOnWrite();
                changed = true;
                stack[0] = context.JavaBase.TypeOfjavaLangThrowable;
            }
        }

    }

}
