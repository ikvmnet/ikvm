/*
  Copyright (C) 2002, 2003, 2004, 2005 Jeroen Frijters

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
#if !COMPACT_FRAMEWORK

using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using IKVM.Runtime;
using IKVM.Attributes;
using IKVM.Internal;

using ILGenerator = IKVM.Internal.CountingILGenerator;

using ExceptionTableEntry = IKVM.Internal.ClassFile.Method.ExceptionTableEntry;
using LocalVariableTableEntry = IKVM.Internal.ClassFile.Method.LocalVariableTableEntry;
using Instruction = IKVM.Internal.ClassFile.Method.Instruction;

class Compiler
{
	private static MethodInfo mapExceptionMethod;
	private static MethodInfo mapExceptionFastMethod;
	private static MethodInfo unmapExceptionMethod;
	private static MethodWrapper initCauseMethod;
	private static MethodInfo suppressFillInStackTraceMethod;
	private static MethodInfo getTypeFromHandleMethod;
	private static MethodInfo getClassFromTypeHandleMethod;
	private static MethodInfo multiANewArrayMethod;
	private static MethodInfo monitorEnterMethod;
	private static MethodInfo monitorExitMethod;
	private static MethodInfo f2iMethod;
	private static MethodInfo d2iMethod;
	private static MethodInfo f2lMethod;
	private static MethodInfo d2lMethod;
	private static MethodInfo arraycopy_fastMethod;
	private static MethodInfo arraycopy_primitive_8Method;
	private static MethodInfo arraycopy_primitive_4Method;
	private static MethodInfo arraycopy_primitive_2Method;
	private static MethodInfo arraycopy_primitive_1Method;
	private static MethodInfo arraycopyMethod;
	private static TypeWrapper java_lang_Object;
	private static TypeWrapper java_lang_Class;
	private static TypeWrapper java_lang_Throwable;
	private static TypeWrapper java_lang_ThreadDeath;
	private static TypeWrapper cli_System_Exception;
	private static Type typeofByteCodeHelper;
	private TypeWrapper clazz;
	private MethodWrapper mw;
	private ClassFile classFile;
	private ClassFile.Method m;
	private ILGenerator ilGenerator;
	private MethodAnalyzer ma;
	private ExceptionTableEntry[] exceptions;
	private ISymbolDocumentWriter symboldocument;
	private LineNumberTableAttribute.LineNumberWriter lineNumbers;
	private bool nonleaf;
	private LocalBuilder[] tempLocals = new LocalBuilder[32];
	private Hashtable invokespecialstubcache;

	static Compiler()
	{
		typeofByteCodeHelper = JVM.LoadType(typeof(ByteCodeHelper));
		getTypeFromHandleMethod = typeof(Type).GetMethod("GetTypeFromHandle", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(RuntimeTypeHandle) }, null);
		getClassFromTypeHandleMethod = typeofByteCodeHelper.GetMethod("GetClassFromTypeHandle");
		multiANewArrayMethod = typeofByteCodeHelper.GetMethod("multianewarray");
		monitorEnterMethod = typeof(System.Threading.Monitor).GetMethod("Enter", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(object) }, null);
		monitorExitMethod = typeof(System.Threading.Monitor).GetMethod("Exit", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(object) }, null);
		f2iMethod = typeofByteCodeHelper.GetMethod("f2i");
		d2iMethod = typeofByteCodeHelper.GetMethod("d2i");
		f2lMethod = typeofByteCodeHelper.GetMethod("f2l");
		d2lMethod = typeofByteCodeHelper.GetMethod("d2l");
		arraycopy_fastMethod = typeofByteCodeHelper.GetMethod("arraycopy_fast");
		arraycopy_primitive_8Method = typeofByteCodeHelper.GetMethod("arraycopy_primitive_8");
		arraycopy_primitive_4Method = typeofByteCodeHelper.GetMethod("arraycopy_primitive_4");
		arraycopy_primitive_2Method = typeofByteCodeHelper.GetMethod("arraycopy_primitive_2");
		arraycopy_primitive_1Method = typeofByteCodeHelper.GetMethod("arraycopy_primitive_1");
		arraycopyMethod = typeofByteCodeHelper.GetMethod("arraycopy");
		java_lang_Throwable = CoreClasses.java.lang.Throwable.Wrapper;
		cli_System_Exception = ClassLoaderWrapper.LoadClassCritical("cli.System.Exception");
		java_lang_Object = CoreClasses.java.lang.Object.Wrapper;
		java_lang_Class = CoreClasses.java.lang.Class.Wrapper;
		java_lang_ThreadDeath = ClassLoaderWrapper.LoadClassCritical("java.lang.ThreadDeath");
		// HACK we need to special case core compilation, because the __<map> methods are HideFromJava
		if(java_lang_Throwable.TypeAsBaseType is TypeBuilder)
		{
			MethodWrapper mw = java_lang_Throwable.GetMethodWrapper("__<map>", "(Ljava.lang.Throwable;Lcli.System.Type;Z)Ljava.lang.Throwable;", false);
			mw.Link();
			mapExceptionMethod = (MethodInfo)mw.GetMethod();
			mw = java_lang_Throwable.GetMethodWrapper("__<map>", "(Ljava.lang.Throwable;Z)Ljava.lang.Throwable;", false);
			mw.Link();
			mapExceptionFastMethod = (MethodInfo)mw.GetMethod();
			mw = java_lang_Throwable.GetMethodWrapper("__<suppressFillInStackTrace>", "()V", false);
			mw.Link();
			suppressFillInStackTraceMethod = (MethodInfo)mw.GetMethod();
			mw = java_lang_Throwable.GetMethodWrapper("__<unmap>", "(Ljava.lang.Throwable;)Ljava.lang.Throwable;", false);
			mw.Link();
			unmapExceptionMethod = (MethodInfo)mw.GetMethod();
		}
		else
		{
			mapExceptionMethod = java_lang_Throwable.TypeAsBaseType.GetMethod("__<map>", new Type[] { typeof(Exception), typeof(Type), typeof(bool) });
			mapExceptionFastMethod = java_lang_Throwable.TypeAsBaseType.GetMethod("__<map>", new Type[] { typeof(Exception), typeof(bool) });
			suppressFillInStackTraceMethod = java_lang_Throwable.TypeAsBaseType.GetMethod("__<suppressFillInStackTrace>", Type.EmptyTypes);
			unmapExceptionMethod = java_lang_Throwable.TypeAsBaseType.GetMethod("__<unmap>", new Type[] { typeof(Exception) });
		}
		initCauseMethod = java_lang_Throwable.GetMethodWrapper("initCause", "(Ljava.lang.Throwable;)Ljava.lang.Throwable;", false);
	}

	private class ExceptionSorter : IComparer
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

	private Compiler(TypeWrapper clazz, MethodWrapper mw, ClassFile classFile, ClassFile.Method m, ILGenerator ilGenerator, ClassLoaderWrapper classLoader, ISymbolDocumentWriter symboldocument, Hashtable invokespecialstubcache)
	{
		this.clazz = clazz;
		this.mw = mw;
		this.classFile = classFile;
		this.m = m;
		this.ilGenerator = ilGenerator;
		this.symboldocument = symboldocument;
		this.invokespecialstubcache = invokespecialstubcache;
		if(m.LineNumberTableAttribute != null && !JVM.NoStackTraceInfo)
		{
			this.lineNumbers = new LineNumberTableAttribute.LineNumberWriter(m.LineNumberTableAttribute.Length);
		}

		Profiler.Enter("MethodAnalyzer");
		try
		{
			ma = new MethodAnalyzer(clazz, mw, classFile, m, classLoader);
		}
		finally
		{
			Profiler.Leave("MethodAnalyzer");
		}

		TypeWrapper[] args = mw.GetParameters();
		LocalVar[] locals = ma.GetAllLocalVars();
		foreach(LocalVar v in locals)
		{
			if(v.isArg)
			{
				int arg = m.ArgMap[v.local];
				TypeWrapper tw;
				if(m.IsStatic)
				{
					tw = args[arg];
				}
				else if(arg == 0)
				{
					tw = clazz;
				}
				else
				{
					tw = args[arg - 1];
				}
				if(!tw.IsUnloadable &&
					v.type != VerifierTypeWrapper.UninitializedThis &&
					(v.type != tw || tw.TypeAsLocalOrStackType != tw.TypeAsSignatureType))
				{
					v.builder = ilGenerator.DeclareLocal(v.type.TypeAsLocalOrStackType);
					if(JVM.Debug && v.name != null)
					{
						v.builder.SetLocalSymInfo(v.name);
					}
					v.isArg = false;
					ilGenerator.Emit(OpCodes.Ldarg_S, (byte)arg);
					tw.EmitConvSignatureTypeToStackType(ilGenerator);
					ilGenerator.Emit(OpCodes.Stloc, v.builder);
				}
			}
		}

		// NOTE we're going to be messing with ExceptionTableEntrys that are owned by the Method, this is very bad practice,
		// this code should probably be changed to use our own ETE class (which should also contain the ordinal, instead
		// of the one in ClassFile.cs)

		ArrayList ar = new ArrayList(m.ExceptionTable);

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
		for(int i = 0; i < ar.Count; i++)
		{
			ExceptionTableEntry ei = (ExceptionTableEntry)ar[i];
			if(ei.start_pc == ei.handler_pc && ei.catch_type == 0)
			{
				int index = FindPcIndex(ei.start_pc);
				if(index + 2 < m.Instructions.Length
					&& FindPcIndex(ei.end_pc) == index + 2
					&& m.Instructions[index].NormalizedOpCode == NormalizedByteCode.__aload
					&& m.Instructions[index + 1].NormalizedOpCode == NormalizedByteCode.__monitorexit
					&& m.Instructions[index + 2].NormalizedOpCode == NormalizedByteCode.__athrow)
				{
					// this is the async exception guard that Jikes and the Eclipse Java Compiler produce
					ar.RemoveAt(i);
					i--;
				}
				else if(index + 4 < m.Instructions.Length
					&& FindPcIndex(ei.end_pc) == index + 3
					&& m.Instructions[index].NormalizedOpCode == NormalizedByteCode.__astore
					&& m.Instructions[index + 1].NormalizedOpCode == NormalizedByteCode.__aload
					&& m.Instructions[index + 2].NormalizedOpCode == NormalizedByteCode.__monitorexit
					&& m.Instructions[index + 3].NormalizedOpCode == NormalizedByteCode.__aload
					&& m.Instructions[index + 4].NormalizedOpCode == NormalizedByteCode.__athrow
					&& m.Instructions[index].NormalizedArg1 == m.Instructions[index + 3].NormalizedArg1)
				{
					// this is the async exception guard that javac produces
					ar.RemoveAt(i);
					i--;
				}
			}
		}

		restart:
			for(int i = 0; i < ar.Count; i++)
			{
				ExceptionTableEntry ei = (ExceptionTableEntry)ar[i];
				for(int j = 0; j < ar.Count; j++)
				{
					ExceptionTableEntry ej = (ExceptionTableEntry)ar[j];
					if(ei.start_pc <= ej.start_pc && ej.start_pc < ei.end_pc)
					{
						// 0006/test.j
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
						// 0007/test.j
						else if(j > i && ej.end_pc < ei.end_pc)
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
						case NormalizedByteCode.__tableswitch:
						case NormalizedByteCode.__lookupswitch:
							// start at -1 to have an opportunity to handle the default offset
							for(int k = -1; k < m.Instructions[j].SwitchEntryCount; k++)
							{
								int targetPC = m.Instructions[j].PC + (k == -1 ? m.Instructions[j].DefaultOffset : m.Instructions[j].GetSwitchTargetOffset(k));
								if(ei.start_pc < targetPC && targetPC < ei.end_pc)
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
						case NormalizedByteCode.__jsr:
						{
							int targetPC = m.Instructions[j].PC + m.Instructions[j].Arg1;
							if(ei.start_pc < targetPC && targetPC < ei.end_pc)
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
				if(ei.start_pc < ej.handler_pc && ej.handler_pc < ei.end_pc)
				{
					ExceptionTableEntry en = new ExceptionTableEntry();
					en.catch_type = ei.catch_type;
					en.handler_pc = ei.handler_pc;
					en.start_pc = ej.handler_pc;
					en.end_pc = ei.end_pc;
					ei.end_pc = ej.handler_pc;
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
					TypeWrapper exceptionType = classFile.GetConstantPoolClassType(ei.catch_type);
					if(!exceptionType.IsUnloadable && !java_lang_ThreadDeath.IsAssignableTo(exceptionType))
					{
						int start = FindPcIndex(ei.start_pc);
						int end = FindPcIndex(ei.end_pc);
						for(int j = start; j < end; j++)
						{
							if(ByteCodeMetaData.CanThrowException(m.Instructions[j].NormalizedOpCode))
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

	private LocalBuilder UnsafeAllocTempLocal(Type type)
	{
		int free = -1;
		for(int i = 0; i < tempLocals.Length; i++)
		{
			LocalBuilder lb = tempLocals[i];
			if(lb == null)
			{
				if(free == -1)
				{
					free = i;
				}
			}
			else if(lb.LocalType == type)
			{
				return lb;
			}
		}
		LocalBuilder lb1 = ilGenerator.DeclareLocal(type);
		if(free != -1)
		{
			tempLocals[free] = lb1;
		}
		return lb1;
	}

	private LocalBuilder AllocTempLocal(Type type)
	{
		for(int i = 0; i < tempLocals.Length; i++)
		{
			LocalBuilder lb = tempLocals[i];
			if(lb != null && lb.LocalType == type)
			{
				tempLocals[i] = null;
				return lb;
			}
		}
		return ilGenerator.DeclareLocal(type);
	}

	private void ReleaseTempLocal(LocalBuilder lb)
	{
		for(int i = 0; i < tempLocals.Length; i++)
		{
			if(tempLocals[i] == null)
			{
				tempLocals[i] = lb;
				break;
			}
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
		internal Label TargetLabel;
		internal bool ContentOnStack;
		internal readonly int TargetPC;
		internal DupHelper dh;

		internal BranchCookie(Compiler compiler, int stackHeight, int targetPC)
		{
			this.Stub = compiler.ilGenerator.DefineLabel();
			this.TargetPC = targetPC;
			this.dh = new DupHelper(compiler, stackHeight);
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
		private Compiler compiler;
		private StackType[] types;
		private LocalBuilder[] locals;

		internal DupHelper(Compiler compiler, int count)
		{
			this.compiler = compiler;
			types = new StackType[count];
			locals = new LocalBuilder[count];
		}

		internal void Release()
		{
			foreach(LocalBuilder lb in locals)
			{
				if(lb != null)
				{
					compiler.ReleaseTempLocal(lb);
				}
			}
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
				locals[i] = compiler.AllocTempLocal(type.TypeAsLocalOrStackType);
			}
		}

		internal void Load(int i)
		{
			switch(types[i])
			{
				case StackType.Null:
					compiler.ilGenerator.Emit(OpCodes.Ldnull);
					break;
				case StackType.New:
					// new objects aren't really there on the stack
					break;
				case StackType.UnitializedThis:
					compiler.ilGenerator.Emit(OpCodes.Ldarg_0);
					break;
				case StackType.Other:
					compiler.ilGenerator.Emit(OpCodes.Ldloc, locals[i]);
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
					compiler.ilGenerator.Emit(OpCodes.Pop);
					break;
				case StackType.New:
					// new objects aren't really there on the stack
					break;
				case StackType.Other:
					compiler.ilGenerator.Emit(OpCodes.Stloc, locals[i]);
					break;
				default:
					throw new InvalidOperationException();
			}
		}
	}

	internal static void Compile(DynamicTypeWrapper clazz, MethodWrapper mw, ClassFile classFile, ClassFile.Method m, ILGenerator ilGenerator, Hashtable invokespecialstubcache)
	{
		bool nonleaf = false;
		Compile(clazz, mw, classFile, m, ilGenerator, ref nonleaf, invokespecialstubcache);
	}

	internal static void Compile(DynamicTypeWrapper clazz, MethodWrapper mw, ClassFile classFile, ClassFile.Method m, ILGenerator ilGenerator, ref bool nonleaf, Hashtable invokespecialstubcache)
	{
		ClassLoaderWrapper classLoader = clazz.GetClassLoader();
		ISymbolDocumentWriter symboldocument = null;
		if(JVM.Debug)
		{
			string sourcefile = classFile.SourceFileAttribute;
			if(sourcefile != null)
			{
				if(JVM.SourcePath != null)
				{
					string package = clazz.Name;
					int index = package.LastIndexOf('.');
					package = index == -1 ? "" : package.Substring(0, index).Replace('.', '/');
					sourcefile = new System.IO.FileInfo(JVM.SourcePath + "/" + package + "/" + sourcefile).FullName;
				}
				symboldocument = classLoader.ModuleBuilder.DefineDocument(sourcefile, SymLanguageType.Java, Guid.Empty, SymDocumentType.Text);
				// the very first instruction in the method must have an associated line number, to be able
				// to step into the method in Visual Studio .NET
				ClassFile.Method.LineNumberTableEntry[] table = m.LineNumberTableAttribute;
				if(table != null)
				{
					int firstPC = int.MaxValue;
					int firstLine = -1;
					for(int i = 0; i < table.Length; i++)
					{
						if(table[i].start_pc < firstPC && table[i].line_number != 0)
						{
							firstLine = table[i].line_number;
							firstPC = table[i].start_pc;
						}
					}
					if(firstLine > 0)
					{
						ilGenerator.MarkSequencePoint(symboldocument, firstLine, 0, firstLine + 1, 0);
						// FXBUG emit an extra nop to workaround Whidbey June CTP dynamic debugging bug
						ilGenerator.Emit(OpCodes.Nop);
					}
				}
			}
		}
		TypeWrapper[] args = mw.GetParameters();
		for(int i = 0; i < args.Length; i++)
		{
			if(args[i].IsUnloadable)
			{
				Profiler.Count("EmitDynamicCast");
				ilGenerator.Emit(OpCodes.Ldarg, (short)(i + (m.IsStatic ? 0 : 1)));
				ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
				ilGenerator.Emit(OpCodes.Ldstr, args[i].Name);
				ilGenerator.Emit(OpCodes.Call, typeofByteCodeHelper.GetMethod("DynamicCast"));
				ilGenerator.Emit(OpCodes.Pop);
			}
		}
		Compiler c;
		try
		{
			Profiler.Enter("new Compiler");
			try
			{
				c = new Compiler(clazz, mw, classFile, m, ilGenerator, classLoader, symboldocument, invokespecialstubcache);
			}
			finally
			{
				Profiler.Leave("new Compiler");
			}
		}
		catch(VerifyError x)
		{
			Tracer.Error(Tracer.Verifier, x.ToString());
			// because in Java the method is only verified if it is actually called,
			// we generate code here to throw the VerificationError
			EmitHelper.Throw(ilGenerator, "java.lang.VerifyError", x.Message);
			return;
		}
		Profiler.Enter("Compile");
		try
		{
			if(m.IsSynchronized && m.IsStatic)
			{
				ilGenerator.Emit(OpCodes.Ldsfld, clazz.ClassObjectField);
				Label label = ilGenerator.DefineLabel();
				ilGenerator.Emit(OpCodes.Brtrue_S, label);
				ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
				ilGenerator.Emit(OpCodes.Call, getClassFromTypeHandleMethod);
				ilGenerator.Emit(OpCodes.Stsfld, clazz.ClassObjectField);
				ilGenerator.MarkLabel(label);
				ilGenerator.Emit(OpCodes.Ldsfld, clazz.ClassObjectField);
				ilGenerator.Emit(OpCodes.Dup);
				LocalBuilder monitor = ilGenerator.DeclareLocal(typeof(object));
				ilGenerator.Emit(OpCodes.Stloc, monitor);
				ilGenerator.Emit(OpCodes.Call, monitorEnterMethod);
				ilGenerator.BeginExceptionBlock();
				Block b = new Block(c, 0, int.MaxValue, -1, new ArrayList(), true);
				c.Compile(b);
				b.Leave();
				ilGenerator.BeginFinallyBlock();
				ilGenerator.Emit(OpCodes.Ldloc, monitor);
				ilGenerator.Emit(OpCodes.Call, monitorExitMethod);
				ilGenerator.EndExceptionBlock();
				b.LeaveStubs(new Block(c, 0, int.MaxValue, -1, null, false));
			}
			else
			{
				Block b = new Block(c, 0, int.MaxValue, -1, null, false);
				c.Compile(b);
				b.Leave();
			}
			if(c.lineNumbers != null)
			{
				AttributeHelper.SetLineNumberTable(mw.GetMethod(), c.lineNumbers);
			}
			// HACK because of the bogus Leave instruction that Reflection.Emit generates, this location
			// sometimes appears reachable (it isn't), so we emit a bogus branch to keep the verifier happy.
			ilGenerator.Emit(OpCodes.Br, - (ilGenerator.GetILOffset() + 5));
			//ilGenerator.Emit(OpCodes.Br_S, (sbyte)-2);
			ilGenerator.Finish();
			nonleaf = c.nonleaf;
		}
		finally
		{
			Profiler.Leave("Compile");
		}
	}

	private class Block
	{
		private Compiler compiler;
		private ILGenerator ilgen;
		private int begin;
		private int end;
		private int exceptionIndex;
		private ArrayList exits;
		private bool nested;
		private object[] labels;

		internal Block(Compiler compiler, int beginPC, int endPC, int exceptionIndex, ArrayList exits, bool nested)
		{
			this.compiler = compiler;
			this.ilgen = compiler.ilGenerator;
			this.begin = beginPC;
			this.end = endPC;
			this.exceptionIndex = exceptionIndex;
			this.exits = exits;
			this.nested = nested;
			labels = new object[compiler.m.Instructions.Length];
		}

		internal int End
		{
			get
			{
				return end;
			}
		}

		internal int ExceptionIndex
		{
			get
			{
				return exceptionIndex;
			}
		}

		internal void SetBackwardBranchLabel(int instructionIndex, BranchCookie bc)
		{
			// NOTE we're overwriting the label that is already there
			labels[instructionIndex] = bc.Stub;
			if(exits == null)
			{
				exits = new ArrayList();
			}
			exits.Add(bc);
		}

		internal Label GetLabel(int targetPC)
		{
			int targetIndex = compiler.FindPcIndex(targetPC);
			if(IsInRange(targetPC))
			{
				object l = labels[targetIndex];
				if(l == null)
				{
					l = ilgen.DefineLabel();
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
					int stackHeight = compiler.ma.GetStackHeight(targetIndex);
					BranchCookie bc = new BranchCookie(compiler, stackHeight, targetPC);
					bc.ContentOnStack = true;
					for(int i = 0; i < stackHeight; i++)
					{
						bc.dh.SetType(i, compiler.ma.GetRawStackTypeWrapper(targetIndex, i));
					}
					exits.Add(bc);
					l = bc;
					labels[targetIndex] = l;
				}
				return ((BranchCookie)l).Stub;
			}
		}

		internal bool HasLabel(int instructionIndex)
		{
			return labels[instructionIndex] != null;
		}

		internal void MarkLabel(int instructionIndex)
		{
			object label = labels[instructionIndex];
			if(label == null)
			{
				Label l = ilgen.DefineLabel();
				ilgen.MarkLabel(l);
				labels[instructionIndex] = l;
			}
			else
			{
				ilgen.MarkLabel((Label)label);
			}
		}

		internal bool IsInRange(int pc)
		{
			return begin <= pc && pc < end;
		}

		internal void Leave()
		{
			if(exits != null)
			{
				for(int i = 0; i < exits.Count; i++)
				{
					object exit = exits[i];
					BranchCookie bc = exit as BranchCookie;
					if(bc != null && bc.ContentOnStack)
					{
						bc.ContentOnStack = false;
						int stack = bc.dh.Count;
						// HACK this is unreachable code, but we make sure that
						// forward pass verification always yields a valid stack
						// (this is required for unreachable leave stubs that are
						// generated for unreachable code that follows an
						// embedded exception emitted by the compiler for invalid
						// code (e.g. NoSuchFieldError))
						for(int n = stack - 1; n >= 0; n--)
						{
							bc.dh.Load(n);
						}
						ilgen.MarkLabel(bc.Stub);
						for(int n = 0; n < stack; n++)
						{
							bc.dh.Store(n);
						}
						if(bc.TargetPC == -1)
						{
							ilgen.Emit(OpCodes.Br, bc.TargetLabel);
						}
						else
						{
							bc.Stub = ilgen.DefineLabel();
							ilgen.Emit(OpCodes.Leave, bc.Stub);
						}
					}
				}
			}
		}

		internal void LeaveStubs(Block newBlock)
		{
			if(exits != null)
			{
				for(int i = 0; i < exits.Count; i++)
				{
					object exit = exits[i];
					ReturnCookie rc = exit as ReturnCookie;
					if(rc != null)
					{
						if(newBlock.IsNested)
						{
							newBlock.exits.Add(rc);
						}
						else
						{
							rc.EmitRet(ilgen);
						}
					}
					else
					{
						BranchCookie bc = exit as BranchCookie;
						if(bc != null && bc.TargetPC != -1)
						{
							Debug.Assert(!bc.ContentOnStack);
							// if the target is within the new block, we handle it, otherwise we
							// defer the cookie to our caller
							if(newBlock.IsInRange(bc.TargetPC))
							{
								bc.ContentOnStack = true;
								ilgen.MarkLabel(bc.Stub);
								int stack = bc.dh.Count;
								for(int n = stack - 1; n >= 0; n--)
								{
									bc.dh.Load(n);
								}
								ilgen.Emit(OpCodes.Br, newBlock.GetLabel(bc.TargetPC));
							}
							else
							{
								newBlock.exits.Add(bc);
							}
						}
					}
				}
			}
		}

		internal void AddExitHack(object bc)
		{
			exits.Add(bc);
		}

		internal bool IsNested
		{
			get
			{
				return nested;
			}
		}
	}

	private bool IsGuardedBlock(Stack blockStack, int instructionIndex, int instructionCount)
	{
		int start_pc = m.Instructions[instructionIndex].PC;
		int end_pc = m.Instructions[instructionIndex + instructionCount].PC;
		for(int i = 0; i < exceptions.Length; i++)
		{
			ExceptionTableEntry e = exceptions[i];
			if(e.end_pc > start_pc && e.start_pc < end_pc)
			{
				foreach(Block block in blockStack)
				{
					if(block.ExceptionIndex == i)
					{
						goto next;
					}
				}
				return true;
			}
		next:;
		}
		return false;
	}

	private void Compile(Block block)
	{
		int[] scope = null;
		// if we're emitting debugging information, we need to use scopes for local variables
		if(JVM.Debug)
		{
			scope = new int[m.Instructions.Length];
			LocalVariableTableEntry[] lvt = m.LocalVariableTableAttribute;
			if(lvt != null)
			{
				for(int i = 0; i < lvt.Length; i++)
				{
					// TODO validate the contents of the LVT entry
					int index = FindPcIndex(lvt[i].start_pc);
					if(index > 0)
					{
						// NOTE javac (correctly) sets start_pc of the LVT entry to the instruction
						// following the store that first initializes the local, so we have to
						// detect that case and adjust our local scope (because we'll be creating
						// the local when we encounter the first store).
						LocalVar v = ma.GetLocalVar(index - 1);
						if(v != null && v.local == lvt[i].index)
						{
							index--;
						}
					}
					scope[index]++;
					int end = lvt[i].start_pc + lvt[i].length;
					if(end == m.Instructions[m.Instructions.Length - 1].PC)
					{
						scope[m.Instructions.Length - 1]--;
					}
					else
					{
						scope[FindPcIndex(end)]--;
					}
				}
			}
		}
		int exceptionIndex = 0;
		Instruction[] code = m.Instructions;
		Stack blockStack = new Stack();
		bool instructionIsForwardReachable = true;
		for(int i = 0; i < code.Length; i++)
		{
			Instruction instr = code[i];

			if(scope != null)
			{
				for(int j = scope[i]; j < 0; j++)
				{
					ilGenerator.EndScope();
				}
				for(int j = scope[i]; j > 0; j--)
				{
					ilGenerator.BeginScope();
				}
			}

			// if we've left the current exception block, do the exit processing
			while(block.End == instr.PC)
			{
				block.Leave();

				ExceptionTableEntry exc = exceptions[block.ExceptionIndex];

				Block prevBlock = block;
				block = (Block)blockStack.Pop();

				exceptionIndex = block.ExceptionIndex + 1;
				// skip over exception handlers that are no longer relevant
				for(; exceptionIndex < exceptions.Length && exceptions[exceptionIndex].end_pc <= instr.PC; exceptionIndex++)
				{
				}

				int handlerIndex = FindPcIndex(exc.handler_pc);

				if(exc.catch_type == 0
					&& handlerIndex + 2 < m.Instructions.Length
					&& m.Instructions[handlerIndex].NormalizedOpCode == NormalizedByteCode.__aload
					&& m.Instructions[handlerIndex + 1].NormalizedOpCode == NormalizedByteCode.__monitorexit
					&& m.Instructions[handlerIndex + 2].NormalizedOpCode == NormalizedByteCode.__athrow
					&& !IsGuardedBlock(blockStack, handlerIndex, 3))
				{
					// this is the Jikes & Eclipse Java Compiler synchronization block exit
					ilGenerator.BeginFaultBlock();
					LoadLocal(m.Instructions[handlerIndex]);
					ilGenerator.Emit(OpCodes.Call, monitorExitMethod);
					ilGenerator.EndExceptionBlock();
					// HACK to keep the verifier happy we need this bogus jump
					// (because of the bogus Leave that Ref.Emit ends the try block with)
					ilGenerator.Emit(OpCodes.Br_S, (sbyte)-2);
				}
				else if(exc.catch_type == 0
					&& handlerIndex + 3 < m.Instructions.Length
					&& m.Instructions[handlerIndex].NormalizedOpCode == NormalizedByteCode.__astore
					&& m.Instructions[handlerIndex + 1].NormalizedOpCode == NormalizedByteCode.__aload
					&& m.Instructions[handlerIndex + 2].NormalizedOpCode == NormalizedByteCode.__monitorexit
					&& m.Instructions[handlerIndex + 3].NormalizedOpCode == NormalizedByteCode.__aload
					&& m.Instructions[handlerIndex + 4].NormalizedOpCode == NormalizedByteCode.__athrow
					&& !IsGuardedBlock(blockStack, handlerIndex, 5))
				{
					// this is the javac synchronization block exit
					ilGenerator.BeginFaultBlock();
					LoadLocal(m.Instructions[handlerIndex + 1]);
					ilGenerator.Emit(OpCodes.Call, monitorExitMethod);
					ilGenerator.EndExceptionBlock();
					// HACK to keep the verifier happy we need this bogus jump
					// (because of the bogus Leave that Ref.Emit ends the try block with)
					ilGenerator.Emit(OpCodes.Br_S, (sbyte)-2);
				}
				else
				{
					TypeWrapper exceptionTypeWrapper;
					bool remap;
					if(exc.catch_type == 0)
					{
						exceptionTypeWrapper = java_lang_Throwable;
						remap = true;
					}
					else
					{
						exceptionTypeWrapper = classFile.GetConstantPoolClassType(exc.catch_type);
						remap = exceptionTypeWrapper.IsUnloadable || !exceptionTypeWrapper.IsSubTypeOf(cli_System_Exception);
					}
					Type excType = exceptionTypeWrapper.TypeAsExceptionType;
					bool mapSafe = !exceptionTypeWrapper.IsUnloadable && !exceptionTypeWrapper.IsMapUnsafeException && !exceptionTypeWrapper.IsRemapped;
					if(mapSafe)
					{
						ilGenerator.BeginCatchBlock(excType);
					}
					else
					{
						ilGenerator.BeginCatchBlock(typeof(Exception));
					}
					BranchCookie bc = new BranchCookie(this, 1, exc.handler_pc);
					prevBlock.AddExitHack(bc);
					Instruction handlerInstr = code[handlerIndex];
					bool unusedException = mapSafe && (handlerInstr.NormalizedOpCode == NormalizedByteCode.__pop ||
						(handlerInstr.NormalizedOpCode == NormalizedByteCode.__astore &&
						ma.GetLocalVar(handlerIndex) == null));
					// special case for catch(Throwable) (and finally), that produces less code and
					// should be faster
					if(mapSafe || exceptionTypeWrapper == java_lang_Throwable)
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
							ilGenerator.Emit(remap ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
							ilGenerator.Emit(OpCodes.Call, mapExceptionFastMethod);
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
							Profiler.Count("EmitDynamicGetTypeAsExceptionType");
							ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
							ilGenerator.Emit(OpCodes.Ldstr, exceptionTypeWrapper.Name);
							ilGenerator.Emit(OpCodes.Call, typeofByteCodeHelper.GetMethod("DynamicGetTypeAsExceptionType"));
							ilGenerator.Emit(remap ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
							ilGenerator.Emit(OpCodes.Call, mapExceptionMethod);
						}
						else
						{
							ilGenerator.Emit(OpCodes.Ldtoken, excType);
							ilGenerator.Emit(OpCodes.Call, getTypeFromHandleMethod);
							ilGenerator.Emit(remap ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
							ilGenerator.Emit(OpCodes.Call, mapExceptionMethod);
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
				prevBlock.LeaveStubs(block);
			}

			if(!instr.IsReachable)
			{
				// skip any unreachable instructions
				continue;
			}

			// if there was a forward branch to this instruction, it is forward reachable
			instructionIsForwardReachable |= block.HasLabel(i);

			if(block.HasLabel(i) || (instr.flags & ClassFile.Method.InstructionFlags.BranchTarget) != 0)
			{
				block.MarkLabel(i);
			}

			// if the instruction is only backward reachable, ECMA says it must have an empty stack,
			// so we move the stack to locals
			if(!instructionIsForwardReachable)
			{
				int stackHeight = ma.GetStackHeight(i);
				if(stackHeight != 0)
				{
					BranchCookie bc = new BranchCookie(this, stackHeight, -1);
					bc.ContentOnStack = true;
					bc.TargetLabel = ilGenerator.DefineLabel();
					ilGenerator.MarkLabel(bc.TargetLabel);
					for(int j = 0; j < stackHeight; j++)
					{
						bc.dh.SetType(j, ma.GetRawStackTypeWrapper(i, j));
					}
					for(int j = stackHeight - 1; j >= 0; j--)
					{
						bc.dh.Load(j);
					}
					block.SetBackwardBranchLabel(i, bc);
				}
			}

			// if we're entering an exception block, we need to setup the exception block and
			// transfer the stack into it
			// Note that an exception block that *starts* at an unreachable instruction,
			// is completely unreachable, because it is impossible to branch into an exception block.
			for(; exceptionIndex < exceptions.Length && exceptions[exceptionIndex].start_pc == instr.PC; exceptionIndex++)
			{
				int stackHeight = ma.GetStackHeight(i);
				if(stackHeight != 0)
				{
					DupHelper dh = new DupHelper(this, stackHeight);
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
					dh.Release();
				}
				else
				{
					ilGenerator.BeginExceptionBlock();
				}
				blockStack.Push(block);
				block = new Block(this, exceptions[exceptionIndex].start_pc, exceptions[exceptionIndex].end_pc, exceptionIndex, new ArrayList(), true);
				block.MarkLabel(i);
			}

			ClassFile.Method.LineNumberTableEntry[] table = m.LineNumberTableAttribute;
			if(table != null && (symboldocument != null || lineNumbers != null))
			{
				for(int j = 0; j < table.Length; j++)
				{
					if(table[j].start_pc == instr.PC && table[j].line_number != 0)
					{
						if(symboldocument != null)
						{
							ilGenerator.MarkSequencePoint(symboldocument, table[j].line_number, 0, table[j].line_number + 1, 0);
							// we emit a nop to make sure we always have an instruction associated with the sequence point
							ilGenerator.Emit(OpCodes.Nop);
						}
						if(lineNumbers != null)
						{
							lineNumbers.AddMapping(ilGenerator.GetILOffset(), table[j].line_number);
						}
						break;
					}
				}
			}

			switch(instr.NormalizedOpCode)
			{
				case NormalizedByteCode.__getstatic:
				case NormalizedByteCode.__getfield:
				{
					ClassFile.ConstantPoolItemFieldref cpi = classFile.GetFieldref(instr.Arg1);
					FieldWrapper field = cpi.GetField();
					field.EmitGet(ilGenerator);
					field.FieldTypeWrapper.EmitConvSignatureTypeToStackType(ilGenerator);
					break;
				}
				case NormalizedByteCode.__putstatic:
				case NormalizedByteCode.__putfield:
				{
					ClassFile.ConstantPoolItemFieldref cpi = classFile.GetFieldref(instr.Arg1);
					FieldWrapper field = cpi.GetField();
					TypeWrapper tw = field.FieldTypeWrapper;
					TypeWrapper val = ma.GetRawStackTypeWrapper(i, 0);
					tw.EmitConvStackTypeToSignatureType(ilGenerator, val);
					field.EmitSet(ilGenerator);
					break;
				}
				case NormalizedByteCode.__dynamic_getstatic:
				case NormalizedByteCode.__dynamic_putstatic:
				case NormalizedByteCode.__dynamic_getfield:
				case NormalizedByteCode.__dynamic_putfield:
					DynamicGetPutField(instr, i);
					break;
				case NormalizedByteCode.__aconst_null:
					ilGenerator.Emit(OpCodes.Ldnull);
					break;
				case NormalizedByteCode.__iconst:
					EmitLdc_I4(instr.NormalizedArg1);
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
					int constant = instr.Arg1;
					switch(classFile.GetConstantPoolConstantType(constant))
					{
						case ClassFile.ConstantType.Double:
							ilGenerator.Emit(OpCodes.Ldc_R8, classFile.GetConstantPoolConstantDouble(constant));
							break;
						case ClassFile.ConstantType.Float:
							ilGenerator.Emit(OpCodes.Ldc_R4, classFile.GetConstantPoolConstantFloat(constant));
							break;
						case ClassFile.ConstantType.Integer:
							EmitLdc_I4(classFile.GetConstantPoolConstantInteger(constant));
							break;
						case ClassFile.ConstantType.Long:
							ilGenerator.Emit(OpCodes.Ldc_I8, classFile.GetConstantPoolConstantLong(constant));
							break;
						case ClassFile.ConstantType.String:
							ilGenerator.Emit(OpCodes.Ldstr, classFile.GetConstantPoolConstantString(constant));
							break;
						case ClassFile.ConstantType.Class:
						{
							TypeWrapper tw = classFile.GetConstantPoolClassType(constant);
							if(tw.IsUnloadable)
							{
								Profiler.Count("EmitDynamicClassLiteral");
								ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
								ilGenerator.Emit(OpCodes.Ldstr, tw.Name);
								ilGenerator.Emit(OpCodes.Call, typeofByteCodeHelper.GetMethod("DynamicClassLiteral"));
							}
							else
							{
								ilGenerator.Emit(OpCodes.Ldtoken, tw.IsRemapped ? tw.TypeAsBaseType : tw.TypeAsTBD);
								ilGenerator.Emit(OpCodes.Call, getClassFromTypeHandleMethod);
							}
							java_lang_Class.EmitCheckcast(clazz, ilGenerator);
							break;
						}
						default:
							throw new InvalidOperationException();
					}
					break;
				}
				case NormalizedByteCode.__dynamic_invokestatic:
				case NormalizedByteCode.__invokestatic:
				{
					ClassFile.ConstantPoolItemMI cpi = classFile.GetMethodref(instr.Arg1);
					// HACK special case for calls to System.arraycopy, if the array arguments on the stack
					// are of a known array type, we can redirect to an optimized version of arraycopy.
					// Note that we also have to handle VMSystem.arraycopy, because StringBuffer directly calls
					// this method to avoid prematurely initialising System.
					if((cpi.Class == "java.lang.System" || cpi.Class == "java.lang.VMSystem")&&
						cpi.Name == "arraycopy" &&
						cpi.Signature == "(Ljava.lang.Object;ILjava.lang.Object;II)V" &&
						cpi.GetClassType().GetClassLoader() == ClassLoaderWrapper.GetBootstrapClassLoader())
					{
						TypeWrapper dst_type = ma.GetRawStackTypeWrapper(i, 2);
						TypeWrapper src_type = ma.GetRawStackTypeWrapper(i, 4);
						if(!dst_type.IsUnloadable && dst_type.IsArray && dst_type == src_type)
						{
							switch(dst_type.Name[1])
							{
								case 'J':
								case 'D':
									ilGenerator.Emit(OpCodes.Call, arraycopy_primitive_8Method);
									break;
								case 'I':
								case 'F':
									ilGenerator.Emit(OpCodes.Call, arraycopy_primitive_4Method);
									break;
								case 'S':
								case 'C':
									ilGenerator.Emit(OpCodes.Call, arraycopy_primitive_2Method);
									break;
								case 'B':
								case 'Z':
									ilGenerator.Emit(OpCodes.Call, arraycopy_primitive_1Method);
									break;
								default:
									// TODO once the verifier tracks actual types (i.e. it knows that
									// a particular reference is the result of a "new" opcode) we can
									// use the fast version if the exact destination type is known
									// (in that case the "dst_type == src_type" above should
									// be changed to "src_type.IsAssignableTo(dst_type)".
									TypeWrapper elemtw = dst_type.ElementTypeWrapper;
									// note that IsFinal returns true for array types, so we have to be careful!
									if(!elemtw.IsArray && elemtw.IsFinal)
									{
										ilGenerator.Emit(OpCodes.Call, arraycopy_fastMethod);
									}
									else
									{
										ilGenerator.Emit(OpCodes.Call, arraycopyMethod);
									}
									break;
							}
							break;
						}
					}
					MethodWrapper method = GetMethodCallEmitter(cpi, instr.NormalizedOpCode);
					// if the stack values don't match the argument types (for interface argument types)
					// we must emit code to cast the stack value to the interface type
					CastInterfaceArgs(method, cpi.GetArgTypes(), i, false);
					method.EmitCall(ilGenerator);
					method.ReturnType.EmitConvSignatureTypeToStackType(ilGenerator);
					nonleaf = true;
					break;
				}
				case NormalizedByteCode.__dynamic_invokeinterface:
				case NormalizedByteCode.__dynamic_invokevirtual:
				case NormalizedByteCode.__invokevirtual:
				case NormalizedByteCode.__invokeinterface:
				case NormalizedByteCode.__invokespecial:
				{
					nonleaf = true;
					ClassFile.ConstantPoolItemMI cpi = classFile.GetMethodref(instr.Arg1);
					int argcount = cpi.GetArgTypes().Length;
					TypeWrapper type = ma.GetRawStackTypeWrapper(i, argcount);
					TypeWrapper thisType = SigTypeToClassName(type, cpi.GetClassType());

					MethodWrapper method = GetMethodCallEmitter(cpi, instr.NormalizedOpCode);

					// if the stack values don't match the argument types (for interface argument types)
					// we must emit code to cast the stack value to the interface type
					if(instr.NormalizedOpCode == NormalizedByteCode.__invokespecial && cpi.Name == "<init>" && VerifierTypeWrapper.IsNew(type))
					{
						TypeWrapper[] args = cpi.GetArgTypes();
						CastInterfaceArgs(method, args, i, false);
					}
					else
					{
						// the this reference is included in the argument list because it may also need to be cast
						TypeWrapper[] methodArgs = cpi.GetArgTypes();
						TypeWrapper[] args = new TypeWrapper[methodArgs.Length + 1];
						methodArgs.CopyTo(args, 1);
						if(instr.NormalizedOpCode == NormalizedByteCode.__invokeinterface)
						{
							args[0] = cpi.GetClassType();
						}
						else
						{
							args[0] = thisType;
						}
						CastInterfaceArgs(method, args, i, true);
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
							for(int j = 0; !nontrivial && j < m.MaxLocals; j++)
							{
								if(ma.GetLocalTypeWrapper(i, j) == type)
								{
									nontrivial = true;
								}
							}
							if(!thisType.IsUnloadable && thisType.IsSubTypeOf(java_lang_Throwable))
							{
								// if the next instruction is an athrow and the exception type
								// doesn't override fillInStackTrace, we can suppress the call
								// to fillInStackTrace from the constructor (and this is
								// a huge perf win)
								// NOTE we also can't call suppressFillInStackTrace for non-Java
								// exceptions (because then the suppress flag won't be cleared),
								// but this case is handled by the "is fillInStackTrace overridden?"
								// test, because cli.System.Exception overrides fillInStackTrace.
								if(code[i + 1].NormalizedOpCode == NormalizedByteCode.__athrow
									&& thisType.GetMethodWrapper("fillInStackTrace", "()Ljava.lang.Throwable;", true).DeclaringType == java_lang_Throwable)
								{
									ilGenerator.Emit(OpCodes.Call, suppressFillInStackTraceMethod);
								}
							}
							method.EmitNewobj(ilGenerator);
							if(!thisType.IsUnloadable && thisType.IsSubTypeOf(cli_System_Exception))
							{
								// HACK we call Throwable.initCause(null) to force creation of an ExceptionInfoHelper
								// (which disables future remapping of the exception) and to prevent others from
								// setting the cause.
								ilGenerator.Emit(OpCodes.Dup);
								ilGenerator.Emit(OpCodes.Ldnull);
								initCauseMethod.EmitCallvirt(ilGenerator);
								ilGenerator.Emit(OpCodes.Pop);
							}
							if(nontrivial)
							{
								// this could be done a little more efficiently, but since in practice this
								// code never runs (for code compiled from Java source) it doesn't
								// really matter
								LocalBuilder newobj = ilGenerator.DeclareLocal(thisType.TypeAsLocalOrStackType);
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
											// NOTE we abuse the newobj local as a cookie to signal null!
											tempstack[j] = newobj;
											ilGenerator.Emit(OpCodes.Pop);
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
										// NOTE we abuse the newobj local as a cookie to signal null!
										if(tempstack[j] == newobj)
										{
											ilGenerator.Emit(OpCodes.Ldnull);
										}
										else
										{
											ilGenerator.Emit(OpCodes.Ldloc, tempstack[j]);
										}
									}
								}
								LocalVar[] locals = ma.GetLocalVarsForInvokeSpecial(i);
								for(int j = 0; j < locals.Length; j++)
								{
									if(locals[j] != null)
									{
										if(locals[j].builder == null)
										{
											// for invokespecial the resulting type can never be null
											locals[j].builder = ilGenerator.DeclareLocal(locals[j].type.TypeAsLocalOrStackType);
										}
										ilGenerator.Emit(OpCodes.Ldloc, newobj);
										ilGenerator.Emit(OpCodes.Stloc, locals[j].builder);
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
							Debug.Assert(type == VerifierTypeWrapper.UninitializedThis);
							method.EmitCall(ilGenerator);
							LocalVar[] locals = ma.GetLocalVarsForInvokeSpecial(i);
							for(int j = 0; j < locals.Length; j++)
							{
								if(locals[j] != null)
								{
									if(locals[j].builder == null)
									{
										// for invokespecial the resulting type can never be null
										locals[j].builder = ilGenerator.DeclareLocal(locals[j].type.TypeAsLocalOrStackType);
									}
									ilGenerator.Emit(OpCodes.Ldarg_0);
									ilGenerator.Emit(OpCodes.Stloc, locals[j].builder);
								}
							}
						}
					}
					else
					{
						if(instr.NormalizedOpCode == NormalizedByteCode.__invokespecial
							&& !method.IsPrivate) // if the method is private, we can get away with a callvirt (and not generate the stub)
						{
							ilGenerator.Emit(OpCodes.Callvirt, GetInvokeSpecialStub(method));
						}
						else
						{
							method.EmitCallvirt(ilGenerator);
						}
						method.ReturnType.EmitConvSignatureTypeToStackType(ilGenerator);
					}
					break;
				}
				case NormalizedByteCode.__clone_array:
					ilGenerator.Emit(OpCodes.Callvirt, ArrayTypeWrapper.CloneMethod);
					break;
				case NormalizedByteCode.__return:
				case NormalizedByteCode.__areturn:
				case NormalizedByteCode.__ireturn:
				case NormalizedByteCode.__lreturn:
				case NormalizedByteCode.__freturn:
				case NormalizedByteCode.__dreturn:
				{
					if(block.IsNested)
					{
						// if we're inside an exception block, copy TOS to local, emit "leave" and push item onto our "todo" list
						LocalBuilder local = null;
						if(instr.NormalizedOpCode != NormalizedByteCode.__return)
						{
							TypeWrapper retTypeWrapper = mw.ReturnType;
							retTypeWrapper.EmitConvStackTypeToSignatureType(ilGenerator, ma.GetRawStackTypeWrapper(i, 0));
							if(ma.GetRawStackTypeWrapper(i, 0).IsUnloadable)
							{
								ilGenerator.Emit(OpCodes.Castclass, retTypeWrapper.TypeAsSignatureType);
							}
							local = UnsafeAllocTempLocal(retTypeWrapper.TypeAsSignatureType);
							ilGenerator.Emit(OpCodes.Stloc, local);
						}
						Label label = ilGenerator.DefineLabel();
						// NOTE leave automatically discards any junk that may be on the stack
						ilGenerator.Emit(OpCodes.Leave, label);
						block.AddExitHack(new ReturnCookie(label, local));
					}
					else
					{
						// if there is junk on the stack (other than the return value), we must pop it off
						// because in .NET this is invalid (unlike in Java)
						int stackHeight = ma.GetStackHeight(i);
						if(instr.NormalizedOpCode == NormalizedByteCode.__return)
						{
							if(stackHeight != 0)
							{
								ilGenerator.Emit(OpCodes.Leave_S, (byte)0);
							}
							ilGenerator.Emit(OpCodes.Ret);
						}
						else
						{
							TypeWrapper retTypeWrapper = mw.ReturnType;
							retTypeWrapper.EmitConvStackTypeToSignatureType(ilGenerator, ma.GetRawStackTypeWrapper(i, 0));
							if(ma.GetRawStackTypeWrapper(i, 0).IsUnloadable)
							{
								ilGenerator.Emit(OpCodes.Castclass, retTypeWrapper.TypeAsSignatureType);
							}
							if(stackHeight != 1)
							{
								LocalBuilder local = AllocTempLocal(retTypeWrapper.TypeAsSignatureType);
								ilGenerator.Emit(OpCodes.Stloc, local);
								ilGenerator.Emit(OpCodes.Leave_S, (byte)0);
								ilGenerator.Emit(OpCodes.Ldloc, local);
								ReleaseTempLocal(local);
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
						// any unitialized this reference has to be loaded from arg 0
						// NOTE if the method overwrites the this references, it will always end up in
						// a different local (due to the way the local variable liveness analysis works),
						// so we don't have to worry about that.
						ilGenerator.Emit(OpCodes.Ldarg_0);
					}
					else
					{
						LocalVar v = LoadLocal(instr);
						if(!type.IsUnloadable && (v.type.IsUnloadable || !v.type.IsAssignableTo(type)))
						{
							type.EmitCheckcast(type, ilGenerator);
						}
					}
					break;
				}
				case NormalizedByteCode.__astore:
				{
					TypeWrapper type = ma.GetRawStackTypeWrapper(i, 0);
					// NOTE we use "int" to track the return address of a jsr
					if(VerifierTypeWrapper.IsRet(type))
					{
						StoreLocal(instr);
					}
					else if(VerifierTypeWrapper.IsNew(type))
					{
						// new objects aren't really on the stack, so we can't copy them into the local
						// (and the local doesn't exist anyway)
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
						StoreLocal(instr);
					}
					break;
				}
				case NormalizedByteCode.__iload:
				case NormalizedByteCode.__lload:
				case NormalizedByteCode.__fload:
				case NormalizedByteCode.__dload:
					LoadLocal(instr);
					break;
				case NormalizedByteCode.__istore:
				case NormalizedByteCode.__lstore:
				case NormalizedByteCode.__fstore:
				case NormalizedByteCode.__dstore:
					StoreLocal(instr);
					break;
				case NormalizedByteCode.__new:
				{
					TypeWrapper wrapper = classFile.GetConstantPoolClassType(instr.Arg1);
					if(wrapper.IsUnloadable)
					{
						Profiler.Count("EmitDynamicNewCheckOnly");
						// this is here to make sure we throw the exception in the right location (before
						// evaluating the constructor arguments)
						ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
						ilGenerator.Emit(OpCodes.Ldstr, wrapper.Name);
						ilGenerator.Emit(OpCodes.Call, typeofByteCodeHelper.GetMethod("DynamicNewCheckOnly"));
					}
					else if(wrapper != clazz)
					{
						// trigger cctor (as the spec requires)
						wrapper.EmitRunClassConstructor(ilGenerator);
					}
					// we don't actually allocate the object here, the call to <init> will be converted into a newobj instruction
					break;
				}
				case NormalizedByteCode.__multianewarray:
				{
					LocalBuilder localArray = UnsafeAllocTempLocal(typeof(int[]));
					LocalBuilder localInt = UnsafeAllocTempLocal(typeof(int));
					EmitLdc_I4(instr.Arg2);
					ilGenerator.Emit(OpCodes.Newarr, typeof(int));
					ilGenerator.Emit(OpCodes.Stloc, localArray);
					for(int j = 1; j <= instr.Arg2; j++)
					{
						ilGenerator.Emit(OpCodes.Stloc, localInt);
						ilGenerator.Emit(OpCodes.Ldloc, localArray);
						EmitLdc_I4(instr.Arg2 - j);
						ilGenerator.Emit(OpCodes.Ldloc, localInt);
						ilGenerator.Emit(OpCodes.Stelem_I4);
					}
					TypeWrapper wrapper = classFile.GetConstantPoolClassType(instr.Arg1);
					if(wrapper.IsUnloadable)
					{
						Profiler.Count("EmitDynamicMultianewarray");
						ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
						ilGenerator.Emit(OpCodes.Ldstr, wrapper.Name);
						ilGenerator.Emit(OpCodes.Ldloc, localArray);
						ilGenerator.Emit(OpCodes.Call, typeofByteCodeHelper.GetMethod("DynamicMultianewarray"));
					}
					else
					{
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
					TypeWrapper wrapper = classFile.GetConstantPoolClassType(instr.Arg1);
					if(wrapper.IsUnloadable)
					{
						Profiler.Count("EmitDynamicNewarray");
						ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
						ilGenerator.Emit(OpCodes.Ldstr, wrapper.Name);
						ilGenerator.Emit(OpCodes.Call, typeofByteCodeHelper.GetMethod("DynamicNewarray"));
					}
					else
					{
						// NOTE for ghost types we create object arrays to make sure that Ghost implementers can be
						// stored in ghost arrays, but this has the unintended consequence that ghost arrays can
						// contain *any* reference type (because they are compiled as Object arrays). We could
						// modify aastore to emit code to check for this, but this would have an huge performance
						// cost for all object arrays.
						// Oddly, while the JVM accepts any reference for any other interface typed references, in the
						// case of aastore it does check that the object actually implements the interface. This
						// is unfortunate, but I think we can live with this minor incompatibility.
						// Note that this does not break type safety, because when the incorrect object is eventually
						// used as the ghost interface type it will generate a ClassCastException.
						ilGenerator.Emit(OpCodes.Newarr, wrapper.TypeAsArrayType);
					}
					break;
				}
				case NormalizedByteCode.__newarray:
				switch(instr.Arg1)
				{
					case 4:
						ilGenerator.Emit(OpCodes.Newarr, PrimitiveTypeWrapper.BOOLEAN.TypeAsArrayType);
						break;
					case 5:
						ilGenerator.Emit(OpCodes.Newarr, PrimitiveTypeWrapper.CHAR.TypeAsArrayType);
						break;
					case 6:
						ilGenerator.Emit(OpCodes.Newarr, PrimitiveTypeWrapper.FLOAT.TypeAsArrayType);
						break;
					case 7:
						ilGenerator.Emit(OpCodes.Newarr, PrimitiveTypeWrapper.DOUBLE.TypeAsArrayType);
						break;
					case 8:
						ilGenerator.Emit(OpCodes.Newarr, PrimitiveTypeWrapper.BYTE.TypeAsArrayType);
						break;
					case 9:
						ilGenerator.Emit(OpCodes.Newarr, PrimitiveTypeWrapper.SHORT.TypeAsArrayType);
						break;
					case 10:
						ilGenerator.Emit(OpCodes.Newarr, PrimitiveTypeWrapper.INT.TypeAsArrayType);
						break;
					case 11:
						ilGenerator.Emit(OpCodes.Newarr, PrimitiveTypeWrapper.LONG.TypeAsArrayType);
						break;
					default:
						// this can't happen, the verifier would have caught it
						throw new InvalidOperationException();
				}
					break;
				case NormalizedByteCode.__checkcast:
				{
					TypeWrapper wrapper = classFile.GetConstantPoolClassType(instr.Arg1);
					wrapper.EmitCheckcast(clazz, ilGenerator);
					break;
				}
				case NormalizedByteCode.__instanceof:
				{
					TypeWrapper wrapper = classFile.GetConstantPoolClassType(instr.Arg1);
					wrapper.EmitInstanceOf(clazz, ilGenerator);
					break;
				}
				case NormalizedByteCode.__aaload:
				{
					TypeWrapper tw = ma.GetRawStackTypeWrapper(i, 1);
					if(tw.IsUnloadable)
					{
						Profiler.Count("EmitDynamicAaload");
						ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
						ilGenerator.Emit(OpCodes.Ldstr, tw.Name);
						ilGenerator.Emit(OpCodes.Call, typeofByteCodeHelper.GetMethod("DynamicAaload"));
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
						Profiler.Count("EmitDynamicAastore");
						ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
						ilGenerator.Emit(OpCodes.Ldstr, tw.Name);
						ilGenerator.Emit(OpCodes.Call, typeofByteCodeHelper.GetMethod("DynamicAastore"));
					}
					else
					{
						TypeWrapper elem = tw.ElementTypeWrapper;
						if(elem.IsNonPrimitiveValueType)
						{
							Type t = elem.TypeAsTBD;
							LocalBuilder local = UnsafeAllocTempLocal(typeof(object));
							ilGenerator.Emit(OpCodes.Stloc, local);
							ilGenerator.Emit(OpCodes.Ldelema, t);
							ilGenerator.Emit(OpCodes.Ldloc, local);
							elem.EmitUnbox(ilGenerator);
							ilGenerator.Emit(OpCodes.Stobj, t);
						}
						else
						{
							// NOTE for verifiability it is expressly *not* required that the
							// value matches the array type, so we don't need to handle interface
							// references here.
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
					LocalBuilder value1 = AllocTempLocal(typeof(long));
					LocalBuilder value2 = AllocTempLocal(typeof(long));
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
					ReleaseTempLocal(value1);
					ReleaseTempLocal(value2);
					break;
				}
				case NormalizedByteCode.__fcmpl:
				{
					LocalBuilder value1 = AllocTempLocal(typeof(float));
					LocalBuilder value2 = AllocTempLocal(typeof(float));
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
					ReleaseTempLocal(value1);
					ReleaseTempLocal(value2);
					break;
				}
				case NormalizedByteCode.__fcmpg:
				{
					LocalBuilder value1 = AllocTempLocal(typeof(float));
					LocalBuilder value2 = AllocTempLocal(typeof(float));
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
					ReleaseTempLocal(value1);
					ReleaseTempLocal(value2);
					break;
				}
				case NormalizedByteCode.__dcmpl:
				{
					LocalBuilder value1 = AllocTempLocal(typeof(double));
					LocalBuilder value2 = AllocTempLocal(typeof(double));
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
					ReleaseTempLocal(value1);
					ReleaseTempLocal(value2);
					break;
				}
				case NormalizedByteCode.__dcmpg:
				{
					LocalBuilder value1 = AllocTempLocal(typeof(double));
					LocalBuilder value2 = AllocTempLocal(typeof(double));
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
					ilGenerator.Emit(OpCodes.Ldc_I4_M1);
					ilGenerator.Emit(OpCodes.Br, end);
					ilGenerator.MarkLabel(res0);
					ilGenerator.Emit(OpCodes.Ldc_I4_0);
					ilGenerator.MarkLabel(end);
					ReleaseTempLocal(value1);
					ReleaseTempLocal(value2);
					break;
				}
				case NormalizedByteCode.__if_icmpeq:
					ilGenerator.Emit(OpCodes.Beq, block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__if_icmpne:
					ilGenerator.Emit(OpCodes.Bne_Un, block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__if_icmple:
					ilGenerator.Emit(OpCodes.Ble, block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__if_icmplt:
					ilGenerator.Emit(OpCodes.Blt, block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__if_icmpge:
					ilGenerator.Emit(OpCodes.Bge, block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__if_icmpgt:
					ilGenerator.Emit(OpCodes.Bgt, block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__ifle:
					ilGenerator.Emit(OpCodes.Ldc_I4_0);
					ilGenerator.Emit(OpCodes.Ble, block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__iflt:
					ilGenerator.Emit(OpCodes.Ldc_I4_0);
					ilGenerator.Emit(OpCodes.Blt, block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__ifge:
					ilGenerator.Emit(OpCodes.Ldc_I4_0);
					ilGenerator.Emit(OpCodes.Bge, block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__ifgt:
					ilGenerator.Emit(OpCodes.Ldc_I4_0);
					ilGenerator.Emit(OpCodes.Bgt, block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__ifne:
					ilGenerator.Emit(OpCodes.Ldc_I4_0);
					ilGenerator.Emit(OpCodes.Bne_Un, block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__ifeq:
					ilGenerator.Emit(OpCodes.Ldc_I4_0);
					ilGenerator.Emit(OpCodes.Beq, block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__ifnonnull:
					ilGenerator.Emit(OpCodes.Brtrue, block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__ifnull:
					ilGenerator.Emit(OpCodes.Brfalse, block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__if_acmpeq:
					ilGenerator.Emit(OpCodes.Beq, block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__if_acmpne:
					ilGenerator.Emit(OpCodes.Bne_Un, block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__goto:
					ilGenerator.Emit(OpCodes.Br, block.GetLabel(instr.PC + instr.Arg1));
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
					EmitLdc_I4(31);
					ilGenerator.Emit(OpCodes.And);
					ilGenerator.Emit(OpCodes.Shl);
					break;
				case NormalizedByteCode.__lshl:
					EmitLdc_I4(63);
					ilGenerator.Emit(OpCodes.And);
					ilGenerator.Emit(OpCodes.Shl);
					break;
				case NormalizedByteCode.__iushr:
					EmitLdc_I4(31);
					ilGenerator.Emit(OpCodes.And);
					ilGenerator.Emit(OpCodes.Shr_Un);
					break;
				case NormalizedByteCode.__lushr:
					EmitLdc_I4(63);
					ilGenerator.Emit(OpCodes.And);
					ilGenerator.Emit(OpCodes.Shr_Un);
					break;
				case NormalizedByteCode.__ishr:
					EmitLdc_I4(31);
					ilGenerator.Emit(OpCodes.And);
					ilGenerator.Emit(OpCodes.Shr);
					break;
				case NormalizedByteCode.__lshr:
					EmitLdc_I4(63);
					ilGenerator.Emit(OpCodes.And);
					ilGenerator.Emit(OpCodes.Shr);
					break;
				case NormalizedByteCode.__swap:
				{
					DupHelper dh = new DupHelper(this, 2);
					dh.SetType(0, ma.GetRawStackTypeWrapper(i, 0));
					dh.SetType(1, ma.GetRawStackTypeWrapper(i, 1));
					dh.Store(0);
					dh.Store(1);
					dh.Load(0);
					dh.Load(1);
					dh.Release();
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
						DupHelper dh = new DupHelper(this, 2);
						dh.SetType(0, type1);
						dh.SetType(1, ma.GetRawStackTypeWrapper(i, 1));
						dh.Store(0);
						dh.Store(1);
						dh.Load(1);
						dh.Load(0);
						dh.Load(1);
						dh.Load(0);
						dh.Release();
					}
					break;
				}
				case NormalizedByteCode.__dup_x1:
				{
					DupHelper dh = new DupHelper(this, 2);
					dh.SetType(0, ma.GetRawStackTypeWrapper(i, 0));
					dh.SetType(1, ma.GetRawStackTypeWrapper(i, 1));
					dh.Store(0);
					dh.Store(1);
					dh.Load(0);
					dh.Load(1);
					dh.Load(0);
					dh.Release();
					break;
				}
				case NormalizedByteCode.__dup2_x1:
				{
					TypeWrapper type1 = ma.GetRawStackTypeWrapper(i, 0);
					if(type1.IsWidePrimitive)
					{
						DupHelper dh = new DupHelper(this, 2);
						dh.SetType(0, type1);
						dh.SetType(1, ma.GetRawStackTypeWrapper(i, 1));
						dh.Store(0);
						dh.Store(1);
						dh.Load(0);
						dh.Load(1);
						dh.Load(0);
						dh.Release();
					}
					else
					{
						DupHelper dh = new DupHelper(this, 3);
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
						dh.Release();
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
							DupHelper dh = new DupHelper(this, 2);
							dh.SetType(0, type1);
							dh.SetType(1, type2);
							dh.Store(0);
							dh.Store(1);
							dh.Load(0);
							dh.Load(1);
							dh.Load(0);
							dh.Release();
						}
						else
						{
							// Form 2
							DupHelper dh = new DupHelper(this, 3);
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
							dh.Release();
						}
					}
					else
					{
						TypeWrapper type3 = ma.GetRawStackTypeWrapper(i, 2);
						if(type3.IsWidePrimitive)
						{
							// Form 3
							DupHelper dh = new DupHelper(this, 3);
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
							dh.Release();
						}
						else
						{
							// Form 1
							DupHelper dh = new DupHelper(this, 4);
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
							dh.Release();
						}
					}
					break;
				}
				case NormalizedByteCode.__dup_x2:
				{
					DupHelper dh = new DupHelper(this, 3);
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
					dh.Release();
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
					// TODO we shouldn't call unmap when we know it isn't needed
					if(ma.GetRawStackTypeWrapper(i, 0).IsUnloadable)
					{
						ilGenerator.Emit(OpCodes.Castclass, typeof(Exception));
					}
					ilGenerator.Emit(OpCodes.Call, unmapExceptionMethod);
					ilGenerator.Emit(OpCodes.Throw);
					break;
				case NormalizedByteCode.__tableswitch:
				{
					// note that a tableswitch always has at least one entry
					// (otherwise it would have failed verification)
					Label[] labels = new Label[instr.SwitchEntryCount];
					for(int j = 0; j < labels.Length; j++)
					{
						labels[j] = block.GetLabel(instr.PC + instr.GetSwitchTargetOffset(j));
					}
					if(instr.GetSwitchValue(0) != 0)
					{
						EmitLdc_I4(instr.GetSwitchValue(0));
						ilGenerator.Emit(OpCodes.Sub);
					}
					ilGenerator.Emit(OpCodes.Switch, labels);
					ilGenerator.Emit(OpCodes.Br, block.GetLabel(instr.PC + instr.DefaultOffset));
					break;
				}
				case NormalizedByteCode.__lookupswitch:
					for(int j = 0; j < instr.SwitchEntryCount; j++)
					{
						ilGenerator.Emit(OpCodes.Dup);
						EmitLdc_I4(instr.GetSwitchValue(j));
						Label label = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Bne_Un_S, label);
						ilGenerator.Emit(OpCodes.Pop);
						ilGenerator.Emit(OpCodes.Br, block.GetLabel(instr.PC + instr.GetSwitchTargetOffset(j)));
						ilGenerator.MarkLabel(label);
					}
					ilGenerator.Emit(OpCodes.Pop);
					ilGenerator.Emit(OpCodes.Br, block.GetLabel(instr.PC + instr.DefaultOffset));
					break;
				case NormalizedByteCode.__iinc:
					LoadLocal(instr);
					EmitLdc_I4(instr.Arg2);
					ilGenerator.Emit(OpCodes.Add);
					StoreLocal(instr);
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
					ilGenerator.Emit(OpCodes.Call, f2iMethod);
					break;
				case NormalizedByteCode.__d2i:
					ilGenerator.Emit(OpCodes.Call, d2iMethod);
					break;
				case NormalizedByteCode.__f2l:
					ilGenerator.Emit(OpCodes.Call, f2lMethod);
					break;
				case NormalizedByteCode.__d2l:
					ilGenerator.Emit(OpCodes.Call, d2lMethod);
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
							EmitLdc_I4(j);
							break;
						}
					}
					ilGenerator.Emit(OpCodes.Br, block.GetLabel(instr.PC + instr.Arg1));
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
						if(m.Instructions[callsites[j]].IsReachable)
						{
							LoadLocal(instr);
							EmitLdc_I4(j);
							ilGenerator.Emit(OpCodes.Beq, block.GetLabel(m.Instructions[callsites[j] + 1].PC));
						}
					}
					if(m.Instructions[callsites[callsites.Length - 1]].IsReachable)
					{
						ilGenerator.Emit(OpCodes.Br, block.GetLabel(m.Instructions[callsites[callsites.Length - 1] + 1].PC));
					}
					break;
				}
				case NormalizedByteCode.__nop:
					ilGenerator.Emit(OpCodes.Nop);
					break;
				case NormalizedByteCode.__static_error:
				{
					TypeWrapper exceptionType;
					switch(instr.HardError)
					{
						case HardError.AbstractMethodError:
							exceptionType = ClassLoaderWrapper.LoadClassCritical("java.lang.AbstractMethodError");
							break;
						case HardError.IllegalAccessError:
							exceptionType = ClassLoaderWrapper.LoadClassCritical("java.lang.IllegalAccessError");
							break;
						case HardError.IncompatibleClassChangeError:
							exceptionType = ClassLoaderWrapper.LoadClassCritical("java.lang.IncompatibleClassChangeError");
							break;
						case HardError.InstantiationError:
							exceptionType = ClassLoaderWrapper.LoadClassCritical("java.lang.InstantiationError");
							break;
						case HardError.LinkageError:
							exceptionType = ClassLoaderWrapper.LoadClassCritical("java.lang.LinkageError");
							break;
						case HardError.NoClassDefFoundError:
							exceptionType = ClassLoaderWrapper.LoadClassCritical("java.lang.NoClassDefFoundError");
							break;
						case HardError.NoSuchFieldError:
							exceptionType = ClassLoaderWrapper.LoadClassCritical("java.lang.NoSuchFieldError");
							break;
						case HardError.NoSuchMethodError:
							exceptionType = ClassLoaderWrapper.LoadClassCritical("java.lang.NoSuchMethodError");
							break;
						default:
							throw new InvalidOperationException();
					}
					string message = ma.GetErrorMessage(instr.HardErrorMessageId);
					Tracer.Error(Tracer.Compiler, "{0}: {1}\n\tat {2}.{3}{4}", exceptionType.Name, message, classFile.Name, m.Name, m.Signature);
					ilGenerator.Emit(OpCodes.Ldstr, message);
					MethodWrapper method = exceptionType.GetMethodWrapper("<init>", "(Ljava.lang.String;)V", false);
					method.Link();
					method.EmitNewobj(ilGenerator);
					ilGenerator.Emit(OpCodes.Throw);
					break;
				}
				default:
					throw new NotImplementedException(instr.NormalizedOpCode.ToString());
			}
			// mark next instruction as inuse
			switch(instr.NormalizedOpCode)
			{
				case NormalizedByteCode.__tableswitch:
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
				case NormalizedByteCode.__static_error:
					instructionIsForwardReachable = false;
					break;
				default:
					instructionIsForwardReachable = true;
					Debug.Assert(m.Instructions[i + 1].IsReachable);
					// don't fall through end of try block
					if(m.Instructions[i + 1].PC == block.End)
					{
						// TODO instead of emitting a branch to the leave stub, it would be more efficient to put the leave stub here
						ilGenerator.Emit(OpCodes.Br, block.GetLabel(m.Instructions[i + 1].PC));
					}
					break;
			}
		}
	}

	private MethodInfo GetInvokeSpecialStub(MethodWrapper method)
	{
		string key = method.DeclaringType.Name + ":" + method.Name + method.Signature;
		MethodInfo mi = (MethodInfo)invokespecialstubcache[key];
		if(mi == null)
		{
			MethodBuilder stub = clazz.TypeAsBuilder.DefineMethod("<>", MethodAttributes.PrivateScope, method.ReturnTypeForDefineMethod, method.GetParametersForDefineMethod());
			ILGenerator ilgen = stub.GetILGenerator();
			ilgen.Emit(OpCodes.Ldarg_0);
			int argc = method.GetParametersForDefineMethod().Length;
			for(int i = 1; i <= argc; i++)
			{
				ilgen.Emit(OpCodes.Ldarg_S, (byte)i);
			}
			method.EmitCall(ilgen);
			ilgen.Emit(OpCodes.Ret);
			invokespecialstubcache[key] = stub;
			mi = stub;
		}
		return mi;
	}

	private void EmitLdc_I4(int v)
	{
		switch(v)
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
				if(v >= -128 && v <= 127)
				{
					ilGenerator.Emit(OpCodes.Ldc_I4_S, (sbyte)v);
				}
				else
				{
					ilGenerator.Emit(OpCodes.Ldc_I4, v);
				}
				break;
		}
	}

	// NOTE despite its name this also handles value type args
	private void CastInterfaceArgs(MethodWrapper method, TypeWrapper[] args, int instructionIndex, bool instanceMethod)
	{
		bool needsCast = false;
		bool dynamic;
		switch(m.Instructions[instructionIndex].NormalizedOpCode)
		{
			case NormalizedByteCode.__dynamic_invokeinterface:
			case NormalizedByteCode.__dynamic_invokestatic:
			case NormalizedByteCode.__dynamic_invokevirtual:
				dynamic = true;
				break;
			default:
				dynamic = false;
				break;
		}

		int firstCastArg = -1;

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
					firstCastArg = i;
					break;
				}
				else if(args[i].IsInterfaceOrInterfaceArray)
				{
					TypeWrapper tw = ma.GetRawStackTypeWrapper(instructionIndex, args.Length - 1 - i);
					if(!tw.IsUnloadable && !tw.IsAssignableTo(args[i]))
					{
						needsCast = true;
						firstCastArg = i;
						break;
					}
				}
				else if(args[i].IsNonPrimitiveValueType)
				{
					if(i == 0 && instanceMethod && method.DeclaringType != args[i])
					{
						// no cast needed because we're calling an inherited method
					}
					else
					{
						needsCast = true;
						firstCastArg = i;
						break;
					}
				}
				// if the stack contains an unloadable, we might need to cast it
				// (e.g. if the argument type is a base class that is loadable)
				if(ma.GetRawStackTypeWrapper(instructionIndex, i).IsUnloadable)
				{
					needsCast = true;
					firstCastArg = i;
					break;
				}
			}
		}

		if(needsCast)
		{
			DupHelper dh = new DupHelper(this, args.Length);
			for(int i = firstCastArg + 1; i < args.Length; i++)
			{
				TypeWrapper tw = ma.GetRawStackTypeWrapper(instructionIndex, args.Length - 1 - i);
				if(tw != VerifierTypeWrapper.UninitializedThis)
				{
					tw = args[i];
				}
				dh.SetType(i, tw);
			}
			for(int i = args.Length - 1; i >= firstCastArg; i--)
			{
				if(!args[i].IsUnloadable && !args[i].IsGhost)
				{
					TypeWrapper tw = ma.GetRawStackTypeWrapper(instructionIndex, args.Length - 1 - i);
					if(tw.IsUnloadable || (args[i].IsInterfaceOrInterfaceArray && !tw.IsAssignableTo(args[i])))
					{
						EmitHelper.EmitAssertType(ilGenerator, args[i].TypeAsTBD);
						Profiler.Count("InterfaceDownCast");
					}
				}
				if(i != firstCastArg)
				{
					dh.Store(i);
				}
			}
			for(int i = firstCastArg; i < args.Length; i++)
			{
				if(i != firstCastArg)
				{
					dh.Load(i);
				}
				if(!args[i].IsUnloadable && args[i].IsGhost && !dynamic)
				{
					if(i == 0 && instanceMethod && !method.DeclaringType.IsInterface)
					{
						// we're calling a java.lang.Object method through a ghost interface reference,
						// no ghost handling is needed
					}
					else
					{
						LocalBuilder ghost = AllocTempLocal(typeof(object));
						ilGenerator.Emit(OpCodes.Stloc, ghost);
						LocalBuilder local = AllocTempLocal(args[i].TypeAsSignatureType);
						ilGenerator.Emit(OpCodes.Ldloca, local);
						ilGenerator.Emit(OpCodes.Ldloc, ghost);
						ilGenerator.Emit(OpCodes.Stfld, args[i].GhostRefField);
						ilGenerator.Emit(OpCodes.Ldloca, local);
						ReleaseTempLocal(local);
						// NOTE when the this argument is a value type, we need the address on the stack instead of the value
						if(i != 0 || !instanceMethod)
						{
							ilGenerator.Emit(OpCodes.Ldobj, args[i].TypeAsSignatureType);
						}
					}
				}
				else
				{
					if(!args[i].IsUnloadable && !dynamic)
					{
						if(args[i].IsNonPrimitiveValueType)
						{
							if(i == 0 && instanceMethod)
							{
								// we only need to unbox if the method was actually declared on the value type
								if(method.DeclaringType == args[i])
								{
									ilGenerator.LazyEmitUnbox(args[i].TypeAsTBD);
								}
							}
							else
							{
								args[i].EmitUnbox(ilGenerator);
							}
						}
						else if(ma.GetRawStackTypeWrapper(instructionIndex, args.Length - 1 - i).IsUnloadable)
						{
							ilGenerator.Emit(OpCodes.Castclass, args[i].TypeAsSignatureType);
						}
					}
				}
			}
			dh.Release();
		}
	}

	private void DynamicGetPutField(Instruction instr, int i)
	{
		NormalizedByteCode bytecode = instr.NormalizedOpCode;
		ClassFile.ConstantPoolItemFieldref cpi = classFile.GetFieldref(instr.Arg1);
		bool write = (bytecode == NormalizedByteCode.__dynamic_putfield || bytecode == NormalizedByteCode.__dynamic_putstatic);
		TypeWrapper wrapper = cpi.GetClassType();
		TypeWrapper fieldTypeWrapper = cpi.GetFieldType();
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
			case NormalizedByteCode.__dynamic_getfield:
				Profiler.Count("EmitDynamicGetfield");
				ilGenerator.Emit(OpCodes.Call, typeofByteCodeHelper.GetMethod("DynamicGetfield"));
				EmitReturnTypeConversion(ilGenerator, fieldTypeWrapper);
				break;
			case NormalizedByteCode.__dynamic_putfield:
				Profiler.Count("EmitDynamicPutfield");
				ilGenerator.Emit(OpCodes.Call, typeofByteCodeHelper.GetMethod("DynamicPutfield"));
				break;
			case NormalizedByteCode.__dynamic_getstatic:
				Profiler.Count("EmitDynamicGetstatic");
				ilGenerator.Emit(OpCodes.Call, typeofByteCodeHelper.GetMethod("DynamicGetstatic"));
				EmitReturnTypeConversion(ilGenerator, fieldTypeWrapper);
				break;
			case NormalizedByteCode.__dynamic_putstatic:
				Profiler.Count("EmitDynamicPutstatic");
				ilGenerator.Emit(OpCodes.Call, typeofByteCodeHelper.GetMethod("DynamicPutstatic"));
				break;
			default:
				throw new InvalidOperationException();
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
			if(typeWrapper == PrimitiveTypeWrapper.BYTE)
			{
				ilgen.Emit(OpCodes.Conv_I1);
			}
		}
		else
		{
			ilgen.Emit(OpCodes.Castclass, typeWrapper.TypeAsTBD);
		}
	}

	private class DynamicMethodWrapper : MethodWrapper
	{
		private static readonly MethodInfo dynamicInvokestatic = typeofByteCodeHelper.GetMethod("DynamicInvokestatic");
		private static readonly MethodInfo dynamicInvokevirtual = typeofByteCodeHelper.GetMethod("DynamicInvokevirtual");
		private static readonly MethodInfo dynamicInvokeSpecialNew = typeofByteCodeHelper.GetMethod("DynamicInvokeSpecialNew");
		private TypeWrapper wrapper;
		private ClassFile.ConstantPoolItemMI cpi;

		internal DynamicMethodWrapper(TypeWrapper wrapper, ClassFile.ConstantPoolItemMI cpi)
			: base(wrapper, cpi.Name, cpi.Signature, null, cpi.GetRetType(), cpi.GetArgTypes(), Modifiers.Public, MemberFlags.None)
		{
			this.wrapper = wrapper;
			this.cpi = cpi;
		}

		internal override void EmitCall(ILGenerator ilgen)
		{
			Emit(dynamicInvokestatic, ilgen, cpi.GetRetType());
		}

		internal override void EmitCallvirt(ILGenerator ilgen)
		{
			Emit(dynamicInvokevirtual, ilgen, cpi.GetRetType());
		}

		internal override void EmitNewobj(ILGenerator ilgen)
		{
			Emit(dynamicInvokeSpecialNew, ilgen, cpi.GetClassType());
		}

		private void Emit(MethodInfo helperMethod, ILGenerator ilGenerator, TypeWrapper retTypeWrapper)
		{
			Profiler.Count("EmitDynamicInvokeEmitter");
			TypeWrapper[] args = cpi.GetArgTypes();
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

	private MethodWrapper GetMethodCallEmitter(ClassFile.ConstantPoolItemMI cpi, NormalizedByteCode invoke)
	{
		switch(invoke)
		{
			case NormalizedByteCode.__invokespecial:
				return cpi.GetMethodForInvokespecial();
			case NormalizedByteCode.__invokeinterface:
			case NormalizedByteCode.__invokestatic:
			case NormalizedByteCode.__invokevirtual:
				return cpi.GetMethod();
			case NormalizedByteCode.__dynamic_invokeinterface:
			case NormalizedByteCode.__dynamic_invokestatic:
			case NormalizedByteCode.__dynamic_invokevirtual:
				return new DynamicMethodWrapper(clazz, cpi);
			default:
				throw new InvalidOperationException();
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

	private LocalVar LoadLocal(ClassFile.Method.Instruction instr)
	{
		LocalVar v = ma.GetLocalVar(FindPcIndex(instr.PC));
		if(v.isArg)
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
						ilGenerator.Emit(OpCodes.Ldarg, (short)i);
					}
					break;
			}
		}
		else if(v.type == VerifierTypeWrapper.Null)
		{
			ilGenerator.Emit(OpCodes.Ldnull);
		}
		else
		{
			if(v.builder == null)
			{
				v.builder = ilGenerator.DeclareLocal(v.type.TypeAsLocalOrStackType);
				if(JVM.Debug && v.name != null)
				{
					v.builder.SetLocalSymInfo(v.name);
				}
			}
			ilGenerator.Emit(OpCodes.Ldloc, v.builder);
		}
		return v;
	}

	private void StoreLocal(ClassFile.Method.Instruction instr)
	{
		LocalVar v = ma.GetLocalVar(FindPcIndex(instr.PC));
		if(v == null)
		{
			// dead store
			ilGenerator.Emit(OpCodes.Pop);
		}
		else if(v.isArg)
		{
			int i = m.ArgMap[instr.NormalizedArg1];
			if(i < 256)
			{
				ilGenerator.Emit(OpCodes.Starg_S, (byte)i);
			}
			else
			{
				ilGenerator.Emit(OpCodes.Starg, (short)i);
			}
		}
		else if(v.type == VerifierTypeWrapper.Null)
		{
			ilGenerator.Emit(OpCodes.Pop);
		}
		else
		{
			if(v.builder == null)
			{
				v.builder = ilGenerator.DeclareLocal(v.type.TypeAsLocalOrStackType);
				if(JVM.Debug && v.name != null)
				{
					v.builder.SetLocalSymInfo(v.name);
				}
			}
			ilGenerator.Emit(OpCodes.Stloc, v.builder);
		}
	}
}

#endif
