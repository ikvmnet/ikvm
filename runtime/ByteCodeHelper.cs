/*
  Copyright (C) 2002, 2003, 2004 Jeroen Frijters

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
using System.Diagnostics;

public class ByteCodeHelper
{
	[DebuggerStepThroughAttribute]
	public static object multianewarray(RuntimeTypeHandle typeHandle, int[] lengths)
	{
		for(int i = 0; i < lengths.Length; i++)
		{
			if(lengths[i] < 0)
			{
				throw JavaException.NegativeArraySizeException();
			}
		}
		return MultianewarrayHelper(Type.GetTypeFromHandle(typeHandle).GetElementType(), lengths, 0);
	}

	private static object MultianewarrayHelper(Type elemType, int[] lengths, int index)
	{
		object o = Array.CreateInstance(elemType, lengths[index++]);
		if(index < lengths.Length)
		{
			elemType = elemType.GetElementType();
			object[] a = (object[])o;
			for(int i = 0; i < a.Length; i++)
			{
				a[i] = MultianewarrayHelper(elemType, lengths, index);
			}
		}
		return o;
	}

	[DebuggerStepThroughAttribute]
	public static object DynamicMultianewarray(RuntimeTypeHandle type, string clazz, int[] lengths)
	{
		Profiler.Count("DynamicMultianewarray");
		TypeWrapper wrapper = LoadTypeWrapper(type, clazz);
		return multianewarray(wrapper.TypeAsArrayType.TypeHandle, lengths);
	}

	[DebuggerStepThroughAttribute]
	public static object DynamicNewarray(int length, RuntimeTypeHandle type, string clazz)
	{
		Profiler.Count("DynamicNewarray");
		if(length < 0)
		{
			throw JavaException.NegativeArraySizeException();
		}
		TypeWrapper wrapper = LoadTypeWrapper(type, clazz);
		return Array.CreateInstance(wrapper.TypeAsArrayType, length);
	}

	[DebuggerStepThroughAttribute]
	public static void DynamicAastore(object arrayref, int index, object val, RuntimeTypeHandle type, string clazz)
	{
		Profiler.Count("DynamicAastore");
		// TODO do we need to load the type here?
		((Array)arrayref).SetValue(val, index);
	}

	[DebuggerStepThroughAttribute]
	public static object DynamicAaload(object arrayref, int index, RuntimeTypeHandle type, string clazz)
	{
		Profiler.Count("DynamicAaload");
		// TODO do we need to load the type here?
		return ((Array)arrayref).GetValue(index);
	}

	private static FieldWrapper GetFieldWrapper(TypeWrapper thisType, RuntimeTypeHandle type, string clazz, string name, string sig, bool isStatic)
	{
		TypeWrapper caller = ClassLoaderWrapper.GetWrapperFromType(Type.GetTypeFromHandle(type));
		TypeWrapper wrapper = LoadTypeWrapper(type, clazz);
		FieldWrapper field = wrapper.GetFieldWrapper(name, caller.GetClassLoader().ExpressionTypeWrapper(sig));
		if(field == null)
		{
			throw JavaException.NoSuchFieldError(clazz + "." + name);
		}
		if(field.IsStatic != isStatic)
		{
			throw JavaException.IncompatibleClassChangeError(clazz + "." + name);
		}
		// NOTE this access check is duplicated in Compiler.GetPutField
		if(field.IsPublic ||
			(field.IsProtected && (isStatic ? caller.IsSubTypeOf(field.DeclaringType) : thisType.IsSubTypeOf(caller))) ||
			(field.IsPrivate && caller == field.DeclaringType) ||
			(!(field.IsPublic || field.IsPrivate) && caller.IsInSamePackageAs(field.DeclaringType)))
		{
			return field;
		}
		throw JavaException.IllegalAccessError(field.DeclaringType.Name + "." + name);
	}

	[DebuggerStepThroughAttribute]
	public static object DynamicGetfield(object obj, string name, string sig, RuntimeTypeHandle type, string clazz)
	{
		Profiler.Count("DynamicGetfield");
		return GetFieldWrapper(ClassLoaderWrapper.GetWrapperFromType(obj.GetType()), type, clazz, name, sig, false).GetValue(obj);
	}

	[DebuggerStepThroughAttribute]
	public static object DynamicGetstatic(string name, string sig, RuntimeTypeHandle type, string clazz)
	{
		Profiler.Count("DynamicGetstatic");
		return GetFieldWrapper(null, type, clazz, name, sig, true).GetValue(null);
	}

	[DebuggerStepThroughAttribute]
	public static void DynamicPutfield(object obj, object val, string name, string sig, RuntimeTypeHandle type, string clazz)
	{
		Profiler.Count("DynamicPutfield");
		GetFieldWrapper(ClassLoaderWrapper.GetWrapperFromType(obj.GetType()), type, clazz, name, sig, false).SetValue(obj, val);
	}

	[DebuggerStepThroughAttribute]
	public static void DynamicPutstatic(object val, string name, string sig, RuntimeTypeHandle type, string clazz)
	{
		Profiler.Count("DynamicPutstatic");
		GetFieldWrapper(null, type, clazz, name, sig, true).SetValue(null, val);
	}

	// the sole purpose of this method is to check whether the clazz can be instantiated (but not to actually do it)
	[DebuggerStepThroughAttribute]
	public static void DynamicNewCheckOnly(RuntimeTypeHandle type, string clazz)
	{
		Profiler.Count("DynamicNewCheckOnly");
		TypeWrapper wrapper = LoadTypeWrapper(type, clazz);
		if(wrapper.IsAbstract || wrapper.IsInterface)
		{
			throw JavaException.InstantiationError(clazz);
		}
	}

	private static TypeWrapper LoadTypeWrapper(RuntimeTypeHandle type, string clazz)
	{
		TypeWrapper context = ClassLoaderWrapper.GetWrapperFromType(Type.GetTypeFromHandle(type));
		TypeWrapper wrapper = context.GetClassLoader().LoadClassByDottedNameFast(clazz);
		if(wrapper == null)
		{
			throw JavaException.NoClassDefFoundError(clazz);
		}
		if(!wrapper.IsAccessibleFrom(context))
		{
			throw JavaException.IllegalAccessError("Try to access class " + wrapper.Name + " from class " + clazz);
		}
		// TODO is this really needed?
		wrapper.Finish();
		return wrapper;
	}

	[DebuggerStepThroughAttribute]
	public static object DynamicClassLiteral(RuntimeTypeHandle type, string clazz)
	{
		Profiler.Count("DynamicClassLiteral");
		return NativeCode.java.lang.VMClass.getClassFromWrapper(LoadTypeWrapper(type, clazz));
	}

	[DebuggerStepThroughAttribute]
	public static object DynamicCast(object obj, RuntimeTypeHandle type, string clazz)
	{
		Profiler.Count("DynamicCast");
		if(!DynamicInstanceOf(obj, type, clazz))
		{
			throw JavaException.ClassCastException(ClassLoaderWrapper.GetWrapperFromType(obj.GetType()).Name);
		}
		return obj;
	}

	[DebuggerStepThroughAttribute]
	public static bool DynamicInstanceOf(object obj, RuntimeTypeHandle type, string clazz)
	{
		Profiler.Count("DynamicInstanceOf");
		TypeWrapper wrapper = LoadTypeWrapper(type, clazz);
		TypeWrapper other = ClassLoaderWrapper.GetWrapperFromType(obj.GetType());
		return other.IsAssignableTo(wrapper);
	}

	[DebuggerStepThroughAttribute]
	public static object DynamicInvokeSpecialNew(RuntimeTypeHandle type, string clazz, string name, string sig, object[] args)
	{
		Profiler.Count("DynamicInvokeSpecialNew");
		TypeWrapper wrapper = LoadTypeWrapper(type, clazz);
		// TODO who checks that the arg types are loadable?
		// TODO check accessibility
		MethodWrapper mw = wrapper.GetMethodWrapper(MethodDescriptor.FromNameSig(wrapper.GetClassLoader(), name, sig), false);
		if(mw == null)
		{
			// TODO throw the appropriate exception
			throw new NotImplementedException("constructor missing");
		}
		return mw.Invoke(null, args, false);
	}

	[DebuggerStepThroughAttribute]
	public static object DynamicInvokestatic(RuntimeTypeHandle type, string clazz, string name, string sig, object[] args)
	{
		Profiler.Count("DynamicInvokestatic");
		TypeWrapper wrapper = LoadTypeWrapper(type, clazz);
		// TODO who checks that the arg types are loadable?
		// TODO check accessibility
		MethodWrapper mw = wrapper.GetMethodWrapper(MethodDescriptor.FromNameSig(wrapper.GetClassLoader(), name, sig), true);
		if(mw == null)
		{
			// TODO throw the appropriate exception
			throw new NotImplementedException("method missing");
		}
		return mw.Invoke(null, args, false);
	}

	[DebuggerStepThroughAttribute]
	public static object DynamicInvokevirtual(object obj, RuntimeTypeHandle type, string clazz, string name, string sig, object[] args)
	{
		Profiler.Count("DynamicInvokevirtual");
		TypeWrapper wrapper = LoadTypeWrapper(type, clazz);
		// TODO who checks that the arg types are loadable?
		// TODO check accessibility
		MethodWrapper mw = wrapper.GetMethodWrapper(MethodDescriptor.FromNameSig(wrapper.GetClassLoader(), name, sig), true);
		if(mw == null)
		{
			// TODO throw the appropriate exception
			throw new NotImplementedException("method missing");
		}
		return mw.Invoke(obj, args, false);
	}

	[DebuggerStepThroughAttribute]
	public static Type DynamicGetTypeAsExceptionType(RuntimeTypeHandle type, string clazz)
	{
		Profiler.Count("DynamicGetTypeAsExceptionType");
		TypeWrapper tw = LoadTypeWrapper(type, clazz);
		tw.Finish();
		return tw.TypeAsExceptionType;
	}

	[DebuggerStepThroughAttribute]
	public static int f2i(float f)
	{
		if(f <= int.MinValue)
		{
			return int.MinValue;
		}
		if(f >= int.MaxValue)
		{
			return int.MaxValue;
		}
		if(f != f)
		{
			return 0;
		}
		return (int)f;
	}

	[DebuggerStepThroughAttribute]
	public static long f2l(float f)
	{
		if(f <= long.MinValue)
		{
			return long.MinValue;
		}
		if(f >= long.MaxValue)
		{
			return long.MaxValue;
		}
		if(f != f)
		{
			return 0;
		}
		return (long)f;
	}

	[DebuggerStepThroughAttribute]
	public static int d2i(double d)
	{
		if(d <= int.MinValue)
		{
			return int.MinValue;
		}
		if(d >= int.MaxValue)
		{
			return int.MaxValue;
		}
		if(d != d)
		{
			return 0;
		}
		return (int)d;
	}

	[DebuggerStepThroughAttribute]
	public static long d2l(double d)
	{
		if(d <= long.MinValue)
		{
			return long.MinValue;
		}
		if(d >= long.MaxValue)
		{
			return long.MaxValue;
		}
		if(d != d)
		{
			return 0;
		}
		return (long)d;
	}
}
