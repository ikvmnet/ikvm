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
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics.SymbolStore;
using ExceptionTableEntry = ClassFile.Method.ExceptionTableEntry;
using Instruction = ClassFile.Method.Instruction;

class ReturnCookie
{
	public Label Stub;
	public LocalBuilder Local;
}

class BranchCookie
{
	public Label Stub;
	public int TargetIndex;
	public Stack Stack = new Stack();
}

class ExceptionSorter : IComparer
{
	public int Compare(object x, object y)
	{
		ExceptionTableEntry e1 = (ExceptionTableEntry)x;
		ExceptionTableEntry e2 = (ExceptionTableEntry)y;
		if(e1.start_pc < e2.start_pc)
		{
			return -1;
		}
		if(e1.start_pc == e2.start_pc)
		{
			if(e1.end_pc == e2.end_pc)
			{
				if(e1.ordinal > e2.ordinal)
				{
					return -1;
				}
				return 1;
			}
			if(e1.end_pc > e2.end_pc)
			{
				return -1;
			}
		}
		return 1;
	}
}

class Compiler
{
	private static MethodInfo mapExceptionMethod = typeof(ExceptionHelper).GetMethod("MapException");
	private static MethodInfo mapExceptionFastMethod = typeof(ExceptionHelper).GetMethod("MapExceptionFast");
	private static MethodInfo fillInStackTraceMethod = typeof(ExceptionHelper).GetMethod("fillInStackTrace");
	private static MethodInfo getTypeFromHandleMethod = typeof(Type).GetMethod("GetTypeFromHandle");
	private static MethodInfo multiANewArrayMethod = typeof(ByteCodeHelper).GetMethod("multianewarray");
	private static MethodInfo monitorEnterMethod = typeof(ByteCodeHelper).GetMethod("monitorenter");
	private static MethodInfo monitorExitMethod = typeof(ByteCodeHelper).GetMethod("monitorexit");
	private static MethodInfo throwHack = typeof(ExceptionHelper).GetMethod("ThrowHack");
	private TypeWrapper clazz;
	private ClassFile.Method.Code m;
	private ILGenerator ilGenerator;
	private ClassLoaderWrapper classLoader;
	private MethodAnalyzer ma;
	private Hashtable locals = new Hashtable();
	private ClassFile.Method.ExceptionTableEntry[] exceptions;
	private ISymbolDocumentWriter symboldocument;

	private Compiler(TypeWrapper clazz, ClassFile.Method.Code m, ILGenerator ilGenerator, ClassLoaderWrapper classLoader)
	{
		this.clazz = clazz;
		this.m = m;
		this.ilGenerator = ilGenerator;
		this.classLoader = classLoader;
		if(JVM.Debug)
		{
			string sourcefile = m.Method.ClassFile.SourceFileAttribute;
			if(sourcefile != null)
			{
				this.symboldocument = classLoader.ModuleBuilder.DefineDocument(sourcefile, Guid.Empty, Guid.Empty, Guid.Empty);
			}
		}
		Profiler.Enter("MethodAnalyzer");
		ma = new MethodAnalyzer(m, classLoader);
		Profiler.Leave("MethodAnalyzer");
		ArrayList ar = new ArrayList(m.ExceptionTable);
//		Console.WriteLine("before processing:");
//		foreach(ExceptionTableEntry e in ar)
//		{
//			Console.WriteLine("{0} to {1} handler {2}", e.start_pc, e.end_pc, e.handler_pc);
//		}
		// TODO it's very bad practice to mess with ExceptionTableEntrys that are owned by the Method, yet we
		// do that here, should be changed to use our own ETE class (which should also contain the ordinal, instead
		// of the one in ClassFile.cs)
		// OPTIMIZE there must be a more efficient algorithm to do this...
	restart:
		for(int i = 0; i < ar.Count; i++)
		{
			ExceptionTableEntry ei = (ExceptionTableEntry)ar[i];
			for(int j = i + 1; j < ar.Count; j++)
			{
				ExceptionTableEntry ej = (ExceptionTableEntry)ar[j];
				if(ei.start_pc <= ej.start_pc && ei.end_pc > ej.start_pc)
				{
					// try1.j
					if(ej.end_pc > ei.end_pc)
					{
						ExceptionTableEntry emi = new ExceptionTableEntry();
						emi.start_pc = ej.start_pc;
						emi.end_pc = ei.end_pc;
						emi.catch_type = ei.catch_type;
						emi.handler_pc = ei.handler_pc;
						ExceptionTableEntry emj = new ExceptionTableEntry();
						emj.start_pc = ej.start_pc;
						emj.end_pc = ei.end_pc;
						emj.catch_type = ej.catch_type;
						emj.handler_pc = ej.handler_pc;
						ei.end_pc = emi.start_pc;
						ej.start_pc = emj.end_pc;
						ar.Insert(j, emj);
						ar.Insert(i + 1, emi);
						goto restart;
					}
					else if(ej.end_pc < ei.end_pc)	// try2.j
					{
						ExceptionTableEntry emi = new ExceptionTableEntry();
						emi.start_pc = ej.start_pc;
						emi.end_pc = ej.end_pc;
						emi.catch_type = ei.catch_type;
						emi.handler_pc = ei.handler_pc;
						ExceptionTableEntry eei = new ExceptionTableEntry();
						eei.start_pc = ej.end_pc;
						eei.end_pc = ei.end_pc;
						eei.catch_type = ei.catch_type;
						eei.handler_pc = ei.handler_pc;
						ei.end_pc = emi.start_pc;
						ar.Insert(i + 1, eei);
						ar.Insert(i + 1, emi);
						goto restart;
					}
				}
			}
		}
		// __jsr inside a try block (to a PC outside the try block) causes the try
		// block to be broken into two blocks surrounding the __jsr
		// This is actually pretty common. Take, for example, the following code:
		//	class hello
		//	{
		//		public static void main(String[] args)
		//		{
		//			try
		//			{
		//				for(;;)
		//				{
		//					if(args.length == 0) return;
		//				}
		//			}
		//			finally
		//			{
		//				System.out.println("Hello, world!");
		//			}
		//		}
		//	}
	restart_jsr:
		for(int i = 0; i < ar.Count; i++)
		{
			ExceptionTableEntry ei = (ExceptionTableEntry)ar[i];
			for(int j = FindPcIndex(ei.start_pc), e = FindPcIndex(ei.end_pc); j < e; j++)
			{
				if(m.Instructions[j].NormalizedOpCode == NormalizedByteCode.__jsr)
				{
					int targetPC = m.Instructions[j].NormalizedArg1 + m.Instructions[j].PC;
					if(targetPC < ei.start_pc || targetPC >= ei.end_pc)
					{
						ExceptionTableEntry en = new ExceptionTableEntry();
						en.catch_type = ei.catch_type;
						en.handler_pc = ei.handler_pc;
						en.start_pc = (ushort)m.Instructions[j + 1].PC;
						en.end_pc = ei.end_pc;
						ei.end_pc = (ushort)m.Instructions[j].PC;
						ar.Insert(i + 1, en);
						goto restart_jsr;
					}
				}
			}
		}
		// Split try blocks at branch targets (branches from outside the try block)
		for(int i = 0; i < ar.Count; i++)
		{
			ExceptionTableEntry ei = (ExceptionTableEntry)ar[i];
			int start = FindPcIndex(ei.start_pc);
			int end = FindPcIndex(ei.end_pc);
			for(int j = 0; j < m.Instructions.Length; j++)
			{
				if(j < start || j >= end)
				{
					switch(m.Instructions[j].NormalizedOpCode)
					{
						case NormalizedByteCode.__lookupswitch:
							// TODO if the switch branches out of the try block, that should be handled too
//							for(int j = 0; j < instr.Values.Length; j++)
//							{
//								state[FindPcIndex(instr.PC + instr.TargetOffsets[j])] += s;
//							}
//							state[FindPcIndex(instr.PC + instr.DefaultOffset)] += s;
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
						case NormalizedByteCode.__jsr:
						{
							int targetPC = m.Instructions[j].PC + m.Instructions[j].Arg1;
							if(targetPC > ei.start_pc && targetPC < ei.end_pc)
							{
								ExceptionTableEntry en = new ExceptionTableEntry();
								en.catch_type = ei.catch_type;
								en.handler_pc = ei.handler_pc;
								en.start_pc = (ushort)targetPC;
								en.end_pc = ei.end_pc;
								ei.end_pc = (ushort)targetPC;
								ar.Insert(i + 1, en);
								goto restart_jsr;
							}
							break;
						}
					}
				}
			}
		}
		// exception handlers are also a kind of jump, so we need to split try blocks around handlers as well
		for(int i = 0; i < ar.Count; i++)
		{
			ExceptionTableEntry ei = (ExceptionTableEntry)ar[i];
			// TODO verify that we don't need to start at j = 0
			for(int j = i; j < ar.Count; j++)
			{
				ExceptionTableEntry ej = (ExceptionTableEntry)ar[j];
				if(ej.handler_pc > ei.start_pc && ej.handler_pc < ei.end_pc)
				{
					ExceptionTableEntry en = new ExceptionTableEntry();
					en.catch_type = ei.catch_type;
					en.handler_pc = ei.handler_pc;
					en.start_pc = (ushort)ej.handler_pc;
					en.end_pc = ei.end_pc;
					ei.end_pc = (ushort)ej.handler_pc;
					ar.Insert(i + 1, en);
					goto restart_jsr;
				}
			}
		}
		// filter out zero length try blocks
		for(int i = 0; i < ar.Count; i++)
		{
			ExceptionTableEntry ei = (ExceptionTableEntry)ar[i];
			if(ei.start_pc == ei.end_pc)
			{
				ar.RemoveAt(i);
				i--;
			}
		}
//		Console.WriteLine("after processing:");
//		foreach(ExceptionTableEntry e in ar)
//		{
//			Console.WriteLine("{0} to {1} handler {2}", e.start_pc, e.end_pc, e.handler_pc);
//		}

		exceptions = new ExceptionTableEntry[ar.Count];
		ar.CopyTo(exceptions, 0);
		for(int i = 0; i < exceptions.Length; i++)
		{
			exceptions[i].ordinal = i;
		}
		Array.Sort(exceptions, new ExceptionSorter());

		// TODO remove these checks, if the above exception untangling is correct, this shouldn't ever
		// be triggered
		for(int i = 0; i < exceptions.Length; i++)
		{
			for(int j = i + 1; j < exceptions.Length; j++)
			{
				// check for partially overlapping try blocks (which is legal for the JVM, but not the CLR)
				if(exceptions[i].start_pc < exceptions[j].start_pc && 
					exceptions[j].start_pc < exceptions[i].end_pc &&
					exceptions[i].end_pc < exceptions[j].end_pc)
				{
					throw new Exception("Partially overlapping try blocks is broken");
				}
				// check that we didn't destroy the ordering, when sorting
				if(exceptions[i].start_pc <= exceptions[j].start_pc &&
					exceptions[i].end_pc >= exceptions[j].end_pc &&
					exceptions[i].ordinal < exceptions[j].ordinal)
				{
					throw new Exception("Non recursive try blocks is broken");
				}
			}
			// make sure __jsr doesn't jump out of try block
			for(int j = FindPcIndex(exceptions[i].start_pc), e = FindPcIndex(exceptions[i].end_pc); j < e; j++)
			{
				if(m.Instructions[j].NormalizedOpCode == NormalizedByteCode.__jsr)
				{
					int targetPC = m.Instructions[j].NormalizedArg1 + m.Instructions[j].PC;
					if(targetPC < exceptions[i].start_pc || targetPC >= exceptions[i].end_pc)
					{
						Console.WriteLine("i = " + i);
						Console.WriteLine("j = " + j);
						Console.WriteLine("targetPC = " + targetPC);
						throw new Exception("Try block splitting around __jsr is broken");
					}
				}
			}
		}
	}

	private struct DupHelper
	{
		private ClassLoaderWrapper classLoader;
		private ILGenerator ilgen;
		private bool[] isnull;
		private LocalBuilder[] locals;

		internal DupHelper(ClassLoaderWrapper classLoader, ILGenerator ilgen, int count)
		{
			this.classLoader = classLoader;
			this.ilgen = ilgen;
			isnull = new bool[count];
			locals = new LocalBuilder[count];
		}

		internal DupHelper SetType(int i, string type)
		{
			if(type == "Lnull")
			{
				isnull[i] = true;
			}
			else if(type[0] != 'N')
			{
				// TODO handle class not found
				locals[i] = ilgen.DeclareLocal(classLoader.ExpressionType(type));
			}
			return this;
		}

		internal DupHelper Load(int i)
		{
			if(isnull[i])
			{
				ilgen.Emit(OpCodes.Ldnull);
			}
			else if(locals[i] != null)
			{
				ilgen.Emit(OpCodes.Ldloc, locals[i]);
			}
			return this;
		}

		internal DupHelper Store(int i)
		{
			if(isnull[i])
			{
				ilgen.Emit(OpCodes.Pop);
			}
			else if(locals[i] != null)
			{
				ilgen.Emit(OpCodes.Stloc, locals[i]);
			}
			return this;
		}
	}

	internal static void Compile(TypeWrapper clazz, ClassFile.Method m, ILGenerator ilGenerator, ClassLoaderWrapper classLoader)
	{
		Compiler c;
		try
		{
			Profiler.Enter("new Compiler");
			c = new Compiler(clazz, m.CodeAttribute, ilGenerator, classLoader);
			Profiler.Leave("new Compiler");
		}
		catch(VerifyError x)
		{
			// because in Java the method is only verified if it is actually called,
			// we generate code here to throw the VerificationError
			Type verifyError = ClassLoaderWrapper.GetType("java.lang.VerifyError");
			ilGenerator.Emit(OpCodes.Ldstr, string.Format("(class: {0}, method: {1}, signature: {2}, offset: {3}, instruction: {4}) {5}", x.Class, x.Method, x.Signature, x.ByteCodeOffset, x.Instruction, x.Message));
			ilGenerator.Emit(OpCodes.Newobj, verifyError.GetConstructor(new Type[] { typeof(string) }));
			ilGenerator.Emit(OpCodes.Throw);
			return;
		}
		Profiler.Enter("Compile");
		c.Compile(0, 0, null);
		Profiler.Leave("Compile");
	}

	private void Compile(int initialInstructionIndex, int exceptionIndex, ArrayList exits)
	{
		int rangeBegin;
		int rangeEnd;		// NOTE points past the last instruction in the range
		if(exceptionIndex == 0)
		{
			rangeBegin = 0;
			// because the last instruction in the code array is always the additional __nop, put there
			// by our classfile reader, this works
			rangeEnd = m.Instructions[m.Instructions.Length - 1].PC;
		}
		else
		{
			rangeBegin = exceptions[exceptionIndex - 1].start_pc;
			rangeEnd = exceptions[exceptionIndex - 1].end_pc;
		}
		object[] labels = new object[m.Instructions[m.Instructions.Length - 1].PC];
		// used to track instructions that are 'live'
		bool[] inuse = new bool[m.Instructions.Length];
		// used to track instructions that have been compiled
		bool[] done = new bool[m.Instructions.Length];
		bool quit = false;
		inuse[initialInstructionIndex] = true;
		Instruction[] code = m.Instructions;
		while(!quit)
		{
			quit = true;
			for(int i = 0; i < code.Length; i++)
			{
			restart:
				if(!inuse[i] || done[i])
				{
					continue;
				}
				quit = false;
				done[i] = true;
				Instruction instr = code[i];

				// make sure we didn't branch into a try block
				// NOTE this check is not strict enough
				// UPDATE since we're now splitting try blocks around branch targets, this shouldn't be possible anymore
				if(exceptionIndex < exceptions.Length &&
					instr.PC > exceptions[exceptionIndex].start_pc &&
					instr.PC < exceptions[exceptionIndex].end_pc)
				{
					throw new NotImplementedException("branch into try block not implemented: " + clazz.Name + "." + m.Method.Name + m.Method.Signature + " (index = " + exceptionIndex + ", pc = " + instr.PC + ")");
				}

				// every instruction has an associated label, for now
				if(true)
				{
					object label = labels[instr.PC];
					if(label == null)
					{
						label = ilGenerator.DefineLabel();
						labels[instr.PC] = label;
					}
					ilGenerator.MarkLabel((Label)label);
					if(symboldocument != null)
					{
						// TODO this needs to be done better
						ClassFile.Method.LineNumberTableEntry[] table = m.LineNumberTableAttribute;
						if(table != null)
						{
							for(int j = 0; j < table.Length; j++)
							{
								if(table[j].start_pc == instr.PC && table[j].line_number != 0)
								{
									ilGenerator.MarkSequencePoint(symboldocument, table[j].line_number, 0, table[j].line_number + 1, 0);
									break;
								}
							}
						}
					}
				}

				// handle the try block here
				for(int j = exceptionIndex; j < exceptions.Length; j++)
				{
					if(exceptions[j].start_pc == instr.PC)
					{
						if(ma.GetStackHeight(i) != 0)
						{
							Stack stack = new Stack();
							int stackHeight = ma.GetStackHeight(i);
							for(int n = 0; n < stackHeight; n++)
							{
								// TODO handle class not found
								string t = ma.GetRawStackType(i, n);
								if(t.Length > 1 && t[0] == 'N')
								{
									// unitialized references aren't really there
									continue;
								}
								if(t == "Lnull")
								{
									stack.Push(null);
								}
								else
								{
									LocalBuilder local = ilGenerator.DeclareLocal(classLoader.ExpressionType(t));
									stack.Push(local);
									ilGenerator.Emit(OpCodes.Stloc, local);
								}
							}
							ilGenerator.BeginExceptionBlock();
							while(stack.Count != 0)
							{
								LocalBuilder local = (LocalBuilder)stack.Pop();
								if(local == null)
								{
									ilGenerator.Emit(OpCodes.Ldnull);
								}
								else
								{
									ilGenerator.Emit(OpCodes.Ldloc, local);
								}
							}
						}
						else
						{
							ilGenerator.BeginExceptionBlock();
						}
						ArrayList newExits = new ArrayList();
						Compile(i, j + 1, newExits);
						for(int k = 0; k < newExits.Count; k++)
						{
							object exit = newExits[k];
							BranchCookie bc = exit as BranchCookie;
							if(bc != null)
							{
								ilGenerator.MarkLabel(bc.Stub);
								int stack = ma.GetStackHeight(bc.TargetIndex);
								for(int n = 0; n < stack; n++)
								{
									// TODO handle class not found
									string t = ma.GetRawStackType(bc.TargetIndex, n);
									if((t.Length > 1 && t[0] == 'N') || t == "Lnull")
									{
										// unitialized references aren't really there, but at the push site we
										// need to know that we have to skip this slot, so we push a null as well,
										// and then at the push site we'll look at the stack type to figure out
										// if it is a real null or an uniti
										bc.Stack.Push(null);
									}
									else
									{
										LocalBuilder local = ilGenerator.DeclareLocal(classLoader.ExpressionType(t));
										bc.Stack.Push(local);
										ilGenerator.Emit(OpCodes.Stloc, local);
									}
								}
								bc.Stub = ilGenerator.DefineLabel();
								ilGenerator.Emit(OpCodes.Leave, bc.Stub);
							}
						}
						Type excType;
						if(exceptions[j].catch_type == 0)
						{
							excType = typeof(Exception);
						}
						else
						{
							// TODO handle class not found
							excType = classLoader.LoadClassBySlashedName(m.Method.ClassFile.GetConstantPoolClass(exceptions[j].catch_type)).Type;
						}
						if(true)
						{
							ilGenerator.BeginCatchBlock(typeof(Exception));
							Label label = ilGenerator.DefineLabel();
							LocalBuilder local = ilGenerator.DeclareLocal(excType);
							// special case for catch(Throwable) (and finally), that produces less code and
							// should be faster
							if(excType == typeof(Exception))
							{
								ilGenerator.Emit(OpCodes.Call, mapExceptionFastMethod);
								ilGenerator.Emit(OpCodes.Stloc, local);
								ilGenerator.Emit(OpCodes.Leave, label);
							}
							else
							{
								ilGenerator.Emit(OpCodes.Ldtoken, excType);
								ilGenerator.Emit(OpCodes.Call, getTypeFromHandleMethod);
								ilGenerator.Emit(OpCodes.Call, mapExceptionMethod);
								ilGenerator.Emit(OpCodes.Castclass, excType);
								ilGenerator.Emit(OpCodes.Stloc, local);
								ilGenerator.Emit(OpCodes.Ldloc, local);
								Label rethrow = ilGenerator.DefineLabel();
								ilGenerator.Emit(OpCodes.Brfalse, rethrow);
								ilGenerator.Emit(OpCodes.Leave, label);
								ilGenerator.MarkLabel(rethrow);
								ilGenerator.Emit(OpCodes.Rethrow);
							}
							ilGenerator.EndExceptionBlock();
							ilGenerator.MarkLabel(label);
							ilGenerator.Emit(OpCodes.Ldloc, local);
							ilGenerator.Emit(OpCodes.Br, GetLabel(labels, exceptions[j].handler_pc, inuse, rangeBegin, rangeEnd, exits));
						}
						for(int k = 0; k < newExits.Count; k++)
						{
							object exit = newExits[k];
							ReturnCookie rc = exit as ReturnCookie;
							if(rc != null)
							{
								if(exceptionIndex == 0)
								{
									ilGenerator.MarkLabel(rc.Stub);
									if(rc.Local != null)
									{
										ilGenerator.Emit(OpCodes.Ldloc, rc.Local);
									}
									ilGenerator.Emit(OpCodes.Ret);
								}
								else
								{
									ReturnCookie rc1 = new ReturnCookie();
									rc1.Local = rc.Local;
									rc1.Stub = ilGenerator.DefineLabel();
									ilGenerator.MarkLabel(rc.Stub);
									ilGenerator.Emit(OpCodes.Leave, rc1.Stub);
									exits.Add(rc1);
								}
							}
							else
							{
								BranchCookie bc = exit as BranchCookie;
								if(bc != null)
								{
									ilGenerator.MarkLabel(bc.Stub);
									int stack = ma.GetStackHeight(bc.TargetIndex);
									for(int n = 0; n < stack; n++)
									{
										LocalBuilder local = (LocalBuilder)bc.Stack.Pop();
										if(local == null)
										{
											if(ma.GetRawStackType(bc.TargetIndex, (stack - 1) - n) == "Lnull")
											{
												ilGenerator.Emit(OpCodes.Ldnull);
											}
											else
											{
												// if the type is not Lnull, it means it was an unitialized object reference,
												// which don't really exist on our stack
											}
										}
										else
										{
											ilGenerator.Emit(OpCodes.Ldloc, local);
										}
									}
									ilGenerator.Emit(OpCodes.Br, GetLabel(labels, code[bc.TargetIndex].PC, inuse, rangeBegin, rangeEnd, exits));
								}
							}
						}
						goto restart;
					}
				}

				switch(instr.NormalizedOpCode)
				{
					case NormalizedByteCode.__getstatic:
					{
						ClassFile.ConstantPoolItemFieldref cpi = m.Method.ClassFile.GetFieldref(instr.Arg1);
						FieldWrapper field = GetField(cpi, true, null, false);
						if(field != null)
						{
							field.EmitGet.Emit(ilGenerator);
						}
						else
						{
							EmitPlaceholder(cpi.Signature);
						}
						break;
					}
					case NormalizedByteCode.__putstatic:
					{
						ClassFile.ConstantPoolItemFieldref cpi = m.Method.ClassFile.GetFieldref(instr.Arg1);
						FieldWrapper field = GetField(cpi, true, null, true);
						if(field != null)
						{
							// because of the way interface merging works, an object reference is valid
							// for any interface reference
							if(field.FieldType != typeof(object) && ma.GetRawStackType(i, 0) == "Ljava/lang/Object;")
							{
								ilGenerator.Emit(OpCodes.Castclass, field.FieldType);
							}
							field.EmitSet.Emit(ilGenerator);
						}
						else
						{
							ilGenerator.Emit(OpCodes.Pop);
						}
						break;
					}
					case NormalizedByteCode.__getfield:
					{
						ClassFile.ConstantPoolItemFieldref cpi = m.Method.ClassFile.GetFieldref(instr.Arg1);
						TypeWrapper thisType = LoadClass(SigTypeToClassName(ma.GetRawStackType(i, 0), cpi.Class));
						if(thisType != null)
						{
							FieldWrapper field = GetField(cpi, false, thisType, false);
							if(field != null)
							{
								field.EmitGet.Emit(ilGenerator);
								break;
							}
						}
						ilGenerator.Emit(OpCodes.Pop);
						EmitPlaceholder(cpi.Signature);
						break;
					}
					case NormalizedByteCode.__putfield:
					{
						ClassFile.ConstantPoolItemFieldref cpi = m.Method.ClassFile.GetFieldref(instr.Arg1);
						TypeWrapper thisType = LoadClass(SigTypeToClassName(ma.GetRawStackType(i, 1), cpi.Class));
						if(thisType != null)
						{
							FieldWrapper field = GetField(cpi, false, thisType, true);
							if(field != null)
							{
								// because of the way interface merging works, an object reference is valid
								// for any interface reference
								if(field.FieldType != typeof(object) && ma.GetRawStackType(i, 0) == "Ljava/lang/Object;")
								{
									ilGenerator.Emit(OpCodes.Castclass, field.FieldType);
								}
								field.EmitSet.Emit(ilGenerator);
								break;
							}
						}
						ilGenerator.Emit(OpCodes.Pop);
						ilGenerator.Emit(OpCodes.Pop);
						break;
					}
					case NormalizedByteCode.__aconst_null:
						ilGenerator.Emit(OpCodes.Ldnull);
						break;
					case NormalizedByteCode.__iconst:
						switch(instr.NormalizedArg1)
						{
							case -1:
								ilGenerator.Emit(OpCodes.Ldc_I4_M1);
								break;
							case 0:
								ilGenerator.Emit(OpCodes.Ldc_I4_0);
								break;
							case 1:
								ilGenerator.Emit(OpCodes.Ldc_I4_1);
								break;
							case 2:
								ilGenerator.Emit(OpCodes.Ldc_I4_2);
								break;
							case 3:
								ilGenerator.Emit(OpCodes.Ldc_I4_3);
								break;
							case 4:
								ilGenerator.Emit(OpCodes.Ldc_I4_4);
								break;
							case 5:
								ilGenerator.Emit(OpCodes.Ldc_I4_5);
								break;
							case 6:
								ilGenerator.Emit(OpCodes.Ldc_I4_6);
								break;
							case 7:
								ilGenerator.Emit(OpCodes.Ldc_I4_7);
								break;
							case 8:
								ilGenerator.Emit(OpCodes.Ldc_I4_8);
								break;
							default:
								if(instr.NormalizedArg1 >= -128 && instr.NormalizedArg1 <= 127)
								{
									ilGenerator.Emit(OpCodes.Ldc_I4_S, (sbyte)instr.NormalizedArg1);
								}
								else
								{
									ilGenerator.Emit(OpCodes.Ldc_I4, instr.NormalizedArg1);
								}
								break;
						}
						break;
					case NormalizedByteCode.__lconst_0:
						ilGenerator.Emit(OpCodes.Ldc_I8, 0L);
						break;
					case NormalizedByteCode.__lconst_1:
						ilGenerator.Emit(OpCodes.Ldc_I8, 1L);
						break;
					case NormalizedByteCode.__fconst_0:
						ilGenerator.Emit(OpCodes.Ldc_R4, 0.0f);
						break;
					case NormalizedByteCode.__fconst_1:
						ilGenerator.Emit(OpCodes.Ldc_R4, 1.0f);
						break;
					case NormalizedByteCode.__fconst_2:
						ilGenerator.Emit(OpCodes.Ldc_R4, 2.0f);
						break;
					case NormalizedByteCode.__dconst_0:
						ilGenerator.Emit(OpCodes.Ldc_R8, 0.0d);
						break;
					case NormalizedByteCode.__dconst_1:
						ilGenerator.Emit(OpCodes.Ldc_R8, 1.0d);
						break;
					case NormalizedByteCode.__ldc:
					{
						object o = instr.MethodCode.Method.ClassFile.GetConstantPoolConstant(instr.Arg1);
						if(o is string)
						{
							ilGenerator.Emit(OpCodes.Ldstr, (string)o);
						}
						else if(o is float)
						{
							ilGenerator.Emit(OpCodes.Ldc_R4, (float)o);
						}
						else if(o is double)
						{
							ilGenerator.Emit(OpCodes.Ldc_R8, (double)o);
						}
						else if(o is int)
						{
							ilGenerator.Emit(OpCodes.Ldc_I4, (int)o);
						}
						else if(o is long)
						{
							ilGenerator.Emit(OpCodes.Ldc_I8, (long)o);
						}
						else
						{
							throw new NotImplementedException(o.GetType().Name);
						}
						break;
					}
					case NormalizedByteCode.__invokestatic:
					{
						ClassFile.ConstantPoolItemFMI cpi = m.Method.ClassFile.GetMethodref(instr.Arg1);
						MethodWrapper method = GetMethod(cpi, null, NormalizedByteCode.__invokestatic);
						if(method != null)
						{
							method.EmitCall.Emit(ilGenerator);
						}
						else
						{
							SigEnumerator sig = new SigEnumerator(cpi.Signature);
							while(sig.MoveNext())
							{
								ilGenerator.Emit(OpCodes.Pop);
							}
							EmitPlaceholder(cpi.Signature.Substring(cpi.Signature.LastIndexOf(')') + 1));
						}
						break;
					}
					case NormalizedByteCode.__invokevirtual:
					case NormalizedByteCode.__invokeinterface:
					case NormalizedByteCode.__invokespecial:
					{
						// TODO invokespecial should check for null "this" reference
						ClassFile.ConstantPoolItemFMI cpi = m.Method.ClassFile.GetMethodref(instr.Arg1);
						SigEnumerator sig = new SigEnumerator(cpi.Signature);
						int argcount = 0;
						while(sig.MoveNext())
						{
							argcount++;
						}
						string type = ma.GetRawStackType(i, argcount);
						TypeWrapper thisType = LoadClass(SigTypeToClassName(type, cpi.Class));
						// invokeinterface needs to have special support for downcasting to the interface (because
						// the verifier may not be able to merge two interfaces, but the resulting code would still be valid)
						if(instr.NormalizedOpCode == NormalizedByteCode.__invokeinterface &&
							thisType == ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassBySlashedName("java/lang/Object"))
						{
							thisType = LoadClass(cpi.Class);
							if(thisType != null)
							{
								DupHelper dup = new DupHelper(classLoader, ilGenerator, argcount);
								for(int k = 0; k < argcount; k++)
								{
									dup.SetType(k, ma.GetRawStackType(i, k));
								}
								for(int k = 0; k < argcount; k++)
								{
									dup.Store(k);
								}
								// TODO this IncompatibleClassChangeError check should also be applied
								// for other locations where we can "consume" an object reference in the
								// place of an interface reference (putstatic / putfield / arguments for invoke*).
								// TODO it turns out that when an interface ref is expected, *any* type will be accepted!
								ilGenerator.Emit(OpCodes.Dup);
								Label label = ilGenerator.DefineLabel();
								ilGenerator.Emit(OpCodes.Brfalse_S, label);
								ilGenerator.Emit(OpCodes.Isinst, thisType.Type);
								ilGenerator.Emit(OpCodes.Dup);
								ilGenerator.Emit(OpCodes.Brtrue_S, label);
								EmitError("java.lang.IncompatibleClassChangeError", null);
								ilGenerator.MarkLabel(label);
								for(int k = argcount - 1; k >= 0; k--)
								{
									dup.Load(k);
								}
							}
						}
						else if(thisType != null && !thisType.IsSubTypeOf(LoadClass(cpi.Class)))
						{
							EmitError("java.lang.IncompatibleClassChangeError", null);
							thisType = null;
						}
						MethodWrapper method = (thisType != null) ? GetMethod(cpi, thisType, instr.NormalizedOpCode) : null;
						if(instr.NormalizedOpCode == NormalizedByteCode.__invokespecial)
						{
							if(cpi.Name == "<init>")
							{
								if(type[0] == 'N')
								{
									if(thisType != null && (thisType.IsAbstract || thisType.IsInterface))
									{
										// the CLR gets confused when we do a newobj on an abstract class,
										// so we set method to null, to basically just comment out the constructor
										// call (the InstantionError was already emitted at the "new" bytecode)
										method = null;
									}
									// we have to construct a list of all the unitialized references to the object
									// we're about to create on the stack, so that we can reconstruct the stack after
									// the "newobj" instruction
									int trivcount = 0;
									bool nontrivial = false;
									bool[] stackfix = new bool[ma.GetStackHeight(i) - (argcount + 1)];
									bool[] localsfix = new bool[m.MaxLocals];
									for(int j = 0; j < stackfix.Length; j++)
									{
										if(ma.GetRawStackType(i, argcount + 1 + j) == type)
										{
											stackfix[j] = true;
											if(trivcount == j)
											{
												trivcount++;
											}
											else
											{
												// if there is other stuff on the stack between the new object
												// references, we need to do more work to construct the proper stack
												// layout after the newobj instruction
												nontrivial = true;
											}
										}
									}
									for(int j = 0; j < localsfix.Length; j++)
									{
										if(ma.GetLocalType(i, j) == type)
										{
											localsfix[j] = true;
											nontrivial = true;
										}
									}
									if(method != null)
									{
										method.EmitNewobj.Emit(ilGenerator);
									}
									else
									{
										for(int j = 0; j < argcount; j++)
										{
											ilGenerator.Emit(OpCodes.Pop);
										}
										ilGenerator.Emit(OpCodes.Ldnull);
									}
									// TODO it is probably a better idea to do this in the constructor for each class
									// derived from java.lang.Throwable, but if we do move this to the constructor, we
									// should still call it here for non-Java exceptions (that aren't derived from Throwable)
									Type t = ExpressionType(type.Substring(type.IndexOf(';') + 1));
									if(t == null)
									{
										// If the type couldn't be loaded, we continue we object to make sure
										// the code remains verifiable (the ExpressionType call above already generated
										// code to throw an exception, but the remaing code still needs to be verifiable,
										// even though it is unreachable).
										t = typeof(object);
									}
									if(typeof(Exception).IsAssignableFrom(t))
									{
										ilGenerator.Emit(OpCodes.Dup);
										ilGenerator.Emit(OpCodes.Call, fillInStackTraceMethod);
										ilGenerator.Emit(OpCodes.Pop);
									}
									if(nontrivial)
									{
										// this could be done a little more efficiently, but since in practice this
										// code never runs (for code compiled from Java source) it doesn't
										// really matter
										LocalBuilder newobj = ilGenerator.DeclareLocal(t);
										ilGenerator.Emit(OpCodes.Stloc, newobj);
										LocalBuilder[] tempstack = new LocalBuilder[stackfix.Length];
										for(int j = 0; j < stackfix.Length; j++)
										{
											if(!stackfix[j])
											{
												string stacktype = ma.GetRawStackType(i, argcount + 1 + j);
												// it could be another new object reference (not from current invokespecial <init>
												// instruction)
												if(stacktype[0] != 'N')
												{
													// TODO handle Lnull stack entries
													// TODO handle class not found
													LocalBuilder lb = ilGenerator.DeclareLocal(classLoader.ExpressionType(stacktype));
													ilGenerator.Emit(OpCodes.Stloc, lb);
													tempstack[j] = lb;
												}
											}
										}
										for(int j = stackfix.Length - 1; j >= 0; j--)
										{
											if(stackfix[j])
											{
												ilGenerator.Emit(OpCodes.Ldloc, newobj);
											}
											else if(tempstack[j] != null)
											{
												ilGenerator.Emit(OpCodes.Ldloc, tempstack[j]);
											}
										}
										for(int j = 0; j < localsfix.Length; j++)
										{
											if(localsfix[j])
											{
												ilGenerator.Emit(OpCodes.Ldloc, newobj);
												ilGenerator.Emit(OpCodes.Stloc, GetLocal(typeof(object), j));
											}
										}
									}
									else
									{
										if(trivcount == 0)
										{
											ilGenerator.Emit(OpCodes.Pop);
										}
										else
										{
											for(int j = 1; j < trivcount; j++)
											{
												ilGenerator.Emit(OpCodes.Dup);
											}
										}
									}
								}
								else
								{
									if(method != null)
									{
										method.EmitCall.Emit(ilGenerator);
									}
									else
									{
										// if we're a constructor and the call to the base class constructor
										// wasn't accessible, we need make sure that there is no code path that
										// returns from the constructor, otherwise the method will be not verifiable
										// TODO this isn't anywhere near a proper solution, but for the time being it works
										// some things to consider:
										// - only pull this full when calls to the base class constructor fail
										// - when control flow is complex, this trivial solution will not work
										ilGenerator.Emit(OpCodes.Ldnull);
										ilGenerator.Emit(OpCodes.Throw);
										return;
//										for(int j = 0; j < argcount + 1; j++)
//										{
//											ilGenerator.Emit(OpCodes.Pop);
//										}
//										EmitPlaceholder(cpi.Signature.Substring(cpi.Signature.LastIndexOf(')') + 1));
									}
								}
							}
							else
							{
								if(method != null)
								{
									method.EmitCall.Emit(ilGenerator);
								}
								else
								{
									for(int j = 0; j < argcount + 1; j++)
									{
										ilGenerator.Emit(OpCodes.Pop);
									}
									EmitPlaceholder(cpi.Signature.Substring(cpi.Signature.LastIndexOf(')') + 1));
								}
							}
						}
						else
						{
							if(method != null)
							{
								method.EmitCallvirt.Emit(ilGenerator);
							}
							else
							{
								for(int j = 0; j < argcount + 1; j++)
								{
									ilGenerator.Emit(OpCodes.Pop);
								}
								EmitPlaceholder(cpi.Signature.Substring(cpi.Signature.LastIndexOf(')') + 1));
							}
						}
						break;
					}
					case NormalizedByteCode.__return:
					case NormalizedByteCode.__areturn:
					case NormalizedByteCode.__ireturn:
					case NormalizedByteCode.__lreturn:
					case NormalizedByteCode.__freturn:
					case NormalizedByteCode.__dreturn:
					{
						if(exceptionIndex != 0)
						{
							// if we're inside an exception block, copy TOS to local, emit "leave" and push item onto our "todo" list
							ReturnCookie rc = new ReturnCookie();
							if(instr.NormalizedOpCode != NormalizedByteCode.__return)
							{
								// TODO handle class not found
								Type retType = classLoader.RetTypeFromSig(m.Method.Signature);
								rc.Local = ilGenerator.DeclareLocal(retType);
								// because of the way interface merging works, an object reference is valid
								// for any interface reference
								if(retType != typeof(object) && ma.GetRawStackType(i, 0) == "Ljava/lang/Object;")
								{
									ilGenerator.Emit(OpCodes.Castclass, retType);
								}
								ilGenerator.Emit(OpCodes.Stloc, rc.Local);
							}
							rc.Stub = ilGenerator.DefineLabel();
							// NOTE leave automatically discards any junk that may be on the stack
							ilGenerator.Emit(OpCodes.Leave, rc.Stub);
							exits.Add(rc);
						}
						else
						{
							// if there is junk on the stack (other than the return value), we must pop it off
							// because in .NET this is invalid (unlike in Java)
							int stackHeight = ma.GetStackHeight(i);
							if(instr.NormalizedOpCode == NormalizedByteCode.__return)
							{
								for(int j = 0; j < stackHeight; j++)
								{
									ilGenerator.Emit(OpCodes.Pop);
								}
								ilGenerator.Emit(OpCodes.Ret);
							}
							else
							{
								// TODO handle class not found
								Type retType = classLoader.RetTypeFromSig(m.Method.Signature);
								// because of the way interface merging works, an object reference is valid
								// for any interface reference
								if(retType != typeof(object) && ma.GetRawStackType(i, 0) == "Ljava/lang/Object;")
								{
									ilGenerator.Emit(OpCodes.Castclass, retType);
								}
								if(stackHeight != 1)
								{
									LocalBuilder local = ilGenerator.DeclareLocal(retType);
									ilGenerator.Emit(OpCodes.Stloc, local);
									for(int j = 1; j < stackHeight; j++)
									{
										ilGenerator.Emit(OpCodes.Pop);
									}
									ilGenerator.Emit(OpCodes.Ldloc, local);
								}
								ilGenerator.Emit(OpCodes.Ret);
							}
						}
						break;
					}
					case NormalizedByteCode.__aload:
					{
						string type = ma.GetLocalType(i, instr.NormalizedArg1);
						if(type == "Lnull")
						{
							// if the local is known to be null, we just emit a null
							ilGenerator.Emit(OpCodes.Ldnull);
						}
						else if(type[0] == 'N')
						{
							// since new objects aren't represented on the stack, we don't need to do anything here
						}
						else if(type[0] == 'U')
						{
							// any unitialized reference has to be the this reference
							// TODO when we get support for overwriting the this reference, this code
							// needs to be aware of that (or, this overwriting should be handled specially for <init>)
							ilGenerator.Emit(OpCodes.Ldarg_0);
						}
						else
						{
							Load(instr, typeof(object));
							if(instr.NormalizedArg1 >= m.ArgMap.Length)
							{
								// HACK since, for now, all locals are of type object, we've got to cast them to the proper type
								// UPDATE the above is no longer true, we now have at least some idea of the type of the local
								if(type != ma.GetDeclaredLocalType(instr.NormalizedArg1))
								{
									// TODO handle class not found
									ilGenerator.Emit(OpCodes.Castclass, classLoader.ExpressionType(ma.GetLocalType(i, instr.NormalizedArg1)));
								}
							}
						}
						break;
					}
					case NormalizedByteCode.__astore:
					{
						string type = ma.GetRawStackType(i, 0);
						// HACK we use "int" to track the return address of a jsr
						if(type.StartsWith("Lret;"))
						{
							Store(instr, typeof(int));
						}
						else if(type[0] == 'N')
						{
							// NOTE new objects aren't really on the stack, so we can't copy them into the local.
							// We do store a null in the local, to prevent it from retaining an unintentional reference
							// to whatever object reference happens to be there
							ilGenerator.Emit(OpCodes.Ldnull);
							Store(instr, typeof(object));
						}
						else if(type[0] == 'U')
						{
							// any unitialized reference, is always the this reference, we don't store anything
							// here (because CLR wont allow unitialized references in locals) and then when
							// the unitialized ref is loaded we redirect to the this reference
							ilGenerator.Emit(OpCodes.Pop);
						}
						else
						{
							Store(instr, typeof(object));
						}
						break;
					}
					case NormalizedByteCode.__iload:
						Load(instr, typeof(int));
						break;
					case NormalizedByteCode.__istore:
						Store(instr, typeof(int));
						break;
					case NormalizedByteCode.__lload:
						Load(instr, typeof(long));
						break;
					case NormalizedByteCode.__lstore:
						Store(instr, typeof(long));
						break;
					case NormalizedByteCode.__fload:
						Load(instr, typeof(float));
						break;
					case NormalizedByteCode.__fstore:
						Store(instr, typeof(float));
						break;
					case NormalizedByteCode.__dload:
						Load(instr, typeof(double));
						break;
					case NormalizedByteCode.__dstore:
						Store(instr, typeof(double));
						break;
					case NormalizedByteCode.__new:
					{
						TypeWrapper wrapper = LoadClass(instr.MethodCode.Method.ClassFile.GetConstantPoolClass(instr.Arg1));
						if(wrapper != null && (wrapper.IsAbstract || wrapper.IsInterface))
						{
							EmitError("java.lang.InstantiationError", wrapper.Name);
						}
						// we don't do anything here, the call to <init> will be converted into a newobj instruction
						break;
					}
					case NormalizedByteCode.__multianewarray:
					{
						TypeWrapper wrapper = LoadClass(instr.MethodCode.Method.ClassFile.GetConstantPoolClass(instr.Arg1));
						if(wrapper != null)
						{
							LocalBuilder localArray = ilGenerator.DeclareLocal(typeof(int[]));
							LocalBuilder localInt = ilGenerator.DeclareLocal(typeof(int));
							ilGenerator.Emit(OpCodes.Ldc_I4, instr.Arg2);
							ilGenerator.Emit(OpCodes.Newarr, typeof(int));
							ilGenerator.Emit(OpCodes.Stloc, localArray);
							for(int j = 1; j <= instr.Arg2; j++)
							{
								ilGenerator.Emit(OpCodes.Stloc, localInt);
								ilGenerator.Emit(OpCodes.Ldloc, localArray);
								ilGenerator.Emit(OpCodes.Ldc_I4, instr.Arg2 - j);
								ilGenerator.Emit(OpCodes.Ldloc, localInt);
								ilGenerator.Emit(OpCodes.Stelem_I4);
							}
							Type type = wrapper.Type;
							ilGenerator.Emit(OpCodes.Ldtoken, type);
							ilGenerator.Emit(OpCodes.Ldloc, localArray);
							ilGenerator.Emit(OpCodes.Call, multiANewArrayMethod);
							ilGenerator.Emit(OpCodes.Castclass, type);
						}
						break;
					}
					case NormalizedByteCode.__anewarray:
					{
						TypeWrapper wrapper = LoadClass(instr.MethodCode.Method.ClassFile.GetConstantPoolClass(instr.Arg1));
						if(wrapper != null)
						{
							ilGenerator.Emit(OpCodes.Newarr, wrapper.Type);
						}
						break;
					}
					case NormalizedByteCode.__newarray:
					switch(instr.Arg1)
					{
						case 4:
							ilGenerator.Emit(OpCodes.Newarr, typeof(bool));
							break;
						case 5:
							ilGenerator.Emit(OpCodes.Newarr, typeof(char));
							break;
						case 6:
							ilGenerator.Emit(OpCodes.Newarr, typeof(float));
							break;
						case 7:
							ilGenerator.Emit(OpCodes.Newarr, typeof(double));
							break;
						case 8:
							ilGenerator.Emit(OpCodes.Newarr, typeof(sbyte));
							break;
						case 9:
							ilGenerator.Emit(OpCodes.Newarr, typeof(short));
							break;
						case 10:
							ilGenerator.Emit(OpCodes.Newarr, typeof(int));
							break;
						case 11:
							ilGenerator.Emit(OpCodes.Newarr, typeof(long));
							break;
						default:
							throw new InvalidOperationException();
					}
						break;
					case NormalizedByteCode.__checkcast:
					{
						TypeWrapper wrapper = LoadClass(instr.MethodCode.Method.ClassFile.GetConstantPoolClass(instr.Arg1));
						if(wrapper != null)
						{
							ilGenerator.Emit(OpCodes.Castclass, wrapper.Type);
						}
						break;
					}
					case NormalizedByteCode.__instanceof:
					{
						TypeWrapper wrapper = LoadClass(instr.MethodCode.Method.ClassFile.GetConstantPoolClass(instr.Arg1));
						if(wrapper != null)
						{
							ilGenerator.Emit(OpCodes.Isinst, wrapper.Type);
							ilGenerator.Emit(OpCodes.Ldnull);
							ilGenerator.Emit(OpCodes.Ceq);
							ilGenerator.Emit(OpCodes.Ldc_I4_0);
							ilGenerator.Emit(OpCodes.Ceq);
						}
						break;
					}
					case NormalizedByteCode.__aaload:
						ilGenerator.Emit(OpCodes.Ldelem_Ref);
						break;
					case NormalizedByteCode.__baload:
						// NOTE both the JVM and the CLR use signed bytes for boolean arrays (how convenient!)
						ilGenerator.Emit(OpCodes.Ldelem_I1);
						break;
					case NormalizedByteCode.__bastore:
						ilGenerator.Emit(OpCodes.Stelem_I1);
						break;
					case NormalizedByteCode.__caload:
						ilGenerator.Emit(OpCodes.Ldelem_U2);
						break;
					case NormalizedByteCode.__castore:
						ilGenerator.Emit(OpCodes.Stelem_I2);
						break;
					case NormalizedByteCode.__saload:
						ilGenerator.Emit(OpCodes.Ldelem_I2);
						break;
					case NormalizedByteCode.__sastore:
						ilGenerator.Emit(OpCodes.Stelem_I2);
						break;
					case NormalizedByteCode.__iaload:
						ilGenerator.Emit(OpCodes.Ldelem_I4);
						break;
					case NormalizedByteCode.__iastore:
						ilGenerator.Emit(OpCodes.Stelem_I4);
						break;
					case NormalizedByteCode.__laload:
						ilGenerator.Emit(OpCodes.Ldelem_I8);
						break;
					case NormalizedByteCode.__lastore:
						ilGenerator.Emit(OpCodes.Stelem_I8);
						break;
					case NormalizedByteCode.__faload:
						ilGenerator.Emit(OpCodes.Ldelem_R4);
						break;
					case NormalizedByteCode.__fastore:
						ilGenerator.Emit(OpCodes.Stelem_R4);
						break;
					case NormalizedByteCode.__daload:
						ilGenerator.Emit(OpCodes.Ldelem_R8);
						break;
					case NormalizedByteCode.__dastore:
						ilGenerator.Emit(OpCodes.Stelem_R8);
						break;
					case NormalizedByteCode.__aastore:
						ilGenerator.Emit(OpCodes.Stelem_Ref);
						break;
					case NormalizedByteCode.__arraylength:
						ilGenerator.Emit(OpCodes.Ldlen);
						break;
					case NormalizedByteCode.__lcmp:
					{
						LocalBuilder value1 = ilGenerator.DeclareLocal(typeof(long));
						LocalBuilder value2 = ilGenerator.DeclareLocal(typeof(long));
						ilGenerator.Emit(OpCodes.Stloc, value2);
						ilGenerator.Emit(OpCodes.Stloc, value1);
						ilGenerator.Emit(OpCodes.Ldloc, value1);
						ilGenerator.Emit(OpCodes.Ldloc, value2);
						Label res1 = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Bgt_S, res1);
						ilGenerator.Emit(OpCodes.Ldloc, value1);
						ilGenerator.Emit(OpCodes.Ldloc, value2);
						Label res0 = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Beq_S, res0);
						ilGenerator.Emit(OpCodes.Ldc_I4_M1);
						Label end = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Br_S, end);
						ilGenerator.MarkLabel(res1);
						ilGenerator.Emit(OpCodes.Ldc_I4_1);
						ilGenerator.Emit(OpCodes.Br_S, end);
						ilGenerator.MarkLabel(res0);
						ilGenerator.Emit(OpCodes.Ldc_I4_0);
						ilGenerator.MarkLabel(end);
						break;
					}
					case NormalizedByteCode.__fcmpl:
					{
						LocalBuilder value1 = ilGenerator.DeclareLocal(typeof(float));
						LocalBuilder value2 = ilGenerator.DeclareLocal(typeof(float));
						ilGenerator.Emit(OpCodes.Stloc, value2);
						ilGenerator.Emit(OpCodes.Stloc, value1);
						ilGenerator.Emit(OpCodes.Ldloc, value1);
						ilGenerator.Emit(OpCodes.Ldloc, value2);
						Label res1 = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Bgt_S, res1);
						ilGenerator.Emit(OpCodes.Ldloc, value1);
						ilGenerator.Emit(OpCodes.Ldloc, value2);
						Label res0 = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Beq_S, res0);
						ilGenerator.Emit(OpCodes.Ldc_I4_M1);
						Label end = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Br_S, end);
						ilGenerator.MarkLabel(res1);
						ilGenerator.Emit(OpCodes.Ldc_I4_1);
						ilGenerator.Emit(OpCodes.Br_S, end);
						ilGenerator.MarkLabel(res0);
						ilGenerator.Emit(OpCodes.Ldc_I4_0);
						ilGenerator.MarkLabel(end);
						break;
					}
					case NormalizedByteCode.__fcmpg:
					{
						LocalBuilder value1 = ilGenerator.DeclareLocal(typeof(float));
						LocalBuilder value2 = ilGenerator.DeclareLocal(typeof(float));
						ilGenerator.Emit(OpCodes.Stloc, value2);
						ilGenerator.Emit(OpCodes.Stloc, value1);
						ilGenerator.Emit(OpCodes.Ldloc, value1);
						ilGenerator.Emit(OpCodes.Ldloc, value2);
						Label resm1 = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Blt_S, resm1);
						ilGenerator.Emit(OpCodes.Ldloc, value1);
						ilGenerator.Emit(OpCodes.Ldloc, value2);
						Label res0 = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Beq_S, res0);
						ilGenerator.Emit(OpCodes.Ldc_I4_1);
						Label end = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Br_S, end);
						ilGenerator.MarkLabel(resm1);
						ilGenerator.Emit(OpCodes.Ldc_I4_M1);
						ilGenerator.Emit(OpCodes.Br_S, end);
						ilGenerator.MarkLabel(res0);
						ilGenerator.Emit(OpCodes.Ldc_I4_0);
						ilGenerator.MarkLabel(end);
						break;
					}
					case NormalizedByteCode.__dcmpl:
					{
						LocalBuilder value1 = ilGenerator.DeclareLocal(typeof(double));
						LocalBuilder value2 = ilGenerator.DeclareLocal(typeof(double));
						ilGenerator.Emit(OpCodes.Stloc, value2);
						ilGenerator.Emit(OpCodes.Stloc, value1);
						ilGenerator.Emit(OpCodes.Ldloc, value1);
						ilGenerator.Emit(OpCodes.Ldloc, value2);
						Label res1 = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Bgt_S, res1);
						ilGenerator.Emit(OpCodes.Ldloc, value1);
						ilGenerator.Emit(OpCodes.Ldloc, value2);
						Label res0 = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Beq_S, res0);
						ilGenerator.Emit(OpCodes.Ldc_I4_M1);
						Label end = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Br_S, end);
						ilGenerator.MarkLabel(res1);
						ilGenerator.Emit(OpCodes.Ldc_I4_1);
						ilGenerator.Emit(OpCodes.Br_S, end);
						ilGenerator.MarkLabel(res0);
						ilGenerator.Emit(OpCodes.Ldc_I4_0);
						ilGenerator.MarkLabel(end);
						break;
					}
					case NormalizedByteCode.__dcmpg:
					{
						LocalBuilder value1 = ilGenerator.DeclareLocal(typeof(double));
						LocalBuilder value2 = ilGenerator.DeclareLocal(typeof(double));
						ilGenerator.Emit(OpCodes.Stloc, value2);
						ilGenerator.Emit(OpCodes.Stloc, value1);
						ilGenerator.Emit(OpCodes.Ldloc, value1);
						ilGenerator.Emit(OpCodes.Ldloc, value2);
						Label resm1 = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Blt, resm1);
						ilGenerator.Emit(OpCodes.Ldloc, value1);
						ilGenerator.Emit(OpCodes.Ldloc, value2);
						Label res0 = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Beq, res0);
						ilGenerator.Emit(OpCodes.Ldc_I4_1);
						Label end = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Br, end);
						ilGenerator.MarkLabel(resm1);
						ilGenerator.Emit(OpCodes.Ldc_I4, -1);
						ilGenerator.Emit(OpCodes.Br, end);
						ilGenerator.MarkLabel(res0);
						ilGenerator.Emit(OpCodes.Ldc_I4_0);
						ilGenerator.MarkLabel(end);
						break;
					}
					case NormalizedByteCode.__if_icmpeq:
						ilGenerator.Emit(OpCodes.Beq, GetLabel(labels, instr.PC + instr.Arg1, inuse, rangeBegin, rangeEnd, exits));
						break;
					case NormalizedByteCode.__if_icmpne:
						ilGenerator.Emit(OpCodes.Bne_Un, GetLabel(labels, instr.PC + instr.Arg1, inuse, rangeBegin, rangeEnd, exits));
						break;
					case NormalizedByteCode.__if_icmple:
						ilGenerator.Emit(OpCodes.Ble, GetLabel(labels, instr.PC + instr.Arg1, inuse, rangeBegin, rangeEnd, exits));
						break;
					case NormalizedByteCode.__if_icmplt:
						ilGenerator.Emit(OpCodes.Blt, GetLabel(labels, instr.PC + instr.Arg1, inuse, rangeBegin, rangeEnd, exits));
						break;
					case NormalizedByteCode.__if_icmpge:
						ilGenerator.Emit(OpCodes.Bge, GetLabel(labels, instr.PC + instr.Arg1, inuse, rangeBegin, rangeEnd, exits));
						break;
					case NormalizedByteCode.__if_icmpgt:
						ilGenerator.Emit(OpCodes.Bgt, GetLabel(labels, instr.PC + instr.Arg1, inuse, rangeBegin, rangeEnd, exits));
						break;
					case NormalizedByteCode.__ifle:
						ilGenerator.Emit(OpCodes.Ldc_I4_0);
						ilGenerator.Emit(OpCodes.Ble, GetLabel(labels, instr.PC + instr.Arg1, inuse, rangeBegin, rangeEnd, exits));
						break;
					case NormalizedByteCode.__iflt:
						ilGenerator.Emit(OpCodes.Ldc_I4_0);
						ilGenerator.Emit(OpCodes.Blt, GetLabel(labels, instr.PC + instr.Arg1, inuse, rangeBegin, rangeEnd, exits));
						break;
					case NormalizedByteCode.__ifge:
						ilGenerator.Emit(OpCodes.Ldc_I4_0);
						ilGenerator.Emit(OpCodes.Bge, GetLabel(labels, instr.PC + instr.Arg1, inuse, rangeBegin, rangeEnd, exits));
						break;
					case NormalizedByteCode.__ifgt:
						ilGenerator.Emit(OpCodes.Ldc_I4_0);
						ilGenerator.Emit(OpCodes.Bgt, GetLabel(labels, instr.PC + instr.Arg1, inuse, rangeBegin, rangeEnd, exits));
						break;
					case NormalizedByteCode.__ifne:
						ilGenerator.Emit(OpCodes.Ldc_I4_0);
						ilGenerator.Emit(OpCodes.Bne_Un, GetLabel(labels, instr.PC + instr.Arg1, inuse, rangeBegin, rangeEnd, exits));
						break;
					case NormalizedByteCode.__ifeq:
						ilGenerator.Emit(OpCodes.Ldc_I4_0);
						ilGenerator.Emit(OpCodes.Beq, GetLabel(labels, instr.PC + instr.Arg1, inuse, rangeBegin, rangeEnd, exits));
						break;
					case NormalizedByteCode.__ifnonnull:
						ilGenerator.Emit(OpCodes.Brtrue, GetLabel(labels, instr.PC + instr.Arg1, inuse, rangeBegin, rangeEnd, exits));
						break;
					case NormalizedByteCode.__ifnull:
						ilGenerator.Emit(OpCodes.Brfalse, GetLabel(labels, instr.PC + instr.Arg1, inuse, rangeBegin, rangeEnd, exits));
						break;
					case NormalizedByteCode.__if_acmpeq:
						ilGenerator.Emit(OpCodes.Beq, GetLabel(labels, instr.PC + instr.Arg1, inuse, rangeBegin, rangeEnd, exits));
						break;
					case NormalizedByteCode.__if_acmpne:
						ilGenerator.Emit(OpCodes.Bne_Un, GetLabel(labels, instr.PC + instr.Arg1, inuse, rangeBegin, rangeEnd, exits));
						break;
					case NormalizedByteCode.__goto:
						ilGenerator.Emit(OpCodes.Br, GetLabel(labels, instr.PC + instr.Arg1, inuse, rangeBegin, rangeEnd, exits));
						break;
					case NormalizedByteCode.__ineg:
					case NormalizedByteCode.__lneg:
					case NormalizedByteCode.__fneg:
					case NormalizedByteCode.__dneg:
						ilGenerator.Emit(OpCodes.Neg);
						break;
					case NormalizedByteCode.__iadd:
					case NormalizedByteCode.__ladd:
					case NormalizedByteCode.__fadd:
					case NormalizedByteCode.__dadd:
						ilGenerator.Emit(OpCodes.Add);
						break;
					case NormalizedByteCode.__isub:
					case NormalizedByteCode.__lsub:
					case NormalizedByteCode.__fsub:
					case NormalizedByteCode.__dsub:
						ilGenerator.Emit(OpCodes.Sub);
						break;
					case NormalizedByteCode.__ixor:
					case NormalizedByteCode.__lxor:
						ilGenerator.Emit(OpCodes.Xor);
						break;
					case NormalizedByteCode.__ior:
					case NormalizedByteCode.__lor:
						ilGenerator.Emit(OpCodes.Or);
						break;
					case NormalizedByteCode.__iand:
					case NormalizedByteCode.__land:
						ilGenerator.Emit(OpCodes.And);
						break;
					case NormalizedByteCode.__imul:
					case NormalizedByteCode.__lmul:
					case NormalizedByteCode.__fmul:
					case NormalizedByteCode.__dmul:
						ilGenerator.Emit(OpCodes.Mul);
						break;
					case NormalizedByteCode.__idiv:
					case NormalizedByteCode.__ldiv:
					{
						// we need to special case dividing by -1, because the CLR div instruction
						// throws an OverflowException when dividing Int32.MinValue by -1, and
						// Java just silently overflows
						ilGenerator.Emit(OpCodes.Dup);
						ilGenerator.Emit(OpCodes.Ldc_I4_M1);
						if(instr.NormalizedOpCode == NormalizedByteCode.__ldiv)
						{
							ilGenerator.Emit(OpCodes.Conv_I8);
						}
						Label label = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Bne_Un_S, label);
						ilGenerator.Emit(OpCodes.Pop);
						ilGenerator.Emit(OpCodes.Neg);
						Label label2 = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Br_S, label2);
						ilGenerator.MarkLabel(label);
						ilGenerator.Emit(OpCodes.Div);
						ilGenerator.MarkLabel(label2);
						break;
					}
					case NormalizedByteCode.__fdiv:
					case NormalizedByteCode.__ddiv:
						ilGenerator.Emit(OpCodes.Div);
						break;
					case NormalizedByteCode.__irem:
					case NormalizedByteCode.__lrem:
					{
						// we need to special case taking the remainder of dividing by -1,
						// because the CLR rem instruction throws an OverflowException when
						// taking the remainder of dividing Int32.MinValue by -1, and
						// Java just silently overflows
						ilGenerator.Emit(OpCodes.Dup);
						ilGenerator.Emit(OpCodes.Ldc_I4_M1);
						if(instr.NormalizedOpCode == NormalizedByteCode.__lrem)
						{
							ilGenerator.Emit(OpCodes.Conv_I8);
						}
						Label label = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Bne_Un_S, label);
						ilGenerator.Emit(OpCodes.Pop);
						ilGenerator.Emit(OpCodes.Pop);
						ilGenerator.Emit(OpCodes.Ldc_I4_0);
						if(instr.NormalizedOpCode == NormalizedByteCode.__lrem)
						{
							ilGenerator.Emit(OpCodes.Conv_I8);
						}
						Label label2 = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Br_S, label2);
						ilGenerator.MarkLabel(label);
						ilGenerator.Emit(OpCodes.Rem);
						ilGenerator.MarkLabel(label2);
						break;
					}
					case NormalizedByteCode.__frem:
					case NormalizedByteCode.__drem:
						ilGenerator.Emit(OpCodes.Rem);
						break;
					case NormalizedByteCode.__ishl:
					case NormalizedByteCode.__lshl:
						ilGenerator.Emit(OpCodes.Shl);
						break;
					case NormalizedByteCode.__iushr:
					case NormalizedByteCode.__lushr:
						ilGenerator.Emit(OpCodes.Shr_Un);
						break;
					case NormalizedByteCode.__ishr:
					case NormalizedByteCode.__lshr:
						ilGenerator.Emit(OpCodes.Shr);
						break;
					case NormalizedByteCode.__swap:
						new DupHelper(classLoader, ilGenerator, 2)
							.SetType(0, ma.GetRawStackType(i, 0))
							.SetType(1, ma.GetRawStackType(i, 1))
							.Store(0)
							.Store(1)
							.Load(0)
							.Load(1);
						break;
					case NormalizedByteCode.__dup:
						// if the TOS contains a "new" object, it isn't really there, so we wont dup it either
						if(ma.GetRawStackType(i, 0)[0] != 'N')
						{
							ilGenerator.Emit(OpCodes.Dup);
						}
						break;
					case NormalizedByteCode.__dup2:
					{
						string type1 = ma.GetRawStackType(i, 0);
						if(type1 == "D" || type1 == "J")
						{
							ilGenerator.Emit(OpCodes.Dup);
						}
						else
						{
							new DupHelper(classLoader, ilGenerator, 2)
								.SetType(0, type1)
								.SetType(1, ma.GetRawStackType(i, 1))
								.Store(0)
								.Store(1)
								.Load(1)
								.Load(0)
								.Load(1)
								.Load(0);
						}
						break;
					}
					case NormalizedByteCode.__dup_x1:
						new DupHelper(classLoader, ilGenerator, 2)
							.SetType(0, ma.GetRawStackType(i, 0))
							.SetType(1, ma.GetRawStackType(i, 1))
							.Store(0)
							.Store(1)
							.Load(0)
							.Load(1)
							.Load(0);
						break;
					case NormalizedByteCode.__dup2_x1:
					{
						string type1 = ma.GetRawStackType(i, 0);
						if(type1 == "D" || type1 == "J")
						{
							new DupHelper(classLoader, ilGenerator, 2)
								.SetType(0, type1)
								.SetType(1, ma.GetRawStackType(i, 1))
								.Store(0)
								.Store(1)
								.Load(0)
								.Load(1)
								.Load(0);
						}
						else
						{
							new DupHelper(classLoader, ilGenerator, 3)
								.SetType(0, type1)
								.SetType(1, ma.GetRawStackType(i, 1))
								.SetType(2, ma.GetRawStackType(i, 2))
								.Store(0)
								.Store(1)
								.Store(2)
								.Load(1)
								.Load(0)
								.Load(2)
								.Load(1)
								.Load(0);
						}
						break;
					}
					case NormalizedByteCode.__dup2_x2:
					{
						string type1 = ma.GetRawStackType(i, 0);
						string type2 = ma.GetRawStackType(i, 1);
						if(type1 == "D" || type1 == "J")
						{
							if(type2 == "D" || type2 == "J")
							{
								// Form 4
								new DupHelper(classLoader, ilGenerator, 2)
									.SetType(0, type1)
									.SetType(1, type2)
									.Store(0)
									.Store(1)
									.Load(0)
									.Load(1)
									.Load(0);
							}
							else
							{
								// Form 2
								new DupHelper(classLoader, ilGenerator, 3)
									.SetType(0, type1)
									.SetType(1, type2)
									.SetType(2, ma.GetRawStackType(i, 2))
									.Store(0)
									.Store(1)
									.Store(2)
									.Load(0)
									.Load(2)
									.Load(1)
									.Load(0);
							}
						}
						else
						{
							string type3 = ma.GetRawStackType(i, 2);
							if(type3 == "D" || type3 == "J")
							{
								// Form 3
								new DupHelper(classLoader, ilGenerator, 3)
									.SetType(0, type1)
									.SetType(1, type2)
									.SetType(2, type3)
									.Store(0)
									.Store(1)
									.Store(2)
									.Load(1)
									.Load(0)
									.Load(2)
									.Load(1)
									.Load(0);
							}
							else
							{
								// Form 1
								new DupHelper(classLoader, ilGenerator, 4)
									.SetType(0, type1)
									.SetType(1, type2)
									.SetType(2, type3)
									.SetType(3, ma.GetRawStackType(i, 3))
									.Store(0)
									.Store(1)
									.Store(2)
									.Store(3)
									.Load(1)
									.Load(0)
									.Load(3)
									.Load(2)
									.Load(1)
									.Load(0);
							}
						}
						break;
					}
					case NormalizedByteCode.__dup_x2:
						new DupHelper(classLoader, ilGenerator, 3)
							.SetType(0, ma.GetRawStackType(i, 0))
							.SetType(1, ma.GetRawStackType(i, 1))
							.SetType(2, ma.GetRawStackType(i, 2))
							.Store(0)
							.Store(1)
							.Store(2)
							.Load(0)
							.Load(2)
							.Load(1)
							.Load(0);
						break;
					case NormalizedByteCode.__pop2:
					{
						string type1 = ma.GetRawStackType(i, 0);
						if(type1 == "D" || type1 == "J")
						{
							ilGenerator.Emit(OpCodes.Pop);
						}
						else
						{
							if(type1[0] != 'N')
							{
								ilGenerator.Emit(OpCodes.Pop);
							}
							if(ma.GetRawStackType(i, 1)[0] != 'N')
							{
								ilGenerator.Emit(OpCodes.Pop);
							}
						}
						break;
					}
					case NormalizedByteCode.__pop:
						// if the TOS is a new object, it isn't really there, so we don't need to pop it
						if(ma.GetRawStackType(i, 0)[0] != 'N')
						{
							ilGenerator.Emit(OpCodes.Pop);
						}
						break;
					case NormalizedByteCode.__monitorenter:
						ilGenerator.Emit(OpCodes.Call, monitorEnterMethod);
						break;
					case NormalizedByteCode.__monitorexit:
						ilGenerator.Emit(OpCodes.Call, monitorExitMethod);
						break;
					case NormalizedByteCode.__athrow:
						ilGenerator.Emit(OpCodes.Throw);
						break;
					case NormalizedByteCode.__lookupswitch:
						// TODO use OpCodes.Switch
						for(int j = 0; j < instr.Values.Length; j++)
						{
							ilGenerator.Emit(OpCodes.Dup);
							ilGenerator.Emit(OpCodes.Ldc_I4, instr.Values[j]);
							Label label = ilGenerator.DefineLabel();
							ilGenerator.Emit(OpCodes.Bne_Un, label);
							ilGenerator.Emit(OpCodes.Pop);
							ilGenerator.Emit(OpCodes.Br, GetLabel(labels, instr.PC + instr.TargetOffsets[j], inuse, rangeBegin, rangeEnd, exits));
							ilGenerator.MarkLabel(label);
						}
						ilGenerator.Emit(OpCodes.Pop);
						ilGenerator.Emit(OpCodes.Br, GetLabel(labels, instr.PC + instr.DefaultOffset, inuse, rangeBegin, rangeEnd, exits));
						break;
					case NormalizedByteCode.__iinc:
						Load(instr, typeof(int));
						ilGenerator.Emit(OpCodes.Ldc_I4, instr.Arg2);
						ilGenerator.Emit(OpCodes.Add);
						Store(instr, typeof(int));
						break;
					case NormalizedByteCode.__i2b:
						ilGenerator.Emit(OpCodes.Conv_I1);
						break;
					case NormalizedByteCode.__i2c:
						ilGenerator.Emit(OpCodes.Conv_U2);
						break;
					case NormalizedByteCode.__i2s:
						ilGenerator.Emit(OpCodes.Conv_I2);
						break;
					case NormalizedByteCode.__l2i:
					case NormalizedByteCode.__f2i:
					case NormalizedByteCode.__d2i:
						ilGenerator.Emit(OpCodes.Conv_I4);
						break;
					case NormalizedByteCode.__i2l:
					case NormalizedByteCode.__f2l:
					case NormalizedByteCode.__d2l:
						ilGenerator.Emit(OpCodes.Conv_I8);
						break;
					case NormalizedByteCode.__i2f:
					case NormalizedByteCode.__l2f:
					case NormalizedByteCode.__d2f:
						ilGenerator.Emit(OpCodes.Conv_R4);
						break;
					case NormalizedByteCode.__i2d:
					case NormalizedByteCode.__l2d:
					case NormalizedByteCode.__f2d:
						ilGenerator.Emit(OpCodes.Conv_R8);
						break;
					case NormalizedByteCode.__jsr:
					{
						int index = FindPcIndex(instr.PC + instr.Arg1);
						int[] callsites = ma.GetCallSites(index);
						for(int j = 0; j < callsites.Length; j++)
						{
							if(callsites[j] == i)
							{
								ilGenerator.Emit(OpCodes.Ldc_I4, j);
								break;
							}
						}
						ilGenerator.Emit(OpCodes.Br, GetLabel(labels, instr.PC + instr.Arg1, inuse, rangeBegin, rangeEnd, exits));
						break;
					}
					case NormalizedByteCode.__ret:
					{
						// NOTE using a OpCodes.Switch here is not efficient, because 99 out of a 100 cases
						// there are either one or two call sites.
						string subid = ma.GetLocalType(i, instr.Arg1);
						int[] callsites = ma.GetCallSites(int.Parse(subid.Substring("Lret;".Length)));
						for(int j = 0; j < callsites.Length - 1; j++)
						{
							Load(instr, typeof(int));
							ilGenerator.Emit(OpCodes.Ldc_I4, j);
							ilGenerator.Emit(OpCodes.Beq, GetLabel(labels, m.Instructions[callsites[j] + 1].PC, inuse, rangeBegin, rangeEnd, exits));
						}
						ilGenerator.Emit(OpCodes.Br, GetLabel(labels, m.Instructions[callsites[callsites.Length - 1] + 1].PC, inuse, rangeBegin, rangeEnd, exits));
						break;
					}
					case NormalizedByteCode.__nop:
						ilGenerator.Emit(OpCodes.Nop);
						break;
					default:
						throw new NotImplementedException(instr.NormalizedOpCode.ToString());
				}
				// mark next instruction as inuse
				switch(instr.NormalizedOpCode)
				{
					case NormalizedByteCode.__lookupswitch:
					case NormalizedByteCode.__goto:
					case NormalizedByteCode.__jsr:
					case NormalizedByteCode.__ret:
					case NormalizedByteCode.__ireturn:
					case NormalizedByteCode.__lreturn:
					case NormalizedByteCode.__freturn:
					case NormalizedByteCode.__dreturn:
					case NormalizedByteCode.__areturn:
					case NormalizedByteCode.__return:
					case NormalizedByteCode.__athrow:
						break;
					default:
						// don't fall through end of try block
						if(m.Instructions[i + 1].PC == rangeEnd)
						{
							ilGenerator.Emit(OpCodes.Br, GetLabel(labels, m.Instructions[i + 1].PC, inuse, rangeBegin, rangeEnd, exits));
						}
						else
						{
							inuse[i + 1] = true;
							if(done[i + 1])
							{
								// since we've already processed the code that is supposed to come next, we have
								// to emit a branch to it
								ilGenerator.Emit(OpCodes.Br, GetLabel(labels, m.Instructions[i + 1].PC, inuse, rangeBegin, rangeEnd, exits));
							}
						}
						break;
				}
			}
		}
	}

	private FieldWrapper GetField(ClassFile.ConstantPoolItemFieldref cpi, bool isStatic, TypeWrapper thisType, bool write)
	{
		TypeWrapper wrapper = LoadClass(cpi.Class);
		if(wrapper != null)
		{
			FieldWrapper field = wrapper.GetFieldWrapper(cpi.Name);
			if(field != null)
			{
				if(field.IsStatic == isStatic)
				{
					if(field.IsPublic ||
						(field.IsProtected && (isStatic ? clazz.IsSubTypeOf(field.DeclaringType) : clazz.IsSubTypeOf(thisType))) ||
						(field.IsPrivate && clazz == wrapper) ||
						(!(field.IsPublic || field.IsPrivate) && clazz.IsInSamePackageAs(field.DeclaringType)))
					{
						// are we trying to mutate a final field (they are read-only from outside of the defining class)
						if(write && field.IsFinal && (isStatic ? clazz != wrapper : clazz != thisType))
						{
							EmitError("java.lang.IllegalAccessError", "Field " + cpi.Class + "." + cpi.Name + " is final");
						}
						else
						{
							return field;
						}
					}
					else
					{
						EmitError("java.lang.IllegalAccessError", "Try to access field " + cpi.Class + "." + cpi.Name + " from class " + clazz.Name);
					}
				}
				else
				{
					EmitError("java.lang.IncompatibleClassChangeError", null);
				}
			}
			else
			{
				EmitError("java.lang.NoSuchFieldError", cpi.Class + "." + cpi.Name);
			}
		}
		return null;
	}

	private static MethodWrapper GetInterfaceMethod(TypeWrapper wrapper, MethodDescriptor md)
	{
		MethodWrapper method = wrapper.GetMethodWrapper(md, false);
		if(method != null)
		{
			return method;
		}
		TypeWrapper[] interfaces = wrapper.Interfaces;
		for(int i = 0; i < interfaces.Length; i++)
		{
			method = GetInterfaceMethod(interfaces[i], md);
			if(method != null)
			{
				return method;
			}
		}
		return null;
	}

	private MethodWrapper GetMethod(ClassFile.ConstantPoolItemFMI cpi, TypeWrapper thisType, NormalizedByteCode invoke)
	{
		// TODO when there is an error resolving a call to the super class constructor (in the constructor of this type),
		// we cannot use EmitError, because that will yield an invalid constructor (that doesn't call the superclass constructor)
		TypeWrapper wrapper = LoadClass(cpi.Class);
		if(wrapper != null)
		{
			if(wrapper.IsInterface != (invoke == NormalizedByteCode.__invokeinterface))
			{
				EmitError("java.lang.IncompatibleClassChangeError", null);
			}
			else
			{
				if(invoke == NormalizedByteCode.__invokespecial && m.Method.ClassFile.IsSuper && thisType != wrapper && thisType.IsSubTypeOf(wrapper))
				{
					wrapper = thisType.BaseTypeWrapper;
				}
				MethodDescriptor md = new MethodDescriptor(classLoader, cpi.Name, cpi.Signature);
				MethodWrapper method = null;
				if(invoke == NormalizedByteCode.__invokeinterface)
				{
					method = GetInterfaceMethod(wrapper, md);
					// NOTE vmspec 5.4.3.4 clearly states that an interfacemethod may also refer to a method in Object
					if(method == null)
					{
						method = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassBySlashedName("java/lang/Object").GetMethodWrapper(md, false);
					}
				}
				else
				{
					method = wrapper.GetMethodWrapper(md, md.Name != "<init>");
					// if the method is not found, we might have to simulate a Miranda method
					if(method == null && invoke == NormalizedByteCode.__invokevirtual)
					{
						method = GetInterfaceMethod(wrapper, md);
					}
				}
				if(method != null)
				{
					if(method.IsStatic == (invoke == NormalizedByteCode.__invokestatic))
					{
						if(method.IsAbstract && invoke == NormalizedByteCode.__invokespecial)
						{
							EmitError("java.lang.AbstractMethodError", cpi.Class + "." + cpi.Name + cpi.Signature);
						}
						else if(method.IsPublic ||
							(method.IsProtected && (method.IsStatic ? clazz.IsSubTypeOf(method.DeclaringType) : clazz.IsSubTypeOf(thisType))) ||
							(method.IsPrivate && clazz == method.DeclaringType) ||
							(!(method.IsPublic || method.IsPrivate) && clazz.IsInSamePackageAs(method.DeclaringType)))
						{
							return method;
						}
						else
						{
							// HACK special case for incorrect invocation of Object.clone(), because this could mean
							// we're calling clone() on an array
							// (bug in javac, see http://developer.java.sun.com/developer/bugParade/bugs/4329886.html)
							if(!method.IsStatic && cpi.Name == "clone" && wrapper.Type == typeof(object) && thisType.Type.IsArray)
							{
								method = thisType.GetMethodWrapper(new MethodDescriptor(classLoader, cpi.Name, cpi.Signature), false);
								if(method != null && method.IsPublic)
								{
									return method;
								}
							}
							EmitError("java.lang.IllegalAccessError", "Try to access method " + method.DeclaringType.Name + "." + cpi.Name + cpi.Signature + " from class " + clazz.Name);
						}
					}
					else
					{
						EmitError("java.lang.IncompatibleClassChangeError", null);
					}
				}
				else
				{
					EmitError("java.lang.NoSuchMethodError", cpi.Class + "." + cpi.Name + cpi.Signature);
				}
			}
		}
		return null;
	}

	private void EmitError(string errorType, string message)
	{
		if(message != null)
		{
			if(JVM.IsStaticCompiler)
			{
				Console.Error.WriteLine(errorType + ": " + message);
				Console.Error.WriteLine("\tat " + m.Method.ClassFile.Name + "." + m.Method.Name + m.Method.Signature);
			}
			ilGenerator.Emit(OpCodes.Ldstr, message);
			TypeWrapper type = classLoader.LoadClassByDottedName(errorType);
			MethodWrapper method = type.GetMethodWrapper(new MethodDescriptor(classLoader, "<init>", "(Ljava/lang/String;)V"), false);
			method.EmitNewobj.Emit(ilGenerator);
		}
		else
		{
			if(JVM.IsStaticCompiler)
			{
				Console.Error.WriteLine(errorType);
				Console.Error.WriteLine("\tat " + m.Method.ClassFile.Name + "." + m.Method.Name + m.Method.Signature);
			}
			TypeWrapper type = classLoader.LoadClassByDottedName(errorType);
			MethodWrapper method = type.GetMethodWrapper(new MethodDescriptor(classLoader, "<init>", "()V"), false);
			method.EmitNewobj.Emit(ilGenerator);
		}
		// we emit a call to ThrowHack instead of a throw instruction, because otherwise the verifier will know
		// that execution won't continue at the next instruction, and the emitted code won't be verifiable
		ilGenerator.Emit(OpCodes.Call, throwHack);
	}

	private TypeWrapper LoadClass(string classname)
	{
		try
		{
			TypeWrapper type = classLoader.LoadClassBySlashedName(classname);
			if(!type.IsPublic && !clazz.IsInSamePackageAs(type))
			{
				// TODO all classnames in error messages should be dotted instead of slashed
				EmitError("java.lang.IllegalAccessError", "Try to access class " + classname + " from class " + clazz.Name);
				return null;
			}
			return type;
		}
		catch(Exception)
		{
			// TODO we should freeze the exception here, instead of always throwing a NoClassDefFoundError
			EmitError("java.lang.NoClassDefFoundError", classname);
			return null;
		}
	}

	private Type ExpressionType(string type)
	{
		try
		{
			return classLoader.ExpressionType(type);
		}
		catch(Exception)
		{
			// TODO we should freeze the exception here, instead of always throwing a NoClassDefFoundError
			EmitError("java.lang.NoClassDefFoundError", type);
			return null;
		}
	}

	private static string SigTypeToClassName(string type, string nullType)
	{
		switch(type[0])
		{
			case 'N':
			case 'U':
			{
				string chop = type.Substring(type.IndexOf(';') + 2);
				return chop.Substring(0, chop.Length - 1);
			}
			case 'L':
				if(type == "Lnull")
				{
					return nullType;
				}
				else
				{
					return type.Substring(1, type.Length - 2);
				}
			case '[':
				return type;
			default:
				throw new InvalidOperationException();
		}
	}

	private void EmitPlaceholder(string sig)
	{
		switch(sig[0])
		{
			case 'L':
			case '[':
				ilGenerator.Emit(OpCodes.Ldnull);
				break;
			case 'Z':
			case 'B':
			case 'S':
			case 'C':
			case 'I':
				ilGenerator.Emit(OpCodes.Ldc_I4_0);
				break;
			case 'J':
				ilGenerator.Emit(OpCodes.Ldc_I8, 0L);
				break;
			case 'F':
				ilGenerator.Emit(OpCodes.Ldc_R4, 0.0f);
				break;
			case 'D':
				ilGenerator.Emit(OpCodes.Ldc_R8, 0.0);
				break;
			case 'V':
				break;
			default:
				throw new InvalidOperationException();
		}
	}

	private int FindPcIndex(int target)
	{
		return m.PcIndexMap[target];
	}

	private void Load(ClassFile.Method.Instruction instr, Type type)
	{
		// TODO this check will become more complex, once we support changing the type of an argument 'local'
		if(instr.NormalizedArg1 >= m.ArgMap.Length)
		{
			// OPTIMIZE use short form when possible
			ilGenerator.Emit(OpCodes.Ldloc, GetLocal(type, instr.NormalizedArg1));
		}
		else
		{
			int i = m.ArgMap[instr.NormalizedArg1];
			switch(i)
			{
				case 0:
					ilGenerator.Emit(OpCodes.Ldarg_0);
					break;
				case 1:
					ilGenerator.Emit(OpCodes.Ldarg_1);
					break;
				case 2:
					ilGenerator.Emit(OpCodes.Ldarg_2);
					break;
				case 3:
					ilGenerator.Emit(OpCodes.Ldarg_3);
					break;
				default:
					if(i < 256)
					{
						ilGenerator.Emit(OpCodes.Ldarg_S, (byte)i);
					}
					else
					{
						ilGenerator.Emit(OpCodes.Ldarg, (ushort)i);
					}
					break;
			}
		}
	}

	private void Store(ClassFile.Method.Instruction instr, Type type)
	{
		// TODO this check will become more complex, once we support changing the type of an argument 'local'
		if(instr.NormalizedArg1 >= m.ArgMap.Length)
		{
			ilGenerator.Emit(OpCodes.Stloc, GetLocal(type, instr.NormalizedArg1));
		}
		else
		{
			int i = m.ArgMap[instr.NormalizedArg1];
			if(i < 256)
			{
				ilGenerator.Emit(OpCodes.Starg_S, (byte)i);
			}
			else
			{
				ilGenerator.Emit(OpCodes.Starg, (ushort)i);
			}
		}
	}

	private LocalBuilder GetLocal(Type type, int index)
	{
		string name;
		if(type.IsValueType)
		{
			name = type.Name + index;
		}
		else
		{
			name = "Obj" + index;
			string t = ma.GetDeclaredLocalType(index);
			if(t != null && t != "Lnull")
			{
				// TODO handle class not found
				type = classLoader.ExpressionType(t);
			}
		}
		LocalBuilder lb = (LocalBuilder)locals[name];
		if(lb == null)
		{
			lb = ilGenerator.DeclareLocal(type);
			locals[name] = lb;
			// the local variable table is disabled, because we need to have
			// better support for overloaded indexes to make this usefull
			if(JVM.Debug && false)
			{
				// TODO this should be done better
				ClassFile.Method.LocalVariableTableEntry[] table = m.LocalVariableTableAttribute;
				if(table != null)
				{
					for(int i = 0; i < table.Length; i++)
					{
						if(table[i].index == index)
						{
							lb.SetLocalSymInfo(table[i].name);
							break;
						}
					}
				}
			}
		}
		return lb;
	}

	private Label GetLabel(object[] labels, int targetPC, bool[] inuse, int rangeBegin, int rangeEnd, ArrayList exits)
	{
		if(rangeBegin <= targetPC && targetPC < rangeEnd)
		{
			inuse[FindPcIndex(targetPC)] = true;
			object l = labels[targetPC];
			if(l == null)
			{
				l = ilGenerator.DefineLabel();
				labels[targetPC] = l;
			}
			return (Label)l;
		}
		else
		{
			BranchCookie bc = new BranchCookie();
			bc.TargetIndex = FindPcIndex(targetPC);
			bc.Stub = ilGenerator.DefineLabel();
			exits.Add(bc);
			return bc.Stub;
		}
	}
}
