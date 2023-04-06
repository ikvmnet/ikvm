using System;
using System.Threading;
using System.Threading.Tasks;

using java.io;
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

        [TestMethod]
        public void SendUrgentDataThrowsWhenBlocked()
        {
            using var ssc = ServerSocketChannel.open();
            ssc.bind(new InetSocketAddress(0));

            using (var sc = SocketChannel.open(new InetSocketAddress(((InetSocketAddress)ssc.getLocalAddress()).getPort())))
            {
                sc.configureBlocking(false);
                sc.socket().setOOBInline(false);

                var bb = ByteBuffer.wrap(new byte[100 * 1000]);
                var blocked = 0L;
                var total = 0L;

                // write data until we block, indicating send buffers are full
                var n = 0;
                do
                {
                    n = sc.write(bb);
                    if (n == 0)
                    {
                        if (++blocked == 10)
                            break;

                        Thread.Sleep(100);
                    }
                    else
                    {
                        total += n;
                        bb.rewind();
                    }
                } while (n > 0);

                // attempt to sendUrgentData to socket that is blocked, and affirm resulting exception
                var attempted = 0;
                while (attempted < total)
                {
                    bb.rewind();
                    n = sc.write(bb);
                    attempted += bb.capacity();

                    var osName = global::java.lang.System.getProperty("os.name").ToLowerInvariant();

                    try
                    {
                        sc.socket().sendUrgentData(0);
                    }
                    catch (IOException ex)
                    {
                        if (osName.Contains("linux"))
                        {
                            if (!ex.getMessage().Contains("Socket buffer full"))
                            {
                                throw new Exception("Unexpected message", ex);
                            }
                        }
                        else if (osName.Contains("os x") || osName.Contains("mac"))
                        {
                            if (!ex.getMessage().Equals("No buffer space available"))
                            {
                                throw new Exception("Unexpected message", ex);
                            }
                        }
                        else if (osName.Contains("windows"))
                        {
                            if (!(ex is SocketException))
                            {
                                throw new Exception("Unexpected exception", ex);
                            }
                            else if (!ex.getMessage().Contains("Resource temporarily unavailable"))
                            {
                                throw new Exception("Unexpected message", ex);
                            }
                        }
                        else
                        {
                            throw new Exception("Unexpected IOException", ex);
                        }
                    }
                }
            }

        }

    }

}
