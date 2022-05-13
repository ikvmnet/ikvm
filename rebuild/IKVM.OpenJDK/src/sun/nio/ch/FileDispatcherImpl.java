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

import java.io.*;
import java.nio.ByteBuffer;
import cli.Microsoft.Win32.SafeHandles.SafeFileHandle;
import cli.System.IntPtr;
import cli.System.IO.FileStream;
import cli.System.Runtime.InteropServices.DllImportAttribute;
import cli.System.Runtime.InteropServices.StructLayoutAttribute;
import cli.System.Runtime.InteropServices.LayoutKind;
import cli.System.Runtime.InteropServices.Marshal;
import static ikvm.internal.Util.WINDOWS;

class FileDispatcherImpl extends FileDispatcher
{
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

    int read(FileDescriptor fd, byte[] buf, int offset, int length) throws IOException {
        return fd.readBytes(buf, offset, length);
    }

    int write(FileDescriptor fd, byte[] buf, int offset, int length) throws IOException {
        fd.writeBytes(buf, offset, length);
        return length;
    }

    long read(FileDescriptor fd, ByteBuffer[] bufs, int offset, int length) throws IOException {
        long totalRead = 0;
        try
        {
            for (int i = offset; i < offset + length; i++)
            {
                int size = bufs[i].remaining();
                if (size > 0)
                {
                    int read = IOUtil.read(fd, bufs[i], -1, this);
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

    long write(FileDescriptor fd, ByteBuffer[] bufs, int offset, int length) throws IOException {
        long totalWritten = 0;
        try
        {
            for (int i = offset; i < offset + length; i++)
            {
                int size = bufs[i].remaining();
                if (size > 0)
                {
                    int written = IOUtil.write(fd, bufs[i], -1, this);
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

    int force(FileDescriptor fd, boolean metaData) throws IOException {
        fd.sync();
        return 0;
    }

    int truncate(FileDescriptor fd, long size) throws IOException {
        if (append) {
            // HACK in append mode we're not allowed to truncate, so we try to reopen the file and truncate that
            try (FileOutputStream fos = new FileOutputStream(((FileStream)fd.getStream()).get_Name())) {
                fos.getFD().setLength(size);
            }
        } else {
            fd.setLength(size);
        }
        return 0;
    }

    long size(FileDescriptor fd) throws IOException {
        return fd.length();
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

    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    int lock(FileDescriptor fd, boolean blocking, long pos, long size,
             boolean shared) throws IOException
    {
        FileStream fs = (FileStream)fd.getStream();
        if (WINDOWS)
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
            int result = LockFileEx(fs.get_SafeFileHandle(), flags, 0, (int)size, (int)(size >> 32), o);
            if (result == 0)
            {
                int error = Marshal.GetLastWin32Error();
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

    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    void release(FileDescriptor fd, long pos, long size) throws IOException {
        FileStream fs = (FileStream)fd.getStream();
        if (WINDOWS)
        {
            int ERROR_NOT_LOCKED = 158;
            OVERLAPPED o = new OVERLAPPED();
            o.OffsetLow = (int)pos;
            o.OffsetHigh = (int)(pos >> 32);
            int result = UnlockFileEx(fs.get_SafeFileHandle(), 0, (int)size, (int)(size >> 32), o);
            if (result == 0 && Marshal.GetLastWin32Error() != ERROR_NOT_LOCKED)
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
            catch (cli.System.IO.IOException x)
            {
                if (!NotLockedHack.isErrorNotLocked(x))
                {
                    throw new IOException(x.getMessage());
                }
            }
            catch (cli.System.ArgumentOutOfRangeException
                | cli.System.ObjectDisposedException x)
            {
                throw new IOException(x.getMessage());
            }
        }
    }

    static class NotLockedHack {
        private static String msg;
        static {
            try {
                File tmp = File.createTempFile("lock", null);
                try (FileStream fs = new FileStream(tmp.getPath(), cli.System.IO.FileMode.wrap(cli.System.IO.FileMode.Create))) {
                    try {
                        if (false) throw new cli.System.IO.IOException();
                        fs.Unlock(0, 1);
                    } catch (cli.System.IO.IOException x) {
                        msg = x.get_Message();
                    }
                }
                tmp.delete();
            } catch (Throwable _) {
            }
        }
        static boolean isErrorNotLocked(cli.System.IO.IOException x) {
            return x.get_Message().equals(msg);
        }
    }


    void close(FileDescriptor fd) throws IOException {
        fd.close();
    }

    FileDescriptor duplicateForMapping(FileDescriptor fd) throws IOException {
        // we return a dummy FileDescriptor, because we don't need it for mapping operations
        // and we don't want the original to be closed
        return new FileDescriptor();
    }

    @DllImportAttribute.Annotation(value="kernel32", SetLastError=true)
    private static native int LockFileEx(SafeFileHandle hFile, int dwFlags, int dwReserved, int nNumberOfBytesToLockLow, int nNumberOfBytesToLockHigh, OVERLAPPED lpOverlapped);

    @DllImportAttribute.Annotation(value="kernel32", SetLastError=true)
    private static native int UnlockFileEx(SafeFileHandle hFile, int dwReserved, int nNumberOfBytesToUnlockLow, int nNumberOfBytesToUnlockHigh, OVERLAPPED lpOverlapped);
}
