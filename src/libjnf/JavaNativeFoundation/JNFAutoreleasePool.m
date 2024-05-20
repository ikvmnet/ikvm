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
 * The JNFAutoreleasePool manages setting up and tearing down autorelease
 * pools for Java calls into the Cocoa frameworks.
 *
 * The external entry point into this machinery is JNFMethodEnter() and JNFMethodExit().
 */

#import "JNFAutoreleasePool.h"

#import <mach/mach_time.h>

// These are vended by the Objective-C runtime, but they are unfortunately
// not available as API in the macOS SDK.  We are following suit with swift
// and clang in declaring them inline here.  They canot be removed or changed
// in the OS without major bincompat ramifications.
//
// These were added in macOS 10.7.
void * _Nonnull objc_autoreleasePoolPush(void);
void objc_autoreleasePoolPop(void * _Nonnull context);

#if TIMED
static int64_t elapsedTime = 0;
#endif

#pragma mark -
#pragma mark External API

// JNFNativeMethodEnter - called on entry to each native method
//
// It sets up an autorelease pool, and will return a token if
// JNFNativeMethodExit should be called. It attempts to consider
// how much time has elapsed since the last autorelease pop.

JNFAutoreleasePoolToken *JNFNativeMethodEnter() {
#if TIMED
    int64_t start = mach_absolute_time();
#endif

    JNFAutoreleasePoolToken * const tokenToReturn = objc_autoreleasePoolPush();

#if TIMED
    elapsedTime += (mach_absolute_time() - start);
#endif

    return tokenToReturn;
}


// JNFNativeMethodExit - called on exit from native methods
//
// This method is only called on exit from the first
// native method to appear in the execution stack.
// This function does not need to be called on exit
// from the inner native methods (as an optimization).
// JNFNativeMethodEnter sets the token to non-nil if
// JNFNativeMethodExit needs to be called on exit.

void JNFNativeMethodExit(JNFAutoreleasePoolToken *token) {

#if TIMED
    int64_t start = mach_absolute_time();
#endif

    objc_autoreleasePoolPop(token);

#if TIMED
    elapsedTime += (mach_absolute_time() - start);

    NSLog(@"elapsedTime: %llu", elapsedTime);
    //	elapsedTime = 0;
#endif
}
