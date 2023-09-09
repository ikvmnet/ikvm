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
using System.Diagnostics;
using System.Text;

using IKVM.Internal;

using jarray = System.IntPtr;
using jboolean = System.SByte;
using jbooleanArray = System.IntPtr;
using jbyte = System.SByte;
using jbyteArray = System.IntPtr;
using jchar = System.UInt16;
using jcharArray = System.IntPtr;
using jclass = System.IntPtr;
using jdouble = System.Double;
using jdoubleArray = System.IntPtr;
using jfieldID = System.IntPtr;
using jfloat = System.Single;
using jfloatArray = System.IntPtr;
using jint = System.Int32;
using jintArray = System.IntPtr;
using jlong = System.Int64;
using jlongArray = System.IntPtr;
using jmethodID = System.IntPtr;
using jobject = System.IntPtr;
using jobjectArray = System.IntPtr;
using jshort = System.Int16;
using jshortArray = System.IntPtr;
using jsize = System.Int32;
using jstring = System.IntPtr;
using jthrowable = System.IntPtr;
using jweak = System.IntPtr;

namespace IKVM.Runtime.JNI
{

    public unsafe struct JNIFrame
    {

        JNIEnv.ManagedJNIEnv env;
        JNIEnv.ManagedJNIEnv.FrameState prevFrameState;

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
            if (env == null)
            {
                env = JNIEnv.CreateJNIEnv()->GetManagedJNIEnv();
            }
            prevFrameState = env.Enter(callerID);
            return (IntPtr)(void*)env.pJNIEnv;
        }

        public void Leave()
        {
            Exception x = env.Leave(prevFrameState);
            if (x != null)
            {
                throw x;
            }
        }

        public static nint GetFuncPtr(ikvm.@internal.CallerID callerID, string clazz, string name, string sig)
        {
            var loader = ClassLoaderWrapper.FromCallerID(callerID);
            int sp = 0;
            for (int i = 1; sig[i] != ')'; i++)
            {
                switch (sig[i])
                {
                    case '[':
                        sp += sizeof(jarray);
                        while (sig[++i] == '[') ;
                        if (sig[i] == 'L')
                            while (sig[++i] != ';') ;
                        break;
                    case 'L':
                        sp += sizeof(jobject);
                        while (sig[++i] != ';') ;
                        break;
                    case 'J':
                        sp += sizeof(jlong);
                        break;
                    case 'D':
                        sp += sizeof(jdouble);
                        break;
                    case 'F':
                        sp += sizeof(jfloat);
                        break;
                    case 'I':
                        sp += sizeof(jint);
                        break;
                    case 'C':
                        sp += sizeof(jchar);
                        break;
                    case 'Z':
                        sp += sizeof(jboolean);
                        break;
                    case 'S':
                        sp += sizeof(jshort);
                        break;
                    case 'B':
                        sp += sizeof(jbyte);
                        break;
                    default:
                        throw new InternalException("Invalid JNI method signature.");
                }
            }

            string mangledClass = JniMangle(clazz);
            string mangledName = JniMangle(name);
            string mangledSig = JniMangle(sig.Substring(1, sig.IndexOf(')') - 1));
            string shortMethodName = $"Java_{mangledClass}_{mangledName}";
            string longMethodName = $"Java_{mangledClass}_{mangledName}__{mangledSig}";
            Tracer.Info(Tracer.Jni, "Linking native method: {0}.{1}{2}, class loader = {3}, short = {4}, long = {5}, args = {6}", clazz, name, sig, loader, shortMethodName, longMethodName, sp + sizeof(nint) + sizeof(nint));

            lock (JNINativeLoader.SyncRoot)
            {
                foreach (var p in loader.GetNativeLibraries())
                {
                    var pfunc = NativeLibrary.GetExport(p, shortMethodName, sp + sizeof(nint) + sizeof(nint));
                    if (pfunc != 0)
                    {
                        Tracer.Info(Tracer.Jni, "Native method {0}.{1}{2} found in library 0x{3:X} (short)", clazz, name, sig, p);
                        return pfunc;
                    }
                    pfunc = NativeLibrary.GetExport(p, longMethodName, sp + sizeof(nint) + sizeof(nint));
                    if (pfunc != 0)
                    {
                        Tracer.Info(Tracer.Jni, "Native method {0}.{1}{2} found in library 0x{3:X} (long)", clazz, name, sig, p);
                        return pfunc;
                    }
                }
            }

            var msg = $"{clazz}.{name}{sig}";
            Tracer.Error(Tracer.Jni, "UnsatisfiedLinkError: {0}", msg);
            throw new java.lang.UnsatisfiedLinkError(msg);
        }

        static string JniMangle(string name)
        {
            var sb = new StringBuilder();

            foreach (var c in name)
            {
                switch (c)
                {
                    case '/':
                        sb.Append('_');
                        break;
                    case '_':
                        sb.Append("_1");
                        break;
                    case ';':
                        sb.Append("_2");
                        break;
                    case '[':
                        sb.Append("_3");
                        break;
                    case >= '0' and <= '9':
                    case >= 'a' and <= 'z':
                    case >= 'A' and <= 'Z':
                        sb.Append(c);
                        break;
                    default:
                        sb.Append($"_0{(int)c:x4}");
                        break;
                }
            }

            return sb.ToString();
        }

        public nint MakeLocalRef(object obj)
        {
            return env.MakeLocalRef(obj);
        }

        public object UnwrapLocalRef(nint p)
        {
            return JNIEnv.UnwrapRef(env, p);
        }

    }

}
