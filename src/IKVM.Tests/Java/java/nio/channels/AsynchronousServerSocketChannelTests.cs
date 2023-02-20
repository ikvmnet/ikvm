using System.Threading.Tasks;

using FluentAssertions;

using java.lang;
using java.net;
using java.nio;
using java.nio.channels;

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
                sc.read(bb).get();

                // respond with same data
                bb.flip();
                sc.write(bb);

                // wait for client to finish
                await Task.Delay(500);
                sc.close();
                ss.close();
            });

            var cs = AsynchronousSocketChannel.open();
            cs.connect(new InetSocketAddress(54101)).get();

            // wait for client to send data
            var bb = ByteBuffer.allocate(1024);
            bb.putLong(0xDEADBEEF);
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

    }

}
