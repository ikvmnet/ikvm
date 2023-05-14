package ikvm.tests.java.java.nio.channels;

import java.net.*;
import java.io.*;
import java.nio.channels.*;
import java.util.Random;

@cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute.Annotation()
public class AsynchronousServerSocketChannelTests {

    static final Random rand = new Random();
    
    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canReadAndWriteStreams() throws Exception {
        // establish loopback connection
        AsynchronousServerSocketChannel listener = AsynchronousServerSocketChannel.open().bind(new InetSocketAddress(0));
        int port = ((InetSocketAddress)(listener.getLocalAddress())).getPort();
        InetSocketAddress isa = new InetSocketAddress(InetAddress.getLocalHost(), port);
        AsynchronousSocketChannel ch1 = AsynchronousSocketChannel.open();
        ch1.connect(isa).get();
        AsynchronousSocketChannel ch2 = listener.accept().get();

        // start thread to write to stream
        Writer writer = new Writer(Channels.newOutputStream(ch1));
        Thread writerThread = new Thread(writer);
        writerThread.start();

        // start thread to read from stream
        Reader reader = new Reader(Channels.newInputStream(ch2));
        Thread readerThread = new Thread(reader);
        readerThread.start();

        // wait for threads to complete
        writerThread.join();
        readerThread.join();

        // shutdown listener
        listener.close();

        // check that reader received what we expected
        if (reader.total() != writer.total())
            throw new RuntimeException("Unexpected number of bytes read");
        if (reader.hash() != writer.hash())
            throw new RuntimeException("Hash incorrect for bytes read");

        // channels should be closed
        if (ch1.isOpen() || ch2.isOpen())
            throw new RuntimeException("Channels should be closed");
    }

    static class Reader implements Runnable {

        private final InputStream in;
        private volatile int total;
        private volatile int hash;

        Reader(InputStream in) {
            this.in = in;
        }

        public void run() {
            try {
                int n;
                do {
                    // random offset/len
                    byte[] buf = new byte[128 + rand.nextInt(128)];
                    int len, off;
                    if (rand.nextBoolean()) {
                        len = buf.length;
                        off = 0;
                        n = in.read(buf);
                    } else {
                        len = 1 + rand.nextInt(64);
                        off = rand.nextInt(64);
                        n = in.read(buf, off, len);
                    }
                    if (n > len)
                        throw new RuntimeException("Too many bytes read");
                    if (n > 0) {
                        total += n;
                        for (int i=0; i<n; i++) {
                            int value = buf[off + i];
                            hash = hash ^ value;
                        }
                    }
                } while (n > 0);
                in.close();
            } catch (IOException x) {
                x.printStackTrace();
            }
        }

        int total() {
            return total;
        }

        int hash() {
            return hash;
        }

    }

    static class Writer implements Runnable {

        private final OutputStream out;
        private final int total;
        private volatile int hash;

        Writer(OutputStream out) {
            this.out = out;
            this.total = 50*1000 + rand.nextInt(50*1000);
        }

        public void run() {
            hash = 0;
            int rem = total;
            try {
                do {
                    byte[] buf = new byte[1 + rand.nextInt(rem)];
                    int off, len;

                    // write random bytes
                    if (rand.nextBoolean()) {
                        off = 0;
                        len = buf.length;
                    } else {
                        off = rand.nextInt(buf.length);
                        int r = buf.length - off;
                        len = (r <= 1) ? 1 : (1 + rand.nextInt(r));
                    }
                    for (int i=0; i<len; i++) {
                        byte value = (byte)rand.nextInt(256);
                        buf[off + i] = value;
                        hash = hash ^ value;
                    }
                    if ((off == 0) && (len == buf.length)) {
                        out.write(buf);
                    } else {
                        out.write(buf, off, len);
                    }
                    rem -= len;
                } while (rem > 0);

                // close stream when done
                out.close();

            } catch (IOException x) {
                x.printStackTrace();
            }
        }

        int total() {
            return total;
        }

        int hash() {
            return hash;
        }

    }

}
