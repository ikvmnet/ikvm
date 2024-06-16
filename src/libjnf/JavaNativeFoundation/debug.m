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

#import "debug.h"

#import "JNFAssert.h"
#import "JNFObject.h"

/*
 * Utility function to print the Java stack backtrace.
 * In gdb, if you have a value for the JNIEnv on the thread
 * you want to trace, you can do a gdb "print" of this function
 * to get the stack trace for the thread.
 */
void JNFJavaStackTrace(JNIEnv *env) {
    jthrowable obj_javaException;
    if ((obj_javaException = (*env)->ExceptionOccurred(env)) != NULL) (*env)->ExceptionClear(env);

    jclass cls_Thread = (*env)->FindClass(env, "java/lang/Thread");
    jmethodID mid_currentThread = (*env)->GetStaticMethodID(env, cls_Thread, "currentThread", "()Ljava/lang/Thread;");
    jobject obj_currentThread = (*env)->CallStaticObjectMethod(env, cls_Thread, mid_currentThread);
    jclass cls_currentThread = (*env)->GetObjectClass(env, obj_currentThread);
    jmethodID mid_getName = (*env)->GetMethodID(env, cls_currentThread, "getName", "()Ljava/lang/String;");
    jobject obj_threadName = (*env)->CallObjectMethod(env, obj_currentThread, mid_getName);
    (*env)->DeleteLocalRef(env, obj_currentThread);

    const char *threadName = (*env)->GetStringUTFChars(env, obj_threadName, NULL);
    JNF_WARN("Stack trace from Java thread \"%s\":", threadName);
    (*env)->ReleaseStringUTFChars(env, obj_threadName, threadName);
    (*env)->DeleteLocalRef(env, obj_threadName);

    jmethodID mid_dumpStack = (*env)->GetStaticMethodID(env, cls_Thread, "dumpStack", "()V");
    (*env)->CallStaticVoidMethod(env, cls_Thread, mid_dumpStack);
    (*env)->DeleteLocalRef(env, cls_Thread);

    if (obj_javaException) (*env)->Throw(env, obj_javaException);
}

/*
 * Utility function to dump some info about a generic Java object.
 * To be called from gdb.  Like JNFJavaStackTrace, you need to have
 * a valid value for the JNIEnv to call this function.
 */
void JNFDumpJavaObject(JNIEnv *env, jobject obj) {
    jthrowable obj_javaException;
    if ((obj_javaException = (*env)->ExceptionOccurred(env)) != NULL) (*env)->ExceptionClear(env);

    jclass cls_CToolkit = (*env)->FindClass(env, "apple/awt/CToolkit");
    jmethodID mid_dumpObject = (*env)->GetStaticMethodID(env, cls_CToolkit, "dumpObject", "(Ljava/lang/Object;)V");
    (*env)->CallStaticVoidMethod(env, cls_CToolkit, mid_dumpObject, obj);
    (*env)->DeleteLocalRef(env, cls_CToolkit);

    if (obj_javaException) (*env)->Throw(env, obj_javaException);
}

/*
 * Utility function to print a Java stack trace into a string
 */
NSString *JNFGetStackTraceAsNSString(JNIEnv *env, jthrowable throwable) {
    // Writer writer = new StringWriter();
    JNF_CLASS_CACHE(jc_StringWriter, "java/io/StringWriter");
    JNF_CTOR_CACHE(jct_StringWriter, jc_StringWriter, "()V");
    jobject writer = JNFNewObject(env, jct_StringWriter);

    // PrintWriter printWriter = new PrintWriter(writer);
    JNF_CLASS_CACHE(jc_PrintWriter, "java/io/PrintWriter");
    JNF_CTOR_CACHE(jct_PrintWriter, jc_PrintWriter, "(Ljava/io/Writer;)V");
    jobject printWriter = JNFNewObject(env, jct_PrintWriter, writer);

    // throwable.printStackTrace(printWriter);
    JNF_CLASS_CACHE(jc_Throwable, "java/lang/Throwable");
    JNF_MEMBER_CACHE(jm_printStackTrace, jc_Throwable, "printStackTrace", "(Ljava/io/PrintWriter;)V");
    JNFCallVoidMethod(env, throwable, jm_printStackTrace, printWriter);
    (*env)->DeleteLocalRef(env, printWriter);

    // return writer.toString();
    NSString *stackTraceAsString = JNFObjectToString(env, writer);
    (*env)->DeleteLocalRef(env, writer);
    return stackTraceAsString;
}
