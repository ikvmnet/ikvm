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
 * Functions that create NSStrings, UTF16 unichars, or UTF8 chars from java.lang.Strings
 */

#import <Foundation/NSString.h>

#import <JavaNativeFoundation/JNFJNI.h>

__BEGIN_DECLS

// Returns an NSString given a java.lang.String object
//	NOTE: Return value is auto-released, so if you need to hang on to it, you should retain it.
JNF_EXPORT extern NSString *JNFJavaToNSString(JNIEnv *env, jstring javaString);

// Returns a java.lang.String object as a JNI local ref.
//	NOTE: This returns a JNI Local Ref. Any code that calls this should call DeleteLocalRef with the return value.
JNF_EXPORT extern jstring JNFNSToJavaString(JNIEnv *env, NSString *nsString);

/*
 * Gets UTF16 unichars from a Java string, and checks for errors and raises a JNFException if
 * the unichars cannot be obtained from Java.
 */
JNF_EXPORT extern const unichar *JNFGetStringUTF16UniChars(JNIEnv *env, jstring javaString);

/*
 * Releases the unichars obtained from JNFGetStringUTF16UniChars()
 */
JNF_EXPORT extern void JNFReleaseStringUTF16UniChars(JNIEnv *env, jstring javaString, const unichar *unichars);

/*
 * Gets UTF8 chars from a Java string, and checks for errors and raises a JNFException if
 * the chars cannot be obtained from Java.
 */
JNF_EXPORT extern const char *JNFGetStringUTF8Chars(JNIEnv *env, jstring javaString);

/*
 * Releases the chars obtained from JNFGetStringUTF8Chars()
 */
JNF_EXPORT extern void JNFReleaseStringUTF8Chars(JNIEnv *env, jstring javaString, const char *chars);

__END_DECLS
