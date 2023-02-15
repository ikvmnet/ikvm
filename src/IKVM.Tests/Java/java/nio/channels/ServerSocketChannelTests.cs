using System.Threading;

using java.net;
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
            var ssc = ServerSocketChannel.open();
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
            var ssc = ServerSocketChannel.open();
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
            var sc = SocketChannel.open();
            sc.configureBlocking(false);
            sc.connect(ss.getLocalSocketAddress());
            Thread.Sleep(100);
            ss.accept();
        }

        [TestMethod]
        public void Foo()
        {

        }


    }

}
