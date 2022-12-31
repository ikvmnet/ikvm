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

JNIEXPORT jobject JNICALL Java_ikvm_tests_jni_JniTests_getObjectField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "objectField", "Ljava/lang/Object;");
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return JNI_FALSE;
    }

    return (*env)->GetObjectField(env, object, field);
}

JNIEXPORT jstring JNICALL Java_ikvm_tests_jni_JniTests_getStringField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "stringField", "Ljava/lang/String;");
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return JNI_FALSE;
    }

    return (*env)->GetObjectField(env, object, field);
}

JNIEXPORT jboolean JNICALL Java_ikvm_tests_jni_JniTests_getBooleanField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "booleanField", "Z");
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return JNI_FALSE;
    }

    return (*env)->GetBooleanField(env, object, field);
}

JNIEXPORT jbyte JNICALL Java_ikvm_tests_jni_JniTests_getByteField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "byteField", "B");
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    return (*env)->GetByteField(env, object, field);
}

JNIEXPORT jchar JNICALL Java_ikvm_tests_jni_JniTests_getCharField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "charField", "C");
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    return (*env)->GetCharField(env, object, field);
}

JNIEXPORT jshort JNICALL Java_ikvm_tests_jni_JniTests_getShortField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "shortField", "S");
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    return (*env)->GetShortField(env, object, field);
}

JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniTests_getIntField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "intField", "I");
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    return (*env)->GetIntField(env, object, field);
}

JNIEXPORT jlong JNICALL Java_ikvm_tests_jni_JniTests_getLongField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "longField", "J");
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    return (*env)->GetLongField(env, object, field);
}

JNIEXPORT jfloat JNICALL Java_ikvm_tests_jni_JniTests_getFloatField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "floatField", "F");
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    return (*env)->GetFloatField(env, object, field);
}

JNIEXPORT jdouble JNICALL Java_ikvm_tests_jni_JniTests_getDoubleField(JNIEnv* env, jobject self, jclass clazz, jobject object)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetFieldID(env, clazz, "doubleField", "D");
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    return (*env)->GetDoubleField(env, object, field);
}

JNIEXPORT jobject JNICALL Java_ikvm_tests_jni_JniTests_getStaticObjectField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "objectField", "Ljava/lang/Object;");
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return NULL;
    }

    return (*env)->GetStaticObjectField(env, clazz, field);
}

JNIEXPORT jstring JNICALL Java_ikvm_tests_jni_JniTests_getStaticStringField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "stringField", "Ljava/lang/String;");
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return NULL;
    }

    return (*env)->GetStaticObjectField(env, clazz, field);
}

JNIEXPORT jboolean JNICALL Java_ikvm_tests_jni_JniTests_getStaticBooleanField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "booleanField", "Z");
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return JNI_FALSE;
    }

    return (*env)->GetStaticBooleanField(env, clazz, field);
}

JNIEXPORT jbyte JNICALL Java_ikvm_tests_jni_JniTests_getStaticByteField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "byteField", "B");
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    return (*env)->GetStaticByteField(env, clazz, field);
}

JNIEXPORT jchar JNICALL Java_ikvm_tests_jni_JniTests_getStaticCharField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "charField", "C");
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    return (*env)->GetStaticCharField(env, clazz, field);
}

JNIEXPORT jshort JNICALL Java_ikvm_tests_jni_JniTests_getStaticShortField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "shortField", "S");
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    return (*env)->GetStaticShortField(env, clazz, field);
}

JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniTests_getStaticIntField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "intField", "I");
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    return (*env)->GetStaticIntField(env, clazz, field);
}

JNIEXPORT jlong JNICALL Java_ikvm_tests_jni_JniTests_getStaticLongField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "longField", "J");
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    return (*env)->GetStaticLongField(env, clazz, field);
}

JNIEXPORT jfloat JNICALL Java_ikvm_tests_jni_JniTests_getStaticFloatField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "floatField", "F");
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    return (*env)->GetStaticFloatField(env, clazz, field);
}

JNIEXPORT jdouble JNICALL Java_ikvm_tests_jni_JniTests_getStaticDoubleField(JNIEnv* env, jobject self, jclass clazz)
{
    jclass exceptionClass = (*env)->FindClass(env, "java/lang/Exception");

    jfieldID field = (*env)->GetStaticFieldID(env, clazz, "doubleField", "D");
    if (field == NULL) {
        (*env)->ThrowNew(env, exceptionClass, "failed");
        return 0;
    }

    return (*env)->GetStaticDoubleField(env, clazz, field);
}

JNIEXPORT jint JNICALL JNI_OnLoad(JavaVM* vm, void* reserved)
{
    return JNI_VERSION_1_8;
}

JNIEXPORT void JNICALL JNI_OnUnload(JavaVM* vm, void* reserved)
{

}
