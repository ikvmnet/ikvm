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

namespace IKVM.Runtime
{
	public sealed class Startup
	{
		private Startup()
		{
		}

		private static string[] Glob(string arg)
		{
			try
			{
				string dir = Path.GetDirectoryName(arg);
				if(dir == "")
				{
					dir = null;
				}
				ArrayList list = new ArrayList();
				foreach(FileSystemInfo fsi in new DirectoryInfo(dir == null ? Environment.CurrentDirectory : dir).GetFileSystemInfos(Path.GetFileName(arg)))
				{
					list.Add(dir != null ? Path.Combine(dir, fsi.Name) : fsi.Name);
				}
				if(list.Count == 0)
				{
					return new string[] { arg };
				}
				return (string[])list.ToArray(typeof(string));
			}
			catch
			{
				return new string[] { arg };
			}
		}

		public static string[] Glob()
		{
			return Glob(1);
		}

		public static string[] Glob(int skip)
		{
			if(IKVM.Internal.JVM.IsUnix)
			{
				string[] args = Environment.GetCommandLineArgs();
				string[] vmargs = new string[args.Length - skip];
				Array.Copy(args, skip, vmargs, 0, args.Length - skip);
				return vmargs;
			}
			else
			{
				ArrayList list = new ArrayList();
				string cmdline = Environment.CommandLine;
				StringBuilder sb = new StringBuilder();
				for(int i = 0; i < cmdline.Length;)
				{
					bool quoted = cmdline[i] == '"';
				cont_arg:
					while(i < cmdline.Length && cmdline[i] != ' ' && cmdline[i] != '"')
					{
						sb.Append(cmdline[i++]);
					}
					if(i < cmdline.Length && cmdline[i] == '"')
					{
						if(quoted && i > 1 && cmdline[i - 1] == '"')
						{
							sb.Append('"');
						}
						i++;
						while(i < cmdline.Length && cmdline[i] != '"')
						{
							sb.Append(cmdline[i++]);
						}
						if(i < cmdline.Length && cmdline[i] == '"')
						{
							i++;
						}
						if(i < cmdline.Length && cmdline[i] != ' ')
						{
							goto cont_arg;
						}
					}
					while(i < cmdline.Length && cmdline[i] == ' ')
					{
						i++;
					}
					if(skip > 0)
					{
						skip--;
					}
					else
					{
						if(quoted)
						{
							list.Add(sb.ToString());
						}
						else
						{
							list.AddRange(Glob(sb.ToString()));
						}
					}
					sb.Length = 0;
				}
				return (string[])list.ToArray(typeof(string));
			}
		}

		public static void SetProperties(System.Collections.Hashtable props)
		{
			IKVM.Internal.JVM.Library.setProperties(props);
		}

		public static void EnterMainThread()
		{
			if(Thread.CurrentThread.Name == null)
			{
				try
				{
					Thread.CurrentThread.Name = "main";
				}
				catch(InvalidOperationException)
				{
				}
			}
		}

		public static void ExitMainThread()
		{
			// FXBUG when the main thread ends, it doesn't actually die, it stays around to manage the lifetime
			// of the CLR, but in doing so it also keeps alive the thread local storage for this thread and we
			// use the TLS as a hack to track when the thread dies (if the object stored in the TLS is finalized,
			// we know the thread is dead). So to make that work for the main thread, we use jniDetach which
			// explicitly cleans up our thread.
			IKVM.Internal.JVM.Library.jniDetach();
		}

		public static string GetVersionAndCopyrightInfo()
		{
			Assembly asm = Assembly.GetEntryAssembly();
			object[] desc = asm.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
			if (desc.Length == 1)
			{
				object[] copyright = asm.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (copyright.Length == 1)
				{
					return string.Format("{0} version {1}{2}{3}{2}http://www.ikvm.net/",
						((AssemblyTitleAttribute)desc[0]).Title,
						asm.GetName().Version,
						Environment.NewLine,
						((AssemblyCopyrightAttribute)copyright[0]).Copyright);
				}
			}
			return "";
		}
	}

	public sealed class Util
	{
		private Util()
		{
		}

		public static object GetClassFromObject(object o)
		{
			Type t = o.GetType();
			if(t.IsPrimitive || (ClassLoaderWrapper.IsRemappedType(t) && !t.IsSealed))
			{
				return DotNetTypeWrapper.GetWrapperFromDotNetType(t).ClassObject;
			}
			return ClassLoaderWrapper.GetWrapperFromType(t).ClassObject;
		}

		public static object GetClassFromTypeHandle(RuntimeTypeHandle handle)
		{
			Type t = Type.GetTypeFromHandle(handle);
			if(t.IsPrimitive || ClassLoaderWrapper.IsRemappedType(t))
			{
				return DotNetTypeWrapper.GetWrapperFromDotNetType(t).ClassObject;
			}
			if(Whidbey.ContainsGenericParameters(t))
			{
				return null;
			}
			return ClassLoaderWrapper.GetWrapperFromType(t).ClassObject;
		}

		public static object GetFriendlyClassFromType(Type type)
		{
			if(Whidbey.ContainsGenericParameters(type))
			{
				return null;
			}
			int rank = 0;
			while(type.IsArray)
			{
				type = type.GetElementType();
				rank++;
			}
			if(type.DeclaringType != null
				&& type.DeclaringType.IsDefined(typeof(GhostInterfaceAttribute), false))
			{
				type = type.DeclaringType;
			}
			TypeWrapper wrapper = ClassLoaderWrapper.GetWrapperFromType(type);
			if(rank > 0)
			{
				wrapper = wrapper.MakeArrayType(rank);
			}
			return wrapper.ClassObject;
		}

		public static Type GetInstanceTypeFromClass(object classObject)
		{
			TypeWrapper wrapper = (TypeWrapper)JVM.Library.getWrapperFromClass(classObject);
			if(wrapper.IsRemapped && wrapper.IsFinal)
			{
				return wrapper.TypeAsTBD;
			}
			return wrapper.TypeAsBaseType;
		}

		private static FieldWrapper GetFieldWrapperFromField(object field)
		{
			if(field == null)
			{
				throw new ArgumentNullException("field");
			}
			if(field.GetType().FullName != "java.lang.reflect.Field")
			{
				throw new ArgumentException("field");
			}
			return (FieldWrapper)IKVM.Internal.JVM.Library.getWrapperFromField(field);
		}

		public static object GetFieldConstantValue(object field)
		{
			return GetFieldWrapperFromField(field).GetConstant();
		}

		public static bool IsFieldDeprecated(object field)
		{
			FieldInfo fi = GetFieldWrapperFromField(field).GetField();
			return fi != null && AttributeHelper.IsDefined(fi, typeof(ObsoleteAttribute));
		}

		public static bool IsMethodDeprecated(object method)
		{
			if(method == null)
			{
				throw new ArgumentNullException("method");
			}
			if(method.GetType().FullName != "java.lang.reflect.Method")
			{
				throw new ArgumentException("method");
			}
			MethodWrapper mw = (MethodWrapper)IKVM.Internal.JVM.Library.getWrapperFromMethodOrConstructor(method);
			MethodBase mb = mw.GetMethod();
			return mb != null && AttributeHelper.IsDefined(mb, typeof(ObsoleteAttribute));
		}

		public static bool IsConstructorDeprecated(object constructor)
		{
			if(constructor == null)
			{
				throw new ArgumentNullException("constructor");
			}
			if(constructor.GetType().FullName != "java.lang.reflect.Constructor")
			{
				throw new ArgumentException("constructor");
			}
			MethodWrapper mw = (MethodWrapper)IKVM.Internal.JVM.Library.getWrapperFromMethodOrConstructor(constructor);
			MethodBase mb = mw.GetMethod();
			return mb != null && AttributeHelper.IsDefined(mb, typeof(ObsoleteAttribute));
		}

		public static bool IsClassDeprecated(object clazz)
		{
			if(clazz == null)
			{
				throw new ArgumentNullException("clazz");
			}
			if(clazz.GetType().FullName != "java.lang.Class")
			{
				throw new ArgumentException("clazz");
			}
			TypeWrapper wrapper = TypeWrapper.FromClass(clazz);
			return AttributeHelper.IsDefined(wrapper.TypeAsTBD, typeof(ObsoleteAttribute));
		}

		[HideFromJava]
		public static Exception MapException(Exception x)
		{
			return IKVM.Internal.JVM.Library.mapException(x);
		}
	}
}

namespace IKVM.Internal
{
	public class JVM
	{
		private static bool debug = System.Diagnostics.Debugger.IsAttached;
		private static bool noJniStubs;
		private static bool isStaticCompiler;
		private static bool isIkvmStub;
		private static bool noStackTraceInfo;
		private static bool compilationPhase1;
		private static string sourcePath;
		private static bool enableReflectionOnMethodsWithUnloadableTypeParameters;
		private static ikvm.@internal.LibraryVMInterface lib;
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
				if(coreAssembly == null)
				{
					object lib = Library;
					if(lib != null)
					{
						coreAssembly = lib.GetType().Assembly;
					}
				}
				return coreAssembly;
			}
			set
			{
				coreAssembly = value;
			}
		}

		internal static ikvm.@internal.LibraryVMInterface Library
		{
			get
			{
#if WHIDBEY
				if(JVM.IsStaticCompiler)
				{
					return null;
				}
#endif
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
					if(lib == null && !IsStaticCompiler)
					{
						JVM.CriticalFailure("Unable to find java.lang.LibraryVMInterfaceImpl", null);
					}
				}
				return lib;
			}
		}
#endif

		public static void SetIkvmStubMode()
		{
			// HACK
			isIkvmStub = true;
		}

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

		internal static bool DisableDynamicBinding
		{
			get
			{
				return isStaticCompiler;
			}
		}

		internal static bool IsIkvmStub
		{
			get
			{
				return isIkvmStub;
			}
		}

		internal static bool IsStaticCompiler
		{
			get
			{
				return isStaticCompiler;
			}
			set
			{
				isStaticCompiler = value;
			}
		}

		internal static bool IsStaticCompilerPhase1
		{
			get
			{
				return compilationPhase1;
			}
			set
			{
				compilationPhase1 = value;
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

		internal static bool CompileInnerClassesAsNestedTypes
		{
			get
			{
				// NOTE at the moment, we always do this when compiling statically
				// note that it makes no sense to turn this on when we're dynamically
				// running Java code, it only makes sense to turn it off when statically
				// compiling code that is never used as a library.
				return IsStaticCompiler;
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

#if !COMPACT_FRAMEWORK
		public static void PrepareForSaveDebugImage()
		{
			DynamicClassLoader.PrepareForSaveDebugImage();
		}
	
		public static void SaveDebugImage(object mainClass)
		{
			DynamicClassLoader.SaveDebugImage(mainClass);
		}
#endif

		public static void SetBootstrapClassLoader(object classLoader)
		{
			ClassLoaderWrapper.GetBootstrapClassLoader().SetJavaClassLoader(classLoader);
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
				message = String.Format("****** Critical Failure: {1} ******{0}" +
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

		internal static Type LoadType(Type type)
		{
#if WHIDBEY && !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || JVM.IsIkvmStub)
			{
				return StaticCompiler.GetType(type.FullName);
			}
#endif
			return type;
		}
	}
}
