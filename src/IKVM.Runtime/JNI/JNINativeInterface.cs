using System;
using System.Runtime.InteropServices;

namespace IKVM.Runtime.JNI
{

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


    /// <summary>
    /// Manged implementation of the JNINativeInterface structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    unsafe struct JNINativeInterface
    {

        /// <summary>
        /// Holds an instance of the JVM library and frees it upon deconstruction.
        /// </summary>
        class LibJvm
        {

            /// <summary>
            /// Gets the handle to the JVM library.
            /// </summary>
            public nint Handle { get; private set; } = Load();

            /// <summary>
            /// Loads the library.
            /// </summary>
            static nint Load()
            {
                var h = NativeLibrary.Load("jvm");
                if (h == 0)
                    throw new DllNotFoundException("Could not preload JVM library.");

                return h;
            }

            /// <summary>
            /// Disposes of the instance.
            /// </summary>
            ~LibJvm()
            {
                if (Handle != 0)
                {
                    NativeLibrary.Free(Handle);
                    Handle = 0;
                }
            }

        }

        /// <summary>
        /// Maintains a <see cref="JNINativeInterface"/> structure in memory.
        /// </summary>
        class JNINativeInterfaceMemory
        {

            internal readonly JNINativeInterface* handle;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            public JNINativeInterfaceMemory()
            {
                handle = (JNINativeInterface*)Marshal.AllocHGlobal(sizeof(JNINativeInterface));
            }

            /// <summary>
            /// Finalizes the instance.
            /// </summary>
            ~JNINativeInterfaceMemory()
            {
                Marshal.FreeHGlobal((nint)handle);
            }

        }

        #region Delegates

        delegate int GetMethodArgsDelegateType(JNIEnv* pEnv, nint methodID, byte* sig);

        delegate jint GetVersionDelegateType(JNIEnv* pEnv);
        delegate jclass DefineClassDelegateType(JNIEnv* pEnv, byte* name, jobject loader, jbyte* pbuf, jint length);
        delegate jclass FindClassDelegateType(JNIEnv* pEnv, byte* name);

        delegate jmethodID FromReflectedMethodDelegateType(JNIEnv* pEnv, jmethodID methodID);
        delegate jfieldID FromReflectedFieldDelegateType(JNIEnv* pEnv, jfieldID fieldID);

        delegate jobject ToReflectedMethodDelegateType(JNIEnv* pEnv, jclass clazz_ignored, jmethodID method, jboolean isStatic);

        delegate jclass GetSuperclassDelegateType(JNIEnv* pEnv, jclass sub);
        delegate jboolean IsAssignableFromDelegateType(JNIEnv* pEnv, jclass sub, jclass super);

        delegate jobject ToReflectedFieldDelegateType(JNIEnv* pEnv, jclass clazz_ignored, jfieldID field, jboolean isStatic);

        delegate jint ThrowDelegateType(JNIEnv* pEnv, jthrowable throwable);
        delegate jint ThrowNewDelegateType(JNIEnv* pEnv, jclass clazz, byte* msg);
        delegate jthrowable ExceptionOccurredDelegateType(JNIEnv* pEnv);
        delegate void ExceptionDescribeDelegateType(JNIEnv* pEnv);
        delegate void ExceptionClearDelegateType(JNIEnv* pEnv);
        delegate void FatalErrorDelegateType(JNIEnv* pEnv, byte* msg);

        delegate jint PushLocalFrameDelegateType(JNIEnv* pEnv, jint capacity);
        delegate jobject PopLocalFrameDelegateType(JNIEnv* pEnv, jobject result);

        delegate jobject NewGlobalRefDelegateType(JNIEnv* pEnv, jobject obj);
        delegate void DeleteGlobalRefDelegateType(JNIEnv* pEnv, jobject obj);
        delegate void DeleteLocalRefDelegateType(JNIEnv* pEnv, jobject obj);
        delegate jboolean IsSameObjectDelegateType(JNIEnv* pEnv, jobject obj1, jobject obj2);
        delegate jobject NewLocalRefDelegateType(JNIEnv* pEnv, jobject obj);
        delegate jint EnsureLocalCapacityDelegateType(JNIEnv* pEnv, jint capacity);

        delegate jobject AllocObjectDelegateType(JNIEnv* pEnv, jclass clazz);
        delegate jobject NewObjectADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args);

        delegate jclass GetObjectClassDelegateType(JNIEnv* pEnv, jobject obj);
        delegate jboolean IsInstanceOfDelegateType(JNIEnv* pEnv, jobject obj, jclass clazz);

        delegate jmethodID GetMethodIDDelegateType(JNIEnv* pEnv, jclass clazz, byte* name, byte* sig);

        delegate jobject CallObjectMethodADelegateType(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args);
        delegate jboolean CallBooleanMethodADelegateType(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args);
        delegate jbyte CallByteMethodADelegateType(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args);
        delegate jchar CallCharMethodADelegateType(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args);
        delegate jshort CallShortMethodADelegateType(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args);
        delegate jint CallIntMethodADelegateType(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args);
        delegate jlong CallLongMethodADelegateType(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args);
        delegate jfloat CallFloatMethodADelegateType(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args);
        delegate jdouble CallDoubleMethodADelegateType(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args);
        delegate void CallVoidMethodADelegateType(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args);

        delegate jobject CallNonvirtualObjectMethodADelegateType(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args);
        delegate jboolean CallNonvirtualBooleanMethodADelegateType(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args);
        delegate jbyte CallNonvirtualByteMethodADelegateType(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args);
        delegate jchar CallNonvirtualCharMethodADelegateType(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args);
        delegate jshort CallNonvirtualShortMethodADelegateType(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args);
        delegate jint CallNonvirtualIntMethodADelegateType(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args);
        delegate jlong CallNonvirtualLongMethodADelegateType(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args);
        delegate jfloat CallNonvirtualFloatMethodADelegateType(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args);
        delegate jdouble CallNonvirtualDoubleMethodADelegateType(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args);
        delegate void CallNonvirtualVoidMethodADelegateType(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args);

        delegate jfieldID GetFieldIDDelegateType(JNIEnv* pEnv, jclass clazz, byte* name, byte* sig);

        delegate jobject GetObjectFieldDelegateType(JNIEnv* pEnv, jobject obj, jfieldID fieldID);
        delegate jboolean GetBooleanFieldDelegateType(JNIEnv* pEnv, jobject obj, jfieldID fieldID);
        delegate jbyte GetByteFieldDelegateType(JNIEnv* pEnv, jobject obj, jfieldID fieldID);
        delegate jchar GetCharFieldDelegateType(JNIEnv* pEnv, jobject obj, jfieldID fieldID);
        delegate jshort GetShortFieldDelegateType(JNIEnv* pEnv, jobject obj, jfieldID fieldID);
        delegate jint GetIntFieldDelegateType(JNIEnv* pEnv, jobject obj, jfieldID fieldID);
        delegate jlong GetLongFieldDelegateType(JNIEnv* pEnv, jobject obj, jfieldID fieldID);
        delegate jfloat GetFloatFieldDelegateType(JNIEnv* pEnv, jobject obj, jfieldID fieldID);
        delegate jdouble GetDoubleFieldDelegateType(JNIEnv* pEnv, jobject obj, jfieldID fieldID);

        delegate void SetObjectFieldDelegateType(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jobject val);
        delegate void SetBooleanFieldDelegateType(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jboolean val);
        delegate void SetByteFieldDelegateType(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jbyte val);
        delegate void SetCharFieldDelegateType(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jchar val);
        delegate void SetShortFieldDelegateType(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jshort val);
        delegate void SetIntFieldDelegateType(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jint val);
        delegate void SetLongFieldDelegateType(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jlong val);
        delegate void SetFloatFieldDelegateType(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jfloat val);
        delegate void SetDoubleFieldDelegateType(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jdouble val);

        delegate jmethodID GetStaticMethodIDDelegateType(JNIEnv* pEnv, jclass clazz, byte* name, byte* sig);

        delegate jobject CallStaticObjectMethodADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args);
        delegate jboolean CallStaticBooleanMethodADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args);
        delegate jbyte CallStaticByteMethodADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args);
        delegate jchar CallStaticCharMethodADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args);
        delegate jshort CallStaticShortMethodADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args);
        delegate jint CallStaticIntMethodADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args);
        delegate jlong CallStaticLongMethodADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args);
        delegate jfloat CallStaticFloatMethodADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args);
        delegate jdouble CallStaticDoubleMethodADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args);
        delegate void CallStaticVoidMethodADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args);

        delegate jfieldID GetStaticFieldIDDelegateType(JNIEnv* pEnv, jclass clazz, byte* name, byte* sig);

        delegate jobject GetStaticObjectFieldDelegateType(JNIEnv* pEnv, jclass clazz, jfieldID fieldID);
        delegate jboolean GetStaticBooleanFieldDelegateType(JNIEnv* pEnv, jclass clazz, jfieldID fieldID);
        delegate jbyte GetStaticByteFieldDelegateType(JNIEnv* pEnv, jclass clazz, jfieldID fieldID);
        delegate jchar GetStaticCharFieldDelegateType(JNIEnv* pEnv, jclass clazz, jfieldID fieldID);
        delegate jshort GetStaticShortFieldDelegateType(JNIEnv* pEnv, jclass clazz, jfieldID fieldID);
        delegate jint GetStaticIntFieldDelegateType(JNIEnv* pEnv, jclass clazz, jfieldID fieldID);
        delegate jlong GetStaticLongFieldDelegateType(JNIEnv* pEnv, jclass clazz, jfieldID fieldID);
        delegate jfloat GetStaticFloatFieldDelegateType(JNIEnv* pEnv, jclass clazz, jfieldID fieldID);
        delegate jdouble GetStaticDoubleFieldDelegateType(JNIEnv* pEnv, jclass clazz, jfieldID fieldID);

        delegate void SetStaticObjectFieldDelegateType(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jobject val);
        delegate void SetStaticBooleanFieldDelegateType(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jboolean val);
        delegate void SetStaticByteFieldDelegateType(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jbyte val);
        delegate void SetStaticCharFieldDelegateType(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jchar val);
        delegate void SetStaticShortFieldDelegateType(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jshort val);
        delegate void SetStaticIntFieldDelegateType(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jint val);
        delegate void SetStaticLongFieldDelegateType(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jlong val);
        delegate void SetStaticFloatFieldDelegateType(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jfloat val);
        delegate void SetStaticDoubleFieldDelegateType(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jdouble val);

        delegate jstring NewStringDelegateType(JNIEnv* pEnv, jchar* unicode, int len);
        delegate jint GetStringLengthDelegateType(JNIEnv* pEnv, jstring str);
        delegate jchar* GetStringCharsDelegateType(JNIEnv* pEnv, jstring str, jboolean* isCopy);
        delegate void ReleaseStringCharsDelegateType(JNIEnv* pEnv, jstring str, jchar* chars);

        delegate jobject NewStringUTFDelegateType(JNIEnv* pEnv, byte* bytes);
        delegate jint GetStringUTFLengthDelegateType(JNIEnv* pEnv, jstring str);
        delegate byte* GetStringUTFCharsDelegateType(JNIEnv* pEnv, jstring @string, jboolean* isCopy);
        delegate void ReleaseStringUTFCharsDelegateType(JNIEnv* pEnv, jstring @string, byte* utf);

        delegate jsize GetArrayLengthDelegateType(JNIEnv* pEnv, jarray array);

        delegate jobjectArray NewObjectArrayDelegateType(JNIEnv* pEnv, jsize len, jclass clazz, jobject init);
        delegate jobject GetObjectArrayElementDelegateType(JNIEnv* pEnv, jarray array, jsize index);
        delegate void SetObjectArrayElementDelegateType(JNIEnv* pEnv, jarray array, jsize index, jobject val);

        delegate jbooleanArray NewBooleanArrayDelegateType(JNIEnv* pEnv, jsize len);
        delegate jbyteArray NewByteArrayDelegateType(JNIEnv* pEnv, jsize len);
        delegate jcharArray NewCharArrayDelegateType(JNIEnv* pEnv, jsize len);
        delegate jshortArray NewShortArrayDelegateType(JNIEnv* pEnv, jsize len);
        delegate jintArray NewIntArrayDelegateType(JNIEnv* pEnv, jsize len);
        delegate jlongArray NewLongArrayDelegateType(JNIEnv* pEnv, jsize len);
        delegate jfloatArray NewFloatArrayDelegateType(JNIEnv* pEnv, jsize len);
        delegate jdoubleArray NewDoubleArrayDelegateType(JNIEnv* pEnv, jsize len);

        delegate jboolean* GetBooleanArrayElementsDelegateType(JNIEnv* pEnv, jbooleanArray array, jboolean* isCopy);
        delegate jbyte* GetByteArrayElementsDelegateType(JNIEnv* pEnv, jbyteArray array, jboolean* isCopy);
        delegate jchar* GetCharArrayElementsDelegateType(JNIEnv* pEnv, jcharArray array, jboolean* isCopy);
        delegate jshort* GetShortArrayElementsDelegateType(JNIEnv* pEnv, jshortArray array, jboolean* isCopy);
        delegate jint* GetIntArrayElementsDelegateType(JNIEnv* pEnv, jintArray array, jboolean* isCopy);
        delegate jlong* GetLongArrayElementsDelegateType(JNIEnv* pEnv, jlongArray array, jboolean* isCopy);
        delegate jfloat* GetFloatArrayElementsDelegateType(JNIEnv* pEnv, jfloatArray array, jboolean* isCopy);
        delegate jdouble* GetDoubleArrayElementsDelegateType(JNIEnv* pEnv, jdoubleArray array, jboolean* isCopy);

        delegate void ReleaseBooleanArrayElementsDelegateType(JNIEnv* pEnv, jbooleanArray array, jboolean* elems, jint mode);
        delegate void ReleaseByteArrayElementsDelegateType(JNIEnv* pEnv, jbyteArray array, jbyte* elems, jint mode);
        delegate void ReleaseCharArrayElementsDelegateType(JNIEnv* pEnv, jcharArray array, jchar* elems, jint mode);
        delegate void ReleaseShortArrayElementsDelegateType(JNIEnv* pEnv, jshortArray array, jshort* elems, jint mode);
        delegate void ReleaseIntArrayElementsDelegateType(JNIEnv* pEnv, jintArray array, jint* elems, jint mode);
        delegate void ReleaseLongArrayElementsDelegateType(JNIEnv* pEnv, jlongArray array, jlong* elems, jint mode);
        delegate void ReleaseFloatArrayElementsDelegateType(JNIEnv* pEnv, jfloatArray array, jfloat* elems, jint mode);
        delegate void ReleaseDoubleArrayElementsDelegateType(JNIEnv* pEnv, jdoubleArray array, jdouble* elems, jint mode);

        delegate void GetBooleanArrayRegionDelegateType(JNIEnv* pEnv, jbooleanArray array, jsize start, jsize len, jboolean* buf);
        delegate void GetByteArrayRegionDelegateType(JNIEnv* pEnv, jbyteArray array, jsize start, jsize len, jbyte* buf);
        delegate void GetCharArrayRegionDelegateType(JNIEnv* pEnv, jcharArray array, jsize start, jsize len, jchar* buf);
        delegate void GetShortArrayRegionDelegateType(JNIEnv* pEnv, jshortArray array, jsize start, jsize len, jshort* buf);
        delegate void GetIntArrayRegionDelegateType(JNIEnv* pEnv, jintArray array, jsize start, jsize len, jint* buf);
        delegate void GetLongArrayRegionDelegateType(JNIEnv* pEnv, jlongArray array, jsize start, jsize len, jlong* buf);
        delegate void GetFloatArrayRegionDelegateType(JNIEnv* pEnv, jfloatArray array, jsize start, jsize len, jfloat* buf);
        delegate void GetDoubleArrayRegionDelegateType(JNIEnv* pEnv, jdoubleArray array, jsize start, jsize len, jdouble* buf);

        delegate void SetBooleanArrayRegionDelegateType(JNIEnv* pEnv, jbooleanArray array, jsize start, jsize len, jboolean* buf);
        delegate void SetByteArrayRegionDelegateType(JNIEnv* pEnv, jbyteArray array, jsize start, jsize len, jbyte* buf);
        delegate void SetCharArrayRegionDelegateType(JNIEnv* pEnv, jcharArray array, jsize start, jsize len, jchar* buf);
        delegate void SetShortArrayRegionDelegateType(JNIEnv* pEnv, jshortArray array, jsize start, jsize len, jshort* buf);
        delegate void SetIntArrayRegionDelegateType(JNIEnv* pEnv, jintArray array, jsize start, jsize len, jint* buf);
        delegate void SetLongArrayRegionDelegateType(JNIEnv* pEnv, jlongArray array, jsize start, jsize len, jlong* buf);
        delegate void SetFloatArrayRegionDelegateType(JNIEnv* pEnv, jfloatArray array, jsize start, jsize len, jfloat* buf);
        delegate void SetDoubleArrayRegionDelegateType(JNIEnv* pEnv, jdoubleArray array, jsize start, jsize len, jdouble* buf);

        delegate jint RegisterNativesDelegateType(JNIEnv* pEnv, jclass clazz, JNIEnv.JNINativeMethod* methods, jint nMethods);
        delegate jint UnregisterNativesDelegateType(JNIEnv* pEnv, jclass clazz);

        delegate jint MonitorEnterDelegateType(JNIEnv* pEnv, jobject obj);
        delegate jint MonitorExitDelegateType(JNIEnv* pEnv, jobject obj);

        delegate jint GetJavaVMDelegateType(JNIEnv* pEnv, JavaVM** ppJavaVM);

        delegate void GetStringRegionDelegateType(JNIEnv* pEnv, jstring str, jsize start, jsize len, jchar* buf);
        delegate void GetStringUTFRegionDelegateType(JNIEnv* pEnv, jstring str, jsize start, jsize len, byte* buf);

        delegate void* GetPrimitiveArrayCriticalDelegateType(JNIEnv* pEnv, jarray array, jboolean* isCopy);
        delegate void ReleasePrimitiveArrayCriticalDelegateType(JNIEnv* pEnv, jarray array, void* carray, jint mode);

        delegate jchar* GetStringCriticalDelegateType(JNIEnv* pEnv, jstring str, jboolean* isCopy);
        delegate void ReleaseStringCriticalDelegateType(JNIEnv* pEnv, jstring str, jchar* cstring);

        delegate jweak NewWeakGlobalRefDelegateType(JNIEnv* pEnv, jobject obj);
        delegate void DeleteWeakGlobalRefDelegateType(JNIEnv* pEnv, jweak obj);

        delegate jboolean ExceptionCheckDelegateType(JNIEnv* pEnv);

        delegate jobject NewDirectByteBufferDelegateType(JNIEnv* pEnv, IntPtr address, jlong capacity);
        delegate void* GetDirectBufferAddressDelegateType(JNIEnv* pEnv, jobject buf);
        delegate jlong GetDirectBufferCapacityDelegateType(JNIEnv* pEnv, jobject buf);

        delegate jobjectRefType GetObjectRefTypeDelegateType(JNIEnv* pEnv, jobject obj);

        #endregion

        static readonly LibJvm libjvm = new LibJvm();
        static readonly JNINativeInterfaceMemory memory = new();
        static readonly JNINativeInterface* handle = memory.handle;

        /// <summary>
        /// Gets a pointer to the JNINativeInterface table.
        /// </summary>
        public static JNINativeInterface* Handle => handle;

        #region Delegates

        readonly static GetMethodArgsDelegateType GetMethodArgsDelegate = JNIEnv.GetMethodArgs;

        readonly static GetVersionDelegateType GetVersionDelegate = JNIEnv.GetVersion;
        readonly static DefineClassDelegateType DefineClassDelegate = JNIEnv.DefineClass;
        readonly static FindClassDelegateType FindClassDelegate = JNIEnv.FindClass;

        readonly static FromReflectedMethodDelegateType FromReflectedMethodDelegate = JNIEnv.FromReflectedMethod;
        readonly static FromReflectedFieldDelegateType FromReflectedFieldDelegate = JNIEnv.FromReflectedField;

        readonly static ToReflectedMethodDelegateType ToReflectedMethodDelegate = JNIEnv.ToReflectedMethod;

        readonly static GetSuperclassDelegateType GetSuperclassDelegate = JNIEnv.GetSuperclass;
        readonly static IsAssignableFromDelegateType IsAssignableFromDelegate = JNIEnv.IsAssignableFrom;

        readonly static ToReflectedFieldDelegateType ToReflectedFieldDelegate = JNIEnv.ToReflectedField;

        readonly static ThrowDelegateType ThrowDelegate = JNIEnv.Throw;
        readonly static ThrowNewDelegateType ThrowNewDelegate = JNIEnv.ThrowNew;
        readonly static ExceptionOccurredDelegateType ExceptionOccurredDelegate = JNIEnv.ExceptionOccurred;
        readonly static ExceptionDescribeDelegateType ExceptionDescribeDelegate = JNIEnv.ExceptionDescribe;
        readonly static ExceptionClearDelegateType ExceptionClearDelegate = JNIEnv.ExceptionClear;
        readonly static FatalErrorDelegateType FatalErrorDelegate = JNIEnv.FatalError;

        readonly static PushLocalFrameDelegateType PushLocalFrameDelegate = JNIEnv.PushLocalFrame;
        readonly static PopLocalFrameDelegateType PopLocalFrameDelegate = JNIEnv.PopLocalFrame;

        readonly static NewGlobalRefDelegateType NewGlobalRefDelegate = JNIEnv.NewGlobalRef;
        readonly static DeleteGlobalRefDelegateType DeleteGlobalRefDelegate = JNIEnv.DeleteGlobalRef;
        readonly static DeleteLocalRefDelegateType DeleteLocalRefDelegate = JNIEnv.DeleteLocalRef;
        readonly static IsSameObjectDelegateType IsSameObjectDelegate = JNIEnv.IsSameObject;
        readonly static NewLocalRefDelegateType NewLocalRefDelegate = JNIEnv.NewLocalRef;
        readonly static EnsureLocalCapacityDelegateType EnsureLocalCapacityDelegate = JNIEnv.EnsureLocalCapacity;

        readonly static AllocObjectDelegateType AllocObjectDelegate = JNIEnv.AllocObject;
        readonly static NewObjectADelegateType NewObjectADelegate = JNIEnv.NewObjectA;

        readonly static GetObjectClassDelegateType GetObjectClassDelegate = JNIEnv.GetObjectClass;
        readonly static IsInstanceOfDelegateType IsInstanceOfDelegate = JNIEnv.IsInstanceOf;

        readonly static GetMethodIDDelegateType GetMethodIDDelegate = JNIEnv.GetMethodID;

        readonly static CallObjectMethodADelegateType CallObjectMethodADelegate = JNIEnv.CallObjectMethodA;
        readonly static CallBooleanMethodADelegateType CallBooleanMethodADelegate = JNIEnv.CallBooleanMethodA;
        readonly static CallByteMethodADelegateType CallByteMethodADelegate = JNIEnv.CallByteMethodA;
        readonly static CallCharMethodADelegateType CallCharMethodADelegate = JNIEnv.CallCharMethodA;
        readonly static CallShortMethodADelegateType CallShortMethodADelegate = JNIEnv.CallShortMethodA;
        readonly static CallIntMethodADelegateType CallIntMethodADelegate = JNIEnv.CallIntMethodA;
        readonly static CallLongMethodADelegateType CallLongMethodADelegate = JNIEnv.CallLongMethodA;
        readonly static CallFloatMethodADelegateType CallFloatMethodADelegate = JNIEnv.CallFloatMethodA;
        readonly static CallDoubleMethodADelegateType CallDoubleMethodADelegate = JNIEnv.CallDoubleMethodA;
        readonly static CallVoidMethodADelegateType CallVoidMethodADelegate = JNIEnv.CallVoidMethodA;

        readonly static CallNonvirtualObjectMethodADelegateType CallNonvirtualObjectMethodADelegate = JNIEnv.CallNonvirtualObjectMethodA;
        readonly static CallNonvirtualBooleanMethodADelegateType CallNonvirtualBooleanMethodADelegate = JNIEnv.CallNonvirtualBooleanMethodA;
        readonly static CallNonvirtualByteMethodADelegateType CallNonvirtualByteMethodADelegate = JNIEnv.CallNonvirtualByteMethodA;
        readonly static CallNonvirtualCharMethodADelegateType CallNonvirtualCharMethodADelegate = JNIEnv.CallNonvirtualCharMethodA;
        readonly static CallNonvirtualShortMethodADelegateType CallNonvirtualShortMethodADelegate = JNIEnv.CallNonvirtualShortMethodA;
        readonly static CallNonvirtualIntMethodADelegateType CallNonvirtualIntMethodADelegate = JNIEnv.CallNonvirtualIntMethodA;
        readonly static CallNonvirtualLongMethodADelegateType CallNonvirtualLongMethodADelegate = JNIEnv.CallNonvirtualLongMethodA;
        readonly static CallNonvirtualFloatMethodADelegateType CallNonvirtualFloatMethodADelegate = JNIEnv.CallNonvirtualFloatMethodA;
        readonly static CallNonvirtualDoubleMethodADelegateType CallNonvirtualDoubleMethodADelegate = JNIEnv.CallNonvirtualDoubleMethodA;
        readonly static CallNonvirtualVoidMethodADelegateType CallNonvirtualVoidMethodADelegate = JNIEnv.CallNonvirtualVoidMethodA;

        readonly static GetFieldIDDelegateType GetFieldIDDelegate = JNIEnv.GetFieldID;

        readonly static GetObjectFieldDelegateType GetObjectFieldDelegate = JNIEnv.GetObjectField;
        readonly static GetBooleanFieldDelegateType GetBooleanFieldDelegate = JNIEnv.GetBooleanField;
        readonly static GetByteFieldDelegateType GetByteFieldDelegate = JNIEnv.GetByteField;
        readonly static GetCharFieldDelegateType GetCharFieldDelegate = JNIEnv.GetCharField;
        readonly static GetShortFieldDelegateType GetShortFieldDelegate = JNIEnv.GetShortField;
        readonly static GetIntFieldDelegateType GetIntFieldDelegate = JNIEnv.GetIntField;
        readonly static GetLongFieldDelegateType GetLongFieldDelegate = JNIEnv.GetLongField;
        readonly static GetFloatFieldDelegateType GetFloatFieldDelegate = JNIEnv.GetFloatField;
        readonly static GetDoubleFieldDelegateType GetDoubleFieldDelegate = JNIEnv.GetDoubleField;

        readonly static SetObjectFieldDelegateType SetObjectFieldDelegate = JNIEnv.SetObjectField;
        readonly static SetBooleanFieldDelegateType SetBooleanFieldDelegate = JNIEnv.SetBooleanField;
        readonly static SetByteFieldDelegateType SetByteFieldDelegate = JNIEnv.SetByteField;
        readonly static SetCharFieldDelegateType SetCharFieldDelegate = JNIEnv.SetCharField;
        readonly static SetShortFieldDelegateType SetShortFieldDelegate = JNIEnv.SetShortField;
        readonly static SetIntFieldDelegateType SetIntFieldDelegate = JNIEnv.SetIntField;
        readonly static SetLongFieldDelegateType SetLongFieldDelegate = JNIEnv.SetLongField;
        readonly static SetFloatFieldDelegateType SetFloatFieldDelegate = JNIEnv.SetFloatField;
        readonly static SetDoubleFieldDelegateType SetDoubleFieldDelegate = JNIEnv.SetDoubleField;

        readonly static GetStaticMethodIDDelegateType GetStaticMethodIDDelegate = JNIEnv.GetStaticMethodID;

        readonly static CallStaticObjectMethodADelegateType CallStaticObjectMethodADelegate = JNIEnv.CallStaticObjectMethodA;
        readonly static CallStaticBooleanMethodADelegateType CallStaticBooleanMethodADelegate = JNIEnv.CallStaticBooleanMethodA;
        readonly static CallStaticByteMethodADelegateType CallStaticByteMethodADelegate = JNIEnv.CallStaticByteMethodA;
        readonly static CallStaticCharMethodADelegateType CallStaticCharMethodADelegate = JNIEnv.CallStaticCharMethodA;
        readonly static CallStaticShortMethodADelegateType CallStaticShortMethodADelegate = JNIEnv.CallStaticShortMethodA;
        readonly static CallStaticIntMethodADelegateType CallStaticIntMethodADelegate = JNIEnv.CallStaticIntMethodA;
        readonly static CallStaticLongMethodADelegateType CallStaticLongMethodADelegate = JNIEnv.CallStaticLongMethodA;
        readonly static CallStaticFloatMethodADelegateType CallStaticFloatMethodADelegate = JNIEnv.CallStaticFloatMethodA;
        readonly static CallStaticDoubleMethodADelegateType CallStaticDoubleMethodADelegate = JNIEnv.CallStaticDoubleMethodA;
        readonly static CallStaticVoidMethodADelegateType CallStaticVoidMethodADelegate = JNIEnv.CallStaticVoidMethodA;

        readonly static GetStaticFieldIDDelegateType GetStaticFieldIDDelegate = JNIEnv.GetStaticFieldID;
        readonly static GetStaticObjectFieldDelegateType GetStaticObjectFieldDelegate = JNIEnv.GetStaticObjectField;
        readonly static GetStaticBooleanFieldDelegateType GetStaticBooleanFieldDelegate = JNIEnv.GetStaticBooleanField;
        readonly static GetStaticByteFieldDelegateType GetStaticByteFieldDelegate = JNIEnv.GetStaticByteField;
        readonly static GetStaticCharFieldDelegateType GetStaticCharFieldDelegate = JNIEnv.GetStaticCharField;
        readonly static GetStaticShortFieldDelegateType GetStaticShortFieldDelegate = JNIEnv.GetStaticShortField;
        readonly static GetStaticIntFieldDelegateType GetStaticIntFieldDelegate = JNIEnv.GetStaticIntField;
        readonly static GetStaticLongFieldDelegateType GetStaticLongFieldDelegate = JNIEnv.GetStaticLongField;
        readonly static GetStaticFloatFieldDelegateType GetStaticFloatFieldDelegate = JNIEnv.GetStaticFloatField;
        readonly static GetStaticDoubleFieldDelegateType GetStaticDoubleFieldDelegate = JNIEnv.GetStaticDoubleField;

        readonly static SetStaticObjectFieldDelegateType SetStaticObjectFieldDelegate = JNIEnv.SetStaticObjectField;
        readonly static SetStaticBooleanFieldDelegateType SetStaticBooleanFieldDelegate = JNIEnv.SetStaticBooleanField;
        readonly static SetStaticByteFieldDelegateType SetStaticByteFieldDelegate = JNIEnv.SetStaticByteField;
        readonly static SetStaticCharFieldDelegateType SetStaticCharFieldDelegate = JNIEnv.SetStaticCharField;
        readonly static SetStaticShortFieldDelegateType SetStaticShortFieldDelegate = JNIEnv.SetStaticShortField;
        readonly static SetStaticIntFieldDelegateType SetStaticIntFieldDelegate = JNIEnv.SetStaticIntField;
        readonly static SetStaticLongFieldDelegateType SetStaticLongFieldDelegate = JNIEnv.SetStaticLongField;
        readonly static SetStaticFloatFieldDelegateType SetStaticFloatFieldDelegate = JNIEnv.SetStaticFloatField;
        readonly static SetStaticDoubleFieldDelegateType SetStaticDoubleFieldDelegate = JNIEnv.SetStaticDoubleField;

        readonly static NewStringDelegateType NewStringDelegate = JNIEnv.NewString;
        readonly static GetStringLengthDelegateType GetStringLengthDelegate = JNIEnv.GetStringLength;
        readonly static GetStringCharsDelegateType GetStringCharsDelegate = JNIEnv.GetStringChars;
        readonly static ReleaseStringCharsDelegateType ReleaseStringCharsDelegate = JNIEnv.ReleaseStringChars;

        readonly static NewStringUTFDelegateType NewStringUTFDelegate = JNIEnv.NewStringUTF;
        readonly static GetStringUTFLengthDelegateType GetStringUTFLengthDelegate = JNIEnv.GetStringUTFLength;
        readonly static GetStringUTFCharsDelegateType GetStringUTFCharsDelegate = JNIEnv.GetStringUTFChars;
        readonly static ReleaseStringUTFCharsDelegateType ReleaseStringUTFCharsDelegate = JNIEnv.ReleaseStringUTFChars;

        readonly static GetArrayLengthDelegateType GetArrayLengthDelegate = JNIEnv.GetArrayLength;

        readonly static NewObjectArrayDelegateType NewObjectArrayDelegate = JNIEnv.NewObjectArray;
        readonly static GetObjectArrayElementDelegateType GetObjectArrayElementDelegate = JNIEnv.GetObjectArrayElement;
        readonly static SetObjectArrayElementDelegateType SetObjectArrayElementDelegate = JNIEnv.SetObjectArrayElement;

        readonly static NewBooleanArrayDelegateType NewBooleanArrayDelegate = JNIEnv.NewBooleanArray;
        readonly static NewByteArrayDelegateType NewByteArrayDelegate = JNIEnv.NewByteArray;
        readonly static NewCharArrayDelegateType NewCharArrayDelegate = JNIEnv.NewCharArray;
        readonly static NewShortArrayDelegateType NewShortArrayDelegate = JNIEnv.NewShortArray;
        readonly static NewIntArrayDelegateType NewIntArrayDelegate = JNIEnv.NewIntArray;
        readonly static NewLongArrayDelegateType NewLongArrayDelegate = JNIEnv.NewLongArray;
        readonly static NewFloatArrayDelegateType NewFloatArrayDelegate = JNIEnv.NewFloatArray;
        readonly static NewDoubleArrayDelegateType NewDoubleArrayDelegate = JNIEnv.NewDoubleArray;

        readonly static GetBooleanArrayElementsDelegateType GetBooleanArrayElementsDelegate = JNIEnv.GetBooleanArrayElements;
        readonly static GetByteArrayElementsDelegateType GetByteArrayElementsDelegate = JNIEnv.GetByteArrayElements;
        readonly static GetCharArrayElementsDelegateType GetCharArrayElementsDelegate = JNIEnv.GetCharArrayElements;
        readonly static GetShortArrayElementsDelegateType GetShortArrayElementsDelegate = JNIEnv.GetShortArrayElements;
        readonly static GetIntArrayElementsDelegateType GetIntArrayElementsDelegate = JNIEnv.GetIntArrayElements;
        readonly static GetLongArrayElementsDelegateType GetLongArrayElementsDelegate = JNIEnv.GetLongArrayElements;
        readonly static GetFloatArrayElementsDelegateType GetFloatArrayElementsDelegate = JNIEnv.GetFloatArrayElements;
        readonly static GetDoubleArrayElementsDelegateType GetDoubleArrayElementsDelegate = JNIEnv.GetDoubleArrayElements;

        readonly static ReleaseBooleanArrayElementsDelegateType ReleaseBooleanArrayElementsDelegate = JNIEnv.ReleaseBooleanArrayElements;
        readonly static ReleaseByteArrayElementsDelegateType ReleaseByteArrayElementsDelegate = JNIEnv.ReleaseByteArrayElements;
        readonly static ReleaseCharArrayElementsDelegateType ReleaseCharArrayElementsDelegate = JNIEnv.ReleaseCharArrayElements;
        readonly static ReleaseShortArrayElementsDelegateType ReleaseShortArrayElementsDelegate = JNIEnv.ReleaseShortArrayElements;
        readonly static ReleaseIntArrayElementsDelegateType ReleaseIntArrayElementsDelegate = JNIEnv.ReleaseIntArrayElements;
        readonly static ReleaseLongArrayElementsDelegateType ReleaseLongArrayElementsDelegate = JNIEnv.ReleaseLongArrayElements;
        readonly static ReleaseFloatArrayElementsDelegateType ReleaseFloatArrayElementsDelegate = JNIEnv.ReleaseFloatArrayElements;
        readonly static ReleaseDoubleArrayElementsDelegateType ReleaseDoubleArrayElementsDelegate = JNIEnv.ReleaseDoubleArrayElements;

        readonly static GetBooleanArrayRegionDelegateType GetBooleanArrayRegionDelegate = JNIEnv.GetBooleanArrayRegion;
        readonly static GetByteArrayRegionDelegateType GetByteArrayRegionDelegate = JNIEnv.GetByteArrayRegion;
        readonly static GetCharArrayRegionDelegateType GetCharArrayRegionDelegate = JNIEnv.GetCharArrayRegion;
        readonly static GetShortArrayRegionDelegateType GetShortArrayRegionDelegate = JNIEnv.GetShortArrayRegion;
        readonly static GetIntArrayRegionDelegateType GetIntArrayRegionDelegate = JNIEnv.GetIntArrayRegion;
        readonly static GetLongArrayRegionDelegateType GetLongArrayRegionDelegate = JNIEnv.GetLongArrayRegion;
        readonly static GetFloatArrayRegionDelegateType GetFloatArrayRegionDelegate = JNIEnv.GetFloatArrayRegion;
        readonly static GetDoubleArrayRegionDelegateType GetDoubleArrayRegionDelegate = JNIEnv.GetDoubleArrayRegion;

        readonly static SetBooleanArrayRegionDelegateType SetBooleanArrayRegionDelegate = JNIEnv.SetBooleanArrayRegion;
        readonly static SetByteArrayRegionDelegateType SetByteArrayRegionDelegate = JNIEnv.SetByteArrayRegion;
        readonly static SetCharArrayRegionDelegateType SetCharArrayRegionDelegate = JNIEnv.SetCharArrayRegion;
        readonly static SetShortArrayRegionDelegateType SetShortArrayRegionDelegate = JNIEnv.SetShortArrayRegion;
        readonly static SetIntArrayRegionDelegateType SetIntArrayRegionDelegate = JNIEnv.SetIntArrayRegion;
        readonly static SetLongArrayRegionDelegateType SetLongArrayRegionDelegate = JNIEnv.SetLongArrayRegion;
        readonly static SetFloatArrayRegionDelegateType SetFloatArrayRegionDelegate = JNIEnv.SetFloatArrayRegion;
        readonly static SetDoubleArrayRegionDelegateType SetDoubleArrayRegionDelegate = JNIEnv.SetDoubleArrayRegion;

        readonly static RegisterNativesDelegateType RegisterNativesDelegate = JNIEnv.RegisterNatives;
        readonly static UnregisterNativesDelegateType UnregisterNativesDelegate = JNIEnv.UnregisterNatives;

        readonly static MonitorEnterDelegateType MonitorEnterDelegate = JNIEnv.MonitorEnter;
        readonly static MonitorExitDelegateType MonitorExitDelegate = JNIEnv.MonitorExit;

        readonly static GetJavaVMDelegateType GetJavaVMDelegate = JNIEnv.GetJavaVM;

        readonly static GetStringRegionDelegateType GetStringRegionDelegate = JNIEnv.GetStringRegion;
        readonly static GetStringUTFRegionDelegateType GetStringUTFRegionDelegate = JNIEnv.GetStringUTFRegion;

        readonly static GetPrimitiveArrayCriticalDelegateType GetPrimitiveArrayCriticalDelegate = JNIEnv.GetPrimitiveArrayCritical;
        readonly static ReleasePrimitiveArrayCriticalDelegateType ReleasePrimitiveArrayCriticalDelegate = JNIEnv.ReleasePrimitiveArrayCritical;

        readonly static GetStringCriticalDelegateType GetStringCriticalDelegate = JNIEnv.GetStringCritical;
        readonly static ReleaseStringCriticalDelegateType ReleaseStringCriticalDelegate = JNIEnv.ReleaseStringCritical;

        readonly static NewWeakGlobalRefDelegateType NewWeakGlobalRefDelegate = JNIEnv.NewWeakGlobalRef;
        readonly static DeleteWeakGlobalRefDelegateType DeleteWeakGlobalRefDelegate = JNIEnv.DeleteWeakGlobalRef;

        readonly static ExceptionCheckDelegateType ExceptionCheckDelegate = JNIEnv.ExceptionCheck;

        readonly static NewDirectByteBufferDelegateType NewDirectByteBufferDelegate = JNIEnv.NewDirectByteBuffer;
        readonly static GetDirectBufferAddressDelegateType GetDirectBufferAddressDelegate = JNIEnv.GetDirectBufferAddress;
        readonly static GetDirectBufferCapacityDelegateType GetDirectBufferCapacityDelegate = JNIEnv.GetDirectBufferCapacity;

        readonly static GetObjectRefTypeDelegateType GetObjectRefTypeDelegate = JNIEnv.GetObjectRefType;

        #endregion

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static JNINativeInterface()
        {
            JNIVM.jvmCreated = true;

            handle->GetMethodArgs = (void*)Marshal.GetFunctionPointerForDelegate(GetMethodArgsDelegate);
            handle->reserved1 = null;
            handle->reserved2 = null;

            handle->reserved3 = null;
            handle->GetVersion = (void*)Marshal.GetFunctionPointerForDelegate(GetVersionDelegate);

            handle->DefineClass = (void*)Marshal.GetFunctionPointerForDelegate(DefineClassDelegate);
            handle->FindClass = (void*)Marshal.GetFunctionPointerForDelegate(FindClassDelegate);

            handle->FromReflectedMethod = (void*)Marshal.GetFunctionPointerForDelegate(FromReflectedMethodDelegate);
            handle->FromReflectedField = (void*)Marshal.GetFunctionPointerForDelegate(FromReflectedFieldDelegate);
            handle->ToReflectedMethod = (void*)Marshal.GetFunctionPointerForDelegate(ToReflectedMethodDelegate);

            handle->GetSuperclass = (void*)Marshal.GetFunctionPointerForDelegate(GetSuperclassDelegate);
            handle->IsAssignableFrom = (void*)Marshal.GetFunctionPointerForDelegate(IsAssignableFromDelegate);

            handle->ToReflectedField = (void*)Marshal.GetFunctionPointerForDelegate(ToReflectedFieldDelegate);

            handle->Throw = (void*)Marshal.GetFunctionPointerForDelegate(ThrowDelegate);
            handle->ThrowNew = (void*)Marshal.GetFunctionPointerForDelegate(ThrowNewDelegate);
            handle->ExceptionOccurred = (void*)Marshal.GetFunctionPointerForDelegate(ExceptionOccurredDelegate);
            handle->ExceptionDescribe = (void*)Marshal.GetFunctionPointerForDelegate(ExceptionDescribeDelegate);
            handle->ExceptionClear = (void*)Marshal.GetFunctionPointerForDelegate(ExceptionClearDelegate);
            handle->FatalError = (void*)Marshal.GetFunctionPointerForDelegate(FatalErrorDelegate);

            handle->PushLocalFrame = (void*)Marshal.GetFunctionPointerForDelegate(PushLocalFrameDelegate);
            handle->PopLocalFrame = (void*)Marshal.GetFunctionPointerForDelegate(PopLocalFrameDelegate);

            handle->NewGlobalRef = (void*)Marshal.GetFunctionPointerForDelegate(NewGlobalRefDelegate);
            handle->DeleteGlobalRef = (void*)Marshal.GetFunctionPointerForDelegate(DeleteGlobalRefDelegate);
            handle->DeleteLocalRef = (void*)Marshal.GetFunctionPointerForDelegate(DeleteLocalRefDelegate);
            handle->IsSameObject = (void*)Marshal.GetFunctionPointerForDelegate(IsSameObjectDelegate);

            handle->NewLocalRef = (void*)Marshal.GetFunctionPointerForDelegate(NewLocalRefDelegate);
            handle->EnsureLocalCapacity = (void*)Marshal.GetFunctionPointerForDelegate(EnsureLocalCapacityDelegate);
            
            handle->AllocObject = (void*)Marshal.GetFunctionPointerForDelegate(AllocObjectDelegate);
            handle->NewObject = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_NewObject");
            handle->NewObjectV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_NewObjectV");
            handle->NewObjectA = (void*)Marshal.GetFunctionPointerForDelegate(NewObjectADelegate);

            handle->GetObjectClass = (void*)Marshal.GetFunctionPointerForDelegate(GetObjectClassDelegate);
            handle->IsInstanceOf = (void*)Marshal.GetFunctionPointerForDelegate(IsInstanceOfDelegate);

            handle->GetMethodID = (void*)Marshal.GetFunctionPointerForDelegate(GetMethodIDDelegate);

            handle->CallObjectMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallObjectMethod");
            handle->CallObjectMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallObjectMethodV");
            handle->CallObjectMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallObjectMethodADelegate);

            handle->CallBooleanMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallBooleanMethod");
            handle->CallBooleanMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallBooleanMethodV");
            handle->CallBooleanMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallBooleanMethodADelegate);

            handle->CallByteMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallByteMethod");
            handle->CallByteMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallByteMethodV");
            handle->CallByteMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallByteMethodADelegate);

            handle->CallCharMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallCharMethod");
            handle->CallCharMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallCharMethodV");
            handle->CallCharMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallCharMethodADelegate);

            handle->CallShortMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallShortMethod");
            handle->CallShortMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallShortMethodV");
            handle->CallShortMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallShortMethodADelegate);

            handle->CallIntMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallIntMethod");
            handle->CallIntMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallIntMethodV");
            handle->CallIntMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallIntMethodADelegate);

            handle->CallLongMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallLongMethod");
            handle->CallLongMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallLongMethodV");
            handle->CallLongMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallLongMethodADelegate);

            handle->CallFloatMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallFloatMethod");
            handle->CallFloatMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallFloatMethodV");
            handle->CallFloatMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallFloatMethodADelegate);

            handle->CallDoubleMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallDoubleMethod");
            handle->CallDoubleMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallDoubleMethodV");
            handle->CallDoubleMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallDoubleMethodADelegate);

            handle->CallVoidMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallVoidMethod");
            handle->CallVoidMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallVoidMethodV");
            handle->CallVoidMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallVoidMethodADelegate);

            handle->CallNonvirtualObjectMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallNonvirtualObjectMethod");
            handle->CallNonvirtualObjectMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallNonvirtualObjectMethodV");
            handle->CallNonvirtualObjectMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallNonvirtualObjectMethodADelegate);

            handle->CallNonvirtualBooleanMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallNonvirtualBooleanMethod");
            handle->CallNonvirtualBooleanMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallNonvirtualBooleanMethodV");
            handle->CallNonvirtualBooleanMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallNonvirtualBooleanMethodADelegate);

            handle->CallNonvirtualByteMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallNonvirtualByteMethod");
            handle->CallNonvirtualByteMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallNonvirtualByteMethodV");
            handle->CallNonvirtualByteMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallNonvirtualByteMethodADelegate);

            handle->CallNonvirtualCharMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallNonvirtualCharMethod");
            handle->CallNonvirtualCharMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallNonvirtualCharMethodV");
            handle->CallNonvirtualCharMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallNonvirtualCharMethodADelegate);

            handle->CallNonvirtualShortMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallNonvirtualShortMethod");
            handle->CallNonvirtualShortMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallNonvirtualShortMethodV");
            handle->CallNonvirtualShortMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallNonvirtualShortMethodADelegate);

            handle->CallNonvirtualIntMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallNonvirtualIntMethod");
            handle->CallNonvirtualIntMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallNonvirtualIntMethodV");
            handle->CallNonvirtualIntMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallNonvirtualIntMethodADelegate);

            handle->CallNonvirtualLongMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallNonvirtualLongMethod");
            handle->CallNonvirtualLongMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallNonvirtualLongMethodV");
            handle->CallNonvirtualLongMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallNonvirtualLongMethodADelegate);

            handle->CallNonvirtualFloatMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallNonvirtualFloatMethod");
            handle->CallNonvirtualFloatMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallNonvirtualFloatMethodV");
            handle->CallNonvirtualFloatMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallNonvirtualFloatMethodADelegate);

            handle->CallNonvirtualDoubleMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallNonvirtualDoubleMethod");
            handle->CallNonvirtualDoubleMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallNonvirtualDoubleMethodV");
            handle->CallNonvirtualDoubleMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallNonvirtualDoubleMethodADelegate);

            handle->CallNonvirtualVoidMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallNonvirtualVoidMethod");
            handle->CallNonvirtualVoidMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallNonvirtualVoidMethodV");
            handle->CallNonvirtualVoidMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallNonvirtualVoidMethodADelegate);

            handle->GetFieldID = (void*)Marshal.GetFunctionPointerForDelegate(GetFieldIDDelegate);

            handle->GetObjectField = (void*)Marshal.GetFunctionPointerForDelegate(GetObjectFieldDelegate);
            handle->GetBooleanField = (void*)Marshal.GetFunctionPointerForDelegate(GetBooleanFieldDelegate);
            handle->GetByteField = (void*)Marshal.GetFunctionPointerForDelegate(GetByteFieldDelegate);
            handle->GetCharField = (void*)Marshal.GetFunctionPointerForDelegate(GetCharFieldDelegate);
            handle->GetShortField = (void*)Marshal.GetFunctionPointerForDelegate(GetShortFieldDelegate);
            handle->GetIntField = (void*)Marshal.GetFunctionPointerForDelegate(GetIntFieldDelegate);
            handle->GetLongField = (void*)Marshal.GetFunctionPointerForDelegate(GetLongFieldDelegate);
            handle->GetFloatField = (void*)Marshal.GetFunctionPointerForDelegate(GetFloatFieldDelegate);
            handle->GetDoubleField = (void*)Marshal.GetFunctionPointerForDelegate(GetDoubleFieldDelegate);

            handle->SetObjectField = (void*)Marshal.GetFunctionPointerForDelegate(SetObjectFieldDelegate);
            handle->SetBooleanField = (void*)Marshal.GetFunctionPointerForDelegate(SetBooleanFieldDelegate);
            handle->SetByteField = (void*)Marshal.GetFunctionPointerForDelegate(SetByteFieldDelegate);
            handle->SetCharField = (void*)Marshal.GetFunctionPointerForDelegate(SetCharFieldDelegate);
            handle->SetShortField = (void*)Marshal.GetFunctionPointerForDelegate(SetShortFieldDelegate);
            handle->SetIntField = (void*)Marshal.GetFunctionPointerForDelegate(SetIntFieldDelegate);
            handle->SetLongField = (void*)Marshal.GetFunctionPointerForDelegate(SetLongFieldDelegate);
            handle->SetFloatField = (void*)Marshal.GetFunctionPointerForDelegate(SetFloatFieldDelegate);
            handle->SetDoubleField = (void*)Marshal.GetFunctionPointerForDelegate(SetDoubleFieldDelegate);

            handle->GetStaticMethodID = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticMethodIDDelegate);

            handle->CallStaticObjectMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallStaticObjectMethod");
            handle->CallStaticObjectMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallStaticObjectMethodV");
            handle->CallStaticObjectMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallStaticObjectMethodADelegate);

            handle->CallStaticBooleanMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallStaticBooleanMethod");
            handle->CallStaticBooleanMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallStaticBooleanMethodV");
            handle->CallStaticBooleanMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallStaticBooleanMethodADelegate);

            handle->CallStaticByteMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallStaticByteMethod");
            handle->CallStaticByteMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallStaticByteMethodV");
            handle->CallStaticByteMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallStaticByteMethodADelegate);

            handle->CallStaticCharMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallStaticCharMethod");
            handle->CallStaticCharMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallStaticCharMethodV");
            handle->CallStaticCharMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallStaticCharMethodADelegate);

            handle->CallStaticShortMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallStaticShortMethod");
            handle->CallStaticShortMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallStaticShortMethodV");
            handle->CallStaticShortMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallStaticShortMethodADelegate);

            handle->CallStaticIntMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallStaticIntMethod");
            handle->CallStaticIntMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallStaticIntMethodV");
            handle->CallStaticIntMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallStaticIntMethodADelegate);

            handle->CallStaticLongMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallStaticLongMethod");
            handle->CallStaticLongMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallStaticLongMethodV");
            handle->CallStaticLongMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallStaticLongMethodADelegate);

            handle->CallStaticFloatMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallStaticFloatMethod");
            handle->CallStaticFloatMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallStaticFloatMethodV");
            handle->CallStaticFloatMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallStaticFloatMethodADelegate);

            handle->CallStaticDoubleMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallStaticDoubleMethod");
            handle->CallStaticDoubleMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallStaticDoubleMethodV");
            handle->CallStaticDoubleMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallStaticDoubleMethodADelegate);

            handle->CallStaticVoidMethod = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallStaticVoidMethod");
            handle->CallStaticVoidMethodV = (void*)NativeLibrary.GetExport(libjvm.Handle, "__JNI_CallStaticVoidMethodV");
            handle->CallStaticVoidMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallStaticVoidMethodADelegate);

            handle->GetStaticFieldID = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticFieldIDDelegate);

            handle->GetStaticObjectField = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticObjectFieldDelegate);
            handle->GetStaticBooleanField = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticBooleanFieldDelegate);
            handle->GetStaticByteField = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticByteFieldDelegate);
            handle->GetStaticCharField = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticCharFieldDelegate);
            handle->GetStaticShortField = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticShortFieldDelegate);
            handle->GetStaticIntField = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticIntFieldDelegate);
            handle->GetStaticLongField = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticLongFieldDelegate);
            handle->GetStaticFloatField = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticFloatFieldDelegate);
            handle->GetStaticDoubleField = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticDoubleFieldDelegate);

            handle->SetStaticObjectField = (void*)Marshal.GetFunctionPointerForDelegate(SetStaticObjectFieldDelegate);
            handle->SetStaticBooleanField = (void*)Marshal.GetFunctionPointerForDelegate(SetStaticBooleanFieldDelegate);
            handle->SetStaticByteField = (void*)Marshal.GetFunctionPointerForDelegate(SetStaticByteFieldDelegate);
            handle->SetStaticCharField = (void*)Marshal.GetFunctionPointerForDelegate(SetStaticCharFieldDelegate);
            handle->SetStaticShortField = (void*)Marshal.GetFunctionPointerForDelegate(SetStaticShortFieldDelegate);
            handle->SetStaticIntField = (void*)Marshal.GetFunctionPointerForDelegate(SetStaticIntFieldDelegate);
            handle->SetStaticLongField = (void*)Marshal.GetFunctionPointerForDelegate(SetStaticLongFieldDelegate);
            handle->SetStaticFloatField = (void*)Marshal.GetFunctionPointerForDelegate(SetStaticFloatFieldDelegate);
            handle->SetStaticDoubleField = (void*)Marshal.GetFunctionPointerForDelegate(SetStaticDoubleFieldDelegate);

            handle->NewString = (void*)Marshal.GetFunctionPointerForDelegate(NewStringDelegate);
            handle->GetStringLength = (void*)Marshal.GetFunctionPointerForDelegate(GetStringLengthDelegate);
            handle->GetStringChars = (void*)Marshal.GetFunctionPointerForDelegate(GetStringCharsDelegate);
            handle->ReleaseStringChars = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseStringCharsDelegate);

            handle->NewStringUTF = (void*)Marshal.GetFunctionPointerForDelegate(NewStringUTFDelegate);
            handle->GetStringUTFLength = (void*)Marshal.GetFunctionPointerForDelegate(GetStringUTFLengthDelegate);
            handle->GetStringUTFChars = (void*)Marshal.GetFunctionPointerForDelegate(GetStringUTFCharsDelegate);
            handle->ReleaseStringUTFChars = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseStringUTFCharsDelegate);

            handle->GetArrayLength = (void*)Marshal.GetFunctionPointerForDelegate(GetArrayLengthDelegate);

            handle->NewObjectArray = (void*)Marshal.GetFunctionPointerForDelegate(NewObjectArrayDelegate);
            handle->GetObjectArrayElement = (void*)Marshal.GetFunctionPointerForDelegate(GetObjectArrayElementDelegate);
            handle->SetObjectArrayElement = (void*)Marshal.GetFunctionPointerForDelegate(SetObjectArrayElementDelegate);

            handle->NewBooleanArray = (void*)Marshal.GetFunctionPointerForDelegate(NewBooleanArrayDelegate);
            handle->NewByteArray = (void*)Marshal.GetFunctionPointerForDelegate(NewByteArrayDelegate);
            handle->NewCharArray = (void*)Marshal.GetFunctionPointerForDelegate(NewCharArrayDelegate);
            handle->NewShortArray = (void*)Marshal.GetFunctionPointerForDelegate(NewShortArrayDelegate);
            handle->NewIntArray = (void*)Marshal.GetFunctionPointerForDelegate(NewIntArrayDelegate);
            handle->NewLongArray = (void*)Marshal.GetFunctionPointerForDelegate(NewLongArrayDelegate);
            handle->NewFloatArray = (void*)Marshal.GetFunctionPointerForDelegate(NewFloatArrayDelegate);
            handle->NewDoubleArray = (void*)Marshal.GetFunctionPointerForDelegate(NewDoubleArrayDelegate);

            handle->GetBooleanArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(GetBooleanArrayElementsDelegate);
            handle->GetByteArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(GetByteArrayElementsDelegate);
            handle->GetCharArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(GetCharArrayElementsDelegate);
            handle->GetShortArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(GetShortArrayElementsDelegate);
            handle->GetIntArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(GetIntArrayElementsDelegate);
            handle->GetLongArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(GetLongArrayElementsDelegate);
            handle->GetFloatArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(GetFloatArrayElementsDelegate);
            handle->GetDoubleArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(GetDoubleArrayElementsDelegate);

            handle->ReleaseBooleanArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseBooleanArrayElementsDelegate);
            handle->ReleaseByteArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseByteArrayElementsDelegate);
            handle->ReleaseCharArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseCharArrayElementsDelegate);
            handle->ReleaseShortArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseShortArrayElementsDelegate);
            handle->ReleaseIntArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseIntArrayElementsDelegate);
            handle->ReleaseLongArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseLongArrayElementsDelegate);
            handle->ReleaseFloatArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseFloatArrayElementsDelegate);
            handle->ReleaseDoubleArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseDoubleArrayElementsDelegate);

            handle->GetBooleanArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(GetBooleanArrayRegionDelegate);
            handle->GetByteArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(GetByteArrayRegionDelegate);
            handle->GetCharArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(GetCharArrayRegionDelegate);
            handle->GetShortArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(GetShortArrayRegionDelegate);
            handle->GetIntArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(GetIntArrayRegionDelegate);
            handle->GetLongArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(GetLongArrayRegionDelegate);
            handle->GetFloatArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(GetFloatArrayRegionDelegate);
            handle->GetDoubleArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(GetDoubleArrayRegionDelegate);

            handle->SetBooleanArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(SetBooleanArrayRegionDelegate);
            handle->SetByteArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(SetByteArrayRegionDelegate);
            handle->SetCharArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(SetCharArrayRegionDelegate);
            handle->SetShortArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(SetShortArrayRegionDelegate);
            handle->SetIntArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(SetIntArrayRegionDelegate);
            handle->SetLongArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(SetLongArrayRegionDelegate);
            handle->SetFloatArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(SetFloatArrayRegionDelegate);
            handle->SetDoubleArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(SetDoubleArrayRegionDelegate);

            handle->RegisterNatives = (void*)Marshal.GetFunctionPointerForDelegate(RegisterNativesDelegate);
            handle->UnregisterNatives = (void*)Marshal.GetFunctionPointerForDelegate(UnregisterNativesDelegate);

            handle->MonitorEnter = (void*)Marshal.GetFunctionPointerForDelegate(MonitorEnterDelegate);
            handle->MonitorExit = (void*)Marshal.GetFunctionPointerForDelegate(MonitorExitDelegate);

            handle->GetJavaVM = (void*)Marshal.GetFunctionPointerForDelegate(GetJavaVMDelegate);

            handle->GetStringRegion = (void*)Marshal.GetFunctionPointerForDelegate(GetStringRegionDelegate);
            handle->GetStringUTFRegion = (void*)Marshal.GetFunctionPointerForDelegate(GetStringUTFRegionDelegate);

            handle->GetPrimitiveArrayCritical = (void*)Marshal.GetFunctionPointerForDelegate(GetPrimitiveArrayCriticalDelegate);
            handle->ReleasePrimitiveArrayCritical = (void*)Marshal.GetFunctionPointerForDelegate(ReleasePrimitiveArrayCriticalDelegate);

            handle->GetStringCritical = (void*)Marshal.GetFunctionPointerForDelegate(GetStringCriticalDelegate);
            handle->ReleaseStringCritical = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseStringCriticalDelegate);

            handle->NewWeakGlobalRef = (void*)Marshal.GetFunctionPointerForDelegate(NewWeakGlobalRefDelegate);
            handle->DeleteWeakGlobalRef = (void*)Marshal.GetFunctionPointerForDelegate(DeleteWeakGlobalRefDelegate);

            handle->ExceptionCheck = (void*)Marshal.GetFunctionPointerForDelegate(ExceptionCheckDelegate);

            handle->NewDirectByteBuffer = (void*)Marshal.GetFunctionPointerForDelegate(NewDirectByteBufferDelegate);
            handle->GetDirectBufferAddress = (void*)Marshal.GetFunctionPointerForDelegate(GetDirectBufferAddressDelegate);
            handle->GetDirectBufferCapacity = (void*)Marshal.GetFunctionPointerForDelegate(GetDirectBufferCapacityDelegate);

            handle->GetObjectRefType = (void*)Marshal.GetFunctionPointerForDelegate(GetObjectRefTypeDelegate);
        }

        public void* GetMethodArgs;
        public void* reserved1;
        public void* reserved2;

        public void* reserved3;
        public void* GetVersion;
        public void* DefineClass;
        public void* FindClass;

        public void* FromReflectedMethod;
        public void* FromReflectedField;

        public void* ToReflectedMethod;

        public void* GetSuperclass;
        public void* IsAssignableFrom;

        public void* ToReflectedField;

        public void* Throw;
        public void* ThrowNew;
        public void* ExceptionOccurred;
        public void* ExceptionDescribe;
        public void* ExceptionClear;
        public void* FatalError;

        public void* PushLocalFrame;
        public void* PopLocalFrame;

        public void* NewGlobalRef;
        public void* DeleteGlobalRef;
        public void* DeleteLocalRef;
        public void* IsSameObject;
        public void* NewLocalRef;
        public void* EnsureLocalCapacity;

        public void* AllocObject;
        public void* NewObject;
        public void* NewObjectV;
        public void* NewObjectA;

        public void* GetObjectClass;
        public void* IsInstanceOf;

        public void* GetMethodID;

        public void* CallObjectMethod;
        public void* CallObjectMethodV;
        public void* CallObjectMethodA;

        public void* CallBooleanMethod;
        public void* CallBooleanMethodV;
        public void* CallBooleanMethodA;

        public void* CallByteMethod;
        public void* CallByteMethodV;
        public void* CallByteMethodA;

        public void* CallCharMethod;
        public void* CallCharMethodV;
        public void* CallCharMethodA;

        public void* CallShortMethod;
        public void* CallShortMethodV;
        public void* CallShortMethodA;

        public void* CallIntMethod;
        public void* CallIntMethodV;
        public void* CallIntMethodA;

        public void* CallLongMethod;
        public void* CallLongMethodV;
        public void* CallLongMethodA;

        public void* CallFloatMethod;
        public void* CallFloatMethodV;
        public void* CallFloatMethodA;

        public void* CallDoubleMethod;
        public void* CallDoubleMethodV;
        public void* CallDoubleMethodA;

        public void* CallVoidMethod;
        public void* CallVoidMethodV;
        public void* CallVoidMethodA;

        public void* CallNonvirtualObjectMethod;
        public void* CallNonvirtualObjectMethodV;
        public void* CallNonvirtualObjectMethodA;

        public void* CallNonvirtualBooleanMethod;
        public void* CallNonvirtualBooleanMethodV;
        public void* CallNonvirtualBooleanMethodA;

        public void* CallNonvirtualByteMethod;
        public void* CallNonvirtualByteMethodV;
        public void* CallNonvirtualByteMethodA;

        public void* CallNonvirtualCharMethod;
        public void* CallNonvirtualCharMethodV;
        public void* CallNonvirtualCharMethodA;

        public void* CallNonvirtualShortMethod;
        public void* CallNonvirtualShortMethodV;
        public void* CallNonvirtualShortMethodA;

        public void* CallNonvirtualIntMethod;
        public void* CallNonvirtualIntMethodV;
        public void* CallNonvirtualIntMethodA;

        public void* CallNonvirtualLongMethod;
        public void* CallNonvirtualLongMethodV;
        public void* CallNonvirtualLongMethodA;

        public void* CallNonvirtualFloatMethod;
        public void* CallNonvirtualFloatMethodV;
        public void* CallNonvirtualFloatMethodA;

        public void* CallNonvirtualDoubleMethod;
        public void* CallNonvirtualDoubleMethodV;
        public void* CallNonvirtualDoubleMethodA;

        public void* CallNonvirtualVoidMethod;
        public void* CallNonvirtualVoidMethodV;
        public void* CallNonvirtualVoidMethodA;

        public void* GetFieldID;

        public void* GetObjectField;
        public void* GetBooleanField;
        public void* GetByteField;
        public void* GetCharField;
        public void* GetShortField;
        public void* GetIntField;
        public void* GetLongField;
        public void* GetFloatField;
        public void* GetDoubleField;

        public void* SetObjectField;
        public void* SetBooleanField;
        public void* SetByteField;
        public void* SetCharField;
        public void* SetShortField;
        public void* SetIntField;
        public void* SetLongField;
        public void* SetFloatField;
        public void* SetDoubleField;

        public void* GetStaticMethodID;

        public void* CallStaticObjectMethod;
        public void* CallStaticObjectMethodV;
        public void* CallStaticObjectMethodA;

        public void* CallStaticBooleanMethod;
        public void* CallStaticBooleanMethodV;
        public void* CallStaticBooleanMethodA;

        public void* CallStaticByteMethod;
        public void* CallStaticByteMethodV;
        public void* CallStaticByteMethodA;

        public void* CallStaticCharMethod;
        public void* CallStaticCharMethodV;
        public void* CallStaticCharMethodA;

        public void* CallStaticShortMethod;
        public void* CallStaticShortMethodV;
        public void* CallStaticShortMethodA;

        public void* CallStaticIntMethod;
        public void* CallStaticIntMethodV;
        public void* CallStaticIntMethodA;

        public void* CallStaticLongMethod;
        public void* CallStaticLongMethodV;
        public void* CallStaticLongMethodA;

        public void* CallStaticFloatMethod;
        public void* CallStaticFloatMethodV;
        public void* CallStaticFloatMethodA;

        public void* CallStaticDoubleMethod;
        public void* CallStaticDoubleMethodV;
        public void* CallStaticDoubleMethodA;

        public void* CallStaticVoidMethod;
        public void* CallStaticVoidMethodV;
        public void* CallStaticVoidMethodA;

        public void* GetStaticFieldID;
        public void* GetStaticObjectField;
        public void* GetStaticBooleanField;
        public void* GetStaticByteField;
        public void* GetStaticCharField;
        public void* GetStaticShortField;
        public void* GetStaticIntField;
        public void* GetStaticLongField;
        public void* GetStaticFloatField;
        public void* GetStaticDoubleField;

        public void* SetStaticObjectField;
        public void* SetStaticBooleanField;
        public void* SetStaticByteField;
        public void* SetStaticCharField;
        public void* SetStaticShortField;
        public void* SetStaticIntField;
        public void* SetStaticLongField;
        public void* SetStaticFloatField;
        public void* SetStaticDoubleField;

        public void* NewString;
        public void* GetStringLength;
        public void* GetStringChars;
        public void* ReleaseStringChars;

        public void* NewStringUTF;
        public void* GetStringUTFLength;
        public void* GetStringUTFChars;
        public void* ReleaseStringUTFChars;

        public void* GetArrayLength;

        public void* NewObjectArray;
        public void* GetObjectArrayElement;
        public void* SetObjectArrayElement;

        public void* NewBooleanArray;
        public void* NewByteArray;
        public void* NewCharArray;
        public void* NewShortArray;
        public void* NewIntArray;
        public void* NewLongArray;
        public void* NewFloatArray;
        public void* NewDoubleArray;

        public void* GetBooleanArrayElements;
        public void* GetByteArrayElements;
        public void* GetCharArrayElements;
        public void* GetShortArrayElements;
        public void* GetIntArrayElements;
        public void* GetLongArrayElements;
        public void* GetFloatArrayElements;
        public void* GetDoubleArrayElements;

        public void* ReleaseBooleanArrayElements;
        public void* ReleaseByteArrayElements;
        public void* ReleaseCharArrayElements;
        public void* ReleaseShortArrayElements;
        public void* ReleaseIntArrayElements;
        public void* ReleaseLongArrayElements;
        public void* ReleaseFloatArrayElements;
        public void* ReleaseDoubleArrayElements;

        public void* GetBooleanArrayRegion;
        public void* GetByteArrayRegion;
        public void* GetCharArrayRegion;
        public void* GetShortArrayRegion;
        public void* GetIntArrayRegion;
        public void* GetLongArrayRegion;
        public void* GetFloatArrayRegion;
        public void* GetDoubleArrayRegion;

        public void* SetBooleanArrayRegion;
        public void* SetByteArrayRegion;
        public void* SetCharArrayRegion;
        public void* SetShortArrayRegion;
        public void* SetIntArrayRegion;
        public void* SetLongArrayRegion;
        public void* SetFloatArrayRegion;
        public void* SetDoubleArrayRegion;

        public void* RegisterNatives;
        public void* UnregisterNatives;

        public void* MonitorEnter;
        public void* MonitorExit;

        public void* GetJavaVM;

        public void* GetStringRegion;
        public void* GetStringUTFRegion;

        public void* GetPrimitiveArrayCritical;
        public void* ReleasePrimitiveArrayCritical;

        public void* GetStringCritical;
        public void* ReleaseStringCritical;

        public void* NewWeakGlobalRef;
        public void* DeleteWeakGlobalRef;

        public void* ExceptionCheck;

        public void* NewDirectByteBuffer;
        public void* GetDirectBufferAddress;
        public void* GetDirectBufferCapacity;

        public void* GetObjectRefType;

    }

}
