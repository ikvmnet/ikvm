/*
  Copyright (C) 2010-2011 Jeroen Frijters

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

package ikvm.internal;

import cli.System.Net.EndPoint;
import cli.System.Net.IPAddress;
import cli.System.Net.IPEndPoint;
import cli.System.Net.Sockets.AddressFamily;
import cli.System.Net.Sockets.IOControlCode;
import cli.System.Net.Sockets.IPv6MulticastOption;
import cli.System.Net.Sockets.LingerOption;
import cli.System.Net.Sockets.MulticastOption;
import cli.System.Net.Sockets.ProtocolType;
import cli.System.Net.Sockets.SocketFlags;
import cli.System.Net.Sockets.SocketOptionName;
import cli.System.Net.Sockets.SocketOptionLevel;
import cli.System.Net.Sockets.SocketShutdown;
import cli.System.Net.Sockets.SocketType;
import ikvm.lang.CIL;

@ikvm.lang.Internal
public final class Winsock
{
    private Winsock() { }

    // remember the last error code
    @cli.System.ThreadStaticAttribute.Annotation
    private static int lastError;

    // Error Codes
    public static final int WSA_NOT_ENOUGH_MEMORY = 8;
    public static final int WSA_OPERATION_ABORTED = 995;
    public static final int WSAEINTR = 10004;
    public static final int WSAEACCES = 10013;
    public static final int WSAEFAULT = 10014;
    public static final int WSAEINVAL = 10022;
    public static final int WSAEMFILE = 10024;
    public static final int WSAEWOULDBLOCK = 10035;
    public static final int WSAEINPROGRESS = 10036;
    public static final int WSAEALREADY = 10037;
    public static final int WSAENOTSOCK = 10038;
    public static final int WSAEDESTADDRREQ = 10039;
    public static final int WSAEMSGSIZE = 10040;
    public static final int WSAEPROTOTYPE = 10041;
    public static final int WSAENOPROTOOPT = 10042;
    public static final int WSAEPROTONOSUPPORT = 10043;
    public static final int WSAESOCKTNOSUPPORT = 10044;
    public static final int WSAEOPNOTSUPP = 10045;
    public static final int WSAEPFNOSUPPORT = 10046;
    public static final int WSAEAFNOSUPPORT = 10047;
    public static final int WSAEADDRINUSE = 10048;
    public static final int WSAEADDRNOTAVAIL = 10049;
    public static final int WSAENETDOWN = 10050;
    public static final int WSAENETUNREACH = 10051;
    public static final int WSAENETRESET = 10052;
    public static final int WSAECONNABORTED = 10053;
    public static final int WSAECONNRESET = 10054;
    public static final int WSAENOBUFS = 10055;
    public static final int WSAEISCONN = 10056;
    public static final int WSAENOTCONN = 10057;
    public static final int WSAESHUTDOWN = 10058;
    public static final int WSAETIMEDOUT = 10060;
    public static final int WSAECONNREFUSED = 10061;
    public static final int WSAEHOSTDOWN = 10064;
    public static final int WSAEHOSTUNREACH = 10065;
    public static final int WSAEPROCLIM = 10067;
    public static final int WSASYSNOTREADY = 10091;
    public static final int WSAVERNOTSUPPORTED = 10092;
    public static final int WSANOTINITIALISED = 10093;
    public static final int WSAEDISCON = 10101;
    public static final int WSATYPE_NOT_FOUND = 10109;
    public static final int WSAHOST_NOT_FOUND = 11001;
    public static final int WSATRY_AGAIN = 11002;
    public static final int WSANO_RECOVERY = 11003;
    public static final int WSANO_DATA = 11004;

    // Other constants
    public static final int SOCKET_ERROR = -1;
    public static final cli.System.Net.Sockets.Socket INVALID_SOCKET = null;

    public static final int AF_INET = AddressFamily.InterNetwork;
    public static final int AF_INET6 = AddressFamily.InterNetworkV6;

    public static final int SOCK_STREAM = SocketType.Stream;
    public static final int SOCK_DGRAM = SocketType.Dgram;

    public static final int SD_RECEIVE = SocketShutdown.Receive;
    public static final int SD_SEND = SocketShutdown.Send;
    public static final int SD_BOTH = SocketShutdown.Both;

    public static final int SOL_SOCKET = SocketOptionLevel.Socket;
    public static final int IPPROTO_TCP = SocketOptionLevel.Tcp;
    public static final int IPPROTO_IP = SocketOptionLevel.IP;
    public static final int IPPROTO_IPV6 = SocketOptionLevel.IPv6;

    public static final int TCP_NODELAY = SocketOptionName.NoDelay;
    public static final int SO_OOBINLINE = SocketOptionName.OutOfBandInline;
    public static final int SO_LINGER = SocketOptionName.Linger;
    public static final int SO_SNDBUF = SocketOptionName.SendBuffer;
    public static final int SO_RCVBUF = SocketOptionName.ReceiveBuffer;
    public static final int SO_KEEPALIVE = SocketOptionName.KeepAlive;
    public static final int SO_REUSEADDR = SocketOptionName.ReuseAddress;
    public static final int SO_EXCLUSIVEADDRUSE = SocketOptionName.ExclusiveAddressUse;
    public static final int SO_BROADCAST = SocketOptionName.Broadcast;
    public static final int SO_RCVTIMEO = SocketOptionName.ReceiveTimeout;
    public static final int SO_ERROR = SocketOptionName.Error;
    public static final int IP_MULTICAST_IF = SocketOptionName.MulticastInterface;
    public static final int IP_MULTICAST_LOOP = SocketOptionName.MulticastLoopback;
    public static final int IP_TOS = SocketOptionName.TypeOfService;
    public static final int IP_MULTICAST_TTL = SocketOptionName.MulticastTimeToLive;
    public static final int IP_ADD_MEMBERSHIP = SocketOptionName.AddMembership;
    public static final int IP_DROP_MEMBERSHIP = SocketOptionName.DropMembership;
    public static final int IPV6_MULTICAST_IF = SocketOptionName.MulticastInterface;
    public static final int IPV6_MULTICAST_LOOP = SocketOptionName.MulticastLoopback;
    public static final int IPV6_MULTICAST_HOPS = SocketOptionName.MulticastTimeToLive;
    public static final int IPV6_ADD_MEMBERSHIP = SocketOptionName.AddMembership;
    public static final int IPV6_DROP_MEMBERSHIP = SocketOptionName.DropMembership;
    public static final int IPV6_V6ONLY = 27;
    public static final int IPV6_TCLASS = 39;

    public static final int SIO_UDP_CONNRESET = 0x9800000C;

    public static final int MSG_PEEK = SocketFlags.Peek;
    public static final int MSG_OOB = SocketFlags.OutOfBand;

    public static final int FIONREAD = (int)IOControlCode.DataToRead;
    public static final int FIONBIO = (int)IOControlCode.NonBlockingIO;

    public static final int MAX_PACKET_LEN = 0xFFFF;

    public static int WSAGetLastError()
    {
        return lastError;
    }

    public static void WSASetLastError(int err)
    {
        lastError = err;
    }

    public static int WSASendDisconnect(cli.System.Net.Sockets.Socket socket)
    {
        if (socket == null)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            socket.Shutdown(SocketShutdown.wrap(SocketShutdown.Send));
            return 0;
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            lastError = x.get_ErrorCode();
            return SOCKET_ERROR;
        }
        catch (cli.System.ObjectDisposedException _)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
    }

    public static int WSAIoctl(cli.System.Net.Sockets.Socket socket, int ioControlCode, boolean optionInValue)
    {
        byte[] in = new byte[4];
        in[0] = optionInValue ? (byte)1 : (byte)0;
        byte[] out = new byte[4];
        return WSAIoctl(socket, ioControlCode, in, out);
    }

    public static int WSAIoctl(cli.System.Net.Sockets.Socket socket, int ioControlCode, byte[] optionInValue, byte[] optionOutValue)
    {
        if (socket == null)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            if (false) throw new cli.System.InvalidOperationException();
            if (ioControlCode == FIONBIO)
            {
                // it's illegal to meddle with the blocking mode via IOControl
                socket.set_Blocking(optionInValue[0] == 0);
            }
            else if (ioControlCode == FIONREAD)
            {
                int avail = socket.get_Available();
                optionOutValue[0] = (byte)(avail >> 0);
                optionOutValue[1] = (byte)(avail >> 8);
                optionOutValue[2] = (byte)(avail >> 16);
                optionOutValue[3] = (byte)(avail >> 24);
            }
            else
            {
                socket.IOControl(ioControlCode, optionInValue, optionOutValue);
            }
            return 0;
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            lastError = x.get_ErrorCode();
            return SOCKET_ERROR;
        }
        catch (cli.System.ObjectDisposedException _)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
        catch (cli.System.InvalidOperationException _)
        {
            lastError = WSAEINVAL;
            return SOCKET_ERROR;
        }
    }

    public static int ioctlsocket(cli.System.Net.Sockets.Socket s, int cmd, int[] argp)
    {
        byte[] in = cli.System.BitConverter.GetBytes(argp[0]);
        byte[] out = new byte[4];
        int ret = WSAIoctl(s, cmd, in, out);
        argp[0] = cli.System.BitConverter.ToInt32(out, 0);
        return ret;
    }

    public static int ioctlsocket(cli.System.Net.Sockets.Socket s, int cmd, int arg)
    {
        byte[] in = cli.System.BitConverter.GetBytes(arg);
        return WSAIoctl(s, cmd, in, in);
    }

    public static cli.System.Net.Sockets.Socket socket(int af, int type, int protocol)
    {
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            return new cli.System.Net.Sockets.Socket(AddressFamily.wrap(af), SocketType.wrap(type), ProtocolType.wrap(protocol));
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            lastError = x.get_ErrorCode();
            return INVALID_SOCKET;
        }
        catch (cli.System.ObjectDisposedException _)
        {
            lastError = WSAENOTSOCK;
            return INVALID_SOCKET;
        }
    }

    public static int closesocket(cli.System.Net.Sockets.Socket socket)
    {
        if (socket == null)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
        socket.Close();
        return 0;
    }

    public static final class linger
    {
        public int l_onoff;
        public int l_linger;

        LingerOption ToLingerOption()
        {
            return new LingerOption(l_onoff != 0, l_linger);
        }
    }

    public static final class ip_mreq
    {
        public final in_addr imr_multiaddr = new in_addr();
        public final in_addr imr_interface = new in_addr();

        MulticastOption ToMulticastOption()
        {
            return new MulticastOption(imr_multiaddr.ToIPAddress(), imr_interface.ToIPAddress());
        }
    }

    public static final class in_addr
    {
        public int s_addr;

        IPAddress ToIPAddress()
        {
            return new IPAddress(s_addr & 0xFFFFFFFFL);
        }
    }

    public static final class ipv6_mreq
    {
        public in6_addr ipv6mr_multiaddr;
        public int ipv6mr_interface;

        IPv6MulticastOption ToIPv6MulticastOption()
        {
            return new IPv6MulticastOption(ipv6mr_multiaddr.addr, ipv6mr_interface & 0xFFFFFFFFL);
        }
    }

    public static final class in6_addr
    {
        IPAddress addr;

        public byte[] s6_bytes()
        {
            return addr == null ? new byte[16] : addr.GetAddressBytes();
        }

        public static in6_addr FromSockAddr(IIPEndPointWrapper ep)
        {
            in6_addr addr = new in6_addr();
            addr.addr = ep.get().get_Address();
            return addr;
        }
    }

    public static int getsockopt(cli.System.Net.Sockets.Socket socket, int level, int optname, Object optval)
    {
        if (socket == null)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            Object val = socket.GetSocketOption(SocketOptionLevel.wrap(level), SocketOptionName.wrap(optname));
            if (val instanceof cli.System.Int32)
            {
                if (optval instanceof in_addr)
                {
                    ((in_addr)optval).s_addr = CIL.unbox_int(val);
                }
                else
                {
                    ((cli.System.Array)optval).SetValue(val, 0);
                }
            }
            else if (val instanceof LingerOption)
            {
                LingerOption lo = (LingerOption)val;
                linger ling = (linger)optval;
                ling.l_onoff = lo.get_Enabled() ? 1 : 0;
                // FXBUG the linger time is treated as a signed short instead of an unsiged short
                ling.l_linger = lo.get_LingerTime() & 0xFFFF;
            }
            else
            {
                lastError = WSAEINVAL;
                return SOCKET_ERROR;
            }
            return 0;
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            lastError = x.get_ErrorCode();
            return SOCKET_ERROR;
        }
        catch (cli.System.ObjectDisposedException _)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
    }

    public static int connect(cli.System.Net.Sockets.Socket socket, IIPEndPointWrapper epw)
    {
        if (socket == null)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            IPEndPoint ep = epw.get();
            if (ep == null)
            {
                // it is a disconnect request, we must connect to the Any address
                if (socket.get_AddressFamily().Value == AddressFamily.InterNetwork)
                {
                    ep = new IPEndPoint(cli.System.Net.IPAddress.Any, 0);
                }
                else
                {
                    ep = new IPEndPoint(cli.System.Net.IPAddress.IPv6Any, 0);
                }
            }
            else
            {
                ep = v4mapped(socket, ep);
            }
            if (socket.get_SocketType().Value == SocketType.Dgram)
            {
                // NOTE we use async connect to work around the issue that the .NET Socket class disallows sync Connect after the socket has received WSAECONNRESET
                socket.EndConnect(socket.BeginConnect(ep, null, null));
            }
            else
            {
                socket.Connect(ep);
            }
            return 0;
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            lastError = x.get_ErrorCode();
            return SOCKET_ERROR;
        }
        catch (cli.System.ObjectDisposedException _)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
    }

    private static IPEndPoint v4mapped(cli.System.Net.Sockets.Socket socket, IPEndPoint ep)
    {
        // when binding an IPv6 socket to an IPv4 address, we need to use a mapped v4 address
        if (socket.get_AddressFamily().Value == AF_INET6 && ep.get_AddressFamily().Value == AF_INET)
        {
            byte[] v4 = ep.get_Address().GetAddressBytes();
            if (v4[0] == 0 && v4[1] == 0 && v4[2] == 0 && v4[3] == 0)
            {
                return new IPEndPoint(IPAddress.IPv6Any, ep.get_Port());
            }
            else
            {
                byte[] v6 = new byte[16];
                v6[10] = -1;
                v6[11] = -1;
                v6[12] = v4[0];
                v6[13] = v4[1];
                v6[14] = v4[2];
                v6[15] = v4[3];
                return new IPEndPoint(new IPAddress(v6), ep.get_Port());
            }
        }
        return ep;
    }

    public static int bind(cli.System.Net.Sockets.Socket socket, IIPEndPointWrapper ep)
    {
        if (socket == null)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            socket.Bind(v4mapped(socket, ep.get()));
            return 0;
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            lastError = x.get_ErrorCode();
            return SOCKET_ERROR;
        }
        catch (cli.System.ObjectDisposedException _)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
    }

    public static int listen(cli.System.Net.Sockets.Socket socket, int count)
    {
        if (socket == null)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            socket.Listen(count);
            return 0;
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            lastError = x.get_ErrorCode();
            return SOCKET_ERROR;
        }
        catch (cli.System.ObjectDisposedException _)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
    }

    public static int shutdown(cli.System.Net.Sockets.Socket socket, int how)
    {
        if (socket == null)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            socket.Shutdown(SocketShutdown.wrap(how));
            return 0;
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            lastError = x.get_ErrorCode();
            return SOCKET_ERROR;
        }
        catch (cli.System.ObjectDisposedException _)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
    }

    public static final class fd_set
    {
        cli.System.Collections.ArrayList list = new cli.System.Collections.ArrayList();
    }

    public static final class timeval
    {
        public long tv_sec;
        public long tv_usec;
    }

    public static void FD_ZERO(fd_set set)
    {
        set.list.Clear();
    }

    public static void FD_SET(cli.System.Net.Sockets.Socket socket, fd_set set)
    {
        set.list.Add(socket);
    }

    public static boolean FD_ISSET(cli.System.Net.Sockets.Socket socket, fd_set set)
    {
        return set.list.Contains(socket);
    }

    private static cli.System.Collections.ArrayList copy(fd_set set)
    {
        return set == null ? null : (cli.System.Collections.ArrayList)set.list.Clone();
    }

    public static int select(fd_set readfds, fd_set writefds, fd_set exceptfds, timeval timeout)
    {
        long expiration;
        long current = cli.System.DateTime.get_UtcNow().get_Ticks();
        if (timeout == null)
        {
            // FXBUG documentation says that -1 will block forever, but in fact it returns immediately,
            // so we simulate timeout with a large expiration
            expiration = Long.MAX_VALUE;
        }
        else
        {
            long timeout100nanos = Math.min(Long.MAX_VALUE / 10, timeout.tv_usec) * 10 + Math.min(Long.MAX_VALUE / 10000000, timeout.tv_sec) * 10000000;
            expiration = current + Math.min(Long.MAX_VALUE - current, timeout100nanos);
        }
        try
        {
            if (false) throw new cli.System.ArgumentNullException();
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            for (; ; )
            {
                cli.System.Collections.ArrayList checkRead = copy(readfds);
                cli.System.Collections.ArrayList checkWrite = copy(writefds);
                cli.System.Collections.ArrayList checkError = copy(exceptfds);
                int microSeconds = (int)Math.min(Integer.MAX_VALUE, (expiration - current) / 10);
                cli.System.Net.Sockets.Socket.Select(checkRead, checkWrite, checkError, microSeconds);
                int count = 0;
                if (checkRead != null)
                {
                    count += checkRead.get_Count();
                }
                if (checkWrite != null)
                {
                    count += checkWrite.get_Count();
                }
                if (checkError != null)
                {
                    count += checkError.get_Count();
                }
                current = cli.System.DateTime.get_UtcNow().get_Ticks();
                if (count != 0 || current >= expiration)
                {
                    if (readfds != null)
                    {
                        readfds.list = checkRead;
                    }
                    if (writefds != null)
                    {
                        writefds.list = checkWrite;
                    }
                    if (exceptfds != null)
                    {
                        exceptfds.list = checkError;
                    }
                    return count;
                }
            }
        }
        catch (cli.System.ArgumentNullException _)
        {
            lastError = WSAEINVAL;
            return SOCKET_ERROR;
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            lastError = x.get_ErrorCode();
            return SOCKET_ERROR;
        }
        catch (cli.System.ObjectDisposedException _)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
    }

    public static int send(cli.System.Net.Sockets.Socket socket, byte[] buf, int len, int flags)
    {
        return send(socket, buf, 0, len, flags);
    }

    public static int send(cli.System.Net.Sockets.Socket socket, byte[] buf, int off, int len, int flags)
    {
        if (socket == null)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
        try
        {
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            return socket.Send(buf, off, len, SocketFlags.wrap(flags));
        }
        catch (cli.System.ArgumentException _)
        {
            lastError = WSAEINVAL;
            return SOCKET_ERROR;
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            lastError = x.get_ErrorCode();
            return SOCKET_ERROR;
        }
        catch (cli.System.ObjectDisposedException _)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
    }

    public static int recv(cli.System.Net.Sockets.Socket socket, byte[] buf, int len, int flags)
    {
        return recv(socket, buf, 0, len, flags);
    }

    public static int recv(cli.System.Net.Sockets.Socket socket, byte[] buf, int off, int len, int flags)
    {
        if (socket == null)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
        try
        {
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            return socket.Receive(buf, off, len, SocketFlags.wrap(flags));
        }
        catch (cli.System.ArgumentException _)
        {
            lastError = WSAEINVAL;
            return SOCKET_ERROR;
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            lastError = x.get_ErrorCode();
            return SOCKET_ERROR;
        }
        catch (cli.System.ObjectDisposedException _)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
    }

    public static int sendto(cli.System.Net.Sockets.Socket socket, byte[] buf, int off, int len, int flags, IIPEndPointWrapper to)
    {
        if (socket == null)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
        try
        {
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            if (to == null)
            {
                return socket.Send(buf, off, len, SocketFlags.wrap(flags));
            }
            else
            {
                return socket.SendTo(buf, off, len, SocketFlags.wrap(flags), v4mapped(socket, to.get()));
            }
        }
        catch (cli.System.ArgumentException _)
        {
            lastError = WSAEINVAL;
            return SOCKET_ERROR;
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            // on Linux we get a WSAECONNREFUSED when sending to an unreachable port/destination, so ignore that
            if (x.get_ErrorCode() == WSAECONNREFUSED)
            {
                return 0;
            }
            lastError = x.get_ErrorCode();
            return SOCKET_ERROR;
        }
        catch (cli.System.ObjectDisposedException _)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
    }

    public static int recvfrom(cli.System.Net.Sockets.Socket socket, byte[] buf, int len, int flags, IIPEndPointWrapper from)
    {
        return recvfrom(socket, buf, 0, len, flags, from);
    }

    public static int recvfrom(cli.System.Net.Sockets.Socket socket, byte[] buf, int off, int len, int flags, IIPEndPointWrapper from)
    {
        if (socket == null)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
        try
        {
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            EndPoint[] ep = new EndPoint[] { socket.get_AddressFamily().Value == AF_INET6 ? new IPEndPoint(IPAddress.IPv6Any, 0) : new IPEndPoint(0, 0) };
            try
            {
                return socket.ReceiveFrom(buf, off, len, SocketFlags.wrap(flags), ep);
            }
            finally
            {
                if (from != null)
                {
                    from.set((IPEndPoint)ep[0]);
                }
            }
        }
        catch (cli.System.ArgumentException _)
        {
            lastError = WSAEINVAL;
            return SOCKET_ERROR;
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            lastError = x.get_ErrorCode();
            return SOCKET_ERROR;
        }
        catch (cli.System.ObjectDisposedException _)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
    }

    public static int setsockopt(cli.System.Net.Sockets.Socket s, int level, int optname, Object optval)
    {
        if (s == null)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
        try
        {
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            if (optval instanceof Boolean)
            {
                s.SetSocketOption(SocketOptionLevel.wrap(level), SocketOptionName.wrap(optname), ((Boolean)optval).booleanValue());
            }
            else if (optval instanceof Integer)
            {
                s.SetSocketOption(SocketOptionLevel.wrap(level), SocketOptionName.wrap(optname), ((Integer)optval).intValue());
            }
            else if (optval instanceof linger)
            {
                s.set_LingerState(((linger)optval).ToLingerOption());
            }
            else if (optval instanceof ip_mreq)
            {
                s.SetSocketOption(SocketOptionLevel.wrap(level), SocketOptionName.wrap(optname), ((ip_mreq)optval).ToMulticastOption());
            }
            else if (optval instanceof ipv6_mreq)
            {
                s.SetSocketOption(SocketOptionLevel.wrap(level), SocketOptionName.wrap(optname), ((ipv6_mreq)optval).ToIPv6MulticastOption());
            }
            else if (optval instanceof in_addr)
            {
                s.SetSocketOption(SocketOptionLevel.wrap(level), SocketOptionName.wrap(optname), ((in_addr)optval).s_addr);
            }
            else
            {
                lastError = WSAEINVAL;
                return SOCKET_ERROR;
            }
            return 0;
        }
        catch (cli.System.ArgumentException _)
        {
            lastError = WSAEINVAL;
            return SOCKET_ERROR;
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            lastError = x.get_ErrorCode();
            return SOCKET_ERROR;
        }
        catch (cli.System.ObjectDisposedException _)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
    }

    public static cli.System.Net.Sockets.Socket accept(cli.System.Net.Sockets.Socket s, IIPEndPointWrapper ep)
    {
        if (s == null)
        {
            lastError = WSAENOTSOCK;
            return INVALID_SOCKET;
        }
        try
        {
            if (false) throw new cli.System.InvalidOperationException();
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            cli.System.Net.Sockets.Socket remote = s.Accept();
            if (ep != null)
            {
                ep.set((IPEndPoint)remote.get_RemoteEndPoint());
            }
            return remote;
        }
        catch (cli.System.ObjectDisposedException _)
        {
            lastError = WSAENOTSOCK;
            return INVALID_SOCKET;
        }
        catch (cli.System.InvalidOperationException _)
        {
            lastError = WSAEINVAL;
            return INVALID_SOCKET;
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            lastError = x.get_ErrorCode();
            return INVALID_SOCKET;
        }
    }

    public interface IIPEndPointWrapper
    {
        void set(IPEndPoint value);
        IPEndPoint get();
    }

    public static int getsockname(cli.System.Net.Sockets.Socket s, IIPEndPointWrapper name)
    {
        if (s == null)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
        try
        {
            if (false) throw new cli.System.Net.Sockets.SocketException();
            if (false) throw new cli.System.ObjectDisposedException("");
            IPEndPoint ep = (IPEndPoint)s.get_LocalEndPoint();
            if (ep == null)
            {
                lastError = WSAEINVAL;
                return SOCKET_ERROR;
            }
            name.set(ep);
            return 0;
        }
        catch (ClassCastException _)
        {
            lastError = WSAEOPNOTSUPP;
            return SOCKET_ERROR;
        }
        catch (cli.System.ObjectDisposedException _)
        {
            lastError = WSAENOTSOCK;
            return SOCKET_ERROR;
        }
        catch (cli.System.Net.Sockets.SocketException x)
        {
            lastError = x.get_ErrorCode();
            return SOCKET_ERROR;
        }
    }

    public static int ntohl(int address)
    {
        return Integer.reverseBytes(address);
    }

    public static int htonl(int address)
    {
        return Integer.reverseBytes(address);
    }

    public static int ntohs(int port)
    {
        return Short.reverseBytes((short)port) & 0xFFFF;
    }

    public static int htons(int port)
    {
        return Short.reverseBytes((short)port) & 0xFFFF;
    }
}
