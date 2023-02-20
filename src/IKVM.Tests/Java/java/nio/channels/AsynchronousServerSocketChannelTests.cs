using System.Security.Cryptography;
using System.Threading.Tasks;

using FluentAssertions;

using java.lang;
using java.net;
using java.nio;
using java.nio.channels;
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

    }

}
