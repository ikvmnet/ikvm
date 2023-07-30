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

namespace IKVM.Runtime.JNI
{

    public unsafe struct JNIFrame
    {

        JNIEnv.ManagedJNIEnv env;
        JNIEnv.ManagedJNIEnv.FrameState prevFrameState;

        /// <summary>
        /// Enters the <see cref="JNIFrame"/> with a specified <see cref="RuntimeClassLoader"/> in scope.
        /// </summary>
        /// <param name="loader"></param>
        /// <returns></returns>
        internal RuntimeClassLoader Enter(RuntimeClassLoader loader)
        {
            Enter((ikvm.@internal.CallerID)null);
            RuntimeClassLoader prev = env.classLoader;
            env.classLoader = loader;
            return prev;
        }

        /// <summary>
        /// Leaves a <see cref="JNIFrame"/> previously entered.
        /// </summary>
        internal void Leave(RuntimeClassLoader prev)
        {
            env.classLoader = prev;
            Leave();
        }

        /// <summary>
        /// Enters the <see cref="JNIFrame"/> with a specified <see cref="ikvm.@internal.CallerID"/> in scope.
        /// </summary>
        /// <param name="callerID"></param>
        /// <returns></returns>
        public IntPtr Enter(ikvm.@internal.CallerID callerID)
        {
#if FIRST_PASS
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
            var x = env.Leave(prevFrameState);
            if (x != null)
                throw x;
        }

        public static IntPtr GetFuncPtr(ikvm.@internal.CallerID callerID, string clazz, string name, string sig)
        {
            var loader = RuntimeClassLoader.FromCallerID(callerID);
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

            var mangledClass = JniMangle(clazz);
            var mangledName = JniMangle(name);
            var mangledSig = JniMangle(sig.Substring(1, sig.IndexOf(')') - 1));
            var shortMethodName = $"Java_{mangledClass}_{mangledName}";
            var longMethodName = $"Java_{mangledClass}_{mangledName}__{mangledSig}";
            Tracer.Info(Tracer.Jni, "Linking native method: {0}.{1}{2}, class loader = {3}, short = {4}, long = {5}, args = {6}", clazz, name, sig, loader, shortMethodName, longMethodName, sp + 2 * IntPtr.Size);

            lock (JNINativeLoader.SyncRoot)
            {
                foreach (var p in loader.GetNativeLibraries())
                {
                    var pfunc = NativeLibrary.GetExport(p, shortMethodName, sp + 2 * IntPtr.Size);
                    if (pfunc != IntPtr.Zero)
                    {
                        Tracer.Info(Tracer.Jni, "Native method {0}.{1}{2} found in library 0x{3:X} (short)", clazz, name, sig, p);
                        return pfunc;
                    }
                    pfunc = NativeLibrary.GetExport(p, longMethodName, sp + 2 * IntPtr.Size);
                    if (pfunc != IntPtr.Zero)
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
