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
using IKVM.Runtime;
using IKVM.Internal;

abstract class RetargetableJavaException : ApplicationException
{
	internal RetargetableJavaException(string msg) : base(msg)
	{
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
		return JVM.Library.newLinkageError(Message);
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
		return JVM.Library.newVerifyError(Message);
	}
}

class ClassNotFoundException : RetargetableJavaException
{
	internal ClassNotFoundException(string name) : base(name)
	{
	}

	internal override Exception ToJava()
	{
		return JVM.Library.newClassNotFoundException(Message);
	}
}

class ClassCircularityError : LinkageError
{
	internal ClassCircularityError(string msg) : base(msg)
	{
	}

	internal override Exception ToJava()
	{
		return JVM.Library.newClassCircularityError(Message);
	}
}

class NoClassDefFoundError : LinkageError
{
	internal NoClassDefFoundError(string msg) : base(msg)
	{
	}

	internal override Exception ToJava()
	{
		return JVM.Library.newNoClassDefFoundError(Message);
	}
}

class IncompatibleClassChangeError : LinkageError
{
	internal IncompatibleClassChangeError(string msg) : base(msg)
	{
	}

	internal override Exception ToJava()
	{
		return JVM.Library.newIncompatibleClassChangeError(Message);
	}
}

class IllegalAccessError : IncompatibleClassChangeError
{
	internal IllegalAccessError(string msg) : base(msg)
	{
	}

	internal override Exception ToJava()
	{
		return JVM.Library.newIllegalAccessError(Message);
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
		return JVM.Library.newClassFormatError(Message);
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
		return JVM.Library.newUnsupportedClassVersionError(Message);
	}
}

sealed class JavaException
{
	private JavaException() {}

	internal static Exception IllegalAccessError(string s, params object[] args)
	{
		return JVM.Library.newIllegalAccessError(string.Format(s, args));
	}

	internal static Exception IllegalAccessException(string s, params object[] args)
	{
		return JVM.Library.newIllegalAccessException(string.Format(s, args));
	}

	internal static Exception IncompatibleClassChangeError(string s, params object[] args)
	{
		return JVM.Library.newIncompatibleClassChangeError(string.Format(s, args));
	}

	internal static Exception NoClassDefFoundError(string s, params object[] args)
	{
		return JVM.Library.newNoClassDefFoundError(string.Format(s, args));
	}

	internal static Exception UnsatisfiedLinkError(string s, params object[] args)
	{
		return JVM.Library.newUnsatisfiedLinkError(string.Format(s, args));
	}

	internal static Exception IllegalArgumentException(string s, params object[] args)
	{
		return JVM.Library.newIllegalArgumentException(string.Format(s, args));
	}

	internal static Exception NegativeArraySizeException()
	{
		return JVM.Library.newNegativeArraySizeException();
	}

	internal static Exception ArrayStoreException()
	{
		return JVM.Library.newArrayStoreException();
	}

	internal static Exception IndexOutOfBoundsException(string s)
	{
		return JVM.Library.newIndexOutOfBoundsException(s);
	}

	internal static Exception StringIndexOutOfBoundsException()
	{
		return JVM.Library.newStringIndexOutOfBoundsException();
	}

	internal static Exception InvocationTargetException(Exception x)
	{
		return JVM.Library.newInvocationTargetException(x);
	}

	internal static Exception UnknownHostException(string s, params object[] args)
	{
		return JVM.Library.newUnknownHostException(string.Format(s, args));
	}

	internal static Exception ArrayIndexOutOfBoundsException()
	{
		return JVM.Library.newArrayIndexOutOfBoundsException();
	}

	internal static Exception NumberFormatException(string s, params object[] args)
	{
		return JVM.Library.newNumberFormatException(string.Format(s, args));
	}

	internal static Exception NullPointerException()
	{
		return JVM.Library.newNullPointerException();
	}

	internal static Exception ClassCastException(string s, params object[] args)
	{
		return JVM.Library.newClassCastException(string.Format(s, args));
	}

	internal static Exception NoSuchFieldError(string s, params object[] args)
	{
		return JVM.Library.newNoSuchFieldError(string.Format(s, args));
	}

	internal static Exception NoSuchMethodError(string s, params object[] args)
	{
		return JVM.Library.newNoSuchMethodError(string.Format(s, args));
	}

	internal static Exception InstantiationError(string s, params object[] args)
	{
		return JVM.Library.newInstantiationError(string.Format(s, args));
	}

	internal static Exception InstantiationException(string s, params object[] args)
	{
		return JVM.Library.newInstantiationException(string.Format(s, args));
	}

	internal static Exception InterruptedException()
	{
		return JVM.Library.newInterruptedException();
	}

	internal static Exception IllegalMonitorStateException()
	{
		return JVM.Library.newIllegalMonitorStateException();
	}
}
