/*
  Copyright (C) 2002 Jeroen Frijters

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

sealed class JavaException
{
	private JavaException() {}

	internal static Exception ClassFormatError(string s, params object[] args)
	{
		s = String.Format(s, args);
		ConstructorInfo ci = ClassLoaderWrapper.GetType("java.lang.ClassFormatError").GetConstructor(new Type[] { typeof(string) });
		return (Exception)ci.Invoke(new object[] { s });
	}

	internal static Exception UnsupportedClassVersionError(string s, params object[] args)
	{
		s = String.Format(s, args);
		ConstructorInfo ci = ClassLoaderWrapper.GetType("java.lang.UnsupportedClassVersionError").GetConstructor(new Type[] { typeof(string) });
		return (Exception)ci.Invoke(new object[] { s });
	}

	internal static Exception IllegalAccessError(string s, params object[] args)
	{
		s = String.Format(s, args);
		ConstructorInfo ci = ClassLoaderWrapper.GetType("java.lang.IllegalAccessError").GetConstructor(new Type[] { typeof(string) });
		return (Exception)ci.Invoke(new object[] { s });
	}

	internal static Exception VerifyError(string s, params object[] args)
	{
		s = String.Format(s, args);
		ConstructorInfo ci = ClassLoaderWrapper.GetType("java.lang.VerifyError").GetConstructor(new Type[] { typeof(string) });
		return (Exception)ci.Invoke(new object[] { s });
	}

	internal static Exception IncompatibleClassChangeError(string s, params object[] args)
	{
		s = String.Format(s, args);
		ConstructorInfo ci = ClassLoaderWrapper.GetType("java.lang.IncompatibleClassChangeError").GetConstructor(new Type[] { typeof(string) });
		return (Exception)ci.Invoke(new object[] { s });
	}

	[ThreadStatic]
	private static bool classNotFound;

	private class BootstrapClassMissing : Exception {}

	internal static Exception ClassNotFoundException(string s, params object[] args)
	{
		// HACK if java.lang.ClassNotFoundException is not found, this method would recurse until the
		// stack overflows, so in order to prevent that, we use this hack
		if(classNotFound)
		{
			throw new BootstrapClassMissing();
		}
		if(JVM.IsStaticCompiler)
		{
			Console.WriteLine("ClassNotFound: " + s);
			//Console.WriteLine(new System.Diagnostics.StackTrace(true));
		}
		try
		{
			classNotFound = true;
			//Console.WriteLine("ClassNotFoundException: " + s);
			s = String.Format(s, args);
			ConstructorInfo ci = ClassLoaderWrapper.GetType("java.lang.ClassNotFoundException").GetConstructor(new Type[] { typeof(string) });
			return (Exception)ci.Invoke(new object[] { s });
		}
		catch(BootstrapClassMissing)
		{
			throw new TypeLoadException("ClassNotFoundException: " + s);
		}
		finally
		{
			classNotFound = false;
		}
	}

	internal static Exception NoClassDefFoundError(string s, params object[] args)
	{
		s = String.Format(s, args);
		ConstructorInfo ci = ClassLoaderWrapper.GetType("java.lang.NoClassDefFoundError").GetConstructor(new Type[] { typeof(string) });
		return (Exception)ci.Invoke(new object[] { s });
	}

	internal static Exception UnsatisfiedLinkError(string s, params object[] args)
	{
		s = String.Format(s, args);
		ConstructorInfo ci = ClassLoaderWrapper.GetType("java.lang.UnsatisfiedLinkError").GetConstructor(new Type[] { typeof(string) });
		return (Exception)ci.Invoke(new object[] { s });
	}

	internal static Exception IllegalStateException(string s, params object[] args)
	{
		s = String.Format(s, args);
		ConstructorInfo ci = ClassLoaderWrapper.GetType("java.lang.IllegalStateException").GetConstructor(new Type[] { typeof(string) });
		return (Exception)ci.Invoke(new object[] { s });
	}

	internal static Exception IllegalArgumentException(string s, params object[] args)
	{
		s = String.Format(s, args);
		ConstructorInfo ci = ClassLoaderWrapper.GetType("java.lang.IllegalArgumentException").GetConstructor(new Type[] { typeof(string) });
		return (Exception)ci.Invoke(new object[] { s });
	}

	internal static Exception NegativeArraySizeException()
	{
		return (Exception)Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.NegativeArraySizeException"));
	}

	internal static Exception ArrayStoreException(string s)
	{
		ConstructorInfo ci = ClassLoaderWrapper.GetType("java.lang.ArrayStoreException").GetConstructor(new Type[] { typeof(string) });
		return (Exception)ci.Invoke(new object[] { s });
	}

	internal static Exception IndexOutOfBoundsException(string s)
	{
		ConstructorInfo ci = ClassLoaderWrapper.GetType("java.lang.IndexOutOfBoundsException").GetConstructor(new Type[] { typeof(string) });
		return (Exception)ci.Invoke(new object[] { s });
	}

	internal static Exception StringIndexOutOfBoundsException(string s)
	{
		ConstructorInfo ci = ClassLoaderWrapper.GetType("java.lang.StringIndexOutOfBoundsException").GetConstructor(new Type[] { typeof(string) });
		return (Exception)ci.Invoke(new object[] { s });
 	}

	internal static Exception InvocationTargetException(Exception x)
	{
		return (Exception)Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.reflect.InvocationTargetException"), new object[] { x });
	}

	internal static Exception IOException(string s, params object[] args)
	{
		s = String.Format(s, args);
		ConstructorInfo ci = ClassLoaderWrapper.GetType("java.io.IOException").GetConstructor(new Type[] { typeof(string) });
		return (Exception)ci.Invoke(new object[] { s });
	}

	internal static Exception UnknownHostException(string s, params object[] args)
	{
		s = String.Format(s, args);
		ConstructorInfo ci = ClassLoaderWrapper.GetType("java.net.UnknownHostException").GetConstructor(new Type[] { typeof(string) });
		return (Exception)ci.Invoke(new object[] { s });
	}

	internal static Exception ArrayIndexOutOfBoundsException()
	{
		return (Exception)Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.ArrayIndexOutOfBoundsException"));
	}

	internal static Exception NumberFormatException(string s, params object[] args)
	{
		s = String.Format(s, args);
		ConstructorInfo ci = ClassLoaderWrapper.GetType("java.lang.NumberFormatException").GetConstructor(new Type[] { typeof(string) });
		return (Exception)ci.Invoke(new object[] { s });
	}

	internal static Exception CloneNotSupportedException()
	{
		return (Exception)Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.CloneNotSupportedException"));
	}
}
