package gnu.java.nio;

import ikvm.internal.Util;
import java.io.IOException;
import java.nio.ByteBuffer;
import java.nio.channels.Pipe;
import java.nio.channels.spi.SelectorProvider;

final class PipeImpl extends Pipe
{
    private final Pipe.SourceChannel source;
    private final Pipe.SinkChannel sink;
  
    PipeImpl(SelectorProvider provider) throws IOException
    {
        source = new Pipe.SourceChannel(provider)
        {
            protected void implCloseSelectableChannel() throws IOException
            {
                throw new Error("Not implemented");
            }

            protected void implConfigureBlocking(boolean blocking) throws IOException
            {
                throw new Error("Not implemented");
            }

            public int read(ByteBuffer src) throws IOException
            {
                throw new Error("Not implemented");
            }

            public long read(ByteBuffer[] srcs) throws IOException
            {
                throw new Error("Not implemented");
            }

            public long read(ByteBuffer[] srcs, int offset, int length) throws IOException
            {
                if (!Util.rangeCheck(srcs.length, offset, length))
                    throw new IndexOutOfBoundsException();

                throw new Error("Not implemented");
            }
        };
        sink = new Pipe.SinkChannel(provider)
        {
            protected void implCloseSelectableChannel() throws IOException
            {
                throw new Error("Not implemented");
            }

            protected void implConfigureBlocking(boolean blocking) throws IOException
            {
                throw new Error("Not implemented");
            }

            public int write(ByteBuffer dst) throws IOException
            {
                throw new Error("Not implemented");
            }

            public long write(ByteBuffer[] dst) throws IOException
            {
                throw new Error("Not implemented");
            }

            public long write(ByteBuffer[] dsts, int offset, int length) throws IOException
            {
                if (!Util.rangeCheck(dsts.length, offset, length))
                    throw new IndexOutOfBoundsException();
      
                throw new Error("Not implemented");
            }
        };
    }

    public Pipe.SinkChannel sink()
    {
        return sink;
    }

    public Pipe.SourceChannel source()
    {
        return source;
    }
}
