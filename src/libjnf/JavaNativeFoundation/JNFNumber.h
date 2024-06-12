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
 * Functions that convert between number container classes.
 */

#import <Foundation/NSValue.h>

#import <JavaNativeFoundation/JNFJNI.h>

__BEGIN_DECLS

/*
 * Converts java.lang.Number to an NSNumber
 *  NOTE: Return value is auto-released, so if you need to hang on to it, you should retain it.
 */
JNF_EXPORT extern NSNumber *JNFJavaToNSNumber(JNIEnv *env, jobject n);

/*
 * Converts an NSNumber to a java.lang.Number
 * Only returns java.lang.Longs or java.lang.Doubles.
 *  NOTE: This returns a JNI Local Ref. Any code that calls must call DeleteLocalRef with the return value.
 */
JNF_EXPORT extern jobject JNFNSToJavaNumber(JNIEnv *env, NSNumber *n);

/*
 * Converts a java.lang.Boolean constants to the CFBooleanRef constants
 */
JNF_EXPORT extern CFBooleanRef JNFJavaToCFBoolean(JNIEnv* env, jobject b);

/*
 * Converts a CFBooleanRef constants to the java.lang.Boolean constants
 */
JNF_EXPORT extern jobject JNFCFToJavaBoolean(JNIEnv *env, CFBooleanRef b);

__END_DECLS
