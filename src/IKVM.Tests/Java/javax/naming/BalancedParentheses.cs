using java.io;
using java.lang;
using java.net;

using javax.naming;
using javax.naming.directory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.javax.naming
{

    [TestClass]
    public class BalancedParentheses
    {

        [TestMethod]
        public void BalancedParenthesesTest()
        {
            if (separateServerThread)
            {
                startServer(true);
                startClient(false);
            }
            else
            {
                startClient(true);
                startServer(false);
            }

            /*
             * Wait for other side to close down.
             */
            if (separateServerThread)
            {
                serverThread.join();
            }
            else
            {
                clientThread.join();
            }

            /*
             * When we get here, the test is pretty much over.
             *
             * If the main thread excepted, that propagates back
             * immediately.  If the other thread threw an exception, we
             * should report back.
             */
            if (serverException != null)
            {
                global::java.lang.System.@out.print("Server Exception:");
                throw serverException;
            }
            if (clientException != null)
            {
                global::java.lang.System.@out.print("Client Exception:");
                throw clientException;
            }
        }

        // Should we run the client or server in a separate thread?
        //
        // Both sides can throw exceptions, but do you have a preference
        // as to which side should be the main thread.
        static bool separateServerThread = true;

        // use any free port by default
        volatile int serverPort = 0;

        // Is the server ready to serve?
        volatile bool serverReady = false;

        // Define the server side of the test.
        //
        // If the server prematurely exits, serverReady will be set to true
        // to avoid infinite hangs.
        void doServerSide()
        {
            ServerSocket serverSock = new ServerSocket(serverPort);

            // signal client, it's ready to accecpt connection
            serverPort = serverSock.getLocalPort();
            serverReady = true;

            // accept a connection
            Socket socket = serverSock.accept();
            global::java.lang.System.@out.println("Server: Connection accepted");

            InputStream @is = socket.getInputStream();
            OutputStream os = socket.getOutputStream();

            // read the bindRequest
            while (@is.read() != -1)
            {
                // ignore
                @is.skip(@is.available());
                break;
            }

            byte[] bindResponse = {0x30, 0x0C, 0x02, 0x01, 0x01, 0x61, 0x07, 0x0A,
                               0x01, 0x00, 0x04, 0x00, 0x04, 0x00};
            // write bindResponse
            os.write(bindResponse);
            os.flush();

            // ignore any more request.
            while (@is.read() != -1)
            {
                // ignore
                @is.skip(@is.available());
            }

            @is.close();
            os.close();
            socket.close();
            serverSock.close();
        }

        //  Define the client side of the test.
        //
        // If the server prematurely exits, serverReady will be set to true
        // to avoid infinite hangs.
        void doClientSide()
        {
            // Wait for server to get started.
            while (!serverReady)
            {
                Thread.sleep(50);
            }

            // set up the environment for creating the initial context
            global::java.util.Hashtable env = new global::java.util.Hashtable();
            env.put(Context.INITIAL_CONTEXT_FACTORY, "com.sun.jndi.ldap.LdapCtxFactory");
            env.put(Context.PROVIDER_URL, "ldap://localhost:" + serverPort);
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
                global::java.lang.System.@out.println("Expected exception: " + isfe.getMessage());
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
                global::java.lang.System.@out.println("Expected exception: " + isfe.getMessage());
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
                global::java.lang.System.@out.println("Expected exception: " + ne.getMessage());
            }

            context.close();
        }

        class ServerThread : Thread
        {

            readonly BalancedParentheses host;

            public ServerThread(BalancedParentheses host)
            {
                this.host = host;
            }

            public override void run()
            {
                try
                {
                    host.doServerSide();
                }
                catch (Exception e)
                {
                    /*
                     * Our server thread just died.
                     *
                     * Release the client, if not active already...
                     */
                    global::java.lang.System.err.println("Server died...");
                    global::java.lang.System.err.println(e);
                    host.serverReady = true;
                    host.serverException = e;
                }
            }

        }

        /*
         * ============================================================
         * The remainder is just support stuff
         */

        // client and server thread
        Thread clientThread = null;
        Thread serverThread = null;

        // client and server exceptions
        volatile Exception serverException = null;
        volatile Exception clientException = null;

        void startServer(bool newThread)
        {
            if (newThread)
            {
                serverThread = new ServerThread(this);
                serverThread.start();
            }
            else
            {
                doServerSide();
            }
        }

        class ClientThread : Thread
        {

            readonly BalancedParentheses host;

            public ClientThread(BalancedParentheses host)
            {
                this.host = host;
            }

            public override void run()
            {
                try
                {
                    host.doClientSide();
                }
                catch (Exception e)
                {
                    /*
                     * Our client thread just died.
                     */
                    global::java.lang.System.err.println("Client died...");
                    host.clientException = e;
                }
            }

        }

        void startClient(bool newThread)
        {
            if (newThread)
            {
                clientThread = new ClientThread(this);
                clientThread.start();
            }
            else
            {
                doClientSide();
            }
        }

    }

}
