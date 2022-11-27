using System.Runtime.InteropServices;

namespace IKVM.Runtime.JNI
{

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct JNINativeMethod
    {

        public byte* name;
        public byte* signature;
        void* fnPtr;

    }

}
