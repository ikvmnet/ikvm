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

public final class SelectorImpl extends AbstractSelector
{
  private static boolean debug = false;
  private Set keys;
  private Set selected;

  /**
   * A dummy object whose monitor regulates access to both our
   * selectThread and unhandledWakeup fields.
   */
  private Object wakeupMutex = new Object ();
  
  /**
   * Any thread that's currently blocked in a select operation.
   */
  private Socket notifySocket;
  
  /**
   * Indicates whether we have an unhandled wakeup call. This can
   * be due to either wakeup() triggering a thread interruption while
   * a thread was blocked in a select operation (in which case we need
   * to reset this thread's interrupt status after interrupting the
   * select), or else that no thread was on a select operation at the
   * time that wakeup() was called, in which case the following select()
   * operation should return immediately with nothing selected.
   */
  private volatile boolean unhandledWakeup;

  public SelectorImpl (SelectorProvider provider)
  {
    super (provider);
    
    keys = new HashSet ();
    selected = new HashSet ();
  }

  protected final void implCloseSelector()
    throws IOException
  {
    // Cancel any pending select operation.
    wakeup();
    
    synchronized (keys)
      {
        synchronized (selected)
          {
            synchronized (cancelledKeys ())
              {
                // FIXME: Release resources here.
              }
          }
      }
  }

  public final Set keys()
  {
    if (!isOpen())
      throw new ClosedSelectorException();

    return Collections.unmodifiableSet (keys);
  }
    
  public final int selectNow()
    throws IOException
  {
    // FIXME: We're simulating an immediate select
    // via a select with a timeout of one millisecond.
    return select (1);
  }

  public final int select()
    throws IOException
  {
    return select (0);
  }

  private final ArrayList getFDsAsArray(int ops)
  {
    ArrayList result = new ArrayList();
    Iterator it = keys.iterator();

    // Fill the array with the file descriptors
    while (it.hasNext())
      {
        SelectionKeyImpl key = (SelectionKeyImpl) it.next ();

        if ((key.interestOps () & ops) != 0)
          {
            result.Add(key.getSocket());
          }
      }

    return result;
  }

  public synchronized int select(long timeout)
    throws IOException
  {
    if (!isOpen())
      throw new ClosedSelectorException();
      
    synchronized (keys)
      {
        synchronized (selected)
          {
            deregisterCancelledKeys();

            // Set only keys with the needed interest ops into the arrays.
            ArrayList read = getFDsAsArray (SelectionKey.OP_READ
                                        | SelectionKey.OP_ACCEPT);
            ArrayList write = getFDsAsArray (SelectionKey.OP_WRITE
                                         | SelectionKey.OP_CONNECT);

            if (debug)
            {
                System.out.println("SelectorImpl.select: read.Count = " + read.get_Count() + ", write.Count = " + write.get_Count() + ", timeout = " + timeout);
            }

            // Test to see if we've got an unhandled wakeup call,
            // in which case we return immediately. Otherwise,
            // remember our current thread and jump into the select.
            // The monitor for dummy object wakeupMutex regulates
            // access to these fields.

            // FIXME: Not sure from the spec at what point we should
            // return "immediately". Is it here or immediately upon
            // entry to this function?
            
            // NOTE: There's a possibility of another thread calling
            // wakeup() immediately after our thread releases
            // wakeupMutex's monitor here, in which case we'll
            // do the select anyway. Since calls to wakeup() and select()
            // among different threads happen in non-deterministic order,
            // I don't think this is an issue.
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

            // Call the native select() on all file descriptors.
            int result = 0;
            try
              {
                begin();
                ArrayList savedReadList = read;
                ArrayList savedWriteList = write;
                if (timeout == 0)
                {
                    do
                    {
                        read = (ArrayList)savedReadList.Clone();
                        write = (ArrayList)savedWriteList.Clone();
                        // TODO we should probably select errors too
                        // TODO we should somehow support waking up in response to Thread.interrupt()
                        read.Add(notifySocket);
                        try
                        {
                            if (false) throw new SocketException();
                            // HACK we shouldn't be holding the keys lock while blocking
                            cli.System.Threading.Monitor.Exit(keys);
                            try
                            {
                                if (debug)
                                {
                                    System.out.println("SelectorImpl.select: Socket.Select");
                                }
                                Socket.Select(read, write, null, Integer.MAX_VALUE);
                            }
                            finally
                            {
                                cli.System.Threading.Monitor.Enter(keys);
                            }
                        }
                        catch(SocketException _)
                        {
                            if (debug)
                            {
                                System.out.println("SelectorImpl.select:");
                                _.printStackTrace();
                            }
                            read.Clear();
                            write.Clear();
                            purgeList(savedReadList);
                            purgeList(savedWriteList);
                        }
                    } while(read.get_Count() == 0 && write.get_Count() == 0 && !unhandledWakeup);
                    // TODO result should be set correctly
                    read.Remove(notifySocket);
                    result = read.get_Count() + write.get_Count();
                    if (debug)
                    {
                        System.out.println("SelectorImpl.select: result = " + result);
                    }
                }
                else
                {
                    do
                    {
                        read = (ArrayList)savedReadList.Clone();
                        write = (ArrayList)savedWriteList.Clone();
                        int microSeconds = 1000 * (int)Math.min(Integer.MAX_VALUE / 1000, timeout);
                        // TODO we should probably select errors too
                        // TODO we should somehow support waking up in response to Thread.interrupt()
                        read.Add(notifySocket);
                        try
                        {
                            if (false) throw new SocketException();
                            // HACK we shouldn't be holding the keys lock while blocking
                            cli.System.Threading.Monitor.Exit(keys);
                            try
                            {
                                Socket.Select(read, write, null, microSeconds);
                            }
                            finally
                            {
                                cli.System.Threading.Monitor.Enter(keys);
                            }
                            timeout -= microSeconds / 1000;
                        }
                        catch(SocketException _)
                        {
                            read.Clear();
                            write.Clear();
                            purgeList(savedReadList);
                            purgeList(savedWriteList);
                        }
                    } while(timeout > 0 && read.get_Count() == 0 && write.get_Count() == 0 && !unhandledWakeup);
                    // TODO result should be set correctly
                    read.Remove(notifySocket);
                    result = read.get_Count() + write.get_Count();
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

            while (it.hasNext ())
              {
                int ops = 0;
                SelectionKeyImpl key = (SelectionKeyImpl) it.next ();

                // If key is already selected retrieve old ready ops.
                if (selected.contains (key))
                  {
                    ops = key.readyOps ();
                  }

                // Set new ready read/accept ops
                for (int i = 0; i < read.get_Count(); i++)
                  {
                    if (key.getSocket() == read.get_Item(i))
                      {
                        if (key.channel () instanceof ServerSocketChannelImpl)
                          {
                            ops = ops | SelectionKey.OP_ACCEPT;
                          }
                        else
                          {
                            ops = ops | SelectionKey.OP_READ;
                          }
                      }
                  }

                // Set new ready write ops
                for (int i = 0; i < write.get_Count(); i++)
                  {
                    if (key.getSocket() == write.get_Item(i))
                      {
                        ops = ops | SelectionKey.OP_WRITE;

        //                 if (key.channel ().isConnected ())
        //                   {
        //                     ops = ops | SelectionKey.OP_WRITE;
        //                   }
        //                 else
        //                   {
        //                     ops = ops | SelectionKey.OP_CONNECT;
        //                   }
                     }
                  }

                // FIXME: We dont handle exceptional file descriptors yet.

                // If key is not yet selected add it.
                if (!selected.contains (key))
                  {
                    selected.add (key);
                  }

                // Set new ready ops
                key.readyOps (key.interestOps () & ops);
                if (debug)
                {
                    System.out.println("SelectorImpl.select: readyOps = " + (key.interestOps () & ops));
                }
              }
            deregisterCancelledKeys();
            
            return result;
          }
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
    Set ckeys = cancelledKeys ();
    synchronized (ckeys)
    {
      Iterator it = ckeys.iterator();

      while (it.hasNext ())
        {
          keys.remove ((SelectionKeyImpl) it.next ());
          it.remove ();
        }
    }
  }

  protected SelectionKey register (SelectableChannel ch, int ops, Object att)
  {
    return register ((AbstractSelectableChannel) ch, ops, att);
  }

  protected final SelectionKey register (AbstractSelectableChannel ch, int ops,
                                         Object att)
  {
    SelectionKeyImpl result;
    
    if (ch instanceof SocketChannelImpl)
      result = new SocketChannelSelectionKey (ch, this);
    else if (ch instanceof DatagramChannelImpl)
      result = new DatagramChannelSelectionKey (ch, this);
    else if (ch instanceof ServerSocketChannelImpl)
      result = new ServerSocketChannelSelectionKey (ch, this);
    else
      throw new InternalError ("No known channel type");

    synchronized (keys)
      {
        keys.add (result);
      }

    result.interestOps (ops);
    result.attach (att);
    return result;
  }
}
