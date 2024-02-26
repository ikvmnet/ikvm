using System;
using System.Runtime.InteropServices;

using IKVM.Runtime.JNI;

namespace IKVM.Java.Externs.java.lang
{

    static class UNIXProcessEnvironment
    {

#if FIRST_PASS == false

        static global::ikvm.@internal.CallerID __callerID;
        delegate IntPtr __jniDelegate__environ(IntPtr jniEnv);
        static __jniDelegate__environ __jniPtr__environ;

#endif

        /// <summary>
        /// Implements the native method 'environ'.
        /// </summary>
        /// <returns></returns>
        public static object environ()
        {
#if FIRST_PASS 
            throw new NotImplementedException();
#else
            __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.lang.UNIXProcessEnvironment).TypeHandle);
            __jniPtr__environ ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__environ>(JNIFrame.GetFuncPtr(__callerID, "java/lang/ProcessEnvironment", nameof(environ), "()[[B"));
            var jniFrm = new JNIFrame();
            var jniEnv = jniFrm.Enter(__callerID);
            try
            {
                return jniFrm.UnwrapLocalRef(__jniPtr__environ(jniEnv));
            }
            catch (Exception ex)
            {
                global::System.Console.WriteLine("*** exception in native code ***");
                global::System.Console.WriteLine(ex);
                throw;
            }
            finally
            {
                jniFrm.Leave();
            }
#endif
        }

    }

}
