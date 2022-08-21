/*
 * Copyright (c) 2007, 2013, Oracle and/or its affiliates. All rights reserved.
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

final class DualStackPlainSocketImpl_c
{
private static final int JVM_IO_ERR = -1;
private static final int JVM_IO_INTR = -2;

private static final int SET_BLOCKING = 0;
private static final int SET_NONBLOCKING = 1;
/*
#include <windows.h>
#include <winsock2.h>
#include "jni.h"
#include "net_util.h"
#include "java_net_DualStackPlainSocketImpl.h"

#define SET_BLOCKING 0
#define SET_NONBLOCKING 1

static jclass isa_class;        /* java.net.InetSocketAddress *-/
static jmethodID isa_ctorID;    /* InetSocketAddress(InetAddress, int) *-/

/*
 * Class:     java_net_DualStackPlainSocketImpl
 * Method:    initIDs
 * Signature: ()V
 *-/
JNIEXPORT void JNICALL Java_java_net_DualStackPlainSocketImpl_initIDs
  (JNIEnv *env, jclass clazz) {

    jclass cls = (*env)->FindClass(env, "java/net/InetSocketAddress");
    CHECK_NULL(cls);
    isa_class = (*env)->NewGlobalRef(env, cls);
    isa_ctorID = (*env)->GetMethodID(env, cls, "<init>",
                                     "(Ljava/net/InetAddress;I)V");

    // implement read timeout with select.
    isRcvTimeoutSupported = 0;
}

/*
 * Class:     java_net_DualStackPlainSocketImpl
 * Method:    socket0
 * Signature: (ZZ)I
 */
static cli.System.Net.Sockets.Socket socket0
  (JNIEnv env, boolean stream, boolean v6Only /*unused*/) {
    cli.System.Net.Sockets.Socket fd;
    int rv, opt=0;

    fd = NET_Socket(AF_INET6, (stream ? SOCK_STREAM : SOCK_DGRAM), 0);
    if (fd == INVALID_SOCKET) {
        NET_ThrowNew(env, WSAGetLastError(), "create");
        return null;
    }

    rv = setsockopt(fd, IPPROTO_IPV6, IPV6_V6ONLY, opt);
    if (rv == SOCKET_ERROR) {
        NET_ThrowNew(env, WSAGetLastError(), "create");
    }


    return fd;
}

/*
 * Class:     java_net_DualStackPlainSocketImpl
 * Method:    bind0
 * Signature: (ILjava/net/InetAddress;I)V
 */
static void bind0
  (JNIEnv env, cli.System.Net.Sockets.Socket fd, InetAddress iaObj, int port,
   boolean exclBind)
{
    SOCKETADDRESS sa;
    sa = new SOCKETADDRESS();
    int rv;

    if (NET_InetAddressToSockaddr(env, iaObj, port, sa,
                                 JNI_TRUE) != 0) {
      return;
    }

    rv = NET_WinBind(fd, sa, exclBind);

    if (rv == SOCKET_ERROR)
        NET_ThrowNew(env, WSAGetLastError(), "JVM_Bind");
}

/*
 * Class:     java_net_DualStackPlainSocketImpl
 * Method:    connect0
 * Signature: (ILjava/net/InetAddress;I)I
 */
static int connect0
  (JNIEnv env, cli.System.Net.Sockets.Socket fd, InetAddress iaObj, int port) {
    SOCKETADDRESS sa;
    sa = new SOCKETADDRESS();
    int rv;

    if (NET_InetAddressToSockaddr(env, iaObj, port, sa,
                                 JNI_TRUE) != 0) {
      return -1;
    }

    rv = connect(fd, sa);
    if (rv == SOCKET_ERROR) {
        int err = WSAGetLastError();
        if (err == WSAEWOULDBLOCK) {
            return java.net.DualStackPlainSocketImpl.WOULDBLOCK;
        } else if (err == WSAEADDRNOTAVAIL) {
            JNU_ThrowByName(env, JNU_JAVANETPKG+"ConnectException",
                "connect: Address is invalid on local machine, or port is not valid on remote machine");
        } else {
            NET_ThrowNew(env, err, "connect");
        }
        return -1;  // return value not important.
    }
    return rv;
}

/*
 * Class:     java_net_DualStackPlainSocketImpl
 * Method:    waitForConnect
 * Signature: (II)V
 */
static void waitForConnect
  (JNIEnv env, cli.System.Net.Sockets.Socket fd, int timeout) {
    int rv, retry;
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
     * Wait for timeout, connection established or
     * connection failed.
     */
    rv = select(null, wr, ex, t);

    /*
     * Timeout before connection is established/failed so
     * we throw exception and shutdown input/output to prevent
     * socket from being used.
     * The socket should be closed immediately by the caller.
     */
    if (rv == 0) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketTimeoutException",
                        "connect timed out");
        shutdown( fd, SD_BOTH );
        return;
    }

    /*
     * Socket is writable or error occurred. On some Windows editions
     * the socket will appear writable when the connect fails so we
     * check for error rather than writable.
     */
    if (!FD_ISSET(fd, ex)) {
        return;         /* connection established */
    }

    /*
     * Connection failed. The logic here is designed to work around
     * bug on Windows NT whereby using getsockopt to obtain the
     * last error (SO_ERROR) indicates there is no error. The workaround
     * on NT is to allow winsock to be scheduled and this is done by
     * yielding and retrying. As yielding is problematic in heavy
     * load conditions we attempt up to 3 times to get the error reason.
     */
    for (retry=0; retry<3; retry++) {
        int[] tmp = { 0 };
        NET_GetSockOpt(fd, SOL_SOCKET, SO_ERROR,
                       tmp);
        rv = tmp[0];
        if (rv != 0) {
            break;
        }
        Sleep(0);
    }

    if (rv == 0) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "Unable to establish connection");
    } else {
        NET_ThrowNew(env, rv, "connect");
    }
}

/*
 * Class:     java_net_DualStackPlainSocketImpl
 * Method:    localPort0
 * Signature: (I)I
 */
static int localPort0
  (JNIEnv env, cli.System.Net.Sockets.Socket fd) {
    SOCKETADDRESS sa;
    sa = new SOCKETADDRESS();

    if (getsockname(fd, sa) == SOCKET_ERROR) {
        if (WSAGetLastError() == WSAENOTSOCK) {
            JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                    "Socket closed");
        } else {
            NET_ThrowNew(env, WSAGetLastError(), "getsockname failed");
        }
        return -1;
    }
    return ntohs(GET_PORT(sa));
}

/*
 * Class:     java_net_DualStackPlainSocketImpl
 * Method:    localAddress
 * Signature: (ILjava/net/InetAddressContainer;)V
 */
static void localAddress
  (JNIEnv env, cli.System.Net.Sockets.Socket fd, InetAddressContainer iaContainerObj) {
    int[] port = { 0 };
    SOCKETADDRESS sa;
    sa = new SOCKETADDRESS();
    InetAddress iaObj;

    if (getsockname(fd, sa) == SOCKET_ERROR) {
        NET_ThrowNew(env, WSAGetLastError(), "Error getting socket name");
        return;
    }
    iaObj = NET_SockaddrToInetAddress(env, sa, port);

    iaContainerObj.addr = iaObj;
}


/*
 * Class:     java_net_DualStackPlainSocketImpl
 * Method:    listen0
 * Signature: (II)V
 */
static void listen0
  (JNIEnv env, cli.System.Net.Sockets.Socket fd, int backlog) {
    if (listen(fd, backlog) == SOCKET_ERROR) {
        NET_ThrowNew(env, WSAGetLastError(), "listen failed");
    }
}

/*
 * Class:     java_net_DualStackPlainSocketImpl
 * Method:    accept0
 * Signature: (I[Ljava/net/InetSocketAddress;)I
 */
static cli.System.Net.Sockets.Socket accept0
  (JNIEnv env, cli.System.Net.Sockets.Socket fd, InetSocketAddress[] isaa) {
    cli.System.Net.Sockets.Socket newfd;
    int[] port = { 0 };
    InetSocketAddress isa;
    InetAddress ia;
    SOCKETADDRESS sa;
    sa = new SOCKETADDRESS();

    newfd = accept(fd, sa);

    if (newfd == INVALID_SOCKET) {
        if (WSAGetLastError() == -2) {
            JNU_ThrowByName(env, JNU_JAVAIOPKG+"InterruptedIOException",
                            "operation interrupted");
        } else {
            JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                            "socket closed");
        }
        return null;
    }

    ia = NET_SockaddrToInetAddress(env, sa, port);
    isa = new InetSocketAddress(ia, port[0]);
    isaa[0] = isa;

    return newfd;
}

/*
 * Class:     java_net_DualStackPlainSocketImpl
 * Method:    waitForNewConnection
 * Signature: (II)V
 */
static void waitForNewConnection
  (JNIEnv env, cli.System.Net.Sockets.Socket fd, int timeout) {
    int rv;

    rv = NET_Timeout(fd, timeout);
    if (rv == 0) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketTimeoutException",
                        "Accept timed out");
    } else if (rv == -1) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "socket closed");
    } else if (rv == -2) {
        JNU_ThrowByName(env, JNU_JAVAIOPKG+"InterruptedIOException",
                        "operation interrupted");
    }
}

/*
 * Class:     java_net_DualStackPlainSocketImpl
 * Method:    available0
 * Signature: (I)I
 */
static int available0
  (JNIEnv env, cli.System.Net.Sockets.Socket fd) {
    int[] available = { -1 };

    if ((ioctlsocket(fd, FIONREAD, available)) == SOCKET_ERROR) {
        NET_ThrowNew(env, WSAGetLastError(), "socket available");
    }

    return available[0];
}

/*
 * Class:     java_net_DualStackPlainSocketImpl
 * Method:    close0
 * Signature: (I)V
 */
static void close0
  (JNIEnv env, cli.System.Net.Sockets.Socket fd) {
     NET_SocketClose(fd);
}

/*
 * Class:     java_net_DualStackPlainSocketImpl
 * Method:    shutdown0
 * Signature: (II)V
 */
static void shutdown0
  (JNIEnv env, cli.System.Net.Sockets.Socket fd, int howto) {
    shutdown(fd, howto);
}


/*
 * Class:     java_net_DualStackPlainSocketImpl
 * Method:    setIntOption
 * Signature: (III)V
 */
static void setIntOption
  (JNIEnv env, cli.System.Net.Sockets.Socket fd, int cmd, int value) {

    int[] level = { 0 };
    int[] opt = { 0 };
    linger linger;
    Object optval;

    if (NET_MapSocketOption(cmd, level, opt) < 0) {
        JNU_ThrowByName(env,
                                     JNU_JAVANETPKG+"SocketException",
                                     "Invalid option");
        return;
    }

    if (opt[0] == java.net.SocketOptions.SO_LINGER) {
        linger = new linger();
        if (value >= 0) {
            linger.l_onoff = 1;
            linger.l_linger = value & 0xFFFF;
        } else {
            linger.l_onoff = 0;
            linger.l_linger = 0;
        }
        optval = linger;
    } else {
        optval = value;
    }

    if (NET_SetSockOpt(fd, level[0], opt[0], optval) < 0) {
        NET_ThrowNew(env, WSAGetLastError(), "setsockopt");
    }
}

/*
 * Class:     java_net_DualStackPlainSocketImpl
 * Method:    getIntOption
 * Signature: (II)I
 */
static int getIntOption
  (JNIEnv env, cli.System.Net.Sockets.Socket fd, int cmd) {

    int[] level = { 0 };
    int[] opt = { 0 };
    int[] result = { 0 };
    linger linger;
    Object optval;

    if (NET_MapSocketOption(cmd, level, opt) < 0) {
        JNU_ThrowByName(env,
                                     JNU_JAVANETPKG+"SocketException",
                                     "Unsupported socket option");
        return -1;
    }

    if (opt[0] == java.net.SocketOptions.SO_LINGER) {
        linger = new linger();
        optval = linger;
    } else {
        linger = null;
        optval = result;
    }

    if (NET_GetSockOpt(fd, level[0], opt[0], optval) < 0) {
        NET_ThrowNew(env, WSAGetLastError(), "getsockopt");
        return -1;
    }

    if (opt[0] == java.net.SocketOptions.SO_LINGER)
        return linger.l_onoff != 0 ? linger.l_linger : -1;
    else
        return result[0];
}


/*
 * Class:     java_net_DualStackPlainSocketImpl
 * Method:    sendOOB
 * Signature: (II)V
 */
static void sendOOB
  (JNIEnv env, cli.System.Net.Sockets.Socket fd, int data) {
    int n;

    n = send(fd, new byte[] { (byte)data }, 1, MSG_OOB);
    if (n == JVM_IO_ERR) {
        NET_ThrowNew(env, WSAGetLastError(), "send");
    } else if (n == JVM_IO_INTR) {
        JNU_ThrowByName(env, "java.io.InterruptedIOException", null);
    }
}

/*
 * Class:     java_net_DualStackPlainSocketImpl
 * Method:    configureBlocking
 * Signature: (IZ)V
 */
static void configureBlocking
  (JNIEnv env, cli.System.Net.Sockets.Socket fd, boolean blocking) {
    int arg;
    int result;

    if (blocking == JNI_TRUE) {
        arg = SET_BLOCKING;    // 0
    } else {
        arg = SET_NONBLOCKING;   // 1
    }

    result = ioctlsocket(fd, FIONBIO, arg);
    if (result == SOCKET_ERROR) {
        NET_ThrowNew(env, WSAGetLastError(), "configureBlocking");
    }
}
}
