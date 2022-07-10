using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.net
{

    [TestClass]
    public class DatagramSocketTests
    {

        [TestMethod]
        public void Can_listen_on_any()
        {
            using var s = new global::java.net.DatagramSocket();
        }

        [TestMethod]
        public void Can_listen_on_specific()
        {
            using var s = new global::java.net.DatagramSocket(42343);
        }

        [TestMethod]
        public void Can_listen_on_wildcard()
        {
            using var s = new global::java.net.DatagramSocket(40104, global::java.net.InetAddress.getByName("0.0.0.0"));
        }

        [TestMethod]
        public async Task Can_send_and_receive()
        {
            var localhost = global::java.net.InetAddress.getLocalHost();
            var received = new List<string>();

            // simple loop that listens and waits
            var serverCts = new CancellationTokenSource();
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
                var client = new global::java.net.DatagramSocket();
                client.send(packet);
            }

            // wait for the server to have a chance to receive, then cancel and wait
            Send("HELLO");
            await Task.Delay(1000);
            serverCts.Cancel();
            await server;

            // check that we received the text
            received.Should().HaveCount(1);
            received[0].Should().Be("HELLO");
        }

    }

}