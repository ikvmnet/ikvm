using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using java.io;
using java.lang;
using java.net;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.net
{

    [TestClass]
    public class DatagramSocketTests
    {

        [TestMethod]
        public void Can_listen_on_any()
        {
            using var s = new global::java.net.DatagramSocket(0);
            s.isClosed().Should().BeFalse();
            s.isBound().Should().BeTrue();
            s.isConnected().Should().BeFalse();
            var a = s.getLocalAddress();
            a.Should().NotBeNull();
            var p = s.getLocalPort();
            p.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void Can_listen_on_specific()
        {
            using var s = new global::java.net.DatagramSocket(42343);
            s.isClosed().Should().BeFalse();
            s.isBound().Should().BeTrue();
            s.isConnected().Should().BeFalse();
            var a = s.getLocalAddress();
            a.Should().NotBeNull();
            var p = s.getLocalPort();
            p.Should().Be(42343);
        }

        [TestMethod]
        public void Can_listen_on_wildcard()
        {
            using var s = new global::java.net.DatagramSocket(40104, global::java.net.InetAddress.getByName("0.0.0.0"));
            s.isClosed().Should().BeFalse();
            s.isBound().Should().BeTrue();
            s.isConnected().Should().BeFalse();
            var a = s.getLocalAddress();
            a.Should().NotBeNull();
            var p = s.getLocalPort();
            p.Should().Be(40104);
        }

        [TestMethod]
        [ExpectedException(typeof(global::java.net.SocketTimeoutException))]
        public void Should_throw_on_receive_timeout()
        {
            var localhost = global::java.net.InetAddress.getLocalHost();
            using var s = new global::java.net.DatagramSocket(41021, localhost);
            s.setSoTimeout(200);
            var b = new byte[1024];
            var packet = new global::java.net.DatagramPacket(b, b.Length);
            s.receive(packet);
        }

        [TestMethod]
        public async Task Can_send_and_receive()
        {
            var localhost = global::java.net.InetAddress.getLocalHost();
            var received = new List<string>();

            // simple loop that listens and waits
            var serverCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var server = Task.Run(() =>
            {
                using var s = new global::java.net.DatagramSocket(41041, localhost);
                serverCts.Token.Register(() => s.close());
                var b = new byte[65535];

                while (serverCts.IsCancellationRequested == false)
                {
                    try
                    {
                        var packet = new global::java.net.DatagramPacket(b, b.Length);
                        s.receive(packet);
                        received.Add(Encoding.UTF8.GetString(b, packet.getOffset(), packet.getLength()));
                    }
                    catch (global::java.net.SocketException e)
                    {

                    }
                }
            });

            void Send(string text)
            {
                var buffer = Encoding.UTF8.GetBytes(text);
                var packet = new global::java.net.DatagramPacket(buffer, buffer.Length, localhost, 41041);
                using var client = new global::java.net.DatagramSocket();
                client.send(packet);
            }

            // wait for the server to have a chance to receive, then cancel and wait
            Send("MESSAGEA");
            await Task.Delay(1000);
            Send("MESSAGEB");
            await Task.Delay(1000);
            Send("MESSAGEC");
            await Task.Delay(1000);
            serverCts.Cancel();
            await server;

            // check that we received the text
            received.Should().HaveCount(3);
            received[0].Should().Be("MESSAGEA");
            received[1].Should().Be("MESSAGEB");
            received[2].Should().Be("MESSAGEC");
        }

        [TestMethod]
        public void PortUnreachableTest()
        {
            var serverSock = new DatagramSocket();
            var serverPort = serverSock.getLocalPort();

            var clientSock = new DatagramSocket();
            var clientPort = clientSock.getLocalPort();

            void serverSend()
            {
                try
                {
                    var addr = InetAddress.getLocalHost();
                    System.Threading.Thread.Sleep(1000);

                    // send a delayed packet which should mean a delayed icmp port unreachable
                    var b = Encoding.ASCII.GetBytes("A late msg");
                    var packet = new DatagramPacket(b, b.Length, addr, serverPort);
                    clientSock.send(packet);

                    var sock = new DatagramSocket(serverPort);
                    b = Encoding.ASCII.GetBytes("Greetings from the server");
                    packet = new DatagramPacket(b, b.Length, addr, clientPort);
                    sock.send(packet);
                    sock.close();
                }
                catch (global::java.lang.Exception e)
                {
                    System.Console.WriteLine(e.StackTrace);
                }
            }

            // send a burst of packets to the unbound port - we should get back icmp port unreachable messages
            var addr = InetAddress.getLocalHost();
            var b = Encoding.ASCII.GetBytes("Hello me");
            var packet = new DatagramPacket(b, b.Length, addr, serverPort);

            // close just before sending
            serverSock.close();
            for (int i = 0; i < 100; i++)
                clientSock.send(packet);

            serverSend();

            // try to receive
            b = new byte[25];
            packet = new DatagramPacket(b, b.Length, addr, serverPort);
            clientSock.setSoTimeout(10000);
            clientSock.receive(packet);
            System.Console.WriteLine("client received data packet " + Encoding.ASCII.GetString(packet.getData()));

            // done
            clientSock.close();
        }

        /// <summary>
        /// Checks that a buffer can be reused in multiple <see cref="DatagramPacket"/>s.
        /// </summary>
        /// <exception cref="RuntimeException"></exception>
        [TestMethod]
        public void ReuseBuffer()
        {
            var msgs = new string[] { "Hello World", "Java", "Good Bye" };
            using var ds = new DatagramSocket();
            var port = ds.getLocalPort();

            var st = Task.Run(() =>
            {
                var b = new byte[100];
                var dp = new DatagramPacket(b, b.Length);
                while (true)
                {
                    ds.receive(dp);
                    var reply = Encoding.ASCII.GetString(dp.getData(), dp.getOffset(), dp.getLength());
                    ds.send(new DatagramPacket(Encoding.ASCII.GetBytes(reply), reply.Length, dp.getAddress(), dp.getPort()));
                    if (reply.Equals(msgs[msgs.Length - 1]))
                        break;
                }

                ds.close();
            });

            var b = new byte[100];
            var dp = new DatagramPacket(b, b.Length);

            for (int i = 0; i < msgs.Length; i++)
            {
                ds.send(new DatagramPacket(Encoding.ASCII.GetBytes(msgs[i]), msgs[i].Length, InetAddress.getLocalHost(), port));
                ds.receive(dp);
                Encoding.ASCII.GetString(dp.getData(), dp.getOffset(), dp.getLength()).Should().Be(msgs[i]);
            }

            st.Wait();
            ds.close();
        }

        [TestMethod]
        public void ShouldAbortCancelWhenClosed()
        {
            var ds = new DatagramSocket(0);
            var ex = (global::java.net.SocketException)null;

            var task = Task.Run(() =>
            {
                try
                {
                    var p = new DatagramPacket(new byte[100], 100);
                    ds.receive(p);
                }
                catch (SocketException e)
                {
                    ex = e;
                }
            });

            global::java.lang.Thread.sleep(1000);
            ds.close();
            global::java.lang.Thread.sleep(1000);
            task.Wait();

            ex.Should().BeAssignableTo<SocketException>();
        }
        class BadAddressServer
        {

            DatagramSocket server;
            byte[] buf = new byte[128];
            DatagramPacket pack;

            public BadAddressServer(DatagramSocket s)
            {
                server = s;
                pack = new DatagramPacket(buf, buf.Length);
            }

            public void receive(int loop, bool expectError)
            {
                for (int i = 0; i < loop; i++)
                {
                    try
                    {
                        server.receive(pack);
                    }
                    catch (global::java.lang.Exception e)
                    {
                        if (expectError)
                        {
                            System.Console.WriteLine("Got expected error: " + e);
                            continue;
                        }
                        else
                        {
                            System.Console.WriteLine("Got: " + Encoding.UTF8.GetString(pack.getData()));
                            System.Console.WriteLine("Expected: " + Encoding.UTF8.GetString(buf));
                            throw new global::java.lang.Exception("Error reading data: Iter " + i);
                        }
                    }
                    var s1 = "Hello, server" + i;
                    byte[] buf2 = Encoding.UTF8.GetBytes(s1);
                    if (!s1.Equals(Encoding.UTF8.GetString(pack.getData(), pack.getOffset(), pack.getLength())))
                    {
                        System.Console.WriteLine("Got: " + Encoding.UTF8.GetString(pack.getData()));
                        System.Console.WriteLine("Expected: " + Encoding.UTF8.GetString(buf2));
                        throw new global::java.lang.Exception("Error comparing data: Iter " + i);
                    }
                }
            }
        }

        [TestMethod]
        public void BadAddress()
        {
            var host = "127.0.0.1";
            var addr = InetAddress.getByName(host);
            var sock = new DatagramSocket();
            var serversock = new DatagramSocket(0);
            DatagramPacket p;
            byte[] buf;
            int port = serversock.getLocalPort();
            const int loop = 5;
            var s = new BadAddressServer(serversock);
            int i;

            System.Console.WriteLine("Checking send to connected address ...");
            sock.connect(addr, port);

            for (i = 0; i < loop; i++)
            {
                try
                {
                    buf = Encoding.UTF8.GetBytes("Hello, server" + i);
                    if (i % 2 == 1)
                        p = new DatagramPacket(buf, buf.Length, addr, port);
                    else
                        p = new DatagramPacket(buf, buf.Length);
                    sock.send(p);
                }
                catch (global::java.lang.Exception ex)
                {
                    System.Console.WriteLine("Got unexpected exception: " + ex);
                    throw new global::java.lang.Exception("Error sending data: ");
                }
            }

            s.receive(loop, false);

            // check disconnect() works

            System.Console.WriteLine("Checking send to non-connected address ...");
            sock.disconnect();
            buf = Encoding.UTF8.GetBytes("Hello, server" + 0);
            p = new DatagramPacket(buf, buf.Length, addr, port);
            sock.send(p);
            s.receive(1, false);

            // check send() to invalid destination followed by a blocking receive
            // returns an error

            System.Console.WriteLine("Checking send to invalid address ...");
            sock.connect(addr, port);
            serversock.close();
            try
            {
                sock.setSoTimeout(4000);
            }
            catch (global::java.lang.Exception e)
            {
                System.Console.WriteLine("could not set timeout");
                throw e;
            }

            var goterror = false;

            for (i = 0; i < loop; i++)
            {
                try
                {
                    buf = Encoding.UTF8.GetBytes("Hello, server" + i);
                    p = new DatagramPacket(buf, buf.Length, addr, port);
                    sock.send(p);
                    p = new DatagramPacket(buf, buf.Length, addr, port);
                    sock.receive(p);
                }
                catch (InterruptedIOException ex)
                {
                    System.Console.WriteLine("socket timeout");
                }
                catch (global::java.lang.Exception ex)
                {
                    System.Console.WriteLine("Got expected exception: " + ex);
                    goterror = true;
                }
            }

            if (!goterror)
            {
                System.Console.WriteLine("Didnt get expected exception: ");
                throw new global::java.lang.Exception("send did not return expected error");
            }
        }

    }

}