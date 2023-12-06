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

using IKVM.Runtime.Accessors.Java.Lang;

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

        /// <summary>
        /// Invoked by managed code to acquire a function pointer to a native JNI method.
        /// </summary>
        /// <param name="callerID"></param>
        /// <param name="clazz"></param>
        /// <param name="name"></param>
        /// <param name="sig"></param>
        /// <returns></returns>
        /// <exception cref="java.lang.UnsatisfiedLinkError"></exception>
        public static nint GetFuncPtr(ikvm.@internal.CallerID callerID, string clazz, string name, string sig)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            var loader = RuntimeClassLoader.FromCallerID(callerID);
            int sp = 0;
            for (int i = 1; sig[i] != ')'; i++)
            {
                switch (sig[i])
                {
                    case '[':
                        sp += sizeof(nint);
                        while (sig[++i] == '[') ;
                        if (sig[i] == 'L')
                        {
                            while (sig[++i] != ';') ;
                        }
                        break;
                    case 'L':
                        sp += sizeof(nint);
                        while (sig[++i] != ';') ;
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

            var mangledClass = JniMangle(clazz);
            var mangledName = JniMangle(name);
            var mangledSig = JniMangle(sig.Substring(1, sig.IndexOf(')') - 1));
            var methodName = $"Java_{mangledClass}_{mangledName}";
            var longMethodName = $"Java_{mangledClass}_{mangledName}__{mangledSig}";
            Tracer.Info(Tracer.Jni, "Linking native method: {0}.{1}{2}, class loader = {3}, short = {4}, long = {5}, args = {6}", clazz, name, sig, loader, methodName, longMethodName, sp + sizeof(nint) + sizeof(nint));

            lock (JNINativeLoader.SyncRoot)
            {
                foreach (var p in loader.GetNativeLibraries())
                {
                    if (LibJvm.Instance.JVM_FindLibraryEntry(p, NativeLibrary.MangleExportName(methodName, sp + sizeof(nint) + sizeof(nint))) is nint h1 and not 0)
                    {
                        Tracer.Info(Tracer.Jni, "Native method {0}.{1}{2} found in library 0x{3:X} (short)", clazz, name, sig, p);
                        return h1;
                    }

                    if (LibJvm.Instance.JVM_FindLibraryEntry(p, NativeLibrary.MangleExportName(longMethodName, sp + sizeof(nint) + sizeof(nint))) is nint h2 and not 0)
                    {
                        Tracer.Info(Tracer.Jni, "Native method {0}.{1}{2} found in library 0x{3:X} (long)", clazz, name, sig, p);
                        return h2;
                    }
                }
            }

            var msg = $"{clazz}.{name}{sig}";
            Tracer.Error(Tracer.Jni, "UnsatisfiedLinkError: {0}", msg);
            throw new java.lang.UnsatisfiedLinkError(msg);
#endif
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

#if FIRST_PASS || IMPORTER || EXPORTER
#else

        JNIEnv.ManagedJNIEnv env;
        JNIEnv.ManagedJNIEnv.FrameState prevFrameState;

#endif

        /// <summary>
        /// Enters the <see cref="JNIFrame"/> with a specified <see cref="RuntimeClassLoader"/> in scope.
        /// </summary>
        /// <param name="loader"></param>
        /// <returns></returns>
        internal RuntimeClassLoader Enter(RuntimeClassLoader loader)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            Enter((ikvm.@internal.CallerID)null);
            RuntimeClassLoader prev = env.classLoader;
            env.classLoader = loader;
            return prev;
#endif
        }

        /// <summary>
        /// Leaves a <see cref="JNIFrame"/> previously entered.
        /// </summary>
        internal void Leave(RuntimeClassLoader prev)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            env.classLoader = prev;
            Leave();
#endif
        }

        /// <summary>
        /// Enters the <see cref="JNIFrame"/> with a specified <see cref="ikvm.@internal.CallerID"/> in scope.
        /// </summary>
        /// <param name="callerID"></param>
        /// <returns></returns>
        public IntPtr Enter(ikvm.@internal.CallerID callerID)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            env = TlsHack.ManagedJNIEnv;
            env ??= JNIEnv.CreateJNIEnv(JVM.Context)->GetManagedJNIEnv();
            prevFrameState = env.Enter(callerID);
            return (IntPtr)(void*)env.pJNIEnv;
#endif
        }

        /// <summary>
        /// Leaves a <see cref="JNIFrame"/> previously entered.
        /// </summary>
        public void Leave()
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            var x = env.Leave(prevFrameState);
            if (x != null)
                throw x;
#endif
        }

        public nint MakeLocalRef(object obj)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            return env.MakeLocalRef(obj);
#endif
        }

        public object UnwrapLocalRef(nint p)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            return JNIEnv.UnwrapRef(env, p);
#endif
        }

    }

}
