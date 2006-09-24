/*
  Copyright (C) 2002, 2003, 2004, 2005, 2006 Jeroen Frijters

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
package gnu.java.nio;

import java.nio.channels.CancelledKeyException;
import java.nio.channels.SelectableChannel;
import java.nio.channels.SelectionKey;
import java.nio.channels.Selector;
import java.nio.channels.spi.AbstractSelectionKey;

final class SelectionKeyImpl extends AbstractSelectionKey
{
    private final SelectableChannel ch;
    private final SelectorImpl impl;
    private final cli.System.Net.Sockets.Socket socket;
    private int readyOps;
    private int interestOps;
    int savedInterestOps; // used by SelectorImpl to store interestOps at the begin of a select()

    SelectionKeyImpl(SelectableChannel ch, SelectorImpl impl, cli.System.Net.Sockets.Socket socket)
    {
        this.ch  = ch;
        this.impl = impl;
        this.socket = socket;
    }

    public SelectableChannel channel()
    {
        return ch;
    }

    public synchronized int readyOps()
    {
        if (!isValid())
            throw new CancelledKeyException();
    
        return readyOps;
    }

    void readyOps(int ops)
    {
        readyOps = ops;
    }

    public synchronized int interestOps()
    {
        if (!isValid())
            throw new CancelledKeyException();
    
        return interestOps;    
    }

    public synchronized SelectionKey interestOps(int ops)
    {
        if (!isValid())
            throw new CancelledKeyException();

        if ((ops & ~ch.validOps()) != 0)
            throw new IllegalArgumentException();
    
        interestOps = ops;
        return this;
    }
    
    public Selector selector()
    {
        return impl;
    }

    cli.System.Net.Sockets.Socket getSocket()
    {
        return socket;
    }
}
