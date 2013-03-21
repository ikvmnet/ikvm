/*
  Copyright (C) 2007-2013 Jeroen Frijters

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
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Security;
using IKVM.Internal;

namespace IKVM.Internal
{
#if !FIRST_PASS
	public interface IReflectionException
	{
		java.lang.IllegalArgumentException GetIllegalArgumentException(object obj);
		java.lang.IllegalArgumentException SetIllegalArgumentException(object obj);
	}
#endif

	// this must be public (due to .NET 4.0 security model)
	public sealed class State
	{
		internal int Value;
	}
}

static class Java_sun_reflect_Reflection
{
#if CLASSGC
	private static readonly ConditionalWeakTable<MethodBase, State> isHideFromJavaCache = new ConditionalWeakTable<MethodBase, State>();

	internal static bool IsHideFromJava(MethodBase mb)
	{
		State state = isHideFromJavaCache.GetOrCreateValue(mb);
		if (state.Value == 0)
		{
			state.Value = IsHideFromJavaImpl(mb);
		}
		return state.Value == 1;
	}

	private static int IsHideFromJavaImpl(MethodBase mb)
	{
		if (mb.Name.StartsWith("__<", StringComparison.Ordinal))
		{
			return 1;
		}
		if (mb.IsDefined(typeof(IKVM.Attributes.HideFromJavaAttribute), false) || mb.IsDefined(typeof(IKVM.Attributes.HideFromReflectionAttribute), false))
		{
			return 1;
		}
		return 2;
	}
#else
	private static readonly Dictionary<RuntimeMethodHandle, bool> isHideFromJavaCache = new Dictionary<RuntimeMethodHandle, bool>();

	internal static bool IsHideFromJava(MethodBase mb)
	{
		if (mb.Name.StartsWith("__<", StringComparison.Ordinal))
		{
			return true;
		}
		RuntimeMethodHandle handle;
		try
		{
			handle = mb.MethodHandle;
		}
		catch (InvalidOperationException)
		{
			// DynamicMethods don't have a RuntimeMethodHandle and we always want to hide them anyway
			return true;
		}
		catch (NotSupportedException)
		{
			// DynamicMethods don't have a RuntimeMethodHandle and we always want to hide them anyway
			return true;
		}
		lock (isHideFromJavaCache)
		{
			bool cached;
			if (isHideFromJavaCache.TryGetValue(handle, out cached))
			{
				return cached;
			}
		}
		bool isHide = mb.IsDefined(typeof(IKVM.Attributes.HideFromJavaAttribute), false) || mb.IsDefined(typeof(IKVM.Attributes.HideFromReflectionAttribute), false);
		lock (isHideFromJavaCache)
		{
			isHideFromJavaCache[handle] = isHide;
		}
		return isHide;
	}
#endif

	// NOTE this method is hooked up explicitly through map.xml to prevent inlining of the native stub
	// and tail-call optimization in the native stub.
	public static object getCallerClass(int realFramesToSkip)
	{
#if FIRST_PASS
		return null;
#else
		int i = 3;
		if (realFramesToSkip <= 1)
		{
			i = 1;
			realFramesToSkip = Math.Max(realFramesToSkip + 2, 2);
		}
		realFramesToSkip--;
		for (; ; )
		{
			MethodBase method = new StackFrame(i++, false).GetMethod();
			if (method == null)
			{
				return null;
			}
			Type type = method.DeclaringType;
			// NOTE these checks should be the same as the ones in SecurityManager.getClassContext
			if (IsHideFromJava(method)
				|| type == null
				|| type.Assembly == typeof(object).Assembly
				|| type.Assembly == typeof(Java_sun_reflect_Reflection).Assembly
				|| type.Assembly == Java_java_lang_SecurityManager.jniAssembly
				|| type == typeof(java.lang.reflect.Method)
				|| type == typeof(java.lang.reflect.Constructor))
			{
				continue;
			}
			if (--realFramesToSkip == 0)
			{
				return ClassLoaderWrapper.GetWrapperFromType(type).ClassObject;
			}
		}
#endif
	}

	public static int getClassAccessFlags(java.lang.Class clazz)
	{
		// the mask comes from JVM_RECOGNIZED_CLASS_MODIFIERS in src/hotspot/share/vm/prims/jvm.h
		int mods = (int)TypeWrapper.FromClass(clazz).Modifiers & 0x7631;
		// interface implies abstract
		mods |= (mods & 0x0200) << 1;
		return mods;
	}

	public static bool checkInternalAccess(java.lang.Class currentClass, java.lang.Class memberClass)
	{
		TypeWrapper current = TypeWrapper.FromClass(currentClass);
		TypeWrapper member = TypeWrapper.FromClass(memberClass);
		return member.IsInternal && member.InternalsVisibleTo(current);
	}
}

static class Java_sun_reflect_ReflectionFactory
{
#if !FIRST_PASS
	private static object[] ConvertArgs(ClassLoaderWrapper loader, TypeWrapper[] argumentTypes, object[] args)
	{
		object[] nargs = new object[args == null ? 0 : args.Length];
		if (nargs.Length != argumentTypes.Length)
		{
			throw new java.lang.IllegalArgumentException("wrong number of arguments");
		}
		for (int i = 0; i < nargs.Length; i++)
		{
			if (argumentTypes[i].IsPrimitive)
			{
				if (args[i] == null)
				{
					throw new java.lang.IllegalArgumentException("primitive wrapper null");
				}
				nargs[i] = JVM.Unbox(args[i]);
				// NOTE we depend on the fact that the .NET reflection parameter type
				// widening rules are the same as in Java, but to have this work for byte
				// we need to convert byte to sbyte.
				if (nargs[i] is byte && argumentTypes[i] != PrimitiveTypeWrapper.BYTE)
				{
					nargs[i] = (sbyte)(byte)nargs[i];
				}
			}
			else
			{
				if (args[i] != null && !argumentTypes[i].EnsureLoadable(loader).IsInstance(args[i]))
				{
					throw new java.lang.IllegalArgumentException();
				}
				nargs[i] = args[i];
			}
		}
		return nargs;
	}

	private sealed class MethodAccessorImpl : sun.reflect.MethodAccessor
	{
		private readonly MethodWrapper mw;

		internal MethodAccessorImpl(java.lang.reflect.Method method)
		{
			mw = MethodWrapper.FromMethodOrConstructor(method);
		}

		[IKVM.Attributes.HideFromJava]
		public object invoke(object obj, object[] args, ikvm.@internal.CallerID callerID)
		{
			if (!mw.IsStatic && !mw.DeclaringType.IsInstance(obj))
			{
				if (obj == null)
				{
					throw new java.lang.NullPointerException();
				}
				throw new java.lang.IllegalArgumentException("object is not an instance of declaring class");
			}
			args = ConvertArgs(mw.DeclaringType.GetClassLoader(), mw.GetParameters(), args);
			// if the method is an interface method, we must explicitly run <clinit>,
			// because .NET reflection doesn't
			if (mw.DeclaringType.IsInterface)
			{
				mw.DeclaringType.RunClassInit();
			}
			object retval;
			try
			{
				retval = ((ICustomInvoke)mw).Invoke(obj, args);
			}
			catch (MethodAccessException x)
			{
				// this can happen if we're calling a non-public method and the call stack doesn't have ReflectionPermission.MemberAccess
				throw new java.lang.IllegalAccessException().initCause(x);
			}
			if (mw.ReturnType.IsPrimitive && mw.ReturnType != PrimitiveTypeWrapper.VOID)
			{
				retval = JVM.Box(retval);
			}
			return retval;
		}
	}

	internal sealed class FastMethodAccessorImpl : sun.reflect.MethodAccessor
	{
		private static readonly MethodInfo valueOfByte;
		private static readonly MethodInfo valueOfBoolean;
		private static readonly MethodInfo valueOfChar;
		private static readonly MethodInfo valueOfShort;
		private static readonly MethodInfo valueOfInt;
		private static readonly MethodInfo valueOfFloat;
		private static readonly MethodInfo valueOfLong;
		private static readonly MethodInfo valueOfDouble;
		private static readonly MethodInfo byteValue;
		private static readonly MethodInfo booleanValue;
		private static readonly MethodInfo charValue;
		private static readonly MethodInfo shortValue;
		private static readonly MethodInfo intValue;
		private static readonly MethodInfo floatValue;
		private static readonly MethodInfo longValue;
		private static readonly MethodInfo doubleValue;
		internal static readonly ConstructorInfo invocationTargetExceptionCtor;
		internal static readonly ConstructorInfo illegalArgumentExceptionCtor;
		internal static readonly MethodInfo get_TargetSite;
		internal static readonly MethodInfo GetCurrentMethod;

		private delegate object Invoker(object obj, object[] args, ikvm.@internal.CallerID callerID);
		private Invoker invoker;

		static FastMethodAccessorImpl()
		{
			valueOfByte = typeof(java.lang.Byte).GetMethod("valueOf", new Type[] { typeof(byte) });
			valueOfBoolean = typeof(java.lang.Boolean).GetMethod("valueOf", new Type[] { typeof(bool) });
			valueOfChar = typeof(java.lang.Character).GetMethod("valueOf", new Type[] { typeof(char) });
			valueOfShort = typeof(java.lang.Short).GetMethod("valueOf", new Type[] { typeof(short) });
			valueOfInt = typeof(java.lang.Integer).GetMethod("valueOf", new Type[] { typeof(int) });
			valueOfFloat = typeof(java.lang.Float).GetMethod("valueOf", new Type[] { typeof(float) });
			valueOfLong = typeof(java.lang.Long).GetMethod("valueOf", new Type[] { typeof(long) });
			valueOfDouble = typeof(java.lang.Double).GetMethod("valueOf", new Type[] { typeof(double) });

			byteValue = typeof(java.lang.Byte).GetMethod("byteValue", Type.EmptyTypes);
			booleanValue = typeof(java.lang.Boolean).GetMethod("booleanValue", Type.EmptyTypes);
			charValue = typeof(java.lang.Character).GetMethod("charValue", Type.EmptyTypes);
			shortValue = typeof(java.lang.Short).GetMethod("shortValue", Type.EmptyTypes);
			intValue = typeof(java.lang.Integer).GetMethod("intValue", Type.EmptyTypes);
			floatValue = typeof(java.lang.Float).GetMethod("floatValue", Type.EmptyTypes);
			longValue = typeof(java.lang.Long).GetMethod("longValue", Type.EmptyTypes);
			doubleValue = typeof(java.lang.Double).GetMethod("doubleValue", Type.EmptyTypes);

			invocationTargetExceptionCtor = typeof(java.lang.reflect.InvocationTargetException).GetConstructor(new Type[] { typeof(Exception) });
			illegalArgumentExceptionCtor = typeof(java.lang.IllegalArgumentException).GetConstructor(Type.EmptyTypes);
			get_TargetSite = typeof(Exception).GetMethod("get_TargetSite");
			GetCurrentMethod = typeof(MethodBase).GetMethod("GetCurrentMethod");
		}

		private sealed class RunClassInit
		{
			private FastMethodAccessorImpl outer;
			private TypeWrapper tw;
			private Invoker invoker;

			internal RunClassInit(FastMethodAccessorImpl outer, TypeWrapper tw, Invoker invoker)
			{
				this.outer = outer;
				this.tw = tw;
				this.invoker = invoker;
			}

			[IKVM.Attributes.HideFromJava]
			internal object invoke(object obj, object[] args, ikvm.@internal.CallerID callerID)
			{
				// FXBUG pre-SP1 a DynamicMethod that calls a static method doesn't trigger the cctor, so we do that explicitly.
				// even on .NET 2.0 SP2, interface method invocations don't run the interface cctor
				// NOTE when testing, please test both the x86 and x64 CLR JIT, because they have different bugs (even on .NET 2.0 SP2)
				tw.RunClassInit();
				outer.invoker = invoker;
				return invoker(obj, args, callerID);
			}
		}

		internal FastMethodAccessorImpl(java.lang.reflect.Method method, bool nonvirtual)
		{
			MethodWrapper mw = MethodWrapper.FromMethodOrConstructor(method);
			TypeWrapper[] parameters;
			try
			{
				mw.DeclaringType.Finish();
				parameters = mw.GetParameters();
				for (int i = 0; i < parameters.Length; i++)
				{
					// the EnsureLoadable shouldn't fail, because we don't allow a java.lang.reflect.Method
					// to "escape" if it has an unloadable type in the signature
					parameters[i] = parameters[i].EnsureLoadable(mw.DeclaringType.GetClassLoader());
					parameters[i].Finish();
				}
			}
			catch (RetargetableJavaException x)
			{
				throw x.ToJava();
			}
			mw.ResolveMethod();
			DynamicMethod dm = DynamicMethodUtils.Create("__<Invoker>", mw.DeclaringType.TypeAsBaseType, !mw.IsPublic || !mw.DeclaringType.IsPublic || nonvirtual, typeof(object), new Type[] { typeof(object), typeof(object[]), typeof(ikvm.@internal.CallerID) });
			CodeEmitter ilgen = CodeEmitter.Create(dm);
			CodeEmitterLocal ret = ilgen.DeclareLocal(typeof(object));
			if (!mw.IsStatic)
			{
				// check target for null
				ilgen.Emit(OpCodes.Ldarg_0);
				ilgen.EmitNullCheck();
			}

			// check args length
			CodeEmitterLabel argsLengthOK = ilgen.DefineLabel();
			if (parameters.Length == 0)
			{
				// zero length array may be null
				ilgen.Emit(OpCodes.Ldarg_1);
				ilgen.EmitBrfalse(argsLengthOK);
			}
			ilgen.Emit(OpCodes.Ldarg_1);
			ilgen.Emit(OpCodes.Ldlen);
			ilgen.EmitLdc_I4(parameters.Length);
			ilgen.EmitBeq(argsLengthOK);
			ilgen.Emit(OpCodes.Newobj, illegalArgumentExceptionCtor);
			ilgen.Emit(OpCodes.Throw);
			ilgen.MarkLabel(argsLengthOK);

			int thisCount = mw.IsStatic ? 0 : 1;
			CodeEmitterLocal[] args = new CodeEmitterLocal[parameters.Length + thisCount];
			if (!mw.IsStatic)
			{
				args[0] = ilgen.DeclareLocal(mw.DeclaringType.TypeAsSignatureType);
			}
			for (int i = thisCount; i < args.Length; i++)
			{
				args[i] = ilgen.DeclareLocal(parameters[i - thisCount].TypeAsSignatureType);
			}
			ilgen.BeginExceptionBlock();
			if (!mw.IsStatic)
			{
				ilgen.Emit(OpCodes.Ldarg_0);
				mw.DeclaringType.EmitCheckcast(ilgen);
				mw.DeclaringType.EmitConvStackTypeToSignatureType(ilgen, null);
				ilgen.Emit(OpCodes.Stloc, args[0]);
			}
			for (int i = thisCount; i < args.Length; i++)
			{
				ilgen.Emit(OpCodes.Ldarg_1);
				ilgen.EmitLdc_I4(i - thisCount);
				ilgen.Emit(OpCodes.Ldelem_Ref);
				TypeWrapper tw = parameters[i - thisCount];
				EmitUnboxArg(ilgen, tw);
				tw.EmitConvStackTypeToSignatureType(ilgen, null);
				ilgen.Emit(OpCodes.Stloc, args[i]);
			}
			CodeEmitterLabel label1 = ilgen.DefineLabel();
			ilgen.EmitLeave(label1);
			ilgen.BeginCatchBlock(typeof(InvalidCastException));
			ilgen.Emit(OpCodes.Newobj, illegalArgumentExceptionCtor);
			ilgen.Emit(OpCodes.Throw);
			ilgen.BeginCatchBlock(typeof(NullReferenceException));
			ilgen.Emit(OpCodes.Newobj, illegalArgumentExceptionCtor);
			ilgen.Emit(OpCodes.Throw);
			ilgen.EndExceptionBlock();

			// this is the actual call
			ilgen.MarkLabel(label1);
			ilgen.BeginExceptionBlock();
			for (int i = 0; i < args.Length; i++)
			{
				if (i == 0 && !mw.IsStatic && (mw.DeclaringType.IsNonPrimitiveValueType || mw.DeclaringType.IsGhost))
				{
					ilgen.Emit(OpCodes.Ldloca, args[i]);
				}
				else
				{
					ilgen.Emit(OpCodes.Ldloc, args[i]);
				}
			}
			if (mw.HasCallerID)
			{
				ilgen.Emit(OpCodes.Ldarg_2);
			}
			if (mw.IsStatic || nonvirtual)
			{
				mw.EmitCall(ilgen);
			}
			else
			{
				mw.EmitCallvirtReflect(ilgen);
			}
			mw.ReturnType.EmitConvSignatureTypeToStackType(ilgen);
			BoxReturnValue(ilgen, mw.ReturnType);
			ilgen.Emit(OpCodes.Stloc, ret);
			CodeEmitterLabel label2 = ilgen.DefineLabel();
			ilgen.EmitLeave(label2);
			ilgen.BeginCatchBlock(typeof(Exception));
			CodeEmitterLabel label = ilgen.DefineLabel();
			CodeEmitterLabel labelWrap = ilgen.DefineLabel();
			if (IntPtr.Size == 8 && nonvirtual)
			{
				// This is a workaround for the x64 JIT, which is completely broken as usual.
				// When MethodBase.GetCurrentMethod() is used in a dynamic method that isn't verifiable,
				// we get an access violation at JIT time. When we're doing a nonvirtual call,
				// the method is not verifiable, so we disable this check (which, at worst, results
				// in any exceptions thrown at the call site being incorrectly wrapped in an InvocationTargetException).
			}
			else
			{
				// If the exception we caught is a java.lang.reflect.InvocationTargetException, we know it must be
				// wrapped, because .NET won't throw that exception and we also cannot check the target site,
				// because it may be the same as us if a method is recursively invoking itself.
				ilgen.Emit(OpCodes.Dup);
				ilgen.Emit(OpCodes.Isinst, typeof(java.lang.reflect.InvocationTargetException));
				ilgen.EmitBrtrue(labelWrap);
				ilgen.Emit(OpCodes.Dup);
				ilgen.Emit(OpCodes.Callvirt, get_TargetSite);
				ilgen.Emit(OpCodes.Call, GetCurrentMethod);
				ilgen.Emit(OpCodes.Ceq);
				ilgen.EmitBrtrue(label);
			}
			ilgen.MarkLabel(labelWrap);
			ilgen.Emit(OpCodes.Ldc_I4_0);
			ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.mapException.MakeGenericMethod(Types.Exception));
			ilgen.Emit(OpCodes.Newobj, invocationTargetExceptionCtor);
			ilgen.MarkLabel(label);
			ilgen.Emit(OpCodes.Throw);
			ilgen.EndExceptionBlock();

			ilgen.MarkLabel(label2);
			ilgen.Emit(OpCodes.Ldloc, ret);
			ilgen.Emit(OpCodes.Ret);
			ilgen.DoEmit();
			invoker = (Invoker)dm.CreateDelegate(typeof(Invoker));
			if ((mw.IsStatic || mw.DeclaringType.IsInterface) && mw.DeclaringType.HasStaticInitializer)
			{
				invoker = new Invoker(new RunClassInit(this, mw.DeclaringType, invoker).invoke);
			}
		}

		private static void Expand(CodeEmitter ilgen, TypeWrapper type)
		{
			if (type == PrimitiveTypeWrapper.FLOAT)
			{
				ilgen.Emit(OpCodes.Conv_R4);
			}
			else if (type == PrimitiveTypeWrapper.LONG)
			{
				ilgen.Emit(OpCodes.Conv_I8);
			}
			else if (type == PrimitiveTypeWrapper.DOUBLE)
			{
				ilgen.Emit(OpCodes.Conv_R8);
			}
		}

		internal static void EmitUnboxArg(CodeEmitter ilgen, TypeWrapper type)
		{
			if (type == PrimitiveTypeWrapper.BYTE)
			{
				ilgen.Emit(OpCodes.Castclass, typeof(java.lang.Byte));
				ilgen.Emit(OpCodes.Call, byteValue);
			}
			else if (type == PrimitiveTypeWrapper.BOOLEAN)
			{
				ilgen.Emit(OpCodes.Castclass, typeof(java.lang.Boolean));
				ilgen.Emit(OpCodes.Call, booleanValue);
			}
			else if (type == PrimitiveTypeWrapper.CHAR)
			{
				ilgen.Emit(OpCodes.Castclass, typeof(java.lang.Character));
				ilgen.Emit(OpCodes.Call, charValue);
			}
			else if (type == PrimitiveTypeWrapper.SHORT
				|| type == PrimitiveTypeWrapper.INT
				|| type == PrimitiveTypeWrapper.FLOAT
				|| type == PrimitiveTypeWrapper.LONG
				|| type == PrimitiveTypeWrapper.DOUBLE)
			{
				ilgen.Emit(OpCodes.Dup);
				ilgen.Emit(OpCodes.Isinst, typeof(java.lang.Byte));
				CodeEmitterLabel next = ilgen.DefineLabel();
				ilgen.EmitBrfalse(next);
				ilgen.Emit(OpCodes.Castclass, typeof(java.lang.Byte));
				ilgen.Emit(OpCodes.Call, byteValue);
				ilgen.Emit(OpCodes.Conv_I1);
				Expand(ilgen, type);
				CodeEmitterLabel done = ilgen.DefineLabel();
				ilgen.EmitBr(done);
				ilgen.MarkLabel(next);
				if (type == PrimitiveTypeWrapper.SHORT)
				{
					ilgen.Emit(OpCodes.Castclass, typeof(java.lang.Short));
					ilgen.Emit(OpCodes.Call, shortValue);
				}
				else
				{
					ilgen.Emit(OpCodes.Dup);
					ilgen.Emit(OpCodes.Isinst, typeof(java.lang.Short));
					next = ilgen.DefineLabel();
					ilgen.EmitBrfalse(next);
					ilgen.Emit(OpCodes.Castclass, typeof(java.lang.Short));
					ilgen.Emit(OpCodes.Call, shortValue);
					Expand(ilgen, type);
					ilgen.EmitBr(done);
					ilgen.MarkLabel(next);
					ilgen.Emit(OpCodes.Dup);
					ilgen.Emit(OpCodes.Isinst, typeof(java.lang.Character));
					next = ilgen.DefineLabel();
					ilgen.EmitBrfalse(next);
					ilgen.Emit(OpCodes.Castclass, typeof(java.lang.Character));
					ilgen.Emit(OpCodes.Call, charValue);
					Expand(ilgen, type);
					ilgen.EmitBr(done);
					ilgen.MarkLabel(next);
					if (type == PrimitiveTypeWrapper.INT)
					{
						ilgen.Emit(OpCodes.Castclass, typeof(java.lang.Integer));
						ilgen.Emit(OpCodes.Call, intValue);
					}
					else
					{
						ilgen.Emit(OpCodes.Dup);
						ilgen.Emit(OpCodes.Isinst, typeof(java.lang.Integer));
						next = ilgen.DefineLabel();
						ilgen.EmitBrfalse(next);
						ilgen.Emit(OpCodes.Castclass, typeof(java.lang.Integer));
						ilgen.Emit(OpCodes.Call, intValue);
						Expand(ilgen, type);
						ilgen.EmitBr(done);
						ilgen.MarkLabel(next);
						if (type == PrimitiveTypeWrapper.LONG)
						{
							ilgen.Emit(OpCodes.Castclass, typeof(java.lang.Long));
							ilgen.Emit(OpCodes.Call, longValue);
						}
						else
						{
							ilgen.Emit(OpCodes.Dup);
							ilgen.Emit(OpCodes.Isinst, typeof(java.lang.Long));
							next = ilgen.DefineLabel();
							ilgen.EmitBrfalse(next);
							ilgen.Emit(OpCodes.Castclass, typeof(java.lang.Long));
							ilgen.Emit(OpCodes.Call, longValue);
							Expand(ilgen, type);
							ilgen.EmitBr(done);
							ilgen.MarkLabel(next);
							if (type == PrimitiveTypeWrapper.FLOAT)
							{
								ilgen.Emit(OpCodes.Castclass, typeof(java.lang.Float));
								ilgen.Emit(OpCodes.Call, floatValue);
							}
							else if (type == PrimitiveTypeWrapper.DOUBLE)
							{
								ilgen.Emit(OpCodes.Dup);
								ilgen.Emit(OpCodes.Isinst, typeof(java.lang.Float));
								next = ilgen.DefineLabel();
								ilgen.EmitBrfalse(next);
								ilgen.Emit(OpCodes.Castclass, typeof(java.lang.Float));
								ilgen.Emit(OpCodes.Call, floatValue);
								ilgen.EmitBr(done);
								ilgen.MarkLabel(next);
								ilgen.Emit(OpCodes.Castclass, typeof(java.lang.Double));
								ilgen.Emit(OpCodes.Call, doubleValue);
							}
							else
							{
								throw new InvalidOperationException();
							}
						}
					}
				}
				ilgen.MarkLabel(done);
			}
			else
			{
				type.EmitCheckcast(ilgen);
			}
		}

		private static void BoxReturnValue(CodeEmitter ilgen, TypeWrapper type)
		{
			if (type == PrimitiveTypeWrapper.VOID)
			{
				ilgen.Emit(OpCodes.Ldnull);
			}
			else if (type == PrimitiveTypeWrapper.BYTE)
			{
				ilgen.Emit(OpCodes.Call, valueOfByte);
			}
			else if (type == PrimitiveTypeWrapper.BOOLEAN)
			{
				ilgen.Emit(OpCodes.Call, valueOfBoolean);
			}
			else if (type == PrimitiveTypeWrapper.CHAR)
			{
				ilgen.Emit(OpCodes.Call, valueOfChar);
			}
			else if (type == PrimitiveTypeWrapper.SHORT)
			{
				ilgen.Emit(OpCodes.Call, valueOfShort);
			}
			else if (type == PrimitiveTypeWrapper.INT)
			{
				ilgen.Emit(OpCodes.Call, valueOfInt);
			}
			else if (type == PrimitiveTypeWrapper.FLOAT)
			{
				ilgen.Emit(OpCodes.Call, valueOfFloat);
			}
			else if (type == PrimitiveTypeWrapper.LONG)
			{
				ilgen.Emit(OpCodes.Call, valueOfLong);
			}
			else if (type == PrimitiveTypeWrapper.DOUBLE)
			{
				ilgen.Emit(OpCodes.Call, valueOfDouble);
			}
		}

		[IKVM.Attributes.HideFromJava]
		public object invoke(object obj, object[] args, ikvm.@internal.CallerID callerID)
		{
			try
			{
				return invoker(obj, args, callerID);
			}
			catch (MethodAccessException x)
			{
				// this can happen if we're calling a non-public method and the call stack doesn't have ReflectionPermission.MemberAccess
				throw new java.lang.IllegalAccessException().initCause(x);
			}
		}
	}

	private sealed class FastConstructorAccessorImpl : sun.reflect.ConstructorAccessor
	{
		private delegate object Invoker(object[] args);
		private Invoker invoker;

		internal FastConstructorAccessorImpl(java.lang.reflect.Constructor constructor)
		{
			MethodWrapper mw = MethodWrapper.FromMethodOrConstructor(constructor);
			TypeWrapper[] parameters;
			try
			{
				mw.DeclaringType.Finish();
				parameters = mw.GetParameters();
				for (int i = 0; i < parameters.Length; i++)
				{
					// the EnsureLoadable shouldn't fail, because we don't allow a java.lang.reflect.Method
					// to "escape" if it has an unloadable type in the signature
					parameters[i] = parameters[i].EnsureLoadable(mw.DeclaringType.GetClassLoader());
					parameters[i].Finish();
				}
			}
			catch (RetargetableJavaException x)
			{
				throw x.ToJava();
			}
			mw.ResolveMethod();
			DynamicMethod dm = DynamicMethodUtils.Create("__<Invoker>", mw.DeclaringType.TypeAsTBD, !mw.IsPublic || !mw.DeclaringType.IsPublic, typeof(object), new Type[] { typeof(object[]) });
			CodeEmitter ilgen = CodeEmitter.Create(dm);
			CodeEmitterLocal ret = ilgen.DeclareLocal(typeof(object));

			// check args length
			CodeEmitterLabel argsLengthOK = ilgen.DefineLabel();
			if (parameters.Length == 0)
			{
				// zero length array may be null
				ilgen.Emit(OpCodes.Ldarg_0);
				ilgen.EmitBrfalse(argsLengthOK);
			}
			ilgen.Emit(OpCodes.Ldarg_0);
			ilgen.Emit(OpCodes.Ldlen);
			ilgen.EmitLdc_I4(parameters.Length);
			ilgen.EmitBeq(argsLengthOK);
			ilgen.Emit(OpCodes.Newobj, FastMethodAccessorImpl.illegalArgumentExceptionCtor);
			ilgen.Emit(OpCodes.Throw);
			ilgen.MarkLabel(argsLengthOK);

			CodeEmitterLocal[] args = new CodeEmitterLocal[parameters.Length];
			for (int i = 0; i < args.Length; i++)
			{
				args[i] = ilgen.DeclareLocal(parameters[i].TypeAsSignatureType);
			}
			ilgen.BeginExceptionBlock();
			for (int i = 0; i < args.Length; i++)
			{
				ilgen.Emit(OpCodes.Ldarg_0);
				ilgen.EmitLdc_I4(i);
				ilgen.Emit(OpCodes.Ldelem_Ref);
				TypeWrapper tw = parameters[i];
				FastMethodAccessorImpl.EmitUnboxArg(ilgen, tw);
				tw.EmitConvStackTypeToSignatureType(ilgen, null);
				ilgen.Emit(OpCodes.Stloc, args[i]);
			}
			CodeEmitterLabel label1 = ilgen.DefineLabel();
			ilgen.EmitLeave(label1);
			ilgen.BeginCatchBlock(typeof(InvalidCastException));
			ilgen.Emit(OpCodes.Newobj, FastMethodAccessorImpl.illegalArgumentExceptionCtor);
			ilgen.Emit(OpCodes.Throw);
			ilgen.BeginCatchBlock(typeof(NullReferenceException));
			ilgen.Emit(OpCodes.Newobj, FastMethodAccessorImpl.illegalArgumentExceptionCtor);
			ilgen.Emit(OpCodes.Throw);
			ilgen.EndExceptionBlock();

			// this is the actual call
			ilgen.MarkLabel(label1);
			ilgen.BeginExceptionBlock();
			for (int i = 0; i < args.Length; i++)
			{
				ilgen.Emit(OpCodes.Ldloc, args[i]);
			}
			mw.EmitNewobj(ilgen);
			ilgen.Emit(OpCodes.Stloc, ret);
			CodeEmitterLabel label2 = ilgen.DefineLabel();
			ilgen.EmitLeave(label2);
			ilgen.BeginCatchBlock(typeof(Exception));
			ilgen.Emit(OpCodes.Dup);
			ilgen.Emit(OpCodes.Callvirt, FastMethodAccessorImpl.get_TargetSite);
			ilgen.Emit(OpCodes.Call, FastMethodAccessorImpl.GetCurrentMethod);
			ilgen.Emit(OpCodes.Ceq);
			CodeEmitterLabel label = ilgen.DefineLabel();
			ilgen.EmitBrtrue(label);
			ilgen.Emit(OpCodes.Ldc_I4_0);
			ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.mapException.MakeGenericMethod(Types.Exception));
			ilgen.Emit(OpCodes.Newobj, FastMethodAccessorImpl.invocationTargetExceptionCtor);
			ilgen.MarkLabel(label);
			ilgen.Emit(OpCodes.Throw);
			ilgen.EndExceptionBlock();

			ilgen.MarkLabel(label2);
			ilgen.Emit(OpCodes.Ldloc, ret);
			ilgen.Emit(OpCodes.Ret);
			ilgen.DoEmit();
			invoker = (Invoker)dm.CreateDelegate(typeof(Invoker));
		}

		[IKVM.Attributes.HideFromJava]
		public object newInstance(object[] args)
		{
			try
			{
				return invoker(args);
			}
			catch (MethodAccessException x)
			{
				// this can happen if we're calling a non-public method and the call stack doesn't have ReflectionPermission.MemberAccess
				throw new java.lang.IllegalAccessException().initCause(x);
			}
		}
	}

	private sealed class FastSerializationConstructorAccessorImpl : sun.reflect.ConstructorAccessor
	{
		private static readonly MethodInfo GetTypeFromHandleMethod = typeof(Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) });
		private static readonly MethodInfo GetUninitializedObjectMethod = typeof(FormatterServices).GetMethod("GetUninitializedObject", new Type[] { typeof(Type) });
		private delegate object InvokeCtor();
		private InvokeCtor invoker;

		internal FastSerializationConstructorAccessorImpl(java.lang.reflect.Constructor constructorToCall, java.lang.Class classToInstantiate)
		{
			MethodWrapper constructor = MethodWrapper.FromMethodOrConstructor(constructorToCall);
			if (constructor.GetParameters().Length != 0)
			{
				throw new NotImplementedException("Serialization constructor cannot have parameters");
			}
			constructor.Link();
			constructor.ResolveMethod();
			Type type;
			try
			{
				TypeWrapper wrapper = TypeWrapper.FromClass(classToInstantiate);
				wrapper.Finish();
				type = wrapper.TypeAsBaseType;
			}
			catch (RetargetableJavaException x)
			{
				throw x.ToJava();
			}
			DynamicMethod dm = DynamicMethodUtils.Create("__<SerializationCtor>", constructor.DeclaringType.TypeAsBaseType, true, typeof(object), null);
			CodeEmitter ilgen = CodeEmitter.Create(dm);
			ilgen.Emit(OpCodes.Ldtoken, type);
			ilgen.Emit(OpCodes.Call, GetTypeFromHandleMethod);
			ilgen.Emit(OpCodes.Call, GetUninitializedObjectMethod);
			ilgen.Emit(OpCodes.Dup);
			constructor.EmitCall(ilgen);
			ilgen.Emit(OpCodes.Ret);
			ilgen.DoEmit();
			invoker = (InvokeCtor)dm.CreateDelegate(typeof(InvokeCtor));
		}

		[IKVM.Attributes.HideFromJava]
		public object newInstance(object[] args)
		{
			try
			{
				return invoker();
			}
			catch (MethodAccessException x)
			{
				// this can happen if we're calling a non-public method and the call stack doesn't have ReflectionPermission.MemberAccess
				throw new java.lang.IllegalAccessException().initCause(x);
			}
		}
	}

	sealed class ActivatorConstructorAccessor : sun.reflect.ConstructorAccessor
	{
		private readonly Type type;

		internal ActivatorConstructorAccessor(MethodWrapper mw)
		{
			this.type = mw.DeclaringType.TypeAsBaseType;
		}

		public object newInstance(object[] objarr)
		{
			if (objarr != null && objarr.Length != 0)
			{
				throw new java.lang.IllegalArgumentException();
			}
			try
			{
				return Activator.CreateInstance(type);
			}
			catch (TargetInvocationException x)
			{
				throw new java.lang.reflect.InvocationTargetException(ikvm.runtime.Util.mapException(x.InnerException));
			}
		}

		internal static bool IsSuitable(MethodWrapper mw)
		{
			MethodBase mb = mw.GetMethod();
			return mb != null
				&& mb.IsConstructor
				&& mb.IsPublic
				&& mb.DeclaringType.IsPublic
				&& mb.DeclaringType == mw.DeclaringType.TypeAsBaseType
				&& mb.GetParameters().Length == 0;
		}
	}

	private abstract class FieldAccessorImplBase : sun.reflect.FieldAccessor, IReflectionException
	{
		protected static readonly ushort inflationThreshold = 15;
		protected readonly FieldWrapper fw;
		protected readonly bool isFinal;
		protected ushort numInvocations;

		static FieldAccessorImplBase()
		{
			string str = java.lang.Props.props.getProperty("ikvm.reflect.field.inflationThreshold");
			int value;
			if (str != null && int.TryParse(str, out value))
			{
				if (value >= ushort.MinValue && value <= ushort.MaxValue)
				{
					inflationThreshold = (ushort)value;
				}
			}
		}

		private FieldAccessorImplBase(FieldWrapper fw, bool overrideAccessCheck)
		{
			this.fw = fw;
			isFinal = (!overrideAccessCheck || fw.IsStatic) && fw.IsFinal;
		}

		private string GetQualifiedFieldName()
		{
			return fw.DeclaringType.Name + "." + fw.Name;
		}

		private string GetFieldTypeName()
		{
			return fw.FieldTypeWrapper.ClassObject.getName();
		}

		public java.lang.IllegalArgumentException GetIllegalArgumentException(object obj)
		{
			// LAME like JDK 6 we return the wrong exception message (talking about setting the field, instead of getting)
			return SetIllegalArgumentException(obj);
		}

		public java.lang.IllegalArgumentException SetIllegalArgumentException(object obj)
		{
			// LAME like JDK 6 we return the wrong exception message (when obj is the object, instead of the value)
			return SetIllegalArgumentException(obj != null ? ikvm.runtime.Util.getClassFromObject(obj).getName() : "", "");
		}

		private java.lang.IllegalArgumentException SetIllegalArgumentException(string attemptedType, string attemptedValue)
		{
			return new java.lang.IllegalArgumentException(GetSetMessage(attemptedType, attemptedValue));
		}

		protected java.lang.IllegalAccessException FinalFieldIllegalAccessException(object obj)
		{
			return FinalFieldIllegalAccessException(obj != null ? ikvm.runtime.Util.getClassFromObject(obj).getName() : "", "");
		}

		private java.lang.IllegalAccessException FinalFieldIllegalAccessException(string attemptedType, string attemptedValue)
		{
			return new java.lang.IllegalAccessException(GetSetMessage(attemptedType, attemptedValue));
		}

		private java.lang.IllegalArgumentException GetIllegalArgumentException(string type)
		{
			return new java.lang.IllegalArgumentException("Attempt to get " + GetFieldTypeName() + " field \"" + GetQualifiedFieldName() + "\" with illegal data type conversion to " + type);
		}

		// this message comes from sun.reflect.UnsafeFieldAccessorImpl
		private string GetSetMessage(String attemptedType, String attemptedValue)
		{
			String err = "Can not set";
			if (fw.IsStatic)
				err += " static";
			if (isFinal)
				err += " final";
			err += " " + GetFieldTypeName() + " field " + GetQualifiedFieldName() + " to ";
			if (attemptedValue.Length > 0)
			{
				err += "(" + attemptedType + ")" + attemptedValue;
			}
			else
			{
				if (attemptedType.Length > 0)
					err += attemptedType;
				else
					err += "null value";
			}
			return err;
		}

		public virtual bool getBoolean(object obj)
		{
			throw GetIllegalArgumentException("boolean");
		}

		public virtual byte getByte(object obj)
		{
			throw GetIllegalArgumentException("byte");
		}

		public virtual char getChar(object obj)
		{
			throw GetIllegalArgumentException("char");
		}

		public virtual short getShort(object obj)
		{
			throw GetIllegalArgumentException("short");
		}

		public virtual int getInt(object obj)
		{
			throw GetIllegalArgumentException("int");
		}

		public virtual long getLong(object obj)
		{
			throw GetIllegalArgumentException("long");
		}

		public virtual float getFloat(object obj)
		{
			throw GetIllegalArgumentException("float");
		}

		public virtual double getDouble(object obj)
		{
			throw GetIllegalArgumentException("double");
		}

		public virtual void setBoolean(object obj, bool z)
		{
			throw SetIllegalArgumentException("boolean", java.lang.Boolean.toString(z));
		}

		public virtual void setByte(object obj, byte b)
		{
			throw SetIllegalArgumentException("byte", java.lang.Byte.toString(b));
		}

		public virtual void setChar(object obj, char c)
		{
			throw SetIllegalArgumentException("char", java.lang.Character.toString(c));
		}

		public virtual void setShort(object obj, short s)
		{
			throw SetIllegalArgumentException("short", java.lang.Short.toString(s));
		}

		public virtual void setInt(object obj, int i)
		{
			throw SetIllegalArgumentException("int", java.lang.Integer.toString(i));
		}

		public virtual void setLong(object obj, long l)
		{
			throw SetIllegalArgumentException("long", java.lang.Long.toString(l));
		}

		public virtual void setFloat(object obj, float f)
		{
			throw SetIllegalArgumentException("float", java.lang.Float.toString(f));
		}

		public virtual void setDouble(object obj, double d)
		{
			throw SetIllegalArgumentException("double", java.lang.Double.toString(d));
		}

		public abstract object get(object obj);
		public abstract void set(object obj, object value);

		private abstract class FieldAccessor<T> : FieldAccessorImplBase
		{
			protected delegate void Setter(object obj, T value, FieldAccessor<T> acc);
			protected delegate T Getter(object obj, FieldAccessor<T> acc);
			private static readonly Setter initialSetter = lazySet;
			private static readonly Getter initialGetter = lazyGet;
			protected Setter setter = initialSetter;
			protected Getter getter = initialGetter;

			internal FieldAccessor(FieldWrapper fw, bool overrideAccessCheck)
				: base(fw, overrideAccessCheck)
			{
				if (!IsSlowPathCompatible(fw))
				{
					// prevent slow path
					numInvocations = inflationThreshold;
				}
			}

			private bool IsSlowPathCompatible(FieldWrapper fw)
			{
				if (fw.DeclaringType.IsRemapped)
				{
					return false;
				}
				if (fw.IsVolatile && (fw.FieldTypeWrapper == PrimitiveTypeWrapper.LONG || fw.FieldTypeWrapper == PrimitiveTypeWrapper.DOUBLE))
				{
					return false;
				}
				fw.Link();
				return true;
			}

			private static T lazyGet(object obj, FieldAccessor<T> acc)
			{
				return acc.lazyGet(obj);
			}

			private static void lazySet(object obj, T value, FieldAccessor<T> acc)
			{
				acc.lazySet(obj, value);
			}

			private T lazyGet(object obj)
			{
				if (numInvocations < inflationThreshold)
				{
					if (fw.IsStatic)
					{
						obj = null;
					}
					else if (obj == null)
					{
						throw new java.lang.NullPointerException();
					}
					else if (!fw.DeclaringType.IsInstance(obj))
					{
						throw GetIllegalArgumentException(obj);
					}
					if (numInvocations == 0)
					{
						fw.DeclaringType.RunClassInit();
						fw.DeclaringType.Finish();
						fw.ResolveField();
					}
					numInvocations++;
					if (fw.FieldTypeWrapper.IsGhost)
					{
						return (T)fw.FieldTypeWrapper.TypeAsTBD.GetMethod("ToObject").Invoke(fw.GetValue(obj), new object[0]);
					}
					else
					{
						return (T)fw.GetValue(obj);
					}
				}
				else
				{
					// FXBUG it appears that a ldsfld/stsfld in a DynamicMethod doesn't trigger the class constructor
					// and if we didn't use the slow path, we haven't yet initialized the class
					fw.DeclaringType.RunClassInit();
					getter = (Getter)GenerateFastGetter(typeof(Getter), typeof(T), fw);
					return getter(obj, this);
				}
			}

			private void lazySet(object obj, T value)
			{
				if (isFinal)
				{
					// for some reason Java runs class initialization before checking if the field is final
					fw.DeclaringType.RunClassInit();
					throw FinalFieldIllegalAccessException(JavaBox(value));
				}
				if (numInvocations < inflationThreshold)
				{
					if (fw.IsStatic)
					{
						obj = null;
					}
					else if (obj == null)
					{
						throw new java.lang.NullPointerException();
					}
					else if (!fw.DeclaringType.IsInstance(obj))
					{
						throw SetIllegalArgumentException(obj);
					}
					CheckValue(value);
					if (numInvocations == 0)
					{
						fw.DeclaringType.RunClassInit();
						fw.DeclaringType.Finish();
						fw.ResolveField();
					}
					numInvocations++;
					if (fw.FieldTypeWrapper.IsGhost)
					{
						fw.SetValue(obj, fw.FieldTypeWrapper.TypeAsTBD.GetMethod("Cast").Invoke(null, new object[] { value }));
					}
					else
					{
						fw.SetValue(obj, value);
					}
				}
				else
				{
					// FXBUG it appears that a ldsfld/stsfld in a DynamicMethod doesn't trigger the class constructor
					// and if we didn't use the slow path, we haven't yet initialized the class
					fw.DeclaringType.RunClassInit();
					setter = (Setter)GenerateFastSetter(typeof(Setter), typeof(T), fw);
					setter(obj, value, this);
				}
			}

			protected virtual void CheckValue(T value)
			{
			}

			protected abstract object JavaBox(T value);
		}

		private sealed class ByteField : FieldAccessor<byte>
		{
			internal ByteField(FieldWrapper field, bool overrideAccessCheck)
				: base(field, overrideAccessCheck)
			{
			}

			public sealed override short getShort(object obj)
			{
				return (sbyte)getByte(obj);
			}

			public sealed override int getInt(object obj)
			{
				return (sbyte)getByte(obj);
			}

			public sealed override long getLong(object obj)
			{
				return (sbyte)getByte(obj);
			}

			public sealed override float getFloat(object obj)
			{
				return (sbyte)getByte(obj);
			}

			public sealed override double getDouble(object obj)
			{
				return (sbyte)getByte(obj);
			}

			public sealed override object get(object obj)
			{
				return java.lang.Byte.valueOf(getByte(obj));
			}

			public sealed override void set(object obj, object val)
			{
				if (!(val is java.lang.Byte))
				{
					throw SetIllegalArgumentException(val);
				}
				setByte(obj, ((java.lang.Byte)val).byteValue());
			}

			public sealed override byte getByte(object obj)
			{
				try
				{
					return getter(obj, this);
				}
				catch (FieldAccessException x)
				{
					throw new java.lang.IllegalAccessException().initCause(x);
				}
			}

			public sealed override void setByte(object obj, byte value)
			{
				try
				{
					setter(obj, value, this);
				}
				catch (FieldAccessException x)
				{
					throw new java.lang.IllegalAccessException().initCause(x);
				}
			}

			protected sealed override object JavaBox(byte value)
			{
				return java.lang.Byte.valueOf(value);
			}
		}

		private sealed class BooleanField : FieldAccessor<bool>
		{
			internal BooleanField(FieldWrapper field, bool overrideAccessCheck)
				: base(field, overrideAccessCheck)
			{
			}

			public sealed override object get(object obj)
			{
				return java.lang.Boolean.valueOf(getBoolean(obj));
			}

			public sealed override void set(object obj, object val)
			{
				if (!(val is java.lang.Boolean))
				{
					throw SetIllegalArgumentException(val);
				}
				setBoolean(obj, ((java.lang.Boolean)val).booleanValue());
			}

			public sealed override bool getBoolean(object obj)
			{
				try
				{
					return getter(obj, this);
				}
				catch (FieldAccessException x)
				{
					throw new java.lang.IllegalAccessException().initCause(x);
				}
			}

			public sealed override void setBoolean(object obj, bool value)
			{
				try
				{
					setter(obj, value, this);
				}
				catch (FieldAccessException x)
				{
					throw new java.lang.IllegalAccessException().initCause(x);
				}
			}

			protected sealed override object JavaBox(bool value)
			{
				return java.lang.Boolean.valueOf(value);
			}
		}

		private sealed class CharField : FieldAccessor<char>
		{
			internal CharField(FieldWrapper field, bool overrideAccessCheck)
				: base(field, overrideAccessCheck)
			{
			}

			public sealed override int getInt(object obj)
			{
				return getChar(obj);
			}

			public sealed override long getLong(object obj)
			{
				return getChar(obj);
			}

			public sealed override float getFloat(object obj)
			{
				return getChar(obj);
			}

			public sealed override double getDouble(object obj)
			{
				return getChar(obj);
			}

			public sealed override object get(object obj)
			{
				return java.lang.Character.valueOf(getChar(obj));
			}

			public sealed override void set(object obj, object val)
			{
				if (val is java.lang.Character)
					setChar(obj, ((java.lang.Character)val).charValue());
				else
					throw SetIllegalArgumentException(val);
			}

			public sealed override char getChar(object obj)
			{
				try
				{
					return getter(obj, this);
				}
				catch (FieldAccessException x)
				{
					throw new java.lang.IllegalAccessException().initCause(x);
				}
			}

			public sealed override void setChar(object obj, char value)
			{
				try
				{
					setter(obj, value, this);
				}
				catch (FieldAccessException x)
				{
					throw new java.lang.IllegalAccessException().initCause(x);
				}
			}

			protected sealed override object JavaBox(char value)
			{
				return java.lang.Character.valueOf(value);
			}
		}

		private sealed class ShortField : FieldAccessor<short>
		{
			internal ShortField(FieldWrapper field, bool overrideAccessCheck)
				: base(field, overrideAccessCheck)
			{
			}

			public sealed override int getInt(object obj)
			{
				return getShort(obj);
			}

			public sealed override long getLong(object obj)
			{
				return getShort(obj);
			}

			public sealed override float getFloat(object obj)
			{
				return getShort(obj);
			}

			public sealed override double getDouble(object obj)
			{
				return getShort(obj);
			}

			public sealed override object get(object obj)
			{
				return java.lang.Short.valueOf(getShort(obj));
			}

			public sealed override void set(object obj, object val)
			{
				if (val is java.lang.Byte
					|| val is java.lang.Short)
					setShort(obj, ((java.lang.Number)val).shortValue());
				else
					throw SetIllegalArgumentException(val);
			}

			public sealed override void setByte(object obj, byte b)
			{
				setShort(obj, (sbyte)b);
			}

			public sealed override short getShort(object obj)
			{
				try
				{
					return getter(obj, this);
				}
				catch (FieldAccessException x)
				{
					throw new java.lang.IllegalAccessException().initCause(x);
				}
			}

			public sealed override void setShort(object obj, short value)
			{
				try
				{
					setter(obj, value, this);
				}
				catch (FieldAccessException x)
				{
					throw new java.lang.IllegalAccessException().initCause(x);
				}
			}

			protected sealed override object JavaBox(short value)
			{
				return java.lang.Short.valueOf(value);
			}
		}

		private sealed class IntField : FieldAccessor<int>
		{
			internal IntField(FieldWrapper field, bool overrideAccessCheck)
				: base(field, overrideAccessCheck)
			{
			}

			public sealed override long getLong(object obj)
			{
				return getInt(obj);
			}

			public sealed override float getFloat(object obj)
			{
				return getInt(obj);
			}

			public sealed override double getDouble(object obj)
			{
				return getInt(obj);
			}

			public sealed override object get(object obj)
			{
				return java.lang.Integer.valueOf(getInt(obj));
			}

			public sealed override void set(object obj, object val)
			{
				if (val is java.lang.Byte
					|| val is java.lang.Short
					|| val is java.lang.Integer)
					setInt(obj, ((java.lang.Number)val).intValue());
				else if (val is java.lang.Character)
					setInt(obj, ((java.lang.Character)val).charValue());
				else
					throw SetIllegalArgumentException(val);
			}

			public sealed override void setByte(object obj, byte b)
			{
				setInt(obj, (sbyte)b);
			}

			public sealed override void setChar(object obj, char c)
			{
				setInt(obj, c);
			}

			public sealed override void setShort(object obj, short s)
			{
				setInt(obj, s);
			}

			public sealed override int getInt(object obj)
			{
				try
				{
					return getter(obj, this);
				}
				catch (FieldAccessException x)
				{
					throw new java.lang.IllegalAccessException().initCause(x);
				}
			}

			public sealed override void setInt(object obj, int value)
			{
				try
				{
					setter(obj, value, this);
				}
				catch (FieldAccessException x)
				{
					throw new java.lang.IllegalAccessException().initCause(x);
				}
			}

			protected sealed override object JavaBox(int value)
			{
				return java.lang.Integer.valueOf(value);
			}
		}

		private sealed class FloatField : FieldAccessor<float>
		{
			internal FloatField(FieldWrapper field, bool overrideAccessCheck)
				: base(field, overrideAccessCheck)
			{
			}

			public sealed override double getDouble(object obj)
			{
				return getFloat(obj);
			}

			public sealed override object get(object obj)
			{
				return java.lang.Float.valueOf(getFloat(obj));
			}

			public sealed override void set(object obj, object val)
			{
				if (val is java.lang.Float
					|| val is java.lang.Byte
					|| val is java.lang.Short
					|| val is java.lang.Integer
					|| val is java.lang.Long)
					setFloat(obj, ((java.lang.Number)val).floatValue());
				else if (val is java.lang.Character)
					setFloat(obj, ((java.lang.Character)val).charValue());
				else
					throw SetIllegalArgumentException(val);
			}

			public sealed override void setByte(object obj, byte b)
			{
				setFloat(obj, (sbyte)b);
			}

			public sealed override void setChar(object obj, char c)
			{
				setFloat(obj, c);
			}

			public sealed override void setShort(object obj, short s)
			{
				setFloat(obj, s);
			}

			public sealed override void setInt(object obj, int i)
			{
				setFloat(obj, i);
			}

			public sealed override void setLong(object obj, long l)
			{
				setFloat(obj, l);
			}

			public sealed override float getFloat(object obj)
			{
				try
				{
					return getter(obj, this);
				}
				catch (FieldAccessException x)
				{
					throw new java.lang.IllegalAccessException().initCause(x);
				}
			}

			public sealed override void setFloat(object obj, float value)
			{
				try
				{
					setter(obj, value, this);
				}
				catch (FieldAccessException x)
				{
					throw new java.lang.IllegalAccessException().initCause(x);
				}
			}

			protected sealed override object JavaBox(float value)
			{
				return java.lang.Float.valueOf(value);
			}
		}

		private sealed class LongField : FieldAccessor<long>
		{
			internal LongField(FieldWrapper field, bool overrideAccessCheck)
				: base(field, overrideAccessCheck)
			{
			}

			public sealed override float getFloat(object obj)
			{
				return getLong(obj);
			}

			public sealed override double getDouble(object obj)
			{
				return getLong(obj);
			}

			public sealed override object get(object obj)
			{
				return java.lang.Long.valueOf(getLong(obj));
			}

			public sealed override void set(object obj, object val)
			{
				if (val is java.lang.Long
					|| val is java.lang.Byte
					|| val is java.lang.Short
					|| val is java.lang.Integer)
					setLong(obj, ((java.lang.Number)val).longValue());
				else if (val is java.lang.Character)
					setLong(obj, ((java.lang.Character)val).charValue());
				else
					throw SetIllegalArgumentException(val);
			}

			public sealed override void setByte(object obj, byte b)
			{
				setLong(obj, (sbyte)b);
			}

			public sealed override void setChar(object obj, char c)
			{
				setLong(obj, c);
			}

			public sealed override void setShort(object obj, short s)
			{
				setLong(obj, s);
			}

			public sealed override void setInt(object obj, int i)
			{
				setLong(obj, i);
			}

			public sealed override long getLong(object obj)
			{
				try
				{
					return getter(obj, this);
				}
				catch (FieldAccessException x)
				{
					throw new java.lang.IllegalAccessException().initCause(x);
				}
			}

			public sealed override void setLong(object obj, long value)
			{
				try
				{
					setter(obj, value, this);
				}
				catch (FieldAccessException x)
				{
					throw new java.lang.IllegalAccessException().initCause(x);
				}
			}

			protected sealed override object JavaBox(long value)
			{
				return java.lang.Long.valueOf(value);
			}
		}

		private sealed class DoubleField : FieldAccessor<double>
		{
			internal DoubleField(FieldWrapper field, bool overrideAccessCheck)
				: base(field, overrideAccessCheck)
			{
			}

			public sealed override object get(object obj)
			{
				return java.lang.Double.valueOf(getDouble(obj));
			}

			public sealed override void set(object obj, object val)
			{
				if (val is java.lang.Double
					|| val is java.lang.Float
					|| val is java.lang.Byte
					|| val is java.lang.Short
					|| val is java.lang.Integer
					|| val is java.lang.Long)
					setDouble(obj, ((java.lang.Number)val).doubleValue());
				else if (val is java.lang.Character)
					setDouble(obj, ((java.lang.Character)val).charValue());
				else
					throw SetIllegalArgumentException(val);
			}

			public sealed override void setByte(object obj, byte b)
			{
				setDouble(obj, (sbyte)b);
			}

			public sealed override void setChar(object obj, char c)
			{
				setDouble(obj, c);
			}

			public sealed override void setShort(object obj, short s)
			{
				setDouble(obj, s);
			}

			public sealed override void setInt(object obj, int i)
			{
				setDouble(obj, i);
			}

			public sealed override void setLong(object obj, long l)
			{
				setDouble(obj, l);
			}

			public sealed override void setFloat(object obj, float f)
			{
				setDouble(obj, f);
			}

			public sealed override double getDouble(object obj)
			{
				try
				{
					return getter(obj, this);
				}
				catch (FieldAccessException x)
				{
					throw new java.lang.IllegalAccessException().initCause(x);
				}
			}

			public sealed override void setDouble(object obj, double value)
			{
				try
				{
					setter(obj, value, this);
				}
				catch (FieldAccessException x)
				{
					throw new java.lang.IllegalAccessException().initCause(x);
				}
			}

			protected sealed override object JavaBox(double value)
			{
				return java.lang.Double.valueOf(value);
			}
		}

		private sealed class ObjectField : FieldAccessor<object>
		{
			internal ObjectField(FieldWrapper field, bool overrideAccessCheck)
				: base(field, overrideAccessCheck)
			{
			}

			protected sealed override void CheckValue(object value)
			{
				if (value != null && !fw.FieldTypeWrapper.EnsureLoadable(fw.DeclaringType.GetClassLoader()).IsInstance(value))
				{
					throw SetIllegalArgumentException(value);
				}
			}

			public sealed override object get(object obj)
			{
				try
				{
					return getter(obj, this);
				}
				catch (FieldAccessException x)
				{
					throw new java.lang.IllegalAccessException().initCause(x);
				}
			}

			public sealed override void set(object obj, object value)
			{
				try
				{
					setter(obj, value, this);
				}
				catch (FieldAccessException x)
				{
					throw new java.lang.IllegalAccessException().initCause(x);
				}
			}

			protected sealed override object JavaBox(object value)
			{
				return value;
			}
		}

		private Delegate GenerateFastGetter(Type delegateType, Type fieldType, FieldWrapper fw)
		{
			TypeWrapper fieldTypeWrapper;
			try
			{
				fieldTypeWrapper = fw.FieldTypeWrapper.EnsureLoadable(fw.DeclaringType.GetClassLoader());
				fieldTypeWrapper.Finish();
				fw.DeclaringType.Finish();
			}
			catch (RetargetableJavaException x)
			{
				throw x.ToJava();
			}
			fw.ResolveField();
			DynamicMethod dm = DynamicMethodUtils.Create("__<Getter>", fw.DeclaringType.TypeAsBaseType, !fw.IsPublic || !fw.DeclaringType.IsPublic, fieldType, new Type[] { typeof(IReflectionException), typeof(object), typeof(object) });
			CodeEmitter ilgen = CodeEmitter.Create(dm);
			if (fw.IsStatic)
			{
				fw.EmitGet(ilgen);
				fieldTypeWrapper.EmitConvSignatureTypeToStackType(ilgen);
			}
			else
			{
				ilgen.BeginExceptionBlock();
				ilgen.Emit(OpCodes.Ldarg_1);
				ilgen.Emit(OpCodes.Castclass, fw.DeclaringType.TypeAsBaseType);
				fw.EmitGet(ilgen);
				fieldTypeWrapper.EmitConvSignatureTypeToStackType(ilgen);
				CodeEmitterLocal local = ilgen.DeclareLocal(fieldType);
				ilgen.Emit(OpCodes.Stloc, local);
				CodeEmitterLabel label = ilgen.DefineLabel();
				ilgen.EmitLeave(label);
				ilgen.BeginCatchBlock(typeof(InvalidCastException));
				ilgen.Emit(OpCodes.Ldarg_0);
				ilgen.Emit(OpCodes.Ldarg_1);
				ilgen.Emit(OpCodes.Callvirt, typeof(IReflectionException).GetMethod("GetIllegalArgumentException"));
				ilgen.Emit(OpCodes.Throw);
				ilgen.EndExceptionBlock();
				ilgen.MarkLabel(label);
				ilgen.Emit(OpCodes.Ldloc, local);
			}
			ilgen.Emit(OpCodes.Ret);
			ilgen.DoEmit();
			return dm.CreateDelegate(delegateType, this);
		}

		private Delegate GenerateFastSetter(Type delegateType, Type fieldType, FieldWrapper fw)
		{
			TypeWrapper fieldTypeWrapper;
			try
			{
				fieldTypeWrapper = fw.FieldTypeWrapper.EnsureLoadable(fw.DeclaringType.GetClassLoader());
				fieldTypeWrapper.Finish();
				fw.DeclaringType.Finish();
			}
			catch (RetargetableJavaException x)
			{
				throw x.ToJava();
			}
			fw.ResolveField();
			DynamicMethod dm = DynamicMethodUtils.Create("__<Setter>", fw.DeclaringType.TypeAsBaseType, !fw.IsPublic || !fw.DeclaringType.IsPublic, null, new Type[] { typeof(IReflectionException), typeof(object), fieldType, typeof(object) });
			CodeEmitter ilgen = CodeEmitter.Create(dm);
			if (fw.IsStatic)
			{
				if (fieldType == typeof(object))
				{
					ilgen.BeginExceptionBlock();
					ilgen.Emit(OpCodes.Ldarg_2);
					fieldTypeWrapper.EmitCheckcast(ilgen);
					fieldTypeWrapper.EmitConvStackTypeToSignatureType(ilgen, null);
					fw.EmitSet(ilgen);
					CodeEmitterLabel label = ilgen.DefineLabel();
					ilgen.EmitLeave(label);
					ilgen.BeginCatchBlock(typeof(InvalidCastException));
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Ldarg_1);
					ilgen.Emit(OpCodes.Callvirt, typeof(IReflectionException).GetMethod("SetIllegalArgumentException"));
					ilgen.Emit(OpCodes.Throw);
					ilgen.EndExceptionBlock();
					ilgen.MarkLabel(label);
				}
				else
				{
					ilgen.Emit(OpCodes.Ldarg_2);
					fw.EmitSet(ilgen);
				}
			}
			else
			{
				ilgen.BeginExceptionBlock();
				ilgen.Emit(OpCodes.Ldarg_1);
				ilgen.Emit(OpCodes.Castclass, fw.DeclaringType.TypeAsBaseType);
				ilgen.Emit(OpCodes.Ldarg_2);
				if (fieldType == typeof(object))
				{
					fieldTypeWrapper.EmitCheckcast(ilgen);
				}
				fieldTypeWrapper.EmitConvStackTypeToSignatureType(ilgen, null);
				fw.EmitSet(ilgen);
				CodeEmitterLabel label = ilgen.DefineLabel();
				ilgen.EmitLeave(label);
				ilgen.BeginCatchBlock(typeof(InvalidCastException));
				ilgen.Emit(OpCodes.Ldarg_0);
				ilgen.Emit(OpCodes.Ldarg_1);
				ilgen.Emit(OpCodes.Callvirt, typeof(IReflectionException).GetMethod("SetIllegalArgumentException"));
				ilgen.Emit(OpCodes.Throw);
				ilgen.EndExceptionBlock();
				ilgen.MarkLabel(label);
			}
			ilgen.Emit(OpCodes.Ret);
			ilgen.DoEmit();
			return dm.CreateDelegate(delegateType, this);
		}

		internal static FieldAccessorImplBase Create(FieldWrapper field, bool overrideAccessCheck)
		{
			TypeWrapper type = field.FieldTypeWrapper;
			if (type.IsPrimitive)
			{
				if (type == PrimitiveTypeWrapper.BYTE)
				{
					return new ByteField(field, overrideAccessCheck);
				}
				if (type == PrimitiveTypeWrapper.BOOLEAN)
				{
					return new BooleanField(field, overrideAccessCheck);
				}
				if (type == PrimitiveTypeWrapper.CHAR)
				{
					return new CharField(field, overrideAccessCheck);
				}
				if (type == PrimitiveTypeWrapper.SHORT)
				{
					return new ShortField(field, overrideAccessCheck);
				}
				if (type == PrimitiveTypeWrapper.INT)
				{
					return new IntField(field, overrideAccessCheck);
				}
				if (type == PrimitiveTypeWrapper.FLOAT)
				{
					return new FloatField(field, overrideAccessCheck);
				}
				if (type == PrimitiveTypeWrapper.LONG)
				{
					return new LongField(field, overrideAccessCheck);
				}
				if (type == PrimitiveTypeWrapper.DOUBLE)
				{
					return new DoubleField(field, overrideAccessCheck);
				}
				throw new InvalidOperationException("field type: " + type);
			}
			else
			{
				return new ObjectField(field, overrideAccessCheck);
			}
		}
	}
#endif

	public static object newFieldAccessor(object thisFactory, object field, bool overrideAccessCheck)
	{
#if FIRST_PASS
		return null;
#else
		return FieldAccessorImplBase.Create(FieldWrapper.FromField(field), overrideAccessCheck);
#endif
	}

#if !FIRST_PASS
	internal static sun.reflect.FieldAccessor NewFieldAccessorJNI(FieldWrapper field)
	{
		return FieldAccessorImplBase.Create(field, true);
	}
#endif

	public static object newMethodAccessor(object thisFactory, object method)
	{
#if FIRST_PASS
		return null;
#else
		java.lang.reflect.Method m = (java.lang.reflect.Method)method;
		MethodWrapper mw = MethodWrapper.FromMethodOrConstructor(method);
		if (mw is ICustomInvoke)
		{
			return new MethodAccessorImpl(m);
		}
		else
		{
			return new FastMethodAccessorImpl(m, false);
		}
#endif
	}

	public static object newConstructorAccessor0(object thisFactory, object constructor)
	{
#if FIRST_PASS
		return null;
#else
		java.lang.reflect.Constructor cons = (java.lang.reflect.Constructor)constructor;
		MethodWrapper mw = MethodWrapper.FromMethodOrConstructor(constructor);
		if (ActivatorConstructorAccessor.IsSuitable(mw))
		{
			// we special case public default constructors, because in that case using Activator.CreateInstance()
			// is almost as fast as FastConstructorAccessorImpl, but it saves us significantly in working set and
			// startup time (because often during startup a sun.nio.cs.* encoder is instantiated using reflection)
			return new ActivatorConstructorAccessor(mw);
		}
		else
		{
			return new FastConstructorAccessorImpl(cons);
		}
#endif
	}

	public static object newConstructorAccessorForSerialization(java.lang.Class classToInstantiate, java.lang.reflect.Constructor constructorToCall)
	{
#if FIRST_PASS
		return null;
#else
		try
		{
			return new FastSerializationConstructorAccessorImpl(constructorToCall, classToInstantiate);
		}
		catch (SecurityException x)
		{
			throw new java.lang.SecurityException(x.Message, ikvm.runtime.Util.mapException(x));
		}
#endif
	}
}

static class Java_sun_reflect_ConstantPool
{
	public static int getSize0(object thisConstantPool, object constantPoolOop)
	{
		throw new NotImplementedException();
	}

	public static object getClassAt0(object thisConstantPool, object constantPoolOop, int index)
	{
		throw new NotImplementedException();
	}

	public static object getClassAtIfLoaded0(object thisConstantPool, object constantPoolOop, int index)
	{
		throw new NotImplementedException();
	}

	public static object getMethodAt0(object thisConstantPool, object constantPoolOop, int index)
	{
		throw new NotImplementedException();
	}

	public static object getMethodAtIfLoaded0(object thisConstantPool, object constantPoolOop, int index)
	{
		throw new NotImplementedException();
	}

	public static object getFieldAt0(object thisConstantPool, object constantPoolOop, int index)
	{
		throw new NotImplementedException();
	}

	public static object getFieldAtIfLoaded0(object thisConstantPool, object constantPoolOop, int index)
	{
		throw new NotImplementedException();
	}

	public static string[] getMemberRefInfoAt0(object thisConstantPool, object constantPoolOop, int index)
	{
		throw new NotImplementedException();
	}

	public static int getIntAt0(object thisConstantPool, object constantPoolOop, int index)
	{
		throw new NotImplementedException();
	}

	public static long getLongAt0(object thisConstantPool, object constantPoolOop, int index)
	{
		throw new NotImplementedException();
	}

	public static float getFloatAt0(object thisConstantPool, object constantPoolOop, int index)
	{
		throw new NotImplementedException();
	}

	public static double getDoubleAt0(object thisConstantPool, object constantPoolOop, int index)
	{
		throw new NotImplementedException();
	}

	public static string getStringAt0(object thisConstantPool, object constantPoolOop, int index)
	{
		throw new NotImplementedException();
	}

	public static string getUTF8At0(object thisConstantPool, object constantPoolOop, int index)
	{
		throw new NotImplementedException();
	}
}
