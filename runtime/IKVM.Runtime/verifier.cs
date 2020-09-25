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
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using IKVM.Internal;
using InstructionFlags = IKVM.Internal.ClassFile.Method.InstructionFlags;
using ExceptionTableEntry = IKVM.Internal.ClassFile.Method.ExceptionTableEntry;

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
			for(int i = 0; i < count; i++)
			{
				if(data[i] == store)
				{
					return;
				}
			}
			if(count == data.Length)
			{
				int[] newarray = new int[data.Length * 2];
				Buffer.BlockCopy(data, 0, newarray, 0, data.Length * 4);
				data = newarray;
				shared = false;
			}
			if(shared)
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
			for(int i = 0; i < localStoreSites.Length; i++)
			{
				localStoreSites[i].shared = true;
			}
		}
	}
	private TypeWrapper[] stack;
	private int stackSize;
	private int stackEnd;
	private TypeWrapper[] locals;
	private bool unitializedThis;
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

	public static InstructionState operator+(InstructionState s1, InstructionState s2)
	{
		if(s1 == null)
		{
			return s2.Copy();
		}
		if(s1.stackSize != s2.stackSize || s1.stackEnd != s2.stackEnd)
		{
			throw new VerifyError(string.Format("Inconsistent stack height: {0} != {1}",
				s1.stackSize + s1.stack.Length - s1.stackEnd,
				s2.stackSize + s2.stack.Length - s2.stackEnd));
		}
		InstructionState s = s1.Copy();
		s.changed = s1.changed;
		for(int i = 0; i < s.stackSize; i++)
		{
			TypeWrapper type = s.stack[i];
			TypeWrapper type2 = s2.stack[i];
			if(type == type2)
			{
				// perfect match, nothing to do
			}
			else if((type == VerifierTypeWrapper.ExtendedDouble && type2 == PrimitiveTypeWrapper.DOUBLE)
				|| (type2 == VerifierTypeWrapper.ExtendedDouble && type == PrimitiveTypeWrapper.DOUBLE))
			{
				if(type != VerifierTypeWrapper.ExtendedDouble)
				{
					s.StackCopyOnWrite();
					s.stack[i] = VerifierTypeWrapper.ExtendedDouble;
					s.changed = true;
				}
			}
			else if((type == VerifierTypeWrapper.ExtendedFloat && type2 == PrimitiveTypeWrapper.FLOAT)
				|| (type2 == VerifierTypeWrapper.ExtendedFloat && type == PrimitiveTypeWrapper.FLOAT))
			{
				if(type != VerifierTypeWrapper.ExtendedFloat)
				{
					s.StackCopyOnWrite();
					s.stack[i] = VerifierTypeWrapper.ExtendedFloat;
					s.changed = true;
				}
			}
			else if(!type.IsPrimitive)
			{
				TypeWrapper baseType = InstructionState.FindCommonBaseType(type, type2);
				if(baseType == VerifierTypeWrapper.Invalid)
				{
					throw new VerifyError(string.Format("cannot merge {0} and {1}", type.Name, type2.Name));
				}
				if(type != baseType)
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
		for(int i = 0; i < s.locals.Length; i++)
		{
			TypeWrapper type = s.locals[i];
			TypeWrapper type2 = s2.locals[i];
			TypeWrapper baseType = InstructionState.FindCommonBaseType(type, type2);
			if(type != baseType)
			{
				s.LocalsCopyOnWrite();
				s.locals[i] = baseType;
				s.changed = true;
			}
		}
		if(!s.unitializedThis && s2.unitializedThis)
		{
			s.unitializedThis = true;
			s.changed = true;
		}
		return s;
	}

	private static LocalStoreSites MergeStoreSites(LocalStoreSites h1, LocalStoreSites h2)
	{
		if(h1.Count == 0)
		{
			return h2.Copy();
		}
		if(h2.Count == 0)
		{
			return h1.Copy();
		}
		LocalStoreSites h = h1.Copy();
		for(int i = 0; i < h2.Count; i++)
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
		if(unitializedThis)
		{
			throw new VerifyError("Base class constructor wasn't called");
		}
	}

	internal static TypeWrapper FindCommonBaseType(TypeWrapper type1, TypeWrapper type2)
	{
		if(type1 == type2)
		{
			return type1;
		}
		if(type1 == VerifierTypeWrapper.Null)
		{
			return type2;
		}
		if(type2 == VerifierTypeWrapper.Null)
		{
			return type1;
		}
		if(type1 == VerifierTypeWrapper.Invalid || type2 == VerifierTypeWrapper.Invalid)
		{
			return VerifierTypeWrapper.Invalid;
		}
		if(VerifierTypeWrapper.IsFaultBlockException(type1))
		{
			VerifierTypeWrapper.ClearFaultBlockException(type1);
			return FindCommonBaseType(CoreClasses.java.lang.Throwable.Wrapper, type2);
		}
		if(VerifierTypeWrapper.IsFaultBlockException(type2))
		{
			VerifierTypeWrapper.ClearFaultBlockException(type2);
			return FindCommonBaseType(type1, CoreClasses.java.lang.Throwable.Wrapper);
		}
		if(type1.IsPrimitive || type2.IsPrimitive)
		{
			return VerifierTypeWrapper.Invalid;
		}
		if(type1 == VerifierTypeWrapper.UninitializedThis || type2 == VerifierTypeWrapper.UninitializedThis)
		{
			return VerifierTypeWrapper.Invalid;
		}
		if(VerifierTypeWrapper.IsNew(type1) || VerifierTypeWrapper.IsNew(type2))
		{
			return VerifierTypeWrapper.Invalid;
		}
		if(VerifierTypeWrapper.IsThis(type1))
		{
			type1 = ((VerifierTypeWrapper)type1).UnderlyingType;
		}
		if(VerifierTypeWrapper.IsThis(type2))
		{
			type2 = ((VerifierTypeWrapper)type2).UnderlyingType;
		}
		if(type1.IsUnloadable || type2.IsUnloadable)
		{
			return VerifierTypeWrapper.Unloadable;
		}
		if(type1.ArrayRank > 0 && type2.ArrayRank > 0)
		{
			int rank = 1;
			int rank1 = type1.ArrayRank - 1;
			int rank2 = type2.ArrayRank - 1;
			TypeWrapper elem1 = type1.ElementTypeWrapper;
			TypeWrapper elem2 = type2.ElementTypeWrapper;
			while(rank1 != 0 && rank2 != 0)
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
			if(elem1.IsPrimitive || elem2.IsPrimitive || elem1.IsNonPrimitiveValueType || elem2.IsNonPrimitiveValueType)
			{
				baseType = CoreClasses.java.lang.Object.Wrapper;
				rank--;
				if(rank == 0)
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
		if(t1 == t2)
		{
			return t1;
		}
		if(t1.IsInterface || t2.IsInterface)
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
		while(t1 != null)
		{
			st1.Push(t1);
			t1 = t1.BaseTypeWrapper;
		}
		while(t2 != null)
		{
			st2.Push(t2);
			t2 = t2.BaseTypeWrapper;
		}
		if(HasMissingBaseType(st1) || HasMissingBaseType(st2))
		{
			return VerifierTypeWrapper.Unloadable;
		}
		TypeWrapper type = CoreClasses.java.lang.Object.Wrapper;
		for(;;)
		{
			t1 = st1.Count > 0 ? st1.Pop() : null;
			t2 = st2.Count > 0 ? st2.Pop() : null;
			if(t1 != t2)
			{
				return type;
			}
			type = t1;
		}
	}

	private static bool HasMissingBaseType(Stack<TypeWrapper> st)
	{
#if STATIC_COMPILER
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
			if(index > 0 && locals[index - 1] != VerifierTypeWrapper.Invalid && locals[index - 1].IsWidePrimitive)
			{
				locals[index - 1] = VerifierTypeWrapper.Invalid;
			}
			locals[index] = type;
		}
		catch(IndexOutOfRangeException)
		{
			throw new VerifyError("Illegal local variable number");
		}
	}

	private void SetLocal2(int index, TypeWrapper type)
	{
		try
		{
			LocalsCopyOnWrite();
			if(index > 0 && locals[index - 1] != VerifierTypeWrapper.Invalid && locals[index - 1].IsWidePrimitive)
			{
				locals[index - 1] = VerifierTypeWrapper.Invalid;
			}
			locals[index] = type;
			locals[index + 1] = VerifierTypeWrapper.Invalid;
		}
		catch(IndexOutOfRangeException)
		{
			throw new VerifyError("Illegal local variable number");
		}
	}

	internal void GetLocalInt(int index)
	{
		if(GetLocalType(index) != PrimitiveTypeWrapper.INT)
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
		if(GetLocalType(index) != PrimitiveTypeWrapper.LONG)
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
		if(GetLocalType(index) != PrimitiveTypeWrapper.FLOAT)
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
		if(GetLocalType(index) != PrimitiveTypeWrapper.DOUBLE)
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
		catch(IndexOutOfRangeException)
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
		if(type.IsWidePrimitive)
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
		if(type.IsIntOnStackPrimitive)
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
		if(tw != PrimitiveTypeWrapper.FLOAT && tw != VerifierTypeWrapper.ExtendedFloat)
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
		if(tw != PrimitiveTypeWrapper.DOUBLE && tw != VerifierTypeWrapper.ExtendedDouble)
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
		if(tw != PrimitiveTypeWrapper.LONG)
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
		if(!VerifierTypeWrapper.IsNullOrUnloadable(type) && type.ArrayRank == 0)
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
		if(type.IsPrimitive || VerifierTypeWrapper.IsNew(type) || type == VerifierTypeWrapper.UninitializedThis)
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
		if(!baseType.IsUnloadable && !baseType.IsInterfaceOrInterfaceArray && !(type.IsUnloadable || type.IsAssignableTo(baseType)))
		{
			throw new VerifyError("Unexpected type " + type.Name + " where " + baseType.Name + " was expected");
		}
		return type;
	}

	internal TypeWrapper PeekType()
	{
		if(stackSize == 0)
		{
			throw new VerifyError("Unable to pop operand off an empty stack");
		}
		return stack[stackSize - 1];
	}

	internal void MultiPopAnyType(int count)
	{
		while(count-- != 0)
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
		if(stackSize == 0)
		{
			throw new VerifyError("Unable to pop operand off an empty stack");
		}
		TypeWrapper type = stack[--stackSize];
		if(type.IsWidePrimitive || type == VerifierTypeWrapper.ExtendedDouble)
		{
			stackEnd++;
		}
		if(VerifierTypeWrapper.IsThis(type))
		{
			type = ((VerifierTypeWrapper)type).UnderlyingType;
		}
		if(VerifierTypeWrapper.IsFaultBlockException(type))
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
		if(type.IsWidePrimitive || type == VerifierTypeWrapper.ExtendedDouble)
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
		if(baseType.IsIntOnStackPrimitive)
		{
			baseType = PrimitiveTypeWrapper.INT;
		}
		if(VerifierTypeWrapper.IsNew(type) || type == VerifierTypeWrapper.UninitializedThis)
		{
			throw new VerifyError("Expecting to find object/array on stack");
		}
		if(type == baseType)
		{
			return type;
		}
		else if(type == VerifierTypeWrapper.ExtendedDouble && baseType == PrimitiveTypeWrapper.DOUBLE)
		{
			return type;
		}
		else if(type == VerifierTypeWrapper.ExtendedFloat && baseType == PrimitiveTypeWrapper.FLOAT)
		{
			return type;
		}
		else if(type.IsPrimitive || baseType.IsPrimitive)
		{
			// throw at the end of the method
		}
		else if(baseType == CoreClasses.java.lang.Object.Wrapper)
		{
			return type;
		}
		else if(type.IsUnloadable || baseType.IsUnloadable)
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
#if STATIC_COMPILER
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
		if(tw == VerifierTypeWrapper.ExtendedDouble)
		{
			tw = PrimitiveTypeWrapper.DOUBLE;
		}
		else if(tw == VerifierTypeWrapper.ExtendedFloat)
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
		if(type.IsWidePrimitive || type == VerifierTypeWrapper.ExtendedDouble)
		{
			stackEnd--;
		}
		if(stackSize >= stackEnd)
		{
			throw new VerifyError("Stack overflow");
		}
		StackCopyOnWrite();
		stack[stackSize++] = type;
	}

	internal void MarkInitialized(TypeWrapper type, TypeWrapper initType, int instructionIndex)
	{
		System.Diagnostics.Debug.Assert(type != null && initType != null);

		for(int i = 0; i < locals.Length; i++)
		{
			if(locals[i] == type)
			{
				LocalsCopyOnWrite();
				locals[i] = initType;
			}
		}
		for(int i = 0; i < stackSize; i++)
		{
			if(stack[i] == type)
			{
				StackCopyOnWrite();
				stack[i] = initType;
			}
		}
	}

	private void StackCopyOnWrite()
	{
		if((flags & ShareFlags.Stack) != 0)
		{
			flags &= ~ShareFlags.Stack;
			stack = (TypeWrapper[])stack.Clone();
		}
	}

	private void LocalsCopyOnWrite()
	{
		if((flags & ShareFlags.Locals) != 0)
		{
			flags &= ~ShareFlags.Locals;
			locals = (TypeWrapper[])locals.Clone();
		}
	}

	internal void DumpLocals()
	{
		Console.Write("// ");
		string sep = "";
		for(int i = 0; i < locals.Length; i++)
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
		for(int i = 0; i < stackSize; i++)
		{
			Console.Write(sep);
			Console.Write(stack[i]);
			sep = ", ";
		}
		Console.WriteLine();
	}

	internal void ClearFaultBlockException()
	{
		if(VerifierTypeWrapper.IsFaultBlockException(stack[0]))
		{
			StackCopyOnWrite();
			changed = true;
			stack[0] = CoreClasses.java.lang.Throwable.Wrapper;
		}
	}
}

struct StackState
{
	private InstructionState state;
	private int sp;

	internal StackState(InstructionState state)
	{
		this.state = state;
		sp = state.GetStackHeight();
	}

	internal TypeWrapper PeekType()
	{
		if(sp == 0)
		{
			throw new VerifyError("Unable to pop operand off an empty stack");
		}
		TypeWrapper type = state.GetStackByIndex(sp - 1);
		if(VerifierTypeWrapper.IsThis(type))
		{
			type = ((VerifierTypeWrapper)type).UnderlyingType;
		}
		return type;
	}

	internal TypeWrapper PopAnyType()
	{
		if(sp == 0)
		{
			throw new VerifyError("Unable to pop operand off an empty stack");
		}
		TypeWrapper type = state.GetStackByIndex(--sp);
		if(VerifierTypeWrapper.IsThis(type))
		{
			type = ((VerifierTypeWrapper)type).UnderlyingType;
		}
		if(VerifierTypeWrapper.IsFaultBlockException(type))
		{
			VerifierTypeWrapper.ClearFaultBlockException(type);
			type = CoreClasses.java.lang.Throwable.Wrapper;
		}
		return type;
	}

	internal TypeWrapper PopType(TypeWrapper baseType)
	{
		return InstructionState.PopTypeImpl(baseType, PopAnyType());
	}

	// NOTE this can *not* be used to pop double or long
	internal TypeWrapper PopType()
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

	internal TypeWrapper PopArrayType()
	{
		return InstructionState.PopArrayTypeImpl(PopAnyType());
	}

	// either null or an initialized object reference
	internal TypeWrapper PopObjectType()
	{
		return InstructionState.PopObjectTypeImpl(PopAnyType());
	}

	// null or an initialized object reference derived from baseType (or baseType)
	internal TypeWrapper PopObjectType(TypeWrapper baseType)
	{
		return InstructionState.PopObjectTypeImpl(baseType, PopObjectType());
	}
}

sealed class ExceptionSorter : IComparer<ExceptionTableEntry>
{
	public int Compare(ExceptionTableEntry e1, ExceptionTableEntry e2)
	{
		if (e1.startIndex < e2.startIndex)
		{
			return -1;
		}
		if (e1.startIndex == e2.startIndex)
		{
			if (e1.endIndex == e2.endIndex)
			{
				if (e1.ordinal > e2.ordinal)
				{
					return -1;
				}
				if (e1.ordinal == e2.ordinal)
				{
					return 0;
				}
				return 1;
			}
			if (e1.endIndex > e2.endIndex)
			{
				return -1;
			}
		}
		return 1;
	}
}

struct UntangledExceptionTable
{
	private readonly ExceptionTableEntry[] exceptions;

	internal UntangledExceptionTable(ExceptionTableEntry[] exceptions)
	{
#if DEBUG
		for (int i = 0; i < exceptions.Length; i++)
		{
			for (int j = i + 1; j < exceptions.Length; j++)
			{
				// check for partially overlapping try blocks (which is legal for the JVM, but not the CLR)
				if (exceptions[i].startIndex < exceptions[j].startIndex &&
					exceptions[j].startIndex < exceptions[i].endIndex &&
					exceptions[i].endIndex < exceptions[j].endIndex)
				{
					throw new InvalidOperationException("Partially overlapping try blocks is broken");
				}
				// check that we didn't destroy the ordering, when sorting
				if (exceptions[i].startIndex <= exceptions[j].startIndex &&
					exceptions[i].endIndex >= exceptions[j].endIndex &&
					exceptions[i].ordinal < exceptions[j].ordinal)
				{
					throw new InvalidOperationException("Non recursive try blocks is broken");
				}
			}
		}
#endif
		this.exceptions = exceptions;
	}

	internal ExceptionTableEntry this[int index]
	{
		get { return exceptions[index]; }
	}

	internal int Length
	{
		get { return exceptions.Length; }
	}

	internal void SetFinally(int index)
	{
		exceptions[index] = new ExceptionTableEntry(exceptions[index].startIndex, exceptions[index].endIndex, exceptions[index].handlerIndex, exceptions[index].catch_type, exceptions[index].ordinal, true);
	}
}

struct CodeInfo
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

sealed class MethodAnalyzer
{
	private readonly static TypeWrapper ByteArrayType;
	private readonly static TypeWrapper BooleanArrayType;
	private readonly static TypeWrapper ShortArrayType;
	private readonly static TypeWrapper CharArrayType;
	private readonly static TypeWrapper IntArrayType;
	private readonly static TypeWrapper FloatArrayType;
	private readonly static TypeWrapper DoubleArrayType;
	private readonly static TypeWrapper LongArrayType;
	private readonly static TypeWrapper java_lang_ThreadDeath;
	private readonly TypeWrapper host;	// used to by Unsafe.defineAnonymousClass() to provide access to private members of the host
	private readonly TypeWrapper wrapper;
	private readonly MethodWrapper mw;
	private readonly ClassFile classFile;
	private readonly ClassFile.Method method;
	private readonly ClassLoaderWrapper classLoader;
	private readonly TypeWrapper thisType;
	private readonly InstructionState[] state;
	private List<string> errorMessages;
	private readonly Dictionary<int, TypeWrapper> newTypes = new Dictionary<int, TypeWrapper>();
	private readonly Dictionary<int, TypeWrapper> faultTypes = new Dictionary<int, TypeWrapper>();

	static MethodAnalyzer()
	{
		ByteArrayType = PrimitiveTypeWrapper.BYTE.MakeArrayType(1);
		BooleanArrayType = PrimitiveTypeWrapper.BOOLEAN.MakeArrayType(1);
		ShortArrayType = PrimitiveTypeWrapper.SHORT.MakeArrayType(1);
		CharArrayType = PrimitiveTypeWrapper.CHAR.MakeArrayType(1);
		IntArrayType = PrimitiveTypeWrapper.INT.MakeArrayType(1);
		FloatArrayType = PrimitiveTypeWrapper.FLOAT.MakeArrayType(1);
		DoubleArrayType = PrimitiveTypeWrapper.DOUBLE.MakeArrayType(1);
		LongArrayType = PrimitiveTypeWrapper.LONG.MakeArrayType(1);
		java_lang_ThreadDeath = ClassLoaderWrapper.LoadClassCritical("java.lang.ThreadDeath");
	}

	internal MethodAnalyzer(TypeWrapper host, TypeWrapper wrapper, MethodWrapper mw, ClassFile classFile, ClassFile.Method method, ClassLoaderWrapper classLoader)
	{
		if(method.VerifyError != null)
		{
			throw new VerifyError(method.VerifyError);
		}

		this.host = host;
		this.wrapper = wrapper;
		this.mw = mw;
		this.classFile = classFile;
		this.method = method;
		this.classLoader = classLoader;
		state = new InstructionState[method.Instructions.Length];

		try
		{
			// ensure that exception blocks and handlers start and end at instruction boundaries
			for(int i = 0; i < method.ExceptionTable.Length; i++)
			{
				int start = method.ExceptionTable[i].startIndex;
				int end = method.ExceptionTable[i].endIndex;
				int handler = method.ExceptionTable[i].handlerIndex;
				if(start >= end || start == -1 || end == -1 || handler <= 0)
				{
					throw new IndexOutOfRangeException();
				}
			}
		}
		catch(IndexOutOfRangeException)
		{
			// TODO figure out if we should throw this during class loading
			throw new ClassFormatError(string.Format("Illegal exception table (class: {0}, method: {1}, signature: {2}", classFile.Name, method.Name, method.Signature));
		}

		// start by computing the initial state, the stack is empty and the locals contain the arguments
		state[0] = new InstructionState(method.MaxLocals, method.MaxStack);
		int firstNonArgLocalIndex = 0;
		if(!method.IsStatic)
		{
			thisType = VerifierTypeWrapper.MakeThis(wrapper);
			// this reference. If we're a constructor, the this reference is uninitialized.
			if(method.IsConstructor)
			{
				state[0].SetLocalType(firstNonArgLocalIndex++, VerifierTypeWrapper.UninitializedThis, -1);
				state[0].SetUnitializedThis(true);
			}
			else
			{
				state[0].SetLocalType(firstNonArgLocalIndex++, thisType, -1);
			}
		}
		else
		{
			thisType = null;
		}
		// mw can be null when we're invoked from IsSideEffectFreeStaticInitializer
		TypeWrapper[] argTypeWrappers = mw == null ? TypeWrapper.EmptyArray : mw.GetParameters();
		for(int i = 0; i < argTypeWrappers.Length; i++)
		{
			TypeWrapper type = argTypeWrappers[i];
			if(type.IsIntOnStackPrimitive)
			{
				type = PrimitiveTypeWrapper.INT;
			}
			state[0].SetLocalType(firstNonArgLocalIndex++, type, -1);
			if(type.IsWidePrimitive)
			{
				firstNonArgLocalIndex++;
			}
		}
		AnalyzeTypeFlow();
		VerifyPassTwo();
		PatchLoadConstants();
	}

	private void PatchLoadConstants()
	{
		ClassFile.Method.Instruction[] code = method.Instructions;
		for (int i = 0; i < code.Length; i++)
		{
			if (state[i] != null)
			{
				switch (code[i].NormalizedOpCode)
				{
					case NormalizedByteCode.__ldc:
						switch (GetConstantPoolConstantType(code[i].Arg1))
						{
							case ClassFile.ConstantType.Double:
							case ClassFile.ConstantType.Float:
							case ClassFile.ConstantType.Integer:
							case ClassFile.ConstantType.Long:
							case ClassFile.ConstantType.String:
							case ClassFile.ConstantType.LiveObject:
								code[i].PatchOpCode(NormalizedByteCode.__ldc_nothrow);
								break;
						}
						break;
				}
			}
		}
	}

	internal CodeInfo GetCodeInfoAndErrors(UntangledExceptionTable exceptions, out List<string> errors)
	{
		CodeInfo codeInfo = new CodeInfo(state);
		OptimizationPass(codeInfo, classFile, method, exceptions, wrapper, classLoader);
		PatchHardErrorsAndDynamicMemberAccess(wrapper, mw);
		errors = errorMessages;
		if (AnalyzePotentialFaultBlocks(codeInfo, method, exceptions))
		{
			AnalyzeTypeFlow();
		}
		ConvertFinallyBlocks(codeInfo, method, exceptions);
		return codeInfo;
	}

	private void AnalyzeTypeFlow()
	{
		InstructionState s = new InstructionState(method.MaxLocals, method.MaxStack);
		bool done = false;
		ClassFile.Method.Instruction[] instructions = method.Instructions;
		while(!done)
		{
			done = true;
			for(int i = 0; i < instructions.Length; i++)
			{
				if(state[i] != null && state[i].changed)
				{
					try
					{
						//Console.WriteLine(method.Instructions[i].PC + ": " + method.Instructions[i].OpCode.ToString());
						done = false;
						state[i].changed = false;
						// mark the exception handlers reachable from this instruction
						for(int j = 0; j < method.ExceptionTable.Length; j++)
						{
							if(method.ExceptionTable[j].startIndex <= i && i < method.ExceptionTable[j].endIndex)
							{
								MergeExceptionHandler(j, state[i]);
							}
						}
						state[i].CopyTo(s);
						ClassFile.Method.Instruction instr = instructions[i];
						switch(instr.NormalizedOpCode)
						{
							case NormalizedByteCode.__aload:
							{
								TypeWrapper type = s.GetLocalType(instr.NormalizedArg1);
								if(type == VerifierTypeWrapper.Invalid || type.IsPrimitive)
								{
									throw new VerifyError("Object reference expected");
								}
								s.PushType(type);
								break;
							}
							case NormalizedByteCode.__astore:
							{
								if(VerifierTypeWrapper.IsFaultBlockException(s.PeekType()))
								{
									s.SetLocalType(instr.NormalizedArg1, s.PopFaultBlockException(), i);
									break;
								}
								// NOTE since the reference can be uninitialized, we cannot use PopObjectType
								TypeWrapper type = s.PopType();
								if(type.IsPrimitive)
								{
									throw new VerifyError("Object reference expected");
								}
								s.SetLocalType(instr.NormalizedArg1, type, i);
								break;
							}
							case NormalizedByteCode.__aconst_null:
								s.PushType(VerifierTypeWrapper.Null);
								break;
							case NormalizedByteCode.__aaload:
							{
								s.PopInt();
								TypeWrapper type = s.PopArrayType();
								if(type == VerifierTypeWrapper.Null)
								{
									// if the array is null, we have use null as the element type, because
									// otherwise the rest of the code will not verify correctly
									s.PushType(VerifierTypeWrapper.Null);
								}
								else if(type.IsUnloadable)
								{
									s.PushType(VerifierTypeWrapper.Unloadable);
								}
								else
								{
									type = type.ElementTypeWrapper;
									if(type.IsPrimitive)
									{
										throw new VerifyError("Object array expected");
									}
									s.PushType(type);
								}
								break;
							}
							case NormalizedByteCode.__aastore:
								s.PopObjectType();
								s.PopInt();
								s.PopArrayType();
								// TODO check that elem is assignable to the array
								break;
							case NormalizedByteCode.__baload:
							{
								s.PopInt();
								TypeWrapper type = s.PopArrayType();
								if(!VerifierTypeWrapper.IsNullOrUnloadable(type) &&
									type != ByteArrayType &&
									type != BooleanArrayType)
								{
									throw new VerifyError();
								}
								s.PushInt();
								break;
							}
							case NormalizedByteCode.__bastore:
							{
								s.PopInt();
								s.PopInt();
								TypeWrapper type = s.PopArrayType();
								if(!VerifierTypeWrapper.IsNullOrUnloadable(type) &&
									type != ByteArrayType &&
									type != BooleanArrayType)
								{
									throw new VerifyError();
								}
								break;
							}
							case NormalizedByteCode.__caload:
								s.PopInt();
								s.PopObjectType(CharArrayType);
								s.PushInt();
								break;
							case NormalizedByteCode.__castore:
								s.PopInt();
								s.PopInt();
								s.PopObjectType(CharArrayType);
								break;
							case NormalizedByteCode.__saload:
								s.PopInt();
								s.PopObjectType(ShortArrayType);
								s.PushInt();
								break;
							case NormalizedByteCode.__sastore:
								s.PopInt();
								s.PopInt();
								s.PopObjectType(ShortArrayType);
								break;
							case NormalizedByteCode.__iaload:
								s.PopInt();
								s.PopObjectType(IntArrayType);
								s.PushInt();
								break;
							case NormalizedByteCode.__iastore:
								s.PopInt();
								s.PopInt();
								s.PopObjectType(IntArrayType);
								break;
							case NormalizedByteCode.__laload:
								s.PopInt();
								s.PopObjectType(LongArrayType);
								s.PushLong();
								break;
							case NormalizedByteCode.__lastore:
								s.PopLong();
								s.PopInt();
								s.PopObjectType(LongArrayType);
								break;
							case NormalizedByteCode.__daload:
								s.PopInt();
								s.PopObjectType(DoubleArrayType);
								s.PushDouble();
								break;
							case NormalizedByteCode.__dastore:
								s.PopDouble();
								s.PopInt();
								s.PopObjectType(DoubleArrayType);
								break;
							case NormalizedByteCode.__faload:
								s.PopInt();
								s.PopObjectType(FloatArrayType);
								s.PushFloat();
								break;
							case NormalizedByteCode.__fastore:
								s.PopFloat();
								s.PopInt();
								s.PopObjectType(FloatArrayType);
								break;
							case NormalizedByteCode.__arraylength:
								s.PopArrayType();
								s.PushInt();
								break;
							case NormalizedByteCode.__iconst:
								s.PushInt();
								break;
							case NormalizedByteCode.__if_icmpeq:
							case NormalizedByteCode.__if_icmpne:
							case NormalizedByteCode.__if_icmplt:
							case NormalizedByteCode.__if_icmpge:
							case NormalizedByteCode.__if_icmpgt:
							case NormalizedByteCode.__if_icmple:
								s.PopInt();
								s.PopInt();
								break;
							case NormalizedByteCode.__ifeq:
							case NormalizedByteCode.__ifge:
							case NormalizedByteCode.__ifgt:
							case NormalizedByteCode.__ifle:
							case NormalizedByteCode.__iflt:
							case NormalizedByteCode.__ifne:
								s.PopInt();
								break;
							case NormalizedByteCode.__ifnonnull:
							case NormalizedByteCode.__ifnull:
								// TODO it might be legal to use an unitialized ref here
								s.PopObjectType();
								break;
							case NormalizedByteCode.__if_acmpeq:
							case NormalizedByteCode.__if_acmpne:
								// TODO it might be legal to use an unitialized ref here
								s.PopObjectType();
								s.PopObjectType();
								break;
							case NormalizedByteCode.__getstatic:
							case NormalizedByteCode.__dynamic_getstatic:
								// special support for when we're being called from IsSideEffectFreeStaticInitializer
								if(mw == null)
								{
									switch(GetFieldref(instr.Arg1).Signature[0])
									{
										case 'B':
										case 'Z':
										case 'C':
										case 'S':
										case 'I':
											s.PushInt();
											break;
										case 'F':
											s.PushFloat();
											break;
										case 'D':
											s.PushDouble();
											break;
										case 'J':
											s.PushLong();
											break;
										case 'L':
										case '[':
											throw new VerifyError();
										default:
											throw new InvalidOperationException();
									}
								}
								else
								{
									ClassFile.ConstantPoolItemFieldref cpi = GetFieldref(instr.Arg1);
									if(cpi.GetField() != null && cpi.GetField().FieldTypeWrapper.IsUnloadable)
									{
										s.PushType(cpi.GetField().FieldTypeWrapper);
									}
									else
									{
										s.PushType(cpi.GetFieldType());
									}
								}
								break;
							case NormalizedByteCode.__putstatic:
							case NormalizedByteCode.__dynamic_putstatic:
								// special support for when we're being called from IsSideEffectFreeStaticInitializer
								if(mw == null)
								{
									switch(GetFieldref(instr.Arg1).Signature[0])
									{
										case 'B':
										case 'Z':
										case 'C':
										case 'S':
										case 'I':
											s.PopInt();
											break;
										case 'F':
											s.PopFloat();
											break;
										case 'D':
											s.PopDouble();
											break;
										case 'J':
											s.PopLong();
											break;
										case 'L':
										case '[':
											if(s.PopAnyType() != VerifierTypeWrapper.Null)
											{
												throw new VerifyError();
											}
											break;
										default:
											throw new InvalidOperationException();
									}
								}
								else
								{
									s.PopType(GetFieldref(instr.Arg1).GetFieldType());
								}
								break;
							case NormalizedByteCode.__getfield:
							case NormalizedByteCode.__dynamic_getfield:
							{
								s.PopObjectType(GetFieldref(instr.Arg1).GetClassType());
								ClassFile.ConstantPoolItemFieldref cpi = GetFieldref(instr.Arg1);
								if(cpi.GetField() != null && cpi.GetField().FieldTypeWrapper.IsUnloadable)
								{
									s.PushType(cpi.GetField().FieldTypeWrapper);
								}
								else
								{
									s.PushType(cpi.GetFieldType());
								}
								break;
							}
							case NormalizedByteCode.__putfield:
							case NormalizedByteCode.__dynamic_putfield:
								s.PopType(GetFieldref(instr.Arg1).GetFieldType());
								// putfield is allowed to access the uninitialized this
								if(s.PeekType() == VerifierTypeWrapper.UninitializedThis
									&& wrapper.IsAssignableTo(GetFieldref(instr.Arg1).GetClassType()))
								{
									s.PopType();
								}
								else
								{
									s.PopObjectType(GetFieldref(instr.Arg1).GetClassType());
								}
								break;
							case NormalizedByteCode.__ldc_nothrow:
							case NormalizedByteCode.__ldc:
							{
								switch(GetConstantPoolConstantType(instr.Arg1))
								{
									case ClassFile.ConstantType.Double:
										s.PushDouble();
										break;
									case ClassFile.ConstantType.Float:
										s.PushFloat();
										break;
									case ClassFile.ConstantType.Integer:
										s.PushInt();
										break;
									case ClassFile.ConstantType.Long:
										s.PushLong();
										break;
									case ClassFile.ConstantType.String:
										s.PushType(CoreClasses.java.lang.String.Wrapper);
										break;
									case ClassFile.ConstantType.LiveObject:
										s.PushType(CoreClasses.java.lang.Object.Wrapper);
										break;
									case ClassFile.ConstantType.Class:
										if(classFile.MajorVersion < 49)
										{
											throw new VerifyError("Illegal type in constant pool");
										}
										s.PushType(CoreClasses.java.lang.Class.Wrapper);
										break;
									case ClassFile.ConstantType.MethodHandle:
										s.PushType(CoreClasses.java.lang.invoke.MethodHandle.Wrapper);
										break;
									case ClassFile.ConstantType.MethodType:
										s.PushType(CoreClasses.java.lang.invoke.MethodType.Wrapper);
										break;
									default:
										// NOTE this is not a VerifyError, because it cannot happen (unless we have
										// a bug in ClassFile.GetConstantPoolConstantType)
										throw new InvalidOperationException();
								}
								break;
							}
							case NormalizedByteCode.__clone_array:
							case NormalizedByteCode.__invokevirtual:
							case NormalizedByteCode.__invokespecial:
							case NormalizedByteCode.__invokeinterface:
							case NormalizedByteCode.__invokestatic:
							case NormalizedByteCode.__dynamic_invokevirtual:
							case NormalizedByteCode.__dynamic_invokespecial:
							case NormalizedByteCode.__dynamic_invokeinterface:
							case NormalizedByteCode.__dynamic_invokestatic:
							case NormalizedByteCode.__privileged_invokevirtual:
							case NormalizedByteCode.__privileged_invokespecial:
							case NormalizedByteCode.__privileged_invokestatic:
							case NormalizedByteCode.__methodhandle_invoke:
							case NormalizedByteCode.__methodhandle_link:
							{
								ClassFile.ConstantPoolItemMI cpi = GetMethodref(instr.Arg1);
								TypeWrapper retType = cpi.GetRetType();
								// HACK to allow the result of Unsafe.getObjectVolatile() (on an array)
								// to be used with Unsafe.putObject() we need to propagate the
								// element type here as the return type (instead of object)
								if(cpi.GetMethod() != null
									&& cpi.GetMethod().IsIntrinsic
									&& cpi.Class == "sun.misc.Unsafe"
									&& cpi.Name == "getObjectVolatile"
									&& Intrinsics.IsSupportedArrayTypeForUnsafeOperation(s.GetStackSlot(1)))
								{
									retType = s.GetStackSlot(1).ElementTypeWrapper;
								}
								s.MultiPopAnyType(cpi.GetArgTypes().Length);
								if(instr.NormalizedOpCode != NormalizedByteCode.__invokestatic
									&& instr.NormalizedOpCode != NormalizedByteCode.__dynamic_invokestatic)
								{
									TypeWrapper type = s.PopType();
									if(ReferenceEquals(cpi.Name, StringConstants.INIT))
									{
										// after we've invoked the constructor, the uninitialized references
										// are now initialized
										if(type == VerifierTypeWrapper.UninitializedThis)
										{
											if(s.GetLocalTypeEx(0) == type)
											{
												s.SetLocalType(0, thisType, i);
											}
											s.MarkInitialized(type, wrapper, i);
											s.SetUnitializedThis(false);
										}
										else if(VerifierTypeWrapper.IsNew(type))
										{
											s.MarkInitialized(type, ((VerifierTypeWrapper)type).UnderlyingType, i);
										}
										else
										{
											// This is a VerifyError, but it will be caught by our second pass
										}
									}
								}
								if(retType != PrimitiveTypeWrapper.VOID)
								{
									if(cpi.GetMethod() != null && cpi.GetMethod().ReturnType.IsUnloadable)
									{
										s.PushType(cpi.GetMethod().ReturnType);
									}
									else if(retType == PrimitiveTypeWrapper.DOUBLE)
									{
										s.PushExtendedDouble();
									}
									else if(retType == PrimitiveTypeWrapper.FLOAT)
									{
										s.PushExtendedFloat();
									}
									else
									{
										s.PushType(retType);
									}
								}
								break;
							}
							case NormalizedByteCode.__invokedynamic:
							{
								ClassFile.ConstantPoolItemInvokeDynamic cpi = GetInvokeDynamic(instr.Arg1);
								s.MultiPopAnyType(cpi.GetArgTypes().Length);
								TypeWrapper retType = cpi.GetRetType();
								if (retType != PrimitiveTypeWrapper.VOID)
								{
									if (retType == PrimitiveTypeWrapper.DOUBLE)
									{
										s.PushExtendedDouble();
									}
									else if (retType == PrimitiveTypeWrapper.FLOAT)
									{
										s.PushExtendedFloat();
									}
									else
									{
										s.PushType(retType);
									}
								}
								break;
							}
							case NormalizedByteCode.__goto:
								break;
							case NormalizedByteCode.__istore:
								s.PopInt();
								s.SetLocalInt(instr.NormalizedArg1, i);
								break;
							case NormalizedByteCode.__iload:
								s.GetLocalInt(instr.NormalizedArg1);
								s.PushInt();
								break;
							case NormalizedByteCode.__ineg:
								s.PopInt();
								s.PushInt();
								break;
							case NormalizedByteCode.__iadd:
							case NormalizedByteCode.__isub:
							case NormalizedByteCode.__imul:
							case NormalizedByteCode.__idiv:
							case NormalizedByteCode.__irem:
							case NormalizedByteCode.__iand:
							case NormalizedByteCode.__ior:
							case NormalizedByteCode.__ixor:
							case NormalizedByteCode.__ishl:
							case NormalizedByteCode.__ishr:
							case NormalizedByteCode.__iushr:
								s.PopInt();
								s.PopInt();
								s.PushInt();
								break;
							case NormalizedByteCode.__lneg:
								s.PopLong();
								s.PushLong();
								break;
							case NormalizedByteCode.__ladd:
							case NormalizedByteCode.__lsub:
							case NormalizedByteCode.__lmul:
							case NormalizedByteCode.__ldiv:
							case NormalizedByteCode.__lrem:
							case NormalizedByteCode.__land:
							case NormalizedByteCode.__lor:
							case NormalizedByteCode.__lxor:
								s.PopLong();
								s.PopLong();
								s.PushLong();
								break;
							case NormalizedByteCode.__lshl:
							case NormalizedByteCode.__lshr:
							case NormalizedByteCode.__lushr:
								s.PopInt();
								s.PopLong();
								s.PushLong();
								break;
							case NormalizedByteCode.__fneg:
								if(s.PopFloat())
								{
									s.PushExtendedFloat();
								}
								else
								{
									s.PushFloat();
								}
								break;
							case NormalizedByteCode.__fadd:
							case NormalizedByteCode.__fsub:
							case NormalizedByteCode.__fmul:
							case NormalizedByteCode.__fdiv:
							case NormalizedByteCode.__frem:
								s.PopFloat();
								s.PopFloat();
								s.PushExtendedFloat();
								break;
							case NormalizedByteCode.__dneg:
								if(s.PopDouble())
								{
									s.PushExtendedDouble();
								}
								else
								{
									s.PushDouble();
								}
								break;
							case NormalizedByteCode.__dadd:
							case NormalizedByteCode.__dsub:
							case NormalizedByteCode.__dmul:
							case NormalizedByteCode.__ddiv:
							case NormalizedByteCode.__drem:
								s.PopDouble();
								s.PopDouble();
								s.PushExtendedDouble();
								break;
							case NormalizedByteCode.__new:
							{
								// mark the type, so that we can ascertain that it is a "new object"
								TypeWrapper type;
								if(!newTypes.TryGetValue(i, out type))
								{
									type = GetConstantPoolClassType(instr.Arg1);
									if(type.IsArray)
									{
										throw new VerifyError("Illegal use of array type");
									}
									type = VerifierTypeWrapper.MakeNew(type, i);
									newTypes[i] = type;
								}
								s.PushType(type);
								break;
							}
							case NormalizedByteCode.__multianewarray:
							{
								if(instr.Arg2 < 1)
								{
									throw new VerifyError("Illegal dimension argument");
								}
								for(int j = 0; j < instr.Arg2; j++)
								{
									s.PopInt();
								}
								TypeWrapper type = GetConstantPoolClassType(instr.Arg1);
								if(type.ArrayRank < instr.Arg2)
								{
									throw new VerifyError("Illegal dimension argument");
								}
								s.PushType(type);
								break;
							}
							case NormalizedByteCode.__anewarray:
							{
								s.PopInt();
								TypeWrapper type = GetConstantPoolClassType(instr.Arg1);
								if(type.IsUnloadable)
								{
									s.PushType(new UnloadableTypeWrapper("[" + type.SigName));
								}
								else
								{
									s.PushType(type.MakeArrayType(1));
								}
								break;
							}
							case NormalizedByteCode.__newarray:
								s.PopInt();
							switch(instr.Arg1)
							{
								case 4:
									s.PushType(BooleanArrayType);
									break;
								case 5:
									s.PushType(CharArrayType);
									break;
								case 6:
									s.PushType(FloatArrayType);
									break;
								case 7:
									s.PushType(DoubleArrayType);
									break;
								case 8:
									s.PushType(ByteArrayType);
									break;
								case 9:
									s.PushType(ShortArrayType);
									break;
								case 10:
									s.PushType(IntArrayType);
									break;
								case 11:
									s.PushType(LongArrayType);
									break;
								default:
									throw new VerifyError("Bad type");
							}
								break;
							case NormalizedByteCode.__swap:
							{
								TypeWrapper t1 = s.PopType();
								TypeWrapper t2 = s.PopType();
								s.PushType(t1);
								s.PushType(t2);
								break;
							}
							case NormalizedByteCode.__dup:
							{
								TypeWrapper t = s.PopType();
								s.PushType(t);
								s.PushType(t);
								break;
							}
							case NormalizedByteCode.__dup2:
							{
								TypeWrapper t = s.PopAnyType();
								if(t.IsWidePrimitive || t == VerifierTypeWrapper.ExtendedDouble)
								{
									s.PushType(t);
									s.PushType(t);
								}
								else
								{
									TypeWrapper t2 = s.PopType();
									s.PushType(t2);
									s.PushType(t);
									s.PushType(t2);
									s.PushType(t);
								}
								break;
							}
							case NormalizedByteCode.__dup_x1:
							{
								TypeWrapper value1 = s.PopType();
								TypeWrapper value2 = s.PopType();
								s.PushType(value1);
								s.PushType(value2);
								s.PushType(value1);
								break;
							}
							case NormalizedByteCode.__dup2_x1:
							{
								TypeWrapper value1 = s.PopAnyType();
								if(value1.IsWidePrimitive || value1 == VerifierTypeWrapper.ExtendedDouble)
								{
									TypeWrapper value2 = s.PopType();
									s.PushType(value1);
									s.PushType(value2);
									s.PushType(value1);
								}
								else
								{
									TypeWrapper value2 = s.PopType();
									TypeWrapper value3 = s.PopType();
									s.PushType(value2);
									s.PushType(value1);
									s.PushType(value3);
									s.PushType(value2);
									s.PushType(value1);
								}
								break;
							}
							case NormalizedByteCode.__dup_x2:
							{
								TypeWrapper value1 = s.PopType();
								TypeWrapper value2 = s.PopAnyType();
								if(value2.IsWidePrimitive || value2 == VerifierTypeWrapper.ExtendedDouble)
								{
									s.PushType(value1);
									s.PushType(value2);
									s.PushType(value1);
								}
								else
								{
									TypeWrapper value3 = s.PopType();
									s.PushType(value1);
									s.PushType(value3);
									s.PushType(value2);
									s.PushType(value1);
								}
								break;
							}
							case NormalizedByteCode.__dup2_x2:
							{
								TypeWrapper value1 = s.PopAnyType();
								if(value1.IsWidePrimitive || value1 == VerifierTypeWrapper.ExtendedDouble)
								{
									TypeWrapper value2 = s.PopAnyType();
									if(value2.IsWidePrimitive || value2 == VerifierTypeWrapper.ExtendedDouble)
									{
										// Form 4
										s.PushType(value1);
										s.PushType(value2);
										s.PushType(value1);
									}
									else
									{
										// Form 2
										TypeWrapper value3 = s.PopType();
										s.PushType(value1);
										s.PushType(value3);
										s.PushType(value2);
										s.PushType(value1);
									}
								}
								else
								{
									TypeWrapper value2 = s.PopType();
									TypeWrapper value3 = s.PopAnyType();
									if(value3.IsWidePrimitive || value3 == VerifierTypeWrapper.ExtendedDouble)
									{
										// Form 3
										s.PushType(value2);
										s.PushType(value1);
										s.PushType(value3);
										s.PushType(value2);
										s.PushType(value1);
									}
									else
									{
										// Form 4
										TypeWrapper value4 = s.PopType();
										s.PushType(value2);
										s.PushType(value1);
										s.PushType(value4);
										s.PushType(value3);
										s.PushType(value2);
										s.PushType(value1);
									}
								}
								break;
							}
							case NormalizedByteCode.__pop:
								s.PopType();
								break;
							case NormalizedByteCode.__pop2:
							{
								TypeWrapper type = s.PopAnyType();
								if(!type.IsWidePrimitive && type != VerifierTypeWrapper.ExtendedDouble)
								{
									s.PopType();
								}
								break;
							}
							case NormalizedByteCode.__monitorenter:
							case NormalizedByteCode.__monitorexit:
								// TODO these bytecodes are allowed on an uninitialized object, but
								// we don't support that at the moment...
								s.PopObjectType();
								break;
							case NormalizedByteCode.__return:
								// mw is null if we're called from IsSideEffectFreeStaticInitializer
								if(mw != null)
								{
									if(mw.ReturnType != PrimitiveTypeWrapper.VOID)
									{
										throw new VerifyError("Wrong return type in function");
									}
									// if we're a constructor, make sure we called the base class constructor
									s.CheckUninitializedThis();
								}
								break;
							case NormalizedByteCode.__areturn:
								s.PopObjectType(mw.ReturnType);
								break;
							case NormalizedByteCode.__ireturn:
							{
								s.PopInt();
								if(!mw.ReturnType.IsIntOnStackPrimitive)
								{
									throw new VerifyError("Wrong return type in function");
								}
								break;
							}
							case NormalizedByteCode.__lreturn:
								s.PopLong();
								if(mw.ReturnType != PrimitiveTypeWrapper.LONG)
								{
									throw new VerifyError("Wrong return type in function");
								}
								break;
							case NormalizedByteCode.__freturn:
								s.PopFloat();
								if(mw.ReturnType != PrimitiveTypeWrapper.FLOAT)
								{
									throw new VerifyError("Wrong return type in function");
								}
								break;
							case NormalizedByteCode.__dreturn:
								s.PopDouble();
								if(mw.ReturnType != PrimitiveTypeWrapper.DOUBLE)
								{
									throw new VerifyError("Wrong return type in function");
								}
								break;
							case NormalizedByteCode.__fload:
								s.GetLocalFloat(instr.NormalizedArg1);
								s.PushFloat();
								break;
							case NormalizedByteCode.__fstore:
								s.PopFloat();
								s.SetLocalFloat(instr.NormalizedArg1, i);
								break;
							case NormalizedByteCode.__dload:
								s.GetLocalDouble(instr.NormalizedArg1);
								s.PushDouble();
								break;
							case NormalizedByteCode.__dstore:
								s.PopDouble();
								s.SetLocalDouble(instr.NormalizedArg1, i);
								break;
							case NormalizedByteCode.__lload:
								s.GetLocalLong(instr.NormalizedArg1);
								s.PushLong();
								break;
							case NormalizedByteCode.__lstore:
								s.PopLong();
								s.SetLocalLong(instr.NormalizedArg1, i);
								break;
							case NormalizedByteCode.__lconst_0:
							case NormalizedByteCode.__lconst_1:
								s.PushLong();
								break;
							case NormalizedByteCode.__fconst_0:
							case NormalizedByteCode.__fconst_1:
							case NormalizedByteCode.__fconst_2:
								s.PushFloat();
								break;
							case NormalizedByteCode.__dconst_0:
							case NormalizedByteCode.__dconst_1:
								s.PushDouble();
								break;
							case NormalizedByteCode.__lcmp:
								s.PopLong();
								s.PopLong();
								s.PushInt();
								break;
							case NormalizedByteCode.__fcmpl:
							case NormalizedByteCode.__fcmpg:
								s.PopFloat();
								s.PopFloat();
								s.PushInt();
								break;
							case NormalizedByteCode.__dcmpl:
							case NormalizedByteCode.__dcmpg:
								s.PopDouble();
								s.PopDouble();
								s.PushInt();
								break;
							case NormalizedByteCode.__checkcast:
								s.PopObjectType();
								s.PushType(GetConstantPoolClassType(instr.Arg1));
								break;
							case NormalizedByteCode.__instanceof:
								s.PopObjectType();
								s.PushInt();
								break;
							case NormalizedByteCode.__iinc:
								s.GetLocalInt(instr.Arg1);
								break;
							case NormalizedByteCode.__athrow:
								if (VerifierTypeWrapper.IsFaultBlockException(s.PeekType()))
								{
									s.PopFaultBlockException();
								}
								else
								{
									s.PopObjectType(CoreClasses.java.lang.Throwable.Wrapper);
								}
								break;
							case NormalizedByteCode.__tableswitch:
							case NormalizedByteCode.__lookupswitch:
								s.PopInt();
								break;
							case NormalizedByteCode.__i2b:
								s.PopInt();
								s.PushInt();
								break;
							case NormalizedByteCode.__i2c:
								s.PopInt();
								s.PushInt();
								break;
							case NormalizedByteCode.__i2s:
								s.PopInt();
								s.PushInt();
								break;
							case NormalizedByteCode.__i2l:
								s.PopInt();
								s.PushLong();
								break;
							case NormalizedByteCode.__i2f:
								s.PopInt();
								s.PushFloat();
								break;
							case NormalizedByteCode.__i2d:
								s.PopInt();
								s.PushDouble();
								break;
							case NormalizedByteCode.__l2i:
								s.PopLong();
								s.PushInt();
								break;
							case NormalizedByteCode.__l2f:
								s.PopLong();
								s.PushFloat();
								break;
							case NormalizedByteCode.__l2d:
								s.PopLong();
								s.PushDouble();
								break;
							case NormalizedByteCode.__f2i:
								s.PopFloat();
								s.PushInt();
								break;
							case NormalizedByteCode.__f2l:
								s.PopFloat();
								s.PushLong();
								break;
							case NormalizedByteCode.__f2d:
								s.PopFloat();
								s.PushDouble();
								break;
							case NormalizedByteCode.__d2i:
								s.PopDouble();
								s.PushInt();
								break;
							case NormalizedByteCode.__d2f:
								s.PopDouble();
								s.PushFloat();
								break;
							case NormalizedByteCode.__d2l:
								s.PopDouble();
								s.PushLong();
								break;
							case NormalizedByteCode.__nop:
								if(i + 1 == instructions.Length)
								{
									throw new VerifyError("Falling off the end of the code");
								}
								break;
							case NormalizedByteCode.__static_error:
								break;
							case NormalizedByteCode.__jsr:
							case NormalizedByteCode.__ret:
								throw new VerifyError("Bad instruction");
							default:
								throw new NotImplementedException(instr.NormalizedOpCode.ToString());
						}
						if(s.GetStackHeight() > method.MaxStack)
						{
							throw new VerifyError("Stack size too large");
						}
						for(int j = 0; j < method.ExceptionTable.Length; j++)
						{
							if(method.ExceptionTable[j].endIndex == i + 1)
							{
								MergeExceptionHandler(j, s);
							}
						}
						try
						{
							switch(ByteCodeMetaData.GetFlowControl(instr.NormalizedOpCode))
							{
								case ByteCodeFlowControl.Switch:
									for(int j = 0; j < instr.SwitchEntryCount; j++)
									{
										state[instr.GetSwitchTargetIndex(j)] += s;
									}
									state[instr.DefaultTarget] += s;
									break;
								case ByteCodeFlowControl.CondBranch:
									state[i + 1] += s;
									state[instr.TargetIndex] += s;
									break;
								case ByteCodeFlowControl.Branch:
									state[instr.TargetIndex] += s;
									break;
								case ByteCodeFlowControl.Return:
								case ByteCodeFlowControl.Throw:
									break;
								case ByteCodeFlowControl.Next:
									state[i + 1] += s;
									break;
								default:
									throw new InvalidOperationException();
							}
						}
						catch(IndexOutOfRangeException)
						{
							// we're going to assume that this always means that we have an invalid branch target
							// NOTE because PcIndexMap returns -1 for illegal PCs (in the middle of an instruction) and
							// we always use that value as an index into the state array, any invalid PC will result
							// in an IndexOutOfRangeException
							throw new VerifyError("Illegal target of jump or branch");
						}
					}
					catch(VerifyError x)
					{
						string opcode = instructions[i].NormalizedOpCode.ToString();
						if(opcode.StartsWith("__"))
						{
							opcode = opcode.Substring(2);
						}
						throw new VerifyError(string.Format("{5} (class: {0}, method: {1}, signature: {2}, offset: {3}, instruction: {4})",
							classFile.Name, method.Name, method.Signature, instructions[i].PC, opcode, x.Message), x);
					}
				}
			}
		}
	}

	private void MergeExceptionHandler(int exceptionIndex, InstructionState curr)
	{
		int idx = method.ExceptionTable[exceptionIndex].handlerIndex;
		InstructionState ex = curr.CopyLocals();
		int catch_type = method.ExceptionTable[exceptionIndex].catch_type;
		if (catch_type == 0)
		{
			TypeWrapper tw;
			if (!faultTypes.TryGetValue(idx, out tw))
			{
				tw = VerifierTypeWrapper.MakeFaultBlockException(this, idx);
				faultTypes.Add(idx, tw);
			}
			ex.PushType(tw);
		}
		else
		{
			// TODO if the exception type is unloadable we should consider pushing
			// Throwable as the type and recording a loader constraint
			ex.PushType(GetConstantPoolClassType(catch_type));
		}
		state[idx] += ex;
	}

	// this verification pass must run on the unmodified bytecode stream
	private void VerifyPassTwo()
	{
		ClassFile.Method.Instruction[] instructions = method.Instructions;
		for (int i = 0; i < instructions.Length; i++)
		{
			if (state[i] != null)
			{
				try
				{
					switch (instructions[i].NormalizedOpCode)
					{
						case NormalizedByteCode.__invokeinterface:
						case NormalizedByteCode.__invokespecial:
						case NormalizedByteCode.__invokestatic:
						case NormalizedByteCode.__invokevirtual:
							VerifyInvokePassTwo(i);
							break;
						case NormalizedByteCode.__invokedynamic:
							VerifyInvokeDynamic(i);
							break;
					}
				}
				catch (VerifyError x)
				{
					string opcode = instructions[i].NormalizedOpCode.ToString();
					if (opcode.StartsWith("__"))
					{
						opcode = opcode.Substring(2);
					}
					throw new VerifyError(string.Format("{5} (class: {0}, method: {1}, signature: {2}, offset: {3}, instruction: {4})",
						classFile.Name, method.Name, method.Signature, instructions[i].PC, opcode, x.Message), x);
				}
			}
		}
	}

	private void VerifyInvokePassTwo(int index)
	{
		StackState stack = new StackState(state[index]);
		NormalizedByteCode invoke = method.Instructions[index].NormalizedOpCode;
		ClassFile.ConstantPoolItemMI cpi = GetMethodref(method.Instructions[index].Arg1);
		if ((invoke == NormalizedByteCode.__invokestatic || invoke == NormalizedByteCode.__invokespecial) && classFile.MajorVersion >= 52)
		{
			// invokestatic and invokespecial may be used to invoke interface methods in Java 8
			// but invokespecial can only invoke methods in the current interface or a directly implemented interface
			if (invoke == NormalizedByteCode.__invokespecial && cpi is ClassFile.ConstantPoolItemInterfaceMethodref)
			{
				if (cpi.GetClassType() == host)
				{
					// ok
				}
				else if (cpi.GetClassType() != wrapper && Array.IndexOf(wrapper.Interfaces, cpi.GetClassType()) == -1)
				{
					throw new VerifyError("Bad invokespecial instruction: interface method reference is in an indirect superinterface.");
				}
			}
		}
		else if ((cpi is ClassFile.ConstantPoolItemInterfaceMethodref) != (invoke == NormalizedByteCode.__invokeinterface))
		{
			throw new VerifyError("Illegal constant pool index");
		}
		if (invoke != NormalizedByteCode.__invokespecial && ReferenceEquals(cpi.Name, StringConstants.INIT))
		{
			throw new VerifyError("Must call initializers using invokespecial");
		}
		if (ReferenceEquals(cpi.Name, StringConstants.CLINIT))
		{
			throw new VerifyError("Illegal call to internal method");
		}
		TypeWrapper[] args = cpi.GetArgTypes();
		for (int j = args.Length - 1; j >= 0; j--)
		{
			stack.PopType(args[j]);
		}
		if (invoke == NormalizedByteCode.__invokeinterface)
		{
			int argcount = args.Length + 1;
			for (int j = 0; j < args.Length; j++)
			{
				if (args[j].IsWidePrimitive)
				{
					argcount++;
				}
			}
			if (method.Instructions[index].Arg2 != argcount)
			{
				throw new VerifyError("Inconsistent args size");
			}
		}
		bool isnew = false;
		TypeWrapper thisType;
		if (invoke == NormalizedByteCode.__invokestatic)
		{
			thisType = null;
		}
		else
		{
			thisType = SigTypeToClassName(stack.PeekType(), cpi.GetClassType(), wrapper);
			if (ReferenceEquals(cpi.Name, StringConstants.INIT))
			{
				TypeWrapper type = stack.PopType();
				isnew = VerifierTypeWrapper.IsNew(type);
				if ((isnew && ((VerifierTypeWrapper)type).UnderlyingType != cpi.GetClassType()) ||
					(type == VerifierTypeWrapper.UninitializedThis && cpi.GetClassType() != wrapper.BaseTypeWrapper && cpi.GetClassType() != wrapper) ||
					(!isnew && type != VerifierTypeWrapper.UninitializedThis))
				{
					// TODO oddly enough, Java fails verification for the class without
					// even running the constructor, so maybe constructors are always
					// verified...
					// NOTE when a constructor isn't verifiable, the static initializer
					// doesn't run either
					throw new VerifyError("Call to wrong initialization method");
				}
			}
			else
			{
				if (invoke != NormalizedByteCode.__invokeinterface)
				{
					TypeWrapper refType = stack.PopObjectType();
					TypeWrapper targetType = cpi.GetClassType();
					if (!VerifierTypeWrapper.IsNullOrUnloadable(refType) &&
						!targetType.IsUnloadable &&
						!refType.IsAssignableTo(targetType))
					{
						throw new VerifyError("Incompatible object argument for function call");
					}
					// for invokespecial we also need to make sure we're calling ourself or a base class
					if (invoke == NormalizedByteCode.__invokespecial)
					{
						if (VerifierTypeWrapper.IsNullOrUnloadable(refType))
						{
							// ok
						}
						else if (refType.IsSubTypeOf(wrapper))
						{
							// ok
						}
						else if (host != null && refType.IsSubTypeOf(host))
						{
							// ok
						}
						else
						{
							throw new VerifyError("Incompatible target object for invokespecial");
						}
						if (targetType.IsUnloadable)
						{
							// ok
						}
						else if (wrapper.IsSubTypeOf(targetType))
						{
							// ok
						}
						else if (host != null && host.IsSubTypeOf(targetType))
						{
							// ok
						}
						else
						{
							throw new VerifyError("Invokespecial cannot call subclass methods");
						}
					}
				}
				else /* __invokeinterface */
				{
					// NOTE unlike in the above case, we also allow *any* interface target type
					// regardless of whether it is compatible or not, because if it is not compatible
					// we want an IncompatibleClassChangeError at runtime
					TypeWrapper refType = stack.PopObjectType();
					TypeWrapper targetType = cpi.GetClassType();
					if (!VerifierTypeWrapper.IsNullOrUnloadable(refType)
						&& !targetType.IsUnloadable
						&& !refType.IsAssignableTo(targetType)
						&& !targetType.IsInterface)
					{
						throw new VerifyError("Incompatible object argument for function call");
					}
				}
			}
		}
	}

	private void VerifyInvokeDynamic(int index)
	{
		StackState stack = new StackState(state[index]);
		ClassFile.ConstantPoolItemInvokeDynamic cpi = GetInvokeDynamic(method.Instructions[index].Arg1);
		TypeWrapper[] args = cpi.GetArgTypes();
		for (int j = args.Length - 1; j >= 0; j--)
		{
			stack.PopType(args[j]);
		}
	}

	private static void OptimizationPass(CodeInfo codeInfo, ClassFile classFile, ClassFile.Method method, UntangledExceptionTable exceptions, TypeWrapper wrapper, ClassLoaderWrapper classLoader)
	{
		// Optimization pass
		if (classLoader.RemoveAsserts)
		{
			// While the optimization is general, in practice it never happens that a getstatic is used on a final field,
			// so we only look for this if assert initialization has been optimized out.
			if (classFile.HasAssertions)
			{
				// compute branch targets
				InstructionFlags[] flags = MethodAnalyzer.ComputePartialReachability(codeInfo, method.Instructions, exceptions, 0, false);
				ClassFile.Method.Instruction[] instructions = method.Instructions;
				for (int i = 0; i < instructions.Length; i++)
				{
					if (instructions[i].NormalizedOpCode == NormalizedByteCode.__getstatic
						&& instructions[i + 1].NormalizedOpCode == NormalizedByteCode.__ifne
						&& instructions[i + 1].TargetIndex > i
						&& (flags[i + 1] & InstructionFlags.BranchTarget) == 0)
					{
						ConstantFieldWrapper field = classFile.GetFieldref(instructions[i].Arg1).GetField() as ConstantFieldWrapper;
						if (field != null && field.FieldTypeWrapper == PrimitiveTypeWrapper.BOOLEAN && (bool)field.GetConstantValue())
						{
							// We know the branch will always be taken, so we replace the getstatic/ifne by a goto.
							instructions[i].PatchOpCode(NormalizedByteCode.__goto, instructions[i + 1].TargetIndex);
						}
					}
				}
			}
		}
	}

	private void PatchHardErrorsAndDynamicMemberAccess(TypeWrapper wrapper, MethodWrapper mw)
	{
		// Now we do another pass to find "hard error" instructions
		if(true)
		{
			ClassFile.Method.Instruction[] instructions = method.Instructions;
			for(int i = 0; i < instructions.Length; i++)
			{
				if(state[i] != null)
				{
					StackState stack = new StackState(state[i]);
					switch(instructions[i].NormalizedOpCode)
					{
						case NormalizedByteCode.__invokeinterface:
						case NormalizedByteCode.__invokespecial:
						case NormalizedByteCode.__invokestatic:
						case NormalizedByteCode.__invokevirtual:
							PatchInvoke(wrapper, ref instructions[i], stack);
							break;
						case NormalizedByteCode.__getfield:
						case NormalizedByteCode.__putfield:
						case NormalizedByteCode.__getstatic:
						case NormalizedByteCode.__putstatic:
							PatchFieldAccess(wrapper, mw, ref instructions[i], stack);
							break;
						case NormalizedByteCode.__ldc:
							switch(classFile.GetConstantPoolConstantType(instructions[i].Arg1))
							{
								case ClassFile.ConstantType.Class:
								{
									TypeWrapper tw = classFile.GetConstantPoolClassType(instructions[i].Arg1);
									if(tw.IsUnloadable)
									{
										ConditionalPatchNoClassDefFoundError(ref instructions[i], tw);
									}
									break;
								}
								case ClassFile.ConstantType.MethodType:
								{
									ClassFile.ConstantPoolItemMethodType cpi = classFile.GetConstantPoolConstantMethodType(instructions[i].Arg1);
									TypeWrapper[] args = cpi.GetArgTypes();
									TypeWrapper tw = cpi.GetRetType();
									for(int j = 0; !tw.IsUnloadable && j < args.Length; j++)
									{
										tw = args[j];
									}
									if(tw.IsUnloadable)
									{
										ConditionalPatchNoClassDefFoundError(ref instructions[i], tw);
									}
									break;
								}
								case ClassFile.ConstantType.MethodHandle:
									PatchLdcMethodHandle(ref instructions[i]);
									break;
							}
							break;
						case NormalizedByteCode.__new:
						{
							TypeWrapper tw = classFile.GetConstantPoolClassType(instructions[i].Arg1);
							if(tw.IsUnloadable)
							{
								ConditionalPatchNoClassDefFoundError(ref instructions[i], tw);
							}
							else if(!tw.IsAccessibleFrom(wrapper))
							{
								SetHardError(wrapper.GetClassLoader(), ref instructions[i], HardError.IllegalAccessError, "Try to access class {0} from class {1}", tw.Name, wrapper.Name);
							}
							else if(tw.IsAbstract)
							{
								SetHardError(wrapper.GetClassLoader(), ref instructions[i], HardError.InstantiationError, "{0}", tw.Name);
							}
							break;
						}
						case NormalizedByteCode.__multianewarray:
						case NormalizedByteCode.__anewarray:
						{
							TypeWrapper tw = classFile.GetConstantPoolClassType(instructions[i].Arg1);
							if(tw.IsUnloadable)
							{
								ConditionalPatchNoClassDefFoundError(ref instructions[i], tw);
							}
							else if(!tw.IsAccessibleFrom(wrapper))
							{
								SetHardError(wrapper.GetClassLoader(), ref instructions[i], HardError.IllegalAccessError, "Try to access class {0} from class {1}", tw.Name, wrapper.Name);
							}
							break;
						}
						case NormalizedByteCode.__checkcast:
						case NormalizedByteCode.__instanceof:
						{
							TypeWrapper tw = classFile.GetConstantPoolClassType(instructions[i].Arg1);
							if(tw.IsUnloadable)
							{
								// If the type is unloadable, we always generate the dynamic code
								// (regardless of ClassLoaderWrapper.DisableDynamicBinding), because at runtime,
								// null references should always pass thru without attempting
								// to load the type (for Sun compatibility).
							}
							else if(!tw.IsAccessibleFrom(wrapper))
							{
								SetHardError(wrapper.GetClassLoader(), ref instructions[i], HardError.IllegalAccessError, "Try to access class {0} from class {1}", tw.Name, wrapper.Name);
							}
							break;
						}
						case NormalizedByteCode.__aaload:
						{
							stack.PopInt();
							TypeWrapper tw = stack.PopArrayType();
							if(tw.IsUnloadable)
							{
								ConditionalPatchNoClassDefFoundError(ref instructions[i], tw);
							}
							break;
						}
						case NormalizedByteCode.__aastore:
						{
							stack.PopObjectType();
							stack.PopInt();
							TypeWrapper tw = stack.PopArrayType();
							if(tw.IsUnloadable)
							{
								ConditionalPatchNoClassDefFoundError(ref instructions[i], tw);
							}
							break;
						}
						default:
							break;
					}
				}
			}
		}
	}

	private void PatchLdcMethodHandle(ref ClassFile.Method.Instruction instr)
	{
		ClassFile.ConstantPoolItemMethodHandle cpi = classFile.GetConstantPoolConstantMethodHandle(instr.Arg1);
		if (cpi.GetClassType().IsUnloadable)
		{
			ConditionalPatchNoClassDefFoundError(ref instr, cpi.GetClassType());
		}
		else if (!cpi.GetClassType().IsAccessibleFrom(wrapper))
		{
			SetHardError(wrapper.GetClassLoader(), ref instr, HardError.IllegalAccessError, "tried to access class {0} from class {1}", cpi.Class, wrapper.Name);
		}
		else if (cpi.Kind == ClassFile.RefKind.invokeVirtual
			&& cpi.GetClassType() == CoreClasses.java.lang.invoke.MethodHandle.Wrapper
			&& (cpi.Name == "invoke" || cpi.Name == "invokeExact"))
		{
			// it's allowed to use ldc to create a MethodHandle invoker
		}
		else if (cpi.Member == null
			|| cpi.Member.IsStatic != (cpi.Kind == ClassFile.RefKind.getStatic || cpi.Kind == ClassFile.RefKind.putStatic || cpi.Kind == ClassFile.RefKind.invokeStatic))
		{
			HardError err;
			string msg;
			switch (cpi.Kind)
			{
				case ClassFile.RefKind.getField:
				case ClassFile.RefKind.getStatic:
				case ClassFile.RefKind.putField:
				case ClassFile.RefKind.putStatic:
					err = HardError.NoSuchFieldError;
					msg = cpi.Name;
					break;
				default:
					err = HardError.NoSuchMethodError;
					msg = cpi.Class + "." + cpi.Name + cpi.Signature;
					break;
			}
			SetHardError(wrapper.GetClassLoader(), ref instr, err, msg, cpi.Class, cpi.Name, SigToString(cpi.Signature));
		}
		else if (!cpi.Member.IsAccessibleFrom(cpi.GetClassType(), wrapper, cpi.GetClassType()))
		{
			if (cpi.Member.IsProtected && wrapper.IsSubTypeOf(cpi.Member.DeclaringType))
			{
				// this is allowed, the receiver will be narrowed to current type
			}
			else
			{
				SetHardError(wrapper.GetClassLoader(), ref instr, HardError.IllegalAccessException, "member is private: {0}.{1}/{2}/{3}, from {4}", cpi.Class, cpi.Name, SigToString(cpi.Signature), cpi.Kind, wrapper.Name);
			}
		}
	}

	private static string SigToString(string sig)
	{
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		string sep = "";
		int dims = 0;
		for (int i = 0; i < sig.Length; i++)
		{
			if (sig[i] == '(' || sig[i] == ')')
			{
				sb.Append(sig[i]);
				sep = "";
				continue;
			}
			else if (sig[i] == '[')
			{
				dims++;
				continue;
			}
			sb.Append(sep);
			sep = ",";
			switch (sig[i])
			{
				case 'V':
					sb.Append("void");
					break;
				case 'B':
					sb.Append("byte");
					break;
				case 'Z':
					sb.Append("boolean");
					break;
				case 'S':
					sb.Append("short");
					break;
				case 'C':
					sb.Append("char");
					break;
				case 'I':
					sb.Append("int");
					break;
				case 'J':
					sb.Append("long");
					break;
				case 'F':
					sb.Append("float");
					break;
				case 'D':
					sb.Append("double");
					break;
				case 'L':
					sb.Append(sig, i + 1, sig.IndexOf(';', i + 1) - (i + 1));
					i = sig.IndexOf(';', i + 1);
					break;
			}
			for (; dims != 0; dims--)
			{
				sb.Append("[]");
			}
		}
		return sb.ToString();
	}

	internal static InstructionFlags[] ComputePartialReachability(CodeInfo codeInfo, ClassFile.Method.Instruction[] instructions, UntangledExceptionTable exceptions, int initialInstructionIndex, bool skipFaultBlocks)
	{
		InstructionFlags[] flags = new InstructionFlags[instructions.Length];
		flags[initialInstructionIndex] |= InstructionFlags.Reachable;
		UpdatePartialReachability(flags, codeInfo, instructions, exceptions, skipFaultBlocks);
		return flags;
	}

	private static void UpdatePartialReachability(InstructionFlags[] flags, CodeInfo codeInfo, ClassFile.Method.Instruction[] instructions, UntangledExceptionTable exceptions, bool skipFaultBlocks)
	{
		bool done = false;
		while (!done)
		{
			done = true;
			for (int i = 0; i < instructions.Length; i++)
			{
				if ((flags[i] & (InstructionFlags.Reachable | InstructionFlags.Processed)) == InstructionFlags.Reachable)
				{
					done = false;
					flags[i] |= InstructionFlags.Processed;
					// mark the exception handlers reachable from this instruction
					for (int j = 0; j < exceptions.Length; j++)
					{
						if (exceptions[j].startIndex <= i && i < exceptions[j].endIndex)
						{
							int idx = exceptions[j].handlerIndex;
							if (!skipFaultBlocks || !VerifierTypeWrapper.IsFaultBlockException(codeInfo.GetRawStackTypeWrapper(idx, 0)))
							{
								flags[idx] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
							}
						}
					}
					MarkSuccessors(instructions, flags, i);
				}
			}
		}
	}

	private static void MarkSuccessors(ClassFile.Method.Instruction[] code, InstructionFlags[] flags, int index)
	{
		switch (ByteCodeMetaData.GetFlowControl(code[index].NormalizedOpCode))
		{
			case ByteCodeFlowControl.Switch:
				{
					for (int i = 0; i < code[index].SwitchEntryCount; i++)
					{
						flags[code[index].GetSwitchTargetIndex(i)] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
					}
					flags[code[index].DefaultTarget] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
					break;
				}
			case ByteCodeFlowControl.Branch:
				flags[code[index].TargetIndex] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
				break;
			case ByteCodeFlowControl.CondBranch:
				flags[code[index].TargetIndex] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
				flags[index + 1] |= InstructionFlags.Reachable;
				break;
			case ByteCodeFlowControl.Return:
			case ByteCodeFlowControl.Throw:
				break;
			case ByteCodeFlowControl.Next:
				flags[index + 1] |= InstructionFlags.Reachable;
				break;
			default:
				throw new InvalidOperationException();
		}
	}

	internal static UntangledExceptionTable UntangleExceptionBlocks(ClassFile classFile, ClassFile.Method method)
	{
		ClassFile.Method.Instruction[] instructions = method.Instructions;
		ExceptionTableEntry[] exceptionTable = method.ExceptionTable;
		List<ExceptionTableEntry> ar = new List<ExceptionTableEntry>(exceptionTable);

		// This optimization removes the recursive exception handlers that Java compiler place around
		// the exit of a synchronization block to be "safe" in the face of asynchronous exceptions.
		// (see http://weblog.ikvm.net/PermaLink.aspx?guid=3af9548e-4905-4557-8809-65a205ce2cd6)
		// We can safely remove them since the code we generate for this construct isn't async safe anyway,
		// but there is another reason why this optimization may be slightly controversial. In some
		// pathological cases it can cause observable differences, where the Sun JVM would spin in an
		// infinite loop, but we will throw an exception. However, the perf benefit is large enough to
		// warrant this "incompatibility".
		// Note that there is also code in the exception handler handling code that detects these bytecode
		// sequences to try to compile them into a fault block, instead of an exception handler.
		for (int i = 0; i < ar.Count; i++)
		{
			ExceptionTableEntry ei = ar[i];
			if (ei.startIndex == ei.handlerIndex && ei.catch_type == 0)
			{
				int index = ei.startIndex;
				if (index + 2 < instructions.Length
					&& ei.endIndex == index + 2
					&& instructions[index].NormalizedOpCode == NormalizedByteCode.__aload
					&& instructions[index + 1].NormalizedOpCode == NormalizedByteCode.__monitorexit
					&& instructions[index + 2].NormalizedOpCode == NormalizedByteCode.__athrow)
				{
					// this is the async exception guard that Jikes and the Eclipse Java Compiler produce
					ar.RemoveAt(i);
					i--;
				}
				else if (index + 4 < instructions.Length
					&& ei.endIndex == index + 3
					&& instructions[index].NormalizedOpCode == NormalizedByteCode.__astore
					&& instructions[index + 1].NormalizedOpCode == NormalizedByteCode.__aload
					&& instructions[index + 2].NormalizedOpCode == NormalizedByteCode.__monitorexit
					&& instructions[index + 3].NormalizedOpCode == NormalizedByteCode.__aload
					&& instructions[index + 4].NormalizedOpCode == NormalizedByteCode.__athrow
					&& instructions[index].NormalizedArg1 == instructions[index + 3].NormalizedArg1)
				{
					// this is the async exception guard that javac produces
					ar.RemoveAt(i);
					i--;
				}
				else if (index + 1 < instructions.Length
					&& ei.endIndex == index + 1
					&& instructions[index].NormalizedOpCode == NormalizedByteCode.__astore)
				{
					// this is the finally guard that javac produces
					ar.RemoveAt(i);
					i--;
				}
			}
		}

		// Modern versions of javac split try blocks when the try block contains a return statement.
		// Here we merge these exception blocks again, because it allows us to generate more efficient code.
		for (int i = 0; i < ar.Count - 1; i++)
		{
			if (ar[i].endIndex + 1 == ar[i + 1].startIndex
				&& ar[i].handlerIndex == ar[i + 1].handlerIndex
				&& ar[i].catch_type == ar[i + 1].catch_type
				&& IsReturn(instructions[ar[i].endIndex].NormalizedOpCode))
			{
				ar[i] = new ExceptionTableEntry(ar[i].startIndex, ar[i + 1].endIndex, ar[i].handlerIndex, ar[i].catch_type, ar[i].ordinal);
				ar.RemoveAt(i + 1);
				i--;
			}
		}

	restart:
		for (int i = 0; i < ar.Count; i++)
		{
			ExceptionTableEntry ei = ar[i];
			for (int j = 0; j < ar.Count; j++)
			{
				ExceptionTableEntry ej = ar[j];
				if (ei.startIndex <= ej.startIndex && ej.startIndex < ei.endIndex)
				{
					// 0006/test.j
					if (ej.endIndex > ei.endIndex)
					{
						ExceptionTableEntry emi = new ExceptionTableEntry(ej.startIndex, ei.endIndex, ei.handlerIndex, ei.catch_type, ei.ordinal);
						ExceptionTableEntry emj = new ExceptionTableEntry(ej.startIndex, ei.endIndex, ej.handlerIndex, ej.catch_type, ej.ordinal);
						ei = new ExceptionTableEntry(ei.startIndex, emi.startIndex, ei.handlerIndex, ei.catch_type, ei.ordinal);
						ej = new ExceptionTableEntry(emj.endIndex, ej.endIndex, ej.handlerIndex, ej.catch_type, ej.ordinal);
						ar[i] = ei;
						ar[j] = ej;
						ar.Insert(j, emj);
						ar.Insert(i + 1, emi);
						goto restart;
					}
					// 0007/test.j
					else if (j > i && ej.endIndex < ei.endIndex)
					{
						ExceptionTableEntry emi = new ExceptionTableEntry(ej.startIndex, ej.endIndex, ei.handlerIndex, ei.catch_type, ei.ordinal);
						ExceptionTableEntry eei = new ExceptionTableEntry(ej.endIndex, ei.endIndex, ei.handlerIndex, ei.catch_type, ei.ordinal);
						ei = new ExceptionTableEntry(ei.startIndex, emi.startIndex, ei.handlerIndex, ei.catch_type, ei.ordinal);
						ar[i] = ei;
						ar.Insert(i + 1, eei);
						ar.Insert(i + 1, emi);
						goto restart;
					}
				}
			}
		}
	// Split try blocks at branch targets (branches from outside the try block)
	restart_split:
		for (int i = 0; i < ar.Count; i++)
		{
			ExceptionTableEntry ei = ar[i];
			int start = ei.startIndex;
			int end = ei.endIndex;
			for (int j = 0; j < instructions.Length; j++)
			{
				if (j < start || j >= end)
				{
					switch (instructions[j].NormalizedOpCode)
					{
						case NormalizedByteCode.__tableswitch:
						case NormalizedByteCode.__lookupswitch:
							// start at -1 to have an opportunity to handle the default offset
							for (int k = -1; k < instructions[j].SwitchEntryCount; k++)
							{
								int targetIndex = (k == -1 ? instructions[j].DefaultTarget : instructions[j].GetSwitchTargetIndex(k));
								if (ei.startIndex < targetIndex && targetIndex < ei.endIndex)
								{
									ExceptionTableEntry en = new ExceptionTableEntry(targetIndex, ei.endIndex, ei.handlerIndex, ei.catch_type, ei.ordinal);
									ei = new ExceptionTableEntry(ei.startIndex, targetIndex, ei.handlerIndex, ei.catch_type, ei.ordinal);
									ar[i] = ei;
									ar.Insert(i + 1, en);
									goto restart_split;
								}
							}
							break;
						case NormalizedByteCode.__ifeq:
						case NormalizedByteCode.__ifne:
						case NormalizedByteCode.__iflt:
						case NormalizedByteCode.__ifge:
						case NormalizedByteCode.__ifgt:
						case NormalizedByteCode.__ifle:
						case NormalizedByteCode.__if_icmpeq:
						case NormalizedByteCode.__if_icmpne:
						case NormalizedByteCode.__if_icmplt:
						case NormalizedByteCode.__if_icmpge:
						case NormalizedByteCode.__if_icmpgt:
						case NormalizedByteCode.__if_icmple:
						case NormalizedByteCode.__if_acmpeq:
						case NormalizedByteCode.__if_acmpne:
						case NormalizedByteCode.__ifnull:
						case NormalizedByteCode.__ifnonnull:
						case NormalizedByteCode.__goto:
							{
								int targetIndex = instructions[j].Arg1;
								if (ei.startIndex < targetIndex && targetIndex < ei.endIndex)
								{
									ExceptionTableEntry en = new ExceptionTableEntry(targetIndex, ei.endIndex, ei.handlerIndex, ei.catch_type, ei.ordinal);
									ei = new ExceptionTableEntry(ei.startIndex, targetIndex, ei.handlerIndex, ei.catch_type, ei.ordinal);
									ar[i] = ei;
									ar.Insert(i + 1, en);
									goto restart_split;
								}
								break;
							}
					}
				}
			}
		}
		// exception handlers are also a kind of jump, so we need to split try blocks around handlers as well
		for (int i = 0; i < ar.Count; i++)
		{
			ExceptionTableEntry ei = ar[i];
			for (int j = 0; j < ar.Count; j++)
			{
				ExceptionTableEntry ej = ar[j];
				if (ei.startIndex < ej.handlerIndex && ej.handlerIndex < ei.endIndex)
				{
					ExceptionTableEntry en = new ExceptionTableEntry(ej.handlerIndex, ei.endIndex, ei.handlerIndex, ei.catch_type, ei.ordinal);
					ei = new ExceptionTableEntry(ei.startIndex, ej.handlerIndex, ei.handlerIndex, ei.catch_type, ei.ordinal);
					ar[i] = ei;
					ar.Insert(i + 1, en);
					goto restart_split;
				}
			}
		}
		// filter out zero length try blocks
		for (int i = 0; i < ar.Count; i++)
		{
			ExceptionTableEntry ei = ar[i];
			if (ei.startIndex == ei.endIndex)
			{
				ar.RemoveAt(i);
				i--;
			}
			else
			{
				// exception blocks that only contain harmless instructions (i.e. instructions that will *never* throw an exception)
				// are also filtered out (to improve the quality of the generated code)
				TypeWrapper exceptionType = ei.catch_type == 0 ? CoreClasses.java.lang.Throwable.Wrapper : classFile.GetConstantPoolClassType(ei.catch_type);
				if (exceptionType.IsUnloadable)
				{
					// we can't remove handlers for unloadable types
				}
				else if (java_lang_ThreadDeath.IsAssignableTo(exceptionType))
				{
					// We only remove exception handlers that could catch ThreadDeath in limited cases, because it can be thrown
					// asynchronously (and thus appear on any instruction). This is particularly important to ensure that
					// we run finally blocks when a thread is killed.
					// Note that even so, we aren't remotely async exception safe.
					int start = ei.startIndex;
					int end = ei.endIndex;
					for (int j = start; j < end; j++)
					{
						switch (instructions[j].NormalizedOpCode)
						{
							case NormalizedByteCode.__aload:
							case NormalizedByteCode.__iload:
							case NormalizedByteCode.__lload:
							case NormalizedByteCode.__fload:
							case NormalizedByteCode.__dload:
							case NormalizedByteCode.__astore:
							case NormalizedByteCode.__istore:
							case NormalizedByteCode.__lstore:
							case NormalizedByteCode.__fstore:
							case NormalizedByteCode.__dstore:
								break;
							case NormalizedByteCode.__dup:
							case NormalizedByteCode.__dup_x1:
							case NormalizedByteCode.__dup_x2:
							case NormalizedByteCode.__dup2:
							case NormalizedByteCode.__dup2_x1:
							case NormalizedByteCode.__dup2_x2:
							case NormalizedByteCode.__pop:
							case NormalizedByteCode.__pop2:
								break;
							case NormalizedByteCode.__return:
							case NormalizedByteCode.__areturn:
							case NormalizedByteCode.__ireturn:
							case NormalizedByteCode.__lreturn:
							case NormalizedByteCode.__freturn:
							case NormalizedByteCode.__dreturn:
								break;
							case NormalizedByteCode.__goto:
								// if there is a branch that stays inside the block, we should keep the block
								if (start <= instructions[j].TargetIndex && instructions[j].TargetIndex < end)
									goto next;
								break;
							default:
								goto next;
						}
					}
					ar.RemoveAt(i);
					i--;
				}
				else
				{
					int start = ei.startIndex;
					int end = ei.endIndex;
					for (int j = start; j < end; j++)
					{
						if (ByteCodeMetaData.CanThrowException(instructions[j].NormalizedOpCode))
						{
							goto next;
						}
					}
					ar.RemoveAt(i);
					i--;
				}
			}
		next: ;
		}

		ExceptionTableEntry[] exceptions = ar.ToArray();
		Array.Sort(exceptions, new ExceptionSorter());

		return new UntangledExceptionTable(exceptions);
	}

	private static bool IsReturn(NormalizedByteCode bc)
	{
		return bc == NormalizedByteCode.__return
			|| bc == NormalizedByteCode.__areturn
			|| bc == NormalizedByteCode.__dreturn
			|| bc == NormalizedByteCode.__ireturn
			|| bc == NormalizedByteCode.__freturn
			|| bc == NormalizedByteCode.__lreturn;
	}

	private static bool AnalyzePotentialFaultBlocks(CodeInfo codeInfo, ClassFile.Method method, UntangledExceptionTable exceptions)
	{
		ClassFile.Method.Instruction[] code = method.Instructions;
		bool changed = false;
		bool done = false;
		while (!done)
		{
			done = true;
			Stack<ExceptionTableEntry> stack = new Stack<ExceptionTableEntry>();
			ExceptionTableEntry current = new ExceptionTableEntry(0, code.Length, -1, ushort.MaxValue, -1);
			stack.Push(current);
			for (int i = 0; i < exceptions.Length; i++)
			{
				while (exceptions[i].startIndex >= current.endIndex)
				{
					current = stack.Pop();
				}
				Debug.Assert(exceptions[i].startIndex >= current.startIndex && exceptions[i].endIndex <= current.endIndex);
				if (exceptions[i].catch_type == 0
					&& codeInfo.HasState(exceptions[i].handlerIndex)
					&& VerifierTypeWrapper.IsFaultBlockException(codeInfo.GetRawStackTypeWrapper(exceptions[i].handlerIndex, 0)))
				{
					InstructionFlags[] flags = MethodAnalyzer.ComputePartialReachability(codeInfo, method.Instructions, exceptions, exceptions[i].handlerIndex, true);
					for (int j = 0; j < code.Length; j++)
					{
						if ((flags[j] & InstructionFlags.Reachable) != 0)
						{
							switch (code[j].NormalizedOpCode)
							{
								case NormalizedByteCode.__return:
								case NormalizedByteCode.__areturn:
								case NormalizedByteCode.__ireturn:
								case NormalizedByteCode.__lreturn:
								case NormalizedByteCode.__freturn:
								case NormalizedByteCode.__dreturn:
									goto not_fault_block;
								case NormalizedByteCode.__athrow:
									for (int k = i + 1; k < exceptions.Length; k++)
									{
										if (exceptions[k].startIndex <= j && j < exceptions[k].endIndex)
										{
											goto not_fault_block;
										}
									}
									if (VerifierTypeWrapper.IsFaultBlockException(codeInfo.GetRawStackTypeWrapper(j, 0))
										&& codeInfo.GetRawStackTypeWrapper(j, 0) != codeInfo.GetRawStackTypeWrapper(exceptions[i].handlerIndex, 0))
									{
										goto not_fault_block;
									}
									break;
							}
							if (j < current.startIndex || j >= current.endIndex)
							{
								goto not_fault_block;
							}
							else if (exceptions[i].startIndex <= j && j < exceptions[i].endIndex)
							{
								goto not_fault_block;
							}
							else
							{
								continue;
							}
						not_fault_block:
							VerifierTypeWrapper.ClearFaultBlockException(codeInfo.GetRawStackTypeWrapper(exceptions[i].handlerIndex, 0));
							done = false;
							changed = true;
							break;
						}
					}
				}
				stack.Push(current);
				current = exceptions[i];
			}
		}
		return changed;
	}

	private static void ConvertFinallyBlocks(CodeInfo codeInfo, ClassFile.Method method, UntangledExceptionTable exceptions)
	{
		ClassFile.Method.Instruction[] code = method.Instructions;
		InstructionFlags[] flags = ComputePartialReachability(codeInfo, code, exceptions, 0, false);
		for (int i = 0; i < exceptions.Length; i++)
		{
			if (exceptions[i].catch_type == 0
				&& codeInfo.HasState(exceptions[i].handlerIndex)
				&& VerifierTypeWrapper.IsFaultBlockException(codeInfo.GetRawStackTypeWrapper(exceptions[i].handlerIndex, 0)))
			{
				int exit;
				if (IsSynchronizedBlockHandler(code, exceptions[i].handlerIndex)
					&& exceptions[i].endIndex - 2 >= exceptions[i].startIndex
					&& TryFindSingleTryBlockExit(code, flags, exceptions, new ExceptionTableEntry(exceptions[i].startIndex, exceptions[i].endIndex - 2, exceptions[i].handlerIndex, 0, exceptions[i].ordinal), i, out exit)
					&& exit == exceptions[i].endIndex - 2
					&& (flags[exit + 1] & InstructionFlags.BranchTarget) == 0
					&& MatchInstructions(code, exit, exceptions[i].handlerIndex + 1)
					&& MatchInstructions(code, exit + 1, exceptions[i].handlerIndex + 2)
					&& MatchExceptionCoverage(exceptions, i, exceptions[i].handlerIndex + 1, exceptions[i].handlerIndex + 3, exit, exit + 2)
					&& exceptions[i].handlerIndex <= ushort.MaxValue)
				{
					code[exit].PatchOpCode(NormalizedByteCode.__goto_finally, exceptions[i].endIndex, (short)exceptions[i].handlerIndex);
					exceptions.SetFinally(i);
				}
				else if (TryFindSingleTryBlockExit(code, flags, exceptions, exceptions[i], i, out exit)
					// the stack must be empty
					&& codeInfo.GetStackHeight(exit) == 0
					// the exit code must not be reachable (except from within the try-block),
					// because we're going to patch it to jump around the exit code
					&& !IsReachableFromOutsideTryBlock(codeInfo, code, exceptions, exceptions[i], exit))
				{
					int exitHandlerEnd;
					int faultHandlerEnd;
					if (MatchFinallyBlock(codeInfo, code, exceptions, exceptions[i].handlerIndex, exit, out exitHandlerEnd, out faultHandlerEnd))
					{
						if (exit != exitHandlerEnd
							&& codeInfo.GetStackHeight(exitHandlerEnd) == 0
							&& MatchExceptionCoverage(exceptions, -1, exceptions[i].handlerIndex, faultHandlerEnd, exit, exitHandlerEnd))
						{
							// We use Arg2 (which is a short) to store the handler in the __goto_finally pseudo-opcode,
							// so we can only do that if handlerIndex fits in a short (note that we can use the sign bit too).
							if (exceptions[i].handlerIndex <= ushort.MaxValue)
							{
								code[exit].PatchOpCode(NormalizedByteCode.__goto_finally, exitHandlerEnd, (short)exceptions[i].handlerIndex);
								exceptions.SetFinally(i);
							}
						}
					}
				}
			}
		}
	}

	private static bool IsSynchronizedBlockHandler(ClassFile.Method.Instruction[] code, int index)
	{
		return code[index].NormalizedOpCode == NormalizedByteCode.__astore
			&& code[index + 1].NormalizedOpCode == NormalizedByteCode.__aload
			&& code[index + 2].NormalizedOpCode == NormalizedByteCode.__monitorexit
			&& code[index + 3].NormalizedOpCode == NormalizedByteCode.__aload && code[index + 3].Arg1 == code[index].Arg1
			&& code[index + 4].NormalizedOpCode == NormalizedByteCode.__athrow;
	}

	private static bool MatchExceptionCoverage(UntangledExceptionTable exceptions, int skipException, int startFault, int endFault, int startExit, int endExit)
	{
		for (int j = 0; j < exceptions.Length; j++)
		{
			if (j != skipException && ExceptionCovers(exceptions[j], startFault, endFault) != ExceptionCovers(exceptions[j], startExit, endExit))
			{
				return false;
			}
		}
		return true;
	}

	private static bool ExceptionCovers(ExceptionTableEntry exception, int start, int end)
	{
		return exception.startIndex < end && exception.endIndex > start;
	}

	private static bool MatchFinallyBlock(CodeInfo codeInfo, ClassFile.Method.Instruction[] code, UntangledExceptionTable exceptions, int faultHandler, int exitHandler, out int exitHandlerEnd, out int faultHandlerEnd)
	{
		exitHandlerEnd = -1;
		faultHandlerEnd = -1;
		if (code[faultHandler].NormalizedOpCode != NormalizedByteCode.__astore)
		{
			return false;
		}
		int startFault = faultHandler;
		int faultLocal = code[faultHandler++].NormalizedArg1;
		for (; ; )
		{
			if (code[faultHandler].NormalizedOpCode == NormalizedByteCode.__aload
				&& code[faultHandler].NormalizedArg1 == faultLocal
				&& code[faultHandler + 1].NormalizedOpCode == NormalizedByteCode.__athrow)
			{
				// make sure that instructions that we haven't covered aren't reachable
				InstructionFlags[] flags = ComputePartialReachability(codeInfo, code, exceptions, startFault, false);
				for (int i = 0; i < flags.Length; i++)
				{
					if ((i < startFault || i > faultHandler + 1) && (flags[i] & InstructionFlags.Reachable) != 0)
					{
						return false;
					}
				}
				exitHandlerEnd = exitHandler;
				faultHandlerEnd = faultHandler;
				return true;
			}
			if (!MatchInstructions(code, faultHandler, exitHandler))
			{
				return false;
			}
			faultHandler++;
			exitHandler++;
		}
	}

	private static bool MatchInstructions(ClassFile.Method.Instruction[] code, int i, int j)
	{
		if (code[i].NormalizedOpCode != code[j].NormalizedOpCode)
		{
			return false;
		}
		switch (ByteCodeMetaData.GetFlowControl(code[i].NormalizedOpCode))
		{
			case ByteCodeFlowControl.Branch:
			case ByteCodeFlowControl.CondBranch:
				if (code[i].Arg1 - i != code[j].Arg1 - j)
				{
					return false;
				}
				break;
			case ByteCodeFlowControl.Switch:
				if (code[i].SwitchEntryCount != code[j].SwitchEntryCount)
				{
					return false;
				}
				for (int k = 0; k < code[i].SwitchEntryCount; k++)
				{
					if (code[i].GetSwitchTargetIndex(k) != code[j].GetSwitchTargetIndex(k))
					{
						return false;
					}
				}
				if (code[i].DefaultTarget != code[j].DefaultTarget)
				{
					return false;
				}
				break;
			default:
				if (code[i].Arg1 != code[j].Arg1)
				{
					return false;
				}
				if (code[i].Arg2 != code[j].Arg2)
				{
					return false;
				}
				break;
		}
		return true;
	}

	private static bool IsReachableFromOutsideTryBlock(CodeInfo codeInfo, ClassFile.Method.Instruction[] code, UntangledExceptionTable exceptions, ExceptionTableEntry tryBlock, int instructionIndex)
	{
		InstructionFlags[] flags = new InstructionFlags[code.Length];
		flags[0] |= InstructionFlags.Reachable;
		// We mark the first instruction of the try-block as already processed, so that UpdatePartialReachability will skip the try-block.
		// Note that we can do this, because it is not possible to jump into the middle of a try-block (after the exceptions have been untangled).
		flags[tryBlock.startIndex] = InstructionFlags.Processed;
		// We mark the successor instructions of the instruction we're examinining as reachable,
		// to figure out if the code following the handler somehow branches back to it.
		MarkSuccessors(code, flags, instructionIndex);
		UpdatePartialReachability(flags, codeInfo, code, exceptions, false);
		return (flags[instructionIndex] & InstructionFlags.Reachable) != 0;
	}

	private static bool TryFindSingleTryBlockExit(ClassFile.Method.Instruction[] code, InstructionFlags[] flags, UntangledExceptionTable exceptions, ExceptionTableEntry exception, int exceptionIndex, out int exit)
	{
		exit = -1;
		bool fail = false;
		bool nextIsReachable = false;
		for (int i = exception.startIndex; !fail && i < exception.endIndex; i++)
		{
			if ((flags[i] & InstructionFlags.Reachable) != 0)
			{
				nextIsReachable = false;
				for (int j = 0; j < exceptions.Length; j++)
				{
					if (j != exceptionIndex && exceptions[j].startIndex >= exception.startIndex && exception.endIndex <= exceptions[j].endIndex)
					{
						UpdateTryBlockExit(exception, exceptions[j].handlerIndex, ref exit, ref fail);
					}
				}
				switch (ByteCodeMetaData.GetFlowControl(code[i].NormalizedOpCode))
				{
					case ByteCodeFlowControl.Switch:
						{
							for (int j = 0; j < code[i].SwitchEntryCount; j++)
							{
								UpdateTryBlockExit(exception, code[i].GetSwitchTargetIndex(j), ref exit, ref fail);
							}
							UpdateTryBlockExit(exception, code[i].DefaultTarget, ref exit, ref fail);
							break;
						}
					case ByteCodeFlowControl.Branch:
						UpdateTryBlockExit(exception, code[i].TargetIndex, ref exit, ref fail);
						break;
					case ByteCodeFlowControl.CondBranch:
						UpdateTryBlockExit(exception, code[i].TargetIndex, ref exit, ref fail);
						nextIsReachable = true;
						break;
					case ByteCodeFlowControl.Return:
						fail = true;
						break;
					case ByteCodeFlowControl.Throw:
						break;
					case ByteCodeFlowControl.Next:
						nextIsReachable = true;
						break;
					default:
						throw new InvalidOperationException();
				}
			}
		}
		if (nextIsReachable)
		{
			UpdateTryBlockExit(exception, exception.endIndex, ref exit, ref fail);
		}
		return !fail && exit != -1;
	}

	private static void UpdateTryBlockExit(ExceptionTableEntry exception, int targetIndex, ref int exitIndex, ref bool fail)
	{
		if (exception.startIndex <= targetIndex && targetIndex < exception.endIndex)
		{
			// branch stays inside try block
		}
		else if (exitIndex == -1)
		{
			exitIndex = targetIndex;
		}
		else if (exitIndex != targetIndex)
		{
			fail = true;
		}
	}

	private void ConditionalPatchNoClassDefFoundError(ref ClassFile.Method.Instruction instruction, TypeWrapper tw)
	{
		ClassLoaderWrapper loader = wrapper.GetClassLoader();
		if (loader.DisableDynamicBinding)
		{
			SetHardError(loader, ref instruction, HardError.NoClassDefFoundError, "{0}", tw.Name);
		}
	}

	private void SetHardError(ClassLoaderWrapper classLoader, ref ClassFile.Method.Instruction instruction, HardError hardError, string message, params object[] args)
	{
		string text = string.Format(message, args);
#if STATIC_COMPILER
		Message msg;
		switch (hardError)
		{
			case HardError.NoClassDefFoundError:
				msg = Message.EmittedNoClassDefFoundError;
				break;
			case HardError.IllegalAccessError:
				msg = Message.EmittedIllegalAccessError;
				break;
			case HardError.InstantiationError:
				msg = Message.EmittedIllegalAccessError;
				break;
			case HardError.IncompatibleClassChangeError:
			case HardError.IllegalAccessException:
				msg = Message.EmittedIncompatibleClassChangeError;
				break;
			case HardError.NoSuchFieldError:
				msg = Message.EmittedNoSuchFieldError;
				break;
			case HardError.AbstractMethodError:
				msg = Message.EmittedAbstractMethodError;
				break;
			case HardError.NoSuchMethodError:
				msg = Message.EmittedNoSuchMethodError;
				break;
			case HardError.LinkageError:
				msg = Message.EmittedLinkageError;
				break;
			default:
				throw new InvalidOperationException();
		}
		classLoader.IssueMessage(msg, classFile.Name + "." + method.Name + method.Signature, text);
#endif
		instruction.SetHardError(hardError, AllocErrorMessage(text));
	}

	private void PatchInvoke(TypeWrapper wrapper, ref ClassFile.Method.Instruction instr, StackState stack)
	{
		ClassFile.ConstantPoolItemMI cpi = GetMethodref(instr.Arg1);
		NormalizedByteCode invoke = instr.NormalizedOpCode;
		bool isnew = false;
		TypeWrapper thisType;
		if (invoke == NormalizedByteCode.__invokevirtual
			&& cpi.Class == "java.lang.invoke.MethodHandle"
			&& (cpi.Name == "invoke" || cpi.Name == "invokeExact" || cpi.Name == "invokeBasic"))
		{
			if (cpi.GetArgTypes().Length > 127 && MethodHandleUtil.SlotCount(cpi.GetArgTypes()) > 254)
			{
				instr.SetHardError(HardError.LinkageError, AllocErrorMessage("bad parameter count"));
				return;
			}
			instr.PatchOpCode(NormalizedByteCode.__methodhandle_invoke);
			return;
		}
		else if (invoke == NormalizedByteCode.__invokestatic
			&& cpi.Class == "java.lang.invoke.MethodHandle"
			&& (cpi.Name == "linkToVirtual" || cpi.Name == "linkToStatic" || cpi.Name == "linkToSpecial" || cpi.Name == "linkToInterface")
			&& CoreClasses.java.lang.invoke.MethodHandle.Wrapper.IsPackageAccessibleFrom(wrapper))
		{
			instr.PatchOpCode(NormalizedByteCode.__methodhandle_link);
			return;
		}
		else if (invoke == NormalizedByteCode.__invokestatic)
		{
			thisType = null;
		}
		else
		{
			TypeWrapper[] args = cpi.GetArgTypes();
			for (int j = args.Length - 1; j >= 0; j--)
			{
				stack.PopType(args[j]);
			}
			thisType = SigTypeToClassName(stack.PeekType(), cpi.GetClassType(), wrapper);
			if(ReferenceEquals(cpi.Name, StringConstants.INIT))
			{
				TypeWrapper type = stack.PopType();
				isnew = VerifierTypeWrapper.IsNew(type);
			}
		}

		if(cpi.GetClassType().IsUnloadable)
		{
			if(wrapper.GetClassLoader().DisableDynamicBinding)
			{
				SetHardError(wrapper.GetClassLoader(), ref instr, HardError.NoClassDefFoundError, "{0}", cpi.GetClassType().Name);
			}
			else
			{
				switch(invoke)
				{
					case NormalizedByteCode.__invokeinterface:
						instr.PatchOpCode(NormalizedByteCode.__dynamic_invokeinterface);
						break;
					case NormalizedByteCode.__invokestatic:
						instr.PatchOpCode(NormalizedByteCode.__dynamic_invokestatic);
						break;
					case NormalizedByteCode.__invokevirtual:
						instr.PatchOpCode(NormalizedByteCode.__dynamic_invokevirtual);
						break;
					case NormalizedByteCode.__invokespecial:
						if(isnew)
						{
							instr.PatchOpCode(NormalizedByteCode.__dynamic_invokespecial);
						}
						else
						{
							throw new VerifyError("Invokespecial cannot call subclass methods");
						}
						break;
					default:
						throw new InvalidOperationException();
				}
			}
		}
		else if(invoke == NormalizedByteCode.__invokeinterface && !cpi.GetClassType().IsInterface)
		{
			SetHardError(wrapper.GetClassLoader(), ref instr, HardError.IncompatibleClassChangeError, "invokeinterface on non-interface");
		}
		else if(cpi.GetClassType().IsInterface && invoke != NormalizedByteCode.__invokeinterface && ((invoke != NormalizedByteCode.__invokestatic && invoke != NormalizedByteCode.__invokespecial) || classFile.MajorVersion < 52))
		{
			SetHardError(wrapper.GetClassLoader(), ref instr, HardError.IncompatibleClassChangeError,
				classFile.MajorVersion < 52
					? "interface method must be invoked using invokeinterface"
					: "interface method must be invoked using invokeinterface, invokespecial or invokestatic");
		}
		else
		{
			MethodWrapper targetMethod = invoke == NormalizedByteCode.__invokespecial ? cpi.GetMethodForInvokespecial() : cpi.GetMethod();
			if(targetMethod != null)
			{
				string errmsg = CheckLoaderConstraints(cpi, targetMethod);
				if(errmsg != null)
				{
					SetHardError(wrapper.GetClassLoader(), ref instr, HardError.LinkageError, "{0}", errmsg);
				}
				else if(targetMethod.IsStatic == (invoke == NormalizedByteCode.__invokestatic))
				{
					if(targetMethod.IsAbstract && invoke == NormalizedByteCode.__invokespecial && (targetMethod.GetMethod() == null || targetMethod.GetMethod().IsAbstract))
					{
						SetHardError(wrapper.GetClassLoader(), ref instr, HardError.AbstractMethodError, "{0}.{1}{2}", cpi.Class, cpi.Name, cpi.Signature);
					}
					else if(invoke == NormalizedByteCode.__invokeinterface && targetMethod.IsPrivate)
					{
						SetHardError(wrapper.GetClassLoader(), ref instr, HardError.IncompatibleClassChangeError, "private interface method requires invokespecial, not invokeinterface: method {0}.{1}{2}", cpi.Class, cpi.Name, cpi.Signature);
					}
					else if(targetMethod.IsAccessibleFrom(cpi.GetClassType(), wrapper, thisType))
					{
						return;
					}
					else if(host != null && targetMethod.IsAccessibleFrom(cpi.GetClassType(), host, thisType))
					{
						switch (invoke)
						{
							case NormalizedByteCode.__invokespecial:
								instr.PatchOpCode(NormalizedByteCode.__privileged_invokespecial);
								break;
							case NormalizedByteCode.__invokestatic:
								instr.PatchOpCode(NormalizedByteCode.__privileged_invokestatic);
								break;
							case NormalizedByteCode.__invokevirtual:
								instr.PatchOpCode(NormalizedByteCode.__privileged_invokevirtual);
								break;
							default:
								throw new InvalidOperationException();
						}
						return;
					}
					else
					{
						// NOTE special case for incorrect invocation of Object.clone(), because this could mean
						// we're calling clone() on an array
						// (bug in javac, see http://developer.java.sun.com/developer/bugParade/bugs/4329886.html)
						if(cpi.GetClassType() == CoreClasses.java.lang.Object.Wrapper
							&& thisType.IsArray
							&& ReferenceEquals(cpi.Name, StringConstants.CLONE))
						{
							// Patch the instruction, so that the compiler doesn't need to do this test again.
							instr.PatchOpCode(NormalizedByteCode.__clone_array);
							return;
						}
						SetHardError(wrapper.GetClassLoader(), ref instr, HardError.IllegalAccessError, "tried to access method {0}.{1}{2} from class {3}", ToSlash(targetMethod.DeclaringType.Name), cpi.Name, ToSlash(cpi.Signature), ToSlash(wrapper.Name));
					}
				}
				else
				{
					SetHardError(wrapper.GetClassLoader(), ref instr, HardError.IncompatibleClassChangeError, "static call to non-static method (or v.v.)");
				}
			}
			else
			{
				SetHardError(wrapper.GetClassLoader(), ref instr, HardError.NoSuchMethodError, "{0}.{1}{2}", cpi.Class, cpi.Name, cpi.Signature);
			}
		}
	}

	private static string ToSlash(string str)
	{
		return str.Replace('.', '/');
	}

	private void PatchFieldAccess(TypeWrapper wrapper, MethodWrapper mw, ref ClassFile.Method.Instruction instr, StackState stack)
	{
		ClassFile.ConstantPoolItemFieldref cpi = classFile.GetFieldref(instr.Arg1);
		bool isStatic;
		bool write;
		TypeWrapper thisType;
		switch(instr.NormalizedOpCode)
		{
			case NormalizedByteCode.__getfield:
				isStatic = false;
				write = false;
				thisType = SigTypeToClassName(stack.PopObjectType(GetFieldref(instr.Arg1).GetClassType()), cpi.GetClassType(), wrapper);
				break;
			case NormalizedByteCode.__putfield:
				stack.PopType(GetFieldref(instr.Arg1).GetFieldType());
				isStatic = false;
				write = true;
				// putfield is allowed to access the unintialized this
				if(stack.PeekType() == VerifierTypeWrapper.UninitializedThis
					&& wrapper.IsAssignableTo(GetFieldref(instr.Arg1).GetClassType()))
				{
					thisType = wrapper;
				}
				else
				{
					thisType = SigTypeToClassName(stack.PopObjectType(GetFieldref(instr.Arg1).GetClassType()), cpi.GetClassType(), wrapper);
				}
				break;
			case NormalizedByteCode.__getstatic:
				isStatic = true;
				write = false;
				thisType = null;
				break;
			case NormalizedByteCode.__putstatic:
				// special support for when we're being called from IsSideEffectFreeStaticInitializer
				if(mw == null)
				{
					switch(GetFieldref(instr.Arg1).Signature[0])
					{
						case 'B':
						case 'Z':
						case 'C':
						case 'S':
						case 'I':
							stack.PopInt();
							break;
						case 'F':
							stack.PopFloat();
							break;
						case 'D':
							stack.PopDouble();
							break;
						case 'J':
							stack.PopLong();
							break;
						case 'L':
						case '[':
							if(stack.PopAnyType() != VerifierTypeWrapper.Null)
							{
								throw new VerifyError();
							}
							break;
						default:
							throw new InvalidOperationException();
					}
				}
				else
				{
					stack.PopType(GetFieldref(instr.Arg1).GetFieldType());
				}
				isStatic = true;
				write = true;
				thisType = null;
				break;
			default:
				throw new InvalidOperationException();
		}
		if(mw == null)
		{
			// We're being called from IsSideEffectFreeStaticInitializer,
			// no further checks are possible (nor needed).
		}
		else if(cpi.GetClassType().IsUnloadable)
		{
			if(wrapper.GetClassLoader().DisableDynamicBinding)
			{
				SetHardError(wrapper.GetClassLoader(), ref instr, HardError.NoClassDefFoundError, "{0}", cpi.GetClassType().Name);
			}
			else
			{
				switch(instr.NormalizedOpCode)
				{
					case NormalizedByteCode.__getstatic:
						instr.PatchOpCode(NormalizedByteCode.__dynamic_getstatic);
						break;
					case NormalizedByteCode.__putstatic:
						instr.PatchOpCode(NormalizedByteCode.__dynamic_putstatic);
						break;
					case NormalizedByteCode.__getfield:
						instr.PatchOpCode(NormalizedByteCode.__dynamic_getfield);
						break;
					case NormalizedByteCode.__putfield:
						instr.PatchOpCode(NormalizedByteCode.__dynamic_putfield);
						break;
					default:
						throw new InvalidOperationException();
				}
			}
			return;
		}
		else
		{
			FieldWrapper field = cpi.GetField();
			if(field == null)
			{
				SetHardError(wrapper.GetClassLoader(), ref instr, HardError.NoSuchFieldError, "{0}.{1}", cpi.Class, cpi.Name);
				return;
			}
			if(cpi.GetFieldType() != field.FieldTypeWrapper && !cpi.GetFieldType().IsUnloadable & !field.FieldTypeWrapper.IsUnloadable)
			{
#if STATIC_COMPILER
				StaticCompiler.LinkageError("Field \"{2}.{3}\" is of type \"{0}\" instead of type \"{1}\" as expected by \"{4}\"", field.FieldTypeWrapper, cpi.GetFieldType(), cpi.GetClassType().Name, cpi.Name, wrapper.Name);
#endif
				SetHardError(wrapper.GetClassLoader(), ref instr, HardError.LinkageError, "Loader constraints violated: {0}.{1}", field.DeclaringType.Name, field.Name);
				return;
			}
			if(field.IsStatic != isStatic)
			{
				SetHardError(wrapper.GetClassLoader(), ref instr, HardError.IncompatibleClassChangeError, "Static field access to non-static field (or v.v.)");
				return;
			}
			if(!field.IsAccessibleFrom(cpi.GetClassType(), wrapper, thisType))
			{
				SetHardError(wrapper.GetClassLoader(), ref instr, HardError.IllegalAccessError, "Try to access field {0}.{1} from class {2}", field.DeclaringType.Name, field.Name, wrapper.Name);
				return;
			}
			// are we trying to mutate a final field? (they are read-only from outside of the defining class)
			if(write && field.IsFinal
				&& ((isStatic ? wrapper != cpi.GetClassType() : wrapper != thisType) || (wrapper.GetClassLoader().StrictFinalFieldSemantics && (isStatic ? (mw != null && mw.Name != "<clinit>") : (mw == null || mw.Name != "<init>")))))
			{
				SetHardError(wrapper.GetClassLoader(), ref instr, HardError.IllegalAccessError, "Field {0}.{1} is final", field.DeclaringType.Name, field.Name);
				return;
			}
		}
	}

	// TODO this method should have a better name
	private TypeWrapper SigTypeToClassName(TypeWrapper type, TypeWrapper nullType, TypeWrapper wrapper)
	{
		if(type == VerifierTypeWrapper.UninitializedThis)
		{
			return wrapper;
		}
		else if(VerifierTypeWrapper.IsNew(type))
		{
			return ((VerifierTypeWrapper)type).UnderlyingType;
		}
		else if(type == VerifierTypeWrapper.Null)
		{
			return nullType;
		}
		else
		{
			return type;
		}
	}

	private int AllocErrorMessage(string message)
	{
		if(errorMessages == null)
		{
			errorMessages = new List<string>();
		}
		int index = errorMessages.Count;
		errorMessages.Add(message);
		return index;
	}

	private string CheckLoaderConstraints(ClassFile.ConstantPoolItemMI cpi, MethodWrapper mw)
	{
		if(cpi.GetRetType() != mw.ReturnType && !cpi.GetRetType().IsUnloadable && !mw.ReturnType.IsUnloadable)
		{
#if STATIC_COMPILER
			StaticCompiler.LinkageError("Method \"{2}.{3}{4}\" has a return type \"{0}\" instead of type \"{1}\" as expected by \"{5}\"", mw.ReturnType, cpi.GetRetType(), cpi.GetClassType().Name, cpi.Name, cpi.Signature, classFile.Name);
#endif
			return "Loader constraints violated (return type): " + mw.DeclaringType.Name + "." + mw.Name + mw.Signature;
		}
		TypeWrapper[] here = cpi.GetArgTypes();
		TypeWrapper[] there = mw.GetParameters();
		for(int i = 0; i < here.Length; i++)
		{
			if(here[i] != there[i] && !here[i].IsUnloadable && !there[i].IsUnloadable)
			{
#if STATIC_COMPILER
				StaticCompiler.LinkageError("Method \"{2}.{3}{4}\" has a argument type \"{0}\" instead of type \"{1}\" as expected by \"{5}\"", there[i], here[i], cpi.GetClassType().Name, cpi.Name, cpi.Signature, classFile.Name);
#endif
				return "Loader constraints violated (arg " + i + "): " + mw.DeclaringType.Name + "." + mw.Name + mw.Signature;
			}
		}
		return null;
	}

	private ClassFile.ConstantPoolItemInvokeDynamic GetInvokeDynamic(int index)
	{
		try
		{
			ClassFile.ConstantPoolItemInvokeDynamic item = classFile.GetInvokeDynamic(index);
			if(item != null)
			{
				return item;
			}
		}
		catch(InvalidCastException)
		{
		}
		catch(IndexOutOfRangeException)
		{
		}
		throw new VerifyError("Illegal constant pool index");
	}

	private ClassFile.ConstantPoolItemMI GetMethodref(int index)
	{
		try
		{
			ClassFile.ConstantPoolItemMI item = classFile.GetMethodref(index);
			if(item != null)
			{
				return item;
			}
		}
		catch(InvalidCastException)
		{
		}
		catch(IndexOutOfRangeException)
		{
		}
		throw new VerifyError("Illegal constant pool index");
	}

	private ClassFile.ConstantPoolItemFieldref GetFieldref(int index)
	{
		try
		{
			ClassFile.ConstantPoolItemFieldref item = classFile.GetFieldref(index);
			if(item != null)
			{
				return item;
			}
		}
		catch(InvalidCastException)
		{
		}
		catch(IndexOutOfRangeException)
		{
		}
		throw new VerifyError("Illegal constant pool index");
	}

	private ClassFile.ConstantType GetConstantPoolConstantType(int index)
	{
		try
		{
			return classFile.GetConstantPoolConstantType(index);
		}
		catch(IndexOutOfRangeException)
		{
			// constant pool index out of range
		}
		catch(InvalidOperationException)
		{
			// specified constant pool entry doesn't contain a constant
		}
		catch(NullReferenceException)
		{
			// specified constant pool entry is empty (entry 0 or the filler following a wide entry)
		}
		throw new VerifyError("Illegal constant pool index");
	}

	private TypeWrapper GetConstantPoolClassType(int index)
	{
		try
		{
			return classFile.GetConstantPoolClassType(index);
		}
		catch(InvalidCastException)
		{
		}
		catch(IndexOutOfRangeException)
		{
		}
		catch(NullReferenceException)
		{
		}
		throw new VerifyError("Illegal constant pool index");
	}

	internal void ClearFaultBlockException(int instructionIndex)
	{
		Debug.Assert(state[instructionIndex].GetStackHeight() == 1);
		state[instructionIndex].ClearFaultBlockException();
	}

	private static void DumpMethod(CodeInfo codeInfo, ClassFile.Method method, UntangledExceptionTable exceptions)
	{
		ClassFile.Method.Instruction[] code = method.Instructions;
		InstructionFlags[] flags = ComputePartialReachability(codeInfo, code, exceptions, 0, false);
		for (int i = 0; i < code.Length; i++)
		{
			bool label = (flags[i] & InstructionFlags.BranchTarget) != 0;
			if (!label)
			{
				for (int j = 0; j < exceptions.Length; j++)
				{
					if (exceptions[j].startIndex == i
						|| exceptions[j].endIndex == i
						|| exceptions[j].handlerIndex == i)
					{
						label = true;
						break;
					}
				}
			}
			if (label)
			{
				Console.WriteLine("label{0}:", i);
			}
			if ((flags[i] & InstructionFlags.Reachable) != 0)
			{
				Console.Write("  {1}", i, code[i].NormalizedOpCode.ToString().Substring(2));
				switch (ByteCodeMetaData.GetFlowControl(code[i].NormalizedOpCode))
				{
					case ByteCodeFlowControl.Branch:
					case ByteCodeFlowControl.CondBranch:
						Console.Write(" label{0}", code[i].Arg1);
						break;
				}
				switch (code[i].NormalizedOpCode)
				{
					case NormalizedByteCode.__iload:
					case NormalizedByteCode.__lload:
					case NormalizedByteCode.__fload:
					case NormalizedByteCode.__dload:
					case NormalizedByteCode.__aload:
					case NormalizedByteCode.__istore:
					case NormalizedByteCode.__lstore:
					case NormalizedByteCode.__fstore:
					case NormalizedByteCode.__dstore:
					case NormalizedByteCode.__astore:
					case NormalizedByteCode.__iconst:
						Console.Write(" {0}", code[i].Arg1);
						break;
					case NormalizedByteCode.__ldc:
					case NormalizedByteCode.__ldc_nothrow:
					case NormalizedByteCode.__getfield:
					case NormalizedByteCode.__getstatic:
					case NormalizedByteCode.__putfield:
					case NormalizedByteCode.__putstatic:
					case NormalizedByteCode.__invokeinterface:
					case NormalizedByteCode.__invokespecial:
					case NormalizedByteCode.__invokestatic:
					case NormalizedByteCode.__invokevirtual:
					case NormalizedByteCode.__new:
						Console.Write(" #{0}", code[i].Arg1);
						break;
				}
				Console.WriteLine();
			}
		}
		for (int i = 0; i < exceptions.Length; i++)
		{
			Console.WriteLine(".catch #{0} from label{1} to label{2} using label{3}", exceptions[i].catch_type, exceptions[i].startIndex, exceptions[i].endIndex, exceptions[i].handlerIndex);
		}
	}
}
