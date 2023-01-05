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

using IKVM.Runtime.Util.Sun.Nio.Ch;

namespace IKVM.Java.Externs.sun.nio.ch
{

    static class WindowsAsynchronousServerSocketChannelImpl
    {

#if !FIRST_PASS

        sealed class Accept : OperationBase<System.Net.Sockets.Socket>
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

        public static int accept0(global::java.io.FileDescriptor listenSocket, global::java.io.FileDescriptor acceptSocket, object handler)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            return new Accept().Do(listenSocket.getSocket(), acceptSocket.getSocket(), handler);
#endif
        }

        public static void updateAcceptContext(global::java.io.FileDescriptor listenSocket, global::java.io.FileDescriptor acceptSocket)
        {

        }

        public static void closesocket0(long socket)
        {

        }

    }

}
