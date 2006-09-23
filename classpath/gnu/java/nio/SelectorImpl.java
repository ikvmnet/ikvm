/* SelectorImpl.java -- 
   Copyright (C) 2002, 2003, 2004  Free Software Foundation, Inc.

This file is part of GNU Classpath.

GNU Classpath is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2, or (at your option)
any later version.

GNU Classpath is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
General Public License for more details.

You should have received a copy of the GNU General Public License
along with GNU Classpath; see the file COPYING.  If not, write to the
Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA
02111-1307 USA.

Linking this library statically or dynamically with other modules is
making a combined work based on this library.  Thus, the terms and
conditions of the GNU General Public License cover the whole
combination.

As a special exception, the copyright holders of this library give you
permission to link this library with independent modules to produce an
executable, regardless of the license terms of these independent
modules, and to copy and distribute the resulting executable under
terms of your choice, provided that you also meet, for each linked
independent module, the terms and conditions of the license of that
module.  An independent module is a module which is not derived from
or based on this library.  If you modify this library, you may extend
this exception to your version of the library, but you are not
obligated to do so.  If you do not wish to do so, delete this
exception statement from your version. */


package gnu.java.nio;

import java.io.IOException;
import java.nio.channels.ClosedSelectorException;
import java.nio.channels.SelectableChannel;
import java.nio.channels.SelectionKey;
import java.nio.channels.Selector;
import java.nio.channels.spi.AbstractSelectableChannel;
import java.nio.channels.spi.AbstractSelector;
import java.nio.channels.spi.SelectorProvider;
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
    private static boolean debug = false;
    private final HashSet keys = new HashSet();
    private final HashSet selected = new HashSet();
    private final Object wakeupMutex = new Object();
    private Socket notifySocket;
    private volatile boolean unhandledWakeup;

    public SelectorImpl(SelectorProvider provider)
    {
        super(provider);
    }

    protected void implCloseSelector() throws IOException
    {
        // Cancel any pending select operation.
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
    
    public int selectNow() throws IOException
    {
        // FIXME: We're simulating an immediate select
        // via a select with a timeout of one millisecond.
        return select(1);
    }

    public int select() throws IOException
    {
        return select(0);
    }

    private ArrayList getSocketArray(int ops)
    {
        ArrayList result = new ArrayList();
        Iterator it = keys.iterator();

        // Fill the array with the file descriptors
        while (it.hasNext())
        {
            SelectionKeyImpl key = (SelectionKeyImpl)it.next();

            int validOps = ops & key.channel().validOps();

            if ((key.interestOps() & validOps) != 0 && (key.readyOps() & validOps) != validOps)
            {
                result.Add(key.getSocket());
            }
        }

        return result;
    }

    public synchronized int select(long timeout) throws IOException
    {
        if (!isOpen())
            throw new ClosedSelectorException();

        synchronized (keys)
        {
            synchronized (selected)
            {
                deregisterCancelledKeys();

                // Set only keys with the needed interest ops into the arrays.
                ArrayList read = getSocketArray(SelectionKey.OP_READ | SelectionKey.OP_ACCEPT);
                ArrayList write = getSocketArray(SelectionKey.OP_WRITE | SelectionKey.OP_CONNECT);
                ArrayList error = getSocketArray(SelectionKey.OP_CONNECT);

                if (debug)
                {
                    System.out.println("SelectorImpl.select: read.Count = " + read.get_Count() + ", write.Count = " + write.get_Count() + ", error.Count = " + error.get_Count() + ", timeout = " + timeout);
                }

                synchronized (wakeupMutex)
                {
                    if (unhandledWakeup)
                    {
                        if (debug)
                        {
                            System.out.println("SelectorImpl.select: returning due to unhandled wakeup");
                        }
                        unhandledWakeup = false;
                        return 0;
                    }
                    else
                    {
                        notifySocket = createNotifySocket();
                    }
                }

                try
                {
                    begin();
                    ArrayList savedReadList = read;
                    ArrayList savedWriteList = write;
                    ArrayList savedErrorList = error;
                    if (timeout == 0)
                    {
                        do
                        {
                            read = (ArrayList)savedReadList.Clone();
                            write = (ArrayList)savedWriteList.Clone();
                            error = (ArrayList)savedErrorList.Clone();
                            read.Add(notifySocket);
                            try
                            {
                                implSelect(read, write, error, Integer.MAX_VALUE);
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
                        } while(read.get_Count() == 0 && write.get_Count() == 0 && error.get_Count() == 0 && !unhandledWakeup);
                        read.Remove(notifySocket);
                    }
                    else
                    {
                        do
                        {
                            read = (ArrayList)savedReadList.Clone();
                            write = (ArrayList)savedWriteList.Clone();
                            error = (ArrayList)savedErrorList.Clone();
                            int microSeconds = 1000 * (int)Math.min(Integer.MAX_VALUE / 1000, timeout);
                            read.Add(notifySocket);
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
                        } while(timeout > 0 && read.get_Count() == 0 && write.get_Count() == 0 && error.get_Count() == 0 && !unhandledWakeup);
                        read.Remove(notifySocket);
                    }
                }
                finally
                {
                    end();
                    synchronized (wakeupMutex)
                    {
                        unhandledWakeup = false;
                        notifySocket.Close();
                        notifySocket = null;
                    }
                }

                Iterator it = keys.iterator ();

                int updatedCount = 0;
                while (it.hasNext())
                {
                    int ops = 0;
                    SelectionKeyImpl key = (SelectionKeyImpl)it.next();

                    for (int i = 0; i < error.get_Count(); i++)
                    {
                        if (key.getSocket() == error.get_Item(i))
                        {
                            ops |= SelectionKey.OP_CONNECT;
                            break;
                        }
                    }

                    for (int i = 0; i < read.get_Count(); i++)
                    {
                        if (key.getSocket() == read.get_Item(i))
                        {
                            if (key.channel() instanceof ServerSocketChannelImpl)
                            {
                                ops |= SelectionKey.OP_ACCEPT;
                            }
                            else
                            {
                                ops |= SelectionKey.OP_READ;
                            }
                            break;
                        }
                    }

                    for (int i = 0; i < write.get_Count(); i++)
                    {
                        if (key.getSocket() == write.get_Item(i))
                        {
                            if (key.channel() instanceof SocketChannelImpl)
                            {
                                SocketChannelImpl impl = (SocketChannelImpl)key.channel();
                                if (impl.isConnected())
                                {
                                    ops |= SelectionKey.OP_WRITE;
                                }
                                else
                                {
                                    ops |= SelectionKey.OP_CONNECT;
                                }
                            }
                            else
                            {
                                ops |= SelectionKey.OP_WRITE;
                            }
                            break;
                        }
                    }

                    ops &= key.interestOps();

                    if ((key.readyOps() & ops) != ops)
                    {
                        updatedCount++;
                        // Set new ready ops
                        key.readyOps(key.readyOps() | ops);
                        if (debug)
                        {
                            System.out.println("SelectorImpl.select: readyOps = " + (key.interestOps () & ops));
                        }
                        if (key.channel() instanceof SocketChannelImpl)
                        {
                            SocketChannelImpl impl = (SocketChannelImpl)key.channel();
                            impl.selected();
                        }
                        // If key is not yet selected add it.
                        if (!selected.contains(key))
                        {
                            selected.add(key);
                        }
                    }
                }
                deregisterCancelledKeys();
            
                return updatedCount;
            }
        }
    }

    private void implSelect(ArrayList read, ArrayList write, ArrayList error, int microSeconds) throws SocketException
    {
        // HACK we shouldn't be holding the keys lock while blocking
        cli.System.Threading.Monitor.Exit(keys);
        try
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
        finally
        {
            cli.System.Threading.Monitor.Enter(keys);
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
    
    public final Set selectedKeys()
    {
        if (!isOpen())
            throw new ClosedSelectorException();

        return selected;
    }

    public final Selector wakeup()
    {
        synchronized (wakeupMutex)
        {
            unhandledWakeup = true;
        
            if (notifySocket != null)
                notifySocket.Close();
        }
      
        return this;
    }

    private final void deregisterCancelledKeys()
    {
        Set ckeys = cancelledKeys();
        synchronized (ckeys)
        {
            Iterator it = ckeys.iterator();

            while (it.hasNext())
            {
                keys.remove((SelectionKeyImpl)it.next());
                it.remove();
            }
        }
    }

    protected SelectionKey register(SelectableChannel ch, int ops, Object att)
    {
        return register((AbstractSelectableChannel)ch, ops, att);
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
        else
            throw new InternalError ("No known channel type");

        result.interestOps(ops);
        result.attach(att);

        synchronized (keys)
        {
            keys.add (result);
        }

        return result;
    }
}
