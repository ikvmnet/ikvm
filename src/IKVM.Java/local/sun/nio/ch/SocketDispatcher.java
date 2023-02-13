package sun.nio.ch;

import java.io.*;

class SocketDispatcher extends NativeDispatcher {

    static {
        IOUtil.load();
    }

    int read(FileDescriptor fd, long address, int len) throws IOException {
        return read0(fd, address, len);
    }

    long readv(FileDescriptor fd, long address, int len) throws IOException {
        return readv0(fd, address, len);
    }

    int write(FileDescriptor fd, long address, int len) throws IOException {
        return write0(fd, address, len);
    }

    long writev(FileDescriptor fd, long address, int len) throws IOException {
        return writev0(fd, address, len);
    }

    void preClose(FileDescriptor fd) throws IOException {
        preClose0(fd);
    }

    void close(FileDescriptor fd) throws IOException {
        close0(fd);
    }

    static native int read0(FileDescriptor fd, long address, int len) throws IOException;

    static native long readv0(FileDescriptor fd, long address, int len) throws IOException;

    static native int write0(FileDescriptor fd, long address, int len) throws IOException;

    static native long writev0(FileDescriptor fd, long address, int len) throws IOException;

    static native void preClose0(FileDescriptor fd) throws IOException;

    static native void close0(FileDescriptor fd) throws IOException;

}
