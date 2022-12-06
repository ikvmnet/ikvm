using System;
using System.Runtime.InteropServices;

using IKVM.Runtime.JNI.Trampolines;

namespace IKVM.Runtime.JNI
{

    /// <summary>
    /// Manged implementation of the JNINativeInterface structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    unsafe struct JNINativeInterface
    {

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

        static readonly JNINativeInterfaceMemory memory = new();
        static readonly JNINativeInterface* handle = memory.handle;

        /// <summary>
        /// Gets a pointer to the JNINativeInterface table.
        /// </summary>
        public static JNINativeInterface* Handle => handle;

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static JNINativeInterface()
        {
            JNIVM.jvmCreated = true;

            handle->GetMethodArgs = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetMethodArgs);
            handle->reserved1 = null;
            handle->reserved2 = null;

            handle->reserved3 = null;
            handle->GetVersion = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetVersion);

            handle->DefineClass = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.DefineClass);
            handle->FindClass = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.FindClass);

            handle->FromReflectedMethod = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.FromReflectedMethod);
            handle->FromReflectedField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.FromReflectedField);
            handle->ToReflectedMethod = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.ToReflectedMethod);

            handle->GetSuperclass = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetSuperclass);
            handle->IsAssignableFrom = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.IsAssignableFrom);

            handle->ToReflectedField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.ToReflectedField);

            handle->Throw = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.Throw);
            handle->ThrowNew = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.ThrowNew);
            handle->ExceptionOccurred = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.ExceptionOccurred);
            handle->ExceptionDescribe = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.ExceptionDescribe);
            handle->ExceptionClear = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.ExceptionClear);
            handle->FatalError = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.FatalError);

            handle->PushLocalFrame = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.PushLocalFrame);
            handle->PopLocalFrame = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.PopLocalFrame);

            handle->NewGlobalRef = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.NewGlobalRef);
            handle->DeleteGlobalRef = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.DeleteGlobalRef);
            handle->DeleteLocalRef = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.DeleteLocalRef);
            handle->IsSameObject = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.IsSameObject);

            handle->NewLocalRef = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.NewLocalRef);
            handle->EnsureLocalCapacity = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.EnsureLocalCapacity);

            handle->AllocObject = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.AllocObject);
            handle->NewObject = (void*)FunctionTable.Instance.JNI_NewObject;
            handle->NewObjectV = (void*)FunctionTable.Instance.JNI_NewObjectV;
            handle->NewObjectA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.NewObjectA);

            handle->GetObjectClass = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetObjectClass);
            handle->IsInstanceOf = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.IsInstanceOf);

            handle->GetMethodID = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetMethodID);

            handle->CallObjectMethod = (void*)FunctionTable.Instance.JNI_CallObjectMethod;
            handle->CallObjectMethodV = (void*)FunctionTable.Instance.JNI_CallObjectMethodV;
            handle->CallObjectMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallObjectMethodA);

            handle->CallBooleanMethod = (void*)FunctionTable.Instance.JNI_CallBooleanMethod;
            handle->CallBooleanMethodV = (void*)FunctionTable.Instance.JNI_CallBooleanMethodV;
            handle->CallBooleanMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallBooleanMethodA);

            handle->CallByteMethod = (void*)FunctionTable.Instance.JNI_CallByteMethod;
            handle->CallByteMethodV = (void*)FunctionTable.Instance.JNI_CallByteMethodV;
            handle->CallByteMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallByteMethodA);

            handle->CallCharMethod = (void*)FunctionTable.Instance.JNI_CallCharMethod;
            handle->CallCharMethodV = (void*)FunctionTable.Instance.JNI_CallCharMethodV;
            handle->CallCharMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallCharMethodA);

            handle->CallShortMethod = (void*)FunctionTable.Instance.JNI_CallShortMethod;
            handle->CallShortMethodV = (void*)FunctionTable.Instance.JNI_CallShortMethodV;
            handle->CallShortMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallShortMethodA);

            handle->CallIntMethod = (void*)FunctionTable.Instance.JNI_CallIntMethod;
            handle->CallIntMethodV = (void*)FunctionTable.Instance.JNI_CallIntMethodV;
            handle->CallIntMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallIntMethodA);

            handle->CallLongMethod = (void*)FunctionTable.Instance.JNI_CallLongMethod;
            handle->CallLongMethodV = (void*)FunctionTable.Instance.JNI_CallLongMethodV;
            handle->CallLongMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallLongMethodA);

            handle->CallFloatMethod = (void*)FunctionTable.Instance.JNI_CallFloatMethod;
            handle->CallFloatMethodV = (void*)FunctionTable.Instance.JNI_CallFloatMethodV;
            handle->CallFloatMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallFloatMethodA);

            handle->CallDoubleMethod = (void*)FunctionTable.Instance.JNI_CallDoubleMethod;
            handle->CallDoubleMethodV = (void*)FunctionTable.Instance.JNI_CallDoubleMethodV;
            handle->CallDoubleMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallDoubleMethodA);

            handle->CallVoidMethod = (void*)FunctionTable.Instance.JNI_CallVoidMethod;
            handle->CallVoidMethodV = (void*)FunctionTable.Instance.JNI_CallVoidMethodV;
            handle->CallVoidMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallVoidMethodA);

            handle->CallNonvirtualObjectMethod = (void*)FunctionTable.Instance.JNI_CallNonvirtualObjectMethod;
            handle->CallNonvirtualObjectMethodV = (void*)FunctionTable.Instance.JNI_CallNonvirtualObjectMethodV;
            handle->CallNonvirtualObjectMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallNonvirtualObjectMethodA);

            handle->CallNonvirtualBooleanMethod = (void*)FunctionTable.Instance.JNI_CallNonvirtualBooleanMethod;
            handle->CallNonvirtualBooleanMethodV = (void*)FunctionTable.Instance.JNI_CallNonvirtualBooleanMethodV;
            handle->CallNonvirtualBooleanMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallNonvirtualBooleanMethodA);

            handle->CallNonvirtualByteMethod = (void*)FunctionTable.Instance.JNI_CallNonvirtualByteMethod;
            handle->CallNonvirtualByteMethodV = (void*)FunctionTable.Instance.JNI_CallNonvirtualByteMethodV;
            handle->CallNonvirtualByteMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallNonvirtualByteMethodA);

            handle->CallNonvirtualCharMethod = (void*)FunctionTable.Instance.JNI_CallNonvirtualCharMethod;
            handle->CallNonvirtualCharMethodV = (void*)FunctionTable.Instance.JNI_CallNonvirtualCharMethodV;
            handle->CallNonvirtualCharMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallNonvirtualCharMethodA);

            handle->CallNonvirtualShortMethod = (void*)FunctionTable.Instance.JNI_CallNonvirtualShortMethod;
            handle->CallNonvirtualShortMethodV = (void*)FunctionTable.Instance.JNI_CallNonvirtualShortMethodV;
            handle->CallNonvirtualShortMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallNonvirtualShortMethodA);

            handle->CallNonvirtualIntMethod = (void*)FunctionTable.Instance.JNI_CallNonvirtualIntMethod;
            handle->CallNonvirtualIntMethodV = (void*)FunctionTable.Instance.JNI_CallNonvirtualIntMethodV;
            handle->CallNonvirtualIntMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallNonvirtualIntMethodA);

            handle->CallNonvirtualLongMethod = (void*)FunctionTable.Instance.JNI_CallNonvirtualLongMethod;
            handle->CallNonvirtualLongMethodV = (void*)FunctionTable.Instance.JNI_CallNonvirtualLongMethodV;
            handle->CallNonvirtualLongMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallNonvirtualLongMethodA);

            handle->CallNonvirtualFloatMethod = (void*)FunctionTable.Instance.JNI_CallNonvirtualFloatMethod;
            handle->CallNonvirtualFloatMethodV = (void*)FunctionTable.Instance.JNI_CallNonvirtualFloatMethodV;
            handle->CallNonvirtualFloatMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallNonvirtualFloatMethodA);

            handle->CallNonvirtualDoubleMethod = (void*)FunctionTable.Instance.JNI_CallNonvirtualDoubleMethod;
            handle->CallNonvirtualDoubleMethodV = (void*)FunctionTable.Instance.JNI_CallNonvirtualDoubleMethodV;
            handle->CallNonvirtualDoubleMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallNonvirtualDoubleMethodA);

            handle->CallNonvirtualVoidMethod = (void*)FunctionTable.Instance.JNI_CallNonvirtualVoidMethod;
            handle->CallNonvirtualVoidMethodV = (void*)FunctionTable.Instance.JNI_CallNonvirtualVoidMethodV;
            handle->CallNonvirtualVoidMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallNonvirtualVoidMethodA);

            handle->GetFieldID = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetFieldID);

            handle->GetObjectField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetObjectField);
            handle->GetBooleanField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetBooleanField);
            handle->GetByteField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetByteField);
            handle->GetCharField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetCharField);
            handle->GetShortField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetShortField);
            handle->GetIntField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetIntField);
            handle->GetLongField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetLongField);
            handle->GetFloatField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetFloatField);
            handle->GetDoubleField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetDoubleField);

            handle->SetObjectField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetObjectField);
            handle->SetBooleanField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetBooleanField);
            handle->SetByteField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetByteField);
            handle->SetCharField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetCharField);
            handle->SetShortField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetShortField);
            handle->SetIntField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetIntField);
            handle->SetLongField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetLongField);
            handle->SetFloatField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetFloatField);
            handle->SetDoubleField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetDoubleField);

            handle->GetStaticMethodID = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetStaticMethodID);

            handle->CallStaticObjectMethod = (void*)FunctionTable.Instance.JNI_CallStaticObjectMethod;
            handle->CallStaticObjectMethodV = (void*)FunctionTable.Instance.JNI_CallStaticObjectMethodV;
            handle->CallStaticObjectMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallStaticObjectMethodA);

            handle->CallStaticBooleanMethod = (void*)FunctionTable.Instance.JNI_CallStaticBooleanMethod;
            handle->CallStaticBooleanMethodV = (void*)FunctionTable.Instance.JNI_CallStaticBooleanMethodV;
            handle->CallStaticBooleanMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallStaticBooleanMethodA);

            handle->CallStaticByteMethod = (void*)FunctionTable.Instance.JNI_CallStaticByteMethod;
            handle->CallStaticByteMethodV = (void*)FunctionTable.Instance.JNI_CallStaticByteMethodV;
            handle->CallStaticByteMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallStaticByteMethodA);

            handle->CallStaticCharMethod = (void*)FunctionTable.Instance.JNI_CallStaticCharMethod;
            handle->CallStaticCharMethodV = (void*)FunctionTable.Instance.JNI_CallStaticCharMethodV;
            handle->CallStaticCharMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallStaticCharMethodA);

            handle->CallStaticShortMethod = (void*)FunctionTable.Instance.JNI_CallStaticShortMethod;
            handle->CallStaticShortMethodV = (void*)FunctionTable.Instance.JNI_CallStaticShortMethodV;
            handle->CallStaticShortMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallStaticShortMethodA);

            handle->CallStaticIntMethod = (void*)FunctionTable.Instance.JNI_CallStaticIntMethod;
            handle->CallStaticIntMethodV = (void*)FunctionTable.Instance.JNI_CallStaticIntMethodV;
            handle->CallStaticIntMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallStaticIntMethodA);

            handle->CallStaticLongMethod = (void*)FunctionTable.Instance.JNI_CallStaticLongMethod;
            handle->CallStaticLongMethodV = (void*)FunctionTable.Instance.JNI_CallStaticLongMethodV;
            handle->CallStaticLongMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallStaticLongMethodA);

            handle->CallStaticFloatMethod = (void*)FunctionTable.Instance.JNI_CallStaticFloatMethod;
            handle->CallStaticFloatMethodV = (void*)FunctionTable.Instance.JNI_CallStaticFloatMethodV;
            handle->CallStaticFloatMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallStaticFloatMethodA);

            handle->CallStaticDoubleMethod = (void*)FunctionTable.Instance.JNI_CallStaticDoubleMethod;
            handle->CallStaticDoubleMethodV = (void*)FunctionTable.Instance.JNI_CallStaticDoubleMethodV;
            handle->CallStaticDoubleMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallStaticDoubleMethodA);

            handle->CallStaticVoidMethod = (void*)FunctionTable.Instance.JNI_CallStaticVoidMethod;
            handle->CallStaticVoidMethodV = (void*)FunctionTable.Instance.JNI_CallStaticVoidMethodV;
            handle->CallStaticVoidMethodA = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.CallStaticVoidMethodA);

            handle->GetStaticFieldID = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetStaticFieldID);

            handle->GetStaticObjectField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetStaticObjectField);
            handle->GetStaticBooleanField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetStaticBooleanField);
            handle->GetStaticByteField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetStaticByteField);
            handle->GetStaticCharField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetStaticCharField);
            handle->GetStaticShortField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetStaticShortField);
            handle->GetStaticIntField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetStaticIntField);
            handle->GetStaticLongField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetStaticLongField);
            handle->GetStaticFloatField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetStaticFloatField);
            handle->GetStaticDoubleField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetStaticDoubleField);

            handle->SetStaticObjectField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetStaticObjectField);
            handle->SetStaticBooleanField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetStaticBooleanField);
            handle->SetStaticByteField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetStaticByteField);
            handle->SetStaticCharField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetStaticCharField);
            handle->SetStaticShortField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetStaticShortField);
            handle->SetStaticIntField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetStaticIntField);
            handle->SetStaticLongField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetStaticLongField);
            handle->SetStaticFloatField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetStaticFloatField);
            handle->SetStaticDoubleField = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetStaticDoubleField);

            handle->NewString = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.NewString);
            handle->GetStringLength = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetStringLength);
            handle->GetStringChars = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetStringChars);
            handle->ReleaseStringChars = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.ReleaseStringChars);

            handle->NewStringUTF = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.NewStringUTF);
            handle->GetStringUTFLength = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetStringUTFLength);
            handle->GetStringUTFChars = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetStringUTFChars);
            handle->ReleaseStringUTFChars = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.ReleaseStringUTFChars);

            handle->GetArrayLength = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetArrayLength);

            handle->NewObjectArray = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.NewObjectArray);
            handle->GetObjectArrayElement = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetObjectArrayElement);
            handle->SetObjectArrayElement = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetObjectArrayElement);

            handle->NewBooleanArray = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.NewBooleanArray);
            handle->NewByteArray = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.NewByteArray);
            handle->NewCharArray = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.NewCharArray);
            handle->NewShortArray = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.NewShortArray);
            handle->NewIntArray = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.NewIntArray);
            handle->NewLongArray = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.NewLongArray);
            handle->NewFloatArray = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.NewFloatArray);
            handle->NewDoubleArray = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.NewDoubleArray);

            handle->GetBooleanArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetBooleanArrayElements);
            handle->GetByteArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetByteArrayElements);
            handle->GetCharArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetCharArrayElements);
            handle->GetShortArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetShortArrayElements);
            handle->GetIntArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetIntArrayElements);
            handle->GetLongArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetLongArrayElements);
            handle->GetFloatArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetFloatArrayElements);
            handle->GetDoubleArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetDoubleArrayElements);

            handle->ReleaseBooleanArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.ReleaseBooleanArrayElements);
            handle->ReleaseByteArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.ReleaseByteArrayElements);
            handle->ReleaseCharArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.ReleaseCharArrayElements);
            handle->ReleaseShortArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.ReleaseShortArrayElements);
            handle->ReleaseIntArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.ReleaseIntArrayElements);
            handle->ReleaseLongArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.ReleaseLongArrayElements);
            handle->ReleaseFloatArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.ReleaseFloatArrayElements);
            handle->ReleaseDoubleArrayElements = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.ReleaseDoubleArrayElements);

            handle->GetBooleanArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetBooleanArrayRegion);
            handle->GetByteArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetByteArrayRegion);
            handle->GetCharArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetCharArrayRegion);
            handle->GetShortArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetShortArrayRegion);
            handle->GetIntArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetIntArrayRegion);
            handle->GetLongArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetLongArrayRegion);
            handle->GetFloatArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetFloatArrayRegion);
            handle->GetDoubleArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetDoubleArrayRegion);

            handle->SetBooleanArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetBooleanArrayRegion);
            handle->SetByteArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetByteArrayRegion);
            handle->SetCharArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetCharArrayRegion);
            handle->SetShortArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetShortArrayRegion);
            handle->SetIntArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetIntArrayRegion);
            handle->SetLongArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetLongArrayRegion);
            handle->SetFloatArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetFloatArrayRegion);
            handle->SetDoubleArrayRegion = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.SetDoubleArrayRegion);

            handle->RegisterNatives = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.RegisterNatives);
            handle->UnregisterNatives = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.UnregisterNatives);

            handle->MonitorEnter = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.MonitorEnter);
            handle->MonitorExit = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.MonitorExit);

            handle->GetJavaVM = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetJavaVM);

            handle->GetStringRegion = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetStringRegion);
            handle->GetStringUTFRegion = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetStringUTFRegion);

            handle->GetPrimitiveArrayCritical = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetPrimitiveArrayCritical);
            handle->ReleasePrimitiveArrayCritical = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.ReleasePrimitiveArrayCritical);

            handle->GetStringCritical = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetStringCritical);
            handle->ReleaseStringCritical = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.ReleaseStringCritical);

            handle->NewWeakGlobalRef = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.NewWeakGlobalRef);
            handle->DeleteWeakGlobalRef = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.DeleteWeakGlobalRef);

            handle->ExceptionCheck = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.ExceptionCheck);

            handle->NewDirectByteBuffer = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.NewDirectByteBuffer);
            handle->GetDirectBufferAddress = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetDirectBufferAddress);
            handle->GetDirectBufferCapacity = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetDirectBufferCapacity);

            handle->GetObjectRefType = (void*)Marshal.GetFunctionPointerForDelegate(JNIEnv.GetObjectRefType);
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
