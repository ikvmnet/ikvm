namespace IKVM.Runtime.Accessors.Sun.Nio.Ch
{

    /// <summary>
    /// Provides runtime access to the 'sun.nio.ch.FileChannelImpl' type.
    /// </summary>
    internal sealed class FileChannelImplAccessor : Accessor
    {

        FieldAccessor<object> fd;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public FileChannelImplAccessor(AccessorTypeResolver resolver) :
            base(resolver("sun.nio.ch.FileChannelImpl"))
        {

        }

        /// <summary>
        /// Gets the value for the 'fd' field.
        /// </summary>
        public object GetFd(object self) => GetField(ref fd, nameof(fd)).GetValue(self);

        /// <summary>
        /// Sets the value for the 'fd' field.
        /// </summary>
        public void SetFd(object self, object value) => GetField(ref fd, nameof(fd)).SetValue(self, value);

    }

}
