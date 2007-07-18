/*
  Copyright (C) 2002, 2003, 2004, 2005, 2006, 2007 Jeroen Frijters

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

import java.io.IOException;
import java.nio.channels.ClosedSelectorException;
import java.nio.channels.SelectableChannel;
import java.nio.channels.SelectionKey;
import java.nio.channels.Selector;
import java.nio.channels.spi.AbstractSelectableChannel;
import java.nio.channels.spi.AbstractSelector;
import java.nio.channels.spi.SelectorProvider;
import java.util.Collection;
import java.util.Collections;
import java.util.HashSet;
import java.util.Iterator;
import java.util.Set;
import cli.System.Net.Sockets.Socket;
import cli.System.Net.Sockets.SocketException;
import cli.System.Net.Sockets.AddressFamily;
import cli.System.Net.Sockets.SocketType;
import cli.System.Net.Sockets.ProtocolType;
import cli.System.Net.Sockets.SelectMode;
import cli.System.Collections.ArrayList;

final class DotNetSelectorImpl extends SelectorImpl
{
    private final Object wakeupMutex = new Object();
    private Socket notifySocket;
    private volatile boolean unhandledWakeup;

    DotNetSelectorImpl(SelectorProvider sp)
    {
	super(sp);
    }

    protected int doSelect(long timeout) throws IOException
    {
	processDeregisterQueue();

	ArrayList read = new ArrayList();
	ArrayList write = new ArrayList();
	ArrayList error = new ArrayList();

	synchronized (wakeupMutex)
	{
	    if (unhandledWakeup)
	    {
		unhandledWakeup = false;
		return 0;
	    }
	    if (notifySocket == null)
	    {
		notifySocket = createNotifySocket();
	    }
	    read.Add(notifySocket);
	}

	for (Iterator it = keys.iterator(); it.hasNext(); )
	{
	    SelectionKeyImpl key = (SelectionKeyImpl)it.next();
	    int ops = key.interestOps();
	    if (key.channel() instanceof SocketChannelImpl)
	    {
		// TODO there's a race condition here...
		if (((SocketChannelImpl)key.channel()).isConnected())
		{
		    ops &= SelectionKey.OP_READ | SelectionKey.OP_WRITE;
		}
		else
		{
		    ops &= SelectionKey.OP_CONNECT;
		}
	    }
	    key.savedInterestOps = ops;
	    if ((ops & (SelectionKey.OP_READ | SelectionKey.OP_ACCEPT)) != 0)
	    {
		read.Add(key.getSocket());
	    }
	    if ((ops & (SelectionKey.OP_WRITE | SelectionKey.OP_CONNECT)) != 0)
	    {
		write.Add(key.getSocket());
	    }
	    if ((ops & SelectionKey.OP_CONNECT) != 0)
	    {
		error.Add(key.getSocket());
	    }
	}
	try
	{
	    begin();
	    ArrayList savedReadList = read;
	    ArrayList savedWriteList = write;
	    ArrayList savedErrorList = error;
	    do
	    {
		read = (ArrayList)savedReadList.Clone();
		write = (ArrayList)savedWriteList.Clone();
		error = (ArrayList)savedErrorList.Clone();
		int microSeconds = 1000 * (int)Math.min(Integer.MAX_VALUE / 1000, timeout);
		try
		{
		    if (false) throw new SocketException();
		    Socket.Select(read, write, error, microSeconds);
		    timeout -= microSeconds / 1000;
		}
		catch (SocketException _)
		{
		    read.Clear();
		    write.Clear();
		    error.Clear();
		    purgeList(savedReadList);
		    purgeList(savedWriteList);
		    purgeList(savedErrorList);
		}
	    } while (timeout > 0
		&& read.get_Count() == 0
		&& write.get_Count() == 0
		&& error.get_Count() == 0
		&& !unhandledWakeup);
	}
	finally
	{
	    end();
	    unhandledWakeup = false;
	}
	processDeregisterQueue();
	int updatedCount = 0;
	for (Iterator it = keys.iterator(); it.hasNext(); )
	{
	    SelectionKeyImpl key = (SelectionKeyImpl)it.next();
	    int ops = 0;
	    Socket socket = key.getSocket();
	    if (error.Contains(socket))
	    {
		ops |= SelectionKey.OP_CONNECT;
	    }
	    if (read.Contains(socket))
	    {
		ops |= SelectionKey.OP_ACCEPT | SelectionKey.OP_READ;
	    }
	    if (write.Contains(socket))
	    {
		ops |= SelectionKey.OP_CONNECT | SelectionKey.OP_WRITE;
	    }
	    ops &= key.savedInterestOps;
	    if (ops != 0)
	    {
		if (selectedKeys.contains(key))
		{
		    int ready = key.readyOps();
		    if ((ready & ops) != ops)
		    {
			updatedCount++;
		    }
		    key.readyOps(ready | ops);
		}
		else
		{
		    key.readyOps(ops);
		    selectedKeys.add(key);
		    updatedCount++;
		}
	    }
	}
	return updatedCount;
    }

    private static void purgeList(ArrayList list)
    {
	for (int i = 0; i < list.get_Count(); i++)
	{
	    Socket s = (Socket)list.get_Item(i);
	    try
	    {
		if (false) throw new cli.System.ObjectDisposedException("");
		s.Poll(0, SelectMode.wrap(SelectMode.SelectError));
	    }
	    catch (cli.System.ObjectDisposedException _)
	    {
		list.RemoveAt(i);
		i--;
	    }
	}
    }

    protected void implClose() throws IOException
    {
	wakeup();
    }

    protected void implRegister(SelectionKeyImpl ski)
    {
	keys.add(ski);
    }

    protected void implDereg(SelectionKeyImpl ski) throws IOException
    {
	keys.remove(ski);
	selectedKeys.remove(ski);
	deregister(ski);
	SelectableChannel selch = ski.channel();
	if (!selch.isOpen() && !selch.isRegistered())
	{
	    ((SelChImpl)selch).kill();
	}
    }

    public Selector wakeup()
    {
	synchronized (wakeupMutex)
	{
	    unhandledWakeup = true;

	    if (notifySocket != null)
	    {
		notifySocket.Close();
		notifySocket = null;
	    }
	}
	return this;
    }

    private static Socket createNotifySocket()
    {
	return new Socket(AddressFamily.wrap(AddressFamily.InterNetwork),
	    SocketType.wrap(SocketType.Dgram),
	    ProtocolType.wrap(ProtocolType.Udp));
    }
}
