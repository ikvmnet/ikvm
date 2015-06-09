/*
 * Copyright (c) 1997, 2013, Oracle and/or its affiliates. All rights reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Oracle designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Oracle in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
 * or visit www.oracle.com if you need additional information or have any
 * questions.
 */

package java.net;

import java.io.FileDescriptor;
import cli.System.Net.IPAddress;
import cli.System.Net.IPEndPoint;
import static ikvm.internal.JNI.*;
import static ikvm.internal.Winsock.*;

final class net_util_md
{
    private net_util_md() { }

    static final int INADDR_ANY = 0;

    static final int IPTOS_TOS_MASK = 0x1e;
    static final int IPTOS_PREC_MASK = 0xe0;

    static boolean isRcvTimeoutSupported = true;

    /*
     * Table of Windows Sockets errors, the specific exception we
     * throw for the error, and the error text.
     *
     * Note that this table excludes OS dependent errors.
     *
     * Latest list of Windows Sockets errors can be found at :-
     * http://msdn.microsoft.com/library/psdk/winsock/errors_3wc2.htm
     */
    private static class WinsockError
    {
        final int errCode;
        final int exc;
        final String errString;

        WinsockError(int errCode, int exc, String errString)
        {
            this.errCode = errCode;
            this.exc = exc;
            this.errString = errString;
        }
    }

    private static final int Exception_BindException = 1;
    private static final int Exception_ConnectException = 2;
    private static final int Exception_NoRouteToHostException = 3;

    private static final WinsockError[] winsock_errors = {
        new WinsockError(WSAEACCES,                0,      "Permission denied"),
        new WinsockError(WSAEADDRINUSE,            Exception_BindException,        "Address already in use"),
        new WinsockError(WSAEADDRNOTAVAIL,         Exception_BindException,        "Cannot assign requested address"),
        new WinsockError(WSAEAFNOSUPPORT,          0,      "Address family not supported by protocol family"),
        new WinsockError(WSAEALREADY,              0,      "Operation already in progress"),
        new WinsockError(WSAECONNABORTED,          0,      "Software caused connection abort"),
        new WinsockError(WSAECONNREFUSED,          Exception_ConnectException,     "Connection refused"),
        new WinsockError(WSAECONNRESET,            0,      "Connection reset by peer"),
        new WinsockError(WSAEDESTADDRREQ,          0,      "Destination address required"),
        new WinsockError(WSAEFAULT,                0,      "Bad address"),
        new WinsockError(WSAEHOSTDOWN,             0,      "Host is down"),
        new WinsockError(WSAEHOSTUNREACH,          Exception_NoRouteToHostException,       "No route to host"),
        new WinsockError(WSAEINPROGRESS,           0,      "Operation now in progress"),
        new WinsockError(WSAEINTR,                 0,      "Interrupted function call"),
        new WinsockError(WSAEINVAL,                0,      "Invalid argument"),
        new WinsockError(WSAEISCONN,               0,      "Socket is already connected"),
        new WinsockError(WSAEMFILE,                0,      "Too many open files"),
        new WinsockError(WSAEMSGSIZE,              0,      "The message is larger than the maximum supported by the underlying transport"),
        new WinsockError(WSAENETDOWN,              0,      "Network is down"),
        new WinsockError(WSAENETRESET,             0,      "Network dropped connection on reset"),
        new WinsockError(WSAENETUNREACH,           0,      "Network is unreachable"),
        new WinsockError(WSAENOBUFS,               0,      "No buffer space available (maximum connections reached?)"),
        new WinsockError(WSAENOPROTOOPT,           0,      "Bad protocol option"),
        new WinsockError(WSAENOTCONN,              0,      "Socket is not connected"),
        new WinsockError(WSAENOTSOCK,              0,      "Socket operation on nonsocket"),
        new WinsockError(WSAEOPNOTSUPP,            0,      "Operation not supported"),
        new WinsockError(WSAEPFNOSUPPORT,          0,      "Protocol family not supported"),
        new WinsockError(WSAEPROCLIM,              0,      "Too many processes"),
        new WinsockError(WSAEPROTONOSUPPORT,       0,      "Protocol not supported"),
        new WinsockError(WSAEPROTOTYPE,            0,      "Protocol wrong type for socket"),
        new WinsockError(WSAESHUTDOWN,             0,      "Cannot send after socket shutdown"),
        new WinsockError(WSAESOCKTNOSUPPORT,       0,      "Socket type not supported"),
        new WinsockError(WSAETIMEDOUT,             Exception_ConnectException,     "Connection timed out"),
        new WinsockError(WSATYPE_NOT_FOUND,        0,      "Class type not found"),
        new WinsockError(WSAEWOULDBLOCK,           0,      "Resource temporarily unavailable"),
        new WinsockError(WSAHOST_NOT_FOUND,        0,      "Host not found"),
        new WinsockError(WSA_NOT_ENOUGH_MEMORY,    0,      "Insufficient memory available"),
        new WinsockError(WSANOTINITIALISED,        0,      "Successful WSAStartup not yet performed"),
        new WinsockError(WSANO_DATA,               0,      "Valid name, no data record of requested type"),
        new WinsockError(WSANO_RECOVERY,           0,      "This is a nonrecoverable error"),
        new WinsockError(WSASYSNOTREADY,           0,      "Network subsystem is unavailable"),
        new WinsockError(WSATRY_AGAIN,             0,      "Nonauthoritative host not found"),
        new WinsockError(WSAVERNOTSUPPORTED,       0,      "Winsock.dll version out of range"),
        new WinsockError(WSAEDISCON,               0,      "Graceful shutdown in progress"),
        new WinsockError(WSA_OPERATION_ABORTED,    0,      "Overlapped operation aborted")
    };

    /*
     * Since winsock doesn't have the equivalent of strerror(errno)
     * use table to lookup error text for the error.
     */
    static SocketException NET_ThrowNew(int errorNum, String msg)
    {
        int i;
        int table_size = winsock_errors.length;
        int excP = 0;
        String fullMsg;

        if (msg == null) {
            msg = "no further information";
        }

        /*
         * Check table for known winsock errors
         */
        i=0;
        while (i < table_size) {
            if (errorNum == winsock_errors[i].errCode) {
                break;
            }
            i++;
        }

        /*
         * If found get pick the specific exception and error
         * message corresponding to this error.
         */
        if (i < table_size) {
            excP = winsock_errors[i].exc;
            fullMsg = winsock_errors[i].errString + ": " + msg;
        } else {
            fullMsg = "Unrecognized Windows Sockets error: " + errorNum + ": " + msg;
        }

        /*
         * Throw SocketException if no specific exception for this
         * error.
         */
        switch (excP) {
            case Exception_BindException:
                return new BindException(fullMsg);
            case Exception_ConnectException:
                return new ConnectException(fullMsg);
            case Exception_NoRouteToHostException:
                return new NoRouteToHostException(fullMsg);
            default:
                return new SocketException(fullMsg);
        }
    }

    static void NET_ThrowNew(JNIEnv env, int errorNum, String msg)
    {
        env.Throw(NET_ThrowNew(errorNum, msg));
    }

    static SocketException NET_ThrowCurrent(String msg)
    {
        return NET_ThrowNew(WSAGetLastError(), msg);
    }

    static void NET_ThrowCurrent(JNIEnv env, String msg)
    {
        env.Throw(NET_ThrowCurrent(msg));
    }

    /*
     * Return the default TOS value
     */
    static int NET_GetDefaultTOS() {
        // we always use the "default" default...
        return 0;
    }

    /*
     * Map the Java level socket option to the platform specific
     * level and option name.
     */

    private static final class sockopts {
        int cmd;
        int level;
        int optname;

        sockopts(int cmd, int level, int optname) {
            this.cmd = cmd;
            this.level = level;
            this.optname = optname;
        }
    }

    private static final sockopts opts[] = {
        new sockopts(SocketOptions.TCP_NODELAY,   IPPROTO_TCP,    TCP_NODELAY ),
        new sockopts(SocketOptions.SO_OOBINLINE,  SOL_SOCKET,     SO_OOBINLINE ),
        new sockopts(SocketOptions.SO_LINGER,     SOL_SOCKET,     SO_LINGER ),
        new sockopts(SocketOptions.SO_SNDBUF,     SOL_SOCKET,     SO_SNDBUF ),
        new sockopts(SocketOptions.SO_RCVBUF,     SOL_SOCKET,     SO_RCVBUF ),
        new sockopts(SocketOptions.SO_KEEPALIVE,  SOL_SOCKET,     SO_KEEPALIVE ),
        new sockopts(SocketOptions.SO_REUSEADDR,  SOL_SOCKET,     SO_REUSEADDR ),
        new sockopts(SocketOptions.SO_BROADCAST,  SOL_SOCKET,     SO_BROADCAST ),
        new sockopts(SocketOptions.IP_MULTICAST_IF,   IPPROTO_IP, IP_MULTICAST_IF ),
        new sockopts(SocketOptions.IP_MULTICAST_LOOP, IPPROTO_IP, IP_MULTICAST_LOOP ),
        new sockopts(SocketOptions.IP_TOS,            IPPROTO_IP, IP_TOS ),

    };

    /* call NET_MapSocketOptionV6 for the IPv6 fd only
     * and NET_MapSocketOption for the IPv4 fd
     */
    static int NET_MapSocketOptionV6(int cmd, int[] level, int[] optname) {

        switch (cmd) {
            case SocketOptions.IP_MULTICAST_IF:
            case SocketOptions.IP_MULTICAST_IF2:
                level[0] = IPPROTO_IPV6;
                optname[0] = IPV6_MULTICAST_IF;
                return 0;

            case SocketOptions.IP_MULTICAST_LOOP:
                level[0] = IPPROTO_IPV6;
                optname[0] = IPV6_MULTICAST_LOOP;
                return 0;
        }
        return NET_MapSocketOption (cmd, level, optname);
    }

    static int NET_MapSocketOption(int cmd, int[] level, int[] optname) {
        /*
         * Map the Java level option to the native level
         */
        for (int i=0; i<opts.length; i++) {
            if (cmd == opts[i].cmd) {
                level[0] = opts[i].level;
                optname[0] = opts[i].optname;
                return 0;
            }
        }

        /* not found */
        return -1;
    }

    static int NET_SetSockOpt(cli.System.Net.Sockets.Socket s, int level, int optname, Object optval)
    {
        int rv;

        if (level == IPPROTO_IP && optname == IP_TOS) {
            int tos = (Integer)optval;
            tos &= (IPTOS_TOS_MASK | IPTOS_PREC_MASK);
            optval = tos;
        }

        if (optname == SO_REUSEADDR) {
            /*
             * Do not set SO_REUSEADDE if SO_EXCLUSIVEADDUSE is already set
             */
            int[] parg = new int[1];
            rv = NET_GetSockOpt(s, SOL_SOCKET, SO_EXCLUSIVEADDRUSE, parg);
            if (rv == 0 && parg[0] == 1) {
                return rv;
            }
        }

        rv = setsockopt(s, level, optname, optval);

        if (rv == SOCKET_ERROR) {
            /*
             * IP_TOS & IP_MULTICAST_LOOP can't be set on some versions
             * of Windows.
             */
            if ((WSAGetLastError() == WSAENOPROTOOPT) &&
                (level == IPPROTO_IP) &&
                (optname == IP_TOS || optname == IP_MULTICAST_LOOP)) {
                rv = 0;
            }

            /*
             * IP_TOS can't be set on unbound UDP sockets.
             */
            if ((WSAGetLastError() == WSAEINVAL) &&
                (level == IPPROTO_IP) &&
                (optname == IP_TOS)) {
                rv = 0;
            }
        }

        return rv;
    }

    /*
     * Wrapper for setsockopt dealing with Windows specific issues :-
     *
     * IP_TOS is not supported on some versions of Windows so
     * instead return the default value for the OS.
     */
    static int NET_GetSockOpt(cli.System.Net.Sockets.Socket s, int level, int optname, Object optval)
    {
        int rv;

        if (level == IPPROTO_IPV6 && optname == IPV6_TCLASS) {
            ((int[])optval)[0] = 0;
            return 0;
        }

        rv = getsockopt(s, level, optname, optval);


        /*
         * IPPROTO_IP/IP_TOS is not supported on some Windows
         * editions so return the default type-of-service
         * value.
         */
        if (rv == SOCKET_ERROR) {

            if (WSAGetLastError() == WSAENOPROTOOPT &&
                level == IPPROTO_IP && optname == IP_TOS) {

                ((int[])optval)[0] = NET_GetDefaultTOS();

                rv = 0;
            }
        }

        return rv;
    }

    /*
     * Sets SO_ECLUSIVEADDRUSE if SO_REUSEADDR is not already set.
     */
    static void setExclusiveBind(cli.System.Net.Sockets.Socket fd) {
        int[] parg = new int[1];
        int rv = 0;
        rv = NET_GetSockOpt(fd, SOL_SOCKET, SO_REUSEADDR, parg);
        if (rv == 0 && parg[0] == 0) {
            rv = NET_SetSockOpt(fd, SOL_SOCKET, SO_EXCLUSIVEADDRUSE, 1);
        }
    }

    /*
     * Wrapper for bind winsock call - transparent converts an
     * error related to binding to a port that has exclusive access
     * into an error indicating the port is in use (facilitates
     * better error reporting).
     */
    static int NET_Bind(cli.System.Net.Sockets.Socket s, SOCKETADDRESS him)
    {
        int rv;
        rv = bind(s, him);

        if (rv == SOCKET_ERROR) {
            /*
             * If bind fails with WSAEACCES it means that a privileged
             * process has done an exclusive bind (NT SP4/2000/XP only).
             */
            if (WSAGetLastError() == WSAEACCES) {
                WSASetLastError(WSAEADDRINUSE);
            }
        }

        return rv;
    }

    /*
     * Wrapper for NET_Bind call. Sets SO_EXCLUSIVEADDRUSE
     * if required, and then calls NET_BIND
     */
    static int NET_WinBind(cli.System.Net.Sockets.Socket s, SOCKETADDRESS him, boolean exclBind)
    {
        if (exclBind == JNI_TRUE)
            setExclusiveBind(s);
        return NET_Bind(s, him);
    }

    static int NET_SocketClose(cli.System.Net.Sockets.Socket fd) {
        linger l = new linger();
        int ret;
        if (getsockopt(fd, SOL_SOCKET, SO_LINGER, l) == 0) {
            if (l.l_onoff == 0) {
                WSASendDisconnect(fd);
            }
        }
        ret = closesocket (fd);
        return ret;
    }

    static int NET_Timeout(cli.System.Net.Sockets.Socket fd, long timeout) {
        int ret;
        fd_set tbl = new fd_set();
        timeval t = new timeval();
        t.tv_sec = timeout / 1000;
        t.tv_usec = (timeout % 1000) * 1000;
        FD_ZERO(tbl);
        FD_SET(fd, tbl);
        ret = select (tbl, null, null, t);
        return ret;
    }

    /*
     * differs from NET_Timeout() as follows:
     *
     * If timeout = -1, it blocks forever.
     *
     * returns 1 or 2 depending if only one or both sockets
     * fire at same time.
     *
     * *fdret is (one of) the active fds. If both sockets
     * fire at same time, *fdret = fd always.
     */
    static int NET_Timeout2(cli.System.Net.Sockets.Socket fd, cli.System.Net.Sockets.Socket fd1, long timeout, cli.System.Net.Sockets.Socket[] fdret) {
        int ret;
        fd_set tbl = new fd_set();
        timeval t = new timeval();
        if (timeout == -1) {
            t = null;
        } else {
            t.tv_sec = timeout / 1000;
            t.tv_usec = (timeout % 1000) * 1000;
        }
        FD_ZERO(tbl);
        FD_SET(fd, tbl);
        FD_SET(fd1, tbl);
        ret = select (tbl, null, null, t);
        switch (ret) {
        case 0:
            return 0; /* timeout */
        case 1:
            if (FD_ISSET (fd, tbl)) {
                fdret[0]= fd;
            } else {
                fdret[0]= fd1;
            }
            return 1;
        case 2:
            fdret[0]= fd;
            return 2;
        }
        return -1;
    }

    /*
     * if ipv6 is available, call NET_BindV6 to bind to the required address/port.
     * Because the same port number may need to be reserved in both v4 and v6 space,
     * this may require socket(s) to be re-opened. Therefore, all of this information
     * is passed in and returned through the ipv6bind structure.
     *
     * If the request is to bind to a specific address, then this (by definition) means
     * only bind in either v4 or v6, and this is just the same as normal. ie. a single
     * call to bind() will suffice. The other socket is closed in this case.
     *
     * The more complicated case is when the requested address is ::0 or 0.0.0.0.
     *
     * Two further cases:
     * 2. If the reqeusted port is 0 (ie. any port) then we try to bind in v4 space
     *    first with a wild-card port argument. We then try to bind in v6 space
     *    using the returned port number. If this fails, we repeat the process
     *    until a free port common to both spaces becomes available.
     *
     * 3. If the requested port is a specific port, then we just try to get that
     *    port in both spaces, and if it is not free in both, then the bind fails.
     *
     * On failure, sockets are closed and an error returned with CLOSE_SOCKETS_AND_RETURN
     */

    static class ipv6bind {
        SOCKETADDRESS addr;
        cli.System.Net.Sockets.Socket ipv4_fd;
        cli.System.Net.Sockets.Socket ipv6_fd;
    }

    private static int CLOSE_SOCKETS_AND_RETURN(
        cli.System.Net.Sockets.Socket fd,
        cli.System.Net.Sockets.Socket ofd,
        cli.System.Net.Sockets.Socket close_fd,
        cli.System.Net.Sockets.Socket close_ofd,
        ipv6bind b) {
        if (fd != null) {
            closesocket (fd);
        }
        if (ofd != null) {
            closesocket (ofd);
        }
        if (close_fd != null) {
            closesocket (close_fd);
        }
        if (close_ofd != null) {
            closesocket (close_ofd);
        }
        b.ipv4_fd = b.ipv6_fd = null;
        return SOCKET_ERROR;
    }

    static int NET_BindV6(ipv6bind b, boolean exclBind) {
        cli.System.Net.Sockets.Socket fd = null;
        cli.System.Net.Sockets.Socket ofd = null;
        int rv;
        /* need to defer close until new sockets created */
        cli.System.Net.Sockets.Socket close_fd = null;
        cli.System.Net.Sockets.Socket close_ofd = null;

        SOCKETADDRESS oaddr = new SOCKETADDRESS();
        int family = b.addr.him.sa_family;
        int ofamily;
        int port;
        int bound_port;

        if (family == AF_INET && (b.addr.him4.sin_addr.s_addr != INADDR_ANY)) {
            /* bind to v4 only */
            int ret;
            ret = NET_WinBind (b.ipv4_fd, b.addr, exclBind);
            if (ret == SOCKET_ERROR) {
                return CLOSE_SOCKETS_AND_RETURN(fd, ofd, close_fd, close_ofd, b);
            }
            closesocket (b.ipv6_fd);
            b.ipv6_fd = null;
            return 0;
        }
        if (family == AF_INET6 && (!IN6ADDR_ISANY(b.addr))) {
            /* bind to v6 only */
            int ret;
            ret = NET_WinBind (b.ipv6_fd, b.addr, exclBind);
            if (ret == SOCKET_ERROR) {
                return CLOSE_SOCKETS_AND_RETURN(fd, ofd, close_fd, close_ofd, b);
            }
            closesocket (b.ipv4_fd);
            b.ipv4_fd = null;
            return 0;
        }

        /* We need to bind on both stacks, with the same port number */

        if (family == AF_INET) {
            ofamily = AF_INET6;
            fd = b.ipv4_fd;
            ofd = b.ipv6_fd;
            port = GET_PORT (b.addr);
            oaddr.set(new IPEndPoint(IPAddress.IPv6Any, htons(port)));
        } else {
            ofamily = AF_INET;
            ofd = b.ipv4_fd;
            fd = b.ipv6_fd;
            port = GET_PORT (b.addr);
            oaddr.set(new IPEndPoint(IPAddress.Any, htons(port)));
        }

        rv = NET_WinBind (fd, b.addr, exclBind);
        if (rv == SOCKET_ERROR) {
            return CLOSE_SOCKETS_AND_RETURN(fd, ofd, close_fd, close_ofd, b);
        }

        /* get the port and set it in the other address */
        if (getsockname(fd, b.addr) == -1) {
            return CLOSE_SOCKETS_AND_RETURN(fd, ofd, close_fd, close_ofd, b);
        }
        bound_port = b.addr.sin_port;
        oaddr.sin_port = bound_port;
        if ((rv=NET_Bind (ofd, oaddr)) == SOCKET_ERROR) {

            /* no retries unless, the request was for any free port */

            if (port != 0) {
                return CLOSE_SOCKETS_AND_RETURN(fd, ofd, close_fd, close_ofd, b);
            }

            int sotype = fd.get_SocketType().Value;

            /* 50 is an arbitrary limit, just to ensure that this
             * cannot be an endless loop. Would expect socket creation to
             * succeed sooner.
             */
            for (int retries = 0; retries < 50 /*SOCK_RETRIES*/; retries ++) {
                close_fd = fd;
                fd = null;
                close_ofd = ofd;
                ofd = null;
                b.ipv4_fd = null;
                b.ipv6_fd = null;

                /* create two new sockets */
                fd = socket (family, sotype, 0);
                if (fd == INVALID_SOCKET) {
                    return CLOSE_SOCKETS_AND_RETURN(fd, ofd, close_fd, close_ofd, b);
                }
                ofd = socket (ofamily, sotype, 0);
                if (ofd == INVALID_SOCKET) {
                    return CLOSE_SOCKETS_AND_RETURN(fd, ofd, close_fd, close_ofd, b);
                }

                /* bind random port on first socket */
                oaddr.sin_port = 0;
                rv = NET_WinBind (ofd, oaddr,
                                  exclBind);
                if (rv == SOCKET_ERROR) {
                    return CLOSE_SOCKETS_AND_RETURN(fd, ofd, close_fd, close_ofd, b);
                }
                /* close the original pair of sockets before continuing */
                closesocket (close_fd);
                closesocket (close_ofd);
                close_fd = close_ofd = null;

                /* bind new port on second socket */
                if (getsockname(ofd, oaddr) == -1) {
                    return CLOSE_SOCKETS_AND_RETURN(fd, ofd, close_fd, close_ofd, b);
                }
                bound_port = oaddr.sin_port;
                b.addr.sin_port = bound_port;
                rv = NET_WinBind (fd, b.addr,
                                  exclBind);

                if (rv != SOCKET_ERROR) {
                    if (family == AF_INET) {
                        b.ipv4_fd = fd;
                        b.ipv6_fd = ofd;
                    } else {
                        b.ipv4_fd = ofd;
                        b.ipv6_fd = fd;
                    }
                    return 0;
                }
            }
            return CLOSE_SOCKETS_AND_RETURN(fd, ofd, close_fd, close_ofd, b);
        }
        return 0;
    }

    /* If address types is IPv6, then IPv6 must be available. Otherwise
     * no address can be generated. In the case of an IPv4 Inetaddress this
     * method will return an IPv4 mapped address where IPv6 is available and
     * v4MappedAddress is TRUE. Otherwise it will return a sockaddr_in
     * structure for an IPv4 InetAddress.
    */
    static int NET_InetAddressToSockaddr(JNIEnv env, InetAddress iaObj, int port, SOCKETADDRESS him, boolean v4MappedAddress) {
        if (iaObj.holder().family == InetAddress.IPv4) {
            him.set(new IPEndPoint(new IPAddress(htonl(iaObj.holder().address) & 0xFFFFFFFFL), port));
            return 0;
        } else {
            Inet6Address v6addr = (Inet6Address)iaObj;
            int scope = v6addr.getScopeId();
            if (scope == 0) {
                him.set(new IPEndPoint(new IPAddress(v6addr.getAddress()), port));
                return 0;
            } else {
                him.set(new IPEndPoint(new IPAddress(v6addr.getAddress(), scope & 0xFFFFFFFFL), port));
                return 0;
            }
        }
    }

    static int NET_GetPortFromSockaddr(SOCKETADDRESS him) {
        return ntohs(GET_PORT(him));
    }

    static boolean NET_IsIPv4Mapped(byte[] caddr) {
        int i;
        for (i = 0; i < 10; i++) {
            if (caddr[i] != 0x00) {
                return false;
            }
        }

        if (((caddr[10] & 0xff) == 0xff) && ((caddr[11] & 0xff) == 0xff)) {
            return true;
        }
        return false;
    }

    static int NET_IPv4MappedToIPv4(byte[] caddr) {
        return ((caddr[12] & 0xff) << 24) | ((caddr[13] & 0xff) << 16) | ((caddr[14] & 0xff) << 8)
            | (caddr[15] & 0xff);
    }

    static boolean NET_IsEqual(byte[] caddr1, byte[] caddr2) {
        int i;
        for (i = 0; i < 16; i++) {
            if (caddr1[i] != caddr2[i]) {
                return false;
            }
        }
        return true;
    }

    static int getScopeID (SOCKETADDRESS him) {
        return him.him6.sin6_scope_id;
    }

    static boolean cmpScopeID (int scope, SOCKETADDRESS him) {
        return him.him6.sin6_scope_id == scope;
    }

    /* these methods are not from net_util_md.c */

    static boolean ipv6_available() {
        return InetAddressImplFactory.isIPv6Supported();
    }

    static boolean IS_NULL(Object obj) {
        return obj == null;
    }

    static boolean IN6ADDR_ISANY(SOCKETADDRESS him) {
        if (him.sa_family != AF_INET6) {
            return false;
        }
        byte[] addr = him.him6.sin6_addr;
        byte b = 0;
        for (int i = 0; i < addr.length; i++) {
            b |= addr[i];
        }
        return b == 0;
    }

    static boolean NET_SockaddrEqualsInetAddress(SOCKETADDRESS him, InetAddress iaObj) {
        int family = iaObj.holder().family == InetAddress.IPv4 ? AF_INET : AF_INET6;

        if (him.sa_family == AF_INET6) {
            byte[] caddrNew = him.him6.sin6_addr;
                if (NET_IsIPv4Mapped(caddrNew)) {
                    int addrNew;
                    int addrCur;
                    if (family == AF_INET6) {
                        return false;
                    }
                    addrNew = NET_IPv4MappedToIPv4(caddrNew);
                    addrCur = iaObj.holder().address;
                    if (addrNew == addrCur) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    byte[] caddrCur;
                    int scope;

                    if (family == AF_INET) {
                        return false;
                    }
                    scope = ((Inet6Address)iaObj).getScopeId();
                    caddrCur = ((Inet6Address)iaObj).getAddress();
                    if (NET_IsEqual(caddrNew, caddrCur) && cmpScopeID(scope, him)) {
                        return true;
                    } else {
                        return false;
                    }
                }
        } else {
            int addrNew, addrCur;
            if (family != AF_INET) {
                return false;
            }
            addrNew = ntohl(him.him4.sin_addr.s_addr);
            addrCur = iaObj.holder().address;
            if (addrNew == addrCur) {
                return true;
            } else {
                return false;
            }
        }
    }

    static InetAddress NET_SockaddrToInetAddress(JNIEnv env, SOCKETADDRESS him, int[] port) {
        return NET_SockaddrToInetAddress(him, port);
    }

    static InetAddress NET_SockaddrToInetAddress(SOCKETADDRESS him, int[] port) {
        InetAddress iaObj;
        if (him.sa_family == AF_INET6) {
            byte[] caddr = him.him6.sin6_addr;
            if (NET_IsIPv4Mapped(caddr)) {
                iaObj = new Inet4Address(null, NET_IPv4MappedToIPv4(caddr));
            } else {
                int scope = getScopeID(him);
                iaObj = new Inet6Address(null, caddr, scope > 0 ? scope : -1);
            }
            port[0] = ntohs(him.him6.sin_port);
        } else {
            iaObj = new Inet4Address(null, ntohl(him.him4.sin_addr.s_addr));
            port[0] = ntohs(him.him4.sin_port);
        }
        return iaObj;
    }

    static void NET_ThrowByNameWithLastError(JNIEnv env, String exceptionClass, String message) {
        JNU_ThrowByName(env, exceptionClass, "errno: " + WSAGetLastError() + ", error: " + message + "\n");
    }

    static boolean IN_MULTICAST(int ipv4address) {
        return ((ipv4address >> 24) & 0xf0) == 0xe0;
    }

    static boolean IN6_IS_ADDR_MULTICAST(in6_addr address) {
        return (address.s6_bytes()[0] & 0xff) == 0xff;
    }

    static final class SOCKETADDRESS implements IIPEndPointWrapper {
        final SOCKETADDRESS him = this;
        final SOCKETADDRESS him4 = this;
        final SOCKETADDRESS him6 = this;
        final SOCKETADDRESS sin_addr = this;
        int sa_family;
        int sin_port;
        int s_addr;
        byte[] sin6_addr;
        int sin6_scope_id;

        public void set(IPEndPoint ep) {
            if (ep == null) {
                sa_family = 0;
                sin_port = 0;
                s_addr = 0;
                sin6_addr = null;
                sin6_scope_id = 0;
            } else {
                sa_family = ep.get_AddressFamily().Value;
                sin_port = htons(ep.get_Port());
                if (sa_family == AF_INET) {
                    s_addr = (int)ep.get_Address().get_Address();
                    sin6_addr = null;
                    sin6_scope_id = 0;
                } else {
                    s_addr = 0;
                    IPAddress addr = ep.get_Address();
                    sin6_addr = addr.GetAddressBytes();
                    sin6_scope_id = (int)addr.get_ScopeId();
                }
            }
        }

        public IPEndPoint get() {
            if (sa_family == AF_INET) {
                return new IPEndPoint(new IPAddress(s_addr & 0xFFFFFFFFL), ntohs(sin_port));
            } else if (sa_family == AF_INET6) {
                IPAddress addr;
                if (sin6_addr == null) {
                    addr = IPAddress.IPv6Any;
                } else {
                    addr = new IPAddress(sin6_addr, sin6_scope_id & 0xFFFFFFFFL);
                }
                return new IPEndPoint(addr, ntohs(sin_port));
            } else {
                return null;
            }
        }
    }

    static int GET_PORT(SOCKETADDRESS sockaddr) {
        return sockaddr.sin_port;
    }

    static void Sleep(int ms) {
        cli.System.Threading.Thread.Sleep(ms);
    }

    static cli.System.Net.Sockets.Socket NET_Socket (int domain, int type, int protocol) {
        cli.System.Net.Sockets.Socket sock;
        sock = socket (domain, type, protocol);
        if (sock != INVALID_SOCKET) {
            //SetHandleInformation((HANDLE)(uintptr_t)sock, HANDLE_FLAG_INHERIT, FALSE);
        }
        return sock;
    }

    static int getInetAddress_addr(JNIEnv env, InetAddress iaObj) {
        return iaObj.holder().address;
    }

    static int getInetAddress_family(JNIEnv env, InetAddress iaObj) {
        return iaObj.holder().family;
    }
}
