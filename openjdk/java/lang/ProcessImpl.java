/*
 * Copyright 1995-2006 Sun Microsystems, Inc.  All Rights Reserved.
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

/*IKVM*/
/*
 * Modified for IKVM by Jeroen Frijters
 */

package java.lang;

import cli.System.Diagnostics.ProcessStartInfo;
import java.io.*;
import java.nio.channels.Channels;

/* This class is for the exclusive use of ProcessBuilder.start() to
 * create new processes.
 *
 * @author Martin Buchholz
 * @version 1.37, 07/05/05
 * @since   1.5
 */

final class ProcessImpl extends Process {

    // System-dependent portion of ProcessBuilder.start()
    static Process start(String cmdarray[],
                         java.util.Map<String,String> environment,
                         String dir,
                         boolean redirectErrorStream)
        throws IOException
    {
        return new ProcessImpl(cmdarray, environment, dir, redirectErrorStream);
    }

    private cli.System.Diagnostics.Process proc;
    private OutputStream stdin_stream;
    private InputStream stdout_stream;
    private InputStream stderr_stream;

    private ProcessImpl(String cmd[],
                        java.util.Map<String,String> environment,
                        String path,
                        boolean redirectErrorStream)
        throws IOException
    {
        // Win32 CreateProcess requires cmd[0] to be normalized
        cmd[0] = new File(cmd[0]).getPath();
        
        // give the runtime an opportunity to map executables from VFS to a real executable
        cmd[0] = mapVfsExecutable(cmd[0]);

        StringBuilder cmdbuf = new StringBuilder(80);
        for (int i = 1; i < cmd.length; i++) {
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

        ProcessStartInfo si = new ProcessStartInfo(cmd[0], cmdstr);
        si.set_UseShellExecute(false);
        si.set_RedirectStandardError(true);
        si.set_RedirectStandardOutput(true);
        si.set_RedirectStandardInput(true);
        si.set_CreateNoWindow(true);
        if (path != null)
        {
            si.set_WorkingDirectory(path.toString());
        }
        if (environment != null)
        {
            for (String key : environment.keySet())
            {
                si.get_EnvironmentVariables().set_Item(key, environment.get(key));
            }
        }
        try
        {
            if(false) throw new cli.System.ComponentModel.Win32Exception();
            if(false) throw new cli.System.InvalidOperationException();
            proc = cli.System.Diagnostics.Process.Start(si);
        }
        catch(cli.System.ComponentModel.Win32Exception x1)
        {
            throw new java.io.IOException(x1.getMessage());
        }
        catch(cli.System.InvalidOperationException x2)
        {
            throw new java.io.IOException(x2.getMessage());
        }

        stdin_stream = new BufferedOutputStream(new FileOutputStream(FileDescriptor.fromStream(proc.get_StandardInput().get_BaseStream())));
        stdout_stream = new BufferedInputStream(new FileInputStream(FileDescriptor.fromStream(proc.get_StandardOutput().get_BaseStream())));
        stderr_stream = new FileInputStream(FileDescriptor.fromStream(proc.get_StandardError().get_BaseStream()));
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

    public int exitValue()
    {
        if (proc.get_HasExited())
        {
            return proc.get_ExitCode();
        }
        throw new IllegalThreadStateException();
    }

    public int waitFor() throws InterruptedException
    {
        // to be interruptable we have to use polling
        // (on .NET 2.0 WaitForExit is actually interruptible, but this isn't documented)
        for (; ; )
        {
            if (Thread.interrupted())
            {
                throw new InterruptedException();
            }
            if (proc.WaitForExit(100))
            {
                return proc.get_ExitCode();
            }
        }
    }

    public void destroy()
    {
        try
        {
            if (false) throw new cli.System.ComponentModel.Win32Exception();
            if (false) throw new cli.System.InvalidOperationException();
            proc.Kill();
        }
        catch (cli.System.ComponentModel.Win32Exception x)
        {
        }
        catch (cli.System.InvalidOperationException x)
        {
        }
    }
}
