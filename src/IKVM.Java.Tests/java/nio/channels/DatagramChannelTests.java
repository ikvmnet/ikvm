package ikvm.tests.java.java.nio.channels;

import java.io.*;
import java.net.*;
import java.nio.*;
import java.nio.channels.*;
import java.nio.charset.*;

@cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute.Annotation()
public class DatagramChannelTests {
    
    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canConnectAndSendAndReceive() throws Throwable {
        if (cli.System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(cli.System.Runtime.InteropServices.OSPlatform.get_OSX()) {
            // https://bugs.openjdk.org/browse/JDK-8285515 waiting on 8u341
            return;
        }

        Reactor r = new Reactor();
        Actor a = new Actor(r.port());
        invoke(a, r);
    }

    static void invoke(Sprintable reader, Sprintable writer) throws Exception {

        Thread writerThread = new Thread(writer);
        writerThread.start();

        Thread readerThread = new Thread(reader);
        readerThread.start();

        writerThread.join();
        readerThread.join();

        reader.throwException();
        writer.throwException();
    }

    public interface Sprintable extends Runnable {
        public void throwException() throws Exception;
    }

    public static class Actor implements Sprintable {
        final int port;
        Exception e = null;

        Actor(int port) {
            this.port = port;
        }

        public void throwException() throws Exception {
            if (e != null)
                throw e;
        }

        public void run() {
            try {
                DatagramChannel dc = DatagramChannel.open();

                // Send a message
                ByteBuffer bb = ByteBuffer.allocateDirect(256);
                bb.put("hello".getBytes());
                bb.flip();
                InetAddress address = InetAddress.getLocalHost();
                if (address.isLoopbackAddress()) {
                    address = InetAddress.getLoopbackAddress();
                }
                InetSocketAddress isa = new InetSocketAddress(address, port);
                dc.connect(isa);
                dc.write(bb);

                // Try to send to some other address
                address = InetAddress.getLocalHost();
                InetSocketAddress bogus = new InetSocketAddress(address, 3333);
                try {
                    dc.send(bb, bogus);
                    throw new RuntimeException("Allowed bogus send while connected");
                } catch (IllegalArgumentException iae) {
                    // Correct behavior
                }

                // Read a reply
                bb.flip();
                dc.read(bb);
                bb.flip();
                CharBuffer cb = Charset.forName("US-ASCII").
                newDecoder().decode(bb);

                // Clean up
                dc.disconnect();
                dc.close();
            } catch (Exception ex) {
                e = ex;
            }
        }
    }

    public static class Reactor implements Sprintable {
        final DatagramChannel dc;
        Exception e = null;

        Reactor() throws IOException {
            dc = DatagramChannel.open().bind(new InetSocketAddress(0));
        }

        int port() {
            return dc.socket().getLocalPort();
        }

        public void throwException() throws Exception {
            if (e != null)
                throw e;
        }

        public void run() {
            try {
                // Listen for a message
                ByteBuffer bb = ByteBuffer.allocateDirect(100);
                SocketAddress sa = dc.receive(bb);
                bb.flip();
                CharBuffer cb = Charset.forName("US-ASCII").
                newDecoder().decode(bb);

                // Reply to sender
                dc.connect(sa);
                bb.flip();
                dc.write(bb);

                // Clean up
                dc.disconnect();
                dc.close();
            } catch (Exception ex) {
                e = ex;
            }
        }
    }

}
