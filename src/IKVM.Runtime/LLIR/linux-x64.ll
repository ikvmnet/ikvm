; ModuleID = 'jni.c'
source_filename = "jni.c"
target datalayout = "e-m:e-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-pc-linux-gnueabihf"

%struct.JNINativeInterface_ = type { i8*, i8*, i8*, i8*, i32 (%struct.JNINativeInterface_**)*, %struct._jobject* (%struct.JNINativeInterface_**, i8*, %struct._jobject*, i8*, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i8*)*, %struct._jmethodID* (%struct.JNINativeInterface_**, %struct._jobject*)*, %struct._jfieldID* (%struct.JNINativeInterface_**, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i8)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, %struct._jobject* (%struct.JNINativeInterface_**)*, void (%struct.JNINativeInterface_**)*, void (%struct.JNINativeInterface_**)*, void (%struct.JNINativeInterface_**, i8*)*, i32 (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, void (%struct.JNINativeInterface_**, %struct._jobject*)*, void (%struct.JNINativeInterface_**, %struct._jobject*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, {}*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*)*, %struct._jmethodID* (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i8*)*, {}*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jfieldID* (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i8*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, %struct._jobject*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i8)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i8)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i16)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i16)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i64)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, float)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, double)*, %struct._jmethodID* (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i8*)*, {}*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jfieldID* (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i8*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, %struct._jobject*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i8)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i8)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i16)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i16)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i64)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, float)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, double)*, %struct._jobject* (%struct.JNINativeInterface_**, i16*, i32)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i16* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i16*)*, %struct._jobject* (%struct.JNINativeInterface_**, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i8* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, i32, %struct._jobject*, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, i8* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i8* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i16* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i16* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i32* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i64* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, float* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, double* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i16*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i16*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i64*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, float*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, double*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i16*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i16*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i32*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i64*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, float*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, double*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i16*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i16*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i32*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i64*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, float*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, double*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct.JNINativeMethod*, i32)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, %struct.JNIInvokeInterface_***)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i16*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i8*)*, i8* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i32)*, i16* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i16*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, void (%struct.JNINativeInterface_**, %struct._jobject*)*, i8 (%struct.JNINativeInterface_**)*, %struct._jobject* (%struct.JNINativeInterface_**, i8*, i64)*, i8* (%struct.JNINativeInterface_**, %struct._jobject*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)* }
%struct._jfieldID = type opaque
%struct.__va_list_tag = type { i32, i32, i8*, i8* }
%union.jvalue = type { i64 }
%struct.JNINativeMethod = type { i8*, i8*, i8* }
%struct.JNIInvokeInterface_ = type { i8*, i8*, i8*, i32 (%struct.JNIInvokeInterface_**)*, i32 (%struct.JNIInvokeInterface_**, i8**, i8*)*, i32 (%struct.JNIInvokeInterface_**)*, i32 (%struct.JNIInvokeInterface_**, i8**, i32)*, i32 (%struct.JNIInvokeInterface_**, i8**, i8*)* }
%struct._jobject = type opaque
%struct._jmethodID = type opaque

; Function Attrs: noinline nounwind optnone uwtable
define dso_local %struct._jobject* @JNI_CallObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %10 = bitcast %struct.__va_list_tag* %9 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 35
  %14 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %19 = call %struct._jobject* %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %struct.__va_list_tag* noundef %18)
  store %struct._jobject* %19, %struct._jobject** %7, align 8
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %21 = bitcast %struct.__va_list_tag* %20 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load %struct._jobject*, %struct._jobject** %7, align 8
  ret %struct._jobject* %22
}

; Function Attrs: nofree nosync nounwind willreturn
declare void @llvm.va_start(i8*) #1

; Function Attrs: nofree nosync nounwind willreturn
declare void @llvm.va_end(i8*) #1

; Function Attrs: noinline nounwind optnone uwtable
define dso_local %struct._jobject* @JNI_CallObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !6

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 36
  %270 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  %275 = call %struct._jobject* %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret %struct._jobject* %275
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local %struct._jobject* @JNI_CallNonvirtualObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca %struct._jobject*, align 8
  %10 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %11 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %12 = bitcast %struct.__va_list_tag* %11 to i8*
  call void @llvm.va_start(i8* %12)
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 65
  %16 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %15, align 8
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %18 = load %struct._jobject*, %struct._jobject** %6, align 8
  %19 = load %struct._jobject*, %struct._jobject** %7, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %21 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %22 = call %struct._jobject* %16(%struct.JNINativeInterface_** noundef %17, %struct._jobject* noundef %18, %struct._jobject* noundef %19, %struct._jmethodID* noundef %20, %struct.__va_list_tag* noundef %21)
  store %struct._jobject* %22, %struct._jobject** %9, align 8
  %23 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %24 = bitcast %struct.__va_list_tag* %23 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load %struct._jobject*, %struct._jobject** %9, align 8
  ret %struct._jobject* %25
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local %struct._jobject* @JNI_CallNonvirtualObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %struct.__va_list_tag* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca %struct.__va_list_tag*, align 8
  %11 = alloca %union.jvalue*, align 8
  %12 = alloca [257 x i8], align 16
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  store %struct.__va_list_tag* %4, %struct.__va_list_tag** %10, align 8
  br label %15

15:                                               ; preds = %5
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 8
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %22 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i64 0, i64 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %13, align 4
  %25 = load i32, i32* %13, align 4
  %26 = sext i32 %25 to i64
  %27 = mul i64 %26, 8
  %28 = alloca i8, i64 %27, align 16
  %29 = bitcast i8* %28 to %union.jvalue*
  store %union.jvalue* %29, %union.jvalue** %11, align 8
  store i32 0, i32* %14, align 4
  br label %30

30:                                               ; preds = %264, %15
  %31 = load i32, i32* %14, align 4
  %32 = load i32, i32* %13, align 4
  %33 = icmp slt i32 %31, %32
  br i1 %33, label %34, label %267

34:                                               ; preds = %30
  %35 = load i32, i32* %14, align 4
  %36 = sext i32 %35 to i64
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i64 0, i64 %36
  %38 = load i8, i8* %37, align 1
  %39 = sext i8 %38 to i32
  switch i32 %39, label %263 [
    i32 90, label %40
    i32 66, label %65
    i32 83, label %90
    i32 67, label %115
    i32 73, label %141
    i32 74, label %165
    i32 68, label %189
    i32 70, label %213
    i32 76, label %238
  ]

40:                                               ; preds = %34
  %41 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %42 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 0
  %43 = load i32, i32* %42, align 8
  %44 = icmp ule i32 %43, 40
  br i1 %44, label %45, label %51

45:                                               ; preds = %40
  %46 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 3
  %47 = load i8*, i8** %46, align 8
  %48 = getelementptr i8, i8* %47, i32 %43
  %49 = bitcast i8* %48 to i32*
  %50 = add i32 %43, 8
  store i32 %50, i32* %42, align 8
  br label %56

51:                                               ; preds = %40
  %52 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 2
  %53 = load i8*, i8** %52, align 8
  %54 = bitcast i8* %53 to i32*
  %55 = getelementptr i8, i8* %53, i32 8
  store i8* %55, i8** %52, align 8
  br label %56

56:                                               ; preds = %51, %45
  %57 = phi i32* [ %49, %45 ], [ %54, %51 ]
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %11, align 8
  %61 = load i32, i32* %14, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %59, i8* %64, align 8
  br label %263

65:                                               ; preds = %34
  %66 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %67 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 0
  %68 = load i32, i32* %67, align 8
  %69 = icmp ule i32 %68, 40
  br i1 %69, label %70, label %76

70:                                               ; preds = %65
  %71 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 3
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr i8, i8* %72, i32 %68
  %74 = bitcast i8* %73 to i32*
  %75 = add i32 %68, 8
  store i32 %75, i32* %67, align 8
  br label %81

76:                                               ; preds = %65
  %77 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 2
  %78 = load i8*, i8** %77, align 8
  %79 = bitcast i8* %78 to i32*
  %80 = getelementptr i8, i8* %78, i32 8
  store i8* %80, i8** %77, align 8
  br label %81

81:                                               ; preds = %76, %70
  %82 = phi i32* [ %74, %70 ], [ %79, %76 ]
  %83 = load i32, i32* %82, align 4
  %84 = trunc i32 %83 to i8
  %85 = load %union.jvalue*, %union.jvalue** %11, align 8
  %86 = load i32, i32* %14, align 4
  %87 = sext i32 %86 to i64
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %85, i64 %87
  %89 = bitcast %union.jvalue* %88 to i8*
  store i8 %84, i8* %89, align 8
  br label %263

90:                                               ; preds = %34
  %91 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %92 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 0
  %93 = load i32, i32* %92, align 8
  %94 = icmp ule i32 %93, 40
  br i1 %94, label %95, label %101

95:                                               ; preds = %90
  %96 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 3
  %97 = load i8*, i8** %96, align 8
  %98 = getelementptr i8, i8* %97, i32 %93
  %99 = bitcast i8* %98 to i32*
  %100 = add i32 %93, 8
  store i32 %100, i32* %92, align 8
  br label %106

101:                                              ; preds = %90
  %102 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 2
  %103 = load i8*, i8** %102, align 8
  %104 = bitcast i8* %103 to i32*
  %105 = getelementptr i8, i8* %103, i32 8
  store i8* %105, i8** %102, align 8
  br label %106

106:                                              ; preds = %101, %95
  %107 = phi i32* [ %99, %95 ], [ %104, %101 ]
  %108 = load i32, i32* %107, align 4
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %11, align 8
  %111 = load i32, i32* %14, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %263

115:                                              ; preds = %34
  %116 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %117 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 0
  %118 = load i32, i32* %117, align 8
  %119 = icmp ule i32 %118, 40
  br i1 %119, label %120, label %126

120:                                              ; preds = %115
  %121 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 3
  %122 = load i8*, i8** %121, align 8
  %123 = getelementptr i8, i8* %122, i32 %118
  %124 = bitcast i8* %123 to i32*
  %125 = add i32 %118, 8
  store i32 %125, i32* %117, align 8
  br label %131

126:                                              ; preds = %115
  %127 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 2
  %128 = load i8*, i8** %127, align 8
  %129 = bitcast i8* %128 to i32*
  %130 = getelementptr i8, i8* %128, i32 8
  store i8* %130, i8** %127, align 8
  br label %131

131:                                              ; preds = %126, %120
  %132 = phi i32* [ %124, %120 ], [ %129, %126 ]
  %133 = load i32, i32* %132, align 4
  %134 = trunc i32 %133 to i16
  %135 = zext i16 %134 to i32
  %136 = load %union.jvalue*, %union.jvalue** %11, align 8
  %137 = load i32, i32* %14, align 4
  %138 = sext i32 %137 to i64
  %139 = getelementptr inbounds %union.jvalue, %union.jvalue* %136, i64 %138
  %140 = bitcast %union.jvalue* %139 to i32*
  store i32 %135, i32* %140, align 8
  br label %263

141:                                              ; preds = %34
  %142 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %143 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 0
  %144 = load i32, i32* %143, align 8
  %145 = icmp ule i32 %144, 40
  br i1 %145, label %146, label %152

146:                                              ; preds = %141
  %147 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 3
  %148 = load i8*, i8** %147, align 8
  %149 = getelementptr i8, i8* %148, i32 %144
  %150 = bitcast i8* %149 to i32*
  %151 = add i32 %144, 8
  store i32 %151, i32* %143, align 8
  br label %157

152:                                              ; preds = %141
  %153 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 2
  %154 = load i8*, i8** %153, align 8
  %155 = bitcast i8* %154 to i32*
  %156 = getelementptr i8, i8* %154, i32 8
  store i8* %156, i8** %153, align 8
  br label %157

157:                                              ; preds = %152, %146
  %158 = phi i32* [ %150, %146 ], [ %155, %152 ]
  %159 = load i32, i32* %158, align 4
  %160 = load %union.jvalue*, %union.jvalue** %11, align 8
  %161 = load i32, i32* %14, align 4
  %162 = sext i32 %161 to i64
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %160, i64 %162
  %164 = bitcast %union.jvalue* %163 to i32*
  store i32 %159, i32* %164, align 8
  br label %263

165:                                              ; preds = %34
  %166 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %167 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 0
  %168 = load i32, i32* %167, align 8
  %169 = icmp ule i32 %168, 40
  br i1 %169, label %170, label %176

170:                                              ; preds = %165
  %171 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 3
  %172 = load i8*, i8** %171, align 8
  %173 = getelementptr i8, i8* %172, i32 %168
  %174 = bitcast i8* %173 to i64*
  %175 = add i32 %168, 8
  store i32 %175, i32* %167, align 8
  br label %181

176:                                              ; preds = %165
  %177 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 2
  %178 = load i8*, i8** %177, align 8
  %179 = bitcast i8* %178 to i64*
  %180 = getelementptr i8, i8* %178, i32 8
  store i8* %180, i8** %177, align 8
  br label %181

181:                                              ; preds = %176, %170
  %182 = phi i64* [ %174, %170 ], [ %179, %176 ]
  %183 = load i64, i64* %182, align 8
  %184 = load %union.jvalue*, %union.jvalue** %11, align 8
  %185 = load i32, i32* %14, align 4
  %186 = sext i32 %185 to i64
  %187 = getelementptr inbounds %union.jvalue, %union.jvalue* %184, i64 %186
  %188 = bitcast %union.jvalue* %187 to i64*
  store i64 %183, i64* %188, align 8
  br label %263

189:                                              ; preds = %34
  %190 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %191 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 1
  %192 = load i32, i32* %191, align 4
  %193 = icmp ule i32 %192, 160
  br i1 %193, label %194, label %200

194:                                              ; preds = %189
  %195 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 3
  %196 = load i8*, i8** %195, align 8
  %197 = getelementptr i8, i8* %196, i32 %192
  %198 = bitcast i8* %197 to double*
  %199 = add i32 %192, 16
  store i32 %199, i32* %191, align 4
  br label %205

200:                                              ; preds = %189
  %201 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 2
  %202 = load i8*, i8** %201, align 8
  %203 = bitcast i8* %202 to double*
  %204 = getelementptr i8, i8* %202, i32 8
  store i8* %204, i8** %201, align 8
  br label %205

205:                                              ; preds = %200, %194
  %206 = phi double* [ %198, %194 ], [ %203, %200 ]
  %207 = load double, double* %206, align 8
  %208 = load %union.jvalue*, %union.jvalue** %11, align 8
  %209 = load i32, i32* %14, align 4
  %210 = sext i32 %209 to i64
  %211 = getelementptr inbounds %union.jvalue, %union.jvalue* %208, i64 %210
  %212 = bitcast %union.jvalue* %211 to double*
  store double %207, double* %212, align 8
  br label %263

213:                                              ; preds = %34
  %214 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %215 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 1
  %216 = load i32, i32* %215, align 4
  %217 = icmp ule i32 %216, 160
  br i1 %217, label %218, label %224

218:                                              ; preds = %213
  %219 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 3
  %220 = load i8*, i8** %219, align 8
  %221 = getelementptr i8, i8* %220, i32 %216
  %222 = bitcast i8* %221 to double*
  %223 = add i32 %216, 16
  store i32 %223, i32* %215, align 4
  br label %229

224:                                              ; preds = %213
  %225 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = bitcast i8* %226 to double*
  %228 = getelementptr i8, i8* %226, i32 8
  store i8* %228, i8** %225, align 8
  br label %229

229:                                              ; preds = %224, %218
  %230 = phi double* [ %222, %218 ], [ %227, %224 ]
  %231 = load double, double* %230, align 8
  %232 = fptrunc double %231 to float
  %233 = load %union.jvalue*, %union.jvalue** %11, align 8
  %234 = load i32, i32* %14, align 4
  %235 = sext i32 %234 to i64
  %236 = getelementptr inbounds %union.jvalue, %union.jvalue* %233, i64 %235
  %237 = bitcast %union.jvalue* %236 to float*
  store float %232, float* %237, align 8
  br label %263

238:                                              ; preds = %34
  %239 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %240 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 0
  %241 = load i32, i32* %240, align 8
  %242 = icmp ule i32 %241, 40
  br i1 %242, label %243, label %249

243:                                              ; preds = %238
  %244 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 3
  %245 = load i8*, i8** %244, align 8
  %246 = getelementptr i8, i8* %245, i32 %241
  %247 = bitcast i8* %246 to i8**
  %248 = add i32 %241, 8
  store i32 %248, i32* %240, align 8
  br label %254

249:                                              ; preds = %238
  %250 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 2
  %251 = load i8*, i8** %250, align 8
  %252 = bitcast i8* %251 to i8**
  %253 = getelementptr i8, i8* %251, i32 8
  store i8* %253, i8** %250, align 8
  br label %254

254:                                              ; preds = %249, %243
  %255 = phi i8** [ %247, %243 ], [ %252, %249 ]
  %256 = load i8*, i8** %255, align 8
  %257 = bitcast i8* %256 to %struct._jobject*
  %258 = load %union.jvalue*, %union.jvalue** %11, align 8
  %259 = load i32, i32* %14, align 4
  %260 = sext i32 %259 to i64
  %261 = getelementptr inbounds %union.jvalue, %union.jvalue* %258, i64 %260
  %262 = bitcast %union.jvalue* %261 to %struct._jobject**
  store %struct._jobject* %257, %struct._jobject** %262, align 8
  br label %263

263:                                              ; preds = %34, %254, %229, %205, %181, %157, %131, %106, %81, %56
  br label %264

264:                                              ; preds = %263
  %265 = load i32, i32* %14, align 4
  %266 = add nsw i32 %265, 1
  store i32 %266, i32* %14, align 4
  br label %30, !llvm.loop !8

267:                                              ; preds = %30
  br label %268

268:                                              ; preds = %267
  %269 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %270 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %269, align 8
  %271 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %270, i32 0, i32 66
  %272 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %271, align 8
  %273 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %274 = load %struct._jobject*, %struct._jobject** %7, align 8
  %275 = load %struct._jobject*, %struct._jobject** %8, align 8
  %276 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %277 = load %union.jvalue*, %union.jvalue** %11, align 8
  %278 = call %struct._jobject* %272(%struct.JNINativeInterface_** noundef %273, %struct._jobject* noundef %274, %struct._jobject* noundef %275, %struct._jmethodID* noundef %276, %union.jvalue* noundef %277)
  ret %struct._jobject* %278
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local %struct._jobject* @JNI_CallStaticObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %10 = bitcast %struct.__va_list_tag* %9 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 115
  %14 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %19 = call %struct._jobject* %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %struct.__va_list_tag* noundef %18)
  store %struct._jobject* %19, %struct._jobject** %7, align 8
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %21 = bitcast %struct.__va_list_tag* %20 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load %struct._jobject*, %struct._jobject** %7, align 8
  ret %struct._jobject* %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local %struct._jobject* @JNI_CallStaticObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !9

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 116
  %270 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  %275 = call %struct._jobject* %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret %struct._jobject* %275
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local zeroext i8 @JNI_CallBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8, align 1
  %8 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %10 = bitcast %struct.__va_list_tag* %9 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 38
  %14 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %19 = call zeroext i8 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %struct.__va_list_tag* noundef %18)
  store i8 %19, i8* %7, align 1
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %21 = bitcast %struct.__va_list_tag* %20 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i8, i8* %7, align 1
  ret i8 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local zeroext i8 @JNI_CallBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !10

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 39
  %270 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  %275 = call zeroext i8 %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret i8 %275
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local zeroext i8 @JNI_CallNonvirtualBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i8, align 1
  %10 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %11 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %12 = bitcast %struct.__va_list_tag* %11 to i8*
  call void @llvm.va_start(i8* %12)
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 68
  %16 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %15, align 8
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %18 = load %struct._jobject*, %struct._jobject** %6, align 8
  %19 = load %struct._jobject*, %struct._jobject** %7, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %21 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %22 = call zeroext i8 %16(%struct.JNINativeInterface_** noundef %17, %struct._jobject* noundef %18, %struct._jobject* noundef %19, %struct._jmethodID* noundef %20, %struct.__va_list_tag* noundef %21)
  store i8 %22, i8* %9, align 1
  %23 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %24 = bitcast %struct.__va_list_tag* %23 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load i8, i8* %9, align 1
  ret i8 %25
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local zeroext i8 @JNI_CallNonvirtualBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %struct.__va_list_tag* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca %struct.__va_list_tag*, align 8
  %11 = alloca %union.jvalue*, align 8
  %12 = alloca [257 x i8], align 16
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  store %struct.__va_list_tag* %4, %struct.__va_list_tag** %10, align 8
  br label %15

15:                                               ; preds = %5
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 8
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %22 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i64 0, i64 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %13, align 4
  %25 = load i32, i32* %13, align 4
  %26 = sext i32 %25 to i64
  %27 = mul i64 %26, 8
  %28 = alloca i8, i64 %27, align 16
  %29 = bitcast i8* %28 to %union.jvalue*
  store %union.jvalue* %29, %union.jvalue** %11, align 8
  store i32 0, i32* %14, align 4
  br label %30

30:                                               ; preds = %264, %15
  %31 = load i32, i32* %14, align 4
  %32 = load i32, i32* %13, align 4
  %33 = icmp slt i32 %31, %32
  br i1 %33, label %34, label %267

34:                                               ; preds = %30
  %35 = load i32, i32* %14, align 4
  %36 = sext i32 %35 to i64
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i64 0, i64 %36
  %38 = load i8, i8* %37, align 1
  %39 = sext i8 %38 to i32
  switch i32 %39, label %263 [
    i32 90, label %40
    i32 66, label %65
    i32 83, label %90
    i32 67, label %115
    i32 73, label %141
    i32 74, label %165
    i32 68, label %189
    i32 70, label %213
    i32 76, label %238
  ]

40:                                               ; preds = %34
  %41 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %42 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 0
  %43 = load i32, i32* %42, align 8
  %44 = icmp ule i32 %43, 40
  br i1 %44, label %45, label %51

45:                                               ; preds = %40
  %46 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 3
  %47 = load i8*, i8** %46, align 8
  %48 = getelementptr i8, i8* %47, i32 %43
  %49 = bitcast i8* %48 to i32*
  %50 = add i32 %43, 8
  store i32 %50, i32* %42, align 8
  br label %56

51:                                               ; preds = %40
  %52 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 2
  %53 = load i8*, i8** %52, align 8
  %54 = bitcast i8* %53 to i32*
  %55 = getelementptr i8, i8* %53, i32 8
  store i8* %55, i8** %52, align 8
  br label %56

56:                                               ; preds = %51, %45
  %57 = phi i32* [ %49, %45 ], [ %54, %51 ]
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %11, align 8
  %61 = load i32, i32* %14, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %59, i8* %64, align 8
  br label %263

65:                                               ; preds = %34
  %66 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %67 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 0
  %68 = load i32, i32* %67, align 8
  %69 = icmp ule i32 %68, 40
  br i1 %69, label %70, label %76

70:                                               ; preds = %65
  %71 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 3
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr i8, i8* %72, i32 %68
  %74 = bitcast i8* %73 to i32*
  %75 = add i32 %68, 8
  store i32 %75, i32* %67, align 8
  br label %81

76:                                               ; preds = %65
  %77 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 2
  %78 = load i8*, i8** %77, align 8
  %79 = bitcast i8* %78 to i32*
  %80 = getelementptr i8, i8* %78, i32 8
  store i8* %80, i8** %77, align 8
  br label %81

81:                                               ; preds = %76, %70
  %82 = phi i32* [ %74, %70 ], [ %79, %76 ]
  %83 = load i32, i32* %82, align 4
  %84 = trunc i32 %83 to i8
  %85 = load %union.jvalue*, %union.jvalue** %11, align 8
  %86 = load i32, i32* %14, align 4
  %87 = sext i32 %86 to i64
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %85, i64 %87
  %89 = bitcast %union.jvalue* %88 to i8*
  store i8 %84, i8* %89, align 8
  br label %263

90:                                               ; preds = %34
  %91 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %92 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 0
  %93 = load i32, i32* %92, align 8
  %94 = icmp ule i32 %93, 40
  br i1 %94, label %95, label %101

95:                                               ; preds = %90
  %96 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 3
  %97 = load i8*, i8** %96, align 8
  %98 = getelementptr i8, i8* %97, i32 %93
  %99 = bitcast i8* %98 to i32*
  %100 = add i32 %93, 8
  store i32 %100, i32* %92, align 8
  br label %106

101:                                              ; preds = %90
  %102 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 2
  %103 = load i8*, i8** %102, align 8
  %104 = bitcast i8* %103 to i32*
  %105 = getelementptr i8, i8* %103, i32 8
  store i8* %105, i8** %102, align 8
  br label %106

106:                                              ; preds = %101, %95
  %107 = phi i32* [ %99, %95 ], [ %104, %101 ]
  %108 = load i32, i32* %107, align 4
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %11, align 8
  %111 = load i32, i32* %14, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %263

115:                                              ; preds = %34
  %116 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %117 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 0
  %118 = load i32, i32* %117, align 8
  %119 = icmp ule i32 %118, 40
  br i1 %119, label %120, label %126

120:                                              ; preds = %115
  %121 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 3
  %122 = load i8*, i8** %121, align 8
  %123 = getelementptr i8, i8* %122, i32 %118
  %124 = bitcast i8* %123 to i32*
  %125 = add i32 %118, 8
  store i32 %125, i32* %117, align 8
  br label %131

126:                                              ; preds = %115
  %127 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 2
  %128 = load i8*, i8** %127, align 8
  %129 = bitcast i8* %128 to i32*
  %130 = getelementptr i8, i8* %128, i32 8
  store i8* %130, i8** %127, align 8
  br label %131

131:                                              ; preds = %126, %120
  %132 = phi i32* [ %124, %120 ], [ %129, %126 ]
  %133 = load i32, i32* %132, align 4
  %134 = trunc i32 %133 to i16
  %135 = zext i16 %134 to i32
  %136 = load %union.jvalue*, %union.jvalue** %11, align 8
  %137 = load i32, i32* %14, align 4
  %138 = sext i32 %137 to i64
  %139 = getelementptr inbounds %union.jvalue, %union.jvalue* %136, i64 %138
  %140 = bitcast %union.jvalue* %139 to i32*
  store i32 %135, i32* %140, align 8
  br label %263

141:                                              ; preds = %34
  %142 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %143 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 0
  %144 = load i32, i32* %143, align 8
  %145 = icmp ule i32 %144, 40
  br i1 %145, label %146, label %152

146:                                              ; preds = %141
  %147 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 3
  %148 = load i8*, i8** %147, align 8
  %149 = getelementptr i8, i8* %148, i32 %144
  %150 = bitcast i8* %149 to i32*
  %151 = add i32 %144, 8
  store i32 %151, i32* %143, align 8
  br label %157

152:                                              ; preds = %141
  %153 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 2
  %154 = load i8*, i8** %153, align 8
  %155 = bitcast i8* %154 to i32*
  %156 = getelementptr i8, i8* %154, i32 8
  store i8* %156, i8** %153, align 8
  br label %157

157:                                              ; preds = %152, %146
  %158 = phi i32* [ %150, %146 ], [ %155, %152 ]
  %159 = load i32, i32* %158, align 4
  %160 = load %union.jvalue*, %union.jvalue** %11, align 8
  %161 = load i32, i32* %14, align 4
  %162 = sext i32 %161 to i64
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %160, i64 %162
  %164 = bitcast %union.jvalue* %163 to i32*
  store i32 %159, i32* %164, align 8
  br label %263

165:                                              ; preds = %34
  %166 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %167 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 0
  %168 = load i32, i32* %167, align 8
  %169 = icmp ule i32 %168, 40
  br i1 %169, label %170, label %176

170:                                              ; preds = %165
  %171 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 3
  %172 = load i8*, i8** %171, align 8
  %173 = getelementptr i8, i8* %172, i32 %168
  %174 = bitcast i8* %173 to i64*
  %175 = add i32 %168, 8
  store i32 %175, i32* %167, align 8
  br label %181

176:                                              ; preds = %165
  %177 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 2
  %178 = load i8*, i8** %177, align 8
  %179 = bitcast i8* %178 to i64*
  %180 = getelementptr i8, i8* %178, i32 8
  store i8* %180, i8** %177, align 8
  br label %181

181:                                              ; preds = %176, %170
  %182 = phi i64* [ %174, %170 ], [ %179, %176 ]
  %183 = load i64, i64* %182, align 8
  %184 = load %union.jvalue*, %union.jvalue** %11, align 8
  %185 = load i32, i32* %14, align 4
  %186 = sext i32 %185 to i64
  %187 = getelementptr inbounds %union.jvalue, %union.jvalue* %184, i64 %186
  %188 = bitcast %union.jvalue* %187 to i64*
  store i64 %183, i64* %188, align 8
  br label %263

189:                                              ; preds = %34
  %190 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %191 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 1
  %192 = load i32, i32* %191, align 4
  %193 = icmp ule i32 %192, 160
  br i1 %193, label %194, label %200

194:                                              ; preds = %189
  %195 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 3
  %196 = load i8*, i8** %195, align 8
  %197 = getelementptr i8, i8* %196, i32 %192
  %198 = bitcast i8* %197 to double*
  %199 = add i32 %192, 16
  store i32 %199, i32* %191, align 4
  br label %205

200:                                              ; preds = %189
  %201 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 2
  %202 = load i8*, i8** %201, align 8
  %203 = bitcast i8* %202 to double*
  %204 = getelementptr i8, i8* %202, i32 8
  store i8* %204, i8** %201, align 8
  br label %205

205:                                              ; preds = %200, %194
  %206 = phi double* [ %198, %194 ], [ %203, %200 ]
  %207 = load double, double* %206, align 8
  %208 = load %union.jvalue*, %union.jvalue** %11, align 8
  %209 = load i32, i32* %14, align 4
  %210 = sext i32 %209 to i64
  %211 = getelementptr inbounds %union.jvalue, %union.jvalue* %208, i64 %210
  %212 = bitcast %union.jvalue* %211 to double*
  store double %207, double* %212, align 8
  br label %263

213:                                              ; preds = %34
  %214 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %215 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 1
  %216 = load i32, i32* %215, align 4
  %217 = icmp ule i32 %216, 160
  br i1 %217, label %218, label %224

218:                                              ; preds = %213
  %219 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 3
  %220 = load i8*, i8** %219, align 8
  %221 = getelementptr i8, i8* %220, i32 %216
  %222 = bitcast i8* %221 to double*
  %223 = add i32 %216, 16
  store i32 %223, i32* %215, align 4
  br label %229

224:                                              ; preds = %213
  %225 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = bitcast i8* %226 to double*
  %228 = getelementptr i8, i8* %226, i32 8
  store i8* %228, i8** %225, align 8
  br label %229

229:                                              ; preds = %224, %218
  %230 = phi double* [ %222, %218 ], [ %227, %224 ]
  %231 = load double, double* %230, align 8
  %232 = fptrunc double %231 to float
  %233 = load %union.jvalue*, %union.jvalue** %11, align 8
  %234 = load i32, i32* %14, align 4
  %235 = sext i32 %234 to i64
  %236 = getelementptr inbounds %union.jvalue, %union.jvalue* %233, i64 %235
  %237 = bitcast %union.jvalue* %236 to float*
  store float %232, float* %237, align 8
  br label %263

238:                                              ; preds = %34
  %239 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %240 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 0
  %241 = load i32, i32* %240, align 8
  %242 = icmp ule i32 %241, 40
  br i1 %242, label %243, label %249

243:                                              ; preds = %238
  %244 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 3
  %245 = load i8*, i8** %244, align 8
  %246 = getelementptr i8, i8* %245, i32 %241
  %247 = bitcast i8* %246 to i8**
  %248 = add i32 %241, 8
  store i32 %248, i32* %240, align 8
  br label %254

249:                                              ; preds = %238
  %250 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 2
  %251 = load i8*, i8** %250, align 8
  %252 = bitcast i8* %251 to i8**
  %253 = getelementptr i8, i8* %251, i32 8
  store i8* %253, i8** %250, align 8
  br label %254

254:                                              ; preds = %249, %243
  %255 = phi i8** [ %247, %243 ], [ %252, %249 ]
  %256 = load i8*, i8** %255, align 8
  %257 = bitcast i8* %256 to %struct._jobject*
  %258 = load %union.jvalue*, %union.jvalue** %11, align 8
  %259 = load i32, i32* %14, align 4
  %260 = sext i32 %259 to i64
  %261 = getelementptr inbounds %union.jvalue, %union.jvalue* %258, i64 %260
  %262 = bitcast %union.jvalue* %261 to %struct._jobject**
  store %struct._jobject* %257, %struct._jobject** %262, align 8
  br label %263

263:                                              ; preds = %34, %254, %229, %205, %181, %157, %131, %106, %81, %56
  br label %264

264:                                              ; preds = %263
  %265 = load i32, i32* %14, align 4
  %266 = add nsw i32 %265, 1
  store i32 %266, i32* %14, align 4
  br label %30, !llvm.loop !11

267:                                              ; preds = %30
  br label %268

268:                                              ; preds = %267
  %269 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %270 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %269, align 8
  %271 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %270, i32 0, i32 69
  %272 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %271, align 8
  %273 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %274 = load %struct._jobject*, %struct._jobject** %7, align 8
  %275 = load %struct._jobject*, %struct._jobject** %8, align 8
  %276 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %277 = load %union.jvalue*, %union.jvalue** %11, align 8
  %278 = call zeroext i8 %272(%struct.JNINativeInterface_** noundef %273, %struct._jobject* noundef %274, %struct._jobject* noundef %275, %struct._jmethodID* noundef %276, %union.jvalue* noundef %277)
  ret i8 %278
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local zeroext i8 @JNI_CallStaticBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8, align 1
  %8 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %10 = bitcast %struct.__va_list_tag* %9 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 118
  %14 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %19 = call zeroext i8 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %struct.__va_list_tag* noundef %18)
  store i8 %19, i8* %7, align 1
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %21 = bitcast %struct.__va_list_tag* %20 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i8, i8* %7, align 1
  ret i8 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local zeroext i8 @JNI_CallStaticBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !12

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 119
  %270 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  %275 = call zeroext i8 %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret i8 %275
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local signext i8 @JNI_CallByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8, align 1
  %8 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %10 = bitcast %struct.__va_list_tag* %9 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 41
  %14 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %19 = call signext i8 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %struct.__va_list_tag* noundef %18)
  store i8 %19, i8* %7, align 1
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %21 = bitcast %struct.__va_list_tag* %20 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i8, i8* %7, align 1
  ret i8 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local signext i8 @JNI_CallByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !13

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 42
  %270 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  %275 = call signext i8 %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret i8 %275
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local signext i8 @JNI_CallNonvirtualByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i8, align 1
  %10 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %11 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %12 = bitcast %struct.__va_list_tag* %11 to i8*
  call void @llvm.va_start(i8* %12)
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 71
  %16 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %15, align 8
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %18 = load %struct._jobject*, %struct._jobject** %6, align 8
  %19 = load %struct._jobject*, %struct._jobject** %7, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %21 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %22 = call signext i8 %16(%struct.JNINativeInterface_** noundef %17, %struct._jobject* noundef %18, %struct._jobject* noundef %19, %struct._jmethodID* noundef %20, %struct.__va_list_tag* noundef %21)
  store i8 %22, i8* %9, align 1
  %23 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %24 = bitcast %struct.__va_list_tag* %23 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load i8, i8* %9, align 1
  ret i8 %25
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local signext i8 @JNI_CallNonvirtualByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %struct.__va_list_tag* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca %struct.__va_list_tag*, align 8
  %11 = alloca %union.jvalue*, align 8
  %12 = alloca [257 x i8], align 16
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  store %struct.__va_list_tag* %4, %struct.__va_list_tag** %10, align 8
  br label %15

15:                                               ; preds = %5
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 8
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %22 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i64 0, i64 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %13, align 4
  %25 = load i32, i32* %13, align 4
  %26 = sext i32 %25 to i64
  %27 = mul i64 %26, 8
  %28 = alloca i8, i64 %27, align 16
  %29 = bitcast i8* %28 to %union.jvalue*
  store %union.jvalue* %29, %union.jvalue** %11, align 8
  store i32 0, i32* %14, align 4
  br label %30

30:                                               ; preds = %264, %15
  %31 = load i32, i32* %14, align 4
  %32 = load i32, i32* %13, align 4
  %33 = icmp slt i32 %31, %32
  br i1 %33, label %34, label %267

34:                                               ; preds = %30
  %35 = load i32, i32* %14, align 4
  %36 = sext i32 %35 to i64
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i64 0, i64 %36
  %38 = load i8, i8* %37, align 1
  %39 = sext i8 %38 to i32
  switch i32 %39, label %263 [
    i32 90, label %40
    i32 66, label %65
    i32 83, label %90
    i32 67, label %115
    i32 73, label %141
    i32 74, label %165
    i32 68, label %189
    i32 70, label %213
    i32 76, label %238
  ]

40:                                               ; preds = %34
  %41 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %42 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 0
  %43 = load i32, i32* %42, align 8
  %44 = icmp ule i32 %43, 40
  br i1 %44, label %45, label %51

45:                                               ; preds = %40
  %46 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 3
  %47 = load i8*, i8** %46, align 8
  %48 = getelementptr i8, i8* %47, i32 %43
  %49 = bitcast i8* %48 to i32*
  %50 = add i32 %43, 8
  store i32 %50, i32* %42, align 8
  br label %56

51:                                               ; preds = %40
  %52 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 2
  %53 = load i8*, i8** %52, align 8
  %54 = bitcast i8* %53 to i32*
  %55 = getelementptr i8, i8* %53, i32 8
  store i8* %55, i8** %52, align 8
  br label %56

56:                                               ; preds = %51, %45
  %57 = phi i32* [ %49, %45 ], [ %54, %51 ]
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %11, align 8
  %61 = load i32, i32* %14, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %59, i8* %64, align 8
  br label %263

65:                                               ; preds = %34
  %66 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %67 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 0
  %68 = load i32, i32* %67, align 8
  %69 = icmp ule i32 %68, 40
  br i1 %69, label %70, label %76

70:                                               ; preds = %65
  %71 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 3
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr i8, i8* %72, i32 %68
  %74 = bitcast i8* %73 to i32*
  %75 = add i32 %68, 8
  store i32 %75, i32* %67, align 8
  br label %81

76:                                               ; preds = %65
  %77 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 2
  %78 = load i8*, i8** %77, align 8
  %79 = bitcast i8* %78 to i32*
  %80 = getelementptr i8, i8* %78, i32 8
  store i8* %80, i8** %77, align 8
  br label %81

81:                                               ; preds = %76, %70
  %82 = phi i32* [ %74, %70 ], [ %79, %76 ]
  %83 = load i32, i32* %82, align 4
  %84 = trunc i32 %83 to i8
  %85 = load %union.jvalue*, %union.jvalue** %11, align 8
  %86 = load i32, i32* %14, align 4
  %87 = sext i32 %86 to i64
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %85, i64 %87
  %89 = bitcast %union.jvalue* %88 to i8*
  store i8 %84, i8* %89, align 8
  br label %263

90:                                               ; preds = %34
  %91 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %92 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 0
  %93 = load i32, i32* %92, align 8
  %94 = icmp ule i32 %93, 40
  br i1 %94, label %95, label %101

95:                                               ; preds = %90
  %96 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 3
  %97 = load i8*, i8** %96, align 8
  %98 = getelementptr i8, i8* %97, i32 %93
  %99 = bitcast i8* %98 to i32*
  %100 = add i32 %93, 8
  store i32 %100, i32* %92, align 8
  br label %106

101:                                              ; preds = %90
  %102 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 2
  %103 = load i8*, i8** %102, align 8
  %104 = bitcast i8* %103 to i32*
  %105 = getelementptr i8, i8* %103, i32 8
  store i8* %105, i8** %102, align 8
  br label %106

106:                                              ; preds = %101, %95
  %107 = phi i32* [ %99, %95 ], [ %104, %101 ]
  %108 = load i32, i32* %107, align 4
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %11, align 8
  %111 = load i32, i32* %14, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %263

115:                                              ; preds = %34
  %116 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %117 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 0
  %118 = load i32, i32* %117, align 8
  %119 = icmp ule i32 %118, 40
  br i1 %119, label %120, label %126

120:                                              ; preds = %115
  %121 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 3
  %122 = load i8*, i8** %121, align 8
  %123 = getelementptr i8, i8* %122, i32 %118
  %124 = bitcast i8* %123 to i32*
  %125 = add i32 %118, 8
  store i32 %125, i32* %117, align 8
  br label %131

126:                                              ; preds = %115
  %127 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 2
  %128 = load i8*, i8** %127, align 8
  %129 = bitcast i8* %128 to i32*
  %130 = getelementptr i8, i8* %128, i32 8
  store i8* %130, i8** %127, align 8
  br label %131

131:                                              ; preds = %126, %120
  %132 = phi i32* [ %124, %120 ], [ %129, %126 ]
  %133 = load i32, i32* %132, align 4
  %134 = trunc i32 %133 to i16
  %135 = zext i16 %134 to i32
  %136 = load %union.jvalue*, %union.jvalue** %11, align 8
  %137 = load i32, i32* %14, align 4
  %138 = sext i32 %137 to i64
  %139 = getelementptr inbounds %union.jvalue, %union.jvalue* %136, i64 %138
  %140 = bitcast %union.jvalue* %139 to i32*
  store i32 %135, i32* %140, align 8
  br label %263

141:                                              ; preds = %34
  %142 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %143 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 0
  %144 = load i32, i32* %143, align 8
  %145 = icmp ule i32 %144, 40
  br i1 %145, label %146, label %152

146:                                              ; preds = %141
  %147 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 3
  %148 = load i8*, i8** %147, align 8
  %149 = getelementptr i8, i8* %148, i32 %144
  %150 = bitcast i8* %149 to i32*
  %151 = add i32 %144, 8
  store i32 %151, i32* %143, align 8
  br label %157

152:                                              ; preds = %141
  %153 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 2
  %154 = load i8*, i8** %153, align 8
  %155 = bitcast i8* %154 to i32*
  %156 = getelementptr i8, i8* %154, i32 8
  store i8* %156, i8** %153, align 8
  br label %157

157:                                              ; preds = %152, %146
  %158 = phi i32* [ %150, %146 ], [ %155, %152 ]
  %159 = load i32, i32* %158, align 4
  %160 = load %union.jvalue*, %union.jvalue** %11, align 8
  %161 = load i32, i32* %14, align 4
  %162 = sext i32 %161 to i64
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %160, i64 %162
  %164 = bitcast %union.jvalue* %163 to i32*
  store i32 %159, i32* %164, align 8
  br label %263

165:                                              ; preds = %34
  %166 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %167 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 0
  %168 = load i32, i32* %167, align 8
  %169 = icmp ule i32 %168, 40
  br i1 %169, label %170, label %176

170:                                              ; preds = %165
  %171 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 3
  %172 = load i8*, i8** %171, align 8
  %173 = getelementptr i8, i8* %172, i32 %168
  %174 = bitcast i8* %173 to i64*
  %175 = add i32 %168, 8
  store i32 %175, i32* %167, align 8
  br label %181

176:                                              ; preds = %165
  %177 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 2
  %178 = load i8*, i8** %177, align 8
  %179 = bitcast i8* %178 to i64*
  %180 = getelementptr i8, i8* %178, i32 8
  store i8* %180, i8** %177, align 8
  br label %181

181:                                              ; preds = %176, %170
  %182 = phi i64* [ %174, %170 ], [ %179, %176 ]
  %183 = load i64, i64* %182, align 8
  %184 = load %union.jvalue*, %union.jvalue** %11, align 8
  %185 = load i32, i32* %14, align 4
  %186 = sext i32 %185 to i64
  %187 = getelementptr inbounds %union.jvalue, %union.jvalue* %184, i64 %186
  %188 = bitcast %union.jvalue* %187 to i64*
  store i64 %183, i64* %188, align 8
  br label %263

189:                                              ; preds = %34
  %190 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %191 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 1
  %192 = load i32, i32* %191, align 4
  %193 = icmp ule i32 %192, 160
  br i1 %193, label %194, label %200

194:                                              ; preds = %189
  %195 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 3
  %196 = load i8*, i8** %195, align 8
  %197 = getelementptr i8, i8* %196, i32 %192
  %198 = bitcast i8* %197 to double*
  %199 = add i32 %192, 16
  store i32 %199, i32* %191, align 4
  br label %205

200:                                              ; preds = %189
  %201 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 2
  %202 = load i8*, i8** %201, align 8
  %203 = bitcast i8* %202 to double*
  %204 = getelementptr i8, i8* %202, i32 8
  store i8* %204, i8** %201, align 8
  br label %205

205:                                              ; preds = %200, %194
  %206 = phi double* [ %198, %194 ], [ %203, %200 ]
  %207 = load double, double* %206, align 8
  %208 = load %union.jvalue*, %union.jvalue** %11, align 8
  %209 = load i32, i32* %14, align 4
  %210 = sext i32 %209 to i64
  %211 = getelementptr inbounds %union.jvalue, %union.jvalue* %208, i64 %210
  %212 = bitcast %union.jvalue* %211 to double*
  store double %207, double* %212, align 8
  br label %263

213:                                              ; preds = %34
  %214 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %215 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 1
  %216 = load i32, i32* %215, align 4
  %217 = icmp ule i32 %216, 160
  br i1 %217, label %218, label %224

218:                                              ; preds = %213
  %219 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 3
  %220 = load i8*, i8** %219, align 8
  %221 = getelementptr i8, i8* %220, i32 %216
  %222 = bitcast i8* %221 to double*
  %223 = add i32 %216, 16
  store i32 %223, i32* %215, align 4
  br label %229

224:                                              ; preds = %213
  %225 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = bitcast i8* %226 to double*
  %228 = getelementptr i8, i8* %226, i32 8
  store i8* %228, i8** %225, align 8
  br label %229

229:                                              ; preds = %224, %218
  %230 = phi double* [ %222, %218 ], [ %227, %224 ]
  %231 = load double, double* %230, align 8
  %232 = fptrunc double %231 to float
  %233 = load %union.jvalue*, %union.jvalue** %11, align 8
  %234 = load i32, i32* %14, align 4
  %235 = sext i32 %234 to i64
  %236 = getelementptr inbounds %union.jvalue, %union.jvalue* %233, i64 %235
  %237 = bitcast %union.jvalue* %236 to float*
  store float %232, float* %237, align 8
  br label %263

238:                                              ; preds = %34
  %239 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %240 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 0
  %241 = load i32, i32* %240, align 8
  %242 = icmp ule i32 %241, 40
  br i1 %242, label %243, label %249

243:                                              ; preds = %238
  %244 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 3
  %245 = load i8*, i8** %244, align 8
  %246 = getelementptr i8, i8* %245, i32 %241
  %247 = bitcast i8* %246 to i8**
  %248 = add i32 %241, 8
  store i32 %248, i32* %240, align 8
  br label %254

249:                                              ; preds = %238
  %250 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 2
  %251 = load i8*, i8** %250, align 8
  %252 = bitcast i8* %251 to i8**
  %253 = getelementptr i8, i8* %251, i32 8
  store i8* %253, i8** %250, align 8
  br label %254

254:                                              ; preds = %249, %243
  %255 = phi i8** [ %247, %243 ], [ %252, %249 ]
  %256 = load i8*, i8** %255, align 8
  %257 = bitcast i8* %256 to %struct._jobject*
  %258 = load %union.jvalue*, %union.jvalue** %11, align 8
  %259 = load i32, i32* %14, align 4
  %260 = sext i32 %259 to i64
  %261 = getelementptr inbounds %union.jvalue, %union.jvalue* %258, i64 %260
  %262 = bitcast %union.jvalue* %261 to %struct._jobject**
  store %struct._jobject* %257, %struct._jobject** %262, align 8
  br label %263

263:                                              ; preds = %34, %254, %229, %205, %181, %157, %131, %106, %81, %56
  br label %264

264:                                              ; preds = %263
  %265 = load i32, i32* %14, align 4
  %266 = add nsw i32 %265, 1
  store i32 %266, i32* %14, align 4
  br label %30, !llvm.loop !14

267:                                              ; preds = %30
  br label %268

268:                                              ; preds = %267
  %269 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %270 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %269, align 8
  %271 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %270, i32 0, i32 72
  %272 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %271, align 8
  %273 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %274 = load %struct._jobject*, %struct._jobject** %7, align 8
  %275 = load %struct._jobject*, %struct._jobject** %8, align 8
  %276 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %277 = load %union.jvalue*, %union.jvalue** %11, align 8
  %278 = call signext i8 %272(%struct.JNINativeInterface_** noundef %273, %struct._jobject* noundef %274, %struct._jobject* noundef %275, %struct._jmethodID* noundef %276, %union.jvalue* noundef %277)
  ret i8 %278
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local signext i8 @JNI_CallStaticByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8, align 1
  %8 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %10 = bitcast %struct.__va_list_tag* %9 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 121
  %14 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %19 = call signext i8 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %struct.__va_list_tag* noundef %18)
  store i8 %19, i8* %7, align 1
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %21 = bitcast %struct.__va_list_tag* %20 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i8, i8* %7, align 1
  ret i8 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local signext i8 @JNI_CallStaticByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !15

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 122
  %270 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  %275 = call signext i8 %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret i8 %275
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local zeroext i16 @JNI_CallCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i16, align 2
  %8 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %10 = bitcast %struct.__va_list_tag* %9 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 44
  %14 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %19 = call zeroext i16 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %struct.__va_list_tag* noundef %18)
  store i16 %19, i16* %7, align 2
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %21 = bitcast %struct.__va_list_tag* %20 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i16, i16* %7, align 2
  ret i16 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local zeroext i16 @JNI_CallCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !16

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 45
  %270 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  %275 = call zeroext i16 %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret i16 %275
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local zeroext i16 @JNI_CallNonvirtualCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i16, align 2
  %10 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %11 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %12 = bitcast %struct.__va_list_tag* %11 to i8*
  call void @llvm.va_start(i8* %12)
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 74
  %16 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %15, align 8
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %18 = load %struct._jobject*, %struct._jobject** %6, align 8
  %19 = load %struct._jobject*, %struct._jobject** %7, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %21 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %22 = call zeroext i16 %16(%struct.JNINativeInterface_** noundef %17, %struct._jobject* noundef %18, %struct._jobject* noundef %19, %struct._jmethodID* noundef %20, %struct.__va_list_tag* noundef %21)
  store i16 %22, i16* %9, align 2
  %23 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %24 = bitcast %struct.__va_list_tag* %23 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load i16, i16* %9, align 2
  ret i16 %25
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local zeroext i16 @JNI_CallNonvirtualCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %struct.__va_list_tag* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca %struct.__va_list_tag*, align 8
  %11 = alloca %union.jvalue*, align 8
  %12 = alloca [257 x i8], align 16
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  store %struct.__va_list_tag* %4, %struct.__va_list_tag** %10, align 8
  br label %15

15:                                               ; preds = %5
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 8
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %22 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i64 0, i64 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %13, align 4
  %25 = load i32, i32* %13, align 4
  %26 = sext i32 %25 to i64
  %27 = mul i64 %26, 8
  %28 = alloca i8, i64 %27, align 16
  %29 = bitcast i8* %28 to %union.jvalue*
  store %union.jvalue* %29, %union.jvalue** %11, align 8
  store i32 0, i32* %14, align 4
  br label %30

30:                                               ; preds = %264, %15
  %31 = load i32, i32* %14, align 4
  %32 = load i32, i32* %13, align 4
  %33 = icmp slt i32 %31, %32
  br i1 %33, label %34, label %267

34:                                               ; preds = %30
  %35 = load i32, i32* %14, align 4
  %36 = sext i32 %35 to i64
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i64 0, i64 %36
  %38 = load i8, i8* %37, align 1
  %39 = sext i8 %38 to i32
  switch i32 %39, label %263 [
    i32 90, label %40
    i32 66, label %65
    i32 83, label %90
    i32 67, label %115
    i32 73, label %141
    i32 74, label %165
    i32 68, label %189
    i32 70, label %213
    i32 76, label %238
  ]

40:                                               ; preds = %34
  %41 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %42 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 0
  %43 = load i32, i32* %42, align 8
  %44 = icmp ule i32 %43, 40
  br i1 %44, label %45, label %51

45:                                               ; preds = %40
  %46 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 3
  %47 = load i8*, i8** %46, align 8
  %48 = getelementptr i8, i8* %47, i32 %43
  %49 = bitcast i8* %48 to i32*
  %50 = add i32 %43, 8
  store i32 %50, i32* %42, align 8
  br label %56

51:                                               ; preds = %40
  %52 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 2
  %53 = load i8*, i8** %52, align 8
  %54 = bitcast i8* %53 to i32*
  %55 = getelementptr i8, i8* %53, i32 8
  store i8* %55, i8** %52, align 8
  br label %56

56:                                               ; preds = %51, %45
  %57 = phi i32* [ %49, %45 ], [ %54, %51 ]
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %11, align 8
  %61 = load i32, i32* %14, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %59, i8* %64, align 8
  br label %263

65:                                               ; preds = %34
  %66 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %67 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 0
  %68 = load i32, i32* %67, align 8
  %69 = icmp ule i32 %68, 40
  br i1 %69, label %70, label %76

70:                                               ; preds = %65
  %71 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 3
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr i8, i8* %72, i32 %68
  %74 = bitcast i8* %73 to i32*
  %75 = add i32 %68, 8
  store i32 %75, i32* %67, align 8
  br label %81

76:                                               ; preds = %65
  %77 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 2
  %78 = load i8*, i8** %77, align 8
  %79 = bitcast i8* %78 to i32*
  %80 = getelementptr i8, i8* %78, i32 8
  store i8* %80, i8** %77, align 8
  br label %81

81:                                               ; preds = %76, %70
  %82 = phi i32* [ %74, %70 ], [ %79, %76 ]
  %83 = load i32, i32* %82, align 4
  %84 = trunc i32 %83 to i8
  %85 = load %union.jvalue*, %union.jvalue** %11, align 8
  %86 = load i32, i32* %14, align 4
  %87 = sext i32 %86 to i64
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %85, i64 %87
  %89 = bitcast %union.jvalue* %88 to i8*
  store i8 %84, i8* %89, align 8
  br label %263

90:                                               ; preds = %34
  %91 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %92 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 0
  %93 = load i32, i32* %92, align 8
  %94 = icmp ule i32 %93, 40
  br i1 %94, label %95, label %101

95:                                               ; preds = %90
  %96 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 3
  %97 = load i8*, i8** %96, align 8
  %98 = getelementptr i8, i8* %97, i32 %93
  %99 = bitcast i8* %98 to i32*
  %100 = add i32 %93, 8
  store i32 %100, i32* %92, align 8
  br label %106

101:                                              ; preds = %90
  %102 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 2
  %103 = load i8*, i8** %102, align 8
  %104 = bitcast i8* %103 to i32*
  %105 = getelementptr i8, i8* %103, i32 8
  store i8* %105, i8** %102, align 8
  br label %106

106:                                              ; preds = %101, %95
  %107 = phi i32* [ %99, %95 ], [ %104, %101 ]
  %108 = load i32, i32* %107, align 4
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %11, align 8
  %111 = load i32, i32* %14, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %263

115:                                              ; preds = %34
  %116 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %117 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 0
  %118 = load i32, i32* %117, align 8
  %119 = icmp ule i32 %118, 40
  br i1 %119, label %120, label %126

120:                                              ; preds = %115
  %121 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 3
  %122 = load i8*, i8** %121, align 8
  %123 = getelementptr i8, i8* %122, i32 %118
  %124 = bitcast i8* %123 to i32*
  %125 = add i32 %118, 8
  store i32 %125, i32* %117, align 8
  br label %131

126:                                              ; preds = %115
  %127 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 2
  %128 = load i8*, i8** %127, align 8
  %129 = bitcast i8* %128 to i32*
  %130 = getelementptr i8, i8* %128, i32 8
  store i8* %130, i8** %127, align 8
  br label %131

131:                                              ; preds = %126, %120
  %132 = phi i32* [ %124, %120 ], [ %129, %126 ]
  %133 = load i32, i32* %132, align 4
  %134 = trunc i32 %133 to i16
  %135 = zext i16 %134 to i32
  %136 = load %union.jvalue*, %union.jvalue** %11, align 8
  %137 = load i32, i32* %14, align 4
  %138 = sext i32 %137 to i64
  %139 = getelementptr inbounds %union.jvalue, %union.jvalue* %136, i64 %138
  %140 = bitcast %union.jvalue* %139 to i32*
  store i32 %135, i32* %140, align 8
  br label %263

141:                                              ; preds = %34
  %142 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %143 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 0
  %144 = load i32, i32* %143, align 8
  %145 = icmp ule i32 %144, 40
  br i1 %145, label %146, label %152

146:                                              ; preds = %141
  %147 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 3
  %148 = load i8*, i8** %147, align 8
  %149 = getelementptr i8, i8* %148, i32 %144
  %150 = bitcast i8* %149 to i32*
  %151 = add i32 %144, 8
  store i32 %151, i32* %143, align 8
  br label %157

152:                                              ; preds = %141
  %153 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 2
  %154 = load i8*, i8** %153, align 8
  %155 = bitcast i8* %154 to i32*
  %156 = getelementptr i8, i8* %154, i32 8
  store i8* %156, i8** %153, align 8
  br label %157

157:                                              ; preds = %152, %146
  %158 = phi i32* [ %150, %146 ], [ %155, %152 ]
  %159 = load i32, i32* %158, align 4
  %160 = load %union.jvalue*, %union.jvalue** %11, align 8
  %161 = load i32, i32* %14, align 4
  %162 = sext i32 %161 to i64
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %160, i64 %162
  %164 = bitcast %union.jvalue* %163 to i32*
  store i32 %159, i32* %164, align 8
  br label %263

165:                                              ; preds = %34
  %166 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %167 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 0
  %168 = load i32, i32* %167, align 8
  %169 = icmp ule i32 %168, 40
  br i1 %169, label %170, label %176

170:                                              ; preds = %165
  %171 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 3
  %172 = load i8*, i8** %171, align 8
  %173 = getelementptr i8, i8* %172, i32 %168
  %174 = bitcast i8* %173 to i64*
  %175 = add i32 %168, 8
  store i32 %175, i32* %167, align 8
  br label %181

176:                                              ; preds = %165
  %177 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 2
  %178 = load i8*, i8** %177, align 8
  %179 = bitcast i8* %178 to i64*
  %180 = getelementptr i8, i8* %178, i32 8
  store i8* %180, i8** %177, align 8
  br label %181

181:                                              ; preds = %176, %170
  %182 = phi i64* [ %174, %170 ], [ %179, %176 ]
  %183 = load i64, i64* %182, align 8
  %184 = load %union.jvalue*, %union.jvalue** %11, align 8
  %185 = load i32, i32* %14, align 4
  %186 = sext i32 %185 to i64
  %187 = getelementptr inbounds %union.jvalue, %union.jvalue* %184, i64 %186
  %188 = bitcast %union.jvalue* %187 to i64*
  store i64 %183, i64* %188, align 8
  br label %263

189:                                              ; preds = %34
  %190 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %191 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 1
  %192 = load i32, i32* %191, align 4
  %193 = icmp ule i32 %192, 160
  br i1 %193, label %194, label %200

194:                                              ; preds = %189
  %195 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 3
  %196 = load i8*, i8** %195, align 8
  %197 = getelementptr i8, i8* %196, i32 %192
  %198 = bitcast i8* %197 to double*
  %199 = add i32 %192, 16
  store i32 %199, i32* %191, align 4
  br label %205

200:                                              ; preds = %189
  %201 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 2
  %202 = load i8*, i8** %201, align 8
  %203 = bitcast i8* %202 to double*
  %204 = getelementptr i8, i8* %202, i32 8
  store i8* %204, i8** %201, align 8
  br label %205

205:                                              ; preds = %200, %194
  %206 = phi double* [ %198, %194 ], [ %203, %200 ]
  %207 = load double, double* %206, align 8
  %208 = load %union.jvalue*, %union.jvalue** %11, align 8
  %209 = load i32, i32* %14, align 4
  %210 = sext i32 %209 to i64
  %211 = getelementptr inbounds %union.jvalue, %union.jvalue* %208, i64 %210
  %212 = bitcast %union.jvalue* %211 to double*
  store double %207, double* %212, align 8
  br label %263

213:                                              ; preds = %34
  %214 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %215 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 1
  %216 = load i32, i32* %215, align 4
  %217 = icmp ule i32 %216, 160
  br i1 %217, label %218, label %224

218:                                              ; preds = %213
  %219 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 3
  %220 = load i8*, i8** %219, align 8
  %221 = getelementptr i8, i8* %220, i32 %216
  %222 = bitcast i8* %221 to double*
  %223 = add i32 %216, 16
  store i32 %223, i32* %215, align 4
  br label %229

224:                                              ; preds = %213
  %225 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = bitcast i8* %226 to double*
  %228 = getelementptr i8, i8* %226, i32 8
  store i8* %228, i8** %225, align 8
  br label %229

229:                                              ; preds = %224, %218
  %230 = phi double* [ %222, %218 ], [ %227, %224 ]
  %231 = load double, double* %230, align 8
  %232 = fptrunc double %231 to float
  %233 = load %union.jvalue*, %union.jvalue** %11, align 8
  %234 = load i32, i32* %14, align 4
  %235 = sext i32 %234 to i64
  %236 = getelementptr inbounds %union.jvalue, %union.jvalue* %233, i64 %235
  %237 = bitcast %union.jvalue* %236 to float*
  store float %232, float* %237, align 8
  br label %263

238:                                              ; preds = %34
  %239 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %240 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 0
  %241 = load i32, i32* %240, align 8
  %242 = icmp ule i32 %241, 40
  br i1 %242, label %243, label %249

243:                                              ; preds = %238
  %244 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 3
  %245 = load i8*, i8** %244, align 8
  %246 = getelementptr i8, i8* %245, i32 %241
  %247 = bitcast i8* %246 to i8**
  %248 = add i32 %241, 8
  store i32 %248, i32* %240, align 8
  br label %254

249:                                              ; preds = %238
  %250 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 2
  %251 = load i8*, i8** %250, align 8
  %252 = bitcast i8* %251 to i8**
  %253 = getelementptr i8, i8* %251, i32 8
  store i8* %253, i8** %250, align 8
  br label %254

254:                                              ; preds = %249, %243
  %255 = phi i8** [ %247, %243 ], [ %252, %249 ]
  %256 = load i8*, i8** %255, align 8
  %257 = bitcast i8* %256 to %struct._jobject*
  %258 = load %union.jvalue*, %union.jvalue** %11, align 8
  %259 = load i32, i32* %14, align 4
  %260 = sext i32 %259 to i64
  %261 = getelementptr inbounds %union.jvalue, %union.jvalue* %258, i64 %260
  %262 = bitcast %union.jvalue* %261 to %struct._jobject**
  store %struct._jobject* %257, %struct._jobject** %262, align 8
  br label %263

263:                                              ; preds = %34, %254, %229, %205, %181, %157, %131, %106, %81, %56
  br label %264

264:                                              ; preds = %263
  %265 = load i32, i32* %14, align 4
  %266 = add nsw i32 %265, 1
  store i32 %266, i32* %14, align 4
  br label %30, !llvm.loop !17

267:                                              ; preds = %30
  br label %268

268:                                              ; preds = %267
  %269 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %270 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %269, align 8
  %271 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %270, i32 0, i32 75
  %272 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %271, align 8
  %273 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %274 = load %struct._jobject*, %struct._jobject** %7, align 8
  %275 = load %struct._jobject*, %struct._jobject** %8, align 8
  %276 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %277 = load %union.jvalue*, %union.jvalue** %11, align 8
  %278 = call zeroext i16 %272(%struct.JNINativeInterface_** noundef %273, %struct._jobject* noundef %274, %struct._jobject* noundef %275, %struct._jmethodID* noundef %276, %union.jvalue* noundef %277)
  ret i16 %278
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local zeroext i16 @JNI_CallStaticCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i16, align 2
  %8 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %10 = bitcast %struct.__va_list_tag* %9 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 124
  %14 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %19 = call zeroext i16 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %struct.__va_list_tag* noundef %18)
  store i16 %19, i16* %7, align 2
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %21 = bitcast %struct.__va_list_tag* %20 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i16, i16* %7, align 2
  ret i16 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local zeroext i16 @JNI_CallStaticCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !18

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 125
  %270 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  %275 = call zeroext i16 %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret i16 %275
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local signext i16 @JNI_CallShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i16, align 2
  %8 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %10 = bitcast %struct.__va_list_tag* %9 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 47
  %14 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %19 = call signext i16 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %struct.__va_list_tag* noundef %18)
  store i16 %19, i16* %7, align 2
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %21 = bitcast %struct.__va_list_tag* %20 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i16, i16* %7, align 2
  ret i16 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local signext i16 @JNI_CallShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !19

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 48
  %270 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  %275 = call signext i16 %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret i16 %275
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local signext i16 @JNI_CallNonvirtualShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i16, align 2
  %10 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %11 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %12 = bitcast %struct.__va_list_tag* %11 to i8*
  call void @llvm.va_start(i8* %12)
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 77
  %16 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %15, align 8
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %18 = load %struct._jobject*, %struct._jobject** %6, align 8
  %19 = load %struct._jobject*, %struct._jobject** %7, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %21 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %22 = call signext i16 %16(%struct.JNINativeInterface_** noundef %17, %struct._jobject* noundef %18, %struct._jobject* noundef %19, %struct._jmethodID* noundef %20, %struct.__va_list_tag* noundef %21)
  store i16 %22, i16* %9, align 2
  %23 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %24 = bitcast %struct.__va_list_tag* %23 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load i16, i16* %9, align 2
  ret i16 %25
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local signext i16 @JNI_CallNonvirtualShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %struct.__va_list_tag* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca %struct.__va_list_tag*, align 8
  %11 = alloca %union.jvalue*, align 8
  %12 = alloca [257 x i8], align 16
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  store %struct.__va_list_tag* %4, %struct.__va_list_tag** %10, align 8
  br label %15

15:                                               ; preds = %5
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 8
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %22 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i64 0, i64 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %13, align 4
  %25 = load i32, i32* %13, align 4
  %26 = sext i32 %25 to i64
  %27 = mul i64 %26, 8
  %28 = alloca i8, i64 %27, align 16
  %29 = bitcast i8* %28 to %union.jvalue*
  store %union.jvalue* %29, %union.jvalue** %11, align 8
  store i32 0, i32* %14, align 4
  br label %30

30:                                               ; preds = %264, %15
  %31 = load i32, i32* %14, align 4
  %32 = load i32, i32* %13, align 4
  %33 = icmp slt i32 %31, %32
  br i1 %33, label %34, label %267

34:                                               ; preds = %30
  %35 = load i32, i32* %14, align 4
  %36 = sext i32 %35 to i64
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i64 0, i64 %36
  %38 = load i8, i8* %37, align 1
  %39 = sext i8 %38 to i32
  switch i32 %39, label %263 [
    i32 90, label %40
    i32 66, label %65
    i32 83, label %90
    i32 67, label %115
    i32 73, label %141
    i32 74, label %165
    i32 68, label %189
    i32 70, label %213
    i32 76, label %238
  ]

40:                                               ; preds = %34
  %41 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %42 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 0
  %43 = load i32, i32* %42, align 8
  %44 = icmp ule i32 %43, 40
  br i1 %44, label %45, label %51

45:                                               ; preds = %40
  %46 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 3
  %47 = load i8*, i8** %46, align 8
  %48 = getelementptr i8, i8* %47, i32 %43
  %49 = bitcast i8* %48 to i32*
  %50 = add i32 %43, 8
  store i32 %50, i32* %42, align 8
  br label %56

51:                                               ; preds = %40
  %52 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 2
  %53 = load i8*, i8** %52, align 8
  %54 = bitcast i8* %53 to i32*
  %55 = getelementptr i8, i8* %53, i32 8
  store i8* %55, i8** %52, align 8
  br label %56

56:                                               ; preds = %51, %45
  %57 = phi i32* [ %49, %45 ], [ %54, %51 ]
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %11, align 8
  %61 = load i32, i32* %14, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %59, i8* %64, align 8
  br label %263

65:                                               ; preds = %34
  %66 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %67 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 0
  %68 = load i32, i32* %67, align 8
  %69 = icmp ule i32 %68, 40
  br i1 %69, label %70, label %76

70:                                               ; preds = %65
  %71 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 3
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr i8, i8* %72, i32 %68
  %74 = bitcast i8* %73 to i32*
  %75 = add i32 %68, 8
  store i32 %75, i32* %67, align 8
  br label %81

76:                                               ; preds = %65
  %77 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 2
  %78 = load i8*, i8** %77, align 8
  %79 = bitcast i8* %78 to i32*
  %80 = getelementptr i8, i8* %78, i32 8
  store i8* %80, i8** %77, align 8
  br label %81

81:                                               ; preds = %76, %70
  %82 = phi i32* [ %74, %70 ], [ %79, %76 ]
  %83 = load i32, i32* %82, align 4
  %84 = trunc i32 %83 to i8
  %85 = load %union.jvalue*, %union.jvalue** %11, align 8
  %86 = load i32, i32* %14, align 4
  %87 = sext i32 %86 to i64
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %85, i64 %87
  %89 = bitcast %union.jvalue* %88 to i8*
  store i8 %84, i8* %89, align 8
  br label %263

90:                                               ; preds = %34
  %91 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %92 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 0
  %93 = load i32, i32* %92, align 8
  %94 = icmp ule i32 %93, 40
  br i1 %94, label %95, label %101

95:                                               ; preds = %90
  %96 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 3
  %97 = load i8*, i8** %96, align 8
  %98 = getelementptr i8, i8* %97, i32 %93
  %99 = bitcast i8* %98 to i32*
  %100 = add i32 %93, 8
  store i32 %100, i32* %92, align 8
  br label %106

101:                                              ; preds = %90
  %102 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 2
  %103 = load i8*, i8** %102, align 8
  %104 = bitcast i8* %103 to i32*
  %105 = getelementptr i8, i8* %103, i32 8
  store i8* %105, i8** %102, align 8
  br label %106

106:                                              ; preds = %101, %95
  %107 = phi i32* [ %99, %95 ], [ %104, %101 ]
  %108 = load i32, i32* %107, align 4
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %11, align 8
  %111 = load i32, i32* %14, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %263

115:                                              ; preds = %34
  %116 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %117 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 0
  %118 = load i32, i32* %117, align 8
  %119 = icmp ule i32 %118, 40
  br i1 %119, label %120, label %126

120:                                              ; preds = %115
  %121 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 3
  %122 = load i8*, i8** %121, align 8
  %123 = getelementptr i8, i8* %122, i32 %118
  %124 = bitcast i8* %123 to i32*
  %125 = add i32 %118, 8
  store i32 %125, i32* %117, align 8
  br label %131

126:                                              ; preds = %115
  %127 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 2
  %128 = load i8*, i8** %127, align 8
  %129 = bitcast i8* %128 to i32*
  %130 = getelementptr i8, i8* %128, i32 8
  store i8* %130, i8** %127, align 8
  br label %131

131:                                              ; preds = %126, %120
  %132 = phi i32* [ %124, %120 ], [ %129, %126 ]
  %133 = load i32, i32* %132, align 4
  %134 = trunc i32 %133 to i16
  %135 = zext i16 %134 to i32
  %136 = load %union.jvalue*, %union.jvalue** %11, align 8
  %137 = load i32, i32* %14, align 4
  %138 = sext i32 %137 to i64
  %139 = getelementptr inbounds %union.jvalue, %union.jvalue* %136, i64 %138
  %140 = bitcast %union.jvalue* %139 to i32*
  store i32 %135, i32* %140, align 8
  br label %263

141:                                              ; preds = %34
  %142 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %143 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 0
  %144 = load i32, i32* %143, align 8
  %145 = icmp ule i32 %144, 40
  br i1 %145, label %146, label %152

146:                                              ; preds = %141
  %147 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 3
  %148 = load i8*, i8** %147, align 8
  %149 = getelementptr i8, i8* %148, i32 %144
  %150 = bitcast i8* %149 to i32*
  %151 = add i32 %144, 8
  store i32 %151, i32* %143, align 8
  br label %157

152:                                              ; preds = %141
  %153 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 2
  %154 = load i8*, i8** %153, align 8
  %155 = bitcast i8* %154 to i32*
  %156 = getelementptr i8, i8* %154, i32 8
  store i8* %156, i8** %153, align 8
  br label %157

157:                                              ; preds = %152, %146
  %158 = phi i32* [ %150, %146 ], [ %155, %152 ]
  %159 = load i32, i32* %158, align 4
  %160 = load %union.jvalue*, %union.jvalue** %11, align 8
  %161 = load i32, i32* %14, align 4
  %162 = sext i32 %161 to i64
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %160, i64 %162
  %164 = bitcast %union.jvalue* %163 to i32*
  store i32 %159, i32* %164, align 8
  br label %263

165:                                              ; preds = %34
  %166 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %167 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 0
  %168 = load i32, i32* %167, align 8
  %169 = icmp ule i32 %168, 40
  br i1 %169, label %170, label %176

170:                                              ; preds = %165
  %171 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 3
  %172 = load i8*, i8** %171, align 8
  %173 = getelementptr i8, i8* %172, i32 %168
  %174 = bitcast i8* %173 to i64*
  %175 = add i32 %168, 8
  store i32 %175, i32* %167, align 8
  br label %181

176:                                              ; preds = %165
  %177 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 2
  %178 = load i8*, i8** %177, align 8
  %179 = bitcast i8* %178 to i64*
  %180 = getelementptr i8, i8* %178, i32 8
  store i8* %180, i8** %177, align 8
  br label %181

181:                                              ; preds = %176, %170
  %182 = phi i64* [ %174, %170 ], [ %179, %176 ]
  %183 = load i64, i64* %182, align 8
  %184 = load %union.jvalue*, %union.jvalue** %11, align 8
  %185 = load i32, i32* %14, align 4
  %186 = sext i32 %185 to i64
  %187 = getelementptr inbounds %union.jvalue, %union.jvalue* %184, i64 %186
  %188 = bitcast %union.jvalue* %187 to i64*
  store i64 %183, i64* %188, align 8
  br label %263

189:                                              ; preds = %34
  %190 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %191 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 1
  %192 = load i32, i32* %191, align 4
  %193 = icmp ule i32 %192, 160
  br i1 %193, label %194, label %200

194:                                              ; preds = %189
  %195 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 3
  %196 = load i8*, i8** %195, align 8
  %197 = getelementptr i8, i8* %196, i32 %192
  %198 = bitcast i8* %197 to double*
  %199 = add i32 %192, 16
  store i32 %199, i32* %191, align 4
  br label %205

200:                                              ; preds = %189
  %201 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 2
  %202 = load i8*, i8** %201, align 8
  %203 = bitcast i8* %202 to double*
  %204 = getelementptr i8, i8* %202, i32 8
  store i8* %204, i8** %201, align 8
  br label %205

205:                                              ; preds = %200, %194
  %206 = phi double* [ %198, %194 ], [ %203, %200 ]
  %207 = load double, double* %206, align 8
  %208 = load %union.jvalue*, %union.jvalue** %11, align 8
  %209 = load i32, i32* %14, align 4
  %210 = sext i32 %209 to i64
  %211 = getelementptr inbounds %union.jvalue, %union.jvalue* %208, i64 %210
  %212 = bitcast %union.jvalue* %211 to double*
  store double %207, double* %212, align 8
  br label %263

213:                                              ; preds = %34
  %214 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %215 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 1
  %216 = load i32, i32* %215, align 4
  %217 = icmp ule i32 %216, 160
  br i1 %217, label %218, label %224

218:                                              ; preds = %213
  %219 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 3
  %220 = load i8*, i8** %219, align 8
  %221 = getelementptr i8, i8* %220, i32 %216
  %222 = bitcast i8* %221 to double*
  %223 = add i32 %216, 16
  store i32 %223, i32* %215, align 4
  br label %229

224:                                              ; preds = %213
  %225 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = bitcast i8* %226 to double*
  %228 = getelementptr i8, i8* %226, i32 8
  store i8* %228, i8** %225, align 8
  br label %229

229:                                              ; preds = %224, %218
  %230 = phi double* [ %222, %218 ], [ %227, %224 ]
  %231 = load double, double* %230, align 8
  %232 = fptrunc double %231 to float
  %233 = load %union.jvalue*, %union.jvalue** %11, align 8
  %234 = load i32, i32* %14, align 4
  %235 = sext i32 %234 to i64
  %236 = getelementptr inbounds %union.jvalue, %union.jvalue* %233, i64 %235
  %237 = bitcast %union.jvalue* %236 to float*
  store float %232, float* %237, align 8
  br label %263

238:                                              ; preds = %34
  %239 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %240 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 0
  %241 = load i32, i32* %240, align 8
  %242 = icmp ule i32 %241, 40
  br i1 %242, label %243, label %249

243:                                              ; preds = %238
  %244 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 3
  %245 = load i8*, i8** %244, align 8
  %246 = getelementptr i8, i8* %245, i32 %241
  %247 = bitcast i8* %246 to i8**
  %248 = add i32 %241, 8
  store i32 %248, i32* %240, align 8
  br label %254

249:                                              ; preds = %238
  %250 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 2
  %251 = load i8*, i8** %250, align 8
  %252 = bitcast i8* %251 to i8**
  %253 = getelementptr i8, i8* %251, i32 8
  store i8* %253, i8** %250, align 8
  br label %254

254:                                              ; preds = %249, %243
  %255 = phi i8** [ %247, %243 ], [ %252, %249 ]
  %256 = load i8*, i8** %255, align 8
  %257 = bitcast i8* %256 to %struct._jobject*
  %258 = load %union.jvalue*, %union.jvalue** %11, align 8
  %259 = load i32, i32* %14, align 4
  %260 = sext i32 %259 to i64
  %261 = getelementptr inbounds %union.jvalue, %union.jvalue* %258, i64 %260
  %262 = bitcast %union.jvalue* %261 to %struct._jobject**
  store %struct._jobject* %257, %struct._jobject** %262, align 8
  br label %263

263:                                              ; preds = %34, %254, %229, %205, %181, %157, %131, %106, %81, %56
  br label %264

264:                                              ; preds = %263
  %265 = load i32, i32* %14, align 4
  %266 = add nsw i32 %265, 1
  store i32 %266, i32* %14, align 4
  br label %30, !llvm.loop !20

267:                                              ; preds = %30
  br label %268

268:                                              ; preds = %267
  %269 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %270 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %269, align 8
  %271 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %270, i32 0, i32 78
  %272 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %271, align 8
  %273 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %274 = load %struct._jobject*, %struct._jobject** %7, align 8
  %275 = load %struct._jobject*, %struct._jobject** %8, align 8
  %276 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %277 = load %union.jvalue*, %union.jvalue** %11, align 8
  %278 = call signext i16 %272(%struct.JNINativeInterface_** noundef %273, %struct._jobject* noundef %274, %struct._jobject* noundef %275, %struct._jmethodID* noundef %276, %union.jvalue* noundef %277)
  ret i16 %278
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local signext i16 @JNI_CallStaticShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i16, align 2
  %8 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %10 = bitcast %struct.__va_list_tag* %9 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 127
  %14 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %19 = call signext i16 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %struct.__va_list_tag* noundef %18)
  store i16 %19, i16* %7, align 2
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %21 = bitcast %struct.__va_list_tag* %20 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i16, i16* %7, align 2
  ret i16 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local signext i16 @JNI_CallStaticShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !21

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 128
  %270 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  %275 = call signext i16 %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret i16 %275
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @JNI_CallIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i32, align 4
  %8 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %10 = bitcast %struct.__va_list_tag* %9 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 50
  %14 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %19 = call i32 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %struct.__va_list_tag* noundef %18)
  store i32 %19, i32* %7, align 4
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %21 = bitcast %struct.__va_list_tag* %20 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i32, i32* %7, align 4
  ret i32 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @JNI_CallIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !22

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 51
  %270 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  %275 = call i32 %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret i32 %275
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @JNI_CallNonvirtualIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i32, align 4
  %10 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %11 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %12 = bitcast %struct.__va_list_tag* %11 to i8*
  call void @llvm.va_start(i8* %12)
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 80
  %16 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %15, align 8
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %18 = load %struct._jobject*, %struct._jobject** %6, align 8
  %19 = load %struct._jobject*, %struct._jobject** %7, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %21 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %22 = call i32 %16(%struct.JNINativeInterface_** noundef %17, %struct._jobject* noundef %18, %struct._jobject* noundef %19, %struct._jmethodID* noundef %20, %struct.__va_list_tag* noundef %21)
  store i32 %22, i32* %9, align 4
  %23 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %24 = bitcast %struct.__va_list_tag* %23 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load i32, i32* %9, align 4
  ret i32 %25
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @JNI_CallNonvirtualIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %struct.__va_list_tag* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca %struct.__va_list_tag*, align 8
  %11 = alloca %union.jvalue*, align 8
  %12 = alloca [257 x i8], align 16
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  store %struct.__va_list_tag* %4, %struct.__va_list_tag** %10, align 8
  br label %15

15:                                               ; preds = %5
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 8
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %22 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i64 0, i64 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %13, align 4
  %25 = load i32, i32* %13, align 4
  %26 = sext i32 %25 to i64
  %27 = mul i64 %26, 8
  %28 = alloca i8, i64 %27, align 16
  %29 = bitcast i8* %28 to %union.jvalue*
  store %union.jvalue* %29, %union.jvalue** %11, align 8
  store i32 0, i32* %14, align 4
  br label %30

30:                                               ; preds = %264, %15
  %31 = load i32, i32* %14, align 4
  %32 = load i32, i32* %13, align 4
  %33 = icmp slt i32 %31, %32
  br i1 %33, label %34, label %267

34:                                               ; preds = %30
  %35 = load i32, i32* %14, align 4
  %36 = sext i32 %35 to i64
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i64 0, i64 %36
  %38 = load i8, i8* %37, align 1
  %39 = sext i8 %38 to i32
  switch i32 %39, label %263 [
    i32 90, label %40
    i32 66, label %65
    i32 83, label %90
    i32 67, label %115
    i32 73, label %141
    i32 74, label %165
    i32 68, label %189
    i32 70, label %213
    i32 76, label %238
  ]

40:                                               ; preds = %34
  %41 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %42 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 0
  %43 = load i32, i32* %42, align 8
  %44 = icmp ule i32 %43, 40
  br i1 %44, label %45, label %51

45:                                               ; preds = %40
  %46 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 3
  %47 = load i8*, i8** %46, align 8
  %48 = getelementptr i8, i8* %47, i32 %43
  %49 = bitcast i8* %48 to i32*
  %50 = add i32 %43, 8
  store i32 %50, i32* %42, align 8
  br label %56

51:                                               ; preds = %40
  %52 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 2
  %53 = load i8*, i8** %52, align 8
  %54 = bitcast i8* %53 to i32*
  %55 = getelementptr i8, i8* %53, i32 8
  store i8* %55, i8** %52, align 8
  br label %56

56:                                               ; preds = %51, %45
  %57 = phi i32* [ %49, %45 ], [ %54, %51 ]
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %11, align 8
  %61 = load i32, i32* %14, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %59, i8* %64, align 8
  br label %263

65:                                               ; preds = %34
  %66 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %67 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 0
  %68 = load i32, i32* %67, align 8
  %69 = icmp ule i32 %68, 40
  br i1 %69, label %70, label %76

70:                                               ; preds = %65
  %71 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 3
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr i8, i8* %72, i32 %68
  %74 = bitcast i8* %73 to i32*
  %75 = add i32 %68, 8
  store i32 %75, i32* %67, align 8
  br label %81

76:                                               ; preds = %65
  %77 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 2
  %78 = load i8*, i8** %77, align 8
  %79 = bitcast i8* %78 to i32*
  %80 = getelementptr i8, i8* %78, i32 8
  store i8* %80, i8** %77, align 8
  br label %81

81:                                               ; preds = %76, %70
  %82 = phi i32* [ %74, %70 ], [ %79, %76 ]
  %83 = load i32, i32* %82, align 4
  %84 = trunc i32 %83 to i8
  %85 = load %union.jvalue*, %union.jvalue** %11, align 8
  %86 = load i32, i32* %14, align 4
  %87 = sext i32 %86 to i64
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %85, i64 %87
  %89 = bitcast %union.jvalue* %88 to i8*
  store i8 %84, i8* %89, align 8
  br label %263

90:                                               ; preds = %34
  %91 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %92 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 0
  %93 = load i32, i32* %92, align 8
  %94 = icmp ule i32 %93, 40
  br i1 %94, label %95, label %101

95:                                               ; preds = %90
  %96 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 3
  %97 = load i8*, i8** %96, align 8
  %98 = getelementptr i8, i8* %97, i32 %93
  %99 = bitcast i8* %98 to i32*
  %100 = add i32 %93, 8
  store i32 %100, i32* %92, align 8
  br label %106

101:                                              ; preds = %90
  %102 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 2
  %103 = load i8*, i8** %102, align 8
  %104 = bitcast i8* %103 to i32*
  %105 = getelementptr i8, i8* %103, i32 8
  store i8* %105, i8** %102, align 8
  br label %106

106:                                              ; preds = %101, %95
  %107 = phi i32* [ %99, %95 ], [ %104, %101 ]
  %108 = load i32, i32* %107, align 4
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %11, align 8
  %111 = load i32, i32* %14, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %263

115:                                              ; preds = %34
  %116 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %117 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 0
  %118 = load i32, i32* %117, align 8
  %119 = icmp ule i32 %118, 40
  br i1 %119, label %120, label %126

120:                                              ; preds = %115
  %121 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 3
  %122 = load i8*, i8** %121, align 8
  %123 = getelementptr i8, i8* %122, i32 %118
  %124 = bitcast i8* %123 to i32*
  %125 = add i32 %118, 8
  store i32 %125, i32* %117, align 8
  br label %131

126:                                              ; preds = %115
  %127 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 2
  %128 = load i8*, i8** %127, align 8
  %129 = bitcast i8* %128 to i32*
  %130 = getelementptr i8, i8* %128, i32 8
  store i8* %130, i8** %127, align 8
  br label %131

131:                                              ; preds = %126, %120
  %132 = phi i32* [ %124, %120 ], [ %129, %126 ]
  %133 = load i32, i32* %132, align 4
  %134 = trunc i32 %133 to i16
  %135 = zext i16 %134 to i32
  %136 = load %union.jvalue*, %union.jvalue** %11, align 8
  %137 = load i32, i32* %14, align 4
  %138 = sext i32 %137 to i64
  %139 = getelementptr inbounds %union.jvalue, %union.jvalue* %136, i64 %138
  %140 = bitcast %union.jvalue* %139 to i32*
  store i32 %135, i32* %140, align 8
  br label %263

141:                                              ; preds = %34
  %142 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %143 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 0
  %144 = load i32, i32* %143, align 8
  %145 = icmp ule i32 %144, 40
  br i1 %145, label %146, label %152

146:                                              ; preds = %141
  %147 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 3
  %148 = load i8*, i8** %147, align 8
  %149 = getelementptr i8, i8* %148, i32 %144
  %150 = bitcast i8* %149 to i32*
  %151 = add i32 %144, 8
  store i32 %151, i32* %143, align 8
  br label %157

152:                                              ; preds = %141
  %153 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 2
  %154 = load i8*, i8** %153, align 8
  %155 = bitcast i8* %154 to i32*
  %156 = getelementptr i8, i8* %154, i32 8
  store i8* %156, i8** %153, align 8
  br label %157

157:                                              ; preds = %152, %146
  %158 = phi i32* [ %150, %146 ], [ %155, %152 ]
  %159 = load i32, i32* %158, align 4
  %160 = load %union.jvalue*, %union.jvalue** %11, align 8
  %161 = load i32, i32* %14, align 4
  %162 = sext i32 %161 to i64
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %160, i64 %162
  %164 = bitcast %union.jvalue* %163 to i32*
  store i32 %159, i32* %164, align 8
  br label %263

165:                                              ; preds = %34
  %166 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %167 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 0
  %168 = load i32, i32* %167, align 8
  %169 = icmp ule i32 %168, 40
  br i1 %169, label %170, label %176

170:                                              ; preds = %165
  %171 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 3
  %172 = load i8*, i8** %171, align 8
  %173 = getelementptr i8, i8* %172, i32 %168
  %174 = bitcast i8* %173 to i64*
  %175 = add i32 %168, 8
  store i32 %175, i32* %167, align 8
  br label %181

176:                                              ; preds = %165
  %177 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 2
  %178 = load i8*, i8** %177, align 8
  %179 = bitcast i8* %178 to i64*
  %180 = getelementptr i8, i8* %178, i32 8
  store i8* %180, i8** %177, align 8
  br label %181

181:                                              ; preds = %176, %170
  %182 = phi i64* [ %174, %170 ], [ %179, %176 ]
  %183 = load i64, i64* %182, align 8
  %184 = load %union.jvalue*, %union.jvalue** %11, align 8
  %185 = load i32, i32* %14, align 4
  %186 = sext i32 %185 to i64
  %187 = getelementptr inbounds %union.jvalue, %union.jvalue* %184, i64 %186
  %188 = bitcast %union.jvalue* %187 to i64*
  store i64 %183, i64* %188, align 8
  br label %263

189:                                              ; preds = %34
  %190 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %191 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 1
  %192 = load i32, i32* %191, align 4
  %193 = icmp ule i32 %192, 160
  br i1 %193, label %194, label %200

194:                                              ; preds = %189
  %195 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 3
  %196 = load i8*, i8** %195, align 8
  %197 = getelementptr i8, i8* %196, i32 %192
  %198 = bitcast i8* %197 to double*
  %199 = add i32 %192, 16
  store i32 %199, i32* %191, align 4
  br label %205

200:                                              ; preds = %189
  %201 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 2
  %202 = load i8*, i8** %201, align 8
  %203 = bitcast i8* %202 to double*
  %204 = getelementptr i8, i8* %202, i32 8
  store i8* %204, i8** %201, align 8
  br label %205

205:                                              ; preds = %200, %194
  %206 = phi double* [ %198, %194 ], [ %203, %200 ]
  %207 = load double, double* %206, align 8
  %208 = load %union.jvalue*, %union.jvalue** %11, align 8
  %209 = load i32, i32* %14, align 4
  %210 = sext i32 %209 to i64
  %211 = getelementptr inbounds %union.jvalue, %union.jvalue* %208, i64 %210
  %212 = bitcast %union.jvalue* %211 to double*
  store double %207, double* %212, align 8
  br label %263

213:                                              ; preds = %34
  %214 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %215 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 1
  %216 = load i32, i32* %215, align 4
  %217 = icmp ule i32 %216, 160
  br i1 %217, label %218, label %224

218:                                              ; preds = %213
  %219 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 3
  %220 = load i8*, i8** %219, align 8
  %221 = getelementptr i8, i8* %220, i32 %216
  %222 = bitcast i8* %221 to double*
  %223 = add i32 %216, 16
  store i32 %223, i32* %215, align 4
  br label %229

224:                                              ; preds = %213
  %225 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = bitcast i8* %226 to double*
  %228 = getelementptr i8, i8* %226, i32 8
  store i8* %228, i8** %225, align 8
  br label %229

229:                                              ; preds = %224, %218
  %230 = phi double* [ %222, %218 ], [ %227, %224 ]
  %231 = load double, double* %230, align 8
  %232 = fptrunc double %231 to float
  %233 = load %union.jvalue*, %union.jvalue** %11, align 8
  %234 = load i32, i32* %14, align 4
  %235 = sext i32 %234 to i64
  %236 = getelementptr inbounds %union.jvalue, %union.jvalue* %233, i64 %235
  %237 = bitcast %union.jvalue* %236 to float*
  store float %232, float* %237, align 8
  br label %263

238:                                              ; preds = %34
  %239 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %240 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 0
  %241 = load i32, i32* %240, align 8
  %242 = icmp ule i32 %241, 40
  br i1 %242, label %243, label %249

243:                                              ; preds = %238
  %244 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 3
  %245 = load i8*, i8** %244, align 8
  %246 = getelementptr i8, i8* %245, i32 %241
  %247 = bitcast i8* %246 to i8**
  %248 = add i32 %241, 8
  store i32 %248, i32* %240, align 8
  br label %254

249:                                              ; preds = %238
  %250 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 2
  %251 = load i8*, i8** %250, align 8
  %252 = bitcast i8* %251 to i8**
  %253 = getelementptr i8, i8* %251, i32 8
  store i8* %253, i8** %250, align 8
  br label %254

254:                                              ; preds = %249, %243
  %255 = phi i8** [ %247, %243 ], [ %252, %249 ]
  %256 = load i8*, i8** %255, align 8
  %257 = bitcast i8* %256 to %struct._jobject*
  %258 = load %union.jvalue*, %union.jvalue** %11, align 8
  %259 = load i32, i32* %14, align 4
  %260 = sext i32 %259 to i64
  %261 = getelementptr inbounds %union.jvalue, %union.jvalue* %258, i64 %260
  %262 = bitcast %union.jvalue* %261 to %struct._jobject**
  store %struct._jobject* %257, %struct._jobject** %262, align 8
  br label %263

263:                                              ; preds = %34, %254, %229, %205, %181, %157, %131, %106, %81, %56
  br label %264

264:                                              ; preds = %263
  %265 = load i32, i32* %14, align 4
  %266 = add nsw i32 %265, 1
  store i32 %266, i32* %14, align 4
  br label %30, !llvm.loop !23

267:                                              ; preds = %30
  br label %268

268:                                              ; preds = %267
  %269 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %270 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %269, align 8
  %271 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %270, i32 0, i32 81
  %272 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %271, align 8
  %273 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %274 = load %struct._jobject*, %struct._jobject** %7, align 8
  %275 = load %struct._jobject*, %struct._jobject** %8, align 8
  %276 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %277 = load %union.jvalue*, %union.jvalue** %11, align 8
  %278 = call i32 %272(%struct.JNINativeInterface_** noundef %273, %struct._jobject* noundef %274, %struct._jobject* noundef %275, %struct._jmethodID* noundef %276, %union.jvalue* noundef %277)
  ret i32 %278
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @JNI_CallStaticIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i32, align 4
  %8 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %10 = bitcast %struct.__va_list_tag* %9 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 130
  %14 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %19 = call i32 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %struct.__va_list_tag* noundef %18)
  store i32 %19, i32* %7, align 4
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %21 = bitcast %struct.__va_list_tag* %20 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i32, i32* %7, align 4
  ret i32 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @JNI_CallStaticIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !24

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 131
  %270 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  %275 = call i32 %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret i32 %275
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i64 @JNI_CallLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i64, align 8
  %8 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %10 = bitcast %struct.__va_list_tag* %9 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 53
  %14 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %19 = call i64 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %struct.__va_list_tag* noundef %18)
  store i64 %19, i64* %7, align 8
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %21 = bitcast %struct.__va_list_tag* %20 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i64, i64* %7, align 8
  ret i64 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i64 @JNI_CallLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !25

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 54
  %270 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  %275 = call i64 %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret i64 %275
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i64 @JNI_CallNonvirtualLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i64, align 8
  %10 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %11 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %12 = bitcast %struct.__va_list_tag* %11 to i8*
  call void @llvm.va_start(i8* %12)
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 83
  %16 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %15, align 8
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %18 = load %struct._jobject*, %struct._jobject** %6, align 8
  %19 = load %struct._jobject*, %struct._jobject** %7, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %21 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %22 = call i64 %16(%struct.JNINativeInterface_** noundef %17, %struct._jobject* noundef %18, %struct._jobject* noundef %19, %struct._jmethodID* noundef %20, %struct.__va_list_tag* noundef %21)
  store i64 %22, i64* %9, align 8
  %23 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %24 = bitcast %struct.__va_list_tag* %23 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load i64, i64* %9, align 8
  ret i64 %25
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i64 @JNI_CallNonvirtualLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %struct.__va_list_tag* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca %struct.__va_list_tag*, align 8
  %11 = alloca %union.jvalue*, align 8
  %12 = alloca [257 x i8], align 16
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  store %struct.__va_list_tag* %4, %struct.__va_list_tag** %10, align 8
  br label %15

15:                                               ; preds = %5
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 8
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %22 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i64 0, i64 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %13, align 4
  %25 = load i32, i32* %13, align 4
  %26 = sext i32 %25 to i64
  %27 = mul i64 %26, 8
  %28 = alloca i8, i64 %27, align 16
  %29 = bitcast i8* %28 to %union.jvalue*
  store %union.jvalue* %29, %union.jvalue** %11, align 8
  store i32 0, i32* %14, align 4
  br label %30

30:                                               ; preds = %264, %15
  %31 = load i32, i32* %14, align 4
  %32 = load i32, i32* %13, align 4
  %33 = icmp slt i32 %31, %32
  br i1 %33, label %34, label %267

34:                                               ; preds = %30
  %35 = load i32, i32* %14, align 4
  %36 = sext i32 %35 to i64
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i64 0, i64 %36
  %38 = load i8, i8* %37, align 1
  %39 = sext i8 %38 to i32
  switch i32 %39, label %263 [
    i32 90, label %40
    i32 66, label %65
    i32 83, label %90
    i32 67, label %115
    i32 73, label %141
    i32 74, label %165
    i32 68, label %189
    i32 70, label %213
    i32 76, label %238
  ]

40:                                               ; preds = %34
  %41 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %42 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 0
  %43 = load i32, i32* %42, align 8
  %44 = icmp ule i32 %43, 40
  br i1 %44, label %45, label %51

45:                                               ; preds = %40
  %46 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 3
  %47 = load i8*, i8** %46, align 8
  %48 = getelementptr i8, i8* %47, i32 %43
  %49 = bitcast i8* %48 to i32*
  %50 = add i32 %43, 8
  store i32 %50, i32* %42, align 8
  br label %56

51:                                               ; preds = %40
  %52 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 2
  %53 = load i8*, i8** %52, align 8
  %54 = bitcast i8* %53 to i32*
  %55 = getelementptr i8, i8* %53, i32 8
  store i8* %55, i8** %52, align 8
  br label %56

56:                                               ; preds = %51, %45
  %57 = phi i32* [ %49, %45 ], [ %54, %51 ]
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %11, align 8
  %61 = load i32, i32* %14, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %59, i8* %64, align 8
  br label %263

65:                                               ; preds = %34
  %66 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %67 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 0
  %68 = load i32, i32* %67, align 8
  %69 = icmp ule i32 %68, 40
  br i1 %69, label %70, label %76

70:                                               ; preds = %65
  %71 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 3
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr i8, i8* %72, i32 %68
  %74 = bitcast i8* %73 to i32*
  %75 = add i32 %68, 8
  store i32 %75, i32* %67, align 8
  br label %81

76:                                               ; preds = %65
  %77 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 2
  %78 = load i8*, i8** %77, align 8
  %79 = bitcast i8* %78 to i32*
  %80 = getelementptr i8, i8* %78, i32 8
  store i8* %80, i8** %77, align 8
  br label %81

81:                                               ; preds = %76, %70
  %82 = phi i32* [ %74, %70 ], [ %79, %76 ]
  %83 = load i32, i32* %82, align 4
  %84 = trunc i32 %83 to i8
  %85 = load %union.jvalue*, %union.jvalue** %11, align 8
  %86 = load i32, i32* %14, align 4
  %87 = sext i32 %86 to i64
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %85, i64 %87
  %89 = bitcast %union.jvalue* %88 to i8*
  store i8 %84, i8* %89, align 8
  br label %263

90:                                               ; preds = %34
  %91 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %92 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 0
  %93 = load i32, i32* %92, align 8
  %94 = icmp ule i32 %93, 40
  br i1 %94, label %95, label %101

95:                                               ; preds = %90
  %96 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 3
  %97 = load i8*, i8** %96, align 8
  %98 = getelementptr i8, i8* %97, i32 %93
  %99 = bitcast i8* %98 to i32*
  %100 = add i32 %93, 8
  store i32 %100, i32* %92, align 8
  br label %106

101:                                              ; preds = %90
  %102 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 2
  %103 = load i8*, i8** %102, align 8
  %104 = bitcast i8* %103 to i32*
  %105 = getelementptr i8, i8* %103, i32 8
  store i8* %105, i8** %102, align 8
  br label %106

106:                                              ; preds = %101, %95
  %107 = phi i32* [ %99, %95 ], [ %104, %101 ]
  %108 = load i32, i32* %107, align 4
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %11, align 8
  %111 = load i32, i32* %14, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %263

115:                                              ; preds = %34
  %116 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %117 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 0
  %118 = load i32, i32* %117, align 8
  %119 = icmp ule i32 %118, 40
  br i1 %119, label %120, label %126

120:                                              ; preds = %115
  %121 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 3
  %122 = load i8*, i8** %121, align 8
  %123 = getelementptr i8, i8* %122, i32 %118
  %124 = bitcast i8* %123 to i32*
  %125 = add i32 %118, 8
  store i32 %125, i32* %117, align 8
  br label %131

126:                                              ; preds = %115
  %127 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 2
  %128 = load i8*, i8** %127, align 8
  %129 = bitcast i8* %128 to i32*
  %130 = getelementptr i8, i8* %128, i32 8
  store i8* %130, i8** %127, align 8
  br label %131

131:                                              ; preds = %126, %120
  %132 = phi i32* [ %124, %120 ], [ %129, %126 ]
  %133 = load i32, i32* %132, align 4
  %134 = trunc i32 %133 to i16
  %135 = zext i16 %134 to i32
  %136 = load %union.jvalue*, %union.jvalue** %11, align 8
  %137 = load i32, i32* %14, align 4
  %138 = sext i32 %137 to i64
  %139 = getelementptr inbounds %union.jvalue, %union.jvalue* %136, i64 %138
  %140 = bitcast %union.jvalue* %139 to i32*
  store i32 %135, i32* %140, align 8
  br label %263

141:                                              ; preds = %34
  %142 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %143 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 0
  %144 = load i32, i32* %143, align 8
  %145 = icmp ule i32 %144, 40
  br i1 %145, label %146, label %152

146:                                              ; preds = %141
  %147 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 3
  %148 = load i8*, i8** %147, align 8
  %149 = getelementptr i8, i8* %148, i32 %144
  %150 = bitcast i8* %149 to i32*
  %151 = add i32 %144, 8
  store i32 %151, i32* %143, align 8
  br label %157

152:                                              ; preds = %141
  %153 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 2
  %154 = load i8*, i8** %153, align 8
  %155 = bitcast i8* %154 to i32*
  %156 = getelementptr i8, i8* %154, i32 8
  store i8* %156, i8** %153, align 8
  br label %157

157:                                              ; preds = %152, %146
  %158 = phi i32* [ %150, %146 ], [ %155, %152 ]
  %159 = load i32, i32* %158, align 4
  %160 = load %union.jvalue*, %union.jvalue** %11, align 8
  %161 = load i32, i32* %14, align 4
  %162 = sext i32 %161 to i64
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %160, i64 %162
  %164 = bitcast %union.jvalue* %163 to i32*
  store i32 %159, i32* %164, align 8
  br label %263

165:                                              ; preds = %34
  %166 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %167 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 0
  %168 = load i32, i32* %167, align 8
  %169 = icmp ule i32 %168, 40
  br i1 %169, label %170, label %176

170:                                              ; preds = %165
  %171 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 3
  %172 = load i8*, i8** %171, align 8
  %173 = getelementptr i8, i8* %172, i32 %168
  %174 = bitcast i8* %173 to i64*
  %175 = add i32 %168, 8
  store i32 %175, i32* %167, align 8
  br label %181

176:                                              ; preds = %165
  %177 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 2
  %178 = load i8*, i8** %177, align 8
  %179 = bitcast i8* %178 to i64*
  %180 = getelementptr i8, i8* %178, i32 8
  store i8* %180, i8** %177, align 8
  br label %181

181:                                              ; preds = %176, %170
  %182 = phi i64* [ %174, %170 ], [ %179, %176 ]
  %183 = load i64, i64* %182, align 8
  %184 = load %union.jvalue*, %union.jvalue** %11, align 8
  %185 = load i32, i32* %14, align 4
  %186 = sext i32 %185 to i64
  %187 = getelementptr inbounds %union.jvalue, %union.jvalue* %184, i64 %186
  %188 = bitcast %union.jvalue* %187 to i64*
  store i64 %183, i64* %188, align 8
  br label %263

189:                                              ; preds = %34
  %190 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %191 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 1
  %192 = load i32, i32* %191, align 4
  %193 = icmp ule i32 %192, 160
  br i1 %193, label %194, label %200

194:                                              ; preds = %189
  %195 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 3
  %196 = load i8*, i8** %195, align 8
  %197 = getelementptr i8, i8* %196, i32 %192
  %198 = bitcast i8* %197 to double*
  %199 = add i32 %192, 16
  store i32 %199, i32* %191, align 4
  br label %205

200:                                              ; preds = %189
  %201 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 2
  %202 = load i8*, i8** %201, align 8
  %203 = bitcast i8* %202 to double*
  %204 = getelementptr i8, i8* %202, i32 8
  store i8* %204, i8** %201, align 8
  br label %205

205:                                              ; preds = %200, %194
  %206 = phi double* [ %198, %194 ], [ %203, %200 ]
  %207 = load double, double* %206, align 8
  %208 = load %union.jvalue*, %union.jvalue** %11, align 8
  %209 = load i32, i32* %14, align 4
  %210 = sext i32 %209 to i64
  %211 = getelementptr inbounds %union.jvalue, %union.jvalue* %208, i64 %210
  %212 = bitcast %union.jvalue* %211 to double*
  store double %207, double* %212, align 8
  br label %263

213:                                              ; preds = %34
  %214 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %215 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 1
  %216 = load i32, i32* %215, align 4
  %217 = icmp ule i32 %216, 160
  br i1 %217, label %218, label %224

218:                                              ; preds = %213
  %219 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 3
  %220 = load i8*, i8** %219, align 8
  %221 = getelementptr i8, i8* %220, i32 %216
  %222 = bitcast i8* %221 to double*
  %223 = add i32 %216, 16
  store i32 %223, i32* %215, align 4
  br label %229

224:                                              ; preds = %213
  %225 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = bitcast i8* %226 to double*
  %228 = getelementptr i8, i8* %226, i32 8
  store i8* %228, i8** %225, align 8
  br label %229

229:                                              ; preds = %224, %218
  %230 = phi double* [ %222, %218 ], [ %227, %224 ]
  %231 = load double, double* %230, align 8
  %232 = fptrunc double %231 to float
  %233 = load %union.jvalue*, %union.jvalue** %11, align 8
  %234 = load i32, i32* %14, align 4
  %235 = sext i32 %234 to i64
  %236 = getelementptr inbounds %union.jvalue, %union.jvalue* %233, i64 %235
  %237 = bitcast %union.jvalue* %236 to float*
  store float %232, float* %237, align 8
  br label %263

238:                                              ; preds = %34
  %239 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %240 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 0
  %241 = load i32, i32* %240, align 8
  %242 = icmp ule i32 %241, 40
  br i1 %242, label %243, label %249

243:                                              ; preds = %238
  %244 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 3
  %245 = load i8*, i8** %244, align 8
  %246 = getelementptr i8, i8* %245, i32 %241
  %247 = bitcast i8* %246 to i8**
  %248 = add i32 %241, 8
  store i32 %248, i32* %240, align 8
  br label %254

249:                                              ; preds = %238
  %250 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 2
  %251 = load i8*, i8** %250, align 8
  %252 = bitcast i8* %251 to i8**
  %253 = getelementptr i8, i8* %251, i32 8
  store i8* %253, i8** %250, align 8
  br label %254

254:                                              ; preds = %249, %243
  %255 = phi i8** [ %247, %243 ], [ %252, %249 ]
  %256 = load i8*, i8** %255, align 8
  %257 = bitcast i8* %256 to %struct._jobject*
  %258 = load %union.jvalue*, %union.jvalue** %11, align 8
  %259 = load i32, i32* %14, align 4
  %260 = sext i32 %259 to i64
  %261 = getelementptr inbounds %union.jvalue, %union.jvalue* %258, i64 %260
  %262 = bitcast %union.jvalue* %261 to %struct._jobject**
  store %struct._jobject* %257, %struct._jobject** %262, align 8
  br label %263

263:                                              ; preds = %34, %254, %229, %205, %181, %157, %131, %106, %81, %56
  br label %264

264:                                              ; preds = %263
  %265 = load i32, i32* %14, align 4
  %266 = add nsw i32 %265, 1
  store i32 %266, i32* %14, align 4
  br label %30, !llvm.loop !26

267:                                              ; preds = %30
  br label %268

268:                                              ; preds = %267
  %269 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %270 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %269, align 8
  %271 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %270, i32 0, i32 84
  %272 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %271, align 8
  %273 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %274 = load %struct._jobject*, %struct._jobject** %7, align 8
  %275 = load %struct._jobject*, %struct._jobject** %8, align 8
  %276 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %277 = load %union.jvalue*, %union.jvalue** %11, align 8
  %278 = call i64 %272(%struct.JNINativeInterface_** noundef %273, %struct._jobject* noundef %274, %struct._jobject* noundef %275, %struct._jmethodID* noundef %276, %union.jvalue* noundef %277)
  ret i64 %278
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i64 @JNI_CallStaticLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i64, align 8
  %8 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %10 = bitcast %struct.__va_list_tag* %9 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 133
  %14 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %19 = call i64 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %struct.__va_list_tag* noundef %18)
  store i64 %19, i64* %7, align 8
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %21 = bitcast %struct.__va_list_tag* %20 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i64, i64* %7, align 8
  ret i64 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i64 @JNI_CallStaticLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !27

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 134
  %270 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  %275 = call i64 %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret i64 %275
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local float @JNI_CallFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca float, align 4
  %8 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %10 = bitcast %struct.__va_list_tag* %9 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 56
  %14 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %19 = call float %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %struct.__va_list_tag* noundef %18)
  store float %19, float* %7, align 4
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %21 = bitcast %struct.__va_list_tag* %20 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load float, float* %7, align 4
  ret float %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local float @JNI_CallFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !28

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 57
  %270 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  %275 = call float %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret float %275
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local float @JNI_CallNonvirtualFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca float, align 4
  %10 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %11 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %12 = bitcast %struct.__va_list_tag* %11 to i8*
  call void @llvm.va_start(i8* %12)
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 86
  %16 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %15, align 8
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %18 = load %struct._jobject*, %struct._jobject** %6, align 8
  %19 = load %struct._jobject*, %struct._jobject** %7, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %21 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %22 = call float %16(%struct.JNINativeInterface_** noundef %17, %struct._jobject* noundef %18, %struct._jobject* noundef %19, %struct._jmethodID* noundef %20, %struct.__va_list_tag* noundef %21)
  store float %22, float* %9, align 4
  %23 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %24 = bitcast %struct.__va_list_tag* %23 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load float, float* %9, align 4
  ret float %25
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local float @JNI_CallNonvirtualFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %struct.__va_list_tag* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca %struct.__va_list_tag*, align 8
  %11 = alloca %union.jvalue*, align 8
  %12 = alloca [257 x i8], align 16
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  store %struct.__va_list_tag* %4, %struct.__va_list_tag** %10, align 8
  br label %15

15:                                               ; preds = %5
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 8
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %22 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i64 0, i64 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %13, align 4
  %25 = load i32, i32* %13, align 4
  %26 = sext i32 %25 to i64
  %27 = mul i64 %26, 8
  %28 = alloca i8, i64 %27, align 16
  %29 = bitcast i8* %28 to %union.jvalue*
  store %union.jvalue* %29, %union.jvalue** %11, align 8
  store i32 0, i32* %14, align 4
  br label %30

30:                                               ; preds = %264, %15
  %31 = load i32, i32* %14, align 4
  %32 = load i32, i32* %13, align 4
  %33 = icmp slt i32 %31, %32
  br i1 %33, label %34, label %267

34:                                               ; preds = %30
  %35 = load i32, i32* %14, align 4
  %36 = sext i32 %35 to i64
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i64 0, i64 %36
  %38 = load i8, i8* %37, align 1
  %39 = sext i8 %38 to i32
  switch i32 %39, label %263 [
    i32 90, label %40
    i32 66, label %65
    i32 83, label %90
    i32 67, label %115
    i32 73, label %141
    i32 74, label %165
    i32 68, label %189
    i32 70, label %213
    i32 76, label %238
  ]

40:                                               ; preds = %34
  %41 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %42 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 0
  %43 = load i32, i32* %42, align 8
  %44 = icmp ule i32 %43, 40
  br i1 %44, label %45, label %51

45:                                               ; preds = %40
  %46 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 3
  %47 = load i8*, i8** %46, align 8
  %48 = getelementptr i8, i8* %47, i32 %43
  %49 = bitcast i8* %48 to i32*
  %50 = add i32 %43, 8
  store i32 %50, i32* %42, align 8
  br label %56

51:                                               ; preds = %40
  %52 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 2
  %53 = load i8*, i8** %52, align 8
  %54 = bitcast i8* %53 to i32*
  %55 = getelementptr i8, i8* %53, i32 8
  store i8* %55, i8** %52, align 8
  br label %56

56:                                               ; preds = %51, %45
  %57 = phi i32* [ %49, %45 ], [ %54, %51 ]
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %11, align 8
  %61 = load i32, i32* %14, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %59, i8* %64, align 8
  br label %263

65:                                               ; preds = %34
  %66 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %67 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 0
  %68 = load i32, i32* %67, align 8
  %69 = icmp ule i32 %68, 40
  br i1 %69, label %70, label %76

70:                                               ; preds = %65
  %71 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 3
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr i8, i8* %72, i32 %68
  %74 = bitcast i8* %73 to i32*
  %75 = add i32 %68, 8
  store i32 %75, i32* %67, align 8
  br label %81

76:                                               ; preds = %65
  %77 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 2
  %78 = load i8*, i8** %77, align 8
  %79 = bitcast i8* %78 to i32*
  %80 = getelementptr i8, i8* %78, i32 8
  store i8* %80, i8** %77, align 8
  br label %81

81:                                               ; preds = %76, %70
  %82 = phi i32* [ %74, %70 ], [ %79, %76 ]
  %83 = load i32, i32* %82, align 4
  %84 = trunc i32 %83 to i8
  %85 = load %union.jvalue*, %union.jvalue** %11, align 8
  %86 = load i32, i32* %14, align 4
  %87 = sext i32 %86 to i64
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %85, i64 %87
  %89 = bitcast %union.jvalue* %88 to i8*
  store i8 %84, i8* %89, align 8
  br label %263

90:                                               ; preds = %34
  %91 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %92 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 0
  %93 = load i32, i32* %92, align 8
  %94 = icmp ule i32 %93, 40
  br i1 %94, label %95, label %101

95:                                               ; preds = %90
  %96 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 3
  %97 = load i8*, i8** %96, align 8
  %98 = getelementptr i8, i8* %97, i32 %93
  %99 = bitcast i8* %98 to i32*
  %100 = add i32 %93, 8
  store i32 %100, i32* %92, align 8
  br label %106

101:                                              ; preds = %90
  %102 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 2
  %103 = load i8*, i8** %102, align 8
  %104 = bitcast i8* %103 to i32*
  %105 = getelementptr i8, i8* %103, i32 8
  store i8* %105, i8** %102, align 8
  br label %106

106:                                              ; preds = %101, %95
  %107 = phi i32* [ %99, %95 ], [ %104, %101 ]
  %108 = load i32, i32* %107, align 4
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %11, align 8
  %111 = load i32, i32* %14, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %263

115:                                              ; preds = %34
  %116 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %117 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 0
  %118 = load i32, i32* %117, align 8
  %119 = icmp ule i32 %118, 40
  br i1 %119, label %120, label %126

120:                                              ; preds = %115
  %121 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 3
  %122 = load i8*, i8** %121, align 8
  %123 = getelementptr i8, i8* %122, i32 %118
  %124 = bitcast i8* %123 to i32*
  %125 = add i32 %118, 8
  store i32 %125, i32* %117, align 8
  br label %131

126:                                              ; preds = %115
  %127 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 2
  %128 = load i8*, i8** %127, align 8
  %129 = bitcast i8* %128 to i32*
  %130 = getelementptr i8, i8* %128, i32 8
  store i8* %130, i8** %127, align 8
  br label %131

131:                                              ; preds = %126, %120
  %132 = phi i32* [ %124, %120 ], [ %129, %126 ]
  %133 = load i32, i32* %132, align 4
  %134 = trunc i32 %133 to i16
  %135 = zext i16 %134 to i32
  %136 = load %union.jvalue*, %union.jvalue** %11, align 8
  %137 = load i32, i32* %14, align 4
  %138 = sext i32 %137 to i64
  %139 = getelementptr inbounds %union.jvalue, %union.jvalue* %136, i64 %138
  %140 = bitcast %union.jvalue* %139 to i32*
  store i32 %135, i32* %140, align 8
  br label %263

141:                                              ; preds = %34
  %142 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %143 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 0
  %144 = load i32, i32* %143, align 8
  %145 = icmp ule i32 %144, 40
  br i1 %145, label %146, label %152

146:                                              ; preds = %141
  %147 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 3
  %148 = load i8*, i8** %147, align 8
  %149 = getelementptr i8, i8* %148, i32 %144
  %150 = bitcast i8* %149 to i32*
  %151 = add i32 %144, 8
  store i32 %151, i32* %143, align 8
  br label %157

152:                                              ; preds = %141
  %153 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 2
  %154 = load i8*, i8** %153, align 8
  %155 = bitcast i8* %154 to i32*
  %156 = getelementptr i8, i8* %154, i32 8
  store i8* %156, i8** %153, align 8
  br label %157

157:                                              ; preds = %152, %146
  %158 = phi i32* [ %150, %146 ], [ %155, %152 ]
  %159 = load i32, i32* %158, align 4
  %160 = load %union.jvalue*, %union.jvalue** %11, align 8
  %161 = load i32, i32* %14, align 4
  %162 = sext i32 %161 to i64
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %160, i64 %162
  %164 = bitcast %union.jvalue* %163 to i32*
  store i32 %159, i32* %164, align 8
  br label %263

165:                                              ; preds = %34
  %166 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %167 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 0
  %168 = load i32, i32* %167, align 8
  %169 = icmp ule i32 %168, 40
  br i1 %169, label %170, label %176

170:                                              ; preds = %165
  %171 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 3
  %172 = load i8*, i8** %171, align 8
  %173 = getelementptr i8, i8* %172, i32 %168
  %174 = bitcast i8* %173 to i64*
  %175 = add i32 %168, 8
  store i32 %175, i32* %167, align 8
  br label %181

176:                                              ; preds = %165
  %177 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 2
  %178 = load i8*, i8** %177, align 8
  %179 = bitcast i8* %178 to i64*
  %180 = getelementptr i8, i8* %178, i32 8
  store i8* %180, i8** %177, align 8
  br label %181

181:                                              ; preds = %176, %170
  %182 = phi i64* [ %174, %170 ], [ %179, %176 ]
  %183 = load i64, i64* %182, align 8
  %184 = load %union.jvalue*, %union.jvalue** %11, align 8
  %185 = load i32, i32* %14, align 4
  %186 = sext i32 %185 to i64
  %187 = getelementptr inbounds %union.jvalue, %union.jvalue* %184, i64 %186
  %188 = bitcast %union.jvalue* %187 to i64*
  store i64 %183, i64* %188, align 8
  br label %263

189:                                              ; preds = %34
  %190 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %191 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 1
  %192 = load i32, i32* %191, align 4
  %193 = icmp ule i32 %192, 160
  br i1 %193, label %194, label %200

194:                                              ; preds = %189
  %195 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 3
  %196 = load i8*, i8** %195, align 8
  %197 = getelementptr i8, i8* %196, i32 %192
  %198 = bitcast i8* %197 to double*
  %199 = add i32 %192, 16
  store i32 %199, i32* %191, align 4
  br label %205

200:                                              ; preds = %189
  %201 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 2
  %202 = load i8*, i8** %201, align 8
  %203 = bitcast i8* %202 to double*
  %204 = getelementptr i8, i8* %202, i32 8
  store i8* %204, i8** %201, align 8
  br label %205

205:                                              ; preds = %200, %194
  %206 = phi double* [ %198, %194 ], [ %203, %200 ]
  %207 = load double, double* %206, align 8
  %208 = load %union.jvalue*, %union.jvalue** %11, align 8
  %209 = load i32, i32* %14, align 4
  %210 = sext i32 %209 to i64
  %211 = getelementptr inbounds %union.jvalue, %union.jvalue* %208, i64 %210
  %212 = bitcast %union.jvalue* %211 to double*
  store double %207, double* %212, align 8
  br label %263

213:                                              ; preds = %34
  %214 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %215 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 1
  %216 = load i32, i32* %215, align 4
  %217 = icmp ule i32 %216, 160
  br i1 %217, label %218, label %224

218:                                              ; preds = %213
  %219 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 3
  %220 = load i8*, i8** %219, align 8
  %221 = getelementptr i8, i8* %220, i32 %216
  %222 = bitcast i8* %221 to double*
  %223 = add i32 %216, 16
  store i32 %223, i32* %215, align 4
  br label %229

224:                                              ; preds = %213
  %225 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = bitcast i8* %226 to double*
  %228 = getelementptr i8, i8* %226, i32 8
  store i8* %228, i8** %225, align 8
  br label %229

229:                                              ; preds = %224, %218
  %230 = phi double* [ %222, %218 ], [ %227, %224 ]
  %231 = load double, double* %230, align 8
  %232 = fptrunc double %231 to float
  %233 = load %union.jvalue*, %union.jvalue** %11, align 8
  %234 = load i32, i32* %14, align 4
  %235 = sext i32 %234 to i64
  %236 = getelementptr inbounds %union.jvalue, %union.jvalue* %233, i64 %235
  %237 = bitcast %union.jvalue* %236 to float*
  store float %232, float* %237, align 8
  br label %263

238:                                              ; preds = %34
  %239 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %240 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 0
  %241 = load i32, i32* %240, align 8
  %242 = icmp ule i32 %241, 40
  br i1 %242, label %243, label %249

243:                                              ; preds = %238
  %244 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 3
  %245 = load i8*, i8** %244, align 8
  %246 = getelementptr i8, i8* %245, i32 %241
  %247 = bitcast i8* %246 to i8**
  %248 = add i32 %241, 8
  store i32 %248, i32* %240, align 8
  br label %254

249:                                              ; preds = %238
  %250 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 2
  %251 = load i8*, i8** %250, align 8
  %252 = bitcast i8* %251 to i8**
  %253 = getelementptr i8, i8* %251, i32 8
  store i8* %253, i8** %250, align 8
  br label %254

254:                                              ; preds = %249, %243
  %255 = phi i8** [ %247, %243 ], [ %252, %249 ]
  %256 = load i8*, i8** %255, align 8
  %257 = bitcast i8* %256 to %struct._jobject*
  %258 = load %union.jvalue*, %union.jvalue** %11, align 8
  %259 = load i32, i32* %14, align 4
  %260 = sext i32 %259 to i64
  %261 = getelementptr inbounds %union.jvalue, %union.jvalue* %258, i64 %260
  %262 = bitcast %union.jvalue* %261 to %struct._jobject**
  store %struct._jobject* %257, %struct._jobject** %262, align 8
  br label %263

263:                                              ; preds = %34, %254, %229, %205, %181, %157, %131, %106, %81, %56
  br label %264

264:                                              ; preds = %263
  %265 = load i32, i32* %14, align 4
  %266 = add nsw i32 %265, 1
  store i32 %266, i32* %14, align 4
  br label %30, !llvm.loop !29

267:                                              ; preds = %30
  br label %268

268:                                              ; preds = %267
  %269 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %270 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %269, align 8
  %271 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %270, i32 0, i32 87
  %272 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %271, align 8
  %273 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %274 = load %struct._jobject*, %struct._jobject** %7, align 8
  %275 = load %struct._jobject*, %struct._jobject** %8, align 8
  %276 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %277 = load %union.jvalue*, %union.jvalue** %11, align 8
  %278 = call float %272(%struct.JNINativeInterface_** noundef %273, %struct._jobject* noundef %274, %struct._jobject* noundef %275, %struct._jmethodID* noundef %276, %union.jvalue* noundef %277)
  ret float %278
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local float @JNI_CallStaticFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca float, align 4
  %8 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %10 = bitcast %struct.__va_list_tag* %9 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 136
  %14 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %19 = call float %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %struct.__va_list_tag* noundef %18)
  store float %19, float* %7, align 4
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %21 = bitcast %struct.__va_list_tag* %20 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load float, float* %7, align 4
  ret float %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local float @JNI_CallStaticFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !30

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 137
  %270 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  %275 = call float %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret float %275
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local double @JNI_CallDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca double, align 8
  %8 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %10 = bitcast %struct.__va_list_tag* %9 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 59
  %14 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %19 = call double %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %struct.__va_list_tag* noundef %18)
  store double %19, double* %7, align 8
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %21 = bitcast %struct.__va_list_tag* %20 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load double, double* %7, align 8
  ret double %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local double @JNI_CallDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !31

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 60
  %270 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  %275 = call double %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret double %275
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local double @JNI_CallNonvirtualDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca double, align 8
  %10 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %11 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %12 = bitcast %struct.__va_list_tag* %11 to i8*
  call void @llvm.va_start(i8* %12)
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 89
  %16 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %15, align 8
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %18 = load %struct._jobject*, %struct._jobject** %6, align 8
  %19 = load %struct._jobject*, %struct._jobject** %7, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %21 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %22 = call double %16(%struct.JNINativeInterface_** noundef %17, %struct._jobject* noundef %18, %struct._jobject* noundef %19, %struct._jmethodID* noundef %20, %struct.__va_list_tag* noundef %21)
  store double %22, double* %9, align 8
  %23 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %10, i64 0, i64 0
  %24 = bitcast %struct.__va_list_tag* %23 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load double, double* %9, align 8
  ret double %25
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local double @JNI_CallNonvirtualDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %struct.__va_list_tag* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca %struct.__va_list_tag*, align 8
  %11 = alloca %union.jvalue*, align 8
  %12 = alloca [257 x i8], align 16
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  store %struct.__va_list_tag* %4, %struct.__va_list_tag** %10, align 8
  br label %15

15:                                               ; preds = %5
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 8
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %22 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i64 0, i64 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %13, align 4
  %25 = load i32, i32* %13, align 4
  %26 = sext i32 %25 to i64
  %27 = mul i64 %26, 8
  %28 = alloca i8, i64 %27, align 16
  %29 = bitcast i8* %28 to %union.jvalue*
  store %union.jvalue* %29, %union.jvalue** %11, align 8
  store i32 0, i32* %14, align 4
  br label %30

30:                                               ; preds = %264, %15
  %31 = load i32, i32* %14, align 4
  %32 = load i32, i32* %13, align 4
  %33 = icmp slt i32 %31, %32
  br i1 %33, label %34, label %267

34:                                               ; preds = %30
  %35 = load i32, i32* %14, align 4
  %36 = sext i32 %35 to i64
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i64 0, i64 %36
  %38 = load i8, i8* %37, align 1
  %39 = sext i8 %38 to i32
  switch i32 %39, label %263 [
    i32 90, label %40
    i32 66, label %65
    i32 83, label %90
    i32 67, label %115
    i32 73, label %141
    i32 74, label %165
    i32 68, label %189
    i32 70, label %213
    i32 76, label %238
  ]

40:                                               ; preds = %34
  %41 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %42 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 0
  %43 = load i32, i32* %42, align 8
  %44 = icmp ule i32 %43, 40
  br i1 %44, label %45, label %51

45:                                               ; preds = %40
  %46 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 3
  %47 = load i8*, i8** %46, align 8
  %48 = getelementptr i8, i8* %47, i32 %43
  %49 = bitcast i8* %48 to i32*
  %50 = add i32 %43, 8
  store i32 %50, i32* %42, align 8
  br label %56

51:                                               ; preds = %40
  %52 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 2
  %53 = load i8*, i8** %52, align 8
  %54 = bitcast i8* %53 to i32*
  %55 = getelementptr i8, i8* %53, i32 8
  store i8* %55, i8** %52, align 8
  br label %56

56:                                               ; preds = %51, %45
  %57 = phi i32* [ %49, %45 ], [ %54, %51 ]
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %11, align 8
  %61 = load i32, i32* %14, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %59, i8* %64, align 8
  br label %263

65:                                               ; preds = %34
  %66 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %67 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 0
  %68 = load i32, i32* %67, align 8
  %69 = icmp ule i32 %68, 40
  br i1 %69, label %70, label %76

70:                                               ; preds = %65
  %71 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 3
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr i8, i8* %72, i32 %68
  %74 = bitcast i8* %73 to i32*
  %75 = add i32 %68, 8
  store i32 %75, i32* %67, align 8
  br label %81

76:                                               ; preds = %65
  %77 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 2
  %78 = load i8*, i8** %77, align 8
  %79 = bitcast i8* %78 to i32*
  %80 = getelementptr i8, i8* %78, i32 8
  store i8* %80, i8** %77, align 8
  br label %81

81:                                               ; preds = %76, %70
  %82 = phi i32* [ %74, %70 ], [ %79, %76 ]
  %83 = load i32, i32* %82, align 4
  %84 = trunc i32 %83 to i8
  %85 = load %union.jvalue*, %union.jvalue** %11, align 8
  %86 = load i32, i32* %14, align 4
  %87 = sext i32 %86 to i64
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %85, i64 %87
  %89 = bitcast %union.jvalue* %88 to i8*
  store i8 %84, i8* %89, align 8
  br label %263

90:                                               ; preds = %34
  %91 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %92 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 0
  %93 = load i32, i32* %92, align 8
  %94 = icmp ule i32 %93, 40
  br i1 %94, label %95, label %101

95:                                               ; preds = %90
  %96 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 3
  %97 = load i8*, i8** %96, align 8
  %98 = getelementptr i8, i8* %97, i32 %93
  %99 = bitcast i8* %98 to i32*
  %100 = add i32 %93, 8
  store i32 %100, i32* %92, align 8
  br label %106

101:                                              ; preds = %90
  %102 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 2
  %103 = load i8*, i8** %102, align 8
  %104 = bitcast i8* %103 to i32*
  %105 = getelementptr i8, i8* %103, i32 8
  store i8* %105, i8** %102, align 8
  br label %106

106:                                              ; preds = %101, %95
  %107 = phi i32* [ %99, %95 ], [ %104, %101 ]
  %108 = load i32, i32* %107, align 4
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %11, align 8
  %111 = load i32, i32* %14, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %263

115:                                              ; preds = %34
  %116 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %117 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 0
  %118 = load i32, i32* %117, align 8
  %119 = icmp ule i32 %118, 40
  br i1 %119, label %120, label %126

120:                                              ; preds = %115
  %121 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 3
  %122 = load i8*, i8** %121, align 8
  %123 = getelementptr i8, i8* %122, i32 %118
  %124 = bitcast i8* %123 to i32*
  %125 = add i32 %118, 8
  store i32 %125, i32* %117, align 8
  br label %131

126:                                              ; preds = %115
  %127 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 2
  %128 = load i8*, i8** %127, align 8
  %129 = bitcast i8* %128 to i32*
  %130 = getelementptr i8, i8* %128, i32 8
  store i8* %130, i8** %127, align 8
  br label %131

131:                                              ; preds = %126, %120
  %132 = phi i32* [ %124, %120 ], [ %129, %126 ]
  %133 = load i32, i32* %132, align 4
  %134 = trunc i32 %133 to i16
  %135 = zext i16 %134 to i32
  %136 = load %union.jvalue*, %union.jvalue** %11, align 8
  %137 = load i32, i32* %14, align 4
  %138 = sext i32 %137 to i64
  %139 = getelementptr inbounds %union.jvalue, %union.jvalue* %136, i64 %138
  %140 = bitcast %union.jvalue* %139 to i32*
  store i32 %135, i32* %140, align 8
  br label %263

141:                                              ; preds = %34
  %142 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %143 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 0
  %144 = load i32, i32* %143, align 8
  %145 = icmp ule i32 %144, 40
  br i1 %145, label %146, label %152

146:                                              ; preds = %141
  %147 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 3
  %148 = load i8*, i8** %147, align 8
  %149 = getelementptr i8, i8* %148, i32 %144
  %150 = bitcast i8* %149 to i32*
  %151 = add i32 %144, 8
  store i32 %151, i32* %143, align 8
  br label %157

152:                                              ; preds = %141
  %153 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 2
  %154 = load i8*, i8** %153, align 8
  %155 = bitcast i8* %154 to i32*
  %156 = getelementptr i8, i8* %154, i32 8
  store i8* %156, i8** %153, align 8
  br label %157

157:                                              ; preds = %152, %146
  %158 = phi i32* [ %150, %146 ], [ %155, %152 ]
  %159 = load i32, i32* %158, align 4
  %160 = load %union.jvalue*, %union.jvalue** %11, align 8
  %161 = load i32, i32* %14, align 4
  %162 = sext i32 %161 to i64
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %160, i64 %162
  %164 = bitcast %union.jvalue* %163 to i32*
  store i32 %159, i32* %164, align 8
  br label %263

165:                                              ; preds = %34
  %166 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %167 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 0
  %168 = load i32, i32* %167, align 8
  %169 = icmp ule i32 %168, 40
  br i1 %169, label %170, label %176

170:                                              ; preds = %165
  %171 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 3
  %172 = load i8*, i8** %171, align 8
  %173 = getelementptr i8, i8* %172, i32 %168
  %174 = bitcast i8* %173 to i64*
  %175 = add i32 %168, 8
  store i32 %175, i32* %167, align 8
  br label %181

176:                                              ; preds = %165
  %177 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 2
  %178 = load i8*, i8** %177, align 8
  %179 = bitcast i8* %178 to i64*
  %180 = getelementptr i8, i8* %178, i32 8
  store i8* %180, i8** %177, align 8
  br label %181

181:                                              ; preds = %176, %170
  %182 = phi i64* [ %174, %170 ], [ %179, %176 ]
  %183 = load i64, i64* %182, align 8
  %184 = load %union.jvalue*, %union.jvalue** %11, align 8
  %185 = load i32, i32* %14, align 4
  %186 = sext i32 %185 to i64
  %187 = getelementptr inbounds %union.jvalue, %union.jvalue* %184, i64 %186
  %188 = bitcast %union.jvalue* %187 to i64*
  store i64 %183, i64* %188, align 8
  br label %263

189:                                              ; preds = %34
  %190 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %191 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 1
  %192 = load i32, i32* %191, align 4
  %193 = icmp ule i32 %192, 160
  br i1 %193, label %194, label %200

194:                                              ; preds = %189
  %195 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 3
  %196 = load i8*, i8** %195, align 8
  %197 = getelementptr i8, i8* %196, i32 %192
  %198 = bitcast i8* %197 to double*
  %199 = add i32 %192, 16
  store i32 %199, i32* %191, align 4
  br label %205

200:                                              ; preds = %189
  %201 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 2
  %202 = load i8*, i8** %201, align 8
  %203 = bitcast i8* %202 to double*
  %204 = getelementptr i8, i8* %202, i32 8
  store i8* %204, i8** %201, align 8
  br label %205

205:                                              ; preds = %200, %194
  %206 = phi double* [ %198, %194 ], [ %203, %200 ]
  %207 = load double, double* %206, align 8
  %208 = load %union.jvalue*, %union.jvalue** %11, align 8
  %209 = load i32, i32* %14, align 4
  %210 = sext i32 %209 to i64
  %211 = getelementptr inbounds %union.jvalue, %union.jvalue* %208, i64 %210
  %212 = bitcast %union.jvalue* %211 to double*
  store double %207, double* %212, align 8
  br label %263

213:                                              ; preds = %34
  %214 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %215 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 1
  %216 = load i32, i32* %215, align 4
  %217 = icmp ule i32 %216, 160
  br i1 %217, label %218, label %224

218:                                              ; preds = %213
  %219 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 3
  %220 = load i8*, i8** %219, align 8
  %221 = getelementptr i8, i8* %220, i32 %216
  %222 = bitcast i8* %221 to double*
  %223 = add i32 %216, 16
  store i32 %223, i32* %215, align 4
  br label %229

224:                                              ; preds = %213
  %225 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = bitcast i8* %226 to double*
  %228 = getelementptr i8, i8* %226, i32 8
  store i8* %228, i8** %225, align 8
  br label %229

229:                                              ; preds = %224, %218
  %230 = phi double* [ %222, %218 ], [ %227, %224 ]
  %231 = load double, double* %230, align 8
  %232 = fptrunc double %231 to float
  %233 = load %union.jvalue*, %union.jvalue** %11, align 8
  %234 = load i32, i32* %14, align 4
  %235 = sext i32 %234 to i64
  %236 = getelementptr inbounds %union.jvalue, %union.jvalue* %233, i64 %235
  %237 = bitcast %union.jvalue* %236 to float*
  store float %232, float* %237, align 8
  br label %263

238:                                              ; preds = %34
  %239 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %240 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 0
  %241 = load i32, i32* %240, align 8
  %242 = icmp ule i32 %241, 40
  br i1 %242, label %243, label %249

243:                                              ; preds = %238
  %244 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 3
  %245 = load i8*, i8** %244, align 8
  %246 = getelementptr i8, i8* %245, i32 %241
  %247 = bitcast i8* %246 to i8**
  %248 = add i32 %241, 8
  store i32 %248, i32* %240, align 8
  br label %254

249:                                              ; preds = %238
  %250 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 2
  %251 = load i8*, i8** %250, align 8
  %252 = bitcast i8* %251 to i8**
  %253 = getelementptr i8, i8* %251, i32 8
  store i8* %253, i8** %250, align 8
  br label %254

254:                                              ; preds = %249, %243
  %255 = phi i8** [ %247, %243 ], [ %252, %249 ]
  %256 = load i8*, i8** %255, align 8
  %257 = bitcast i8* %256 to %struct._jobject*
  %258 = load %union.jvalue*, %union.jvalue** %11, align 8
  %259 = load i32, i32* %14, align 4
  %260 = sext i32 %259 to i64
  %261 = getelementptr inbounds %union.jvalue, %union.jvalue* %258, i64 %260
  %262 = bitcast %union.jvalue* %261 to %struct._jobject**
  store %struct._jobject* %257, %struct._jobject** %262, align 8
  br label %263

263:                                              ; preds = %34, %254, %229, %205, %181, %157, %131, %106, %81, %56
  br label %264

264:                                              ; preds = %263
  %265 = load i32, i32* %14, align 4
  %266 = add nsw i32 %265, 1
  store i32 %266, i32* %14, align 4
  br label %30, !llvm.loop !32

267:                                              ; preds = %30
  br label %268

268:                                              ; preds = %267
  %269 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %270 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %269, align 8
  %271 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %270, i32 0, i32 90
  %272 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %271, align 8
  %273 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %274 = load %struct._jobject*, %struct._jobject** %7, align 8
  %275 = load %struct._jobject*, %struct._jobject** %8, align 8
  %276 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %277 = load %union.jvalue*, %union.jvalue** %11, align 8
  %278 = call double %272(%struct.JNINativeInterface_** noundef %273, %struct._jobject* noundef %274, %struct._jobject* noundef %275, %struct._jmethodID* noundef %276, %union.jvalue* noundef %277)
  ret double %278
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local double @JNI_CallStaticDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca double, align 8
  %8 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %10 = bitcast %struct.__va_list_tag* %9 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 139
  %14 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %19 = call double %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %struct.__va_list_tag* noundef %18)
  store double %19, double* %7, align 8
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %21 = bitcast %struct.__va_list_tag* %20 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load double, double* %7, align 8
  ret double %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local double @JNI_CallStaticDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !33

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 140
  %270 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  %275 = call double %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret double %275
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local %struct._jobject* @JNI_NewObject(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %10 = bitcast %struct.__va_list_tag* %9 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 29
  %14 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %19 = call %struct._jobject* %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %struct.__va_list_tag* noundef %18)
  store %struct._jobject* %19, %struct._jobject** %7, align 8
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %8, i64 0, i64 0
  %21 = bitcast %struct.__va_list_tag* %20 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load %struct._jobject*, %struct._jobject** %7, align 8
  ret %struct._jobject* %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local %struct._jobject* @JNI_NewObjectV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !34

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 30
  %270 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  %275 = call %struct._jobject* %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret %struct._jobject* %275
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @JNI_CallVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %8 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %7, i64 0, i64 0
  %9 = bitcast %struct.__va_list_tag* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 8
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 62
  %13 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %12, align 8
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %15 = load %struct._jobject*, %struct._jobject** %5, align 8
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %7, i64 0, i64 0
  call void %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, %struct.__va_list_tag* noundef %17)
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %7, i64 0, i64 0
  %19 = bitcast %struct.__va_list_tag* %18 to i8*
  call void @llvm.va_end(i8* %19)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @JNI_CallVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !35

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 63
  %270 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  call void %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @JNI_CallNonvirtualVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %10 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %9, i64 0, i64 0
  %11 = bitcast %struct.__va_list_tag* %10 to i8*
  call void @llvm.va_start(i8* %11)
  %12 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %13 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %12, align 8
  %14 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %13, i32 0, i32 92
  %15 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %14, align 8
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %17 = load %struct._jobject*, %struct._jobject** %6, align 8
  %18 = load %struct._jobject*, %struct._jobject** %7, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %9, i64 0, i64 0
  call void %15(%struct.JNINativeInterface_** noundef %16, %struct._jobject* noundef %17, %struct._jobject* noundef %18, %struct._jmethodID* noundef %19, %struct.__va_list_tag* noundef %20)
  %21 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %9, i64 0, i64 0
  %22 = bitcast %struct.__va_list_tag* %21 to i8*
  call void @llvm.va_end(i8* %22)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @JNI_CallNonvirtualVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %struct.__va_list_tag* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca %struct.__va_list_tag*, align 8
  %11 = alloca %union.jvalue*, align 8
  %12 = alloca [257 x i8], align 16
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  store %struct.__va_list_tag* %4, %struct.__va_list_tag** %10, align 8
  br label %15

15:                                               ; preds = %5
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 8
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %22 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i64 0, i64 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %13, align 4
  %25 = load i32, i32* %13, align 4
  %26 = sext i32 %25 to i64
  %27 = mul i64 %26, 8
  %28 = alloca i8, i64 %27, align 16
  %29 = bitcast i8* %28 to %union.jvalue*
  store %union.jvalue* %29, %union.jvalue** %11, align 8
  store i32 0, i32* %14, align 4
  br label %30

30:                                               ; preds = %264, %15
  %31 = load i32, i32* %14, align 4
  %32 = load i32, i32* %13, align 4
  %33 = icmp slt i32 %31, %32
  br i1 %33, label %34, label %267

34:                                               ; preds = %30
  %35 = load i32, i32* %14, align 4
  %36 = sext i32 %35 to i64
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i64 0, i64 %36
  %38 = load i8, i8* %37, align 1
  %39 = sext i8 %38 to i32
  switch i32 %39, label %263 [
    i32 90, label %40
    i32 66, label %65
    i32 83, label %90
    i32 67, label %115
    i32 73, label %141
    i32 74, label %165
    i32 68, label %189
    i32 70, label %213
    i32 76, label %238
  ]

40:                                               ; preds = %34
  %41 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %42 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 0
  %43 = load i32, i32* %42, align 8
  %44 = icmp ule i32 %43, 40
  br i1 %44, label %45, label %51

45:                                               ; preds = %40
  %46 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 3
  %47 = load i8*, i8** %46, align 8
  %48 = getelementptr i8, i8* %47, i32 %43
  %49 = bitcast i8* %48 to i32*
  %50 = add i32 %43, 8
  store i32 %50, i32* %42, align 8
  br label %56

51:                                               ; preds = %40
  %52 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %41, i32 0, i32 2
  %53 = load i8*, i8** %52, align 8
  %54 = bitcast i8* %53 to i32*
  %55 = getelementptr i8, i8* %53, i32 8
  store i8* %55, i8** %52, align 8
  br label %56

56:                                               ; preds = %51, %45
  %57 = phi i32* [ %49, %45 ], [ %54, %51 ]
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %11, align 8
  %61 = load i32, i32* %14, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %59, i8* %64, align 8
  br label %263

65:                                               ; preds = %34
  %66 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %67 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 0
  %68 = load i32, i32* %67, align 8
  %69 = icmp ule i32 %68, 40
  br i1 %69, label %70, label %76

70:                                               ; preds = %65
  %71 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 3
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr i8, i8* %72, i32 %68
  %74 = bitcast i8* %73 to i32*
  %75 = add i32 %68, 8
  store i32 %75, i32* %67, align 8
  br label %81

76:                                               ; preds = %65
  %77 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %66, i32 0, i32 2
  %78 = load i8*, i8** %77, align 8
  %79 = bitcast i8* %78 to i32*
  %80 = getelementptr i8, i8* %78, i32 8
  store i8* %80, i8** %77, align 8
  br label %81

81:                                               ; preds = %76, %70
  %82 = phi i32* [ %74, %70 ], [ %79, %76 ]
  %83 = load i32, i32* %82, align 4
  %84 = trunc i32 %83 to i8
  %85 = load %union.jvalue*, %union.jvalue** %11, align 8
  %86 = load i32, i32* %14, align 4
  %87 = sext i32 %86 to i64
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %85, i64 %87
  %89 = bitcast %union.jvalue* %88 to i8*
  store i8 %84, i8* %89, align 8
  br label %263

90:                                               ; preds = %34
  %91 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %92 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 0
  %93 = load i32, i32* %92, align 8
  %94 = icmp ule i32 %93, 40
  br i1 %94, label %95, label %101

95:                                               ; preds = %90
  %96 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 3
  %97 = load i8*, i8** %96, align 8
  %98 = getelementptr i8, i8* %97, i32 %93
  %99 = bitcast i8* %98 to i32*
  %100 = add i32 %93, 8
  store i32 %100, i32* %92, align 8
  br label %106

101:                                              ; preds = %90
  %102 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %91, i32 0, i32 2
  %103 = load i8*, i8** %102, align 8
  %104 = bitcast i8* %103 to i32*
  %105 = getelementptr i8, i8* %103, i32 8
  store i8* %105, i8** %102, align 8
  br label %106

106:                                              ; preds = %101, %95
  %107 = phi i32* [ %99, %95 ], [ %104, %101 ]
  %108 = load i32, i32* %107, align 4
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %11, align 8
  %111 = load i32, i32* %14, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %263

115:                                              ; preds = %34
  %116 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %117 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 0
  %118 = load i32, i32* %117, align 8
  %119 = icmp ule i32 %118, 40
  br i1 %119, label %120, label %126

120:                                              ; preds = %115
  %121 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 3
  %122 = load i8*, i8** %121, align 8
  %123 = getelementptr i8, i8* %122, i32 %118
  %124 = bitcast i8* %123 to i32*
  %125 = add i32 %118, 8
  store i32 %125, i32* %117, align 8
  br label %131

126:                                              ; preds = %115
  %127 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %116, i32 0, i32 2
  %128 = load i8*, i8** %127, align 8
  %129 = bitcast i8* %128 to i32*
  %130 = getelementptr i8, i8* %128, i32 8
  store i8* %130, i8** %127, align 8
  br label %131

131:                                              ; preds = %126, %120
  %132 = phi i32* [ %124, %120 ], [ %129, %126 ]
  %133 = load i32, i32* %132, align 4
  %134 = trunc i32 %133 to i16
  %135 = zext i16 %134 to i32
  %136 = load %union.jvalue*, %union.jvalue** %11, align 8
  %137 = load i32, i32* %14, align 4
  %138 = sext i32 %137 to i64
  %139 = getelementptr inbounds %union.jvalue, %union.jvalue* %136, i64 %138
  %140 = bitcast %union.jvalue* %139 to i32*
  store i32 %135, i32* %140, align 8
  br label %263

141:                                              ; preds = %34
  %142 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %143 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 0
  %144 = load i32, i32* %143, align 8
  %145 = icmp ule i32 %144, 40
  br i1 %145, label %146, label %152

146:                                              ; preds = %141
  %147 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 3
  %148 = load i8*, i8** %147, align 8
  %149 = getelementptr i8, i8* %148, i32 %144
  %150 = bitcast i8* %149 to i32*
  %151 = add i32 %144, 8
  store i32 %151, i32* %143, align 8
  br label %157

152:                                              ; preds = %141
  %153 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %142, i32 0, i32 2
  %154 = load i8*, i8** %153, align 8
  %155 = bitcast i8* %154 to i32*
  %156 = getelementptr i8, i8* %154, i32 8
  store i8* %156, i8** %153, align 8
  br label %157

157:                                              ; preds = %152, %146
  %158 = phi i32* [ %150, %146 ], [ %155, %152 ]
  %159 = load i32, i32* %158, align 4
  %160 = load %union.jvalue*, %union.jvalue** %11, align 8
  %161 = load i32, i32* %14, align 4
  %162 = sext i32 %161 to i64
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %160, i64 %162
  %164 = bitcast %union.jvalue* %163 to i32*
  store i32 %159, i32* %164, align 8
  br label %263

165:                                              ; preds = %34
  %166 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %167 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 0
  %168 = load i32, i32* %167, align 8
  %169 = icmp ule i32 %168, 40
  br i1 %169, label %170, label %176

170:                                              ; preds = %165
  %171 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 3
  %172 = load i8*, i8** %171, align 8
  %173 = getelementptr i8, i8* %172, i32 %168
  %174 = bitcast i8* %173 to i64*
  %175 = add i32 %168, 8
  store i32 %175, i32* %167, align 8
  br label %181

176:                                              ; preds = %165
  %177 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %166, i32 0, i32 2
  %178 = load i8*, i8** %177, align 8
  %179 = bitcast i8* %178 to i64*
  %180 = getelementptr i8, i8* %178, i32 8
  store i8* %180, i8** %177, align 8
  br label %181

181:                                              ; preds = %176, %170
  %182 = phi i64* [ %174, %170 ], [ %179, %176 ]
  %183 = load i64, i64* %182, align 8
  %184 = load %union.jvalue*, %union.jvalue** %11, align 8
  %185 = load i32, i32* %14, align 4
  %186 = sext i32 %185 to i64
  %187 = getelementptr inbounds %union.jvalue, %union.jvalue* %184, i64 %186
  %188 = bitcast %union.jvalue* %187 to i64*
  store i64 %183, i64* %188, align 8
  br label %263

189:                                              ; preds = %34
  %190 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %191 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 1
  %192 = load i32, i32* %191, align 4
  %193 = icmp ule i32 %192, 160
  br i1 %193, label %194, label %200

194:                                              ; preds = %189
  %195 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 3
  %196 = load i8*, i8** %195, align 8
  %197 = getelementptr i8, i8* %196, i32 %192
  %198 = bitcast i8* %197 to double*
  %199 = add i32 %192, 16
  store i32 %199, i32* %191, align 4
  br label %205

200:                                              ; preds = %189
  %201 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %190, i32 0, i32 2
  %202 = load i8*, i8** %201, align 8
  %203 = bitcast i8* %202 to double*
  %204 = getelementptr i8, i8* %202, i32 8
  store i8* %204, i8** %201, align 8
  br label %205

205:                                              ; preds = %200, %194
  %206 = phi double* [ %198, %194 ], [ %203, %200 ]
  %207 = load double, double* %206, align 8
  %208 = load %union.jvalue*, %union.jvalue** %11, align 8
  %209 = load i32, i32* %14, align 4
  %210 = sext i32 %209 to i64
  %211 = getelementptr inbounds %union.jvalue, %union.jvalue* %208, i64 %210
  %212 = bitcast %union.jvalue* %211 to double*
  store double %207, double* %212, align 8
  br label %263

213:                                              ; preds = %34
  %214 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %215 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 1
  %216 = load i32, i32* %215, align 4
  %217 = icmp ule i32 %216, 160
  br i1 %217, label %218, label %224

218:                                              ; preds = %213
  %219 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 3
  %220 = load i8*, i8** %219, align 8
  %221 = getelementptr i8, i8* %220, i32 %216
  %222 = bitcast i8* %221 to double*
  %223 = add i32 %216, 16
  store i32 %223, i32* %215, align 4
  br label %229

224:                                              ; preds = %213
  %225 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %214, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = bitcast i8* %226 to double*
  %228 = getelementptr i8, i8* %226, i32 8
  store i8* %228, i8** %225, align 8
  br label %229

229:                                              ; preds = %224, %218
  %230 = phi double* [ %222, %218 ], [ %227, %224 ]
  %231 = load double, double* %230, align 8
  %232 = fptrunc double %231 to float
  %233 = load %union.jvalue*, %union.jvalue** %11, align 8
  %234 = load i32, i32* %14, align 4
  %235 = sext i32 %234 to i64
  %236 = getelementptr inbounds %union.jvalue, %union.jvalue* %233, i64 %235
  %237 = bitcast %union.jvalue* %236 to float*
  store float %232, float* %237, align 8
  br label %263

238:                                              ; preds = %34
  %239 = load %struct.__va_list_tag*, %struct.__va_list_tag** %10, align 8
  %240 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 0
  %241 = load i32, i32* %240, align 8
  %242 = icmp ule i32 %241, 40
  br i1 %242, label %243, label %249

243:                                              ; preds = %238
  %244 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 3
  %245 = load i8*, i8** %244, align 8
  %246 = getelementptr i8, i8* %245, i32 %241
  %247 = bitcast i8* %246 to i8**
  %248 = add i32 %241, 8
  store i32 %248, i32* %240, align 8
  br label %254

249:                                              ; preds = %238
  %250 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %239, i32 0, i32 2
  %251 = load i8*, i8** %250, align 8
  %252 = bitcast i8* %251 to i8**
  %253 = getelementptr i8, i8* %251, i32 8
  store i8* %253, i8** %250, align 8
  br label %254

254:                                              ; preds = %249, %243
  %255 = phi i8** [ %247, %243 ], [ %252, %249 ]
  %256 = load i8*, i8** %255, align 8
  %257 = bitcast i8* %256 to %struct._jobject*
  %258 = load %union.jvalue*, %union.jvalue** %11, align 8
  %259 = load i32, i32* %14, align 4
  %260 = sext i32 %259 to i64
  %261 = getelementptr inbounds %union.jvalue, %union.jvalue* %258, i64 %260
  %262 = bitcast %union.jvalue* %261 to %struct._jobject**
  store %struct._jobject* %257, %struct._jobject** %262, align 8
  br label %263

263:                                              ; preds = %34, %254, %229, %205, %181, %157, %131, %106, %81, %56
  br label %264

264:                                              ; preds = %263
  %265 = load i32, i32* %14, align 4
  %266 = add nsw i32 %265, 1
  store i32 %266, i32* %14, align 4
  br label %30, !llvm.loop !36

267:                                              ; preds = %30
  br label %268

268:                                              ; preds = %267
  %269 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %270 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %269, align 8
  %271 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %270, i32 0, i32 93
  %272 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %271, align 8
  %273 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %274 = load %struct._jobject*, %struct._jobject** %7, align 8
  %275 = load %struct._jobject*, %struct._jobject** %8, align 8
  %276 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %277 = load %union.jvalue*, %union.jvalue** %11, align 8
  call void %272(%struct.JNINativeInterface_** noundef %273, %struct._jobject* noundef %274, %struct._jobject* noundef %275, %struct._jmethodID* noundef %276, %union.jvalue* noundef %277)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @JNI_CallStaticVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca [1 x %struct.__va_list_tag], align 16
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %8 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %7, i64 0, i64 0
  %9 = bitcast %struct.__va_list_tag* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 8
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 142
  %13 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %struct.__va_list_tag*)** %12, align 8
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %15 = load %struct._jobject*, %struct._jobject** %5, align 8
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %7, i64 0, i64 0
  call void %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, %struct.__va_list_tag* noundef %17)
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %7, i64 0, i64 0
  %19 = bitcast %struct.__va_list_tag* %18 to i8*
  call void @llvm.va_end(i8* %19)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @JNI_CallStaticVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %struct.__va_list_tag*, align 8
  %9 = alloca %union.jvalue*, align 8
  %10 = alloca [257 x i8], align 16
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store %struct.__va_list_tag* %3, %struct.__va_list_tag** %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 8
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %11, align 4
  %23 = load i32, i32* %11, align 4
  %24 = sext i32 %23 to i64
  %25 = mul i64 %24, 8
  %26 = alloca i8, i64 %25, align 16
  %27 = bitcast i8* %26 to %union.jvalue*
  store %union.jvalue* %27, %union.jvalue** %9, align 8
  store i32 0, i32* %12, align 4
  br label %28

28:                                               ; preds = %262, %13
  %29 = load i32, i32* %12, align 4
  %30 = load i32, i32* %11, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %265

32:                                               ; preds = %28
  %33 = load i32, i32* %12, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i64 0, i64 %34
  %36 = load i8, i8* %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %261 [
    i32 90, label %38
    i32 66, label %63
    i32 83, label %88
    i32 67, label %113
    i32 73, label %139
    i32 74, label %163
    i32 68, label %187
    i32 70, label %211
    i32 76, label %236
  ]

38:                                               ; preds = %32
  %39 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %40 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 0
  %41 = load i32, i32* %40, align 8
  %42 = icmp ule i32 %41, 40
  br i1 %42, label %43, label %49

43:                                               ; preds = %38
  %44 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 3
  %45 = load i8*, i8** %44, align 8
  %46 = getelementptr i8, i8* %45, i32 %41
  %47 = bitcast i8* %46 to i32*
  %48 = add i32 %41, 8
  store i32 %48, i32* %40, align 8
  br label %54

49:                                               ; preds = %38
  %50 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %39, i32 0, i32 2
  %51 = load i8*, i8** %50, align 8
  %52 = bitcast i8* %51 to i32*
  %53 = getelementptr i8, i8* %51, i32 8
  store i8* %53, i8** %50, align 8
  br label %54

54:                                               ; preds = %49, %43
  %55 = phi i32* [ %47, %43 ], [ %52, %49 ]
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %9, align 8
  %59 = load i32, i32* %12, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %261

63:                                               ; preds = %32
  %64 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %65 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 0
  %66 = load i32, i32* %65, align 8
  %67 = icmp ule i32 %66, 40
  br i1 %67, label %68, label %74

68:                                               ; preds = %63
  %69 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 3
  %70 = load i8*, i8** %69, align 8
  %71 = getelementptr i8, i8* %70, i32 %66
  %72 = bitcast i8* %71 to i32*
  %73 = add i32 %66, 8
  store i32 %73, i32* %65, align 8
  br label %79

74:                                               ; preds = %63
  %75 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %64, i32 0, i32 2
  %76 = load i8*, i8** %75, align 8
  %77 = bitcast i8* %76 to i32*
  %78 = getelementptr i8, i8* %76, i32 8
  store i8* %78, i8** %75, align 8
  br label %79

79:                                               ; preds = %74, %68
  %80 = phi i32* [ %72, %68 ], [ %77, %74 ]
  %81 = load i32, i32* %80, align 4
  %82 = trunc i32 %81 to i8
  %83 = load %union.jvalue*, %union.jvalue** %9, align 8
  %84 = load i32, i32* %12, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds %union.jvalue, %union.jvalue* %83, i64 %85
  %87 = bitcast %union.jvalue* %86 to i8*
  store i8 %82, i8* %87, align 8
  br label %261

88:                                               ; preds = %32
  %89 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %90 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 0
  %91 = load i32, i32* %90, align 8
  %92 = icmp ule i32 %91, 40
  br i1 %92, label %93, label %99

93:                                               ; preds = %88
  %94 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 3
  %95 = load i8*, i8** %94, align 8
  %96 = getelementptr i8, i8* %95, i32 %91
  %97 = bitcast i8* %96 to i32*
  %98 = add i32 %91, 8
  store i32 %98, i32* %90, align 8
  br label %104

99:                                               ; preds = %88
  %100 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %89, i32 0, i32 2
  %101 = load i8*, i8** %100, align 8
  %102 = bitcast i8* %101 to i32*
  %103 = getelementptr i8, i8* %101, i32 8
  store i8* %103, i8** %100, align 8
  br label %104

104:                                              ; preds = %99, %93
  %105 = phi i32* [ %97, %93 ], [ %102, %99 ]
  %106 = load i32, i32* %105, align 4
  %107 = trunc i32 %106 to i16
  %108 = load %union.jvalue*, %union.jvalue** %9, align 8
  %109 = load i32, i32* %12, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %108, i64 %110
  %112 = bitcast %union.jvalue* %111 to i16*
  store i16 %107, i16* %112, align 8
  br label %261

113:                                              ; preds = %32
  %114 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %115 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 0
  %116 = load i32, i32* %115, align 8
  %117 = icmp ule i32 %116, 40
  br i1 %117, label %118, label %124

118:                                              ; preds = %113
  %119 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 3
  %120 = load i8*, i8** %119, align 8
  %121 = getelementptr i8, i8* %120, i32 %116
  %122 = bitcast i8* %121 to i32*
  %123 = add i32 %116, 8
  store i32 %123, i32* %115, align 8
  br label %129

124:                                              ; preds = %113
  %125 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %114, i32 0, i32 2
  %126 = load i8*, i8** %125, align 8
  %127 = bitcast i8* %126 to i32*
  %128 = getelementptr i8, i8* %126, i32 8
  store i8* %128, i8** %125, align 8
  br label %129

129:                                              ; preds = %124, %118
  %130 = phi i32* [ %122, %118 ], [ %127, %124 ]
  %131 = load i32, i32* %130, align 4
  %132 = trunc i32 %131 to i16
  %133 = zext i16 %132 to i32
  %134 = load %union.jvalue*, %union.jvalue** %9, align 8
  %135 = load i32, i32* %12, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds %union.jvalue, %union.jvalue* %134, i64 %136
  %138 = bitcast %union.jvalue* %137 to i32*
  store i32 %133, i32* %138, align 8
  br label %261

139:                                              ; preds = %32
  %140 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %141 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 0
  %142 = load i32, i32* %141, align 8
  %143 = icmp ule i32 %142, 40
  br i1 %143, label %144, label %150

144:                                              ; preds = %139
  %145 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 3
  %146 = load i8*, i8** %145, align 8
  %147 = getelementptr i8, i8* %146, i32 %142
  %148 = bitcast i8* %147 to i32*
  %149 = add i32 %142, 8
  store i32 %149, i32* %141, align 8
  br label %155

150:                                              ; preds = %139
  %151 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %140, i32 0, i32 2
  %152 = load i8*, i8** %151, align 8
  %153 = bitcast i8* %152 to i32*
  %154 = getelementptr i8, i8* %152, i32 8
  store i8* %154, i8** %151, align 8
  br label %155

155:                                              ; preds = %150, %144
  %156 = phi i32* [ %148, %144 ], [ %153, %150 ]
  %157 = load i32, i32* %156, align 4
  %158 = load %union.jvalue*, %union.jvalue** %9, align 8
  %159 = load i32, i32* %12, align 4
  %160 = sext i32 %159 to i64
  %161 = getelementptr inbounds %union.jvalue, %union.jvalue* %158, i64 %160
  %162 = bitcast %union.jvalue* %161 to i32*
  store i32 %157, i32* %162, align 8
  br label %261

163:                                              ; preds = %32
  %164 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %165 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 0
  %166 = load i32, i32* %165, align 8
  %167 = icmp ule i32 %166, 40
  br i1 %167, label %168, label %174

168:                                              ; preds = %163
  %169 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 3
  %170 = load i8*, i8** %169, align 8
  %171 = getelementptr i8, i8* %170, i32 %166
  %172 = bitcast i8* %171 to i64*
  %173 = add i32 %166, 8
  store i32 %173, i32* %165, align 8
  br label %179

174:                                              ; preds = %163
  %175 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %164, i32 0, i32 2
  %176 = load i8*, i8** %175, align 8
  %177 = bitcast i8* %176 to i64*
  %178 = getelementptr i8, i8* %176, i32 8
  store i8* %178, i8** %175, align 8
  br label %179

179:                                              ; preds = %174, %168
  %180 = phi i64* [ %172, %168 ], [ %177, %174 ]
  %181 = load i64, i64* %180, align 8
  %182 = load %union.jvalue*, %union.jvalue** %9, align 8
  %183 = load i32, i32* %12, align 4
  %184 = sext i32 %183 to i64
  %185 = getelementptr inbounds %union.jvalue, %union.jvalue* %182, i64 %184
  %186 = bitcast %union.jvalue* %185 to i64*
  store i64 %181, i64* %186, align 8
  br label %261

187:                                              ; preds = %32
  %188 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %189 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 1
  %190 = load i32, i32* %189, align 4
  %191 = icmp ule i32 %190, 160
  br i1 %191, label %192, label %198

192:                                              ; preds = %187
  %193 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 3
  %194 = load i8*, i8** %193, align 8
  %195 = getelementptr i8, i8* %194, i32 %190
  %196 = bitcast i8* %195 to double*
  %197 = add i32 %190, 16
  store i32 %197, i32* %189, align 4
  br label %203

198:                                              ; preds = %187
  %199 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %188, i32 0, i32 2
  %200 = load i8*, i8** %199, align 8
  %201 = bitcast i8* %200 to double*
  %202 = getelementptr i8, i8* %200, i32 8
  store i8* %202, i8** %199, align 8
  br label %203

203:                                              ; preds = %198, %192
  %204 = phi double* [ %196, %192 ], [ %201, %198 ]
  %205 = load double, double* %204, align 8
  %206 = load %union.jvalue*, %union.jvalue** %9, align 8
  %207 = load i32, i32* %12, align 4
  %208 = sext i32 %207 to i64
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %206, i64 %208
  %210 = bitcast %union.jvalue* %209 to double*
  store double %205, double* %210, align 8
  br label %261

211:                                              ; preds = %32
  %212 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %213 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 1
  %214 = load i32, i32* %213, align 4
  %215 = icmp ule i32 %214, 160
  br i1 %215, label %216, label %222

216:                                              ; preds = %211
  %217 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 3
  %218 = load i8*, i8** %217, align 8
  %219 = getelementptr i8, i8* %218, i32 %214
  %220 = bitcast i8* %219 to double*
  %221 = add i32 %214, 16
  store i32 %221, i32* %213, align 4
  br label %227

222:                                              ; preds = %211
  %223 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %212, i32 0, i32 2
  %224 = load i8*, i8** %223, align 8
  %225 = bitcast i8* %224 to double*
  %226 = getelementptr i8, i8* %224, i32 8
  store i8* %226, i8** %223, align 8
  br label %227

227:                                              ; preds = %222, %216
  %228 = phi double* [ %220, %216 ], [ %225, %222 ]
  %229 = load double, double* %228, align 8
  %230 = fptrunc double %229 to float
  %231 = load %union.jvalue*, %union.jvalue** %9, align 8
  %232 = load i32, i32* %12, align 4
  %233 = sext i32 %232 to i64
  %234 = getelementptr inbounds %union.jvalue, %union.jvalue* %231, i64 %233
  %235 = bitcast %union.jvalue* %234 to float*
  store float %230, float* %235, align 8
  br label %261

236:                                              ; preds = %32
  %237 = load %struct.__va_list_tag*, %struct.__va_list_tag** %8, align 8
  %238 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 0
  %239 = load i32, i32* %238, align 8
  %240 = icmp ule i32 %239, 40
  br i1 %240, label %241, label %247

241:                                              ; preds = %236
  %242 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 3
  %243 = load i8*, i8** %242, align 8
  %244 = getelementptr i8, i8* %243, i32 %239
  %245 = bitcast i8* %244 to i8**
  %246 = add i32 %239, 8
  store i32 %246, i32* %238, align 8
  br label %252

247:                                              ; preds = %236
  %248 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %237, i32 0, i32 2
  %249 = load i8*, i8** %248, align 8
  %250 = bitcast i8* %249 to i8**
  %251 = getelementptr i8, i8* %249, i32 8
  store i8* %251, i8** %248, align 8
  br label %252

252:                                              ; preds = %247, %241
  %253 = phi i8** [ %245, %241 ], [ %250, %247 ]
  %254 = load i8*, i8** %253, align 8
  %255 = bitcast i8* %254 to %struct._jobject*
  %256 = load %union.jvalue*, %union.jvalue** %9, align 8
  %257 = load i32, i32* %12, align 4
  %258 = sext i32 %257 to i64
  %259 = getelementptr inbounds %union.jvalue, %union.jvalue* %256, i64 %258
  %260 = bitcast %union.jvalue* %259 to %struct._jobject**
  store %struct._jobject* %255, %struct._jobject** %260, align 8
  br label %261

261:                                              ; preds = %32, %252, %227, %203, %179, %155, %129, %104, %79, %54
  br label %262

262:                                              ; preds = %261
  %263 = load i32, i32* %12, align 4
  %264 = add nsw i32 %263, 1
  store i32 %264, i32* %12, align 4
  br label %28, !llvm.loop !37

265:                                              ; preds = %28
  br label %266

266:                                              ; preds = %265
  %267 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %268 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %267, align 8
  %269 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %268, i32 0, i32 143
  %270 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %269, align 8
  %271 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %272 = load %struct._jobject*, %struct._jobject** %6, align 8
  %273 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %274 = load %union.jvalue*, %union.jvalue** %9, align 8
  call void %270(%struct.JNINativeInterface_** noundef %271, %struct._jobject* noundef %272, %struct._jmethodID* noundef %273, %union.jvalue* noundef %274)
  ret void
}

attributes #0 = { noinline nounwind optnone uwtable "frame-pointer"="all" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
attributes #1 = { nofree nosync nounwind willreturn }

!llvm.module.flags = !{!0, !1, !2, !3, !4}
!llvm.ident = !{!5}

!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 7, !"PIE Level", i32 2}
!3 = !{i32 7, !"uwtable", i32 1}
!4 = !{i32 7, !"frame-pointer", i32 2}
!5 = !{!"Ubuntu clang version 14.0.0-1ubuntu1"}
!6 = distinct !{!6, !7}
!7 = !{!"llvm.loop.mustprogress"}
!8 = distinct !{!8, !7}
!9 = distinct !{!9, !7}
!10 = distinct !{!10, !7}
!11 = distinct !{!11, !7}
!12 = distinct !{!12, !7}
!13 = distinct !{!13, !7}
!14 = distinct !{!14, !7}
!15 = distinct !{!15, !7}
!16 = distinct !{!16, !7}
!17 = distinct !{!17, !7}
!18 = distinct !{!18, !7}
!19 = distinct !{!19, !7}
!20 = distinct !{!20, !7}
!21 = distinct !{!21, !7}
!22 = distinct !{!22, !7}
!23 = distinct !{!23, !7}
!24 = distinct !{!24, !7}
!25 = distinct !{!25, !7}
!26 = distinct !{!26, !7}
!27 = distinct !{!27, !7}
!28 = distinct !{!28, !7}
!29 = distinct !{!29, !7}
!30 = distinct !{!30, !7}
!31 = distinct !{!31, !7}
!32 = distinct !{!32, !7}
!33 = distinct !{!33, !7}
!34 = distinct !{!34, !7}
!35 = distinct !{!35, !7}
!36 = distinct !{!36, !7}
!37 = distinct !{!37, !7}
