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

#if !FIRST_PASS
namespace IKVM.Runtime.Util.Sun.Nio.Ch
{
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
