﻿using System.Linq;

using FluentAssertions;

using java.lang;
using java.net;
using java.util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.net
{

    [TestClass]
    public class SocketTests
    {

        [TestMethod]
        public void ReuseAddressShouldWork()
        {
            using var s1 = new Socket();
            s1.getReuseAddress().Should().BeFalse();
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
            using var s1 = new ServerSocket();
            s1.bind(new InetSocketAddress(0));
            using var s2 = new Socket();
            s2.setReuseAddress(false);
            s2.bind(new InetSocketAddress(s1.getLocalPort()));
            s2.close();
            s1.close();
        }

        [TestMethod]
        [ExpectedException(typeof(SocketTimeoutException))]
        public void AcceptShouldTimeOut()
        {
            using var server = new ServerSocket(0);
            server.setSoTimeout(5000);
            server.accept();
            throw new System.Exception("accept should not have returned");
        }

        [TestMethod]
        public void ConnectShouldNotTimeOut()
        {
            using var server = new ServerSocket(0);
            server.setSoTimeout(5000);
            int port = server.getLocalPort();

            var dest = NetworkInterface.getNetworkInterfaces()
                .AsEnumerable<NetworkInterface>()
                .Where(i => i.isUp())
                .SelectMany(i => i.getInterfaceAddresses().AsEnumerable<InterfaceAddress>())
                .Select(i => i.getAddress())
                .OfType<Inet4Address>()
                .FirstOrDefault(i => i.isLoopbackAddress() == false && !i.isAnyLocalAddress());

            using var c1 = new Socket();
            c1.connect(new InetSocketAddress(dest, port), 1000);
            using var s1 = server.accept();
        }

        [TestMethod]
        [ExpectedException(typeof(ConnectException))]
        public void ConnectWithNoListeningServerShouldThrowConnectException()
        {
            using var server = new ServerSocket(0);
            int port = server.getLocalPort();
            server.close();
            using var client = new Socket();
            client.connect(new InetSocketAddress(InetAddress.getByName("127.0.0.1"), port), 8888);
        }

    }

}
