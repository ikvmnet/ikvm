/*
  Copyright (C) 2002-2008 Jeroen Frijters

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
using System.Collections.Specialized;
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

[assembly: AssemblyTitle("IKVM.NET Runtime JNI Layer")]
[assembly: AssemblyDescription("JVM for Mono and .NET")]
#if SIGNCODE
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("IKVM.Runtime, PublicKey=0024000004800000940000000602000000240000525341310004000001000100DD6B140E5209CAE3D1C710030021EF589D0F00D05ACA8771101A7E99E10EE063E66040DF96E6F842F717BFC5B62D2EC2B62CEB0282E4649790DACB424DB29B68ADC7EAEAB0356FCE04702379F84400B8427EDBB33DAB8720B9F16A42E2CDB87F885EF413DBC4229F2BD157C9B8DC2CD14866DEC5F31C764BFB9394CC3C60E6C0")]
#else
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("IKVM.Runtime")]
#endif

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
			return version == JNIEnv.JNI_VERSION_1_1 || version == JNIEnv.JNI_VERSION_1_2 || version == JNIEnv.JNI_VERSION_1_4 || version == JNIEnv.JNI_VERSION_1_6;
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
			Hashtable props = new Hashtable();
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

			JVM.SetProperties(props);

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

		public unsafe struct Frame
		{
			private JNIEnv* pJNIEnv;
			private RuntimeMethodHandle prevMethod;
			private object[] quickLocals;
			private int quickLocalIndex;
			private int prevLocalRefSlot;

			internal ClassLoaderWrapper Enter(ClassLoaderWrapper loader)
			{
				Enter(new RuntimeMethodHandle());
				ClassLoaderWrapper prev = (ClassLoaderWrapper)pJNIEnv->classLoader.Target;
				pJNIEnv->classLoader.Target = loader;
				return prev;
			}

			internal void Leave(ClassLoaderWrapper prev)
			{
				pJNIEnv->classLoader.Target = prev;
				Leave();
			}

			public IntPtr Enter(RuntimeMethodHandle method)
			{
				pJNIEnv = TlsHack.pJNIEnv;
				if(pJNIEnv == null)
				{
					pJNIEnv = JNIEnv.CreateJNIEnv();
				}
				prevMethod = pJNIEnv->currentMethod;
				pJNIEnv->currentMethod = method;
				object[][] localRefs = pJNIEnv->GetLocalRefs();
				prevLocalRefSlot = pJNIEnv->localRefSlot;
				pJNIEnv->localRefSlot++;
				if(pJNIEnv->localRefSlot >= localRefs.Length)
				{
					object[][] tmp = new object[localRefs.Length * 2][];
					Array.Copy(localRefs, 0, tmp, 0, localRefs.Length);
					pJNIEnv->localRefs.Target = localRefs = tmp;
				}
				if(localRefs[pJNIEnv->localRefSlot] == null)
				{
					localRefs[pJNIEnv->localRefSlot] = new object[32];
				}
				quickLocals = localRefs[pJNIEnv->localRefSlot];
				quickLocalIndex = (pJNIEnv->localRefSlot << JNIEnv.LOCAL_REF_SHIFT);
				return (IntPtr)(void*)pJNIEnv;
			}

			public void Leave()
			{
				pJNIEnv->currentMethod = prevMethod;
				Exception x = (Exception)pJNIEnv->UnwrapRef(pJNIEnv->pendingException);
				pJNIEnv->pendingException = IntPtr.Zero;
				object[][] localRefs = pJNIEnv->GetLocalRefs();
				while(pJNIEnv->localRefSlot != prevLocalRefSlot)
				{
					if(localRefs[pJNIEnv->localRefSlot] != null)
					{
						if(localRefs[pJNIEnv->localRefSlot].Length == JNIEnv.LOCAL_REF_BUCKET_SIZE)
						{
							// if the bucket is totally allocated, we're assuming a leaky method so we throw the bucket away
							localRefs[pJNIEnv->localRefSlot] = null;
						}
						else
						{
							Array.Clear(localRefs[pJNIEnv->localRefSlot], 0, localRefs[pJNIEnv->localRefSlot].Length);
						}
					}
					pJNIEnv->localRefSlot--;
				}
				if(x != null)
				{
					throw x;
				}
			}

			public static IntPtr GetFuncPtr(RuntimeMethodHandle method, string clazz, string name, string sig)
			{
				MethodBase mb = MethodBase.GetMethodFromHandle(method);
				ClassLoaderWrapper loader =	ClassLoaderWrapper.GetWrapperFromType(mb.DeclaringType).GetClassLoader();
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
						IntPtr pfunc = JniHelper.ikvm_GetProcAddress(p, shortMethodName, sp + 2 * IntPtr.Size);
						if(pfunc != IntPtr.Zero)
						{
							Tracer.Info(Tracer.Jni, "Native method {0}.{1}{2} found in library 0x{3:X} (short)", clazz, name, sig, p.ToInt64());
							return pfunc;
						}
						pfunc = JniHelper.ikvm_GetProcAddress(p, longMethodName, sp + 2 * IntPtr.Size);
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
				if(obj == null)
				{
					return IntPtr.Zero;
				}
				int i = quickLocalIndex & JNIEnv.LOCAL_REF_MASK;
				if(i < quickLocals.Length)
				{
					quickLocals[i] = obj;
					return (IntPtr)quickLocalIndex++;
				}
				else if(i < JNIEnv.LOCAL_REF_BUCKET_SIZE)
				{
					object[] tmp = new object[quickLocals.Length * 2];
					Array.Copy(quickLocals, 0, tmp, 0, quickLocals.Length);
					quickLocals = tmp;
					object[][] localRefs = (object[][])pJNIEnv->localRefs.Target;
					localRefs[pJNIEnv->localRefSlot] = quickLocals;
					quickLocals[i] = obj;
					return (IntPtr)quickLocalIndex++;
				}
				else
				{
					// this can't happen, because LOCAL_REF_BUCKET_SIZE is larger than the maximum number of object
					// references that can be required by a native method call (256 arguments + a class reference)
					JVM.CriticalFailure("JNI.Frame.MakeLocalRef cannot spill into next slot", null);
					return IntPtr.Zero;
				}
			}

			public object UnwrapLocalRef(IntPtr p)
			{
				return pJNIEnv->UnwrapRef(p);
			}
		}
	}

	sealed class JniHelper
	{
		[DllImport("ikvm-native")]
		private static extern IntPtr ikvm_LoadLibrary(string filename);
		[DllImport("ikvm-native")]
		private static extern void ikvm_FreeLibrary(IntPtr handle);
		[DllImport("ikvm-native")]
		internal static extern IntPtr ikvm_GetProcAddress(IntPtr handle, string name, int argc);
		[DllImport("ikvm-native")]
		private unsafe static extern int ikvm_CallOnLoad(IntPtr method, void* jvm, void* reserved);
		[DllImport("ikvm-native")]
		internal unsafe static extern void** ikvm_GetJNIEnvVTable();
		[DllImport("ikvm-native")]
		internal unsafe static extern void* ikvm_MarshalDelegate(Delegate d);

		private static ArrayList nativeLibraries = new ArrayList();
		internal static readonly object JniLock = new object();

		// MONOBUG with mcs we can't pass ClassLoaderWrapper from IKVM.Runtime.dll to IKVM.Runtime.JNI.dll
		internal unsafe static int LoadLibrary(string filename, object loader)
		{
			return LoadLibrary(filename, (ClassLoaderWrapper)loader);
		}

		private unsafe static int LoadLibrary(string filename, ClassLoaderWrapper loader)
		{
			Tracer.Info(Tracer.Jni, "loadLibrary: {0}, class loader: {1}", filename, loader);
			lock(JniLock)
			{
				IntPtr p = ikvm_LoadLibrary(filename);
				if(p == IntPtr.Zero)
				{
					Tracer.Info(Tracer.Jni, "Library not found: {0}", filename);
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
							ikvm_FreeLibrary(p);
							Tracer.Warning(Tracer.Jni, "Library was already loaded: {0}", filename);
							return 1;
						}
					}
					if(nativeLibraries.Contains(p))
					{
						string msg = string.Format("Native library {0} already loaded in another classloader", filename);
						Tracer.Error(Tracer.Jni, "UnsatisfiedLinkError: {0}", msg);
						throw new java.lang.UnsatisfiedLinkError(msg);
					}
					Tracer.Info(Tracer.Jni, "Library loaded: {0}, handle = 0x{1:X}", filename, p.ToInt64());
					IntPtr onload = ikvm_GetProcAddress(p, "JNI_OnLoad", IntPtr.Size * 2);
					if(onload != IntPtr.Zero)
					{
						Tracer.Info(Tracer.Jni, "Calling JNI_OnLoad on: {0}", filename);
						JNI.Frame f = new JNI.Frame();
						int version;
						ClassLoaderWrapper prevLoader = f.Enter(loader);
						try
						{
							// TODO on Whidbey we should be able to use Marshal.GetDelegateForFunctionPointer to call OnLoad
							version = ikvm_CallOnLoad(onload, JavaVM.pJavaVM, null);
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
					return 1;
				}
				catch
				{
					ikvm_FreeLibrary(p);
					throw;
				}
			}
		}
	}

	class GlobalRefs
	{
		internal static System.Collections.ArrayList globalRefs = new System.Collections.ArrayList();
		internal static readonly object weakRefLock = new object();
		internal static GCHandle[] weakRefs = new GCHandle[16];
	}

	unsafe class VtableBuilder
	{
		// TODO on Whidbey we should use generics to cut down on the number of delegates needed
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
			void** pmcpp = JniHelper.ikvm_GetJNIEnvVTable();
			void** p = (void**)JniMem.Alloc(IntPtr.Size * vtableDelegates.Length);
			for(int i = 0; i < vtableDelegates.Length; i++)
			{
				if(vtableDelegates[i] != null)
				{
					// TODO on Whidbey we can use Marshal.GetFunctionPointerForDelegate
					p[i] = JniHelper.ikvm_MarshalDelegate(vtableDelegates[i]);
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
				pJavaVM->vtable[i] = JniHelper.ikvm_MarshalDelegate(vtableDelegates[i]);
			}
		}

		internal static jint DestroyJavaVM(JavaVM* pJVM)
		{
			if(JNI.jvmDestroyed)
			{
				return JNIEnv.JNI_ERR;
			}
			JNI.jvmDestroyed = true;
#if OPENJDK
			IKVM.NativeCode.java.lang.Thread.WaitUntilLastJniThread();
#else
			JVM.Library.jniWaitUntilLastThread();
#endif
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
			JNIEnv* p = TlsHack.pJNIEnv;
			if(p != null)
			{
				*penv = p;
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
				// NOTE we're calling UnwrapRef on a null reference, but that's OK because group is a global reference
				object threadGroup = p->UnwrapRef(pAttachArgs->group);
				if(threadGroup != null)
				{
#if OPENJDK
					IKVM.NativeCode.java.lang.Thread.AttachThreadFromJni(threadGroup);
#else
					JVM.Library.setThreadGroup(threadGroup);
#endif
				}
			}
			*penv = JNIEnv.CreateJNIEnv();
			return JNIEnv.JNI_OK;
		}

		internal static jint DetachCurrentThread(JavaVM* pJVM)
		{
			if(TlsHack.pJNIEnv == null)
			{
				// the JDK allows detaching from an already detached thread
				return JNIEnv.JNI_OK;
			}
			// TODO if we set Thread.IsBackground to false when we attached, now might be a good time to set it back to true.
			JNIEnv.FreeJNIEnv();
#if OPENJDK
			IKVM.NativeCode.java.lang.VMThread.jniDetach();
#else
			JVM.Library.jniDetach();
#endif
			return JNIEnv.JNI_OK;
		}

		internal static jint GetEnv(JavaVM* pJVM, void **penv, jint version)
		{
			if(JNI.IsSupportedJniVersion(version))
			{
				JNIEnv* p = TlsHack.pJNIEnv;
				if(p != null)
				{
					*penv = p;
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
		// NOTE LOCAL_REF_BUCKET_SIZE must be at least 512 to allow all method arguments to fit
		// in a single slot, because Frame.MakeLocalRef() doesn't support spilling into a new bucket
		internal const int LOCAL_REF_SHIFT = 10;
		internal const int LOCAL_REF_BUCKET_SIZE = (1 << LOCAL_REF_SHIFT);
		internal const int LOCAL_REF_MASK = (LOCAL_REF_BUCKET_SIZE - 1);
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
		internal const int JNIInvalidRefType = 0;
		internal const int JNILocalRefType = 1;
		internal const int JNIGlobalRefType = 2;
		internal const int JNIWeakGlobalRefType = 3;
		internal const sbyte JNI_TRUE = 1;
		internal const sbyte JNI_FALSE = 0;
		internal void* vtable;
		internal GCHandle localRefs;
		internal int localRefSlot;
		internal IntPtr pendingException;
		internal RuntimeMethodHandle currentMethod;
		internal GCHandle classLoader;
		internal GCHandle criticalArrayHandle1;
		internal GCHandle criticalArrayHandle2;
		private static LocalDataStoreSlot cleanupHelperDataSlot = System.Threading.Thread.AllocateDataSlot();

		static JNIEnv()
		{
			// we set the field here so that IKVM.Runtime.dll doesn't have to load us if we're not otherwise needed
			IKVM.NativeCode.java.lang.SecurityManager.jniAssembly = typeof(JNIEnv).Assembly;
		}

		unsafe class JNIEnvCleanupHelper
		{
			private JNIEnv* pJNIEnv;

			internal JNIEnvCleanupHelper(JNIEnv* pJNIEnv)
			{
				this.pJNIEnv = pJNIEnv;
			}

			~JNIEnvCleanupHelper()
			{
				// NOTE don't clean up when we're being unloaded (we'll get cleaned up anyway and because
				// of the unorderedness of the finalization process native code could still be run after
				// we run).
				// NOTE when we're not the default AppDomain and we're being unloaded,
				// we're leaking the JNIEnv (but since JNI outside of the default AppDomain isn't currently supported,
				// I can live with that).
				if(!Environment.HasShutdownStarted)
				{
					if(pJNIEnv->localRefs.IsAllocated)
					{
						pJNIEnv->localRefs.Free();
					}
					if(pJNIEnv->classLoader.IsAllocated)
					{
						pJNIEnv->classLoader.Free();
					}
					if(pJNIEnv->criticalArrayHandle1.IsAllocated)
					{
						pJNIEnv->criticalArrayHandle1.Free();
					}
					if(pJNIEnv->criticalArrayHandle2.IsAllocated)
					{
						pJNIEnv->criticalArrayHandle2.Free();
					}
					JniMem.Free((IntPtr)(void*)pJNIEnv);
				}
			}
		}

		internal static JNIEnv* CreateJNIEnv()
		{
			JNIEnv* pJNIEnv = TlsHack.pJNIEnv = (JNIEnv*)JniMem.Alloc(sizeof(JNIEnv));
			// don't touch the LocalDataStore slot when we're being unloaded
			// (it may have been finalized already)
			if(!Environment.HasShutdownStarted)
			{
				System.Threading.Thread.SetData(cleanupHelperDataSlot, new JNIEnvCleanupHelper(pJNIEnv));
			}
			pJNIEnv->vtable = VtableBuilder.vtable;
			object[][] localRefs = new object[32][];
			localRefs[0] = new object[JNIEnv.LOCAL_REF_BUCKET_SIZE];
			// stuff something in the first entry to make sure we don't hand out a zero handle
			// (a zero handle corresponds to a null reference)
			localRefs[0][0] = "";
			pJNIEnv->localRefs = GCHandle.Alloc(localRefs);
			pJNIEnv->localRefSlot = 0;
			pJNIEnv->pendingException = IntPtr.Zero;
			pJNIEnv->currentMethod = new RuntimeMethodHandle();
			pJNIEnv->classLoader = GCHandle.Alloc(null);
			pJNIEnv->criticalArrayHandle1 = GCHandle.Alloc(null, GCHandleType.Pinned);
			pJNIEnv->criticalArrayHandle2 = GCHandle.Alloc(null, GCHandleType.Pinned);
			return pJNIEnv;
		}

		internal static void FreeJNIEnv()
		{
			// don't touch the LocalDataStore slot when we're being unloaded
			// (it may have been finalized already)
			if(!Environment.HasShutdownStarted)
			{
				// the cleanup helper will eventually free the JNIEnv
				System.Threading.Thread.SetData(cleanupHelperDataSlot, null);
			}
			TlsHack.pJNIEnv = null;
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

		// this method returns a simplified method argument descriptor.
		// some examples:
		// "()V" -> ""
		// "(ILjava.lang.String;)I" -> "IL"
		// "([Ljava.lang.String;)V" -> "L"
		private static string GetMethodArgList(IntPtr cookie)
		{
			try
			{
				StringBuilder sb = new StringBuilder();
				string s = MethodWrapper.FromCookie(cookie).Signature;
				for(int i = 1;; i++)
				{
					switch(s[i])
					{
						case '[':
							while(s[i] == '[') i++;
							if(s[i] == 'L')
							{
								while(s[i] != ';') i++;
							}
							sb.Append('L');
							break;
						case 'L':
							while(s[i] != ';') i++;
							sb.Append('L');
							break;
						case ')':
							return sb.ToString();
						default:
							sb.Append(s[i]);
							break;
					}
				}
			}
			catch
			{
				Debug.Assert(false);
				throw;
			}
		}

		internal static jint GetMethodArgs(JNIEnv* pEnv, IntPtr method, byte* sig)
		{
			string s = GetMethodArgList(method);
			for(int i = 0; i < s.Length; i++)
			{
				sig[i] = (byte)s[i];
			}
			return s.Length;
		}

		internal static jint GetVersion(JNIEnv* pEnv)
		{
			return JNI_VERSION_1_6;
		}

		internal static jclass DefineClass(JNIEnv* pEnv, byte* name, jobject loader, jbyte* pbuf, jint length)
		{
			try
			{
				byte[] buf = new byte[length];
				Marshal.Copy((IntPtr)(void*)pbuf, buf, 0, length);
				// TODO what should the protection domain be?
				// NOTE I'm assuming name is platform encoded (as opposed to UTF-8), but the Sun JVM only seems to work for ASCII.
#if OPENJDK
				return pEnv->MakeLocalRef(IKVM.NativeCode.java.lang.ClassLoader.defineClass0(pEnv->UnwrapRef(loader), name != null ? StringFromOEM(name) : null, buf, 0, buf.Length, null));
#else
				return pEnv->MakeLocalRef(IKVM.NativeCode.java.lang.VMClassLoader.defineClassImpl(pEnv->UnwrapRef(loader), name != null ? StringFromOEM(name) : null, buf, 0, buf.Length, null));
#endif
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, x);
				return IntPtr.Zero;
			}
		}

		private static ClassLoaderWrapper FindNativeMethodClassLoader(JNIEnv* pEnv)
		{
			if(pEnv->currentMethod.Value != IntPtr.Zero)
			{
				MethodBase mb = MethodBase.GetMethodFromHandle(pEnv->currentMethod);
				return ClassLoaderWrapper.GetWrapperFromType(mb.DeclaringType).GetClassLoader();
			}
			if(pEnv->classLoader.Target != null)
			{
				return (ClassLoaderWrapper)pEnv->classLoader.Target;
			}
			return ClassLoaderWrapper.GetBootstrapClassLoader();
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
			return MethodWrapper.FromMethodOrConstructor(pEnv->UnwrapRef(method)).Cookie;
		}

		internal static jfieldID FromReflectedField(JNIEnv* pEnv, jobject field)
		{
			return FieldWrapper.FromField(pEnv->UnwrapRef(field)).Cookie;
		}

		internal static jobject ToReflectedMethod(JNIEnv* pEnv, jclass clazz_ignored, jmethodID method, jboolean isStatic)
		{
			return pEnv->MakeLocalRef(MethodWrapper.FromCookie(method).ToMethodOrConstructor(true));
		}

		internal static jclass GetSuperclass(JNIEnv* pEnv, jclass sub)
		{
			TypeWrapper wrapper = TypeWrapper.FromClass(pEnv->UnwrapRef(sub)).BaseTypeWrapper;
			return pEnv->MakeLocalRef(wrapper == null ? null : wrapper.ClassObject);
		}

		internal static jboolean IsAssignableFrom(JNIEnv* pEnv, jclass sub, jclass super)
		{
			TypeWrapper w1 = TypeWrapper.FromClass(pEnv->UnwrapRef(sub));
			TypeWrapper w2 = TypeWrapper.FromClass(pEnv->UnwrapRef(super));
			return w1.IsAssignableTo(w2) ? JNI_TRUE : JNI_FALSE;
		}

		internal static jobject ToReflectedField(JNIEnv* pEnv, jclass clazz_ignored, jfieldID field, jboolean isStatic)
		{
			return pEnv->MakeLocalRef(FieldWrapper.FromCookie(field).ToField(true));
		}

		private static void SetPendingException(JNIEnv* pEnv, Exception x)
		{
			DeleteLocalRef(pEnv, pEnv->pendingException);
			pEnv->pendingException = x == null ? IntPtr.Zero : pEnv->MakeLocalRef(ikvm.runtime.Util.mapException(x));
		}

		internal static jint Throw(JNIEnv* pEnv, jthrowable throwable)
		{
			DeleteLocalRef(pEnv, pEnv->pendingException);
			pEnv->pendingException = NewLocalRef(pEnv, throwable);
			return JNI_OK;
		}

		internal static jint ThrowNew(JNIEnv* pEnv, jclass clazz, byte* msg)
		{
			TypeWrapper wrapper = TypeWrapper.FromClass(pEnv->UnwrapRef(clazz));
			MethodWrapper mw = wrapper.GetMethodWrapper("<init>", "(Ljava.lang.String;)V", false);
			if(mw != null)
			{
				jint rc;
				Exception exception;
				try
				{
					wrapper.Finish();
					exception = (Exception)mw.Invoke(null, new object[] { StringFromOEM(msg) }, false);
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
			return NewLocalRef(pEnv, pEnv->pendingException);
		}

		internal static void ExceptionDescribe(JNIEnv* pEnv)
		{
			Exception x = (Exception)pEnv->UnwrapRef(pEnv->pendingException);
			if(x != null)
			{
				SetPendingException(pEnv, null);
				try
				{
					java.lang.Throwable.instancehelper_printStackTrace(x);
				}
				catch(Exception ex)
				{
					Debug.Assert(false, ex.ToString());
				}
			}
		}

		internal static void ExceptionClear(JNIEnv* pEnv)
		{
			DeleteLocalRef(pEnv, pEnv->pendingException);
			pEnv->pendingException = IntPtr.Zero;
		}

		internal static void FatalError(JNIEnv* pEnv, byte* msg)
		{
			Console.Error.WriteLine("FATAL ERROR in native method: {0}", msg == null ? "(null)" : StringFromOEM(msg));
			Console.Error.WriteLine(new StackTrace(1, true));
			Environment.Exit(1);
		}

		internal static jint PushLocalFrame(JNIEnv* pEnv, jint capacity)
		{
			object[][] localRefs = pEnv->GetLocalRefs();
			pEnv->localRefSlot += 2;
			if(pEnv->localRefSlot >= localRefs.Length)
			{
				object[][] tmp = new object[localRefs.Length * 2][];
				Array.Copy(localRefs, 0, tmp, 0, localRefs.Length);
				pEnv->localRefs.Target = localRefs = tmp;
			}
			// we use a null slot to mark the fact that we used PushLocalFrame
			localRefs[pEnv->localRefSlot - 1] = null;
			if(localRefs[pEnv->localRefSlot] == null)
			{
				// we can't use capacity directly, because the array length must be a power of two
				// and it can't be bigger than LOCAL_REF_BUCKET_SIZE
				int r = 1;
				capacity = Math.Min(capacity, LOCAL_REF_BUCKET_SIZE);
				while(r < capacity)
				{
					r *= 2;
				}
				localRefs[pEnv->localRefSlot] = new object[r];
			}
			return JNI_OK;
		}

		internal static jobject PopLocalFrame(JNIEnv* pEnv, jobject result)
		{
			object res = pEnv->UnwrapRef(result);
			object[][] localRefs = pEnv->GetLocalRefs();
			while(localRefs[pEnv->localRefSlot] != null)
			{
				localRefs[pEnv->localRefSlot] = null;
				pEnv->localRefSlot--;
			}
			pEnv->localRefSlot--;
			return pEnv->MakeLocalRef(res);
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

		internal object[][] GetLocalRefs()
		{
			return (object[][])localRefs.Target;
		}

		internal static void DeleteLocalRef(JNIEnv* pEnv, jobject obj)
		{
			int i = obj.ToInt32();
			if(i > 0)
			{
				object[][] localRefs = pEnv->GetLocalRefs();
				localRefs[i >> LOCAL_REF_SHIFT][i & LOCAL_REF_MASK] = null;
				return;
			}
			if(i < 0)
			{
				Debug.Assert(false, "bogus localref in DeleteLocalRef");
			}
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
			try
			{
				TypeWrapper wrapper = TypeWrapper.FromClass(pEnv->UnwrapRef(clazz));
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

		private static object InvokeHelper(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue *args, bool nonVirtual)
		{
			string sig = GetMethodArgList(methodID);
			object[] argarray = new object[sig.Length];
			for(int i = 0; i < sig.Length; i++)
			{
				switch(sig[i])
				{
					case 'Z':
						argarray[i] = args[i].z != JNI_FALSE;
						break;
					case 'B':
						argarray[i] = args[i].b;
						break;
					case 'C':
						argarray[i] = (char)args[i].c;
						break;
					case 'S':
						argarray[i] = args[i].s;
						break;
					case 'I':
						argarray[i] = args[i].i;
						break;
					case 'J':
						argarray[i] = args[i].j;
						break;
					case 'F':
						argarray[i] = args[i].f;
						break;
					case 'D':
						argarray[i] = args[i].d;
						break;
					case 'L':
						argarray[i] = pEnv->UnwrapRef(args[i].l);
						break;
				}
			}
			try
			{
				return MethodWrapper.FromCookie(methodID).Invoke(pEnv->UnwrapRef(obj), argarray, nonVirtual);
			}
			catch(java.lang.reflect.InvocationTargetException x)
			{
				SetPendingException(pEnv, ikvm.runtime.Util.mapException(x.getCause()));
				return null;
			}
			catch(Exception x)
			{
				SetPendingException(pEnv, ikvm.runtime.Util.mapException(x));
				return null;
			}
		}

		internal static jobject NewObjectA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue *args)
		{
			jobject obj = AllocObject(pEnv, clazz);
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
			object objClass = IKVM.NativeCode.ikvm.runtime.Util.getClassFromObject(pEnv->UnwrapRef(obj));
			TypeWrapper w1 = TypeWrapper.FromClass(pEnv->UnwrapRef(clazz));
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

		private static jmethodID FindMethodID(JNIEnv* pEnv, jclass clazz, byte* name, byte* sig, bool isstatic)
		{
			try
			{
				TypeWrapper wrapper = TypeWrapper.FromClass(pEnv->UnwrapRef(clazz));
				wrapper.Finish();
				string methodsig = StringFromUTF8(sig);
				// don't allow dotted names!
				if(methodsig.IndexOf('.') < 0)
				{
					MethodWrapper mw = GetMethodImpl(wrapper, StringFromUTF8(name), methodsig.Replace('/', '.'));
					if(mw != null)
					{
						if(mw.IsStatic == isstatic)
						{
							mw.Link();
							return mw.Cookie;
						}
					}
				}
				SetPendingException(pEnv, new java.lang.NoSuchMethodError(string.Format("{0}{1}", StringFromUTF8(name), StringFromUTF8(sig).Replace('/', '.'))));
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
				TypeWrapper wrapper = TypeWrapper.FromClass(pEnv->UnwrapRef(clazz));
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

		private static void SetFieldValue(jfieldID cookie, object obj, object val)
		{
			try
			{
				FieldWrapper.FromCookie(cookie).SetValue(obj, val);
			}
			catch
			{
				Debug.Assert(false);
				throw;
			}
		}

		private static object GetFieldValue(jfieldID cookie, object obj)
		{
			try
			{
				return FieldWrapper.FromCookie(cookie).GetValue(obj);
			}
			catch
			{
				Debug.Assert(false);
				throw;
			}
		}

		internal static jobject GetObjectField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
		{
			return pEnv->MakeLocalRef(GetFieldValue(fieldID, pEnv->UnwrapRef(obj)));
		}

		internal static jboolean GetBooleanField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
		{
			return ((bool)GetFieldValue(fieldID, pEnv->UnwrapRef(obj))) ? JNI_TRUE : JNI_FALSE;
		}

		internal static jbyte GetByteField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
		{
			return (jbyte)(byte)GetFieldValue(fieldID, pEnv->UnwrapRef(obj));
		}

		internal static jchar GetCharField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
		{
			return (jchar)(char)GetFieldValue(fieldID, pEnv->UnwrapRef(obj));
		}

		internal static jshort GetShortField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
		{
			return (jshort)(short)GetFieldValue(fieldID, pEnv->UnwrapRef(obj));
		}

		internal static jint GetIntField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
		{
			return (jint)(int)GetFieldValue(fieldID, pEnv->UnwrapRef(obj));
		}

		internal static jlong GetLongField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
		{
			return (jlong)(long)GetFieldValue(fieldID, pEnv->UnwrapRef(obj));
		}

		internal static jfloat GetFloatField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
		{
			return (jfloat)(float)GetFieldValue(fieldID, pEnv->UnwrapRef(obj));
		}

		internal static jdouble GetDoubleField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
		{
			return (jdouble)(double)GetFieldValue(fieldID, pEnv->UnwrapRef(obj));
		}

		internal static void SetObjectField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jobject val)
		{
			SetFieldValue(fieldID, pEnv->UnwrapRef(obj), pEnv->UnwrapRef(val));
		}

		internal static void SetBooleanField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jboolean val)
		{
			SetFieldValue(fieldID, pEnv->UnwrapRef(obj), val != JNI_FALSE);
		}

		internal static void SetByteField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jbyte val)
		{
			SetFieldValue(fieldID, pEnv->UnwrapRef(obj), (byte)val);
		}

		internal static void SetCharField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jchar val)
		{
			SetFieldValue(fieldID, pEnv->UnwrapRef(obj), (char)val);
		}

		internal static void SetShortField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jshort val)
		{
			SetFieldValue(fieldID, pEnv->UnwrapRef(obj), (short)val);
		}

		internal static void SetIntField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jint val)
		{
			SetFieldValue(fieldID, pEnv->UnwrapRef(obj), (int)val);
		}

		internal static void SetLongField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jlong val)
		{
			SetFieldValue(fieldID, pEnv->UnwrapRef(obj), (long)val);
		}

		internal static void SetFloatField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jfloat val)
		{
			SetFieldValue(fieldID, pEnv->UnwrapRef(obj), (float)val);
		}

		internal static void SetDoubleField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jdouble val)
		{
			SetFieldValue(fieldID, pEnv->UnwrapRef(obj), (double)val);
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
			return pEnv->MakeLocalRef(GetFieldValue(fieldID, null));
		}

		internal static jboolean GetStaticBooleanField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
		{
			return ((bool)GetFieldValue(fieldID, null)) ? JNI_TRUE : JNI_FALSE;
		}

		internal static jbyte GetStaticByteField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
		{
			return (jbyte)(byte)GetFieldValue(fieldID, null);
		}

		internal static jchar GetStaticCharField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
		{
			return (jchar)(char)GetFieldValue(fieldID, null);
		}

		internal static jshort GetStaticShortField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
		{
			return (jshort)(short)GetFieldValue(fieldID, null);
		}

		internal static jint GetStaticIntField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
		{
			return (jint)(int)GetFieldValue(fieldID, null);
		}

		internal static jlong GetStaticLongField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
		{
			return (jlong)(long)GetFieldValue(fieldID, null);
		}

		internal static jfloat GetStaticFloatField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
		{
			return (jfloat)(float)GetFieldValue(fieldID, null);
		}

		internal static jdouble GetStaticDoubleField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
		{
			return (jdouble)(double)GetFieldValue(fieldID, null);
		}

		internal static void SetStaticObjectField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jobject val)
		{
			SetFieldValue(fieldID, null, pEnv->UnwrapRef(val));
		}

		internal static void SetStaticBooleanField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jboolean val)
		{
			SetFieldValue(fieldID, null, val != JNI_FALSE);
		}

		internal static void SetStaticByteField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jbyte val)
		{
			SetFieldValue(fieldID, null, (byte)val);
		}

		internal static void SetStaticCharField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jchar val)
		{
			SetFieldValue(fieldID, null, (char)val);
		}

		internal static void SetStaticShortField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jshort val)
		{
			SetFieldValue(fieldID, null, (short)val);
		}

		internal static void SetStaticIntField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jint val)
		{
			SetFieldValue(fieldID, null, (int)val);
		}

		internal static void SetStaticLongField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jlong val)
		{
			SetFieldValue(fieldID, null, (long)val);
		}

		internal static void SetStaticFloatField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jfloat val)
		{
			SetFieldValue(fieldID, null, (float)val);
		}

		internal static void SetStaticDoubleField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jdouble val)
		{
			SetFieldValue(fieldID, null, (double)val);
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
				Array array = Array.CreateInstance(TypeWrapper.FromClass(pEnv->UnwrapRef(clazz)).TypeAsArrayType, len);
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
				TypeWrapper wrapper = TypeWrapper.FromClass(pEnv->UnwrapRef(clazz));
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
				TypeWrapper wrapper = TypeWrapper.FromClass(pEnv->UnwrapRef(clazz));
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
				System.Threading.Monitor.Enter(pEnv->UnwrapRef(obj));
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

		private static int GetPrimitiveArrayElementSize(Array ar)
		{
			Type type = ar.GetType().GetElementType();
			if(type == PrimitiveTypeWrapper.BYTE.TypeAsArrayType || type == PrimitiveTypeWrapper.BOOLEAN.TypeAsArrayType)
			{
				return 1;
			}
			else if(type == PrimitiveTypeWrapper.SHORT.TypeAsArrayType || type == PrimitiveTypeWrapper.CHAR.TypeAsArrayType)
			{
				return 2;
			}
			else if(type == PrimitiveTypeWrapper.INT.TypeAsArrayType || type == PrimitiveTypeWrapper.FLOAT.TypeAsArrayType)
			{
				return 4;
			}
			else if(type == PrimitiveTypeWrapper.LONG.TypeAsArrayType || type == PrimitiveTypeWrapper.DOUBLE.TypeAsArrayType)
			{
				return 8;
			}
			else
			{
				JVM.CriticalFailure("invalid array type", null);
				return 0;
			}
		}

		internal static void* GetPrimitiveArrayCritical(JNIEnv* pEnv, jarray array, jboolean* isCopy)
		{
			Array ar = (Array)pEnv->UnwrapRef(array);
			if(pEnv->criticalArrayHandle1.Target == null)
			{
				pEnv->criticalArrayHandle1.Target = ar;
				if(isCopy != null)
				{
					*isCopy = JNI_FALSE;
				}
				return (void*)pEnv->criticalArrayHandle1.AddrOfPinnedObject();
			}
			if(pEnv->criticalArrayHandle2.Target == null)
			{
				pEnv->criticalArrayHandle2.Target = ar;
				if(isCopy != null)
				{
					*isCopy = JNI_FALSE;
				}
				return (void*)pEnv->criticalArrayHandle2.AddrOfPinnedObject();
			}
			// TODO not 64-bit safe (len can overflow)
			int len = ar.Length * GetPrimitiveArrayElementSize(ar);
			GCHandle h = GCHandle.Alloc(ar, GCHandleType.Pinned);
			try
			{
				IntPtr hglobal = JniMem.Alloc(len);
				byte* pdst = (byte*)(void*)hglobal;
				byte* psrc = (byte*)(void*)h.AddrOfPinnedObject();
				// TODO isn't there a managed memcpy?
				for(int i = 0; i < len; i++)
				{
					*pdst++ = *psrc++;
				}
				if(isCopy != null)
				{
					*isCopy = JNI_TRUE;
				}
				return (void*)hglobal;
			}
			finally
			{
				h.Free();
			}		
		}

		internal static void ReleasePrimitiveArrayCritical(JNIEnv* pEnv, jarray array, void* carray, jint mode)
		{
			Array ar = (Array)pEnv->UnwrapRef(array);
			if(pEnv->criticalArrayHandle1.Target == ar
				&& (void*)pEnv->criticalArrayHandle1.AddrOfPinnedObject() == carray)
			{
				if(mode == 0 || mode == JNI_ABORT)
				{
					pEnv->criticalArrayHandle1.Target = null;
				}
				return;
			}
			if(pEnv->criticalArrayHandle2.Target == ar
				&& (void*)pEnv->criticalArrayHandle2.AddrOfPinnedObject() == carray)
			{
				if(mode == 0 || mode == JNI_ABORT)
				{
					pEnv->criticalArrayHandle2.Target = null;
				}
				return;
			}
			if(mode == 0 || mode == JNI_COMMIT)
			{
				// TODO not 64-bit safe (len can overflow)
				int len = ar.Length * GetPrimitiveArrayElementSize(ar);
				GCHandle h = GCHandle.Alloc(ar, GCHandleType.Pinned);
				try
				{
					byte* pdst = (byte*)(void*)h.AddrOfPinnedObject();
					byte* psrc = (byte*)(void*)carray;
					// TODO isn't there a managed memcpy?
					for(int i = 0; i < len; i++)
					{
						*pdst++ = *psrc++;
					}
				}
				finally
				{
					h.Free();
				}
			}
			if(mode == 0 || mode == JNI_ABORT)
			{
				JniMem.Free((IntPtr)carray);
			}
		}

		internal static jchar* GetStringCritical(JNIEnv* pEnv, jstring str, jboolean* isCopy)
		{
			string s = (string)pEnv->UnwrapRef(str);
			if(s != null)
			{
				if(isCopy != null)
				{
					*isCopy = JNI_TRUE;
				}
				return (jchar*)(void*)Marshal.StringToHGlobalUni(s);		
			}
			SetPendingException(pEnv, new java.lang.NullPointerException());
			return null;
		}

		internal static void ReleaseStringCritical(JNIEnv* pEnv, jstring str, jchar* cstring)
		{
			Marshal.FreeHGlobal((IntPtr)(void*)cstring);
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
			return pEnv->UnwrapRef(pEnv->pendingException) != null ? JNI_TRUE : JNI_FALSE;
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
#if OPENJDK
				return pEnv->MakeLocalRef(JVM.CoreAssembly.GetType("java.nio.DirectByteBuffer")
					.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(long), typeof(int) }, null)
					.Invoke(new object[] { address.ToInt64(), (int)capacity }));
#else
				return pEnv->MakeLocalRef(JVM.Library.newDirectByteBuffer(address, (int)capacity));
#endif
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
#if OPENJDK
				return (IntPtr)((sun.nio.ch.DirectBuffer)pEnv->UnwrapRef(buf)).address();
#else
				return JVM.Library.getDirectBufferAddress(pEnv->UnwrapRef(buf));
#endif
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
			if(obj == null)
			{
				return IntPtr.Zero;
			}
			object[][] localRefs = GetLocalRefs();
			object[] active = localRefs[localRefSlot];
			for(int i = 0; i < active.Length; i++)
			{
				if(active[i] == null)
				{
					active[i] = obj;
					return (IntPtr)((localRefSlot << LOCAL_REF_SHIFT) + i);
				}
			}
			if(active.Length < LOCAL_REF_BUCKET_SIZE)
			{
				int i = active.Length;
				object[] tmp = new object[i * 2];
				Array.Copy(active, 0, tmp, 0, i);
				active = localRefs[localRefSlot] = tmp;
				active[i] = obj;
				return (IntPtr)((localRefSlot << LOCAL_REF_SHIFT) + i);
			}
			// if we get here, we're in a native method that most likely is leaking locals refs,
			// so we're going to allocate a new bucket and increment localRefSlot, this means that
			// any slots that become available in the previous bucket are not going to be reused,
			// but since we're assuming that the method is leaking anyway, that isn't a problem
			// (it's never a correctness issue, just a resource consumption issue)
			localRefSlot++;
			if(localRefSlot == localRefs.Length)
			{
				object[][] tmp = new object[localRefSlot * 2][];
				Array.Copy(localRefs, 0, tmp, 0, localRefSlot);
				this.localRefs.Target = localRefs = tmp;
			}
			if(localRefs[localRefSlot] == null)
			{
				localRefs[localRefSlot] = new object[LOCAL_REF_BUCKET_SIZE];
			}
			localRefs[localRefSlot][0] = obj;
			return (IntPtr)(localRefSlot << LOCAL_REF_SHIFT);
		}

		internal object UnwrapRef(IntPtr o)
		{
			int i = o.ToInt32();
			if(i > 0)
			{
				object[][] localRefs = GetLocalRefs();
				return localRefs[i >> LOCAL_REF_SHIFT][i & LOCAL_REF_MASK];
			}
			if(i < 0)
			{
				i = -i;
				if((i & (1 << 30)) != 0)
				{
					lock(GlobalRefs.weakRefLock)
					{
						return GlobalRefs.weakRefs[i - (1 << 30)].Target;
					}
				}
				else
				{
					lock(GlobalRefs.globalRefs)
					{
						return GlobalRefs.globalRefs[i - 1];
					}
				}
			}
			return null;
		}
	}

	class JniMem
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

	unsafe class TlsHack
	{
		[ThreadStatic]
		internal static JNIEnv* pJNIEnv;
	}
}
