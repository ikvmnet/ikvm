/*
  Copyright (C) 2002, 2003, 2004, 2005, 2006 Jeroen Frijters

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
package gnu.java.nio;

import gnu.java.net.PlainDatagramSocketImpl;
import ikvm.internal.Util;
import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetSocketAddress;
import java.net.SocketAddress;
import java.net.SocketTimeoutException;
import java.nio.ByteBuffer;
import java.nio.channels.ClosedChannelException;
import java.nio.channels.DatagramChannel;
import java.nio.channels.IllegalBlockingModeException;
import java.nio.channels.NotYetConnectedException;
import java.nio.channels.spi.SelectorProvider;
import java.nio.channels.UnsupportedAddressTypeException;

public final class DatagramChannelImpl extends DatagramChannel
{
    private PlainDatagramSocketImpl impl;
    private DatagramSocket socket;
    private boolean blocking;
    private InetSocketAddress connectedTo;

    protected DatagramChannelImpl(SelectorProvider provider) throws IOException
    {
        super(provider);
        impl = new PlainDatagramSocketImpl();
        socket = new DatagramSocket(impl)
        {
            public void send(DatagramPacket p) throws IOException
            {
                if(blocking)
                    throw new IllegalBlockingModeException();

                super.send(p);
            }

            public void receive(DatagramPacket p) throws IOException
            {
                if(blocking)
                    throw new IllegalBlockingModeException();

                super.receive(p);
            }

            public DatagramChannel getChannel()
            {
                return DatagramChannelImpl.this;
            }
        };
    }

    public boolean isInChannelOperation()
    {
        return false;
    }

    public DatagramSocket socket()
    {
        return socket;
    }
    
    protected void implCloseSelectableChannel() throws IOException
    {
        socket.close();
    }

    protected void implConfigureBlocking(boolean blocking) throws IOException
    {
        this.blocking = blocking;
    }

    public DatagramChannel connect(SocketAddress remote) throws IOException
    {
        if (!isOpen())
            throw new ClosedChannelException();

        if (!(remote instanceof InetSocketAddress))
            throw new UnsupportedAddressTypeException();

        // HACK the .NET Socket class has no way to disconnect, so we simulate connected sockets
        connectedTo = (InetSocketAddress)remote;
        return this;
    }
    
    public DatagramChannel disconnect() throws IOException
    {
        connectedTo = null;
        return this;
    }
    
    public boolean isConnected()
    {
        return connectedTo != null;
    }
    
    public int write(ByteBuffer src) throws IOException
    {
        if (!isOpen())
            throw new ClosedChannelException();

        if (!isConnected ())
            throw new NotYetConnectedException ();
    
        return sendImpl(src, connectedTo);
    }

    public long write(ByteBuffer[] srcs, int offset, int length) throws IOException
    {
        if (!isOpen())
            throw new ClosedChannelException();

        if (!isConnected())
            throw new NotYetConnectedException();

        if (!Util.rangeCheck(srcs.length, offset, length))
            throw new IndexOutOfBoundsException();

        throw new Error("Not implemented");
    }

    public int read(ByteBuffer dst) throws IOException
    {
        if (!isOpen())
            throw new ClosedChannelException();

        if (!isConnected ())
            throw new NotYetConnectedException ();
    
        int remaining = dst.remaining();
        receive(dst);
        return remaining - dst.remaining();
    }
    
    public long read(ByteBuffer[] dsts, int offset, int length) throws IOException
    {
        if (!isOpen())
            throw new ClosedChannelException();

        if (!isConnected())
            throw new NotYetConnectedException();

        if (!Util.rangeCheck(dsts.length, offset, length))
            throw new IndexOutOfBoundsException();

        throw new Error("Not implemented");
    }
    
    public SocketAddress receive(ByteBuffer dst) throws IOException
    {
        if (!isOpen())
            throw new ClosedChannelException();

        try
        {
            begin();
            byte[] buf = new byte[dst.remaining()];
            DatagramPacket packet;
            do
            {
                packet = new DatagramPacket(buf, buf.length);
                impl.receive(packet);
            }
            while (connectedTo != null && !connectedTo.equals(packet.getSocketAddress()));
            dst.put(buf, 0, packet.getLength());
            return packet.getSocketAddress();
        }
        finally
        {
            end(true);
        }
    }

    public int send(ByteBuffer src, SocketAddress target) throws IOException
    {
        if (!isOpen())
            throw new ClosedChannelException();

        if (!(target instanceof InetSocketAddress))
            throw new IOException("can only send to inet socket addresses");

        InetSocketAddress dst = (InetSocketAddress) target;
        if (dst.isUnresolved())
            throw new IOException("Target address not resolved");

        if (connectedTo != null && !dst.equals(connectedTo))
            throw new IllegalArgumentException("Connected address not equal to target address");

        return sendImpl(src, dst);
    }

    private int sendImpl(ByteBuffer src, InetSocketAddress dst) throws IOException
    {
        // TODO add non-blocking support
        if (src.hasArray())
        {
            int offset = src.arrayOffset() + src.position();
            int length = src.remaining();
            impl.send(new DatagramPacket(src.array(), offset, length, dst.getAddress(), dst.getPort()));
            src.position(src.position() + length);
            return length;
        }
        else
        {
            byte[] buf = new byte[src.remaining()];
            src.get(buf);
            impl.send(new DatagramPacket(buf, buf.length, dst.getAddress(), dst.getPort()));
            return buf.length;
        }
    }

    cli.System.Net.Sockets.Socket getSocket()
    {
        return impl.getSocket();
    }
}
