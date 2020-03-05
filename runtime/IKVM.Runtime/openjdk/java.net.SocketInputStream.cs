/*
 * Copyright (c) 1995, 2013, Oracle and/or its affiliates. All rights reserved.
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
#if !FIRST_PASS
using Winsock = ikvm.@internal.Winsock;
using java.net;
#endif

static class Java_java_net_SocketInputStream
{
	public static int socketRead0(object _this, java.io.FileDescriptor fd, byte[] b, int off, int len, int timeout)
	{
#if FIRST_PASS
		return 0;
#else
		// [IKVM] this method is a direct port of the native code in openjdk6-b18\jdk\src\windows\native\java\net\SocketInputStream.c
		System.Net.Sockets.Socket socket = null;
		int nread;

		if (fd == null)
		{
			throw new SocketException("socket closed");
		}
		socket = fd.getSocket();
		if (socket == null)
		{
			throw new SocketException("Socket closed");
		}

		if (timeout != 0)
		{
			if (timeout <= 5000 || !net_util_md.isRcvTimeoutSupported)
			{
				int ret = net_util_md.NET_Timeout(socket, timeout);

				if (ret <= 0)
				{
					if (ret == 0)
					{
						throw new SocketTimeoutException("Read timed out");
					}
					else
					{
						// [IKVM] the OpenJDK native code is broken and always throws this exception on any failure of NET_Timeout
						throw new SocketException("socket closed");
					}
				}

				/*check if the socket has been closed while we were in timeout*/
				if (fd.getSocket() == null)
				{
					throw new SocketException("Socket Closed");
				}
			}
		}

		nread = Winsock.recv(socket, b, off, len, 0);
		if (nread > 0)
		{
			// ok
		}
		else
		{
			if (nread < 0)
			{
				/*
				 * Recv failed.
				 */
				switch (Winsock.WSAGetLastError())
				{
					case Winsock.WSAEINTR:
						throw new SocketException("socket closed");

					case Winsock.WSAECONNRESET:
					case Winsock.WSAESHUTDOWN:
						/*
						 * Connection has been reset - Windows sometimes reports
						 * the reset as a shutdown error.
						 */
						throw new sun.net.ConnectionResetException();

					case Winsock.WSAETIMEDOUT:
						throw new SocketTimeoutException("Read timed out");

					default:
						throw net_util_md.NET_ThrowCurrent("recv failed");
				}
			}
		}
		return nread;
#endif
	}

	public static void init()
	{
	}
}

static class Java_java_net_SocketOutputStream
{
	public static void socketWrite0(object _this, java.io.FileDescriptor fd, byte[] data, int off, int len)
	{
#if !FIRST_PASS
		// [IKVM] this method is a direct port of the native code in openjdk6-b18\jdk\src\windows\native\java\net\SocketOutputStream.c
		const int MAX_BUFFER_LEN = 2048;
		System.Net.Sockets.Socket socket;
		int buflen = 65536; // MAX_HEAP_BUFFER_LEN
		int n;

		if (fd == null)
		{
			throw new SocketException("socket closed");
		}
		else
		{
			socket = fd.getSocket();
		}
		if (data == null)
		{
			throw new java.lang.NullPointerException("data argument");
		}

		while (len > 0)
		{
			int loff = 0;
			int chunkLen = java.lang.Math.min(buflen, len);
			int llen = chunkLen;
			int retry = 0;

			while (llen > 0)
			{
				n = Winsock.send(socket, data, off + loff, llen, 0);
				if (n > 0)
				{
					llen -= n;
					loff += n;
					continue;
				}

				/*
				 * Due to a bug in Windows Sockets (observed on NT and Windows
				 * 2000) it may be necessary to retry the send. The issue is that
				 * on blocking sockets send/WSASend is supposed to block if there
				 * is insufficient buffer space available. If there are a large
				 * number of threads blocked on write due to congestion then it's
				 * possile to hit the NT/2000 bug whereby send returns WSAENOBUFS.
				 * The workaround we use is to retry the send. If we have a
				 * large buffer to send (>2k) then we retry with a maximum of
				 * 2k buffer. If we hit the issue with <=2k buffer then we backoff
				 * for 1 second and retry again. We repeat this up to a reasonable
				 * limit before bailing out and throwing an exception. In load
				 * conditions we've observed that the send will succeed after 2-3
				 * attempts but this depends on network buffers associated with
				 * other sockets draining.
				 */
				if (Winsock.WSAGetLastError() == Winsock.WSAENOBUFS)
				{
					if (llen > MAX_BUFFER_LEN)
					{
						buflen = MAX_BUFFER_LEN;
						chunkLen = MAX_BUFFER_LEN;
						llen = MAX_BUFFER_LEN;
						continue;
					}
					if (retry >= 30)
					{
						throw new SocketException("No buffer space available - exhausted attempts to queue buffer");
					}
					System.Threading.Thread.Sleep(1000);
					retry++;
					continue;
				}

				/*
				 * Send failed - can be caused by close or write error.
				 */
				if (Winsock.WSAGetLastError() == Winsock.WSAENOTSOCK)
				{
					throw new SocketException("Socket closed");
				}
				else
				{
					throw net_util_md.NET_ThrowCurrent("socket write error");
				}
			}
			len -= chunkLen;
			off += chunkLen;
		}
#endif
	}

	public static void init()
	{
	}
}
