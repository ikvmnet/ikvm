using System.Threading;

using IKVM.Runtime.Accessors.Java.Lang;

namespace IKVM.Runtime
{

    /// <summary>
    /// Provides a static cache of java.lang.Class instances per .NET type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ClassLiteral<T>
    {

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false

        static ClassAccessor classAccessor;
        static object value;

        static ClassAccessor ClassAccessor => JVM.BaseAccessors.Get(ref classAccessor);

        /// <summary>
        /// Gets the class literal value, caching the result.
        /// </summary>
        public static object Value => GetValue();

        /// <summary>
        /// Gets the class literal value, caching the result.
        /// </summary>
        /// <returns></returns>
        static object GetValue()
        {
            if (value == null)
                Interlocked.CompareExchange(ref value, ClassAccessor.Init(typeof(T)), null);

            return value;
        }

#else

        public static object Value => null;

#endif

    }

}
