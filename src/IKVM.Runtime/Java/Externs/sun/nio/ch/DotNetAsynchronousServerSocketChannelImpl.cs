using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

using IKVM.Runtime.Util.Java.Security;
using IKVM.Runtime.Util.Java.Net;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Internal;

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
        /// <returns></returns>
        /// <exception cref="ClosedChannelException"></exception>
        /// <exception cref="RuntimeException"></exception>
        /// <exception cref="NotYetBoundException"></exception>
        /// <exception cref="AcceptPendingException"></exception>
        /// <exception cref="InterruptedIOException"></exception>
        /// <exception cref="AsynchronousCloseException"></exception>
        /// <exception cref="IOException"></exception>
        static async Task<global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl> AcceptAsync(global::sun.nio.ch.DotNetAsynchronousServerSocketChannelImpl self, global::java.security.AccessControlContext accessControlContext, CancellationToken cancellationToken)
        {
            if (self.isOpen() == false)
                throw new ClosedChannelException();

            if (self.isAcceptKilled())
                throw new RuntimeException("Accept not allowed due to cancellation.");

            if (self.localAddress == null)
                throw new NotYetBoundException();

            var client = CreateClientChannel(self);
            if (client == null)
                throw new RuntimeException("Could not create channel.");

            if (self.accepting.compareAndSet(false, true) == false)
                throw new AcceptPendingException();

            try
            {
                try
                {
                    // cancel was invoked during the operation
                    if (self.isAcceptKilled())
                        throw new RuntimeException("Accept not allowed due to cancellation.");

                    // execute asynchronous Accept task
                    var listenSocket = FileDescriptorAccessor.GetSocket(self.fd);
                    var clientSocket = await Task.Factory.FromAsync(listenSocket.BeginAccept, listenSocket.EndAccept, client.fd.getSocket(), 0, null);

                    // cancel was invoked during the operation
                    if (self.isAcceptKilled())
                        throw new RuntimeException("Accept not allowed due to cancellation.");

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

                            // set the client channel to connected
                            lock (client.stateLock)
                            {
                                client.state = AsynchronousSocketChannelImpl.ST_CONNECTED;
                                client.localAddress = ((IPEndPoint)clientSocket.LocalEndPoint).ToInetSocketAddress();
                                client.remoteAddress = ((IPEndPoint)clientSocket.RemoteEndPoint).ToInetSocketAddress();
                            }

                            // check access to specified host
                            if (accessControlContext != null)
                            {
                                global::java.security.AccessController.doPrivileged(new ActionPrivilegedAction(() =>
                                {
                                    var ep = (IPEndPoint)client.fd.getSocket().RemoteEndPoint;
                                    global::java.lang.System.getSecurityManager()?.checkAccept(ep.Address.ToString(), ep.Port);
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
                catch (System.Net.Sockets.SocketException e) when (e.SocketErrorCode == SocketError.Interrupted)
                {
                    throw new InterruptedIOException(e.Message);
                }
                catch (ClosedChannelException)
                {
                    throw new AsynchronousCloseException();
                }
                catch (SecurityException)
                {
                    throw;
                }
                catch (IOException)
                {
                    throw;
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

            // if we're not configured to linger, disable sending, but continue to allow receive
            if (socket.LingerState.Enabled == false)
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Send);
                }
                catch (SocketException)
                {
                    // ignore
                }
            }

            // null socket before close, as close may take a minute to flush
            FileDescriptorAccessor.SetSocket(self.fd, null);
            socket.Close();
        }

#endif

    }

}