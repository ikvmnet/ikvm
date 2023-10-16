using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Lang;
using IKVM.Runtime.Accessors.Java.Util;

namespace IKVM.Java.Externs.java.lang
{

    /// <summary>
    /// Implements the native methods for 'java.lang.System'.
    /// </summary>
    static class System
    {

#if FIRST_PASS == false

        static SystemAccessor systemAccessor;
        static PropertiesAccessor propertiesAccessor;

        static SystemAccessor SystemAccessor => JVM.BaseAccessors.Get(ref systemAccessor);

        static PropertiesAccessor PropertiesAccessor => JVM.BaseAccessors.Get(ref propertiesAccessor);

#endif

        const long january_1st_1970 = 62135596800000L;

        /// <summary>
        /// Implements the native method 'registerNatives'.
        /// </summary>
        public static void registerNatives()
        {

        }

        /// <summary>
        /// Implements the native method 'setIn0'.
        /// </summary>
        /// <param name="in"></param>
        public static void setIn0(object @in)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            SystemAccessor.SetIn(@in);
#endif
        }

        /// <summary>
        /// Implements the native method 'setOut0'.
        /// </summary>
        public static void setOut0(object @out)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            SystemAccessor.SetOut(@out);
#endif
        }

        /// <summary>
        /// Implements the native method 'setErr0'.
        /// </summary>
        public static void setErr0(object err)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            SystemAccessor.SetErr(err);
#endif
        }

        /// <summary>
        /// Implements the native method 'currentTimeMillis'.
        /// </summary>
        /// <returns></returns>
        public static long currentTimeMillis()
        {
            return DateTime.UtcNow.Ticks / 10000L - january_1st_1970;
        }

        /// <summary>
        /// Implements the native method 'nanoTime'.
        /// </summary>
        /// <returns></returns>
        public static long nanoTime()
        {
            const long NANOS_PER_SEC = 1000000000;

            var time = (double)Stopwatch.GetTimestamp();
            var freq = (double)Stopwatch.Frequency;
            return (long)(time / freq * NANOS_PER_SEC);
        }

        /// <summary>
        /// Implements the native method 'arraycopy'.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="srcPos"></param>
        /// <param name="dest"></param>
        /// <param name="destPos"></param>
        /// <param name="length"></param>
        public static void arraycopy(object src, int srcPos, object dest, int destPos, int length)
        {
            ByteCodeHelper.arraycopy(src, srcPos, dest, destPos, length);
        }

        /// <summary>
        /// Implements the native method 'identityHashCode'.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int identityHashCode(object x)
        {
            return RuntimeHelpers.GetHashCode(x);
        }

        /// <summary>
        /// Implements the native method 'initProperties'.
        /// </summary>
        /// <param name="props"></param>
        /// <returns></returns>
        public static object initProperties(object props)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            foreach (var kvp in JVM.Properties.Init)
                if (kvp.Value is string v)
                    PropertiesAccessor.InvokeSetProperty(props, kvp.Key, v);

            return props;
#endif
        }

        /// <summary>
        /// Implements the native method 'mapLibraryName'.
        /// </summary>
        /// <param name="libname"></param>
        /// <returns></returns>
        public static string mapLibraryName(string libname)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (libname == null)
                throw new global::java.lang.NullPointerException();

            if (RuntimeUtil.IsWindows)
                return libname + ".dll";
            else if (RuntimeUtil.IsOSX)
                return "lib" + libname + ".dylib";
            else
                return "lib" + libname + ".so";
#endif
        }


    }

}
