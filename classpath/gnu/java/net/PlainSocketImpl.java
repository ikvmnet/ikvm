package gnu.java.net;

public class PlainSocketImpl extends java.net.PlainSocketImpl
{
    /**
     * Indicates whether a channel initiated whatever operation
     * is being invoked on this socket.
     */
    private boolean inChannelOperation;

    /**
     * Indicates whether we should ignore whether any associated
     * channel is set to non-blocking mode. Certain operations
     * throw an <code>IllegalBlockingModeException</code> if the
     * associated channel is in non-blocking mode, <i>except</i>
     * if the operation is invoked by the channel itself.
     */
    public final boolean isInChannelOperation()
    {
	return inChannelOperation;
    }
  
    /**
     * Sets our indicator of whether an I/O operation is being
     * initiated by a channel.
     */
    public final void setInChannelOperation(boolean b)
    {
	inChannelOperation = b;
    }
    
    public int getNativeFD() { throw new NoSuchMethodError("Not supported"); }
}
