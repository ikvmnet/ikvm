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

final class DualStackPlainDatagramSocketImpl_c
{
static final int TRUE = 1;
static final int FALSE = 0;

static final int JVM_IO_ERR = -1;
static final int JVM_IO_INTR = -2;

/*
#include <windows.h>
#include <winsock2.h>
#include "jni.h"
#include "net_util.h"
#include "java_net_DualStackPlainDatagramSocketImpl.h"

/*
 * This function "purges" all outstanding ICMP port unreachable packets
 * outstanding on a socket and returns JNI_TRUE if any ICMP messages
 * have been purged. The rational for purging is to emulate normal BSD
 * behaviour whereby receiving a "connection reset" status resets the
 * socket.
 */
static boolean purgeOutstandingICMP(JNIEnv env, cli.System.Net.Sockets.Socket fd)
{
    boolean got_icmp = false;
    byte[] buf = new byte[1];
    fd_set tbl = new fd_set();
    timeval t = new timeval();
    SOCKETADDRESS rmtaddr = null;

    /*
     * Peek at the queue to see if there is an ICMP port unreachable. If there
     * is then receive it.
     */
    FD_ZERO(tbl);
    FD_SET(fd, tbl);
    while(true) {
        if (select(tbl, null, null, t) <= 0) {
            break;
        }
        if (recvfrom(fd, buf, 1, MSG_PEEK,
                         rmtaddr) != JVM_IO_ERR) {
            break;
        }
        if (WSAGetLastError() != WSAECONNRESET) {
            /* some other error - we don't care here */
            break;
        }

        recvfrom(fd, buf, 1, 0, rmtaddr);
        got_icmp = JNI_TRUE;
    }

    return got_icmp;
}

/*
 * Class:     java_net_DualStackPlainDatagramSocketImpl
 * Method:    socketCreate
 * Signature: (Z)I
 */
static cli.System.Net.Sockets.Socket socketCreate
  (JNIEnv env, boolean v6Only /*unused*/) {
    cli.System.Net.Sockets.Socket fd;
    int rv, opt=0, t=TRUE;

    fd = socket(AF_INET6, SOCK_DGRAM, 0);
    if (fd == INVALID_SOCKET) {
        NET_ThrowNew(env, WSAGetLastError(), "Socket creation failed");
        return null;
    }

    rv = setsockopt(fd, IPPROTO_IPV6, IPV6_V6ONLY, opt);
    if (rv == SOCKET_ERROR) {
        NET_ThrowNew(env, WSAGetLastError(), "Socket creation failed");
        return null;
    }

    //SetHandleInformation((HANDLE)(UINT_PTR)fd, HANDLE_FLAG_INHERIT, FALSE);
    NET_SetSockOpt(fd, SOL_SOCKET, SO_BROADCAST, t);

    /* SIO_UDP_CONNRESET fixes a "bug" introduced in Windows 2000, which
     * returns connection reset errors on unconnected UDP sockets (as well
     * as connected sockets). The solution is to only enable this feature
     * when the socket is connected.
     */
    t = FALSE;
    WSAIoctl(fd ,SIO_UDP_CONNRESET ,false);

    return fd;
}

/*
 * Class:     java_net_DualStackPlainDatagramSocketImpl
 * Method:    socketBind
 * Signature: (ILjava/net/InetAddress;I)V
 */
static void socketBind
  (JNIEnv env, cli.System.Net.Sockets.Socket fd, InetAddress iaObj, int port, boolean exclBind) {
    SOCKETADDRESS sa;
    sa = new SOCKETADDRESS();
    int rv;

    if (NET_InetAddressToSockaddr(env, iaObj, port, sa,
                                 JNI_TRUE) != 0) {
        return;
    }
    rv = NET_WinBind(fd, sa, exclBind);

    if (rv == SOCKET_ERROR) {
        if (WSAGetLastError() == WSAEACCES) {
            WSASetLastError(WSAEADDRINUSE);
        }
        NET_ThrowNew(env, WSAGetLastError(), "Cannot bind");
    }
}

/*
 * Class:     java_net_DualStackPlainDatagramSocketImpl
 * Method:    socketConnect
 * Signature: (ILjava/net/InetAddress;I)V
 */
static void socketConnect
  (JNIEnv env, cli.System.Net.Sockets.Socket fd, InetAddress iaObj, int port) {
    SOCKETADDRESS sa;
    sa = new SOCKETADDRESS();
    int rv;
    int t = TRUE;

    if (NET_InetAddressToSockaddr(env, iaObj, port, sa,
                                   JNI_TRUE) != 0) {
        return;
    }

    rv = connect(fd, sa);
    if (rv == SOCKET_ERROR) {
        NET_ThrowNew(env, WSAGetLastError(), "connect");
        return;
    }

    /* see comment in socketCreate */
    WSAIoctl(fd, SIO_UDP_CONNRESET, true);
}

/*
 * Class:     java_net_DualStackPlainDatagramSocketImpl
 * Method:    socketDisconnect
 * Signature: (I)V
 */
static void socketDisconnect
  (JNIEnv env, cli.System.Net.Sockets.Socket fd ) {
    SOCKETADDRESS sa;
    sa = new SOCKETADDRESS();

    connect(fd, sa);

    /* see comment in socketCreate */
    WSAIoctl(fd, SIO_UDP_CONNRESET, false);
}

/*
 * Class:     java_net_DualStackPlainDatagramSocketImpl
 * Method:    socketClose
 * Signature: (I)V
 */
static void socketClose
  (JNIEnv env, cli.System.Net.Sockets.Socket fd) {
    NET_SocketClose(fd);
}


/*
 * Class:     java_net_DualStackPlainDatagramSocketImpl
 * Method:    socketLocalPort
 * Signature: (I)I
 */
static int socketLocalPort
  (JNIEnv env, cli.System.Net.Sockets.Socket fd) {
    SOCKETADDRESS sa;
    sa = new SOCKETADDRESS();

    if (getsockname(fd, sa) == SOCKET_ERROR) {
        NET_ThrowNew(env, WSAGetLastError(), "JVM_GetSockName");
        return -1;
    }
    return ntohs(GET_PORT(sa));
}

/*
 * Class:     java_net_DualStackPlainDatagramSocketImpl
 * Method:    socketLocalAddress
 * Signature: (I)Ljava/lang/Object;
 */
static InetAddress socketLocalAddress
  (JNIEnv env , cli.System.Net.Sockets.Socket fd) {
    SOCKETADDRESS sa;
    sa = new SOCKETADDRESS();
    InetAddress iaObj;
    int[] port = { 0 };

    if (getsockname(fd, sa) == SOCKET_ERROR) {
        NET_ThrowNew(env, WSAGetLastError(), "Error getting socket name");
        return null;
    }

    iaObj = NET_SockaddrToInetAddress(env, sa, port);
    return iaObj;
}

/*
 * Class:     java_net_DualStackPlainDatagramSocketImpl
 * Method:    socketReceiveOrPeekData
 * Signature: (ILjava/net/DatagramPacket;IZZ)I
 */
static int socketReceiveOrPeekData
  (JNIEnv env, cli.System.Net.Sockets.Socket fd, DatagramPacket dpObj,
   int timeout, boolean connected, boolean peek) {
    SOCKETADDRESS sa;
    sa = new SOCKETADDRESS();
    int port, rv, flags=0;
    boolean retry;
    long prevTime = 0;

    int packetBufferOffset, packetBufferLen;
    byte[] packetBuffer;

    /* if we are only peeking. Called from peekData */
    if (peek) {
        flags = MSG_PEEK;
    }

    packetBuffer = dpObj.buf;
    packetBufferOffset = dpObj.offset;
    packetBufferLen = dpObj.bufLength;
    /* Note: the buffer needn't be greater than 65,536 (0xFFFF)
    * the max size of an IP packet. Anything bigger is truncated anyway.
    *-/
    if (packetBufferLen > MAX_PACKET_LEN) {
        packetBufferLen = MAX_PACKET_LEN;
    }

    if (packetBufferLen > MAX_BUFFER_LEN) {
        fullPacket = (char *)malloc(packetBufferLen);
        if (!fullPacket) {
            JNU_ThrowOutOfMemoryError(env, "Native heap allocation failed");
            return -1;
        }
    } else {
        fullPacket = &(BUF[0]);
    }
    */

    do {
        retry = false;

        if (timeout != 0) {
            if (prevTime == 0) {
                prevTime = JVM_CurrentTimeMillis(env, 0);
            }
            rv = NET_Timeout(fd, timeout);
            if (rv <= 0) {
                if (rv == 0) {
                    JNU_ThrowByName(env,JNU_JAVANETPKG+"SocketTimeoutException",
                                    "Receive timed out");
                } else if (rv == JVM_IO_ERR) {
                    JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                                    "Socket closed");
                } else if (rv == JVM_IO_INTR) {
                    JNU_ThrowByName(env, JNU_JAVAIOPKG+"InterruptedIOException",
                                    "operation interrupted");
                }
                return -1;
            }
        }

        /* receive the packet */
        rv = recvfrom(fd, packetBuffer, packetBufferOffset, packetBufferLen, flags,
                    sa);

        if (rv == SOCKET_ERROR && (WSAGetLastError() == WSAECONNRESET)) {
            /* An icmp port unreachable - we must receive this as Windows
             * does not reset the state of the socket until this has been
             * received.
             */
            purgeOutstandingICMP(env, fd);

            if (connected) {
                JNU_ThrowByName(env, JNU_JAVANETPKG+"PortUnreachableException",
                                "ICMP Port Unreachable");
                return -1;
            } else if (timeout != 0) {
                /* Adjust timeout */
                long newTime = JVM_CurrentTimeMillis(env, 0);
                timeout -= (int)(newTime - prevTime);
                if (timeout <= 0) {
                    JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketTimeoutException",
                                    "Receive timed out");
                    return -1;
                }
                prevTime = newTime;
            }
            retry = true;
        }
    } while (retry);

    port = ntohs (GET_PORT(sa));

    /* truncate the data if the packet's length is too small */
    if (rv > packetBufferLen) {
        rv = packetBufferLen;
    }
    if (rv < 0) {
        if (WSAGetLastError() == WSAEMSGSIZE) {
            /* it is because the buffer is too small. It's UDP, it's
             * unreliable, it's all good. discard the rest of the
             * data..
             */
            rv = packetBufferLen;
        } else {
            /* failure */
            dpObj.length = 0;
        }
    }

    if (rv == -1) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "socket closed");
    } else if (rv == -2) {
        JNU_ThrowByName(env, JNU_JAVAIOPKG+"InterruptedIOException",
                        "operation interrupted");
    } else if (rv < 0) {
        NET_ThrowCurrent(env, "Datagram receive failed");
    } else {
        InetAddress packetAddress;
        /*
         * Check if there is an InetAddress already associated with this
         * packet. If so, we check if it is the same source address. We
         * can't update any existing InetAddress because it is immutable
         */
        packetAddress = dpObj.address;
        if (packetAddress != NULL) {
            if (!NET_SockaddrEqualsInetAddress(sa,
                                               packetAddress)) {
                /* force a new InetAddress to be created */
                packetAddress = null;
            }
        }
        if (packetAddress == NULL) {
            int[] tmp = { port };
            packetAddress = NET_SockaddrToInetAddress(sa, tmp);
            port = tmp[0];
            if (packetAddress != NULL) {
                /* stuff the new Inetaddress into the packet */
                dpObj.address = packetAddress;
            }
        }

        /* populate the packet */
        dpObj.port = port;
        dpObj.length = rv;
    }

    return port;
}

/*
 * Class:     java_net_DualStackPlainDatagramSocketImpl
 * Method:    socketSend
 * Signature: (I[BIILjava/net/InetAddress;IZ)V
 */
static void socketSend
  (JNIEnv env, cli.System.Net.Sockets.Socket fd, byte[] data, int offset, int length,
     InetAddress iaObj, int port, boolean connected) {
    SOCKETADDRESS sa;
    int rv;

    if (connected) {
        sa = null; /* arg to JVM_Sendto () null in this case */
    } else {
        sa = new SOCKETADDRESS();
        if (NET_InetAddressToSockaddr(env, iaObj, port, sa,
                                       JNI_TRUE) != 0) {
            return;
        }
    }

    /*
    if (length > MAX_BUFFER_LEN) {
        /* Note: the buffer needn't be greater than 65,536 (0xFFFF)
         * the max size of an IP packet. Anything bigger is truncated anyway.
         *-/
        if (length > MAX_PACKET_LEN) {
            length = MAX_PACKET_LEN;
        }
        fullPacket = (char *)malloc(length);
        if (!fullPacket) {
            JNU_ThrowOutOfMemoryError(env, "Native heap allocation failed");
            return;
        }
    } else {
        fullPacket = &(BUF[0]);
    }
    */

    rv = sendto(fd, data, offset, length, 0, sa);
    if (rv == SOCKET_ERROR) {
        if (rv == JVM_IO_ERR) {
            NET_ThrowNew(env, WSAGetLastError(), "Datagram send failed");
        } else if (rv == JVM_IO_INTR) {
            JNU_ThrowByName(env, JNU_JAVAIOPKG+"InterruptedIOException",
                            "operation interrupted");
        }
    }

}

/*
 * Class:     java_net_DualStackPlainDatagramSocketImpl
 * Method:    socketSetIntOption
 * Signature: (III)V
 */
static void socketSetIntOption
  (JNIEnv env, cli.System.Net.Sockets.Socket fd , int cmd, int value) {
    int[] level = { 0 }, opt = { 0 };

    if (NET_MapSocketOption(cmd, level, opt) < 0) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                                     "Invalid option");
        return;
    }

    if (NET_SetSockOpt(fd, level[0], opt[0], value) < 0) {
        NET_ThrowNew(env, WSAGetLastError(), "setsockopt");
    }
}

/*
 * Class:     java_net_DualStackPlainDatagramSocketImpl
 * Method:    socketGetIntOption
 * Signature: (II)I
 */
static int socketGetIntOption
  (JNIEnv env, cli.System.Net.Sockets.Socket fd, int cmd) {
    int[] level = { 0 }, opt = { 0 }, result = { 0 };

    if (NET_MapSocketOption(cmd, level, opt) < 0) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                                     "Invalid option");
        return -1;
    }

    if (NET_GetSockOpt(fd, level[0], opt[0], result) < 0) {
        NET_ThrowNew(env, WSAGetLastError(), "getsockopt");
        return -1;
    }

    return result[0];
}
}
