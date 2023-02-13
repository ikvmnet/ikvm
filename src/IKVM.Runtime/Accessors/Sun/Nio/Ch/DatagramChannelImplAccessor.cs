namespace IKVM.Runtime.Accessors.Sun.Nio.Ch
{

    /// <summary>
    /// Provides runtime access to the 'sun.nio.ch.DatagramChannelImpl' type.
    /// </summary>
    internal sealed class DatagramChannelImplAccessor : Accessor
    {

        FieldAccessor<object> sender;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public DatagramChannelImplAccessor(AccessorTypeResolver resolver) :
            base(resolver("sun.nio.ch.DatagramChannelImpl"))
        {

        }

        /// <summary>
        /// Gets the value for the 'fd' field.
        /// </summary>
        public object GetSender(object self) => GetField(ref sender, nameof(sender)).GetValue(self);

        /// <summary>
        /// Sets the value for the 'fd' field.
        /// </summary>
        public void SetSender(object self, object value) => GetField(ref sender, nameof(sender)).SetValue(self, value);

    }

}
