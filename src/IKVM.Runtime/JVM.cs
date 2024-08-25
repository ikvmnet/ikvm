using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading;

using IKVM.CoreLib.Diagnostics.Tracing;
using IKVM.Runtime.Vfs;

namespace IKVM.Runtime
{

    /// <summary>
    /// Main state of the running JVM.
    /// </summary>
    public static partial class JVM
    {

        static readonly object syncRoot = new object();
        static bool initialized;

#if EXPORTER == false
        static int emitSymbols;
        internal static bool RelaxedVerification = true;
        internal static bool AllowNonVirtualCalls;
        internal static readonly bool DisableEagerClassLoading = SafeGetEnvironmentVariable("IKVM_DISABLE_EAGER_CLASS_LOADING") != null;
#endif

        /// <summary>
        /// Ensures the JVM is initialized.
        /// </summary>
        internal static void Init()
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            if (initialized == false)
            {
                lock (syncRoot)
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
                        Internal.SystemAccessor.InvokeInitializeSystemClass();

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

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false

        /// <summary>
        /// Gets the current <see cref="RuntimeContext"/> of the JVM.
        /// </summary>
        internal static RuntimeContext Context => Internal.context;

        /// <summary>
        /// Gets the current <see cref="VfsTable"/> of the JVM.
        /// </summary>
        internal static VfsTable Vfs => Internal.vfs;

        /// <summary>
        /// Gets the 'system' thread group.
        /// </summary>
        internal static object SystemThreadGroup => Internal.systemThreadGroup.Value;

        /// <summary>
        /// Gets the 'main' thread group.
        /// </summary>
        internal static object MainThreadGroup => Internal.mainThreadGroup.Value;

        /// <summary>
        /// Thread-local storage of queued exception to be thrown.
        /// </summary>
        [ThreadStatic]
        static Exception pendingException;

        /// <summary>
        /// Gets the currently pending exception for the current thread.
        /// </summary>
        internal static Exception GetPendingException()
        {
            return pendingException;
        }

        /// <summary>
        /// Sets the pending exception for the current thread.
        /// </summary>
        /// <param name="e"></param>
        internal static void SetPendingException(Exception e)
        {
            pendingException = e != null ? ikvm.runtime.Util.mapException(e) : null;
        }

        /// <summary>
        /// Throws a pending exception for the current thread.
        /// </summary>
        internal static void ThrowPendingException()
        {
            var e = GetPendingException();
            if (e is not null)
            {
                pendingException = null;
                ExceptionDispatchInfo.Capture(e).Throw();
                throw null;
            }
        }

#endif

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

        internal static string MangleResourceName(string name)
        {
            // FXBUG there really shouldn't be any need to mangle the resource names,
            // but in order for ILDASM/ILASM round tripping to work reliably, we have
            // to make sure that we don't produce resource names that'll cause ILDASM
            // to generate invalid filenames.
            var sb = new StringBuilder("ikvm__", name.Length + 6);
            foreach (var c in name)
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

        /// <summary>
        /// Returns a hash code for the specified string value which is guarenteed to always be the same.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static int PersistableHash(string str)
        {
            var key = 1u;

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
            throw new NotImplementedException();
#else
            return val switch
            {
                byte => java.lang.Byte.valueOf((byte)val),
                bool => java.lang.Boolean.valueOf((bool)val),
                short => java.lang.Short.valueOf((short)val),
                char => java.lang.Character.valueOf((char)val),
                int => java.lang.Integer.valueOf((int)val),
                float => java.lang.Float.valueOf((float)val),
                long => java.lang.Long.valueOf((long)val),
                double => java.lang.Double.valueOf((double)val),
                _ => throw new java.lang.IllegalArgumentException()
            };
#endif
        }

        internal static object Unbox(object val)
        {
#if IMPORTER || FIRST_PASS || EXPORTER
            throw new NotImplementedException();
#else
            return val switch
            {
                java.lang.Byte b => b.byteValue(),
                java.lang.Boolean b1 => b1.booleanValue(),
                java.lang.Short s => s.shortValue(),
                java.lang.Character c => c.charValue(),
                java.lang.Integer i => i.intValue(),
                java.lang.Float f => f.floatValue(),
                java.lang.Long l => l.longValue(),
                java.lang.Double d => d.doubleValue(),
                _ => throw new java.lang.IllegalArgumentException()
            };
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

    }

}
