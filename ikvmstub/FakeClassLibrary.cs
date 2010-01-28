/*
  Copyright (C) 2010 Jeroen Frijters

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
using System.Collections.Generic;
using System.Text;
using IKVM.Attributes;

[module: JavaModule]
[assembly: RemappedClass("java.lang.Object", typeof(object))]
[assembly: RemappedClass("java.lang.Throwable", typeof(Exception))]
[assembly: RemappedClass("java.lang.String", typeof(string))]
[assembly: RemappedClass("java.lang.Comparable", typeof(IComparable))]

namespace java.lang
{
	[RemappedType(typeof(object))]
	public abstract class Object
	{
		public abstract int hashCode();
		public abstract bool equals(object obj);
		protected abstract object clone();
		public abstract string toString();
		protected abstract void finalize();
	}

	[RemappedType(typeof(Exception))]
	public abstract class Throwable : Exception
	{
		// object methods
		public abstract int hashCode();
		public abstract bool equals(object obj);
		protected abstract object clone();
		public abstract string toString();
		protected abstract void finalize();
		
		// throwable methods
		public abstract string getMessage();
		public abstract string getLocalizedMessage();
		public abstract Throwable getCause();
		public abstract Throwable initCause(Throwable cause);
		public abstract void printStackTrace();
		public abstract void printStackTrace(java.io.PrintStream s);
		public abstract void printStackTrace(java.io.PrintWriter s);
		public abstract Throwable fillInStackTrace();
		public abstract StackTraceElement[] getStackTrace();
		public abstract void setStackTrace(StackTraceElement[] stackTrace);
	}

	[RemappedType(typeof(string))]
	public class String
	{
	}

	[RemappedType(typeof(IComparable))]
	public interface Comparable : IComparable
	{
	}

	public class Class
	{
	}

	public interface CharSequence
	{
	}

	public interface Cloneable
	{
	}

	public class Enum
	{
	}

	public class StackTraceElement
	{
	}

	namespace annotation
	{
		public interface Annotation
		{
		}
	}
}

namespace java.io
{
	public interface Serializable
	{
	}

	public class PrintStream
	{
	}

	public class PrintWriter
	{
	}
}
