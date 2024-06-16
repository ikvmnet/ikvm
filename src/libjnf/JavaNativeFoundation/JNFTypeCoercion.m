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

#import "JNFTypeCoercion.h"

#import "JNFJNI.h"
#import "JNFObject.h"
#import "JNFString.h"
#import "JNFNumber.h"
#import "JNFDate.h"
#import "JNFJObjectWrapper.h"

// #define DEBUG 1

@interface JNFInternalJavaClassToCoersionHolder : JNFJObjectWrapper

- (id) initWithCoercion:(NSObject <JNFTypeCoercion> *)coercionIn className:(NSString *)className withEnv:(JNIEnv *)env;
- (BOOL) isClassFor:(jobject)obj withEnv:(JNIEnv *)env;
- (NSObject <JNFTypeCoercion> *)coercion;

@property (nonatomic, readwrite, strong) NSObject <JNFTypeCoercion> *coercion;

@end

@interface JNFTypeCoercer ()

@property (nonatomic, readwrite, strong) NSObject <JNFTypeCoercion> *parent;
@property (nonatomic, readwrite, strong) NSMutableDictionary *nsClassToCoercion;
@property (nonatomic, readwrite, strong) NSMutableDictionary *javaClassNameToCoercion;
@property (nonatomic, readwrite, strong) NSArray *javaClassToCoercion;

@end

@implementation JNFTypeCoercer

- (id) init {
    return [self initWithParent:nil];
}

- (id) initWithParent:(NSObject <JNFTypeCoercion> *)parentIn {
    self = [super init];
    if (!self) return self;

    self.parent = parentIn;
    self.nsClassToCoercion = [NSMutableDictionary dictionary];
    self.javaClassNameToCoercion = [NSMutableDictionary dictionary];
    self.javaClassToCoercion = nil;

    return self;
}

- (void) dealloc {
    self.parent = nil;
    self.nsClassToCoercion = nil;
    self.javaClassNameToCoercion = nil;
    self.javaClassToCoercion = nil;

    [super dealloc];
}

- (JNFTypeCoercer *) deriveCoercer {
    return [[[JNFTypeCoercer alloc] initWithParent:self] autorelease];
}

- (void) addCoercion:(NSObject <JNFTypeCoercion> *)coercion forNSClass:(Class)nsClass javaClass:(NSString *)javaClassName {
    if (nsClass) [self.nsClassToCoercion setObject:coercion forKey:(id)nsClass];
    if (javaClassName) [self.javaClassNameToCoercion setObject:coercion forKey:javaClassName];
    self.javaClassToCoercion = nil; // invalidate Java Class cache
}

- (NSObject <JNFTypeCoercion> *) findCoercerForNSObject:(id)obj {
    NSMutableDictionary * const nsClassToCoercion = self.nsClassToCoercion;
    NSObject <JNFTypeCoercion> *coercer = [nsClassToCoercion objectForKey:[obj class]];
    if (coercer) return coercer;

    NSEnumerator *classes = [nsClassToCoercion keyEnumerator];
    Class clazzIter;
    while ((clazzIter = [classes nextObject]) != nil) {
        if ([obj isKindOfClass:clazzIter]) {
            return [nsClassToCoercion objectForKey:clazzIter];
        }
    }

    return self.parent;
}

- (jobject) coerceNSObject:(id)obj withEnv:(JNIEnv *)env usingCoercer:(JNFTypeCoercion *)coercer {
    return [[self findCoercerForNSObject:obj] coerceNSObject:obj withEnv:env usingCoercer:coercer];
}

- (jobject) coerceNSObject:(id)obj withEnv:(JNIEnv *)env {
    return [self coerceNSObject:obj withEnv:env usingCoercer:(JNFTypeCoercion *)self];
}


- (NSArray *) javaClassCacheUsingEnv:(JNIEnv *)env {
    NSArray * const javaClassToCoercion = self.javaClassToCoercion;
    if (javaClassToCoercion) return javaClassToCoercion;

    NSMutableArray *array = [[NSMutableArray alloc] init];

    NSMutableDictionary * const javaClassNameToCoercion = self.javaClassNameToCoercion;
    NSEnumerator *classNames = [javaClassNameToCoercion keyEnumerator];
    NSString *className;
    while ((className = [classNames nextObject]) != nil) {
        NSObject <JNFTypeCoercion> *coercion = [javaClassNameToCoercion objectForKey:className];
        JNFInternalJavaClassToCoersionHolder *holder = [[JNFInternalJavaClassToCoersionHolder alloc] initWithCoercion:coercion className:className withEnv:env];
        [array addObject:holder];
        [holder release];
    }

    self.javaClassToCoercion = array;
    return [array autorelease];
}

- (NSObject <JNFTypeCoercion> *) findCoercerForJavaObject:(jobject)obj withEnv:(JNIEnv *)env {
    if (obj == NULL) return nil;

    NSMutableDictionary * const javaClassNameToCoercion = self.javaClassNameToCoercion;
    NSString *javaClazzName = JNFObjectClassName(env, obj);
    NSObject <JNFTypeCoercion> *coercer = [javaClassNameToCoercion objectForKey:javaClazzName];
    if (coercer) return coercer;

#ifdef DEBUG
    NSLog(@"attempting to find coercer for: %@", javaClazzName);
#endif

    NSArray *javaClassCache = [self javaClassCacheUsingEnv:env];
    NSEnumerator *holderIter = [javaClassCache objectEnumerator];
    JNFInternalJavaClassToCoersionHolder *holder;
    while ((holder = [holderIter nextObject]) != nil) {
        if ([holder isClassFor:obj withEnv:env]) {
            NSObject <JNFTypeCoercion> *coercion = [holder coercion];
            [javaClassNameToCoercion setObject:coercion forKey:javaClazzName];
            return coercion;
        }
    }

    return self.parent;
}

- (id) coerceJavaObject:(jobject)obj withEnv:(JNIEnv *)env usingCoercer:(JNFTypeCoercion *)coercer {
    NSObject <JNFTypeCoercion> *coercion = [self findCoercerForJavaObject:obj withEnv:env];
    id nsObj = [coercion coerceJavaObject:obj withEnv:env usingCoercer:coercer];
    if (nsObj == nil) return [NSNull null];
    return nsObj;
}

- (id) coerceJavaObject:(jobject)obj withEnv:(JNIEnv *)env {
    return [self coerceJavaObject:obj withEnv:env usingCoercer:(JNFTypeCoercion *)self];
}

@end


@implementation JNFInternalJavaClassToCoersionHolder

- (id) initWithCoercion:(NSObject <JNFTypeCoercion> *)coercionIn className:(NSString *)className withEnv:(JNIEnv *)env {
    const char *classNameCStr = [className cStringUsingEncoding:NSUTF8StringEncoding];
    jclass clz = (*env)->FindClass(env, classNameCStr);
    if (clz == NULL) [JNFException raise:env as:kClassNotFoundException reason:"Unable to create type converter."];

    self = [super initWithJObject:clz withEnv:env];
    if (!self) return self;

    self.coercion = coercionIn;
    return self;
}

- (BOOL)isEqual:(id)object {
    jclass javaClazz = [self jObject];

    if ([object isKindOfClass:[self class]]) {
        return [((JNFInternalJavaClassToCoersionHolder *)object) jObject] == javaClazz;
    }

    return NO;
}

- (void) dealloc {
    self.coercion = nil;
    [super dealloc];
}

- (NSUInteger)hash {
    jclass javaClazz = [self jObject];
    return (NSUInteger)ptr_to_jlong(javaClazz);
}

- (BOOL) isClassFor:(jobject)obj withEnv:(JNIEnv *)env {
    if (obj == NULL) return JNI_FALSE;
#ifdef DEBUG
    NSLog(@"is (%@(%@)) a kind of (%@)?", JNFToString(env, obj), JNFClassName(env, obj), JNFToString(env, javaClazz));
#endif

    jclass javaClazz = [self jObject];
    return (BOOL)(*env)->IsInstanceOf(env, obj, javaClazz);
}

@end


@interface JNFStringCoercion : NSObject <JNFTypeCoercion> { }
@end

@implementation JNFStringCoercion

- (jobject) coerceNSObject:(id)obj withEnv:(JNIEnv *)env usingCoercer:(__unused JNFTypeCoercion *)coercer {
    return JNFNSToJavaString(env, (NSString *)obj);
}

- (id) coerceJavaObject:(jobject)obj withEnv:(JNIEnv *)env usingCoercer:(__unused JNFTypeCoercion *)coercer {
    return JNFJavaToNSString(env, (jstring)obj);
}

@end


@interface JNFNumberCoercion : NSObject <JNFTypeCoercion> { }
@end

@implementation JNFNumberCoercion

- (jobject) coerceNSObject:(id)obj withEnv:(JNIEnv *)env usingCoercer:(__unused JNFTypeCoercion *)coercer {
    return JNFNSToJavaNumber(env, (NSNumber *)obj);
}

- (id) coerceJavaObject:(jobject)obj withEnv:(JNIEnv *)env usingCoercer:(__unused JNFTypeCoercion *)coercer {
    return JNFJavaToNSNumber(env, obj);
}

@end


@interface JNFDateCoercion : NSObject <JNFTypeCoercion> { }
@end

@implementation JNFDateCoercion

- (jobject) coerceNSObject:(id)obj withEnv:(JNIEnv *)env usingCoercer:(__unused JNFTypeCoercion *)coercer {
    return JNFNSToJavaCalendar(env, (NSDate *)obj);
}

- (id) coerceJavaObject:(jobject)obj withEnv:(JNIEnv *)env usingCoercer:(__unused JNFTypeCoercion *)coercer {
    return JNFJavaToNSDate(env, obj);
}

@end


static JNF_CLASS_CACHE(jc_Iterator, "java/util/Iterator");
static JNF_MEMBER_CACHE(jm_Iterator_hasNext, jc_Iterator, "hasNext", "()Z");
static JNF_MEMBER_CACHE(jm_Iterator_next, jc_Iterator, "next", "()Ljava/lang/Object;");

@interface JNFMapCoercion : NSObject <JNFTypeCoercion> { }
@end

@implementation JNFMapCoercion

- (jobject) coerceNSObject:(id)obj withEnv:(JNIEnv *)env usingCoercer:(JNFTypeCoercion *)coercer {
    static JNF_CLASS_CACHE(jc_HashMap, "java/util/HashMap");
    static JNF_CTOR_CACHE(jm_HashMap_ctor, jc_HashMap, "()V");
    static JNF_MEMBER_CACHE(jm_HashMap_put, jc_HashMap, "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");

    NSDictionary *nsDict = (NSDictionary *)obj;
    NSEnumerator *keyEnum = [nsDict keyEnumerator];

    jobject jHashMap = JNFNewObject(env, jm_HashMap_ctor);

    id key;
    while ((key = [keyEnum nextObject]) != nil) {
        jobject jkey = [coercer coerceNSObject:key withEnv:env usingCoercer:coercer];

        id value = [nsDict objectForKey:key];
        jobject java_value = [coercer coerceNSObject:value withEnv:env usingCoercer:coercer];

        JNFCallObjectMethod(env, jHashMap, jm_HashMap_put, jkey, java_value);

        if (jkey != NULL) (*env)->DeleteLocalRef(env, jkey);
        if (java_value != NULL) (*env)->DeleteLocalRef(env, java_value);
    }

    return jHashMap;
}

- (id) coerceJavaObject:(jobject)obj withEnv:(JNIEnv *)env usingCoercer:(JNFTypeCoercion *)coercer {
    static JNF_CLASS_CACHE(jc_Map, "java/util/Map");
    static JNF_MEMBER_CACHE(jm_Map_keySet, jc_Map, "keySet", "()Ljava/util/Set;");
    static JNF_MEMBER_CACHE(jm_Map_get, jc_Map, "get", "(Ljava/lang/Object;)Ljava/lang/Object;");

    static JNF_CLASS_CACHE(jc_Set, "java/util/Set");
    static JNF_MEMBER_CACHE(jm_Set_iterator, jc_Set, "iterator", "()Ljava/util/Iterator;");

    NSMutableDictionary *dict = [[NSMutableDictionary alloc] init];
    jobject jKeySet = JNFCallObjectMethod(env, obj, jm_Map_keySet);
    jobject jKeyIter = JNFCallObjectMethod(env, jKeySet, jm_Set_iterator);
    if (jKeySet != NULL) (*env)->DeleteLocalRef(env, jKeySet);

    while (JNFCallBooleanMethod(env, jKeyIter, jm_Iterator_hasNext)) {
        jobject jkey = JNFCallObjectMethod(env, jKeyIter, jm_Iterator_next);
        id nsKey = [coercer coerceJavaObject:jkey withEnv:env usingCoercer:coercer];

        jobject java_value = JNFCallObjectMethod(env, obj, jm_Map_get, jkey);
        if (jkey != NULL) (*env)->DeleteLocalRef(env, jkey);

        id nsValue = [coercer coerceJavaObject:java_value withEnv:env usingCoercer:coercer];
        if (java_value != NULL) (*env)->DeleteLocalRef(env, java_value);

        [dict setObject:nsValue forKey:nsKey];
    }

    return [dict autorelease];
}

@end


@interface JNFListCoercion : NSObject <JNFTypeCoercion> { }
@end

@implementation JNFListCoercion

- (jobject) coerceNSObject:(id)obj withEnv:(JNIEnv *)env usingCoercer:(JNFTypeCoercion *)coercer {
    static JNF_CLASS_CACHE(jc_List, "java/util/ArrayList");
    static JNF_CTOR_CACHE(jm_List_ctor, jc_List, "(I)V");
    static JNF_MEMBER_CACHE(jm_List_add, jc_List, "add", "(Ljava/lang/Object;)Z");

    NSArray *nsArray = (NSArray *)obj;
    unsigned long count = [nsArray count];

    jobject javaArray = JNFNewObject(env, jm_List_ctor, (jint)count);

    unsigned long i;
    for (i = 0; i < count; i++) {
        id iThObj = [nsArray objectAtIndex:i];
        jobject iThJObj = [coercer coerceNSObject:iThObj withEnv:env usingCoercer:coercer];
        JNFCallBooleanMethod(env, javaArray, jm_List_add, iThJObj);
        if (iThJObj != NULL) (*env)->DeleteLocalRef(env, iThJObj);
    }

    return javaArray;
}

- (id) coerceJavaObject:(jobject)obj withEnv:(JNIEnv *)env usingCoercer:(JNFTypeCoercion *)coercer {
    static JNF_CLASS_CACHE(jc_List, "java/util/List");
    static JNF_MEMBER_CACHE(jm_List_iterator, jc_List, "iterator", "()Ljava/util/Iterator;");

    jobject jIterator = JNFCallObjectMethod(env, obj, jm_List_iterator);

    NSMutableArray *nsArray = [[NSMutableArray alloc] init];
    while (JNFCallBooleanMethod(env, jIterator, jm_Iterator_hasNext)) {
        jobject jobj = JNFCallObjectMethod(env, jIterator, jm_Iterator_next);
        id nsObj = [coercer coerceJavaObject:jobj withEnv:env usingCoercer:coercer];
        if (jobj != NULL) (*env)->DeleteLocalRef(env, jobj);
        [nsArray addObject:nsObj];
    }

    return [nsArray autorelease];
}

@end


@interface JNFSetCoercion : NSObject <JNFTypeCoercion> { }
@end

@implementation JNFSetCoercion

- (jobject) coerceNSObject:(id)obj withEnv:(JNIEnv *)env usingCoercer:(JNFTypeCoercion *)coercer {
    static JNF_CLASS_CACHE(jc_Set, "java/util/HashSet");
    static JNF_CTOR_CACHE(jm_Set_ctor, jc_Set, "()V");
    static JNF_MEMBER_CACHE(jm_Set_add, jc_Set, "add", "(Ljava/lang/Object;)Z");

    NSSet *nsSet = (NSSet *)obj;
    NSEnumerator *enumerator = [nsSet objectEnumerator];

    jobject javaSet = JNFNewObject(env, jm_Set_ctor);
    id next;
    while ((next = [enumerator nextObject]) != nil) {
        jobject jnext = [coercer coerceNSObject:next withEnv:env usingCoercer:coercer];
        if (jnext != NULL) {
            JNFCallBooleanMethod(env, javaSet, jm_Set_add, jnext);
            (*env)->DeleteLocalRef(env, jnext);
        }
    }

    return javaSet;
}

- (id) coerceJavaObject:(jobject)obj withEnv:(JNIEnv *)env usingCoercer:(JNFTypeCoercion *)coercer {
    static JNF_CLASS_CACHE(jc_Set, "java/util/Set");
    static JNF_MEMBER_CACHE(jm_Set_iterator, jc_Set, "iterator", "()Ljava/util/Iterator;");

    jobject jIterator = JNFCallObjectMethod(env, obj, jm_Set_iterator);

    NSMutableSet *nsSet = [[NSMutableSet alloc] init];
    while (JNFCallBooleanMethod(env, jIterator, jm_Iterator_hasNext)) {
        jobject jobj = JNFCallObjectMethod(env, jIterator, jm_Iterator_next);
        if (jobj != NULL) {
            id nsObj = [coercer coerceJavaObject:jobj withEnv:env usingCoercer:coercer];
            (*env)->DeleteLocalRef(env, jobj);
            [nsSet addObject:nsObj];
        }
    }

    return [nsSet autorelease];
}

@end


@implementation JNFDefaultCoercions

+ (void) addStringCoercionTo:(JNFTypeCoercer *)coercer {
    [coercer addCoercion:[[[JNFStringCoercion alloc] init] autorelease] forNSClass:[NSString class] javaClass:@"java/lang/String"];
}

+ (void) addNumberCoercionTo:(JNFTypeCoercer *)coercer {
    [coercer addCoercion:[[[JNFNumberCoercion alloc] init] autorelease] forNSClass:[NSNumber class] javaClass:@"java/lang/Number"];
}

+ (void) addDateCoercionTo:(JNFTypeCoercer *)coercer {
    id dateCoercion = [[[JNFDateCoercion alloc] init] autorelease];
    [coercer addCoercion:dateCoercion forNSClass:[NSDate class] javaClass:@"java/util/Calendar"];
    [coercer addCoercion:dateCoercion forNSClass:[NSDate class] javaClass:@"java/util/Date"];
}

+ (void) addListCoercionTo:(JNFTypeCoercer *)coercer {
    [coercer addCoercion:[[[JNFListCoercion alloc] init] autorelease] forNSClass:[NSArray class] javaClass:@"java/util/List"];
}

+ (void) addMapCoercionTo:(JNFTypeCoercer *)coercer {
    [coercer addCoercion:[[[JNFMapCoercion alloc] init] autorelease] forNSClass:[NSDictionary class] javaClass:@"java/util/Map"];
}

+ (void) addSetCoercionTo:(JNFTypeCoercer *)coercer {
    [coercer addCoercion:[[[JNFSetCoercion alloc] init] autorelease] forNSClass:[NSSet class] javaClass:@"java/util/Set"];
}

+ (JNFTypeCoercer *) defaultCoercer {
    JNFTypeCoercer *coercer = [[[JNFTypeCoercer alloc] initWithParent:nil] autorelease];

    [JNFDefaultCoercions addStringCoercionTo:coercer];
    [JNFDefaultCoercions addNumberCoercionTo:coercer];
    [JNFDefaultCoercions addDateCoercionTo:coercer];
    [JNFDefaultCoercions addListCoercionTo:coercer];
    [JNFDefaultCoercions addMapCoercionTo:coercer];
    [JNFDefaultCoercions addSetCoercionTo:coercer];

    return coercer;
}

@end
