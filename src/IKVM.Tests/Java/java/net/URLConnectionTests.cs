using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using java.lang;
using java.net;

using jdk.nashorn.@internal.objects;

using Microsoft.VisualStudio.TestTools.UnitTesting;


using sun.net.www;

namespace IKVM.Tests.Java.java.net
{

    [TestClass]
    public class URLConnectionTests
    {

        [TestMethod]
        public void ShouldThrowConnectException()
        {
            // get random unallocated port
            using var ss = new ServerSocket(0);
            var url = new URL("http://localhost:" + ss.getLocalPort() + "/");
            ss.close();

            var uc = url.openConnection();
            uc.getHeaderFieldKey(20);
            global::java.lang.Exception ex = null;

            try
            {
                // should throw
                uc.getInputStream();
                throw new System.Exception();
            }
            catch (global::java.lang.Exception e)
            {
                ex = e;
            }

            ex.Should().BeOfType<ConnectException>();
            ex.getMessage().Should().NotBeNull();
            ex.getCause().Should().NotBeNull();
        }

        [TestMethod]
        public void CanHandleHttpRequest()
        {
            using var ss = new ServerSocket(0);
            ss.setSoTimeout(1000);
            var port = ss.getLocalPort();
            var stop = new CancellationTokenSource();

            var task = Task.Run(() =>
            {
                while (stop.IsCancellationRequested == false)
                {
                    try
                    {
                        var reply200 = "HTTP/1.1 200 OK\r\n" + "Content-Length: 0\r\n\r\n";
                        var reply404 = "HTTP/1.1 404 Not Found\r\n\r\n";

                        using var sock = ss.accept();
                        var ins = sock.getInputStream();
                        var ops = sock.getOutputStream();

                        var headers = new MessageHeader(ins);
                        var requestLine = headers.getValue(0);

                        var first = requestLine.IndexOf(' ');
                        var second = requestLine.LastIndexOf(' ');
                        var uri = requestLine.Substring(first + 1, second - first - 1);

                        if (uri == "/content")
                            ops.write(Encoding.UTF8.GetBytes(reply200));
                        else
                            ops.write(Encoding.UTF8.GetBytes(reply404));
                    }
                    catch (SocketTimeoutException)
                    {
                        // ignore
                    }
                }
            });

            var contentRequest = (HttpURLConnection)new URL($"http://localhost:{ss.getLocalPort()}/content").openConnection();
            var contentResponse = contentRequest.getResponseCode();
            contentResponse.Should().Be(200);

            var missingRequest = (HttpURLConnection)new URL($"http://localhost:{ss.getLocalPort()}/missing").openConnection();
            var missingResponse = missingRequest.getResponseCode();
            missingResponse.Should().Be(404);

            stop.Cancel();
            task.Wait();
            ss.close();
        }

    }
}
