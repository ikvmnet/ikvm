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
	private static CodeEmitter mapExceptionMethod;
	private static CodeEmitter mapExceptionFastMethod;
	private static CodeEmitter fillInStackTraceMethod;
	private static MethodInfo getTypeFromHandleMethod = typeof(Type).GetMethod("GetTypeFromHandle");
	private static MethodInfo getClassFromTypeMethod = typeof(NativeCode.java.lang.VMClass).GetMethod("getClassFromType");
	private static MethodInfo multiANewArrayMethod = typeof(ByteCodeHelper).GetMethod("multianewarray");
	private static MethodInfo monitorEnterMethod = typeof(System.Threading.Monitor).GetMethod("Enter");
	private static MethodInfo monitorExitMethod = typeof(System.Threading.Monitor).GetMethod("Exit");
	private static MethodInfo objectToStringMethod = typeof(object).GetMethod("ToString");
	private static TypeWrapper java_lang_Object;
	private static TypeWrapper java_lang_Class;
	private static TypeWrapper java_lang_Throwable;
	private static TypeWrapper java_lang_ThreadDeath;
	private TypeWrapper clazz;
	private ClassFile.Method.Code m;
	private ILGenerator ilGenerator;
	private ClassLoaderWrapper classLoader;
	private MethodAnalyzer ma;
	private Hashtable locals = new Hashtable();
	private ClassFile.Method.ExceptionTableEntry[] exceptions;
	private ISymbolDocumentWriter symboldocument;

	static Compiler()
	{
		TypeWrapper exceptionHelper = ClassLoaderWrapper.LoadClassCritical("java.lang.ExceptionHelper");
		mapExceptionMethod = exceptionHelper.GetMethodWrapper(MethodDescriptor.FromNameSig(exceptionHelper.GetClassLoader(), "MapException", "(Ljava.lang.Throwable;Lcli.System.Type;)Ljava.lang.Throwable;"), false).EmitCall;
		mapExceptionFastMethod = exceptionHelper.GetMethodWrapper(MethodDescriptor.FromNameSig(exceptionHelper.GetClassLoader(), "MapExceptionFast", "(Ljava.lang.Throwable;)Ljava.lang.Throwable;"), false).EmitCall;
		fillInStackTraceMethod = exceptionHelper.GetMethodWrapper(MethodDescriptor.FromNameSig(exceptionHelper.GetClassLoader(), "fillInStackTrace", "(Ljava.lang.Throwable;)Ljava.lang.Throwable;"), false).EmitCall;
		java_lang_Throwable = CoreClasses.java_lang_Throwable;
		java_lang_Object = CoreClasses.java_lang_Object;
		java_lang_Class = CoreClasses.java_lang_Class;
		java_lang_ThreadDeath = ClassLoaderWrapper.LoadClassCritical("java.lang.ThreadDeath");
	}

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
		ma = new MethodAnalyzer(clazz, m, classLoader);
		Profiler.Leave("MethodAnalyzer");
		ArrayList ar = new ArrayList(m.ExceptionTable);
		//		Console.WriteLine(m.Method.ClassFile.Name + "." + m.Method.Name + m.Method.Signature);
		//		Console.WriteLine("before processing:");
		//		foreach(ExceptionTableEntry e in ar)
		//		{
		//			Console.WriteLine("{0} to {1} handler {2}", e.start_pc, e.end_pc, e.handler_pc);
		//		}
		// TODO it's very bad practice to mess with ExceptionTableEntrys that are owned by the Method, yet we
		// do that here, should be changed to use our own ETE class (which should also contain the ordinal, instead
		// of the one in ClassFile.cs)
		// OPTIMIZE there must be a more efficient algorithm to do this...
		// TODO we should ensure that exception blocks and handlers start and end at instruction boundaries (note: wide prefix)
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
			for(int j = 0; j < ar.Count; j++)
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
			else
			{
				// exception blocks that only contain harmless instructions (i.e. instructions that will *never* throw an exception)
				// are also filtered out (to improve the quality of the generated code)
				// NOTE we don't remove exception handlers that could catch ThreadDeath, because that can be thrown
				// asynchronously (and thus appear on any instruction). This is particularly important to ensure that
				// we run finally blocks when a thread is killed.
				if(ei.catch_type != 0)
				{
					TypeWrapper exceptionType = m.Method.ClassFile.GetConstantPoolClassType(ei.catch_type, classLoader);
					if(!exceptionType.IsUnloadable && !java_lang_ThreadDeath.IsAssignableTo(exceptionType))
					{
						int start = FindPcIndex(ei.start_pc);
						int end = FindPcIndex(ei.end_pc);
						for(int j = start; j < end; j++)
						{
							if(ByteCodeMetaData.CanThrowException(m.Instructions[j].OpCode))
							{
								goto next;
							}
						}
						ar.RemoveAt(i);
						i--;
					}
				}
			}
		next:;
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
					throw new InvalidOperationException("Partially overlapping try blocks is broken");
				}
				// check that we didn't destroy the ordering, when sorting
				if(exceptions[i].start_pc <= exceptions[j].start_pc &&
					exceptions[i].end_pc >= exceptions[j].end_pc &&
					exceptions[i].ordinal < exceptions[j].ordinal)
				{
					throw new InvalidOperationException("Non recursive try blocks is broken");
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
						throw new InvalidOperationException("Try block splitting around __jsr is broken");
					}
				}
			}
		}
	}

	private class EmitException : ApplicationException
	{
		private TypeWrapper type;

		internal EmitException(string message, TypeWrapper type)
			: base(message)
		{
			this.type = type;
		}

		internal void Emit(ILGenerator ilgen, ClassFile.Method m)
		{
			Tracer.Warning(Tracer.Compiler, "{0}: {1}\n\tat {2}.{3}{4}", type.Name, Message, m.ClassFile.Name, m.Name, m.Signature);
			ilgen.Emit(OpCodes.Ldstr, Message);
			MethodWrapper method = type.GetMethodWrapper(MethodDescriptor.FromNameSig(type.GetClassLoader(), "<init>", "(Ljava.lang.String;)V"), false);
			method.EmitNewobj.Emit(ilgen);
			ilgen.Emit(OpCodes.Throw);
		}
	}

	private sealed class IllegalAccessError : EmitException
	{
		internal IllegalAccessError(string message)
			: base(message, ClassLoaderWrapper.LoadClassCritical("java.lang.IllegalAccessError"))
		{
		}
	}

	private sealed class InstantiationError : EmitException
	{
		internal InstantiationError(string message)
			: base(message, ClassLoaderWrapper.LoadClassCritical("java.lang.InstantiationError"))
		{
		}
	}

	private sealed class IncompatibleClassChangeError : EmitException
	{
		internal IncompatibleClassChangeError(string message)
			: base(message, ClassLoaderWrapper.LoadClassCritical("java.lang.IncompatibleClassChangeError"))
		{
		}
	}

	private sealed class NoSuchFieldError : EmitException
	{
		internal NoSuchFieldError(string message)
			: base(message, ClassLoaderWrapper.LoadClassCritical("java.lang.NoSuchFieldError"))
		{
		}
	}

	private sealed class AbstractMethodError : EmitException
	{
		internal AbstractMethodError(string message)
			: base(message, ClassLoaderWrapper.LoadClassCritical("java.lang.AbstractMethodError"))
		{
		}
	}
	
	private sealed class NoSuchMethodError : EmitException
	{
		internal NoSuchMethodError(string message)
			: base(message, ClassLoaderWrapper.LoadClassCritical("java.lang.NoSuchMethodError"))
		{
		}
	}

	private sealed class ReturnCookie
	{
		private Label stub;
		private LocalBuilder local;

		internal ReturnCookie(Label stub, LocalBuilder local)
		{
			this.stub = stub;
			this.local = local;
		}

		internal void EmitRet(ILGenerator ilgen)
		{
			ilgen.MarkLabel(stub);
			if(local != null)
			{
				ilgen.Emit(OpCodes.Ldloc, local);
			}
			ilgen.Emit(OpCodes.Ret);
		}
	}

	private sealed class BranchCookie
	{
		// NOTE Stub gets used for both the push stub (inside the exception block) as well as the pop stub (outside the block)
		internal Label Stub;
		internal bool ContentOnStack;
		internal readonly int TargetPC;
		internal DupHelper dh;

		internal BranchCookie(ILGenerator ilgen, int stackHeight, int targetPC)
		{
			this.Stub = ilgen.DefineLabel();
			this.TargetPC = targetPC;
			this.dh = new DupHelper(ilgen, stackHeight);
		}

		internal BranchCookie(Label label, int targetPC)
		{
			this.Stub = label;
			this.TargetPC = targetPC;
		}
	}

	private struct DupHelper
	{
		private enum StackType : byte
		{
			Null,
			New,
			UnitializedThis,
			Other
		}
		private ILGenerator ilgen;
		private StackType[] types;
		private LocalBuilder[] locals;

		internal DupHelper(ILGenerator ilgen, int count)
		{
			this.ilgen = ilgen;
			types = new StackType[count];
			locals = new LocalBuilder[count];
		}

		internal int Count
		{
			get
			{
				return types.Length;
			}
		}

		internal void SetType(int i, TypeWrapper type)
		{
			if(type == VerifierTypeWrapper.Null)
			{
				types[i] = StackType.Null;
			}
			else if(VerifierTypeWrapper.IsNew(type))
			{
				// new objects aren't really there on the stack
				types[i] = StackType.New;
			}
			else if(type == VerifierTypeWrapper.UninitializedThis)
			{
				// uninitialized references cannot be stored in a local, but we can reload them
				types[i] = StackType.UnitializedThis;
			}
			else
			{
				types[i] = StackType.Other;
				locals[i] = ilgen.DeclareLocal(type.TypeAsLocalOrStackType);
			}
		}

		internal void Load(int i)
		{
			switch(types[i])
			{
				case StackType.Null:
					ilgen.Emit(OpCodes.Ldnull);
					break;
				case StackType.New:
					// new objects aren't really there on the stack
					break;
				case StackType.UnitializedThis:
					ilgen.Emit(OpCodes.Ldarg_0);
					break;
				case StackType.Other:
					ilgen.Emit(OpCodes.Ldloc, locals[i]);
					break;
				default:
					throw new InvalidOperationException();
			}
		}

		internal void Store(int i)
		{
			switch(types[i])
			{
				case StackType.Null:
				case StackType.UnitializedThis:
					ilgen.Emit(OpCodes.Pop);
					break;
				case StackType.New:
					// new objects aren't really there on the stack
					break;
				case StackType.Other:
					ilgen.Emit(OpCodes.Stloc, locals[i]);
					break;
				default:
					throw new InvalidOperationException();
			}
		}
	}

	internal static void Compile(TypeWrapper clazz, ClassFile.Method m, ILGenerator ilGenerator, ClassLoaderWrapper classLoader)
	{
		TypeWrapper[] args= m.GetArgTypes(classLoader);
		for(int i = 0; i < args.Length; i++)
		{
			if(args[i].IsUnloadable)
			{
				ilGenerator.Emit(OpCodes.Ldarg, (ushort)(i + (m.IsStatic ? 0 : 1)));
				ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
				ilGenerator.Emit(OpCodes.Ldstr, args[i].Name);
				ilGenerator.Emit(OpCodes.Call, typeof(ByteCodeHelper).GetMethod("DynamicCast"));
				ilGenerator.Emit(OpCodes.Pop);
			}
		}
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
			string msg = string.Format("(class: {0}, method: {1}, signature: {2}, offset: {3}, instruction: {4}) {5}", x.Class, x.Method, x.Signature, x.ByteCodeOffset, x.Instruction, x.Message);
			EmitHelper.Throw(ilGenerator, "java.lang.VerifyError", msg);
			return;
		}
		Profiler.Enter("Compile");
		if(m.IsStatic && m.IsSynchronized)
		{
			ArrayList exits = new ArrayList();
			// TODO consider caching the Class object in a static field
			ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
			ilGenerator.Emit(OpCodes.Call, getTypeFromHandleMethod);
			ilGenerator.Emit(OpCodes.Call, getClassFromTypeMethod);
			ilGenerator.Emit(OpCodes.Dup);
			LocalBuilder monitor = ilGenerator.DeclareLocal(typeof(object));
			ilGenerator.Emit(OpCodes.Stloc, monitor);
			ilGenerator.Emit(OpCodes.Call, monitorEnterMethod);
			ilGenerator.BeginExceptionBlock();
			c.Compile(0, 0, exits);
			ilGenerator.BeginFinallyBlock();
			ilGenerator.Emit(OpCodes.Ldloc, monitor);
			ilGenerator.Emit(OpCodes.Call, monitorExitMethod);
			ilGenerator.EndExceptionBlock();
			foreach(ReturnCookie rc in exits)
			{
				rc.EmitRet(ilGenerator);
			}
		}
		else
		{
			c.Compile(0, 0, null);
		}
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
		object[] labels = new object[m.Instructions.Length];
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
								// HACK this nop is a workaround for a bizarre bug in System.Diagnostics.StackTrace
								// that causes it to report the incorrect line number sometimes...
								ilGenerator.Emit(OpCodes.Nop);
								ilGenerator.MarkSequencePoint(symboldocument, table[j].line_number, 0, table[j].line_number + 1, 0);
								break;
							}
						}
					}
				}
				if(true)
				{
					// TODO for now, every instruction has an associated label, optimize this
					// NOTE labels are local to the current block, this is needed because the same JVM instruction
					// can actually be compiled into several CLR instruction (in the case of exception block boundaries,
					// each boundary has its own stack hoisting code).
					object label = labels[i];
					if(label == null)
					{
						label = ilGenerator.DefineLabel();
						labels[i] = label;
					}
					ilGenerator.MarkLabel((Label)label);
				}

				// handle the try block here
				for(int j = exceptionIndex; j < exceptions.Length; j++)
				{
					if(exceptions[j].start_pc == instr.PC)
					{
						int stackHeight = ma.GetStackHeight(i);
						if(stackHeight != 0)
						{
							// TODO instead of creating new locals for each block, we should reuse them
							DupHelper dh = new DupHelper(ilGenerator, stackHeight);
							for(int k = 0; k < stackHeight; k++)
							{
								dh.SetType(k, ma.GetRawStackTypeWrapper(i, k));
								dh.Store(k);
							}
							ilGenerator.BeginExceptionBlock();
							for(int k = stackHeight - 1; k >= 0; k--)
							{
								dh.Load(k);
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
								if(bc.ContentOnStack)
								{
									bc.ContentOnStack = false;
									ilGenerator.MarkLabel(bc.Stub);
									int stack = bc.dh.Count;
									for(int n = 0; n < stack; n++)
									{
										bc.dh.Store(n);
									}
									bc.Stub = ilGenerator.DefineLabel();
									ilGenerator.Emit(OpCodes.Leave, bc.Stub);
								}
							}
						}
						TypeWrapper exceptionTypeWrapper;
						if(exceptions[j].catch_type == 0)
						{
							exceptionTypeWrapper = java_lang_Throwable;
						}
						else
						{
							exceptionTypeWrapper = m.Method.ClassFile.GetConstantPoolClassType(exceptions[j].catch_type, classLoader);
						}
						Type excType = exceptionTypeWrapper.TypeAsExceptionType;
						bool mapSafe = !exceptionTypeWrapper.IsUnloadable && !exceptionTypeWrapper.IsMapUnsafeException;
						if(true)
						{
							if(mapSafe)
							{
								ilGenerator.BeginCatchBlock(excType);
							}
							else
							{
								ilGenerator.BeginCatchBlock(typeof(Exception));
							}
							BranchCookie bc = new BranchCookie(ilGenerator, 1, exceptions[j].handler_pc);
							newExits.Add(bc);
							Instruction handlerInstr = code[FindPcIndex(exceptions[j].handler_pc)];
							bool unusedException = handlerInstr.NormalizedOpCode == NormalizedByteCode.__pop ||
									(handlerInstr.NormalizedOpCode == NormalizedByteCode.__astore &&
									!ma.IsAloadUsed(handlerInstr.NormalizedArg1));
							// special case for catch(Throwable) (and finally), that produces less code and
							// should be faster
							if(mapSafe || excType == typeof(Exception))
							{
								if(unusedException)
								{
									// we must still have an item on the stack, even though it isn't used!
									bc.dh.SetType(0, VerifierTypeWrapper.Null);
								}
								else
								{
									if(mapSafe)
									{
										ilGenerator.Emit(OpCodes.Dup);
									}
									mapExceptionFastMethod.Emit(ilGenerator);
									if(mapSafe)
									{
										ilGenerator.Emit(OpCodes.Pop);
									}
									bc.dh.SetType(0, exceptionTypeWrapper);
									bc.dh.Store(0);
								}
								ilGenerator.Emit(OpCodes.Leave, bc.Stub);
							}
							else
							{
								if(exceptionTypeWrapper.IsUnloadable)
								{
									ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
									ilGenerator.Emit(OpCodes.Ldstr, exceptionTypeWrapper.Name);
									ilGenerator.Emit(OpCodes.Call, typeof(ByteCodeHelper).GetMethod("DynamicGetTypeAsExceptionType"));
									mapExceptionMethod.Emit(ilGenerator);
								}
								else
								{
									ilGenerator.Emit(OpCodes.Ldtoken, excType);
									ilGenerator.Emit(OpCodes.Call, getTypeFromHandleMethod);
									mapExceptionMethod.Emit(ilGenerator);
									ilGenerator.Emit(OpCodes.Castclass, excType);
								}
								if(unusedException)
								{
									// we must still have an item on the stack, even though it isn't used!
									bc.dh.SetType(0, VerifierTypeWrapper.Null);
								}
								else
								{
									bc.dh.SetType(0, exceptionTypeWrapper);
									ilGenerator.Emit(OpCodes.Dup);
									bc.dh.Store(0);
								}
								Label rethrow = ilGenerator.DefineLabel();
								ilGenerator.Emit(OpCodes.Brfalse, rethrow);
								ilGenerator.Emit(OpCodes.Leave, bc.Stub);
								ilGenerator.MarkLabel(rethrow);
								ilGenerator.Emit(OpCodes.Rethrow);
							}
							ilGenerator.EndExceptionBlock();
						}
						for(int k = 0; k < newExits.Count; k++)
						{
							object exit = newExits[k];
							ReturnCookie rc = exit as ReturnCookie;
							if(rc != null)
							{
								if(exits == null)
								{
									rc.EmitRet(ilGenerator);
								}
								else
								{
									exits.Add(rc);
								}
							}
							else
							{
								BranchCookie bc = exit as BranchCookie;
								if(bc != null)
								{
									System.Diagnostics.Debug.Assert(!bc.ContentOnStack);
									// if the target is within the current range, we handle it, otherwise we
									// defer the cookie to our caller
									if(rangeBegin <= bc.TargetPC && bc.TargetPC < rangeEnd)
									{
										bc.ContentOnStack = true;
										ilGenerator.MarkLabel(bc.Stub);
										int stack = bc.dh.Count;
										for(int n = stack - 1; n >= 0; n--)
										{
											bc.dh.Load(n);
										}
										ilGenerator.Emit(OpCodes.Br, GetLabel(labels, bc.TargetPC, inuse, rangeBegin, rangeEnd, exits));
									}
									else
									{
										exits.Add(bc);
									}
								}
							}
						}
						goto restart;
					}
				}

				try
				{
					switch(instr.NormalizedOpCode)
					{
						case NormalizedByteCode.__getstatic:
						case NormalizedByteCode.__putstatic:
						case NormalizedByteCode.__getfield:
						case NormalizedByteCode.__putfield:
							GetPutField(instr, i);
							break;
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
							ilGenerator.Emit(OpCodes.Ldc_I4_0);
							ilGenerator.Emit(OpCodes.Conv_I8);
							break;
						case NormalizedByteCode.__lconst_1:
							ilGenerator.Emit(OpCodes.Ldc_I4_1);
							ilGenerator.Emit(OpCodes.Conv_I8);
							break;
						case NormalizedByteCode.__fconst_0:
						case NormalizedByteCode.__dconst_0:
							// floats are stored as native size on the stack, so both R4 and R8 are the same
							ilGenerator.Emit(OpCodes.Ldc_R4, 0.0f);
							break;
						case NormalizedByteCode.__fconst_1:
						case NormalizedByteCode.__dconst_1:
							// floats are stored as native size on the stack, so both R4 and R8 are the same
							ilGenerator.Emit(OpCodes.Ldc_R4, 1.0f);
							break;
						case NormalizedByteCode.__fconst_2:
							ilGenerator.Emit(OpCodes.Ldc_R4, 2.0f);
							break;
						case NormalizedByteCode.__ldc:
						{
							ClassFile cf = instr.MethodCode.Method.ClassFile;
							int constant = instr.Arg1;
							switch(cf.GetConstantPoolConstantType(constant))
							{
								case ClassFile.ConstantType.Double:
									ilGenerator.Emit(OpCodes.Ldc_R8, cf.GetConstantPoolConstantDouble(constant));
									break;
								case ClassFile.ConstantType.Float:
									ilGenerator.Emit(OpCodes.Ldc_R4, cf.GetConstantPoolConstantFloat(constant));
									break;
								case ClassFile.ConstantType.Integer:
									ilGenerator.Emit(OpCodes.Ldc_I4, cf.GetConstantPoolConstantInteger(constant));
									break;
								case ClassFile.ConstantType.Long:
									ilGenerator.Emit(OpCodes.Ldc_I8, cf.GetConstantPoolConstantLong(constant));
									break;
								case ClassFile.ConstantType.String:
									ilGenerator.Emit(OpCodes.Ldstr, cf.GetConstantPoolConstantString(constant));
									break;
								case ClassFile.ConstantType.Class:
								{
									TypeWrapper tw = cf.GetConstantPoolClassType(constant, classLoader);
									if(tw.IsUnloadable)
									{
										ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
										ilGenerator.Emit(OpCodes.Ldstr, tw.Name);
										ilGenerator.Emit(OpCodes.Call, typeof(ByteCodeHelper).GetMethod("DynamicClassLiteral"));
									}
									else
									{
										ilGenerator.Emit(OpCodes.Ldtoken, tw.IsRemapped ? tw.TypeAsBaseType : tw.TypeAsTBD);
										ilGenerator.Emit(OpCodes.Call, getTypeFromHandleMethod);
										ilGenerator.Emit(OpCodes.Call, getClassFromTypeMethod);
									}
									java_lang_Class.EmitCheckcast(clazz, ilGenerator);
									break;
								}
								default:
									throw new InvalidOperationException();
							}
							break;
						}
						case NormalizedByteCode.__invokestatic:
						{
							ClassFile.ConstantPoolItemFMI cpi = m.Method.ClassFile.GetMethodref(instr.Arg1);
							// HACK special case for calls to System.arraycopy, if the array arguments on the stack
							// are of a known array type, we can redirect to an optimized version of arraycopy.
							// TODO make sure that the java.lang.System we're referring to is in the bootstrap class loader
							if(cpi.Class == "java.lang.System" &&
								cpi.Name == "arraycopy" &&
								cpi.Signature == "(Ljava.lang.Object;ILjava.lang.Object;II)V")
							{
								TypeWrapper t1 = ma.GetRawStackTypeWrapper(i, 2);
								TypeWrapper t2 = ma.GetRawStackTypeWrapper(i, 4);
								if(t1.IsArray && t1 == t2)
								{
									switch(t1.Name[1])
									{
										case 'J':
										case 'D':
											ilGenerator.Emit(OpCodes.Call, typeof(NativeCode.java.lang.VMSystem).GetMethod("arraycopy_primitive_8"));
											break;
										case 'I':
										case 'F':
											ilGenerator.Emit(OpCodes.Call, typeof(NativeCode.java.lang.VMSystem).GetMethod("arraycopy_primitive_4"));
											break;
										case 'S':
										case 'C':
											ilGenerator.Emit(OpCodes.Call, typeof(NativeCode.java.lang.VMSystem).GetMethod("arraycopy_primitive_2"));
											break;
										case 'B':
										case 'Z':
											ilGenerator.Emit(OpCodes.Call, typeof(NativeCode.java.lang.VMSystem).GetMethod("arraycopy_primitive_1"));
											break;
										default:
											ilGenerator.Emit(OpCodes.Call, typeof(NativeCode.java.lang.VMSystem).GetMethod("arraycopy"));
											break;
									}
									break;
								}
							}
							CodeEmitter emitNewobj;
							CodeEmitter emitCall;
							CodeEmitter emitCallvirt;
							GetMethodCallEmitter(cpi, null, NormalizedByteCode.__invokestatic, out emitNewobj, out emitCall, out emitCallvirt);
							// if the stack values don't match the argument types (for interface argument types)
							// we must emit code to cast the stack value to the interface type
							CastInterfaceArgs(cpi.GetArgTypes(classLoader), i, false, false);
							emitCall.Emit(ilGenerator);
							break;
						}
						case NormalizedByteCode.__invokevirtual:
						case NormalizedByteCode.__invokeinterface:
						case NormalizedByteCode.__invokespecial:
						{
							ClassFile.ConstantPoolItemFMI cpi = m.Method.ClassFile.GetMethodref(instr.Arg1);
							int argcount = cpi.GetArgTypes(classLoader).Length;
							TypeWrapper type = ma.GetRawStackTypeWrapper(i, argcount);
							TypeWrapper thisType = SigTypeToClassName(type, cpi.GetClassType(classLoader));

							// if the stack values don't match the argument types (for interface argument types)
							// we must emit code to cast the stack value to the interface type
							if(instr.NormalizedOpCode == NormalizedByteCode.__invokespecial && cpi.Name == "<init>" && VerifierTypeWrapper.IsNew(type))
							{
								TypeWrapper[] args = cpi.GetArgTypes(classLoader);
								CastInterfaceArgs(args, i, false, false);
							}
							else
							{
								// the this reference is included in the argument list because it may also need to be cast
								TypeWrapper[] methodArgs = cpi.GetArgTypes(classLoader);
								TypeWrapper[] args = new TypeWrapper[methodArgs.Length + 1];
								methodArgs.CopyTo(args, 1);
								if(instr.NormalizedOpCode == NormalizedByteCode.__invokeinterface)
								{
									args[0] = cpi.GetClassType(classLoader);
								}
								else
								{
									args[0] = thisType;
								}
								CastInterfaceArgs(args, i, true, instr.NormalizedOpCode == NormalizedByteCode.__invokespecial && type != VerifierTypeWrapper.UninitializedThis);
							}

							CodeEmitter emitNewobj = null;
							CodeEmitter emitCall = null;
							CodeEmitter emitCallvirt = null;
							CodeEmitter emit = null;
							GetMethodCallEmitter(cpi, thisType, instr.NormalizedOpCode, out emitNewobj, out emitCall, out emitCallvirt);
							if(instr.NormalizedOpCode == NormalizedByteCode.__invokespecial)
							{
								emit = emitCall;
							}
							else
							{
								emit = emitCallvirt;
							}
							if(instr.NormalizedOpCode == NormalizedByteCode.__invokespecial && cpi.Name == "<init>")
							{
								if(VerifierTypeWrapper.IsNew(type))
								{
									// we have to construct a list of all the unitialized references to the object
									// we're about to create on the stack, so that we can reconstruct the stack after
									// the "newobj" instruction
									int trivcount = 0;
									bool nontrivial = false;
									bool[] stackfix = new bool[ma.GetStackHeight(i) - (argcount + 1)];
									bool[] localsfix = new bool[m.MaxLocals];
									for(int j = 0; j < stackfix.Length; j++)
									{
										if(ma.GetRawStackTypeWrapper(i, argcount + 1 + j) == type)
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
										if(ma.GetLocalTypeWrapper(i, j) == type)
										{
											localsfix[j] = true;
											nontrivial = true;
										}
									}
									emitNewobj.Emit(ilGenerator);
									if(!thisType.IsUnloadable && thisType.IsSubTypeOf(java_lang_Throwable))
									{
										// HACK if the next instruction isn't an athrow, we need to
										// call fillInStackTrace, because the object might be used
										// to print out a stack trace without ever being thrown
										if(code[i + 1].NormalizedOpCode != NormalizedByteCode.__athrow)
										{
											ilGenerator.Emit(OpCodes.Dup);
											fillInStackTraceMethod.Emit(ilGenerator);
											ilGenerator.Emit(OpCodes.Pop);
										}
									}
									if(nontrivial)
									{
										// this could be done a little more efficiently, but since in practice this
										// code never runs (for code compiled from Java source) it doesn't
										// really matter
										LocalBuilder newobj = ilGenerator.DeclareLocal(thisType.TypeAsTBD);
										ilGenerator.Emit(OpCodes.Stloc, newobj);
										LocalBuilder[] tempstack = new LocalBuilder[stackfix.Length];
										for(int j = 0; j < stackfix.Length; j++)
										{
											if(!stackfix[j])
											{
												TypeWrapper stacktype = ma.GetRawStackTypeWrapper(i, argcount + 1 + j);
												// it could be another new object reference (not from current invokespecial <init>
												// instruction)
												if(stacktype == VerifierTypeWrapper.Null)
												{
													// TODO handle null stack entries
													throw new NotImplementedException();
												}
												else if(!VerifierTypeWrapper.IsNew(stacktype))
												{
													LocalBuilder lb = ilGenerator.DeclareLocal(stacktype.TypeAsLocalOrStackType);
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
									emitCall.Emit(ilGenerator);
								}
							}
							else
							{
								emit.Emit(ilGenerator);
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
							if(exits != null)
							{
								// if we're inside an exception block, copy TOS to local, emit "leave" and push item onto our "todo" list
								LocalBuilder local = null;
								if(instr.NormalizedOpCode != NormalizedByteCode.__return)
								{
									TypeWrapper retTypeWrapper = m.Method.GetRetType(classLoader);
									retTypeWrapper.EmitConvStackToParameterType(ilGenerator, ma.GetRawStackTypeWrapper(i, 0));
									if(ma.GetRawStackTypeWrapper(i, 0).IsUnloadable)
									{
										ilGenerator.Emit(OpCodes.Castclass, retTypeWrapper.TypeAsParameterType);
									}
									local = ilGenerator.DeclareLocal(retTypeWrapper.TypeAsParameterType);
									ilGenerator.Emit(OpCodes.Stloc, local);
								}
								Label label = ilGenerator.DefineLabel();
								// NOTE leave automatically discards any junk that may be on the stack
								ilGenerator.Emit(OpCodes.Leave, label);
								exits.Add(new ReturnCookie(label, local));
							}
							else
							{
								// if there is junk on the stack (other than the return value), we must pop it off
								// because in .NET this is invalid (unlike in Java)
								int stackHeight = ma.GetStackHeight(i);
								if(instr.NormalizedOpCode == NormalizedByteCode.__return)
								{
									ilGenerator.Emit(OpCodes.Leave_S, (byte)0);
									ilGenerator.Emit(OpCodes.Ret);
								}
								else
								{
									TypeWrapper retTypeWrapper = m.Method.GetRetType(classLoader);
									retTypeWrapper.EmitConvStackToParameterType(ilGenerator, ma.GetRawStackTypeWrapper(i, 0));
									if(ma.GetRawStackTypeWrapper(i, 0).IsUnloadable)
									{
										ilGenerator.Emit(OpCodes.Castclass, retTypeWrapper.TypeAsParameterType);
									}
									if(stackHeight != 1)
									{
										LocalBuilder local = ilGenerator.DeclareLocal(retTypeWrapper.TypeAsParameterType);
										ilGenerator.Emit(OpCodes.Stloc, local);
										ilGenerator.Emit(OpCodes.Leave_S, (byte)0);
										ilGenerator.Emit(OpCodes.Ldloc, local);
									}
									ilGenerator.Emit(OpCodes.Ret);
								}
							}
							break;
						}
						case NormalizedByteCode.__aload:
						{
							TypeWrapper type = ma.GetLocalTypeWrapper(i, instr.NormalizedArg1);
							if(type == VerifierTypeWrapper.Null)
							{
								// if the local is known to be null, we just emit a null
								ilGenerator.Emit(OpCodes.Ldnull);
							}
							else if(VerifierTypeWrapper.IsNew(type))
							{
								// since new objects aren't represented on the stack, we don't need to do anything here
							}
							else if(type == VerifierTypeWrapper.UninitializedThis)
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
									if(type != ma.GetDeclaredLocalTypeWrapper(instr.NormalizedArg1) && !type.IsUnloadable && !type.IsGhost && !type.IsNonPrimitiveValueType)
									{
										ilGenerator.Emit(OpCodes.Castclass, type.TypeAsTBD);
									}
								}
								else
								{
									// HACK we're boxing the arguments when they are loaded, this is inconsistent
									// with the way locals are treated, so we probably should only box the arguments
									// once (on method entry)
									type.EmitConvParameterToStackType(ilGenerator);
								}
							}
							break;
						}
						case NormalizedByteCode.__astore:
						{
							TypeWrapper type = ma.GetRawStackTypeWrapper(i, 0);
							// HACK we use "int" to track the return address of a jsr
							if(VerifierTypeWrapper.IsRet(type))
							{
								Store(instr, typeof(int));
							}
							else if(VerifierTypeWrapper.IsNew(type))
							{
								// NOTE new objects aren't really on the stack, so we can't copy them into the local.
								// We do store a null in the local, to prevent it from retaining an unintentional reference
								// to whatever object reference happens to be there
								ilGenerator.Emit(OpCodes.Ldnull);
								Store(instr, typeof(object));
							}
							else if(type == VerifierTypeWrapper.UninitializedThis)
							{
								// any unitialized reference is always the this reference, we don't store anything
								// here (because CLR won't allow unitialized references in locals) and then when
								// the unitialized ref is loaded we redirect to the this reference
								ilGenerator.Emit(OpCodes.Pop);
							}
							else
							{
								if(instr.NormalizedArg1 < m.ArgMap.Length)
								{
									if(type != VerifierTypeWrapper.Null)
									{
										type.EmitConvStackToParameterType(ilGenerator, type);
									}
									if(type.IsUnloadable)
									{
										TypeWrapper[] args = m.Method.GetArgTypes(classLoader);
										int arg = m.ArgMap[instr.NormalizedArg1];
										if(!m.Method.IsStatic)
										{
											arg--;
										}
										if(arg == -1)
										{
											// TODO once we have this aliasing this should work
											throw new NotImplementedException("overwriting this with unloadable");
										}
										ilGenerator.Emit(OpCodes.Castclass, args[arg].TypeAsParameterType);
									}
								}
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
							TypeWrapper wrapper = instr.MethodCode.Method.ClassFile.GetConstantPoolClassType(instr.Arg1, classLoader);
							if(wrapper.IsUnloadable)
							{
								// this is here to make sure we throw the exception in the right location (before
								// evaluating the constructor arguments)
								ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
								ilGenerator.Emit(OpCodes.Ldstr, wrapper.Name);
								ilGenerator.Emit(OpCodes.Call, typeof(ByteCodeHelper).GetMethod("DynamicNewCheckOnly"));
							}
							else if(!wrapper.IsAccessibleFrom(clazz))
							{
								throw new IllegalAccessError("Try to access class " + wrapper.Name + " from class " + clazz.Name);
							}
							else if(wrapper.IsAbstract || wrapper.IsInterface)
							{
								throw new InstantiationError(wrapper.Name);
							}
							// we don't do anything here, the call to <init> will be converted into a newobj instruction
							break;
						}
						case NormalizedByteCode.__multianewarray:
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
							TypeWrapper wrapper = instr.MethodCode.Method.ClassFile.GetConstantPoolClassType(instr.Arg1, classLoader);
							if(wrapper.IsUnloadable)
							{
								ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
								ilGenerator.Emit(OpCodes.Ldstr, wrapper.Name);
								ilGenerator.Emit(OpCodes.Ldloc, localArray);
								ilGenerator.Emit(OpCodes.Call, typeof(ByteCodeHelper).GetMethod("DynamicMultianewarray"));
							}
							else
							{
								if(!wrapper.IsAccessibleFrom(clazz))
								{
									throw new IllegalAccessError("Try to access class " + wrapper.Name + " from class " + clazz.Name);
								}
								Type type = wrapper.TypeAsArrayType;
								ilGenerator.Emit(OpCodes.Ldtoken, type);
								ilGenerator.Emit(OpCodes.Ldloc, localArray);
								ilGenerator.Emit(OpCodes.Call, multiANewArrayMethod);
								ilGenerator.Emit(OpCodes.Castclass, type);
							}
							break;
						}
						case NormalizedByteCode.__anewarray:
						{
							TypeWrapper wrapper = instr.MethodCode.Method.ClassFile.GetConstantPoolClassType(instr.Arg1, classLoader);
							if(wrapper.IsUnloadable)
							{
								ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
								ilGenerator.Emit(OpCodes.Ldstr, wrapper.Name);
								ilGenerator.Emit(OpCodes.Call, typeof(ByteCodeHelper).GetMethod("DynamicNewarray"));
							}
							else
							{
								if(!wrapper.IsAccessibleFrom(clazz))
								{
									throw new IllegalAccessError("Try to access class " + wrapper.Name + " from class " + clazz.Name);
								}
								// NOTE for ghost types we create object arrays to make sure that Ghost implementers can be
								// stored in ghost arrays, but this has the unintended consequence that ghost arrays can
								// contain *any* reference type (because they are compiled as Object arrays). We could
								// modify aastore to emit code to check for this, but this would have an huge performance
								// cost for all object arrays.
								// Oddly, while the JVM accepts any reference for any other interface typed references, in the
								// case of aastore it does check that the object actually implements the interface. This
								// is unfortunate, but I think we can live with this minor incompatibility.
								// NOTE that this does not break type safety, because when the incorrect object is eventually
								// used as the ghost interface type it will generate a ClassCastException.
								ilGenerator.Emit(OpCodes.Newarr, wrapper.TypeAsArrayType);
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
								// this can't happen, the verifier would have caught it
								throw new InvalidOperationException();
						}
							break;
						case NormalizedByteCode.__checkcast:
						{
							TypeWrapper wrapper = instr.MethodCode.Method.ClassFile.GetConstantPoolClassType(instr.Arg1, classLoader);
							if(!wrapper.IsUnloadable && !wrapper.IsAccessibleFrom(clazz))
							{
								throw new IllegalAccessError("Try to access class " + wrapper.Name + " from class " + clazz.Name);
							}
							wrapper.EmitCheckcast(clazz, ilGenerator);
							break;
						}
						case NormalizedByteCode.__instanceof:
						{
							TypeWrapper wrapper = instr.MethodCode.Method.ClassFile.GetConstantPoolClassType(instr.Arg1, classLoader);
							if(!wrapper.IsUnloadable && !wrapper.IsAccessibleFrom(clazz))
							{
								throw new IllegalAccessError("Try to access class " + wrapper.Name + " from class " + clazz.Name);
							}
							wrapper.EmitInstanceOf(clazz, ilGenerator);
							break;
						}
						case NormalizedByteCode.__aaload:
						{
							TypeWrapper tw = ma.GetRawStackTypeWrapper(i, 1);
							if(tw.IsUnloadable)
							{
								ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
								ilGenerator.Emit(OpCodes.Ldstr, tw.Name);
								ilGenerator.Emit(OpCodes.Call, typeof(ByteCodeHelper).GetMethod("DynamicAaload"));
							}
							else
							{
								TypeWrapper elem = tw.ElementTypeWrapper;
								if(elem.IsNonPrimitiveValueType)
								{
									Type t = elem.TypeAsTBD;
									ilGenerator.Emit(OpCodes.Ldelema, t);
									ilGenerator.Emit(OpCodes.Ldobj, t);
									elem.EmitBox(ilGenerator);
								}
								else
								{
									ilGenerator.Emit(OpCodes.Ldelem_Ref);
								}
							}
							break;
						}
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
						{
							TypeWrapper tw = ma.GetRawStackTypeWrapper(i, 2);
							if(tw.IsUnloadable)
							{
								ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
								ilGenerator.Emit(OpCodes.Ldstr, tw.Name);
								ilGenerator.Emit(OpCodes.Call, typeof(ByteCodeHelper).GetMethod("DynamicAastore"));
							}
							else
							{
								TypeWrapper elem = tw.ElementTypeWrapper;
								if(elem.IsNonPrimitiveValueType)
								{
									Type t = elem.TypeAsTBD;
									LocalBuilder local = ilGenerator.DeclareLocal(typeof(object));
									ilGenerator.Emit(OpCodes.Stloc, local);
									ilGenerator.Emit(OpCodes.Ldelema, t);
									ilGenerator.Emit(OpCodes.Ldloc, local);
									elem.EmitUnbox(ilGenerator);
									ilGenerator.Emit(OpCodes.Stobj, t);
								}
								else
								{
									ilGenerator.Emit(OpCodes.Stelem_Ref);
								}
							}
							break;
						}
						case NormalizedByteCode.__arraylength:
							if(ma.GetRawStackTypeWrapper(i, 0).IsUnloadable)
							{
								ilGenerator.Emit(OpCodes.Castclass, typeof(Array));
								ilGenerator.Emit(OpCodes.Callvirt, typeof(Array).GetMethod("get_Length"));
							}
							else
							{
								ilGenerator.Emit(OpCodes.Ldlen);
							}
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
							ilGenerator.Emit(OpCodes.Ldc_I4, 31);
							ilGenerator.Emit(OpCodes.And);
							ilGenerator.Emit(OpCodes.Shl);
							break;
						case NormalizedByteCode.__lshl:
							ilGenerator.Emit(OpCodes.Ldc_I4, 63);
							ilGenerator.Emit(OpCodes.And);
							ilGenerator.Emit(OpCodes.Shl);
							break;
						case NormalizedByteCode.__iushr:
							ilGenerator.Emit(OpCodes.Ldc_I4, 31);
							ilGenerator.Emit(OpCodes.And);
							ilGenerator.Emit(OpCodes.Shr_Un);
							break;
						case NormalizedByteCode.__lushr:
							ilGenerator.Emit(OpCodes.Ldc_I4, 63);
							ilGenerator.Emit(OpCodes.And);
							ilGenerator.Emit(OpCodes.Shr_Un);
							break;
						case NormalizedByteCode.__ishr:
							ilGenerator.Emit(OpCodes.Ldc_I4, 31);
							ilGenerator.Emit(OpCodes.And);
							ilGenerator.Emit(OpCodes.Shr);
							break;
						case NormalizedByteCode.__lshr:
							ilGenerator.Emit(OpCodes.Ldc_I4, 63);
							ilGenerator.Emit(OpCodes.And);
							ilGenerator.Emit(OpCodes.Shr);
							break;
						case NormalizedByteCode.__swap:
						{
							DupHelper dh = new DupHelper(ilGenerator, 2);
							dh.SetType(0, ma.GetRawStackTypeWrapper(i, 0));
							dh.SetType(1, ma.GetRawStackTypeWrapper(i, 1));
							dh.Store(0);
							dh.Store(1);
							dh.Load(0);
							dh.Load(1);
							break;
						}
						case NormalizedByteCode.__dup:
							// if the TOS contains a "new" object, it isn't really there, so we don't dup it
							if(!VerifierTypeWrapper.IsNew(ma.GetRawStackTypeWrapper(i, 0)))
							{
								ilGenerator.Emit(OpCodes.Dup);
							}
							break;
						case NormalizedByteCode.__dup2:
						{
							TypeWrapper type1 = ma.GetRawStackTypeWrapper(i, 0);
							if(type1.IsWidePrimitive)
							{
								ilGenerator.Emit(OpCodes.Dup);
							}
							else
							{
								DupHelper dh = new DupHelper(ilGenerator, 2);
								dh.SetType(0, type1);
								dh.SetType(1, ma.GetRawStackTypeWrapper(i, 1));
								dh.Store(0);
								dh.Store(1);
								dh.Load(1);
								dh.Load(0);
								dh.Load(1);
								dh.Load(0);
							}
							break;
						}
						case NormalizedByteCode.__dup_x1:
						{
							DupHelper dh = new DupHelper(ilGenerator, 2);
							dh.SetType(0, ma.GetRawStackTypeWrapper(i, 0));
							dh.SetType(1, ma.GetRawStackTypeWrapper(i, 1));
							dh.Store(0);
							dh.Store(1);
							dh.Load(0);
							dh.Load(1);
							dh.Load(0);
							break;
						}
						case NormalizedByteCode.__dup2_x1:
						{
							TypeWrapper type1 = ma.GetRawStackTypeWrapper(i, 0);
							if(type1.IsWidePrimitive)
							{
								DupHelper dh = new DupHelper(ilGenerator, 2);
								dh.SetType(0, type1);
								dh.SetType(1, ma.GetRawStackTypeWrapper(i, 1));
								dh.Store(0);
								dh.Store(1);
								dh.Load(0);
								dh.Load(1);
								dh.Load(0);
							}
							else
							{
								DupHelper dh = new DupHelper(ilGenerator, 3);
								dh.SetType(0, type1);
								dh.SetType(1, ma.GetRawStackTypeWrapper(i, 1));
								dh.SetType(2, ma.GetRawStackTypeWrapper(i, 2));
								dh.Store(0);
								dh.Store(1);
								dh.Store(2);
								dh.Load(1);
								dh.Load(0);
								dh.Load(2);
								dh.Load(1);
								dh.Load(0);
							}
							break;
						}
						case NormalizedByteCode.__dup2_x2:
						{
							TypeWrapper type1 = ma.GetRawStackTypeWrapper(i, 0);
							TypeWrapper type2 = ma.GetRawStackTypeWrapper(i, 1);
							if(type1.IsWidePrimitive)
							{
								if(type2.IsWidePrimitive)
								{
									// Form 4
									DupHelper dh = new DupHelper(ilGenerator, 2);
									dh.SetType(0, type1);
									dh.SetType(1, type2);
									dh.Store(0);
									dh.Store(1);
									dh.Load(0);
									dh.Load(1);
									dh.Load(0);
								}
								else
								{
									// Form 2
									DupHelper dh = new DupHelper(ilGenerator, 3);
									dh.SetType(0, type1);
									dh.SetType(1, type2);
									dh.SetType(2, ma.GetRawStackTypeWrapper(i, 2));
									dh.Store(0);
									dh.Store(1);
									dh.Store(2);
									dh.Load(0);
									dh.Load(2);
									dh.Load(1);
									dh.Load(0);
								}
							}
							else
							{
								TypeWrapper type3 = ma.GetRawStackTypeWrapper(i, 2);
								if(type3.IsWidePrimitive)
								{
									// Form 3
									DupHelper dh = new DupHelper(ilGenerator, 3);
									dh.SetType(0, type1);
									dh.SetType(1, type2);
									dh.SetType(2, type3);
									dh.Store(0);
									dh.Store(1);
									dh.Store(2);
									dh.Load(1);
									dh.Load(0);
									dh.Load(2);
									dh.Load(1);
									dh.Load(0);
								}
								else
								{
									// Form 1
									DupHelper dh = new DupHelper(ilGenerator, 4);
									dh.SetType(0, type1);
									dh.SetType(1, type2);
									dh.SetType(2, type3);
									dh.SetType(3, ma.GetRawStackTypeWrapper(i, 3));
									dh.Store(0);
									dh.Store(1);
									dh.Store(2);
									dh.Store(3);
									dh.Load(1);
									dh.Load(0);
									dh.Load(3);
									dh.Load(2);
									dh.Load(1);
									dh.Load(0);
								}
							}
							break;
						}
						case NormalizedByteCode.__dup_x2:
						{
							DupHelper dh = new DupHelper(ilGenerator, 3);
							dh.SetType(0, ma.GetRawStackTypeWrapper(i, 0));
							dh.SetType(1, ma.GetRawStackTypeWrapper(i, 1));
							dh.SetType(2, ma.GetRawStackTypeWrapper(i, 2));
							dh.Store(0);
							dh.Store(1);
							dh.Store(2);
							dh.Load(0);
							dh.Load(2);
							dh.Load(1);
							dh.Load(0);
							break;
						}
						case NormalizedByteCode.__pop2:
						{
							TypeWrapper type1 = ma.GetRawStackTypeWrapper(i, 0);
							if(type1.IsWidePrimitive)
							{
								ilGenerator.Emit(OpCodes.Pop);
							}
							else
							{
								if(!VerifierTypeWrapper.IsNew(type1))
								{
									ilGenerator.Emit(OpCodes.Pop);
								}
								if(!VerifierTypeWrapper.IsNew(ma.GetRawStackTypeWrapper(i, 1)))
								{
									ilGenerator.Emit(OpCodes.Pop);
								}
							}
							break;
						}
						case NormalizedByteCode.__pop:
							// if the TOS is a new object, it isn't really there, so we don't need to pop it
							if(!VerifierTypeWrapper.IsNew(ma.GetRawStackTypeWrapper(i, 0)))
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
							ilGenerator.Emit(OpCodes.Conv_I4);
							break;
						case NormalizedByteCode.__f2i:
							ilGenerator.Emit(OpCodes.Call, typeof(ByteCodeHelper).GetMethod("f2i"));
							break;
						case NormalizedByteCode.__d2i:
							ilGenerator.Emit(OpCodes.Call, typeof(ByteCodeHelper).GetMethod("d2i"));
							break;
						case NormalizedByteCode.__f2l:
							ilGenerator.Emit(OpCodes.Call, typeof(ByteCodeHelper).GetMethod("f2l"));
							break;
						case NormalizedByteCode.__d2l:
							ilGenerator.Emit(OpCodes.Call, typeof(ByteCodeHelper).GetMethod("d2l"));
							break;
						case NormalizedByteCode.__i2l:
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
							int subid = ((VerifierTypeWrapper)ma.GetLocalTypeWrapper(i, instr.Arg1)).Index;
							int[] callsites = ma.GetCallSites(subid);
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
								// TODO instead of emitting a branch to the leave stub, it would be more efficient to put the leave stub here
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
				catch(EmitException x)
				{
					x.Emit(ilGenerator, m.Method);
				}
			}
		}
	}

	// NOTE despite its name this also handles value type args
	private void CastInterfaceArgs(TypeWrapper[] args, int instructionIndex, bool instanceMethod, bool checkThisForNull)
	{
		bool needsCast = checkThisForNull;

		if(!needsCast)
		{
			for(int i = 0; i < args.Length; i++)
			{
				if(args[i].IsUnloadable)
				{
					// nothing to do, callee will (eventually) do the cast
				}
				else if(args[i].IsGhost)
				{
					needsCast = true;
					break;
				}
				else if(args[i].IsInterfaceOrInterfaceArray)
				{
					TypeWrapper tw = ma.GetRawStackTypeWrapper(instructionIndex, args.Length - 1 - i);
					if(!tw.IsUnloadable && !tw.IsAssignableTo(args[i]))
					{
						needsCast = true;
						break;
					}
				}
				else if(args[i].IsNonPrimitiveValueType)
				{
					needsCast = true;
					break;
				}
				// if the stack contains an unloadable, we might need to cast it
				// (e.g. if the argument type is a base class that is loadable)
				if(ma.GetRawStackTypeWrapper(instructionIndex, i).IsUnloadable)
				{
					needsCast = true;
					break;
				}
			}
		}

		if(needsCast)
		{
			// OPTIMIZE if the first n arguments don't need a cast, they can be left on the stack
			DupHelper dh = new DupHelper(ilGenerator, args.Length);
			for(int i = 0; i < args.Length; i++)
			{
				TypeWrapper tw = ma.GetRawStackTypeWrapper(instructionIndex, args.Length - 1 - i);
				if(tw != VerifierTypeWrapper.UninitializedThis)
				{
					tw = args[i];
				}
				dh.SetType(i, tw);
			}
			for(int i = args.Length - 1; i >= 0; i--)
			{
				if(!args[i].IsUnloadable)
				{
					TypeWrapper tw = ma.GetRawStackTypeWrapper(instructionIndex, args.Length - 1 - i);
					if(tw.IsUnloadable || (args[i].IsInterfaceOrInterfaceArray && !tw.IsAssignableTo(args[i])))
					{
						// TODO ideally, instead of an InvalidCastException, the castclass should throw a IncompatibleClassChangeError
						ilGenerator.Emit(OpCodes.Castclass, args[i].TypeAsTBD);
					}
				}
				dh.Store(i);
			}
			if(checkThisForNull)
			{
				dh.Load(0);
				EmitHelper.NullCheck(ilGenerator);
			}
			for(int i = 0; i < args.Length; i++)
			{
				if(!args[i].IsUnloadable && args[i].IsGhost)
				{
					LocalBuilder local = ilGenerator.DeclareLocal(args[i].TypeAsParameterType);
					ilGenerator.Emit(OpCodes.Ldloca, local);
					dh.Load(i);
					ilGenerator.Emit(OpCodes.Stfld, args[i].GhostRefField);
					ilGenerator.Emit(OpCodes.Ldloca, local);
					// NOTE when the this argument is a value type, we need the address on the stack instead of the value
					if(i != 0 || !instanceMethod)
					{
						ilGenerator.Emit(OpCodes.Ldobj, args[i].TypeAsParameterType);
					}
				}
				else
				{
					dh.Load(i);
					if(!args[i].IsUnloadable)
					{
						if(args[i].IsNonPrimitiveValueType)
						{
							if(i != 0 || !instanceMethod)
							{
								args[i].EmitUnbox(ilGenerator);
							}
							else
							{
								ilGenerator.Emit(OpCodes.Unbox, args[i].TypeAsTBD);
							}
						}
						else if(ma.GetRawStackTypeWrapper(instructionIndex, args.Length - 1 - i).IsUnloadable)
						{
							ilGenerator.Emit(OpCodes.Castclass, args[i].TypeAsParameterType);
						}
					}
				}
			}
		}
	}

	private void GetPutField(Instruction instr, int i)
	{
		NormalizedByteCode bytecode = instr.NormalizedOpCode;
		ClassFile.ConstantPoolItemFieldref cpi = m.Method.ClassFile.GetFieldref(instr.Arg1);
		bool write = (bytecode == NormalizedByteCode.__putfield || bytecode == NormalizedByteCode.__putstatic);
		TypeWrapper wrapper = cpi.GetClassType(classLoader);
		if(wrapper.IsUnloadable)
		{
			TypeWrapper fieldTypeWrapper = cpi.GetFieldType(classLoader);
			if(write && !fieldTypeWrapper.IsUnloadable && fieldTypeWrapper.IsPrimitive)
			{
				ilGenerator.Emit(OpCodes.Box, fieldTypeWrapper.TypeAsTBD);
			}
			ilGenerator.Emit(OpCodes.Ldstr, cpi.Name);
			ilGenerator.Emit(OpCodes.Ldstr, cpi.Signature);
			ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
			ilGenerator.Emit(OpCodes.Ldstr, wrapper.Name);
			switch(bytecode)
			{
				case NormalizedByteCode.__getfield:
					ilGenerator.Emit(OpCodes.Call, typeof(ByteCodeHelper).GetMethod("DynamicGetfield"));
					EmitReturnTypeConversion(ilGenerator, fieldTypeWrapper);
					break;
				case NormalizedByteCode.__putfield:
					ilGenerator.Emit(OpCodes.Call, typeof(ByteCodeHelper).GetMethod("DynamicPutfield"));
					break;
				case NormalizedByteCode.__getstatic:
					ilGenerator.Emit(OpCodes.Call, typeof(ByteCodeHelper).GetMethod("DynamicGetstatic"));
					EmitReturnTypeConversion(ilGenerator, fieldTypeWrapper);
					break;
				case NormalizedByteCode.__putstatic:
					ilGenerator.Emit(OpCodes.Call, typeof(ByteCodeHelper).GetMethod("DynamicPutstatic"));
					break;
			}
			return;
		}
		else
		{
			if(!wrapper.IsAccessibleFrom(clazz))
			{
				throw new IllegalAccessError("Try to access class " + wrapper.Name + " from class " + clazz.Name);
			}
			TypeWrapper thisType = null;
			if(bytecode == NormalizedByteCode.__getfield)
			{
				thisType = SigTypeToClassName(ma.GetRawStackTypeWrapper(i, 0), cpi.GetClassType(classLoader));
			}
			else if(bytecode == NormalizedByteCode.__putfield)
			{
				thisType = SigTypeToClassName(ma.GetRawStackTypeWrapper(i, 1), cpi.GetClassType(classLoader));
			}
			bool isStatic = (bytecode == NormalizedByteCode.__putstatic || bytecode == NormalizedByteCode.__getstatic);
			FieldWrapper field = wrapper.GetFieldWrapper(cpi.Name, cpi.GetFieldType(classLoader));
			if(field != null)
			{
				if(field.IsStatic == isStatic)
				{
					// NOTE this access check is duplicated in ByteCodeHelper.GetFieldWrapper
					if(field.IsPublic ||
						(field.IsProtected && (isStatic ? clazz.IsSubTypeOf(field.DeclaringType) : thisType.IsSubTypeOf(clazz))) ||
						(field.IsPrivate && clazz == field.DeclaringType) ||
						(!(field.IsPublic || field.IsPrivate) && clazz.IsInSamePackageAs(field.DeclaringType)))
					{
						// are we trying to mutate a final field? (they are read-only from outside of the defining class)
						if(write && field.IsFinal && (isStatic ? clazz != wrapper : clazz != thisType))
						{
							throw new IllegalAccessError("Field " + field.DeclaringType.Name + "." + field.Name + " is final");
						}
						else
						{
							if(!write)
							{
								field.EmitGet.Emit(ilGenerator);
								return;
							}
							else
							{
								TypeWrapper tw = field.FieldTypeWrapper;
								TypeWrapper val = ma.GetRawStackTypeWrapper(i, 0);
								if(!tw.IsUnloadable && (val.IsUnloadable || (tw.IsInterfaceOrInterfaceArray && !tw.IsGhost && !val.IsAssignableTo(tw))))
								{
									ilGenerator.Emit(OpCodes.Castclass, tw.TypeAsTBD);
								}
								field.EmitSet.Emit(ilGenerator);
								return;
							}
						}
					}
					else
					{
						throw new IllegalAccessError("Try to access field " + field.DeclaringType.Name + "." + field.Name + " from class " + clazz.Name);
					}
				}
				else
				{
					throw new IncompatibleClassChangeError("Static field access to non-static field (or v.v.)");
				}
			}
			else
			{
				throw new NoSuchFieldError(cpi.Class + "." + cpi.Name);
			}
		}
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

	private class DynamicInvokeEmitter : CodeEmitter
	{
		private ClassLoaderWrapper classLoader;
		private TypeWrapper wrapper;
		private ClassFile.ConstantPoolItemFMI cpi;
		private MethodInfo helperMethod;
		private TypeWrapper retTypeWrapper;

		internal DynamicInvokeEmitter(ClassLoaderWrapper classLoader, TypeWrapper wrapper, ClassFile.ConstantPoolItemFMI cpi, TypeWrapper retTypeWrapper, MethodInfo helperMethod)
		{
			this.classLoader = classLoader;
			this.wrapper = wrapper;
			this.cpi = cpi;
			this.retTypeWrapper = retTypeWrapper;
			this.helperMethod = helperMethod;
		}

		internal override void Emit(ILGenerator ilGenerator)
		{
			TypeWrapper[] args = cpi.GetArgTypes(classLoader);
			LocalBuilder argarray = ilGenerator.DeclareLocal(typeof(object[]));
				LocalBuilder val = ilGenerator.DeclareLocal(typeof(object));
			ilGenerator.Emit(OpCodes.Ldc_I4, args.Length);
			ilGenerator.Emit(OpCodes.Newarr, typeof(object));
			ilGenerator.Emit(OpCodes.Stloc, argarray);
			for(int i = args.Length - 1; i >= 0; i--)
			{
				if(args[i].IsPrimitive)
				{
					ilGenerator.Emit(OpCodes.Box, args[i].TypeAsTBD);
				}
				ilGenerator.Emit(OpCodes.Stloc, val);
				ilGenerator.Emit(OpCodes.Ldloc, argarray);
				ilGenerator.Emit(OpCodes.Ldc_I4, i);
				ilGenerator.Emit(OpCodes.Ldloc, val);
				ilGenerator.Emit(OpCodes.Stelem_Ref);
			}
			ilGenerator.Emit(OpCodes.Ldtoken, wrapper.TypeAsTBD);
			ilGenerator.Emit(OpCodes.Ldstr, cpi.Class);
			ilGenerator.Emit(OpCodes.Ldstr, cpi.Name);
			ilGenerator.Emit(OpCodes.Ldstr, cpi.Signature);
			ilGenerator.Emit(OpCodes.Ldloc, argarray);
			ilGenerator.Emit(OpCodes.Call, helperMethod);
			EmitReturnTypeConversion(ilGenerator, retTypeWrapper);
		}
	}

	private static void EmitReturnTypeConversion(ILGenerator ilgen, TypeWrapper typeWrapper)
	{
		if(typeWrapper.IsUnloadable)
		{
			// nothing to do for unloadables
		}
		else if(typeWrapper == PrimitiveTypeWrapper.VOID)
		{
			ilgen.Emit(OpCodes.Pop);
		}
		else if(typeWrapper.IsPrimitive)
		{
			// NOTE we don't need to use TypeWrapper.EmitUnbox, because the return value cannot be null
			ilgen.Emit(OpCodes.Unbox, typeWrapper.TypeAsTBD);
			ilgen.Emit(OpCodes.Ldobj, typeWrapper.TypeAsTBD);
		}
		else
		{
			ilgen.Emit(OpCodes.Castclass, typeWrapper.TypeAsTBD);
		}
	}

	private void GetMethodCallEmitter(ClassFile.ConstantPoolItemFMI cpi, TypeWrapper thisType, NormalizedByteCode invoke, out CodeEmitter emitNewobj, out CodeEmitter emitCall, out CodeEmitter emitCallvirt)
	{
		TypeWrapper wrapper = cpi.GetClassType(classLoader);
		if(wrapper.IsUnloadable || (thisType != null && thisType.IsUnloadable))
		{
			emitNewobj = new DynamicInvokeEmitter(classLoader, clazz, cpi, cpi.GetClassType(classLoader), typeof(ByteCodeHelper).GetMethod("DynamicInvokeSpecialNew"));
			if(invoke == NormalizedByteCode.__invokestatic)
			{
				emitCall = new DynamicInvokeEmitter(classLoader, clazz, cpi, cpi.GetRetType(classLoader), typeof(ByteCodeHelper).GetMethod("DynamicInvokestatic"));
			}
			else
			{
				// NOTE this shouldn't happen, because invokespecial is only used to call
				// methods in this class or its base classes and those are obviously always loadable.
				emitCall = CodeEmitter.InternalError;
			}
			emitCallvirt = new DynamicInvokeEmitter(classLoader, clazz, cpi, cpi.GetRetType(classLoader), typeof(ByteCodeHelper).GetMethod("DynamicInvokevirtual"));
			return;
		}
		else
		{
			if(!wrapper.IsAccessibleFrom(clazz))
			{
				throw new IllegalAccessError("Try to access class " + wrapper.Name + " from class " + clazz.Name);
			}
			else if(wrapper.IsInterface != (invoke == NormalizedByteCode.__invokeinterface))
			{
				throw new IncompatibleClassChangeError("invokeinterface on non-interface");
			}
			else
			{
				if(invoke == NormalizedByteCode.__invokespecial && m.Method.ClassFile.IsSuper && thisType != wrapper && thisType.IsSubTypeOf(wrapper))
				{
					wrapper = thisType.BaseTypeWrapper;
				}
				MethodDescriptor md = new MethodDescriptor(classLoader, cpi);
				MethodWrapper method = null;
				if(invoke == NormalizedByteCode.__invokeinterface)
				{
					method = GetInterfaceMethod(wrapper, md);
					// NOTE vmspec 5.4.3.4 clearly states that an interfacemethod may also refer to a method in Object
					if(method == null)
					{
						method = java_lang_Object.GetMethodWrapper(md, false);
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
							throw new AbstractMethodError(cpi.Class + "." + cpi.Name + cpi.Signature);
						}
						else if(method.IsPublic ||
							(method.IsProtected && (method.IsStatic ? clazz.IsSubTypeOf(method.DeclaringType) : thisType.IsSubTypeOf(clazz))) ||
							(method.IsPrivate && clazz == method.DeclaringType) ||
							(!(method.IsPublic || method.IsPrivate) && clazz.IsInSamePackageAs(method.DeclaringType)))
						{
							emitNewobj = method.EmitNewobj;
							emitCall = method.EmitCall;
							emitCallvirt = method.EmitCallvirt;
							return;
						}
						else
						{
							// HACK special case for incorrect invocation of Object.clone(), because this could mean
							// we're calling clone() on an array
							// (bug in javac, see http://developer.java.sun.com/developer/bugParade/bugs/4329886.html)
							if(wrapper == java_lang_Object && thisType.IsArray && cpi.Name == "clone")
							{
								method = thisType.GetMethodWrapper(new MethodDescriptor(classLoader, cpi), false);
								if(method != null && method.IsPublic)
								{
									emitNewobj = method.EmitNewobj;
									emitCall = method.EmitCall;
									emitCallvirt = method.EmitCallvirt;
									return;
								}
							}
							throw new IllegalAccessError("Try to access method " + method.DeclaringType.Name + "." + cpi.Name + cpi.Signature + " from class " + clazz.Name);
						}
					}
					else
					{
						throw new IncompatibleClassChangeError("static call to non-static method (or v.v.)");
					}
				}
				else
				{
					throw new NoSuchMethodError(cpi.Class + "." + cpi.Name + cpi.Signature);
				}
			}
		}
	}

	// TODO this method should have a better name
	private TypeWrapper SigTypeToClassName(TypeWrapper type, TypeWrapper nullType)
	{
		if(type == VerifierTypeWrapper.UninitializedThis)
		{
			return clazz;
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
			TypeWrapper t = ma.GetDeclaredLocalTypeWrapper(index);
			if(t != VerifierTypeWrapper.Null)
			{
				type = t.TypeAsLocalOrStackType;
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
		int targetIndex = FindPcIndex(targetPC);
		if(rangeBegin <= targetPC && targetPC < rangeEnd)
		{
			inuse[targetIndex] = true;
			object l = labels[targetIndex];
			if(l == null)
			{
				l = ilGenerator.DefineLabel();
				labels[targetIndex] = l;
			}
			return (Label)l;
		}
		else
		{
			object l = labels[targetIndex];
			if(l == null)
			{
				// if we're branching out of the current exception block, we need to indirect this thru a stub
				// that saves the stack and uses leave to leave the exception block (to another stub that recovers
				// the stack)
				int stackHeight = ma.GetStackHeight(targetIndex);
				BranchCookie bc = new BranchCookie(ilGenerator, stackHeight, targetPC);
				bc.ContentOnStack = true;
				for(int i = 0; i < stackHeight; i++)
				{
					bc.dh.SetType(i, ma.GetRawStackTypeWrapper(targetIndex, i));
				}
				exits.Add(bc);
				l = bc;
				labels[targetIndex] = l;
			}
			return ((BranchCookie)l).Stub;
		}
	}

	private bool IsUnloadable(ClassFile.ConstantPoolItemFMI cpi)
	{
		if(cpi.GetClassType(classLoader).IsUnloadable || cpi.GetRetType(classLoader).IsUnloadable)
		{
			return true;
		}
		TypeWrapper[] args = cpi.GetArgTypes(classLoader);
		for(int i = 0; i < args.Length; i++)
		{
			if(args[i].IsUnloadable)
			{
				return true;
			}
		}
		return false;
	}
}
