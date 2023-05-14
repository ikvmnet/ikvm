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
        public void ShouldThrowOnReceiveTimeout()
        {
            var localhost = global::java.net.InetAddress.getLocalHost();
            using var s = new global::java.net.DatagramSocket(41021, localhost);
            s.setSoTimeout(200);
            var b = new byte[1024];
            var packet = new global::java.net.DatagramPacket(b, b.Length);
            s.receive(packet);
        }

        [TestMethod]
        public async Task CanSendAndReceive()
        {
            var localhost = InetAddress.getLocalHost();
            var received = new List<string>();

            // simple loop that listens and waits
            using var serverSocket = new DatagramSocket(0, localhost);
            serverSocket.setSoTimeout(5000);
            var serverCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var server = Task.Run(() =>
            {
                serverCts.Token.Register(() => serverSocket.close());
                var b = new byte[65535];

                while (serverCts.IsCancellationRequested == false)
                {
                    try
                    {
                        var packet = new DatagramPacket(b, b.Length);
                        serverSocket.receive(packet);
                        received.Add(Encoding.UTF8.GetString(b, packet.getOffset(), packet.getLength()));
                    }
                    catch (InterruptedIOException)
                    {

                    }
                    catch (SocketException)
                    {

                    }
                }
            });

            void Send(string text)
            {
                var buffer = Encoding.UTF8.GetBytes(text);
                var packet = new DatagramPacket(buffer, buffer.Length, localhost, serverSocket.getLocalPort());
                using var client = new DatagramSocket();
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

            using var dc = new DatagramSocket();
            var b = new byte[100];
            var dp = new DatagramPacket(b, b.Length);

            for (int i = 0; i < msgs.Length; i++)
            {
                dc.send(new DatagramPacket(Encoding.ASCII.GetBytes(msgs[i]), msgs[i].Length, InetAddress.getLocalHost(), port));
                dc.receive(dp);
                Encoding.ASCII.GetString(dp.getData(), dp.getOffset(), dp.getLength()).Should().Be(msgs[i]);
            }

            st.Wait();
            dc.close();
            ds.close();
        }

        [TestMethod]
        public void ShouldAbortCancelWhenClosed()
        {
            using var ds = new DatagramSocket(0);
            var ex = (SocketException)null;

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

        [TestMethod]
        public void SendDatagramToBadAddress()
        {
            // open a socket to get a local port
            using var srvr = new DatagramSocket(0);
            var port = srvr.getLocalPort();
            srvr.close();

            // create a new socket to be a client
            var sock = new DatagramSocket();
            sock.setSoTimeout(2000);
            var addr = InetAddress.getByName("127.0.0.1");
            sock.connect(addr, port);

            // send should pass, but obviously nothing went anywhere
            var buff = new byte[1024];
            var p1 = new DatagramPacket(buff, buff.Length, addr, port);
            sock.send(p1);

            try
            {
                // next attempt to receive should fail with ICMP
                var p2 = new DatagramPacket(buff, buff.Length, addr, port);
                sock.receive(p2);
            }
            catch (PortUnreachableException)
            {
                return;
            }

            throw new System.Exception("Did not receive exception.");
        }

        [TestMethod]
        public void ConnectAfterDisconnect()
        {
            var addr = InetAddress.getByName("127.0.0.1");
            using var srvr = new DatagramSocket(0);
            var port = srvr.getLocalPort();
            srvr.close();

            using var sock = new DatagramSocket();
            sock.connect(addr, port);
            sock.disconnect();
            sock.connect(addr, port);
            sock.disconnect();
        }

    }

}
