/* PlainDatagramSocketImpl.java -- Default DatagramSocket implementation
   Copyright (C) 1998, 1999, 2001 Free Software Foundation, Inc.

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

import java.io.IOException;
import system.net.*;
import system.net.sockets.*;
import ikvm.lang.CIL;

/**
* This is the default socket implementation for datagram sockets.
* It makes native calls to C routines that implement BSD style
* SOCK_DGRAM sockets in the AF_INET family.
*
* @version 0.1
*
* @author Aaron M. Renn (arenn@urbanophile.com)
*/
public class PlainDatagramSocketImpl extends DatagramSocketImpl
{
	/*
	 * Static Variables
	 */

	/**
	 * Option id for the IP_TTL (time to live) value.
	 */
	private static final int IP_TTL = 0x1E61; // 7777


	/*
	 * Instance Variables
	 */

	private static class MyUdpClient extends UdpClient
	{
		MyUdpClient(system.net.IPEndPoint ep)
		{
			super(ep);
		}

		system.net.sockets.Socket getSocket()
		{
			return super.get_Client();
		}
	}

	/**
	 * This is the actual underlying socket
	 */
	private MyUdpClient socket;

	/*************************************************************************/

	/*
	 * Constructors
	 */

	/**
	 * Default do nothing constructor
	 */
	public PlainDatagramSocketImpl()
	{
	}

	/*************************************************************************/

	/*
	 * Instance Methods
	 */

	/**
	 * Creates a new datagram socket
	 *
	 * @exception SocketException If an error occurs
	 */
	protected void create() throws SocketException
	{
	}

	/*************************************************************************/

	/**
	 * Closes the socket
	 */
	protected void close()
	{
		socket.Close();
	}

	/*************************************************************************/

	/**
	 * Binds this socket to a particular port and interface
	 *
	 * @param port The port to bind to
	 * @param addr The address to bind to
	 *
	 * @exception SocketException If an error occurs
	 */
	protected void bind(int port, InetAddress addr) throws SocketException
	{
		// TODO error handling
		socket = new MyUdpClient(new IPEndPoint(PlainSocketImpl.getAddressFromInetAddress(addr), port));
	}

	/*************************************************************************/

	/**
	 * Sends a packet of data to a remote host
	 *
	 * @param packet The packet to send
	 *
	 * @exception IOException If an error occurs
	 */
	protected void send(DatagramPacket packet) throws IOException
	{
		// TODO error handling
		int len = packet.getLength();
		if(socket.Send(packet.getData(), len, new IPEndPoint(PlainSocketImpl.getAddressFromInetAddress(packet.getAddress()), packet.getPort())) != len)
		{
			// TODO
			throw new IOException();
		}
	}

	/*************************************************************************/

	/**
	 * What does this method really do?
	 */
	protected int peek(InetAddress addr) throws IOException
	{
		throw new IOException("Not Implemented Yet");
	}

	/*************************************************************************/

	/**
	 * Receives a UDP packet from the network
	 *
	 * @param packet The packet to fill in with the data received
	 *
	 * @exception IOException IOException If an error occurs
	 */
	protected void receive(DatagramPacket packet) throws IOException
	{
	    try
	    {
		if(false) throw new system.net.sockets.SocketException();
		byte[] data = packet.getData();
		int length = packet.getLength();
		system.net.IPEndPoint[] remoteEP = new system.net.IPEndPoint[] {
		    new system.net.IPEndPoint(0, 0)
		};
		byte[] buf = socket.Receive(remoteEP);
		System.arraycopy(buf, 0, data, 0, Math.min(length, buf.length));
		// I think the spec says that the Length property of DatagramPacket
		// contains the number of bytes in the network packet (even if
		// the buffer was smaller than the network packet)
		packet.setLength(buf.length);
		int remoteIP = (int)remoteEP[0].get_Address().get_Address();
		byte[] ipv4 = new byte[] { (byte)remoteIP, (byte)(remoteIP >> 8), (byte)(remoteIP >> 16), (byte)(remoteIP >> 24) };
		InetAddress remoteAddress = InetAddress.getByAddress(ipv4);
		packet.setAddress(remoteAddress);
		packet.setPort(remoteEP[0].get_Port());
	    }
	    catch(system.net.sockets.SocketException x)
	    {
		// TODO error handling
		throw new IOException(x.get_Message());
	    }
	}

	/*************************************************************************/

	/**
	 * Joins a multicast group
	 *
	 * @param addr The group to join
	 *
	 * @exception IOException If an error occurs
	 */
	protected void join(InetAddress addr) throws IOException
	{
		try
		{
			if(false) throw new system.net.sockets.SocketException();
			socket.JoinMulticastGroup(new system.net.IPAddress(PlainSocketImpl.getAddressFromInetAddress(addr)));
		}
		catch(system.net.sockets.SocketException x)
		{
			// TODO error handling
			throw new IOException(x.get_Message());
		}
	}

	/*************************************************************************/

	/**
	 * Leaves a multicast group
	 *
	 * @param addr The group to leave
	 *
	 * @exception IOException If an error occurs
	 */
	protected void leave(InetAddress addr) throws IOException
	{
		try
		{
			if(false) throw new system.net.sockets.SocketException();
			socket.DropMulticastGroup(new system.net.IPAddress(PlainSocketImpl.getAddressFromInetAddress(addr)));
		}
		catch(system.net.sockets.SocketException x)
		{
			// TODO error handling
			throw new IOException(x.get_Message());
		}
	}

	/*************************************************************************/

	/**
	 * Gets the Time to Live value for the socket
	 *
	 * @return The TTL value
	 *
	 * @exception IOException If an error occurs
	 */
	protected byte getTTL() throws IOException
	{
		Object obj = getOption(IP_TTL);

		if (!(obj instanceof Integer))
			throw new IOException("Internal Error");

		return(((Integer)obj).byteValue());
	}

	/*************************************************************************/

	/**
	 * Sets the Time to Live value for the socket
	 *
	 * @param ttl The new TTL value
	 *
	 * @exception IOException If an error occurs
	 */
	protected void setTTL(byte ttl) throws IOException
	{
		setOption(IP_TTL, new Integer(ttl & 0xFF));
	}

	/*************************************************************************/

	/**
	 * Gets the Time to Live value for the socket
	 *
	 * @return The TTL value
	 *
	 * @exception IOException If an error occurs
	 */
	protected int getTimeToLive() throws IOException
	{
		Object obj = getOption(IP_TTL);

		if (!(obj instanceof Integer))
			throw new IOException("Internal Error");

		return(((Integer)obj).intValue());
	}

	/*************************************************************************/

	/**
	 * Sets the Time to Live value for the socket
	 *
	 * @param ttl The new TTL value
	 *
	 * @exception IOException If an error occurs
	 */
	protected void setTimeToLive(int ttl) throws IOException
	{
		setOption(IP_TTL, new Integer(ttl));
	}

	/*************************************************************************/

	/**
	 * Retrieves the value of an option on the socket
	 *
	 * @param option_id The identifier of the option to retrieve
	 *
	 * @return The value of the option
	 *
	 * @exception SocketException If an error occurs
	 */
	public Object getOption(int option_id) throws SocketException
	{
		try
		{
			if(false) throw new system.net.sockets.SocketException();
			switch(option_id)
			{
				case IP_TTL:
					return new Integer(CIL.unbox_int(socket.getSocket().GetSocketOption(SocketOptionLevel.IP, SocketOptionName.IpTimeToLive)));
				default:
					throw new Error("getOption(" + option_id + ") not implemented");
			}
		}
		catch(system.net.sockets.SocketException x)
		{
			throw new SocketException(x.get_Message());
		}
	}

	/*************************************************************************/

	/**
	 * Sets the value of an option on the socket
	 *
	 * @param option_id The identifier of the option to set
	 * @param val The value of the option to set
	 *
	 * @exception SocketException If an error occurs
	 */
	public void setOption(int option_id, Object val) throws SocketException
	{
		try
		{
			if(false) throw new system.net.sockets.SocketException();
			switch(option_id)
			{
				case IP_TTL:
					socket.getSocket().SetSocketOption(SocketOptionLevel.IP, SocketOptionName.IpTimeToLive, ((Integer)val).intValue());
					break;
				default:
					throw new Error("getOption(" + option_id + ") not implemented");
			}
		}
		catch(system.net.sockets.SocketException x)
		{
			throw new SocketException(x.get_Message());
		}
	}

	public int peekData(DatagramPacket packet)
	{
		throw new InternalError ("PlainDatagramSocketImpl::peekData is not implemented");
	}

	public void joinGroup(SocketAddress address, NetworkInterface netIf)
	{
		throw new InternalError ("PlainDatagramSocketImpl::joinGroup is not implemented");
	}

	public void leaveGroup(SocketAddress address, NetworkInterface netIf)
	{
		throw new InternalError ("PlainDatagramSocketImpl::leaveGroup is not implemented");
	}
} // class PlainDatagramSocketImpl
