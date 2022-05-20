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
using System.Collections.Generic;
using FileDescriptor = java.io.FileDescriptor;
using InetAddress = java.net.InetAddress;
using ByteBuffer = java.nio.ByteBuffer;

static class Java_sun_nio_ch_DatagramChannelImpl
{
	public static void initIDs()
	{
	}

	public static void disconnect0(FileDescriptor fd, bool isIPv6)
	{
#if !FIRST_PASS
		try
		{
			fd.getSocket().Connect(new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0));
			IKVM.NativeCode.sun.nio.ch.Net.setConnectionReset(fd.getSocket(), false);
		}
		catch (System.Net.Sockets.SocketException x)
		{
			throw java.net.SocketUtil.convertSocketExceptionToIOException(x);
		}
		catch (ObjectDisposedException)
		{
			throw new java.net.SocketException("Socket is closed");
		}
#endif
	}

	public static int receive0(object obj, FileDescriptor fd, byte[] buf, int pos, int len, bool connected)
	{
#if FIRST_PASS
		return 0;
#else
		sun.nio.ch.DatagramChannelImpl impl = (sun.nio.ch.DatagramChannelImpl)obj;
		java.net.SocketAddress remoteAddress = impl.remoteAddress();
		System.Net.EndPoint remoteEP;
		if (fd.getSocket().AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
		{
			remoteEP = new System.Net.IPEndPoint(System.Net.IPAddress.IPv6Any, 0);
		}
		else
		{
			remoteEP = new System.Net.IPEndPoint(0, 0);
		}
		java.net.InetSocketAddress addr;
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
					if (x.ErrorCode == java.net.SocketUtil.WSAECONNRESET)
					{
						// A previous send failed (i.e. the remote host responded with a ICMP that the port is closed) and
						// the winsock stack helpfully lets us know this, but we only care about this when we're connected,
						// otherwise we'll simply retry the receive (note that we use SIO_UDP_CONNRESET to prevent these
						// WSAECONNRESET exceptions, but when switching from connected to disconnected, some can slip through).
						if (connected)
						{
							throw new java.net.PortUnreachableException();
						}
						continue;
					}
					if (x.ErrorCode == java.net.SocketUtil.WSAEMSGSIZE)
					{
						// The buffer size was too small for the packet, ReceiveFrom receives the part of the packet
						// that fits in the buffer and then throws an exception, so we have to ignore the exception in this case.
						length = len;
						break;
					}
					if (x.ErrorCode == java.net.SocketUtil.WSAEWOULDBLOCK)
					{
						return sun.nio.ch.IOStatus.UNAVAILABLE;
					}
					throw java.net.SocketUtil.convertSocketExceptionToIOException(x);
				}
				catch (ObjectDisposedException)
				{
					throw new java.net.SocketException("Socket is closed");
				}
			}
			System.Net.IPEndPoint ep = (System.Net.IPEndPoint)remoteEP;
			addr = new java.net.InetSocketAddress(java.net.SocketUtil.getInetAddressFromIPEndPoint(ep), ep.Port);
		} while (remoteAddress != null && !addr.equals(remoteAddress));
		impl.sender = addr;
		return length;
#endif
	}

	public static int send0(object obj, bool preferIPv6, FileDescriptor fd, byte[] buf, int pos, int len, InetAddress addr, int port)
	{
#if FIRST_PASS
		return 0;
#else
		try
		{
			return fd.getSocket().SendTo(buf, pos, len, System.Net.Sockets.SocketFlags.None, new System.Net.IPEndPoint(java.net.SocketUtil.getAddressFromInetAddress(addr, preferIPv6), port));
		}
		catch (System.Net.Sockets.SocketException x)
		{
			if (x.ErrorCode == java.net.SocketUtil.WSAEWOULDBLOCK)
			{
				return sun.nio.ch.IOStatus.UNAVAILABLE;
			}
			throw java.net.SocketUtil.convertSocketExceptionToIOException(x);
		}
		catch (ObjectDisposedException)
		{
			throw new java.net.SocketException("Socket is closed");
		}
#endif
	}
}

#if !FIRST_PASS
namespace IKVM.Internal.AsyncSocket
{
	using System.Net;
	using System.Net.Sockets;

	abstract class OperationBase<TInput>
	{
		private static readonly AsyncCallback callback = CallbackProc;
		private Socket socket;
		private sun.nio.ch.Iocp.ResultHandler handler;
		private int result;
		private Exception exception;

		internal int Do(Socket socket, TInput input, object handler)
		{
			try
			{
				this.socket = socket;
				this.handler = (sun.nio.ch.Iocp.ResultHandler)handler;
				IAsyncResult ar = Begin(socket, input, callback, this);
				if (ar.CompletedSynchronously)
				{
					if (exception != null)
					{
						throw exception;
					}
					return result;
				}
				else
				{
					return sun.nio.ch.IOStatus.UNAVAILABLE;
				}
			}
			catch (SocketException x)
			{
				throw java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
			catch (ObjectDisposedException)
			{
				throw new java.nio.channels.ClosedChannelException();
			}
		}

		private static void CallbackProc(IAsyncResult ar)
		{
			OperationBase<TInput> obj = (OperationBase<TInput>)ar.AsyncState;
			try
			{
				int result = obj.End(obj.socket, ar);
				if (ar.CompletedSynchronously)
				{
					obj.result = result;
				}
				else
				{
					obj.handler.completed(result, false);
				}
			}
			catch (SocketException x)
			{
				if (ar.CompletedSynchronously)
				{
					obj.exception = x;
				}
				else
				{
					obj.handler.failed(x.ErrorCode, java.net.SocketUtil.convertSocketExceptionToIOException(x));
				}
			}
			catch (ObjectDisposedException x)
			{
				if (ar.CompletedSynchronously)
				{
					obj.exception = x;
				}
				else
				{
					obj.handler.failed(0, new java.nio.channels.ClosedChannelException());
				}
			}
		}

		protected abstract IAsyncResult Begin(Socket socket, TInput input, AsyncCallback callback, object state);
		protected abstract int End(Socket socket, IAsyncResult ar);
	}
}
#endif

static class Java_sun_nio_ch_WindowsAsynchronousServerSocketChannelImpl
{
#if !FIRST_PASS
	sealed class Accept : IKVM.Internal.AsyncSocket.OperationBase<System.Net.Sockets.Socket>
	{
		protected override IAsyncResult Begin(System.Net.Sockets.Socket listenSocket, System.Net.Sockets.Socket acceptSocket, AsyncCallback callback, object state)
		{
			return listenSocket.BeginAccept(acceptSocket, 0, callback, state);
		}

		protected override int End(System.Net.Sockets.Socket socket, IAsyncResult ar)
		{
			socket.EndAccept(ar);
			return 0;
		}
	}
#endif

	public static void initIDs()
	{
	}

	public static int accept0(FileDescriptor listenSocket, FileDescriptor acceptSocket, object handler)
	{
#if FIRST_PASS
		return 0;
#else
		return new Accept().Do(listenSocket.getSocket(), acceptSocket.getSocket(), handler);
#endif
	}

	public static void updateAcceptContext(FileDescriptor listenSocket, FileDescriptor acceptSocket)
	{
		// already handled by .NET Framework
	}

	public static void closesocket0(long socket)
	{
		// unused
	}
}

static class Java_sun_nio_ch_WindowsAsynchronousSocketChannelImpl
{
#if !FIRST_PASS
	sealed class Connect : IKVM.Internal.AsyncSocket.OperationBase<System.Net.IPEndPoint>
	{
		protected override IAsyncResult Begin(System.Net.Sockets.Socket socket, System.Net.IPEndPoint remoteEP, AsyncCallback callback, object state)
		{
			return socket.BeginConnect(remoteEP, callback, state);
		}

		protected override int End(System.Net.Sockets.Socket socket, IAsyncResult ar)
		{
			socket.EndConnect(ar);
			return 0;
		}
	}

	private static List<ArraySegment<byte>> ByteBuffersToList(ByteBuffer[] bufs)
	{
		List<ArraySegment<byte>> list = new List<ArraySegment<byte>>(bufs.Length);
		foreach (ByteBuffer bb in bufs)
		{
			list.Add(new ArraySegment<byte>(bb.array(), bb.arrayOffset() + bb.position(), bb.remaining()));
		}
		return list;
	}

	sealed class Receive : IKVM.Internal.AsyncSocket.OperationBase<ByteBuffer[]>
	{
		protected override IAsyncResult Begin(System.Net.Sockets.Socket socket, ByteBuffer[] bufs, AsyncCallback callback, object state)
		{
			return socket.BeginReceive(ByteBuffersToList(bufs), System.Net.Sockets.SocketFlags.None, callback, state);
		}

		protected override int End(System.Net.Sockets.Socket socket, IAsyncResult ar)
		{
			return socket.EndReceive(ar);
		}
	}

	sealed class Send : IKVM.Internal.AsyncSocket.OperationBase<ByteBuffer[]>
	{
		protected override IAsyncResult Begin(System.Net.Sockets.Socket socket, ByteBuffer[] bufs, AsyncCallback callback, object state)
		{
			return socket.BeginSend(ByteBuffersToList(bufs), System.Net.Sockets.SocketFlags.None, callback, state);
		}

		protected override int End(System.Net.Sockets.Socket socket, IAsyncResult ar)
		{
			return socket.EndSend(ar);
		}
	}
#endif

	public static void initIDs()
	{
	}

	public static int connect0(FileDescriptor fd, bool preferIPv6, InetAddress remote, int remotePort, object handler)
	{
#if FIRST_PASS
		return 0;
#else
		return new Connect().Do(fd.getSocket(), new System.Net.IPEndPoint(java.net.SocketUtil.getAddressFromInetAddress(remote, preferIPv6), remotePort), handler);
#endif
	}

	public static void updateConnectContext(FileDescriptor fd)
	{
		// already handled by .NET Framework
	}

	public static int read0(FileDescriptor fd, ByteBuffer[] bufs, object handler)
	{
#if FIRST_PASS
		return 0;
#else
		return new Receive().Do(fd.getSocket(), bufs, handler);
#endif
	}

	public static int write0(FileDescriptor fd, ByteBuffer[] bufs, object handler)
	{
#if FIRST_PASS
		return 0;
#else
		return new Send().Do(fd.getSocket(), bufs, handler);
#endif
	}

	public static void shutdown0(long socket, int how)
	{
		// unused
	}

	public static void closesocket0(long socket)
	{
		// unused
	}
}

namespace IKVM.NativeCode.sun.nio.ch
{
	static class SocketDispatcher
	{
		public static long read(object nd, FileDescriptor fd, ByteBuffer[] bufs, int offset, int length)
		{
#if FIRST_PASS
			return 0;
#else
			ByteBuffer[] altBufs = null;
			List<ArraySegment<byte>> list = new List<ArraySegment<byte>>(length);
			for (int i = 0; i < length; i++)
			{
				ByteBuffer bb = bufs[i + offset];
				if (!bb.hasArray())
				{
					if (altBufs == null)
					{
						altBufs = new ByteBuffer[bufs.Length];
					}
					bb = altBufs[i + offset] = ByteBuffer.allocate(bb.remaining());
				}
				list.Add(new ArraySegment<byte>(bb.array(), bb.arrayOffset() + bb.position(), bb.remaining()));
			}
			int count;
			try
			{
				count = fd.getSocket().Receive(list);
			}
			catch (System.Net.Sockets.SocketException x)
			{
				if (x.ErrorCode == global::java.net.SocketUtil.WSAEWOULDBLOCK)
				{
					count = 0;
				}
				else
				{
					throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
				}
			}
			catch (ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
			int total = count;
			for (int i = 0; total > 0 && i < length; i++)
			{
				ByteBuffer bb = bufs[i + offset];
				ByteBuffer abb;
				int consumed = Math.Min(total, bb.remaining());
				if (altBufs != null && (abb = altBufs[i + offset]) != null)
				{
					abb.position(consumed);
					abb.flip();
					bb.put(abb);
				}
				else
				{
					bb.position(bb.position() + consumed);
				}
				total -= consumed;
			}
			return count;
#endif
		}

		public static long write(object nd, FileDescriptor fd, ByteBuffer[] bufs, int offset, int length)
		{
#if FIRST_PASS
			return 0;
#else
			ByteBuffer[] altBufs = null;
			List<ArraySegment<byte>> list = new List<ArraySegment<byte>>(length);
			for (int i = 0; i < length; i++)
			{
				ByteBuffer bb = bufs[i + offset];
				if (!bb.hasArray())
				{
					if (altBufs == null)
					{
						altBufs = new ByteBuffer[bufs.Length];
					}
					ByteBuffer abb = ByteBuffer.allocate(bb.remaining());
					int pos = bb.position();
					abb.put(bb);
					bb.position(pos);
					abb.flip();
					bb = altBufs[i + offset] = abb;
				}
				list.Add(new ArraySegment<byte>(bb.array(), bb.arrayOffset() + bb.position(), bb.remaining()));
			}
			int count;
			try
			{
				count = fd.getSocket().Send(list);
			}
			catch (System.Net.Sockets.SocketException x)
			{
				if (x.ErrorCode == global::java.net.SocketUtil.WSAEWOULDBLOCK)
				{
					count = 0;
				}
				else
				{
					throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
				}
			}
			catch (ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
			int total = count;
			for (int i = 0; total > 0 && i < length; i++)
			{
				ByteBuffer bb = bufs[i + offset];
				int consumed = Math.Min(total, bb.remaining());
				bb.position(bb.position() + consumed);
				total -= consumed;
			}
			return count;
#endif
		}
	}

	static class Net
	{
		public static bool isIPv6Available0()
		{
			string env = IKVM.Internal.JVM.SafeGetEnvironmentVariable("IKVM_IPV6");
			int val;
			if (env != null && Int32.TryParse(env, out val))
			{
				return (val & 2) != 0;
			}
			// we only support IPv6 on Vista and up (because there is no TwoStacks nio implementation)
			// (non-Windows OSses are currently not supported)
			// Mono on Windows doesn't appear to support IPv6 either (Mono on Linux does).
			return Type.GetType("Mono.Runtime") == null
				&& System.Net.Sockets.Socket.OSSupportsIPv6
				&& Environment.OSVersion.Platform == PlatformID.Win32NT
				&& Environment.OSVersion.Version.Major >= 6;
		}

		public static int isExclusiveBindAvailable()
		{
			return Environment.OSVersion.Platform == PlatformID.Win32NT
				? Environment.OSVersion.Version.Major >= 6 ? 1 : 0
				: -1;
		}

		public static bool canIPv6SocketJoinIPv4Group0()
		{
			return false;
		}

		public static bool canJoin6WithIPv4Group0()
		{
			return false;
		}

		public static void shutdown(FileDescriptor fd, int how)
		{
#if !FIRST_PASS
			try
			{
				fd.getSocket().Shutdown(how == global::sun.nio.ch.Net.SHUT_RD
					? System.Net.Sockets.SocketShutdown.Receive
					: System.Net.Sockets.SocketShutdown.Send);
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

		public static int localPort(FileDescriptor fd)
		{
#if FIRST_PASS
			return 0;
#else
			try
			{
				System.Net.IPEndPoint ep = (System.Net.IPEndPoint)fd.getSocket().LocalEndPoint;
				return ep.Port;
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
		}

		public static InetAddress localInetAddress(FileDescriptor fd)
		{
#if FIRST_PASS
			return null;
#else
			try
			{
				System.Net.IPEndPoint ep = (System.Net.IPEndPoint)fd.getSocket().LocalEndPoint;
				return global::java.net.SocketUtil.getInetAddressFromIPEndPoint(ep);
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
		}

		public static int remotePort(FileDescriptor fd)
		{
#if FIRST_PASS
			return 0;
#else
			try
			{
				System.Net.IPEndPoint ep = (System.Net.IPEndPoint)fd.getSocket().RemoteEndPoint;
				return ep.Port;
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
		}

		public static InetAddress remoteInetAddress(FileDescriptor fd)
		{
#if FIRST_PASS
			return null;
#else
			try
			{
				System.Net.IPEndPoint ep = (System.Net.IPEndPoint)fd.getSocket().RemoteEndPoint;
				return global::java.net.SocketUtil.getInetAddressFromIPEndPoint(ep);
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
		}

		public static int getIntOption0(FileDescriptor fd, bool mayNeedConversion, int level, int opt)
		{
#if FIRST_PASS
			return 0;
#else
			if (level == global::ikvm.@internal.Winsock.IPPROTO_IPV6 && opt == global::ikvm.@internal.Winsock.IPV6_TCLASS)
			{
				return 0;
			}
			System.Net.Sockets.SocketOptionLevel sol = (System.Net.Sockets.SocketOptionLevel)level;
			System.Net.Sockets.SocketOptionName son = (System.Net.Sockets.SocketOptionName)opt;
			try
			{
				object obj = fd.getSocket().GetSocketOption(sol, son);
				System.Net.Sockets.LingerOption linger = obj as System.Net.Sockets.LingerOption;
				if (linger != null)
				{
					return linger.Enabled ? linger.LingerTime : -1;
				}
				return (int)obj;
			}
			catch (System.Net.Sockets.SocketException x)
			{
				if (mayNeedConversion)
				{
					if (x.ErrorCode == global::java.net.SocketUtil.WSAENOPROTOOPT
						&& sol == System.Net.Sockets.SocketOptionLevel.IP
						&& son == System.Net.Sockets.SocketOptionName.TypeOfService)
					{
						return 0;
					}
				}
				throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
		}

		public static void setIntOption0(FileDescriptor fd, bool mayNeedConversion, int level, int opt, int arg, bool isIPv6)
		{
#if !FIRST_PASS
			if (level == global::ikvm.@internal.Winsock.IPPROTO_IPV6 && opt == global::ikvm.@internal.Winsock.IPV6_TCLASS)
			{
				return;
			}
			System.Net.Sockets.SocketOptionLevel sol = (System.Net.Sockets.SocketOptionLevel)level;
			System.Net.Sockets.SocketOptionName son = (System.Net.Sockets.SocketOptionName)opt;
			if (mayNeedConversion)
			{
				const int IPTOS_TOS_MASK = 0x1e;
				const int IPTOS_PREC_MASK = 0xe0;
				if (sol == System.Net.Sockets.SocketOptionLevel.IP
					&& son == System.Net.Sockets.SocketOptionName.TypeOfService)
				{
					arg &= (IPTOS_TOS_MASK | IPTOS_PREC_MASK);
				}
			}
			try
			{
				fd.getSocket().SetSocketOption(sol, son, arg);
			}
			catch (System.Net.Sockets.SocketException x)
			{
				if (mayNeedConversion)
				{
					if (x.ErrorCode == global::java.net.SocketUtil.WSAENOPROTOOPT
						&& sol == System.Net.Sockets.SocketOptionLevel.IP
						&& (son == System.Net.Sockets.SocketOptionName.TypeOfService || son == System.Net.Sockets.SocketOptionName.MulticastLoopback))
					{
						return;
					}
					if (x.ErrorCode == global::java.net.SocketUtil.WSAEINVAL
						&& sol == System.Net.Sockets.SocketOptionLevel.IP
						&& son == System.Net.Sockets.SocketOptionName.TypeOfService)
					{
						return;
					}
				}
				throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
		}

		private static void PutInt(byte[] buf, int pos, int value)
		{
			buf[pos + 0] = (byte)(value >> 24);
			buf[pos + 1] = (byte)(value >> 16);
			buf[pos + 2] = (byte)(value >> 8);
			buf[pos + 3] = (byte)(value >> 0);
		}

		public static int joinOrDrop4(bool join, FileDescriptor fd, int group, int interf, int source)
		{
#if FIRST_PASS
			return 0;
#else
			try
			{
				if (source == 0)
				{
					fd.getSocket().SetSocketOption(System.Net.Sockets.SocketOptionLevel.IP,
						join ? System.Net.Sockets.SocketOptionName.AddMembership : System.Net.Sockets.SocketOptionName.DropMembership,
						new System.Net.Sockets.MulticastOption(new System.Net.IPAddress(System.Net.IPAddress.HostToNetworkOrder(group) & 0xFFFFFFFFL), new System.Net.IPAddress(System.Net.IPAddress.HostToNetworkOrder(interf) & 0xFFFFFFFFL)));
				}
				else
				{
					// ip_mreq_source
					byte[] optionValue = new byte[12];
					PutInt(optionValue, 0, group);
					PutInt(optionValue, 4, source);
					PutInt(optionValue, 8, interf);
					fd.getSocket().SetSocketOption(System.Net.Sockets.SocketOptionLevel.IP,
						join ? System.Net.Sockets.SocketOptionName.AddSourceMembership : System.Net.Sockets.SocketOptionName.DropSourceMembership,
						optionValue);
				}
				return 0;
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
		}

		public static int blockOrUnblock4(bool block, FileDescriptor fd, int group, int interf, int source)
		{
#if FIRST_PASS
			return 0;
#else
			try
			{
				// ip_mreq_source
				byte[] optionValue = new byte[12];
				PutInt(optionValue, 0, group);
				PutInt(optionValue, 4, source);
				PutInt(optionValue, 8, interf);
				fd.getSocket().SetSocketOption(System.Net.Sockets.SocketOptionLevel.IP,
					block ? System.Net.Sockets.SocketOptionName.BlockSource : System.Net.Sockets.SocketOptionName.UnblockSource,
					optionValue);
				return 0;
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
		}

		// write a sockaddr_in6 into optionValue at offset pos
		private static void PutSockAddrIn6(byte[] optionValue, int pos, byte[] addr)
		{
			// sin6_family
			optionValue[pos] = 23; // AF_INET6

			// sin6_addr
			Buffer.BlockCopy(addr, 0, optionValue, pos + 8, addr.Length);
		}

		public static int joinOrDrop6(bool join, FileDescriptor fd, byte[] group, int index, byte[] source)
		{
#if FIRST_PASS
			return 0;
#else
			try
			{
				if (source == null)
				{
					fd.getSocket().SetSocketOption(System.Net.Sockets.SocketOptionLevel.IPv6,
						join ? System.Net.Sockets.SocketOptionName.AddMembership : System.Net.Sockets.SocketOptionName.DropMembership,
						new System.Net.Sockets.IPv6MulticastOption(new System.Net.IPAddress(group), index));
				}
				else
				{
					const System.Net.Sockets.SocketOptionName MCAST_JOIN_SOURCE_GROUP = (System.Net.Sockets.SocketOptionName)45;
					const System.Net.Sockets.SocketOptionName MCAST_LEAVE_SOURCE_GROUP = (System.Net.Sockets.SocketOptionName)46;
					// group_source_req
					byte[] optionValue = new byte[264];
					optionValue[0] = (byte)index;
					optionValue[1] = (byte)(index >> 8);
					optionValue[2] = (byte)(index >> 16);
					optionValue[3] = (byte)(index >> 24);
					PutSockAddrIn6(optionValue, 8, group);
					PutSockAddrIn6(optionValue, 136, source);
					fd.getSocket().SetSocketOption(System.Net.Sockets.SocketOptionLevel.IPv6, join ? MCAST_JOIN_SOURCE_GROUP : MCAST_LEAVE_SOURCE_GROUP, optionValue);
				}
				return 0;
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
		}

		public static int blockOrUnblock6(bool block, FileDescriptor fd, byte[] group, int index, byte[] source)
		{
#if FIRST_PASS
			return 0;
#else
			try
			{
				const System.Net.Sockets.SocketOptionName MCAST_BLOCK_SOURCE = (System.Net.Sockets.SocketOptionName)43;
				const System.Net.Sockets.SocketOptionName MCAST_UNBLOCK_SOURCE = (System.Net.Sockets.SocketOptionName)44;
				// group_source_req
				byte[] optionValue = new byte[264];
				optionValue[0] = (byte)index;
				optionValue[1] = (byte)(index >> 8);
				optionValue[2] = (byte)(index >> 16);
				optionValue[3] = (byte)(index >> 24);
				PutSockAddrIn6(optionValue, 8, group);
				PutSockAddrIn6(optionValue, 136, source);
				fd.getSocket().SetSocketOption(System.Net.Sockets.SocketOptionLevel.IPv6, block ? MCAST_BLOCK_SOURCE : MCAST_UNBLOCK_SOURCE, optionValue);
				return 0;
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
		}

		public static void setInterface4(FileDescriptor fd, int interf)
		{
#if !FIRST_PASS
			try
			{
				fd.getSocket().SetSocketOption(System.Net.Sockets.SocketOptionLevel.IP, System.Net.Sockets.SocketOptionName.MulticastInterface, System.Net.IPAddress.HostToNetworkOrder(interf));
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
		}

		public static int getInterface4(FileDescriptor fd)
		{
#if FIRST_PASS
			return 0;
#else
			try
			{
				return System.Net.IPAddress.NetworkToHostOrder((int)fd.getSocket().GetSocketOption(System.Net.Sockets.SocketOptionLevel.IP, System.Net.Sockets.SocketOptionName.MulticastInterface));
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
		}

		public static void setInterface6(FileDescriptor fd, int index)
		{
#if !FIRST_PASS
			try
			{
				fd.getSocket().SetSocketOption(System.Net.Sockets.SocketOptionLevel.IPv6, System.Net.Sockets.SocketOptionName.MulticastInterface, index);
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
		}

		public static int getInterface6(FileDescriptor fd)
		{
#if FIRST_PASS
			return 0;
#else
			try
			{
				return (int)fd.getSocket().GetSocketOption(System.Net.Sockets.SocketOptionLevel.IPv6, System.Net.Sockets.SocketOptionName.MulticastInterface);
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
		}

		public static FileDescriptor socket0(bool preferIPv6, bool stream, bool reuse)
		{
#if FIRST_PASS
			return null;
#else
			try
			{
				System.Net.Sockets.AddressFamily addressFamily = preferIPv6
					? System.Net.Sockets.AddressFamily.InterNetworkV6
					: System.Net.Sockets.AddressFamily.InterNetwork;
				System.Net.Sockets.SocketType socketType = stream
					? System.Net.Sockets.SocketType.Stream
					: System.Net.Sockets.SocketType.Dgram;
				System.Net.Sockets.ProtocolType protocolType = stream
					? System.Net.Sockets.ProtocolType.Tcp
					: System.Net.Sockets.ProtocolType.Udp;
				System.Net.Sockets.Socket socket = new System.Net.Sockets.Socket(addressFamily, socketType, protocolType);
				if (preferIPv6)
				{
					// enable IPv4 over IPv6 sockets (note that we don't have to check for >= Vista here, because nio sockets only support IPv6 on >= Vista)
					const System.Net.Sockets.SocketOptionName IPV6_V6ONLY = (System.Net.Sockets.SocketOptionName)27;
					socket.SetSocketOption(System.Net.Sockets.SocketOptionLevel.IPv6, IPV6_V6ONLY, 0);
				}
				if (!stream)
				{
					setConnectionReset(socket, false);
				}
				FileDescriptor fd = new FileDescriptor();
				fd.setSocket(socket);
				return fd;
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
#endif
		}

		public static void bind0(FileDescriptor fd, bool preferIPv6, bool useExclBind, InetAddress addr, int port)
		{
#if !FIRST_PASS
			try
			{
				if (useExclBind)
				{
					global::java.net.net_util_md.setExclusiveBind(fd.getSocket());
				}
				fd.getSocket().Bind(new System.Net.IPEndPoint(global::java.net.SocketUtil.getAddressFromInetAddress(addr, preferIPv6), port));
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
		}

		public static void listen(FileDescriptor fd, int backlog)
		{
#if !FIRST_PASS
			try
			{
				fd.getSocket().Listen(backlog);
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
		}

		internal static void setConnectionReset(System.Net.Sockets.Socket socket, bool enable)
		{
			// Windows 2000 introduced a "feature" that causes it to return WSAECONNRESET from receive,
			// if a previous send resulted in an ICMP port unreachable. For unconnected datagram sockets,
			// we disable this feature by using this ioctl.
			const int IOC_IN = unchecked((int)0x80000000);
			const int IOC_VENDOR = 0x18000000;
			const int SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;

			socket.IOControl(SIO_UDP_CONNRESET, new byte[] { enable ? (byte)1 : (byte)0 }, null);
		}

		public static int connect0(bool preferIPv6, FileDescriptor fd, InetAddress remote, int remotePort)
		{
#if FIRST_PASS
			return 0;
#else
			try
			{
				System.Net.IPEndPoint ep = new System.Net.IPEndPoint(global::java.net.SocketUtil.getAddressFromInetAddress(remote, preferIPv6), remotePort);
				bool datagram = fd.getSocket().SocketType == System.Net.Sockets.SocketType.Dgram;
				if (datagram || fd.isSocketBlocking())
				{
					fd.getSocket().Connect(ep);
					if (datagram)
					{
						setConnectionReset(fd.getSocket(), true);
					}
					return 1;
				}
				else
				{
					fd.setAsyncResult(fd.getSocket().BeginConnect(ep, null, null));
					return global::sun.nio.ch.IOStatus.UNAVAILABLE;
				}
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw new global::java.net.ConnectException(x.Message);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
		}

		public static int poll(FileDescriptor fd, int events, long timeout)
		{
#if FIRST_PASS
			return 0;
#else
			System.Net.Sockets.SelectMode selectMode;
			switch (events)
			{
				case global::sun.nio.ch.Net.POLLCONN:
				case global::sun.nio.ch.Net.POLLOUT:
					selectMode = System.Net.Sockets.SelectMode.SelectWrite;
					break;
				case global::sun.nio.ch.Net.POLLIN:
					selectMode = System.Net.Sockets.SelectMode.SelectRead;
					break;
				default:
					throw new NotSupportedException();
			}
			int microSeconds = timeout >= Int32.MaxValue / 1000 ? Int32.MaxValue : (int)(timeout * 1000);
			try
			{
				if (fd.getSocket().Poll(microSeconds, selectMode))
				{
					return events;
				}
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw new global::java.net.SocketException(x.Message);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
			return 0;
#endif
		}
	}

	static class ServerSocketChannelImpl
	{
		public static int accept0(object _this, FileDescriptor ssfd, FileDescriptor newfd, object isaa)
		{
#if FIRST_PASS
			return 0;
#else
			try
			{
				System.Net.Sockets.Socket netSocket = ssfd.getSocket();
				if (netSocket.Blocking || netSocket.Poll(0, System.Net.Sockets.SelectMode.SelectRead))
				{
					System.Net.Sockets.Socket accsock = netSocket.Accept();
					newfd.setSocket(accsock);
					System.Net.IPEndPoint ep = (System.Net.IPEndPoint)accsock.RemoteEndPoint;
					((global::java.net.InetSocketAddress[])isaa)[0] = new global::java.net.InetSocketAddress(global::java.net.SocketUtil.getInetAddressFromIPEndPoint(ep), ep.Port);
					return 1;
				}
				else
				{
					return global::sun.nio.ch.IOStatus.UNAVAILABLE;
				}
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw global::java.net.SocketUtil.convertSocketExceptionToIOException(x);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
		}

		public static void initIDs()
		{
		}
	}

	static class SocketChannelImpl
	{
		public static int checkConnect(FileDescriptor fd, bool block, bool ready)
		{
#if FIRST_PASS
			return 0;
#else
			try
			{
				IAsyncResult asyncConnect = fd.getAsyncResult();
				if (block || ready || asyncConnect.IsCompleted)
				{
					fd.setAsyncResult(null);
					fd.getSocket().EndConnect(asyncConnect);
					// work around for blocking issue
					fd.getSocket().Blocking = fd.isSocketBlocking();
					return 1;
				}
				else
				{
					return global::sun.nio.ch.IOStatus.UNAVAILABLE;
				}
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw new global::java.net.ConnectException(x.Message);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
		}

		public static int sendOutOfBandData(FileDescriptor fd, byte data)
		{
#if FIRST_PASS
			return 0;
#else
			try
			{
				fd.getSocket().Send(new byte[] { data }, 1, System.Net.Sockets.SocketFlags.OutOfBand);
				return 1;
			}
			catch (System.Net.Sockets.SocketException x)
			{
				throw new global::java.net.ConnectException(x.Message);
			}
			catch (System.ObjectDisposedException)
			{
				throw new global::java.net.SocketException("Socket is closed");
			}
#endif
		}
	}
}
