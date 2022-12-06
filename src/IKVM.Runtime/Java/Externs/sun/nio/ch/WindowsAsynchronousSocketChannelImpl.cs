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

using IKVM.Runtime.Util.Sun.Nio.Ch;

namespace IKVM.Java.Externs.sun.nio.ch
{

    static class WindowsAsynchronousSocketChannelImpl
    {

#if !FIRST_PASS

        sealed class Connect : OperationBase<System.Net.IPEndPoint>
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

        static List<ArraySegment<byte>> ByteBuffersToList(global::java.nio.ByteBuffer[] bufs)
        {
            var list = new List<ArraySegment<byte>>(bufs.Length);
            foreach (var bb in bufs)
                list.Add(new ArraySegment<byte>(bb.array(), bb.arrayOffset() + bb.position(), bb.remaining()));

            return list;
        }

        sealed class Receive : OperationBase<global::java.nio.ByteBuffer[]>
        {

            protected override IAsyncResult Begin(System.Net.Sockets.Socket socket, global::java.nio.ByteBuffer[] bufs, AsyncCallback callback, object state)
            {
                return socket.BeginReceive(ByteBuffersToList(bufs), System.Net.Sockets.SocketFlags.None, callback, state);
            }

            protected override int End(System.Net.Sockets.Socket socket, IAsyncResult ar)
            {
                return socket.EndReceive(ar);
            }

        }

        sealed class Send : OperationBase<global::java.nio.ByteBuffer[]>
        {

            protected override IAsyncResult Begin(System.Net.Sockets.Socket socket, global::java.nio.ByteBuffer[] bufs, AsyncCallback callback, object state)
            {
                return socket.BeginSend(ByteBuffersToList(bufs), System.Net.Sockets.SocketFlags.None, callback, state);
            }

            protected override int End(System.Net.Sockets.Socket socket, IAsyncResult ar)
            {
                return socket.EndSend(ar);
            }

        }

#endif

        public static int connect0(global::java.io.FileDescriptor fd, bool preferIPv6, global::java.net.InetAddress remote, int remotePort, object handler)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            return new Connect().Do(fd.getSocket(), new System.Net.IPEndPoint(global::java.net.SocketUtil.getAddressFromInetAddress(remote, preferIPv6), remotePort), handler);
#endif
        }

        public static void updateConnectContext(global::java.io.FileDescriptor fd)
        {

        }

        public static int read0(global::java.io.FileDescriptor fd, global::java.nio.ByteBuffer[] bufs, object handler)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            return new Receive().Do(fd.getSocket(), bufs, handler);
#endif
        }

        public static int write0(global::java.io.FileDescriptor fd, global::java.nio.ByteBuffer[] bufs, object handler)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            return new Send().Do(fd.getSocket(), bufs, handler);
#endif
        }

        public static void shutdown0(long socket, int how)
        {

        }

        public static void closesocket0(long socket)
        {

        }

        public static void initIDs()
        {

        }

    }

}
