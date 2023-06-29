/**

We use this file to redefine a few methods that should be exported, but which are not exported in jni.h.
OpenJDK passes /export or -export to the command line to do these manually.

*/

#include <jni.h>

#ifndef _IKVM_JVM_H_
#define _IKVM_JVM_H_

#ifdef __cplusplus
extern "C" {
#endif

    JNIEXPORT int jio_vsnprintf(char* str, size_t count, const char* fmt, va_list args);

    JNIEXPORT int jio_snprintf(char* str, size_t count, const char* fmt, ...);

    JNIEXPORT int jio_fprintf(FILE*, const char* fmt, ...);

    JNIEXPORT int jio_vfprintf(FILE*, const char* fmt, va_list args);

#ifdef __cplusplus
}
#endif

#endif
