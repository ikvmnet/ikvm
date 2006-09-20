package gnu.java.nio;

import java.nio.channels.CancelledKeyException;
import java.nio.channels.SelectableChannel;
import java.nio.channels.SelectionKey;
import java.nio.channels.Selector;
import java.nio.channels.spi.AbstractSelectionKey;

final class SelectionKeyImpl extends AbstractSelectionKey
{
  private int readyOps;
  private int interestOps;
  private SelectorImpl impl;
  private SelectableChannel ch;
  private cli.System.Net.Sockets.Socket socket;

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

  public int readyOps()
  {
    if (!isValid())
      throw new CancelledKeyException();
    
    return readyOps;
  }

  public SelectionKey readyOps(int ops)
  {
    if (!isValid())
      throw new CancelledKeyException();
    
    readyOps = ops;
    return this;
  }

  public int interestOps()
  {
    if (!isValid())
      throw new CancelledKeyException();
    
    return interestOps;    
  }

  public SelectionKey interestOps(int ops)
  {
    if (!isValid())
      throw new CancelledKeyException();
    
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
