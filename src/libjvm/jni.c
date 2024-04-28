#include <jni.h>

#include "ikvm.h"

JNIEXPORT jint JNICALL JNI_GetDefaultJavaVMInitArgs(void* vm_args)
{
    return jvmii->JNI_GetDefaultJavaVMInitArgs(vm_args);
}

JNIEXPORT jint JNICALL JNI_GetCreatedJavaVMs(JavaVM** vmBuf, jsize bufLen, jsize* nVMs)
{
    return jvmii->JNI_GetCreatedJavaVMs(vmBuf, bufLen, nVMs);
}

JNIEXPORT jint JNICALL JNI_CreateJavaVM(JavaVM** p_vm, void** p_env, void* vm_args)
{
    return jvmii->JNI_CreateJavaVM(p_vm, p_env, vm_args);
}
