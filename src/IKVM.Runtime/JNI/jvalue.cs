using System.Runtime.InteropServices;

namespace IKVM.Runtime.JNI
{

    using jboolean = System.SByte;
    using jbyte = System.SByte;
    using jchar = System.UInt16;
    using jdouble = System.Double;
    using jfloat = System.Single;
    using jint = System.Int32;
    using jlong = System.Int64;
    using jobject = System.IntPtr;
    using jshort = System.Int16;

    /// <summary>
    /// Union type that represents 'jvalue'.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    struct JValue
    {

        [FieldOffset(0)]
        public jboolean z;

        [FieldOffset(0)]
        public jbyte b;

        [FieldOffset(0)]
        public jchar c;

        [FieldOffset(0)]
        public jshort s;

        [FieldOffset(0)]
        public jint i;

        [FieldOffset(0)]
        public jlong j;

        [FieldOffset(0)]
        public jfloat f;

        [FieldOffset(0)]
        public jdouble d;

        [FieldOffset(0)]
        public jobject l;

    }

}
