using System;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using java.net;
using java.nio;
using java.nio.channels;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.nio.channels
{

    [TestClass]
    public class ServerSocketChannelTests
    {

        [TestMethod]
        public void CanOpenAndClose()
        {
            using var ssc = ServerSocketChannel.open();
            ssc.configureBlocking(false);
            ssc.close();
        }

        /// <summary>
        /// Check that we can conduct a non-blocking accept.
        /// </summary>
        /// <exception cref="System.Exception"></exception>
        [TestMethod]
        public void NonBlockingAccept()
        {
            using var ssc = ServerSocketChannel.open();
            ssc.bind(new InetSocketAddress(0));
            ssc.configureBlocking(false);
            var ss = ssc.socket();

            // exception should be thrown when no connection is waiting
            try
            {
                ss.accept();
                throw new Exception("Expected exception not thrown");
            }
            catch (IllegalBlockingModeException)
            {
                // correct
            }

            // no exception should be thrown when a connection is waiting
            using var sc = SocketChannel.open();
            sc.configureBlocking(false);
            sc.connect(ss.getLocalSocketAddress());
            Thread.Sleep(100);
            ss.accept();
        }

        /// <summary>
        /// Runs a server socket, spawns a client, and attempts to transfer some data.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CanReceiveBlocking()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var receive = ByteBuffer.allocate(sizeof(int) * 4);

            // bind on the test thread rather than inside the server task: port 0 asks the OS for a
            // free port instead of claiming a fixed one that something else on the machine may hold,
            // and returning from bind establishes the listen backlog, so the client below can connect
            // immediately instead of sleeping and hoping the task got there first
            using var server = ServerSocketChannel.open();
            server.bind(new InetSocketAddress(0));
            server.configureBlocking(true);
            var port = server.socket().getLocalPort();

            // server receives messages until cancelled
            var serverTask = Task.Run(() =>
            {
                // accept the first socket
                var c = server.accept();

                // read into receive buffer
                while (c.read(receive) != -1 && cancellationTokenSource.Token.IsCancellationRequested == false)
                    continue;
            });

            // write some messages to the server
            using (var c = SocketChannel.open(new InetSocketAddress("127.0.0.1", port)))
            {
                foreach (var i in new[] { 1, 2, 3, 4 })
                {
                    var b = ByteBuffer.allocate(sizeof(int));
                    b.putInt(i);
                    b.flip();
                    c.write(b);

                    // small delay to allow server to receive as multiple packets
                    await Task.Delay(100);
                }
            }

            // wait for the server to receive them and then exit
            cancellationTokenSource.Cancel();
            await serverTask;

            // check that we received 4 ints
            receive.flip();
            receive.getInt().Should().Be(1);
            receive.getInt().Should().Be(2);
            receive.getInt().Should().Be(3);
            receive.getInt().Should().Be(4);
        }

        /// <summary>
        /// Runs a server socket, spawns a client, and attempts to transfer some data.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CanReceiveNonBlocking()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var receive = ByteBuffer.allocate(sizeof(int) * 4);

            // bind on the test thread rather than inside the server task. The port was previously
            // read back from a field the task assigned, a second after starting it, so a task that
            // had not been scheduled yet left the client connecting to port 0
            using var server = ServerSocketChannel.open();
            server.bind(new InetSocketAddress(0));
            server.configureBlocking(false);
            var port = server.socket().getLocalPort();

            // server receives messages until cancelled
            var serverTask = Task.Run(() =>
            {
                // begin selector
                var selector = Selector.open();
                var serverKey = server.register(selector, server.validOps(), null);

                // continue until cancelled
                while (cancellationTokenSource.Token.IsCancellationRequested == false)
                {
                    selector.select();

                    var keys = selector.selectedKeys();
                    var iter = keys.iterator();
                    while (iter.hasNext())
                    {
                        try
                        {
                            var k = (SelectionKey)iter.next();

                            if (k.isAcceptable())
                            {
                                var c = server.accept();
                                c.configureBlocking(false);
                                c.register(selector, SelectionKey.OP_READ);
                            }
                            else if (k.isReadable())
                            {
                                var c = (SocketChannel)k.channel();
                                var n = c.read(receive);
                            }
                        }
                        catch (Exception)
                        {

                        }

                        iter.remove();
                    }
                }
            });

            // wait a second and write some messages to the server
            await Task.Delay(1000);
            using (var c = SocketChannel.open(new InetSocketAddress("127.0.0.1", port)))
            {
                foreach (var i in new[] { 1, 2, 3, 4 })
                {
                    var b = ByteBuffer.allocate(sizeof(int));
                    b.putInt(i);
                    b.flip();
                    c.write(b);

                    // small delay to allow server to receive as multiple packets
                    await Task.Delay(100);
                }
            }

            // wait for the server to receive them and then exit
            cancellationTokenSource.Cancel();
            await serverTask;

            // check that we received 4 ints
            receive.flip();
            receive.getInt().Should().Be(1);
            receive.getInt().Should().Be(2);
            receive.getInt().Should().Be(3);
            receive.getInt().Should().Be(4);
        }

    }

}
