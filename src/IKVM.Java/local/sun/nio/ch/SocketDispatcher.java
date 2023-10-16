package sun.nio.ch;

import java.io.*;

class SocketDispatcher extends NativeDispatcher {

    static {
        IOUtil.load();
    }

    native int read(FileDescriptor fd, long address, int len) throws IOException;

    native long readv(FileDescriptor fd, long address, int len) throws IOException;

    native int write(FileDescriptor fd, long address, int len) throws IOException;

    native long writev(FileDescriptor fd, long address, int len) throws IOException;

    native void preClose(FileDescriptor fd) throws IOException;

    native void close(FileDescriptor fd) throws IOException;

}
