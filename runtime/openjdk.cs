/*
  Copyright (C) 2007 Jeroen Frijters

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
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using StackFrame = System.Diagnostics.StackFrame;
using StackTrace = System.Diagnostics.StackTrace;
using SystemArray = System.Array;
using SystemDouble = System.Double;
using SystemTimeZone = System.TimeZone;
using SystemThreadingThread = System.Threading.Thread;
using SystemThreadingThreadInterruptedException = System.Threading.ThreadInterruptedException;
using SystemThreadingThreadPriority = System.Threading.ThreadPriority;
using IKVM.Internal;
#if !FIRST_PASS
using jlClass = java.lang.Class;
using jlClassLoader = java.lang.ClassLoader;
using jlArrayIndexOutOfBoundsException = java.lang.ArrayIndexOutOfBoundsException;
using jlClassNotFoundException = java.lang.ClassNotFoundException;
using jlIllegalAccessException = java.lang.IllegalAccessException;
using jlIllegalArgumentException = java.lang.IllegalArgumentException;
using jlInterruptedException = java.lang.InterruptedException;
using jlNegativeArraySizeException = java.lang.NegativeArraySizeException;
using jlNoClassDefFoundError = java.lang.NoClassDefFoundError;
using jlNullPointerException = java.lang.NullPointerException;
using jlRunnable = java.lang.Runnable;
using jlSecurityManager = java.lang.SecurityManager;
using jlStackTraceElement = java.lang.StackTraceElement;
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
using jlrConstructor = java.lang.reflect.Constructor;
using jlrMethod = java.lang.reflect.Method;
using jlrField = java.lang.reflect.Field;
using jlrModifier = java.lang.reflect.Modifier;
using ProtectionDomain = java.security.ProtectionDomain;
using srMethodAccessor = sun.reflect.MethodAccessor;
using srConstantPool = sun.reflect.ConstantPool;
using srConstructorAccessor = sun.reflect.ConstructorAccessor;
using srFieldAccessor = sun.reflect.FieldAccessor;
using srLangReflectAccess = sun.reflect.LangReflectAccess;
using srReflection = sun.reflect.Reflection;
using srReflectionFactory = sun.reflect.ReflectionFactory;
using jnByteBuffer = java.nio.ByteBuffer;
using StubGenerator = ikvm.@internal.stubgen.StubGenerator;
using IConstantPoolWriter = ikvm.@internal.stubgen.StubGenerator.IConstantPoolWriter;
using Annotation = java.lang.annotation.Annotation;
using smJavaIOAccess = sun.misc.JavaIOAccess;
using smLauncher = sun.misc.Launcher;
using smSharedSecrets = sun.misc.SharedSecrets;
using jiConsole = java.io.Console;
using jiIOException = java.io.IOException;
using jiFile = java.io.File;
using jnCharset = java.nio.charset.Charset;
using juProperties = java.util.Properties;
using gcSystemProperties = gnu.classpath.SystemProperties;
using irUtil = ikvm.runtime.Util;
using jsDriverManager = java.sql.DriverManager;
#endif

namespace IKVM.NativeCode.java
{
	namespace io
	{
		public sealed class Console
		{
			public static string encoding()
			{
				// TODO
				return "IBM437";
			}

			public static bool echo(bool on)
			{
				// TODO
				return false;
			}

			public static bool istty()
			{
				// the JDK returns false here if stdin or stdout is redirected (not stderr)
				// or if there is no console associated with the current process
				// TODO figure out if there is a managed way to detect redirection or console presence
				return true;
			}
		}

		public sealed class FileSystem
		{
			public static object getFileSystem()
			{
#if FIRST_PASS
				return null;
#else
				if (Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32Windows)
				{
					return Activator.CreateInstance(typeof(jlClass).Assembly.GetType("java.io.Win32FileSystem"), true);
				}
				else
				{
					return Activator.CreateInstance(typeof(jlClass).Assembly.GetType("java.io.UnixFileSystem"), true);
				}
#endif
			}
		}

		public sealed class ObjectInputStream
		{
			public static void bytesToFloats(byte[] src, int srcpos, float[] dst, int dstpos, int nfloats)
			{
				while (nfloats-- > 0)
				{
					dst[dstpos++] = BitConverter.ToSingle(src, srcpos);
					srcpos += 4;
				}
			}

			public static void bytesToDoubles(byte[] src, int srcpos, double[] dst, int dstpos, int ndoubles)
			{
				while (ndoubles-- > 0)
				{
					dst[dstpos++] = BitConverter.ToDouble(src, srcpos);
					srcpos += 4;
				}
			}

			public static object latestUserDefinedLoader()
			{
				for (int i = 1; ; i++)
				{
					StackFrame frame = new StackFrame(i);
					MethodBase method = frame.GetMethod();
					if (method == null)
					{
						return null;
					}
					Type type = method.DeclaringType;
					if (type != null)
					{
						TypeWrapper tw = ClassLoaderWrapper.GetWrapperFromType(type);
						if (tw != null)
						{
							object javaClassLoader = tw.GetClassLoader().GetJavaClassLoader();
							if (javaClassLoader != null)
							{
								return javaClassLoader;
							}
						}
					}
				}
			}
		}

		public sealed class ObjectOutputStream
		{
			public static void floatsToBytes(float[] src, int srcpos, byte[] dst, int dstpos, int nfloats)
			{
				while (nfloats-- > 0)
				{
					Buffer.BlockCopy(BitConverter.GetBytes(src[srcpos++]), 0, dst, dstpos, 4);
					dstpos += 4;
				}
			}

			public static void doublesToBytes(double[] src, int srcpos, byte[] dst, int dstpos, int ndoubles)
			{
				while (ndoubles-- > 0)
				{
					Buffer.BlockCopy(BitConverter.GetBytes(src[srcpos++]), 0, dst, dstpos, 8);
					dstpos += 8;
				}
			}
		}

		public sealed class ObjectStreamClass
		{
			public static void initNative()
			{
			}

			public static bool hasStaticInitializer(object cl)
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
		}

		public sealed class Win32FileSystem
		{
			const int ACCESS_READ = 0x04;
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

			private static System.IO.FileInfo GetFileInfo(string path)
			{
#if FIRST_PASS
				return null;
#else
				try
				{
					return new System.IO.FileInfo(path);
				}
				catch(System.Security.SecurityException x1)
				{
					throw new jiIOException(x1.Message);
				}
				catch(System.ArgumentException x2)
				{
					throw new jiIOException(x2.Message);
				}
				catch(System.UnauthorizedAccessException x3)
				{
					throw new jiIOException(x3.Message);
				}
				catch(System.IO.IOException x4)
				{
					throw new jiIOException(x4.Message);
				}
				catch(System.NotSupportedException x5)
				{
					throw new jiIOException(x5.Message);
				}
#endif
			}

			public static string canonicalize0(object _this, string path)
			{
				return GetFileInfo(path).FullName;
			}

			public static string canonicalizeWithPrefix0(object _this, string canonicalPrefix, string pathWithCanonicalPrefix)
			{
				// TODO what's this all about?
				return GetFileInfo(pathWithCanonicalPrefix).FullName;
			}

			private static string GetPathFromFile(object file)
			{
#if FIRST_PASS
				return null;
#else
				return ((jiFile)file).getPath();
#endif
			}

			public static int getBooleanAttributes(object _this, object f)
			{
				try
				{
					System.IO.FileAttributes attr = System.IO.File.GetAttributes(GetPathFromFile(f));
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

			public static bool checkAccess(object _this, object f, int access)
			{
				string path = GetPathFromFile(f);
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
						System.IO.FileInfo fileInfo = new System.IO.FileInfo(path);
						// Like the JDK we'll only look at the read-only attribute and not
						// the security permissions associated with the file or directory.
						ok = (fileInfo.Attributes & System.IO.FileAttributes.ReadOnly) == 0;
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

			public static long getLastModifiedTime(object _this, object f)
			{
				try
				{
					return DateTimeToJavaLongTime(System.IO.File.GetLastWriteTime(GetPathFromFile(f)));
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

			public static long getLength(object _this, object f)
			{
				try
				{
					return new System.IO.FileInfo(GetPathFromFile(f)).Length;
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

			public static bool setPermission(object _this, object f, int access, bool enable, bool owneronly)
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
					throw new jiIOException(x.Message);
				}
				catch (System.NotSupportedException x)
				{
					throw new jiIOException(x.Message);
				}
#endif
				return false;
			}

			public static bool delete0(object _this, object f)
			{
				try
				{
					string path = GetPathFromFile(f);
					System.IO.FileSystemInfo fileInfo;
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

			public static string[] list(object _this, object f)
			{
				try
				{
					string[] l = System.IO.Directory.GetFileSystemEntries(GetPathFromFile(f));
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
				return null;
			}

			public static bool createDirectory(object _this, object f)
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

			public static bool rename0(object _this, object f1, object f2)
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

			public static bool setLastModifiedTime(object _this, object f, long time)
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

			public static bool setReadOnly(object _this, object f)
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
					foreach (string drive in System.IO.Directory.GetLogicalDrives())
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
				return 0;
			}

			public static long getSpace0(object _this, object f, int t)
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

		public sealed class UnixFileSystem
		{
			public static int getBooleanAttributes0(object _this, object f)
			{
				return Win32FileSystem.getBooleanAttributes(_this, f);
			}

			public static long getSpace(object _this, object f, int t)
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

			public static bool checkAccess(object _this, object f, int access)
			{
				return Win32FileSystem.checkAccess(_this, f, access);
			}

			public static long getLastModifiedTime(object _this, object f)
			{
				return Win32FileSystem.getLastModifiedTime(_this, f);
			}

			public static long getLength(object _this, object f)
			{
				return Win32FileSystem.getLength(_this, f);
			}

			public static bool setPermission(object _this, object f, int access, bool enable, bool owneronly)
			{
				// TODO consider using Mono.Posix
				return Win32FileSystem.setPermission(_this, f, access, enable, owneronly);
			}

			public static bool createFileExclusively(object _this, string path)
			{
				return Win32FileSystem.createFileExclusively(_this, path);
			}

			public static bool delete0(object _this, object f)
			{
				return Win32FileSystem.delete0(_this, f);
			}

			public static string[] list(object _this, object f)
			{
				return Win32FileSystem.list(_this, f);
			}

			public static bool createDirectory(object _this, object f)
			{
				return Win32FileSystem.createDirectory(_this, f);
			}

			public static bool rename0(object _this, object f1, object f2)
			{
				return Win32FileSystem.rename0(_this, f1, f2);
			}

			public static bool setLastModifiedTime(object _this, object f, long time)
			{
				return Win32FileSystem.setLastModifiedTime(_this, f, time);
			}

			public static bool setReadOnly(object _this, object f)
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
		namespace reflect
		{
			public sealed class Array
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

			public static object newArray(object componentType, int length)
			{
				return null;
			}

			public static object multiNewArray(object componentType, int[] dimensions)
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

				public static object newArray(object componentType, int length)
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
						return SystemArray.CreateInstance(wrapper.TypeAsArrayType, length);
					}
					catch (RetargetableJavaException x)
					{
						throw x.ToJava();
					}
				}

				public static object multiNewArray(object componentType, int[] dimensions)
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
						TypeWrapper wrapper = TypeWrapper.FromClass(componentType);
						wrapper.Finish();
						return IKVM.Runtime.ByteCodeHelper.multianewarray(wrapper.MakeArrayType(dimensions.Length).TypeAsArrayType.TypeHandle, dimensions);
					}
					catch (RetargetableJavaException x)
					{
						throw x.ToJava();
					}
				}
#endif // FIRST_PASS
			}
		}

#if !FIRST_PASS
		sealed class ConstantPoolWriter : IConstantPoolWriter
		{
			private ArrayList items = new ArrayList();

			public short AddUtf8(String str)
			{
				return (short)items.Add(str);
			}

			public short AddInt(int i)
			{
				return (short)items.Add(i);
			}

			public short AddLong(long l)
			{
				return (short)items.Add(l);
			}

			public short AddFloat(float f)
			{
				return (short)items.Add(f);
			}

			public short AddDouble(double d)
			{
				return (short)items.Add(d);
			}

			internal string GetUtf8(int index)
			{
				return (string)items[index];
			}

			internal int GetInt(int index)
			{
				return (int)items[index];
			}

			internal long GetLong(int index)
			{
				return (long)items[index];
			}

			internal float GetFloat(int index)
			{
				return (float)items[index];
			}

			internal double GetDouble(int index)
			{
				return (double)items[index];
			}
		}
#endif

		public sealed class Class
		{
			private Class() { }

			private static FieldInfo signersField;
			private static FieldInfo pdField;
			private static FieldInfo constantPoolField;
			private static FieldInfo constantPoolOopField;

			public static void registerNatives()
			{
#if !FIRST_PASS
				signersField = typeof(jlClass).GetField("signers", BindingFlags.Instance | BindingFlags.NonPublic);
				pdField = typeof(jlClass).GetField("pd", BindingFlags.Instance | BindingFlags.NonPublic);
				constantPoolField = typeof(jlClass).GetField("constantPool", BindingFlags.Instance | BindingFlags.NonPublic);
				constantPoolOopField = typeof(srConstantPool).GetField("constantPoolOop", BindingFlags.Instance | BindingFlags.NonPublic);
#endif
			}

			public static object forName0(string name, bool initialize, object loader)
			{
#if FIRST_PASS
				return null;
#else
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
						tw = DotNetTypeWrapper.GetWrapperFromDotNetType(type);
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
					catch (RetargetableJavaException x)
					{
						throw x.ToJava();
					}
				}
				if (initialize && !tw.IsArray)
				{
					tw.Finish();
					tw.RunClassInit();
				}
				return tw.ClassObject;
#endif
			}

			public static bool isInstance(object thisClass, object obj)
			{
				return TypeWrapper.FromClass(thisClass).IsInstance(obj);
			}

			public static bool isAssignableFrom(object thisClass, object otherClass)
			{
#if !FIRST_PASS
				if (otherClass == null)
				{
					throw new jlNullPointerException();
				}
#endif
				return TypeWrapper.FromClass(otherClass).IsAssignableTo(TypeWrapper.FromClass(thisClass));
			}

			public static bool isInterface(object thisClass)
			{
				return TypeWrapper.FromClass(thisClass).IsInterface;
			}

			public static bool isArray(object thisClass)
			{
				return TypeWrapper.FromClass(thisClass).IsArray;
			}

			public static bool isPrimitive(object thisClass)
			{
				return TypeWrapper.FromClass(thisClass).IsPrimitive;
			}

			public static string getName0(object thisClass)
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

			public static object getClassLoader0(object thisClass)
			{
				return TypeWrapper.FromClass(thisClass).GetClassLoader().GetJavaClassLoader();
			}

			public static object getSuperclass(object thisClass)
			{
				TypeWrapper super = TypeWrapper.FromClass(thisClass).BaseTypeWrapper;
				return super != null ? super.ClassObject : null;
			}

			public static object getInterfaces(object thisClass)
			{
#if FIRST_PASS
				return null;
#else
				TypeWrapper[] ifaces = TypeWrapper.FromClass(thisClass).Interfaces;
				jlClass[] interfaces = new jlClass[ifaces.Length];
				for (int i = 0; i < ifaces.Length; i++)
				{
					interfaces[i] = (jlClass)ifaces[i].ClassObject;
				}
				return interfaces;
#endif
			}

			public static object getComponentType(object thisClass)
			{
				TypeWrapper tw = TypeWrapper.FromClass(thisClass);
				return tw.IsArray ? tw.ElementTypeWrapper.ClassObject : null;
			}

			public static int getModifiers(object thisClass)
			{
				return (int)TypeWrapper.FromClass(thisClass).ReflectiveModifiers;
			}

			public static object[] getSigners(object thisClass)
			{
				return (object[])signersField.GetValue(thisClass);
			}

			public static void setSigners(object thisClass, object[] signers)
			{
				signersField.SetValue(thisClass, signers);
			}

			public static object[] getEnclosingMethod0(object thisClass)
			{
				TypeWrapper tw = TypeWrapper.FromClass(thisClass);
				string[] enc = tw.GetEnclosingMethod();
				if (enc == null)
				{
					return null;
				}
				try
				{
					return new object[] { tw.GetClassLoader().LoadClassByDottedName(enc[0]).ClassObject, enc[1], enc[2] == null ? null : enc[2].Replace('.', '/') };
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
			}

			public static object getDeclaringClass(object thisClass)
			{
				try
				{
					TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
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

			public static object getProtectionDomain0(object thisClass)
			{
				TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
				while (wrapper.IsArray)
				{
					wrapper = wrapper.ElementTypeWrapper;
				}
				object pd = pdField.GetValue(wrapper.ClassObject);
				if (pd == null && wrapper.GetClassLoader() is AssemblyClassLoader)
				{
					object loader = wrapper.GetClassLoader().GetJavaClassLoader();
					if (loader != null)
					{
						// The protection domain for statically compiled code is created lazily (not at java.lang.Class creation time),
						// to work around boot strap issues.
						// TODO this should be done more efficiently
						MethodInfo method = loader.GetType().GetMethod("getProtectionDomain", BindingFlags.NonPublic | BindingFlags.Instance);
						if (method != null)
						{
							pd = method.Invoke(loader, null);
						}
					}
				}
				return pd;
			}

			public static void setProtectionDomain0(object thisClass, object pd)
			{
				pdField.SetValue(thisClass, pd);
			}

			public static object getPrimitiveClass(string name)
			{
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

			public static string getGenericSignature(object thisClass)
			{
				string sig = TypeWrapper.FromClass(thisClass).GetGenericSignature();
				if (sig == null)
				{
					return null;
				}
				return sig.Replace('.', '/');
			}

			public static byte[] getRawAnnotations(object thisClass)
			{
#if FIRST_PASS
				return null;
#else
				TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
				wrapper.Finish();
				object[] objAnn = wrapper.GetDeclaredAnnotations();
				if (objAnn == null)
				{
					return null;
				}
				ArrayList ann = new ArrayList();
				foreach (object obj in objAnn)
				{
					if (obj is Annotation)
					{
						ann.Add(obj);
					}
				}
				IConstantPoolWriter cp = (IConstantPoolWriter)constantPoolOopField.GetValue(getConstantPool(thisClass));
				return StubGenerator.writeAnnotations(cp, (Annotation[])ann.ToArray(typeof(Annotation)));
#endif
			}

#if !FIRST_PASS
			internal static IConstantPoolWriter GetConstantPoolWriter(TypeWrapper wrapper)
			{
				return (IConstantPoolWriter)constantPoolOopField.GetValue(getConstantPool(wrapper.ClassObject));
			}
#endif

			public static object getConstantPool(object thisClass)
			{
#if FIRST_PASS
				return null;
#else
				lock (thisClass)
				{
					object cp = constantPoolField.GetValue(thisClass);
					if (cp == null)
					{
						cp = new srConstantPool();
						constantPoolOopField.SetValue(cp, new ConstantPoolWriter());
						constantPoolField.SetValue(thisClass, cp);
					}
					return cp;
				}
#endif
			}

			public static object getDeclaredFields0(object thisClass, bool publicOnly)
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
					ArrayList list = new ArrayList();
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
							fields[i].FieldTypeWrapper.EnsureLoadable(wrapper.GetClassLoader());
							list.Add(fields[i].ToField(false));
						}
					}
					return (jlrField[])list.ToArray(typeof(jlrField));
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

			public static object getDeclaredMethods0(object thisClass, bool publicOnly)
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
					// we need to look through the array for unloadable types, because we may not let them
					// escape into the 'wild'
					MethodWrapper[] methods = wrapper.GetMethods();
					ArrayList list = new ArrayList();
					for (int i = 0; i < methods.Length; i++)
					{
						// we don't want to expose "hideFromReflection" methods (one reason is that it would
						// mess up the serialVersionUID computation)
						if (!methods[i].IsHideFromReflection
							&& methods[i].Name != "<clinit>" && methods[i].Name != "<init>"
							&& (!publicOnly || methods[i].IsPublic))
						{
							methods[i].ReturnType.EnsureLoadable(wrapper.GetClassLoader());
							TypeWrapper[] args = methods[i].GetParameters();
							for (int j = 0; j < args.Length; j++)
							{
								args[j].EnsureLoadable(wrapper.GetClassLoader());
							}
							list.Add(methods[i].ToMethodOrConstructor(false));
						}
					}
					return (jlrMethod[])list.ToArray(typeof(jlrMethod));
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

			public static object getDeclaredConstructors0(object thisClass, bool publicOnly)
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
					// we need to look through the array for unloadable types, because we may not let them
					// escape into the 'wild'
					MethodWrapper[] methods = wrapper.GetMethods();
					ArrayList list = new ArrayList();
					for (int i = 0; i < methods.Length; i++)
					{
						// we don't want to expose "hideFromReflection" methods (one reason is that it would
						// mess up the serialVersionUID computation)
						if (!methods[i].IsHideFromReflection
							&& methods[i].Name == "<init>"
							&& (!publicOnly || methods[i].IsPublic))
						{
							TypeWrapper[] args = methods[i].GetParameters();
							for (int j = 0; j < args.Length; j++)
							{
								args[j].EnsureLoadable(wrapper.GetClassLoader());
							}
							list.Add(methods[i].ToMethodOrConstructor(false));
						}
					}
					return (jlrConstructor[])list.ToArray(typeof(jlrConstructor));
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

			public static object getDeclaredClasses0(object thisClass)
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
						wrappers[i].Finish();
						if (wrappers[i].IsUnloadable)
						{
							throw new jlNoClassDefFoundError(wrappers[i].Name);
						}
						if (!wrappers[i].IsAccessibleFrom(wrapper))
						{
							throw new IllegalAccessError(string.Format("tried to access class {0} from class {1}", wrappers[i].Name, wrapper.Name));
						}
						innerclasses[i] = (jlClass)wrappers[i].ClassObject;
					}
					return innerclasses;
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
#endif
			}

			public static bool desiredAssertionStatus0(object clazz)
			{
				// TODO
				return false;
			}
		}

		public sealed class ClassLoader
		{
#if !FIRST_PASS
			private static jlClassNotFoundException classNotFoundException;
#endif

			private ClassLoader() { }

			public static void registerNatives()
			{
				Thread.Bootstrap();
			}

			public static object defineClass0(object thisClassLoader, string name, byte[] b, int off, int len, object pd)
			{
				return defineClass1(thisClassLoader, name, b, off, len, pd, null);
			}

			public static object defineClass1(object thisClassLoader, string name, byte[] b, int off, int len, object pd, string source)
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

			public static object defineClass2(object thisClassLoader, string name, object b, int off, int len, object pd, string source)
			{
#if FIRST_PASS
				return null;
#else
				jnByteBuffer bb = (jnByteBuffer)b;
				byte[] buf = new byte[bb.remaining()];
				bb.get(buf);
				return defineClass1(thisClassLoader, name, buf, 0, buf.Length, pd, source);
#endif
			}

			public static void resolveClass0(object thisClassLoader, object clazz)
			{
				// no-op
			}

			public static object findBootstrapClass(object thisClassLoader, string name)
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
				if (tw == null)
				{
					// HACK for efficiency, we don't allocate a new exception here
					// (as this exception is thrown for *every* non-boot class that we load and
					// the exception is thrown away by our caller anyway)
					if (classNotFoundException == null)
					{
						jlClassNotFoundException ex = new jlClassNotFoundException(null, null);
						ex.setStackTrace(new jlStackTraceElement[] { new jlStackTraceElement("java.lang.ClassLoader", "findBootstrapClass", null, -2) });
						classNotFoundException = ex;
					}
					throw classNotFoundException;
				}
				return tw.ClassObject;
#endif
			}

			public static object findLoadedClass0(object thisClassLoader, string name)
			{
				ClassLoaderWrapper loader = ClassLoaderWrapper.GetClassLoaderWrapper(thisClassLoader);
				TypeWrapper tw = loader.GetLoadedClass(name);
				return tw != null ? tw.ClassObject : null;
			}

			public sealed class NativeLibrary
			{
				private NativeLibrary() { }

				public static void load(object thisNativeLibrary, string name)
				{
					object fromClass = thisNativeLibrary.GetType().GetField("fromClass", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(thisNativeLibrary);
					if (IKVM.Runtime.JniHelper.LoadLibrary(name, TypeWrapper.FromClass(fromClass).GetClassLoader()) == 1)
					{
						SetHandle(thisNativeLibrary, -1);
					}
				}

				private static void SetHandle(object thisNativeLibrary, long handle)
				{
					thisNativeLibrary.GetType().GetField("handle", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(thisNativeLibrary, handle);
				}

				public static long find(object thisNativeLibrary, string name)
				{
					// TODO
					throw new NotImplementedException();
				}

				public static void unload(object thisNativeLibrary)
				{
					// TODO
					throw new NotImplementedException();
				}
			}

			public static object retrieveDirectives()
			{
				// TODO
				throw new NotImplementedException();
			}
		}

		public sealed class Compiler
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

		public sealed class Double
		{
			public static long doubleToRawLongBits(double value)
			{
				return BitConverter.DoubleToInt64Bits(value);
			}

			public static double longBitsToDouble(long bits)
			{
				return BitConverter.Int64BitsToDouble(bits);
			}
		}

		public sealed class Float
		{
			public static int floatToRawIntBits(float value)
			{
				return BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
			}

			public static float intBitsToFloat(int bits)
			{
				return BitConverter.ToSingle(BitConverter.GetBytes(bits), 0);
			}
		}

		public sealed class Package
		{
			private Package() { }

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

		public sealed class ProcessEnvironment
		{
			public static string environmentBlock()
			{
				StringBuilder sb = new StringBuilder();
				foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
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

		public sealed class Runtime
		{
			public static int availableProcessors(object thisRuntime)
			{
				string s = JVM.SafeGetEnvironmentVariable("NUMBER_OF_PROCESSORS");
				if (s != null)
				{
					try
					{
						return Int32.Parse(s, NumberFormatInfo.InvariantInfo);
					}
					catch (FormatException)
					{
					}
					catch (OverflowException)
					{
					}
				}
				return 1;
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

		public sealed class SecurityManager
		{
			public static object getClassContext(object thisSecurityManager)
			{
#if FIRST_PASS
				return null;
#else
				ArrayList stack = new ArrayList();
				StackTrace trace = new StackTrace();
				for (int i = 0; i < trace.FrameCount; i++)
				{
					StackFrame frame = trace.GetFrame(i);
					MethodBase method = frame.GetMethod();
					Type type = method.DeclaringType;
					// NOTE these checks should be the same as the ones in Reflection.getCallerClass
					if (IKVM.NativeCode.gnu.classpath.VMStackWalker.isHideFromJava(method)
						|| type == null
						|| type.Assembly == typeof(object).Assembly
						|| type.Assembly == typeof(SecurityManager).Assembly
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
				return stack.ToArray(typeof(jlClass));
#endif
			}

			public static object currentClassLoader0(object thisSecurityManager)
			{
				object currentClass = currentLoadedClass0(thisSecurityManager);
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

			public static object currentLoadedClass0(object thisSecurityManager)
			{
				throw new NotImplementedException();
			}
		}

		public sealed class Shutdown
		{
			public static void halt0(int status)
			{
				Environment.Exit(status);
			}

			// runAllFinalizers() implementation lives in map.xml
		}

		public sealed class StrictMath
		{
			public static double sin(double d)
			{
				return Math.Sin(d);
			}

			public static double cos(double d)
			{
				return Math.Cos(d);
			}

			public static double tan(double d)
			{
				return Math.Tan(d);
			}

			public static double asin(double d)
			{
				return Math.Asin(d);
			}

			public static double acos(double d)
			{
				return Math.Acos(d);
			}

			public static double atan(double d)
			{
				return Math.Atan(d);
			}

			public static double exp(double d)
			{
				return Math.Exp(d);
			}

			public static double log(double d)
			{
				return Math.Log(d);
			}

			public static double log10(double d)
			{
				return Math.Log10(d);
			}

			public static double sqrt(double d)
			{
				return Math.Sqrt(d);
			}

			public static double cbrt(double d)
			{
				return Math.Pow(d, 1.0 / 3.0);
			}

			public static double IEEEremainder(double f1, double f2)
			{
				if (SystemDouble.IsInfinity(f2) && !SystemDouble.IsInfinity(f1))
				{
					return f1;
				}
				return Math.IEEERemainder(f1, f2);
			}

			public static double ceil(double d)
			{
				return Math.Ceiling(d);
			}

			public static double floor(double d)
			{
				return Math.Floor(d);
			}

			public static double atan2(double y, double x)
			{
				if (SystemDouble.IsInfinity(y) && SystemDouble.IsInfinity(x))
				{
					if (SystemDouble.IsPositiveInfinity(y))
					{
						if (SystemDouble.IsPositiveInfinity(x))
						{
							return Math.PI / 4.0;
						}
						else
						{
							return Math.PI * 3.0 / 4.0;
						}
					}
					else
					{
						if (SystemDouble.IsPositiveInfinity(x))
						{
							return -Math.PI / 4.0;
						}
						else
						{
							return -Math.PI * 3.0 / 4.0;
						}
					}
				}
				return Math.Atan2(y, x);
			}

			public static double pow(double x, double y)
			{
				if (Math.Abs(x) == 1.0 && SystemDouble.IsInfinity(y))
				{
					return SystemDouble.NaN;
				}
				return Math.Pow(x, y);
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
				return Math.Round(d);
			}

			public static double hypot(double a, double b)
			{
				return a * a + b * b;
			}

			public static double expm1(double d)
			{
				return Math.Exp(d) - 1.0;
			}

			public static double log1p(double d)
			{
				return Math.Log(d + 1.0);
			}
		}

		public sealed class System
		{
			public static void arraycopy(object src, int srcPos, object dest, int destPos, int length)
			{
				IKVM.Runtime.ByteCodeHelper.arraycopy(src, srcPos, dest, destPos, length);
			}

			// FXBUG this is implemented by a non-virtual call to System.Object.GetHashCode (in map.xml),
			// because RuntimeHelpers.GetHashCode is broken (in v1.x) when called in a secondary AppDomain.
			// See http://weblog.ikvm.net/PermaLink.aspx?guid=c2442bc8-7b48-4570-b082-82649cc347dc
			//
			// public static int identityHashCode(object obj)

			public static long currentTimeMillis()
			{
				const long january_1st_1970 = 62135596800000L;
				return DateTime.UtcNow.Ticks / 10000L - january_1st_1970;
			}

			public static long nanoTime()
			{
				// Note that the epoch is undefined, but we use something similar to the JDK
				const long epoch = 632785401332600000L;
				return (DateTime.UtcNow.Ticks - epoch) * 100L;
			}

			public static string mapLibraryName(string libname)
			{
#if FIRST_PASS
				return null;
#else
				// TODO instead of using the System property, we should use
				// a VM level shared variable that contains the os
				// (and that os.name is defined by)
				string osname = jlSystem.getProperty("os.name");
				if (osname == null)
				{
					return libname;
				}
				else if (osname.IndexOf("Windows") >= 0)
				{
					return libname + ".dll";
				}
				else if (osname == "Mac OS X")
				{
					return "lib" + libname + ".jnilib";
				}
				else
				{
					return "lib" + libname + ".so";
				}
#endif
			}

			public static void registerNatives()
			{
				Thread.Bootstrap();
			}

			public static void setIn0(object @in)
			{
				SetSystemField("in", @in);
			}

			public static void setOut0(object @out)
			{
				SetSystemField("out", @out);
			}

			public static void setErr0(object err)
			{
				SetSystemField("err", err);
			}

			private static void SetSystemField(string field, object obj)
			{
#if !FIRST_PASS
				// MONOBUG due to a bug in mcs we currently prefix the backing fields with __<>
				field = "__<>" + field;
				typeof(jlSystem)
					.GetField(field, BindingFlags.NonPublic | BindingFlags.Static)
					.SetValue(null, obj);
#endif
			}

			public static object initProperties(object props)
			{
#if !FIRST_PASS
				juProperties p1 = gcSystemProperties.getProperties();
				juProperties p = (juProperties)props;
				foreach (string key in (IEnumerable)p1.keySet())
				{
					p.put(key, p1.getProperty(key));
				}
#endif
				return props;
			}
		}

		public sealed class Thread
		{
			[ThreadStatic]
			private static VMThread vmThread;
			[ThreadStatic]
			private static object cleanup;
			private static readonly ConstructorInfo threadConstructor1;
			private static readonly ConstructorInfo threadConstructor2;
			private static readonly FieldInfo vmThreadField;
			private static readonly MethodInfo threadGroupAddMethod;
			private static readonly FieldInfo threadStatusField;
			private static readonly FieldInfo daemonField;
			private static readonly FieldInfo threadPriorityField;
			private static readonly MethodInfo threadExitMethod;
			private static readonly object mainThreadGroup;
			// we don't really use the Thread.threadStatus field, but we have to set it to a non-zero value,
			// so we use RUNNABLE (which the HotSpot also uses) and the value was taken from the
			// ThreadStatus enum in /openjdk/hotspot/src/share/vm/memory/javaClasses.hpp
			private const int JVMTI_THREAD_STATE_ALIVE = 0x0001;
			private const int JVMTI_THREAD_STATE_RUNNABLE = 0x0004;
			private const int JVMTI_THREAD_STATE_TERMINATED = 0x0002;
			private const int NEW = 0;
			private const int RUNNABLE = JVMTI_THREAD_STATE_ALIVE + JVMTI_THREAD_STATE_RUNNABLE;
			private const int TERMINATED = JVMTI_THREAD_STATE_TERMINATED;

			private sealed class VMThread
			{
				internal readonly SystemThreadingThread nativeThread;
#if !FIRST_PASS
				internal readonly jlThread javaThread;
#endif
				internal Exception stillborn;
				internal bool running;
				private bool interruptPending;
				private bool interruptableWait;
				private bool timedWait;

#if !FIRST_PASS
				internal VMThread(jlThread javaThread, SystemThreadingThread nativeThread)
				{
					this.javaThread = javaThread;
					this.nativeThread = nativeThread;
				}

				internal VMThread(jlThread javaThread)
				{
					this.javaThread = javaThread;
					nativeThread = new SystemThreadingThread(new ThreadStart(ThreadProc));
				}
#endif

				internal bool IsInterruptPending(bool clearInterrupt)
				{
					lock (this)
					{
						bool b = interruptPending;
						if (clearInterrupt)
						{
							interruptPending = false;
						}
						return b;
					}
				}

#if !FIRST_PASS
				internal jlThread.State GetState()
				{
					switch (GetThreadStatus(javaThread))
					{
						case NEW:
							return jlThread.State.NEW;
						case TERMINATED:
							return jlThread.State.TERMINATED;
					}
					lock (this)
					{
						if (interruptableWait)
						{
							// NOTE if objectWait has satisfied the wait condition (or has been interrupted or has timed-out),
							// it can be blocking on the re-acquire of the monitor, but we have no way of detecting that.
							return timedWait ? jlThread.State.TIMED_WAITING : jlThread.State.WAITING;
						}
						if ((nativeThread.ThreadState & ThreadState.WaitSleepJoin) != 0)
						{
							return jlThread.State.BLOCKED;
						}
					}
					return jlThread.State.RUNNABLE;
				}
#endif

				internal void EnterInterruptableWait(bool timedWait)
				{
#if !FIRST_PASS
					lock (this)
					{
						if (interruptPending)
						{
							interruptPending = false;
							throw new jlInterruptedException();
						}
						interruptableWait = true;
						this.timedWait = timedWait;
					}
#endif
				}

				internal void LeaveInterruptableWait()
				{
#if !FIRST_PASS
					SystemThreadingThreadInterruptedException dotnetInterrupt = null;
					for (; ; )
					{
						try
						{
							lock (this)
							{
								interruptableWait = false;
								if (interruptPending)
								{
									interruptPending = false;
									throw new jlInterruptedException();
								}
							}
							break;
						}
						catch (SystemThreadingThreadInterruptedException x)
						{
							dotnetInterrupt = x;
						}
					}
					if (dotnetInterrupt != null)
					{
						throw dotnetInterrupt;
					}
#endif
				}

				internal void Interrupt()
				{
					lock (this)
					{
						interruptPending = true;
						if (interruptableWait)
						{
							nativeThread.Interrupt();
						}
					}
				}

				internal void ThreadProc()
				{
#if !FIRST_PASS
					vmThread = this;
					try
					{
						lock (javaThread)
						{
							running = true;
							Exception x = stillborn;
							if (x != null)
							{
								stillborn = null;
								throw x;
							}
						}
						javaThread.run();
					}
					catch (Exception x)
					{
						try
						{
							javaThread.getUncaughtExceptionHandler().uncaughtException(javaThread, irUtil.mapException(x));
						}
						catch
						{
						}
					}
					finally
					{
						DetachThread();
					}
#endif
				}
			}

			private sealed class Cleanup
			{
				private object javaThread;

				internal Cleanup(object javaThread)
				{
					this.javaThread = javaThread;
				}

				~Cleanup()
				{
					SetThreadStatus(javaThread, TERMINATED);
				}
			}

#if !FIRST_PASS
			static Thread()
			{
				threadConstructor1 = typeof(jlThread).GetConstructor(new Type[] { typeof(jlThreadGroup), typeof(string) });
				threadConstructor2 = typeof(jlThread).GetConstructor(new Type[] { typeof(jlThreadGroup), typeof(jlRunnable) });
				vmThreadField = typeof(jlThread).GetField("vmThread", BindingFlags.Instance | BindingFlags.NonPublic);
				threadGroupAddMethod = typeof(jlThreadGroup).GetMethod("add", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(jlThread) }, null);
				threadStatusField = typeof(jlThread).GetField("threadStatus", BindingFlags.Instance | BindingFlags.NonPublic);
				daemonField = typeof(jlThread).GetField("daemon", BindingFlags.Instance | BindingFlags.NonPublic);
				threadPriorityField = typeof(jlThread).GetField("priority", BindingFlags.Instance | BindingFlags.NonPublic);
				threadExitMethod = typeof(jlThread).GetMethod("exit", BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);

				jlThreadGroup systemThreadGroup = (jlThreadGroup)Activator.CreateInstance(typeof(jlThreadGroup), true);
				mainThreadGroup = new jlThreadGroup(systemThreadGroup, "main");
				AttachThread(null, false, null);
				typeof(jlSystem).GetMethod("initializeSystemClass", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
				// make sure the Launcher singleton is created on the main thread and allow it to install the security manager
				smLauncher.getLauncher();
				if (jlClassLoader.getSystemClassLoader() == null)
				{
					// HACK because of bootstrap issues (Launcher instantiates a URL and GNU Classpath's URL calls getSystemClassLoader) we have
					// to set clear the sclSet flag here, to make sure the system class loader is constructed next time getSystemClassLoader is called
					typeof(jlClassLoader).GetField("sclSet", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, false);
				}
			}
#endif
			internal static void Bootstrap()
			{
				// call this method to trigger the bootstrap (in the static initializer)
			}

			public static void registerNatives()
			{
			}

			public static object currentThread()
			{
#if FIRST_PASS
				return null;
#else
				return CurrentVMThread().javaThread;
#endif
			}

			internal static int GetThreadStatus(object javaThread)
			{
				// this needs to be a volatile read, but note that we can't synchronize on javaThread
				SystemThreadingThread.MemoryBarrier();
				return (int)threadStatusField.GetValue(javaThread);
			}

			internal static void SetThreadStatus(object javaThread, int value)
			{
				if (value == TERMINATED)
				{
					// NOTE there might be a race condition here (when the thread's Cleanup object
					// is finalized during AppDomain shutdown while the thread is also exiting on its own),
					// but that doesn't matter because Thread.exit() is idempotent.
					threadExitMethod.Invoke(javaThread, null);
				}
				// this needs to be a volatile write, but note that we can't synchronize on javaThread
				threadStatusField.SetValue(javaThread, value);
				SystemThreadingThread.MemoryBarrier();
				if (value == TERMINATED)
				{
					// NOTE locking javaThread here isn't ideal, because we might be invoked from
					// the Cleanup object's finalizer and some user code might own the lock and hence
					// block the finalizer thread.
					lock (javaThread)
					{
						Monitor.PulseAll(javaThread);
					}
				}
			}

			private static VMThread CurrentVMThread()
			{
				VMThread t = vmThread;
				if (t == null)
				{
					t = AttachThread(null, true, null);
				}
				return t;
			}

			private static VMThread GetVMThread(object threadObj)
			{
				return (VMThread)vmThreadField.GetValue(threadObj);
			}

			private static VMThread AttachThread(string name, bool addToGroup, object threadGroup)
			{
#if FIRST_PASS
				return null;
#else
				if (threadGroup == null)
				{
					threadGroup = mainThreadGroup;
				}
				// because the Thread constructor calls Thread.currentThread(), we have to have an instance before we
				// run the constructor
				jlThread thread = (jlThread)FormatterServices.GetUninitializedObject(typeof(jlThread));
				VMThread t = new VMThread(thread, SystemThreadingThread.CurrentThread);
				t.running = true;
				vmThreadField.SetValue(thread, t);
				vmThread = t;
				cleanup = new Cleanup(thread);
				threadPriorityField.SetValue(thread, MapNativePriorityToJava(t.nativeThread.Priority));
				if (name == null)
				{
					// inherit the .NET name of the thread (if it has a name)
					name = t.nativeThread.Name;
				}
				if (name != null)
				{
					threadConstructor1.Invoke(thread, new object[] { threadGroup, name });
				}
				else
				{
					threadConstructor2.Invoke(thread, new object[] { threadGroup, null });
				}
				daemonField.SetValue(thread, t.nativeThread.IsBackground);
				SetThreadStatus(thread, RUNNABLE);
				if (addToGroup)
				{
					threadGroupAddMethod.Invoke(threadGroup, new object[] { thread });
				}
				return t;
#endif
			}

#if !FIRST_PASS
			private static int MapNativePriorityToJava(SystemThreadingThreadPriority priority)
			{
				// TODO consider supporting -XX:JavaPriorityX_To_OSPriority settings
				switch (priority)
				{
					case SystemThreadingThreadPriority.Lowest:
						return jlThread.MIN_PRIORITY;
					case SystemThreadingThreadPriority.BelowNormal:
						return 3;
					default:
					case SystemThreadingThreadPriority.Normal:
						return jlThread.NORM_PRIORITY;
					case SystemThreadingThreadPriority.AboveNormal:
						return 7;
					case SystemThreadingThreadPriority.Highest:
						return jlThread.MAX_PRIORITY;
				}
			}

			private static SystemThreadingThreadPriority MapJavaPriorityToNative(int priority)
			{
				// TODO consider supporting -XX:JavaPriorityX_To_OSPriority settings
				if (priority == jlThread.MIN_PRIORITY)
				{
					return SystemThreadingThreadPriority.Lowest;
				}
				else if (priority > jlThread.MIN_PRIORITY && priority < jlThread.NORM_PRIORITY)
				{
					return SystemThreadingThreadPriority.BelowNormal;
				}
				else if (priority == jlThread.NORM_PRIORITY)
				{
					return SystemThreadingThreadPriority.Normal;
				}
				else if (priority > jlThread.NORM_PRIORITY && priority < jlThread.MAX_PRIORITY)
				{
					return SystemThreadingThreadPriority.AboveNormal;
				}
				else if (priority == jlThread.MAX_PRIORITY)
				{
					return SystemThreadingThreadPriority.Highest;
				}
				else
				{
					// can't happen
					return SystemThreadingThreadPriority.Normal;
				}
			}
#endif

			public static void yield()
			{
				SystemThreadingThread.Sleep(0);
			}

			public static void sleep(long millis)
			{
#if !FIRST_PASS
				if (millis < 0)
				{
					throw new jlIllegalArgumentException("timeout value is negative");
				}
				VMThread t = CurrentVMThread();
				t.EnterInterruptableWait(true);
				try
				{
					for (long iter = millis / int.MaxValue; iter != 0; iter--)
					{
						SystemThreadingThread.Sleep(int.MaxValue);
					}
					SystemThreadingThread.Sleep((int)(millis % int.MaxValue));
				}
				finally
				{
					t.LeaveInterruptableWait();
				}
#endif
			}

			public static void start0(object thisThread)
			{
#if !FIRST_PASS
				// TODO on NET 2.0 set the stack size
				VMThread t = new VMThread((jlThread)thisThread);
				vmThreadField.SetValue(thisThread, t);
				t.nativeThread.Name = t.javaThread.getName();
				t.nativeThread.IsBackground = t.javaThread.isDaemon();
				t.nativeThread.Priority = MapJavaPriorityToNative(t.javaThread.getPriority());
				string apartment = jlSystem.getProperty("ikvm.apartmentstate", "").ToLower();
				if (apartment == "mta")
				{
					t.nativeThread.ApartmentState = ApartmentState.MTA;
				}
				else if (apartment == "sta")
				{
					t.nativeThread.ApartmentState = ApartmentState.STA;
				}
				SetThreadStatus(thisThread, RUNNABLE);
				t.nativeThread.Start();
#endif
			}

			public static bool isInterrupted(object thisThread, bool clearInterrupted)
			{
				VMThread t = GetVMThread(thisThread);
				return t != null && t.IsInterruptPending(clearInterrupted);
			}

			public static bool isAlive(object thisThread)
			{
				int status = GetThreadStatus(thisThread);
				return status != NEW && status != TERMINATED;
			}

			public static int countStackFrames(object thisThread)
			{
				return 0;
			}

			public static bool holdsLock(object obj)
			{
#if FIRST_PASS
				return false;
#else
				if (obj == null)
				{
					throw new jlNullPointerException();
				}
				try
				{
					// The 1.5 memory model (JSR133) explicitly allows spurious wake-ups from Object.wait,
					// so we abuse Pulse to check if we own the monitor.
					Monitor.Pulse(obj);
					return true;
				}
				catch (SynchronizationLockException)
				{
					return false;
				}
#endif
			}

			public static object[][] dumpThreads(object[] threads)
			{
				throw new NotImplementedException();
			}

			public static object[] getThreads()
			{
				throw new NotImplementedException();
			}

			public static void setPriority0(object thisThread, int newPriority)
			{
#if !FIRST_PASS
				lock (thisThread)
				{
					VMThread t = GetVMThread(thisThread);
					if (t != null)
					{
						t.nativeThread.Priority = MapJavaPriorityToNative(newPriority);
					}
				}
#endif
			}

			public static void stop0(object thisThread, object o)
			{
#if !FIRST_PASS
				VMThread t = GetVMThread(thisThread);
				if (t.running)
				{
					// NOTE we allow ThreadDeath (and its subclasses) to be thrown on every thread, but any
					// other exception is ignored, except if we're throwing it on the current Thread. This
					// is done to allow exception handlers to be type specific, otherwise every exception
					// handler would have to catch ThreadAbortException and look inside it to see if it
					// contains the real exception that we wish to handle.
					// I hope we can get away with this behavior, because Thread.stop() is deprecated
					// anyway. Note that we do allow arbitrary exceptions to be thrown on the current
					// thread, since this is harmless (because they aren't wrapped) and also because it
					// provides some real value, because it is one of the ways you can throw arbitrary checked
					// exceptions from Java.
					if (t == vmThread)
					{
						throw (Exception)o;
					}
					else if (o is jlThreadDeath)
					{
						try
						{
							t.nativeThread.Abort(o);
						}
						catch (ThreadStateException)
						{
							// .NET 2.0 throws a ThreadStateException if the target thread is currently suspended
							// (but it does record the Abort request)
						}
						try
						{
							ThreadState suspend = ThreadState.Suspended | ThreadState.SuspendRequested;
							while ((t.nativeThread.ThreadState & suspend) != 0)
							{
								t.nativeThread.Resume();
							}
						}
						catch (ThreadStateException)
						{
						}
					}
				}
				else
				{
					t.stillborn = (Exception)o;
				}
#endif
			}

			public static void suspend0(object thisThread)
			{
				VMThread t = GetVMThread(thisThread);
				if (t != null)
				{
					try
					{
						t.nativeThread.Suspend();
					}
					catch (ThreadStateException)
					{
					}
				}
			}

			public static void resume0(object thisThread)
			{
				VMThread t = GetVMThread(thisThread);
				if (t != null)
				{
					try
					{
						t.nativeThread.Resume();
					}
					catch (ThreadStateException)
					{
					}
				}
			}

			public static void interrupt0(object thisThread)
			{
				// if the thread hasn't been started yet, the interrupt is ignored
				// (like on the reference implementation)
				VMThread t = GetVMThread(thisThread);
				if (t != null)
				{
					t.Interrupt();
				}
			}

			public static object getState(object thisThread)
			{
#if FIRST_PASS
				return null;
#else
				VMThread t = GetVMThread(thisThread);
				return t == null ? jlThread.State.NEW : t.GetState();
#endif
			}

			// this is called from JniInterface.cs
			internal static void WaitUntilLastJniThread()
			{
				throw new NotImplementedException();
			}

			// this is called from JniInterface.cs
			internal static void AttachThreadFromJni(object threadGroup)
			{
#if !FIRST_PASS
				AttachThread(null, true, threadGroup);
#endif
			}

			// this is called from JNI and from VMThread.ThreadProc
			internal static void DetachThread()
			{
				SetThreadStatus(currentThread(), TERMINATED);
				vmThread = null;
				if (cleanup != null)
				{
					GC.SuppressFinalize(cleanup);
					cleanup = null;
				}
			}

			public static void objectWait(object o, long timeout, int nanos)
			{
#if !FIRST_PASS
				if (o == null)
				{
					throw new jlNullPointerException();
				}
				if (timeout < 0)
				{
					throw new jlIllegalArgumentException("timeout value is negative");
				}
				if (nanos < 0 || nanos > 999999)
				{
					throw new jlIllegalArgumentException("nanosecond timeout value out of range");
				}
				if (nanos >= 500000 || (nanos != 0 && timeout == 0))
				{
					timeout++;
				}
				VMThread t = CurrentVMThread();
				t.EnterInterruptableWait(timeout != 0);
				try
				{
					if (timeout == 0 || timeout > 922337203685476L)
					{
						Monitor.Wait(o);
					}
					else
					{
						Monitor.Wait(o, new TimeSpan(timeout * 10000));
					}
				}
				finally
				{
					t.LeaveInterruptableWait();
				}
#endif
			}
		}

		public sealed class VMThread
		{
			// this method has the wrong name, it is only called by ikvm.runtime.Startup.exitMainThread()
			public static void jniDetach()
			{
				Thread.DetachThread();
			}

			public static void park(Object blocker, long nanos)
			{
				throw new NotImplementedException();
			}

			public static void park(long nanos)
			{
				throw new NotImplementedException();
			}

			public static void unpark(object javaThread)
			{
				throw new NotImplementedException();
			}

			public static Object getBlocker(object javaThread)
			{
				throw new NotImplementedException();
			}

			public static SystemThreadingThread getNativeThread(object javaThread)
			{
				throw new NotImplementedException();
			}

			public static object getThreadFromId(long id)
			{
				throw new NotImplementedException();
			}
		}

		namespace reflect
		{
			public sealed class Proxy
			{
				private Proxy() { }

				public static object defineClass0(object classLoader, string name, byte[] b, int off, int len)
				{
					return ClassLoader.defineClass1(classLoader, name, b, off, len, null, null);
				}
			}
		}
	}

	namespace sql
	{
		public sealed class DriverManager
		{
			public static object getCallerClassLoader()
			{
#if FIRST_PASS
				return null;
#else
				for (int i = 1; ; i++)
				{
					StackFrame frame = new StackFrame(i);
					MethodBase method = frame.GetMethod();
					if (method == null)
					{
						return null;
					}
					Type type = method.DeclaringType;
					if (type != typeof(jsDriverManager))
					{
						if (type != null)
						{
							TypeWrapper wrapper = ClassLoaderWrapper.GetWrapperFromType(type);
							if (wrapper != null)
							{
								return wrapper.GetClassLoader().GetJavaClassLoader();
							}
						}
						return null;
					}
				}
#endif
			}
		}
	}

	namespace util
	{
		public sealed class ResourceBundle
		{
			public static object getClassContext()
			{
#if FIRST_PASS
				return null;
#else
				// the caller is only interested in context[2], so that's all we'll fill
				jlClass[] context = new jlClass[3];
				int index = 4;
				// HACK handle inlining or tail-call optimization of native method stub
				if (new StackFrame(1).GetMethod().Name != "getClassContext")
				{
					index--;
				}
				Type type = new StackFrame(index).GetMethod().DeclaringType;
				if (type != null)
				{
					context[2] = (jlClass)ClassLoaderWrapper.GetWrapperFromType(type).ClassObject;
				}
				return context;
#endif
			}
		}

		public sealed class TimeZone
		{
			public static string getSystemTimeZoneID(string javaHome, string country)
			{
				// TODO we need to return the corresponding Java name for TimeZone.CurrentTimeZone.StandardName.
				// The mappings are defined in $JAVA_HOME/lib/tzmappings, but we need the entire $JAVA_HOME/lib/zi
				// directory tree before this is useless, because those files contain the actual time zone definitions.
				return null;
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

namespace IKVM.NativeCode.sun.misc
{
	public sealed class MessageUtils
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

	public sealed class MiscHelper
	{
		public static object getAssemblyClassLoader(Assembly asm)
		{
			return ClassLoaderWrapper.GetAssemblyClassLoader(asm).GetJavaClassLoader();
		}
	}

	public sealed class Signal
	{
		public static int findSignal(string sigName)
		{
			return 0;
		}

		public static long handle0(int sig, long nativeH)
		{
			return 0;
		}

		public static void raise0(int sig)
		{
			throw new NotImplementedException();
		}
	}

	public sealed class NativeSignalHandler
	{
		public static void handle0(int number, long handler)
		{
			throw new NotImplementedException();
		}
	}

	public sealed class Unsafe
	{
		private Unsafe() { }

		public static void throwException(object thisUnsafe, Exception x)
		{
			throw x;
		}

		public static void ensureClassInitialized(object thisUnsafe, object clazz)
		{
			TypeWrapper tw = TypeWrapper.FromClass(clazz);
			if (!tw.IsArray)
			{
				tw.Finish();
				tw.RunClassInit();
			}
		}

		public static object allocateInstance(object thisUnsafe, object clazz)
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
	}

	public sealed class Version
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

	public sealed class VM
	{
		private VM() { }

		public static void getThreadStateValues(int[][] vmThreadStateValues, string[][] vmThreadStateNames)
		{
			// TODO
		}

		public static void initialize()
		{
		}
	}
}

namespace IKVM.NativeCode.sun.reflect
{
	public sealed class Reflection
	{
		private Reflection() { }

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
				if (IKVM.NativeCode.gnu.classpath.VMStackWalker.isHideFromJava(method)
					|| type == null
					|| type.Assembly == typeof(object).Assembly
					|| type.Assembly == typeof(Reflection).Assembly
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

		public static int getClassAccessFlags(object clazz)
		{
			return (int)TypeWrapper.FromClass(clazz).Modifiers;
		}

		public static bool checkInternalAccess(object currentClass, object memberClass)
		{
			TypeWrapper current = TypeWrapper.FromClass(currentClass);
			TypeWrapper member = TypeWrapper.FromClass(memberClass);
			return member.IsInternal && member.GetClassLoader() == current.GetClassLoader();
		}
	}

	public sealed class ReflectionFactory
	{
		private ReflectionFactory() { }

#if !FIRST_PASS
		private static object[] ConvertArgs(TypeWrapper[] argumentTypes, object[] args)
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
					nargs[i] = JVM.Library.unbox(args[i]);
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
			public object invoke(object obj, object[] args)
			{
				if (!mw.IsStatic && !mw.DeclaringType.IsInstance(obj))
				{
					if (obj == null)
					{
						throw new jlNullPointerException();
					}
					throw new jlIllegalArgumentException("object is not an instance of declaring class");
				}
				args = ConvertArgs(mw.GetParameters(), args);
				// if the method is an interface method, we must explicitly run <clinit>,
				// because .NET reflection doesn't
				if (mw.DeclaringType.IsInterface)
				{
					mw.DeclaringType.RunClassInit();
				}
				object retval;
				try
				{
					retval = mw.Invoke(obj, args, false);
				}
				catch (MethodAccessException x)
				{
					// this can happen if we're calling a non-public method and the call stack doesn't have ReflectionPermission.MemberAccess
					throw new jlIllegalAccessException().initCause(x);
				}
				if (mw.ReturnType.IsPrimitive && mw.ReturnType != PrimitiveTypeWrapper.VOID)
				{
					retval = JVM.Library.box(retval);
				}
				return retval;
			}
		}

		private sealed class ConstructorAccessorImpl : srConstructorAccessor
		{
			private readonly MethodWrapper mw;

			internal ConstructorAccessorImpl(jlrConstructor constructor)
			{
				mw = MethodWrapper.FromMethodOrConstructor(constructor);
			}

			[IKVM.Attributes.HideFromJava]
			public object newInstance(object[] args)
			{
				args = ConvertArgs(mw.GetParameters(), args);
				try
				{
					return mw.Invoke(null, args, false);
				}
				catch (MethodAccessException x)
				{
					// this can happen if we're calling a non-public method and the call stack doesn't have ReflectionPermission.MemberAccess
					throw new jlIllegalAccessException().initCause(x);
				}
			}
		}

		private sealed class SerializationConstructorAccessorImpl : srConstructorAccessor
		{
			private Type type;
			private MethodWrapper constructor;

			internal SerializationConstructorAccessorImpl(jlrConstructor constructorToCall, jlClass classToInstantiate)
			{
				constructor = MethodWrapper.FromMethodOrConstructor(constructorToCall);
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
			}

			[IKVM.Attributes.HideFromJava]
			public object newInstance(object[] args)
			{
				// if we're trying to deserialize a string as a TC_OBJECT, just return an emtpy string (Sun does the same)
				if (type == typeof(string))
				{
					return "";
				}
				args = ConvertArgs(constructor.GetParameters(), args);
				try
				{
					object obj = FormatterServices.GetUninitializedObject(type);
					constructor.Invoke(obj, args, false);
					return obj;
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				catch (MethodAccessException x)
				{
					// this can happen if we're calling a non-public method and the call stack doesn't have ReflectionPermission.MemberAccess
					throw new jlIllegalAccessException().initCause(x);
				}
			}
		}

		private abstract class FieldAccessorImplBase : srFieldAccessor
		{
			private readonly FieldWrapper fw;
			private readonly bool isFinal;
			private bool runInit;

			private FieldAccessorImplBase(jlrField field, bool overrideAccessCheck)
			{
				fw = FieldWrapper.FromField(field);
				isFinal = (!overrideAccessCheck || fw.IsStatic) && fw.IsFinal;
				runInit = fw.DeclaringType.IsInterface;
			}

			private object getImpl(object obj)
			{
				// if the field is an interface field, we must explicitly run <clinit>,
				// because .NET reflection doesn't
				if (runInit)
				{
					fw.DeclaringType.RunClassInit();
					runInit = false;
				}
				return fw.GetValue(obj);
			}

			private void setImpl(object obj, object value)
			{
				if (isFinal)
				{
					throw new jlIllegalAccessException();
				}
				// if the field is an interface field, we must explicitly run <clinit>,
				// because .NET reflection doesn't
				if (runInit)
				{
					fw.DeclaringType.RunClassInit();
					runInit = false;
				}
				fw.SetValue(obj, value);
			}

			public virtual bool getBoolean(object obj)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual byte getByte(object obj)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual char getChar(object obj)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual short getShort(object obj)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual int getInt(object obj)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual long getLong(object obj)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual float getFloat(object obj)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual double getDouble(object obj)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual void setBoolean(object obj, bool z)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual void setByte(object obj, byte b)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual void setChar(object obj, char c)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual void setShort(object obj, short s)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual void setInt(object obj, int i)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual void setLong(object obj, long l)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual void setFloat(object obj, float f)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual void setDouble(object obj, double d)
			{
				throw new jlIllegalArgumentException();
			}

			public abstract object get(object obj);
			public abstract void set(object obj, object value);
			
			private sealed class ObjectField : FieldAccessorImplBase
			{
				private readonly jlClass fieldType;

				internal ObjectField(jlrField field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
					fieldType = field.getType();
				}

				public override object get(object obj)
				{
					return getImpl(obj);
				}

				public override void set(object obj, object value)
				{
					if (value != null && !fieldType.isInstance(value))
					{
						throw new jlIllegalArgumentException();
					}
					setImpl(obj, value);
				}
			}

			private sealed class ByteField : FieldAccessorImplBase
			{
				internal ByteField(jlrField field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public override byte getByte(object obj)
				{
					return (byte)getImpl(obj);
				}

				public override short getShort(object obj)
				{
					return (sbyte)getByte(obj);
				}

				public override int getInt(object obj)
				{
					return (sbyte)getByte(obj);
				}

				public override long getLong(object obj)
				{
					return (sbyte)getByte(obj);
				}

				public override float getFloat(object obj)
				{
					return (sbyte)getByte(obj);
				}

				public override double getDouble(object obj)
				{
					return (sbyte)getByte(obj);
				}

				public override object get(object obj)
				{
					return jlByte.valueOf(getByte(obj));
				}

				public override void set(object obj, object val)
				{
					if (!(val is jlByte))
					{
						throw new jlIllegalArgumentException();
					}
					setByte(obj, ((jlByte)val).byteValue());
				}

				public override void setByte(object obj, byte b)
				{
					setImpl(obj, b);
				}
			}

			private sealed class BooleanField : FieldAccessorImplBase
			{
				internal BooleanField(jlrField field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public override bool getBoolean(object obj)
				{
					return (bool)getImpl(obj);
				}

				public override object get(object obj)
				{
					return jlBoolean.valueOf(getBoolean(obj));
				}

				public override void set(object obj, object val)
				{
					if (!(val is jlBoolean))
					{
						throw new jlIllegalArgumentException();
					}
					setBoolean(obj, ((jlBoolean)val).booleanValue());
				}

				public override void setBoolean(object obj, bool b)
				{
					setImpl(obj, b);
				}
			}

			private sealed class CharField : FieldAccessorImplBase
			{
				internal CharField(jlrField field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public override char getChar(object obj)
				{
					return (char)getImpl(obj);
				}

				public override int getInt(object obj)
				{
					return getChar(obj);
				}

				public override long getLong(object obj)
				{
					return getChar(obj);
				}

				public override float getFloat(object obj)
				{
					return getChar(obj);
				}

				public override double getDouble(object obj)
				{
					return getChar(obj);
				}

				public override object get(object obj)
				{
					return jlCharacter.valueOf(getChar(obj));
				}

				public override void set(object obj, object val)
				{
					if (val is jlCharacter)
						setChar(obj, ((jlCharacter)val).charValue());
					else
						throw new jlIllegalArgumentException();
				}

				public override void setChar(object obj, char c)
				{
					setImpl(obj, c);
				}
			}

			private sealed class ShortField : FieldAccessorImplBase
			{
				internal ShortField(jlrField field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public override short getShort(object obj)
				{
					return (short)getImpl(obj);
				}

				public override int getInt(object obj)
				{
					return getShort(obj);
				}

				public override long getLong(object obj)
				{
					return getShort(obj);
				}

				public override float getFloat(object obj)
				{
					return getShort(obj);
				}

				public override double getDouble(object obj)
				{
					return getShort(obj);
				}

				public override object get(object obj)
				{
					return jlShort.valueOf(getShort(obj));
				}

				public override void set(object obj, object val)
				{
					if (val is jlByte
						|| val is jlShort)
						setShort(obj, ((jlNumber)val).shortValue());
					else
						throw new jlIllegalArgumentException();
				}

				public override void setByte(object obj, byte b)
				{
					setShort(obj, (sbyte)b);
				}

				public override void setShort(object obj, short s)
				{
					setImpl(obj, s);
				}
			}

			private sealed class IntField : FieldAccessorImplBase
			{
				internal IntField(jlrField field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public override int getInt(object obj)
				{
					return (int)getImpl(obj);
				}

				public override long getLong(object obj)
				{
					return getInt(obj);
				}

				public override float getFloat(object obj)
				{
					return getInt(obj);
				}

				public override double getDouble(object obj)
				{
					return getInt(obj);
				}

				public override object get(object obj)
				{
					return jlInteger.valueOf(getInt(obj));
				}

				public override void set(object obj, object val)
				{
					if (val is jlByte
						|| val is jlShort
						|| val is jlInteger)
						setInt(obj, ((jlNumber)val).intValue());
					else if (val is jlCharacter)
						setInt(obj, ((jlCharacter)val).charValue());
					else
						throw new jlIllegalArgumentException();
				}

				public override void setByte(object obj, byte b)
				{
					setInt(obj, (sbyte)b);
				}

				public override void setChar(object obj, char c)
				{
					setInt(obj, c);
				}

				public override void setShort(object obj, short s)
				{
					setInt(obj, s);
				}

				public override void setInt(object obj, int i)
				{
					setImpl(obj, i);
				}
			}

			private sealed class FloatField : FieldAccessorImplBase
			{
				internal FloatField(jlrField field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public override float getFloat(object obj)
				{
					return (float)getImpl(obj);
				}

				public override double getDouble(object obj)
				{
					return getFloat(obj);
				}

				public override object get(object obj)
				{
					return jlFloat.valueOf(getFloat(obj));
				}

				public override void set(object obj, object val)
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
						throw new jlIllegalArgumentException();
				}

				public override void setByte(object obj, byte b)
				{
					setFloat(obj, (sbyte)b);
				}

				public override void setChar(object obj, char c)
				{
					setFloat(obj, c);
				}

				public override void setShort(object obj, short s)
				{
					setFloat(obj, s);
				}

				public override void setInt(object obj, int i)
				{
					setFloat(obj, i);
				}

				public override void setLong(object obj, long l)
				{
					setFloat(obj, l);
				}

				public override void setFloat(object obj, float f)
				{
					setImpl(obj, f);
				}
			}

			private sealed class LongField : FieldAccessorImplBase
			{
				internal LongField(jlrField field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public override long getLong(object obj)
				{
					return (long)getImpl(obj);
				}

				public override float getFloat(object obj)
				{
					return getLong(obj);
				}

				public override double getDouble(object obj)
				{
					return getLong(obj);
				}

				public override object get(object obj)
				{
					return jlLong.valueOf(getLong(obj));
				}

				public override void setLong(object obj, long l)
				{
					setImpl(obj, l);
				}

				public override void set(object obj, object val)
				{
					if (val is jlLong
						|| val is jlByte
						|| val is jlShort
						|| val is jlInteger)
						setLong(obj, ((jlNumber)val).longValue());
					else if (val is jlCharacter)
						setLong(obj, ((jlCharacter)val).charValue());
					else
						throw new jlIllegalArgumentException();
				}

				public override void setByte(object obj, byte b)
				{
					setLong(obj, (sbyte)b);
				}

				public override void setChar(object obj, char c)
				{
					setLong(obj, c);
				}

				public override void setShort(object obj, short s)
				{
					setLong(obj, s);
				}

				public override void setInt(object obj, int i)
				{
					setLong(obj, i);
				}
			}

			private sealed class DoubleField : FieldAccessorImplBase
			{
				internal DoubleField(jlrField field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public override double getDouble(object obj)
				{
					return (double)getImpl(obj);
				}

				public override object get(object obj)
				{
					return jlDouble.valueOf(getDouble(obj));
				}

				public override void set(object obj, object val)
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
						throw new jlIllegalArgumentException();
				}

				public override void setByte(object obj, byte b)
				{
					setDouble(obj, (sbyte)b);
				}

				public override void setChar(object obj, char c)
				{
					setDouble(obj, c);
				}

				public override void setShort(object obj, short s)
				{
					setDouble(obj, s);
				}

				public override void setInt(object obj, int i)
				{
					setDouble(obj, i);
				}

				public override void setLong(object obj, long l)
				{
					setDouble(obj, l);
				}

				public override void setFloat(object obj, float f)
				{
					setDouble(obj, f);
				}

				public override void setDouble(object obj, double d)
				{
					setImpl(obj, d);
				}
			}

			internal static FieldAccessorImplBase Create(jlrField field, bool overrideAccessCheck)
			{
				jlClass type = field.getType();
				if (type.isPrimitive())
				{
					if (type == jlByte.TYPE)
					{
						return new ByteField(field, overrideAccessCheck);
					}
					if (type == jlBoolean.TYPE)
					{
						return new BooleanField(field, overrideAccessCheck);
					}
					if (type == jlCharacter.TYPE)
					{
						return new CharField(field, overrideAccessCheck);
					}
					if (type == jlShort.TYPE)
					{
						return new ShortField(field, overrideAccessCheck);
					}
					if (type == jlInteger.TYPE)
					{
						return new IntField(field, overrideAccessCheck);
					}
					if (type == jlFloat.TYPE)
					{
						return new FloatField(field, overrideAccessCheck);
					}
					if (type == jlLong.TYPE)
					{
						return new LongField(field, overrideAccessCheck);
					}
					if (type == jlDouble.TYPE)
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
			return FieldAccessorImplBase.Create((jlrField)field, overrideAccessCheck);
#endif
		}

		public static object newMethodAccessor(object thisFactory, object method)
		{
#if FIRST_PASS
			return null;
#else
			return new MethodAccessorImpl((jlrMethod)method);
#endif
		}

		public static object newConstructorAccessor0(object thisFactory, object constructor)
		{
#if FIRST_PASS
			return null;
#else
			return new ConstructorAccessorImpl((jlrConstructor)constructor);
#endif
		}

		public static object newConstructorAccessorForSerialization(object classToInstantiate, object constructorToCall)
		{
#if FIRST_PASS
			return null;
#else
			return new SerializationConstructorAccessorImpl((jlrConstructor)constructorToCall, (jlClass)classToInstantiate);
#endif
		}
	}

	public sealed class ConstantPool
	{
		private ConstantPool() { }

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
#if FIRST_PASS
			return 0;
#else
			return ((IKVM.NativeCode.java.lang.ConstantPoolWriter)constantPoolOop).GetInt(index);
#endif
		}

		public static long getLongAt0(object thisConstantPool, object constantPoolOop, int index)
		{
#if FIRST_PASS
			return 0;
#else
			return ((IKVM.NativeCode.java.lang.ConstantPoolWriter)constantPoolOop).GetLong(index);
#endif
		}

		public static float getFloatAt0(object thisConstantPool, object constantPoolOop, int index)
		{
#if FIRST_PASS
			return 0;
#else
			return ((IKVM.NativeCode.java.lang.ConstantPoolWriter)constantPoolOop).GetFloat(index);
#endif
		}

		public static double getDoubleAt0(object thisConstantPool, object constantPoolOop, int index)
		{
#if FIRST_PASS
			return 0;
#else
			return ((IKVM.NativeCode.java.lang.ConstantPoolWriter)constantPoolOop).GetDouble(index);
#endif
		}

		public static string getStringAt0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static string getUTF8At0(object thisConstantPool, object constantPoolOop, int index)
		{
#if FIRST_PASS
			return null;
#else
			return ((IKVM.NativeCode.java.lang.ConstantPoolWriter)constantPoolOop).GetUtf8(index);
#endif
		}
	}
}

#if FIRST_PASS
namespace ikvm.@internal
{
	public interface LibraryVMInterface
	{
		object newClass(object wrapper, object protectionDomain, object classLoader);
		object getWrapperFromClass(object clazz);

		object getWrapperFromClassLoader(object classLoader);
		void setWrapperForClassLoader(object classLoader, object wrapper);

		object box(object val);
		object unbox(object val);

		Exception mapException(Exception t);

		object newDirectByteBuffer(IntPtr address, int capacity);
		IntPtr getDirectBufferAddress(object buffer);
		int getDirectBufferCapacity(object buffer);

		void setProperties(System.Collections.Hashtable props);

		bool runFinalizersOnExit();

		object newAnnotation(object classLoader, object definition);
		object newAnnotationElementValue(object classLoader, object expectedClass, object definition);

		object newAssemblyClassLoader(Assembly asm);
	}
}
#endif // !FIRST_PASS
