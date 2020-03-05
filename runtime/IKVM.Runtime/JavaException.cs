/*
  Copyright (C) 2002-2014 Jeroen Frijters

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
using IKVM.Internal;

abstract class RetargetableJavaException : ApplicationException
{
	internal RetargetableJavaException(string msg) : base(msg)
	{
	}

	internal RetargetableJavaException(string msg, Exception x) : base(msg, x)
	{
	}

	internal static string Format(string s, object[] args)
	{
		if (args == null || args.Length == 0)
		{
			return s;
		}
		return String.Format(s, args);
	}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
	internal abstract Exception ToJava();
#elif FIRST_PASS
	internal virtual Exception ToJava()
	{
		return null;
	}
#endif
}

// NOTE this is not a Java exception, but instead it wraps a Java exception that
// was thrown by a class loader. It is used so ClassFile.LoadClassHelper() can catch
// Java exceptions and turn them into UnloadableTypeWrappers without inadvertantly
// hiding exceptions caused by coding errors in the IKVM code.
sealed class ClassLoadingException : RetargetableJavaException
{
	internal ClassLoadingException(Exception x, string className)
		: base(className, x)
	{
	}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
	internal override Exception ToJava()
	{
		if (!(InnerException is java.lang.Error) && !(InnerException is java.lang.RuntimeException))
		{
			return new java.lang.NoClassDefFoundError(Message.Replace('.', '/')).initCause(InnerException);
		}
		return InnerException;
	}
#endif
}

class LinkageError : RetargetableJavaException
{
	internal LinkageError(string msg) : base(msg)
	{
	}

	internal LinkageError(string msg, Exception x) : base(msg, x)
	{
	}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
	internal override Exception ToJava()
	{
		return new java.lang.LinkageError(Message);
	}
#endif
}

sealed class VerifyError : LinkageError
{
	internal VerifyError() : base("")
	{
	}

	internal VerifyError(string msg) : base(msg)
	{
	}

	internal VerifyError(string msg, Exception x) : base(msg, x)
	{
	}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
	internal override Exception ToJava()
	{
		return new java.lang.VerifyError(Message);
	}
#endif
}

sealed class ClassNotFoundException : RetargetableJavaException
{
	internal ClassNotFoundException(string name) : base(name)
	{
	}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
	internal override Exception ToJava()
	{
		return new java.lang.NoClassDefFoundError(Message);
	}
#endif
}

sealed class ClassCircularityError : LinkageError
{
	internal ClassCircularityError(string msg) : base(msg)
	{
	}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
	internal override Exception ToJava()
	{
		return new java.lang.ClassCircularityError(Message);
	}
#endif
}

sealed class NoClassDefFoundError : LinkageError
{
	internal NoClassDefFoundError(string msg) : base(msg)
	{
	}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
	internal override Exception ToJava()
	{
		return new java.lang.NoClassDefFoundError(Message);
	}
#endif
}

class IncompatibleClassChangeError : LinkageError
{
	internal IncompatibleClassChangeError(string msg) : base(msg)
	{
	}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
	internal override Exception ToJava()
	{
		return new java.lang.IncompatibleClassChangeError(Message);
	}
#endif
}

sealed class IllegalAccessError : IncompatibleClassChangeError
{
	internal IllegalAccessError(string msg) : base(msg)
	{
	}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
	internal override Exception ToJava()
	{
		return new java.lang.IllegalAccessError(Message);
	}
#endif
}

class ClassFormatError : LinkageError
{
	internal ClassFormatError(string msg, params object[] p)
		: base(Format(msg, p))
	{
	}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
	internal override Exception ToJava()
	{
		return new java.lang.ClassFormatError(Message);
	}
#endif
}

sealed class UnsupportedClassVersionError : ClassFormatError
{
	internal UnsupportedClassVersionError(string msg)
		: base(msg)
	{
	}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
	internal override Exception ToJava()
	{
		return new java.lang.UnsupportedClassVersionError(Message);
	}
#endif
}

sealed class JavaSecurityException : RetargetableJavaException
{
	internal JavaSecurityException(string msg)
		: base(msg)
	{
	}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
	internal override Exception ToJava()
	{
		return new java.lang.SecurityException(Message);
	}
#endif
}
