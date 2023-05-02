using System;

namespace IKVM.Runtime.Accessors.Java.Util
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.util.Map$Entry' type.
    /// </summary>
    internal sealed class MapEntryAccessor : Accessor<object>
    {

        MethodAccessor<Func<object, object>> getKey;
        MethodAccessor<Func<object, object>> getValue;

        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public MapEntryAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.util.Map+Entry")
        {

        }

        /// <summary>
        /// Invokes the 'getKey' method.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public object InvokeGetKey(object self) => GetMethod(ref getKey, nameof(getKey), typeof(object)).Invoker(self);

        /// <summary>
        /// Invokes the 'getValue' method.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public object InvokeGetValue(object self) => GetMethod(ref getValue, nameof(getValue), typeof(object)).Invoker(self);

    }

#endif

}
