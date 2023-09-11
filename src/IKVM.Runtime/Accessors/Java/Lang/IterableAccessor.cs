using System;

namespace IKVM.Runtime.Accessors.Java.Lang
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.lang.Iterable' type.
    /// </summary>
    internal sealed class IterableAccessor : Accessor<object>
    {

        Type javaLangIterator;

        MethodAccessor<Func<object, object>> iterator;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public IterableAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.lang.Iterable")
        {

        }

        Type JavaLangIterator => Resolve(ref javaLangIterator, "java.util.Iterator");

        /// <summary>
        /// Invokes the 'iterator' method.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public object InvokeIterator(object self) => GetMethod(ref iterator, nameof(iterator), JavaLangIterator).Invoker(self);

    }

#endif

}
