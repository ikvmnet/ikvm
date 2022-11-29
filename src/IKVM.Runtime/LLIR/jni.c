#include "jni.h"

MAKE_METHOD(Object, jobject)
MAKE_METHOD(Boolean, jboolean)
MAKE_METHOD(Byte, jbyte)
MAKE_METHOD(Char, jchar)
MAKE_METHOD(Short, jshort)
MAKE_METHOD(Int, jint)
MAKE_METHOD(Long, jlong)
MAKE_METHOD(Float, jfloat)
MAKE_METHOD(Double, jdouble)

IMPL jobject JNI_NewObject(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...)
{
	jobject o;
	va_list args;
	va_start(args, methodID);
	o = (*pEnv)->NewObjectV(pEnv, clazz, methodID, args);
	va_end(args);
	return o;
}

IMPL jobject JNI_NewObjectV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args)
{
	jvalue* argarray;
	MAKE_ARG_ARRAY(pEnv, args, argarray);
	return (*pEnv)->NewObjectA(pEnv, clazz, methodID, argarray);
}

IMPL void JNI_CallVoidMethod(JNIEnv* pEnv, jobject obj, jmethodID methodID, ...)
{
	va_list args;
	va_start(args, methodID);
	(*pEnv)->CallVoidMethodV(pEnv, obj, methodID, args);
	va_end(args);
}

IMPL void JNI_CallVoidMethodV(JNIEnv* pEnv, jobject obj, jmethodID methodID, va_list args)
{
	jvalue* argarray;
	MAKE_ARG_ARRAY(pEnv, args, argarray);
	(*pEnv)->CallVoidMethodA(pEnv, obj, methodID, argarray);
}

IMPL void JNI_CallNonvirtualVoidMethod(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, ...)
{
	va_list args;
	va_start(args, methodID);
	(*pEnv)->CallNonvirtualVoidMethodV(pEnv, obj, clazz, methodID, args);
	va_end(args);
}

IMPL void JNI_CallNonvirtualVoidMethodV(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, va_list args)
{
	jvalue* argarray;
	MAKE_ARG_ARRAY(pEnv, args, argarray);
	(*pEnv)->CallNonvirtualVoidMethodA(pEnv, obj, clazz, methodID, argarray);
}

IMPL void JNI_CallStaticVoidMethod(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...)
{
	va_list args;
	va_start(args, methodID);
	(*pEnv)->CallStaticVoidMethodV(pEnv, clazz, methodID, args);
	va_end(args);
}

IMPL void JNI_CallStaticVoidMethodV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args)
{
	jvalue* argarray;
	MAKE_ARG_ARRAY(pEnv, args, argarray);
	(*pEnv)->CallStaticVoidMethodA(pEnv, clazz, methodID, argarray);
}
