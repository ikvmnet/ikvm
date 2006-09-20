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

import gnu.java.net.PlainSocketImpl;
import ikvm.internal.Util;
import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.net.SocketAddress;
import java.nio.ByteBuffer;
import java.nio.channels.AlreadyConnectedException;
import java.nio.channels.ClosedChannelException;
import java.nio.channels.ConnectionPendingException;
import java.nio.channels.IllegalBlockingModeException;
import java.nio.channels.NoConnectionPendingException;
import java.nio.channels.NotYetConnectedException;
import java.nio.channels.SocketChannel;
import java.nio.channels.UnresolvedAddressException;
import java.nio.channels.UnsupportedAddressTypeException;
import java.nio.channels.spi.SelectorProvider;

public final class SocketChannelImpl extends SocketChannel
{
    private PlainSocketImpl impl;
    private Socket socket;
    private boolean connected;
    private boolean connectionPending;
    private boolean blocking;
  
    SocketChannelImpl(SelectorProvider provider) throws IOException
    {
        this(provider, new PlainSocketImpl(), false);
        impl.create(true);
    }

    SocketChannelImpl(SelectorProvider provider, PlainSocketImpl impl, boolean connected) throws IOException
    {
        super(provider);
        this.impl = impl;
        this.connected = connected;
        this.blocking = true;
        socket = new Socket(impl)
        {
            public void connect(SocketAddress endpoint) throws IOException
            {
                if (blocking)
                    throw new IllegalBlockingModeException();

                super.connect(endpoint);
            }

            public void connect(SocketAddress endpoint, int timeout) throws IOException
            {
                if (blocking)
                    throw new IllegalBlockingModeException();

                super.connect(endpoint, timeout);
            }

            public SocketChannel getChannel() 
            {
                return SocketChannelImpl.this;
            }
        };
    }
  
    protected void implCloseSelectableChannel() throws IOException
    {
        connectionPending = false;
        connected = false;
        socket.close();
    }

    protected void implConfigureBlocking(boolean blocking) throws IOException
    {
        this.blocking = blocking;
        if (connected)
        {
            impl.setBlocking(blocking);
        }
    }

    public boolean connect(SocketAddress remote) throws IOException
    {
        if (!isOpen())
            throw new ClosedChannelException();
    
        if (isConnected())
            throw new AlreadyConnectedException();

        if (connectionPending)
            throw new ConnectionPendingException();

        if (!(remote instanceof InetSocketAddress))
            throw new UnsupportedAddressTypeException();
    
        InetSocketAddress connectAddress = (InetSocketAddress) remote;

        if (connectAddress.isUnresolved())
            throw new UnresolvedAddressException();

        if (blocking)
        {
            try
            {
                impl.connect(connectAddress.getAddress(), connectAddress.getPort());
                impl.setBlocking(blocking);
                connected = true;
            }
            finally
            {
                if (!connected)
                {
                    close();
                }
            }
            return true;
        }
        else
        {
            impl.beginConnect(connectAddress);
            connectionPending = true;
            return false;
        }
    }
    
    public boolean finishConnect() throws IOException
    {
        if (!isOpen())
            throw new ClosedChannelException();

        if (connected)
            return true;
    
        if (!connectionPending)
            throw new NoConnectionPendingException();

        if (blocking || impl.isConnectFinished())
        {
            try
            {
                connectionPending = false;
                impl.endConnect();
                connected = true;
                impl.setBlocking(blocking);
            }
            finally
            {
                if (!connected)
                {
                    close();
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public boolean isConnected()
    {
        return connected;
    }
    
    public boolean isConnectionPending()
    {
        return connectionPending;
    }
    
    public Socket socket()
    {
        return socket;
    }

    public int read(ByteBuffer dst) throws IOException
    {
        if (!isOpen())
            throw new ClosedChannelException();

        if (!isConnected())
            throw new NotYetConnectedException();

        if (dst.hasArray())
        {
            byte[] buf = dst.array();
            int len = impl.read(buf, dst.arrayOffset() + dst.position(), dst.remaining());
            dst.position(dst.position() + len);
            return len;
        }
        else
        {
            byte[] buf = new byte[dst.remaining()];
            int len = impl.read(buf, 0, buf.length);
            dst.put(buf, 0, len);
            return len;
        }
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

    public int write(ByteBuffer src) throws IOException
    {
        if (!isOpen())
            throw new ClosedChannelException();

        if (!isConnected())
            throw new NotYetConnectedException();

        if (src.hasArray())
        {
            byte[] buf = src.array();
            int len = impl.writeImpl(buf, src.arrayOffset() + src.position(), src.remaining());
            src.position(src.position() + len);
            return len;
        }
        else
        {
            int pos = src.position();
            byte[] buf = new byte[src.remaining()];
            src.get(buf);
            int len = impl.writeImpl(buf, 0, buf.length);
            src.position(pos + len);
            return len;
        }
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

    cli.System.Net.Sockets.Socket getSocket()
    {
        return impl.getSocket();
    }
}
