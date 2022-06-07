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

namespace IKVM.Java.Externs.java.net
{

    static class SocketInputStream
	{

		public static int socketRead0(object _this, global::java.io.FileDescriptor fd, byte[] b, int off, int len, int timeout)
		{
#if FIRST_PASS
			return 0;
#else
			// [IKVM] this method is a direct port of the native code in openjdk6-b18\jdk\src\windows\native\java\net\SocketInputStream.c
			System.Net.Sockets.Socket socket = null;
			int nread;

			if (fd == null)
			{
				throw new global::java.net.SocketException("socket closed");
			}
			socket = fd.getSocket();
			if (socket == null)
			{
				throw new global::java.net.SocketException("Socket closed");
			}

			if (timeout != 0)
			{
				if (timeout <= 5000 || !global::java.net.net_util_md.isRcvTimeoutSupported)
				{
					int ret = global::java.net.net_util_md.NET_Timeout(socket, timeout);

					if (ret <= 0)
					{
						if (ret == 0)
						{
							throw new global::java.net.SocketTimeoutException("Read timed out");
						}
						else
						{
							// [IKVM] the OpenJDK native code is broken and always throws this exception on any failure of NET_Timeout
							throw new global::java.net.SocketException("socket closed");
						}
					}

					/*check if the socket has been closed while we were in timeout*/
					if (fd.getSocket() == null)
					{
						throw new global::java.net.SocketException("Socket Closed");
					}
				}
			}

			nread = global::ikvm.@internal.Winsock.recv(socket, b, off, len, 0);
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
					switch (global::ikvm.@internal.Winsock.WSAGetLastError())
					{
						case global::ikvm.@internal.Winsock.WSAEINTR:
							throw new global::java.net.SocketException("socket closed");

						case global::ikvm.@internal.Winsock.WSAECONNRESET:
						case global::ikvm.@internal.Winsock.WSAESHUTDOWN:
							/*
							 * Connection has been reset - Windows sometimes reports
							 * the reset as a shutdown error.
							 */
							throw new global::sun.net.ConnectionResetException();

						case global::ikvm.@internal.Winsock.WSAETIMEDOUT:
							throw new global::java.net.SocketTimeoutException("Read timed out");

						default:
							throw global::java.net.net_util_md.NET_ThrowCurrent("recv failed");
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

}