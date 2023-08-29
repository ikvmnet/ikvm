#include <jni.h>

/**
* Dummy method to prevent dlsym from loading JNI_OnLoad from libikvm.
*/
JNIEXPORT jint JNI_OnLoad(JavaVM* vm, void* reserved) {
    return JNI_VERSION_1_8;
}

/**
* Dummy method to prevent dlsym from loading JNI_OnUnload from libikvm.
*/
JNIEXPORT void JNI_OnUnload(JavaVM* vm, void* reserved) {
    
}
