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
	private ArrayList stack;			// each entry contains a string with a Java signature of the type
	private string[] locals;			// each entry contains a string with a Java signature of the type
	private ArrayList subroutines;
	private int callsites;
	internal bool changed = true;

	private InstructionState(MethodAnalyzer ma, ArrayList stack, string[] locals, ArrayList subroutines, int callsites)
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
		this.locals = new string[maxLocals];
		this.subroutines = new ArrayList();
	}

	internal InstructionState Copy()
	{
		return new InstructionState(ma, (ArrayList)stack.Clone(), (string[])locals.Clone(), CopySubroutines(subroutines), callsites);
	}

	internal InstructionState CopyLocals()
	{
		return new InstructionState(ma, new ArrayList(), (string[])locals.Clone(), new ArrayList(), callsites);
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
			string type = (string)s.stack[i];
			if(type == (string)s2.stack[i])
			{
				// perfect match, nothing to do
			}
			else if(type[0] == 'L' || type[0] == '[' || type[0] == 'U' || type[0] == 'N')
			{
				string baseType = s.FindCommonBaseType(type, (string)s2.stack[i]);
				if(type != baseType)
				{
					s.stack[i] = baseType;
					s.changed = true;
				}
			}
			else
			{
				throw new VerifyError(string.Format("cannot merge {0} and {1}", type, s2.stack[i]));
			}
		}
		for(int i = 0; i < s.locals.Length; i++)
		{
			string type = s.locals[i];
			string type2;
			if(locals_modified == null || locals_modified[i])
			{
				type2 = s2.locals[i];
			}
			else
			{
				type2 = s3.locals[i];
			}
			if(type == type2)
			{
				// perfect match, nothing to do
			}
			else if(type != null)
			{
				if(type[0] == 'L' || type[0] == '[')
				{
					string baseType = s2.FindCommonBaseType(type, type2);
					if(type != baseType)
					{
						s.locals[i] = baseType;
						s.changed = true;
					}
				}
				else
				{
					// mark the slot as invalid
					s.locals[i] = null;
					s.changed = true;
				}
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

	private bool IsSubType(string subType, string baseType)
	{
		if(subType == baseType)
		{
			return true;
		}
		if(subType.Length == 1 || baseType.Length == 1)
		{
			// primitives can never be subtypes of another type
			return false;
		}
		if(baseType == "Ljava/lang/Object;")
		{
			return true;
		}
		if(baseType[0] == '[')
		{
			if(subType[0] != '[')
			{
				return false;
			}
			int subDepth = 0;
			while(subType[subDepth] == '[')
			{
				subDepth++;
			}
			int baseDepth = 0;
			while(baseType[baseDepth] == '[')
			{
				baseDepth++;
			}
			if(baseDepth > subDepth)
			{
				return false;
			}
			if(baseDepth < subDepth)
			{
				return baseType.EndsWith("[Ljava/lang/Object;");
			}
			if(subType[subDepth] == baseType[baseDepth])
			{
				if(subType[subDepth] != 'L')
				{
					return baseDepth == subDepth;
				}
				string baseElemType = baseType.Substring(baseDepth + 1, baseType.Length - baseDepth - 2);
				if(baseElemType == "java/lang/Object")
				{
					return true;
				}
				if(baseDepth == subDepth)
				{
					string subElemType = subType.Substring(subDepth + 1, subType.Length - subDepth - 2);
					return ma.classLoader.IsSubType(subElemType, baseElemType);
				}
			}
			return false;
		}
		else if(subType[0] == '[')
		{
			return false;
		}
		return ma.classLoader.IsSubType(subType.Substring(1, subType.Length - 2), baseType.Substring(1, baseType.Length - 2));
	}

	internal string FindCommonBaseType(string type1, string type2)
	{
//		Console.WriteLine("FindCommonBaseType: {0} v {1}", type1, type2);
		if(type1 == "Lnull")
		{
			return type2;
		}
		if(type2 == "Lnull")
		{
			return type1;
		}
		if(type1 == type2)
		{
			return type1;
		}
		if(type1 == null || type2 == null)
		{
			return null;
		}
		if(type1.Length == 1 || type2.Length == 1)
		{
			return null;
		}
		if(type1[0] == '[' || type2[0] == '[')
		{
			int rank1 = 0;
			while(type1[rank1] == '[')
			{
				rank1++;
			}
			int rank2 = 0;
			while(type2[rank2] == '[')
			{
				rank2++;
			}
			if(rank1 == 0 || rank2 == 0)
			{
				return "Ljava/lang/Object;";
			}
			if(rank1 != rank2)
			{
				if(rank1 > rank2)
				{
					int temp = rank1;
					rank1 = rank2;
					rank2 = temp;
					string temps = type1;
					type1 = type2;
					type2 = temps;
				}
				if(type1.EndsWith("Ljava/lang/Object;"))
				{
					return type1;
				}
				return "Ljava/lang/Object;";
			}
			else
			{
				if(type1[rank1] != type2[rank1])
				{
					// two different primitive arrays, or a primitive and a reference array
					return "Ljava/lang/Object;";
				}
				return new String('[', rank1) + "L" + ma.classLoader.FindCommonBaseType(type1.Substring(1 + rank1, type1.Length - (2 + rank1)), type2.Substring(1 + rank2, type2.Length - (2 + rank2))) + ";";
			}
		}
		if(type1.StartsWith("Lret;") || type2.StartsWith("Lret;"))
		{
			return null;
		}
		return "L" + ma.classLoader.FindCommonBaseType(type1.Substring(1, type1.Length - 2), type2.Substring(1, type2.Length - 2)) + ";";
	}

	private void SetLocal1(int index, string type)
	{
		if(index > 0 && (locals[index] == "D2" || locals[index] == "J2"))
		{
			locals[index - 1] = null;
		}
		locals[index] = type;
		foreach(Subroutine s in subroutines)
		{
			s.SetLocalModified(index);
		}
	}

	private void SetLocal2(int index, string type)
	{
		if(index > 0 && (locals[index] == "D2" || locals[index] == "J2"))
		{
			locals[index - 1] = null;
		}
		locals[index] = type;
		locals[index + 1] = type[0] == 'D' ? "D2" : "J2";
		foreach(Subroutine s in subroutines)
		{
			s.SetLocalModified(index);
			s.SetLocalModified(index + 1);
		}
	}

	internal void GetLocalInt(int index)
	{
		if(locals[index] != "I")
		{
			throw new VerifyError("Invalid local type");
		}
	}

	internal void SetLocalInt(int index)
	{
		SetLocal1(index, "I");
	}

	internal void SetLocalLong(int index)
	{
		SetLocal2(index, "J");
	}

	internal void GetLocalLong(int index)
	{
		if(locals[index] != "J" || locals[index + 1] != "J2")
		{
			throw new VerifyError("incorrect local type, not long");
		}
	}

	internal void GetLocalFloat(int index)
	{
		if(locals[index] != "F")
		{
			throw new VerifyError("incorrect local type, not float");
		}
	}

	internal void SetLocalFloat(int index)
	{
		SetLocal1(index, "F");
	}

	internal void SetLocalDouble(int index)
	{
		SetLocal2(index, "D");
	}

	internal void GetLocalDouble(int index)
	{
		if(locals[index] != "D" || locals[index + 1] != "D2")
		{
			throw new VerifyError("incorrect local type, not double");
		}
	}

	internal string GetLocalType(int index)
	{
		return locals[index];
	}

	internal int GetLocalRet(int index)
	{
		string type = locals[index];
		if(!type.StartsWith("Lret;"))
		{
			throw new VerifyError("incorrect local type, not ret");
		}
		return int.Parse(type.Substring(5));
	}

	internal string GetLocalObject(int index)
	{
		string s = locals[index];
		if(s == null || (s[0] != 'L' && s[0] != '[' && s[0] != 'U' && s[0] != 'N') || s.StartsWith("Lret;"))
		{
			throw new VerifyError("incorrect local type, not object");
		}
		return s;
	}

	internal void SetLocalObject(int index, string type)
	{
		if(type[0] != 'L' && type[0] != '[' && type[0] != 'U' && type[0] != 'N')
		{
			throw new VerifyError("SetLocalObject");
		}
		SetLocal1(index, type);
	}

	internal void Push(string type)
	{
		if(type.Length == 1)
		{
			switch(type[0])
			{
				case 'Z':
				case 'B':
				case 'C':
				case 'S':
					type = "I";
					break;
			}
		}
		PushHelper(type);
	}

	internal void PushObject(string type)
	{
		if(type == null)
		{
			throw new VerifyError("PushObject null");
		}
		if(type[0] == 'L' || type[0] == '[' || type[0] == 'U' || type[0] == 'N')
		{
			PushHelper(type);
			return;
		}
		throw new VerifyError("PushObject not object");
	}

	internal void PushInt()
	{
		PushHelper("I");
	}

	internal void PushLong()
	{
		PushHelper("J");
	}

	internal void PushFloat()
	{
		PushHelper("F");
	}

	internal void PushDouble()
	{
		PushHelper("D");
	}

	internal void PopInt()
	{
		Pop('I');
	}

	internal void PopFloat()
	{
		Pop('F');
	}

	internal void PopDouble()
	{
		Pop('D');
	}

	internal void PopLong()
	{
		Pop('J');
	}

	internal string PopArray()
	{
		string s = PopHelper();
		if(s[0] == '[' || s == "Lnull")
		{
			return s;
		}
		throw new VerifyError("Array type expected");
	}

	internal string PopUninitializedObject(string type)
	{
		string s = PopHelper();
		string u = s;
		if(s[0] != 'U' && s[0] != 'N')
		{
			throw new VerifyError("Expecting to find unitialized object on stack");
		}
		s = s.Substring(s.IndexOf(';') + 1);
		if(s != type)
		{
			if(IsSubType(s, type))
			{
				// OK
			}
			else
			{
				throw new VerifyError(string.Format("popped {0} and expected {1}", s, type));
			}
		}
		return u;
	}

	internal void Pop(string type)
	{
		if(type.Length == 1)
		{
			switch(type[0])
			{
				case 'Z':
				case 'B':
				case 'C':
				case 'S':
					Pop('I');
					return;
			}
		}
		string s = PopHelper();
		if(s != type)
		{
			if((type[0] == 'L' || type[0] == '[') && s == "Lnull")
			{
			}
			else if(IsSubType(s, type))
			{
			}
			else
			{
				throw new VerifyError(string.Format("popped {0} and expected {1}", s, type));
			}
		}
	}

	internal string PopObject(string type)
	{
		string s = PopHelper();
		if(s != type)
		{
			if(s == "Lnull")
			{
				// null can be used as any type
			}
			else if(s[0] == 'N' || s[0] == 'U')
			{
				throw new VerifyError("Unexpected unitialized objref " + s);
			}
			else if(IsSubType(s, type))
			{
				// OK
			}
			else
			{
				throw new VerifyError(string.Format("popped {0} and expected {1}", s, type));
			}
		}
		return s;
	}

	internal string PopAny()
	{
		return PopHelper();
	}

	internal string Pop()
	{
		string type = PopHelper();
		if(type == "D" || type == "J")
		{
			throw new VerifyError("Attempt to split long or double on the stack");
		}
		return type;
	}

	internal string Pop2()
	{
		string type = PopHelper();
		if(type == "D" || type == "J")
		{
			return type;
		}
		type = PopHelper();
		if(type == "D" || type == "J")
		{
			throw new VerifyError("Attempt to split long or double on the stack");
		}
		return type;
	}

	internal int GetStackHeight()
	{
		return stack.Count;
	}

	internal string GetStackSlot(int pos)
	{
		return (string)stack[stack.Count - 1 - pos];
	}

	internal string Peek()
	{
		if(stack.Count == 0)
		{
			// return null, if the stack is empty
			return null;
		}
		return (string)stack[stack.Count - 1];
	}

	private string PopHelper()
	{
		if(stack.Count == 0)
		{
			throw new VerifyError("Unable to pop operand off an empty stack");
		}
		string s = (string)stack[stack.Count - 1];
		stack.RemoveAt(stack.Count - 1);
		return s;
	}

	private void PushHelper(string s)
	{
		if(s.IndexOf("L[") >= 0)
		{
			throw new VerifyError("Internal error: L[ type found");
		}
		stack.Add(s);
	}

	private string Pop(char type)
	{
		string s = PopHelper();
		if(s[0] != type)
		{
			switch(type)
			{
				case 'I':
					throw new VerifyError("Expecting to find int on stack");
				case '[':
					throw new VerifyError("Expecting to find array on stack");
				case 'L':
					throw new VerifyError("Expecting to find object on stack");
				case 'F':
					throw new VerifyError("Expecting to find float on stack");
				case 'D':
					throw new VerifyError("Expecting to find double on stack");
				case 'J':
					throw new VerifyError("Expecting to find long on stack");
				default:
					throw new VerifyError("Expecting to find " + type + " on stack");
			}
		}
		return s;
	}

	internal void MarkInitialized(string type)
	{
		string initType = type.Substring(type.IndexOf(';') + 1);
		for(int i = 0; i < locals.Length; i++)
		{
			if(locals[i] == type)
			{
				locals[i] = initType;
			}
		}
		for(int i = 0; i < stack.Count; i++)
		{
			if((string)stack[i] == type)
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
			if(locals[i] != null && (((locals[i])[0] == 'U') || ((locals[i])[0] == 'N')))
			{
				throw new VerifyError("uninitialized object ref in local (2)");
			}
		}
		for(int i = 0; i < stack.Count; i++)
		{
			if((((string)stack[i])[0] == 'U') || (((string)stack[i])[0] == 'N'))
			{
				throw new VerifyError("uninitialized object ref on stack");
			}
		}
	}

	// this method ensures that no uninitialized objects, of the specified type are in the current state
	internal void CheckUninitializedObjRefs(string type)
	{
		for(int i = 0; i < locals.Length; i++)
		{
			if(locals[i] == type)
			{
				throw new VerifyError("unininitialized " + type + " in locals");
			}
		}
		for(int i = 0; i < stack.Count; i++)
		{
			if((string)stack[i] == type)
			{
				throw new VerifyError("uninitialized " + type + " on stack");
			}
		}
	}
}

class SigEnumerator
{
	private string sig;
	private int pos;
	private int length;

	internal SigEnumerator(string sig)
	{
		this.sig = sig;
		pos = 1;
		length = 0;
	}

	internal bool MoveNext()
	{
		pos += length;
		switch(sig[pos])
		{
			case 'L':
			{
				length = sig.IndexOf(';', pos) - pos + 1;
				break;
			}
			case '[':
			{
				length = 0;
				while(sig[pos + length] == '[') length++;
				if(sig[pos + length] == 'L')
				{
					length = sig.IndexOf(';', pos) - pos;
				}
				length++;
				break;
			}
			case ')':
				length = 0;
				return false;
			default:
				length = 1;
				break;
		}
		return true;
	}

	internal string Current
	{
		get
		{
			return sig.Substring(pos, length);
		}
	}
}

class ReverseSigEnumerator
{
	private string[] items;
	private int pos;

	internal ReverseSigEnumerator(string sig)
	{
		ArrayList ar = new ArrayList();
		SigEnumerator se = new SigEnumerator(sig);
		while(se.MoveNext())
		{
			ar.Add(se.Current);
		}
		items = new String[ar.Count];
		ar.CopyTo(items);
		pos = items.Length;
	}

	internal bool MoveNext()
	{
		pos--;
		return pos >= 0;
	}

	internal string Current
	{
		get
		{
			return items[pos];
		}
	}
}

class MethodAnalyzer
{
	internal readonly ClassLoaderWrapper classLoader;
	private ClassFile.Method.Code method;
	private InstructionState[] state;
	private ArrayList[] callsites;
	private string[] localTypes;
	private bool[] aload_used;

	internal MethodAnalyzer(ClassFile.Method.Code method, ClassLoaderWrapper classLoader)
	{
		this.classLoader = classLoader;
		this.method = method;
		state = new InstructionState[method.Instructions.Length];
		callsites = new ArrayList[method.Instructions.Length];
		localTypes = new String[method.MaxLocals];
		// HACK aload_used is used to track whether aload is ever used on a particular local (a very lame way of
		// trying to determine if a local that contains an exception, is ever used)
		// TODO we really need real liveness analyses for the locals
		aload_used = new Boolean[method.MaxLocals];

		// start by computing the initial state, the stack is empty and the locals contain the arguments
		state[0] = new InstructionState(this, method.MaxLocals);
		int arg = 0;
		if(!method.Method.IsStatic)
		{
			// this reference. If we're a constructor, the this reference is uninitialized.
			if(method.Method.Name == "<init>")
			{
				state[0].SetLocalObject(arg++, "U0;L" + method.Method.ClassFile.Name + ";");
			}
			else
			{
				state[0].SetLocalObject(arg++, "L" + method.Method.ClassFile.Name + ";");
			}
		}
		string sig = method.Method.Signature;
		for(int i = 1; sig[i] != ')'; i++)
		{
			switch(sig[i])
			{
				case 'D':
					state[0].SetLocalDouble(arg);
					arg += 2;
					break;
				case 'J':
					state[0].SetLocalLong(arg);
					arg += 2;
					break;
				case 'L':
				{
					int pos = sig.IndexOf(';', i);
					state[0].SetLocalObject(arg++, sig.Substring(i, pos - i + 1));
					i = pos;
					break;
				}
				case '[':
				{
					int start = i;
					while(sig[i] == '[') i++;
					if(sig[i] == 'L')
					{
						i = sig.IndexOf(';', i);
					}
					state[0].SetLocalObject(arg++, sig.Substring(start, i - start + 1));
					break;
				}
				case 'F':
					state[0].SetLocalFloat(arg++);
					break;
				case 'Z':
				case 'B':
				case 'S':
				case 'C':
				case 'I':
					state[0].SetLocalInt(arg++);
					break;
				default:
					throw new NotImplementedException();
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
									ex.PushObject("Ljava/lang/Throwable;");
								}
								else
								{
									ex.PushObject("L" + GetConstantPoolClass(catch_type) + ";");
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
								aload_used[instr.NormalizedArg1] = true;
								s.PushObject(s.GetLocalObject(instr.NormalizedArg1));
								break;
							case NormalizedByteCode.__astore:
							{
								string type = s.Pop();
								switch(type[0])
								{
									case 'L':
									case '[':
									case 'N':
									case 'U':
										s.SetLocalObject(instr.NormalizedArg1, type);
										break;
									default:
										throw new VerifyError("Object reference expected");
								}
								break;
							}
							case NormalizedByteCode.__aconst_null:
								s.PushObject("Lnull");
								break;
							case NormalizedByteCode.__aaload:
							{
								s.PopInt();
								string type = s.PopArray();
								if(type == "Lnull")
								{
									// if the array is null, we have use null as the element type, because
									// otherwise the rest of the code will not verify correctly
									s.PushObject(type);
								}
								else
								{
									s.PushObject(type.Substring(1));
								}
								break;
							}
							case NormalizedByteCode.__aastore:
							{
								string elem = s.PopObject("Ljava/lang/Object;");
								s.PopInt();
								string type = s.PopArray();
								// TODO check that elem is assignable to the array
								break;
							}
							case NormalizedByteCode.__baload:
							{
								s.PopInt();
								string type = s.PopArray();
								if(type[1] != 'B' && type[1] != 'Z' && type != "Lnull")
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
								string type = s.PopArray();
								if(type[1] != 'B' && type[1] != 'Z' && type != "Lnull")
								{
									throw new VerifyError();
								}
								break;
							}
							case NormalizedByteCode.__caload:
								s.PopInt();
								s.PopObject("[C");
								s.PushInt();
								break;
							case NormalizedByteCode.__castore:
								s.PopInt();
								s.PopInt();
								s.PopObject("[C");
								break;
							case NormalizedByteCode.__saload:
								s.PopInt();
								s.PopObject("[S");
								s.PushInt();
								break;
							case NormalizedByteCode.__sastore:
								s.PopInt();
								s.PopInt();
								s.PopObject("[S");
								break;
							case NormalizedByteCode.__iaload:
								s.PopInt();
								s.PopObject("[I");
								s.PushInt();
								break;
							case NormalizedByteCode.__iastore:
								s.PopInt();
								s.PopInt();
								s.PopObject("[I");
								break;
							case NormalizedByteCode.__laload:
								s.PopInt();
								s.PopObject("[J");
								s.PushLong();
								break;
							case NormalizedByteCode.__lastore:
								s.PopLong();
								s.PopInt();
								s.PopObject("[J");
								break;
							case NormalizedByteCode.__daload:
								s.PopInt();
								s.PopObject("[D");
								s.PushDouble();
								break;
							case NormalizedByteCode.__dastore:
								s.PopDouble();
								s.PopInt();
								s.PopObject("[D");
								break;
							case NormalizedByteCode.__faload:
								s.PopInt();
								s.PopObject("[F");
								s.PushFloat();
								break;
							case NormalizedByteCode.__fastore:
								s.PopFloat();
								s.PopInt();
								s.PopObject("[F");
								break;
							case NormalizedByteCode.__arraylength:
								s.PopArray();
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
								s.PopObject("Ljava/lang/Object;");
								break;
							case NormalizedByteCode.__if_acmpeq:
							case NormalizedByteCode.__if_acmpne:
								// TODO it might be legal to use an unitialized ref here
								s.PopObject("Ljava/lang/Object;");
								s.PopObject("Ljava/lang/Object;");
								break;
							case NormalizedByteCode.__getstatic:
								s.Push((GetFieldref(instr.Arg1)).Signature);
								break;
							case NormalizedByteCode.__putstatic:
							{
								// HACK because of the way interface merging works, if the
								// type on the stack is Object, it can be assigned to anything
								// (the compiler will emit a cast)
								string type = (GetFieldref(instr.Arg1)).Signature;
								if(type[0] == 'L' && s.Peek() == "Ljava/lang/Object;")
								{
									s.Pop();
								}
								else
								{
									s.Pop(type);
								}
								break;
							}
							case NormalizedByteCode.__getfield:
								s.PopObject("L" + (GetFieldref(instr.Arg1)).Class + ";");
								s.Push((GetFieldref(instr.Arg1)).Signature);
								break;
							case NormalizedByteCode.__putfield:
							{
								// HACK because of the way interface merging works, if the
								// type on the stack is Object, it can be assigned to anything
								// (the compiler will emit a cast)
								string type = (GetFieldref(instr.Arg1)).Signature;
								if(type[0] == 'L' && s.Peek() == "Ljava/lang/Object;")
								{
									s.Pop();
								}
								else
								{
									s.Pop(type);
								}
								s.PopObject("L" + (GetFieldref(instr.Arg1)).Class + ";");
								break;
							}
							case NormalizedByteCode.__ldc:
							{
								object o = GetConstantPoolConstant(instr.Arg1);
								if(o is int)
								{
									s.PushInt();
								}
								else if(o is string)
								{
									s.PushObject("Ljava/lang/String;");
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
								ReverseSigEnumerator rse = new ReverseSigEnumerator(cpi.Signature);
								while(rse.MoveNext())
								{
									switch(rse.Current[0])
									{
										case 'Z':
										case 'B':
										case 'S':
										case 'C':
										case 'I':
											s.PopInt();
											break;
										case 'J':
											s.PopLong();
											break;
										case 'D':
											s.PopDouble();
											break;
										case 'F':
											s.PopFloat();
											break;
										case 'L':
										{
											// HACK if the return type is an interface, any object is legal
											if(classLoader.RetTypeFromSig("()" + rse.Current).IsInterface)
											{
												// TODO implement support in the compiler for this condition (the code that
												// is currently generated works, but isn't verifiable)
												s.PopObject("Ljava/lang/Object;");
											}
											else
											{
												s.PopObject(rse.Current);
											}
											break;
										}
										case '[':
											s.PopObject(rse.Current);
											break;
										default:
											throw new NotImplementedException(rse.Current);
									}
								}
								if(instr.NormalizedOpCode == NormalizedByteCode.__invokeinterface)
								{
									// TODO check arg (should match signature)
									// error: "Inconsistent args size"
								}
								if(instr.NormalizedOpCode != NormalizedByteCode.__invokestatic)
								{
									if(cpi.Name == "<init>")
									{
										string type = s.PopUninitializedObject("L" + cpi.Class + ";");
										if((type[0] == 'N' && !type.EndsWith("L" + cpi.Class + ";")) ||
											(type[0] == 'U' && cpi.Class != method.Method.ClassFile.SuperClass && cpi.Class != method.Method.ClassFile.Name))
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
										s.MarkInitialized(type);
									}
									else
									{
										// NOTE previously we checked the type here, but it turns out that
										// the JVM throws an IncompatibleClassChangeError at runtime instead
										// of a VerifyError if this doesn't match
										s.PopObject("Ljava/lang/Object;");
										/*
										string type = cpi.Class;
										if(type[0] != '[')
										{
											type = "L" + type + ";";
										}
										// invokeinterface is allowed on java/lang/Object (because merging interfaces is
										// complicated), this will generate a runtime cast in the compiler
										if(instr.NormalizedOpCode == NormalizedByteCode.__invokeinterface &&
											s.Peek() == "Ljava/lang/Object;")
										{
											s.PopAny();
										}
										else
										{
											// TODO if this fails it shouldn't generate a VerifyError, but instead
											// an IncompatibleClassChangeError (at run time)
											s.PopObject(type);
										}
										*/
									}
								}
								string ret = cpi.Signature.Substring(cpi.Signature.IndexOf(')') + 1);
								switch(ret[0])
								{
									case 'Z':
									case 'B':
									case 'S':
									case 'C':
									case 'I':
										s.PushInt();
										break;
									case 'J':
										s.PushLong();
										break;
									case 'D':
										s.PushDouble();
										break;
									case 'F':
										s.PushFloat();
										break;
									case 'L':
									case '[':
										s.PushObject(ret);
										break;
									case 'V':
										break;
									default:
										throw new NotImplementedException(ret);
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
								// mark the type, so that we can ascertain that it is a "new object"
								s.PushObject(string.Format("N{0};L{1};", instr.PC, GetConstantPoolClass(instr.Arg1)));
								break;
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
								string type = GetConstantPoolClass(instr.Arg1);
								if(!type.StartsWith(new String('[', instr.Arg2)))
								{
									throw new VerifyError("Illegal dimension argument");
								}
								s.PushObject(type);
								break;
							}
							case NormalizedByteCode.__anewarray:
							{
								s.PopInt();
								string type = GetConstantPoolClass(instr.Arg1);
								if(type[0] != '[')
								{
									type = "L" + type + ";";
								}
								s.PushObject("[" + type);
								break;
							}
							case NormalizedByteCode.__newarray:
								s.PopInt();
							switch(instr.Arg1)
							{
								case 4:
									s.PushObject("[Z");
									break;
								case 5:
									s.PushObject("[C");
									break;
								case 6:
									s.PushObject("[F");
									break;
								case 7:
									s.PushObject("[D");
									break;
								case 8:
									s.PushObject("[B");
									break;
								case 9:
									s.PushObject("[S");
									break;
								case 10:
									s.PushObject("[I");
									break;
								case 11:
									s.PushObject("[J");
									break;
								default:
									throw new VerifyError("Bad type");
							}
								break;
							case NormalizedByteCode.__swap:
							{
								string t1 = s.Pop();
								string t2 = s.Pop();
								s.Push(t1);
								s.Push(t2);
								break;
							}
							case NormalizedByteCode.__dup:
							{
								string t = s.Pop();
								s.Push(t);
								s.Push(t);
								break;
							}
							case NormalizedByteCode.__dup2:
							{
								string t = s.PopAny();
								if(t == "D" || t == "J")
								{
									s.Push(t);
									s.Push(t);
								}
								else
								{
									string t2 = s.Pop();
									s.Push(t2);
									s.Push(t);
									s.Push(t2);
									s.Push(t);
								}
								break;
							}
							case NormalizedByteCode.__dup_x1:
							{
								string value1 = s.Pop();
								string value2 = s.Pop();
								s.Push(value1);
								s.Push(value2);
								s.Push(value1);
								break;
							}
							case NormalizedByteCode.__dup2_x1:
							{
								string value1 = s.PopAny();
								if(value1 == "D" || value1 == "J")
								{
									string value2 = s.Pop();
									s.Push(value1);
									s.Push(value2);
									s.Push(value1);
								}
								else
								{
									string value2 = s.Pop();
									string value3 = s.Pop();
									s.Push(value2);
									s.Push(value1);
									s.Push(value3);
									s.Push(value2);
									s.Push(value1);
								}
								break;
							}
							case NormalizedByteCode.__dup_x2:
							{
								string value1 = s.Pop();
								string value2 = s.Pop();
								string value3 = s.Pop();
								s.Push(value1);
								s.Push(value3);
								s.Push(value2);
								s.Push(value1);
								break;
							}
							case NormalizedByteCode.__dup2_x2:
							{
								string value1 = s.PopAny();
								if(value1 == "D" || value1 == "J")
								{
									string value2 = s.PopAny();
									if(value2 == "D" || value2 == "J")
									{
										// Form 4
										s.Push(value1);
										s.Push(value2);
										s.Push(value1);
									}
									else
									{
										// Form 2
										string value3 = s.Pop();
										s.Push(value1);
										s.Push(value3);
										s.Push(value2);
										s.Push(value1);
									}
								}
								else
								{
									string value2 = s.Pop();
									string value3 = s.PopAny();
									if(value3 == "D" || value3 == "J")
									{
										// Form 3
										s.Push(value2);
										s.Push(value1);
										s.Push(value3);
										s.Push(value2);
										s.Push(value1);
									}
									else
									{
										// Form 4
										string value4 = s.Pop();
										s.Push(value2);
										s.Push(value1);
										s.Push(value4);
										s.Push(value3);
										s.Push(value2);
										s.Push(value1);
									}
								}
								break;
							}
							case NormalizedByteCode.__pop:
								s.Pop();
								break;
							case NormalizedByteCode.__pop2:
								s.Pop2();
								break;
							case NormalizedByteCode.__monitorenter:
							case NormalizedByteCode.__monitorexit:
								// TODO is this allowed to be an uninitialized object?
								s.PopObject("Ljava/lang/Object;");
								break;
							case NormalizedByteCode.__return:
								if(method.Method.Signature.Substring(method.Method.Signature.IndexOf(')') + 1) != "V")
								{
									throw new VerifyError("Wrong return type in function");
								}
								break;
							case NormalizedByteCode.__areturn:
							{
								// HACK if the return type is an interface, any object is legal
								if(classLoader.RetTypeFromSig(method.Method.Signature).IsInterface)
								{
									s.PopObject("Ljava/lang/Object;");
								}
								else
								{
									s.PopObject(method.Method.Signature.Substring(method.Method.Signature.IndexOf(')') + 1));
								}
								break;
							}
							case NormalizedByteCode.__ireturn:
								s.PopInt();
								switch(method.Method.Signature.Substring(method.Method.Signature.IndexOf(')') + 1))
								{
									case "Z":
									case "B":
									case "S":
									case "C":
									case "I":
										break;
									default:
										throw new VerifyError("Wrong return type in function");
								}
								break;
							case NormalizedByteCode.__lreturn:
								s.PopLong();
								if(method.Method.Signature.Substring(method.Method.Signature.IndexOf(')') + 1) != "J")
								{
									throw new VerifyError("Wrong return type in function");
								}
								break;
							case NormalizedByteCode.__freturn:
								s.PopFloat();
								if(method.Method.Signature.Substring(method.Method.Signature.IndexOf(')') + 1) != "F")
								{
									throw new VerifyError("Wrong return type in function");
								}
								break;
							case NormalizedByteCode.__dreturn:
								s.PopDouble();
								if(method.Method.Signature.Substring(method.Method.Signature.IndexOf(')') + 1) != "D")
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
							{
								s.PopObject("Ljava/lang/Object;");
								string type = GetConstantPoolClass(instr.Arg1);
								if(type[0] != '[')
								{
									type = "L" + type + ";";
								}
								s.PushObject(type);
								break;
							}
							case NormalizedByteCode.__instanceof:
								s.PopObject("Ljava/lang/Object;");
								s.PushInt();
								break;
							case NormalizedByteCode.__iinc:
								s.GetLocalInt(instr.Arg1);
								break;
							case NormalizedByteCode.__athrow:
								s.PopObject("Ljava/lang/Throwable;");
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
									s.Push("Lret;" + index);
									s.SetSubroutineId(index);
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
							string l = s.GetLocalType(j);
							if(l != null && (l[0] == 'U' || l[0] == 'N' || l[0] == 'L' || l[0] == '[') && !l.StartsWith("Lret;"))
							{
								if(l[0] == 'U' || l[0] == 'N')
								{
									l = l.Substring(l.IndexOf(';') + 1);
								}
								if(localTypes[j] == null)
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

	internal string GetRawStackType(int index, int pos)
	{
		return state[index].GetStackSlot(pos);
	}

	internal string GetLocalType(int index, int local)
	{
		return state[index].GetLocalType(local);
	}

	internal string GetDeclaredLocalType(int local)
	{
		return localTypes[local];
	}

	internal bool IsAloadUsed(int local)
	{
		return aload_used[local];
	}
}
