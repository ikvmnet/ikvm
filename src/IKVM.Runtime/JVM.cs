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

using IKVM.Runtime.Accessors;
using IKVM.Runtime.Accessors.Java.Lang;

using System.Runtime.CompilerServices;

using IKVM.Runtime.Vfs;

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
#endif

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Runtime
{

    internal static partial class JVM
    {

        internal const string JarClassList = "--ikvm-classes--/";

#if EXPORTER == false
        static int emitSymbols;
        internal static bool relaxedVerification = true;
        internal static bool AllowNonVirtualCalls;
        internal static readonly bool DisableEagerClassLoading = SafeGetEnvironmentVariable("IKVM_DISABLE_EAGER_CLASS_LOADING") != null;
#endif

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false

        static readonly RuntimeContext context = new RuntimeContext(new Resolver(), false);
        static readonly VfsTable vfs = VfsTable.BuildDefaultTable(new VfsRuntimeContext(context), Properties.HomePath);

        /// <summary>
        /// Gets the current <see cref="RuntimeContext"/> of the JVM.
        /// </summary>
        public static RuntimeContext Context => context;

        /// <summary>
        /// Gets the current <see cref="VfsTable"/> of the JVM.
        /// </summary>
        public static VfsTable Vfs => vfs;

        static readonly object initializedLock = new object();
        static bool initialized;

        static AccessorCache baseAccessors;
        static ThreadGroupAccessor threadGroupAccessor;
        static SystemAccessor systemAccessor;

        static Lazy<object> systemThreadGroup = new Lazy<object>(MakeSystemThreadGroup);
        static Lazy<object> mainThreadGroup = new Lazy<object>(MakeMainThreadGroup);

        internal static AccessorCache BaseAccessors => AccessorCache.Get(ref baseAccessors, context.Resolver.ResolveBaseAssembly());

        static ThreadGroupAccessor ThreadGroupAccessor => BaseAccessors.Get(ref threadGroupAccessor);

        static SystemAccessor SystemAccessor => BaseAccessors.Get(ref systemAccessor);

        /// <summary>
        /// Gets the 'system' thread group.
        /// </summary>
        public static object SystemThreadGroup => systemThreadGroup.Value;

        /// <summary>
        /// Gets the 'main' thread group.
        /// </summary>
        public static object MainThreadGroup => mainThreadGroup.Value;

#endif

        /// <summary>
        /// Ensures the JVM is initialized.
        /// </summary>
        public static void EnsureInitialized()
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            if (initialized == false)
            {
                lock (initializedLock)
                {
                    if (initialized == false)
                    {
                        initialized = true;

                        // always required
                        RuntimeHelpers.RunClassConstructor(typeof(java.lang.String).TypeHandle);

                        // initialize java_lang.System (needed before creating the thread)
                        RuntimeHelpers.RunClassConstructor(typeof(java.lang.System).TypeHandle);

                        // initialize thread groups
                        RuntimeHelpers.RunClassConstructor(typeof(java.lang.ThreadGroup).TypeHandle);
                        RuntimeHelpers.RunClassConstructor(typeof(java.lang.Thread).TypeHandle);
                        GC.KeepAlive(SystemThreadGroup);
                        GC.KeepAlive(MainThreadGroup);

                        // the VM creates & returns objects of this class
                        RuntimeHelpers.RunClassConstructor(typeof(java.lang.Class).TypeHandle);

                        // the VM preresolves methods to these classes
                        RuntimeHelpers.RunClassConstructor(typeof(java.lang.reflect.Method).TypeHandle);

                        // ensure the System class is initialized
                        SystemAccessor.InvokeInitializeSystemClass();

                        // should be done before System, but that fails for now
                        RuntimeHelpers.RunClassConstructor(typeof(java.lang.@ref.Finalizer).TypeHandle);

                        // initialize certain classes after System properties are available
                        RuntimeHelpers.RunClassConstructor(typeof(java.lang.OutOfMemoryError).TypeHandle);
                        RuntimeHelpers.RunClassConstructor(typeof(java.lang.NullPointerException).TypeHandle);
                        RuntimeHelpers.RunClassConstructor(typeof(java.lang.ClassCastException).TypeHandle);
                        RuntimeHelpers.RunClassConstructor(typeof(java.lang.ArrayStoreException).TypeHandle);
                        RuntimeHelpers.RunClassConstructor(typeof(java.lang.ArithmeticException).TypeHandle);
                        RuntimeHelpers.RunClassConstructor(typeof(java.lang.StackOverflowError).TypeHandle);
                        RuntimeHelpers.RunClassConstructor(typeof(java.lang.IllegalMonitorStateException).TypeHandle);
                        RuntimeHelpers.RunClassConstructor(typeof(java.lang.IllegalArgumentException).TypeHandle);

                        // the static initializer of java.lang.Compiler tries to read property "java.compiler" and read & write property "java.vm.info"
                        RuntimeHelpers.RunClassConstructor(typeof(java.lang.Compiler).TypeHandle);

                        // initialize some JSR292 core classes to avoid deadlock during class loading
                        RuntimeHelpers.RunClassConstructor(typeof(java.lang.invoke.MemberName).TypeHandle);
                        RuntimeHelpers.RunClassConstructor(typeof(java.lang.invoke.MethodHandle).TypeHandle);
                        RuntimeHelpers.RunClassConstructor(typeof(java.lang.invoke.MethodHandleNatives).TypeHandle);
                    }
                }
            }
#endif
        }

        /// <summary>
        /// Creates the 'system' thread group.
        /// </summary>
        /// <returns></returns>
        static object MakeSystemThreadGroup()
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            return ThreadGroupAccessor.Init();
#endif
        }

        /// <summary>
        /// Creates the 'main' thread group.
        /// </summary>
        /// <returns></returns>
        static object MakeMainThreadGroup()
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            return ThreadGroupAccessor.Init(null, SystemThreadGroup, "main");
#endif
        }

        /// <summary>
        /// Gets an environmental variable.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
#if FIRST_PASS
            throw new NotImplementedException();
#else
            java.lang.annotation.Annotation ann = null;
            try
            {
                ann = (java.lang.annotation.Annotation)ikvm.@internal.AnnotationAttributeBase.newAnnotation(classLoader, definition);
            }
            catch (java.lang.TypeNotPresentException)
            {

            }

            if (ann != null && sun.reflect.annotation.AnnotationType.getInstance(ann.annotationType()).retention() == java.lang.annotation.RetentionPolicy.RUNTIME)
                return ann;

            return null;
#endif
        }

#endif

#if !IMPORTER && !EXPORTER

        internal static object NewAnnotationElementValue(java.lang.ClassLoader classLoader, java.lang.Class expectedClass, object definition)
        {
#if FIRST_PASS
            throw new NotImplementedException();
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

    }

}
