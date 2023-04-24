using System.Security.Cryptography;
using System.Threading.Tasks;

using FluentAssertions;

using java.lang;
using java.net;
using java.nio;
using java.nio.channels;
using java.util;
using java.util.concurrent;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.nio.channels
{

    [TestClass]
    public class AsynchronousServerSocketChannelTests
    {

        [TestMethod]
        public void CanOpenAndClose()
        {
            using var c = AsynchronousServerSocketChannel.open();
            c.close();
        }

        [TestMethod]
        public void CanBindAndClose()
        {
            using var ss = AsynchronousServerSocketChannel.open();
            ss.bind(new InetSocketAddress(0));
            ss.close();
        }

        [TestMethod]
        public async Task CanSendAndReceiveData()
        {
            var s = Task.Run(async () =>
            {
                using var ss = AsynchronousServerSocketChannel.open();
                ss.bind(new InetSocketAddress(54101));

                // wait for client to connect
                var sc = (AsynchronousSocketChannel)ss.accept().get();

                // wait for client to send data
                var bb = ByteBuffer.allocate(1024);
                var l = (Integer)sc.read(bb).get();

                // respond with same data
                bb.flip();
                bb.limit(l.intValue());
                sc.write(bb);

                // wait for client to finish
                await Task.Delay(500);
                sc.close();
                ss.close();
            });

            var cs = AsynchronousSocketChannel.open();
            cs.connect(new InetSocketAddress("127.0.0.1", 54101)).get();

            // wait for client to send data
            var bb = ByteBuffer.allocate(1024);
            bb.putLong(0xDEADBEEF);
            bb.limit(bb.position());
            bb.flip();
            cs.write(bb);

            // read the response and check that it matches what we sent
            bb.clear();
            var n = ((Integer)cs.read(bb).get()).intValue();
            n.Should().Be(sizeof(long));
            bb.flip();
            bb.getLong().Should().Be(0xDEADBEEF);

            // wait for server to end
            await s;
        }

        [TestMethod]
        public async Task CanSendAndReceiveDataWithDirectBuffers()
        {
            var s = Task.Run(async () =>
            {
                using var ss = AsynchronousServerSocketChannel.open();
                ss.bind(new InetSocketAddress(54102));

                // wait for client to connect
                var sc = (AsynchronousSocketChannel)ss.accept().get();

                // wait for client to send data
                var bb = ByteBuffer.allocateDirect(1024);
                var l = (Integer)sc.read(bb).get();

                // respond with same data
                bb.flip();
                bb.limit(l.intValue());
                sc.write(bb);

                // wait for client to finish
                await Task.Delay(500);
                sc.close();
                ss.close();
            });

            var cs = AsynchronousSocketChannel.open();
            cs.connect(new InetSocketAddress("127.0.0.1", 54102)).get();

            // wait for client to send data
            var bb = ByteBuffer.allocateDirect(1024);
            bb.putLong(0xDEADBEEF);
            bb.limit(bb.position());
            bb.flip();
            cs.write(bb);

            // read the response and check that it matches what we sent
            bb.clear();
            var n = ((Integer)cs.read(bb).get()).intValue();
            n.Should().Be(sizeof(long));
            bb.flip();
            bb.getLong().Should().Be(0xDEADBEEF);

            // wait for server to end
            await s;
        }

        [TestMethod]
        public async Task CanSendAndReceiveDataWithMultipleBuffers()
        {
            var s = Task.Run(async () =>
            {
                using var ss = AsynchronousServerSocketChannel.open();
                ss.bind(new InetSocketAddress(54103));

                // wait for client to connect
                var sc = (AsynchronousSocketChannel)ss.accept().get();

                // wait for client to send data
                var bb1 = ByteBuffer.allocate(1024);
                var bb2 = ByteBuffer.allocate(1024);
                var h1 = new AwaitableCompletionHandler<Long>();
                sc.read(new[] { bb1, bb2 }, 0, 2, 0, TimeUnit.MILLISECONDS, null, h1);
                var l1 = await h1;

                // respond with same data
                bb1.limit(bb1.position());
                bb1.flip();
                bb2.limit(bb2.position());
                bb2.flip();
                var h2 = new AwaitableCompletionHandler<Long>();
                sc.write(new[] { bb1, bb2 }, 0, 2, 0, TimeUnit.MILLISECONDS, null, h2);
                var l2 = await h2;

                // wait for client to finish
                await Task.Delay(500);
                sc.close();
                ss.close();
            });

            var cs = AsynchronousSocketChannel.open();
            cs.connect(new InetSocketAddress("127.0.0.1", 54103)).get();

            var dat = new byte[1024];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(dat);

            // wait for client to send data
            var bb1 = ByteBuffer.allocate(1024);
            bb1.put(dat);
            bb1.flip();

            var bb2 = ByteBuffer.allocate(1024);
            bb2.put(dat);
            bb2.flip();

            var h1 = new AwaitableCompletionHandler<Long>();
            cs.write(new[] { bb1, bb2 }, 0, 2, 0, TimeUnit.MILLISECONDS, null, h1);
            var w = (await h1).longValue();
            w.Should().Be(2048);

            // read the response and check that it matches what we sent
            bb1.clear();
            bb2.clear();
            var h2 = new AwaitableCompletionHandler<Long>();
            cs.read(new[] { bb1, bb2 }, 0, 2, 0, TimeUnit.MILLISECONDS, null, h2);
            var r = (await h2).longValue();
            r.Should().Be(2048);

            bb1.flip();
            bb2.flip();

            var aa1 = new byte[bb1.limit()];
            bb1.get(aa1);

            var aa2 = new byte[bb2.limit()];
            bb2.get(aa2);

            // check that the result equals the random data
            aa1.Should().BeEquivalentTo(dat);
            aa2.Should().BeEquivalentTo(dat);

            // wait for server to end
            await s;
        }

        [TestMethod]
        public async Task CanSendAndReceiveDataWithMultipleDirectBuffers()
        {
            var s = Task.Run(async () =>
            {
                using var ss = AsynchronousServerSocketChannel.open();
                ss.bind(new InetSocketAddress(54104));

                // wait for client to connect
                var sc = (AsynchronousSocketChannel)ss.accept().get();

                // wait for client to send data
                var bb1 = ByteBuffer.allocateDirect(1024);
                var bb2 = ByteBuffer.allocateDirect(1024);
                var h1 = new AwaitableCompletionHandler<Long>();
                sc.read(new[] { bb1, bb2 }, 0, 2, 0, TimeUnit.MILLISECONDS, null, h1);
                var l1 = await h1;

                // respond with same data
                bb1.limit(bb1.position());
                bb1.flip();
                bb2.limit(bb2.position());
                bb2.flip();
                var h2 = new AwaitableCompletionHandler<Long>();
                sc.write(new[] { bb1, bb2 }, 0, 2, 0, TimeUnit.MILLISECONDS, null, h2);
                var l2 = await h2;

                // wait for client to finish
                await Task.Delay(500);
                sc.close();
                ss.close();
            });

            var cs = AsynchronousSocketChannel.open();
            cs.connect(new InetSocketAddress("127.0.0.1", 54104)).get();

            var dat = new byte[1024];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(dat);

            // wait for client to send data
            var bb1 = ByteBuffer.allocateDirect(1024);
            bb1.put(dat);
            bb1.limit(bb1.position());
            bb1.flip();

            var bb2 = ByteBuffer.allocateDirect(1024);
            bb2.put(dat);
            bb2.limit(bb2.position());
            bb2.flip();

            var h1 = new AwaitableCompletionHandler<Long>();
            cs.write(new[] { bb1, bb2 }, 0, 2, 0, TimeUnit.MILLISECONDS, null, h1);
            var w = (await h1).longValue();
            w.Should().Be(2048);

            // read the response and check that it matches what we sent
            bb1.clear();
            bb2.clear();
            var h2 = new AwaitableCompletionHandler<Long>();
            cs.read(new[] { bb1, bb2 }, 0, 2, 0, TimeUnit.MILLISECONDS, null, h2);
            var r = (await h2).longValue();
            r.Should().Be(2048);

            bb1.limit(bb1.position());
            bb1.flip();
            bb2.limit(bb2.position());
            bb2.flip();

            var aa1 = new byte[bb1.limit()];
            bb1.get(aa1);

            var aa2 = new byte[bb2.limit()];
            bb2.get(aa2);

            // check that the result equals the random data
            aa1.Should().BeEquivalentTo(dat);
            aa2.Should().BeEquivalentTo(dat);

            // wait for server to end
            await s;
        }

        [TestMethod]
        [ExpectedException(typeof(AcceptPendingException))]
        public void ShouldThrowAcceptPending()
        {
            var l = AsynchronousServerSocketChannel.open().bind(new InetSocketAddress(0));
            var h = new AwaitableCompletionHandler<AsynchronousSocketChannel>();
            l.accept(null, h);
            l.accept();
        }

        [TestMethod]
        public void CanReadAndWriteWithStreamsSimple()
        {
            using var listener = AsynchronousServerSocketChannel.open().bind(new InetSocketAddress(0));
            var port = ((InetSocketAddress)listener.getLocalAddress()).getPort();
            var sendChannel = AsynchronousSocketChannel.open();
            sendChannel.connect(new InetSocketAddress(InetAddress.getLocalHost(), port)).get();
            var recvChannel = (AsynchronousSocketChannel)listener.accept().get();

            var sendStream = Channels.newOutputStream(sendChannel);

            var sendBuffer = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            sendStream.write(sendBuffer);
            sendStream.close();

            var recvStream = Channels.newInputStream(recvChannel);
            var recvBuffer = new byte[4];
            recvStream.read(recvBuffer);
            recvStream.close();

            recvBuffer.Should().BeEquivalentTo(sendBuffer);
        }

        [TestMethod]
        public void CanReadAndWriteWithStreamsOffset()
        {
            using var listener = AsynchronousServerSocketChannel.open().bind(new InetSocketAddress(0));
            var port = ((InetSocketAddress)listener.getLocalAddress()).getPort();
            var sendChannel = AsynchronousSocketChannel.open();
            sendChannel.connect(new InetSocketAddress(InetAddress.getLocalHost(), port)).get();
            var recvChannel = (AsynchronousSocketChannel)listener.accept().get();

            // send middle two bytes
            var sendStream = Channels.newOutputStream(sendChannel);
            var sendBuffer = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            sendStream.write(sendBuffer, 1, 2);
            sendStream.close();

            // receve into middle two bytes
            var recvStream = Channels.newInputStream(recvChannel);
            var recvBuffer = new byte[] { 0x01, 0x00, 0x00, 0x04 };
            recvStream.read(recvBuffer, 1, 2);
            recvStream.close();

            recvBuffer.Should().BeEquivalentTo(sendBuffer);
        }

        [TestMethod]
        public void CanReadAndWriteWithStreams()
        {
            var rand = new Random();

            using var listener = AsynchronousServerSocketChannel.open().bind(new InetSocketAddress(0));
            int port = ((InetSocketAddress)listener.getLocalAddress()).getPort();
            var isa = new InetSocketAddress(InetAddress.getLocalHost(), port);
            var ch1 = AsynchronousSocketChannel.open();
            ch1.connect(isa).get();
            var ch2 = (AsynchronousSocketChannel)listener.accept().get();

            // thread to write random data
            var writeStream = Channels.newOutputStream(ch1);
            int writeSize = 50 * 1000 + rand.nextInt(50 * 1000);
            int writeHash = 0;
            var writeTask = Task.Run(() =>
            {
                int rem = writeSize;

                do
                {
                    byte[] buf = new byte[1 + rand.nextInt(rem)];
                    int off, len;

                    // write random bytes
                    if (rand.nextBoolean())
                    {
                        off = 0;
                        len = buf.Length;
                    }
                    else
                    {
                        off = rand.nextInt(buf.Length);
                        int r = buf.Length - off;
                        len = (r <= 1) ? 1 : (1 + rand.nextInt(r));
                    }

                    for (int i = 0; i < len; i++)
                    {
                        byte value = (byte)rand.nextInt(256);
                        buf[off + i] = value;
                        writeHash = writeHash ^ value;
                    }

                    if ((off == 0) && (len == buf.Length))
                    {
                        writeStream.write(buf);
                    }
                    else
                    {
                        writeStream.write(buf, off, len);
                    }

                    rem -= len;
                } while (rem > 0);

                // close stream when done
                writeStream.close();
            });

            // thread to read random data
            var readStream = Channels.newInputStream(ch2);
            int readSize = 0;
            int readHash = 0;
            var readTask = Task.Run(() =>
            {
                int n;

                do
                {
                    // random offset/len
                    byte[] buf = new byte[128 + rand.nextInt(128)];
                    int len, off;
                    if (rand.nextBoolean())
                    {
                        len = buf.Length;
                        off = 0;
                        n = readStream.read(buf);
                    }
                    else
                    {
                        len = 1 + rand.nextInt(64);
                        off = rand.nextInt(64);
                        n = readStream.read(buf, off, len);
                    }

                    if (n > len)
                        throw new RuntimeException("Too many bytes read");

                    if (n > 0)
                    {
                        readSize += n;
                        for (int i = 0; i < n; i++)
                        {
                            int value = buf[off + i];
                            readHash = readHash ^ value;
                        }
                    }
                } while (n > 0);

                readStream.close();
            });

            writeTask.GetAwaiter().GetResult();
            readTask.GetAwaiter().GetResult();

            // shutdown listener
            listener.close();

            // check that reader received what we expected
            if (readSize != writeSize)
                throw new RuntimeException("Unexpected number of bytes read");
            if (readHash != writeHash)
                throw new RuntimeException("Hash incorrect for bytes read");

            // channels should be closed
            if (ch1.isOpen() || ch2.isOpen())
                throw new RuntimeException("Channels should be closed");
        }

        [TestMethod]
        [ExpectedException(typeof(AcceptPendingException))]
        public void ShouldThrowAcceptPendingExceptionOnAccept()
        {
            var listener = AsynchronousServerSocketChannel.open().bind(new InetSocketAddress(0));
            var accept = new AwaitableCompletionHandler();
            listener.accept(null, accept);
            listener.accept();
            listener.close();
        }

        [TestMethod]
        [ExpectedException(typeof(AsynchronousCloseException))]
        public async Task ShouldThrowAsynchronousCloseExceptionForAcceptDuringAccept()
        {
            var listener = AsynchronousServerSocketChannel.open().bind(new InetSocketAddress(0));

            // initiate an outstanding accept
            var accept = new AwaitableCompletionHandler();
            listener.accept(null, accept);
            listener.close();

            // complete first acceptance, which should throw nested AsynchronousCloseException
            try
            {
                await accept;
            }
            catch (ExecutionException e)
            {
                throw e.getCause();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ClosedChannelException))]
        public void ShouldThrowClosedChannelExceptionForAcceptAfterClose()
        {
            var listener = AsynchronousServerSocketChannel.open().bind(new InetSocketAddress(0));

            // initiate an outstanding accept
            listener.close();

            // try accept while closed
            try
            {
                listener.accept().get();
            }
            catch (ExecutionException e)
            {
                throw e.getCause();
            }
        }

    }

}
