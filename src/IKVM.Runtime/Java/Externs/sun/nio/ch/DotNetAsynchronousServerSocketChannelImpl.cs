using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

using IKVM.Runtime;
using IKVM.Runtime.Util.Java.Security;
using IKVM.Runtime.Util.Java.Net;
using IKVM.Runtime.Accessors.Java.Io;

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

        static FileDescriptorAccessor fileDescriptorAccessor;
        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);

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
        /// <param name="self"></param>
        /// <returns></returns>
        static global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl CreateClientChannel(global::sun.nio.ch.DotNetAsynchronousServerSocketChannelImpl self)
        {
            try
            {
                self.begin();
                return new global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl((DotNetAsynchronousChannelGroup)self.group(), false);
            }
            finally
            {
                self.end();
            }
        }

        /// <summary>
        /// Implements the Accept logic as an asynchronous task.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>s
        static Task<global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl> AcceptAsync(global::sun.nio.ch.DotNetAsynchronousServerSocketChannelImpl self, global::java.security.AccessControlContext accessControlContext, CancellationToken cancellationToken)
        {
            if (self.isOpen() == false)
                return Task.FromException<global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl>(new ClosedChannelException());

            if (self.isAcceptKilled())
                throw new RuntimeException("Accept not allowed due to cancellation.");

            if (self.localAddress == null)
                throw new NotYetBoundException();

            global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl client = null;

            try
            {
                client = CreateClientChannel(self) ?? throw new RuntimeException("Could not create channel.");
            }
            catch (System.Exception e)
            {
                return Task.FromException<global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl>(e);
            }

            if (self.accepting.compareAndSet(false, true) == false)
                throw new AcceptPendingException();

            return ImplAsync();

            async Task<global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl> ImplAsync()
            {
                try
                {
                    try
                    {
                        // get initial clientSocket
                        var acceptSocket = FileDescriptorAccessor.GetSocket(client.fd);

                        // execute asynchronous Accept task
                        var listenSocket = FileDescriptorAccessor.GetSocket(self.fd);
#if NETFRAMEWORK
                        acceptSocket = await Task.Factory.FromAsync(listenSocket.BeginAccept, listenSocket.EndAccept, acceptSocket, 0, null);
#else
                        acceptSocket = await listenSocket.AcceptAsync(acceptSocket);
#endif

                        // connection accept completed after group has shutdown
                        if (self.group().isShutdown())
                            throw new ShutdownChannelGroupException();

                        try
                        {
                            self.begin();

                            try
                            {
                                // initialize resulting channel
                                client.begin();

                                // obtain new addresses
                                var local = ((IPEndPoint)acceptSocket.LocalEndPoint).ToInetSocketAddress();
                                var remote = ((IPEndPoint)acceptSocket.RemoteEndPoint).ToInetSocketAddress();

                                // set the client channel to connected
                                lock (client.stateLock)
                                {
                                    client.state = AsynchronousSocketChannelImpl.ST_CONNECTED;
                                    client.localAddress = local;
                                    client.remoteAddress = remote;
                                }

                                // check access to specified host
                                if (accessControlContext != null)
                                {
                                    global::java.security.AccessController.doPrivileged(new ActionPrivilegedAction(() =>
                                    {
                                        global::java.lang.System.getSecurityManager()?.checkAccept(remote.getAddress().getHostAddress(), remote.getPort());
                                    }), accessControlContext);
                                }

                                // return resulting channel
                                return client;
                            }
                            finally
                            {
                                client.end();
                            }
                        }
                        finally
                        {
                            self.end();
                        }
                    }
                    catch (SecurityException)
                    {
                        throw;
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
                catch
                {
                    try
                    {
                        client?.close();
                    }
                    catch
                    {
                        // ignore
                    }

                    throw;
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

            var socket = FileDescriptorAccessor.GetSocket(self.fd);
            if (socket == null)
                return;

            try
            {
                // null socket before close, as close may take a minute to flush
                FileDescriptorAccessor.SetSocket(self.fd, null);
                socket.Close();
            }
            catch (SocketException)
            {
                // ignore
            }
        }

#endif

    }

}
