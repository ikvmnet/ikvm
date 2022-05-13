/*
  Copyright (C) 2002-2013 Jeroen Frijters

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
#if STATIC_COMPILER || STUB_GENERATOR
using IKVM.Reflection;
using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
#endif
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Security;
using System.Security.Permissions;
using IKVM.Internal;

#if !STATIC_COMPILER && !STUB_GENERATOR
namespace IKVM.Internal
{
	public static class Starter
	{
		public static void PrepareForSaveDebugImage()
		{
			JVM.IsSaveDebugImage  = true;
		}
	
		public static void SaveDebugImage()
		{
			DynamicClassLoader.SaveDebugImages();
		}

		public static bool ClassUnloading
		{
#if CLASSGC
			get { return JVM.classUnloading; }
			set { JVM.classUnloading = value; }
#else
			get { return false; }
			set { }
#endif
		}

		public static bool RelaxedVerification
		{
			get { return JVM.relaxedVerification; }
			set { JVM.relaxedVerification = value; }
		}

		public static bool AllowNonVirtualCalls
		{
			get { return JVM.AllowNonVirtualCalls; }
			set { JVM.AllowNonVirtualCalls = value; }
		}
	}
}
#endif // !STATIC_COMPILER && !STUB_GENERATOR

namespace IKVM.Internal
{
	static class JVM
	{
		internal const string JarClassList = "--ikvm-classes--/";
#if STATIC_COMPILER
		internal const bool FinishingForDebugSave = false;
		internal const bool IsSaveDebugImage = false;
#elif !STUB_GENERATOR
		private static bool finishingForDebugSave;
		private static int emitSymbols;
		internal static bool IsSaveDebugImage;
#if CLASSGC
		internal static bool classUnloading = true;
#endif
#endif // STATIC_COMPILER
		private static Assembly coreAssembly;
#if !STUB_GENERATOR
		internal static bool relaxedVerification = true;
		internal static bool AllowNonVirtualCalls;
		internal static readonly bool DisableEagerClassLoading = SafeGetEnvironmentVariable("IKVM_DISABLE_EAGER_CLASS_LOADING") != null;
#endif

#if !STATIC_COMPILER && !STUB_GENERATOR && !FIRST_PASS
		static JVM()
		{
			if (SafeGetEnvironmentVariable("IKVM_SAVE_DYNAMIC_ASSEMBLIES") != null)
			{
				IsSaveDebugImage = true;
				java.lang.Runtime.getRuntime().addShutdownHook(new java.lang.Thread(ikvm.runtime.Delegates.toRunnable(DynamicClassLoader.SaveDebugImages)));
			}
		}
#endif

		internal static Version SafeGetAssemblyVersion(System.Reflection.Assembly asm)
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

		internal static Assembly CoreAssembly
		{
			get
			{
#if !STATIC_COMPILER && !STUB_GENERATOR
				if(coreAssembly == null)
				{
#if FIRST_PASS
					throw new InvalidOperationException("This version of IKVM.Runtime.dll was compiled with FIRST_PASS defined.");
#else
					coreAssembly = typeof(java.lang.Object).Assembly;
#endif
				}
#endif // !STATIC_COMPILER
				return coreAssembly;
			}
			set
			{
				coreAssembly = value;
			}
		}

#if !STATIC_COMPILER && !STUB_GENERATOR
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

		internal static bool EmitSymbols
		{
			get
			{
				if (emitSymbols == 0)
				{
					int state;
					string debug = System.Configuration.ConfigurationManager.AppSettings["ikvm-emit-symbols"];
					if (debug == null)
					{
						state = Debugger.IsAttached ? 1 : 2;
					}
					else
					{
						state = debug.Equals("True", StringComparison.OrdinalIgnoreCase) ? 1 : 2;
					}
					// make sure we only set the value once, because it isn't allowed to changed as that could cause
					// the compiler to try emitting symbols into a ModuleBuilder that doesn't accept them (and would
					// throw an InvalidOperationException)
					Interlocked.CompareExchange(ref emitSymbols, state, 0);
				}
				return emitSymbols == 1;
			}
		}
#endif // !STATIC_COMPILER && !STUB_GENERATOR

		internal static bool IsUnix
		{
			get
			{
				return Environment.OSVersion.Platform == PlatformID.Unix;
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

		// based on Bret Mulvey's C# port of Jenkins32
		// note that this algorithm cannot be changed, because we persist these hashcodes in the metadata of shared class loader assemblies
		internal static int PersistableHash(string str)
		{
			uint key = 1;
			foreach (char c in str)
			{
				key += c;
				key += (key << 12);
				key ^= (key >> 22);
				key += (key << 4);
				key ^= (key >> 9);
				key += (key << 10);
				key ^= (key >> 2);
				key += (key << 7);
				key ^= (key >> 12);
			}
			return (int)key;
		}

#if !STATIC_COMPILER
		internal static void CriticalFailure(string message, Exception x)
		{
			try
			{
				Tracer.Error(Tracer.Runtime, "CRITICAL FAILURE: {0}", message);
				System.Type messageBox = null;
#if !STUB_GENERATOR
				// NOTE we use reflection to invoke MessageBox.Show, to make sure we run in environments where WinForms isn't available
				Assembly winForms = IsUnix ? null : Assembly.Load("System.Windows.Forms, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
				if(winForms != null)
				{
					messageBox = winForms.GetType("System.Windows.Forms.MessageBox");
				}
#endif
				message = String.Format("****** Critical Failure: {1} ******{0}{0}" +
					"PLEASE FILE A BUG REPORT FOR IKVM.NET WHEN YOU SEE THIS MESSAGE{0}{0}" +
					(messageBox != null ? "(on Windows you can use Ctrl+C to copy the contents of this message to the clipboard){0}{0}" : "") +
					"{2}{0}" +
					"{3}{0}" +
					"{4} {5}-bit{0}{0}" +
					"{6}{0}" + 
					"{7}{0}" +
					"{8}",
					Environment.NewLine,
					message,
					System.Reflection.Assembly.GetExecutingAssembly().FullName,
					System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory(),
					Environment.Version,
					IntPtr.Size * 8,
					x,
					x != null ? new StackTrace(x, true).ToString() : "",
					new StackTrace(true));
				if(messageBox != null)
				{
					try
					{
						Version ver = SafeGetAssemblyVersion(typeof(JVM).Assembly);
						messageBox.InvokeMember("Show", System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public, null, null, new object[] { message, "IKVM.NET " + ver + " Critical Failure" });
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
#endif // !STATIC_COMPILER

#if STATIC_COMPILER || STUB_GENERATOR
		internal static Type LoadType(System.Type type)
		{
			return StaticCompiler.GetRuntimeType(type.FullName);
		}
#endif

		// this method resolves types in IKVM.Runtime.dll
		// (the version of IKVM.Runtime.dll that we're running
		// with can be different from the one we're compiling against.)
		internal static Type LoadType(Type type)
		{
#if STATIC_COMPILER || STUB_GENERATOR
			return StaticCompiler.GetRuntimeType(type.FullName);
#else
			return type;
#endif
		}

		internal static object Box(object val)
		{
#if STATIC_COMPILER || FIRST_PASS || STUB_GENERATOR
			return null;
#else
			if(val is byte)
			{
				return java.lang.Byte.valueOf((byte)val);
			}
			else if(val is bool)
			{
				return java.lang.Boolean.valueOf((bool)val);
			}
			else if(val is short)
			{
				return java.lang.Short.valueOf((short)val);
			}
			else if(val is char)
			{
				return java.lang.Character.valueOf((char)val);
			}
			else if(val is int)
			{
				return java.lang.Integer.valueOf((int)val);
			}
			else if(val is float)
			{
				return java.lang.Float.valueOf((float)val);
			}
			else if(val is long)
			{
				return java.lang.Long.valueOf((long)val);
			}
			else if(val is double)
			{
				return java.lang.Double.valueOf((double)val);
			}
			else
			{
				throw new java.lang.IllegalArgumentException();
			}
#endif
		}

		internal static object Unbox(object val)
		{
#if STATIC_COMPILER || FIRST_PASS || STUB_GENERATOR
			return null;
#else
			java.lang.Byte b = val as java.lang.Byte;
			if(b != null)
			{
				return b.byteValue();
			}
			java.lang.Boolean b1 = val as java.lang.Boolean;
			if(b1 != null)
			{
				return b1.booleanValue();
			}
			java.lang.Short s = val as java.lang.Short;
			if(s != null)
			{
				return s.shortValue();
			}
			java.lang.Character c = val as java.lang.Character;
			if(c != null)
			{
				return c.charValue();
			}
			java.lang.Integer i = val as java.lang.Integer;
			if(i != null)
			{
				return i.intValue();
			}
			java.lang.Float f = val as java.lang.Float;
			if(f != null)
			{
				return f.floatValue();
			}
			java.lang.Long l = val as java.lang.Long;
			if(l != null)
			{
				return l.longValue();
			}
			java.lang.Double d = val as java.lang.Double;
			if(d != null)
			{
				return d.doubleValue();
			}
			else
			{
				throw new java.lang.IllegalArgumentException();
			}
#endif
		}

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal static object NewAnnotation(java.lang.ClassLoader classLoader, object definition)
		{
#if !FIRST_PASS
			java.lang.annotation.Annotation ann = null;
			try
			{
				ann = (java.lang.annotation.Annotation)ikvm.@internal.AnnotationAttributeBase.newAnnotation(classLoader, definition);
			}
			catch (java.lang.TypeNotPresentException) { }
			if (ann != null && sun.reflect.annotation.AnnotationType.getInstance(ann.annotationType()).retention() == java.lang.annotation.RetentionPolicy.RUNTIME)
			{
				return ann;
			}
#endif
			return null;
		}
#endif

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal static object NewAnnotationElementValue(java.lang.ClassLoader classLoader, java.lang.Class expectedClass, object definition)
		{
#if FIRST_PASS
			return null;
#else
			try
			{
				return ikvm.@internal.AnnotationAttributeBase.decodeElementValue(definition, expectedClass, classLoader);
			}
			catch(java.lang.IllegalAccessException)
			{
				// TODO this shouldn't be here
				return null;
			}
#endif
		}
#endif

#if !STATIC_COMPILER && !STUB_GENERATOR
		// helper for JNI (which doesn't have access to core library internals)
		internal static object NewDirectByteBuffer(long address, int capacity)
		{
#if FIRST_PASS
			return null;
#else
			return java.nio.DirectByteBuffer.__new(address, capacity);
#endif
		}
#endif

		internal static Type Import(System.Type type)
		{
#if STATIC_COMPILER || STUB_GENERATOR
			return StaticCompiler.Universe.Import(type);
#else
			return type;
#endif
		}
	}

	static class Experimental
	{
		internal static readonly bool JDK_9 = JVM.SafeGetEnvironmentVariable("IKVM_EXPERIMENTAL_JDK_9") != null;
	}
}
