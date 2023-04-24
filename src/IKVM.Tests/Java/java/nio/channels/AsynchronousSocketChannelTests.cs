using FluentAssertions;

using java.io;
using java.lang;
using java.net;
using java.nio;
using java.nio.channels;
using java.util.concurrent;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.nio.channels
{

    [TestClass]
    public class AsynchronousSocketChannelTests
    {

        /// <summary>
        /// Copied from similar test in OpenJDK. Should be cleaned up to just be a simple send/receive loop.
        /// </summary>
        /// <exception cref="RuntimeException"></exception>
        [TestMethod]
        public void TestStressLoopback()
        {
            // setup listener
            var listener = AsynchronousServerSocketChannel.open().bind(new InetSocketAddress(0));
            int port = ((InetSocketAddress)(listener.getLocalAddress())).getPort();
            var lh = InetAddress.getLocalHost();
            var remote = new InetSocketAddress(lh, port);

            // create sources and sinks
            var ch = AsynchronousSocketChannel.open();
            ch.connect(remote).get();
            var src = new Source(ch);
            var dst = new Sink((AsynchronousByteChannel)listener.accept().get());

            // start the sink and source
            dst.start();
            src.Start();

            // let the test run for a while
            Thread.sleep(20 * 1000);

            // wait until everyone is done
            var failed = false;

            var sent = src.Finish();
            var recv = dst.Finish();

            if (recv != sent)
                failed = true;

            if (failed)
                throw new RuntimeException("Test failed - see log for details");
        }

        sealed class Source
        {

            readonly AsynchronousByteChannel channel;
            readonly ByteBuffer sentBuffer;
            long bytesSent;
            bool finished;

            internal Source(AsynchronousByteChannel channel)
            {
                this.channel = channel;
                this.sentBuffer = ByteBuffer.allocate(1024);
            }

            internal void Start()
            {
                sentBuffer.position(0);
                sentBuffer.limit(sentBuffer.capacity());
                channel.write(sentBuffer, null, new Completion(this));
            }

            internal long Finish()
            {
                finished = true;
                WaitUntilClosed(channel);
                return bytesSent;
            }

            private class Completion : CompletionHandler
            {

                private readonly Source source;

                public Completion(Source @this)
                {
                    this.source = @this;
                }

                public void completed(Integer nwrote, object _)
                {
                    source.bytesSent += nwrote.intValue();

                    if (source.finished)
                    {
                        CloseUnchecked(source.channel);
                    }
                    else
                    {
                        source.sentBuffer.position(0);
                        source.sentBuffer.limit(source.sentBuffer.capacity());
                        source.channel.write(source.sentBuffer, null, this);
                    }
                }

                void CompletionHandler.completed(object result, object attachment) => completed(result as Integer, attachment);

                public void failed(System.Exception exc, object attachment)
                {
                    (exc as Throwable)?.printStackTrace();
                    CloseUnchecked(source.channel);
                }
            }
        }

        /**
         * Read bytes from a channel until EOF is received.
         */
        sealed class Sink
        {

            readonly AsynchronousByteChannel channel;
            readonly ByteBuffer readBuffer;
            long bytesRead;

            internal Sink(AsynchronousByteChannel channel)
            {
                this.channel = channel;
                this.readBuffer = ByteBuffer.allocate(1024);
            }

            internal void start()
            {
                channel.read(readBuffer, null, new Completion(this));
            }

            internal long Finish()
            {
                WaitUntilClosed(channel);
                return bytesRead;
            }

            class Completion : CompletionHandler
            {

                readonly Sink sink;

                public Completion(Sink @this)
                {
                    this.sink = @this;
                }

                public void completed(Integer nread, object _)
                {
                    if (nread.intValue() < 0)
                    {
                        CloseUnchecked(sink.channel);
                    }
                    else
                    {
                        sink.bytesRead += nread.intValue();
                        sink.readBuffer.clear();
                        sink.channel.read(sink.readBuffer, null, this);
                    }
                }

                void CompletionHandler.completed(object result, object attachment) => completed(result as Integer, attachment);

                public void failed(System.Exception exc, object att)
                {
                    (exc as Throwable)?.printStackTrace();
                    CloseUnchecked(sink.channel);
                }

            }

        }

        static void WaitUntilClosed(Channel ch)
        {
            while (ch.isOpen())
            {
                try
                {
                    Thread.sleep(100);
                }
                catch (InterruptedException e)
                {

                }
            }
        }

        static void CloseUnchecked(Channel ch)
        {
            try
            {
                ch.close();
            }
            catch (IOException e)
            {

            }
        }

        [TestMethod]
        public void ShouldTimeoutOnRead()
        {
            // listen for connection
            using var listener = AsynchronousServerSocketChannel.open().bind(new InetSocketAddress(0));
            var port = ((InetSocketAddress)listener.getLocalAddress()).getPort();

            // initiate connection
            var sendChannel = AsynchronousSocketChannel.open();
            sendChannel.connect(new InetSocketAddress(InetAddress.getLocalHost(), port)).get();

            // accept connection
            var recvChannel = (AsynchronousSocketChannel)listener.accept().get();

            // start a read
            var buf = ByteBuffer.allocate(512);
            var hnd = new AwaitableCompletionHandler();
            sendChannel.read(buf, 1, TimeUnit.SECONDS, null, hnd);

            // after 2 seconds should have thrown timeout
            Thread.sleep(2000);
            hnd.GetAwaiter().Invoking(a => a.GetResult()).Should().Throw<InterruptedByTimeoutException>();

            // the next attempt should thrown an unspecified runtime exception
            sendChannel.Invoking(c => c.read(buf)).Should().Throw<RuntimeException>();
        }

    }

}
