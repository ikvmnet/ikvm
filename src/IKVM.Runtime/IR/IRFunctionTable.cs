using System;
using System.Runtime.InteropServices;

using IKVM.Runtime.JNI.Memory;

namespace IKVM.Runtime.IR
{

    /// <summary>
    /// Maintains a set of IR function pointers.
    /// </summary>
    abstract class IRFunctionTable
    {

        static IRFunctionTable instance;

        /// <summary>
        /// Gets the instance of the function table based on the current platform.
        /// </summary>
        /// <returns></returns>
        public static IRFunctionTable Instance => instance ??= CreateInstance();

        /// <summary>
        /// Gets the instance of the function table based on the current platform.
        /// </summary>
        /// <returns></returns>
        static IRFunctionTable CreateInstance()
        {
#if STATIC_COMPILER || STUB_GENERATOR
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (RuntimeInformation.ProcessArchitecture == Architecture.X86)
                    return new IR_win7_x86();
                else if (RuntimeInformation.ProcessArchitecture == Architecture.X64)
                    return new IR_win7_x64();
                //else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm)
                //    return new IR_win81_arm();
                else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                    return new IR_win10_arm64();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (RuntimeInformation.ProcessArchitecture == Architecture.X64)
                    return new IR_linux_x64();
                //else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm)
                //    return new IR_linux_arm();
                else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                    return new IR_linux_arm64();
            }

            throw new PlatformNotSupportedException();
#endif
        }

        readonly ExecutableMemory _NewObjectRef;
        readonly ExecutableMemory _NewObjectVRef;
        readonly ExecutableMemory _CallObjectMethodRef;
        readonly ExecutableMemory _CallObjectMethodVRef;
        readonly ExecutableMemory _CallNonvirtualObjectMethodRef;
        readonly ExecutableMemory _CallNonvirtualObjectMethodVRef;
        readonly ExecutableMemory _CallStaticObjectMethodRef;
        readonly ExecutableMemory _CallStaticObjectMethodVRef;
        readonly ExecutableMemory _CallBooleanMethodRef;
        readonly ExecutableMemory _CallBooleanMethodVRef;
        readonly ExecutableMemory _CallNonvirtualBooleanMethodRef;
        readonly ExecutableMemory _CallNonvirtualBooleanMethodVRef;
        readonly ExecutableMemory _CallStaticBooleanMethodRef;
        readonly ExecutableMemory _CallStaticBooleanMethodVRef;
        readonly ExecutableMemory _CallByteMethodRef;
        readonly ExecutableMemory _CallByteMethodVRef;
        readonly ExecutableMemory _CallNonvirtualByteMethodRef;
        readonly ExecutableMemory _CallNonvirtualByteMethodVRef;
        readonly ExecutableMemory _CallStaticByteMethodRef;
        readonly ExecutableMemory _CallStaticByteMethodVRef;
        readonly ExecutableMemory _CallCharMethodRef;
        readonly ExecutableMemory _CallCharMethodVRef;
        readonly ExecutableMemory _CallNonvirtualCharMethodRef;
        readonly ExecutableMemory _CallNonvirtualCharMethodVRef;
        readonly ExecutableMemory _CallStaticCharMethodRef;
        readonly ExecutableMemory _CallStaticCharMethodVRef;
        readonly ExecutableMemory _CallShortMethodRef;
        readonly ExecutableMemory _CallShortMethodVRef;
        readonly ExecutableMemory _CallNonvirtualShortMethodRef;
        readonly ExecutableMemory _CallNonvirtualShortMethodVRef;
        readonly ExecutableMemory _CallStaticShortMethodRef;
        readonly ExecutableMemory _CallStaticShortMethodVRef;
        readonly ExecutableMemory _CallIntMethodRef;
        readonly ExecutableMemory _CallIntMethodVRef;
        readonly ExecutableMemory _CallNonvirtualIntMethodRef;
        readonly ExecutableMemory _CallNonvirtualIntMethodVRef;
        readonly ExecutableMemory _CallStaticIntMethodRef;
        readonly ExecutableMemory _CallStaticIntMethodVRef;
        readonly ExecutableMemory _CallLongMethodRef;
        readonly ExecutableMemory _CallLongMethodVRef;
        readonly ExecutableMemory _CallNonvirtualLongMethodRef;
        readonly ExecutableMemory _CallNonvirtualLongMethodVRef;
        readonly ExecutableMemory _CallStaticLongMethodRef;
        readonly ExecutableMemory _CallStaticLongMethodVRef;
        readonly ExecutableMemory _CallFloatMethodRef;
        readonly ExecutableMemory _CallFloatMethodVRef;
        readonly ExecutableMemory _CallNonvirtualFloatMethodRef;
        readonly ExecutableMemory _CallNonvirtualFloatMethodVRef;
        readonly ExecutableMemory _CallStaticFloatMethodRef;
        readonly ExecutableMemory _CallStaticFloatMethodVRef;
        readonly ExecutableMemory _CallDoubleMethodRef;
        readonly ExecutableMemory _CallDoubleMethodVRef;
        readonly ExecutableMemory _CallNonvirtualDoubleMethodRef;
        readonly ExecutableMemory _CallNonvirtualDoubleMethodVRef;
        readonly ExecutableMemory _CallStaticDoubleMethodRef;
        readonly ExecutableMemory _CallStaticDoubleMethodVRef;
        readonly ExecutableMemory _CallVoidMethodRef;
        readonly ExecutableMemory _CallVoidMethodVRef;
        readonly ExecutableMemory _CallNonvirtualVoidMethodRef;
        readonly ExecutableMemory _CallNonvirtualVoidMethodVRef;
        readonly ExecutableMemory _CallStaticVoidMethodRef;
        readonly ExecutableMemory _CallStaticVoidMethodVRef;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IRFunctionTable()
        {
            _NewObjectRef = AllocateFunctionCode(JNI_NewObject);
            _NewObjectVRef = AllocateFunctionCode(JNI_NewObjectV);
            _CallObjectMethodRef = AllocateFunctionCode(JNI_CallObjectMethod);
            _CallObjectMethodVRef = AllocateFunctionCode(JNI_CallObjectMethodV);
            _CallNonvirtualObjectMethodRef = AllocateFunctionCode(JNI_CallNonvirtualObjectMethod);
            _CallNonvirtualObjectMethodVRef = AllocateFunctionCode(JNI_CallNonvirtualObjectMethodV);
            _CallStaticObjectMethodRef = AllocateFunctionCode(JNI_CallStaticObjectMethod);
            _CallStaticObjectMethodVRef = AllocateFunctionCode(JNI_CallStaticObjectMethodV);
            _CallBooleanMethodRef = AllocateFunctionCode(JNI_CallBooleanMethod);
            _CallBooleanMethodVRef = AllocateFunctionCode(JNI_CallBooleanMethodV);
            _CallNonvirtualBooleanMethodRef = AllocateFunctionCode(JNI_CallNonvirtualBooleanMethod);
            _CallNonvirtualBooleanMethodVRef = AllocateFunctionCode(JNI_CallNonvirtualBooleanMethodV);
            _CallStaticBooleanMethodRef = AllocateFunctionCode(JNI_CallStaticBooleanMethod);
            _CallStaticBooleanMethodVRef = AllocateFunctionCode(JNI_CallStaticBooleanMethodV);
            _CallByteMethodRef = AllocateFunctionCode(JNI_CallByteMethod);
            _CallByteMethodVRef = AllocateFunctionCode(JNI_CallByteMethodV);
            _CallNonvirtualByteMethodRef = AllocateFunctionCode(JNI_CallNonvirtualByteMethod);
            _CallNonvirtualByteMethodVRef = AllocateFunctionCode(JNI_CallNonvirtualByteMethodV);
            _CallStaticByteMethodRef = AllocateFunctionCode(JNI_CallStaticByteMethod);
            _CallStaticByteMethodVRef = AllocateFunctionCode(JNI_CallStaticByteMethodV);
            _CallCharMethodRef = AllocateFunctionCode(JNI_CallCharMethod);
            _CallCharMethodVRef = AllocateFunctionCode(JNI_CallCharMethodV);
            _CallNonvirtualCharMethodRef = AllocateFunctionCode(JNI_CallNonvirtualCharMethod);
            _CallNonvirtualCharMethodVRef = AllocateFunctionCode(JNI_CallNonvirtualCharMethodV);
            _CallStaticCharMethodRef = AllocateFunctionCode(JNI_CallStaticCharMethod);
            _CallStaticCharMethodVRef = AllocateFunctionCode(JNI_CallStaticCharMethodV);
            _CallShortMethodRef = AllocateFunctionCode(JNI_CallShortMethod);
            _CallShortMethodVRef = AllocateFunctionCode(JNI_CallShortMethodV);
            _CallNonvirtualShortMethodRef = AllocateFunctionCode(JNI_CallNonvirtualShortMethod);
            _CallNonvirtualShortMethodVRef = AllocateFunctionCode(JNI_CallNonvirtualShortMethodV);
            _CallStaticShortMethodRef = AllocateFunctionCode(JNI_CallStaticShortMethod);
            _CallStaticShortMethodVRef = AllocateFunctionCode(JNI_CallStaticShortMethodV);
            _CallIntMethodRef = AllocateFunctionCode(JNI_CallIntMethod);
            _CallIntMethodVRef = AllocateFunctionCode(JNI_CallIntMethodV);
            _CallNonvirtualIntMethodRef = AllocateFunctionCode(JNI_CallNonvirtualIntMethod);
            _CallNonvirtualIntMethodVRef = AllocateFunctionCode(JNI_CallNonvirtualIntMethodV);
            _CallStaticIntMethodRef = AllocateFunctionCode(JNI_CallStaticIntMethod);
            _CallStaticIntMethodVRef = AllocateFunctionCode(JNI_CallStaticIntMethodV);
            _CallLongMethodRef = AllocateFunctionCode(JNI_CallLongMethod);
            _CallLongMethodVRef = AllocateFunctionCode(JNI_CallLongMethodV);
            _CallNonvirtualLongMethodRef = AllocateFunctionCode(JNI_CallNonvirtualLongMethod);
            _CallNonvirtualLongMethodVRef = AllocateFunctionCode(JNI_CallNonvirtualLongMethodV);
            _CallStaticLongMethodRef = AllocateFunctionCode(JNI_CallStaticLongMethod);
            _CallStaticLongMethodVRef = AllocateFunctionCode(JNI_CallStaticLongMethodV);
            _CallFloatMethodRef = AllocateFunctionCode(JNI_CallFloatMethod);
            _CallFloatMethodVRef = AllocateFunctionCode(JNI_CallFloatMethodV);
            _CallNonvirtualFloatMethodRef = AllocateFunctionCode(JNI_CallNonvirtualFloatMethod);
            _CallNonvirtualFloatMethodVRef = AllocateFunctionCode(JNI_CallNonvirtualFloatMethodV);
            _CallStaticFloatMethodRef = AllocateFunctionCode(JNI_CallStaticFloatMethod);
            _CallStaticFloatMethodVRef = AllocateFunctionCode(JNI_CallStaticFloatMethodV);
            _CallDoubleMethodRef = AllocateFunctionCode(JNI_CallDoubleMethod);
            _CallDoubleMethodVRef = AllocateFunctionCode(JNI_CallDoubleMethodV);
            _CallNonvirtualDoubleMethodRef = AllocateFunctionCode(JNI_CallNonvirtualDoubleMethod);
            _CallNonvirtualDoubleMethodVRef = AllocateFunctionCode(JNI_CallNonvirtualDoubleMethodV);
            _CallStaticDoubleMethodRef = AllocateFunctionCode(JNI_CallStaticDoubleMethod);
            _CallStaticDoubleMethodVRef = AllocateFunctionCode(JNI_CallStaticDoubleMethodV);
            _CallVoidMethodRef = AllocateFunctionCode(JNI_CallVoidMethod);
            _CallVoidMethodVRef = AllocateFunctionCode(JNI_CallVoidMethodV);
            _CallNonvirtualVoidMethodRef = AllocateFunctionCode(JNI_CallNonvirtualVoidMethod);
            _CallNonvirtualVoidMethodVRef = AllocateFunctionCode(JNI_CallNonvirtualVoidMethodV);
            _CallStaticVoidMethodRef = AllocateFunctionCode(JNI_CallStaticVoidMethod);
            _CallStaticVoidMethodVRef = AllocateFunctionCode(JNI_CallStaticVoidMethodV);
        }

        /// <summary>
        /// Allocates the given function code in executable memory and returns a reference.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        ExecutableMemory AllocateFunctionCode(byte[] code)
        {
            if (code is null)
                throw new ArgumentNullException(nameof(code));

            var m = ExecutableMemory.Allocate(code.Length);
            Marshal.Copy(code, 0, m.DangerousGetHandle(), m.Size);
            return m;
        }

        public IntPtr NewObject => _NewObjectRef.DangerousGetHandle();

        public IntPtr NewObjectV => _NewObjectVRef.DangerousGetHandle();

        public IntPtr CallObjectMethod => _CallObjectMethodRef.DangerousGetHandle();

        public IntPtr CallObjectMethodV => _CallObjectMethodVRef.DangerousGetHandle();

        public IntPtr CallNonvirtualObjectMethod => _CallNonvirtualObjectMethodRef.DangerousGetHandle();

        public IntPtr CallNonvirtualObjectMethodV => _CallNonvirtualObjectMethodVRef.DangerousGetHandle();

        public IntPtr CallStaticObjectMethod => _CallStaticObjectMethodRef.DangerousGetHandle();

        public IntPtr CallStaticObjectMethodV => _CallStaticObjectMethodVRef.DangerousGetHandle();

        public IntPtr CallBooleanMethod => _CallBooleanMethodRef.DangerousGetHandle();

        public IntPtr CallBooleanMethodV => _CallBooleanMethodVRef.DangerousGetHandle();

        public IntPtr CallNonvirtualBooleanMethod => _CallNonvirtualBooleanMethodRef.DangerousGetHandle();

        public IntPtr CallNonvirtualBooleanMethodV => _CallNonvirtualBooleanMethodVRef.DangerousGetHandle();

        public IntPtr CallStaticBooleanMethod => _CallStaticBooleanMethodRef.DangerousGetHandle();

        public IntPtr CallStaticBooleanMethodV => _CallStaticBooleanMethodVRef.DangerousGetHandle();

        public IntPtr CallByteMethod => _CallByteMethodRef.DangerousGetHandle();

        public IntPtr CallByteMethodV => _CallByteMethodVRef.DangerousGetHandle();

        public IntPtr CallNonvirtualByteMethod => _CallNonvirtualByteMethodRef.DangerousGetHandle();

        public IntPtr CallNonvirtualByteMethodV => _CallNonvirtualByteMethodVRef.DangerousGetHandle();

        public IntPtr CallStaticByteMethod => _CallStaticByteMethodRef.DangerousGetHandle();

        public IntPtr CallStaticByteMethodV => _CallStaticByteMethodVRef.DangerousGetHandle();

        public IntPtr CallCharMethod => _CallCharMethodRef.DangerousGetHandle();

        public IntPtr CallCharMethodV => _CallCharMethodVRef.DangerousGetHandle();

        public IntPtr CallNonvirtualCharMethod => _CallNonvirtualCharMethodRef.DangerousGetHandle();

        public IntPtr CallNonvirtualCharMethodV => _CallNonvirtualCharMethodVRef.DangerousGetHandle();

        public IntPtr CallStaticCharMethod => _CallStaticCharMethodRef.DangerousGetHandle();

        public IntPtr CallStaticCharMethodV => _CallStaticCharMethodVRef.DangerousGetHandle();

        public IntPtr CallShortMethod => _CallShortMethodRef.DangerousGetHandle();

        public IntPtr CallShortMethodV => _CallShortMethodVRef.DangerousGetHandle();

        public IntPtr CallNonvirtualShortMethod => _CallNonvirtualShortMethodRef.DangerousGetHandle();

        public IntPtr CallNonvirtualShortMethodV => _CallNonvirtualShortMethodVRef.DangerousGetHandle();

        public IntPtr CallStaticShortMethod => _CallStaticShortMethodRef.DangerousGetHandle();

        public IntPtr CallStaticShortMethodV => _CallStaticShortMethodVRef.DangerousGetHandle();

        public IntPtr CallIntMethod => _CallIntMethodRef.DangerousGetHandle();

        public IntPtr CallIntMethodV => _CallIntMethodVRef.DangerousGetHandle();

        public IntPtr CallNonvirtualIntMethod => _CallNonvirtualIntMethodRef.DangerousGetHandle();

        public IntPtr CallNonvirtualIntMethodV => _CallNonvirtualIntMethodVRef.DangerousGetHandle();

        public IntPtr CallStaticIntMethod => _CallStaticIntMethodRef.DangerousGetHandle();

        public IntPtr CallStaticIntMethodV => _CallStaticIntMethodVRef.DangerousGetHandle();

        public IntPtr CallLongMethod => _CallLongMethodRef.DangerousGetHandle();

        public IntPtr CallLongMethodV => _CallLongMethodVRef.DangerousGetHandle();

        public IntPtr CallNonvirtualLongMethod => _CallNonvirtualLongMethodRef.DangerousGetHandle();

        public IntPtr CallNonvirtualLongMethodV => _CallNonvirtualLongMethodVRef.DangerousGetHandle();

        public IntPtr CallStaticLongMethod => _CallStaticLongMethodRef.DangerousGetHandle();

        public IntPtr CallStaticLongMethodV => _CallStaticLongMethodVRef.DangerousGetHandle();

        public IntPtr CallFloatMethod => _CallFloatMethodRef.DangerousGetHandle();

        public IntPtr CallFloatMethodV => _CallFloatMethodVRef.DangerousGetHandle();

        public IntPtr CallNonvirtualFloatMethod => _CallNonvirtualFloatMethodRef.DangerousGetHandle();

        public IntPtr CallNonvirtualFloatMethodV => _CallNonvirtualFloatMethodVRef.DangerousGetHandle();

        public IntPtr CallStaticFloatMethod => _CallStaticFloatMethodRef.DangerousGetHandle();

        public IntPtr CallStaticFloatMethodV => _CallStaticFloatMethodVRef.DangerousGetHandle();

        public IntPtr CallDoubleMethod => _CallDoubleMethodRef.DangerousGetHandle();

        public IntPtr CallDoubleMethodV => _CallDoubleMethodVRef.DangerousGetHandle();

        public IntPtr CallNonvirtualDoubleMethod => _CallNonvirtualDoubleMethodRef.DangerousGetHandle();

        public IntPtr CallNonvirtualDoubleMethodV => _CallNonvirtualDoubleMethodVRef.DangerousGetHandle();

        public IntPtr CallStaticDoubleMethod => _CallStaticDoubleMethodRef.DangerousGetHandle();

        public IntPtr CallStaticDoubleMethodV => _CallStaticDoubleMethodVRef.DangerousGetHandle();

        public IntPtr CallVoidMethod => _CallVoidMethodRef.DangerousGetHandle();

        public IntPtr CallVoidMethodV => _CallVoidMethodVRef.DangerousGetHandle();

        public IntPtr CallNonvirtualVoidMethod => _CallNonvirtualVoidMethodRef.DangerousGetHandle();

        public IntPtr CallNonvirtualVoidMethodV => _CallNonvirtualVoidMethodVRef.DangerousGetHandle();

        public IntPtr CallStaticVoidMethod => _CallStaticVoidMethodRef.DangerousGetHandle();

        public IntPtr CallStaticVoidMethodV => _CallStaticVoidMethodVRef.DangerousGetHandle();

        protected abstract byte[] JNI_CallObjectMethod { get; }

        protected abstract byte[] JNI_CallObjectMethodV { get; }

        protected abstract byte[] JNI_CallNonvirtualObjectMethod { get; }

        protected abstract byte[] JNI_CallNonvirtualObjectMethodV { get; }

        protected abstract byte[] JNI_CallStaticObjectMethod { get; }

        protected abstract byte[] JNI_CallStaticObjectMethodV { get; }

        protected abstract byte[] JNI_CallBooleanMethod { get; }

        protected abstract byte[] JNI_CallBooleanMethodV { get; }

        protected abstract byte[] JNI_CallNonvirtualBooleanMethod { get; }

        protected abstract byte[] JNI_CallNonvirtualBooleanMethodV { get; }

        protected abstract byte[] JNI_CallStaticBooleanMethod { get; }

        protected abstract byte[] JNI_CallStaticBooleanMethodV { get; }

        protected abstract byte[] JNI_CallByteMethod { get; }

        protected abstract byte[] JNI_CallByteMethodV { get; }

        protected abstract byte[] JNI_CallNonvirtualByteMethod { get; }

        protected abstract byte[] JNI_CallNonvirtualByteMethodV { get; }

        protected abstract byte[] JNI_CallStaticByteMethod { get; }

        protected abstract byte[] JNI_CallStaticByteMethodV { get; }

        protected abstract byte[] JNI_CallCharMethod { get; }

        protected abstract byte[] JNI_CallCharMethodV { get; }

        protected abstract byte[] JNI_CallNonvirtualCharMethod { get; }

        protected abstract byte[] JNI_CallNonvirtualCharMethodV { get; }

        protected abstract byte[] JNI_CallStaticCharMethod { get; }

        protected abstract byte[] JNI_CallStaticCharMethodV { get; }

        protected abstract byte[] JNI_CallShortMethod { get; }

        protected abstract byte[] JNI_CallShortMethodV { get; }

        protected abstract byte[] JNI_CallNonvirtualShortMethod { get; }

        protected abstract byte[] JNI_CallNonvirtualShortMethodV { get; }

        protected abstract byte[] JNI_CallStaticShortMethod { get; }

        protected abstract byte[] JNI_CallStaticShortMethodV { get; }

        protected abstract byte[] JNI_CallIntMethod { get; }

        protected abstract byte[] JNI_CallIntMethodV { get; }

        protected abstract byte[] JNI_CallNonvirtualIntMethod { get; }

        protected abstract byte[] JNI_CallNonvirtualIntMethodV { get; }

        protected abstract byte[] JNI_CallStaticIntMethod { get; }

        protected abstract byte[] JNI_CallStaticIntMethodV { get; }

        protected abstract byte[] JNI_CallLongMethod { get; }

        protected abstract byte[] JNI_CallLongMethodV { get; }

        protected abstract byte[] JNI_CallNonvirtualLongMethod { get; }

        protected abstract byte[] JNI_CallNonvirtualLongMethodV { get; }

        protected abstract byte[] JNI_CallStaticLongMethod { get; }

        protected abstract byte[] JNI_CallStaticLongMethodV { get; }

        protected abstract byte[] JNI_CallFloatMethod { get; }

        protected abstract byte[] JNI_CallFloatMethodV { get; }

        protected abstract byte[] JNI_CallNonvirtualFloatMethod { get; }

        protected abstract byte[] JNI_CallNonvirtualFloatMethodV { get; }

        protected abstract byte[] JNI_CallStaticFloatMethod { get; }

        protected abstract byte[] JNI_CallStaticFloatMethodV { get; }

        protected abstract byte[] JNI_CallDoubleMethod { get; }

        protected abstract byte[] JNI_CallDoubleMethodV { get; }

        protected abstract byte[] JNI_CallNonvirtualDoubleMethod { get; }

        protected abstract byte[] JNI_CallNonvirtualDoubleMethodV { get; }

        protected abstract byte[] JNI_CallStaticDoubleMethod { get; }

        protected abstract byte[] JNI_CallStaticDoubleMethodV { get; }

        protected abstract byte[] JNI_NewObject { get; }

        protected abstract byte[] JNI_NewObjectV { get; }

        protected abstract byte[] JNI_CallVoidMethod { get; }

        protected abstract byte[] JNI_CallVoidMethodV { get; }

        protected abstract byte[] JNI_CallNonvirtualVoidMethod { get; }

        protected abstract byte[] JNI_CallNonvirtualVoidMethodV { get; }

        protected abstract byte[] JNI_CallStaticVoidMethod { get; }

        protected abstract byte[] JNI_CallStaticVoidMethodV { get; }

    }

}
