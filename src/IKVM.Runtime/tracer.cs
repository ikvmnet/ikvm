/*
  Copyright (C) 2004, 2005, 2006 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;

namespace IKVM.Internal
{

    public static class Tracer
    {

        public readonly static TraceSwitch Compiler = new TraceSwitch("compiler", "Static Compiler");
        public readonly static TraceSwitch FxBug = new TraceSwitch("fxbug", ".NET Framework bug related events");
        public readonly static TraceSwitch ClassLoading = new TraceSwitch("classloading", "Class loading");
        public readonly static TraceSwitch Verifier = new TraceSwitch("verifier", "Bytecode Verifier");
        public readonly static TraceSwitch Runtime = new TraceSwitch("runtime", "Miscellaneous runtime events");
        public readonly static TraceSwitch Jni = new TraceSwitch("jni", "JNI");

        readonly static Dictionary<string, TraceSwitch> allTraceSwitches = new Dictionary<string, TraceSwitch>();
        readonly static List<string> methodtraces = new List<string>();

        sealed class MyTextWriterTraceListener : TextWriterTraceListener
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="tw"></param>
            internal MyTextWriterTraceListener(System.IO.TextWriter tw) :
                base(tw)
            {

            }

            public override void Fail(string message)
            {
                WriteLine($"Assert.Fail: {message}");
                WriteLine(new StackTrace(true).ToString());
                base.Fail(message);
            }

            public override void Fail(string message, string detailMessage)
            {
                WriteLine($"Assert.Fail: {message}.\n{detailMessage}");
                WriteLine(new StackTrace(true).ToString());
                base.Fail(message, detailMessage);
            }

        }

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static Tracer()
        {
            try
            {
                Init();
            }
            catch (System.Security.SecurityException)
            {

            }
        }

        private static void Init()
        {
            allTraceSwitches[Compiler.DisplayName] = Compiler;
            allTraceSwitches[FxBug.DisplayName] = FxBug;
            allTraceSwitches[ClassLoading.DisplayName] = ClassLoading;
            allTraceSwitches[Verifier.DisplayName] = Verifier;
            allTraceSwitches[Runtime.DisplayName] = Runtime;
            allTraceSwitches[Jni.DisplayName] = Jni;

#if NETFRAMEWORK

            try
            {
                Trace.AutoFlush = true;

#if !EXPORTER
                /* If the app config file gives some method trace - add it */
                var trace = ConfigurationManager.AppSettings["Traced Methods"];
                if (trace != null)
                    methodtraces.Add(trace);
#endif
            }
            catch (ConfigurationException)
            {
                // app.config is malformed, ignore
            }
#endif
        }

        [Conditional("DEBUG")]
        public static void EnableTraceForDebug()
        {
            SetTraceLevel("*", TraceLevel.Verbose);
        }

        /// <summary>
        /// Enables trace output to to the console.
        /// </summary>
        public static void EnableTraceConsoleListener()
        {
            Trace.Listeners.Add(new MyTextWriterTraceListener(Console.Error));
        }

        public static void HandleMethodTrace(string name)
        {
            methodtraces.Add(name);
        }

        public static bool IsTracedMethod(string name)
        {
            foreach (var trace in methodtraces)
            {
                try
                {
                    if (Regex.IsMatch(name, trace))
                        return true;
                }
                catch (Exception x)
                {
                    Tracer.Warning(Tracer.Compiler, $"Regular Expression match failure: {trace}:{x}");
                }
            }

            return false;
        }

        public static void SetTraceLevel(string name)
        {
            var trace = name.Split('=');
            var level = TraceLevel.Verbose;

            if (trace.Length == 2)
                level = (TraceLevel)Enum.Parse(typeof(TraceLevel), trace[1], true);

            SetTraceLevel(trace[0], level);
        }

        public static void SetTraceLevel(string name, TraceLevel level)
        {
            if (name == "*")
            {
                foreach (var ts in allTraceSwitches.Values)
                    ts.Level = level;
            }
            else
            {
                if (allTraceSwitches.TryGetValue(name, out var ts))
                {
                    ts.Level = level;
                }
                else
                {
                    Console.Error.WriteLine("Warning: Invalid traceswitch name: {0}", name);
                }
            }
        }

        static void WriteLine(string message, object[] p)
        {
            if (p.Length > 0)
                message = string.Format(message, p);

            Trace.WriteLine($"[{DateTime.Now:HH':'mm':'ss'.'fffff} {Thread.CurrentThread.Name}] {message}");
        }

        [Conditional("TRACE")]
        public static void Info(TraceSwitch traceSwitch, string message, params object[] p)
        {
            if (traceSwitch.TraceInfo)
                WriteLine(message, p);
        }

        [Conditional("TRACE")]
        public static void MethodInfo(string message)
        {
            Trace.WriteLine($"[{DateTime.Now:HH':'mm':'ss'.'fffff} {Thread.CurrentThread.Name}] {message}");
        }

        [Conditional("TRACE")]
        public static void Warning(TraceSwitch traceSwitch, string message, params object[] p)
        {
            if (traceSwitch.TraceWarning)
                WriteLine(message, p);
        }

        [Conditional("TRACE")]
        public static void Error(TraceSwitch traceSwitch, string message, params object[] p)
        {
            if (traceSwitch.TraceError)
                WriteLine(message, p);
        }

    }

}
