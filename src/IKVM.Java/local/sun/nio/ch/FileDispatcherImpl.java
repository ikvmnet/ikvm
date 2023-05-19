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

    native int read(FileDescriptor fd, long address, int len) throws IOException;

    native int pread(FileDescriptor fd, long address, int len, long position) throws IOException;

    native long readv(FileDescriptor fd, long address, int len) throws IOException;

    int write(FileDescriptor fd, long address, int len) throws IOException {
        return write0(fd, address, len, append);
    }

    native int write0(FileDescriptor fd, long address, int len, boolean append) throws IOException;

    native int pwrite(FileDescriptor fd, long address, int len, long position) throws IOException;

    long writev(FileDescriptor fd, long address, int len) throws IOException {
        return writev0(fd, address, len, append);
    }

    native long writev0(FileDescriptor fd, long address, int len, boolean append) throws IOException;

    native int force(FileDescriptor fd, boolean metaData) throws IOException;

    native int truncate(FileDescriptor fd, long size) throws IOException;

    native long size(FileDescriptor fd) throws IOException;

    native int lock(FileDescriptor fd, boolean blocking, long pos, long size, boolean shared) throws IOException;

    native void release(FileDescriptor fd, long pos, long size) throws IOException;

    native void close(FileDescriptor fd) throws IOException;

    native FileDescriptor duplicateForMapping(FileDescriptor fd) throws IOException;

    boolean canTransferToDirectly(java.nio.channels.SelectableChannel sc) {
        return fastFileTransfer && sc.isBlocking();
    }

    boolean transferToDirectlyNeedsPositionLock() {
        return true;
    }

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
