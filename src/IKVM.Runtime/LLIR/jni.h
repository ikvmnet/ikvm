#ifndef IKVM_H_INCLUDED
#define IKVM_H_INCLUDED

#ifdef _WIN32
#define IMPL __declspec(dllexport) __attribute__((always_inline))
#define HEAD __declspec(dllexport) __attribute__((always_inline))
#else
#if (defined(__GNUC__) && ((__GNUC__ > 4) || (__GNUC__ == 4) && (__GNUC_MINOR__ > 2))) || __has_attribute(visibility)
#define IMPL __attribute__((always_inline)) __attribute__((visibility("default")))
#define HEAD __attribute__((always_inline)) __attribute__((visibility("default")))
#else
#define IMPL __attribute__((always_inline)) __attribute__((visibility("default")))
#define HEAD __attribute__((always_inline)) __attribute__((visibility("default")))
#endif
#endif

#include <stdarg.h>
#include <jni.h>

#ifdef _WIN32
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

#define MAKE_ARG_ARRAY(pEnv, args, argarray) \
do { \
	char sig[257];\
	int argc = GET_METHOD_ARGS(pEnv, methodID, sig);\
	int i;\
	argarray = (jvalue*)ALLOCA((long unsigned int)argc * sizeof(jvalue));\
	for (i = 0; i < argc; i++)\
	{\
		switch (sig[i])\
		{\
			case 'Z':\
				argarray[i].z = (jboolean)va_arg(args, int);\
				break;\
			case 'B':\
				argarray[i].b = (jbyte)va_arg(args, int);\
				break;\
			case 'S':\
				argarray[i].s = (jshort)va_arg(args, int);\
				break;\
			case 'C':\
				argarray[i].i = (jchar)va_arg(args, int);\
				break;\
			case 'I':\
				argarray[i].i = (jint)va_arg(args, int);\
				break;\
			case 'J':\
				argarray[i].j = (jlong)va_arg(args, long);\
				break;\
			case 'D':\
				argarray[i].d = (jdouble)va_arg(args, double);\
				break;\
			case 'F':\
				argarray[i].f = (jfloat)va_arg(args, double);\
				break;\
			case 'L':\
				argarray[i].l = (jobject)va_arg(args, void*);\
				break;\
		}\
	}\
} while(0);

#define MAKE_METHOD_SIGNATURE(Type, type) \
HEAD type JNI_Call##Type##Method(JNIEnv* pEnv, jobject obj, jmethodID methodID, ...);\
HEAD type JNI_Call##Type##MethodV(JNIEnv* pEnv, jobject obj, jmethodID methodID, va_list args);\
HEAD type JNI_CallNonvirtual##Type##Method(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, ...);\
HEAD type JNI_CallNonvirtual##Type##MethodV(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, va_list args);\
HEAD type JNI_CallStatic##Type##Method(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...);\
HEAD type JNI_CallStatic##Type##MethodV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args);

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
IMPL type JNI_Call##Type##Method(JNIEnv* pEnv, jobject obj, jmethodID methodID, ...)\
{\
	type ret;\
	va_list args;\
	va_start(args, methodID);\
	ret = (*pEnv)->Call##Type##MethodV(pEnv, obj, methodID, args);\
	va_end(args);\
	return ret;\
}\
IMPL type JNI_Call##Type##MethodV(JNIEnv* pEnv, jobject obj, jmethodID methodID, va_list args)\
{\
	jvalue* argarray;\
	MAKE_ARG_ARRAY(pEnv, args, argarray);\
	return (*pEnv)->Call##Type##MethodA(pEnv, obj, methodID, argarray);\
}\
IMPL type JNI_CallNonvirtual##Type##Method(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, ...)\
{\
	type ret;\
	va_list args;\
	va_start(args, methodID);\
	ret = (*pEnv)->CallNonvirtual##Type##MethodV(pEnv, obj, clazz, methodID, args);\
	va_end(args);\
	return ret;\
}\
IMPL type JNI_CallNonvirtual##Type##MethodV(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, va_list args)\
{\
	jvalue* argarray;\
	MAKE_ARG_ARRAY(pEnv, args, argarray);\
	return (*pEnv)->CallNonvirtual##Type##MethodA(pEnv, obj, clazz, methodID, argarray);\
}\
IMPL type JNI_CallStatic##Type##Method(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...)\
{\
	type ret;\
	va_list args;\
	va_start(args, methodID);\
	ret = (*pEnv)->CallStatic##Type##MethodV(pEnv, clazz, methodID, args);\
	va_end(args);\
	return ret;\
}\
IMPL type JNI_CallStatic##Type##MethodV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args)\
{\
	jvalue* argarray;\
	MAKE_ARG_ARRAY(pEnv, args, argarray);\
	return (*pEnv)->CallStatic##Type##MethodA(pEnv, clazz, methodID, argarray);\
}

HEAD jobject JNI_NewObject(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...);
HEAD jobject JNI_NewObjectV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args);
HEAD void JNI_CallVoidMethod(JNIEnv* pEnv, jobject obj, jmethodID methodID, ...);
HEAD void JNI_CallVoidMethodV(JNIEnv* pEnv, jobject obj, jmethodID methodID, va_list args);
HEAD void JNI_CallNonvirtualVoidMethod(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, ...);
HEAD void JNI_CallNonvirtualVoidMethodV(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, va_list args);
HEAD void JNI_CallStaticVoidMethod(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...);
HEAD void JNI_CallStaticVoidMethodV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args);

#endif
