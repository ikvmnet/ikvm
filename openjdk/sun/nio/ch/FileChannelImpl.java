/*
 * Copyright (c) 2000, 2013, Oracle and/or its affiliates. All rights reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Oracle designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Oracle in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
 * or visit www.oracle.com if you need additional information or have any
 * questions.
 */

package sun.nio.ch;

import cli.Microsoft.Win32.SafeHandles.SafeFileHandle;
import cli.System.IntPtr;
import cli.System.IO.FileStream;
import cli.System.Runtime.InteropServices.DllImportAttribute;
import java.io.FileDescriptor;
import java.io.IOException;
import java.nio.ByteBuffer;
import java.nio.MappedByteBuffer;
import java.nio.channels.ClosedByInterruptException;
import java.nio.channels.ClosedChannelException;
import java.nio.channels.FileChannel;
import java.nio.channels.FileLock;
import java.nio.channels.FileLockInterruptionException;
import java.nio.channels.NonReadableChannelException;
import java.nio.channels.NonWritableChannelException;
import java.nio.channels.OverlappingFileLockException;
import java.nio.channels.ReadableByteChannel;
import java.nio.channels.WritableByteChannel;
import java.security.AccessController;
import java.util.ArrayList;
import java.util.List;

import sun.misc.Cleaner;
import sun.security.action.GetPropertyAction;

public class FileChannelImpl
    extends FileChannel
{
    private static final boolean win32 = ikvm.internal.Util.WINDOWS;

    // Memory allocation size for mapping buffers
    private static final long allocationGranularity = 64 * 1024;    // HACK we're using a hard coded value here that works on all mainstream platforms

    // Used to make native read and write calls
    private final FileDispatcher nd;

    // File descriptor
    private final FileDescriptor fd;

    // File access mode (immutable)
    private final boolean writable;
    private final boolean readable;
    private final boolean append;

    // Required to prevent finalization of creating stream (immutable)
    private final Object parent;

    // The path of the referenced file
    // (null if the parent stream is created with a file descriptor)
    private final String path;

    // Thread-safe set of IDs of native threads, for signalling
    private final NativeThreadSet threads = new NativeThreadSet(2);

    // Lock for operations involving position and size
    private final Object positionLock = new Object();

    private FileChannelImpl(FileDescriptor fd, String path, boolean readable,
                            boolean writable, boolean append, Object parent)
    {
        this.fd = fd;
        this.readable = readable;
        this.writable = writable;
        this.append = append;
        this.parent = parent;
        this.path = path;
        this.nd = new FileDispatcherImpl(append);
    }

    // Used by FileInputStream.getChannel() and RandomAccessFile.getChannel()
    public static FileChannel open(FileDescriptor fd, String path,
                                   boolean readable, boolean writable,
                                   Object parent)
    {
        return new FileChannelImpl(fd, path, readable, writable, false, parent);
    }

    // Used by FileOutputStream.getChannel
    public static FileChannel open(FileDescriptor fd, String path,
                                   boolean readable, boolean writable,
                                   boolean append, Object parent)
    {
        return new FileChannelImpl(fd, path, readable, writable, append, parent);
    }

    private void ensureOpen() throws IOException {
        if (!isOpen())
            throw new ClosedChannelException();
    }


    // -- Standard channel operations --

    protected void implCloseChannel() throws IOException {
        // Release and invalidate any locks that we still hold
        if (fileLockTable != null) {
            for (FileLock fl: fileLockTable.removeAll()) {
                synchronized (fl) {
                    if (fl.isValid()) {
                        nd.release(fd, fl.position(), fl.size());
                        ((FileLockImpl)fl).invalidate();
                    }
                }
            }
        }

        // signal any threads blocked on this channel
        threads.signalAndWait();

        if (parent != null) {

            // Close the fd via the parent stream's close method.  The parent
            // will reinvoke our close method, which is defined in the
            // superclass AbstractInterruptibleChannel, but the isOpen logic in
            // that method will prevent this method from being reinvoked.
            //
            ((java.io.Closeable)parent).close();
        } else {
            nd.close(fd);
        }

    }

    public int read(ByteBuffer dst) throws IOException {
        ensureOpen();
        if (!readable)
            throw new NonReadableChannelException();
        synchronized (positionLock) {
            int n = 0;
            int ti = -1;
            try {
                begin();
                ti = threads.add();
                if (!isOpen())
                    return 0;
                do {
                    n = IOUtil.read(fd, dst, -1, nd);
                } while ((n == IOStatus.INTERRUPTED) && isOpen());
                return IOStatus.normalize(n);
            } finally {
                threads.remove(ti);
                end(n > 0);
                assert IOStatus.check(n);
            }
        }
    }

    public long read(ByteBuffer[] dsts, int offset, int length)
        throws IOException
    {
        if ((offset < 0) || (length < 0) || (offset > dsts.length - length))
            throw new IndexOutOfBoundsException();
        ensureOpen();
        if (!readable)
            throw new NonReadableChannelException();
        synchronized (positionLock) {
            long n = 0;
            int ti = -1;
            try {
                begin();
                ti = threads.add();
                if (!isOpen())
                    return 0;
                do {
                    n = IOUtil.read(fd, dsts, offset, length, nd);
                } while ((n == IOStatus.INTERRUPTED) && isOpen());
                return IOStatus.normalize(n);
            } finally {
                threads.remove(ti);
                end(n > 0);
                assert IOStatus.check(n);
            }
        }
    }

    public int write(ByteBuffer src) throws IOException {
        ensureOpen();
        if (!writable)
            throw new NonWritableChannelException();
        synchronized (positionLock) {
            int n = 0;
            int ti = -1;
            try {
                begin();
                ti = threads.add();
                if (!isOpen())
                    return 0;
                do {
                    n = IOUtil.write(fd, src, -1, nd);
                } while ((n == IOStatus.INTERRUPTED) && isOpen());
                return IOStatus.normalize(n);
            } finally {
                threads.remove(ti);
                end(n > 0);
                assert IOStatus.check(n);
            }
        }
    }

    public long write(ByteBuffer[] srcs, int offset, int length)
        throws IOException
    {
        if ((offset < 0) || (length < 0) || (offset > srcs.length - length))
            throw new IndexOutOfBoundsException();
        ensureOpen();
        if (!writable)
            throw new NonWritableChannelException();
        synchronized (positionLock) {
            long n = 0;
            int ti = -1;
            try {
                begin();
                ti = threads.add();
                if (!isOpen())
                    return 0;
                do {
                    n = IOUtil.write(fd, srcs, offset, length, nd);
                } while ((n == IOStatus.INTERRUPTED) && isOpen());
                return IOStatus.normalize(n);
            } finally {
                threads.remove(ti);
                end(n > 0);
                assert IOStatus.check(n);
            }
        }
    }

    // -- Other operations --

    public long position() throws IOException {
        ensureOpen();
        synchronized (positionLock) {
            long p = -1;
            int ti = -1;
            try {
                begin();
                ti = threads.add();
                if (!isOpen())
                    return 0;
                do {
                    // in append-mode then position is advanced to end before writing
                    p = (append) ? nd.size(fd) : position0(fd, -1);
                } while ((p == IOStatus.INTERRUPTED) && isOpen());
                return IOStatus.normalize(p);
            } finally {
                threads.remove(ti);
                end(p > -1);
                assert IOStatus.check(p);
            }
        }
    }

    public FileChannel position(long newPosition) throws IOException {
        ensureOpen();
        if (newPosition < 0)
            throw new IllegalArgumentException();
        synchronized (positionLock) {
            long p = -1;
            int ti = -1;
            try {
                begin();
                ti = threads.add();
                if (!isOpen())
                    return null;
                do {
                    p  = position0(fd, newPosition);
                } while ((p == IOStatus.INTERRUPTED) && isOpen());
                return this;
            } finally {
                threads.remove(ti);
                end(p > -1);
                assert IOStatus.check(p);
            }
        }
    }

    public long size() throws IOException {
        ensureOpen();
        synchronized (positionLock) {
            long s = -1;
            int ti = -1;
            try {
                begin();
                ti = threads.add();
                if (!isOpen())
                    return -1;
                do {
                    s = nd.size(fd);
                } while ((s == IOStatus.INTERRUPTED) && isOpen());
                return IOStatus.normalize(s);
            } finally {
                threads.remove(ti);
                end(s > -1);
                assert IOStatus.check(s);
            }
        }
    }

    public FileChannel truncate(long newSize) throws IOException {
        ensureOpen();
        if (newSize < 0)
            throw new IllegalArgumentException("Negative size");
        if (!writable)
            throw new NonWritableChannelException();
        synchronized (positionLock) {
            int rv = -1;
            long p = -1;
            int ti = -1;
            try {
                begin();
                ti = threads.add();
                if (!isOpen())
                    return null;

                // get current size
                long size;
                do {
                    size = nd.size(fd);
                } while ((size == IOStatus.INTERRUPTED) && isOpen());
                if (!isOpen())
                    return null;

                // get current position
                do {
                    p = position0(fd, -1);
                } while ((p == IOStatus.INTERRUPTED) && isOpen());
                if (!isOpen())
                    return null;
                assert p >= 0;

                // truncate file if given size is less than the current size
                if (newSize < size) {
                    do {
                        rv = nd.truncate(fd, newSize);
                    } while ((rv == IOStatus.INTERRUPTED) && isOpen());
                    if (!isOpen())
                        return null;
                }

                // [IKVM] in append mode we're not allowed to seek backwards, but the atomic append will honor the new file size
                if (append)
                    return this;

                // if position is beyond new size then adjust it
                if (p > newSize)
                    p = newSize;
                do {
                    rv = (int)position0(fd, p);
                } while ((rv == IOStatus.INTERRUPTED) && isOpen());
                return this;
            } finally {
                threads.remove(ti);
                end(rv > -1);
                assert IOStatus.check(rv);
            }
        }
    }

    public void force(boolean metaData) throws IOException {
        ensureOpen();
        int rv = -1;
        int ti = -1;
        try {
            begin();
            ti = threads.add();
            if (!isOpen())
                return;
            do {
                rv = nd.force(fd, metaData);
            } while ((rv == IOStatus.INTERRUPTED) && isOpen());
        } finally {
            threads.remove(ti);
            end(rv > -1);
            assert IOStatus.check(rv);
        }
    }

    private long transferToArbitraryChannel(long position, int icount,
                                            WritableByteChannel target)
        throws IOException
    {
        // Untrusted target: Use a newly-erased buffer
        int c = Math.min(icount, TRANSFER_SIZE);
        ByteBuffer bb = ByteBuffer.allocate(c);
        long tw = 0;                    // Total bytes written
        long pos = position;
        try {
            while (tw < icount) {
                bb.limit(Math.min((int)(icount - tw), TRANSFER_SIZE));
                int nr = read(bb, pos);
                if (nr <= 0)
                    break;
                bb.flip();
                // ## Bug: Will block writing target if this channel
                // ##      is asynchronously closed
                int nw = target.write(bb);
                tw += nw;
                if (nw != nr)
                    break;
                pos += nw;
                bb.clear();
            }
            return tw;
        } catch (IOException x) {
            if (tw > 0)
                return tw;
            throw x;
        }
    }

    public long transferTo(long position, long count,
                           WritableByteChannel target)
        throws IOException
    {
        ensureOpen();
        if (!target.isOpen())
            throw new ClosedChannelException();
        if (!readable)
            throw new NonReadableChannelException();
        if (target instanceof FileChannelImpl &&
            !((FileChannelImpl)target).writable)
            throw new NonWritableChannelException();
        if ((position < 0) || (count < 0))
            throw new IllegalArgumentException();
        long sz = size();
        if (position > sz)
            return 0;
        int icount = (int)Math.min(count, Integer.MAX_VALUE);
        if ((sz - position) < icount)
            icount = (int)(sz - position);

        // Slow path for untrusted targets
        return transferToArbitraryChannel(position, icount, target);
    }

    private long transferFromFileChannel(FileChannelImpl src,
                                         long position, long count)
        throws IOException
    {
        if (!src.readable)
            throw new NonReadableChannelException();
        return transferFromArbitraryChannel(src, position, count);
    }

    private static final int TRANSFER_SIZE = 8192;

    private long transferFromArbitraryChannel(ReadableByteChannel src,
                                              long position, long count)
        throws IOException
    {
        // Untrusted target: Use a newly-erased buffer
        int c = (int)Math.min(count, TRANSFER_SIZE);
        ByteBuffer bb = ByteBuffer.allocate(c);
        long tw = 0;                    // Total bytes written
        long pos = position;
        try {
            while (tw < count) {
                bb.limit((int)Math.min((count - tw), (long)TRANSFER_SIZE));
                // ## Bug: Will block reading src if this channel
                // ##      is asynchronously closed
                int nr = src.read(bb);
                if (nr <= 0)
                    break;
                bb.flip();
                int nw = write(bb, pos);
                tw += nw;
                if (nw != nr)
                    break;
                pos += nw;
                bb.clear();
            }
            return tw;
        } catch (IOException x) {
            if (tw > 0)
                return tw;
            throw x;
        }
    }

    public long transferFrom(ReadableByteChannel src,
                             long position, long count)
        throws IOException
    {
        ensureOpen();
        if (!src.isOpen())
            throw new ClosedChannelException();
        if (!writable)
            throw new NonWritableChannelException();
        if ((position < 0) || (count < 0))
            throw new IllegalArgumentException();
        if (position > size())
            return 0;
        if (src instanceof FileChannelImpl)
           return transferFromFileChannel((FileChannelImpl)src,
                                          position, count);

        return transferFromArbitraryChannel(src, position, count);
    }

    public int read(ByteBuffer dst, long position) throws IOException {
        if (dst == null)
            throw new NullPointerException();
        if (position < 0)
            throw new IllegalArgumentException("Negative position");
        if (!readable)
            throw new NonReadableChannelException();
        ensureOpen();
        if (nd.needsPositionLock()) {
            synchronized (positionLock) {
                return readInternal(dst, position);
            }
        } else {
            return readInternal(dst, position);
        }
    }

    private int readInternal(ByteBuffer dst, long position) throws IOException {
        assert !nd.needsPositionLock() || Thread.holdsLock(positionLock);
        int n = 0;
        int ti = -1;
        try {
            begin();
            ti = threads.add();
            if (!isOpen())
                return -1;
            do {
                n = IOUtil.read(fd, dst, position, nd);
            } while ((n == IOStatus.INTERRUPTED) && isOpen());
            return IOStatus.normalize(n);
        } finally {
            threads.remove(ti);
            end(n > 0);
            assert IOStatus.check(n);
        }
    }

    public int write(ByteBuffer src, long position) throws IOException {
        if (src == null)
            throw new NullPointerException();
        if (position < 0)
            throw new IllegalArgumentException("Negative position");
        if (!writable)
            throw new NonWritableChannelException();
        ensureOpen();
        if (nd.needsPositionLock()) {
            synchronized (positionLock) {
                return writeInternal(src, position);
            }
        } else {
            return writeInternal(src, position);
        }
    }

    private int writeInternal(ByteBuffer src, long position) throws IOException {
        assert !nd.needsPositionLock() || Thread.holdsLock(positionLock);
        int n = 0;
        int ti = -1;
        try {
            begin();
            ti = threads.add();
            if (!isOpen())
                return -1;
            do {
                n = IOUtil.write(fd, src, position, nd);
            } while ((n == IOStatus.INTERRUPTED) && isOpen());
            return IOStatus.normalize(n);
        } finally {
            threads.remove(ti);
            end(n > 0);
            assert IOStatus.check(n);
        }
    }


    // -- Memory-mapped buffers --

    private static class Unmapper
        implements Runnable
    {
        // may be required to close file
        private static final NativeDispatcher nd = new FileDispatcherImpl();

        // keep track of mapped buffer usage
        static volatile int count;
        static volatile long totalSize;
        static volatile long totalCapacity;

        private volatile long address;
        private final long size;
        private final int cap;
        private final FileDescriptor fd;

        private Unmapper(long address, long size, int cap,
                         FileDescriptor fd)
        {
            assert (address != 0);
            this.address = address;
            this.size = size;
            this.cap = cap;
            this.fd = fd;

            synchronized (Unmapper.class) {
                count++;
                totalSize += size;
                totalCapacity += cap;
            }
        }

        public void run() {
            if (address == 0)
                return;
            unmap0(address, size);
            address = 0;

            // if this mapping has a valid file descriptor then we close it
            if (fd.valid()) {
                try {
                    nd.close(fd);
                } catch (IOException ignore) {
                    // nothing we can do
                }
            }

            synchronized (Unmapper.class) {
                count--;
                totalSize -= size;
                totalCapacity -= cap;
            }
        }
    }

    private static void unmap(MappedByteBuffer bb) {
        Cleaner cl = ((DirectBuffer)bb).cleaner();
        if (cl != null)
            cl.clean();
    }

    private static final int MAP_RO = 0;
    private static final int MAP_RW = 1;
    private static final int MAP_PV = 2;

    public MappedByteBuffer map(MapMode mode, long position, long size)
        throws IOException
    {
        ensureOpen();
        if (mode == null)
            throw new NullPointerException("Mode is null");
        if (position < 0L)
            throw new IllegalArgumentException("Negative position");
        if (size < 0L)
            throw new IllegalArgumentException("Negative size");
        if (position + size < 0)
            throw new IllegalArgumentException("Position + size overflow");
        if (size > Integer.MAX_VALUE)
            throw new IllegalArgumentException("Size exceeds Integer.MAX_VALUE");

        int imode = -1;
        if (mode == MapMode.READ_ONLY)
            imode = MAP_RO;
        else if (mode == MapMode.READ_WRITE)
            imode = MAP_RW;
        else if (mode == MapMode.PRIVATE)
            imode = MAP_PV;
        assert (imode >= 0);
        if ((mode != MapMode.READ_ONLY) && !writable)
            throw new NonWritableChannelException();
        if (!readable)
            throw new NonReadableChannelException();

        long addr = -1;
        int ti = -1;
        try {
            begin();
            ti = threads.add();
            if (!isOpen())
                return null;

            long filesize;
            do {
                filesize = nd.size(fd);
            } while ((filesize == IOStatus.INTERRUPTED) && isOpen());
            if (!isOpen())
                return null;

            if (filesize < position + size) { // Extend file size
                if (!writable) {
                    throw new IOException("Channel not open for writing " +
                        "- cannot extend file to required size");
                }
                int rv;
                do {
                    rv = nd.truncate(fd, position + size);
                } while ((rv == IOStatus.INTERRUPTED) && isOpen());
                if (!isOpen())
                    return null;
            }
            if (size == 0) {
                addr = 0;
                // a valid file descriptor is not required
                FileDescriptor dummy = new FileDescriptor();
                if ((!writable) || (imode == MAP_RO))
                    return Util.newMappedByteBufferR(0, 0, dummy, null);
                else
                    return Util.newMappedByteBuffer(0, 0, dummy, null);
            }

            int pagePosition = (int)(position % allocationGranularity);
            long mapPosition = position - pagePosition;
            long mapSize = size + pagePosition;
            try {
                // If no exception was thrown from map0, the address is valid
                addr = map0(imode, mapPosition, mapSize);
            } catch (OutOfMemoryError x) {
                // An OutOfMemoryError may indicate that we've exhausted memory
                // so force gc and re-attempt map
                System.gc();
                try {
                    Thread.sleep(100);
                } catch (InterruptedException y) {
                    Thread.currentThread().interrupt();
                }
                try {
                    addr = map0(imode, mapPosition, mapSize);
                } catch (OutOfMemoryError y) {
                    // After a second OOME, fail
                    throw new IOException("Map failed", y);
                }
            }

            // On Windows, and potentially other platforms, we need an open
            // file descriptor for some mapping operations.
            FileDescriptor mfd;
            try {
                mfd = nd.duplicateForMapping(fd);
            } catch (IOException ioe) {
                unmap0(addr, mapSize);
                throw ioe;
            }

            assert (IOStatus.checkAll(addr));
            assert (addr % allocationGranularity == 0);
            int isize = (int)size;
            Unmapper um = new Unmapper(addr, mapSize, isize, mfd);
            if ((!writable) || (imode == MAP_RO)) {
                return Util.newMappedByteBufferR(isize,
                                                 addr + pagePosition,
                                                 mfd,
                                                 um);
            } else {
                return Util.newMappedByteBuffer(isize,
                                                addr + pagePosition,
                                                mfd,
                                                um);
            }
        } finally {
            threads.remove(ti);
            end(IOStatus.checkAll(addr));
        }
    }

    /**
     * Invoked by sun.management.ManagementFactoryHelper to create the management
     * interface for mapped buffers.
     */
    public static sun.misc.JavaNioAccess.BufferPool getMappedBufferPool() {
        return new sun.misc.JavaNioAccess.BufferPool() {
            @Override
            public String getName() {
                return "mapped";
            }
            @Override
            public long getCount() {
                return Unmapper.count;
            }
            @Override
            public long getTotalCapacity() {
                return Unmapper.totalCapacity;
            }
            @Override
            public long getMemoryUsed() {
                return Unmapper.totalSize;
            }
        };
    }

    // -- Locks --



    // keeps track of locks on this file
    private volatile FileLockTable fileLockTable;

    // indicates if file locks are maintained system-wide (as per spec)
    private static boolean isSharedFileLockTable;

    // indicates if the disableSystemWideOverlappingFileLockCheck property
    // has been checked
    private static volatile boolean propertyChecked;

    // The lock list in J2SE 1.4/5.0 was local to each FileChannel instance so
    // the overlap check wasn't system wide when there were multiple channels to
    // the same file. This property is used to get 1.4/5.0 behavior if desired.
    private static boolean isSharedFileLockTable() {
        if (!propertyChecked) {
            synchronized (FileChannelImpl.class) {
                if (!propertyChecked) {
                    String value = AccessController.doPrivileged(
                        new GetPropertyAction(
                            "sun.nio.ch.disableSystemWideOverlappingFileLockCheck"));
                    isSharedFileLockTable = ((value == null) || value.equals("false"));
                    propertyChecked = true;
                }
            }
        }
        return isSharedFileLockTable;
    }

    private FileLockTable fileLockTable() throws IOException {
        if (fileLockTable == null) {
            synchronized (this) {
                if (fileLockTable == null) {
                    if (isSharedFileLockTable()) {
                        int ti = threads.add();
                        try {
                            ensureOpen();
                            fileLockTable = FileLockTable.newSharedFileLockTable(this, fd);
                        } finally {
                            threads.remove(ti);
                        }
                    } else {
                        fileLockTable = new SimpleFileLockTable();
                    }
                }
            }
        }
        return fileLockTable;
    }

    public FileLock lock(long position, long size, boolean shared)
        throws IOException
    {
        ensureOpen();
        if (shared && !readable)
            throw new NonReadableChannelException();
        if (!shared && !writable)
            throw new NonWritableChannelException();
        FileLockImpl fli = new FileLockImpl(this, position, size, shared);
        FileLockTable flt = fileLockTable();
        flt.add(fli);
        boolean completed = false;
        int ti = -1;
        try {
            begin();
            ti = threads.add();
            if (!isOpen())
                return null;
            int n;
            do {
                n = nd.lock(fd, true, position, size, shared);
            } while ((n == FileDispatcher.INTERRUPTED) && isOpen());
            if (isOpen()) {
                if (n == FileDispatcher.RET_EX_LOCK) {
                    assert shared;
                    FileLockImpl fli2 = new FileLockImpl(this, position, size,
                                                         false);
                    flt.replace(fli, fli2);
                    fli = fli2;
                }
                completed = true;
            }
        } finally {
            if (!completed)
                flt.remove(fli);
            threads.remove(ti);
            try {
                end(completed);
            } catch (ClosedByInterruptException e) {
                throw new FileLockInterruptionException();
            }
        }
        return fli;
    }

    public FileLock tryLock(long position, long size, boolean shared)
        throws IOException
    {
        ensureOpen();
        if (shared && !readable)
            throw new NonReadableChannelException();
        if (!shared && !writable)
            throw new NonWritableChannelException();
        FileLockImpl fli = new FileLockImpl(this, position, size, shared);
        FileLockTable flt = fileLockTable();
        flt.add(fli);
        int result;

        int ti = threads.add();
        try {
            try {
                ensureOpen();
                result = nd.lock(fd, false, position, size, shared);
            } catch (IOException e) {
                flt.remove(fli);
                throw e;
            }
            if (result == FileDispatcher.NO_LOCK) {
                flt.remove(fli);
                return null;
            }
            if (result == FileDispatcher.RET_EX_LOCK) {
                assert shared;
                FileLockImpl fli2 = new FileLockImpl(this, position, size,
                                                     false);
                flt.replace(fli, fli2);
                return fli2;
            }
            return fli;
        } finally {
            threads.remove(ti);
        }
    }

    void release(FileLockImpl fli) throws IOException {
        int ti = threads.add();
        try {
            ensureOpen();
            nd.release(fd, fli.position(), fli.size());
        } finally {
            threads.remove(ti);
        }
        assert fileLockTable != null;
        fileLockTable.remove(fli);
    }

    // -- File lock support --

    /**
     * A simple file lock table that maintains a list of FileLocks obtained by a
     * FileChannel. Use to get 1.4/5.0 behaviour.
     */
    private static class SimpleFileLockTable extends FileLockTable {
        // synchronize on list for access
        private final List<FileLock> lockList = new ArrayList<FileLock>(2);

        public SimpleFileLockTable() {
        }

        private void checkList(long position, long size)
            throws OverlappingFileLockException
        {
            assert Thread.holdsLock(lockList);
            for (FileLock fl: lockList) {
                if (fl.overlaps(position, size)) {
                    throw new OverlappingFileLockException();
                }
            }
        }

        public void add(FileLock fl) throws OverlappingFileLockException {
            synchronized (lockList) {
                checkList(fl.position(), fl.size());
                lockList.add(fl);
            }
        }

        public void remove(FileLock fl) {
            synchronized (lockList) {
                lockList.remove(fl);
            }
        }

        public List<FileLock> removeAll() {
            synchronized(lockList) {
                List<FileLock> result = new ArrayList<FileLock>(lockList);
                lockList.clear();
                return result;
            }
        }

        public void replace(FileLock fl1, FileLock fl2) {
            synchronized (lockList) {
                lockList.remove(fl1);
                lockList.add(fl2);
            }
        }
    }

    // -- Native methods --

    // Creates a new mapping
    private long map0(int prot, long position, long length) throws IOException
    {
        FileStream fs = (FileStream)fd.getStream();
        if (win32)
            return mapViewOfFileWin32(fs, prot, position, length);
        else
            return mapViewOfFilePosix(fs, prot, position, length);
    }

    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    private static long mapViewOfFileWin32(FileStream fs, int prot, long position, long length) throws IOException
    {
        try
        {
            int PAGE_READONLY = 2;
            int PAGE_READWRITE = 4;
            int PAGE_WRITECOPY = 8;
            
            int FILE_MAP_WRITE = 2;
            int FILE_MAP_READ = 4;
            int FILE_MAP_COPY = 1;

            int fileProtect;
            int mapAccess;

            switch (prot)
            {
                case MAP_RO:
                    fileProtect = PAGE_READONLY;
                    mapAccess = FILE_MAP_READ;
                    break;
                case MAP_RW:
                    fileProtect = PAGE_READWRITE;
                    mapAccess = FILE_MAP_WRITE;
                    break;
                case MAP_PV:
                    fileProtect = PAGE_WRITECOPY;
                    mapAccess = FILE_MAP_COPY;
                    break;
                default:
                    throw new Error();
            }

            long maxSize = length + position;
            SafeFileHandle hFileMapping = CreateFileMapping(fs.get_SafeFileHandle(), IntPtr.Zero, fileProtect, (int)(maxSize >> 32), (int)maxSize, null);
            int err = cli.System.Runtime.InteropServices.Marshal.GetLastWin32Error();
            if (hFileMapping.get_IsInvalid())
            {
                throw new IOException("Win32 error " + err);
            }
            IntPtr p = MapViewOfFile(hFileMapping, mapAccess, (int)(position >> 32), (int)position, IntPtr.op_Explicit(length));
            err = cli.System.Runtime.InteropServices.Marshal.GetLastWin32Error();
            hFileMapping.Close();
            if (p.Equals(IntPtr.Zero))
            {
                if (err == 8 /*ERROR_NOT_ENOUGH_MEMORY*/)
                {
                    throw new OutOfMemoryError("Map failed");
                }
                throw new IOException("Win32 error " + err);
            }
            cli.System.GC.AddMemoryPressure(length);
            return p.ToInt64();
        }
        finally
        {
            cli.System.GC.KeepAlive(fs);
        }
    }

    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    private static long mapViewOfFilePosix(FileStream fs, int prot, long position, long length) throws IOException
    {
        byte writeable = prot != MAP_RO ? (byte)1 : (byte)0;
        byte copy_on_write = prot == MAP_PV ? (byte)1 : (byte)0;
        IntPtr p = ikvm_mmap(fs.get_SafeFileHandle(), writeable, copy_on_write, position, (int)length);
        cli.System.GC.KeepAlive(fs);
        // HACK ikvm_mmap should really be changed to return a null pointer on failure,
        // instead of whatever MAP_FAILED is defined to on the particular system we're running on,
        // common values for MAP_FAILED are 0 and -1, so we test for these.
        if (p.Equals(IntPtr.Zero) || p.Equals(new IntPtr(-1)))
        {
            throw new IOException("file mapping failed");
        }
        cli.System.GC.AddMemoryPressure(length);
        return p.ToInt64();
    }

    @DllImportAttribute.Annotation(value="kernel32", SetLastError=true)
    private static native SafeFileHandle CreateFileMapping(SafeFileHandle hFile, IntPtr lpAttributes, int flProtect, int dwMaximumSizeHigh, int dwMaximumSizeLow, String lpName);

    @DllImportAttribute.Annotation(value="kernel32", SetLastError=true)
    private static native IntPtr MapViewOfFile(SafeFileHandle hFileMapping, int dwDesiredAccess, int dwFileOffsetHigh, int dwFileOffsetLow, IntPtr dwNumberOfBytesToMap);

    @DllImportAttribute.Annotation("kernel32")
    private static native int UnmapViewOfFile(IntPtr lpBaseAddress);

    @DllImportAttribute.Annotation("ikvm-native")
    private static native int ikvm_munmap(IntPtr address, int size);

    @DllImportAttribute.Annotation("ikvm-native")
    private static native IntPtr ikvm_mmap(SafeFileHandle handle, byte writeable, byte copy_on_write, long position, int size);

    // Removes an existing mapping
    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    static int unmap0(long address, long length)
    {
        if (win32)
            UnmapViewOfFile(IntPtr.op_Explicit(address));
        else
            ikvm_munmap(IntPtr.op_Explicit(address), (int)length);
        cli.System.GC.RemoveMemoryPressure(length);
        return 0;
    }

    // Sets or reports this file's position
    // If offset is -1, the current position is returned
    // otherwise the position is set to offset
    private static long position0(FileDescriptor fd, long offset) throws IOException
    {
        if (offset == -1)
        {
            return fd.getFilePointer();
        }
        fd.seek(offset);
        return offset;
    }
}
