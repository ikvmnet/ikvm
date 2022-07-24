/*
  Copyright (C) 2011 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/

using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using IKVM.Java.Externs.java.io;

namespace IKVM.Java.Externs.sun.nio.ch
{

    static class Net
    {

        public static bool isIPv6Available0()
        {
            return Socket.OSSupportsIPv6;
        }

        public static int isExclusiveBindAvailable()
        {
#if NETCOREAPP
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? 1 : -1;
#else
            return 1;
#endif
        }

        public static bool canIPv6SocketJoinIPv4Group0()
        {
            return false;
        }

        public static bool canJoin6WithIPv4Group0()
        {
            return false;
        }

        public static void shutdown(global::java.io.FileDescriptor fd, int how)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = (Socket)fd.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket is closed.");

            try
            {
                socket.Shutdown(how == global::sun.nio.ch.Net.SHUT_RD ? SocketShutdown.Receive : SocketShutdown.Send);
            }
            catch (SocketException e)
            {
                throw global::java.net.SocketUtil.convertSocketExceptionToIOException(e);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed.");
            }
#endif
        }

        public static int localPort(global::java.io.FileDescriptor fd)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = (Socket)fd.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket is closed.");

            try
            {
                var ep = (System.Net.IPEndPoint)socket.LocalEndPoint;
                return ep.Port;
            }
            catch (SocketException e)
            {
                throw global::java.net.SocketUtil.convertSocketExceptionToIOException(e);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed.");
            }
#endif
        }

        public static global::java.net.InetAddress localInetAddress(global::java.io.FileDescriptor fd)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = (Socket)fd.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket is closed.");

            try
            {
                var ep = (System.Net.IPEndPoint)socket.LocalEndPoint;
                return global::java.net.SocketUtil.getInetAddressFromIPEndPoint(ep);
            }
            catch (SocketException e)
            {
                throw global::java.net.SocketUtil.convertSocketExceptionToIOException(e);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed.");
            }
#endif
        }

        public static int remotePort(global::java.io.FileDescriptor fd)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = (Socket)fd.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket is closed.");

            try
            {
                var ep = (System.Net.IPEndPoint)socket.RemoteEndPoint;
                return ep.Port;
            }
            catch (SocketException e)
            {
                throw global::java.net.SocketUtil.convertSocketExceptionToIOException(e);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed.");
            }
#endif
        }

        public static global::java.net.InetAddress remoteInetAddress(global::java.io.FileDescriptor fd)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = (Socket)fd.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket is closed.");

            try
            {
                var ep = (System.Net.IPEndPoint)socket.RemoteEndPoint;
                return global::java.net.SocketUtil.getInetAddressFromIPEndPoint(ep);
            }
            catch (SocketException e)
            {
                throw global::java.net.SocketUtil.convertSocketExceptionToIOException(e);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed.");
            }
#endif
        }

        public static int getIntOption0(global::java.io.FileDescriptor fd, bool mayNeedConversion, int level, int opt)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = (Socket)fd.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket is closed.");

            var sol = (SocketOptionLevel)level;
            var son = (SocketOptionName)opt;

            if (sol == SocketOptionLevel.IPv6 && opt == global::ikvm.@internal.Winsock.IPV6_TCLASS)
                return 0;

            try
            {
                object obj = socket.GetSocketOption(sol, son);
                return obj is LingerOption linger ? linger.Enabled ? linger.LingerTime : -1 : (int)obj;
                }
            catch (SocketException e) when (mayNeedConversion && e.SocketErrorCode == SocketError.ProtocolOption && sol == SocketOptionLevel.IP && son == SocketOptionName.TypeOfService)
            {
                if (mayNeedConversion)
                {
                    if (x.ErrorCode == global::java.net.SocketUtil.WSAENOPROTOOPT
                        && sol == System.Net.Sockets.SocketOptionLevel.IP
                        && son == System.Net.Sockets.SocketOptionName.TypeOfService)
                    {
                        return 0;
                    }
            catch (SocketException e)
            {
                throw global::java.net.SocketUtil.convertSocketExceptionToIOException(e);
                }
                throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed.");
            }
#endif
        }

        public static void setIntOption0(global::java.io.FileDescriptor fd, bool mayNeedConversion, int level, int opt, int arg, bool isIPv6)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = (Socket)fd.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket is closed.");

            var sol = (SocketOptionLevel)level;
            var son = (SocketOptionName)opt;

            if (sol == SocketOptionLevel.IPv6 && opt == global::ikvm.@internal.Winsock.IPV6_TCLASS)
                return;

            if (mayNeedConversion)
            {
                const int IPTOS_TOS_MASK = 0x1e;
                const int IPTOS_PREC_MASK = 0xe0;
                if (sol == SocketOptionLevel.IP && son == SocketOptionName.TypeOfService)
                    arg &= IPTOS_TOS_MASK | IPTOS_PREC_MASK;
                }

            try
            {
                socket.SetSocketOption(sol, son, arg);
            }
            catch (SocketException x)
            {
                if (mayNeedConversion)
                {
                    if (x.SocketErrorCode == SocketError.ProtocolOption && sol == SocketOptionLevel.IP && (son == SocketOptionName.TypeOfService || son == SocketOptionName.MulticastLoopback))
                        return;
                    if (x.SocketErrorCode == SocketError.InvalidArgument && sol == SocketOptionLevel.IP && son == SocketOptionName.TypeOfService)
                        return;
                    }

                throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed.");
            }
#endif
        }

        static void PutInt(byte[] buf, int pos, int value)
        {
            buf[pos + 0] = (byte)(value >> 24);
            buf[pos + 1] = (byte)(value >> 16);
            buf[pos + 2] = (byte)(value >> 8);
            buf[pos + 3] = (byte)(value >> 0);
        }

        public static int joinOrDrop4(bool join, global::java.io.FileDescriptor fd, int group, int interf, int source)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = (Socket)fd.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket is closed.");

            try
            {
                if (source == 0)
                {
                    socket.SetSocketOption(SocketOptionLevel.IP, join ? SocketOptionName.AddMembership : SocketOptionName.DropMembership, new MulticastOption(new System.Net.IPAddress(System.Net.IPAddress.HostToNetworkOrder(group) & 0xFFFFFFFFL), new System.Net.IPAddress(System.Net.IPAddress.HostToNetworkOrder(interf) & 0xFFFFFFFFL)));
                }
                else
                {
                    // ip_mreq_source
                    var optionValue = new byte[12];
                    PutInt(optionValue, 0, group);
                    PutInt(optionValue, 4, source);
                    PutInt(optionValue, 8, interf);
                    socket.SetSocketOption(SocketOptionLevel.IP, join ? SocketOptionName.AddSourceMembership : SocketOptionName.DropSourceMembership, optionValue);
                }

                return 0;
            }
            catch (SocketException e)
            {
                throw global::java.net.SocketUtil.convertSocketExceptionToIOException(e);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed.");
            }
#endif
        }

        public static int blockOrUnblock4(bool block, global::java.io.FileDescriptor fd, int group, int interf, int source)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = (Socket)fd.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket is closed.");

            try
            {
                // ip_mreq_source
                var optionValue = new byte[12];
                PutInt(optionValue, 0, group);
                PutInt(optionValue, 4, source);
                PutInt(optionValue, 8, interf);
                socket.SetSocketOption(SocketOptionLevel.IP, block ? SocketOptionName.BlockSource : SocketOptionName.UnblockSource, optionValue);
                return 0;
            }
            catch (SocketException x)
            {
                throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed");
            }
#endif
        }

        /// <summary>
        /// Write a sockaddr_in6 into optionValue at offset pos.
        /// </summary>
        /// <param name="optionValue"></param>
        /// <param name="pos"></param>
        /// <param name="addr"></param>
        static void PutSockAddrIn6(byte[] optionValue, int pos, byte[] addr)
        {
            // sin6_family
            optionValue[pos] = 23; // AF_INET6

            // sin6_addr
            Buffer.BlockCopy(addr, 0, optionValue, pos + 8, addr.Length);
        }

        public static int joinOrDrop6(bool join, global::java.io.FileDescriptor fd, byte[] group, int index, byte[] source)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = (Socket)fd.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket is closed.");

            try
            {
                if (source == null)
                {
                    socket.SetSocketOption(SocketOptionLevel.IPv6, join ? SocketOptionName.AddMembership : SocketOptionName.DropMembership, new IPv6MulticastOption(new System.Net.IPAddress(group), index));
                }
                else
                {
                    const SocketOptionName MCAST_JOIN_SOURCE_GROUP = (SocketOptionName)45;
                    const SocketOptionName MCAST_LEAVE_SOURCE_GROUP = (SocketOptionName)46;

                    // group_source_req
                    var optionValue = new byte[264];
                    optionValue[0] = (byte)index;
                    optionValue[1] = (byte)(index >> 8);
                    optionValue[2] = (byte)(index >> 16);
                    optionValue[3] = (byte)(index >> 24);
                    PutSockAddrIn6(optionValue, 8, group);
                    PutSockAddrIn6(optionValue, 136, source);
                    socket.SetSocketOption(SocketOptionLevel.IPv6, join ? MCAST_JOIN_SOURCE_GROUP : MCAST_LEAVE_SOURCE_GROUP, optionValue);
                }
                return 0;
            }
            catch (SocketException e)
            {
                throw global::java.net.SocketUtil.convertSocketExceptionToIOException(e);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed.");
            }
#endif
        }

        public static int blockOrUnblock6(bool block, global::java.io.FileDescriptor fd, byte[] group, int index, byte[] source)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = (Socket)fd.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket is closed.");

            try
            {
                const SocketOptionName MCAST_BLOCK_SOURCE = (SocketOptionName)43;
                const SocketOptionName MCAST_UNBLOCK_SOURCE = (SocketOptionName)44;

                // group_source_req
                var optionValue = new byte[264];
                optionValue[0] = (byte)index;
                optionValue[1] = (byte)(index >> 8);
                optionValue[2] = (byte)(index >> 16);
                optionValue[3] = (byte)(index >> 24);
                PutSockAddrIn6(optionValue, 8, group);
                PutSockAddrIn6(optionValue, 136, source);
                socket.SetSocketOption(SocketOptionLevel.IPv6, block ? MCAST_BLOCK_SOURCE : MCAST_UNBLOCK_SOURCE, optionValue);
                return 0;
            }
            catch (SocketException e)
            {
                throw global::java.net.SocketUtil.convertSocketExceptionToIOException(e);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed.");
            }
#endif
        }

        public static void setInterface4(global::java.io.FileDescriptor fd, int interf)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = (Socket)fd.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket is closed.");

            try
            {
                socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface, System.Net.IPAddress.HostToNetworkOrder(interf));
            }
            catch (SocketException e)
            {
                throw global::java.net.SocketUtil.convertSocketExceptionToIOException(e);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed.");
            }
#endif
        }

        public static int getInterface4(global::java.io.FileDescriptor fd)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = (Socket)fd.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket is closed.");

            try
            {
                return System.Net.IPAddress.NetworkToHostOrder((int)socket.GetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface));
            }
            catch (SocketException e)
            {
                throw global::java.net.SocketUtil.convertSocketExceptionToIOException(e);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed.");
            }
#endif
        }

        public static void setInterface6(global::java.io.FileDescriptor fd, int index)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = (Socket)fd.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket is closed.");

            try
            {
                socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastInterface, index);
            }
            catch (SocketException e)
            {
                throw global::java.net.SocketUtil.convertSocketExceptionToIOException(e);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed.");
            }
#endif
        }

        public static int getInterface6(global::java.io.FileDescriptor fd)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = (Socket)fd.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket is closed.");

            try
            {
                return (int)socket.GetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastInterface);
            }
            catch (SocketException e)
            {
                throw global::java.net.SocketUtil.convertSocketExceptionToIOException(e);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed");
            }
#endif
        }

        public static global::java.io.FileDescriptor socket0(bool preferIPv6, bool stream, bool reuse)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            try
            {
                var addressFamily = preferIPv6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;
                var socketType = stream ? SocketType.Stream : SocketType.Dgram;
                var protocolType = stream ? ProtocolType.Tcp : ProtocolType.Udp;
                var socket = new Socket(addressFamily, socketType, protocolType);

                if (preferIPv6)
                    socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, 0);

                if (!stream)
                {
                    setConnectionReset(socket, false);

                var fd = new global::java.io.FileDescriptor();
                fd.setSocket(socket);
                return fd;
            }
            catch (SocketException e)
            {
                throw global::java.net.SocketUtil.convertSocketExceptionToIOException(e);
            }
#endif
        }

        public static void bind0(global::java.io.FileDescriptor fd, bool preferIPv6, bool useExclBind, global::java.net.InetAddress addr, int port)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = (Socket)fd.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket is closed.");

            try
            {
                if (useExclBind)
                    socket.ExclusiveAddressUse = true;

                socket.Bind(new System.Net.IPEndPoint(global::java.net.SocketUtil.getAddressFromInetAddress(addr, preferIPv6), port));
            }
            catch (SocketException e)
            {
                throw global::java.net.SocketUtil.convertSocketExceptionToIOException(e);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed.");
            }
#endif
        }

        public static void listen(global::java.io.FileDescriptor fd, int backlog)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = (Socket)fd.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket is closed.");

            try
            {
                socket.Listen(backlog);
            }
            catch (SocketException e)
            {
                throw global::java.net.SocketUtil.convertSocketExceptionToIOException(e);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed");
            }
#endif
        }

        internal static void setConnectionReset(Socket socket, bool enable)
        {
            // Windows 2000 introduced a "feature" that causes it to return WSAECONNRESET from receive,
            // if a previous send resulted in an ICMP port unreachable. For unconnected datagram sockets,
            // we disable this feature by using this ioctl.
            const int IOC_IN = unchecked((int)0x80000000);
            const int IOC_VENDOR = 0x18000000;
            const int SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;

            socket.IOControl(SIO_UDP_CONNRESET, new byte[] { enable ? (byte)1 : (byte)0 }, null);
        }

        public static int connect0(bool preferIPv6, global::java.io.FileDescriptor fd, global::java.net.InetAddress remote, int remotePort)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = (Socket)fd.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket is closed.");

            try
            {
                var ep = new System.Net.IPEndPoint(global::java.net.SocketUtil.getAddressFromInetAddress(remote, preferIPv6), remotePort);
                var datagram = socket.SocketType == SocketType.Dgram;
                if (datagram || fd.isSocketBlocking())
                {
                    socket.Connect(ep);
                    if (datagram)
                        setConnectionReset(socket, true);

                    return 1;
                }
                else
                {
                    fd.setAsyncResult(socket.BeginConnect(ep, null, null));
                    return global::sun.nio.ch.IOStatus.UNAVAILABLE;
                }
            }
            catch (SocketException e)
            {
                throw new global::java.net.ConnectException(e.Message);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed.");
            }
#endif
        }

        public static int poll(global::java.io.FileDescriptor fd, int events, long timeout)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = (Socket)fd.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket is closed.");

            var selectMode = events switch
            {
                int e when e == global::sun.nio.ch.Net.POLLCONN => SelectMode.SelectWrite,
                int e when e == global::sun.nio.ch.Net.POLLOUT => SelectMode.SelectWrite,
                int e when e == global::sun.nio.ch.Net.POLLIN => SelectMode.SelectRead,
                _ => throw new NotSupportedException(),
            };

            var microSeconds = timeout >= int.MaxValue / 1000 ? int.MaxValue : (int)(timeout * 1000);
            try
            {
                if (socket.Poll(microSeconds, selectMode))
                    return events;
                }
            catch (SocketException e)
            {
                throw new global::java.net.SocketException(e.Message);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed.");
            }

            return 0;
#endif
        }

        public static short pollinValue() => 0x0001;

        public static short polloutValue() => 0x0004;

        public static short pollerrValue() => 0x0008;

        public static short pollhupValue() => 0x0010;

        public static short pollnvalValue() => 0x0020;

        public static short pollconnValue() => 0x0002;

    }

}
