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

using System;
using System.Net.Sockets;

using static IKVM.Java.Externs.java.net.SocketImplUtil;

namespace IKVM.Java.Externs.java.net
{

    static class SocketOutputStream
    {

        /// <summary>
        /// Implements the native method for 'init'.
        /// </summary>
        public static void init()
        {

        }

        /// <summary>
        /// Implements the native method for 'socketWrite0'.
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="fd"></param>
        /// <param name="data"></param>
        /// <param name="off"></param>
        /// <param name="len"></param>
        /// <exception cref="global::java.net.SocketException"></exception>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        public static void socketWrite0(object this_, global::java.io.FileDescriptor fd, byte[] data, int off, int len)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (data == null)
                throw new global::java.lang.NullPointerException("data argument.");

            InvokeAction<global::java.net.SocketOutputStream>(this_, impl =>
            {
                InvokeActionWithSocket(fd, socket =>
                {
                    var prevBlocking = socket.Blocking;
                    var prevSendTimeout = socket.SendTimeout;

                    try
                    {
                        socket.Blocking = true;
                        socket.SendTimeout = 0;
                        socket.Send(data, off, Math.Min(len, data.Length - off), SocketFlags.None);
                    }
                    finally
                    {
                        socket.Blocking = prevBlocking;
                        socket.SendTimeout = prevSendTimeout;
                    }
                });
            });
#endif
        }

#if !FIRST_PASS

        /// <summary>
        /// Invokes the given action with the current socket, catching and mapping any resulting .NET exceptions.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        static void InvokeActionWithSocket(global::java.io.FileDescriptor fd, Action<Socket> action)
        {
            var socket = fd?.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed");

            action(socket);
        }

#endif

    }

}
