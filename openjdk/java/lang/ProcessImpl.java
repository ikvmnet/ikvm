/*
 * Copyright (c) 1995, 2010, Oracle and/or its affiliates. All rights reserved.
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

/*IKVM*/
/*
 * Modified for IKVM by Jeroen Frijters
 */

package java.lang;

import java.io.IOException;
import java.io.File;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.FileDescriptor;
import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.lang.ProcessBuilder.Redirect;
import java.security.AccessController;
import java.security.PrivilegedAction;
import cli.System.AsyncCallback;
import cli.System.IAsyncResult;
import cli.System.Diagnostics.ProcessStartInfo;
import cli.System.IO.FileAccess;
import cli.System.IO.FileShare;
import cli.System.IO.FileMode;
import cli.System.IO.FileOptions;
import cli.System.IO.FileStream;
import cli.System.IO.Stream;
import cli.System.Security.AccessControl.FileSystemRights;

/* This class is for the exclusive use of ProcessBuilder.start() to
 * create new processes.
 *
 * @author Martin Buchholz
 * @since   1.5
 */

final class ProcessImpl extends Process {
    static class fdAccess {
        static Stream getHandle(FileDescriptor fd) {
            return fd.getStream();
        }
    }

    /**
     * Open a file for writing. If {@code append} is {@code true} then the file
     * is opened for atomic append directly and a FileOutputStream constructed
     * with the resulting handle. This is because a FileOutputStream created
     * to append to a file does not open the file in a manner that guarantees
     * that writes by the child process will be atomic.
     */
    private static FileOutputStream newFileOutputStream(File f, boolean append)
        throws IOException
    {
        if (append) {
            String path = f.getPath();
            SecurityManager sm = System.getSecurityManager();
            if (sm != null)
                sm.checkWrite(path);
            final FileDescriptor fd = openForAtomicAppend(path);
            return AccessController.doPrivileged(
                new PrivilegedAction<FileOutputStream>() {
                    public FileOutputStream run() {
                        return new FileOutputStream(fd);
                    }
                }
            );
        } else {
            return new FileOutputStream(f);
        }
    }

    // System-dependent portion of ProcessBuilder.start()
    static Process start(String cmdarray[],
                         java.util.Map<String,String> environment,
                         String dir,
                         ProcessBuilder.Redirect[] redirects,
                         boolean redirectErrorStream)
        throws IOException
    {
        FileInputStream  f0 = null;
        FileOutputStream f1 = null;
        FileOutputStream f2 = null;

        try {
            Stream[] stdHandles;
            if (redirects == null) {
                stdHandles = new Stream[3];
            } else {
                stdHandles = new Stream[3];

                if (redirects[0] == Redirect.PIPE)
                    stdHandles[0] = null;
                else if (redirects[0] == Redirect.INHERIT)
                    stdHandles[0] = fdAccess.getHandle(FileDescriptor.in);
                else {
                    f0 = new FileInputStream(redirects[0].file());
                    stdHandles[0] = fdAccess.getHandle(f0.getFD());
                }

                if (redirects[1] == Redirect.PIPE)
                    stdHandles[1] = null;
                else if (redirects[1] == Redirect.INHERIT)
                    stdHandles[1] = fdAccess.getHandle(FileDescriptor.out);
                else {
                    f1 = newFileOutputStream(redirects[1].file(),
                                             redirects[1].append());
                    stdHandles[1] = fdAccess.getHandle(f1.getFD());
                }

                if (redirects[2] == Redirect.PIPE)
                    stdHandles[2] = null;
                else if (redirects[2] == Redirect.INHERIT)
                    stdHandles[2] = fdAccess.getHandle(FileDescriptor.err);
                else {
                    f2 = newFileOutputStream(redirects[2].file(),
                                             redirects[2].append());
                    stdHandles[2] = fdAccess.getHandle(f2.getFD());
                }
            }

            return new ProcessImpl(cmdarray, environment, dir,
                                   stdHandles, redirectErrorStream);
        } finally {
            // HACK prevent the File[In|Out]putStream objects from closing the streams
            if (f0 != null)
                cli.System.GC.SuppressFinalize(f0);
            if (f1 != null)
                cli.System.GC.SuppressFinalize(f1);
            if (f2 != null)
                cli.System.GC.SuppressFinalize(f2);
        }

    }

    private cli.System.Diagnostics.Process handle;
    private OutputStream stdin_stream;
    private InputStream stdout_stream;
    private InputStream stderr_stream;

    private ProcessImpl(final String cmd[],
                        final java.util.Map<String,String> envblock,
                        final String path,
                        final Stream[] stdHandles,
                        final boolean redirectErrorStream)
        throws IOException
    {
        // Win32 CreateProcess requires cmd[0] to be normalized
        cmd[0] = new File(cmd[0]).getPath();

        // give the runtime an opportunity to map executables from VFS to a real executable
        cmd[0] = mapVfsExecutable(cmd[0]);

        StringBuilder cmdbuf = new StringBuilder(80);
        for (int i = 0; i < cmd.length; i++) {
            if (i > 0) {
                cmdbuf.append(' ');
            }
            String s = cmd[i];
            if (s.indexOf(' ') >= 0 || s.indexOf('\t') >= 0) {
                if (s.charAt(0) != '"') {
                    cmdbuf.append('"');
                    cmdbuf.append(s);
                    if (s.endsWith("\\")) {
                        cmdbuf.append("\\");
                    }
                    cmdbuf.append('"');
                } else if (s.endsWith("\"")) {
                    /* The argument has already been quoted. */
                    cmdbuf.append(s);
                } else {
                    /* Unmatched quote for the argument. */
                    throw new IllegalArgumentException();
                }
            } else {
                cmdbuf.append(s);
            }
        }
        String cmdstr = cmdbuf.toString();

        handle = create(cmdstr, envblock, path,
                        stdHandles, redirectErrorStream);

        java.security.AccessController.doPrivileged(
        new java.security.PrivilegedAction<Void>() {
        public Void run() {
            if (stdHandles[0] == null)
                stdin_stream = ProcessBuilder.NullOutputStream.INSTANCE;
            else {
                FileDescriptor stdin_fd = FileDescriptor.fromStream(stdHandles[0]);
                stdin_stream = new BufferedOutputStream(
                    new FileOutputStream(stdin_fd));
            }

            if (stdHandles[1] == null)
                stdout_stream = ProcessBuilder.NullInputStream.INSTANCE;
            else {
                FileDescriptor stdout_fd = FileDescriptor.fromStream(stdHandles[1]);
                stdout_stream = new BufferedInputStream(
                    new FileInputStream(stdout_fd));
            }

            if (stdHandles[2] == null)
                stderr_stream = ProcessBuilder.NullInputStream.INSTANCE;
            else {
                FileDescriptor stderr_fd = FileDescriptor.fromStream(stdHandles[2]);
                stderr_stream = new FileInputStream(stderr_fd);
            }

            return null; }});
    }

    private static native String mapVfsExecutable(String path);

    public OutputStream getOutputStream() {
        return stdin_stream;
    }

    public InputStream getInputStream() {
        return stdout_stream;
    }

    public InputStream getErrorStream() {
        return stderr_stream;
    }

    public int exitValue() {
        if (!handle.get_HasExited())
            throw new IllegalThreadStateException("process has not exited");
        return handle.get_ExitCode();
    }

    public int waitFor() throws InterruptedException {
        waitForInterruptibly(handle);
        if (Thread.interrupted())
            throw new InterruptedException();
        return exitValue();
    }
    private static void waitForInterruptibly(cli.System.Diagnostics.Process handle) throws InterruptedException {
        // to be interruptable we have to use polling
        // (on .NET 2.0 WaitForExit is actually interruptible, but this isn't documented)
        Thread current = Thread.currentThread();
        while (!current.isInterrupted() && !handle.WaitForExit(100))
            ;
    }

    public void destroy() { terminateProcess(handle); }
    private static void terminateProcess(cli.System.Diagnostics.Process handle) {
        try {
            if (false) throw new cli.System.ComponentModel.Win32Exception();
            if (false) throw new cli.System.InvalidOperationException();
            handle.Kill();
        } catch (cli.System.ComponentModel.Win32Exception _) {
        } catch (cli.System.InvalidOperationException _) {
        }
    }

    /**
     * Create a process using the win32 function CreateProcess.
     *
     * @param cmdstr the Windows commandline
     * @param envblock NUL-separated, double-NUL-terminated list of
     *        environment strings in VAR=VALUE form
     * @param dir the working directory of the process, or null if
     *        inheriting the current directory from the parent process
     * @param stdHandles array of windows HANDLEs.  Indexes 0, 1, and
     *        2 correspond to standard input, standard output and
     *        standard error, respectively.  On input, a value of -1
     *        means to create a pipe to connect child and parent
     *        processes.  On output, a value which is not -1 is the
     *        parent pipe handle corresponding to the pipe which has
     *        been created.  An element of this array is -1 on input
     *        if and only if it is <em>not</em> -1 on output.
     * @param redirectErrorStream redirectErrorStream attribute
     * @return the native subprocess HANDLE returned by CreateProcess
     */
    private static cli.System.Diagnostics.Process create(String cmdstr,
                                      java.util.Map<String,String> envblock,
                                      String dir,
                                      Stream[] stdHandles,
                                      boolean redirectErrorStream)
        throws IOException {

        int programEnd = parseCommandString(cmdstr);
        int argumentsStart = programEnd;
        if (cmdstr.length() > argumentsStart && cmdstr.charAt(argumentsStart) == ' ') {
            argumentsStart++;
        }

        ProcessStartInfo si = new ProcessStartInfo(cmdstr.substring(0, programEnd), cmdstr.substring(argumentsStart));
        si.set_UseShellExecute(false);
        si.set_RedirectStandardError(true);
        si.set_RedirectStandardOutput(true);
        si.set_RedirectStandardInput(true);
        si.set_CreateNoWindow(true);
        if (dir != null) {
            si.set_WorkingDirectory(dir);
        }
        if (envblock != null) {
            si.get_EnvironmentVariables().Clear();
            for (String key : envblock.keySet()) {
                si.get_EnvironmentVariables().set_Item(key, envblock.get(key));
            }
        }

        cli.System.Diagnostics.Process proc;
        try {
            if (false) throw new cli.System.ComponentModel.Win32Exception();
            if (false) throw new cli.System.InvalidOperationException();
            proc = cli.System.Diagnostics.Process.Start(si);
        } catch (cli.System.ComponentModel.Win32Exception x1) {
            throw new IOException(x1.getMessage());
        } catch (cli.System.InvalidOperationException x2) {
            throw new IOException(x2.getMessage());
        }
        
        Stream stdin = proc.get_StandardInput().get_BaseStream();
        Stream stdout = proc.get_StandardOutput().get_BaseStream();
        Stream stderr = proc.get_StandardError().get_BaseStream();

        if (stdHandles[0] != null) {
            connectPipe(stdHandles[0], stdin);
            stdHandles[0] = null;
        } else {
            stdHandles[0] = stdin;
        }

        Stream stdoutDrain = null;
        if (stdHandles[1] != null) {
            stdoutDrain = stdHandles[1];
            connectPipe(stdout, stdoutDrain);
            stdHandles[1] = null;
        } else if (redirectErrorStream) {
            PipeStream pipe = new PipeStream();
            connectPipe(stdout, pipe);
            connectPipe(stderr, pipe);
            stdHandles[1] = pipe;
        } else {
            stdHandles[1] = stdout;
        }

        if (redirectErrorStream) {
            if (stdoutDrain != null) {
                connectPipe(stderr, stdoutDrain);
            }
            stdHandles[2] = null;
        } else if (stdHandles[2] != null) {
            connectPipe(stderr, stdHandles[2]);
            stdHandles[2] = null;
        } else {
            stdHandles[2] = stderr;
        }

        return proc;
    }

    private static final class PipeStream extends Stream
    {
        private final byte[] buf = new byte[4096];
        private int pos;
        private int users = 2;

        @Override
        public synchronized int Read(byte[] buffer, int offset, int count)
        {
            if (count == 0)
            {
                return 0;
            }
            while (pos == 0)
            {
                try
                {
                    wait();
                }
                catch (InterruptedException _) { }
            }
            if (pos == -1)
            {
                return 0;
            }
            count = Math.min(count, pos);
            System.arraycopy(buf, 0, buffer, offset, count);
            pos -= count;
            System.arraycopy(buf, count, buf, 0, pos);
            notifyAll();
            return count;
        }

        @Override
        public synchronized void Write(byte[] buffer, int offset, int count)
        {
            while (buf.length - pos < count)
            {
                try
                {
                    wait();
                }
                catch (InterruptedException _) { }
            }
            System.arraycopy(buffer, offset, buf, pos, count);
            pos += count;
            notifyAll();
        }

        @Override
        public synchronized void Close()
        {
            if (--users == 0)
            {
                pos = -1;
                notifyAll();
            }
        }

        @Override
        public boolean get_CanRead()
        {
            return true;
        }

        @Override
        public boolean get_CanSeek()
        {
            return false;
        }

        @Override
        public boolean get_CanWrite()
        {
            return true;
        }

        @Override
        public void Flush()
        {
        }

        @Override
        public long get_Length()
        {
            ikvm.runtime.Util.throwException(new cli.System.NotSupportedException());
            return 0;
        }

        @Override
        public long get_Position()
        {
            ikvm.runtime.Util.throwException(new cli.System.NotSupportedException());
            return 0;
        }

        @Override
        public long Seek(long offset, cli.System.IO.SeekOrigin origin)
        {
            ikvm.runtime.Util.throwException(new cli.System.NotSupportedException());
            return 0;
        }

        @Override
        public void SetLength(long value)
        {
            ikvm.runtime.Util.throwException(new cli.System.NotSupportedException());
        }

        @Override
        public void set_Position(long position)
        {
            ikvm.runtime.Util.throwException(new cli.System.NotSupportedException());
        }
    }

    private static native int parseCommandString(String cmdstr);

    /**
     * Opens a file for atomic append. The file is created if it doesn't
     * already exist.
     *
     * @param file the file to open or create
     * @return the native HANDLE
     */
    private static FileDescriptor openForAtomicAppend(String path)
        throws IOException {
        try {
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.Security.SecurityException();
            if (false) throw new cli.System.UnauthorizedAccessException();
            return FileDescriptor.fromStream(new FileStream(path, FileMode.wrap(FileMode.Append), FileSystemRights.wrap(FileSystemRights.AppendData), FileShare.wrap(FileShare.ReadWrite), 1, FileOptions.wrap(FileOptions.None)));
        } catch (cli.System.ArgumentException x) {
            throw new IOException(x.getMessage());
        } catch (cli.System.IO.IOException x) {
            throw new IOException(x.getMessage());
        } catch (cli.System.Security.SecurityException x) {
            throw new IOException(x.getMessage());
        } catch (cli.System.UnauthorizedAccessException x) {
            throw new IOException(x.getMessage());
        }
    }

    private static void connectPipe(final Stream in, final Stream out) {
        final byte[] buf = new byte[4096];
        final AsyncCallback[] callback = new AsyncCallback[1];
        callback[0] = new AsyncCallback(new AsyncCallback.Method() {
            public void Invoke(IAsyncResult ar) {
                try {
                    int count = in.EndRead(ar);
                    if (count > 0) {
                        out.Write(buf, 0, count);
                        out.Flush();
                        in.BeginRead(buf, 0, buf.length, callback[0], null);
                    } else {
                        out.Close();
                    }
                } catch (Throwable _) {
                }
            }
        });
        try {
            in.BeginRead(buf, 0, buf.length, callback[0], null);
        } catch (Throwable _) {
        }
    }
}
