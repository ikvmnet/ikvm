using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

using IKVM.Internal;

namespace IKVM.Runtime
{

    /// <summary>
    /// Utility for launching a Java class from a main entry point. Parses JVM command line options, then passes the
    /// remainder through to the underlying main method.
    /// </summary>
    public static class Launcher
    {

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
        /// <param name="arg"></param>
        /// <returns></returns>
        static IEnumerable<string> Glob(string arg)
        {
            var dir = Path.GetDirectoryName(arg);
            if (dir == "")
                dir = null;

            var exp = false;
            foreach (var fsi in new DirectoryInfo(dir ?? Environment.CurrentDirectory).GetFileSystemInfos(Path.GetFileName(arg)))
            {
                exp = true;
                yield return dir != null ? Path.Combine(dir, fsi.Name) : fsi.Name;
            }

            if (exp == false)
                yield return arg;
        }

        static string[] Glob(string[] args, int skip)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                var vmargs = new string[args.Length - skip];
                Array.Copy(args, skip, vmargs, 0, args.Length - skip);
                return vmargs;
            }
            else
            {
                var list = new List<string>();
                for (var i = skip; i < args.Length; i++)
                {
                    var arg = args[i];
                    if (arg.IndexOf('*') != -1 || arg.IndexOf('?') != -1)
                        list.AddRange(Glob(arg));
                    else
                        list.Add(arg);
                }

                return list.ToArray();
            }
        }

        /// <summary>
        /// Sets the startup properties.
        /// </summary>
        /// <param name="properties"></param>
        static void SetProperties(IDictionary properties)
        {
#if FIRST_PASS || STATIC_COMPILER
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
#if FIRST_PASS || STATIC_COMPILER
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
#if FIRST_PASS || STATIC_COMPILER
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
#if FIRST_PASS || STATIC_COMPILER
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
#if FIRST_PASS || STATIC_COMPILER
            throw new NotImplementedException();
#else
            ClassLoaderWrapper.GetBootstrapClassLoader().AddDelegate(AssemblyClassLoader.FromAssembly(assembly));
#endif
        }

        /// <summary>
        /// Splits the given set of input arguments into two output sets, the first being those args which are passed
        /// to the JVM, while the second is those args which are passed to the application.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="jvmArgs"></param>
        /// <param name="appArgs"></param>
        /// <param name="rarg"></param>
        internal static void SplitArguments(IEnumerable<string> args, out IEnumerable<string> jvmArgs, out IEnumerable<string> appArgs, string rarg = "-J")
        {
            jvmArgs = args.Where(i => rarg != null).Where(i => i.StartsWith(rarg)).Select(i => i.Substring(rarg.Length));
            appArgs = args.Where(i => rarg == null || i.StartsWith(rarg) == false);
        }

        /// <summary>
        /// Accepts the given command line arguments, default main value and argument information, and executes
        /// the application.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="main"></param>
        /// <param name="jar"></param>
        /// <param name="rarg"></param>
        /// <returns></returns>
        public static int Execute(IEnumerable<string> args, string main = null, bool jar = false, string rarg = "-J")
        {
            SplitArguments(args, out var jvmArgs, out var appArgs, rarg);
            return Execute(jvmArgs, appArgs, main, jar);
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
        /// Executes the main class, given the input JVM args and input app arguments. The JVM arguments can potentially override the main and jar settings.
        /// </summary>
        /// <param name="jvmArgs"></param>
        /// <param name="appArgs"></param>
        /// <param name="main"></param>
        /// <param name="jar"></param>
        internal static int Execute(IEnumerable<string> jvmArgs, IEnumerable<string> appArgs, string main = null, bool jar = false)
        {
#if FIRST_PASS || STATIC_COMPILER
            throw new NotImplementedException();
#else
            var properties = new Hashtable();
            var showversion = false;
            var hasMainArg = false;
            var appArgList = new List<string>(appArgs);
            var setAppArgs = false;

            // classpath from environment by default
            if (Environment.GetEnvironmentVariable("CLASSPATH") is string cp && !string.IsNullOrEmpty(cp))
                properties["java.class.path"] = cp;

            // process through each incoming argument
            for (var jvmArg = jvmArgs.GetEnumerator(); jvmArg.MoveNext();)
            {
                var arg = jvmArg.Current.AsSpan();
                if (arg.StartsWith("-".AsSpan()))
                {
                    // define system property
                    if (arg.StartsWith("-D".AsSpan()))
                    {
                        var sep = arg.IndexOf('=');
                        var key = sep > -1 ? arg.Slice(0, sep) : "".AsSpan();
                        var val = sep > -1 ? arg.Slice(sep) : "".AsSpan();
                        properties.Add(key.ToString(), val.ToString());
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
                            Assertions.EnableAssertions(arg.Slice(v).ToString());
                    }

                    if (ArgEquals(arg, "-da") || ArgEquals(arg, "-disableassertions"))
                    {
                        Assertions.DisableAssertions();
                        continue;
                    }

                    if (arg.StartsWith("-da:".AsSpan()) || arg.StartsWith("-disableassertions:".AsSpan()))
                    {
                        if (arg.IndexOf(':') is int v && v > -1)
                            Assertions.DisableAssertions(arg.Slice(v).ToString());
                    }

                    if (ArgEquals(arg, "-esa") || ArgEquals(arg, " - enablesystemassertions"))
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

                        properties.Add("java.class.path", jvmArg.Current);
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
                            Tracer.SetTraceLevel(arg.Slice(v).ToString());

                        continue;
                    }

                    if (arg.StartsWith("-Xmethodtrace:".AsSpan()))
                    {
                        if (arg.IndexOf(':') is int v && v > -1)
                            Tracer.HandleMethodTrace(arg.Slice(v).ToString());

                        continue;
                    }

                    if (arg.StartsWith("-Xreference:".AsSpan()))
                    {
                        if (arg.IndexOf(':') is int v && v > -1)
                            AddBootClassPathAssembly(Assembly.LoadFrom(arg.Slice(v).ToString()));

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
                    break;
                }
                else
                {
                    // indicate we're resetting the application arguments
                    if (setAppArgs == false)
                    {
                        setAppArgs = true;
                        appArgList.Clear();
                    }

                    // append new application argument
                    appArgList.Add(arg.ToString());
                }
            }

            try
            {
                // if a jar file is specified, we're going to set the classpath to the jar itself
                if (jar)
                    properties["java.class.path"] = main;

                // like the JDK we don't quote the args (even if they contain spaces)
                properties["sun.java.command"] = string.Join(" ", Prepend(appArgList, main));
                properties["sun.java.launcher"] = "SUN_STANDARD";

                // apply the loaded VM properties
                SetProperties(properties);
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
                java.lang.System.setProperty("sun.java.command", (string)properties["sun.java.command"]);

                // find the main method and ensure it is accessible
                var method = clazz.getMethod("main", typeof(string[]));
                method.setAccessible(true);

                try
                {
                    // invoke main method, which is responsible for exit
                    method.invoke(null, new object[] { appArgList.ToArray() });
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
