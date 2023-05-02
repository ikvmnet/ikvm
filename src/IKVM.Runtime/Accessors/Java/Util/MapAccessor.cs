using System;

namespace IKVM.Runtime.Accessors.Java.Util
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.util.Map' type.
    /// </summary>
    internal sealed class MapAccessor : Accessor<object>
    {

        MethodAccessor<Func<object, object>> entrySet;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public MapAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.util.Map")
        {

        }

        /// <summary>
        /// Invokes the 'entrySet' method.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public object InvokeEntrySet(object self) => GetMethod(ref entrySet, nameof(entrySet), Resolve("java.util.Set")).Invoker(self);

    }

#endif

}
