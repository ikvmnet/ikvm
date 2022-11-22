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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using IKVM.Internal;

namespace IKVM.Runtime
{

    public sealed unsafe class JNI
    {

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

            public static IntPtr GetFuncPtr(ikvm.@internal.CallerID callerID, string clazz, string name, string sig)
            {
                ClassLoaderWrapper loader = ClassLoaderWrapper.FromCallerID(callerID);
                int sp = 0;
                for (int i = 1; sig[i] != ')'; i++)
                {
                    switch (sig[i])
                    {
                        case '[':
                            sp += IntPtr.Size;
                            while (sig[++i] == '[') ;
                            if (sig[i] == 'L')
                            {
                                while (sig[++i] != ';') ;
                            }
                            break;
                        case 'L':
                            sp += IntPtr.Size;
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
                string mangledClass = JniMangle(clazz);
                string mangledName = JniMangle(name);
                string mangledSig = JniMangle(sig.Substring(1, sig.IndexOf(')') - 1));
                string shortMethodName = $"Java_{mangledClass}_{mangledName}";
                string longMethodName = $"Java_{mangledClass}_{mangledName}__{mangledSig}";
                Tracer.Info(Tracer.Jni, "Linking native method: {0}.{1}{2}, class loader = {3}, short = {4}, long = {5}, args = {6}", clazz, name, sig, loader, shortMethodName, longMethodName, sp + 2 * IntPtr.Size);

                lock (JniHelper.JniLock)
                {
                    foreach (var p in loader.GetNativeLibraries())
                    {
                        var pfunc = JniNativeLibrary.GetExport(p, shortMethodName, sp + 2 * IntPtr.Size);
                        if (pfunc != IntPtr.Zero)
                        {
                            Tracer.Info(Tracer.Jni, "Native method {0}.{1}{2} found in library 0x{3:X} (short)", clazz, name, sig, p.ToInt64());
                            return pfunc;
                        }
                        pfunc = JniNativeLibrary.GetExport(p, longMethodName, sp + 2 * IntPtr.Size);
                        if (pfunc != IntPtr.Zero)
                        {
                            Tracer.Info(Tracer.Jni, "Native method {0}.{1}{2} found in library 0x{3:X} (long)", clazz, name, sig, p.ToInt64());
                            return pfunc;
                        }
                    }
                }

                var msg = $"{clazz}.{name}{sig}";
                Tracer.Error(Tracer.Jni, "UnsatisfiedLinkError: {0}", msg);
                throw new java.lang.UnsatisfiedLinkError(msg);
            }

            private static string JniMangle(string name)
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

            // NOTE this method has the wrong name, it should unwrap *any* jobject reference type (local and global)
            public object UnwrapLocalRef(IntPtr p)
            {
                return JNIEnv.UnwrapRef(env, p);
            }

        }

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
            if (!IsSupportedJniVersion(pInitArgs->version) || pInitArgs->version == JNIEnv.JNI_VERSION_1_1)
                return JNIEnv.JNI_EVERSION;
            if (jvmCreated)
                return JNIEnv.JNI_ERR;

            var properties = new Dictionary<string, string>();
            for (int i = 0; i < pInitArgs->nOptions; i++)
            {
                var option = JNIEnv.StringFromOEM(pInitArgs->options[i].optionString);
                if (option.StartsWith("-D"))
                {
                    var idx = option.IndexOf('=', 2);
                    properties[option.Substring(2, idx - 2)] = option.Substring(idx + 1);
                }
                else if (option.StartsWith("-verbose"))
                {
                    // ignore
                }
                else if (option == "vfprintf" || option == "exit" || option == "abort")
                {
                    // not supported
                }
                else if (pInitArgs->ignoreUnrecognized == JNIEnv.JNI_FALSE)
                {
                    return JNIEnv.JNI_ERR;
                }
            }

            // set properties to be imported
            IKVM.Java.Externs.java.lang.VMSystemProperties.ImportProperties = properties;

            // initialize the class library
            java.lang.Thread.currentThread();

            *(void**)ppvm = JavaVM.pJavaVM;
            return JavaVM.AttachCurrentThread(JavaVM.pJavaVM, (void**)ppenv, null);
        }

        public static int GetDefaultJavaVMInitArgs(void* vm_args)
        {
            // This is only used for JDK 1.1 JavaVMInitArgs, and we don't support those.
            return JNIEnv.JNI_ERR;
        }

        public static int GetCreatedJavaVMs(void* ppvmBuf, int bufLen, int* nVMs)
        {
            if (jvmCreated)
            {
                if (bufLen >= 1)
                    *((void**)ppvmBuf) = JavaVM.pJavaVM;

                if (nVMs != null)
                    *nVMs = 1;
            }
            else if (nVMs != null)
                *nVMs = 0;

            return JNIEnv.JNI_OK;
        }

    }

}
