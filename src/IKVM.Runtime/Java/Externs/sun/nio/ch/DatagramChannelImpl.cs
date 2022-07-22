/*
  Copyright (C) 2011 Jeroen Frijters

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

using System;

namespace IKVM.Java.Externs.sun.nio.ch
{

    static class DatagramChannelImpl
    {

        public static void initIDs()
        {

        }

        public static void disconnect0(global::java.io.FileDescriptor fd, bool isIPv6)
        {
#if !FIRST_PASS
			try
			{
				fd.getSocket().Connect(new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0));
                Net.setConnectionReset(fd.getSocket(), false);
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
			catch (ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
        }

        public static int receive0(object obj, global::java.io.FileDescriptor fd, byte[] buf, int pos, int len, bool connected)
        {
#if FIRST_PASS
            return 0;
#else
			global::sun.nio.ch.DatagramChannelImpl impl = (global::sun.nio.ch.DatagramChannelImpl)obj;
			global::java.net.SocketAddress remoteAddress = impl.remoteAddress();
			System.Net.EndPoint remoteEP;
			if (fd.getSocket().AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
			{
				remoteEP = new System.Net.IPEndPoint(System.Net.IPAddress.IPv6Any, 0);
			}
			else
			{
				remoteEP = new System.Net.IPEndPoint(0, 0);
			}
			global::java.net.InetSocketAddress addr;
			int length;
			do
			{
				for (; ; )
				{
					try
					{
						length = fd.getSocket().ReceiveFrom(buf, pos, len, System.Net.Sockets.SocketFlags.None, ref remoteEP);
						break;
					}
					catch (System.Net.Sockets.SocketException x)
					{
						if (x.ErrorCode == global::java.net.SocketUtil.WSAECONNRESET)
						{
							// A previous send failed (i.e. the remote host responded with a ICMP that the port is closed) and
							// the winsock stack helpfully lets us know this, but we only care about this when we're connected,
							// otherwise we'll simply retry the receive (note that we use SIO_UDP_CONNRESET to prevent these
							// WSAECONNRESET exceptions, but when switching from connected to disconnected, some can slip through).
							if (connected)
							{
								throw new global::java.net.PortUnreachableException();
							}
							continue;
						}
						if (x.ErrorCode == global::java.net.SocketUtil.WSAEMSGSIZE)
						{
							// The buffer size was too small for the packet, ReceiveFrom receives the part of the packet
							// that fits in the buffer and then throws an exception, so we have to ignore the exception in this case.
							length = len;
							break;
						}
						if (x.ErrorCode == global::java.net.SocketUtil.WSAEWOULDBLOCK)
						{
							return global::sun.nio.ch.IOStatus.UNAVAILABLE;
						}
						throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
					}
					catch (ObjectDisposedException)
					{
						throw new global::java.net.SocketException("Socket is closed");
					}
				}
				System.Net.IPEndPoint ep = (System.Net.IPEndPoint)remoteEP;
				addr = new global::java.net.InetSocketAddress(global::java.net.SocketUtil.getInetAddressFromIPEndPoint(ep), ep.Port);
			} while (remoteAddress != null && !addr.equals(remoteAddress));
			impl.sender = addr;
			return length;
#endif
        }

        public static int send0(object obj, bool preferIPv6, global::java.io.FileDescriptor fd, byte[] buf, int pos, int len, global::java.net.InetAddress addr, int port)
        {
#if FIRST_PASS
            return 0;
#else
			try
			{
				return fd.getSocket().SendTo(buf, pos, len, System.Net.Sockets.SocketFlags.None, new System.Net.IPEndPoint(global::java.net.SocketUtil.getAddressFromInetAddress(addr, preferIPv6), port));
			}
			catch (System.Net.Sockets.SocketException x)
			{
				if (x.ErrorCode == global::java.net.SocketUtil.WSAEWOULDBLOCK)
				{
					return global::sun.nio.ch.IOStatus.UNAVAILABLE;
				}
				throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
			catch (ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
        }

    }

}
