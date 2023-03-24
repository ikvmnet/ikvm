using System;
using System.Buffers;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using IKVM.Runtime;
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
            var cts = (CancellationTokenSource)future.getContext();
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
                throw new global::java.io.IOException(e);
            }
        }

        /// <summary>
        /// Implements the Connect logic as an asynchronous task.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="remote"></param>
        /// <param name="accessControlContext"></param>
        /// <returns></returns>
        static Task ConnectAsync(global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl self, global::java.net.SocketAddress remote, global::java.security.AccessControlContext accessControlContext, CancellationToken cancellationToken)
        {
            if (self.isOpen() == false)
                return Task.FromException(new ClosedChannelException());

            var socket = FileDescriptorAccessor.GetSocket(self.fd);
            if (socket == null)
                return Task.FromException(new global::java.nio.channels.ClosedChannelException());

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
                    catch (global::java.io.IOException e)
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
                        return Task.FromException(e);
                    }
                }
            }

            return ImplAsync();

            async Task ImplAsync()
            {
                try
                {
                    try
                    {
                        // execute asynchronous Connect task
#if NETFRAMEWORK
                        await Task.Factory.FromAsync(socket.BeginConnect, socket.EndConnect, remoteAddress.ToIPEndpoint(), null);
#else
                        await socket.ConnectAsync(remoteAddress.ToIPEndpoint());
#endif

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
                        throw new global::java.io.IOException(e);
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
        static Task<Number> ReadAsync(global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl self, bool isScatteringRead, ByteBuffer dst, ByteBuffer[] dsts, long timeout, TimeUnit unit, AccessControlContext accessControlContext, CancellationToken cancellationToken)
        {
            var socket = FileDescriptorAccessor.GetSocket(self.fd);
            if (socket == null)
                throw new global::java.io.IOException("Socket is closed.");

            // we were told to do a scattering read, but only received one buffer, do a regular read
            if (isScatteringRead && dsts.Length == 1)
                dst = dsts[0];

            return ImplAsync();

            async Task<Number> ImplAsync()
            {
                try
                {
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

                                        // copy segments back into buffers
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
                                            throw new global::java.lang.RuntimeException("Bytes remaining after read.");

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
                            socket.ReceiveTimeout = 0;

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
                                        length = await Task.Factory.FromAsync((cb, state) => socket.BeginReceive(buf, 0, rem, SocketFlags.None, cb, state), socket.EndReceive, null);
                                        Marshal.Copy(buf, 0, (IntPtr)dir.address() + pos, length);
                                    }
                                    finally
                                    {
                                        ArrayPool<byte>.Shared.Return(buf);
                                    }
#else
                                    using var mgr = new DirectBufferMemoryManager(dir);
                                    var mem = mgr.Memory.Slice(pos, rem);
                                    length = await socket.ReceiveAsync(mem, SocketFlags.None, cancellationToken);
#endif
                                }
                                else
                                {
#if NETFRAMEWORK
                                    length = await Task.Factory.FromAsync((cb, state) => socket.BeginReceive(dst.array(), dst.arrayOffset() + pos, rem, SocketFlags.None, cb, state), socket.EndReceive, null);
#else
                                    length = await socket.ReceiveAsync(new Memory<byte>(dst.array(), dst.arrayOffset() + pos, rem), SocketFlags.None, cancellationToken);
#endif
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
#if NETFRAMEWORK
                                    length = await Task.Factory.FromAsync(socket.BeginReceive, socket.EndReceive, bufs, SocketFlags.None, null);
#else
                                    length = await socket.ReceiveAsync(bufs, SocketFlags.None);
#endif

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
        }

        /// <summary>
        /// Implements the Accept logic as an asynchronous task.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="gatheringWrite"></param>
        /// <param name="src"></param>
        /// <param name="srcs"></param>
        /// <param name="timeout"></param>
        /// <param name="unit"></param>
        /// <param name="accessControlContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        static Task<global::java.lang.Number> WriteAsync(global::sun.nio.ch.DotNetAsynchronousSocketChannelImpl self, bool gatheringWrite, ByteBuffer src, ByteBuffer[] srcs, long timeout, TimeUnit unit, AccessControlContext accessControlContext, CancellationToken cancellationToken)
        {
            var socket = FileDescriptorAccessor.GetSocket(self.fd);
            if (socket == null)
                throw new global::java.io.IOException("Socket is closed.");

            // we were told to do a scattering read, but only received one buffer, do a regular read
            if (gatheringWrite && srcs.Length == 1)
                src = srcs[0];

            return ImplAsync();

            async Task<Number> ImplAsync()
            {
                try
                {
                    // timeout was specified, wait for data to be sent
                    var t = (int)System.Math.Min(unit.convert(timeout, TimeUnit.MILLISECONDS), int.MaxValue);
                    if (t > 0)
                    {
                        // non-optimal usage, but we have no way to combine timeout with true async
                        return await Task.Run<Number>(() =>
                        {
                            var previousBlocking = socket.Blocking;
                            var previousSendTimeout = socket.SendTimeout;

                            try
                            {
                                socket.Blocking = true;
                                socket.SendTimeout = t;

                                if (src != null)
                                {
                                    int length = 0;

                                    int pos = src.position();
                                    int lim = src.limit();
                                    int rem = pos <= lim ? lim - pos : 0;

                                    if (src is DirectBuffer dir)
                                    {
#if NETFRAMEWORK
                                        var buf = ArrayPool<byte>.Shared.Rent(rem);

                                        try
                                        {
                                            Marshal.Copy((IntPtr)dir.address() + pos, buf, 0, rem);
                                            length = socket.Send(buf, rem, SocketFlags.None);
                                        }
                                        finally
                                        {
                                            ArrayPool<byte>.Shared.Return(buf);
                                        }
#else
                                        unsafe
                                        {
                                            length = socket.Send(new Span<byte>((byte*)(IntPtr)dir.address() + pos, rem), SocketFlags.None);
                                        }
#endif
                                    }
                                    else
                                    {
                                        length = socket.Send(src.array(), src.arrayOffset() + pos, rem, SocketFlags.None);
                                    }

                                    src.position(pos + length);
                                    return gatheringWrite ? Long.valueOf(length) : Integer.valueOf(length);
                                }
                                else
                                {
                                    int length = 0;
                                    var bufs = new ArraySegment<byte>[srcs.Length];

                                    try
                                    {
                                        // prepare array segments for buffers
                                        for (int i = 0; i < srcs.Length; i++)
                                        {
                                            var dbf = srcs[i];
                                            int pos = dbf.position();
                                            int lim = dbf.limit();
                                            int rem = pos <= lim ? lim - pos : 0;

                                            if (dbf is DirectBuffer dir)
                                            {
                                                var buf = ArrayPool<byte>.Shared.Rent(rem);
                                                Marshal.Copy((IntPtr)dir.address() + pos, buf, 0, rem);
                                                bufs[i] = new ArraySegment<byte>(buf, 0, rem);
                                            }
                                            else
                                                bufs[i] = new ArraySegment<byte>(dbf.array(), dbf.arrayOffset() + pos, rem);
                                        }

                                        // send data from segments
                                        length = socket.Send(bufs, SocketFlags.None);
                                    }
                                    finally
                                    {
                                        for (int i = 0; i < srcs.Length; i++)
                                        {
                                            var dbf = srcs[i];
                                            var buf = bufs[i];

                                            // return temporary buffer
                                            if (dbf is DirectBuffer dir)
                                                ArrayPool<byte>.Shared.Return(buf.Array);
                                        }
                                    }

                                    // advance buffers by amount sent
                                    var l = length;
                                    for (int i = 0; i < srcs.Length; i++)
                                    {
                                        var dbf = srcs[i];
                                        int pos = dbf.position();
                                        int lim = dbf.limit();
                                        int rem = pos <= lim ? lim - pos : 0;
                                        var len = System.Math.Min(l, rem);

                                        dbf.position(pos + len);
                                        l -= len;
                                    }

                                    // return total number of bytes written
                                    return gatheringWrite ? Long.valueOf(length) : Integer.valueOf(length);
                                }
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
                            socket.SendTimeout = 0;

                            if (src != null)
                            {
                                int length = 0;

                                int pos = src.position();
                                int lim = src.limit();
                                int rem = pos <= lim ? lim - pos : 0;

                                if (src is DirectBuffer dir)
                                {
#if NETFRAMEWORK
                                    var buf = ArrayPool<byte>.Shared.Rent(rem);

                                    try
                                    {
                                        Marshal.Copy((IntPtr)dir.address() + pos, buf, 0, rem);
                                        length = await Task.Factory.FromAsync((cb, state) => socket.BeginSend(buf, 0, rem, SocketFlags.None, cb, state), socket.EndSend, null);
                                    }
                                    finally
                                    {
                                        ArrayPool<byte>.Shared.Return(buf);
                                    }
#else
                                    using var mgr = new DirectBufferMemoryManager(dir);
                                    var mem = mgr.Memory.Slice(pos, rem);
                                    length = await socket.SendAsync(mem, SocketFlags.None, cancellationToken);
#endif
                                }
                                else
                                {
#if NETFRAMEWORK
                                    length = await Task.Factory.FromAsync((cb, state) => socket.BeginSend(src.array(), src.arrayOffset() + pos, rem, SocketFlags.None, cb, state), socket.EndSend, null);
#else
                                    length = await socket.SendAsync(new Memory<byte>(src.array(), src.arrayOffset() + pos, rem), SocketFlags.None, cancellationToken);
#endif
                                }

                                src.position(pos + length);
                                return gatheringWrite ? Long.valueOf(length) : Integer.valueOf(length);
                            }
                            else
                            {
                                int length = 0;
                                var bufs = new ArraySegment<byte>[srcs.Length];

                                try
                                {
                                    // prepare array segments for buffers
                                    for (int i = 0; i < srcs.Length; i++)
                                    {
                                        var dbf = srcs[i];
                                        int pos = dbf.position();
                                        int lim = dbf.limit();
                                        int rem = pos <= lim ? lim - pos : 0;

                                        if (dbf is DirectBuffer dir)
                                        {
                                            var buf = ArrayPool<byte>.Shared.Rent(rem);
                                            Marshal.Copy((IntPtr)dir.address() + pos, buf, 0, rem);
                                            bufs[i] = new ArraySegment<byte>(buf, 0, rem);
                                        }
                                        else
                                            bufs[i] = new ArraySegment<byte>(dbf.array(), dbf.arrayOffset() + pos, rem);
                                    }

                                    // send data from segments
#if NETFRAMEWORK
                                    length = await Task.Factory.FromAsync(socket.BeginSend, socket.EndSend, bufs, SocketFlags.None, null);
#else
                                    length = await socket.SendAsync(bufs, SocketFlags.None);
#endif
                                }
                                finally
                                {
                                    for (int i = 0; i < srcs.Length; i++)
                                    {
                                        var dbf = srcs[i];
                                        var buf = bufs[i];

                                        // return temporary buffer
                                        if (dbf is DirectBuffer dir)
                                            ArrayPool<byte>.Shared.Return(buf.Array);
                                    }
                                }

                                // advance buffers by amount sent
                                var l = length;
                                for (int i = 0; i < srcs.Length; i++)
                                {
                                    var dbf = srcs[i];
                                    int pos = dbf.position();
                                    int lim = dbf.limit();
                                    int rem = pos <= lim ? lim - pos : 0;
                                    var len = System.Math.Min(l, rem);

                                    dbf.position(pos + len);
                                    l -= len;
                                }

                                // return total number of bytes written
                                return gatheringWrite ? Long.valueOf(length) : Integer.valueOf(length);
                            }
                        }
                        finally
                        {
                            socket.Blocking = previousBlocking;
                            socket.SendTimeout = previousSendTimeout;
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
                    self.enableWriting();
                }
            }

        }

#endif

                    }

}
