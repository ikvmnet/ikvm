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
 * Assertions used by the JNF_COCOA_ENTER()/JNF_COCOA_EXIT() and class
 * caching macros. When building debug builds, improper use of the caching
 * macros will trigger warnings output to the console.
 */

#import <JavaNativeFoundation/JNFJNI.h>

#ifdef DEBUG
#define JAVA_ASSERTIONS_ON
#endif /* DEBUG */

// Use the WARN macro to send a message to stderr in the
// debug build.  It gets removed from the optimized build
// during preprocessing.
#ifdef DEBUG
#define JNF_WARN JNFDebugWarning
#else
#define JNF_WARN if (0) JNFDebugWarning
#endif /* DEBUG */

__BEGIN_DECLS

JNF_EXPORT extern void JNFDebugWarning(const char *fmt, ...);

JNF_EXPORT extern void JNFAssertionFailure(const char *file, int line, const char *condition, const char *msg);

#ifdef JAVA_ASSERTIONS_ON

#define JNF_ASSERT_FAILURE(condition, msg)					\
    JNFAssertionFailure(__FILE__, __LINE__, condition, msg)	\


#define JNF_ASSERT_MSG(condition, msg)						\
do {														\
    if (!(condition)) {										\
        JNF_ASSERT_FAILURE(#condition, msg);				\
    }														\
} while(0)													\


#define JNF_ASSERT_COND(condition)							\
    JNF_ASSERT_MSG(condition, NULL)							\


#define JNF_EXCEPTION_WARN(env, msg)						\
do {														\
    (*(env))->ExceptionDescribe(env);						\
    JNF_ASSERT_FAILURE("Java exception thrown", msg);		\
} while (0)													\


#define JNF_ASSERT_NO_EXCEPTION_MSG(env, msg)				\
if ((*(env))->ExceptionOccurred(env)) {						\
    JNF_EXCEPTION_WARN(env, msg);							\
}															\


#define JNF_ASSERT_NO_EXCEPTION(env)						\
    JNF_ASSERT_NO_EXCEPTION_MSG(env, NULL)					\

#else

#define JNF_ASSERT_COND(condition)
#define JNF_ASSERT_MSG(condition, msg)
#define JNF_EXCEPTION_WARN(env, msg)
#define JNF_ASSERT_NO_EXCEPTION(env)
#define JNF_ASSERT_NO_EXCEPTION_MSG(env, msg)

#endif /* JAVA_ASSERTIONS_ON */

JNF_EXPORT extern void JNFDumpJavaStack(JNIEnv *env);

__END_DECLS
