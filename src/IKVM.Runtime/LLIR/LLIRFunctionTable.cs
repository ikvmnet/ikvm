using System;
using System.Runtime.InteropServices;

using IKVM.Runtime.JNI.Memory;

namespace IKVM.Runtime.LLIR
{

    /// <summary>
    /// Maintains a set of LLIR function pointers.
    /// </summary>
    abstract class LLIRFunctionTable
    {

        static LLIRFunctionTable instance;

        /// <summary>
        /// Gets the instance of the function table based on the current platform.
        /// </summary>
        /// <returns></returns>
        public static LLIRFunctionTable Instance => instance ??= CreateInstance();

        /// <summary>
        /// Gets the instance of the function table based on the current platform.
        /// </summary>
        /// <returns></returns>
        static LLIRFunctionTable CreateInstance()
        {
#if FIRST_PASS
            return null;
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (RuntimeInformation.ProcessArchitecture == Architecture.X86)
                    return new LLIR_win7_x86();
                else if (RuntimeInformation.ProcessArchitecture == Architecture.X64)
                    return new LLIR_win7_x64();
                else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm)
                    return new LLIR_win81_arm();
                else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                    return new LLIR_win10_arm64();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (RuntimeInformation.ProcessArchitecture == Architecture.X64)
                    return new LLIR_linux_x64();
                else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm)
                    return new LLIR_linux_arm();
                else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                    return new LLIR_linux_arm64();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                if (RuntimeInformation.ProcessArchitecture == Architecture.X64)
                    return new LLIR_osx_x64();
            }

            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Allocates the given function code in executable memory and returns a reference.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        static ExecutableMemory AllocateMemory(byte[] code)
        {
            if (code is null)
                throw new ArgumentNullException(nameof(code));

            var m = ExecutableMemory.Allocate(code.Length);
            Marshal.Copy(code, 0, m.DangerousGetHandle(), m.Size);
            return m;
        }

        readonly ExecutableMemory memory;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public LLIRFunctionTable()
        {
            memory = AllocateMemory(Text);
        }

        /// <summary>
        /// Gets a pointer to the function at teh specified offset.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        IntPtr GetFunctionPointerFromOffset(int offset) => memory.DangerousGetHandle() + offset;

        #region Function Pointers

        public IntPtr JNI_NewObject => GetFunctionPointerFromOffset(idx_JNI_NewObject);

        public IntPtr JNI_NewObjectV => GetFunctionPointerFromOffset(idx_JNI_NewObjectV);

        public IntPtr JNI_CallObjectMethod => GetFunctionPointerFromOffset(idx_JNI_CallObjectMethod);

        public IntPtr JNI_CallObjectMethodV => GetFunctionPointerFromOffset(idx_JNI_CallObjectMethodV);

        public IntPtr JNI_CallNonvirtualObjectMethod => GetFunctionPointerFromOffset(idx_JNI_CallNonvirtualObjectMethod);

        public IntPtr JNI_CallNonvirtualObjectMethodV => GetFunctionPointerFromOffset(idx_JNI_CallNonvirtualObjectMethodV);

        public IntPtr JNI_CallStaticObjectMethod => GetFunctionPointerFromOffset(idx_JNI_CallStaticObjectMethod);

        public IntPtr JNI_CallStaticObjectMethodV => GetFunctionPointerFromOffset(idx_JNI_CallStaticObjectMethodV);

        public IntPtr JNI_CallBooleanMethod => GetFunctionPointerFromOffset(idx_JNI_CallBooleanMethod);

        public IntPtr JNI_CallBooleanMethodV => GetFunctionPointerFromOffset(idx_JNI_CallBooleanMethodV);

        public IntPtr JNI_CallNonvirtualBooleanMethod => GetFunctionPointerFromOffset(idx_JNI_CallNonvirtualBooleanMethod);

        public IntPtr JNI_CallNonvirtualBooleanMethodV => GetFunctionPointerFromOffset(idx_JNI_CallNonvirtualBooleanMethodV);

        public IntPtr JNI_CallStaticBooleanMethod => GetFunctionPointerFromOffset(idx_JNI_CallStaticBooleanMethod);

        public IntPtr JNI_CallStaticBooleanMethodV => GetFunctionPointerFromOffset(idx_JNI_CallStaticBooleanMethodV);

        public IntPtr JNI_CallByteMethod => GetFunctionPointerFromOffset(idx_JNI_CallByteMethod);

        public IntPtr JNI_CallByteMethodV => GetFunctionPointerFromOffset(idx_JNI_CallByteMethodV);

        public IntPtr JNI_CallNonvirtualByteMethod => GetFunctionPointerFromOffset(idx_JNI_CallNonvirtualByteMethod);

        public IntPtr JNI_CallNonvirtualByteMethodV => GetFunctionPointerFromOffset(idx_JNI_CallNonvirtualByteMethodV);

        public IntPtr JNI_CallStaticByteMethod => GetFunctionPointerFromOffset(idx_JNI_CallStaticByteMethod);

        public IntPtr JNI_CallStaticByteMethodV => GetFunctionPointerFromOffset(idx_JNI_CallStaticByteMethodV);

        public IntPtr JNI_CallCharMethod => GetFunctionPointerFromOffset(idx_JNI_CallCharMethod);

        public IntPtr JNI_CallCharMethodV => GetFunctionPointerFromOffset(idx_JNI_CallCharMethodV);

        public IntPtr JNI_CallNonvirtualCharMethod => GetFunctionPointerFromOffset(idx_JNI_CallNonvirtualCharMethod);

        public IntPtr JNI_CallNonvirtualCharMethodV => GetFunctionPointerFromOffset(idx_JNI_CallNonvirtualCharMethodV);

        public IntPtr JNI_CallStaticCharMethod => GetFunctionPointerFromOffset(idx_JNI_CallStaticCharMethod);

        public IntPtr JNI_CallStaticCharMethodV => GetFunctionPointerFromOffset(idx_JNI_CallStaticCharMethodV);

        public IntPtr JNI_CallShortMethod => GetFunctionPointerFromOffset(idx_JNI_CallShortMethod);

        public IntPtr JNI_CallShortMethodV => GetFunctionPointerFromOffset(idx_JNI_CallShortMethodV);

        public IntPtr JNI_CallNonvirtualShortMethod => GetFunctionPointerFromOffset(idx_JNI_CallNonvirtualShortMethod);

        public IntPtr JNI_CallNonvirtualShortMethodV => GetFunctionPointerFromOffset(idx_JNI_CallNonvirtualShortMethodV);

        public IntPtr JNI_CallStaticShortMethod => GetFunctionPointerFromOffset(idx_JNI_CallStaticShortMethod);

        public IntPtr JNI_CallStaticShortMethodV => GetFunctionPointerFromOffset(idx_JNI_CallStaticShortMethodV);

        public IntPtr JNI_CallIntMethod => GetFunctionPointerFromOffset(idx_JNI_CallIntMethod);

        public IntPtr JNI_CallIntMethodV => GetFunctionPointerFromOffset(idx_JNI_CallIntMethodV);

        public IntPtr JNI_CallNonvirtualIntMethod => GetFunctionPointerFromOffset(idx_JNI_CallNonvirtualIntMethod);

        public IntPtr JNI_CallNonvirtualIntMethodV => GetFunctionPointerFromOffset(idx_JNI_CallNonvirtualIntMethodV);

        public IntPtr JNI_CallStaticIntMethod => GetFunctionPointerFromOffset(idx_JNI_CallStaticIntMethod);

        public IntPtr JNI_CallStaticIntMethodV => GetFunctionPointerFromOffset(idx_JNI_CallStaticIntMethodV);

        public IntPtr JNI_CallLongMethod => GetFunctionPointerFromOffset(idx_JNI_CallLongMethod);

        public IntPtr JNI_CallLongMethodV => GetFunctionPointerFromOffset(idx_JNI_CallLongMethodV);

        public IntPtr JNI_CallNonvirtualLongMethod => GetFunctionPointerFromOffset(idx_JNI_CallNonvirtualLongMethod);

        public IntPtr JNI_CallNonvirtualLongMethodV => GetFunctionPointerFromOffset(idx_JNI_CallNonvirtualLongMethodV);

        public IntPtr JNI_CallStaticLongMethod => GetFunctionPointerFromOffset(idx_JNI_CallStaticLongMethod);

        public IntPtr JNI_CallStaticLongMethodV => GetFunctionPointerFromOffset(idx_JNI_CallStaticLongMethodV);

        public IntPtr JNI_CallFloatMethod => GetFunctionPointerFromOffset(idx_JNI_CallFloatMethod);

        public IntPtr JNI_CallFloatMethodV => GetFunctionPointerFromOffset(idx_JNI_CallFloatMethodV);

        public IntPtr JNI_CallNonvirtualFloatMethod => GetFunctionPointerFromOffset(idx_JNI_CallNonvirtualFloatMethod);

        public IntPtr JNI_CallNonvirtualFloatMethodV => GetFunctionPointerFromOffset(idx_JNI_CallNonvirtualFloatMethodV);

        public IntPtr JNI_CallStaticFloatMethod => GetFunctionPointerFromOffset(idx_JNI_CallStaticFloatMethod);

        public IntPtr JNI_CallStaticFloatMethodV => GetFunctionPointerFromOffset(idx_JNI_CallStaticFloatMethodV);

        public IntPtr JNI_CallDoubleMethod => GetFunctionPointerFromOffset(idx_JNI_CallDoubleMethod);

        public IntPtr JNI_CallDoubleMethodV => GetFunctionPointerFromOffset(idx_JNI_CallDoubleMethodV);

        public IntPtr JNI_CallNonvirtualDoubleMethod => GetFunctionPointerFromOffset(idx_JNI_CallNonvirtualDoubleMethod);

        public IntPtr JNI_CallNonvirtualDoubleMethodV => GetFunctionPointerFromOffset(idx_JNI_CallNonvirtualDoubleMethodV);

        public IntPtr JNI_CallStaticDoubleMethod => GetFunctionPointerFromOffset(idx_JNI_CallStaticDoubleMethod);

        public IntPtr JNI_CallStaticDoubleMethodV => GetFunctionPointerFromOffset(idx_JNI_CallStaticDoubleMethodV);

        public IntPtr JNI_CallVoidMethod => GetFunctionPointerFromOffset(idx_JNI_CallVoidMethod);

        public IntPtr JNI_CallVoidMethodV => GetFunctionPointerFromOffset(idx_JNI_CallVoidMethodV);

        public IntPtr JNI_CallNonvirtualVoidMethod => GetFunctionPointerFromOffset(idx_JNI_CallNonvirtualVoidMethod);

        public IntPtr JNI_CallNonvirtualVoidMethodV => GetFunctionPointerFromOffset(idx_JNI_CallNonvirtualVoidMethodV);

        public IntPtr JNI_CallStaticVoidMethod => GetFunctionPointerFromOffset(idx_JNI_CallStaticVoidMethod);

        public IntPtr JNI_CallStaticVoidMethodV => GetFunctionPointerFromOffset(idx_JNI_CallStaticVoidMethodV);

        #endregion

        /// <summary>
        /// Overridden to supply the LLIR generated .text for the platform.
        /// </summary>
        protected abstract byte[] Text { get; }

        #region Function Indexes

        protected abstract int idx_JNI_CallObjectMethod { get; }

        protected abstract int idx_JNI_CallObjectMethodV { get; }

        protected abstract int idx_JNI_CallNonvirtualObjectMethod { get; }

        protected abstract int idx_JNI_CallNonvirtualObjectMethodV { get; }

        protected abstract int idx_JNI_CallStaticObjectMethod { get; }

        protected abstract int idx_JNI_CallStaticObjectMethodV { get; }

        protected abstract int idx_JNI_CallBooleanMethod { get; }

        protected abstract int idx_JNI_CallBooleanMethodV { get; }

        protected abstract int idx_JNI_CallNonvirtualBooleanMethod { get; }

        protected abstract int idx_JNI_CallNonvirtualBooleanMethodV { get; }

        protected abstract int idx_JNI_CallStaticBooleanMethod { get; }

        protected abstract int idx_JNI_CallStaticBooleanMethodV { get; }

        protected abstract int idx_JNI_CallByteMethod { get; }

        protected abstract int idx_JNI_CallByteMethodV { get; }

        protected abstract int idx_JNI_CallNonvirtualByteMethod { get; }

        protected abstract int idx_JNI_CallNonvirtualByteMethodV { get; }

        protected abstract int idx_JNI_CallStaticByteMethod { get; }

        protected abstract int idx_JNI_CallStaticByteMethodV { get; }

        protected abstract int idx_JNI_CallCharMethod { get; }

        protected abstract int idx_JNI_CallCharMethodV { get; }

        protected abstract int idx_JNI_CallNonvirtualCharMethod { get; }

        protected abstract int idx_JNI_CallNonvirtualCharMethodV { get; }

        protected abstract int idx_JNI_CallStaticCharMethod { get; }

        protected abstract int idx_JNI_CallStaticCharMethodV { get; }

        protected abstract int idx_JNI_CallShortMethod { get; }

        protected abstract int idx_JNI_CallShortMethodV { get; }

        protected abstract int idx_JNI_CallNonvirtualShortMethod { get; }

        protected abstract int idx_JNI_CallNonvirtualShortMethodV { get; }

        protected abstract int idx_JNI_CallStaticShortMethod { get; }

        protected abstract int idx_JNI_CallStaticShortMethodV { get; }

        protected abstract int idx_JNI_CallIntMethod { get; }

        protected abstract int idx_JNI_CallIntMethodV { get; }

        protected abstract int idx_JNI_CallNonvirtualIntMethod { get; }

        protected abstract int idx_JNI_CallNonvirtualIntMethodV { get; }

        protected abstract int idx_JNI_CallStaticIntMethod { get; }

        protected abstract int idx_JNI_CallStaticIntMethodV { get; }

        protected abstract int idx_JNI_CallLongMethod { get; }

        protected abstract int idx_JNI_CallLongMethodV { get; }

        protected abstract int idx_JNI_CallNonvirtualLongMethod { get; }

        protected abstract int idx_JNI_CallNonvirtualLongMethodV { get; }

        protected abstract int idx_JNI_CallStaticLongMethod { get; }

        protected abstract int idx_JNI_CallStaticLongMethodV { get; }

        protected abstract int idx_JNI_CallFloatMethod { get; }

        protected abstract int idx_JNI_CallFloatMethodV { get; }

        protected abstract int idx_JNI_CallNonvirtualFloatMethod { get; }

        protected abstract int idx_JNI_CallNonvirtualFloatMethodV { get; }

        protected abstract int idx_JNI_CallStaticFloatMethod { get; }

        protected abstract int idx_JNI_CallStaticFloatMethodV { get; }

        protected abstract int idx_JNI_CallDoubleMethod { get; }

        protected abstract int idx_JNI_CallDoubleMethodV { get; }

        protected abstract int idx_JNI_CallNonvirtualDoubleMethod { get; }

        protected abstract int idx_JNI_CallNonvirtualDoubleMethodV { get; }

        protected abstract int idx_JNI_CallStaticDoubleMethod { get; }

        protected abstract int idx_JNI_CallStaticDoubleMethodV { get; }

        protected abstract int idx_JNI_NewObject { get; }

        protected abstract int idx_JNI_NewObjectV { get; }

        protected abstract int idx_JNI_CallVoidMethod { get; }

        protected abstract int idx_JNI_CallVoidMethodV { get; }

        protected abstract int idx_JNI_CallNonvirtualVoidMethod { get; }

        protected abstract int idx_JNI_CallNonvirtualVoidMethodV { get; }

        protected abstract int idx_JNI_CallStaticVoidMethod { get; }

        protected abstract int idx_JNI_CallStaticVoidMethodV { get; }

        #endregion

    }

}
