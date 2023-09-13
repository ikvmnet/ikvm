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
        class EchoSocketServer : global::java.lang.Thread
        {

            int port;
            global::java.net.ServerSocket serverSocket;
            bool running = false;

            /// <summary>
            /// Gets the current port.
            /// </summary>
            public int Port => port;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="port"></param>
            /// <param name="onLine"></param>
            public EchoSocketServer(int port)
            {
                this.port = port;
            }

            /// <summary>
            /// Starts the server.
            /// </summary>
            public void Start()
            {
                try
                {
                    serverSocket = new ServerSocket(port);
                    port = serverSocket.getLocalPort();
                    serverSocket.setSoTimeout(15000);
                    base.start();
                }
                catch (IOException e)
                {
                    e.printStackTrace();
                }
            }

            public void Stop()
            {
                running = false;
                base.interrupt();
            }

            public override void run()
            {
                try
                {
                    running = true;

                    while (running)
                    {
                        try
                        {
                            new ClientHandler(serverSocket.accept()).start();
                        }
                        catch (IOException e)
                        {
                            e.printStackTrace();
                        }
                    }
                }
                finally
                {
                    serverSocket.close();
                }
            }
        }

        /// <summary>
        /// Handles a single client.
        /// </summary>
        class ClientHandler : global::java.lang.Thread
        {

            readonly global::java.net.Socket socket;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="socket"></param>
            /// <param name="onLine"></param>
            public ClientHandler(global::java.net.Socket socket)
            {
                this.socket = socket ?? throw new ArgumentNullException(nameof(socket));
            }

            public override void run()
            {
                var rdr = new global::java.io.BufferedReader(new global::java.io.InputStreamReader(socket.getInputStream()));
                var wrt = new global::java.io.PrintWriter(socket.getOutputStream());

                wrt.println("HELLO");
                wrt.flush();

                var line = rdr.readLine();
                while (line != null && line.Length > 0)
                {
                    wrt.println("RECEIVED " + line);
                    wrt.flush();
                    line = rdr.readLine();
                }

                wrt.println("GOODBYE");
                wrt.flush();

                rdr.close();
                wrt.close();
                socket.close();
            }
        }

        [TestMethod]
        public void EchoTest()
        {
            var l = new List<string>();

            var server = new EchoSocketServer(0);
            server.Start();

            var socket = new Socket("localhost", server.Port);
            socket.setSoTimeout(15000);
            var wrt = new PrintStream(socket.getOutputStream());
            var rdr = new BufferedReader(new InputStreamReader(socket.getInputStream()));

            // send a handful of test messages
            wrt.println("MESSAGEA");
            wrt.println("MESSAGEB");
            wrt.println("MESSAGEC");
            wrt.println("");

            // read responses
            var line = rdr.readLine();
            while (line != null)
            {
                l.Add(line);
                line = rdr.readLine();
            }

            rdr.close();
            wrt.close();
            socket.close();
            server.Stop();

            l.Should().HaveCount(5);
            l[0].Should().Be("HELLO");
            l[1].Should().Be("RECEIVED MESSAGEA");
            l[2].Should().Be("RECEIVED MESSAGEB");
            l[3].Should().Be("RECEIVED MESSAGEC");
            l[4].Should().Be("GOODBYE");
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
                    ss.setSoTimeout(60000);
                    ss.accept();
                }
                catch (SocketException e)
                {
                    ex = e;
                }
            });

            Thread.sleep(1000);
            ss.close();
            Thread.sleep(1000);
            task.Wait();

            ex.Should().BeAssignableTo<SocketException>();
        }

    }

}