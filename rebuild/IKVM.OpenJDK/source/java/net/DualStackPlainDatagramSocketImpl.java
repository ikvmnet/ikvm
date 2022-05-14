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

/**
 * This class defines the plain DatagramSocketImpl that is used on
 * Windows platforms greater than or equal to Windows Vista. These
 * platforms have a dual layer TCP/IP stack and can handle both IPv4
 * and IPV6 through a single file descriptor.
 * <p>
 * Note: Multicasting on a dual layer TCP/IP stack is always done with
 * TwoStacksPlainDatagramSocketImpl. This is to overcome the lack
 * of behavior defined for multicasting over a dual layer socket by the RFC.
 *
 * @author Chris Hegarty
 */

class DualStackPlainDatagramSocketImpl extends AbstractPlainDatagramSocketImpl
{

    // true if this socket is exclusively bound
    private final boolean exclusiveBind;

    /*
     * Set to true if SO_REUSEADDR is set after the socket is bound to
     * indicate SO_REUSEADDR is being emulated
     */
    private boolean reuseAddressEmulated;

    // emulates SO_REUSEADDR when exclusiveBind is true and socket is bound
    private boolean isReuseAddress;

    DualStackPlainDatagramSocketImpl(boolean exclBind) {
        exclusiveBind = exclBind;
    }

    protected void datagramSocketCreate() throws SocketException {
        if (fd == null)
            throw new SocketException("Socket closed");

        cli.System.Net.Sockets.Socket newfd = socketCreate(false /* v6Only */);

        fd.setSocket(newfd);
    }

    protected synchronized void bind0(int lport, InetAddress laddr)
        throws SocketException {
        cli.System.Net.Sockets.Socket nativefd = checkAndReturnNativeFD();

        if (laddr == null)
            throw new NullPointerException("argument address");

        socketBind(nativefd, laddr, lport, exclusiveBind);
        if (lport == 0) {
            localPort = socketLocalPort(nativefd);
        } else {
            localPort = lport;
        }
    }

    protected synchronized int peek(InetAddress address) throws IOException {
        cli.System.Net.Sockets.Socket nativefd = checkAndReturnNativeFD();

        if (address == null)
            throw new NullPointerException("Null address in peek()");

        // Use peekData()
        DatagramPacket peekPacket = new DatagramPacket(new byte[1], 1);
        int peekPort = peekData(peekPacket);
        address = peekPacket.getAddress();
        return peekPort;
    }

    protected synchronized int peekData(DatagramPacket p) throws IOException {
        cli.System.Net.Sockets.Socket nativefd = checkAndReturnNativeFD();

        if (p == null)
            throw new NullPointerException("packet");
        if (p.getData() == null)
            throw new NullPointerException("packet buffer");

        return socketReceiveOrPeekData(nativefd, p, timeout, connected, true /*peek*/);
    }

    protected synchronized void receive0(DatagramPacket p) throws IOException {
        cli.System.Net.Sockets.Socket nativefd = checkAndReturnNativeFD();

        if (p == null)
            throw new NullPointerException("packet");
        if (p.getData() == null)
            throw new NullPointerException("packet buffer");

        socketReceiveOrPeekData(nativefd, p, timeout, connected, false /*receive*/);
    }

    protected void send(DatagramPacket p) throws IOException {
        cli.System.Net.Sockets.Socket nativefd = checkAndReturnNativeFD();

        if (p == null)
            throw new NullPointerException("null packet");

        if (p.getAddress() == null ||p.getData() ==null)
            throw new NullPointerException("null address || null buffer");

        socketSend(nativefd, p.getData(), p.getOffset(), p.getLength(),
                   p.getAddress(), p.getPort(), connected);
    }

    protected void connect0(InetAddress address, int port) throws SocketException {
        cli.System.Net.Sockets.Socket nativefd = checkAndReturnNativeFD();

        if (address == null)
            throw new NullPointerException("address");

        socketConnect(nativefd, address, port);
    }

    protected void disconnect0(int family /*unused*/) {
        if (fd == null || !fd.valid())
            return;   // disconnect doesn't throw any exceptions

        socketDisconnect(fd.getSocket());
    }

    protected void datagramSocketClose() {
        if (fd == null || !fd.valid())
            return;   // close doesn't throw any exceptions

        socketClose(fd.getSocket());
        fd.setSocket(null);
    }

    @SuppressWarnings("fallthrough")
    protected void socketSetOption(int opt, Object val) throws SocketException {
        cli.System.Net.Sockets.Socket nativefd = checkAndReturnNativeFD();

        int optionValue = 0;

        switch(opt) {
            case IP_TOS :
            case SO_RCVBUF :
            case SO_SNDBUF :
                optionValue = ((Integer)val).intValue();
                break;
            case SO_REUSEADDR :
                if (exclusiveBind && localPort != 0)  {
                    // socket already bound, emulate SO_REUSEADDR
                    reuseAddressEmulated = true;
                    isReuseAddress = (Boolean)val;
                    return;
                }
                //Intentional fallthrough
            case SO_BROADCAST :
                optionValue = ((Boolean)val).booleanValue() ? 1 : 0;
                break;
            default: /* shouldn't get here */
                throw new SocketException("Option not supported");
        }

        socketSetIntOption(nativefd, opt, optionValue);
    }

    protected Object socketGetOption(int opt) throws SocketException {
        cli.System.Net.Sockets.Socket nativefd = checkAndReturnNativeFD();

         // SO_BINDADDR is not a socket option.
        if (opt == SO_BINDADDR) {
            return socketLocalAddress(nativefd);
        }
        if (opt == SO_REUSEADDR && reuseAddressEmulated)
            return isReuseAddress;

        int value = socketGetIntOption(nativefd, opt);
        Object returnValue = null;

        switch (opt) {
            case SO_REUSEADDR :
            case SO_BROADCAST :
                returnValue =  (value == 0) ? Boolean.FALSE : Boolean.TRUE;
                break;
            case IP_TOS :
            case SO_RCVBUF :
            case SO_SNDBUF :
                returnValue = new Integer(value);
                break;
            default: /* shouldn't get here */
                throw new SocketException("Option not supported");
        }

        return returnValue;
    }

    /* Multicast specific methods.
     * Multicasting on a dual layer TCP/IP stack is always done with
     * TwoStacksPlainDatagramSocketImpl. This is to overcome the lack
     * of behavior defined for multicasting over a dual layer socket by the RFC.
     */
    protected void join(InetAddress inetaddr, NetworkInterface netIf)
        throws IOException {
        throw new IOException("Method not implemented!");
    }

    protected void leave(InetAddress inetaddr, NetworkInterface netIf)
        throws IOException {
        throw new IOException("Method not implemented!");
    }

    protected void setTimeToLive(int ttl) throws IOException {
        throw new IOException("Method not implemented!");
    }

    protected int getTimeToLive() throws IOException {
        throw new IOException("Method not implemented!");
    }

    @Deprecated
    protected void setTTL(byte ttl) throws IOException {
        throw new IOException("Method not implemented!");
    }

    @Deprecated
    protected byte getTTL() throws IOException {
        throw new IOException("Method not implemented!");
    }
    /* END Multicast specific methods */

    private cli.System.Net.Sockets.Socket checkAndReturnNativeFD() throws SocketException {
        if (fd == null || !fd.valid())
            throw new SocketException("Socket closed");

        return fd.getSocket();
    }

    /* Native methods */

    private static cli.System.Net.Sockets.Socket socketCreate(boolean v6Only) {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        cli.System.Net.Sockets.Socket ret = DualStackPlainDatagramSocketImpl_c.socketCreate(env, v6Only);
        env.ThrowPendingException();
        return ret;
    }

    private static void socketBind(cli.System.Net.Sockets.Socket fd, InetAddress localAddress,
            int localport, boolean exclBind) throws SocketException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        DualStackPlainDatagramSocketImpl_c.socketBind(env, fd, localAddress, localport, exclBind);
        env.ThrowPendingException();
    }

    private static void socketConnect(cli.System.Net.Sockets.Socket fd, InetAddress address, int port)
        throws SocketException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        DualStackPlainDatagramSocketImpl_c.socketConnect(env, fd, address, port);
        env.ThrowPendingException();
    }

    private static void socketDisconnect(cli.System.Net.Sockets.Socket fd) {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        DualStackPlainDatagramSocketImpl_c.socketDisconnect(env, fd);
        env.ThrowPendingException();
    }

    private static void socketClose(cli.System.Net.Sockets.Socket fd) {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        DualStackPlainDatagramSocketImpl_c.socketClose(env, fd);
        env.ThrowPendingException();
    }

    private static int socketLocalPort(cli.System.Net.Sockets.Socket fd) throws SocketException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        int ret = DualStackPlainDatagramSocketImpl_c.socketLocalPort(env, fd);
        env.ThrowPendingException();
        return ret;
    }

    private static Object socketLocalAddress(cli.System.Net.Sockets.Socket fd) throws SocketException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        Object ret = DualStackPlainDatagramSocketImpl_c.socketLocalAddress(env, fd);
        env.ThrowPendingException();
        return ret;
    }

    private static int socketReceiveOrPeekData(cli.System.Net.Sockets.Socket fd, DatagramPacket packet,
        int timeout, boolean connected, boolean peek) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        int ret = DualStackPlainDatagramSocketImpl_c.socketReceiveOrPeekData(env, fd, packet, timeout, connected, peek);
        env.ThrowPendingException();
        return ret;
    }

    private static void socketSend(cli.System.Net.Sockets.Socket fd, byte[] data, int offset, int length,
        InetAddress address, int port, boolean connected) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        DualStackPlainDatagramSocketImpl_c.socketSend(env, fd, data, offset, length, address, port, connected);
        env.ThrowPendingException();
    }

    private static void socketSetIntOption(cli.System.Net.Sockets.Socket fd, int cmd,
        int optionValue) throws SocketException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        DualStackPlainDatagramSocketImpl_c.socketSetIntOption(env, fd, cmd, optionValue);
        env.ThrowPendingException();
    }

    private static int socketGetIntOption(cli.System.Net.Sockets.Socket fd, int cmd) throws SocketException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        int ret = DualStackPlainDatagramSocketImpl_c.socketGetIntOption(env, fd, cmd);
        env.ThrowPendingException();
        return ret;
    }
}
