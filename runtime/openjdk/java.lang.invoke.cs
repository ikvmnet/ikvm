/*
  Copyright (C) 2011-2013 Jeroen Frijters

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
using System.Reflection;
using System.Reflection.Emit;
using IKVM.Internal;
using java.lang.invoke;
using jlClass = java.lang.Class;

static class Java_java_lang_invoke_BoundMethodHandle
{
	public static object createDelegate(MethodType newType, MethodHandle mh, int argnum, object argument)
	{
#if FIRST_PASS
		return null;
#else
		Delegate del = (Delegate)mh.vmtarget;
		if (argnum == 0
			&& del.Target == null
			// we don't have to check for instance methods on a Value Type, because DirectMethodHandle can't use a direct delegate for that anyway
			&& (!del.Method.IsStatic || !del.Method.GetParameters()[0].ParameterType.IsValueType)
			&& !ReflectUtil.IsDynamicMethod(del.Method))
		{
			return Delegate.CreateDelegate(MethodHandleUtil.CreateDelegateType(newType), argument, del.Method);
		}
		else
		{
			// slow path where we're generating a DynamicMethod
			if (mh.type().parameterType(argnum).isPrimitive())
			{
				argument = JVM.Unbox(argument);
			}
			MethodHandleUtil.DynamicMethodBuilder dm = new MethodHandleUtil.DynamicMethodBuilder("BoundMethodHandle", newType, mh, argument);
			for (int i = 0, count = mh.type().parameterCount(), pos = 0; i < count; i++)
			{
				if (i == argnum)
				{
					dm.LoadValue();
				}
				else
				{
					dm.Ldarg(pos++);
				}
			}
			dm.CallTarget();
			dm.Ret();
			return dm.CreateDelegate();
		}
#endif
	}
}

static class Java_java_lang_invoke_CallSite
{
	public static object createIndyCallSite(object target)
	{
		return Activator.CreateInstance(typeof(IKVM.Runtime.IndyCallSite<>).MakeGenericType(target.GetType()), true);
	}
}

static class Java_java_lang_invoke_DirectMethodHandle
{
    public static object createDelegate(MethodType type, MemberName m, bool doDispatch, jlClass lookupClass)
	{
#if FIRST_PASS
		return null;
#else
		int index = m.getVMIndex();
		if (index == Int32.MaxValue)
		{
			bool invokeExact = m.getName() == "invokeExact";
			Type targetDelegateType = MethodHandleUtil.CreateDelegateType(invokeExact ? type.dropParameterTypes(0, 1) : type);
			MethodHandleUtil.DynamicMethodBuilder dm = new MethodHandleUtil.DynamicMethodBuilder("DirectMethodHandle." + m.getName(), type, typeof(IKVM.Runtime.InvokeCache<>).MakeGenericType(targetDelegateType));
			dm.Ldarg(0);
			if (invokeExact)
			{
				dm.Call(ByteCodeHelperMethods.GetDelegateForInvokeExact.MakeGenericMethod(targetDelegateType));
			}
			else
			{
				dm.LoadValueAddress();
				dm.Call(ByteCodeHelperMethods.GetDelegateForInvoke.MakeGenericMethod(targetDelegateType));
				dm.Ldarg(0);
			}
			for (int i = 1, count = type.parameterCount(); i < count; i++)
			{
				dm.Ldarg(i);
			}
			dm.CallDelegate(targetDelegateType);
			dm.Ret();
			return dm.CreateDelegate();
		}
		else
		{
			TypeWrapper tw = (TypeWrapper)typeof(MemberName).GetField("vmtarget", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(m);
			tw.Finish();
			MethodWrapper mw = tw.GetMethods()[index];
			if (mw.IsDynamicOnly)
			{
				MethodHandleUtil.DynamicMethodBuilder dm = new MethodHandleUtil.DynamicMethodBuilder("CustomInvoke:" + mw.Name, type, (ICustomInvoke)mw);
				if (mw.IsStatic)
				{
					dm.LoadNull();
					dm.BoxArgs(0);
				}
				else
				{
					dm.Ldarg(0);
					dm.BoxArgs(1);
				}
				dm.Callvirt(typeof(ICustomInvoke).GetMethod("Invoke"));
				dm.Convert(typeof(object), type.returnType(), 0);
				dm.Ret();
				return dm.CreateDelegate();
			}
			// HACK this code is duplicated in compiler.cs
			if (mw.IsProtected && (mw.DeclaringType == CoreClasses.java.lang.Object.Wrapper || mw.DeclaringType == CoreClasses.java.lang.Throwable.Wrapper))
			{
				TypeWrapper thisType = TypeWrapper.FromClass(lookupClass);
				TypeWrapper cli_System_Object = ClassLoaderWrapper.LoadClassCritical("cli.System.Object");
				TypeWrapper cli_System_Exception = ClassLoaderWrapper.LoadClassCritical("cli.System.Exception");
				// HACK we may need to redirect finalize or clone from java.lang.Object/Throwable
				// to a more specific base type.
				if (thisType.IsAssignableTo(cli_System_Object))
				{
					mw = cli_System_Object.GetMethodWrapper(mw.Name, mw.Signature, true);
				}
				else if (thisType.IsAssignableTo(cli_System_Exception))
				{
					mw = cli_System_Exception.GetMethodWrapper(mw.Name, mw.Signature, true);
				}
				else if (thisType.IsAssignableTo(CoreClasses.java.lang.Throwable.Wrapper))
				{
					mw = CoreClasses.java.lang.Throwable.Wrapper.GetMethodWrapper(mw.Name, mw.Signature, true);
				}
			}
			mw.ResolveMethod();
			MethodInfo mi = mw.GetMethod() as MethodInfo;
			if (mi != null
				&& !tw.IsRemapped
				&& !tw.IsGhost
				&& !tw.IsNonPrimitiveValueType
				&& type.parameterCount() <= MethodHandleUtil.MaxArity
				// FXBUG we should be able to use a normal (unbound) delegate for virtual methods
				// (when doDispatch is set), but the x64 CLR crashes when doing a virtual method dispatch on
				// a null reference
				&& (!mi.IsVirtual || (doDispatch && IntPtr.Size == 4))
				&& (doDispatch || !mi.IsVirtual))
			{
				return Delegate.CreateDelegate(MethodHandleUtil.CreateDelegateType(tw, mw), mi);
			}
			else
			{
				// slow path where we emit a DynamicMethod
				MethodHandleUtil.DynamicMethodBuilder dm = new MethodHandleUtil.DynamicMethodBuilder(mw.DeclaringType.TypeAsBaseType, "DirectMethodHandle:" + mw.Name, type);
				for (int i = 0, count = type.parameterCount(); i < count; i++)
				{
					if (i == 0 && !mw.IsStatic && (tw.IsGhost || tw.IsNonPrimitiveValueType))
					{
						dm.LoadFirstArgAddress();
					}
					else
					{
						dm.Ldarg(i);
					}
				}
				if (doDispatch && !mw.IsStatic)
				{
					dm.Callvirt(mw);
				}
				else
				{
					dm.Call(mw);
				}
				dm.Ret();
				return dm.CreateDelegate();
			}
		}
#endif
	}
}

static class Java_java_lang_invoke_MethodHandle
{
	private static IKVM.Runtime.InvokeCache<IKVM.Runtime.MH<MethodHandle, object[], object>> cache;

	public static object invokeExact(MethodHandle mh, object[] args)
	{
#if FIRST_PASS
		return null;
#else
		return IKVM.Runtime.ByteCodeHelper.GetDelegateForInvokeExact<IKVM.Runtime.MH<object[], object>>(mh)(args);
#endif
	}

	public static object invoke(MethodHandle mh, object[] args)
	{
#if FIRST_PASS
		return null;
#else
		MethodType type = mh.type();
		if (mh.isVarargsCollector())
		{
			java.lang.Class varargType = type.parameterType(type.parameterCount() - 1);
			if (type.parameterCount() == args.Length)
			{
				if (!varargType.isInstance(args[args.Length - 1]))
				{
					Array arr = (Array)java.lang.reflect.Array.newInstance(varargType.getComponentType(), 1);
					arr.SetValue(args[args.Length - 1], 0);
					args[args.Length - 1] = arr;
				}
			}
			else if (type.parameterCount() - 1 > args.Length)
			{
				throw new WrongMethodTypeException();
			}
			else
			{
				object[] newArgs = new object[type.parameterCount()];
				Array.Copy(args, newArgs, newArgs.Length - 1);
				Array varargs = (Array)java.lang.reflect.Array.newInstance(varargType.getComponentType(), args.Length - (newArgs.Length - 1));
				Array.Copy(args, newArgs.Length - 1, varargs, 0, varargs.Length);
				newArgs[newArgs.Length - 1] = varargs;
				args = newArgs;
			}
		}
		if (mh.type().parameterCount() != args.Length)
		{
			throw new WrongMethodTypeException();
		}
		mh = mh.asSpreader(typeof(object[]), args.Length);
		return IKVM.Runtime.ByteCodeHelper.GetDelegateForInvoke<IKVM.Runtime.MH<MethodHandle, object[], object>>(mh, ref cache)(mh, args);
#endif
	}
}

static partial class MethodHandleUtil
{
	internal static Type CreateDelegateType(MethodType type)
	{
#if FIRST_PASS
		return null;
#else
		TypeWrapper[] args = new TypeWrapper[type.parameterCount()];
		for (int i = 0; i < args.Length; i++)
		{
			args[i] = TypeWrapper.FromClass(type.parameterType(i));
			args[i].Finish();
		}
		TypeWrapper ret = TypeWrapper.FromClass(type.returnType());
		ret.Finish();
		return CreateDelegateType(args, ret);
#endif
	}

#if !FIRST_PASS
	private static Type[] GetParameterTypes(MethodBase mb)
	{
		ParameterInfo[] pi = mb.GetParameters();
		Type[] args = new Type[pi.Length];
		for (int i = 0; i < args.Length; i++)
		{
			args[i] = pi[i].ParameterType;
		}
		return args;
	}

	private static Type[] GetParameterTypes(Type thisType, MethodBase mb)
	{
		ParameterInfo[] pi = mb.GetParameters();
		Type[] args = new Type[pi.Length + 1];
		args[0] = thisType;
		for (int i = 1; i < args.Length; i++)
		{
			args[i] = pi[i - 1].ParameterType;
		}
		return args;
	}

	internal static MethodType GetDelegateMethodType(Type type)
	{
		java.lang.Class[] types;
		MethodInfo mi = GetDelegateInvokeMethod(type);
		ParameterInfo[] pi = mi.GetParameters();
		if (pi.Length > 0 && IsPackedArgsContainer(pi[pi.Length - 1].ParameterType))
		{
			System.Collections.Generic.List<java.lang.Class> list = new System.Collections.Generic.List<java.lang.Class>();
			for (int i = 0; i < pi.Length - 1; i++)
			{
				list.Add(ClassLoaderWrapper.GetWrapperFromType(pi[i].ParameterType).ClassObject);
			}
			Type[] args = pi[pi.Length - 1].ParameterType.GetGenericArguments();
			while (IsPackedArgsContainer(args[args.Length - 1]))
			{
				for (int i = 0; i < args.Length - 1; i++)
				{
					list.Add(ClassLoaderWrapper.GetWrapperFromType(args[i]).ClassObject);
				}
				args = args[args.Length - 1].GetGenericArguments();
			}
			for (int i = 0; i < args.Length; i++)
			{
				list.Add(ClassLoaderWrapper.GetWrapperFromType(args[i]).ClassObject);
			}
			types = list.ToArray();
		}
		else
		{
			types = new java.lang.Class[pi.Length];
			for (int i = 0; i < types.Length; i++)
			{
				types[i] = ClassLoaderWrapper.GetWrapperFromType(pi[i].ParameterType).ClassObject;
			}
		}
		return MethodType.methodType(ClassLoaderWrapper.GetWrapperFromType(mi.ReturnType).ClassObject, types);
	}

	internal sealed class DynamicMethodBuilder
	{
		private readonly MethodType type;
		private readonly int firstArg;
		private readonly Type delegateType;
		private readonly object target;
		private readonly object value;
		private readonly Type container;
		private readonly DynamicMethod dm;
		private readonly CodeEmitter ilgen;
		private readonly Type packedArgType;
		private readonly int packedArgPos;

		sealed class Container<T1, T2>
		{
			public T1 target;
			public T2 value;

			public Container(T1 target, T2 value)
			{
				this.target = target;
				this.value = value;
			}
		}

		private DynamicMethodBuilder(string name, MethodType type, Type container, object target, object value, Type owner)
		{
			this.type = type;
			this.delegateType = CreateDelegateType(type);
			this.target = target;
			this.value = value;
			this.container = container;
			MethodInfo mi = GetDelegateInvokeMethod(delegateType);
			Type[] paramTypes;
			if (container != null)
			{
				this.firstArg = 1;
				paramTypes = GetParameterTypes(container, mi);
			}
			else if (target != null)
			{
				this.firstArg = 1;
				paramTypes = GetParameterTypes(target.GetType(), mi);
			}
			else
			{
				paramTypes = GetParameterTypes(mi);
			}
			if (!ReflectUtil.CanOwnDynamicMethod(owner))
			{
				owner = typeof(DynamicMethodBuilder);
			}
			this.dm = new DynamicMethod(name, mi.ReturnType, paramTypes, owner, true);
			this.ilgen = CodeEmitter.Create(dm);

			if (type.parameterCount() > MaxArity)
			{
				ParameterInfo[] pi = mi.GetParameters();
				this.packedArgType = pi[pi.Length - 1].ParameterType;
				this.packedArgPos = pi.Length - 1 + firstArg;
			}
			else
			{
				this.packedArgPos = Int32.MaxValue;
			}
		}

		internal DynamicMethodBuilder(Type owner, string name, MethodType type)
			: this(name, type, null, null, null, owner)
		{
		}

		internal DynamicMethodBuilder(string name, MethodType type, MethodHandle target)
			: this(name, type, null, target.vmtarget, null, null)
		{
			ilgen.Emit(OpCodes.Ldarg_0);
		}

		internal DynamicMethodBuilder(string name, MethodType type, ICustomInvoke target)
			: this(name, type, null, target, null, null)
		{
			ilgen.Emit(OpCodes.Ldarg_0);
		}

		internal DynamicMethodBuilder(string name, MethodType type, MethodHandle target, object value)
			: this(name, type, typeof(Container<,>).MakeGenericType(target.vmtarget.GetType(), value.GetType()), target.vmtarget, value, null)
		{
			ilgen.Emit(OpCodes.Ldarg_0);
			ilgen.Emit(OpCodes.Ldfld, container.GetField("target"));
		}

		internal DynamicMethodBuilder(string name, MethodType type, Type valueType)
			: this(name, type, typeof(Container<,>).MakeGenericType(typeof(object), valueType), null, null, null)
		{
		}

		internal void Call(MethodInfo method)
		{
			ilgen.Emit(OpCodes.Call, method);
		}

		internal void Callvirt(MethodInfo method)
		{
			ilgen.Emit(OpCodes.Callvirt, method);
		}

		internal void Call(MethodWrapper mw)
		{
			mw.EmitCall(ilgen);
		}

		internal void Callvirt(MethodWrapper mw)
		{
			mw.EmitCallvirt(ilgen);
		}

		internal void CallDelegate(Type delegateType)
		{
			EmitCallDelegateInvokeMethod(ilgen, delegateType);
		}

		internal void LoadFirstArgAddress()
		{
			ilgen.EmitLdarga(firstArg);
		}

		internal void Ldarg(int i)
		{
			i += firstArg;
			if (i >= packedArgPos)
			{
				ilgen.EmitLdarga(packedArgPos);
				int fieldPos = i - packedArgPos;
				Type type = packedArgType;
				while (fieldPos >= MaxArity || (fieldPos == MaxArity - 1 && IsPackedArgsContainer(type.GetField("t8").FieldType)))
				{
					FieldInfo field = type.GetField("t8");
					type = field.FieldType;
					ilgen.Emit(OpCodes.Ldflda, field);
					fieldPos -= MaxArity - 1;
				}
				ilgen.Emit(OpCodes.Ldfld, type.GetField("t" + (1 + fieldPos)));
			}
			else
			{
				ilgen.EmitLdarg(i);
			}
		}

		internal void LoadArrayElement(int index, TypeWrapper tw)
		{
			ilgen.EmitLdc_I4(index);
			if (tw.IsNonPrimitiveValueType)
			{
				ilgen.Emit(OpCodes.Ldelema, tw.TypeAsArrayType);
				ilgen.Emit(OpCodes.Ldobj, tw.TypeAsArrayType);
			}
			else if (tw == PrimitiveTypeWrapper.BYTE
				|| tw == PrimitiveTypeWrapper.BOOLEAN)
			{
				ilgen.Emit(OpCodes.Ldelem_I1);
			}
			else if (tw == PrimitiveTypeWrapper.SHORT)
			{
				ilgen.Emit(OpCodes.Ldelem_I2);
			}
			else if (tw == PrimitiveTypeWrapper.CHAR)
			{
				ilgen.Emit(OpCodes.Ldelem_U2);
			}
			else if (tw == PrimitiveTypeWrapper.INT)
			{
				ilgen.Emit(OpCodes.Ldelem_I4);
			}
			else if (tw == PrimitiveTypeWrapper.LONG)
			{
				ilgen.Emit(OpCodes.Ldelem_I8);
			}
			else if (tw == PrimitiveTypeWrapper.FLOAT)
			{
				ilgen.Emit(OpCodes.Ldelem_R4);
			}
			else if (tw == PrimitiveTypeWrapper.DOUBLE)
			{
				ilgen.Emit(OpCodes.Ldelem_R8);
			}
			else
			{
				ilgen.Emit(OpCodes.Ldelem_Ref);
				if (tw.IsGhost)
				{
					tw.EmitConvStackTypeToSignatureType(ilgen, null);
				}
			}
		}

		internal void Convert(java.lang.Class srcType, java.lang.Class dstType, int level)
		{
			EmitConvert(ilgen, srcType, dstType, level);
		}

		internal void CallTarget()
		{
			EmitCallDelegateInvokeMethod(ilgen, target.GetType());
		}

		internal void LoadValueAddress()
		{
			ilgen.Emit(OpCodes.Ldarg_0);
			ilgen.Emit(OpCodes.Ldflda, container.GetField("value"));
		}

		internal void LoadValue()
		{
			ilgen.Emit(OpCodes.Ldarg_0);
			ilgen.Emit(OpCodes.Ldfld, container.GetField("value"));
		}

		internal void CallValue()
		{
			EmitCallDelegateInvokeMethod(ilgen, value.GetType());
		}

		internal void Ret()
		{
			ilgen.Emit(OpCodes.Ret);
		}

		internal Delegate CreateDelegate()
		{
			ilgen.DoEmit();
			return ValidateDelegate(firstArg == 0
				? dm.CreateDelegate(delegateType)
				: dm.CreateDelegate(delegateType, container == null ? target : Activator.CreateInstance(container, target, value)));
		}

		internal AdapterMethodHandle CreateAdapter()
		{
			return new AdapterMethodHandle(type, CreateDelegate());
		}

		internal void BoxArgs(int start)
		{
			int paramCount = type.parameterCount();
			ilgen.EmitLdc_I4(paramCount - start);
			ilgen.Emit(OpCodes.Newarr, Types.Object);
			for (int i = start; i < paramCount; i++)
			{
				ilgen.Emit(OpCodes.Dup);
				ilgen.EmitLdc_I4(i - start);
				Ldarg(i);
				Convert(type.parameterType(i), typeof(object), 0);
				ilgen.Emit(OpCodes.Stelem_Ref);
			}
		}

		internal void LoadNull()
		{
			ilgen.Emit(OpCodes.Ldnull);
		}
	}

#if DEBUG
	[System.Security.SecuritySafeCritical]
#endif
	private static Delegate ValidateDelegate(Delegate d)
	{
#if DEBUG
		try
		{
			System.Runtime.CompilerServices.RuntimeHelpers.PrepareDelegate(d);
		}
		catch (Exception x)
		{
			JVM.CriticalFailure("Delegate failed to JIT", x);
		}
#endif
		return d;
	}

	private struct BoxUtil
	{
		private static readonly BoxUtil[] boxers = new BoxUtil[] {
			BoxUtil.Create<java.lang.Boolean, bool>(java.lang.Boolean.TYPE, "boolean", "Boolean"),
			BoxUtil.Create<java.lang.Byte, byte>(java.lang.Byte.TYPE, "byte", "Byte"),
			BoxUtil.Create<java.lang.Character, char>(java.lang.Character.TYPE, "char", "Character"),
			BoxUtil.Create<java.lang.Short, short>(java.lang.Short.TYPE, "short", "Short"),
			BoxUtil.Create<java.lang.Integer, int>(java.lang.Integer.TYPE, "int", "Integer"),
			BoxUtil.Create<java.lang.Long, int>(java.lang.Long.TYPE, "long", "Long"),
			BoxUtil.Create<java.lang.Float, float>(java.lang.Float.TYPE, "float", "Float"),
			BoxUtil.Create<java.lang.Double, double>(java.lang.Double.TYPE, "double", "Double"),
		};
		private readonly jlClass clazz;
		private readonly jlClass type;
		private readonly MethodInfo box;
		private readonly MethodInfo unbox;
		private readonly MethodInfo unboxObject;

		private BoxUtil(jlClass clazz, jlClass type, MethodInfo box, MethodInfo unbox, MethodInfo unboxObject)
		{
			this.clazz = clazz;
			this.type = type;
			this.box = box;
			this.unbox = unbox;
			this.unboxObject = unboxObject;
		}

		private static BoxUtil Create<T, P>(jlClass type, string name, string longName)
		{
			return new BoxUtil(ikvm.@internal.ClassLiteral<T>.Value, type,
				typeof(T).GetMethod("valueOf", new Type[] { typeof(P) }),
				typeof(T).GetMethod(name + "Value", Type.EmptyTypes),
				typeof(sun.invoke.util.ValueConversions).GetMethod("unbox" + longName, BindingFlags.Static | BindingFlags.NonPublic));
		}

		internal static void Box(CodeEmitter ilgen, jlClass srcClass, jlClass dstClass, int level)
		{
			for (int i = 0; i < boxers.Length; i++)
			{
				if (boxers[i].type == srcClass)
				{
					ilgen.Emit(OpCodes.Call, boxers[i].box);
					EmitConvert(ilgen, boxers[i].clazz, dstClass, level);
					return;
				}
			}
			throw new InvalidOperationException();
		}

		internal static void Unbox(CodeEmitter ilgen, jlClass srcClass, jlClass dstClass, int level)
		{
			for (int i = 0; i < boxers.Length; i++)
			{
				if (boxers[i].clazz == srcClass)
				{
					// typed unboxing
					ilgen.Emit(OpCodes.Call, boxers[i].unbox);
					EmitConvert(ilgen, boxers[i].type, dstClass, level);
					return;
				}
			}
			for (int i = 0; i < boxers.Length; i++)
			{
				if (boxers[i].type == dstClass)
				{
					// untyped unboxing
					ilgen.EmitLdc_I4(level > 1 ? 1 : 0);
					ilgen.Emit(OpCodes.Call, boxers[i].unboxObject);
					return;
				}
			}
			throw new InvalidOperationException();
		}
	}

	private static void EmitConvert(CodeEmitter ilgen, java.lang.Class srcClass, java.lang.Class dstClass, int level)
	{
		if (srcClass != dstClass)
		{
			TypeWrapper src = TypeWrapper.FromClass(srcClass);
			TypeWrapper dst = TypeWrapper.FromClass(dstClass);
			src.Finish();
			dst.Finish();
			if (src.IsNonPrimitiveValueType)
			{
				src.EmitBox(ilgen);
			}
			if (dst == PrimitiveTypeWrapper.VOID)
			{
				ilgen.Emit(OpCodes.Pop);
			}
			else if (src.IsPrimitive)
			{
				if (dst.IsPrimitive)
				{
					if (src == PrimitiveTypeWrapper.BYTE)
					{
						ilgen.Emit(OpCodes.Conv_I1);
					}
					if (dst == PrimitiveTypeWrapper.FLOAT)
					{
						ilgen.Emit(OpCodes.Conv_R4);
					}
					else if (dst == PrimitiveTypeWrapper.DOUBLE)
					{
						ilgen.Emit(OpCodes.Conv_R8);
					}
					else if (dst == PrimitiveTypeWrapper.LONG)
					{
						if (src == PrimitiveTypeWrapper.FLOAT)
						{
							ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.f2l);
						}
						else if (src == PrimitiveTypeWrapper.DOUBLE)
						{
							ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.d2l);
						}
						else
						{
							ilgen.Emit(OpCodes.Conv_I8);
						}
					}
					else if (dst == PrimitiveTypeWrapper.BOOLEAN)
					{
						if (src == PrimitiveTypeWrapper.FLOAT)
						{
							ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.f2i);
						}
						else if (src == PrimitiveTypeWrapper.DOUBLE)
						{
							ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.d2i);
						}
						else
						{
							ilgen.Emit(OpCodes.Conv_I4);
						}
						ilgen.Emit(OpCodes.Ldc_I4_1);
						ilgen.Emit(OpCodes.And);
					}
					else if (src == PrimitiveTypeWrapper.LONG)
					{
						ilgen.Emit(OpCodes.Conv_I4);
					}
					else if (src == PrimitiveTypeWrapper.FLOAT)
					{
						ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.f2i);
					}
					else if (src == PrimitiveTypeWrapper.DOUBLE)
					{
						ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.d2i);
					}
				}
				else
				{
					BoxUtil.Box(ilgen, srcClass, dstClass, level);
				}
			}
			else if (src.IsGhost)
			{
				src.EmitConvSignatureTypeToStackType(ilgen);
				EmitConvert(ilgen, ikvm.@internal.ClassLiteral<java.lang.Object>.Value, dstClass, level);
			}
			else if (srcClass == ikvm.@internal.ClassLiteral<sun.invoke.empty.Empty>.Value)
			{
				ilgen.Emit(OpCodes.Pop);
				ilgen.Emit(OpCodes.Ldloc, ilgen.DeclareLocal(dst.TypeAsSignatureType));
			}
			else if (dst.IsPrimitive)
			{
				BoxUtil.Unbox(ilgen, srcClass, dstClass, level);
			}
			else if (dst.IsGhost)
			{
				dst.EmitConvStackTypeToSignatureType(ilgen, null);
			}
			else if (dst.IsNonPrimitiveValueType)
			{
				dst.EmitUnbox(ilgen);
			}
			else
			{
				dst.EmitCheckcast(ilgen);
			}
		}
	}

	internal static void Dump(MethodHandle mh)
	{
		Console.WriteLine("----");
		Dump((Delegate)mh.vmtarget, 0);
	}

	private static void WriteNest(int nest)
	{
		for (int i = 0; i < nest; i++)
		{
			Console.Write("  ");
		}
	}

	private static void Dump(Delegate d, int nest)
	{
		if (nest > 0)
		{
			WriteNest(nest - 1);
			Console.Write("->");
		}
		Console.Write(d.Method.Name + "(");
		string sep = "";
		foreach (ParameterInfo pi in d.Method.GetParameters())
		{
			Console.WriteLine(sep);
			WriteNest(nest);
			Console.Write("  {0}", TypeToString(pi.ParameterType));
			sep = ",";
		}
		Console.WriteLine(")");
		WriteNest(nest);
		Console.WriteLine("  : {0}", TypeToString(d.Method.ReturnType));
		if (d.Target is Delegate)
		{
			Dump((Delegate)d.Target, nest == 0 ? 1 : nest);
		}
		else if (d.Target != null)
		{
			FieldInfo field = d.Target.GetType().GetField("value");
			if (field != null && field.GetValue(d.Target) is Delegate)
			{
				WriteNest(nest + 1);
				Console.WriteLine("Collector:");
				Dump((Delegate)field.GetValue(d.Target), nest + 2);
			}
			field = d.Target.GetType().GetField("target");
			if (field != null && field.GetValue(d.Target) != null)
			{
				Dump((Delegate)field.GetValue(d.Target), nest == 0 ? 1 : nest);
			}
		}
	}

	private static string TypeToString(Type type)
	{
		if (type.IsGenericType
			&& type.Namespace == "IKVM.Runtime"
			&& (type.Name.StartsWith("MH`") || type.Name.StartsWith("MHV`")))
		{
			return type.Name.Substring(0, type.Name.IndexOf('`')) + "<" + TypesToString(type.GetGenericArguments()) + ">";
		}
		else if (type.DeclaringType == typeof(DynamicMethodBuilder))
		{
			return "C<" + TypesToString(type.GetGenericArguments()) + ">";
		}
		else if (ReflectUtil.IsVector(type))
		{
			return TypeToString(type.GetElementType()) + "[]";
		}
		else if (type == typeof(object))
		{
			return "object";
		}
		else if (type == typeof(string))
		{
			return "string";
		}
		else if (type.IsPrimitive)
		{
			return type.Name.ToLowerInvariant();
		}
		else
		{
			return type.ToString();
		}
	}

	private static string TypesToString(Type[] types)
	{
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		foreach (Type type in types)
		{
			if (sb.Length != 0)
			{
				sb.Append(", ");
			}
			sb.Append(TypeToString(type));
		}
		return sb.ToString();
	}
#endif
}

static class Java_java_lang_invoke_AdapterMethodHandle
{
	public static MethodHandle makePairwiseConvert(MethodType newType, MethodHandle target, int level)
	{
#if FIRST_PASS
		return null;
#else
		MethodType oldType = target.type();
		MethodHandleUtil.DynamicMethodBuilder dm = new MethodHandleUtil.DynamicMethodBuilder("AdapterMethodHandle.pairwiseConvert", newType, target);
		for (int i = 0, count = newType.parameterCount(); i < count; i++)
		{
			dm.Ldarg(i);
			dm.Convert(newType.parameterType(i), oldType.parameterType(i), level);
		}
		dm.CallTarget();
		dm.Convert(oldType.returnType(), newType.returnType(), level);
		dm.Ret();
		return dm.CreateAdapter();
#endif
	}

	public static MethodHandle makeRetype(MethodType newType, MethodHandle target, bool raw)
	{
#if FIRST_PASS
		return null;
#else
		MethodType oldType = target.type();
		if (oldType == newType)
		{
			return target;
		}
		if (!AdapterMethodHandle.canRetype(newType, oldType, raw))
		{
			return null;
		}
		// TODO does raw translate into a level?
		return makePairwiseConvert(newType, target, 0);
#endif
	}

	public static MethodHandle makeSpreadArguments(MethodType newType, MethodHandle target, java.lang.Class spreadArgType, int spreadArgPos, int spreadArgCount)
	{
#if FIRST_PASS
		return null;
#else
		TypeWrapper twComponent = TypeWrapper.FromClass(spreadArgType).ElementTypeWrapper;
		MethodHandleUtil.DynamicMethodBuilder dm = new MethodHandleUtil.DynamicMethodBuilder("AdapterMethodHandle.spreadArguments", newType, target);
		for (int i = 0, count = newType.parameterCount(); i < count; i++)
		{
			if (i == spreadArgPos)
			{
				for (int j = 0; j < spreadArgCount; j++)
				{
					dm.Ldarg(i);
					dm.LoadArrayElement(j, twComponent);
					dm.Convert(twComponent.ClassObject, target.type().parameterType(i + j), 0);
				}
			}
			else
			{
				dm.Ldarg(i);
			}
		}
		dm.CallTarget();
		dm.Ret();
		return dm.CreateAdapter();
#endif
	}

	public static MethodHandle makeCollectArguments(MethodHandle target, MethodHandle collector, int collectArgPos, bool retainOriginalArgs)
	{
#if FIRST_PASS
		return null;
#else
		MethodType targetType = target.type();
		MethodType collectorType = collector.type();
		bool isfilter = collectorType.returnType() == java.lang.Void.TYPE;
		MethodType newType = targetType.dropParameterTypes(collectArgPos, collectArgPos + (isfilter ? 0 : 1));
		if (!retainOriginalArgs)
		{
			newType = newType.insertParameterTypes(collectArgPos, collectorType.parameterList());
		}
		MethodHandleUtil.DynamicMethodBuilder dm = new MethodHandleUtil.DynamicMethodBuilder("AdapterMethodHandle.collectArguments", newType, target, collector.vmtarget);
		for (int i = 0, count = newType.parameterCount(); i < count || i == collectArgPos; i++)
		{
			if (i == collectArgPos)
			{
				dm.LoadValue();
				for (int j = 0; j < collectorType.parameterCount(); j++)
				{
					dm.Ldarg(i + j);
				}
				dm.CallValue();

				collectArgPos = -1;
				i--;
				if (!retainOriginalArgs)
				{
					i += collectorType.parameterCount();
				}
			}
			else
			{
				dm.Ldarg(i);
			}
		}
		dm.CallTarget();
		dm.Ret();
		return dm.CreateAdapter();
#endif
	}
}

static class Java_java_lang_invoke_MethodHandleImpl
{
	public static MethodHandle permuteArguments(MethodHandle target, MethodType newType, MethodType oldType, int[] permutationOrNull)
	{
#if FIRST_PASS
		return null;
#else
		// LAME why does OpenJDK name the parameter permutationOrNull while it is not allowed to be null?
		if (permutationOrNull.Length != oldType.parameterCount())
		{
			throw new java.lang.IllegalArgumentException("wrong number of arguments in permutation");
		}
		MethodHandleUtil.DynamicMethodBuilder dm = new MethodHandleUtil.DynamicMethodBuilder("MethodHandleImpl.permuteArguments", newType, target);
		for (int i = 0, argCount = newType.parameterCount(); i < permutationOrNull.Length; i++)
		{
			// make sure to only read each array element once, to avoid having to make a defensive copy of the array
			int perm = permutationOrNull[i];
			if (perm < 0 || perm >= argCount)
			{
				throw new java.lang.IllegalArgumentException("permutation argument out of range");
			}
			dm.Ldarg(perm);
			dm.Convert(oldType.parameterType(i), newType.parameterType(perm), 0);
		}
		dm.CallTarget();
		dm.Convert(oldType.returnType(), newType.returnType(), 0);
		dm.Ret();
		return dm.CreateAdapter();
#endif
	}
}

static class Java_java_lang_invoke_MethodHandleNatives
{
	public static void init(MemberName self, object r)
	{
#if !FIRST_PASS
		if (r is java.lang.reflect.Method || r is java.lang.reflect.Constructor)
		{
			MethodWrapper mw = MethodWrapper.FromMethodOrConstructor(r);
			int index = Array.IndexOf(mw.DeclaringType.GetMethods(), mw);
			if (index != -1)
			{
				// TODO self.setVMIndex(index);
				typeof(MemberName).GetField("vmindex", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(self, index);
				typeof(MemberName).GetField("vmtarget", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(self, mw.DeclaringType);
				int flags = (int)mw.Modifiers;
				if (r is java.lang.reflect.Method)
				{
					flags |= MemberName.IS_METHOD;
				}
				else
				{
					flags |= MemberName.IS_CONSTRUCTOR;
				}
				typeof(MemberName).GetField("flags", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(self, flags);
			}
		}
		else if (r is java.lang.reflect.Field)
		{
			FieldWrapper fw = FieldWrapper.FromField(r);
			int index = Array.IndexOf(fw.DeclaringType.GetFields(), fw);
			if (index != -1)
			{
				// TODO self.setVMIndex(index);
				typeof(MemberName).GetField("vmindex", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(self, index);
				typeof(MemberName).GetField("flags", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(self, (int)fw.Modifiers | MemberName.IS_FIELD);
			}
		}
		else
		{
			throw new InvalidOperationException();
		}
#endif
	}

	public static void expand(MemberName self)
	{
		throw new NotImplementedException();
	}

	public static void resolve(MemberName self, jlClass caller)
	{
#if !FIRST_PASS
		if (self.isMethod() || self.isConstructor())
		{
			TypeWrapper tw = TypeWrapper.FromClass(self.getDeclaringClass());
			if (tw == CoreClasses.java.lang.invoke.MethodHandle.Wrapper
				&& (self.getName() == "invoke" || self.getName() == "invokeExact"))
			{
				typeof(MemberName).GetField("vmindex", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(self, Int32.MaxValue);
				return;
			}
			MethodWrapper mw = tw.GetMethodWrapper(self.getName(), self.getSignature().Replace('/', '.'), true);
			if (mw != null)
			{
				tw = mw.DeclaringType;
				int index = Array.IndexOf(tw.GetMethods(), mw);
				if (index != -1)
				{
					// TODO self.setVMIndex(index);
					typeof(MemberName).GetField("vmindex", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(self, index);
					typeof(MemberName).GetField("vmtarget", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(self, tw);
					int flags = (int)mw.Modifiers;
					if (self.isMethod())
					{
						flags |= MemberName.IS_METHOD;
					}
					else
					{
						flags |= MemberName.IS_CONSTRUCTOR;
					}
					typeof(MemberName).GetField("flags", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(self, flags);
				}
			}
		}
		else if (self.isField())
		{
			TypeWrapper tw = TypeWrapper.FromClass(self.getDeclaringClass());
			// TODO should we look in base classes?
			FieldWrapper fw = tw.GetFieldWrapper(self.getName(), self.getSignature().Replace('/', '.'));
			if (fw != null)
			{
				int index = Array.IndexOf(fw.DeclaringType.GetFields(), fw);
				if (index != -1)
				{
					// TODO self.setVMIndex(index);
					typeof(MemberName).GetField("vmindex", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(self, index);
					typeof(MemberName).GetField("flags", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(self, (int)fw.Modifiers | MemberName.IS_FIELD);
				}
			}
		}
		else
		{
			throw new InvalidOperationException();
		}
#endif
	}

	public static int getMembers(jlClass defc, string matchName, string matchSig, int matchFlags, jlClass caller, int skip, object[] results)
	{
		return 1;
	}
}
