#include "jni_vargs.h"

MAKE_METHOD(Object, jobject)
MAKE_METHOD(Boolean, jboolean)
MAKE_METHOD(Byte, jbyte)
MAKE_METHOD(Char, jchar)
MAKE_METHOD(Short, jshort)
MAKE_METHOD(Int, jint)
MAKE_METHOD(Long, jlong)
MAKE_METHOD(Float, jfloat)
MAKE_METHOD(Double, jdouble)

JNIEXPORT jobject JNICALL __JNI_NewObject(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...)
{
    va_list args;
    va_start(args, methodID);
    jobject o = (*pEnv)->NewObjectV(pEnv, clazz, methodID, args);
    va_end(args);
    return o;
}

JNIEXPORT jobject JNICALL __JNI_NewObjectV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args)
{
	jvalue *argv;
    MAKE_ARG_ARRAY(pEnv, methodID, args, argv);
    return (*pEnv)->NewObjectA(pEnv, clazz, methodID, argv);
}

JNIEXPORT void JNICALL __JNI_CallVoidMethod(JNIEnv* pEnv, jobject obj, jmethodID methodID, ...)
{
    va_list args;
    va_start(args, methodID);
    (*pEnv)->CallVoidMethodV(pEnv, obj, methodID, args);
    va_end(args);
}

JNIEXPORT void JNICALL __JNI_CallVoidMethodV(JNIEnv* pEnv, jobject obj, jmethodID methodID, va_list args)
{
    jvalue *argv;
    MAKE_ARG_ARRAY(pEnv, methodID, args, argv);
    (*pEnv)->CallVoidMethodA(pEnv, obj, methodID, argv);
}

JNIEXPORT void JNICALL __JNI_CallNonvirtualVoidMethod(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, ...)
{
    va_list args;
    va_start(args, methodID);
    (*pEnv)->CallNonvirtualVoidMethodV(pEnv, obj, clazz, methodID, args);
    va_end(args);
}

JNIEXPORT void JNICALL __JNI_CallNonvirtualVoidMethodV(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, va_list args)
{
    jvalue *argv;
    MAKE_ARG_ARRAY(pEnv, methodID, args, argv);
    (*pEnv)->CallNonvirtualVoidMethodA(pEnv, obj, clazz, methodID, argv);
}

JNIEXPORT void JNICALL __JNI_CallStaticVoidMethod(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...)
{
    va_list args;
    va_start(args, methodID);
    (*pEnv)->CallStaticVoidMethodV(pEnv, clazz, methodID, args);
    va_end(args);
}

JNIEXPORT void JNICALL __JNI_CallStaticVoidMethodV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args)
{
    jvalue *argv;
    MAKE_ARG_ARRAY(pEnv, methodID, args, argv);
    (*pEnv)->CallStaticVoidMethodA(pEnv, clazz, methodID, argv);
}
