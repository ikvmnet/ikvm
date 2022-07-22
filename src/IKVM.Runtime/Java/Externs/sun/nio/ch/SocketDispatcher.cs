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

using ByteBuffer = java.nio.ByteBuffer;
using FileDescriptor = java.io.FileDescriptor;

namespace IKVM.Java.Externs.sun.nio.ch
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

}
