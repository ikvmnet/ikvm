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

	private static Type Load(string clazz)
	{
		TypeWrapper tw = ClassLoaderWrapper.LoadClassCritical(clazz);
		tw.Finish();
		return tw.Type;
	}

	internal static Exception ClassFormatError(string s, params object[] args)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.ClassFormatError"), new object[] { String.Format(s, args) });
	}

	internal static Exception UnsupportedClassVersionError(string s, params object[] args)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.UnsupportedClassVersionError"), new object[] { String.Format(s, args) });
	}

	internal static Exception IllegalAccessError(string s, params object[] args)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.IllegalAccessError"), new object[] { String.Format(s, args) });
	}

	internal static Exception VerifyError(string s, params object[] args)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.VerifyError"), new object[] { String.Format(s, args) });
	}

	internal static Exception IncompatibleClassChangeError(string s, params object[] args)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.IncompatibleClassChangeError"), new object[] { String.Format(s, args) });
	}

	internal static Exception ClassNotFoundException(string s, params object[] args)
	{
		if(JVM.IsStaticCompilerPhase1)
		{
			Console.Error.WriteLine("ClassNotFoundException: {0}", s);
		}
		return (Exception)Activator.CreateInstance(Load("java.lang.ClassNotFoundException"), new object[] { String.Format(s, args) });
	}

	internal static Exception NoClassDefFoundError(string s, params object[] args)
	{
		if(JVM.IsStaticCompilerPhase1)
		{
			Console.Error.WriteLine("NoClassDefFoundError: {0}", s);
		}
		return (Exception)Activator.CreateInstance(Load("java.lang.NoClassDefFoundError"), new object[] { String.Format(s, args) });
	}

	internal static Exception UnsatisfiedLinkError(string s, params object[] args)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.UnsatisfiedLinkError"), new object[] { String.Format(s, args) });
	}

	internal static Exception IllegalStateException(string s, params object[] args)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.IllegalStateException"), new object[] { String.Format(s, args) });
	}

	internal static Exception IllegalArgumentException(string s, params object[] args)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.IllegalArgumentException"), new object[] { String.Format(s, args) });
	}

	internal static Exception NegativeArraySizeException()
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.NegativeArraySizeException"));
	}

	internal static Exception ArrayStoreException(string s)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.ArrayStoreException"), new object[] { s });
	}

	internal static Exception IndexOutOfBoundsException(string s)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.IndexOutOfBoundsException"), new object[] { s });
	}

	internal static Exception StringIndexOutOfBoundsException(string s)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.StringIndexOutOfBoundsException"), new object[] { s });
	}

	internal static Exception InvocationTargetException(Exception x)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.reflect.InvocationTargetException"), new object[] { x });
	}

	internal static Exception IOException(string s, params object[] args)
	{
		return (Exception)Activator.CreateInstance(Load("java.io.IOException"), new object[] { String.Format(s, args) });
	}

	internal static Exception UnknownHostException(string s, params object[] args)
	{
		return (Exception)Activator.CreateInstance(Load("java.net.UnknownHostException"), new object[] { String.Format(s, args) });
	}

	internal static Exception ArrayIndexOutOfBoundsException()
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.ArrayIndexOutOfBoundsException"));
	}

	internal static Exception NumberFormatException(string s, params object[] args)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.NumberFormatException"), new object[] { String.Format(s, args) });
	}

	internal static Exception CloneNotSupportedException()
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.CloneNotSupportedException"));
	}

	internal static Exception LinkageError(string s, params object[] args)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.LinkageError"), new object[] { String.Format(s, args) });
	}

	internal static Exception ClassCircularityError(string s, params object[] args)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.ClassCircularityError"), new object[] { String.Format(s, args) });
	}

	internal static Exception NullPointerException()
	{
		// TODO if we ever stop remapping exceptions generated in non-Java code, this needs to use
		// reflection to get a real java.lang.NullPointerException
		return new NullReferenceException();
	}

	internal static Exception ClassCastException(string s, params object[] args)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.ClassCastException"), new object[] { String.Format(s, args) });
	}

	internal static Exception NoSuchFieldError(string s, params object[] args)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.NoSuchFieldError"), new object[] { String.Format(s, args) });
	}
}
