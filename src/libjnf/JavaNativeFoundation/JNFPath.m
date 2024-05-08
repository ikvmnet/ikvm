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

#import "JNFPath.h"

#import <Cocoa/Cocoa.h>

#import "JNFString.h"

jstring JNFNormalizedJavaStringForPath(JNIEnv *env, NSString *inString)
{
    if (inString == nil) return NULL;

    CFMutableStringRef mutableDisplayName = CFStringCreateMutableCopy(NULL, 0, (CFStringRef)inString);
    CFStringNormalize(mutableDisplayName, kCFStringNormalizationFormC);
    jstring returnValue = JNFNSToJavaString(env, (NSString *)mutableDisplayName);
    CFRelease(mutableDisplayName);

    return returnValue;
}

NSString *JNFNormalizedNSStringForPath(JNIEnv *env, jstring javaString)
{
    if (javaString == NULL) return nil;

    // We were given a filename, so convert it to a compatible representation for the file system.
    NSFileManager *fm = [NSFileManager defaultManager];
    NSString *fileName = JNFJavaToNSString(env, javaString);
    const char *compatibleFilename = [fm fileSystemRepresentationWithPath:fileName];
    return [fm stringWithFileSystemRepresentation:compatibleFilename length:strlen(compatibleFilename)];
}
