using System;

using com.sun.net.httpserver;

using java.lang;
using java.net;
using java.util.concurrent;
using java.util.logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.com.sun.net.httpserver
{

    [TestClass]
    public class HttpServerTests
    {

        class ActionHandler : HttpHandler
        {

            readonly Action<HttpExchange> action;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="action"></param>
            public ActionHandler(Action<HttpExchange> action)
            {
                this.action = action;
            }

            public void handle(HttpExchange t) => action(t);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="RuntimeException"></exception>
        [TestMethod]
        public void B6886436()
        {
            var logger = Logger.getLogger("com.sun.net.httpserver");
            var c = new ConsoleHandler();
            c.setLevel(Level.WARNING);
            logger.addHandler(c);
            logger.setLevel(Level.WARNING);

            var server = HttpServer.create(new InetSocketAddress(0), 0);

            // handle /test
            var ctx = server.createContext("/test", new ActionHandler(t =>
            {
                var b = t.getRequestBody();
                var h = t.getRequestHeaders();
                var r = t.getResponseHeaders();

                // read entire body but ignore
                while (b.read() != -1) ;
                b.close();

                // send 204 response with no contents
                t.sendResponseHeaders(204, 0);
                t.close();
            }));

            // execute server on a thread pool
            var executor = Executors.newCachedThreadPool();
            server.setExecutor(executor);
            server.start();

            // connet to server
            var url = new URL("http://localhost:" + server.getAddress().getPort() + "/test/foo.html");
            var urlc = (HttpURLConnection)url.openConnection();

            try
            {
                using (var iss = urlc.getInputStream())
                    while (iss.read() != -1) continue;

                // second attempt with timeout
                urlc = (HttpURLConnection)url.openConnection();
                urlc.setReadTimeout(3000);
                using (var iss2 = urlc.getInputStream())
                    while (iss2.read() != -1) continue;
            }
            finally
            {
                server.stop(2);
                executor.shutdown();
            }
        }

    }

}
