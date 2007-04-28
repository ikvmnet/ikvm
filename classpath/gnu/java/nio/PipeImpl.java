/*
  Copyright (C) 2006, 2007 Jeroen Frijters

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

import ikvm.internal.Util;
import java.io.IOException;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.ServerSocket;
import java.nio.ByteBuffer;
import java.nio.channels.Pipe;
import java.nio.channels.ServerSocketChannel;
import java.nio.channels.SocketChannel;
import java.nio.channels.spi.SelectorProvider;

final class PipeImpl extends Pipe
{
	private static ServerSocketChannel server;
    private final SourceChannelImpl source;
	private final SocketChannel sourceSocket;
    private final SinkChannelImpl sink;
	private final SocketChannel sinkSocket;

	static
	{
		try
		{
			server = ServerSocketChannel.open();
			server.socket().bind(new InetSocketAddress(InetAddress.getAllByName(null)[0], 0), 1);
		}
		catch(IOException _)
		{
		}
	}

	final class SourceChannelImpl extends Pipe.SourceChannel
	{
		SourceChannelImpl(SelectorProvider provider)
		{
			super(provider);
		}

		cli.System.Net.Sockets.Socket getSocket()
		{
			return ((SocketChannelImpl)sourceSocket).getSocket();
		}

		protected void implCloseSelectableChannel() throws IOException
		{
			sourceSocket.close();
		}

		protected void implConfigureBlocking(boolean blocking) throws IOException
		{
			sourceSocket.configureBlocking(blocking);
		}

		public int read(ByteBuffer src) throws IOException
		{
			return sourceSocket.read(src);
		}

		public long read(ByteBuffer[] srcs) throws IOException
		{
			return sourceSocket.read(srcs);
		}

		public long read(ByteBuffer[] srcs, int offset, int length) throws IOException
		{
			return sourceSocket.read(srcs, offset, length);
		}
	}

	final class SinkChannelImpl extends Pipe.SinkChannel
	{
		SinkChannelImpl(SelectorProvider provider)
		{
			super(provider);
		}

		cli.System.Net.Sockets.Socket getSocket()
		{
			return ((SocketChannelImpl)sinkSocket).getSocket();
		}

		protected void implCloseSelectableChannel() throws IOException
		{
			sinkSocket.close();
		}

		protected void implConfigureBlocking(boolean blocking) throws IOException
		{
			sinkSocket.configureBlocking(blocking);
		}

		public int write(ByteBuffer dst) throws IOException
		{
			return sinkSocket.write(dst);
		}

		public long write(ByteBuffer[] dsts) throws IOException
		{
			return sinkSocket.write(dsts);
		}

		public long write(ByteBuffer[] dsts, int offset, int length) throws IOException
		{
			return sinkSocket.write(dsts, offset, length);
		}
	}
  
    PipeImpl(SelectorProvider provider) throws IOException
    {
		sinkSocket = SocketChannel.open();
		sinkSocket.socket().bind(null);
		sinkSocket.configureBlocking(false);
		sinkSocket.connect(server.socket().getLocalSocketAddress());
		SocketChannel ch;
		for(;;)
		{
			ch = server.accept();
			// we have a connection, now make sure that accept returned the socket associated with our connection
			int sourcePort = ((InetSocketAddress)ch.socket().getRemoteSocketAddress()).getPort();
			int sinkPort = sinkSocket.socket().getLocalPort();
			if(sourcePort == sinkPort)
			{
				sinkSocket.configureBlocking(true);
				if(!sinkSocket.finishConnect())
				{
					throw new IOException("Unexpected: finishConnect returned false");
				}
				break;
			}
			try
			{
				ch.close();
			}
			catch(IOException _)
			{
			}
		}
		sourceSocket = ch;
		source = new SourceChannelImpl(provider);
		sink = new SinkChannelImpl(provider);
	}

    public Pipe.SinkChannel sink()
    {
        return sink;
    }

    public Pipe.SourceChannel source()
    {
        return source;
    }
}
