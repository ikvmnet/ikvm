#ifndef JNI_VARGS_H_INCLUDED
#define JNI_VARGS_H_INCLUDED

#include <stdarg.h>
#include <jni.h>

typedef int (*GetMethodArgs_t)(JNIEnv* pEnv, jmethodID method, char* sig);
#define GET_METHOD_ARGS(pEnv, method, sig) (((GetMethodArgs_t)((*pEnv)->reserved0))(pEnv, methodID, sig))

#define MAKE_ARG_ARRAY(pEnv, args) \
	char sig[257];\
	int argc = GET_METHOD_ARGS(pEnv, methodID, sig);\
	jvalue *argv = (jvalue*)__builtin_alloca((long unsigned int)argc * sizeof(jvalue));\
	for (int i = 0; i < argc; i++)\
	{\
		if (sig[i] == 'Z')\
			argv[i].z = (jboolean)va_arg(args, int);\
		else if (sig[i] == 'B')\
			argv[i].b = (jbyte)va_arg(args, int);\
		else if (sig[i] == 'C')\
			argv[i].c = (jchar)va_arg(args, int);\
		else if (sig[i] == 'S')\
			argv[i].s = (jshort)va_arg(args, int);\
		else if (sig[i] == 'I')\
			argv[i].i = (jint)va_arg(args, int);\
		else if (sig[i] == 'J')\
			argv[i].j = (jlong)va_arg(args, jlong);\
		else if (sig[i] == 'F')\
			argv[i].f = (jfloat)va_arg(args, double);\
		else if (sig[i] == 'D')\
			argv[i].d = (jdouble)va_arg(args, double);\
		else if (sig[i] == 'L')\
			argv[i].l = (jobject)va_arg(args, void*);\
	}


#define MAKE_METHOD_SIGNATURE(Type, type) \
JNIEXPORT type JNICALL __JNI_Call##Type##Method(JNIEnv* pEnv, jobject obj, jmethodID methodID, ...);\
JNIEXPORT type JNICALL __JNI_Call##Type##MethodV(JNIEnv* pEnv, jobject obj, jmethodID methodID, va_list args);\
JNIEXPORT type JNICALL __JNI_CallNonvirtual##Type##Method(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, ...);\
JNIEXPORT type JNICALL __JNI_CallNonvirtual##Type##MethodV(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, va_list args);\
JNIEXPORT type JNICALL __JNI_CallStatic##Type##Method(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...);\
JNIEXPORT type JNICALL __JNI_CallStatic##Type##MethodV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args);

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
JNIEXPORT type JNICALL __JNI_Call##Type##Method(JNIEnv* pEnv, jobject obj, jmethodID methodID, ...)\
{\
	va_list args;\
	va_start(args, methodID);\
    MAKE_ARG_ARRAY(pEnv, args);\
    type ret = (*pEnv)->Call##Type##MethodA(pEnv, obj, methodID, argv);\
	va_end(args);\
	return ret;\
}\
JNIEXPORT type JNICALL __JNI_Call##Type##MethodV(JNIEnv* pEnv, jobject obj, jmethodID methodID, va_list args)\
{\
	MAKE_ARG_ARRAY(pEnv, args);\
	return (*pEnv)->Call##Type##MethodA(pEnv, obj, methodID, argv);\
}\
JNIEXPORT type JNICALL __JNI_CallNonvirtual##Type##Method(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, ...)\
{\
	va_list args;\
	va_start(args, methodID);\
	MAKE_ARG_ARRAY(pEnv, args);\
    type ret = (*pEnv)->CallNonvirtual##Type##MethodA(pEnv, obj, clazz, methodID, argv);\
	va_end(args);\
	return ret;\
}\
JNIEXPORT type JNICALL __JNI_CallNonvirtual##Type##MethodV(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, va_list args)\
{\
	MAKE_ARG_ARRAY(pEnv, args);\
	return (*pEnv)->CallNonvirtual##Type##MethodA(pEnv, obj, clazz, methodID, argv);\
}\
JNIEXPORT type JNICALL __JNI_CallStatic##Type##Method(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...)\
{\
	va_list args;\
	va_start(args, methodID);\
    MAKE_ARG_ARRAY(pEnv, args);\
    type ret = (*pEnv)->CallStatic##Type##MethodA(pEnv, clazz, methodID, argv);\
	va_end(args);\
	return ret;\
}\
JNIEXPORT type JNICALL __JNI_CallStatic##Type##MethodV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args)\
{\
	MAKE_ARG_ARRAY(pEnv, args);\
	return (*pEnv)->CallStatic##Type##MethodA(pEnv, clazz, methodID, argv);\
}

JNIEXPORT jobject JNICALL __JNI_NewObject(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...);
JNIEXPORT jobject JNICALL __JNI_NewObjectV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args);
JNIEXPORT void JNICALL __JNI_CallVoidMethod(JNIEnv* pEnv, jobject obj, jmethodID methodID, ...);
JNIEXPORT void JNICALL __JNI_CallVoidMethodV(JNIEnv* pEnv, jobject obj, jmethodID methodID, va_list args);
JNIEXPORT void JNICALL __JNI_CallNonvirtualVoidMethod(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, ...);
JNIEXPORT void JNICALL __JNI_CallNonvirtualVoidMethodV(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, va_list args);
JNIEXPORT void JNICALL __JNI_CallStaticVoidMethod(JNIEnv* pEnv, jclass clazz, jmethodID methodID, ...);
JNIEXPORT void JNICALL __JNI_CallStaticVoidMethodV(JNIEnv* pEnv, jclass clazz, jmethodID methodID, va_list args);

#endif
