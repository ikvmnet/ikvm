#include <stddef.h>
#include <jni.h>

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_getVersionTest(JNIEnv* env, jobject self) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jint version = (*env)->GetVersion(env);
    if (version != 0x00010008) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_defineClassTest(JNIEnv* env, jobject self) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_findClassTest(JNIEnv* env, jobject self) {
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jclass objectClass = (*env)->FindClass(env, "java/lang/Object");
    if (objectClass == 0) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return;
    }
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_getSuperclassTest(JNIEnv* env, jobject self) {
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

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_throwTest(JNIEnv* env, jobject self) {
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

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_throwNewTest(JNIEnv* env, jobject self) {
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

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_newObjectTest(JNIEnv* env, jobject self) {
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

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_newObjectVTest(JNIEnv* env, jobject self) {
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

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_newObjectATest(JNIEnv* env, jobject self) {
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

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_newObjectTestWithArg(JNIEnv* env, jobject self) {
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

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_newObjectVTestWithArg(JNIEnv* env, jobject self) {
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

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniTests_newObjectATestWithArg(JNIEnv* env, jobject self) {
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

JNIEXPORT jobject JNICALL Java_ikvm_tests_jni_JniTests_getNullObjectField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "nullObjectField", "Ljava/lang/Object;");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return NULL;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return NULL;
    }

    jobject ret = (*env)->GetObjectField(env, object, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return NULL;
    }

    return ret;
}

JNIEXPORT jobject JNICALL Java_ikvm_tests_jni_JniTests_getObjectField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "objectField", "Ljava/lang/Object;");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return NULL;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return NULL;
    }

    jobject ret = (*env)->GetObjectField(env, object, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return NULL;
    }

    return ret;
}

JNIEXPORT jstring JNICALL Java_ikvm_tests_jni_JniTests_getStringField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "stringField", "Ljava/lang/String;");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return NULL;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return NULL;
    }

    jstring ret = (*env)->GetObjectField(env, object, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return NULL;
    }

    return ret;
}

JNIEXPORT jboolean JNICALL Java_ikvm_tests_jni_JniTests_getBooleanField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "booleanField", "Z");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return JNI_FALSE;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return JNI_FALSE;
    }

    jboolean ret = (*env)->GetBooleanField(env, object, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return JNI_FALSE;
    }

    return ret;
}

JNIEXPORT jbyte JNICALL Java_ikvm_tests_jni_JniTests_getByteField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "byteField", "B");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    jbyte ret = (*env)->GetByteField(env, object, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }

    return ret;
}

JNIEXPORT jchar JNICALL Java_ikvm_tests_jni_JniTests_getCharField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "charField", "C");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    jchar ret = (*env)->GetCharField(env, object, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }

    return ret;
}

JNIEXPORT jshort JNICALL Java_ikvm_tests_jni_JniTests_getShortField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "shortField", "S");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    jshort ret = (*env)->GetShortField(env, object, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }

    return ret;
}

JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniTests_getIntField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "intField", "I");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    jint ret = (*env)->GetIntField(env, object, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }

    return ret;
}

JNIEXPORT jlong JNICALL Java_ikvm_tests_jni_JniTests_getLongField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "longField", "J");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    jlong ret = (*env)->GetLongField(env, object, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }

    return ret;
}

JNIEXPORT jfloat JNICALL Java_ikvm_tests_jni_JniTests_getFloatField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "floatField", "F");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    jfloat ret = (*env)->GetFloatField(env, object, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }

    return ret;
}

JNIEXPORT jdouble JNICALL Java_ikvm_tests_jni_JniTests_getDoubleField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "doubleField", "D");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    jdouble ret = (*env)->GetDoubleField(env, object, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }

    return ret;
}

JNIEXPORT jobject JNICALL Java_ikvm_tests_jni_JniTests_getStaticNullObjectField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "nullObjectField", "Ljava/lang/Object;");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return NULL;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return NULL;
    }

    jobject ret = (*env)->GetStaticObjectField(env, clazz, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return NULL;
    }

    return ret;
}

JNIEXPORT jobject JNICALL Java_ikvm_tests_jni_JniTests_getStaticObjectField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "objectField", "Ljava/lang/Object;");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return NULL;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return NULL;
    }

    jobject ret = (*env)->GetStaticObjectField(env, clazz, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return NULL;
    }

    return ret;
}

JNIEXPORT jstring JNICALL Java_ikvm_tests_jni_JniTests_getStaticStringField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "stringField", "Ljava/lang/String;");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return NULL;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return NULL;
    }

    jstring ret = (*env)->GetStaticObjectField(env, clazz, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return NULL;
    }

    return ret;
}

JNIEXPORT jboolean JNICALL Java_ikvm_tests_jni_JniTests_getStaticBooleanField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "booleanField", "Z");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return JNI_FALSE;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return JNI_FALSE;
    }

    jboolean ret = (*env)->GetStaticBooleanField(env, clazz, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return JNI_FALSE;
    }

    return ret;
}

JNIEXPORT jbyte JNICALL Java_ikvm_tests_jni_JniTests_getStaticByteField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "byteField", "B");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    jbyte ret = (*env)->GetStaticByteField(env, clazz, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }

    return ret;
}

JNIEXPORT jchar JNICALL Java_ikvm_tests_jni_JniTests_getStaticCharField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "charField", "C");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    jchar ret = (*env)->GetStaticCharField(env, clazz, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }

    return ret;
}

JNIEXPORT jshort JNICALL Java_ikvm_tests_jni_JniTests_getStaticShortField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "shortField", "S");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    jshort ret = (*env)->GetStaticShortField(env, clazz, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }

    return ret;
}

JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniTests_getStaticIntField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "intField", "I");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    jint ret = (*env)->GetStaticIntField(env, clazz, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }

    return ret;
}

JNIEXPORT jlong JNICALL Java_ikvm_tests_jni_JniTests_getStaticLongField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "longField", "J");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    jlong ret = (*env)->GetStaticLongField(env, clazz, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }

    return ret;
}

JNIEXPORT jfloat JNICALL Java_ikvm_tests_jni_JniTests_getStaticFloatField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "floatField", "F");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    jfloat ret = (*env)->GetStaticFloatField(env, clazz, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }

    return ret;
}

JNIEXPORT jdouble JNICALL Java_ikvm_tests_jni_JniTests_getStaticDoubleField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "doubleField", "D");
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    jdouble ret = (*env)->GetStaticDoubleField(env, clazz, field);
    if ((*env)->ExceptionCheck(env) == JNI_TRUE) {
        return 0;
    }

    return ret;
}

JNIEXPORT jobject JNICALL Java_ikvm_tests_jni_JniTests_newWeakGlobalRef(JNIEnv* env, jobject self, jobject o)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");
    jobject ret = NULL;

    jweak weak = (*env)->NewWeakGlobalRef(env, o);
    if (weak == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "NewWeakGlobalRef returned NULL.");
        ret = NULL;
        goto end;
    }
    else if ((*env)->IsSameObject(env, weak, NULL)) {
        (*env)->ThrowNew(env, exceptionClass, "NewWeakGlobalRef is the same object as NULL.");
        ret = NULL;
        goto end;
    }
    else if ((*env)->IsSameObject(env, weak, o) == JNI_FALSE) {
        (*env)->ThrowNew(env, exceptionClass, "NewWeakGlobalRef is not the same object as passed.");
        ret = NULL;
        goto end;
    }

    jobject local = (*env)->NewLocalRef(env, weak);
    if (local == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "NewLocalRef from weak ref returned NULL.");
        ret = NULL;
        goto end;
    }
    else if ((*env)->IsSameObject(env, local, NULL)) {
        (*env)->ThrowNew(env, exceptionClass, "NewLocalRef from weak ref is the same object as NULL.");
        ret = NULL;
        goto end;
    }
    else if ((*env)->IsSameObject(env, local, weak) == JNI_FALSE) {
        (*env)->ThrowNew(env, exceptionClass, "NewLocalRef from weak ref is not the same object as weak.");
        ret = NULL;
        goto end;
    }
    else if ((*env)->IsSameObject(env, local, o) == JNI_FALSE) {
        (*env)->ThrowNew(env, exceptionClass, "NewLocalRef from weak ref is not the same object as passed.");
        ret = NULL;
        goto end;
    }

    ret = local;

end:
    if (weak != NULL) {
        (*env)->DeleteWeakGlobalRef(env, weak);
        weak = NULL;
    }

    return ret;
}

JNIEXPORT jint JNICALL JNI_OnLoad(JavaVM* vm, void* reserved)
{
    return JNI_VERSION_1_8;
}

JNIEXPORT void JNICALL JNI_OnUnload(JavaVM* vm, void* reserved)
{

}
