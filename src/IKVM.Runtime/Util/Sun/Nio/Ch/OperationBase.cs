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

using System.Net.Sockets;

namespace IKVM.Runtime.Util.Sun.Nio.Ch
{

#if !FIRST_PASS

    abstract class OperationBase<TInput>
    {

        static readonly AsyncCallback callback = CallbackProc;
        Socket socket;
        global::sun.nio.ch.Iocp.ResultHandler handler;
        int result;
        Exception exception;

        internal int Do(Socket socket, TInput input, object handler)
        {
            try
            {
                this.socket = socket;
                this.handler = (global::sun.nio.ch.Iocp.ResultHandler)handler;
                var ar = Begin(socket, input, callback, this);
                if (ar.CompletedSynchronously)
                {
                    if (exception != null)
                        throw exception;

                    return result;
                }
                else
                {
                    return global::sun.nio.ch.IOStatus.UNAVAILABLE;
                }
            }
            catch (SocketException e)
            {
                throw java.net.SocketUtil.convertSocketExceptionToIOException(e);
            }
            catch (ObjectDisposedException)
            {
                throw new java.nio.channels.ClosedChannelException();
            }
        }

        static void CallbackProc(IAsyncResult ar)
        {
            var obj = (OperationBase<TInput>)ar.AsyncState;

            try
            {
                var result = obj.End(obj.socket, ar);
                if (ar.CompletedSynchronously)
                    obj.result = result;
                else
                    obj.handler.completed(result, false);
            }
            catch (SocketException e)
            {
                if (ar.CompletedSynchronously)
                {
                    obj.exception = e;
                }
                else
                {
                    obj.handler.failed((int)e.SocketErrorCode, global::java.net.SocketUtil.convertSocketExceptionToIOException(e));
                }
            }
            catch (ObjectDisposedException e)
            {
                if (ar.CompletedSynchronously)
                {
                    obj.exception = e;
                }
                else
                {
                    obj.handler.failed(0, new global::java.nio.channels.ClosedChannelException());
                }
            }
        }

        protected abstract IAsyncResult Begin(Socket socket, TInput input, AsyncCallback callback, object state);

        protected abstract int End(Socket socket, IAsyncResult ar);

    }

#endif

}
