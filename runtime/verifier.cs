/*
  Copyright (C) 2002-2010 Jeroen Frijters

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
#if STATIC_COMPILER
using IKVM.Reflection.Emit;
using Type = IKVM.Reflection.Type;
#else
using System.Reflection.Emit;
#endif
using IKVM.Internal;
using InstructionFlags = IKVM.Internal.ClassFile.Method.InstructionFlags;
using ExceptionTableEntry = IKVM.Internal.ClassFile.Method.ExceptionTableEntry;

class InstructionState
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
	private LocalStoreSites[] localStoreSites;
	private bool unitializedThis;
	internal bool changed = true;
	private enum ShareFlags : byte
	{
		None = 0,
		Stack = 1,
		Locals = 2,
		LocalStoreSites = 4,
		All = Stack | Locals | LocalStoreSites
	}
	private ShareFlags flags;

	private InstructionState(TypeWrapper[] stack, int stackSize, int stackEnd, TypeWrapper[] locals, LocalStoreSites[] localStoreSites, bool unitializedThis)
	{
		this.flags = ShareFlags.All;
		this.stack = stack;
		this.stackSize = stackSize;
		this.stackEnd = stackEnd;
		this.locals = locals;
		this.localStoreSites = localStoreSites;
		this.unitializedThis = unitializedThis;
	}

	internal InstructionState(int maxLocals, int maxStack)
	{
		this.flags = ShareFlags.None;
		this.stack = new TypeWrapper[maxStack];
		this.stackEnd = maxStack;
		this.locals = new TypeWrapper[maxLocals];
		this.localStoreSites = new LocalStoreSites[maxLocals];
	}

	internal InstructionState Copy()
	{
		return new InstructionState(stack, stackSize, stackEnd, locals, localStoreSites, unitializedThis);
	}

	internal void CopyTo(InstructionState target)
	{
		target.flags = ShareFlags.All;
		target.stack = stack;
		target.stackSize = stackSize;
		target.stackEnd = stackEnd;
		target.locals = locals;
		target.localStoreSites = localStoreSites;
		target.unitializedThis = unitializedThis;
		target.changed = true;
	}

	internal InstructionState CopyLocals()
	{
		InstructionState copy = new InstructionState(new TypeWrapper[stack.Length], 0, stack.Length, locals, localStoreSites, unitializedThis);
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
			LocalStoreSites storeSites = s.localStoreSites[i];
			LocalStoreSites storeSites2 = s2.localStoreSites[i];
			TypeWrapper baseType = InstructionState.FindCommonBaseType(type, type2);
			if(type != baseType)
			{
				s.LocalsCopyOnWrite();
				s.locals[i] = baseType;
				s.changed = true;
			}
			storeSites = MergeStoreSites(storeSites, storeSites2);
			if(storeSites.Count != s.localStoreSites[i].Count)
			{
				s.LocalStoreSitesCopyOnWrite();
				s.localStoreSites[i] = storeSites;
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
		TypeWrapper type = null;
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

	private void SetLocalStoreSite(int localIndex, int instructionIndex)
	{
		LocalStoreSitesCopyOnWrite();
		localStoreSites[localIndex] = LocalStoreSites.Alloc();
		localStoreSites[localIndex].Add(instructionIndex);
	}

	internal void GetLocalInt(int index, ref Dictionary<int, string> readers)
	{
		if(GetLocalType(index, ref readers) != PrimitiveTypeWrapper.INT)
		{
			throw new VerifyError("Invalid local type");
		}
	}

	internal void SetLocalInt(int index, int instructionIndex)
	{
		SetLocal1(index, PrimitiveTypeWrapper.INT);
		SetLocalStoreSite(index, instructionIndex);
	}

	internal void GetLocalLong(int index, ref Dictionary<int, string> readers)
	{
		if(GetLocalType(index, ref readers) != PrimitiveTypeWrapper.LONG)
		{
			throw new VerifyError("incorrect local type, not long");
		}
	}

	internal void SetLocalLong(int index, int instructionIndex)
	{
		SetLocal2(index, PrimitiveTypeWrapper.LONG);
		SetLocalStoreSite(index, instructionIndex);
	}

	internal void GetLocalFloat(int index, ref Dictionary<int, string> readers)
	{
		if(GetLocalType(index, ref readers) != PrimitiveTypeWrapper.FLOAT)
		{
			throw new VerifyError("incorrect local type, not float");
		}
	}

	internal void SetLocalFloat(int index, int instructionIndex)
	{
		SetLocal1(index, PrimitiveTypeWrapper.FLOAT);
		SetLocalStoreSite(index, instructionIndex);
	}

	internal void GetLocalDouble(int index, ref Dictionary<int, string> readers)
	{
		if(GetLocalType(index, ref readers) != PrimitiveTypeWrapper.DOUBLE)
		{
			throw new VerifyError("incorrect local type, not double");
		}
	}

	internal void SetLocalDouble(int index, int instructionIndex)
	{
		SetLocal2(index, PrimitiveTypeWrapper.DOUBLE);
		SetLocalStoreSite(index, instructionIndex);
	}

	internal TypeWrapper GetLocalType(int index, ref Dictionary<int, string> readers)
	{
		try
		{
			if(readers == null)
			{
				readers = new Dictionary<int,string>();
			}
			for(int i = 0; i < localStoreSites[index].Count; i++)
			{
				readers[localStoreSites[index][i]] = "";
			}
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
		SetLocalStoreSite(index, instructionIndex);
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
		if(PopAnyType() != PrimitiveTypeWrapper.INT)
		{
			throw new VerifyError("Int expected on stack");
		}
	}

	internal bool PopFloat()
	{
		TypeWrapper tw = PopAnyType();
		if(tw != PrimitiveTypeWrapper.FLOAT && tw != VerifierTypeWrapper.ExtendedFloat)
		{
			throw new VerifyError("Float expected on stack");
		}
		return tw == VerifierTypeWrapper.ExtendedFloat;
	}

	internal bool PopDouble()
	{
		TypeWrapper tw = PopAnyType();
		if(tw != PrimitiveTypeWrapper.DOUBLE && tw != VerifierTypeWrapper.ExtendedDouble)
		{
			throw new VerifyError("Double expected on stack");
		}
		return tw == VerifierTypeWrapper.ExtendedDouble;
	}

	internal void PopLong()
	{
		if(PopAnyType() != PrimitiveTypeWrapper.LONG)
		{
			throw new VerifyError("Long expected on stack");
		}
	}

	internal TypeWrapper PopArrayType()
	{
		TypeWrapper type = PopAnyType();
		if(!VerifierTypeWrapper.IsNullOrUnloadable(type) && type.ArrayRank == 0)
		{
			throw new VerifyError("Array reference expected on stack");
		}
		return type;
	}

	// null or an initialized object reference
	internal TypeWrapper PopObjectType()
	{
		TypeWrapper type = PopType();
		if(type.IsPrimitive || VerifierTypeWrapper.IsNew(type) || type == VerifierTypeWrapper.UninitializedThis)
		{
			throw new VerifyError("Expected object reference on stack");
		}
		return type;
	}

	// null or an initialized object reference derived from baseType (or baseType)
	internal TypeWrapper PopObjectType(TypeWrapper baseType)
	{
		TypeWrapper type = PopObjectType();
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
		TypeWrapper type = PopAnyType();
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
		if(baseType.IsIntOnStackPrimitive)
		{
			baseType = PrimitiveTypeWrapper.INT;
		}
		TypeWrapper type = PopAnyType();
		if(VerifierTypeWrapper.IsNew(type) || type == VerifierTypeWrapper.UninitializedThis)
		{
			throw new VerifyError("Expecting to find object/array on stack");
		}
		if(type != baseType &&
			!((type.IsUnloadable && !baseType.IsPrimitive) || (baseType.IsUnloadable && !type.IsPrimitive) ||
				type.IsAssignableTo(baseType)))
		{
			// HACK because of the way interfaces references works, if baseType
			// is an interface or array of interfaces, any reference will be accepted
			if(baseType.IsInterfaceOrInterfaceArray && !type.IsPrimitive)
			{
				return type;
			}
			if(type == VerifierTypeWrapper.ExtendedDouble && baseType == PrimitiveTypeWrapper.DOUBLE)
			{
				return type;
			}
			if(type == VerifierTypeWrapper.ExtendedFloat && baseType == PrimitiveTypeWrapper.FLOAT)
			{
				return type;
			}
			throw new VerifyError("Unexpected type " + type.Name + " where " + baseType.Name + " was expected");
		}
		return type;
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
				SetLocalStoreSite(i, instructionIndex);
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

	private void LocalStoreSitesCopyOnWrite()
	{
		if((flags & ShareFlags.LocalStoreSites) != 0)
		{
			flags &= ~ShareFlags.LocalStoreSites;
			LocalStoreSites.MarkShared(localStoreSites);
			localStoreSites = (LocalStoreSites[])localStoreSites.Clone();
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

	// this method ensures that no uninitialized object are in the current state
	internal void CheckUninitializedObjRefs()
	{
		for(int i = 0; i < locals.Length; i++)
		{
			TypeWrapper type = locals[i];
			if(VerifierTypeWrapper.IsNew(type))
			{
				throw new VerifyError("uninitialized object ref in local (2)");
			}
		}
		for(int i = 0; i < stackSize; i++)
		{
			TypeWrapper type = stack[i];
			if(VerifierTypeWrapper.IsNew(type))
			{
				throw new VerifyError("uninitialized object ref on stack");
			}
		}
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
		if(baseType.IsIntOnStackPrimitive)
		{
			baseType = PrimitiveTypeWrapper.INT;
		}
		TypeWrapper type = PopAnyType();
		if(VerifierTypeWrapper.IsNew(type) || type == VerifierTypeWrapper.UninitializedThis)
		{
			throw new VerifyError("Expecting to find object/array on stack");
		}
		if(type != baseType &&
			!((type.IsUnloadable && !baseType.IsPrimitive) || (baseType.IsUnloadable && !type.IsPrimitive) ||
			type.IsAssignableTo(baseType)))
		{
			// HACK because of the way interfaces references works, if baseType
			// is an interface or array of interfaces, any reference will be accepted
			if(baseType.IsInterfaceOrInterfaceArray && !type.IsPrimitive)
			{
				return type;
			}
			if(type == VerifierTypeWrapper.ExtendedDouble && baseType == PrimitiveTypeWrapper.DOUBLE)
			{
				return type;
			}
			if(type == VerifierTypeWrapper.ExtendedFloat && baseType == PrimitiveTypeWrapper.FLOAT)
			{
				return type;
			}
			throw new VerifyError("Unexpected type " + type.Name + " where " + baseType.Name + " was expected");
		}
		return type;
	}

	// NOTE this can *not* be used to pop double or long
	internal TypeWrapper PopType()
	{
		TypeWrapper type = PopAnyType();
		if(type.IsWidePrimitive || type == VerifierTypeWrapper.ExtendedDouble)
		{
			throw new VerifyError("Attempt to split long or double on the stack");
		}
		return type;
	}

	internal void PopInt()
	{
		if(PopAnyType() != PrimitiveTypeWrapper.INT)
		{
			throw new VerifyError("Int expected on stack");
		}
	}

	internal void PopFloat()
	{
		TypeWrapper tw = PopAnyType();
		if(tw != PrimitiveTypeWrapper.FLOAT && tw != VerifierTypeWrapper.ExtendedFloat)
		{
			throw new VerifyError("Float expected on stack");
		}
	}

	internal void PopDouble()
	{
		TypeWrapper tw = PopAnyType();
		if(tw != PrimitiveTypeWrapper.DOUBLE && tw != VerifierTypeWrapper.ExtendedDouble)
		{
			throw new VerifyError("Double expected on stack");
		}
	}

	internal void PopLong()
	{
		if(PopAnyType() != PrimitiveTypeWrapper.LONG)
		{
			throw new VerifyError("Long expected on stack");
		}
	}

	internal TypeWrapper PopArrayType()
	{
		TypeWrapper type = PopAnyType();
		if(!VerifierTypeWrapper.IsNullOrUnloadable(type) && type.ArrayRank == 0)
		{
			throw new VerifyError("Array reference expected on stack");
		}
		return type;
	}

	// either null or an initialized object reference
	internal TypeWrapper PopObjectType()
	{
		TypeWrapper type = PopAnyType();
		if(type.IsPrimitive || VerifierTypeWrapper.IsNew(type) || type == VerifierTypeWrapper.UninitializedThis)
		{
			throw new VerifyError("Expected object reference on stack");
		}
		return type;
	}

	// null or an initialized object reference derived from baseType (or baseType)
	internal TypeWrapper PopObjectType(TypeWrapper baseType)
	{
		TypeWrapper type = PopObjectType();
		// HACK because of the way interfaces references works, if baseType
		// is an interface or array of interfaces, any reference will be accepted
		if(!baseType.IsUnloadable && !baseType.IsInterfaceOrInterfaceArray && !(type.IsUnloadable || type.IsAssignableTo(baseType)))
		{
			throw new VerifyError("Unexpected type " + type + " where " + baseType + " was expected");
		}
		return type;
	}
}

class LocalVar
{
	internal bool isArg;
	internal int local;
	internal TypeWrapper type;
	internal LocalBuilder builder;
	// used to emit debugging info, only available if ClassLoaderWrapper.EmitDebugInfo is true
	internal string name;
	internal int start_pc;
	internal int end_pc;

	internal void FindLvtEntry(ClassFile.Method method, int instructionIndex)
	{
		ClassFile.Method.LocalVariableTableEntry[] lvt = method.LocalVariableTableAttribute;
		if(lvt != null)
		{
			int pc = method.Instructions[instructionIndex].PC;
			int nextPC = method.Instructions[instructionIndex + 1].PC;
			bool isStore = MethodAnalyzer.IsStoreLocal(method.Instructions[instructionIndex].NormalizedOpCode);
			foreach(ClassFile.Method.LocalVariableTableEntry e in lvt)
			{
				// TODO validate the contents of the LVT entry
				if(e.index == local &&
					(e.start_pc <= pc || (e.start_pc == nextPC && isStore)) && 
					e.start_pc + e.length > pc)
				{
					name = e.name;
					start_pc = e.start_pc;
					end_pc = e.start_pc + e.length;
					break;
				}
			}
		}
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

class MethodAnalyzer
{
	private static TypeWrapper ByteArrayType;
	private static TypeWrapper BooleanArrayType;
	private static TypeWrapper ShortArrayType;
	private static TypeWrapper CharArrayType;
	private static TypeWrapper IntArrayType;
	private static TypeWrapper FloatArrayType;
	private static TypeWrapper DoubleArrayType;
	private static TypeWrapper LongArrayType;
	private static TypeWrapper java_lang_ThreadDeath;
	private ClassFile classFile;
	private ClassFile.Method method;
	private InstructionState[] state;
	private LocalVar[/*instructionIndex*/] localVars;
	private LocalVar[/*instructionIndex*/][/*localIndex*/] invokespecialLocalVars;
	private LocalVar[/*index*/] allLocalVars;
	private List<string> errorMessages;
	private ExceptionTableEntry[] exceptions;

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

	internal MethodAnalyzer(TypeWrapper wrapper, MethodWrapper mw, ClassFile classFile, ClassFile.Method method, ClassLoaderWrapper classLoader)
	{
		if(method.VerifyError != null)
		{
			throw new VerifyError(method.VerifyError);
		}

		this.classFile = classFile;
		this.method = method;
		state = new InstructionState[method.Instructions.Length];

		Dictionary<int,string>[] localStoreReaders = new Dictionary<int,string>[method.Instructions.Length];

		// HACK because types have to have identity, the new types are cached here
		Dictionary<int, TypeWrapper> newTypes = new Dictionary<int,TypeWrapper>();
		Dictionary<int, TypeWrapper> faultTypes = new Dictionary<int, TypeWrapper>();

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
		TypeWrapper thisType;
		int firstNonArgLocalIndex = 0;
		if(!method.IsStatic)
		{
			thisType = VerifierTypeWrapper.MakeThis(wrapper);
			// this reference. If we're a constructor, the this reference is uninitialized.
			if(ReferenceEquals(method.Name, StringConstants.INIT))
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
		AnalyzeTypeFlow(wrapper, thisType, mw, localStoreReaders, newTypes, faultTypes);
		exceptions = UntangleExceptionBlocks(classFile, method.ExceptionTable);
		OptimizationPass(wrapper, classLoader);
		FinalCodePatchup(wrapper, mw);
		if (AnalyzePotentialFaultBlocks())
		{
			AnalyzeTypeFlow(wrapper, thisType, mw, localStoreReaders, newTypes, faultTypes);
		}
		AnalyzeLocalVariables(localStoreReaders, classLoader);
		ComputePartialReachability(0, true);
	}

	private void AnalyzeTypeFlow(TypeWrapper wrapper, TypeWrapper thisType, MethodWrapper mw, Dictionary<int, string>[] localStoreReaders, Dictionary<int, TypeWrapper> newTypes, Dictionary<int, TypeWrapper> faultTypes)
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
								int idx = method.ExceptionTable[j].handlerIndex;
								InstructionState ex = state[i].CopyLocals();
								int catch_type = method.ExceptionTable[j].catch_type;
								if(catch_type == 0)
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
						}
						state[i].CopyTo(s);
						ClassFile.Method.Instruction instr = instructions[i];
						switch(instr.NormalizedOpCode)
						{
							case NormalizedByteCode.__aload:
							{
								TypeWrapper type = s.GetLocalType(instr.NormalizedArg1, ref localStoreReaders[i]);
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
							case NormalizedByteCode.__dastore_conv:
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
							case NormalizedByteCode.__fastore_conv:
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
								s.PopType(GetFieldref(instr.Arg1).GetFieldType());
								// putfield is allowed to access the unintialized this
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
									case ClassFile.ConstantType.Class:
										if(classFile.MajorVersion < 49)
										{
											throw new VerifyError("Illegal type in constant pool");
										}
										s.PushType(CoreClasses.java.lang.Class.Wrapper);
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
							{
								ClassFile.ConstantPoolItemMI cpi = GetMethodref(instr.Arg1);
								s.MultiPopAnyType(cpi.GetArgTypes().Length);
								if(instr.NormalizedOpCode != NormalizedByteCode.__invokestatic)
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
								TypeWrapper retType = cpi.GetRetType();
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
							case NormalizedByteCode.__goto:
								break;
							case NormalizedByteCode.__istore:
								s.PopInt();
								s.SetLocalInt(instr.NormalizedArg1, i);
								break;
							case NormalizedByteCode.__iload:
								s.GetLocalInt(instr.NormalizedArg1, ref localStoreReaders[i]);
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
								s.GetLocalFloat(instr.NormalizedArg1, ref localStoreReaders[i]);
								s.PushFloat();
								break;
							case NormalizedByteCode.__fstore:
							case NormalizedByteCode.__fstore_conv:
								s.PopFloat();
								s.SetLocalFloat(instr.NormalizedArg1, i);
								break;
							case NormalizedByteCode.__dload:
								s.GetLocalDouble(instr.NormalizedArg1, ref localStoreReaders[i]);
								s.PushDouble();
								break;
							case NormalizedByteCode.__dstore:
							case NormalizedByteCode.__dstore_conv:
								s.PopDouble();
								s.SetLocalDouble(instr.NormalizedArg1, i);
								break;
							case NormalizedByteCode.__lload:
								s.GetLocalLong(instr.NormalizedArg1, ref localStoreReaders[i]);
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
								s.GetLocalInt(instr.Arg1, ref localStoreReaders[i]);
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
							default:
								throw new NotImplementedException(instr.NormalizedOpCode.ToString());
						}
						if(s.GetStackHeight() > method.MaxStack)
						{
							throw new VerifyError("Stack size too large");
						}
						try
						{
							// another big switch to handle the opcode targets
							switch(instr.NormalizedOpCode)
							{
								case NormalizedByteCode.__tableswitch:
								case NormalizedByteCode.__lookupswitch:
									for(int j = 0; j < instr.SwitchEntryCount; j++)
									{
										state[instr.GetSwitchTargetIndex(j)] += s;
									}
									state[instr.DefaultTarget] += s;
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
									state[i + 1] += s;
									state[instr.TargetIndex] += s;
									break;
								case NormalizedByteCode.__goto:
									state[instr.TargetIndex] += s;
									break;
								case NormalizedByteCode.__ireturn:
								case NormalizedByteCode.__lreturn:
								case NormalizedByteCode.__freturn:
								case NormalizedByteCode.__dreturn:
								case NormalizedByteCode.__areturn:
								case NormalizedByteCode.__return:
								case NormalizedByteCode.__athrow:
								case NormalizedByteCode.__static_error:
									break;
								default:
									state[i + 1] += s;
									break;
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

	private void OptimizationPass(TypeWrapper wrapper, ClassLoaderWrapper classLoader)
	{
		// Optimization pass
		if (classLoader.RemoveAsserts)
		{
			FieldWrapper assertionsDisabled = null;
			foreach (FieldWrapper fw in wrapper.GetFields())
			{
				// HACK we assume that all compilers use the same name for this field (ecj and javac do)
				if (fw.Name == "$assertionsDisabled" && fw.Signature == "Z"
					&& (fw.Modifiers & (IKVM.Attributes.Modifiers.AccessMask | IKVM.Attributes.Modifiers.Final | IKVM.Attributes.Modifiers.Static | IKVM.Attributes.Modifiers.Synthetic))
						== (IKVM.Attributes.Modifiers.Static | IKVM.Attributes.Modifiers.Final | IKVM.Attributes.Modifiers.Synthetic))
				{
					assertionsDisabled = fw;
					break;
				}
			}
			if (assertionsDisabled != null)
			{
				// compute branch targets
				InstructionFlags[] flags = ComputePartialReachability(0, false);
				ClassFile.Method.Instruction[] instructions = method.Instructions;
				for (int i = 0; i < instructions.Length; i++)
				{
					if (instructions[i].NormalizedOpCode == NormalizedByteCode.__getstatic
						&& instructions[i + 1].NormalizedOpCode == NormalizedByteCode.__ifne
						&& instructions[i + 1].TargetIndex > i
						&& (flags[i + 1] & InstructionFlags.BranchTarget) == 0)
					{
						ClassFile.ConstantPoolItemFieldref cpi = classFile.GetFieldref(instructions[i].Arg1);
						if (cpi.GetField() == assertionsDisabled)
						{
							// We've found an assertion. We patch the instruction to branch around it so that
							// the assertion code will be unreachable (and hence optimized away).
							// Note that the goto will be optimized away later by the code generator (which removes unnecessary branches).
							instructions[i].PatchOpCode(NormalizedByteCode.__goto, instructions[i + 1].TargetIndex);
						}
					}
				}
			}
		}
	}

	private void FinalCodePatchup(TypeWrapper wrapper, MethodWrapper mw)
	{
		// Now we do another pass to find "hard error" instructions and verify backward branches
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
						case NormalizedByteCode.__fstore:
							if(stack.PeekType() == VerifierTypeWrapper.ExtendedFloat && !method.IsStrictfp)
							{
								instructions[i].PatchOpCode(NormalizedByteCode.__fstore_conv);
							}
							break;
						case NormalizedByteCode.__fastore:
							if(stack.PeekType() == VerifierTypeWrapper.ExtendedFloat && !method.IsStrictfp)
							{
								instructions[i].PatchOpCode(NormalizedByteCode.__fastore_conv);
							}
							break;
						case NormalizedByteCode.__dstore:
							if(stack.PeekType() == VerifierTypeWrapper.ExtendedDouble && !method.IsStrictfp)
							{
								instructions[i].PatchOpCode(NormalizedByteCode.__dstore_conv);
							}
							break;
						case NormalizedByteCode.__dastore:
							if(stack.PeekType() == VerifierTypeWrapper.ExtendedDouble && !method.IsStrictfp)
							{
								instructions[i].PatchOpCode(NormalizedByteCode.__dastore_conv);
							}
							break;
						case NormalizedByteCode.__invokeinterface:
						case NormalizedByteCode.__invokespecial:
						case NormalizedByteCode.__invokestatic:
						case NormalizedByteCode.__invokevirtual:
							VerifyInvoke(wrapper, ref instructions[i], stack);
							break;
						case NormalizedByteCode.__getfield:
						case NormalizedByteCode.__putfield:
						case NormalizedByteCode.__getstatic:
						case NormalizedByteCode.__putstatic:
							VerifyFieldAccess(wrapper, mw, ref instructions[i], stack);
							break;
						case NormalizedByteCode.__ldc:
							if(classFile.GetConstantPoolConstantType(instructions[i].Arg1) == ClassFile.ConstantType.Class)
							{
								TypeWrapper tw = classFile.GetConstantPoolClassType(instructions[i].Arg1);
								if(tw.IsUnloadable)
								{
#if STATIC_COMPILER
									SetHardError(ref instructions[i], HardError.NoClassDefFoundError, "{0}", tw.Name);
#endif
								}
							}
							break;
						case NormalizedByteCode.__new:
						{
							TypeWrapper tw = classFile.GetConstantPoolClassType(instructions[i].Arg1);
							if(tw.IsUnloadable)
							{
#if STATIC_COMPILER
								SetHardError(ref instructions[i], HardError.NoClassDefFoundError, "{0}", tw.Name);
#endif
							}
							else if(!tw.IsAccessibleFrom(wrapper))
							{
								SetHardError(ref instructions[i], HardError.IllegalAccessError, "Try to access class {0} from class {1}", tw.Name, wrapper.Name);
							}
							else if(tw.IsAbstract)
							{
								SetHardError(ref instructions[i], HardError.InstantiationError, "{0}", tw.Name);
							}
							break;
						}
						case NormalizedByteCode.__multianewarray:
						case NormalizedByteCode.__anewarray:
						{
							TypeWrapper tw = classFile.GetConstantPoolClassType(instructions[i].Arg1);
							if(tw.IsUnloadable)
							{
#if STATIC_COMPILER
								SetHardError(ref instructions[i], HardError.NoClassDefFoundError, "{0}", tw.Name);
#endif
							}
							else if(!tw.IsAccessibleFrom(wrapper))
							{
								SetHardError(ref instructions[i], HardError.IllegalAccessError, "Try to access class {0} from class {1}", tw.Name, wrapper.Name);
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
								// (regardless of JVM.DisableDynamicBinding), because at runtime,
								// null references should always pass thru without attempting
								// to load the type (for Sun compatibility).
							}
							else if(!tw.IsAccessibleFrom(wrapper))
							{
								SetHardError(ref instructions[i], HardError.IllegalAccessError, "Try to access class {0} from class {1}", tw.Name, wrapper.Name);
							}
							break;
						}
						case NormalizedByteCode.__aaload:
						{
							stack.PopInt();
							TypeWrapper tw = stack.PopArrayType();
							if(tw == VerifierTypeWrapper.Null)
							{
							}
							else if(tw.IsUnloadable)
							{
#if STATIC_COMPILER
								SetHardError(ref instructions[i], HardError.NoClassDefFoundError, "{0}", tw.Name);
#endif
							}
							else
							{
								tw = tw.ElementTypeWrapper;
								if(tw.IsPrimitive)
								{
									throw new VerifyError("Object array expected");
								}
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
#if STATIC_COMPILER
								SetHardError(ref instructions[i], HardError.NoClassDefFoundError, "{0}", tw.Name);
#endif
							}
							else
							{
								// TODO do we need any other tests?
							}
							break;
						}
						default:
							break;
					}
					// verify backward branches
					switch(instructions[i].NormalizedOpCode)
					{
						case NormalizedByteCode.__tableswitch:
						case NormalizedByteCode.__lookupswitch:
						{
							bool hasbackbranch = false;
							for(int j = 0; j < instructions[i].SwitchEntryCount; j++)
							{
								hasbackbranch |= instructions[i].GetSwitchTargetIndex(j) < i;
							}
							hasbackbranch |= instructions[i].DefaultTarget < i;
							if(hasbackbranch)
							{
								// backward branches cannot have uninitialized objects on
								// the stack or in local variables
								state[i].CheckUninitializedObjRefs();
							}
							break;
						}
						case NormalizedByteCode.__goto:
							if(instructions[i].TargetIndex < i)
							{
								// backward branches cannot have uninitialized objects on
								// the stack or in local variables
								state[i].CheckUninitializedObjRefs();
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
							if(instructions[i].TargetIndex < i)
							{
								// backward branches cannot have uninitialized objects on
								// the stack or in local variables
								state[i].CheckUninitializedObjRefs();
							}
							break;
					}
				}
			}
		}
	}

	internal InstructionFlags[] ComputePartialReachability(int initialInstructionIndex, bool skipFaultBlocks)
	{
		ClassFile.Method.Instruction[] instructions = method.Instructions;
		InstructionFlags[] flags = new InstructionFlags[instructions.Length];
		bool done = false;
		flags[initialInstructionIndex] |= InstructionFlags.Reachable;
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
							if (!skipFaultBlocks || !VerifierTypeWrapper.IsFaultBlockException(state[idx].GetStackByIndex(0)))
							{
								flags[idx] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
							}
						}
					}
					// mark the successor instructions
					switch (instructions[i].NormalizedOpCode)
					{
						case NormalizedByteCode.__tableswitch:
						case NormalizedByteCode.__lookupswitch:
							{
								for (int j = 0; j < instructions[i].SwitchEntryCount; j++)
								{
									flags[instructions[i].GetSwitchTargetIndex(j)] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
								}
								flags[instructions[i].DefaultTarget] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
								break;
							}
						case NormalizedByteCode.__goto:
							flags[instructions[i].TargetIndex] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
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
							flags[instructions[i].TargetIndex] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
							flags[i + 1] |= InstructionFlags.Reachable;
							break;
						case NormalizedByteCode.__ireturn:
						case NormalizedByteCode.__lreturn:
						case NormalizedByteCode.__freturn:
						case NormalizedByteCode.__dreturn:
						case NormalizedByteCode.__areturn:
						case NormalizedByteCode.__return:
						case NormalizedByteCode.__athrow:
						case NormalizedByteCode.__athrow_no_unmap:
						case NormalizedByteCode.__static_error:
							break;
						default:
							flags[i + 1] |= InstructionFlags.Reachable;
							break;
					}
				}
			}
		}
		return flags;
	}

	private void AnalyzeLocalVariables(Dictionary<int, string>[] localStoreReaders, ClassLoaderWrapper classLoader)
	{
		// now that we've done the code flow analysis, we can do a liveness analysis on the local variables
		ClassFile.Method.Instruction[] instructions = method.Instructions;
		Dictionary<long, LocalVar> localByStoreSite = new Dictionary<long, LocalVar>();
		List<LocalVar> locals = new List<LocalVar>();
		for(int i = 0; i < localStoreReaders.Length; i++)
		{
			if(localStoreReaders[i] != null)
			{
				VisitLocalLoads(locals, localByStoreSite, localStoreReaders[i], i, classLoader.EmitDebugInfo);
			}
		}
		Dictionary<LocalVar, LocalVar> forwarders = new Dictionary<LocalVar,LocalVar>();
		if(classLoader.EmitDebugInfo)
		{
			InstructionFlags[] flags = ComputePartialReachability(0, false);
			// if we're emitting debug info, we need to keep dead stores as well...
			for(int i = 0; i < instructions.Length; i++)
			{
				if((flags[i] & InstructionFlags.Reachable) != 0
					&& IsStoreLocal(instructions[i].NormalizedOpCode))
				{
					if(!localByStoreSite.ContainsKey(MakeKey(i, instructions[i].NormalizedArg1)))
					{
						LocalVar v = new LocalVar();
						v.local = instructions[i].NormalizedArg1;
						v.type = GetStackTypeWrapper(i, 0);
						v.FindLvtEntry(method, i);
						locals.Add(v);
						localByStoreSite.Add(MakeKey(i, v.local), v);
					}
				}
			}
			// to make the debugging experience better, we have to trust the
			// LocalVariableTable (unless it's clearly bogus) and merge locals
			// together that are the same according to the LVT
			for(int i = 0; i < locals.Count - 1; i++)
			{
				for(int j = i + 1; j < locals.Count; j++)
				{
					LocalVar v1 = (LocalVar)locals[i];
					LocalVar v2 = (LocalVar)locals[j];
					if(v1.name != null && v1.name == v2.name && v1.start_pc == v2.start_pc && v1.end_pc == v2.end_pc)
					{
						// we can only merge if the resulting type is valid (this protects against incorrect
						// LVT data, but is also needed for constructors, where the uninitialized this is a different
						// type from the initialized this)
						TypeWrapper tw = InstructionState.FindCommonBaseType(v1.type, v2.type);
						if(tw != VerifierTypeWrapper.Invalid)
						{
							v1.isArg |= v2.isArg;
							v1.type = tw;
							forwarders.Add(v2, v1);
							locals.RemoveAt(j);
							j--;
						}
					}
				}
			}
		}
		else
		{
			for(int i = 0; i < locals.Count - 1; i++)
			{
				for(int j = i + 1; j < locals.Count; j++)
				{
					LocalVar v1 = (LocalVar)locals[i];
					LocalVar v2 = (LocalVar)locals[j];
					// if the two locals are the same, we merge them, this is a small
					// optimization, it should *not* be required for correctness.
					if(v1.local == v2.local && v1.type == v2.type)
					{
						v1.isArg |= v2.isArg;
						forwarders.Add(v2, v1);
						locals.RemoveAt(j);
						j--;
					}
				}
			}
		}
		invokespecialLocalVars = new LocalVar[instructions.Length][];
		localVars = new LocalVar[instructions.Length];
		for(int i = 0; i < localVars.Length; i++)
		{
			LocalVar v = null;
			if(localStoreReaders[i] != null)
			{
				Debug.Assert(IsLoadLocal(instructions[i].NormalizedOpCode));
				// lame way to look up the local variable for a load
				// (by indirecting through a corresponding store)
				foreach(int store in localStoreReaders[i].Keys)
				{
					v = localByStoreSite[MakeKey(store, instructions[i].NormalizedArg1)];
					break;
				}
			}
			else
			{
				if(instructions[i].NormalizedOpCode == NormalizedByteCode.__invokespecial)
				{
					invokespecialLocalVars[i] = new LocalVar[method.MaxLocals];
					for(int j = 0; j < invokespecialLocalVars[i].Length; j++)
					{
						localByStoreSite.TryGetValue(MakeKey(i, j), out invokespecialLocalVars[i][j]);
					}
				}
				else
				{
					localByStoreSite.TryGetValue(MakeKey(i, instructions[i].NormalizedArg1), out v);
				}
			}
			if(v != null)
			{
				LocalVar fwd;
				if(forwarders.TryGetValue(v, out fwd))
				{
					v = fwd;
				}
				localVars[i] = v;
			}
		}
		this.allLocalVars = locals.ToArray();
	}

	private ExceptionTableEntry[] UntangleExceptionBlocks(ClassFile classFile, ExceptionTableEntry[] exceptionTable)
	{
		ClassFile.Method.Instruction[] instructions = method.Instructions;
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

		// TODO remove these checks, if the above exception untangling is correct, this shouldn't ever
		// be triggered
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

		return exceptions;
	}

	private bool AnalyzePotentialFaultBlocks()
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
					&& state[exceptions[i].handlerIndex] != null
					&& VerifierTypeWrapper.IsFaultBlockException(GetRawStackTypeWrapper(exceptions[i].handlerIndex, 0)))
				{
					InstructionFlags[] flags = ComputePartialReachability(exceptions[i].handlerIndex, true);
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
							VerifierTypeWrapper.ClearFaultBlockException(GetRawStackTypeWrapper(exceptions[i].handlerIndex, 0));
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

	private void SetHardError(ref ClassFile.Method.Instruction instruction, HardError hardError, string message, params object[] args)
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
		StaticCompiler.IssueMessage(msg, classFile.Name + "." + method.Name + method.Signature, text);
#endif
		instruction.SetHardError(hardError, AllocErrorMessage(text));
	}

	private void VerifyInvoke(TypeWrapper wrapper, ref ClassFile.Method.Instruction instr, StackState stack)
	{
		ClassFile.ConstantPoolItemMI cpi = GetMethodref(instr.Arg1);
		if((cpi is ClassFile.ConstantPoolItemInterfaceMethodref) != (instr.NormalizedOpCode == NormalizedByteCode.__invokeinterface))
		{
			throw new VerifyError("Illegal constant pool index");
		}
		if(instr.NormalizedOpCode != NormalizedByteCode.__invokespecial && ReferenceEquals(cpi.Name, StringConstants.INIT))
		{
			throw new VerifyError("Must call initializers using invokespecial");
		}
		if(ReferenceEquals(cpi.Name, StringConstants.CLINIT))
		{
			throw new VerifyError("Illegal call to internal method");
		}
		NormalizedByteCode invoke = instr.NormalizedOpCode;
		TypeWrapper[] args = cpi.GetArgTypes();
		for(int j = args.Length - 1; j >= 0; j--)
		{
			stack.PopType(args[j]);
		}
		if(invoke == NormalizedByteCode.__invokeinterface)
		{
			int argcount = args.Length + 1;
			for(int j = 0; j < args.Length; j++)
			{
				if(args[j].IsWidePrimitive)
				{
					argcount++;
				}
			}
			if(instr.Arg2 != argcount)
			{
				throw new VerifyError("Inconsistent args size");
			}
		}
		bool isnew = false;
		TypeWrapper thisType;
		if(invoke == NormalizedByteCode.__invokestatic)
		{
			thisType = null;
		}
		else
		{
			thisType = SigTypeToClassName(stack.PeekType(), cpi.GetClassType(), wrapper);
			if(ReferenceEquals(cpi.Name, StringConstants.INIT))
			{
				TypeWrapper type = stack.PopType();
				isnew = VerifierTypeWrapper.IsNew(type);
				if((isnew && ((VerifierTypeWrapper)type).UnderlyingType != cpi.GetClassType()) ||
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
				if(invoke != NormalizedByteCode.__invokeinterface)
				{
					TypeWrapper refType = stack.PopObjectType();
					TypeWrapper targetType = cpi.GetClassType();
					if(!VerifierTypeWrapper.IsNullOrUnloadable(refType) && 
						!targetType.IsUnloadable &&
						!refType.IsAssignableTo(targetType))
					{
						throw new VerifyError("Incompatible object argument for function call");
					}
					// for invokespecial we also need to make sure we're calling ourself or a base class
					if(invoke == NormalizedByteCode.__invokespecial)
					{
						if(!VerifierTypeWrapper.IsNullOrUnloadable(refType) && !refType.IsSubTypeOf(wrapper))
						{
							throw new VerifyError("Incompatible target object for invokespecial");
						}
						if(!targetType.IsUnloadable && !wrapper.IsSubTypeOf(targetType))
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
					if(!VerifierTypeWrapper.IsNullOrUnloadable(refType) 
						&& !targetType.IsUnloadable
						&& !refType.IsAssignableTo(targetType)
						&& !targetType.IsInterface)
					{
						throw new VerifyError("Incompatible object argument for function call");
					}
				}
			}
		}

		if(cpi.GetClassType().IsUnloadable || (thisType != null && thisType.IsUnloadable))
		{
#if STATIC_COMPILER
			SetHardError(ref instr, HardError.NoClassDefFoundError, "{0}", cpi.GetClassType().Name);
#else
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
						SetHardError(ref instr, HardError.LinkageError, "Base class no longer loadable");
					}
					break;
				default:
					throw new InvalidOperationException();
			}
#endif
		}
		else if(cpi.GetClassType().IsInterface != (invoke == NormalizedByteCode.__invokeinterface))
		{
			SetHardError(ref instr, HardError.IncompatibleClassChangeError, "invokeinterface on non-interface");
		}
		else
		{
			MethodWrapper targetMethod = invoke == NormalizedByteCode.__invokespecial ? cpi.GetMethodForInvokespecial() : cpi.GetMethod();
			if(targetMethod != null)
			{
				string errmsg = CheckLoaderConstraints(cpi, targetMethod);
				if(errmsg != null)
				{
					SetHardError(ref instr, HardError.LinkageError, "{0}", errmsg);
				}
				else if(targetMethod.IsStatic == (invoke == NormalizedByteCode.__invokestatic))
				{
					if(targetMethod.IsAbstract && invoke == NormalizedByteCode.__invokespecial)
					{
						SetHardError(ref instr, HardError.AbstractMethodError, "{0}.{1}{2}", cpi.Class, cpi.Name, cpi.Signature);
					}
					else if(targetMethod.IsAccessibleFrom(cpi.GetClassType(), wrapper, thisType))
					{
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
						SetHardError(ref instr, HardError.IllegalAccessError, "Try to access method {0}.{1}{2} from class {3}", targetMethod.DeclaringType.Name, cpi.Name, cpi.Signature, wrapper.Name);
					}
				}
				else
				{
					SetHardError(ref instr, HardError.IncompatibleClassChangeError, "static call to non-static method (or v.v.)");
				}
			}
			else
			{
				SetHardError(ref instr, HardError.NoSuchMethodError, "{0}.{1}{2}", cpi.Class, cpi.Name, cpi.Signature);
			}
		}
	}

	private void VerifyFieldAccess(TypeWrapper wrapper, MethodWrapper mw, ref ClassFile.Method.Instruction instr, StackState stack)
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
		else if(cpi.GetClassType().IsUnloadable || (thisType != null && thisType.IsUnloadable))
		{
#if STATIC_COMPILER
			SetHardError(ref instr, HardError.NoClassDefFoundError, "{0}", cpi.GetClassType().Name);
#else
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
#endif
			return;
		}
		else
		{
			FieldWrapper field = cpi.GetField();
			if(field == null)
			{
				SetHardError(ref instr, HardError.NoSuchFieldError, "{0}.{1}", cpi.Class, cpi.Name);
				return;
			}
			if(cpi.GetFieldType() != field.FieldTypeWrapper && !field.FieldTypeWrapper.IsUnloadable)
			{
#if STATIC_COMPILER
				StaticCompiler.LinkageError("Field \"{2}.{3}\" is of type \"{0}\" instead of type \"{1}\" as expected by \"{4}\"", field.FieldTypeWrapper, cpi.GetFieldType(), cpi.GetClassType().Name, cpi.Name, wrapper.Name);
#endif
				SetHardError(ref instr, HardError.LinkageError, "Loader constraints violated: {0}.{1}", field.DeclaringType.Name, field.Name);
				return;
			}
			if(field.IsStatic != isStatic)
			{
				SetHardError(ref instr, HardError.IncompatibleClassChangeError, "Static field access to non-static field (or v.v.)");
				return;
			}
			if(!field.IsAccessibleFrom(cpi.GetClassType(), wrapper, thisType))
			{
				SetHardError(ref instr, HardError.IllegalAccessError, "Try to access field {0}.{1} from class {2}", field.DeclaringType.Name, field.Name, wrapper.Name);
				return;
			}
			// are we trying to mutate a final field? (they are read-only from outside of the defining class)
			if(write && field.IsFinal
				&& ((isStatic ? wrapper != cpi.GetClassType() : wrapper != thisType) || (wrapper.GetClassLoader().StrictFinalFieldSemantics && (isStatic ? (mw != null && mw.Name != "<clinit>") : (mw == null || mw.Name != "<init>")))))
			{
				SetHardError(ref instr, HardError.IllegalAccessError, "Field {0}.{1} is final", field.DeclaringType.Name, field.Name);
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

	internal string GetErrorMessage(int messageId)
	{
		return errorMessages[messageId];
	}

	private string CheckLoaderConstraints(ClassFile.ConstantPoolItemMI cpi, MethodWrapper mw)
	{
		if(cpi.GetRetType() != mw.ReturnType && !mw.ReturnType.IsUnloadable)
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
			if(here[i] != there[i] && !there[i].IsUnloadable)
			{
#if STATIC_COMPILER
				StaticCompiler.LinkageError("Method \"{2}.{3}{4}\" has a argument type \"{0}\" instead of type \"{1}\" as expected by \"{5}\"", there[i], here[i], cpi.GetClassType().Name, cpi.Name, cpi.Signature, classFile.Name);
#endif
				return "Loader constraints violated (arg " + i + "): " + mw.DeclaringType.Name + "." + mw.Name + mw.Signature;
			}
		}
		return null;
	}

	private static bool IsLoadLocal(NormalizedByteCode bc)
	{
		return bc == NormalizedByteCode.__aload ||
			bc == NormalizedByteCode.__iload ||
			bc == NormalizedByteCode.__lload ||
			bc == NormalizedByteCode.__fload ||
			bc == NormalizedByteCode.__dload ||
			bc == NormalizedByteCode.__iinc ||
			bc == NormalizedByteCode.__ret;
	}

	internal static bool IsStoreLocal(NormalizedByteCode bc)
	{
		return bc == NormalizedByteCode.__astore ||
			bc == NormalizedByteCode.__istore ||
			bc == NormalizedByteCode.__lstore ||
			bc == NormalizedByteCode.__fstore ||
			bc == NormalizedByteCode.__dstore ||
			bc == NormalizedByteCode.__fstore_conv ||
			bc == NormalizedByteCode.__dstore_conv;
	}

	private void VisitLocalLoads(List<LocalVar> locals, Dictionary<long, LocalVar> localByStoreSite, Dictionary<int, string> storeSites, int instructionIndex, bool debug)
	{
		Debug.Assert(IsLoadLocal(method.Instructions[instructionIndex].NormalizedOpCode));
		LocalVar local = null;
		TypeWrapper type = VerifierTypeWrapper.Null;
		int localIndex = method.Instructions[instructionIndex].NormalizedArg1;
		bool isArg = false;
		foreach(int store in storeSites.Keys)
		{
			if(store == -1)
			{
				// it's a method argument, it has no initial store, but the type is simply the parameter type
				type = InstructionState.FindCommonBaseType(type, state[0].GetLocalTypeEx(localIndex));
				isArg = true;
			}
			else
			{
				if(method.Instructions[store].NormalizedOpCode == NormalizedByteCode.__invokespecial)
				{
					type = InstructionState.FindCommonBaseType(type, GetLocalTypeWrapper(store + 1, localIndex));
				}
				else if(method.Instructions[store].NormalizedOpCode == NormalizedByteCode.__static_error)
				{
					// it's an __invokespecial that turned into a __static_error
					// (since a __static_error doesn't continue, we don't need to set type)
				}
				else
				{
					Debug.Assert(IsStoreLocal(method.Instructions[store].NormalizedOpCode));
					type = InstructionState.FindCommonBaseType(type, GetStackTypeWrapper(store, 0));
				}
			}
			// we can't have an invalid type, because that would have failed verification earlier
			Debug.Assert(type != VerifierTypeWrapper.Invalid);

			LocalVar l;
			if(localByStoreSite.TryGetValue(MakeKey(store, localIndex), out l))
			{
				if(local == null)
				{
					local = l;
				}
				else if(local != l)
				{
					// If we've already defined a LocalVar and we find another one, then we merge them
					// together.
					// This happens for the following code fragment:
					//
					// int i = -1;
					// try { i = 0; for(; ; ) System.out.println(i); } catch(Exception x) {}
					// try { i = 0; for(; ; ) System.out.println(i); } catch(Exception x) {}
					// System.out.println(i);
					//
					local = MergeLocals(locals, localByStoreSite, local, l);
				}
			}
		}
		if(local == null)
		{
			local = new LocalVar();
			local.local = localIndex;
			if(VerifierTypeWrapper.IsThis(type))
			{
				local.type = ((VerifierTypeWrapper)type).UnderlyingType;
			}
			else
			{
				local.type = type;
			}
			local.isArg = isArg;
			if(debug)
			{
				local.FindLvtEntry(method, instructionIndex);
			}
			locals.Add(local);
		}
		else
		{
			local.isArg |= isArg;
			local.type = InstructionState.FindCommonBaseType(local.type, type);
			Debug.Assert(local.type != VerifierTypeWrapper.Invalid);
		}
		foreach(int store in storeSites.Keys)
		{
			LocalVar v;
			if(!localByStoreSite.TryGetValue(MakeKey(store, localIndex), out v))
			{
				localByStoreSite[MakeKey(store, localIndex)] = local;
			}
			else if(v != local)
			{
				local = MergeLocals(locals, localByStoreSite, local, v);
			}
		}
	}

	private static long MakeKey(int i, int j)
	{
		return (((long)(uint)i) << 32) + (uint)j;
	}

	private static LocalVar MergeLocals(List<LocalVar> locals, Dictionary<long, LocalVar> localByStoreSite, LocalVar l1, LocalVar l2)
	{
		Debug.Assert(l1 != l2);
		Debug.Assert(l1.local == l2.local);
		for(int i = 0; i < locals.Count; i++)
		{
			if(locals[i] == l2)
			{
				locals.RemoveAt(i);
				i--;
			}
		}
		Dictionary<long, LocalVar> temp = new Dictionary<long,LocalVar>(localByStoreSite);
		localByStoreSite.Clear();
		foreach(KeyValuePair<long, LocalVar> kv in temp)
		{
			localByStoreSite[kv.Key] = kv.Value == l2 ? l1 : kv.Value;
		}
		l1.isArg |= l2.isArg;
		l1.type = InstructionState.FindCommonBaseType(l1.type, l2.type);
		Debug.Assert(l1.type != VerifierTypeWrapper.Invalid);
		return l1;
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

	internal int GetStackHeight(int index)
	{
		return state[index].GetStackHeight();
	}

	internal TypeWrapper GetStackTypeWrapper(int index, int pos)
	{
		TypeWrapper type = state[index].GetStackSlot(pos);
		if(VerifierTypeWrapper.IsThis(type))
		{
			type = ((VerifierTypeWrapper)type).UnderlyingType;
		}
		return type;
	}

	internal TypeWrapper GetRawStackTypeWrapper(int index, int pos)
	{
		return state[index].GetStackSlot(pos);
	}

	internal TypeWrapper GetLocalTypeWrapper(int index, int local)
	{
		return state[index].GetLocalTypeEx(local);
	}

	// NOTE for dead stores, this returns null
	internal LocalVar GetLocalVar(int instructionIndex)
	{
		return localVars[instructionIndex];
	}

	internal LocalVar[] GetLocalVarsForInvokeSpecial(int instructionIndex)
	{
		Debug.Assert(method.Instructions[instructionIndex].NormalizedOpCode == NormalizedByteCode.__invokespecial);
		return invokespecialLocalVars[instructionIndex];
	}

	internal LocalVar[] GetAllLocalVars()
	{
		return allLocalVars;
	}

	internal void ClearFaultBlockException(int instructionIndex)
	{
		Debug.Assert(state[instructionIndex].GetStackHeight() == 1);
		state[instructionIndex].ClearFaultBlockException();
	}

	internal ExceptionTableEntry[] GetExceptionTableFor(InstructionFlags[] flags)
	{
		List<ExceptionTableEntry> list = new List<ExceptionTableEntry>();
		// return only reachable exception handlers (because the code gen depends on that)
		for (int i = 0; i < exceptions.Length; i++)
		{
			// if the first instruction is unreachable, the entire block is unreachable,
			// because you can't jump into a block (we've just split the blocks to ensure that)
			if ((flags[exceptions[i].startIndex] & InstructionFlags.Reachable) != 0)
			{
				list.Add(exceptions[i]);
			}
		}
		return list.ToArray();
	}
}
