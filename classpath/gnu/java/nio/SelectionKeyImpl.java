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

  public synchronized SelectionKey readyOps(int ops)
  {
    if (!isValid())
      throw new CancelledKeyException();
    
    readyOps = ops;
    return this;
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
