/*
 * Copyright 2000-2006 Sun Microsystems, Inc.  All Rights Reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Sun designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Sun in the LICENSE file that accompanied this code.
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
 * Please contact Sun Microsystems, Inc., 4150 Network Circle, Santa Clara,
 * CA 95054 USA or visit www.sun.com if you need additional information or
 * have any questions.
 */

package sun.nio.ch;

import cli.System.IntPtr;
import cli.System.IO.FileStream;
import cli.System.Reflection.MethodInfo;
import cli.System.Reflection.ParameterModifier;
import cli.System.Reflection.BindingFlags;
import cli.System.Runtime.InteropServices.DllImportAttribute;
import cli.System.Runtime.InteropServices.StructLayoutAttribute;
import cli.System.Runtime.InteropServices.LayoutKind;
import cli.System.Type;
import java.io.FileDescriptor;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.RandomAccessFile;
import java.io.IOException;
import java.nio.ByteBuffer;
import java.nio.MappedByteBuffer;
import java.nio.channels.*;
import java.nio.channels.spi.*;
import java.util.ArrayList;
import java.util.List;
import java.util.Iterator;
import java.util.concurrent.ConcurrentHashMap;
import java.lang.ref.WeakReference;
import java.lang.ref.ReferenceQueue;
import java.lang.reflect.Field;
import java.security.AccessController;
import java.security.PrivilegedAction;
import sun.misc.Cleaner;
import sun.security.action.GetPropertyAction;


public class FileChannelImpl
    extends FileChannel
{
    private static final boolean win32 = ikvm.internal.Util.WINDOWS;
    private static final boolean winNT = cli.System.Environment.get_OSVersion().get_Platform().Value == cli.System.PlatformID.Win32NT;

    // Memory allocation size for mapping buffers
    private static final long allocationGranularity = 64 * 1024;    // HACK we're using a hard coded value here that works on all mainstream platforms

    // Cached field for MappedByteBuffer.isAMappedBuffer
    private static Field isAMappedBufferField;

    // File descriptor
    private FileDescriptor fd;   

    // File access mode (immutable)
    private boolean writable;
    private boolean readable;
    private boolean appending;

    // Required to prevent finalization of creating stream (immutable)
    private Object parent;

    // Lock for operations involving position and size
    private Object positionLock = new Object();   

    private FileChannelImpl(FileDescriptor fd, boolean readable,
                            boolean writable, Object parent, boolean append)
    {
	this.fd = fd;
	this.readable = readable;
	this.writable = writable;
        this.parent = parent;
        this.appending = append;            
    }

    // Invoked by getChannel() methods
    // of java.io.File{Input,Output}Stream and RandomAccessFile
    //
    public static FileChannel open(FileDescriptor fd,
				   boolean readable, boolean writable,
				   Object parent)
    {
	return new FileChannelImpl(fd, readable, writable, parent, false);
    }

    public static FileChannel open(FileDescriptor fd,
				   boolean readable, boolean writable,
				   Object parent, boolean append)
    {
	return new FileChannelImpl(fd, readable, writable, parent, append);
    }

    private void ensureOpen() throws IOException {
	if (!isOpen())
	    throw new ClosedChannelException();
    }


    // -- Standard channel operations --

    protected void implCloseChannel() throws IOException {

	fd.close();
        
        // Invalidate and release any locks that we still hold
        if (fileLockTable != null) {
            fileLockTable.removeAll( new FileLockTable.Releaser() { 
                public void release(FileLock fl) throws IOException {
                    ((FileLockImpl)fl).invalidate();
                    release0(fd, fl.position(), fl.size());
                }
            });
        }

	if (parent != null) {

	    // Close the fd via the parent stream's close method.  The parent
	    // will reinvoke our close method, which is defined in the
	    // superclass AbstractInterruptibleChannel, but the isOpen logic in
	    // that method will prevent this method from being reinvoked.
	    //
	    if (parent instanceof FileInputStream)
		((FileInputStream)parent).close();
	    else if (parent instanceof FileOutputStream)
		((FileOutputStream)parent).close();
	    else if (parent instanceof RandomAccessFile)
		((RandomAccessFile)parent).close();
	    else
		assert false;

	}

    }

    public int read(ByteBuffer dst) throws IOException {
	ensureOpen();
	if (!readable)
	    throw new NonReadableChannelException();
	synchronized (positionLock) {
	    int n = 0;
	    try {
		begin();
		if (!isOpen())
		    return 0;
		n = readImpl(dst);
		return IOStatus.normalize(n);
	    } finally {
		end(n > 0);
		assert IOStatus.check(n);
	    }
	}
    }

    private long read0(ByteBuffer[] dsts) throws IOException {
	ensureOpen();
        if (!readable)
            throw new NonReadableChannelException();
	synchronized (positionLock) {
	    long n = 0;
	    try {
		begin();
		if (!isOpen())
		    return 0;
		n = readImpl(dsts);
		return IOStatus.normalize(n);
	    } finally {
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
	// ## Fix IOUtil.write so that we can avoid this array copy
	return read0(Util.subsequence(dsts, offset, length));
    }

    public int write(ByteBuffer src) throws IOException {
	ensureOpen();
        if (!writable)
            throw new NonWritableChannelException();
	synchronized (positionLock) {
	    int n = 0;
	    try {
		begin();
		if (!isOpen())
		    return 0;
                if (appending)
                    position(size());
		n = writeImpl(src);
		return IOStatus.normalize(n);
	    } finally {
		end(n > 0);
		assert IOStatus.check(n);
	    }
	}
    }

    private long write0(ByteBuffer[] srcs) throws IOException {
	ensureOpen();
        if (!writable)
            throw new NonWritableChannelException();
	synchronized (positionLock) {
	    long n = 0;
	    try {
		begin();
		if (!isOpen())
		    return 0;
                if (appending)
                    position(size());
		n = writeImpl(srcs);
		return IOStatus.normalize(n);
	    } finally {
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
	// ## Fix IOUtil.write so that we can avoid this array copy
	return write0(Util.subsequence(srcs, offset, length));
    }


    // -- Other operations --

    public long position() throws IOException {
	ensureOpen();
	synchronized (positionLock) {
	    long p = -1;
	    try {
		begin();
		if (!isOpen())
		    return 0;
		do {
		    p = position0(fd, -1);
		} while ((p == IOStatus.INTERRUPTED) && isOpen());
		return IOStatus.normalize(p);
	    } finally {
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
	    try {
		begin();
		if (!isOpen())
		    return null;
		do {
		    p  = position0(fd, newPosition);
		} while ((p == IOStatus.INTERRUPTED) && isOpen());
		return this;
	    } finally {
		end(p > -1);
		assert IOStatus.check(p);
	    }
	}
    }

    public long size() throws IOException {
	ensureOpen();
	synchronized (positionLock) {
	    long s = -1;
	    try {
		begin();
		if (!isOpen())
		    return -1;
		do {
		    s = size0(fd);
		} while ((s == IOStatus.INTERRUPTED) && isOpen());
		return IOStatus.normalize(s);
	    } finally {
		end(s > -1);
		assert IOStatus.check(s);
	    }
	}
    }

    public FileChannel truncate(long size) throws IOException {
	ensureOpen();
        if (size < 0)
            throw new IllegalArgumentException();
        if (size > size())
            return this;
	if (!writable)
	    throw new NonWritableChannelException();
	synchronized (positionLock) {
            int rv = -1;
	    long p = -1;
	    try {
		begin();
		if (!isOpen())
		    return null;

                // get current position
		do {
		    p = position0(fd, -1);
		} while ((p == IOStatus.INTERRUPTED) && isOpen());
		if (!isOpen()) 
                    return null;
                assert p >= 0;

		// truncate file
		do {
		    rv = truncate0(fd, size);
		} while ((rv == IOStatus.INTERRUPTED) && isOpen());
                if (!isOpen())
                    return null;

		// set position to size if greater than size
                if (p > size)
                    p = size;
                do {
                    rv = (int)position0(fd, p);
                } while ((rv == IOStatus.INTERRUPTED) && isOpen());
		return this;
	    } finally {
		end(rv > -1);
		assert IOStatus.check(rv);
	    }
	}
    }

    public void force(boolean metaData) throws IOException {
	ensureOpen();
	int rv = -1;
	try {
	    begin();
	    if (!isOpen())
		return;
	    do {
		rv = force0(fd, metaData);
	    } while ((rv == IOStatus.INTERRUPTED) && isOpen());
	} finally {
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
	long tw = 0;			// Total bytes written
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

    private static final int TRANSFER_SIZE = 8192;

    private long transferFromArbitraryChannel(ReadableByteChannel src,
                                              long position, long count)
        throws IOException
    {
	// Untrusted target: Use a newly-erased buffer
	int c = (int)Math.min(count, TRANSFER_SIZE);
        ByteBuffer bb = ByteBuffer.allocate(c);
	long tw = 0;			// Total bytes written
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
	int n = 0;
	try {
	    begin();
	    if (!isOpen())
		return -1;
	    n = readImpl(dst, position);
	    return IOStatus.normalize(n);
	} finally {
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
	int n = 0;
	try {
	    begin();
	    if (!isOpen())
		return -1;
	    n = writeImpl(src, position);
	    return IOStatus.normalize(n);
	} finally {
	    end(n > 0);
	    assert IOStatus.check(n);
	}
    }


    // -- Memory-mapped buffers --

    private static class Unmapper
	implements Runnable
    {

	private long address;
	private long size;

	private Unmapper(long address, long size) {
	    assert (address != 0);
	    this.address = address;
	    this.size = size;
	}

	public void run() {
	    if (address == 0)
		return;
	    unmap0(address, size);
	    address = 0;
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
	try {
	    begin();
	    if (!isOpen())
		return null;
            if (size() < position + size) { // Extend file size
                if (!writable) {
                    throw new IOException("Channel not open for writing " +
                        "- cannot extend file to required size");
                }
		int rv;
		do {
		    rv = truncate0(fd, position + size);
		} while ((rv == IOStatus.INTERRUPTED) && isOpen());
            }
            if (size == 0) {
                addr = 0;
                if ((!writable) || (imode == MAP_RO))
                    return Util.newMappedByteBufferR(0, 0, null);
                else
                    return Util.newMappedByteBuffer(0, 0, null);
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

            assert (IOStatus.checkAll(addr));
            assert (addr % allocationGranularity == 0);
            int isize = (int)size;
            Unmapper um = new Unmapper(addr, size + pagePosition);
            if ((!writable) || (imode == MAP_RO))
                return Util.newMappedByteBufferR(isize, addr + pagePosition, um);
            else
                return Util.newMappedByteBuffer(isize, addr + pagePosition, um);
	} finally {
	    end(IOStatus.checkAll(addr));
	}
    }


    // -- Locks --

    public static final int NO_LOCK = -1;       // Failed to lock
    public static final int LOCKED = 0;         // Obtained requested lock
    public static final int RET_EX_LOCK = 1;    // Obtained exclusive lock
    public static final int INTERRUPTED = 2;    // Request interrupted
    
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
                    PrivilegedAction pa = new GetPropertyAction(
                        "sun.nio.ch.disableSystemWideOverlappingFileLockCheck");
                    String value = (String)AccessController.doPrivileged(pa);                   
                    isSharedFileLockTable = ((value == null) || value.equals("false"));
                    propertyChecked = true;
                }
            }
        }        
        return isSharedFileLockTable;        
    }
                      
    private FileLockTable fileLockTable() {
        if (fileLockTable == null) {
            synchronized (this) {
                if (fileLockTable == null) {       
                    fileLockTable = isSharedFileLockTable() ? 
                        new SharedFileLockTable(this) : new SimpleFileLockTable();
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
        boolean i = true;
        try {
            begin();
	    if (!isOpen())
		return null;
            int result = lock0(fd, true, position, size, shared);
            if (result == RET_EX_LOCK) {
                assert shared;
                FileLockImpl fli2 = new FileLockImpl(this, position, size,
                                                     false);
                flt.replace(fli, fli2);
                return fli2;
            }
            if (result == INTERRUPTED || result == NO_LOCK) {
                flt.remove(fli);
                i = false;
            }
        } catch (IOException e) {
            flt.remove(fli);
            throw e;
        } finally {
            try {
                end(i);
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
	int result = lock0(fd, false, position, size, shared);
	if (result == NO_LOCK) {
	    flt.remove(fli);
	    return null;
	}
	if (result == RET_EX_LOCK) {
	    assert shared;
	    FileLockImpl fli2 = new FileLockImpl(this, position, size,
						 false);
	    flt.replace(fli, fli2);
	    return fli2;
	}
        return fli;
    }

    void release(FileLockImpl fli) throws IOException {
	ensureOpen();
        release0(fd, fli.position(), fli.size());
        assert fileLockTable != null;
        fileLockTable.remove(fli);
    }
      

    // -- File lock support  --
        
    /**
     * A table of FileLocks.
     */
    private interface FileLockTable {   
        /**
         * Adds a file lock to the table.
         *
         * @throws OverlappingFileLockException if the file lock overlaps
         *         with an existing file lock in the table
         */
        void add(FileLock fl) throws OverlappingFileLockException;
        
        /**
         * Remove an existing file lock from the table.
         */
        void remove(FileLock fl);
        
        /**
         * An implementation of this interface releases a given file lock.
         * Used with removeAll.
         */
        interface Releaser { 
            void release(FileLock fl) throws IOException; 
        } 
        
        /**
         * Removes all file locks from the table.
         * <p>
         * The Releaser#release method is invoked for each file lock before
         * it is removed.
         *
         * @throws IOException if the release method throws IOException
         */
        void removeAll(Releaser r) throws IOException;
                        
        /**
         * Replaces an existing file lock in the table.
         */         
        void replace(FileLock fl1, FileLock fl2);        
    }
        
    /**
     * A simple file lock table that maintains a list of FileLocks obtained by a
     * FileChannel. Use to get 1.4/5.0 behaviour.
     */
    private static class SimpleFileLockTable implements FileLockTable {
        // synchronize on list for access
        private List<FileLock> lockList = new ArrayList<FileLock>(2);       
                        
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
        
        public void removeAll(Releaser releaser) throws IOException {
            synchronized(lockList) {
                Iterator<FileLock> i = lockList.iterator();
                while (i.hasNext()) {
                    FileLock fl = i.next();
                    releaser.release(fl);
                    i.remove();
                }
            }
        }
        
        public void replace(FileLock fl1, FileLock fl2) {
            synchronized (lockList) {
                lockList.remove(fl1);
                lockList.add(fl2);
            }
        }                      
    }
        
    /**
     * A weak reference to a FileLock. 
     * <p>
     * SharedFileLockTable uses a list of file lock references to avoid keeping the 
     * FileLock (and FileChannel) alive.
     */    
    private static class FileLockReference extends WeakReference<FileLock> {
        private FileKey fileKey; 
        
        FileLockReference(FileLock referent,
                          ReferenceQueue queue,
                          FileKey key) {                           
            super(referent, queue);
            this.fileKey = key;
        }
                
        private FileKey fileKey() {
            return fileKey;
        }       
    }
        
    /**
     * A file lock table that is over a system-wide map of all file locks.   
     */
    private static class SharedFileLockTable implements FileLockTable {              
        // The system-wide map is a ConcurrentHashMap that is keyed on the FileKey.
        // The map value is a list of file locks represented by FileLockReferences.
        // All access to the list must be synchronized on the list.         
        private static ConcurrentHashMap<FileKey, ArrayList<FileLockReference>> lockMap = 
            new ConcurrentHashMap<FileKey, ArrayList<FileLockReference>>();
        
        // reference queue for cleared refs
        private static ReferenceQueue queue = new ReferenceQueue();
            
        // the enclosing file channel
        private FileChannelImpl fci;            

        // File key for the file that this channel is connected to
        private FileKey fileKey;    
                
        public SharedFileLockTable(FileChannelImpl fci) {
            this.fci = fci;
            this.fileKey = FileKey.create(fci.fd);
        }        
                
        public void add(FileLock fl) throws OverlappingFileLockException {                        
            ArrayList<FileLockReference> list = lockMap.get(fileKey);
            
            for (;;) {
                
                // The key isn't in the map so we try to create it atomically                
                if (list == null) {
                    list = new ArrayList<FileLockReference>(2);
                    ArrayList<FileLockReference> prev;
                    synchronized (list) {                                        
                        prev = lockMap.putIfAbsent(fileKey, list);
                        if (prev == null) {   
                            // we successfully created the key so we add the file lock
                            list.add(new FileLockReference(fl, queue, fileKey));                                                       
                            break;
                        }                            
                    }
                    // someone else got there first
                    list = prev; 
                } 
                
                // There is already a key. It is possible that some other thread  
                // is removing it so we re-fetch the value from the map. If it 
                // hasn't changed then we check the list for overlapping locks 
                // and add the new lock to the list.                 
                synchronized (list) {
                    ArrayList<FileLockReference> current = lockMap.get(fileKey);
                    if (list == current) {
                        checkList(list, fl.position(), fl.size());
                        list.add(new FileLockReference(fl, queue, fileKey));                                                
                        break;
                    }
                    list = current;
                }
                
            } 
            
            // process any stale entries pending in the reference queue 
            removeStaleEntries();
        }
        
        private void removeKeyIfEmpty(FileKey fk, ArrayList<FileLockReference> list) {
            assert Thread.holdsLock(list);
            assert lockMap.get(fk) == list;
            if (list.isEmpty()) {                     
                lockMap.remove(fk);                   
            }             
        }
                
        public void remove(FileLock fl) {
            assert fl != null;
            
            // the lock must exist so the list of locks must be present
            ArrayList<FileLockReference> list = lockMap.get(fileKey);            
            assert list != null;                                              
                        
            synchronized (list) {                     
                int index = 0;
                while (index < list.size()) {
                    FileLockReference ref = list.get(index);
                    FileLock lock = ref.get();
                    if (lock == fl) {                        
                        assert (lock != null) && (lock.channel() == fci);
                        ref.clear();
                        list.remove(index);
                        break;
                    }
                    index++;
                }         
            }            
        }
        
        public void removeAll(Releaser releaser) throws IOException {    
            ArrayList<FileLockReference> list = lockMap.get(fileKey);
            if (list != null) {                                             
                synchronized (list) {                                
                    int index = 0;
                    while (index < list.size()) {
                        FileLockReference ref = list.get(index);
                        FileLock lock = ref.get();
                        
                        // remove locks obtained by this channel
                        if (lock != null && lock.channel() == fci) {                                                       
                            // invoke the releaser to invalidate/release the lock
                            releaser.release(lock);
                            
                            // remove the lock from the list
                            ref.clear();
                            list.remove(index);
                        } else {
                            index++;
                        }
                    }
                    
                    // once the lock list is empty we remove it from the map
                    removeKeyIfEmpty(fileKey, list);         
                }    
            }
        }
        
        public void replace(FileLock fromLock, FileLock toLock) {           
            // the lock must exist so there must be a list
            ArrayList<FileLockReference> list = lockMap.get(fileKey);            
            assert list != null;                         
            
            synchronized (list) {               
                for (int index=0; index<list.size(); index++) {
                    FileLockReference ref = list.get(index);
                    FileLock lock = ref.get();
                    if (lock == fromLock) {   
                        ref.clear();
                        list.set(index, new FileLockReference(toLock, queue, fileKey));
                        break;
                    }
                }
            }
        }   
                     
        // Check for overlapping file locks         
        private void checkList(List<FileLockReference> list, long position, long size) 
            throws OverlappingFileLockException
        {
            assert Thread.holdsLock(list);
            for (FileLockReference ref: list) {                
                FileLock fl = ref.get();
                if (fl != null && fl.overlaps(position, size)) 
                    throw new OverlappingFileLockException();
            }
        }                          
        
        // Process the reference queue        
        private void removeStaleEntries() {
            FileLockReference ref;
            while ((ref = (FileLockReference)queue.poll()) != null) {                
                FileKey fk = ref.fileKey();
                ArrayList<FileLockReference> list = lockMap.get(fk);
                if (list != null) {
                    synchronized (list) {
                        list.remove(ref);                              
                        removeKeyIfEmpty(fk, list); 
                    }
                }
            }
        }               
    }
 
    // -- Native methods --

    private int readImpl(ByteBuffer dst) throws IOException
    {
	if (dst.hasArray())
	{
	    byte[] buf = dst.array();
	    int len = fd.readBytes(buf, dst.arrayOffset() + dst.position(), dst.remaining());
	    if (len > 0)
	    {
		dst.position(dst.position() + len);
	    }
	    return len;
	}
	else
	{
	    byte[] buf = new byte[dst.remaining()];
	    int len = fd.readBytes(buf, 0, buf.length);
	    if (len > 0)
	    {
		dst.put(buf, 0, len);
	    }
	    return len;
	}
    }

    private int readImpl(ByteBuffer dst, long position) throws IOException
    {
	synchronized (positionLock)
	{
	    long prev = position0(fd, -1);
	    try
	    {
		position0(fd, position);
		return readImpl(dst);
	    }
	    finally
	    {
		position0(fd, prev);
	    }
	}
    }

    private long readImpl(ByteBuffer[] dsts) throws IOException
    {
	long totalRead = 0;
	try
	{
	    for (int i = 0; i < dsts.length; i++)
	    {
		int size = dsts[i].remaining();
		if (size > 0)
		{
		    int read = readImpl(dsts[i]);
		    if (read < 0)
		    {
			break;
		    }
		    totalRead += read;
		    if (read < size || fd.available() == 0)
		    {
			break;
		    }
		}
	    }
	}
	catch (IOException x)
	{
	    if (totalRead == 0)
	    {
		throw x;
	    }
	}
	return totalRead;
    }

    private int writeImpl(ByteBuffer src) throws IOException
    {
	if (src.hasArray())
	{
	    byte[] buf = src.array();
	    int len = src.remaining();
	    fd.writeBytes(buf, src.arrayOffset() + src.position(), len);
	    src.position(src.position() + len);
	    return len;
	}
	else
	{
	    int pos = src.position();
	    byte[] buf = new byte[src.remaining()];
	    src.get(buf);
	    fd.writeBytes(buf, 0, buf.length);
	    src.position(pos + buf.length);
	    return buf.length;
	}
    }

    private int writeImpl(ByteBuffer src, long position) throws IOException
    {
	synchronized (positionLock)
	{
	    long prev = position0(fd, -1);
	    try
	    {
		position0(fd, position);
		return writeImpl(src);
	    }
	    finally
	    {
		position0(fd, prev);
	    }
	}
    }

    private long writeImpl(ByteBuffer[] srcs) throws IOException
    {
	long totalWritten = 0;
	try
	{
	    for (int i = 0; i < srcs.length; i++)
	    {
		int size = srcs[i].remaining();
		if (size > 0)
		{
		    int written = writeImpl(srcs[i]);
		    totalWritten += written;
		    if (written < size)
		    {
			break;
		    }
		}
	    }
	}
	catch (IOException x)
	{
	    if (totalWritten == 0)
	    {
		throw x;
	    }
	}
	return totalWritten;
    }

    @StructLayoutAttribute.Annotation(LayoutKind.__Enum.Sequential)
    private static final class OVERLAPPED extends cli.System.Object
    {
	IntPtr Internal;
	IntPtr InternalHigh;
	int OffsetLow;
	int OffsetHigh;
	IntPtr hEvent;
    }

    // Grabs a file lock
    static int lock0(FileDescriptor fd, boolean blocking, long pos, long size, boolean shared) throws IOException
    {
	FileStream fs = (FileStream)fd.getStream();
	if (winNT)
	{
	    int LOCKFILE_FAIL_IMMEDIATELY = 1;
	    int LOCKFILE_EXCLUSIVE_LOCK = 2;
	    int ERROR_LOCK_VIOLATION = 33;
	    int flags = 0;
	    OVERLAPPED o = new OVERLAPPED();
	    o.OffsetLow = (int)pos;
	    o.OffsetHigh = (int)(pos >> 32);
	    if (!blocking)
	    {
		flags |= LOCKFILE_FAIL_IMMEDIATELY;
	    }
	    if (!shared)
	    {
		flags |= LOCKFILE_EXCLUSIVE_LOCK;
	    }
	    int result = LockFileEx(fs.get_Handle(), flags, 0, (int)size, (int)(size >> 32), o);
	    if (result == 0)
	    {
		int error = cli.System.Runtime.InteropServices.Marshal.GetLastWin32Error();
		if (!blocking && error == ERROR_LOCK_VIOLATION)
		{
		    return NO_LOCK;
		}
		throw new IOException("Lock failed");
	    }
	    return LOCKED;
	}
	else
	{
	    try
	    {
		if (false) throw new cli.System.ArgumentOutOfRangeException();
		for (;;)
		{
		    try
		    {
			if (false) throw new cli.System.IO.IOException();
			if (false) throw new cli.System.ObjectDisposedException("");
			fs.Lock(pos, size);
			return shared ? RET_EX_LOCK : LOCKED;
		    }
		    catch (cli.System.IO.IOException x)
		    {
			if (!blocking)
			{
			    return NO_LOCK;
			}
			cli.System.Threading.Thread.Sleep(100);
		    }
		    catch (cli.System.ObjectDisposedException x)
		    {
			throw new IOException(x.getMessage());
		    }
		}
	    }
	    catch (cli.System.ArgumentOutOfRangeException x)
	    {
		throw new IOException(x.getMessage());
	    }
	}
    }

    // Releases a file lock
    static void release0(FileDescriptor fd, long pos, long size) throws IOException
    {
	FileStream fs = (FileStream)fd.getStream();
	if (winNT)
	{
	    OVERLAPPED o = new OVERLAPPED();
	    o.OffsetLow = (int)pos;
	    o.OffsetHigh = (int)(pos >> 32);
	    int result = UnlockFileEx(fs.get_Handle(), 0, (int)size, (int)(size >> 32), o);
	    if (result == 0)
	    {
		throw new IOException("Release failed");
	    }
	}
	else
	{
	    try
	    {
		if (false) throw new cli.System.ArgumentOutOfRangeException();
		if (false) throw new cli.System.IO.IOException();
		if (false) throw new cli.System.ObjectDisposedException("");
		fs.Unlock(pos, size);
	    }
	    catch (cli.System.ArgumentOutOfRangeException x)
	    {
		throw new IOException(x.getMessage());
	    }
	    catch (cli.System.IO.IOException x)
	    {
		throw new IOException(x.getMessage());
	    }
	    catch (cli.System.ObjectDisposedException x)
	    {
		throw new IOException(x.getMessage());
	    }
	}
    }

    // Creates a new mapping
    private long map0(int prot, long position, long length) throws IOException
    {
	FileStream fs = (FileStream)fd.getStream();
	if (win32)
	    return mapViewOfFileWin32(fs, prot, position, length);
	else
	    return mapViewOfFilePosix(fs, prot, position, length);
    }

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

	    IntPtr hFileMapping = CreateFileMapping(fs.get_Handle(), IntPtr.Zero, fileProtect, (int)(length >> 32), (int)length, null);
	    int err = cli.System.Runtime.InteropServices.Marshal.GetLastWin32Error();
	    if (hFileMapping.Equals(IntPtr.Zero))
	    {
		throw new IOException("Win32 error " + err);
	    }
	    IntPtr p = MapViewOfFile(hFileMapping, mapAccess, (int)(position >> 32), (int)position, IntPtr.op_Explicit(length));
	    err = cli.System.Runtime.InteropServices.Marshal.GetLastWin32Error();
	    CloseHandle(hFileMapping);
	    if (p.Equals(IntPtr.Zero))
	    {
		if (err == 8 /*ERROR_NOT_ENOUGH_MEMORY*/)
		{
		    throw new OutOfMemoryError("Map failed");
		}
		throw new IOException("Win32 error " + err);
	    }
	    return p.ToInt64();
	}
	finally
	{
	    cli.System.GC.KeepAlive(fs);
	}
    }

    private static long mapViewOfFilePosix(FileStream fs, int prot, long position, long length) throws IOException
    {
	byte writeable = prot != MAP_RO ? (byte)1 : (byte)0;
	byte copy_on_write = prot == MAP_PV ? (byte)1 : (byte)0;
        IntPtr p = ikvm_mmap(fs.get_Handle(), writeable, copy_on_write, position, (int)length);
        cli.System.GC.KeepAlive(fs);
        // HACK ikvm_mmap should really be changed to return a null pointer on failure,
        // instead of whatever MAP_FAILED is defined to on the particular system we're running on,
        // common values for MAP_FAILED are 0 and -1, so we test for these.
        if (p.Equals(IntPtr.Zero) || p.Equals(new IntPtr(-1)))
        {
            throw new IOException("file mapping failed");
        }
        return p.ToInt64();
    }

    private static boolean flushWin32(FileStream fs)
    {
        int rc = FlushFileBuffers(fs.get_Handle());
        cli.System.GC.KeepAlive(fs);
        return rc != 0;
    }

    private static boolean flushPosix(FileStream fs)
    {
        Type t = Type.GetType("Mono.Posix.Syscall, Mono.Posix");
        if(t != null)
        {
            BindingFlags flags = BindingFlags.wrap(BindingFlags.Public | BindingFlags.Static);
            MethodInfo mono_1_1_Flush = t.GetMethod("fsync", flags, null, new Type[] { Type.GetType("System.Int32") }, new ParameterModifier[0]);
            if(mono_1_1_Flush != null)
            {
                Object[] args = new Object[] { ikvm.lang.CIL.box_int(fs.get_Handle().ToInt32()) };
                return ikvm.lang.CIL.unbox_int(mono_1_1_Flush.Invoke(null, args)) == 0;
            }
        }
        return true;
    }

    @DllImportAttribute.Annotation("kernel32")
    private static native int FlushFileBuffers(IntPtr handle);

    @DllImportAttribute.Annotation("kernel32")
    private static native int CloseHandle(IntPtr handle);

    @DllImportAttribute.Annotation(value="kernel32", SetLastError=true)
    private static native IntPtr CreateFileMapping(IntPtr hFile, IntPtr lpAttributes, int flProtect, int dwMaximumSizeHigh, int dwMaximumSizeLow, String lpName);

    @DllImportAttribute.Annotation(value="kernel32", SetLastError=true)
    private static native IntPtr MapViewOfFile(IntPtr hFileMapping, int dwDesiredAccess, int dwFileOffsetHigh, int dwFileOffsetLow, IntPtr dwNumberOfBytesToMap);

    @DllImportAttribute.Annotation("kernel32")
    private static native int UnmapViewOfFile(IntPtr lpBaseAddress);

    @DllImportAttribute.Annotation(value="kernel32", SetLastError=true)
    static native int LockFileEx(IntPtr hFile, int dwFlags, int dwReserved, int nNumberOfBytesToLockLow, int nNumberOfBytesToLockHigh, OVERLAPPED lpOverlapped);

    @DllImportAttribute.Annotation("kernel32")
    static native int UnlockFileEx(IntPtr hFile, int dwReserved, int nNumberOfBytesToUnlockLow, int nNumberOfBytesToUnlockHigh, OVERLAPPED lpOverlapped);

    @DllImportAttribute.Annotation("ikvm-native")
    private static native int ikvm_munmap(IntPtr address, int size);

    @DllImportAttribute.Annotation("ikvm-native")
    private static native IntPtr ikvm_mmap(IntPtr handle, byte writeable, byte copy_on_write, long position, int size);

    // Removes an existing mapping
    static int unmap0(long address, long length)
    {
        if (win32)
            UnmapViewOfFile(IntPtr.op_Explicit(address));
        else
            ikvm_munmap(IntPtr.op_Explicit(address), (int)length);
	return 0;
    }

    // Forces output to device
    private static int force0(FileDescriptor fd, boolean metaData) throws IOException
    {
	FileStream fs = (FileStream)fd.getStream();
	boolean rc = win32 ? flushWin32(fs) : flushPosix(fs);
	if (!rc)
	{
	    throw new IOException("Force failed");
	}
	return 0;
    }

    // Truncates a file
    private static int truncate0(FileDescriptor fd, long size) throws IOException
    {
	fd.setLength(size);
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

    // Reports this file's size
    private static long size0(FileDescriptor fd) throws IOException
    {
	return fd.length();
    }

    static {
        isAMappedBufferField = Reflect.lookupField("java.nio.MappedByteBuffer",
                                          "isAMappedBuffer");
    }

}
