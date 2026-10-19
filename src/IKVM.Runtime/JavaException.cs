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

using IKVM.CoreLib.Exceptions;

namespace IKVM.Runtime
{

    /// <summary>
    /// .NET exception that corresponds to a Java exception.
    /// </summary>
    abstract class RetargetableJavaException : TranslatableJavaException
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="msg"></param>
        internal RetargetableJavaException(string msg) :
            base(msg)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        internal RetargetableJavaException(string message, Exception innerException) :
            base(message, innerException)
        {

        }

        internal static string Format(string s, object[] args)
        {
            return args == null || args.Length == 0 ? s : string.Format(s, args);
        }

#if !IMPORTER && !FIRST_PASS && !EXPORTER

        internal abstract Exception ToJava();

#elif FIRST_PASS

	internal virtual Exception ToJava()
	{
		return null;
	}

#endif

    }

    /// <summary>
    /// This is not a Java exception, but instead it wraps a Java exception that
    /// was thrown by a class loader. It is used so ClassFile.LoadClassHelper() can catch
    /// Java exceptions and turn them into UnloadableTypeWrappers without inadvertantly
    /// hiding exceptions caused by coding errors in the IKVM code.
    /// </summary>
    sealed class ClassLoadingException : RetargetableJavaException
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="className"></param>
        internal ClassLoadingException(Exception innerException, string className) :
            base(className, innerException)
        {

        }

#if !IMPORTER && !FIRST_PASS && !EXPORTER

        internal override Exception ToJava()
        {
            if (InnerException is not java.lang.Error and not java.lang.RuntimeException)
                return new java.lang.NoClassDefFoundError(Message.Replace('.', '/')).initCause(InnerException);

            return InnerException;
        }

#endif

    }

    class LinkageError : RetargetableJavaException
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        internal LinkageError(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        internal LinkageError(string message, Exception innerException) :
            base(message, innerException)
        {

        }

#if !IMPORTER && !FIRST_PASS && !EXPORTER

        internal override Exception ToJava()
        {
            return new java.lang.LinkageError(Message);
        }

#endif

    }

    sealed class VerifyError : LinkageError
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal VerifyError() :
            base("")
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        internal VerifyError(string message) :
            base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        internal VerifyError(string message, Exception innerException) : base(message, innerException)
        {

        }

#if !IMPORTER && !FIRST_PASS && !EXPORTER

        internal override Exception ToJava()
        {
            return new java.lang.VerifyError(Message);
        }

#endif

    }

    sealed class ClassNotFoundException : RetargetableJavaException
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        internal ClassNotFoundException(string message) :
            base(message)
        {

        }

#if !IMPORTER && !FIRST_PASS && !EXPORTER

        internal override Exception ToJava()
        {
            return new java.lang.NoClassDefFoundError(Message);
        }

#endif

    }

    sealed class ClassCircularityError : LinkageError
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        internal ClassCircularityError(string message) :
            base(message)
        {

        }

#if !IMPORTER && !FIRST_PASS && !EXPORTER

        internal override Exception ToJava()
        {
            return new java.lang.ClassCircularityError(Message);
        }

#endif

    }

    sealed class NoClassDefFoundError : LinkageError
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        internal NoClassDefFoundError(string message) :
            base(message)
        {

        }

#if !IMPORTER && !FIRST_PASS && !EXPORTER

        internal override Exception ToJava()
        {
            return new java.lang.NoClassDefFoundError(Message);
        }

#endif

    }

    class IncompatibleClassChangeError : LinkageError
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        internal IncompatibleClassChangeError(string message) :
            base(message)
        {

        }

#if !IMPORTER && !FIRST_PASS && !EXPORTER

        internal override Exception ToJava()
        {
            return new java.lang.IncompatibleClassChangeError(Message);
        }

#endif

    }

    sealed class IllegalAccessError : IncompatibleClassChangeError
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        internal IllegalAccessError(string message) :
            base(message)
        {

        }

#if !IMPORTER && !FIRST_PASS && !EXPORTER
        internal override Exception ToJava()
        {
            return new java.lang.IllegalAccessError(Message);
        }

#endif

    }

    class ClassFormatError : LinkageError
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal ClassFormatError(string message, params object[] args) :
            base(Format(message, args))
        {

        }

#if !IMPORTER && !FIRST_PASS && !EXPORTER

        internal override Exception ToJava()
        {
            return new java.lang.ClassFormatError(Message);
        }

#endif

    }

    sealed class JavaSecurityException : RetargetableJavaException
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="msg"></param>
        internal JavaSecurityException(string msg) :
            base(msg)
        {

        }

#if !IMPORTER && !FIRST_PASS && !EXPORTER

        internal override Exception ToJava()
        {
            return new java.lang.SecurityException(Message);
        }

#endif

    }

}
