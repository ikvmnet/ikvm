using System;

namespace IKVM.Runtime.Accessors.Sun.Nio.Ch
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'sun.nio.ch.DatagramChannelImpl' type.
    /// </summary>
    internal sealed class DatagramChannelImplAccessor : Accessor<object>
    {

        Type javaNetSocketAddress;

        FieldAccessor<object, object> sender;
        MethodAccessor<Func<object, object>> remoteAddress;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public DatagramChannelImplAccessor(AccessorTypeResolver resolver) :
            base(resolver, "sun.nio.ch.DatagramChannelImpl")
        {

        }

        Type JavaNetSocketAddress => Resolve(ref javaNetSocketAddress, "java.net.SocketAddress");

        /// <summary>
        /// Gets the value for the 'sender' field.
        /// </summary>
        public object GetSender(object self) => GetField(ref sender, nameof(sender)).GetValue(self);

        /// <summary>
        /// Sets the value for the 'sender' field.
        /// </summary>
        public void SetSender(object self, object value) => GetField(ref sender, nameof(sender)).SetValue(self, value);

        /// <summary>
        /// Invokes the 'remoteAddress' function.
        /// </summary>
        /// <param name="self"></param>
        public object InvokeRemoteAddress(object self) => GetMethod(ref remoteAddress, nameof(remoteAddress), JavaNetSocketAddress).Invoker(self);

    }

#endif

}
