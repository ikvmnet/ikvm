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

import java.io.IOException;
import java.io.FileDescriptor;

/**
 * This class defines the plain SocketImpl that is used on Windows platforms
 * greater or equal to Windows Vista. These platforms have a dual
 * layer TCP/IP stack and can handle both IPv4 and IPV6 through a
 * single file descriptor.
 *
 * @author Chris Hegarty
 */

class DualStackPlainSocketImpl extends AbstractPlainSocketImpl
{


    // true if this socket is exclusively bound
    private final boolean exclusiveBind;

    // emulates SO_REUSEADDR when exclusiveBind is true
    private boolean isReuseAddress;

    public DualStackPlainSocketImpl(boolean exclBind) {
        exclusiveBind = exclBind;
    }

    public DualStackPlainSocketImpl(FileDescriptor fd, boolean exclBind) {
        this.fd = fd;
        exclusiveBind = exclBind;
    }

    void socketCreate(boolean stream) throws IOException {
        if (fd == null)
            throw new SocketException("Socket closed");

        cli.System.Net.Sockets.Socket newfd = socket0(stream, false /*v6 Only*/);

        fd.setSocket(newfd);
    }

    void socketConnect(InetAddress address, int port, int timeout)
        throws IOException {
        cli.System.Net.Sockets.Socket nativefd = checkAndReturnNativeFD();

        if (address == null)
            throw new NullPointerException("inet address argument is null.");

        int connectResult;
        if (timeout <= 0) {
            connectResult = connect0(nativefd, address, port);
        } else {
            configureBlocking(nativefd, false);
            try {
                connectResult = connect0(nativefd, address, port);
                if (connectResult == WOULDBLOCK) {
                    waitForConnect(nativefd, timeout);
                }
            } finally {
                configureBlocking(nativefd, true);
            }
        }
        /*
         * We need to set the local port field. If bind was called
         * previous to the connect (by the client) then localport field
         * will already be set.
         */
        if (localport == 0)
            localport = localPort0(nativefd);
    }

    void socketBind(InetAddress address, int port) throws IOException {
        cli.System.Net.Sockets.Socket nativefd = checkAndReturnNativeFD();

        if (address == null)
            throw new NullPointerException("inet address argument is null.");

        bind0(nativefd, address, port, exclusiveBind);
        if (port == 0) {
            localport = localPort0(nativefd);
        } else {
            localport = port;
        }

        this.address = address;
    }

    void socketListen(int backlog) throws IOException {
        cli.System.Net.Sockets.Socket nativefd = checkAndReturnNativeFD();

        listen0(nativefd, backlog);
    }

    void socketAccept(SocketImpl s) throws IOException {
        cli.System.Net.Sockets.Socket nativefd = checkAndReturnNativeFD();

        if (s == null)
            throw new NullPointerException("socket is null");

        cli.System.Net.Sockets.Socket newfd = null;
        InetSocketAddress[] isaa = new InetSocketAddress[1];
        if (timeout <= 0) {
            newfd = accept0(nativefd, isaa);
        } else {
            configureBlocking(nativefd, false);
            try {
                waitForNewConnection(nativefd, timeout);
                newfd = accept0(nativefd, isaa);
                if (newfd != null) {
                    configureBlocking(newfd, true);
                }
            } finally {
                configureBlocking(nativefd, true);
            }
        }
        /* Update (SocketImpl)s' fd */
        s.fd.setSocket(newfd);
        /* Update socketImpls remote port, address and localport */
        InetSocketAddress isa = isaa[0];
        s.port = isa.getPort();
        s.address = isa.getAddress();
        s.localport = localport;
    }

    int socketAvailable() throws IOException {
        cli.System.Net.Sockets.Socket nativefd = checkAndReturnNativeFD();
        return available0(nativefd);
    }

    void socketClose0(boolean useDeferredClose/*unused*/) throws IOException {
        if (fd == null)
            throw new SocketException("Socket closed");

        if (!fd.valid())
            return;

        cli.System.Net.Sockets.Socket nativefd = fd.getSocket();
        fd.setSocket(null);
        close0(nativefd);
    }

    void socketShutdown(int howto) throws IOException {
        cli.System.Net.Sockets.Socket nativefd = checkAndReturnNativeFD();
        shutdown0(nativefd, howto);
    }

    // Intentional fallthrough after SO_REUSEADDR
    @SuppressWarnings("fallthrough")
    void socketSetOption(int opt, boolean on, Object value)
        throws SocketException {
        cli.System.Net.Sockets.Socket nativefd = checkAndReturnNativeFD();

        if (opt == SO_TIMEOUT) {  // timeout implemented through select.
            return;
        }

        int optionValue = 0;

        switch(opt) {
            case SO_REUSEADDR :
                if (exclusiveBind) {
                    // SO_REUSEADDR emulated when using exclusive bind
                    isReuseAddress = on;
                    return;
                }
                // intentional fallthrough
            case TCP_NODELAY :
            case SO_OOBINLINE :
            case SO_KEEPALIVE :
                optionValue = on ? 1 : 0;
                break;
            case SO_SNDBUF :
            case SO_RCVBUF :
            case IP_TOS :
                optionValue = ((Integer)value).intValue();
                break;
            case SO_LINGER :
                if (on) {
                    optionValue =  ((Integer)value).intValue();
                } else {
                    optionValue = -1;
                }
                break;
            default :/* shouldn't get here */
                throw new SocketException("Option not supported");
        }

        setIntOption(nativefd, opt, optionValue);
    }

    int socketGetOption(int opt, Object iaContainerObj) throws SocketException {
        cli.System.Net.Sockets.Socket nativefd = checkAndReturnNativeFD();

        // SO_BINDADDR is not a socket option.
        if (opt == SO_BINDADDR) {
            localAddress(nativefd, (InetAddressContainer)iaContainerObj);
            return 0;  // return value doesn't matter.
        }

        // SO_REUSEADDR emulated when using exclusive bind
        if (opt == SO_REUSEADDR && exclusiveBind)
            return isReuseAddress? 1 : -1;

        int value = getIntOption(nativefd, opt);

        switch (opt) {
            case TCP_NODELAY :
            case SO_OOBINLINE :
            case SO_KEEPALIVE :
            case SO_REUSEADDR :
                return (value == 0) ? -1 : 1;
        }
        return value;
    }

    void socketSendUrgentData(int data) throws IOException {
        cli.System.Net.Sockets.Socket nativefd = checkAndReturnNativeFD();
        sendOOB(nativefd, data);
    }

    private cli.System.Net.Sockets.Socket checkAndReturnNativeFD() throws SocketException {
        if (fd == null || !fd.valid())
            throw new SocketException("Socket closed");

        return fd.getSocket();
    }

    static final int WOULDBLOCK = -2;       // Nothing available (non-blocking)

    /* Native methods */

    static cli.System.Net.Sockets.Socket socket0(boolean stream, boolean v6Only) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        cli.System.Net.Sockets.Socket ret = DualStackPlainSocketImpl_c.socket0(env, stream, v6Only);
        env.ThrowPendingException();
        return ret;
    }

    static void bind0(cli.System.Net.Sockets.Socket fd, InetAddress localAddress, int localport,
                             boolean exclBind)
        throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        DualStackPlainSocketImpl_c.bind0(env, fd, localAddress, localport, exclBind);
        env.ThrowPendingException();
    }

    static int connect0(cli.System.Net.Sockets.Socket fd, InetAddress remote, int remotePort)
        throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        int ret = DualStackPlainSocketImpl_c.connect0(env, fd, remote, remotePort);
        env.ThrowPendingException();
        return ret;
    }

    static void waitForConnect(cli.System.Net.Sockets.Socket fd, int timeout) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        DualStackPlainSocketImpl_c.waitForConnect(env, fd, timeout);
        env.ThrowPendingException();
    }

    static int localPort0(cli.System.Net.Sockets.Socket fd) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        int ret = DualStackPlainSocketImpl_c.localPort0(env, fd);
        env.ThrowPendingException();
        return ret;
    }

    static void localAddress(cli.System.Net.Sockets.Socket fd, InetAddressContainer in) throws SocketException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        DualStackPlainSocketImpl_c.localAddress(env, fd, in);
        env.ThrowPendingException();
    }

    static void listen0(cli.System.Net.Sockets.Socket fd, int backlog) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        DualStackPlainSocketImpl_c.listen0(env, fd, backlog);
        env.ThrowPendingException();
    }

    static cli.System.Net.Sockets.Socket accept0(cli.System.Net.Sockets.Socket fd, InetSocketAddress[] isaa) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        cli.System.Net.Sockets.Socket ret = DualStackPlainSocketImpl_c.accept0(env, fd, isaa);
        env.ThrowPendingException();
        return ret;
    }

    static void waitForNewConnection(cli.System.Net.Sockets.Socket fd, int timeout) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        DualStackPlainSocketImpl_c.waitForNewConnection(env, fd, timeout);
        env.ThrowPendingException();
    }

    static int available0(cli.System.Net.Sockets.Socket fd) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        int ret = DualStackPlainSocketImpl_c.available0(env, fd);
        env.ThrowPendingException();
        return ret;
    }

    static void close0(cli.System.Net.Sockets.Socket fd) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        DualStackPlainSocketImpl_c.close0(env, fd);
        env.ThrowPendingException();
    }

    static void shutdown0(cli.System.Net.Sockets.Socket fd, int howto) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        DualStackPlainSocketImpl_c.shutdown0(env, fd, howto);
        env.ThrowPendingException();
    }

    static void setIntOption(cli.System.Net.Sockets.Socket fd, int cmd, int optionValue) throws SocketException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        DualStackPlainSocketImpl_c.setIntOption(env, fd, cmd, optionValue);
        env.ThrowPendingException();
    }

    static int getIntOption(cli.System.Net.Sockets.Socket fd, int cmd) throws SocketException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        int ret = DualStackPlainSocketImpl_c.getIntOption(env, fd, cmd);
        env.ThrowPendingException();
        return ret;
    }

    static void sendOOB(cli.System.Net.Sockets.Socket fd, int data) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        DualStackPlainSocketImpl_c.sendOOB(env, fd, data);
        env.ThrowPendingException();
    }

    static void configureBlocking(cli.System.Net.Sockets.Socket fd, boolean blocking) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        DualStackPlainSocketImpl_c.configureBlocking(env, fd, blocking);
        env.ThrowPendingException();
    }
}
