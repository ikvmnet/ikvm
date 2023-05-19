using System;

namespace IKVM.Runtime.Accessors.Ikvm.Internal
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'ikvm.internal.CallerID' type.
    /// </summary>
    internal sealed class CallerIDAccessor : Accessor<object>
    {

        MethodAccessor<Func<RuntimeTypeHandle, object>> create;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public CallerIDAccessor(AccessorTypeResolver resolver) :
            base(resolver, "ikvm.internal.CallerID")
        {

        }

        /// <summary>
        /// Invokes the 'create' method.
        /// </summary>
        /// <param name="typeHandle"></param>
        public object InvokeCreate(RuntimeTypeHandle typeHandle) => GetMethod(ref create, "create", Resolve("ikvm.internal.CallerID"), typeof(RuntimeTypeHandle)).Invoker(typeHandle);

    }

#endif

}
