#include <jni.h>

#ifdef __cplusplus
extern "C" {
#endif

typedef struct JVMInvokeInterface {
    jint (*JNI_GetDefaultJavaVMInitArgs)(void *);
    jint (*JNI_GetCreatedJavaVMs)(JavaVM **vmBuf, jsize bufLen, jsize *nVMs);
    jint (*JNI_CreateJavaVM)(JavaVM **p_vm, void **p_env, void *vm_args);
    void (*JVM_ThrowException)(const char*, const char*);
    void* (*JVM_GetThreadInterruptEvent)();
    jint (*JVM_ActiveProcessorCount)();
    jint (*JVM_IHashCode)(JNIEnv *pEnv, jobject handle);
    void (*JVM_ArrayCopy)(JNIEnv *pEnv, jclass ignored, jobject src, jint src_pos, jobject dst, jint dst_pst, jint length);
    jobject (*JVM_InitProperties)(JNIEnv *pEnv, jobject props);
} JVMInvokeInterface;

extern JVMInvokeInterface *jvmii;

JNIEXPORT void JNICALL JVM_Init(JVMInvokeInterface *p_jvmii);

JNIEXPORT void JNICALL JVM_ThrowException(const char *name, const char *msg);

#ifdef __cplusplus
}
#endif
