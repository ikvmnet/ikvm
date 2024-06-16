/*
 * Copyright (c) 2008-2020 Apple Inc. All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 *   1. Redistributions of source code must retain the above copyright notice,
 *      this list of conditions and the following disclaimer.
 *
 *   2. Redistributions in binary form must reproduce the above copyright
 *      notice, this list of conditions and the following disclaimer in the
 *      documentation and/or other materials provided with the distribution.
 *
 *   3. Neither the name of the copyright holder nor the names of its
 *      contributors may be used to endorse or promote products derived from
 *      this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 */

#import "JNFJNI.h"

#import "JNFAssert.h"
#import "debug.h"

// constants ripped from jvm.h
#define JVM_SIGNATURE_ARRAY		'['
#define JVM_SIGNATURE_BYTE		'B'
#define JVM_SIGNATURE_CHAR		'C'
#define JVM_SIGNATURE_CLASS		'L'
#define JVM_SIGNATURE_ENDCLASS	';'
#define JVM_SIGNATURE_ENUM		'E'
#define JVM_SIGNATURE_FLOAT		'F'
#define JVM_SIGNATURE_DOUBLE    'D'
#define JVM_SIGNATURE_FUNC		'('
#define JVM_SIGNATURE_ENDFUNC	')'
#define JVM_SIGNATURE_INT		'I'
#define JVM_SIGNATURE_LONG		'J'
#define JVM_SIGNATURE_SHORT		'S'
#define JVM_SIGNATURE_VOID		'V'
#define JVM_SIGNATURE_BOOLEAN	'Z'

// IS_METHOD - Does the parameter point to a method member?
//
// This is used mostly in asserts.
#define IS_METHOD(member) (*(member->sig) == SIGNATURE_FUNC)

static inline BOOL isMethod(JNFMemberInfo *member) {
    return (*(member->sig)) == JVM_SIGNATURE_FUNC;
}

static void JNFLookupClass(JNIEnv *env, JNFClassInfo *class) {
    jclass localCls = NULL;
    JNF_ASSERT_COND(class);
    localCls = (*env)->FindClass(env, class->name);
    if (localCls == NULL) [JNFException raiseUnnamedException:env];

    class->cls = JNFNewGlobalRef(env, localCls);
    (*env)->DeleteLocalRef(env, localCls);
    if (class->cls == NULL) [JNFException raiseUnnamedException:env];
}

static void JNFLookupMemberID(JNIEnv *env, JNFMemberInfo *member) {
    JNF_ASSERT_COND(member);
    JNF_ASSERT_COND(member->classInfo);

    if (member->classInfo->cls == NULL) JNFLookupClass(env, member->classInfo);

    if (isMethod(member)) {
        member->j.methodID = member->isStatic ?
        (*env)->GetStaticMethodID(env, member->classInfo->cls, member->name, member->sig) :
        (*env)->GetMethodID(env, member->classInfo->cls, member->name, member->sig);
    } else {   // This member is a field
        member->j.fieldID = member->isStatic ?
        (*env)->GetStaticFieldID(env, member->classInfo->cls, member->name, member->sig) :
        (*env)->GetFieldID(env, member->classInfo->cls, member->name, member->sig);
    }

    // If NULL, then exception occurred
    if (member->j.methodID == NULL) [JNFException raiseUnnamedException:env];
}

BOOL JNFIsInstanceOf(JNIEnv *env, jobject obj, JNFClassInfo *clazz) {
    if (clazz->cls == NULL) JNFLookupClass(env, clazz);
    return (BOOL)(*env)->IsInstanceOf(env, obj, clazz->cls);
}

//
// A whole mess o' macros
//
// All of these macros provide caching of class refs and member IDs,
// which speeds all member accesses following the first.  In addition,
// Java exceptions are caught and turned into NSExceptions,
// so error handling is much easier on the native side.
//
// You can use these if you need them for optimization, but it's generally
// easier and better to use the utility functions in jni_utilities.h.

// LOOKUP_MEMBER_ID - Lookup a fieldID if needed
//
// Give this macro a JNFMemberInfo*, and it will
// lookup and cache the field ID as needed.
#define LOOKUP_MEMBER_ID(env, member) \
if (member->j.fieldID == NULL)    \
JNFLookupMemberID(env, member)

#ifdef RAWT_DEBUG
#define VERIFY_MEMBERS
#endif

#ifdef VERIFY_MEMBERS
#define VERIFY_FIELD(env, obj, field, sig)    VerifyMember(env, obj, field, sig, NO)
#define VERIFY_METHOD(env, obj, method, sig)  VerifyMember(env, obj, method, sig, YES)
#else
#define VERIFY_FIELD(env, obj, member, sig)
#define VERIFY_METHOD(env, obj, member, sig)
#endif /* RAWT_DEBUG */

// GET_FIELD - Safe way to get a java field
//
// This macro takes care of the caching and exception
// propgation.
#define GET_FIELD(env, obj, field, result, sig, jni_call)						\
    LOOKUP_MEMBER_ID(env, field);												\
    VERIFY_FIELD(env, obj, field, sig);											\
    result = (*env)->jni_call(env, obj, field->j.fieldID);						\
    JNF_CHECK_AND_RETHROW_EXCEPTION(env);

// GET_STATIC_FIELD
//
#define GET_STATIC_FIELD(env, field, result, sig, jni_call)						\
    LOOKUP_MEMBER_ID(env, field);												\
    VERIFY_FIELD(env, NULL, field, sig);										\
    result = (*env)->jni_call(env, field->classInfo->cls, field->j.fieldID);	\
    JNF_CHECK_AND_RETHROW_EXCEPTION(env);

// SET_FIELD - Safe way to set a java field
//
// Similar to GET_FIELD
#define SET_FIELD(env, obj, field, val, sig, jni_call)							\
    LOOKUP_MEMBER_ID(env, field);												\
    VERIFY_FIELD(env, obj, field, sig);											\
    (*env)->jni_call(env, obj, field->j.fieldID, val);							\
    JNF_CHECK_AND_RETHROW_EXCEPTION(env);

// SET_STATIC_FIELD
//
#define SET_STATIC_FIELD(env, field, val, sig, jni_call)						\
    LOOKUP_MEMBER_ID(env, field);												\
    VERIFY_FIELD(env, NULL, field, sig);										\
    (*env)->jni_call(env, field->classInfo->cls, field->j.fieldID, val);		\
    JNF_CHECK_AND_RETHROW_EXCEPTION(env);

// CALL_VOID_METHOD
//
// "args" is a va_list
#define CALL_VOID_METHOD(env, obj, method, jni_call, args)						\
    LOOKUP_MEMBER_ID(env, method);												\
    VERIFY_METHOD(env, obj, method, JVM_SIGNATURE_VOID);						\
    (*env)->jni_call(env, obj, method->j.methodID, args);						\
    JNF_CHECK_AND_RETHROW_EXCEPTION(env);

// CALL_STATIC_VOID_METHOD
//
// "args" is a va_list
#define CALL_STATIC_VOID_METHOD(env, method, jni_call, args)					\
    LOOKUP_MEMBER_ID(env, method);												\
    VERIFY_METHOD(env, NULL, method, JVM_SIGNATURE_VOID);						\
    (*env)->jni_call(env, method->classInfo->cls, method->j.methodID, args);	\
    JNF_CHECK_AND_RETHROW_EXCEPTION(env);

// CALL_METHOD - Call a method that returns a value
//
// "args" is a va_list
#define CALL_METHOD(env, obj, method, result, sig, jni_call, args )				\
    LOOKUP_MEMBER_ID(env, method);												\
    VERIFY_METHOD(env, obj, method, sig);										\
    result = (*env)->jni_call(env, obj, method->j.methodID, args);				\
    JNF_CHECK_AND_RETHROW_EXCEPTION(env);

// CALL_STATIC_METHOD - Call a static method that returns a value
//
// "args" is a va_list
#define CALL_STATIC_METHOD(env, method, result, sig, jni_call, args)							\
    LOOKUP_MEMBER_ID(env, method);																\
    VERIFY_METHOD(env, NULL, method, sig);														\
    result = (*env)->jni_call(env, method->classInfo->cls, method->j.methodID, args);			\
    JNF_CHECK_AND_RETHROW_EXCEPTION(env);


// NEW_OBJECT - Create instances
//
// "constructor" is a JNFMemberInfo* to a constructor method
// "args" is a va_list of arguments
#define NEW_OBJECT(env, constructor, obj, args)													\
    LOOKUP_MEMBER_ID(env, constructor);															\
    VERIFY_METHOD(env, NULL, constructor, JVM_SIGNATURE_VOID);									\
    obj = (*env)->NewObjectV(env, constructor->classInfo->cls, constructor->j.methodID, args);	\
    JNF_CHECK_AND_RETHROW_EXCEPTION(env);


//
// Non-static getters & setters
#define GET_FIELD_IMPLEMENTATION(type, sig, jni_call)						\
    type JNF ## jni_call(JNIEnv *env, jobject obj, JNFMemberInfo* field) {	\
        type result;															\
        JNF_ASSERT_COND(!field->isStatic);										\
        GET_FIELD(env, obj, field, result, sig, jni_call);						\
        return result;															\
    }

#define SET_FIELD_IMPLEMENTATION(type, sig, jni_call)								\
    void JNF ## jni_call(JNIEnv *env, jobject obj, JNFMemberInfo* field, type val) {\
        JNF_ASSERT_COND(!field->isStatic);												\
        SET_FIELD(env, obj, field, val, sig, jni_call);									\
    }

//
// Non-static getter implemenations
GET_FIELD_IMPLEMENTATION(jobject,  JVM_SIGNATURE_ENDCLASS, GetObjectField)
GET_FIELD_IMPLEMENTATION(jboolean, JVM_SIGNATURE_BOOLEAN,  GetBooleanField)
GET_FIELD_IMPLEMENTATION(jbyte,    JVM_SIGNATURE_BYTE,     GetByteField)
GET_FIELD_IMPLEMENTATION(jchar,    JVM_SIGNATURE_CHAR,     GetCharField)
GET_FIELD_IMPLEMENTATION(jshort,   JVM_SIGNATURE_SHORT,    GetShortField)
GET_FIELD_IMPLEMENTATION(jint,     JVM_SIGNATURE_INT,      GetIntField)
GET_FIELD_IMPLEMENTATION(jlong,    JVM_SIGNATURE_LONG,     GetLongField)
GET_FIELD_IMPLEMENTATION(jfloat,   JVM_SIGNATURE_FLOAT,    GetFloatField)
GET_FIELD_IMPLEMENTATION(jdouble,  JVM_SIGNATURE_DOUBLE,   GetDoubleField)

//
// Non-static setter implemenations
SET_FIELD_IMPLEMENTATION(jobject,  JVM_SIGNATURE_ENDCLASS, SetObjectField)
SET_FIELD_IMPLEMENTATION(jboolean, JVM_SIGNATURE_BOOLEAN,  SetBooleanField)
SET_FIELD_IMPLEMENTATION(jbyte,    JVM_SIGNATURE_BYTE,     SetByteField)
SET_FIELD_IMPLEMENTATION(jchar,    JVM_SIGNATURE_CHAR,     SetCharField)
SET_FIELD_IMPLEMENTATION(jshort,   JVM_SIGNATURE_SHORT,    SetShortField)
SET_FIELD_IMPLEMENTATION(jint,     JVM_SIGNATURE_INT,      SetIntField)
SET_FIELD_IMPLEMENTATION(jlong,    JVM_SIGNATURE_LONG,     SetLongField)
SET_FIELD_IMPLEMENTATION(jfloat,   JVM_SIGNATURE_FLOAT,    SetFloatField)
SET_FIELD_IMPLEMENTATION(jdouble,  JVM_SIGNATURE_DOUBLE,   SetDoubleField)

//
// Static getters & setters
#define GET_STATIC_FIELD_IMPLEMENTATION(type, sig, jni_call)		\
    type JNF ## jni_call(JNIEnv *env, JNFMemberInfo* field) {		\
        type result;													\
        JNF_ASSERT_COND(field->isStatic);								\
        GET_STATIC_FIELD(env, field, result, sig, jni_call);			\
        return result;													\
    }

#define SET_STATIC_FIELD_IMPLEMENTATION(type, sig, jni_call)			\
    void JNF ## jni_call(JNIEnv *env, JNFMemberInfo* field, type val) {	\
        JNF_ASSERT_COND(field->isStatic);									\
        SET_STATIC_FIELD(env, field, val, sig, jni_call);					\
    }

//
// Static getter implementations
GET_STATIC_FIELD_IMPLEMENTATION(jobject,  JVM_SIGNATURE_ENDCLASS, GetStaticObjectField)
GET_STATIC_FIELD_IMPLEMENTATION(jboolean, JVM_SIGNATURE_BOOLEAN,  GetStaticBooleanField)
GET_STATIC_FIELD_IMPLEMENTATION(jbyte,    JVM_SIGNATURE_BYTE,     GetStaticByteField)
GET_STATIC_FIELD_IMPLEMENTATION(jchar,    JVM_SIGNATURE_CHAR,     GetStaticCharField)
GET_STATIC_FIELD_IMPLEMENTATION(jshort,   JVM_SIGNATURE_SHORT,    GetStaticShortField)
GET_STATIC_FIELD_IMPLEMENTATION(jint,     JVM_SIGNATURE_INT,      GetStaticIntField)
GET_STATIC_FIELD_IMPLEMENTATION(jlong,    JVM_SIGNATURE_LONG,     GetStaticLongField)
GET_STATIC_FIELD_IMPLEMENTATION(jfloat,   JVM_SIGNATURE_FLOAT,    GetStaticFloatField)
GET_STATIC_FIELD_IMPLEMENTATION(jdouble,  JVM_SIGNATURE_DOUBLE,   GetStaticDoubleField)

//
// Static setter implemenations
SET_STATIC_FIELD_IMPLEMENTATION(jobject,  JVM_SIGNATURE_ENDCLASS, SetStaticObjectField)
SET_STATIC_FIELD_IMPLEMENTATION(jboolean, JVM_SIGNATURE_BOOLEAN,  SetStaticBooleanField)
SET_STATIC_FIELD_IMPLEMENTATION(jbyte,    JVM_SIGNATURE_BYTE,     SetStaticByteField)
SET_STATIC_FIELD_IMPLEMENTATION(jchar,    JVM_SIGNATURE_CHAR,     SetStaticCharField)
SET_STATIC_FIELD_IMPLEMENTATION(jshort,   JVM_SIGNATURE_SHORT,    SetStaticShortField)
SET_STATIC_FIELD_IMPLEMENTATION(jint,     JVM_SIGNATURE_INT,      SetStaticIntField)
SET_STATIC_FIELD_IMPLEMENTATION(jlong,    JVM_SIGNATURE_LONG,     SetStaticLongField)
SET_STATIC_FIELD_IMPLEMENTATION(jfloat,   JVM_SIGNATURE_FLOAT,    SetStaticFloatField)
SET_STATIC_FIELD_IMPLEMENTATION(jdouble,  JVM_SIGNATURE_DOUBLE,   SetStaticDoubleField)


//
// Calling instance methods
//
// FIX: if an exception is thrown, va_end is not called.
// On i386, va_end is a null macro.  Check on PPC to verify the same.
void JNFCallVoidMethod(JNIEnv *env, jobject obj, JNFMemberInfo* method, ...) {
    va_list args;
    JNF_ASSERT_COND(!method->isStatic);
    va_start(args, method);
    CALL_VOID_METHOD(env, obj, method, CallVoidMethodV, args);
    va_end(args);
}

#define CALL_METHOD_IMPLEMENTATION(type, sig, jni_call)							\
    type JNF ## jni_call(JNIEnv *env, jobject obj, JNFMemberInfo* method, ...) {\
        type result;																\
        va_list args;																\
        JNF_ASSERT_COND(!method->isStatic);											\
        va_start(args, method);														\
        CALL_METHOD(env, obj, method, result, sig, jni_call ## V, args);			\
        va_end(args);																\
        return result;																\
    }

CALL_METHOD_IMPLEMENTATION(jobject,  JVM_SIGNATURE_ENDCLASS, CallObjectMethod)
CALL_METHOD_IMPLEMENTATION(jboolean, JVM_SIGNATURE_BOOLEAN,  CallBooleanMethod)
CALL_METHOD_IMPLEMENTATION(jbyte,    JVM_SIGNATURE_BYTE,     CallByteMethod)
CALL_METHOD_IMPLEMENTATION(jchar,    JVM_SIGNATURE_CHAR,     CallCharMethod)
CALL_METHOD_IMPLEMENTATION(jshort,   JVM_SIGNATURE_SHORT,    CallShortMethod)
CALL_METHOD_IMPLEMENTATION(jint,     JVM_SIGNATURE_INT,      CallIntMethod)
CALL_METHOD_IMPLEMENTATION(jlong,    JVM_SIGNATURE_LONG,     CallLongMethod)
CALL_METHOD_IMPLEMENTATION(jfloat,   JVM_SIGNATURE_FLOAT,    CallFloatMethod)
CALL_METHOD_IMPLEMENTATION(jdouble,  JVM_SIGNATURE_DOUBLE,   CallDoubleMethod)

//
// Calling static methods
//
// FIX: if an exception is thrown, va_end is not called.
// On i386, va_end is a null macro.  Check on PPC to verify the same.
void JNFCallStaticVoidMethod(JNIEnv *env, JNFMemberInfo* method, ...) {
    va_list args;
    JNF_ASSERT_COND(method->isStatic);
    va_start(args, method);
    CALL_STATIC_VOID_METHOD(env, method, CallStaticVoidMethodV, args);
    va_end(args);
}

#define CALL_STATIC_METHOD_IMPLEMENTATION(type, sig, jni_call)				\
    type JNF ## jni_call(JNIEnv *env, JNFMemberInfo* method, ...) {			\
        type result;														\
        va_list args;														\
        JNF_ASSERT_COND(method->isStatic);									\
        va_start(args, method);												\
        CALL_STATIC_METHOD(env, method, result, sig, jni_call ## V, args);	\
        va_end(args);														\
        return result;														\
    }

CALL_STATIC_METHOD_IMPLEMENTATION(jobject,  JVM_SIGNATURE_ENDCLASS, CallStaticObjectMethod)
CALL_STATIC_METHOD_IMPLEMENTATION(jboolean, JVM_SIGNATURE_BOOLEAN,  CallStaticBooleanMethod)
CALL_STATIC_METHOD_IMPLEMENTATION(jbyte,    JVM_SIGNATURE_BYTE,     CallStaticByteMethod)
CALL_STATIC_METHOD_IMPLEMENTATION(jchar,    JVM_SIGNATURE_CHAR,     CallStaticCharMethod)
CALL_STATIC_METHOD_IMPLEMENTATION(jshort,   JVM_SIGNATURE_SHORT,    CallStaticShortMethod)
CALL_STATIC_METHOD_IMPLEMENTATION(jint,     JVM_SIGNATURE_INT,      CallStaticIntMethod)
CALL_STATIC_METHOD_IMPLEMENTATION(jlong,    JVM_SIGNATURE_LONG,     CallStaticLongMethod)
CALL_STATIC_METHOD_IMPLEMENTATION(jfloat,   JVM_SIGNATURE_FLOAT,    CallStaticFloatMethod)
CALL_STATIC_METHOD_IMPLEMENTATION(jdouble,  JVM_SIGNATURE_DOUBLE,   CallStaticDoubleMethod)

//
// Creating new object instances
jobject JNFNewObject(JNIEnv *env, JNFMemberInfo* constructor, ...)
{
    jobject newobj;
    va_list args;
    JNF_ASSERT_COND(!constructor->isStatic);
    va_start(args, constructor);
    NEW_OBJECT(env, constructor, newobj, args);
    va_end(args);
    return newobj;
}

jobjectArray JNFNewObjectArray(JNIEnv *env, JNFClassInfo *clazz, jsize length)
{
    if (clazz->cls == NULL) JNFLookupClass(env, clazz);
    JNF_ASSERT_COND(clazz->cls);
    jobjectArray newArray = (*env)->NewObjectArray(env, length, clazz->cls, NULL);
    JNF_CHECK_AND_RETHROW_EXCEPTION(env);
    return newArray;
}

#define NEW_PRIMITIVE_ARRAY(primitiveArrayType, methodName)				\
    primitiveArrayType JNF ## methodName(JNIEnv *env, jsize length) {	\
        primitiveArrayType array = (*env)->methodName(env, length);		\
        JNF_CHECK_AND_RETHROW_EXCEPTION(env);							\
        return array;													\
    }

NEW_PRIMITIVE_ARRAY(jbooleanArray,	NewBooleanArray)
NEW_PRIMITIVE_ARRAY(jbyteArray,		NewByteArray)
NEW_PRIMITIVE_ARRAY(jcharArray,		NewCharArray)
NEW_PRIMITIVE_ARRAY(jshortArray,	NewShortArray)
NEW_PRIMITIVE_ARRAY(jintArray,		NewIntArray)
NEW_PRIMITIVE_ARRAY(jlongArray,		NewLongArray)
NEW_PRIMITIVE_ARRAY(jfloatArray,	NewFloatArray)
NEW_PRIMITIVE_ARRAY(jdoubleArray,	NewDoubleArray)


// Class-related functions

// A bottleneck for creating global references
jobject JNFNewGlobalRef(JNIEnv *env, jobject obj)
{
    if (!obj) return NULL;

    jobject globalRef = (*env)->NewGlobalRef(env, obj);
    if (!globalRef) JNF_CHECK_AND_RETHROW_EXCEPTION(env);

    //	JNF_WARN("Created global ref %#08lx to object:", globalRef);
    //	JNFDumpJavaObject(env, globalRef);
    return globalRef;
}

// A bottleneck for deleting global references.
void JNFDeleteGlobalRef(JNIEnv *env, jobject globalRef)
{
    if (!globalRef) return;
    //	JNF_WARN("Deleting global ref %#08lx to object:", globalRef);
    //	JNFDumpJavaObject(env, globalRef);
    (*env)->DeleteGlobalRef(env, globalRef);
}

// A bottleneck for creating weak global references
jobject JNFNewWeakGlobalRef(JNIEnv *env, jobject obj)
{
    if (!obj) return NULL;

    jobject globalRef = (*env)->NewWeakGlobalRef(env, obj);
    if (!globalRef) JNF_CHECK_AND_RETHROW_EXCEPTION(env);

    //	JNF_WARN("Created global ref %#08lx to object:", globalRef);
    //	JNFDumpJavaObject(env, globalRef);
    return globalRef;
}

// A bottleneck for deleting weak global references.
void JNFDeleteWeakGlobalRef(JNIEnv *env, jobject globalRef)
{
    if (!globalRef) return;
    //	JNF_WARN("Deleting global ref %#08lx to object:", globalRef);
    //	JNFDumpJavaObject(env, globalRef);
    (*env)->DeleteWeakGlobalRef(env, globalRef);
}
