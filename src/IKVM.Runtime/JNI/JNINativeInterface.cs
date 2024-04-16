using System;
using System.Runtime.InteropServices;

using Microsoft.Win32.SafeHandles;

namespace IKVM.Runtime.JNI
{

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false

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
        /// Maintains a <see cref="JNINativeInterface"/> structure in memory.
        /// </summary>
        class JNINativeInterfaceHandle : SafeHandleZeroOrMinusOneIsInvalid
        {

            /// <summary>
            /// Gets a reference to the JNINativeInterface instance.
            /// </summary>
            public static readonly JNINativeInterfaceHandle Instance = new();

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            public JNINativeInterfaceHandle() :
                base(true)
            {
                SetHandle(Marshal.AllocHGlobal(sizeof(JNINativeInterface)));
            }

            /// <summary>
            /// Gets the handle to the JNIEnv structure.
            /// </summary>
            public JNINativeInterface* Handle => (JNINativeInterface*)DangerousGetHandle();

            /// <summary>
            /// Releases the handle.
            /// </summary>
            /// <returns></returns>
            protected override bool ReleaseHandle()
            {
                Marshal.FreeHGlobal(handle);
                return true;
            }

        }

        #region Delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int GetMethodArgsDelegateType(JNIEnv* pEnv, jmethodID methodID, byte* sig);

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
        delegate jobject NewObjectADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, JValue* args);

        delegate jclass GetObjectClassDelegateType(JNIEnv* pEnv, jobject obj);
        delegate jboolean IsInstanceOfDelegateType(JNIEnv* pEnv, jobject obj, jclass clazz);

        delegate jmethodID GetMethodIDDelegateType(JNIEnv* pEnv, jclass clazz, byte* name, byte* sig);

        delegate jobject CallObjectMethodADelegateType(JNIEnv* pEnv, jobject obj, jmethodID methodID, JValue* args);
        delegate jboolean CallBooleanMethodADelegateType(JNIEnv* pEnv, jobject obj, jmethodID methodID, JValue* args);
        delegate jbyte CallByteMethodADelegateType(JNIEnv* pEnv, jobject obj, jmethodID methodID, JValue* args);
        delegate jchar CallCharMethodADelegateType(JNIEnv* pEnv, jobject obj, jmethodID methodID, JValue* args);
        delegate jshort CallShortMethodADelegateType(JNIEnv* pEnv, jobject obj, jmethodID methodID, JValue* args);
        delegate jint CallIntMethodADelegateType(JNIEnv* pEnv, jobject obj, jmethodID methodID, JValue* args);
        delegate jlong CallLongMethodADelegateType(JNIEnv* pEnv, jobject obj, jmethodID methodID, JValue* args);
        delegate jfloat CallFloatMethodADelegateType(JNIEnv* pEnv, jobject obj, jmethodID methodID, JValue* args);
        delegate jdouble CallDoubleMethodADelegateType(JNIEnv* pEnv, jobject obj, jmethodID methodID, JValue* args);
        delegate void CallVoidMethodADelegateType(JNIEnv* pEnv, jobject obj, jmethodID methodID, JValue* args);

        delegate jobject CallNonvirtualObjectMethodADelegateType(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, JValue* args);
        delegate jboolean CallNonvirtualBooleanMethodADelegateType(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, JValue* args);
        delegate jbyte CallNonvirtualByteMethodADelegateType(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, JValue* args);
        delegate jchar CallNonvirtualCharMethodADelegateType(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, JValue* args);
        delegate jshort CallNonvirtualShortMethodADelegateType(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, JValue* args);
        delegate jint CallNonvirtualIntMethodADelegateType(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, JValue* args);
        delegate jlong CallNonvirtualLongMethodADelegateType(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, JValue* args);
        delegate jfloat CallNonvirtualFloatMethodADelegateType(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, JValue* args);
        delegate jdouble CallNonvirtualDoubleMethodADelegateType(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, JValue* args);
        delegate void CallNonvirtualVoidMethodADelegateType(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, JValue* args);

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

        delegate jobject CallStaticObjectMethodADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, JValue* args);
        delegate jboolean CallStaticBooleanMethodADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, JValue* args);
        delegate jbyte CallStaticByteMethodADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, JValue* args);
        delegate jchar CallStaticCharMethodADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, JValue* args);
        delegate jshort CallStaticShortMethodADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, JValue* args);
        delegate jint CallStaticIntMethodADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, JValue* args);
        delegate jlong CallStaticLongMethodADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, JValue* args);
        delegate jfloat CallStaticFloatMethodADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, JValue* args);
        delegate jdouble CallStaticDoubleMethodADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, JValue* args);
        delegate void CallStaticVoidMethodADelegateType(JNIEnv* pEnv, jclass clazz, jmethodID methodID, JValue* args);

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

        delegate JObjectRefType GetObjectRefTypeDelegateType(JNIEnv* pEnv, jobject obj);

        #endregion

        static readonly LibJvm libjvm = LibJvm.Instance;
        static readonly JNINativeInterfaceHandle handle = JNINativeInterfaceHandle.Instance;

        /// <summary>
        /// Gets a pointer to the JNINativeInterface table.
        /// </summary>
        public static JNINativeInterface* Handle => handle.Handle;

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
        /// Gets the native handle for a function pointer in libjvm.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="argl"></param>
        /// <returns></returns>
        static nint GetLibJvmFunctionHandle(string name, int argl = -1)
        {
            if (libjvm.Handle.GetExport(name, argl).Handle is nint h and not 0)
                return h;
            else
                throw new InternalException($"Cannot find libjvm native method '{name}'.");
        }

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static JNINativeInterface()
        {
            JNIVM.jvmCreated = true;

            Handle->reserved0 = (void*)Marshal.GetFunctionPointerForDelegate(GetMethodArgsDelegate);
            Handle->reserved1 = null;
            Handle->reserved2 = null;

            Handle->reserved3 = null;
            Handle->GetVersion = (void*)Marshal.GetFunctionPointerForDelegate(GetVersionDelegate);

            Handle->DefineClass = (void*)Marshal.GetFunctionPointerForDelegate(DefineClassDelegate);
            Handle->FindClass = (void*)Marshal.GetFunctionPointerForDelegate(FindClassDelegate);

            Handle->FromReflectedMethod = (void*)Marshal.GetFunctionPointerForDelegate(FromReflectedMethodDelegate);
            Handle->FromReflectedField = (void*)Marshal.GetFunctionPointerForDelegate(FromReflectedFieldDelegate);
            Handle->ToReflectedMethod = (void*)Marshal.GetFunctionPointerForDelegate(ToReflectedMethodDelegate);

            Handle->GetSuperclass = (void*)Marshal.GetFunctionPointerForDelegate(GetSuperclassDelegate);
            Handle->IsAssignableFrom = (void*)Marshal.GetFunctionPointerForDelegate(IsAssignableFromDelegate);

            Handle->ToReflectedField = (void*)Marshal.GetFunctionPointerForDelegate(ToReflectedFieldDelegate);

            Handle->Throw = (void*)Marshal.GetFunctionPointerForDelegate(ThrowDelegate);
            Handle->ThrowNew = (void*)Marshal.GetFunctionPointerForDelegate(ThrowNewDelegate);
            Handle->ExceptionOccurred = (void*)Marshal.GetFunctionPointerForDelegate(ExceptionOccurredDelegate);
            Handle->ExceptionDescribe = (void*)Marshal.GetFunctionPointerForDelegate(ExceptionDescribeDelegate);
            Handle->ExceptionClear = (void*)Marshal.GetFunctionPointerForDelegate(ExceptionClearDelegate);
            Handle->FatalError = (void*)Marshal.GetFunctionPointerForDelegate(FatalErrorDelegate);

            Handle->PushLocalFrame = (void*)Marshal.GetFunctionPointerForDelegate(PushLocalFrameDelegate);
            Handle->PopLocalFrame = (void*)Marshal.GetFunctionPointerForDelegate(PopLocalFrameDelegate);

            Handle->NewGlobalRef = (void*)Marshal.GetFunctionPointerForDelegate(NewGlobalRefDelegate);
            Handle->DeleteGlobalRef = (void*)Marshal.GetFunctionPointerForDelegate(DeleteGlobalRefDelegate);
            Handle->DeleteLocalRef = (void*)Marshal.GetFunctionPointerForDelegate(DeleteLocalRefDelegate);
            Handle->IsSameObject = (void*)Marshal.GetFunctionPointerForDelegate(IsSameObjectDelegate);

            Handle->NewLocalRef = (void*)Marshal.GetFunctionPointerForDelegate(NewLocalRefDelegate);
            Handle->EnsureLocalCapacity = (void*)Marshal.GetFunctionPointerForDelegate(EnsureLocalCapacityDelegate);

            Handle->AllocObject = (void*)Marshal.GetFunctionPointerForDelegate(AllocObjectDelegate);
            Handle->NewObject = (void*)GetLibJvmFunctionHandle("__JNI_NewObject");
            Handle->NewObjectV = (void*)GetLibJvmFunctionHandle("__JNI_NewObjectV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->NewObjectA = (void*)Marshal.GetFunctionPointerForDelegate(NewObjectADelegate);

            Handle->GetObjectClass = (void*)Marshal.GetFunctionPointerForDelegate(GetObjectClassDelegate);
            Handle->IsInstanceOf = (void*)Marshal.GetFunctionPointerForDelegate(IsInstanceOfDelegate);

            Handle->GetMethodID = (void*)Marshal.GetFunctionPointerForDelegate(GetMethodIDDelegate);

            Handle->CallObjectMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallObjectMethod");
            Handle->CallObjectMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallObjectMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallObjectMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallObjectMethodADelegate);

            Handle->CallBooleanMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallBooleanMethod");
            Handle->CallBooleanMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallBooleanMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallBooleanMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallBooleanMethodADelegate);

            Handle->CallByteMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallByteMethod");
            Handle->CallByteMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallByteMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallByteMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallByteMethodADelegate);

            Handle->CallCharMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallCharMethod");
            Handle->CallCharMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallCharMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallCharMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallCharMethodADelegate);

            Handle->CallShortMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallShortMethod");
            Handle->CallShortMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallShortMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallShortMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallShortMethodADelegate);

            Handle->CallIntMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallIntMethod");
            Handle->CallIntMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallIntMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallIntMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallIntMethodADelegate);

            Handle->CallLongMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallLongMethod");
            Handle->CallLongMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallLongMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallLongMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallLongMethodADelegate);

            Handle->CallFloatMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallFloatMethod");
            Handle->CallFloatMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallFloatMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallFloatMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallFloatMethodADelegate);

            Handle->CallDoubleMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallDoubleMethod");
            Handle->CallDoubleMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallDoubleMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallDoubleMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallDoubleMethodADelegate);

            Handle->CallVoidMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallVoidMethod");
            Handle->CallVoidMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallVoidMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallVoidMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallVoidMethodADelegate);

            Handle->CallNonvirtualObjectMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallNonvirtualObjectMethod");
            Handle->CallNonvirtualObjectMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallNonvirtualObjectMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jobject) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallNonvirtualObjectMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallNonvirtualObjectMethodADelegate);

            Handle->CallNonvirtualBooleanMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallNonvirtualBooleanMethod");
            Handle->CallNonvirtualBooleanMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallNonvirtualBooleanMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jobject) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallNonvirtualBooleanMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallNonvirtualBooleanMethodADelegate);

            Handle->CallNonvirtualByteMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallNonvirtualByteMethod");
            Handle->CallNonvirtualByteMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallNonvirtualByteMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jobject) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallNonvirtualByteMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallNonvirtualByteMethodADelegate);

            Handle->CallNonvirtualCharMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallNonvirtualCharMethod");
            Handle->CallNonvirtualCharMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallNonvirtualCharMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jobject) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallNonvirtualCharMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallNonvirtualCharMethodADelegate);

            Handle->CallNonvirtualShortMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallNonvirtualShortMethod");
            Handle->CallNonvirtualShortMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallNonvirtualShortMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jobject) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallNonvirtualShortMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallNonvirtualShortMethodADelegate);

            Handle->CallNonvirtualIntMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallNonvirtualIntMethod");
            Handle->CallNonvirtualIntMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallNonvirtualIntMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jobject) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallNonvirtualIntMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallNonvirtualIntMethodADelegate);

            Handle->CallNonvirtualLongMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallNonvirtualLongMethod");
            Handle->CallNonvirtualLongMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallNonvirtualLongMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jobject) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallNonvirtualLongMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallNonvirtualLongMethodADelegate);

            Handle->CallNonvirtualFloatMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallNonvirtualFloatMethod");
            Handle->CallNonvirtualFloatMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallNonvirtualFloatMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jobject) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallNonvirtualFloatMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallNonvirtualFloatMethodADelegate);

            Handle->CallNonvirtualDoubleMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallNonvirtualDoubleMethod");
            Handle->CallNonvirtualDoubleMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallNonvirtualDoubleMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jobject) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallNonvirtualDoubleMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallNonvirtualDoubleMethodADelegate);

            Handle->CallNonvirtualVoidMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallNonvirtualVoidMethod");
            Handle->CallNonvirtualVoidMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallNonvirtualVoidMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jobject) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallNonvirtualVoidMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallNonvirtualVoidMethodADelegate);

            Handle->GetFieldID = (void*)Marshal.GetFunctionPointerForDelegate(GetFieldIDDelegate);

            Handle->GetObjectField = (void*)Marshal.GetFunctionPointerForDelegate(GetObjectFieldDelegate);
            Handle->GetBooleanField = (void*)Marshal.GetFunctionPointerForDelegate(GetBooleanFieldDelegate);
            Handle->GetByteField = (void*)Marshal.GetFunctionPointerForDelegate(GetByteFieldDelegate);
            Handle->GetCharField = (void*)Marshal.GetFunctionPointerForDelegate(GetCharFieldDelegate);
            Handle->GetShortField = (void*)Marshal.GetFunctionPointerForDelegate(GetShortFieldDelegate);
            Handle->GetIntField = (void*)Marshal.GetFunctionPointerForDelegate(GetIntFieldDelegate);
            Handle->GetLongField = (void*)Marshal.GetFunctionPointerForDelegate(GetLongFieldDelegate);
            Handle->GetFloatField = (void*)Marshal.GetFunctionPointerForDelegate(GetFloatFieldDelegate);
            Handle->GetDoubleField = (void*)Marshal.GetFunctionPointerForDelegate(GetDoubleFieldDelegate);

            Handle->SetObjectField = (void*)Marshal.GetFunctionPointerForDelegate(SetObjectFieldDelegate);
            Handle->SetBooleanField = (void*)Marshal.GetFunctionPointerForDelegate(SetBooleanFieldDelegate);
            Handle->SetByteField = (void*)Marshal.GetFunctionPointerForDelegate(SetByteFieldDelegate);
            Handle->SetCharField = (void*)Marshal.GetFunctionPointerForDelegate(SetCharFieldDelegate);
            Handle->SetShortField = (void*)Marshal.GetFunctionPointerForDelegate(SetShortFieldDelegate);
            Handle->SetIntField = (void*)Marshal.GetFunctionPointerForDelegate(SetIntFieldDelegate);
            Handle->SetLongField = (void*)Marshal.GetFunctionPointerForDelegate(SetLongFieldDelegate);
            Handle->SetFloatField = (void*)Marshal.GetFunctionPointerForDelegate(SetFloatFieldDelegate);
            Handle->SetDoubleField = (void*)Marshal.GetFunctionPointerForDelegate(SetDoubleFieldDelegate);

            Handle->GetStaticMethodID = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticMethodIDDelegate);

            Handle->CallStaticObjectMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallStaticObjectMethod");
            Handle->CallStaticObjectMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallStaticObjectMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallStaticObjectMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallStaticObjectMethodADelegate);

            Handle->CallStaticBooleanMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallStaticBooleanMethod");
            Handle->CallStaticBooleanMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallStaticBooleanMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallStaticBooleanMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallStaticBooleanMethodADelegate);

            Handle->CallStaticByteMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallStaticByteMethod");
            Handle->CallStaticByteMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallStaticByteMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallStaticByteMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallStaticByteMethodADelegate);

            Handle->CallStaticCharMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallStaticCharMethod");
            Handle->CallStaticCharMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallStaticCharMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallStaticCharMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallStaticCharMethodADelegate);

            Handle->CallStaticShortMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallStaticShortMethod");
            Handle->CallStaticShortMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallStaticShortMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallStaticShortMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallStaticShortMethodADelegate);

            Handle->CallStaticIntMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallStaticIntMethod");
            Handle->CallStaticIntMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallStaticIntMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallStaticIntMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallStaticIntMethodADelegate);

            Handle->CallStaticLongMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallStaticLongMethod");
            Handle->CallStaticLongMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallStaticLongMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallStaticLongMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallStaticLongMethodADelegate);

            Handle->CallStaticFloatMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallStaticFloatMethod");
            Handle->CallStaticFloatMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallStaticFloatMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallStaticFloatMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallStaticFloatMethodADelegate);

            Handle->CallStaticDoubleMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallStaticDoubleMethod");
            Handle->CallStaticDoubleMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallStaticDoubleMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallStaticDoubleMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallStaticDoubleMethodADelegate);

            Handle->CallStaticVoidMethod = (void*)GetLibJvmFunctionHandle("__JNI_CallStaticVoidMethod");
            Handle->CallStaticVoidMethodV = (void*)GetLibJvmFunctionHandle("__JNI_CallStaticVoidMethodV", sizeof(JNIEnv*) + sizeof(jclass) + sizeof(jmethodID) + sizeof(JValue*));
            Handle->CallStaticVoidMethodA = (void*)Marshal.GetFunctionPointerForDelegate(CallStaticVoidMethodADelegate);

            Handle->GetStaticFieldID = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticFieldIDDelegate);

            Handle->GetStaticObjectField = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticObjectFieldDelegate);
            Handle->GetStaticBooleanField = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticBooleanFieldDelegate);
            Handle->GetStaticByteField = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticByteFieldDelegate);
            Handle->GetStaticCharField = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticCharFieldDelegate);
            Handle->GetStaticShortField = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticShortFieldDelegate);
            Handle->GetStaticIntField = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticIntFieldDelegate);
            Handle->GetStaticLongField = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticLongFieldDelegate);
            Handle->GetStaticFloatField = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticFloatFieldDelegate);
            Handle->GetStaticDoubleField = (void*)Marshal.GetFunctionPointerForDelegate(GetStaticDoubleFieldDelegate);

            Handle->SetStaticObjectField = (void*)Marshal.GetFunctionPointerForDelegate(SetStaticObjectFieldDelegate);
            Handle->SetStaticBooleanField = (void*)Marshal.GetFunctionPointerForDelegate(SetStaticBooleanFieldDelegate);
            Handle->SetStaticByteField = (void*)Marshal.GetFunctionPointerForDelegate(SetStaticByteFieldDelegate);
            Handle->SetStaticCharField = (void*)Marshal.GetFunctionPointerForDelegate(SetStaticCharFieldDelegate);
            Handle->SetStaticShortField = (void*)Marshal.GetFunctionPointerForDelegate(SetStaticShortFieldDelegate);
            Handle->SetStaticIntField = (void*)Marshal.GetFunctionPointerForDelegate(SetStaticIntFieldDelegate);
            Handle->SetStaticLongField = (void*)Marshal.GetFunctionPointerForDelegate(SetStaticLongFieldDelegate);
            Handle->SetStaticFloatField = (void*)Marshal.GetFunctionPointerForDelegate(SetStaticFloatFieldDelegate);
            Handle->SetStaticDoubleField = (void*)Marshal.GetFunctionPointerForDelegate(SetStaticDoubleFieldDelegate);

            Handle->NewString = (void*)Marshal.GetFunctionPointerForDelegate(NewStringDelegate);
            Handle->GetStringLength = (void*)Marshal.GetFunctionPointerForDelegate(GetStringLengthDelegate);
            Handle->GetStringChars = (void*)Marshal.GetFunctionPointerForDelegate(GetStringCharsDelegate);
            Handle->ReleaseStringChars = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseStringCharsDelegate);

            Handle->NewStringUTF = (void*)Marshal.GetFunctionPointerForDelegate(NewStringUTFDelegate);
            Handle->GetStringUTFLength = (void*)Marshal.GetFunctionPointerForDelegate(GetStringUTFLengthDelegate);
            Handle->GetStringUTFChars = (void*)Marshal.GetFunctionPointerForDelegate(GetStringUTFCharsDelegate);
            Handle->ReleaseStringUTFChars = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseStringUTFCharsDelegate);

            Handle->GetArrayLength = (void*)Marshal.GetFunctionPointerForDelegate(GetArrayLengthDelegate);

            Handle->NewObjectArray = (void*)Marshal.GetFunctionPointerForDelegate(NewObjectArrayDelegate);
            Handle->GetObjectArrayElement = (void*)Marshal.GetFunctionPointerForDelegate(GetObjectArrayElementDelegate);
            Handle->SetObjectArrayElement = (void*)Marshal.GetFunctionPointerForDelegate(SetObjectArrayElementDelegate);

            Handle->NewBooleanArray = (void*)Marshal.GetFunctionPointerForDelegate(NewBooleanArrayDelegate);
            Handle->NewByteArray = (void*)Marshal.GetFunctionPointerForDelegate(NewByteArrayDelegate);
            Handle->NewCharArray = (void*)Marshal.GetFunctionPointerForDelegate(NewCharArrayDelegate);
            Handle->NewShortArray = (void*)Marshal.GetFunctionPointerForDelegate(NewShortArrayDelegate);
            Handle->NewIntArray = (void*)Marshal.GetFunctionPointerForDelegate(NewIntArrayDelegate);
            Handle->NewLongArray = (void*)Marshal.GetFunctionPointerForDelegate(NewLongArrayDelegate);
            Handle->NewFloatArray = (void*)Marshal.GetFunctionPointerForDelegate(NewFloatArrayDelegate);
            Handle->NewDoubleArray = (void*)Marshal.GetFunctionPointerForDelegate(NewDoubleArrayDelegate);

            Handle->GetBooleanArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(GetBooleanArrayElementsDelegate);
            Handle->GetByteArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(GetByteArrayElementsDelegate);
            Handle->GetCharArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(GetCharArrayElementsDelegate);
            Handle->GetShortArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(GetShortArrayElementsDelegate);
            Handle->GetIntArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(GetIntArrayElementsDelegate);
            Handle->GetLongArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(GetLongArrayElementsDelegate);
            Handle->GetFloatArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(GetFloatArrayElementsDelegate);
            Handle->GetDoubleArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(GetDoubleArrayElementsDelegate);

            Handle->ReleaseBooleanArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseBooleanArrayElementsDelegate);
            Handle->ReleaseByteArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseByteArrayElementsDelegate);
            Handle->ReleaseCharArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseCharArrayElementsDelegate);
            Handle->ReleaseShortArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseShortArrayElementsDelegate);
            Handle->ReleaseIntArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseIntArrayElementsDelegate);
            Handle->ReleaseLongArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseLongArrayElementsDelegate);
            Handle->ReleaseFloatArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseFloatArrayElementsDelegate);
            Handle->ReleaseDoubleArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseDoubleArrayElementsDelegate);

            Handle->GetBooleanArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(GetBooleanArrayRegionDelegate);
            Handle->GetByteArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(GetByteArrayRegionDelegate);
            Handle->GetCharArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(GetCharArrayRegionDelegate);
            Handle->GetShortArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(GetShortArrayRegionDelegate);
            Handle->GetIntArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(GetIntArrayRegionDelegate);
            Handle->GetLongArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(GetLongArrayRegionDelegate);
            Handle->GetFloatArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(GetFloatArrayRegionDelegate);
            Handle->GetDoubleArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(GetDoubleArrayRegionDelegate);

            Handle->SetBooleanArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(SetBooleanArrayRegionDelegate);
            Handle->SetByteArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(SetByteArrayRegionDelegate);
            Handle->SetCharArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(SetCharArrayRegionDelegate);
            Handle->SetShortArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(SetShortArrayRegionDelegate);
            Handle->SetIntArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(SetIntArrayRegionDelegate);
            Handle->SetLongArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(SetLongArrayRegionDelegate);
            Handle->SetFloatArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(SetFloatArrayRegionDelegate);
            Handle->SetDoubleArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(SetDoubleArrayRegionDelegate);

            Handle->RegisterNatives = (void*)Marshal.GetFunctionPointerForDelegate(RegisterNativesDelegate);
            Handle->UnregisterNatives = (void*)Marshal.GetFunctionPointerForDelegate(UnregisterNativesDelegate);

            Handle->MonitorEnter = (void*)Marshal.GetFunctionPointerForDelegate(MonitorEnterDelegate);
            Handle->MonitorExit = (void*)Marshal.GetFunctionPointerForDelegate(MonitorExitDelegate);

            Handle->GetJavaVM = (void*)Marshal.GetFunctionPointerForDelegate(GetJavaVMDelegate);

            Handle->GetStringRegion = (void*)Marshal.GetFunctionPointerForDelegate(GetStringRegionDelegate);
            Handle->GetStringUTFRegion = (void*)Marshal.GetFunctionPointerForDelegate(GetStringUTFRegionDelegate);

            Handle->GetPrimitiveArrayCritical = (void*)Marshal.GetFunctionPointerForDelegate(GetPrimitiveArrayCriticalDelegate);
            Handle->ReleasePrimitiveArrayCritical = (void*)Marshal.GetFunctionPointerForDelegate(ReleasePrimitiveArrayCriticalDelegate);

            Handle->GetStringCritical = (void*)Marshal.GetFunctionPointerForDelegate(GetStringCriticalDelegate);
            Handle->ReleaseStringCritical = (void*)Marshal.GetFunctionPointerForDelegate(ReleaseStringCriticalDelegate);

            Handle->NewWeakGlobalRef = (void*)Marshal.GetFunctionPointerForDelegate(NewWeakGlobalRefDelegate);
            Handle->DeleteWeakGlobalRef = (void*)Marshal.GetFunctionPointerForDelegate(DeleteWeakGlobalRefDelegate);

            Handle->ExceptionCheck = (void*)Marshal.GetFunctionPointerForDelegate(ExceptionCheckDelegate);

            Handle->NewDirectByteBuffer = (void*)Marshal.GetFunctionPointerForDelegate(NewDirectByteBufferDelegate);
            Handle->GetDirectBufferAddress = (void*)Marshal.GetFunctionPointerForDelegate(GetDirectBufferAddressDelegate);
            Handle->GetDirectBufferCapacity = (void*)Marshal.GetFunctionPointerForDelegate(GetDirectBufferCapacityDelegate);

            Handle->GetObjectRefType = (void*)Marshal.GetFunctionPointerForDelegate(GetObjectRefTypeDelegate);
        }

        public void* reserved0;
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

#endif

}
