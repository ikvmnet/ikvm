using System;
using System.Buffers;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

using IKVM.Internal;
using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.Accessors.Sun.Nio.Ch;
using IKVM.Runtime.Util.Java.Net;

namespace IKVM.Java.Externs.sun.nio.ch
{

    static class DatagramChannelImpl
    {

#if FIRST_PASS == false

        static FileDescriptorAccessor fileDescriptorAccessor;

        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);


        static DatagramChannelImplAccessor datagramChannelImplAccessor;

        static DatagramChannelImplAccessor DatagramChannelImplAccessor => JVM.BaseAccessors.Get(ref datagramChannelImplAccessor);

        /// <summary>
        /// Compiles a fast setter for a <see cref="FieldInfo"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        static Action<T, V> MakeFieldSetter<T, V>(FieldInfo field)
        {
            var p = Expression.Parameter(typeof(T));
            var v = Expression.Parameter(typeof(V));
            var e = Expression.Lambda<Action<T, V>>(Expression.Assign(Expression.Field(field.DeclaringType.IsValueType ? Expression.Unbox(p, field.DeclaringType) : Expression.ConvertChecked(p, field.DeclaringType), field), v), p, v);
            return e.Compile();
        }

#if NETCOREAPP3_1_OR_GREATER

        // HACK .NET Core has an explicit check for _isConnected https://github.com/dotnet/runtime/issues/77962
        static readonly FieldInfo SocketIsConnectedField = typeof(Socket).GetField("_isConnected", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly Action<Socket, bool> SocketIsConnectedFieldSetter = MakeFieldSetter<Socket, bool>(SocketIsConnectedField);

#endif

        const IOControlCode SIO_UDP_CONNRESET = (IOControlCode)(-1744830452);
        static readonly byte[] IOControlTrueBuffer = BitConverter.GetBytes(1);
        static readonly byte[] IOControlFalseBuffer = BitConverter.GetBytes(0);
        static readonly byte[] TempBuffer = new byte[1];

        /// <summary>
        /// Peek at the queue to see if there is an ICMP port unreachable. If there is, then receive it.
        /// </summary>
        /// <param name="socket"></param>
        static void PurgeOutstandingICMP(Socket socket)
        {
            while (true)
            {
                // check for outstanding packet
                if (socket.Poll(0, SelectMode.SelectRead) == false)
                    break;

                try
                {
                    var ep = (EndPoint)new IPEndPoint(IPAddress.IPv6Any, 0);
                    socket.EndReceiveFrom(socket.BeginReceiveFrom(TempBuffer, 0, TempBuffer.Length, SocketFlags.Peek, ref ep, null, null), ref ep);
                }
                catch (SocketException e) when (e.SocketErrorCode == SocketError.ConnectionReset)
                {
                    try
                    {
                        var ep = (EndPoint)new IPEndPoint(IPAddress.IPv6Any, 0);
                        socket.EndReceiveFrom(socket.BeginReceiveFrom(TempBuffer, 0, TempBuffer.Length, SocketFlags.Peek, ref ep, null, null), ref ep);
                    }
                    catch (SocketException e2) when (e2.SocketErrorCode == SocketError.ConnectionReset)
                    {

                    }

                    continue;
                }

                break;
            }
        }

#endif

        public static void initIDs()
        {

        }

        /// <summary>
        /// Implements the native method 'disconnect0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="isIPv6"></param>
        public static void disconnect0(object fd, bool isIPv6)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = FileDescriptorAccessor.GetSocket(fd);
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed.");

            try
            {
                // NOTE we use async connect to work around the issue that the .NET Socket class disallows sync Connect after the socket has received WSAECONNRESET
#if NETCOREAPP3_1_OR_GREATER
                // HACK .NET Core has an explicit check for _isConnected https://github.com/dotnet/runtime/issues/77962
                SocketIsConnectedFieldSetter(socket, false);
#endif
                socket.EndConnect(socket.BeginConnect(new IPEndPoint(IPAddress.IPv6Any, 0), null, null));

                // see comment in in socketCreate
                if (RuntimeUtil.IsWindows)
                    socket.IOControl(SIO_UDP_CONNRESET, IOControlFalseBuffer, null);
            }
            catch (SocketException e)
            {
                throw global::java.net.SocketUtil.convertSocketExceptionToIOException(e);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket closed.");
            }
#endif
        }

        /// <summary>
        /// Implements the native 'receive0' method.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <param name="connected"></param>
        /// <returns></returns>
        public static unsafe int receive0(object self, object fd, long address, int len, bool connected)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = FileDescriptorAccessor.GetSocket(fd);
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed.");

            var remoteEndpoint = (EndPoint)new IPEndPoint(IPAddress.IPv6Any, 0);
            var length = 0;
            var packet = ArrayPool<byte>.Shared.Rent(len);

            try
            {
                length = socket.EndReceiveFrom(socket.BeginReceiveFrom(packet, 0, len, SocketFlags.None, ref remoteEndpoint, null, null), ref remoteEndpoint);
                packet.CopyTo(new Span<byte>((byte**)(IntPtr)address, length));
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.WouldBlock)
            {
                return global::sun.nio.ch.IOStatus.UNAVAILABLE;
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.MessageSize)
            {
                length = len;
                packet.CopyTo(new Span<byte>((byte**)(IntPtr)address, length));
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.ConnectionReset)
            {
                // Windows may leave multiple ICMP packets on the socket, purge them
                if (RuntimeUtil.IsWindows)
                    PurgeOutstandingICMP(socket);

                throw new global::java.net.PortUnreachableException("ICMP Port Unreachable");
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.ConnectionRefused)
            {
                throw new global::java.net.PortUnreachableException("ICMP Port Unreachable");
            }
            catch (SocketException e)
            {
                throw e.ToIOException();
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket closed.");
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(packet);
            }

            // check that we received an IP endpoint
            if (remoteEndpoint is not IPEndPoint ipRemoteEndpoint)
                throw new global::java.net.SocketException("Unexpected resulting endpoint type.");

            // update remote address if it has changed
            var remoteAddress = DatagramChannelImplAccessor.InvokeRemoteAddress(self);
            if (remoteAddress == null || ipRemoteEndpoint.ToInetAddress().equals(remoteAddress) == false)
                DatagramChannelImplAccessor.SetSender(self, ipRemoteEndpoint.ToInetAddress());

            return length;
#endif
        }

        /// <summary>
        /// Implements the native method 'send0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="preferIPv6"></param>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <param name="addr"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static unsafe int send0(object self, bool preferIPv6, object fd, long address, int len, global::java.net.InetAddress addr, int port)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = FileDescriptorAccessor.GetSocket(fd);
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed.");

            var packet = ArrayPool<byte>.Shared.Rent(len);

            try
            {
                new ReadOnlySpan<byte>((byte*)(IntPtr)address, len).CopyTo(packet);
                return socket.SendTo(packet, 0, len, SocketFlags.None, new IPEndPoint(addr.ToIPAddress(), port));
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.WouldBlock)
            {
                return global::sun.nio.ch.IOStatus.UNAVAILABLE;
            }
            catch (SocketException e)
            {
                throw e.ToIOException();
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket closed.");
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(packet);
            }
#endif
        }

    }

}
