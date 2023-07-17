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

        TypeWrapper[] stack;
        int stackSize;
        int stackEnd;
        TypeWrapper[] locals;
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

        private InstructionState(TypeWrapper[] stack, int stackSize, int stackEnd, TypeWrapper[] locals, bool unitializedThis)
        {
            this.flags = ShareFlags.All;
            this.stack = stack;
            this.stackSize = stackSize;
            this.stackEnd = stackEnd;
            this.locals = locals;
            this.unitializedThis = unitializedThis;
        }

        internal InstructionState(int maxLocals, int maxStack)
        {
            this.flags = ShareFlags.None;
            this.stack = new TypeWrapper[maxStack];
            this.stackEnd = maxStack;
            this.locals = new TypeWrapper[maxLocals];
        }

        internal InstructionState Copy()
        {
            return new InstructionState(stack, stackSize, stackEnd, locals, unitializedThis);
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
            InstructionState copy = new InstructionState(new TypeWrapper[stack.Length], 0, stack.Length, locals, unitializedThis);
            copy.flags &= ~ShareFlags.Stack;
            return copy;
        }

        public static InstructionState operator +(InstructionState s1, InstructionState s2)
        {
            if (s1 == null)
            {
                return s2.Copy();
            }
            if (s1.stackSize != s2.stackSize || s1.stackEnd != s2.stackEnd)
            {
                throw new VerifyError(string.Format("Inconsistent stack height: {0} != {1}",
                    s1.stackSize + s1.stack.Length - s1.stackEnd,
                    s2.stackSize + s2.stack.Length - s2.stackEnd));
            }
            InstructionState s = s1.Copy();
            s.changed = s1.changed;
            for (int i = 0; i < s.stackSize; i++)
            {
                TypeWrapper type = s.stack[i];
                TypeWrapper type2 = s2.stack[i];
                if (type == type2)
                {
                    // perfect match, nothing to do
                }
                else if ((type == VerifierTypeWrapper.ExtendedDouble && type2 == PrimitiveTypeWrapper.DOUBLE)
                    || (type2 == VerifierTypeWrapper.ExtendedDouble && type == PrimitiveTypeWrapper.DOUBLE))
                {
                    if (type != VerifierTypeWrapper.ExtendedDouble)
                    {
                        s.StackCopyOnWrite();
                        s.stack[i] = VerifierTypeWrapper.ExtendedDouble;
                        s.changed = true;
                    }
                }
                else if ((type == VerifierTypeWrapper.ExtendedFloat && type2 == PrimitiveTypeWrapper.FLOAT)
                    || (type2 == VerifierTypeWrapper.ExtendedFloat && type == PrimitiveTypeWrapper.FLOAT))
                {
                    if (type != VerifierTypeWrapper.ExtendedFloat)
                    {
                        s.StackCopyOnWrite();
                        s.stack[i] = VerifierTypeWrapper.ExtendedFloat;
                        s.changed = true;
                    }
                }
                else if (!type.IsPrimitive)
                {
                    TypeWrapper baseType = InstructionState.FindCommonBaseType(type, type2);
                    if (baseType == VerifierTypeWrapper.Invalid)
                    {
                        throw new VerifyError(string.Format("cannot merge {0} and {1}", type.Name, type2.Name));
                    }
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
                TypeWrapper type = s.locals[i];
                TypeWrapper type2 = s2.locals[i];
                TypeWrapper baseType = InstructionState.FindCommonBaseType(type, type2);
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

        internal static TypeWrapper FindCommonBaseType(TypeWrapper type1, TypeWrapper type2)
        {
            if (type1 == type2)
            {
                return type1;
            }
            if (type1 == VerifierTypeWrapper.Null)
            {
                return type2;
            }
            if (type2 == VerifierTypeWrapper.Null)
            {
                return type1;
            }
            if (type1 == VerifierTypeWrapper.Invalid || type2 == VerifierTypeWrapper.Invalid)
            {
                return VerifierTypeWrapper.Invalid;
            }
            if (VerifierTypeWrapper.IsFaultBlockException(type1))
            {
                VerifierTypeWrapper.ClearFaultBlockException(type1);
                return FindCommonBaseType(CoreClasses.java.lang.Throwable.Wrapper, type2);
            }
            if (VerifierTypeWrapper.IsFaultBlockException(type2))
            {
                VerifierTypeWrapper.ClearFaultBlockException(type2);
                return FindCommonBaseType(type1, CoreClasses.java.lang.Throwable.Wrapper);
            }
            if (type1.IsPrimitive || type2.IsPrimitive)
            {
                return VerifierTypeWrapper.Invalid;
            }
            if (type1 == VerifierTypeWrapper.UninitializedThis || type2 == VerifierTypeWrapper.UninitializedThis)
            {
                return VerifierTypeWrapper.Invalid;
            }
            if (VerifierTypeWrapper.IsNew(type1) || VerifierTypeWrapper.IsNew(type2))
            {
                return VerifierTypeWrapper.Invalid;
            }
            if (VerifierTypeWrapper.IsThis(type1))
            {
                type1 = ((VerifierTypeWrapper)type1).UnderlyingType;
            }
            if (VerifierTypeWrapper.IsThis(type2))
            {
                type2 = ((VerifierTypeWrapper)type2).UnderlyingType;
            }
            if (type1.IsUnloadable || type2.IsUnloadable)
            {
                return VerifierTypeWrapper.Unloadable;
            }
            if (type1.ArrayRank > 0 && type2.ArrayRank > 0)
            {
                int rank = 1;
                int rank1 = type1.ArrayRank - 1;
                int rank2 = type2.ArrayRank - 1;
                TypeWrapper elem1 = type1.ElementTypeWrapper;
                TypeWrapper elem2 = type2.ElementTypeWrapper;
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
                TypeWrapper baseType;
                if (elem1.IsPrimitive || elem2.IsPrimitive || elem1.IsNonPrimitiveValueType || elem2.IsNonPrimitiveValueType)
                {
                    baseType = CoreClasses.java.lang.Object.Wrapper;
                    rank--;
                    if (rank == 0)
                    {
                        return baseType;
                    }
                }
                else
                {
                    baseType = FindCommonBaseTypeHelper(elem1, elem2);
                }
                return baseType.MakeArrayType(rank);
            }
            return FindCommonBaseTypeHelper(type1, type2);
        }

        private static TypeWrapper FindCommonBaseTypeHelper(TypeWrapper t1, TypeWrapper t2)
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
                return CoreClasses.java.lang.Object.Wrapper;
            }
            Stack<TypeWrapper> st1 = new Stack<TypeWrapper>();
            Stack<TypeWrapper> st2 = new Stack<TypeWrapper>();
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
            if (HasMissingBaseType(st1) || HasMissingBaseType(st2))
            {
                return VerifierTypeWrapper.Unloadable;
            }
            TypeWrapper type = CoreClasses.java.lang.Object.Wrapper;
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

        private static bool HasMissingBaseType(Stack<TypeWrapper> st)
        {
#if IMPORTER
        if (st.Pop().IsUnloadable)
        {
            // we have a missing type in base class hierarchy
            StaticCompiler.IssueMissingTypeMessage(st.Pop().TypeAsBaseType.BaseType);
            return true;
        }
#endif
            return false;
        }

        private void SetLocal1(int index, TypeWrapper type)
        {
            try
            {
                LocalsCopyOnWrite();
                if (index > 0 && locals[index - 1] != VerifierTypeWrapper.Invalid && locals[index - 1].IsWidePrimitive)
                {
                    locals[index - 1] = VerifierTypeWrapper.Invalid;
                }
                locals[index] = type;
            }
            catch (IndexOutOfRangeException)
            {
                throw new VerifyError("Illegal local variable number");
            }
        }

        private void SetLocal2(int index, TypeWrapper type)
        {
            try
            {
                LocalsCopyOnWrite();
                if (index > 0 && locals[index - 1] != VerifierTypeWrapper.Invalid && locals[index - 1].IsWidePrimitive)
                {
                    locals[index - 1] = VerifierTypeWrapper.Invalid;
                }
                locals[index] = type;
                locals[index + 1] = VerifierTypeWrapper.Invalid;
            }
            catch (IndexOutOfRangeException)
            {
                throw new VerifyError("Illegal local variable number");
            }
        }

        internal void GetLocalInt(int index)
        {
            if (GetLocalType(index) != PrimitiveTypeWrapper.INT)
            {
                throw new VerifyError("Invalid local type");
            }
        }

        internal void SetLocalInt(int index, int instructionIndex)
        {
            SetLocal1(index, PrimitiveTypeWrapper.INT);
        }

        internal void GetLocalLong(int index)
        {
            if (GetLocalType(index) != PrimitiveTypeWrapper.LONG)
            {
                throw new VerifyError("incorrect local type, not long");
            }
        }

        internal void SetLocalLong(int index, int instructionIndex)
        {
            SetLocal2(index, PrimitiveTypeWrapper.LONG);
        }

        internal void GetLocalFloat(int index)
        {
            if (GetLocalType(index) != PrimitiveTypeWrapper.FLOAT)
            {
                throw new VerifyError("incorrect local type, not float");
            }
        }

        internal void SetLocalFloat(int index, int instructionIndex)
        {
            SetLocal1(index, PrimitiveTypeWrapper.FLOAT);
        }

        internal void GetLocalDouble(int index)
        {
            if (GetLocalType(index) != PrimitiveTypeWrapper.DOUBLE)
            {
                throw new VerifyError("incorrect local type, not double");
            }
        }

        internal void SetLocalDouble(int index, int instructionIndex)
        {
            SetLocal2(index, PrimitiveTypeWrapper.DOUBLE);
        }

        internal TypeWrapper GetLocalType(int index)
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
        internal TypeWrapper GetLocalTypeEx(int index)
        {
            return locals[index];
        }

        internal void SetLocalType(int index, TypeWrapper type, int instructionIndex)
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

        internal void PushType(TypeWrapper type)
        {
            if (type.IsIntOnStackPrimitive)
            {
                type = PrimitiveTypeWrapper.INT;
            }
            PushHelper(type);
        }

        internal void PushInt()
        {
            PushHelper(PrimitiveTypeWrapper.INT);
        }

        internal void PushLong()
        {
            PushHelper(PrimitiveTypeWrapper.LONG);
        }

        internal void PushFloat()
        {
            PushHelper(PrimitiveTypeWrapper.FLOAT);
        }

        internal void PushExtendedFloat()
        {
            PushHelper(VerifierTypeWrapper.ExtendedFloat);
        }

        internal void PushDouble()
        {
            PushHelper(PrimitiveTypeWrapper.DOUBLE);
        }

        internal void PushExtendedDouble()
        {
            PushHelper(VerifierTypeWrapper.ExtendedDouble);
        }

        internal void PopInt()
        {
            PopIntImpl(PopAnyType());
        }

        internal static void PopIntImpl(TypeWrapper type)
        {
            if (type != PrimitiveTypeWrapper.INT)
            {
                throw new VerifyError("Int expected on stack");
            }
        }

        internal bool PopFloat()
        {
            TypeWrapper tw = PopAnyType();
            PopFloatImpl(tw);
            return tw == VerifierTypeWrapper.ExtendedFloat;
        }

        internal static void PopFloatImpl(TypeWrapper tw)
        {
            if (tw != PrimitiveTypeWrapper.FLOAT && tw != VerifierTypeWrapper.ExtendedFloat)
            {
                throw new VerifyError("Float expected on stack");
            }
        }

        internal bool PopDouble()
        {
            TypeWrapper tw = PopAnyType();
            PopDoubleImpl(tw);
            return tw == VerifierTypeWrapper.ExtendedDouble;
        }

        internal static void PopDoubleImpl(TypeWrapper tw)
        {
            if (tw != PrimitiveTypeWrapper.DOUBLE && tw != VerifierTypeWrapper.ExtendedDouble)
            {
                throw new VerifyError("Double expected on stack");
            }
        }

        internal void PopLong()
        {
            PopLongImpl(PopAnyType());
        }

        internal static void PopLongImpl(TypeWrapper tw)
        {
            if (tw != PrimitiveTypeWrapper.LONG)
            {
                throw new VerifyError("Long expected on stack");
            }
        }

        internal TypeWrapper PopArrayType()
        {
            return PopArrayTypeImpl(PopAnyType());
        }

        internal static TypeWrapper PopArrayTypeImpl(TypeWrapper type)
        {
            if (!VerifierTypeWrapper.IsNullOrUnloadable(type) && type.ArrayRank == 0)
            {
                throw new VerifyError("Array reference expected on stack");
            }
            return type;
        }

        // null or an initialized object reference
        internal TypeWrapper PopObjectType()
        {
            return PopObjectTypeImpl(PopType());
        }

        internal static TypeWrapper PopObjectTypeImpl(TypeWrapper type)
        {
            if (type.IsPrimitive || VerifierTypeWrapper.IsNew(type) || type == VerifierTypeWrapper.UninitializedThis)
            {
                throw new VerifyError("Expected object reference on stack");
            }
            return type;
        }

        // null or an initialized object reference derived from baseType (or baseType)
        internal TypeWrapper PopObjectType(TypeWrapper baseType)
        {
            return PopObjectTypeImpl(baseType, PopObjectType());
        }

        internal static TypeWrapper PopObjectTypeImpl(TypeWrapper baseType, TypeWrapper type)
        {
            // HACK because of the way interfaces references works, if baseType
            // is an interface or array of interfaces, any reference will be accepted
            if (!baseType.IsUnloadable && !baseType.IsInterfaceOrInterfaceArray && !(type.IsUnloadable || type.IsAssignableTo(baseType)))
            {
                throw new VerifyError("Unexpected type " + type.Name + " where " + baseType.Name + " was expected");
            }
            return type;
        }

        internal TypeWrapper PeekType()
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

        internal TypeWrapper PopFaultBlockException()
        {
            return stack[--stackSize];
        }

        internal TypeWrapper PopAnyType()
        {
            if (stackSize == 0)
            {
                throw new VerifyError("Unable to pop operand off an empty stack");
            }
            TypeWrapper type = stack[--stackSize];
            if (type.IsWidePrimitive || type == VerifierTypeWrapper.ExtendedDouble)
            {
                stackEnd++;
            }
            if (VerifierTypeWrapper.IsThis(type))
            {
                type = ((VerifierTypeWrapper)type).UnderlyingType;
            }
            if (VerifierTypeWrapper.IsFaultBlockException(type))
            {
                VerifierTypeWrapper.ClearFaultBlockException(type);
                type = CoreClasses.java.lang.Throwable.Wrapper;
            }
            return type;
        }

        // NOTE this can *not* be used to pop double or long
        internal TypeWrapper PopType()
        {
            return PopTypeImpl(PopAnyType());
        }

        internal static TypeWrapper PopTypeImpl(TypeWrapper type)
        {
            if (type.IsWidePrimitive || type == VerifierTypeWrapper.ExtendedDouble)
            {
                throw new VerifyError("Attempt to split long or double on the stack");
            }
            return type;
        }

        // this will accept null, a primitive type of the specified type or an initialized reference of the
        // specified type or derived from it
        // NOTE this can also be used to pop double or long
        internal TypeWrapper PopType(TypeWrapper baseType)
        {
            return PopTypeImpl(baseType, PopAnyType());
        }

        internal static TypeWrapper PopTypeImpl(TypeWrapper baseType, TypeWrapper type)
        {
            if (baseType.IsIntOnStackPrimitive)
            {
                baseType = PrimitiveTypeWrapper.INT;
            }
            if (VerifierTypeWrapper.IsNew(type) || type == VerifierTypeWrapper.UninitializedThis)
            {
                throw new VerifyError("Expecting to find object/array on stack");
            }
            if (type == baseType)
            {
                return type;
            }
            else if (type == VerifierTypeWrapper.ExtendedDouble && baseType == PrimitiveTypeWrapper.DOUBLE)
            {
                return type;
            }
            else if (type == VerifierTypeWrapper.ExtendedFloat && baseType == PrimitiveTypeWrapper.FLOAT)
            {
                return type;
            }
            else if (type.IsPrimitive || baseType.IsPrimitive)
            {
                // throw at the end of the method
            }
            else if (baseType == CoreClasses.java.lang.Object.Wrapper)
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

        private static bool HasMissingBaseType(TypeWrapper tw)
        {
#if IMPORTER
        for (TypeWrapper baseTypeWrapper; (baseTypeWrapper = tw.BaseTypeWrapper) != null; tw = baseTypeWrapper)
        {
            if (baseTypeWrapper.IsUnloadable)
            {
                StaticCompiler.IssueMissingTypeMessage(tw.TypeAsBaseType.BaseType);
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

        internal TypeWrapper GetStackSlot(int pos)
        {
            TypeWrapper tw = stack[stackSize - 1 - pos];
            if (tw == VerifierTypeWrapper.ExtendedDouble)
            {
                tw = PrimitiveTypeWrapper.DOUBLE;
            }
            else if (tw == VerifierTypeWrapper.ExtendedFloat)
            {
                tw = PrimitiveTypeWrapper.FLOAT;
            }
            return tw;
        }

        internal TypeWrapper GetStackSlotEx(int pos)
        {
            return stack[stackSize - 1 - pos];
        }

        internal TypeWrapper GetStackByIndex(int index)
        {
            return stack[index];
        }

        private void PushHelper(TypeWrapper type)
        {
            if (type.IsWidePrimitive || type == VerifierTypeWrapper.ExtendedDouble)
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

        internal void MarkInitialized(TypeWrapper type, TypeWrapper initType, int instructionIndex)
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
                stack = (TypeWrapper[])stack.Clone();
            }
        }

        private void LocalsCopyOnWrite()
        {
            if ((flags & ShareFlags.Locals) != 0)
            {
                flags &= ~ShareFlags.Locals;
                locals = (TypeWrapper[])locals.Clone();
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
            if (VerifierTypeWrapper.IsFaultBlockException(stack[0]))
            {
                StackCopyOnWrite();
                changed = true;
                stack[0] = CoreClasses.java.lang.Throwable.Wrapper;
            }
        }

    }

}
