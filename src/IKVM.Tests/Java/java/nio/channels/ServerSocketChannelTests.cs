using System;
using System.Collections.Generic;
using System.Text;
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
                throw new System.Exception("Expected exception not thrown");
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
        public async Task BasicServerSocketChannelTest()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var messages = new List<string>();

            // server runs and waits for messages from clients
            var serverTask = Task.Run(() =>
            {
                using var server = ServerSocketChannel.open();
                var serverAddr = new InetSocketAddress("0.0.0.0", 42341);
                server.bind(serverAddr);
                server.configureBlocking(false);
                var ops = server.validOps();
                cancellationTokenSource.Token.Register(() => server.close());

                var selector = Selector.open();
                var server1Key = server.register(selector, ops, null);

                while (cancellationTokenSource.Token.IsCancellationRequested == false)
                {
                    selector.select();
                    var keys = selector.selectedKeys();
                    var iter = keys.iterator();
                    while (iter.hasNext())
                    {
                        try
                        {
                            var key = (SelectionKey)iter.next();

                            if (key.isAcceptable())
                            {
                                var client = server.accept();
                                client.configureBlocking(false);
                                client.register(selector, SelectionKey.OP_READ);
                            }
                            else if (key.isReadable())
                            {
                                var client = (SocketChannel)key.channel();
                                var buffer = ByteBuffer.allocate(256);
                                client.read(buffer);
                                var data = (byte[])buffer.array();
                                var term = Array.FindIndex(data, i => i == 0x00);
                                var result = Encoding.UTF8.GetString(data, 0, term);
                                messages.Add(result);

                                if (result == "BYE")
                                    client.close();
                            }
                        }
                        catch (Exception)
                        {

                        }

                        iter.remove();
                    }
                }

                server.close();
            });

            await Task.Delay(100);

            // client sends 3 messages and then BYE to the server
            var clientTask = Task.Run(() =>
            {
                using var sock = SocketChannel.open(new InetSocketAddress("127.0.0.1", 42341));

                foreach (var i in new[] { "MESSAGEA", "MESSAGEB", "MESSAGEC", "BYE" })
                {
                    var data = new byte[Encoding.UTF8.GetByteCount(i) + 1];
                    Encoding.UTF8.GetBytes(i).CopyTo(data, 0);
                    data[data.Length - 1] = 0x00;
                    var buffer = ByteBuffer.wrap(data);
                    sock.write(buffer);
                    buffer.clear();
                    Thread.Sleep(500);
                }

                sock.close();
            });

            await Task.Delay(5000);
            cancellationTokenSource.Cancel();

            await clientTask;
            await serverTask;

            messages.Should().HaveCount(4);
            messages[0].Should().Be("MESSAGEA");
            messages[1].Should().Be("MESSAGEB");
            messages[2].Should().Be("MESSAGEC");
            messages[3].Should().Be("BYE");
        }


    }

}
