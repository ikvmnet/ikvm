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
package sun.nio.ch;

import java.nio.channels.CancelledKeyException;
import java.nio.channels.SelectableChannel;
import java.nio.channels.SelectionKey;
import java.nio.channels.Selector;
import java.nio.channels.spi.AbstractSelectionKey;

final class SelectionKeyImpl extends AbstractSelectionKey
{
    final SelChImpl channel;
    final SelectorImpl selector;
    private final cli.System.Net.Sockets.Socket socket;
    private int readyOps;
    private volatile int interestOps;

    SelectionKeyImpl(SelChImpl ch, SelectorImpl sel)
    {
        this.channel  = ch;
        this.selector = sel;
        socket = ch.getFD().getSocket();
    }

    public SelectableChannel channel()
    {
        return (SelectableChannel)channel;
    }

    public int readyOps()
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

        if ((ops & ~channel.validOps()) != 0)
            throw new IllegalArgumentException();
    
        interestOps = ops;
        return this;
    }
    
    public Selector selector()
    {
        return selector;
    }

    cli.System.Net.Sockets.Socket getSocket()
    {
        return socket;
    }

    void nioReadyOps(int ops)
    {
        readyOps = ops;
    }

    int nioReadyOps()
    {
        return readyOps;
    }

    int nioInterestOps()
    {
        return interestOps;
    }

    SelectionKey nioInterestOps(int ops)
    {
        if ((ops & ~channel().validOps()) != 0)
            throw new IllegalArgumentException();
        channel.translateAndSetInterestOps(ops, this);
        interestOps = ops;
        return this;
    }
}
