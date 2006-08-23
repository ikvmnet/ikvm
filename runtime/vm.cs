/*
  Copyright (C) 2002, 2003, 2004, 2005, 2006 Jeroen Frijters

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
using System.Reflection;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Security;
using System.Security.Permissions;
using IKVM.Attributes;
using IKVM.Internal;

#if !STATIC_COMPILER && !COMPACT_FRAMEWORK
namespace IKVM.Internal
{
	public sealed class Starter
	{
		private Starter() {}

		public static void PrepareForSaveDebugImage()
		{
			DynamicClassLoader.PrepareForSaveDebugImage();
		}
	
		public static void SaveDebugImage(object mainClass)
		{
			DynamicClassLoader.SaveDebugImage(mainClass);
		}

		public static bool EnableReflectionOnMethodsWithUnloadableTypeParameters
		{
			get
			{
				return JVM.EnableReflectionOnMethodsWithUnloadableTypeParameters;
			}
			set
			{
				JVM.EnableReflectionOnMethodsWithUnloadableTypeParameters = value;
			}
		}
	}
}
#endif // !STATIC_COMPILER && !COMPACT_FRAMEWORK

namespace IKVM.Internal
{
	class JVM
	{
#if STATIC_COMPILER
		internal const bool IsStaticCompiler = true;
#else
		internal const bool IsStaticCompiler = false;
#endif

		private static bool debug = System.Diagnostics.Debugger.IsAttached;
		private static bool noJniStubs;
		private static bool noStackTraceInfo;
		private static string sourcePath;
		private static bool enableReflectionOnMethodsWithUnloadableTypeParameters;
#if !STATIC_COMPILER
		private static ikvm.@internal.LibraryVMInterface lib;
#endif
		private static bool strictFinalFieldSemantics;
		private static bool finishingForDebugSave;
		private static Assembly coreAssembly;

		internal static Version SafeGetAssemblyVersion(Assembly asm)
		{
			// Assembly.GetName().Version requires FileIOPermission,
			// so we parse the FullName manually :-(
			string name = asm.FullName;
			int start = name.IndexOf(", Version=");
			if(start >= 0)
			{
				start += 10;
				int end = name.IndexOf(',', start);
				if(end >= 0)
				{
					return new Version(name.Substring(start, end - start));
				}
			}
			return new Version();
		}

		internal static string SafeGetEnvironmentVariable(string name)
		{
			try
			{
				return Environment.GetEnvironmentVariable(name);
			}
			catch(SecurityException)
			{
				return null;
			}
		}

#if COMPACT_FRAMEWORK
		internal static ikvm.@internal.LibraryVMInterface Library
		{
			get
			{
				return ikvm.@internal.Library.getImpl();
			}
		}

		internal static Assembly CoreAssembly
		{
			get
			{
				return Library.GetType().Assembly;
			}
		}
#else

		private static Assembly[] UnsafeGetAssemblies()
		{
			new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Assert();
#if WHIDBEY
			if(JVM.IsStaticCompiler)
			{
				return AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies();
			}
#endif
			return AppDomain.CurrentDomain.GetAssemblies();
		}

		private static Type UnsafeGetType(Assembly asm, string name)
		{
			new ReflectionPermission(ReflectionPermissionFlag.MemberAccess
#if !WHIDBEY
				| ReflectionPermissionFlag.TypeInformation
#endif
			).Assert();
			return asm.GetType(name);
		}

		private static object UnsafeCreateInstance(Type type)
		{
			new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Assert();
			return Activator.CreateInstance(type, true);
		}

		internal static Assembly CoreAssembly
		{
			get
			{
#if !STATIC_COMPILER
				if(coreAssembly == null)
				{
					object lib = Library;
					if(lib != null)
					{
						coreAssembly = lib.GetType().Assembly;
					}
				}
#endif
				return coreAssembly;
			}
			set
			{
				coreAssembly = value;
			}
		}

#if !STATIC_COMPILER
		internal static ikvm.@internal.LibraryVMInterface Library
		{
			get
			{
				if(lib == null)
				{
					foreach(Assembly asm in UnsafeGetAssemblies())
					{
						Type type = UnsafeGetType(asm, "java.lang.LibraryVMInterfaceImpl");
						if(type != null)
						{
							lib = UnsafeCreateInstance(type) as ikvm.@internal.LibraryVMInterface;
							if(lib == null)
							{
								// If the "as" fails, this is most likely due to an IKVM.GNU.Classpath.dll version
								// that was linked against an incompatible version of IKVM.Runtime.dll.
								JVM.CriticalFailure("Incompatible core library version", null);
							}
							break;
						}
					}
					if(lib == null)
					{
						JVM.CriticalFailure("Unable to find java.lang.LibraryVMInterfaceImpl", null);
					}
				}
				return lib;
			}
		}
#endif // STATIC_COMPILER
#endif // COMPACT_FRAMEWORK

		internal static bool StrictFinalFieldSemantics
		{
			get
			{
				return strictFinalFieldSemantics;
			}
			set
			{
				strictFinalFieldSemantics = value;
			}
		}

		public static bool Debug
		{
			get
			{
				return debug;
			}
			set
			{
				debug = value;
			}
		}

		public static string SourcePath
		{
			get
			{
				return sourcePath;
			}
			set
			{
				sourcePath = value;
			}
		}

		internal static bool NoJniStubs
		{
			get
			{
				return noJniStubs;
			}
			set
			{
				noJniStubs = value;
			}
		}

		internal static bool NoStackTraceInfo
		{
			get
			{
				return noStackTraceInfo;
			}
			set
			{
				noStackTraceInfo = value;
			}
		}

		public static bool EnableReflectionOnMethodsWithUnloadableTypeParameters
		{
			get
			{
				return enableReflectionOnMethodsWithUnloadableTypeParameters;
			}
			set
			{
				enableReflectionOnMethodsWithUnloadableTypeParameters = value;
			}
		}

		internal static bool FinishingForDebugSave
		{
			get
			{
				return finishingForDebugSave;
			}
			set
			{
				finishingForDebugSave = value;
			}
		}

		internal static bool IsUnix
		{
			get
			{
				return Environment.OSVersion.ToString().IndexOf("Unix") >= 0;
			}
		}
	
		internal static string MangleResourceName(string name)
		{
			// FXBUG there really shouldn't be any need to mangle the resource names,
			// but in order for ILDASM/ILASM round tripping to work reliably, we have
			// to make sure that we don't produce resource names that'll cause ILDASM
			// to generate invalid filenames.
			StringBuilder sb = new StringBuilder("ikvm__", name.Length + 6);
			foreach(char c in name)
			{
				if("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_-+.()$#@~=&{}[]0123456789`".IndexOf(c) != -1)
				{
					sb.Append(c);
				}
				else if(c == '/')
				{
					sb.Append('!');
				}
				else
				{
					sb.Append('%');
					sb.Append(string.Format("{0:X4}", (int)c));
				}
			}
			return sb.ToString();
		}

		internal static void CriticalFailure(string message, Exception x)
		{
			try
			{
				Tracer.Error(Tracer.Runtime, "CRITICAL FAILURE: {0}", message);
				// NOTE we use reflection to invoke MessageBox.Show, to make sure we run in environments where WinForms isn't available
				Assembly winForms = IsUnix ? null : Assembly.Load("System.Windows.Forms, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
				Type messageBox = null;
				if(winForms != null)
				{
					messageBox = winForms.GetType("System.Windows.Forms.MessageBox");
				}
				new ReflectionPermission(ReflectionPermissionFlag.MemberAccess
#if !WHIDBEY
					| ReflectionPermissionFlag.TypeInformation
#endif
				).Assert();
				message = String.Format("****** Critical Failure: {1} ******{0}{0}" +
					"PLEASE FILE A BUG REPORT FOR IKVM.NET WHEN YOU SEE THIS MESSAGE{0}{0}" +
					"{2}{0}" + 
					"{3}{0}" +
					"{4}",
					Environment.NewLine,
					message,
					x,
					x != null ? new StackTrace(x, true).ToString() : "",
					new StackTrace(true));
				CodeAccessPermission.RevertAssert();
				if(messageBox != null)
				{
					try
					{
						Version ver = SafeGetAssemblyVersion(typeof(JVM).Assembly);
						messageBox.InvokeMember("Show", BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public, null, null, new object[] { message, "IKVM.NET " + ver + " Critical Failure" });
					}
					catch
					{
						Console.Error.WriteLine(message);
					}
				}
				else
				{
					Console.Error.WriteLine(message);
				}
			}
			catch(Exception ex)
			{
				Console.Error.WriteLine(ex);
			}
			finally
			{
				Environment.Exit(666);
			}
		}

		// this method resolves types in IKVM.Runtime.dll
		// (the version of IKVM.Runtime.dll that we're running
		// with can be different from the one we're compiling against.)
		internal static Type LoadType(Type type)
		{
#if WHIDBEY && STATIC_COMPILER
			return StaticCompiler.GetType(type.FullName);
#endif
			return type;
		}
	}
}
