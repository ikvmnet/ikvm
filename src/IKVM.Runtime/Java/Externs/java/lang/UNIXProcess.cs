using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Lang;
using IKVM.Runtime.JNI;

namespace IKVM.Java.Externs.java.lang
{

    static class UNIXProcess
    {

#if FIRST_PASS == false

        static UNIXProcessAccessor unixProcessAccessor;

        static UNIXProcessAccessor UNIXProcessAccessor => JVM.Internal.BaseAccessors.Get(ref unixProcessAccessor);

        static global::ikvm.@internal.CallerID __callerID;
        delegate void __jniDelegate__init(IntPtr jniEnv, IntPtr clazz);
        static __jniDelegate__init __jniPtr__init;

#endif

        /// <summary>
        /// Implements the native method 'init'.
        /// </summary>
        /// <remarks>
        /// We save and load the old SIGCHLD handler between calls to prevent Java from overwriting the .NET handler.
        /// This isn't thread safe but should suffice since it's limited to static initialization.
        /// </remarks>
        /// <returns></returns>
        public static unsafe void init()
        {
#if FIRST_PASS 
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return;

            // get size of sigaction structure and allocate space
            var size = LibIkvm.Instance.sig_get_size_sigaction();
            Debug.Assert(size > 0);
            var save = stackalloc byte[size];

            // save old SIGCHLD structure
            if (LibIkvm.Instance.sig_get_chld_action(save) != 0)
                throw new InternalException("Could not save SIGCHLD handler.");

            try
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(UNIXProcessAccessor.Type.TypeHandle);
                __jniPtr__init ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__init>(JNIFrame.GetFuncPtr(__callerID, "java/lang/UNIXProcess", nameof(init), "()V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__init(jniEnv, jniFrm.MakeLocalRef(typeof(ClassLiteral<>).MakeGenericType(UNIXProcessAccessor.Type)));
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
            }
            finally
            {
                // restore old SIGCHLD structure
                if (LibIkvm.Instance.sig_set_chld_action(save) != 0)
                    throw new InternalException("Could not restore SIGCHLD handler.");
            }
#endif
        }

    }

}