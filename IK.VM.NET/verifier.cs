/*
  Copyright (C) 2002 Jeroen Frijters

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
using System.Collections;

class VerifyError : ApplicationException
{
	internal int ByteCodeOffset;
	internal string Class;
	internal string Method;
	internal string Signature;
	internal string Instruction;

	internal VerifyError()
	{
	}

	internal VerifyError(string msg) : base(msg)
	{
	}
}

class Subroutine
{
	private int subroutineIndex;
	private bool[] localsModified;

	private Subroutine(int subroutineIndex, bool[] localsModified)
	{
		this.subroutineIndex = subroutineIndex;
		this.localsModified = localsModified;
	}

	internal Subroutine(int subroutineIndex, int maxLocals)
	{
		this.subroutineIndex = subroutineIndex;
		localsModified = new bool[maxLocals];
	}

	internal int SubroutineIndex
	{
		get
		{
			return subroutineIndex;
		}
	}

	internal bool[] LocalsModified
	{
		get
		{
			return localsModified;
		}
	}

	internal void SetLocalModified(int local)
	{
		localsModified[local] = true;
	}

	internal Subroutine Copy()
	{
		return new Subroutine(subroutineIndex, (bool[])localsModified.Clone());
	}
}

class InstructionState
{
	private static ArrayList empty = new ArrayList();
	private MethodAnalyzer ma;
	private ArrayList stack;			// each entry contains a TypeWrapper object
	private TypeWrapper[] locals;		// each entry contains a TypeWrapper object
	private ArrayList subroutines;
	private int callsites;
	internal bool changed = true;

	private InstructionState(MethodAnalyzer ma, ArrayList stack, TypeWrapper[] locals, ArrayList subroutines, int callsites)
	{
		this.ma = ma;
		this.stack = stack;
		this.locals = locals;
		this.subroutines = subroutines;
		this.callsites = callsites;
	}

	internal InstructionState(MethodAnalyzer ma, int maxLocals)
	{
		this.ma = ma;
		this.stack = new ArrayList();
		this.locals = new TypeWrapper[maxLocals];
		this.subroutines = new ArrayList();
	}

	internal InstructionState Copy()
	{
		return new InstructionState(ma, (ArrayList)stack.Clone(), (TypeWrapper[])locals.Clone(), CopySubroutines(subroutines), callsites);
	}

	internal InstructionState CopyLocals()
	{
		return new InstructionState(ma, new ArrayList(), (TypeWrapper[])locals.Clone(), new ArrayList(), callsites);
	}

	private ArrayList CopySubroutines(ArrayList l)
	{
		ArrayList n = new ArrayList(l.Count);
		foreach(Subroutine s in l)
		{
			n.Add(s.Copy());
		}
		return n;
	}

	public static InstructionState operator+(InstructionState s1, InstructionState s2)
	{
		return Merge(s1, s2, null, null);
	}

	private void MergeSubroutineHelper(InstructionState s2)
	{
		foreach(Subroutine ss2 in s2.subroutines)
		{
			bool found = false;
			foreach(Subroutine ss in subroutines)
			{
				if(ss.SubroutineIndex == ss2.SubroutineIndex)
				{
					for(int i = 0; i < ss.LocalsModified.Length; i++)
					{
						if(ss2.LocalsModified[i] && !ss.LocalsModified[i])
						{
							ss.LocalsModified[i] = true;
							changed = true;
						}
					}
					found = true;
				}
			}
			if(!found)
			{
				subroutines.Add(ss2.Copy());
				changed = true;
			}
		}
		if(s2.callsites > callsites)
		{
			//Console.WriteLine("s2.callsites = {0}, callsites = {1}", s2.callsites, callsites);
			callsites = s2.callsites;
			changed = true;
		}
	}

	internal static InstructionState Merge(InstructionState s1, InstructionState s2, bool[] locals_modified, InstructionState s3)
	{
		if(s1 == null)
		{
			s2 = s2.Copy();
			if(locals_modified != null)
			{
				for(int i = 0; i < s2.locals.Length; i++)
				{
					if(!locals_modified[i])
					{
						s2.locals[i] = s3.locals[i];
					}
				}
			}
			if(s3 != null)
			{
				s2.MergeSubroutineHelper(s3);
			}
			return s2;
		}
		if(s1.stack.Count != s2.stack.Count)
		{
			throw new VerifyError(string.Format("Inconsistent stack height: {0} != {1}", s1.stack.Count, s2.stack.Count));
		}
		InstructionState s = s1.Copy();
		s.changed = s1.changed;
		for(int i = 0; i < s.stack.Count; i++)
		{
			TypeWrapper type = (TypeWrapper)s.stack[i];
			if(type == s2.stack[i])
			{
				// perfect match, nothing to do
			}
			else if(!type.IsPrimitive)
			{
				TypeWrapper baseType = s.FindCommonBaseType(type, (TypeWrapper)s2.stack[i]);
				if(baseType == VerifierTypeWrapper.Invalid)
				{
					Console.WriteLine("type = " + type);
					Console.WriteLine("s2.stack[i] = " + s2.stack[i]);
					throw new InvalidOperationException();
				}
				if(type != baseType)
				{
					s.stack[i] = baseType;
					s.changed = true;
				}
			}
			else
			{
				throw new VerifyError(string.Format("cannot merge {0} and {1}", type.Name, ((TypeWrapper)s2.stack[i]).Name));
			}
		}
		for(int i = 0; i < s.locals.Length; i++)
		{
			TypeWrapper type = s.locals[i];
			TypeWrapper type2;
			if(locals_modified == null || locals_modified[i])
			{
				type2 = s2.locals[i];
			}
			else
			{
				type2 = s3.locals[i];
			}
			TypeWrapper baseType = s2.FindCommonBaseType(type, type2);
			if(type != baseType)
			{
				s.locals[i] = baseType;
				s.changed = true;
			}
		}
		s.MergeSubroutineHelper(s2);
		if(s3 != null)
		{
			s.MergeSubroutineHelper(s3);
		}
		return s;
	}

	internal void AddCallSite()
	{
		callsites++;
		changed = true;
	}

	internal void SetSubroutineId(int subroutineIndex)
	{
		foreach(Subroutine s in subroutines)
		{
			if(s.SubroutineIndex == subroutineIndex)
			{
				// subroutines cannot recursivly call themselves
				throw new VerifyError("subroutines cannot recurse");
			}
		}
		subroutines.Add(new Subroutine(subroutineIndex, locals.Length));
	}

	internal bool[] ClearSubroutineId(int subroutineIndex)
	{
		foreach(Subroutine s in subroutines)
		{
			if(s.SubroutineIndex == subroutineIndex)
			{
				// TODO i'm not 100% sure about this, but I think we need to clear
				// the subroutines here (because when you return you can never "become" inside a subroutine)
				subroutines.Clear();
				return s.LocalsModified;
			}
		}
		throw new VerifyError("return from wrong subroutine");
	}

	internal void CheckSubroutineActive(int subroutineIndex)
	{
		foreach(Subroutine s in subroutines)
		{
			if(s.SubroutineIndex == subroutineIndex)
			{
				return;
			}
		}
		throw new VerifyError("inactive subroutine");
	}

	internal TypeWrapper FindCommonBaseType(TypeWrapper type1, TypeWrapper type2)
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
		if(VerifierTypeWrapper.IsRet(type1) || VerifierTypeWrapper.IsRet(type2))
		{
			return VerifierTypeWrapper.Invalid;
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
			TypeWrapper baseType = FindCommonBaseTypeHelper(elem1, elem2);
			if(baseType == VerifierTypeWrapper.Invalid)
			{
				// TODO cache java.lang.Object type somewhere
				baseType = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassBySlashedName("java/lang/Object");
				rank--;
				if(rank == 0)
				{
					return baseType;
				}
			}
			// HACK load the array type
			return baseType.GetClassLoader().LoadClassBySlashedName(new String('[', rank) + "L" + baseType.Name + ";");
		}
		return FindCommonBaseTypeHelper(type1, type2);
	}

	private TypeWrapper FindCommonBaseTypeHelper(TypeWrapper t1, TypeWrapper t2)
	{
		if(t1 == t2)
		{
			return t1;
		}
		if(t1.IsInterface || t2.IsInterface)
		{
			// TODO I don't know how finding the common base for interfaces is defined, but
			// for now I'm just doing the naive thing
			// UPDATE according to a paper by Alessandro Coglio & Allen Goldberg titled
			// "Type Safety in the JVM: Some Problems in Java 2 SDK 1.2 and Proposed Solutions"
			// the common base of two interfaces is java/lang/Object, and there is special
			// treatment for java/lang/Object types that allow it to be assigned to any interface
			// type, the JVM's typesafety then depends on the invokeinterface instruction to make
			// sure that the reference actually implements the interface.
			// So strictly speaking, the code below isn't correct, but it works, so for now it stays in.
			if(t1.ImplementsInterface(t2))
			{
				return t2;
			}
			if(t2.ImplementsInterface(t1))
			{
				return t1;
			}
			// TODO cache java.lang.Object type somewhere
			return ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassBySlashedName("java/lang/Object");
		}
		Stack st1 = new Stack();
		Stack st2 = new Stack();
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
			t1 = st1.Count > 0 ? (TypeWrapper)st1.Pop() : null;
			t2 = st2.Count > 0 ? (TypeWrapper)st2.Pop() : null;
			if(t1 != t2)
			{
				return type;
			}
			type = t1;
		}
	}

	private void SetLocal1(int index, TypeWrapper type)
	{
		if(index > 0 && (locals[index - 1] == PrimitiveTypeWrapper.DOUBLE || locals[index - 1] == PrimitiveTypeWrapper.LONG))
		{
			locals[index - 1] = VerifierTypeWrapper.Invalid;
			foreach(Subroutine s in subroutines)
			{
				s.SetLocalModified(index - 1);
			}
		}
		locals[index] = type;
		foreach(Subroutine s in subroutines)
		{
			s.SetLocalModified(index);
		}
	}

	private void SetLocal2(int index, TypeWrapper type)
	{
		if(index > 0 && (locals[index - 1] == PrimitiveTypeWrapper.DOUBLE || locals[index - 1] == PrimitiveTypeWrapper.LONG))
		{
			locals[index - 1] = VerifierTypeWrapper.Invalid;
			foreach(Subroutine s in subroutines)
			{
				s.SetLocalModified(index - 1);
			}
		}
		locals[index] = type;
		locals[index + 1] = VerifierTypeWrapper.Invalid;
		foreach(Subroutine s in subroutines)
		{
			s.SetLocalModified(index);
			s.SetLocalModified(index + 1);
		}
	}

	internal void GetLocalInt(int index)
	{
		if(GetLocalType(index) != PrimitiveTypeWrapper.INT)
		{
			throw new VerifyError("Invalid local type");
		}
	}

	internal void SetLocalInt(int index)
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

	internal void SetLocalLong(int index)
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

	internal void SetLocalFloat(int index)
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

	internal void SetLocalDouble(int index)
	{
		SetLocal2(index, PrimitiveTypeWrapper.DOUBLE);
	}

	internal TypeWrapper GetLocalType(int index)
	{
		return locals[index];
	}

	internal int GetLocalRet(int index)
	{
		TypeWrapper type = GetLocalType(index);
		if(VerifierTypeWrapper.IsRet(type))
		{
			return ((VerifierTypeWrapper)type).Index;
		}
		throw new VerifyError("incorrect local type, not ret");
	}

	internal void SetLocalType(int index, TypeWrapper type)
	{
		if(type == PrimitiveTypeWrapper.DOUBLE || type == PrimitiveTypeWrapper.LONG)
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
		if(type.IsPrimitive)
		{
			if(type == PrimitiveTypeWrapper.BOOLEAN ||
				type == PrimitiveTypeWrapper.BYTE ||
				type == PrimitiveTypeWrapper.CHAR ||
				type == PrimitiveTypeWrapper.SHORT)
			{
				type = PrimitiveTypeWrapper.INT;
			}
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

	internal void PushDouble()
	{
		PushHelper(PrimitiveTypeWrapper.DOUBLE);
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
		if(PopAnyType() != PrimitiveTypeWrapper.FLOAT)
		{
			throw new VerifyError("Float expected on stack");
		}
	}

	internal void PopDouble()
	{
		if(PopAnyType() != PrimitiveTypeWrapper.DOUBLE)
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
		if(type != VerifierTypeWrapper.Null && type.ArrayRank == 0)
		{
			throw new VerifyError("Array reference expected on stack");
		}
		return type;
	}

	// null or an initialized object reference (or a subroutine return address)
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
		// is an interface, any reference will be accepted
		if(!baseType.IsInterface && !type.IsAssignableTo(baseType))
		{
			throw new VerifyError("Unexpected type " + type + " where " + baseType + " was expected");
		}
		return type;
	}

	internal TypeWrapper PopAnyType()
	{
		if(stack.Count == 0)
		{
			throw new VerifyError("Unable to pop operand off an empty stack");
		}
		TypeWrapper s = (TypeWrapper)stack[stack.Count - 1];
		stack.RemoveAt(stack.Count - 1);
		return s;
	}

	// NOTE this can *not* be used to pop double or long
	internal TypeWrapper PopType()
	{
		TypeWrapper type = PopAnyType();
		if(type == PrimitiveTypeWrapper.DOUBLE || type == PrimitiveTypeWrapper.LONG)
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
		if(baseType.IsPrimitive)
		{
			if(baseType == PrimitiveTypeWrapper.BOOLEAN ||
				baseType == PrimitiveTypeWrapper.BYTE ||
				baseType == PrimitiveTypeWrapper.CHAR ||
				baseType == PrimitiveTypeWrapper.SHORT)
			{
				baseType = PrimitiveTypeWrapper.INT;
			}
		}
		TypeWrapper type = PopAnyType();
		if(type != baseType && !type.IsAssignableTo(baseType))
		{
			// HACK because of the way interfaces references works, if baseType
			// is an interface, any reference will be accepted
			if(baseType.IsInterface && !type.IsPrimitive)
			{
				return type;
			}
			throw new VerifyError("Unexpected type " + type.Name + " where " + baseType.Name + " was expected");
		}
		return type;
	}

	internal int GetStackHeight()
	{
		return stack.Count;
	}

	internal TypeWrapper GetStackSlot(int pos)
	{
		return (TypeWrapper)stack[stack.Count - 1 - pos];
	}

	private void PushHelper(TypeWrapper type)
	{
		stack.Add(type);
	}

	internal void MarkInitialized(TypeWrapper type, TypeWrapper initType)
	{
		if(type == null || initType == null)
		{
			throw new InvalidOperationException();
		}
		for(int i = 0; i < locals.Length; i++)
		{
			if(locals[i] == type)
			{
				locals[i] = initType;
			}
		}
		for(int i = 0; i < stack.Count; i++)
		{
			if(stack[i] == type)
			{
				stack[i] = initType;
			}
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
		for(int i = 0; i < stack.Count; i++)
		{
			Console.Write(sep);
			Console.Write(stack[i]);
			sep = ", ";
		}
		Console.WriteLine();
	}

	internal void DumpSubroutines()
	{
		Console.Write("// subs: ");
		string sep = "";
		if(subroutines != null)
		{
			for(int i = 0; i < subroutines.Count; i++)
			{
				Console.Write(sep);
				Console.Write(((Subroutine)subroutines[i]).SubroutineIndex);
				sep = ", ";
			}
		}
		Console.WriteLine();
	}

	// this method ensures that no uninitialized object are in the current state
	internal void CheckUninitializedObjRefs()
	{
		for(int i = 0; i < locals.Length; i++)
		{
			TypeWrapper type = locals[i];
			if(type == VerifierTypeWrapper.UninitializedThis || VerifierTypeWrapper.IsNew(type))
			{
				throw new VerifyError("uninitialized object ref in local (2)");
			}
		}
		for(int i = 0; i < stack.Count; i++)
		{
			TypeWrapper type = (TypeWrapper)stack[i];
			if(type == VerifierTypeWrapper.UninitializedThis || VerifierTypeWrapper.IsNew(type))
			{
				throw new VerifyError("uninitialized object ref on stack");
			}
		}
	}
}

// this is a container for the special verifier TypeWrappers
class VerifierTypeWrapper : TypeWrapper
{
	internal static readonly TypeWrapper Invalid = null;
	internal static readonly TypeWrapper Null = new VerifierTypeWrapper("null", 0, null);
	internal static readonly TypeWrapper UninitializedThis = new VerifierTypeWrapper("this", 0, null);

	private int index;
	private TypeWrapper underlyingType;

	public override string ToString()
	{
		return GetType().Name + "[" + Name + "," + index + "," + underlyingType + "]";
	}

	internal static TypeWrapper MakeNew(TypeWrapper type, int bytecodeIndex)
	{
		return new VerifierTypeWrapper("new", bytecodeIndex, type);
	}

	internal static TypeWrapper MakeRet(int bytecodeIndex)
	{
		return new VerifierTypeWrapper("ret", bytecodeIndex, null);
	}

	internal static bool IsNew(TypeWrapper w)
	{
		return w != null && w.IsVerifierType && w.Name == "new";
	}

	internal static bool IsRet(TypeWrapper w)
	{
		return w != null && w.IsVerifierType && w.Name == "ret";
	}

	internal int Index
	{
		get
		{
			return index;
		}
	}

	internal TypeWrapper UnderlyingType
	{
		get
		{
			return underlyingType;
		}
	}

	private VerifierTypeWrapper(string name, int index, TypeWrapper underlyingType)
		: base(Modifiers.Final | Modifiers.Interface, name, null, null)
	{
		this.index = index;
		this.underlyingType = underlyingType;
	}

	protected override FieldWrapper GetFieldImpl(string fieldName)
	{
		throw new InvalidOperationException("GetFieldImpl called on " + this);
	}

	protected override MethodWrapper GetMethodImpl(MethodDescriptor md)
	{
		throw new InvalidOperationException("GetMethodImpl called on " + this);
	}

	public override Type Type
	{
		get
		{
			throw new InvalidOperationException("get_Type called on " + this);
		}
	}

	public override bool IsInterface
	{
		get
		{
			throw new InvalidOperationException("get_IsInterface called on " + this);
		}
	}

	public override TypeWrapper[] Interfaces
	{
		get
		{
			throw new InvalidOperationException("get_Interfaces called on " + this);
		}
	}

	public override void Finish()
	{
		throw new InvalidOperationException("Finish called on " + this);
	}
}

class MethodAnalyzer
{
	private static TypeWrapper java_lang_Throwable;
	private static TypeWrapper java_lang_String;
	private static TypeWrapper ByteArrayType;
	private static TypeWrapper BooleanArrayType;
	private static TypeWrapper ShortArrayType;
	private static TypeWrapper CharArrayType;
	private static TypeWrapper IntArrayType;
	private static TypeWrapper FloatArrayType;
	private static TypeWrapper DoubleArrayType;
	private static TypeWrapper LongArrayType;
	internal readonly ClassLoaderWrapper classLoader;
	private ClassFile.Method.Code method;
	private InstructionState[] state;
	private ArrayList[] callsites;
	private TypeWrapper[] localTypes;
	private bool[] aload_used;

	internal MethodAnalyzer(TypeWrapper wrapper, ClassFile.Method.Code method, ClassLoaderWrapper classLoader)
	{
		this.classLoader = classLoader;
		this.method = method;
		lock(GetType())
		{
			if(java_lang_Throwable == null)
			{
				java_lang_Throwable = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassBySlashedName("java/lang/Throwable");
				java_lang_String = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassBySlashedName("java/lang/String");
				ByteArrayType = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassBySlashedName("[B");
				BooleanArrayType = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassBySlashedName("[Z");
				ShortArrayType = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassBySlashedName("[S");
				CharArrayType = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassBySlashedName("[C");
				IntArrayType = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassBySlashedName("[I");
				FloatArrayType = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassBySlashedName("[F");
				DoubleArrayType = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassBySlashedName("[D");
				LongArrayType = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassBySlashedName("[J");
			}
		}
		state = new InstructionState[method.Instructions.Length];
		callsites = new ArrayList[method.Instructions.Length];
		localTypes = new TypeWrapper[method.MaxLocals];
		// HACK aload_used is used to track whether aload is ever used on a particular local (a very lame way of
		// trying to determine if a local that contains an exception, is ever used)
		// TODO we really need real liveness analyses for the locals
		aload_used = new Boolean[method.MaxLocals];

		// HACK because types have to have identity, the subroutine return address and new types are cached here
		Hashtable returnAddressTypes = new Hashtable();
		Hashtable newTypes = new Hashtable();

		// start by computing the initial state, the stack is empty and the locals contain the arguments
		state[0] = new InstructionState(this, method.MaxLocals);
		int arg = 0;
		if(!method.Method.IsStatic)
		{
			// this reference. If we're a constructor, the this reference is uninitialized.
			if(method.Method.Name == "<init>")
			{
				state[0].SetLocalType(arg++, VerifierTypeWrapper.UninitializedThis);
			}
			else
			{
				state[0].SetLocalType(arg++, wrapper);
			}
		}
		// HACK articial scope to make "args" name reusable
		if(true)
		{
			TypeWrapper[] args = method.Method.GetArgTypes(classLoader);
			for(int i = 0; i < args.Length; i++)
			{
				TypeWrapper type = args[i];
				if(type.IsPrimitive &&
					(type == PrimitiveTypeWrapper.BOOLEAN ||
					type == PrimitiveTypeWrapper.BYTE ||
					type == PrimitiveTypeWrapper.CHAR ||
					type == PrimitiveTypeWrapper.SHORT))
				{
					type = PrimitiveTypeWrapper.INT;
				}
				state[0].SetLocalType(arg++, type);
				if(type == PrimitiveTypeWrapper.DOUBLE || type == PrimitiveTypeWrapper.LONG)
				{
					arg++;
				}
			}
		}
		bool done = false;
		while(!done)
		{
			done = true;
			for(int i = 0; i < method.Instructions.Length; i++)
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
							if(method.ExceptionTable[j].start_pc <= method.Instructions[i].PC && method.ExceptionTable[j].end_pc > method.Instructions[i].PC)
							{
								// NOTE this used to be CopyLocalsAndSubroutines, but it doesn't (always) make
								// sense to copy the subroutine state
								// TODO figure out if there are circumstances under which it does make sense
								// to copy the active subroutine state
								InstructionState ex = state[i].CopyLocals();
								int catch_type = method.ExceptionTable[j].catch_type;
								if(catch_type == 0)
								{
									ex.PushType(java_lang_Throwable);
								}
								else
								{
									// TODO if the exception type is unloadable we should consider pushing
									// Throwable as the type and recording a loader constraint
									ex.PushType(GetConstantPoolClassType(catch_type));
								}
								int idx = method.PcIndexMap[method.ExceptionTable[j].handler_pc];
								state[idx] += ex;
							}
						}
						InstructionState s = state[i].Copy();
						ClassFile.Method.Instruction instr = method.Instructions[i];
						switch(instr.NormalizedOpCode)
						{
							case NormalizedByteCode.__aload:
							{
								aload_used[instr.NormalizedArg1] = true;
								TypeWrapper type = s.GetLocalType(instr.NormalizedArg1);
								if(type.IsPrimitive || type == VerifierTypeWrapper.Invalid)
								{
									throw new VerifyError("Object reference expected");
								}
								s.PushType(type);
								break;
							}
							case NormalizedByteCode.__astore:
							{
								// NOTE since the reference can be uninitialized, we cannot use PopObjectType
								TypeWrapper type = s.PopType();
								if(type.IsPrimitive)
								{
									throw new VerifyError("Object reference expected");
								}
								s.SetLocalType(instr.NormalizedArg1, type);
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
								if(type != VerifierTypeWrapper.Null &&
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
								if(type != VerifierTypeWrapper.Null &&
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
								s.PushType((GetFieldref(instr.Arg1)).GetFieldType(classLoader));
								break;
							case NormalizedByteCode.__putstatic:
								s.PopType(GetFieldref(instr.Arg1).GetFieldType(classLoader));
								break;
							case NormalizedByteCode.__getfield:
								s.PopObjectType(GetFieldref(instr.Arg1).GetClassType(classLoader));
								s.PushType(GetFieldref(instr.Arg1).GetFieldType(classLoader));
								break;
							case NormalizedByteCode.__putfield:
								s.PopType(GetFieldref(instr.Arg1).GetFieldType(classLoader));
								s.PopObjectType(GetFieldref(instr.Arg1).GetClassType(classLoader));
								break;
							case NormalizedByteCode.__ldc:
							{
								object o = GetConstantPoolConstant(instr.Arg1);
								if(o is int)
								{
									s.PushInt();
								}
								else if(o is string)
								{
									s.PushType(java_lang_String);
								}
								else if(o is long)
								{
									s.PushLong();
								}
								else if(o is float)
								{
									s.PushFloat();
								}
								else if(o is double)
								{
									s.PushDouble();
								}
								else
								{
									throw new InvalidOperationException(o.GetType().FullName);
								}
								break;
							}
							case NormalizedByteCode.__invokevirtual:
							case NormalizedByteCode.__invokespecial:
							case NormalizedByteCode.__invokeinterface:
							case NormalizedByteCode.__invokestatic:
							{
								ClassFile.ConstantPoolItemFMI cpi = GetMethodref(instr.Arg1);
								if((cpi is ClassFile.ConstantPoolItemInterfaceMethodref) != (instr.NormalizedOpCode == NormalizedByteCode.__invokeinterface))
								{
									throw new VerifyError("Illegal constant pool index");
								}
								if(instr.NormalizedOpCode != NormalizedByteCode.__invokespecial && cpi.Name == "<init>")
								{
									throw new VerifyError("Must call initializers using invokespecial");
								}
								if(cpi.Name == "<clinit>")
								{
									throw new VerifyError("Illegal call to internal method");
								}
								TypeWrapper[] args = cpi.GetArgTypes(classLoader);
								for(int j = args.Length - 1; j >= 0; j--)
								{
									s.PopType(args[j]);
								}
								if(instr.NormalizedOpCode == NormalizedByteCode.__invokeinterface)
								{
									// TODO check arg (should match signature)
									// error: "Inconsistent args size"
								}
								if(instr.NormalizedOpCode != NormalizedByteCode.__invokestatic)
								{
									// TODO we should verify that in a constructor, the base class constructor is actually called
									if(cpi.Name == "<init>")
									{
										TypeWrapper type = s.PopType();
										if((VerifierTypeWrapper.IsNew(type) && ((VerifierTypeWrapper)type).UnderlyingType != cpi.GetClassType(classLoader)) ||
											(type == VerifierTypeWrapper.UninitializedThis && cpi.GetClassType(classLoader) != method.Method.ClassFile.GetSuperTypeWrapper(classLoader) && cpi.GetClassType(classLoader) != wrapper) ||
											(!VerifierTypeWrapper.IsNew(type) && type != VerifierTypeWrapper.UninitializedThis))
										{
											// TODO oddly enough, Java fails verification for the class without
											// even running the constructor, so maybe constructors are always
											// verified...
											// NOTE when a constructor isn't verifiable, the static initializer
											// doesn't run either (or so I believe)
											throw new VerifyError("Call to wrong initialization method");
										}
										// after the constructor invocation, the uninitialized reference, is now
										// suddenly initialized
										if(type == VerifierTypeWrapper.UninitializedThis)
										{
											s.MarkInitialized(type, wrapper);
										}
										else
										{
											s.MarkInitialized(type, ((VerifierTypeWrapper)type).UnderlyingType);
										}
									}
									else
									{
										// NOTE previously we checked the type here, but it turns out that
										// the JVM throws an IncompatibleClassChangeError at runtime instead
										// of a VerifyError if this doesn't match
										s.PopObjectType();
									}
								}
								TypeWrapper retType = cpi.GetRetType(classLoader);
								if(retType != PrimitiveTypeWrapper.VOID)
								{
									s.PushType(retType);
								}
								break;
							}
							case NormalizedByteCode.__goto:
								break;
							case NormalizedByteCode.__istore:
								s.PopInt();
								s.SetLocalInt(instr.NormalizedArg1);
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
								s.PopFloat();
								s.PushFloat();
								break;
							case NormalizedByteCode.__fadd:
							case NormalizedByteCode.__fsub:
							case NormalizedByteCode.__fmul:
							case NormalizedByteCode.__fdiv:
							case NormalizedByteCode.__frem:
								s.PopFloat();
								s.PopFloat();
								s.PushFloat();
								break;
							case NormalizedByteCode.__dneg:
								s.PopDouble();
								s.PushDouble();
								break;
							case NormalizedByteCode.__dadd:
							case NormalizedByteCode.__dsub:
							case NormalizedByteCode.__dmul:
							case NormalizedByteCode.__ddiv:
							case NormalizedByteCode.__drem:
								s.PopDouble();
								s.PopDouble();
								s.PushDouble();
								break;
							case NormalizedByteCode.__new:
							{
								// mark the type, so that we can ascertain that it is a "new object"
								TypeWrapper type = (TypeWrapper)newTypes[instr.PC];
								if(type == null)
								{
									type = VerifierTypeWrapper.MakeNew(GetConstantPoolClassType(instr.Arg1), instr.PC);
									newTypes[instr.PC] = type;
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
								string name = type.Name;
								if(name[0] != '[')
								{
									name = "L" + name + ";";
								}
								if(type.IsUnloadable)
								{
									s.PushType(new UnloadableTypeWrapper(name));
								}
								else
								{
									s.PushType(type.GetClassLoader().LoadClassBySlashedName("[" + name));
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
								if(t == PrimitiveTypeWrapper.DOUBLE || t == PrimitiveTypeWrapper.LONG)
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
								if(value1 == PrimitiveTypeWrapper.DOUBLE || value1 == PrimitiveTypeWrapper.LONG)
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
								TypeWrapper value2 = s.PopType();
								TypeWrapper value3 = s.PopType();
								s.PushType(value1);
								s.PushType(value3);
								s.PushType(value2);
								s.PushType(value1);
								break;
							}
							case NormalizedByteCode.__dup2_x2:
							{
								TypeWrapper value1 = s.PopAnyType();
								if(value1 == PrimitiveTypeWrapper.DOUBLE || value1 == PrimitiveTypeWrapper.LONG)
								{
									TypeWrapper value2 = s.PopAnyType();
									if(value2 == PrimitiveTypeWrapper.DOUBLE || value2 == PrimitiveTypeWrapper.LONG)
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
									if(value3 == PrimitiveTypeWrapper.DOUBLE || value3 == PrimitiveTypeWrapper.LONG)
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
								if(type != PrimitiveTypeWrapper.DOUBLE && type != PrimitiveTypeWrapper.LONG)
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
								if(method.Method.GetRetType(classLoader) != PrimitiveTypeWrapper.VOID)
								{
									throw new VerifyError("Wrong return type in function");
								}
								break;
							case NormalizedByteCode.__areturn:
								s.PopObjectType(method.Method.GetRetType(classLoader));
								break;
							case NormalizedByteCode.__ireturn:
							{
								s.PopInt();
								TypeWrapper retType = method.Method.GetRetType(classLoader);
								if(retType != PrimitiveTypeWrapper.BOOLEAN &&
									retType != PrimitiveTypeWrapper.BYTE &&
									retType != PrimitiveTypeWrapper.CHAR &&
									retType != PrimitiveTypeWrapper.SHORT &&
									retType != PrimitiveTypeWrapper.INT)
								{
									throw new VerifyError("Wrong return type in function");
								}
								break;
							}
							case NormalizedByteCode.__lreturn:
								s.PopLong();
								if(method.Method.GetRetType(classLoader) != PrimitiveTypeWrapper.LONG)
								{
									throw new VerifyError("Wrong return type in function");
								}
								break;
							case NormalizedByteCode.__freturn:
								s.PopFloat();
								if(method.Method.GetRetType(classLoader) != PrimitiveTypeWrapper.FLOAT)
								{
									throw new VerifyError("Wrong return type in function");
								}
								break;
							case NormalizedByteCode.__dreturn:
								s.PopDouble();
								if(method.Method.GetRetType(classLoader) != PrimitiveTypeWrapper.DOUBLE)
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
								s.SetLocalFloat(instr.NormalizedArg1);
								break;
							case NormalizedByteCode.__dload:
								s.GetLocalDouble(instr.NormalizedArg1);
								s.PushDouble();
								break;
							case NormalizedByteCode.__dstore:
								s.PopDouble();
								s.SetLocalDouble(instr.NormalizedArg1);
								break;
							case NormalizedByteCode.__lload:
								s.GetLocalLong(instr.NormalizedArg1);
								s.PushLong();
								break;
							case NormalizedByteCode.__lstore:
								s.PopLong();
								s.SetLocalLong(instr.NormalizedArg1);
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
								s.PopObjectType(java_lang_Throwable);
								break;
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
							case NormalizedByteCode.__jsr:
								// TODO make sure we're not calling a subroutine we're already in
								break;
							case NormalizedByteCode.__ret:
							{
								// TODO if we're returning from a higher level subroutine, invalidate
								// all the intermediate return addresses
								int subroutineIndex = s.GetLocalRet(instr.Arg1);
								s.CheckSubroutineActive(subroutineIndex);
								break;
							}
							case NormalizedByteCode.__nop:
								if(i + 1 == method.Method.CodeAttribute.Instructions.Length)
								{
									throw new VerifyError("Falling off the end of the code");
								}
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
								case NormalizedByteCode.__lookupswitch:
									for(int j = 0; j < instr.Values.Length; j++)
									{
										state[method.PcIndexMap[instr.PC + instr.TargetOffsets[j]]] += s;
									}
									state[method.PcIndexMap[instr.PC + instr.DefaultOffset]] += s;
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
									if(instr.Arg1 < 0)
									{
										// backward branches cannot have uninitialized objects on
										// the stack or in local variables
										s.CheckUninitializedObjRefs();
									}
									state[i + 1] += s;
									state[method.PcIndexMap[instr.PC + instr.Arg1]] += s;
									break;
								case NormalizedByteCode.__goto:
									if(instr.Arg1 < 0)
									{
										// backward branches cannot have uninitialized objects on
										// the stack or in local variables
										s.CheckUninitializedObjRefs();
									}
									state[method.PcIndexMap[instr.PC + instr.Arg1]] += s;
									break;
								case NormalizedByteCode.__jsr:
								{
									int index = method.PcIndexMap[instr.PC + instr.Arg1];
									s.SetSubroutineId(index);
									TypeWrapper retAddressType = (TypeWrapper)returnAddressTypes[index];
									if(retAddressType == null)
									{
										retAddressType = VerifierTypeWrapper.MakeRet(index);
										returnAddressTypes[index] = retAddressType;
									}
									s.PushType(retAddressType);
									state[index] += s;
									AddCallSite(index, i);
									break;
								}
								case NormalizedByteCode.__ret:
								{
									// HACK if the ret is processed before all of the jsr instructions to this subroutine
									// we wouldn't be able to properly merge, so that is why we track the number of callsites
									// for each subroutine instruction (see Instruction.AddCallSite())
									int subroutineIndex = s.GetLocalRet(instr.Arg1);
									int[] cs = GetCallSites(subroutineIndex);
									bool[] locals_modified = s.ClearSubroutineId(subroutineIndex);
									for(int j = 0; j < cs.Length; j++)
									{
										state[cs[j] + 1] = InstructionState.Merge(state[cs[j] + 1], s, locals_modified, state[cs[j]]);
									}
									break;
								}
								case NormalizedByteCode.__ireturn:
								case NormalizedByteCode.__lreturn:
								case NormalizedByteCode.__freturn:
								case NormalizedByteCode.__dreturn:
								case NormalizedByteCode.__areturn:
								case NormalizedByteCode.__return:
								case NormalizedByteCode.__athrow:
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
						// HACK track the local types (but only for object references)
						for(int j = 0; j < localTypes.Length ; j++)
						{
							TypeWrapper l = s.GetLocalType(j);
							if(l != VerifierTypeWrapper.Invalid)
							{
								if(l == VerifierTypeWrapper.UninitializedThis)
								{
									localTypes[j] = wrapper;
								}
								else if(VerifierTypeWrapper.IsNew(l))
								{
									localTypes[j] = ((VerifierTypeWrapper)l).UnderlyingType;
								}
								else if(!VerifierTypeWrapper.IsRet(l) && !l.IsPrimitive)
								{
									if(localTypes[j] == VerifierTypeWrapper.Invalid)
									{
										localTypes[j] = l;
									}
									else
									{
										localTypes[j] = s.FindCommonBaseType(localTypes[j], l);
									}
								}
							}
						}
					}
					catch(VerifyError x)
					{
						x.Class = method.Method.ClassFile.Name;
						x.Method = method.Method.Name;
						x.Signature = method.Method.Signature;
						x.ByteCodeOffset = method.Instructions[i].PC;
						string opcode = method.Instructions[i].OpCode.ToString();
						if(opcode.StartsWith("__"))
						{
							opcode = opcode.Substring(2);
						}
						x.Instruction = opcode;
						Console.WriteLine(x);
						/*
						for(int j = 0; j < method.Instructions.Length; j++)
						{
							//state[j].DumpLocals();
							//state[j].DumpStack();
							if(state[j] != null)
							{
								state[j].DumpSubroutines();
								Console.WriteLine("{0}: {1}", method.Instructions[j].PC, method.Instructions[j].OpCode.ToString());
							}
						}
						*/
						throw;
					}
				}
			}
		}		
	}

	private ClassFile.ConstantPoolItemFMI GetMethodref(int index)
	{
		try
		{
			ClassFile.ConstantPoolItemFMI item = method.Method.ClassFile.GetMethodref(index);
			if(item != null && !(item is ClassFile.ConstantPoolItemFieldref))
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
			ClassFile.ConstantPoolItemFieldref item = method.Method.ClassFile.GetFieldref(index);
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

	private object GetConstantPoolConstant(int index)
	{
		try
		{
			return method.Method.ClassFile.GetConstantPoolConstant(index);
		}
		catch(IndexOutOfRangeException)
		{
		}
		throw new VerifyError("Illegal constant pool index");
	}

	private string GetConstantPoolClass(int index)
	{
		try
		{
			return method.Method.ClassFile.GetConstantPoolClass(index);
		}
		catch(InvalidCastException)
		{
		}
		catch(IndexOutOfRangeException)
		{
		}
		throw new VerifyError("Illegal constant pool index");
	}

	private TypeWrapper GetConstantPoolClassType(int index)
	{
		try
		{
			return method.Method.ClassFile.GetConstantPoolClassType(index, classLoader);
		}
		catch(InvalidCastException)
		{
		}
		catch(IndexOutOfRangeException)
		{
		}
		throw new VerifyError("Illegal constant pool index");
	}

	private void AddCallSite(int subroutineIndex, int callSiteIndex)
	{
		if(callsites[subroutineIndex] == null)
		{
			callsites[subroutineIndex] = new ArrayList();
		}
		ArrayList l = (ArrayList)callsites[subroutineIndex];
		if(l.IndexOf(callSiteIndex) == -1)
		{
			l.Add(callSiteIndex);
			state[subroutineIndex].AddCallSite();
		}
	}

	internal int[] GetCallSites(int subroutineIndex)
	{
		return (int[])((ArrayList)callsites[subroutineIndex]).ToArray(typeof(int));
	}

	internal int GetStackHeight(int index)
	{
		return state[index].GetStackHeight();
	}

	internal TypeWrapper GetRawStackTypeWrapper(int index, int pos)
	{
		return state[index].GetStackSlot(pos);
	}

	internal TypeWrapper GetLocalTypeWrapper(int index, int local)
	{
		return state[index].GetLocalType(local);
	}

	internal TypeWrapper GetDeclaredLocalTypeWrapper(int local)
	{
		return localTypes[local];
	}

	internal bool IsAloadUsed(int local)
	{
		return aload_used[local];
	}
}
