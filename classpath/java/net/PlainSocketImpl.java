/* PlainSocketImpl.java -- Default socket implementation
   Copyright (C) 1998, 1999 Free Software Foundation, Inc.

This file is part of GNU Classpath.

GNU Classpath is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2, or (at your option)
any later version.
 
GNU Classpath is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
General Public License for more details.

You should have received a copy of the GNU General Public License
along with GNU Classpath; see the file COPYING.  If not, write to the
Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA
02111-1307 USA.

Linking this library statically or dynamically with other modules is
making a combined work based on this library.  Thus, the terms and
conditions of the GNU General Public License cover the whole
combination.

As a special exception, the copyright holders of this library give you
permission to link this library with independent modules to produce an
executable, regardless of the license terms of these independent
modules, and to copy and distribute the resulting executable under
terms of your choice, provided that you also meet, for each linked
independent module, the terms and conditions of the license of that
module.  An independent module is a module which is not derived from
or based on this library.  If you modify this library, you may extend
this exception to your version of the library, but you are not
obligated to do so.  If you do not wish to do so, delete this
exception statement from your version. */


package java.net;

import java.io.InputStream;
import java.io.OutputStream;
import java.io.IOException;
import cli.System.Net.*;
import cli.System.Net.Sockets.*;
import ikvm.lang.CIL;
import ikvm.lang.ByteArrayHack;

/**
  * Unless the application installs its own SocketImplFactory, this is the
  * default socket implemetation that will be used.  It simply uses a
  * combination of Java and native routines to implement standard BSD
  * style sockets of family AF_INET and types SOCK_STREAM and SOCK_DGRAM
  *
  * @version 0.1
  *
  * @author Aaron M. Renn (arenn@urbanophile.com)
  */
class PlainSocketImpl extends SocketImpl
{
	/**
	 * This is the native file descriptor for this socket
	 */
	private cli.System.Net.Sockets.Socket socket;


	/**
	 * Default do nothing constructor
	 */
	public PlainSocketImpl()
	{
	}

	/**
	 * Accepts a new connection on this socket and returns in in the 
	 * passed in SocketImpl.
	 *
	 * @param impl The SocketImpl object to accept this connection.
	 */
	protected void accept(SocketImpl impl) throws IOException
	{
		try
		{
			if(false) throw new cli.System.Net.Sockets.SocketException();
			cli.System.Net.Sockets.Socket accept = socket.Accept();
			((PlainSocketImpl)impl).socket = accept;
			IPEndPoint remoteEndPoint = ((IPEndPoint)accept.get_RemoteEndPoint());
			long remoteIP = remoteEndPoint.get_Address().get_Address();
			String remote = (remoteIP & 0xff) + "." + ((remoteIP >> 8) & 0xff) + "." + ((remoteIP >> 16) & 0xff) + "." + ((remoteIP >> 24) & 0xff);
			impl.address = InetAddress.getByName(remote);
			impl.port = remoteEndPoint.get_Port();
			impl.localport = ((IPEndPoint)accept.get_LocalEndPoint()).get_Port();
		}
		catch(cli.System.Net.Sockets.SocketException x)
		{
			// TODO error handling
			throw new IOException(x.getMessage());
		}
	}

	/**
	 * Returns the number of bytes that the caller can read from this socket
	 * without blocking. 
	 *
	 * @return The number of readable bytes before blocking
	 *
	 * @exception IOException If an error occurs
	 */
	protected int available() throws IOException
	{
		try
		{
			if(false) throw new cli.System.Net.Sockets.SocketException();
			return socket.get_Available();
		}
		catch(cli.System.Net.Sockets.SocketException x)
		{
			// TODO error handling
			throw new IOException(x.getMessage());
		}
	}

	/**
	 * Binds to the specified port on the specified addr.  Note that this addr
	 * must represent a local IP address.  **** How bind to INADDR_ANY? ****
	 *
	 * @param addr The address to bind to
	 * @param port The port number to bind to
	 *
	 * @exception IOException If an error occurs
	 */
	protected void bind(InetAddress addr, int port) throws IOException
	{
		try
		{
			if(false) throw new cli.System.Net.Sockets.SocketException();
			socket.Bind(new IPEndPoint(getAddressFromInetAddress(addr), port));
			this.address = addr;
		}
		catch(cli.System.Net.Sockets.SocketException x)
		{
			// TODO error handling
			throw new IOException(x.getMessage());
		}
	}

	static long getAddressFromInetAddress(InetAddress addr)
	{
		byte[] b = addr.getAddress();
		return (((b[3] & 0xff) << 24) + ((b[2] & 0xff) << 16) + ((b[1] & 0xff) << 8) + (b[0] & 0xff)) & 0xffffffffL;
	}

	/**
	 * Closes the socket.  This will cause any InputStream or OutputStream
	 * objects for this Socket to be closed as well.
	 * <p>
	 * Note that if the SO_LINGER option is set on this socket, then the
	 * operation could block.
	 *
	 * @exception IOException If an error occurs
	 */
	protected void close() throws IOException
	{
		try
		{
			if(false) throw new cli.System.Net.Sockets.SocketException();
			socket.Close();
		}
		catch(cli.System.Net.Sockets.SocketException x)
		{
			// TODO error handling
			throw new IOException(x.getMessage());
		}
	}

	/**
	 * Connects to the remote address and port specified as arguments.
	 *
	 * @param addr The remote address to connect to
	 * @param port The remote port to connect to
	 *
	 * @exception IOException If an error occurs
	 */
	protected void connect(InetAddress addr, int port) throws IOException
	{
		try
		{
			if(false) throw new cli.System.Net.Sockets.SocketException();
			socket.Connect(new IPEndPoint(getAddressFromInetAddress(addr), port));
			this.address = addr;
			this.port = port;
			this.localport = ((IPEndPoint)socket.get_LocalEndPoint()).get_Port();
		}
		catch(cli.System.Net.Sockets.SocketException x)
		{
			// TODO error handling
			throw new IOException(x.getMessage());
		}
	}

	/**
	 * Connects to the remote hostname and port specified as arguments.
	 *
	 * @param hostname The remote hostname to connect to
	 * @param port The remote port to connect to
	 *
	 * @exception IOException If an error occurs
	 */
	protected void connect(String hostname, int port) throws IOException
	{
		connect(InetAddress.getByName(hostname), port);
	}

	/**
	 * Creates a new socket that is not bound to any local address/port and
	 * is not connected to any remote address/port.  This will be created as
	 * a stream socket if the stream parameter is true, or a datagram socket
	 * if the stream parameter is false.
	 *
	 * @param stream true for a stream socket, false for a datagram socket
	 */
	protected void create(boolean stream) throws IOException
	{
		// TODO error handling
		if(!stream)
		{
			// TODO
			System.out.println("NOTE: PlainSocketImpl.create(false) not implemented");
			throw new IOException("PlainSocketImpl.create(false) not implemented");
		}
		try
		{
			if(false) throw new cli.System.Net.Sockets.SocketException();
			socket = new cli.System.Net.Sockets.Socket(AddressFamily.wrap(AddressFamily.InterNetwork), SocketType.wrap(SocketType.Stream), ProtocolType.wrap(ProtocolType.Tcp));
		}
		catch(cli.System.Net.Sockets.SocketException x)
		{
			// TODO error handling
			throw new IOException(x.getMessage());
		}
	}

	/**
	 * Starts listening for connections on a socket. The queuelen parameter
	 * is how many pending connections will queue up waiting to be serviced
	 * before being accept'ed.  If the queue of pending requests exceeds this
	 * number, additional connections will be refused.
	 *
	 * @param queuelen The length of the pending connection queue
	 * 
	 * @exception IOException If an error occurs
	 */
	protected void listen(int queuelen) throws IOException
	{
		try
		{
			if(false) throw new cli.System.Net.Sockets.SocketException();
			socket.Listen(queuelen);
			localport = ((IPEndPoint)socket.get_LocalEndPoint()).get_Port();
		}
		catch(cli.System.Net.Sockets.SocketException x)
		{
			// TODO error handling
			throw new IOException(x.getMessage());
		}
	}

	/**
	 * Internal method used by SocketInputStream for reading data from
	 * the connection.  Reads up to len bytes of data into the buffer
	 * buf starting at offset bytes into the buffer.
	 *
	 * @return The actual number of bytes read or -1 if end of stream.
	 *
	 * @exception IOException If an error occurs
	 */
	protected int read(byte[] buf, int offset, int len) throws IOException
	{
		try
		{
			if(false) throw new cli.System.Net.Sockets.SocketException();
			return socket.Receive(ByteArrayHack.cast(buf), offset, len, SocketFlags.wrap(SocketFlags.None));
		}
		catch(cli.System.Net.Sockets.SocketException x)
		{
			// TODO error handling
			throw new IOException(x.getMessage());
		}
	}

	/**
	 * Internal method used by SocketOuputStream for writing data to
	 * the connection.  Writes up to len bytes of data from the buffer
	 * buf starting at offset bytes into the buffer.
	 *
	 * @exception IOException If an error occurs
	 */
	protected void write(byte[] buf, int offset, int len) throws IOException
	{
		try
		{
			if(false) throw new cli.System.Net.Sockets.SocketException();
			socket.Send(ByteArrayHack.cast(buf), offset, len, SocketFlags.wrap(SocketFlags.None));
		}
		catch(cli.System.Net.Sockets.SocketException x)
		{
			// TODO error handling
			throw new IOException(x.getMessage());
		}
	}

	/**
	 * Sets the specified option on a socket to the passed in object.  For
	 * options that take an integer argument, the passed in object is an
	 * Integer.  The option_id parameter is one of the defined constants in
	 * this interface.
	 *
	 * @param option_id The identifier of the option
	 * @param val The value to set the option to
	 *
	 * @exception SocketException If an error occurs
	 */
	public void setOption(int option_id, Object val) throws SocketException
	{
		try
		{
			if(false) throw new cli.System.Net.Sockets.SocketException();
			switch(option_id)
			{
				case SocketOptions.TCP_NODELAY:
					socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Tcp), SocketOptionName.wrap(SocketOptionName.NoDelay), ((Boolean)val).booleanValue() ? 1 : 0);
					break;
				case SocketOptions.SO_KEEPALIVE:
					socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.KeepAlive), ((Boolean)val).booleanValue() ? 1 : 0);
					break;
				case SocketOptions.SO_TIMEOUT:
					socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReceiveTimeout), ((Integer)val).intValue());
					socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.SendTimeout), ((Integer)val).intValue());
					break;
				case SocketOptions.SO_LINGER:
				{
					cli.System.Net.Sockets.LingerOption linger;
					if(val instanceof Boolean)
					{
						linger = new cli.System.Net.Sockets.LingerOption(false, 0);
					}
					else
					{
						linger = new cli.System.Net.Sockets.LingerOption(true, ((Integer)val).intValue());
					}
					socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.Linger), linger);
					break;
				}
				case SocketOptions.SO_OOBINLINE:
					socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.OutOfBandInline), ((Boolean)val).booleanValue() ? 1 : 0);
					break;
				case SocketOptions.SO_SNDBUF:
					socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.SendBuffer), ((Integer)val).intValue());
					break;
				case SocketOptions.SO_RCVBUF:
					socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReceiveBuffer), ((Integer)val).intValue());
					break;
				case SocketOptions.SO_REUSEADDR:
					socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReuseAddress), ((Boolean)val).booleanValue() ? 1 : 0);
					break;
				case SocketOptions.IP_TOS:
					socket.SetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.TypeOfService), ((Integer)val).intValue());
					break;
				default:
					throw new Error("Socket.setOption(" + option_id + ") not implemented");
			}
		}
		catch(cli.System.Net.Sockets.SocketException x)
		{
			// TODO error handling
			throw new SocketException(x.getMessage());
		}
	}

	/**
	 * Returns the current setting of the specified option.  The Object returned
	 * will be an Integer for options that have integer values.  The option_id
	 * is one of the defined constants in this interface.
	 *
	 * @param option_id The option identifier
	 *
	 * @return The current value of the option
	 *
	 * @exception SocketException If an error occurs
	 */
	public Object getOption(int option_id) throws SocketException
	{
		try
		{
			if(false) throw new cli.System.Net.Sockets.SocketException();
			switch(option_id)
			{
				case SocketOptions.SO_BINDADDR:
					try
					{
						return InetAddress.getByAddress(getLocalAddress(socket));
					}
					catch(UnknownHostException x)
					{
						throw new SocketException(x.getMessage());
					}
				case SocketOptions.TCP_NODELAY:
					return new Boolean(CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Tcp), SocketOptionName.wrap(SocketOptionName.NoDelay))) != 0);
				case SocketOptions.SO_KEEPALIVE:
					return new Boolean(CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.KeepAlive))) != 0);
				case SocketOptions.SO_TIMEOUT:
					return new Integer(CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReceiveTimeout))));
				case SocketOptions.SO_LINGER:
				{
					cli.System.Net.Sockets.LingerOption linger = (cli.System.Net.Sockets.LingerOption)socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.Linger));
					if(linger.get_Enabled())
					{
						return new Integer(linger.get_LingerTime());
					}
					return Boolean.FALSE;
				}
				case SocketOptions.SO_OOBINLINE:
					return new Integer(CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.OutOfBandInline))));
				case SocketOptions.SO_SNDBUF:
					return new Integer(CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.SendBuffer))));
				case SocketOptions.SO_RCVBUF:
					return new Integer(CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReceiveBuffer))));
				case SocketOptions.SO_REUSEADDR:
					return new Boolean(CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.Socket), SocketOptionName.wrap(SocketOptionName.ReuseAddress))) != 0);
				case SocketOptions.IP_TOS:
					return new Integer(CIL.unbox_int(socket.GetSocketOption(SocketOptionLevel.wrap(SocketOptionLevel.IP), SocketOptionName.wrap(SocketOptionName.TypeOfService))));
				default:
					throw new Error("Socket.getOption(" + option_id + ") not implemented");
			}
		}
		catch(cli.System.Net.Sockets.SocketException x)
		{
			// TODO error handling
			throw new SocketException(x.getMessage());
		}
	}

	private static byte[] getLocalAddress(cli.System.Net.Sockets.Socket socket)
	{
	    int address = (int)((cli.System.Net.IPEndPoint)socket.get_LocalEndPoint()).get_Address().get_Address();
	    return new byte[] { (byte)address, (byte)(address >> 8), (byte)(address >> 16), (byte)(address >> 24) };
	}

	/**
	 * Returns an InputStream object for reading from this socket.  This will
	 * be an instance of SocketInputStream.
	 *
	 * @return An InputStream
	 *
	 * @exception IOException If an error occurs
	 */
	protected InputStream getInputStream() throws IOException
	{
		return new SocketInputStream(this);
	}

	/**
	 * Returns an OutputStream object for writing to this socket.  This will
	 * be an instance of SocketOutputStream.
	 * 
	 * @return An OutputStream
	 *
	 * @exception IOException If an error occurs
	 */
	protected OutputStream getOutputStream() throws IOException
	{
		return new SocketOutputStream(this);
	}

	public void connect(SocketAddress address, int timeout) throws IOException
	{
		// NOTE for now we ignore the timeout and we only support InetSocketAddress
		InetSocketAddress inetAddress = (InetSocketAddress)address;
		connect(inetAddress.getAddress(), inetAddress.getPort());
	}

	public void sendUrgentData(int data)
	{
		throw new InternalError ("PlainSocketImpl::sendUrgentData not implemented");
	}

	public void shutdownInput()
	{
		throw new InternalError ("PlainSocketImpl::shutdownInput not implemented");
	}

	public void shutdownOutput()
	{
		throw new InternalError ("PlainSocketImpl::shutdownOutput not implemented");
	}
} // class PlainSocketImpl
