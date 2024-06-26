using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using FluentAssertions;

using java.io;
using java.lang;
using java.net;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.net
{

    [TestClass]
    public class ServerSocketTests
    {

        /// <summary>
        /// Accepts connections.
        /// </summary>
        class EchoSocketServerThread : global::java.lang.Thread
        {

            int port;
            global::java.net.ServerSocket serverSocket;
            volatile bool running = false;

            /// <summary>
            /// Gets the current port.
            /// </summary>
            public int Port => port;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="port"></param>
            /// <param name="onLine"></param>
            public EchoSocketServerThread(int port)
            {
                this.port = port;
            }

            public void Start()
            {
                serverSocket = new ServerSocket(port, 1, InetAddress.getLocalHost());
                port = serverSocket.getLocalPort();
                serverSocket.setSoTimeout(1000);
                running = true;
                base.start();
            }

            public void Stop()
            {
                running = false;
                serverSocket.close();
                base.interrupt();
            }

            public override void run()
            {
                try
                {
                    while (running)
                    {
                            try
                            {
                                new EchoSocketClientThread(serverSocket.accept()).start();
                            }
                            catch (SocketTimeoutException)
                            {

                            }
                    }
                }
                catch (System.Exception e)
                {
                    global::java.lang.System.err.println(e);
                }

                global::java.lang.System.@out.println("EchoSocketServerThread exited");
            }

        }

        /// <summary>
        /// Handles a single client.
        /// </summary>
        class EchoSocketClientThread : global::java.lang.Thread
        {

            readonly global::java.net.Socket socket;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="socket"></param>
            /// <param name="onLine"></param>
            public EchoSocketClientThread(global::java.net.Socket socket)
            {
                this.socket = socket ?? throw new ArgumentNullException(nameof(socket));
            }

            public override void run()
            {
                var rdr = new global::java.io.BufferedReader(new global::java.io.InputStreamReader(socket.getInputStream()));
                var wrt = new global::java.io.PrintWriter(socket.getOutputStream());

                do
                {
                    var line = rdr.readLine();
                    if (line == null || line.Length == 0)
                        break;

                    wrt.println(line);
                    wrt.flush();
                } while (true);

                rdr.close();
                wrt.close();
                socket.close();

                global::java.lang.System.@out.println("EchoSocketClientThread exited");
            }

        }

        [TestMethod]
        public void EchoTest()
        {
            var l = new List<string>();

            var server = new EchoSocketServerThread(0);
            server.Start();

            var socket = new Socket(InetAddress.getLocalHost(), server.Port);
            socket.setSoTimeout(15000);
            var wrt = new PrintStream(socket.getOutputStream());
            var rdr = new BufferedReader(new InputStreamReader(socket.getInputStream()));

            // send a handful of test messages
            wrt.println("MESSAGEA");
            wrt.println("MESSAGEB");
            wrt.println("MESSAGEC");
            wrt.println();
            wrt.flush();

            try
            {
                do
                {
                    var line = rdr.readLine();
                    if (line == null || line.Length == 0)
                        break;

                    l.Add(line);
                } while (true);
            }
            catch (SocketTimeoutException)
            {

            }

            rdr.close();
            wrt.close();
            socket.close();
            server.Stop();
            server.join();

            l.Should().HaveCount(3);
            l[0].Should().Be("MESSAGEA");
            l[1].Should().Be("MESSAGEB");
            l[2].Should().Be("MESSAGEC");
        }

        [TestMethod]
        public void ReuseAddressShouldWork()
        {
            var s1 = new ServerSocket();
            s1.setReuseAddress(true);
            s1.getReuseAddress().Should().BeTrue();
            s1.setReuseAddress(false);
            s1.getReuseAddress().Should().BeFalse();
            s1.close();
        }

        [TestMethod]
        [ExpectedException(typeof(BindException))]
        public void BindingToSamePortShouldThrowBindException()
        {
            var s1 = new ServerSocket();
            s1.setReuseAddress(false);
            s1.bind(new InetSocketAddress(0));
            var s2 = new ServerSocket();
            s2.bind(new InetSocketAddress(s1.getLocalPort()));

            s2.close();
            s1.close();
        }

        [TestMethod]
        public void AddressShouldMatchAfterClose()
        {
            var ss = new ServerSocket(0, 0, null);
            var ssInetAddress = ss.getInetAddress();
            var ssLocalPort = ss.getLocalPort();
            var ssLocalSocketAddress = ss.getLocalSocketAddress();
            ss.close();

            ss.getInetAddress().Should().Be(ssInetAddress);
            ss.getLocalPort().Should().Be(ssLocalPort);
            ss.getLocalSocketAddress().Should().Be(ssLocalSocketAddress);
            ss.isBound().Should().BeTrue();
        }

        [TestMethod]
        public void ShouldAbortCancelWhenClosed()
        {
            var ss = new ServerSocket(0);
            var ex = (global::java.net.SocketException)null;

            var task = Task.Run(() =>
            {
                try
                {
                    ss.setSoTimeout(10000);
                    ss.accept();
                }
                catch (SocketException e)
                {
                    ex = e;
                }
            });

            Thread.sleep(2000);
            ss.close();
            Thread.sleep(1000);
            task.Wait();

            ex.Should().BeAssignableTo<SocketException>();
        }

    }

}