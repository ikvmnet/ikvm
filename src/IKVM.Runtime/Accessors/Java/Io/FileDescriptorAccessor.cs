using System;
using System.IO;
using System.Net.Sockets;
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
        MethodAccessor<Func<Stream, object>> fromStream;
        MethodAccessor<Func<Socket, object>> fromSocket;

        MethodAccessor<Func<object>> init;
        PropertyAccessor<object, int> fd;
        FieldAccessor<object, long> handle;
        FieldAccessor<object, Task> task;
        FieldAccessor<object, Stream> stream;
        FieldAccessor<object, Socket> socket;
        MethodAccessor<Action<object>> sync;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public FileDescriptorAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.io.FileDescriptor")
        {

        }

        Type JavaIoFileDescriptor => Resolve(ref javaIoFileDescriptor, "java.io.FileDescriptor");

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
        /// Invokes the 'fromStream' static method.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public object FromStream(Stream stream) => GetMethod(ref fromStream, nameof(fromStream), JavaIoFileDescriptor, typeof(Stream)).Invoker(stream);

        /// <summary>
        /// Invokes the 'fromSocket' static method.
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public object FromSocket(Socket socket) => GetMethod(ref fromSocket, nameof(fromSocket), JavaIoFileDescriptor, typeof(Socket)).Invoker(socket);

        /// <summary>
        /// Gets the value for the 'fd' property.
        /// </summary>
        public int GetFd(object self) => GetProperty(ref fd, nameof(fd)).GetValue(self);

        /// <summary>
        /// Gets the value for the 'handle' property.
        /// </summary>
        public long GetHandle(object self) => GetField(ref handle, nameof(handle)).GetValue(self);

        /// <summary>
        /// Gets the value of the 'task' field.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public Task GetTask(object self) => GetField(ref task, nameof(task)).GetValue(self);

        /// <summary>
        /// Sets the value of the 'task' field.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetTask(object self, Task value) => GetField(ref task, nameof(task)).SetValue(self, value);

        /// <summary>
        /// Gets the value for the 'stream' property.
        /// </summary>
        public Stream GetStream(object self) => GetField(ref stream, nameof(stream)).GetValue(self);

        /// <summary>
        /// Sets the value for the 'stream' property.
        /// </summary>
        public void SetStream(object self, Stream value) => GetField(ref stream, nameof(stream)).SetValue(self, value);

        /// <summary>
        /// Gets the value for the 'socket' property.
        /// </summary>
        public Socket GetSocket(object self) => GetField(ref socket, nameof(socket)).GetValue(self);

        /// <summary>
        /// Sets the value for the 'socket' property.
        /// </summary>
        public void SetSocket(object self, Socket value) => GetField(ref socket, nameof(socket)).SetValue(self, value);

        /// <summary>
        /// Invokes the 'sync' method.
        /// </summary>
        /// <param name="self"></param>
        public void InvokeSync(object self) => GetMethod(ref sync, nameof(sync), typeof(void)).Invoker(self);

    }

#endif

}
