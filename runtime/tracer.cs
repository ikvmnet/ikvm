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
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Text.RegularExpressions;
using System.Configuration;

namespace IKVM.Internal
{
	public class Tracer
	{
#if !COMPACT_FRAMEWORK
		public readonly static TraceSwitch Compiler = new TraceSwitch("compiler", "Static Compiler");
		public readonly static TraceSwitch FxBug = new TraceSwitch("fxbug", ".NET Framework bug related events");
		public readonly static TraceSwitch ClassLoading = new TraceSwitch("classloading", "Class loading");
		public readonly static TraceSwitch Verifier = new TraceSwitch("verifier", "Bytecode Verifier");
		public readonly static TraceSwitch Runtime = new TraceSwitch("runtime", "Miscellaneous runtime events");
		public readonly static TraceSwitch Jni = new TraceSwitch("jni", "JNI");
		//	public readonly static TraceSwitch Methods = new TraceSwitch("methods", "Method Trace");
		private readonly static Hashtable allTraceSwitches = new Hashtable();

		private readonly static ArrayList methodtraces = new ArrayList();

		private class MyTextWriterTraceListener : TextWriterTraceListener
		{
			internal MyTextWriterTraceListener(System.IO.TextWriter tw)
				: base(tw)
			{
			}

			public override void Fail(string message)
			{
				this.WriteLine("Assert.Fail: " + message);
				this.WriteLine(new StackTrace(true).ToString());
				base.Fail(message);
			}

			public override void Fail(string message, string detailMessage)
			{
				this.WriteLine("Assert.Fail: " + message + ".\n" + detailMessage);
				this.WriteLine(new StackTrace(true).ToString());
				base.Fail(message, detailMessage);
			}
		}

		static Tracer()
		{
			allTraceSwitches[Compiler.DisplayName] = Compiler;
			allTraceSwitches[FxBug.DisplayName] = FxBug;
			allTraceSwitches[ClassLoading.DisplayName] = ClassLoading;
			allTraceSwitches[Verifier.DisplayName] = Verifier;
			allTraceSwitches[Runtime.DisplayName] = Runtime;
			allTraceSwitches[Jni.DisplayName] = Jni;

			try
			{
				Trace.AutoFlush = true;
				Trace.Listeners.Add(new MyTextWriterTraceListener(Console.Error));
				/* If the app config file gives some method trace - add it */
				string trace = ConfigurationManager.AppSettings["Traced Methods"];
				if(trace != null)
				{
					methodtraces.Add(trace);
				}
			}
			catch(ConfigurationException)
			{
				// app.config is malformed, ignore
			}
		}

		[Conditional("DEBUG")]
		public static void EnableTraceForDebug()
		{
			SetTraceLevel("*", TraceLevel.Verbose);
		}

		public static void HandleMethodTrace(string name)
		{
			methodtraces.Add(name);
		}

		public static bool IsTracedMethod(string name)
		{
			if(methodtraces.Count == 0)
			{
				return false;
			}
			IEnumerator e = methodtraces.GetEnumerator();
			while(e.MoveNext())
			{
				try 
				{
					if(Regex.IsMatch(name, e.Current.ToString()))
					{
						return true;
					}
				}
				catch(Exception x) 
				{
					Tracer.Warning(Tracer.Compiler,"Regular Expression match failure: " + e.Current + ":" + x);
				}
			}
			return false;
		}

		public static void SetTraceLevel(string name)
		{
			string[] trace = name.Split('=');
			System.Diagnostics.TraceLevel level = System.Diagnostics.TraceLevel.Verbose;
			if(trace.Length == 2)
			{
				level = (System.Diagnostics.TraceLevel)Enum.Parse(typeof(System.Diagnostics.TraceLevel), trace[1], true);
			}
			SetTraceLevel(trace[0], level);
		}

		public static void SetTraceLevel(string name, TraceLevel level)
		{
			if(name == "*")
			{
				foreach(TraceSwitch ts in allTraceSwitches.Values)
				{
					ts.Level = level;
				}
			}
			else
			{
				TraceSwitch ts = (TraceSwitch)allTraceSwitches[name];
				if(ts != null)
				{
					ts.Level = level;
				}
				else
				{
					Console.Error.WriteLine("Warning: Invalid traceswitch name: {0}", name);
				}
			}
		}

		private static void WriteLine(string message, object[] p)
		{
			if(p.Length > 0)
			{
				message = string.Format(message, p);
			}
			Trace.WriteLine(string.Format("[{0:HH':'mm':'ss'.'fffff} {1}] {2}", DateTime.Now, Thread.CurrentThread.Name, message));
		}

		[Conditional("TRACE")]
		public static void Info(TraceSwitch traceSwitch, string message, params object[] p)
		{
			if(traceSwitch.TraceInfo)
			{
				WriteLine(message, p);
			}
		}

		[Conditional("TRACE")]
		public static void MethodInfo(string message)
		{
			Trace.WriteLine(string.Format("[{0:HH':'mm':'ss'.'fffff} {1}] {2}", DateTime.Now, Thread.CurrentThread.Name, message));
		}

		[Conditional("TRACE")]
		public static void Warning(TraceSwitch traceSwitch, string message, params object[] p)
		{
			if(traceSwitch.TraceWarning)
			{
				WriteLine(message, p);
			}
		}

		[Conditional("TRACE")]
		public static void Error(TraceSwitch traceSwitch, string message, params object[] p)
		{
			if(traceSwitch.TraceError)
			{
				WriteLine(message, p);
			}
		}
#else
		public const int Compiler = 0;
		public const int FxBug = 0;
		public const int ClassLoading = 0;
		public const int Verifier = 0;
		public const int Runtime = 0;
		public const int Jni = 0;

		[Conditional("NEVER")]
		public static void Info(int traceSwitch, string message, params object[] p)
		{
		}

		[Conditional("NEVER")]
		public static void Error(int traceSwitch, string message, params object[] p)
		{
		}

		[Conditional("NEVER")]
		public static void Warning(int traceSwitch, string message, params object[] p)
		{
		}
#endif
	}
}
