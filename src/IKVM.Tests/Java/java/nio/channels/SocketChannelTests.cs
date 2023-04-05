using System.Threading;
using System.Threading.Tasks;

using java.net;
using java.nio;
using java.nio.channels;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.nio.channels
{

    [TestClass]
    public class SocketChannelTests
    {

        [TestMethod]
        public async Task CanSendUrgentData()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var receive = ByteBuffer.allocate(1024);

            using var ssc = ServerSocketChannel.open();
            ssc.bind(new InetSocketAddress(0));

            var sst = Task.Run(() =>
            {
                // accept first connect
                var sc = ssc.accept();

                // read into receive buffer
                while (sc.read(receive) != -1 && cancellationTokenSource.Token.IsCancellationRequested == false)
                    continue;
            });

            // wait a bit and write some messages to the server
            await Task.Delay(100);
            using (var sc = SocketChannel.open(new InetSocketAddress(((InetSocketAddress)ssc.getLocalAddress()).getPort())))
            {
                sc.configureBlocking(false);
                sc.socket().setOOBInline(false);

                foreach (var i in new byte[] { 1, 2, 3, 4 })
                {
                    sc.socket().sendUrgentData(i);
                    await Task.Delay(100);
                }
            }

            cancellationTokenSource.Cancel();
            await sst;
        }

    }

}
