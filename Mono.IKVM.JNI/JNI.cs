//
// JNI: a class used by IKVM.NET to access JNI related functionality.
//

//
// This is the mono version of the JNI provider. As much code is written in C#
// as possible. The code depends on having a non-copying garbage collector.
//

using System;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

public class VM
{
	public static Exception UnsatisfiedLinkError (string msg) {
		return JniHelper.UnsatisfiedLinkError (msg);
	}
}

public class JNI : IJniProvider
{
	static Hashtable modules = new Hashtable ();

	[DllImport("mono-ikvm-jni-native")]
		private static extern IntPtr native_load_native_library (string filename);

	[DllImport("mono-ikvm-jni-native")]
		private static extern bool native_lookup_symbol (IntPtr module, string symbol_name, ref int symbol);

	[DllImport("gmodule-2.0")]
	private static extern IntPtr g_module_open (string file_name, int flags);

	[DllImport("gmodule-2.0")]
	private static extern bool g_module_symbol (IntPtr module, string symbol_name, ref int symbol);

	public int LoadNativeLibrary (string filename) {
		Console.WriteLine ("LoadNativeLibrary : " + filename);

		IntPtr module = g_module_open (filename, 0);
		if (module == (IntPtr)0)
			return 0;

		modules [module] = filename;
		return 1;
	}

	public Type GetLocalRefStructType () {
		return typeof (IkvmJNIEnv);
	}

	public MethodInfo GetJniFuncPtrMethod () {
		return typeof (JNI).GetMethod ("GetJniFuncPtr");
	}

	//
	// Return a pointer to the C function implementing the given native
	// method.
	public static IntPtr GetJniFuncPtr (string method, string sig, string klass) {
		//		Console.WriteLine ("JNI_FUNC: " + method + " " + sig + " " + klass);

		StringBuilder mangledSig = new StringBuilder ();
		int sp = 0;
		for (int i = 1; sig[i] != ')'; ++i) {
			switch (sig[i]) {
			case '[':
				mangledSig.Append("_3");
				sp += 4;
				while(sig[++i] == '[')
						mangledSig.Append("_3");
				mangledSig.Append(sig[i]);
				if(sig[i] == 'L') {
					while(sig[++i] != ';') {
						if(sig[i] == '/')
							mangledSig.Append("_");
						else if(sig[i] == '_')
							mangledSig.Append("_1");
						else
							mangledSig.Append(sig[i]);
					}
					mangledSig.Append("_2");
				}
				break;
			case 'L':
				sp += 4;
				mangledSig.Append("L");
				while(sig[++i] != ';') {
					if(sig[i] == '/')
						mangledSig.Append("_");
					else if(sig[i] == '_')
						mangledSig.Append("_1");
					else
						mangledSig.Append(sig[i]);
				}
				mangledSig.Append("_2");
				break;
			case 'J':
			case 'D':
				mangledSig.Append(sig[i]);
				sp += 8;
				break;
			case 'F':
			case 'I':
			case 'C':
			case 'Z':
			case 'S':
			case 'B':
				mangledSig.Append(sig[i]);
				sp += 4;
				break;
			default:
				throw new NotImplementedException();
			}
		}

		string methodName = "";
		for (int pass = 0; pass < 2; ++pass) {
			methodName = String.Format ("Java_{0}_{1}", klass.Replace("_", "_1").Replace('/', '_'), method.Replace("_", "_1"));
			if (pass == 1)
				methodName = methodName + "__" + mangledSig;				
			Console.WriteLine ("METHOD_NAME1: " + methodName);

			foreach (IntPtr module in modules.Keys) {
				int function = 0;
				bool found = g_module_symbol (module, methodName, ref function);
				if (found) {
					//					Console.WriteLine ("FOUND IN " + modules[module] + " -> " + function);
					return new IntPtr (function);
				}
			}
		}

		throw VM.UnsatisfiedLinkError(methodName);
	}
}	

//
// When running on a runtime with a non-copying collector, we can use object
// references instead of handles.
//

// Hackish struct used for ptr<->reference conversion

[StructLayout (LayoutKind.Sequential)]
struct PtrStruct {
	public IntPtr ptr;
	public object localref;
}

[StructLayout (LayoutKind.Sequential)]
class JNIEnv {
	IntPtr func_table;

	JNIEnv self;

	object[] localrefs;

	int localref_ptr;

	public object pendingException;

	GCHandle gc_handle;

	public JNIEnv (IntPtr the_func_table) {
		func_table = the_func_table;
		localrefs = new object[32];
		self = this;

		/* Pin ourselves */
		gc_handle = GCHandle.Alloc (this, GCHandleType.Pinned);
	}

	public IntPtr Enter () {
		pendingException = null;
		localref_ptr = 0;
		return GetEnvPtr ();
	}

	public void Leave () {
		if (localref_ptr > 0) {
			for (int i = 0; i < localref_ptr; ++i)
				localrefs [i] = null;
			localref_ptr = 0;
		}
	}

	public unsafe IntPtr GetEnvPtr () {
		void *current;
		fixed (void *p = &func_table) {
			current = p;
		}
		
		return (IntPtr)current;
	}

	public unsafe static JNIEnv GetJniEnvFromEnvPtr (IntPtr ptr) {
		/* FIXME: */
		return (JNIEnv)ConvertToObject (*(IntPtr*)((long)ptr + 4));
	}

	public IntPtr MakeLocalRef (object o) {
#if COPYING_COLLECTOR
		//		Console.WriteLine ("MakeLocalRef.");

		if (o == null)
			return (IntPtr)0;

		localrefs [localref_ptr] = o;
		localref_ptr ++;
		if (localref_ptr == localrefs.Length) {
			object[] localrefs2 = new object [localrefs.Length * 2];
			System.Array.Copy (localrefs, localrefs2, localrefs.Length);
			localrefs = localrefs2;
		}
		return (IntPtr)(localref_ptr);
#else
		return ConvertToPtr (o);
#endif
	}

	public unsafe void DeleteLocalRef (IntPtr localRef) {
#if COPYING_COLLECTOR
		//		Console.WriteLine ("DeleteLocalRef.");

		int index = (int)localRef;
		localrefs [index - 1] = null;
#endif	   
	}

	public unsafe object UnwrapLocalRef (IntPtr localRef) {
#if COPYING_COLLECTOR
		//		Console.WriteLine ("UnwrapLocalRef.");

		int index = (int)localRef;
		if (index == 0)
			return null;
		else
			return localrefs [index - 1];
#else
		return ConvertToObject (localRef);
#endif
	}

	private unsafe static object ConvertToObject (IntPtr ptr) {
		PtrStruct s;
		s.ptr = ptr;
		s.localref = null;
		IntPtr *p = &s.ptr;
		p [1] = ptr;
		return s.localref;
	}		

	private unsafe static IntPtr ConvertToPtr (object o) {
		PtrStruct s;
		s.localref = o;
		IntPtr *p = &s.ptr;
		return p [1];
	}
}

[StructLayout (LayoutKind.Sequential)]
public unsafe struct IkvmJNIEnv {
	JNIEnv env;

	// This should be the first field in the structure
	IntPtr func_table;

#if COPYING_COLLECTOR	
	// Allocate most of the array in-line
	IntPtr localref0;
	object localref1;
	object localref2;
	object localref3;
	object localref4;
	object localref5;
	object localref6;
	object localref7;
	object localref8;
	object localref9;
	object localref10;
	object localref11;
	object localref12;
	object localref13;
	object localref14;
	object localref15;
	object localref16;

	object[] localrefs;

	int localref_ptr;
#endif

	[ThreadStatic]
	static JNIEnv current;

	void* previous;

	public static IntPtr the_func_table;

	static ArrayList globalRefs;

	static int t_count = 0;

	//
	// Called before entering a native method
	//
	public IntPtr Enter () {
		env = current;
		if (env == null) {
			env = new JNIEnv (the_func_table);
			current = env;
		}

		return env.Enter ();
	}

	//
	// Called after leaving a native method
	//
	public void Leave () {
		env.Leave ();
	}

	public IntPtr MakeLocalRef (object o) {
		return env.MakeLocalRef (o);
	}

	public unsafe object UnwrapLocalRef (IntPtr localRef) {
		return env.UnwrapLocalRef (localRef);
	}

	[DllImport("mono-ikvm-jni-native")]
		private static extern IntPtr mono_jni_get_func_table ();

	[DllImport("mono-ikvm-jni-native")]
		private static extern int mono_jni_jnienv_init (
			Delegate makelocalref_func,
			Delegate unwind_func,
			Delegate makeglobalref_func,
			Delegate deleteglobalref_func,
			Delegate getfieldcookie_func,
			Delegate getmethodcookie_func,
			Delegate setfieldvalue_func,
			Delegate getfieldvalue_func,
			Delegate getclassfromobject_func,
			Delegate exceptioncheck_func,
			Delegate getpendingexception_func,
			Delegate setpendingexception_func,
			Delegate invokemethod_func,
			Delegate getmethodarglist_func,
			Delegate findclass_func,
			Delegate getjnienv_func);

	[DllImport("mono-ikvm-jni-native")]
		public extern static void mono_jni_set_jnifunc (int index, Delegate func);

	static IkvmJNIEnv () {
		Console.WriteLine ("INIT");

		globalRefs = new ArrayList (); 

		mono_jni_jnienv_init (
			new MakeLocalRefDelegate (HelperMakeLocalRef),
			new UnwrapRefDelegate (HelperUnwrapRef),
			new MakeGlobalRefDelegate (MakeGlobalRef),
			new DeleteRefDelegate (DeleteRef),
			new GetFieldCookieDelegate (GetFieldCookie),
			new GetMethodCookieDelegate (GetMethodCookie),
			new SetFieldValueDelegate (SetFieldValue),
			new GetFieldValueDelegate (GetFieldValue),
			new GetClassFromObjectDelegate (GetClassFromObject),
			new ExceptionCheckDelegate (ExceptionCheck),
			new GetPendingExceptionDelegate (GetPendingException),
			new SetPendingExceptionDelegate (SetPendingException),
			new InvokeMethodDelegate (InvokeMethod),
			new GetMethodArgListDelegate (GetMethodArgList),
			new FindClassDelegate (FindClass),
			new GetJniEnvDelegate (GetJniEnv));

		the_func_table = mono_jni_get_func_table ();
	}

	private static object ConvertToObject (IntPtr ptr) {
		PtrStruct s;
		s.ptr = ptr;
		s.localref = null;
		IntPtr *p = &s.ptr;
		p [1] = ptr;
		return s.localref;
	}		

	private static IntPtr ConvertToPtr (object o) {
		PtrStruct s;
		s.localref = o;
		IntPtr *p = &s.ptr;
		return p [1];
	}

	/*
	// Return the address of the 'func_table' member from a boxed JNIEnv object
	// Implemented in IL
	private static IntPtr GetJniEnvPtr (object o) {
		return null;
	}
	*/

	//
	// Methods called by the native code
	//

	public delegate IntPtr MakeLocalRefDelegate (IntPtr jniEnv, object obj);

	public static unsafe IntPtr HelperMakeLocalRef (IntPtr jniEnv, object obj) {
#if COPYING_COLLECTOR
		return JNIEnv.GetJniEnvFromEnvPtr (jniEnv).MakeLocalRef (obj);
#else
		return ConvertToPtr (obj);
#endif
	}

	public delegate object UnwrapRefDelegate (IntPtr jniEnv, IntPtr reference);

	public static unsafe object HelperUnwrapRef (IntPtr jniEnv, IntPtr reference) {
#if COPYING_COLLECTOR
		if ((int)reference >= 0) 
			return JNIEnv.GetJniEnvFromEnvPtr (jniEnv).UnwrapLocalRef (reference);
		else
			return globalRefs [(- (int)reference) - 1];
#else
		return ConvertToObject (reference);
#endif
	}

	public delegate IntPtr MakeGlobalRefDelegate (object obj);

	public static IntPtr MakeGlobalRef (object obj) {
#if COPYING_COLLECTOR
		if (obj == null)
			return (IntPtr)0;
		else
			return (IntPtr)(- (globalRefs.Add (obj) + 1));
#else
		return ConvertToPtr (obj);
#endif
	}

	public delegate void DeleteRefDelegate (IntPtr jniEnv, IntPtr reference);

	public static unsafe void DeleteRef (IntPtr jniEnv, IntPtr reference) {
#if COPYING_COLLECTOR
		if ((int)reference == 0)
			return;
		else
			if ((int)reference > 0)
				JNIEnv.GetJniEnvFromEnvPtr (jniEnv).DeleteLocalRef (reference);
			else {
				int index = (- (int)reference) - 1;
				globalRefs [index] = null;
			}
#endif
	}

	public delegate IntPtr GetFieldCookieDelegate(object clazz, object name, object sig, bool isStatic);

	public static IntPtr GetFieldCookie(object clazz, object name, object sig, bool isStatic) {
		IntPtr res = JniHelper.GetFieldCookie (clazz, (string)name, (string)sig, isStatic);
		return res;
		/*
		if ((int)res == 0)
			throw new NotImplementedException ();
		else
			return res;
		*/
	}

    public delegate IntPtr GetMethodCookieDelegate (object clazz, object name, object sig, bool isStatic);

	public static IntPtr GetMethodCookie(object clazz, object name, object sig, bool isStatic) {
		IntPtr res = JniHelper.GetMethodCookie (clazz, (string)name, (string)sig, isStatic);
		return res;
		/*
		if ((int)res == 0)
			throw new NotImplementedException ();
		else
			return res;
		*/		
	}

	public delegate void SetFieldValueDelegate(IntPtr cookie, object obj, object val);

	public static void SetFieldValue (IntPtr cookie, object obj, object val) {
		JniHelper.SetFieldValue (cookie, obj, val);
	}

	public delegate object GetFieldValueDelegate (IntPtr cookie, object obj);

	public static object GetFieldValue (IntPtr cookie, object obj) {
		return JniHelper.GetFieldValue (cookie, obj);
	}

	public delegate object GetClassFromObjectDelegate (object obj);

	public static object GetClassFromObject (object obj) {
		return JniHelper.GetClassFromType (obj.GetType ());
	}

	public delegate bool ExceptionCheckDelegate (IntPtr jniEnv);

	public static unsafe bool ExceptionCheck (IntPtr jniEnv) {
		if (JNIEnv.GetJniEnvFromEnvPtr (jniEnv).pendingException == null) 
		    return false;
		else
		    return true;
	}

	public delegate object GetPendingExceptionDelegate (IntPtr jniEnv);

	public static unsafe object GetPendingException (IntPtr jniEnv) {
		return JNIEnv.GetJniEnvFromEnvPtr (jniEnv).pendingException;
	}

	public delegate void SetPendingExceptionDelegate (IntPtr jniEnv, object obj);

	public static unsafe void SetPendingException (IntPtr jniEnv, object obj) {
		JNIEnv.GetJniEnvFromEnvPtr (jniEnv).pendingException = obj;
	}

	public delegate object InvokeMethodDelegate (IntPtr jniEnv, IntPtr cookie, object obj, object args, bool nonVirtual);

	public static object InvokeMethod (IntPtr jniEnv, IntPtr cookie, object obj, object args, bool nonVirtual) {
		try {
			return JniHelper.InvokeMethod (cookie, obj, (object[])args, nonVirtual);
		}
		catch (Exception ex) {
			SetPendingException (jniEnv, ex);
			return null;
		}
	}

	public delegate string GetMethodArgListDelegate (IntPtr cookie);

	public static string GetMethodArgList (IntPtr cookie) {
		return JniHelper.GetMethodArgList (cookie);
	}

	public delegate object FindClassDelegate (object javaName);

	public static object FindClass (object javaName) {
		return JniHelper.FindClass ((string)javaName);
	}

	public unsafe delegate IntPtr GetJniEnvDelegate ();

	public static unsafe IntPtr GetJniEnv () {
		return current.GetEnvPtr ();
	}
}
