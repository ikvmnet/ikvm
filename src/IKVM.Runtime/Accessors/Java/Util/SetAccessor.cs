using System;

namespace IKVM.Runtime.Accessors.Java.Util
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.util.Set' type.
    /// </summary>
    internal sealed class SetAccessor : Accessor<object>
    {

        MethodAccessor<Func<object, object>> iterator;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public SetAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.util.Set")
        {

        }

        /// <summary>
        /// Invokes the 'iterator' method.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public object InvokeIterator(object self) => GetMethod(ref iterator, nameof(iterator), Resolve("java.util.Iterator")).Invoker(self);

    }

#endif

}
