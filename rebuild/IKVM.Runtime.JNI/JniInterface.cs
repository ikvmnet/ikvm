/*
  Copyright (C) 2002-2014 Jeroen Frijters

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
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using IKVM.Internal;

// Java type JNI aliases
using jboolean = System.SByte;
using jbyte = System.SByte;
using jchar = System.UInt16;
using jshort = System.Int16;
using jint = System.Int32;
using jsize = System.Int32;
using jlong = System.Int64;
using jfloat = System.Single;
using jdouble = System.Double;
using jobject = System.IntPtr;
using jstring = System.IntPtr;
using jclass = System.IntPtr;
using jarray = System.IntPtr;
using jobjectArray = System.IntPtr;
using jbooleanArray = System.IntPtr;
using jbyteArray = System.IntPtr;
using jcharArray = System.IntPtr;
using jshortArray = System.IntPtr;
using jintArray = System.IntPtr;
using jlongArray = System.IntPtr;
using jfloatArray = System.IntPtr;
using jdoubleArray = System.IntPtr;
using jthrowable = System.IntPtr;
using jweak = System.IntPtr;
using jmethodID = System.IntPtr;
using jfieldID = System.IntPtr;

namespace IKVM.Runtime
{
	[StructLayout(LayoutKind.Sequential)]
	unsafe struct JavaVMOption
	{
		internal byte* optionString;
		internal void* extraInfo;
	}

	[StructLayout(LayoutKind.Sequential)]
	unsafe struct JavaVMInitArgs
	{
		internal jint version;
		internal jint nOptions;
		internal JavaVMOption* options;
		internal jboolean ignoreUnrecognized;
	}

	public unsafe sealed class JNI
	{
		internal static volatile bool jvmCreated;
		internal static volatile bool jvmDestroyed;
		internal const string METHOD_PTR_FIELD_PREFIX = "__<jniptr>";

		internal static bool IsSupportedJniVersion(jint version)
		{
			return version == JNIEnv.JNI_VERSION_1_1
				|| version == JNIEnv.JNI_VERSION_1_2
				|| version == JNIEnv.JNI_VERSION_1_4
				|| version == JNIEnv.JNI_VERSION_1_6
				|| version == JNIEnv.JNI_VERSION_1_8
				;
		}

		public static int CreateJavaVM(void* ppvm, void* ppenv, void* args)
		{
			JavaVMInitArgs* pInitArgs = (JavaVMInitArgs*)args;
			// we don't support the JDK 1.1 JavaVMInitArgs
			if(!IsSupportedJniVersion(pInitArgs->version) || pInitArgs->version == JNIEnv.JNI_VERSION_1_1)
			{
				return JNIEnv.JNI_EVERSION;
			}
			if(jvmCreated)
			{
				return JNIEnv.JNI_ERR;
			}
			System.Collections.Hashtable props = new System.Collections.Hashtable();
			for(int i = 0; i < pInitArgs->nOptions; i++)
			{
				string option = JNIEnv.StringFromOEM(pInitArgs->options[i].optionString);
				if(option.StartsWith("-D"))
				{
					int idx = option.IndexOf('=', 2);
					props[option.Substring(2, idx - 2)] = option.Substring(idx + 1);
				}
				else if(option.StartsWith("-verbose"))
				{
					// ignore
				}
				else if(option == "vfprintf" || option == "exit" || option == "abort")
				{
					// not supported
				}
				else if(pInitArgs->ignoreUnrecognized == JNIEnv.JNI_FALSE)
				{
					return JNIEnv.JNI_ERR;
				}
			}

			ikvm.runtime.Startup.setProperties(props);

			// initialize the class library
			java.lang.Thread.currentThread();

			*((void**)ppvm) = JavaVM.pJavaVM;
			return JavaVM.AttachCurrentThread(JavaVM.pJavaVM, (void**)ppenv, null);
		}

		public static int GetDefaultJavaVMInitArgs(void* vm_args)
		{
			// This is only used for JDK 1.1 JavaVMInitArgs, and we don't support those.
			return JNIEnv.JNI_ERR;
		}

		public static int GetCreatedJavaVMs(void* ppvmBuf, int bufLen, int* nVMs)
		{
			if(jvmCreated)
			{
				if(bufLen >= 1)
				{
					*((void**)ppvmBuf) = JavaVM.pJavaVM;
				}
				if(nVMs != null)
				{
					*nVMs = 1;
				}
			}
			else if(nVMs != null)
			{
				*nVMs = 0;
			}
			return JNIEnv.JNI_OK;
		}

		public struct Frame
		{
			private JNIEnv.ManagedJNIEnv env;
			private JNIEnv.ManagedJNIEnv.FrameState prevFrameState;

			internal ClassLoaderWrapper Enter(ClassLoaderWrapper loader)
			{
				Enter((ikvm.@internal.CallerID)null);
				ClassLoaderWrapper prev = env.classLoader;
				env.classLoader = loader;
				return prev;
			}

			internal void Leave(ClassLoaderWrapper prev)
			{
				env.classLoader = prev;
				Leave();
			}

			public IntPtr Enter(ikvm.@internal.CallerID callerID)
			{
				env = TlsHack.ManagedJNIEnv;
				if(env == null)
				{
					env = JNIEnv.CreateJNIEnv()->GetManagedJNIEnv();
				}
				prevFrameState = env.Enter(callerID);
				return (IntPtr)(void*)env.pJNIEnv;
			}

			public void Leave()
			{
				Exception x = env.Leave(prevFrameState);
				if(x != null)
				{
					throw x;
				}
			}

			public static IntPtr GetFuncPtr(ikvm.@internal.CallerID callerID, string clazz, string name, string sig)
			{
				ClassLoaderWrapper loader = ClassLoaderWrapper.FromCallerID(callerID);
				int sp = 0;
				for(int i = 1; sig[i] != ')'; i++)
				{
					switch(sig[i])
					{
						case '[':
							sp += IntPtr.Size;
							while(sig[++i] == '[');
							if(sig[i] == 'L')
							{
								while(sig[++i] != ';');
							}
							break;
						case 'L':
							sp += IntPtr.Size;
							while(sig[++i] != ';');
							break;
						case 'J':
						case 'D':
							sp += 8;
							break;
						case 'F':
						case 'I':
						case 'C':
						case 'Z':
						case 'S':
						case 'B':
							sp += 4;
							break;
						default:
							Debug.Assert(false);
							break;
					}
				}
				string mangledClass = JniMangle(clazz);
				string mangledName = JniMangle(name);
				string mangledSig = JniMangle(sig.Substring(1, sig.IndexOf(')') - 1));
				string shortMethodName = String.Format("Java_{0}_{1}", mangledClass, mangledName);
				string longMethodName = String.Format("Java_{0}_{1}__{2}", mangledClass, mangledName, mangledSig);
				Tracer.Info(Tracer.Jni, "Linking native method: {0}.{1}{2}, class loader = {3}, short = {4}, long = {5}, args = {6}",
					clazz, name, sig, loader, shortMethodName, longMethodName, sp + 2 * IntPtr.Size);
				lock(JniHelper.JniLock)
				{
					foreach(IntPtr p in loader.GetNativeLibraries())
					{
						IntPtr pfunc = NativeLibrary.GetProcAddress(p, shortMethodName, sp + 2 * IntPtr.Size);
						if(pfunc != IntPtr.Zero)
						{
							Tracer.Info(Tracer.Jni, "Native method {0}.{1}{2} found in library 0x{3:X} (short)", clazz, name, sig, p.ToInt64());
							return pfunc;
						}
						pfunc = NativeLibrary.GetProcAddress(p, longMethodName, sp + 2 * IntPtr.Size);
						if(pfunc != IntPtr.Zero)
						{
							Tracer.Info(Tracer.Jni, "Native method {0}.{1}{2} found in library 0x{3:X} (long)", clazz, name, sig, p.ToInt64());
							return pfunc;
						}
					}
				}
				string msg = string.Format("{0}.{1}{2}", clazz, name, sig);
				Tracer.Error(Tracer.Jni, "UnsatisfiedLinkError: {0}", msg);
				throw new java.lang.UnsatisfiedLinkError(msg);
			}

			private static string JniMangle(string name)
			{
				StringBuilder sb = new StringBuilder();
				foreach(char c in name)
				{
					if(c == '/')
					{
						sb.Append('_');
					}
					else if(c == '_')
					{
						sb.Append("_1");
					}
					else if(c == ';')
					{
						sb.Append("_2");
					}
					else if(c == '[')
					{
						sb.Append("_3");
					}
					else if((c >= '0' && c <= '9') || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
					{
						sb.Append(c);
					}
					else
					{
						sb.Append(String.Format("_0{0:x4}", (int)c));
					}
				}
				return sb.ToString();
			}

			public IntPtr MakeLocalRef(object obj)
			{
				return env.MakeLocalRef(obj);
			}

			// NOTE this method has the wrong name, it should unwrap *any* jobject reference type (local and global)
			public object UnwrapLocalRef(IntPtr p)
			{
				return JNIEnv.UnwrapRef(env, p);
			}
		}
	}

	abstract unsafe class NativeLibrary
	{
		private NativeLibrary() { }
		private static readonly NativeLibrary impl;

		static NativeLibrary()
		{
			try
			{
				if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				{
					if (IntPtr.Size == 4)
					{
						impl = new Win32_x86();
					}
					else
					{
						impl = new Win32_x64();
					}
					// call a method to trigger the native library load
					// (if this fails, we fall back to the classic native library)
					impl._MarshalDelegate(null);
				}
				else
				{
					impl = new Classic();
				}
			}
			catch (DllNotFoundException)
			{
				impl = new Classic();
			}
			catch (BadImageFormatException)
			{
				impl = new Classic();
			}
		}

		private sealed class Win32_x86 : NativeLibrary
		{
			private const string library = "ikvm-native-win32-x86";

			[DllImport(library, SetLastError = true)]
			private static extern IntPtr ikvm_LoadLibrary(string filename);
			[DllImport(library)]
			private static extern void ikvm_FreeLibrary(IntPtr handle);
			[DllImport(library)]
			private static extern IntPtr ikvm_GetProcAddress(IntPtr handle, string name, int argc);
			[DllImport(library)]
			private static extern int ikvm_CallOnLoad(IntPtr method, void* jvm, void* reserved);
			[DllImport(library)]
			private static extern void** ikvm_GetJNIEnvVTable();
			[DllImport(library)]
			private static extern void* ikvm_MarshalDelegate(Delegate d);

			protected override IntPtr _LoadLibrary(string filename)
			{
				return ikvm_LoadLibrary(filename);
			}

			protected override void _FreeLibrary(IntPtr handle)
			{
				ikvm_FreeLibrary(handle);
			}

			protected override IntPtr _GetProcAddress(IntPtr handle, string name, int argc)
			{
				return ikvm_GetProcAddress(handle, name, argc);
			}

			protected override int _CallOnLoad(IntPtr method, void* jvm, void* reserved)
			{
				return ikvm_CallOnLoad(method, jvm, reserved);
			}

			protected override void** _GetJNIEnvVTable()
			{
				return ikvm_GetJNIEnvVTable();
			}

			protected override void* _MarshalDelegate(Delegate d)
			{
				return ikvm_MarshalDelegate(d);
			}
		}

		private sealed class Win32_x64 : NativeLibrary
		{
			private const string library = "ikvm-native-win32-x64";

			[DllImport(library, SetLastError = true)]
			private static extern IntPtr ikvm_LoadLibrary(string filename);
			[DllImport(library)]
			private static extern void ikvm_FreeLibrary(IntPtr handle);
			[DllImport(library)]
			private static extern IntPtr ikvm_GetProcAddress(IntPtr handle, string name, int argc);
			[DllImport(library)]
			private static extern int ikvm_CallOnLoad(IntPtr method, void* jvm, void* reserved);
			[DllImport(library)]
			private static extern void** ikvm_GetJNIEnvVTable();
			[DllImport(library)]
			private static extern void* ikvm_MarshalDelegate(Delegate d);

			protected override IntPtr _LoadLibrary(string filename)
			{
				return ikvm_LoadLibrary(filename);
			}

			protected override void _FreeLibrary(IntPtr handle)
			{
				ikvm_FreeLibrary(handle);
			}

			protected override IntPtr _GetProcAddress(IntPtr handle, string name, int argc)
			{
				return ikvm_GetProcAddress(handle, name, argc);
			}

			protected override int _CallOnLoad(IntPtr method, void* jvm, void* reserved)
			{
				return ikvm_CallOnLoad(method, jvm, reserved);
			}

			protected override void** _GetJNIEnvVTable()
			{
				return ikvm_GetJNIEnvVTable();
			}

			protected override void* _MarshalDelegate(Delegate d)
			{
				return ikvm_MarshalDelegate(d);
			}
		}

		private sealed class Classic : NativeLibrary
		{
			private const string library = "ikvm-native";

			[DllImport(library, SetLastError = true)]
			private static extern IntPtr ikvm_LoadLibrary(string filename);
			[DllImport(library)]
			private static extern void ikvm_FreeLibrary(IntPtr handle);
			[DllImport(library)]
			private static extern IntPtr ikvm_GetProcAddress(IntPtr handle, string name, int argc);
			[DllImport(library)]
			private static extern int ikvm_CallOnLoad(IntPtr method, void* jvm, void* reserved);
			[DllImport(library)]
			private static extern void** ikvm_GetJNIEnvVTable();
			[DllImport(library)]
			private static extern void* ikvm_MarshalDelegate(Delegate d);

			protected override IntPtr _LoadLibrary(string filename)
			{
				return ikvm_LoadLibrary(filename);
			}

			protected override void _FreeLibrary(IntPtr handle)
			{
				ikvm_FreeLibrary(handle);
			}

			protected override IntPtr _GetProcAddress(IntPtr handle, string name, int argc)
			{
				return ikvm_GetProcAddress(handle, name, argc);
			}

			protected override int _CallOnLoad(IntPtr method, void* jvm, void* reserved)
			{
				return ikvm_CallOnLoad(method, jvm, reserved);
			}

			protected override void** _GetJNIEnvVTable()
			{
				return ikvm_GetJNIEnvVTable();
			}

			protected override void* _MarshalDelegate(Delegate d)
			{
				return ikvm_MarshalDelegate(d);
			}
		}

		protected abstract IntPtr _LoadLibrary(string filename);
		protected abstract void _FreeLibrary(IntPtr handle);
		protected abstract IntPtr _GetProcAddress(IntPtr handle, string name, int argc);
		protected abstract int _CallOnLoad(IntPtr method, void* jvm, void* reserved);
		protected abstract void** _GetJNIEnvVTable();
		protected abstract void* _MarshalDelegate(Delegate d);

		internal static IntPtr LoadLibrary(string filename)
		{
			return impl._LoadLibrary(filename);
		}

		internal static void FreeLibrary(IntPtr handle)
		{
			impl._FreeLibrary(handle);
		}

		internal static IntPtr GetProcAddress(IntPtr handle, string name, int argc)
		{
			return impl._GetProcAddress(handle, name, argc);
		}

		internal static int CallOnLoad(IntPtr method, void* jvm, void* reserved)
		{
			return impl._CallOnLoad(method, jvm, reserved);
		}

		internal static void** GetJNIEnvVTable()
		{
			return impl._GetJNIEnvVTable();
		}

		internal static void* MarshalDelegate(Delegate d)
		{
			return impl._MarshalDelegate(d);
		}
	}

	sealed class JniHelper
	{
		private static List<IntPtr> nativeLibraries = new List<IntPtr>();
		internal static readonly object JniLock = new object();

		// MONOBUG with mcs we can't pass ClassLoaderWrapper from IKVM.Runtime.dll to IKVM.Runtime.JNI.dll
		internal unsafe static long LoadLibrary(string filename, object loader)
		{
			return LoadLibrary(filename, (ClassLoaderWrapper)loader);
		}

		// MONOBUG with mcs we can't pass ClassLoaderWrapper from IKVM.Runtime.dll to IKVM.Runtime.JNI.dll
		internal static void UnloadLibrary(long handle, object loader)
		{
			UnloadLibrary(handle, (ClassLoaderWrapper)loader);
		}

		private unsafe static long LoadLibrary(string filename, ClassLoaderWrapper loader)
		{
			Tracer.Info(Tracer.Jni, "loadLibrary: {0}, class loader: {1}", filename, loader);
			lock(JniLock)
			{
				IntPtr p = NativeLibrary.LoadLibrary(filename);
				if(p == IntPtr.Zero)
				{
					Tracer.Info(Tracer.Jni, "Failed to load library: path = '{0}', error = {1}, message = {2}", filename,
						Marshal.GetLastWin32Error(), new System.ComponentModel.Win32Exception().Message);
					return 0;
				}
				try
				{
					foreach(IntPtr tmp in loader.GetNativeLibraries())
					{
						if(tmp == p)
						{
							// the library was already loaded by the current class loader,
							// no need to do anything
							NativeLibrary.FreeLibrary(p);
							Tracer.Warning(Tracer.Jni, "Library was already loaded: {0}", filename);
							return p.ToInt64();
						}
					}
					if(nativeLibraries.Contains(p))
					{
						string msg = string.Format("Native library {0} already loaded in another classloader", filename);
						Tracer.Error(Tracer.Jni, "UnsatisfiedLinkError: {0}", msg);
						throw new java.lang.UnsatisfiedLinkError(msg);
					}
					Tracer.Info(Tracer.Jni, "Library loaded: {0}, handle = 0x{1:X}", filename, p.ToInt64());
					IntPtr onload = NativeLibrary.GetProcAddress(p, "JNI_OnLoad", IntPtr.Size * 2);
					if(onload != IntPtr.Zero)
					{
						Tracer.Info(Tracer.Jni, "Calling JNI_OnLoad on: {0}", filename);
						JNI.Frame f = new JNI.Frame();
						int version;
						ClassLoaderWrapper prevLoader = f.Enter(loader);
						try
						{
							// TODO on Whidbey we should be able to use Marshal.GetDelegateForFunctionPointer to call OnLoad
							version = NativeLibrary.CallOnLoad(onload, JavaVM.pJavaVM, null);
							Tracer.Info(Tracer.Jni, "JNI_OnLoad returned: 0x{0:X8}", version);
						}
						finally
						{
							f.Leave(prevLoader);
						}
						if(!JNI.IsSupportedJniVersion(version))
						{
							string msg = string.Format("Unsupported JNI version 0x{0:X} required by {1}", version, filename);
							Tracer.Error(Tracer.Jni, "UnsatisfiedLinkError: {0}", msg);
							throw new java.lang.UnsatisfiedLinkError(msg);
						}
					}
					nativeLibraries.Add(p);
					loader.RegisterNativeLibrary(p);
					return p.ToInt64();
				}
				catch
				{
					NativeLibrary.FreeLibrary(p);
					throw;
				}
			}
		}

		private unsafe static void UnloadLibrary(long handle, ClassLoaderWrapper loader)
		{
			lock (JniLock)
			{
				Tracer.Info(Tracer.Jni, "Unloading library: handle = 0x{0:X}, class loader = {1}", handle, loader);
				IntPtr p = (IntPtr)handle;
				IntPtr onunload = NativeLibrary.GetProcAddress(p, "JNI_OnUnload", IntPtr.Size * 2);
				if (onunload != IntPtr.Zero)
				{
					Tracer.Info(Tracer.Jni, "Calling JNI_OnUnload on: handle = 0x{0:X}", handle);
					JNI.Frame f = new JNI.Frame();
					ClassLoaderWrapper prevLoader = f.Enter(loader);
					try
					{
						// TODO on Whidbey we should be able to use Marshal.GetDelegateForFunctionPointer to call OnLoad
						NativeLibrary.CallOnLoad(onunload, JavaVM.pJavaVM, null);
					}
					finally
					{
						f.Leave(prevLoader);
					}
				}
				nativeLibraries.Remove(p);
				loader.UnregisterNativeLibrary(p);
				NativeLibrary.FreeLibrary((IntPtr)handle);
			}
		}
	}

	static class GlobalRefs
	{
		internal static System.Collections.ArrayList globalRefs = new System.Collections.ArrayList();
		internal static readonly object weakRefLock = new object();
		internal static GCHandle[] weakRefs = new GCHandle[16];

		internal static object Unwrap(int i)
		{
			i = -i;
			if ((i & (1 << 30)) != 0)
			{
				lock (GlobalRefs.weakRefLock)
				{
					return GlobalRefs.weakRefs[i - (1 << 30)].Target;
				}
			}
			else
			{
				lock (GlobalRefs.globalRefs)
				{
					return GlobalRefs.globalRefs[i - 1];
				}
			}
		}
	}

	unsafe class VtableBuilder
	{
		delegate int pf_int_IntPtr(JNIEnv* pEnv, IntPtr p);
		delegate IntPtr pf_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p);
		delegate void pf_void_IntPtr(JNIEnv* pEnv, IntPtr p);
		delegate IntPtr pf_IntPtr(JNIEnv* pEnv);
		delegate void pf_void(JNIEnv* pEnv);
		delegate sbyte pf_sbyte(JNIEnv* pEnv);
		delegate IntPtr pf_IntPtr_pbyte(JNIEnv* pEnv, byte* p);
		delegate int pf_int(JNIEnv* pEnv);
		delegate IntPtr pf_IntPtr_pbyte_IntPtr_psbyte_IntPtr(JNIEnv* pEnv, byte* p1, IntPtr p2, sbyte* p3, int p4);
		delegate IntPtr pf_IntPtr_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p1, IntPtr p2);
		delegate jchar* pf_pjchar_IntPtr_pjboolean(JNIEnv* pEnv, IntPtr p1, jboolean* p2);
		delegate void pf_void_IntPtr_pvoid_int(JNIEnv* pEnv, IntPtr p1, void* p2, int p3);
		delegate void* pf_pvoid_IntPtr_pjboolean(JNIEnv* pEnv, IntPtr p1, jboolean* p2);
		delegate int pf_int_IntPtr_pbyte(JNIEnv* pEnv, IntPtr p1, byte* p2);
		delegate void pf_void_pbyte(JNIEnv* pEnv, byte* p1);
		delegate IntPtr pf_IntPtr_IntPtr_pbyte_pbyte(JNIEnv* pEnv, IntPtr p1, byte* p2, byte* p3);
		delegate int pf_int_IntPtr_pJNINativeMethod_int(JNIEnv* pEnv, IntPtr p1, JNIEnv.JNINativeMethod* p2, int p3);
		delegate int pf_int_ppJavaVM(JNIEnv* pEnv, JavaVM** ppJavaVM);
		delegate sbyte pf_sbyte_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p1, IntPtr p2);
		delegate short pf_short_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p1, IntPtr p2);
		delegate ushort pf_ushort_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p1, IntPtr p2);
		delegate int pf_int_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p1, IntPtr p2);
		delegate long pf_long_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p1, IntPtr p2);
		delegate float pf_float_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p1, IntPtr p2);
		delegate double pf_double_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p1, IntPtr p2);
		delegate void pf_void_IntPtr_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p1, IntPtr p2, IntPtr p3);
		delegate void pf_void_IntPtr_IntPtr_sbyte(JNIEnv* pEnv, IntPtr p1, IntPtr p2, sbyte p3);
		delegate void pf_void_IntPtr_IntPtr_short(JNIEnv* pEnv, IntPtr p1, IntPtr p2, short p3);
		delegate void pf_void_IntPtr_IntPtr_ushort(JNIEnv* pEnv, IntPtr p1, IntPtr p2, ushort p3);
		delegate void pf_void_IntPtr_IntPtr_int(JNIEnv* pEnv, IntPtr p1, IntPtr p2, int p3);
		delegate void pf_void_IntPtr_IntPtr_long(JNIEnv* pEnv, IntPtr p1, IntPtr p2, long p3);
		delegate void pf_void_IntPtr_IntPtr_float(JNIEnv* pEnv, IntPtr p1, IntPtr p2, float p3);
		delegate void pf_void_IntPtr_IntPtr_double(JNIEnv* pEnv, IntPtr p1, IntPtr p2, double p3);
		delegate IntPtr pf_IntPtr_pjchar_int(JNIEnv* pEnv, jchar* p1, int p2);
		delegate void pf_void_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p1, IntPtr p2);
		delegate void pf_void_IntPtr_pjchar(JNIEnv* pEnv, IntPtr p1, jchar* p2);
		delegate IntPtr pf_IntPtr_int_IntPtr_IntPtr(JNIEnv* pEnv, int p1, IntPtr p2, IntPtr p3);
		delegate IntPtr pf_IntPtr_IntPtr_int(JNIEnv* pEnv, IntPtr p1, int p2);
		delegate void pf_void_IntPtr_int_IntPtr(JNIEnv* pEnv, IntPtr p1, int p2, IntPtr p3);
		delegate IntPtr pf_IntPtr_int(JNIEnv* pEnv, int p1);
		delegate void pf_void_IntPtr_int_int_IntPtr(JNIEnv* pEnv, IntPtr p1, int p2, int p3, IntPtr p4);
		delegate IntPtr pf_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, JNIEnv.jvalue* p3);
		delegate sbyte pf_sbyte_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, JNIEnv.jvalue* p3);
		delegate short pf_short_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, JNIEnv.jvalue* p3);
		delegate ushort pf_ushort_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, JNIEnv.jvalue* p3);
		delegate int pf_int_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, JNIEnv.jvalue* p3);
		delegate long pf_long_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, JNIEnv.jvalue* p3);
		delegate float pf_float_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, JNIEnv.jvalue* p3);
		delegate double pf_double_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, JNIEnv.jvalue* p3);
		delegate void pf_void_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, JNIEnv.jvalue* p3);
		delegate IntPtr pf_IntPtr_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, IntPtr p3, JNIEnv.jvalue* p4);
		delegate sbyte pf_sbyte_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, IntPtr p3, JNIEnv.jvalue* p4);
		delegate ushort pf_ushort_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, IntPtr p3, JNIEnv.jvalue* p4);
		delegate short pf_short_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, IntPtr p3, JNIEnv.jvalue* p4);
		delegate int pf_int_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, IntPtr p3, JNIEnv.jvalue* p4);
		delegate long pf_long_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, IntPtr p3, JNIEnv.jvalue* p4);
		delegate float pf_float_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, IntPtr p3, JNIEnv.jvalue* p4);
		delegate double pf_double_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, IntPtr p3, JNIEnv.jvalue* p4);
		delegate void pf_void_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, IntPtr p3, JNIEnv.jvalue* p4);
		delegate byte* pf_pbyte_IntPtr_pjboolean(JNIEnv* pEnv, IntPtr p1, jboolean* p2);
		delegate void pf_void_IntPtr_pbyte(JNIEnv* pEnv, IntPtr p1, byte* p2);
		delegate jboolean* pf_pjboolean_IntPtr_pjboolean(JNIEnv* pEnv, IntPtr p1, jboolean* p2);
		delegate jbyte* pf_pjbyte_IntPtr_pjboolean(JNIEnv* pEnv, IntPtr p1, jboolean* p2);
		delegate jshort* pf_pjshort_IntPtr_pjboolean(JNIEnv* pEnv, IntPtr p1, jboolean* p2);
		delegate jint* pf_pjint_IntPtr_pjboolean(JNIEnv* pEnv, IntPtr p1, jboolean* p2);
		delegate jlong* pf_pjlong_IntPtr_pjboolean(JNIEnv* pEnv, IntPtr p1, jboolean* p2);
		delegate jfloat* pf_pjfloat_IntPtr_pjboolean(JNIEnv* pEnv, IntPtr p1, jboolean* p2);
		delegate jdouble* pf_pjdouble_IntPtr_pjboolean(JNIEnv* pEnv, IntPtr p1, jboolean* p2);
		delegate void pf_void_IntPtr_pjboolean_int(JNIEnv* pEnv, IntPtr p1, jboolean* p2, int p3);
		delegate void pf_void_IntPtr_pjbyte_int(JNIEnv* pEnv, IntPtr p1, jbyte* p2, int p3);
		delegate void pf_void_IntPtr_pjchar_int(JNIEnv* pEnv, IntPtr p1, jchar* p2, int p3);
		delegate void pf_void_IntPtr_pjshort_int(JNIEnv* pEnv, IntPtr p1, jshort* p2, int p3);
		delegate void pf_void_IntPtr_pjint_int(JNIEnv* pEnv, IntPtr p1, jint* p2, int p3);
		delegate void pf_void_IntPtr_pjlong_int(JNIEnv* pEnv, IntPtr p1, jlong* p2, int p3);
		delegate void pf_void_IntPtr_pjfloat_int(JNIEnv* pEnv, IntPtr p1, jfloat* p2, int p3);
		delegate void pf_void_IntPtr_pjdouble_int(JNIEnv* pEnv, IntPtr p1, jdouble* p2, int p3);
		delegate int pf_int_int(JNIEnv* pEnv, int p1);
		delegate IntPtr pf_IntPtr_IntPtr_long(JNIEnv* pEnv, IntPtr p1, long p2);
		delegate long pf_long_IntPtr(JNIEnv* pEnv, IntPtr p1);
		delegate IntPtr pf_IntPtr_IntPtr_IntPtr_sbyte(JNIEnv* pEnv, IntPtr p1, IntPtr p2, sbyte p3);

		internal static void* vtable;

		static VtableBuilder()
		{
			JNI.jvmCreated = true;
			// JNIEnv
			void** pmcpp = NativeLibrary.GetJNIEnvVTable();
			void** p = (void**)JniMem.Alloc(IntPtr.Size * vtableDelegates.Length);
			for(int i = 0; i < vtableDelegates.Length; i++)
			{
				if(vtableDelegates[i] != null)
				{
					// TODO on Whidbey we can use Marshal.GetFunctionPointerForDelegate
					p[i] = NativeLibrary.MarshalDelegate(vtableDelegates[i]);
				}
				else
				{
					p[i] = pmcpp[i];
				}
			}
			vtable = p;
		}

		static Delegate[] vtableDelegates =
		{
			new pf_int_IntPtr_pbyte(JNIEnv.GetMethodArgs), //virtual void JNICALL reserved0();
			null, //virtual void JNICALL reserved1();
			null, //virtual void JNICALL reserved2();
			null, //virtual void JNICALL reserved3();

			new pf_int(JNIEnv.GetVersion), //virtual jint JNICALL GetVersion();

			new pf_IntPtr_pbyte_IntPtr_psbyte_IntPtr(JNIEnv.DefineClass), //virtual jclass JNICALL DefineClass(const char *name, jobject loader, const jbyte *buf, jsize len);
			new pf_IntPtr_pbyte(JNIEnv.FindClass), //virtual jclass JNICALL FindClass(const char *name);

			new pf_IntPtr_IntPtr(JNIEnv.FromReflectedMethod), //virtual jmethodID JNICALL FromReflectedMethod(jobject method);
			new pf_IntPtr_IntPtr(JNIEnv.FromReflectedField), //virtual jfieldID JNICALL FromReflectedField(jobject field);
			new pf_IntPtr_IntPtr_IntPtr_sbyte(JNIEnv.ToReflectedMethod), //virtual jobject JNICALL ToReflectedMethod(jclass clazz, jmethodID methodID, jboolean isStatic);

			new pf_IntPtr_IntPtr(JNIEnv.GetSuperclass), //virtual jclass JNICALL GetSuperclass(jclass sub);
			new pf_sbyte_IntPtr_IntPtr(JNIEnv.IsAssignableFrom), //virtual jboolean JNICALL IsAssignableFrom(jclass sub, jclass sup);

			new pf_IntPtr_IntPtr_IntPtr_sbyte(JNIEnv.ToReflectedField), //virtual jobject JNICALL ToReflectedField(jclass clazz, jfieldID fieldID, jboolean isStatic);

			new pf_int_IntPtr(JNIEnv.Throw), //virtual jint JNICALL Throw(jthrowable obj);
			new pf_int_IntPtr_pbyte(JNIEnv.ThrowNew), //virtual jint JNICALL ThrowNew(jclass clazz, const char *msg);
			new pf_IntPtr(JNIEnv.ExceptionOccurred), //virtual jthrowable JNICALL ExceptionOccurred();
			new pf_void(JNIEnv.ExceptionDescribe), //virtual void JNICALL ExceptionDescribe();
			new pf_void(JNIEnv.ExceptionClear), //virtual void JNICALL ExceptionClear();
			new pf_void_pbyte(JNIEnv.FatalError), //virtual void JNICALL FatalError(const char *msg);

			new pf_int_int(JNIEnv.PushLocalFrame), //virtual jint JNICALL PushLocalFrame(jint capacity); 
			new pf_IntPtr_IntPtr(JNIEnv.PopLocalFrame), //virtual jobject JNICALL PopLocalFrame(jobject result);

			new pf_IntPtr_IntPtr(JNIEnv.NewGlobalRef), //virtual jobject JNICALL NewGlobalRef(jobject lobj);
			new pf_void_IntPtr(JNIEnv.DeleteGlobalRef), //virtual void JNICALL DeleteGlobalRef(jobject gref);
			new pf_void_IntPtr(JNIEnv.DeleteLocalRef), //virtual void JNICALL DeleteLocalRef(jobject obj);
			new pf_sbyte_IntPtr_IntPtr(JNIEnv.IsSameObject), //virtual jboolean JNICALL IsSameObject(jobject obj1, jobject obj2);

			new pf_IntPtr_IntPtr(JNIEnv.NewLocalRef), //virtual jobject JNICALL NewLocalRef(jobject ref);
			new pf_int_int(JNIEnv.EnsureLocalCapacity), //virtual jint JNICALL EnsureLocalCapacity(jint capacity);

			new pf_IntPtr_IntPtr(JNIEnv.AllocObject), //virtual jobject JNICALL AllocObject(jclass clazz);
			null, //virtual jobject JNICALL NewObject(jclass clazz, jmethodID methodID, ...);
			null, //virtual jobject JNICALL NewObjectV(jclass clazz, jmethodID methodID, va_list args);
			new pf_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.NewObjectA), //virtual jobject JNICALL NewObjectA(jclass clazz, jmethodID methodID, jvalue *args);

			new pf_IntPtr_IntPtr(JNIEnv.GetObjectClass), //virtual jclass JNICALL GetObjectClass(jobject obj);
			new pf_sbyte_IntPtr_IntPtr(JNIEnv.IsInstanceOf), //virtual jboolean JNICALL IsInstanceOf(jobject obj, jclass clazz);

			new pf_IntPtr_IntPtr_pbyte_pbyte(JNIEnv.GetMethodID), //virtual jmethodID JNICALL GetMethodID(jclass clazz, const char *name, const char *sig);

			null, //virtual jobject JNICALL CallObjectMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jobject JNICALL CallObjectMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallObjectMethodA), //virtual jobject JNICALL CallObjectMethodA(jobject obj, jmethodID methodID, jvalue * args);

			null, //virtual jboolean JNICALL CallBooleanMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jboolean JNICALL CallBooleanMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_sbyte_IntPtr_IntPtr_pjvalue(JNIEnv.CallBooleanMethodA), //virtual jboolean JNICALL CallBooleanMethodA(jobject obj, jmethodID methodID, jvalue * args);

			null, //virtual jbyte JNICALL CallByteMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jbyte JNICALL CallByteMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_sbyte_IntPtr_IntPtr_pjvalue(JNIEnv.CallByteMethodA), //virtual jbyte JNICALL CallByteMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual jchar JNICALL CallCharMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jchar JNICALL CallCharMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_ushort_IntPtr_IntPtr_pjvalue(JNIEnv.CallCharMethodA), //virtual jchar JNICALL CallCharMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual jshort JNICALL CallShortMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jshort JNICALL CallShortMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_short_IntPtr_IntPtr_pjvalue(JNIEnv.CallShortMethodA), //virtual jshort JNICALL CallShortMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual jint JNICALL CallIntMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jint JNICALL CallIntMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_int_IntPtr_IntPtr_pjvalue(JNIEnv.CallIntMethodA), //virtual jint JNICALL CallIntMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual jlong JNICALL CallLongMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jlong JNICALL CallLongMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_long_IntPtr_IntPtr_pjvalue(JNIEnv.CallLongMethodA), //virtual jlong JNICALL CallLongMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual jfloat JNICALL CallFloatMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jfloat JNICALL CallFloatMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_float_IntPtr_IntPtr_pjvalue(JNIEnv.CallFloatMethodA), //virtual jfloat JNICALL CallFloatMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual jdouble JNICALL CallDoubleMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jdouble JNICALL CallDoubleMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_double_IntPtr_IntPtr_pjvalue(JNIEnv.CallDoubleMethodA), //virtual jdouble JNICALL CallDoubleMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual void JNICALL CallVoidMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual void JNICALL CallVoidMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_void_IntPtr_IntPtr_pjvalue(JNIEnv.CallVoidMethodA), //virtual void JNICALL CallVoidMethodA(jobject obj, jmethodID methodID, jvalue * args);

			null, //virtual jobject JNICALL CallNonvirtualObjectMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jobject JNICALL CallNonvirtualObjectMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_IntPtr_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallNonvirtualObjectMethodA), //virtual jobject JNICALL CallNonvirtualObjectMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue * args);

			null, //virtual jboolean JNICALL CallNonvirtualBooleanMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jboolean JNICALL CallNonvirtualBooleanMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_sbyte_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallNonvirtualBooleanMethodA), //virtual jboolean JNICALL CallNonvirtualBooleanMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue * args);

			null, //virtual jbyte JNICALL CallNonvirtualByteMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jbyte JNICALL CallNonvirtualByteMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_sbyte_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallNonvirtualByteMethodA), //virtual jbyte JNICALL CallNonvirtualByteMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jchar JNICALL CallNonvirtualCharMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jchar JNICALL CallNonvirtualCharMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_ushort_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallNonvirtualCharMethodA), //virtual jchar JNICALL CallNonvirtualCharMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jshort JNICALL CallNonvirtualShortMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jshort JNICALL CallNonvirtualShortMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_short_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallNonvirtualShortMethodA), //virtual jshort JNICALL CallNonvirtualShortMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jint JNICALL CallNonvirtualIntMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jint JNICALL CallNonvirtualIntMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_int_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallNonvirtualIntMethodA), //virtual jint JNICALL CallNonvirtualIntMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jlong JNICALL CallNonvirtualLongMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jlong JNICALL CallNonvirtualLongMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_long_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallNonvirtualLongMethodA), //virtual jlong JNICALL CallNonvirtualLongMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jfloat JNICALL CallNonvirtualFloatMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jfloat JNICALL CallNonvirtualFloatMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_float_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallNonvirtualFloatMethodA), //virtual jfloat JNICALL CallNonvirtualFloatMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jdouble JNICALL CallNonvirtualDoubleMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jdouble JNICALL CallNonvirtualDoubleMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_double_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallNonvirtualDoubleMethodA), //virtual jdouble JNICALL CallNonvirtualDoubleMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual void JNICALL CallNonvirtualVoidMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual void JNICALL CallNonvirtualVoidMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_void_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallNonvirtualVoidMethodA), //virtual void JNICALL CallNonvirtualVoidMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue * args);

			new pf_IntPtr_IntPtr_pbyte_pbyte(JNIEnv.GetFieldID), //virtual jfieldID JNICALL GetFieldID(jclass clazz, const char *name, const char *sig);

			new pf_IntPtr_IntPtr_IntPtr(JNIEnv.GetObjectField), //virtual jobject JNICALL GetObjectField(jobject obj, jfieldID fieldID);
			new pf_sbyte_IntPtr_IntPtr(JNIEnv.GetBooleanField), //virtual jboolean JNICALL GetBooleanField(jobject obj, jfieldID fieldID);
			new pf_sbyte_IntPtr_IntPtr(JNIEnv.GetByteField), //virtual jbyte JNICALL GetByteField(jobject obj, jfieldID fieldID);
			new pf_ushort_IntPtr_IntPtr(JNIEnv.GetCharField), //virtual jchar JNICALL GetCharField(jobject obj, jfieldID fieldID);
			new pf_short_IntPtr_IntPtr(JNIEnv.GetShortField), //virtual jshort JNICALL GetShortField(jobject obj, jfieldID fieldID);
			new pf_int_IntPtr_IntPtr(JNIEnv.GetIntField), //virtual jint JNICALL GetIntField(jobject obj, jfieldID fieldID);
			new pf_long_IntPtr_IntPtr(JNIEnv.GetLongField), //virtual jlong JNICALL GetLongField(jobject obj, jfieldID fieldID);
			new pf_float_IntPtr_IntPtr(JNIEnv.GetFloatField), //virtual jfloat JNICALL GetFloatField(jobject obj, jfieldID fieldID);
			new pf_double_IntPtr_IntPtr(JNIEnv.GetDoubleField), //virtual jdouble JNICALL GetDoubleField(jobject obj, jfieldID fieldID);

			new pf_void_IntPtr_IntPtr_IntPtr(JNIEnv.SetObjectField), //virtual void JNICALL SetObjectField(jobject obj, jfieldID fieldID, jobject val);
			new pf_void_IntPtr_IntPtr_sbyte(JNIEnv.SetBooleanField), //virtual void JNICALL SetBooleanField(jobject obj, jfieldID fieldID, jboolean val);
			new pf_void_IntPtr_IntPtr_sbyte(JNIEnv.SetByteField), //virtual void JNICALL SetByteField(jobject obj, jfieldID fieldID, jbyte val);
			new pf_void_IntPtr_IntPtr_ushort(JNIEnv.SetCharField), //virtual void JNICALL SetCharField(jobject obj, jfieldID fieldID, jchar val);
			new pf_void_IntPtr_IntPtr_short(JNIEnv.SetShortField), //virtual void JNICALL SetShortField(jobject obj, jfieldID fieldID, jshort val);
			new pf_void_IntPtr_IntPtr_int(JNIEnv.SetIntField), //virtual void JNICALL SetIntField(jobject obj, jfieldID fieldID, jint val);
			new pf_void_IntPtr_IntPtr_long(JNIEnv.SetLongField), //virtual void JNICALL SetLongField(jobject obj, jfieldID fieldID, jlong val);
			new pf_void_IntPtr_IntPtr_float(JNIEnv.SetFloatField), //virtual void JNICALL SetFloatField(jobject obj, jfieldID fieldID, jfloat val);
			new pf_void_IntPtr_IntPtr_double(JNIEnv.SetDoubleField), //virtual void JNICALL SetDoubleField(jobject obj, jfieldID fieldID, jdouble val);

			new pf_IntPtr_IntPtr_pbyte_pbyte(JNIEnv.GetStaticMethodID), //virtual jmethodID JNICALL GetStaticMethodID(jclass clazz, const char *name, const char *sig);

			null, //virtual jobject JNICALL CallStaticObjectMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jobject JNICALL CallStaticObjectMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallStaticObjectMethodA), //virtual jobject JNICALL CallStaticObjectMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jboolean JNICALL CallStaticBooleanMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jboolean JNICALL CallStaticBooleanMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_sbyte_IntPtr_IntPtr_pjvalue(JNIEnv.CallStaticBooleanMethodA), //virtual jboolean JNICALL CallStaticBooleanMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jbyte JNICALL CallStaticByteMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jbyte JNICALL CallStaticByteMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_sbyte_IntPtr_IntPtr_pjvalue(JNIEnv.CallStaticByteMethodA), //virtual jbyte JNICALL CallStaticByteMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jchar JNICALL CallStaticCharMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jchar JNICALL CallStaticCharMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_ushort_IntPtr_IntPtr_pjvalue(JNIEnv.CallStaticCharMethodA), //virtual jchar JNICALL CallStaticCharMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jshort JNICALL CallStaticShortMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jshort JNICALL CallStaticShortMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_short_IntPtr_IntPtr_pjvalue(JNIEnv.CallStaticShortMethodA), //virtual jshort JNICALL CallStaticShortMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jint JNICALL CallStaticIntMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jint JNICALL CallStaticIntMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_int_IntPtr_IntPtr_pjvalue(JNIEnv.CallStaticIntMethodA), //virtual jint JNICALL CallStaticIntMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jlong JNICALL CallStaticLongMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jlong JNICALL CallStaticLongMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_long_IntPtr_IntPtr_pjvalue(JNIEnv.CallStaticLongMethodA), //virtual jlong JNICALL CallStaticLongMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jfloat JNICALL CallStaticFloatMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jfloat JNICALL CallStaticFloatMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_float_IntPtr_IntPtr_pjvalue(JNIEnv.CallStaticFloatMethodA), //virtual jfloat JNICALL CallStaticFloatMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jdouble JNICALL CallStaticDoubleMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jdouble JNICALL CallStaticDoubleMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_double_IntPtr_IntPtr_pjvalue(JNIEnv.CallStaticDoubleMethodA), //virtual jdouble JNICALL CallStaticDoubleMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual void JNICALL CallStaticVoidMethod(jclass cls, jmethodID methodID, ...);
			null, //virtual void JNICALL CallStaticVoidMethodV(jclass cls, jmethodID methodID, va_list args);
			new pf_void_IntPtr_IntPtr_pjvalue(JNIEnv.CallStaticVoidMethodA), //virtual void JNICALL CallStaticVoidMethodA(jclass cls, jmethodID methodID, jvalue * args);

			new pf_IntPtr_IntPtr_pbyte_pbyte(JNIEnv.GetStaticFieldID), //virtual jfieldID JNICALL GetStaticFieldID(jclass clazz, const char *name, const char *sig);

			new pf_IntPtr_IntPtr_IntPtr(JNIEnv.GetStaticObjectField), //virtual jobject JNICALL GetObjectField(jobject obj, jfieldID fieldID);
			new pf_sbyte_IntPtr_IntPtr(JNIEnv.GetStaticBooleanField), //virtual jboolean JNICALL GetBooleanField(jobject obj, jfieldID fieldID);
			new pf_sbyte_IntPtr_IntPtr(JNIEnv.GetStaticByteField), //virtual jbyte JNICALL GetByteField(jobject obj, jfieldID fieldID);
			new pf_ushort_IntPtr_IntPtr(JNIEnv.GetStaticCharField), //virtual jchar JNICALL GetCharField(jobject obj, jfieldID fieldID);
			new pf_short_IntPtr_IntPtr(JNIEnv.GetStaticShortField), //virtual jshort JNICALL GetShortField(jobject obj, jfieldID fieldID);
			new pf_int_IntPtr_IntPtr(JNIEnv.GetStaticIntField), //virtual jint JNICALL GetIntField(jobject obj, jfieldID fieldID);
			new pf_long_IntPtr_IntPtr(JNIEnv.GetStaticLongField), //virtual jlong JNICALL GetLongField(jobject obj, jfieldID fieldID);
			new pf_float_IntPtr_IntPtr(JNIEnv.GetStaticFloatField), //virtual jfloat JNICALL GetFloatField(jobject obj, jfieldID fieldID);
			new pf_double_IntPtr_IntPtr(JNIEnv.GetStaticDoubleField), //virtual jdouble JNICALL GetDoubleField(jobject obj, jfieldID fieldID);

			new pf_void_IntPtr_IntPtr_IntPtr(JNIEnv.SetStaticObjectField), //virtual void JNICALL SetObjectField(jobject obj, jfieldID fieldID, jobject val);
			new pf_void_IntPtr_IntPtr_sbyte(JNIEnv.SetStaticBooleanField), //virtual void JNICALL SetBooleanField(jobject obj, jfieldID fieldID, jboolean val);
			new pf_void_IntPtr_IntPtr_sbyte(JNIEnv.SetStaticByteField), //virtual void JNICALL SetByteField(jobject obj, jfieldID fieldID, jbyte val);
			new pf_void_IntPtr_IntPtr_ushort(JNIEnv.SetStaticCharField), //virtual void JNICALL SetCharField(jobject obj, jfieldID fieldID, jchar val);
			new pf_void_IntPtr_IntPtr_short(JNIEnv.SetStaticShortField), //virtual void JNICALL SetShortField(jobject obj, jfieldID fieldID, jshort val);
			new pf_void_IntPtr_IntPtr_int(JNIEnv.SetStaticIntField), //virtual void JNICALL SetIntField(jobject obj, jfieldID fieldID, jint val);
			new pf_void_IntPtr_IntPtr_long(JNIEnv.SetStaticLongField), //virtual void JNICALL SetLongField(jobject obj, jfieldID fieldID, jlong val);
			new pf_void_IntPtr_IntPtr_float(JNIEnv.SetStaticFloatField), //virtual void JNICALL SetFloatField(jobject obj, jfieldID fieldID, jfloat val);
			new pf_void_IntPtr_IntPtr_double(JNIEnv.SetStaticDoubleField), //virtual void JNICALL SetDoubleField(jobject obj, jfieldID fieldID, jdouble val);

			new pf_IntPtr_pjchar_int(JNIEnv.NewString), //virtual jstring JNICALL NewString(const jchar *unicode, jsize len);
			new pf_int_IntPtr(JNIEnv.GetStringLength), //virtual jsize JNICALL GetStringLength(jstring str);
			new pf_pjchar_IntPtr_pjboolean(JNIEnv.GetStringChars), //virtual const jchar *JNICALL GetStringChars(jstring str, jboolean *isCopy);
			new pf_void_IntPtr_pjchar(JNIEnv.ReleaseStringChars), //virtual void JNICALL ReleaseStringChars(jstring str, const jchar *chars);

			new pf_IntPtr_pbyte(JNIEnv.NewStringUTF), //virtual jstring JNICALL NewStringUTF(const char *utf);
			new pf_int_IntPtr(JNIEnv.GetStringUTFLength), //virtual jsize JNICALL GetStringUTFLength(jstring str);
			new pf_pbyte_IntPtr_pjboolean(JNIEnv.GetStringUTFChars), //virtual const char* JNICALL GetStringUTFChars(jstring str, jboolean *isCopy);
			new pf_void_IntPtr_pbyte(JNIEnv.ReleaseStringUTFChars), //virtual void JNICALL ReleaseStringUTFChars(jstring str, const char* chars);

			new pf_int_IntPtr(JNIEnv.GetArrayLength), //virtual jsize JNICALL GetArrayLength(jarray array);

			new pf_IntPtr_int_IntPtr_IntPtr(JNIEnv.NewObjectArray), //virtual jobjectArray JNICALL NewObjectArray(jsize len, jclass clazz, jobject init);
			new pf_IntPtr_IntPtr_int(JNIEnv.GetObjectArrayElement), //virtual jobject JNICALL GetObjectArrayElement(jobjectArray array, jsize index);
			new pf_void_IntPtr_int_IntPtr(JNIEnv.SetObjectArrayElement), //virtual void JNICALL SetObjectArrayElement(jobjectArray array, jsize index, jobject val);

			new pf_IntPtr_int(JNIEnv.NewBooleanArray), //virtual jbooleanArray JNICALL NewBooleanArray(jsize len);
			new pf_IntPtr_int(JNIEnv.NewByteArray), //virtual jbyteArray JNICALL NewByteArray(jsize len);
			new pf_IntPtr_int(JNIEnv.NewCharArray), //virtual jcharArray JNICALL NewCharArray(jsize len);
			new pf_IntPtr_int(JNIEnv.NewShortArray), //virtual jshortArray JNICALL NewShortArray(jsize len);
			new pf_IntPtr_int(JNIEnv.NewIntArray), //virtual jintArray JNICALL NewIntArray(jsize len);
			new pf_IntPtr_int(JNIEnv.NewLongArray), //virtual jlongArray JNICALL NewLongArray(jsize len);
			new pf_IntPtr_int(JNIEnv.NewFloatArray), //virtual jfloatArray JNICALL NewFloatArray(jsize len);
			new pf_IntPtr_int(JNIEnv.NewDoubleArray), //virtual jdoubleArray JNICALL NewDoubleArray(jsize len);

			new pf_pjboolean_IntPtr_pjboolean(JNIEnv.GetBooleanArrayElements), //virtual jboolean * JNICALL GetBooleanArrayElements(jbooleanArray array, jboolean *isCopy);
			new pf_pjbyte_IntPtr_pjboolean(JNIEnv.GetByteArrayElements), //virtual jbyte * JNICALL GetByteArrayElements(jbyteArray array, jboolean *isCopy);
			new pf_pjchar_IntPtr_pjboolean(JNIEnv.GetCharArrayElements), //virtual jchar * JNICALL GetCharArrayElements(jcharArray array, jboolean *isCopy);
			new pf_pjshort_IntPtr_pjboolean(JNIEnv.GetShortArrayElements), //virtual jshort * JNICALL GetShortArrayElements(jshortArray array, jboolean *isCopy);
			new pf_pjint_IntPtr_pjboolean(JNIEnv.GetIntArrayElements), //virtual jint * JNICALL GetIntArrayElements(jintArray array, jboolean *isCopy);
			new pf_pjlong_IntPtr_pjboolean(JNIEnv.GetLongArrayElements), //virtual jlong * JNICALL GetLongArrayElements(jlongArray array, jboolean *isCopy);
			new pf_pjfloat_IntPtr_pjboolean(JNIEnv.GetFloatArrayElements), //virtual jfloat * JNICALL GetFloatArrayElements(jfloatArray array, jboolean *isCopy);
			new pf_pjdouble_IntPtr_pjboolean(JNIEnv.GetDoubleArrayElements), //virtual jdouble * JNICALL GetDoubleArrayElements(jdoubleArray array, jboolean *isCopy);

			new pf_void_IntPtr_pjboolean_int(JNIEnv.ReleaseBooleanArrayElements), //virtual void JNICALL ReleaseBooleanArrayElements(jbooleanArray array, jboolean *elems, jint mode);
			new pf_void_IntPtr_pjbyte_int(JNIEnv.ReleaseByteArrayElements), //virtual void JNICALL ReleaseByteArrayElements(jbyteArray array, jbyte *elems, jint mode);
			new pf_void_IntPtr_pjchar_int(JNIEnv.ReleaseCharArrayElements), //virtual void JNICALL ReleaseCharArrayElements(jcharArray array, jchar *elems, jint mode);
			new pf_void_IntPtr_pjshort_int(JNIEnv.ReleaseShortArrayElements), //virtual void JNICALL ReleaseShortArrayElements(jshortArray array, jshort *elems, jint mode);
			new pf_void_IntPtr_pjint_int(JNIEnv.ReleaseIntArrayElements), //virtual void JNICALL ReleaseIntArrayElements(jintArray array, jint *elems, jint mode);
			new pf_void_IntPtr_pjlong_int(JNIEnv.ReleaseLongArrayElements), //virtual void JNICALL ReleaseLongArrayElements(jlongArray array, jlong *elems, jint mode);
			new pf_void_IntPtr_pjfloat_int(JNIEnv.ReleaseFloatArrayElements), //virtual void JNICALL ReleaseFloatArrayElements(jfloatArray array, jfloat *elems, jint mode);
			new pf_void_IntPtr_pjdouble_int(JNIEnv.ReleaseDoubleArrayElements), //virtual void JNICALL ReleaseDoubleArrayElements(jdoubleArray array, jdouble *elems, jint mode);

			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.GetBooleanArrayRegion), //virtual void JNICALL GetBooleanArrayRegion(jbooleanArray array, jsize start, jsize l, jboolean *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.GetByteArrayRegion), //virtual void JNICALL GetByteArrayRegion(jbyteArray array, jsize start, jsize len, jbyte *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.GetCharArrayRegion), //virtual void JNICALL GetCharArrayRegion(jcharArray array, jsize start, jsize len, jchar *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.GetShortArrayRegion), //virtual void JNICALL GetShortArrayRegion(jshortArray array, jsize start, jsize len, jshort *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.GetIntArrayRegion), //virtual void JNICALL GetIntArrayRegion(jintArray array, jsize start, jsize len, jint *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.GetLongArrayRegion), //virtual void JNICALL GetLongArrayRegion(jlongArray array, jsize start, jsize len, jlong *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.GetFloatArrayRegion), //virtual void JNICALL GetFloatArrayRegion(jfloatArray array, jsize start, jsize len, jfloat *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.GetDoubleArrayRegion), //virtual void JNICALL GetDoubleArrayRegion(jdoubleArray array, jsize start, jsize len, jdouble *buf);

			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.SetBooleanArrayRegion), //virtual void JNICALL SetBooleanArrayRegion(jbooleanArray array, jsize start, jsize l, jboolean *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.SetByteArrayRegion), //virtual void JNICALL SetByteArrayRegion(jbyteArray array, jsize start, jsize len, jbyte *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.SetCharArrayRegion), //virtual void JNICALL SetCharArrayRegion(jcharArray array, jsize start, jsize len, jchar *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.SetShortArrayRegion), //virtual void JNICALL SetShortArrayRegion(jshortArray array, jsize start, jsize len, jshort *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.SetIntArrayRegion), //virtual void JNICALL SetIntArrayRegion(jintArray array, jsize start, jsize len, jint *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.SetLongArrayRegion), //virtual void JNICALL SetLongArrayRegion(jlongArray array, jsize start, jsize len, jlong *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.SetFloatArrayRegion), //virtual void JNICALL SetFloatArrayRegion(jfloatArray array, jsize start, jsize len, jfloat *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.SetDoubleArrayRegion), //virtual void JNICALL SetDoubleArrayRegion(jdoubleArray array, jsize start, jsize len, jdouble *buf);

			new pf_int_IntPtr_pJNINativeMethod_int(JNIEnv.RegisterNatives), //virtual jint JNICALL RegisterNatives(jclass clazz, const JNINativeMethod *methods, jint nMethods);
			new pf_int_IntPtr(JNIEnv.UnregisterNatives), //virtual jint JNICALL UnregisterNatives(jclass clazz);

			new pf_int_IntPtr(JNIEnv.MonitorEnter), //virtual jint JNICALL MonitorEnter(jobject obj);
			new pf_int_IntPtr(JNIEnv.MonitorExit), //virtual jint JNICALL MonitorExit(jobject obj);

			new pf_int_ppJavaVM(JNIEnv.GetJavaVM), //virtual jint JNICALL GetJavaVM(JavaVM **vm);

			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.GetStringRegion), //virtual void JNICALL GetStringRegion(jstring str, jsize start, jsize len, jchar *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.GetStringUTFRegion), //virtual void JNICALL GetStringUTFRegion(jstring str, jsize start, jsize len, char *buf);

			new pf_pvoid_IntPtr_pjboolean(JNIEnv.GetPrimitiveArrayCritical), //virtual void* JNICALL GetPrimitiveArrayCritical(jarray array, jboolean *isCopy);
			new pf_void_IntPtr_pvoid_int(JNIEnv.ReleasePrimitiveArrayCritical), //virtual void JNICALL ReleasePrimitiveArrayCritical(jarray array, void *carray, jint mode);

			new pf_pjchar_IntPtr_pjboolean(JNIEnv.GetStringCritical), //virtual const jchar* JNICALL GetStringCritical(jstring string, jboolean *isCopy);
			new pf_void_IntPtr_pjchar(JNIEnv.ReleaseStringCritical), //virtual void JNICALL ReleaseStringCritical(jstring string, const jchar *cstring);

			new pf_IntPtr_IntPtr(JNIEnv.NewWeakGlobalRef), //virtual jweak JNICALL NewWeakGlobalRef(jobject obj);
			new pf_void_IntPtr(JNIEnv.DeleteWeakGlobalRef), //virtual void JNICALL DeleteWeakGlobalRef(jweak ref);

			new pf_sbyte(JNIEnv.ExceptionCheck), //virtual jboolean JNICALL ExceptionCheck();

			new pf_IntPtr_IntPtr_long(JNIEnv.NewDirectByteBuffer), //virtual jobject JNICALL NewDirectByteBuffer(void* address, jlong capacity);
			new pf_IntPtr_IntPtr(JNIEnv.GetDirectBufferAddress), //virtual void* JNICALL GetDirectBufferAddress(jobject buf);
			new pf_long_IntPtr(JNIEnv.GetDirectBufferCapacity), //virtual jlong JNICALL GetDirectBufferCapacity(jobject buf);

			new pf_int_IntPtr(JNIEnv.GetObjectRefType) // virtual jobjectRefType GetObjectRefType(jobject obj);
		};
	}

	[StructLayout(LayoutKind.Sequential)]
	unsafe struct JavaVMAttachArgs
	{
		internal jint version;
		internal byte* name;
		internal jobject group;
	}

	[StructLayout(LayoutKind.Sequential)]
	unsafe struct JavaVM
	{
		internal static JavaVM* pJavaVM;
		void** vtable;
		void* firstVtableEntry;
		delegate int pf_int(JavaVM* pJVM);
		delegate int pf_int_ppvoid_pvoid(JavaVM* pJVM, void** p1, void* p2);
		delegate int pf_int_ppvoid_int(JavaVM* pJVM, void** p1, int p2);

		static Delegate[] vtableDelegates =
		{
			null,
			null,
			null,
			new pf_int(DestroyJavaVM),
			new pf_int_ppvoid_pvoid(AttachCurrentThread),
			new pf_int(DetachCurrentThread),
			new pf_int_ppvoid_int(GetEnv),
			new pf_int_ppvoid_pvoid(AttachCurrentThreadAsDaemon)
		};

		static JavaVM()
		{
			JNI.jvmCreated = true;
			pJavaVM = (JavaVM*)(void*)JniMem.Alloc(IntPtr.Size * (1 + vtableDelegates.Length));
			pJavaVM->vtable = &pJavaVM->firstVtableEntry;
			for(int i = 0; i < vtableDelegates.Length; i++)
			{
				pJavaVM->vtable[i] = NativeLibrary.MarshalDelegate(vtableDelegates[i]);
			}
		}

		internal static jint DestroyJavaVM(JavaVM* pJVM)
		{
			if(JNI.jvmDestroyed)
			{
				return JNIEnv.JNI_ERR;
			}
			JNI.jvmDestroyed = true;
			Java_java_lang_Thread.WaitUntilLastJniThread();
			return JNIEnv.JNI_OK;
		}

		internal static jint AttachCurrentThread(JavaVM* pJVM, void **penv, void *args)
		{
			return AttachCurrentThreadImpl(pJVM, penv, (JavaVMAttachArgs*)args, false);
		}

		internal static jint AttachCurrentThreadImpl(JavaVM* pJVM, void** penv, JavaVMAttachArgs* pAttachArgs, bool asDaemon)
		{
			if(pAttachArgs != null)
			{
				if(!JNI.IsSupportedJniVersion(pAttachArgs->version) || pAttachArgs->version == JNIEnv.JNI_VERSION_1_1)
				{
					*penv = null;
					return JNIEnv.JNI_EVERSION;
				}
			}
			JNIEnv.ManagedJNIEnv env = TlsHack.ManagedJNIEnv;
			if(env != null)
			{
				*penv = env.pJNIEnv;
				return JNIEnv.JNI_OK;
			}
			// NOTE if we're here, it is *very* likely that the thread was created by native code and not by managed code,
			// but it's not impossible that the thread started life as a managed thread and if it did the changes to the
			// thread we're making are somewhat dubious.
			System.Threading.Thread.CurrentThread.IsBackground = asDaemon;
			if(pAttachArgs != null)
			{
				if(pAttachArgs->name != null && System.Threading.Thread.CurrentThread.Name == null)
				{
					try
					{
						System.Threading.Thread.CurrentThread.Name = JNIEnv.StringFromUTF8(pAttachArgs->name);
					}
					catch(InvalidOperationException)
					{
						// someone beat us to it...
					}
				}
				object threadGroup = GlobalRefs.Unwrap(pAttachArgs->group.ToInt32());
				if(threadGroup != null)
				{
					Java_java_lang_Thread.AttachThreadFromJni(threadGroup);
				}
			}
			*penv = JNIEnv.CreateJNIEnv();
			return JNIEnv.JNI_OK;
		}

		internal static jint DetachCurrentThread(JavaVM* pJVM)
		{
			if(TlsHack.ManagedJNIEnv == null)
			{
				// the JDK allows detaching from an already detached thread
				return JNIEnv.JNI_OK;
			}
			// TODO if we set Thread.IsBackground to false when we attached, now might be a good time to set it back to true.
			JNIEnv.FreeJNIEnv();
			Java_ikvm_runtime_Startup.jniDetach();
			return JNIEnv.JNI_OK;
		}

		internal static jint GetEnv(JavaVM* pJVM, void **penv, jint version)
		{
			if(JNI.IsSupportedJniVersion(version))
			{
				JNIEnv.ManagedJNIEnv env = TlsHack.ManagedJNIEnv;
				if(env != null)
				{
					*penv = env.pJNIEnv;
					return JNIEnv.JNI_OK;
				}
				*penv = null;
				return JNIEnv.JNI_EDETACHED;
			}
			*penv = null;
			return JNIEnv.JNI_EVERSION;
		}

		internal static jint AttachCurrentThreadAsDaemon(JavaVM* pJVM, void **penv, void *args)
		{
			return AttachCurrentThreadImpl(pJVM, penv, (JavaVMAttachArgs*)args, true);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	unsafe struct JNIEnv
	{
		internal const int JNI_OK = 0;
		internal const int JNI_ERR = -1;
		internal const int JNI_EDETACHED = -2;
		internal const int JNI_EVERSION = -3;
		internal const int JNI_COMMIT = 1;
		internal const int JNI_ABORT = 2;
		internal const int JNI_VERSION_1_1 = 0x00010001;
		internal const int JNI_VERSION_1_2 = 0x00010002;
		internal const int JNI_VERSION_1_4 = 0x00010004;
		internal const int JNI_VERSION_1_6 = 0x00010006;
		internal const int JNI_VERSION_1_8 = 0x00010008;
		internal const int JNIInvalidRefType = 0;
		internal const int JNILocalRefType = 1;
		internal const int JNIGlobalRefType = 2;
		internal const int JNIWeakGlobalRefType = 3;
		internal const sbyte JNI_TRUE = 1;
		internal const sbyte JNI_FALSE = 0;
		private void* vtable;
		private GCHandle managedJNIEnv;
		private GCHandle* pinHandles;
		private int pinHandleMaxCount;
		private int pinHandleInUseCount;

		static JNIEnv()
		{
			// we set the field here so that IKVM.Runtime.dll doesn't have to load us if we're not otherwise needed
			Java_java_lang_SecurityManager.jniAssembly = typeof(JNIEnv).Assembly;
		}

		internal ManagedJNIEnv GetManagedJNIEnv()
		{
			return (ManagedJNIEnv)managedJNIEnv.Target;
		}

		internal sealed class ManagedJNIEnv
		{
			// NOTE the initial bucket size must be a power of two < LOCAL_REF_MAX_BUCKET_SIZE,
			// because each time we grow it, we double the size and it must eventually reach
			// exactly LOCAL_REF_MAX_BUCKET_SIZE
			private const int LOCAL_REF_INITIAL_BUCKET_SIZE = 32;
			private const int LOCAL_REF_SHIFT = 10;
			private const int LOCAL_REF_MAX_BUCKET_SIZE = (1 << LOCAL_REF_SHIFT);
			private const int LOCAL_REF_MASK = (LOCAL_REF_MAX_BUCKET_SIZE - 1);
			internal readonly JNIEnv* pJNIEnv;
			internal ClassLoaderWrapper classLoader;
			internal ikvm.@internal.CallerID callerID;
			private object[][] localRefs;
			private int localRefSlot;
			private int localRefIndex;
			private object[] active;
			internal Exception pendingException;

			internal ManagedJNIEnv()
			{
				pJNIEnv = (JNIEnv*)JniMem.Alloc(sizeof(JNIEnv));
				localRefs = new object[32][];
				active = localRefs[0] = new object[LOCAL_REF_INITIAL_BUCKET_SIZE];
				// stuff something in the first entry to make sure we don't hand out a zero handle
				// (a zero handle corresponds to a null reference)
				active[0] = "";
				localRefIndex = 1;
			}

			~ManagedJNIEnv()
			{
				// NOTE don't clean up when we're being unloaded (we'll get cleaned up anyway and because
				// of the unorderedness of the finalization process native code could still be run after
				// we run).
				// NOTE when we're not the default AppDomain and we're being unloaded,
				// we're leaking the JNIEnv (but since JNI outside of the default AppDomain isn't currently supported,
				// I can live with that).
				if(!Environment.HasShutdownStarted)
				{
					if(pJNIEnv->managedJNIEnv.IsAllocated)
					{
						pJNIEnv->managedJNIEnv.Free();
					}
					for(int i = 0; i < pJNIEnv->pinHandleMaxCount; i++)
					{
						if(pJNIEnv->pinHandles[i].IsAllocated)
						{
							pJNIEnv->pinHandles[i].Free();
						}
					}
					JniMem.Free((IntPtr)(void*)pJNIEnv);
				}
			}

			internal struct FrameState
			{
				internal readonly ikvm.@internal.CallerID callerID;
				internal readonly int localRefSlot;
				internal readonly int localRefIndex;

				internal FrameState(ikvm.@internal.CallerID callerID, int localRefSlot, int localRefIndex)
				{
					this.callerID = callerID;
					this.localRefSlot = localRefSlot;
					this.localRefIndex = localRefIndex;
				}
			}

			internal FrameState Enter(ikvm.@internal.CallerID newCallerID)
			{
				FrameState prev = new FrameState(callerID, localRefSlot, localRefIndex);
				this.callerID = newCallerID;
				localRefSlot++;
				if (localRefSlot >= localRefs.Length)
				{
					object[][] tmp = new object[localRefs.Length * 2][];
					Array.Copy(localRefs, 0, tmp, 0, localRefs.Length);
					localRefs = tmp;
				}
				localRefIndex = 0;
				active = localRefs[localRefSlot];
				if (active == null)
				{
					active = localRefs[localRefSlot] = new object[LOCAL_REF_INITIAL_BUCKET_SIZE];
				}
				return prev;
			}

			internal Exception Leave(FrameState prev)
			{
				// on the current (.NET 2.0 SP2) x86 JIT an explicit for loop is faster than Array.Clear() up to about 100 elements
				for (int i = 0; i < localRefIndex; i++)
				{
					active[i] = null;
				}
				while (--localRefSlot != prev.localRefSlot)
				{
					if (localRefs[localRefSlot] != null)
					{
						if (localRefs[localRefSlot].Length == LOCAL_REF_MAX_BUCKET_SIZE)
						{
							// if the bucket is totally allocated, we're assuming a leaky method so we throw the bucket away
							localRefs[localRefSlot] = null;
						}
						else
						{
							Array.Clear(localRefs[localRefSlot], 0, localRefs[localRefSlot].Length);
						}
					}
				}
				active = localRefs[localRefSlot];
				this.localRefIndex = prev.localRefIndex;
				this.callerID = prev.callerID;
				Exception x = pendingException;
				pendingException = null;
				return x;
			}

			internal jobject MakeLocalRef(object obj)
			{
				if (obj == null)
				{
					return IntPtr.Zero;
				}

				int index;
				if (localRefIndex == active.Length)
				{
					index = FindFreeIndex();
				}
				else
				{
					index = localRefIndex++;
				}
				active[index] = obj;
				return (IntPtr)((localRefSlot << LOCAL_REF_SHIFT) + index);
			}

			private int FindFreeIndex()
			{
				for (int i = 0; i < active.Length; i++)
				{
					if (active[i] == null)
					{
						while (localRefIndex - 1 > i && active[localRefIndex - 1] == null)
						{
							localRefIndex--;
						}
						return i;
					}
				}
				GrowActiveSlot();
				return localRefIndex++;
			}

			private void GrowActiveSlot()
			{
				if (active.Length < LOCAL_REF_MAX_BUCKET_SIZE)
				{
					object[] tmp = new object[active.Length * 2];
					Array.Copy(active, tmp, active.Length);
					active = localRefs[localRefSlot] = tmp;
					return;
				}
				// if we get here, we're in a native method that most likely is leaking locals refs,
				// so we're going to allocate a new bucket and increment localRefSlot, this means that
				// any slots that become available in the previous bucket are not going to be reused,
				// but since we're assuming that the method is leaking anyway, that isn't a problem
				// (it's never a correctness issue, just a resource consumption issue)
				localRefSlot++;
				localRefIndex = 0;
				if (localRefSlot == localRefs.Length)
				{
					object[][] tmp = new object[localRefSlot * 2][];
					Array.Copy(localRefs, 0, tmp, 0, localRefSlot);
					localRefs = tmp;
				}
				active = localRefs[localRefSlot];
				if (active == null)
				{
					active = localRefs[localRefSlot] = new object[LOCAL_REF_MAX_BUCKET_SIZE];
				}
			}

			internal object UnwrapLocalRef(int i)
			{
				return localRefs[i >> LOCAL_REF_SHIFT][i & LOCAL_REF_MASK];
			}

			internal int PushLocalFrame(jint capacity)
			{
				localRefSlot += 2;
				if (localRefSlot >= localRefs.Length)
				{
					object[][] tmp = new object[localRefs.Length * 2][];
					Array.Copy(localRefs, 0, tmp, 0, localRefs.Length);
					localRefs = tmp;
				}
				// we use a null slot to mark the fact that we used PushLocalFrame
				localRefs[localRefSlot - 1] = null;
				if (localRefs[localRefSlot] == null)
				{
					// we can't use capacity directly, because the array length must be a power of two
					// and it can't be bigger than LOCAL_REF_MAX_BUCKET_SIZE
					int r = 1;
					capacity = Math.Min(capacity, LOCAL_REF_MAX_BUCKET_SIZE);
					while (r < capacity)
					{
						r *= 2;
					}
					localRefs[localRefSlot] = new object[r];
				}
				localRefIndex = 0;
				active = localRefs[localRefSlot];
				return JNI_OK;
			}

			internal jobject PopLocalFrame(object res)
			{
				while (localRefs[localRefSlot] != null)
				{
					localRefs[localRefSlot] = null;
					localRefSlot--;
				}
				localRefSlot--;
				localRefIndex = localRefs[localRefSlot].Length;
				active = localRefs[localRefSlot];
				return MakeLocalRef(res);
			}

			internal void DeleteLocalRef(jobject obj)
			{
				int i = obj.ToInt32();
				if (i > 0)
				{
					localRefs[i >> LOCAL_REF_SHIFT][i & LOCAL_REF_MASK] = null;
					return;
				}
				if (i < 0)
				{
					Debug.Assert(false, "bogus localref in DeleteLocalRef");
				}
			}
		}

		internal static JNIEnv* CreateJNIEnv()
		{
			ManagedJNIEnv env = new ManagedJNIEnv();
			TlsHack.ManagedJNIEnv = env;
			JNIEnv* pJNIEnv = env.pJNIEnv;
			pJNIEnv->vtable = VtableBuilder.vtable;
			pJNIEnv->managedJNIEnv = GCHandle.Alloc(env, GCHandleType.WeakTrackResurrection);
			pJNIEnv->pinHandles = null;
			pJNIEnv->pinHandleMaxCount = 0;
			pJNIEnv->pinHandleInUseCount = 0;
			return pJNIEnv;
		}

		internal static void FreeJNIEnv()
		{
			TlsHack.ManagedJNIEnv = null;
		}

		internal static string StringFromOEM(byte* psz)
		{
			for(int i = 0;; i++)
			{
				if(psz[i] == 0)
				{
					int oem = System.Globalization.CultureInfo.CurrentCulture.TextInfo.OEMCodePage;
					return new String((sbyte*)psz, 0, i, Encoding.GetEncoding(oem));
				}
			}
		}

		internal static string StringFromUTF8(byte* psz)
		{
			// Sun's modified UTF8 encoding is not compatible with System.Text.Encoding.UTF8,
			// so we need to roll our own
			int len = 0;
			bool hasNonAscii = false;
			while(psz[len] != 0)
			{
				hasNonAscii |= psz[len] >= 128;
				len++;
			}
			if(!hasNonAscii)
			{
				// optimize the common case of 7-bit ASCII
				return new String((sbyte*)psz);
			}
			StringBuilder sb = new StringBuilder(len);
			for(int i = 0; i < len; i++)
			{
				int c = *psz++;
				int char2, char3;
				switch(c >> 4)
				{
					case 12:
					case 13:
						char2 = *psz++;
						i++;
						c = (((c & 0x1F) << 6) | (char2 & 0x3F));
						break;
					case 14:
						char2 = *psz++;
						char3 = *psz++;
						i++;
						i++;
						c = ((c & 0x0F) << 12) | ((char2 & 0x3F) << 6) | (char3 & 0x3F);
						break;
				}
				sb.Append((char)c);
			}
			return sb.ToString();
		}

		private static int StringUTF8Length(string s)
		{
			int len = 0;
			for(int i = 0; i < s.Length; i++)
			{
				char ch = s[i];
				if((ch != 0) && (ch <= 0x7F))
				{
					len++;
				}
				else if(ch <= 0x7FF)
				{
					len += 2;
				}
				else
				{
					len += 3;
				}
			}
			return len;
		}

		internal static jint GetMethodArgs(JNIEnv* pEnv, IntPtr method, byte* sig)
		{
			TypeWrapper[] argTypes = MethodWrapper.FromCookie(method).GetParameters();
			for (int i = 0; i < argTypes.Length; i++)
			{
				TypeWrapper tw = argTypes[i];
				if (tw.IsPrimitive)
				{
					sig[i] = (byte)tw.SigName[0];
				}
				else
				{
					sig[i] = (byte)'L';
				}
			}
			return argTypes.Length;
		}

		internal static jint GetVersion(JNIEnv* pEnv)
		{
			return JNI_VERSION_1_8;
		}

		internal static jclass DefineClass(JNIEnv* pEnv, byte* name, jobject loader, jbyte* pbuf, jint length)
		{
			try
			{
				byte[] buf = new byte[length];
				Marshal.Copy((IntPtr)(void*)pbuf, buf, 0, length);
				// TODO what should the protection domain be?
				// NOTE I'm assuming name is platform encoded (as opposed to UTF-8), but the Sun JVM only seems to work for ASCII.
				global::java.lang.ClassLoader classLoader = (global::java.lang.ClassLoader)pEnv->UnwrapRef(loader);
				return pEnv->MakeLocalRef(Java_java_lang_ClassLoader.defineClass0(classLoader, name != null ? StringFromOEM(name) : null, buf, 0, buf.Length, null));
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, x);
				return IntPtr.Zero;
			}
		}

		private static ClassLoaderWrapper FindNativeMethodClassLoader(JNIEnv* pEnv)
		{
			ManagedJNIEnv env = pEnv->GetManagedJNIEnv();
			if(env.callerID != null)
			{
				return ClassLoaderWrapper.FromCallerID(env.callerID);
			}
			if(env.classLoader != null)
			{
				return env.classLoader;
			}
			return ClassLoaderWrapper.GetClassLoaderWrapper(java.lang.ClassLoader.getSystemClassLoader());
		}

		internal static jclass FindClass(JNIEnv* pEnv, byte* pszName)
		{
			try
			{
				string name = StringFromOEM(pszName);
				// don't allow dotted names!
				if(name.IndexOf('.') >= 0)
				{
					SetPendingException(pEnv, new java.lang.NoClassDefFoundError(name));
					return IntPtr.Zero;
				}
				// spec doesn't say it, but Sun allows signature format class names (but not for primitives)
				if(name.StartsWith("L") && name.EndsWith(";"))
				{
					name = name.Substring(1, name.Length - 2);
				}
				TypeWrapper wrapper = FindNativeMethodClassLoader(pEnv).LoadClassByDottedNameFast(name.Replace('/', '.'));
				if(wrapper == null)
				{
					SetPendingException(pEnv, new java.lang.NoClassDefFoundError(name));
					return IntPtr.Zero;
				}
				wrapper.Finish();
				// spec doesn't say it, but Sun runs the static initializer
				wrapper.RunClassInit();
				return pEnv->MakeLocalRef(wrapper.ClassObject);
			}
			catch(Exception x)
			{
				if(x is RetargetableJavaException)
				{
					x = ((RetargetableJavaException)x).ToJava();
				}
				SetPendingException(pEnv, x);
				return IntPtr.Zero;
			}
		}

		internal static jmethodID FromReflectedMethod(JNIEnv* pEnv, jobject method)
		{
			return MethodWrapper.FromExecutable((java.lang.reflect.Executable)pEnv->UnwrapRef(method)).Cookie;
		}

		internal static jfieldID FromReflectedField(JNIEnv* pEnv, jobject field)
		{
			return FieldWrapper.FromField((java.lang.reflect.Field)pEnv->UnwrapRef(field)).Cookie;
		}

		internal static jobject ToReflectedMethod(JNIEnv* pEnv, jclass clazz_ignored, jmethodID method, jboolean isStatic)
		{
			return pEnv->MakeLocalRef(MethodWrapper.FromCookie(method).ToMethodOrConstructor(true));
		}

		internal static jclass GetSuperclass(JNIEnv* pEnv, jclass sub)
		{
			TypeWrapper wrapper = TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(sub)).BaseTypeWrapper;
			return pEnv->MakeLocalRef(wrapper == null ? null : wrapper.ClassObject);
		}

		internal static jboolean IsAssignableFrom(JNIEnv* pEnv, jclass sub, jclass super)
		{
			TypeWrapper w1 = TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(sub));
			TypeWrapper w2 = TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(super));
			return w1.IsAssignableTo(w2) ? JNI_TRUE : JNI_FALSE;
		}

		internal static jobject ToReflectedField(JNIEnv* pEnv, jclass clazz_ignored, jfieldID field, jboolean isStatic)
		{
			return pEnv->MakeLocalRef(FieldWrapper.FromCookie(field).ToField(true));
		}

		private static void SetPendingException(JNIEnv* pEnv, Exception x)
		{
			pEnv->GetManagedJNIEnv().pendingException = ikvm.runtime.Util.mapException(x);
		}

		internal static jint Throw(JNIEnv* pEnv, jthrowable throwable)
		{
			ManagedJNIEnv env = pEnv->GetManagedJNIEnv();
			Exception x = UnwrapRef(env, throwable) as Exception;
			if (x != null)
			{
				env.pendingException = x;
			}
			return JNI_OK;
		}

		internal static jint ThrowNew(JNIEnv* pEnv, jclass clazz, byte* msg)
		{
			ManagedJNIEnv env = pEnv->GetManagedJNIEnv();
			TypeWrapper wrapper = TypeWrapper.FromClass((java.lang.Class)UnwrapRef(env, clazz));
			MethodWrapper mw = wrapper.GetMethodWrapper("<init>", msg == null ? "()V" : "(Ljava.lang.String;)V", false);
			if(mw != null)
			{
				jint rc;
				Exception exception;
				try
				{
					wrapper.Finish();
					java.lang.reflect.Constructor cons = (java.lang.reflect.Constructor)mw.ToMethodOrConstructor(false);
					exception = (Exception)cons.newInstance(msg == null ? new object[0] : new object[] { StringFromOEM(msg) }, env.callerID);
					rc = JNI_OK;
				}
				catch(RetargetableJavaException x)
				{
					exception = x.ToJava();
					rc = JNI_ERR;
				}
				catch(Exception x)
				{
					exception = x;
					rc = JNI_ERR;
				}
				SetPendingException(pEnv, exception);
				return rc;
			}
			else
			{
				SetPendingException(pEnv, new java.lang.NoSuchMethodError("<init>(Ljava.lang.String;)V"));
				return JNI_ERR;
			}
		}

		internal static jthrowable ExceptionOccurred(JNIEnv* pEnv)
		{
			ManagedJNIEnv env = pEnv->GetManagedJNIEnv();
			return pEnv->MakeLocalRef(env.pendingException);
		}

		internal static void ExceptionDescribe(JNIEnv* pEnv)
		{
			ManagedJNIEnv env = pEnv->GetManagedJNIEnv();
			Exception x = env.pendingException;
			if(x != null)
			{
				env.pendingException = null;
				try
				{
					ikvm.extensions.ExtensionMethods.printStackTrace(x);
				}
				catch(Exception ex)
				{
					Debug.Assert(false, ex.ToString());
				}
			}
		}

		internal static void ExceptionClear(JNIEnv* pEnv)
		{
			ManagedJNIEnv env = pEnv->GetManagedJNIEnv();
			env.pendingException = null;
		}

		internal static void FatalError(JNIEnv* pEnv, byte* msg)
		{
			Console.Error.WriteLine("FATAL ERROR in native method: {0}", msg == null ? "(null)" : StringFromOEM(msg));
			Console.Error.WriteLine(new StackTrace(1, true));
			Environment.Exit(1);
		}

		internal static jint PushLocalFrame(JNIEnv* pEnv, jint capacity)
		{
			return pEnv->GetManagedJNIEnv().PushLocalFrame(capacity);
		}

		internal static jobject PopLocalFrame(JNIEnv* pEnv, jobject result)
		{
			ManagedJNIEnv env = pEnv->GetManagedJNIEnv();
			return env.PopLocalFrame(UnwrapRef(env, result));
		}

		internal static jobject NewGlobalRef(JNIEnv* pEnv, jobject obj)
		{
			object o = pEnv->UnwrapRef(obj);
			if(o == null)
			{
				return IntPtr.Zero;
			}
			lock(GlobalRefs.globalRefs)
			{
				int index = GlobalRefs.globalRefs.IndexOf(null);
				if(index >= 0)
				{
					GlobalRefs.globalRefs[index] = o;
				}
				else
				{
					index = GlobalRefs.globalRefs.Add(o);
				}
				return (IntPtr)(-(index + 1));
			}
		}

		internal static void DeleteGlobalRef(JNIEnv* pEnv, jobject obj)
		{
			int i = obj.ToInt32();
			if(i < 0)
			{
				lock(GlobalRefs.globalRefs)
				{
					GlobalRefs.globalRefs[(-i) - 1] = null;
				}
				return;
			}
			if(i > 0)
			{
				Debug.Assert(false, "Local ref passed to DeleteGlobalRef");
			}
		}

		internal static void DeleteLocalRef(JNIEnv* pEnv, jobject obj)
		{
			pEnv->GetManagedJNIEnv().DeleteLocalRef(obj);
		}

		internal static jboolean IsSameObject(JNIEnv* pEnv, jobject obj1, jobject obj2)
		{
			return pEnv->UnwrapRef(obj1) == pEnv->UnwrapRef(obj2) ? JNI_TRUE : JNI_FALSE;
		}

		internal static jobject NewLocalRef(JNIEnv* pEnv, jobject obj)
		{
			return pEnv->MakeLocalRef(pEnv->UnwrapRef(obj));
		}

		internal static jint EnsureLocalCapacity(JNIEnv* pEnv, jint capacity)
		{
			// since we can dynamically grow the local ref table, we'll just return success for any number
			return JNI_OK;
		}

		internal static jobject AllocObject(JNIEnv* pEnv, jclass clazz)
		{
			return AllocObjectImpl(pEnv, TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(clazz)));
		}

		private static jobject AllocObjectImpl(JNIEnv* pEnv, TypeWrapper wrapper)
		{
			try
			{
				if(wrapper.IsAbstract)
				{
					SetPendingException(pEnv, new java.lang.InstantiationException(wrapper.Name));
					return IntPtr.Zero;
				}
				wrapper.Finish();
				return pEnv->MakeLocalRef(System.Runtime.Serialization.FormatterServices.GetUninitializedObject(wrapper.TypeAsBaseType));
			}
			catch(RetargetableJavaException x)
			{
				SetPendingException(pEnv, x.ToJava());
				return IntPtr.Zero;
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, x);
				return IntPtr.Zero;
			}
		}

		[StructLayout(LayoutKind.Explicit)]
			internal struct jvalue
		{
			[FieldOffset(0)]
			public jboolean z;
			[FieldOffset(0)]
			public jbyte b;
			[FieldOffset(0)]
			public jchar c;
			[FieldOffset(0)]
			public jshort s;
			[FieldOffset(0)]
			public jint i;
			[FieldOffset(0)]
			public jlong j;
			[FieldOffset(0)]
			public jfloat f;
			[FieldOffset(0)]
			public jdouble d;
			[FieldOffset(0)]
			public jobject l;
		}

		private static object InvokeHelper(JNIEnv* pEnv, jobject objHandle, jmethodID methodID, jvalue* pArgs, bool nonVirtual)
		{
			ManagedJNIEnv env = pEnv->GetManagedJNIEnv();
			object obj = UnwrapRef(env, objHandle);
			MethodWrapper mw = MethodWrapper.FromCookie(methodID);
			mw.Link();
			mw.ResolveMethod();
			TypeWrapper[] argTypes = mw.GetParameters();
			object[] args = new object[argTypes.Length + (mw.HasCallerID ? 1 : 0)];
			for (int i = 0; i < argTypes.Length; i++)
			{
				TypeWrapper type = argTypes[i];
				if (type == PrimitiveTypeWrapper.BOOLEAN)
					args[i] = pArgs[i].z != JNI_FALSE;
				else if (type == PrimitiveTypeWrapper.BYTE)
					args[i] = (byte)pArgs[i].b;
				else if (type == PrimitiveTypeWrapper.CHAR)
					args[i] = (char)pArgs[i].c;
				else if (type == PrimitiveTypeWrapper.SHORT)
					args[i] = pArgs[i].s;
				else if (type == PrimitiveTypeWrapper.INT)
					args[i] = pArgs[i].i;
				else if (type == PrimitiveTypeWrapper.LONG)
					args[i] = pArgs[i].j;
				else if (type == PrimitiveTypeWrapper.FLOAT)
					args[i] = pArgs[i].f;
				else if (type == PrimitiveTypeWrapper.DOUBLE)
					args[i] = pArgs[i].d;
				else
					args[i] = argTypes[i].GhostWrap(UnwrapRef(env, pArgs[i].l));
			}
			if (mw.HasCallerID)
			{
				args[args.Length - 1] = env.callerID;
			}
			try
			{
				if (nonVirtual && mw.RequiresNonVirtualDispatcher)
				{
					return InvokeNonVirtual(env, mw, obj, args);
				}
				if (mw.IsConstructor)
				{
					if (obj == null)
					{
						return mw.CreateInstance(args);
					}
					else
					{
						MethodBase mb = mw.GetMethod();
						if (mb.IsStatic)
						{
							// we're dealing with a constructor on a remapped type, if obj is supplied, it means
							// that we should call the constructor on an already existing instance, but that isn't
							// possible with remapped types
							throw new NotSupportedException(string.Format("Remapped type {0} doesn't support constructor invocation on an existing instance", mw.DeclaringType.Name));
						}
						else if (!mb.DeclaringType.IsInstanceOfType(obj))
						{
							// we're trying to initialize an existing instance of a remapped type
							throw new NotSupportedException("Unable to partially construct object of type " + obj.GetType().FullName + " to type " + mb.DeclaringType.FullName);
						}
					}
				}
				return mw.Invoke(obj, args);
			}
			catch (Exception x)
			{
				SetPendingException(pEnv, ikvm.runtime.Util.mapException(x));
				return null;
			}
		}

		private static object InvokeNonVirtual(ManagedJNIEnv env, MethodWrapper mw, object obj, object[] argarray)
		{
			if (mw.HasCallerID || mw.IsDynamicOnly)
			{
				throw new NotSupportedException();
			}
			if (mw.DeclaringType.IsRemapped && !mw.DeclaringType.TypeAsBaseType.IsInstanceOfType(obj))
			{
				return mw.InvokeNonvirtualRemapped(obj, argarray);
			}
			else
			{
				Delegate del = (Delegate)Activator.CreateInstance(mw.GetDelegateType(),
					new object[] { obj, mw.GetMethod().MethodHandle.GetFunctionPointer() });
				try
				{
					return del.DynamicInvoke(argarray);
				}
				catch (TargetInvocationException x)
				{
					throw ikvm.runtime.Util.mapException(x.InnerException);
				}
			}
		}

		internal static jobject NewObjectA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue *args)
		{
			TypeWrapper wrapper = TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(clazz));
			if(!wrapper.IsAbstract && wrapper.TypeAsBaseType.IsAbstract)
			{
				// static newinstance helper method
				return pEnv->MakeLocalRef(InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false));
			}
			jobject obj = AllocObjectImpl(pEnv, wrapper);
			if(obj != IntPtr.Zero)
			{
				InvokeHelper(pEnv, obj, methodID, args, false);
				if(ExceptionCheck(pEnv) == JNI_TRUE)
				{
					DeleteLocalRef(pEnv, obj);
					obj = IntPtr.Zero;
				}
			}
			return obj;
		}

		internal static jclass GetObjectClass(JNIEnv* pEnv, jobject obj)
		{
			return pEnv->MakeLocalRef(IKVM.NativeCode.ikvm.runtime.Util.getClassFromObject(pEnv->UnwrapRef(obj)));
		}

		internal static jboolean IsInstanceOf(JNIEnv* pEnv, jobject obj, jclass clazz)
		{
			// NOTE if clazz is an interface, this is still the right thing to do
			// (i.e. if the object implements the interface, we return true)
			java.lang.Class objClass = IKVM.NativeCode.ikvm.runtime.Util.getClassFromObject(pEnv->UnwrapRef(obj));
			TypeWrapper w1 = TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(clazz));
			TypeWrapper w2 = TypeWrapper.FromClass(objClass);
			return w2.IsAssignableTo(w1) ? JNI_TRUE : JNI_FALSE;
		}

		private static MethodWrapper GetMethodImpl(TypeWrapper tw, string name, string sig)
		{
			for(;;)
			{
				MethodWrapper mw = tw.GetMethodWrapper(name, sig, true);
				if(mw == null || !mw.IsHideFromReflection)
				{
					return mw;
				}
				tw = mw.DeclaringType.BaseTypeWrapper;
				if(tw == null)
				{
					return null;
				}
			}
		}

		private static void AppendInterfaces(List<TypeWrapper> list, IList<TypeWrapper> add)
		{
			foreach (TypeWrapper iface in add)
			{
				if (!list.Contains(iface))
				{
					list.Add(iface);
				}
			}
		}

		private static List<TypeWrapper> TransitiveInterfaces(TypeWrapper tw)
		{
			List<TypeWrapper> list = new List<TypeWrapper>();
			if (tw.BaseTypeWrapper != null)
			{
				AppendInterfaces(list, TransitiveInterfaces(tw.BaseTypeWrapper));
			}
			foreach (TypeWrapper iface in tw.Interfaces)
			{
				AppendInterfaces(list, TransitiveInterfaces(iface));
			}
			AppendInterfaces(list, tw.Interfaces);
			return list;
		}

		private static MethodWrapper GetInterfaceMethodImpl(TypeWrapper tw, string name, string sig)
		{
			foreach (TypeWrapper iface in TransitiveInterfaces(tw))
			{
				MethodWrapper mw = iface.GetMethodWrapper(name, sig, false);
				if (mw != null && !mw.IsHideFromReflection)
				{
					return mw;
				}
			}
			return null;
		}

		private static jmethodID FindMethodID(JNIEnv* pEnv, jclass clazz, byte* name, byte* sig, bool isstatic)
		{
			try
			{
				TypeWrapper wrapper = TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(clazz));
				wrapper.Finish();
				// if name == NULL, the JDK returns the constructor
				string methodname = (IntPtr)name == IntPtr.Zero ? "<init>" : StringFromUTF8(name);
				string methodsig = StringFromUTF8(sig);
				MethodWrapper mw = null;
				// don't allow dotted names!
				if(methodsig.IndexOf('.') < 0)
				{
					methodsig = methodsig.Replace('/', '.');
					if(methodname == "<init>" || methodname == "<clinit>")
					{
						mw = wrapper.GetMethodWrapper(methodname, methodsig, false);
					}
					else
					{
						mw = GetMethodImpl(wrapper, methodname, methodsig);
						if(mw == null)
						{
							mw = GetInterfaceMethodImpl(wrapper, methodname, methodsig);
						}
					}
				}
				if(mw != null && mw.IsStatic == isstatic)
				{
					mw.Link();
					return mw.Cookie;
				}
				SetPendingException(pEnv, new java.lang.NoSuchMethodError(string.Format("{0}{1}", methodname, methodsig)));
			}
			catch(RetargetableJavaException x)
			{
				SetPendingException(pEnv, x.ToJava());
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, x);
			}
			return IntPtr.Zero;
		}

		internal static jmethodID GetMethodID(JNIEnv* pEnv, jclass clazz, byte* name, byte* sig)
		{
			return FindMethodID(pEnv, clazz, name, sig, false);
		}

		internal static jobject CallObjectMethodA(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args)
		{
			return pEnv->MakeLocalRef(InvokeHelper(pEnv, obj, methodID, args, false));
		}

		internal static jboolean CallBooleanMethodA(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args)
		{
			object o = InvokeHelper(pEnv, obj, methodID, args, false);
			if(o != null)
			{
				return ((bool)o) ? JNI_TRUE : JNI_FALSE;
			}
			return JNI_FALSE;
		}

		internal static jbyte CallByteMethodA(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args)
		{
			object o = InvokeHelper(pEnv, obj, methodID, args, false);
			if(o != null)
			{
				return (jbyte)(byte)o;
			}
			return 0;
		}

		internal static jchar CallCharMethodA(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args)
		{
			object o = InvokeHelper(pEnv, obj, methodID, args, false);
			if(o != null)
			{
				return (jchar)(char)o;
			}
			return 0;
		}

		internal static jshort CallShortMethodA(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args)
		{
			object o = InvokeHelper(pEnv, obj, methodID, args, false);
			if(o != null)
			{
				return (jshort)(short)o;
			}
			return 0;
		}

		internal static jint CallIntMethodA(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args)
		{
			object o = InvokeHelper(pEnv, obj, methodID, args, false);
			if(o != null)
			{
				return (jint)(int)o;
			}
			return 0;
		}

		internal static jlong CallLongMethodA(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args)
		{
			object o = InvokeHelper(pEnv, obj, methodID, args, false);
			if(o != null)
			{
				return (jlong)(long)o;
			}
			return 0;
		}

		internal static jfloat CallFloatMethodA(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args)
		{
			object o = InvokeHelper(pEnv, obj, methodID, args, false);
			if(o != null)
			{
				return (jfloat)(float)o;
			}
			return 0;
		}

		internal static jdouble CallDoubleMethodA(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args)
		{
			object o = InvokeHelper(pEnv, obj, methodID, args, false);
			if(o != null)
			{
				return (jdouble)(double)o;
			}
			return 0;
		}

		internal static void CallVoidMethodA(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue*  args)
		{
			InvokeHelper(pEnv, obj, methodID, args, false);
		}

		internal static jobject CallNonvirtualObjectMethodA(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue*  args)
		{
			return pEnv->MakeLocalRef(InvokeHelper(pEnv, obj, methodID, args, true));
		}

		internal static jboolean CallNonvirtualBooleanMethodA(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue*  args)
		{
			object o = InvokeHelper(pEnv, obj, methodID, args, true);
			if(o != null)
			{
				return ((bool)o) ? JNI_TRUE : JNI_FALSE;
			}
			return JNI_FALSE;
		}

		internal static jbyte CallNonvirtualByteMethodA(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args)
		{
			object o = InvokeHelper(pEnv, obj, methodID, args, true);
			if(o != null)
			{
				return (jbyte)(byte)o;
			}
			return 0;
		}

		internal static jchar CallNonvirtualCharMethodA(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args)
		{
			object o = InvokeHelper(pEnv, obj, methodID, args, true);
			if(o != null)
			{
				return (jchar)(char)o;
			}
			return 0;
		}

		internal static jshort CallNonvirtualShortMethodA(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args)
		{
			object o = InvokeHelper(pEnv, obj, methodID, args, true);
			if(o != null)
			{
				return (jshort)(short)o;
			}
			return 0;
		}

		internal static jint CallNonvirtualIntMethodA(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args)
		{
			object o = InvokeHelper(pEnv, obj, methodID, args, true);
			if(o != null)
			{
				return (jint)(int)o;
			}
			return 0;
		}

		internal static jlong CallNonvirtualLongMethodA(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args)
		{
			object o = InvokeHelper(pEnv, obj, methodID, args, true);
			if(o != null)
			{
				return (jlong)(long)o;
			}
			return 0;
		}

		internal static jfloat CallNonvirtualFloatMethodA(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args)
		{
			object o = InvokeHelper(pEnv, obj, methodID, args, true);
			if(o != null)
			{
				return (jfloat)(float)o;
			}
			return 0;
		}

		internal static jdouble CallNonvirtualDoubleMethodA(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args)
		{
			object o = InvokeHelper(pEnv, obj, methodID, args, true);
			if(o != null)
			{
				return (jdouble)(double)o;
			}
			return 0;
		}

		internal static void CallNonvirtualVoidMethodA(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args)
		{
			InvokeHelper(pEnv, obj, methodID, args, true);
		}

		private static FieldWrapper GetFieldImpl(TypeWrapper tw, string name, string sig)
		{
			for(;;)
			{
				FieldWrapper fw = tw.GetFieldWrapper(name, sig);
				if(fw == null || !fw.IsHideFromReflection)
				{
					return fw;
				}
				tw = fw.DeclaringType.BaseTypeWrapper;
				if(tw == null)
				{
					return null;
				}
			}
		}

		private static jfieldID FindFieldID(JNIEnv* pEnv, jclass clazz, byte* name, byte* sig, bool isstatic)
		{
			try
			{
				TypeWrapper wrapper = TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(clazz));
				wrapper.Finish();
				string fieldsig = StringFromUTF8(sig);
				// don't allow dotted names!
				if(fieldsig.IndexOf('.') < 0)
				{
					FieldWrapper fw = GetFieldImpl(wrapper, StringFromUTF8(name), fieldsig.Replace('/', '.'));
					if(fw != null)
					{
						if(fw.IsStatic == isstatic)
						{
							return fw.Cookie;
						}
					}
				}
				SetPendingException(pEnv, new java.lang.NoSuchFieldError((isstatic ? "Static" : "Instance") + " field '" + StringFromUTF8(name) + "' with signature '" + fieldsig + "' not found in class '" + wrapper.Name + "'"));
			}
			catch(RetargetableJavaException x)
			{
				SetPendingException(pEnv, x.ToJava());
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, x);
			}
			return IntPtr.Zero;
		}

		internal static jfieldID GetFieldID(JNIEnv* pEnv, jclass clazz, byte* name, byte* sig)
		{
			return FindFieldID(pEnv, clazz, name, sig, false);
		}

		private static sun.reflect.FieldAccessor GetFieldAccessor(jfieldID cookie)
		{
			return (sun.reflect.FieldAccessor)FieldWrapper.FromCookie(cookie).GetFieldAccessorJNI();
		}

		internal static jobject GetObjectField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
		{
			return pEnv->MakeLocalRef(GetFieldAccessor(fieldID).get(pEnv->UnwrapRef(obj)));
		}

		internal static jboolean GetBooleanField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
		{
			return GetFieldAccessor(fieldID).getBoolean(pEnv->UnwrapRef(obj)) ? JNI_TRUE : JNI_FALSE;
		}

		internal static jbyte GetByteField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
		{
			return (jbyte)GetFieldAccessor(fieldID).getByte(pEnv->UnwrapRef(obj));
		}

		internal static jchar GetCharField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
		{
			return (jchar)GetFieldAccessor(fieldID).getChar(pEnv->UnwrapRef(obj));
		}

		internal static jshort GetShortField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
		{
			return (jshort)GetFieldAccessor(fieldID).getShort(pEnv->UnwrapRef(obj));
		}

		internal static jint GetIntField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
		{
			return (jint)GetFieldAccessor(fieldID).getInt(pEnv->UnwrapRef(obj));
		}

		internal static jlong GetLongField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
		{
			return (jlong)GetFieldAccessor(fieldID).getLong(pEnv->UnwrapRef(obj));
		}

		internal static jfloat GetFloatField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
		{
			return (jfloat)GetFieldAccessor(fieldID).getFloat(pEnv->UnwrapRef(obj));
		}

		internal static jdouble GetDoubleField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
		{
			return (jdouble)GetFieldAccessor(fieldID).getDouble(pEnv->UnwrapRef(obj));
		}

		internal static void SetObjectField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jobject val)
		{
			GetFieldAccessor(fieldID).set(pEnv->UnwrapRef(obj), pEnv->UnwrapRef(val));
		}

		internal static void SetBooleanField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jboolean val)
		{
			GetFieldAccessor(fieldID).setBoolean(pEnv->UnwrapRef(obj), val != JNI_FALSE);
		}

		internal static void SetByteField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jbyte val)
		{
			GetFieldAccessor(fieldID).setByte(pEnv->UnwrapRef(obj), (byte)val);
		}

		internal static void SetCharField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jchar val)
		{
			GetFieldAccessor(fieldID).setChar(pEnv->UnwrapRef(obj), (char)val);
		}

		internal static void SetShortField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jshort val)
		{
			GetFieldAccessor(fieldID).setShort(pEnv->UnwrapRef(obj), (short)val);
		}

		internal static void SetIntField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jint val)
		{
			GetFieldAccessor(fieldID).setInt(pEnv->UnwrapRef(obj), (int)val);
		}

		internal static void SetLongField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jlong val)
		{
			GetFieldAccessor(fieldID).setLong(pEnv->UnwrapRef(obj), (long)val);
		}

		internal static void SetFloatField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jfloat val)
		{
			GetFieldAccessor(fieldID).setFloat(pEnv->UnwrapRef(obj), (float)val);
		}

		internal static void SetDoubleField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jdouble val)
		{
			GetFieldAccessor(fieldID).setDouble(pEnv->UnwrapRef(obj), (double)val);
		}

		internal static jmethodID GetStaticMethodID(JNIEnv* pEnv, jclass clazz, byte* name, byte* sig)
		{
			return FindMethodID(pEnv, clazz, name, sig, true);
		}

		internal static jobject CallStaticObjectMethodA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue *args)
		{
			return pEnv->MakeLocalRef(InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false));
		}

		internal static jboolean CallStaticBooleanMethodA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue *args)
		{
			object o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
			if(o != null)
			{
				return ((bool)o) ? JNI_TRUE : JNI_FALSE;
			}
			return JNI_FALSE;
		}

		internal static jbyte CallStaticByteMethodA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue *args)
		{
			object o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
			if(o != null)
			{
				return (jbyte)(byte)o;
			}
			return 0;
		}

		internal static jchar CallStaticCharMethodA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue *args)
		{
			object o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
			if(o != null)
			{
				return (jchar)(char)o;
			}
			return 0;
		}

		internal static jshort CallStaticShortMethodA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue *args)
		{
			object o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
			if(o != null)
			{
				return (jshort)(short)o;
			}
			return 0;
		}

		internal static jint CallStaticIntMethodA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue *args)
		{
			object o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
			if(o != null)
			{
				return (jint)(int)o;
			}
			return 0;
		}

		internal static jlong CallStaticLongMethodA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue *args)
		{
			object o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
			if(o != null)
			{
				return (jlong)(long)o;
			}
			return 0;
		}

		internal static jfloat CallStaticFloatMethodA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue *args)
		{
			object o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
			if(o != null)
			{
				return (jfloat)(float)o;
			}
			return 0;
		}

		internal static jdouble CallStaticDoubleMethodA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue *args)
		{
			object o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
			if(o != null)
			{
				return (jdouble)(double)o;
			}
			return 0;
		}

		internal static void CallStaticVoidMethodA(JNIEnv* pEnv, jclass cls, jmethodID methodID, jvalue * args)
		{
			InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
		}

		internal static jfieldID GetStaticFieldID(JNIEnv* pEnv, jclass clazz, byte* name, byte* sig)
		{
			return FindFieldID(pEnv, clazz, name, sig, true);
		}

		internal static jobject GetStaticObjectField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
		{
			return pEnv->MakeLocalRef(GetFieldAccessor(fieldID).get(null));
		}

		internal static jboolean GetStaticBooleanField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
		{
			return GetFieldAccessor(fieldID).getBoolean(null) ? JNI_TRUE : JNI_FALSE;
		}

		internal static jbyte GetStaticByteField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
		{
			return (jbyte)GetFieldAccessor(fieldID).getByte(null);
		}

		internal static jchar GetStaticCharField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
		{
			return (jchar)GetFieldAccessor(fieldID).getChar(null);
		}

		internal static jshort GetStaticShortField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
		{
			return (jshort)GetFieldAccessor(fieldID).getShort(null);
		}

		internal static jint GetStaticIntField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
		{
			return (jint)GetFieldAccessor(fieldID).getInt(null);
		}

		internal static jlong GetStaticLongField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
		{
			return (jlong)GetFieldAccessor(fieldID).getLong(null);
		}

		internal static jfloat GetStaticFloatField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
		{
			return (jfloat)GetFieldAccessor(fieldID).getFloat(null);
		}

		internal static jdouble GetStaticDoubleField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
		{
			return (jdouble)GetFieldAccessor(fieldID).getDouble(null);
		}

		internal static void SetStaticObjectField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jobject val)
		{
			GetFieldAccessor(fieldID).set(null, pEnv->UnwrapRef(val));
		}

		internal static void SetStaticBooleanField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jboolean val)
		{
			GetFieldAccessor(fieldID).setBoolean(null, val != JNI_FALSE);
		}

		internal static void SetStaticByteField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jbyte val)
		{
			GetFieldAccessor(fieldID).setByte(null, (byte)val);
		}

		internal static void SetStaticCharField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jchar val)
		{
			GetFieldAccessor(fieldID).setChar(null, (char)val);
		}

		internal static void SetStaticShortField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jshort val)
		{
			GetFieldAccessor(fieldID).setShort(null, (short)val);
		}

		internal static void SetStaticIntField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jint val)
		{
			GetFieldAccessor(fieldID).setInt(null, (int)val);
		}

		internal static void SetStaticLongField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jlong val)
		{
			GetFieldAccessor(fieldID).setLong(null, (long)val);
		}

		internal static void SetStaticFloatField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jfloat val)
		{
			GetFieldAccessor(fieldID).setFloat(null, (float)val);
		}

		internal static void SetStaticDoubleField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jdouble val)
		{
			GetFieldAccessor(fieldID).setDouble(null, (double)val);
		}

		internal static jstring NewString(JNIEnv* pEnv, jchar* unicode, int len)
		{
			return pEnv->MakeLocalRef(new String((char*)unicode, 0, len));
		}

		internal static jint GetStringLength(JNIEnv* pEnv, jstring str)
		{
			return ((string)pEnv->UnwrapRef(str)).Length;
		}

		internal static jchar* GetStringChars(JNIEnv* pEnv, jstring str, jboolean* isCopy)
		{
			string s = (string)pEnv->UnwrapRef(str);
			if(isCopy != null)
			{
				*isCopy = JNI_TRUE;
			}
			return (jchar*)(void*)Marshal.StringToHGlobalUni(s);
		}

		internal static void ReleaseStringChars(JNIEnv* pEnv, jstring str, jchar* chars)
		{
			Marshal.FreeHGlobal((IntPtr)(void*)chars);
		}

		internal static jobject NewStringUTF(JNIEnv* pEnv, byte* psz)
		{
			if (psz == null)
			{
				// The JNI spec does not explicitly allow a null pointer, but the JDK accepts it
				return IntPtr.Zero;
			}
			return pEnv->MakeLocalRef(StringFromUTF8(psz));
		}

		internal static jint GetStringUTFLength(JNIEnv* pEnv, jstring str)
		{
			return StringUTF8Length((string)pEnv->UnwrapRef(str));
		}

		internal static byte* GetStringUTFChars(JNIEnv* pEnv, jstring str, jboolean* isCopy)
		{
			string s = (string)pEnv->UnwrapRef(str);
			byte* buf = (byte*)JniMem.Alloc(StringUTF8Length(s) + 1);
			int j = 0;
			for(int i = 0; i < s.Length; i++)
			{
				char ch = s[i];
				if((ch != 0) && (ch <= 0x7F))
				{
					buf[j++] = (byte)ch;
				}
				else if(ch <= 0x7FF)
				{
					buf[j++] = (byte)((ch >> 6) | 0xC0);
					buf[j++] = (byte)((ch & 0x3F) | 0x80);
				}
				else
				{
					buf[j++] = (byte)((ch >> 12) | 0xE0);
					buf[j++] = (byte)(((ch >> 6) & 0x3F) | 0x80);
					buf[j++] = (byte)((ch & 0x3F) | 0x80);
				}
			}
			buf[j] = 0;
			if(isCopy != null)
			{
				*isCopy = JNI_TRUE;
			}
			return buf;
		}

		internal static void ReleaseStringUTFChars(JNIEnv* pEnv, jstring str, byte* chars)
		{
			JniMem.Free((IntPtr)(void*)chars);
		}

		internal static jsize GetArrayLength(JNIEnv* pEnv, jarray array)
		{
			return ((Array)pEnv->UnwrapRef(array)).Length;
		}

		internal static jobject NewObjectArray(JNIEnv* pEnv, jsize len, jclass clazz, jobject init)
		{
			try
			{
				// we want to support (non-primitive) value types so we can't cast to object[]
				Array array = Array.CreateInstance(TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(clazz)).TypeAsArrayType, len);
				object o = pEnv->UnwrapRef(init);
				if(o != null)
				{
					for(int i = 0; i < array.Length; i++)
					{
						array.SetValue(o, i);
					}
				}
				return pEnv->MakeLocalRef(array);
			}
			catch(ArgumentOutOfRangeException)
			{
				SetPendingException(pEnv, new java.lang.NegativeArraySizeException());
				return IntPtr.Zero;
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, x);
				return IntPtr.Zero;
			}
		}

		internal static jobject GetObjectArrayElement(JNIEnv* pEnv, jarray array, jsize index)
		{
			try
			{
				// we want to support (non-primitive) value types so we can't cast to object[]
				return pEnv->MakeLocalRef(((Array)pEnv->UnwrapRef(array)).GetValue(index));
			}
			catch(IndexOutOfRangeException)
			{
				SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
				return IntPtr.Zero;
			}
		}

		internal static void SetObjectArrayElement(JNIEnv* pEnv, jarray array, jsize index, jobject val)
		{
			try
			{
				// we want to support (non-primitive) value types so we can't cast to object[]
				((Array)pEnv->UnwrapRef(array)).SetValue(pEnv->UnwrapRef(val), index);
			}
			catch(IndexOutOfRangeException)
			{
				SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
			}
		}

		internal static jbooleanArray NewBooleanArray(JNIEnv* pEnv, jsize len)
		{
			try
			{
				return pEnv->MakeLocalRef(new bool[len]);
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, x);
				return IntPtr.Zero;
			}
		}

		internal static jbyteArray NewByteArray(JNIEnv* pEnv, jsize len)
		{
			try
			{
				return pEnv->MakeLocalRef(new byte[len]);
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, x);
				return IntPtr.Zero;
			}
		}

		internal static jcharArray NewCharArray(JNIEnv* pEnv, jsize len)
		{
			try
			{
				return pEnv->MakeLocalRef(new char[len]);
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, x);
				return IntPtr.Zero;
			}
		}

		internal static jshortArray NewShortArray(JNIEnv* pEnv, jsize len)
		{
			try
			{
				return pEnv->MakeLocalRef(new short[len]);
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, x);
				return IntPtr.Zero;
			}
		}

		internal static jintArray NewIntArray(JNIEnv* pEnv, jsize len)
		{
			try
			{
				return pEnv->MakeLocalRef(new int[len]);
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, x);
				return IntPtr.Zero;
			}
		}

		internal static jlongArray NewLongArray(JNIEnv* pEnv, jsize len)
		{
			try
			{
				return pEnv->MakeLocalRef(new long[len]);
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, x);
				return IntPtr.Zero;
			}
		}

		internal static jfloatArray NewFloatArray(JNIEnv* pEnv, jsize len)
		{
			try
			{
				return pEnv->MakeLocalRef(new float[len]);
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, x);
				return IntPtr.Zero;
			}
		}

		internal static jdoubleArray NewDoubleArray(JNIEnv* pEnv, jsize len)
		{
			try
			{
				return pEnv->MakeLocalRef(new double[len]);
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, x);
				return IntPtr.Zero;
			}
		}

		internal static jboolean* GetBooleanArrayElements(JNIEnv* pEnv, jbooleanArray array, jboolean* isCopy)
		{
			bool[] b = (bool[])pEnv->UnwrapRef(array);
			jboolean* p = (jboolean*)(void*)JniMem.Alloc(b.Length * 1);
			for(int i = 0; i < b.Length; i++)
			{
				p[i] = b[i] ? JNI_TRUE : JNI_FALSE;
			}
			if(isCopy != null)
			{
				*isCopy = JNI_TRUE;
			}
			return p;
		}

		internal static jbyte* GetByteArrayElements(JNIEnv* pEnv, jbyteArray array, jboolean* isCopy)
		{
			byte[] b = (byte[])pEnv->UnwrapRef(array);
			jbyte* p = (jbyte*)(void*)JniMem.Alloc(b.Length * 1);
			for(int i = 0; i < b.Length; i++)
			{
				p[i] = (jbyte)b[i];
			}
			if(isCopy != null)
			{
				*isCopy = JNI_TRUE;
			}
			return p;
		}

		internal static jchar* GetCharArrayElements(JNIEnv* pEnv, jcharArray array, jboolean* isCopy)
		{
			char[] b = (char[])pEnv->UnwrapRef(array);
			IntPtr buf = JniMem.Alloc(b.Length * 2);
			Marshal.Copy(b, 0, buf, b.Length);
			if(isCopy != null)
			{
				*isCopy = JNI_TRUE;
			}
			return (jchar*)(void*)buf;
		}

		internal static jshort* GetShortArrayElements(JNIEnv* pEnv, jshortArray array, jboolean* isCopy)
		{
			short[] b = (short[])pEnv->UnwrapRef(array);
			IntPtr buf = JniMem.Alloc(b.Length * 2);
			Marshal.Copy(b, 0, buf, b.Length);
			if(isCopy != null)
			{
				*isCopy = JNI_TRUE;
			}
			return (jshort*)(void*)buf;
		}

		internal static jint* GetIntArrayElements(JNIEnv* pEnv, jintArray array, jboolean* isCopy)
		{
			int[] b = (int[])pEnv->UnwrapRef(array);
			IntPtr buf = JniMem.Alloc(b.Length * 4);
			Marshal.Copy(b, 0, buf, b.Length);
			if(isCopy != null)
			{
				*isCopy = JNI_TRUE;
			}
			return (jint*)(void*)buf;
		}

		internal static jlong* GetLongArrayElements(JNIEnv* pEnv, jlongArray array, jboolean* isCopy)
		{
			long[] b = (long[])pEnv->UnwrapRef(array);
			IntPtr buf = JniMem.Alloc(b.Length * 8);
			Marshal.Copy(b, 0, buf, b.Length);
			if(isCopy != null)
			{
				*isCopy = JNI_TRUE;
			}
			return (jlong*)(void*)buf;
		}

		internal static jfloat* GetFloatArrayElements(JNIEnv* pEnv, jfloatArray array, jboolean* isCopy)
		{
			float[] b = (float[])pEnv->UnwrapRef(array);
			IntPtr buf = JniMem.Alloc(b.Length * 4);
			Marshal.Copy(b, 0, buf, b.Length);
			if(isCopy != null)
			{
				*isCopy = JNI_TRUE;
			}
			return (jfloat*)(void*)buf;
		}

		internal static jdouble* GetDoubleArrayElements(JNIEnv* pEnv, jdoubleArray array, jboolean* isCopy)
		{
			double[] b = (double[])pEnv->UnwrapRef(array);
			IntPtr buf = JniMem.Alloc(b.Length * 8);
			Marshal.Copy(b, 0, buf, b.Length);
			if(isCopy != null)
			{
				*isCopy = JNI_TRUE;
			}
			return (jdouble*)(void*)buf;
		}

		internal static void ReleaseBooleanArrayElements(JNIEnv* pEnv, jbooleanArray array, jboolean* elems, jint mode)
		{
			if(mode == 0 || mode == JNI_COMMIT)
			{
				bool[] b = (bool[])pEnv->UnwrapRef(array);
				for(int i = 0; i < b.Length; i++)
				{
					b[i] = elems[i] != JNI_FALSE;
				}
			}
			if(mode == 0 || mode == JNI_ABORT)
			{
				JniMem.Free((IntPtr)(void*)elems);
			}
		}

		internal static void ReleaseByteArrayElements(JNIEnv* pEnv, jbyteArray array, jbyte* elems, jint mode)
		{
			if(mode == 0 || mode == JNI_COMMIT)
			{
				byte[] b = (byte[])pEnv->UnwrapRef(array);
				for(int i = 0; i < b.Length; i++)
				{
					b[i] = (byte)elems[i];
				}
			}
			if(mode == 0 || mode == JNI_ABORT)
			{
				JniMem.Free((IntPtr)(void*)elems);
			}
		}

		internal static void ReleaseCharArrayElements(JNIEnv* pEnv, jcharArray array, jchar* elems, jint mode)
		{
			if(mode == 0 || mode == JNI_COMMIT)
			{
				char[] b = (char[])pEnv->UnwrapRef(array);
				Marshal.Copy((IntPtr)(void*)elems, b, 0, b.Length);
			}
			if(mode == 0 || mode == JNI_ABORT)
			{
				JniMem.Free((IntPtr)(void*)elems);
			}
		}

		internal static void ReleaseShortArrayElements(JNIEnv* pEnv, jshortArray array, jshort* elems, jint mode)
		{
			if(mode == 0 || mode == JNI_COMMIT)
			{
				short[] b = (short[])pEnv->UnwrapRef(array);
				Marshal.Copy((IntPtr)(void*)elems, b, 0, b.Length);
			}
			if(mode == 0 || mode == JNI_ABORT)
			{
				JniMem.Free((IntPtr)(void*)elems);
			}
		}

		internal static void ReleaseIntArrayElements(JNIEnv* pEnv, jintArray array, jint* elems, jint mode)
		{
			if(mode == 0 || mode == JNI_COMMIT)
			{
				int[] b = (int[])pEnv->UnwrapRef(array);
				Marshal.Copy((IntPtr)(void*)elems, b, 0, b.Length);
			}
			if(mode == 0 || mode == JNI_ABORT)
			{
				JniMem.Free((IntPtr)(void*)elems);
			}
		}

		internal static void ReleaseLongArrayElements(JNIEnv* pEnv, jlongArray array, jlong* elems, jint mode)
		{
			if(mode == 0 || mode == JNI_COMMIT)
			{
				long[] b = (long[])pEnv->UnwrapRef(array);
				Marshal.Copy((IntPtr)(void*)elems, b, 0, b.Length);
			}
			if(mode == 0 || mode == JNI_ABORT)
			{
				JniMem.Free((IntPtr)(void*)elems);
			}
		}

		internal static void ReleaseFloatArrayElements(JNIEnv* pEnv, jfloatArray array, jfloat* elems, jint mode)
		{
			if(mode == 0 || mode == JNI_COMMIT)
			{
				float[] b = (float[])pEnv->UnwrapRef(array);
				Marshal.Copy((IntPtr)(void*)elems, b, 0, b.Length);
			}
			if(mode == 0 || mode == JNI_ABORT)
			{
				JniMem.Free((IntPtr)(void*)elems);
			}
		}

		internal static void ReleaseDoubleArrayElements(JNIEnv* pEnv, jdoubleArray array, jdouble* elems, jint mode)
		{
			if(mode == 0 || mode == JNI_COMMIT)
			{
				double[] b = (double[])pEnv->UnwrapRef(array);
				Marshal.Copy((IntPtr)(void*)elems, b, 0, b.Length);
			}
			if(mode == 0 || mode == JNI_ABORT)
			{
				JniMem.Free((IntPtr)(void*)elems);
			}
		}

		internal static void GetBooleanArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
		{
			try
			{
				bool[] b = (bool[])pEnv->UnwrapRef(array);
				sbyte* p = (sbyte*)(void*)buf;
				for(int i = 0; i < len; i++)
				{
					*p++ = b[start + i] ? JNI_TRUE : JNI_FALSE;
				}
			}
			catch(IndexOutOfRangeException)
			{
				SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
			}
		}

		internal static void GetByteArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
		{
			try
			{
				byte[] b = (byte[])pEnv->UnwrapRef(array);
				byte* p = (byte*)(void*)buf;
				for(int i = 0; i < len; i++)
				{
					*p++ = b[start + i];
				}
			}
			catch(IndexOutOfRangeException)
			{
				SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
			}
		}

		internal static void GetCharArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
		{
			try
			{
				char[] b = (char[])pEnv->UnwrapRef(array);
				Marshal.Copy(b, start, buf, len);
			}
			catch(ArgumentOutOfRangeException)
			{
				SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
			}
		}

		internal static void GetShortArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
		{
			try
			{
				short[] b = (short[])pEnv->UnwrapRef(array);
				Marshal.Copy(b, start, buf, len);
			}
			catch(ArgumentOutOfRangeException)
			{
				SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
			}
		}

		internal static void GetIntArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
		{
			try
			{
				int[] b = (int[])pEnv->UnwrapRef(array);
				Marshal.Copy(b, start, buf, len);
			}
			catch(ArgumentOutOfRangeException)
			{
				SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
			}
		}

		internal static void GetLongArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
		{
			try
			{
				long[] b = (long[])pEnv->UnwrapRef(array);
				Marshal.Copy(b, start, buf, len);
			}
			catch(ArgumentOutOfRangeException)
			{
				SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
			}
		}

		internal static void GetFloatArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
		{
			try
			{
				float[] b = (float[])pEnv->UnwrapRef(array);
				Marshal.Copy(b, start, buf, len);
			}
			catch(ArgumentOutOfRangeException)
			{
				SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
			}
		}

		internal static void GetDoubleArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
		{
			try
			{
				double[] b = (double[])pEnv->UnwrapRef(array);
				Marshal.Copy(b, start, buf, len);
			}
			catch(ArgumentOutOfRangeException)
			{
				SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
			}
		}

		internal static void SetBooleanArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
		{
			try
			{
				bool[] b = (bool[])pEnv->UnwrapRef(array);
				sbyte* p = (sbyte*)(void*)buf;
				for(int i = 0; i < len; i++)
				{
					b[start + i] = *p++ != JNI_FALSE;
				}
			}
			catch(IndexOutOfRangeException)
			{
				SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
			}
		}

		internal static void SetByteArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
		{
			try
			{
				byte[] b = (byte[])pEnv->UnwrapRef(array);
				byte* p = (byte*)(void*)buf;
				for(int i = 0; i < len; i++)
				{
					b[start + i] = *p++;
				}
			}
			catch(IndexOutOfRangeException)
			{
				SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
			}
		}

		internal static void SetCharArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
		{
			try
			{
				char[] b = (char[])pEnv->UnwrapRef(array);
				Marshal.Copy(buf, b, start, len);
			}
			catch(ArgumentOutOfRangeException)
			{
				SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
			}
		}

		internal static void SetShortArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
		{
			try
			{
				short[] b = (short[])pEnv->UnwrapRef(array);
				Marshal.Copy(buf, b, start, len);
			}
			catch(ArgumentOutOfRangeException)
			{
				SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
			}
		}

		internal static void SetIntArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
		{
			try
			{
				int[] b = (int[])pEnv->UnwrapRef(array);
				Marshal.Copy(buf, b, start, len);
			}
			catch(ArgumentOutOfRangeException)
			{
				SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
			}
		}

		internal static void SetLongArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
		{
			try
			{
				long[] b = (long[])pEnv->UnwrapRef(array);
				Marshal.Copy(buf, b, start, len);
			}
			catch(ArgumentOutOfRangeException)
			{
				SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
			}
		}

		internal static void SetFloatArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
		{
			try
			{
				float[] b = (float[])pEnv->UnwrapRef(array);
				Marshal.Copy(buf, b, start, len);
			}
			catch(ArgumentOutOfRangeException)
			{
				SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
			}
		}

		internal static void SetDoubleArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
		{
			try
			{
				double[] b = (double[])pEnv->UnwrapRef(array);
				Marshal.Copy(buf, b, start, len);
			}
			catch(ArgumentOutOfRangeException)
			{
				SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		unsafe internal struct JNINativeMethod
		{
			public byte* name;
			public byte* signature;
			public void* fnPtr;
		}

		internal static int RegisterNatives(JNIEnv* pEnv, IntPtr clazz, JNINativeMethod* methods, int nMethods)
		{
			try
			{
				TypeWrapper wrapper = TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(clazz));
				wrapper.Finish();
				for(int i = 0; i < nMethods; i++)
				{
					string methodName = StringFromUTF8(methods[i].name);
					string methodSig = StringFromUTF8(methods[i].signature);
					Tracer.Info(Tracer.Jni, "Registering native method: {0}.{1}{2}, fnPtr = 0x{3:X}", wrapper.Name, methodName, methodSig, ((IntPtr)methods[i].fnPtr).ToInt64());
					FieldInfo fi = null;
					// don't allow dotted names!
					if(methodSig.IndexOf('.') < 0)
					{
						// TODO this won't work when we're putting the JNI methods in jniproxy.dll
						fi = wrapper.TypeAsTBD.GetField(JNI.METHOD_PTR_FIELD_PREFIX + methodName + methodSig, BindingFlags.Static | BindingFlags.NonPublic);
					}
					if(fi == null)
					{
						Tracer.Error(Tracer.Jni, "Failed to register native method: {0}.{1}{2}", wrapper.Name, methodName, methodSig);
						SetPendingException(pEnv, new java.lang.NoSuchMethodError(methodName));
						return JNI_ERR;
					}
					fi.SetValue(null, (IntPtr)methods[i].fnPtr);
				}
				return JNI_OK;
			}
			catch(RetargetableJavaException x)
			{
				SetPendingException(pEnv, x.ToJava());
				return JNI_ERR;
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, x);
				return JNI_ERR;
			}
		}

		internal static int UnregisterNatives(JNIEnv* pEnv, IntPtr clazz)
		{
			try
			{
				TypeWrapper wrapper = TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(clazz));
				wrapper.Finish();
				// TODO this won't work when we're putting the JNI methods in jniproxy.dll
				foreach(FieldInfo fi in wrapper.TypeAsTBD.GetFields(BindingFlags.Static | BindingFlags.NonPublic))
				{
					string name = fi.Name;
					if(name.StartsWith(JNI.METHOD_PTR_FIELD_PREFIX))
					{
						Tracer.Info(Tracer.Jni, "Unregistering native method: {0}.{1}", wrapper.Name, name.Substring(JNI.METHOD_PTR_FIELD_PREFIX.Length));
						fi.SetValue(null, IntPtr.Zero);
					}
				}
				return JNI_OK;
			}
			catch(RetargetableJavaException x)
			{
				SetPendingException(pEnv, x.ToJava());
				return JNI_ERR;
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, x);
				return JNI_ERR;
			}
		}

		internal static int MonitorEnter(JNIEnv* pEnv, IntPtr obj)
		{
			try
			{
				// on .NET 4.0 Monitor.Enter has been marked obsolete,
				// but in this case the alternative adds no value
#pragma warning disable 618
				System.Threading.Monitor.Enter(pEnv->UnwrapRef(obj));
#pragma warning restore 618
				return JNI_OK;
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, x);
				return JNI_ERR;
			}
		}

		internal static int MonitorExit(JNIEnv* pEnv, IntPtr obj)
		{
			try
			{
				System.Threading.Monitor.Exit(pEnv->UnwrapRef(obj));
				return JNI_OK;
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, x);
				return JNI_ERR;
			}
		}

		internal static int GetJavaVM(JNIEnv* pEnv, JavaVM **ppJavaVM)
		{
			*ppJavaVM = JavaVM.pJavaVM;
			return JNI_OK;
		}

		internal static void GetStringRegion(JNIEnv* pEnv, IntPtr str, int start, int len, IntPtr buf)
		{
			string s = (string)pEnv->UnwrapRef(str);
			if(s != null)
			{
				if(start < 0 || start > s.Length || s.Length - start < len)
				{
					SetPendingException(pEnv, new java.lang.StringIndexOutOfBoundsException());
					return;
				}
				else
				{
					char* p = (char*)(void*)buf;
					// TODO isn't there a managed memcpy?
					for(int i = 0; i < len; i++)
					{
						*p++ = s[start + i];
					}
					return;
				}
			}
			else
			{
				SetPendingException(pEnv, new java.lang.NullPointerException());
			}
		}

		internal static void GetStringUTFRegion(JNIEnv* pEnv, IntPtr str, int start, int len, IntPtr buf)
		{
			string s = (string)pEnv->UnwrapRef(str);
			if(s != null)
			{
				if(start < 0 || start > s.Length || s.Length - start < len)
				{
					SetPendingException(pEnv, new java.lang.StringIndexOutOfBoundsException());
					return;
				}
				else
				{
					byte* p = (byte*)(void*)buf;
					for(int i = 0; i < len; i++)
					{
						char ch = s[start + i];
						if((ch != 0) && (ch <= 0x7F))
						{
							*p++ = (byte)ch;
						}
						else if(ch <= 0x7FF)
						{
							*p++ = (byte)((ch >> 6) | 0xC0);
							*p++ = (byte)((ch & 0x3F) | 0x80);
						}
						else
						{
							*p++ = (byte)((ch >> 12) | 0xE0);
							*p++ = (byte)(((ch >> 6) & 0x3F) | 0x80);
							*p++ = (byte)((ch & 0x3F) | 0x80);
						}
					}
					return;
				}
			}
			else
			{
				SetPendingException(pEnv, new java.lang.NullPointerException());
			}
		}

		private void* PinObject(object obj)
		{
			if(pinHandleInUseCount == pinHandleMaxCount)
			{
				int newCount = pinHandleMaxCount + 32;
				GCHandle* pNew = (GCHandle*)JniMem.Alloc(sizeof(GCHandle) * newCount);
				for(int i = 0; i < pinHandleMaxCount; i++)
				{
					pNew[i] = pinHandles[i];
				}
				for(int i = pinHandleMaxCount; i < newCount; i++)
				{
					pNew[i] = new GCHandle();
				}
				JniMem.Free((IntPtr)pinHandles);
				pinHandles = pNew;
				pinHandleMaxCount = newCount;
			}
			int index = pinHandleInUseCount++;
			if(!pinHandles[index].IsAllocated)
			{
				pinHandles[index] = GCHandle.Alloc(null, GCHandleType.Pinned);
			}
			pinHandles[index].Target = obj;
			return (void*)pinHandles[index].AddrOfPinnedObject();
		}

		private void UnpinObject(object obj)
		{
			for(int i = 0; i < pinHandleInUseCount; i++)
			{
				if(pinHandles[i].Target == obj)
				{
					pinHandles[i].Target = pinHandles[--pinHandleInUseCount].Target;
					pinHandles[pinHandleInUseCount].Target = null;
					return;
				}
			}
		}

		internal static void* GetPrimitiveArrayCritical(JNIEnv* pEnv, jarray array, jboolean* isCopy)
		{
			if(isCopy != null)
			{
				*isCopy = JNI_FALSE;
			}
			return pEnv->PinObject(pEnv->UnwrapRef(array));
		}

		internal static void ReleasePrimitiveArrayCritical(JNIEnv* pEnv, jarray array, void* carray, jint mode)
		{
			pEnv->UnpinObject(pEnv->UnwrapRef(array));
		}

		internal static jchar* GetStringCritical(JNIEnv* pEnv, jstring str, jboolean* isCopy)
		{
			if(isCopy != null)
			{
				*isCopy = JNI_FALSE;
			}
			return (jchar*)pEnv->PinObject(pEnv->UnwrapRef(str));
		}

		internal static void ReleaseStringCritical(JNIEnv* pEnv, jstring str, jchar* cstring)
		{
			pEnv->UnpinObject(pEnv->UnwrapRef(str));
		}

		internal static jweak NewWeakGlobalRef(JNIEnv* pEnv, jobject obj)
		{
			object o = pEnv->UnwrapRef(obj);
			if(o == null)
			{
				return IntPtr.Zero;
			}
			lock(GlobalRefs.weakRefLock)
			{
				for(int i = 0; i < GlobalRefs.weakRefs.Length; i++)
				{
					if(!GlobalRefs.weakRefs[i].IsAllocated)
					{
						GlobalRefs.weakRefs[i] = GCHandle.Alloc(o, GCHandleType.WeakTrackResurrection);
						return (IntPtr)(- (i | (1 << 30)));
					}
				}
				int len = GlobalRefs.weakRefs.Length;
				GCHandle[] tmp = new GCHandle[len * 2];
				Array.Copy(GlobalRefs.weakRefs, 0, tmp, 0, len);
				tmp[len] = GCHandle.Alloc(o, GCHandleType.WeakTrackResurrection);
				GlobalRefs.weakRefs = tmp;
				return (IntPtr)(- (len | (1 << 30)));
			}
		}

		internal static void DeleteWeakGlobalRef(JNIEnv* pEnv, jweak obj)
		{
			int i = obj.ToInt32();
			if(i < 0)
			{
				i = -i;
				i -= (1 << 30);
				lock(GlobalRefs.weakRefLock)
				{
					GlobalRefs.weakRefs[i].Free();
				}
			}
			if(i > 0)
			{
				Debug.Assert(false, "local ref passed to DeleteWeakGlobalRef");
			}
		}

		internal static jboolean ExceptionCheck(JNIEnv* pEnv)
		{
			ManagedJNIEnv env = pEnv->GetManagedJNIEnv();
			return env.pendingException != null ? JNI_TRUE : JNI_FALSE;
		}

		internal static jobject NewDirectByteBuffer(JNIEnv* pEnv, IntPtr address, jlong capacity)
		{
			try
			{
				if(capacity < 0 || capacity > int.MaxValue)
				{
					SetPendingException(pEnv, new java.lang.IllegalArgumentException("capacity"));
					return IntPtr.Zero;
				}
				return pEnv->MakeLocalRef(JVM.NewDirectByteBuffer(address.ToInt64(), (int)capacity));
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, ikvm.runtime.Util.mapException(x));
				return IntPtr.Zero;
			}
		}

		internal static IntPtr GetDirectBufferAddress(JNIEnv* pEnv, jobject buf)
		{
			try
			{
				return (IntPtr)((sun.nio.ch.DirectBuffer)pEnv->UnwrapRef(buf)).address();
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, ikvm.runtime.Util.mapException(x));
				return IntPtr.Zero;
			}
		}
		
		internal static jlong GetDirectBufferCapacity(JNIEnv* pEnv, jobject buf)
		{
			try
			{
				return (jlong)(long)((java.nio.Buffer)pEnv->UnwrapRef(buf)).capacity();
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, ikvm.runtime.Util.mapException(x));
				return 0;
			}
		}

		internal static int GetObjectRefType(JNIEnv* pEnv, jobject obj)
		{
			int i = obj.ToInt32();
			if(i >= 0)
			{
				return JNILocalRefType;
			}
			i = -i;
			if((i & (1 << 30)) != 0)
			{
				return JNIWeakGlobalRefType;
			}
			else
			{
				return JNIGlobalRefType;
			}
		}

		internal IntPtr MakeLocalRef(object obj)
		{
			return GetManagedJNIEnv().MakeLocalRef(obj);
		}

		internal object UnwrapRef(IntPtr o)
		{
			int i = o.ToInt32();
			if(i > 0)
			{
				return GetManagedJNIEnv().UnwrapLocalRef(i);
			}
			if(i < 0)
			{
				return GlobalRefs.Unwrap(i);
			}
			return null;
		}

		internal static object UnwrapRef(ManagedJNIEnv env, IntPtr o)
		{
			int i = o.ToInt32();
			if(i > 0)
			{
				return env.UnwrapLocalRef(i);
			}
			if(i < 0)
			{
				return GlobalRefs.Unwrap(i);
			}
			return null;
		}
	}

	static class JniMem
	{
		internal static IntPtr Alloc(int cb)
		{
			return Marshal.AllocHGlobal(cb);
		}

		internal static void Free(IntPtr p)
		{
			Marshal.FreeHGlobal(p);
		}
	}

	static class TlsHack
	{
		[ThreadStatic]
		internal static JNIEnv.ManagedJNIEnv ManagedJNIEnv;
	}
}
