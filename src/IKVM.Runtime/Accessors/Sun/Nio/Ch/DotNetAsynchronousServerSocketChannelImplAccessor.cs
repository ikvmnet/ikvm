using System;

namespace IKVM.Runtime.Accessors.Sun.Nio.Ch
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'sun.nio.ch.DotNetAsynchronousServerSocketChannelImpl' type.
    /// </summary>
    internal sealed class DotNetAsynchronousServerSocketChannelImplAccessor : Accessor<object>
    {

        Type sunNioChAsynchronousChannelGroupImpl;

        MethodAccessor<Action<object>> begin;
        MethodAccessor<Action<object>> end;
        MethodAccessor<Func<object, object>> group;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public DotNetAsynchronousServerSocketChannelImplAccessor(AccessorTypeResolver resolver) :
            base(resolver, "sun.nio.ch.DotNetAsynchronousServerSocketChannelImpl")
        {

        }

        Type SunNioChAsynchronousChannelGroupImpl => Resolve(ref sunNioChAsynchronousChannelGroupImpl, "sun.nio.ch.AsynchronousChannelGroupImpl");

        public void InvokeBegin(object self) => GetMethod(ref begin, nameof(begin), typeof(void)).Invoker(self);

        public void InvokeEnd(object self) => GetMethod(ref end, nameof(end), typeof(void)).Invoker(self);

        public object InvokeGroup(object self) => GetMethod(ref group, nameof(group), SunNioChAsynchronousChannelGroupImpl).Invoker(self);

    }

#endif

}
