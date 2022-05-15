/*
 * Copyright (c) 2002, 2007, Oracle and/or its affiliates. All rights reserved.
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

// Parts Copyright (C) 2002-2007 Jeroen Frijters

package sun.nio.ch;

import cli.System.Net.Sockets.Socket;
import cli.System.Net.Sockets.SocketException;
import cli.System.Net.Sockets.AddressFamily;
import cli.System.Net.Sockets.SocketType;
import cli.System.Net.Sockets.ProtocolType;
import cli.System.Net.Sockets.SelectMode;
import cli.System.Collections.ArrayList;
import java.io.IOException;
import java.nio.channels.ClosedSelectorException;
import java.nio.channels.Pipe;
import java.nio.channels.SelectableChannel;
import java.nio.channels.SelectionKey;
import java.nio.channels.Selector;
import java.nio.channels.spi.AbstractSelectableChannel;
import java.nio.channels.spi.AbstractSelector;
import java.nio.channels.spi.SelectorProvider;
import java.util.Collection;
import java.util.Collections;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Iterator;
import java.util.Set;

final class DotNetSelectorImpl extends SelectorImpl
{
    private ArrayList channelArray = new ArrayList();
    private long updateCount = 0;

    //Pipe used as a wakeup object.
    private final Pipe wakeupPipe;

    // File descriptors corresponding to source and sink
    private final Socket wakeupSourceFd, wakeupSinkFd;

    // Lock for interrupt triggering and clearing
    private final Object interruptLock = new Object();
    private volatile boolean interruptTriggered = false;

    // class for fdMap entries
    private final static class MapEntry
    {
        SelectionKeyImpl ski;
        long updateCount = 0;
        long clearedCount = 0;
        MapEntry(SelectionKeyImpl ski)
        {
            this.ski = ski;
        }
    }
    private final HashMap<Socket, MapEntry> fdMap = new HashMap<Socket, MapEntry>();

    DotNetSelectorImpl(SelectorProvider sp) throws IOException
    {
        super(sp);
        wakeupPipe = Pipe.open();
        wakeupSourceFd = ((SelChImpl)wakeupPipe.source()).getFD().getSocket();

        // Disable the Nagle algorithm so that the wakeup is more immediate
        SinkChannelImpl sink = (SinkChannelImpl)wakeupPipe.sink();
        (sink.sc).socket().setTcpNoDelay(true);
        wakeupSinkFd = ((SelChImpl)sink).getFD().getSocket();
    }

    protected int doSelect(long timeout) throws IOException
    {
        if (channelArray == null)
            throw new ClosedSelectorException();
        processDeregisterQueue();
        if (interruptTriggered)
        {
            resetWakeupSocket();
            return 0;
        }

        ArrayList read = new ArrayList();
        ArrayList write = new ArrayList();
        ArrayList error = new ArrayList();
        for (int i = 0; i < channelArray.get_Count(); i++)
        {
            SelectionKeyImpl ski = (SelectionKeyImpl)channelArray.get_Item(i);
            int ops = ski.interestOps();
            if (ski.channel() instanceof SocketChannelImpl)
            {
                // TODO there's a race condition here...
                if (((SocketChannelImpl)ski.channel()).isConnected())
                {
                    ops &= SelectionKey.OP_READ | SelectionKey.OP_WRITE;
                }
                else
                {
                    ops &= SelectionKey.OP_CONNECT;
                }
            }
            if ((ops & (SelectionKey.OP_READ | SelectionKey.OP_ACCEPT)) != 0)
            {
                read.Add(ski.getSocket());
            }
            if ((ops & (SelectionKey.OP_WRITE | SelectionKey.OP_CONNECT)) != 0)
            {
                write.Add(ski.getSocket());
            }
            if ((ops & SelectionKey.OP_CONNECT) != 0)
            {
                error.Add(ski.getSocket());
            }
        }
        read.Add(wakeupSourceFd);
        try
        {
            begin();
            int microSeconds = 1000 * (int)Math.min(Integer.MAX_VALUE / 1000, timeout);
            try
            {
                if (false) throw new SocketException();
                // FXBUG docs say that -1 is infinite timeout, but that doesn't appear to work
                Socket.Select(read, write, error, timeout < 0 ? Integer.MAX_VALUE : microSeconds);
            }
            catch (SocketException _)
            {
                read.Clear();
                write.Clear();
                error.Clear();
            }
        }
        finally
        {
            end();
        }
        processDeregisterQueue();
        int updated = updateSelectedKeys(read, write, error);
        // Done with poll(). Set wakeupSocket to nonsignaled  for the next run.
        resetWakeupSocket();
        return updated;
    }

    private int updateSelectedKeys(ArrayList read, ArrayList write, ArrayList error)
    {
        updateCount++;
        int keys = processFDSet(updateCount, read, Net.POLLIN);
        keys += processFDSet(updateCount, write, Net.POLLCONN | Net.POLLOUT);
        keys += processFDSet(updateCount, error, Net.POLLIN | Net.POLLCONN | Net.POLLOUT);
        return keys;
    }

    private int processFDSet(long updateCount, ArrayList sockets, int rOps)
    {
        int numKeysUpdated = 0;
        for (int i = 0; i < sockets.get_Count(); i++)
        {
            Socket desc = (Socket)sockets.get_Item(i);
            if (desc == wakeupSourceFd)
            {
                synchronized (interruptLock)
                {
                    interruptTriggered = true;
                }
                continue;
            }
            MapEntry me = fdMap.get(desc);
            // If me is null, the key was deregistered in the previous
            // processDeregisterQueue.
            if (me == null)
                continue;
            SelectionKeyImpl sk = me.ski;
            if (selectedKeys.contains(sk))
            { // Key in selected set
                if (me.clearedCount != updateCount)
                {
                    if (sk.channel.translateAndSetReadyOps(rOps, sk) &&
                        (me.updateCount != updateCount))
                    {
                        me.updateCount = updateCount;
                        numKeysUpdated++;
                    }
                }
                else
                { // The readyOps have been set; now add
                    if (sk.channel.translateAndUpdateReadyOps(rOps, sk) &&
                        (me.updateCount != updateCount))
                    {
                        me.updateCount = updateCount;
                        numKeysUpdated++;
                    }
                }
                me.clearedCount = updateCount;
            }
            else
            { // Key is not in selected set yet
                if (me.clearedCount != updateCount)
                {
                    sk.channel.translateAndSetReadyOps(rOps, sk);
                    if ((sk.nioReadyOps() & sk.nioInterestOps()) != 0)
                    {
                        selectedKeys.add(sk);
                        me.updateCount = updateCount;
                        numKeysUpdated++;
                    }
                }
                else
                { // The readyOps have been set; now add
                    sk.channel.translateAndUpdateReadyOps(rOps, sk);
                    if ((sk.nioReadyOps() & sk.nioInterestOps()) != 0)
                    {
                        selectedKeys.add(sk);
                        me.updateCount = updateCount;
                        numKeysUpdated++;
                    }
                }
                me.clearedCount = updateCount;
            }
        }
        return numKeysUpdated;
    }

    protected void implClose() throws IOException
    {
        if (channelArray != null)
        {
            // prevent further wakeup
            synchronized (interruptLock) {
                interruptTriggered = true;
            }
            wakeupPipe.sink().close();
            wakeupPipe.source().close();
            for (int i = 0; i < channelArray.get_Count(); i++)
            { // Deregister channels
                SelectionKeyImpl ski = (SelectionKeyImpl)channelArray.get_Item(i);
                deregister(ski);
                SelectableChannel selch = ski.channel();
                if (!selch.isOpen() && !selch.isRegistered())
                    ((SelChImpl)selch).kill();
            }
            selectedKeys = null;
            channelArray = null;
        }
    }

    protected void implRegister(SelectionKeyImpl ski)
    {
        channelArray.Add(ski);
        fdMap.put(ski.getSocket(), new MapEntry(ski));
        keys.add(ski);
    }

    protected void implDereg(SelectionKeyImpl ski) throws IOException
    {
        channelArray.Remove(ski);
        fdMap.remove(ski.getSocket());
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
        synchronized (interruptLock)
        {
            if (!interruptTriggered)
            {
                setWakeupSocket();
                interruptTriggered = true;
            }
        }
        return this;
    }

    // Sets Windows wakeup socket to a signaled state.
    private void setWakeupSocket() {
        wakeupSinkFd.Send(new byte[1]);
    }

    // Sets Windows wakeup socket to a non-signaled state.
    private void resetWakeupSocket() {
        synchronized (interruptLock)
        {
            if (interruptTriggered == false)
                return;
            resetWakeupSocket0(wakeupSourceFd);
            interruptTriggered = false;
        }
    }

    private static void resetWakeupSocket0(Socket wakeupSourceFd)
    {
        while (wakeupSourceFd.get_Available() > 0)
        {
            wakeupSourceFd.Receive(new byte[1]);
        }
    }
}
