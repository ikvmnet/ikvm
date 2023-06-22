#include <jni.h>

typedef jint JNI_GetDefaultJavaVMInitArgs_Func();
typedef jint JNI_GetCreatedJavaVMs_Func(JavaVM** vmBuf, jsize bufLen, jsize* nVMs);
typedef jint JNI_CreateJavaVM_Func(JavaVM** p_vm, void** p_env, void* vm_args);

static JNI_GetDefaultJavaVMInitArgs_Func *JNI_GetDefaultJavaVMInitArgs_Ptr;
static JNI_GetCreatedJavaVMs_Func *JNI_GetCreatedJavaVMs_Ptr;
static JNI_CreateJavaVM_Func *JNI_CreateJavaVM_Ptr;

JNIEXPORT void Set_JNI_GetDefaultJavaVMInitArgs(JNI_GetDefaultJavaVMInitArgs_Func *func)
{
    JNI_GetDefaultJavaVMInitArgs_Ptr = func;
}

JNIEXPORT void Set_JNI_GetCreatedJavaVMs(JNI_GetCreatedJavaVMs_Func *func)
{
    JNI_GetCreatedJavaVMs_Ptr = func;
}

JNIEXPORT void Set_JNI_CreateJavaVM(JNI_CreateJavaVM_Func *func)
{
    JNI_CreateJavaVM_Ptr = func;
}

JNIEXPORT jint JNICALL JNI_GetDefaultJavaVMInitArgs(void* vm_args)
{
    return (*JNI_GetDefaultJavaVMInitArgs_Ptr)(vm_args);
}

JNIEXPORT jint JNICALL JNI_GetCreatedJavaVMs(JavaVM** vmBuf, jsize bufLen, jsize* nVMs)
{
    return (*JNI_GetCreatedJavaVMs_Ptr)(vmBuf, bufLen, nVMs);
}

JNIEXPORT jint JNICALL JNI_CreateJavaVM(JavaVM** p_vm, void** p_env, void* vm_args)
{
    return (*JNI_CreateJavaVM_Ptr)(p_vm, p_env, vm_args);
}
