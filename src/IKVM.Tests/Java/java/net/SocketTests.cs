using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using java.lang;
using java.net;
using java.util;

using javax.naming.directory;
using javax.naming;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.net
{

    [TestClass]
    public class SocketTests
    {

        [TestMethod]
        public void ReuseAddressShouldWork()
        {
            var s1 = new Socket();
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
            var s1 = new Socket();
            s1.bind(new InetSocketAddress(0));
            var s2 = new Socket();
            s2.bind(new InetSocketAddress(s1.getLocalPort()));
            s2.close();
            s1.close();
        }

        [TestMethod]
        [ExpectedException(typeof(SocketTimeoutException))]
        public void AcceptShouldTimeOut()
        {
            var server = new ServerSocket(0);
            server.setSoTimeout(5000);
            server.accept();
            throw new System.Exception("accept should not have returned");
        }

        [TestMethod]
        public void ConnectShouldNotTimeOut()
        {
            var server = new ServerSocket(0);
            server.setSoTimeout(5000);
            int port = server.getLocalPort();

            var dest = NetworkInterface.getNetworkInterfaces()
                .AsEnumerable<NetworkInterface>()
                .Where(i => i.isUp())
                .SelectMany(i => i.getInterfaceAddresses().AsEnumerable<InterfaceAddress>())
                .Select(i => i.getAddress())
                .OfType<Inet4Address>()
                .FirstOrDefault(i => i.isLoopbackAddress() == false && !i.isAnyLocalAddress());

            var c1 = new Socket();
            c1.connect(new InetSocketAddress(dest, port), 1000);
            var s1 = server.accept();
        }

        [TestMethod]
        [ExpectedException(typeof(ConnectException))]
        public void ConnectWithNoListeningServerShouldThrowConnectException()
        {
            var server = new ServerSocket(0);
            int port = server.getLocalPort();
            server.close();
            var client = new Socket();
            client.connect(new InetSocketAddress(InetAddress.getByName("127.0.0.1"), port), 2000);
        }

        [TestMethod]
        public void SendAfterClose()
        {
            var server = new ServerSocket(0);
            int port = server.getLocalPort();

            var task = Task.Run(() =>
            {
                try
                {
                    var socket = server.accept();
                    var ist = socket.getInputStream();

                    // read the request
                    while (ist.read() != -1)
                    {
                        ist.skip(ist.available());
                        break;
                    }

                    var ost = socket.getOutputStream();
                    ost.write(new byte[] { 0x01 });
                    ost.flush();

                    // read remainder (this might fail but shouldn't)
                    while (ist.read() != -1)
                    {
                        ist.skip(ist.available());
                        break;
                    }

                    ist.close();
                    ost.close();
                    socket.close();
                    server.close();
                }
                catch (global::java.net.SocketException)
                {
                    // ignore
                }
            });

            // set up the environment for creating the initial context
            global::java.util.Hashtable env = new global::java.util.Hashtable();
            env.put(Context.INITIAL_CONTEXT_FACTORY, "com.sun.jndi.ldap.LdapCtxFactory");
            env.put(Context.PROVIDER_URL, "ldap://localhost:" + server.getLocalPort());
            env.put("com.sun.jndi.ldap.read.timeout", "1000");

            // env.put(Context.SECURITY_AUTHENTICATION, "simple");
            // env.put(Context.SECURITY_PRINCIPAL,"cn=root");
            // env.put(Context.SECURITY_CREDENTIALS,"root");

            // create initial context
            DirContext context = new InitialDirContext(env);

            // searching
            SearchControls scs = new SearchControls();
            scs.setSearchScope(SearchControls.SUBTREE_SCOPE);

            try
            {
                NamingEnumeration answer = context.search("o=sun,c=us", "(&(cn=Bob)))", scs);
            }
            catch (InvalidSearchFilterException isfe)
            {
                // ignore, it is the expected filter exception.
                System.Console.Error.WriteLine("Expected exception: " + isfe.getMessage());
            }
            catch (NamingException ne)
            {
                // maybe a read timeout exception, as the server does not response.
                throw new Exception("Expect a InvalidSearchFilterException", ne);
            }

            try
            {
                NamingEnumeration answer = context.search("o=sun,c=us", ")(&(cn=Bob)", scs);
            }
            catch (InvalidSearchFilterException isfe)
            {
                // ignore, it is the expected filter exception.
                System.Console.Error.WriteLine("Expected exception: " + isfe.getMessage());
            }
            catch (NamingException ne)
            {
                // maybe a read timeout exception, as the server does not response.
                throw new Exception("Expect a InvalidSearchFilterException", ne);
            }

            try
            {
                NamingEnumeration answer = context.search("o=sun,c=us", "(&(cn=Bob))", scs);
            }
            catch (InvalidSearchFilterException isfe)
            {
                // ignore, it is the expected filter exception.
                throw new Exception("Unexpected ISFE", isfe);
            }
            catch (NamingException ne)
            {
                // maybe a read timeout exception, as the server does not response.
                System.Console.Error.WriteLine("Expected exception: " + ne.getMessage());
            }

            context.close();
            task.Wait();
        }

    }

}
