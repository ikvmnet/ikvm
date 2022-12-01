#ifndef IKVM_H_INCLUDED
#define IKVM_H_INCLUDED

#include <stdarg.h>
#include <jni.h>

#define EXPOR

#if defined(_WIN32) || defined(_WIN64)
#include <malloc.h>
#define ALLOCA _alloca
#else
#if defined(__FreeBSD__) || defined(__NetBSD__) || defined(__OpenBSD__)
#include <stdlib.h>
#else
#include <alloca.h>
#endif
#define ALLOCA alloca
#endif

typedef int (*GetMethodArgs_t)(JNIEnv* pEnv, jmethodID method, char* sig);
#define GET_METHOD_ARGS(pEnv, method, sig) (((GetMethodArgs_t)((*pEnv)->reserved0))(pEnv, methodID, sig))

#define MAKE_ARG_ARRAY(pEnv, args) \
	char sig[256];\
	int argc = GET_METHOD_ARGS(pEnv, methodID, sig);\
	jvalue argv[256];\
	for (int i = 0; i < argc; i++)\
	{\
		switch (sig[i])\
		{\
			case 'Z':\
				argv[i].z = (jboolean)va_arg(args, int);\
				break;\
			case 'B':\
				argv[i].b = (jbyte)va_arg(args, int);\
				break;\
			case 'C':\
				argv[i].c = (jchar)va_arg(args, int);\
				break;\
			case 'S':\
				argv[i].s = (jshort)va_arg(args, int);\
				break;\
			case 'I':\
				argv[i].i = (jint)va_arg(args, int);\
				break;\
			case 'J':\
				argv[i].j = (jlong)va_arg(args, long);\
				break;\
			case 'F':\
				argv[i].f = (jfloat)va_arg(args, double);\
				break;\
			case 'D':\
				argv[i].d = (jdouble)va_arg(args, double);\
				break;\
			case 'L':\
				argv[i].l = (jobject)va_arg(args, void*);\
				break;\
			default:\
				break; \
		}\
	}

#define MAKE_METHOD_SIGNATURE(Type, type) \
JNIEXPORT type JNICALL JNI_Call##Type##Method(JNIEnv* pEnv, jobject obj, jmethodID methodID, ...);\
JNIEXPORT type JNICALL JNI_Call##Type##MethodV(JNIEnv* pEnv, jobject obj, jmethodID methodID, va_list args);\
JNIEXPORT type JNICALL JNI_CallNonvirtual##Type##Method(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, ...);\
JNIEXPORT type JNICALL JNI_CallNonvirtual##Type##MethodV(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, va_list args);\
JNIEXPORT type JNICALL JNI_CallStatic##Type##Method(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...);\
JNIEXPORT type JNICALL JNI_CallStatic##Type##MethodV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args);

MAKE_METHOD_SIGNATURE(Object, jobject)
MAKE_METHOD_SIGNATURE(Boolean, jboolean)
MAKE_METHOD_SIGNATURE(Byte, jbyte)
MAKE_METHOD_SIGNATURE(Char, jchar)
MAKE_METHOD_SIGNATURE(Short, jshort)
MAKE_METHOD_SIGNATURE(Int, jint)
MAKE_METHOD_SIGNATURE(Long, jlong)
MAKE_METHOD_SIGNATURE(Float, jfloat)
MAKE_METHOD_SIGNATURE(Double, jdouble)

#define MAKE_METHOD(Type, type) \
JNIEXPORT type JNICALL JNI_Call##Type##Method(JNIEnv* pEnv, jobject obj, jmethodID methodID, ...)\
{\
	va_list args;\
	va_start(args, methodID);\
    MAKE_ARG_ARRAY(pEnv, args);\
    type ret = (*pEnv)->Call##Type##MethodA(pEnv, obj, methodID, argv);\
	va_end(args);\
	return ret;\
}\
JNIEXPORT type JNICALL JNI_Call##Type##MethodV(JNIEnv* pEnv, jobject obj, jmethodID methodID, va_list args)\
{\
	MAKE_ARG_ARRAY(pEnv, args);\
	return (*pEnv)->Call##Type##MethodA(pEnv, obj, methodID, argv);\
}\
JNIEXPORT type JNICALL JNI_CallNonvirtual##Type##Method(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, ...)\
{\
	va_list args;\
	va_start(args, methodID);\
	MAKE_ARG_ARRAY(pEnv, args);\
    type ret = (*pEnv)->CallNonvirtual##Type##MethodA(pEnv, obj, clazz, methodID, argv);\
	va_end(args);\
	return ret;\
}\
JNIEXPORT type JNICALL JNI_CallNonvirtual##Type##MethodV(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, va_list args)\
{\
	MAKE_ARG_ARRAY(pEnv, args);\
	return (*pEnv)->CallNonvirtual##Type##MethodA(pEnv, obj, clazz, methodID, argv);\
}\
JNIEXPORT type JNICALL JNI_CallStatic##Type##Method(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...)\
{\
	va_list args;\
	va_start(args, methodID);\
    MAKE_ARG_ARRAY(pEnv, args);\
    type ret = (*pEnv)->CallStatic##Type##MethodA(pEnv, clazz, methodID, argv);\
	va_end(args);\
	return ret;\
}\
JNIEXPORT type JNICALL JNI_CallStatic##Type##MethodV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args)\
{\
	MAKE_ARG_ARRAY(pEnv, args);\
	return (*pEnv)->CallStatic##Type##MethodA(pEnv, clazz, methodID, argv);\
}

JNIEXPORT jobject JNICALL JNI_NewObject(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...);
JNIEXPORT jobject JNICALL JNI_NewObjectV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args);
JNIEXPORT void JNICALL JNI_CallVoidMethod(JNIEnv* pEnv, jobject obj, jmethodID methodID, ...);
JNIEXPORT void JNICALL JNI_CallVoidMethodV(JNIEnv* pEnv, jobject obj, jmethodID methodID, va_list args);
JNIEXPORT void JNICALL JNI_CallNonvirtualVoidMethod(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, ...);
JNIEXPORT void JNICALL JNI_CallNonvirtualVoidMethodV(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, va_list args);
JNIEXPORT void JNICALL JNI_CallStaticVoidMethod(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...);
JNIEXPORT void JNICALL JNI_CallStaticVoidMethodV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args);

#endif
