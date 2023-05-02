using System;

namespace IKVM.Runtime.Accessors.Java.Io
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.io.FileInputStreamAccessor' type.
    /// </summary>
    internal sealed class FileInputStreamAccessor : Accessor<object>
    {

        MethodAccessor<Func<object, object>> init1;
        MethodAccessor<Func<object, object>> init2;
        FieldAccessor<object, object> fd;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public FileInputStreamAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.io.FileInputStream")
        {

        }

        /// <summary>
        /// Initializes a new instance of the object.
        /// </summary>
        /// <returns></returns>
        public object Init1(object fd) => GetConstructor(ref init1, Resolve("java.io.File")).Invoker(fd);

        /// <summary>
        /// Initializes a new instance of the object.
        /// </summary>
        /// <returns></returns>
        public object Init2(object fd) => GetConstructor(ref init2, Resolve("java.io.FileDescriptor")).Invoker(fd);

        /// <summary>
        /// Gets the value of the 'fd' field.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public object GetFd(object self) => GetField(ref fd, nameof(fd)).GetValue(self);

        /// <summary>
        /// Sets the value of the 'fd' field.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetFd(object self, object value) => GetField(ref fd, nameof(fd)).SetValue(self, value);

    }

#endif

}
