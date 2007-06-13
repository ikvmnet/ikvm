/*
  Copyright (C) 2002, 2003, 2004, 2005, 2006, 2007 Jeroen Frijters

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
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Text;
using System.Security;
using System.Security.Permissions;
using System.Runtime.Serialization;
using SystemArray = System.Array;
using IKVM.Attributes;
using IKVM.Runtime;
using IKVM.Internal;
#if !FIRST_PASS
using NegativeArraySizeException = java.lang.NegativeArraySizeException;
using IllegalArgumentException = java.lang.IllegalArgumentException;
using IllegalAccessException = java.lang.IllegalAccessException;
using NumberFormatException = java.lang.NumberFormatException;
using jlNoClassDefFoundError = java.lang.NoClassDefFoundError;
using jlrConstructor = java.lang.reflect.Constructor;
using jlrField = java.lang.reflect.Field;
#endif

namespace IKVM.NativeCode.java
{
	namespace io
	{
		public class VMObjectStreamClass
		{
			public static bool hasClassInitializer(object clazz)
			{
				TypeWrapper wrapper = TypeWrapper.FromClass(clazz);
				try
				{
					wrapper.Finish();
				}
				catch(RetargetableJavaException x)
				{
					x.ToJava();
				}
				Type type = wrapper.TypeAsTBD;
				if(!type.IsArray && type.TypeInitializer != null)
				{
					wrapper.RunClassInit();
					return !AttributeHelper.IsHideFromJava(type.TypeInitializer);
				}
				return false;
			}

			private static FieldWrapper GetFieldWrapperFromField(object field)
			{
#if FIRST_PASS
				return null;
#else
				return FieldWrapper.FromField(field);
#endif
			}

			public static void setDoubleNative(object field, object obj, double val)
			{
				GetFieldWrapperFromField(field).SetValue(obj, val);
			}

			public static void setFloatNative(object field, object obj, float val)
			{
				GetFieldWrapperFromField(field).SetValue(obj, val);
			}

			public static void setLongNative(object field, object obj, long val)
			{
				GetFieldWrapperFromField(field).SetValue(obj, val);
			}

			public static void setIntNative(object field, object obj, int val)
			{
				GetFieldWrapperFromField(field).SetValue(obj, val);
			}

			public static void setShortNative(object field, object obj, short val)
			{
				GetFieldWrapperFromField(field).SetValue(obj, val);
			}

			public static void setCharNative(object field, object obj, char val)
			{
				GetFieldWrapperFromField(field).SetValue(obj, val);
			}

			public static void setByteNative(object field, object obj, byte val)
			{
				GetFieldWrapperFromField(field).SetValue(obj, val);
			}

			public static void setBooleanNative(object field, object obj, bool val)
			{
				GetFieldWrapperFromField(field).SetValue(obj, val);
			}

			public static void setObjectNative(object field, object obj, object val)
			{
				GetFieldWrapperFromField(field).SetValue(obj, val);
			}
		}

		public class VMObjectInputStream
		{
			public static object allocateObject(object clazz, object constructor_clazz, object constructor)
			{
#if FIRST_PASS
				return null;
#else
				Profiler.Enter("ObjectInputStream.allocateObject");
				try
				{
					TypeWrapper wrapper = TypeWrapper.FromClass(clazz);
					// if we're trying to deserialize a string as a TC_OBJECT, just return an emtpy string (Sun does the same)
					if(wrapper == CoreClasses.java.lang.String.Wrapper)
					{
						return "";
					}
					wrapper.Finish();
					// TODO do we need error handling? (e.g. when trying to instantiate an interface or abstract class)
					object obj = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(wrapper.TypeAsBaseType);
					MethodWrapper mw = MethodWrapper.FromMethodOrConstructor(constructor);
					// TODO do we need error handling?
					mw.Invoke(obj, null, false);
					return obj;
				}
				catch(RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				finally
				{
					Profiler.Leave("ObjectInputStream.allocateObject");
				}
#endif
			}
		}
	}

#if !COMPACT_FRAMEWORK
	namespace security
	{
		public class VMAccessController
		{
			public static object getClassFromFrame(System.Diagnostics.StackFrame frame)
			{
				return gnu.classpath.VMStackWalker.getClassFromType(frame.GetMethod().DeclaringType);
			}
		}
	}
#endif
}

namespace IKVM.NativeCode.gnu.java.lang.management
{
	public class VMClassLoadingMXBeanImpl
	{
		public static int getLoadedClassCount()
		{
			// we don't really have a number of classes loaded, but we'll
			// return something anyway
			return ClassLoaderWrapper.GetLoadedClassCount();
		}
	}
}

namespace IKVM.NativeCode.gnu.classpath
{
	public class VMSystemProperties
	{
		public static string getVersion()
		{
			try
			{
				return JVM.SafeGetAssemblyVersion(typeof(VMSystemProperties).Assembly).ToString();
			}
			catch(Exception)
			{
				return "(unknown)";
			}
		}
	}

	public class VMStackWalker
	{
		private static readonly Hashtable isHideFromJavaCache = Hashtable.Synchronized(new Hashtable());

		public static object getClassFromType(Type type)
		{
			TypeWrapper.AssertFinished(type);
			if(type == null)
			{
				return null;
			}
			TypeWrapper tw = ClassLoaderWrapper.GetWrapperFromType(type);
			if(tw == null)
			{
				return null;
			}
			return tw.ClassObject;
		}

		public static object getClassLoaderFromType(Type type)
		{
			// global methods have no type
			if(type == null)
			{
				return null;
			}
			else if(type.Module is System.Reflection.Emit.ModuleBuilder)
			{
				return ClassLoaderWrapper.GetWrapperFromType(type).GetClassLoader().GetJavaClassLoader();
			}
			else
			{
				return ClassLoaderWrapper.GetAssemblyClassLoader(type.Assembly).GetJavaClassLoader();
			}
		}

		public static Type getJNIEnvType()
		{
#if COMPACT_FRAMEWORK
			return null;
#else
			return typeof(IKVM.Runtime.JNIEnv);
#endif
		}

		public static bool isHideFromJava(MethodBase mb)
		{
			// TODO on .NET 2.0 isHideFromJavaCache should be a Dictionary<RuntimeMethodHandle, bool>
			object cached = isHideFromJavaCache[mb];
			if(cached == null)
			{
				cached = mb.IsDefined(typeof(HideFromJavaAttribute), false)
					|| mb.IsDefined(typeof(HideFromReflectionAttribute), false);
				isHideFromJavaCache[mb] = cached;
			}
			return (bool)cached;
		}
	}
}
