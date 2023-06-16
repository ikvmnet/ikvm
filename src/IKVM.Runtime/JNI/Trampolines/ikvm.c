#include "ikvm.h"

JNIEXPORT void JNICALL NOOP(JNIEnv* pEnv)
{

}

MAKE_METHOD(Object, jobject)
MAKE_METHOD(Boolean, jboolean)
MAKE_METHOD(Byte, jbyte)
MAKE_METHOD(Char, jchar)
MAKE_METHOD(Short, jshort)
MAKE_METHOD(Int, jint)
MAKE_METHOD(Long, jlong)
MAKE_METHOD(Float, jfloat)
MAKE_METHOD(Double, jdouble)

JNIEXPORT jobject JNICALL JNI_NewObject(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...)
{
    va_list args;
    va_start(args, methodID);
    MAKE_ARG_ARRAY(pEnv, args);
    jobject o = (*pEnv)->NewObjectA(pEnv, clazz, methodID, argv);
    va_end(args);
    return o;
}

JNIEXPORT jobject JNICALL JNI_NewObjectV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args)
{
    MAKE_ARG_ARRAY(pEnv, args);
    return (*pEnv)->NewObjectA(pEnv, clazz, methodID, argv);
}

JNIEXPORT void JNICALL JNI_CallVoidMethod(JNIEnv* pEnv, jobject obj, jmethodID methodID, ...)
{
    va_list args;
    va_start(args, methodID);
    MAKE_ARG_ARRAY(pEnv, args);
    (*pEnv)->CallVoidMethodA(pEnv, obj, methodID, argv);
    va_end(args);
}

JNIEXPORT void JNICALL JNI_CallVoidMethodV(JNIEnv* pEnv, jobject obj, jmethodID methodID, va_list args)
{
    MAKE_ARG_ARRAY(pEnv, args);
    (*pEnv)->CallVoidMethodA(pEnv, obj, methodID, argv);
}

JNIEXPORT void JNICALL JNI_CallNonvirtualVoidMethod(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, ...)
{
    va_list args;
    va_start(args, methodID);
    MAKE_ARG_ARRAY(pEnv, args);
    (*pEnv)->CallNonvirtualVoidMethodA(pEnv, obj, clazz, methodID, argv);
    va_end(args);
}

JNIEXPORT void JNICALL JNI_CallNonvirtualVoidMethodV(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, va_list args)
{
    MAKE_ARG_ARRAY(pEnv, args);
    (*pEnv)->CallNonvirtualVoidMethodA(pEnv, obj, clazz, methodID, argv);
}

JNIEXPORT void JNICALL JNI_CallStaticVoidMethod(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...)
{
    va_list args;
    va_start(args, methodID);
    MAKE_ARG_ARRAY(pEnv, args);
    (*pEnv)->CallStaticVoidMethodA(pEnv, clazz, methodID, argv);
    va_end(args);
}

JNIEXPORT void JNICALL JNI_CallStaticVoidMethodV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args)
{
    MAKE_ARG_ARRAY(pEnv, args);
    (*pEnv)->CallStaticVoidMethodA(pEnv, clazz, methodID, argv);
}
