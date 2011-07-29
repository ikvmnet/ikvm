/*
  Copyright (C) 2011 Jeroen Frijters

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

static class Java_java_lang_invoke_MethodHandle
{
	public static object invokeExact(object obj, object[] args)
	{
		// this can never be called, because the compiler special cases these methods
		// TODO check reflection code paths
		throw new InvalidOperationException();
	}

	public static object invoke(object obj, object[] args)
	{
		// this can never be called, because the compiler special cases these methods
		// TODO check reflection code paths
		throw new InvalidOperationException();
	}
}

static class Java_java_lang_invoke_MethodHandleNatives
{
	public static void init(MemberName self, object r)
	{
		throw new NotImplementedException();
	}

	public static void expand(MemberName self)
	{
		throw new NotImplementedException();
	}

	public static void resolve(MemberName self, jlClass caller)
	{
#if !FIRST_PASS
		if (self.isMethod())
		{
			TypeWrapper tw = TypeWrapper.FromClass(self.getDeclaringClass());
			MethodWrapper mw = tw.GetMethodWrapper(self.getName(), self.getSignature().Replace('/', '.'), false);
			if (mw != null)
			{
				int index = Array.IndexOf(tw.GetMethods(), mw);
				if (index != -1)
				{
					// TODO self.setVMIndex(index);
					self.GetType().GetField("vmindex", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(self, index);
				}
			}
		}
		else if (self.isConstructor())
		{
			throw new NotImplementedException();
		}
		else
		{
			throw new NotImplementedException();
		}
#endif
	}

	public static int getMembers(jlClass defc, string matchName, string matchSig, int matchFlags, jlClass caller, int skip, object[] results)
	{
		return 1;
	}

	public static void init(AdapterMethodHandle self, MethodHandle target, int argnum)
	{
		throw new NotImplementedException();
	}

	sealed class Bound<TValue, TDelegate>
	{
		public readonly TDelegate d;
		public readonly TValue v;

		public Bound(TDelegate d, TValue v)
		{
			this.d = d;
			this.v = v;
		}
	}

	public static void init(BoundMethodHandle self, object target, int argnum)
	{
#if !FIRST_PASS
		MethodHandle mh = target as MethodHandle;
		if (mh == null)
		{
			// TODO what does this mean?
			throw new NotImplementedException();
		}
		Delegate del = (Delegate)mh.vmtarget;
		object argument = self.GetType().GetField("argument", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(self);
		if (argnum == 0
			&& del.Target == null
			// TODO do we need more type checking on "argument" here?
			&& !del.Method.GetParameters()[0].ParameterType.IsValueType)
		{
			self.vmtarget = Delegate.CreateDelegate(CreateDelegateType(self.type()), argument, del.Method);
		}
		else
		{
			// slow path where we're generating a DynamicMethod
			if (del.Method.GetParameters()[argnum].ParameterType.IsPrimitive)
			{
				argument = JavaBoxToClrBox(argument);
			}
			Type container = typeof(Bound<,>).MakeGenericType(argument == null ? typeof(object) : argument.GetType(), del.GetType());
			Type outgoingDelegateType = CreateDelegateType(self.type());
			MethodInfo mi = outgoingDelegateType.GetMethod("Invoke");
			ParameterInfo[] pi = mi.GetParameters();
			Type[] args = new Type[pi.Length + 1];
			args[0] = container;
			for (int i = 1; i < args.Length; i++)
			{
				args[i] = pi[i - 1].ParameterType;
			}
			DynamicMethod dm = DynamicMethodUtils.Create("MethodHandle", container, true, mi.ReturnType, args);
			ILGenerator ilgen = dm.GetILGenerator();
			ilgen.Emit(OpCodes.Ldarg_0);
			ilgen.Emit(OpCodes.Ldfld, container.GetField("d"));
			for (int i = 0; i < args.Length; i++)
			{
				if (i == argnum)
				{
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Ldfld, container.GetField("v"));
				}
				if (i != 0)
				{
					ilgen.Emit(OpCodes.Ldarg, (short)i);
				}
			}
			ilgen.Emit(OpCodes.Callvirt, del.GetType().GetMethod("Invoke"));
			ilgen.Emit(OpCodes.Ret);
			self.vmtarget = dm.CreateDelegate(outgoingDelegateType, Activator.CreateInstance(container, del, argument));
		}
#endif
	}

#if !FIRST_PASS
	private static object JavaBoxToClrBox(object value)
	{
		// TODO don't we have this somewhere already?
		if (value is java.lang.Integer)
		{
			return ((java.lang.Integer)value).intValue();
		}
		else
		{
			throw new NotImplementedException();
		}
	}

	private static Type CreateDelegateType(MethodType type)
	{
		TypeWrapper[] args = new TypeWrapper[type.parameterCount()];
		for (int i = 0; i < args.Length; i++)
		{
			args[i] = TypeWrapper.FromClass(type.parameterType(i));
		}
		TypeWrapper ret = TypeWrapper.FromClass(type.returnType());
		return Compiler.CreateDelegateType(args, ret);
	}

	private static Type CreateDelegateType(TypeWrapper tw, MethodWrapper mw)
	{
		TypeWrapper[] args = mw.GetParameters();
		if (!mw.IsStatic)
		{
			Array.Resize(ref args, args.Length + 1);
			Array.Copy(args, 0, args, 1, args.Length - 1);
			args[0] = tw;
		}
		return Compiler.CreateDelegateType(args, mw.ReturnType);
	}
#endif

	public static void init(DirectMethodHandle self, object r, bool doDispatch, jlClass caller)
	{
#if !FIRST_PASS
		self.GetType().GetField("vmindex", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(self, 0);
		MemberName m = r as MemberName;
		if (m == null)
		{
			// TODO what does this mean?
			throw new NotImplementedException();
		}
		TypeWrapper tw = TypeWrapper.FromClass(m.getDeclaringClass());
		tw.Finish();
		MethodWrapper mw = tw.GetMethods()[m.getVMIndex()];
		mw.ResolveMethod();
		MethodInfo mi = mw.GetMethod() as MethodInfo;
		if (mi != null
			// FXBUG we should be able to use a normal (unbound) delegate for virtual methods
			// (when doDispatch is set), but the CLR crashes when doing a virtual method dispatch on
			// a null reference
			&& !mi.IsVirtual
			&& !doDispatch)
		{
			self.vmtarget = Delegate.CreateDelegate(CreateDelegateType(tw, mw), mi);
		}
		else
		{
			// slow path where we emit a DynamicMethod
			Type[] args = new Type[mw.GetParameters().Length + (mw.IsStatic ? 0 : 1)];
			int pos = 0;
			if (!mw.IsStatic)
			{
				args[pos++] = tw.TypeAsSignatureType;
			}
			foreach (TypeWrapper argType in mw.GetParameters())
			{
				args[pos++] = argType.TypeAsSignatureType;
			}
			DynamicMethod dm = DynamicMethodUtils.Create("MethodHandle", tw.TypeAsBaseType, true, mw.ReturnType.TypeAsLocalOrStackType, args);
			CodeEmitter ilgen = CodeEmitter.Create(dm);
			for (int i = 0; i < args.Length; i++)
			{
				ilgen.Emit(OpCodes.Ldarg, (short)i);
			}
			if (doDispatch)
			{
				mw.EmitCallvirt(ilgen);
			}
			else
			{
				// TODO do we need to support newobj here?
				mw.EmitCall(ilgen);
			}
			ilgen.Emit(OpCodes.Ret);
			ilgen.DoEmit();
			self.vmtarget = dm.CreateDelegate(CreateDelegateType(tw, mw));
		}
#endif
	}

	public static void init(MethodType self)
	{
	}

	public static void registerBootstrap(jlClass caller, MethodHandle bootstrapMethod)
	{
		throw new NotImplementedException();
	}

	public static object getBootstrap(jlClass caller)
	{
		throw new NotImplementedException();
	}

	public static void setCallSiteTarget(CallSite site, MethodHandle target)
	{
		throw new NotImplementedException();
	}

	public static object getTarget(MethodHandle self, int format)
	{
		throw new NotImplementedException();
	}

	public static int getConstant(int which)
	{
#if FIRST_PASS
		return 0;
#else
		switch (which)
		{
			case MethodHandleNatives.Constants.GC_JVM_PUSH_LIMIT:
				return 3;
			case MethodHandleNatives.Constants.GC_JVM_STACK_MOVE_UNIT:
				return 1;
			case MethodHandleNatives.Constants.GC_CONV_OP_IMPLEMENTED_MASK:
				return 0;
			case MethodHandleNatives.Constants.GC_OP_ROT_ARGS_DOWN_LIMIT_BIAS:
				return 0;
			default:
				throw new InvalidOperationException();
		}
#endif
	}

	public static int getNamedCon(int which, object[] name)
	{
		throw new NotImplementedException();
	}

	public static void registerNatives()
	{
	}
}
