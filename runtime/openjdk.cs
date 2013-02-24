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

		public static void addBootClassPathAssemby(Assembly asm)
		{
			ClassLoaderWrapper.GetBootstrapClassLoader().AddDelegate(global::IKVM.Internal.AssemblyClassLoader.FromAssembly(asm));
		}
	}
}

namespace IKVM.NativeCode.java
{
	namespace io
	{
		static class Console
		{
			public static string encoding()
			{
				int cp = 437;
				try
				{
					cp = global::System.Console.InputEncoding.CodePage;
				}
				catch
				{
				}
				if (cp >= 874 && cp <= 950)
				{
					return "ms" + cp;
				}
				return "cp" + cp;
			}

			private const int STD_INPUT_HANDLE = -10;
			private const int ENABLE_ECHO_INPUT = 0x0004;

			[System.Runtime.InteropServices.DllImport("kernel32")]
			private static extern IntPtr GetStdHandle(int nStdHandle);

			[System.Runtime.InteropServices.DllImport("kernel32")]
			private static extern int GetConsoleMode(IntPtr hConsoleHandle, out int lpMode);

			[System.Runtime.InteropServices.DllImport("kernel32")]
			private static extern int SetConsoleMode(IntPtr hConsoleHandle, int dwMode);

			public static bool echo(bool on)
			{
#if !FIRST_PASS
				// HACK the only way to get this to work is by p/invoking the Win32 APIs
				if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				{
					IntPtr hStdIn = GetStdHandle(STD_INPUT_HANDLE);
					if (hStdIn.ToInt64() == 0 || hStdIn.ToInt64() == -1)
					{
						throw new global::java.io.IOException("The handle is invalid");
					}
					int fdwMode;
					if (GetConsoleMode(hStdIn, out fdwMode) == 0)
					{
						throw new global::java.io.IOException("GetConsoleMode failed");
					}
					bool old = (fdwMode & ENABLE_ECHO_INPUT) != 0;
					if (on)
					{
						fdwMode |= ENABLE_ECHO_INPUT;
					}
					else
					{
						fdwMode &= ~ENABLE_ECHO_INPUT;
					}
					if (SetConsoleMode(hStdIn, fdwMode) == 0)
					{
						throw new global::java.io.IOException("SetConsoleMode failed");
					}
					return old;
				}
#endif
				return true;
			}

			public static bool istty()
			{
				// The JDK returns false here if stdin or stdout (not stderr) is redirected to a file
				// or if there is no console associated with the current process.
				// The best we can do is to look at the KeyAvailable property, which
				// will throw an InvalidOperationException if stdin is redirected or not available
				try
				{
					return global::System.Console.KeyAvailable || true;
				}
				catch (InvalidOperationException)
				{
					return false;
				}
			}
		}

		static class FileDescriptor
		{
			private static Converter<int, int> fsync;

			public static System.IO.Stream open(String name, System.IO.FileMode fileMode, System.IO.FileAccess fileAccess)
			{
				if (VirtualFileSystem.IsVirtualFS(name))
				{
					return VirtualFileSystem.Open(name, fileMode, fileAccess);
				}
				else if (fileMode == System.IO.FileMode.Append)
				{
					// this is the way to get atomic append behavior for all writes
					return new System.IO.FileStream(name, fileMode, System.Security.AccessControl.FileSystemRights.AppendData, System.IO.FileShare.ReadWrite, 1, System.IO.FileOptions.None);
				}
				else
				{
					return new System.IO.FileStream(name, fileMode, fileAccess, System.IO.FileShare.ReadWrite, 1, false);
				}
			}

			[System.Security.SecuritySafeCritical]
			public static bool flushPosix(System.IO.FileStream fs)
			{
				if (fsync == null)
				{
					ResolveFSync();
				}
				bool success = false;
				Microsoft.Win32.SafeHandles.SafeFileHandle handle = fs.SafeFileHandle;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					handle.DangerousAddRef(ref success);
					return fsync(handle.DangerousGetHandle().ToInt32()) == 0;
				}
				finally
				{
					if (success)
					{
						handle.DangerousRelease();
					}
				}
			}

			[System.Security.SecurityCritical]
			private static void ResolveFSync()
			{
				// we don't want a build time dependency on this Mono assembly, so we use reflection
				Type type = Type.GetType("Mono.Unix.Native.Syscall, Mono.Posix, Version=2.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");
				if (type != null)
				{
					fsync = (Converter<int, int>)Delegate.CreateDelegate(typeof(Converter<int, int>), type, "fsync", false, false);
				}
				if (fsync == null)
				{
					fsync = DummyFSync;
				}
			}

			private static int DummyFSync(int fd)
			{
				return 0;
			}
		}

		static class FileSystem
		{
			public static object getFileSystem()
			{
#if FIRST_PASS
				return null;
#else
				if (JVM.IsUnix)
				{
					return new global::java.io.UnixFileSystem();
				}
				else
				{
					return new global::java.io.Win32FileSystem();
				}
#endif
			}
		}

		static class ObjectInputStream
		{
			public static void bytesToFloats(byte[] src, int srcpos, float[] dst, int dstpos, int nfloats)
			{
				IKVM.Runtime.FloatConverter converter = new IKVM.Runtime.FloatConverter();
				for (int i = 0; i < nfloats; i++)
				{
					int v = src[srcpos++];
					v = (v << 8) | src[srcpos++];
					v = (v << 8) | src[srcpos++];
					v = (v << 8) | src[srcpos++];
					dst[dstpos++] = IKVM.Runtime.FloatConverter.ToFloat(v, ref converter);
				}
			}

			public static void bytesToDoubles(byte[] src, int srcpos, double[] dst, int dstpos, int ndoubles)
			{
				IKVM.Runtime.DoubleConverter converter = new IKVM.Runtime.DoubleConverter();
				for (int i = 0; i < ndoubles; i++)
				{
					long v = src[srcpos++];
					v = (v << 8) | src[srcpos++];
					v = (v << 8) | src[srcpos++];
					v = (v << 8) | src[srcpos++];
					v = (v << 8) | src[srcpos++];
					v = (v << 8) | src[srcpos++];
					v = (v << 8) | src[srcpos++];
					v = (v << 8) | src[srcpos++];
					dst[dstpos++] = IKVM.Runtime.DoubleConverter.ToDouble(v, ref converter);
				}
			}

			public static object latestUserDefinedLoader()
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
								object javaClassLoader = classLoader.GetJavaClassLoader();
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

		static class ObjectOutputStream
		{
			public static void floatsToBytes(float[] src, int srcpos, byte[] dst, int dstpos, int nfloats)
			{
				IKVM.Runtime.FloatConverter converter = new IKVM.Runtime.FloatConverter();
				for (int i = 0; i < nfloats; i++)
				{
					int v = IKVM.Runtime.FloatConverter.ToInt(src[srcpos++], ref converter);
					dst[dstpos++] = (byte)(v >> 24);
					dst[dstpos++] = (byte)(v >> 16);
					dst[dstpos++] = (byte)(v >> 8);
					dst[dstpos++] = (byte)(v >> 0);
				}
			}

			public static void doublesToBytes(double[] src, int srcpos, byte[] dst, int dstpos, int ndoubles)
			{
				IKVM.Runtime.DoubleConverter converter = new IKVM.Runtime.DoubleConverter();
				for (int i = 0; i < ndoubles; i++)
				{
					long v = IKVM.Runtime.DoubleConverter.ToLong(src[srcpos++], ref converter);
					dst[dstpos++] = (byte)(v >> 56);
					dst[dstpos++] = (byte)(v >> 48);
					dst[dstpos++] = (byte)(v >> 40);
					dst[dstpos++] = (byte)(v >> 32);
					dst[dstpos++] = (byte)(v >> 24);
					dst[dstpos++] = (byte)(v >> 16);
					dst[dstpos++] = (byte)(v >> 8);
					dst[dstpos++] = (byte)(v >> 0);
				}
			}
		}

		public static class IOHelpers
		{
			public static void WriteByte(byte[] buf, int offset, byte value)
			{
				buf[offset] = value;
			}

			public static void WriteBoolean(byte[] buf, int offset, bool value)
			{
				buf[offset] = value ? (byte)1 : (byte)0;
			}

			public static void WriteChar(byte[] buf, int offset, char value)
			{
				buf[offset + 0] = (byte)(value >> 8);
				buf[offset + 1] = (byte)(value >> 0);
			}

			public static void WriteShort(byte[] buf, int offset, short value)
			{
				buf[offset + 0] = (byte)(value >> 8);
				buf[offset + 1] = (byte)(value >> 0);
			}

			public static void WriteInt(byte[] buf, int offset, int value)
			{
				buf[offset + 0] = (byte)(value >> 24);
				buf[offset + 1] = (byte)(value >> 16);
				buf[offset + 2] = (byte)(value >> 8);
				buf[offset + 3] = (byte)(value >> 0);
			}

			public static void WriteFloat(byte[] buf, int offset, float value)
			{
#if !FIRST_PASS
				global::java.io.Bits.putFloat(buf, offset, value);
#endif
			}

			public static void WriteLong(byte[] buf, int offset, long value)
			{
				WriteInt(buf, offset, (int)(value >> 32));
				WriteInt(buf, offset + 4, (int)value);
			}

			public static void WriteDouble(byte[] buf, int offset, double value)
			{
#if !FIRST_PASS
				global::java.io.Bits.putDouble(buf, offset, value);
#endif
			}

			public static byte ReadByte(byte[] buf, int offset)
			{
				return buf[offset];
			}

			public static bool ReadBoolean(byte[] buf, int offset)
			{
				return buf[offset] != 0;
			}

			public static char ReadChar(byte[] buf, int offset)
			{
				return (char)((buf[offset] << 8) + buf[offset + 1]);
			}

			public static short ReadShort(byte[] buf, int offset)
			{
				return (short)((buf[offset] << 8) + buf[offset + 1]);
			}

			public static int ReadInt(byte[] buf, int offset)
			{
				return (buf[offset + 0] << 24)
					 + (buf[offset + 1] << 16)
					 + (buf[offset + 2] << 8)
					 + (buf[offset + 3] << 0);
			}

			public static float ReadFloat(byte[] buf, int offset)
			{
#if FIRST_PASS
				return 0;
#else
				return jlFloat.intBitsToFloat(ReadInt(buf, offset));
#endif
			}

			public static long ReadLong(byte[] buf, int offset)
			{
				long hi = (uint)ReadInt(buf, offset);
				long lo = (uint)ReadInt(buf, offset + 4);
				return lo + (hi << 32);
			}

			public static double ReadDouble(byte[] buf, int offset)
			{
#if FIRST_PASS
				return 0;
#else
				return jlDouble.longBitsToDouble(ReadLong(buf, offset));
#endif
			}
		}

		static class ObjectStreamClass
		{
			public static void initNative()
			{
			}

			public static bool isDynamicTypeWrapper(jlClass cl)
			{
				TypeWrapper wrapper = TypeWrapper.FromClass(cl);
				return !wrapper.IsFastClassLiteralSafe;
			}

			public static bool hasStaticInitializer(jlClass cl)
			{
				TypeWrapper wrapper = TypeWrapper.FromClass(cl);
				try
				{
					wrapper.Finish();
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				Type type = wrapper.TypeAsTBD;
				if (!type.IsArray && type.TypeInitializer != null)
				{
					wrapper.RunClassInit();
					return !AttributeHelper.IsHideFromJava(type.TypeInitializer);
				}
				return false;
			}

#if !FIRST_PASS
			private sealed class FastFieldReflector : iiFieldReflectorBase
			{
				private static readonly MethodInfo ReadByteMethod = typeof(IOHelpers).GetMethod("ReadByte");
				private static readonly MethodInfo ReadBooleanMethod = typeof(IOHelpers).GetMethod("ReadBoolean");
				private static readonly MethodInfo ReadCharMethod = typeof(IOHelpers).GetMethod("ReadChar");
				private static readonly MethodInfo ReadShortMethod = typeof(IOHelpers).GetMethod("ReadShort");
				private static readonly MethodInfo ReadIntMethod = typeof(IOHelpers).GetMethod("ReadInt");
				private static readonly MethodInfo ReadFloatMethod = typeof(IOHelpers).GetMethod("ReadFloat");
				private static readonly MethodInfo ReadLongMethod = typeof(IOHelpers).GetMethod("ReadLong");
				private static readonly MethodInfo ReadDoubleMethod = typeof(IOHelpers).GetMethod("ReadDouble");
				private static readonly MethodInfo WriteByteMethod = typeof(IOHelpers).GetMethod("WriteByte");
				private static readonly MethodInfo WriteBooleanMethod = typeof(IOHelpers).GetMethod("WriteBoolean");
				private static readonly MethodInfo WriteCharMethod = typeof(IOHelpers).GetMethod("WriteChar");
				private static readonly MethodInfo WriteShortMethod = typeof(IOHelpers).GetMethod("WriteShort");
				private static readonly MethodInfo WriteIntMethod = typeof(IOHelpers).GetMethod("WriteInt");
				private static readonly MethodInfo WriteFloatMethod = typeof(IOHelpers).GetMethod("WriteFloat");
				private static readonly MethodInfo WriteLongMethod = typeof(IOHelpers).GetMethod("WriteLong");
				private static readonly MethodInfo WriteDoubleMethod = typeof(IOHelpers).GetMethod("WriteDouble");
				private delegate void ObjFieldGetterSetter(object obj, object[] objarr);
				private delegate void PrimFieldGetterSetter(object obj, byte[] objarr);
				private static readonly ObjFieldGetterSetter objDummy = new ObjFieldGetterSetter(Dummy);
				private static readonly PrimFieldGetterSetter primDummy = new PrimFieldGetterSetter(Dummy);
				private jiObjectStreamField[] fields;
				private ObjFieldGetterSetter objFieldGetter;
				private PrimFieldGetterSetter primFieldGetter;
				private ObjFieldGetterSetter objFieldSetter;
				private PrimFieldGetterSetter primFieldSetter;

				private static void Dummy(object obj, object[] objarr)
				{
				}

				private static void Dummy(object obj, byte[] barr)
				{
				}

				internal FastFieldReflector(jiObjectStreamField[] fields)
				{
					this.fields = fields;
					TypeWrapper tw = null;
					foreach (jiObjectStreamField field in fields)
					{
						FieldWrapper fw = GetFieldWrapper(field);
						if (fw != null)
						{
							if (tw == null)
							{
								tw = fw.DeclaringType;
							}
							else if (tw != fw.DeclaringType)
							{
								// pre-condition is that all fields are from the same Type!
								throw new jlInternalError();
							}
						}
					}
					if (tw == null)
					{
						objFieldGetter = objFieldSetter = objDummy;
						primFieldGetter = primFieldSetter = primDummy;
					}
					else
					{
						try
						{
							tw.Finish();
						}
						catch (RetargetableJavaException x)
						{
							throw x.ToJava();
						}
						DynamicMethod dmObjGetter = DynamicMethodUtils.Create("__<ObjFieldGetter>", tw.TypeAsBaseType, true, null, new Type[] { typeof(object), typeof(object[]) });
						DynamicMethod dmPrimGetter = DynamicMethodUtils.Create("__<PrimFieldGetter>", tw.TypeAsBaseType, true, null, new Type[] { typeof(object), typeof(byte[]) });
						DynamicMethod dmObjSetter = DynamicMethodUtils.Create("__<ObjFieldSetter>", tw.TypeAsBaseType, true, null, new Type[] { typeof(object), typeof(object[]) });
						DynamicMethod dmPrimSetter = DynamicMethodUtils.Create("__<PrimFieldSetter>", tw.TypeAsBaseType, true, null, new Type[] { typeof(object), typeof(byte[]) });
						CodeEmitter ilgenObjGetter = CodeEmitter.Create(dmObjGetter);
						CodeEmitter ilgenPrimGetter = CodeEmitter.Create(dmPrimGetter);
						CodeEmitter ilgenObjSetter = CodeEmitter.Create(dmObjSetter);
						CodeEmitter ilgenPrimSetter = CodeEmitter.Create(dmPrimSetter);

						// we want the getters to be verifiable (because writeObject can be used from partial trust),
						// so we create a local to hold the properly typed object reference
						CodeEmitterLocal objGetterThis = ilgenObjGetter.DeclareLocal(tw.TypeAsBaseType);
						CodeEmitterLocal primGetterThis = ilgenPrimGetter.DeclareLocal(tw.TypeAsBaseType);
						ilgenObjGetter.Emit(OpCodes.Ldarg_0);
						ilgenObjGetter.Emit(OpCodes.Castclass, tw.TypeAsBaseType);
						ilgenObjGetter.Emit(OpCodes.Stloc, objGetterThis);
						ilgenPrimGetter.Emit(OpCodes.Ldarg_0);
						ilgenPrimGetter.Emit(OpCodes.Castclass, tw.TypeAsBaseType);
						ilgenPrimGetter.Emit(OpCodes.Stloc, primGetterThis);

						foreach (jiObjectStreamField field in fields)
						{
							FieldWrapper fw = GetFieldWrapper(field);
							if (fw == null)
							{
								continue;
							}
							fw.ResolveField();
							TypeWrapper fieldType = fw.FieldTypeWrapper;
							try
							{
								fieldType = fieldType.EnsureLoadable(tw.GetClassLoader());
								fieldType.Finish();
							}
							catch (RetargetableJavaException x)
							{
								throw x.ToJava();
							}
							if (fieldType.IsPrimitive)
							{
								// Getter
								ilgenPrimGetter.Emit(OpCodes.Ldarg_1);
								ilgenPrimGetter.EmitLdc_I4(field.getOffset());
								ilgenPrimGetter.Emit(OpCodes.Ldloc, primGetterThis);
								fw.EmitGet(ilgenPrimGetter);
								if (fieldType == PrimitiveTypeWrapper.BYTE)
								{
									ilgenPrimGetter.Emit(OpCodes.Call, WriteByteMethod);
								}
								else if (fieldType == PrimitiveTypeWrapper.BOOLEAN)
								{
									ilgenPrimGetter.Emit(OpCodes.Call, WriteBooleanMethod);
								}
								else if (fieldType == PrimitiveTypeWrapper.CHAR)
								{
									ilgenPrimGetter.Emit(OpCodes.Call, WriteCharMethod);
								}
								else if (fieldType == PrimitiveTypeWrapper.SHORT)
								{
									ilgenPrimGetter.Emit(OpCodes.Call, WriteShortMethod);
								}
								else if (fieldType == PrimitiveTypeWrapper.INT)
								{
									ilgenPrimGetter.Emit(OpCodes.Call, WriteIntMethod);
								}
								else if (fieldType == PrimitiveTypeWrapper.FLOAT)
								{
									ilgenPrimGetter.Emit(OpCodes.Call, WriteFloatMethod);
								}
								else if (fieldType == PrimitiveTypeWrapper.LONG)
								{
									ilgenPrimGetter.Emit(OpCodes.Call, WriteLongMethod);
								}
								else if (fieldType == PrimitiveTypeWrapper.DOUBLE)
								{
									ilgenPrimGetter.Emit(OpCodes.Call, WriteDoubleMethod);
								}
								else
								{
									throw new jlInternalError();
								}

								// Setter
								ilgenPrimSetter.Emit(OpCodes.Ldarg_0);
								ilgenPrimSetter.Emit(OpCodes.Castclass, tw.TypeAsBaseType);
								ilgenPrimSetter.Emit(OpCodes.Ldarg_1);
								ilgenPrimSetter.EmitLdc_I4(field.getOffset());
								if (fieldType == PrimitiveTypeWrapper.BYTE)
								{
									ilgenPrimSetter.Emit(OpCodes.Call, ReadByteMethod);
								}
								else if (fieldType == PrimitiveTypeWrapper.BOOLEAN)
								{
									ilgenPrimSetter.Emit(OpCodes.Call, ReadBooleanMethod);
								}
								else if (fieldType == PrimitiveTypeWrapper.CHAR)
								{
									ilgenPrimSetter.Emit(OpCodes.Call, ReadCharMethod);
								}
								else if (fieldType == PrimitiveTypeWrapper.SHORT)
								{
									ilgenPrimSetter.Emit(OpCodes.Call, ReadShortMethod);
								}
								else if (fieldType == PrimitiveTypeWrapper.INT)
								{
									ilgenPrimSetter.Emit(OpCodes.Call, ReadIntMethod);
								}
								else if (fieldType == PrimitiveTypeWrapper.FLOAT)
								{
									ilgenPrimSetter.Emit(OpCodes.Call, ReadFloatMethod);
								}
								else if (fieldType == PrimitiveTypeWrapper.LONG)
								{
									ilgenPrimSetter.Emit(OpCodes.Call, ReadLongMethod);
								}
								else if (fieldType == PrimitiveTypeWrapper.DOUBLE)
								{
									ilgenPrimSetter.Emit(OpCodes.Call, ReadDoubleMethod);
								}
								else
								{
									throw new jlInternalError();
								}
								fw.EmitSet(ilgenPrimSetter);
							}
							else
							{
								// Getter
								ilgenObjGetter.Emit(OpCodes.Ldarg_1);
								ilgenObjGetter.EmitLdc_I4(field.getOffset());
								ilgenObjGetter.Emit(OpCodes.Ldloc, objGetterThis);
								fw.EmitGet(ilgenObjGetter);
								fieldType.EmitConvSignatureTypeToStackType(ilgenObjGetter);
								ilgenObjGetter.Emit(OpCodes.Stelem_Ref);

								// Setter
								ilgenObjSetter.Emit(OpCodes.Ldarg_0);
								ilgenObjSetter.Emit(OpCodes.Ldarg_1);
								ilgenObjSetter.EmitLdc_I4(field.getOffset());
								ilgenObjSetter.Emit(OpCodes.Ldelem_Ref);
								fieldType.EmitCheckcast(ilgenObjSetter);
								fieldType.EmitConvStackTypeToSignatureType(ilgenObjSetter, null);
								fw.EmitSet(ilgenObjSetter);
							}
						}
						ilgenObjGetter.Emit(OpCodes.Ret);
						ilgenPrimGetter.Emit(OpCodes.Ret);
						ilgenObjSetter.Emit(OpCodes.Ret);
						ilgenPrimSetter.Emit(OpCodes.Ret);
						ilgenObjGetter.DoEmit();
						ilgenPrimGetter.DoEmit();
						ilgenObjSetter.DoEmit();
						ilgenPrimSetter.DoEmit();
						objFieldGetter = (ObjFieldGetterSetter)dmObjGetter.CreateDelegate(typeof(ObjFieldGetterSetter));
						primFieldGetter = (PrimFieldGetterSetter)dmPrimGetter.CreateDelegate(typeof(PrimFieldGetterSetter));
						objFieldSetter = (ObjFieldGetterSetter)dmObjSetter.CreateDelegate(typeof(ObjFieldGetterSetter));
						primFieldSetter = (PrimFieldGetterSetter)dmPrimSetter.CreateDelegate(typeof(PrimFieldGetterSetter));
					}
				}

				private static FieldWrapper GetFieldWrapper(jiObjectStreamField field)
				{
					jlrField f = field.getField();
					return f == null ? null : FieldWrapper.FromField(f);
				}

				public override jiObjectStreamField[] getFields()
				{
					return fields;
				}

				public override void getObjFieldValues(object obj, object[] objarr)
				{
					objFieldGetter(obj, objarr);
				}

				public override void setObjFieldValues(object obj, object[] objarr)
				{
					objFieldSetter(obj, objarr);
				}

				public override void getPrimFieldValues(object obj, byte[] barr)
				{
					primFieldGetter(obj, barr);
				}

				public override void setPrimFieldValues(object obj, byte[] barr)
				{
					primFieldSetter(obj, barr);
				}
			}
#endif // !FIRST_PASS

			public static object getFastFieldReflector(object fieldsObj)
			{
#if FIRST_PASS
				return null;
#else
				return new FastFieldReflector((jiObjectStreamField[])fieldsObj);
#endif
			}
		}

		static class Win32FileSystem
		{
			internal const int ACCESS_READ = 0x04;
			const int ACCESS_WRITE = 0x02;
			const int ACCESS_EXECUTE = 0x01;

			public static string getDriveDirectory(object _this, int drive)
			{
				try
				{
					string path = ((char)('A' + (drive - 1))) + ":";
					return System.IO.Path.GetFullPath(path).Substring(2);
				}
				catch (ArgumentException)
				{
				}
				catch (System.Security.SecurityException)
				{
				}
				catch (System.IO.PathTooLongException)
				{
				}
				return "\\";
			}

			private static string CanonicalizePath(string path)
			{
				try
				{
					System.IO.FileInfo fi = new System.IO.FileInfo(path);
					if (fi.DirectoryName == null)
					{
						return path.Length > 1 && path[1] == ':'
							? (Char.ToUpper(path[0]) + ":" + System.IO.Path.DirectorySeparatorChar)
							: path;
					}
					string dir = CanonicalizePath(fi.DirectoryName);
					string name = fi.Name;
					try
					{
						if (!VirtualFileSystem.IsVirtualFS(path))
						{
							string[] arr = System.IO.Directory.GetFileSystemEntries(dir, name);
							if (arr.Length == 1)
							{
								name = arr[0];
							}
						}
					}
					catch (System.UnauthorizedAccessException)
					{
					}
					catch (System.IO.IOException)
					{
					}
					return System.IO.Path.Combine(dir, name);
				}
				catch (System.UnauthorizedAccessException)
				{
				}
				catch (System.IO.IOException)
				{
				}
				catch (System.Security.SecurityException)
				{
				}
				catch (System.NotSupportedException)
				{
				}
				return path;
			}

			public static string canonicalize0(object _this, string path)
			{
#if FIRST_PASS
				return null;
#else
				try
				{
					// TODO there is still a known bug here. A dotted path component right after the root component
					// are not removed as they should be. E.g. "c:\..." => "C:\..." or "\\server\..." => IOException
					// Another know issue is that when running under Mono on Windows, the case names aren't converted
					// to the correct (on file system) casing.
					//
					// FXBUG we're appending the directory separator to work around an apparent .NET bug.
					// If we don't do this, "c:\j\." would be canonicalized to "C:\"
					int colon = path.IndexOf(':', 2);
					if (colon != -1)
					{
						return CanonicalizePath(path.Substring(0, colon) + System.IO.Path.DirectorySeparatorChar) + path.Substring(colon);
					}
					return CanonicalizePath(path + System.IO.Path.DirectorySeparatorChar);
				}
				catch (System.ArgumentException x)
				{
					throw new jiIOException(x.Message);
				}
#endif
			}

			public static string canonicalizeWithPrefix0(object _this, string canonicalPrefix, string pathWithCanonicalPrefix)
			{
				return canonicalize0(_this, pathWithCanonicalPrefix);
			}

			private static string GetPathFromFile(jiFile file)
			{
#if FIRST_PASS
				return null;
#else
				return file.getPath();
#endif
			}

			public static int getBooleanAttributes(object _this, jiFile f)
			{
				try
				{
					string path = GetPathFromFile(f);
					if (VirtualFileSystem.IsVirtualFS(path))
					{
						return VirtualFileSystem.GetBooleanAttributes(path);
					}
					System.IO.FileAttributes attr = System.IO.File.GetAttributes(path);
					const int BA_EXISTS = 0x01;
					const int BA_REGULAR = 0x02;
					const int BA_DIRECTORY = 0x04;
					const int BA_HIDDEN = 0x08;
					int rv = BA_EXISTS;
					if ((attr & System.IO.FileAttributes.Directory) != 0)
					{
						rv |= BA_DIRECTORY;
					}
					else
					{
						rv |= BA_REGULAR;
					}
					if ((attr & System.IO.FileAttributes.Hidden) != 0)
					{
						rv |= BA_HIDDEN;
					}
					return rv;
				}
				catch (System.ArgumentException)
				{
				}
				catch (System.UnauthorizedAccessException)
				{
				}
				catch (System.Security.SecurityException)
				{
				}
				catch (System.NotSupportedException)
				{
				}
				catch (System.IO.IOException)
				{
				}
				return 0;
			}

			public static bool checkAccess(object _this, jiFile f, int access)
			{
				string path = GetPathFromFile(f);
				if (VirtualFileSystem.IsVirtualFS(path))
				{
					return VirtualFileSystem.CheckAccess(path, access);
				}
				bool ok = true;
				if ((access & (ACCESS_READ | ACCESS_EXECUTE)) != 0)
				{
					ok = false;
					try
					{
						// HACK if path refers to a directory, we always return true
						if (!System.IO.Directory.Exists(path))
						{
							new System.IO.FileInfo(path).Open(
								System.IO.FileMode.Open,
								System.IO.FileAccess.Read,
								System.IO.FileShare.ReadWrite).Close();
						}
						ok = true;
					}
					catch (System.Security.SecurityException)
					{
					}
					catch (System.ArgumentException)
					{
					}
					catch (System.UnauthorizedAccessException)
					{
					}
					catch (System.IO.IOException)
					{
					}
					catch (System.NotSupportedException)
					{
					}
				}
				if (ok && ((access & ACCESS_WRITE) != 0))
				{
					ok = false;
					try
					{
						// HACK if path refers to a directory, we always return true
						if (System.IO.Directory.Exists(path))
						{
							ok = true;
						}
						else
						{
							System.IO.FileInfo fileInfo = new System.IO.FileInfo(path);
							// Like the JDK we'll only look at the read-only attribute and not
							// the security permissions associated with the file or directory.
							ok = (fileInfo.Attributes & System.IO.FileAttributes.ReadOnly) == 0;
						}
					}
					catch (System.Security.SecurityException)
					{
					}
					catch (System.ArgumentException)
					{
					}
					catch (System.UnauthorizedAccessException)
					{
					}
					catch (System.IO.IOException)
					{
					}
					catch (System.NotSupportedException)
					{
					}
				}
				return ok;
			}

			private static long DateTimeToJavaLongTime(System.DateTime datetime)
			{
				return (System.TimeZone.CurrentTimeZone.ToUniversalTime(datetime) - new System.DateTime(1970, 1, 1)).Ticks / 10000L;
			}

			private static System.DateTime JavaLongTimeToDateTime(long datetime)
			{
				return System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(new System.DateTime(1970, 1, 1).Ticks + datetime * 10000L));
			}

			public static long getLastModifiedTime(object _this, jiFile f)
			{
				try
				{
					DateTime dt = System.IO.File.GetLastWriteTime(GetPathFromFile(f));
					if (dt.ToFileTime() == 0)
					{
						return 0;
					}
					else
					{
						return DateTimeToJavaLongTime(dt);
					}
				}
				catch (System.UnauthorizedAccessException)
				{
				}
				catch (System.ArgumentException)
				{
				}
				catch (System.IO.IOException)
				{
				}
				catch (System.NotSupportedException)
				{
				}
				return 0;
			}

			public static long getLength(object _this, jiFile f)
			{
				try
				{
					string path = GetPathFromFile(f);
					if (VirtualFileSystem.IsVirtualFS(path))
					{
						return VirtualFileSystem.GetLength(path);
					}
					return new System.IO.FileInfo(path).Length;
				}
				catch (System.Security.SecurityException)
				{
				}
				catch (System.ArgumentException)
				{
				}
				catch (System.UnauthorizedAccessException)
				{
				}
				catch (System.IO.IOException)
				{
				}
				catch (System.NotSupportedException)
				{
				}
				return 0;
			}

			public static bool setPermission(object _this, jiFile f, int access, bool enable, bool owneronly)
			{
				if ((access & ACCESS_WRITE) != 0)
				{
					try
					{
						System.IO.FileInfo file = new System.IO.FileInfo(GetPathFromFile(f));
						if (enable)
						{
							file.Attributes &= ~System.IO.FileAttributes.ReadOnly;
						}
						else
						{
							file.Attributes |= System.IO.FileAttributes.ReadOnly;
						}
						return true;
					}
					catch (System.Security.SecurityException)
					{
					}
					catch (System.ArgumentException)
					{
					}
					catch (System.UnauthorizedAccessException)
					{
					}
					catch (System.IO.IOException)
					{
					}
					catch (System.NotSupportedException)
					{
					}
					return false;
				}
				return enable;
			}

			public static bool createFileExclusively(object _this, string path)
			{
#if !FIRST_PASS
				try
				{
					System.IO.File.Open(path, System.IO.FileMode.CreateNew, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None).Close();
					return true;
				}
				catch (System.ArgumentException x)
				{
					throw new jiIOException(x.Message);
				}
				catch (System.IO.IOException x)
				{
					if (!System.IO.File.Exists(path) && !System.IO.Directory.Exists(path))
					{
						throw new jiIOException(x.Message);
					}
				}
				catch (System.UnauthorizedAccessException x)
				{
					if (!System.IO.File.Exists(path) && !System.IO.Directory.Exists(path))
					{
						throw new jiIOException(x.Message);
					}
				}
				catch (System.NotSupportedException x)
				{
					throw new jiIOException(x.Message);
				}
#endif
				return false;
			}

			public static bool delete0(object _this, jiFile f)
			{
				System.IO.FileSystemInfo fileInfo = null;
				try
				{
					string path = GetPathFromFile(f);
					if (System.IO.Directory.Exists(path))
					{
						fileInfo = new System.IO.DirectoryInfo(path);
					}
					else if (System.IO.File.Exists(path))
					{
						fileInfo = new System.IO.FileInfo(path);
					}
					else
					{
						return false;
					}
					// We need to be able to delete read-only files/dirs too, so we clear
					// the read-only attribute, if set.
					if ((fileInfo.Attributes & System.IO.FileAttributes.ReadOnly) != 0)
					{
						fileInfo.Attributes &= ~System.IO.FileAttributes.ReadOnly;
					}
					fileInfo.Delete();
					return true;
				}
				catch (System.Security.SecurityException)
				{
				}
				catch (System.ArgumentException)
				{
				}
				catch (System.UnauthorizedAccessException)
				{
				}
				catch (System.IO.IOException)
				{
				}
				catch (System.NotSupportedException)
				{
				}
				return false;
			}

			public static string[] list(object _this, jiFile f)
			{
				try
				{
					string path = GetPathFromFile(f);
					if (VirtualFileSystem.IsVirtualFS(path))
					{
						return VirtualFileSystem.List(path);
					}
					string[] l = System.IO.Directory.GetFileSystemEntries(path);
					for (int i = 0; i < l.Length; i++)
					{
						int pos = l[i].LastIndexOf(System.IO.Path.DirectorySeparatorChar);
						if (pos >= 0)
						{
							l[i] = l[i].Substring(pos + 1);
						}
					}
					return l;
				}
				catch (System.ArgumentException)
				{
				}
				catch (System.IO.IOException)
				{
				}
				catch (System.UnauthorizedAccessException)
				{
				}
				catch (System.NotSupportedException)
				{
				}
				return null;
			}

			public static bool createDirectory(object _this, jiFile f)
			{
				try
				{
					string path = GetPathFromFile(f);
					System.IO.DirectoryInfo parent = System.IO.Directory.GetParent(path);
					if (parent == null ||
						!System.IO.Directory.Exists(parent.FullName) ||
						System.IO.Directory.Exists(path))
					{
						return false;
					}
					return System.IO.Directory.CreateDirectory(path) != null;
				}
				catch (System.Security.SecurityException)
				{
				}
				catch (System.ArgumentException)
				{
				}
				catch (System.UnauthorizedAccessException)
				{
				}
				catch (System.IO.IOException)
				{
				}
				catch (System.NotSupportedException)
				{
				}
				return false;
			}

			public static bool rename0(object _this, jiFile f1, jiFile f2)
			{
				try
				{
					new System.IO.FileInfo(GetPathFromFile(f1)).MoveTo(GetPathFromFile(f2));
					return true;
				}
				catch (System.Security.SecurityException)
				{
				}
				catch (System.ArgumentException)
				{
				}
				catch (System.UnauthorizedAccessException)
				{
				}
				catch (System.IO.IOException)
				{
				}
				catch (System.NotSupportedException)
				{
				}
				return false;
			}

			public static bool setLastModifiedTime(object _this, jiFile f, long time)
			{
				try
				{
					new System.IO.FileInfo(GetPathFromFile(f)).LastWriteTime = JavaLongTimeToDateTime(time);
					return true;
				}
				catch (System.Security.SecurityException)
				{
				}
				catch (System.ArgumentException)
				{
				}
				catch (System.UnauthorizedAccessException)
				{
				}
				catch (System.IO.IOException)
				{
				}
				catch (System.NotSupportedException)
				{
				}
				return false;
			}

			public static bool setReadOnly(object _this, jiFile f)
			{
				try
				{
					System.IO.FileInfo fileInfo = new System.IO.FileInfo(GetPathFromFile(f));
					fileInfo.Attributes |= System.IO.FileAttributes.ReadOnly;
					return true;
				}
				catch (System.Security.SecurityException)
				{
				}
				catch (System.ArgumentException)
				{
				}
				catch (System.UnauthorizedAccessException)
				{
				}
				catch (System.IO.IOException)
				{
				}
				catch (System.NotSupportedException)
				{
				}
				return false;
			}

			public static int listRoots0()
			{
				try
				{
					int drives = 0;
					foreach (string drive in Environment.GetLogicalDrives())
					{
						char c = Char.ToUpper(drive[0]);
						drives |= 1 << (c - 'A');
					}
					return drives;
				}
				catch (System.IO.IOException)
				{
				}
				catch (System.UnauthorizedAccessException)
				{
				}
				catch (System.Security.SecurityException)
				{
				}
				return 0;
			}
			
			[System.Security.SecuritySafeCritical]
			public static long getSpace0(object _this, jiFile f, int t)
			{
				const int SPACE_TOTAL = 0;
				const int SPACE_FREE = 1;
				const int SPACE_USABLE = 2;
				long freeAvailable;
				long total;
				long totalFree;
				if (GetDiskFreeSpaceEx(GetPathFromFile(f), out freeAvailable, out total, out totalFree) != 0)
				{
					switch (t)
					{
						case SPACE_TOTAL:
							return total;
						case SPACE_FREE:
							return totalFree;
						case SPACE_USABLE:
							return freeAvailable;
					}
				}
				return 0;
			}

			[System.Runtime.InteropServices.DllImport("kernel32")]
			private static extern int GetDiskFreeSpaceEx(string directory, out long freeAvailable, out long total, out long totalFree);

			public static void initIDs()
			{
			}
		}

		static class UnixFileSystem
		{
			public static int getBooleanAttributes0(object _this, jiFile f)
			{
				return Win32FileSystem.getBooleanAttributes(_this, f);
			}

			public static long getSpace(object _this, jiFile f, int t)
			{
				// TODO
				return 0;
			}

			public static string getDriveDirectory(object _this, int drive)
			{
				return Win32FileSystem.getDriveDirectory(_this, drive);
			}

			public static string canonicalize0(object _this, string path)
			{
				return Win32FileSystem.canonicalize0(_this, path);
			}

			public static bool checkAccess(object _this, jiFile f, int access)
			{
				return Win32FileSystem.checkAccess(_this, f, access);
			}

			public static long getLastModifiedTime(object _this, jiFile f)
			{
				return Win32FileSystem.getLastModifiedTime(_this, f);
			}

			public static long getLength(object _this, jiFile f)
			{
				return Win32FileSystem.getLength(_this, f);
			}

			public static bool setPermission(object _this, jiFile f, int access, bool enable, bool owneronly)
			{
				// TODO consider using Mono.Posix
				return Win32FileSystem.setPermission(_this, f, access, enable, owneronly);
			}

			public static bool createFileExclusively(object _this, string path)
			{
				return Win32FileSystem.createFileExclusively(_this, path);
			}

			public static bool delete0(object _this, jiFile f)
			{
				return Win32FileSystem.delete0(_this, f);
			}

			public static string[] list(object _this, jiFile f)
			{
				return Win32FileSystem.list(_this, f);
			}

			public static bool createDirectory(object _this, jiFile f)
			{
				return Win32FileSystem.createDirectory(_this, f);
			}

			public static bool rename0(object _this, jiFile f1, jiFile f2)
			{
				return Win32FileSystem.rename0(_this, f1, f2);
			}

			public static bool setLastModifiedTime(object _this, jiFile f, long time)
			{
				return Win32FileSystem.setLastModifiedTime(_this, f, time);
			}

			public static bool setReadOnly(object _this, jiFile f)
			{
				return Win32FileSystem.setReadOnly(_this, f);
			}

			public static void initIDs()
			{
			}
		}
	}

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

		namespace reflect
		{
			static class Array
			{
#if FIRST_PASS
			public static int getLength(object arrayObj)
			{
				return 0;
			}

			public static object get(object arrayObj, int index)
			{
				return null;
			}

			public static bool getBoolean(object arrayObj, int index)
			{
				return false;
			}

			public static byte getByte(object arrayObj, int index)
			{
				return 0;
			}

			public static char getChar(object arrayObj, int index)
			{
				return '\u0000';
			}

			public static short getShort(object arrayObj, int index)
			{
				return 0;
			}

			public static int getInt(object arrayObj, int index)
			{
				return 0;
			}

			public static float getFloat(object arrayObj, int index)
			{
				return 0;
			}

			public static long getLong(object arrayObj, int index)
			{
				return 0;
			}

			public static double getDouble(object arrayObj, int index)
			{
				return 0;
			}

			public static void set(object arrayObj, int index, object value)
			{
			}

			public static void setBoolean(object arrayObj, int index, bool value)
			{
			}

			public static void setByte(object arrayObj, int index, byte value)
			{
			}

			public static void setChar(object arrayObj, int index, char value)
			{
			}

			public static void setShort(object arrayObj, int index, short value)
			{
			}

			public static void setInt(object arrayObj, int index, int value)
			{
			}

			public static void setFloat(object arrayObj, int index, float value)
			{
			}

			public static void setLong(object arrayObj, int index, long value)
			{
			}

			public static void setDouble(object arrayObj, int index, double value)
			{
			}

			public static object newArray(jlClass componentType, int length)
			{
				return null;
			}

			public static object multiNewArray(jlClass componentType, int[] dimensions)
			{
				return null;
			}
#else
				private static SystemArray CheckArray(object arrayObj)
				{
					if (arrayObj == null)
					{
						throw new jlNullPointerException();
					}
					SystemArray arr = arrayObj as SystemArray;
					if (arr != null)
					{
						return arr;
					}
					throw new jlIllegalArgumentException("Argument is not an array");
				}

				public static int getLength(object arrayObj)
				{
					return CheckArray(arrayObj).Length;
				}

				public static object get(object arrayObj, int index)
				{
					SystemArray arr = CheckArray(arrayObj);
					if (index < 0 || index >= arr.Length)
					{
						throw new jlArrayIndexOutOfBoundsException();
					}
					// We need to look at the actual type here, because "is" or "as"
					// will convert enums to their underlying type and unsigned integral types
					// to their signed counter parts.
					Type type = arrayObj.GetType();
					if (type == typeof(bool[]))
					{
						return jlBoolean.valueOf(((bool[])arr)[index]);
					}
					if (type == typeof(byte[]))
					{
						return jlByte.valueOf(((byte[])arr)[index]);
					}
					if (type == typeof(short[]))
					{
						return jlShort.valueOf(((short[])arr)[index]);
					}
					if (type == typeof(char[]))
					{
						return jlCharacter.valueOf(((char[])arr)[index]);
					}
					if (type == typeof(int[]))
					{
						return jlInteger.valueOf(((int[])arr)[index]);
					}
					if (type == typeof(float[]))
					{
						return jlFloat.valueOf(((float[])arr)[index]);
					}
					if (type == typeof(long[]))
					{
						return jlLong.valueOf(((long[])arr)[index]);
					}
					if (type == typeof(double[]))
					{
						return jlDouble.valueOf(((double[])arr)[index]);
					}
					return arr.GetValue(index);
				}

				public static bool getBoolean(object arrayObj, int index)
				{
					if (arrayObj == null)
					{
						throw new jlNullPointerException();
					}
					bool[] arr = arrayObj as bool[];
					if (arr != null)
					{
						if (index < 0 || index >= arr.Length)
						{
							throw new jlArrayIndexOutOfBoundsException();
						}
						return arr[index];
					}
					throw new jlIllegalArgumentException("argument type mismatch");
				}

				public static byte getByte(object arrayObj, int index)
				{
					if (arrayObj == null)
					{
						throw new jlNullPointerException();
					}
					byte[] arr = arrayObj as byte[];
					if (arr != null)
					{
						if (index < 0 || index >= arr.Length)
						{
							throw new jlArrayIndexOutOfBoundsException();
						}
						return arr[index];
					}
					throw new jlIllegalArgumentException("argument type mismatch");
				}

				public static char getChar(object arrayObj, int index)
				{
					if (arrayObj == null)
					{
						throw new jlNullPointerException();
					}
					char[] arr = arrayObj as char[];
					if (arr != null)
					{
						if (index < 0 || index >= arr.Length)
						{
							throw new jlArrayIndexOutOfBoundsException();
						}
						return arr[index];
					}
					throw new jlIllegalArgumentException("argument type mismatch");
				}

				public static short getShort(object arrayObj, int index)
				{
					if (arrayObj == null)
					{
						throw new jlNullPointerException();
					}
					short[] arr = arrayObj as short[];
					if (arr != null)
					{
						if (index < 0 || index >= arr.Length)
						{
							throw new jlArrayIndexOutOfBoundsException();
						}
						return arr[index];
					}
					return (sbyte)getByte(arrayObj, index);
				}

				public static int getInt(object arrayObj, int index)
				{
					if (arrayObj == null)
					{
						throw new jlNullPointerException();
					}
					int[] arr1 = arrayObj as int[];
					if (arr1 != null)
					{
						if (index < 0 || index >= arr1.Length)
						{
							throw new jlArrayIndexOutOfBoundsException();
						}
						return arr1[index];
					}
					char[] arr2 = arrayObj as char[];
					if (arr2 != null)
					{
						if (index < 0 || index >= arr2.Length)
						{
							throw new jlArrayIndexOutOfBoundsException();
						}
						return arr2[index];
					}
					return getShort(arrayObj, index);
				}

				public static float getFloat(object arrayObj, int index)
				{
					if (arrayObj == null)
					{
						throw new jlNullPointerException();
					}
					float[] arr = arrayObj as float[];
					if (arr != null)
					{
						if (index < 0 || index >= arr.Length)
						{
							throw new jlArrayIndexOutOfBoundsException();
						}
						return arr[index];
					}
					return getLong(arrayObj, index);
				}

				public static long getLong(object arrayObj, int index)
				{
					if (arrayObj == null)
					{
						throw new jlNullPointerException();
					}
					long[] arr = arrayObj as long[];
					if (arr != null)
					{
						if (index < 0 || index >= arr.Length)
						{
							throw new jlArrayIndexOutOfBoundsException();
						}
						return arr[index];
					}
					return getInt(arrayObj, index);
				}

				public static double getDouble(object arrayObj, int index)
				{
					if (arrayObj == null)
					{
						throw new jlNullPointerException();
					}
					double[] arr = arrayObj as double[];
					if (arr != null)
					{
						if (index < 0 || index >= arr.Length)
						{
							throw new jlArrayIndexOutOfBoundsException();
						}
						return arr[index];
					}
					return getFloat(arrayObj, index);
				}

				public static void set(object arrayObj, int index, object value)
				{
					if (arrayObj == null)
					{
						throw new jlNullPointerException();
					}
					Type type = arrayObj.GetType();
					if (ReflectUtil.IsVector(type) && ClassLoaderWrapper.GetWrapperFromType(type.GetElementType()).IsPrimitive)
					{
						jlBoolean booleanValue = value as jlBoolean;
						if (booleanValue != null)
						{
							setBoolean(arrayObj, index, booleanValue.booleanValue());
							return;
						}
						jlByte byteValue = value as jlByte;
						if (byteValue != null)
						{
							setByte(arrayObj, index, byteValue.byteValue());
							return;
						}
						jlCharacter charValue = value as jlCharacter;
						if (charValue != null)
						{
							setChar(arrayObj, index, charValue.charValue());
							return;
						}
						jlShort shortValue = value as jlShort;
						if (shortValue != null)
						{
							setShort(arrayObj, index, shortValue.shortValue());
							return;
						}
						jlInteger intValue = value as jlInteger;
						if (intValue != null)
						{
							setInt(arrayObj, index, intValue.intValue());
							return;
						}
						jlFloat floatValue = value as jlFloat;
						if (floatValue != null)
						{
							setFloat(arrayObj, index, floatValue.floatValue());
							return;
						}
						jlLong longValue = value as jlLong;
						if (longValue != null)
						{
							setLong(arrayObj, index, longValue.longValue());
							return;
						}
						jlDouble doubleValue = value as jlDouble;
						if (doubleValue != null)
						{
							setDouble(arrayObj, index, doubleValue.doubleValue());
							return;
						}
					}
					try
					{
						CheckArray(arrayObj).SetValue(value, index);
					}
					catch (InvalidCastException)
					{
						throw new jlIllegalArgumentException("argument type mismatch");
					}
					catch (IndexOutOfRangeException)
					{
						throw new jlArrayIndexOutOfBoundsException();
					}
				}

				public static void setBoolean(object arrayObj, int index, bool value)
				{
					if (arrayObj == null)
					{
						throw new jlNullPointerException();
					}
					bool[] arr = arrayObj as bool[];
					if (arr != null)
					{
						if (index < 0 || index >= arr.Length)
						{
							throw new jlArrayIndexOutOfBoundsException();
						}
						arr[index] = value;
					}
					else
					{
						throw new jlIllegalArgumentException("argument type mismatch");
					}
				}

				public static void setByte(object arrayObj, int index, byte value)
				{
					if (arrayObj == null)
					{
						throw new jlNullPointerException();
					}
					byte[] arr = arrayObj as byte[];
					if (arr != null)
					{
						if (index < 0 || index >= arr.Length)
						{
							throw new jlArrayIndexOutOfBoundsException();
						}
						arr[index] = value;
					}
					else
					{
						setShort(arrayObj, index, (sbyte)value);
					}
				}

				public static void setChar(object arrayObj, int index, char value)
				{
					if (arrayObj == null)
					{
						throw new jlNullPointerException();
					}
					char[] arr = arrayObj as char[];
					if (arr != null)
					{
						if (index < 0 || index >= arr.Length)
						{
							throw new jlArrayIndexOutOfBoundsException();
						}
						arr[index] = value;
					}
					else
					{
						setInt(arrayObj, index, value);
					}
				}

				public static void setShort(object arrayObj, int index, short value)
				{
					if (arrayObj == null)
					{
						throw new jlNullPointerException();
					}
					short[] arr = arrayObj as short[];
					if (arr != null)
					{
						if (index < 0 || index >= arr.Length)
						{
							throw new jlArrayIndexOutOfBoundsException();
						}
						arr[index] = value;
					}
					else
					{
						setInt(arrayObj, index, value);
					}
				}

				public static void setInt(object arrayObj, int index, int value)
				{
					if (arrayObj == null)
					{
						throw new jlNullPointerException();
					}
					int[] arr = arrayObj as int[];
					if (arr != null)
					{
						if (index < 0 || index >= arr.Length)
						{
							throw new jlArrayIndexOutOfBoundsException();
						}
						arr[index] = value;
					}
					else
					{
						setLong(arrayObj, index, value);
					}
				}

				public static void setFloat(object arrayObj, int index, float value)
				{
					if (arrayObj == null)
					{
						throw new jlNullPointerException();
					}
					float[] arr = arrayObj as float[];
					if (arr != null)
					{
						if (index < 0 || index >= arr.Length)
						{
							throw new jlArrayIndexOutOfBoundsException();
						}
						arr[index] = value;
					}
					else
					{
						setDouble(arrayObj, index, value);
					}
				}

				public static void setLong(object arrayObj, int index, long value)
				{
					if (arrayObj == null)
					{
						throw new jlNullPointerException();
					}
					long[] arr = arrayObj as long[];
					if (arr != null)
					{
						if (index < 0 || index >= arr.Length)
						{
							throw new jlArrayIndexOutOfBoundsException();
						}
						arr[index] = value;
					}
					else
					{
						setFloat(arrayObj, index, value);
					}
				}

				public static void setDouble(object arrayObj, int index, double value)
				{
					if (arrayObj == null)
					{
						throw new jlNullPointerException();
					}
					double[] arr = arrayObj as double[];
					if (arr != null)
					{
						if (index < 0 || index >= arr.Length)
						{
							throw new jlArrayIndexOutOfBoundsException();
						}
						arr[index] = value;
					}
					else
					{
						throw new jlIllegalArgumentException("argument type mismatch");
					}
				}

				public static object newArray(jlClass componentType, int length)
				{
					if (componentType == null)
					{
						throw new jlNullPointerException();
					}
					if (componentType == jlVoid.TYPE)
					{
						throw new jlIllegalArgumentException();
					}
					if (length < 0)
					{
						throw new jlNegativeArraySizeException();
					}
					try
					{
						TypeWrapper wrapper = TypeWrapper.FromClass(componentType);
						wrapper.Finish();
						object obj = SystemArray.CreateInstance(wrapper.TypeAsArrayType, length);
						if (wrapper.IsGhost || wrapper.IsGhostArray)
						{
							IKVM.Runtime.GhostTag.SetTag(obj, wrapper.MakeArrayType(1));
						}
						return obj;
					}
					catch (RetargetableJavaException x)
					{
						throw x.ToJava();
					}
					catch (NotSupportedException x)
					{
						// This happens when you try to create an array from TypedReference, ArgIterator, ByRef,
						// RuntimeArgumentHandle or an open generic type.
						throw new jlIllegalArgumentException(x.Message);
					}
				}

				public static object multiNewArray(jlClass componentType, int[] dimensions)
				{
					if (componentType == null || dimensions == null)
					{
						throw new jlNullPointerException();
					}
					if (componentType == jlVoid.TYPE)
					{
						throw new jlIllegalArgumentException();
					}
					if (dimensions.Length == 0 || dimensions.Length > 255)
					{
						throw new jlIllegalArgumentException();
					}
					try
					{
						TypeWrapper wrapper = TypeWrapper.FromClass(componentType).MakeArrayType(dimensions.Length);
						wrapper.Finish();
						object obj = IKVM.Runtime.ByteCodeHelper.multianewarray(wrapper.TypeAsArrayType.TypeHandle, dimensions);
						if (wrapper.IsGhostArray)
						{
							IKVM.Runtime.GhostTag.SetTag(obj, wrapper);
						}
						return obj;
					}
					catch (RetargetableJavaException x)
					{
						throw x.ToJava();
					}
					catch (NotSupportedException x)
					{
						// This happens when you try to create an array from TypedReference, ArgIterator, ByRef,
						// RuntimeArgumentHandle or an open generic type.
						throw new jlIllegalArgumentException(x.Message);
					}
				}
#endif // FIRST_PASS
			}
		}

		static class Class
		{
			public static jlClass forName0(string name, bool initialize, jlClassLoader loader)
			{
#if FIRST_PASS
				return null;
#else
				//Console.WriteLine("forName: " + name + ", loader = " + loader);
				TypeWrapper tw = null;
				if (name.IndexOf(',') > 0)
				{
					// we essentially require full trust before allowing arbitrary types to be loaded,
					// hence we do the "createClassLoader" permission check
					jlSecurityManager sm = jlSystem.getSecurityManager();
					if (sm != null)
						sm.checkPermission(new jlRuntimePermission("createClassLoader"));
					Type type = Type.GetType(name);
					if (type != null)
					{
						tw = ClassLoaderWrapper.GetWrapperFromType(type);
					}
					if (tw == null)
					{
						throw new jlClassNotFoundException(name);
					}
				}
				else
				{
					try
					{
						ClassLoaderWrapper classLoaderWrapper = ClassLoaderWrapper.GetClassLoaderWrapper(loader);
						tw = classLoaderWrapper.LoadClassByDottedName(name);
					}
					catch (ClassNotFoundException x)
					{
						throw new global::java.lang.ClassNotFoundException(x.Message);
					}
					catch (ClassLoadingException x)
					{
						throw x.InnerException;
					}
					catch (RetargetableJavaException x)
					{
						throw x.ToJava();
					}
				}
				if (initialize && !tw.IsArray)
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
				return tw.ClassObject;
#endif
			}

			public static bool isInstance(jlClass thisClass, object obj)
			{
				return TypeWrapper.FromClass(thisClass).IsInstance(obj);
			}

			public static bool isAssignableFrom(jlClass thisClass, jlClass otherClass)
			{
#if !FIRST_PASS
				if (otherClass == null)
				{
					throw new jlNullPointerException();
				}
#endif
				return TypeWrapper.FromClass(otherClass).IsAssignableTo(TypeWrapper.FromClass(thisClass));
			}

			public static bool isInterface(jlClass thisClass)
			{
				return TypeWrapper.FromClass(thisClass).IsInterface;
			}

			public static bool isArray(jlClass thisClass)
			{
				return TypeWrapper.FromClass(thisClass).IsArray;
			}

			public static bool isPrimitive(jlClass thisClass)
			{
				return TypeWrapper.FromClass(thisClass).IsPrimitive;
			}

			public static string getName0(jlClass thisClass)
			{
				TypeWrapper tw = TypeWrapper.FromClass(thisClass);
				if (tw.IsPrimitive)
				{
					if (tw == PrimitiveTypeWrapper.BYTE)
					{
						return "byte";
					}
					else if (tw == PrimitiveTypeWrapper.CHAR)
					{
						return "char";
					}
					else if (tw == PrimitiveTypeWrapper.DOUBLE)
					{
						return "double";
					}
					else if (tw == PrimitiveTypeWrapper.FLOAT)
					{
						return "float";
					}
					else if (tw == PrimitiveTypeWrapper.INT)
					{
						return "int";
					}
					else if (tw == PrimitiveTypeWrapper.LONG)
					{
						return "long";
					}
					else if (tw == PrimitiveTypeWrapper.SHORT)
					{
						return "short";
					}
					else if (tw == PrimitiveTypeWrapper.BOOLEAN)
					{
						return "boolean";
					}
					else if (tw == PrimitiveTypeWrapper.VOID)
					{
						return "void";
					}
				}
				return tw.Name;
			}

			public static string getSigName(jlClass thisClass)
			{
				return TypeWrapper.FromClass(thisClass).SigName;
			}

			public static global::java.lang.ClassLoader getClassLoader0(jlClass thisClass)
			{
				return TypeWrapper.FromClass(thisClass).GetClassLoader().GetJavaClassLoader();
			}

			public static jlClass getSuperclass(jlClass thisClass)
			{
				TypeWrapper super = TypeWrapper.FromClass(thisClass).BaseTypeWrapper;
				return super != null ? super.ClassObject : null;
			}

			public static jlClass[] getInterfaces(jlClass thisClass)
			{
#if FIRST_PASS
				return null;
#else
				TypeWrapper[] ifaces = TypeWrapper.FromClass(thisClass).Interfaces;
				jlClass[] interfaces = new jlClass[ifaces.Length];
				for (int i = 0; i < ifaces.Length; i++)
				{
					interfaces[i] = ifaces[i].ClassObject;
				}
				return interfaces;
#endif
			}

			public static jlClass getComponentType(jlClass thisClass)
			{
				TypeWrapper tw = TypeWrapper.FromClass(thisClass);
				return tw.IsArray ? tw.ElementTypeWrapper.ClassObject : null;
			}

			public static int getModifiers(jlClass thisClass)
			{
				// the 0x7FFF mask comes from JVM_ACC_WRITTEN_FLAGS in hotspot\src\share\vm\utilities\accessFlags.hpp
				// masking out ACC_SUPER comes from instanceKlass::compute_modifier_flags() in hotspot\src\share\vm\oops\instanceKlass.cpp
				const int mask = 0x7FFF & (int)~IKVM.Attributes.Modifiers.Super;
				return (int)TypeWrapper.FromClass(thisClass).ReflectiveModifiers & mask;
			}

			public static object[] getSigners(jlClass thisClass)
			{
#if FIRST_PASS
				return null;
#else
				return thisClass.signers;
#endif
			}

			public static void setSigners(jlClass thisClass, object[] signers)
			{
#if !FIRST_PASS
				thisClass.signers = signers;
#endif
			}

			public static object[] getEnclosingMethod0(jlClass thisClass)
			{
				try
				{
					TypeWrapper tw = TypeWrapper.FromClass(thisClass);
					tw.Finish();
					string[] enc = tw.GetEnclosingMethod();
					if (enc == null)
					{
						return null;
					}
					return new object[] { tw.GetClassLoader().LoadClassByDottedName(enc[0]).ClassObject, enc[1], enc[2] == null ? null : enc[2].Replace('.', '/') };
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
			}

			public static jlClass getDeclaringClass(jlClass thisClass)
			{
				try
				{
					TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
					wrapper.Finish();
					TypeWrapper decl = wrapper.DeclaringTypeWrapper;
					if (decl == null)
					{
						return null;
					}
					if (!decl.IsAccessibleFrom(wrapper))
					{
						throw new IllegalAccessError(string.Format("tried to access class {0} from class {1}", decl.Name, wrapper.Name));
					}
					decl.Finish();
					if (SystemArray.IndexOf(decl.InnerClasses, wrapper) == -1)
					{
						throw new IncompatibleClassChangeError(string.Format("{0} and {1} disagree on InnerClasses attribute", decl.Name, wrapper.Name));
					}
					return decl.ClassObject;
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
			}

			public static ProtectionDomain getProtectionDomain0(jlClass thisClass)
			{
#if FIRST_PASS
				return null;
#else
				TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
				while (wrapper.IsArray)
				{
					wrapper = wrapper.ElementTypeWrapper;
				}
				ProtectionDomain pd = wrapper.ClassObject.pd;
				if (pd == null)
				{
					// The protection domain for statically compiled code is created lazily (not at java.lang.Class creation time),
					// to work around boot strap issues.
					AssemblyClassLoader acl = wrapper.GetClassLoader() as AssemblyClassLoader;
					if (acl != null)
					{
						pd = acl.GetProtectionDomain();
					}
				}
				return pd;
#endif
			}

			public static void setProtectionDomain0(jlClass thisClass, ProtectionDomain pd)
			{
#if !FIRST_PASS
				thisClass.pd = pd;
#endif
			}

			public static jlClass getPrimitiveClass(string name)
			{
				// note that this method isn't used anymore (because it is an intrinsic (during core class library compilation))
				// it still remains for compat because it might be invoked through reflection by evil code
				switch (name)
				{
					case "byte":
						return PrimitiveTypeWrapper.BYTE.ClassObject;
					case "char":
						return PrimitiveTypeWrapper.CHAR.ClassObject;
					case "double":
						return PrimitiveTypeWrapper.DOUBLE.ClassObject;
					case "float":
						return PrimitiveTypeWrapper.FLOAT.ClassObject;
					case "int":
						return PrimitiveTypeWrapper.INT.ClassObject;
					case "long":
						return PrimitiveTypeWrapper.LONG.ClassObject;
					case "short":
						return PrimitiveTypeWrapper.SHORT.ClassObject;
					case "boolean":
						return PrimitiveTypeWrapper.BOOLEAN.ClassObject;
					case "void":
						return PrimitiveTypeWrapper.VOID.ClassObject;
					default:
						throw new ArgumentException(name);
				}
			}

			public static string getGenericSignature(jlClass thisClass)
			{
				TypeWrapper tw = TypeWrapper.FromClass(thisClass);
				tw.Finish();
				return tw.GetGenericSignature();
			}

			internal static object AnnotationsToMap(ClassLoaderWrapper loader, object[] objAnn)
			{
#if FIRST_PASS
				return null;
#else
				global::java.util.LinkedHashMap map = new global::java.util.LinkedHashMap();
				if (objAnn != null)
				{
					foreach (object obj in objAnn)
					{
						Annotation a = obj as Annotation;
						if (a != null)
						{
							map.put(a.annotationType(), FreezeOrWrapAttribute(a));
						}
						else if (obj is IKVM.Attributes.DynamicAnnotationAttribute)
						{
							a = (Annotation)JVM.NewAnnotation(loader.GetJavaClassLoader(), ((IKVM.Attributes.DynamicAnnotationAttribute)obj).Definition);
							map.put(a.annotationType(), a);
						}
					}
				}
				return map;
#endif
			}

#if !FIRST_PASS
			internal static Annotation FreezeOrWrapAttribute(Annotation ann)
			{
				global::ikvm.@internal.AnnotationAttributeBase attr = ann as global::ikvm.@internal.AnnotationAttributeBase;
				if (attr != null)
				{
#if DONT_WRAP_ANNOTATION_ATTRIBUTES
					attr.freeze();
#else
					// freeze to make sure the defaults are set
					attr.freeze();
					ann = global::sun.reflect.annotation.AnnotationParser.annotationForMap(attr.annotationType(), attr.getValues());
#endif
				}
				return ann;
			}
#endif

			public static object getDeclaredAnnotationsImpl(jlClass thisClass)
			{
#if FIRST_PASS
				return null;
#else
				TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
				try
				{
					wrapper.Finish();
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				return AnnotationsToMap(wrapper.GetClassLoader(), wrapper.GetDeclaredAnnotations());
#endif
			}

			public static object getDeclaredFields0(jlClass thisClass, bool publicOnly)
			{
#if FIRST_PASS
				return null;
#else
				Profiler.Enter("Class.getDeclaredFields0");
				try
				{
					TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
					// we need to finish the type otherwise all fields will not be in the field map yet
					wrapper.Finish();
					FieldWrapper[] fields = wrapper.GetFields();
					List<jlrField> list = new List<jlrField>();
					for (int i = 0; i < fields.Length; i++)
					{
						if (fields[i].IsHideFromReflection)
						{
							// skip
						}
						else if (publicOnly && !fields[i].IsPublic)
						{
							// caller is only asking for public field, so we don't return this non-public field
						}
						else
						{
							list.Add((jlrField)fields[i].ToField(false, i));
						}
					}
					return list.ToArray();
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				finally
				{
					Profiler.Leave("Class.getDeclaredFields0");
				}
#endif
			}

			public static object getDeclaredMethods0(jlClass thisClass, bool publicOnly)
			{
#if FIRST_PASS
				return null;
#else
				Profiler.Enter("Class.getDeclaredMethods0");
				try
				{
					TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
					wrapper.Finish();
					if (wrapper.HasVerifyError)
					{
						// TODO we should get the message from somewhere
						throw new VerifyError();
					}
					if (wrapper.HasClassFormatError)
					{
						// TODO we should get the message from somewhere
						throw new ClassFormatError(wrapper.Name);
					}
					MethodWrapper[] methods = wrapper.GetMethods();
					List<jlrMethod> list = new List<jlrMethod>();
					for (int i = 0; i < methods.Length; i++)
					{
						// we don't want to expose "hideFromReflection" methods (one reason is that it would
						// mess up the serialVersionUID computation)
						if (!methods[i].IsHideFromReflection
							&& methods[i].Name != "<clinit>" && methods[i].Name != "<init>"
							&& (!publicOnly || methods[i].IsPublic))
						{
							list.Add((jlrMethod)methods[i].ToMethodOrConstructor(false));
						}
					}
					return list.ToArray();
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				finally
				{
					Profiler.Leave("Class.getDeclaredMethods0");
				}
#endif
			}

			public static object getDeclaredConstructors0(jlClass thisClass, bool publicOnly)
			{
#if FIRST_PASS
				return null;
#else
				Profiler.Enter("Class.getDeclaredConstructors0");
				try
				{
					TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
					wrapper.Finish();
					if (wrapper.HasVerifyError)
					{
						// TODO we should get the message from somewhere
						throw new VerifyError();
					}
					if (wrapper.HasClassFormatError)
					{
						// TODO we should get the message from somewhere
						throw new ClassFormatError(wrapper.Name);
					}
					MethodWrapper[] methods = wrapper.GetMethods();
					List<jlrConstructor> list = new List<jlrConstructor>();
					for (int i = 0; i < methods.Length; i++)
					{
						// we don't want to expose "hideFromReflection" methods (one reason is that it would
						// mess up the serialVersionUID computation)
						if (!methods[i].IsHideFromReflection
							&& methods[i].Name == "<init>"
							&& (!publicOnly || methods[i].IsPublic))
						{
							list.Add((jlrConstructor)methods[i].ToMethodOrConstructor(false));
						}
					}
					return list.ToArray();
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				finally
				{
					Profiler.Leave("Class.getDeclaredConstructors0");
				}
#endif
			}

			public static jlClass[] getDeclaredClasses0(jlClass thisClass)
			{
#if FIRST_PASS
				return null;
#else
				try
				{
					TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
					// NOTE to get at the InnerClasses we need to finish the type
					wrapper.Finish();
					TypeWrapper[] wrappers = wrapper.InnerClasses;
					jlClass[] innerclasses = new jlClass[wrappers.Length];
					for (int i = 0; i < innerclasses.Length; i++)
					{
						if (wrappers[i].IsUnloadable)
						{
							throw new jlNoClassDefFoundError(wrappers[i].Name);
						}
						if (!wrappers[i].IsAccessibleFrom(wrapper))
						{
							throw new IllegalAccessError(string.Format("tried to access class {0} from class {1}", wrappers[i].Name, wrapper.Name));
						}
						wrappers[i].Finish();
						innerclasses[i] = wrappers[i].ClassObject;
					}
					return innerclasses;
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
#endif
			}

			public static bool desiredAssertionStatus0(jlClass clazz)
			{
				return IKVM.Runtime.Assertions.IsEnabled(TypeWrapper.FromClass(clazz));
			}
		}

		static class ClassLoader
		{
			public static global::java.net.URL getBootstrapResource(string name)
			{
				foreach (global::java.net.URL url in ClassLoaderWrapper.GetBootstrapClassLoader().GetResources(name))
				{
					return url;
				}
				return null;
			}

			public static global::java.util.Enumeration getBootstrapResources(string name)
			{
#if FIRST_PASS
				return null;
#else
				return new global::ikvm.runtime.EnumerationWrapper(ClassLoaderWrapper.GetBootstrapClassLoader().GetResources(name));
#endif
			}

			public static jlClass defineClass0(jlClassLoader thisClassLoader, string name, byte[] b, int off, int len, ProtectionDomain pd)
			{
				return defineClass1(thisClassLoader, name, b, off, len, pd, null);
			}

			public static jlClass defineClass1(jlClassLoader thisClassLoader, string name, byte[] b, int off, int len, ProtectionDomain pd, string source)
			{
				// it appears the source argument is only used for trace messages in HotSpot. We'll just ignore it for now.
				Profiler.Enter("ClassLoader.defineClass");
				try
				{
					try
					{
						ClassLoaderWrapper classLoaderWrapper = ClassLoaderWrapper.GetClassLoaderWrapper(thisClassLoader);
						ClassFileParseOptions cfp = ClassFileParseOptions.LineNumberTable;
						if (classLoaderWrapper.EmitDebugInfo)
						{
							cfp |= ClassFileParseOptions.LocalVariableTable;
						}
						if (classLoaderWrapper.RelaxedClassNameValidation)
						{
							cfp |= ClassFileParseOptions.RelaxedClassNameValidation;
						}
						ClassFile classFile = new ClassFile(b, off, len, name, cfp);
						if (name != null && classFile.Name != name)
						{
#if !FIRST_PASS
							throw new jlNoClassDefFoundError(name + " (wrong name: " + classFile.Name + ")");
#endif
						}
						TypeWrapper type = classLoaderWrapper.DefineClass(classFile, pd);
						return type.ClassObject;
					}
					catch (RetargetableJavaException x)
					{
						throw x.ToJava();
					}
				}
				finally
				{
					Profiler.Leave("ClassLoader.defineClass");
				}
			}

			public static jlClass defineClass2(jlClassLoader thisClassLoader, string name, jnByteBuffer bb, int off, int len, ProtectionDomain pd, string source)
			{
#if FIRST_PASS
				return null;
#else
				byte[] buf = new byte[bb.remaining()];
				bb.get(buf);
				return defineClass1(thisClassLoader, name, buf, 0, buf.Length, pd, source);
#endif
			}

			public static void resolveClass0(jlClassLoader thisClassLoader, jlClass clazz)
			{
				// no-op
			}

			public static jlClass findBootstrapClass(jlClassLoader thisClassLoader, string name)
			{
#if FIRST_PASS
				return null;
#else
				TypeWrapper tw;
				try
				{
					tw = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedNameFast(name);
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				return tw != null ? tw.ClassObject : null;
#endif
			}

			public static jlClass findLoadedClass0(jlClassLoader thisClassLoader, string name)
			{
				if (name == null)
				{
					return null;
				}
				ClassLoaderWrapper loader = ClassLoaderWrapper.GetClassLoaderWrapper(thisClassLoader);
				TypeWrapper tw = loader.FindLoadedClass(name);
				return tw != null ? tw.ClassObject : null;
			}

			internal static class NativeLibrary
			{
				public static void load(object thisNativeLibrary, string name)
				{
#if !FIRST_PASS
					if (VirtualFileSystem.IsVirtualFS(name))
					{
						// we fake success for native libraries loaded from VFS
						((global::java.lang.ClassLoader.NativeLibrary)thisNativeLibrary).handle = -1;
					}
					else
					{
						doLoad(thisNativeLibrary, name);
					}
#endif
				}

#if !FIRST_PASS
				// we don't want to inline this method, because that would needlessly cause IKVM.Runtime.JNI.dll to be loaded when loading a fake native library from VFS
				[MethodImpl(MethodImplOptions.NoInlining)]
				[global::System.Security.SecuritySafeCritical]
				private static void doLoad(object thisNativeLibrary, string name)
				{
					global::java.lang.ClassLoader.NativeLibrary lib = (global::java.lang.ClassLoader.NativeLibrary)thisNativeLibrary;
					lib.handle = IKVM.Runtime.JniHelper.LoadLibrary(name, TypeWrapper.FromClass(lib.fromClass).GetClassLoader());
				}
#endif

				public static long find(object thisNativeLibrary, string name)
				{
					// TODO
					throw new NotImplementedException();
				}

				[global::System.Security.SecuritySafeCritical]
				public static void unload(object thisNativeLibrary)
				{
#if !FIRST_PASS
					global::java.lang.ClassLoader.NativeLibrary lib = (global::java.lang.ClassLoader.NativeLibrary)thisNativeLibrary;
					long handle = Interlocked.Exchange(ref lib.handle, 0);
					if (handle != 0)
					{
						IKVM.Runtime.JniHelper.UnloadLibrary(handle, TypeWrapper.FromClass(lib.fromClass).GetClassLoader());
					}
#endif
				}
			}

			public static object retrieveDirectives()
			{
				return IKVM.Runtime.Assertions.RetrieveDirectives();
			}
		}

		static class Compiler
		{
			public static void initialize()
			{
			}

			public static void registerNatives()
			{
			}

			public static bool compileClass(object clazz)
			{
				return false;
			}

			public static bool compileClasses(string str)
			{
				return false;
			}

			public static object command(object any)
			{
				return null;
			}

			public static void enable()
			{
			}

			public static void disable()
			{
			}
		}

		static class Double
		{
			public static long doubleToRawLongBits(double value)
			{
				IKVM.Runtime.DoubleConverter converter = new IKVM.Runtime.DoubleConverter();
				return IKVM.Runtime.DoubleConverter.ToLong(value, ref converter);
			}

			public static double longBitsToDouble(long bits)
			{
				IKVM.Runtime.DoubleConverter converter = new IKVM.Runtime.DoubleConverter();
				return IKVM.Runtime.DoubleConverter.ToDouble(bits, ref converter);
			}
		}

		static class Float
		{
			public static int floatToRawIntBits(float value)
			{
				IKVM.Runtime.FloatConverter converter = new IKVM.Runtime.FloatConverter();
				return IKVM.Runtime.FloatConverter.ToInt(value, ref converter);
			}

			public static float intBitsToFloat(int bits)
			{
				IKVM.Runtime.FloatConverter converter = new IKVM.Runtime.FloatConverter();
				return IKVM.Runtime.FloatConverter.ToFloat(bits, ref converter);
			}
		}

		static class LangHelper
		{
			// NOTE the array may contain duplicates!
			public static string[] getBootClassPackages()
			{
				return ClassLoaderWrapper.GetBootstrapClassLoader().GetPackages();
			}
		}

		static class Package
		{
			public static string getSystemPackage0(string name)
			{
				// this method is not implemented because we redirect Package.getSystemPackage() to our implementation in LangHelper
				throw new NotImplementedException();
			}

			public static string[] getSystemPackages0()
			{
				// this method is not implemented because we redirect Package.getSystemPackages() to our implementation in LangHelper
				throw new NotImplementedException();
			}
		}

		static class ProcessEnvironment
		{
			public static string environmentBlock()
			{
				StringBuilder sb = new StringBuilder();
				foreach (global::System.Collections.DictionaryEntry de in Environment.GetEnvironmentVariables())
				{
					sb.Append(de.Key).Append('=').Append(de.Value).Append('\u0000');
				}
				if (sb.Length == 0)
				{
					sb.Append('\u0000');
				}
				sb.Append('\u0000');
				return sb.ToString();
			}
		}

		static class Runtime
		{
			public static int availableProcessors(object thisRuntime)
			{
				return Environment.ProcessorCount;
			}

			public static long freeMemory(object thisRuntime)
			{
				// TODO figure out if there is anything meaningful we can return here
				return 10 * 1024 * 1024;
			}

			public static long totalMemory(object thisRuntime)
			{
				// NOTE this really is a bogus number, but we have to return something
				return GC.GetTotalMemory(false) + freeMemory(thisRuntime);
			}

			public static long maxMemory(object thisRuntime)
			{
				// spec says: If there is no inherent limit then the value Long.MAX_VALUE will be returned.
				return Int64.MaxValue;
			}

			public static void gc(object thisRuntime)
			{
				GC.Collect();
			}

			public static void traceInstructions(object thisRuntime, bool on)
			{
			}

			public static void traceMethodCalls(object thisRuntime, bool on)
			{
			}

			public static void runFinalization0()
			{
				GC.WaitForPendingFinalizers();
			}
		}

		static class SecurityManager
		{
			// this field is set by code in the JNI assembly itself,
			// to prevent having to load the JNI assembly when it isn't used.
			internal static volatile Assembly jniAssembly;

			public static jlClass[] getClassContext(object thisSecurityManager)
			{
#if FIRST_PASS
				return null;
#else
				List<jlClass> stack = new List<jlClass>();
				StackTrace trace = new StackTrace();
				for (int i = 0; i < trace.FrameCount; i++)
				{
					StackFrame frame = trace.GetFrame(i);
					MethodBase method = frame.GetMethod();
					Type type = method.DeclaringType;
					// NOTE these checks should be the same as the ones in Reflection.getCallerClass
					if (IKVM.NativeCode.sun.reflect.Reflection.IsHideFromJava(method)
						|| type == null
						|| type.Assembly == typeof(object).Assembly
						|| type.Assembly == typeof(SecurityManager).Assembly
						|| type.Assembly == jniAssembly
						|| type == typeof(jlrConstructor)
						|| type == typeof(jlrMethod))
					{
						continue;
					}
					if (type == typeof(jlSecurityManager))
					{
						continue;
					}
					stack.Add(ClassLoaderWrapper.GetWrapperFromType(type).ClassObject);
				}
				return stack.ToArray();
#endif
			}

			public static object currentClassLoader0(object thisSecurityManager)
			{
				jlClass currentClass = currentLoadedClass0(thisSecurityManager);
				if (currentClass != null)
				{
					return TypeWrapper.FromClass(currentClass).GetClassLoader().GetJavaClassLoader();
				}
				return null;
			}

			public static int classDepth(object thisSecurityManager, string name)
			{
				throw new NotImplementedException();
			}

			public static int classLoaderDepth0(object thisSecurityManager)
			{
				throw new NotImplementedException();
			}

			public static jlClass currentLoadedClass0(object thisSecurityManager)
			{
				throw new NotImplementedException();
			}
		}

		static class StrictMath
		{
			public static double sin(double d)
			{
#if FIRST_PASS
				return 0;
#else
				return global::ikvm.@internal.JMath.sin(d);
#endif
			}

			public static double cos(double d)
			{
#if FIRST_PASS
				return 0;
#else
				return global::ikvm.@internal.JMath.cos(d);
#endif
			}

			public static double tan(double d)
			{
				return fdlibm.tan(d);
			}

			public static double asin(double d)
			{
#if FIRST_PASS
				return 0;
#else
				return global::ikvm.@internal.JMath.asin(d);
#endif
			}

			public static double acos(double d)
			{
#if FIRST_PASS
				return 0;
#else
				return global::ikvm.@internal.JMath.acos(d);
#endif
			}

			public static double atan(double d)
			{
#if FIRST_PASS
				return 0;
#else
				return global::ikvm.@internal.JMath.atan(d);
#endif
			}

			public static double exp(double d)
			{
#if FIRST_PASS
				return 0;
#else
				return global::ikvm.@internal.JMath.exp(d);
#endif
			}

			public static double log(double d)
			{
				// FPU behavior is correct
				return Math.Log(d);
			}

			public static double log10(double d)
			{
				// FPU behavior is correct
				return Math.Log10(d);
			}

			public static double sqrt(double d)
			{
				// FPU behavior is correct
				return Math.Sqrt(d);
			}

			public static double cbrt(double d)
			{
				return fdlibm.cbrt(d);
			}

			public static double IEEEremainder(double f1, double f2)
			{
#if FIRST_PASS
				return 0;
#else
				return global::ikvm.@internal.JMath.IEEEremainder(f1, f2);
#endif
			}

			public static double ceil(double d)
			{
#if FIRST_PASS
				return 0;
#else
				return global::ikvm.@internal.JMath.ceil(d);
#endif
			}

			public static double floor(double d)
			{
				return fdlibm.floor(d);
			}

			public static double atan2(double y, double x)
			{
#if FIRST_PASS
				return 0;
#else
				return global::ikvm.@internal.JMath.atan2(y, x);
#endif
			}

			public static double pow(double x, double y)
			{
				return fdlibm.__ieee754_pow(x, y);
			}

			public static double sinh(double d)
			{
				return Math.Sinh(d);
			}

			public static double cosh(double d)
			{
				return Math.Cosh(d);
			}

			public static double tanh(double d)
			{
				return Math.Tanh(d);
			}

			public static double rint(double d)
			{
#if FIRST_PASS
				return 0;
#else
				return global::ikvm.@internal.JMath.rint(d);
#endif
			}

			public static double hypot(double a, double b)
			{
				return fdlibm.__ieee754_hypot(a, b);
			}

			public static double expm1(double d)
			{
				return fdlibm.expm1(d);
			}

			public static double log1p(double d)
			{
				return fdlibm.log1p(d);
			}
		}

		static class System
		{
			public static void arraycopy(object src, int srcPos, object dest, int destPos, int length)
			{
				IKVM.Runtime.ByteCodeHelper.arraycopy(src, srcPos, dest, destPos, length);
			}
		}

		static class Thread
		{
			private static readonly object mainThreadGroup;

#if !FIRST_PASS
			static Thread()
			{
				mainThreadGroup = new jlThreadGroup(jlThreadGroup.createRootGroup(), "main");
			}
#endif

			public static object getMainThreadGroup()
			{
				return mainThreadGroup;
			}

			// this is called from JniInterface.cs
			internal static void WaitUntilLastJniThread()
			{
#if !FIRST_PASS
				int count = jlThread.currentThread().isDaemon() ? 0 : 1;
				while (Interlocked.CompareExchange(ref jlThread.nonDaemonCount[0], 0, 0) > count)
				{
					SystemThreadingThread.Sleep(1);
				}
#endif
			}

			// this is called from JniInterface.cs
			internal static void AttachThreadFromJni(object threadGroup)
			{
#if !FIRST_PASS
				if (threadGroup == null)
				{
					threadGroup = mainThreadGroup;
				}
				if (jlThread.current == null)
				{
					new jlThread((jlThreadGroup)threadGroup);
				}
#endif
			}

			public static jlStackTraceElement[] getStackTrace(StackTrace stack)
			{
#if FIRST_PASS
				return null;
#else
				List<jlStackTraceElement> stackTrace = new List<jlStackTraceElement>();
				ExceptionHelper.ExceptionInfoHelper.Append(stackTrace, stack, 0, true);
				return stackTrace.ToArray();
#endif
			}

            public static object getThreads()
            {
#if FIRST_PASS
				return null;
#else
                return global::java.security.AccessController.doPrivileged(global::ikvm.runtime.Delegates.toPrivilegedAction(delegate
                {
                    jlThreadGroup root = (jlThreadGroup)mainThreadGroup;
                    for (; ; )
                    {
                        jlThread[] threads = new jlThread[root.activeCount()];
                        if (root.enumerate(threads) == threads.Length)
                        {
                            return threads;
                        }
                    }
                }));
#endif
            }
			
		}

		static class ProcessImpl
		{
			public static string mapVfsExecutable(string path)
			{
				if (VirtualFileSystem.IsVirtualFS(path))
				{
					return VirtualFileSystem.MapExecutable(path);
				}
				return path;
			}

			public static int parseCommandString(string cmdstr)
			{
				int pos = cmdstr.IndexOf(' ');
				if (pos == -1)
				{
					return cmdstr.Length;
				}
				if (cmdstr[0] == '"')
				{
					int close = cmdstr.IndexOf('"', 1);
					return close == -1 ? cmdstr.Length : close + 1;
				}
				if (Environment.OSVersion.Platform != PlatformID.Win32NT)
				{
					return pos;
				}
				IList<string> path = null;
				for (; ; )
				{
					string str = cmdstr.Substring(0, pos);
					if (global::System.IO.Path.IsPathRooted(str))
					{
						if (Exists(str))
						{
							return pos;
						}
					}
					else
					{
						if (path == null)
						{
							path = GetSearchPath();
						}
						foreach (string p in path)
						{
							if (Exists(global::System.IO.Path.Combine(p, str)))
							{
								return pos;
							}
						}
					}
					if (pos == cmdstr.Length)
					{
						return cmdstr.IndexOf(' ');
					}
					pos = cmdstr.IndexOf(' ', pos + 1);
					if (pos == -1)
					{
						pos = cmdstr.Length;
					}
				}
			}

			private static List<string> GetSearchPath()
			{
				List<string> list = new List<string>();
				list.Add(global::System.IO.Path.GetDirectoryName(global::System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName));
				list.Add(Environment.CurrentDirectory);
				list.Add(Environment.SystemDirectory);
				string windir = global::System.IO.Path.GetDirectoryName(Environment.SystemDirectory);
				list.Add(global::System.IO.Path.Combine(windir, "System"));
				list.Add(windir);
				string path = Environment.GetEnvironmentVariable("PATH");
				if (path != null)
				{
					foreach (string p in path.Split(global::System.IO.Path.PathSeparator))
					{
						list.Add(p);
					}
				}
				return list;
			}

			private static bool Exists(string file)
			{
				try
				{
					if (global::System.IO.File.Exists(file))
					{
						return true;
					}
					else if (global::System.IO.Directory.Exists(file))
					{
						return false;
					}
					else if (file.IndexOf('.') == -1 && global::System.IO.File.Exists(file + ".exe"))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				catch
				{
					return false;
				}
			}
		}

		namespace reflect
		{
			static class Proxy
			{
				public static object defineClass0(jlClassLoader classLoader, string name, byte[] b, int off, int len)
				{
					return ClassLoader.defineClass1(classLoader, name, b, off, len, null, null);
				}

				public static jlClass getPrecompiledProxy(jlClassLoader classLoader, string proxyName, jlClass[] interfaces)
				{
					AssemblyClassLoader acl = ClassLoaderWrapper.GetClassLoaderWrapper(classLoader) as AssemblyClassLoader;
					if (acl == null)
					{
						return null;
					}
					TypeWrapper[] wrappers = new TypeWrapper[interfaces.Length];
					for (int i = 0; i < wrappers.Length; i++)
					{
						wrappers[i] = TypeWrapper.FromClass(interfaces[i]);
					}
					// TODO support multi assembly class loaders
					Type type = acl.MainAssembly.GetType(DynamicClassLoader.GetProxyName(wrappers));
					if (type == null)
					{
						return null;
					}
					TypeWrapper tw = CompiledTypeWrapper.newInstance(proxyName, type);
					TypeWrapper tw2 = acl.RegisterInitiatingLoader(tw);
					if (tw != tw2)
					{
						return null;
					}
					TypeWrapper[] wrappers2 = tw.Interfaces;
					if (wrappers.Length != wrappers.Length)
					{
						return null;
					}
					for (int i = 0; i < wrappers.Length; i++)
					{
						if (wrappers[i] != wrappers2[i])
						{
							return null;
						}
					}
					return tw.ClassObject;
				}
			}

			static class Field
			{
				public static object getDeclaredAnnotationsImpl(object thisField)
				{
					FieldWrapper fw = FieldWrapper.FromField(thisField);
					return Class.AnnotationsToMap(fw.DeclaringType.GetClassLoader(), fw.DeclaringType.GetFieldAnnotations(fw));
				}
			}

			static class Method
			{
				public static object getDeclaredAnnotationsImpl(object methodOrConstructor)
				{
					MethodWrapper mw = MethodWrapper.FromMethodOrConstructor(methodOrConstructor);
					return Class.AnnotationsToMap(mw.DeclaringType.GetClassLoader(), mw.DeclaringType.GetMethodAnnotations(mw));
				}

				public static object[][] getParameterAnnotationsImpl(object methodOrConstructor)
				{
#if FIRST_PASS
					return null;
#else
					MethodWrapper mw = MethodWrapper.FromMethodOrConstructor(methodOrConstructor);
					object[][] objAnn = mw.DeclaringType.GetParameterAnnotations(mw);
					if (objAnn == null)
					{
						return null;
					}
					Annotation[][] ann = new Annotation[objAnn.Length][];
					for (int i = 0; i < ann.Length; i++)
					{
						List<Annotation> list = new List<Annotation>();
						foreach (object obj in objAnn[i])
						{
							Annotation a = obj as Annotation;
							if (a != null)
							{
								list.Add(Class.FreezeOrWrapAttribute(a));
							}
							else if (obj is IKVM.Attributes.DynamicAnnotationAttribute)
							{
								list.Add((Annotation)JVM.NewAnnotation(mw.DeclaringType.GetClassLoader().GetJavaClassLoader(), ((IKVM.Attributes.DynamicAnnotationAttribute)obj).Definition));
							}
						}
						ann[i] = list.ToArray();
					}
					return ann;
#endif
				}

				public static object getDefaultValue(object thisMethod)
				{
					MethodWrapper mw = MethodWrapper.FromMethodOrConstructor(thisMethod);
					return mw.DeclaringType.GetAnnotationDefault(mw);
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
						protection_domain = caller == null ? null : java.lang.Class.getProtectionDomain0(caller);
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
					|| type.Assembly == java.lang.SecurityManager.jniAssembly
					|| type.Assembly == typeof(jlThread).Assembly)
				{
					return null;
				}
				TypeWrapper tw = ClassLoaderWrapper.GetWrapperFromType(type);
				if (tw != null)
				{
					return java.lang.Class.getProtectionDomain0(tw.ClassObject);
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
				if (VirtualFileSystem.CheckAccess(path, IKVM.NativeCode.java.io.Win32FileSystem.ACCESS_READ))
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
					|| type.Assembly == java.lang.SecurityManager.jniAssembly
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
			return java.io.ObjectInputStream.latestUserDefinedLoader();
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