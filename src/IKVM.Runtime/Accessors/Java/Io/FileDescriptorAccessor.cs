using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace IKVM.Runtime.Accessors.Java.Io
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.io.FileDescriptor' type.
    /// </summary>
    internal sealed class FileDescriptorAccessor : Accessor<object>
    {

        Type javaIoFileDescriptor;

        FieldAccessor<object> @in;
        FieldAccessor<object> @out;
        FieldAccessor<object> @err;

        FieldAccessor<object, Stream> stream;
        FieldAccessor<object, long> ptr;
        PropertyAccessor<object, int> fd;
        PropertyAccessor<object, long> handle;
        MethodAccessor<Func<object>> init;
        MethodAccessor<Action<object>> sync;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public FileDescriptorAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.io.FileDescriptor")
        {

        }

        /// <summary>
        /// Invokes the constructor.
        /// </summary>
        /// <returns></returns>
        public object Init() => GetConstructor(ref init).Invoker();

        /// <summary>
        /// Gets the value for the 'in' field.
        /// </summary>
        public object GetIn() => GetField(ref @in, nameof(@in)).GetValue();

        /// <summary>
        /// Gets the value for the 'out' field.
        /// </summary>
        public object GetOut() => GetField(ref @out, nameof(@out)).GetValue();

        /// <summary>
        /// Gets the value for the 'err' field.
        /// </summary>
        public object GetErr() => GetField(ref @err, nameof(@err)).GetValue();

        /// <summary>
        /// Gets the value for the 'obj' field.
        /// </summary>
        public Stream GetStream(object self) => GetField(ref stream, nameof(stream)).GetValue(self);

        /// <summary>
        /// Sets the value for the 'obj' field.
        /// </summary>
        public void SetStream(object self, Stream value) => GetField(ref stream, nameof(stream)).SetValue(self, value);

        /// <summary>
        /// Gets the value for the 'ptr' field.
        /// </summary>
        public long GetPtr(object self) => GetField(ref ptr, nameof(ptr)).GetValue(self);

        /// <summary>
        /// Sets the value for the 'ptr' field.
        /// </summary>
        public void SetPtr(object self, long value) => GetField(ref ptr, nameof(ptr)).SetValue(self, value);

        /// <summary>
        /// Gets the value for the 'fd' property.
        /// </summary>
        public int GetFd(object self) => GetProperty(ref fd, nameof(fd)).GetValue(self);

        /// <summary>
        /// Sets the value for the 'fd' property.
        /// </summary>
        public void SetFd(object self, int value) => GetProperty(ref fd, nameof(fd)).SetValue(self, value);

        /// <summary>
        /// Gets the value for the 'handle' property.
        /// </summary>
        public long GetHandle(object self) => GetProperty(ref handle, nameof(handle)).GetValue(self);

        /// <summary>
        /// Sets the value for the 'handle' property.
        /// </summary>
        public void SetHandle(object self, long value) => GetProperty(ref handle, nameof(handle)).SetValue(self, value);

        /// <summary>
        /// Invokes the 'sync' method.
        /// </summary>
        /// <param name="self"></param>
        public void InvokeSync(object self) => GetMethod(ref sync, nameof(sync), typeof(void)).Invoker(self);

    }

#endif

}
