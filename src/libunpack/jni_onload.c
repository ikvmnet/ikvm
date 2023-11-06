#include <jni.h>

/**
* Dummy method to prevent dlsym from loading JNI_OnLoad from libiava.
*/
JNIEXPORT jint JNI_OnLoad(JavaVM* vm, void* reserved) {
    return JNI_VERSION_1_8;
}

/**
* Dummy method to prevent dlsym from loading JNI_OnUnload from libiava.
*/
JNIEXPORT void JNI_OnUnload(JavaVM* vm, void* reserved) {
    
}
