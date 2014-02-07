/*
  Copyright (C) 2007-2014 Jeroen Frijters
  Copyright (C) 2009 Volker Berlin (i-net software)

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
using System.Diagnostics;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using IKVM.Internal;

static class Java_sun_misc_GC
{
	public static long maxObjectInspectionAge()
	{
		return 0;
	}
}

static class Java_sun_misc_MessageUtils
{
	public static void toStderr(string msg)
	{
		Console.Error.Write(msg);
	}

	public static void toStdout(string msg)
	{
		Console.Out.Write(msg);
	}
}

static class Java_sun_misc_MiscHelper
{
	public static object getAssemblyClassLoader(Assembly asm, object extcl)
	{
		if (extcl == null || asm.IsDefined(typeof(IKVM.Attributes.CustomAssemblyClassLoaderAttribute), false))
		{
			return AssemblyClassLoader.FromAssembly(asm).GetJavaClassLoader();
		}
		return null;
	}
}

static class Java_sun_misc_Signal
{
	/* derived from version 6.0 VC98/include/signal.h */
	private const int SIGINT = 2;       /* interrupt */
	private const int SIGILL = 4;       /* illegal instruction - invalid function image */
	private const int SIGFPE = 8;       /* floating point exception */
	private const int SIGSEGV = 11;     /* segment violation */
	private const int SIGTERM = 15;     /* Software termination signal from kill */
	private const int SIGBREAK = 21;    /* Ctrl-Break sequence */
	private const int SIGABRT = 22;     /* abnormal termination triggered by abort call */

	private static Dictionary<int, long> handler = new Dictionary<int, long>();

	// Delegate type to be used as the Handler Routine for SetConsoleCtrlHandler
	private delegate Boolean ConsoleCtrlDelegate(CtrlTypes CtrlType);

	// Enumerated type for the control messages sent to the handler routine
	private enum CtrlTypes : uint
	{
		CTRL_C_EVENT = 0,
		CTRL_BREAK_EVENT,
		CTRL_CLOSE_EVENT,
		CTRL_LOGOFF_EVENT = 5,
		CTRL_SHUTDOWN_EVENT
	}

	[SecurityCritical]
	private sealed class CriticalCtrlHandler : CriticalFinalizerObject
	{
		private ConsoleCtrlDelegate consoleCtrlDelegate;
		private bool ok;

		[DllImport("kernel32.dll")]
		private static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate e, bool add);

		internal CriticalCtrlHandler()
		{
			consoleCtrlDelegate = new ConsoleCtrlDelegate(ConsoleCtrlCheck);
			ok = SetConsoleCtrlHandler(consoleCtrlDelegate, true);
		}

		[SecuritySafeCritical]
		~CriticalCtrlHandler()
		{
			if (ok)
			{
				SetConsoleCtrlHandler(consoleCtrlDelegate, false);
			}
		}
	}

	private static object defaultConsoleCtrlDelegate;

	private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
	{
#if !FIRST_PASS
		switch (ctrlType)
		{
			case CtrlTypes.CTRL_BREAK_EVENT:
				DumpAllJavaThreads();
				return true;

		}
#endif
		return false;
	}

#if !FIRST_PASS
	private static void DumpAllJavaThreads()
	{
		Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
		java.util.Map traces = java.lang.Thread.getAllStackTraces();
		Console.WriteLine("Full thread dump IKVM.NET {0} ({1} bit):", JVM.SafeGetAssemblyVersion(Assembly.GetExecutingAssembly()), IntPtr.Size * 8);
		java.util.Iterator entries = traces.entrySet().iterator();
		while (entries.hasNext())
		{
			java.util.Map.Entry entry = (java.util.Map.Entry)entries.next();
			java.lang.Thread thread = (java.lang.Thread)entry.getKey();
			Console.WriteLine("\n\"{0}\"{1} prio={2} tid=0x{3:X8}", thread.getName(), thread.isDaemon() ? " daemon" : "", thread.getPriority(), thread.getId());
			Console.WriteLine("   java.lang.Thread.State: " + thread.getState());
			java.lang.StackTraceElement[] trace = (java.lang.StackTraceElement[])entry.getValue();
			for (int i = 0; i < trace.Length; i++)
			{
				Console.WriteLine("\tat {0}", trace[i]);
			}
		}
		Console.WriteLine();
	}
#endif

	public static int findSignal(string sigName)
	{
		if (Environment.OSVersion.Platform == PlatformID.Win32NT)
		{
			switch (sigName)
			{
				case "ABRT": /* abnormal termination triggered by abort cl */
					return SIGABRT;
				case "FPE": /* floating point exception */
					return SIGFPE;
				case "SEGV": /* segment violation */
					return SIGSEGV;
				case "INT": /* interrupt */
					return SIGINT;
				case "TERM": /* software term signal from kill */
					return SIGTERM;
				case "BREAK": /* Ctrl-Break sequence */
					return SIGBREAK;
				case "ILL": /* illegal instruction */
					return SIGILL;
			}
		}
		return -1;
	}

	// this is a separate method to be able to catch the SecurityException (for the LinkDemand)
	[SecuritySafeCritical]
	private static void RegisterCriticalCtrlHandler()
	{
		defaultConsoleCtrlDelegate = new CriticalCtrlHandler();
	}

	// Register a signal handler
	public static long handle0(int sig, long nativeH)
	{
		long oldHandler;
		handler.TryGetValue(sig, out oldHandler);
		switch (nativeH)
		{
			case 0: // Default Signal Handler
				if (defaultConsoleCtrlDelegate == null && Environment.OSVersion.Platform == PlatformID.Win32NT)
				{
					try
					{
						RegisterCriticalCtrlHandler();
					}
					catch (SecurityException)
					{
					}
				}
				break;
			case 1: // Ignore Signal
				break;
			case 2: // Custom Signal Handler
				switch (sig)
				{
					case SIGBREAK:
					case SIGFPE:
						return -1;
				}
				break;
		}
		handler[sig] = nativeH;
		return oldHandler;
	}

	public static void raise0(int sig)
	{
#if !FIRST_PASS
		java.security.AccessController.doPrivileged(ikvm.runtime.Delegates.toPrivilegedAction(delegate
		{
			java.lang.Class clazz = typeof(sun.misc.Signal);
			java.lang.reflect.Method dispatch = clazz.getDeclaredMethod("dispatch", java.lang.Integer.TYPE);
			dispatch.setAccessible(true);
			dispatch.invoke(null, java.lang.Integer.valueOf(sig));
			return null;
		}));
#endif
	}
}

static class Java_sun_misc_NativeSignalHandler
{
	public static void handle0(int number, long handler)
	{
		throw new NotImplementedException();
	}
}

static class Java_sun_misc_Perf
{
	public static object attach(object thisPerf, string user, int lvmid, int mode)
	{
		throw new NotImplementedException();
	}

	public static void detach(object thisPerf, object bb)
	{
		throw new NotImplementedException();
	}

	public static object createLong(object thisPerf, string name, int variability, int units, long value)
	{
#if FIRST_PASS
		return null;
#else
		return java.nio.ByteBuffer.allocate(8);
#endif
	}

	public static object createByteArray(object thisPerf, string name, int variability, int units, byte[] value, int maxLength)
	{
#if FIRST_PASS
		return null;
#else
		return java.nio.ByteBuffer.allocate(maxLength).put(value);
#endif
	}

	public static long highResCounter(object thisPerf)
	{
		throw new NotImplementedException();
	}

	public static long highResFrequency(object thisPerf)
	{
		throw new NotImplementedException();
	}

	public static void registerNatives()
	{
	}
}

static class Java_sun_misc_Unsafe
{
	public static void throwException(object thisUnsafe, Exception x)
	{
		throw x;
	}

	public static void ensureClassInitialized(object thisUnsafe, java.lang.Class clazz)
	{
		TypeWrapper tw = TypeWrapper.FromClass(clazz);
		if (!tw.IsArray)
		{
			try
			{
				tw.Finish();
			}
			catch (RetargetableJavaException x)
			{
				throw x.ToJava();
			}
			tw.RunClassInit();
		}
	}

	[SecurityCritical]
	public static object allocateInstance(object thisUnsafe, java.lang.Class clazz)
	{
		TypeWrapper wrapper = TypeWrapper.FromClass(clazz);
		try
		{
			wrapper.Finish();
		}
		catch (RetargetableJavaException x)
		{
			throw x.ToJava();
		}
		return FormatterServices.GetUninitializedObject(wrapper.TypeAsBaseType);
	}

	public static java.lang.Class defineClass(object thisUnsafe, string name, byte[] buf, int offset, int length, java.lang.ClassLoader cl, java.security.ProtectionDomain pd)
	{
#if FIRST_PASS
		return null;
#else
		return cl.defineClass(name, buf, offset, length, pd);
#endif
	}
}

static class Java_sun_misc_Version
{
	public static string getJvmSpecialVersion()
	{
		throw new NotImplementedException();
	}

	public static string getJdkSpecialVersion()
	{
		throw new NotImplementedException();
	}

	public static bool getJvmVersionInfo()
	{
		throw new NotImplementedException();
	}

	public static void getJdkVersionInfo()
	{
		throw new NotImplementedException();
	}
}

static class Java_sun_misc_VM
{
	public static void initialize()
	{
	}

	public static java.lang.ClassLoader latestUserDefinedLoader()
	{
		// testing shows that it is cheaper the get the full stack trace and then look at a few frames than getting the frames individually
		StackTrace trace = new StackTrace(2, false);
		for (int i = 0; i < trace.FrameCount; i++)
		{
			StackFrame frame = trace.GetFrame(i);
			MethodBase method = frame.GetMethod();
			if (method == null)
			{
				continue;
			}
			Type type = method.DeclaringType;
			if (type != null)
			{
				TypeWrapper tw = ClassLoaderWrapper.GetWrapperFromType(type);
				if (tw != null)
				{
					ClassLoaderWrapper classLoader = tw.GetClassLoader();
					AssemblyClassLoader acl = classLoader as AssemblyClassLoader;
					if (acl == null || acl.GetAssembly(tw) != typeof(object).Assembly)
					{
						java.lang.ClassLoader javaClassLoader = classLoader.GetJavaClassLoader();
						if (javaClassLoader != null)
						{
							return javaClassLoader;
						}
					}
				}
			}
		}
		return null;
	}
}

static class Java_sun_misc_VMSupport
{
	public static object initAgentProperties(object props)
	{
		return props;
	}
}
