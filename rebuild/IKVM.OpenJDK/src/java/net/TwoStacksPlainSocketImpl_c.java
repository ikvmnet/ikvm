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
import static ikvm.internal.JNI.*;
import static ikvm.internal.Winsock.*;
import static java.net.net_util_md.*;
import static java.net.InetAddress.IPv4;
import static java.net.InetAddress.IPv6;

final class TwoStacksPlainSocketImpl_c
{
static final int JVM_IO_ERR = -1;
static final int JVM_IO_INTR = -2;

static final int java_net_SocketOptions_SO_TIMEOUT = SocketOptions.SO_TIMEOUT;
static final int java_net_SocketOptions_SO_BINDADDR = SocketOptions.SO_BINDADDR;
static final int java_net_SocketOptions_SO_SNDBUF = SocketOptions.SO_SNDBUF;
static final int java_net_SocketOptions_SO_RCVBUF = SocketOptions.SO_RCVBUF;
static final int java_net_SocketOptions_IP_TOS = SocketOptions.IP_TOS;
static final int java_net_SocketOptions_SO_REUSEADDR = SocketOptions.SO_REUSEADDR;
static final int java_net_SocketOptions_TCP_NODELAY = SocketOptions.TCP_NODELAY;
static final int java_net_SocketOptions_SO_OOBINLINE = SocketOptions.SO_OOBINLINE;
static final int java_net_SocketOptions_SO_KEEPALIVE = SocketOptions.SO_KEEPALIVE;
static final int java_net_SocketOptions_SO_LINGER = SocketOptions.SO_LINGER;

/*
#include <windows.h>
#include <winsock2.h>
#include <ctype.h>
#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>
#include <sys/types.h>

#include "java_net_SocketOptions.h"
#include "java_net_TwoStacksPlainSocketImpl.h"
#include "java_net_InetAddress.h"
#include "java_io_FileDescriptor.h"
#include "java_lang_Integer.h"

#include "jvm.h"
#include "net_util.h"
#include "jni_util.h"
*/

/************************************************************************
 * TwoStacksPlainSocketImpl
 */

/*
static jfieldID IO_fd_fdID;

jfieldID psi_fdID;
jfieldID psi_fd1ID;
jfieldID psi_addressID;
jfieldID psi_portID;
jfieldID psi_localportID;
jfieldID psi_timeoutID;
jfieldID psi_trafficClassID;
jfieldID psi_serverSocketID;
jfieldID psi_lastfdID;
*/

/*
 * the level of the TCP protocol for setsockopt and getsockopt
 * we only want to look this up once, from the static initializer
 * of TwoStacksPlainSocketImpl
 */
static int tcp_level = -1;

static cli.System.Net.Sockets.Socket getFD(JNIEnv env, TwoStacksPlainSocketImpl _this) {
    FileDescriptor fdObj = _this.fd;

    if (fdObj == NULL) {
        return null;
    }
    return fdObj.getSocket();
}

static cli.System.Net.Sockets.Socket getFD1(JNIEnv env, TwoStacksPlainSocketImpl _this) {
    FileDescriptor fdObj = _this.fd1;

    if (fdObj == NULL) {
        return null;
    }
    return fdObj.getSocket();
}


/*
 * The initProto function is called whenever TwoStacksPlainSocketImpl is
 * loaded, to cache fieldIds for efficiency. This is called everytime
 * the Java class is loaded.
 *
 * Class:     java_net_TwoStacksPlainSocketImpl
 * Method:    initProto

 * Signature: ()V
 */
/*
JNIEXPORT void JNICALL
Java_java_net_TwoStacksPlainSocketImpl_initProto(JNIEnv *env, jclass cls) {

    struct protoent *proto = getprotobyname("TCP");
    tcp_level = (proto == 0 ? IPPROTO_TCP: proto->p_proto);

    psi_fdID = (*env)->GetFieldID(env, cls , "fd", "Ljava/io/FileDescriptor;");
    CHECK_NULL(psi_fdID);
    psi_fd1ID =(*env)->GetFieldID(env, cls , "fd1", "Ljava/io/FileDescriptor;");
    CHECK_NULL(psi_fd1ID);
    psi_addressID = (*env)->GetFieldID(env, cls, "address",
                                          "Ljava/net/InetAddress;");
    CHECK_NULL(psi_addressID);
    psi_portID = (*env)->GetFieldID(env, cls, "port", "I");
    CHECK_NULL(psi_portID);
    psi_lastfdID = (*env)->GetFieldID(env, cls, "lastfd", "I");
    CHECK_NULL(psi_portID);
    psi_localportID = (*env)->GetFieldID(env, cls, "localport", "I");
    CHECK_NULL(psi_localportID);
    psi_timeoutID = (*env)->GetFieldID(env, cls, "timeout", "I");
    CHECK_NULL(psi_timeoutID);
    psi_trafficClassID = (*env)->GetFieldID(env, cls, "trafficClass", "I");
    CHECK_NULL(psi_trafficClassID);
    psi_serverSocketID = (*env)->GetFieldID(env, cls, "serverSocket",
                                            "Ljava/net/ServerSocket;");
    CHECK_NULL(psi_serverSocketID);
    IO_fd_fdID = NET_GetFileDescriptorID(env);
    CHECK_NULL(IO_fd_fdID);
}
*/

/*
 * Class:     java_net_TwoStacksPlainSocketImpl
 * Method:    socketCreate
 * Signature: (Z)V
 */
static void socketCreate(JNIEnv env, TwoStacksPlainSocketImpl _this, boolean stream) {
    FileDescriptor fdObj = _this.fd;
    FileDescriptor fd1Obj = _this.fd1;
    cli.System.Net.Sockets.Socket fd = null;
    cli.System.Net.Sockets.Socket fd1 = null;

    if (IS_NULL(fdObj)) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "null fd object");
        return;
    }
    fd = socket(AF_INET, (stream ? SOCK_STREAM: SOCK_DGRAM), 0);
    if (fd == INVALID_SOCKET) {
        NET_ThrowCurrent(env, "create");
        return;
    } else {
        /* Set socket attribute so it is not passed to any child process */
        //SetHandleInformation((HANDLE)(UINT_PTR)fd, HANDLE_FLAG_INHERIT, FALSE);
        fdObj.setSocket(fd);
    }
    if (ipv6_available()) {

        if (IS_NULL(fd1Obj)) {
            JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                            "null fd1 object");
            fdObj.setSocket(null);
            NET_SocketClose(fd);
            return;
        }
        fd1 = socket(AF_INET6, (stream ? SOCK_STREAM: SOCK_DGRAM), 0);
        if (fd1 == INVALID_SOCKET) {
            NET_ThrowCurrent(env, "create");
            fdObj.setSocket(null);
            NET_SocketClose(fd);
            return;
        } else {
            fd1Obj.setSocket(fd1);
        }
    } else {
        _this.fd1 = null;
    }
}

/*
 * inetAddress is the address object passed to the socket connect
 * call.
 *
 * Class:     java_net_TwoStacksPlainSocketImpl
 * Method:    socketConnect
 * Signature: (Ljava/net/InetAddress;I)V
 */
static void socketConnect(JNIEnv env, TwoStacksPlainSocketImpl _this, InetAddress iaObj, int port, int timeout)
{
    int localport = _this.localport;

    /* family and localport are int fields of iaObj */
    int family;
    cli.System.Net.Sockets.Socket fd = null;
    cli.System.Net.Sockets.Socket fd1 = null;
    boolean ipv6_supported = ipv6_available();

    /* fd initially points to the IPv4 socket and fd1 to the IPv6 socket
     * If we want to connect to IPv6 then we swap the two sockets/objects
     * This way, fd is always the connected socket, and fd1 always gets closed.
     */
    FileDescriptor fdObj = _this.fd;
    FileDescriptor fd1Obj = _this.fd1;

    SOCKETADDRESS him;
    him = new SOCKETADDRESS();

    /* The result of the connection */
    int connect_res;

    if (!IS_NULL(fdObj)) {
        fd = fdObj.getSocket();
    }

    if (ipv6_supported && !IS_NULL(fd1Obj)) {
        fd1 = fd1Obj.getSocket();
    }

    if (IS_NULL(iaObj)) {
        JNU_ThrowNullPointerException(env, "inet address argument is null.");
        return;
    }

    if (NET_InetAddressToSockaddr(env, iaObj, port, him, JNI_FALSE) != 0) {
      return;
    }

    family = him.him.sa_family;
    if (family == AF_INET6) {
        if (!ipv6_supported) {
            JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                            "Protocol family not supported");
            return;
        } else {
            if (fd1 == null) {
                JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                                "Destination unreachable");
                return;
            }
            /* close the v4 socket, and set fd to be the v6 socket */
            _this.fd = fd1Obj;
            _this.fd1 = null;
            NET_SocketClose(fd);
            fd = fd1; fdObj = fd1Obj;
        }
    } else {
        if (fd1 != null) {
            fd1Obj.setSocket(null);
            NET_SocketClose(fd1);
        }
        if (fd == null) {
            JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                            "Destination unreachable");
            return;
        }
    }
    _this.fd1 = null;

    if (timeout <= 0) {
        connect_res = connect(fd, him);
        if (connect_res == SOCKET_ERROR) {
            connect_res = WSAGetLastError();
        }
    } else {
        int optval;

        /* make socket non-blocking */
        optval = 1;
        ioctlsocket( fd, FIONBIO, optval );

        /* initiate the connect */
        connect_res = connect(fd, him);
        if (connect_res == SOCKET_ERROR) {
            if (WSAGetLastError() != WSAEWOULDBLOCK) {
                connect_res = WSAGetLastError();
            } else {
                fd_set wr, ex;
                wr = new fd_set(); ex = new fd_set();
                timeval t = new timeval();

                FD_ZERO(wr);
                FD_ZERO(ex);
                FD_SET(fd, wr);
                FD_SET(fd, ex);
                t.tv_sec = timeout / 1000;
                t.tv_usec = (timeout % 1000) * 1000;

                /*
                 * Wait for timout, connection established or
                 * connection failed.
                 */
                connect_res = select(null, wr, ex, t);

                /*
                 * Timeout before connection is established/failed so
                 * we throw exception and shutdown input/output to prevent
                 * socket from being used.
                 * The socket should be closed immediately by the caller.
                 */
                if (connect_res == 0) {
                    JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketTimeoutException",
                                    "connect timed out");
                    shutdown( fd, SD_BOTH );

                     /* make socket blocking again - just in case */
                    optval = 0;
                    ioctlsocket( fd, FIONBIO, optval );
                    return;
                }

                /*
                 * We must now determine if the connection has been established
                 * or if it has failed. The logic here is designed to work around
                 * bug on Windows NT whereby using getsockopt to obtain the
                 * last error (SO_ERROR) indicates there is no error. The workaround
                 * on NT is to allow winsock to be scheduled and this is done by
                 * yielding and retrying. As yielding is problematic in heavy
                 * load conditions we attempt up to 3 times to get the error reason.
                 */
                if (!FD_ISSET(fd, ex)) {
                    connect_res = 0;
                } else {
                    int retry;
                    for (retry=0; retry<3; retry++) {
                        int[] tmp = { 0 };
                        NET_GetSockOpt(fd, SOL_SOCKET, SO_ERROR,
                                       tmp);
                        connect_res = tmp[0];
                        if (connect_res != 0) {
                            break;
                        }
                        Sleep(0);
                    }

                    if (connect_res == 0) {
                        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                                        "Unable to establish connection");
                        return;
                    }
                }
            }
        }

        /* make socket blocking again */
        optval = 0;
        ioctlsocket(fd, FIONBIO, optval);
    }

    if (connect_res != 0) {
        if (connect_res == WSAEADDRNOTAVAIL) {
            JNU_ThrowByName(env, JNU_JAVANETPKG+"ConnectException",
                "connect: Address is invalid on local machine, or port is not valid on remote machine");
        } else {
            NET_ThrowNew(env, connect_res, "connect");
        }
        return;
    }

    fdObj.setSocket(fd);

    /* set the remote peer address and port */
    _this.address = iaObj;
    _this.port = port;

    /*
     * we need to initialize the local port field if bind was called
     * previously to the connect (by the client) then localport field
     * will already be initialized
     */
    if (localport == 0) {
        /* Now that we're a connected socket, let's extract the port number
         * that the system chose for us and store it in the Socket object.
         */
        if (getsockname(fd, him) == -1) {

            if (WSAGetLastError() == WSAENOTSOCK) {
                JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "Socket closed");
            } else {
                NET_ThrowCurrent(env, "getsockname failed");
            }
            return;
        }
        port = ntohs (GET_PORT(him));
        _this.localport = port;
    }
}

/*
 * Class:     java_net_TwoStacksPlainSocketImpl
 * Method:    socketBind
 * Signature: (Ljava/net/InetAddress;I)V
 */
static void socketBind(JNIEnv env, TwoStacksPlainSocketImpl _this,
                                         InetAddress iaObj, int localport,
                                         boolean exclBind) {
    FileDescriptor fdObj = _this.fd;
    FileDescriptor fd1Obj = _this.fd1;
    cli.System.Net.Sockets.Socket fd = null;
    cli.System.Net.Sockets.Socket fd1 = null;
    boolean ipv6_supported = ipv6_available();

    /* family is an int field of iaObj */
    int family;
    int rv;

    SOCKETADDRESS him;
    him = new SOCKETADDRESS();

    family = getInetAddress_family(env, iaObj);

    if (family == IPv6 && !ipv6_supported) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "Protocol family not supported");
        return;
    }

    if (IS_NULL(fdObj) || (ipv6_supported && IS_NULL(fd1Obj))) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "Socket closed");
        return;
    } else {
        fd = fdObj.getSocket();
        if (ipv6_supported) {
            fd1 = fd1Obj.getSocket();
        }
    }
    if (IS_NULL(iaObj)) {
        JNU_ThrowNullPointerException(env, "inet address argument");
        return;
    }

    if (NET_InetAddressToSockaddr(env, iaObj, localport,
                          him, JNI_FALSE) != 0) {
      return;
    }
    if (ipv6_supported) {
        ipv6bind v6bind = new ipv6bind();
        v6bind.addr = him;
        v6bind.ipv4_fd = fd;
        v6bind.ipv6_fd = fd1;
        rv = NET_BindV6(v6bind, exclBind);
        if (rv != -1) {
            /* check if the fds have changed */
            if (v6bind.ipv4_fd != fd) {
                fd = v6bind.ipv4_fd;
                if (fd == null) {
                    /* socket is closed. */
                    _this.fd = null;
                } else {
                    /* socket was re-created */
                    fdObj.setSocket(fd);
                }
            }
            if (v6bind.ipv6_fd != fd1) {
                fd1 = v6bind.ipv6_fd;
                if (fd1 == null) {
                    /* socket is closed. */
                    _this.fd1 = null;
                } else {
                    /* socket was re-created */
                    fd1Obj.setSocket(fd1);
                }
            } else {
                /* NET_BindV6() closes both sockets upon a failure */
                _this.fd = null;
                _this.fd1 = null;
            }
        }
    } else {
        rv = NET_WinBind(fd, him, exclBind);
    }

    if (rv == -1) {
        NET_ThrowCurrent(env, "JVM_Bind");
        return;
    }

    /* set the address */
    _this.address = iaObj;

    /* intialize the local port */
    if (localport == 0) {
        /* Now that we're a bound socket, let's extract the port number
         * that the system chose for us and store it in the Socket object.
         */
        int port;
        fd = him.him.sa_family == AF_INET? fd: fd1;

        if (getsockname(fd, him) == -1) {
            NET_ThrowCurrent(env, "getsockname in plain socketBind");
            return;
        }
        port = ntohs (GET_PORT (him));

        _this.localport = port;
    } else {
        _this.localport = localport;
    }
}

/*
 * Class:     java_net_TwoStacksPlainSocketImpl
 * Method:    socketListen
 * Signature: (I)V
 */
static void socketListen (JNIEnv env, TwoStacksPlainSocketImpl _this, int count)
{
    /* this FileDescriptor fd field */
    FileDescriptor fdObj = _this.fd;
    FileDescriptor fd1Obj = _this.fd1;
    InetAddress address;
    /* fdObj's int fd field */
    cli.System.Net.Sockets.Socket fd = null;
    cli.System.Net.Sockets.Socket fd1 = null;
    SOCKETADDRESS addr = new SOCKETADDRESS();

    if (IS_NULL(fdObj) && IS_NULL(fd1Obj)) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "socket closed");
        return;
    }

    if (!IS_NULL(fdObj)) {
        fd = fdObj.getSocket();
    }
    /* Listen on V4 if address type is v4 or if v6 and address is ::0.
     * Listen on V6 if address type is v6 or if v4 and address is 0.0.0.0.
     * In cases, where we listen on one space only, we close the other socket.
     */
    address = _this.address;
    if (IS_NULL(address)) {
        JNU_ThrowNullPointerException(env, "socket address");
        return;
    }
    if (NET_InetAddressToSockaddr(env, address, 0, addr,
                                  JNI_FALSE) != 0) {
      return;
    }

    if (addr.him.sa_family == AF_INET || IN6ADDR_ISANY(addr.him6)) {
        /* listen on v4 */
        if (listen(fd, count) == -1) {
            NET_ThrowCurrent(env, "listen failed");
        }
    } else {
        NET_SocketClose (fd);
        _this.fd = null;
    }
    if (ipv6_available() && !IS_NULL(fd1Obj)) {
        fd1 = fd1Obj.getSocket();
        if (addr.him.sa_family == AF_INET6 || addr.him4.sin_addr.s_addr == INADDR_ANY) {
            /* listen on v6 */
            if (listen(fd1, count) == -1) {
                NET_ThrowCurrent(env, "listen failed");
            }
        } else {
            NET_SocketClose (fd1);
            _this.fd1 = null;
        }
    }
}

/*
 * Class:     java_net_TwoStacksPlainSocketImpl
 * Method:    socketAccept
 * Signature: (Ljava/net/SocketImpl;)V
 */
static void socketAccept(JNIEnv env, TwoStacksPlainSocketImpl _this, SocketImpl socket)
{
    /* fields on this */
    int port;
    int timeout = _this.timeout;
    FileDescriptor fdObj = _this.fd;
    FileDescriptor fd1Obj = _this.fd1;

    /* the FileDescriptor field on socket */
    FileDescriptor socketFdObj;

    /* the InetAddress field on socket */
    InetAddress socketAddressObj;

    /* the fd int field on fdObj */
    cli.System.Net.Sockets.Socket fd=null, fd1=null;

    SOCKETADDRESS him;
    him = new SOCKETADDRESS();

    if (IS_NULL(fdObj) && IS_NULL(fd1Obj)) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "Socket closed");
        return;
    }
    if (!IS_NULL(fdObj)) {
        fd = fdObj.getSocket();
    }
    if (!IS_NULL(fd1Obj)) {
        fd1 = fd1Obj.getSocket();
    }
    if (IS_NULL(socket)) {
        JNU_ThrowNullPointerException(env, "socket is null");
        return;
    } else {
        socketFdObj = socket.fd;
        socketAddressObj = socket.address;
    }
    if ((IS_NULL(socketAddressObj)) || (IS_NULL(socketFdObj))) {
        JNU_ThrowNullPointerException(env, "socket address or fd obj");
        return;
    }
    if (fd != null && fd1 != null) {
        fd_set rfds = new fd_set();
        timeval t = new timeval();
        cli.System.Net.Sockets.Socket lastfd, fd2;
        FD_ZERO(rfds);
        FD_SET(fd,rfds);
        FD_SET(fd1,rfds);
        if (timeout != 0) {
            t.tv_sec = timeout/1000;
            t.tv_usec = (timeout%1000)*1000;
        } else {
            t = null;
        }
        int res = select (rfds, null, null, t);
        if (res == 0) {
            JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketTimeoutException",
                            "Accept timed out");
            return;
        } else if (res == 1) {
            fd2 = FD_ISSET(fd, rfds)? fd: fd1;
        } else if (res == 2) {
            /* avoid starvation */
            lastfd = _this.lastfd;
            if (lastfd != null) {
                fd2 = lastfd==fd? fd1: fd;
            } else {
                fd2 = fd;
            }
            _this.lastfd = fd2;
        } else {
            JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                            "select failed");
            return;
        }
        fd = fd2;
    } else {
        int ret;
        if (fd1 != null) {
            fd = fd1;
        }
        if (timeout != 0) {
            ret = NET_Timeout(fd, timeout);
            if (ret == 0) {
                JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketTimeoutException",
                                "Accept timed out");
                return;
            } else if (ret == -1) {
                JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "socket closed");
            /* REMIND: SOCKET CLOSED PROBLEM */
    /*        NET_ThrowCurrent(env, "Accept failed"); */
                return;
            } else if (ret == -2) {
                JNU_ThrowByName(env, JNU_JAVAIOPKG+"InterruptedIOException",
                                "operation interrupted");
                return;
            }
        }
    }
    fd = accept(fd, him);
    if (fd == null) {
        /* REMIND: SOCKET CLOSED PROBLEM */
        if (false) {
            JNU_ThrowByName(env, JNU_JAVAIOPKG+"InterruptedIOException",
                            "operation interrupted");
        } else {
            JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                            "socket closed");
        }
        return;
    }
    socketFdObj.setSocket(fd);

    if (him.him.sa_family == AF_INET) {

        /*
         * fill up the remote peer port and address in the new socket structure
         */
        socketAddressObj = new Inet4Address(null, ntohl(him.him4.sin_addr.s_addr));
        socket.address = socketAddressObj;
    } else {
        /* AF_INET6 -> Inet6Address */

        // [IKVM] We need to convert scope_id 0 to -1 here, because for sin6_scope_id 0 means unspecified, whereas Java uses -1
        int scopeId = him.him6.sin6_scope_id;
        socketAddressObj = new Inet6Address(null, him.him6.sin6_addr, scopeId == 0 ? -1 : scopeId);
    }
    /* fields common to AF_INET and AF_INET6 */

    port = ntohs (GET_PORT (him));
    socket.port = port;
    port = _this.localport;
    socket.localport = port;
    socket.address = socketAddressObj;
}

/*
 * Class:     java_net_TwoStacksPlainSocketImpl
 * Method:    socketAvailable
 * Signature: ()I
 */
static int socketAvailable(JNIEnv env, TwoStacksPlainSocketImpl _this) {

    int[] available = { -1 };
    int res;
    FileDescriptor fdObj = _this.fd;
    cli.System.Net.Sockets.Socket fd;

    if (IS_NULL(fdObj)) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "Socket closed");
        return -1;
    } else {
        fd = fdObj.getSocket();
    }
    res = ioctlsocket(fd, FIONREAD, available);
    /* if result isn't 0, it means an error */
    if (res != 0) {
        NET_ThrowNew(env, res, "socket available");
    }
    return available[0];
}

/*
 * Class:     java_net_TwoStacksPlainSocketImpl
 * Method:    socketClose
 * Signature: ()V
 */
static void socketClose0(JNIEnv env, TwoStacksPlainSocketImpl _this, boolean useDeferredClose) {

    FileDescriptor fdObj = _this.fd;
    FileDescriptor fd1Obj = _this.fd1;
    cli.System.Net.Sockets.Socket fd = null;
    cli.System.Net.Sockets.Socket fd1 = null;

    if (IS_NULL(fdObj) && IS_NULL(fd1Obj)) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "socket already closed");
        return;
    }
    if (!IS_NULL(fdObj)) {
        fd = fdObj.getSocket();
    }
    if (!IS_NULL(fd1Obj)) {
        fd1 = fd1Obj.getSocket();
    }
    if (fd != null) {
        fdObj.setSocket(null);
        NET_SocketClose(fd);
    }
    if (fd1 != null) {
        fd1Obj.setSocket(null);
        NET_SocketClose(fd1);
    }
}

/*
 * Socket options for plainsocketImpl
 *
 *
 * Class:     java_net_TwoStacksPlainSocketImpl
 * Method:    socketNativeSetOption
 * Signature: (IZLjava/lang/Object;)V
 */
static void socketNativeSetOption(JNIEnv env, TwoStacksPlainSocketImpl _this, int cmd, boolean on, Object value) {
    cli.System.Net.Sockets.Socket fd, fd1;
    int[] level = new int[1];
    int[] optname = new int[1];
    Object optval;

    /*
     * Get SOCKET and check that it hasn't been closed
     */
    fd = getFD(env, _this);
    fd1 = getFD1(env, _this);
    if (fd == null && fd1 == null) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "Socket closed");
        return;
    }

    /*
     * SO_TIMEOUT is the socket option used to specify the timeout
     * for ServerSocket.accept and Socket.getInputStream().read.
     * It does not typically map to a native level socket option.
     * For Windows we special-case this and use the SOL_SOCKET/SO_RCVTIMEO
     * socket option to specify a receive timeout on the socket. This
     * receive timeout is applicable to Socket only and the socket
     * option should not be set on ServerSocket.
     */
    if (cmd == java_net_SocketOptions_SO_TIMEOUT) {

        /*
         * Don't enable the socket option on ServerSocket as it's
         * meaningless (we don't receive on a ServerSocket).
         */
        Object ssObj = _this.serverSocket;
        if (ssObj != NULL) {
            return;
        }

        /*
         * SO_RCVTIMEO is only supported on Microsoft's implementation
         * of Windows Sockets so if WSAENOPROTOOPT returned then
         * reset flag and timeout will be implemented using
         * select() -- see SocketInputStream.socketRead.
         */
        if (isRcvTimeoutSupported) {
            int timeout = ((Integer)value).intValue();

            /*
             * Disable SO_RCVTIMEO if timeout is <= 5 second.
             */
            if (timeout <= 5000) {
                timeout = 0;
            }

            if (setsockopt(fd, SOL_SOCKET, SO_RCVTIMEO, timeout) < 0) {
                if (WSAGetLastError() == WSAENOPROTOOPT) {
                    isRcvTimeoutSupported = JNI_FALSE;
                } else {
                    NET_ThrowCurrent(env, "setsockopt SO_RCVTIMEO");
                }
            }
            if (fd1 != null) {
                if (setsockopt(fd1, SOL_SOCKET, SO_RCVTIMEO, timeout) < 0) {
                    NET_ThrowCurrent(env, "setsockopt SO_RCVTIMEO");
                }
            }
        }
        return;
    }

    /*
     * Map the Java level socket option to the platform specific
     * level
     */
    if (NET_MapSocketOption(cmd, level, optname) != 0) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "Invalid option");
        return;
    }

    switch (cmd) {

        case java_net_SocketOptions_TCP_NODELAY :
        case java_net_SocketOptions_SO_OOBINLINE :
        case java_net_SocketOptions_SO_KEEPALIVE :
        case java_net_SocketOptions_SO_REUSEADDR :
            optval = on;
            break;

        case java_net_SocketOptions_SO_SNDBUF :
        case java_net_SocketOptions_SO_RCVBUF :
        case java_net_SocketOptions_IP_TOS :
            optval = ((Integer)value).intValue();
            break;

        case java_net_SocketOptions_SO_LINGER :
            {
                linger ling = new linger();
                if (on) {
                    ling.l_onoff = 1;
                    ling.l_linger = ((Integer)value).intValue();
                } else {
                    ling.l_onoff = 0;
                    ling.l_linger = 0;
                }
                optval = ling;
            }
            break;

        default: /* shouldn't get here */
            JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                "Option not supported by TwoStacksPlainSocketImpl");
            return;
    }

    if (fd != null) {
        if (NET_SetSockOpt(fd, level[0], optname[0], optval) < 0) {
            NET_ThrowCurrent(env, "setsockopt");
        }
    }

    if (fd1 != null) {
        if (NET_SetSockOpt(fd1, level[0], optname[0], optval) < 0) {
            NET_ThrowCurrent(env, "setsockopt");
        }
    }
}


/*
 * Class:     java_net_TwoStacksPlainSocketImpl
 * Method:    socketGetOption
 * Signature: (I)I
 */
static int socketGetOption(JNIEnv env, TwoStacksPlainSocketImpl _this, int opt, Object iaContainerObj) {

    cli.System.Net.Sockets.Socket fd, fd1;
    int[] level = new int[1];
    int[] optname = new int[1];
    Object optval;

    /*
     * Get SOCKET and check it hasn't been closed
     */
    fd = getFD(env, _this);
    fd1 = getFD1(env, _this);

    if (fd == null && fd1 == null) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "Socket closed");
        return -1;
    }
    if (fd == null) {
        fd = fd1;
    }

    /* For IPv6, we assume both sockets have the same setting always */

    /*
     * SO_BINDADDR isn't a socket option
     */
    if (opt == java_net_SocketOptions_SO_BINDADDR) {
        SOCKETADDRESS him;
        him = new SOCKETADDRESS();
        int[] port = { 0 };
        InetAddress iaObj;

        if (fd == null) {
            /* must be an IPV6 only socket. Case where both sockets are != -1
             * is handled in java
             */
            fd = getFD1 (env, _this);
        }

        if (getsockname(fd, him) < 0) {
            NET_ThrowByNameWithLastError(env, JNU_JAVANETPKG+"SocketException",
                             "Error getting socket name");
            return -1;
        }
        iaObj = NET_SockaddrToInetAddress(him, port);
        ((InetAddressContainer)iaContainerObj).addr = iaObj;
        return 0; /* notice change from before */
    }

    /*
     * Map the Java level socket option to the platform specific
     * level and option name.
     */
    if (NET_MapSocketOption(opt, level, optname) != 0) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "Invalid option");
        return -1;
    }

    /*
     * Args are int except for SO_LINGER
     */
    if (opt == java_net_SocketOptions_SO_LINGER) {
        optval = new linger();
    } else {
        optval = new int[1];
    }

    if (NET_GetSockOpt(fd, level[0], optname[0], optval) < 0) {
        NET_ThrowCurrent(env, "getsockopt");
        return -1;
    }

    switch (opt) {
        case java_net_SocketOptions_SO_LINGER:
            return (((linger)optval).l_onoff != 0 ? ((linger)optval).l_linger: -1);

        case java_net_SocketOptions_SO_SNDBUF:
        case java_net_SocketOptions_SO_RCVBUF:
        case java_net_SocketOptions_IP_TOS:
            return ((int[])optval)[0];

        case java_net_SocketOptions_TCP_NODELAY :
        case java_net_SocketOptions_SO_OOBINLINE :
        case java_net_SocketOptions_SO_KEEPALIVE :
        case java_net_SocketOptions_SO_REUSEADDR :
            return (((int[])optval)[0] == 0) ? -1 : 1;

        default: /* shouldn't get here */
            JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                "Option not supported by TwoStacksPlainSocketImpl");
            return -1;
    }
}

/*
 * Class:     java_net_TwoStacksPlainSocketImpl
 * Method:    socketShutdown
 * Signature: (I)V
 */
static void socketShutdown(JNIEnv env, TwoStacksPlainSocketImpl _this, int howto)
{

    FileDescriptor fdObj = _this.fd;
    cli.System.Net.Sockets.Socket fd;

    /*
     * WARNING: THIS NEEDS LOCKING. ALSO: SHOULD WE CHECK for fd being
     * -1 already?
     */
    if (IS_NULL(fdObj)) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "socket already closed");
        return;
    } else {
        fd = fdObj.getSocket();
    }
    shutdown(fd, howto);
}

/*
 * Class:     java_net_TwoStacksPlainSocketImpl
 * Method:    socketSendUrgentData
 * Signature: (B)V
 */
static void socketSendUrgentData(JNIEnv env, TwoStacksPlainSocketImpl _this, int data) {
    /* The fd field */
    FileDescriptor fdObj = _this.fd;
    int n;
    cli.System.Net.Sockets.Socket fd;

    if (IS_NULL(fdObj)) {
        JNU_ThrowByName(env, "java/net/SocketException", "Socket closed");
        return;
    } else {
        fd = fdObj.getSocket();
        /* Bug 4086704 - If the Socket associated with this file descriptor
         * was closed (sysCloseFD), the the file descriptor is set to -1.
         */
        if (fd == null) {
            JNU_ThrowByName(env, "java/net/SocketException", "Socket closed");
            return;
        }

    }
    n = send(fd, new byte[] { (byte)data }, 1, MSG_OOB);
    if (n == JVM_IO_ERR) {
        NET_ThrowCurrent(env, "send");
        return;
    }
    if (n == JVM_IO_INTR) {
        JNU_ThrowByName(env, "java/io/InterruptedIOException", null);
        return;
    }
}
}
