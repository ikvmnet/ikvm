/*
  Copyright (C) 2007-2013 Jeroen Frijters
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
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using StackFrame = System.Diagnostics.StackFrame;
using StackTrace = System.Diagnostics.StackTrace;
using SystemArray = System.Array;
using SystemDouble = System.Double;
using SystemTimeZone = System.TimeZone;
using SystemThreadingThread = System.Threading.Thread;
using SystemThreadingThreadInterruptedException = System.Threading.ThreadInterruptedException;
using SystemThreadingThreadPriority = System.Threading.ThreadPriority;
using IKVM.Internal;
using jiFile = java.io.File;
using jiObjectStreamField = java.io.ObjectStreamField;
using jlClass = java.lang.Class;
using jlClassLoader = java.lang.ClassLoader;
using jlrConstructor = java.lang.reflect.Constructor;
using jlStackTraceElement = java.lang.StackTraceElement;
using jnByteBuffer = java.nio.ByteBuffer;
using ProtectionDomain = java.security.ProtectionDomain;
#if !FIRST_PASS
using jlArrayIndexOutOfBoundsException = java.lang.ArrayIndexOutOfBoundsException;
using jlClassNotFoundException = java.lang.ClassNotFoundException;
using jlException = java.lang.Exception;
using jlIllegalAccessException = java.lang.IllegalAccessException;
using jlIllegalArgumentException = java.lang.IllegalArgumentException;
using jlInterruptedException = java.lang.InterruptedException;
using jlInternalError = java.lang.InternalError;
using jlNegativeArraySizeException = java.lang.NegativeArraySizeException;
using jlNoClassDefFoundError = java.lang.NoClassDefFoundError;
using jlNullPointerException = java.lang.NullPointerException;
using jlRunnable = java.lang.Runnable;
using jlRuntimeException = java.lang.RuntimeException;
using jlSecurityManager = java.lang.SecurityManager;
using jlSystem = java.lang.System;
using jlThread = java.lang.Thread;
using jlThreadDeath = java.lang.ThreadDeath;
using jlThreadGroup = java.lang.ThreadGroup;
using jlRuntimePermission = java.lang.RuntimePermission;
using jlBoolean = java.lang.Boolean;
using jlByte = java.lang.Byte;
using jlShort = java.lang.Short;
using jlCharacter = java.lang.Character;
using jlInteger = java.lang.Integer;
using jlFloat = java.lang.Float;
using jlLong = java.lang.Long;
using jlDouble = java.lang.Double;
using jlVoid = java.lang.Void;
using jlNumber = java.lang.Number;
using jlrMethod = java.lang.reflect.Method;
using jlrField = java.lang.reflect.Field;
using jlrModifier = java.lang.reflect.Modifier;
using jlrAccessibleObject = java.lang.reflect.AccessibleObject;
using jlrInvocationTargetException = java.lang.reflect.InvocationTargetException;
using srMethodAccessor = sun.reflect.MethodAccessor;
using srConstructorAccessor = sun.reflect.ConstructorAccessor;
using srFieldAccessor = sun.reflect.FieldAccessor;
using srLangReflectAccess = sun.reflect.LangReflectAccess;
using srReflection = sun.reflect.Reflection;
using srReflectionFactory = sun.reflect.ReflectionFactory;
using Annotation = java.lang.annotation.Annotation;
using smJavaIOAccess = sun.misc.JavaIOAccess;
using smLauncher = sun.misc.Launcher;
using smSharedSecrets = sun.misc.SharedSecrets;
using smVM = sun.misc.VM;
using jiConsole = java.io.Console;
using jiIOException = java.io.IOException;
using jnCharset = java.nio.charset.Charset;
using juProperties = java.util.Properties;
using irUtil = ikvm.runtime.Util;
using iiFieldReflectorBase = ikvm.@internal.FieldReflectorBase;
using juzZipFile = java.util.zip.ZipFile;
using juzZipEntry = java.util.zip.ZipEntry;
using juEnumeration = java.util.Enumeration;
using jiInputStream = java.io.InputStream;
using jsAccessController = java.security.AccessController;
using jsAccessControlContext = java.security.AccessControlContext;
using jsPrivilegedAction = java.security.PrivilegedAction;
using jsPrivilegedExceptionAction = java.security.PrivilegedExceptionAction;
using jsPrivilegedActionException = java.security.PrivilegedActionException;
using jnUnknownHostException = java.net.UnknownHostException;
using jnInetAddress = java.net.InetAddress;
using jnInet4Address = java.net.Inet4Address;
using jnInet6Address = java.net.Inet6Address;
using jnNetworkInterface = java.net.NetworkInterface;
using jnInterfaceAddress = java.net.InterfaceAddress;
using ssaGetPropertyAction = sun.security.action.GetPropertyAction;
#endif

namespace IKVM.Runtime
{
	public static class Assertions
	{
		private static bool sysAsserts;
		private static bool userAsserts;
		private static OptionNode classes;
		private static OptionNode packages;

		private sealed class OptionNode
		{
			internal readonly string name;
			internal readonly bool enabled;
			internal readonly OptionNode next;

			internal OptionNode(string name, bool enabled, OptionNode next)
			{
				this.name = name;
				this.enabled = enabled;
				this.next = next;
			}
		}

		private static void AddOption(string classOrPackage, bool enabled)
		{
			if (classOrPackage == null)
			{
				throw new ArgumentNullException("classOrPackage");
			}

			if (classOrPackage.EndsWith("..."))
			{
				packages = new OptionNode(classOrPackage.Substring(0, classOrPackage.Length - 3), enabled, packages);
			}
			else
			{
				classes = new OptionNode(classOrPackage, enabled, classes);
			}
		}

		public static void EnableAssertions(string classOrPackage)
		{
			AddOption(classOrPackage, true);
		}

		public static void DisableAssertions(string classOrPackage)
		{
			AddOption(classOrPackage, false);
		}

		public static void EnableAssertions()
		{
			userAsserts = true;
		}

		public static void DisableAssertions()
		{
			userAsserts = false;
		}

		public static void EnableSystemAssertions()
		{
			sysAsserts = true;
		}

		public static void DisableSystemAssertions()
		{
			sysAsserts = false;
		}

		internal static bool IsEnabled(TypeWrapper tw)
		{
			string className = tw.Name;

			// match class name
			for (OptionNode n = classes; n != null; n = n.next)
			{
				if (n.name == className)
				{
					return n.enabled;
				}
			}

			// match package name
			if (packages != null)
			{
				int len = className.Length;
				while (len > 0 && className[--len] != '.') ;

				do
				{
					for (OptionNode n = packages; n != null; n = n.next)
					{
						if (String.Compare(n.name, 0, className, 0, len, false, System.Globalization.CultureInfo.InvariantCulture) == 0 && len == n.name.Length)
						{
							return n.enabled;
						}
					}
					while (len > 0 && className[--len] != '.') ;
				} while (len > 0);
			}

			return tw.GetClassLoader() == ClassLoaderWrapper.GetBootstrapClassLoader() ? sysAsserts : userAsserts;
		}

		private static int Count(OptionNode n)
		{
			int count = 0;
			while (n != null)
			{
				count++;
				n = n.next;
			}
			return count;
		}

		internal static object RetrieveDirectives()
		{
#if FIRST_PASS
			return null;
#else

			java.lang.AssertionStatusDirectives asd = new java.lang.AssertionStatusDirectives();
			string[] arrStrings = new string[Count(classes)];
			bool[] arrBools = new bool[arrStrings.Length];
			OptionNode n = classes;
			for (int i = 0; i < arrStrings.Length; i++)
			{
				arrStrings[i] = n.name;
				arrBools[i] = n.enabled;
				n = n.next;
			}
			asd.classes = arrStrings;
			asd.classEnabled = arrBools;
			arrStrings = new string[Count(packages)];
			arrBools = new bool[arrStrings.Length];
			n = packages;
			for (int i = 0; i < arrStrings.Length; i++)
			{
				arrStrings[i] = n.name;
				arrBools[i] = n.enabled;
				n = n.next;
			}
			asd.packages = arrStrings;
			asd.packageEnabled = arrBools;
			asd.deflt = userAsserts;
			return asd;
#endif
		}
	}
}

static class DynamicMethodUtils
{
#if NET_4_0
	private static Module dynamicModule;
#endif

	[System.Security.SecuritySafeCritical]
	internal static DynamicMethod Create(string name, Type owner, bool nonPublic, Type returnType, Type[] paramTypes)
	{
		try
		{
#if NET_4_0
			if (dynamicModule == null)
			{
				// we have to create a module that is security critical to hold the dynamic method, if we want to be able to emit unverifiable code
				AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("<DynamicMethodHolder>"), AssemblyBuilderAccess.RunAndCollect);
				Interlocked.CompareExchange(ref dynamicModule, ab.DefineDynamicModule("<DynamicMethodHolder>"), null);
			}
			return new DynamicMethod(name, MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, returnType, paramTypes, dynamicModule, true);
#else
			if (!ReflectUtil.CanOwnDynamicMethod(owner))
			{
				// interfaces and arrays aren't allowed as owners of dynamic methods
				return new DynamicMethod(name, MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, returnType, paramTypes, owner.Module, true);
			}
			else
			{
				return new DynamicMethod(name, returnType, paramTypes, owner);
			}
#endif
		}
		catch (System.Security.SecurityException)
		{
			if (nonPublic && !RestrictedMemberAccess)
			{
				// we don't have RestrictedMemberAccess, so we stick the dynamic method in our module and hope for the best
				// (i.e. that we're trying to access something with assembly access in an assembly that lets us)
				return new DynamicMethod(name, returnType, paramTypes, typeof(DynamicMethodUtils).Module);
			}
			// apparently we don't have full trust, so we try again with .NET 2.0 SP1 method
			// and we only request restrictSkipVisibility if it is required
			return new DynamicMethod(name, returnType, paramTypes, nonPublic);
		}
	}

	private static bool RestrictedMemberAccess
	{
		get
		{
			try
			{
				new System.Security.Permissions.ReflectionPermission(System.Security.Permissions.ReflectionPermissionFlag.RestrictedMemberAccess).Demand();
				return true;
			}
			catch (System.Security.SecurityException)
			{
				return false;
			}
		}
	}
}

namespace IKVM.NativeCode.ikvm.runtime
{
	static class Startup
	{
		// this method is called from ikvm.runtime.Startup.exitMainThread() and from JNI's DetachCurrentThread
		public static void jniDetach()
		{
#if !FIRST_PASS
			jlThread.currentThread().die();
#endif
		}

		public static void addBootClassPathAssembly(Assembly asm)
		{
			ClassLoaderWrapper.GetBootstrapClassLoader().AddDelegate(global::IKVM.Internal.AssemblyClassLoader.FromAssembly(asm));
		}
	}
}

namespace IKVM.NativeCode.java
{
	namespace lang
	{
		namespace @ref
		{
			static class Reference
			{
				public static bool noclassgc()
				{
#if CLASSGC
					return !JVM.classUnloading;
#else
					return true;
#endif
				}
			}
		}
	}

	namespace net
	{
		static class DatagramPacket
		{
			public static void init()
			{
			}
		}

		static class InetAddress
		{
			public static void init()
			{
			}

#if !FIRST_PASS
			internal static jnInetAddress ConvertIPAddress(System.Net.IPAddress address, string hostname)
			{
				if (address.IsIPv6LinkLocal || address.IsIPv6SiteLocal)
				{
					return jnInet6Address.getByAddress(hostname, address.GetAddressBytes(), (int)address.ScopeId);
				}
				else
				{
					return jnInetAddress.getByAddress(hostname, address.GetAddressBytes());
				}
			}
#endif
		}

		static class InetAddressImplFactory
		{
			private static readonly bool ipv6supported = Init();

			private static bool Init()
			{
				string env = IKVM.Internal.JVM.SafeGetEnvironmentVariable("IKVM_IPV6");
				int val;
				if (env != null && Int32.TryParse(env, out val))
				{
					return (val & 1) != 0;
				}
				// On Linux we can't bind both an IPv4 and IPv6 to the same port, so we have to disable IPv6 until we have a dual-stack implementation.
				// Mono on Windows doesn't appear to support IPv6 either (Mono on Linux does).
				return Type.GetType("Mono.Runtime") == null
					&& Environment.OSVersion.Platform == PlatformID.Win32NT
					&& System.Net.Sockets.Socket.OSSupportsIPv6;
			}

			public static bool isIPv6Supported()
			{
				return ipv6supported;
			}
		}

		static class Inet4Address
		{
			public static void init()
			{
			}
		}

		static class Inet4AddressImpl
		{
			public static string getLocalHostName(object thisInet4AddressImpl)
			{
#if FIRST_PASS
				return null;
#else
				try
				{
					return System.Net.Dns.GetHostName();
				}
				catch (System.Net.Sockets.SocketException)
				{
				}
				catch (System.Security.SecurityException)
				{
				}
				return "localhost";
#endif
			}

			public static object lookupAllHostAddr(object thisInet4AddressImpl, string hostname)
			{
#if FIRST_PASS
				return null;
#else
				try
				{
					System.Net.IPAddress[] addr = System.Net.Dns.GetHostAddresses(hostname);
					List<jnInetAddress> addresses = new List<jnInetAddress>();
					for (int i = 0; i < addr.Length; i++)
					{
						byte[] b = addr[i].GetAddressBytes();
						if (b.Length == 4)
						{
							addresses.Add(jnInetAddress.getByAddress(hostname, b));
						}
					}
					if (addresses.Count == 0)
					{
						throw new jnUnknownHostException(hostname);
					}
					return addresses.ToArray();
				}
				catch (System.ArgumentException x)
				{
					throw new jnUnknownHostException(x.Message);
				}
				catch (System.Net.Sockets.SocketException x)
				{
					throw new jnUnknownHostException(x.Message);
				}
#endif
			}

			public static string getHostByAddr(object thisInet4AddressImpl, byte[] addr)
			{
#if FIRST_PASS
				return null;
#else
				try
				{
					return System.Net.Dns.GetHostEntry(new System.Net.IPAddress(addr)).HostName;
				}
				catch (System.ArgumentException x)
				{
					throw new jnUnknownHostException(x.Message);
				}
				catch (System.Net.Sockets.SocketException x)
				{
					throw new jnUnknownHostException(x.Message);
				}
#endif
			}

			public static bool isReachable0(object thisInet4AddressImpl, byte[] addr, int timeout, byte[] ifaddr, int ttl)
			{
				// like the JDK, we don't use Ping, but we try a TCP connection to the echo port
				// (.NET 2.0 has a System.Net.NetworkInformation.Ping class, but that doesn't provide the option of binding to a specific interface)
				try
				{
					using (System.Net.Sockets.Socket sock = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp))
					{
						if (ifaddr != null)
						{
							sock.Bind(new System.Net.IPEndPoint(((ifaddr[3] << 24) + (ifaddr[2] << 16) + (ifaddr[1] << 8) + ifaddr[0]) & 0xFFFFFFFFL, 0));
						}
						if (ttl > 0)
						{
							sock.SetSocketOption(System.Net.Sockets.SocketOptionLevel.IP, System.Net.Sockets.SocketOptionName.IpTimeToLive, ttl);
						}
						System.Net.IPEndPoint ep = new System.Net.IPEndPoint(((addr[3] << 24) + (addr[2] << 16) + (addr[1] << 8) + addr[0]) & 0xFFFFFFFFL, 7);
						IAsyncResult res = sock.BeginConnect(ep, null, null);
						if (res.AsyncWaitHandle.WaitOne(timeout, false))
						{
							try
							{
								sock.EndConnect(res);
								return true;
							}
							catch (System.Net.Sockets.SocketException x)
							{
								const int WSAECONNREFUSED = 10061;
								if (x.ErrorCode == WSAECONNREFUSED)
								{
									// we got back an explicit "connection refused", that means the host was reachable.
									return true;
								}
							}
						}
					}
				}
				catch (System.Net.Sockets.SocketException)
				{
				}
				return false;
			}
		}

		static class Inet6Address
		{
			public static void init()
			{
			}
		}

		static class Inet6AddressImpl
		{
			public static string getLocalHostName(object thisInet6AddressImpl)
			{
#if FIRST_PASS
				return null;
#else
				try
				{
					return System.Net.Dns.GetHostName();
				}
				catch (System.Net.Sockets.SocketException x)
				{
					throw new jnUnknownHostException(x.Message);
				}
#endif
			}

			public static object lookupAllHostAddr(object thisInet6AddressImpl, string hostname)
			{
#if FIRST_PASS
				return null;
#else
				try
				{
					System.Net.IPAddress[] addr = System.Net.Dns.GetHostAddresses(hostname);
					jnInetAddress[] addresses = new jnInetAddress[addr.Length];
					int pos = 0;
					for (int i = 0; i < addr.Length; i++)
					{
						if (addr[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 == jnInetAddress.preferIPv6Address)
						{
							addresses[pos++] = InetAddress.ConvertIPAddress(addr[i], hostname);
						}
					}
					for (int i = 0; i < addr.Length; i++)
					{
						if (addr[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 != jnInetAddress.preferIPv6Address)
						{
							addresses[pos++] = InetAddress.ConvertIPAddress(addr[i], hostname);
						}
					}
					if (addresses.Length == 0)
					{
						throw new jnUnknownHostException(hostname);
					}
					return addresses;
				}
				catch (System.ArgumentException x)
				{
					throw new jnUnknownHostException(x.Message);
				}
				catch (System.Net.Sockets.SocketException x)
				{
					throw new jnUnknownHostException(x.Message);
				}
#endif
			}

			public static string getHostByAddr(object thisInet6AddressImpl, byte[] addr)
			{
#if FIRST_PASS
				return null;
#else
				try
				{
					return System.Net.Dns.GetHostEntry(new System.Net.IPAddress(addr)).HostName;
				}
				catch (System.ArgumentException x)
				{
					throw new jnUnknownHostException(x.Message);
				}
				catch (System.Net.Sockets.SocketException x)
				{
					throw new jnUnknownHostException(x.Message);
				}
#endif
			}

			public static bool isReachable0(object thisInet6AddressImpl, byte[] addr, int scope, int timeout, byte[] inf, int ttl, int if_scope)
			{
				if (addr.Length == 4)
				{
					return Inet4AddressImpl.isReachable0(null, addr, timeout, inf, ttl);
				}
				// like the JDK, we don't use Ping, but we try a TCP connection to the echo port
				// (.NET 2.0 has a System.Net.NetworkInformation.Ping class, but that doesn't provide the option of binding to a specific interface)
				try
				{
					using (System.Net.Sockets.Socket sock = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetworkV6, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp))
					{
						if (inf != null)
						{
							sock.Bind(new System.Net.IPEndPoint(new System.Net.IPAddress(inf, (uint)if_scope), 0));
						}
						if (ttl > 0)
						{
							sock.SetSocketOption(System.Net.Sockets.SocketOptionLevel.IPv6, System.Net.Sockets.SocketOptionName.HopLimit, ttl);
						}
						System.Net.IPEndPoint ep = new System.Net.IPEndPoint(new System.Net.IPAddress(addr, (uint)scope), 7);
						IAsyncResult res = sock.BeginConnect(ep, null, null);
						if (res.AsyncWaitHandle.WaitOne(timeout, false))
						{
							try
							{
								sock.EndConnect(res);
								return true;
							}
							catch (System.Net.Sockets.SocketException x)
							{
								const int WSAECONNREFUSED = 10061;
								if (x.ErrorCode == WSAECONNREFUSED)
								{
									// we got back an explicit "connection refused", that means the host was reachable.
									return true;
								}
							}
						}
					}
				}
				catch (System.ArgumentException)
				{
				}
				catch (System.Net.Sockets.SocketException)
				{
				}
				return false;
			}
		}

		static class NetworkInterface
		{
#if !FIRST_PASS
			private static NetworkInterfaceInfo cache;
			private static DateTime cachedSince;
#endif

			public static void init()
			{
			}

#if !FIRST_PASS
			private sealed class NetworkInterfaceInfo
			{
				internal System.Net.NetworkInformation.NetworkInterface[] dotnetInterfaces;
				internal jnNetworkInterface[] javaInterfaces;
			}

			private static int Compare(System.Net.NetworkInformation.NetworkInterface ni1, System.Net.NetworkInformation.NetworkInterface ni2)
			{
				int index1 = GetIndex(ni1);
				int index2 = GetIndex(ni2);
				return index1.CompareTo(index2);
			}

			private static System.Net.NetworkInformation.IPv4InterfaceProperties GetIPv4Properties(System.Net.NetworkInformation.IPInterfaceProperties props)
			{
				try
				{
					return props.GetIPv4Properties();
				}
				catch (System.Net.NetworkInformation.NetworkInformationException)
				{
					return null;
				}
			}

			private static System.Net.NetworkInformation.IPv6InterfaceProperties GetIPv6Properties(System.Net.NetworkInformation.IPInterfaceProperties props)
			{
				try
				{
					return props.GetIPv6Properties();
				}
				catch (System.Net.NetworkInformation.NetworkInformationException)
				{
					return null;
				}
			}

			private static int GetIndex(System.Net.NetworkInformation.NetworkInterface ni)
			{
				System.Net.NetworkInformation.IPInterfaceProperties ipprops = ni.GetIPProperties();
				System.Net.NetworkInformation.IPv4InterfaceProperties ipv4props = GetIPv4Properties(ipprops);
				if (ipv4props != null)
				{
					return ipv4props.Index;
				}
				else if (InetAddressImplFactory.isIPv6Supported())
				{
					System.Net.NetworkInformation.IPv6InterfaceProperties ipv6props = GetIPv6Properties(ipprops);
					if (ipv6props != null)
					{
						return ipv6props.Index;
					}
				}
				return -1;
			}

			private static bool IsValid(System.Net.NetworkInformation.NetworkInterface ni)
			{
				return GetIndex(ni) != -1;
			}

			private static NetworkInterfaceInfo GetInterfaces()
			{
				// Since many of the methods in java.net.NetworkInterface end up calling this method and the underlying stuff this is
				// based on isn't very quick either, we cache the array for a couple of seconds.
				if (cache != null && DateTime.UtcNow - cachedSince < new TimeSpan(0, 0, 5))
				{
					return cache;
				}
				System.Net.NetworkInformation.NetworkInterface[] ifaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
				// on Mono (on Windows) we need to filter out the network interfaces that don't have any IP properties
				ifaces = Array.FindAll(ifaces, IsValid);
				Array.Sort(ifaces, Compare);
				jnNetworkInterface[] ret = new jnNetworkInterface[ifaces.Length];
				int eth = 0;
				int tr = 0;
				int fddi = 0;
				int lo = 0;
				int ppp = 0;
				int sl = 0;
				int net = 0;
				for (int i = 0; i < ifaces.Length; i++)
				{
					string name;
					switch (ifaces[i].NetworkInterfaceType)
					{
						case System.Net.NetworkInformation.NetworkInterfaceType.Ethernet:
							name = "eth" + eth++;
							break;
						case System.Net.NetworkInformation.NetworkInterfaceType.TokenRing:
							name = "tr" + tr++;
							break;
						case System.Net.NetworkInformation.NetworkInterfaceType.Fddi:
							name = "fddi" + fddi++;
							break;
						case System.Net.NetworkInformation.NetworkInterfaceType.Loopback:
							if (lo > 0)
							{
								continue;
							}
							name = "lo";
							lo++;
							break;
						case System.Net.NetworkInformation.NetworkInterfaceType.Ppp:
							name = "ppp" + ppp++;
							break;
						case System.Net.NetworkInformation.NetworkInterfaceType.Slip:
							name = "sl" + sl++;
							break;
						default:
							name = "net" + net++;
							break;
					}
					jnNetworkInterface netif = new jnNetworkInterface();
					ret[i] = netif;
					netif._set1(name, ifaces[i].Description, GetIndex(ifaces[i]));
					System.Net.NetworkInformation.UnicastIPAddressInformationCollection uipaic = ifaces[i].GetIPProperties().UnicastAddresses;
					List<jnInetAddress> addresses = new List<jnInetAddress>();
					List<jnInterfaceAddress> bindings = new List<jnInterfaceAddress>();
					for (int j = 0; j < uipaic.Count; j++)
					{
						System.Net.IPAddress addr = uipaic[j].Address;
						if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
						{
							jnInet4Address address = new jnInet4Address(null, addr.GetAddressBytes());
							jnInterfaceAddress binding = new jnInterfaceAddress();
							short mask = 32;
							jnInet4Address broadcast = null;
							System.Net.IPAddress v4mask;
							try
							{
								v4mask = uipaic[j].IPv4Mask;
							}
							catch (NotImplementedException)
							{
								// Mono (as of 2.6.7) doesn't implement the IPv4Mask property
								v4mask = null;
							}
							if (v4mask != null && !v4mask.Equals(System.Net.IPAddress.Any))
							{
								broadcast = new jnInet4Address(null, -1);
								mask = 0;
								foreach (byte b in v4mask.GetAddressBytes())
								{
									mask += (short)global::java.lang.Integer.bitCount(b);
								}
							}
							else if ((address.address & ~0xffffff) == 0x7f000000)
							{
								mask = 8;
								broadcast = new jnInet4Address(null, 0xffffff);
							}
							binding._set(address, broadcast, mask);
							addresses.Add(address);
							bindings.Add(binding);
						}
						else if (InetAddressImplFactory.isIPv6Supported())
						{
							int scope = 0;
							if (addr.IsIPv6LinkLocal || addr.IsIPv6SiteLocal)
							{
								scope = (int)addr.ScopeId;
							}
							jnInet6Address ia6 = new jnInet6Address();
							ia6.ipaddress = addr.GetAddressBytes();
							if (scope != 0)
							{
								ia6._set(scope, netif);
							}
							jnInterfaceAddress binding = new jnInterfaceAddress();
							// TODO where do we get the IPv6 subnet prefix length?
							short mask = 128;
							binding._set(ia6, null, mask);
							addresses.Add(ia6);
							bindings.Add(binding);
						}
					}
					netif._set2(addresses.ToArray(), bindings.ToArray(), new jnNetworkInterface[0]);
				}
				NetworkInterfaceInfo nii = new NetworkInterfaceInfo();
				nii.dotnetInterfaces = ifaces;
				nii.javaInterfaces = ret;
				cache = nii;
				cachedSince = DateTime.UtcNow;
				return nii;
			}
#endif

			private static System.Net.NetworkInformation.NetworkInterface GetDotNetNetworkInterfaceByIndex(int index)
			{
#if FIRST_PASS
				return null;
#else
				NetworkInterfaceInfo nii = GetInterfaces();
				for (int i = 0; i < nii.javaInterfaces.Length; i++)
				{
					if (nii.javaInterfaces[i].getIndex() == index)
					{
						return nii.dotnetInterfaces[i];
					}
				}
				throw new global::java.net.SocketException("interface index not found");
#endif
			}

			public static object getByIndex(int index)
			{
#if FIRST_PASS
				return null;
#else
				foreach (jnNetworkInterface iface in GetInterfaces().javaInterfaces)
				{
					if (iface.getIndex() == index)
					{
						return iface;
					}
				}
				return null;
#endif
			}

			public static object getAll()
			{
#if FIRST_PASS
				return null;
#else
				return GetInterfaces().javaInterfaces;
#endif
			}

			public static object getByName0(string name)
			{
#if FIRST_PASS
				return null;
#else
				foreach (jnNetworkInterface iface in GetInterfaces().javaInterfaces)
				{
					if (iface.getName() == name)
					{
						return iface;
					}
				}
				return null;
#endif
			}

			public static object getByIndex0(int index)
			{
#if FIRST_PASS
				return null;
#else
				foreach (jnNetworkInterface iface in GetInterfaces().javaInterfaces)
				{
					if (iface.getIndex() == index)
					{
						return iface;
					}
				}
				return null;
#endif
			}

			public static object getByInetAddress0(object addr)
			{
#if FIRST_PASS
				return null;
#else
				foreach (jnNetworkInterface iface in GetInterfaces().javaInterfaces)
				{
					juEnumeration addresses = iface.getInetAddresses();
					while (addresses.hasMoreElements())
					{
						if (addresses.nextElement().Equals(addr))
						{
							return iface;
						}
					}
				}
				return null;
#endif
			}

			public static long getSubnet0(string name, int ind)
			{
				// this method is not used by the java code (!)
				return 0;
			}

			public static object getBroadcast0(string name, int ind)
			{
				// this method is not used by the java code (!)
				return null;
			}

			public static bool isUp0(string name, int ind)
			{
#if FIRST_PASS
				return false;
#else
				return GetDotNetNetworkInterfaceByIndex(ind).OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up;
#endif
			}

			public static bool isLoopback0(string name, int ind)
			{
#if FIRST_PASS
				return false;
#else
				return GetDotNetNetworkInterfaceByIndex(ind).NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Loopback;
#endif
			}

			public static bool supportsMulticast0(string name, int ind)
			{
#if FIRST_PASS
				return false;
#else
				return GetDotNetNetworkInterfaceByIndex(ind).SupportsMulticast;
#endif
			}

			public static bool isP2P0(string name, int ind)
			{
#if FIRST_PASS
				return false;
#else
				switch (GetDotNetNetworkInterfaceByIndex(ind).NetworkInterfaceType)
				{
					case System.Net.NetworkInformation.NetworkInterfaceType.Ppp:
					case System.Net.NetworkInformation.NetworkInterfaceType.Slip:
						return true;
					default:
						return false;
				}
#endif
			}

			public static byte[] getMacAddr0(byte[] inAddr, string name, int ind)
			{
#if FIRST_PASS
				return null;
#else
				return GetDotNetNetworkInterfaceByIndex(ind).GetPhysicalAddress().GetAddressBytes();
#endif
			}

			public static int getMTU0(string name, int ind)
			{
#if FIRST_PASS
				return 0;
#else
				System.Net.NetworkInformation.IPInterfaceProperties ipprops = GetDotNetNetworkInterfaceByIndex(ind).GetIPProperties();
				System.Net.NetworkInformation.IPv4InterfaceProperties v4props = GetIPv4Properties(ipprops);
				if (v4props != null)
				{
					return v4props.Mtu;
				}
				if (InetAddressImplFactory.isIPv6Supported())
				{
					System.Net.NetworkInformation.IPv6InterfaceProperties v6props = GetIPv6Properties(ipprops);
					if (v6props != null)
					{
						return v6props.Mtu;
					}
				}
				return -1;
#endif
			}
		}
	}

	namespace nio
	{
		[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, UnmanagedCode = true)]
		[System.Security.SecurityCritical]
		static class Bits
		{
			public static void copyFromByteArray(object src, long srcPos, long dstAddr, long length)
			{
				byte[] byteArray = src as byte[];
				if (byteArray != null)
				{
					System.Runtime.InteropServices.Marshal.Copy(byteArray, (int)srcPos, (IntPtr)dstAddr, (int)length);
					return;
				}
				char[] charArray = src as char[];
				if (charArray != null)
				{
					System.Runtime.InteropServices.Marshal.Copy(charArray, ((int)srcPos) >> 1, (IntPtr)dstAddr, ((int)length) >> 1);
					return;
				}
				short[] shortArray = src as short[];
				if (shortArray != null)
				{
					System.Runtime.InteropServices.Marshal.Copy(shortArray, ((int)srcPos) >> 1, (IntPtr)dstAddr, ((int)length) >> 1);
					return;
				}
				int[] intArray = src as int[];
				if (intArray != null)
				{
					System.Runtime.InteropServices.Marshal.Copy(intArray, ((int)srcPos) >> 2, (IntPtr)dstAddr, ((int)length) >> 2);
					return;
				}
				float[] floatArray = src as float[];
				if (floatArray != null)
				{
					System.Runtime.InteropServices.Marshal.Copy(floatArray, ((int)srcPos) >> 2, (IntPtr)dstAddr, ((int)length) >> 2);
					return;
				}
				long[] longArray = src as long[];
				if (longArray != null)
				{
					System.Runtime.InteropServices.Marshal.Copy(longArray, ((int)srcPos) >> 3, (IntPtr)dstAddr, ((int)length) >> 3);
					return;
				}
				double[] doubleArray = src as double[];
				if (doubleArray != null)
				{
					System.Runtime.InteropServices.Marshal.Copy(doubleArray, ((int)srcPos) >> 3, (IntPtr)dstAddr, ((int)length) >> 3);
					return;
				}
			}

			public static void copyToByteArray(long srcAddr, object dst, long dstPos, long length)
			{
				byte[] byteArray = dst as byte[];
				if (byteArray != null)
				{
					System.Runtime.InteropServices.Marshal.Copy((IntPtr)srcAddr, byteArray, (int)dstPos, (int)length);
					return;
				}
				char[] charArray = dst as char[];
				if (charArray != null)
				{
					System.Runtime.InteropServices.Marshal.Copy((IntPtr)srcAddr, charArray, ((int)dstPos) >> 1, ((int)length) >> 1);
					return;
				}
				short[] shortArray = dst as short[];
				if (shortArray != null)
				{
					System.Runtime.InteropServices.Marshal.Copy((IntPtr)srcAddr, shortArray, ((int)dstPos) >> 1, ((int)length) >> 1);
					return;
				}
				int[] intArray = dst as int[];
				if (intArray != null)
				{
					System.Runtime.InteropServices.Marshal.Copy((IntPtr)srcAddr, intArray, ((int)dstPos) >> 2, ((int)length) >> 2);
					return;
				}
				float[] floatArray = dst as float[];
				if (floatArray != null)
				{
					System.Runtime.InteropServices.Marshal.Copy((IntPtr)srcAddr, floatArray, ((int)dstPos) >> 2, ((int)length) >> 2);
					return;
				}
				long[] longArray = dst as long[];
				if (longArray != null)
				{
					System.Runtime.InteropServices.Marshal.Copy((IntPtr)srcAddr, longArray, ((int)dstPos) >> 3, ((int)length) >> 3);
					return;
				}
				double[] doubleArray = dst as double[];
				if (doubleArray != null)
				{
					System.Runtime.InteropServices.Marshal.Copy((IntPtr)srcAddr, doubleArray, ((int)dstPos) >> 3, ((int)length) >> 3);
					return;
				}
			}

			public static void copyFromShortArray(object src, long srcPos, long dstAddr, long length)
			{
#if !FIRST_PASS
				short[] shortArray = src as short[];
				if (shortArray != null)
				{
					int index = ((int)srcPos) >> 1;
					while (length > 0)
					{
						short v = jlShort.reverseBytes(shortArray[index++]);
						System.Runtime.InteropServices.Marshal.WriteInt16((IntPtr)dstAddr, v);
						dstAddr += 2;
						length -= 2;
					}
				}
				else
				{
					char[] charArray = (char[])src;
					int index = ((int)srcPos) >> 1;
					while (length > 0)
					{
						short v = jlShort.reverseBytes((short)charArray[index++]);
						System.Runtime.InteropServices.Marshal.WriteInt16((IntPtr)dstAddr, v);
						dstAddr += 2;
						length -= 2;
					}
				}
#endif
			}

			public static void copyToShortArray(long srcAddr, object dst, long dstPos, long length)
			{
#if !FIRST_PASS
				short[] shortArray = dst as short[];
				if (shortArray != null)
				{
					int index = ((int)dstPos) >> 1;
					while (length > 0)
					{
						short v = System.Runtime.InteropServices.Marshal.ReadInt16((IntPtr)srcAddr);
						shortArray[index++] = jlShort.reverseBytes(v);
						srcAddr += 2;
						length -= 2;
					}
				}
				else
				{
					char[] charArray = (char[])dst;
					int index = ((int)dstPos) >> 1;
					while (length > 0)
					{
						short v = System.Runtime.InteropServices.Marshal.ReadInt16((IntPtr)srcAddr);
						charArray[index++] = (char)jlShort.reverseBytes(v);
						srcAddr += 2;
						length -= 2;
					}
				}
#endif
			}

			public static void copyFromIntArray(object src, long srcPos, long dstAddr, long length)
			{
#if !FIRST_PASS
				int[] intArray = src as int[];
				if (intArray != null)
				{
					int index = ((int)srcPos) >> 2;
					while (length > 0)
					{
						int v = jlInteger.reverseBytes(intArray[index++]);
						System.Runtime.InteropServices.Marshal.WriteInt32((IntPtr)dstAddr, v);
						dstAddr += 4;
						length -= 4;
					}
				}
				else
				{
					float[] floatArray = (float[])src;
					int index = ((int)srcPos) >> 2;
					while (length > 0)
					{
						int v = jlInteger.reverseBytes(jlFloat.floatToRawIntBits(floatArray[index++]));
						System.Runtime.InteropServices.Marshal.WriteInt32((IntPtr)dstAddr, v);
						dstAddr += 4;
						length -= 4;
					}
				}
#endif
			}

			public static void copyToIntArray(long srcAddr, object dst, long dstPos, long length)
			{
#if !FIRST_PASS
				int[] intArray = dst as int[];
				if (intArray != null)
				{
					int index = ((int)dstPos) >> 2;
					while (length > 0)
					{
						int v = System.Runtime.InteropServices.Marshal.ReadInt32((IntPtr)srcAddr);
						intArray[index++] = jlInteger.reverseBytes(v);
						srcAddr += 4;
						length -= 4;
					}
				}
				else
				{
					float[] floatArray = (float[])dst;
					int index = ((int)dstPos) >> 2;
					while (length > 0)
					{
						int v = System.Runtime.InteropServices.Marshal.ReadInt32((IntPtr)srcAddr);
						floatArray[index++] = jlFloat.intBitsToFloat(jlInteger.reverseBytes(v));
						srcAddr += 4;
						length -= 4;
					}
				}
#endif
			}

			public static void copyFromLongArray(object src, long srcPos, long dstAddr, long length)
			{
#if !FIRST_PASS
				long[] longArray = src as long[];
				if (longArray != null)
				{
					int index = ((int)srcPos) >> 3;
					while (length > 0)
					{
						long v = jlLong.reverseBytes(longArray[index++]);
						System.Runtime.InteropServices.Marshal.WriteInt64((IntPtr)dstAddr, v);
						dstAddr += 8;
						length -= 8;
					}
				}
				else
				{
					double[] doubleArray = (double[])src;
					int index = ((int)srcPos) >> 3;
					while (length > 0)
					{
						long v = jlLong.reverseBytes(BitConverter.DoubleToInt64Bits(doubleArray[index++]));
						System.Runtime.InteropServices.Marshal.WriteInt64((IntPtr)dstAddr, v);
						dstAddr += 8;
						length -= 8;
					}
				}
#endif
			}

			public static void copyToLongArray(long srcAddr, object dst, long dstPos, long length)
			{
#if !FIRST_PASS
				long[] longArray = dst as long[];
				if (longArray != null)
				{
					int index = ((int)dstPos) >> 3;
					while (length > 0)
					{
						long v = System.Runtime.InteropServices.Marshal.ReadInt64((IntPtr)srcAddr);
						longArray[index++] = jlLong.reverseBytes(v);
						srcAddr += 8;
						length -= 8;
					}
				}
				else
				{
					double[] doubleArray = (double[])dst;
					int index = ((int)dstPos) >> 3;
					while (length > 0)
					{
						long v = System.Runtime.InteropServices.Marshal.ReadInt64((IntPtr)srcAddr);
						doubleArray[index++] = BitConverter.Int64BitsToDouble(jlLong.reverseBytes(v));
						srcAddr += 8;
						length -= 8;
					}
				}
#endif
			}
		}

		static class MappedByteBuffer
		{
			private static volatile int bogusField;

			public static bool isLoaded0(object thisMappedByteBuffer, long address, long length, int pageCount)
			{
				// on Windows, JDK simply returns false, so we can get away with that too.
				return false;
			}

			[System.Security.SecuritySafeCritical]
			public static void load0(object thisMappedByteBuffer, long address, long length)
			{
				int bogus = bogusField;
				while (length > 0)
				{
					// touch a byte in every page
					bogus += System.Runtime.InteropServices.Marshal.ReadByte((IntPtr)address);
					length -= 4096;
					address += 4096;
				}
				// do a volatile store of the sum of the bytes to make sure the reads don't get optimized out
				bogusField = bogus;
				GC.KeepAlive(thisMappedByteBuffer);
			}

			[System.Security.SecuritySafeCritical]
			public static void force0(object thisMappedByteBuffer, object fd, long address, long length)
			{
				if (JVM.IsUnix)
				{
					ikvm_msync((IntPtr)address, (int)length);
					GC.KeepAlive(thisMappedByteBuffer);
				}
				else
				{
					// according to the JDK sources, FlushViewOfFile can fail with an ERROR_LOCK_VIOLATION error,
					// so like the JDK, we retry up to three times if that happens.
					for (int i = 0; i < 3; i++)
					{
						if (FlushViewOfFile((IntPtr)address, (IntPtr)length) != 0)
						{
							GC.KeepAlive(thisMappedByteBuffer);
							return;
						}
						const int ERROR_LOCK_VIOLATION = 33;
						if (System.Runtime.InteropServices.Marshal.GetLastWin32Error() != ERROR_LOCK_VIOLATION)
						{
							break;
						}
					}
#if !FIRST_PASS
					throw new jiIOException("Flush failed");
#endif
				}
			}

			[System.Runtime.InteropServices.DllImport("kernel32", SetLastError = true)]
			private static extern int FlushViewOfFile(IntPtr lpBaseAddress, IntPtr dwNumberOfBytesToFlush);

			[System.Runtime.InteropServices.DllImport("ikvm-native")]
		    private static extern int ikvm_msync(IntPtr address, int size);
		}
	}

	namespace security
	{
		static class AccessController
		{
			public static object getStackAccessControlContext(global::java.security.AccessControlContext context, global::ikvm.@internal.CallerID callerID)
			{
#if FIRST_PASS
				return null;
#else
				List<ProtectionDomain> array = new List<ProtectionDomain>();
				bool is_privileged = GetProtectionDomains(array, callerID, new StackTrace(1));
				if (array.Count == 0)
				{
					if (is_privileged && context == null)
					{
						return null;
					}
				}
				return CreateAccessControlContext(array, is_privileged, context);
#endif
			}

#if !FIRST_PASS
			private static bool GetProtectionDomains(List<ProtectionDomain> array, global::ikvm.@internal.CallerID callerID, StackTrace stack)
			{
				ProtectionDomain previous_protection_domain = null;
				for (int i = 0; i < stack.FrameCount; i++)
				{
					bool is_privileged = false;
					ProtectionDomain protection_domain;
					MethodBase method = stack.GetFrame(i).GetMethod();
					if (method.DeclaringType == typeof(global::java.security.AccessController)
						&& method.Name == "doPrivileged")
					{
						is_privileged = true;
						global::java.lang.Class caller = callerID.getCallerClass();
						protection_domain = caller == null ? null : Java_java_lang_Class.getProtectionDomain0(caller);
					}
					else
					{
						protection_domain = GetProtectionDomainFromType(method.DeclaringType);
					}

					if (previous_protection_domain != protection_domain && protection_domain != null)
					{
						previous_protection_domain = protection_domain;
						array.Add(protection_domain);
					}

					if (is_privileged)
					{
						return true;
					}
				}
				return false;
			}

			private static object CreateAccessControlContext(List<ProtectionDomain> context, bool is_privileged, jsAccessControlContext privileged_context)
			{
				jsAccessControlContext acc = new jsAccessControlContext(context == null || context.Count == 0 ? null : context.ToArray(), is_privileged);
				acc._privilegedContext(privileged_context);
				return acc;
			}

			private static ProtectionDomain GetProtectionDomainFromType(Type type)
			{
				if (type == null
					|| type.Assembly == typeof(object).Assembly
					|| type.Assembly == typeof(AccessController).Assembly
					|| type.Assembly == Java_java_lang_SecurityManager.jniAssembly
					|| type.Assembly == typeof(jlThread).Assembly)
				{
					return null;
				}
				TypeWrapper tw = ClassLoaderWrapper.GetWrapperFromType(type);
				if (tw != null)
				{
					return Java_java_lang_Class.getProtectionDomain0(tw.ClassObject);
				}
				return null;
			}
#endif

			public static object getInheritedAccessControlContext()
			{
#if FIRST_PASS
				return null;
#else
				global::java.security.AccessController.LazyContext lc = jlThread.currentThread().lazyInheritedAccessControlContext;
				if (lc == null)
				{
					return null;
				}
				List<ProtectionDomain> list = new List<ProtectionDomain>();
				while (lc != null)
				{
					if (GetProtectionDomains(list, lc.callerID, lc.stackTrace))
					{
						return CreateAccessControlContext(list, true, lc.context);
					}
					lc = lc.parent;
				}
				return CreateAccessControlContext(list, false, null);
#endif
			}
		}
	}

	namespace util
	{
		namespace logging
		{
			static class FileHandler
			{
				public static bool isSetUID()
				{
					// TODO
					return false;
				}
			}
		}

		namespace prefs
		{
			static class FileSystemPreferences
			{
				public static int chmod(string filename, int permission)
				{
					// TODO
					return 0;
				}

				public static int[] lockFile0(string filename, int permission, bool shared)
				{
					// TODO
					return new int[] { 1, 0 };
				}

				public static int unlockFile0(int fd)
				{
					// TODO
					return 0;
				}
			}

			static class WindowsPreferences
			{
				// HACK we currently support only 16 handles at a time
				private static readonly Microsoft.Win32.RegistryKey[] keys = new Microsoft.Win32.RegistryKey[16];

				private static Microsoft.Win32.RegistryKey MapKey(int hKey)
				{
					switch (hKey)
					{
						case unchecked((int)0x80000001):
							return Microsoft.Win32.Registry.CurrentUser;
						case unchecked((int)0x80000002):
							return Microsoft.Win32.Registry.LocalMachine;
						default:
							return keys[hKey - 1];
					}
				}

				private static int AllocHandle(Microsoft.Win32.RegistryKey key)
				{
					lock (keys)
					{
						if (key != null)
						{
							for (int i = 0; i < keys.Length; i++)
							{
								if (keys[i] == null)
								{
									keys[i] = key;
									return i + 1;
								}
							}
						}
						return 0;
					}
				}

				private static string BytesToString(byte[] bytes)
				{
					int len = bytes.Length;
					if (bytes[len - 1] == 0)
					{
						len--;
					}
					return Encoding.ASCII.GetString(bytes, 0, len);
				}

				private static byte[] StringToBytes(string str)
				{
					if (str.Length == 0 || str[str.Length - 1] != 0)
					{
						str += '\u0000';
					}
					return Encoding.ASCII.GetBytes(str);
				}

				public static int[] WindowsRegOpenKey(int hKey, byte[] subKey, int securityMask)
				{
                    // writeable = DELETE == 0x10000 || KEY_SET_VALUE == 2 || KEY_CREATE_SUB_KEY == 4 || KEY_WRITE = 0x20006;
                    // !writeable : KEY_ENUMERATE_SUB_KEYS == 8 || KEY_READ == 0x20019 || KEY_QUERY_VALUE == 1
					bool writable = (securityMask & 0x10006) != 0;
					Microsoft.Win32.RegistryKey resultKey = null;
					int error = 0;
					try
					{
                        Microsoft.Win32.RegistryKey parent = MapKey(hKey);
						// HACK we check if we can write in the system preferences 
						// we want not user registry virtualization for compatibility
						if (writable && parent.Name.StartsWith("HKEY_LOCAL_MACHINE", StringComparison.Ordinal) && UACVirtualization.Enabled)
						{
                            resultKey = parent.OpenSubKey(BytesToString(subKey), false);
                            if (resultKey != null) {
                                // error only if key exists
                                resultKey.Close();
                                error = 5;
                                resultKey = null;
                            }
                        } else
                        {
                            resultKey = parent.OpenSubKey(BytesToString(subKey), writable);
                        }
					}
					catch (System.Security.SecurityException)
					{
						error = 5;
					}
					catch (UnauthorizedAccessException)
					{
						error = 5;
					}
					return new int[] { AllocHandle(resultKey), error };
				}

				public static int WindowsRegCloseKey(int hKey)
				{
					keys[hKey - 1].Close();
					lock (keys)
					{
						keys[hKey - 1] = null;
					}
					return 0;
				}

				public static int[] WindowsRegCreateKeyEx(int hKey, byte[] subKey)
				{
					Microsoft.Win32.RegistryKey resultKey = null;
					int error = 0;
					int disposition = -1;
					try
					{
						Microsoft.Win32.RegistryKey key = MapKey(hKey);
						string name = BytesToString(subKey);
						resultKey = key.OpenSubKey(name);
						disposition = 2;
						if (resultKey == null)
						{
							resultKey = key.CreateSubKey(name);
							disposition = 1;
						}
					}
					catch (System.Security.SecurityException)
					{
						error = 5;
					}
					catch (UnauthorizedAccessException)
					{
						error = 5;
					}
					return new int[] { AllocHandle(resultKey), error, disposition };
				}

				public static int WindowsRegDeleteKey(int hKey, byte[] subKey)
				{
					try
					{
						MapKey(hKey).DeleteSubKey(BytesToString(subKey), false);
						return 0;
					}
					catch (System.Security.SecurityException)
					{
						return 5;
					}
				}

				public static int WindowsRegFlushKey(int hKey)
				{
					MapKey(hKey).Flush();
					return 0;
				}

				public static byte[] WindowsRegQueryValueEx(int hKey, byte[] valueName)
				{
					try
					{
						string value = MapKey(hKey).GetValue(BytesToString(valueName)) as string;
						if (value == null)
						{
							return null;
						}
						return StringToBytes(value);
					}
					catch (System.Security.SecurityException)
					{
						return null;
					}
					catch (UnauthorizedAccessException)
					{
						return null;
					}
				}

				public static int WindowsRegSetValueEx(int hKey, byte[] valueName, byte[] data)
				{
					if (valueName == null || data == null)
					{
						return -1;
					}
					try
					{
						MapKey(hKey).SetValue(BytesToString(valueName), BytesToString(data));
						return 0;
					}
					catch (System.Security.SecurityException)
					{
						return 5;
					}
					catch (UnauthorizedAccessException)
					{
						return 5;
					}
				}

                public static int WindowsRegDeleteValue(int hKey, byte[] valueName)
                {
                    try
                    {
                        MapKey(hKey).DeleteValue(BytesToString(valueName));
                        return 0;
                    }
                    catch (System.ArgumentException)
                    {
                        return 2; //ERROR_FILE_NOT_FOUND
                    }
                    catch (System.Security.SecurityException)
                    {
                        return 5; //ERROR_ACCESS_DENIED
                    }
                    catch (UnauthorizedAccessException)
                    {
                        return 5; //ERROR_ACCESS_DENIED
                    }
                }

				public static int[] WindowsRegQueryInfoKey(int hKey)
				{
					int[] result = new int[5] { -1, -1, -1, -1, -1 };
					try
					{
						Microsoft.Win32.RegistryKey key = MapKey(hKey);
						result[0] = key.SubKeyCount;
						result[1] = 0;
						result[2] = key.ValueCount;
						foreach (string s in key.GetSubKeyNames())
						{
							result[3] = Math.Max(result[3], s.Length);
						}
						foreach (string s in key.GetValueNames())
						{
							result[4] = Math.Max(result[4], s.Length);
						}
					}
					catch (System.Security.SecurityException)
					{
						result[1] = 5;
					}
					catch (UnauthorizedAccessException)
					{
						result[1] = 5;
					}
					return result;
				}

				public static byte[] WindowsRegEnumKeyEx(int hKey, int subKeyIndex, int maxKeyLength)
				{
					try
					{
						return StringToBytes(MapKey(hKey).GetSubKeyNames()[subKeyIndex]);
					}
					catch (System.Security.SecurityException)
					{
						return null;
					}
					catch (UnauthorizedAccessException)
					{
						return null;
					}
				}

				public static byte[] WindowsRegEnumValue(int hKey, int valueIndex, int maxValueNameLength)
				{
					try
					{
						return StringToBytes(MapKey(hKey).GetValueNames()[valueIndex]);
					}
					catch (System.Security.SecurityException)
					{
						return null;
					}
					catch (UnauthorizedAccessException)
					{
						return null;
					}
				}
			}

            internal static class UACVirtualization {
                private enum TOKEN_INFORMATION_CLASS {
                    TokenVirtualizationEnabled = 24
                }

                [DllImport("advapi32.dll", SetLastError = true)]
                private static extern bool GetTokenInformation(
                    IntPtr TokenHandle,
                    TOKEN_INFORMATION_CLASS TokenInformationClass,
                    out uint TokenInformation,
                    uint TokenInformationLength,
                    out uint ReturnLength);

                internal static bool Enabled {
                    [System.Security.SecuritySafeCritical]
                    get {
						OperatingSystem os = Environment.OSVersion;
						if (os.Platform != PlatformID.Win32NT || os.Version.Major < 6) {
							return false;
						}
                        uint enabled, length;
                        GetTokenInformation(System.Security.Principal.WindowsIdentity.GetCurrent().Token, TOKEN_INFORMATION_CLASS.TokenVirtualizationEnabled, out enabled, 4, out length);
                        return enabled != 0;
                    }
                }
            }
        }

		namespace jar
		{
			static class JarFile
			{
				public static string[] getMetaInfEntryNames(object thisJarFile)
				{
#if FIRST_PASS
					return null;
#else
					juzZipFile zf = (juzZipFile)thisJarFile;
					juEnumeration entries = zf.entries();
					List<string> list = null;
					while (entries.hasMoreElements())
					{
						juzZipEntry entry = (juzZipEntry)entries.nextElement();
						if (entry.getName().StartsWith("META-INF/", StringComparison.OrdinalIgnoreCase))
						{
							if (list == null)
							{
								list = new List<string>();
							}
							list.Add(entry.getName());
						}
					}
					return list == null ? null : list.ToArray();
#endif
				}
			}
		}

		namespace zip
		{
			static class ClassStubZipEntry
			{
				public static void expandIkvmClasses(object _zipFile, object _entries)
				{
#if !FIRST_PASS
					juzZipFile zipFile = (juzZipFile)_zipFile;
					global::java.util.LinkedHashMap entries = (global::java.util.LinkedHashMap)_entries;

					try
					{
						string path = zipFile.getName();
						juzZipEntry entry = (juzZipEntry)entries.get(JVM.JarClassList);
						if (entry != null && VirtualFileSystem.IsVirtualFS(path))
						{
							using (VirtualFileSystem.ZipEntryStream stream = new VirtualFileSystem.ZipEntryStream(zipFile, entry))
							{
								entries.remove(entry.name);
								System.IO.BinaryReader br = new System.IO.BinaryReader(stream);
								int count = br.ReadInt32();
								for (int i = 0; i < count; i++)
								{
									global::java.util.zip.ClassStubZipEntry classEntry = new global::java.util.zip.ClassStubZipEntry(path, br.ReadString());
									classEntry.setMethod(global::java.util.zip.ClassStubZipEntry.STORED);
									classEntry.setTime(entry.getTime());
									entries.put(classEntry.name, classEntry);
								}
							}
						}
					}
					catch (global::java.io.IOException)
					{
					}
					catch (System.IO.IOException)
					{
					}
#endif
				}
			}
		}

		static class TimeZone
		{
			private static string GetCurrentTimeZoneID()
			{
#if NET_4_0
				return TimeZoneInfo.Local.Id;
#else
				// we don't want a static dependency on System.Core (to be able to run on .NET 2.0)
				Type typeofTimeZoneInfo = Type.GetType("System.TimeZoneInfo, System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
				if (typeofTimeZoneInfo != null)
				{
					try
					{
						return (string)typeofTimeZoneInfo.GetProperty("Id").GetValue(typeofTimeZoneInfo.GetProperty("Local").GetValue(null, null), null);
					}
					catch (Exception x)
					{
						if (typeofTimeZoneInfo.Assembly.GetType("System.TimeZoneNotFoundException").IsInstanceOfType(x))
						{
							// MONOBUG Mono's TimeZoneInfo.Local property throws a TimeZoneNotFoundException on Windows
							// (https://bugzilla.novell.com/show_bug.cgi?id=622524)
							return SystemTimeZone.CurrentTimeZone.StandardName;
						}
						else
						{
							throw;
						}
					}
				}
				else
				{
					// HACK this is very lame and probably won't work on localized windows versions
					return SystemTimeZone.CurrentTimeZone.StandardName;
				}
#endif
			}

			public static string getSystemTimeZoneID(string javaHome, string country)
			{
				// (the switch was generated from the contents of $JAVA_HOME/lib/tzmappings)
				switch (GetCurrentTimeZoneID())
				{
					case "Romance":
					case "Romance Standard Time":
						return "Europe/Paris";
					case "Warsaw":
						return "Europe/Warsaw";
					case "Central Europe":
					case "Central Europe Standard Time":
					case "Prague Bratislava":
						return "Europe/Prague";
					case "W. Central Africa Standard Time":
						return "Africa/Luanda";
					case "FLE":
					case "FLE Standard Time":
						return "Europe/Helsinki";
					case "GFT":
					case "GFT Standard Time":
					case "GTB":
					case "GTB Standard Time":
						return "Europe/Athens";
					case "Israel":
					case "Israel Standard Time":
						return "Asia/Jerusalem";
					case "Arab":
					case "Arab Standard Time":
						return "Asia/Riyadh";
					case "Arabic Standard Time":
						return "Asia/Baghdad";
					case "E. Africa":
					case "E. Africa Standard Time":
						return "Africa/Nairobi";
					case "Saudi Arabia":
					case "Saudi Arabia Standard Time":
						return "Asia/Riyadh";
					case "Iran":
					case "Iran Standard Time":
						return "Asia/Tehran";
					case "Afghanistan":
					case "Afghanistan Standard Time":
						return "Asia/Kabul";
					case "India":
					case "India Standard Time":
						return "Asia/Calcutta";
					case "Myanmar Standard Time":
						return "Asia/Rangoon";
					case "Nepal Standard Time":
						return "Asia/Katmandu";
					case "Sri Lanka":
					case "Sri Lanka Standard Time":
						return "Asia/Colombo";
					case "Beijing":
					case "China":
					case "China Standard Time":
						return "Asia/Shanghai";
					case "AUS Central":
					case "AUS Central Standard Time":
						return "Australia/Darwin";
					case "Cen. Australia":
					case "Cen. Australia Standard Time":
						return "Australia/Adelaide";
					case "Vladivostok":
					case "Vladivostok Standard Time":
						return "Asia/Vladivostok";
					case "West Pacific":
					case "West Pacific Standard Time":
						return "Pacific/Guam";
					case "E. South America":
					case "E. South America Standard Time":
						return "America/Sao_Paulo";
					case "Greenland Standard Time":
						return "America/Godthab";
					case "Newfoundland":
					case "Newfoundland Standard Time":
						return "America/St_Johns";
					case "Pacific SA":
					case "Pacific SA Standard Time":
						return "America/Santiago";
					case "SA Western":
					case "SA Western Standard Time":
						return "America/Caracas";
					case "SA Pacific":
					case "SA Pacific Standard Time":
						return "America/Bogota";
					case "US Eastern":
					case "US Eastern Standard Time":
						return "America/Indianapolis";
					case "Central America Standard Time":
						return "America/Regina";
					case "Mexico":
					case "Mexico Standard Time":
						return "America/Mexico_City";
					case "Canada Central":
					case "Canada Central Standard Time":
						return "America/Regina";
					case "US Mountain":
					case "US Mountain Standard Time":
						return "America/Phoenix";
					case "GMT":
					case "GMT Standard Time":
						return "Europe/London";
					case "Ekaterinburg":
					case "Ekaterinburg Standard Time":
						return "Asia/Yekaterinburg";
					case "West Asia":
					case "West Asia Standard Time":
						return "Asia/Karachi";
					case "Central Asia":
					case "Central Asia Standard Time":
						return "Asia/Dhaka";
					case "N. Central Asia Standard Time":
						return "Asia/Novosibirsk";
					case "Bangkok":
					case "Bangkok Standard Time":
						return "Asia/Bangkok";
					case "North Asia Standard Time":
						return "Asia/Krasnoyarsk";
					case "SE Asia":
					case "SE Asia Standard Time":
						return "Asia/Bangkok";
					case "North Asia East Standard Time":
						return "Asia/Ulaanbaatar";
					case "Singapore":
					case "Singapore Standard Time":
						return "Asia/Singapore";
					case "Taipei":
					case "Taipei Standard Time":
						return "Asia/Taipei";
					case "W. Australia":
					case "W. Australia Standard Time":
						return "Australia/Perth";
					case "Korea":
					case "Korea Standard Time":
						return "Asia/Seoul";
					case "Tokyo":
					case "Tokyo Standard Time":
						return "Asia/Tokyo";
					case "Yakutsk":
					case "Yakutsk Standard Time":
						return "Asia/Yakutsk";
					case "Central European":
					case "Central European Standard Time":
						return "Europe/Belgrade";
					case "W. Europe":
					case "W. Europe Standard Time":
						return "Europe/Berlin";
					case "Tasmania":
					case "Tasmania Standard Time":
						return "Australia/Hobart";
					case "AUS Eastern":
					case "AUS Eastern Standard Time":
						return "Australia/Sydney";
					case "E. Australia":
					case "E. Australia Standard Time":
						return "Australia/Brisbane";
					case "Sydney Standard Time":
						return "Australia/Sydney";
					case "Central Pacific":
					case "Central Pacific Standard Time":
						return "Pacific/Guadalcanal";
					case "Dateline":
					case "Dateline Standard Time":
						return "GMT-1200";
					case "Fiji":
					case "Fiji Standard Time":
						return "Pacific/Fiji";
					case "Samoa":
					case "Samoa Standard Time":
						return "Pacific/Apia";
					case "Hawaiian":
					case "Hawaiian Standard Time":
						return "Pacific/Honolulu";
					case "Alaskan":
					case "Alaskan Standard Time":
						return "America/Anchorage";
					case "Pacific":
					case "Pacific Standard Time":
						return "America/Los_Angeles";
					case "Mexico Standard Time 2":
						return "America/Chihuahua";
					case "Mountain":
					case "Mountain Standard Time":
						return "America/Denver";
					case "Central":
					case "Central Standard Time":
						return "America/Chicago";
					case "Eastern":
					case "Eastern Standard Time":
						return "America/New_York";
					case "E. Europe":
					case "E. Europe Standard Time":
						return "Europe/Minsk";
					case "Egypt":
					case "Egypt Standard Time":
						return "Africa/Cairo";
					case "South Africa":
					case "South Africa Standard Time":
						return "Africa/Harare";
					case "Atlantic":
					case "Atlantic Standard Time":
						return "America/Halifax";
					case "SA Eastern":
					case "SA Eastern Standard Time":
						return "America/Buenos_Aires";
					case "Mid-Atlantic":
					case "Mid-Atlantic Standard Time":
						return "Atlantic/South_Georgia";
					case "Azores":
					case "Azores Standard Time":
						return "Atlantic/Azores";
					case "Cape Verde Standard Time":
						return "Atlantic/Cape_Verde";
					case "Russian":
					case "Russian Standard Time":
						return "Europe/Moscow";
					case "New Zealand":
					case "New Zealand Standard Time":
						return "Pacific/Auckland";
					case "Tonga Standard Time":
						return "Pacific/Tongatapu";
					case "Arabian":
					case "Arabian Standard Time":
						return "Asia/Muscat";
					case "Caucasus":
					case "Caucasus Standard Time":
						return "Asia/Yerevan";
					case "Greenwich":
					case "Greenwich Standard Time":
						return "GMT";
					case "Central Brazilian Standard Time":
						return "America/Manaus";
					case "Central Standard Time (Mexico)":
						return "America/Mexico_City";
					case "Georgian Standard Time":
						return "Asia/Tbilisi";
					case "Mountain Standard Time (Mexico)":
						return "America/Chihuahua";
					case "Namibia Standard Time":
						return "Africa/Windhoek";
					case "Pacific Standard Time (Mexico)":
						return "America/Tijuana";
					case "Western Brazilian Standard Time":
						return "America/Rio_Branco";
					case "Azerbaijan Standard Time":
						return "Asia/Baku";
					case "Jordan Standard Time":
						return "Asia/Amman";
					case "Middle East Standard Time":
						return "Asia/Beirut";
					default:
						// this means fall back to GMT offset
						return getSystemGMTOffsetID();
				}
			}

			public static string getSystemGMTOffsetID()
			{
				TimeSpan sp = SystemTimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
				int hours = sp.Hours;
				int mins = sp.Minutes;
				if (hours >= 0 && mins >= 0)
				{
					return String.Format("GMT+{0:D2}:{1:D2}", hours, mins);
				}
				else
				{
					return String.Format("GMT-{0:D2}:{1:D2}", -hours, -mins);
				}
			}
		}
	}
}

namespace IKVM.NativeCode.sun.awt
{
	static class FontDescriptor
	{
		public static void initIDs()
		{
		}
	}
}

namespace IKVM.NativeCode.sun.invoke.anon
{
	static class AnonymousClassLoader
	{
		public static jlClass loadClassInternal(jlClass hostClass, byte[] classFile, object[] patchArray)
		{
			throw new NotImplementedException();
		}
	}
}

namespace IKVM.NativeCode.sun.misc
{
	static class GC
	{
		public static long maxObjectInspectionAge()
		{
			return 0;
		}
	}

	static class MessageUtils
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

	static class MiscHelper
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

    static class Signal
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

        [System.Security.SecurityCritical]
        private sealed class CriticalCtrlHandler : System.Runtime.ConstrainedExecution.CriticalFinalizerObject
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

            [System.Security.SecuritySafeCritical]
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
			global::java.util.Map traces = global::java.lang.Thread.getAllStackTraces();
			Console.WriteLine("Full thread dump IKVM.NET {0} ({1} bit):", JVM.SafeGetAssemblyVersion(Assembly.GetExecutingAssembly()), IntPtr.Size * 8);
			global::java.util.Iterator entries = traces.entrySet().iterator();
			while (entries.hasNext())
			{
				global::java.util.Map.Entry entry = (global::java.util.Map.Entry)entries.next();
				global::java.lang.Thread thread = (global::java.lang.Thread)entry.getKey();
				Console.WriteLine("\n\"{0}\"{1} prio={2} tid=0x{3:X8}", thread.getName(), thread.isDaemon() ? " daemon" : "", thread.getPriority(), thread.getId());
				Console.WriteLine("   java.lang.Thread.State: " + thread.getState());
				global::java.lang.StackTraceElement[] trace = (global::java.lang.StackTraceElement[])entry.getValue();
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
		[System.Security.SecuritySafeCritical]
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
						catch (System.Security.SecurityException)
						{
						}
                    }
                    break;
                case 1: // Ignore Signal
                    break;
                case 2: // Custom Signal Handler
                    switch(sig){
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
            global::java.security.AccessController.doPrivileged(global::ikvm.runtime.Delegates.toPrivilegedAction(delegate
            {
                global::java.lang.Class clazz = typeof(global::sun.misc.Signal);
                global::java.lang.reflect.Method dispatch = clazz.getDeclaredMethod("dispatch", global::java.lang.Integer.TYPE);
                dispatch.setAccessible(true);
                dispatch.invoke(null, global::java.lang.Integer.valueOf(sig));
                return null;
            }));
#endif
        }
    }

	static class NativeSignalHandler
	{
		public static void handle0(int number, long handler)
		{
			throw new NotImplementedException();
		}
	}

	static class Perf
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
			return global::java.nio.ByteBuffer.allocate(8);
#endif
		}

		public static object createByteArray(object thisPerf, string name, int variability, int units, byte[] value, int maxLength)
		{
#if FIRST_PASS
			return null;
#else
			return global::java.nio.ByteBuffer.allocate(maxLength).put(value);
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

	static class Unsafe
	{
		public static void throwException(object thisUnsafe, Exception x)
		{
			throw x;
		}

		public static void ensureClassInitialized(object thisUnsafe, jlClass clazz)
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

		[System.Security.SecurityCritical]
		public static object allocateInstance(object thisUnsafe, jlClass clazz)
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

		public static jlClass defineClass(object thisUnsafe, string name, byte[] buf, int offset, int length, jlClassLoader cl, ProtectionDomain pd)
		{
#if FIRST_PASS
			return null;
#else
			return cl.defineClass(name, buf, offset, length, pd);
#endif
		}
	}

	static class Version
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

	static class VM
	{
		public static void initialize()
		{
		}
	}

	static class VMSupport
	{
		public static object initAgentProperties(object props)
		{
			return props;
		}
	}
}

namespace IKVM.NativeCode.sun.net.spi
{
	static class DefaultProxySelector
	{
		public static bool init()
		{
			return true;
		}

		public static object getSystemProxy(object thisDefaultProxySelector, string protocol, string host)
		{
			// TODO on Whidbey we might be able to use System.Net.Configuration.DefaultProxySection.Proxy
			return null;
		}
	}
}

namespace IKVM.NativeCode.sun.nio.fs
{
	static class NetPath
	{
		public static string toRealPathImpl(string path)
		{
#if FIRST_PASS
			return null;
#else
			path = global::java.io.FileSystem.getFileSystem().canonicalize(path);
			if (VirtualFileSystem.IsVirtualFS(path))
			{
				if (VirtualFileSystem.CheckAccess(path, Java_java_io_Win32FileSystem.ACCESS_READ))
				{
					return path;
				}
				throw new global::java.nio.file.NoSuchFileException(path);
			}
			try
			{
				System.IO.File.GetAttributes(path);
				return path;
			}
			catch (System.IO.FileNotFoundException)
			{
				throw new global::java.nio.file.NoSuchFileException(path);
			}
			catch (System.IO.DirectoryNotFoundException)
			{
				throw new global::java.nio.file.NoSuchFileException(path);
			}
			catch (System.UnauthorizedAccessException)
			{
				throw new global::java.nio.file.AccessDeniedException(path);
			}
			catch (System.Security.SecurityException)
			{
				throw new global::java.nio.file.AccessDeniedException(path);
			}
			catch (System.ArgumentException x)
			{
				throw new global::java.nio.file.FileSystemException(path, null, x.Message);
			}
			catch (System.NotSupportedException x)
			{
				throw new global::java.nio.file.FileSystemException(path, null, x.Message);
			}
			catch (System.IO.IOException x)
			{
				throw new global::java.nio.file.FileSystemException(path, null, x.Message);
			}
#endif
		}
	}
}

namespace IKVM.NativeCode.sun.reflect
{
#if !FIRST_PASS
	public interface IReflectionException
	{
		jlIllegalArgumentException GetIllegalArgumentException(object obj);
		jlIllegalArgumentException SetIllegalArgumentException(object obj);
	}
#endif

	// this must be public (due to .NET 4.0 security model)
	public sealed class State
	{
		internal int Value;
	}

	static class Reflection
	{
#if CLASSGC
		private static readonly ConditionalWeakTable<MethodBase, State> isHideFromJavaCache = new ConditionalWeakTable<MethodBase, State>();

		internal static bool IsHideFromJava(MethodBase mb)
		{
			State state = isHideFromJavaCache.GetOrCreateValue(mb);
			if (state.Value == 0)
			{
				state.Value = IsHideFromJavaImpl(mb);
			}
			return state.Value == 1;
		}

		private static int IsHideFromJavaImpl(MethodBase mb)
		{
			if (mb.Name.StartsWith("__<", StringComparison.Ordinal))
			{
				return 1;
			}
			if (mb.IsDefined(typeof(IKVM.Attributes.HideFromJavaAttribute), false) || mb.IsDefined(typeof(IKVM.Attributes.HideFromReflectionAttribute), false))
			{
				return 1;
			}
			return 2;
		}
#else
		private static readonly Dictionary<RuntimeMethodHandle, bool> isHideFromJavaCache = new Dictionary<RuntimeMethodHandle, bool>();

		internal static bool IsHideFromJava(MethodBase mb)
		{
			if (mb.Name.StartsWith("__<", StringComparison.Ordinal))
			{
				return true;
			}
			RuntimeMethodHandle handle;
			try
			{
				handle = mb.MethodHandle;
			}
			catch (InvalidOperationException)
			{
				// DynamicMethods don't have a RuntimeMethodHandle and we always want to hide them anyway
				return true;
			}
			catch (NotSupportedException)
			{
				// DynamicMethods don't have a RuntimeMethodHandle and we always want to hide them anyway
				return true;
			}
			lock (isHideFromJavaCache)
			{
				bool cached;
				if (isHideFromJavaCache.TryGetValue(handle, out cached))
				{
					return cached;
				}
			}
			bool isHide = mb.IsDefined(typeof(IKVM.Attributes.HideFromJavaAttribute), false) || mb.IsDefined(typeof(IKVM.Attributes.HideFromReflectionAttribute), false);
			lock (isHideFromJavaCache)
			{
				isHideFromJavaCache[handle] = isHide;
			}
			return isHide;
		}
#endif

		// NOTE this method is hooked up explicitly through map.xml to prevent inlining of the native stub
		// and tail-call optimization in the native stub.
		public static object getCallerClass(int realFramesToSkip)
		{
#if FIRST_PASS
			return null;
#else
			int i = 3;
			if (realFramesToSkip <= 1)
			{
				i = 1;
				realFramesToSkip = Math.Max(realFramesToSkip + 2, 2);
			}
			realFramesToSkip--;
			for (; ; )
			{
				MethodBase method = new StackFrame(i++, false).GetMethod();
				if (method == null)
				{
					return null;
				}
				Type type = method.DeclaringType;
				// NOTE these checks should be the same as the ones in SecurityManager.getClassContext
				if (IsHideFromJava(method)
					|| type == null
					|| type.Assembly == typeof(object).Assembly
					|| type.Assembly == typeof(Reflection).Assembly
					|| type.Assembly == Java_java_lang_SecurityManager.jniAssembly
					|| type == typeof(jlrMethod)
					|| type == typeof(jlrConstructor))
				{
					continue;
				}
				if (--realFramesToSkip == 0)
				{
					return ClassLoaderWrapper.GetWrapperFromType(type).ClassObject;
				}
			}
#endif
		}

		public static int getClassAccessFlags(jlClass clazz)
		{
			// the mask comes from JVM_RECOGNIZED_CLASS_MODIFIERS in src/hotspot/share/vm/prims/jvm.h
			int mods = (int)TypeWrapper.FromClass(clazz).Modifiers & 0x7631;
			// interface implies abstract
			mods |= (mods & 0x0200) << 1;
			return mods;
		}

		public static bool checkInternalAccess(jlClass currentClass, jlClass memberClass)
		{
			TypeWrapper current = TypeWrapper.FromClass(currentClass);
			TypeWrapper member = TypeWrapper.FromClass(memberClass);
			return member.IsInternal && member.InternalsVisibleTo(current);
		}
	}

	static class ReflectionFactory
	{
#if !FIRST_PASS
		private static object[] ConvertArgs(ClassLoaderWrapper loader, TypeWrapper[] argumentTypes, object[] args)
		{
			object[] nargs = new object[args == null ? 0 : args.Length];
			if (nargs.Length != argumentTypes.Length)
			{
				throw new jlIllegalArgumentException("wrong number of arguments");
			}
			for (int i = 0; i < nargs.Length; i++)
			{
				if (argumentTypes[i].IsPrimitive)
				{
					if (args[i] == null)
					{
						throw new jlIllegalArgumentException("primitive wrapper null");
					}
					nargs[i] = JVM.Unbox(args[i]);
					// NOTE we depend on the fact that the .NET reflection parameter type
					// widening rules are the same as in Java, but to have this work for byte
					// we need to convert byte to sbyte.
					if (nargs[i] is byte && argumentTypes[i] != PrimitiveTypeWrapper.BYTE)
					{
						nargs[i] = (sbyte)(byte)nargs[i];
					}
				}
				else
				{
					if (args[i] != null && !argumentTypes[i].EnsureLoadable(loader).IsInstance(args[i]))
					{
						throw new jlIllegalArgumentException();
					}
					nargs[i] = args[i];
				}
			}
			return nargs;
		}

		private sealed class MethodAccessorImpl : srMethodAccessor
		{
			private readonly MethodWrapper mw;

			internal MethodAccessorImpl(jlrMethod method)
			{
				mw = MethodWrapper.FromMethodOrConstructor(method);
			}

			[IKVM.Attributes.HideFromJava]
			public object invoke(object obj, object[] args, global::ikvm.@internal.CallerID callerID)
			{
				if (!mw.IsStatic && !mw.DeclaringType.IsInstance(obj))
				{
					if (obj == null)
					{
						throw new jlNullPointerException();
					}
					throw new jlIllegalArgumentException("object is not an instance of declaring class");
				}
				args = ConvertArgs(mw.DeclaringType.GetClassLoader(), mw.GetParameters(), args);
				// if the method is an interface method, we must explicitly run <clinit>,
				// because .NET reflection doesn't
				if (mw.DeclaringType.IsInterface)
				{
					mw.DeclaringType.RunClassInit();
				}
				object retval;
				try
				{
					retval = ((ICustomInvoke)mw).Invoke(obj, args);
				}
				catch (MethodAccessException x)
				{
					// this can happen if we're calling a non-public method and the call stack doesn't have ReflectionPermission.MemberAccess
					throw new jlIllegalAccessException().initCause(x);
				}
				if (mw.ReturnType.IsPrimitive && mw.ReturnType != PrimitiveTypeWrapper.VOID)
				{
					retval = JVM.Box(retval);
				}
				return retval;
			}
		}

		internal sealed class FastMethodAccessorImpl : srMethodAccessor
		{
			private static readonly MethodInfo valueOfByte;
			private static readonly MethodInfo valueOfBoolean;
			private static readonly MethodInfo valueOfChar;
			private static readonly MethodInfo valueOfShort;
			private static readonly MethodInfo valueOfInt;
			private static readonly MethodInfo valueOfFloat;
			private static readonly MethodInfo valueOfLong;
			private static readonly MethodInfo valueOfDouble;
			private static readonly MethodInfo byteValue;
			private static readonly MethodInfo booleanValue;
			private static readonly MethodInfo charValue;
			private static readonly MethodInfo shortValue;
			private static readonly MethodInfo intValue;
			private static readonly MethodInfo floatValue;
			private static readonly MethodInfo longValue;
			private static readonly MethodInfo doubleValue;
			internal static readonly ConstructorInfo invocationTargetExceptionCtor;
			internal static readonly ConstructorInfo illegalArgumentExceptionCtor;
			internal static readonly MethodInfo get_TargetSite;
			internal static readonly MethodInfo GetCurrentMethod;

			private delegate object Invoker(object obj, object[] args, global::ikvm.@internal.CallerID callerID);
			private Invoker invoker;

			static FastMethodAccessorImpl()
			{
				valueOfByte = typeof(jlByte).GetMethod("valueOf", new Type[] { typeof(byte) });
				valueOfBoolean = typeof(jlBoolean).GetMethod("valueOf", new Type[] { typeof(bool) });
				valueOfChar = typeof(jlCharacter).GetMethod("valueOf", new Type[] { typeof(char) });
				valueOfShort = typeof(jlShort).GetMethod("valueOf", new Type[] { typeof(short) });
				valueOfInt = typeof(jlInteger).GetMethod("valueOf", new Type[] { typeof(int) });
				valueOfFloat = typeof(jlFloat).GetMethod("valueOf", new Type[] { typeof(float) });
				valueOfLong = typeof(jlLong).GetMethod("valueOf", new Type[] { typeof(long) });
				valueOfDouble = typeof(jlDouble).GetMethod("valueOf", new Type[] { typeof(double) });

				byteValue = typeof(jlByte).GetMethod("byteValue", Type.EmptyTypes);
				booleanValue = typeof(jlBoolean).GetMethod("booleanValue", Type.EmptyTypes);
				charValue = typeof(jlCharacter).GetMethod("charValue", Type.EmptyTypes);
				shortValue = typeof(jlShort).GetMethod("shortValue", Type.EmptyTypes);
				intValue = typeof(jlInteger).GetMethod("intValue", Type.EmptyTypes);
				floatValue = typeof(jlFloat).GetMethod("floatValue", Type.EmptyTypes);
				longValue = typeof(jlLong).GetMethod("longValue", Type.EmptyTypes);
				doubleValue = typeof(jlDouble).GetMethod("doubleValue", Type.EmptyTypes);

				invocationTargetExceptionCtor = typeof(jlrInvocationTargetException).GetConstructor(new Type[] { typeof(Exception) });
				illegalArgumentExceptionCtor = typeof(jlIllegalArgumentException).GetConstructor(Type.EmptyTypes);
				get_TargetSite = typeof(Exception).GetMethod("get_TargetSite");
				GetCurrentMethod = typeof(MethodBase).GetMethod("GetCurrentMethod");
			}

			private sealed class RunClassInit
			{
				private FastMethodAccessorImpl outer;
				private TypeWrapper tw;
				private Invoker invoker;

				internal RunClassInit(FastMethodAccessorImpl outer, TypeWrapper tw, Invoker invoker)
				{
					this.outer = outer;
					this.tw = tw;
					this.invoker = invoker;
				}

				[IKVM.Attributes.HideFromJava]
				internal object invoke(object obj, object[] args, global::ikvm.@internal.CallerID callerID)
				{
					// FXBUG pre-SP1 a DynamicMethod that calls a static method doesn't trigger the cctor, so we do that explicitly.
					// even on .NET 2.0 SP2, interface method invocations don't run the interface cctor
					// NOTE when testing, please test both the x86 and x64 CLR JIT, because they have different bugs (even on .NET 2.0 SP2)
					tw.RunClassInit();
					outer.invoker = invoker;
					return invoker(obj, args, callerID);
				}
			}

			internal FastMethodAccessorImpl(jlrMethod method, bool nonvirtual)
			{
				MethodWrapper mw = MethodWrapper.FromMethodOrConstructor(method);
				TypeWrapper[] parameters;
				try
				{
					mw.DeclaringType.Finish();
					parameters = mw.GetParameters();
					for (int i = 0; i < parameters.Length; i++)
					{
						// the EnsureLoadable shouldn't fail, because we don't allow a java.lang.reflect.Method
						// to "escape" if it has an unloadable type in the signature
						parameters[i] = parameters[i].EnsureLoadable(mw.DeclaringType.GetClassLoader());
						parameters[i].Finish();
					}
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				mw.ResolveMethod();
				DynamicMethod dm = DynamicMethodUtils.Create("__<Invoker>", mw.DeclaringType.TypeAsBaseType, !mw.IsPublic || !mw.DeclaringType.IsPublic || nonvirtual, typeof(object), new Type[] { typeof(object), typeof(object[]), typeof(global::ikvm.@internal.CallerID) });
				CodeEmitter ilgen = CodeEmitter.Create(dm);
				CodeEmitterLocal ret = ilgen.DeclareLocal(typeof(object));
				if (!mw.IsStatic)
				{
					// check target for null
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.EmitNullCheck();
				}

				// check args length
				CodeEmitterLabel argsLengthOK = ilgen.DefineLabel();
				if (parameters.Length == 0)
				{
					// zero length array may be null
					ilgen.Emit(OpCodes.Ldarg_1);
					ilgen.EmitBrfalse(argsLengthOK);
				}
				ilgen.Emit(OpCodes.Ldarg_1);
				ilgen.Emit(OpCodes.Ldlen);
				ilgen.EmitLdc_I4(parameters.Length);
				ilgen.EmitBeq(argsLengthOK);
				ilgen.Emit(OpCodes.Newobj, illegalArgumentExceptionCtor);
				ilgen.Emit(OpCodes.Throw);
				ilgen.MarkLabel(argsLengthOK);

				int thisCount = mw.IsStatic ? 0 : 1;
				CodeEmitterLocal[] args = new CodeEmitterLocal[parameters.Length + thisCount];
				if (!mw.IsStatic)
				{
					args[0] = ilgen.DeclareLocal(mw.DeclaringType.TypeAsSignatureType);
				}
				for (int i = thisCount; i < args.Length; i++)
				{
					args[i] = ilgen.DeclareLocal(parameters[i - thisCount].TypeAsSignatureType);
				}
				ilgen.BeginExceptionBlock();
				if (!mw.IsStatic)
				{
					ilgen.Emit(OpCodes.Ldarg_0);
					mw.DeclaringType.EmitCheckcast(ilgen);
					mw.DeclaringType.EmitConvStackTypeToSignatureType(ilgen, null);
					ilgen.Emit(OpCodes.Stloc, args[0]);
				}
				for (int i = thisCount; i < args.Length; i++)
				{
					ilgen.Emit(OpCodes.Ldarg_1);
					ilgen.EmitLdc_I4(i - thisCount);
					ilgen.Emit(OpCodes.Ldelem_Ref);
					TypeWrapper tw = parameters[i - thisCount];
					EmitUnboxArg(ilgen, tw);
					tw.EmitConvStackTypeToSignatureType(ilgen, null);
					ilgen.Emit(OpCodes.Stloc, args[i]);
				}
				CodeEmitterLabel label1 = ilgen.DefineLabel();
				ilgen.EmitLeave(label1);
				ilgen.BeginCatchBlock(typeof(InvalidCastException));
				ilgen.Emit(OpCodes.Newobj, illegalArgumentExceptionCtor);
				ilgen.Emit(OpCodes.Throw);
				ilgen.BeginCatchBlock(typeof(NullReferenceException));
				ilgen.Emit(OpCodes.Newobj, illegalArgumentExceptionCtor);
				ilgen.Emit(OpCodes.Throw);
				ilgen.EndExceptionBlock();

				// this is the actual call
				ilgen.MarkLabel(label1);
				ilgen.BeginExceptionBlock();
				for (int i = 0; i < args.Length; i++)
				{
					if (i == 0 && !mw.IsStatic && (mw.DeclaringType.IsNonPrimitiveValueType || mw.DeclaringType.IsGhost))
					{
						ilgen.Emit(OpCodes.Ldloca, args[i]);
					}
					else
					{
						ilgen.Emit(OpCodes.Ldloc, args[i]);
					}
				}
				if (mw.HasCallerID)
				{
					ilgen.Emit(OpCodes.Ldarg_2);
				}
				if (mw.IsStatic || nonvirtual)
				{
					mw.EmitCall(ilgen);
				}
				else
				{
					mw.EmitCallvirtReflect(ilgen);
				}
				mw.ReturnType.EmitConvSignatureTypeToStackType(ilgen);
				BoxReturnValue(ilgen, mw.ReturnType);
				ilgen.Emit(OpCodes.Stloc, ret);
				CodeEmitterLabel label2 = ilgen.DefineLabel();
				ilgen.EmitLeave(label2);
				ilgen.BeginCatchBlock(typeof(Exception));
				CodeEmitterLabel label = ilgen.DefineLabel();
				CodeEmitterLabel labelWrap = ilgen.DefineLabel();
				if (IntPtr.Size == 8 && nonvirtual)
				{
					// This is a workaround for the x64 JIT, which is completely broken as usual.
					// When MethodBase.GetCurrentMethod() is used in a dynamic method that isn't verifiable,
					// we get an access violation at JIT time. When we're doing a nonvirtual call,
					// the method is not verifiable, so we disable this check (which, at worst, results
					// in any exceptions thrown at the call site being incorrectly wrapped in an InvocationTargetException).
				}
				else
				{
					// If the exception we caught is a jlrInvocationTargetException, we know it must be
					// wrapped, because .NET won't throw that exception and we also cannot check the target site,
					// because it may be the same as us if a method is recursively invoking itself.
					ilgen.Emit(OpCodes.Dup);
					ilgen.Emit(OpCodes.Isinst, typeof(jlrInvocationTargetException));
					ilgen.EmitBrtrue(labelWrap);
					ilgen.Emit(OpCodes.Dup);
					ilgen.Emit(OpCodes.Callvirt, get_TargetSite);
					ilgen.Emit(OpCodes.Call, GetCurrentMethod);
					ilgen.Emit(OpCodes.Ceq);
					ilgen.EmitBrtrue(label);
				}
				ilgen.MarkLabel(labelWrap);
				ilgen.Emit(OpCodes.Ldc_I4_0);
				ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.mapException.MakeGenericMethod(Types.Exception));
				ilgen.Emit(OpCodes.Newobj, invocationTargetExceptionCtor);
				ilgen.MarkLabel(label);
				ilgen.Emit(OpCodes.Throw);
				ilgen.EndExceptionBlock();

				ilgen.MarkLabel(label2);
				ilgen.Emit(OpCodes.Ldloc, ret);
				ilgen.Emit(OpCodes.Ret);
				ilgen.DoEmit();
				invoker = (Invoker)dm.CreateDelegate(typeof(Invoker));
				if ((mw.IsStatic || mw.DeclaringType.IsInterface) && mw.DeclaringType.HasStaticInitializer)
				{
					invoker = new Invoker(new RunClassInit(this, mw.DeclaringType, invoker).invoke);
				}
			}

			private static void Expand(CodeEmitter ilgen, TypeWrapper type)
			{
				if (type == PrimitiveTypeWrapper.FLOAT)
				{
					ilgen.Emit(OpCodes.Conv_R4);
				}
				else if (type == PrimitiveTypeWrapper.LONG)
				{
					ilgen.Emit(OpCodes.Conv_I8);
				}
				else if (type == PrimitiveTypeWrapper.DOUBLE)
				{
					ilgen.Emit(OpCodes.Conv_R8);
				}
			}

			internal static void EmitUnboxArg(CodeEmitter ilgen, TypeWrapper type)
			{
				if (type == PrimitiveTypeWrapper.BYTE)
				{
					ilgen.Emit(OpCodes.Castclass, typeof(jlByte));
					ilgen.Emit(OpCodes.Call, byteValue);
				}
				else if (type == PrimitiveTypeWrapper.BOOLEAN)
				{
					ilgen.Emit(OpCodes.Castclass, typeof(jlBoolean));
					ilgen.Emit(OpCodes.Call, booleanValue);
				}
				else if (type == PrimitiveTypeWrapper.CHAR)
				{
					ilgen.Emit(OpCodes.Castclass, typeof(jlCharacter));
					ilgen.Emit(OpCodes.Call, charValue);
				}
				else if (type == PrimitiveTypeWrapper.SHORT
					|| type == PrimitiveTypeWrapper.INT
					|| type == PrimitiveTypeWrapper.FLOAT
					|| type == PrimitiveTypeWrapper.LONG
					|| type == PrimitiveTypeWrapper.DOUBLE)
				{
					ilgen.Emit(OpCodes.Dup);
					ilgen.Emit(OpCodes.Isinst, typeof(jlByte));
					CodeEmitterLabel next = ilgen.DefineLabel();
					ilgen.EmitBrfalse(next);
					ilgen.Emit(OpCodes.Castclass, typeof(jlByte));
					ilgen.Emit(OpCodes.Call, byteValue);
					ilgen.Emit(OpCodes.Conv_I1);
					Expand(ilgen, type);
					CodeEmitterLabel done = ilgen.DefineLabel();
					ilgen.EmitBr(done);
					ilgen.MarkLabel(next);
					if (type == PrimitiveTypeWrapper.SHORT)
					{
						ilgen.Emit(OpCodes.Castclass, typeof(jlShort));
						ilgen.Emit(OpCodes.Call, shortValue);
					}
					else
					{
						ilgen.Emit(OpCodes.Dup);
						ilgen.Emit(OpCodes.Isinst, typeof(jlShort));
						next = ilgen.DefineLabel();
						ilgen.EmitBrfalse(next);
						ilgen.Emit(OpCodes.Castclass, typeof(jlShort));
						ilgen.Emit(OpCodes.Call, shortValue);
						Expand(ilgen, type);
						ilgen.EmitBr(done);
						ilgen.MarkLabel(next);
						ilgen.Emit(OpCodes.Dup);
						ilgen.Emit(OpCodes.Isinst, typeof(jlCharacter));
						next = ilgen.DefineLabel();
						ilgen.EmitBrfalse(next);
						ilgen.Emit(OpCodes.Castclass, typeof(jlCharacter));
						ilgen.Emit(OpCodes.Call, charValue);
						Expand(ilgen, type);
						ilgen.EmitBr(done);
						ilgen.MarkLabel(next);
						if (type == PrimitiveTypeWrapper.INT)
						{
							ilgen.Emit(OpCodes.Castclass, typeof(jlInteger));
							ilgen.Emit(OpCodes.Call, intValue);
						}
						else
						{
							ilgen.Emit(OpCodes.Dup);
							ilgen.Emit(OpCodes.Isinst, typeof(jlInteger));
							next = ilgen.DefineLabel();
							ilgen.EmitBrfalse(next);
							ilgen.Emit(OpCodes.Castclass, typeof(jlInteger));
							ilgen.Emit(OpCodes.Call, intValue);
							Expand(ilgen, type);
							ilgen.EmitBr(done);
							ilgen.MarkLabel(next);
							if (type == PrimitiveTypeWrapper.LONG)
							{
								ilgen.Emit(OpCodes.Castclass, typeof(jlLong));
								ilgen.Emit(OpCodes.Call, longValue);
							}
							else
							{
								ilgen.Emit(OpCodes.Dup);
								ilgen.Emit(OpCodes.Isinst, typeof(jlLong));
								next = ilgen.DefineLabel();
								ilgen.EmitBrfalse(next);
								ilgen.Emit(OpCodes.Castclass, typeof(jlLong));
								ilgen.Emit(OpCodes.Call, longValue);
								Expand(ilgen, type);
								ilgen.EmitBr(done);
								ilgen.MarkLabel(next);
								if (type == PrimitiveTypeWrapper.FLOAT)
								{
									ilgen.Emit(OpCodes.Castclass, typeof(jlFloat));
									ilgen.Emit(OpCodes.Call, floatValue);
								}
								else if (type == PrimitiveTypeWrapper.DOUBLE)
								{
									ilgen.Emit(OpCodes.Dup);
									ilgen.Emit(OpCodes.Isinst, typeof(jlFloat));
									next = ilgen.DefineLabel();
									ilgen.EmitBrfalse(next);
									ilgen.Emit(OpCodes.Castclass, typeof(jlFloat));
									ilgen.Emit(OpCodes.Call, floatValue);
									ilgen.EmitBr(done);
									ilgen.MarkLabel(next);
									ilgen.Emit(OpCodes.Castclass, typeof(jlDouble));
									ilgen.Emit(OpCodes.Call, doubleValue);
								}
								else
								{
									throw new InvalidOperationException();
								}
							}
						}
					}
					ilgen.MarkLabel(done);
				}
				else
				{
					type.EmitCheckcast(ilgen);
				}
			}

			private static void BoxReturnValue(CodeEmitter ilgen, TypeWrapper type)
			{
				if (type == PrimitiveTypeWrapper.VOID)
				{
					ilgen.Emit(OpCodes.Ldnull);
				}
				else if (type == PrimitiveTypeWrapper.BYTE)
				{
					ilgen.Emit(OpCodes.Call, valueOfByte);
				}
				else if (type == PrimitiveTypeWrapper.BOOLEAN)
				{
					ilgen.Emit(OpCodes.Call, valueOfBoolean);
				}
				else if (type == PrimitiveTypeWrapper.CHAR)
				{
					ilgen.Emit(OpCodes.Call, valueOfChar);
				}
				else if (type == PrimitiveTypeWrapper.SHORT)
				{
					ilgen.Emit(OpCodes.Call, valueOfShort);
				}
				else if (type == PrimitiveTypeWrapper.INT)
				{
					ilgen.Emit(OpCodes.Call, valueOfInt);
				}
				else if (type == PrimitiveTypeWrapper.FLOAT)
				{
					ilgen.Emit(OpCodes.Call, valueOfFloat);
				}
				else if (type == PrimitiveTypeWrapper.LONG)
				{
					ilgen.Emit(OpCodes.Call, valueOfLong);
				}
				else if (type == PrimitiveTypeWrapper.DOUBLE)
				{
					ilgen.Emit(OpCodes.Call, valueOfDouble);
				}
			}

			[IKVM.Attributes.HideFromJava]
			public object invoke(object obj, object[] args, global::ikvm.@internal.CallerID callerID)
			{
				try
				{
					return invoker(obj, args, callerID);
				}
				catch (MethodAccessException x)
				{
					// this can happen if we're calling a non-public method and the call stack doesn't have ReflectionPermission.MemberAccess
					throw new jlIllegalAccessException().initCause(x);
				}
			}
		}

		private sealed class FastConstructorAccessorImpl : srConstructorAccessor
		{
			private delegate object Invoker(object[] args);
			private Invoker invoker;

			internal FastConstructorAccessorImpl(jlrConstructor constructor)
			{
				MethodWrapper mw = MethodWrapper.FromMethodOrConstructor(constructor);
				TypeWrapper[] parameters;
				try
				{
					mw.DeclaringType.Finish();
					parameters = mw.GetParameters();
					for (int i = 0; i < parameters.Length; i++)
					{
						// the EnsureLoadable shouldn't fail, because we don't allow a java.lang.reflect.Method
						// to "escape" if it has an unloadable type in the signature
						parameters[i] = parameters[i].EnsureLoadable(mw.DeclaringType.GetClassLoader());
						parameters[i].Finish();
					}
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				mw.ResolveMethod();
				DynamicMethod dm = DynamicMethodUtils.Create("__<Invoker>", mw.DeclaringType.TypeAsTBD, !mw.IsPublic || !mw.DeclaringType.IsPublic, typeof(object), new Type[] { typeof(object[]) });
				CodeEmitter ilgen = CodeEmitter.Create(dm);
				CodeEmitterLocal ret = ilgen.DeclareLocal(typeof(object));

				// check args length
				CodeEmitterLabel argsLengthOK = ilgen.DefineLabel();
				if (parameters.Length == 0)
				{
					// zero length array may be null
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.EmitBrfalse(argsLengthOK);
				}
				ilgen.Emit(OpCodes.Ldarg_0);
				ilgen.Emit(OpCodes.Ldlen);
				ilgen.EmitLdc_I4(parameters.Length);
				ilgen.EmitBeq(argsLengthOK);
				ilgen.Emit(OpCodes.Newobj, FastMethodAccessorImpl.illegalArgumentExceptionCtor);
				ilgen.Emit(OpCodes.Throw);
				ilgen.MarkLabel(argsLengthOK);

				CodeEmitterLocal[] args = new CodeEmitterLocal[parameters.Length];
				for (int i = 0; i < args.Length; i++)
				{
					args[i] = ilgen.DeclareLocal(parameters[i].TypeAsSignatureType);
				}
				ilgen.BeginExceptionBlock();
				for (int i = 0; i < args.Length; i++)
				{
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.EmitLdc_I4(i);
					ilgen.Emit(OpCodes.Ldelem_Ref);
					TypeWrapper tw = parameters[i];
					FastMethodAccessorImpl.EmitUnboxArg(ilgen, tw);
					tw.EmitConvStackTypeToSignatureType(ilgen, null);
					ilgen.Emit(OpCodes.Stloc, args[i]);
				}
				CodeEmitterLabel label1 = ilgen.DefineLabel();
				ilgen.EmitLeave(label1);
				ilgen.BeginCatchBlock(typeof(InvalidCastException));
				ilgen.Emit(OpCodes.Newobj, FastMethodAccessorImpl.illegalArgumentExceptionCtor);
				ilgen.Emit(OpCodes.Throw);
				ilgen.BeginCatchBlock(typeof(NullReferenceException));
				ilgen.Emit(OpCodes.Newobj, FastMethodAccessorImpl.illegalArgumentExceptionCtor);
				ilgen.Emit(OpCodes.Throw);
				ilgen.EndExceptionBlock();

				// this is the actual call
				ilgen.MarkLabel(label1);
				ilgen.BeginExceptionBlock();
				for (int i = 0; i < args.Length; i++)
				{
					ilgen.Emit(OpCodes.Ldloc, args[i]);
				}
				mw.EmitNewobj(ilgen);
				ilgen.Emit(OpCodes.Stloc, ret);
				CodeEmitterLabel label2 = ilgen.DefineLabel();
				ilgen.EmitLeave(label2);
				ilgen.BeginCatchBlock(typeof(Exception));
				ilgen.Emit(OpCodes.Dup);
				ilgen.Emit(OpCodes.Callvirt, FastMethodAccessorImpl.get_TargetSite);
				ilgen.Emit(OpCodes.Call, FastMethodAccessorImpl.GetCurrentMethod);
				ilgen.Emit(OpCodes.Ceq);
				CodeEmitterLabel label = ilgen.DefineLabel();
				ilgen.EmitBrtrue(label);
				ilgen.Emit(OpCodes.Ldc_I4_0);
				ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.mapException.MakeGenericMethod(Types.Exception));
				ilgen.Emit(OpCodes.Newobj, FastMethodAccessorImpl.invocationTargetExceptionCtor);
				ilgen.MarkLabel(label);
				ilgen.Emit(OpCodes.Throw);
				ilgen.EndExceptionBlock();

				ilgen.MarkLabel(label2);
				ilgen.Emit(OpCodes.Ldloc, ret);
				ilgen.Emit(OpCodes.Ret);
				ilgen.DoEmit();
				invoker = (Invoker)dm.CreateDelegate(typeof(Invoker));
			}

			[IKVM.Attributes.HideFromJava]
			public object newInstance(object[] args)
			{
				try
				{
					return invoker(args);
				}
				catch (MethodAccessException x)
				{
					// this can happen if we're calling a non-public method and the call stack doesn't have ReflectionPermission.MemberAccess
					throw new jlIllegalAccessException().initCause(x);
				}
			}
		}

		private sealed class FastSerializationConstructorAccessorImpl : srConstructorAccessor
		{
			private static readonly MethodInfo GetTypeFromHandleMethod = typeof(Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) });
			private static readonly MethodInfo GetUninitializedObjectMethod = typeof(FormatterServices).GetMethod("GetUninitializedObject", new Type[] { typeof(Type) });
			private delegate object InvokeCtor();
			private InvokeCtor invoker;

			internal FastSerializationConstructorAccessorImpl(jlrConstructor constructorToCall, jlClass classToInstantiate)
			{
				MethodWrapper constructor = MethodWrapper.FromMethodOrConstructor(constructorToCall);
				if (constructor.GetParameters().Length != 0)
				{
					throw new NotImplementedException("Serialization constructor cannot have parameters");
				}
				constructor.Link();
				constructor.ResolveMethod();
				Type type;
				try
				{
					TypeWrapper wrapper = TypeWrapper.FromClass(classToInstantiate);
					wrapper.Finish();
					type = wrapper.TypeAsBaseType;
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				DynamicMethod dm = DynamicMethodUtils.Create("__<SerializationCtor>", constructor.DeclaringType.TypeAsBaseType, true, typeof(object), null);
				CodeEmitter ilgen = CodeEmitter.Create(dm);
				ilgen.Emit(OpCodes.Ldtoken, type);
				ilgen.Emit(OpCodes.Call, GetTypeFromHandleMethod);
				ilgen.Emit(OpCodes.Call, GetUninitializedObjectMethod);
				ilgen.Emit(OpCodes.Dup);
				constructor.EmitCall(ilgen);
				ilgen.Emit(OpCodes.Ret);
				ilgen.DoEmit();
				invoker = (InvokeCtor)dm.CreateDelegate(typeof(InvokeCtor));
			}

			[IKVM.Attributes.HideFromJava]
			public object newInstance(object[] args)
			{
				try
				{
					return invoker();
				}
				catch (MethodAccessException x)
				{
					// this can happen if we're calling a non-public method and the call stack doesn't have ReflectionPermission.MemberAccess
					throw new jlIllegalAccessException().initCause(x);
				}
			}
		}

		sealed class ActivatorConstructorAccessor : srConstructorAccessor
		{
			private readonly Type type;

			internal ActivatorConstructorAccessor(MethodWrapper mw)
			{
				this.type = mw.DeclaringType.TypeAsBaseType;
			}

			public object newInstance(object[] objarr)
			{
#if FIRST_PASS
				return null;
#else
				if (objarr != null && objarr.Length != 0)
				{
					throw new global::java.lang.IllegalArgumentException();
				}
				try
				{
					return Activator.CreateInstance(type);
				}
				catch (TargetInvocationException x)
				{
					throw new global::java.lang.reflect.InvocationTargetException(global::ikvm.runtime.Util.mapException(x.InnerException));
				}
#endif
			}

			internal static bool IsSuitable(MethodWrapper mw)
			{
				MethodBase mb = mw.GetMethod();
				return mb != null
					&& mb.IsConstructor
					&& mb.IsPublic
					&& mb.DeclaringType.IsPublic
					&& mb.DeclaringType == mw.DeclaringType.TypeAsBaseType
					&& mb.GetParameters().Length == 0;
			}
		}

		private abstract class FieldAccessorImplBase : srFieldAccessor, IReflectionException
		{
			protected static readonly ushort inflationThreshold = 15;
			protected readonly FieldWrapper fw;
			protected readonly bool isFinal;
			protected ushort numInvocations;

			static FieldAccessorImplBase()
			{
				string str = jlSystem.getProperty("ikvm.reflect.field.inflationThreshold");
				int value;
				if (str != null && int.TryParse(str, out value))
				{
					if (value >= ushort.MinValue && value <= ushort.MaxValue)
					{
						inflationThreshold = (ushort)value;
					}
				}
			}

			private FieldAccessorImplBase(FieldWrapper fw, bool overrideAccessCheck)
			{
				this.fw = fw;
				isFinal = (!overrideAccessCheck || fw.IsStatic) && fw.IsFinal;
			}

			private string GetQualifiedFieldName()
			{
				return fw.DeclaringType.Name + "." + fw.Name;
			}

			private string GetFieldTypeName()
			{
				return fw.FieldTypeWrapper.ClassObject.getName();
			}

			public jlIllegalArgumentException GetIllegalArgumentException(object obj)
			{
				// LAME like JDK 6 we return the wrong exception message (talking about setting the field, instead of getting)
				return SetIllegalArgumentException(obj);
			}

			public jlIllegalArgumentException SetIllegalArgumentException(object obj)
			{
				// LAME like JDK 6 we return the wrong exception message (when obj is the object, instead of the value)
				return SetIllegalArgumentException(obj != null ? irUtil.getClassFromObject(obj).getName() : "", "");
			}

			private jlIllegalArgumentException SetIllegalArgumentException(string attemptedType, string attemptedValue)
			{
				return new jlIllegalArgumentException(GetSetMessage(attemptedType, attemptedValue));
			}

			protected jlIllegalAccessException FinalFieldIllegalAccessException(object obj)
			{
				return FinalFieldIllegalAccessException(obj != null ? irUtil.getClassFromObject(obj).getName() : "", "");
			}

			private jlIllegalAccessException FinalFieldIllegalAccessException(string attemptedType, string attemptedValue)
			{
				return new jlIllegalAccessException(GetSetMessage(attemptedType, attemptedValue));
			}

			private jlIllegalArgumentException GetIllegalArgumentException(string type)
			{
				return new jlIllegalArgumentException("Attempt to get " + GetFieldTypeName() + " field \"" + GetQualifiedFieldName() + "\" with illegal data type conversion to " + type);
			}

			// this message comes from sun.reflect.UnsafeFieldAccessorImpl
			private string GetSetMessage(String attemptedType, String attemptedValue)
			{
				String err = "Can not set";
				if (fw.IsStatic)
					err += " static";
				if (isFinal)
					err += " final";
				err += " " + GetFieldTypeName() + " field " + GetQualifiedFieldName() + " to ";
				if (attemptedValue.Length > 0)
				{
					err += "(" + attemptedType + ")" + attemptedValue;
				}
				else
				{
					if (attemptedType.Length > 0)
						err += attemptedType;
					else
						err += "null value";
				}
				return err;
			}

			public virtual bool getBoolean(object obj)
			{
				throw GetIllegalArgumentException("boolean");
			}

			public virtual byte getByte(object obj)
			{
				throw GetIllegalArgumentException("byte");
			}

			public virtual char getChar(object obj)
			{
				throw GetIllegalArgumentException("char");
			}

			public virtual short getShort(object obj)
			{
				throw GetIllegalArgumentException("short");
			}

			public virtual int getInt(object obj)
			{
				throw GetIllegalArgumentException("int");
			}

			public virtual long getLong(object obj)
			{
				throw GetIllegalArgumentException("long");
			}

			public virtual float getFloat(object obj)
			{
				throw GetIllegalArgumentException("float");
			}

			public virtual double getDouble(object obj)
			{
				throw GetIllegalArgumentException("double");
			}

			public virtual void setBoolean(object obj, bool z)
			{
				throw SetIllegalArgumentException("boolean", jlBoolean.toString(z));
			}

			public virtual void setByte(object obj, byte b)
			{
				throw SetIllegalArgumentException("byte", jlByte.toString(b));
			}

			public virtual void setChar(object obj, char c)
			{
				throw SetIllegalArgumentException("char", jlCharacter.toString(c));
			}

			public virtual void setShort(object obj, short s)
			{
				throw SetIllegalArgumentException("short", jlShort.toString(s));
			}

			public virtual void setInt(object obj, int i)
			{
				throw SetIllegalArgumentException("int", jlInteger.toString(i));
			}

			public virtual void setLong(object obj, long l)
			{
				throw SetIllegalArgumentException("long", jlLong.toString(l));
			}

			public virtual void setFloat(object obj, float f)
			{
				throw SetIllegalArgumentException("float", jlFloat.toString(f));
			}

			public virtual void setDouble(object obj, double d)
			{
				throw SetIllegalArgumentException("double", jlDouble.toString(d));
			}

			public abstract object get(object obj);
			public abstract void set(object obj, object value);

			private abstract class FieldAccessor<T> : FieldAccessorImplBase
			{
				protected delegate void Setter(object obj, T value, FieldAccessor<T> acc);
				protected delegate T Getter(object obj, FieldAccessor<T> acc);
				private static readonly Setter initialSetter = lazySet;
				private static readonly Getter initialGetter = lazyGet;
				protected Setter setter = initialSetter;
				protected Getter getter = initialGetter;

				internal FieldAccessor(FieldWrapper fw, bool overrideAccessCheck)
					: base(fw, overrideAccessCheck)
				{
					if (!IsSlowPathCompatible(fw))
					{
						// prevent slow path
						numInvocations = inflationThreshold;
					}
				}

				private bool IsSpecialType(TypeWrapper tw)
				{
					return tw.IsUnloadable
						|| tw.IsNonPrimitiveValueType
						|| tw.IsGhost
						|| tw.IsFakeNestedType;
				}

				private bool IsSlowPathCompatible(FieldWrapper fw)
				{
					if (IsSpecialType(fw.DeclaringType) || IsSpecialType(fw.FieldTypeWrapper) || fw.DeclaringType.IsRemapped)
					{
						return false;
					}
					fw.Link();
					return fw.GetField() != null;
				}

				private static T lazyGet(object obj, FieldAccessor<T> acc)
				{
					return acc.lazyGet(obj);
				}

				private static void lazySet(object obj, T value, FieldAccessor<T> acc)
				{
					acc.lazySet(obj, value);
				}

				private T lazyGet(object obj)
				{
					if (numInvocations < inflationThreshold)
					{
						if (fw.IsStatic)
						{
							obj = null;
						}
						else if (obj == null)
						{
#if !FIRST_PASS
							throw new global::java.lang.NullPointerException();
#endif
						}
						else if (!fw.DeclaringType.IsInstance(obj))
						{
							throw GetIllegalArgumentException(obj);
						}
						if (numInvocations == 0)
						{
							fw.DeclaringType.RunClassInit();
							fw.DeclaringType.Finish();
							fw.ResolveField();
						}
						numInvocations++;
						return (T)fw.GetField().GetValue(obj);
					}
					else
					{
						// FXBUG it appears that a ldsfld/stsfld in a DynamicMethod doesn't trigger the class constructor
						// and if we didn't use the slow path, we haven't yet initialized the class
						fw.DeclaringType.RunClassInit();
						getter = (Getter)GenerateFastGetter(typeof(Getter), typeof(T), fw);
						return getter(obj, this);
					}
				}

				private void lazySet(object obj, T value)
				{
					if (isFinal)
					{
						// for some reason Java runs class initialization before checking if the field is final
						fw.DeclaringType.RunClassInit();
						throw FinalFieldIllegalAccessException(JavaBox(value));
					}
					if (numInvocations < inflationThreshold)
					{
						if (fw.IsStatic)
						{
							obj = null;
						}
						else if (obj == null)
						{
#if !FIRST_PASS
							throw new global::java.lang.NullPointerException();
#endif
						}
						else if (!fw.DeclaringType.IsInstance(obj))
						{
							throw SetIllegalArgumentException(obj);
						}
						CheckValue(value);
						if (numInvocations == 0)
						{
							fw.DeclaringType.RunClassInit();
							fw.DeclaringType.Finish();
							fw.ResolveField();
						}
						numInvocations++;
						fw.GetField().SetValue(obj, value);
					}
					else
					{
						// FXBUG it appears that a ldsfld/stsfld in a DynamicMethod doesn't trigger the class constructor
						// and if we didn't use the slow path, we haven't yet initialized the class
						fw.DeclaringType.RunClassInit();
						setter = (Setter)GenerateFastSetter(typeof(Setter), typeof(T), fw);
						setter(obj, value, this);
					}
				}

				protected virtual void CheckValue(T value)
				{
				}

				protected abstract object JavaBox(T value);
			}

			private sealed class ByteField : FieldAccessor<byte>
			{
				internal ByteField(FieldWrapper field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public sealed override short getShort(object obj)
				{
					return (sbyte)getByte(obj);
				}

				public sealed override int getInt(object obj)
				{
					return (sbyte)getByte(obj);
				}

				public sealed override long getLong(object obj)
				{
					return (sbyte)getByte(obj);
				}

				public sealed override float getFloat(object obj)
				{
					return (sbyte)getByte(obj);
				}

				public sealed override double getDouble(object obj)
				{
					return (sbyte)getByte(obj);
				}

				public sealed override object get(object obj)
				{
					return jlByte.valueOf(getByte(obj));
				}

				public sealed override void set(object obj, object val)
				{
					if (!(val is jlByte))
					{
						throw SetIllegalArgumentException(val);
					}
					setByte(obj, ((jlByte)val).byteValue());
				}

				public sealed override byte getByte(object obj)
				{
					try
					{
						return getter(obj, this);
					}
					catch (FieldAccessException x)
					{
						throw new jlIllegalAccessException().initCause(x);
					}
				}

				public sealed override void setByte(object obj, byte value)
				{
					try
					{
						setter(obj, value, this);
					}
					catch (FieldAccessException x)
					{
						throw new jlIllegalAccessException().initCause(x);
					}
				}

				protected sealed override object JavaBox(byte value)
				{
					return jlByte.valueOf(value);
				}
			}

			private sealed class BooleanField : FieldAccessor<bool>
			{
				internal BooleanField(FieldWrapper field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public sealed override object get(object obj)
				{
					return jlBoolean.valueOf(getBoolean(obj));
				}

				public sealed override void set(object obj, object val)
				{
					if (!(val is jlBoolean))
					{
						throw SetIllegalArgumentException(val);
					}
					setBoolean(obj, ((jlBoolean)val).booleanValue());
				}

				public sealed override bool getBoolean(object obj)
				{
					try
					{
						return getter(obj, this);
					}
					catch (FieldAccessException x)
					{
						throw new jlIllegalAccessException().initCause(x);
					}
				}

				public sealed override void setBoolean(object obj, bool value)
				{
					try
					{
						setter(obj, value, this);
					}
					catch (FieldAccessException x)
					{
						throw new jlIllegalAccessException().initCause(x);
					}
				}

				protected sealed override object JavaBox(bool value)
				{
					return jlBoolean.valueOf(value);
				}
			}

			private sealed class CharField : FieldAccessor<char>
			{
				internal CharField(FieldWrapper field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public sealed override int getInt(object obj)
				{
					return getChar(obj);
				}

				public sealed override long getLong(object obj)
				{
					return getChar(obj);
				}

				public sealed override float getFloat(object obj)
				{
					return getChar(obj);
				}

				public sealed override double getDouble(object obj)
				{
					return getChar(obj);
				}

				public sealed override object get(object obj)
				{
					return jlCharacter.valueOf(getChar(obj));
				}

				public sealed override void set(object obj, object val)
				{
					if (val is jlCharacter)
						setChar(obj, ((jlCharacter)val).charValue());
					else
						throw SetIllegalArgumentException(val);
				}

				public sealed override char getChar(object obj)
				{
					try
					{
						return getter(obj, this);
					}
					catch (FieldAccessException x)
					{
						throw new jlIllegalAccessException().initCause(x);
					}
				}

				public sealed override void setChar(object obj, char value)
				{
					try
					{
						setter(obj, value, this);
					}
					catch (FieldAccessException x)
					{
						throw new jlIllegalAccessException().initCause(x);
					}
				}

				protected sealed override object JavaBox(char value)
				{
					return jlCharacter.valueOf(value);
				}
			}

			private sealed class ShortField : FieldAccessor<short>
			{
				internal ShortField(FieldWrapper field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public sealed override int getInt(object obj)
				{
					return getShort(obj);
				}

				public sealed override long getLong(object obj)
				{
					return getShort(obj);
				}

				public sealed override float getFloat(object obj)
				{
					return getShort(obj);
				}

				public sealed override double getDouble(object obj)
				{
					return getShort(obj);
				}

				public sealed override object get(object obj)
				{
					return jlShort.valueOf(getShort(obj));
				}

				public sealed override void set(object obj, object val)
				{
					if (val is jlByte
						|| val is jlShort)
						setShort(obj, ((jlNumber)val).shortValue());
					else
						throw SetIllegalArgumentException(val);
				}

				public sealed override void setByte(object obj, byte b)
				{
					setShort(obj, (sbyte)b);
				}

				public sealed override short getShort(object obj)
				{
					try
					{
						return getter(obj, this);
					}
					catch (FieldAccessException x)
					{
						throw new jlIllegalAccessException().initCause(x);
					}
				}

				public sealed override void setShort(object obj, short value)
				{
					try
					{
						setter(obj, value, this);
					}
					catch (FieldAccessException x)
					{
						throw new jlIllegalAccessException().initCause(x);
					}
				}

				protected sealed override object JavaBox(short value)
				{
					return jlShort.valueOf(value);
				}
			}

			private sealed class IntField : FieldAccessor<int>
			{
				internal IntField(FieldWrapper field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public sealed override long getLong(object obj)
				{
					return getInt(obj);
				}

				public sealed override float getFloat(object obj)
				{
					return getInt(obj);
				}

				public sealed override double getDouble(object obj)
				{
					return getInt(obj);
				}

				public sealed override object get(object obj)
				{
					return jlInteger.valueOf(getInt(obj));
				}

				public sealed override void set(object obj, object val)
				{
					if (val is jlByte
						|| val is jlShort
						|| val is jlInteger)
						setInt(obj, ((jlNumber)val).intValue());
					else if (val is jlCharacter)
						setInt(obj, ((jlCharacter)val).charValue());
					else
						throw SetIllegalArgumentException(val);
				}

				public sealed override void setByte(object obj, byte b)
				{
					setInt(obj, (sbyte)b);
				}

				public sealed override void setChar(object obj, char c)
				{
					setInt(obj, c);
				}

				public sealed override void setShort(object obj, short s)
				{
					setInt(obj, s);
				}

				public sealed override int getInt(object obj)
				{
					try
					{
						return getter(obj, this);
					}
					catch (FieldAccessException x)
					{
						throw new jlIllegalAccessException().initCause(x);
					}
				}

				public sealed override void setInt(object obj, int value)
				{
					try
					{
						setter(obj, value, this);
					}
					catch (FieldAccessException x)
					{
						throw new jlIllegalAccessException().initCause(x);
					}
				}

				protected sealed override object JavaBox(int value)
				{
					return jlInteger.valueOf(value);
				}
			}

			private sealed class FloatField : FieldAccessor<float>
			{
				internal FloatField(FieldWrapper field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public sealed override double getDouble(object obj)
				{
					return getFloat(obj);
				}

				public sealed override object get(object obj)
				{
					return jlFloat.valueOf(getFloat(obj));
				}

				public sealed override void set(object obj, object val)
				{
					if (val is jlFloat
						|| val is jlByte
						|| val is jlShort
						|| val is jlInteger
						|| val is jlLong)
						setFloat(obj, ((jlNumber)val).floatValue());
					else if (val is jlCharacter)
						setFloat(obj, ((jlCharacter)val).charValue());
					else
						throw SetIllegalArgumentException(val);
				}

				public sealed override void setByte(object obj, byte b)
				{
					setFloat(obj, (sbyte)b);
				}

				public sealed override void setChar(object obj, char c)
				{
					setFloat(obj, c);
				}

				public sealed override void setShort(object obj, short s)
				{
					setFloat(obj, s);
				}

				public sealed override void setInt(object obj, int i)
				{
					setFloat(obj, i);
				}

				public sealed override void setLong(object obj, long l)
				{
					setFloat(obj, l);
				}

				public sealed override float getFloat(object obj)
				{
					try
					{
						return getter(obj, this);
					}
					catch (FieldAccessException x)
					{
						throw new jlIllegalAccessException().initCause(x);
					}
				}

				public sealed override void setFloat(object obj, float value)
				{
					try
					{
						setter(obj, value, this);
					}
					catch (FieldAccessException x)
					{
						throw new jlIllegalAccessException().initCause(x);
					}
				}

				protected sealed override object JavaBox(float value)
				{
					return jlFloat.valueOf(value);
				}
			}

			private sealed class LongField : FieldAccessor<long>
			{
				internal LongField(FieldWrapper field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public sealed override float getFloat(object obj)
				{
					return getLong(obj);
				}

				public sealed override double getDouble(object obj)
				{
					return getLong(obj);
				}

				public sealed override object get(object obj)
				{
					return jlLong.valueOf(getLong(obj));
				}

				public sealed override void set(object obj, object val)
				{
					if (val is jlLong
						|| val is jlByte
						|| val is jlShort
						|| val is jlInteger)
						setLong(obj, ((jlNumber)val).longValue());
					else if (val is jlCharacter)
						setLong(obj, ((jlCharacter)val).charValue());
					else
						throw SetIllegalArgumentException(val);
				}

				public sealed override void setByte(object obj, byte b)
				{
					setLong(obj, (sbyte)b);
				}

				public sealed override void setChar(object obj, char c)
				{
					setLong(obj, c);
				}

				public sealed override void setShort(object obj, short s)
				{
					setLong(obj, s);
				}

				public sealed override void setInt(object obj, int i)
				{
					setLong(obj, i);
				}

				public sealed override long getLong(object obj)
				{
					try
					{
						return getter(obj, this);
					}
					catch (FieldAccessException x)
					{
						throw new jlIllegalAccessException().initCause(x);
					}
				}

				public sealed override void setLong(object obj, long value)
				{
					try
					{
						setter(obj, value, this);
					}
					catch (FieldAccessException x)
					{
						throw new jlIllegalAccessException().initCause(x);
					}
				}

				protected sealed override object JavaBox(long value)
				{
					return jlLong.valueOf(value);
				}
			}

			private sealed class DoubleField : FieldAccessor<double>
			{
				internal DoubleField(FieldWrapper field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public sealed override object get(object obj)
				{
					return jlDouble.valueOf(getDouble(obj));
				}

				public sealed override void set(object obj, object val)
				{
					if (val is jlDouble
						|| val is jlFloat
						|| val is jlByte
						|| val is jlShort
						|| val is jlInteger
						|| val is jlLong)
						setDouble(obj, ((jlNumber)val).doubleValue());
					else if (val is jlCharacter)
						setDouble(obj, ((jlCharacter)val).charValue());
					else
						throw SetIllegalArgumentException(val);
				}

				public sealed override void setByte(object obj, byte b)
				{
					setDouble(obj, (sbyte)b);
				}

				public sealed override void setChar(object obj, char c)
				{
					setDouble(obj, c);
				}

				public sealed override void setShort(object obj, short s)
				{
					setDouble(obj, s);
				}

				public sealed override void setInt(object obj, int i)
				{
					setDouble(obj, i);
				}

				public sealed override void setLong(object obj, long l)
				{
					setDouble(obj, l);
				}

				public sealed override void setFloat(object obj, float f)
				{
					setDouble(obj, f);
				}

				public sealed override double getDouble(object obj)
				{
					try
					{
						return getter(obj, this);
					}
					catch (FieldAccessException x)
					{
						throw new jlIllegalAccessException().initCause(x);
					}
				}

				public sealed override void setDouble(object obj, double value)
				{
					try
					{
						setter(obj, value, this);
					}
					catch (FieldAccessException x)
					{
						throw new jlIllegalAccessException().initCause(x);
					}
				}

				protected sealed override object JavaBox(double value)
				{
					return jlDouble.valueOf(value);
				}
			}

			private sealed class ObjectField : FieldAccessor<object>
			{
				internal ObjectField(FieldWrapper field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				protected sealed override void CheckValue(object value)
				{
					if (value != null && !fw.FieldTypeWrapper.IsInstance(value))
					{
						throw SetIllegalArgumentException(value);
					}
				}

				public sealed override object get(object obj)
				{
					try
					{
						return getter(obj, this);
					}
					catch (FieldAccessException x)
					{
						throw new jlIllegalAccessException().initCause(x);
					}
				}

				public sealed override void set(object obj, object value)
				{
					try
					{
						setter(obj, value, this);
					}
					catch (FieldAccessException x)
					{
						throw new jlIllegalAccessException().initCause(x);
					}
				}

				protected sealed override object JavaBox(object value)
				{
					return value;
				}
			}

			private Delegate GenerateFastGetter(Type delegateType, Type fieldType, FieldWrapper fw)
			{
				TypeWrapper fieldTypeWrapper;
				try
				{
					fieldTypeWrapper = fw.FieldTypeWrapper.EnsureLoadable(fw.DeclaringType.GetClassLoader());
					fieldTypeWrapper.Finish();
					fw.DeclaringType.Finish();
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				fw.ResolveField();
				DynamicMethod dm = DynamicMethodUtils.Create("__<Getter>", fw.DeclaringType.TypeAsBaseType, !fw.IsPublic || !fw.DeclaringType.IsPublic, fieldType, new Type[] { typeof(IReflectionException), typeof(object), typeof(object) });
				CodeEmitter ilgen = CodeEmitter.Create(dm);
				if (fw.IsStatic)
				{
					fw.EmitGet(ilgen);
					fieldTypeWrapper.EmitConvSignatureTypeToStackType(ilgen);
				}
				else
				{
					ilgen.BeginExceptionBlock();
					ilgen.Emit(OpCodes.Ldarg_1);
					ilgen.Emit(OpCodes.Castclass, fw.DeclaringType.TypeAsBaseType);
					fw.EmitGet(ilgen);
					fieldTypeWrapper.EmitConvSignatureTypeToStackType(ilgen);
					CodeEmitterLocal local = ilgen.DeclareLocal(fieldType);
					ilgen.Emit(OpCodes.Stloc, local);
					CodeEmitterLabel label = ilgen.DefineLabel();
					ilgen.EmitLeave(label);
					ilgen.BeginCatchBlock(typeof(InvalidCastException));
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Ldarg_1);
					ilgen.Emit(OpCodes.Callvirt, typeof(IReflectionException).GetMethod("GetIllegalArgumentException"));
					ilgen.Emit(OpCodes.Throw);
					ilgen.EndExceptionBlock();
					ilgen.MarkLabel(label);
					ilgen.Emit(OpCodes.Ldloc, local);
				}
				ilgen.Emit(OpCodes.Ret);
				ilgen.DoEmit();
				return dm.CreateDelegate(delegateType, this);
			}

			private Delegate GenerateFastSetter(Type delegateType, Type fieldType, FieldWrapper fw)
			{
				TypeWrapper fieldTypeWrapper;
				try
				{
					fieldTypeWrapper = fw.FieldTypeWrapper.EnsureLoadable(fw.DeclaringType.GetClassLoader());
					fieldTypeWrapper.Finish();
					fw.DeclaringType.Finish();
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				fw.ResolveField();
				DynamicMethod dm = DynamicMethodUtils.Create("__<Setter>", fw.DeclaringType.TypeAsBaseType, !fw.IsPublic || !fw.DeclaringType.IsPublic, null, new Type[] { typeof(IReflectionException), typeof(object), fieldType, typeof(object) });
				CodeEmitter ilgen = CodeEmitter.Create(dm);
				if (fw.IsStatic)
				{
					if (fieldType == typeof(object))
					{
						ilgen.BeginExceptionBlock();
						ilgen.Emit(OpCodes.Ldarg_2);
						fieldTypeWrapper.EmitCheckcast(ilgen);
						fieldTypeWrapper.EmitConvStackTypeToSignatureType(ilgen, null);
						fw.EmitSet(ilgen);
						CodeEmitterLabel label = ilgen.DefineLabel();
						ilgen.EmitLeave(label);
						ilgen.BeginCatchBlock(typeof(InvalidCastException));
						ilgen.Emit(OpCodes.Ldarg_0);
						ilgen.Emit(OpCodes.Ldarg_1);
						ilgen.Emit(OpCodes.Callvirt, typeof(IReflectionException).GetMethod("SetIllegalArgumentException"));
						ilgen.Emit(OpCodes.Throw);
						ilgen.EndExceptionBlock();
						ilgen.MarkLabel(label);
					}
					else
					{
						ilgen.Emit(OpCodes.Ldarg_2);
						fw.EmitSet(ilgen);
					}
				}
				else
				{
					ilgen.BeginExceptionBlock();
					ilgen.Emit(OpCodes.Ldarg_1);
					ilgen.Emit(OpCodes.Castclass, fw.DeclaringType.TypeAsBaseType);
					ilgen.Emit(OpCodes.Ldarg_2);
					if (fieldType == typeof(object))
					{
						fieldTypeWrapper.EmitCheckcast(ilgen);
					}
					fieldTypeWrapper.EmitConvStackTypeToSignatureType(ilgen, null);
					fw.EmitSet(ilgen);
					CodeEmitterLabel label = ilgen.DefineLabel();
					ilgen.EmitLeave(label);
					ilgen.BeginCatchBlock(typeof(InvalidCastException));
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Ldarg_1);
					ilgen.Emit(OpCodes.Callvirt, typeof(IReflectionException).GetMethod("SetIllegalArgumentException"));
					ilgen.Emit(OpCodes.Throw);
					ilgen.EndExceptionBlock();
					ilgen.MarkLabel(label);
				}
				ilgen.Emit(OpCodes.Ret);
				ilgen.DoEmit();
				return dm.CreateDelegate(delegateType, this);
			}

			internal static FieldAccessorImplBase Create(FieldWrapper field, bool overrideAccessCheck)
			{
				TypeWrapper type = field.FieldTypeWrapper;
				if (type.IsPrimitive)
				{
					if (type == PrimitiveTypeWrapper.BYTE)
					{
						return new ByteField(field, overrideAccessCheck);
					}
					if (type == PrimitiveTypeWrapper.BOOLEAN)
					{
						return new BooleanField(field, overrideAccessCheck);
					}
					if (type == PrimitiveTypeWrapper.CHAR)
					{
						return new CharField(field, overrideAccessCheck);
					}
					if (type == PrimitiveTypeWrapper.SHORT)
					{
						return new ShortField(field, overrideAccessCheck);
					}
					if (type == PrimitiveTypeWrapper.INT)
					{
						return new IntField(field, overrideAccessCheck);
					}
					if (type == PrimitiveTypeWrapper.FLOAT)
					{
						return new FloatField(field, overrideAccessCheck);
					}
					if (type == PrimitiveTypeWrapper.LONG)
					{
						return new LongField(field, overrideAccessCheck);
					}
					if (type == PrimitiveTypeWrapper.DOUBLE)
					{
						return new DoubleField(field, overrideAccessCheck);
					}
					throw new InvalidOperationException("field type: " + type);
				}
				else
				{
					return new ObjectField(field, overrideAccessCheck);
				}
			}
		}
#endif

		public static object newFieldAccessor(object thisFactory, object field, bool overrideAccessCheck)
		{
#if FIRST_PASS
			return null;
#else
			return FieldAccessorImplBase.Create(FieldWrapper.FromField(field), overrideAccessCheck);
#endif
		}

#if !FIRST_PASS
		internal static global::sun.reflect.FieldAccessor NewFieldAccessorJNI(FieldWrapper field)
		{
			return FieldAccessorImplBase.Create(field, true);
		}
#endif

		public static object newMethodAccessor(object thisFactory, object method)
		{
#if FIRST_PASS
			return null;
#else
			jlrMethod m = (jlrMethod)method;
			MethodWrapper mw = MethodWrapper.FromMethodOrConstructor(method);
			if (mw is ICustomInvoke)
			{
				return new MethodAccessorImpl(m);
			}
			else
			{
				return new FastMethodAccessorImpl(m, false);
			}
#endif
		}

		public static object newConstructorAccessor0(object thisFactory, object constructor)
		{
#if FIRST_PASS
			return null;
#else
			jlrConstructor cons = (jlrConstructor)constructor;
			MethodWrapper mw = MethodWrapper.FromMethodOrConstructor(constructor);
			if (ActivatorConstructorAccessor.IsSuitable(mw))
			{
				// we special case public default constructors, because in that case using Activator.CreateInstance()
				// is almost as fast as FastConstructorAccessorImpl, but it saves us significantly in working set and
				// startup time (because often during startup a sun.nio.cs.* encoder is instantiated using reflection)
				return new ActivatorConstructorAccessor(mw);
			}
			else
			{
				return new FastConstructorAccessorImpl(cons);
			}
#endif
		}

		public static object newConstructorAccessorForSerialization(jlClass classToInstantiate, jlrConstructor constructorToCall)
		{
#if FIRST_PASS
			return null;
#else
			try
			{
				return new FastSerializationConstructorAccessorImpl(constructorToCall, classToInstantiate);
			}
			catch (System.Security.SecurityException x)
			{
				throw new global::java.lang.SecurityException(x.Message, irUtil.mapException(x));
			}
#endif
		}
	}

	static class ConstantPool
	{
		public static int getSize0(object thisConstantPool, object constantPoolOop)
		{
			throw new NotImplementedException();
		}

		public static object getClassAt0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static object getClassAtIfLoaded0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static object getMethodAt0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static object getMethodAtIfLoaded0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static object getFieldAt0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static object getFieldAtIfLoaded0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static string[] getMemberRefInfoAt0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static int getIntAt0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static long getLongAt0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static float getFloatAt0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static double getDoubleAt0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static string getStringAt0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static string getUTF8At0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}
	}
}

namespace IKVM.NativeCode.sun.rmi.server
{
	static class MarshalInputStream
	{
		public static object latestUserDefinedLoader()
		{
			return Java_java_io_ObjectInputStream.latestUserDefinedLoader();
		}
	}
}

namespace IKVM.NativeCode.sun.security.provider
{
	static class NativeSeedGenerator
	{
		public static bool nativeGenerateSeed(byte[] result)
		{
			try
			{
				System.Security.Cryptography.RNGCryptoServiceProvider csp = new System.Security.Cryptography.RNGCryptoServiceProvider();
				csp.GetBytes(result);
				return true;
			}
			catch (System.Security.Cryptography.CryptographicException)
			{
				return false;
			}
		}
	}
}

namespace IKVM.NativeCode.com.sun.java.util.jar.pack
{
	static class NativeUnpack
	{
		public static void initIDs()
		{
		}

		public static long start(object thisNativeUnpack, object buf, long offset)
		{
			throw new NotImplementedException();
		}

		public static bool getNextFile(object thisNativeUnpack, object[] parts)
		{
			throw new NotImplementedException();
		}

		public static object getUnusedInput(object thisNativeUnpack)
		{
			throw new NotImplementedException();
		}

		public static long finish(object thisNativeUnpack)
		{
			throw new NotImplementedException();
		}

		public static bool setOption(object thisNativeUnpack, string opt, string value)
		{
			throw new NotImplementedException();
		}

		public static string getOption(object thisNativeUnpack, string opt)
		{
			throw new NotImplementedException();
		}
	}
}

namespace IKVM.NativeCode.com.sun.security.auth.module
{
	using System.Security.Principal;

	static class NTSystem
	{
		public static void getCurrent(object thisObj, bool debug)
		{
			WindowsIdentity id = WindowsIdentity.GetCurrent();
			string[] name = id.Name.Split('\\');
			SetField(thisObj, "userName", name[1]);
			SetField(thisObj, "domain", name[0]);
			SetField(thisObj, "domainSID", id.User.AccountDomainSid.Value);
			SetField(thisObj, "userSID", id.User.Value);
			string[] groups = new string[id.Groups.Count];
			for (int i = 0; i < groups.Length; i++)
			{
				groups[i] = id.Groups[i].Value;
			}
			SetField(thisObj, "groupIDs", groups);
			// HACK it turns out that Groups[0] is the primary group, but AFAIK this is not documented anywhere
			SetField(thisObj, "primaryGroupID", groups[0]);
		}

		private static void SetField(object thisObj, string field, object value)
		{
			thisObj.GetType().GetField(field, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(thisObj, value);
		}

		public static long getImpersonationToken0(object thisObj)
		{
			return WindowsIdentity.GetCurrent().Token.ToInt64();
		}
	}

	static class SolarisSystem
	{
		public static void getSolarisInfo(object thisObj)
		{
			throw new NotImplementedException();
		}
	}

	static class UnixSystem
	{
		public static void getUnixInfo(object thisObj)
		{
			throw new NotImplementedException();
		}
	}
}

namespace IKVM.NativeCode.com.sun.media.sound
{
	static class JDK13Services
	{
		public static string getDefaultProviderClassName(object deviceClass)
		{
			return null;
		}

		public static string getDefaultInstanceName(object deviceClass)
		{
			return null;
		}

		public static object getProviders(object providerClass)
		{
#if FIRST_PASS
			return null;
#else
			return new global::java.util.ArrayList();
#endif
		}
	}
}

namespace IKVM.NativeCode.java.awt
{
	static class AWTEvent
	{
		public static void initIDs() { }
		public static void nativeSetSource(object thisObj, object peer){ }
	}
	
	static class Button
	{
		public static void initIDs() { }
	}
	
	static class Checkbox
	{
		public static void initIDs() { }
	}
	
	static class CheckboxMenuItem
	{
		public static void initIDs() { }
	}
	
	static class Color
	{
		public static void initIDs() { }
	}
	
	static class Component
	{
		public static void initIDs() { }
	}
	
	static class Container
	{
		public static void initIDs() { }
	}
	
	static class Cursor
	{
		public static void initIDs() { }
		public static void finalizeImpl(Int64 pData){ }
	}
	
	static class Dialog
	{
		public static void initIDs() { }
	}
	
	static class Dimension
	{
		public static void initIDs() { }
	}
	
	static class Event
	{
		public static void initIDs() { }
	}
	
	static class FileDialog
	{
		public static void initIDs() { }
	}
	
	static class Frame
	{
		public static void initIDs() { }
	}
	
	static class FontMetrics
	{
		public static void initIDs() { }
	}
	
	static class Insets
	{
		public static void initIDs() { }
	}
	
	static class KeyboardFocusManager
	{
		public static void initIDs() { }
	}
	
	static class Label
	{
		public static void initIDs() { }
	}
	
	static class Menu
	{
		public static void initIDs() { }
	}
	
	static class MenuBar
	{
		public static void initIDs() { }
	}
	
	static class MenuComponent
	{
		public static void initIDs() { }
	}
	
	static class MenuItem
	{
		public static void initIDs() { }
	}
	
	static class Rectangle
	{
		public static void initIDs() { }
	}
	
	static class Scrollbar
	{
		public static void initIDs() { }
	}
	
	static class ScrollPane
	{
		public static void initIDs() { }
	}
	
	static class ScrollPaneAdjustable
	{
		public static void initIDs() { }
	}
	
	static class SplashScreen
	{
	    public static void _update(long splashPtr, int[] data, int x, int y, int width, int height, int scanlineStride){}
		public static bool _isVisible(long splashPtr){return false;}
		public static object _getBounds(long splashPtr){return null;}
		public static long _getInstance(){return 0;}
		public static void _close(long splashPtr){}
		public static String _getImageFileName(long splashPtr){return null;}
		public static String _getImageJarName(long splashPtr){return null;}
		public static bool _setImageData(long splashPtr, byte[] data){return false;}
	}
	
	static class TextArea
	{
		public static void initIDs() { }
	}
	
	static class TextField
	{
		public static void initIDs() { }
	}
	
	static class Toolkit
	{
		public static void initIDs() { }
	}
	
	static class TrayIcon
	{
		public static void initIDs() { }
	}
	
	static class Window
	{
		public static void initIDs() { }
	}
}

namespace IKVM.NativeCode.java.awt.@event
{
	static class InputEvent
	{
		public static void initIDs() { }
	}

	static class MouseEvent
	{
		public static void initIDs() { }
	}

	static class KeyEvent
	{
		public static void initIDs() { }
	}
}

namespace IKVM.NativeCode.java.awt.image
{
	static class ColorModel
	{
		public static void initIDs() { }
	}

	static class ComponentSampleModel
	{
		public static void initIDs() { }
	}

	static class Kernel
	{
		public static void initIDs() { }
	}

	static class Raster
	{
		public static void initIDs() { }
	}

	static class SinglePixelPackedSampleModel
	{
		public static void initIDs() { }
	}

	static class SampleModel
	{
		public static void initIDs() { }
	}
}