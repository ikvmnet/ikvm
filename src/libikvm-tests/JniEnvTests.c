#include <stdarg.h>
#include <stddef.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <jni.h>

#ifdef _WIN32
#include <windows.h>
#else
#include <pthread.h>
#endif

// Native half of ikvm.tests.jni.JniEnvTests. Each native method checks its own results and reports a mismatch by
// throwing JniEnvTests$Failure, with the exception that was pending at the time as its cause.

#define TESTS_CLASS "ikvm/tests/jni/JniEnvTests"
#define FAILURE_CLASS TESTS_CLASS "$Failure"
#define TARGET_CLASS TESTS_CLASS "$Target"
#define SUB_TARGET_CLASS TESTS_CLASS "$SubTarget"
#define TARGET_INTERFACE TESTS_CLASS "$TargetInterface"
#define ABSTRACT_TARGET_CLASS TESTS_CLASS "$AbstractTarget"

// signature of the parameters every target method takes
#define ARGS_SIG "(ZBCSIJFDLjava/lang/Object;)"

// the arguments every target method is called with, see Target.format for how they are recorded
#define VARARGS(l) JNI_TRUE, (jbyte)-5, (jchar)0x263A, (jshort)-300, (jint)70000, (jlong)0x123456789ALL, (jfloat)0.5f, (jdouble)-0.125, (l)

static void fillArgs(jvalue* v, jobject l)
{
    v[0].z = JNI_TRUE;
    v[1].b = -5;
    v[2].c = 0x263A;
    v[3].s = -300;
    v[4].i = 70000;
    v[5].j = 0x123456789ALL;
    v[6].f = 0.5f;
    v[7].d = -0.125;
    v[8].l = l;
}

static void fail(JNIEnv* env, const char* func, int line, const char* what)
{
    char message[1024];
    snprintf(message, sizeof(message), "%s:%d: %s", func, line, what);

    // whatever is pending becomes the cause of the failure
    jthrowable cause = (*env)->ExceptionOccurred(env);
    (*env)->ExceptionClear(env);

    jclass failureClass = (*env)->FindClass(env, FAILURE_CLASS);
    if (failureClass == NULL)
        return;

    jmethodID ctor = (*env)->GetMethodID(env, failureClass, "<init>", "(Ljava/lang/String;Ljava/lang/Throwable;)V");
    if (ctor == NULL)
        return;

    jstring s = (*env)->NewStringUTF(env, message);
    if (s == NULL)
        return;

    jobject failure = (*env)->NewObject(env, failureClass, ctor, s, cause);
    if (failure != NULL)
        (*env)->Throw(env, (jthrowable)failure);
}

#define FAIL(what) fail(env, __func__, __LINE__, what)
#define CHECK_R(cond, ret) do { if (!(cond)) { FAIL(#cond); return ret; } } while (0)
#define CHECK(cond) CHECK_R(cond, )
#define NO_EXCEPTION_R(ret) CHECK_R((*env)->ExceptionCheck(env) == JNI_FALSE, ret)
#define NO_EXCEPTION() NO_EXCEPTION_R()

// Clears the pending exception and returns true if it is an instance of the named class. Otherwise leaves it pending,
// so that it becomes the cause of the failure.
static jboolean takeException(JNIEnv* env, const char* className)
{
    jthrowable t = (*env)->ExceptionOccurred(env);
    if (t == NULL)
        return JNI_FALSE;

    (*env)->ExceptionClear(env);
    jclass c = (*env)->FindClass(env, className);
    if (c != NULL && (*env)->IsInstanceOf(env, t, c) == JNI_TRUE)
        return JNI_TRUE;

    (*env)->ExceptionClear(env);
    (*env)->Throw(env, t);
    return JNI_FALSE;
}

#define EXPECT_EXCEPTION_R(className, ret) CHECK_R(takeException(env, className), ret)
#define EXPECT_EXCEPTION(className) EXPECT_EXCEPTION_R(className, )

static jboolean callEquals(JNIEnv* env, jobject a, jobject b)
{
    jclass objectClass = (*env)->FindClass(env, "java/lang/Object");
    jmethodID equals = (*env)->GetMethodID(env, objectClass, "equals", "(Ljava/lang/Object;)Z");
    return (*env)->CallBooleanMethod(env, a, equals, b);
}

static jboolean stringEquals(JNIEnv* env, jstring s, const char* expected)
{
    const char* chars = (*env)->GetStringUTFChars(env, s, NULL);
    if (chars == NULL)
        return JNI_FALSE;

    jboolean result = strcmp(chars, expected) == 0;
    (*env)->ReleaseStringUTFChars(env, s, chars);
    return result;
}

static jboolean holdsLock(JNIEnv* env, jobject o)
{
    jclass threadClass = (*env)->FindClass(env, "java/lang/Thread");
    jmethodID m = (*env)->GetStaticMethodID(env, threadClass, "holdsLock", "(Ljava/lang/Object;)Z");
    return (*env)->CallStaticBooleanMethod(env, threadClass, m, o);
}

static jobject newTarget(JNIEnv* env, const char* className)
{
    jclass c = (*env)->FindClass(env, className);
    if (c == NULL)
        return NULL;

    jmethodID ctor = (*env)->GetMethodID(env, c, "<init>", "()V");
    if (ctor == NULL)
        return NULL;

    return (*env)->NewObject(env, c, ctor);
}

static jobject newIllegalStateException(JNIEnv* env, const char* message)
{
    jclass c = (*env)->FindClass(env, "java/lang/IllegalStateException");
    jmethodID ctor = (*env)->GetMethodID(env, c, "<init>", "(Ljava/lang/String;)V");
    return (*env)->NewObject(env, c, ctor, (*env)->NewStringUTF(env, message));
}

// version and classes

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getVersion(JNIEnv* env, jobject self)
{
    CHECK((*env)->GetVersion(env) == JNI_VERSION_1_8);
}

JNIEXPORT jclass JNICALL Java_ikvm_tests_jni_JniEnvTests_defineClass(JNIEnv* env, jobject self, jstring name, jobject loader, jbyteArray bytes)
{
    jsize length = (*env)->GetArrayLength(env, bytes);
    jbyte* buf = (jbyte*)malloc(length + 1);
    (*env)->GetByteArrayRegion(env, bytes, 0, length, buf);

    const char* n = (*env)->GetStringUTFChars(env, name, NULL);
    jclass c = (*env)->DefineClass(env, n, loader, buf, length);
    (*env)->ReleaseStringUTFChars(env, name, n);
    free(buf);

    return c;
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_defineClassWithBadBytes(JNIEnv* env, jobject self, jobject loader)
{
    jbyte buf[] = { (jbyte)0xCA, (jbyte)0xFE, (jbyte)0xBA, (jbyte)0xBE, 0, 0 };
    jclass c = (*env)->DefineClass(env, "ikvm/tests/jni/Bad", loader, buf, sizeof(buf));
    CHECK(c == NULL);
    EXPECT_EXCEPTION("java/lang/ClassFormatError");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_findClass(JNIEnv* env, jobject self)
{
    jclass stringClass = (*env)->FindClass(env, "java/lang/String");
    CHECK(stringClass != NULL);
    CHECK((*env)->IsSameObject(env, stringClass, (*env)->GetObjectClass(env, (*env)->NewStringUTF(env, "s"))));

    jclass intArrayClass = (*env)->FindClass(env, "[I");
    CHECK(intArrayClass != NULL);
    CHECK((*env)->IsSameObject(env, intArrayClass, (*env)->GetObjectClass(env, (*env)->NewIntArray(env, 1))));

    jclass stringArrayClass = (*env)->FindClass(env, "[Ljava/lang/String;");
    CHECK(stringArrayClass != NULL);
    CHECK((*env)->IsSameObject(env, stringArrayClass, (*env)->GetObjectClass(env, (*env)->NewObjectArray(env, 1, stringClass, NULL))));
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_findClassVisibleOnlyToTheCallersLoader(JNIEnv* env, jobject self)
{
    jclass c = (*env)->FindClass(env, TARGET_CLASS);
    CHECK(c != NULL);
    CHECK((*env)->IsSameObject(env, (*env)->GetObjectClass(env, newTarget(env, TARGET_CLASS)), c));
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_findClassNotFound(JNIEnv* env, jobject self)
{
    CHECK((*env)->FindClass(env, "does/not/Exist") == NULL);
    EXPECT_EXCEPTION("java/lang/NoClassDefFoundError");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_findClassWithDottedName(JNIEnv* env, jobject self)
{
    CHECK((*env)->FindClass(env, "java.lang.String") == NULL);
    EXPECT_EXCEPTION("java/lang/NoClassDefFoundError");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getSuperclass(JNIEnv* env, jobject self)
{
    jclass objectClass = (*env)->FindClass(env, "java/lang/Object");
    jclass numberClass = (*env)->FindClass(env, "java/lang/Number");
    jclass targetClass = (*env)->FindClass(env, TARGET_CLASS);
    jclass subTargetClass = (*env)->FindClass(env, SUB_TARGET_CLASS);

    CHECK((*env)->IsSameObject(env, (*env)->GetSuperclass(env, numberClass), objectClass));
    CHECK((*env)->IsSameObject(env, (*env)->GetSuperclass(env, subTargetClass), targetClass));
    CHECK((*env)->IsSameObject(env, (*env)->GetSuperclass(env, (*env)->FindClass(env, "[I")), objectClass));
    CHECK((*env)->GetSuperclass(env, objectClass) == NULL);
    NO_EXCEPTION();

    // an interface has no superclass
    CHECK((*env)->GetSuperclass(env, (*env)->FindClass(env, TARGET_INTERFACE)) == NULL);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_isAssignableFrom(JNIEnv* env, jobject self)
{
    jclass objectClass = (*env)->FindClass(env, "java/lang/Object");
    jclass numberClass = (*env)->FindClass(env, "java/lang/Number");
    jclass integerClass = (*env)->FindClass(env, "java/lang/Integer");
    jclass stringClass = (*env)->FindClass(env, "java/lang/String");
    jclass charSequenceClass = (*env)->FindClass(env, "java/lang/CharSequence");
    jclass intArrayClass = (*env)->FindClass(env, "[I");
    jclass objectArrayClass = (*env)->FindClass(env, "[Ljava/lang/Object;");
    jclass stringArrayClass = (*env)->FindClass(env, "[Ljava/lang/String;");

    CHECK((*env)->IsAssignableFrom(env, integerClass, numberClass) == JNI_TRUE);
    CHECK((*env)->IsAssignableFrom(env, numberClass, integerClass) == JNI_FALSE);
    CHECK((*env)->IsAssignableFrom(env, integerClass, integerClass) == JNI_TRUE);
    CHECK((*env)->IsAssignableFrom(env, stringClass, charSequenceClass) == JNI_TRUE);
    CHECK((*env)->IsAssignableFrom(env, charSequenceClass, objectClass) == JNI_TRUE);
    CHECK((*env)->IsAssignableFrom(env, intArrayClass, objectClass) == JNI_TRUE);
    CHECK((*env)->IsAssignableFrom(env, intArrayClass, objectArrayClass) == JNI_FALSE);
    CHECK((*env)->IsAssignableFrom(env, stringArrayClass, objectArrayClass) == JNI_TRUE);
    CHECK((*env)->IsAssignableFrom(env, objectArrayClass, stringArrayClass) == JNI_FALSE);
    CHECK((*env)->IsAssignableFrom(env, (*env)->FindClass(env, SUB_TARGET_CLASS), (*env)->FindClass(env, TARGET_INTERFACE)) == JNI_TRUE);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getObjectClass(JNIEnv* env, jobject self)
{
    jobject subTarget = newTarget(env, SUB_TARGET_CLASS);
    CHECK(subTarget != NULL);
    CHECK((*env)->IsSameObject(env, (*env)->GetObjectClass(env, subTarget), (*env)->FindClass(env, SUB_TARGET_CLASS)));
    CHECK((*env)->IsSameObject(env, (*env)->GetObjectClass(env, (*env)->NewStringUTF(env, "s")), (*env)->FindClass(env, "java/lang/String")));
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_isInstanceOf(JNIEnv* env, jobject self)
{
    jstring s = (*env)->NewStringUTF(env, "s");
    CHECK((*env)->IsInstanceOf(env, s, (*env)->FindClass(env, "java/lang/Object")) == JNI_TRUE);
    CHECK((*env)->IsInstanceOf(env, s, (*env)->FindClass(env, "java/lang/String")) == JNI_TRUE);
    CHECK((*env)->IsInstanceOf(env, s, (*env)->FindClass(env, "java/lang/CharSequence")) == JNI_TRUE);
    CHECK((*env)->IsInstanceOf(env, s, (*env)->FindClass(env, "java/lang/Integer")) == JNI_FALSE);

    jobject subTarget = newTarget(env, SUB_TARGET_CLASS);
    CHECK((*env)->IsInstanceOf(env, subTarget, (*env)->FindClass(env, TARGET_CLASS)) == JNI_TRUE);
    CHECK((*env)->IsInstanceOf(env, subTarget, (*env)->FindClass(env, TARGET_INTERFACE)) == JNI_TRUE);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_isInstanceOfNull(JNIEnv* env, jobject self)
{
    // a NULL object can be cast to any class
    CHECK((*env)->IsInstanceOf(env, NULL, (*env)->FindClass(env, "java/lang/String")) == JNI_TRUE);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_isSameObject(JNIEnv* env, jobject self)
{
    jstring a = (*env)->NewStringUTF(env, "a");
    jstring b = (*env)->NewStringUTF(env, "a");
    jobject global = (*env)->NewGlobalRef(env, a);

    CHECK((*env)->IsSameObject(env, a, a) == JNI_TRUE);
    CHECK((*env)->IsSameObject(env, a, (*env)->NewLocalRef(env, a)) == JNI_TRUE);
    CHECK((*env)->IsSameObject(env, a, global) == JNI_TRUE);
    CHECK((*env)->IsSameObject(env, a, b) == JNI_FALSE);
    CHECK((*env)->IsSameObject(env, a, NULL) == JNI_FALSE);
    CHECK((*env)->IsSameObject(env, NULL, NULL) == JNI_TRUE);

    (*env)->DeleteGlobalRef(env, global);
    NO_EXCEPTION();
}

// reflection

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_fromReflectedMethodAndBack(JNIEnv* env, jobject self, jobject method)
{
    jclass targetClass = (*env)->FindClass(env, TARGET_CLASS);
    jmethodID id = (*env)->FromReflectedMethod(env, method);
    CHECK(id != NULL);

    // the ID can be used to call the method
    jobject target = newTarget(env, TARGET_CLASS);
    jvalue args[9];
    fillArgs(args, NULL);
    CHECK((*env)->CallIntMethodA(env, target, id, args) == 123456789);

    jobject back = (*env)->ToReflectedMethod(env, targetClass, id, JNI_FALSE);
    CHECK(back != NULL);
    CHECK(callEquals(env, back, method));
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_fromReflectedConstructorAndBack(JNIEnv* env, jobject self, jobject ctor)
{
    jclass targetClass = (*env)->FindClass(env, TARGET_CLASS);
    jmethodID id = (*env)->FromReflectedMethod(env, ctor);
    CHECK(id != NULL);

    jobject target = (*env)->NewObject(env, targetClass, id);
    CHECK(target != NULL);
    CHECK((*env)->IsInstanceOf(env, target, targetClass));

    jobject back = (*env)->ToReflectedMethod(env, targetClass, id, JNI_FALSE);
    CHECK(back != NULL);
    CHECK(callEquals(env, back, ctor));
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_fromReflectedFieldAndBack(JNIEnv* env, jobject self, jobject field)
{
    jclass targetClass = (*env)->FindClass(env, TARGET_CLASS);
    jfieldID id = (*env)->FromReflectedField(env, field);
    CHECK(id != NULL);

    // the ID can be used to access the field
    jobject target = newTarget(env, TARGET_CLASS);
    (*env)->SetIntField(env, target, id, 7);
    CHECK((*env)->GetIntField(env, target, id) == 7);

    jobject back = (*env)->ToReflectedField(env, targetClass, id, JNI_FALSE);
    CHECK(back != NULL);
    CHECK(callEquals(env, back, field));
}

// exceptions

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_throwAndCheck(JNIEnv* env, jobject self)
{
    jobject t = newIllegalStateException(env, "thrown");
    CHECK(t != NULL);

    CHECK((*env)->Throw(env, (jthrowable)t) == JNI_OK);
    CHECK((*env)->ExceptionCheck(env) == JNI_TRUE);

    jthrowable occurred = (*env)->ExceptionOccurred(env);
    CHECK(occurred != NULL);
    CHECK((*env)->IsSameObject(env, occurred, t));

    (*env)->ExceptionClear(env);
    CHECK((*env)->ExceptionCheck(env) == JNI_FALSE);
    CHECK((*env)->ExceptionOccurred(env) == NULL);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_throwNewWithNullMessage(JNIEnv* env, jobject self)
{
    jclass c = (*env)->FindClass(env, "java/lang/IllegalStateException");
    CHECK((*env)->ThrowNew(env, c, NULL) == JNI_OK);

    jthrowable t = (*env)->ExceptionOccurred(env);
    CHECK(t != NULL);
    (*env)->ExceptionClear(env);
    CHECK((*env)->IsInstanceOf(env, t, c));

    jmethodID getMessage = (*env)->GetMethodID(env, c, "getMessage", "()Ljava/lang/String;");
    CHECK((*env)->CallObjectMethod(env, t, getMessage) == NULL);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_exceptionOccurredAndClear(JNIEnv* env, jobject self)
{
    CHECK((*env)->ExceptionOccurred(env) == NULL);
    CHECK((*env)->ExceptionCheck(env) == JNI_FALSE);

    // clearing when nothing is pending does nothing
    (*env)->ExceptionClear(env);
    CHECK((*env)->ExceptionCheck(env) == JNI_FALSE);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_exceptionDescribeClears(JNIEnv* env, jobject self)
{
    (*env)->ThrowNew(env, (*env)->FindClass(env, "java/lang/IllegalStateException"), "described");
    CHECK((*env)->ExceptionCheck(env) == JNI_TRUE);
    (*env)->ExceptionDescribe(env);
    CHECK((*env)->ExceptionCheck(env) == JNI_FALSE);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_exceptionFromCalledMethodIsPending(JNIEnv* env, jobject self)
{
    jobject target = newTarget(env, TARGET_CLASS);
    jmethodID m = (*env)->GetMethodID(env, (*env)->FindClass(env, TARGET_CLASS), "throwingMethod", "()V");
    (*env)->CallVoidMethod(env, target, m);
    EXPECT_EXCEPTION("java/lang/IllegalStateException");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_exceptionFromCalledStaticMethodIsPending(JNIEnv* env, jobject self)
{
    jclass c = (*env)->FindClass(env, TARGET_CLASS);
    jmethodID m = (*env)->GetStaticMethodID(env, c, "staticThrowingMethod", "()V");
    (*env)->CallStaticVoidMethod(env, c, m);
    EXPECT_EXCEPTION("java/lang/IllegalStateException");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_leavesExceptionPending(JNIEnv* env, jobject self)
{
    (*env)->ThrowNew(env, (*env)->FindClass(env, "java/lang/IllegalStateException"), "pending");
}

// references

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_newLocalRefAndDeleteLocalRef(JNIEnv* env, jobject self)
{
    jstring o = (*env)->NewStringUTF(env, "x");
    jobject r = (*env)->NewLocalRef(env, o);
    CHECK(r != NULL);
    CHECK((*env)->IsSameObject(env, r, o));

    (*env)->DeleteLocalRef(env, r);
    CHECK((*env)->GetStringLength(env, o) == 1);

    CHECK((*env)->NewLocalRef(env, NULL) == NULL);
    (*env)->DeleteLocalRef(env, NULL);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_manyLocalRefs(JNIEnv* env, jobject self, jint count)
{
    jobject* refs = (jobject*)malloc(sizeof(jobject) * count);
    for (jint i = 0; i < count; i++)
        refs[i] = (*env)->NewIntArray(env, i % 7);

    for (jint i = 0; i < count; i++)
    {
        if ((*env)->GetArrayLength(env, refs[i]) != i % 7)
        {
            free(refs);
            FAIL("local ref resolves to the wrong object");
            return;
        }
    }

    free(refs);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_deletedLocalRefsAreReused(JNIEnv* env, jobject self, jint count)
{
    jobject first = (*env)->NewIntArray(env, 1);
    jobject second = (*env)->NewIntArray(env, 2);

    for (jint i = 0; i < count; i++)
    {
        jobject r = (*env)->NewIntArray(env, 3);
        (*env)->DeleteLocalRef(env, r);
    }

    CHECK((*env)->GetArrayLength(env, first) == 1);
    CHECK((*env)->GetArrayLength(env, second) == 2);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_ensureLocalCapacity(JNIEnv* env, jobject self)
{
    CHECK((*env)->EnsureLocalCapacity(env, 100) == JNI_OK);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_pushAndPopLocalFrame(JNIEnv* env, jobject self)
{
    jobject outer = (*env)->NewIntArray(env, 1);

    CHECK((*env)->PushLocalFrame(env, 16) == JNI_OK);
    jobject inner = (*env)->NewIntArray(env, 2);
    jobject result = (*env)->PopLocalFrame(env, inner);
    NO_EXCEPTION();

    CHECK(result != NULL);
    CHECK((*env)->GetObjectRefType(env, result) == JNILocalRefType);
    CHECK((*env)->GetArrayLength(env, result) == 2);
    CHECK((*env)->GetArrayLength(env, outer) == 1);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_popLocalFrameWithNullResult(JNIEnv* env, jobject self)
{
    CHECK((*env)->PushLocalFrame(env, 4) == JNI_OK);
    (*env)->NewIntArray(env, 1);
    CHECK((*env)->PopLocalFrame(env, NULL) == NULL);
    NO_EXCEPTION();
}

static jobject nestLocalFrames(JNIEnv* env, jint level, jint depth)
{
    if (level == depth)
        return (*env)->NewIntArray(env, level);

    if ((*env)->PushLocalFrame(env, 4) != JNI_OK)
        return NULL;

    jobject mine = (*env)->NewIntArray(env, level);
    jobject deeper = nestLocalFrames(env, level + 1, depth);
    if (deeper == NULL || (*env)->GetArrayLength(env, deeper) != depth || (*env)->GetArrayLength(env, mine) != level)
    {
        (*env)->PopLocalFrame(env, NULL);
        return NULL;
    }

    return (*env)->PopLocalFrame(env, deeper);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_nestedLocalFrames(JNIEnv* env, jobject self, jint depth)
{
    jobject before = (*env)->NewIntArray(env, 1000);
    jobject result = nestLocalFrames(env, 0, depth);
    NO_EXCEPTION();
    CHECK(result != NULL);
    CHECK((*env)->GetArrayLength(env, result) == depth);
    CHECK((*env)->GetArrayLength(env, before) == 1000);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_manyLocalFrames(JNIEnv* env, jobject self, jint count)
{
    jobject first = (*env)->NewIntArray(env, 1);

    for (jint i = 0; i < count; i++)
    {
        CHECK((*env)->PushLocalFrame(env, 4) == JNI_OK);
        (*env)->NewIntArray(env, 2);
        (*env)->NewIntArray(env, 3);
        jobject kept = (*env)->PopLocalFrame(env, (*env)->NewIntArray(env, 4));
        CHECK(kept != NULL);
        CHECK((*env)->GetArrayLength(env, kept) == 4);
        CHECK((*env)->GetArrayLength(env, first) == 1);
        (*env)->DeleteLocalRef(env, kept);
    }

    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_localRefsSurviveNestedNativeCalls(JNIEnv* env, jobject self, jint depth)
{
    jobject mine = (*env)->NewIntArray(env, depth);

    if (depth > 0)
    {
        jmethodID nest = (*env)->GetMethodID(env, (*env)->GetObjectClass(env, self), "nest", "(I)V");
        CHECK(nest != NULL);
        (*env)->CallVoidMethod(env, self, nest, depth - 1);
        NO_EXCEPTION();
    }

    CHECK((*env)->GetArrayLength(env, mine) == depth);
}

JNIEXPORT jlong JNICALL Java_ikvm_tests_jni_JniEnvTests_newGlobalRef(JNIEnv* env, jobject self, jobject o)
{
    jobject r = (*env)->NewGlobalRef(env, o);
    CHECK_R(r != NULL, 0);
    CHECK_R((*env)->GetObjectRefType(env, r) == JNIGlobalRefType, 0);
    return (jlong)(intptr_t)r;
}

JNIEXPORT jobject JNICALL Java_ikvm_tests_jni_JniEnvTests_resolveGlobalRef(JNIEnv* env, jobject self, jlong ref)
{
    return (*env)->NewLocalRef(env, (jobject)(intptr_t)ref);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_deleteGlobalRef(JNIEnv* env, jobject self, jlong ref)
{
    (*env)->DeleteGlobalRef(env, (jobject)(intptr_t)ref);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_newGlobalRefOfNull(JNIEnv* env, jobject self)
{
    CHECK((*env)->NewGlobalRef(env, NULL) == NULL);
    (*env)->DeleteGlobalRef(env, NULL);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_manyGlobalRefs(JNIEnv* env, jobject self, jint count)
{
    jobject* refs = (jobject*)malloc(sizeof(jobject) * count);

    for (int round = 0; round < 2; round++)
    {
        for (jint i = 0; i < count; i++)
        {
            jobject local = (*env)->NewIntArray(env, i % 7);
            refs[i] = (*env)->NewGlobalRef(env, local);
            (*env)->DeleteLocalRef(env, local);
        }

        // free every other one and take them again, so the slots of deleted refs get reused
        for (jint i = 0; i < count; i += 2)
            (*env)->DeleteGlobalRef(env, refs[i]);

        for (jint i = 0; i < count; i += 2)
        {
            jobject local = (*env)->NewIntArray(env, i % 7);
            refs[i] = (*env)->NewGlobalRef(env, local);
            (*env)->DeleteLocalRef(env, local);
        }

        jboolean ok = JNI_TRUE;
        for (jint i = 0; i < count; i++)
            if ((*env)->GetArrayLength(env, refs[i]) != i % 7)
                ok = JNI_FALSE;

        for (jint i = 0; i < count; i++)
            (*env)->DeleteGlobalRef(env, refs[i]);

        if (ok == JNI_FALSE)
        {
            free(refs);
            FAIL("global ref resolves to the wrong object");
            return;
        }
    }

    free(refs);
    NO_EXCEPTION();
}

JNIEXPORT jlong JNICALL Java_ikvm_tests_jni_JniEnvTests_newWeakGlobalRef(JNIEnv* env, jobject self, jobject o)
{
    jweak r = (*env)->NewWeakGlobalRef(env, o);
    CHECK_R(r != NULL, 0);
    CHECK_R((*env)->GetObjectRefType(env, r) == JNIWeakGlobalRefType, 0);
    return (jlong)(intptr_t)r;
}

JNIEXPORT jobject JNICALL Java_ikvm_tests_jni_JniEnvTests_resolveWeakGlobalRef(JNIEnv* env, jobject self, jlong ref)
{
    return (*env)->NewLocalRef(env, (jobject)(intptr_t)ref);
}

JNIEXPORT jboolean JNICALL Java_ikvm_tests_jni_JniEnvTests_isWeakGlobalRefCleared(JNIEnv* env, jobject self, jlong ref)
{
    return (*env)->IsSameObject(env, (jobject)(intptr_t)ref, NULL);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_deleteWeakGlobalRef(JNIEnv* env, jobject self, jlong ref)
{
    (*env)->DeleteWeakGlobalRef(env, (jweak)(intptr_t)ref);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_newWeakGlobalRefOfNull(JNIEnv* env, jobject self)
{
    CHECK((*env)->NewWeakGlobalRef(env, NULL) == NULL);
    (*env)->DeleteWeakGlobalRef(env, NULL);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_manyWeakGlobalRefs(JNIEnv* env, jobject self, jint count)
{
    jobject* locals = (jobject*)malloc(sizeof(jobject) * count);
    jweak* refs = (jweak*)malloc(sizeof(jweak) * count);

    // the locals keep the objects reachable
    for (jint i = 0; i < count; i++)
    {
        locals[i] = (*env)->NewIntArray(env, i % 7);
        refs[i] = (*env)->NewWeakGlobalRef(env, locals[i]);
    }

    jboolean ok = JNI_TRUE;
    for (jint i = 0; i < count; i++)
        if ((*env)->IsSameObject(env, refs[i], locals[i]) == JNI_FALSE)
            ok = JNI_FALSE;

    for (jint i = 0; i < count; i++)
        (*env)->DeleteWeakGlobalRef(env, refs[i]);

    free(locals);
    free(refs);

    CHECK(ok);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getObjectRefType(JNIEnv* env, jobject self)
{
    jobject local = (*env)->NewIntArray(env, 1);
    jobject global = (*env)->NewGlobalRef(env, local);
    jweak weak = (*env)->NewWeakGlobalRef(env, local);

    CHECK((*env)->GetObjectRefType(env, local) == JNILocalRefType);
    CHECK((*env)->GetObjectRefType(env, global) == JNIGlobalRefType);
    CHECK((*env)->GetObjectRefType(env, weak) == JNIWeakGlobalRefType);

    (*env)->DeleteGlobalRef(env, global);
    (*env)->DeleteWeakGlobalRef(env, weak);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getObjectRefTypeOfNull(JNIEnv* env, jobject self)
{
    CHECK((*env)->GetObjectRefType(env, NULL) == JNIInvalidRefType);
}

// objects

JNIEXPORT jobject JNICALL Java_ikvm_tests_jni_JniEnvTests_allocObject(JNIEnv* env, jobject self)
{
    jobject o = (*env)->AllocObject(env, (*env)->FindClass(env, TARGET_CLASS));
    CHECK_R(o != NULL, NULL);
    return o;
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_allocObjectOfAbstractClass(JNIEnv* env, jobject self)
{
    CHECK((*env)->AllocObject(env, (*env)->FindClass(env, ABSTRACT_TARGET_CLASS)) == NULL);
    EXPECT_EXCEPTION("java/lang/InstantiationException");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_allocObjectOfInterface(JNIEnv* env, jobject self)
{
    CHECK((*env)->AllocObject(env, (*env)->FindClass(env, TARGET_INTERFACE)) == NULL);
    EXPECT_EXCEPTION("java/lang/InstantiationException");
}

static jobject newObjectV(JNIEnv* env, jclass c, jmethodID m, ...)
{
    va_list a;
    va_start(a, m);
    jobject o = (*env)->NewObjectV(env, c, m, a);
    va_end(a);
    return o;
}

JNIEXPORT jobject JNICALL Java_ikvm_tests_jni_JniEnvTests_newObjectWithArgs(JNIEnv* env, jobject self, jint mode)
{
    jclass c = (*env)->FindClass(env, TARGET_CLASS);
    jmethodID ctor = (*env)->GetMethodID(env, c, "<init>", ARGS_SIG "V");
    CHECK_R(ctor != NULL, NULL);

    jstring l = (*env)->NewStringUTF(env, "arg");
    jvalue v[9];
    fillArgs(v, l);

    switch (mode)
    {
        case 0:
            return (*env)->NewObject(env, c, ctor, VARARGS(l));
        case 1:
            return newObjectV(env, c, ctor, VARARGS(l));
        default:
            return (*env)->NewObjectA(env, c, ctor, v);
    }
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_newObjectOfAbstractClass(JNIEnv* env, jobject self)
{
    jclass c = (*env)->FindClass(env, ABSTRACT_TARGET_CLASS);
    jmethodID ctor = (*env)->GetMethodID(env, c, "<init>", "()V");
    CHECK(ctor != NULL);
    CHECK((*env)->NewObject(env, c, ctor) == NULL);
    EXPECT_EXCEPTION("java/lang/InstantiationException");
}

JNIEXPORT jobject JNICALL Java_ikvm_tests_jni_JniEnvTests_newObjectThatThrows(JNIEnv* env, jobject self)
{
    jclass c = (*env)->FindClass(env, "java/lang/Integer");
    jmethodID ctor = (*env)->GetMethodID(env, c, "<init>", "(Ljava/lang/String;)V");
    jobject o = (*env)->NewObject(env, c, ctor, (*env)->NewStringUTF(env, "not a number"));
    CHECK_R(o == NULL, NULL);
    EXPECT_EXCEPTION_R("java/lang/NumberFormatException", NULL);
    return o;
}

// method lookup

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getMethodID(JNIEnv* env, jobject self)
{
    jclass c = (*env)->FindClass(env, TARGET_CLASS);
    CHECK((*env)->GetMethodID(env, c, "intMethod", ARGS_SIG "I") != NULL);
    CHECK((*env)->GetMethodID(env, c, "<init>", "()V") != NULL);
    CHECK((*env)->GetMethodID(env, c, "hashCode", "()I") != NULL);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getMethodIDOfInheritedMethod(JNIEnv* env, jobject self)
{
    jmethodID m = (*env)->GetMethodID(env, (*env)->FindClass(env, SUB_TARGET_CLASS), "inheritedMethod", "()I");
    CHECK(m != NULL);
    CHECK((*env)->CallIntMethod(env, newTarget(env, SUB_TARGET_CLASS), m) == 5);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getMethodIDOfInterfaceMethod(JNIEnv* env, jobject self)
{
    jmethodID m = (*env)->GetMethodID(env, (*env)->FindClass(env, TARGET_INTERFACE), "interfaceMethod", "()I");
    CHECK(m != NULL);
    CHECK((*env)->CallIntMethod(env, newTarget(env, TARGET_CLASS), m) == 1);
    CHECK((*env)->CallIntMethod(env, newTarget(env, SUB_TARGET_CLASS), m) == 2);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getMethodIDNotFound(JNIEnv* env, jobject self)
{
    CHECK((*env)->GetMethodID(env, (*env)->FindClass(env, TARGET_CLASS), "missing", "()V") == NULL);
    EXPECT_EXCEPTION("java/lang/NoSuchMethodError");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getMethodIDOfStaticMethod(JNIEnv* env, jobject self)
{
    CHECK((*env)->GetMethodID(env, (*env)->FindClass(env, TARGET_CLASS), "staticThrowingMethod", "()V") == NULL);
    EXPECT_EXCEPTION("java/lang/NoSuchMethodError");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getStaticMethodID(JNIEnv* env, jobject self)
{
    CHECK((*env)->GetStaticMethodID(env, (*env)->FindClass(env, TARGET_CLASS), "staticIntMethod", ARGS_SIG "I") != NULL);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getStaticMethodIDNotFound(JNIEnv* env, jobject self)
{
    CHECK((*env)->GetStaticMethodID(env, (*env)->FindClass(env, TARGET_CLASS), "missing", "()V") == NULL);
    EXPECT_EXCEPTION("java/lang/NoSuchMethodError");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getStaticMethodIDOfInstanceMethod(JNIEnv* env, jobject self)
{
    CHECK((*env)->GetStaticMethodID(env, (*env)->FindClass(env, TARGET_CLASS), "throwingMethod", "()V") == NULL);
    EXPECT_EXCEPTION("java/lang/NoSuchMethodError");
}

// method calls, where mode 0 is the variadic function, 1 the va_list function and 2 the jvalue array function

#define DEFINE_CALLS(Type, type, jtype, rsig) \
static jtype call##Type##MethodV(JNIEnv* env, jobject o, jmethodID m, ...) \
{ \
    va_list a; \
    va_start(a, m); \
    jtype r = (*env)->Call##Type##MethodV(env, o, m, a); \
    va_end(a); \
    return r; \
} \
static jtype callNonvirtual##Type##MethodV(JNIEnv* env, jobject o, jclass c, jmethodID m, ...) \
{ \
    va_list a; \
    va_start(a, m); \
    jtype r = (*env)->CallNonvirtual##Type##MethodV(env, o, c, m, a); \
    va_end(a); \
    return r; \
} \
static jtype callStatic##Type##MethodV(JNIEnv* env, jclass c, jmethodID m, ...) \
{ \
    va_list a; \
    va_start(a, m); \
    jtype r = (*env)->CallStatic##Type##MethodV(env, c, m, a); \
    va_end(a); \
    return r; \
} \
JNIEXPORT jtype JNICALL Java_ikvm_tests_jni_JniEnvTests_call##Type##Method(JNIEnv* env, jobject self, jobject target, jint mode) \
{ \
    jclass c = (*env)->FindClass(env, TARGET_CLASS); \
    jmethodID m = (*env)->GetMethodID(env, c, #type "Method", ARGS_SIG rsig); \
    CHECK_R(m != NULL, 0); \
    jstring l = (*env)->NewStringUTF(env, "arg"); \
    jvalue v[9]; \
    fillArgs(v, l); \
    switch (mode) \
    { \
        case 0: return (*env)->Call##Type##Method(env, target, m, VARARGS(l)); \
        case 1: return call##Type##MethodV(env, target, m, VARARGS(l)); \
        default: return (*env)->Call##Type##MethodA(env, target, m, v); \
    } \
} \
JNIEXPORT jtype JNICALL Java_ikvm_tests_jni_JniEnvTests_callNonvirtual##Type##Method(JNIEnv* env, jobject self, jobject target, jint mode) \
{ \
    jclass c = (*env)->FindClass(env, TARGET_CLASS); \
    jmethodID m = (*env)->GetMethodID(env, c, #type "Method", ARGS_SIG rsig); \
    CHECK_R(m != NULL, 0); \
    jstring l = (*env)->NewStringUTF(env, "arg"); \
    jvalue v[9]; \
    fillArgs(v, l); \
    switch (mode) \
    { \
        case 0: return (*env)->CallNonvirtual##Type##Method(env, target, c, m, VARARGS(l)); \
        case 1: return callNonvirtual##Type##MethodV(env, target, c, m, VARARGS(l)); \
        default: return (*env)->CallNonvirtual##Type##MethodA(env, target, c, m, v); \
    } \
} \
JNIEXPORT jtype JNICALL Java_ikvm_tests_jni_JniEnvTests_callStatic##Type##Method(JNIEnv* env, jobject self, jint mode) \
{ \
    jclass c = (*env)->FindClass(env, TARGET_CLASS); \
    jmethodID m = (*env)->GetStaticMethodID(env, c, "static" #Type "Method", ARGS_SIG rsig); \
    CHECK_R(m != NULL, 0); \
    jstring l = (*env)->NewStringUTF(env, "arg"); \
    jvalue v[9]; \
    fillArgs(v, l); \
    switch (mode) \
    { \
        case 0: return (*env)->CallStatic##Type##Method(env, c, m, VARARGS(l)); \
        case 1: return callStatic##Type##MethodV(env, c, m, VARARGS(l)); \
        default: return (*env)->CallStatic##Type##MethodA(env, c, m, v); \
    } \
}

DEFINE_CALLS(Boolean, boolean, jboolean, "Z")
DEFINE_CALLS(Byte, byte, jbyte, "B")
DEFINE_CALLS(Char, char, jchar, "C")
DEFINE_CALLS(Short, short, jshort, "S")
DEFINE_CALLS(Int, int, jint, "I")
DEFINE_CALLS(Long, long, jlong, "J")
DEFINE_CALLS(Float, float, jfloat, "F")
DEFINE_CALLS(Double, double, jdouble, "D")
DEFINE_CALLS(Object, object, jobject, "Ljava/lang/Object;")

static void callVoidMethodV(JNIEnv* env, jobject o, jmethodID m, ...)
{
    va_list a;
    va_start(a, m);
    (*env)->CallVoidMethodV(env, o, m, a);
    va_end(a);
}

static void callNonvirtualVoidMethodV(JNIEnv* env, jobject o, jclass c, jmethodID m, ...)
{
    va_list a;
    va_start(a, m);
    (*env)->CallNonvirtualVoidMethodV(env, o, c, m, a);
    va_end(a);
}

static void callStaticVoidMethodV(JNIEnv* env, jclass c, jmethodID m, ...)
{
    va_list a;
    va_start(a, m);
    (*env)->CallStaticVoidMethodV(env, c, m, a);
    va_end(a);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_callVoidMethod(JNIEnv* env, jobject self, jobject target, jint mode)
{
    jclass c = (*env)->FindClass(env, TARGET_CLASS);
    jmethodID m = (*env)->GetMethodID(env, c, "voidMethod", ARGS_SIG "V");
    CHECK(m != NULL);
    jstring l = (*env)->NewStringUTF(env, "arg");
    jvalue v[9];
    fillArgs(v, l);

    switch (mode)
    {
        case 0: (*env)->CallVoidMethod(env, target, m, VARARGS(l)); break;
        case 1: callVoidMethodV(env, target, m, VARARGS(l)); break;
        default: (*env)->CallVoidMethodA(env, target, m, v); break;
    }
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_callNonvirtualVoidMethod(JNIEnv* env, jobject self, jobject target, jint mode)
{
    jclass c = (*env)->FindClass(env, TARGET_CLASS);
    jmethodID m = (*env)->GetMethodID(env, c, "voidMethod", ARGS_SIG "V");
    CHECK(m != NULL);
    jstring l = (*env)->NewStringUTF(env, "arg");
    jvalue v[9];
    fillArgs(v, l);

    switch (mode)
    {
        case 0: (*env)->CallNonvirtualVoidMethod(env, target, c, m, VARARGS(l)); break;
        case 1: callNonvirtualVoidMethodV(env, target, c, m, VARARGS(l)); break;
        default: (*env)->CallNonvirtualVoidMethodA(env, target, c, m, v); break;
    }
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_callStaticVoidMethod(JNIEnv* env, jobject self, jint mode)
{
    jclass c = (*env)->FindClass(env, TARGET_CLASS);
    jmethodID m = (*env)->GetStaticMethodID(env, c, "staticVoidMethod", ARGS_SIG "V");
    CHECK(m != NULL);
    jstring l = (*env)->NewStringUTF(env, "arg");
    jvalue v[9];
    fillArgs(v, l);

    switch (mode)
    {
        case 0: (*env)->CallStaticVoidMethod(env, c, m, VARARGS(l)); break;
        case 1: callStaticVoidMethodV(env, c, m, VARARGS(l)); break;
        default: (*env)->CallStaticVoidMethodA(env, c, m, v); break;
    }
}

JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniEnvTests_callInterfaceMethod(JNIEnv* env, jobject self, jobject target)
{
    jmethodID m = (*env)->GetMethodID(env, (*env)->FindClass(env, TARGET_INTERFACE), "interfaceMethod", "()I");
    CHECK_R(m != NULL, 0);
    return (*env)->CallIntMethod(env, target, m);
}

JNIEXPORT jclass JNICALL Java_ikvm_tests_jni_JniEnvTests_callStaticMethodWithCallerID(JNIEnv* env, jobject self, jstring className)
{
    // Class.forName uses the class loader of its caller, which for a call from native code is that of the native method
    jclass c = (*env)->FindClass(env, "java/lang/Class");
    jmethodID forName = (*env)->GetStaticMethodID(env, c, "forName", "(Ljava/lang/String;)Ljava/lang/Class;");
    CHECK_R(forName != NULL, NULL);
    return (jclass)(*env)->CallStaticObjectMethod(env, c, forName, className);
}

// fields

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getFieldID(JNIEnv* env, jobject self)
{
    jclass c = (*env)->FindClass(env, TARGET_CLASS);
    CHECK((*env)->GetFieldID(env, c, "iField", "I") != NULL);
    CHECK((*env)->GetFieldID(env, c, "lField", "Ljava/lang/Object;") != NULL);
    CHECK((*env)->GetStaticFieldID(env, c, "iStatic", "I") != NULL);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getFieldIDOfInheritedField(JNIEnv* env, jobject self)
{
    jfieldID f = (*env)->GetFieldID(env, (*env)->FindClass(env, SUB_TARGET_CLASS), "iField", "I");
    CHECK(f != NULL);

    jobject subTarget = newTarget(env, SUB_TARGET_CLASS);
    (*env)->SetIntField(env, subTarget, f, 11);
    CHECK((*env)->GetIntField(env, subTarget, f) == 11);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getFieldIDNotFound(JNIEnv* env, jobject self)
{
    CHECK((*env)->GetFieldID(env, (*env)->FindClass(env, TARGET_CLASS), "missing", "I") == NULL);
    EXPECT_EXCEPTION("java/lang/NoSuchFieldError");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getFieldIDWithWrongSignature(JNIEnv* env, jobject self)
{
    CHECK((*env)->GetFieldID(env, (*env)->FindClass(env, TARGET_CLASS), "iField", "J") == NULL);
    EXPECT_EXCEPTION("java/lang/NoSuchFieldError");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getFieldIDOfStaticField(JNIEnv* env, jobject self)
{
    CHECK((*env)->GetFieldID(env, (*env)->FindClass(env, TARGET_CLASS), "iStatic", "I") == NULL);
    EXPECT_EXCEPTION("java/lang/NoSuchFieldError");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getStaticFieldIDOfInstanceField(JNIEnv* env, jobject self)
{
    CHECK((*env)->GetStaticFieldID(env, (*env)->FindClass(env, TARGET_CLASS), "iField", "I") == NULL);
    EXPECT_EXCEPTION("java/lang/NoSuchFieldError");
}

#define DEFINE_FIELDS(Type, jtype, prefix, sig) \
JNIEXPORT jtype JNICALL Java_ikvm_tests_jni_JniEnvTests_get##Type##Field(JNIEnv* env, jobject self, jobject o) \
{ \
    jfieldID f = (*env)->GetFieldID(env, (*env)->FindClass(env, TARGET_CLASS), prefix "Field", sig); \
    CHECK_R(f != NULL, 0); \
    return (*env)->Get##Type##Field(env, o, f); \
} \
JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_set##Type##Field(JNIEnv* env, jobject self, jobject o, jtype value) \
{ \
    jfieldID f = (*env)->GetFieldID(env, (*env)->FindClass(env, TARGET_CLASS), prefix "Field", sig); \
    CHECK(f != NULL); \
    (*env)->Set##Type##Field(env, o, f, value); \
    NO_EXCEPTION(); \
} \
JNIEXPORT jtype JNICALL Java_ikvm_tests_jni_JniEnvTests_getStatic##Type##Field(JNIEnv* env, jobject self) \
{ \
    jclass c = (*env)->FindClass(env, TARGET_CLASS); \
    jfieldID f = (*env)->GetStaticFieldID(env, c, prefix "Static", sig); \
    CHECK_R(f != NULL, 0); \
    return (*env)->GetStatic##Type##Field(env, c, f); \
} \
JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_setStatic##Type##Field(JNIEnv* env, jobject self, jtype value) \
{ \
    jclass c = (*env)->FindClass(env, TARGET_CLASS); \
    jfieldID f = (*env)->GetStaticFieldID(env, c, prefix "Static", sig); \
    CHECK(f != NULL); \
    (*env)->SetStatic##Type##Field(env, c, f, value); \
    NO_EXCEPTION(); \
}

DEFINE_FIELDS(Boolean, jboolean, "z", "Z")
DEFINE_FIELDS(Byte, jbyte, "b", "B")
DEFINE_FIELDS(Char, jchar, "c", "C")
DEFINE_FIELDS(Short, jshort, "s", "S")
DEFINE_FIELDS(Int, jint, "i", "I")
DEFINE_FIELDS(Long, jlong, "j", "J")
DEFINE_FIELDS(Float, jfloat, "f", "F")
DEFINE_FIELDS(Double, jdouble, "d", "D")
DEFINE_FIELDS(Object, jobject, "l", "Ljava/lang/Object;")

// strings

JNIEXPORT jstring JNICALL Java_ikvm_tests_jni_JniEnvTests_newString(JNIEnv* env, jobject self, jcharArray chars)
{
    jsize length = (*env)->GetArrayLength(env, chars);
    jchar* buf = (jchar*)malloc(sizeof(jchar) * (length + 1));
    (*env)->GetCharArrayRegion(env, chars, 0, length, buf);
    jstring s = (*env)->NewString(env, buf, length);
    free(buf);
    return s;
}

JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniEnvTests_getStringLength(JNIEnv* env, jobject self, jstring s)
{
    return (*env)->GetStringLength(env, s);
}

JNIEXPORT jcharArray JNICALL Java_ikvm_tests_jni_JniEnvTests_getStringChars(JNIEnv* env, jobject self, jstring s)
{
    jsize length = (*env)->GetStringLength(env, s);
    jboolean isCopy = 2;
    const jchar* chars = (*env)->GetStringChars(env, s, &isCopy);
    CHECK_R(chars != NULL, NULL);
    CHECK_R(isCopy == JNI_TRUE || isCopy == JNI_FALSE, NULL);

    jchar* buf = (jchar*)malloc(sizeof(jchar) * (length + 1));
    memcpy(buf, chars, sizeof(jchar) * length);
    (*env)->ReleaseStringChars(env, s, chars);

    jcharArray result = (*env)->NewCharArray(env, length);
    (*env)->SetCharArrayRegion(env, result, 0, length, buf);
    free(buf);
    return result;
}

JNIEXPORT jstring JNICALL Java_ikvm_tests_jni_JniEnvTests_newStringUTF(JNIEnv* env, jobject self, jbyteArray bytes)
{
    jsize length = (*env)->GetArrayLength(env, bytes);
    char* buf = (char*)calloc(length + 1, 1);
    (*env)->GetByteArrayRegion(env, bytes, 0, length, (jbyte*)buf);
    jstring s = (*env)->NewStringUTF(env, buf);
    free(buf);
    return s;
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_newStringUTFOfNull(JNIEnv* env, jobject self)
{
    CHECK((*env)->NewStringUTF(env, NULL) == NULL);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_newStringUTFWithInvalidBytes(JNIEnv* env, jobject self)
{
    // the result for bytes that are not modified UTF-8 is up to the implementation, but it must be either a local
    // reference or NULL with an exception pending
    jstring s = (*env)->NewStringUTF(env, "\xFF\xFE\xC0");
    if (s == NULL)
    {
        CHECK((*env)->ExceptionCheck(env) == JNI_TRUE);
        (*env)->ExceptionClear(env);
        return;
    }

    CHECK((*env)->GetObjectRefType(env, s) == JNILocalRefType);
}

JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniEnvTests_getStringUTFLength(JNIEnv* env, jobject self, jstring s)
{
    return (*env)->GetStringUTFLength(env, s);
}

JNIEXPORT jbyteArray JNICALL Java_ikvm_tests_jni_JniEnvTests_getStringUTFChars(JNIEnv* env, jobject self, jstring s)
{
    jboolean isCopy = 2;
    const char* chars = (*env)->GetStringUTFChars(env, s, &isCopy);
    CHECK_R(chars != NULL, NULL);
    CHECK_R(isCopy == JNI_TRUE || isCopy == JNI_FALSE, NULL);

    // the result is NUL terminated, and modified UTF-8 has no other NUL bytes
    jsize length = (jsize)strlen(chars);
    jbyteArray result = (*env)->NewByteArray(env, length);
    (*env)->SetByteArrayRegion(env, result, 0, length, (const jbyte*)chars);
    (*env)->ReleaseStringUTFChars(env, s, chars);

    CHECK_R(length == (*env)->GetStringUTFLength(env, s), NULL);
    return result;
}

JNIEXPORT jcharArray JNICALL Java_ikvm_tests_jni_JniEnvTests_getStringRegion(JNIEnv* env, jobject self, jstring s, jint start, jint len)
{
    jsize n = len > 0 ? len : 0;
    jchar* buf = (jchar*)calloc(n + 1, sizeof(jchar));
    (*env)->GetStringRegion(env, s, start, len, buf);

    jcharArray result = NULL;
    if ((*env)->ExceptionCheck(env) == JNI_FALSE)
    {
        result = (*env)->NewCharArray(env, n);
        (*env)->SetCharArrayRegion(env, result, 0, n, buf);
    }

    free(buf);
    return result;
}

JNIEXPORT jbyteArray JNICALL Java_ikvm_tests_jni_JniEnvTests_getStringUTFRegion(JNIEnv* env, jobject self, jstring s, jint start, jint len)
{
    // a char takes at most three bytes, and the buffer is zeroed so the end of what was written can be found
    jsize n = len > 0 ? len : 0;
    char* buf = (char*)calloc(n * 3 + 1, 1);
    (*env)->GetStringUTFRegion(env, s, start, len, buf);

    jbyteArray result = NULL;
    if ((*env)->ExceptionCheck(env) == JNI_FALSE)
    {
        jsize length = (jsize)strlen(buf);
        result = (*env)->NewByteArray(env, length);
        (*env)->SetByteArrayRegion(env, result, 0, length, (const jbyte*)buf);
    }

    free(buf);
    return result;
}

JNIEXPORT jcharArray JNICALL Java_ikvm_tests_jni_JniEnvTests_getStringCritical(JNIEnv* env, jobject self, jstring s)
{
    jsize length = (*env)->GetStringLength(env, s);
    jchar* buf = (jchar*)malloc(sizeof(jchar) * (length + 1));

    jboolean isCopy = 2;
    const jchar* chars = (*env)->GetStringCritical(env, s, &isCopy);
    if (chars == NULL)
    {
        free(buf);
        FAIL("GetStringCritical returned NULL");
        return NULL;
    }

    memcpy(buf, chars, sizeof(jchar) * length);
    (*env)->ReleaseStringCritical(env, s, chars);
    CHECK_R(isCopy == JNI_TRUE || isCopy == JNI_FALSE, NULL);

    jcharArray result = (*env)->NewCharArray(env, length);
    (*env)->SetCharArrayRegion(env, result, 0, length, buf);
    free(buf);
    return result;
}

// arrays

JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniEnvTests_getArrayLength(JNIEnv* env, jobject self, jarray array)
{
    return (*env)->GetArrayLength(env, array);
}

JNIEXPORT jobjectArray JNICALL Java_ikvm_tests_jni_JniEnvTests_newObjectArray(JNIEnv* env, jobject self, jint length, jclass clazz, jobject init)
{
    return (*env)->NewObjectArray(env, length, clazz, init);
}

JNIEXPORT jobject JNICALL Java_ikvm_tests_jni_JniEnvTests_getObjectArrayElement(JNIEnv* env, jobject self, jobjectArray array, jint index)
{
    return (*env)->GetObjectArrayElement(env, array, index);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_setObjectArrayElement(JNIEnv* env, jobject self, jobjectArray array, jint index, jobject value)
{
    (*env)->SetObjectArrayElement(env, array, index, value);
}

#define NEXT_BOOLEAN(x) ((x) == JNI_FALSE ? JNI_TRUE : JNI_FALSE)
#define NEXT_NUMBER(x) ((x) + 1)

#define DEFINE_ARRAYS(Type, jtype, jarr, NEXT) \
JNIEXPORT jarr JNICALL Java_ikvm_tests_jni_JniEnvTests_new##Type##Array(JNIEnv* env, jobject self, jint length) \
{ \
    return (*env)->New##Type##Array(env, length); \
} \
JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_modify##Type##ArrayElements(JNIEnv* env, jobject self, jarr array, jint mode) \
{ \
    jsize length = (*env)->GetArrayLength(env, array); \
    jboolean isCopy = 2; \
    jtype* elems = (*env)->Get##Type##ArrayElements(env, array, &isCopy); \
    CHECK(elems != NULL); \
    CHECK(isCopy == JNI_TRUE || isCopy == JNI_FALSE); \
    for (jsize i = 0; i < length; i++) \
        elems[i] = (jtype)NEXT(elems[i]); \
    (*env)->Release##Type##ArrayElements(env, array, elems, mode); \
    if (mode == JNI_COMMIT) \
        (*env)->Release##Type##ArrayElements(env, array, elems, JNI_ABORT); \
    NO_EXCEPTION(); \
} \
JNIEXPORT jarr JNICALL Java_ikvm_tests_jni_JniEnvTests_get##Type##ArrayRegion(JNIEnv* env, jobject self, jarr array, jint start, jint len) \
{ \
    jsize n = len > 0 ? len : 0; \
    jtype* buf = (jtype*)calloc(n + 1, sizeof(jtype)); \
    (*env)->Get##Type##ArrayRegion(env, array, start, len, buf); \
    jarr result = NULL; \
    if ((*env)->ExceptionCheck(env) == JNI_FALSE) \
    { \
        result = (*env)->New##Type##Array(env, n); \
        (*env)->Set##Type##ArrayRegion(env, result, 0, n, buf); \
    } \
    free(buf); \
    return result; \
} \
JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_set##Type##ArrayRegion(JNIEnv* env, jobject self, jarr array, jint start, jint len, jarr values) \
{ \
    jsize n = (*env)->GetArrayLength(env, values); \
    jsize capacity = (len > n ? len : n) + 1; \
    jtype* buf = (jtype*)calloc(capacity, sizeof(jtype)); \
    (*env)->Get##Type##ArrayRegion(env, values, 0, n, buf); \
    if ((*env)->ExceptionCheck(env) == JNI_FALSE) \
        (*env)->Set##Type##ArrayRegion(env, array, start, len, buf); \
    free(buf); \
}

DEFINE_ARRAYS(Boolean, jboolean, jbooleanArray, NEXT_BOOLEAN)
DEFINE_ARRAYS(Byte, jbyte, jbyteArray, NEXT_NUMBER)
DEFINE_ARRAYS(Char, jchar, jcharArray, NEXT_NUMBER)
DEFINE_ARRAYS(Short, jshort, jshortArray, NEXT_NUMBER)
DEFINE_ARRAYS(Int, jint, jintArray, NEXT_NUMBER)
DEFINE_ARRAYS(Long, jlong, jlongArray, NEXT_NUMBER)
DEFINE_ARRAYS(Float, jfloat, jfloatArray, NEXT_NUMBER)
DEFINE_ARRAYS(Double, jdouble, jdoubleArray, NEXT_NUMBER)

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_modifyPrimitiveArrayCritical(JNIEnv* env, jobject self, jintArray array, jint mode)
{
    jsize length = (*env)->GetArrayLength(env, array);
    jboolean isCopy = 2;
    jint* elems = (jint*)(*env)->GetPrimitiveArrayCritical(env, array, &isCopy);
    CHECK(elems != NULL);

    for (jsize i = 0; i < length; i++)
        elems[i] += 1;

    (*env)->ReleasePrimitiveArrayCritical(env, array, elems, mode);
    CHECK(isCopy == JNI_TRUE || isCopy == JNI_FALSE);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_nestedPrimitiveArrayCritical(JNIEnv* env, jobject self, jintArray a, jbyteArray b)
{
    jsize aLength = (*env)->GetArrayLength(env, a);
    jsize bLength = (*env)->GetArrayLength(env, b);

    jint* aElems = (jint*)(*env)->GetPrimitiveArrayCritical(env, a, NULL);
    jbyte* bElems = (jbyte*)(*env)->GetPrimitiveArrayCritical(env, b, NULL);
    if (aElems == NULL || bElems == NULL)
    {
        if (bElems != NULL)
            (*env)->ReleasePrimitiveArrayCritical(env, b, bElems, 0);
        if (aElems != NULL)
            (*env)->ReleasePrimitiveArrayCritical(env, a, aElems, 0);
        FAIL("GetPrimitiveArrayCritical returned NULL");
        return;
    }

    for (jsize i = 0; i < aLength; i++)
        aElems[i] += 1;
    for (jsize i = 0; i < bLength; i++)
        bElems[i] += 1;

    // released in the order they were taken, rather than nested
    (*env)->ReleasePrimitiveArrayCritical(env, a, aElems, 0);
    (*env)->ReleasePrimitiveArrayCritical(env, b, bElems, 0);
    NO_EXCEPTION();
}

// natives

static jint JNICALL registeredImpl(JNIEnv* env, jclass clazz, jint value)
{
    return value * 2 + 1;
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_registerNatives(JNIEnv* env, jobject self)
{
    jclass c = (*env)->FindClass(env, TESTS_CLASS "$RegisteredNatives");
    CHECK(c != NULL);

    JNINativeMethod method = { "registered", "(I)I", (void*)registeredImpl };
    CHECK((*env)->RegisterNatives(env, c, &method, 1) == JNI_OK);

    jmethodID m = (*env)->GetStaticMethodID(env, c, "registered", "(I)I");
    CHECK((*env)->CallStaticIntMethod(env, c, m, 20) == 41);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_unregisterNatives(JNIEnv* env, jobject self)
{
    jclass c = (*env)->FindClass(env, TESTS_CLASS "$UnregisteredNatives");
    CHECK(c != NULL);

    JNINativeMethod method = { "unregistered", "(I)I", (void*)registeredImpl };
    CHECK((*env)->RegisterNatives(env, c, &method, 1) == JNI_OK);

    jmethodID m = (*env)->GetStaticMethodID(env, c, "unregistered", "(I)I");
    CHECK((*env)->CallStaticIntMethod(env, c, m, 1) == 3);
    NO_EXCEPTION();

    // with the registration gone there is nothing to bind the method to, since this library doesn't export it
    CHECK((*env)->UnregisterNatives(env, c) == JNI_OK);
    (*env)->CallStaticIntMethod(env, c, m, 1);
    EXPECT_EXCEPTION("java/lang/UnsatisfiedLinkError");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_registerNativesNotFound(JNIEnv* env, jobject self)
{
    jclass c = (*env)->FindClass(env, TESTS_CLASS "$RegisteredNatives");
    JNINativeMethod method = { "missing", "(I)I", (void*)registeredImpl };
    CHECK((*env)->RegisterNatives(env, c, &method, 1) < 0);
    EXPECT_EXCEPTION("java/lang/NoSuchMethodError");
}

JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniEnvTests_under_1score(JNIEnv* env, jobject self)
{
    return 1;
}

JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniEnvTests_dollar_00024sign(JNIEnv* env, jobject self)
{
    return 2;
}

JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniEnvTests_caf_000e9(JNIEnv* env, jobject self)
{
    return 3;
}

JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniEnvTests_overloaded__I(JNIEnv* env, jobject self, jint value)
{
    return 4;
}

JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniEnvTests_overloaded__Ljava_lang_String_2(JNIEnv* env, jobject self, jstring value)
{
    return 5;
}

JNIEXPORT jstring JNICALL Java_ikvm_tests_jni_JniEnvTests_returnsWrongType(JNIEnv* env, jobject self, jobject o)
{
    return (jstring)o;
}

JNIEXPORT jboolean JNICALL Java_ikvm_tests_jni_JniEnvTests_synchronizedHoldsLock(JNIEnv* env, jobject self)
{
    return holdsLock(env, self);
}

JNIEXPORT jboolean JNICALL Java_ikvm_tests_jni_JniEnvTests_staticSynchronizedHoldsLock(JNIEnv* env, jclass clazz)
{
    return holdsLock(env, clazz);
}

// monitors

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_monitorEnterAndExit(JNIEnv* env, jobject self, jobject o)
{
    CHECK(holdsLock(env, o) == JNI_FALSE);
    CHECK((*env)->MonitorEnter(env, o) == JNI_OK);
    CHECK(holdsLock(env, o) == JNI_TRUE);
    CHECK((*env)->MonitorExit(env, o) == JNI_OK);
    CHECK(holdsLock(env, o) == JNI_FALSE);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_monitorEnterIsReentrant(JNIEnv* env, jobject self, jobject o)
{
    CHECK((*env)->MonitorEnter(env, o) == JNI_OK);
    CHECK((*env)->MonitorEnter(env, o) == JNI_OK);
    CHECK((*env)->MonitorExit(env, o) == JNI_OK);
    CHECK(holdsLock(env, o) == JNI_TRUE);
    CHECK((*env)->MonitorExit(env, o) == JNI_OK);
    CHECK(holdsLock(env, o) == JNI_FALSE);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_monitorExitNotOwned(JNIEnv* env, jobject self, jobject o)
{
    CHECK((*env)->MonitorExit(env, o) < 0);
    EXPECT_EXCEPTION("java/lang/IllegalMonitorStateException");
}

// direct buffers

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_newDirectByteBuffer(JNIEnv* env, jobject self)
{
    jbyte* memory = (jbyte*)calloc(16, 1);
    jobject buffer = (*env)->NewDirectByteBuffer(env, memory, 16);
    if (buffer == NULL)
    {
        free(memory);
        FAIL("NewDirectByteBuffer returned NULL");
        return;
    }

    jclass c = (*env)->FindClass(env, "java/nio/ByteBuffer");
    jmethodID isDirect = (*env)->GetMethodID(env, c, "isDirect", "()Z");
    jmethodID put = (*env)->GetMethodID(env, c, "put", "(IB)Ljava/nio/ByteBuffer;");
    jmethodID get = (*env)->GetMethodID(env, c, "get", "(I)B");

    // writes through the buffer land in the memory, and the other way around
    jboolean direct = (*env)->CallBooleanMethod(env, buffer, isDirect);
    (*env)->CallObjectMethod(env, buffer, put, 3, (jbyte)42);
    memory[5] = 7;
    jbyte read = (*env)->CallByteMethod(env, buffer, get, 5);
    jboolean sameAddress = (*env)->GetDirectBufferAddress(env, buffer) == memory;
    jlong capacity = (*env)->GetDirectBufferCapacity(env, buffer);
    jbyte written = memory[3];
    free(memory);

    NO_EXCEPTION();
    CHECK(direct == JNI_TRUE);
    CHECK(sameAddress);
    CHECK(capacity == 16);
    CHECK(written == 42);
    CHECK(read == 7);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getDirectBufferAddressOfHeapBuffer(JNIEnv* env, jobject self)
{
    jclass c = (*env)->FindClass(env, TESTS_CLASS);
    jmethodID allocate = (*env)->GetStaticMethodID(env, c, "allocateHeapBuffer", "(I)Ljava/nio/ByteBuffer;");
    jobject buffer = (*env)->CallStaticObjectMethod(env, c, allocate, 8);
    CHECK(buffer != NULL);

    // a buffer that isn't direct has no address and a capacity of -1
    CHECK((*env)->GetDirectBufferAddress(env, buffer) == NULL);
    NO_EXCEPTION();
    CHECK((*env)->GetDirectBufferCapacity(env, buffer) == -1);
    NO_EXCEPTION();
}

// virtual machine

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getJavaVM(JNIEnv* env, jobject self)
{
    JavaVM* vm = NULL;
    CHECK((*env)->GetJavaVM(env, &vm) == JNI_OK);
    CHECK(vm != NULL);

    jint versions[] = { JNI_VERSION_1_1, JNI_VERSION_1_2, JNI_VERSION_1_4, JNI_VERSION_1_6, JNI_VERSION_1_8 };
    for (size_t i = 0; i < sizeof(versions) / sizeof(versions[0]); i++)
    {
        JNIEnv* e = NULL;
        CHECK((*vm)->GetEnv(vm, (void**)&e, versions[i]) == JNI_OK);
        CHECK(e == env);
    }
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getEnvWithUnsupportedVersion(JNIEnv* env, jobject self)
{
    JavaVM* vm = NULL;
    CHECK((*env)->GetJavaVM(env, &vm) == JNI_OK);

    JNIEnv* e = (JNIEnv*)1;
    CHECK((*vm)->GetEnv(vm, (void**)&e, 0x00090009) == JNI_EVERSION);
    CHECK(e == NULL);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_getCreatedJavaVMs(JNIEnv* env, jobject self)
{
    JavaVM* vm = NULL;
    CHECK((*env)->GetJavaVM(env, &vm) == JNI_OK);

    JavaVM* vms[2] = { NULL, NULL };
    jsize count = -1;
    CHECK(JNI_GetCreatedJavaVMs(vms, 2, &count) == JNI_OK);
    CHECK(count == 1);
    CHECK(vms[0] == vm);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_createJavaVMWhenOneExists(JNIEnv* env, jobject self)
{
    JavaVMInitArgs args;
    args.version = JNI_VERSION_1_8;
    args.nOptions = 0;
    args.options = NULL;
    args.ignoreUnrecognized = JNI_TRUE;

    JavaVM* vm = NULL;
    JNIEnv* e = NULL;
    CHECK(JNI_CreateJavaVM(&vm, (void**)&e, &args) < 0);
}

typedef struct
{
    JavaVM* vm;
    int variant;
    const char* error;
} AttachContext;

#define THREAD_CHECK(cond) do { if (!(cond)) { context->error = #cond; return; } } while (0)

static void attachThreadMain(AttachContext* context)
{
    JavaVM* vm = context->vm;
    JNIEnv* env = NULL;

    // a thread created by native code starts out detached
    THREAD_CHECK((*vm)->GetEnv(vm, (void**)&env, JNI_VERSION_1_8) == JNI_EDETACHED);

    // the same thread can attach and detach repeatedly
    if (context->variant == 3)
    {
        for (int i = 0; i < 50; i++)
        {
            THREAD_CHECK((*vm)->AttachCurrentThread(vm, (void**)&env, NULL) == JNI_OK);
            THREAD_CHECK((*env)->FindClass(env, "java/lang/Object") != NULL);
            THREAD_CHECK((*vm)->DetachCurrentThread(vm) == JNI_OK);
        }

        return;
    }

    JavaVMAttachArgs args;
    args.version = JNI_VERSION_1_8;
    args.name = "jni-attached";
    args.group = NULL;

    switch (context->variant)
    {
        case 0:
            THREAD_CHECK((*vm)->AttachCurrentThread(vm, (void**)&env, NULL) == JNI_OK);
            break;
        case 1:
            THREAD_CHECK((*vm)->AttachCurrentThread(vm, (void**)&env, &args) == JNI_OK);
            break;
        case 2:
            THREAD_CHECK((*vm)->AttachCurrentThreadAsDaemon(vm, (void**)&env, NULL) == JNI_OK);
            break;
    }

    THREAD_CHECK(env != NULL);

    JNIEnv* current = NULL;
    THREAD_CHECK((*vm)->GetEnv(vm, (void**)&current, JNI_VERSION_1_8) == JNI_OK);
    THREAD_CHECK(current == env);

    // the attached thread can call Java
    jclass threadClass = (*env)->FindClass(env, "java/lang/Thread");
    THREAD_CHECK(threadClass != NULL);
    jmethodID currentThread = (*env)->GetStaticMethodID(env, threadClass, "currentThread", "()Ljava/lang/Thread;");
    jobject thread = (*env)->CallStaticObjectMethod(env, threadClass, currentThread);
    THREAD_CHECK(thread != NULL);

    if (context->variant == 1)
    {
        jmethodID getName = (*env)->GetMethodID(env, threadClass, "getName", "()Ljava/lang/String;");
        jstring name = (jstring)(*env)->CallObjectMethod(env, thread, getName);
        THREAD_CHECK(name != NULL);
        THREAD_CHECK(stringEquals(env, name, "jni-attached"));
    }

    if (context->variant == 2)
    {
        jmethodID isDaemon = (*env)->GetMethodID(env, threadClass, "isDaemon", "()Z");
        THREAD_CHECK((*env)->CallBooleanMethod(env, thread, isDaemon) == JNI_TRUE);
    }

    THREAD_CHECK((*env)->ExceptionCheck(env) == JNI_FALSE);

    // attaching an attached thread does nothing
    JNIEnv* again = NULL;
    THREAD_CHECK((*vm)->AttachCurrentThread(vm, (void**)&again, NULL) == JNI_OK);
    THREAD_CHECK(again == env);

    THREAD_CHECK((*vm)->DetachCurrentThread(vm) == JNI_OK);
    THREAD_CHECK((*vm)->GetEnv(vm, (void**)&current, JNI_VERSION_1_8) == JNI_EDETACHED);

    // detaching a detached thread is allowed
    THREAD_CHECK((*vm)->DetachCurrentThread(vm) == JNI_OK);
}

#ifdef _WIN32

static DWORD WINAPI attachThreadStart(LPVOID p)
{
    attachThreadMain((AttachContext*)p);
    return 0;
}

static int runAttachThread(AttachContext* context)
{
    HANDLE h = CreateThread(NULL, 0, attachThreadStart, context, 0, NULL);
    if (h == NULL)
        return 0;

    WaitForSingleObject(h, INFINITE);
    CloseHandle(h);
    return 1;
}

#else

static void* attachThreadStart(void* p)
{
    attachThreadMain((AttachContext*)p);
    return NULL;
}

static int runAttachThread(AttachContext* context)
{
    pthread_t t;
    if (pthread_create(&t, NULL, attachThreadStart, context) != 0)
        return 0;

    pthread_join(t, NULL);
    return 1;
}

#endif

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_attachCurrentThread(JNIEnv* env, jobject self, jint variant)
{
    AttachContext context;
    CHECK((*env)->GetJavaVM(env, &context.vm) == JNI_OK);
    context.variant = variant;
    context.error = NULL;

    CHECK(runAttachThread(&context));
    if (context.error != NULL)
        FAIL(context.error);
}

// native method arguments and results

JNIEXPORT jstring JNICALL Java_ikvm_tests_jni_JniEnvTests_echoArgs(JNIEnv* env, jobject self, jboolean z, jbyte b, jchar c, jshort s, jint i, jlong j, jfloat f, jdouble d, jobject l)
{
    jclass targetClass = (*env)->FindClass(env, TARGET_CLASS);
    jmethodID format = (*env)->GetStaticMethodID(env, targetClass, "format", ARGS_SIG "Ljava/lang/String;");
    CHECK_R(format != NULL, NULL);
    return (jstring)(*env)->CallStaticObjectMethod(env, targetClass, format, z, b, c, s, i, j, f, d, l);
}

JNIEXPORT jstring JNICALL Java_ikvm_tests_jni_JniEnvTests_staticEchoArgs(JNIEnv* env, jclass clazz, jboolean z, jbyte b, jchar c, jshort s, jint i, jlong j, jfloat f, jdouble d, jobject l)
{
    return Java_ikvm_tests_jni_JniEnvTests_echoArgs(env, NULL, z, b, c, s, i, j, f, d, l);
}

JNIEXPORT jdouble JNICALL Java_ikvm_tests_jni_JniEnvTests_manyNativeArgs(JNIEnv* env, jobject self, jint i1, jlong j1, jfloat f1, jdouble d1, jint i2, jlong j2, jfloat f2, jdouble d2, jint i3, jlong j3, jfloat f3, jdouble d3, jint i4, jlong j4, jfloat f4, jdouble d4)
{
    // the same expression as Target.weigh, which C evaluates with the same types as Java
    return i1 + 2 * j1 + 3 * f1 + 4 * d1 + 5 * i2 + 6 * j2 + 7 * f2 + 8 * d2 + 9 * i3 + 10 * j3 + 11 * f3 + 12 * d3 + 13 * i4 + 14 * j4 + 15 * f4 + 16 * d4;
}

JNIEXPORT jboolean JNICALL Java_ikvm_tests_jni_JniEnvTests_returnBoolean(JNIEnv* env, jobject self, jboolean value)
{
    return value;
}

JNIEXPORT jbyte JNICALL Java_ikvm_tests_jni_JniEnvTests_returnByte(JNIEnv* env, jobject self, jbyte value)
{
    return value;
}

JNIEXPORT jchar JNICALL Java_ikvm_tests_jni_JniEnvTests_returnChar(JNIEnv* env, jobject self, jchar value)
{
    return value;
}

JNIEXPORT jshort JNICALL Java_ikvm_tests_jni_JniEnvTests_returnShort(JNIEnv* env, jobject self, jshort value)
{
    return value;
}

JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniEnvTests_returnInt(JNIEnv* env, jobject self, jint value)
{
    return value;
}

JNIEXPORT jlong JNICALL Java_ikvm_tests_jni_JniEnvTests_returnLong(JNIEnv* env, jobject self, jlong value)
{
    return value;
}

JNIEXPORT jfloat JNICALL Java_ikvm_tests_jni_JniEnvTests_returnFloat(JNIEnv* env, jobject self, jfloat value)
{
    return value;
}

JNIEXPORT jdouble JNICALL Java_ikvm_tests_jni_JniEnvTests_returnDouble(JNIEnv* env, jobject self, jdouble value)
{
    return value;
}

JNIEXPORT jobject JNICALL Java_ikvm_tests_jni_JniEnvTests_returnObject(JNIEnv* env, jobject self, jobject value)
{
    return value;
}

JNIEXPORT jintArray JNICALL Java_ikvm_tests_jni_JniEnvTests_returnArray(JNIEnv* env, jobject self, jintArray value)
{
    return value;
}

JNIEXPORT jobject JNICALL Java_ikvm_tests_jni_JniEnvTests_returnNull(JNIEnv* env, jobject self)
{
    return NULL;
}

static jobject lastReturnedGlobalRef = NULL;

JNIEXPORT jobject JNICALL Java_ikvm_tests_jni_JniEnvTests_returnGlobalRef(JNIEnv* env, jobject self, jobject value)
{
    // a native method may return a global reference; the one from the previous call is no longer needed
    if (lastReturnedGlobalRef != NULL)
        (*env)->DeleteGlobalRef(env, lastReturnedGlobalRef);

    lastReturnedGlobalRef = (*env)->NewGlobalRef(env, value);
    return lastReturnedGlobalRef;
}

JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniEnvTests_throwAndReturnValue(JNIEnv* env, jobject self)
{
    (*env)->ThrowNew(env, (*env)->FindClass(env, "java/lang/IllegalStateException"), "returned anyway");
    return 42;
}

// more calls

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_callMethodsOnString(JNIEnv* env, jobject self, jstring s)
{
    jclass objectClass = (*env)->FindClass(env, "java/lang/Object");
    jclass stringClass = (*env)->FindClass(env, "java/lang/String");
    jclass charSequenceClass = (*env)->FindClass(env, "java/lang/CharSequence");
    jclass comparableClass = (*env)->FindClass(env, "java/lang/Comparable");

    // String is a .NET string, so these go through the remapped type
    jmethodID length = (*env)->GetMethodID(env, stringClass, "length", "()I");
    CHECK((*env)->CallIntMethod(env, s, length) == 5);

    jmethodID substring = (*env)->GetMethodID(env, stringClass, "substring", "(II)Ljava/lang/String;");
    CHECK(stringEquals(env, (jstring)(*env)->CallObjectMethod(env, s, substring, 1, 3), "el"));

    jmethodID equals = (*env)->GetMethodID(env, stringClass, "equals", "(Ljava/lang/Object;)Z");
    CHECK((*env)->CallBooleanMethod(env, s, equals, (*env)->NewStringUTF(env, "hello")) == JNI_TRUE);

    jmethodID objectToString = (*env)->GetMethodID(env, objectClass, "toString", "()Ljava/lang/String;");
    CHECK(stringEquals(env, (jstring)(*env)->CallObjectMethod(env, s, objectToString), "hello"));

    jmethodID objectHashCode = (*env)->GetMethodID(env, objectClass, "hashCode", "()I");
    jmethodID stringHashCode = (*env)->GetMethodID(env, stringClass, "hashCode", "()I");
    CHECK((*env)->CallIntMethod(env, s, objectHashCode) == (*env)->CallIntMethod(env, s, stringHashCode));

    // CharSequence and Comparable are implemented by String only on the Java side
    jmethodID charSequenceLength = (*env)->GetMethodID(env, charSequenceClass, "length", "()I");
    CHECK((*env)->CallIntMethod(env, s, charSequenceLength) == 5);

    jmethodID compareTo = (*env)->GetMethodID(env, comparableClass, "compareTo", "(Ljava/lang/Object;)I");
    CHECK((*env)->CallIntMethod(env, s, compareTo, (*env)->NewStringUTF(env, "hellp")) < 0);

    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_callObjectMethodsOnArray(JNIEnv* env, jobject self, jintArray a)
{
    jclass objectClass = (*env)->FindClass(env, "java/lang/Object");
    jclass systemClass = (*env)->FindClass(env, "java/lang/System");

    jmethodID getClass = (*env)->GetMethodID(env, objectClass, "getClass", "()Ljava/lang/Class;");
    CHECK((*env)->IsSameObject(env, (*env)->CallObjectMethod(env, a, getClass), (*env)->FindClass(env, "[I")));

    jmethodID hashCode = (*env)->GetMethodID(env, objectClass, "hashCode", "()I");
    jmethodID identityHashCode = (*env)->GetStaticMethodID(env, systemClass, "identityHashCode", "(Ljava/lang/Object;)I");
    CHECK((*env)->CallIntMethod(env, a, hashCode) == (*env)->CallStaticIntMethod(env, systemClass, identityHashCode, a));

    jmethodID equals = (*env)->GetMethodID(env, objectClass, "equals", "(Ljava/lang/Object;)Z");
    CHECK((*env)->CallBooleanMethod(env, a, equals, a) == JNI_TRUE);
    CHECK((*env)->CallBooleanMethod(env, a, equals, (*env)->NewIntArray(env, 1)) == JNI_FALSE);

    NO_EXCEPTION();
}

JNIEXPORT jstring JNICALL Java_ikvm_tests_jni_JniEnvTests_callThrowableGetMessage(JNIEnv* env, jobject self, jthrowable t)
{
    jmethodID getMessage = (*env)->GetMethodID(env, (*env)->FindClass(env, "java/lang/Throwable"), "getMessage", "()Ljava/lang/String;");
    CHECK_R(getMessage != NULL, NULL);
    return (jstring)(*env)->CallObjectMethod(env, t, getMessage);
}

JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniEnvTests_callDefaultMethod(JNIEnv* env, jobject self, jobject target)
{
    jmethodID m = (*env)->GetMethodID(env, (*env)->FindClass(env, TARGET_INTERFACE), "defaultMethod", "()I");
    CHECK_R(m != NULL, 0);
    return (*env)->CallIntMethod(env, target, m);
}

JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniEnvTests_callPrivateMethod(JNIEnv* env, jobject self, jobject target)
{
    // JNI does not check access
    jclass c = (*env)->FindClass(env, TARGET_CLASS);
    jmethodID m = (*env)->GetMethodID(env, c, "privateMethod", "()I");
    CHECK_R(m != NULL, 0);
    CHECK_R((*env)->CallNonvirtualIntMethod(env, target, c, m) == 7, 0);
    return (*env)->CallIntMethod(env, target, m);
}

JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniEnvTests_callInheritedStaticMethod(JNIEnv* env, jobject self)
{
    jclass c = (*env)->FindClass(env, SUB_TARGET_CLASS);
    jmethodID m = (*env)->GetStaticMethodID(env, c, "staticSimpleMethod", "()I");
    CHECK_R(m != NULL, 0);
    return (*env)->CallStaticIntMethod(env, c, m);
}

JNIEXPORT jobject JNICALL Java_ikvm_tests_jni_JniEnvTests_constructWithCallNonvirtualVoidMethod(JNIEnv* env, jobject self)
{
    // allocating first and running the constructor afterwards is a common way to construct from native code
    jclass c = (*env)->FindClass(env, TARGET_CLASS);
    jobject o = (*env)->AllocObject(env, c);
    CHECK_R(o != NULL, NULL);

    jmethodID ctor = (*env)->GetMethodID(env, c, "<init>", ARGS_SIG "V");
    jstring l = (*env)->NewStringUTF(env, "arg");
    (*env)->CallNonvirtualVoidMethod(env, o, c, ctor, VARARGS(l));
    NO_EXCEPTION_R(NULL);
    return o;
}

JNIEXPORT jintArray JNICALL Java_ikvm_tests_jni_JniEnvTests_callWithArrays(JNIEnv* env, jobject self, jobject target, jintArray a)
{
    jmethodID m = (*env)->GetMethodID(env, (*env)->FindClass(env, TARGET_CLASS), "reverse", "([I)[I");
    CHECK_R(m != NULL, NULL);
    return (jintArray)(*env)->CallObjectMethod(env, target, m, a);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_callReturningNull(JNIEnv* env, jobject self, jobject target)
{
    jmethodID m = (*env)->GetMethodID(env, (*env)->FindClass(env, TARGET_CLASS), "nullMethod", "()Ljava/lang/Object;");
    CHECK((*env)->CallObjectMethod(env, target, m) == NULL);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_callThrowingChecked(JNIEnv* env, jobject self, jobject target)
{
    jmethodID m = (*env)->GetMethodID(env, (*env)->FindClass(env, TARGET_CLASS), "throwsChecked", "()V");
    (*env)->CallVoidMethod(env, target, m);
    EXPECT_EXCEPTION("java/io/IOException");
}

#define MANY_ARGS_SIG "(IJFDIJFDIJFDIJFD)D"
#define MANY_ARGS (jint)1, (jlong)2, (jfloat)0.5f, (jdouble)0.25, (jint)-3, (jlong)4000000000LL, (jfloat)1.5f, (jdouble)-2.75, (jint)5, (jlong)-6, (jfloat)2.5f, (jdouble)8.125, (jint)-7, (jlong)9, (jfloat)-3.25f, (jdouble)1e10

static void fillManyArgs(jvalue* v)
{
    v[0].i = 1; v[1].j = 2; v[2].f = 0.5f; v[3].d = 0.25;
    v[4].i = -3; v[5].j = 4000000000LL; v[6].f = 1.5f; v[7].d = -2.75;
    v[8].i = 5; v[9].j = -6; v[10].f = 2.5f; v[11].d = 8.125;
    v[12].i = -7; v[13].j = 9; v[14].f = -3.25f; v[15].d = 1e10;
}

JNIEXPORT jdouble JNICALL Java_ikvm_tests_jni_JniEnvTests_callManyArgs(JNIEnv* env, jobject self, jobject target, jint mode)
{
    jmethodID m = (*env)->GetMethodID(env, (*env)->FindClass(env, TARGET_CLASS), "manyArgs", MANY_ARGS_SIG);
    CHECK_R(m != NULL, 0);
    jvalue v[16];
    fillManyArgs(v);

    switch (mode)
    {
        case 0: return (*env)->CallDoubleMethod(env, target, m, MANY_ARGS);
        case 1: return callDoubleMethodV(env, target, m, MANY_ARGS);
        default: return (*env)->CallDoubleMethodA(env, target, m, v);
    }
}

JNIEXPORT jdouble JNICALL Java_ikvm_tests_jni_JniEnvTests_callStaticManyArgs(JNIEnv* env, jobject self, jint mode)
{
    jclass c = (*env)->FindClass(env, TARGET_CLASS);
    jmethodID m = (*env)->GetStaticMethodID(env, c, "staticManyArgs", MANY_ARGS_SIG);
    CHECK_R(m != NULL, 0);
    jvalue v[16];
    fillManyArgs(v);

    switch (mode)
    {
        case 0: return (*env)->CallStaticDoubleMethod(env, c, m, MANY_ARGS);
        case 1: return callStaticDoubleMethodV(env, c, m, MANY_ARGS);
        default: return (*env)->CallStaticDoubleMethodA(env, c, m, v);
    }
}

// more fields

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_accessPrivateField(JNIEnv* env, jobject self, jobject target)
{
    // JNI does not check access
    jfieldID f = (*env)->GetFieldID(env, (*env)->FindClass(env, TARGET_CLASS), "privateField", "I");
    CHECK(f != NULL);
    CHECK((*env)->GetIntField(env, target, f) == 3);
    (*env)->SetIntField(env, target, f, 5);
    CHECK((*env)->GetIntField(env, target, f) == 5);
    NO_EXCEPTION();
}

JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniEnvTests_getStaticConstant(JNIEnv* env, jobject self)
{
    jclass c = (*env)->FindClass(env, TARGET_CLASS);
    jfieldID f = (*env)->GetStaticFieldID(env, c, "CONSTANT", "I");
    CHECK_R(f != NULL, 0);
    return (*env)->GetStaticIntField(env, c, f);
}

JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniEnvTests_getInterfaceConstant(JNIEnv* env, jobject self)
{
    jclass c = (*env)->FindClass(env, TARGET_INTERFACE);
    jfieldID f = (*env)->GetStaticFieldID(env, c, "CONSTANT", "I");
    CHECK_R(f != NULL, 0);
    return (*env)->GetStaticIntField(env, c, f);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_accessInheritedStaticField(JNIEnv* env, jobject self)
{
    // a static field can be looked up through a subclass, and is the same field
    jclass subTargetClass = (*env)->FindClass(env, SUB_TARGET_CLASS);
    jclass targetClass = (*env)->FindClass(env, TARGET_CLASS);
    jfieldID inherited = (*env)->GetStaticFieldID(env, subTargetClass, "iStatic", "I");
    jfieldID declared = (*env)->GetStaticFieldID(env, targetClass, "iStatic", "I");
    CHECK(inherited != NULL);
    CHECK(declared != NULL);

    (*env)->SetStaticIntField(env, subTargetClass, inherited, 123);
    CHECK((*env)->GetStaticIntField(env, targetClass, declared) == 123);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_accessArrayAndStringFields(JNIEnv* env, jobject self, jobject target)
{
    jclass c = (*env)->FindClass(env, TARGET_CLASS);
    jfieldID arrayField = (*env)->GetFieldID(env, c, "intArrayField", "[I");
    jfieldID stringField = (*env)->GetFieldID(env, c, "stringField", "Ljava/lang/String;");
    CHECK(arrayField != NULL);
    CHECK(stringField != NULL);

    jintArray a = (*env)->NewIntArray(env, 3);
    (*env)->SetObjectField(env, target, arrayField, a);
    CHECK((*env)->IsSameObject(env, (*env)->GetObjectField(env, target, arrayField), a));

    (*env)->SetObjectField(env, target, stringField, (*env)->NewStringUTF(env, "s"));
    CHECK(stringEquals(env, (jstring)(*env)->GetObjectField(env, target, stringField), "s"));

    (*env)->SetObjectField(env, target, stringField, NULL);
    CHECK((*env)->GetObjectField(env, target, stringField) == NULL);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_accessShadowedFields(JNIEnv* env, jobject self, jobject subTarget)
{
    // SubTarget declares a field with the same name as one of Target, and each class finds its own
    jfieldID inTarget = (*env)->GetFieldID(env, (*env)->FindClass(env, TARGET_CLASS), "shadowed", "I");
    jfieldID inSubTarget = (*env)->GetFieldID(env, (*env)->FindClass(env, SUB_TARGET_CLASS), "shadowed", "I");
    CHECK(inTarget != NULL);
    CHECK(inSubTarget != NULL);
    CHECK(inTarget != inSubTarget);
    CHECK((*env)->GetIntField(env, subTarget, inTarget) == 1);
    CHECK((*env)->GetIntField(env, subTarget, inSubTarget) == 2);
    NO_EXCEPTION();
}

// more classes

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_findArrayAndNestedClasses(JNIEnv* env, jobject self)
{
    jclass intArrayClass = (*env)->FindClass(env, "[I");
    jclass intArrayArrayClass = (*env)->FindClass(env, "[[I");
    CHECK(intArrayArrayClass != NULL);
    CHECK((*env)->IsSameObject(env, intArrayArrayClass, (*env)->GetObjectClass(env, (*env)->NewObjectArray(env, 1, intArrayClass, NULL))));
    CHECK((*env)->IsSameObject(env, (*env)->GetSuperclass(env, intArrayArrayClass), (*env)->FindClass(env, "java/lang/Object")));

    CHECK((*env)->FindClass(env, "[[Ljava/lang/String;") != NULL);
    CHECK((*env)->FindClass(env, "[L" TARGET_CLASS ";") != NULL);
    CHECK((*env)->FindClass(env, TARGET_INTERFACE) != NULL);
    CHECK((*env)->FindClass(env, ABSTRACT_TARGET_CLASS) != NULL);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_findClassRunsStaticInitializer(JNIEnv* env, jobject self)
{
    jclass tracker = (*env)->FindClass(env, TESTS_CLASS "$InitTracker");
    jfieldID initialized = (*env)->GetStaticFieldID(env, tracker, "initialized", "Z");
    CHECK((*env)->GetStaticBooleanField(env, tracker, initialized) == JNI_FALSE);

    CHECK((*env)->FindClass(env, TESTS_CLASS "$Initialized") != NULL);
    CHECK((*env)->GetStaticBooleanField(env, tracker, initialized) == JNI_TRUE);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_findClassWithFailingStaticInitializer(JNIEnv* env, jobject self)
{
    CHECK((*env)->FindClass(env, TESTS_CLASS "$FailingInitializer") == NULL);
    EXPECT_EXCEPTION("java/lang/ExceptionInInitializerError");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_arraysImplementCloneableAndSerializable(JNIEnv* env, jobject self)
{
    jclass cloneable = (*env)->FindClass(env, "java/lang/Cloneable");
    jclass serializable = (*env)->FindClass(env, "java/io/Serializable");
    jintArray a = (*env)->NewIntArray(env, 1);

    CHECK((*env)->IsInstanceOf(env, a, cloneable) == JNI_TRUE);
    CHECK((*env)->IsInstanceOf(env, a, serializable) == JNI_TRUE);
    CHECK((*env)->IsAssignableFrom(env, (*env)->FindClass(env, "[I"), cloneable) == JNI_TRUE);
    CHECK((*env)->IsAssignableFrom(env, (*env)->FindClass(env, "[Ljava/lang/String;"), serializable) == JNI_TRUE);
    NO_EXCEPTION();
}

// more exceptions

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_throwNewWithUnicodeMessage(JNIEnv* env, jobject self)
{
    // the message is modified UTF-8, which for these characters is the same as UTF-8
    (*env)->ThrowNew(env, (*env)->FindClass(env, "java/lang/IllegalStateException"), "caf\xC3\xA9 \xE2\x98\x83");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_throwNewChecked(JNIEnv* env, jobject self)
{
    (*env)->ThrowNew(env, (*env)->FindClass(env, "java/io/IOException"), "checked");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_rethrow(JNIEnv* env, jobject self, jthrowable t)
{
    CHECK((*env)->Throw(env, t) == JNI_OK);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_exceptionOccurredIsTheThrownObject(JNIEnv* env, jobject self)
{
    jobject target = newTarget(env, TARGET_CLASS);
    jmethodID m = (*env)->GetMethodID(env, (*env)->FindClass(env, TARGET_CLASS), "throwingMethod", "()V");
    (*env)->CallVoidMethod(env, target, m);

    jthrowable t = (*env)->ExceptionOccurred(env);
    CHECK(t != NULL);
    (*env)->ExceptionClear(env);

    // the reference stays valid after the exception is cleared
    CHECK((*env)->IsInstanceOf(env, t, (*env)->FindClass(env, "java/lang/IllegalStateException")));
    jmethodID getMessage = (*env)->GetMethodID(env, (*env)->FindClass(env, "java/lang/Throwable"), "getMessage", "()Ljava/lang/String;");
    CHECK(stringEquals(env, (jstring)(*env)->CallObjectMethod(env, t, getMessage), "thrown by target"));
    NO_EXCEPTION();
}

// more references

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_convertBetweenRefKinds(JNIEnv* env, jobject self)
{
    jobject local = (*env)->NewIntArray(env, 1);
    jobject global = (*env)->NewGlobalRef(env, local);
    jweak weak = (*env)->NewWeakGlobalRef(env, global);
    jobject globalFromWeak = (*env)->NewGlobalRef(env, weak);
    jweak weakFromWeak = (*env)->NewWeakGlobalRef(env, weak);
    jobject localFromWeak = (*env)->NewLocalRef(env, weak);
    jobject localFromGlobal = (*env)->NewLocalRef(env, global);

    jboolean typesOk =
        (*env)->GetObjectRefType(env, globalFromWeak) == JNIGlobalRefType &&
        (*env)->GetObjectRefType(env, weakFromWeak) == JNIWeakGlobalRefType &&
        (*env)->GetObjectRefType(env, localFromWeak) == JNILocalRefType &&
        (*env)->GetObjectRefType(env, localFromGlobal) == JNILocalRefType;

    jboolean sameOk =
        (*env)->IsSameObject(env, weak, local) &&
        (*env)->IsSameObject(env, globalFromWeak, local) &&
        (*env)->IsSameObject(env, weakFromWeak, local) &&
        (*env)->IsSameObject(env, localFromWeak, local) &&
        (*env)->IsSameObject(env, localFromGlobal, local);

    (*env)->DeleteGlobalRef(env, global);
    (*env)->DeleteGlobalRef(env, globalFromWeak);
    (*env)->DeleteWeakGlobalRef(env, weak);
    (*env)->DeleteWeakGlobalRef(env, weakFromWeak);

    CHECK(typesOk);
    CHECK(sameOk);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_deleteLocalRefFromOuterFrame(JNIEnv* env, jobject self)
{
    jobject outer = (*env)->NewIntArray(env, 1);
    jobject kept = (*env)->NewIntArray(env, 7);

    CHECK((*env)->PushLocalFrame(env, 4) == JNI_OK);
    (*env)->DeleteLocalRef(env, outer);
    jobject result = (*env)->PopLocalFrame(env, (*env)->NewIntArray(env, 2));

    CHECK(result != NULL);
    CHECK((*env)->GetArrayLength(env, result) == 2);
    CHECK((*env)->GetArrayLength(env, kept) == 7);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_popLocalFrameWithGlobalResult(JNIEnv* env, jobject self)
{
    jobject global = (*env)->NewGlobalRef(env, (*env)->NewIntArray(env, 3));

    CHECK((*env)->PushLocalFrame(env, 4) == JNI_OK);
    jobject result = (*env)->PopLocalFrame(env, global);
    jboolean same = (*env)->IsSameObject(env, result, global);
    (*env)->DeleteGlobalRef(env, global);

    CHECK(result != NULL);
    CHECK(same);
    CHECK((*env)->GetObjectRefType(env, result) == JNILocalRefType);
    CHECK((*env)->GetArrayLength(env, result) == 3);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_pushLocalFrameWithLargeAndZeroCapacity(JNIEnv* env, jobject self)
{
    jint capacities[] = { 0, 1, 100000 };
    for (size_t c = 0; c < sizeof(capacities) / sizeof(capacities[0]); c++)
    {
        CHECK((*env)->PushLocalFrame(env, capacities[c]) == JNI_OK);

        jobject refs[50];
        for (int i = 0; i < 50; i++)
            refs[i] = (*env)->NewIntArray(env, i);
        for (int i = 0; i < 50; i++)
            CHECK((*env)->GetArrayLength(env, refs[i]) == i);

        (*env)->PopLocalFrame(env, NULL);
    }

    NO_EXCEPTION();
}

// more arrays

#define CHECK_EMPTY_ELEMENTS(Type, jarr) \
    do \
    { \
        jarr a = (*env)->New##Type##Array(env, 0); \
        CHECK(a != NULL); \
        void* elems = (*env)->Get##Type##ArrayElements(env, a, NULL); \
        CHECK(elems != NULL); \
        (*env)->Release##Type##ArrayElements(env, a, elems, 0); \
        NO_EXCEPTION(); \
    } while (0)

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_emptyArrayElements(JNIEnv* env, jobject self)
{
    // NULL means failure, so even an empty array gets a pointer
    CHECK_EMPTY_ELEMENTS(Boolean, jbooleanArray);
    CHECK_EMPTY_ELEMENTS(Byte, jbyteArray);
    CHECK_EMPTY_ELEMENTS(Char, jcharArray);
    CHECK_EMPTY_ELEMENTS(Short, jshortArray);
    CHECK_EMPTY_ELEMENTS(Int, jintArray);
    CHECK_EMPTY_ELEMENTS(Long, jlongArray);
    CHECK_EMPTY_ELEMENTS(Float, jfloatArray);
    CHECK_EMPTY_ELEMENTS(Double, jdoubleArray);
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_arraysOfArrays(JNIEnv* env, jobject self)
{
    jobjectArray a = (*env)->NewObjectArray(env, 2, (*env)->FindClass(env, "[I"), NULL);
    CHECK(a != NULL);
    CHECK((*env)->IsSameObject(env, (*env)->GetObjectClass(env, a), (*env)->FindClass(env, "[[I")));

    (*env)->SetObjectArrayElement(env, a, 0, (*env)->NewIntArray(env, 3));
    CHECK((*env)->GetArrayLength(env, (*env)->GetObjectArrayElement(env, a, 0)) == 3);
    CHECK((*env)->GetObjectArrayElement(env, a, 1) == NULL);

    // an int[][] doesn't take a long[]
    (*env)->SetObjectArrayElement(env, a, 1, (*env)->NewLongArray(env, 1));
    EXPECT_EXCEPTION("java/lang/ArrayStoreException");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_objectArrayCovariance(JNIEnv* env, jobject self)
{
    jobjectArray targets = (*env)->NewObjectArray(env, 1, (*env)->FindClass(env, TARGET_CLASS), NULL);
    (*env)->SetObjectArrayElement(env, targets, 0, newTarget(env, SUB_TARGET_CLASS));
    NO_EXCEPTION();
    CHECK((*env)->IsInstanceOf(env, (*env)->GetObjectArrayElement(env, targets, 0), (*env)->FindClass(env, SUB_TARGET_CLASS)));

    jobjectArray sequences = (*env)->NewObjectArray(env, 2, (*env)->FindClass(env, "java/lang/CharSequence"), (*env)->NewStringUTF(env, "x"));
    CHECK(sequences != NULL);
    CHECK(stringEquals(env, (jstring)(*env)->GetObjectArrayElement(env, sequences, 1), "x"));

    (*env)->SetObjectArrayElement(env, targets, 0, (*env)->NewStringUTF(env, "not a target"));
    EXPECT_EXCEPTION("java/lang/ArrayStoreException");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_ghostInterfaceArrayStoreCheck(JNIEnv* env, jobject self)
{
    // CharSequence is implemented by String only on the Java side, so an array of it needs its own store check
    jobjectArray sequences = (*env)->NewObjectArray(env, 1, (*env)->FindClass(env, "java/lang/CharSequence"), NULL);
    jclass integerClass = (*env)->FindClass(env, "java/lang/Integer");
    jmethodID valueOf = (*env)->GetStaticMethodID(env, integerClass, "valueOf", "(I)Ljava/lang/Integer;");
    (*env)->SetObjectArrayElement(env, sequences, 0, (*env)->CallStaticObjectMethod(env, integerClass, valueOf, 1));
    EXPECT_EXCEPTION("java/lang/ArrayStoreException");
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_primitiveArrayCriticalOfWideTypes(JNIEnv* env, jobject self, jlongArray l, jdoubleArray d)
{
    jsize lLength = (*env)->GetArrayLength(env, l);
    jsize dLength = (*env)->GetArrayLength(env, d);

    jlong* lElems = (jlong*)(*env)->GetPrimitiveArrayCritical(env, l, NULL);
    jdouble* dElems = (jdouble*)(*env)->GetPrimitiveArrayCritical(env, d, NULL);
    if (lElems == NULL || dElems == NULL)
    {
        if (dElems != NULL)
            (*env)->ReleasePrimitiveArrayCritical(env, d, dElems, 0);
        if (lElems != NULL)
            (*env)->ReleasePrimitiveArrayCritical(env, l, lElems, 0);
        FAIL("GetPrimitiveArrayCritical returned NULL");
        return;
    }

    for (jsize i = 0; i < lLength; i++)
        lElems[i] += 0x100000000LL;
    for (jsize i = 0; i < dLength; i++)
        dElems[i] += 0.5;

    (*env)->ReleasePrimitiveArrayCritical(env, d, dElems, 0);
    (*env)->ReleasePrimitiveArrayCritical(env, l, lElems, 0);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_emptyStrings(JNIEnv* env, jobject self)
{
    jstring s = (*env)->NewStringUTF(env, "");
    CHECK(s != NULL);
    CHECK((*env)->GetStringLength(env, s) == 0);
    CHECK((*env)->GetStringUTFLength(env, s) == 0);

    const char* utf = (*env)->GetStringUTFChars(env, s, NULL);
    CHECK(utf != NULL);
    CHECK(utf[0] == 0);
    (*env)->ReleaseStringUTFChars(env, s, utf);

    const jchar* chars = (*env)->GetStringChars(env, s, NULL);
    CHECK(chars != NULL);
    (*env)->ReleaseStringChars(env, s, chars);

    jchar buf[1] = { 0 };
    jstring fromChars = (*env)->NewString(env, buf, 0);
    CHECK(fromChars != NULL);
    CHECK((*env)->GetStringLength(env, fromChars) == 0);

    (*env)->GetStringRegion(env, s, 0, 0, buf);
    NO_EXCEPTION();
}

// more natives

static jint JNICALL firstImpl(JNIEnv* env, jclass clazz, jint value)
{
    return value + 100;
}

static jint JNICALL secondImpl(JNIEnv* env, jobject self, jint value)
{
    return value + 200;
}

static jint JNICALL reRegisteredImpl(JNIEnv* env, jclass clazz)
{
    return 2;
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_registerMultipleNatives(JNIEnv* env, jobject self)
{
    jclass c = (*env)->FindClass(env, TESTS_CLASS "$MultiRegisteredNatives");
    JNINativeMethod methods[] =
    {
        { "first", "(I)I", (void*)firstImpl },
        { "second", "(I)I", (void*)secondImpl },
    };
    CHECK((*env)->RegisterNatives(env, c, methods, 2) == JNI_OK);

    jmethodID first = (*env)->GetStaticMethodID(env, c, "first", "(I)I");
    CHECK((*env)->CallStaticIntMethod(env, c, first, 1) == 101);

    jmethodID second = (*env)->GetMethodID(env, c, "second", "(I)I");
    CHECK((*env)->CallIntMethod(env, newTarget(env, TESTS_CLASS "$MultiRegisteredNatives"), second, 2) == 202);
    NO_EXCEPTION();
}

// exported, so that the method first binds to this
JNIEXPORT jint JNICALL Java_ikvm_tests_jni_JniEnvTests_00024ReRegisteredNatives_value(JNIEnv* env, jclass clazz)
{
    return 1;
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_reRegisterNatives(JNIEnv* env, jobject self)
{
    jclass c = (*env)->FindClass(env, TESTS_CLASS "$ReRegisteredNatives");
    jmethodID m = (*env)->GetStaticMethodID(env, c, "value", "()I");
    CHECK((*env)->CallStaticIntMethod(env, c, m) == 1);

    // registering replaces the binding that was already made
    JNINativeMethod method = { "value", "()I", (void*)reRegisteredImpl };
    CHECK((*env)->RegisterNatives(env, c, &method, 1) == JNI_OK);
    CHECK((*env)->CallStaticIntMethod(env, c, m) == 2);
    NO_EXCEPTION();
}

// more monitors

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_monitorEnterThenSynchronizedJava(JNIEnv* env, jobject self, jobject o)
{
    // a monitor entered through JNI is the same one Java code synchronizes on
    jclass c = (*env)->FindClass(env, TESTS_CLASS);
    jmethodID m = (*env)->GetStaticMethodID(env, c, "holdsLockWhileSynchronized", "(Ljava/lang/Object;)Z");

    CHECK((*env)->MonitorEnter(env, o) == JNI_OK);
    CHECK((*env)->CallStaticBooleanMethod(env, c, m, o) == JNI_TRUE);
    CHECK(holdsLock(env, o) == JNI_TRUE);
    CHECK((*env)->MonitorExit(env, o) == JNI_OK);
    CHECK(holdsLock(env, o) == JNI_FALSE);
    NO_EXCEPTION();
}

// more direct buffers

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_directBufferFromJava(JNIEnv* env, jobject self, jobject buffer)
{
    jbyte* address = (jbyte*)(*env)->GetDirectBufferAddress(env, buffer);
    CHECK(address != NULL);
    CHECK((*env)->GetDirectBufferCapacity(env, buffer) == 32);

    address[0] = 9;
    jmethodID get = (*env)->GetMethodID(env, (*env)->FindClass(env, "java/nio/ByteBuffer"), "get", "(I)B");
    CHECK((*env)->CallByteMethod(env, buffer, get, 0) == 9);
    NO_EXCEPTION();
}

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_zeroCapacityDirectBuffer(JNIEnv* env, jobject self)
{
    static jbyte memory[1];
    jobject buffer = (*env)->NewDirectByteBuffer(env, memory, 0);
    CHECK(buffer != NULL);
    CHECK((*env)->GetDirectBufferCapacity(env, buffer) == 0);
    NO_EXCEPTION();
}

// more virtual machine

extern JavaVM* onLoadVM;

JNIEXPORT void JNICALL Java_ikvm_tests_jni_JniEnvTests_javaVMFromOnLoad(JNIEnv* env, jobject self)
{
    JavaVM* vm = NULL;
    CHECK((*env)->GetJavaVM(env, &vm) == JNI_OK);
    CHECK(onLoadVM != NULL);
    CHECK(onLoadVM == vm);
}
