using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace IKVM.Runtime.Accessors.Java.Io
{

    /// <summary>
    /// Provides runtime access to the 'java.io.FileDescriptor' type.
    /// </summary>
    internal sealed class FileDescriptorAccessor : Accessor
    {

        StaticFieldAccessor<object> @in;
        StaticFieldAccessor<object> @out;
        StaticFieldAccessor<object> @err;
        StaticMethodAccessor<Func<Stream, object>> fromStream;
        StaticMethodAccessor<Func<Socket, object>> fromSocket;
        PropertyAccessor<int> fd;
        PropertyAccessor<long> handle;
        FieldAccessor<Task> task;
        FieldAccessor<Stream> stream;
        FieldAccessor<Socket> socket;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public FileDescriptorAccessor(AccessorTypeResolver resolver) :
            base(resolver("java.io.FileDescriptor"))
        {

        }

        /// <summary>
        /// Gets the value for the 'in' field.
        /// </summary>
        public object GetIn() => GetStaticField(ref @in, nameof(@in)).GetValue();

        /// <summary>
        /// Gets the value for the 'out' field.
        /// </summary>
        public object GetOut() => GetStaticField(ref @out, nameof(@out)).GetValue();

        /// <summary>
        /// Gets the value for the 'err' field.
        /// </summary>
        public object GetErr() => GetStaticField(ref @err, nameof(@err)).GetValue();

        /// <summary>
        /// Invokes the 'fromStream' static method.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public object FromStream(Stream stream) => GetStaticMethod(ref fromStream, nameof(fromStream)).Invoker(stream);

        /// <summary>
        /// Invokes the 'fromSocket' static method.
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public object FromSocket(Socket socket) => GetStaticMethod(ref fromSocket, nameof(fromSocket)).Invoker(socket);

        /// <summary>
        /// Gets the value for the 'fd' property.
        /// </summary>
        public int GetFd(object self) => GetProperty(ref fd, nameof(fd)).GetValue(self);

        /// <summary>
        /// Gets the value for the 'handle' property.
        /// </summary>
        public long GetHandle(object self) => GetProperty(ref handle, nameof(handle)).GetValue(self);

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

    }

}
