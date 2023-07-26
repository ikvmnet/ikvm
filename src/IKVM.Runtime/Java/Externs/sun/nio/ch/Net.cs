using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.Util.Java.Net;

using static IKVM.Java.Externs.java.net.SocketImplUtil;

namespace IKVM.Java.Externs.sun.nio.ch
{

    /// <summary>
    /// Implements the external methods for <see cref="global::sun.nio.ch.Net"/>.
    /// </summary>
    static class Net
    {

#if FIRST_PASS == false

        static FileDescriptorAccessor fileDescriptorAccessor;
        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);

        [StructLayout(LayoutKind.Explicit)]
        unsafe struct sockaddr_storage
        {

            [FieldOffset(0)]
            void* __ss_align;

            [FieldOffset(0)]
            fixed byte __ss_size[128];

            [FieldOffset(0)]
            ushort __ss_family;

            public ushort ss_family { get => __ss_family; set => __ss_family = value; }

        }

        [StructLayout(LayoutKind.Explicit)]
        public unsafe struct in_addr
        {

            [FieldOffset(0)]
            uint __align;

            [FieldOffset(0)]
            public fixed byte s_addr[4];

        }

        [StructLayout(LayoutKind.Sequential)]
        unsafe struct sockaddr_in
        {

            public ushort sin_family;

            public ushort sin_port;

            public in_addr sin_addr;

            fixed byte __sin_zero[8];

        }

        [StructLayout(LayoutKind.Explicit)]
        unsafe struct in6_addr
        {

            [FieldOffset(0)]
            uint __align;

            [FieldOffset(0)]
            public fixed byte s6_addr[16];

        }

        [StructLayout(LayoutKind.Sequential)]
        struct sockaddr_in6
        {

            public ushort sin6_family;

            public ushort sin6_port;

            public uint sin6_flowinfo;

            public in6_addr sin6_addr;

            public uint sin6_scope_id;

        }

        [StructLayout(LayoutKind.Sequential)]
        struct ip_mreq_source
        {

            public in_addr imr_multiaddr;
            public in_addr imr_sourceaddr;
            public in_addr imr_interface;

        }

        [StructLayout(LayoutKind.Sequential)]
        struct group_source_req
        {

            public uint gsr_interface;
            public sockaddr_storage gsr_group;
            public sockaddr_storage gsr_source;

        }

        static readonly ushort AF_UNSPEC;
        static readonly ushort AF_INET;
        static readonly ushort AF_INET6;

        static readonly int IPPROTO_IP;
        static readonly int IPPROTO_IPV6;
        static readonly int IP_ADD_SOURCE_MEMBERSHIP;
        static readonly int IP_DROP_SOURCE_MEMBERSHIP;
        static readonly int MCAST_BLOCK_SOURCE;
        static readonly int MCAST_UNBLOCK_SOURCE;
        static readonly int MCAST_JOIN_SOURCE_GROUP;
        static readonly int MCAST_LEAVE_SOURCE_GROUP;

        /// <summary>
        /// Invokes the 'setsockopt' Posix function.
        /// </summary>
        /// <param name="sockfd"></param>
        /// <param name="level"></param>
        /// <param name="optname"></param>
        /// <param name="optval"></param>
        /// <param name="optlen"></param>
        /// <returns></returns>
        [DllImport("libc", SetLastError = true)]
        static unsafe extern int setsockopt(SafeHandle sockfd, int level, int optname, void* optval, int optlen);

        /// <summary>
        /// Invokes the 'getsockopt' Posix function.
        /// </summary>
        /// <param name="sockfd"></param>
        /// <param name="level"></param>
        /// <param name="optname"></param>
        /// <param name="optval"></param>
        /// <param name="optlen"></param>
        /// <returns></returns>
        [DllImport("libc", SetLastError = true)]
        static unsafe extern int getsockopt(SafeHandle sockfd, int level, int optname, void* optval, int* optlen);

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static Net()
        {
            if (RuntimeUtil.IsWindows)
            {
                AF_UNSPEC = 0;
                AF_INET = 2;
                AF_INET6 = 23;
                IPPROTO_IP = 0;
                IPPROTO_IPV6 = 41;
                IP_ADD_SOURCE_MEMBERSHIP = 39;
                IP_DROP_SOURCE_MEMBERSHIP = 40;
                MCAST_BLOCK_SOURCE = 43;
                MCAST_UNBLOCK_SOURCE = 44;
                MCAST_JOIN_SOURCE_GROUP = 45;
                MCAST_LEAVE_SOURCE_GROUP = 46;
            }
            else if (RuntimeUtil.IsLinux)
            {
                AF_UNSPEC = 0;
                AF_INET = 2;
                AF_INET6 = 10;
                IPPROTO_IP = 0;
                IPPROTO_IPV6 = 41;
                IP_ADD_SOURCE_MEMBERSHIP = 39;
                IP_DROP_SOURCE_MEMBERSHIP = 40;
                MCAST_BLOCK_SOURCE = 43;
                MCAST_UNBLOCK_SOURCE = 44;
                MCAST_JOIN_SOURCE_GROUP = 46;
                MCAST_LEAVE_SOURCE_GROUP = 47;
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

#endif

        /// <summary>
        /// Implements the native method for 'isIPv6Available0'.
        /// </summary>
        /// <returns></returns>
        public static bool isIPv6Available0()
        {
            return Socket.OSSupportsIPv6;
        }

        /// <summary>
        /// Implements the native method for 'isExclusiveBindAvailable'.
        /// </summary>
        /// <returns></returns>
        public static int isExclusiveBindAvailable()
        {
            return RuntimeUtil.IsWindows ? 1 : -1;
        }

        /// <summary>
        /// Implements the native method for 'canIPv6SocketJoinIPv4Group0'.
        /// </summary>
        /// <returns></returns>
        public static bool canIPv6SocketJoinIPv4Group0()
        {
            return false;
        }

        /// <summary>
        /// Implements the native method for 'canJoin6WithIPv4Group0'.
        /// </summary>
        /// <returns></returns>
        public static bool canJoin6WithIPv4Group0()
        {
            return false;
        }

        /// <summary>
        /// Implements the native method for 'socket0'.
        /// </summary>
        /// <param name="preferIPv6"></param>
        /// <param name="stream"></param>
        /// <param name="reuse"></param>
        /// <param name="fastLoopback"></param>
        /// <returns></returns>
        public static object socket0(bool preferIPv6, bool stream, bool reuse, bool fastLoopback)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                var addressFamily = preferIPv6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;
                var socketType = stream ? SocketType.Stream : SocketType.Dgram;
                var protocolType = stream ? ProtocolType.Tcp : ProtocolType.Udp;
                var socket = new Socket(addressFamily, socketType, protocolType);

                if (preferIPv6)
                    socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, 0);

                if (stream == false)
                    SetConnectionReset(socket, false);

                var fd = new global::java.io.FileDescriptor();
                FileDescriptorAccessor.SetSocket(fd, socket);
                return fd;
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'bind0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="preferIPv6"></param>
        /// <param name="useExclBind"></param>
        /// <param name="addr"></param>
        /// <param name="port"></param>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void bind0(global::java.io.FileDescriptor fd, bool preferIPv6, bool useExclBind, global::java.net.InetAddress addr, int port)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction(() =>
            {
                InvokeWithSocket(fd, socket =>
                {
                    if (useExclBind && (int)socket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress) != 1)
                        socket.ExclusiveAddressUse = true;

                    socket.Bind(new IPEndPoint(addr.ToIPAddress(), port));
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'listen'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="backlog"></param>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void listen(object fd, int backlog)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction(() =>
            {
                InvokeWithSocket(fd, socket =>
                {
                    socket.Listen(backlog);
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'connect0'.
        /// </summary>
        /// <param name="preferIPv6"></param>
        /// <param name="fd"></param>
        /// <param name="remote"></param>
        /// <param name="remotePort"></param>
        /// <returns></returns>
        public static int connect0(bool preferIPv6, object fd, global::java.net.InetAddress remote, int remotePort)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    var ep = new IPEndPoint(remote.ToIPAddress(), remotePort);
                    var datagram = socket.SocketType == SocketType.Dgram;
                    if (datagram || socket.Blocking)
                    {
                        socket.Connect(ep);
                        if (datagram)
                            SetConnectionReset(socket, true);

                        return 1;
                    }
                    else
                    {
                        var task = FileDescriptorAccessor.GetTask(fd);
                        if (task != null)
                            throw new global::java.net.SocketException("Outstanding async request.");

                        task = Task.Factory.FromAsync(socket.BeginConnect, socket.EndConnect, ep, null);
                        FileDescriptorAccessor.SetTask(fd, task);
                        return global::sun.nio.ch.IOStatus.UNAVAILABLE;
                    }
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'shutdown'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="how"></param>
        public static void shutdown(object fd, int how)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction(() =>
            {
                InvokeWithSocket(fd, socket =>
                {
                    socket.Shutdown((SocketShutdown)how);
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'localInetAddress'.
        /// </summary>
        /// <param name="fd"></param>
        /// <returns></returns>
        public static int localPort(object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    return ((IPEndPoint)socket.LocalEndPoint).Port;
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'localInetAddress'.
        /// </summary>
        /// <param name="fd"></param>
        /// <returns></returns>
        public static global::java.net.InetAddress localInetAddress(object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    return ((IPEndPoint)socket.LocalEndPoint).ToInetAddress();
                });
            });
#endif
        }

        /// <summary>
        /// <summary>
        /// Implements the native method for 'remotePort'.
        /// </summary>
        /// <param name="fd"></param>
        /// <returns></returns>
        public static int remotePort(object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    return ((IPEndPoint)socket.RemoteEndPoint).Port;
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'remoteInetAddress'.
        /// </summary>
        /// <param name="fd"></param>
        /// <returns></returns>
        public static global::java.net.InetAddress remoteInetAddress(object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    return ((IPEndPoint)socket.RemoteEndPoint).ToInetAddress();
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'setIntOption0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="mayNeedConversion"></param>
        /// <param name="level"></param>
        /// <param name="opt"></param>
        /// <returns></returns>
        public static int getIntOption0(object fd, bool mayNeedConversion, int level, int opt)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
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
                        return 0;
                    }
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'setIntOption0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="mayNeedConversion"></param>
        /// <param name="level"></param>
        /// <param name="opt"></param>
        /// <param name="arg"></param>
        /// <param name="isIPv6"></param>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void setIntOption0(object fd, bool mayNeedConversion, int level, int opt, int arg, bool isIPv6)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction(() =>
            {
                InvokeWithSocket(fd, socket =>
                {
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
                    catch (SocketException e) when (mayNeedConversion && e.SocketErrorCode == SocketError.ProtocolOption && sol == SocketOptionLevel.IP && (son == SocketOptionName.TypeOfService || son == SocketOptionName.MulticastLoopback))
                    {
                        return;
                    }
                    catch (SocketException e) when (mayNeedConversion && e.SocketErrorCode == SocketError.InvalidArgument && sol == SocketOptionLevel.IP && son == SocketOptionName.TypeOfService)
                    {
                        return;
                    }
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'poll'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="events"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static int poll(object fd, int events, long timeout)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    var selectMode = events switch
                    {
                        int e when e == global::sun.nio.ch.Net.POLLCONN => SelectMode.SelectWrite,
                        int e when e == global::sun.nio.ch.Net.POLLOUT => SelectMode.SelectWrite,
                        int e when e == global::sun.nio.ch.Net.POLLIN => SelectMode.SelectRead,
                        _ => throw new NotSupportedException(),
                    };

                    if (socket.Poll(timeout * 1000L > int.MaxValue ? int.MaxValue : (int)timeout * 1000, selectMode))
                        return events;

                    return 0;
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'joinOrDrop4'.
        /// </summary>
        /// <param name="join"></param>
        /// <param name="fd"></param>
        /// <param name="group"></param>
        /// <param name="interf"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        public unsafe static int joinOrDrop4(bool join, object fd, int group, int interf, int source)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    if (source == 0)
                    {
                        socket.SetSocketOption(SocketOptionLevel.IP, join ? SocketOptionName.AddMembership : SocketOptionName.DropMembership, new MulticastOption(new IPAddress(IPAddress.HostToNetworkOrder(group) & 0xFFFFFFFFL), new IPAddress(IPAddress.HostToNetworkOrder(interf) & 0xFFFFFFFFL)));
                    }
                    else
                    {
                        var imrMultiAddr = IPAddress.HostToNetworkOrder(group);
                        var imrSourceAddr = IPAddress.HostToNetworkOrder(source);
                        var imrInterface = IPAddress.HostToNetworkOrder(interf);

                        var optionValue = new ip_mreq_source();
                        optionValue.imr_multiaddr = Unsafe.As<int, in_addr>(ref imrMultiAddr);
                        optionValue.imr_sourceaddr = Unsafe.As<int, in_addr>(ref imrSourceAddr);
                        optionValue.imr_interface = Unsafe.As<int, in_addr>(ref imrInterface);

                        if (RuntimeUtil.IsWindows)
                        {
                            var v = ArrayPool<byte>.Shared.Rent(sizeof(ip_mreq_source));

                            try
                            {
                                fixed (byte* vptr = v)
                                    Buffer.MemoryCopy(&optionValue, vptr, v.Length, sizeof(ip_mreq_source));

                                socket.SetSocketOption(SocketOptionLevel.IP, join ? SocketOptionName.AddSourceMembership : SocketOptionName.DropSourceMembership, v);
                            }
                            finally
                            {
                                ArrayPool<byte>.Shared.Return(v);
                            }
                        }
                        else if (RuntimeUtil.IsLinux)
                        {
#if NETCOREAPP
                            if (setsockopt(socket.SafeHandle, IPPROTO_IP, join ? IP_ADD_SOURCE_MEMBERSHIP : IP_DROP_SOURCE_MEMBERSHIP, &optionValue, sizeof(ip_mreq_source)) != 0)
                                throw new SocketException(Marshal.GetLastWin32Error());
#else
                            throw new PlatformNotSupportedException();
#endif
                        }
                        else
                        {
                            throw new global::java.net.SocketException("Invalid option.");
                        }
                    }

                    return 0;
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'blockOrUnblock4'.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="fd"></param>
        /// <param name="group"></param>
        /// <param name="interf"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static unsafe int blockOrUnblock4(bool block, object fd, int group, int interf, int source)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    var imrMultiAddr = IPAddress.HostToNetworkOrder(group);
                    var imrSourceAddr = IPAddress.HostToNetworkOrder(source);
                    var imrInterface = IPAddress.HostToNetworkOrder(interf);

                    var optionValue = new ip_mreq_source();
                    optionValue.imr_multiaddr = Unsafe.As<int, in_addr>(ref imrMultiAddr);
                    optionValue.imr_sourceaddr = Unsafe.As<int, in_addr>(ref imrSourceAddr);
                    optionValue.imr_interface = Unsafe.As<int, in_addr>(ref imrInterface);

                    var v = ArrayPool<byte>.Shared.Rent(sizeof(ip_mreq_source));

                    try
                    {
                        fixed (byte* vptr = v)
                            Buffer.MemoryCopy(&optionValue, vptr, v.Length, sizeof(ip_mreq_source));

                        socket.SetSocketOption(SocketOptionLevel.IP, block ? SocketOptionName.BlockSource : SocketOptionName.UnblockSource, v);
                        return 0;
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(v);
                    }
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'joinOrDrop6'.
        /// </summary>
        /// <param name="join"></param>
        /// <param name="fd"></param>
        /// <param name="group"></param>
        /// <param name="index"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static unsafe int joinOrDrop6(bool join, object fd, byte[] group, int index, byte[] source)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    if (source == null)
                    {
                        socket.SetSocketOption(SocketOptionLevel.IPv6, join ? SocketOptionName.AddMembership : SocketOptionName.DropMembership, new IPv6MulticastOption(new IPAddress(group), index));
                    }
                    else
                    {
                        var groupSockAddr = new sockaddr_in6();
                        groupSockAddr.sin6_family = AF_INET6;
#if NETCOREAPP
                        groupSockAddr.sin6_addr = MemoryMarshal.AsRef<in6_addr>(group);
#else
                        fixed (byte* groupBufPtr = group)
                            groupSockAddr.sin6_addr = Marshal.PtrToStructure<in6_addr>((IntPtr)groupBufPtr);
#endif

                        var sourceSockAddr = new sockaddr_in6();
                        sourceSockAddr.sin6_family = AF_INET6;
#if NETCOREAPP
                        sourceSockAddr.sin6_addr = MemoryMarshal.AsRef<in6_addr>(source);
#else
                        fixed (byte* sourceBufPtr = source)
                            sourceSockAddr.sin6_addr = Marshal.PtrToStructure<in6_addr>((IntPtr)sourceBufPtr);
#endif

                        var groupSourceReq = new group_source_req();
                        groupSourceReq.gsr_interface = (uint)index;
                        groupSourceReq.gsr_group = Unsafe.As<sockaddr_in6, sockaddr_storage>(ref groupSockAddr);
                        groupSourceReq.gsr_source = Unsafe.As<sockaddr_in6, sockaddr_storage>(ref sourceSockAddr);

                        if (RuntimeUtil.IsWindows)
                        {
                            var v = ArrayPool<byte>.Shared.Rent(sizeof(group_source_req));

                            try
                            {
                                fixed (byte* vptr = v)
                                    Buffer.MemoryCopy(&groupSourceReq, vptr, v.Length, sizeof(group_source_req));

                                socket.SetSocketOption(SocketOptionLevel.IPv6, join ? (SocketOptionName)MCAST_JOIN_SOURCE_GROUP : (SocketOptionName)MCAST_LEAVE_SOURCE_GROUP, v);
                            }
                            finally
                            {
                                ArrayPool<byte>.Shared.Return(v);
                            }
                        }
                        else if (RuntimeUtil.IsLinux)
                        {
#if NETCOREAPP
                            if (setsockopt(socket.SafeHandle, IPPROTO_IPV6, join ? MCAST_JOIN_SOURCE_GROUP : MCAST_LEAVE_SOURCE_GROUP, &groupSourceReq, sizeof(group_source_req)) != 0)
                                throw new SocketException(Marshal.GetLastWin32Error());
#else
                            throw new PlatformNotSupportedException();
#endif
                        }
                        else
                        {
                            throw new global::java.net.SocketException("Invalid option.");
                        }
                    }

                    return 0;
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'blockOrUnblock6'.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="fd"></param>
        /// <param name="group"></param>
        /// <param name="index"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static unsafe int blockOrUnblock6(bool block, object fd, byte[] group, int index, byte[] source)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    // group_source_req
                    var groupSockAddr = new sockaddr_in6();
                    groupSockAddr.sin6_family = AF_INET6;
#if NETCOREAPP
                    groupSockAddr.sin6_addr = MemoryMarshal.AsRef<in6_addr>(group);
#else
                    fixed (byte* groupBufPtr = group)
                        groupSockAddr.sin6_addr = Marshal.PtrToStructure<in6_addr>((IntPtr)groupBufPtr);
#endif

                    var sourceSockAddr = new sockaddr_in6();
                    sourceSockAddr.sin6_family = AF_INET6;
#if NETCOREAPP
                    sourceSockAddr.sin6_addr = MemoryMarshal.AsRef<in6_addr>(source);
#else
                    fixed (byte* sourceBufPtr = source)
                        sourceSockAddr.sin6_addr = Marshal.PtrToStructure<in6_addr>((IntPtr)sourceBufPtr);
#endif

                    var groupSourceReq = new group_source_req();
                    groupSourceReq.gsr_interface = (uint)index;
                    groupSourceReq.gsr_group = Unsafe.As<sockaddr_in6, sockaddr_storage>(ref groupSockAddr);
                    groupSourceReq.gsr_source = Unsafe.As<sockaddr_in6, sockaddr_storage>(ref sourceSockAddr);

                    var v = ArrayPool<byte>.Shared.Rent(sizeof(group_source_req));

                    try
                    {
                        fixed (byte* vptr = v)
                            Buffer.MemoryCopy(&groupSourceReq, vptr, v.Length, sizeof(group_source_req));

                        socket.SetSocketOption(SocketOptionLevel.IPv6, block ? SocketOptionName.BlockSource : SocketOptionName.UnblockSource, v);
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(v);
                    }

                    return 0;
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'setInterface4'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="interf"></param>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void setInterface4(object fd, int interf)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction(() =>
            {
                InvokeWithSocket(fd, socket =>
                {
                    socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastInterface, IPAddress.HostToNetworkOrder(interf));
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'getInterface4'.
        /// </summary>
        /// <param name="fd"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static int getInterface4(object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    return IPAddress.NetworkToHostOrder((int)socket.GetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastInterface));
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'setInterface6'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="index"></param>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void setInterface6(object fd, int index)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction(() =>
            {
                InvokeWithSocket(fd, socket =>
                {
                    socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastInterface, index);
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'getInterface6'.
        /// </summary>
        /// <param name="fd"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static int getInterface6(object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    return (int)socket.GetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastInterface);
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'initIDs'.
        /// </summary>
        /// <returns></returns>
        public static void initIDs()
        {

        }

        /// <summary>
        /// Implements the native method 'pollinValue'.
        /// </summary>
        /// <returns></returns>
        public static short pollinValue() => 0x0001;

        /// <summary>
        /// Implements the native method 'polloutValue'.
        /// </summary>
        /// <returns></returns>
        public static short polloutValue() => 0x0004;

        /// <summary>
        /// Implements the native method 'pollerrValue'.
        /// </summary>
        /// <returns></returns>
        public static short pollerrValue() => 0x0008;

        /// <summary>
        /// Implements the native method 'pollhupValue'.
        /// </summary>
        /// <returns></returns>
        public static short pollhupValue() => 0x0010;

        /// <summary>
        /// Implements the native method 'pollnvalValue'.
        /// </summary>
        /// <returns></returns>
        public static short pollnvalValue() => 0x0020;

        /// <summary>
        /// Implements the native method 'pollconnValue'.
        /// </summary>
        /// <returns></returns>
        public static short pollconnValue() => 0x0002;

        /// <summary>
        /// Windows 2000 introduced a "feature" that causes it to return WSAECONNRESET from receive,
        /// if a previous send resulted in an ICMP port unreachable. For unconnected datagram sockets,
        /// we disable this feature by using this ioctl.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="enable"></param>
        internal static void SetConnectionReset(Socket socket, bool enable)
        {
            if (RuntimeUtil.IsWindows)
            {
                const int IOC_IN = unchecked((int)0x80000000);
                const int IOC_VENDOR = 0x18000000;
                const int SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
                socket.IOControl(SIO_UDP_CONNRESET, new byte[] { enable ? (byte)1 : (byte)0 }, null);
            }
        }

#if !FIRST_PASS

        /// <summary>
        /// Invokes the given action with the current socket, catching and mapping any resulting .NET exceptions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fd"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        static void InvokeWithSocket(object fd, Action<Socket> action)
        {
            var socket = FileDescriptorAccessor.GetSocket(fd);
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed.");

            action(socket);
        }

        /// <summary>
        /// Invokes the given function with the current socket, catching and mapping any resulting .NET exceptions.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="fd"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        static TResult InvokeWithSocket<TResult>(object fd, Func<Socket, TResult> func)
        {
            var socket = FileDescriptorAccessor.GetSocket(fd);
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed.");

            return func(socket);
        }

#endif

    }

}
