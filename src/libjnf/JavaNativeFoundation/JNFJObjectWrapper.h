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
 * Simple wrapper classes to hold Java Objects in JNI global references.
 *
 * This is used to pass Java objects across thread boundries, often through
 * -performSelectorOnMainThread invocations. This wrapper properly creates a
 * new global ref, and clears it on -dealloc or -finalize, attaching to the
 * current VM, attaching the current thread if necessary, releasing the global
 * ref, and detaching the thread from the VM if it attached it.
 *
 * Destruction of this wrapper is expensive if the jobject has not been
 * pre-cleared, because it must re-attach to the JVM.
 *
 * The JNFWeakJObjectWrapper manages a weak global reference which may become
 * invalid if the JVM garbage collects the original object.
 */

#import <Foundation/Foundation.h>
#import <JavaNativeFoundation/JNFJNI.h>

__BEGIN_DECLS

JNF_EXPORT
@interface JNFJObjectWrapper : NSObject

+ (JNFJObjectWrapper *) wrapperWithJObject:(jobject)jObjectIn withEnv:(JNIEnv *)env;
- (id) initWithJObject:(jobject)jObjectIn withEnv:(JNIEnv *)env;
- (void) setJObject:(jobject)jObjectIn withEnv:(JNIEnv *)env; // clears any pre-existing global-ref
- (jobject) jObjectWithEnv:(JNIEnv *)env; // returns a new local-ref, must be released with DeleteLocalRef

@property (readonly, nonatomic, assign) jobject jObject;

@end


JNF_EXPORT
@interface JNFWeakJObjectWrapper : JNFJObjectWrapper { }

+ (JNFWeakJObjectWrapper *) wrapperWithJObject:(jobject)jObjectIn withEnv:(JNIEnv *)env;

@end

__END_DECLS
