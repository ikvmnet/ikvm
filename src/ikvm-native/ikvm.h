#ifndef IKVM_H_INCLUDED
#define IKVM_H_INCLUDED

#ifdef _WIN32
#define EXPORT __declspec(dllexport)
#else
#if (defined(__GNUC__) && ((__GNUC__ > 4) || (__GNUC__ == 4) && (__GNUC_MINOR__ > 2))) || __has_attribute(visibility)
#define EXPORT __attribute__((visibility("default")))
#else
#define EXPORT
#endif
#endif

#include <stdarg.h>
#include <jni.h>

#ifdef _WIN32
#include <malloc.h>
#define ALLOCA _alloca
#else
#if defined(__FreeBSD__) || defined(__NetBSD__) || defined(__OpenBSD__)
#include <stdlib.h>
#else
#include <alloca.h>
#endif
#define ALLOCA alloca
#endif

EXPORT void** ikvm_GetJNIEnvVTable();

#endif
