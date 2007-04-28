/*
  Copyright (C) 2002, 2003, 2004, 2005, 2006, 2007 Jeroen Frijters

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
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.NioServerSocket;
import java.net.Socket;
import java.net.SocketAddress;
import java.net.SocketException;
import java.nio.ByteBuffer;
import java.nio.channels.AlreadyConnectedException;
import java.nio.channels.AsynchronousCloseException;
import java.nio.channels.ClosedByInterruptException;
import java.nio.channels.ClosedChannelException;
import java.nio.channels.ConnectionPendingException;
import java.nio.channels.IllegalBlockingModeException;
import java.nio.channels.NoConnectionPendingException;
import java.nio.channels.NotYetConnectedException;
import java.nio.channels.SocketChannel;
import java.nio.channels.UnresolvedAddressException;
import java.nio.channels.UnsupportedAddressTypeException;
import java.nio.channels.spi.SelectorProvider;

public final class SocketChannelImpl extends SocketChannel implements VMThread.InterruptProc
{
    private static final int NOT_CONNECTED = 0;
    private static final int CONNECTION_PENDING = 1;
    private static final int CONNECTED = 2;

    private final PlainSocketImpl impl;
    private final Socket socket;
    private final Object lockRead = new Object();
    private final Object lockWrite = new Object();
    private volatile int state; // state is only valid while isOpen()
    private volatile boolean connectionPendingReady;

    SocketChannelImpl(SelectorProvider provider) throws IOException
    {
        this(provider, new PlainSocketImpl(), false);
        impl.create(true);
    }

    SocketChannelImpl(SelectorProvider provider, PlainSocketImpl impl, boolean connected) throws IOException
    {
        super(provider);
        this.impl = impl;
        socket = new Socket(impl)
        {
            public void connect(SocketAddress endpoint) throws IOException
            {
                if (isBlocking())
                    throw new IllegalBlockingModeException();

                super.connect(endpoint);
            }

            public void connect(SocketAddress endpoint, int timeout) throws IOException
            {
                if (isBlocking())
                    throw new IllegalBlockingModeException();

                super.connect(endpoint, timeout);
            }

            public SocketChannel getChannel() 
            {
                return SocketChannelImpl.this;
            }
        };
		if (connected)
		{
			state = CONNECTED;
			NioServerSocket.setFlags(socket);
		}
	}
  
    protected void implCloseSelectableChannel() throws IOException
    {
        impl.close();
    }

    protected void implConfigureBlocking(boolean blocking) throws IOException
    {
        if (state == CONNECTED)
        {
            impl.setBlocking(blocking);
        }
    }

    public boolean connect(SocketAddress remote) throws IOException
    {
        synchronized (lockRead)
        {
            synchronized (lockWrite)
            {
                if (!isOpen())
                    throw new ClosedChannelException();
            
                if (isConnected())
                    throw new AlreadyConnectedException();

                if (isConnectionPending())
                    throw new ConnectionPendingException();

                if (!(remote instanceof InetSocketAddress))
                    throw new UnsupportedAddressTypeException();
            
                InetSocketAddress connectAddress = (InetSocketAddress) remote;

                if (connectAddress.isUnresolved())
                    throw new UnresolvedAddressException();

                if (isBlocking())
                {
                    try
                    {
                        implConnect(connectAddress.getAddress(), connectAddress.getPort());
                        state = CONNECTED;
                        synchronized (blockingLock())
                        {
                            impl.setBlocking(isBlocking());
                        }
                    }
                    finally
                    {
                        if (state != CONNECTED)
                        {
                            close();
                        }
                    }
                    return true;
                }
                else
                {
                    implBeginConnect(connectAddress);
                    state = CONNECTION_PENDING;
                    return false;
                }
            }
        }
    }

    private void implConnect(InetAddress address, int port) throws IOException
    {
        try
        {
            VMThread.enterInterruptableWait(this);
            try
            {
                impl.connect(address, port);
            }
            finally
            {
                VMThread.leaveInterruptableWait();
            }
        }
        catch (SocketException x)
        {
            if (!isOpen())
                throw new AsynchronousCloseException();
            throw x;
        }
        catch (InterruptedException _)
        {
            close();
            Thread.currentThread().interrupt();
            throw new ClosedByInterruptException();
        }
    }

    private void implBeginConnect(InetSocketAddress address) throws IOException
    {
        try
        {
            VMThread.enterInterruptableWait(this);
            try
            {
                impl.beginConnect(address);
            }
            finally
            {
                VMThread.leaveInterruptableWait();
            }
        }
        catch (SocketException x)
        {
            if (!isOpen())
                throw new AsynchronousCloseException();
            throw x;
        }
        catch (InterruptedException _)
        {
            close();
            Thread.currentThread().interrupt();
            throw new ClosedByInterruptException();
        }
    }

    void selected()
    {
        if (state == CONNECTION_PENDING)
        {
            // if select() returned for this socket, we must make sure that
            // a following finishConnect actually finishes the connect, so we
            // set this flag (because oddly enough the .NET async object may still be
            // unsignaled at this point, so impl.isConnectFinished may return false)
            connectionPendingReady = true;
        }
    }

    public boolean finishConnect() throws IOException
    {
        synchronized (lockRead)
        {
            synchronized (lockWrite)
            {
                if (!isOpen())
                    throw new ClosedChannelException();

                if (state == CONNECTED)
                    return true;
            
                if (state != CONNECTION_PENDING)
                    throw new NoConnectionPendingException();

                if (isBlocking() || connectionPendingReady || impl.isConnectFinished())
                {
                    try
                    {
                        implEndConnect();
                        state = CONNECTED;
                        synchronized (blockingLock())
                        {
                            impl.setBlocking(isBlocking());
                        }
                    }
                    finally
                    {
                        if (state != CONNECTED)
                        {
                            close();
                        }
                    }
                    return true;
                }
                else
                {
                    if (Thread.interrupted())
                    {
                        close();
                        Thread.currentThread().interrupt();
                        throw new ClosedByInterruptException();
                    }
                    return false;
                }
            }
        }
    }

    private void implEndConnect() throws IOException
    {
        try
        {
            VMThread.enterInterruptableWait(this);
            try
            {
                impl.endConnect();
            }
            finally
            {
                VMThread.leaveInterruptableWait();
            }
        }
        catch (SocketException x)
        {
            if (!isOpen())
                throw new AsynchronousCloseException();
            throw x;
        }
        catch (InterruptedException _)
        {
            close();
            Thread.currentThread().interrupt();
            throw new ClosedByInterruptException();
        }
    }

    public boolean isConnected()
    {
        return isOpen() && state == CONNECTED;
    }
    
    public boolean isConnectionPending()
    {
        return isOpen() && state == CONNECTION_PENDING;
    }
    
    public Socket socket()
    {
        return socket;
    }

    public void interrupt()
    {
        try
        {
            impl.close();
        }
        catch (IOException _)
        {
        }
    }

    public int read(ByteBuffer dst) throws IOException
    {
        synchronized (lockRead)
        {
            if (!isOpen())
                throw new ClosedChannelException();

            if (state != CONNECTED)
                throw new NotYetConnectedException();

            if (dst.hasArray())
            {
                byte[] buf = dst.array();
                int len = implRead(buf, dst.arrayOffset() + dst.position(), dst.remaining());
				if (len > 0)
				{
					dst.position(dst.position() + len);
				}
                return len;
            }
            else
            {
                byte[] buf = new byte[dst.remaining()];
                int len = implRead(buf, 0, buf.length);
				dst.put(buf, 0, len);
                return len;
            }
        }
    }

    public long read(ByteBuffer[] dsts, int offset, int length) throws IOException
    {
        synchronized (lockRead)
        {
            if (!isOpen())
                throw new ClosedChannelException();

            if (state != CONNECTED)
                throw new NotYetConnectedException();
        
            if (!Util.rangeCheck(dsts.length, offset, length))
                throw new IndexOutOfBoundsException();
        
            long totalRead = 0;
            for (int i = 0; i < length; i++)
            {
                int size = dsts[i + offset].remaining();
                if (size > 0)
                {
                    int read = read(dsts[i + offset]);
					if (read < 0)
					{
						break;
					}
                    totalRead += read;
                    if (read < size || available() == 0)
                    {
                        break;
                    }
                }
            }
            return totalRead;
        }
    }

    private int available()
    {
        try
        {
            return impl.available();
        }
        catch (IOException _)
        {
            return 0;
        }
    }

    private int implRead(byte[] buf, int offset, int length) throws IOException
    {
        try
        {
            VMThread.enterInterruptableWait(this);
            try
            {
                return impl.read(buf, offset, length);
            }
            finally
            {
                VMThread.leaveInterruptableWait();
            }
        }
        catch (SocketException x)
        {
            if (!isOpen())
                throw new AsynchronousCloseException();
            throw x;
        }
        catch (InterruptedException _)
        {
            close();
            Thread.currentThread().interrupt();
            throw new ClosedByInterruptException();
        }
    }

    private int implWrite(byte[] buf, int offset, int length) throws IOException
    {
        try
        {
            VMThread.enterInterruptableWait(this);
            try
            {
                return impl.writeImpl(buf, offset, length);
            }
            finally
            {
                VMThread.leaveInterruptableWait();
            }
        }
        catch (SocketException x)
        {
            if (!isOpen())
                throw new AsynchronousCloseException();
            throw x;
        }
        catch (InterruptedException _)
        {
            close();
            Thread.currentThread().interrupt();
            throw new ClosedByInterruptException();
        }
    }

    public int write(ByteBuffer src) throws IOException
    {
        synchronized (lockWrite)
        {
            if (!isOpen())
                throw new ClosedChannelException();

            if (state != CONNECTED)
                throw new NotYetConnectedException();

            if (src.hasArray())
            {
                byte[] buf = src.array();
                int len = implWrite(buf, src.arrayOffset() + src.position(), src.remaining());
                src.position(src.position() + len);
                return len;
            }
            else
            {
                int pos = src.position();
                byte[] buf = new byte[src.remaining()];
                src.get(buf);
                int len = implWrite(buf, 0, buf.length);
                src.position(pos + len);
                return len;
            }
        }
    }

    public long write(ByteBuffer[] srcs, int offset, int length) throws IOException
    {
        synchronized (lockWrite)
        {
            if (!isOpen())
                throw new ClosedChannelException();

            if (state != CONNECTED)
                throw new NotYetConnectedException();
        
            if (!Util.rangeCheck(srcs.length, offset, length))
                throw new IndexOutOfBoundsException();

            long totalWritten = 0;
            for (int i = 0; i < length; i++)
            {
                int size = srcs[i + offset].remaining();
                if (size > 0)
                {
                    int written = write(srcs[i + offset]);
                    totalWritten += written;
                    if (written < size)
                    {
                        break;
                    }
                }
            }
            return totalWritten;
        }
    }

    cli.System.Net.Sockets.Socket getSocket()
    {
        return impl.getSocket();
    }
}
