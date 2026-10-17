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

using IKVM.ByteCode;

namespace IKVM.Runtime
{

    /// <summary>
    /// .NET exception that corresponds to a Java exception.
    /// </summary>
    abstract class RetargetableJavaException : ApplicationException
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
            if (!(InnerException is java.lang.Error) && !(InnerException is java.lang.RuntimeException))
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

    sealed class UnsupportedClassVersionError : ClassFormatError
    {

        /// <summary>
        /// The lowest class file version that can be loaded.
        /// </summary>
        internal static readonly ClassFormatVersion MinSupportedVersion = new ClassFormatVersion(45, 0);

        /// <summary>
        /// The highest class file version that can be loaded.
        /// </summary>
        internal static readonly ClassFormatVersion MaxSupportedVersion = new ClassFormatVersion(52, 0);

        /// <summary>
        /// Returns <c>true</c> if a class file of the given version can be loaded.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        internal static bool IsSupported(ClassFormatVersion version)
        {
            return version >= MinSupportedVersion && version <= MaxSupportedVersion;
        }

        /// <summary>
        /// Gets the name of the Java release that produces class files of the given version, for example "Java 11" for 55.0.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        internal static string GetJavaVersionName(ClassFormatVersion version)
        {
            var name = version.Major switch
            {
                < 45 => "unknown Java version",
                45 => version.Minor < 3 ? "Java 1.0" : "Java 1.1",
                46 => "Java 1.2",
                47 => "Java 1.3",
                48 => "Java 1.4",
                _ => "Java " + (version.Major - 44),
            };

            // class files that depend on preview features of their release set every bit of the minor version
            if (version.Major >= 56 && version.Minor == 0xFFFF)
                name += " preview";

            return name;
        }

        /// <summary>
        /// Formats the diagnostic message the importer reports when a class file of the given version cannot be compiled.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        internal static string FormatMessage(ClassFormatVersion version)
        {
            if (version < MinSupportedVersion)
                return $"Class file version {version} ({GetJavaVersionName(version)}) is not supported; IKVM supports class files from version {MinSupportedVersion} ({GetJavaVersionName(MinSupportedVersion)}) up to version {MaxSupportedVersion} ({GetJavaVersionName(MaxSupportedVersion)})";
            else
                return $"Class file version {version} ({GetJavaVersionName(version)}) is not supported; IKVM supports class files up to version {MaxSupportedVersion} ({GetJavaVersionName(MaxSupportedVersion)})";
        }

        /// <summary>
        /// Formats the message of the exception thrown to Java code, which matches the message produced by HotSpot. HotSpot
        /// uses the same wording for versions that are too old.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="className">The binary name of the class being defined, or <c>null</c> if it was not given.</param>
        /// <returns></returns>
        internal static string FormatJavaMessage(ClassFormatVersion version, string className)
        {
            if (className == null)
                return $"Unsupported class file version {version}, this version of the Java Runtime only recognizes class file versions up to {MaxSupportedVersion}";
            else
                return $"{className.Replace('.', '/')} has been compiled by a more recent version of the Java Runtime (class file version {version}), this version of the Java Runtime only recognizes class file versions up to {MaxSupportedVersion}";
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="className">The binary name of the class being defined, or <c>null</c> if it was not given.</param>
        internal UnsupportedClassVersionError(ClassFormatVersion version, string className = null) :
            base(FormatJavaMessage(version, className))
        {
            Version = version;
        }

        /// <summary>
        /// Gets the unsupported version of the class file.
        /// </summary>
        internal ClassFormatVersion Version { get; }

#if !IMPORTER && !FIRST_PASS && !EXPORTER

        internal override Exception ToJava()
        {
            return new java.lang.UnsupportedClassVersionError(Message);
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
