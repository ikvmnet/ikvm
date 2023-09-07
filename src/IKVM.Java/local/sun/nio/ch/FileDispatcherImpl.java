package sun.nio.ch;

import java.io.FileDescriptor;
import java.io.IOException;
import java.security.PrivilegedAction;

import sun.misc.SharedSecrets;
import sun.misc.JavaIOFileDescriptorAccess;

class FileDispatcherImpl extends FileDispatcher {

    // set to true if fast file transmission (TransmitFile) is enabled
    private static final boolean fastFileTransfer;

    /**
     * Indicates if the dispatcher should first advance the file position
     * to the end of file when writing.
     */
    private final boolean append;

    FileDispatcherImpl(boolean append) {
        this.append = append;
    }

    FileDispatcherImpl() {
        this(false);
    }

    @Override
    boolean needsPositionLock() {
        return true;
    }

    int read(FileDescriptor fd, long address, int len) throws IOException {
        return read0(fd, address, len);
    }

    static native int read0(FileDescriptor fd, long address, int len) throws IOException;

    int pread(FileDescriptor fd, long address, int len, long position) throws IOException {
        return pread0(fd, address, len, position);
    }

    static native int pread0(FileDescriptor fd, long address, int len, long position) throws IOException;

    long readv(FileDescriptor fd, long address, int len) throws IOException {
        return readv0(fd, address, len);
    }

    static native long readv0(FileDescriptor fd, long address, int len) throws IOException;

    int write(FileDescriptor fd, long address, int len) throws IOException {
        return write0(fd, address, len, append);
    }

    static native int write0(FileDescriptor fd, long address, int len, boolean append) throws IOException;

    int pwrite(FileDescriptor fd, long address, int len, long position) throws IOException {
        return pwrite0(fd, address, len, position);
    }

    static native int pwrite0(FileDescriptor fd, long address, int len, long position) throws IOException;

    long writev(FileDescriptor fd, long address, int len) throws IOException {
        return writev0(fd, address, len, append);
    }

    static native long writev0(FileDescriptor fd, long address, int len, boolean append) throws IOException;

    int force(FileDescriptor fd, boolean metaData) throws IOException {
        return force0(fd, metaData);
    }

    static native int force0(FileDescriptor fd, boolean metaData) throws IOException;

    int truncate(FileDescriptor fd, long size) throws IOException {
        return truncate0(fd, size);
    }

    static native int truncate0(FileDescriptor fd, long size) throws IOException;

    long size(FileDescriptor fd) throws IOException {
        return size0(fd);
    }

    static native long size0(FileDescriptor fd) throws IOException;

    int lock(FileDescriptor fd, boolean blocking, long pos, long size, boolean shared) throws IOException {
        return lock0(fd, blocking, pos, size, shared);
    }

    static native int lock0(FileDescriptor fd, boolean blocking, long pos, long size, boolean shared) throws IOException;

    void release(FileDescriptor fd, long pos, long size) throws IOException {
        release0(fd, pos, size);
    }

    static native void release0(FileDescriptor fd, long pos, long size) throws IOException;

    void close(FileDescriptor fd) throws IOException {
        close0(fd);
    }

    static native void close0(FileDescriptor fd) throws IOException;

    native FileDescriptor duplicateForMapping(FileDescriptor fd) throws IOException;

    boolean canTransferToDirectly(java.nio.channels.SelectableChannel sc) {
        return fastFileTransfer && sc.isBlocking();
    }

    boolean transferToDirectlyNeedsPositionLock() {
        return transferToDirectlyNeedsPositionLock0();
    }

    static native boolean transferToDirectlyNeedsPositionLock0();

    static boolean isFastFileTransferRequested() {
        String fileTransferProp = java.security.AccessController.doPrivileged(new PrivilegedAction<String>() {
            @Override
            public String run() {
                return System.getProperty("jdk.nio.enableFastFileTransfer");
            }
        });

        boolean enable;
        if ("".equals(fileTransferProp)) {
            enable = true;
        } else {
            enable = Boolean.parseBoolean(fileTransferProp);
        }

        return enable;
    }

    static {
        IOUtil.load();
        fastFileTransfer = isFastFileTransferRequested();
    }

}
