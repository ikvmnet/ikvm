﻿namespace IKVM.Runtime.Accessors.Java.Io
{

    /// <summary>
    /// Provides runtime access to the 'java.io.FileOutputStream' type.
    /// </summary>
    internal sealed class FileOutputStreamAccessor : Accessor<object>
    {

        FieldAccessor<object, object> fd;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public FileOutputStreamAccessor(AccessorTypeResolver resolver) :
            base(resolver("java.io.FileOutputStream"))
        {

        }

        /// <summary>
        /// Gets the value of the 'fd' field.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public object GetFd(object self) => GetField(ref fd, nameof(fd), "Ljava.io.FileDescriptor;").GetValue(self);

        /// <summary>
        /// Sets the value of the 'fd' field.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetFd(object self, object value) => GetField(ref fd, nameof(fd), "Ljava.io.FileDescriptor;").SetValue(self, value);

    }

}