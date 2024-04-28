#include <jni.h>

#ifdef __cplusplus
extern "C" {
#endif

typedef struct JVMInvokeInterface {
    int (*JNI_GetDefaultJavaVMInitArgs)(void*);
    int (*JNI_GetCreatedJavaVMs)(JavaVM **vmBuf, jsize bufLen, jsize *nVMs);
    int (*JNI_CreateJavaVM)(JavaVM **p_vm, void **p_env, void *vm_args);
    void (*JVM_ThrowException)(const char*, const char*);
    void* (*JVM_GetThreadInterruptEvent)();
} JVMInvokeInterface;

extern JVMInvokeInterface* jvmii;

JNIEXPORT void JNICALL JVM_Init(JVMInvokeInterface* p_jvmii);

void JNICALL JVM_ThrowException(const char* name, const char* msg);

#ifdef __cplusplus
}
#endif
