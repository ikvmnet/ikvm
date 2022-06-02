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

using FileDescriptor = java.io.FileDescriptor;

namespace IKVM.Runtime.Internal.Sun.Nio.Ch
{

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
