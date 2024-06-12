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

#import "JNFJObjectWrapper.h"

#import "JNFJNI.h"
#import "JNFThread.h"

@interface JNFJObjectWrapper ()
@property (readwrite, nonatomic, assign) jobject jObject;
@end

@implementation JNFJObjectWrapper

- (jobject) _getWithEnv:(__unused JNIEnv *)env {
    return self.jObject;
}

- (jobject) _createObj:(jobject)jObjectIn withEnv:(JNIEnv *)env {
    return JNFNewGlobalRef(env, jObjectIn);
}

- (void) _destroyObj:(jobject)jObjectIn withEnv:(JNIEnv *)env {
    JNFDeleteGlobalRef(env, jObjectIn);
}

+ (JNFJObjectWrapper *) wrapperWithJObject:(jobject)jObjectIn withEnv:(JNIEnv *)env {
    return [[[JNFJObjectWrapper alloc] initWithJObject:jObjectIn withEnv:env] autorelease];
}

- (id) initWithJObject:(jobject)jObjectIn withEnv:(JNIEnv *)env {
    self = [super init];
    if (!self) return self;

    if (jObjectIn) {
        self.jObject = [self _createObj:jObjectIn withEnv:env];
    }

    return self;
}

- (jobject) jObjectWithEnv:(JNIEnv *)env {
    jobject validObj = [self _getWithEnv:env];
    if (!validObj) return NULL;

    return (*env)->NewLocalRef(env, validObj);
}

- (void) setJObject:(jobject)jObjectIn withEnv:(JNIEnv *)env {
    jobject const jobj = self.jObject;
    if (jobj == jObjectIn) return;

    if (jobj) {
        [self _destroyObj:jobj withEnv:env];
    }

    if (jObjectIn) {
        self.jObject = [self _createObj:jObjectIn withEnv:env];
    } else {
        self.jObject = NULL;
    }
}

- (void) clearJObjectReference {
    jobject const jobj = self.jObject;
    if (!jobj) return;

    JNFThreadContext threadContext = JNFThreadDetachImmediately;
    JNIEnv *env = JNFObtainEnv(&threadContext);
    if (env == NULL) return; // leak?

    [self _destroyObj:jobj withEnv:env];
    self.jObject = NULL;

    JNFReleaseEnv(env, &threadContext);
}

- (void) dealloc {
    [self clearJObjectReference];
    [super dealloc];
}

@end


@implementation JNFWeakJObjectWrapper

+ (JNFWeakJObjectWrapper *) wrapperWithJObject:(jobject)jObjectIn withEnv:(JNIEnv *)env {
    return [[[JNFWeakJObjectWrapper alloc] initWithJObject:jObjectIn withEnv:env] autorelease];
}

- (jobject) _getWithEnv:(JNIEnv *)env {
    jobject const jobj = self.jObject;

    if ((*env)->IsSameObject(env, jobj, NULL) == JNI_TRUE) {
        self.jObject = NULL; // object went invalid
        return NULL;
    }
    return jobj;
}

- (jobject) _createObj:(jobject)jObjectIn withEnv:(JNIEnv *)env {
    return JNFNewWeakGlobalRef(env, jObjectIn);
}

- (void) _destroyObj:(jobject)jObjectIn withEnv:(JNIEnv *)env {
    JNFDeleteWeakGlobalRef(env, jObjectIn);
}

@end
