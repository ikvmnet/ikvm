using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

using IKVM.Attributes;
using IKVM.Internal;

namespace IKVM.Runtime
{

    /// <summary>
    /// Utility for launching a Java class from a main entry point. Parses JVM command line options, then passes the
    /// remainder through to the underlying Java main method.
    /// </summary>
    static class Launcher
    {

        /// <summary>
        /// Data sent as part of the debug ping protocol.
        /// </summary>
        class IkvmStartEvent
        {

            [JsonPropertyName("processId")]
            public int ProcessId { get; set; }

        }

        /// <summary>
        /// Returns an enumeration with the given argument prepended.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        static IEnumerable<string> Prepend(IEnumerable<string> source, string value)
        {
            yield return value;
            foreach (var i in source)
                yield return i;
        }

        /// <summary>
        /// Applies a glob to the given argument value, potentially returning multiple expanded arguments.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static string[] Glob(string path)
        {
            try
            {
                var dir = Path.GetDirectoryName(path);
                if (dir == "")
                    dir = null;

                var list = new List<string>();
                foreach (var fsi in new DirectoryInfo(dir ?? Environment.CurrentDirectory).GetFileSystemInfos(Path.GetFileName(path)))
                    list.Add(dir != null ? Path.Combine(dir, fsi.Name) : fsi.Name);

                if (list.Count == 0)
                    return new string[] { path };

                return list.ToArray();
            }
            catch
            {
                return new string[] { path };
            }
        }

        /// <summary>
        /// Applies a glob to the given arguments.
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        static string[] Glob(string[] paths)
        {
            var list = new List<string>();
            for (var i = 0; i < paths.Length; i++)
            {
                var path = paths[i];
                if (path.IndexOf('*') != -1 || path.IndexOf('?') != -1)
                    list.AddRange(Glob(path));
                else
                    list.Add(path);
            }

            return list.ToArray();
        }

        /// <summary>
        /// Sets the startup properties.
        /// </summary>
        /// <param name="properties"></param>
        static void SetProperties(IDictionary properties)
        {
#if FIRST_PASS || IMPORTER
            throw new NotImplementedException();
#else
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));

            IKVM.Java.Externs.java.lang.VMSystemProperties.ImportProperties = properties;
#endif
        }

        /// <summary>
        /// Invoked at the enter of the main thread.
        /// </summary>
        static void EnterMainThread()
        {
#if FIRST_PASS || IMPORTER
            throw new NotImplementedException();
#else
            if (Thread.CurrentThread.Name == null)
            {
                try
                {
                    if (false)
                        throw new InvalidOperationException();

                    Thread.CurrentThread.Name = "main";
                }
                catch (InvalidOperationException)
                {

                }
            }

            java.lang.Thread.currentThread();

            try
            {
                sun.misc.Signal.handle(new sun.misc.Signal("BREAK"), sun.misc.SignalHandler.SIG_DFL);
            }
            catch (java.lang.IllegalArgumentException)
            {
                // ignore it;
            }
#endif
        }

        /// <summary>
        /// Invoked at the exit of the main thread.
        /// </summary>
        static void ExitMainThread()
        {
#if FIRST_PASS || IMPORTER
            throw new NotImplementedException();
#else
            // FXBUG when the main thread ends, it doesn't actually die, it stays around to manage the lifetime
            // of the CLR, but in doing so it also keeps alive the thread local storage for this thread and we
            // use the TLS as a hack to track when the thread dies (if the object stored in the TLS is finalized,
            // we know the thread is dead). So to make that work for the main thread, we use jniDetach which
            // explicitly cleans up our thread.
            java.lang.Thread.currentThread().die();
#endif
        }

        /// <summary>
        /// Gets a string containing the runtime version and copyright information.
        /// </summary>
        /// <returns></returns>
        static string GetVersionAndCopyrightInfo()
        {
            var assembly = typeof(Launcher).Assembly;

            var description = assembly.GetCustomAttributes<AssemblyTitleAttribute>().FirstOrDefault();
            if (description is not null)
            {
                var copyright = assembly.GetCustomAttributes<AssemblyCopyrightAttribute>().FirstOrDefault();
                if (copyright is not null)
                    return $"{description.Title} version {assembly.GetName().Version}{Environment.NewLine}{copyright.Copyright}";
            }

            return "";
        }

        /// <summary>
        /// Prints the version information.
        /// </summary>
        static void PrintVersion()
        {
#if FIRST_PASS || IMPORTER
            throw new NotImplementedException();
#else
            Console.WriteLine(GetVersionAndCopyrightInfo());
            Console.WriteLine("CLR version: {0} ({1} bit)", Environment.Version, IntPtr.Size * 8);
            var ver = java.lang.System.getProperty("openjdk.version");
            if (ver != null)
                Console.WriteLine("OpenJDK version: {0}", ver);
#endif
        }

        /// <summary>
        /// Adds the given assembly to the boot classpath.
        /// </summary>
        /// <param name="asm"></param>
        static void AddBootClassPathAssembly(Assembly assembly)
        {
#if FIRST_PASS || IMPORTER
            throw new NotImplementedException();
#else
            ClassLoaderWrapper.GetBootstrapClassLoader().AddDelegate(AssemblyClassLoader.FromAssembly(assembly));
#endif
        }

        /// <summary>
        /// Checks whether the given argument value equals the specified string.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static bool ArgEquals(ReadOnlySpan<char> a, string b)
        {
            return a.Equals(b.AsSpan(), StringComparison.Ordinal);
        }

        /// <summary>
        /// Launches a Java application.
        /// </summary>
        /// <param name="main"></param>
        /// <param name="jar"></param>
        /// <param name="args"></param>
        /// <param name="rarg"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        [HideFromJava(HideFromJavaFlags.StackTrace)]
        public static int Run(string main, bool jar, string[] args, string rarg, IDictionary<string, string> properties)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

#if FIRST_PASS || IMPORTER
            throw new NotImplementedException();
#else
            HandleDebugTrace();

            var initialize = properties != null ? new Dictionary<string, string>(properties) : new Dictionary<string, string>();
            var showversion = false;
            var hasMainArg = false;
            var jvmArgs = args.Where(i => rarg != null).Where(i => i.StartsWith(rarg)).Select(i => i.Substring(rarg.Length)).ToList();
            var appArgs = args.Where(i => rarg == null || i.StartsWith(rarg) == false).ToList();
            var appArgsReset = false;

            // classpath from environment by default
            initialize["java.class.path"] = ".";
            if (Environment.GetEnvironmentVariable("CLASSPATH") is string cp && !string.IsNullOrEmpty(cp))
                initialize["java.class.path"] = string.Join(Path.PathSeparator.ToString(), Glob(cp.Split(Path.PathSeparator)));

            // ikvm.home from environment by default
            initialize["ikvm.home"] = "ikvm";
            if (Environment.GetEnvironmentVariable("IKVM_HOME") is string ih && !string.IsNullOrEmpty(ih))
                initialize["ikvm.home"] = ih;

            // process through each incoming argument
            for (var jvmArg = jvmArgs.GetEnumerator(); jvmArg.MoveNext();)
            {
                var arg = jvmArg.Current.AsSpan();
                if (hasMainArg == false && arg.StartsWith("-".AsSpan()))
                {
                    // define system property
                    if (arg.StartsWith("-D".AsSpan()))
                    {
                        var def = arg.Slice(2);
                        var sep = def.IndexOf('=');
                        var key = sep > -1 ? def.Slice(0, sep) : def;
                        var val = sep > -1 ? def.Slice(sep + 1) : "".AsSpan();
                        initialize[key.ToString()] = val.ToString();
                        continue;
                    }

                    if (ArgEquals(arg, "-ea") || ArgEquals(arg, "-enableassertions"))
                    {
                        Assertions.EnableAssertions();
                        continue;
                    }

                    if (arg.StartsWith("-ea:".AsSpan()) || arg.StartsWith("-enableassertions:".AsSpan()))
                    {
                        if (arg.IndexOf(':') is int v && v > -1)
                            Assertions.EnableAssertions(arg.Slice(v + 1).ToString());
                    }

                    if (ArgEquals(arg, "-da") || ArgEquals(arg, "-disableassertions"))
                    {
                        Assertions.DisableAssertions();
                        continue;
                    }

                    if (arg.StartsWith("-da:".AsSpan()) || arg.StartsWith("-disableassertions:".AsSpan()))
                    {
                        if (arg.IndexOf(':') is int v && v > -1)
                            Assertions.DisableAssertions(arg.Slice(v + 1).ToString());
                    }

                    if (ArgEquals(arg, "-esa") || ArgEquals(arg, "-enablesystemassertions"))
                    {
                        Assertions.EnableSystemAssertions();
                        continue;
                    }

                    if (ArgEquals(arg, "-dsa") || ArgEquals(arg, "-disablesystemassertions"))
                    {
                        Assertions.DisableSystemAssertions();
                        continue;
                    }

                    if (ArgEquals(arg, "-cp") || ArgEquals(arg, "-classpath"))
                    {
                        if (jvmArg.MoveNext() == false)
                        {
                            Console.Error.WriteLine("Error: {0} requires class path specification", arg.ToString());
                            PrintHelp();
                            return 1;
                        }

                        initialize["java.class.path"] = string.Join(Path.PathSeparator.ToString(), Glob(jvmArg.Current.Split(Path.PathSeparator)));
                        continue;
                    }

                    if (ArgEquals(arg, "-version"))
                    {
                        PrintVersion();
                        return 0;
                    }

                    if (ArgEquals(arg, "-showversion"))
                    {
                        showversion = true;
                        continue;
                    }

                    if (ArgEquals(arg, "-jar"))
                    {
                        jar = true;
                        continue;
                    }

                    if (ArgEquals(arg, "-?") || ArgEquals(arg, "-help"))
                    {
                        PrintHelp();
                        return 1;
                    }

                    if (ArgEquals(arg, "-X"))
                    {
                        PrintXHelp();
                        return 1;
                    }

                    if (ArgEquals(arg, "-Xtime"))
                    {
                        Console.Error.WriteLine("Unrecognized option: {0}", arg.ToString());
                        return 1;
                    }

                    if (ArgEquals(arg, "-Xbreak"))
                    {
                        Debugger.Break();
                        continue;
                    }

                    if (ArgEquals(arg, "-Xnoclassgc"))
                    {
#if CLASSGC
                        JVM.classUnloading = false;
#endif
                        continue;
                    }

                    if (ArgEquals(arg, "-Xverify"))
                    {
                        JVM.relaxedVerification = false;
                        continue;
                    }

                    if (arg.StartsWith("-Xtrace:".AsSpan()))
                    {
                        if (arg.IndexOf(':') is int v && v > -1)
                            Tracer.SetTraceLevel(arg.Slice(v + 1).ToString());

                        continue;
                    }

                    if (arg.StartsWith("-Xmethodtrace:".AsSpan()))
                    {
                        if (arg.IndexOf(':') is int v && v > -1)
                            Tracer.HandleMethodTrace(arg.Slice(v + 1).ToString());

                        continue;
                    }

                    if (arg.StartsWith("-Xreference:".AsSpan()))
                    {
                        if (arg.IndexOf(':') is int v && v > -1)
                            AddBootClassPathAssembly(Assembly.LoadFrom(arg.Slice(v + 1).ToString()));

                        continue;
                    }

                    if (ArgEquals(arg, "-XX:+AllowNonVirtualCalls"))
                    {
                        JVM.AllowNonVirtualCalls = true;
                        continue;
                    }

                    if (arg.StartsWith("-Xms".AsSpan()) ||
                        arg.StartsWith("-Xmx".AsSpan()) ||
                        arg.StartsWith("-Xmn".AsSpan()) ||
                        arg.StartsWith("-Xss".AsSpan()) ||
                        arg.StartsWith("-XX:".AsSpan()) ||
                        ArgEquals(arg, "-Xmixed") ||
                        ArgEquals(arg, "-Xint") ||
                        ArgEquals(arg, "-Xincgc") ||
                        ArgEquals(arg, "-Xbatch") ||
                        ArgEquals(arg, "-Xfuture") ||
                        ArgEquals(arg, "-Xrs") ||
                        ArgEquals(arg, "-Xcheck:jni") ||
                        ArgEquals(arg, "-Xshare:off") ||
                        ArgEquals(arg, "-Xshare:auto") ||
                        ArgEquals(arg, "-Xshare:on"))
                    {
                        Console.Error.WriteLine("Ignoring unrecognized option: {0}", arg.ToString());
                        continue;
                    }

                    Console.Error.WriteLine("Unrecognized option: {0}", arg.ToString());
                    return 1;
                }
                else if (hasMainArg == false)
                {
                    hasMainArg = true;
                    main = arg.ToString();
                    continue;
                }
                else
                {
                    // indicate we're resetting the application arguments
                    if (appArgsReset == false)
                    {
                        appArgsReset = true;
                        appArgs.Clear();
                    }

                    // append new application argument
                    appArgs.Add(arg.ToString());
                    continue;
                }
            }

            try
            {
                // if a jar file is specified, we're going to set the classpath to the jar itself
                if (jar)
                    initialize["java.class.path"] = main;

                // like the JDK we don't quote the args (even if they contain spaces)
                initialize["sun.java.command"] = string.Join(" ", Prepend(appArgs, main));
                initialize["sun.java.launcher"] = "SUN_STANDARD";

                // apply the loaded VM properties
                SetProperties(initialize);
                EnterMainThread();

                // we were instructed to show the version
                if (showversion)
                    PrintVersion();

                // we require a main argument, either a class name or jar file
                if (main == null)
                {
                    PrintHelp();
                    return 1;
                }

                // process the main argument, returning the true value, and resetting the command property to match
                var clazz = sun.launcher.LauncherHelper.checkAndLoadMain(true, jar ? 2 : 1, main);
                java.lang.System.setProperty("sun.java.command", initialize["sun.java.command"]);

                // find the main method and ensure it is accessible
                var method = clazz.getMethod("main", typeof(string[]));
                method.setAccessible(true);

                try
                {
                    // invoke main method, which is responsible for exit
                    method.invoke(null, new object[] { appArgs.ToArray() });
                    return 0;
                }
                catch (java.lang.reflect.InvocationTargetException e)
                {
                    throw e.getCause();
                }
            }
            catch (Exception e)
            {
                var thread = java.lang.Thread.currentThread();
                thread.getThreadGroup().uncaughtException(thread, ikvm.runtime.Util.mapException(e));
            }
            finally
            {
                ExitMainThread();
            }

            return 1;
#endif
        }

        /// <summary>
        /// Handles the configured IKVM debug settings.
        /// </summary>
        static void HandleDebugTrace()
        {
            var debugWait = 0;
            var debugUri = (Uri)null;

            // wait some number of seconds for a debugger to attach
            if (Environment.GetEnvironmentVariable("IKVM_DEBUG_WAIT") is string debugWait_)
                if (int.TryParse(debugWait_, out var i))
                    debugWait = i;

            // send a ping message to the given hostname and port to signal a debugger to attach
            if (Environment.GetEnvironmentVariable("IKVM_DEBUG_URI") is string debugUri_)
                if (Uri.TryCreate(debugUri_, UriKind.Absolute, out var u))
                    debugUri = u;

            // send a start event to the host
            if (debugUri != null)
            {
                if (debugUri.Scheme == "tcp" &&  IPAddress.TryParse(debugUri.Host, out var ip) && debugUri.Port > 0)
                {
                    var message = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new IkvmStartEvent() { ProcessId = Process.GetCurrentProcess().Id, }));
                    using var c = new TcpClient();
                    c.Connect(new IPEndPoint(ip, debugUri.Port));
                    c.GetStream().Write(message, 0, message.Length);
                    c.GetStream().WriteByte(0);
                    c.GetStream().Flush();
                    c.Close();
                }
                else
                {
                    Console.Error.WriteLine("Invalid debug URI: {0}", debugUri);
                }
            }

            // wait for debugger to attach
            if (debugWait > 0)
            {
                Console.Write("Waiting for debugger...");

                // waits for the debugger to be attached
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(debugWait));
                while (Debugger.IsAttached == false && cts.IsCancellationRequested == false)
                {
                    Thread.Sleep(1000);
                    Console.Write(".");
                }

                Console.WriteLine();

                // not attached, and cancelled?
                if (Debugger.IsAttached == false && cts.IsCancellationRequested)
                    Console.Error.WriteLine("Debugger wait timed out.");
            }
        }

        /// <summary>
        /// Prints out the standard launcher help information.
        /// </summary>
        public static void PrintHelp()
        {
            var exe = Process.GetCurrentProcess().ProcessName;
            Console.Error.WriteLine("Usage: {0} [-options] class [args...]", exe);
            Console.Error.WriteLine("          (to execute a class)");
            Console.Error.WriteLine("    or {0} [-options] -jar jarfile [args...]", exe);
            Console.Error.WriteLine("          (to execute a jar file)");
            Console.Error.WriteLine();
            Console.Error.WriteLine("where options include:");
            Console.Error.WriteLine("    -cp <class search path of directories and zip/jar files>");
            Console.Error.WriteLine("    -classpath <class search path of directories and zip/jar files>");
            Console.Error.WriteLine("                  A {0} separated list of directories, JAR archives,", System.IO.Path.PathSeparator);
            Console.Error.WriteLine("                  and ZIP archives to search for class files.");
            Console.Error.WriteLine("    -D<name>=<value>");
            Console.Error.WriteLine("                  set a system property");
            Console.Error.WriteLine("    -version      print product version and exit");
            Console.Error.WriteLine("    -showversion  print product version and continue");
            Console.Error.WriteLine("    -? -help      Display this message");
            Console.Error.WriteLine("    -X            Display non-standard options");
            Console.Error.WriteLine("    -ea[:<packagename>...|:<classname>]");
            Console.Error.WriteLine("    -enableassertions[:<packagename>...|:<classname>]");
            Console.Error.WriteLine("                   enable assertions with specified granularity");
            Console.Error.WriteLine("    -da[:<packagename>...|:<classname>]");
            Console.Error.WriteLine("    -disableassertions[:<packagename>...|:<classname>]");
            Console.Error.WriteLine("                   disable assertions with specified granularity");
            Console.Error.WriteLine("    -esa | -enablesystemassertions");
            Console.Error.WriteLine("                   enable system assertions");
            Console.Error.WriteLine("    -dsa    | -enablesystemassertions");
            Console.Error.WriteLine("                   disable system assertions");
            Console.Error.WriteLine("    -agentlib:<libname>[=<options>]");
            Console.Error.WriteLine("                   load native agent library <libname>, e.g. -agentlib:hprof");
            Console.Error.WriteLine("                   see also, -agentlib:jdwp=help and -agentlib:hprof=help");
            Console.Error.WriteLine("    -agentpath:<pathname>[=<options>]");
            Console.Error.WriteLine("                   load native agent library by full pathname");
            Console.Error.WriteLine("    -javaagent:<jarpath>[=<options>]");
            Console.Error.WriteLine("                   load Java programming language agent, see java.lang.instrument");
            Console.Error.WriteLine("    -splash:<imagepath>");
            Console.Error.WriteLine("                   show splash screen with specified image");
            Console.Error.WriteLine("See http://www.oracle.com/technetwork/java/javase/documentation/index.html for more details.");
            Console.Error.WriteLine("");
        }

        /// <summary>
        /// Prints out the extended launcher help information.
        /// </summary>
        public static void PrintXHelp()
        {
            Console.Error.WriteLine("    -Xnoclassgc    disable class garbage collection");
            Console.Error.WriteLine("    -Xtime         time the execution");
            Console.Error.WriteLine("    -Xwait         keep process hanging around after exit");
            Console.Error.WriteLine("    -Xbreak        trigger a user defined breakpoint at startup");
            Console.Error.WriteLine("    -Xnoglobbing   Disable argument globbing");
            Console.Error.WriteLine("    -Xverify       Enable strict class file verification");
            Console.Error.WriteLine("    -Xtrace:<string>");
            Console.Error.WriteLine("                   Displays all tracepoints with the given name");
            Console.Error.WriteLine("    -Xmethodtrace:<string>");
            Console.Error.WriteLine("                      Builds method trace into the specified output methods");
            Console.Error.WriteLine();
            Console.Error.WriteLine("The -X options are non-standard and subject to change without notice.");
            Console.Error.WriteLine();
        }

    }

}
