using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using IKVM.Internal;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.Util.Java.Net;
using IKVM.Runtime.Util.Java.Nio;
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
using sun.security.util;

#endif

namespace IKVM.Java.Externs.sun.nio.ch
{

    /// <summary>
    /// Implements the native methods for 'DotNetAsynchronousSocketChannelImpl'.
    /// </summary>
    static class DotNetAsynchronousSocketChannelImpl
    {

#if FIRST_PASS == false

        static FileDescriptorAccessor fileDescriptorAccessor;
        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);

#endif

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
            (Task _, CancellationTokenSource cts) = ((Task, CancellationTokenSource))future.getContext();
            cts?.Cancel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        static void Close(global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl self)
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
            catch (System.Net.Sockets.SocketException e)
            {
                throw e.ToIOException();
            }
            catch (ObjectDisposedException)
            {
                // ignore
            }
            catch (System.Exception e)
            {
                throw new IOException(e);
            }
        }

        /// <summary>
        /// Implements the Connect logic as an asynchronous task.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="remote"></param>
        /// <param name="accessControlContext"></param>
        /// <returns></returns>
        static async Task ConnectAsync(global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl self, global::java.net.SocketAddress remote, global::java.security.AccessControlContext accessControlContext, CancellationToken cancellationToken)
        {
            if (self.isOpen() == false)
                throw new ClosedChannelException();

            // cancel was invoked during the operation
            if (cancellationToken.IsCancellationRequested)
                throw new IllegalStateException("Connect not allowed due to timeout or cancellation.");

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
                    catch (global::java.io.IOException)
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
                    var socket = FileDescriptorAccessor.GetSocket(self.fd);
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
            catch (System.Exception)
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

                throw;
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
        static unsafe ArraySegment<byte> PrepareArraySegment(ByteBuffer buf, bool copy = false)
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
        static unsafe void ReleaseArraySegment(ArraySegment<byte> seg, ByteBuffer buf, ref int length, bool copy = false)
        {
            int pos = buf.position();
            int len = buf.remaining();

            // number of bytes used in segment
            var rem = System.Math.Min(len, length);

            // original buffer is direct, meaning we need to copy to its address
            if (buf is DirectBuffer dir)
            {

                // optionally copy the segment back to the buffer data
                if (copy)
                    seg.AsSpan().CopyTo(new Span<byte>((byte*)(IntPtr)dir.address() + pos, rem));

                ArrayPool<byte>.Shared.Return(seg.Array);
            }

            // advance buffer by used bytes
            buf.position(pos + rem);
            length -= rem;
        }

        /// <summary>
        /// Implements the Accept logic as an asynchronous task.
        /// </summary>
        /// <remarks>
        /// This implementation differs based on whether we are doing a scattering read, whether there is a timeout specified and whether the buffers under consideration are direct.
        /// </remarks>
        /// <param name="self"></param>
        /// <param name="isScatteringRead"></param>
        /// <param name="dst"></param>
        /// <param name="dsts"></param>
        /// <param name="timeout"></param>
        /// <param name="unit"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        static async Task<Number> ReadAsync(global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl self, bool isScatteringRead, ByteBuffer dst, ByteBuffer[] dsts, long timeout, TimeUnit unit, AccessControlContext accessControlContext, CancellationToken cancellationToken)
        {
            try
            {
                var socket = FileDescriptorAccessor.GetSocket(self.fd);

                // we were told to do a scattering read, but only received one buffer, do a regular read
                if (isScatteringRead && dsts.Length == 1)
                    dst = dsts[0];

                // timeout was specified, wait for data to be available
                var t = (int)System.Math.Min(unit.convert(timeout, TimeUnit.MILLISECONDS), int.MaxValue);
                if (t > 0)
                {
                    // non-optimal usage, but we have no way to combine timeout with true async
                    return await Task.Run<Number>(() =>
                    {
                        var previousBlocking = socket.Blocking;
                        var previousReceiveTimeout = socket.ReceiveTimeout;

                        try
                        {
                            socket.Blocking = true;
                            socket.ReceiveTimeout = t;

                            if (dst != null)
                            {
                                int length = 0;

                                int pos = dst.position();
                                int lim = dst.limit();
                                int rem = pos <= lim ? lim - pos : 0;

                                if (dst is DirectBuffer dir)
                                {
#if NETFRAMEWORK
                                    var buf = ArrayPool<byte>.Shared.Rent(rem);

                                    try
                                    {
                                        length = socket.Receive(buf, rem, SocketFlags.None);
                                        Marshal.Copy(buf, 0, (IntPtr)dir.address() + pos, length);
                                    }
                                    finally
                                    {
                                        ArrayPool<byte>.Shared.Return(buf);
                                    }
#else
                                    unsafe
                                    {
                                        length = socket.Receive(new Span<byte>((byte*)(IntPtr)dir.address() + pos, rem), SocketFlags.None);
                                    }
#endif
                                }
                                else
                                {
                                    // synchronous read (inside of a Task) directly into the underlying array of the buffer
                                    length = socket.Receive(dst.array(), dst.arrayOffset() + pos, rem, SocketFlags.None);
                                }

                                dst.position(pos + length);
                                return isScatteringRead ? Long.valueOf(length) : Integer.valueOf(length);
                            }
                            else
                            {
                                int length = 0;
                                var bufs = new ArraySegment<byte>[dsts.Length];

                                try
                                {
                                    // prepare array segments for buffers
                                    for (int i = 0; i < dsts.Length; i++)
                                    {
                                        var dbf = dsts[i];
                                        int pos = dbf.position();
                                        int lim = dbf.limit();
                                        int rem = pos <= lim ? lim - pos : 0;

                                        if (dbf is DirectBuffer dir)
                                            bufs[i] = new ArraySegment<byte>(ArrayPool<byte>.Shared.Rent(rem), 0, rem);
                                        else
                                            bufs[i] = new ArraySegment<byte>(dbf.array(), dbf.arrayOffset() + pos, rem);
                                    }

                                    // receive data into segments
                                    length = socket.Receive(bufs, SocketFlags.None);

                                    var l = length;
                                    for (int i = 0; i < dsts.Length; i++)
                                    {
                                        var dbf = dsts[i];
                                        var buf = bufs[i];
                                        int pos = dbf.position();
                                        int lim = dbf.limit();
                                        int rem = pos <= lim ? lim - pos : 0;
                                        int len = System.Math.Min(l, rem);

                                        if (dbf is DirectBuffer dir)
                                            Marshal.Copy(buf.Array, 0, (IntPtr)dir.address() + pos, len);

                                        dbf.position(pos + len);
                                        l -= len;
                                    }

                                    // we should have accounted for all of the bytes
                                    if (l != 0)
                                        throw new RuntimeException("Bytes remaining after read.");

                                    // return total number of bytes read
                                    return isScatteringRead ? Long.valueOf(length) : Integer.valueOf(length);
                                }
                                finally
                                {
                                    for (int i = 0; i < dsts.Length; i++)
                                    {
                                        var dbf = dsts[i];
                                        var buf = bufs[i];

                                        if (dbf is DirectBuffer dir)
                                            ArrayPool<byte>.Shared.Return(buf.Array);
                                    }
                                }
                            }
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
                    var previousBlocking = socket.Blocking;
                    var previousReceiveTimeout = socket.ReceiveTimeout;

                    try
                    {
                        socket.Blocking = false;
                        socket.ReceiveTimeout = t;

                        if (dst != null)
                        {
                            int pos = dst.position();
                            int lim = dst.limit();
                            int rem = pos <= lim ? lim - pos : 0;

                            if (dst is DirectBuffer dir)
                            {
                                var buf = ArrayPool<byte>.Shared.Rent(rem);

                                try
                                {
                                    var length = await Task.Factory.FromAsync((cb, state) => socket.BeginReceive(buf, 0, rem, SocketFlags.None, cb, state), socket.EndReceive, null);
                                    Marshal.Copy(buf, 0, (IntPtr)dir.address() + pos, length);
                                    dst.position(pos + length);
                                    return isScatteringRead ? Long.valueOf(length) : Integer.valueOf(length);
                                }
                                finally
                                {
                                    ArrayPool<byte>.Shared.Return(buf);
                                }
                            }
                            else
                            {
                                var length = await Task.Factory.FromAsync((cb, state) => socket.BeginReceive(dst.array(), dst.arrayOffset() + pos, rem, SocketFlags.None, cb, state), socket.EndReceive, null);
                                dst.position(pos + length);
                                return isScatteringRead ? Long.valueOf(length) : Integer.valueOf(length);
                            }
                        }
                        else
                        {
                            int length = 0;
                            var bufs = new ArraySegment<byte>[dsts.Length];

                            try
                            {
                                // prepare array segments for buffers
                                for (int i = 0; i < dsts.Length; i++)
                                {
                                    var dbf = dsts[i];
                                    int pos = dbf.position();
                                    int lim = dbf.limit();
                                    int rem = pos <= lim ? lim - pos : 0;

                                    if (dbf is DirectBuffer dir)
                                        bufs[i] = new ArraySegment<byte>(ArrayPool<byte>.Shared.Rent(rem), 0, rem);
                                    else
                                        bufs[i] = new ArraySegment<byte>(dbf.array(), dbf.arrayOffset() + pos, rem);
                                }

                                // receive data into segments
                                length = await Task.Factory.FromAsync(socket.BeginReceive, socket.EndReceive, bufs, SocketFlags.None, null);

                                var l = length;
                                for (int i = 0; i < dsts.Length; i++)
                                {
                                    var dbf = dsts[i];
                                    var buf = bufs[i];
                                    int pos = dbf.position();
                                    int lim = dbf.limit();
                                    int rem = pos <= lim ? lim - pos : 0;
                                    int len = System.Math.Min(l, rem);

                                    if (dbf is DirectBuffer dir)
                                        Marshal.Copy(buf.Array, 0, (IntPtr)dir.address() + pos, len);

                                    dbf.position(pos + len);
                                    l -= len;
                                }

                                // we should have accounted for all of the bytes
                                if (l != 0)
                                    throw new RuntimeException("Bytes remaining after read.");

                                // return total number of bytes read
                                return isScatteringRead ? Long.valueOf(length) : Integer.valueOf(length);
                            }
                            finally
                            {
                                for (int i = 0; i < dsts.Length; i++)
                                {
                                    var dbf = dsts[i];
                                    var buf = bufs[i];

                                    if (dbf is DirectBuffer dir)
                                        ArrayPool<byte>.Shared.Return(buf.Array);
                                }
                            }

                        }
                    }
                    finally
                    {
                        socket.Blocking = previousBlocking;
                        socket.ReceiveTimeout = previousReceiveTimeout;
                    }
                }
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
                throw new global::java.io.IOException(e);
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
        /// <param name="gatheringWrite"></param>
        /// <param name="dst"></param>
        /// <param name="dsts"></param>
        /// <param name="timeout"></param>
        /// <param name="unit"></param>
        /// <param name="accessControlContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        static async Task<global::java.lang.Number> WriteAsync(global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl self, bool gatheringWrite, ByteBuffer dst, ByteBuffer[] dsts, long timeout, TimeUnit unit, AccessControlContext accessControlContext, CancellationToken cancellationToken)
        {
            // scattering read uses multiple buffers
            if (gatheringWrite == false)
                dsts = new ByteBuffer[] { dst };

            // replace buffers with array segments
            var bufs = new ArraySegment<byte>[dsts.Length];
            for (int i = 0; i < dsts.Length; i++)
                bufs[i] = PrepareArraySegment(dsts[i], true);

            try
            {
                var socket = FileDescriptorAccessor.GetSocket(self.fd);
                var length = 0;

                // timeout was specified, wait for data to be available
                var t = (int)System.Math.Min(unit.convert(timeout, TimeUnit.MILLISECONDS), int.MaxValue);
                if (t > 0)
                {
                    // non-optimal usage, but we have no way to combine timeout with true async
                    length = await Task.Run(() =>
                    {
                        var previousBlocking = socket.Blocking;
                        var previousSendTimeout = socket.SendTimeout;

                        try
                        {
                            socket.Blocking = true;
                            socket.SendTimeout = t;
                            return socket.Send(bufs, SocketFlags.None);
                        }
                        finally
                        {
                            socket.Blocking = previousBlocking;
                            socket.SendTimeout = previousSendTimeout;
                        }
                    });

                }
                else
                {
                    var previousBlocking = socket.Blocking;
                    var previousSendTimeout = socket.SendTimeout;

                    try
                    {
                        socket.Blocking = false;
                        socket.SendTimeout = t;
                        length = await Task.Factory.FromAsync(socket.BeginSend, socket.EndSend, bufs, SocketFlags.None, null);
                    }
                    finally
                    {
                        socket.Blocking = previousBlocking;
                        socket.SendTimeout = previousSendTimeout;
                    }
                }

                // advance original buffers based on amount written
                var l = length;
                for (int i = 0; i < bufs.Length; i++)
                    ReleaseArraySegment(bufs[i], dsts[i], ref l);

                if (l != 0)
                    throw new RuntimeException("Bytes remaining after write.");

                return gatheringWrite ? Long.valueOf(length) : Integer.valueOf(length);
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
            finally
            {
                self.enableWriting();
            }
        }

#endif

    }

}
