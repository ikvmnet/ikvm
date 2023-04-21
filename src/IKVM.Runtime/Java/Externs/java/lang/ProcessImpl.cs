using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Threading;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.Accessors.Java.Lang;
using IKVM.Runtime.Accessors.Java.Util;
using IKVM.Runtime.Util.Java.Security;

namespace IKVM.Java.Externs.java.lang
{

    /// <summary>
    /// Implements the native methods for 'ProcessImpl'.
    /// </summary>
    static class ProcessImpl
    {

#if FIRST_PASS == false

        static SystemAccessor systemAccessor;
        static SecurityManagerAccessor securityManagerAccessor;
        static ThreadAccessor threadAccessor;
        static ProcessBuilderRedirectAccessor processBuilderRedirectAccessor;
        static ProcessBuilderNullInputStreamAccessor processBuilderNullInputStreamAccessor;
        static ProcessBuilderNullOutputStreamAccessor processBuilderNullOutputStreamAccessor;
        static FileAccessor fileAccessor;
        static FileDescriptorAccessor fileDescriptorAccessor;
        static InputStreamAccessor inputStreamAccessor;
        static OutputStreamAccessor outputStreamAccessor;
        static FileInputStreamAccessor fileInputStreamAccessor;
        static FileOutputStreamAccessor fileOutputStreamAccessor;
        static BufferedInputStreamAccessor bufferedInputStreamAccessor;
        static BufferedOutputStreamAccessor bufferedOutputStreamAccessor;
        static AccessControllerAccessor accessControllerAccessor;
        static MapAccessor mapAccessor;
        static MapEntryAccessor mapEntryAccessor;
        static SetAccessor setAccessor;
        static IteratorAccessor iteratorAccessor;
        static ProcessImplAccessor processImplAccessor;

        static SystemAccessor SystemAccessor => JVM.BaseAccessors.Get(ref systemAccessor);

        static SecurityManagerAccessor SecurityManagerAccessor => JVM.BaseAccessors.Get(ref securityManagerAccessor);

        static ThreadAccessor ThreadAccessor => JVM.BaseAccessors.Get(ref threadAccessor);

        static ProcessBuilderRedirectAccessor ProcessBuilderRedirectAccessor => JVM.BaseAccessors.Get(ref processBuilderRedirectAccessor);

        static ProcessBuilderNullInputStreamAccessor ProcessBuilderNullInputStreamAccessor => JVM.BaseAccessors.Get(ref processBuilderNullInputStreamAccessor);

        static ProcessBuilderNullOutputStreamAccessor ProcessBuilderNullOutputStreamAccessor => JVM.BaseAccessors.Get(ref processBuilderNullOutputStreamAccessor);

        static FileAccessor FileAccessor => JVM.BaseAccessors.Get(ref fileAccessor);

        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);

        static InputStreamAccessor InputStreamAccessor => JVM.BaseAccessors.Get(ref inputStreamAccessor);

        static OutputStreamAccessor OutputStreamAccessor => JVM.BaseAccessors.Get(ref outputStreamAccessor);

        static FileInputStreamAccessor FileInputStreamAccessor => JVM.BaseAccessors.Get(ref fileInputStreamAccessor);

        static FileOutputStreamAccessor FileOutputStreamAccessor => JVM.BaseAccessors.Get(ref fileOutputStreamAccessor);

        static BufferedInputStreamAccessor BufferedInputStreamAccessor => JVM.BaseAccessors.Get(ref bufferedInputStreamAccessor);

        static BufferedOutputStreamAccessor BufferedOutputStreamAccessor => JVM.BaseAccessors.Get(ref bufferedOutputStreamAccessor);

        static AccessControllerAccessor AccessControllerAccessor => JVM.BaseAccessors.Get(ref accessControllerAccessor);

        static MapAccessor MapAccessor => JVM.BaseAccessors.Get(ref mapAccessor);

        static MapEntryAccessor MapEntryAccessor => JVM.BaseAccessors.Get(ref mapEntryAccessor);

        static SetAccessor SetAccessor => JVM.BaseAccessors.Get(ref setAccessor);

        static IteratorAccessor IteratorAccessor => JVM.BaseAccessors.Get(ref iteratorAccessor);

        static ProcessImplAccessor ProcessImplAccessor => JVM.BaseAccessors.Get(ref processImplAccessor);

#endif

        enum WindowsVerificationMode
        {

            CmdOrBat = 0,
            Win32 = 1,
            Legacy = 2,

        }

        static readonly char[][] WindowsInvalidChars =
        {
            // We guarantee the only command file execution for implicit [cmd.exe] run.
            //    http://technet.microsoft.com/en-us/library/bb490954.aspx
            new [] {' ', '\t', '<', '>', '&', '|', '^'},
            new [] {' ', '\t', '<', '>'},
            new [] {' ', '\t'}
        };

        /// <summary>
        /// Implements the native method 'start'.
        /// </summary>
        /// <param name="cmdarray"></param>
        /// <param name="environment"></param>
        /// <param name="dir"></param>
        /// <param name="redirects"></param>
        /// <param name="redirectErrorStream"></param>
        public static object start(string[] cmdarray, object environment, string dir, object[] redirects, bool redirectErrorStream)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            // close these specifically upon exception
            FileStream f0 = null;
            FileStream f1 = null;
            FileStream f2 = null;

            try
            {
                Stream h0 = null;
                Stream h1 = null;
                Stream h2 = null;

                // transform redirect specifications into Streams
                if (redirects != null)
                {
                    var PIPE = ProcessBuilderRedirectAccessor.GetPipe();
                    var INHERIT = ProcessBuilderRedirectAccessor.GetInherit();

                    if (redirects[0] == PIPE)
                        h0 = null;
                    else if (redirects[0] == INHERIT)
                        h0 = FileDescriptorAccessor.GetStream(FileDescriptorAccessor.GetIn());
                    else
                    {
                        f0 = File.OpenRead(FileAccessor.InvokeGetPath(ProcessBuilderRedirectAccessor.InvokeFile(redirects[0])));
                        h0 = f0;
                    }

                    if (redirects[1] == PIPE)
                        h1 = null;
                    else if (redirects[1] == INHERIT)
                        h1 = FileDescriptorAccessor.GetStream(FileDescriptorAccessor.GetOut());
                    else
                    {
                        f1 = NewFileOutputStream(ProcessBuilderRedirectAccessor.InvokeFile(redirects[1]), ProcessBuilderRedirectAccessor.InvokeAppend(redirects[1]));
                        h1 = f1;
                    }

                    if (redirects[2] == PIPE)
                        h2 = null;
                    else if (redirects[2] == INHERIT)
                        h2 = FileDescriptorAccessor.GetStream(FileDescriptorAccessor.GetErr());
                    else
                    {
                        f2 = NewFileOutputStream(ProcessBuilderRedirectAccessor.InvokeFile(redirects[2]), ProcessBuilderRedirectAccessor.InvokeAppend(redirects[2]));
                        h2 = f2;
                    }
                }

                return Create(cmdarray, MapToDictionary(environment), dir, h0, h1, h2, redirectErrorStream);
            }
            catch (Exception e)
            {
                f0?.Dispose();
                f1?.Dispose();
                f2?.Dispose();

                if (e is global::java.lang.Throwable)
                    throw;
                else
                    throw new global::java.io.IOException(e.Message, e);
            }
#endif
        }

        /// <summary>
        /// Converts a map of string key and string values to a dictionary of the same.
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        static Dictionary<string, string> MapToDictionary(object map)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (map == null)
                return null;

            var d = new Dictionary<string, string>();
            var i = SetAccessor.InvokeIterator(MapAccessor.InvokeEntrySet(map));
            while (IteratorAccessor.InvokeHasNext(i))
            {
                var e = IteratorAccessor.InvokeNext(i);
                var k = (string)MapEntryAccessor.InvokeGetKey(e);
                var v = (string)MapEntryAccessor.InvokeGetValue(e);
                d[k] = v;
            }

            return d;
#endif
        }

        /// <summary>
        /// Creates a new 'ProcessImpl' given the specified input.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="env"></param>
        /// <param name="dir"></param>
        /// <param name="s0"></param>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="redirectErrorStream"></param>
        /// <returns></returns>
        static object Create(string[] cmd, Dictionary<string, string> env, string dir, Stream s0, Stream s1, Stream s2, bool redirectErrorStream)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeUtil.IsWindows)
                return CreateWindows(cmd, env, dir, s0, s1, s2, redirectErrorStream);
            else
                return CreateUnix(cmd, env, dir, s0, s1, s2, redirectErrorStream);
#endif
        }

        /// <summary>
        /// Returns whether the argument has already been quoted.
        /// </summary>
        /// <param name="noQuotesInside"></param>
        /// <param name="arg"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        static bool IsQuoted(bool noQuotesInside, string arg, string errorMessage)
        {
            int lastPos = arg.Length - 1;
            if (lastPos >= 1 && arg[0] == '"' && arg[lastPos] == '"')
            {
                // the argument has already been quoted
                if (noQuotesInside)
                {
                    if (arg.IndexOf('"', 1) != lastPos)
                    {
                        // there is ["] inside
                        throw new ArgumentException(errorMessage);
                    }
                }
                return true;
            }

            if (noQuotesInside)
            {
                if (arg.IndexOf('"') >= 0)
                {
                    // there is ["] inside
                    throw new ArgumentException(errorMessage);
                }
            }

            return false;
        }

        /// <summary>
        /// Returns whether the argument needs escaping, based on the given verification type.
        /// </summary>
        /// <param name="verificationType"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        static bool NeedsEscapingOnWindows(WindowsVerificationMode verificationType, string arg)
        {
            // Switch off MS heuristic for internal ["].
            // Please, use the explicit [cmd.exe] call
            // if you need the internal ["].
            //    Example: "cmd.exe", "/C", "Extended_MS_Syntax"

            // For [.exe] or [.com] file the unpaired/internal ["]
            // in the argument is not a problem.
            var argIsQuoted = IsQuoted(verificationType == WindowsVerificationMode.CmdOrBat, arg, "Argument has embedded quote, use the explicit CMD.EXE call.");
            if (argIsQuoted == false)
            {
                var testEscape = WindowsInvalidChars[(int)verificationType];
                for (int i = 0; i < testEscape.Length; ++i)
                {
                    if (arg.IndexOf(testEscape[i]) >= 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns whether the specified file ends with a shell executable extension.
        /// </summary>
        /// <param name="executablePath"></param>
        /// <returns></returns>
        static bool IsWindowsShellFile(string executablePath)
        {
            return executablePath.EndsWith(".CMD", StringComparison.OrdinalIgnoreCase) || executablePath.EndsWith(".BAT", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Surrounds the specified string with quotes.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        static string QuoteString(string arg)
        {
            return '"' + arg + '"';
        }

        /// <summary>
        /// Merges the arguments into a single command line suitable for Windows.
        /// </summary>
        /// <param name="verificationType"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        static string CreateWindowsArguments(WindowsVerificationMode verificationType, IEnumerable<string> cmd)
        {
            var cmdbuf = new global::System.Text.StringBuilder(80);

            foreach (var s in cmd)
            {
                cmdbuf.Append(' ');

                if (NeedsEscapingOnWindows(verificationType, s))
                {
                    cmdbuf.Append('"').Append(s);

                    // The code protects the [java.exe] and console command line
                    // parser, that interprets the [\"] combination as an escape
                    // sequence for the ["] char.
                    //     http://msdn.microsoft.com/en-us/library/17w5ykft.aspx
                    //
                    // If the argument is an FS path, doubling of the tail [\]
                    // char is not a problem for non-console applications.
                    //
                    // The [\"] sequence is not an escape sequence for the [cmd.exe]
                    // command line parser. The case of the [""] tail escape
                    // sequence could not be realized due to the argument validation
                    // procedure.
                    if ((verificationType != WindowsVerificationMode.CmdOrBat) && s.EndsWith("\\"))
                        cmdbuf.Append('\\');

                    cmdbuf.Append('"');
                }
                else
                {
                    cmdbuf.Append(s);
                }
            }

            return cmdbuf.ToString().TrimStart(' ');
        }

        /// <summary>
        /// Returns the proper path to the executable.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static string GetWindowsExecutablePath(string path)
        {
            var pathIsQuoted = IsQuoted(true, path, "Executable name has embedded quote, split the arguments");

            // Win32 CreateProcess requires path to be normalized
            var fileToRun = pathIsQuoted ? path.Substring(1, path.Length - 1) : path;

            // From the [CreateProcess] function documentation:
            //
            // "If the file name does not contain an extension, .exe is appended.
            // Therefore, if the file name extension is .com, this parameter
            // must include the .com extension. If the file name ends in
            // a period (.) with no extension, or if the file name contains a path,
            // .exe is not appended."
            //
            // "If the file name !does not contain a directory path!,
            // the system searches for the executable file in the following
            // sequence:..."
            //
            // In practice ANY non-existent path is extended by [.exe] extension
            // in the [CreateProcess] funcion with the only exception:
            // the path ends by (.)

            return fileToRun;
        }

        /// <summary>
        /// Extracts individual tokens from a command.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        static string[] GetTokensFromWindowsCommand(string command)
        {
            var matchList = new List<string>(8);
            foreach (Match m in Regex.Matches(command, "[^\\s\"]+|\"[^\"]*\""))
                matchList.Add(m.Value);

            return matchList.ToArray();
        }

        /// <summary>
        /// Initializes the ProcessImpl object for a Windows platform.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="env"></param>
        /// <param name="dir"></param>
        /// <param name="h0"></param>
        /// <param name="h1"></param>
        /// <param name="h2"></param>
        /// <param name="redirectErrorStream"></param>
        /// <returns></returns>
        static object CreateWindows(string[] cmd, Dictionary<string, string> env, string dir, Stream h0, Stream h1, Stream h2, bool redirectErrorStream)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            string exe;
            string arg;

            var sm = SystemAccessor.InvokeGetSecurityManager();
            var allowAmbiguousCommands = false;
            if (sm == null)
            {
                allowAmbiguousCommands = true;
                var value = SystemAccessor.InvokeGetProperty("jdk.lang.Process.allowAmbiguousCommands");
                if (value != null)
                    allowAmbiguousCommands = string.Equals(value, "false", StringComparison.OrdinalIgnoreCase) == false;
            }

            if (allowAmbiguousCommands)
            {
                exe = cmd[0];

                if (NeedsEscapingOnWindows(WindowsVerificationMode.Legacy, exe))
                    exe = QuoteString(exe);

                // legacy mode doesn't worry about extended verification
                arg = CreateWindowsArguments(WindowsVerificationMode.Legacy, cmd.Skip(1));
            }
            else
            {
                try
                {
                    exe = GetWindowsExecutablePath(cmd[0]);
                }
                catch (ArgumentException)
                {
                    // Workaround for the calls like
                    // Runtime.getRuntime().exec("\"C:\\Program Files\\foo\" bar")

                    // No chance to avoid CMD/BAT injection, except to do the work
                    // right from the beginning. Otherwise we have too many corner
                    // cases from
                    //    Runtime.getRuntime().exec(String[] cmd [, ...])
                    // calls with internal ["] and escape sequences.

                    // Restore original command line.
                    var join = new global::System.Text.StringBuilder();

                    // terminal space in command line is ok
                    foreach (string s in cmd.Skip(1))
                        join.Append(s).Append(' ');

                    // Parse the command line again.
                    cmd = GetTokensFromWindowsCommand(join.ToString());
                    exe = GetWindowsExecutablePath(cmd[0]);

                    // Check new executable name once more
                    if (sm != null)
                        SecurityManagerAccessor.InvokeCheckExec(sm, exe);
                }

                // Quotation protects from interpretation of the [path] argument as
                // start of longer path with spaces. Quotation has no influence to
                // [.exe] extension heuristic.
                arg = CreateWindowsArguments(IsWindowsShellFile(exe) ? WindowsVerificationMode.CmdOrBat : WindowsVerificationMode.Win32, cmd);
            }

            // on Windows, always use single argument string, since we are responsible for formatting
            var psi = new ProcessStartInfo();
            psi.FileName = exe;
            psi.Arguments = arg;
            return Create(psi, env, dir, h0, h1, h2, redirectErrorStream);
#endif
        }

        /// <summary>
        /// Creates a new 'ProcessImpl' for a Unix machine.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="env"></param>
        /// <param name="dir"></param>
        /// <param name="h0"></param>
        /// <param name="h1"></param>
        /// <param name="h2"></param>
        /// <param name="redirectErrorStream"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        static object CreateUnix(string[] cmd, Dictionary<string, string> env, string dir, Stream h0, Stream h1, Stream h2, bool redirectErrorStream)
        {
#if NETFRAMEWORK
            throw new NotImplementedException();
#else
            var psi = new ProcessStartInfo();
            psi.FileName = cmd[0];

            foreach (var arg in cmd.Skip(1))
                psi.ArgumentList.Add(arg);

            return Create(psi, env, dir, h0, h1, h2, redirectErrorStream);
#endif

        }

        /// <summary>
        /// Creates a new 'ProcessImpl' based on the filename and arguments configured on the <see cref="ProcessStartInfo"/>.
        /// </summary>
        /// <param name="psi"></param>
        /// <param name="env"></param>
        /// <param name="dir"></param>
        /// <param name="h0"></param>
        /// <param name="h1"></param>
        /// <param name="h2"></param>
        /// <param name="redirectErrorStream"></param>
        /// <returns></returns>
        static object Create(ProcessStartInfo psi, Dictionary<string, string> env, string dir, Stream h0, Stream h1, Stream h2, bool redirectErrorStream)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.WorkingDirectory = dir;

            if (env != null)
                foreach (var kvp in env)
                    psi.Environment.Add(kvp);

            // begin new process
            var process = Process.Start(psi);

            // if any of the handles are files, close files when process exits
            var f0 = h0 as FileStream;
            var f1 = h1 as FileStream;
            var f2 = h2 as FileStream;
            if (f0 != null || f1 != null || f2 != null)
            {
                process.EnableRaisingEvents = true;
                process.Exited += (s, a) =>
                {
                    f0?.Close();
                    f1?.Close();
                    f2?.Close();
                };
            }

            // base streams from process
            var b0 = process.StandardInput.BaseStream;
            var b1 = process.StandardOutput.BaseStream;
            var b2 = process.StandardError.BaseStream;

            // pipe input data into process standard input
            if (h0 != null)
            {
                ConnectPipe(h0, b0);
                h0 = null;
            }
            else
            {
                h0 = b0;
            }

            Stream stdoutDrain = null;

            if (h1 != null)
            {
                stdoutDrain = h1;
                ConnectPipe(b1, stdoutDrain);
                h1 = null;
            }
            else if (redirectErrorStream)
            {
                var pipe = new PipeStream();
                ConnectPipe(b1, pipe);
                ConnectPipe(b2, pipe);
                h1 = pipe;
            }
            else
            {
                h1 = b1;
            }

            if (redirectErrorStream)
            {
                if (stdoutDrain != null)
                    ConnectPipe(b2, stdoutDrain);
                h2 = null;
            }
            else if (h2 != null)
            {
                ConnectPipe(b2, h2);
                h2 = null;
            }
            else
            {
                h2 = b2;
            }

            // wrap streams for access from Java
            var s0 = h0 == null ? ProcessBuilderNullOutputStreamAccessor.GetInstance() : BufferedOutputStreamAccessor.Init(FileOutputStreamAccessor.Init2(FileDescriptorAccessor.FromStream(h0)));
            var s1 = h1 == null ? ProcessBuilderNullInputStreamAccessor.GetInstance() : BufferedInputStreamAccessor.Init(FileInputStreamAccessor.Init2(FileDescriptorAccessor.FromStream(h1)));
            var s2 = h2 == null ? ProcessBuilderNullInputStreamAccessor.GetInstance() : FileInputStreamAccessor.Init2(FileDescriptorAccessor.FromStream(h2));

            // return new process
            return ProcessImplAccessor.Init(process, s0, s1, s2);
#endif
        }

        class PipeStream : Stream
        {

            readonly byte[] buf = new byte[4096];
            int pos;
            int users = 2;

            public override bool CanRead => throw new NotImplementedException();

            public override bool CanSeek => throw new NotImplementedException();

            public override bool CanWrite => throw new NotImplementedException();

            public override long Length => throw new NotImplementedException();

            public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public override int Read(byte[] buffer, int offset, int count)
            {
                lock (this)
                {
                    if (count == 0)
                        return 0;

                    while (pos == 0)
                    {
                        try
                        {
                            Monitor.Wait(this);
                        }
                        catch (ThreadInterruptedException)
                        {

                        }
                    }

                    if (pos == -1)
                        return 0;

                    count = Math.Min(count, pos);
                    Array.Copy(buf, 0, buffer, offset, count);
                    pos -= count;

                    Array.Copy(buf, count, buf, 0, pos);
                    Monitor.PulseAll(this);
                    return count;
                }
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                lock (this)
                {
                    while (buf.Length - pos < count)
                    {
                        try
                        {
                            Monitor.Wait(this);
                        }
                        catch (ThreadInterruptedException)
                        {

                        }
                    }

                    Array.Copy(buffer, offset, buf, pos, count);
                    pos += count;
                    Monitor.PulseAll(this);
                }
            }

            public override void Close()
            {
                lock (this)
                {
                    if (--users == 0)
                    {
                        pos = -1;
                        Monitor.PulseAll(this);
                    }
                }
            }

            public override void Flush()
            {
                throw new NotImplementedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Begins reading from <paramref name="src"/> and writing to <see cref="to"/>.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        static void ConnectPipe(Stream src, Stream dst)
        {
            var buf = new byte[4096];

            void cb(IAsyncResult ar)
            {
                try
                {
                    int count = src.EndRead(ar);
                    if (count > 0)
                    {
                        dst.Write(buf, 0, count);
                        dst.Flush();
                        src.BeginRead(buf, 0, buf.Length, cb, null);
                    }
                    else
                    {
                        dst.Close();
                    }
                }
                catch
                {

                }
            };

            src.BeginRead(buf, 0, buf.Length, cb, null);
        }

        static void WaitForWithInterrupt(Process pid)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var current = ThreadAccessor.InvokeCurrentThread();

            // spin until thread is interrupted, or process exits
            while (!ThreadAccessor.InvokeIsInterrupted(current) && !pid.WaitForExit(100))
                continue;
#endif
        }

        public static int waitFor(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var pid = ProcessImplAccessor.GetProcess(self);
            if (pid.HasExited)
                return pid.ExitCode;

            // wait for the thread to be interrupted, or for the process to exit
            WaitForWithInterrupt(pid);

            // check if we were interrupted and throw
            if (ThreadAccessor.InvokeInterrupted())
                throw new global::java.lang.InterruptedException();

            return pid.ExitCode;
#endif
        }

        public static int exitValue(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var pid = ProcessImplAccessor.GetProcess(self);
            if (pid.HasExited == false)
                throw new global::java.lang.IllegalThreadStateException("process has not exited");

            return pid.ExitCode;
#endif
        }

        public static void destroy(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var pid = ProcessImplAccessor.GetProcess(self);
            if (pid.HasExited)
                return;

            try
            {
                pid.Kill();
            }
            catch
            {
                // ignore
            }
#endif
        }

        static FileStream OpenForAtomicAppend(string path)
        {
#if NETFRAMEWORK
            return new FileStream(path, FileMode.Append, FileSystemRights.AppendData, FileShare.ReadWrite, 1, FileOptions.None);
#else
            return new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 1, FileOptions.None);
#endif
        }

        /// <summary>
        /// Open a file for writing. If <paramref name="append"/> is <c>true</c> then the file is opened for atomic
        /// append directly and a FileOutputStream constructed with the resulting handle. This is because a
        /// FileOutputStream created to append to a file does not open the file in a manner that guarantees that writes
        /// by the child process will be atomic.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="append"></param>
        /// <returns></returns>
        static FileStream NewFileOutputStream(object f, bool append)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var path = FileAccessor.InvokeGetPath(f);
            var sm = SystemAccessor.InvokeGetSecurityManager();
            if (sm != null)
                SecurityManagerAccessor.InvokeCheckWrite(sm, path);

            if (append)
            {
                return OpenForAtomicAppend(path);
            }
            else
            {
                return File.OpenWrite(path);
            }
#endif
        }

    }

}
