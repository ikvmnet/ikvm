namespace IKVM.Runtime.Accessors.Java.Io
{

    /// <summary>
    /// Provides runtime access to the 'java.io.FileDescriptor' type.
    /// </summary>
    internal sealed class FileDescriptorAccessor : Accessor
    {

        PropertyAccessor<int> fd;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public FileDescriptorAccessor(AccessorTypeResolver resolver) :
            base(resolver("java.io.FileDescriptor"))
        {

        }

        /// <summary>
        /// Gets the value for the 'fd' property.
        /// </summary>
        public int GetFd(object self) => GetProperty(ref fd, nameof(fd)).GetValue(self);

    }

}
