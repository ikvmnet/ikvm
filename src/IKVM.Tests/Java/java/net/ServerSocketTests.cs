using System;
using System.Collections.Generic;

using FluentAssertions;

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

            readonly int port;

            global::java.net.ServerSocket serverSocket;
            bool running = false;

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
                    serverSocket = new global::java.net.ServerSocket(port);
                    base.start();
                }
                catch (global::java.io.IOException e)
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
                        catch (global::java.io.IOException e)
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
        public void Can_listen()
        {
            var l = new List<string>();

            var server = new EchoSocketServer(51041);
            server.Start();

            var socket = new global::java.net.Socket("localhost", 51041);
            var wrt = new global::java.io.PrintStream(socket.getOutputStream());
            var rdr = new global::java.io.BufferedReader(new global::java.io.InputStreamReader(socket.getInputStream()));

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

    }

}