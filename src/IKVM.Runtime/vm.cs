/*
  Copyright (C) 2002-2013 Jeroen Frijters

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
using System.Threading;
using System.Diagnostics;
using System.Text;
using System.Security;

#if IMPORTER || EXPORTER
using IKVM.Reflection;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
#endif

#if IMPORTER
using IKVM.Tools.Importer;
#endif

#if !IMPORTER && !EXPORTER

namespace IKVM.Internal
{

    public static class Starter
    {

    }

}

#endif // !IMPORTER && !EXPORTER

namespace IKVM.Internal
{

    static class JVM
    {

        internal const string JarClassList = "--ikvm-classes--/";

#if !EXPORTER
        private static int emitSymbols;
#if CLASSGC
        internal static bool classUnloading = true;
#endif
#endif
        private static Assembly coreAssembly;

#if !EXPORTER
        internal static bool relaxedVerification = true;
        internal static bool AllowNonVirtualCalls;
        internal static readonly bool DisableEagerClassLoading = SafeGetEnvironmentVariable("IKVM_DISABLE_EAGER_CLASS_LOADING") != null;
#endif

#if !IMPORTER && !EXPORTER && !FIRST_PASS

        static JVM()
        {

        }

#endif

        internal static Version SafeGetAssemblyVersion(System.Reflection.Assembly asm)
        {
            // Assembly.GetName().Version requires FileIOPermission,
            // so we parse the FullName manually :-(
            string name = asm.FullName;
            int start = name.IndexOf(", Version=");
            if (start >= 0)
            {
                start += 10;
                int end = name.IndexOf(',', start);
                if (end >= 0)
                {
                    return new Version(name.Substring(start, end - start));
                }
            }
            return new Version();
        }

        internal static string SafeGetEnvironmentVariable(string name)
        {
            try
            {
                return Environment.GetEnvironmentVariable(name);
            }
            catch (SecurityException)
            {
                return null;
            }
        }

        internal static Assembly CoreAssembly
        {
            get
            {
#if !IMPORTER && !EXPORTER
                if (coreAssembly == null)
                {
#if FIRST_PASS
                    throw new InvalidOperationException("This version of IKVM.Runtime.dll was compiled with FIRST_PASS defined.");
#else
                    coreAssembly = typeof(java.lang.Object).Assembly;
#endif
                }
#endif // !IMPORTER
                return coreAssembly;
            }
            set
            {
                coreAssembly = value;
            }
        }

#if !IMPORTER && !EXPORTER

        internal static bool EmitSymbols
        {
            get
            {
                if (emitSymbols == 0)
                {
                    var state = 2;

#if NETFRAMEWORK
                    // check app.config on Framework
                    if (string.Equals(System.Configuration.ConfigurationManager.AppSettings["ikvm-emit-symbols"] ?? "", "true", StringComparison.OrdinalIgnoreCase))
                        state = 1;
#endif

                    // respect the IKVM_EMIT_SYMBOLs environmental variable
                    if (string.Equals(Environment.GetEnvironmentVariable("IKVM_EMIT_SYMBOLS") ?? "", "true", StringComparison.OrdinalIgnoreCase))
                        state = 1;

                    // by default enable symbols if a debugger is attached
                    if (state == 2 && Debugger.IsAttached)
                        state = 1;

                    // make sure we only set the value once, because it isn't allowed to changed as that could cause
                    // the compiler to try emitting symbols into a ModuleBuilder that doesn't accept them (and would
                    // throw an InvalidOperationException)
                    Interlocked.CompareExchange(ref emitSymbols, state, 0);
                }

                return emitSymbols == 1;
            }
        }
#endif

        internal static bool IsUnix
        {
            get
            {
                return Environment.OSVersion.Platform == PlatformID.Unix;
            }
        }

        internal static string MangleResourceName(string name)
        {
            // FXBUG there really shouldn't be any need to mangle the resource names,
            // but in order for ILDASM/ILASM round tripping to work reliably, we have
            // to make sure that we don't produce resource names that'll cause ILDASM
            // to generate invalid filenames.
            StringBuilder sb = new StringBuilder("ikvm__", name.Length + 6);
            foreach (char c in name)
            {
                if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_-+.()$#@~=&{}[]0123456789`".IndexOf(c) != -1)
                {
                    sb.Append(c);
                }
                else if (c == '/')
                {
                    sb.Append('!');
                }
                else
                {
                    sb.Append('%');
                    sb.Append(string.Format("{0:X4}", (int)c));
                }
            }
            return sb.ToString();
        }

        // based on Bret Mulvey's C# port of Jenkins32
        // note that this algorithm cannot be changed, because we persist these hashcodes in the metadata of shared class loader assemblies
        internal static int PersistableHash(string str)
        {
            uint key = 1;
            foreach (char c in str)
            {
                key += c;
                key += (key << 12);
                key ^= (key >> 22);
                key += (key << 4);
                key ^= (key >> 9);
                key += (key << 10);
                key ^= (key >> 2);
                key += (key << 7);
                key ^= (key >> 12);
            }
            return (int)key;
        }

#if IMPORTER || EXPORTER
		internal static Type LoadType(System.Type type)
		{
			return StaticCompiler.GetRuntimeType(type.FullName);
		}
#endif

        // this method resolves types in IKVM.Runtime.dll
        // (the version of IKVM.Runtime.dll that we're running
        // with can be different from the one we're compiling against.)
        internal static Type LoadType(Type type)
        {
#if IMPORTER || EXPORTER
			return StaticCompiler.GetRuntimeType(type.FullName);
#else
            return type;
#endif
        }

        internal static object Box(object val)
        {
#if IMPORTER || FIRST_PASS || EXPORTER
            return null;
#else
            if (val is byte)
            {
                return java.lang.Byte.valueOf((byte)val);
            }
            else if (val is bool)
            {
                return java.lang.Boolean.valueOf((bool)val);
            }
            else if (val is short)
            {
                return java.lang.Short.valueOf((short)val);
            }
            else if (val is char)
            {
                return java.lang.Character.valueOf((char)val);
            }
            else if (val is int)
            {
                return java.lang.Integer.valueOf((int)val);
            }
            else if (val is float)
            {
                return java.lang.Float.valueOf((float)val);
            }
            else if (val is long)
            {
                return java.lang.Long.valueOf((long)val);
            }
            else if (val is double)
            {
                return java.lang.Double.valueOf((double)val);
            }
            else
            {
                throw new java.lang.IllegalArgumentException();
            }
#endif
        }

        internal static object Unbox(object val)
        {
#if IMPORTER || FIRST_PASS || EXPORTER
            return null;
#else
            java.lang.Byte b = val as java.lang.Byte;
            if (b != null)
            {
                return b.byteValue();
            }
            java.lang.Boolean b1 = val as java.lang.Boolean;
            if (b1 != null)
            {
                return b1.booleanValue();
            }
            java.lang.Short s = val as java.lang.Short;
            if (s != null)
            {
                return s.shortValue();
            }
            java.lang.Character c = val as java.lang.Character;
            if (c != null)
            {
                return c.charValue();
            }
            java.lang.Integer i = val as java.lang.Integer;
            if (i != null)
            {
                return i.intValue();
            }
            java.lang.Float f = val as java.lang.Float;
            if (f != null)
            {
                return f.floatValue();
            }
            java.lang.Long l = val as java.lang.Long;
            if (l != null)
            {
                return l.longValue();
            }
            java.lang.Double d = val as java.lang.Double;
            if (d != null)
            {
                return d.doubleValue();
            }
            else
            {
                throw new java.lang.IllegalArgumentException();
            }
#endif
        }

#if !IMPORTER && !EXPORTER
        internal static object NewAnnotation(java.lang.ClassLoader classLoader, object definition)
        {
#if !FIRST_PASS
            java.lang.annotation.Annotation ann = null;
            try
            {
                ann = (java.lang.annotation.Annotation)ikvm.@internal.AnnotationAttributeBase.newAnnotation(classLoader, definition);
            }
            catch (java.lang.TypeNotPresentException) { }
            if (ann != null && sun.reflect.annotation.AnnotationType.getInstance(ann.annotationType()).retention() == java.lang.annotation.RetentionPolicy.RUNTIME)
            {
                return ann;
            }
#endif
            return null;
        }
#endif

#if !IMPORTER && !EXPORTER
        internal static object NewAnnotationElementValue(java.lang.ClassLoader classLoader, java.lang.Class expectedClass, object definition)
        {
#if FIRST_PASS
            return null;
#else
            try
            {
                return ikvm.@internal.AnnotationAttributeBase.decodeElementValue(definition, expectedClass, classLoader);
            }
            catch (java.lang.IllegalAccessException)
            {
                // TODO this shouldn't be here
                return null;
            }
#endif
        }
#endif

#if !IMPORTER && !EXPORTER
        // helper for JNI (which doesn't have access to core library internals)
        internal static object NewDirectByteBuffer(long address, int capacity)
        {
#if FIRST_PASS
            return null;
#else
            return java.nio.DirectByteBuffer.__new(address, capacity);
#endif
        }
#endif

        internal static Type Import(System.Type type)
        {
#if IMPORTER || EXPORTER
			return StaticCompiler.Universe.Import(type);
#else
            return type;
#endif
        }

    }

    static class Experimental
    {
        internal static readonly bool JDK_9 = JVM.SafeGetEnvironmentVariable("IKVM_EXPERIMENTAL_JDK_9") != null;
    }

}
