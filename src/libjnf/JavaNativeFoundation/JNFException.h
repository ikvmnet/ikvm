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
 *
 * --
 *
 * JNFExceptions handle bridging exceptions between Foundation and Java. NSExceptions are
 * caught by the JNF_COCOA_ENTER()/JNF_COCOA_EXIT() macros, and transformed and thrown as
 * Java exceptions at the JNI boundry. The macros in JNFJNI.h also check for Java exceptions
 * and rethrow them as NSExceptions until they hit an @try/@catch block, or the
 * JNF_COCOA_ENTER()/JNF_COCOA_EXIT() macros.
 */

#import <Foundation/Foundation.h>

#import <JavaNativeFoundation/JNFJNI.h>

__BEGIN_DECLS

// Some exception class names.
// These strings contain the full class name of each Java exception, so
// they are handy to use when you need to throw an exception.
JNF_EXPORT extern const char *kOutOfMemoryError;
JNF_EXPORT extern const char *kClassNotFoundException;
JNF_EXPORT extern const char *kNullPointerException;
JNF_EXPORT extern const char *kIllegalAccessException;
JNF_EXPORT extern const char *kIllegalArgumentException;
JNF_EXPORT extern const char *kNoSuchFieldException;
JNF_EXPORT extern const char *kNoSuchMethodException;
JNF_EXPORT extern const char *kRuntimeException;

// JNFException - a subclass of NSException that wraps a Java exception
//
// When a java exception is thrown out to a native method, use +raiseUnnamedException:
// to turn it into an NSException.  When returning out of a native method in
// which an NSException has been raised, use the -raiseToJava: method to turn
// it back into a Java exception and "throw" it, in the Java sense.

JNF_EXPORT
@interface JNFException : NSException

+ (void)raiseUnnamedException:(JNIEnv *)env;
+ (void)raise:(JNIEnv *)env throwable:(jthrowable)throwable;
+ (void)raise:(JNIEnv *)env as:(const char *)javaExceptionType reason:(const char *)reasonMsg;

- init:(JNIEnv *)env throwable:(jthrowable)throwable;
- init:(JNIEnv *)env as:(const char *)javaExceptionType reason:(const char *)reasonMsg;

+ (void)throwToJava:(JNIEnv *)env exception:(NSException *)exception;
+ (void)throwToJava:(JNIEnv *)env exception:(NSException *)exception as:(const char *)javaExceptionType;

- (void)raiseToJava:(JNIEnv *)env;

@end

__END_DECLS
