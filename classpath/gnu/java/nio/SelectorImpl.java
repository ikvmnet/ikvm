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
package gnu.java.nio;

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

public final class SelectorImpl extends AbstractSelector implements VMThread.InterruptProc
{
    private final HashSet keys = new HashSet();
    private final RemoveOnlyHashSet selected = new RemoveOnlyHashSet();
    private final Object wakeupMutex = new Object();
    private Socket notifySocket;
    private volatile boolean unhandledWakeup;

    private static final class RemoveOnlyHashSet extends HashSet
    {
        void addInternal(Object o)
        {
            super.add(o);
        }

        public boolean add(Object o)
        {
            throw new UnsupportedOperationException();
        }

        public boolean addAll(Collection c)
        {
            throw new UnsupportedOperationException();
        }
    }

    public SelectorImpl(SelectorProvider provider)
    {
        super(provider);
    }

    protected void implCloseSelector() throws IOException
    {
        // note that notifySocket gets closed by wakeup
        wakeup();
    }

    public void interrupt()
    {
        wakeup();
    }

    public Set keys()
    {
        if (!isOpen())
            throw new ClosedSelectorException();

        return Collections.unmodifiableSet(keys);
    }
    
    public Set selectedKeys()
    {
        if (!isOpen())
            throw new ClosedSelectorException();

        return selected;
    }

    public int selectNow() throws IOException
    {
        return selectImpl(0);
    }

    public int select() throws IOException
    {
        return selectImpl(Long.MAX_VALUE);
    }

    public int select(long timeout) throws IOException
    {
        if (timeout < 0)
            throw new IllegalArgumentException();

        if (timeout == 0)
            timeout = Long.MAX_VALUE;

        return selectImpl(timeout);
    }

    private synchronized int selectImpl(long timeout) throws IOException
    {
        if (!isOpen())
            throw new ClosedSelectorException();

        ArrayList read = new ArrayList();
        ArrayList write = new ArrayList();
        ArrayList error = new ArrayList();
        synchronized (keys)
        {
            Set cancelled = cancelledKeys();
            synchronized (cancelled)
            {
                synchronized (selected)
                {
                    for (Iterator it = keys.iterator(); it.hasNext();)
                    {
                        SelectionKeyImpl key = (SelectionKeyImpl)it.next();
                        if (cancelled.contains(key))
                        {
                            cancelled.remove(key);
                            selected.remove(key);
                            it.remove();
                            deregister(key);
                            continue;
                        }
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
                }
            }
        }

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

        try
        {
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
                    implSelect(read, write, error, microSeconds);
                    timeout -= microSeconds / 1000;
                }
                catch(SocketException _)
                {
                    read.Clear();
                    write.Clear();
                    error.Clear();
                    purgeList(savedReadList);
                    purgeList(savedWriteList);
                    purgeList(savedErrorList);
                }
            } while(timeout > 0
                && read.get_Count() == 0
                && write.get_Count() == 0
                && error.get_Count() == 0
                && !unhandledWakeup);
        }
        finally
        {
            unhandledWakeup = false;
        }

        int updatedCount = 0;
        synchronized (keys)
        {
            Set cancelled = cancelledKeys();
            synchronized (cancelled)
            {
                synchronized (selected)
                {
                    for (Iterator it = keys.iterator(); it.hasNext(); )
                    {
                        SelectionKeyImpl key = (SelectionKeyImpl)it.next();
                        if (cancelled.contains(key))
                        {
                            cancelled.remove(key);
                            selected.remove(key);
                            it.remove();
                            deregister(key);
                            continue;
                        }
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
                            if (selected.contains(key))
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
                                selected.addInternal(key);
                                updatedCount++;
                            }
                            if (key.channel() instanceof SocketChannelImpl)
                            {
                                SocketChannelImpl impl = (SocketChannelImpl)key.channel();
                                impl.selected();
                            }
                        }
                    }
                }
            }
        }
        return updatedCount;
    }

    private void implSelect(ArrayList read, ArrayList write, ArrayList error, int microSeconds) throws SocketException
    {
        try
        {
            VMThread.enterInterruptableWait(this);
            try
            {
                Socket.Select(read, write, error, microSeconds);
            }
            finally
            {
                VMThread.leaveInterruptableWait();
            }
        }
        catch (InterruptedException _)
        {
            wakeup();
            Thread.currentThread().interrupt();
        }
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

    private static Socket createNotifySocket()
    {
        return new Socket(AddressFamily.wrap(AddressFamily.InterNetwork),
            SocketType.wrap(SocketType.Dgram),
            ProtocolType.wrap(ProtocolType.Udp));
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

    protected SelectionKey register(AbstractSelectableChannel ch, int ops, Object att)
    {
        SelectionKeyImpl result;

        if (ch instanceof SocketChannelImpl)
            result = new SelectionKeyImpl(ch, this, ((SocketChannelImpl)ch).getSocket());
        else if (ch instanceof DatagramChannelImpl)
            result = new SelectionKeyImpl(ch, this, ((DatagramChannelImpl)ch).getSocket());
        else if (ch instanceof ServerSocketChannelImpl)
            result = new SelectionKeyImpl(ch, this, ((ServerSocketChannelImpl)ch).getSocket());
        else if (ch instanceof PipeImpl.SourceChannelImpl)
            result = new SelectionKeyImpl(ch, this, ((PipeImpl.SourceChannelImpl)ch).getSocket());
        else if (ch instanceof PipeImpl.SinkChannelImpl)
            result = new SelectionKeyImpl(ch, this, ((PipeImpl.SinkChannelImpl)ch).getSocket());
        else
            throw new InternalError("No known channel type");

        result.interestOps(ops);
        result.attach(att);

        synchronized (keys)
        {
            keys.add(result);
        }
        return result;
    }
}
