using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace IKVM.Runtime.Accessors.Java.Io
{

    /// <summary>
    /// Provides runtime access to the 'java.io.FileDescriptor' type.
    /// </summary>
    internal sealed class FileDescriptorAccessor : Accessor<object>
    {

        FieldAccessor<object> @in;
        FieldAccessor<object> @out;
        FieldAccessor<object> @err;
        MethodAccessor<Func<Stream, object>> fromStream;
        MethodAccessor<Func<Socket, object>> fromSocket;

        FieldAccessor<object, int> fd;
        FieldAccessor<object, long> handle;
        FieldAccessor<object, Task> task;
        FieldAccessor<object, Stream> stream;
        FieldAccessor<object, Socket> socket;

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
        public object GetIn() => GetField(ref @in, nameof(@in), "Ljava.io.FileDescriptor;").GetValue();

        /// <summary>
        /// Gets the value for the 'out' field.
        /// </summary>
        public object GetOut() => GetField(ref @out, nameof(@out), "Ljava.io.FileDescriptor;").GetValue();

        /// <summary>
        /// Gets the value for the 'err' field.
        /// </summary>
        public object GetErr() => GetField(ref @err, nameof(@err), "Ljava.io.FileDescriptor;").GetValue();

        /// <summary>
        /// Invokes the 'fromStream' static method.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public object FromStream(Stream stream) => GetMethod(ref fromStream, nameof(fromStream), "(Lcli.System.IO.Stream;)Ljava.io.FileDescriptor;").Invoker(stream);

        /// <summary>
        /// Invokes the 'fromSocket' static method.
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public object FromSocket(Socket socket) => GetMethod(ref fromSocket, nameof(fromSocket), "(Lcli.System.Net.Sockets.Socket;)Ljava.io.FileDescriptor;").Invoker(socket);

        /// <summary>
        /// Gets the value for the 'fd' property.
        /// </summary>
        public int GetFd(object self) => GetField(ref fd, nameof(fd), "I").GetValue(self);

        /// <summary>
        /// Gets the value for the 'handle' property.
        /// </summary>
        public long GetHandle(object self) => GetField(ref handle, nameof(handle), "J").GetValue(self);

        /// <summary>
        /// Gets the value of the 'task' field.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public Task GetTask(object self) => GetField(ref task, nameof(task), "Lcli.System.Threading.Tasks.Task;").GetValue(self);

        /// <summary>
        /// Sets the value of the 'task' field.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetTask(object self, Task value) => GetField(ref task, nameof(task), "Lcli.System.Threading.Tasks.Task;").SetValue(self, value);

        /// <summary>
        /// Gets the value for the 'stream' property.
        /// </summary>
        public Stream GetStream(object self) => GetField(ref stream, nameof(stream), "Lcli.System.IO.Stream;").GetValue(self);

        /// <summary>
        /// Sets the value for the 'stream' property.
        /// </summary>
        public void SetStream(object self, Stream value) => GetField(ref stream, nameof(stream), "Lcli.System.IO.Stream;").SetValue(self, value);

        /// <summary>
        /// Gets the value for the 'socket' property.
        /// </summary>
        public Socket GetSocket(object self) => GetField(ref socket, nameof(socket), "Lcli.System.Net.Sockets.Socket;").GetValue(self);

        /// <summary>
        /// Sets the value for the 'socket' property.
        /// </summary>
        public void SetSocket(object self, Socket value) => GetField(ref socket, nameof(socket), "Lcli.System.Net.Sockets.Socket;").SetValue(self, value);

    }

}
