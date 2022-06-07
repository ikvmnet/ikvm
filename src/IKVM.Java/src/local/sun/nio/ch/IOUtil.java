/*
 * Copyright (c) 2000, 2013, Oracle and/or its affiliates. All rights reserved.
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

package sun.nio.ch;

import java.io.FileDescriptor;
import java.io.IOException;
import java.net.*;
import java.nio.ByteBuffer;
import java.nio.channels.*;
import java.nio.channels.spi.*;


/**
 * File-descriptor based I/O utilities that are shared by NIO classes.
 */

public class IOUtil {

    private IOUtil() { }                // No instantiation

    static boolean randomBytes(byte[] someBytes)
    {
        try
        {
            if (false) throw new cli.System.Security.Cryptography.CryptographicException();
            cli.System.Security.Cryptography.RNGCryptoServiceProvider csp = new cli.System.Security.Cryptography.RNGCryptoServiceProvider();
            csp.GetBytes(someBytes);
            return true;
        }
        catch (cli.System.Security.Cryptography.CryptographicException _)
        {
            return false;
        }
    }

    static void configureBlocking(FileDescriptor fd, boolean blocking) throws IOException
    {
        fd.setSocketBlocking(blocking);
    }

    // this is a dummy method to allow us to use unmodified socket channel impls
    static int fdVal(FileDescriptor fd)
    {
        return 0xbadc0de;
    }

    static int read(FileDescriptor fd, ByteBuffer dst, long position,
                    NativeDispatcher nd)
        throws IOException
    {
        if (dst.isReadOnly())
            throw new IllegalArgumentException("Read-only buffer");

        if (position != -1)
        {
            long prevpos = fd.getFilePointer();
            try
            {
                fd.seek(position);
                return read(fd, dst, -1, nd);
            }
            finally
            {
                fd.seek(prevpos);
            }
        }

        if (dst.hasArray())
        {
            byte[] buf = dst.array();
            int len = nd.read(fd, buf, dst.arrayOffset() + dst.position(), dst.remaining());
            if (len > 0)
            {
                dst.position(dst.position() + len);
            }
            return len;
        }
        else
        {
            byte[] buf = new byte[dst.remaining()];
            int len = nd.read(fd, buf, 0, buf.length);
            if (len > 0)
            {
                dst.put(buf, 0, len);
            }
            return len;
        }
    }

    static long read(FileDescriptor fd, ByteBuffer[] bufs, int offset, int length, NativeDispatcher nd)
        throws IOException
    {
        return nd.read(fd, bufs, offset, length);
    }

    static int write(FileDescriptor fd, ByteBuffer src, long position,
                     NativeDispatcher nd)
        throws IOException
    {
        if (position != -1)
        {
            long prevpos = fd.getFilePointer();
            try
            {
                fd.seek(position);
                return write(fd, src, -1, nd);
            }
            finally
            {
                fd.seek(prevpos);
            }
        }

        if (src.hasArray())
        {
            byte[] buf = src.array();
            int len = nd.write(fd, buf, src.arrayOffset() + src.position(), src.remaining());
            if (len > 0)
            {
                src.position(src.position() + len);
            }
            return len;
        }
        else
        {
            int pos = src.position();
            byte[] buf = new byte[src.remaining()];
            src.get(buf);
            src.position(pos);
            int len = nd.write(fd, buf, 0, buf.length);
            if (len > 0)
            {
                src.position(pos + len);
            }
            return len;
        }
    }

    static long write(FileDescriptor fd, ByteBuffer[] bufs, NativeDispatcher nd)
        throws IOException
    {
        return nd.write(fd, bufs, 0, bufs.length);
    }

    static long write(FileDescriptor fd, ByteBuffer[] bufs, int offset, int length, NativeDispatcher nd)
        throws IOException
    {
        return nd.write(fd, bufs, offset, length);
    }

    /**
     * Used to trigger loading of native libraries
     */
    public static void load() { }
}
