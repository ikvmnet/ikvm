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
