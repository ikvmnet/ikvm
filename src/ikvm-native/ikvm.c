#include "ikvm.h"

typedef int (*GetMethodArgs_t)(JNIEnv* pEnv, jmethodID method, char* sig);
#define GET_METHOD_ARGS(pEnv, method, sig) (((GetMethodArgs_t)((*pEnv)->reserved0))(pEnv, methodID, sig))

#define MAKE_ARG_ARRAY(pEnv, args, argarray) \
do { \
	jbyte sig[257];\
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

#define MAKE_METHOD(Type, type) \
EXPORT type JNI_Call##Type##Method(JNIEnv* pEnv, jobject obj, jmethodID methodID, ...)\
{\
	type ret;\
	va_list args;\
	va_start(args, methodID);\
	ret = (*pEnv)->Call##Type##MethodV(pEnv, obj, methodID, args);\
	va_end(args);\
	return ret;\
}\
EXPORT type JNI_Call##Type##MethodV(JNIEnv* pEnv, jobject obj, jmethodID methodID, va_list args)\
{\
	jvalue* argarray;\
	MAKE_ARG_ARRAY(pEnv, args, argarray);\
	return (*pEnv)->Call##Type##MethodA(pEnv, obj, methodID, argarray);\
}\
EXPORT type JNI_CallNonvirtual##Type##Method(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, ...)\
{\
	type ret;\
	va_list args;\
	va_start(args, methodID);\
	ret = (*pEnv)->CallNonvirtual##Type##MethodV(pEnv, obj, clazz, methodID, args);\
	va_end(args);\
	return ret;\
}\
EXPORT type JNI_CallNonvirtual##Type##MethodV(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, va_list args)\
{\
	jvalue* argarray;\
	MAKE_ARG_ARRAY(pEnv, args, argarray);\
	return (*pEnv)->CallNonvirtual##Type##MethodA(pEnv, obj, clazz, methodID, argarray);\
}\
EXPORT type JNI_CallStatic##Type##Method(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...)\
{\
	type ret;\
	va_list args;\
	va_start(args, methodID);\
	ret = (*pEnv)->CallStatic##Type##MethodV(pEnv, clazz, methodID, args);\
	va_end(args);\
	return ret;\
}\
EXPORT type JNI_CallStatic##Type##MethodV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args)\
{\
	jvalue* argarray;\
	MAKE_ARG_ARRAY(pEnv, args, argarray);\
	return (*pEnv)->CallStatic##Type##MethodA(pEnv, clazz, methodID, argarray);\
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

EXPORT jobject JNI_NewObject(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...)
{
	jobject o;
	va_list args;
	va_start(args, methodID);
	o = (*pEnv)->NewObjectV(pEnv, clazz, methodID, args);
	va_end(args);
	return o;
}

EXPORT jobject JNI_NewObjectV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args)
{
	jvalue* argarray;
	MAKE_ARG_ARRAY(pEnv, args, argarray);
	return (*pEnv)->NewObjectA(pEnv, clazz, methodID, argarray);
}

EXPORT void JNI_CallVoidMethod(JNIEnv* pEnv, jobject obj, jmethodID methodID, ...)
{
	va_list args;
	va_start(args, methodID);
	(*pEnv)->CallVoidMethodV(pEnv, obj, methodID, args);
	va_end(args);
}
EXPORT void JNI_CallVoidMethodV(JNIEnv* pEnv, jobject obj, jmethodID methodID, va_list args)
{
	jvalue* argarray;
	MAKE_ARG_ARRAY(pEnv, args, argarray);
	(*pEnv)->CallVoidMethodA(pEnv, obj, methodID, argarray);
}
EXPORT void JNI_CallNonvirtualVoidMethod(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, ...)
{
	va_list args;
	va_start(args, methodID);
	(*pEnv)->CallNonvirtualVoidMethodV(pEnv, obj, clazz, methodID, args);
	va_end(args);
}
EXPORT void JNI_CallNonvirtualVoidMethodV(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, va_list args)
{
	jvalue* argarray;
	MAKE_ARG_ARRAY(pEnv, args, argarray);
	(*pEnv)->CallNonvirtualVoidMethodA(pEnv, obj, clazz, methodID, argarray);
}
EXPORT void JNI_CallStaticVoidMethod(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...)
{
	va_list args;
	va_start(args, methodID);
	(*pEnv)->CallStaticVoidMethodV(pEnv, clazz, methodID, args);
	va_end(args);
}

EXPORT void JNI_CallStaticVoidMethodV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args)
{
	jvalue* argarray;
	MAKE_ARG_ARRAY(pEnv, args, argarray);
	(*pEnv)->CallStaticVoidMethodA(pEnv, clazz, methodID, argarray);
}
