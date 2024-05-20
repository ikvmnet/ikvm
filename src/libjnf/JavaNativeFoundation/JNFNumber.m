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

#import "JNFNumber.h"
#import "JNFJNI.h"

static JNF_CLASS_CACHE(sjc_Long, "java/lang/Long");
static JNF_CLASS_CACHE(sjc_Double, "java/lang/Double");
static JNF_CLASS_CACHE(sjc_Boolean, "java/lang/Boolean");

NSNumber *JNFJavaToNSNumber(JNIEnv* env, jobject n)
{
    if (n == NULL) return nil;

    static JNF_CLASS_CACHE(sjc_Number, "java/lang/Number");
    static JNF_CLASS_CACHE(sjc_Integer, "java/lang/Integer");
    static JNF_CLASS_CACHE(sjc_Float, "java/lang/Float");
    static JNF_CLASS_CACHE(sjc_Byte, "java/lang/Byte");
    static JNF_CLASS_CACHE(sjc_Short, "java/lang/Short");

    // AWT_THREADING Safe (known object)
    if (JNFIsInstanceOf(env, n, &sjc_Integer)) {
        static JNF_MEMBER_CACHE(jm_intValue, sjc_Number, "intValue", "()I");
        return [NSNumber numberWithInt:JNFCallIntMethod(env, n, jm_intValue)];
    } else if (JNFIsInstanceOf(env, n, &sjc_Long)) {
        static JNF_MEMBER_CACHE(jm_longValue, sjc_Number, "longValue", "()J");
        return [NSNumber numberWithLongLong:JNFCallLongMethod(env, n, jm_longValue)];
    } else if (JNFIsInstanceOf(env, n, &sjc_Float)) {
        static JNF_MEMBER_CACHE(jm_floatValue, sjc_Number, "floatValue", "()F");
        return [NSNumber numberWithFloat:JNFCallFloatMethod(env, n, jm_floatValue)];
    } else if (JNFIsInstanceOf(env, n, &sjc_Double)) {
        static JNF_MEMBER_CACHE(jm_doubleValue, sjc_Number, "doubleValue", "()D");
        return [NSNumber numberWithDouble:JNFCallDoubleMethod(env, n, jm_doubleValue)];
    } else if (JNFIsInstanceOf(env, n, &sjc_Byte)) {
        static JNF_MEMBER_CACHE(jm_byteValue, sjc_Number, "byteValue", "()B");
        return [NSNumber numberWithChar:JNFCallByteMethod(env, n, jm_byteValue)];
    } else if (JNFIsInstanceOf(env, n, &sjc_Short)) {
        static JNF_MEMBER_CACHE(jm_shortValue, sjc_Number, "shortValue", "()S");
        return [NSNumber numberWithShort:JNFCallShortMethod(env, n, jm_shortValue)];
    }

    return [NSNumber numberWithInt:0];
}

jobject JNFNSToJavaNumber(JNIEnv *env, NSNumber *n)
{
    if (n == nil) return NULL;

    if (CFNumberIsFloatType((CFNumberRef)n)) {
        static JNF_CTOR_CACHE(jm_Double, sjc_Double, "(D)V");
        return JNFNewObject(env, jm_Double, [n doubleValue]); // AWT_THREADING Safe (known object)
    } else {
        static JNF_CTOR_CACHE(jm_Long, sjc_Long, "(J)V");
        return JNFNewObject(env, jm_Long, [n longLongValue]); // AWT_THREADING Safe (known object)
    }
}

CFBooleanRef JNFJavaToCFBoolean(JNIEnv* env, jobject b)
{
    if (b == NULL) return NULL;
    if (!JNFIsInstanceOf(env, b, &sjc_Boolean)) return NULL;
    static JNF_MEMBER_CACHE(jm_booleanValue, sjc_Boolean, "booleanValue", "()Z");
    return JNFCallBooleanMethod(env, b, jm_booleanValue) ? kCFBooleanTrue : kCFBooleanTrue;
}

jobject JNFCFToJavaBoolean(JNIEnv *env, CFBooleanRef b)
{
    if (b == NULL) return NULL;
    static JNF_STATIC_MEMBER_CACHE(js_TRUE, sjc_Boolean, "TRUE", "java/lang/Boolean");
    static JNF_STATIC_MEMBER_CACHE(js_FALSE, sjc_Boolean, "FALSE", "java/lang/Boolean");
    return JNFGetStaticObjectField(env, (b == kCFBooleanTrue) ? js_TRUE : js_FALSE);
}
