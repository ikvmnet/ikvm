#include <stddef.h>
#include <jni.h>

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_getVersionTest(JNIEnv* env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jint version = (*env)->GetVersion(env);
    if (version != 0x00010008) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_defineClassTest(JNIEnv* env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_findClassTest(JNIEnv* env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jclass objectClass = (*env)->FindClass(env, "java/lang/Object");
    if (objectClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_getSuperclassTest(JNIEnv* env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jclass integerClass = (*env)->FindClass(env, "java/lang/Integer");
    if (integerClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jclass objectClass = (*env)->FindClass(env, "java/lang/Object");
    if (objectClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jclass superClass = (*env)->GetSuperclass(env, integerClass);
    if (superClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    if (superClass != objectClass) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_throwTest(JNIEnv* env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jclass runtimeExceptionClass = (*env)->FindClass(env, "java/lang/RuntimeException");
    if (runtimeExceptionClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jmethodID runtimeExceptionClassCtor = (*env)->GetMethodID(env, exceptionClass, "<init>", "(Ljava/lang/String;)V");
    if (runtimeExceptionClassCtor == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jthrowable runtimeException = (*env)->NewObject(env, exceptionClass, runtimeExceptionClassCtor, "success");
    if (runtimeException == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    (*env)->Throw(env, runtimeException);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_throwNewTest(JNIEnv* env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jclass runtimeExceptionClass = (*env)->FindClass(env, "java/lang/RuntimeException");
    if (runtimeExceptionClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    (*env)->ThrowNew(env, runtimeExceptionClass, "success");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_newObjectTest(JNIEnv* env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jclass integerClass = (*env)->FindClass(env, "java/lang/Integer");
    if (integerClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jmethodID integerClassCtor = (*env)->GetMethodID(env, exceptionClass, "<init>", "(I)V");
    if (integerClassCtor == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jobject integer = (*env)->NewObject(env, exceptionClass, integerClassCtor, 1);
    if (integer == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_newObjectVTest(JNIEnv* env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jclass integerClass = (*env)->FindClass(env, "java/lang/Integer");
    if (integerClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jmethodID integerClassCtor = (*env)->GetMethodID(env, exceptionClass, "<init>", "(I)V");
    if (integerClassCtor == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jobject integer = (*env)->NewObjectV(env, exceptionClass, integerClassCtor, 1);
    if (integer == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_newObjectATest(JNIEnv* env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jclass integerClass = (*env)->FindClass(env, "java/lang/Integer");
    if (integerClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jmethodID integerClassCtor = (*env)->GetMethodID(env, exceptionClass, "<init>", "(I)V");
    if (integerClassCtor == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jvalue a[1];
    a[0].i = 1;

    jobject integer = (*env)->NewObjectA(env, exceptionClass, integerClassCtor, a);
    if (integer == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }
}
