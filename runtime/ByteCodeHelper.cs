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

namespace IKVM.Runtime
{
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
			// TODO check loader constraints
			if(field.IsStatic != isStatic)
			{
				throw JavaException.IncompatibleClassChangeError(clazz + "." + name);
			}
			if(field.IsAccessibleFrom(caller, thisType))
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
			FieldWrapper fw = GetFieldWrapper(ClassLoaderWrapper.GetWrapperFromType(obj.GetType()), type, clazz, name, sig, false);
			if(fw.IsFinal)
			{
				throw JavaException.IllegalAccessError("Field " + fw.DeclaringType.Name + "." + fw.Name + " is final");
			}
			fw.SetValue(obj, val);
		}

		[DebuggerStepThroughAttribute]
		public static void DynamicPutstatic(object val, string name, string sig, RuntimeTypeHandle type, string clazz)
		{
			Profiler.Count("DynamicPutstatic");
			FieldWrapper fw = GetFieldWrapper(null, type, clazz, name, sig, true);
			if(fw.IsFinal)
			{
				throw JavaException.IllegalAccessError("Field " + fw.DeclaringType.Name + "." + fw.Name + " is final");
			}
			fw.SetValue(null, val);
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
			try
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
				wrapper.Finish();
				return wrapper;
			}
			catch(RetargetableJavaException x)
			{
				throw x.ToJava();
			}
		}

		[DebuggerStepThroughAttribute]
		public static object DynamicClassLiteral(RuntimeTypeHandle type, string clazz)
		{
			Profiler.Count("DynamicClassLiteral");
			return IKVM.NativeCode.java.lang.VMClass.getClassFromWrapper(LoadTypeWrapper(type, clazz));
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

		private static MethodWrapper GetMethodWrapper(TypeWrapper thisType, RuntimeTypeHandle type, string clazz, string name, string sig, bool isStatic)
		{
			TypeWrapper caller = ClassLoaderWrapper.GetWrapperFromType(Type.GetTypeFromHandle(type));
			TypeWrapper wrapper = LoadTypeWrapper(type, clazz);
			MethodWrapper mw = wrapper.GetMethodWrapper(new MethodDescriptor(name, sig), false);
			if(mw == null)
			{
				throw JavaException.NoSuchMethodError(clazz + "." + name + sig);
			}
			// TODO check loader constraints
			if(mw.IsStatic != isStatic)
			{
				throw JavaException.IncompatibleClassChangeError(clazz + "." + name);
			}
			if(mw.IsAccessibleFrom(caller, thisType == null ? wrapper : thisType))
			{
				return mw;
			}
			throw JavaException.IllegalAccessError(clazz + "." + name + sig);
		}

		[DebuggerStepThroughAttribute]
		public static object DynamicInvokeSpecialNew(RuntimeTypeHandle type, string clazz, string name, string sig, object[] args)
		{
			Profiler.Count("DynamicInvokeSpecialNew");
			return GetMethodWrapper(null, type, clazz, name, sig, false).Invoke(null, args, false);
		}

		[DebuggerStepThroughAttribute]
		public static object DynamicInvokestatic(RuntimeTypeHandle type, string clazz, string name, string sig, object[] args)
		{
			Profiler.Count("DynamicInvokestatic");
			return GetMethodWrapper(null, type, clazz, name, sig, true).Invoke(null, args, false);
		}

		[DebuggerStepThroughAttribute]
		public static object DynamicInvokevirtual(object obj, RuntimeTypeHandle type, string clazz, string name, string sig, object[] args)
		{
			Profiler.Count("DynamicInvokevirtual");
			return GetMethodWrapper(ClassLoaderWrapper.GetWrapperFromType(obj.GetType()), type, clazz, name, sig, false).Invoke(obj, args, true);
		}

		[DebuggerStepThroughAttribute]
		public static Type DynamicGetTypeAsExceptionType(RuntimeTypeHandle type, string clazz)
		{
			Profiler.Count("DynamicGetTypeAsExceptionType");
			return LoadTypeWrapper(type, clazz).TypeAsExceptionType;
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

		[DebuggerStepThroughAttribute]
		public static void monitorenter(object obj)
		{
			bool interruptPending = false;
			for(;;)
			{
				try
				{
					System.Threading.Monitor.Enter(obj);
					if(interruptPending)
					{
						System.Threading.Thread.CurrentThread.Interrupt();
					}
					return;
				}
				catch(System.Threading.ThreadInterruptedException)
				{
					interruptPending = true;
				}
			}
		}

		// This is used by static JNI and synchronized methods that need a class object
		[DebuggerStepThroughAttribute]
		public static object GetClassFromTypeHandle(RuntimeTypeHandle typeHandle)
		{
			return IKVM.NativeCode.java.lang.VMClass.getClassFromType(Type.GetTypeFromHandle(typeHandle));
		}

		[DebuggerStepThroughAttribute]
		public static void arraycopy(object src, int srcStart, object dest, int destStart, int len)
		{
			if(src != dest)
			{
				// NOTE side effect of GetTypeHandle call is null check for src and dest (it
				// throws an ArgumentNullException)
				// Since constructing a Type object is expensive, we use Type.GetTypeHandle and
				// hope that it is implemented in a such a way that it is more efficient than
				// Object.GetType()
				try
				{
					RuntimeTypeHandle type_src = Type.GetTypeHandle(src);
					RuntimeTypeHandle type_dst = Type.GetTypeHandle(dest);
					if(type_src.Value != type_dst.Value)
					{
						if(len >= 0)
						{
							try
							{
								// since Java strictly defines what happens when an ArrayStoreException occurs during copying
								// and .NET doesn't, we have to do it by hand
								object[] src1 = src as object[];
								object[] dst1 = dest as object[];
								if(src1 != null && dst1 != null)
								{
									for(; len > 0; len--)
									{
										dst1[destStart++] = src1[srcStart++];
									}
								}
								else
								{
									// one (or both) of the arrays might be a value type array
									Array src2 = (Array)src;
									Array dst2 = (Array)dest;
									// we don't want to allow copying a primitive into an object array!
									if(ClassLoaderWrapper.GetWrapperFromType(src2.GetType().GetElementType()).IsPrimitive ||
										ClassLoaderWrapper.GetWrapperFromType(dst2.GetType().GetElementType()).IsPrimitive)
									{
										throw JavaException.ArrayStoreException();
									}
									for(; len > 0; len--)
									{
										dst2.SetValue(src2.GetValue(srcStart++), destStart++);
									}
								}
								return;
							}
							catch(InvalidCastException)
							{
								throw JavaException.ArrayStoreException();
							}
						}
						throw JavaException.ArrayIndexOutOfBoundsException();
					}
				}
				catch(ArgumentNullException)
				{
					throw JavaException.NullPointerException();
				}
			}
			try 
			{
				Array.Copy((Array)src, srcStart, (Array)dest, destStart, len);
			}
			catch(ArgumentNullException)
			{
				throw JavaException.NullPointerException();
			}
			catch(ArgumentException) 
			{
				throw JavaException.ArrayIndexOutOfBoundsException();
			}
			catch(InvalidCastException)
			{
				throw JavaException.ArrayStoreException();
			}
		}
		
		[DebuggerStepThroughAttribute]
		public static void arraycopy_primitive_8(Array src, int srcStart, Array dest, int destStart, int len)
		{
			try 
			{
				checked
				{
					Buffer.BlockCopy(src, srcStart << 3, dest, destStart << 3, len << 3);
					return;
				}
			}
			catch(ArgumentNullException)
			{
				throw JavaException.NullPointerException();
			}
			catch(OverflowException)
			{
				throw JavaException.ArrayIndexOutOfBoundsException();
			}
			catch(ArgumentException) 
			{
				throw JavaException.ArrayIndexOutOfBoundsException();
			}
		}

		[DebuggerStepThroughAttribute]
		public static void arraycopy_primitive_4(Array src, int srcStart, Array dest, int destStart, int len)
		{
			try 
			{
				checked
				{
					Buffer.BlockCopy(src, srcStart << 2, dest, destStart << 2, len << 2);
					return;
				}
			}
			catch(ArgumentNullException)
			{
				throw JavaException.NullPointerException();
			}
			catch(OverflowException)
			{
				throw JavaException.ArrayIndexOutOfBoundsException();
			}
			catch(ArgumentException) 
			{
				throw JavaException.ArrayIndexOutOfBoundsException();
			}
		}

		[DebuggerStepThroughAttribute]
		public static void arraycopy_primitive_2(Array src, int srcStart, Array dest, int destStart, int len)
		{
			try 
			{
				checked
				{
					Buffer.BlockCopy(src, srcStart << 1, dest, destStart << 1, len << 1);
					return;
				}
			}
			catch(ArgumentNullException)
			{
				throw JavaException.NullPointerException();
			}
			catch(OverflowException)
			{
				throw JavaException.ArrayIndexOutOfBoundsException();
			}
			catch(ArgumentException) 
			{
				throw JavaException.ArrayIndexOutOfBoundsException();
			}
		}

		[DebuggerStepThroughAttribute]
		public static void arraycopy_primitive_1(Array src, int srcStart, Array dest, int destStart, int len)
		{
			try 
			{
				Buffer.BlockCopy(src, srcStart, dest, destStart, len);
				return;
			}
			catch(ArgumentNullException)
			{
				throw JavaException.NullPointerException();
			}
			catch(OverflowException)
			{
				throw JavaException.ArrayIndexOutOfBoundsException();
			}
			catch(ArgumentException) 
			{
				throw JavaException.ArrayIndexOutOfBoundsException();
			}
		}
	}
}
