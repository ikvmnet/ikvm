using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using java.io;
using java.lang;
using java.net;

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

        [TestMethod]
        public void CanCloseKeepAliveStreamWithWrongContentLength()
        {
            var serversocket = new ServerSocket(0);
            var port = serversocket.getLocalPort();

            var task = Task.Run(() =>
            {
                OutputStream ost = null;

                try
                {
                    var s = serversocket.accept();
                    var ist = s.getInputStream();

                    // read the first ten bytes
                    for (int i = 0; i < 10; i++)
                        ist.read();

                    var ow = new OutputStreamWriter(ost = s.getOutputStream());
                    ow.write("HTTP/1.0 200 OK\n");

                    // Note: The client expects 10 bytes.
                    ow.write("Content-Length: 10\n");
                    ow.write("Content-Type: text/html\n");

                    // Note: If this line is missing, everything works fine.
                    ow.write("Connection: Keep-Alive\n");
                    ow.write("\n");

                    // Note: The (buggy) server only sends 9 bytes.
                    ow.write("123456789");
                    ow.flush();
                }
                catch (global::java.lang.Exception)
                {

                }
                finally
                {
                    try
                    {
                        ost?.close();
                    }
                    catch (global::java.io.IOException)
                    {

                    }
                }
            });

            try
            {
                var url = new URL("http://localhost:" + port);
                var urlc = (HttpURLConnection)url.openConnection();
                var st = urlc.getInputStream();
                int c = 0;
                while (c != -1)
                {
                    try
                    {
                        c = st.read();
                    }
                    catch (global::java.io.IOException)
                    {
                        st.read();
                        break;
                    }
                }

                st.close();
            }
            catch (global::java.io.IOException e)
            {
                return;
            }
            catch (global::java.lang.NullPointerException e)
            {
                throw new RuntimeException(e);
            }
            finally
            {
                task.Wait();

                if (serversocket != null)
                    serversocket.close();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ConnectException))]
        public void ConnectToBeURLShouldThrowConnectException()
        {
            new URL("http://localhost:8812").getContent();
        }

    }

}
