/*
  Copyright (C) 2002-2008 Jeroen Frijters

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
using System.Collections.Generic;
using System.Reflection;
#if IKVM_REF_EMIT
using IKVM.Reflection.Emit;
#else
using System.Reflection.Emit;
#endif
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using IKVM.Attributes;
using IKVM.Internal;

using ExceptionTableEntry = IKVM.Internal.ClassFile.Method.ExceptionTableEntry;
using LocalVariableTableEntry = IKVM.Internal.ClassFile.Method.LocalVariableTableEntry;
using Instruction = IKVM.Internal.ClassFile.Method.Instruction;

static class ByteCodeHelperMethods
{
	internal static readonly MethodInfo GetClassFromTypeHandle;
	internal static readonly MethodInfo multianewarray;
	internal static readonly MethodInfo multianewarray_ghost;
	internal static readonly MethodInfo anewarray_ghost;
	internal static readonly MethodInfo f2i;
	internal static readonly MethodInfo d2i;
	internal static readonly MethodInfo f2l;
	internal static readonly MethodInfo d2l;
	internal static readonly MethodInfo arraycopy_fast;
	internal static readonly MethodInfo arraycopy_primitive_8;
	internal static readonly MethodInfo arraycopy_primitive_4;
	internal static readonly MethodInfo arraycopy_primitive_2;
	internal static readonly MethodInfo arraycopy_primitive_1;
	internal static readonly MethodInfo arraycopy;
	internal static readonly MethodInfo DynamicCast;
	internal static readonly MethodInfo DynamicGetTypeAsExceptionType;
	internal static readonly MethodInfo DynamicAaload;
	internal static readonly MethodInfo DynamicAastore;
	internal static readonly MethodInfo DynamicClassLiteral;
	internal static readonly MethodInfo DynamicGetfield;
	internal static readonly MethodInfo DynamicGetstatic;
	internal static readonly MethodInfo DynamicInvokeSpecialNew;
	internal static readonly MethodInfo DynamicInvokestatic;
	internal static readonly MethodInfo DynamicInvokevirtual;
	internal static readonly MethodInfo DynamicMultianewarray;
	internal static readonly MethodInfo DynamicNewarray;
	internal static readonly MethodInfo DynamicNewCheckOnly;
	internal static readonly MethodInfo DynamicPutfield;
	internal static readonly MethodInfo DynamicPutstatic;
	internal static readonly MethodInfo VerboseCastFailure;
	internal static readonly MethodInfo SkipFinalizer;
	internal static readonly MethodInfo DynamicInstanceOf;
	internal static readonly MethodInfo volatileReadDouble;
	internal static readonly MethodInfo volatileReadLong;
	internal static readonly MethodInfo volatileWriteDouble;
	internal static readonly MethodInfo volatileWriteLong;

	static ByteCodeHelperMethods()
	{
#if STATIC_COMPILER
		Type typeofByteCodeHelper = StaticCompiler.GetType("IKVM.Runtime.ByteCodeHelper");
#else
		Type typeofByteCodeHelper = typeof(IKVM.Runtime.ByteCodeHelper);
#endif
		GetClassFromTypeHandle = typeofByteCodeHelper.GetMethod("GetClassFromTypeHandle");
		multianewarray = typeofByteCodeHelper.GetMethod("multianewarray");
		multianewarray_ghost = typeofByteCodeHelper.GetMethod("multianewarray_ghost");
		anewarray_ghost = typeofByteCodeHelper.GetMethod("anewarray_ghost");
		f2i = typeofByteCodeHelper.GetMethod("f2i");
		d2i = typeofByteCodeHelper.GetMethod("d2i");
		f2l = typeofByteCodeHelper.GetMethod("f2l");
		d2l = typeofByteCodeHelper.GetMethod("d2l");
		arraycopy_fast = typeofByteCodeHelper.GetMethod("arraycopy_fast");
		arraycopy_primitive_8 = typeofByteCodeHelper.GetMethod("arraycopy_primitive_8");
		arraycopy_primitive_4 = typeofByteCodeHelper.GetMethod("arraycopy_primitive_4");
		arraycopy_primitive_2 = typeofByteCodeHelper.GetMethod("arraycopy_primitive_2");
		arraycopy_primitive_1 = typeofByteCodeHelper.GetMethod("arraycopy_primitive_1");
		arraycopy = typeofByteCodeHelper.GetMethod("arraycopy");
		DynamicCast = typeofByteCodeHelper.GetMethod("DynamicCast");
		DynamicGetTypeAsExceptionType = typeofByteCodeHelper.GetMethod("DynamicGetTypeAsExceptionType");
		DynamicAaload = typeofByteCodeHelper.GetMethod("DynamicAaload");
		DynamicAastore = typeofByteCodeHelper.GetMethod("DynamicAastore");
		DynamicClassLiteral = typeofByteCodeHelper.GetMethod("DynamicClassLiteral");
		DynamicGetfield = typeofByteCodeHelper.GetMethod("DynamicGetfield");
		DynamicGetstatic = typeofByteCodeHelper.GetMethod("DynamicGetstatic");
		DynamicInvokeSpecialNew = typeofByteCodeHelper.GetMethod("DynamicInvokeSpecialNew");
		DynamicInvokestatic = typeofByteCodeHelper.GetMethod("DynamicInvokestatic");
		DynamicInvokevirtual = typeofByteCodeHelper.GetMethod("DynamicInvokevirtual");
		DynamicMultianewarray = typeofByteCodeHelper.GetMethod("DynamicMultianewarray");
		DynamicNewarray = typeofByteCodeHelper.GetMethod("DynamicNewarray");
		DynamicNewCheckOnly = typeofByteCodeHelper.GetMethod("DynamicNewCheckOnly");
		DynamicPutfield = typeofByteCodeHelper.GetMethod("DynamicPutfield");
		DynamicPutstatic = typeofByteCodeHelper.GetMethod("DynamicPutstatic");
		VerboseCastFailure = typeofByteCodeHelper.GetMethod("VerboseCastFailure");
		SkipFinalizer = typeofByteCodeHelper.GetMethod("SkipFinalizer");
		DynamicInstanceOf = typeofByteCodeHelper.GetMethod("DynamicInstanceOf");
		volatileReadDouble = typeofByteCodeHelper.GetMethod("VolatileRead", new Type[] { Type.GetType("System.Double&") });
		volatileReadLong = typeofByteCodeHelper.GetMethod("VolatileRead", new Type[] { Type.GetType("System.Int64&") });
		volatileWriteDouble = typeofByteCodeHelper.GetMethod("VolatileWrite", new Type[] { Type.GetType("System.Double&"), typeof(double) });
		volatileWriteLong = typeofByteCodeHelper.GetMethod("VolatileWrite", new Type[] { Type.GetType("System.Int64&"), typeof(long) });
	}
}

struct MethodKey : IEquatable<MethodKey>
{
	private readonly string className;
	private readonly string methodName;
	private readonly string methodSig;

	internal MethodKey(string className, string methodName, string methodSig)
	{
		this.className = className;
		this.methodName = methodName;
		this.methodSig = methodSig;
	}

	public bool Equals(MethodKey other)
	{
		return className == other.className && methodName == other.methodName && methodSig == other.methodSig;
	}

	public override int GetHashCode()
	{
		return className.GetHashCode() ^ methodName.GetHashCode() ^ methodSig.GetHashCode();
	}
}

class Compiler
{
	private static MethodInfo mapExceptionMethod;
	internal static MethodInfo mapExceptionFastMethod;
	private static MethodInfo unmapExceptionMethod;
	private static MethodWrapper initCauseMethod;
	private static MethodInfo suppressFillInStackTraceMethod;
	private static MethodInfo getTypeFromHandleMethod;
	private static MethodInfo monitorEnterMethod;
	private static MethodInfo monitorExitMethod;
	private static MethodInfo keepAliveMethod;
	internal static MethodWrapper getClassFromTypeHandle;
	private static TypeWrapper java_lang_Object;
	private static TypeWrapper java_lang_Class;
	private static TypeWrapper java_lang_Throwable;
	private static TypeWrapper java_lang_ThreadDeath;
	private static TypeWrapper cli_System_Object;
	private static TypeWrapper cli_System_Exception;
	private readonly DynamicTypeWrapper.FinishContext context;
	private TypeWrapper clazz;
	private MethodWrapper mw;
	private ClassFile classFile;
	private ClassFile.Method m;
	private CodeEmitter ilGenerator;
	private MethodAnalyzer ma;
	private ExceptionTableEntry[] exceptions;
	private ISymbolDocumentWriter symboldocument;
	private LineNumberTableAttribute.LineNumberWriter lineNumbers;
	private bool nonleaf;
	private Dictionary<MethodKey, MethodInfo> invokespecialstubcache;
	private bool debug;
	private bool keepAlive;
	private bool strictfp;
#if STATIC_COMPILER
	private IKVM.Internal.MapXml.ReplaceMethodCall[] replacedMethods;
	private MethodWrapper[] replacedMethodWrappers;
#endif

	static Compiler()
	{
		getTypeFromHandleMethod = typeof(Type).GetMethod("GetTypeFromHandle", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(RuntimeTypeHandle) }, null);
		monitorEnterMethod = typeof(System.Threading.Monitor).GetMethod("Enter", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(object) }, null);
		monitorExitMethod = typeof(System.Threading.Monitor).GetMethod("Exit", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(object) }, null);
		keepAliveMethod = typeof(System.GC).GetMethod("KeepAlive", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(object) }, null);
		java_lang_Object = CoreClasses.java.lang.Object.Wrapper;
		java_lang_Throwable = CoreClasses.java.lang.Throwable.Wrapper;
		cli_System_Object = DotNetTypeWrapper.GetWrapperFromDotNetType(typeof(System.Object));
		cli_System_Exception = DotNetTypeWrapper.GetWrapperFromDotNetType(typeof(System.Exception));
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
		getClassFromTypeHandle = ClassLoaderWrapper.LoadClassCritical("ikvm.runtime.Util").GetMethodWrapper("getClassFromTypeHandle", "(Lcli.System.RuntimeTypeHandle;)Ljava.lang.Class;", false);
		getClassFromTypeHandle.Link();
	}

	private class ExceptionSorter : IComparer<ExceptionTableEntry>
	{
		public int Compare(ExceptionTableEntry e1, ExceptionTableEntry e2)
		{
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

	private Compiler(DynamicTypeWrapper.FinishContext context, TypeWrapper clazz, MethodWrapper mw, ClassFile classFile, ClassFile.Method m, CodeEmitter ilGenerator, ClassLoaderWrapper classLoader, ISymbolDocumentWriter symboldocument, Dictionary<MethodKey, MethodInfo> invokespecialstubcache)
	{
		this.context = context;
		this.clazz = clazz;
		this.mw = mw;
		this.classFile = classFile;
		this.m = m;
		this.ilGenerator = ilGenerator;
		this.symboldocument = symboldocument;
		this.invokespecialstubcache = invokespecialstubcache;
		this.debug = classLoader.EmitDebugInfo;
		this.strictfp = m.IsStrictfp;
		if(m.LineNumberTableAttribute != null && classLoader.EmitStackTraceInfo)
		{
			this.lineNumbers = new LineNumberTableAttribute.LineNumberWriter(m.LineNumberTableAttribute.Length);
		}
		if(ReferenceEquals(mw.Name, StringConstants.INIT))
		{
			MethodWrapper finalize = clazz.GetMethodWrapper(StringConstants.FINALIZE, StringConstants.SIG_VOID, true);
			keepAlive = finalize != null && finalize.DeclaringType != java_lang_Object && finalize.DeclaringType != cli_System_Object && finalize.DeclaringType != java_lang_Throwable && finalize.DeclaringType != cli_System_Exception;
		}
#if STATIC_COMPILER
		replacedMethods = ((CompilerClassLoader)clazz.GetClassLoader()).GetReplacedMethodsFor(mw);
		if(replacedMethods != null)
		{
			replacedMethodWrappers = new MethodWrapper[replacedMethods.Length];
		}
#endif

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
					if(debug && v.name != null)
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

		List<ExceptionTableEntry> ar = new List<ExceptionTableEntry>(m.ExceptionTable);

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
			ExceptionTableEntry ei = ar[i];
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
				ExceptionTableEntry ei = ar[i];
				for(int j = 0; j < ar.Count; j++)
				{
					ExceptionTableEntry ej = ar[j];
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
				ExceptionTableEntry ei = ar[i];
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
			ExceptionTableEntry ei = ar[i];
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
			ExceptionTableEntry ei = ar[i];
			for(int j = 0; j < ar.Count; j++)
			{
				ExceptionTableEntry ej = ar[j];
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
			ExceptionTableEntry ei = ar[i];
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

		exceptions = ar.ToArray();
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

	private sealed class ReturnCookie
	{
		private CodeEmitterLabel stub;
		private LocalBuilder local;

		internal ReturnCookie(CodeEmitterLabel stub, LocalBuilder local)
		{
			this.stub = stub;
			this.local = local;
		}

		internal void EmitRet(CodeEmitter ilgen)
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
		internal CodeEmitterLabel Stub;
		internal CodeEmitterLabel TargetLabel;
		internal bool ContentOnStack;
		internal readonly int TargetPC;
		internal DupHelper dh;

		internal BranchCookie(Compiler compiler, int stackHeight, int targetPC)
		{
			this.Stub = compiler.ilGenerator.DefineLabel();
			this.TargetPC = targetPC;
			this.dh = new DupHelper(compiler, stackHeight);
		}

		internal BranchCookie(CodeEmitterLabel label, int targetPC)
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
			This,
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
					compiler.ilGenerator.ReleaseTempLocal(lb);
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
			else if(VerifierTypeWrapper.IsThis(type))
			{
				types[i] = StackType.This;
			}
			else if(type == VerifierTypeWrapper.UninitializedThis)
			{
				// uninitialized references cannot be stored in a local, but we can reload them
				types[i] = StackType.UnitializedThis;
			}
			else
			{
				types[i] = StackType.Other;
				locals[i] = compiler.ilGenerator.AllocTempLocal(type.TypeAsLocalOrStackType);
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
				case StackType.This:
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
				case StackType.This:
				case StackType.UnitializedThis:
					compiler.ilGenerator.LazyEmitPop();
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

	internal static void Compile(DynamicTypeWrapper.FinishContext context, DynamicTypeWrapper clazz, MethodWrapper mw, ClassFile classFile, ClassFile.Method m, CodeEmitter ilGenerator, ref bool nonleaf, Dictionary<MethodKey, MethodInfo> invokespecialstubcache, ref LineNumberTableAttribute.LineNumberWriter lineNumberTable)
	{
		ClassLoaderWrapper classLoader = clazz.GetClassLoader();
		ISymbolDocumentWriter symboldocument = null;
		if(classLoader.EmitDebugInfo)
		{
			string sourcefile = classFile.SourceFileAttribute;
			if(sourcefile != null)
			{
				if(classLoader.SourcePath != null)
				{
					string package = clazz.Name;
					int index = package.LastIndexOf('.');
					package = index == -1 ? "" : package.Substring(0, index).Replace('.', '/');
					sourcefile = new System.IO.FileInfo(classLoader.SourcePath + "/" + package + "/" + sourcefile).FullName;
				}
				symboldocument = classLoader.GetTypeWrapperFactory().ModuleBuilder.DefineDocument(sourcefile, SymLanguageType.Java, Guid.Empty, SymDocumentType.Text);
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
				ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicCast);
				ilGenerator.Emit(OpCodes.Pop);
			}
		}
		Compiler c;
		try
		{
			Profiler.Enter("new Compiler");
			try
			{
				c = new Compiler(context, clazz, mw, classFile, m, ilGenerator, classLoader, symboldocument, invokespecialstubcache);
			}
			finally
			{
				Profiler.Leave("new Compiler");
			}
		}
		catch(VerifyError x)
		{
			Tracer.Error(Tracer.Verifier, x.ToString());
			clazz.HasVerifyError = true;
			// because in Java the method is only verified if it is actually called,
			// we generate code here to throw the VerificationError
			EmitHelper.Throw(ilGenerator, "java.lang.VerifyError", x.Message);
			return;
		}
		catch(ClassFormatError x)
		{
			Tracer.Error(Tracer.Verifier, x.ToString());
			clazz.HasClassFormatError = true;
			EmitHelper.Throw(ilGenerator, "java.lang.ClassFormatError", x.Message);
			return;
		}
		Profiler.Enter("Compile");
		try
		{
			if(m.IsSynchronized && m.IsStatic)
			{
				ilGenerator.Emit(OpCodes.Ldsfld, context.ClassObjectField);
				CodeEmitterLabel label = ilGenerator.DefineLabel();
				ilGenerator.Emit(OpCodes.Brtrue_S, label);
				ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
				getClassFromTypeHandle.EmitCall(ilGenerator);
				ilGenerator.Emit(OpCodes.Stsfld, context.ClassObjectField);
				ilGenerator.MarkLabel(label);
				ilGenerator.Emit(OpCodes.Ldsfld, context.ClassObjectField);
				ilGenerator.Emit(OpCodes.Dup);
				LocalBuilder monitor = ilGenerator.DeclareLocal(typeof(object));
				ilGenerator.Emit(OpCodes.Stloc, monitor);
				ilGenerator.Emit(OpCodes.Call, monitorEnterMethod);
				ilGenerator.BeginExceptionBlock();
				Block b = new Block(c, 0, int.MaxValue, -1, new List<object>(), true);
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
				for(int i = 0; i < m.Instructions.Length; i++)
				{
					if(!m.Instructions[i].IsReachable)
					{
						// skip unreachable instructions
					}
					else if(m.Instructions[i].NormalizedOpCode == NormalizedByteCode.__getfield
						&& VerifierTypeWrapper.IsThis(c.ma.GetRawStackTypeWrapper(i, 0)))
					{
						// loading a field from the current object cannot throw
					}
					else if(m.Instructions[i].NormalizedOpCode == NormalizedByteCode.__putfield
						&& VerifierTypeWrapper.IsThis(c.ma.GetRawStackTypeWrapper(i, 1)))
					{
						// storing a field in the current object cannot throw
					}
					else if(m.Instructions[i].NormalizedOpCode == NormalizedByteCode.__getstatic
						&& classFile.GetFieldref(m.Instructions[i].Arg1).GetClassType() == clazz)
					{
						// loading a field from the current class cannot throw
					}
					else if(m.Instructions[i].NormalizedOpCode == NormalizedByteCode.__putstatic
						&& classFile.GetFieldref(m.Instructions[i].Arg1).GetClassType() == clazz)
					{
						// storing a field to the current class cannot throw
					}
					else if(ByteCodeMetaData.CanThrowException(m.Instructions[i].NormalizedOpCode))
					{
						lineNumberTable = c.lineNumbers;
						break;
					}
				}
			}
			if((m.IsSynchronized && m.IsStatic) || c.exceptions.Length > 0)
			{
				// HACK because of the bogus Leave instruction that Reflection.Emit generates, this location
				// sometimes appears reachable (it isn't), so we emit a bogus branch to keep the verifier happy.
				//ilGenerator.Emit(OpCodes.Br, - (ilGenerator.GetILOffset() + 5));
				ilGenerator.Emit(OpCodes.Br_S, (sbyte)-2);
			}
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
		private CodeEmitter ilgen;
		private int begin;
		private int end;
		private int exceptionIndex;
		private List<object> exits;
		private bool nested;
		private object[] labels;

		internal Block(Compiler compiler, int beginPC, int endPC, int exceptionIndex, List<object> exits, bool nested)
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
				exits = new List<object>();
			}
			exits.Add(bc);
		}

		internal CodeEmitterLabel GetLabel(int targetPC)
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
				return (CodeEmitterLabel)l;
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
				CodeEmitterLabel l = ilgen.DefineLabel();
				ilgen.MarkLabel(l);
				labels[instructionIndex] = l;
			}
			else
			{
				ilgen.MarkLabel((CodeEmitterLabel)label);
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

	private bool IsGuardedBlock(Stack<Block> blockStack, int instructionIndex, int instructionCount)
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
		if(debug)
		{
			scope = new int[m.Instructions.Length];
			LocalVariableTableEntry[] lvt = m.LocalVariableTableAttribute;
			if(lvt != null)
			{
				for(int i = 0; i < lvt.Length; i++)
				{
					// TODO validate the contents of the LVT entry
					int startIndex = SafeFindPcIndex(lvt[i].start_pc);
					if(startIndex > 0)
					{
						// NOTE javac (correctly) sets start_pc of the LVT entry to the instruction
						// following the store that first initializes the local, so we have to
						// detect that case and adjust our local scope (because we'll be creating
						// the local when we encounter the first store).
						LocalVar v = ma.GetLocalVar(startIndex - 1);
						if(v != null && v.local == lvt[i].index)
						{
							startIndex--;
						}
					}
					int end = lvt[i].start_pc + lvt[i].length;
					int endIndex;
					if(end == m.Instructions[m.Instructions.Length - 1].PC)
					{
						endIndex = m.Instructions.Length - 1;
					}
					else
					{
						endIndex = SafeFindPcIndex(end);
					}
					if(startIndex != -1 && endIndex != -1)
					{
						scope[startIndex]++;
						scope[endIndex]--;
					}
					else
					{
						// the LVT range is invalid, but we need to have a scope for the variable,
						// so we create an artificial scope that spans the method
						scope[0]++;
						scope[m.Instructions.Length - 1]--;
					}
				}
			}
		}
		int exceptionIndex = 0;
		Instruction[] code = m.Instructions;
		Stack<Block> blockStack = new Stack<Block>();
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
				block = blockStack.Pop();

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
							ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicGetTypeAsExceptionType);
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
						CodeEmitterLabel rethrow = ilGenerator.DefineLabel();
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

			if(block.HasLabel(i) || instr.IsBranchTarget)
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
				block = new Block(this, exceptions[exceptionIndex].start_pc, exceptions[exceptionIndex].end_pc, exceptionIndex, new List<object>(), true);
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
						// we only add a line number mapping if the stack is empty for two reasons:
						// 1) the CLR JIT only generates native to IL mappings for locations where the stack is empty
						// 2) GetILOffset() flushes the lazy emit stack, so if we don't do this check we miss some optimization opportunities
						if(lineNumbers != null && ilGenerator.IsStackEmpty)
						{
							lineNumbers.AddMapping(ilGenerator.GetILOffset(), table[j].line_number);
						}
						break;
					}
				}
			}

			if(keepAlive)
			{
				// JSR 133 specifies that a finalizer cannot run while the constructor is still in progress.
				// This code attempts to implement that by adding calls to GC.KeepAlive(this) before return,
				// backward branches and throw instructions. I don't think it is perfect, you may be able to
				// fool it by calling a trivial method that loops forever which the CLR JIT will then inline
				// and see that control flow doesn't continue and hence the lifetime of "this" will be
				// shorter than the constructor.
				switch(instr.NormalizedOpCode)
				{
					case NormalizedByteCode.__return:
					case NormalizedByteCode.__areturn:
					case NormalizedByteCode.__ireturn:
					case NormalizedByteCode.__lreturn:
					case NormalizedByteCode.__freturn:
					case NormalizedByteCode.__dreturn:
						ilGenerator.Emit(OpCodes.Ldarg_0);
						ilGenerator.Emit(OpCodes.Call, keepAliveMethod);
						break;
					case NormalizedByteCode.__if_icmpeq:
					case NormalizedByteCode.__if_icmpne:
					case NormalizedByteCode.__if_icmple:
					case NormalizedByteCode.__if_icmplt:
					case NormalizedByteCode.__if_icmpge:
					case NormalizedByteCode.__if_icmpgt:
					case NormalizedByteCode.__ifle:
					case NormalizedByteCode.__iflt:
					case NormalizedByteCode.__ifge:
					case NormalizedByteCode.__ifgt:
					case NormalizedByteCode.__ifne:
					case NormalizedByteCode.__ifeq:
					case NormalizedByteCode.__ifnonnull:
					case NormalizedByteCode.__ifnull:
					case NormalizedByteCode.__if_acmpeq:
					case NormalizedByteCode.__if_acmpne:
					case NormalizedByteCode.__goto:
						if(instr.Arg1 <= 0)
						{
							ilGenerator.Emit(OpCodes.Ldarg_0);
							ilGenerator.Emit(OpCodes.Call, keepAliveMethod);
						}
						break;
					case NormalizedByteCode.__athrow:
					case NormalizedByteCode.__athrow_no_unmap:
					case NormalizedByteCode.__lookupswitch:
					case NormalizedByteCode.__tableswitch:
						if(ma.GetLocalTypeWrapper(i, 0) != VerifierTypeWrapper.UninitializedThis)
						{
							ilGenerator.Emit(OpCodes.Ldarg_0);
							ilGenerator.Emit(OpCodes.Call, keepAliveMethod);
						}
						break;
				}
			}

			switch(instr.NormalizedOpCode)
			{
				case NormalizedByteCode.__getstatic:
				{
					ClassFile.ConstantPoolItemFieldref cpi = classFile.GetFieldref(instr.Arg1);
					if(cpi.GetClassType() != clazz)
					{
						// we may trigger a static initializer, which is equivalent to a call
						nonleaf = true;
					}
					FieldWrapper field = cpi.GetField();
					field.EmitGet(ilGenerator);
					field.FieldTypeWrapper.EmitConvSignatureTypeToStackType(ilGenerator);
					break;
				}
				case NormalizedByteCode.__getfield:
				{
					ClassFile.ConstantPoolItemFieldref cpi = classFile.GetFieldref(instr.Arg1);
					FieldWrapper field = cpi.GetField();
					field.EmitGet(ilGenerator);
					field.FieldTypeWrapper.EmitConvSignatureTypeToStackType(ilGenerator);
					break;
				}
				case NormalizedByteCode.__putstatic:
				{
					ClassFile.ConstantPoolItemFieldref cpi = classFile.GetFieldref(instr.Arg1);
					if(cpi.GetClassType() != clazz)
					{
						// we may trigger a static initializer, which is equivalent to a call
						nonleaf = true;
					}
					FieldWrapper field = cpi.GetField();
					TypeWrapper tw = field.FieldTypeWrapper;
					tw.EmitConvStackTypeToSignatureType(ilGenerator, ma.GetStackTypeWrapper(i, 0));
					if(strictfp)
					{
						// no need to convert
					}
					else if(tw == PrimitiveTypeWrapper.DOUBLE)
					{
						ilGenerator.Emit(OpCodes.Conv_R8);
					}
					field.EmitSet(ilGenerator);
					break;
				}
				case NormalizedByteCode.__putfield:
				{
					ClassFile.ConstantPoolItemFieldref cpi = classFile.GetFieldref(instr.Arg1);
					FieldWrapper field = cpi.GetField();
					TypeWrapper tw = field.FieldTypeWrapper;
					tw.EmitConvStackTypeToSignatureType(ilGenerator, ma.GetStackTypeWrapper(i, 0));
					if(strictfp)
					{
						// no need to convert
					}
					else if(tw == PrimitiveTypeWrapper.DOUBLE)
					{
						ilGenerator.Emit(OpCodes.Conv_R8);
					}
					field.EmitSet(ilGenerator);
					break;
				}
				case NormalizedByteCode.__dynamic_getstatic:
				case NormalizedByteCode.__dynamic_putstatic:
				case NormalizedByteCode.__dynamic_getfield:
				case NormalizedByteCode.__dynamic_putfield:
					nonleaf = true;
					DynamicGetPutField(instr, i);
					break;
				case NormalizedByteCode.__aconst_null:
					ilGenerator.LazyEmitLdnull();
					break;
				case NormalizedByteCode.__iconst:
					ilGenerator.LazyEmitLdc_I4(instr.NormalizedArg1);
					break;
				case NormalizedByteCode.__lconst_0:
					ilGenerator.LazyEmitLdc_I8(0);
					break;
				case NormalizedByteCode.__lconst_1:
					ilGenerator.LazyEmitLdc_I8(1);
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
						{
							double v = classFile.GetConstantPoolConstantDouble(constant);
							if(v == 0.0 && BitConverter.DoubleToInt64Bits(v) < 0)
							{
								// FXBUG the x64 CLR JIT has a bug [1] that causes "cond ? -0:0 : 0.0" to be optimized to 0.0
								// This bug causes problems for the sun.misc.FloatingDecimal code, so as a workaround we obfuscate the -0.0 constant.
								// [1] https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=276714
								ilGenerator.Emit(OpCodes.Ldc_I8, Int64.MinValue);
								ilGenerator.Emit(OpCodes.Call, typeof(BitConverter).GetMethod("Int64BitsToDouble"));
							}
							else
							{
								ilGenerator.Emit(OpCodes.Ldc_R8, v);
							}
							break;
						}
						case ClassFile.ConstantType.Float:
						{
							float v = classFile.GetConstantPoolConstantFloat(constant);
							if(v == 0.0 && BitConverter.DoubleToInt64Bits(v) < 0)
							{
								// FXBUG the x64 CLR JIT has a bug [1] that causes "cond ? -0:0 : 0.0" to be optimized to 0.0
								// This bug causes problems for the sun.misc.FloatingDecimal code, so as a workaround we obfuscate the -0.0 constant.
								// [1] https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=276714
								ilGenerator.Emit(OpCodes.Ldc_I8, Int64.MinValue);
								ilGenerator.Emit(OpCodes.Call, typeof(BitConverter).GetMethod("Int64BitsToDouble"));
							}
							else
							{
								ilGenerator.Emit(OpCodes.Ldc_R4, v);
							}
							break;
						}
						case ClassFile.ConstantType.Integer:
							ilGenerator.LazyEmitLdc_I4(classFile.GetConstantPoolConstantInteger(constant));
							break;
						case ClassFile.ConstantType.Long:
							ilGenerator.LazyEmitLdc_I8(classFile.GetConstantPoolConstantLong(constant));
							break;
						case ClassFile.ConstantType.String:
							ilGenerator.LazyEmitLdstr(classFile.GetConstantPoolConstantString(constant));
							break;
						case ClassFile.ConstantType.Class:
						{
							TypeWrapper tw = classFile.GetConstantPoolClassType(constant);
							if(tw.IsUnloadable)
							{
								Profiler.Count("EmitDynamicClassLiteral");
								ilGenerator.Emit(OpCodes.Ldtoken, clazz.TypeAsTBD);
								ilGenerator.Emit(OpCodes.Ldstr, tw.Name);
								ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicClassLiteral);
								java_lang_Class.EmitCheckcast(clazz, ilGenerator);
							}
							else if(tw.IsGhostArray)
							{
								int rank = tw.ArrayRank;
								while(tw.IsArray)
								{
									tw = tw.ElementTypeWrapper;
								}
								ilGenerator.LazyEmitLoadClass(ArrayTypeWrapper.MakeArrayType(tw.TypeAsTBD, rank));
							}
							else
							{
								ilGenerator.LazyEmitLoadClass(tw.IsRemapped ? tw.TypeAsBaseType : tw.TypeAsTBD);
							}
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
					MethodWrapper method = GetMethodCallEmitter(cpi, instr.NormalizedOpCode);
					if(method.IsIntrinsic && Intrinsics.Emit(context, ilGenerator, method, ma, i, mw, classFile, code))
					{
						break;
					}
					// if the stack values don't match the argument types (for interface argument types)
					// we must emit code to cast the stack value to the interface type
					CastInterfaceArgs(method, cpi.GetArgTypes(), i, false);
					if(method.HasCallerID)
					{
						ilGenerator.Emit(OpCodes.Ldsfld, context.CallerIDField);
					}
					method.EmitCall(ilGenerator);
					method.ReturnType.EmitConvSignatureTypeToStackType(ilGenerator);
					if(!strictfp)
					{
						// no need to convert
					}
					else if(method.ReturnType == PrimitiveTypeWrapper.DOUBLE)
					{
						ilGenerator.Emit(OpCodes.Conv_R8);
					}
					else if(method.ReturnType == PrimitiveTypeWrapper.FLOAT)
					{
						ilGenerator.Emit(OpCodes.Conv_R4);
					}
					nonleaf = true;
					break;
				}
				case NormalizedByteCode.__dynamic_invokeinterface:
				case NormalizedByteCode.__dynamic_invokevirtual:
				case NormalizedByteCode.__dynamic_invokespecial:
				case NormalizedByteCode.__invokevirtual:
				case NormalizedByteCode.__invokeinterface:
				case NormalizedByteCode.__invokespecial:
				{
					bool isinvokespecial = instr.NormalizedOpCode == NormalizedByteCode.__invokespecial || instr.NormalizedOpCode == NormalizedByteCode.__dynamic_invokespecial;
					nonleaf = true;
					ClassFile.ConstantPoolItemMI cpi = classFile.GetMethodref(instr.Arg1);
					int argcount = cpi.GetArgTypes().Length;
					TypeWrapper type = ma.GetRawStackTypeWrapper(i, argcount);
					TypeWrapper thisType = SigTypeToClassName(type, cpi.GetClassType());

					MethodWrapper method = GetMethodCallEmitter(cpi, instr.NormalizedOpCode);

					if(method.IsIntrinsic && Intrinsics.Emit(context, ilGenerator, method, ma, i, mw, classFile, code))
					{
						break;
					}

					if(method.IsProtected && (method.DeclaringType == java_lang_Object || method.DeclaringType == java_lang_Throwable))
					{
						// HACK we may need to redirect finalize or clone from java.lang.Object/Throwable
						// to a more specific base type.
						if(thisType.IsAssignableTo(cli_System_Object))
						{
							method = cli_System_Object.GetMethodWrapper(cpi.Name, cpi.Signature, true);
						}
						else if(thisType.IsAssignableTo(cli_System_Exception))
						{
							method = cli_System_Exception.GetMethodWrapper(cpi.Name, cpi.Signature, true);
						}
						else if(thisType.IsAssignableTo(java_lang_Throwable))
						{
							method = java_lang_Throwable.GetMethodWrapper(cpi.Name, cpi.Signature, true);
						}
					}

					// if the stack values don't match the argument types (for interface argument types)
					// we must emit code to cast the stack value to the interface type
					if(isinvokespecial && ReferenceEquals(cpi.Name, StringConstants.INIT) && VerifierTypeWrapper.IsNew(type))
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
							if(method.DeclaringType.IsGhost)
							{
								// if we're calling a ghost interface method, we need to make sure that CastInterfaceArgs knows
								// (cpi.GetClassType() could be an interface that extends the ghost interface)
								args[0] = method.DeclaringType;
							}
							else
							{
								args[0] = cpi.GetClassType();
							}
						}
						else
						{
							args[0] = thisType;
						}
						CastInterfaceArgs(method, args, i, true);
					}

					if(isinvokespecial && ReferenceEquals(cpi.Name, StringConstants.INIT))
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
									if(!code[i + 1].IsBranchTarget)
									{
										code[i + 1].PatchOpCode(NormalizedByteCode.__athrow_no_unmap);
									}
								}
							}
							method.EmitNewobj(ilGenerator, ma, i);
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
										TypeWrapper stacktype = ma.GetStackTypeWrapper(i, argcount + 1 + j);
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
						if(method.HasCallerID)
						{
							ilGenerator.Emit(OpCodes.Ldsfld, context.CallerIDField);
						}

						if(isinvokespecial)
						{
							if(VerifierTypeWrapper.IsThis(type))
							{
								method.EmitCall(ilGenerator);
							}
							else if(method.IsPrivate)
							{
								// if the method is private, we can get away with a callvirt (and not generate the stub)
								method.EmitCallvirt(ilGenerator);
							}
							else
							{
								ilGenerator.Emit(OpCodes.Callvirt, GetInvokeSpecialStub(method));
							}
						}
						else
						{
							// NOTE this check is written somewhat pessimistically, because we need to
							// take remapped types into account. For example, Throwable.getClass() "overrides"
							// the final Object.getClass() method and we don't want to call Object.getClass()
							// on a Throwable instance, because that would yield unverifiable code (java.lang.Throwable
							// extends System.Exception instead of java.lang.Object in the .NET type system).
							if(VerifierTypeWrapper.IsThis(type)
								&& (method.IsFinal || clazz.IsFinal)
								&& clazz.GetMethodWrapper(method.Name, method.Signature, true) == method)
							{
								// we're calling a method on our own instance that can't possibly be overriden,
								// so we don't need to use callvirt
								method.EmitCall(ilGenerator);
							}
							else
							{
								method.EmitCallvirt(ilGenerator);
							}
						}
						method.ReturnType.EmitConvSignatureTypeToStackType(ilGenerator);
						if(!strictfp)
						{
							// no need to convert
						}
						else if(method.ReturnType == PrimitiveTypeWrapper.DOUBLE)
						{
							ilGenerator.Emit(OpCodes.Conv_R8);
						}
						else if(method.ReturnType == PrimitiveTypeWrapper.FLOAT)
						{
							ilGenerator.Emit(OpCodes.Conv_R4);
						}
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
							retTypeWrapper.EmitConvStackTypeToSignatureType(ilGenerator, ma.GetStackTypeWrapper(i, 0));
							local = ilGenerator.UnsafeAllocTempLocal(retTypeWrapper.TypeAsSignatureType);
							ilGenerator.Emit(OpCodes.Stloc, local);
						}
						CodeEmitterLabel label = ilGenerator.DefineLabel();
						// NOTE leave automatically discards any junk that may be on the stack
						ilGenerator.Emit(OpCodes.Leave, label);
						block.AddExitHack(new ReturnCookie(label, local));
					}
					else
					{
						// HACK the x64 JIT is lame and optimizes calls before ret into a tail call
						// and this makes the method disappear from the call stack, so we try to thwart that
						// by inserting some bogus instructions between the call and the return.
						// Note that this optimization doesn't appear to happen if the method has exception handlers,
						// so in that case we don't do anything.
						bool x64hack = false;
						if(exceptions.Length == 0 && i > 0)
						{
							int k = i - 1;
							while(k > 0 && m.Instructions[k].NormalizedOpCode == NormalizedByteCode.__nop)
							{
								k--;
							}
							switch(m.Instructions[k].NormalizedOpCode)
							{
								case NormalizedByteCode.__invokeinterface:
								case NormalizedByteCode.__invokespecial:
								case NormalizedByteCode.__invokestatic:
								case NormalizedByteCode.__invokevirtual:
									x64hack = true;
									break;
							}
						}
						// if there is junk on the stack (other than the return value), we must pop it off
						// because in .NET this is invalid (unlike in Java)
						int stackHeight = ma.GetStackHeight(i);
						if(instr.NormalizedOpCode == NormalizedByteCode.__return)
						{
							if(stackHeight != 0 || x64hack)
							{
								ilGenerator.Emit(OpCodes.Leave_S, (byte)0);
							}
							ilGenerator.Emit(OpCodes.Ret);
						}
						else
						{
							TypeWrapper retTypeWrapper = mw.ReturnType;
							retTypeWrapper.EmitConvStackTypeToSignatureType(ilGenerator, ma.GetStackTypeWrapper(i, 0));
							if(stackHeight != 1)
							{
								LocalBuilder local = ilGenerator.AllocTempLocal(retTypeWrapper.TypeAsSignatureType);
								ilGenerator.Emit(OpCodes.Stloc, local);
								ilGenerator.Emit(OpCodes.Leave_S, (byte)0);
								ilGenerator.Emit(OpCodes.Ldloc, local);
								ilGenerator.ReleaseTempLocal(local);
							}
							else if(x64hack)
							{
								ilGenerator.Emit(OpCodes.Ldnull);
								ilGenerator.Emit(OpCodes.Pop);
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
					else if(VerifierTypeWrapper.IsThis(type))
					{
						ilGenerator.Emit(OpCodes.Ldarg_0);
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
						ilGenerator.LazyEmitPop();
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
					StoreLocal(instr);
					break;
				case NormalizedByteCode.__fstore_conv:	// since we convert after every FP-operation, we don't need this convert anymore
				case NormalizedByteCode.__fstore:
					StoreLocal(instr);
					break;
				case NormalizedByteCode.__dstore_conv:
					ilGenerator.Emit(OpCodes.Conv_R8);
					StoreLocal(instr);
					break;
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
						ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicNewCheckOnly);
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
					LocalBuilder localArray = ilGenerator.UnsafeAllocTempLocal(typeof(int[]));
					LocalBuilder localInt = ilGenerator.UnsafeAllocTempLocal(typeof(int));
					ilGenerator.LazyEmitLdc_I4(instr.Arg2);
					ilGenerator.Emit(OpCodes.Newarr, typeof(int));
					ilGenerator.Emit(OpCodes.Stloc, localArray);
					for(int j = 1; j <= instr.Arg2; j++)
					{
						ilGenerator.Emit(OpCodes.Stloc, localInt);
						ilGenerator.Emit(OpCodes.Ldloc, localArray);
						ilGenerator.LazyEmitLdc_I4(instr.Arg2 - j);
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
						ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicMultianewarray);
					}
					else if(wrapper.IsGhost || wrapper.IsGhostArray)
					{
						TypeWrapper tw = wrapper;
						while(tw.IsArray)
						{
							tw = tw.ElementTypeWrapper;
						}
						ilGenerator.Emit(OpCodes.Ldtoken, ArrayTypeWrapper.MakeArrayType(tw.TypeAsTBD, wrapper.ArrayRank));
						ilGenerator.Emit(OpCodes.Ldloc, localArray);
						ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.multianewarray_ghost);
						ilGenerator.Emit(OpCodes.Castclass, wrapper.TypeAsArrayType);
					}
					else
					{
						Type type = wrapper.TypeAsArrayType;
						ilGenerator.Emit(OpCodes.Ldtoken, type);
						ilGenerator.Emit(OpCodes.Ldloc, localArray);
						ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.multianewarray);
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
						ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicNewarray);
					}
					else if(wrapper.IsGhost || wrapper.IsGhostArray)
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
						TypeWrapper tw = wrapper;
						while(tw.IsArray)
						{
							tw = tw.ElementTypeWrapper;
						}
						ilGenerator.Emit(OpCodes.Ldtoken, ArrayTypeWrapper.MakeArrayType(tw.TypeAsTBD, wrapper.ArrayRank + 1));
						ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.anewarray_ghost.MakeGenericMethod(wrapper.TypeAsArrayType));
					}
					else
					{
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
						ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicAaload);
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
				case NormalizedByteCode.__fastore_conv:	// since we convert after every FP-operation, we don't need this convert anymore
				case NormalizedByteCode.__fastore:
					ilGenerator.Emit(OpCodes.Stelem_R4);
					break;
				case NormalizedByteCode.__daload:
					ilGenerator.Emit(OpCodes.Ldelem_R8);
					break;
				case NormalizedByteCode.__dastore_conv:
					ilGenerator.Emit(OpCodes.Conv_R8);
					ilGenerator.Emit(OpCodes.Stelem_R8);
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
						ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicAastore);
					}
					else
					{
						TypeWrapper elem = tw.ElementTypeWrapper;
						if(elem.IsNonPrimitiveValueType)
						{
							Type t = elem.TypeAsTBD;
							LocalBuilder local = ilGenerator.UnsafeAllocTempLocal(typeof(object));
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
					ilGenerator.LazyEmit_lcmp();
					break;
				case NormalizedByteCode.__fcmpl:
					ilGenerator.LazyEmit_fcmpl();
					break;
				case NormalizedByteCode.__fcmpg:
					ilGenerator.LazyEmit_fcmpg();
					break;
				case NormalizedByteCode.__dcmpl:
					ilGenerator.LazyEmit_dcmpl();
					break;
				case NormalizedByteCode.__dcmpg:
					ilGenerator.LazyEmit_dcmpg();
					break;
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
					ilGenerator.LazyEmit_if_le_lt_ge_gt(CodeEmitter.Comparison.LessOrEqual, block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__iflt:
					ilGenerator.LazyEmit_if_le_lt_ge_gt(CodeEmitter.Comparison.LessThan, block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__ifge:
					ilGenerator.LazyEmit_if_le_lt_ge_gt(CodeEmitter.Comparison.GreaterOrEqual, block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__ifgt:
					ilGenerator.LazyEmit_if_le_lt_ge_gt(CodeEmitter.Comparison.GreaterThan, block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__ifne:
					ilGenerator.LazyEmit_ifne(block.GetLabel(instr.PC + instr.Arg1));
					break;
				case NormalizedByteCode.__ifeq:
					ilGenerator.LazyEmit_ifeq(block.GetLabel(instr.PC + instr.Arg1));
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
					ilGenerator.Emit(OpCodes.Add);
					break;
				case NormalizedByteCode.__fadd:
					ilGenerator.Emit(OpCodes.Add);
					ilGenerator.Emit(OpCodes.Conv_R4);
					break;
				case NormalizedByteCode.__dadd:
					ilGenerator.Emit(OpCodes.Add);
					if(strictfp)
					{
						ilGenerator.Emit(OpCodes.Conv_R8);
					}
					break;
				case NormalizedByteCode.__isub:
				case NormalizedByteCode.__lsub:
					ilGenerator.Emit(OpCodes.Sub);
					break;
				case NormalizedByteCode.__fsub:
					ilGenerator.Emit(OpCodes.Sub);
					ilGenerator.Emit(OpCodes.Conv_R4);
					break;
				case NormalizedByteCode.__dsub:
					ilGenerator.Emit(OpCodes.Sub);
					if(strictfp)
					{
						ilGenerator.Emit(OpCodes.Conv_R8);
					}
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
					ilGenerator.Emit(OpCodes.Mul);
					break;
				case NormalizedByteCode.__fmul:
					ilGenerator.Emit(OpCodes.Mul);
					ilGenerator.Emit(OpCodes.Conv_R4);
					break;
				case NormalizedByteCode.__dmul:
					ilGenerator.Emit(OpCodes.Mul);
					if(strictfp)
					{
						ilGenerator.Emit(OpCodes.Conv_R8);
					}
					break;
				case NormalizedByteCode.__idiv:
					ilGenerator.LazyEmit_idiv();
					break;
				case NormalizedByteCode.__ldiv:
					ilGenerator.LazyEmit_ldiv();
					break;
				case NormalizedByteCode.__fdiv:
					ilGenerator.Emit(OpCodes.Div);
					ilGenerator.Emit(OpCodes.Conv_R4);
					break;
				case NormalizedByteCode.__ddiv:
					ilGenerator.Emit(OpCodes.Div);
					if(strictfp)
					{
						ilGenerator.Emit(OpCodes.Conv_R8);
					}
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
					CodeEmitterLabel label = ilGenerator.DefineLabel();
					ilGenerator.Emit(OpCodes.Bne_Un_S, label);
					ilGenerator.Emit(OpCodes.Pop);
					ilGenerator.Emit(OpCodes.Pop);
					ilGenerator.Emit(OpCodes.Ldc_I4_0);
					if(instr.NormalizedOpCode == NormalizedByteCode.__lrem)
					{
						ilGenerator.Emit(OpCodes.Conv_I8);
					}
					CodeEmitterLabel label2 = ilGenerator.DefineLabel();
					ilGenerator.Emit(OpCodes.Br_S, label2);
					ilGenerator.MarkLabel(label);
					ilGenerator.Emit(OpCodes.Rem);
					ilGenerator.MarkLabel(label2);
					break;
				}
				case NormalizedByteCode.__frem:
					ilGenerator.Emit(OpCodes.Rem);
					ilGenerator.Emit(OpCodes.Conv_R4);
					break;
				case NormalizedByteCode.__drem:
					ilGenerator.Emit(OpCodes.Rem);
					if(strictfp)
					{
						ilGenerator.Emit(OpCodes.Conv_R8);
					}
					break;
				case NormalizedByteCode.__ishl:
					ilGenerator.LazyEmitAnd_I4(31);
					ilGenerator.Emit(OpCodes.Shl);
					break;
				case NormalizedByteCode.__lshl:
					ilGenerator.LazyEmitAnd_I4(63);
					ilGenerator.Emit(OpCodes.Shl);
					break;
				case NormalizedByteCode.__iushr:
					ilGenerator.LazyEmitAnd_I4(31);
					ilGenerator.Emit(OpCodes.Shr_Un);
					break;
				case NormalizedByteCode.__lushr:
					ilGenerator.LazyEmitAnd_I4(63);
					ilGenerator.Emit(OpCodes.Shr_Un);
					break;
				case NormalizedByteCode.__ishr:
					ilGenerator.LazyEmitAnd_I4(31);
					ilGenerator.Emit(OpCodes.Shr);
					break;
				case NormalizedByteCode.__lshr:
					ilGenerator.LazyEmitAnd_I4(63);
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
					TypeWrapper type2 = ma.GetRawStackTypeWrapper(i, 1);
					if(type2.IsWidePrimitive)
					{
						// Form 2
						DupHelper dh = new DupHelper(this, 2);
						dh.SetType(0, ma.GetRawStackTypeWrapper(i, 0));
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
						// Form 1
						DupHelper dh = new DupHelper(this, 3);
						dh.SetType(0, ma.GetRawStackTypeWrapper(i, 0));
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
					break;
				}
				case NormalizedByteCode.__pop2:
				{
					TypeWrapper type1 = ma.GetRawStackTypeWrapper(i, 0);
					if(type1.IsWidePrimitive)
					{
						ilGenerator.LazyEmitPop();
					}
					else
					{
						if(!VerifierTypeWrapper.IsNew(type1))
						{
							ilGenerator.LazyEmitPop();
						}
						if(!VerifierTypeWrapper.IsNew(ma.GetRawStackTypeWrapper(i, 1)))
						{
							ilGenerator.LazyEmitPop();
						}
					}
					break;
				}
				case NormalizedByteCode.__pop:
					// if the TOS is a new object, it isn't really there, so we don't need to pop it
					if(!VerifierTypeWrapper.IsNew(ma.GetRawStackTypeWrapper(i, 0)))
					{
						ilGenerator.LazyEmitPop();
					}
					break;
				case NormalizedByteCode.__monitorenter:
					ilGenerator.Emit(OpCodes.Call, monitorEnterMethod);
					break;
				case NormalizedByteCode.__monitorexit:
					ilGenerator.Emit(OpCodes.Call, monitorExitMethod);
					break;
				case NormalizedByteCode.__athrow_no_unmap:
					if(ma.GetRawStackTypeWrapper(i, 0).IsUnloadable)
					{
						ilGenerator.Emit(OpCodes.Castclass, typeof(Exception));
					}
					ilGenerator.Emit(OpCodes.Throw);
					break;
				case NormalizedByteCode.__athrow:
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
					CodeEmitterLabel[] labels = new CodeEmitterLabel[instr.SwitchEntryCount];
					for(int j = 0; j < labels.Length; j++)
					{
						labels[j] = block.GetLabel(instr.PC + instr.GetSwitchTargetOffset(j));
					}
					if(instr.GetSwitchValue(0) != 0)
					{
						ilGenerator.LazyEmitLdc_I4(instr.GetSwitchValue(0));
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
						ilGenerator.LazyEmitLdc_I4(instr.GetSwitchValue(j));
						CodeEmitterLabel label = ilGenerator.DefineLabel();
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
					ilGenerator.LazyEmitLdc_I4(instr.Arg2);
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
					ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.f2i);
					break;
				case NormalizedByteCode.__d2i:
					ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.d2i);
					break;
				case NormalizedByteCode.__f2l:
					ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.f2l);
					break;
				case NormalizedByteCode.__d2l:
					ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.d2l);
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
							ilGenerator.LazyEmitLdc_I4(j);
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
							ilGenerator.LazyEmitLdc_I4(j);
							ilGenerator.Emit(OpCodes.Beq, block.GetLabel(m.Instructions[callsites[j] + 1].PC));
						}
					}
					if(m.Instructions[callsites[callsites.Length - 1]].IsReachable)
					{
						ilGenerator.Emit(OpCodes.Br, block.GetLabel(m.Instructions[callsites[callsites.Length - 1] + 1].PC));
					}
					else
					{
						// this code location is unreachable, but the verifier doesn't know that, so we emit a branch to keep it happy
						// (it would be a little nicer to rewrite the above for loop to dynamically find the last reachable callsite,
						// but since this only happens with unreachable code, it's not a big deal).
						ilGenerator.Emit(OpCodes.Br_S, (sbyte)-2);
					}
					break;
				}
				case NormalizedByteCode.__nop:
					ilGenerator.Emit(OpCodes.Nop);
					break;
				case NormalizedByteCode.__intrinsic_gettypehandlevalue:
					ilGenerator.Emit(OpCodes.Dup);
					EmitHelper.NullCheck(ilGenerator);
					EmitHelper.GetTypeHandleValue(ilGenerator);
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
				case NormalizedByteCode.__athrow_no_unmap:
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
		MethodKey key = new MethodKey(method.DeclaringType.Name, method.Name, method.Signature);
		MethodInfo mi;
		if(!invokespecialstubcache.TryGetValue(key, out mi))
		{
			MethodBuilder stub = clazz.TypeAsBuilder.DefineMethod("__<>", MethodAttributes.PrivateScope, method.ReturnTypeForDefineMethod, method.GetParametersForDefineMethod());
			CodeEmitter ilgen = CodeEmitter.Create(stub);
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
					TypeWrapper tw = ma.GetStackTypeWrapper(instructionIndex, args.Length - 1 - i);
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
				if(ma.GetRawStackTypeWrapper(instructionIndex, args.Length - 1 - i).IsUnloadable)
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
				if(tw != VerifierTypeWrapper.UninitializedThis
					&& !VerifierTypeWrapper.IsThis(tw))
				{
					tw = args[i];
				}
				dh.SetType(i, tw);
			}
			for(int i = args.Length - 1; i >= firstCastArg; i--)
			{
				if(!args[i].IsUnloadable && !args[i].IsGhost)
				{
					TypeWrapper tw = ma.GetStackTypeWrapper(instructionIndex, args.Length - 1 - i);
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
						LocalBuilder ghost = ilGenerator.AllocTempLocal(typeof(object));
						ilGenerator.Emit(OpCodes.Stloc, ghost);
						LocalBuilder local = ilGenerator.AllocTempLocal(args[i].TypeAsSignatureType);
						ilGenerator.Emit(OpCodes.Ldloca, local);
						ilGenerator.Emit(OpCodes.Ldloc, ghost);
						ilGenerator.Emit(OpCodes.Stfld, args[i].GhostRefField);
						ilGenerator.Emit(OpCodes.Ldloca, local);
						ilGenerator.ReleaseTempLocal(local);
						ilGenerator.ReleaseTempLocal(ghost);
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
		ilGenerator.Emit(OpCodes.Ldsfld, context.CallerIDField);
		switch(bytecode)
		{
			case NormalizedByteCode.__dynamic_getfield:
				Profiler.Count("EmitDynamicGetfield");
				ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicGetfield);
				EmitReturnTypeConversion(ilGenerator, fieldTypeWrapper);
				break;
			case NormalizedByteCode.__dynamic_putfield:
				Profiler.Count("EmitDynamicPutfield");
				ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicPutfield);
				break;
			case NormalizedByteCode.__dynamic_getstatic:
				Profiler.Count("EmitDynamicGetstatic");
				ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicGetstatic);
				EmitReturnTypeConversion(ilGenerator, fieldTypeWrapper);
				break;
			case NormalizedByteCode.__dynamic_putstatic:
				Profiler.Count("EmitDynamicPutstatic");
				ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicPutstatic);
				break;
			default:
				throw new InvalidOperationException();
		}
	}

	private static void EmitReturnTypeConversion(CodeEmitter ilgen, TypeWrapper typeWrapper)
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
			typeWrapper.EmitCheckcast(null, ilgen);
		}
	}

	private class DynamicMethodWrapper : MethodWrapper
	{
		private DynamicTypeWrapper.FinishContext context;
		private TypeWrapper wrapper;
		private ClassFile.ConstantPoolItemMI cpi;

		internal DynamicMethodWrapper(DynamicTypeWrapper.FinishContext context, TypeWrapper wrapper, ClassFile.ConstantPoolItemMI cpi)
			: base(wrapper, cpi.Name, cpi.Signature, null, cpi.GetRetType(), cpi.GetArgTypes(), Modifiers.Public, MemberFlags.None)
		{
			this.context = context;
			this.wrapper = wrapper;
			this.cpi = cpi;
		}

		internal override void EmitCall(CodeEmitter ilgen)
		{
			Emit(ByteCodeHelperMethods.DynamicInvokestatic, ilgen, cpi.GetRetType());
		}

		internal override void EmitCallvirt(CodeEmitter ilgen)
		{
			Emit(ByteCodeHelperMethods.DynamicInvokevirtual, ilgen, cpi.GetRetType());
		}

		internal override void EmitNewobj(CodeEmitter ilgen, MethodAnalyzer ma, int opcodeIndex)
		{
			Emit(ByteCodeHelperMethods.DynamicInvokeSpecialNew, ilgen, cpi.GetClassType());
		}

		private void Emit(MethodInfo helperMethod, CodeEmitter ilGenerator, TypeWrapper retTypeWrapper)
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
			ilGenerator.Emit(OpCodes.Ldsfld, context.CallerIDField);
			ilGenerator.Emit(OpCodes.Call, helperMethod);
			EmitReturnTypeConversion(ilGenerator, retTypeWrapper);
		}
	}

#if STATIC_COMPILER
	private class ReplacedMethodWrapper : MethodWrapper
	{
		private IKVM.Internal.MapXml.InstructionList code;

		internal ReplacedMethodWrapper(ClassFile.ConstantPoolItemMI cpi, IKVM.Internal.MapXml.InstructionList code)
			: base(cpi.GetClassType(), cpi.Name, cpi.Signature, null, cpi.GetRetType(), cpi.GetArgTypes(), Modifiers.Public, MemberFlags.None)
		{
			this.code = code;
		}

		internal override void EmitCall(CodeEmitter ilgen)
		{
			code.Emit(DeclaringType.GetClassLoader(), ilgen);
		}

		internal override void EmitCallvirt(CodeEmitter ilgen)
		{
			code.Emit(DeclaringType.GetClassLoader(), ilgen);
		}

		internal override void EmitNewobj(CodeEmitter ilgen, MethodAnalyzer ma, int opcodeIndex)
		{
			code.Emit(DeclaringType.GetClassLoader(), ilgen);
		}
	}
#endif

	private MethodWrapper GetMethodCallEmitter(ClassFile.ConstantPoolItemMI cpi, NormalizedByteCode invoke)
	{
#if STATIC_COMPILER
		if(replacedMethods != null)
		{
			for(int i = 0; i < replacedMethods.Length; i++)
			{
				if(replacedMethods[i].Class == cpi.Class
					&& replacedMethods[i].Name == cpi.Name
					&& replacedMethods[i].Sig == cpi.Signature)
				{
					if(replacedMethodWrappers[i] == null)
					{
						replacedMethodWrappers[i] = new ReplacedMethodWrapper(cpi, replacedMethods[i].code);
					}
					return replacedMethodWrappers[i];
				}
			}
		}
#endif
		MethodWrapper mw = null;
		switch (invoke)
		{
			case NormalizedByteCode.__invokespecial:
				mw = cpi.GetMethodForInvokespecial();
				break;
			case NormalizedByteCode.__invokeinterface:
				mw = cpi.GetMethod();
				break;
			case NormalizedByteCode.__invokestatic:
			case NormalizedByteCode.__invokevirtual:
				mw = cpi.GetMethod();
				break;
			case NormalizedByteCode.__dynamic_invokeinterface:
			case NormalizedByteCode.__dynamic_invokestatic:
			case NormalizedByteCode.__dynamic_invokevirtual:
			case NormalizedByteCode.__dynamic_invokespecial:
				return new DynamicMethodWrapper(context, clazz, cpi);
			default:
				throw new InvalidOperationException();
		}
		if(mw.IsDynamicOnly)
		{
			return new DynamicMethodWrapper(context, clazz, cpi);
		}
		return mw;
	}

	// TODO this method should have a better name
	private TypeWrapper SigTypeToClassName(TypeWrapper type, TypeWrapper nullType)
	{
		if(type == VerifierTypeWrapper.UninitializedThis
			|| VerifierTypeWrapper.IsThis(type))
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

	private int SafeFindPcIndex(int target)
	{
		if(target < 0 || target >= m.PcIndexMap.Length)
		{
			return -1;
		}
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
			if(v.type == PrimitiveTypeWrapper.DOUBLE)
			{
				ilGenerator.Emit(OpCodes.Conv_R8);
			}
			if(v.type == PrimitiveTypeWrapper.FLOAT)
			{
				ilGenerator.Emit(OpCodes.Conv_R4);
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
				if(debug && v.name != null)
				{
					v.builder.SetLocalSymInfo(v.name);
				}
			}
			ilGenerator.Emit(OpCodes.Ldloc, v.builder);
		}
		return v;
	}

	private LocalVar StoreLocal(ClassFile.Method.Instruction instr)
	{
		LocalVar v = ma.GetLocalVar(FindPcIndex(instr.PC));
		if(v == null)
		{
			// dead store
			ilGenerator.LazyEmitPop();
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
			ilGenerator.LazyEmitPop();
		}
		else
		{
			if(v.builder == null)
			{
				v.builder = ilGenerator.DeclareLocal(v.type.TypeAsLocalOrStackType);
				if(debug && v.name != null)
				{
					v.builder.SetLocalSymInfo(v.name);
				}
			}
			ilGenerator.Emit(OpCodes.Stloc, v.builder);
		}
		return v;
	}
}

#endif
