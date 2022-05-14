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
import sun.net.ResourceManager;

/**
 * This class defines the plain DatagramSocketImpl that is used for all
 * Windows versions lower than Vista. It adds support for IPv6 on
 * these platforms where available.
 *
 * For backward compatibility windows platforms that do not have IPv6
 * support also use this implementation, and fd1 gets set to null
 * during socket creation.
 *
 * @author Chris Hegarty
 */

class TwoStacksPlainDatagramSocketImpl extends AbstractPlainDatagramSocketImpl
{
    /* Used for IPv6 on Windows only */
    FileDescriptor fd1;

    /*
     * Needed for ipv6 on windows because we need to know
     * if the socket was bound to ::0 or 0.0.0.0, when a caller
     * asks for it. In this case, both sockets are used, but we
     * don't know whether the caller requested ::0 or 0.0.0.0
     * and need to remember it here.
     */
    private InetAddress anyLocalBoundAddr=null;

    cli.System.Net.Sockets.Socket fduse=null; /* saved between peek() and receive() calls */

    /* saved between successive calls to receive, if data is detected
     * on both sockets at same time. To ensure that one socket is not
     * starved, they rotate using this field
     */
    cli.System.Net.Sockets.Socket lastfd=null;

    // true if this socket is exclusively bound
    private final boolean exclusiveBind;

    /*
     * Set to true if SO_REUSEADDR is set after the socket is bound to
     * indicate SO_REUSEADDR is being emulated
     */
    private boolean reuseAddressEmulated;

    // emulates SO_REUSEADDR when exclusiveBind is true and socket is bound
    private boolean isReuseAddress;

    TwoStacksPlainDatagramSocketImpl(boolean exclBind) {
        exclusiveBind = exclBind;
    }

    protected synchronized void create() throws SocketException {
        fd1 = new FileDescriptor();
        try {
            super.create();
        } catch (SocketException e) {
            fd1 = null;
            throw e;
        }
    }

    protected synchronized void bind(int lport, InetAddress laddr)
        throws SocketException {
        super.bind(lport, laddr);
        if (laddr.isAnyLocalAddress()) {
            anyLocalBoundAddr = laddr;
        }
    }

    @Override
    protected synchronized void bind0(int lport, InetAddress laddr)
        throws SocketException
    {
        bind0(lport, laddr, exclusiveBind);

    }

    protected synchronized void receive(DatagramPacket p)
        throws IOException {
        try {
            receive0(p);
        } finally {
            fduse = null;
        }
    }

    public Object getOption(int optID) throws SocketException {
        if (isClosed()) {
            throw new SocketException("Socket Closed");
        }

        if (optID == SO_BINDADDR) {
            if ((fd != null && fd1 != null) && !connected) {
                return anyLocalBoundAddr;
            }
            int family = connectedAddress == null ? -1 : connectedAddress.holder().getFamily();
            return socketLocalAddress(family);
        } else if (optID == SO_REUSEADDR && reuseAddressEmulated) {
            return isReuseAddress;
        } else {
            return super.getOption(optID);
        }
    }

    protected void socketSetOption(int opt, Object val)
        throws SocketException
    {
        if (opt == SO_REUSEADDR && exclusiveBind && localPort != 0)  {
            // socket already bound, emulate
            reuseAddressEmulated = true;
            isReuseAddress = (Boolean)val;
        } else {
            socketNativeSetOption(opt, val);
        }

    }

    protected boolean isClosed() {
        return (fd == null && fd1 == null) ? true : false;
    }

    protected void close() {
        if (fd != null || fd1 != null) {
            datagramSocketClose();
            ResourceManager.afterUdpClose();
            fd = null;
            fd1 = null;
        }
    }

    /* Native methods */

    protected synchronized void bind0(int lport, InetAddress laddr,
                                             boolean exclBind) throws SocketException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        TwoStacksPlainDatagramSocketImpl_c.bind0(env, this, lport, laddr, exclBind);
        env.ThrowPendingException();
    }

    protected void send(DatagramPacket packet) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        TwoStacksPlainDatagramSocketImpl_c.send(env, this, packet);
        env.ThrowPendingException();
    }

    protected synchronized int peek(InetAddress addressObj) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        int ret = TwoStacksPlainDatagramSocketImpl_c.peek(env, this, addressObj);
        env.ThrowPendingException();
        return ret;
    }

    protected synchronized int peekData(DatagramPacket p) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        int ret = TwoStacksPlainDatagramSocketImpl_c.peekData(env, this, p);
        env.ThrowPendingException();
        return ret;
    }

    protected synchronized void receive0(DatagramPacket packet) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        TwoStacksPlainDatagramSocketImpl_c.receive0(env, this, packet);
        env.ThrowPendingException();
    }

    protected void setTimeToLive(int ttl) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        TwoStacksPlainDatagramSocketImpl_c.setTimeToLive(env, this, ttl);
        env.ThrowPendingException();
    }

    protected int getTimeToLive() throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        int ret = TwoStacksPlainDatagramSocketImpl_c.getTimeToLive(env, this);
        env.ThrowPendingException();
        return ret;
    }

    protected void setTTL(byte ttl) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        TwoStacksPlainDatagramSocketImpl_c.setTTL(env, this, ttl);
        env.ThrowPendingException();
    }

    protected byte getTTL() throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        byte ret = TwoStacksPlainDatagramSocketImpl_c.getTTL(env, this);
        env.ThrowPendingException();
        return ret;
    }

    protected void join(InetAddress inetaddr, NetworkInterface netIf) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        TwoStacksPlainDatagramSocketImpl_c.join(env, this, inetaddr, netIf);
        env.ThrowPendingException();
    }

    protected void leave(InetAddress inetaddr, NetworkInterface netIf) throws IOException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        TwoStacksPlainDatagramSocketImpl_c.leave(env, this, inetaddr, netIf);
        env.ThrowPendingException();
    }

    protected void datagramSocketCreate() throws SocketException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        TwoStacksPlainDatagramSocketImpl_c.datagramSocketCreate(env, this);
        env.ThrowPendingException();
    }

    protected void datagramSocketClose() {
        TwoStacksPlainDatagramSocketImpl_c.datagramSocketClose(this);
    }

    protected void socketNativeSetOption(int opt, Object val) throws SocketException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        TwoStacksPlainDatagramSocketImpl_c.socketNativeSetOption(env, this, opt, val);
        env.ThrowPendingException();
    }

    protected Object socketGetOption(int opt) throws SocketException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        Object ret = TwoStacksPlainDatagramSocketImpl_c.socketGetOption(env, this, opt);
        env.ThrowPendingException();
        return ret;
    }

    protected void connect0(InetAddress address, int port) throws SocketException {
        if (ikvm.internal.Util.MONO) {
            // MONOBUG Mono doesn't allow Socket.Connect(IPAddress.Any, 0) to disconnect a datagram socket,
            // so we throw a SocketException, this will cause DatagramSocket to emulate connectedness
            throw new SocketException("connected datagram sockets not supported on Mono");
        }
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        TwoStacksPlainDatagramSocketImpl_c.connect0(env, this, address, port);
        env.ThrowPendingException();
    }

    protected Object socketLocalAddress(int family) throws SocketException {
        ikvm.internal.JNI.JNIEnv env = new ikvm.internal.JNI.JNIEnv();
        Object ret = TwoStacksPlainDatagramSocketImpl_c.socketLocalAddress(env, this, family);
        env.ThrowPendingException();
        return ret;
    }

    protected void disconnect0(int family) {
        TwoStacksPlainDatagramSocketImpl_c.disconnect0(this, family);
    }
}
