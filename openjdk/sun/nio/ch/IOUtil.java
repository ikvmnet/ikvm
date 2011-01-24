/*
 * Copyright (c) 2000, 2002, Oracle and/or its affiliates. All rights reserved.
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

class IOUtil {

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
        Net.configureBlocking(fd, blocking);
    }

}
