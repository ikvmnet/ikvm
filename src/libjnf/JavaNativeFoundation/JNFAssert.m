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

static void JNFDebugMessageV(const char *fmt, va_list args) {
    // Prints a message and breaks into debugger.
    fprintf(stderr, "JavaNativeFoundation: ");
    vfprintf(stderr, fmt, args);
    fprintf(stderr, "\n");
}

static void JNFDebugMessage(const char *fmt, ...) {
    // Takes printf args and then calls DebugBreak
    va_list args;
    va_start(args, fmt);
    JNFDebugMessageV(fmt, args);
    va_end(args);
}

void JNFDebugWarning(const char *fmt, ...) {
    // Takes printf args and then calls DebugBreak
    va_list args;
    va_start(args, fmt);
    JNFDebugMessageV(fmt, args);
    va_end(args);
}

void JNFAssertionFailure(const char *file, int line, const char *condition, const char *msg) {
    JNFDebugMessage("Assertion failure: %s", condition);
    if (msg) JNFDebugMessage(msg);
    JNFDebugMessage("File %s; Line %d", file, line);
}

void JNFDumpJavaStack(JNIEnv *env) {
    static JNF_CLASS_CACHE(jc_Thread, "java/lang/Thread");
    static JNF_STATIC_MEMBER_CACHE(jsm_Thread_dumpStack, jc_Thread, "dumpStack", "()V");
    JNFCallVoidMethod(env, jc_Thread.cls, jsm_Thread_dumpStack);
}
