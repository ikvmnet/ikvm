/*
 * Copyright 2000-2005 Sun Microsystems, Inc.  All Rights Reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Sun designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Sun in the LICENSE file that accompanied this code.
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
 * Please contact Sun Microsystems, Inc., 4150 Network Circle, Santa Clara,
 * CA 95054 USA or visit www.sun.com if you need additional information or
 * have any questions.
 */

package sun.nio.ch;

import java.io.*;
import java.lang.reflect.*;
import java.net.*;
import java.nio.channels.*;


class Net {						// package-private

    private Net() { }


    // -- Miscellaneous utilities --

    static InetSocketAddress checkAddress(SocketAddress sa) {
	if (sa == null)
	    throw new IllegalArgumentException();
	if (!(sa instanceof InetSocketAddress))
	    throw new UnsupportedAddressTypeException(); // ## needs arg
	InetSocketAddress isa = (InetSocketAddress)sa;
	if (isa.isUnresolved())
	    throw new UnresolvedAddressException(); // ## needs arg
	return isa;
    }

    static InetSocketAddress asInetSocketAddress(SocketAddress sa) {
	if (!(sa instanceof InetSocketAddress))
	    throw new UnsupportedAddressTypeException();
	return (InetSocketAddress)sa;
    }

    static void translateToSocketException(Exception x)
	throws SocketException
    {
	if (x instanceof SocketException) 
	    throw (SocketException)x;
	Exception nx = x;
	if (x instanceof ClosedChannelException)
	    nx = new SocketException("Socket is closed");
	else if (x instanceof AlreadyBoundException)
	    nx = new SocketException("Already bound");
	else if (x instanceof NotYetBoundException)
	    nx = new SocketException("Socket is not bound yet");
	else if (x instanceof UnsupportedAddressTypeException)
	    nx = new SocketException("Unsupported address type");
	else if (x instanceof UnresolvedAddressException) {
	    nx = new SocketException("Unresolved address");
	}
	if (nx != x) 
	    nx.initCause(x);

	if (nx instanceof SocketException)
	    throw (SocketException)nx;
        else if (nx instanceof RuntimeException)
	    throw (RuntimeException)nx;
	else
	    throw new Error("Untranslated exception", nx);
    }

    static void translateException(Exception x,
				   boolean unknownHostForUnresolved)
	throws IOException
    {
	if (x instanceof IOException)
	    throw (IOException)x;
        // Throw UnknownHostException from here since it cannot
        // be thrown as a SocketException
	if (unknownHostForUnresolved &&
            (x instanceof UnresolvedAddressException))
        {
             throw new UnknownHostException();
        }
	translateToSocketException(x);
    }

    static void translateException(Exception x)
	throws IOException
    {
	translateException(x, false);
    }
}
