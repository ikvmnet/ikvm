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

abstract class RetargetableJavaException : ApplicationException
{
	internal RetargetableJavaException(string msg) : base(msg)
	{
	}

	protected static Type Load(string clazz)
	{
		Tracer.Info(Tracer.Runtime, "Loading exception class: {0}", clazz);
		TypeWrapper tw = ClassLoaderWrapper.LoadClassCritical(clazz);
		tw.Finish();
		return tw.TypeAsTBD;
	}

	internal abstract Exception ToJava();
}

class LinkageError : RetargetableJavaException
{
	internal LinkageError(string msg) : base(msg)
	{
	}

	internal override Exception ToJava()
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.LinkageError"), new object[] { Message });
	}
}

class VerifyError : LinkageError
{
	private int byteCodeOffset;
	private string clazz;
	private string method;
	private string signature;
	private string instruction;

	internal VerifyError() : base("")
	{
	}

	internal VerifyError(string msg) : base(msg)
	{
	}

	internal void SetInfo(int byteCodeOffset, string clazz, string method, string signature, string instruction)
	{
		this.byteCodeOffset = byteCodeOffset;
		this.clazz = clazz;
		this.method = method;
		this.signature = signature;
		this.instruction = instruction;
	}

	public override string Message
	{
		get
		{
			if(clazz != null)
			{
				return string.Format("(class: {0}, method: {1}, signature: {2}, offset: {3}, instruction: {4}) {5}", clazz, method, signature, byteCodeOffset, instruction, base.Message);
			}
			else
			{
				return base.Message;
			}
		}
	}

	internal override Exception ToJava()
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.VerifyError"), new object[] { Message });
	}
}

class ClassNotFoundException : RetargetableJavaException
{
	internal ClassNotFoundException(string name) : base(name)
	{
	}

	internal override Exception ToJava()
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.ClassNotFoundException"), new object[] { Message });
	}
}

class ClassCircularityError : LinkageError
{
	internal ClassCircularityError(string msg) : base(msg)
	{
	}

	internal override Exception ToJava()
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.ClassCircularityError"), new object[] { Message });
	}
}

class NoClassDefFoundError : LinkageError
{
	internal NoClassDefFoundError(string msg) : base(msg)
	{
	}

	internal override Exception ToJava()
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.NoClassDefFoundError"), new object[] { Message });
	}
}

class IncompatibleClassChangeError : LinkageError
{
	internal IncompatibleClassChangeError(string msg) : base(msg)
	{
	}

	internal override Exception ToJava()
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.IncompatibleClassChangeError"), new object[] { Message });
	}
}

class IllegalAccessError : IncompatibleClassChangeError
{
	internal IllegalAccessError(string msg) : base(msg)
	{
	}

	internal override Exception ToJava()
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.IllegalAccessError"), new object[] { Message });
	}
}

internal class ClassFormatError : LinkageError
{
	internal ClassFormatError(string msg, params object[] p)
		: base(string.Format(msg, p))
	{
	}

	internal override Exception ToJava()
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.ClassFormatError"), new object[] { Message });
	}
}

internal class UnsupportedClassVersionError : ClassFormatError
{
	internal UnsupportedClassVersionError(string msg)
		: base(msg)
	{
	}

	internal override Exception ToJava()
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.UnsupportedClassVersionError"), new object[] { Message });
	}
}

sealed class JavaException
{
	private JavaException() {}

	private static Type Load(string clazz)
	{
		Tracer.Info(Tracer.Runtime, "Loading exception class: {0}", clazz);
		TypeWrapper tw = ClassLoaderWrapper.LoadClassCritical(clazz);
		tw.Finish();
		return tw.TypeAsTBD;
	}

	internal static Exception IllegalAccessError(string s, params object[] args)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.IllegalAccessError"), new object[] { String.Format(s, args) });
	}

	internal static Exception IllegalAccessException(string s, params object[] args)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.IllegalAccessException"), new object[] { String.Format(s, args) });
	}

	internal static Exception IncompatibleClassChangeError(string s, params object[] args)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.IncompatibleClassChangeError"), new object[] { String.Format(s, args) });
	}

	internal static Exception NoClassDefFoundError(string s, params object[] args)
	{
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

	internal static Exception NullPointerException()
	{
		// if we ever stop remapping exceptions generated in non-Java code, this needs to use
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

	internal static Exception NoSuchMethodError(string s, params object[] args)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.NoSuchMethodError"), new object[] { String.Format(s, args) });
	}

	internal static Exception InstantiationError(string s, params object[] args)
	{
		return (Exception)Activator.CreateInstance(Load("java.lang.InstantiationError"), new object[] { String.Format(s, args) });
	}
}
