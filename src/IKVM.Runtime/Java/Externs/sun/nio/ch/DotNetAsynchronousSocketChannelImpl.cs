using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using IKVM.Runtime.Util.Java.Net;
using IKVM.Runtime.Util.Java.Security;

#if FIRST_PASS == false

using java.io;
using java.lang;
using java.net;
using java.nio;
using java.nio.channels;
using java.security;
using java.util.concurrent;

using sun.nio.ch;

#endif

namespace IKVM.Java.Externs.sun.nio.ch
{

    static class DotNetAsynchronousSocketChannelImpl
    {

        /// <summary>
        /// Implements the native method for 'onCancel0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="task"></param>
        public static void onCancel0(object self, object task)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            OnCancel((global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl)self, (PendingFuture)task);
#endif
        }

        /// <summary>
        /// Implements the native method for 'implClose0'.
        /// </summary>
        /// <param name="self"></param>
        public static void implClose0(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            Close((global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl)self);
#endif
        }

        /// <summary>
        /// Implements the native method for 'implConnect0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="remote"></param>
        /// <param name="attachment"></param>
        /// <param name="handler"></param>
        public static object implConnect0(object self, object remote, object attachment, object handler)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return ((DotNetAsynchronousChannelGroup)((global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl)self).group()).Execute((global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl)self, (global::java.net.SocketAddress)remote, attachment, (CompletionHandler)handler, ConnectAsync);
#endif
        }

        /// <summary>
        /// Implements the native method for 'implRead0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="isScatteringRead"></param>
        /// <param name="dst"></param>
        /// <param name="dsts"></param>
        /// <param name="timeout"></param>
        /// <param name="unit"></param>
        /// <param name="attachment"></param>
        /// <param name="handler"></param>
        public static object implRead0(object self, bool isScatteringRead, object dst, object[] dsts, long timeout, object unit, object attachment, object handler)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return ((DotNetAsynchronousChannelGroup)((global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl)self).group()).Execute((global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl)self, isScatteringRead, (ByteBuffer)dst, (ByteBuffer[])dsts, timeout, (TimeUnit)unit, attachment, (CompletionHandler)handler, ReadAsync);
#endif
        }

        /// <summary>
        /// Implements the native method for 'implWrite0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="gatheringWrite"></param>
        /// <param name="src"></param>
        /// <param name="srcs"></param>
        /// <param name="timeout"></param>
        /// <param name="unit"></param>
        /// <param name="attachment"></param>
        /// <param name="handler"></param>
        public static object implWrite0(object self, bool gatheringWrite, object src, object[] srcs, long timeout, object unit, object attachment, object handler)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return ((DotNetAsynchronousChannelGroup)((global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl)self).group()).Execute((global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl)self, gatheringWrite, (ByteBuffer)src, (ByteBuffer[])srcs, timeout, (TimeUnit)unit, attachment, (CompletionHandler)handler, WriteAsync);
#endif
        }


#if FIRST_PASS == false

        /// <summary>
        /// Invoked when the pending future is cancelled.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="future"></param>
        /// <returns></returns>
        static void OnCancel(global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl self, PendingFuture future)
        {
            // signal cancellation on associated cancellation token source
            (_, CancellationTokenSource cts) = future.getContext();
            cts?.Cancel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        static void Close(global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl self)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implements the Connect logic as an asynchronous task.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="remote"></param>
        /// <param name="accessControlContext"></param>
        /// <returns></returns>
        /// <exception cref="ClosedChannelException"></exception>
        /// <exception cref="RuntimeException"></exception>
        /// <exception cref="NotYetBoundException"></exception>
        /// <exception cref="AcceptPendingException"></exception>
        /// <exception cref="InterruptedIOException"></exception>
        /// <exception cref="AsynchronousCloseException"></exception>
        /// <exception cref="IOException"></exception>
        static async Task ConnectAsync(global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl self, global::java.net.SocketAddress remote, global::java.security.AccessControlContext accessControlContext, CancellationToken cancellationToken)
        {
            if (self.isOpen() == false)
                throw new ClosedChannelException();

            // cancel was invoked during the operation
            if (cancellationToken.IsCancellationRequested)
                throw new RuntimeException("Connect not allowed due to cancellation.");

            // checks the validity of the socket address
            var remoteAddress = global::sun.nio.ch.Net.checkAddress(remote);
            if (accessControlContext != null)
                AccessController.doPrivileged(new ActionPrivilegedAction(() => global::java.lang.System.getSecurityManager()?.checkConnect(remoteAddress.getAddress().getHostAddress(), remoteAddress.getPort())), accessControlContext);

            lock (self.stateLock)
            {
                // ensure channel is not already connected
                if (self.state == AsynchronousSocketChannelImpl.ST_CONNECTED)
                    throw new AlreadyConnectedException();

                // ensure channel is not already in process of being connected
                if (self.state == AsynchronousSocketChannelImpl.ST_PENDING)
                    throw new ConnectionPendingException();

                // ensure channel is bound to a local address
                if (self.localAddress == null)
                {
                    try
                    {
                        var any = new InetSocketAddress(0);
                        if (accessControlContext == null)
                            self.bind(any);
                        else
                            AccessController.doPrivileged(new ActionPrivilegedExceptionAction(() => self.bind(any)), accessControlContext);
                    }
                    catch (IOException)
                    {
                        try
                        {
                            self.close();
                        }
                        catch
                        {
                            // ignore
                        }

                        self.state = AsynchronousSocketChannelImpl.ST_PENDING;
                        throw;
                    }
                }
            }

            try
            {
                try
                {
                    // execute asynchronous Connect task
                    var socket = self.fd.getSocket();
                    await Task.Factory.FromAsync(socket.BeginConnect, socket.EndConnect, remoteAddress.ToIPEndpoint(), null);

                    try
                    {
                        self.begin();

                        // set the channel to connected
                        lock (self.stateLock)
                        {
                            self.state = AsynchronousSocketChannelImpl.ST_CONNECTED;
                            self.remoteAddress = remoteAddress;
                        }

                        return;
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
                    if (self.isOpen())
                        self.close();
                }
                catch
                {
                    // ignore
                }
            }
        }

        /// <summary>
        /// Returns an <see cref="ArraySegment{byte}"/> for the given <see cref="ByteBuffer"/>. May be a memory
        /// mapping to the original array, or may be a newly allocated temporary buffer. Optionally, copy the
        /// original data to the temporary location.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="copy"></param>
        /// <returns></returns>
        static unsafe ArraySegment<byte> PrepareArray(ByteBuffer buf, bool copy = false)
        {
            int pos = buf.position();
            int lim = buf.limit();
            int rem = pos <= lim ? lim - pos : 0;

            if (buf is DirectBuffer dir)
            {
                var s = new ArraySegment<byte>(ArrayPool<byte>.Shared.Rent(rem), 0, rem);

                // optionally copy the original buffer data to the new segment
                if (copy)
                    new ReadOnlySpan<byte>((byte*)(IntPtr)dir.address() + pos, rem).CopyTo(s);

                return s;
            }
            else
            {
                return new ArraySegment<byte>(buf.array(), buf.arrayOffset(), rem);
            }
        }

        /// <summary>
        /// Accepts an array segment, and it's original buffer. If required copies the segment back into the buffer,
        /// subtracting the remaining bytes from the total.
        /// </summary>
        /// <param name="seg"></param>
        /// <param name="buf"></param>
        /// <param name="length"></param>
        /// <exception cref="NotImplementedException"></exception>
        static unsafe void ReleaseArraySegment(ArraySegment<byte> seg, ByteBuffer buf, ref int length)
        {
            int pos = buf.position();
            int len = buf.remaining();

            // number of bytes used in segment
            var rem = System.Math.Min(len, length);

            // original buffer is direct, meaning we need to copy to its address
            if (buf is DirectBuffer dir)
                seg.AsSpan().CopyTo(new Span<byte>((byte*)(IntPtr)dir.address() + pos, rem));

            // advance buffer by used bytes
            buf.position(pos + rem);
            length -= rem;
        }

        /// <summary>
        /// Implements the Accept logic as an asynchronous task.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="isScatteringRead"></param>
        /// <param name="dst"></param>
        /// <param name="dsts"></param>
        /// <param name="timeout"></param>
        /// <param name="unit"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ClosedChannelException"></exception>
        /// <exception cref="RuntimeException"></exception>
        /// <exception cref="NotYetBoundException"></exception>
        /// <exception cref="AcceptPendingException"></exception>
        /// <exception cref="InterruptedIOException"></exception>
        /// <exception cref="AsynchronousCloseException"></exception>
        /// <exception cref="IOException"></exception>
        static async Task<Number> ReadAsync(global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl self, bool isScatteringRead, ByteBuffer dst, ByteBuffer[] dsts, long timeout, TimeUnit unit, AccessControlContext accessControlContext, CancellationToken cancellationToken)
        {
            // set of buffers to receive into
            var bufs = new List<ArraySegment<byte>>();

            // scattering read uses multiple buffers
            if (isScatteringRead)
                dsts = new ByteBuffer[] { dst };

            // replace buffers with array segments, if direct
            for (int i = 0; i < dsts.Length; i++)
                bufs[i] = PrepareArraySegment(dsts[i]);

            try
            {
                var socket = self.fd.getSocket();
                var length = 0;

                // timeout was specified, wait for data to be available
                var t = (int)System.Math.Min(unit.convert(timeout, TimeUnit.MILLISECONDS), int.MaxValue);
                if (t > 0)
                {
                    // non-optimal usage, but we have no way to combine timeout with true async
                    length = await Task.Run(() =>
                    {
                        var previousBlocking = socket.Blocking;
                        var previousReceiveTimeout = socket.ReceiveTimeout;

                        try
                        {
                            socket.Blocking = true;
                            socket.ReceiveTimeout = t;
                            return socket.Receive(bufs, SocketFlags.None);
                        }
                        finally
                        {
                            socket.Blocking = previousBlocking;
                            socket.ReceiveTimeout = previousReceiveTimeout;
                        }
                    });

                }
                else
                {
                    socket.Blocking = false;
                    length = socket.EndReceive(socket.BeginReceive(bufs, SocketFlags.None, null, null), out var error);
                }

                // copy any rented buffers back to their original input
                var l = length;
                for (int i = 0; i < dsts.Length; i++)
                    ReleaseArraySegment(bufs[i], dsts[i], ref l);

                if (l != 0)
                    throw new RuntimeException("Bytes remaining after ");

                return isScatteringRead ? Long.valueOf(length) : Integer.valueOf(length);
            }
            catch (System.Net.Sockets.SocketException e) when (e.SocketErrorCode == SocketError.Interrupted)
            {
                throw new InterruptedIOException(e.Message);
            }
            catch (ClosedChannelException)
            {
                throw new AsynchronousCloseException();
            }
            catch (IOException)
            {
                throw;
            }
            catch (System.Exception e)
            {
                throw new IOException(e);
            }
            finally
            {
                self.enableReading();
            }
        }

        /// <summary>
        /// Implements the Accept logic as an asynchronous task.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="isScatteringRead"></param>
        /// <param name="dst"></param>
        /// <param name="dsts"></param>
        /// <param name="timeout"></param>
        /// <param name="unit"></param>
        /// <param name="accessControlContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ClosedChannelException"></exception>
        /// <exception cref="RuntimeException"></exception>
        /// <exception cref="NotYetBoundException"></exception>
        /// <exception cref="AcceptPendingException"></exception>
        /// <exception cref="InterruptedIOException"></exception>
        /// <exception cref="AsynchronousCloseException"></exception>
        /// <exception cref="IOException"></exception>
        static async Task<global::java.lang.Number> WriteAsync(global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl self, bool isScatteringRead, ByteBuffer dst, ByteBuffer[] dsts, long timeout, TimeUnit unit, AccessControlContext accessControlContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

#endif

    }

}
