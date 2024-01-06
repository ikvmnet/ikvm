using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

using IKVM.Runtime;
using IKVM.Runtime.Util.Java.Security;
using IKVM.Runtime.Util.Java.Net;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.Accessors.Sun.Nio.Ch;
using IKVM.Runtime.Accessors.Java.Lang;

#if FIRST_PASS == false

using java.lang;
using java.io;
using java.nio.channels;
using java.util.concurrent;

using sun.nio.ch;

#endif

namespace IKVM.Java.Externs.sun.nio.ch
{

    /// <summary>
    /// Implements the native methods for 'DotNetAsynchronousServerSocketChannelImpl'.
    /// </summary>
    static class DotNetAsynchronousServerSocketChannelImpl
    {

#if FIRST_PASS == false

        static SystemAccessor systemAccessor;
        static SecurityManagerAccessor securityManagerAccessor;
        static AccessControllerAccessor accessControllerAccessor;
        static FileDescriptorAccessor fileDescriptorAccessor;
        static DotNetAsynchronousServerSocketChannelImplAccessor dotNetAsynchronousServerSocketChannelImplAccessor;
        static DotNetAsynchronousSocketChannelImplAccessor dotNetAsynchronousSocketChannelImplAccessor;

        static SystemAccessor SystemAccessor => JVM.Internal.BaseAccessors.Get(ref systemAccessor);

        static SecurityManagerAccessor SecurityManagerAccessor => JVM.Internal.BaseAccessors.Get(ref securityManagerAccessor);

        static AccessControllerAccessor AccessControllerAccessor => JVM.Internal.BaseAccessors.Get(ref accessControllerAccessor);

        static FileDescriptorAccessor FileDescriptorAccessor => JVM.Internal.BaseAccessors.Get(ref fileDescriptorAccessor);

        static DotNetAsynchronousServerSocketChannelImplAccessor DotNetAsynchronousServerSocketChannelImplAccessor => JVM.Internal.BaseAccessors.Get(ref dotNetAsynchronousServerSocketChannelImplAccessor);

        static DotNetAsynchronousSocketChannelImplAccessor DotNetAsynchronousSocketChannelImplAccessor => JVM.Internal.BaseAccessors.Get(ref dotNetAsynchronousSocketChannelImplAccessor);

#endif

        /// <summary>
        /// Implements the native method for 'implAccept0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="attachment"></param>
        /// <param name="handler"></param>
        public static object implAccept0(object self, object attachment, object handler)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return Accept((global::sun.nio.ch.DotNetAsynchronousServerSocketChannelImpl)self, attachment, (CompletionHandler)handler);
#endif
        }

        /// <summary>
        /// Implements the native methodf or 'implClose0'.
        /// </summary>
        /// <param name="self"></param>
        public static void implClose0(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            Close((global::sun.nio.ch.DotNetAsynchronousServerSocketChannelImpl)self);
#endif
        }

#if FIRST_PASS == false

        /// <summary>
        /// Implements the Accept logic as an asynchronous task converted to a Future.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="attachment"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        static Future Accept(global::sun.nio.ch.DotNetAsynchronousServerSocketChannelImpl self, object attachment, CompletionHandler handler)
        {
            return ((DotNetAsynchronousChannelGroup)self.group()).Execute(self, attachment, handler, AcceptAsync);
        }

        /// <summary>
        /// Creates a new client channel.
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        static object CreateClientChannel(object server, object fd, object remote)
        {
            try
            {
                DotNetAsynchronousServerSocketChannelImplAccessor.InvokeBegin(server);
                return DotNetAsynchronousSocketChannelImplAccessor.Init(DotNetAsynchronousServerSocketChannelImplAccessor.InvokeGroup(server), fd, remote);
            }
            finally
            {
                DotNetAsynchronousServerSocketChannelImplAccessor.InvokeEnd(server);
            }
        }

        /// <summary>
        /// Implements the Accept logic as an asynchronous task.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>s
        static Task<object> AcceptAsync(global::sun.nio.ch.DotNetAsynchronousServerSocketChannelImpl self, object accessControlContext, CancellationToken cancellationToken)
        {
            if (self.isOpen() == false)
                return Task.FromException<object>(new ClosedChannelException());

            if (self.isAcceptKilled())
                throw new RuntimeException("Accept not allowed due to cancellation.");

            if (self.localAddress == null)
                throw new NotYetBoundException();

            if (self.accepting.compareAndSet(false, true) == false)
                throw new AcceptPendingException();

            return ImplAsync();

            async Task<object> ImplAsync()
            {
                try
                {
                    try
                    {
                        // execute asynchronous Accept task
                        var listenSocket = FileDescriptorAccessor.GetSocket(self.fd);
#if NETFRAMEWORK
                        var acceptSocket = await Task.Factory.FromAsync(listenSocket.BeginAccept, listenSocket.EndAccept, 0, null);
#else
                        var acceptSocket = await listenSocket.AcceptAsync();
#endif

                        // connection accept completed after group has shutdown
                        if (self.group().isShutdown())
                            throw new ShutdownChannelGroupException();

                        try
                        {
                            DotNetAsynchronousServerSocketChannelImplAccessor.InvokeBegin(self);

                            // obtain new addresses
                            var local = ((IPEndPoint)acceptSocket.LocalEndPoint).ToInetSocketAddress();
                            var remote = ((IPEndPoint)acceptSocket.RemoteEndPoint).ToInetSocketAddress();
                            var fdo = FileDescriptorAccessor.Init();
                            FileDescriptorAccessor.SetSocket(fdo, acceptSocket);
                            var client = CreateClientChannel(self, fdo, remote);

                            // check access to specified host
                            if (accessControlContext != null)
                                AccessControllerAccessor.InvokeDoPrivileged(new ActionPrivilegedAction(() => SecurityManagerAccessor.InvokeCheckAccept(SystemAccessor.InvokeGetSecurityManager(), (string)remote.getAddress().getHostAddress(), (int)remote.getPort())), accessControlContext);

                            // return resulting channel
                            return client;
                        }
                        finally
                        {
                            DotNetAsynchronousServerSocketChannelImplAccessor.InvokeEnd(self);
                        }
                    }
                    catch (SecurityException)
                    {
                        throw;
                    }
                    catch (System.Net.Sockets.SocketException) when (self.isOpen() == false)
                    {
                        throw new AsynchronousCloseException();
                    }
                    catch (System.Net.Sockets.SocketException e)
                    {
                        throw e.ToIOException();
                    }
                    catch (ObjectDisposedException)
                    {
                        throw new AsynchronousCloseException();
                    }
                    catch (System.Exception e)
                    {
                        throw new IOException(e);
                    }
                }
                finally
                {
                    self.accepting.set(false);
                }
            }
        }

        /// <summary>
        /// Closes the socket associaed with the server socket channel.
        /// </summary>
        /// <param name="self"></param>
        static void Close(global::sun.nio.ch.DotNetAsynchronousServerSocketChannelImpl self)
        {
            if (self.fd == null)
                return;

            try
            {
                var h = FileDescriptorAccessor.GetHandle(self.fd);
                FileDescriptorAccessor.SetHandle(self.fd, -1);
                LibIkvm.Instance.io_close_socket(h);
            }
            catch
            {
                // ignore
            }
        }

#endif

    }

}
