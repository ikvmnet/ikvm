using System;

namespace IKVM.Runtime.Accessors.Java.Util
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.util.Iterator' type.
    /// </summary>
    internal sealed class IteratorAccessor : Accessor<object>
    {

        MethodAccessor<Func<object, bool>> hasNext;
        MethodAccessor<Func<object, object>> next;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public IteratorAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.util.Iterator")
        {

        }

        /// <summary>
        /// Invokes the 'hasNext' method.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public bool InvokeHasNext(object self) => GetMethod(ref hasNext, nameof(hasNext), typeof(bool)).Invoker(self);

        /// <summary>
        /// Invokes the 'next' method.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public object InvokeNext(object self) => GetMethod(ref next, nameof(next), typeof(object)).Invoker(self);

    }

#endif

}
