#include <stddef.h>
#include <jni.h>

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_getVersionTest(JNIEnv *env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jint version = (*env)->GetVersion(env);
    if (version != 0x00010008) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_defineClassTest(JNIEnv *env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_findClassTest(JNIEnv *env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jclass objectClass = (*env)->FindClass(env, "java/lang/Object");
    if (objectClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_getSuperclassTest(JNIEnv *env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jclass numberClass = (*env)->FindClass(env, "java/lang/Number");
    if (numberClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jclass superClass = (*env)->GetSuperclass(env, numberClass);
    if (superClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jclass objectClass = (*env)->FindClass(env, "java/lang/Object");
    if (objectClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    if ((*env)->IsSameObject(env, superClass, objectClass) != JNI_TRUE) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_throwTest(JNIEnv *env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jclass runtimeExceptionClass = (*env)->FindClass(env, "java/lang/RuntimeException");
    if (runtimeExceptionClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jmethodID runtimeExceptionClassCtor = (*env)->GetMethodID(env, runtimeExceptionClass, "<init>", "(Ljava/lang/String;)V");
    if (runtimeExceptionClassCtor == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jstring message = (*env)->NewStringUTF(env, "success");
    if (message == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jthrowable runtimeException = (*env)->NewObject(env, runtimeExceptionClass, runtimeExceptionClassCtor, message);
    if (runtimeException == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    (*env)->Throw(env, runtimeException);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_throwNewTest(JNIEnv *env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jclass runtimeExceptionClass = (*env)->FindClass(env, "java/lang/RuntimeException");
    if (runtimeExceptionClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    (*env)->ThrowNew(env, runtimeExceptionClass, "success");
}

jobject newObject(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...) {

    va_list args;
    va_start(args, methodID);
    jobject o = (*pEnv)->NewObjectV(pEnv, clazz, methodID, args);
    va_end(args);
    return o;
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_newObjectTest(JNIEnv *env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jclass objectClass = (*env)->FindClass(env, "java/lang/Object");
    if (objectClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jmethodID objectClassCtor = (*env)->GetMethodID(env, objectClass, "<init>", "()V");
    if (objectClassCtor == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jobject object = (*env)->NewObject(env, objectClass, objectClassCtor);
    if (object == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_newObjectVTest(JNIEnv *env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jclass objectClass = (*env)->FindClass(env, "java/lang/Object");
    if (objectClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jmethodID objectClassCtor = (*env)->GetMethodID(env, objectClass, "<init>", "()V");
    if (objectClassCtor == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jobject object = newObject(env, objectClass, objectClassCtor);
    if (object == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_newObjectATest(JNIEnv *env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jclass objectClass = (*env)->FindClass(env, "java/lang/Object");
    if (objectClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jmethodID objectClassCtor = (*env)->GetMethodID(env, objectClass, "<init>", "()V");
    if (objectClassCtor == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jobject object = (*env)->NewObjectA(env, objectClass, objectClassCtor, NULL);
    if (object == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_newObjectTestWithArg(JNIEnv *env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jclass integerClass = (*env)->FindClass(env, "java/lang/Integer");
    if (integerClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jmethodID integerClassCtor = (*env)->GetMethodID(env, integerClass, "<init>", "(I)V");
    if (integerClassCtor == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jobject integer = (*env)->NewObject(env, integerClass, integerClassCtor, 1);
    if (integer == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jmethodID integerIntValueMethodID = (*env)->GetMethodID(env, integerClass, "intValue", "()I");
    if (integerIntValueMethodID == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jint integerValue = (*env)->CallIntMethod(env, integer, integerIntValueMethodID);
    if (integerValue != 1) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_newObjectVTestWithArg(JNIEnv *env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jclass integerClass = (*env)->FindClass(env, "java/lang/Integer");
    if (integerClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jmethodID integerClassCtor = (*env)->GetMethodID(env, integerClass, "<init>", "(I)V");
    if (integerClassCtor == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jobject integer = newObject(env, integerClass, integerClassCtor, 1);
    if (integer == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jmethodID integerIntValueMethodID = (*env)->GetMethodID(env, integerClass, "intValue", "()I");
    if (integerIntValueMethodID == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jint integerValue = (*env)->CallIntMethodA(env, integer, integerIntValueMethodID, NULL);
    if (integerValue != 1) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_newObjectATestWithArg(JNIEnv *env) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jclass integerClass = (*env)->FindClass(env, "java/lang/Integer");
    if (integerClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jmethodID integerClassCtor = (*env)->GetMethodID(env, integerClass, "<init>", "(I)V");
    if (integerClassCtor == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jvalue a[1];
    a[0].i = 1;

    jobject integer = (*env)->NewObjectA(env, integerClass, integerClassCtor, a);
    if (integer == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jmethodID integerIntValueMethodID = (*env)->GetMethodID(env, integerClass, "intValue", "()I");
    if (integerIntValueMethodID == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }

    jint integerValue = (*env)->CallIntMethodA(env, integer, integerIntValueMethodID, NULL);
    if (integerValue != 1) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }
}
