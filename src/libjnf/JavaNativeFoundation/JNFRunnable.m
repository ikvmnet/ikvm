/*
 * Copyright (c) 2009-2020 Apple Inc. All rights reserved.
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

#import "JNFRunnable.h"
#import "JNFThread.h"
#import "JNFJObjectWrapper.h"


static JNF_CLASS_CACHE(jc_Runnable, "java/lang/Runnable");
static JNF_MEMBER_CACHE(jm_run, jc_Runnable, "run", "()V");

@interface JNFRunnableWrapper : JNFJObjectWrapper { }
- (void) invokeRunnable;
@end

@implementation JNFRunnableWrapper

- (void) invokeRunnable {
    JNFThreadContext ctx = JNFThreadDetachOnThreadDeath | JNFThreadSetSystemClassLoaderOnAttach | JNFThreadAttachAsDaemon;
    JNIEnv *env = JNFObtainEnv(&ctx);
    JNFCallVoidMethod(env, [self jObjectWithEnv:env], jm_run);
    JNFReleaseEnv(env, &ctx);
}

@end


@implementation JNFRunnable

+ (NSInvocation *) invocationWithRunnable:(jobject)runnable withEnv:(JNIEnv *)env {
    SEL sel = @selector(invokeRunnable);
    NSMethodSignature *sig = [JNFRunnableWrapper instanceMethodSignatureForSelector:sel];
    NSInvocation *invocation = [NSInvocation invocationWithMethodSignature:sig];
    [invocation retainArguments];
    [invocation setSelector:sel];

    JNFRunnableWrapper *runnableWrapper = [[JNFRunnableWrapper alloc] initWithJObject:runnable withEnv:env];
    [invocation setTarget:runnableWrapper];
    [runnableWrapper release];

    return invocation;
}

#if __BLOCKS__
+ (void(^)(void)) blockWithRunnable:(jobject)runnable withEnv:(JNIEnv *)env {
    JNFJObjectWrapper *runnableWrapper = [JNFJObjectWrapper wrapperWithJObject:runnable withEnv:env];

    return [[^() {
        JNFThreadContext ctx = JNFThreadDetachOnThreadDeath | JNFThreadSetSystemClassLoaderOnAttach | JNFThreadAttachAsDaemon;
        JNIEnv *_block_local_env = JNFObtainEnv(&ctx);
        JNFCallVoidMethod(env, [runnableWrapper jObjectWithEnv:_block_local_env], jm_run);
        JNFReleaseEnv(_block_local_env, &ctx);
    } copy] autorelease];
}
#endif

@end
