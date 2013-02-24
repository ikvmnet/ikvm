/*
  Copyright (C) 2002-2013 Jeroen Frijters

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
#if STATIC_COMPILER
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using IKVM.Attributes;
using IKVM.Internal;

using ExceptionTableEntry = IKVM.Internal.ClassFile.Method.ExceptionTableEntry;
using LocalVariableTableEntry = IKVM.Internal.ClassFile.Method.LocalVariableTableEntry;
using Instruction = IKVM.Internal.ClassFile.Method.Instruction;
using InstructionFlags = IKVM.Internal.ClassFile.Method.InstructionFlags;

static class ByteCodeHelperMethods
{
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
	internal static readonly MethodInfo DynamicAaload;
	internal static readonly MethodInfo DynamicAastore;
	internal static readonly MethodInfo DynamicClassLiteral;
	internal static readonly MethodInfo DynamicGetfield;
	internal static readonly MethodInfo DynamicGetstatic;
	internal static readonly MethodInfo DynamicMultianewarray;
	internal static readonly MethodInfo DynamicNewarray;
	internal static readonly MethodInfo DynamicNewCheckOnly;
	internal static readonly MethodInfo DynamicPutfield;
	internal static readonly MethodInfo DynamicPutstatic;
	internal static readonly MethodInfo DynamicCreateDelegate;
	internal static readonly MethodInfo DynamicLoadMethodType;
	internal static readonly MethodInfo DynamicLoadMethodHandle;
	internal static readonly MethodInfo DynamicBinderMemberLookup;
	internal static readonly MethodInfo VerboseCastFailure;
	internal static readonly MethodInfo SkipFinalizer;
	internal static readonly MethodInfo DynamicInstanceOf;
	internal static readonly MethodInfo volatileReadDouble;
	internal static readonly MethodInfo volatileReadLong;
	internal static readonly MethodInfo volatileWriteDouble;
	internal static readonly MethodInfo volatileWriteLong;
	internal static readonly MethodInfo mapException;
	internal static readonly MethodInfo GetDelegateForInvokeExact;
	internal static readonly MethodInfo GetDelegateForInvoke;
	internal static readonly MethodInfo LoadMethodType;
	internal static readonly MethodInfo MethodHandleFromDelegate;
	internal static readonly MethodInfo LinkIndyCallSite;

	static ByteCodeHelperMethods()
	{
#if STATIC_COMPILER
		Type typeofByteCodeHelper = StaticCompiler.GetRuntimeType("IKVM.Runtime.ByteCodeHelper");
#else
		Type typeofByteCodeHelper = typeof(IKVM.Runtime.ByteCodeHelper);
#endif
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
		DynamicAaload = typeofByteCodeHelper.GetMethod("DynamicAaload");
		DynamicAastore = typeofByteCodeHelper.GetMethod("DynamicAastore");
		DynamicClassLiteral = typeofByteCodeHelper.GetMethod("DynamicClassLiteral");
		DynamicGetfield = typeofByteCodeHelper.GetMethod("DynamicGetfield");
		DynamicGetstatic = typeofByteCodeHelper.GetMethod("DynamicGetstatic");
		DynamicMultianewarray = typeofByteCodeHelper.GetMethod("DynamicMultianewarray");
		DynamicNewarray = typeofByteCodeHelper.GetMethod("DynamicNewarray");
		DynamicNewCheckOnly = typeofByteCodeHelper.GetMethod("DynamicNewCheckOnly");
		DynamicPutfield = typeofByteCodeHelper.GetMethod("DynamicPutfield");
		DynamicPutstatic = typeofByteCodeHelper.GetMethod("DynamicPutstatic");
		DynamicCreateDelegate = typeofByteCodeHelper.GetMethod("DynamicCreateDelegate");
		DynamicLoadMethodType = typeofByteCodeHelper.GetMethod("DynamicLoadMethodType");
		DynamicLoadMethodHandle = typeofByteCodeHelper.GetMethod("DynamicLoadMethodHandle");
		DynamicBinderMemberLookup = typeofByteCodeHelper.GetMethod("DynamicBinderMemberLookup");
		VerboseCastFailure = typeofByteCodeHelper.GetMethod("VerboseCastFailure");
		SkipFinalizer = typeofByteCodeHelper.GetMethod("SkipFinalizer");
		DynamicInstanceOf = typeofByteCodeHelper.GetMethod("DynamicInstanceOf");
		volatileReadDouble = typeofByteCodeHelper.GetMethod("VolatileRead", new Type[] { Types.Double.MakeByRefType() });
		volatileReadLong = typeofByteCodeHelper.GetMethod("VolatileRead", new Type[] { Types.Int64.MakeByRefType() });
		volatileWriteDouble = typeofByteCodeHelper.GetMethod("VolatileWrite", new Type[] { Types.Double.MakeByRefType(), Types.Double });
		volatileWriteLong = typeofByteCodeHelper.GetMethod("VolatileWrite", new Type[] { Types.Int64.MakeByRefType(), Types.Int64 });
		mapException = typeofByteCodeHelper.GetMethod("MapException");
		GetDelegateForInvokeExact = typeofByteCodeHelper.GetMethod("GetDelegateForInvokeExact");
		GetDelegateForInvoke = typeofByteCodeHelper.GetMethod("GetDelegateForInvoke");
		LoadMethodType = typeofByteCodeHelper.GetMethod("LoadMethodType");
		MethodHandleFromDelegate = typeofByteCodeHelper.GetMethod("MethodHandleFromDelegate");
		LinkIndyCallSite = typeofByteCodeHelper.GetMethod("LinkIndyCallSite");
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

static partial class MethodHandleUtil
{
	internal const int MaxArity = 8;
	private static readonly Type typeofMHA;
	private static readonly Type[] typeofMHV;
	private static readonly Type[] typeofMH;

	static MethodHandleUtil()
	{
#if STATIC_COMPILER
		typeofMHA = StaticCompiler.GetRuntimeType("IKVM.Runtime.MHA`8");
		typeofMHV = new Type[] {
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MHV"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MHV`1"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MHV`2"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MHV`3"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MHV`4"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MHV`5"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MHV`6"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MHV`7"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MHV`8"),
		};
		typeofMH = new Type[] {
			null,
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MH`1"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MH`2"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MH`3"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MH`4"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MH`5"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MH`6"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MH`7"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MH`8"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MH`9"),
		};
#else
		typeofMHA = typeof(IKVM.Runtime.MHA<,,,,,,,>);
		typeofMHV = new Type[] {
			typeof(IKVM.Runtime.MHV),
			typeof(IKVM.Runtime.MHV<>),
			typeof(IKVM.Runtime.MHV<,>),
			typeof(IKVM.Runtime.MHV<,,>),
			typeof(IKVM.Runtime.MHV<,,,>),
			typeof(IKVM.Runtime.MHV<,,,,>),
			typeof(IKVM.Runtime.MHV<,,,,,>),
			typeof(IKVM.Runtime.MHV<,,,,,,>),
			typeof(IKVM.Runtime.MHV<,,,,,,,>),
		};
		typeofMH = new Type[] {
			null,
			typeof(IKVM.Runtime.MH<>),
			typeof(IKVM.Runtime.MH<,>),
			typeof(IKVM.Runtime.MH<,,>),
			typeof(IKVM.Runtime.MH<,,,>),
			typeof(IKVM.Runtime.MH<,,,,>),
			typeof(IKVM.Runtime.MH<,,,,,>),
			typeof(IKVM.Runtime.MH<,,,,,,>),
			typeof(IKVM.Runtime.MH<,,,,,,,>),
			typeof(IKVM.Runtime.MH<,,,,,,,,>),
		};
#endif
	}

	internal static void EmitCallDelegateInvokeMethod(CodeEmitter ilgen, Type delegateType)
	{
		if (delegateType.IsGenericType)
		{
			// MONOBUG we don't look at the invoke method directly here, because Mono doesn't support GetParameters() on a builder instantiation
			Type[] typeArgs = delegateType.GetGenericArguments();
			if (IsPackedArgsContainer(typeArgs[typeArgs.Length - 1]))
			{
				WrapArgs(ilgen, typeArgs[typeArgs.Length - 1]);
			}
			else if (typeArgs.Length > 2 && IsPackedArgsContainer(typeArgs[typeArgs.Length - 2]))
			{
				WrapArgs(ilgen, typeArgs[typeArgs.Length - 2]);
			}
		}
		ilgen.Emit(OpCodes.Callvirt, GetDelegateInvokeMethod(delegateType));
	}

	private static void WrapArgs(CodeEmitter ilgen, Type type)
	{
		Type last = type.GetGenericArguments()[MaxArity - 1];
		if (MethodHandleUtil.IsPackedArgsContainer(last))
		{
			WrapArgs(ilgen, last);
		}
		ilgen.Emit(OpCodes.Newobj, type.GetConstructors()[0]);
	}

	internal static MethodInfo GetDelegateInvokeMethod(Type delegateType)
	{
		if (ReflectUtil.ContainsTypeBuilder(delegateType))
		{
			return TypeBuilder.GetMethod(delegateType, delegateType.GetGenericTypeDefinition().GetMethod("Invoke"));
		}
		else
		{
			return delegateType.GetMethod("Invoke");
		}
	}

	internal static ConstructorInfo GetDelegateConstructor(Type delegateType)
	{
		if (ReflectUtil.ContainsTypeBuilder(delegateType))
		{
			return TypeBuilder.GetConstructor(delegateType, delegateType.GetGenericTypeDefinition().GetConstructors()[0]);
		}
		else
		{
			return delegateType.GetConstructors()[0];
		}
	}

	internal static Type CreateDelegateType(TypeWrapper tw, MethodWrapper mw)
	{
		TypeWrapper[] args = mw.GetParameters();
		if (!mw.IsStatic)
		{
			Array.Resize(ref args, args.Length + 1);
			Array.Copy(args, 0, args, 1, args.Length - 1);
			args[0] = tw;
		}
		return CreateDelegateType(args, mw.ReturnType);
	}

	internal static Type CreateDelegateType(TypeWrapper[] args, TypeWrapper ret)
	{
		Type[] typeArgs = new Type[args.Length];
		for (int i = 0; i < args.Length; i++)
		{
			typeArgs[i] = args[i].TypeAsSignatureType;
		}
		return CreateDelegateType(typeArgs, ret.TypeAsSignatureType);
	}

	// for delegate types used for "ldc <MethodType>" we don't want ghost arrays to be erased
	internal static Type CreateDelegateTypeForLoadConstant(TypeWrapper[] args, TypeWrapper ret)
	{
		Type[] typeArgs = new Type[args.Length];
		for (int i = 0; i < args.Length; i++)
		{
			typeArgs[i] = TypeWrapperToTypeForLoadConstant(args[i]);
		}
		return CreateDelegateType(typeArgs, TypeWrapperToTypeForLoadConstant(ret));
	}

	private static Type TypeWrapperToTypeForLoadConstant(TypeWrapper tw)
	{
		if (tw.IsGhostArray)
		{
			int dims = tw.ArrayRank;
			while (tw.IsArray)
			{
				tw = tw.ElementTypeWrapper;
			}
			return ArrayTypeWrapper.MakeArrayType(tw.TypeAsSignatureType, dims);
		}
		else
		{
			return tw.TypeAsSignatureType;
		}
	}

	private static Type CreateDelegateType(Type[] types, Type retType)
	{
		if (types.Length == 0 && retType == Types.Void)
		{
			return typeofMHV[0];
		}
		else if (types.Length > MaxArity)
		{
			int arity = types.Length;
			int remainder = (arity - 8) % 7;
			int count = (arity - 8) / 7;
			if (remainder == 0)
			{
				remainder = 7;
				count--;
			}
			Type last = typeofMHA.MakeGenericType(SubArray(types, types.Length - 8, 8));
			for (int i = 0; i < count; i++)
			{
				Type[] temp = SubArray(types, types.Length - 8 - 7 * (i + 1), 8);
				temp[7] = last;
				last = typeofMHA.MakeGenericType(temp);
			}
			types = SubArray(types, 0, remainder + 1);
			types[remainder] = last;
		}
		if (retType == Types.Void)
		{
			return typeofMHV[types.Length].MakeGenericType(types);
		}
		else
		{
			Array.Resize(ref types, types.Length + 1);
			types[types.Length - 1] = retType;
			return typeofMH[types.Length].MakeGenericType(types);
		}
	}

	private static Type[] SubArray(Type[] inArray, int start, int length)
	{
		Type[] outArray = new Type[length];
		Array.Copy(inArray, start, outArray, 0, length);
		return outArray;
	}

	internal static bool IsPackedArgsContainer(Type type)
	{
		return type.IsGenericType
			&& type.GetGenericTypeDefinition() == typeofMHA;
	}
}

sealed class Compiler
{
	private static readonly MethodInfo unmapExceptionMethod;
	private static readonly MethodInfo fixateExceptionMethod;
	private static readonly MethodInfo suppressFillInStackTraceMethod;
	internal static readonly MethodInfo getTypeFromHandleMethod;
	internal static readonly MethodInfo getTypeMethod;
	private static readonly MethodInfo keepAliveMethod;
	internal static readonly MethodWrapper getClassFromTypeHandle;
	internal static readonly MethodWrapper getClassFromTypeHandle2;
	private static readonly TypeWrapper java_lang_Object;
	private static readonly TypeWrapper java_lang_Class;
	private static readonly TypeWrapper java_lang_Throwable;
	private static readonly TypeWrapper cli_System_Object;
	private static readonly TypeWrapper cli_System_Exception;
	private readonly DynamicTypeWrapper.FinishContext context;
	private readonly DynamicTypeWrapper clazz;
	private readonly MethodWrapper mw;
	private readonly ClassFile classFile;
	private readonly ClassFile.Method m;
	private readonly CodeEmitter ilGenerator;
	private readonly CodeInfo ma;
	private readonly UntangledExceptionTable exceptions;
	private readonly List<string> harderrors;
	private readonly LocalVarInfo localVars;
	private bool nonleaf;
	private readonly bool debug;
	private readonly bool keepAlive;
	private readonly bool strictfp;
	private readonly bool emitLineNumbers;
	private int[] scopeBegin;
	private int[] scopeClose;
#if STATIC_COMPILER
	private readonly MethodWrapper[] replacedMethodWrappers;
#endif

	static Compiler()
	{
		getTypeFromHandleMethod = Types.Type.GetMethod("GetTypeFromHandle", BindingFlags.Static | BindingFlags.Public, null, new Type[] { Types.RuntimeTypeHandle }, null);
		getTypeMethod = Types.Object.GetMethod("GetType", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
		keepAliveMethod = JVM.Import(typeof(System.GC)).GetMethod("KeepAlive", BindingFlags.Static | BindingFlags.Public, null, new Type[] { Types.Object }, null);
		java_lang_Object = CoreClasses.java.lang.Object.Wrapper;
		java_lang_Throwable = CoreClasses.java.lang.Throwable.Wrapper;
		cli_System_Object = DotNetTypeWrapper.GetWrapperFromDotNetType(Types.Object);
		cli_System_Exception = DotNetTypeWrapper.GetWrapperFromDotNetType(Types.Exception);
		java_lang_Class = CoreClasses.java.lang.Class.Wrapper;
		// HACK we need to special case core compilation, because the __<map> methods are HideFromJava
		if(java_lang_Throwable.TypeAsBaseType is TypeBuilder)
		{
			MethodWrapper mw;
			mw = java_lang_Throwable.GetMethodWrapper("__<suppressFillInStackTrace>", "()V", false);
			mw.Link();
			suppressFillInStackTraceMethod = (MethodInfo)mw.GetMethod();
			mw = java_lang_Throwable.GetMethodWrapper("__<unmap>", "(Ljava.lang.Throwable;)Ljava.lang.Throwable;", false);
			mw.Link();
			unmapExceptionMethod = (MethodInfo)mw.GetMethod();
			mw = java_lang_Throwable.GetMethodWrapper("__<fixate>", "(Ljava.lang.Throwable;)Ljava.lang.Throwable;", false);
			mw.Link();
			fixateExceptionMethod = (MethodInfo)mw.GetMethod();
		}
		else
		{
			suppressFillInStackTraceMethod = java_lang_Throwable.TypeAsBaseType.GetMethod("__<suppressFillInStackTrace>", Type.EmptyTypes);
			unmapExceptionMethod = java_lang_Throwable.TypeAsBaseType.GetMethod("__<unmap>", new Type[] { Types.Exception });
			fixateExceptionMethod = java_lang_Throwable.TypeAsBaseType.GetMethod("__<fixate>", new Type[] { Types.Exception });
		}
		getClassFromTypeHandle = ClassLoaderWrapper.LoadClassCritical("ikvm.runtime.Util").GetMethodWrapper("getClassFromTypeHandle", "(Lcli.System.RuntimeTypeHandle;)Ljava.lang.Class;", false);
		getClassFromTypeHandle.Link();
		getClassFromTypeHandle2 = ClassLoaderWrapper.LoadClassCritical("ikvm.runtime.Util").GetMethodWrapper("getClassFromTypeHandle", "(Lcli.System.RuntimeTypeHandle;I)Ljava.lang.Class;", false);
		getClassFromTypeHandle2.Link();
	}

	private Compiler(DynamicTypeWrapper.FinishContext context, DynamicTypeWrapper clazz, MethodWrapper mw, ClassFile classFile, ClassFile.Method m, CodeEmitter ilGenerator, ClassLoaderWrapper classLoader)
	{
		this.context = context;
		this.clazz = clazz;
		this.mw = mw;
		this.classFile = classFile;
		this.m = m;
		this.ilGenerator = ilGenerator;
		this.debug = classLoader.EmitDebugInfo;
		this.strictfp = m.IsStrictfp;
		if(mw.IsConstructor)
		{
			MethodWrapper finalize = clazz.GetMethodWrapper(StringConstants.FINALIZE, StringConstants.SIG_VOID, true);
			keepAlive = finalize != null && finalize.DeclaringType != java_lang_Object && finalize.DeclaringType != cli_System_Object && finalize.DeclaringType != java_lang_Throwable && finalize.DeclaringType != cli_System_Exception;
		}
#if STATIC_COMPILER
		replacedMethodWrappers = clazz.GetReplacedMethodsFor(mw);
#endif

		Profiler.Enter("MethodAnalyzer");
		try
		{
			if(classFile.MajorVersion < 51 && m.HasJsr)
			{
				JsrInliner.InlineJsrs(classLoader, mw, classFile, m);
			}
			MethodAnalyzer verifier = new MethodAnalyzer(clazz, mw, classFile, m, classLoader);
			exceptions = MethodAnalyzer.UntangleExceptionBlocks(classFile, m);
			ma = verifier.GetCodeInfoAndErrors(exceptions, out harderrors);
			localVars = new LocalVarInfo(ma, classFile, m, exceptions, mw, classLoader);
		}
		finally
		{
			Profiler.Leave("MethodAnalyzer");
		}

		if (m.LineNumberTableAttribute != null)
		{
			if (classLoader.EmitDebugInfo)
			{
				emitLineNumbers = true;
			}
			else if (classLoader.EmitStackTraceInfo)
			{
				InstructionFlags[] flags = ComputePartialReachability(0, false);
				for (int i = 0; i < m.Instructions.Length; i++)
				{
					if ((flags[i] & InstructionFlags.Reachable) == 0)
					{
						// skip unreachable instructions
					}
					else if (m.Instructions[i].NormalizedOpCode == NormalizedByteCode.__getfield
						&& VerifierTypeWrapper.IsThis(ma.GetRawStackTypeWrapper(i, 0)))
					{
						// loading a field from the current object cannot throw
					}
					else if (m.Instructions[i].NormalizedOpCode == NormalizedByteCode.__putfield
						&& VerifierTypeWrapper.IsThis(ma.GetRawStackTypeWrapper(i, 1)))
					{
						// storing a field in the current object cannot throw
					}
					else if (m.Instructions[i].NormalizedOpCode == NormalizedByteCode.__getstatic
						&& classFile.GetFieldref(m.Instructions[i].Arg1).GetClassType() == clazz)
					{
						// loading a field from the current class cannot throw
					}
					else if (m.Instructions[i].NormalizedOpCode == NormalizedByteCode.__putstatic
						&& classFile.GetFieldref(m.Instructions[i].Arg1).GetClassType() == clazz)
					{
						// storing a field to the current class cannot throw
					}
					else if (ByteCodeMetaData.CanThrowException(m.Instructions[i].NormalizedOpCode))
					{
						emitLineNumbers = true;
						break;
					}
				}
			}
		}

		TypeWrapper[] args = mw.GetParameters();
		LocalVar[] locals = localVars.GetAllLocalVars();
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
					v.builder = ilGenerator.DeclareLocal(GetLocalBuilderType(v.type));
					if(debug && v.name != null)
					{
						v.builder.SetLocalSymInfo(v.name);
					}
					v.isArg = false;
					ilGenerator.EmitLdarg(arg);
					tw.EmitConvSignatureTypeToStackType(ilGenerator);
					ilGenerator.Emit(OpCodes.Stloc, v.builder);
				}
			}
		}

		// if we're emitting debugging information, we need to use scopes for local variables
		if(debug)
		{
			SetupLocalVariableScopes();
		}

		Workaroundx64JitBug(args);
	}

	// workaround for x64 JIT bug
	// https://connect.microsoft.com/VisualStudio/feedback/details/636466/variable-is-not-incrementing-in-c-release-x64#details
	// (see also https://sourceforge.net/mailarchive/message.php?msg_id=28250469)
	private void Workaroundx64JitBug(TypeWrapper[] args)
	{
		if(args.Length > (m.IsStatic ? 4 : 3) && m.ExceptionTable.Length != 0)
		{
			bool[] workarounds = null;
			InstructionFlags[] flags = ComputePartialReachability(0, false);
			for(int i = 0; i < m.Instructions.Length; i++)
			{
				if((flags[i] & InstructionFlags.Reachable) == 0)
				{
					// skip unreachable instructions
				}
				else
				{
					switch(m.Instructions[i].NormalizedOpCode)
					{
						case NormalizedByteCode.__iinc:
						case NormalizedByteCode.__astore:
						case NormalizedByteCode.__istore:
						case NormalizedByteCode.__lstore:
						case NormalizedByteCode.__fstore:
						case NormalizedByteCode.__dstore:
							int arg = m.IsStatic ? m.Instructions[i].Arg1 : m.Instructions[i].Arg1 - 1;
							if(arg >= 3 && arg < args.Length)
							{
								if(workarounds == null)
								{
									workarounds = new bool[args.Length + 1];
								}
								workarounds[m.Instructions[i].Arg1] = true;
							}
							break;
					}
				}
			}
			if(workarounds != null)
			{
				for(int i = 0; i < workarounds.Length; i++)
				{
					if(workarounds[i])
					{
						// TODO prevent this from getting optimized away
						ilGenerator.EmitLdarga(i);
						ilGenerator.Emit(OpCodes.Pop);
					}
				}
			}
		}
	}

	private void SetupLocalVariableScopes()
	{
		LocalVariableTableEntry[] lvt = m.LocalVariableTableAttribute;
		if(lvt != null)
		{
			scopeBegin = new int[m.Instructions.Length];
			scopeClose = new int[m.Instructions.Length];

			// FXBUG make sure we always have an outer scope
			// (otherwise LocalBuilder.SetLocalSymInfo() might throw an IndexOutOfRangeException)
			// fix for bug 2881954.
			scopeBegin[0]++;
			scopeClose[m.Instructions.Length - 1]++;

			for (int i = 0; i < lvt.Length; i++)
			{
				int startIndex = SafeFindPcIndex(lvt[i].start_pc);
				int endIndex = SafeFindPcIndex(lvt[i].start_pc + lvt[i].length);
				if(startIndex != -1 && endIndex != -1)
				{
					if(startIndex > 0)
					{
						// NOTE javac (correctly) sets start_pc of the LVT entry to the instruction
						// following the store that first initializes the local, so we have to
						// detect that case and adjust our local scope (because we'll be creating
						// the local when we encounter the first store).
						LocalVar v = localVars.GetLocalVar(startIndex - 1);
						if(v != null && v.local == lvt[i].index)
						{
							startIndex--;
						}
					}
					scopeBegin[startIndex]++;
					scopeClose[endIndex]++;
				}
			}
		}
	}

	private int SafeFindPcIndex(int pc)
	{
		for(int i = 0; i < m.Instructions.Length; i++)
		{
			if(m.Instructions[i].PC >= pc)
			{
				return i;
			}
		}
		return -1;
	}

	private sealed class ReturnCookie
	{
		private readonly CodeEmitterLabel stub;
		private readonly CodeEmitterLocal local;

		internal ReturnCookie(CodeEmitterLabel stub, CodeEmitterLocal local)
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
		internal readonly int TargetIndex;
		internal DupHelper dh;

		internal BranchCookie(Compiler compiler, int stackHeight, int targetIndex)
		{
			this.Stub = compiler.ilGenerator.DefineLabel();
			this.TargetIndex = targetIndex;
			this.dh = new DupHelper(compiler, stackHeight);
		}

		internal BranchCookie(CodeEmitterLabel label, int targetIndex)
		{
			this.Stub = label;
			this.TargetIndex = targetIndex;
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
			FaultBlockException,
			Other
		}
		private readonly Compiler compiler;
		private readonly StackType[] types;
		private readonly CodeEmitterLocal[] locals;

		internal DupHelper(Compiler compiler, int count)
		{
			this.compiler = compiler;
			types = new StackType[count];
			locals = new CodeEmitterLocal[count];
		}

		internal void Release()
		{
			foreach (CodeEmitterLocal lb in locals)
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
			else if (VerifierTypeWrapper.IsFaultBlockException(type))
			{
				types[i] = StackType.FaultBlockException;
			}
			else
			{
				types[i] = StackType.Other;
				locals[i] = compiler.ilGenerator.AllocTempLocal(compiler.GetLocalBuilderType(type));
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
				case StackType.FaultBlockException:
					// objects aren't really there on the stack
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
					compiler.ilGenerator.Emit(OpCodes.Pop);
					break;
				case StackType.New:
				case StackType.FaultBlockException:
					// objects aren't really there on the stack
					break;
				case StackType.Other:
					compiler.ilGenerator.Emit(OpCodes.Stloc, locals[i]);
					break;
				default:
					throw new InvalidOperationException();
			}
		}
	}

	internal static void Compile(DynamicTypeWrapper.FinishContext context, DynamicTypeWrapper clazz, MethodWrapper mw, ClassFile classFile, ClassFile.Method m, CodeEmitter ilGenerator, ref bool nonleaf)
	{
		ClassLoaderWrapper classLoader = clazz.GetClassLoader();
		if(classLoader.EmitDebugInfo)
		{
			if(classFile.SourcePath != null)
			{
				ilGenerator.DefineSymbolDocument(classLoader.GetTypeWrapperFactory().ModuleBuilder, classFile.SourcePath, SymLanguageType.Java, Guid.Empty, SymDocumentType.Text);
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
						ilGenerator.SetLineNumber((ushort)firstLine);
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
				ilGenerator.EmitLdarg(i + (m.IsStatic ? 0 : 1));
				ilGenerator.Emit(OpCodes.Ldstr, args[i].Name);
				context.EmitCallerID(ilGenerator);
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
				c = new Compiler(context, clazz, mw, classFile, m, ilGenerator, classLoader);
			}
			finally
			{
				Profiler.Leave("new Compiler");
			}
		}
		catch(VerifyError x)
		{
#if STATIC_COMPILER
			classLoader.IssueMessage(Message.EmittedVerificationError, classFile.Name + "." + m.Name + m.Signature, x.Message);
#endif
			Tracer.Error(Tracer.Verifier, x.ToString());
			clazz.SetHasVerifyError();
			// because in Java the method is only verified if it is actually called,
			// we generate code here to throw the VerificationError
			ilGenerator.EmitThrow("java.lang.VerifyError", x.Message);
			return;
		}
		catch(ClassFormatError x)
		{
#if STATIC_COMPILER
			classLoader.IssueMessage(Message.EmittedClassFormatError, classFile.Name + "." + m.Name + m.Signature, x.Message);
#endif
			Tracer.Error(Tracer.Verifier, x.ToString());
			clazz.SetHasClassFormatError();
			ilGenerator.EmitThrow("java.lang.ClassFormatError", x.Message);
			return;
		}
		Profiler.Enter("Compile");
		try
		{
			if(m.IsSynchronized && m.IsStatic)
			{
				clazz.EmitClassLiteral(ilGenerator);
				ilGenerator.Emit(OpCodes.Dup);
				CodeEmitterLocal monitor = ilGenerator.DeclareLocal(Types.Object);
				ilGenerator.Emit(OpCodes.Stloc, monitor);
				ilGenerator.EmitMonitorEnter();
				ilGenerator.BeginExceptionBlock();
				Block b = new Block(c, 0, int.MaxValue, -1, new List<object>(), true);
				c.Compile(b, 0);
				b.Leave();
				ilGenerator.BeginFinallyBlock();
				ilGenerator.Emit(OpCodes.Ldloc, monitor);
				ilGenerator.EmitMonitorExit();
				ilGenerator.Emit(OpCodes.Endfinally);
				ilGenerator.EndExceptionBlock();
				b.LeaveStubs(new Block(c, 0, int.MaxValue, -1, null, false));
			}
			else
			{
				Block b = new Block(c, 0, int.MaxValue, -1, null, false);
				c.Compile(b, 0);
				b.Leave();
			}
			nonleaf = c.nonleaf;
		}
		finally
		{
			Profiler.Leave("Compile");
		}
	}

	private sealed class Block
	{
		private readonly Compiler compiler;
		private readonly CodeEmitter ilgen;
		private readonly int beginIndex;
		private readonly int endIndex;
		private readonly int exceptionIndex;
		private List<object> exits;
		private readonly bool nested;
		private readonly object[] labels;

		internal Block(Compiler compiler, int beginIndex, int endIndex, int exceptionIndex, List<object> exits, bool nested)
		{
			this.compiler = compiler;
			this.ilgen = compiler.ilGenerator;
			this.beginIndex = beginIndex;
			this.endIndex = endIndex;
			this.exceptionIndex = exceptionIndex;
			this.exits = exits;
			this.nested = nested;
			labels = new object[compiler.m.Instructions.Length];
		}

		internal int EndIndex
		{
			get
			{
				return endIndex;
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

		internal CodeEmitterLabel GetLabel(int targetIndex)
		{
			if(IsInRange(targetIndex))
			{
				CodeEmitterLabel l = (CodeEmitterLabel)labels[targetIndex];
				if(l == null)
				{
					l = ilgen.DefineLabel();
					labels[targetIndex] = l;
				}
				return l;
			}
			else
			{
				BranchCookie l = (BranchCookie)labels[targetIndex];
				if(l == null)
				{
					// if we're branching out of the current exception block, we need to indirect this thru a stub
					// that saves the stack and uses leave to leave the exception block (to another stub that recovers
					// the stack)
					int stackHeight = compiler.ma.GetStackHeight(targetIndex);
					BranchCookie bc = new BranchCookie(compiler, stackHeight, targetIndex);
					bc.ContentOnStack = true;
					for(int i = 0; i < stackHeight; i++)
					{
						bc.dh.SetType(i, compiler.ma.GetRawStackTypeWrapper(targetIndex, i));
					}
					exits.Add(bc);
					l = bc;
					labels[targetIndex] = l;
				}
				return l.Stub;
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

		internal bool IsInRange(int index)
		{
			return beginIndex <= index && index < endIndex;
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
						if(bc.TargetIndex == -1)
						{
							ilgen.EmitBr(bc.TargetLabel);
						}
						else
						{
							bc.Stub = ilgen.DefineLabel();
							ilgen.EmitLeave(bc.Stub);
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
						if(bc != null && bc.TargetIndex != -1)
						{
							Debug.Assert(!bc.ContentOnStack);
							// if the target is within the new block, we handle it, otherwise we
							// defer the cookie to our caller
							if(newBlock.IsInRange(bc.TargetIndex))
							{
								bc.ContentOnStack = true;
								ilgen.MarkLabel(bc.Stub);
								int stack = bc.dh.Count;
								for(int n = stack - 1; n >= 0; n--)
								{
									bc.dh.Load(n);
								}
								ilgen.EmitBr(newBlock.GetLabel(bc.TargetIndex));
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

	private void Compile(Block block, int startIndex)
	{
		InstructionFlags[] flags = ComputePartialReachability(startIndex, true);
		ExceptionTableEntry[] exceptions = GetExceptionTableFor(flags);
		int exceptionIndex = 0;
		Instruction[] code = m.Instructions;
		Stack<Block> blockStack = new Stack<Block>();
		bool instructionIsForwardReachable = true;
		if(startIndex != 0)
		{
			for(int i = 0; i < flags.Length; i++)
			{
				if((flags[i] & InstructionFlags.Reachable) != 0)
				{
					if(i < startIndex)
					{
						instructionIsForwardReachable = false;
						ilGenerator.EmitBr(block.GetLabel(startIndex));
					}
					break;
				}
			}
		}
		for(int i = 0; i < code.Length; i++)
		{
			Instruction instr = code[i];

			if (scopeBegin != null)
			{
				for(int j = scopeClose[i]; j > 0; j--)
				{
					ilGenerator.EndScope();
				}
				for(int j = scopeBegin[i]; j > 0; j--)
				{
					ilGenerator.BeginScope();
				}
			}

			// if we've left the current exception block, do the exit processing
			while(block.EndIndex == i)
			{
				block.Leave();

				ExceptionTableEntry exc = exceptions[block.ExceptionIndex];

				Block prevBlock = block;
				block = blockStack.Pop();

				exceptionIndex = block.ExceptionIndex + 1;
				// skip over exception handlers that are no longer relevant
				for(; exceptionIndex < exceptions.Length && exceptions[exceptionIndex].endIndex <= i; exceptionIndex++)
				{
				}

				int handlerIndex = exc.handlerIndex;

				if(exc.catch_type == 0 && VerifierTypeWrapper.IsFaultBlockException(ma.GetRawStackTypeWrapper(handlerIndex, 0)))
				{
					if(exc.isFinally)
					{
						ilGenerator.BeginFinallyBlock();
					}
					else
					{
						ilGenerator.BeginFaultBlock();
					}
					Block b = new Block(this, 0, block.EndIndex, -1, null, false);
					Compile(b, handlerIndex);
					b.Leave();
					ilGenerator.EndExceptionBlock();
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
						ilGenerator.BeginCatchBlock(Types.Exception);
					}
					BranchCookie bc = new BranchCookie(this, 1, exc.handlerIndex);
					prevBlock.AddExitHack(bc);
					Instruction handlerInstr = code[handlerIndex];
					bool unusedException = (handlerInstr.NormalizedOpCode == NormalizedByteCode.__pop ||
						(handlerInstr.NormalizedOpCode == NormalizedByteCode.__astore &&
						localVars.GetLocalVar(handlerIndex) == null));
					int mapFlags = unusedException ? 2 : 0;
					if(mapSafe && unusedException)
					{
						// we don't need to do anything with the exception
					}
					else if(mapSafe)
					{
						ilGenerator.EmitLdc_I4(mapFlags | 1);
						ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.mapException.MakeGenericMethod(excType));
					}
					else if(exceptionTypeWrapper == java_lang_Throwable)
					{
						ilGenerator.EmitLdc_I4(mapFlags);
						ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.mapException.MakeGenericMethod(Types.Exception));
					}
					else
					{
						ilGenerator.EmitLdc_I4(mapFlags | (remap ? 0 : 1));
						ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.mapException.MakeGenericMethod(excType));
						if(!unusedException)
						{
							ilGenerator.Emit(OpCodes.Dup);
						}
						if(exceptionTypeWrapper.IsUnloadable)
						{
							Profiler.Count("EmitDynamicExceptionHandler");
							ilGenerator.Emit(OpCodes.Ldstr, exceptionTypeWrapper.Name);
							context.EmitCallerID(ilGenerator);
							ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicInstanceOf);
						}
						CodeEmitterLabel leave = ilGenerator.DefineLabel();
						ilGenerator.EmitBrtrue(leave);
						ilGenerator.Emit(OpCodes.Rethrow);
						ilGenerator.MarkLabel(leave);
					}
					if(unusedException)
					{
						// we must still have an item on the stack, even though it isn't used!
						bc.dh.SetType(0, VerifierTypeWrapper.Null);
					}
					else
					{
						bc.dh.SetType(0, exceptionTypeWrapper);
						bc.dh.Store(0);
					}
					ilGenerator.EmitLeave(bc.Stub);
					ilGenerator.EndExceptionBlock();
				}
				prevBlock.LeaveStubs(block);
			}

			if((flags[i] & InstructionFlags.Reachable) == 0)
			{
				// skip any unreachable instructions
				continue;
			}

			// if there was a forward branch to this instruction, it is forward reachable
			instructionIsForwardReachable |= block.HasLabel(i);

			if(block.HasLabel(i) || (flags[i] & InstructionFlags.BranchTarget) != 0)
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
			for(; exceptionIndex < exceptions.Length && exceptions[exceptionIndex].startIndex == i; exceptionIndex++)
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
				block = new Block(this, exceptions[exceptionIndex].startIndex, exceptions[exceptionIndex].endIndex, exceptionIndex, new List<object>(), true);
				block.MarkLabel(i);
			}

			if(emitLineNumbers)
			{
				ClassFile.Method.LineNumberTableEntry[] table = m.LineNumberTableAttribute;
				for (int j = 0; j < table.Length; j++)
				{
					if(table[j].start_pc == code[i].PC && table[j].line_number != 0)
					{
						ilGenerator.SetLineNumber(table[j].line_number);
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
				switch(ByteCodeMetaData.GetFlowControl(instr.NormalizedOpCode))
				{
					case ByteCodeFlowControl.Return:
						ilGenerator.Emit(OpCodes.Ldarg_0);
						ilGenerator.Emit(OpCodes.Call, keepAliveMethod);
						break;
					case ByteCodeFlowControl.Branch:
					case ByteCodeFlowControl.CondBranch:
						if(instr.TargetIndex <= i)
						{
							ilGenerator.Emit(OpCodes.Ldarg_0);
							ilGenerator.Emit(OpCodes.Call, keepAliveMethod);
						}
						break;
					case ByteCodeFlowControl.Throw:
					case ByteCodeFlowControl.Switch:
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
					ilGenerator.Emit(OpCodes.Ldnull);
					break;
				case NormalizedByteCode.__iconst:
					ilGenerator.EmitLdc_I4(instr.NormalizedArg1);
					break;
				case NormalizedByteCode.__lconst_0:
					ilGenerator.EmitLdc_I8(0L);
					break;
				case NormalizedByteCode.__lconst_1:
					ilGenerator.EmitLdc_I8(1L);
					break;
				case NormalizedByteCode.__fconst_0:
				case NormalizedByteCode.__dconst_0:
					// floats are stored as native size on the stack, so both R4 and R8 are the same
					ilGenerator.EmitLdc_R4(0.0f);
					break;
				case NormalizedByteCode.__fconst_1:
				case NormalizedByteCode.__dconst_1:
					// floats are stored as native size on the stack, so both R4 and R8 are the same
					ilGenerator.EmitLdc_R4(1.0f);
					break;
				case NormalizedByteCode.__fconst_2:
					ilGenerator.EmitLdc_R4(2.0f);
					break;
				case NormalizedByteCode.__ldc_nothrow:
				case NormalizedByteCode.__ldc:
					EmitLoadConstant(ilGenerator, instr.Arg1);
					break;
				case NormalizedByteCode.__invokedynamic:
				{
					ClassFile.ConstantPoolItemInvokeDynamic cpi = classFile.GetInvokeDynamic(instr.Arg1);
					CastInterfaceArgs(null, cpi.GetArgTypes(), i, false);
					EmitInvokeDynamic(cpi);
					EmitReturnTypeConversion(cpi.GetRetType());
					nonleaf = true;
					break;
				}
				case NormalizedByteCode.__dynamic_invokestatic:
				case NormalizedByteCode.__invokestatic:
				{
					MethodWrapper method = GetMethodCallEmitter(instr.NormalizedOpCode, instr.Arg1);
					if(method.IsIntrinsic && method.EmitIntrinsic(new EmitIntrinsicContext(method, context, ilGenerator, ma, i, mw, classFile, code, flags)))
					{
						break;
					}
					// if the stack values don't match the argument types (for interface argument types)
					// we must emit code to cast the stack value to the interface type
					CastInterfaceArgs(method.DeclaringType, method.GetParameters(), i, false);
					if(method.HasCallerID)
					{
						context.EmitCallerID(ilGenerator);
					}
					method.EmitCall(ilGenerator);
					EmitReturnTypeConversion(method.ReturnType);
					nonleaf = true;
					break;
				}
				case NormalizedByteCode.__dynamic_invokeinterface:
				case NormalizedByteCode.__dynamic_invokevirtual:
				case NormalizedByteCode.__dynamic_invokespecial:
				case NormalizedByteCode.__invokevirtual:
				case NormalizedByteCode.__invokeinterface:
				case NormalizedByteCode.__invokespecial:
				case NormalizedByteCode.__methodhandle_invoke:
				case NormalizedByteCode.__methodhandle_invokeexact:
				{
					bool isinvokespecial = instr.NormalizedOpCode == NormalizedByteCode.__invokespecial || instr.NormalizedOpCode == NormalizedByteCode.__dynamic_invokespecial;
					MethodWrapper method = GetMethodCallEmitter(instr.NormalizedOpCode, instr.Arg1);
					int argcount = method.GetParameters().Length;
					TypeWrapper type = ma.GetRawStackTypeWrapper(i, argcount);
					TypeWrapper thisType = SigTypeToClassName(type, method.DeclaringType);

					EmitIntrinsicContext eic = new EmitIntrinsicContext(method, context, ilGenerator, ma, i, mw, classFile, code, flags);
					if(method.IsIntrinsic && method.EmitIntrinsic(eic))
					{
						nonleaf |= eic.NonLeaf;
						break;
					}

					nonleaf = true;

					// HACK this code is duplicated in java.lang.invoke.cs
					if(method.IsProtected && (method.DeclaringType == java_lang_Object || method.DeclaringType == java_lang_Throwable))
					{
						// HACK we may need to redirect finalize or clone from java.lang.Object/Throwable
						// to a more specific base type.
						if(thisType.IsAssignableTo(cli_System_Object))
						{
							method = cli_System_Object.GetMethodWrapper(method.Name, method.Signature, true);
						}
						else if(thisType.IsAssignableTo(cli_System_Exception))
						{
							method = cli_System_Exception.GetMethodWrapper(method.Name, method.Signature, true);
						}
						else if(thisType.IsAssignableTo(java_lang_Throwable))
						{
							method = java_lang_Throwable.GetMethodWrapper(method.Name, method.Signature, true);
						}
					}

					// if the stack values don't match the argument types (for interface argument types)
					// we must emit code to cast the stack value to the interface type
					if(isinvokespecial && method.IsConstructor && VerifierTypeWrapper.IsNew(type))
					{
						CastInterfaceArgs(method.DeclaringType, method.GetParameters(), i, false);
					}
					else
					{
						// the this reference is included in the argument list because it may also need to be cast
						TypeWrapper[] methodArgs = method.GetParameters();
						TypeWrapper[] args = new TypeWrapper[methodArgs.Length + 1];
						methodArgs.CopyTo(args, 1);
						if(instr.NormalizedOpCode == NormalizedByteCode.__invokeinterface)
						{
							args[0] = method.DeclaringType;
						}
						else
						{
							args[0] = thisType;
						}
						CastInterfaceArgs(method.DeclaringType, args, i, true);
					}

					if(isinvokespecial && method.IsConstructor)
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
								if(code[i + 1].NormalizedOpCode == NormalizedByteCode.__athrow)
								{
									if(thisType.GetMethodWrapper("fillInStackTrace", "()Ljava.lang.Throwable;", true).DeclaringType == java_lang_Throwable)
									{
										ilGenerator.Emit(OpCodes.Call, suppressFillInStackTraceMethod);
									}
									if((flags[i + 1] & InstructionFlags.BranchTarget) == 0)
									{
										code[i + 1].PatchOpCode(NormalizedByteCode.__athrow_no_unmap);
									}
								}
							}
							method.EmitNewobj(ilGenerator);
							if(!thisType.IsUnloadable && thisType.IsSubTypeOf(cli_System_Exception))
							{
								// we call Throwable.__<fixate>() to disable remapping the exception
								ilGenerator.Emit(OpCodes.Call, fixateExceptionMethod);
							}
							if(nontrivial)
							{
								// this could be done a little more efficiently, but since in practice this
								// code never runs (for code compiled from Java source) it doesn't
								// really matter
								CodeEmitterLocal newobj = ilGenerator.DeclareLocal(GetLocalBuilderType(thisType));
								ilGenerator.Emit(OpCodes.Stloc, newobj);
								CodeEmitterLocal[] tempstack = new CodeEmitterLocal[stackfix.Length];
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
										else if(!VerifierTypeWrapper.IsNotPresentOnStack(stacktype))
										{
											CodeEmitterLocal lb = ilGenerator.DeclareLocal(GetLocalBuilderType(stacktype));
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
								LocalVar[] locals = localVars.GetLocalVarsForInvokeSpecial(i);
								for(int j = 0; j < locals.Length; j++)
								{
									if(locals[j] != null)
									{
										if(locals[j].builder == null)
										{
											// for invokespecial the resulting type can never be null
											locals[j].builder = ilGenerator.DeclareLocal(GetLocalBuilderType(locals[j].type));
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
							LocalVar[] locals = localVars.GetLocalVarsForInvokeSpecial(i);
							for(int j = 0; j < locals.Length; j++)
							{
								if(locals[j] != null)
								{
									if(locals[j].builder == null)
									{
										// for invokespecial the resulting type can never be null
										locals[j].builder = ilGenerator.DeclareLocal(GetLocalBuilderType(locals[j].type));
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
							context.EmitCallerID(ilGenerator);
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
								ilGenerator.Emit(OpCodes.Callvirt, context.GetInvokeSpecialStub(method));
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
						EmitReturnTypeConversion(method.ReturnType);
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
						CodeEmitterLocal local = null;
						if(instr.NormalizedOpCode != NormalizedByteCode.__return)
						{
							TypeWrapper retTypeWrapper = mw.ReturnType;
							retTypeWrapper.EmitConvStackTypeToSignatureType(ilGenerator, ma.GetStackTypeWrapper(i, 0));
							local = ilGenerator.UnsafeAllocTempLocal(retTypeWrapper.TypeAsSignatureType);
							ilGenerator.Emit(OpCodes.Stloc, local);
						}
						CodeEmitterLabel label = ilGenerator.DefineLabel();
						// NOTE leave automatically discards any junk that may be on the stack
						ilGenerator.EmitLeave(label);
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
#if !NET_4_0
						if(exceptions.Length == 0 && i > 0)
						{
							int k = i - 1;
							while(k > 0 && code[k].NormalizedOpCode == NormalizedByteCode.__nop)
							{
								k--;
							}
							switch(code[k].NormalizedOpCode)
							{
								case NormalizedByteCode.__invokeinterface:
								case NormalizedByteCode.__invokespecial:
								case NormalizedByteCode.__invokestatic:
								case NormalizedByteCode.__invokevirtual:
									x64hack = true;
									break;
							}
						}
#endif
						// if there is junk on the stack (other than the return value), we must pop it off
						// because in .NET this is invalid (unlike in Java)
						int stackHeight = ma.GetStackHeight(i);
						if(instr.NormalizedOpCode == NormalizedByteCode.__return)
						{
							if(stackHeight != 0 || x64hack)
							{
								ilGenerator.EmitClearStack();
							}
							ilGenerator.Emit(OpCodes.Ret);
						}
						else
						{
							TypeWrapper retTypeWrapper = mw.ReturnType;
							retTypeWrapper.EmitConvStackTypeToSignatureType(ilGenerator, ma.GetStackTypeWrapper(i, 0));
							if(stackHeight != 1)
							{
								CodeEmitterLocal local = ilGenerator.AllocTempLocal(retTypeWrapper.TypeAsSignatureType);
								ilGenerator.Emit(OpCodes.Stloc, local);
								ilGenerator.EmitClearStack();
								ilGenerator.Emit(OpCodes.Ldloc, local);
								ilGenerator.ReleaseTempLocal(local);
							}
							else if(x64hack)
							{
								ilGenerator.EmitTailCallPrevention();
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
					else if(VerifierTypeWrapper.IsNotPresentOnStack(type))
					{
						// since object isn't represented on the stack, we don't need to do anything here
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
						LocalVar v = LoadLocal(i);
						if(!type.IsUnloadable && (v.type.IsUnloadable || !v.type.IsAssignableTo(type)))
						{
							type.EmitCheckcast(ilGenerator);
						}
					}
					break;
				}
				case NormalizedByteCode.__astore:
				{
					TypeWrapper type = ma.GetRawStackTypeWrapper(i, 0);
					if(VerifierTypeWrapper.IsNotPresentOnStack(type))
					{
						// object isn't really on the stack, so we can't copy it into the local
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
						StoreLocal(i);
					}
					break;
				}
				case NormalizedByteCode.__iload:
				case NormalizedByteCode.__lload:
				case NormalizedByteCode.__fload:
				case NormalizedByteCode.__dload:
					LoadLocal(i);
					break;
				case NormalizedByteCode.__istore:
				case NormalizedByteCode.__lstore:
					StoreLocal(i);
					break;
				case NormalizedByteCode.__fstore:
					StoreLocal(i);
					break;
				case NormalizedByteCode.__dstore:
					if(ma.IsStackTypeExtendedDouble(i, 0))
					{
						ilGenerator.Emit(OpCodes.Conv_R8);
					}
					StoreLocal(i);
					break;
				case NormalizedByteCode.__new:
				{
					TypeWrapper wrapper = classFile.GetConstantPoolClassType(instr.Arg1);
					if(wrapper.IsUnloadable)
					{
						Profiler.Count("EmitDynamicNewCheckOnly");
						// this is here to make sure we throw the exception in the right location (before
						// evaluating the constructor arguments)
						ilGenerator.Emit(OpCodes.Ldstr, wrapper.Name);
						context.EmitCallerID(ilGenerator);
						ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicNewCheckOnly);
					}
					else if(wrapper != clazz && RequiresExplicitClassInit(wrapper, i + 1, flags))
					{
						// trigger cctor (as the spec requires)
						wrapper.EmitRunClassConstructor(ilGenerator);
					}
					// we don't actually allocate the object here, the call to <init> will be converted into a newobj instruction
					break;
				}
				case NormalizedByteCode.__multianewarray:
				{
					CodeEmitterLocal localArray = ilGenerator.UnsafeAllocTempLocal(JVM.Import(typeof(int[])));
					CodeEmitterLocal localInt = ilGenerator.UnsafeAllocTempLocal(Types.Int32);
					ilGenerator.EmitLdc_I4(instr.Arg2);
					ilGenerator.Emit(OpCodes.Newarr, Types.Int32);
					ilGenerator.Emit(OpCodes.Stloc, localArray);
					for(int j = 1; j <= instr.Arg2; j++)
					{
						ilGenerator.Emit(OpCodes.Stloc, localInt);
						ilGenerator.Emit(OpCodes.Ldloc, localArray);
						ilGenerator.EmitLdc_I4(instr.Arg2 - j);
						ilGenerator.Emit(OpCodes.Ldloc, localInt);
						ilGenerator.Emit(OpCodes.Stelem_I4);
					}
					TypeWrapper wrapper = classFile.GetConstantPoolClassType(instr.Arg1);
					if(wrapper.IsUnloadable)
					{
						Profiler.Count("EmitDynamicMultianewarray");
						ilGenerator.Emit(OpCodes.Ldstr, wrapper.Name);
						ilGenerator.Emit(OpCodes.Ldloc, localArray);
						context.EmitCallerID(ilGenerator);
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
						ilGenerator.Emit(OpCodes.Ldstr, wrapper.Name);
						context.EmitCallerID(ilGenerator);
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
					if(wrapper.IsUnloadable)
					{
						ilGenerator.Emit(OpCodes.Ldstr, wrapper.Name);
						context.EmitCallerID(ilGenerator);
						ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicCast);
					}
					else
					{
						wrapper.EmitCheckcast(ilGenerator);
					}
					break;
				}
				case NormalizedByteCode.__instanceof:
				{
					TypeWrapper wrapper = classFile.GetConstantPoolClassType(instr.Arg1);
					if(wrapper.IsUnloadable)
					{
						ilGenerator.Emit(OpCodes.Ldstr, wrapper.Name);
						context.EmitCallerID(ilGenerator);
						ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicInstanceOf);
					}
					else
					{
						wrapper.EmitInstanceOf(ilGenerator);
					}
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
				case NormalizedByteCode.__fastore:
					ilGenerator.Emit(OpCodes.Stelem_R4);
					break;
				case NormalizedByteCode.__daload:
					ilGenerator.Emit(OpCodes.Ldelem_R8);
					break;
				case NormalizedByteCode.__dastore:
					if(ma.IsStackTypeExtendedDouble(i, 0))
					{
						ilGenerator.Emit(OpCodes.Conv_R8);
					}
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
							CodeEmitterLocal local = ilGenerator.UnsafeAllocTempLocal(Types.Object);
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
						ilGenerator.Emit(OpCodes.Castclass, Types.Array);
						ilGenerator.Emit(OpCodes.Callvirt, Types.Array.GetMethod("get_Length"));
					}
					else
					{
						ilGenerator.Emit(OpCodes.Ldlen);
					}
					break;
				case NormalizedByteCode.__lcmp:
					ilGenerator.Emit_lcmp();
					break;
				case NormalizedByteCode.__fcmpl:
					ilGenerator.Emit_fcmpl();
					break;
				case NormalizedByteCode.__fcmpg:
					ilGenerator.Emit_fcmpg();
					break;
				case NormalizedByteCode.__dcmpl:
					ilGenerator.Emit_dcmpl();
					break;
				case NormalizedByteCode.__dcmpg:
					ilGenerator.Emit_dcmpg();
					break;
				case NormalizedByteCode.__if_icmpeq:
					ilGenerator.EmitBeq(block.GetLabel(instr.TargetIndex));
					break;
				case NormalizedByteCode.__if_icmpne:
					ilGenerator.EmitBne_Un(block.GetLabel(instr.TargetIndex));
					break;
				case NormalizedByteCode.__if_icmple:
					ilGenerator.EmitBle(block.GetLabel(instr.TargetIndex));
					break;
				case NormalizedByteCode.__if_icmplt:
					ilGenerator.EmitBlt(block.GetLabel(instr.TargetIndex));
					break;
				case NormalizedByteCode.__if_icmpge:
					ilGenerator.EmitBge(block.GetLabel(instr.TargetIndex));
					break;
				case NormalizedByteCode.__if_icmpgt:
					ilGenerator.EmitBgt(block.GetLabel(instr.TargetIndex));
					break;
				case NormalizedByteCode.__ifle:
					ilGenerator.Emit_if_le_lt_ge_gt(CodeEmitter.Comparison.LessOrEqual, block.GetLabel(instr.TargetIndex));
					break;
				case NormalizedByteCode.__iflt:
					ilGenerator.Emit_if_le_lt_ge_gt(CodeEmitter.Comparison.LessThan, block.GetLabel(instr.TargetIndex));
					break;
				case NormalizedByteCode.__ifge:
					ilGenerator.Emit_if_le_lt_ge_gt(CodeEmitter.Comparison.GreaterOrEqual, block.GetLabel(instr.TargetIndex));
					break;
				case NormalizedByteCode.__ifgt:
					ilGenerator.Emit_if_le_lt_ge_gt(CodeEmitter.Comparison.GreaterThan, block.GetLabel(instr.TargetIndex));
					break;
				case NormalizedByteCode.__ifne:
				case NormalizedByteCode.__ifnonnull:
					ilGenerator.EmitBrtrue(block.GetLabel(instr.TargetIndex));
					break;
				case NormalizedByteCode.__ifeq:
				case NormalizedByteCode.__ifnull:
					ilGenerator.EmitBrfalse(block.GetLabel(instr.TargetIndex));
					break;
				case NormalizedByteCode.__if_acmpeq:
					ilGenerator.EmitBeq(block.GetLabel(instr.TargetIndex));
					break;
				case NormalizedByteCode.__if_acmpne:
					ilGenerator.EmitBne_Un(block.GetLabel(instr.TargetIndex));
					break;
				case NormalizedByteCode.__goto:
				case NormalizedByteCode.__goto_finally:
					ilGenerator.EmitBr(block.GetLabel(instr.TargetIndex));
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
					ilGenerator.Emit_idiv();
					break;
				case NormalizedByteCode.__ldiv:
					ilGenerator.Emit_ldiv();
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
					ilGenerator.EmitBne_Un(label);
					ilGenerator.Emit(OpCodes.Pop);
					ilGenerator.Emit(OpCodes.Pop);
					ilGenerator.Emit(OpCodes.Ldc_I4_0);
					if(instr.NormalizedOpCode == NormalizedByteCode.__lrem)
					{
						ilGenerator.Emit(OpCodes.Conv_I8);
					}
					CodeEmitterLabel label2 = ilGenerator.DefineLabel();
					ilGenerator.EmitBr(label2);
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
					ilGenerator.Emit_And_I4(31);
					ilGenerator.Emit(OpCodes.Shl);
					break;
				case NormalizedByteCode.__lshl:
					ilGenerator.Emit_And_I4(63);
					ilGenerator.Emit(OpCodes.Shl);
					break;
				case NormalizedByteCode.__iushr:
					ilGenerator.Emit_And_I4(31);
					ilGenerator.Emit(OpCodes.Shr_Un);
					break;
				case NormalizedByteCode.__lushr:
					ilGenerator.Emit_And_I4(63);
					ilGenerator.Emit(OpCodes.Shr_Un);
					break;
				case NormalizedByteCode.__ishr:
					ilGenerator.Emit_And_I4(31);
					ilGenerator.Emit(OpCodes.Shr);
					break;
				case NormalizedByteCode.__lshr:
					ilGenerator.Emit_And_I4(63);
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
					// if the TOS contains a "new" object or a fault block exception, it isn't really there, so we don't dup it
					if(!VerifierTypeWrapper.IsNotPresentOnStack(ma.GetRawStackTypeWrapper(i, 0)))
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
						ilGenerator.Emit(OpCodes.Pop);
					}
					else
					{
						if (!VerifierTypeWrapper.IsNotPresentOnStack(type1))
						{
							ilGenerator.Emit(OpCodes.Pop);
						}
						if (!VerifierTypeWrapper.IsNotPresentOnStack(ma.GetRawStackTypeWrapper(i, 1)))
						{
							ilGenerator.Emit(OpCodes.Pop);
						}
					}
					break;
				}
				case NormalizedByteCode.__pop:
					// if the TOS is a new object or a fault block exception, it isn't really there, so we don't need to pop it
					if(!VerifierTypeWrapper.IsNotPresentOnStack(ma.GetRawStackTypeWrapper(i, 0)))
					{
						ilGenerator.Emit(OpCodes.Pop);
					}
					break;
				case NormalizedByteCode.__monitorenter:
					ilGenerator.EmitMonitorEnter();
					break;
				case NormalizedByteCode.__monitorexit:
					ilGenerator.EmitMonitorExit();
					break;
				case NormalizedByteCode.__athrow_no_unmap:
					if(ma.GetRawStackTypeWrapper(i, 0).IsUnloadable)
					{
						ilGenerator.Emit(OpCodes.Castclass, Types.Exception);
					}
					ilGenerator.Emit(OpCodes.Throw);
					break;
				case NormalizedByteCode.__athrow:
					if (VerifierTypeWrapper.IsFaultBlockException(ma.GetRawStackTypeWrapper(i, 0)))
					{
						ilGenerator.Emit(OpCodes.Endfinally);
					}
					else
					{
						if (ma.GetRawStackTypeWrapper(i, 0).IsUnloadable)
						{
							ilGenerator.Emit(OpCodes.Castclass, Types.Exception);
						}
						ilGenerator.Emit(OpCodes.Call, unmapExceptionMethod);
						ilGenerator.Emit(OpCodes.Throw);
					}
					break;
				case NormalizedByteCode.__tableswitch:
				{
					// note that a tableswitch always has at least one entry
					// (otherwise it would have failed verification)
					CodeEmitterLabel[] labels = new CodeEmitterLabel[instr.SwitchEntryCount];
					for(int j = 0; j < labels.Length; j++)
					{
						labels[j] = block.GetLabel(instr.GetSwitchTargetIndex(j));
					}
					if(instr.GetSwitchValue(0) != 0)
					{
						ilGenerator.EmitLdc_I4(instr.GetSwitchValue(0));
						ilGenerator.Emit(OpCodes.Sub);
					}
					ilGenerator.EmitSwitch(labels);
					ilGenerator.EmitBr(block.GetLabel(instr.DefaultTarget));
					break;
				}
				case NormalizedByteCode.__lookupswitch:
					for(int j = 0; j < instr.SwitchEntryCount; j++)
					{
						ilGenerator.Emit(OpCodes.Dup);
						ilGenerator.EmitLdc_I4(instr.GetSwitchValue(j));
						CodeEmitterLabel label = ilGenerator.DefineLabel();
						ilGenerator.EmitBne_Un(label);
						ilGenerator.Emit(OpCodes.Pop);
						ilGenerator.EmitBr(block.GetLabel(instr.GetSwitchTargetIndex(j)));
						ilGenerator.MarkLabel(label);
					}
					ilGenerator.Emit(OpCodes.Pop);
					ilGenerator.EmitBr(block.GetLabel(instr.DefaultTarget));
					break;
				case NormalizedByteCode.__iinc:
					LoadLocal(i);
					ilGenerator.EmitLdc_I4(instr.Arg2);
					ilGenerator.Emit(OpCodes.Add);
					StoreLocal(i);
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
				case NormalizedByteCode.__nop:
					ilGenerator.Emit(OpCodes.Nop);
					break;
				case NormalizedByteCode.__intrinsic_gettype:
					ilGenerator.Emit(OpCodes.Callvirt, getTypeMethod);
					break;
				case NormalizedByteCode.__static_error:
				{
					bool wrapIncompatibleClassChangeError = false;
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
						case HardError.NoSuchFieldException:
							exceptionType = ClassLoaderWrapper.LoadClassCritical("java.lang.NoSuchFieldException");
							wrapIncompatibleClassChangeError = true;
							break;
						case HardError.NoSuchMethodException:
							exceptionType = ClassLoaderWrapper.LoadClassCritical("java.lang.NoSuchMethodException");
							wrapIncompatibleClassChangeError = true;
							break;
						case HardError.IllegalAccessException:
							exceptionType = ClassLoaderWrapper.LoadClassCritical("java.lang.IllegalAccessException");
							wrapIncompatibleClassChangeError = true;
							break;
						default:
							throw new InvalidOperationException();
					}
					if(wrapIncompatibleClassChangeError)
					{
						ClassLoaderWrapper.LoadClassCritical("java.lang.IncompatibleClassChangeError").GetMethodWrapper("<init>", "()V", false).EmitNewobj(ilGenerator);
					}
					string message = harderrors[instr.HardErrorMessageId];
					Tracer.Error(Tracer.Compiler, "{0}: {1}\n\tat {2}.{3}{4}", exceptionType.Name, message, classFile.Name, m.Name, m.Signature);
					ilGenerator.Emit(OpCodes.Ldstr, message);
					MethodWrapper method = exceptionType.GetMethodWrapper("<init>", "(Ljava.lang.String;)V", false);
					method.Link();
					method.EmitNewobj(ilGenerator);
					if(wrapIncompatibleClassChangeError)
					{
						CoreClasses.java.lang.Throwable.Wrapper.GetMethodWrapper("initCause", "(Ljava.lang.Throwable;)Ljava.lang.Throwable;", false).EmitCallvirt(ilGenerator);
					}
					ilGenerator.Emit(OpCodes.Throw);
					break;
				}
				default:
					throw new NotImplementedException(instr.NormalizedOpCode.ToString());
			}
			// mark next instruction as inuse
			switch(ByteCodeMetaData.GetFlowControl(instr.NormalizedOpCode))
			{
				case ByteCodeFlowControl.Switch:
				case ByteCodeFlowControl.Branch:
				case ByteCodeFlowControl.Return:
				case ByteCodeFlowControl.Throw:
					instructionIsForwardReachable = false;
					break;
				case ByteCodeFlowControl.CondBranch:
				case ByteCodeFlowControl.Next:
					instructionIsForwardReachable = true;
					Debug.Assert((flags[i + 1] & InstructionFlags.Reachable) != 0);
					// don't fall through end of try block
					if(block.EndIndex == i + 1)
					{
						// TODO instead of emitting a branch to the leave stub, it would be more efficient to put the leave stub here
						ilGenerator.EmitBr(block.GetLabel(i + 1));
					}
					break;
				default:
					throw new InvalidOperationException();
			}
		}
	}

	private void EmitReturnTypeConversion(TypeWrapper returnType)
	{
		returnType.EmitConvSignatureTypeToStackType(ilGenerator);
		if (!strictfp)
		{
			// no need to convert
		}
		else if (returnType == PrimitiveTypeWrapper.DOUBLE)
		{
			ilGenerator.Emit(OpCodes.Conv_R8);
		}
		else if (returnType == PrimitiveTypeWrapper.FLOAT)
		{
			ilGenerator.Emit(OpCodes.Conv_R4);
		}
	}

	private void EmitLoadConstant(CodeEmitter ilgen, int constant)
	{
		switch (classFile.GetConstantPoolConstantType(constant))
		{
			case ClassFile.ConstantType.Double:
				ilgen.EmitLdc_R8(classFile.GetConstantPoolConstantDouble(constant));
				break;
			case ClassFile.ConstantType.Float:
				ilgen.EmitLdc_R4(classFile.GetConstantPoolConstantFloat(constant));
				break;
			case ClassFile.ConstantType.Integer:
				ilgen.EmitLdc_I4(classFile.GetConstantPoolConstantInteger(constant));
				break;
			case ClassFile.ConstantType.Long:
				ilgen.EmitLdc_I8(classFile.GetConstantPoolConstantLong(constant));
				break;
			case ClassFile.ConstantType.String:
				ilgen.Emit(OpCodes.Ldstr, classFile.GetConstantPoolConstantString(constant));
				break;
			case ClassFile.ConstantType.Class:
				EmitLoadClass(ilgen, classFile.GetConstantPoolClassType(constant));
				break;
			case ClassFile.ConstantType.MethodHandle:
				context.GetValue<MethodHandleConstant>(constant).Emit(this, ilgen, constant);
				break;
			case ClassFile.ConstantType.MethodType:
				context.GetValue<MethodTypeConstant>(constant).Emit(this, ilgen, constant);
				break;
			default:
				throw new InvalidOperationException();
		}
	}

	private void EmitLoadClass(CodeEmitter ilgen, TypeWrapper tw)
	{
		if (tw.IsUnloadable)
		{
			Profiler.Count("EmitDynamicClassLiteral");
			ilgen.Emit(OpCodes.Ldstr, tw.Name);
			context.EmitCallerID(ilgen);
			ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicClassLiteral);
		}
		else
		{
			tw.EmitClassLiteral(ilgen);
		}
	}

	internal static bool HasUnloadable(TypeWrapper[] args, TypeWrapper ret)
	{
		TypeWrapper tw = ret;
		for (int i = 0; !tw.IsUnloadable && i < args.Length; i++)
		{
			tw = args[i];
		}
		return tw.IsUnloadable;
	}

	private static class InvokeDynamicBuilder
	{
		private static readonly Type typeofOpenIndyCallSite;
		private static readonly Type typeofCallSite;
		private static readonly MethodInfo methodLookup;

		static InvokeDynamicBuilder()
		{
			Type typeofMethodHandles;
#if STATIC_COMPILER
			typeofOpenIndyCallSite = StaticCompiler.GetRuntimeType("IKVM.Runtime.IndyCallSite`1");
			typeofCallSite = ClassLoaderWrapper.LoadClassCritical("java.lang.invoke.CallSite").TypeAsSignatureType;
			typeofMethodHandles = ClassLoaderWrapper.LoadClassCritical("java.lang.invoke.MethodHandles").TypeAsBaseType;
#elif FIRST_PASS
			typeofMethodHandles = null;
#else
			typeofOpenIndyCallSite = typeof(IKVM.Runtime.IndyCallSite<>);
			typeofCallSite = typeof(java.lang.invoke.CallSite);
			typeofMethodHandles = typeof(java.lang.invoke.MethodHandles);
#endif
			methodLookup = typeofMethodHandles.GetMethod("lookup", new Type[] { CoreClasses.ikvm.@internal.CallerID.Wrapper.TypeAsSignatureType });
		}

		internal static void Emit(Compiler compiler, ClassFile.ConstantPoolItemInvokeDynamic cpi, Type delegateType)
		{
			Type typeofIndyCallSite = typeofOpenIndyCallSite.MakeGenericType(delegateType);
			MethodInfo methodCreateBootStrap;
			MethodInfo methodGetTarget;
			if (ReflectUtil.ContainsTypeBuilder(typeofIndyCallSite))
			{
				methodCreateBootStrap = TypeBuilder.GetMethod(typeofIndyCallSite, typeofOpenIndyCallSite.GetMethod("CreateBootstrap"));
				methodGetTarget = TypeBuilder.GetMethod(typeofIndyCallSite, typeofOpenIndyCallSite.GetMethod("GetTarget"));
			}
			else
			{
				methodCreateBootStrap = typeofIndyCallSite.GetMethod("CreateBootstrap");
				methodGetTarget = typeofIndyCallSite.GetMethod("GetTarget");
			}
			TypeBuilder tb = compiler.context.DefineIndyCallSiteType();
			FieldBuilder fb = tb.DefineField("value", typeofIndyCallSite, FieldAttributes.Static | FieldAttributes.Assembly);
			CodeEmitter ilgen = CodeEmitter.Create(ReflectUtil.DefineTypeInitializer(tb));
			ilgen.Emit(OpCodes.Ldnull);
			ilgen.Emit(OpCodes.Ldftn, CreateBootstrapStub(compiler, cpi, delegateType, tb, fb, methodGetTarget));
			ilgen.Emit(OpCodes.Newobj, MethodHandleUtil.GetDelegateConstructor(delegateType));
			ilgen.Emit(OpCodes.Call, methodCreateBootStrap);
			ilgen.Emit(OpCodes.Stsfld, fb);
			ilgen.Emit(OpCodes.Ret);
			ilgen.DoEmit();

			compiler.ilGenerator.Emit(OpCodes.Ldsfld, fb);
			compiler.ilGenerator.Emit(OpCodes.Call, methodGetTarget);
		}

		private static MethodBuilder CreateBootstrapStub(Compiler compiler, ClassFile.ConstantPoolItemInvokeDynamic cpi, Type delegateType, TypeBuilder tb, FieldBuilder fb, MethodInfo methodGetTarget)
		{
			TypeWrapper[] args = cpi.GetArgTypes();
			Type[] argTypes = new Type[args.Length];
			for (int i = 0; i < args.Length; i++)
			{
				argTypes[i] = args[i].TypeAsSignatureType;
			}
			MethodBuilder mb = tb.DefineMethod("BootstrapStub", MethodAttributes.Static | MethodAttributes.PrivateScope, cpi.GetRetType().TypeAsSignatureType, argTypes);
			CodeEmitter ilgen = CodeEmitter.Create(mb);
			CodeEmitterLocal cs = ilgen.DeclareLocal(typeofCallSite);
			CodeEmitterLocal ex = ilgen.DeclareLocal(Types.Exception);
			CodeEmitterLabel label = ilgen.DefineLabel();
			ilgen.BeginExceptionBlock();
			if (EmitCallBootstrapMethod(compiler, cpi, delegateType, ilgen))
			{
				ilgen.Emit(OpCodes.Isinst, typeofCallSite);
				ilgen.Emit(OpCodes.Stloc, cs);
			}
			ilgen.EmitLeave(label);
			ilgen.BeginCatchBlock(Types.Exception);
			ilgen.Emit(OpCodes.Stloc, ex);
			ilgen.EmitLeave(label);
			ilgen.BeginFinallyBlock();
			ilgen.Emit(OpCodes.Ldsflda, fb);
			ilgen.Emit(OpCodes.Ldloc, cs);
			ilgen.Emit(OpCodes.Ldloc, ex);
			ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.LinkIndyCallSite.MakeGenericMethod(delegateType));
			ilgen.Emit(OpCodes.Endfinally);
			ilgen.EndExceptionBlock();
			ilgen.MarkLabel(label);
			ilgen.Emit(OpCodes.Ldsfld, fb);
			ilgen.Emit(OpCodes.Call, methodGetTarget);
			for (int i = 0; i < args.Length; i++)
			{
				ilgen.EmitLdarg(i);
			}
			MethodHandleUtil.EmitCallDelegateInvokeMethod(ilgen, delegateType);
			ilgen.Emit(OpCodes.Ret);
			ilgen.DoEmit();
			return mb;
		}

		private static bool EmitCallBootstrapMethod(Compiler compiler, ClassFile.ConstantPoolItemInvokeDynamic cpi, Type delegateType, CodeEmitter ilgen)
		{
			ClassFile.BootstrapMethod bsm = compiler.classFile.GetBootstrapMethod(cpi.BootstrapMethod);
			if (3 + bsm.ArgumentCount > 255)
			{
				ilgen.EmitThrow("java.lang.BootstrapMethodError", "too many bootstrap method arguments");
				return false;
			}
			ClassFile.ConstantPoolItemMethodHandle mh = compiler.classFile.GetConstantPoolConstantMethodHandle(bsm.BootstrapMethodIndex);
			MethodWrapper mw = mh.Member as MethodWrapper;
			ClassFile.ConstantPoolItemMI cpiMI;
			if (mw == null && (cpiMI = mh.MemberConstantPoolItem as ClassFile.ConstantPoolItemMI) != null)
			{
				mw = new DynamicBinder().Get(compiler.context, ClassFile.RefKind.invokeStatic, cpiMI);
			}
			if (mw == null || !mw.IsStatic)
			{
				ilgen.EmitThrow("java.lang.invoke.WrongMethodTypeException");
				return false;
			}
			TypeWrapper[] parameters = mw.GetParameters();
			int extraArgs = parameters.Length - 3;
			int fixedArgs;
			int varArgs;
			if (extraArgs == 1 && parameters[3].IsArray && parameters[3].ElementTypeWrapper == CoreClasses.java.lang.Object.Wrapper)
			{
				fixedArgs = 0;
				varArgs = bsm.ArgumentCount - fixedArgs;
			}
			else if (extraArgs != bsm.ArgumentCount)
			{
				ilgen.EmitThrow("java.lang.invoke.WrongMethodTypeException");
				return false;
			}
			else
			{
				fixedArgs = extraArgs;
				varArgs = -1;
			}
			compiler.context.EmitCallerID(ilgen);
			ilgen.Emit(OpCodes.Call, methodLookup);
			ilgen.Emit(OpCodes.Ldstr, cpi.Name);
			parameters[1].EmitConvStackTypeToSignatureType(ilgen, CoreClasses.java.lang.String.Wrapper);
			ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.LoadMethodType.MakeGenericMethod(delegateType));
			parameters[2].EmitConvStackTypeToSignatureType(ilgen, CoreClasses.java.lang.invoke.MethodType.Wrapper);
			for (int i = 0; i < fixedArgs; i++)
			{
				EmitExtraArg(compiler, ilgen, bsm, i, parameters[i + 3]);
			}
			if (varArgs >= 0)
			{
				ilgen.EmitLdc_I4(varArgs);
				TypeWrapper elemType = parameters[parameters.Length - 1].ElementTypeWrapper;
				ilgen.Emit(OpCodes.Newarr, elemType.TypeAsArrayType);
				for (int i = 0; i < varArgs; i++)
				{
					ilgen.Emit(OpCodes.Dup);
					ilgen.EmitLdc_I4(i);
					EmitExtraArg(compiler, ilgen, bsm, i + fixedArgs, elemType);
					ilgen.Emit(OpCodes.Stelem_Ref);
				}
			}
			mw.EmitCall(ilgen);
			return true;
		}

		private static void EmitExtraArg(Compiler compiler, CodeEmitter ilgen, ClassFile.BootstrapMethod bsm, int index, TypeWrapper targetType)
		{
			int constant = bsm.GetArgument(index);
			compiler.EmitLoadConstant(ilgen, constant);
			TypeWrapper constType;
			switch (compiler.classFile.GetConstantPoolConstantType(constant))
			{
				case ClassFile.ConstantType.Integer:
					constType = PrimitiveTypeWrapper.INT;
					break;
				case ClassFile.ConstantType.Long:
					constType = PrimitiveTypeWrapper.LONG;
					break;
				case ClassFile.ConstantType.Float:
					constType = PrimitiveTypeWrapper.FLOAT;
					break;
				case ClassFile.ConstantType.Double:
					constType = PrimitiveTypeWrapper.DOUBLE;
					break;
				case ClassFile.ConstantType.Class:
					constType = CoreClasses.java.lang.Class.Wrapper;
					break;
				case ClassFile.ConstantType.String:
					constType = CoreClasses.java.lang.String.Wrapper;
					break;
				case ClassFile.ConstantType.MethodHandle:
					constType = CoreClasses.java.lang.invoke.MethodHandle.Wrapper;
					break;
				case ClassFile.ConstantType.MethodType:
					constType = CoreClasses.java.lang.invoke.MethodType.Wrapper;
					break;
				default:
					throw new InvalidOperationException();
			}
			if (constType != targetType)
			{
				if (constType.IsPrimitive)
				{
					string dummy;
					TypeWrapper wrapper = GetWrapperType(constType, out dummy);
					wrapper.GetMethodWrapper("valueOf", "(" + constType.SigName + ")" + wrapper.SigName, false).EmitCall(ilgen);
				}
				if (targetType.IsPrimitive)
				{
					string unbox;
					TypeWrapper wrapper = GetWrapperType(targetType, out unbox);
					ilgen.Emit(OpCodes.Castclass, wrapper.TypeAsBaseType);
					wrapper.GetMethodWrapper(unbox, "()" + targetType.SigName, false).EmitCallvirt(ilgen);
				}
				else if (!constType.IsAssignableTo(targetType))
				{
					ilgen.Emit(OpCodes.Castclass, targetType.TypeAsBaseType);
				}
				targetType.EmitConvStackTypeToSignatureType(ilgen, targetType);
			}
		}

		private static TypeWrapper GetWrapperType(TypeWrapper tw, out string unbox)
		{
			if (tw == PrimitiveTypeWrapper.INT)
			{
				unbox = "intValue";
				return ClassLoaderWrapper.LoadClassCritical("java.lang.Integer");
			}
			else if (tw == PrimitiveTypeWrapper.LONG)
			{
				unbox = "longValue";
				return ClassLoaderWrapper.LoadClassCritical("java.lang.Long");
			}
			else if (tw == PrimitiveTypeWrapper.FLOAT)
			{
				unbox = "floatValue";
				return ClassLoaderWrapper.LoadClassCritical("java.lang.Float");
			}
			else if (tw == PrimitiveTypeWrapper.DOUBLE)
			{
				unbox = "doubleValue";
				return ClassLoaderWrapper.LoadClassCritical("java.lang.Double");
			}
			else
			{
				throw new InvalidOperationException();
			}
		}
	}

	private void EmitInvokeDynamic(ClassFile.ConstantPoolItemInvokeDynamic cpi)
	{
		CodeEmitter ilgen = ilGenerator;
		TypeWrapper[] args = cpi.GetArgTypes();
		CodeEmitterLocal[] temps = new CodeEmitterLocal[args.Length];
		for (int i = args.Length - 1; i >= 0; i--)
		{
			temps[i] = ilgen.DeclareLocal(args[i].TypeAsSignatureType);
			ilgen.Emit(OpCodes.Stloc, temps[i]);
		}
		Type delegateType = MethodHandleUtil.CreateDelegateType(args, cpi.GetRetType());
		InvokeDynamicBuilder.Emit(this, cpi, delegateType);
		for (int i = 0; i < args.Length; i++)
		{
			ilgen.Emit(OpCodes.Ldloc, temps[i]);
		}
		MethodHandleUtil.EmitCallDelegateInvokeMethod(ilgen, delegateType);
	}

	private sealed class MethodHandleConstant
	{
		private FieldBuilder field;
		private bool dynamic;

		internal void Emit(Compiler compiler, CodeEmitter ilgen, int index)
		{
			if (field == null)
			{
				field = CreateField(compiler, index, ref dynamic);
			}
			if (dynamic)
			{
				ClassFile.ConstantPoolItemMethodHandle mh = compiler.classFile.GetConstantPoolConstantMethodHandle(index);
				ilgen.Emit(OpCodes.Ldsflda, field);
				ilgen.EmitLdc_I4((int)mh.Kind);
				ilgen.Emit(OpCodes.Ldstr, mh.Class);
				ilgen.Emit(OpCodes.Ldstr, mh.Name);
				ilgen.Emit(OpCodes.Ldstr, mh.Signature);
				compiler.context.EmitCallerID(ilgen);
				ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicLoadMethodHandle);
			}
			else
			{
				ilgen.Emit(OpCodes.Ldsfld, field);
			}
		}

		private static FieldBuilder CreateField(Compiler compiler, int index, ref bool dynamic)
		{
			ClassFile.ConstantPoolItemMethodHandle mh = compiler.classFile.GetConstantPoolConstantMethodHandle(index);
			if (mh.GetClassType().IsUnloadable)
			{
				dynamic = true;
				return compiler.context.DefineDynamicMethodHandleCacheField();
			}
			TypeWrapper[] args;
			TypeWrapper arg0 = null;
			TypeWrapper ret;
			switch (mh.Kind)
			{
				case ClassFile.RefKind.getField:
					args = new TypeWrapper[] { mh.Member.DeclaringType };
					ret = ((FieldWrapper)mh.Member).FieldTypeWrapper;
					break;
				case ClassFile.RefKind.putField:
					args = new TypeWrapper[] { mh.Member.DeclaringType, ((FieldWrapper)mh.Member).FieldTypeWrapper };
					ret = PrimitiveTypeWrapper.VOID;
					break;
				case ClassFile.RefKind.getStatic:
					args = TypeWrapper.EmptyArray;
					ret = ((FieldWrapper)mh.Member).FieldTypeWrapper;
					break;
				case ClassFile.RefKind.putStatic:
					args = new TypeWrapper[] { ((FieldWrapper)mh.Member).FieldTypeWrapper };
					ret = PrimitiveTypeWrapper.VOID;
					break;
				case ClassFile.RefKind.invokeInterface:
				case ClassFile.RefKind.invokeSpecial:
				case ClassFile.RefKind.invokeVirtual:
				case ClassFile.RefKind.invokeStatic:
					if (mh.Member == null)
					{
						// it's MethodHandle.invoke[Exact]
						ClassFile.ConstantPoolItemMI cpi = (ClassFile.ConstantPoolItemMI)mh.MemberConstantPoolItem;
						args = cpi.GetArgTypes();
						arg0 = mh.GetClassType();
						ret = cpi.GetRetType();
					}
					else
					{
						MethodWrapper mw = (MethodWrapper)mh.Member;
						args = mw.GetParameters();
						ret = mw.ReturnType;
						if (mw.IsStatic)
						{
							// no receiver type
						}
						else if (mw.IsProtected && !mw.IsAccessibleFrom(mh.GetClassType(), compiler.clazz, mh.GetClassType()))
						{
							arg0 = compiler.clazz;
						}
						else
						{
							arg0 = mh.GetClassType();
						}
					}
					break;
				case ClassFile.RefKind.newInvokeSpecial:
					args = ((MethodWrapper)mh.Member).GetParameters();
					ret = mh.Member.DeclaringType;
					break;
				default:
					throw new InvalidOperationException();
			}
			if (arg0 != null)
			{
				TypeWrapper[] newArgs = new TypeWrapper[args.Length + 1];
				newArgs[0] = arg0;
				Array.Copy(args, 0, newArgs, 1, args.Length);
				args = newArgs;
			}
			if (HasUnloadable(args, ret))
			{
				dynamic = true;
				return compiler.context.DefineDynamicMethodHandleCacheField();
			}

			Type delegateType = MethodHandleUtil.CreateDelegateType(args, ret);
			MethodInfo method;
			if (mh.Kind == ClassFile.RefKind.invokeStatic
				&& (method = (MethodInfo)((MethodWrapper)mh.Member).GetMethod()) != null
				&& ((MethodWrapper)mh.Member).GetParameters().Length <= MethodHandleUtil.MaxArity)
			{
				// we can create a delegate that directly points to the target method
			}
			else
			{
				method = CreateDispatchStub(compiler, mh, delegateType);
			}
			TypeBuilder tb = compiler.context.DefineMethodHandleConstantType(index);
			FieldBuilder field = tb.DefineField("value", CoreClasses.java.lang.invoke.MethodHandle.Wrapper.TypeAsSignatureType, FieldAttributes.Assembly | FieldAttributes.Static | FieldAttributes.InitOnly);
			CodeEmitter ilgen = CodeEmitter.Create(ReflectUtil.DefineTypeInitializer(tb));
			ilgen.Emit(OpCodes.Ldnull);
			ilgen.Emit(OpCodes.Ldftn, method);
			ilgen.Emit(OpCodes.Newobj, MethodHandleUtil.GetDelegateConstructor(delegateType));
			ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.MethodHandleFromDelegate);
			ilgen.Emit(OpCodes.Stsfld, field);
			ilgen.Emit(OpCodes.Ret);
			ilgen.DoEmit();
			return field;
		}

		private static MethodInfo CreateDispatchStub(Compiler compiler, ClassFile.ConstantPoolItemMethodHandle mh, Type delegateType)
		{
			// the dispatch stub lives in the actual type (not the nested type that caches the value) to make sure
			// we have access to everything that the actual type has access to
			Type[] args = delegateType.GetGenericArguments();
			Type ret = Types.Void;
			// NOTE we can't use the method returned by GetDelegateInvokeMethod to determine the parameter types, due to Mono and CLR bugs
			if (MethodHandleUtil.GetDelegateInvokeMethod(delegateType).ReturnType != Types.Void)
			{
				ret = args[args.Length - 1];
				Array.Resize(ref args, args.Length - 1);
			}
			MethodBuilder mb = compiler.context.DefineMethodHandleDispatchStub(ret, args);
			CodeEmitter ilgen = CodeEmitter.Create(mb);
			if (args.Length > 0 && MethodHandleUtil.IsPackedArgsContainer(args[args.Length - 1]))
			{
				int packedArgPos = args.Length - 1;
				Type packedArgType = args[packedArgPos];
				for (int i = 0; i < packedArgPos; i++)
				{
					ilgen.EmitLdarg(i);
				}
				List<FieldInfo> fields = new List<FieldInfo>();
			next:
				args = args[args.Length - 1].GetGenericArguments();
				for (int i = 0; i < MethodHandleUtil.MaxArity; i++)
				{
					if (i == MethodHandleUtil.MaxArity - 1 && MethodHandleUtil.IsPackedArgsContainer(args[i]))
					{
						FieldInfo field = packedArgType.GetField("t" + (i + 1));
						packedArgType = field.FieldType;
						fields.Add(field);
						goto next;
					}
					else
					{
						ilgen.EmitLdarga(packedArgPos);
						foreach (FieldInfo field in fields)
						{
							ilgen.Emit(OpCodes.Ldflda, field);
						}
						ilgen.Emit(OpCodes.Ldfld, packedArgType.GetField("t" + (i + 1)));
					}
				}
			}
			else
			{
				for (int i = 0; i < args.Length; i++)
				{
					ilgen.EmitLdarg(i);
				}
			}
			switch (mh.Kind)
			{
				case ClassFile.RefKind.getField:
				case ClassFile.RefKind.getStatic:
					((FieldWrapper)mh.Member).EmitGet(ilgen);
					break;
				case ClassFile.RefKind.putField:
				case ClassFile.RefKind.putStatic:
					((FieldWrapper)mh.Member).EmitSet(ilgen);
					break;
				case ClassFile.RefKind.invokeInterface:
				case ClassFile.RefKind.invokeVirtual:
					if (mh.Member == null)
					{
						// it's a MethodHandle.invoke[Exact] constant MethodHandle
						new MethodHandleMethodWrapper(compiler.context, compiler.clazz, (ClassFile.ConstantPoolItemMI)mh.MemberConstantPoolItem, mh.Name == "invokeExact").EmitCallvirt(ilgen);
					}
					else
					{
						((MethodWrapper)mh.Member).EmitCallvirt(ilgen);
					}
					break;
				case ClassFile.RefKind.invokeStatic:
					((MethodWrapper)mh.Member).EmitCall(ilgen);
					break;
				case ClassFile.RefKind.invokeSpecial:
					ilgen.Emit(OpCodes.Callvirt, compiler.context.GetInvokeSpecialStub((MethodWrapper)mh.Member));
					break;
				case ClassFile.RefKind.newInvokeSpecial:
					((MethodWrapper)mh.Member).EmitNewobj(ilgen);
					break;
				default:
					throw new InvalidOperationException();
			}
			ilgen.Emit(OpCodes.Ret);
			ilgen.DoEmit();
			return mb;
		}
	}

	private sealed class MethodTypeConstant
	{
		private FieldBuilder field;
		private bool dynamic;

		internal void Emit(Compiler compiler, CodeEmitter ilgen, int index)
		{
			if (field == null)
			{
				field = CreateField(compiler, index, ref dynamic);
			}
			if (dynamic)
			{
				ilgen.Emit(OpCodes.Ldsflda, field);
				ilgen.Emit(OpCodes.Ldstr, compiler.classFile.GetConstantPoolConstantMethodType(index).Signature);
				compiler.context.EmitCallerID(ilgen);
				ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicLoadMethodType);
			}
			else
			{
				ilgen.Emit(OpCodes.Ldsfld, field);
			}
		}

		private static FieldBuilder CreateField(Compiler compiler, int index, ref bool dynamic)
		{
			ClassFile.ConstantPoolItemMethodType cpi = compiler.classFile.GetConstantPoolConstantMethodType(index);
			TypeWrapper[] args = cpi.GetArgTypes();
			TypeWrapper ret = cpi.GetRetType();

			if (HasUnloadable(args, ret))
			{
				dynamic = true;
				return compiler.context.DefineDynamicMethodTypeCacheField();
			}
			else
			{
				TypeBuilder tb = compiler.context.DefineMethodTypeConstantType(index);
				FieldBuilder field = tb.DefineField("value", CoreClasses.java.lang.invoke.MethodType.Wrapper.TypeAsSignatureType, FieldAttributes.Assembly | FieldAttributes.Static | FieldAttributes.InitOnly);
				CodeEmitter ilgen = CodeEmitter.Create(ReflectUtil.DefineTypeInitializer(tb));
				Type delegateType = MethodHandleUtil.CreateDelegateTypeForLoadConstant(args, ret);
				ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.LoadMethodType.MakeGenericMethod(delegateType));
				ilgen.Emit(OpCodes.Stsfld, field);
				ilgen.Emit(OpCodes.Ret);
				ilgen.DoEmit();
				return field;
			}
		}
	}

	private bool RequiresExplicitClassInit(TypeWrapper tw, int index, InstructionFlags[] flags)
	{
		ClassFile.Method.Instruction[] code = m.Instructions;
		for (; index < code.Length; index++)
		{
			if (code[index].NormalizedOpCode == NormalizedByteCode.__invokespecial)
			{
				ClassFile.ConstantPoolItemMI cpi = classFile.GetMethodref(code[index].Arg1);
				MethodWrapper mw = cpi.GetMethodForInvokespecial();
				return !mw.IsConstructor || mw.DeclaringType != tw;
			}
			if ((flags[index] & InstructionFlags.BranchTarget) != 0
				|| ByteCodeMetaData.IsBranch(code[index].NormalizedOpCode)
				|| ByteCodeMetaData.CanThrowException(code[index].NormalizedOpCode))
			{
				break;
			}
		}
		return true;
	}

	// NOTE despite its name this also handles value type args
	private void CastInterfaceArgs(TypeWrapper declaringType, TypeWrapper[] args, int instructionIndex, bool instanceMethod)
	{
		bool needsCast = false;
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
					if(tw.IsUnloadable || NeedsInterfaceDownCast(tw, args[i]))
					{
						needsCast = true;
						firstCastArg = i;
						break;
					}
				}
				else if(args[i].IsNonPrimitiveValueType)
				{
					if(i == 0 && instanceMethod && declaringType != args[i])
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
					if(tw.IsUnloadable || (args[i].IsInterfaceOrInterfaceArray && NeedsInterfaceDownCast(tw, args[i])))
					{
						ilGenerator.EmitAssertType(args[i].TypeAsTBD);
						Profiler.Count("InterfaceDownCast");
					}
				}
				if(i != firstCastArg)
				{
					dh.Store(i);
				}
			}
			if(instanceMethod && args[0].IsUnloadable && !declaringType.IsUnloadable)
			{
				if(declaringType.IsInterface)
				{
					ilGenerator.EmitAssertType(declaringType.TypeAsTBD);
				}
				else
				{
					ilGenerator.Emit(OpCodes.Castclass, declaringType.TypeAsSignatureType);
				}
			}
			for(int i = firstCastArg; i < args.Length; i++)
			{
				if(i != firstCastArg)
				{
					dh.Load(i);
				}
				if(!args[i].IsUnloadable && args[i].IsGhost)
				{
					if(i == 0 && instanceMethod && !declaringType.IsInterface)
					{
						// we're calling a java.lang.Object method through a ghost interface reference,
						// no ghost handling is needed
					}
					else
					{
						CodeEmitterLocal ghost = ilGenerator.AllocTempLocal(Types.Object);
						ilGenerator.Emit(OpCodes.Stloc, ghost);
						CodeEmitterLocal local = ilGenerator.AllocTempLocal(args[i].TypeAsSignatureType);
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
					if(!args[i].IsUnloadable)
					{
						if(args[i].IsNonPrimitiveValueType)
						{
							if(i == 0 && instanceMethod)
							{
								// we only need to unbox if the method was actually declared on the value type
								if(declaringType == args[i])
								{
									ilGenerator.Emit(OpCodes.Unbox, args[i].TypeAsTBD);
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

	private bool NeedsInterfaceDownCast(TypeWrapper tw, TypeWrapper arg)
	{
		if (tw == VerifierTypeWrapper.Null)
		{
			return false;
		}
		if (!tw.IsAccessibleFrom(clazz))
		{
			tw = tw.GetPublicBaseTypeWrapper();
		}
		return !tw.IsAssignableTo(arg);
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
		ilGenerator.Emit(OpCodes.Ldstr, wrapper.Name);
		context.EmitCallerID(ilGenerator);
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
			typeWrapper.EmitCheckcast(ilgen);
		}
	}

	private sealed class MethodHandleMethodWrapper : MethodWrapper
	{
		private readonly DynamicTypeWrapper.FinishContext context;
		private readonly TypeWrapper wrapper;
		private readonly ClassFile.ConstantPoolItemMI cpi;
		private readonly bool exact;

		internal MethodHandleMethodWrapper(DynamicTypeWrapper.FinishContext context, TypeWrapper wrapper, ClassFile.ConstantPoolItemMI cpi, bool exact)
			: base(wrapper, cpi.Name, cpi.Signature, null, cpi.GetRetType(), cpi.GetArgTypes(), Modifiers.Public, MemberFlags.None)
		{
			this.context = context;
			this.wrapper = wrapper;
			this.cpi = cpi;
			this.exact = exact;
		}

		internal override void EmitCall(CodeEmitter ilgen)
		{
			throw new InvalidOperationException();
		}

		internal override void EmitCallvirt(CodeEmitter ilgen)
		{
			TypeWrapper[] args;
			CodeEmitterLocal[] temps;
			Type delegateType;
			if (exact)
			{
				args = cpi.GetArgTypes();
				temps = new CodeEmitterLocal[args.Length];
				for (int i = args.Length - 1; i >= 0; i--)
				{
					temps[i] = ilgen.DeclareLocal(args[i].TypeAsSignatureType);
					ilgen.Emit(OpCodes.Stloc, temps[i]);
				}
				delegateType = MethodHandleUtil.CreateDelegateType(args, cpi.GetRetType());
				MethodInfo mi = ByteCodeHelperMethods.GetDelegateForInvokeExact.MakeGenericMethod(delegateType);
				ilgen.Emit(OpCodes.Call, mi);
			}
			else
			{
				args = new TypeWrapper[cpi.GetArgTypes().Length + 1];
				Array.Copy(cpi.GetArgTypes(), 0, args, 1, args.Length - 1);
				args[0] = CoreClasses.java.lang.invoke.MethodHandle.Wrapper;
				temps = new CodeEmitterLocal[args.Length];
				for (int i = args.Length - 1; i >= 0; i--)
				{
					temps[i] = ilgen.DeclareLocal(args[i].TypeAsSignatureType);
					ilgen.Emit(OpCodes.Stloc, temps[i]);
				}
				delegateType = MethodHandleUtil.CreateDelegateType(args, cpi.GetRetType());
				MethodInfo mi = ByteCodeHelperMethods.GetDelegateForInvoke.MakeGenericMethod(delegateType);
				Type typeofInvokeCache;
#if STATIC_COMPILER
				typeofInvokeCache = StaticCompiler.GetRuntimeType("IKVM.Runtime.InvokeCache`1");
#else
				typeofInvokeCache = typeof(IKVM.Runtime.InvokeCache<>);
#endif
				FieldBuilder fb = context.DefineMethodHandleInvokeCacheField(typeofInvokeCache.MakeGenericType(delegateType));
				ilgen.Emit(OpCodes.Ldloc, temps[0]);
				ilgen.Emit(OpCodes.Ldsflda, fb);
				ilgen.Emit(OpCodes.Call, mi);
			}
			for (int i = 0; i < args.Length; i++)
			{
				ilgen.Emit(OpCodes.Ldloc, temps[i]);
			}
			MethodHandleUtil.EmitCallDelegateInvokeMethod(ilgen, delegateType);
		}

		internal override void EmitNewobj(CodeEmitter ilgen)
		{
			throw new InvalidOperationException();
		}
	}

	private sealed class DynamicBinder
	{
		private MethodWrapper mw;

		internal MethodWrapper Get(DynamicTypeWrapper.FinishContext context, ClassFile.RefKind kind, ClassFile.ConstantPoolItemMI cpi)
		{
			return mw ?? (mw = new DynamicBinderMethodWrapper(cpi, Emit(context, kind, cpi), kind));
		}

		private static MethodInfo Emit(DynamicTypeWrapper.FinishContext context, ClassFile.RefKind kind, ClassFile.ConstantPoolItemMI cpi)
		{
			TypeWrapper ret;
			TypeWrapper[] args;
			if (kind == ClassFile.RefKind.invokeStatic)
			{
				ret = cpi.GetRetType();
				args = cpi.GetArgTypes();
			}
			else if (kind == ClassFile.RefKind.newInvokeSpecial)
			{
				ret = cpi.GetClassType();
				args = cpi.GetArgTypes();
			}
			else
			{
				ret = cpi.GetRetType();
				args = new TypeWrapper[cpi.GetArgTypes().Length + 1];
				Array.Copy(cpi.GetArgTypes(), 0, args, 1, args.Length - 1);
				args[0] = cpi.GetClassType();
			}
			Type delegateType = MethodHandleUtil.CreateDelegateType(args, ret);
			FieldBuilder fb = context.DefineMethodHandleInvokeCacheField(delegateType);
			Type[] types = new Type[args.Length];
			for (int i = 0; i < types.Length; i++)
			{
				types[i] = args[i].TypeAsSignatureType;
			}
			MethodBuilder mb = context.DefineMethodHandleDispatchStub(ret.TypeAsSignatureType, types);
			CodeEmitter ilgen = CodeEmitter.Create(mb);
			ilgen.Emit(OpCodes.Ldsfld, fb);
			CodeEmitterLabel label = ilgen.DefineLabel();
			ilgen.EmitBrtrue(label);
			ilgen.EmitLdc_I4((int)kind);
			ilgen.Emit(OpCodes.Ldstr, cpi.Class);
			ilgen.Emit(OpCodes.Ldstr, cpi.Name);
			ilgen.Emit(OpCodes.Ldstr, cpi.Signature);
			context.EmitCallerID(ilgen);
			ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicBinderMemberLookup.MakeGenericMethod(delegateType));
			ilgen.Emit(OpCodes.Volatile);
			ilgen.Emit(OpCodes.Stsfld, fb);
			ilgen.MarkLabel(label);
			ilgen.Emit(OpCodes.Ldsfld, fb);
			for (int i = 0; i < args.Length; i++)
			{
				ilgen.EmitLdarg(i);
			}
			MethodHandleUtil.EmitCallDelegateInvokeMethod(ilgen, delegateType);
			ilgen.Emit(OpCodes.Ret);
			ilgen.DoEmit();
			return mb;
		}

		private sealed class DynamicBinderMethodWrapper : MethodWrapper
		{
			private readonly MethodInfo method;

			internal DynamicBinderMethodWrapper(ClassFile.ConstantPoolItemMI cpi, MethodInfo method, ClassFile.RefKind kind)
				: base(cpi.GetClassType(), cpi.Name, cpi.Signature, null, cpi.GetRetType(), cpi.GetArgTypes(), kind == ClassFile.RefKind.invokeStatic ? Modifiers.Public | Modifiers.Static : Modifiers.Public, MemberFlags.None)
			{
				this.method = method;
			}

			internal override void EmitCall(CodeEmitter ilgen)
			{
				ilgen.Emit(OpCodes.Call, method);
			}

			internal override void EmitCallvirt(CodeEmitter ilgen)
			{
				ilgen.Emit(OpCodes.Call, method);
			}

			internal override void EmitNewobj(CodeEmitter ilgen)
			{
				ilgen.Emit(OpCodes.Call, method);
			}
		}
	}

	private MethodWrapper GetMethodCallEmitter(NormalizedByteCode invoke, int constantPoolIndex)
	{
		ClassFile.ConstantPoolItemMI cpi = classFile.GetMethodref(constantPoolIndex);
#if STATIC_COMPILER
		if(replacedMethodWrappers != null)
		{
			for(int i = 0; i < replacedMethodWrappers.Length; i++)
			{
				if(replacedMethodWrappers[i].DeclaringType == cpi.GetClassType()
					&& replacedMethodWrappers[i].Name == cpi.Name
					&& replacedMethodWrappers[i].Signature == cpi.Signature)
				{
					MethodWrapper rmw = replacedMethodWrappers[i];
					rmw.Link();
					return rmw;
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
				return context.GetValue<DynamicBinder>(constantPoolIndex).Get(context, ClassFile.RefKind.invokeInterface, cpi);
			case NormalizedByteCode.__dynamic_invokestatic:
				return context.GetValue<DynamicBinder>(constantPoolIndex).Get(context, ClassFile.RefKind.invokeStatic, cpi);
			case NormalizedByteCode.__dynamic_invokevirtual:
				return context.GetValue<DynamicBinder>(constantPoolIndex).Get(context, ClassFile.RefKind.invokeVirtual, cpi);
			case NormalizedByteCode.__dynamic_invokespecial:
				return context.GetValue<DynamicBinder>(constantPoolIndex).Get(context, ClassFile.RefKind.newInvokeSpecial, cpi);
			case NormalizedByteCode.__methodhandle_invoke:
				return new MethodHandleMethodWrapper(context, clazz, cpi, false);
			case NormalizedByteCode.__methodhandle_invokeexact:
				return new MethodHandleMethodWrapper(context, clazz, cpi, true);
			default:
				throw new InvalidOperationException();
		}
		if(mw.IsDynamicOnly)
		{
			switch (invoke)
			{
				case NormalizedByteCode.__invokespecial:
					return context.GetValue<DynamicBinder>(constantPoolIndex).Get(context, ClassFile.RefKind.invokeSpecial, cpi);
				case NormalizedByteCode.__invokeinterface:
					return context.GetValue<DynamicBinder>(constantPoolIndex).Get(context, ClassFile.RefKind.invokeInterface, cpi);
				case NormalizedByteCode.__invokestatic:
					return context.GetValue<DynamicBinder>(constantPoolIndex).Get(context, ClassFile.RefKind.invokeStatic, cpi);
				case NormalizedByteCode.__invokevirtual:
					return context.GetValue<DynamicBinder>(constantPoolIndex).Get(context, ClassFile.RefKind.invokeVirtual, cpi);
				default:
					throw new InvalidOperationException();
			}
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

	private LocalVar LoadLocal(int instructionIndex)
	{
		LocalVar v = localVars.GetLocalVar(instructionIndex);
		if(v.isArg)
		{
			ClassFile.Method.Instruction instr = m.Instructions[instructionIndex];
			int i = m.ArgMap[instr.NormalizedArg1];
			ilGenerator.EmitLdarg(i);
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
				v.builder = ilGenerator.DeclareLocal(GetLocalBuilderType(v.type));
				if(debug && v.name != null)
				{
					v.builder.SetLocalSymInfo(v.name);
				}
			}
			ilGenerator.Emit(OpCodes.Ldloc, v.builder);
		}
		return v;
	}

	private LocalVar StoreLocal(int instructionIndex)
	{
		LocalVar v = localVars.GetLocalVar(instructionIndex);
		if(v == null)
		{
			// dead store
			ilGenerator.Emit(OpCodes.Pop);
		}
		else if(v.isArg)
		{
			ClassFile.Method.Instruction instr = m.Instructions[instructionIndex];
			int i = m.ArgMap[instr.NormalizedArg1];
			ilGenerator.EmitStarg(i);
		}
		else if(v.type == VerifierTypeWrapper.Null)
		{
			ilGenerator.Emit(OpCodes.Pop);
		}
		else
		{
			if(v.builder == null)
			{
				v.builder = ilGenerator.DeclareLocal(GetLocalBuilderType(v.type));
				if(debug && v.name != null)
				{
					v.builder.SetLocalSymInfo(v.name);
				}
			}
			ilGenerator.Emit(OpCodes.Stloc, v.builder);
		}
		return v;
	}

	private Type GetLocalBuilderType(TypeWrapper tw)
	{
		if (tw.IsUnloadable)
		{
			return Types.Object;
		}
		else if (tw.IsAccessibleFrom(clazz))
		{
			return tw.TypeAsLocalOrStackType;
		}
		else
		{
			return tw.GetPublicBaseTypeWrapper().TypeAsLocalOrStackType;
		}
	}

	private ExceptionTableEntry[] GetExceptionTableFor(InstructionFlags[] flags)
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

	private InstructionFlags[] ComputePartialReachability(int initialInstructionIndex, bool skipFaultBlocks)
	{
		return MethodAnalyzer.ComputePartialReachability(ma, m.Instructions, exceptions, initialInstructionIndex, skipFaultBlocks);
	}
}
