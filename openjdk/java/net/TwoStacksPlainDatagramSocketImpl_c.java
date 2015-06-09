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

final class TwoStacksPlainDatagramSocketImpl_c
{
static final int ni_class = 0;
static final int JVM_IO_ERR = -1;
static final int JVM_IO_INTR = -2;

static final int java_net_SocketOptions_SO_BINDADDR = SocketOptions.SO_BINDADDR;
static final int java_net_SocketOptions_SO_SNDBUF = SocketOptions.SO_SNDBUF;
static final int java_net_SocketOptions_SO_RCVBUF = SocketOptions.SO_RCVBUF;
static final int java_net_SocketOptions_IP_TOS = SocketOptions.IP_TOS;
static final int java_net_SocketOptions_SO_REUSEADDR = SocketOptions.SO_REUSEADDR;
static final int java_net_SocketOptions_SO_BROADCAST = SocketOptions.SO_BROADCAST;
static final int java_net_SocketOptions_IP_MULTICAST_LOOP = SocketOptions.IP_MULTICAST_LOOP;
static final int java_net_SocketOptions_IP_MULTICAST_IF = SocketOptions.IP_MULTICAST_IF;
static final int java_net_SocketOptions_IP_MULTICAST_IF2 = SocketOptions.IP_MULTICAST_IF2;

/*

#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <ctype.h>
#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>
#include <sys/types.h>

#ifndef IPTOS_TOS_MASK
#define IPTOS_TOS_MASK 0x1e
#endif
#ifndef IPTOS_PREC_MASK
#define IPTOS_PREC_MASK 0xe0
#endif

#include "java_net_TwoStacksPlainDatagramSocketImpl.h"
#include "java_net_SocketOptions.h"
#include "java_net_NetworkInterface.h"

#include "NetworkInterface.h"
#include "jvm.h"
#include "jni_util.h"
#include "net_util.h"

#define IN_CLASSD(i)    (((long)(i) & 0xf0000000) == 0xe0000000)
#define IN_MULTICAST(i) IN_CLASSD(i)

*/

static boolean IN_MULTICAST(int ipv4address) {
    return ((ipv4address >> 24) & 0xf0) == 0xe0;
}

/************************************************************************
 * TwoStacksPlainDatagramSocketImpl
 */

/*
static jfieldID IO_fd_fdID;
static jfieldID pdsi_trafficClassID;
jfieldID pdsi_fdID;
jfieldID pdsi_fd1ID;
jfieldID pdsi_fduseID;
jfieldID pdsi_lastfdID;
jfieldID pdsi_timeoutID;

jfieldID pdsi_localPortID;
jfieldID pdsi_connected;

static jclass ia4_clazz;
static jmethodID ia4_ctor;

static CRITICAL_SECTION sizeCheckLock;
*/

/* Windows OS version is XP or better */
static final boolean xp_or_later = true;
/* Windows OS version is Windows 2000 or better */
//static int w2k_or_later = 0;

/*
 * Notes about UDP/IPV6 on Windows (XP and 2003 server):
 *
 * fd always points to the IPv4 fd, and fd1 points to the IPv6 fd.
 * Both fds are used when we bind to a wild-card address. When a specific
 * address is used, only one of them is used.
 */

/*
 * Returns a java.lang.Integer based on 'i'
 */
/*
jobject createInteger(JNIEnv *env, int i) {
    static jclass i_class;
    static jmethodID i_ctrID;
    static jfieldID i_valueID;

    if (i_class == NULL) {
        jclass c = (*env)->FindClass(env, "java/lang/Integer");
        CHECK_NULL_RETURN(c, NULL);
        i_ctrID = (*env)->GetMethodID(env, c, "<init>", "(I)V");
        CHECK_NULL_RETURN(i_ctrID, NULL);
        i_class = (*env)->NewGlobalRef(env, c);
        CHECK_NULL_RETURN(i_class, NULL);
    }

    return ( (*env)->NewObject(env, i_class, i_ctrID, i) );
}
*/

/*
 * Returns a java.lang.Boolean based on 'b'
 */
/*
jobject createBoolean(JNIEnv *env, int b) {
    static jclass b_class;
    static jmethodID b_ctrID;
    static jfieldID b_valueID;

    if (b_class == NULL) {
        jclass c = (*env)->FindClass(env, "java/lang/Boolean");
        CHECK_NULL_RETURN(c, NULL);
        b_ctrID = (*env)->GetMethodID(env, c, "<init>", "(Z)V");
        CHECK_NULL_RETURN(b_ctrID, NULL);
        b_class = (*env)->NewGlobalRef(env, c);
        CHECK_NULL_RETURN(b_class, NULL);
    }

    return( (*env)->NewObject(env, b_class, b_ctrID, (jboolean)(b!=0)) );
}
*/


static cli.System.Net.Sockets.Socket getFD(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this) {
    FileDescriptor fdObj = _this.fd;

    if (fdObj == NULL) {
        return null;
    }
    return fdObj.getSocket();
}

static cli.System.Net.Sockets.Socket getFD1(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this) {
    FileDescriptor fdObj = _this.fd1;

    if (fdObj == NULL) {
        return null;
    }
    return fdObj.getSocket();
}

/*
 * This function returns JNI_TRUE if the datagram size exceeds the underlying
 * provider's ability to send to the target address. The following OS
 * oddities have been observed :-
 *
 * 1. On Windows 95/98 if we try to send a datagram > 12k to an application
 *    on the same machine then the send will fail silently.
 *
 * 2. On Windows ME if we try to send a datagram > supported by underlying
 *    provider then send will not return an error.
 *
 * 3. On Windows NT/2000 if we exceeds the maximum size then send will fail
 *    with WSAEADDRNOTAVAIL.
 *
 * 4. On Windows 95/98 if we exceed the maximum size when sending to
 *    another machine then WSAEINVAL is returned.
 *
 */
/*
jboolean exceedSizeLimit(JNIEnv *env, jint fd, jint addr, jint size)
{
#define DEFAULT_MSG_SIZE        65527
    static jboolean initDone;
    static jboolean is95or98;
    static int maxmsg;

    typedef struct _netaddr  {          /* Windows 95/98 only *-/
        unsigned long addr;
        struct _netaddr *next;
    } netaddr;
    static netaddr *addrList;
    netaddr *curr;

    /*
     * First time we are called we must determine which OS this is and also
     * get the maximum size supported by the underlying provider.
     *
     * In addition on 95/98 we must enumerate our IP addresses.
     *-/
    if (!initDone) {
        EnterCriticalSection(&sizeCheckLock);

        if (initDone) {
            /* another thread got there first *-/
            LeaveCriticalSection(&sizeCheckLock);

        } else {
            OSVERSIONINFO ver;
            int len;

            /*
             * Step 1: Determine which OS this is.
             *-/
            ver.dwOSVersionInfoSize = sizeof(ver);
            GetVersionEx(&ver);

            is95or98 = JNI_FALSE;
            if (ver.dwPlatformId == VER_PLATFORM_WIN32_WINDOWS &&
                ver.dwMajorVersion == 4 &&
                (ver.dwMinorVersion == 0 || ver.dwMinorVersion == 10)) {

                is95or98 = JNI_TRUE;
            }

            /*
             * Step 2: Determine the maximum datagram supported by the
             * underlying provider. On Windows 95 if winsock hasn't been
             * upgraded (ie: unsupported configuration) then we assume
             * the default 64k limit.
             *-/
            len = sizeof(maxmsg);
            if (NET_GetSockOpt(fd, SOL_SOCKET, SO_MAX_MSG_SIZE, (char *)&maxmsg, &len) < 0) {
                maxmsg = DEFAULT_MSG_SIZE;
            }

            /*
             * Step 3: On Windows 95/98 then enumerate the IP addresses on
             * this machine. This is neccesary because we need to check if the
             * datagram is being sent to an application on the same machine.
             *-/
            if (is95or98) {
                char hostname[255];
                struct hostent *hp;

                if (gethostname(hostname, sizeof(hostname)) == -1) {
                    LeaveCriticalSection(&sizeCheckLock);
                    JNU_ThrowByName(env, JNU_JAVANETPKG "SocketException", "Unable to obtain hostname");
                    return JNI_TRUE;
                }
                hp = (struct hostent *)gethostbyname(hostname);
                if (hp != NULL) {
                    struct in_addr **addrp = (struct in_addr **) hp->h_addr_list;

                    while (*addrp != (struct in_addr *) 0) {
                        curr = (netaddr *)malloc(sizeof(netaddr));
                        if (curr == NULL) {
                            while (addrList != NULL) {
                                curr = addrList->next;
                                free(addrList);
                                addrList = curr;
                            }
                            LeaveCriticalSection(&sizeCheckLock);
                            JNU_ThrowOutOfMemoryError(env, "Native heap allocation failed");
                            return JNI_TRUE;
                        }
                        curr->addr = htonl((*addrp)->S_un.S_addr);
                        curr->next = addrList;
                        addrList = curr;
                        addrp++;
                    }
                }
            }

            /*
             * Step 4: initialization is done so set flag and unlock cs
             *-/
            initDone = JNI_TRUE;
            LeaveCriticalSection(&sizeCheckLock);
        }
    }

    /*
     * Now examine the size of the datagram :-
     *
     * (a) If exceeds size of service provider return 'false' to indicate that
     *     we exceed the limit.
     * (b) If not 95/98 then return 'true' to indicate that the size is okay.
     * (c) On 95/98 if the size is <12k we are okay.
     * (d) On 95/98 if size > 12k then check if the destination is the current
     *     machine.
     *-/
    if (size > maxmsg) {        /* step (a) *-/
        return JNI_TRUE;
    }
    if (!is95or98) {            /* step (b) *-/
        return JNI_FALSE;
    }
    if (size <= 12280) {        /* step (c) *-/
        return JNI_FALSE;
    }

    /* step (d) *-/

    if ((addr & 0x7f000000) == 0x7f000000) {
        return JNI_TRUE;
    }
    curr = addrList;
    while (curr != NULL) {
        if (curr->addr == addr) {
            return JNI_TRUE;
        }
        curr = curr->next;
    }
    return JNI_FALSE;
}
*/

/*
 * Return JNI_TRUE if this Windows edition supports ICMP Port Unreachable
 */
static boolean supportPortUnreachable() {
    // we don't support anything pre-Win2K anyway
    return true;
}

/*
 * This function "purges" all outstanding ICMP port unreachable packets
 * outstanding on a socket and returns JNI_TRUE if any ICMP messages
 * have been purged. The rational for purging is to emulate normal BSD
 * behaviour whereby receiving a "connection reset" status resets the
 * socket.
 */
static boolean purgeOutstandingICMP(cli.System.Net.Sockets.Socket fd)
{
    boolean got_icmp = false;
    byte[] buf = new byte[1];
    fd_set tbl = new fd_set();
    timeval t = new timeval();
    SOCKETADDRESS rmtaddr = null;

    /*
     * A no-op if this OS doesn't support it.
     */
    if (!supportPortUnreachable()) {
        return JNI_FALSE;
    }

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
 * Class:     java_net_TwoStacksPlainDatagramSocketImpl
 * Method:    init
 * Signature: ()V
 */
/*
JNIEXPORT void JNICALL
Java_java_net_TwoStacksPlainDatagramSocketImpl_init(JNIEnv *env, jclass cls) {

    OSVERSIONINFO ver;
    int version;
    ver.dwOSVersionInfoSize = sizeof(ver);
    GetVersionEx(&ver);

    version = ver.dwMajorVersion * 10 + ver.dwMinorVersion;
    xp_or_later = (ver.dwPlatformId == VER_PLATFORM_WIN32_NT) && (version >= 51);
    w2k_or_later = (ver.dwPlatformId == VER_PLATFORM_WIN32_NT) && (version >= 50);

    /* get fieldIDs *-/
    pdsi_fdID = (*env)->GetFieldID(env, cls, "fd", "Ljava/io/FileDescriptor;");
    CHECK_NULL(pdsi_fdID);
    pdsi_fd1ID = (*env)->GetFieldID(env, cls, "fd1", "Ljava/io/FileDescriptor;");
    CHECK_NULL(pdsi_fd1ID);
    pdsi_timeoutID = (*env)->GetFieldID(env, cls, "timeout", "I");
    CHECK_NULL(pdsi_timeoutID);
    pdsi_fduseID = (*env)->GetFieldID(env, cls, "fduse", "I");
    CHECK_NULL(pdsi_fduseID);
    pdsi_lastfdID = (*env)->GetFieldID(env, cls, "lastfd", "I");
    CHECK_NULL(pdsi_lastfdID);
    pdsi_trafficClassID = (*env)->GetFieldID(env, cls, "trafficClass", "I");
    CHECK_NULL(pdsi_trafficClassID);
    pdsi_localPortID = (*env)->GetFieldID(env, cls, "localPort", "I");
    CHECK_NULL(pdsi_localPortID);
    pdsi_connected = (*env)->GetFieldID(env, cls, "connected", "Z");
    CHECK_NULL(pdsi_connected);

    cls = (*env)->FindClass(env, "java/io/FileDescriptor");
    CHECK_NULL(cls);
    IO_fd_fdID = NET_GetFileDescriptorID(env);
    CHECK_NULL(IO_fd_fdID);

    ia4_clazz = (*env)->FindClass(env, "java/net/Inet4Address");
    CHECK_NULL(ia4_clazz);
    ia4_clazz = (*env)->NewGlobalRef(env, ia4_clazz);
    CHECK_NULL(ia4_clazz);
    ia4_ctor = (*env)->GetMethodID(env, ia4_clazz, "<init>", "()V");
    CHECK_NULL(ia4_ctor);


    InitializeCriticalSection(&sizeCheckLock);
}
*/

static void bind0(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this,
                                           int port, InetAddress addressObj,
                                           boolean exclBind) {
    FileDescriptor fdObj = _this.fd;
    FileDescriptor fd1Obj = _this.fd1;

    cli.System.Net.Sockets.Socket fd = null;
    cli.System.Net.Sockets.Socket fd1 = null;
    int family;
    boolean ipv6_supported = ipv6_available();

    SOCKETADDRESS lcladdr;
    lcladdr = new SOCKETADDRESS();

    family = getInetAddress_family(env, addressObj);
    if (family == IPv6 && !ipv6_supported) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "Protocol family not supported");
        return;
    }

    if (IS_NULL(fdObj) || (ipv6_supported && IS_NULL(fd1Obj))) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "socket closed");
        return;
    } else {
        fd = fdObj.getSocket();
        if (ipv6_supported) {
            fd1 = fd1Obj.getSocket();
        }
    }
    if (IS_NULL(addressObj)) {
        JNU_ThrowNullPointerException(env, "argument address");
        return;
    }

    if (NET_InetAddressToSockaddr(env, addressObj, port, lcladdr, JNI_FALSE) != 0) {
      return;
    }

    if (ipv6_supported) {
        ipv6bind v6bind = new ipv6bind();
        v6bind.addr = lcladdr;
        v6bind.ipv4_fd = fd;
        v6bind.ipv6_fd = fd1;
        if (NET_BindV6(v6bind, exclBind) != -1) {
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
            }
        } else {
            /* NET_BindV6() closes both sockets upon a failure */
            _this.fd = null;
            _this.fd1 = null;
            NET_ThrowCurrent (env, "Cannot bind");
            return;
        }
    } else {
        if (NET_WinBind(fd, lcladdr, exclBind) == -1) {
            if (WSAGetLastError() == WSAEACCES) {
                WSASetLastError(WSAEADDRINUSE);
            }
            NET_ThrowCurrent(env, "Cannot bind");
            return;
        }
    }

    if (port == 0) {
        if (fd == null) {
            /* must be an IPV6 only socket. */
            fd = fd1;
        }
        if (getsockname(fd, lcladdr) == -1) {
            NET_ThrowCurrent(env, "JVM_GetSockName");
            return;
        }
        port = ntohs(GET_PORT (lcladdr));
    }
    _this.localPort = port;
}


/*
 * Class:     java_net_TwoStacksPlainDatagramSocketImpl
 * Method:    connect0
 * Signature: (Ljava/net/InetAddress;I)V
 */

static void connect0(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this, InetAddress address, int port) {
    /* The object's field */
    FileDescriptor fdObj = _this.fd;
    FileDescriptor fd1Obj = _this.fd1;
    /* The fdObj'fd */
    cli.System.Net.Sockets.Socket fd = null;
    cli.System.Net.Sockets.Socket fd1 = null;
    cli.System.Net.Sockets.Socket fdc;
    /* The packetAddress address, family and port */
    int addr, family;
    SOCKETADDRESS rmtaddr;
    rmtaddr = new SOCKETADDRESS();
    boolean ipv6_supported = ipv6_available();

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

    if (IS_NULL(address)) {
        JNU_ThrowNullPointerException(env, "address");
        return;
    }

    addr = getInetAddress_addr(env, address);

    family = getInetAddress_family(env, address);
    if (family == IPv6 && !ipv6_supported) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "Protocol family not supported");
        return;
    }

    fdc = family == IPv4? fd: fd1;

    if (xp_or_later) {
        /* SIO_UDP_CONNRESET fixes a bug introduced in Windows 2000, which
         * returns connection reset errors on connected UDP sockets (as well
         * as connected sockets). The solution is to only enable this feature
         * when the socket is connected
         */
        WSAIoctl(fdc, SIO_UDP_CONNRESET, true);
    }

    if (NET_InetAddressToSockaddr(env, address, port, rmtaddr, JNI_FALSE) != 0) {
      return;
    }

    if (connect(fdc, rmtaddr) == -1) {
        NET_ThrowCurrent(env, "connect");
        return;
    }
}

/*
 * Class:     java_net_TwoStacksPlainDatagramSocketImpl
 * Method:    disconnect0
 * Signature: ()V
 */

static void disconnect0(TwoStacksPlainDatagramSocketImpl _this, int family) {
    /* The object's field */
    FileDescriptor fdObj;
    /* The fdObj'fd */
    cli.System.Net.Sockets.Socket fd;
    SOCKETADDRESS addr;
    addr = new SOCKETADDRESS();

    if (family == IPv4) {
        fdObj = _this.fd;
    } else {
        fdObj = _this.fd1;
    }

    if (IS_NULL(fdObj)) {
        /* disconnect doesn't throw any exceptions */
        return;
    }
    fd = fdObj.getSocket();

    connect(fd, addr);

    /*
     * use SIO_UDP_CONNRESET
     * to disable ICMP port unreachable handling here.
     */
    if (xp_or_later) {
        WSAIoctl(fd,SIO_UDP_CONNRESET,false);
    }
}

/*
 * Class:     java_net_TwoStacksPlainDatagramSocketImpl
 * Method:    send
 * Signature: (Ljava/net/DatagramPacket;)V
 */
static void send(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this, DatagramPacket packet) {
    FileDescriptor fdObj;
    cli.System.Net.Sockets.Socket fd;

    InetAddress iaObj;
    int address;
    int family;

    int packetBufferOffset, packetBufferLen, packetPort;
    byte[] packetBuffer;
    boolean connected;

    SOCKETADDRESS rmtaddr;
    rmtaddr = new SOCKETADDRESS();

    if (IS_NULL(packet)) {
        JNU_ThrowNullPointerException(env, "null packet");
        return;
    }

    iaObj = packet.address;

    packetPort = packet.port;
    packetBufferOffset = packet.offset;
    packetBuffer = packet.buf;
    connected = _this.connected;

    if (IS_NULL(iaObj) || IS_NULL(packetBuffer)) {
        JNU_ThrowNullPointerException(env, "null address || null buffer");
        return;
    }

    family = getInetAddress_family(env, iaObj);
    if (family == IPv4) {
        fdObj = _this.fd;
    } else {
        if (!ipv6_available()) {
            JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "Protocol not allowed");
            return;
        }
        fdObj = _this.fd1;
    }

    if (IS_NULL(fdObj)) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "Socket closed");
        return;
    }
    fd = fdObj.getSocket();

    packetBufferLen = packet.length;
    /* Note: the buffer needn't be greater than 65,536 (0xFFFF)...
     * the maximum size of an IP packet. Anything bigger is truncated anyway.
     */
    if (packetBufferLen > MAX_PACKET_LEN) {
        packetBufferLen = MAX_PACKET_LEN;
    }

    if (connected) {
        rmtaddr = null;
    } else {
      if (NET_InetAddressToSockaddr(env, iaObj, packetPort, rmtaddr, JNI_FALSE) != 0) {
        return;
      }
    }

    /*
    if (packetBufferLen > MAX_BUFFER_LEN) {

        /*
         * On 95/98 if we try to send a datagram >12k to an application
         * on the same machine then this will fail silently. Thus we
         * catch this situation here so that we can throw an exception
         * when this arises.
         * On ME if we try to send a datagram with a size greater than
         * that supported by the service provider then no error is
         * returned.
         *-/
        if (!w2k_or_later) { /* avoid this check on Win 2K or better. Does not work with IPv6.
                      * Check is not necessary on these OSes *-/
            if (connected) {
                address = getInetAddress_addr(env, iaObj);
            } else {
                address = ntohl(rmtaddr.him4.sin_addr.s_addr);
            }

            if (exceedSizeLimit(env, fd, address, packetBufferLen)) {
                if (!((*env)->ExceptionOccurred(env))) {
                    NET_ThrowNew(env, WSAEMSGSIZE, "Datagram send failed");
                }
                return;
            }
        }

        /* When JNI-ifying the JDK's IO routines, we turned
         * reads and writes of byte arrays of size greater
         * than 2048 bytes into several operations of size 2048.
         * This saves a malloc()/memcpy()/free() for big
         * buffers.  This is OK for file IO and TCP, but that
         * strategy violates the semantics of a datagram protocol.
         * (one big send) != (several smaller sends).  So here
         * we *must* alloc the buffer.  Note it needn't be bigger
         * than 65,536 (0xFFFF) the max size of an IP packet.
         * anything bigger is truncated anyway.
         *-/
        fullPacket = (char *)malloc(packetBufferLen);
        if (!fullPacket) {
            JNU_ThrowOutOfMemoryError(env, "Send buf native heap allocation failed");
            return;
        }
    } else {
        fullPacket = &(BUF[0]);
    }
    */

    switch (sendto(fd, packetBuffer, packetBufferOffset, packetBufferLen, 0, rmtaddr)) {
        case JVM_IO_ERR:
            NET_ThrowCurrent(env, "Datagram send failed");
            break;

        case JVM_IO_INTR:
            JNU_ThrowByName(env, JNU_JAVAIOPKG+"InterruptedIOException",
                            "operation interrupted");
    }
}

/*
 * check which socket was last serviced when there was data on both sockets.
 * Only call this if sure that there is data on both sockets.
 */
private static cli.System.Net.Sockets.Socket checkLastFD (TwoStacksPlainDatagramSocketImpl _this, cli.System.Net.Sockets.Socket fd, cli.System.Net.Sockets.Socket fd1) {
    cli.System.Net.Sockets.Socket nextfd, lastfd = _this.lastfd;
    if (lastfd == null) {
        /* arbitrary. Choose fd */
        _this.lastfd = fd;
        return fd;
    } else {
        if (lastfd == fd) {
            nextfd = fd1;
        } else {
            nextfd = fd;
        }
        _this.lastfd = nextfd;
        return nextfd;
    }
}

/*
 * Class:     java_net_TwoStacksPlainDatagramSocketImpl
 * Method:    peek
 * Signature: (Ljava/net/InetAddress;)I
 */
static int peek(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this, InetAddress addressObj) {
    FileDescriptor fdObj = _this.fd;
    int timeout = _this.timeout;
    cli.System.Net.Sockets.Socket fd;

    /* The address and family fields of addressObj */
    int address, family;

    int n;
    SOCKETADDRESS remote_addr = new SOCKETADDRESS();
    byte[] buf = new byte[1];
    boolean retry;
    long prevTime = 0;

    if (IS_NULL(fdObj)) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "Socket closed");
        return -1;
    } else {
        fd = fdObj.getSocket();
        if (fd == null) {
           JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                           "socket closed");
           return -1;
        }
    }
    if (IS_NULL(addressObj)) {
        JNU_ThrowNullPointerException(env, "Null address in peek()");
        return -1;
    } else {
        address = getInetAddress_addr(env, addressObj);
        /* We only handle IPv4 for now. Will support IPv6 once its in the os */
        family = AF_INET;
    }

    do {
        retry = FALSE;

        /*
         * If a timeout has been specified then we select on the socket
         * waiting for a read event or a timeout.
         */
        if (timeout != 0) {
            int ret;
            prevTime = JVM_CurrentTimeMillis(env, 0);
            ret = NET_Timeout (fd, timeout);
            if (ret == 0) {
                JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketTimeoutException",
                                "Peek timed out");
                return ret;
            } else if (ret == JVM_IO_ERR) {
                NET_ThrowCurrent(env, "timeout in datagram socket peek");
                return ret;
            } else if (ret == JVM_IO_INTR) {
                JNU_ThrowByName(env, JNU_JAVAIOPKG+"InterruptedIOException",
                                "operation interrupted");
                return ret;
            }
        }

        /* now try the peek */
        n = recvfrom(fd, buf, 1, MSG_PEEK,
                         remote_addr);

        if (n == JVM_IO_ERR) {
            if (WSAGetLastError() == WSAECONNRESET) {
                boolean connected;

                /*
                 * An icmp port unreachable - we must receive this as Windows
                 * does not reset the state of the socket until this has been
                 * received.
                 */
                purgeOutstandingICMP(fd);

                connected =  _this.connected;
                if (connected) {
                    JNU_ThrowByName(env, JNU_JAVANETPKG+"PortUnreachableException",
                                       "ICMP Port Unreachable");
                    return 0;
                }

                /*
                 * If a timeout was specified then we need to adjust it because
                 * we may have used up some of the timeout befor the icmp port
                 * unreachable arrived.
                 */
                if (timeout != 0) {
                    long newTime = JVM_CurrentTimeMillis(env, 0);
                    timeout -= (newTime - prevTime);
                    if (timeout <= 0) {
                        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketTimeoutException",
                                "Receive timed out");
                        return 0;
                    }
                    prevTime = newTime;
                }

                /* Need to retry the recv */
                retry = TRUE;
            }
        }
    } while (retry);

    if (n == JVM_IO_ERR && WSAGetLastError() != WSAEMSGSIZE) {
        NET_ThrowCurrent(env, "Datagram peek failed");
        return 0;
    }
    if (n == JVM_IO_INTR) {
        JNU_ThrowByName(env, JNU_JAVAIOPKG+"InterruptedIOException", null);
        return 0;
    }
    addressObj.holder().address = ntohl(remote_addr.sin_addr.s_addr);
    addressObj.holder().family = IPv4;

    /* return port */
    return ntohs(remote_addr.sin_port);
}

static int peekData(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this, DatagramPacket packet) {

    FileDescriptor fdObj = _this.fd;
    FileDescriptor fd1Obj = _this.fd1;
    int timeout = _this.timeout;

    byte[] packetBuffer;
    int packetBufferOffset, packetBufferLen;

    cli.System.Net.Sockets.Socket fd = null, fd1 = null, fduse = null;
    int nsockets=0, errorCode;
    int port;
    byte[] data;

    boolean checkBoth = false;
    int datalen;
    int n;
    SOCKETADDRESS remote_addr;
    remote_addr = new SOCKETADDRESS();
    boolean retry;
    long prevTime = 0;

    if (!IS_NULL(fdObj)) {
        fd = fdObj.getSocket();
        if (fd == null) {
           JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                           "socket closed");
           return -1;
        }
        nsockets = 1;
    }

    if (!IS_NULL(fd1Obj)) {
        fd1 = fd1Obj.getSocket();
        if (fd1 == null) {
           JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                           "socket closed");
           return -1;
        }
        nsockets ++;
    }

    switch (nsockets) {
      case 0:
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                       "socket closed");
        return -1;
      case 1:
        if (!IS_NULL(fdObj)) {
           fduse = fd;
        } else {
           fduse = fd1;
        }
        break;
      case 2:
        checkBoth = TRUE;
        break;
    }

    if (IS_NULL(packet)) {
        JNU_ThrowNullPointerException(env, "packet");
        return -1;
    }

    packetBuffer = packet.buf;

    if (IS_NULL(packetBuffer)) {
        JNU_ThrowNullPointerException(env, "packet buffer");
        return -1;
    }

    packetBufferOffset = packet.offset;
    packetBufferLen = packet.bufLength;

    /*
    if (packetBufferLen > MAX_BUFFER_LEN) {

        /* When JNI-ifying the JDK's IO routines, we turned
         * read's and write's of byte arrays of size greater
         * than 2048 bytes into several operations of size 2048.
         * This saves a malloc()/memcpy()/free() for big
         * buffers.  This is OK for file IO and TCP, but that
         * strategy violates the semantics of a datagram protocol.
         * (one big send) != (several smaller sends).  So here
         * we *must* alloc the buffer.  Note it needn't be bigger
         * than 65,536 (0xFFFF) the max size of an IP packet.
         * anything bigger is truncated anyway.
         *-/
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
        int ret;
        retry = FALSE;

        /*
         * If a timeout has been specified then we select on the socket
         * waiting for a read event or a timeout.
         */
        if (checkBoth) {
            int t = timeout == 0 ? -1: timeout;
            prevTime = JVM_CurrentTimeMillis(env, 0);
            cli.System.Net.Sockets.Socket[] tmp = new cli.System.Net.Sockets.Socket[] { fduse };
            ret = NET_Timeout2 (fd, fd1, t, tmp);
            fduse = tmp[0];
            /* all subsequent calls to recv() or select() will use the same fd
             * for this call to peek() */
            if (ret <= 0) {
                if (ret == 0) {
                    JNU_ThrowByName(env,JNU_JAVANETPKG+"SocketTimeoutException",
                                        "Peek timed out");
                } else if (ret == JVM_IO_ERR) {
                    NET_ThrowCurrent(env, "timeout in datagram socket peek");
                } else if (ret == JVM_IO_INTR) {
                    JNU_ThrowByName(env, JNU_JAVAIOPKG+"InterruptedIOException",
                                    "operation interrupted");
                }
                return -1;
            }
            if (ret == 2) {
                fduse = checkLastFD (_this, fd, fd1);
            }
            checkBoth = FALSE;
        } else if (timeout != 0) {
            if (prevTime == 0) {
                prevTime = JVM_CurrentTimeMillis(env, 0);
            }
            ret = NET_Timeout (fduse, timeout);
            if (ret <= 0) {
                if (ret == 0) {
                    JNU_ThrowByName(env,JNU_JAVANETPKG+"SocketTimeoutException",
                                    "Receive timed out");
                } else if (ret == JVM_IO_ERR) {
                    JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                                    "Socket closed");
                } else if (ret == JVM_IO_INTR) {
                    JNU_ThrowByName(env, JNU_JAVAIOPKG+"InterruptedIOException",
                                    "operation interrupted");
                }
                return -1;
            }
        }

        /* receive the packet */
        n = recvfrom(fduse, packetBuffer, packetBufferOffset, packetBufferLen, MSG_PEEK, remote_addr);
        port = ntohs (GET_PORT(remote_addr));
        if (n == JVM_IO_ERR) {
            if (WSAGetLastError() == WSAECONNRESET) {
                boolean connected;

                /*
                 * An icmp port unreachable - we must receive this as Windows
                 * does not reset the state of the socket until this has been
                 * received.
                 */
                purgeOutstandingICMP(fduse);

                connected = _this.connected;
                if (connected) {
                    JNU_ThrowByName(env, JNU_JAVANETPKG+"PortUnreachableException",
                                       "ICMP Port Unreachable");

                    return -1;
                }

                /*
                 * If a timeout was specified then we need to adjust it because
                 * we may have used up some of the timeout befor the icmp port
                 * unreachable arrived.
                 */
                if (timeout != 0) {
                    long newTime = JVM_CurrentTimeMillis(env, 0);
                    timeout -= (newTime - prevTime);
                    if (timeout <= 0) {
                        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketTimeoutException",
                                "Receive timed out");
                        return -1;
                    }
                    prevTime = newTime;
                }
                retry = TRUE;
            }
        }
    } while (retry);

    if (n < 0) {
        errorCode = WSAGetLastError();
        /* check to see if it's because the buffer was too small */
        if (errorCode == WSAEMSGSIZE) {
            /* it is because the buffer is too small. It's UDP, it's
             * unreliable, it's all good. discard the rest of the
             * data..
             */
            n = packetBufferLen;
        } else {
            /* failure */
            packet.length = 0;
        }
    }
    if (n == -1) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "socket closed");
    } else if (n == -2) {
        JNU_ThrowByName(env, JNU_JAVAIOPKG+"InterruptedIOException",
                        "operation interrupted");
    } else if (n < 0) {
        NET_ThrowCurrent(env, "Datagram receive failed");
    } else {
        InetAddress packetAddress;

        /*
         * Check if there is an InetAddress already associated with this
         * packet. If so we check if it is the same source address. We
         * can't update any existing InetAddress because it is immutable
         */
        packetAddress = packet.address;
        if (packetAddress != NULL) {
            if (!NET_SockaddrEqualsInetAddress(remote_addr, packetAddress)) {
                /* force a new InetAddress to be created */
                packetAddress = null;
            }
        }
        if (packetAddress == NULL) {
            int[] tmp = { port };
            packetAddress = NET_SockaddrToInetAddress(remote_addr, tmp);
            port = tmp[0];
            /* stuff the new Inetaddress in the packet */
            packet.address = packetAddress;
        }

        /* populate the packet */
        packet.port = port;
        packet.length = n;
    }

    /* make sure receive() picks up the right fd */
    _this.fduse = fduse;

    return port;
}

/*
 * Class:     java_net_TwoStacksPlainDatagramSocketImpl
 * Method:    receive
 * Signature: (Ljava/net/DatagramPacket;)V
 */
static void receive0(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this, DatagramPacket packet) {
    FileDescriptor fdObj = _this.fd;
    FileDescriptor fd1Obj = _this.fd1;
    int timeout = _this.timeout;
    byte[] packetBuffer;
    int packetBufferOffset, packetBufferLen;
    boolean ipv6_supported = ipv6_available();

    /* as a result of the changes for ipv6, peek() or peekData()
     * must be called prior to receive() so that fduse can be set.
     */
    cli.System.Net.Sockets.Socket fd = null;
    cli.System.Net.Sockets.Socket fd1 = null;
    cli.System.Net.Sockets.Socket fduse = null;
    int errorCode;

    int n, nsockets=0;
    SOCKETADDRESS remote_addr;
    remote_addr = new SOCKETADDRESS();
    boolean retry;
    long prevTime = 0, selectTime=0;
    boolean connected;

    if (IS_NULL(fdObj) && IS_NULL(fd1Obj)) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "Socket closed");
        return;
    }

    if (!IS_NULL(fdObj)) {
        fd = fdObj.getSocket();
        nsockets ++;
    }
    if (!IS_NULL(fd1Obj)) {
        fd1 = fd1Obj.getSocket();
        nsockets ++;
    }

    if (nsockets == 2) { /* need to choose one of them */
        /* was fduse set in peek? */
        fduse = _this.fduse;
        if (fduse == null) {
            /* not set in peek(), must select on both sockets */
            int ret, t = (timeout == 0) ? -1: timeout;
            cli.System.Net.Sockets.Socket[] tmp = new cli.System.Net.Sockets.Socket[] { fduse };
            ret = NET_Timeout2 (fd, fd1, t, tmp);
            fduse = tmp[0];
            if (ret == 2) {
                fduse = checkLastFD (_this, fd, fd1);
            } else if (ret <= 0) {
                if (ret == 0) {
                    JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketTimeoutException",
                                    "Receive timed out");
                } else if (ret == JVM_IO_ERR) {
                    JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                                    "Socket closed");
                } else if (ret == JVM_IO_INTR) {
                    JNU_ThrowByName(env, JNU_JAVAIOPKG+"InterruptedIOException",
                                    "operation interrupted");
                }
                return;
            }
        }
    } else if (!ipv6_supported) {
        fduse = fd;
    } else if (IS_NULL(fdObj)) {
        /* ipv6 supported: and this socket bound to an IPV6 only address */
        fduse = fd1;
    } else {
        /* ipv6 supported: and this socket bound to an IPV4 only address */
        fduse = fd;
    }

    if (IS_NULL(packet)) {
        JNU_ThrowNullPointerException(env, "packet");
        return;
    }

    packetBuffer = packet.buf;

    if (IS_NULL(packetBuffer)) {
        JNU_ThrowNullPointerException(env, "packet buffer");
        return;
    }

    packetBufferOffset = packet.offset;
    packetBufferLen = packet.bufLength;

    /*
    if (packetBufferLen > MAX_BUFFER_LEN) {

        /* When JNI-ifying the JDK's IO routines, we turned
         * read's and write's of byte arrays of size greater
         * than 2048 bytes into several operations of size 2048.
         * This saves a malloc()/memcpy()/free() for big
         * buffers.  This is OK for file IO and TCP, but that
         * strategy violates the semantics of a datagram protocol.
         * (one big send) != (several smaller sends).  So here
         * we *must* alloc the buffer.  Note it needn't be bigger
         * than 65,536 (0xFFFF) the max size of an IP packet.
         * anything bigger is truncated anyway.
         *-/
        fullPacket = (char *)malloc(packetBufferLen);
        if (!fullPacket) {
            JNU_ThrowOutOfMemoryError(env, "Receive buf native heap allocation failed");
            return;
        }
    } else {
        fullPacket = &(BUF[0]);
    }
    */



    /*
     * If this Windows edition supports ICMP port unreachable and if we
     * are not connected then we need to know if a timeout has been specified
     * and if so we need to pick up the current time. These are required in
     * order to implement the semantics of timeout, viz :-
     * timeout set to t1 but ICMP port unreachable arrives in t2 where
     * t2 < t1. In this case we must discard the ICMP packets and then
     * wait for the next packet up to a maximum of t1 minus t2.
     */
    connected = _this.connected;
    if (supportPortUnreachable() && !connected && timeout != 0 &&!ipv6_supported) {
        prevTime = JVM_CurrentTimeMillis(env, 0);
    }

    if (timeout != 0 && nsockets == 1) {
        int ret;
        ret = NET_Timeout(fduse, timeout);
        if (ret <= 0) {
            if (ret == 0) {
                JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketTimeoutException",
                                "Receive timed out");
            } else if (ret == JVM_IO_ERR) {
                JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                                "Socket closed");
            } else if (ret == JVM_IO_INTR) {
                JNU_ThrowByName(env, JNU_JAVAIOPKG+"InterruptedIOException",
                                "operation interrupted");
            }
            return;
        }
    }

    /*
     * Loop only if we discarding ICMP port unreachable packets
     */
    do {
        retry = FALSE;

        /* receive the packet */
        n = recvfrom(fduse, packetBuffer, packetBufferOffset, packetBufferLen, 0, remote_addr);

        if (n == JVM_IO_ERR) {
            if (WSAGetLastError() == WSAECONNRESET) {
                /*
                 * An icmp port unreachable has been received - consume any other
                 * outstanding packets.
                 */
                purgeOutstandingICMP(fduse);

                /*
                 * If connected throw a PortUnreachableException
                 */

                if (connected) {
                    JNU_ThrowByName(env, JNU_JAVANETPKG+"PortUnreachableException",
                                       "ICMP Port Unreachable");
                    return;
                }

                /*
                 * If a timeout was specified then we need to adjust it because
                 * we may have used up some of the timeout before the icmp port
                 * unreachable arrived.
                 */
                if (timeout != 0) {
                    int ret;
                    long newTime = JVM_CurrentTimeMillis(env, 0);
                    timeout -= (newTime - prevTime);
                    prevTime = newTime;

                    if (timeout <= 0) {
                        ret = 0;
                    } else {
                        ret = NET_Timeout(fduse, timeout);
                    }

                    if (ret <= 0) {
                        if (ret == 0) {
                            JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketTimeoutException",
                                            "Receive timed out");
                        } else if (ret == JVM_IO_ERR) {
                            JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                                            "Socket closed");
                        } else if (ret == JVM_IO_INTR) {
                            JNU_ThrowByName(env, JNU_JAVAIOPKG+"InterruptedIOException",
                                            "operation interrupted");
                        }
                        return;
                    }
                }

                /*
                 * An ICMP port unreachable was received but we are
                 * not connected so ignore it.
                 */
                retry = TRUE;
            }
        }
    } while (retry);

    if (n < 0) {
        errorCode = WSAGetLastError();
        /* check to see if it's because the buffer was too small */
        if (errorCode == WSAEMSGSIZE) {
            /* it is because the buffer is too small. It's UDP, it's
             * unreliable, it's all good. discard the rest of the
             * data..
             */
            n = packetBufferLen;
        } else {
            /* failure */
            packet.length = 0;
        }
    }
    if (n == -1) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "socket closed");
    } else if (n == -2) {
        JNU_ThrowByName(env, JNU_JAVAIOPKG+"InterruptedIOException",
                        "operation interrupted");
    } else if (n < 0) {
        NET_ThrowCurrent(env, "Datagram receive failed");
    } else {
        int port;
        InetAddress packetAddress;

        /*
         * Check if there is an InetAddress already associated with this
         * packet. If so we check if it is the same source address. We
         * can't update any existing InetAddress because it is immutable
         */
        packetAddress = packet.address;

        if (packetAddress != NULL) {
            if (!NET_SockaddrEqualsInetAddress(remote_addr, packetAddress)) {
                /* force a new InetAddress to be created */
                packetAddress = null;
            }
        }
        if (packetAddress == NULL) {
            int[] tmp = { 0 };
            packetAddress = NET_SockaddrToInetAddress(remote_addr, tmp);
            port = tmp[0];
            /* stuff the new Inetaddress in the packet */
            packet.address = packetAddress;
        } else {
            /* only get the new port number */
            port = NET_GetPortFromSockaddr(remote_addr);
        }
        /* populate the packet */
        packet.port = port;
        packet.length = n;
    }
}

/*
 * Class:     java_net_TwoStacksPlainDatagramSocketImpl
 * Method:    datagramSocketCreate
 * Signature: ()V
 */
static void datagramSocketCreate(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this) {
    FileDescriptor fdObj = _this.fd;
    FileDescriptor fd1Obj = _this.fd1;
    cli.System.Net.Sockets.Socket fd = null;
    cli.System.Net.Sockets.Socket fd1 = null;
    boolean ipv6_supported = ipv6_available();

    if (IS_NULL(fdObj) || (ipv6_supported && IS_NULL(fd1Obj))) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "Socket closed");
        return;
    } else {
        fd =  socket (AF_INET, SOCK_DGRAM, 0);
    }
    if (fd == INVALID_SOCKET) {
        NET_ThrowCurrent(env, "Socket creation failed");
        return;
    }
    fdObj.setSocket(fd);
    NET_SetSockOpt(fd, SOL_SOCKET, SO_BROADCAST, true);

    if (ipv6_supported) {
        /* SIO_UDP_CONNRESET fixes a bug introduced in Windows 2000, which
         * returns connection reset errors un connected UDP sockets (as well
         * as connected sockets. The solution is to only enable this feature
         * when the socket is connected
         */
        WSAIoctl(fd,SIO_UDP_CONNRESET,false);
        fd1 = socket (AF_INET6, SOCK_DGRAM, 0);
        if (fd1 == INVALID_SOCKET) {
            NET_ThrowCurrent(env, "Socket creation failed");
            return;
        }
        NET_SetSockOpt(fd1, SOL_SOCKET, SO_BROADCAST, true);
        WSAIoctl(fd1,SIO_UDP_CONNRESET,false);
        fd1Obj.setSocket(fd1);
    } else {
        /* drop the second fd */
        _this.fd1 = null;
    }
}

/*
 * Class:     java_net_TwoStacksPlainDatagramSocketImpl
 * Method:    datagramSocketClose
 * Signature: ()V
 */
static void datagramSocketClose(TwoStacksPlainDatagramSocketImpl _this) {
    /*
     * REMIND: PUT A LOCK AROUND THIS CODE
     */
    FileDescriptor fdObj = _this.fd;
    FileDescriptor fd1Obj = _this.fd1;
    boolean ipv6_supported = ipv6_available();
    cli.System.Net.Sockets.Socket fd = null, fd1 = null;

    if (IS_NULL(fdObj) && (!ipv6_supported || IS_NULL(fd1Obj))) {
        return;
    }

    if (!IS_NULL(fdObj)) {
        fd = fdObj.getSocket();
        if (fd != null) {
            fdObj.setSocket(null);
            NET_SocketClose(fd);
        }
    }

    if (ipv6_supported && fd1Obj != NULL) {
        fd1 = fd1Obj.getSocket();
        if (fd1 == null) {
            return;
        }
        fd1Obj.setSocket(null);
        NET_SocketClose(fd1);
    }
}

/*
 * check the addresses attached to the NetworkInterface object
 * and return the first one (of the requested family Ipv4 or Ipv6)
 * in *iaddr
 */

private static int getInetAddrFromIf (JNIEnv env, int family, NetworkInterface nif, InetAddress[] iaddr)
{
    InetAddress[] addrArray;
    int len;
    InetAddress addr;
    int i;

    addrArray = getNetworkInterfaceAddresses(nif);
    len = addrArray.length;

    /*
     * Check that there is at least one address bound to this
     * interface.
     */
    if (len < 1) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
            "bad argument for IP_MULTICAST_IF2: No IP addresses bound to interface");
        return -1;
    }
    for (i=0; i<len; i++) {
        int fam;
        addr = addrArray[i];
        fam = getInetAddress_family(env, addr);
        if (fam == family) {
            iaddr[0] = addr;
            return 0;
        }
    }
    return -1;
}

private static int getInet4AddrFromIf (JNIEnv env, NetworkInterface nif, in_addr iaddr)
{
    InetAddress[] addr = new InetAddress[1];

    int ret = getInetAddrFromIf (env, IPv4, nif, addr);
    if (ret == -1) {
        return -1;
    }

    iaddr.s_addr = htonl(getInetAddress_addr(env, addr[0]));
    return 0;
}

/* Get the multicasting index from the interface */

private static int getIndexFromIf (JNIEnv env, NetworkInterface nif) {
    return nif.getIndex();
}

private static InetAddress[] getNetworkInterfaceAddresses(final NetworkInterface nif) {
    // [IKVM] this is IKVM specific, because I don't want to use reflection (or map.xml hacks) to access the "addrs" member of NetworkInterface
    return java.security.AccessController.doPrivileged(new java.security.PrivilegedAction<InetAddress[]>() {
        public InetAddress[] run() {
            java.util.ArrayList<InetAddress> list = new java.util.ArrayList<InetAddress>();
            for (java.util.Enumeration<InetAddress> e = nif.getInetAddresses(); e.hasMoreElements(); ) {
                list.add(e.nextElement());
            }
            return list.toArray(new InetAddress[list.size()]);
        }
    });
}

static int isAdapterIpv6Enabled(JNIEnv env, int index) {
    return java.security.AccessController.doPrivileged(new java.security.PrivilegedAction<Integer>() {
        public Integer run() {
            try {
                for (java.util.Enumeration<InetAddress> e = NetworkInterface.getByIndex(index).getInetAddresses(); e.hasMoreElements(); ) {
                    if (e.nextElement() instanceof Inet6Address) {
                        return 1;
                    }
                }
            } catch (SocketException x) {
            }
            return 0;
        }
    }).intValue();
}

private static NetworkInterface Java_java_net_NetworkInterface_getByIndex(JNIEnv env, int ni_class, int index)
{
    try {
        return NetworkInterface.getByIndex(index);
    } catch (Exception x) {
        env.Throw(x);
        return null;
    }
}

private static NetworkInterface Java_java_net_NetworkInterface_getByInetAddress0(JNIEnv env, int ni_class, Object address)
{
    try {
        return NetworkInterface.getByInetAddress((InetAddress)address);
    } catch (Exception x) {
        env.Throw(x);
        return null;
    }
}

/*
 * Sets the multicast interface.
 *
 * SocketOptions.IP_MULTICAST_IF (argument is an InetAddress) :-
 *      IPv4:   set outgoing multicast interface using
 *              IPPROTO_IP/IP_MULTICAST_IF
 *
 *      IPv6:   Get the interface to which the
 *              InetAddress is bound
 *              and do same as SockOptions.IF_MULTICAST_IF2
 *
 * SockOptions.IF_MULTICAST_IF2 (argument is a NetworkInterface ) :-
 *      For each stack:
 *      IPv4:   Obtain IP address bound to network interface
 *              (NetworkInterface.addres[0])
 *              set outgoing multicast interface using
 *              IPPROTO_IP/IP_MULTICAST_IF
 *
 *      IPv6:   Obtain NetworkInterface.index
 *              Set outgoing multicast interface using
 *              IPPROTO_IPV6/IPV6_MULTICAST_IF
 *
 */
private static void setMulticastInterface(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this, cli.System.Net.Sockets.Socket fd, cli.System.Net.Sockets.Socket fd1,
                                  int opt, Object value)
{
    boolean ipv6_supported = ipv6_available();

    if (opt == java_net_SocketOptions_IP_MULTICAST_IF) {
        /*
         * value is an InetAddress.
         * On IPv4 system use IP_MULTICAST_IF socket option
         * On IPv6 system get the NetworkInterface that this IP
         * address is bound to and use the IPV6_MULTICAST_IF
         * option instead of IP_MULTICAST_IF
         */
        if (ipv6_supported) {
            value = Java_java_net_NetworkInterface_getByInetAddress0(env, ni_class, value);
            if (value == NULL) {
                if (env.ExceptionOccurred() == null) {
                    JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                         "bad argument for IP_MULTICAST_IF"
                        +": address not bound to any interface");
                }
                return;
            }
            opt = java_net_SocketOptions_IP_MULTICAST_IF2;
        } else {
            in_addr in = new in_addr();

            in.s_addr = htonl(getInetAddress_addr(env, (InetAddress)value));

            if (setsockopt(fd, IPPROTO_IP, IP_MULTICAST_IF,
                               in) < 0) {
                NET_ThrowByNameWithLastError(env, JNU_JAVANETPKG+"SocketException",
                                 "Error setting socket option");
            }
            return;
        }
    }

    if (opt == java_net_SocketOptions_IP_MULTICAST_IF2) {
        /*
         * value is a NetworkInterface.
         * On IPv6 system get the index of the interface and use the
         * IPV6_MULTICAST_IF socket option
         * On IPv4 system extract addr[0] and use the IP_MULTICAST_IF
         * option. For IPv6 both must be done.
         */
        if (ipv6_supported) {
            in_addr in = new in_addr();
            int index;

            index = ((NetworkInterface)value).getIndex();

            if ( isAdapterIpv6Enabled(env, index) != 0 ) {
                if (setsockopt(fd1, IPPROTO_IPV6, IPV6_MULTICAST_IF,
                               index) < 0) {
                    if (WSAGetLastError() == WSAEINVAL && index > 0) {
                        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                            "IPV6_MULTICAST_IF failed (interface has IPv4 "
                           +"address only?)");
                    } else {
                        NET_ThrowByNameWithLastError(env, JNU_JAVANETPKG+"SocketException",
                                   "Error setting socket option");
                    }
                    return;
                }
            }

            /* If there are any IPv4 addresses on this interface then
             * repeat the operation on the IPv4 fd */

            if (getInet4AddrFromIf (env, (NetworkInterface)value, in) < 0) {
                return;
            }
            if (setsockopt(fd, IPPROTO_IP, IP_MULTICAST_IF,
                               in) < 0) {
                NET_ThrowByNameWithLastError(env, JNU_JAVANETPKG+"SocketException",
                                 "Error setting socket option");
            }
            return;
        } else {
            in_addr in = new in_addr();

            if (getInet4AddrFromIf (env, (NetworkInterface)value, in) < 0) {
                if (env.ExceptionOccurred() != null) {
                    return;
                }
                JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "no InetAddress instances of requested type");
                return;
            }

            if (setsockopt(fd, IPPROTO_IP, IP_MULTICAST_IF,
                               in) < 0) {
                NET_ThrowByNameWithLastError(env, JNU_JAVANETPKG+"SocketException",
                               "Error setting socket option");
            }
            return;
        }
    }
}

/*
 * Class:     java_net_TwoStacksPlainDatagramSocketImpl
 * Method:    socketNativeSetOption
 * Signature: (ILjava/lang/Object;)V
 */
static void socketNativeSetOption(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this, int opt, Object value) {
    cli.System.Net.Sockets.Socket fd = null;
    cli.System.Net.Sockets.Socket fd1 = null;
    int[] levelv4 = new int[1];
    int[] levelv6 = new int[1];
    int[] optnamev4 = new int[1];
    int[] optnamev6 = new int[1];
    Object optval;
    boolean ipv6_supported = ipv6_available();

    fd = getFD(env, _this);

    if (ipv6_supported) {
        fd1 = getFD1(env, _this);
    }
    if (fd == null && fd1 == null) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "socket closed");
        return;
    }

    if ((opt == java_net_SocketOptions_IP_MULTICAST_IF) ||
        (opt == java_net_SocketOptions_IP_MULTICAST_IF2)) {

        setMulticastInterface(env, _this, fd, fd1, opt, value);
        return;
    }

    /*
     * Map the Java level socket option to the platform specific
     * level(s) and option name(s).
     */
    if (fd1 != null) {
        if (NET_MapSocketOptionV6(opt, levelv6, optnamev6) != 0) {
            JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "Invalid option");
            return;
        }
    }
    if (fd != null) {
        if (NET_MapSocketOption(opt, levelv4, optnamev4) != 0) {
            JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "Invalid option");
            return;
        }
    }

    switch (opt) {
        case java_net_SocketOptions_SO_SNDBUF :
        case java_net_SocketOptions_SO_RCVBUF :
        case java_net_SocketOptions_IP_TOS :
            optval = ((Integer)value).intValue();
            break;

        case java_net_SocketOptions_SO_REUSEADDR:
        case java_net_SocketOptions_SO_BROADCAST:
        case java_net_SocketOptions_IP_MULTICAST_LOOP:
            {
                boolean on = ((Boolean)value).booleanValue();
                optval = on;
                /*
                 * setLoopbackMode (true) disables IP_MULTICAST_LOOP rather
                 * than enabling it.
                 */
                if (opt == java_net_SocketOptions_IP_MULTICAST_LOOP) {
                    optval = !on;
                }
            }
            break;

        default :
            JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                "Socket option not supported by PlainDatagramSocketImp");
            return;
    }

    if (fd1 != null) {
        if (NET_SetSockOpt(fd1, levelv6[0], optnamev6[0], optval) < 0) {
            NET_ThrowCurrent(env, "setsockopt IPv6");
            return;
        }
    }
    if (fd != null) {
        if (NET_SetSockOpt(fd, levelv4[0], optnamev4[0], optval) < 0) {
            NET_ThrowCurrent(env, "setsockopt");
            return;
        }
    }
}

/*
 *
 * called by getMulticastInterface to retrieve a NetworkInterface
 * configured for IPv4.
 * The ipv4Mode parameter, is a closet boolean, which allows for a NULL return,
 * or forces the creation of a NetworkInterface object with null data.
 * It relates to its calling context in getMulticastInterface.
 * ipv4Mode == 1, the context is IPV4 processing only.
 * ipv4Mode == 0, the context is IPV6 processing
 *
 *-/
static jobject getIPv4NetworkInterface (JNIEnv *env, jobject this, int fd, jint opt, int ipv4Mode) {
        static jclass inet4_class;
        static jmethodID inet4_ctrID;

        static jclass ni_class; static jmethodID ni_ctrID;
        static jfieldID ni_indexID;
        static jfieldID ni_addrsID;

        jobjectArray addrArray;
        jobject addr;
        jobject ni;

        struct in_addr in;
        struct in_addr *inP = &in;
        int len = sizeof(struct in_addr);
        if (getsockopt(fd, IPPROTO_IP, IP_MULTICAST_IF,
                           (char *)inP, &len) < 0) {
            NET_ThrowByNameWithLastError(env, JNU_JAVANETPKG "SocketException",
                             "Error getting socket option");
            return NULL;
        }

        /*
         * Construct and populate an Inet4Address
         *-/
        if (inet4_class == NULL) {
            jclass c = (*env)->FindClass(env, "java/net/Inet4Address");
            CHECK_NULL_RETURN(c, NULL);
            inet4_ctrID = (*env)->GetMethodID(env, c, "<init>", "()V");
            CHECK_NULL_RETURN(inet4_ctrID, NULL);
            inet4_class = (*env)->NewGlobalRef(env, c);
            CHECK_NULL_RETURN(inet4_class, NULL);
        }
        addr = (*env)->NewObject(env, inet4_class, inet4_ctrID, 0);
        CHECK_NULL_RETURN(addr, NULL);

        setInetAddress_addr(env, addr, ntohl(in.s_addr));

        /*
         * For IP_MULTICAST_IF return InetAddress
         *-/
        if (opt == java_net_SocketOptions_IP_MULTICAST_IF) {
            return addr;
        }

        /*
         * For IP_MULTICAST_IF2 we get the NetworkInterface for
         * this address and return it
         *-/
        if (ni_class == NULL) {
            jclass c = (*env)->FindClass(env, "java/net/NetworkInterface");
            CHECK_NULL_RETURN(c, NULL);
            ni_ctrID = (*env)->GetMethodID(env, c, "<init>", "()V");
            CHECK_NULL_RETURN(ni_ctrID, NULL);
            ni_indexID = (*env)->GetFieldID(env, c, "index", "I");
            CHECK_NULL_RETURN(ni_indexID, NULL);
            ni_addrsID = (*env)->GetFieldID(env, c, "addrs",
                                            "[Ljava/net/InetAddress;");
            CHECK_NULL_RETURN(ni_addrsID, NULL);
            ni_class = (*env)->NewGlobalRef(env, c);
            CHECK_NULL_RETURN(ni_class, NULL);
        }
        ni = Java_java_net_NetworkInterface_getByInetAddress0(env, ni_class, addr);
        if (ni) {
            return ni;
        }
        if (ipv4Mode) {
            ni = (*env)->NewObject(env, ni_class, ni_ctrID, 0);
            CHECK_NULL_RETURN(ni, NULL);

            (*env)->SetIntField(env, ni, ni_indexID, -1);
            addrArray = (*env)->NewObjectArray(env, 1, inet4_class, NULL);
            CHECK_NULL_RETURN(addrArray, NULL);
            (*env)->SetObjectArrayElement(env, addrArray, 0, addr);
            (*env)->SetObjectField(env, ni, ni_addrsID, addrArray);
        } else {
            ni = NULL;
        }
        return ni;
}

/*
 * Return the multicast interface:
 *
 * SocketOptions.IP_MULTICAST_IF
 *      IPv4:   Query IPPROTO_IP/IP_MULTICAST_IF
 *              Create InetAddress
 *              IP_MULTICAST_IF returns struct ip_mreqn on 2.2
 *              kernel but struct in_addr on 2.4 kernel
 *      IPv6:   Query IPPROTO_IPV6 / IPV6_MULTICAST_IF or
 *              obtain from impl is Linux 2.2 kernel
 *              If index == 0 return InetAddress representing
 *              anyLocalAddress.
 *              If index > 0 query NetworkInterface by index
 *              and returns addrs[0]
 *
 * SocketOptions.IP_MULTICAST_IF2
 *      IPv4:   Query IPPROTO_IP/IP_MULTICAST_IF
 *              Query NetworkInterface by IP address and
 *              return the NetworkInterface that the address
 *              is bound too.
 *      IPv6:   Query IPPROTO_IPV6 / IPV6_MULTICAST_IF
 *              (except Linux .2 kernel)
 *              Query NetworkInterface by index and
 *              return NetworkInterface.
 */
private static Object getMulticastInterface(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this, cli.System.Net.Sockets.Socket fd, cli.System.Net.Sockets.Socket fd1, int opt) {
    boolean isIPV4 = !ipv6_available() || fd1 == null;

    /*
     * IPv4 implementation
     */
    if (isIPV4) {
        Inet4Address addr;

        in_addr in = new in_addr();

        if (getsockopt(fd, IPPROTO_IP, IP_MULTICAST_IF,
                           in) < 0) {
            NET_ThrowByNameWithLastError(env, JNU_JAVANETPKG+"SocketException",
                             "Error getting socket option");
            return NULL;
        }

        /*
         * Construct and populate an Inet4Address
         */
        addr = new Inet4Address();
        addr.holder().address = ntohl(in.s_addr);

        /*
         * For IP_MULTICAST_IF return InetAddress
         */
        if (opt == java_net_SocketOptions_IP_MULTICAST_IF) {
            return addr;
        }

        NetworkInterface ni;
        ni = Java_java_net_NetworkInterface_getByInetAddress0(env, ni_class, addr);
        if (ni != null) {
            return ni;
        }

        /*
         * The address doesn't appear to be bound at any known
         * NetworkInterface. Therefore we construct a NetworkInterface
         * with this address.
         */
        return new NetworkInterface(null, -1, new InetAddress[] { addr });
    }


    /*
     * IPv6 implementation
     */
    if ((opt == java_net_SocketOptions_IP_MULTICAST_IF) ||
        (opt == java_net_SocketOptions_IP_MULTICAST_IF2)) {

        int index;

        InetAddress[] addrArray;
        InetAddress addr;
        NetworkInterface ni;

        {
            int[] tmp = { 0 };
            if (getsockopt(fd1, IPPROTO_IPV6, IPV6_MULTICAST_IF,
                               tmp) < 0) {
                NET_ThrowByNameWithLastError(env, JNU_JAVANETPKG+"SocketException",
                               "Error getting socket option");
                return NULL;
            }
            index = tmp[0];
        }

        /*
         * If multicast to a specific interface then return the
         * interface (for IF2) or the any address on that interface
         * (for IF).
         */
        if (index > 0) {
            ni = Java_java_net_NetworkInterface_getByIndex(env, ni_class,
                                                                   index);
            if (ni == NULL) {
                String errmsg = "IPV6_MULTICAST_IF returned index to unrecognized interface: " + index;
                JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", errmsg);
                return NULL;
            }

            /*
             * For IP_MULTICAST_IF2 return the NetworkInterface
             */
            if (opt == java_net_SocketOptions_IP_MULTICAST_IF2) {
                return ni;
            }

            /*
             * For IP_MULTICAST_IF return addrs[0]
             */
            addrArray = getNetworkInterfaceAddresses(ni);
            if (addrArray.length < 1) {
                JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                    "IPV6_MULTICAST_IF returned interface without IP bindings");
                return NULL;
            }

            addr = addrArray[0];
            return addr;
        }

        /*
         * Multicast to any address - return anyLocalAddress
         * or a NetworkInterface with addrs[0] set to anyLocalAddress
         */

        addr = InetAddress.anyLocalAddress();
        if (opt == java_net_SocketOptions_IP_MULTICAST_IF) {
            return addr;
        }

        return new NetworkInterface(null, -1, new InetAddress[] { addr });
    }
    return NULL;
}
/*
 * Returns relevant info as a jint.
 *
 * Class:     java_net_TwoStacksPlainDatagramSocketImpl
 * Method:    socketGetOption
 * Signature: (I)Ljava/lang/Object;
 */
static Object socketGetOption(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this, int opt) {
    cli.System.Net.Sockets.Socket fd = null;
    cli.System.Net.Sockets.Socket fd1 = null;
    int[] level = new int[1];
    int[] optname = new int[1];
    int[] optval = new int[1];
    boolean ipv6_supported = ipv6_available();

    fd = getFD(env, _this);
    if (ipv6_supported) {
        fd1 = getFD1(env, _this);
    }

    if (fd == null && fd1 ==  null) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "Socket closed");
        return NULL;
    }

    /*
     * Handle IP_MULTICAST_IF separately
     */
    if (opt == java_net_SocketOptions_IP_MULTICAST_IF ||
        opt == java_net_SocketOptions_IP_MULTICAST_IF2) {
        return getMulticastInterface(env, _this, fd, fd1, opt);
    }

    /*
     * Map the Java level socket option to the platform specific
     * level and option name.
     */
    if (NET_MapSocketOption(opt, level, optname) != 0) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "Invalid option");
        return NULL;
    }

    if (fd == null) {
        if (NET_MapSocketOptionV6(opt, level, optname) != 0) {
            JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "Invalid option");
            return NULL;
        }
        fd = fd1; /* must be IPv6 only */
    }

    if (NET_GetSockOpt(fd, level[0], optname[0], optval) < 0) {
        String errmsg = "error getting socket option: " + WSAGetLastError();
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", errmsg);
        return NULL;
    }

    switch (opt) {
        case java_net_SocketOptions_SO_BROADCAST:
        case java_net_SocketOptions_SO_REUSEADDR:
            return optval[0] != 0;

        case java_net_SocketOptions_IP_MULTICAST_LOOP:
            /* getLoopbackMode() returns true if IP_MULTICAST_LOOP is disabled */
            return optval[0] == 0;

        case java_net_SocketOptions_SO_SNDBUF:
        case java_net_SocketOptions_SO_RCVBUF:
        case java_net_SocketOptions_IP_TOS:
            return optval[0];

        default :
            JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                "Socket option not supported by TwoStacksPlainDatagramSocketImpl");
            return NULL;

    }
}

/*
 * Returns local address of the socket.
 *
 * Class:     java_net_TwoStacksPlainDatagramSocketImpl
 * Method:    socketLocalAddress
 * Signature: (I)Ljava/lang/Object;
 */
static Object socketLocalAddress(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this,
                                                      int family) {
    cli.System.Net.Sockets.Socket fd = null;
    cli.System.Net.Sockets.Socket fd1 = null;
    SOCKETADDRESS him;
    him = new SOCKETADDRESS();
    Object iaObj;
    boolean ipv6_supported = ipv6_available();

    fd = getFD(env, _this);
    if (ipv6_supported) {
        fd1 = getFD1(env, _this);
    }

    if (fd == null && fd1 == null) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "Socket closed");
        return NULL;
    }

    /* find out local IP address */

    /* family==-1 when socket is not connected */
    if ((family == IPv6) || (family == -1 && fd == null)) {
        fd = fd1; /* must be IPv6 only */
    }

    if (fd == null) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "Socket closed");
        return NULL;
    }

    if (getsockname(fd, him) == -1) {
        NET_ThrowByNameWithLastError(env, JNU_JAVANETPKG+"SocketException",
                       "Error getting socket name");
        return NULL;
    }
    iaObj = NET_SockaddrToInetAddress(him, new int[1]);

    return iaObj;
}

/*
 * Class:     java_net_TwoStacksPlainDatagramSocketImpl
 * Method:    setTimeToLive
 * Signature: (I)V
 */
static void setTimeToLive(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this, int ttl) {
    FileDescriptor fdObj = _this.fd;
    FileDescriptor fd1Obj = _this.fd1;
    cli.System.Net.Sockets.Socket fd = null;
    cli.System.Net.Sockets.Socket fd1 = null;

    if (IS_NULL(fdObj) && IS_NULL(fd1Obj)) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "Socket closed");
        return;
    } else {
      if (!IS_NULL(fdObj)) {
        fd = fdObj.getSocket();
      }
      if (!IS_NULL(fd1Obj)) {
        fd1 = fd1Obj.getSocket();
      }
    }

    /* setsockopt to be correct ttl */
    if (fd != null) {
      if (NET_SetSockOpt(fd, IPPROTO_IP, IP_MULTICAST_TTL, ttl) < 0) {
        NET_ThrowCurrent(env, "set IP_MULTICAST_TTL failed");
      }
    }

    if (fd1 != null) {
      if (NET_SetSockOpt(fd1, IPPROTO_IPV6, IPV6_MULTICAST_HOPS, ttl) <0) {
        NET_ThrowCurrent(env, "set IPV6_MULTICAST_HOPS failed");
      }
    }
}

/*
 * Class:     java_net_TwoStacksPlainDatagramSocketImpl
 * Method:    setTTL
 * Signature: (B)V
 */
static void setTTL(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this, byte ttl) {
    setTimeToLive(env, _this, ttl & 0xFF);
}

/*
 * Class:     java_net_TwoStacksPlainDatagramSocketImpl
 * Method:    getTimeToLive
 * Signature: ()I
 */
static int getTimeToLive(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this) {
    FileDescriptor fdObj = _this.fd;
    FileDescriptor fd1Obj = _this.fd1;
    cli.System.Net.Sockets.Socket fd = null;
    cli.System.Net.Sockets.Socket fd1 = null;
    int[] ttl = new int[1];

    if (IS_NULL(fdObj) && IS_NULL(fd1Obj)) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "Socket closed");
        return -1;
    } else {
      if (!IS_NULL(fdObj)) {
        fd = fdObj.getSocket();
      }
      if (!IS_NULL(fd1Obj)) {
        fd1 = fd1Obj.getSocket();
      }
    }

    /* getsockopt of ttl */
    if (fd != null) {
      if (NET_GetSockOpt(fd, IPPROTO_IP, IP_MULTICAST_TTL, ttl) < 0) {
        NET_ThrowCurrent(env, "get IP_MULTICAST_TTL failed");
        return -1;
      }
      return ttl[0];
    }
    if (fd1 != null) {
      if (NET_GetSockOpt(fd1, IPPROTO_IPV6, IPV6_MULTICAST_HOPS, ttl) < 0) {
        NET_ThrowCurrent(env, "get IP_MULTICAST_TTL failed");
        return -1;
      }
      return ttl[0];
    }
    return -1;
}

/*
 * Class:     java_net_TwoStacksPlainDatagramSocketImpl
 * Method:    getTTL
 * Signature: ()B
 */
static byte getTTL(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this) {
    int result = getTimeToLive(env, _this);

    return (byte)result;
}

/* join/leave the named group on the named interface, or if no interface specified
 * then the interface set with setInterfac(), or the default interface otherwise */

private static void mcast_join_leave(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this, InetAddress iaObj, NetworkInterface niObj, boolean join)
{
    FileDescriptor fdObj = _this.fd;
    FileDescriptor fd1Obj = _this.fd1;
    cli.System.Net.Sockets.Socket fd = null;
    cli.System.Net.Sockets.Socket fd1 = null;

    SOCKETADDRESS name;
    name = new SOCKETADDRESS();
    ip_mreq mname = new ip_mreq();
    ipv6_mreq mname6 = new ipv6_mreq();

    in_addr in = new in_addr();
    int ifindex;

    int family;
    boolean ipv6_supported = ipv6_available();
    int cmd ;

    if (IS_NULL(fdObj) && IS_NULL(fd1Obj)) {
        JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                        "Socket closed");
        return;
    }
    if (!IS_NULL(fdObj)) {
        fd = fdObj.getSocket();
    }
    if (ipv6_supported && !IS_NULL(fd1Obj)) {
        fd1 = fd1Obj.getSocket();
    }

    if (IS_NULL(iaObj)) {
        JNU_ThrowNullPointerException(env, "address");
        return;
    }

    if (NET_InetAddressToSockaddr(env, iaObj, 0, name, JNI_FALSE) != 0) {
      return;
    }

    /* Set the multicast group address in the ip_mreq field
     * eventually this check should be done by the security manager
     */
    family = name.him.sa_family;

    if (family == AF_INET) {
        int address = name.him4.sin_addr.s_addr;
        if (!IN_MULTICAST(ntohl(address))) {
            JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "not in multicast");
            return;
        }
        mname.imr_multiaddr.s_addr = address;
        if (fd == null) {
          JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "Can't join an IPv4 group on an IPv6 only socket");
          return;
        }
        if (IS_NULL(niObj)) {
            if (NET_GetSockOpt(fd, IPPROTO_IP, IP_MULTICAST_IF, in) < 0) {
                NET_ThrowCurrent(env, "get IP_MULTICAST_IF failed");
                return;
            }
            mname.imr_interface.s_addr = in.s_addr;
        } else {
            if (getInet4AddrFromIf (env, niObj, mname.imr_interface) != 0) {
                NET_ThrowCurrent(env, "no Inet4Address associated with interface");
                return;
            }
        }

        cmd = join ? IP_ADD_MEMBERSHIP: IP_DROP_MEMBERSHIP;

        /* Join the multicast group */
        if (NET_SetSockOpt(fd, IPPROTO_IP, cmd, mname) < 0) {
            if (WSAGetLastError() == WSAENOBUFS) {
                JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                    "IP_ADD_MEMBERSHIP failed (out of hardware filters?)");
            } else {
                JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException","error setting options");
            }
        }
    } else /* AF_INET6 */ {
        if (ipv6_supported) {
            in6_addr address;
            address = in6_addr.FromSockAddr(name);
            if (!IN6_IS_ADDR_MULTICAST(address)) {
                JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "not in6 multicast");
                return;
            }
            mname6.ipv6mr_multiaddr = address;
        } else {
            JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "IPv6 not supported");
            return;
        }
        if (fd1 == null) {
          JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException", "Can't join an IPv6 group on a IPv4 socket");
          return;
        }
        if (IS_NULL(niObj)) {
            int[] tmp = { 0 };
            if (NET_GetSockOpt(fd1, IPPROTO_IPV6, IPV6_MULTICAST_IF, tmp) < 0) {
                NET_ThrowCurrent(env, "get IPV6_MULTICAST_IF failed");
                return;
            }
            ifindex = tmp[0];
        } else {
            ifindex = getIndexFromIf (env, niObj);
            if (ifindex == -1) {
                NET_ThrowCurrent(env, "get ifindex failed");
                return;
            }
        }
        mname6.ipv6mr_interface = ifindex;
        cmd = join ? IPV6_ADD_MEMBERSHIP: IPV6_DROP_MEMBERSHIP;

        /* Join the multicast group */
        if (NET_SetSockOpt(fd1, IPPROTO_IPV6, cmd, mname6) < 0) {
            if (WSAGetLastError() == WSAENOBUFS) {
                JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException",
                    "IP_ADD_MEMBERSHIP failed (out of hardware filters?)");
            } else {
                JNU_ThrowByName(env, JNU_JAVANETPKG+"SocketException","error setting options");
            }
        }
    }

    return;
}

/*
 * Class:     java_net_TwoStacksPlainDatagramSocketImpl
 * Method:    join
 * Signature: (Ljava/net/InetAddress;)V
 */
static void join(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this, InetAddress inetaddr, NetworkInterface netIf) {
    mcast_join_leave(env, _this, inetaddr, netIf, true);
}

/*
 * Class:     java_net_TwoStacksPlainDatagramSocketImpl
 * Method:    leave
 * Signature: (Ljava/net/InetAddress;)V
 */
static void leave(JNIEnv env, TwoStacksPlainDatagramSocketImpl _this, InetAddress inetaddr, NetworkInterface netIf) {
    mcast_join_leave(env, _this, inetaddr, netIf, false);
}

}
