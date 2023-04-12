using java.io;
using java.lang;
using java.net;
using java.nio;
using java.nio.channels;
using java.util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.nio.channels;

[TestClass]
public class AsynchronousSocketChannelTests
{
    static readonly Random rand = new Random();

    public TestContext TestContext { get; set; }

    [TestMethod]
    public void TestStressLoopback()
    {
        // setup listener
        AsynchronousServerSocketChannel listener =
            AsynchronousServerSocketChannel.open().bind(new InetSocketAddress(0));
        int port = ((InetSocketAddress)(listener.getLocalAddress())).getPort();
        InetAddress lh = InetAddress.getLocalHost();
        SocketAddress remote = new InetSocketAddress(lh, port);

        // create sources and sinks
        int count = 1;// 2 + rand.nextInt(9);
        Source[] source = new Source[count];
        Sink[] sink = new Sink[count];
        for (int i = 0; i < count; i++)
        {
            AsynchronousSocketChannel ch = AsynchronousSocketChannel.open();
            ch.connect(remote).get();
            source[i] = new Source(ch);
            sink[i] = new Sink((AsynchronousByteChannel)listener.accept().get());
        }

        // start the sinks and sources
        for (int i = 0; i < count; i++)
        {
            sink[i].start();
            source[i].start();
        }

        // let the test run for a while
        Thread.sleep(20 * 1000);

        // wait until everyone is done
        bool failed = false;
        long total = 0L;
        for (int i = 0; i < count; i++)
        {
            long nwrote = source[i].finish();
            long nread = sink[i].finish();
            if (nread != nwrote)
                failed = true;
            TestContext.WriteLine($"{nwrote} -> {nread} ({((failed) ? "FAIL" : "PASS")})");
            total += nwrote;
        }
        if (failed)
            throw new RuntimeException("Test failed - see log for details");
        TestContext.WriteLine($"Total sent {total / (1024L * 1024L)} MB");
    }

    /**
     * Writes bytes to a channel until "done". When done the channel is closed.
     */
    sealed class Source
    {
        private readonly AsynchronousByteChannel channel;
        private readonly ByteBuffer sentBuffer;
        private long bytesSent;
        private bool finished;

        internal Source(AsynchronousByteChannel channel)
        {
            this.channel = channel;
            int size = 1024 + rand.nextInt(10000);
            this.sentBuffer = (rand.nextBoolean()) ?
                ByteBuffer.allocateDirect(size) : ByteBuffer.allocate(size);
        }

        internal void start()
        {
            sentBuffer.position(0);
            sentBuffer.limit(sentBuffer.capacity());
            channel.write(sentBuffer, null, new Completion(this));
        }

        internal long finish()
        {
            finished = true;
            waitUntilClosed(channel);
            return bytesSent;
        }

        private class Completion : CompletionHandler
        {
            private readonly Source @this;

            public Completion(Source @this)
            {
                this.@this = @this;
            }

            public void completed(Integer nwrote, object _)
            {
                @this.bytesSent += nwrote.intValue();
                if (@this.finished)
                {
                    closeUnchecked(@this.channel);
                }
                else
                {
                    @this.sentBuffer.position(0);
                    @this.sentBuffer.limit(@this.sentBuffer.capacity());
                    @this.channel.write(@this.sentBuffer, null, this);
                }
            }

            void CompletionHandler.completed(object result, object attachment) => completed(result as Integer, attachment);

            public void failed(System.Exception exc, object attachment)
            {
                (exc as Throwable)?.printStackTrace();
                closeUnchecked(@this.channel);
            }
        }
    }

    /**
     * Read bytes from a channel until EOF is received.
     */
    sealed class Sink
    {
        private readonly AsynchronousByteChannel channel;
        private readonly ByteBuffer readBuffer;
        private long bytesRead;

        internal Sink(AsynchronousByteChannel channel)
        {
            this.channel = channel;
            int size = 1024 + rand.nextInt(10000);
            this.readBuffer = (rand.nextBoolean()) ?
                ByteBuffer.allocateDirect(size) : ByteBuffer.allocate(size);
        }

        internal void start()
        {
            channel.read(readBuffer, null, new Completion(this));
        }

        internal long finish()
        {
            waitUntilClosed(channel);
            return bytesRead;
        }

        private class Completion : CompletionHandler
        {
            private readonly Sink @this;

            public Completion(Sink @this)
            {
                this.@this = @this;
            }

            public void completed(Integer nread, object _)
            {
                if (nread.intValue() < 0)
                {
                    closeUnchecked(@this.channel);
                }
                else
                {
                    @this.bytesRead += nread.intValue();
                    @this.readBuffer.clear();
                    @this.channel.read(@this.readBuffer, null, this);
                }
            }

            void CompletionHandler.completed(object result, object attachment) => completed(result as Integer, attachment);

            public void failed(System.Exception exc, object att)
            {
                (exc as Throwable)?.printStackTrace();
                closeUnchecked(@this.channel);
            }
        }
    }

    static void waitUntilClosed(Channel c)
    {
        while (c.isOpen())
        {
            try
            {
                Thread.sleep(100);
            }
            catch (InterruptedException ignore) { }
        }
    }

    static void closeUnchecked(Channel c)
    {
        try
        {
            c.close();
        }
        catch (IOException ignore) { }
    }
}

