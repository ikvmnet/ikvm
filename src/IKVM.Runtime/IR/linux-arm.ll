; ModuleID = 'jni.c'
source_filename = "jni.c"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-pc-linux-gnueabihf"

%struct.JNINativeInterface_ = type { i8*, i8*, i8*, i8*, i32 (%struct.JNINativeInterface_**)*, %struct._jobject* (%struct.JNINativeInterface_**, i8*, %struct._jobject*, i8*, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i8*)*, %struct._jmethodID* (%struct.JNINativeInterface_**, %struct._jobject*)*, %struct._jfieldID* (%struct.JNINativeInterface_**, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i8)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, %struct._jobject* (%struct.JNINativeInterface_**)*, void (%struct.JNINativeInterface_**)*, void (%struct.JNINativeInterface_**)*, void (%struct.JNINativeInterface_**, i8*)*, i32 (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, void (%struct.JNINativeInterface_**, %struct._jobject*)*, void (%struct.JNINativeInterface_**, %struct._jobject*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, {}*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*)*, %struct._jmethodID* (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i8*)*, {}*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jfieldID* (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i8*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, %struct._jobject*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i8)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i8)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i16)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i16)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i64)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, float)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, double)*, %struct._jmethodID* (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i8*)*, {}*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jfieldID* (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i8*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, %struct._jobject*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i8)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i8)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i16)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i16)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i64)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, float)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, double)*, %struct._jobject* (%struct.JNINativeInterface_**, i16*, i32)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i16* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i16*)*, %struct._jobject* (%struct.JNINativeInterface_**, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i8* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, i32, %struct._jobject*, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, i8* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i8* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i16* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i16* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i32* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i64* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, float* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, double* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i16*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i16*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i64*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, float*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, double*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i16*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i16*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i32*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i64*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, float*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, double*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i16*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i16*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i32*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i64*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, float*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, double*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct.JNINativeMethod*, i32)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, %struct.JNIInvokeInterface_***)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i16*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i8*)*, i8* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i32)*, i16* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i16*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, void (%struct.JNINativeInterface_**, %struct._jobject*)*, i8 (%struct.JNINativeInterface_**)*, %struct._jobject* (%struct.JNINativeInterface_**, i8*, i64)*, i8* (%struct.JNINativeInterface_**, %struct._jobject*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)* }
%struct._jfieldID = type opaque
%union.jvalue = type { i64 }
%struct.JNINativeMethod = type { i8*, i8*, i8* }
%struct.JNIInvokeInterface_ = type { i8*, i8*, i8*, i32 (%struct.JNIInvokeInterface_**)*, i32 (%struct.JNIInvokeInterface_**, i8**, i8*)*, i32 (%struct.JNIInvokeInterface_**)*, i32 (%struct.JNIInvokeInterface_**, i8**, i32)*, i32 (%struct.JNIInvokeInterface_**, i8**, i8*)* }
%struct._jobject = type opaque
%struct._jmethodID = type opaque
%struct.__va_list = type { i8* }

; Function Attrs: alwaysinline nounwind
define dso_local %struct._jobject* @JNI_CallObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca %struct._jobject*, align 4
  %16 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %17 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_start(i8* %17)
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %19 = load %struct._jobject*, %struct._jobject** %13, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %21 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %16, i32 0, i32 0
  %22 = bitcast i8** %21 to [1 x i32]*
  %23 = load [1 x i32], [1 x i32]* %22, align 4
  %24 = call i8* @llvm.stacksave()
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  store [1 x i32] %23, [1 x i32]* %26, align 4
  store %struct.JNINativeInterface_** %18, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %19, %struct._jobject** %6, align 4
  store %struct._jmethodID* %20, %struct._jmethodID** %7, align 4
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %28 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %27, align 4
  %29 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %28, i32 0, i32 0
  %30 = load i8*, i8** %29, align 4
  %31 = bitcast i8* %30 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %32 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %33 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %35 = call i32 %31(%struct.JNINativeInterface_** noundef %32, %struct._jmethodID* noundef %33, i8* noundef %34) #2
  store i32 %35, i32* %10, align 4
  %36 = load i32, i32* %10, align 4
  %37 = mul i32 %36, 8
  %38 = alloca i8, i32 %37, align 8
  %39 = bitcast i8* %38 to %union.jvalue*
  store %union.jvalue* %39, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %40

40:                                               ; preds = %155, %3
  %41 = load i32, i32* %11, align 4
  %42 = load i32, i32* %10, align 4
  %43 = icmp slt i32 %41, %42
  br i1 %43, label %44, label %158

44:                                               ; preds = %40
  %45 = load i32, i32* %11, align 4
  %46 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  switch i32 %48, label %155 [
    i32 90, label %49
    i32 66, label %60
    i32 83, label %71
    i32 67, label %82
    i32 73, label %94
    i32 74, label %104
    i32 68, label %115
    i32 70, label %129
    i32 76, label %144
  ]

49:                                               ; preds = %44
  %50 = bitcast %struct.__va_list* %4 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %8, align 4
  %57 = load i32, i32* %11, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %155

60:                                               ; preds = %44
  %61 = bitcast %struct.__va_list* %4 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i8
  %67 = load %union.jvalue*, %union.jvalue** %8, align 4
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i8*
  store i8 %66, i8* %70, align 8
  br label %155

71:                                               ; preds = %44
  %72 = bitcast %struct.__va_list* %4 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = load %union.jvalue*, %union.jvalue** %8, align 4
  %79 = load i32, i32* %11, align 4
  %80 = getelementptr inbounds %union.jvalue, %union.jvalue* %78, i32 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %155

82:                                               ; preds = %44
  %83 = bitcast %struct.__va_list* %4 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = zext i16 %88 to i32
  %90 = load %union.jvalue*, %union.jvalue** %8, align 4
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds %union.jvalue, %union.jvalue* %90, i32 %91
  %93 = bitcast %union.jvalue* %92 to i32*
  store i32 %89, i32* %93, align 8
  br label %155

94:                                               ; preds = %44
  %95 = bitcast %struct.__va_list* %4 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = getelementptr inbounds i8, i8* %96, i32 4
  store i8* %97, i8** %95, align 4
  %98 = bitcast i8* %96 to i32*
  %99 = load i32, i32* %98, align 4
  %100 = load %union.jvalue*, %union.jvalue** %8, align 4
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i32*
  store i32 %99, i32* %103, align 8
  br label %155

104:                                              ; preds = %44
  %105 = bitcast %struct.__va_list* %4 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = getelementptr inbounds i8, i8* %106, i32 4
  store i8* %107, i8** %105, align 4
  %108 = bitcast i8* %106 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = sext i32 %109 to i64
  %111 = load %union.jvalue*, %union.jvalue** %8, align 4
  %112 = load i32, i32* %11, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %111, i32 %112
  %114 = bitcast %union.jvalue* %113 to i64*
  store i64 %110, i64* %114, align 8
  br label %155

115:                                              ; preds = %44
  %116 = bitcast %struct.__va_list* %4 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = ptrtoint i8* %117 to i32
  %119 = add i32 %118, 7
  %120 = and i32 %119, -8
  %121 = inttoptr i32 %120 to i8*
  %122 = getelementptr inbounds i8, i8* %121, i32 8
  store i8* %122, i8** %116, align 4
  %123 = bitcast i8* %121 to double*
  %124 = load double, double* %123, align 8
  %125 = load %union.jvalue*, %union.jvalue** %8, align 4
  %126 = load i32, i32* %11, align 4
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %125, i32 %126
  %128 = bitcast %union.jvalue* %127 to double*
  store double %124, double* %128, align 8
  br label %155

129:                                              ; preds = %44
  %130 = bitcast %struct.__va_list* %4 to i8**
  %131 = load i8*, i8** %130, align 4
  %132 = ptrtoint i8* %131 to i32
  %133 = add i32 %132, 7
  %134 = and i32 %133, -8
  %135 = inttoptr i32 %134 to i8*
  %136 = getelementptr inbounds i8, i8* %135, i32 8
  store i8* %136, i8** %130, align 4
  %137 = bitcast i8* %135 to double*
  %138 = load double, double* %137, align 8
  %139 = fptrunc double %138 to float
  %140 = load %union.jvalue*, %union.jvalue** %8, align 4
  %141 = load i32, i32* %11, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %139, float* %143, align 8
  br label %155

144:                                              ; preds = %44
  %145 = bitcast %struct.__va_list* %4 to i8**
  %146 = load i8*, i8** %145, align 4
  %147 = getelementptr inbounds i8, i8* %146, i32 4
  store i8* %147, i8** %145, align 4
  %148 = bitcast i8* %146 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = bitcast i8* %149 to %struct._jobject*
  %151 = load %union.jvalue*, %union.jvalue** %8, align 4
  %152 = load i32, i32* %11, align 4
  %153 = getelementptr inbounds %union.jvalue, %union.jvalue* %151, i32 %152
  %154 = bitcast %union.jvalue* %153 to %struct._jobject**
  store %struct._jobject* %150, %struct._jobject** %154, align 8
  br label %155

155:                                              ; preds = %144, %129, %115, %104, %94, %82, %71, %60, %49, %44
  %156 = load i32, i32* %11, align 4
  %157 = add nsw i32 %156, 1
  store i32 %157, i32* %11, align 4
  br label %40, !llvm.loop !10

158:                                              ; preds = %40
  %159 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %160 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %159, align 4
  %161 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %160, i32 0, i32 36
  %162 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %161, align 4
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %164 = load %struct._jobject*, %struct._jobject** %6, align 4
  %165 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %166 = load %union.jvalue*, %union.jvalue** %8, align 4
  %167 = call %struct._jobject* %162(%struct.JNINativeInterface_** noundef %163, %struct._jobject* noundef %164, %struct._jmethodID* noundef %165, %union.jvalue* noundef %166) #2
  call void @llvm.stackrestore(i8* %24)
  store %struct._jobject* %167, %struct._jobject** %15, align 4
  %168 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_end(i8* %168)
  %169 = load %struct._jobject*, %struct._jobject** %15, align 4
  ret %struct._jobject* %169
}

; Function Attrs: nofree nosync nounwind willreturn
declare void @llvm.va_start(i8*) #1

; Function Attrs: alwaysinline nounwind
define dso_local %struct._jobject* @JNI_CallObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !10

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 36
  %153 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  %158 = call %struct._jobject* %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret %struct._jobject* %158
}

; Function Attrs: nofree nosync nounwind willreturn
declare void @llvm.va_end(i8*) #1

; Function Attrs: alwaysinline nounwind
define dso_local %struct._jobject* @JNI_CallNonvirtualObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jmethodID*, align 4
  %10 = alloca %union.jvalue*, align 4
  %11 = alloca [257 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca %struct.JNINativeInterface_**, align 4
  %15 = alloca %struct._jobject*, align 4
  %16 = alloca %struct._jobject*, align 4
  %17 = alloca %struct._jmethodID*, align 4
  %18 = alloca %struct._jobject*, align 4
  %19 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %14, align 4
  store %struct._jobject* %1, %struct._jobject** %15, align 4
  store %struct._jobject* %2, %struct._jobject** %16, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %17, align 4
  %20 = bitcast %struct.__va_list* %19 to i8*
  call void @llvm.va_start(i8* %20)
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %14, align 4
  %22 = load %struct._jobject*, %struct._jobject** %15, align 4
  %23 = load %struct._jobject*, %struct._jobject** %16, align 4
  %24 = load %struct._jmethodID*, %struct._jmethodID** %17, align 4
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %19, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  %27 = load [1 x i32], [1 x i32]* %26, align 4
  %28 = call i8* @llvm.stacksave()
  %29 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %30 = bitcast i8** %29 to [1 x i32]*
  store [1 x i32] %27, [1 x i32]* %30, align 4
  store %struct.JNINativeInterface_** %21, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %22, %struct._jobject** %7, align 4
  store %struct._jobject* %23, %struct._jobject** %8, align 4
  store %struct._jmethodID* %24, %struct._jmethodID** %9, align 4
  %31 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %32 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %31, align 4
  %33 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %32, i32 0, i32 0
  %34 = load i8*, i8** %33, align 4
  %35 = bitcast i8* %34 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %36 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %37 = load %struct._jmethodID*, %struct._jmethodID** %9, align 4
  %38 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i32 0, i32 0
  %39 = call i32 %35(%struct.JNINativeInterface_** noundef %36, %struct._jmethodID* noundef %37, i8* noundef %38) #2
  store i32 %39, i32* %12, align 4
  %40 = load i32, i32* %12, align 4
  %41 = mul i32 %40, 8
  %42 = alloca i8, i32 %41, align 8
  %43 = bitcast i8* %42 to %union.jvalue*
  store %union.jvalue* %43, %union.jvalue** %10, align 4
  store i32 0, i32* %13, align 4
  br label %44

44:                                               ; preds = %159, %4
  %45 = load i32, i32* %13, align 4
  %46 = load i32, i32* %12, align 4
  %47 = icmp slt i32 %45, %46
  br i1 %47, label %48, label %162

48:                                               ; preds = %44
  %49 = load i32, i32* %13, align 4
  %50 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i32 0, i32 %49
  %51 = load i8, i8* %50, align 1
  %52 = zext i8 %51 to i32
  switch i32 %52, label %159 [
    i32 90, label %53
    i32 66, label %64
    i32 83, label %75
    i32 67, label %86
    i32 73, label %98
    i32 74, label %108
    i32 68, label %119
    i32 70, label %133
    i32 76, label %148
  ]

53:                                               ; preds = %48
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %10, align 4
  %61 = load i32, i32* %13, align 4
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i32 %61
  %63 = bitcast %union.jvalue* %62 to i8*
  store i8 %59, i8* %63, align 8
  br label %159

64:                                               ; preds = %48
  %65 = bitcast %struct.__va_list* %5 to i8**
  %66 = load i8*, i8** %65, align 4
  %67 = getelementptr inbounds i8, i8* %66, i32 4
  store i8* %67, i8** %65, align 4
  %68 = bitcast i8* %66 to i32*
  %69 = load i32, i32* %68, align 4
  %70 = trunc i32 %69 to i8
  %71 = load %union.jvalue*, %union.jvalue** %10, align 4
  %72 = load i32, i32* %13, align 4
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %71, i32 %72
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %70, i8* %74, align 8
  br label %159

75:                                               ; preds = %48
  %76 = bitcast %struct.__va_list* %5 to i8**
  %77 = load i8*, i8** %76, align 4
  %78 = getelementptr inbounds i8, i8* %77, i32 4
  store i8* %78, i8** %76, align 4
  %79 = bitcast i8* %77 to i32*
  %80 = load i32, i32* %79, align 4
  %81 = trunc i32 %80 to i16
  %82 = load %union.jvalue*, %union.jvalue** %10, align 4
  %83 = load i32, i32* %13, align 4
  %84 = getelementptr inbounds %union.jvalue, %union.jvalue* %82, i32 %83
  %85 = bitcast %union.jvalue* %84 to i16*
  store i16 %81, i16* %85, align 8
  br label %159

86:                                               ; preds = %48
  %87 = bitcast %struct.__va_list* %5 to i8**
  %88 = load i8*, i8** %87, align 4
  %89 = getelementptr inbounds i8, i8* %88, i32 4
  store i8* %89, i8** %87, align 4
  %90 = bitcast i8* %88 to i32*
  %91 = load i32, i32* %90, align 4
  %92 = trunc i32 %91 to i16
  %93 = zext i16 %92 to i32
  %94 = load %union.jvalue*, %union.jvalue** %10, align 4
  %95 = load i32, i32* %13, align 4
  %96 = getelementptr inbounds %union.jvalue, %union.jvalue* %94, i32 %95
  %97 = bitcast %union.jvalue* %96 to i32*
  store i32 %93, i32* %97, align 8
  br label %159

98:                                               ; preds = %48
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load %union.jvalue*, %union.jvalue** %10, align 4
  %105 = load i32, i32* %13, align 4
  %106 = getelementptr inbounds %union.jvalue, %union.jvalue* %104, i32 %105
  %107 = bitcast %union.jvalue* %106 to i32*
  store i32 %103, i32* %107, align 8
  br label %159

108:                                              ; preds = %48
  %109 = bitcast %struct.__va_list* %5 to i8**
  %110 = load i8*, i8** %109, align 4
  %111 = getelementptr inbounds i8, i8* %110, i32 4
  store i8* %111, i8** %109, align 4
  %112 = bitcast i8* %110 to i32*
  %113 = load i32, i32* %112, align 4
  %114 = sext i32 %113 to i64
  %115 = load %union.jvalue*, %union.jvalue** %10, align 4
  %116 = load i32, i32* %13, align 4
  %117 = getelementptr inbounds %union.jvalue, %union.jvalue* %115, i32 %116
  %118 = bitcast %union.jvalue* %117 to i64*
  store i64 %114, i64* %118, align 8
  br label %159

119:                                              ; preds = %48
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = ptrtoint i8* %121 to i32
  %123 = add i32 %122, 7
  %124 = and i32 %123, -8
  %125 = inttoptr i32 %124 to i8*
  %126 = getelementptr inbounds i8, i8* %125, i32 8
  store i8* %126, i8** %120, align 4
  %127 = bitcast i8* %125 to double*
  %128 = load double, double* %127, align 8
  %129 = load %union.jvalue*, %union.jvalue** %10, align 4
  %130 = load i32, i32* %13, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to double*
  store double %128, double* %132, align 8
  br label %159

133:                                              ; preds = %48
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = ptrtoint i8* %135 to i32
  %137 = add i32 %136, 7
  %138 = and i32 %137, -8
  %139 = inttoptr i32 %138 to i8*
  %140 = getelementptr inbounds i8, i8* %139, i32 8
  store i8* %140, i8** %134, align 4
  %141 = bitcast i8* %139 to double*
  %142 = load double, double* %141, align 8
  %143 = fptrunc double %142 to float
  %144 = load %union.jvalue*, %union.jvalue** %10, align 4
  %145 = load i32, i32* %13, align 4
  %146 = getelementptr inbounds %union.jvalue, %union.jvalue* %144, i32 %145
  %147 = bitcast %union.jvalue* %146 to float*
  store float %143, float* %147, align 8
  br label %159

148:                                              ; preds = %48
  %149 = bitcast %struct.__va_list* %5 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = getelementptr inbounds i8, i8* %150, i32 4
  store i8* %151, i8** %149, align 4
  %152 = bitcast i8* %150 to i8**
  %153 = load i8*, i8** %152, align 4
  %154 = bitcast i8* %153 to %struct._jobject*
  %155 = load %union.jvalue*, %union.jvalue** %10, align 4
  %156 = load i32, i32* %13, align 4
  %157 = getelementptr inbounds %union.jvalue, %union.jvalue* %155, i32 %156
  %158 = bitcast %union.jvalue* %157 to %struct._jobject**
  store %struct._jobject* %154, %struct._jobject** %158, align 8
  br label %159

159:                                              ; preds = %148, %133, %119, %108, %98, %86, %75, %64, %53, %48
  %160 = load i32, i32* %13, align 4
  %161 = add nsw i32 %160, 1
  store i32 %161, i32* %13, align 4
  br label %44, !llvm.loop !12

162:                                              ; preds = %44
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %164 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %163, align 4
  %165 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %164, i32 0, i32 66
  %166 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %165, align 4
  %167 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %168 = load %struct._jobject*, %struct._jobject** %7, align 4
  %169 = load %struct._jobject*, %struct._jobject** %8, align 4
  %170 = load %struct._jmethodID*, %struct._jmethodID** %9, align 4
  %171 = load %union.jvalue*, %union.jvalue** %10, align 4
  %172 = call %struct._jobject* %166(%struct.JNINativeInterface_** noundef %167, %struct._jobject* noundef %168, %struct._jobject* noundef %169, %struct._jmethodID* noundef %170, %union.jvalue* noundef %171) #2
  call void @llvm.stackrestore(i8* %28)
  store %struct._jobject* %172, %struct._jobject** %18, align 4
  %173 = bitcast %struct.__va_list* %19 to i8*
  call void @llvm.va_end(i8* %173)
  %174 = load %struct._jobject*, %struct._jobject** %18, align 4
  ret %struct._jobject* %174
}

; Function Attrs: alwaysinline nounwind
define dso_local %struct._jobject* @JNI_CallNonvirtualObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca %union.jvalue*, align 4
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %6, i32 0, i32 0
  %16 = bitcast i8** %15 to [1 x i32]*
  store [1 x i32] %4, [1 x i32]* %16, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %7, align 4
  store %struct._jobject* %1, %struct._jobject** %8, align 4
  store %struct._jobject* %2, %struct._jobject** %9, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %10, align 4
  br label %17

17:                                               ; preds = %5
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %19 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %18, align 4
  %20 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %19, i32 0, i32 0
  %21 = load i8*, i8** %20, align 4
  %22 = bitcast i8* %21 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %24 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i32 0, i32 0
  %26 = call i32 %22(%struct.JNINativeInterface_** noundef %23, %struct._jmethodID* noundef %24, i8* noundef %25)
  store i32 %26, i32* %13, align 4
  %27 = load i32, i32* %13, align 4
  %28 = mul i32 %27, 8
  %29 = alloca i8, i32 %28, align 8
  %30 = bitcast i8* %29 to %union.jvalue*
  store %union.jvalue* %30, %union.jvalue** %11, align 4
  store i32 0, i32* %14, align 4
  br label %31

31:                                               ; preds = %147, %17
  %32 = load i32, i32* %14, align 4
  %33 = load i32, i32* %13, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %150

35:                                               ; preds = %31
  %36 = load i32, i32* %14, align 4
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i32 0, i32 %36
  %38 = load i8, i8* %37, align 1
  %39 = zext i8 %38 to i32
  switch i32 %39, label %146 [
    i32 90, label %40
    i32 66, label %51
    i32 83, label %62
    i32 67, label %73
    i32 73, label %85
    i32 74, label %95
    i32 68, label %106
    i32 70, label %120
    i32 76, label %135
  ]

40:                                               ; preds = %35
  %41 = bitcast %struct.__va_list* %6 to i8**
  %42 = load i8*, i8** %41, align 4
  %43 = getelementptr inbounds i8, i8* %42, i32 4
  store i8* %43, i8** %41, align 4
  %44 = bitcast i8* %42 to i32*
  %45 = load i32, i32* %44, align 4
  %46 = trunc i32 %45 to i8
  %47 = load %union.jvalue*, %union.jvalue** %11, align 4
  %48 = load i32, i32* %14, align 4
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %47, i32 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %146

51:                                               ; preds = %35
  %52 = bitcast %struct.__va_list* %6 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %11, align 4
  %59 = load i32, i32* %14, align 4
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i32 %59
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %57, i8* %61, align 8
  br label %146

62:                                               ; preds = %35
  %63 = bitcast %struct.__va_list* %6 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load %union.jvalue*, %union.jvalue** %11, align 4
  %70 = load i32, i32* %14, align 4
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %69, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %68, i16* %72, align 8
  br label %146

73:                                               ; preds = %35
  %74 = bitcast %struct.__va_list* %6 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = trunc i32 %78 to i16
  %80 = zext i16 %79 to i32
  %81 = load %union.jvalue*, %union.jvalue** %11, align 4
  %82 = load i32, i32* %14, align 4
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %81, i32 %82
  %84 = bitcast %union.jvalue* %83 to i32*
  store i32 %80, i32* %84, align 8
  br label %146

85:                                               ; preds = %35
  %86 = bitcast %struct.__va_list* %6 to i8**
  %87 = load i8*, i8** %86, align 4
  %88 = getelementptr inbounds i8, i8* %87, i32 4
  store i8* %88, i8** %86, align 4
  %89 = bitcast i8* %87 to i32*
  %90 = load i32, i32* %89, align 4
  %91 = load %union.jvalue*, %union.jvalue** %11, align 4
  %92 = load i32, i32* %14, align 4
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %91, i32 %92
  %94 = bitcast %union.jvalue* %93 to i32*
  store i32 %90, i32* %94, align 8
  br label %146

95:                                               ; preds = %35
  %96 = bitcast %struct.__va_list* %6 to i8**
  %97 = load i8*, i8** %96, align 4
  %98 = getelementptr inbounds i8, i8* %97, i32 4
  store i8* %98, i8** %96, align 4
  %99 = bitcast i8* %97 to i32*
  %100 = load i32, i32* %99, align 4
  %101 = sext i32 %100 to i64
  %102 = load %union.jvalue*, %union.jvalue** %11, align 4
  %103 = load i32, i32* %14, align 4
  %104 = getelementptr inbounds %union.jvalue, %union.jvalue* %102, i32 %103
  %105 = bitcast %union.jvalue* %104 to i64*
  store i64 %101, i64* %105, align 8
  br label %146

106:                                              ; preds = %35
  %107 = bitcast %struct.__va_list* %6 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load %union.jvalue*, %union.jvalue** %11, align 4
  %117 = load i32, i32* %14, align 4
  %118 = getelementptr inbounds %union.jvalue, %union.jvalue* %116, i32 %117
  %119 = bitcast %union.jvalue* %118 to double*
  store double %115, double* %119, align 8
  br label %146

120:                                              ; preds = %35
  %121 = bitcast %struct.__va_list* %6 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = ptrtoint i8* %122 to i32
  %124 = add i32 %123, 7
  %125 = and i32 %124, -8
  %126 = inttoptr i32 %125 to i8*
  %127 = getelementptr inbounds i8, i8* %126, i32 8
  store i8* %127, i8** %121, align 4
  %128 = bitcast i8* %126 to double*
  %129 = load double, double* %128, align 8
  %130 = fptrunc double %129 to float
  %131 = load %union.jvalue*, %union.jvalue** %11, align 4
  %132 = load i32, i32* %14, align 4
  %133 = getelementptr inbounds %union.jvalue, %union.jvalue* %131, i32 %132
  %134 = bitcast %union.jvalue* %133 to float*
  store float %130, float* %134, align 8
  br label %146

135:                                              ; preds = %35
  %136 = bitcast %struct.__va_list* %6 to i8**
  %137 = load i8*, i8** %136, align 4
  %138 = getelementptr inbounds i8, i8* %137, i32 4
  store i8* %138, i8** %136, align 4
  %139 = bitcast i8* %137 to i8**
  %140 = load i8*, i8** %139, align 4
  %141 = bitcast i8* %140 to %struct._jobject*
  %142 = load %union.jvalue*, %union.jvalue** %11, align 4
  %143 = load i32, i32* %14, align 4
  %144 = getelementptr inbounds %union.jvalue, %union.jvalue* %142, i32 %143
  %145 = bitcast %union.jvalue* %144 to %struct._jobject**
  store %struct._jobject* %141, %struct._jobject** %145, align 8
  br label %146

146:                                              ; preds = %35, %135, %120, %106, %95, %85, %73, %62, %51, %40
  br label %147

147:                                              ; preds = %146
  %148 = load i32, i32* %14, align 4
  %149 = add nsw i32 %148, 1
  store i32 %149, i32* %14, align 4
  br label %31, !llvm.loop !12

150:                                              ; preds = %31
  br label %151

151:                                              ; preds = %150
  %152 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %153 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %152, align 4
  %154 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %153, i32 0, i32 66
  %155 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %154, align 4
  %156 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %157 = load %struct._jobject*, %struct._jobject** %8, align 4
  %158 = load %struct._jobject*, %struct._jobject** %9, align 4
  %159 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %160 = load %union.jvalue*, %union.jvalue** %11, align 4
  %161 = call %struct._jobject* %155(%struct.JNINativeInterface_** noundef %156, %struct._jobject* noundef %157, %struct._jobject* noundef %158, %struct._jmethodID* noundef %159, %union.jvalue* noundef %160)
  ret %struct._jobject* %161
}

; Function Attrs: alwaysinline nounwind
define dso_local %struct._jobject* @JNI_CallStaticObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca %struct._jobject*, align 4
  %16 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %17 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_start(i8* %17)
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %19 = load %struct._jobject*, %struct._jobject** %13, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %21 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %16, i32 0, i32 0
  %22 = bitcast i8** %21 to [1 x i32]*
  %23 = load [1 x i32], [1 x i32]* %22, align 4
  %24 = call i8* @llvm.stacksave()
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  store [1 x i32] %23, [1 x i32]* %26, align 4
  store %struct.JNINativeInterface_** %18, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %19, %struct._jobject** %6, align 4
  store %struct._jmethodID* %20, %struct._jmethodID** %7, align 4
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %28 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %27, align 4
  %29 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %28, i32 0, i32 0
  %30 = load i8*, i8** %29, align 4
  %31 = bitcast i8* %30 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %32 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %33 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %35 = call i32 %31(%struct.JNINativeInterface_** noundef %32, %struct._jmethodID* noundef %33, i8* noundef %34) #2
  store i32 %35, i32* %10, align 4
  %36 = load i32, i32* %10, align 4
  %37 = mul i32 %36, 8
  %38 = alloca i8, i32 %37, align 8
  %39 = bitcast i8* %38 to %union.jvalue*
  store %union.jvalue* %39, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %40

40:                                               ; preds = %155, %3
  %41 = load i32, i32* %11, align 4
  %42 = load i32, i32* %10, align 4
  %43 = icmp slt i32 %41, %42
  br i1 %43, label %44, label %158

44:                                               ; preds = %40
  %45 = load i32, i32* %11, align 4
  %46 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  switch i32 %48, label %155 [
    i32 90, label %49
    i32 66, label %60
    i32 83, label %71
    i32 67, label %82
    i32 73, label %94
    i32 74, label %104
    i32 68, label %115
    i32 70, label %129
    i32 76, label %144
  ]

49:                                               ; preds = %44
  %50 = bitcast %struct.__va_list* %4 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %8, align 4
  %57 = load i32, i32* %11, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %155

60:                                               ; preds = %44
  %61 = bitcast %struct.__va_list* %4 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i8
  %67 = load %union.jvalue*, %union.jvalue** %8, align 4
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i8*
  store i8 %66, i8* %70, align 8
  br label %155

71:                                               ; preds = %44
  %72 = bitcast %struct.__va_list* %4 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = load %union.jvalue*, %union.jvalue** %8, align 4
  %79 = load i32, i32* %11, align 4
  %80 = getelementptr inbounds %union.jvalue, %union.jvalue* %78, i32 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %155

82:                                               ; preds = %44
  %83 = bitcast %struct.__va_list* %4 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = zext i16 %88 to i32
  %90 = load %union.jvalue*, %union.jvalue** %8, align 4
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds %union.jvalue, %union.jvalue* %90, i32 %91
  %93 = bitcast %union.jvalue* %92 to i32*
  store i32 %89, i32* %93, align 8
  br label %155

94:                                               ; preds = %44
  %95 = bitcast %struct.__va_list* %4 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = getelementptr inbounds i8, i8* %96, i32 4
  store i8* %97, i8** %95, align 4
  %98 = bitcast i8* %96 to i32*
  %99 = load i32, i32* %98, align 4
  %100 = load %union.jvalue*, %union.jvalue** %8, align 4
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i32*
  store i32 %99, i32* %103, align 8
  br label %155

104:                                              ; preds = %44
  %105 = bitcast %struct.__va_list* %4 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = getelementptr inbounds i8, i8* %106, i32 4
  store i8* %107, i8** %105, align 4
  %108 = bitcast i8* %106 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = sext i32 %109 to i64
  %111 = load %union.jvalue*, %union.jvalue** %8, align 4
  %112 = load i32, i32* %11, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %111, i32 %112
  %114 = bitcast %union.jvalue* %113 to i64*
  store i64 %110, i64* %114, align 8
  br label %155

115:                                              ; preds = %44
  %116 = bitcast %struct.__va_list* %4 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = ptrtoint i8* %117 to i32
  %119 = add i32 %118, 7
  %120 = and i32 %119, -8
  %121 = inttoptr i32 %120 to i8*
  %122 = getelementptr inbounds i8, i8* %121, i32 8
  store i8* %122, i8** %116, align 4
  %123 = bitcast i8* %121 to double*
  %124 = load double, double* %123, align 8
  %125 = load %union.jvalue*, %union.jvalue** %8, align 4
  %126 = load i32, i32* %11, align 4
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %125, i32 %126
  %128 = bitcast %union.jvalue* %127 to double*
  store double %124, double* %128, align 8
  br label %155

129:                                              ; preds = %44
  %130 = bitcast %struct.__va_list* %4 to i8**
  %131 = load i8*, i8** %130, align 4
  %132 = ptrtoint i8* %131 to i32
  %133 = add i32 %132, 7
  %134 = and i32 %133, -8
  %135 = inttoptr i32 %134 to i8*
  %136 = getelementptr inbounds i8, i8* %135, i32 8
  store i8* %136, i8** %130, align 4
  %137 = bitcast i8* %135 to double*
  %138 = load double, double* %137, align 8
  %139 = fptrunc double %138 to float
  %140 = load %union.jvalue*, %union.jvalue** %8, align 4
  %141 = load i32, i32* %11, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %139, float* %143, align 8
  br label %155

144:                                              ; preds = %44
  %145 = bitcast %struct.__va_list* %4 to i8**
  %146 = load i8*, i8** %145, align 4
  %147 = getelementptr inbounds i8, i8* %146, i32 4
  store i8* %147, i8** %145, align 4
  %148 = bitcast i8* %146 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = bitcast i8* %149 to %struct._jobject*
  %151 = load %union.jvalue*, %union.jvalue** %8, align 4
  %152 = load i32, i32* %11, align 4
  %153 = getelementptr inbounds %union.jvalue, %union.jvalue* %151, i32 %152
  %154 = bitcast %union.jvalue* %153 to %struct._jobject**
  store %struct._jobject* %150, %struct._jobject** %154, align 8
  br label %155

155:                                              ; preds = %144, %129, %115, %104, %94, %82, %71, %60, %49, %44
  %156 = load i32, i32* %11, align 4
  %157 = add nsw i32 %156, 1
  store i32 %157, i32* %11, align 4
  br label %40, !llvm.loop !13

158:                                              ; preds = %40
  %159 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %160 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %159, align 4
  %161 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %160, i32 0, i32 116
  %162 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %161, align 4
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %164 = load %struct._jobject*, %struct._jobject** %6, align 4
  %165 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %166 = load %union.jvalue*, %union.jvalue** %8, align 4
  %167 = call %struct._jobject* %162(%struct.JNINativeInterface_** noundef %163, %struct._jobject* noundef %164, %struct._jmethodID* noundef %165, %union.jvalue* noundef %166) #2
  call void @llvm.stackrestore(i8* %24)
  store %struct._jobject* %167, %struct._jobject** %15, align 4
  %168 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_end(i8* %168)
  %169 = load %struct._jobject*, %struct._jobject** %15, align 4
  ret %struct._jobject* %169
}

; Function Attrs: alwaysinline nounwind
define dso_local %struct._jobject* @JNI_CallStaticObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !13

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 116
  %153 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  %158 = call %struct._jobject* %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret %struct._jobject* %158
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i8 @JNI_CallBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca i8, align 1
  %16 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %17 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_start(i8* %17)
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %19 = load %struct._jobject*, %struct._jobject** %13, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %21 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %16, i32 0, i32 0
  %22 = bitcast i8** %21 to [1 x i32]*
  %23 = load [1 x i32], [1 x i32]* %22, align 4
  %24 = call i8* @llvm.stacksave()
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  store [1 x i32] %23, [1 x i32]* %26, align 4
  store %struct.JNINativeInterface_** %18, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %19, %struct._jobject** %6, align 4
  store %struct._jmethodID* %20, %struct._jmethodID** %7, align 4
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %28 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %27, align 4
  %29 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %28, i32 0, i32 0
  %30 = load i8*, i8** %29, align 4
  %31 = bitcast i8* %30 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %32 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %33 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %35 = call i32 %31(%struct.JNINativeInterface_** noundef %32, %struct._jmethodID* noundef %33, i8* noundef %34) #2
  store i32 %35, i32* %10, align 4
  %36 = load i32, i32* %10, align 4
  %37 = mul i32 %36, 8
  %38 = alloca i8, i32 %37, align 8
  %39 = bitcast i8* %38 to %union.jvalue*
  store %union.jvalue* %39, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %40

40:                                               ; preds = %155, %3
  %41 = load i32, i32* %11, align 4
  %42 = load i32, i32* %10, align 4
  %43 = icmp slt i32 %41, %42
  br i1 %43, label %44, label %158

44:                                               ; preds = %40
  %45 = load i32, i32* %11, align 4
  %46 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  switch i32 %48, label %155 [
    i32 90, label %49
    i32 66, label %60
    i32 83, label %71
    i32 67, label %82
    i32 73, label %94
    i32 74, label %104
    i32 68, label %115
    i32 70, label %129
    i32 76, label %144
  ]

49:                                               ; preds = %44
  %50 = bitcast %struct.__va_list* %4 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %8, align 4
  %57 = load i32, i32* %11, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %155

60:                                               ; preds = %44
  %61 = bitcast %struct.__va_list* %4 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i8
  %67 = load %union.jvalue*, %union.jvalue** %8, align 4
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i8*
  store i8 %66, i8* %70, align 8
  br label %155

71:                                               ; preds = %44
  %72 = bitcast %struct.__va_list* %4 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = load %union.jvalue*, %union.jvalue** %8, align 4
  %79 = load i32, i32* %11, align 4
  %80 = getelementptr inbounds %union.jvalue, %union.jvalue* %78, i32 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %155

82:                                               ; preds = %44
  %83 = bitcast %struct.__va_list* %4 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = zext i16 %88 to i32
  %90 = load %union.jvalue*, %union.jvalue** %8, align 4
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds %union.jvalue, %union.jvalue* %90, i32 %91
  %93 = bitcast %union.jvalue* %92 to i32*
  store i32 %89, i32* %93, align 8
  br label %155

94:                                               ; preds = %44
  %95 = bitcast %struct.__va_list* %4 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = getelementptr inbounds i8, i8* %96, i32 4
  store i8* %97, i8** %95, align 4
  %98 = bitcast i8* %96 to i32*
  %99 = load i32, i32* %98, align 4
  %100 = load %union.jvalue*, %union.jvalue** %8, align 4
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i32*
  store i32 %99, i32* %103, align 8
  br label %155

104:                                              ; preds = %44
  %105 = bitcast %struct.__va_list* %4 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = getelementptr inbounds i8, i8* %106, i32 4
  store i8* %107, i8** %105, align 4
  %108 = bitcast i8* %106 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = sext i32 %109 to i64
  %111 = load %union.jvalue*, %union.jvalue** %8, align 4
  %112 = load i32, i32* %11, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %111, i32 %112
  %114 = bitcast %union.jvalue* %113 to i64*
  store i64 %110, i64* %114, align 8
  br label %155

115:                                              ; preds = %44
  %116 = bitcast %struct.__va_list* %4 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = ptrtoint i8* %117 to i32
  %119 = add i32 %118, 7
  %120 = and i32 %119, -8
  %121 = inttoptr i32 %120 to i8*
  %122 = getelementptr inbounds i8, i8* %121, i32 8
  store i8* %122, i8** %116, align 4
  %123 = bitcast i8* %121 to double*
  %124 = load double, double* %123, align 8
  %125 = load %union.jvalue*, %union.jvalue** %8, align 4
  %126 = load i32, i32* %11, align 4
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %125, i32 %126
  %128 = bitcast %union.jvalue* %127 to double*
  store double %124, double* %128, align 8
  br label %155

129:                                              ; preds = %44
  %130 = bitcast %struct.__va_list* %4 to i8**
  %131 = load i8*, i8** %130, align 4
  %132 = ptrtoint i8* %131 to i32
  %133 = add i32 %132, 7
  %134 = and i32 %133, -8
  %135 = inttoptr i32 %134 to i8*
  %136 = getelementptr inbounds i8, i8* %135, i32 8
  store i8* %136, i8** %130, align 4
  %137 = bitcast i8* %135 to double*
  %138 = load double, double* %137, align 8
  %139 = fptrunc double %138 to float
  %140 = load %union.jvalue*, %union.jvalue** %8, align 4
  %141 = load i32, i32* %11, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %139, float* %143, align 8
  br label %155

144:                                              ; preds = %44
  %145 = bitcast %struct.__va_list* %4 to i8**
  %146 = load i8*, i8** %145, align 4
  %147 = getelementptr inbounds i8, i8* %146, i32 4
  store i8* %147, i8** %145, align 4
  %148 = bitcast i8* %146 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = bitcast i8* %149 to %struct._jobject*
  %151 = load %union.jvalue*, %union.jvalue** %8, align 4
  %152 = load i32, i32* %11, align 4
  %153 = getelementptr inbounds %union.jvalue, %union.jvalue* %151, i32 %152
  %154 = bitcast %union.jvalue* %153 to %struct._jobject**
  store %struct._jobject* %150, %struct._jobject** %154, align 8
  br label %155

155:                                              ; preds = %144, %129, %115, %104, %94, %82, %71, %60, %49, %44
  %156 = load i32, i32* %11, align 4
  %157 = add nsw i32 %156, 1
  store i32 %157, i32* %11, align 4
  br label %40, !llvm.loop !14

158:                                              ; preds = %40
  %159 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %160 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %159, align 4
  %161 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %160, i32 0, i32 39
  %162 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %161, align 4
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %164 = load %struct._jobject*, %struct._jobject** %6, align 4
  %165 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %166 = load %union.jvalue*, %union.jvalue** %8, align 4
  %167 = call zeroext i8 %162(%struct.JNINativeInterface_** noundef %163, %struct._jobject* noundef %164, %struct._jmethodID* noundef %165, %union.jvalue* noundef %166) #2
  call void @llvm.stackrestore(i8* %24)
  store i8 %167, i8* %15, align 1
  %168 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_end(i8* %168)
  %169 = load i8, i8* %15, align 1
  ret i8 %169
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i8 @JNI_CallBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !14

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 39
  %153 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  %158 = call zeroext i8 %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret i8 %158
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i8 @JNI_CallNonvirtualBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jmethodID*, align 4
  %10 = alloca %union.jvalue*, align 4
  %11 = alloca [257 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca %struct.JNINativeInterface_**, align 4
  %15 = alloca %struct._jobject*, align 4
  %16 = alloca %struct._jobject*, align 4
  %17 = alloca %struct._jmethodID*, align 4
  %18 = alloca i8, align 1
  %19 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %14, align 4
  store %struct._jobject* %1, %struct._jobject** %15, align 4
  store %struct._jobject* %2, %struct._jobject** %16, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %17, align 4
  %20 = bitcast %struct.__va_list* %19 to i8*
  call void @llvm.va_start(i8* %20)
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %14, align 4
  %22 = load %struct._jobject*, %struct._jobject** %15, align 4
  %23 = load %struct._jobject*, %struct._jobject** %16, align 4
  %24 = load %struct._jmethodID*, %struct._jmethodID** %17, align 4
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %19, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  %27 = load [1 x i32], [1 x i32]* %26, align 4
  %28 = call i8* @llvm.stacksave()
  %29 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %30 = bitcast i8** %29 to [1 x i32]*
  store [1 x i32] %27, [1 x i32]* %30, align 4
  store %struct.JNINativeInterface_** %21, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %22, %struct._jobject** %7, align 4
  store %struct._jobject* %23, %struct._jobject** %8, align 4
  store %struct._jmethodID* %24, %struct._jmethodID** %9, align 4
  %31 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %32 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %31, align 4
  %33 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %32, i32 0, i32 0
  %34 = load i8*, i8** %33, align 4
  %35 = bitcast i8* %34 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %36 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %37 = load %struct._jmethodID*, %struct._jmethodID** %9, align 4
  %38 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i32 0, i32 0
  %39 = call i32 %35(%struct.JNINativeInterface_** noundef %36, %struct._jmethodID* noundef %37, i8* noundef %38) #2
  store i32 %39, i32* %12, align 4
  %40 = load i32, i32* %12, align 4
  %41 = mul i32 %40, 8
  %42 = alloca i8, i32 %41, align 8
  %43 = bitcast i8* %42 to %union.jvalue*
  store %union.jvalue* %43, %union.jvalue** %10, align 4
  store i32 0, i32* %13, align 4
  br label %44

44:                                               ; preds = %159, %4
  %45 = load i32, i32* %13, align 4
  %46 = load i32, i32* %12, align 4
  %47 = icmp slt i32 %45, %46
  br i1 %47, label %48, label %162

48:                                               ; preds = %44
  %49 = load i32, i32* %13, align 4
  %50 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i32 0, i32 %49
  %51 = load i8, i8* %50, align 1
  %52 = zext i8 %51 to i32
  switch i32 %52, label %159 [
    i32 90, label %53
    i32 66, label %64
    i32 83, label %75
    i32 67, label %86
    i32 73, label %98
    i32 74, label %108
    i32 68, label %119
    i32 70, label %133
    i32 76, label %148
  ]

53:                                               ; preds = %48
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %10, align 4
  %61 = load i32, i32* %13, align 4
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i32 %61
  %63 = bitcast %union.jvalue* %62 to i8*
  store i8 %59, i8* %63, align 8
  br label %159

64:                                               ; preds = %48
  %65 = bitcast %struct.__va_list* %5 to i8**
  %66 = load i8*, i8** %65, align 4
  %67 = getelementptr inbounds i8, i8* %66, i32 4
  store i8* %67, i8** %65, align 4
  %68 = bitcast i8* %66 to i32*
  %69 = load i32, i32* %68, align 4
  %70 = trunc i32 %69 to i8
  %71 = load %union.jvalue*, %union.jvalue** %10, align 4
  %72 = load i32, i32* %13, align 4
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %71, i32 %72
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %70, i8* %74, align 8
  br label %159

75:                                               ; preds = %48
  %76 = bitcast %struct.__va_list* %5 to i8**
  %77 = load i8*, i8** %76, align 4
  %78 = getelementptr inbounds i8, i8* %77, i32 4
  store i8* %78, i8** %76, align 4
  %79 = bitcast i8* %77 to i32*
  %80 = load i32, i32* %79, align 4
  %81 = trunc i32 %80 to i16
  %82 = load %union.jvalue*, %union.jvalue** %10, align 4
  %83 = load i32, i32* %13, align 4
  %84 = getelementptr inbounds %union.jvalue, %union.jvalue* %82, i32 %83
  %85 = bitcast %union.jvalue* %84 to i16*
  store i16 %81, i16* %85, align 8
  br label %159

86:                                               ; preds = %48
  %87 = bitcast %struct.__va_list* %5 to i8**
  %88 = load i8*, i8** %87, align 4
  %89 = getelementptr inbounds i8, i8* %88, i32 4
  store i8* %89, i8** %87, align 4
  %90 = bitcast i8* %88 to i32*
  %91 = load i32, i32* %90, align 4
  %92 = trunc i32 %91 to i16
  %93 = zext i16 %92 to i32
  %94 = load %union.jvalue*, %union.jvalue** %10, align 4
  %95 = load i32, i32* %13, align 4
  %96 = getelementptr inbounds %union.jvalue, %union.jvalue* %94, i32 %95
  %97 = bitcast %union.jvalue* %96 to i32*
  store i32 %93, i32* %97, align 8
  br label %159

98:                                               ; preds = %48
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load %union.jvalue*, %union.jvalue** %10, align 4
  %105 = load i32, i32* %13, align 4
  %106 = getelementptr inbounds %union.jvalue, %union.jvalue* %104, i32 %105
  %107 = bitcast %union.jvalue* %106 to i32*
  store i32 %103, i32* %107, align 8
  br label %159

108:                                              ; preds = %48
  %109 = bitcast %struct.__va_list* %5 to i8**
  %110 = load i8*, i8** %109, align 4
  %111 = getelementptr inbounds i8, i8* %110, i32 4
  store i8* %111, i8** %109, align 4
  %112 = bitcast i8* %110 to i32*
  %113 = load i32, i32* %112, align 4
  %114 = sext i32 %113 to i64
  %115 = load %union.jvalue*, %union.jvalue** %10, align 4
  %116 = load i32, i32* %13, align 4
  %117 = getelementptr inbounds %union.jvalue, %union.jvalue* %115, i32 %116
  %118 = bitcast %union.jvalue* %117 to i64*
  store i64 %114, i64* %118, align 8
  br label %159

119:                                              ; preds = %48
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = ptrtoint i8* %121 to i32
  %123 = add i32 %122, 7
  %124 = and i32 %123, -8
  %125 = inttoptr i32 %124 to i8*
  %126 = getelementptr inbounds i8, i8* %125, i32 8
  store i8* %126, i8** %120, align 4
  %127 = bitcast i8* %125 to double*
  %128 = load double, double* %127, align 8
  %129 = load %union.jvalue*, %union.jvalue** %10, align 4
  %130 = load i32, i32* %13, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to double*
  store double %128, double* %132, align 8
  br label %159

133:                                              ; preds = %48
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = ptrtoint i8* %135 to i32
  %137 = add i32 %136, 7
  %138 = and i32 %137, -8
  %139 = inttoptr i32 %138 to i8*
  %140 = getelementptr inbounds i8, i8* %139, i32 8
  store i8* %140, i8** %134, align 4
  %141 = bitcast i8* %139 to double*
  %142 = load double, double* %141, align 8
  %143 = fptrunc double %142 to float
  %144 = load %union.jvalue*, %union.jvalue** %10, align 4
  %145 = load i32, i32* %13, align 4
  %146 = getelementptr inbounds %union.jvalue, %union.jvalue* %144, i32 %145
  %147 = bitcast %union.jvalue* %146 to float*
  store float %143, float* %147, align 8
  br label %159

148:                                              ; preds = %48
  %149 = bitcast %struct.__va_list* %5 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = getelementptr inbounds i8, i8* %150, i32 4
  store i8* %151, i8** %149, align 4
  %152 = bitcast i8* %150 to i8**
  %153 = load i8*, i8** %152, align 4
  %154 = bitcast i8* %153 to %struct._jobject*
  %155 = load %union.jvalue*, %union.jvalue** %10, align 4
  %156 = load i32, i32* %13, align 4
  %157 = getelementptr inbounds %union.jvalue, %union.jvalue* %155, i32 %156
  %158 = bitcast %union.jvalue* %157 to %struct._jobject**
  store %struct._jobject* %154, %struct._jobject** %158, align 8
  br label %159

159:                                              ; preds = %148, %133, %119, %108, %98, %86, %75, %64, %53, %48
  %160 = load i32, i32* %13, align 4
  %161 = add nsw i32 %160, 1
  store i32 %161, i32* %13, align 4
  br label %44, !llvm.loop !15

162:                                              ; preds = %44
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %164 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %163, align 4
  %165 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %164, i32 0, i32 69
  %166 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %165, align 4
  %167 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %168 = load %struct._jobject*, %struct._jobject** %7, align 4
  %169 = load %struct._jobject*, %struct._jobject** %8, align 4
  %170 = load %struct._jmethodID*, %struct._jmethodID** %9, align 4
  %171 = load %union.jvalue*, %union.jvalue** %10, align 4
  %172 = call zeroext i8 %166(%struct.JNINativeInterface_** noundef %167, %struct._jobject* noundef %168, %struct._jobject* noundef %169, %struct._jmethodID* noundef %170, %union.jvalue* noundef %171) #2
  call void @llvm.stackrestore(i8* %28)
  store i8 %172, i8* %18, align 1
  %173 = bitcast %struct.__va_list* %19 to i8*
  call void @llvm.va_end(i8* %173)
  %174 = load i8, i8* %18, align 1
  ret i8 %174
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i8 @JNI_CallNonvirtualBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca %union.jvalue*, align 4
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %6, i32 0, i32 0
  %16 = bitcast i8** %15 to [1 x i32]*
  store [1 x i32] %4, [1 x i32]* %16, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %7, align 4
  store %struct._jobject* %1, %struct._jobject** %8, align 4
  store %struct._jobject* %2, %struct._jobject** %9, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %10, align 4
  br label %17

17:                                               ; preds = %5
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %19 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %18, align 4
  %20 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %19, i32 0, i32 0
  %21 = load i8*, i8** %20, align 4
  %22 = bitcast i8* %21 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %24 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i32 0, i32 0
  %26 = call i32 %22(%struct.JNINativeInterface_** noundef %23, %struct._jmethodID* noundef %24, i8* noundef %25)
  store i32 %26, i32* %13, align 4
  %27 = load i32, i32* %13, align 4
  %28 = mul i32 %27, 8
  %29 = alloca i8, i32 %28, align 8
  %30 = bitcast i8* %29 to %union.jvalue*
  store %union.jvalue* %30, %union.jvalue** %11, align 4
  store i32 0, i32* %14, align 4
  br label %31

31:                                               ; preds = %147, %17
  %32 = load i32, i32* %14, align 4
  %33 = load i32, i32* %13, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %150

35:                                               ; preds = %31
  %36 = load i32, i32* %14, align 4
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i32 0, i32 %36
  %38 = load i8, i8* %37, align 1
  %39 = zext i8 %38 to i32
  switch i32 %39, label %146 [
    i32 90, label %40
    i32 66, label %51
    i32 83, label %62
    i32 67, label %73
    i32 73, label %85
    i32 74, label %95
    i32 68, label %106
    i32 70, label %120
    i32 76, label %135
  ]

40:                                               ; preds = %35
  %41 = bitcast %struct.__va_list* %6 to i8**
  %42 = load i8*, i8** %41, align 4
  %43 = getelementptr inbounds i8, i8* %42, i32 4
  store i8* %43, i8** %41, align 4
  %44 = bitcast i8* %42 to i32*
  %45 = load i32, i32* %44, align 4
  %46 = trunc i32 %45 to i8
  %47 = load %union.jvalue*, %union.jvalue** %11, align 4
  %48 = load i32, i32* %14, align 4
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %47, i32 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %146

51:                                               ; preds = %35
  %52 = bitcast %struct.__va_list* %6 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %11, align 4
  %59 = load i32, i32* %14, align 4
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i32 %59
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %57, i8* %61, align 8
  br label %146

62:                                               ; preds = %35
  %63 = bitcast %struct.__va_list* %6 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load %union.jvalue*, %union.jvalue** %11, align 4
  %70 = load i32, i32* %14, align 4
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %69, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %68, i16* %72, align 8
  br label %146

73:                                               ; preds = %35
  %74 = bitcast %struct.__va_list* %6 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = trunc i32 %78 to i16
  %80 = zext i16 %79 to i32
  %81 = load %union.jvalue*, %union.jvalue** %11, align 4
  %82 = load i32, i32* %14, align 4
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %81, i32 %82
  %84 = bitcast %union.jvalue* %83 to i32*
  store i32 %80, i32* %84, align 8
  br label %146

85:                                               ; preds = %35
  %86 = bitcast %struct.__va_list* %6 to i8**
  %87 = load i8*, i8** %86, align 4
  %88 = getelementptr inbounds i8, i8* %87, i32 4
  store i8* %88, i8** %86, align 4
  %89 = bitcast i8* %87 to i32*
  %90 = load i32, i32* %89, align 4
  %91 = load %union.jvalue*, %union.jvalue** %11, align 4
  %92 = load i32, i32* %14, align 4
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %91, i32 %92
  %94 = bitcast %union.jvalue* %93 to i32*
  store i32 %90, i32* %94, align 8
  br label %146

95:                                               ; preds = %35
  %96 = bitcast %struct.__va_list* %6 to i8**
  %97 = load i8*, i8** %96, align 4
  %98 = getelementptr inbounds i8, i8* %97, i32 4
  store i8* %98, i8** %96, align 4
  %99 = bitcast i8* %97 to i32*
  %100 = load i32, i32* %99, align 4
  %101 = sext i32 %100 to i64
  %102 = load %union.jvalue*, %union.jvalue** %11, align 4
  %103 = load i32, i32* %14, align 4
  %104 = getelementptr inbounds %union.jvalue, %union.jvalue* %102, i32 %103
  %105 = bitcast %union.jvalue* %104 to i64*
  store i64 %101, i64* %105, align 8
  br label %146

106:                                              ; preds = %35
  %107 = bitcast %struct.__va_list* %6 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load %union.jvalue*, %union.jvalue** %11, align 4
  %117 = load i32, i32* %14, align 4
  %118 = getelementptr inbounds %union.jvalue, %union.jvalue* %116, i32 %117
  %119 = bitcast %union.jvalue* %118 to double*
  store double %115, double* %119, align 8
  br label %146

120:                                              ; preds = %35
  %121 = bitcast %struct.__va_list* %6 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = ptrtoint i8* %122 to i32
  %124 = add i32 %123, 7
  %125 = and i32 %124, -8
  %126 = inttoptr i32 %125 to i8*
  %127 = getelementptr inbounds i8, i8* %126, i32 8
  store i8* %127, i8** %121, align 4
  %128 = bitcast i8* %126 to double*
  %129 = load double, double* %128, align 8
  %130 = fptrunc double %129 to float
  %131 = load %union.jvalue*, %union.jvalue** %11, align 4
  %132 = load i32, i32* %14, align 4
  %133 = getelementptr inbounds %union.jvalue, %union.jvalue* %131, i32 %132
  %134 = bitcast %union.jvalue* %133 to float*
  store float %130, float* %134, align 8
  br label %146

135:                                              ; preds = %35
  %136 = bitcast %struct.__va_list* %6 to i8**
  %137 = load i8*, i8** %136, align 4
  %138 = getelementptr inbounds i8, i8* %137, i32 4
  store i8* %138, i8** %136, align 4
  %139 = bitcast i8* %137 to i8**
  %140 = load i8*, i8** %139, align 4
  %141 = bitcast i8* %140 to %struct._jobject*
  %142 = load %union.jvalue*, %union.jvalue** %11, align 4
  %143 = load i32, i32* %14, align 4
  %144 = getelementptr inbounds %union.jvalue, %union.jvalue* %142, i32 %143
  %145 = bitcast %union.jvalue* %144 to %struct._jobject**
  store %struct._jobject* %141, %struct._jobject** %145, align 8
  br label %146

146:                                              ; preds = %35, %135, %120, %106, %95, %85, %73, %62, %51, %40
  br label %147

147:                                              ; preds = %146
  %148 = load i32, i32* %14, align 4
  %149 = add nsw i32 %148, 1
  store i32 %149, i32* %14, align 4
  br label %31, !llvm.loop !15

150:                                              ; preds = %31
  br label %151

151:                                              ; preds = %150
  %152 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %153 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %152, align 4
  %154 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %153, i32 0, i32 69
  %155 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %154, align 4
  %156 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %157 = load %struct._jobject*, %struct._jobject** %8, align 4
  %158 = load %struct._jobject*, %struct._jobject** %9, align 4
  %159 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %160 = load %union.jvalue*, %union.jvalue** %11, align 4
  %161 = call zeroext i8 %155(%struct.JNINativeInterface_** noundef %156, %struct._jobject* noundef %157, %struct._jobject* noundef %158, %struct._jmethodID* noundef %159, %union.jvalue* noundef %160)
  ret i8 %161
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i8 @JNI_CallStaticBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca i8, align 1
  %16 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %17 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_start(i8* %17)
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %19 = load %struct._jobject*, %struct._jobject** %13, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %21 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %16, i32 0, i32 0
  %22 = bitcast i8** %21 to [1 x i32]*
  %23 = load [1 x i32], [1 x i32]* %22, align 4
  %24 = call i8* @llvm.stacksave()
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  store [1 x i32] %23, [1 x i32]* %26, align 4
  store %struct.JNINativeInterface_** %18, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %19, %struct._jobject** %6, align 4
  store %struct._jmethodID* %20, %struct._jmethodID** %7, align 4
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %28 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %27, align 4
  %29 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %28, i32 0, i32 0
  %30 = load i8*, i8** %29, align 4
  %31 = bitcast i8* %30 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %32 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %33 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %35 = call i32 %31(%struct.JNINativeInterface_** noundef %32, %struct._jmethodID* noundef %33, i8* noundef %34) #2
  store i32 %35, i32* %10, align 4
  %36 = load i32, i32* %10, align 4
  %37 = mul i32 %36, 8
  %38 = alloca i8, i32 %37, align 8
  %39 = bitcast i8* %38 to %union.jvalue*
  store %union.jvalue* %39, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %40

40:                                               ; preds = %155, %3
  %41 = load i32, i32* %11, align 4
  %42 = load i32, i32* %10, align 4
  %43 = icmp slt i32 %41, %42
  br i1 %43, label %44, label %158

44:                                               ; preds = %40
  %45 = load i32, i32* %11, align 4
  %46 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  switch i32 %48, label %155 [
    i32 90, label %49
    i32 66, label %60
    i32 83, label %71
    i32 67, label %82
    i32 73, label %94
    i32 74, label %104
    i32 68, label %115
    i32 70, label %129
    i32 76, label %144
  ]

49:                                               ; preds = %44
  %50 = bitcast %struct.__va_list* %4 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %8, align 4
  %57 = load i32, i32* %11, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %155

60:                                               ; preds = %44
  %61 = bitcast %struct.__va_list* %4 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i8
  %67 = load %union.jvalue*, %union.jvalue** %8, align 4
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i8*
  store i8 %66, i8* %70, align 8
  br label %155

71:                                               ; preds = %44
  %72 = bitcast %struct.__va_list* %4 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = load %union.jvalue*, %union.jvalue** %8, align 4
  %79 = load i32, i32* %11, align 4
  %80 = getelementptr inbounds %union.jvalue, %union.jvalue* %78, i32 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %155

82:                                               ; preds = %44
  %83 = bitcast %struct.__va_list* %4 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = zext i16 %88 to i32
  %90 = load %union.jvalue*, %union.jvalue** %8, align 4
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds %union.jvalue, %union.jvalue* %90, i32 %91
  %93 = bitcast %union.jvalue* %92 to i32*
  store i32 %89, i32* %93, align 8
  br label %155

94:                                               ; preds = %44
  %95 = bitcast %struct.__va_list* %4 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = getelementptr inbounds i8, i8* %96, i32 4
  store i8* %97, i8** %95, align 4
  %98 = bitcast i8* %96 to i32*
  %99 = load i32, i32* %98, align 4
  %100 = load %union.jvalue*, %union.jvalue** %8, align 4
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i32*
  store i32 %99, i32* %103, align 8
  br label %155

104:                                              ; preds = %44
  %105 = bitcast %struct.__va_list* %4 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = getelementptr inbounds i8, i8* %106, i32 4
  store i8* %107, i8** %105, align 4
  %108 = bitcast i8* %106 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = sext i32 %109 to i64
  %111 = load %union.jvalue*, %union.jvalue** %8, align 4
  %112 = load i32, i32* %11, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %111, i32 %112
  %114 = bitcast %union.jvalue* %113 to i64*
  store i64 %110, i64* %114, align 8
  br label %155

115:                                              ; preds = %44
  %116 = bitcast %struct.__va_list* %4 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = ptrtoint i8* %117 to i32
  %119 = add i32 %118, 7
  %120 = and i32 %119, -8
  %121 = inttoptr i32 %120 to i8*
  %122 = getelementptr inbounds i8, i8* %121, i32 8
  store i8* %122, i8** %116, align 4
  %123 = bitcast i8* %121 to double*
  %124 = load double, double* %123, align 8
  %125 = load %union.jvalue*, %union.jvalue** %8, align 4
  %126 = load i32, i32* %11, align 4
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %125, i32 %126
  %128 = bitcast %union.jvalue* %127 to double*
  store double %124, double* %128, align 8
  br label %155

129:                                              ; preds = %44
  %130 = bitcast %struct.__va_list* %4 to i8**
  %131 = load i8*, i8** %130, align 4
  %132 = ptrtoint i8* %131 to i32
  %133 = add i32 %132, 7
  %134 = and i32 %133, -8
  %135 = inttoptr i32 %134 to i8*
  %136 = getelementptr inbounds i8, i8* %135, i32 8
  store i8* %136, i8** %130, align 4
  %137 = bitcast i8* %135 to double*
  %138 = load double, double* %137, align 8
  %139 = fptrunc double %138 to float
  %140 = load %union.jvalue*, %union.jvalue** %8, align 4
  %141 = load i32, i32* %11, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %139, float* %143, align 8
  br label %155

144:                                              ; preds = %44
  %145 = bitcast %struct.__va_list* %4 to i8**
  %146 = load i8*, i8** %145, align 4
  %147 = getelementptr inbounds i8, i8* %146, i32 4
  store i8* %147, i8** %145, align 4
  %148 = bitcast i8* %146 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = bitcast i8* %149 to %struct._jobject*
  %151 = load %union.jvalue*, %union.jvalue** %8, align 4
  %152 = load i32, i32* %11, align 4
  %153 = getelementptr inbounds %union.jvalue, %union.jvalue* %151, i32 %152
  %154 = bitcast %union.jvalue* %153 to %struct._jobject**
  store %struct._jobject* %150, %struct._jobject** %154, align 8
  br label %155

155:                                              ; preds = %144, %129, %115, %104, %94, %82, %71, %60, %49, %44
  %156 = load i32, i32* %11, align 4
  %157 = add nsw i32 %156, 1
  store i32 %157, i32* %11, align 4
  br label %40, !llvm.loop !16

158:                                              ; preds = %40
  %159 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %160 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %159, align 4
  %161 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %160, i32 0, i32 119
  %162 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %161, align 4
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %164 = load %struct._jobject*, %struct._jobject** %6, align 4
  %165 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %166 = load %union.jvalue*, %union.jvalue** %8, align 4
  %167 = call zeroext i8 %162(%struct.JNINativeInterface_** noundef %163, %struct._jobject* noundef %164, %struct._jmethodID* noundef %165, %union.jvalue* noundef %166) #2
  call void @llvm.stackrestore(i8* %24)
  store i8 %167, i8* %15, align 1
  %168 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_end(i8* %168)
  %169 = load i8, i8* %15, align 1
  ret i8 %169
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i8 @JNI_CallStaticBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !16

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 119
  %153 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  %158 = call zeroext i8 %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret i8 %158
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i8 @JNI_CallByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca i8, align 1
  %16 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %17 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_start(i8* %17)
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %19 = load %struct._jobject*, %struct._jobject** %13, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %21 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %16, i32 0, i32 0
  %22 = bitcast i8** %21 to [1 x i32]*
  %23 = load [1 x i32], [1 x i32]* %22, align 4
  %24 = call i8* @llvm.stacksave()
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  store [1 x i32] %23, [1 x i32]* %26, align 4
  store %struct.JNINativeInterface_** %18, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %19, %struct._jobject** %6, align 4
  store %struct._jmethodID* %20, %struct._jmethodID** %7, align 4
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %28 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %27, align 4
  %29 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %28, i32 0, i32 0
  %30 = load i8*, i8** %29, align 4
  %31 = bitcast i8* %30 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %32 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %33 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %35 = call i32 %31(%struct.JNINativeInterface_** noundef %32, %struct._jmethodID* noundef %33, i8* noundef %34) #2
  store i32 %35, i32* %10, align 4
  %36 = load i32, i32* %10, align 4
  %37 = mul i32 %36, 8
  %38 = alloca i8, i32 %37, align 8
  %39 = bitcast i8* %38 to %union.jvalue*
  store %union.jvalue* %39, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %40

40:                                               ; preds = %155, %3
  %41 = load i32, i32* %11, align 4
  %42 = load i32, i32* %10, align 4
  %43 = icmp slt i32 %41, %42
  br i1 %43, label %44, label %158

44:                                               ; preds = %40
  %45 = load i32, i32* %11, align 4
  %46 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  switch i32 %48, label %155 [
    i32 90, label %49
    i32 66, label %60
    i32 83, label %71
    i32 67, label %82
    i32 73, label %94
    i32 74, label %104
    i32 68, label %115
    i32 70, label %129
    i32 76, label %144
  ]

49:                                               ; preds = %44
  %50 = bitcast %struct.__va_list* %4 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %8, align 4
  %57 = load i32, i32* %11, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %155

60:                                               ; preds = %44
  %61 = bitcast %struct.__va_list* %4 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i8
  %67 = load %union.jvalue*, %union.jvalue** %8, align 4
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i8*
  store i8 %66, i8* %70, align 8
  br label %155

71:                                               ; preds = %44
  %72 = bitcast %struct.__va_list* %4 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = load %union.jvalue*, %union.jvalue** %8, align 4
  %79 = load i32, i32* %11, align 4
  %80 = getelementptr inbounds %union.jvalue, %union.jvalue* %78, i32 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %155

82:                                               ; preds = %44
  %83 = bitcast %struct.__va_list* %4 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = zext i16 %88 to i32
  %90 = load %union.jvalue*, %union.jvalue** %8, align 4
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds %union.jvalue, %union.jvalue* %90, i32 %91
  %93 = bitcast %union.jvalue* %92 to i32*
  store i32 %89, i32* %93, align 8
  br label %155

94:                                               ; preds = %44
  %95 = bitcast %struct.__va_list* %4 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = getelementptr inbounds i8, i8* %96, i32 4
  store i8* %97, i8** %95, align 4
  %98 = bitcast i8* %96 to i32*
  %99 = load i32, i32* %98, align 4
  %100 = load %union.jvalue*, %union.jvalue** %8, align 4
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i32*
  store i32 %99, i32* %103, align 8
  br label %155

104:                                              ; preds = %44
  %105 = bitcast %struct.__va_list* %4 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = getelementptr inbounds i8, i8* %106, i32 4
  store i8* %107, i8** %105, align 4
  %108 = bitcast i8* %106 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = sext i32 %109 to i64
  %111 = load %union.jvalue*, %union.jvalue** %8, align 4
  %112 = load i32, i32* %11, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %111, i32 %112
  %114 = bitcast %union.jvalue* %113 to i64*
  store i64 %110, i64* %114, align 8
  br label %155

115:                                              ; preds = %44
  %116 = bitcast %struct.__va_list* %4 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = ptrtoint i8* %117 to i32
  %119 = add i32 %118, 7
  %120 = and i32 %119, -8
  %121 = inttoptr i32 %120 to i8*
  %122 = getelementptr inbounds i8, i8* %121, i32 8
  store i8* %122, i8** %116, align 4
  %123 = bitcast i8* %121 to double*
  %124 = load double, double* %123, align 8
  %125 = load %union.jvalue*, %union.jvalue** %8, align 4
  %126 = load i32, i32* %11, align 4
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %125, i32 %126
  %128 = bitcast %union.jvalue* %127 to double*
  store double %124, double* %128, align 8
  br label %155

129:                                              ; preds = %44
  %130 = bitcast %struct.__va_list* %4 to i8**
  %131 = load i8*, i8** %130, align 4
  %132 = ptrtoint i8* %131 to i32
  %133 = add i32 %132, 7
  %134 = and i32 %133, -8
  %135 = inttoptr i32 %134 to i8*
  %136 = getelementptr inbounds i8, i8* %135, i32 8
  store i8* %136, i8** %130, align 4
  %137 = bitcast i8* %135 to double*
  %138 = load double, double* %137, align 8
  %139 = fptrunc double %138 to float
  %140 = load %union.jvalue*, %union.jvalue** %8, align 4
  %141 = load i32, i32* %11, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %139, float* %143, align 8
  br label %155

144:                                              ; preds = %44
  %145 = bitcast %struct.__va_list* %4 to i8**
  %146 = load i8*, i8** %145, align 4
  %147 = getelementptr inbounds i8, i8* %146, i32 4
  store i8* %147, i8** %145, align 4
  %148 = bitcast i8* %146 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = bitcast i8* %149 to %struct._jobject*
  %151 = load %union.jvalue*, %union.jvalue** %8, align 4
  %152 = load i32, i32* %11, align 4
  %153 = getelementptr inbounds %union.jvalue, %union.jvalue* %151, i32 %152
  %154 = bitcast %union.jvalue* %153 to %struct._jobject**
  store %struct._jobject* %150, %struct._jobject** %154, align 8
  br label %155

155:                                              ; preds = %144, %129, %115, %104, %94, %82, %71, %60, %49, %44
  %156 = load i32, i32* %11, align 4
  %157 = add nsw i32 %156, 1
  store i32 %157, i32* %11, align 4
  br label %40, !llvm.loop !17

158:                                              ; preds = %40
  %159 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %160 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %159, align 4
  %161 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %160, i32 0, i32 42
  %162 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %161, align 4
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %164 = load %struct._jobject*, %struct._jobject** %6, align 4
  %165 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %166 = load %union.jvalue*, %union.jvalue** %8, align 4
  %167 = call signext i8 %162(%struct.JNINativeInterface_** noundef %163, %struct._jobject* noundef %164, %struct._jmethodID* noundef %165, %union.jvalue* noundef %166) #2
  call void @llvm.stackrestore(i8* %24)
  store i8 %167, i8* %15, align 1
  %168 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_end(i8* %168)
  %169 = load i8, i8* %15, align 1
  ret i8 %169
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i8 @JNI_CallByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !17

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 42
  %153 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  %158 = call signext i8 %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret i8 %158
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i8 @JNI_CallNonvirtualByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jmethodID*, align 4
  %10 = alloca %union.jvalue*, align 4
  %11 = alloca [257 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca %struct.JNINativeInterface_**, align 4
  %15 = alloca %struct._jobject*, align 4
  %16 = alloca %struct._jobject*, align 4
  %17 = alloca %struct._jmethodID*, align 4
  %18 = alloca i8, align 1
  %19 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %14, align 4
  store %struct._jobject* %1, %struct._jobject** %15, align 4
  store %struct._jobject* %2, %struct._jobject** %16, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %17, align 4
  %20 = bitcast %struct.__va_list* %19 to i8*
  call void @llvm.va_start(i8* %20)
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %14, align 4
  %22 = load %struct._jobject*, %struct._jobject** %15, align 4
  %23 = load %struct._jobject*, %struct._jobject** %16, align 4
  %24 = load %struct._jmethodID*, %struct._jmethodID** %17, align 4
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %19, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  %27 = load [1 x i32], [1 x i32]* %26, align 4
  %28 = call i8* @llvm.stacksave()
  %29 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %30 = bitcast i8** %29 to [1 x i32]*
  store [1 x i32] %27, [1 x i32]* %30, align 4
  store %struct.JNINativeInterface_** %21, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %22, %struct._jobject** %7, align 4
  store %struct._jobject* %23, %struct._jobject** %8, align 4
  store %struct._jmethodID* %24, %struct._jmethodID** %9, align 4
  %31 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %32 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %31, align 4
  %33 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %32, i32 0, i32 0
  %34 = load i8*, i8** %33, align 4
  %35 = bitcast i8* %34 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %36 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %37 = load %struct._jmethodID*, %struct._jmethodID** %9, align 4
  %38 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i32 0, i32 0
  %39 = call i32 %35(%struct.JNINativeInterface_** noundef %36, %struct._jmethodID* noundef %37, i8* noundef %38) #2
  store i32 %39, i32* %12, align 4
  %40 = load i32, i32* %12, align 4
  %41 = mul i32 %40, 8
  %42 = alloca i8, i32 %41, align 8
  %43 = bitcast i8* %42 to %union.jvalue*
  store %union.jvalue* %43, %union.jvalue** %10, align 4
  store i32 0, i32* %13, align 4
  br label %44

44:                                               ; preds = %159, %4
  %45 = load i32, i32* %13, align 4
  %46 = load i32, i32* %12, align 4
  %47 = icmp slt i32 %45, %46
  br i1 %47, label %48, label %162

48:                                               ; preds = %44
  %49 = load i32, i32* %13, align 4
  %50 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i32 0, i32 %49
  %51 = load i8, i8* %50, align 1
  %52 = zext i8 %51 to i32
  switch i32 %52, label %159 [
    i32 90, label %53
    i32 66, label %64
    i32 83, label %75
    i32 67, label %86
    i32 73, label %98
    i32 74, label %108
    i32 68, label %119
    i32 70, label %133
    i32 76, label %148
  ]

53:                                               ; preds = %48
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %10, align 4
  %61 = load i32, i32* %13, align 4
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i32 %61
  %63 = bitcast %union.jvalue* %62 to i8*
  store i8 %59, i8* %63, align 8
  br label %159

64:                                               ; preds = %48
  %65 = bitcast %struct.__va_list* %5 to i8**
  %66 = load i8*, i8** %65, align 4
  %67 = getelementptr inbounds i8, i8* %66, i32 4
  store i8* %67, i8** %65, align 4
  %68 = bitcast i8* %66 to i32*
  %69 = load i32, i32* %68, align 4
  %70 = trunc i32 %69 to i8
  %71 = load %union.jvalue*, %union.jvalue** %10, align 4
  %72 = load i32, i32* %13, align 4
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %71, i32 %72
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %70, i8* %74, align 8
  br label %159

75:                                               ; preds = %48
  %76 = bitcast %struct.__va_list* %5 to i8**
  %77 = load i8*, i8** %76, align 4
  %78 = getelementptr inbounds i8, i8* %77, i32 4
  store i8* %78, i8** %76, align 4
  %79 = bitcast i8* %77 to i32*
  %80 = load i32, i32* %79, align 4
  %81 = trunc i32 %80 to i16
  %82 = load %union.jvalue*, %union.jvalue** %10, align 4
  %83 = load i32, i32* %13, align 4
  %84 = getelementptr inbounds %union.jvalue, %union.jvalue* %82, i32 %83
  %85 = bitcast %union.jvalue* %84 to i16*
  store i16 %81, i16* %85, align 8
  br label %159

86:                                               ; preds = %48
  %87 = bitcast %struct.__va_list* %5 to i8**
  %88 = load i8*, i8** %87, align 4
  %89 = getelementptr inbounds i8, i8* %88, i32 4
  store i8* %89, i8** %87, align 4
  %90 = bitcast i8* %88 to i32*
  %91 = load i32, i32* %90, align 4
  %92 = trunc i32 %91 to i16
  %93 = zext i16 %92 to i32
  %94 = load %union.jvalue*, %union.jvalue** %10, align 4
  %95 = load i32, i32* %13, align 4
  %96 = getelementptr inbounds %union.jvalue, %union.jvalue* %94, i32 %95
  %97 = bitcast %union.jvalue* %96 to i32*
  store i32 %93, i32* %97, align 8
  br label %159

98:                                               ; preds = %48
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load %union.jvalue*, %union.jvalue** %10, align 4
  %105 = load i32, i32* %13, align 4
  %106 = getelementptr inbounds %union.jvalue, %union.jvalue* %104, i32 %105
  %107 = bitcast %union.jvalue* %106 to i32*
  store i32 %103, i32* %107, align 8
  br label %159

108:                                              ; preds = %48
  %109 = bitcast %struct.__va_list* %5 to i8**
  %110 = load i8*, i8** %109, align 4
  %111 = getelementptr inbounds i8, i8* %110, i32 4
  store i8* %111, i8** %109, align 4
  %112 = bitcast i8* %110 to i32*
  %113 = load i32, i32* %112, align 4
  %114 = sext i32 %113 to i64
  %115 = load %union.jvalue*, %union.jvalue** %10, align 4
  %116 = load i32, i32* %13, align 4
  %117 = getelementptr inbounds %union.jvalue, %union.jvalue* %115, i32 %116
  %118 = bitcast %union.jvalue* %117 to i64*
  store i64 %114, i64* %118, align 8
  br label %159

119:                                              ; preds = %48
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = ptrtoint i8* %121 to i32
  %123 = add i32 %122, 7
  %124 = and i32 %123, -8
  %125 = inttoptr i32 %124 to i8*
  %126 = getelementptr inbounds i8, i8* %125, i32 8
  store i8* %126, i8** %120, align 4
  %127 = bitcast i8* %125 to double*
  %128 = load double, double* %127, align 8
  %129 = load %union.jvalue*, %union.jvalue** %10, align 4
  %130 = load i32, i32* %13, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to double*
  store double %128, double* %132, align 8
  br label %159

133:                                              ; preds = %48
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = ptrtoint i8* %135 to i32
  %137 = add i32 %136, 7
  %138 = and i32 %137, -8
  %139 = inttoptr i32 %138 to i8*
  %140 = getelementptr inbounds i8, i8* %139, i32 8
  store i8* %140, i8** %134, align 4
  %141 = bitcast i8* %139 to double*
  %142 = load double, double* %141, align 8
  %143 = fptrunc double %142 to float
  %144 = load %union.jvalue*, %union.jvalue** %10, align 4
  %145 = load i32, i32* %13, align 4
  %146 = getelementptr inbounds %union.jvalue, %union.jvalue* %144, i32 %145
  %147 = bitcast %union.jvalue* %146 to float*
  store float %143, float* %147, align 8
  br label %159

148:                                              ; preds = %48
  %149 = bitcast %struct.__va_list* %5 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = getelementptr inbounds i8, i8* %150, i32 4
  store i8* %151, i8** %149, align 4
  %152 = bitcast i8* %150 to i8**
  %153 = load i8*, i8** %152, align 4
  %154 = bitcast i8* %153 to %struct._jobject*
  %155 = load %union.jvalue*, %union.jvalue** %10, align 4
  %156 = load i32, i32* %13, align 4
  %157 = getelementptr inbounds %union.jvalue, %union.jvalue* %155, i32 %156
  %158 = bitcast %union.jvalue* %157 to %struct._jobject**
  store %struct._jobject* %154, %struct._jobject** %158, align 8
  br label %159

159:                                              ; preds = %148, %133, %119, %108, %98, %86, %75, %64, %53, %48
  %160 = load i32, i32* %13, align 4
  %161 = add nsw i32 %160, 1
  store i32 %161, i32* %13, align 4
  br label %44, !llvm.loop !18

162:                                              ; preds = %44
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %164 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %163, align 4
  %165 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %164, i32 0, i32 72
  %166 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %165, align 4
  %167 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %168 = load %struct._jobject*, %struct._jobject** %7, align 4
  %169 = load %struct._jobject*, %struct._jobject** %8, align 4
  %170 = load %struct._jmethodID*, %struct._jmethodID** %9, align 4
  %171 = load %union.jvalue*, %union.jvalue** %10, align 4
  %172 = call signext i8 %166(%struct.JNINativeInterface_** noundef %167, %struct._jobject* noundef %168, %struct._jobject* noundef %169, %struct._jmethodID* noundef %170, %union.jvalue* noundef %171) #2
  call void @llvm.stackrestore(i8* %28)
  store i8 %172, i8* %18, align 1
  %173 = bitcast %struct.__va_list* %19 to i8*
  call void @llvm.va_end(i8* %173)
  %174 = load i8, i8* %18, align 1
  ret i8 %174
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i8 @JNI_CallNonvirtualByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca %union.jvalue*, align 4
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %6, i32 0, i32 0
  %16 = bitcast i8** %15 to [1 x i32]*
  store [1 x i32] %4, [1 x i32]* %16, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %7, align 4
  store %struct._jobject* %1, %struct._jobject** %8, align 4
  store %struct._jobject* %2, %struct._jobject** %9, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %10, align 4
  br label %17

17:                                               ; preds = %5
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %19 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %18, align 4
  %20 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %19, i32 0, i32 0
  %21 = load i8*, i8** %20, align 4
  %22 = bitcast i8* %21 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %24 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i32 0, i32 0
  %26 = call i32 %22(%struct.JNINativeInterface_** noundef %23, %struct._jmethodID* noundef %24, i8* noundef %25)
  store i32 %26, i32* %13, align 4
  %27 = load i32, i32* %13, align 4
  %28 = mul i32 %27, 8
  %29 = alloca i8, i32 %28, align 8
  %30 = bitcast i8* %29 to %union.jvalue*
  store %union.jvalue* %30, %union.jvalue** %11, align 4
  store i32 0, i32* %14, align 4
  br label %31

31:                                               ; preds = %147, %17
  %32 = load i32, i32* %14, align 4
  %33 = load i32, i32* %13, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %150

35:                                               ; preds = %31
  %36 = load i32, i32* %14, align 4
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i32 0, i32 %36
  %38 = load i8, i8* %37, align 1
  %39 = zext i8 %38 to i32
  switch i32 %39, label %146 [
    i32 90, label %40
    i32 66, label %51
    i32 83, label %62
    i32 67, label %73
    i32 73, label %85
    i32 74, label %95
    i32 68, label %106
    i32 70, label %120
    i32 76, label %135
  ]

40:                                               ; preds = %35
  %41 = bitcast %struct.__va_list* %6 to i8**
  %42 = load i8*, i8** %41, align 4
  %43 = getelementptr inbounds i8, i8* %42, i32 4
  store i8* %43, i8** %41, align 4
  %44 = bitcast i8* %42 to i32*
  %45 = load i32, i32* %44, align 4
  %46 = trunc i32 %45 to i8
  %47 = load %union.jvalue*, %union.jvalue** %11, align 4
  %48 = load i32, i32* %14, align 4
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %47, i32 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %146

51:                                               ; preds = %35
  %52 = bitcast %struct.__va_list* %6 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %11, align 4
  %59 = load i32, i32* %14, align 4
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i32 %59
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %57, i8* %61, align 8
  br label %146

62:                                               ; preds = %35
  %63 = bitcast %struct.__va_list* %6 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load %union.jvalue*, %union.jvalue** %11, align 4
  %70 = load i32, i32* %14, align 4
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %69, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %68, i16* %72, align 8
  br label %146

73:                                               ; preds = %35
  %74 = bitcast %struct.__va_list* %6 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = trunc i32 %78 to i16
  %80 = zext i16 %79 to i32
  %81 = load %union.jvalue*, %union.jvalue** %11, align 4
  %82 = load i32, i32* %14, align 4
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %81, i32 %82
  %84 = bitcast %union.jvalue* %83 to i32*
  store i32 %80, i32* %84, align 8
  br label %146

85:                                               ; preds = %35
  %86 = bitcast %struct.__va_list* %6 to i8**
  %87 = load i8*, i8** %86, align 4
  %88 = getelementptr inbounds i8, i8* %87, i32 4
  store i8* %88, i8** %86, align 4
  %89 = bitcast i8* %87 to i32*
  %90 = load i32, i32* %89, align 4
  %91 = load %union.jvalue*, %union.jvalue** %11, align 4
  %92 = load i32, i32* %14, align 4
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %91, i32 %92
  %94 = bitcast %union.jvalue* %93 to i32*
  store i32 %90, i32* %94, align 8
  br label %146

95:                                               ; preds = %35
  %96 = bitcast %struct.__va_list* %6 to i8**
  %97 = load i8*, i8** %96, align 4
  %98 = getelementptr inbounds i8, i8* %97, i32 4
  store i8* %98, i8** %96, align 4
  %99 = bitcast i8* %97 to i32*
  %100 = load i32, i32* %99, align 4
  %101 = sext i32 %100 to i64
  %102 = load %union.jvalue*, %union.jvalue** %11, align 4
  %103 = load i32, i32* %14, align 4
  %104 = getelementptr inbounds %union.jvalue, %union.jvalue* %102, i32 %103
  %105 = bitcast %union.jvalue* %104 to i64*
  store i64 %101, i64* %105, align 8
  br label %146

106:                                              ; preds = %35
  %107 = bitcast %struct.__va_list* %6 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load %union.jvalue*, %union.jvalue** %11, align 4
  %117 = load i32, i32* %14, align 4
  %118 = getelementptr inbounds %union.jvalue, %union.jvalue* %116, i32 %117
  %119 = bitcast %union.jvalue* %118 to double*
  store double %115, double* %119, align 8
  br label %146

120:                                              ; preds = %35
  %121 = bitcast %struct.__va_list* %6 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = ptrtoint i8* %122 to i32
  %124 = add i32 %123, 7
  %125 = and i32 %124, -8
  %126 = inttoptr i32 %125 to i8*
  %127 = getelementptr inbounds i8, i8* %126, i32 8
  store i8* %127, i8** %121, align 4
  %128 = bitcast i8* %126 to double*
  %129 = load double, double* %128, align 8
  %130 = fptrunc double %129 to float
  %131 = load %union.jvalue*, %union.jvalue** %11, align 4
  %132 = load i32, i32* %14, align 4
  %133 = getelementptr inbounds %union.jvalue, %union.jvalue* %131, i32 %132
  %134 = bitcast %union.jvalue* %133 to float*
  store float %130, float* %134, align 8
  br label %146

135:                                              ; preds = %35
  %136 = bitcast %struct.__va_list* %6 to i8**
  %137 = load i8*, i8** %136, align 4
  %138 = getelementptr inbounds i8, i8* %137, i32 4
  store i8* %138, i8** %136, align 4
  %139 = bitcast i8* %137 to i8**
  %140 = load i8*, i8** %139, align 4
  %141 = bitcast i8* %140 to %struct._jobject*
  %142 = load %union.jvalue*, %union.jvalue** %11, align 4
  %143 = load i32, i32* %14, align 4
  %144 = getelementptr inbounds %union.jvalue, %union.jvalue* %142, i32 %143
  %145 = bitcast %union.jvalue* %144 to %struct._jobject**
  store %struct._jobject* %141, %struct._jobject** %145, align 8
  br label %146

146:                                              ; preds = %35, %135, %120, %106, %95, %85, %73, %62, %51, %40
  br label %147

147:                                              ; preds = %146
  %148 = load i32, i32* %14, align 4
  %149 = add nsw i32 %148, 1
  store i32 %149, i32* %14, align 4
  br label %31, !llvm.loop !18

150:                                              ; preds = %31
  br label %151

151:                                              ; preds = %150
  %152 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %153 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %152, align 4
  %154 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %153, i32 0, i32 72
  %155 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %154, align 4
  %156 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %157 = load %struct._jobject*, %struct._jobject** %8, align 4
  %158 = load %struct._jobject*, %struct._jobject** %9, align 4
  %159 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %160 = load %union.jvalue*, %union.jvalue** %11, align 4
  %161 = call signext i8 %155(%struct.JNINativeInterface_** noundef %156, %struct._jobject* noundef %157, %struct._jobject* noundef %158, %struct._jmethodID* noundef %159, %union.jvalue* noundef %160)
  ret i8 %161
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i8 @JNI_CallStaticByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca i8, align 1
  %16 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %17 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_start(i8* %17)
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %19 = load %struct._jobject*, %struct._jobject** %13, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %21 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %16, i32 0, i32 0
  %22 = bitcast i8** %21 to [1 x i32]*
  %23 = load [1 x i32], [1 x i32]* %22, align 4
  %24 = call i8* @llvm.stacksave()
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  store [1 x i32] %23, [1 x i32]* %26, align 4
  store %struct.JNINativeInterface_** %18, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %19, %struct._jobject** %6, align 4
  store %struct._jmethodID* %20, %struct._jmethodID** %7, align 4
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %28 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %27, align 4
  %29 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %28, i32 0, i32 0
  %30 = load i8*, i8** %29, align 4
  %31 = bitcast i8* %30 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %32 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %33 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %35 = call i32 %31(%struct.JNINativeInterface_** noundef %32, %struct._jmethodID* noundef %33, i8* noundef %34) #2
  store i32 %35, i32* %10, align 4
  %36 = load i32, i32* %10, align 4
  %37 = mul i32 %36, 8
  %38 = alloca i8, i32 %37, align 8
  %39 = bitcast i8* %38 to %union.jvalue*
  store %union.jvalue* %39, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %40

40:                                               ; preds = %155, %3
  %41 = load i32, i32* %11, align 4
  %42 = load i32, i32* %10, align 4
  %43 = icmp slt i32 %41, %42
  br i1 %43, label %44, label %158

44:                                               ; preds = %40
  %45 = load i32, i32* %11, align 4
  %46 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  switch i32 %48, label %155 [
    i32 90, label %49
    i32 66, label %60
    i32 83, label %71
    i32 67, label %82
    i32 73, label %94
    i32 74, label %104
    i32 68, label %115
    i32 70, label %129
    i32 76, label %144
  ]

49:                                               ; preds = %44
  %50 = bitcast %struct.__va_list* %4 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %8, align 4
  %57 = load i32, i32* %11, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %155

60:                                               ; preds = %44
  %61 = bitcast %struct.__va_list* %4 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i8
  %67 = load %union.jvalue*, %union.jvalue** %8, align 4
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i8*
  store i8 %66, i8* %70, align 8
  br label %155

71:                                               ; preds = %44
  %72 = bitcast %struct.__va_list* %4 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = load %union.jvalue*, %union.jvalue** %8, align 4
  %79 = load i32, i32* %11, align 4
  %80 = getelementptr inbounds %union.jvalue, %union.jvalue* %78, i32 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %155

82:                                               ; preds = %44
  %83 = bitcast %struct.__va_list* %4 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = zext i16 %88 to i32
  %90 = load %union.jvalue*, %union.jvalue** %8, align 4
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds %union.jvalue, %union.jvalue* %90, i32 %91
  %93 = bitcast %union.jvalue* %92 to i32*
  store i32 %89, i32* %93, align 8
  br label %155

94:                                               ; preds = %44
  %95 = bitcast %struct.__va_list* %4 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = getelementptr inbounds i8, i8* %96, i32 4
  store i8* %97, i8** %95, align 4
  %98 = bitcast i8* %96 to i32*
  %99 = load i32, i32* %98, align 4
  %100 = load %union.jvalue*, %union.jvalue** %8, align 4
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i32*
  store i32 %99, i32* %103, align 8
  br label %155

104:                                              ; preds = %44
  %105 = bitcast %struct.__va_list* %4 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = getelementptr inbounds i8, i8* %106, i32 4
  store i8* %107, i8** %105, align 4
  %108 = bitcast i8* %106 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = sext i32 %109 to i64
  %111 = load %union.jvalue*, %union.jvalue** %8, align 4
  %112 = load i32, i32* %11, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %111, i32 %112
  %114 = bitcast %union.jvalue* %113 to i64*
  store i64 %110, i64* %114, align 8
  br label %155

115:                                              ; preds = %44
  %116 = bitcast %struct.__va_list* %4 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = ptrtoint i8* %117 to i32
  %119 = add i32 %118, 7
  %120 = and i32 %119, -8
  %121 = inttoptr i32 %120 to i8*
  %122 = getelementptr inbounds i8, i8* %121, i32 8
  store i8* %122, i8** %116, align 4
  %123 = bitcast i8* %121 to double*
  %124 = load double, double* %123, align 8
  %125 = load %union.jvalue*, %union.jvalue** %8, align 4
  %126 = load i32, i32* %11, align 4
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %125, i32 %126
  %128 = bitcast %union.jvalue* %127 to double*
  store double %124, double* %128, align 8
  br label %155

129:                                              ; preds = %44
  %130 = bitcast %struct.__va_list* %4 to i8**
  %131 = load i8*, i8** %130, align 4
  %132 = ptrtoint i8* %131 to i32
  %133 = add i32 %132, 7
  %134 = and i32 %133, -8
  %135 = inttoptr i32 %134 to i8*
  %136 = getelementptr inbounds i8, i8* %135, i32 8
  store i8* %136, i8** %130, align 4
  %137 = bitcast i8* %135 to double*
  %138 = load double, double* %137, align 8
  %139 = fptrunc double %138 to float
  %140 = load %union.jvalue*, %union.jvalue** %8, align 4
  %141 = load i32, i32* %11, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %139, float* %143, align 8
  br label %155

144:                                              ; preds = %44
  %145 = bitcast %struct.__va_list* %4 to i8**
  %146 = load i8*, i8** %145, align 4
  %147 = getelementptr inbounds i8, i8* %146, i32 4
  store i8* %147, i8** %145, align 4
  %148 = bitcast i8* %146 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = bitcast i8* %149 to %struct._jobject*
  %151 = load %union.jvalue*, %union.jvalue** %8, align 4
  %152 = load i32, i32* %11, align 4
  %153 = getelementptr inbounds %union.jvalue, %union.jvalue* %151, i32 %152
  %154 = bitcast %union.jvalue* %153 to %struct._jobject**
  store %struct._jobject* %150, %struct._jobject** %154, align 8
  br label %155

155:                                              ; preds = %144, %129, %115, %104, %94, %82, %71, %60, %49, %44
  %156 = load i32, i32* %11, align 4
  %157 = add nsw i32 %156, 1
  store i32 %157, i32* %11, align 4
  br label %40, !llvm.loop !19

158:                                              ; preds = %40
  %159 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %160 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %159, align 4
  %161 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %160, i32 0, i32 122
  %162 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %161, align 4
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %164 = load %struct._jobject*, %struct._jobject** %6, align 4
  %165 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %166 = load %union.jvalue*, %union.jvalue** %8, align 4
  %167 = call signext i8 %162(%struct.JNINativeInterface_** noundef %163, %struct._jobject* noundef %164, %struct._jmethodID* noundef %165, %union.jvalue* noundef %166) #2
  call void @llvm.stackrestore(i8* %24)
  store i8 %167, i8* %15, align 1
  %168 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_end(i8* %168)
  %169 = load i8, i8* %15, align 1
  ret i8 %169
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i8 @JNI_CallStaticByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !19

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 122
  %153 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  %158 = call signext i8 %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret i8 %158
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i16 @JNI_CallCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca i16, align 2
  %16 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %17 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_start(i8* %17)
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %19 = load %struct._jobject*, %struct._jobject** %13, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %21 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %16, i32 0, i32 0
  %22 = bitcast i8** %21 to [1 x i32]*
  %23 = load [1 x i32], [1 x i32]* %22, align 4
  %24 = call i8* @llvm.stacksave()
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  store [1 x i32] %23, [1 x i32]* %26, align 4
  store %struct.JNINativeInterface_** %18, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %19, %struct._jobject** %6, align 4
  store %struct._jmethodID* %20, %struct._jmethodID** %7, align 4
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %28 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %27, align 4
  %29 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %28, i32 0, i32 0
  %30 = load i8*, i8** %29, align 4
  %31 = bitcast i8* %30 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %32 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %33 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %35 = call i32 %31(%struct.JNINativeInterface_** noundef %32, %struct._jmethodID* noundef %33, i8* noundef %34) #2
  store i32 %35, i32* %10, align 4
  %36 = load i32, i32* %10, align 4
  %37 = mul i32 %36, 8
  %38 = alloca i8, i32 %37, align 8
  %39 = bitcast i8* %38 to %union.jvalue*
  store %union.jvalue* %39, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %40

40:                                               ; preds = %155, %3
  %41 = load i32, i32* %11, align 4
  %42 = load i32, i32* %10, align 4
  %43 = icmp slt i32 %41, %42
  br i1 %43, label %44, label %158

44:                                               ; preds = %40
  %45 = load i32, i32* %11, align 4
  %46 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  switch i32 %48, label %155 [
    i32 90, label %49
    i32 66, label %60
    i32 83, label %71
    i32 67, label %82
    i32 73, label %94
    i32 74, label %104
    i32 68, label %115
    i32 70, label %129
    i32 76, label %144
  ]

49:                                               ; preds = %44
  %50 = bitcast %struct.__va_list* %4 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %8, align 4
  %57 = load i32, i32* %11, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %155

60:                                               ; preds = %44
  %61 = bitcast %struct.__va_list* %4 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i8
  %67 = load %union.jvalue*, %union.jvalue** %8, align 4
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i8*
  store i8 %66, i8* %70, align 8
  br label %155

71:                                               ; preds = %44
  %72 = bitcast %struct.__va_list* %4 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = load %union.jvalue*, %union.jvalue** %8, align 4
  %79 = load i32, i32* %11, align 4
  %80 = getelementptr inbounds %union.jvalue, %union.jvalue* %78, i32 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %155

82:                                               ; preds = %44
  %83 = bitcast %struct.__va_list* %4 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = zext i16 %88 to i32
  %90 = load %union.jvalue*, %union.jvalue** %8, align 4
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds %union.jvalue, %union.jvalue* %90, i32 %91
  %93 = bitcast %union.jvalue* %92 to i32*
  store i32 %89, i32* %93, align 8
  br label %155

94:                                               ; preds = %44
  %95 = bitcast %struct.__va_list* %4 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = getelementptr inbounds i8, i8* %96, i32 4
  store i8* %97, i8** %95, align 4
  %98 = bitcast i8* %96 to i32*
  %99 = load i32, i32* %98, align 4
  %100 = load %union.jvalue*, %union.jvalue** %8, align 4
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i32*
  store i32 %99, i32* %103, align 8
  br label %155

104:                                              ; preds = %44
  %105 = bitcast %struct.__va_list* %4 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = getelementptr inbounds i8, i8* %106, i32 4
  store i8* %107, i8** %105, align 4
  %108 = bitcast i8* %106 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = sext i32 %109 to i64
  %111 = load %union.jvalue*, %union.jvalue** %8, align 4
  %112 = load i32, i32* %11, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %111, i32 %112
  %114 = bitcast %union.jvalue* %113 to i64*
  store i64 %110, i64* %114, align 8
  br label %155

115:                                              ; preds = %44
  %116 = bitcast %struct.__va_list* %4 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = ptrtoint i8* %117 to i32
  %119 = add i32 %118, 7
  %120 = and i32 %119, -8
  %121 = inttoptr i32 %120 to i8*
  %122 = getelementptr inbounds i8, i8* %121, i32 8
  store i8* %122, i8** %116, align 4
  %123 = bitcast i8* %121 to double*
  %124 = load double, double* %123, align 8
  %125 = load %union.jvalue*, %union.jvalue** %8, align 4
  %126 = load i32, i32* %11, align 4
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %125, i32 %126
  %128 = bitcast %union.jvalue* %127 to double*
  store double %124, double* %128, align 8
  br label %155

129:                                              ; preds = %44
  %130 = bitcast %struct.__va_list* %4 to i8**
  %131 = load i8*, i8** %130, align 4
  %132 = ptrtoint i8* %131 to i32
  %133 = add i32 %132, 7
  %134 = and i32 %133, -8
  %135 = inttoptr i32 %134 to i8*
  %136 = getelementptr inbounds i8, i8* %135, i32 8
  store i8* %136, i8** %130, align 4
  %137 = bitcast i8* %135 to double*
  %138 = load double, double* %137, align 8
  %139 = fptrunc double %138 to float
  %140 = load %union.jvalue*, %union.jvalue** %8, align 4
  %141 = load i32, i32* %11, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %139, float* %143, align 8
  br label %155

144:                                              ; preds = %44
  %145 = bitcast %struct.__va_list* %4 to i8**
  %146 = load i8*, i8** %145, align 4
  %147 = getelementptr inbounds i8, i8* %146, i32 4
  store i8* %147, i8** %145, align 4
  %148 = bitcast i8* %146 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = bitcast i8* %149 to %struct._jobject*
  %151 = load %union.jvalue*, %union.jvalue** %8, align 4
  %152 = load i32, i32* %11, align 4
  %153 = getelementptr inbounds %union.jvalue, %union.jvalue* %151, i32 %152
  %154 = bitcast %union.jvalue* %153 to %struct._jobject**
  store %struct._jobject* %150, %struct._jobject** %154, align 8
  br label %155

155:                                              ; preds = %144, %129, %115, %104, %94, %82, %71, %60, %49, %44
  %156 = load i32, i32* %11, align 4
  %157 = add nsw i32 %156, 1
  store i32 %157, i32* %11, align 4
  br label %40, !llvm.loop !20

158:                                              ; preds = %40
  %159 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %160 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %159, align 4
  %161 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %160, i32 0, i32 45
  %162 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %161, align 4
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %164 = load %struct._jobject*, %struct._jobject** %6, align 4
  %165 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %166 = load %union.jvalue*, %union.jvalue** %8, align 4
  %167 = call zeroext i16 %162(%struct.JNINativeInterface_** noundef %163, %struct._jobject* noundef %164, %struct._jmethodID* noundef %165, %union.jvalue* noundef %166) #2
  call void @llvm.stackrestore(i8* %24)
  store i16 %167, i16* %15, align 2
  %168 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_end(i8* %168)
  %169 = load i16, i16* %15, align 2
  ret i16 %169
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i16 @JNI_CallCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !20

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 45
  %153 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  %158 = call zeroext i16 %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret i16 %158
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i16 @JNI_CallNonvirtualCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jmethodID*, align 4
  %10 = alloca %union.jvalue*, align 4
  %11 = alloca [257 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca %struct.JNINativeInterface_**, align 4
  %15 = alloca %struct._jobject*, align 4
  %16 = alloca %struct._jobject*, align 4
  %17 = alloca %struct._jmethodID*, align 4
  %18 = alloca i16, align 2
  %19 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %14, align 4
  store %struct._jobject* %1, %struct._jobject** %15, align 4
  store %struct._jobject* %2, %struct._jobject** %16, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %17, align 4
  %20 = bitcast %struct.__va_list* %19 to i8*
  call void @llvm.va_start(i8* %20)
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %14, align 4
  %22 = load %struct._jobject*, %struct._jobject** %15, align 4
  %23 = load %struct._jobject*, %struct._jobject** %16, align 4
  %24 = load %struct._jmethodID*, %struct._jmethodID** %17, align 4
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %19, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  %27 = load [1 x i32], [1 x i32]* %26, align 4
  %28 = call i8* @llvm.stacksave()
  %29 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %30 = bitcast i8** %29 to [1 x i32]*
  store [1 x i32] %27, [1 x i32]* %30, align 4
  store %struct.JNINativeInterface_** %21, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %22, %struct._jobject** %7, align 4
  store %struct._jobject* %23, %struct._jobject** %8, align 4
  store %struct._jmethodID* %24, %struct._jmethodID** %9, align 4
  %31 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %32 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %31, align 4
  %33 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %32, i32 0, i32 0
  %34 = load i8*, i8** %33, align 4
  %35 = bitcast i8* %34 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %36 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %37 = load %struct._jmethodID*, %struct._jmethodID** %9, align 4
  %38 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i32 0, i32 0
  %39 = call i32 %35(%struct.JNINativeInterface_** noundef %36, %struct._jmethodID* noundef %37, i8* noundef %38) #2
  store i32 %39, i32* %12, align 4
  %40 = load i32, i32* %12, align 4
  %41 = mul i32 %40, 8
  %42 = alloca i8, i32 %41, align 8
  %43 = bitcast i8* %42 to %union.jvalue*
  store %union.jvalue* %43, %union.jvalue** %10, align 4
  store i32 0, i32* %13, align 4
  br label %44

44:                                               ; preds = %159, %4
  %45 = load i32, i32* %13, align 4
  %46 = load i32, i32* %12, align 4
  %47 = icmp slt i32 %45, %46
  br i1 %47, label %48, label %162

48:                                               ; preds = %44
  %49 = load i32, i32* %13, align 4
  %50 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i32 0, i32 %49
  %51 = load i8, i8* %50, align 1
  %52 = zext i8 %51 to i32
  switch i32 %52, label %159 [
    i32 90, label %53
    i32 66, label %64
    i32 83, label %75
    i32 67, label %86
    i32 73, label %98
    i32 74, label %108
    i32 68, label %119
    i32 70, label %133
    i32 76, label %148
  ]

53:                                               ; preds = %48
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %10, align 4
  %61 = load i32, i32* %13, align 4
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i32 %61
  %63 = bitcast %union.jvalue* %62 to i8*
  store i8 %59, i8* %63, align 8
  br label %159

64:                                               ; preds = %48
  %65 = bitcast %struct.__va_list* %5 to i8**
  %66 = load i8*, i8** %65, align 4
  %67 = getelementptr inbounds i8, i8* %66, i32 4
  store i8* %67, i8** %65, align 4
  %68 = bitcast i8* %66 to i32*
  %69 = load i32, i32* %68, align 4
  %70 = trunc i32 %69 to i8
  %71 = load %union.jvalue*, %union.jvalue** %10, align 4
  %72 = load i32, i32* %13, align 4
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %71, i32 %72
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %70, i8* %74, align 8
  br label %159

75:                                               ; preds = %48
  %76 = bitcast %struct.__va_list* %5 to i8**
  %77 = load i8*, i8** %76, align 4
  %78 = getelementptr inbounds i8, i8* %77, i32 4
  store i8* %78, i8** %76, align 4
  %79 = bitcast i8* %77 to i32*
  %80 = load i32, i32* %79, align 4
  %81 = trunc i32 %80 to i16
  %82 = load %union.jvalue*, %union.jvalue** %10, align 4
  %83 = load i32, i32* %13, align 4
  %84 = getelementptr inbounds %union.jvalue, %union.jvalue* %82, i32 %83
  %85 = bitcast %union.jvalue* %84 to i16*
  store i16 %81, i16* %85, align 8
  br label %159

86:                                               ; preds = %48
  %87 = bitcast %struct.__va_list* %5 to i8**
  %88 = load i8*, i8** %87, align 4
  %89 = getelementptr inbounds i8, i8* %88, i32 4
  store i8* %89, i8** %87, align 4
  %90 = bitcast i8* %88 to i32*
  %91 = load i32, i32* %90, align 4
  %92 = trunc i32 %91 to i16
  %93 = zext i16 %92 to i32
  %94 = load %union.jvalue*, %union.jvalue** %10, align 4
  %95 = load i32, i32* %13, align 4
  %96 = getelementptr inbounds %union.jvalue, %union.jvalue* %94, i32 %95
  %97 = bitcast %union.jvalue* %96 to i32*
  store i32 %93, i32* %97, align 8
  br label %159

98:                                               ; preds = %48
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load %union.jvalue*, %union.jvalue** %10, align 4
  %105 = load i32, i32* %13, align 4
  %106 = getelementptr inbounds %union.jvalue, %union.jvalue* %104, i32 %105
  %107 = bitcast %union.jvalue* %106 to i32*
  store i32 %103, i32* %107, align 8
  br label %159

108:                                              ; preds = %48
  %109 = bitcast %struct.__va_list* %5 to i8**
  %110 = load i8*, i8** %109, align 4
  %111 = getelementptr inbounds i8, i8* %110, i32 4
  store i8* %111, i8** %109, align 4
  %112 = bitcast i8* %110 to i32*
  %113 = load i32, i32* %112, align 4
  %114 = sext i32 %113 to i64
  %115 = load %union.jvalue*, %union.jvalue** %10, align 4
  %116 = load i32, i32* %13, align 4
  %117 = getelementptr inbounds %union.jvalue, %union.jvalue* %115, i32 %116
  %118 = bitcast %union.jvalue* %117 to i64*
  store i64 %114, i64* %118, align 8
  br label %159

119:                                              ; preds = %48
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = ptrtoint i8* %121 to i32
  %123 = add i32 %122, 7
  %124 = and i32 %123, -8
  %125 = inttoptr i32 %124 to i8*
  %126 = getelementptr inbounds i8, i8* %125, i32 8
  store i8* %126, i8** %120, align 4
  %127 = bitcast i8* %125 to double*
  %128 = load double, double* %127, align 8
  %129 = load %union.jvalue*, %union.jvalue** %10, align 4
  %130 = load i32, i32* %13, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to double*
  store double %128, double* %132, align 8
  br label %159

133:                                              ; preds = %48
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = ptrtoint i8* %135 to i32
  %137 = add i32 %136, 7
  %138 = and i32 %137, -8
  %139 = inttoptr i32 %138 to i8*
  %140 = getelementptr inbounds i8, i8* %139, i32 8
  store i8* %140, i8** %134, align 4
  %141 = bitcast i8* %139 to double*
  %142 = load double, double* %141, align 8
  %143 = fptrunc double %142 to float
  %144 = load %union.jvalue*, %union.jvalue** %10, align 4
  %145 = load i32, i32* %13, align 4
  %146 = getelementptr inbounds %union.jvalue, %union.jvalue* %144, i32 %145
  %147 = bitcast %union.jvalue* %146 to float*
  store float %143, float* %147, align 8
  br label %159

148:                                              ; preds = %48
  %149 = bitcast %struct.__va_list* %5 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = getelementptr inbounds i8, i8* %150, i32 4
  store i8* %151, i8** %149, align 4
  %152 = bitcast i8* %150 to i8**
  %153 = load i8*, i8** %152, align 4
  %154 = bitcast i8* %153 to %struct._jobject*
  %155 = load %union.jvalue*, %union.jvalue** %10, align 4
  %156 = load i32, i32* %13, align 4
  %157 = getelementptr inbounds %union.jvalue, %union.jvalue* %155, i32 %156
  %158 = bitcast %union.jvalue* %157 to %struct._jobject**
  store %struct._jobject* %154, %struct._jobject** %158, align 8
  br label %159

159:                                              ; preds = %148, %133, %119, %108, %98, %86, %75, %64, %53, %48
  %160 = load i32, i32* %13, align 4
  %161 = add nsw i32 %160, 1
  store i32 %161, i32* %13, align 4
  br label %44, !llvm.loop !21

162:                                              ; preds = %44
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %164 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %163, align 4
  %165 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %164, i32 0, i32 75
  %166 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %165, align 4
  %167 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %168 = load %struct._jobject*, %struct._jobject** %7, align 4
  %169 = load %struct._jobject*, %struct._jobject** %8, align 4
  %170 = load %struct._jmethodID*, %struct._jmethodID** %9, align 4
  %171 = load %union.jvalue*, %union.jvalue** %10, align 4
  %172 = call zeroext i16 %166(%struct.JNINativeInterface_** noundef %167, %struct._jobject* noundef %168, %struct._jobject* noundef %169, %struct._jmethodID* noundef %170, %union.jvalue* noundef %171) #2
  call void @llvm.stackrestore(i8* %28)
  store i16 %172, i16* %18, align 2
  %173 = bitcast %struct.__va_list* %19 to i8*
  call void @llvm.va_end(i8* %173)
  %174 = load i16, i16* %18, align 2
  ret i16 %174
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i16 @JNI_CallNonvirtualCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca %union.jvalue*, align 4
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %6, i32 0, i32 0
  %16 = bitcast i8** %15 to [1 x i32]*
  store [1 x i32] %4, [1 x i32]* %16, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %7, align 4
  store %struct._jobject* %1, %struct._jobject** %8, align 4
  store %struct._jobject* %2, %struct._jobject** %9, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %10, align 4
  br label %17

17:                                               ; preds = %5
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %19 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %18, align 4
  %20 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %19, i32 0, i32 0
  %21 = load i8*, i8** %20, align 4
  %22 = bitcast i8* %21 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %24 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i32 0, i32 0
  %26 = call i32 %22(%struct.JNINativeInterface_** noundef %23, %struct._jmethodID* noundef %24, i8* noundef %25)
  store i32 %26, i32* %13, align 4
  %27 = load i32, i32* %13, align 4
  %28 = mul i32 %27, 8
  %29 = alloca i8, i32 %28, align 8
  %30 = bitcast i8* %29 to %union.jvalue*
  store %union.jvalue* %30, %union.jvalue** %11, align 4
  store i32 0, i32* %14, align 4
  br label %31

31:                                               ; preds = %147, %17
  %32 = load i32, i32* %14, align 4
  %33 = load i32, i32* %13, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %150

35:                                               ; preds = %31
  %36 = load i32, i32* %14, align 4
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i32 0, i32 %36
  %38 = load i8, i8* %37, align 1
  %39 = zext i8 %38 to i32
  switch i32 %39, label %146 [
    i32 90, label %40
    i32 66, label %51
    i32 83, label %62
    i32 67, label %73
    i32 73, label %85
    i32 74, label %95
    i32 68, label %106
    i32 70, label %120
    i32 76, label %135
  ]

40:                                               ; preds = %35
  %41 = bitcast %struct.__va_list* %6 to i8**
  %42 = load i8*, i8** %41, align 4
  %43 = getelementptr inbounds i8, i8* %42, i32 4
  store i8* %43, i8** %41, align 4
  %44 = bitcast i8* %42 to i32*
  %45 = load i32, i32* %44, align 4
  %46 = trunc i32 %45 to i8
  %47 = load %union.jvalue*, %union.jvalue** %11, align 4
  %48 = load i32, i32* %14, align 4
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %47, i32 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %146

51:                                               ; preds = %35
  %52 = bitcast %struct.__va_list* %6 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %11, align 4
  %59 = load i32, i32* %14, align 4
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i32 %59
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %57, i8* %61, align 8
  br label %146

62:                                               ; preds = %35
  %63 = bitcast %struct.__va_list* %6 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load %union.jvalue*, %union.jvalue** %11, align 4
  %70 = load i32, i32* %14, align 4
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %69, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %68, i16* %72, align 8
  br label %146

73:                                               ; preds = %35
  %74 = bitcast %struct.__va_list* %6 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = trunc i32 %78 to i16
  %80 = zext i16 %79 to i32
  %81 = load %union.jvalue*, %union.jvalue** %11, align 4
  %82 = load i32, i32* %14, align 4
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %81, i32 %82
  %84 = bitcast %union.jvalue* %83 to i32*
  store i32 %80, i32* %84, align 8
  br label %146

85:                                               ; preds = %35
  %86 = bitcast %struct.__va_list* %6 to i8**
  %87 = load i8*, i8** %86, align 4
  %88 = getelementptr inbounds i8, i8* %87, i32 4
  store i8* %88, i8** %86, align 4
  %89 = bitcast i8* %87 to i32*
  %90 = load i32, i32* %89, align 4
  %91 = load %union.jvalue*, %union.jvalue** %11, align 4
  %92 = load i32, i32* %14, align 4
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %91, i32 %92
  %94 = bitcast %union.jvalue* %93 to i32*
  store i32 %90, i32* %94, align 8
  br label %146

95:                                               ; preds = %35
  %96 = bitcast %struct.__va_list* %6 to i8**
  %97 = load i8*, i8** %96, align 4
  %98 = getelementptr inbounds i8, i8* %97, i32 4
  store i8* %98, i8** %96, align 4
  %99 = bitcast i8* %97 to i32*
  %100 = load i32, i32* %99, align 4
  %101 = sext i32 %100 to i64
  %102 = load %union.jvalue*, %union.jvalue** %11, align 4
  %103 = load i32, i32* %14, align 4
  %104 = getelementptr inbounds %union.jvalue, %union.jvalue* %102, i32 %103
  %105 = bitcast %union.jvalue* %104 to i64*
  store i64 %101, i64* %105, align 8
  br label %146

106:                                              ; preds = %35
  %107 = bitcast %struct.__va_list* %6 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load %union.jvalue*, %union.jvalue** %11, align 4
  %117 = load i32, i32* %14, align 4
  %118 = getelementptr inbounds %union.jvalue, %union.jvalue* %116, i32 %117
  %119 = bitcast %union.jvalue* %118 to double*
  store double %115, double* %119, align 8
  br label %146

120:                                              ; preds = %35
  %121 = bitcast %struct.__va_list* %6 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = ptrtoint i8* %122 to i32
  %124 = add i32 %123, 7
  %125 = and i32 %124, -8
  %126 = inttoptr i32 %125 to i8*
  %127 = getelementptr inbounds i8, i8* %126, i32 8
  store i8* %127, i8** %121, align 4
  %128 = bitcast i8* %126 to double*
  %129 = load double, double* %128, align 8
  %130 = fptrunc double %129 to float
  %131 = load %union.jvalue*, %union.jvalue** %11, align 4
  %132 = load i32, i32* %14, align 4
  %133 = getelementptr inbounds %union.jvalue, %union.jvalue* %131, i32 %132
  %134 = bitcast %union.jvalue* %133 to float*
  store float %130, float* %134, align 8
  br label %146

135:                                              ; preds = %35
  %136 = bitcast %struct.__va_list* %6 to i8**
  %137 = load i8*, i8** %136, align 4
  %138 = getelementptr inbounds i8, i8* %137, i32 4
  store i8* %138, i8** %136, align 4
  %139 = bitcast i8* %137 to i8**
  %140 = load i8*, i8** %139, align 4
  %141 = bitcast i8* %140 to %struct._jobject*
  %142 = load %union.jvalue*, %union.jvalue** %11, align 4
  %143 = load i32, i32* %14, align 4
  %144 = getelementptr inbounds %union.jvalue, %union.jvalue* %142, i32 %143
  %145 = bitcast %union.jvalue* %144 to %struct._jobject**
  store %struct._jobject* %141, %struct._jobject** %145, align 8
  br label %146

146:                                              ; preds = %35, %135, %120, %106, %95, %85, %73, %62, %51, %40
  br label %147

147:                                              ; preds = %146
  %148 = load i32, i32* %14, align 4
  %149 = add nsw i32 %148, 1
  store i32 %149, i32* %14, align 4
  br label %31, !llvm.loop !21

150:                                              ; preds = %31
  br label %151

151:                                              ; preds = %150
  %152 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %153 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %152, align 4
  %154 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %153, i32 0, i32 75
  %155 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %154, align 4
  %156 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %157 = load %struct._jobject*, %struct._jobject** %8, align 4
  %158 = load %struct._jobject*, %struct._jobject** %9, align 4
  %159 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %160 = load %union.jvalue*, %union.jvalue** %11, align 4
  %161 = call zeroext i16 %155(%struct.JNINativeInterface_** noundef %156, %struct._jobject* noundef %157, %struct._jobject* noundef %158, %struct._jmethodID* noundef %159, %union.jvalue* noundef %160)
  ret i16 %161
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i16 @JNI_CallStaticCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca i16, align 2
  %16 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %17 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_start(i8* %17)
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %19 = load %struct._jobject*, %struct._jobject** %13, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %21 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %16, i32 0, i32 0
  %22 = bitcast i8** %21 to [1 x i32]*
  %23 = load [1 x i32], [1 x i32]* %22, align 4
  %24 = call i8* @llvm.stacksave()
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  store [1 x i32] %23, [1 x i32]* %26, align 4
  store %struct.JNINativeInterface_** %18, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %19, %struct._jobject** %6, align 4
  store %struct._jmethodID* %20, %struct._jmethodID** %7, align 4
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %28 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %27, align 4
  %29 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %28, i32 0, i32 0
  %30 = load i8*, i8** %29, align 4
  %31 = bitcast i8* %30 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %32 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %33 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %35 = call i32 %31(%struct.JNINativeInterface_** noundef %32, %struct._jmethodID* noundef %33, i8* noundef %34) #2
  store i32 %35, i32* %10, align 4
  %36 = load i32, i32* %10, align 4
  %37 = mul i32 %36, 8
  %38 = alloca i8, i32 %37, align 8
  %39 = bitcast i8* %38 to %union.jvalue*
  store %union.jvalue* %39, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %40

40:                                               ; preds = %155, %3
  %41 = load i32, i32* %11, align 4
  %42 = load i32, i32* %10, align 4
  %43 = icmp slt i32 %41, %42
  br i1 %43, label %44, label %158

44:                                               ; preds = %40
  %45 = load i32, i32* %11, align 4
  %46 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  switch i32 %48, label %155 [
    i32 90, label %49
    i32 66, label %60
    i32 83, label %71
    i32 67, label %82
    i32 73, label %94
    i32 74, label %104
    i32 68, label %115
    i32 70, label %129
    i32 76, label %144
  ]

49:                                               ; preds = %44
  %50 = bitcast %struct.__va_list* %4 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %8, align 4
  %57 = load i32, i32* %11, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %155

60:                                               ; preds = %44
  %61 = bitcast %struct.__va_list* %4 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i8
  %67 = load %union.jvalue*, %union.jvalue** %8, align 4
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i8*
  store i8 %66, i8* %70, align 8
  br label %155

71:                                               ; preds = %44
  %72 = bitcast %struct.__va_list* %4 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = load %union.jvalue*, %union.jvalue** %8, align 4
  %79 = load i32, i32* %11, align 4
  %80 = getelementptr inbounds %union.jvalue, %union.jvalue* %78, i32 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %155

82:                                               ; preds = %44
  %83 = bitcast %struct.__va_list* %4 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = zext i16 %88 to i32
  %90 = load %union.jvalue*, %union.jvalue** %8, align 4
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds %union.jvalue, %union.jvalue* %90, i32 %91
  %93 = bitcast %union.jvalue* %92 to i32*
  store i32 %89, i32* %93, align 8
  br label %155

94:                                               ; preds = %44
  %95 = bitcast %struct.__va_list* %4 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = getelementptr inbounds i8, i8* %96, i32 4
  store i8* %97, i8** %95, align 4
  %98 = bitcast i8* %96 to i32*
  %99 = load i32, i32* %98, align 4
  %100 = load %union.jvalue*, %union.jvalue** %8, align 4
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i32*
  store i32 %99, i32* %103, align 8
  br label %155

104:                                              ; preds = %44
  %105 = bitcast %struct.__va_list* %4 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = getelementptr inbounds i8, i8* %106, i32 4
  store i8* %107, i8** %105, align 4
  %108 = bitcast i8* %106 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = sext i32 %109 to i64
  %111 = load %union.jvalue*, %union.jvalue** %8, align 4
  %112 = load i32, i32* %11, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %111, i32 %112
  %114 = bitcast %union.jvalue* %113 to i64*
  store i64 %110, i64* %114, align 8
  br label %155

115:                                              ; preds = %44
  %116 = bitcast %struct.__va_list* %4 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = ptrtoint i8* %117 to i32
  %119 = add i32 %118, 7
  %120 = and i32 %119, -8
  %121 = inttoptr i32 %120 to i8*
  %122 = getelementptr inbounds i8, i8* %121, i32 8
  store i8* %122, i8** %116, align 4
  %123 = bitcast i8* %121 to double*
  %124 = load double, double* %123, align 8
  %125 = load %union.jvalue*, %union.jvalue** %8, align 4
  %126 = load i32, i32* %11, align 4
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %125, i32 %126
  %128 = bitcast %union.jvalue* %127 to double*
  store double %124, double* %128, align 8
  br label %155

129:                                              ; preds = %44
  %130 = bitcast %struct.__va_list* %4 to i8**
  %131 = load i8*, i8** %130, align 4
  %132 = ptrtoint i8* %131 to i32
  %133 = add i32 %132, 7
  %134 = and i32 %133, -8
  %135 = inttoptr i32 %134 to i8*
  %136 = getelementptr inbounds i8, i8* %135, i32 8
  store i8* %136, i8** %130, align 4
  %137 = bitcast i8* %135 to double*
  %138 = load double, double* %137, align 8
  %139 = fptrunc double %138 to float
  %140 = load %union.jvalue*, %union.jvalue** %8, align 4
  %141 = load i32, i32* %11, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %139, float* %143, align 8
  br label %155

144:                                              ; preds = %44
  %145 = bitcast %struct.__va_list* %4 to i8**
  %146 = load i8*, i8** %145, align 4
  %147 = getelementptr inbounds i8, i8* %146, i32 4
  store i8* %147, i8** %145, align 4
  %148 = bitcast i8* %146 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = bitcast i8* %149 to %struct._jobject*
  %151 = load %union.jvalue*, %union.jvalue** %8, align 4
  %152 = load i32, i32* %11, align 4
  %153 = getelementptr inbounds %union.jvalue, %union.jvalue* %151, i32 %152
  %154 = bitcast %union.jvalue* %153 to %struct._jobject**
  store %struct._jobject* %150, %struct._jobject** %154, align 8
  br label %155

155:                                              ; preds = %144, %129, %115, %104, %94, %82, %71, %60, %49, %44
  %156 = load i32, i32* %11, align 4
  %157 = add nsw i32 %156, 1
  store i32 %157, i32* %11, align 4
  br label %40, !llvm.loop !22

158:                                              ; preds = %40
  %159 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %160 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %159, align 4
  %161 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %160, i32 0, i32 125
  %162 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %161, align 4
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %164 = load %struct._jobject*, %struct._jobject** %6, align 4
  %165 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %166 = load %union.jvalue*, %union.jvalue** %8, align 4
  %167 = call zeroext i16 %162(%struct.JNINativeInterface_** noundef %163, %struct._jobject* noundef %164, %struct._jmethodID* noundef %165, %union.jvalue* noundef %166) #2
  call void @llvm.stackrestore(i8* %24)
  store i16 %167, i16* %15, align 2
  %168 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_end(i8* %168)
  %169 = load i16, i16* %15, align 2
  ret i16 %169
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i16 @JNI_CallStaticCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !22

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 125
  %153 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  %158 = call zeroext i16 %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret i16 %158
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i16 @JNI_CallShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca i16, align 2
  %16 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %17 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_start(i8* %17)
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %19 = load %struct._jobject*, %struct._jobject** %13, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %21 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %16, i32 0, i32 0
  %22 = bitcast i8** %21 to [1 x i32]*
  %23 = load [1 x i32], [1 x i32]* %22, align 4
  %24 = call i8* @llvm.stacksave()
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  store [1 x i32] %23, [1 x i32]* %26, align 4
  store %struct.JNINativeInterface_** %18, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %19, %struct._jobject** %6, align 4
  store %struct._jmethodID* %20, %struct._jmethodID** %7, align 4
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %28 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %27, align 4
  %29 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %28, i32 0, i32 0
  %30 = load i8*, i8** %29, align 4
  %31 = bitcast i8* %30 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %32 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %33 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %35 = call i32 %31(%struct.JNINativeInterface_** noundef %32, %struct._jmethodID* noundef %33, i8* noundef %34) #2
  store i32 %35, i32* %10, align 4
  %36 = load i32, i32* %10, align 4
  %37 = mul i32 %36, 8
  %38 = alloca i8, i32 %37, align 8
  %39 = bitcast i8* %38 to %union.jvalue*
  store %union.jvalue* %39, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %40

40:                                               ; preds = %155, %3
  %41 = load i32, i32* %11, align 4
  %42 = load i32, i32* %10, align 4
  %43 = icmp slt i32 %41, %42
  br i1 %43, label %44, label %158

44:                                               ; preds = %40
  %45 = load i32, i32* %11, align 4
  %46 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  switch i32 %48, label %155 [
    i32 90, label %49
    i32 66, label %60
    i32 83, label %71
    i32 67, label %82
    i32 73, label %94
    i32 74, label %104
    i32 68, label %115
    i32 70, label %129
    i32 76, label %144
  ]

49:                                               ; preds = %44
  %50 = bitcast %struct.__va_list* %4 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %8, align 4
  %57 = load i32, i32* %11, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %155

60:                                               ; preds = %44
  %61 = bitcast %struct.__va_list* %4 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i8
  %67 = load %union.jvalue*, %union.jvalue** %8, align 4
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i8*
  store i8 %66, i8* %70, align 8
  br label %155

71:                                               ; preds = %44
  %72 = bitcast %struct.__va_list* %4 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = load %union.jvalue*, %union.jvalue** %8, align 4
  %79 = load i32, i32* %11, align 4
  %80 = getelementptr inbounds %union.jvalue, %union.jvalue* %78, i32 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %155

82:                                               ; preds = %44
  %83 = bitcast %struct.__va_list* %4 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = zext i16 %88 to i32
  %90 = load %union.jvalue*, %union.jvalue** %8, align 4
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds %union.jvalue, %union.jvalue* %90, i32 %91
  %93 = bitcast %union.jvalue* %92 to i32*
  store i32 %89, i32* %93, align 8
  br label %155

94:                                               ; preds = %44
  %95 = bitcast %struct.__va_list* %4 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = getelementptr inbounds i8, i8* %96, i32 4
  store i8* %97, i8** %95, align 4
  %98 = bitcast i8* %96 to i32*
  %99 = load i32, i32* %98, align 4
  %100 = load %union.jvalue*, %union.jvalue** %8, align 4
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i32*
  store i32 %99, i32* %103, align 8
  br label %155

104:                                              ; preds = %44
  %105 = bitcast %struct.__va_list* %4 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = getelementptr inbounds i8, i8* %106, i32 4
  store i8* %107, i8** %105, align 4
  %108 = bitcast i8* %106 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = sext i32 %109 to i64
  %111 = load %union.jvalue*, %union.jvalue** %8, align 4
  %112 = load i32, i32* %11, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %111, i32 %112
  %114 = bitcast %union.jvalue* %113 to i64*
  store i64 %110, i64* %114, align 8
  br label %155

115:                                              ; preds = %44
  %116 = bitcast %struct.__va_list* %4 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = ptrtoint i8* %117 to i32
  %119 = add i32 %118, 7
  %120 = and i32 %119, -8
  %121 = inttoptr i32 %120 to i8*
  %122 = getelementptr inbounds i8, i8* %121, i32 8
  store i8* %122, i8** %116, align 4
  %123 = bitcast i8* %121 to double*
  %124 = load double, double* %123, align 8
  %125 = load %union.jvalue*, %union.jvalue** %8, align 4
  %126 = load i32, i32* %11, align 4
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %125, i32 %126
  %128 = bitcast %union.jvalue* %127 to double*
  store double %124, double* %128, align 8
  br label %155

129:                                              ; preds = %44
  %130 = bitcast %struct.__va_list* %4 to i8**
  %131 = load i8*, i8** %130, align 4
  %132 = ptrtoint i8* %131 to i32
  %133 = add i32 %132, 7
  %134 = and i32 %133, -8
  %135 = inttoptr i32 %134 to i8*
  %136 = getelementptr inbounds i8, i8* %135, i32 8
  store i8* %136, i8** %130, align 4
  %137 = bitcast i8* %135 to double*
  %138 = load double, double* %137, align 8
  %139 = fptrunc double %138 to float
  %140 = load %union.jvalue*, %union.jvalue** %8, align 4
  %141 = load i32, i32* %11, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %139, float* %143, align 8
  br label %155

144:                                              ; preds = %44
  %145 = bitcast %struct.__va_list* %4 to i8**
  %146 = load i8*, i8** %145, align 4
  %147 = getelementptr inbounds i8, i8* %146, i32 4
  store i8* %147, i8** %145, align 4
  %148 = bitcast i8* %146 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = bitcast i8* %149 to %struct._jobject*
  %151 = load %union.jvalue*, %union.jvalue** %8, align 4
  %152 = load i32, i32* %11, align 4
  %153 = getelementptr inbounds %union.jvalue, %union.jvalue* %151, i32 %152
  %154 = bitcast %union.jvalue* %153 to %struct._jobject**
  store %struct._jobject* %150, %struct._jobject** %154, align 8
  br label %155

155:                                              ; preds = %144, %129, %115, %104, %94, %82, %71, %60, %49, %44
  %156 = load i32, i32* %11, align 4
  %157 = add nsw i32 %156, 1
  store i32 %157, i32* %11, align 4
  br label %40, !llvm.loop !23

158:                                              ; preds = %40
  %159 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %160 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %159, align 4
  %161 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %160, i32 0, i32 48
  %162 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %161, align 4
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %164 = load %struct._jobject*, %struct._jobject** %6, align 4
  %165 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %166 = load %union.jvalue*, %union.jvalue** %8, align 4
  %167 = call signext i16 %162(%struct.JNINativeInterface_** noundef %163, %struct._jobject* noundef %164, %struct._jmethodID* noundef %165, %union.jvalue* noundef %166) #2
  call void @llvm.stackrestore(i8* %24)
  store i16 %167, i16* %15, align 2
  %168 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_end(i8* %168)
  %169 = load i16, i16* %15, align 2
  ret i16 %169
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i16 @JNI_CallShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !23

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 48
  %153 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  %158 = call signext i16 %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret i16 %158
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i16 @JNI_CallNonvirtualShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jmethodID*, align 4
  %10 = alloca %union.jvalue*, align 4
  %11 = alloca [257 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca %struct.JNINativeInterface_**, align 4
  %15 = alloca %struct._jobject*, align 4
  %16 = alloca %struct._jobject*, align 4
  %17 = alloca %struct._jmethodID*, align 4
  %18 = alloca i16, align 2
  %19 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %14, align 4
  store %struct._jobject* %1, %struct._jobject** %15, align 4
  store %struct._jobject* %2, %struct._jobject** %16, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %17, align 4
  %20 = bitcast %struct.__va_list* %19 to i8*
  call void @llvm.va_start(i8* %20)
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %14, align 4
  %22 = load %struct._jobject*, %struct._jobject** %15, align 4
  %23 = load %struct._jobject*, %struct._jobject** %16, align 4
  %24 = load %struct._jmethodID*, %struct._jmethodID** %17, align 4
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %19, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  %27 = load [1 x i32], [1 x i32]* %26, align 4
  %28 = call i8* @llvm.stacksave()
  %29 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %30 = bitcast i8** %29 to [1 x i32]*
  store [1 x i32] %27, [1 x i32]* %30, align 4
  store %struct.JNINativeInterface_** %21, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %22, %struct._jobject** %7, align 4
  store %struct._jobject* %23, %struct._jobject** %8, align 4
  store %struct._jmethodID* %24, %struct._jmethodID** %9, align 4
  %31 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %32 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %31, align 4
  %33 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %32, i32 0, i32 0
  %34 = load i8*, i8** %33, align 4
  %35 = bitcast i8* %34 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %36 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %37 = load %struct._jmethodID*, %struct._jmethodID** %9, align 4
  %38 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i32 0, i32 0
  %39 = call i32 %35(%struct.JNINativeInterface_** noundef %36, %struct._jmethodID* noundef %37, i8* noundef %38) #2
  store i32 %39, i32* %12, align 4
  %40 = load i32, i32* %12, align 4
  %41 = mul i32 %40, 8
  %42 = alloca i8, i32 %41, align 8
  %43 = bitcast i8* %42 to %union.jvalue*
  store %union.jvalue* %43, %union.jvalue** %10, align 4
  store i32 0, i32* %13, align 4
  br label %44

44:                                               ; preds = %159, %4
  %45 = load i32, i32* %13, align 4
  %46 = load i32, i32* %12, align 4
  %47 = icmp slt i32 %45, %46
  br i1 %47, label %48, label %162

48:                                               ; preds = %44
  %49 = load i32, i32* %13, align 4
  %50 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i32 0, i32 %49
  %51 = load i8, i8* %50, align 1
  %52 = zext i8 %51 to i32
  switch i32 %52, label %159 [
    i32 90, label %53
    i32 66, label %64
    i32 83, label %75
    i32 67, label %86
    i32 73, label %98
    i32 74, label %108
    i32 68, label %119
    i32 70, label %133
    i32 76, label %148
  ]

53:                                               ; preds = %48
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %10, align 4
  %61 = load i32, i32* %13, align 4
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i32 %61
  %63 = bitcast %union.jvalue* %62 to i8*
  store i8 %59, i8* %63, align 8
  br label %159

64:                                               ; preds = %48
  %65 = bitcast %struct.__va_list* %5 to i8**
  %66 = load i8*, i8** %65, align 4
  %67 = getelementptr inbounds i8, i8* %66, i32 4
  store i8* %67, i8** %65, align 4
  %68 = bitcast i8* %66 to i32*
  %69 = load i32, i32* %68, align 4
  %70 = trunc i32 %69 to i8
  %71 = load %union.jvalue*, %union.jvalue** %10, align 4
  %72 = load i32, i32* %13, align 4
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %71, i32 %72
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %70, i8* %74, align 8
  br label %159

75:                                               ; preds = %48
  %76 = bitcast %struct.__va_list* %5 to i8**
  %77 = load i8*, i8** %76, align 4
  %78 = getelementptr inbounds i8, i8* %77, i32 4
  store i8* %78, i8** %76, align 4
  %79 = bitcast i8* %77 to i32*
  %80 = load i32, i32* %79, align 4
  %81 = trunc i32 %80 to i16
  %82 = load %union.jvalue*, %union.jvalue** %10, align 4
  %83 = load i32, i32* %13, align 4
  %84 = getelementptr inbounds %union.jvalue, %union.jvalue* %82, i32 %83
  %85 = bitcast %union.jvalue* %84 to i16*
  store i16 %81, i16* %85, align 8
  br label %159

86:                                               ; preds = %48
  %87 = bitcast %struct.__va_list* %5 to i8**
  %88 = load i8*, i8** %87, align 4
  %89 = getelementptr inbounds i8, i8* %88, i32 4
  store i8* %89, i8** %87, align 4
  %90 = bitcast i8* %88 to i32*
  %91 = load i32, i32* %90, align 4
  %92 = trunc i32 %91 to i16
  %93 = zext i16 %92 to i32
  %94 = load %union.jvalue*, %union.jvalue** %10, align 4
  %95 = load i32, i32* %13, align 4
  %96 = getelementptr inbounds %union.jvalue, %union.jvalue* %94, i32 %95
  %97 = bitcast %union.jvalue* %96 to i32*
  store i32 %93, i32* %97, align 8
  br label %159

98:                                               ; preds = %48
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load %union.jvalue*, %union.jvalue** %10, align 4
  %105 = load i32, i32* %13, align 4
  %106 = getelementptr inbounds %union.jvalue, %union.jvalue* %104, i32 %105
  %107 = bitcast %union.jvalue* %106 to i32*
  store i32 %103, i32* %107, align 8
  br label %159

108:                                              ; preds = %48
  %109 = bitcast %struct.__va_list* %5 to i8**
  %110 = load i8*, i8** %109, align 4
  %111 = getelementptr inbounds i8, i8* %110, i32 4
  store i8* %111, i8** %109, align 4
  %112 = bitcast i8* %110 to i32*
  %113 = load i32, i32* %112, align 4
  %114 = sext i32 %113 to i64
  %115 = load %union.jvalue*, %union.jvalue** %10, align 4
  %116 = load i32, i32* %13, align 4
  %117 = getelementptr inbounds %union.jvalue, %union.jvalue* %115, i32 %116
  %118 = bitcast %union.jvalue* %117 to i64*
  store i64 %114, i64* %118, align 8
  br label %159

119:                                              ; preds = %48
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = ptrtoint i8* %121 to i32
  %123 = add i32 %122, 7
  %124 = and i32 %123, -8
  %125 = inttoptr i32 %124 to i8*
  %126 = getelementptr inbounds i8, i8* %125, i32 8
  store i8* %126, i8** %120, align 4
  %127 = bitcast i8* %125 to double*
  %128 = load double, double* %127, align 8
  %129 = load %union.jvalue*, %union.jvalue** %10, align 4
  %130 = load i32, i32* %13, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to double*
  store double %128, double* %132, align 8
  br label %159

133:                                              ; preds = %48
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = ptrtoint i8* %135 to i32
  %137 = add i32 %136, 7
  %138 = and i32 %137, -8
  %139 = inttoptr i32 %138 to i8*
  %140 = getelementptr inbounds i8, i8* %139, i32 8
  store i8* %140, i8** %134, align 4
  %141 = bitcast i8* %139 to double*
  %142 = load double, double* %141, align 8
  %143 = fptrunc double %142 to float
  %144 = load %union.jvalue*, %union.jvalue** %10, align 4
  %145 = load i32, i32* %13, align 4
  %146 = getelementptr inbounds %union.jvalue, %union.jvalue* %144, i32 %145
  %147 = bitcast %union.jvalue* %146 to float*
  store float %143, float* %147, align 8
  br label %159

148:                                              ; preds = %48
  %149 = bitcast %struct.__va_list* %5 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = getelementptr inbounds i8, i8* %150, i32 4
  store i8* %151, i8** %149, align 4
  %152 = bitcast i8* %150 to i8**
  %153 = load i8*, i8** %152, align 4
  %154 = bitcast i8* %153 to %struct._jobject*
  %155 = load %union.jvalue*, %union.jvalue** %10, align 4
  %156 = load i32, i32* %13, align 4
  %157 = getelementptr inbounds %union.jvalue, %union.jvalue* %155, i32 %156
  %158 = bitcast %union.jvalue* %157 to %struct._jobject**
  store %struct._jobject* %154, %struct._jobject** %158, align 8
  br label %159

159:                                              ; preds = %148, %133, %119, %108, %98, %86, %75, %64, %53, %48
  %160 = load i32, i32* %13, align 4
  %161 = add nsw i32 %160, 1
  store i32 %161, i32* %13, align 4
  br label %44, !llvm.loop !24

162:                                              ; preds = %44
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %164 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %163, align 4
  %165 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %164, i32 0, i32 78
  %166 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %165, align 4
  %167 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %168 = load %struct._jobject*, %struct._jobject** %7, align 4
  %169 = load %struct._jobject*, %struct._jobject** %8, align 4
  %170 = load %struct._jmethodID*, %struct._jmethodID** %9, align 4
  %171 = load %union.jvalue*, %union.jvalue** %10, align 4
  %172 = call signext i16 %166(%struct.JNINativeInterface_** noundef %167, %struct._jobject* noundef %168, %struct._jobject* noundef %169, %struct._jmethodID* noundef %170, %union.jvalue* noundef %171) #2
  call void @llvm.stackrestore(i8* %28)
  store i16 %172, i16* %18, align 2
  %173 = bitcast %struct.__va_list* %19 to i8*
  call void @llvm.va_end(i8* %173)
  %174 = load i16, i16* %18, align 2
  ret i16 %174
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i16 @JNI_CallNonvirtualShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca %union.jvalue*, align 4
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %6, i32 0, i32 0
  %16 = bitcast i8** %15 to [1 x i32]*
  store [1 x i32] %4, [1 x i32]* %16, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %7, align 4
  store %struct._jobject* %1, %struct._jobject** %8, align 4
  store %struct._jobject* %2, %struct._jobject** %9, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %10, align 4
  br label %17

17:                                               ; preds = %5
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %19 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %18, align 4
  %20 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %19, i32 0, i32 0
  %21 = load i8*, i8** %20, align 4
  %22 = bitcast i8* %21 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %24 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i32 0, i32 0
  %26 = call i32 %22(%struct.JNINativeInterface_** noundef %23, %struct._jmethodID* noundef %24, i8* noundef %25)
  store i32 %26, i32* %13, align 4
  %27 = load i32, i32* %13, align 4
  %28 = mul i32 %27, 8
  %29 = alloca i8, i32 %28, align 8
  %30 = bitcast i8* %29 to %union.jvalue*
  store %union.jvalue* %30, %union.jvalue** %11, align 4
  store i32 0, i32* %14, align 4
  br label %31

31:                                               ; preds = %147, %17
  %32 = load i32, i32* %14, align 4
  %33 = load i32, i32* %13, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %150

35:                                               ; preds = %31
  %36 = load i32, i32* %14, align 4
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i32 0, i32 %36
  %38 = load i8, i8* %37, align 1
  %39 = zext i8 %38 to i32
  switch i32 %39, label %146 [
    i32 90, label %40
    i32 66, label %51
    i32 83, label %62
    i32 67, label %73
    i32 73, label %85
    i32 74, label %95
    i32 68, label %106
    i32 70, label %120
    i32 76, label %135
  ]

40:                                               ; preds = %35
  %41 = bitcast %struct.__va_list* %6 to i8**
  %42 = load i8*, i8** %41, align 4
  %43 = getelementptr inbounds i8, i8* %42, i32 4
  store i8* %43, i8** %41, align 4
  %44 = bitcast i8* %42 to i32*
  %45 = load i32, i32* %44, align 4
  %46 = trunc i32 %45 to i8
  %47 = load %union.jvalue*, %union.jvalue** %11, align 4
  %48 = load i32, i32* %14, align 4
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %47, i32 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %146

51:                                               ; preds = %35
  %52 = bitcast %struct.__va_list* %6 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %11, align 4
  %59 = load i32, i32* %14, align 4
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i32 %59
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %57, i8* %61, align 8
  br label %146

62:                                               ; preds = %35
  %63 = bitcast %struct.__va_list* %6 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load %union.jvalue*, %union.jvalue** %11, align 4
  %70 = load i32, i32* %14, align 4
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %69, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %68, i16* %72, align 8
  br label %146

73:                                               ; preds = %35
  %74 = bitcast %struct.__va_list* %6 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = trunc i32 %78 to i16
  %80 = zext i16 %79 to i32
  %81 = load %union.jvalue*, %union.jvalue** %11, align 4
  %82 = load i32, i32* %14, align 4
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %81, i32 %82
  %84 = bitcast %union.jvalue* %83 to i32*
  store i32 %80, i32* %84, align 8
  br label %146

85:                                               ; preds = %35
  %86 = bitcast %struct.__va_list* %6 to i8**
  %87 = load i8*, i8** %86, align 4
  %88 = getelementptr inbounds i8, i8* %87, i32 4
  store i8* %88, i8** %86, align 4
  %89 = bitcast i8* %87 to i32*
  %90 = load i32, i32* %89, align 4
  %91 = load %union.jvalue*, %union.jvalue** %11, align 4
  %92 = load i32, i32* %14, align 4
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %91, i32 %92
  %94 = bitcast %union.jvalue* %93 to i32*
  store i32 %90, i32* %94, align 8
  br label %146

95:                                               ; preds = %35
  %96 = bitcast %struct.__va_list* %6 to i8**
  %97 = load i8*, i8** %96, align 4
  %98 = getelementptr inbounds i8, i8* %97, i32 4
  store i8* %98, i8** %96, align 4
  %99 = bitcast i8* %97 to i32*
  %100 = load i32, i32* %99, align 4
  %101 = sext i32 %100 to i64
  %102 = load %union.jvalue*, %union.jvalue** %11, align 4
  %103 = load i32, i32* %14, align 4
  %104 = getelementptr inbounds %union.jvalue, %union.jvalue* %102, i32 %103
  %105 = bitcast %union.jvalue* %104 to i64*
  store i64 %101, i64* %105, align 8
  br label %146

106:                                              ; preds = %35
  %107 = bitcast %struct.__va_list* %6 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load %union.jvalue*, %union.jvalue** %11, align 4
  %117 = load i32, i32* %14, align 4
  %118 = getelementptr inbounds %union.jvalue, %union.jvalue* %116, i32 %117
  %119 = bitcast %union.jvalue* %118 to double*
  store double %115, double* %119, align 8
  br label %146

120:                                              ; preds = %35
  %121 = bitcast %struct.__va_list* %6 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = ptrtoint i8* %122 to i32
  %124 = add i32 %123, 7
  %125 = and i32 %124, -8
  %126 = inttoptr i32 %125 to i8*
  %127 = getelementptr inbounds i8, i8* %126, i32 8
  store i8* %127, i8** %121, align 4
  %128 = bitcast i8* %126 to double*
  %129 = load double, double* %128, align 8
  %130 = fptrunc double %129 to float
  %131 = load %union.jvalue*, %union.jvalue** %11, align 4
  %132 = load i32, i32* %14, align 4
  %133 = getelementptr inbounds %union.jvalue, %union.jvalue* %131, i32 %132
  %134 = bitcast %union.jvalue* %133 to float*
  store float %130, float* %134, align 8
  br label %146

135:                                              ; preds = %35
  %136 = bitcast %struct.__va_list* %6 to i8**
  %137 = load i8*, i8** %136, align 4
  %138 = getelementptr inbounds i8, i8* %137, i32 4
  store i8* %138, i8** %136, align 4
  %139 = bitcast i8* %137 to i8**
  %140 = load i8*, i8** %139, align 4
  %141 = bitcast i8* %140 to %struct._jobject*
  %142 = load %union.jvalue*, %union.jvalue** %11, align 4
  %143 = load i32, i32* %14, align 4
  %144 = getelementptr inbounds %union.jvalue, %union.jvalue* %142, i32 %143
  %145 = bitcast %union.jvalue* %144 to %struct._jobject**
  store %struct._jobject* %141, %struct._jobject** %145, align 8
  br label %146

146:                                              ; preds = %35, %135, %120, %106, %95, %85, %73, %62, %51, %40
  br label %147

147:                                              ; preds = %146
  %148 = load i32, i32* %14, align 4
  %149 = add nsw i32 %148, 1
  store i32 %149, i32* %14, align 4
  br label %31, !llvm.loop !24

150:                                              ; preds = %31
  br label %151

151:                                              ; preds = %150
  %152 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %153 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %152, align 4
  %154 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %153, i32 0, i32 78
  %155 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %154, align 4
  %156 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %157 = load %struct._jobject*, %struct._jobject** %8, align 4
  %158 = load %struct._jobject*, %struct._jobject** %9, align 4
  %159 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %160 = load %union.jvalue*, %union.jvalue** %11, align 4
  %161 = call signext i16 %155(%struct.JNINativeInterface_** noundef %156, %struct._jobject* noundef %157, %struct._jobject* noundef %158, %struct._jmethodID* noundef %159, %union.jvalue* noundef %160)
  ret i16 %161
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i16 @JNI_CallStaticShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca i16, align 2
  %16 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %17 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_start(i8* %17)
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %19 = load %struct._jobject*, %struct._jobject** %13, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %21 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %16, i32 0, i32 0
  %22 = bitcast i8** %21 to [1 x i32]*
  %23 = load [1 x i32], [1 x i32]* %22, align 4
  %24 = call i8* @llvm.stacksave()
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  store [1 x i32] %23, [1 x i32]* %26, align 4
  store %struct.JNINativeInterface_** %18, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %19, %struct._jobject** %6, align 4
  store %struct._jmethodID* %20, %struct._jmethodID** %7, align 4
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %28 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %27, align 4
  %29 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %28, i32 0, i32 0
  %30 = load i8*, i8** %29, align 4
  %31 = bitcast i8* %30 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %32 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %33 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %35 = call i32 %31(%struct.JNINativeInterface_** noundef %32, %struct._jmethodID* noundef %33, i8* noundef %34) #2
  store i32 %35, i32* %10, align 4
  %36 = load i32, i32* %10, align 4
  %37 = mul i32 %36, 8
  %38 = alloca i8, i32 %37, align 8
  %39 = bitcast i8* %38 to %union.jvalue*
  store %union.jvalue* %39, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %40

40:                                               ; preds = %155, %3
  %41 = load i32, i32* %11, align 4
  %42 = load i32, i32* %10, align 4
  %43 = icmp slt i32 %41, %42
  br i1 %43, label %44, label %158

44:                                               ; preds = %40
  %45 = load i32, i32* %11, align 4
  %46 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  switch i32 %48, label %155 [
    i32 90, label %49
    i32 66, label %60
    i32 83, label %71
    i32 67, label %82
    i32 73, label %94
    i32 74, label %104
    i32 68, label %115
    i32 70, label %129
    i32 76, label %144
  ]

49:                                               ; preds = %44
  %50 = bitcast %struct.__va_list* %4 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %8, align 4
  %57 = load i32, i32* %11, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %155

60:                                               ; preds = %44
  %61 = bitcast %struct.__va_list* %4 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i8
  %67 = load %union.jvalue*, %union.jvalue** %8, align 4
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i8*
  store i8 %66, i8* %70, align 8
  br label %155

71:                                               ; preds = %44
  %72 = bitcast %struct.__va_list* %4 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = load %union.jvalue*, %union.jvalue** %8, align 4
  %79 = load i32, i32* %11, align 4
  %80 = getelementptr inbounds %union.jvalue, %union.jvalue* %78, i32 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %155

82:                                               ; preds = %44
  %83 = bitcast %struct.__va_list* %4 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = zext i16 %88 to i32
  %90 = load %union.jvalue*, %union.jvalue** %8, align 4
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds %union.jvalue, %union.jvalue* %90, i32 %91
  %93 = bitcast %union.jvalue* %92 to i32*
  store i32 %89, i32* %93, align 8
  br label %155

94:                                               ; preds = %44
  %95 = bitcast %struct.__va_list* %4 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = getelementptr inbounds i8, i8* %96, i32 4
  store i8* %97, i8** %95, align 4
  %98 = bitcast i8* %96 to i32*
  %99 = load i32, i32* %98, align 4
  %100 = load %union.jvalue*, %union.jvalue** %8, align 4
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i32*
  store i32 %99, i32* %103, align 8
  br label %155

104:                                              ; preds = %44
  %105 = bitcast %struct.__va_list* %4 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = getelementptr inbounds i8, i8* %106, i32 4
  store i8* %107, i8** %105, align 4
  %108 = bitcast i8* %106 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = sext i32 %109 to i64
  %111 = load %union.jvalue*, %union.jvalue** %8, align 4
  %112 = load i32, i32* %11, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %111, i32 %112
  %114 = bitcast %union.jvalue* %113 to i64*
  store i64 %110, i64* %114, align 8
  br label %155

115:                                              ; preds = %44
  %116 = bitcast %struct.__va_list* %4 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = ptrtoint i8* %117 to i32
  %119 = add i32 %118, 7
  %120 = and i32 %119, -8
  %121 = inttoptr i32 %120 to i8*
  %122 = getelementptr inbounds i8, i8* %121, i32 8
  store i8* %122, i8** %116, align 4
  %123 = bitcast i8* %121 to double*
  %124 = load double, double* %123, align 8
  %125 = load %union.jvalue*, %union.jvalue** %8, align 4
  %126 = load i32, i32* %11, align 4
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %125, i32 %126
  %128 = bitcast %union.jvalue* %127 to double*
  store double %124, double* %128, align 8
  br label %155

129:                                              ; preds = %44
  %130 = bitcast %struct.__va_list* %4 to i8**
  %131 = load i8*, i8** %130, align 4
  %132 = ptrtoint i8* %131 to i32
  %133 = add i32 %132, 7
  %134 = and i32 %133, -8
  %135 = inttoptr i32 %134 to i8*
  %136 = getelementptr inbounds i8, i8* %135, i32 8
  store i8* %136, i8** %130, align 4
  %137 = bitcast i8* %135 to double*
  %138 = load double, double* %137, align 8
  %139 = fptrunc double %138 to float
  %140 = load %union.jvalue*, %union.jvalue** %8, align 4
  %141 = load i32, i32* %11, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %139, float* %143, align 8
  br label %155

144:                                              ; preds = %44
  %145 = bitcast %struct.__va_list* %4 to i8**
  %146 = load i8*, i8** %145, align 4
  %147 = getelementptr inbounds i8, i8* %146, i32 4
  store i8* %147, i8** %145, align 4
  %148 = bitcast i8* %146 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = bitcast i8* %149 to %struct._jobject*
  %151 = load %union.jvalue*, %union.jvalue** %8, align 4
  %152 = load i32, i32* %11, align 4
  %153 = getelementptr inbounds %union.jvalue, %union.jvalue* %151, i32 %152
  %154 = bitcast %union.jvalue* %153 to %struct._jobject**
  store %struct._jobject* %150, %struct._jobject** %154, align 8
  br label %155

155:                                              ; preds = %144, %129, %115, %104, %94, %82, %71, %60, %49, %44
  %156 = load i32, i32* %11, align 4
  %157 = add nsw i32 %156, 1
  store i32 %157, i32* %11, align 4
  br label %40, !llvm.loop !25

158:                                              ; preds = %40
  %159 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %160 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %159, align 4
  %161 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %160, i32 0, i32 128
  %162 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %161, align 4
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %164 = load %struct._jobject*, %struct._jobject** %6, align 4
  %165 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %166 = load %union.jvalue*, %union.jvalue** %8, align 4
  %167 = call signext i16 %162(%struct.JNINativeInterface_** noundef %163, %struct._jobject* noundef %164, %struct._jmethodID* noundef %165, %union.jvalue* noundef %166) #2
  call void @llvm.stackrestore(i8* %24)
  store i16 %167, i16* %15, align 2
  %168 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_end(i8* %168)
  %169 = load i16, i16* %15, align 2
  ret i16 %169
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i16 @JNI_CallStaticShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !25

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 128
  %153 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  %158 = call signext i16 %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret i16 %158
}

; Function Attrs: alwaysinline nounwind
define dso_local i32 @JNI_CallIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca i32, align 4
  %16 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %17 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_start(i8* %17)
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %19 = load %struct._jobject*, %struct._jobject** %13, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %21 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %16, i32 0, i32 0
  %22 = bitcast i8** %21 to [1 x i32]*
  %23 = load [1 x i32], [1 x i32]* %22, align 4
  %24 = call i8* @llvm.stacksave()
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  store [1 x i32] %23, [1 x i32]* %26, align 4
  store %struct.JNINativeInterface_** %18, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %19, %struct._jobject** %6, align 4
  store %struct._jmethodID* %20, %struct._jmethodID** %7, align 4
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %28 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %27, align 4
  %29 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %28, i32 0, i32 0
  %30 = load i8*, i8** %29, align 4
  %31 = bitcast i8* %30 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %32 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %33 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %35 = call i32 %31(%struct.JNINativeInterface_** noundef %32, %struct._jmethodID* noundef %33, i8* noundef %34) #2
  store i32 %35, i32* %10, align 4
  %36 = load i32, i32* %10, align 4
  %37 = mul i32 %36, 8
  %38 = alloca i8, i32 %37, align 8
  %39 = bitcast i8* %38 to %union.jvalue*
  store %union.jvalue* %39, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %40

40:                                               ; preds = %155, %3
  %41 = load i32, i32* %11, align 4
  %42 = load i32, i32* %10, align 4
  %43 = icmp slt i32 %41, %42
  br i1 %43, label %44, label %158

44:                                               ; preds = %40
  %45 = load i32, i32* %11, align 4
  %46 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  switch i32 %48, label %155 [
    i32 90, label %49
    i32 66, label %60
    i32 83, label %71
    i32 67, label %82
    i32 73, label %94
    i32 74, label %104
    i32 68, label %115
    i32 70, label %129
    i32 76, label %144
  ]

49:                                               ; preds = %44
  %50 = bitcast %struct.__va_list* %4 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %8, align 4
  %57 = load i32, i32* %11, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %155

60:                                               ; preds = %44
  %61 = bitcast %struct.__va_list* %4 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i8
  %67 = load %union.jvalue*, %union.jvalue** %8, align 4
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i8*
  store i8 %66, i8* %70, align 8
  br label %155

71:                                               ; preds = %44
  %72 = bitcast %struct.__va_list* %4 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = load %union.jvalue*, %union.jvalue** %8, align 4
  %79 = load i32, i32* %11, align 4
  %80 = getelementptr inbounds %union.jvalue, %union.jvalue* %78, i32 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %155

82:                                               ; preds = %44
  %83 = bitcast %struct.__va_list* %4 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = zext i16 %88 to i32
  %90 = load %union.jvalue*, %union.jvalue** %8, align 4
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds %union.jvalue, %union.jvalue* %90, i32 %91
  %93 = bitcast %union.jvalue* %92 to i32*
  store i32 %89, i32* %93, align 8
  br label %155

94:                                               ; preds = %44
  %95 = bitcast %struct.__va_list* %4 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = getelementptr inbounds i8, i8* %96, i32 4
  store i8* %97, i8** %95, align 4
  %98 = bitcast i8* %96 to i32*
  %99 = load i32, i32* %98, align 4
  %100 = load %union.jvalue*, %union.jvalue** %8, align 4
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i32*
  store i32 %99, i32* %103, align 8
  br label %155

104:                                              ; preds = %44
  %105 = bitcast %struct.__va_list* %4 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = getelementptr inbounds i8, i8* %106, i32 4
  store i8* %107, i8** %105, align 4
  %108 = bitcast i8* %106 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = sext i32 %109 to i64
  %111 = load %union.jvalue*, %union.jvalue** %8, align 4
  %112 = load i32, i32* %11, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %111, i32 %112
  %114 = bitcast %union.jvalue* %113 to i64*
  store i64 %110, i64* %114, align 8
  br label %155

115:                                              ; preds = %44
  %116 = bitcast %struct.__va_list* %4 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = ptrtoint i8* %117 to i32
  %119 = add i32 %118, 7
  %120 = and i32 %119, -8
  %121 = inttoptr i32 %120 to i8*
  %122 = getelementptr inbounds i8, i8* %121, i32 8
  store i8* %122, i8** %116, align 4
  %123 = bitcast i8* %121 to double*
  %124 = load double, double* %123, align 8
  %125 = load %union.jvalue*, %union.jvalue** %8, align 4
  %126 = load i32, i32* %11, align 4
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %125, i32 %126
  %128 = bitcast %union.jvalue* %127 to double*
  store double %124, double* %128, align 8
  br label %155

129:                                              ; preds = %44
  %130 = bitcast %struct.__va_list* %4 to i8**
  %131 = load i8*, i8** %130, align 4
  %132 = ptrtoint i8* %131 to i32
  %133 = add i32 %132, 7
  %134 = and i32 %133, -8
  %135 = inttoptr i32 %134 to i8*
  %136 = getelementptr inbounds i8, i8* %135, i32 8
  store i8* %136, i8** %130, align 4
  %137 = bitcast i8* %135 to double*
  %138 = load double, double* %137, align 8
  %139 = fptrunc double %138 to float
  %140 = load %union.jvalue*, %union.jvalue** %8, align 4
  %141 = load i32, i32* %11, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %139, float* %143, align 8
  br label %155

144:                                              ; preds = %44
  %145 = bitcast %struct.__va_list* %4 to i8**
  %146 = load i8*, i8** %145, align 4
  %147 = getelementptr inbounds i8, i8* %146, i32 4
  store i8* %147, i8** %145, align 4
  %148 = bitcast i8* %146 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = bitcast i8* %149 to %struct._jobject*
  %151 = load %union.jvalue*, %union.jvalue** %8, align 4
  %152 = load i32, i32* %11, align 4
  %153 = getelementptr inbounds %union.jvalue, %union.jvalue* %151, i32 %152
  %154 = bitcast %union.jvalue* %153 to %struct._jobject**
  store %struct._jobject* %150, %struct._jobject** %154, align 8
  br label %155

155:                                              ; preds = %144, %129, %115, %104, %94, %82, %71, %60, %49, %44
  %156 = load i32, i32* %11, align 4
  %157 = add nsw i32 %156, 1
  store i32 %157, i32* %11, align 4
  br label %40, !llvm.loop !26

158:                                              ; preds = %40
  %159 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %160 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %159, align 4
  %161 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %160, i32 0, i32 51
  %162 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %161, align 4
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %164 = load %struct._jobject*, %struct._jobject** %6, align 4
  %165 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %166 = load %union.jvalue*, %union.jvalue** %8, align 4
  %167 = call i32 %162(%struct.JNINativeInterface_** noundef %163, %struct._jobject* noundef %164, %struct._jmethodID* noundef %165, %union.jvalue* noundef %166) #2
  call void @llvm.stackrestore(i8* %24)
  store i32 %167, i32* %15, align 4
  %168 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_end(i8* %168)
  %169 = load i32, i32* %15, align 4
  ret i32 %169
}

; Function Attrs: alwaysinline nounwind
define dso_local i32 @JNI_CallIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !26

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 51
  %153 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  %158 = call i32 %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret i32 %158
}

; Function Attrs: alwaysinline nounwind
define dso_local i32 @JNI_CallNonvirtualIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jmethodID*, align 4
  %10 = alloca %union.jvalue*, align 4
  %11 = alloca [257 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca %struct.JNINativeInterface_**, align 4
  %15 = alloca %struct._jobject*, align 4
  %16 = alloca %struct._jobject*, align 4
  %17 = alloca %struct._jmethodID*, align 4
  %18 = alloca i32, align 4
  %19 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %14, align 4
  store %struct._jobject* %1, %struct._jobject** %15, align 4
  store %struct._jobject* %2, %struct._jobject** %16, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %17, align 4
  %20 = bitcast %struct.__va_list* %19 to i8*
  call void @llvm.va_start(i8* %20)
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %14, align 4
  %22 = load %struct._jobject*, %struct._jobject** %15, align 4
  %23 = load %struct._jobject*, %struct._jobject** %16, align 4
  %24 = load %struct._jmethodID*, %struct._jmethodID** %17, align 4
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %19, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  %27 = load [1 x i32], [1 x i32]* %26, align 4
  %28 = call i8* @llvm.stacksave()
  %29 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %30 = bitcast i8** %29 to [1 x i32]*
  store [1 x i32] %27, [1 x i32]* %30, align 4
  store %struct.JNINativeInterface_** %21, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %22, %struct._jobject** %7, align 4
  store %struct._jobject* %23, %struct._jobject** %8, align 4
  store %struct._jmethodID* %24, %struct._jmethodID** %9, align 4
  %31 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %32 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %31, align 4
  %33 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %32, i32 0, i32 0
  %34 = load i8*, i8** %33, align 4
  %35 = bitcast i8* %34 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %36 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %37 = load %struct._jmethodID*, %struct._jmethodID** %9, align 4
  %38 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i32 0, i32 0
  %39 = call i32 %35(%struct.JNINativeInterface_** noundef %36, %struct._jmethodID* noundef %37, i8* noundef %38) #2
  store i32 %39, i32* %12, align 4
  %40 = load i32, i32* %12, align 4
  %41 = mul i32 %40, 8
  %42 = alloca i8, i32 %41, align 8
  %43 = bitcast i8* %42 to %union.jvalue*
  store %union.jvalue* %43, %union.jvalue** %10, align 4
  store i32 0, i32* %13, align 4
  br label %44

44:                                               ; preds = %159, %4
  %45 = load i32, i32* %13, align 4
  %46 = load i32, i32* %12, align 4
  %47 = icmp slt i32 %45, %46
  br i1 %47, label %48, label %162

48:                                               ; preds = %44
  %49 = load i32, i32* %13, align 4
  %50 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i32 0, i32 %49
  %51 = load i8, i8* %50, align 1
  %52 = zext i8 %51 to i32
  switch i32 %52, label %159 [
    i32 90, label %53
    i32 66, label %64
    i32 83, label %75
    i32 67, label %86
    i32 73, label %98
    i32 74, label %108
    i32 68, label %119
    i32 70, label %133
    i32 76, label %148
  ]

53:                                               ; preds = %48
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %10, align 4
  %61 = load i32, i32* %13, align 4
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i32 %61
  %63 = bitcast %union.jvalue* %62 to i8*
  store i8 %59, i8* %63, align 8
  br label %159

64:                                               ; preds = %48
  %65 = bitcast %struct.__va_list* %5 to i8**
  %66 = load i8*, i8** %65, align 4
  %67 = getelementptr inbounds i8, i8* %66, i32 4
  store i8* %67, i8** %65, align 4
  %68 = bitcast i8* %66 to i32*
  %69 = load i32, i32* %68, align 4
  %70 = trunc i32 %69 to i8
  %71 = load %union.jvalue*, %union.jvalue** %10, align 4
  %72 = load i32, i32* %13, align 4
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %71, i32 %72
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %70, i8* %74, align 8
  br label %159

75:                                               ; preds = %48
  %76 = bitcast %struct.__va_list* %5 to i8**
  %77 = load i8*, i8** %76, align 4
  %78 = getelementptr inbounds i8, i8* %77, i32 4
  store i8* %78, i8** %76, align 4
  %79 = bitcast i8* %77 to i32*
  %80 = load i32, i32* %79, align 4
  %81 = trunc i32 %80 to i16
  %82 = load %union.jvalue*, %union.jvalue** %10, align 4
  %83 = load i32, i32* %13, align 4
  %84 = getelementptr inbounds %union.jvalue, %union.jvalue* %82, i32 %83
  %85 = bitcast %union.jvalue* %84 to i16*
  store i16 %81, i16* %85, align 8
  br label %159

86:                                               ; preds = %48
  %87 = bitcast %struct.__va_list* %5 to i8**
  %88 = load i8*, i8** %87, align 4
  %89 = getelementptr inbounds i8, i8* %88, i32 4
  store i8* %89, i8** %87, align 4
  %90 = bitcast i8* %88 to i32*
  %91 = load i32, i32* %90, align 4
  %92 = trunc i32 %91 to i16
  %93 = zext i16 %92 to i32
  %94 = load %union.jvalue*, %union.jvalue** %10, align 4
  %95 = load i32, i32* %13, align 4
  %96 = getelementptr inbounds %union.jvalue, %union.jvalue* %94, i32 %95
  %97 = bitcast %union.jvalue* %96 to i32*
  store i32 %93, i32* %97, align 8
  br label %159

98:                                               ; preds = %48
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load %union.jvalue*, %union.jvalue** %10, align 4
  %105 = load i32, i32* %13, align 4
  %106 = getelementptr inbounds %union.jvalue, %union.jvalue* %104, i32 %105
  %107 = bitcast %union.jvalue* %106 to i32*
  store i32 %103, i32* %107, align 8
  br label %159

108:                                              ; preds = %48
  %109 = bitcast %struct.__va_list* %5 to i8**
  %110 = load i8*, i8** %109, align 4
  %111 = getelementptr inbounds i8, i8* %110, i32 4
  store i8* %111, i8** %109, align 4
  %112 = bitcast i8* %110 to i32*
  %113 = load i32, i32* %112, align 4
  %114 = sext i32 %113 to i64
  %115 = load %union.jvalue*, %union.jvalue** %10, align 4
  %116 = load i32, i32* %13, align 4
  %117 = getelementptr inbounds %union.jvalue, %union.jvalue* %115, i32 %116
  %118 = bitcast %union.jvalue* %117 to i64*
  store i64 %114, i64* %118, align 8
  br label %159

119:                                              ; preds = %48
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = ptrtoint i8* %121 to i32
  %123 = add i32 %122, 7
  %124 = and i32 %123, -8
  %125 = inttoptr i32 %124 to i8*
  %126 = getelementptr inbounds i8, i8* %125, i32 8
  store i8* %126, i8** %120, align 4
  %127 = bitcast i8* %125 to double*
  %128 = load double, double* %127, align 8
  %129 = load %union.jvalue*, %union.jvalue** %10, align 4
  %130 = load i32, i32* %13, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to double*
  store double %128, double* %132, align 8
  br label %159

133:                                              ; preds = %48
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = ptrtoint i8* %135 to i32
  %137 = add i32 %136, 7
  %138 = and i32 %137, -8
  %139 = inttoptr i32 %138 to i8*
  %140 = getelementptr inbounds i8, i8* %139, i32 8
  store i8* %140, i8** %134, align 4
  %141 = bitcast i8* %139 to double*
  %142 = load double, double* %141, align 8
  %143 = fptrunc double %142 to float
  %144 = load %union.jvalue*, %union.jvalue** %10, align 4
  %145 = load i32, i32* %13, align 4
  %146 = getelementptr inbounds %union.jvalue, %union.jvalue* %144, i32 %145
  %147 = bitcast %union.jvalue* %146 to float*
  store float %143, float* %147, align 8
  br label %159

148:                                              ; preds = %48
  %149 = bitcast %struct.__va_list* %5 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = getelementptr inbounds i8, i8* %150, i32 4
  store i8* %151, i8** %149, align 4
  %152 = bitcast i8* %150 to i8**
  %153 = load i8*, i8** %152, align 4
  %154 = bitcast i8* %153 to %struct._jobject*
  %155 = load %union.jvalue*, %union.jvalue** %10, align 4
  %156 = load i32, i32* %13, align 4
  %157 = getelementptr inbounds %union.jvalue, %union.jvalue* %155, i32 %156
  %158 = bitcast %union.jvalue* %157 to %struct._jobject**
  store %struct._jobject* %154, %struct._jobject** %158, align 8
  br label %159

159:                                              ; preds = %148, %133, %119, %108, %98, %86, %75, %64, %53, %48
  %160 = load i32, i32* %13, align 4
  %161 = add nsw i32 %160, 1
  store i32 %161, i32* %13, align 4
  br label %44, !llvm.loop !27

162:                                              ; preds = %44
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %164 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %163, align 4
  %165 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %164, i32 0, i32 81
  %166 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %165, align 4
  %167 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %168 = load %struct._jobject*, %struct._jobject** %7, align 4
  %169 = load %struct._jobject*, %struct._jobject** %8, align 4
  %170 = load %struct._jmethodID*, %struct._jmethodID** %9, align 4
  %171 = load %union.jvalue*, %union.jvalue** %10, align 4
  %172 = call i32 %166(%struct.JNINativeInterface_** noundef %167, %struct._jobject* noundef %168, %struct._jobject* noundef %169, %struct._jmethodID* noundef %170, %union.jvalue* noundef %171) #2
  call void @llvm.stackrestore(i8* %28)
  store i32 %172, i32* %18, align 4
  %173 = bitcast %struct.__va_list* %19 to i8*
  call void @llvm.va_end(i8* %173)
  %174 = load i32, i32* %18, align 4
  ret i32 %174
}

; Function Attrs: alwaysinline nounwind
define dso_local i32 @JNI_CallNonvirtualIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca %union.jvalue*, align 4
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %6, i32 0, i32 0
  %16 = bitcast i8** %15 to [1 x i32]*
  store [1 x i32] %4, [1 x i32]* %16, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %7, align 4
  store %struct._jobject* %1, %struct._jobject** %8, align 4
  store %struct._jobject* %2, %struct._jobject** %9, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %10, align 4
  br label %17

17:                                               ; preds = %5
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %19 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %18, align 4
  %20 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %19, i32 0, i32 0
  %21 = load i8*, i8** %20, align 4
  %22 = bitcast i8* %21 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %24 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i32 0, i32 0
  %26 = call i32 %22(%struct.JNINativeInterface_** noundef %23, %struct._jmethodID* noundef %24, i8* noundef %25)
  store i32 %26, i32* %13, align 4
  %27 = load i32, i32* %13, align 4
  %28 = mul i32 %27, 8
  %29 = alloca i8, i32 %28, align 8
  %30 = bitcast i8* %29 to %union.jvalue*
  store %union.jvalue* %30, %union.jvalue** %11, align 4
  store i32 0, i32* %14, align 4
  br label %31

31:                                               ; preds = %147, %17
  %32 = load i32, i32* %14, align 4
  %33 = load i32, i32* %13, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %150

35:                                               ; preds = %31
  %36 = load i32, i32* %14, align 4
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i32 0, i32 %36
  %38 = load i8, i8* %37, align 1
  %39 = zext i8 %38 to i32
  switch i32 %39, label %146 [
    i32 90, label %40
    i32 66, label %51
    i32 83, label %62
    i32 67, label %73
    i32 73, label %85
    i32 74, label %95
    i32 68, label %106
    i32 70, label %120
    i32 76, label %135
  ]

40:                                               ; preds = %35
  %41 = bitcast %struct.__va_list* %6 to i8**
  %42 = load i8*, i8** %41, align 4
  %43 = getelementptr inbounds i8, i8* %42, i32 4
  store i8* %43, i8** %41, align 4
  %44 = bitcast i8* %42 to i32*
  %45 = load i32, i32* %44, align 4
  %46 = trunc i32 %45 to i8
  %47 = load %union.jvalue*, %union.jvalue** %11, align 4
  %48 = load i32, i32* %14, align 4
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %47, i32 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %146

51:                                               ; preds = %35
  %52 = bitcast %struct.__va_list* %6 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %11, align 4
  %59 = load i32, i32* %14, align 4
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i32 %59
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %57, i8* %61, align 8
  br label %146

62:                                               ; preds = %35
  %63 = bitcast %struct.__va_list* %6 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load %union.jvalue*, %union.jvalue** %11, align 4
  %70 = load i32, i32* %14, align 4
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %69, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %68, i16* %72, align 8
  br label %146

73:                                               ; preds = %35
  %74 = bitcast %struct.__va_list* %6 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = trunc i32 %78 to i16
  %80 = zext i16 %79 to i32
  %81 = load %union.jvalue*, %union.jvalue** %11, align 4
  %82 = load i32, i32* %14, align 4
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %81, i32 %82
  %84 = bitcast %union.jvalue* %83 to i32*
  store i32 %80, i32* %84, align 8
  br label %146

85:                                               ; preds = %35
  %86 = bitcast %struct.__va_list* %6 to i8**
  %87 = load i8*, i8** %86, align 4
  %88 = getelementptr inbounds i8, i8* %87, i32 4
  store i8* %88, i8** %86, align 4
  %89 = bitcast i8* %87 to i32*
  %90 = load i32, i32* %89, align 4
  %91 = load %union.jvalue*, %union.jvalue** %11, align 4
  %92 = load i32, i32* %14, align 4
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %91, i32 %92
  %94 = bitcast %union.jvalue* %93 to i32*
  store i32 %90, i32* %94, align 8
  br label %146

95:                                               ; preds = %35
  %96 = bitcast %struct.__va_list* %6 to i8**
  %97 = load i8*, i8** %96, align 4
  %98 = getelementptr inbounds i8, i8* %97, i32 4
  store i8* %98, i8** %96, align 4
  %99 = bitcast i8* %97 to i32*
  %100 = load i32, i32* %99, align 4
  %101 = sext i32 %100 to i64
  %102 = load %union.jvalue*, %union.jvalue** %11, align 4
  %103 = load i32, i32* %14, align 4
  %104 = getelementptr inbounds %union.jvalue, %union.jvalue* %102, i32 %103
  %105 = bitcast %union.jvalue* %104 to i64*
  store i64 %101, i64* %105, align 8
  br label %146

106:                                              ; preds = %35
  %107 = bitcast %struct.__va_list* %6 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load %union.jvalue*, %union.jvalue** %11, align 4
  %117 = load i32, i32* %14, align 4
  %118 = getelementptr inbounds %union.jvalue, %union.jvalue* %116, i32 %117
  %119 = bitcast %union.jvalue* %118 to double*
  store double %115, double* %119, align 8
  br label %146

120:                                              ; preds = %35
  %121 = bitcast %struct.__va_list* %6 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = ptrtoint i8* %122 to i32
  %124 = add i32 %123, 7
  %125 = and i32 %124, -8
  %126 = inttoptr i32 %125 to i8*
  %127 = getelementptr inbounds i8, i8* %126, i32 8
  store i8* %127, i8** %121, align 4
  %128 = bitcast i8* %126 to double*
  %129 = load double, double* %128, align 8
  %130 = fptrunc double %129 to float
  %131 = load %union.jvalue*, %union.jvalue** %11, align 4
  %132 = load i32, i32* %14, align 4
  %133 = getelementptr inbounds %union.jvalue, %union.jvalue* %131, i32 %132
  %134 = bitcast %union.jvalue* %133 to float*
  store float %130, float* %134, align 8
  br label %146

135:                                              ; preds = %35
  %136 = bitcast %struct.__va_list* %6 to i8**
  %137 = load i8*, i8** %136, align 4
  %138 = getelementptr inbounds i8, i8* %137, i32 4
  store i8* %138, i8** %136, align 4
  %139 = bitcast i8* %137 to i8**
  %140 = load i8*, i8** %139, align 4
  %141 = bitcast i8* %140 to %struct._jobject*
  %142 = load %union.jvalue*, %union.jvalue** %11, align 4
  %143 = load i32, i32* %14, align 4
  %144 = getelementptr inbounds %union.jvalue, %union.jvalue* %142, i32 %143
  %145 = bitcast %union.jvalue* %144 to %struct._jobject**
  store %struct._jobject* %141, %struct._jobject** %145, align 8
  br label %146

146:                                              ; preds = %35, %135, %120, %106, %95, %85, %73, %62, %51, %40
  br label %147

147:                                              ; preds = %146
  %148 = load i32, i32* %14, align 4
  %149 = add nsw i32 %148, 1
  store i32 %149, i32* %14, align 4
  br label %31, !llvm.loop !27

150:                                              ; preds = %31
  br label %151

151:                                              ; preds = %150
  %152 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %153 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %152, align 4
  %154 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %153, i32 0, i32 81
  %155 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %154, align 4
  %156 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %157 = load %struct._jobject*, %struct._jobject** %8, align 4
  %158 = load %struct._jobject*, %struct._jobject** %9, align 4
  %159 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %160 = load %union.jvalue*, %union.jvalue** %11, align 4
  %161 = call i32 %155(%struct.JNINativeInterface_** noundef %156, %struct._jobject* noundef %157, %struct._jobject* noundef %158, %struct._jmethodID* noundef %159, %union.jvalue* noundef %160)
  ret i32 %161
}

; Function Attrs: alwaysinline nounwind
define dso_local i32 @JNI_CallStaticIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca i32, align 4
  %16 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %17 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_start(i8* %17)
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %19 = load %struct._jobject*, %struct._jobject** %13, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %21 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %16, i32 0, i32 0
  %22 = bitcast i8** %21 to [1 x i32]*
  %23 = load [1 x i32], [1 x i32]* %22, align 4
  %24 = call i8* @llvm.stacksave()
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  store [1 x i32] %23, [1 x i32]* %26, align 4
  store %struct.JNINativeInterface_** %18, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %19, %struct._jobject** %6, align 4
  store %struct._jmethodID* %20, %struct._jmethodID** %7, align 4
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %28 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %27, align 4
  %29 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %28, i32 0, i32 0
  %30 = load i8*, i8** %29, align 4
  %31 = bitcast i8* %30 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %32 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %33 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %35 = call i32 %31(%struct.JNINativeInterface_** noundef %32, %struct._jmethodID* noundef %33, i8* noundef %34) #2
  store i32 %35, i32* %10, align 4
  %36 = load i32, i32* %10, align 4
  %37 = mul i32 %36, 8
  %38 = alloca i8, i32 %37, align 8
  %39 = bitcast i8* %38 to %union.jvalue*
  store %union.jvalue* %39, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %40

40:                                               ; preds = %155, %3
  %41 = load i32, i32* %11, align 4
  %42 = load i32, i32* %10, align 4
  %43 = icmp slt i32 %41, %42
  br i1 %43, label %44, label %158

44:                                               ; preds = %40
  %45 = load i32, i32* %11, align 4
  %46 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  switch i32 %48, label %155 [
    i32 90, label %49
    i32 66, label %60
    i32 83, label %71
    i32 67, label %82
    i32 73, label %94
    i32 74, label %104
    i32 68, label %115
    i32 70, label %129
    i32 76, label %144
  ]

49:                                               ; preds = %44
  %50 = bitcast %struct.__va_list* %4 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %8, align 4
  %57 = load i32, i32* %11, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %155

60:                                               ; preds = %44
  %61 = bitcast %struct.__va_list* %4 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i8
  %67 = load %union.jvalue*, %union.jvalue** %8, align 4
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i8*
  store i8 %66, i8* %70, align 8
  br label %155

71:                                               ; preds = %44
  %72 = bitcast %struct.__va_list* %4 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = load %union.jvalue*, %union.jvalue** %8, align 4
  %79 = load i32, i32* %11, align 4
  %80 = getelementptr inbounds %union.jvalue, %union.jvalue* %78, i32 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %155

82:                                               ; preds = %44
  %83 = bitcast %struct.__va_list* %4 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = zext i16 %88 to i32
  %90 = load %union.jvalue*, %union.jvalue** %8, align 4
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds %union.jvalue, %union.jvalue* %90, i32 %91
  %93 = bitcast %union.jvalue* %92 to i32*
  store i32 %89, i32* %93, align 8
  br label %155

94:                                               ; preds = %44
  %95 = bitcast %struct.__va_list* %4 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = getelementptr inbounds i8, i8* %96, i32 4
  store i8* %97, i8** %95, align 4
  %98 = bitcast i8* %96 to i32*
  %99 = load i32, i32* %98, align 4
  %100 = load %union.jvalue*, %union.jvalue** %8, align 4
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i32*
  store i32 %99, i32* %103, align 8
  br label %155

104:                                              ; preds = %44
  %105 = bitcast %struct.__va_list* %4 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = getelementptr inbounds i8, i8* %106, i32 4
  store i8* %107, i8** %105, align 4
  %108 = bitcast i8* %106 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = sext i32 %109 to i64
  %111 = load %union.jvalue*, %union.jvalue** %8, align 4
  %112 = load i32, i32* %11, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %111, i32 %112
  %114 = bitcast %union.jvalue* %113 to i64*
  store i64 %110, i64* %114, align 8
  br label %155

115:                                              ; preds = %44
  %116 = bitcast %struct.__va_list* %4 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = ptrtoint i8* %117 to i32
  %119 = add i32 %118, 7
  %120 = and i32 %119, -8
  %121 = inttoptr i32 %120 to i8*
  %122 = getelementptr inbounds i8, i8* %121, i32 8
  store i8* %122, i8** %116, align 4
  %123 = bitcast i8* %121 to double*
  %124 = load double, double* %123, align 8
  %125 = load %union.jvalue*, %union.jvalue** %8, align 4
  %126 = load i32, i32* %11, align 4
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %125, i32 %126
  %128 = bitcast %union.jvalue* %127 to double*
  store double %124, double* %128, align 8
  br label %155

129:                                              ; preds = %44
  %130 = bitcast %struct.__va_list* %4 to i8**
  %131 = load i8*, i8** %130, align 4
  %132 = ptrtoint i8* %131 to i32
  %133 = add i32 %132, 7
  %134 = and i32 %133, -8
  %135 = inttoptr i32 %134 to i8*
  %136 = getelementptr inbounds i8, i8* %135, i32 8
  store i8* %136, i8** %130, align 4
  %137 = bitcast i8* %135 to double*
  %138 = load double, double* %137, align 8
  %139 = fptrunc double %138 to float
  %140 = load %union.jvalue*, %union.jvalue** %8, align 4
  %141 = load i32, i32* %11, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %139, float* %143, align 8
  br label %155

144:                                              ; preds = %44
  %145 = bitcast %struct.__va_list* %4 to i8**
  %146 = load i8*, i8** %145, align 4
  %147 = getelementptr inbounds i8, i8* %146, i32 4
  store i8* %147, i8** %145, align 4
  %148 = bitcast i8* %146 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = bitcast i8* %149 to %struct._jobject*
  %151 = load %union.jvalue*, %union.jvalue** %8, align 4
  %152 = load i32, i32* %11, align 4
  %153 = getelementptr inbounds %union.jvalue, %union.jvalue* %151, i32 %152
  %154 = bitcast %union.jvalue* %153 to %struct._jobject**
  store %struct._jobject* %150, %struct._jobject** %154, align 8
  br label %155

155:                                              ; preds = %144, %129, %115, %104, %94, %82, %71, %60, %49, %44
  %156 = load i32, i32* %11, align 4
  %157 = add nsw i32 %156, 1
  store i32 %157, i32* %11, align 4
  br label %40, !llvm.loop !28

158:                                              ; preds = %40
  %159 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %160 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %159, align 4
  %161 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %160, i32 0, i32 131
  %162 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %161, align 4
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %164 = load %struct._jobject*, %struct._jobject** %6, align 4
  %165 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %166 = load %union.jvalue*, %union.jvalue** %8, align 4
  %167 = call i32 %162(%struct.JNINativeInterface_** noundef %163, %struct._jobject* noundef %164, %struct._jmethodID* noundef %165, %union.jvalue* noundef %166) #2
  call void @llvm.stackrestore(i8* %24)
  store i32 %167, i32* %15, align 4
  %168 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_end(i8* %168)
  %169 = load i32, i32* %15, align 4
  ret i32 %169
}

; Function Attrs: alwaysinline nounwind
define dso_local i32 @JNI_CallStaticIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !28

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 131
  %153 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  %158 = call i32 %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret i32 %158
}

; Function Attrs: alwaysinline nounwind
define dso_local i64 @JNI_CallLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca i64, align 8
  %16 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %17 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_start(i8* %17)
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %19 = load %struct._jobject*, %struct._jobject** %13, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %21 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %16, i32 0, i32 0
  %22 = bitcast i8** %21 to [1 x i32]*
  %23 = load [1 x i32], [1 x i32]* %22, align 4
  %24 = call i8* @llvm.stacksave()
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  store [1 x i32] %23, [1 x i32]* %26, align 4
  store %struct.JNINativeInterface_** %18, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %19, %struct._jobject** %6, align 4
  store %struct._jmethodID* %20, %struct._jmethodID** %7, align 4
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %28 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %27, align 4
  %29 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %28, i32 0, i32 0
  %30 = load i8*, i8** %29, align 4
  %31 = bitcast i8* %30 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %32 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %33 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %35 = call i32 %31(%struct.JNINativeInterface_** noundef %32, %struct._jmethodID* noundef %33, i8* noundef %34) #2
  store i32 %35, i32* %10, align 4
  %36 = load i32, i32* %10, align 4
  %37 = mul i32 %36, 8
  %38 = alloca i8, i32 %37, align 8
  %39 = bitcast i8* %38 to %union.jvalue*
  store %union.jvalue* %39, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %40

40:                                               ; preds = %155, %3
  %41 = load i32, i32* %11, align 4
  %42 = load i32, i32* %10, align 4
  %43 = icmp slt i32 %41, %42
  br i1 %43, label %44, label %158

44:                                               ; preds = %40
  %45 = load i32, i32* %11, align 4
  %46 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  switch i32 %48, label %155 [
    i32 90, label %49
    i32 66, label %60
    i32 83, label %71
    i32 67, label %82
    i32 73, label %94
    i32 74, label %104
    i32 68, label %115
    i32 70, label %129
    i32 76, label %144
  ]

49:                                               ; preds = %44
  %50 = bitcast %struct.__va_list* %4 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %8, align 4
  %57 = load i32, i32* %11, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %155

60:                                               ; preds = %44
  %61 = bitcast %struct.__va_list* %4 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i8
  %67 = load %union.jvalue*, %union.jvalue** %8, align 4
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i8*
  store i8 %66, i8* %70, align 8
  br label %155

71:                                               ; preds = %44
  %72 = bitcast %struct.__va_list* %4 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = load %union.jvalue*, %union.jvalue** %8, align 4
  %79 = load i32, i32* %11, align 4
  %80 = getelementptr inbounds %union.jvalue, %union.jvalue* %78, i32 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %155

82:                                               ; preds = %44
  %83 = bitcast %struct.__va_list* %4 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = zext i16 %88 to i32
  %90 = load %union.jvalue*, %union.jvalue** %8, align 4
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds %union.jvalue, %union.jvalue* %90, i32 %91
  %93 = bitcast %union.jvalue* %92 to i32*
  store i32 %89, i32* %93, align 8
  br label %155

94:                                               ; preds = %44
  %95 = bitcast %struct.__va_list* %4 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = getelementptr inbounds i8, i8* %96, i32 4
  store i8* %97, i8** %95, align 4
  %98 = bitcast i8* %96 to i32*
  %99 = load i32, i32* %98, align 4
  %100 = load %union.jvalue*, %union.jvalue** %8, align 4
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i32*
  store i32 %99, i32* %103, align 8
  br label %155

104:                                              ; preds = %44
  %105 = bitcast %struct.__va_list* %4 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = getelementptr inbounds i8, i8* %106, i32 4
  store i8* %107, i8** %105, align 4
  %108 = bitcast i8* %106 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = sext i32 %109 to i64
  %111 = load %union.jvalue*, %union.jvalue** %8, align 4
  %112 = load i32, i32* %11, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %111, i32 %112
  %114 = bitcast %union.jvalue* %113 to i64*
  store i64 %110, i64* %114, align 8
  br label %155

115:                                              ; preds = %44
  %116 = bitcast %struct.__va_list* %4 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = ptrtoint i8* %117 to i32
  %119 = add i32 %118, 7
  %120 = and i32 %119, -8
  %121 = inttoptr i32 %120 to i8*
  %122 = getelementptr inbounds i8, i8* %121, i32 8
  store i8* %122, i8** %116, align 4
  %123 = bitcast i8* %121 to double*
  %124 = load double, double* %123, align 8
  %125 = load %union.jvalue*, %union.jvalue** %8, align 4
  %126 = load i32, i32* %11, align 4
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %125, i32 %126
  %128 = bitcast %union.jvalue* %127 to double*
  store double %124, double* %128, align 8
  br label %155

129:                                              ; preds = %44
  %130 = bitcast %struct.__va_list* %4 to i8**
  %131 = load i8*, i8** %130, align 4
  %132 = ptrtoint i8* %131 to i32
  %133 = add i32 %132, 7
  %134 = and i32 %133, -8
  %135 = inttoptr i32 %134 to i8*
  %136 = getelementptr inbounds i8, i8* %135, i32 8
  store i8* %136, i8** %130, align 4
  %137 = bitcast i8* %135 to double*
  %138 = load double, double* %137, align 8
  %139 = fptrunc double %138 to float
  %140 = load %union.jvalue*, %union.jvalue** %8, align 4
  %141 = load i32, i32* %11, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %139, float* %143, align 8
  br label %155

144:                                              ; preds = %44
  %145 = bitcast %struct.__va_list* %4 to i8**
  %146 = load i8*, i8** %145, align 4
  %147 = getelementptr inbounds i8, i8* %146, i32 4
  store i8* %147, i8** %145, align 4
  %148 = bitcast i8* %146 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = bitcast i8* %149 to %struct._jobject*
  %151 = load %union.jvalue*, %union.jvalue** %8, align 4
  %152 = load i32, i32* %11, align 4
  %153 = getelementptr inbounds %union.jvalue, %union.jvalue* %151, i32 %152
  %154 = bitcast %union.jvalue* %153 to %struct._jobject**
  store %struct._jobject* %150, %struct._jobject** %154, align 8
  br label %155

155:                                              ; preds = %144, %129, %115, %104, %94, %82, %71, %60, %49, %44
  %156 = load i32, i32* %11, align 4
  %157 = add nsw i32 %156, 1
  store i32 %157, i32* %11, align 4
  br label %40, !llvm.loop !29

158:                                              ; preds = %40
  %159 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %160 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %159, align 4
  %161 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %160, i32 0, i32 54
  %162 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %161, align 4
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %164 = load %struct._jobject*, %struct._jobject** %6, align 4
  %165 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %166 = load %union.jvalue*, %union.jvalue** %8, align 4
  %167 = call i64 %162(%struct.JNINativeInterface_** noundef %163, %struct._jobject* noundef %164, %struct._jmethodID* noundef %165, %union.jvalue* noundef %166) #2
  call void @llvm.stackrestore(i8* %24)
  store i64 %167, i64* %15, align 8
  %168 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_end(i8* %168)
  %169 = load i64, i64* %15, align 8
  ret i64 %169
}

; Function Attrs: alwaysinline nounwind
define dso_local i64 @JNI_CallLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !29

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 54
  %153 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  %158 = call i64 %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret i64 %158
}

; Function Attrs: alwaysinline nounwind
define dso_local i64 @JNI_CallNonvirtualLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jmethodID*, align 4
  %10 = alloca %union.jvalue*, align 4
  %11 = alloca [257 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca %struct.JNINativeInterface_**, align 4
  %15 = alloca %struct._jobject*, align 4
  %16 = alloca %struct._jobject*, align 4
  %17 = alloca %struct._jmethodID*, align 4
  %18 = alloca i64, align 8
  %19 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %14, align 4
  store %struct._jobject* %1, %struct._jobject** %15, align 4
  store %struct._jobject* %2, %struct._jobject** %16, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %17, align 4
  %20 = bitcast %struct.__va_list* %19 to i8*
  call void @llvm.va_start(i8* %20)
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %14, align 4
  %22 = load %struct._jobject*, %struct._jobject** %15, align 4
  %23 = load %struct._jobject*, %struct._jobject** %16, align 4
  %24 = load %struct._jmethodID*, %struct._jmethodID** %17, align 4
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %19, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  %27 = load [1 x i32], [1 x i32]* %26, align 4
  %28 = call i8* @llvm.stacksave()
  %29 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %30 = bitcast i8** %29 to [1 x i32]*
  store [1 x i32] %27, [1 x i32]* %30, align 4
  store %struct.JNINativeInterface_** %21, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %22, %struct._jobject** %7, align 4
  store %struct._jobject* %23, %struct._jobject** %8, align 4
  store %struct._jmethodID* %24, %struct._jmethodID** %9, align 4
  %31 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %32 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %31, align 4
  %33 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %32, i32 0, i32 0
  %34 = load i8*, i8** %33, align 4
  %35 = bitcast i8* %34 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %36 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %37 = load %struct._jmethodID*, %struct._jmethodID** %9, align 4
  %38 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i32 0, i32 0
  %39 = call i32 %35(%struct.JNINativeInterface_** noundef %36, %struct._jmethodID* noundef %37, i8* noundef %38) #2
  store i32 %39, i32* %12, align 4
  %40 = load i32, i32* %12, align 4
  %41 = mul i32 %40, 8
  %42 = alloca i8, i32 %41, align 8
  %43 = bitcast i8* %42 to %union.jvalue*
  store %union.jvalue* %43, %union.jvalue** %10, align 4
  store i32 0, i32* %13, align 4
  br label %44

44:                                               ; preds = %159, %4
  %45 = load i32, i32* %13, align 4
  %46 = load i32, i32* %12, align 4
  %47 = icmp slt i32 %45, %46
  br i1 %47, label %48, label %162

48:                                               ; preds = %44
  %49 = load i32, i32* %13, align 4
  %50 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i32 0, i32 %49
  %51 = load i8, i8* %50, align 1
  %52 = zext i8 %51 to i32
  switch i32 %52, label %159 [
    i32 90, label %53
    i32 66, label %64
    i32 83, label %75
    i32 67, label %86
    i32 73, label %98
    i32 74, label %108
    i32 68, label %119
    i32 70, label %133
    i32 76, label %148
  ]

53:                                               ; preds = %48
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %10, align 4
  %61 = load i32, i32* %13, align 4
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i32 %61
  %63 = bitcast %union.jvalue* %62 to i8*
  store i8 %59, i8* %63, align 8
  br label %159

64:                                               ; preds = %48
  %65 = bitcast %struct.__va_list* %5 to i8**
  %66 = load i8*, i8** %65, align 4
  %67 = getelementptr inbounds i8, i8* %66, i32 4
  store i8* %67, i8** %65, align 4
  %68 = bitcast i8* %66 to i32*
  %69 = load i32, i32* %68, align 4
  %70 = trunc i32 %69 to i8
  %71 = load %union.jvalue*, %union.jvalue** %10, align 4
  %72 = load i32, i32* %13, align 4
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %71, i32 %72
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %70, i8* %74, align 8
  br label %159

75:                                               ; preds = %48
  %76 = bitcast %struct.__va_list* %5 to i8**
  %77 = load i8*, i8** %76, align 4
  %78 = getelementptr inbounds i8, i8* %77, i32 4
  store i8* %78, i8** %76, align 4
  %79 = bitcast i8* %77 to i32*
  %80 = load i32, i32* %79, align 4
  %81 = trunc i32 %80 to i16
  %82 = load %union.jvalue*, %union.jvalue** %10, align 4
  %83 = load i32, i32* %13, align 4
  %84 = getelementptr inbounds %union.jvalue, %union.jvalue* %82, i32 %83
  %85 = bitcast %union.jvalue* %84 to i16*
  store i16 %81, i16* %85, align 8
  br label %159

86:                                               ; preds = %48
  %87 = bitcast %struct.__va_list* %5 to i8**
  %88 = load i8*, i8** %87, align 4
  %89 = getelementptr inbounds i8, i8* %88, i32 4
  store i8* %89, i8** %87, align 4
  %90 = bitcast i8* %88 to i32*
  %91 = load i32, i32* %90, align 4
  %92 = trunc i32 %91 to i16
  %93 = zext i16 %92 to i32
  %94 = load %union.jvalue*, %union.jvalue** %10, align 4
  %95 = load i32, i32* %13, align 4
  %96 = getelementptr inbounds %union.jvalue, %union.jvalue* %94, i32 %95
  %97 = bitcast %union.jvalue* %96 to i32*
  store i32 %93, i32* %97, align 8
  br label %159

98:                                               ; preds = %48
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load %union.jvalue*, %union.jvalue** %10, align 4
  %105 = load i32, i32* %13, align 4
  %106 = getelementptr inbounds %union.jvalue, %union.jvalue* %104, i32 %105
  %107 = bitcast %union.jvalue* %106 to i32*
  store i32 %103, i32* %107, align 8
  br label %159

108:                                              ; preds = %48
  %109 = bitcast %struct.__va_list* %5 to i8**
  %110 = load i8*, i8** %109, align 4
  %111 = getelementptr inbounds i8, i8* %110, i32 4
  store i8* %111, i8** %109, align 4
  %112 = bitcast i8* %110 to i32*
  %113 = load i32, i32* %112, align 4
  %114 = sext i32 %113 to i64
  %115 = load %union.jvalue*, %union.jvalue** %10, align 4
  %116 = load i32, i32* %13, align 4
  %117 = getelementptr inbounds %union.jvalue, %union.jvalue* %115, i32 %116
  %118 = bitcast %union.jvalue* %117 to i64*
  store i64 %114, i64* %118, align 8
  br label %159

119:                                              ; preds = %48
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = ptrtoint i8* %121 to i32
  %123 = add i32 %122, 7
  %124 = and i32 %123, -8
  %125 = inttoptr i32 %124 to i8*
  %126 = getelementptr inbounds i8, i8* %125, i32 8
  store i8* %126, i8** %120, align 4
  %127 = bitcast i8* %125 to double*
  %128 = load double, double* %127, align 8
  %129 = load %union.jvalue*, %union.jvalue** %10, align 4
  %130 = load i32, i32* %13, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to double*
  store double %128, double* %132, align 8
  br label %159

133:                                              ; preds = %48
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = ptrtoint i8* %135 to i32
  %137 = add i32 %136, 7
  %138 = and i32 %137, -8
  %139 = inttoptr i32 %138 to i8*
  %140 = getelementptr inbounds i8, i8* %139, i32 8
  store i8* %140, i8** %134, align 4
  %141 = bitcast i8* %139 to double*
  %142 = load double, double* %141, align 8
  %143 = fptrunc double %142 to float
  %144 = load %union.jvalue*, %union.jvalue** %10, align 4
  %145 = load i32, i32* %13, align 4
  %146 = getelementptr inbounds %union.jvalue, %union.jvalue* %144, i32 %145
  %147 = bitcast %union.jvalue* %146 to float*
  store float %143, float* %147, align 8
  br label %159

148:                                              ; preds = %48
  %149 = bitcast %struct.__va_list* %5 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = getelementptr inbounds i8, i8* %150, i32 4
  store i8* %151, i8** %149, align 4
  %152 = bitcast i8* %150 to i8**
  %153 = load i8*, i8** %152, align 4
  %154 = bitcast i8* %153 to %struct._jobject*
  %155 = load %union.jvalue*, %union.jvalue** %10, align 4
  %156 = load i32, i32* %13, align 4
  %157 = getelementptr inbounds %union.jvalue, %union.jvalue* %155, i32 %156
  %158 = bitcast %union.jvalue* %157 to %struct._jobject**
  store %struct._jobject* %154, %struct._jobject** %158, align 8
  br label %159

159:                                              ; preds = %148, %133, %119, %108, %98, %86, %75, %64, %53, %48
  %160 = load i32, i32* %13, align 4
  %161 = add nsw i32 %160, 1
  store i32 %161, i32* %13, align 4
  br label %44, !llvm.loop !30

162:                                              ; preds = %44
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %164 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %163, align 4
  %165 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %164, i32 0, i32 84
  %166 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %165, align 4
  %167 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %168 = load %struct._jobject*, %struct._jobject** %7, align 4
  %169 = load %struct._jobject*, %struct._jobject** %8, align 4
  %170 = load %struct._jmethodID*, %struct._jmethodID** %9, align 4
  %171 = load %union.jvalue*, %union.jvalue** %10, align 4
  %172 = call i64 %166(%struct.JNINativeInterface_** noundef %167, %struct._jobject* noundef %168, %struct._jobject* noundef %169, %struct._jmethodID* noundef %170, %union.jvalue* noundef %171) #2
  call void @llvm.stackrestore(i8* %28)
  store i64 %172, i64* %18, align 8
  %173 = bitcast %struct.__va_list* %19 to i8*
  call void @llvm.va_end(i8* %173)
  %174 = load i64, i64* %18, align 8
  ret i64 %174
}

; Function Attrs: alwaysinline nounwind
define dso_local i64 @JNI_CallNonvirtualLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca %union.jvalue*, align 4
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %6, i32 0, i32 0
  %16 = bitcast i8** %15 to [1 x i32]*
  store [1 x i32] %4, [1 x i32]* %16, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %7, align 4
  store %struct._jobject* %1, %struct._jobject** %8, align 4
  store %struct._jobject* %2, %struct._jobject** %9, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %10, align 4
  br label %17

17:                                               ; preds = %5
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %19 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %18, align 4
  %20 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %19, i32 0, i32 0
  %21 = load i8*, i8** %20, align 4
  %22 = bitcast i8* %21 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %24 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i32 0, i32 0
  %26 = call i32 %22(%struct.JNINativeInterface_** noundef %23, %struct._jmethodID* noundef %24, i8* noundef %25)
  store i32 %26, i32* %13, align 4
  %27 = load i32, i32* %13, align 4
  %28 = mul i32 %27, 8
  %29 = alloca i8, i32 %28, align 8
  %30 = bitcast i8* %29 to %union.jvalue*
  store %union.jvalue* %30, %union.jvalue** %11, align 4
  store i32 0, i32* %14, align 4
  br label %31

31:                                               ; preds = %147, %17
  %32 = load i32, i32* %14, align 4
  %33 = load i32, i32* %13, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %150

35:                                               ; preds = %31
  %36 = load i32, i32* %14, align 4
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i32 0, i32 %36
  %38 = load i8, i8* %37, align 1
  %39 = zext i8 %38 to i32
  switch i32 %39, label %146 [
    i32 90, label %40
    i32 66, label %51
    i32 83, label %62
    i32 67, label %73
    i32 73, label %85
    i32 74, label %95
    i32 68, label %106
    i32 70, label %120
    i32 76, label %135
  ]

40:                                               ; preds = %35
  %41 = bitcast %struct.__va_list* %6 to i8**
  %42 = load i8*, i8** %41, align 4
  %43 = getelementptr inbounds i8, i8* %42, i32 4
  store i8* %43, i8** %41, align 4
  %44 = bitcast i8* %42 to i32*
  %45 = load i32, i32* %44, align 4
  %46 = trunc i32 %45 to i8
  %47 = load %union.jvalue*, %union.jvalue** %11, align 4
  %48 = load i32, i32* %14, align 4
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %47, i32 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %146

51:                                               ; preds = %35
  %52 = bitcast %struct.__va_list* %6 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %11, align 4
  %59 = load i32, i32* %14, align 4
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i32 %59
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %57, i8* %61, align 8
  br label %146

62:                                               ; preds = %35
  %63 = bitcast %struct.__va_list* %6 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load %union.jvalue*, %union.jvalue** %11, align 4
  %70 = load i32, i32* %14, align 4
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %69, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %68, i16* %72, align 8
  br label %146

73:                                               ; preds = %35
  %74 = bitcast %struct.__va_list* %6 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = trunc i32 %78 to i16
  %80 = zext i16 %79 to i32
  %81 = load %union.jvalue*, %union.jvalue** %11, align 4
  %82 = load i32, i32* %14, align 4
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %81, i32 %82
  %84 = bitcast %union.jvalue* %83 to i32*
  store i32 %80, i32* %84, align 8
  br label %146

85:                                               ; preds = %35
  %86 = bitcast %struct.__va_list* %6 to i8**
  %87 = load i8*, i8** %86, align 4
  %88 = getelementptr inbounds i8, i8* %87, i32 4
  store i8* %88, i8** %86, align 4
  %89 = bitcast i8* %87 to i32*
  %90 = load i32, i32* %89, align 4
  %91 = load %union.jvalue*, %union.jvalue** %11, align 4
  %92 = load i32, i32* %14, align 4
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %91, i32 %92
  %94 = bitcast %union.jvalue* %93 to i32*
  store i32 %90, i32* %94, align 8
  br label %146

95:                                               ; preds = %35
  %96 = bitcast %struct.__va_list* %6 to i8**
  %97 = load i8*, i8** %96, align 4
  %98 = getelementptr inbounds i8, i8* %97, i32 4
  store i8* %98, i8** %96, align 4
  %99 = bitcast i8* %97 to i32*
  %100 = load i32, i32* %99, align 4
  %101 = sext i32 %100 to i64
  %102 = load %union.jvalue*, %union.jvalue** %11, align 4
  %103 = load i32, i32* %14, align 4
  %104 = getelementptr inbounds %union.jvalue, %union.jvalue* %102, i32 %103
  %105 = bitcast %union.jvalue* %104 to i64*
  store i64 %101, i64* %105, align 8
  br label %146

106:                                              ; preds = %35
  %107 = bitcast %struct.__va_list* %6 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load %union.jvalue*, %union.jvalue** %11, align 4
  %117 = load i32, i32* %14, align 4
  %118 = getelementptr inbounds %union.jvalue, %union.jvalue* %116, i32 %117
  %119 = bitcast %union.jvalue* %118 to double*
  store double %115, double* %119, align 8
  br label %146

120:                                              ; preds = %35
  %121 = bitcast %struct.__va_list* %6 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = ptrtoint i8* %122 to i32
  %124 = add i32 %123, 7
  %125 = and i32 %124, -8
  %126 = inttoptr i32 %125 to i8*
  %127 = getelementptr inbounds i8, i8* %126, i32 8
  store i8* %127, i8** %121, align 4
  %128 = bitcast i8* %126 to double*
  %129 = load double, double* %128, align 8
  %130 = fptrunc double %129 to float
  %131 = load %union.jvalue*, %union.jvalue** %11, align 4
  %132 = load i32, i32* %14, align 4
  %133 = getelementptr inbounds %union.jvalue, %union.jvalue* %131, i32 %132
  %134 = bitcast %union.jvalue* %133 to float*
  store float %130, float* %134, align 8
  br label %146

135:                                              ; preds = %35
  %136 = bitcast %struct.__va_list* %6 to i8**
  %137 = load i8*, i8** %136, align 4
  %138 = getelementptr inbounds i8, i8* %137, i32 4
  store i8* %138, i8** %136, align 4
  %139 = bitcast i8* %137 to i8**
  %140 = load i8*, i8** %139, align 4
  %141 = bitcast i8* %140 to %struct._jobject*
  %142 = load %union.jvalue*, %union.jvalue** %11, align 4
  %143 = load i32, i32* %14, align 4
  %144 = getelementptr inbounds %union.jvalue, %union.jvalue* %142, i32 %143
  %145 = bitcast %union.jvalue* %144 to %struct._jobject**
  store %struct._jobject* %141, %struct._jobject** %145, align 8
  br label %146

146:                                              ; preds = %35, %135, %120, %106, %95, %85, %73, %62, %51, %40
  br label %147

147:                                              ; preds = %146
  %148 = load i32, i32* %14, align 4
  %149 = add nsw i32 %148, 1
  store i32 %149, i32* %14, align 4
  br label %31, !llvm.loop !30

150:                                              ; preds = %31
  br label %151

151:                                              ; preds = %150
  %152 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %153 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %152, align 4
  %154 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %153, i32 0, i32 84
  %155 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %154, align 4
  %156 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %157 = load %struct._jobject*, %struct._jobject** %8, align 4
  %158 = load %struct._jobject*, %struct._jobject** %9, align 4
  %159 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %160 = load %union.jvalue*, %union.jvalue** %11, align 4
  %161 = call i64 %155(%struct.JNINativeInterface_** noundef %156, %struct._jobject* noundef %157, %struct._jobject* noundef %158, %struct._jmethodID* noundef %159, %union.jvalue* noundef %160)
  ret i64 %161
}

; Function Attrs: alwaysinline nounwind
define dso_local i64 @JNI_CallStaticLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca i64, align 8
  %16 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %17 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_start(i8* %17)
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %19 = load %struct._jobject*, %struct._jobject** %13, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %21 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %16, i32 0, i32 0
  %22 = bitcast i8** %21 to [1 x i32]*
  %23 = load [1 x i32], [1 x i32]* %22, align 4
  %24 = call i8* @llvm.stacksave()
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  store [1 x i32] %23, [1 x i32]* %26, align 4
  store %struct.JNINativeInterface_** %18, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %19, %struct._jobject** %6, align 4
  store %struct._jmethodID* %20, %struct._jmethodID** %7, align 4
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %28 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %27, align 4
  %29 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %28, i32 0, i32 0
  %30 = load i8*, i8** %29, align 4
  %31 = bitcast i8* %30 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %32 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %33 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %35 = call i32 %31(%struct.JNINativeInterface_** noundef %32, %struct._jmethodID* noundef %33, i8* noundef %34) #2
  store i32 %35, i32* %10, align 4
  %36 = load i32, i32* %10, align 4
  %37 = mul i32 %36, 8
  %38 = alloca i8, i32 %37, align 8
  %39 = bitcast i8* %38 to %union.jvalue*
  store %union.jvalue* %39, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %40

40:                                               ; preds = %155, %3
  %41 = load i32, i32* %11, align 4
  %42 = load i32, i32* %10, align 4
  %43 = icmp slt i32 %41, %42
  br i1 %43, label %44, label %158

44:                                               ; preds = %40
  %45 = load i32, i32* %11, align 4
  %46 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  switch i32 %48, label %155 [
    i32 90, label %49
    i32 66, label %60
    i32 83, label %71
    i32 67, label %82
    i32 73, label %94
    i32 74, label %104
    i32 68, label %115
    i32 70, label %129
    i32 76, label %144
  ]

49:                                               ; preds = %44
  %50 = bitcast %struct.__va_list* %4 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %8, align 4
  %57 = load i32, i32* %11, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %155

60:                                               ; preds = %44
  %61 = bitcast %struct.__va_list* %4 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i8
  %67 = load %union.jvalue*, %union.jvalue** %8, align 4
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i8*
  store i8 %66, i8* %70, align 8
  br label %155

71:                                               ; preds = %44
  %72 = bitcast %struct.__va_list* %4 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = load %union.jvalue*, %union.jvalue** %8, align 4
  %79 = load i32, i32* %11, align 4
  %80 = getelementptr inbounds %union.jvalue, %union.jvalue* %78, i32 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %155

82:                                               ; preds = %44
  %83 = bitcast %struct.__va_list* %4 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = zext i16 %88 to i32
  %90 = load %union.jvalue*, %union.jvalue** %8, align 4
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds %union.jvalue, %union.jvalue* %90, i32 %91
  %93 = bitcast %union.jvalue* %92 to i32*
  store i32 %89, i32* %93, align 8
  br label %155

94:                                               ; preds = %44
  %95 = bitcast %struct.__va_list* %4 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = getelementptr inbounds i8, i8* %96, i32 4
  store i8* %97, i8** %95, align 4
  %98 = bitcast i8* %96 to i32*
  %99 = load i32, i32* %98, align 4
  %100 = load %union.jvalue*, %union.jvalue** %8, align 4
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i32*
  store i32 %99, i32* %103, align 8
  br label %155

104:                                              ; preds = %44
  %105 = bitcast %struct.__va_list* %4 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = getelementptr inbounds i8, i8* %106, i32 4
  store i8* %107, i8** %105, align 4
  %108 = bitcast i8* %106 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = sext i32 %109 to i64
  %111 = load %union.jvalue*, %union.jvalue** %8, align 4
  %112 = load i32, i32* %11, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %111, i32 %112
  %114 = bitcast %union.jvalue* %113 to i64*
  store i64 %110, i64* %114, align 8
  br label %155

115:                                              ; preds = %44
  %116 = bitcast %struct.__va_list* %4 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = ptrtoint i8* %117 to i32
  %119 = add i32 %118, 7
  %120 = and i32 %119, -8
  %121 = inttoptr i32 %120 to i8*
  %122 = getelementptr inbounds i8, i8* %121, i32 8
  store i8* %122, i8** %116, align 4
  %123 = bitcast i8* %121 to double*
  %124 = load double, double* %123, align 8
  %125 = load %union.jvalue*, %union.jvalue** %8, align 4
  %126 = load i32, i32* %11, align 4
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %125, i32 %126
  %128 = bitcast %union.jvalue* %127 to double*
  store double %124, double* %128, align 8
  br label %155

129:                                              ; preds = %44
  %130 = bitcast %struct.__va_list* %4 to i8**
  %131 = load i8*, i8** %130, align 4
  %132 = ptrtoint i8* %131 to i32
  %133 = add i32 %132, 7
  %134 = and i32 %133, -8
  %135 = inttoptr i32 %134 to i8*
  %136 = getelementptr inbounds i8, i8* %135, i32 8
  store i8* %136, i8** %130, align 4
  %137 = bitcast i8* %135 to double*
  %138 = load double, double* %137, align 8
  %139 = fptrunc double %138 to float
  %140 = load %union.jvalue*, %union.jvalue** %8, align 4
  %141 = load i32, i32* %11, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %139, float* %143, align 8
  br label %155

144:                                              ; preds = %44
  %145 = bitcast %struct.__va_list* %4 to i8**
  %146 = load i8*, i8** %145, align 4
  %147 = getelementptr inbounds i8, i8* %146, i32 4
  store i8* %147, i8** %145, align 4
  %148 = bitcast i8* %146 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = bitcast i8* %149 to %struct._jobject*
  %151 = load %union.jvalue*, %union.jvalue** %8, align 4
  %152 = load i32, i32* %11, align 4
  %153 = getelementptr inbounds %union.jvalue, %union.jvalue* %151, i32 %152
  %154 = bitcast %union.jvalue* %153 to %struct._jobject**
  store %struct._jobject* %150, %struct._jobject** %154, align 8
  br label %155

155:                                              ; preds = %144, %129, %115, %104, %94, %82, %71, %60, %49, %44
  %156 = load i32, i32* %11, align 4
  %157 = add nsw i32 %156, 1
  store i32 %157, i32* %11, align 4
  br label %40, !llvm.loop !31

158:                                              ; preds = %40
  %159 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %160 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %159, align 4
  %161 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %160, i32 0, i32 134
  %162 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %161, align 4
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %164 = load %struct._jobject*, %struct._jobject** %6, align 4
  %165 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %166 = load %union.jvalue*, %union.jvalue** %8, align 4
  %167 = call i64 %162(%struct.JNINativeInterface_** noundef %163, %struct._jobject* noundef %164, %struct._jmethodID* noundef %165, %union.jvalue* noundef %166) #2
  call void @llvm.stackrestore(i8* %24)
  store i64 %167, i64* %15, align 8
  %168 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_end(i8* %168)
  %169 = load i64, i64* %15, align 8
  ret i64 %169
}

; Function Attrs: alwaysinline nounwind
define dso_local i64 @JNI_CallStaticLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !31

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 134
  %153 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  %158 = call i64 %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret i64 %158
}

; Function Attrs: alwaysinline nounwind
define dso_local float @JNI_CallFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca float, align 4
  %16 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %17 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_start(i8* %17)
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %19 = load %struct._jobject*, %struct._jobject** %13, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %21 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %16, i32 0, i32 0
  %22 = bitcast i8** %21 to [1 x i32]*
  %23 = load [1 x i32], [1 x i32]* %22, align 4
  %24 = call i8* @llvm.stacksave()
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  store [1 x i32] %23, [1 x i32]* %26, align 4
  store %struct.JNINativeInterface_** %18, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %19, %struct._jobject** %6, align 4
  store %struct._jmethodID* %20, %struct._jmethodID** %7, align 4
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %28 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %27, align 4
  %29 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %28, i32 0, i32 0
  %30 = load i8*, i8** %29, align 4
  %31 = bitcast i8* %30 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %32 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %33 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %35 = call i32 %31(%struct.JNINativeInterface_** noundef %32, %struct._jmethodID* noundef %33, i8* noundef %34) #2
  store i32 %35, i32* %10, align 4
  %36 = load i32, i32* %10, align 4
  %37 = mul i32 %36, 8
  %38 = alloca i8, i32 %37, align 8
  %39 = bitcast i8* %38 to %union.jvalue*
  store %union.jvalue* %39, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %40

40:                                               ; preds = %155, %3
  %41 = load i32, i32* %11, align 4
  %42 = load i32, i32* %10, align 4
  %43 = icmp slt i32 %41, %42
  br i1 %43, label %44, label %158

44:                                               ; preds = %40
  %45 = load i32, i32* %11, align 4
  %46 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  switch i32 %48, label %155 [
    i32 90, label %49
    i32 66, label %60
    i32 83, label %71
    i32 67, label %82
    i32 73, label %94
    i32 74, label %104
    i32 68, label %115
    i32 70, label %129
    i32 76, label %144
  ]

49:                                               ; preds = %44
  %50 = bitcast %struct.__va_list* %4 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %8, align 4
  %57 = load i32, i32* %11, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %155

60:                                               ; preds = %44
  %61 = bitcast %struct.__va_list* %4 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i8
  %67 = load %union.jvalue*, %union.jvalue** %8, align 4
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i8*
  store i8 %66, i8* %70, align 8
  br label %155

71:                                               ; preds = %44
  %72 = bitcast %struct.__va_list* %4 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = load %union.jvalue*, %union.jvalue** %8, align 4
  %79 = load i32, i32* %11, align 4
  %80 = getelementptr inbounds %union.jvalue, %union.jvalue* %78, i32 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %155

82:                                               ; preds = %44
  %83 = bitcast %struct.__va_list* %4 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = zext i16 %88 to i32
  %90 = load %union.jvalue*, %union.jvalue** %8, align 4
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds %union.jvalue, %union.jvalue* %90, i32 %91
  %93 = bitcast %union.jvalue* %92 to i32*
  store i32 %89, i32* %93, align 8
  br label %155

94:                                               ; preds = %44
  %95 = bitcast %struct.__va_list* %4 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = getelementptr inbounds i8, i8* %96, i32 4
  store i8* %97, i8** %95, align 4
  %98 = bitcast i8* %96 to i32*
  %99 = load i32, i32* %98, align 4
  %100 = load %union.jvalue*, %union.jvalue** %8, align 4
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i32*
  store i32 %99, i32* %103, align 8
  br label %155

104:                                              ; preds = %44
  %105 = bitcast %struct.__va_list* %4 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = getelementptr inbounds i8, i8* %106, i32 4
  store i8* %107, i8** %105, align 4
  %108 = bitcast i8* %106 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = sext i32 %109 to i64
  %111 = load %union.jvalue*, %union.jvalue** %8, align 4
  %112 = load i32, i32* %11, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %111, i32 %112
  %114 = bitcast %union.jvalue* %113 to i64*
  store i64 %110, i64* %114, align 8
  br label %155

115:                                              ; preds = %44
  %116 = bitcast %struct.__va_list* %4 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = ptrtoint i8* %117 to i32
  %119 = add i32 %118, 7
  %120 = and i32 %119, -8
  %121 = inttoptr i32 %120 to i8*
  %122 = getelementptr inbounds i8, i8* %121, i32 8
  store i8* %122, i8** %116, align 4
  %123 = bitcast i8* %121 to double*
  %124 = load double, double* %123, align 8
  %125 = load %union.jvalue*, %union.jvalue** %8, align 4
  %126 = load i32, i32* %11, align 4
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %125, i32 %126
  %128 = bitcast %union.jvalue* %127 to double*
  store double %124, double* %128, align 8
  br label %155

129:                                              ; preds = %44
  %130 = bitcast %struct.__va_list* %4 to i8**
  %131 = load i8*, i8** %130, align 4
  %132 = ptrtoint i8* %131 to i32
  %133 = add i32 %132, 7
  %134 = and i32 %133, -8
  %135 = inttoptr i32 %134 to i8*
  %136 = getelementptr inbounds i8, i8* %135, i32 8
  store i8* %136, i8** %130, align 4
  %137 = bitcast i8* %135 to double*
  %138 = load double, double* %137, align 8
  %139 = fptrunc double %138 to float
  %140 = load %union.jvalue*, %union.jvalue** %8, align 4
  %141 = load i32, i32* %11, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %139, float* %143, align 8
  br label %155

144:                                              ; preds = %44
  %145 = bitcast %struct.__va_list* %4 to i8**
  %146 = load i8*, i8** %145, align 4
  %147 = getelementptr inbounds i8, i8* %146, i32 4
  store i8* %147, i8** %145, align 4
  %148 = bitcast i8* %146 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = bitcast i8* %149 to %struct._jobject*
  %151 = load %union.jvalue*, %union.jvalue** %8, align 4
  %152 = load i32, i32* %11, align 4
  %153 = getelementptr inbounds %union.jvalue, %union.jvalue* %151, i32 %152
  %154 = bitcast %union.jvalue* %153 to %struct._jobject**
  store %struct._jobject* %150, %struct._jobject** %154, align 8
  br label %155

155:                                              ; preds = %144, %129, %115, %104, %94, %82, %71, %60, %49, %44
  %156 = load i32, i32* %11, align 4
  %157 = add nsw i32 %156, 1
  store i32 %157, i32* %11, align 4
  br label %40, !llvm.loop !32

158:                                              ; preds = %40
  %159 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %160 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %159, align 4
  %161 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %160, i32 0, i32 57
  %162 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %161, align 4
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %164 = load %struct._jobject*, %struct._jobject** %6, align 4
  %165 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %166 = load %union.jvalue*, %union.jvalue** %8, align 4
  %167 = call float %162(%struct.JNINativeInterface_** noundef %163, %struct._jobject* noundef %164, %struct._jmethodID* noundef %165, %union.jvalue* noundef %166) #2
  call void @llvm.stackrestore(i8* %24)
  store float %167, float* %15, align 4
  %168 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_end(i8* %168)
  %169 = load float, float* %15, align 4
  ret float %169
}

; Function Attrs: alwaysinline nounwind
define dso_local float @JNI_CallFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !32

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 57
  %153 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  %158 = call float %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret float %158
}

; Function Attrs: alwaysinline nounwind
define dso_local float @JNI_CallNonvirtualFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jmethodID*, align 4
  %10 = alloca %union.jvalue*, align 4
  %11 = alloca [257 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca %struct.JNINativeInterface_**, align 4
  %15 = alloca %struct._jobject*, align 4
  %16 = alloca %struct._jobject*, align 4
  %17 = alloca %struct._jmethodID*, align 4
  %18 = alloca float, align 4
  %19 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %14, align 4
  store %struct._jobject* %1, %struct._jobject** %15, align 4
  store %struct._jobject* %2, %struct._jobject** %16, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %17, align 4
  %20 = bitcast %struct.__va_list* %19 to i8*
  call void @llvm.va_start(i8* %20)
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %14, align 4
  %22 = load %struct._jobject*, %struct._jobject** %15, align 4
  %23 = load %struct._jobject*, %struct._jobject** %16, align 4
  %24 = load %struct._jmethodID*, %struct._jmethodID** %17, align 4
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %19, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  %27 = load [1 x i32], [1 x i32]* %26, align 4
  %28 = call i8* @llvm.stacksave()
  %29 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %30 = bitcast i8** %29 to [1 x i32]*
  store [1 x i32] %27, [1 x i32]* %30, align 4
  store %struct.JNINativeInterface_** %21, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %22, %struct._jobject** %7, align 4
  store %struct._jobject* %23, %struct._jobject** %8, align 4
  store %struct._jmethodID* %24, %struct._jmethodID** %9, align 4
  %31 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %32 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %31, align 4
  %33 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %32, i32 0, i32 0
  %34 = load i8*, i8** %33, align 4
  %35 = bitcast i8* %34 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %36 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %37 = load %struct._jmethodID*, %struct._jmethodID** %9, align 4
  %38 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i32 0, i32 0
  %39 = call i32 %35(%struct.JNINativeInterface_** noundef %36, %struct._jmethodID* noundef %37, i8* noundef %38) #2
  store i32 %39, i32* %12, align 4
  %40 = load i32, i32* %12, align 4
  %41 = mul i32 %40, 8
  %42 = alloca i8, i32 %41, align 8
  %43 = bitcast i8* %42 to %union.jvalue*
  store %union.jvalue* %43, %union.jvalue** %10, align 4
  store i32 0, i32* %13, align 4
  br label %44

44:                                               ; preds = %159, %4
  %45 = load i32, i32* %13, align 4
  %46 = load i32, i32* %12, align 4
  %47 = icmp slt i32 %45, %46
  br i1 %47, label %48, label %162

48:                                               ; preds = %44
  %49 = load i32, i32* %13, align 4
  %50 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i32 0, i32 %49
  %51 = load i8, i8* %50, align 1
  %52 = zext i8 %51 to i32
  switch i32 %52, label %159 [
    i32 90, label %53
    i32 66, label %64
    i32 83, label %75
    i32 67, label %86
    i32 73, label %98
    i32 74, label %108
    i32 68, label %119
    i32 70, label %133
    i32 76, label %148
  ]

53:                                               ; preds = %48
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %10, align 4
  %61 = load i32, i32* %13, align 4
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i32 %61
  %63 = bitcast %union.jvalue* %62 to i8*
  store i8 %59, i8* %63, align 8
  br label %159

64:                                               ; preds = %48
  %65 = bitcast %struct.__va_list* %5 to i8**
  %66 = load i8*, i8** %65, align 4
  %67 = getelementptr inbounds i8, i8* %66, i32 4
  store i8* %67, i8** %65, align 4
  %68 = bitcast i8* %66 to i32*
  %69 = load i32, i32* %68, align 4
  %70 = trunc i32 %69 to i8
  %71 = load %union.jvalue*, %union.jvalue** %10, align 4
  %72 = load i32, i32* %13, align 4
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %71, i32 %72
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %70, i8* %74, align 8
  br label %159

75:                                               ; preds = %48
  %76 = bitcast %struct.__va_list* %5 to i8**
  %77 = load i8*, i8** %76, align 4
  %78 = getelementptr inbounds i8, i8* %77, i32 4
  store i8* %78, i8** %76, align 4
  %79 = bitcast i8* %77 to i32*
  %80 = load i32, i32* %79, align 4
  %81 = trunc i32 %80 to i16
  %82 = load %union.jvalue*, %union.jvalue** %10, align 4
  %83 = load i32, i32* %13, align 4
  %84 = getelementptr inbounds %union.jvalue, %union.jvalue* %82, i32 %83
  %85 = bitcast %union.jvalue* %84 to i16*
  store i16 %81, i16* %85, align 8
  br label %159

86:                                               ; preds = %48
  %87 = bitcast %struct.__va_list* %5 to i8**
  %88 = load i8*, i8** %87, align 4
  %89 = getelementptr inbounds i8, i8* %88, i32 4
  store i8* %89, i8** %87, align 4
  %90 = bitcast i8* %88 to i32*
  %91 = load i32, i32* %90, align 4
  %92 = trunc i32 %91 to i16
  %93 = zext i16 %92 to i32
  %94 = load %union.jvalue*, %union.jvalue** %10, align 4
  %95 = load i32, i32* %13, align 4
  %96 = getelementptr inbounds %union.jvalue, %union.jvalue* %94, i32 %95
  %97 = bitcast %union.jvalue* %96 to i32*
  store i32 %93, i32* %97, align 8
  br label %159

98:                                               ; preds = %48
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load %union.jvalue*, %union.jvalue** %10, align 4
  %105 = load i32, i32* %13, align 4
  %106 = getelementptr inbounds %union.jvalue, %union.jvalue* %104, i32 %105
  %107 = bitcast %union.jvalue* %106 to i32*
  store i32 %103, i32* %107, align 8
  br label %159

108:                                              ; preds = %48
  %109 = bitcast %struct.__va_list* %5 to i8**
  %110 = load i8*, i8** %109, align 4
  %111 = getelementptr inbounds i8, i8* %110, i32 4
  store i8* %111, i8** %109, align 4
  %112 = bitcast i8* %110 to i32*
  %113 = load i32, i32* %112, align 4
  %114 = sext i32 %113 to i64
  %115 = load %union.jvalue*, %union.jvalue** %10, align 4
  %116 = load i32, i32* %13, align 4
  %117 = getelementptr inbounds %union.jvalue, %union.jvalue* %115, i32 %116
  %118 = bitcast %union.jvalue* %117 to i64*
  store i64 %114, i64* %118, align 8
  br label %159

119:                                              ; preds = %48
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = ptrtoint i8* %121 to i32
  %123 = add i32 %122, 7
  %124 = and i32 %123, -8
  %125 = inttoptr i32 %124 to i8*
  %126 = getelementptr inbounds i8, i8* %125, i32 8
  store i8* %126, i8** %120, align 4
  %127 = bitcast i8* %125 to double*
  %128 = load double, double* %127, align 8
  %129 = load %union.jvalue*, %union.jvalue** %10, align 4
  %130 = load i32, i32* %13, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to double*
  store double %128, double* %132, align 8
  br label %159

133:                                              ; preds = %48
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = ptrtoint i8* %135 to i32
  %137 = add i32 %136, 7
  %138 = and i32 %137, -8
  %139 = inttoptr i32 %138 to i8*
  %140 = getelementptr inbounds i8, i8* %139, i32 8
  store i8* %140, i8** %134, align 4
  %141 = bitcast i8* %139 to double*
  %142 = load double, double* %141, align 8
  %143 = fptrunc double %142 to float
  %144 = load %union.jvalue*, %union.jvalue** %10, align 4
  %145 = load i32, i32* %13, align 4
  %146 = getelementptr inbounds %union.jvalue, %union.jvalue* %144, i32 %145
  %147 = bitcast %union.jvalue* %146 to float*
  store float %143, float* %147, align 8
  br label %159

148:                                              ; preds = %48
  %149 = bitcast %struct.__va_list* %5 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = getelementptr inbounds i8, i8* %150, i32 4
  store i8* %151, i8** %149, align 4
  %152 = bitcast i8* %150 to i8**
  %153 = load i8*, i8** %152, align 4
  %154 = bitcast i8* %153 to %struct._jobject*
  %155 = load %union.jvalue*, %union.jvalue** %10, align 4
  %156 = load i32, i32* %13, align 4
  %157 = getelementptr inbounds %union.jvalue, %union.jvalue* %155, i32 %156
  %158 = bitcast %union.jvalue* %157 to %struct._jobject**
  store %struct._jobject* %154, %struct._jobject** %158, align 8
  br label %159

159:                                              ; preds = %148, %133, %119, %108, %98, %86, %75, %64, %53, %48
  %160 = load i32, i32* %13, align 4
  %161 = add nsw i32 %160, 1
  store i32 %161, i32* %13, align 4
  br label %44, !llvm.loop !33

162:                                              ; preds = %44
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %164 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %163, align 4
  %165 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %164, i32 0, i32 87
  %166 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %165, align 4
  %167 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %168 = load %struct._jobject*, %struct._jobject** %7, align 4
  %169 = load %struct._jobject*, %struct._jobject** %8, align 4
  %170 = load %struct._jmethodID*, %struct._jmethodID** %9, align 4
  %171 = load %union.jvalue*, %union.jvalue** %10, align 4
  %172 = call float %166(%struct.JNINativeInterface_** noundef %167, %struct._jobject* noundef %168, %struct._jobject* noundef %169, %struct._jmethodID* noundef %170, %union.jvalue* noundef %171) #2
  call void @llvm.stackrestore(i8* %28)
  store float %172, float* %18, align 4
  %173 = bitcast %struct.__va_list* %19 to i8*
  call void @llvm.va_end(i8* %173)
  %174 = load float, float* %18, align 4
  ret float %174
}

; Function Attrs: alwaysinline nounwind
define dso_local float @JNI_CallNonvirtualFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca %union.jvalue*, align 4
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %6, i32 0, i32 0
  %16 = bitcast i8** %15 to [1 x i32]*
  store [1 x i32] %4, [1 x i32]* %16, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %7, align 4
  store %struct._jobject* %1, %struct._jobject** %8, align 4
  store %struct._jobject* %2, %struct._jobject** %9, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %10, align 4
  br label %17

17:                                               ; preds = %5
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %19 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %18, align 4
  %20 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %19, i32 0, i32 0
  %21 = load i8*, i8** %20, align 4
  %22 = bitcast i8* %21 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %24 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i32 0, i32 0
  %26 = call i32 %22(%struct.JNINativeInterface_** noundef %23, %struct._jmethodID* noundef %24, i8* noundef %25)
  store i32 %26, i32* %13, align 4
  %27 = load i32, i32* %13, align 4
  %28 = mul i32 %27, 8
  %29 = alloca i8, i32 %28, align 8
  %30 = bitcast i8* %29 to %union.jvalue*
  store %union.jvalue* %30, %union.jvalue** %11, align 4
  store i32 0, i32* %14, align 4
  br label %31

31:                                               ; preds = %147, %17
  %32 = load i32, i32* %14, align 4
  %33 = load i32, i32* %13, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %150

35:                                               ; preds = %31
  %36 = load i32, i32* %14, align 4
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i32 0, i32 %36
  %38 = load i8, i8* %37, align 1
  %39 = zext i8 %38 to i32
  switch i32 %39, label %146 [
    i32 90, label %40
    i32 66, label %51
    i32 83, label %62
    i32 67, label %73
    i32 73, label %85
    i32 74, label %95
    i32 68, label %106
    i32 70, label %120
    i32 76, label %135
  ]

40:                                               ; preds = %35
  %41 = bitcast %struct.__va_list* %6 to i8**
  %42 = load i8*, i8** %41, align 4
  %43 = getelementptr inbounds i8, i8* %42, i32 4
  store i8* %43, i8** %41, align 4
  %44 = bitcast i8* %42 to i32*
  %45 = load i32, i32* %44, align 4
  %46 = trunc i32 %45 to i8
  %47 = load %union.jvalue*, %union.jvalue** %11, align 4
  %48 = load i32, i32* %14, align 4
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %47, i32 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %146

51:                                               ; preds = %35
  %52 = bitcast %struct.__va_list* %6 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %11, align 4
  %59 = load i32, i32* %14, align 4
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i32 %59
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %57, i8* %61, align 8
  br label %146

62:                                               ; preds = %35
  %63 = bitcast %struct.__va_list* %6 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load %union.jvalue*, %union.jvalue** %11, align 4
  %70 = load i32, i32* %14, align 4
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %69, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %68, i16* %72, align 8
  br label %146

73:                                               ; preds = %35
  %74 = bitcast %struct.__va_list* %6 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = trunc i32 %78 to i16
  %80 = zext i16 %79 to i32
  %81 = load %union.jvalue*, %union.jvalue** %11, align 4
  %82 = load i32, i32* %14, align 4
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %81, i32 %82
  %84 = bitcast %union.jvalue* %83 to i32*
  store i32 %80, i32* %84, align 8
  br label %146

85:                                               ; preds = %35
  %86 = bitcast %struct.__va_list* %6 to i8**
  %87 = load i8*, i8** %86, align 4
  %88 = getelementptr inbounds i8, i8* %87, i32 4
  store i8* %88, i8** %86, align 4
  %89 = bitcast i8* %87 to i32*
  %90 = load i32, i32* %89, align 4
  %91 = load %union.jvalue*, %union.jvalue** %11, align 4
  %92 = load i32, i32* %14, align 4
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %91, i32 %92
  %94 = bitcast %union.jvalue* %93 to i32*
  store i32 %90, i32* %94, align 8
  br label %146

95:                                               ; preds = %35
  %96 = bitcast %struct.__va_list* %6 to i8**
  %97 = load i8*, i8** %96, align 4
  %98 = getelementptr inbounds i8, i8* %97, i32 4
  store i8* %98, i8** %96, align 4
  %99 = bitcast i8* %97 to i32*
  %100 = load i32, i32* %99, align 4
  %101 = sext i32 %100 to i64
  %102 = load %union.jvalue*, %union.jvalue** %11, align 4
  %103 = load i32, i32* %14, align 4
  %104 = getelementptr inbounds %union.jvalue, %union.jvalue* %102, i32 %103
  %105 = bitcast %union.jvalue* %104 to i64*
  store i64 %101, i64* %105, align 8
  br label %146

106:                                              ; preds = %35
  %107 = bitcast %struct.__va_list* %6 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load %union.jvalue*, %union.jvalue** %11, align 4
  %117 = load i32, i32* %14, align 4
  %118 = getelementptr inbounds %union.jvalue, %union.jvalue* %116, i32 %117
  %119 = bitcast %union.jvalue* %118 to double*
  store double %115, double* %119, align 8
  br label %146

120:                                              ; preds = %35
  %121 = bitcast %struct.__va_list* %6 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = ptrtoint i8* %122 to i32
  %124 = add i32 %123, 7
  %125 = and i32 %124, -8
  %126 = inttoptr i32 %125 to i8*
  %127 = getelementptr inbounds i8, i8* %126, i32 8
  store i8* %127, i8** %121, align 4
  %128 = bitcast i8* %126 to double*
  %129 = load double, double* %128, align 8
  %130 = fptrunc double %129 to float
  %131 = load %union.jvalue*, %union.jvalue** %11, align 4
  %132 = load i32, i32* %14, align 4
  %133 = getelementptr inbounds %union.jvalue, %union.jvalue* %131, i32 %132
  %134 = bitcast %union.jvalue* %133 to float*
  store float %130, float* %134, align 8
  br label %146

135:                                              ; preds = %35
  %136 = bitcast %struct.__va_list* %6 to i8**
  %137 = load i8*, i8** %136, align 4
  %138 = getelementptr inbounds i8, i8* %137, i32 4
  store i8* %138, i8** %136, align 4
  %139 = bitcast i8* %137 to i8**
  %140 = load i8*, i8** %139, align 4
  %141 = bitcast i8* %140 to %struct._jobject*
  %142 = load %union.jvalue*, %union.jvalue** %11, align 4
  %143 = load i32, i32* %14, align 4
  %144 = getelementptr inbounds %union.jvalue, %union.jvalue* %142, i32 %143
  %145 = bitcast %union.jvalue* %144 to %struct._jobject**
  store %struct._jobject* %141, %struct._jobject** %145, align 8
  br label %146

146:                                              ; preds = %35, %135, %120, %106, %95, %85, %73, %62, %51, %40
  br label %147

147:                                              ; preds = %146
  %148 = load i32, i32* %14, align 4
  %149 = add nsw i32 %148, 1
  store i32 %149, i32* %14, align 4
  br label %31, !llvm.loop !33

150:                                              ; preds = %31
  br label %151

151:                                              ; preds = %150
  %152 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %153 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %152, align 4
  %154 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %153, i32 0, i32 87
  %155 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %154, align 4
  %156 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %157 = load %struct._jobject*, %struct._jobject** %8, align 4
  %158 = load %struct._jobject*, %struct._jobject** %9, align 4
  %159 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %160 = load %union.jvalue*, %union.jvalue** %11, align 4
  %161 = call float %155(%struct.JNINativeInterface_** noundef %156, %struct._jobject* noundef %157, %struct._jobject* noundef %158, %struct._jmethodID* noundef %159, %union.jvalue* noundef %160)
  ret float %161
}

; Function Attrs: alwaysinline nounwind
define dso_local float @JNI_CallStaticFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca float, align 4
  %16 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %17 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_start(i8* %17)
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %19 = load %struct._jobject*, %struct._jobject** %13, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %21 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %16, i32 0, i32 0
  %22 = bitcast i8** %21 to [1 x i32]*
  %23 = load [1 x i32], [1 x i32]* %22, align 4
  %24 = call i8* @llvm.stacksave()
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  store [1 x i32] %23, [1 x i32]* %26, align 4
  store %struct.JNINativeInterface_** %18, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %19, %struct._jobject** %6, align 4
  store %struct._jmethodID* %20, %struct._jmethodID** %7, align 4
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %28 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %27, align 4
  %29 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %28, i32 0, i32 0
  %30 = load i8*, i8** %29, align 4
  %31 = bitcast i8* %30 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %32 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %33 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %35 = call i32 %31(%struct.JNINativeInterface_** noundef %32, %struct._jmethodID* noundef %33, i8* noundef %34) #2
  store i32 %35, i32* %10, align 4
  %36 = load i32, i32* %10, align 4
  %37 = mul i32 %36, 8
  %38 = alloca i8, i32 %37, align 8
  %39 = bitcast i8* %38 to %union.jvalue*
  store %union.jvalue* %39, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %40

40:                                               ; preds = %155, %3
  %41 = load i32, i32* %11, align 4
  %42 = load i32, i32* %10, align 4
  %43 = icmp slt i32 %41, %42
  br i1 %43, label %44, label %158

44:                                               ; preds = %40
  %45 = load i32, i32* %11, align 4
  %46 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  switch i32 %48, label %155 [
    i32 90, label %49
    i32 66, label %60
    i32 83, label %71
    i32 67, label %82
    i32 73, label %94
    i32 74, label %104
    i32 68, label %115
    i32 70, label %129
    i32 76, label %144
  ]

49:                                               ; preds = %44
  %50 = bitcast %struct.__va_list* %4 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %8, align 4
  %57 = load i32, i32* %11, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %155

60:                                               ; preds = %44
  %61 = bitcast %struct.__va_list* %4 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i8
  %67 = load %union.jvalue*, %union.jvalue** %8, align 4
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i8*
  store i8 %66, i8* %70, align 8
  br label %155

71:                                               ; preds = %44
  %72 = bitcast %struct.__va_list* %4 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = load %union.jvalue*, %union.jvalue** %8, align 4
  %79 = load i32, i32* %11, align 4
  %80 = getelementptr inbounds %union.jvalue, %union.jvalue* %78, i32 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %155

82:                                               ; preds = %44
  %83 = bitcast %struct.__va_list* %4 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = zext i16 %88 to i32
  %90 = load %union.jvalue*, %union.jvalue** %8, align 4
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds %union.jvalue, %union.jvalue* %90, i32 %91
  %93 = bitcast %union.jvalue* %92 to i32*
  store i32 %89, i32* %93, align 8
  br label %155

94:                                               ; preds = %44
  %95 = bitcast %struct.__va_list* %4 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = getelementptr inbounds i8, i8* %96, i32 4
  store i8* %97, i8** %95, align 4
  %98 = bitcast i8* %96 to i32*
  %99 = load i32, i32* %98, align 4
  %100 = load %union.jvalue*, %union.jvalue** %8, align 4
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i32*
  store i32 %99, i32* %103, align 8
  br label %155

104:                                              ; preds = %44
  %105 = bitcast %struct.__va_list* %4 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = getelementptr inbounds i8, i8* %106, i32 4
  store i8* %107, i8** %105, align 4
  %108 = bitcast i8* %106 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = sext i32 %109 to i64
  %111 = load %union.jvalue*, %union.jvalue** %8, align 4
  %112 = load i32, i32* %11, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %111, i32 %112
  %114 = bitcast %union.jvalue* %113 to i64*
  store i64 %110, i64* %114, align 8
  br label %155

115:                                              ; preds = %44
  %116 = bitcast %struct.__va_list* %4 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = ptrtoint i8* %117 to i32
  %119 = add i32 %118, 7
  %120 = and i32 %119, -8
  %121 = inttoptr i32 %120 to i8*
  %122 = getelementptr inbounds i8, i8* %121, i32 8
  store i8* %122, i8** %116, align 4
  %123 = bitcast i8* %121 to double*
  %124 = load double, double* %123, align 8
  %125 = load %union.jvalue*, %union.jvalue** %8, align 4
  %126 = load i32, i32* %11, align 4
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %125, i32 %126
  %128 = bitcast %union.jvalue* %127 to double*
  store double %124, double* %128, align 8
  br label %155

129:                                              ; preds = %44
  %130 = bitcast %struct.__va_list* %4 to i8**
  %131 = load i8*, i8** %130, align 4
  %132 = ptrtoint i8* %131 to i32
  %133 = add i32 %132, 7
  %134 = and i32 %133, -8
  %135 = inttoptr i32 %134 to i8*
  %136 = getelementptr inbounds i8, i8* %135, i32 8
  store i8* %136, i8** %130, align 4
  %137 = bitcast i8* %135 to double*
  %138 = load double, double* %137, align 8
  %139 = fptrunc double %138 to float
  %140 = load %union.jvalue*, %union.jvalue** %8, align 4
  %141 = load i32, i32* %11, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %139, float* %143, align 8
  br label %155

144:                                              ; preds = %44
  %145 = bitcast %struct.__va_list* %4 to i8**
  %146 = load i8*, i8** %145, align 4
  %147 = getelementptr inbounds i8, i8* %146, i32 4
  store i8* %147, i8** %145, align 4
  %148 = bitcast i8* %146 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = bitcast i8* %149 to %struct._jobject*
  %151 = load %union.jvalue*, %union.jvalue** %8, align 4
  %152 = load i32, i32* %11, align 4
  %153 = getelementptr inbounds %union.jvalue, %union.jvalue* %151, i32 %152
  %154 = bitcast %union.jvalue* %153 to %struct._jobject**
  store %struct._jobject* %150, %struct._jobject** %154, align 8
  br label %155

155:                                              ; preds = %144, %129, %115, %104, %94, %82, %71, %60, %49, %44
  %156 = load i32, i32* %11, align 4
  %157 = add nsw i32 %156, 1
  store i32 %157, i32* %11, align 4
  br label %40, !llvm.loop !34

158:                                              ; preds = %40
  %159 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %160 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %159, align 4
  %161 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %160, i32 0, i32 137
  %162 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %161, align 4
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %164 = load %struct._jobject*, %struct._jobject** %6, align 4
  %165 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %166 = load %union.jvalue*, %union.jvalue** %8, align 4
  %167 = call float %162(%struct.JNINativeInterface_** noundef %163, %struct._jobject* noundef %164, %struct._jmethodID* noundef %165, %union.jvalue* noundef %166) #2
  call void @llvm.stackrestore(i8* %24)
  store float %167, float* %15, align 4
  %168 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_end(i8* %168)
  %169 = load float, float* %15, align 4
  ret float %169
}

; Function Attrs: alwaysinline nounwind
define dso_local float @JNI_CallStaticFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !34

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 137
  %153 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  %158 = call float %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret float %158
}

; Function Attrs: alwaysinline nounwind
define dso_local double @JNI_CallDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca double, align 8
  %16 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %17 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_start(i8* %17)
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %19 = load %struct._jobject*, %struct._jobject** %13, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %21 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %16, i32 0, i32 0
  %22 = bitcast i8** %21 to [1 x i32]*
  %23 = load [1 x i32], [1 x i32]* %22, align 4
  %24 = call i8* @llvm.stacksave()
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  store [1 x i32] %23, [1 x i32]* %26, align 4
  store %struct.JNINativeInterface_** %18, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %19, %struct._jobject** %6, align 4
  store %struct._jmethodID* %20, %struct._jmethodID** %7, align 4
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %28 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %27, align 4
  %29 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %28, i32 0, i32 0
  %30 = load i8*, i8** %29, align 4
  %31 = bitcast i8* %30 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %32 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %33 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %35 = call i32 %31(%struct.JNINativeInterface_** noundef %32, %struct._jmethodID* noundef %33, i8* noundef %34) #2
  store i32 %35, i32* %10, align 4
  %36 = load i32, i32* %10, align 4
  %37 = mul i32 %36, 8
  %38 = alloca i8, i32 %37, align 8
  %39 = bitcast i8* %38 to %union.jvalue*
  store %union.jvalue* %39, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %40

40:                                               ; preds = %155, %3
  %41 = load i32, i32* %11, align 4
  %42 = load i32, i32* %10, align 4
  %43 = icmp slt i32 %41, %42
  br i1 %43, label %44, label %158

44:                                               ; preds = %40
  %45 = load i32, i32* %11, align 4
  %46 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  switch i32 %48, label %155 [
    i32 90, label %49
    i32 66, label %60
    i32 83, label %71
    i32 67, label %82
    i32 73, label %94
    i32 74, label %104
    i32 68, label %115
    i32 70, label %129
    i32 76, label %144
  ]

49:                                               ; preds = %44
  %50 = bitcast %struct.__va_list* %4 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %8, align 4
  %57 = load i32, i32* %11, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %155

60:                                               ; preds = %44
  %61 = bitcast %struct.__va_list* %4 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i8
  %67 = load %union.jvalue*, %union.jvalue** %8, align 4
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i8*
  store i8 %66, i8* %70, align 8
  br label %155

71:                                               ; preds = %44
  %72 = bitcast %struct.__va_list* %4 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = load %union.jvalue*, %union.jvalue** %8, align 4
  %79 = load i32, i32* %11, align 4
  %80 = getelementptr inbounds %union.jvalue, %union.jvalue* %78, i32 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %155

82:                                               ; preds = %44
  %83 = bitcast %struct.__va_list* %4 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = zext i16 %88 to i32
  %90 = load %union.jvalue*, %union.jvalue** %8, align 4
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds %union.jvalue, %union.jvalue* %90, i32 %91
  %93 = bitcast %union.jvalue* %92 to i32*
  store i32 %89, i32* %93, align 8
  br label %155

94:                                               ; preds = %44
  %95 = bitcast %struct.__va_list* %4 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = getelementptr inbounds i8, i8* %96, i32 4
  store i8* %97, i8** %95, align 4
  %98 = bitcast i8* %96 to i32*
  %99 = load i32, i32* %98, align 4
  %100 = load %union.jvalue*, %union.jvalue** %8, align 4
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i32*
  store i32 %99, i32* %103, align 8
  br label %155

104:                                              ; preds = %44
  %105 = bitcast %struct.__va_list* %4 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = getelementptr inbounds i8, i8* %106, i32 4
  store i8* %107, i8** %105, align 4
  %108 = bitcast i8* %106 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = sext i32 %109 to i64
  %111 = load %union.jvalue*, %union.jvalue** %8, align 4
  %112 = load i32, i32* %11, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %111, i32 %112
  %114 = bitcast %union.jvalue* %113 to i64*
  store i64 %110, i64* %114, align 8
  br label %155

115:                                              ; preds = %44
  %116 = bitcast %struct.__va_list* %4 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = ptrtoint i8* %117 to i32
  %119 = add i32 %118, 7
  %120 = and i32 %119, -8
  %121 = inttoptr i32 %120 to i8*
  %122 = getelementptr inbounds i8, i8* %121, i32 8
  store i8* %122, i8** %116, align 4
  %123 = bitcast i8* %121 to double*
  %124 = load double, double* %123, align 8
  %125 = load %union.jvalue*, %union.jvalue** %8, align 4
  %126 = load i32, i32* %11, align 4
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %125, i32 %126
  %128 = bitcast %union.jvalue* %127 to double*
  store double %124, double* %128, align 8
  br label %155

129:                                              ; preds = %44
  %130 = bitcast %struct.__va_list* %4 to i8**
  %131 = load i8*, i8** %130, align 4
  %132 = ptrtoint i8* %131 to i32
  %133 = add i32 %132, 7
  %134 = and i32 %133, -8
  %135 = inttoptr i32 %134 to i8*
  %136 = getelementptr inbounds i8, i8* %135, i32 8
  store i8* %136, i8** %130, align 4
  %137 = bitcast i8* %135 to double*
  %138 = load double, double* %137, align 8
  %139 = fptrunc double %138 to float
  %140 = load %union.jvalue*, %union.jvalue** %8, align 4
  %141 = load i32, i32* %11, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %139, float* %143, align 8
  br label %155

144:                                              ; preds = %44
  %145 = bitcast %struct.__va_list* %4 to i8**
  %146 = load i8*, i8** %145, align 4
  %147 = getelementptr inbounds i8, i8* %146, i32 4
  store i8* %147, i8** %145, align 4
  %148 = bitcast i8* %146 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = bitcast i8* %149 to %struct._jobject*
  %151 = load %union.jvalue*, %union.jvalue** %8, align 4
  %152 = load i32, i32* %11, align 4
  %153 = getelementptr inbounds %union.jvalue, %union.jvalue* %151, i32 %152
  %154 = bitcast %union.jvalue* %153 to %struct._jobject**
  store %struct._jobject* %150, %struct._jobject** %154, align 8
  br label %155

155:                                              ; preds = %144, %129, %115, %104, %94, %82, %71, %60, %49, %44
  %156 = load i32, i32* %11, align 4
  %157 = add nsw i32 %156, 1
  store i32 %157, i32* %11, align 4
  br label %40, !llvm.loop !35

158:                                              ; preds = %40
  %159 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %160 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %159, align 4
  %161 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %160, i32 0, i32 60
  %162 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %161, align 4
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %164 = load %struct._jobject*, %struct._jobject** %6, align 4
  %165 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %166 = load %union.jvalue*, %union.jvalue** %8, align 4
  %167 = call double %162(%struct.JNINativeInterface_** noundef %163, %struct._jobject* noundef %164, %struct._jmethodID* noundef %165, %union.jvalue* noundef %166) #2
  call void @llvm.stackrestore(i8* %24)
  store double %167, double* %15, align 8
  %168 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_end(i8* %168)
  %169 = load double, double* %15, align 8
  ret double %169
}

; Function Attrs: alwaysinline nounwind
define dso_local double @JNI_CallDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !35

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 60
  %153 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  %158 = call double %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret double %158
}

; Function Attrs: alwaysinline nounwind
define dso_local double @JNI_CallNonvirtualDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jmethodID*, align 4
  %10 = alloca %union.jvalue*, align 4
  %11 = alloca [257 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca %struct.JNINativeInterface_**, align 4
  %15 = alloca %struct._jobject*, align 4
  %16 = alloca %struct._jobject*, align 4
  %17 = alloca %struct._jmethodID*, align 4
  %18 = alloca double, align 8
  %19 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %14, align 4
  store %struct._jobject* %1, %struct._jobject** %15, align 4
  store %struct._jobject* %2, %struct._jobject** %16, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %17, align 4
  %20 = bitcast %struct.__va_list* %19 to i8*
  call void @llvm.va_start(i8* %20)
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %14, align 4
  %22 = load %struct._jobject*, %struct._jobject** %15, align 4
  %23 = load %struct._jobject*, %struct._jobject** %16, align 4
  %24 = load %struct._jmethodID*, %struct._jmethodID** %17, align 4
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %19, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  %27 = load [1 x i32], [1 x i32]* %26, align 4
  %28 = call i8* @llvm.stacksave()
  %29 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %30 = bitcast i8** %29 to [1 x i32]*
  store [1 x i32] %27, [1 x i32]* %30, align 4
  store %struct.JNINativeInterface_** %21, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %22, %struct._jobject** %7, align 4
  store %struct._jobject* %23, %struct._jobject** %8, align 4
  store %struct._jmethodID* %24, %struct._jmethodID** %9, align 4
  %31 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %32 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %31, align 4
  %33 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %32, i32 0, i32 0
  %34 = load i8*, i8** %33, align 4
  %35 = bitcast i8* %34 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %36 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %37 = load %struct._jmethodID*, %struct._jmethodID** %9, align 4
  %38 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i32 0, i32 0
  %39 = call i32 %35(%struct.JNINativeInterface_** noundef %36, %struct._jmethodID* noundef %37, i8* noundef %38) #2
  store i32 %39, i32* %12, align 4
  %40 = load i32, i32* %12, align 4
  %41 = mul i32 %40, 8
  %42 = alloca i8, i32 %41, align 8
  %43 = bitcast i8* %42 to %union.jvalue*
  store %union.jvalue* %43, %union.jvalue** %10, align 4
  store i32 0, i32* %13, align 4
  br label %44

44:                                               ; preds = %159, %4
  %45 = load i32, i32* %13, align 4
  %46 = load i32, i32* %12, align 4
  %47 = icmp slt i32 %45, %46
  br i1 %47, label %48, label %162

48:                                               ; preds = %44
  %49 = load i32, i32* %13, align 4
  %50 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i32 0, i32 %49
  %51 = load i8, i8* %50, align 1
  %52 = zext i8 %51 to i32
  switch i32 %52, label %159 [
    i32 90, label %53
    i32 66, label %64
    i32 83, label %75
    i32 67, label %86
    i32 73, label %98
    i32 74, label %108
    i32 68, label %119
    i32 70, label %133
    i32 76, label %148
  ]

53:                                               ; preds = %48
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %10, align 4
  %61 = load i32, i32* %13, align 4
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i32 %61
  %63 = bitcast %union.jvalue* %62 to i8*
  store i8 %59, i8* %63, align 8
  br label %159

64:                                               ; preds = %48
  %65 = bitcast %struct.__va_list* %5 to i8**
  %66 = load i8*, i8** %65, align 4
  %67 = getelementptr inbounds i8, i8* %66, i32 4
  store i8* %67, i8** %65, align 4
  %68 = bitcast i8* %66 to i32*
  %69 = load i32, i32* %68, align 4
  %70 = trunc i32 %69 to i8
  %71 = load %union.jvalue*, %union.jvalue** %10, align 4
  %72 = load i32, i32* %13, align 4
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %71, i32 %72
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %70, i8* %74, align 8
  br label %159

75:                                               ; preds = %48
  %76 = bitcast %struct.__va_list* %5 to i8**
  %77 = load i8*, i8** %76, align 4
  %78 = getelementptr inbounds i8, i8* %77, i32 4
  store i8* %78, i8** %76, align 4
  %79 = bitcast i8* %77 to i32*
  %80 = load i32, i32* %79, align 4
  %81 = trunc i32 %80 to i16
  %82 = load %union.jvalue*, %union.jvalue** %10, align 4
  %83 = load i32, i32* %13, align 4
  %84 = getelementptr inbounds %union.jvalue, %union.jvalue* %82, i32 %83
  %85 = bitcast %union.jvalue* %84 to i16*
  store i16 %81, i16* %85, align 8
  br label %159

86:                                               ; preds = %48
  %87 = bitcast %struct.__va_list* %5 to i8**
  %88 = load i8*, i8** %87, align 4
  %89 = getelementptr inbounds i8, i8* %88, i32 4
  store i8* %89, i8** %87, align 4
  %90 = bitcast i8* %88 to i32*
  %91 = load i32, i32* %90, align 4
  %92 = trunc i32 %91 to i16
  %93 = zext i16 %92 to i32
  %94 = load %union.jvalue*, %union.jvalue** %10, align 4
  %95 = load i32, i32* %13, align 4
  %96 = getelementptr inbounds %union.jvalue, %union.jvalue* %94, i32 %95
  %97 = bitcast %union.jvalue* %96 to i32*
  store i32 %93, i32* %97, align 8
  br label %159

98:                                               ; preds = %48
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load %union.jvalue*, %union.jvalue** %10, align 4
  %105 = load i32, i32* %13, align 4
  %106 = getelementptr inbounds %union.jvalue, %union.jvalue* %104, i32 %105
  %107 = bitcast %union.jvalue* %106 to i32*
  store i32 %103, i32* %107, align 8
  br label %159

108:                                              ; preds = %48
  %109 = bitcast %struct.__va_list* %5 to i8**
  %110 = load i8*, i8** %109, align 4
  %111 = getelementptr inbounds i8, i8* %110, i32 4
  store i8* %111, i8** %109, align 4
  %112 = bitcast i8* %110 to i32*
  %113 = load i32, i32* %112, align 4
  %114 = sext i32 %113 to i64
  %115 = load %union.jvalue*, %union.jvalue** %10, align 4
  %116 = load i32, i32* %13, align 4
  %117 = getelementptr inbounds %union.jvalue, %union.jvalue* %115, i32 %116
  %118 = bitcast %union.jvalue* %117 to i64*
  store i64 %114, i64* %118, align 8
  br label %159

119:                                              ; preds = %48
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = ptrtoint i8* %121 to i32
  %123 = add i32 %122, 7
  %124 = and i32 %123, -8
  %125 = inttoptr i32 %124 to i8*
  %126 = getelementptr inbounds i8, i8* %125, i32 8
  store i8* %126, i8** %120, align 4
  %127 = bitcast i8* %125 to double*
  %128 = load double, double* %127, align 8
  %129 = load %union.jvalue*, %union.jvalue** %10, align 4
  %130 = load i32, i32* %13, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to double*
  store double %128, double* %132, align 8
  br label %159

133:                                              ; preds = %48
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = ptrtoint i8* %135 to i32
  %137 = add i32 %136, 7
  %138 = and i32 %137, -8
  %139 = inttoptr i32 %138 to i8*
  %140 = getelementptr inbounds i8, i8* %139, i32 8
  store i8* %140, i8** %134, align 4
  %141 = bitcast i8* %139 to double*
  %142 = load double, double* %141, align 8
  %143 = fptrunc double %142 to float
  %144 = load %union.jvalue*, %union.jvalue** %10, align 4
  %145 = load i32, i32* %13, align 4
  %146 = getelementptr inbounds %union.jvalue, %union.jvalue* %144, i32 %145
  %147 = bitcast %union.jvalue* %146 to float*
  store float %143, float* %147, align 8
  br label %159

148:                                              ; preds = %48
  %149 = bitcast %struct.__va_list* %5 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = getelementptr inbounds i8, i8* %150, i32 4
  store i8* %151, i8** %149, align 4
  %152 = bitcast i8* %150 to i8**
  %153 = load i8*, i8** %152, align 4
  %154 = bitcast i8* %153 to %struct._jobject*
  %155 = load %union.jvalue*, %union.jvalue** %10, align 4
  %156 = load i32, i32* %13, align 4
  %157 = getelementptr inbounds %union.jvalue, %union.jvalue* %155, i32 %156
  %158 = bitcast %union.jvalue* %157 to %struct._jobject**
  store %struct._jobject* %154, %struct._jobject** %158, align 8
  br label %159

159:                                              ; preds = %148, %133, %119, %108, %98, %86, %75, %64, %53, %48
  %160 = load i32, i32* %13, align 4
  %161 = add nsw i32 %160, 1
  store i32 %161, i32* %13, align 4
  br label %44, !llvm.loop !36

162:                                              ; preds = %44
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %164 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %163, align 4
  %165 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %164, i32 0, i32 90
  %166 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %165, align 4
  %167 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %168 = load %struct._jobject*, %struct._jobject** %7, align 4
  %169 = load %struct._jobject*, %struct._jobject** %8, align 4
  %170 = load %struct._jmethodID*, %struct._jmethodID** %9, align 4
  %171 = load %union.jvalue*, %union.jvalue** %10, align 4
  %172 = call double %166(%struct.JNINativeInterface_** noundef %167, %struct._jobject* noundef %168, %struct._jobject* noundef %169, %struct._jmethodID* noundef %170, %union.jvalue* noundef %171) #2
  call void @llvm.stackrestore(i8* %28)
  store double %172, double* %18, align 8
  %173 = bitcast %struct.__va_list* %19 to i8*
  call void @llvm.va_end(i8* %173)
  %174 = load double, double* %18, align 8
  ret double %174
}

; Function Attrs: alwaysinline nounwind
define dso_local double @JNI_CallNonvirtualDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca %union.jvalue*, align 4
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %6, i32 0, i32 0
  %16 = bitcast i8** %15 to [1 x i32]*
  store [1 x i32] %4, [1 x i32]* %16, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %7, align 4
  store %struct._jobject* %1, %struct._jobject** %8, align 4
  store %struct._jobject* %2, %struct._jobject** %9, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %10, align 4
  br label %17

17:                                               ; preds = %5
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %19 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %18, align 4
  %20 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %19, i32 0, i32 0
  %21 = load i8*, i8** %20, align 4
  %22 = bitcast i8* %21 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %24 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i32 0, i32 0
  %26 = call i32 %22(%struct.JNINativeInterface_** noundef %23, %struct._jmethodID* noundef %24, i8* noundef %25)
  store i32 %26, i32* %13, align 4
  %27 = load i32, i32* %13, align 4
  %28 = mul i32 %27, 8
  %29 = alloca i8, i32 %28, align 8
  %30 = bitcast i8* %29 to %union.jvalue*
  store %union.jvalue* %30, %union.jvalue** %11, align 4
  store i32 0, i32* %14, align 4
  br label %31

31:                                               ; preds = %147, %17
  %32 = load i32, i32* %14, align 4
  %33 = load i32, i32* %13, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %150

35:                                               ; preds = %31
  %36 = load i32, i32* %14, align 4
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i32 0, i32 %36
  %38 = load i8, i8* %37, align 1
  %39 = zext i8 %38 to i32
  switch i32 %39, label %146 [
    i32 90, label %40
    i32 66, label %51
    i32 83, label %62
    i32 67, label %73
    i32 73, label %85
    i32 74, label %95
    i32 68, label %106
    i32 70, label %120
    i32 76, label %135
  ]

40:                                               ; preds = %35
  %41 = bitcast %struct.__va_list* %6 to i8**
  %42 = load i8*, i8** %41, align 4
  %43 = getelementptr inbounds i8, i8* %42, i32 4
  store i8* %43, i8** %41, align 4
  %44 = bitcast i8* %42 to i32*
  %45 = load i32, i32* %44, align 4
  %46 = trunc i32 %45 to i8
  %47 = load %union.jvalue*, %union.jvalue** %11, align 4
  %48 = load i32, i32* %14, align 4
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %47, i32 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %146

51:                                               ; preds = %35
  %52 = bitcast %struct.__va_list* %6 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %11, align 4
  %59 = load i32, i32* %14, align 4
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i32 %59
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %57, i8* %61, align 8
  br label %146

62:                                               ; preds = %35
  %63 = bitcast %struct.__va_list* %6 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load %union.jvalue*, %union.jvalue** %11, align 4
  %70 = load i32, i32* %14, align 4
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %69, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %68, i16* %72, align 8
  br label %146

73:                                               ; preds = %35
  %74 = bitcast %struct.__va_list* %6 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = trunc i32 %78 to i16
  %80 = zext i16 %79 to i32
  %81 = load %union.jvalue*, %union.jvalue** %11, align 4
  %82 = load i32, i32* %14, align 4
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %81, i32 %82
  %84 = bitcast %union.jvalue* %83 to i32*
  store i32 %80, i32* %84, align 8
  br label %146

85:                                               ; preds = %35
  %86 = bitcast %struct.__va_list* %6 to i8**
  %87 = load i8*, i8** %86, align 4
  %88 = getelementptr inbounds i8, i8* %87, i32 4
  store i8* %88, i8** %86, align 4
  %89 = bitcast i8* %87 to i32*
  %90 = load i32, i32* %89, align 4
  %91 = load %union.jvalue*, %union.jvalue** %11, align 4
  %92 = load i32, i32* %14, align 4
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %91, i32 %92
  %94 = bitcast %union.jvalue* %93 to i32*
  store i32 %90, i32* %94, align 8
  br label %146

95:                                               ; preds = %35
  %96 = bitcast %struct.__va_list* %6 to i8**
  %97 = load i8*, i8** %96, align 4
  %98 = getelementptr inbounds i8, i8* %97, i32 4
  store i8* %98, i8** %96, align 4
  %99 = bitcast i8* %97 to i32*
  %100 = load i32, i32* %99, align 4
  %101 = sext i32 %100 to i64
  %102 = load %union.jvalue*, %union.jvalue** %11, align 4
  %103 = load i32, i32* %14, align 4
  %104 = getelementptr inbounds %union.jvalue, %union.jvalue* %102, i32 %103
  %105 = bitcast %union.jvalue* %104 to i64*
  store i64 %101, i64* %105, align 8
  br label %146

106:                                              ; preds = %35
  %107 = bitcast %struct.__va_list* %6 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load %union.jvalue*, %union.jvalue** %11, align 4
  %117 = load i32, i32* %14, align 4
  %118 = getelementptr inbounds %union.jvalue, %union.jvalue* %116, i32 %117
  %119 = bitcast %union.jvalue* %118 to double*
  store double %115, double* %119, align 8
  br label %146

120:                                              ; preds = %35
  %121 = bitcast %struct.__va_list* %6 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = ptrtoint i8* %122 to i32
  %124 = add i32 %123, 7
  %125 = and i32 %124, -8
  %126 = inttoptr i32 %125 to i8*
  %127 = getelementptr inbounds i8, i8* %126, i32 8
  store i8* %127, i8** %121, align 4
  %128 = bitcast i8* %126 to double*
  %129 = load double, double* %128, align 8
  %130 = fptrunc double %129 to float
  %131 = load %union.jvalue*, %union.jvalue** %11, align 4
  %132 = load i32, i32* %14, align 4
  %133 = getelementptr inbounds %union.jvalue, %union.jvalue* %131, i32 %132
  %134 = bitcast %union.jvalue* %133 to float*
  store float %130, float* %134, align 8
  br label %146

135:                                              ; preds = %35
  %136 = bitcast %struct.__va_list* %6 to i8**
  %137 = load i8*, i8** %136, align 4
  %138 = getelementptr inbounds i8, i8* %137, i32 4
  store i8* %138, i8** %136, align 4
  %139 = bitcast i8* %137 to i8**
  %140 = load i8*, i8** %139, align 4
  %141 = bitcast i8* %140 to %struct._jobject*
  %142 = load %union.jvalue*, %union.jvalue** %11, align 4
  %143 = load i32, i32* %14, align 4
  %144 = getelementptr inbounds %union.jvalue, %union.jvalue* %142, i32 %143
  %145 = bitcast %union.jvalue* %144 to %struct._jobject**
  store %struct._jobject* %141, %struct._jobject** %145, align 8
  br label %146

146:                                              ; preds = %35, %135, %120, %106, %95, %85, %73, %62, %51, %40
  br label %147

147:                                              ; preds = %146
  %148 = load i32, i32* %14, align 4
  %149 = add nsw i32 %148, 1
  store i32 %149, i32* %14, align 4
  br label %31, !llvm.loop !36

150:                                              ; preds = %31
  br label %151

151:                                              ; preds = %150
  %152 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %153 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %152, align 4
  %154 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %153, i32 0, i32 90
  %155 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %154, align 4
  %156 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %157 = load %struct._jobject*, %struct._jobject** %8, align 4
  %158 = load %struct._jobject*, %struct._jobject** %9, align 4
  %159 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %160 = load %union.jvalue*, %union.jvalue** %11, align 4
  %161 = call double %155(%struct.JNINativeInterface_** noundef %156, %struct._jobject* noundef %157, %struct._jobject* noundef %158, %struct._jmethodID* noundef %159, %union.jvalue* noundef %160)
  ret double %161
}

; Function Attrs: alwaysinline nounwind
define dso_local double @JNI_CallStaticDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca double, align 8
  %16 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %17 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_start(i8* %17)
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %19 = load %struct._jobject*, %struct._jobject** %13, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %21 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %16, i32 0, i32 0
  %22 = bitcast i8** %21 to [1 x i32]*
  %23 = load [1 x i32], [1 x i32]* %22, align 4
  %24 = call i8* @llvm.stacksave()
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  store [1 x i32] %23, [1 x i32]* %26, align 4
  store %struct.JNINativeInterface_** %18, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %19, %struct._jobject** %6, align 4
  store %struct._jmethodID* %20, %struct._jmethodID** %7, align 4
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %28 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %27, align 4
  %29 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %28, i32 0, i32 0
  %30 = load i8*, i8** %29, align 4
  %31 = bitcast i8* %30 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %32 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %33 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %35 = call i32 %31(%struct.JNINativeInterface_** noundef %32, %struct._jmethodID* noundef %33, i8* noundef %34) #2
  store i32 %35, i32* %10, align 4
  %36 = load i32, i32* %10, align 4
  %37 = mul i32 %36, 8
  %38 = alloca i8, i32 %37, align 8
  %39 = bitcast i8* %38 to %union.jvalue*
  store %union.jvalue* %39, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %40

40:                                               ; preds = %155, %3
  %41 = load i32, i32* %11, align 4
  %42 = load i32, i32* %10, align 4
  %43 = icmp slt i32 %41, %42
  br i1 %43, label %44, label %158

44:                                               ; preds = %40
  %45 = load i32, i32* %11, align 4
  %46 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  switch i32 %48, label %155 [
    i32 90, label %49
    i32 66, label %60
    i32 83, label %71
    i32 67, label %82
    i32 73, label %94
    i32 74, label %104
    i32 68, label %115
    i32 70, label %129
    i32 76, label %144
  ]

49:                                               ; preds = %44
  %50 = bitcast %struct.__va_list* %4 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %8, align 4
  %57 = load i32, i32* %11, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %155

60:                                               ; preds = %44
  %61 = bitcast %struct.__va_list* %4 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i8
  %67 = load %union.jvalue*, %union.jvalue** %8, align 4
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i8*
  store i8 %66, i8* %70, align 8
  br label %155

71:                                               ; preds = %44
  %72 = bitcast %struct.__va_list* %4 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = load %union.jvalue*, %union.jvalue** %8, align 4
  %79 = load i32, i32* %11, align 4
  %80 = getelementptr inbounds %union.jvalue, %union.jvalue* %78, i32 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %155

82:                                               ; preds = %44
  %83 = bitcast %struct.__va_list* %4 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = zext i16 %88 to i32
  %90 = load %union.jvalue*, %union.jvalue** %8, align 4
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds %union.jvalue, %union.jvalue* %90, i32 %91
  %93 = bitcast %union.jvalue* %92 to i32*
  store i32 %89, i32* %93, align 8
  br label %155

94:                                               ; preds = %44
  %95 = bitcast %struct.__va_list* %4 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = getelementptr inbounds i8, i8* %96, i32 4
  store i8* %97, i8** %95, align 4
  %98 = bitcast i8* %96 to i32*
  %99 = load i32, i32* %98, align 4
  %100 = load %union.jvalue*, %union.jvalue** %8, align 4
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i32*
  store i32 %99, i32* %103, align 8
  br label %155

104:                                              ; preds = %44
  %105 = bitcast %struct.__va_list* %4 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = getelementptr inbounds i8, i8* %106, i32 4
  store i8* %107, i8** %105, align 4
  %108 = bitcast i8* %106 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = sext i32 %109 to i64
  %111 = load %union.jvalue*, %union.jvalue** %8, align 4
  %112 = load i32, i32* %11, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %111, i32 %112
  %114 = bitcast %union.jvalue* %113 to i64*
  store i64 %110, i64* %114, align 8
  br label %155

115:                                              ; preds = %44
  %116 = bitcast %struct.__va_list* %4 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = ptrtoint i8* %117 to i32
  %119 = add i32 %118, 7
  %120 = and i32 %119, -8
  %121 = inttoptr i32 %120 to i8*
  %122 = getelementptr inbounds i8, i8* %121, i32 8
  store i8* %122, i8** %116, align 4
  %123 = bitcast i8* %121 to double*
  %124 = load double, double* %123, align 8
  %125 = load %union.jvalue*, %union.jvalue** %8, align 4
  %126 = load i32, i32* %11, align 4
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %125, i32 %126
  %128 = bitcast %union.jvalue* %127 to double*
  store double %124, double* %128, align 8
  br label %155

129:                                              ; preds = %44
  %130 = bitcast %struct.__va_list* %4 to i8**
  %131 = load i8*, i8** %130, align 4
  %132 = ptrtoint i8* %131 to i32
  %133 = add i32 %132, 7
  %134 = and i32 %133, -8
  %135 = inttoptr i32 %134 to i8*
  %136 = getelementptr inbounds i8, i8* %135, i32 8
  store i8* %136, i8** %130, align 4
  %137 = bitcast i8* %135 to double*
  %138 = load double, double* %137, align 8
  %139 = fptrunc double %138 to float
  %140 = load %union.jvalue*, %union.jvalue** %8, align 4
  %141 = load i32, i32* %11, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %139, float* %143, align 8
  br label %155

144:                                              ; preds = %44
  %145 = bitcast %struct.__va_list* %4 to i8**
  %146 = load i8*, i8** %145, align 4
  %147 = getelementptr inbounds i8, i8* %146, i32 4
  store i8* %147, i8** %145, align 4
  %148 = bitcast i8* %146 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = bitcast i8* %149 to %struct._jobject*
  %151 = load %union.jvalue*, %union.jvalue** %8, align 4
  %152 = load i32, i32* %11, align 4
  %153 = getelementptr inbounds %union.jvalue, %union.jvalue* %151, i32 %152
  %154 = bitcast %union.jvalue* %153 to %struct._jobject**
  store %struct._jobject* %150, %struct._jobject** %154, align 8
  br label %155

155:                                              ; preds = %144, %129, %115, %104, %94, %82, %71, %60, %49, %44
  %156 = load i32, i32* %11, align 4
  %157 = add nsw i32 %156, 1
  store i32 %157, i32* %11, align 4
  br label %40, !llvm.loop !37

158:                                              ; preds = %40
  %159 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %160 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %159, align 4
  %161 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %160, i32 0, i32 140
  %162 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %161, align 4
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %164 = load %struct._jobject*, %struct._jobject** %6, align 4
  %165 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %166 = load %union.jvalue*, %union.jvalue** %8, align 4
  %167 = call double %162(%struct.JNINativeInterface_** noundef %163, %struct._jobject* noundef %164, %struct._jmethodID* noundef %165, %union.jvalue* noundef %166) #2
  call void @llvm.stackrestore(i8* %24)
  store double %167, double* %15, align 8
  %168 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_end(i8* %168)
  %169 = load double, double* %15, align 8
  ret double %169
}

; Function Attrs: alwaysinline nounwind
define dso_local double @JNI_CallStaticDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !37

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 140
  %153 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  %158 = call double %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret double %158
}

; Function Attrs: alwaysinline nounwind
define dso_local %struct._jobject* @JNI_NewObject(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca %struct._jobject*, align 4
  %16 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %17 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_start(i8* %17)
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %19 = load %struct._jobject*, %struct._jobject** %13, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %21 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %16, i32 0, i32 0
  %22 = bitcast i8** %21 to [1 x i32]*
  %23 = load [1 x i32], [1 x i32]* %22, align 4
  %24 = call i8* @llvm.stacksave()
  %25 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %26 = bitcast i8** %25 to [1 x i32]*
  store [1 x i32] %23, [1 x i32]* %26, align 4
  store %struct.JNINativeInterface_** %18, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %19, %struct._jobject** %6, align 4
  store %struct._jmethodID* %20, %struct._jmethodID** %7, align 4
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %28 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %27, align 4
  %29 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %28, i32 0, i32 0
  %30 = load i8*, i8** %29, align 4
  %31 = bitcast i8* %30 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %32 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %33 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %35 = call i32 %31(%struct.JNINativeInterface_** noundef %32, %struct._jmethodID* noundef %33, i8* noundef %34) #2
  store i32 %35, i32* %10, align 4
  %36 = load i32, i32* %10, align 4
  %37 = mul i32 %36, 8
  %38 = alloca i8, i32 %37, align 8
  %39 = bitcast i8* %38 to %union.jvalue*
  store %union.jvalue* %39, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %40

40:                                               ; preds = %155, %3
  %41 = load i32, i32* %11, align 4
  %42 = load i32, i32* %10, align 4
  %43 = icmp slt i32 %41, %42
  br i1 %43, label %44, label %158

44:                                               ; preds = %40
  %45 = load i32, i32* %11, align 4
  %46 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  switch i32 %48, label %155 [
    i32 90, label %49
    i32 66, label %60
    i32 83, label %71
    i32 67, label %82
    i32 73, label %94
    i32 74, label %104
    i32 68, label %115
    i32 70, label %129
    i32 76, label %144
  ]

49:                                               ; preds = %44
  %50 = bitcast %struct.__va_list* %4 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %8, align 4
  %57 = load i32, i32* %11, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %155

60:                                               ; preds = %44
  %61 = bitcast %struct.__va_list* %4 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i8
  %67 = load %union.jvalue*, %union.jvalue** %8, align 4
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i8*
  store i8 %66, i8* %70, align 8
  br label %155

71:                                               ; preds = %44
  %72 = bitcast %struct.__va_list* %4 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = load %union.jvalue*, %union.jvalue** %8, align 4
  %79 = load i32, i32* %11, align 4
  %80 = getelementptr inbounds %union.jvalue, %union.jvalue* %78, i32 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %155

82:                                               ; preds = %44
  %83 = bitcast %struct.__va_list* %4 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = zext i16 %88 to i32
  %90 = load %union.jvalue*, %union.jvalue** %8, align 4
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds %union.jvalue, %union.jvalue* %90, i32 %91
  %93 = bitcast %union.jvalue* %92 to i32*
  store i32 %89, i32* %93, align 8
  br label %155

94:                                               ; preds = %44
  %95 = bitcast %struct.__va_list* %4 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = getelementptr inbounds i8, i8* %96, i32 4
  store i8* %97, i8** %95, align 4
  %98 = bitcast i8* %96 to i32*
  %99 = load i32, i32* %98, align 4
  %100 = load %union.jvalue*, %union.jvalue** %8, align 4
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i32*
  store i32 %99, i32* %103, align 8
  br label %155

104:                                              ; preds = %44
  %105 = bitcast %struct.__va_list* %4 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = getelementptr inbounds i8, i8* %106, i32 4
  store i8* %107, i8** %105, align 4
  %108 = bitcast i8* %106 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = sext i32 %109 to i64
  %111 = load %union.jvalue*, %union.jvalue** %8, align 4
  %112 = load i32, i32* %11, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %111, i32 %112
  %114 = bitcast %union.jvalue* %113 to i64*
  store i64 %110, i64* %114, align 8
  br label %155

115:                                              ; preds = %44
  %116 = bitcast %struct.__va_list* %4 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = ptrtoint i8* %117 to i32
  %119 = add i32 %118, 7
  %120 = and i32 %119, -8
  %121 = inttoptr i32 %120 to i8*
  %122 = getelementptr inbounds i8, i8* %121, i32 8
  store i8* %122, i8** %116, align 4
  %123 = bitcast i8* %121 to double*
  %124 = load double, double* %123, align 8
  %125 = load %union.jvalue*, %union.jvalue** %8, align 4
  %126 = load i32, i32* %11, align 4
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %125, i32 %126
  %128 = bitcast %union.jvalue* %127 to double*
  store double %124, double* %128, align 8
  br label %155

129:                                              ; preds = %44
  %130 = bitcast %struct.__va_list* %4 to i8**
  %131 = load i8*, i8** %130, align 4
  %132 = ptrtoint i8* %131 to i32
  %133 = add i32 %132, 7
  %134 = and i32 %133, -8
  %135 = inttoptr i32 %134 to i8*
  %136 = getelementptr inbounds i8, i8* %135, i32 8
  store i8* %136, i8** %130, align 4
  %137 = bitcast i8* %135 to double*
  %138 = load double, double* %137, align 8
  %139 = fptrunc double %138 to float
  %140 = load %union.jvalue*, %union.jvalue** %8, align 4
  %141 = load i32, i32* %11, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %139, float* %143, align 8
  br label %155

144:                                              ; preds = %44
  %145 = bitcast %struct.__va_list* %4 to i8**
  %146 = load i8*, i8** %145, align 4
  %147 = getelementptr inbounds i8, i8* %146, i32 4
  store i8* %147, i8** %145, align 4
  %148 = bitcast i8* %146 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = bitcast i8* %149 to %struct._jobject*
  %151 = load %union.jvalue*, %union.jvalue** %8, align 4
  %152 = load i32, i32* %11, align 4
  %153 = getelementptr inbounds %union.jvalue, %union.jvalue* %151, i32 %152
  %154 = bitcast %union.jvalue* %153 to %struct._jobject**
  store %struct._jobject* %150, %struct._jobject** %154, align 8
  br label %155

155:                                              ; preds = %144, %129, %115, %104, %94, %82, %71, %60, %49, %44
  %156 = load i32, i32* %11, align 4
  %157 = add nsw i32 %156, 1
  store i32 %157, i32* %11, align 4
  br label %40, !llvm.loop !38

158:                                              ; preds = %40
  %159 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %160 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %159, align 4
  %161 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %160, i32 0, i32 30
  %162 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %161, align 4
  %163 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %164 = load %struct._jobject*, %struct._jobject** %6, align 4
  %165 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %166 = load %union.jvalue*, %union.jvalue** %8, align 4
  %167 = call %struct._jobject* %162(%struct.JNINativeInterface_** noundef %163, %struct._jobject* noundef %164, %struct._jmethodID* noundef %165, %union.jvalue* noundef %166) #2
  call void @llvm.stackrestore(i8* %24)
  store %struct._jobject* %167, %struct._jobject** %15, align 4
  %168 = bitcast %struct.__va_list* %16 to i8*
  call void @llvm.va_end(i8* %168)
  %169 = load %struct._jobject*, %struct._jobject** %15, align 4
  ret %struct._jobject* %169
}

; Function Attrs: alwaysinline nounwind
define dso_local %struct._jobject* @JNI_NewObjectV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !38

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 30
  %153 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  %158 = call %struct._jobject* %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret %struct._jobject* %158
}

; Function Attrs: alwaysinline nounwind
define dso_local void @JNI_CallVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %16 = bitcast %struct.__va_list* %15 to i8*
  call void @llvm.va_start(i8* %16)
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %18 = load %struct._jobject*, %struct._jobject** %13, align 4
  %19 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %20 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %15, i32 0, i32 0
  %21 = bitcast i8** %20 to [1 x i32]*
  %22 = load [1 x i32], [1 x i32]* %21, align 4
  %23 = call i8* @llvm.stacksave()
  %24 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %25 = bitcast i8** %24 to [1 x i32]*
  store [1 x i32] %22, [1 x i32]* %25, align 4
  store %struct.JNINativeInterface_** %17, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %18, %struct._jobject** %6, align 4
  store %struct._jmethodID* %19, %struct._jmethodID** %7, align 4
  %26 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %27 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %26, align 4
  %28 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %27, i32 0, i32 0
  %29 = load i8*, i8** %28, align 4
  %30 = bitcast i8* %29 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %31 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %32 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %34 = call i32 %30(%struct.JNINativeInterface_** noundef %31, %struct._jmethodID* noundef %32, i8* noundef %33) #2
  store i32 %34, i32* %10, align 4
  %35 = load i32, i32* %10, align 4
  %36 = mul i32 %35, 8
  %37 = alloca i8, i32 %36, align 8
  %38 = bitcast i8* %37 to %union.jvalue*
  store %union.jvalue* %38, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %39

39:                                               ; preds = %154, %3
  %40 = load i32, i32* %11, align 4
  %41 = load i32, i32* %10, align 4
  %42 = icmp slt i32 %40, %41
  br i1 %42, label %43, label %157

43:                                               ; preds = %39
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  switch i32 %47, label %154 [
    i32 90, label %48
    i32 66, label %59
    i32 83, label %70
    i32 67, label %81
    i32 73, label %93
    i32 74, label %103
    i32 68, label %114
    i32 70, label %128
    i32 76, label %143
  ]

48:                                               ; preds = %43
  %49 = bitcast %struct.__va_list* %4 to i8**
  %50 = load i8*, i8** %49, align 4
  %51 = getelementptr inbounds i8, i8* %50, i32 4
  store i8* %51, i8** %49, align 4
  %52 = bitcast i8* %50 to i32*
  %53 = load i32, i32* %52, align 4
  %54 = trunc i32 %53 to i8
  %55 = load %union.jvalue*, %union.jvalue** %8, align 4
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds %union.jvalue, %union.jvalue* %55, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %54, i8* %58, align 8
  br label %154

59:                                               ; preds = %43
  %60 = bitcast %struct.__va_list* %4 to i8**
  %61 = load i8*, i8** %60, align 4
  %62 = getelementptr inbounds i8, i8* %61, i32 4
  store i8* %62, i8** %60, align 4
  %63 = bitcast i8* %61 to i32*
  %64 = load i32, i32* %63, align 4
  %65 = trunc i32 %64 to i8
  %66 = load %union.jvalue*, %union.jvalue** %8, align 4
  %67 = load i32, i32* %11, align 4
  %68 = getelementptr inbounds %union.jvalue, %union.jvalue* %66, i32 %67
  %69 = bitcast %union.jvalue* %68 to i8*
  store i8 %65, i8* %69, align 8
  br label %154

70:                                               ; preds = %43
  %71 = bitcast %struct.__va_list* %4 to i8**
  %72 = load i8*, i8** %71, align 4
  %73 = getelementptr inbounds i8, i8* %72, i32 4
  store i8* %73, i8** %71, align 4
  %74 = bitcast i8* %72 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = load %union.jvalue*, %union.jvalue** %8, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds %union.jvalue, %union.jvalue* %77, i32 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %154

81:                                               ; preds = %43
  %82 = bitcast %struct.__va_list* %4 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = zext i16 %87 to i32
  %89 = load %union.jvalue*, %union.jvalue** %8, align 4
  %90 = load i32, i32* %11, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %154

93:                                               ; preds = %43
  %94 = bitcast %struct.__va_list* %4 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = load %union.jvalue*, %union.jvalue** %8, align 4
  %100 = load i32, i32* %11, align 4
  %101 = getelementptr inbounds %union.jvalue, %union.jvalue* %99, i32 %100
  %102 = bitcast %union.jvalue* %101 to i32*
  store i32 %98, i32* %102, align 8
  br label %154

103:                                              ; preds = %43
  %104 = bitcast %struct.__va_list* %4 to i8**
  %105 = load i8*, i8** %104, align 4
  %106 = getelementptr inbounds i8, i8* %105, i32 4
  store i8* %106, i8** %104, align 4
  %107 = bitcast i8* %105 to i32*
  %108 = load i32, i32* %107, align 4
  %109 = sext i32 %108 to i64
  %110 = load %union.jvalue*, %union.jvalue** %8, align 4
  %111 = load i32, i32* %11, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i32 %111
  %113 = bitcast %union.jvalue* %112 to i64*
  store i64 %109, i64* %113, align 8
  br label %154

114:                                              ; preds = %43
  %115 = bitcast %struct.__va_list* %4 to i8**
  %116 = load i8*, i8** %115, align 4
  %117 = ptrtoint i8* %116 to i32
  %118 = add i32 %117, 7
  %119 = and i32 %118, -8
  %120 = inttoptr i32 %119 to i8*
  %121 = getelementptr inbounds i8, i8* %120, i32 8
  store i8* %121, i8** %115, align 4
  %122 = bitcast i8* %120 to double*
  %123 = load double, double* %122, align 8
  %124 = load %union.jvalue*, %union.jvalue** %8, align 4
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %124, i32 %125
  %127 = bitcast %union.jvalue* %126 to double*
  store double %123, double* %127, align 8
  br label %154

128:                                              ; preds = %43
  %129 = bitcast %struct.__va_list* %4 to i8**
  %130 = load i8*, i8** %129, align 4
  %131 = ptrtoint i8* %130 to i32
  %132 = add i32 %131, 7
  %133 = and i32 %132, -8
  %134 = inttoptr i32 %133 to i8*
  %135 = getelementptr inbounds i8, i8* %134, i32 8
  store i8* %135, i8** %129, align 4
  %136 = bitcast i8* %134 to double*
  %137 = load double, double* %136, align 8
  %138 = fptrunc double %137 to float
  %139 = load %union.jvalue*, %union.jvalue** %8, align 4
  %140 = load i32, i32* %11, align 4
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %139, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %138, float* %142, align 8
  br label %154

143:                                              ; preds = %43
  %144 = bitcast %struct.__va_list* %4 to i8**
  %145 = load i8*, i8** %144, align 4
  %146 = getelementptr inbounds i8, i8* %145, i32 4
  store i8* %146, i8** %144, align 4
  %147 = bitcast i8* %145 to i8**
  %148 = load i8*, i8** %147, align 4
  %149 = bitcast i8* %148 to %struct._jobject*
  %150 = load %union.jvalue*, %union.jvalue** %8, align 4
  %151 = load i32, i32* %11, align 4
  %152 = getelementptr inbounds %union.jvalue, %union.jvalue* %150, i32 %151
  %153 = bitcast %union.jvalue* %152 to %struct._jobject**
  store %struct._jobject* %149, %struct._jobject** %153, align 8
  br label %154

154:                                              ; preds = %143, %128, %114, %103, %93, %81, %70, %59, %48, %43
  %155 = load i32, i32* %11, align 4
  %156 = add nsw i32 %155, 1
  store i32 %156, i32* %11, align 4
  br label %39, !llvm.loop !39

157:                                              ; preds = %39
  %158 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %159 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %158, align 4
  %160 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %159, i32 0, i32 63
  %161 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %160, align 4
  %162 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %163 = load %struct._jobject*, %struct._jobject** %6, align 4
  %164 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %165 = load %union.jvalue*, %union.jvalue** %8, align 4
  call void %161(%struct.JNINativeInterface_** noundef %162, %struct._jobject* noundef %163, %struct._jmethodID* noundef %164, %union.jvalue* noundef %165) #2
  call void @llvm.stackrestore(i8* %23)
  %166 = bitcast %struct.__va_list* %15 to i8*
  call void @llvm.va_end(i8* %166)
  ret void
}

; Function Attrs: alwaysinline nounwind
define dso_local void @JNI_CallVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !39

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 63
  %153 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  call void %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret void
}

; Function Attrs: alwaysinline nounwind
define dso_local void @JNI_CallNonvirtualVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jmethodID*, align 4
  %10 = alloca %union.jvalue*, align 4
  %11 = alloca [257 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca %struct.JNINativeInterface_**, align 4
  %15 = alloca %struct._jobject*, align 4
  %16 = alloca %struct._jobject*, align 4
  %17 = alloca %struct._jmethodID*, align 4
  %18 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %14, align 4
  store %struct._jobject* %1, %struct._jobject** %15, align 4
  store %struct._jobject* %2, %struct._jobject** %16, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %17, align 4
  %19 = bitcast %struct.__va_list* %18 to i8*
  call void @llvm.va_start(i8* %19)
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %14, align 4
  %21 = load %struct._jobject*, %struct._jobject** %15, align 4
  %22 = load %struct._jobject*, %struct._jobject** %16, align 4
  %23 = load %struct._jmethodID*, %struct._jmethodID** %17, align 4
  %24 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %18, i32 0, i32 0
  %25 = bitcast i8** %24 to [1 x i32]*
  %26 = load [1 x i32], [1 x i32]* %25, align 4
  %27 = call i8* @llvm.stacksave()
  %28 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %29 = bitcast i8** %28 to [1 x i32]*
  store [1 x i32] %26, [1 x i32]* %29, align 4
  store %struct.JNINativeInterface_** %20, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %21, %struct._jobject** %7, align 4
  store %struct._jobject* %22, %struct._jobject** %8, align 4
  store %struct._jmethodID* %23, %struct._jmethodID** %9, align 4
  %30 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %31 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %30, align 4
  %32 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %31, i32 0, i32 0
  %33 = load i8*, i8** %32, align 4
  %34 = bitcast i8* %33 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %35 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %36 = load %struct._jmethodID*, %struct._jmethodID** %9, align 4
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i32 0, i32 0
  %38 = call i32 %34(%struct.JNINativeInterface_** noundef %35, %struct._jmethodID* noundef %36, i8* noundef %37) #2
  store i32 %38, i32* %12, align 4
  %39 = load i32, i32* %12, align 4
  %40 = mul i32 %39, 8
  %41 = alloca i8, i32 %40, align 8
  %42 = bitcast i8* %41 to %union.jvalue*
  store %union.jvalue* %42, %union.jvalue** %10, align 4
  store i32 0, i32* %13, align 4
  br label %43

43:                                               ; preds = %158, %4
  %44 = load i32, i32* %13, align 4
  %45 = load i32, i32* %12, align 4
  %46 = icmp slt i32 %44, %45
  br i1 %46, label %47, label %161

47:                                               ; preds = %43
  %48 = load i32, i32* %13, align 4
  %49 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i32 0, i32 %48
  %50 = load i8, i8* %49, align 1
  %51 = zext i8 %50 to i32
  switch i32 %51, label %158 [
    i32 90, label %52
    i32 66, label %63
    i32 83, label %74
    i32 67, label %85
    i32 73, label %97
    i32 74, label %107
    i32 68, label %118
    i32 70, label %132
    i32 76, label %147
  ]

52:                                               ; preds = %47
  %53 = bitcast %struct.__va_list* %5 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = load %union.jvalue*, %union.jvalue** %10, align 4
  %60 = load i32, i32* %13, align 4
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %59, i32 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %58, i8* %62, align 8
  br label %158

63:                                               ; preds = %47
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i8
  %70 = load %union.jvalue*, %union.jvalue** %10, align 4
  %71 = load i32, i32* %13, align 4
  %72 = getelementptr inbounds %union.jvalue, %union.jvalue* %70, i32 %71
  %73 = bitcast %union.jvalue* %72 to i8*
  store i8 %69, i8* %73, align 8
  br label %158

74:                                               ; preds = %47
  %75 = bitcast %struct.__va_list* %5 to i8**
  %76 = load i8*, i8** %75, align 4
  %77 = getelementptr inbounds i8, i8* %76, i32 4
  store i8* %77, i8** %75, align 4
  %78 = bitcast i8* %76 to i32*
  %79 = load i32, i32* %78, align 4
  %80 = trunc i32 %79 to i16
  %81 = load %union.jvalue*, %union.jvalue** %10, align 4
  %82 = load i32, i32* %13, align 4
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %81, i32 %82
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %80, i16* %84, align 8
  br label %158

85:                                               ; preds = %47
  %86 = bitcast %struct.__va_list* %5 to i8**
  %87 = load i8*, i8** %86, align 4
  %88 = getelementptr inbounds i8, i8* %87, i32 4
  store i8* %88, i8** %86, align 4
  %89 = bitcast i8* %87 to i32*
  %90 = load i32, i32* %89, align 4
  %91 = trunc i32 %90 to i16
  %92 = zext i16 %91 to i32
  %93 = load %union.jvalue*, %union.jvalue** %10, align 4
  %94 = load i32, i32* %13, align 4
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %93, i32 %94
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %92, i32* %96, align 8
  br label %158

97:                                               ; preds = %47
  %98 = bitcast %struct.__va_list* %5 to i8**
  %99 = load i8*, i8** %98, align 4
  %100 = getelementptr inbounds i8, i8* %99, i32 4
  store i8* %100, i8** %98, align 4
  %101 = bitcast i8* %99 to i32*
  %102 = load i32, i32* %101, align 4
  %103 = load %union.jvalue*, %union.jvalue** %10, align 4
  %104 = load i32, i32* %13, align 4
  %105 = getelementptr inbounds %union.jvalue, %union.jvalue* %103, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %102, i32* %106, align 8
  br label %158

107:                                              ; preds = %47
  %108 = bitcast %struct.__va_list* %5 to i8**
  %109 = load i8*, i8** %108, align 4
  %110 = getelementptr inbounds i8, i8* %109, i32 4
  store i8* %110, i8** %108, align 4
  %111 = bitcast i8* %109 to i32*
  %112 = load i32, i32* %111, align 4
  %113 = sext i32 %112 to i64
  %114 = load %union.jvalue*, %union.jvalue** %10, align 4
  %115 = load i32, i32* %13, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to i64*
  store i64 %113, i64* %117, align 8
  br label %158

118:                                              ; preds = %47
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = load %union.jvalue*, %union.jvalue** %10, align 4
  %129 = load i32, i32* %13, align 4
  %130 = getelementptr inbounds %union.jvalue, %union.jvalue* %128, i32 %129
  %131 = bitcast %union.jvalue* %130 to double*
  store double %127, double* %131, align 8
  br label %158

132:                                              ; preds = %47
  %133 = bitcast %struct.__va_list* %5 to i8**
  %134 = load i8*, i8** %133, align 4
  %135 = ptrtoint i8* %134 to i32
  %136 = add i32 %135, 7
  %137 = and i32 %136, -8
  %138 = inttoptr i32 %137 to i8*
  %139 = getelementptr inbounds i8, i8* %138, i32 8
  store i8* %139, i8** %133, align 4
  %140 = bitcast i8* %138 to double*
  %141 = load double, double* %140, align 8
  %142 = fptrunc double %141 to float
  %143 = load %union.jvalue*, %union.jvalue** %10, align 4
  %144 = load i32, i32* %13, align 4
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %143, i32 %144
  %146 = bitcast %union.jvalue* %145 to float*
  store float %142, float* %146, align 8
  br label %158

147:                                              ; preds = %47
  %148 = bitcast %struct.__va_list* %5 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = getelementptr inbounds i8, i8* %149, i32 4
  store i8* %150, i8** %148, align 4
  %151 = bitcast i8* %149 to i8**
  %152 = load i8*, i8** %151, align 4
  %153 = bitcast i8* %152 to %struct._jobject*
  %154 = load %union.jvalue*, %union.jvalue** %10, align 4
  %155 = load i32, i32* %13, align 4
  %156 = getelementptr inbounds %union.jvalue, %union.jvalue* %154, i32 %155
  %157 = bitcast %union.jvalue* %156 to %struct._jobject**
  store %struct._jobject* %153, %struct._jobject** %157, align 8
  br label %158

158:                                              ; preds = %147, %132, %118, %107, %97, %85, %74, %63, %52, %47
  %159 = load i32, i32* %13, align 4
  %160 = add nsw i32 %159, 1
  store i32 %160, i32* %13, align 4
  br label %43, !llvm.loop !40

161:                                              ; preds = %43
  %162 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %163 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %162, align 4
  %164 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %163, i32 0, i32 93
  %165 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %164, align 4
  %166 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %167 = load %struct._jobject*, %struct._jobject** %7, align 4
  %168 = load %struct._jobject*, %struct._jobject** %8, align 4
  %169 = load %struct._jmethodID*, %struct._jmethodID** %9, align 4
  %170 = load %union.jvalue*, %union.jvalue** %10, align 4
  call void %165(%struct.JNINativeInterface_** noundef %166, %struct._jobject* noundef %167, %struct._jobject* noundef %168, %struct._jmethodID* noundef %169, %union.jvalue* noundef %170) #2
  call void @llvm.stackrestore(i8* %27)
  %171 = bitcast %struct.__va_list* %18 to i8*
  call void @llvm.va_end(i8* %171)
  ret void
}

; Function Attrs: alwaysinline nounwind
define dso_local void @JNI_CallNonvirtualVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca %union.jvalue*, align 4
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %6, i32 0, i32 0
  %16 = bitcast i8** %15 to [1 x i32]*
  store [1 x i32] %4, [1 x i32]* %16, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %7, align 4
  store %struct._jobject* %1, %struct._jobject** %8, align 4
  store %struct._jobject* %2, %struct._jobject** %9, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %10, align 4
  br label %17

17:                                               ; preds = %5
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %19 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %18, align 4
  %20 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %19, i32 0, i32 0
  %21 = load i8*, i8** %20, align 4
  %22 = bitcast i8* %21 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %24 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i32 0, i32 0
  %26 = call i32 %22(%struct.JNINativeInterface_** noundef %23, %struct._jmethodID* noundef %24, i8* noundef %25)
  store i32 %26, i32* %13, align 4
  %27 = load i32, i32* %13, align 4
  %28 = mul i32 %27, 8
  %29 = alloca i8, i32 %28, align 8
  %30 = bitcast i8* %29 to %union.jvalue*
  store %union.jvalue* %30, %union.jvalue** %11, align 4
  store i32 0, i32* %14, align 4
  br label %31

31:                                               ; preds = %147, %17
  %32 = load i32, i32* %14, align 4
  %33 = load i32, i32* %13, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %150

35:                                               ; preds = %31
  %36 = load i32, i32* %14, align 4
  %37 = getelementptr inbounds [257 x i8], [257 x i8]* %12, i32 0, i32 %36
  %38 = load i8, i8* %37, align 1
  %39 = zext i8 %38 to i32
  switch i32 %39, label %146 [
    i32 90, label %40
    i32 66, label %51
    i32 83, label %62
    i32 67, label %73
    i32 73, label %85
    i32 74, label %95
    i32 68, label %106
    i32 70, label %120
    i32 76, label %135
  ]

40:                                               ; preds = %35
  %41 = bitcast %struct.__va_list* %6 to i8**
  %42 = load i8*, i8** %41, align 4
  %43 = getelementptr inbounds i8, i8* %42, i32 4
  store i8* %43, i8** %41, align 4
  %44 = bitcast i8* %42 to i32*
  %45 = load i32, i32* %44, align 4
  %46 = trunc i32 %45 to i8
  %47 = load %union.jvalue*, %union.jvalue** %11, align 4
  %48 = load i32, i32* %14, align 4
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %47, i32 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %146

51:                                               ; preds = %35
  %52 = bitcast %struct.__va_list* %6 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %11, align 4
  %59 = load i32, i32* %14, align 4
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i32 %59
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %57, i8* %61, align 8
  br label %146

62:                                               ; preds = %35
  %63 = bitcast %struct.__va_list* %6 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load %union.jvalue*, %union.jvalue** %11, align 4
  %70 = load i32, i32* %14, align 4
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %69, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %68, i16* %72, align 8
  br label %146

73:                                               ; preds = %35
  %74 = bitcast %struct.__va_list* %6 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = trunc i32 %78 to i16
  %80 = zext i16 %79 to i32
  %81 = load %union.jvalue*, %union.jvalue** %11, align 4
  %82 = load i32, i32* %14, align 4
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %81, i32 %82
  %84 = bitcast %union.jvalue* %83 to i32*
  store i32 %80, i32* %84, align 8
  br label %146

85:                                               ; preds = %35
  %86 = bitcast %struct.__va_list* %6 to i8**
  %87 = load i8*, i8** %86, align 4
  %88 = getelementptr inbounds i8, i8* %87, i32 4
  store i8* %88, i8** %86, align 4
  %89 = bitcast i8* %87 to i32*
  %90 = load i32, i32* %89, align 4
  %91 = load %union.jvalue*, %union.jvalue** %11, align 4
  %92 = load i32, i32* %14, align 4
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %91, i32 %92
  %94 = bitcast %union.jvalue* %93 to i32*
  store i32 %90, i32* %94, align 8
  br label %146

95:                                               ; preds = %35
  %96 = bitcast %struct.__va_list* %6 to i8**
  %97 = load i8*, i8** %96, align 4
  %98 = getelementptr inbounds i8, i8* %97, i32 4
  store i8* %98, i8** %96, align 4
  %99 = bitcast i8* %97 to i32*
  %100 = load i32, i32* %99, align 4
  %101 = sext i32 %100 to i64
  %102 = load %union.jvalue*, %union.jvalue** %11, align 4
  %103 = load i32, i32* %14, align 4
  %104 = getelementptr inbounds %union.jvalue, %union.jvalue* %102, i32 %103
  %105 = bitcast %union.jvalue* %104 to i64*
  store i64 %101, i64* %105, align 8
  br label %146

106:                                              ; preds = %35
  %107 = bitcast %struct.__va_list* %6 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load %union.jvalue*, %union.jvalue** %11, align 4
  %117 = load i32, i32* %14, align 4
  %118 = getelementptr inbounds %union.jvalue, %union.jvalue* %116, i32 %117
  %119 = bitcast %union.jvalue* %118 to double*
  store double %115, double* %119, align 8
  br label %146

120:                                              ; preds = %35
  %121 = bitcast %struct.__va_list* %6 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = ptrtoint i8* %122 to i32
  %124 = add i32 %123, 7
  %125 = and i32 %124, -8
  %126 = inttoptr i32 %125 to i8*
  %127 = getelementptr inbounds i8, i8* %126, i32 8
  store i8* %127, i8** %121, align 4
  %128 = bitcast i8* %126 to double*
  %129 = load double, double* %128, align 8
  %130 = fptrunc double %129 to float
  %131 = load %union.jvalue*, %union.jvalue** %11, align 4
  %132 = load i32, i32* %14, align 4
  %133 = getelementptr inbounds %union.jvalue, %union.jvalue* %131, i32 %132
  %134 = bitcast %union.jvalue* %133 to float*
  store float %130, float* %134, align 8
  br label %146

135:                                              ; preds = %35
  %136 = bitcast %struct.__va_list* %6 to i8**
  %137 = load i8*, i8** %136, align 4
  %138 = getelementptr inbounds i8, i8* %137, i32 4
  store i8* %138, i8** %136, align 4
  %139 = bitcast i8* %137 to i8**
  %140 = load i8*, i8** %139, align 4
  %141 = bitcast i8* %140 to %struct._jobject*
  %142 = load %union.jvalue*, %union.jvalue** %11, align 4
  %143 = load i32, i32* %14, align 4
  %144 = getelementptr inbounds %union.jvalue, %union.jvalue* %142, i32 %143
  %145 = bitcast %union.jvalue* %144 to %struct._jobject**
  store %struct._jobject* %141, %struct._jobject** %145, align 8
  br label %146

146:                                              ; preds = %35, %135, %120, %106, %95, %85, %73, %62, %51, %40
  br label %147

147:                                              ; preds = %146
  %148 = load i32, i32* %14, align 4
  %149 = add nsw i32 %148, 1
  store i32 %149, i32* %14, align 4
  br label %31, !llvm.loop !40

150:                                              ; preds = %31
  br label %151

151:                                              ; preds = %150
  %152 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %153 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %152, align 4
  %154 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %153, i32 0, i32 93
  %155 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %154, align 4
  %156 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %157 = load %struct._jobject*, %struct._jobject** %8, align 4
  %158 = load %struct._jobject*, %struct._jobject** %9, align 4
  %159 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %160 = load %union.jvalue*, %union.jvalue** %11, align 4
  call void %155(%struct.JNINativeInterface_** noundef %156, %struct._jobject* noundef %157, %struct._jobject* noundef %158, %struct._jmethodID* noundef %159, %union.jvalue* noundef %160)
  ret void
}

; Function Attrs: alwaysinline nounwind
define dso_local void @JNI_CallStaticVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jmethodID*, align 4
  %8 = alloca %union.jvalue*, align 4
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca %struct.JNINativeInterface_**, align 4
  %13 = alloca %struct._jobject*, align 4
  %14 = alloca %struct._jmethodID*, align 4
  %15 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %12, align 4
  store %struct._jobject* %1, %struct._jobject** %13, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %14, align 4
  %16 = bitcast %struct.__va_list* %15 to i8*
  call void @llvm.va_start(i8* %16)
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %12, align 4
  %18 = load %struct._jobject*, %struct._jobject** %13, align 4
  %19 = load %struct._jmethodID*, %struct._jmethodID** %14, align 4
  %20 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %15, i32 0, i32 0
  %21 = bitcast i8** %20 to [1 x i32]*
  %22 = load [1 x i32], [1 x i32]* %21, align 4
  %23 = call i8* @llvm.stacksave()
  %24 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %4, i32 0, i32 0
  %25 = bitcast i8** %24 to [1 x i32]*
  store [1 x i32] %22, [1 x i32]* %25, align 4
  store %struct.JNINativeInterface_** %17, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %18, %struct._jobject** %6, align 4
  store %struct._jmethodID* %19, %struct._jmethodID** %7, align 4
  %26 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %27 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %26, align 4
  %28 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %27, i32 0, i32 0
  %29 = load i8*, i8** %28, align 4
  %30 = bitcast i8* %29 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %31 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %32 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 0
  %34 = call i32 %30(%struct.JNINativeInterface_** noundef %31, %struct._jmethodID* noundef %32, i8* noundef %33) #2
  store i32 %34, i32* %10, align 4
  %35 = load i32, i32* %10, align 4
  %36 = mul i32 %35, 8
  %37 = alloca i8, i32 %36, align 8
  %38 = bitcast i8* %37 to %union.jvalue*
  store %union.jvalue* %38, %union.jvalue** %8, align 4
  store i32 0, i32* %11, align 4
  br label %39

39:                                               ; preds = %154, %3
  %40 = load i32, i32* %11, align 4
  %41 = load i32, i32* %10, align 4
  %42 = icmp slt i32 %40, %41
  br i1 %42, label %43, label %157

43:                                               ; preds = %39
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  switch i32 %47, label %154 [
    i32 90, label %48
    i32 66, label %59
    i32 83, label %70
    i32 67, label %81
    i32 73, label %93
    i32 74, label %103
    i32 68, label %114
    i32 70, label %128
    i32 76, label %143
  ]

48:                                               ; preds = %43
  %49 = bitcast %struct.__va_list* %4 to i8**
  %50 = load i8*, i8** %49, align 4
  %51 = getelementptr inbounds i8, i8* %50, i32 4
  store i8* %51, i8** %49, align 4
  %52 = bitcast i8* %50 to i32*
  %53 = load i32, i32* %52, align 4
  %54 = trunc i32 %53 to i8
  %55 = load %union.jvalue*, %union.jvalue** %8, align 4
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds %union.jvalue, %union.jvalue* %55, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %54, i8* %58, align 8
  br label %154

59:                                               ; preds = %43
  %60 = bitcast %struct.__va_list* %4 to i8**
  %61 = load i8*, i8** %60, align 4
  %62 = getelementptr inbounds i8, i8* %61, i32 4
  store i8* %62, i8** %60, align 4
  %63 = bitcast i8* %61 to i32*
  %64 = load i32, i32* %63, align 4
  %65 = trunc i32 %64 to i8
  %66 = load %union.jvalue*, %union.jvalue** %8, align 4
  %67 = load i32, i32* %11, align 4
  %68 = getelementptr inbounds %union.jvalue, %union.jvalue* %66, i32 %67
  %69 = bitcast %union.jvalue* %68 to i8*
  store i8 %65, i8* %69, align 8
  br label %154

70:                                               ; preds = %43
  %71 = bitcast %struct.__va_list* %4 to i8**
  %72 = load i8*, i8** %71, align 4
  %73 = getelementptr inbounds i8, i8* %72, i32 4
  store i8* %73, i8** %71, align 4
  %74 = bitcast i8* %72 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = load %union.jvalue*, %union.jvalue** %8, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds %union.jvalue, %union.jvalue* %77, i32 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %154

81:                                               ; preds = %43
  %82 = bitcast %struct.__va_list* %4 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = zext i16 %87 to i32
  %89 = load %union.jvalue*, %union.jvalue** %8, align 4
  %90 = load i32, i32* %11, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %154

93:                                               ; preds = %43
  %94 = bitcast %struct.__va_list* %4 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = load %union.jvalue*, %union.jvalue** %8, align 4
  %100 = load i32, i32* %11, align 4
  %101 = getelementptr inbounds %union.jvalue, %union.jvalue* %99, i32 %100
  %102 = bitcast %union.jvalue* %101 to i32*
  store i32 %98, i32* %102, align 8
  br label %154

103:                                              ; preds = %43
  %104 = bitcast %struct.__va_list* %4 to i8**
  %105 = load i8*, i8** %104, align 4
  %106 = getelementptr inbounds i8, i8* %105, i32 4
  store i8* %106, i8** %104, align 4
  %107 = bitcast i8* %105 to i32*
  %108 = load i32, i32* %107, align 4
  %109 = sext i32 %108 to i64
  %110 = load %union.jvalue*, %union.jvalue** %8, align 4
  %111 = load i32, i32* %11, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i32 %111
  %113 = bitcast %union.jvalue* %112 to i64*
  store i64 %109, i64* %113, align 8
  br label %154

114:                                              ; preds = %43
  %115 = bitcast %struct.__va_list* %4 to i8**
  %116 = load i8*, i8** %115, align 4
  %117 = ptrtoint i8* %116 to i32
  %118 = add i32 %117, 7
  %119 = and i32 %118, -8
  %120 = inttoptr i32 %119 to i8*
  %121 = getelementptr inbounds i8, i8* %120, i32 8
  store i8* %121, i8** %115, align 4
  %122 = bitcast i8* %120 to double*
  %123 = load double, double* %122, align 8
  %124 = load %union.jvalue*, %union.jvalue** %8, align 4
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %124, i32 %125
  %127 = bitcast %union.jvalue* %126 to double*
  store double %123, double* %127, align 8
  br label %154

128:                                              ; preds = %43
  %129 = bitcast %struct.__va_list* %4 to i8**
  %130 = load i8*, i8** %129, align 4
  %131 = ptrtoint i8* %130 to i32
  %132 = add i32 %131, 7
  %133 = and i32 %132, -8
  %134 = inttoptr i32 %133 to i8*
  %135 = getelementptr inbounds i8, i8* %134, i32 8
  store i8* %135, i8** %129, align 4
  %136 = bitcast i8* %134 to double*
  %137 = load double, double* %136, align 8
  %138 = fptrunc double %137 to float
  %139 = load %union.jvalue*, %union.jvalue** %8, align 4
  %140 = load i32, i32* %11, align 4
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %139, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %138, float* %142, align 8
  br label %154

143:                                              ; preds = %43
  %144 = bitcast %struct.__va_list* %4 to i8**
  %145 = load i8*, i8** %144, align 4
  %146 = getelementptr inbounds i8, i8* %145, i32 4
  store i8* %146, i8** %144, align 4
  %147 = bitcast i8* %145 to i8**
  %148 = load i8*, i8** %147, align 4
  %149 = bitcast i8* %148 to %struct._jobject*
  %150 = load %union.jvalue*, %union.jvalue** %8, align 4
  %151 = load i32, i32* %11, align 4
  %152 = getelementptr inbounds %union.jvalue, %union.jvalue* %150, i32 %151
  %153 = bitcast %union.jvalue* %152 to %struct._jobject**
  store %struct._jobject* %149, %struct._jobject** %153, align 8
  br label %154

154:                                              ; preds = %143, %128, %114, %103, %93, %81, %70, %59, %48, %43
  %155 = load i32, i32* %11, align 4
  %156 = add nsw i32 %155, 1
  store i32 %156, i32* %11, align 4
  br label %39, !llvm.loop !41

157:                                              ; preds = %39
  %158 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %159 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %158, align 4
  %160 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %159, i32 0, i32 143
  %161 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %160, align 4
  %162 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %163 = load %struct._jobject*, %struct._jobject** %6, align 4
  %164 = load %struct._jmethodID*, %struct._jmethodID** %7, align 4
  %165 = load %union.jvalue*, %union.jvalue** %8, align 4
  call void %161(%struct.JNINativeInterface_** noundef %162, %struct._jobject* noundef %163, %struct._jmethodID* noundef %164, %union.jvalue* noundef %165) #2
  call void @llvm.stackrestore(i8* %23)
  %166 = bitcast %struct.__va_list* %15 to i8*
  call void @llvm.va_end(i8* %166)
  ret void
}

; Function Attrs: alwaysinline nounwind
define dso_local void @JNI_CallStaticVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %union.jvalue*, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  br label %15

15:                                               ; preds = %4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  %25 = load i32, i32* %11, align 4
  %26 = mul i32 %25, 8
  %27 = alloca i8, i32 %26, align 8
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %9, align 4
  store i32 0, i32* %12, align 4
  br label %29

29:                                               ; preds = %145, %15
  %30 = load i32, i32* %12, align 4
  %31 = load i32, i32* %11, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %148

33:                                               ; preds = %29
  %34 = load i32, i32* %12, align 4
  %35 = getelementptr inbounds [257 x i8], [257 x i8]* %10, i32 0, i32 %34
  %36 = load i8, i8* %35, align 1
  %37 = zext i8 %36 to i32
  switch i32 %37, label %144 [
    i32 90, label %38
    i32 66, label %49
    i32 83, label %60
    i32 67, label %71
    i32 73, label %83
    i32 74, label %93
    i32 68, label %104
    i32 70, label %118
    i32 76, label %133
  ]

38:                                               ; preds = %33
  %39 = bitcast %struct.__va_list* %5 to i8**
  %40 = load i8*, i8** %39, align 4
  %41 = getelementptr inbounds i8, i8* %40, i32 4
  store i8* %41, i8** %39, align 4
  %42 = bitcast i8* %40 to i32*
  %43 = load i32, i32* %42, align 4
  %44 = trunc i32 %43 to i8
  %45 = load %union.jvalue*, %union.jvalue** %9, align 4
  %46 = load i32, i32* %12, align 4
  %47 = getelementptr inbounds %union.jvalue, %union.jvalue* %45, i32 %46
  %48 = bitcast %union.jvalue* %47 to i8*
  store i8 %44, i8* %48, align 8
  br label %144

49:                                               ; preds = %33
  %50 = bitcast %struct.__va_list* %5 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load %union.jvalue*, %union.jvalue** %9, align 4
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds %union.jvalue, %union.jvalue* %56, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %55, i8* %59, align 8
  br label %144

60:                                               ; preds = %33
  %61 = bitcast %struct.__va_list* %5 to i8**
  %62 = load i8*, i8** %61, align 4
  %63 = getelementptr inbounds i8, i8* %62, i32 4
  store i8* %63, i8** %61, align 4
  %64 = bitcast i8* %62 to i32*
  %65 = load i32, i32* %64, align 4
  %66 = trunc i32 %65 to i16
  %67 = load %union.jvalue*, %union.jvalue** %9, align 4
  %68 = load i32, i32* %12, align 4
  %69 = getelementptr inbounds %union.jvalue, %union.jvalue* %67, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %66, i16* %70, align 8
  br label %144

71:                                               ; preds = %33
  %72 = bitcast %struct.__va_list* %5 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = zext i16 %77 to i32
  %79 = load %union.jvalue*, %union.jvalue** %9, align 4
  %80 = load i32, i32* %12, align 4
  %81 = getelementptr inbounds %union.jvalue, %union.jvalue* %79, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %78, i32* %82, align 8
  br label %144

83:                                               ; preds = %33
  %84 = bitcast %struct.__va_list* %5 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = load %union.jvalue*, %union.jvalue** %9, align 4
  %90 = load i32, i32* %12, align 4
  %91 = getelementptr inbounds %union.jvalue, %union.jvalue* %89, i32 %90
  %92 = bitcast %union.jvalue* %91 to i32*
  store i32 %88, i32* %92, align 8
  br label %144

93:                                               ; preds = %33
  %94 = bitcast %struct.__va_list* %5 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = getelementptr inbounds i8, i8* %95, i32 4
  store i8* %96, i8** %94, align 4
  %97 = bitcast i8* %95 to i32*
  %98 = load i32, i32* %97, align 4
  %99 = sext i32 %98 to i64
  %100 = load %union.jvalue*, %union.jvalue** %9, align 4
  %101 = load i32, i32* %12, align 4
  %102 = getelementptr inbounds %union.jvalue, %union.jvalue* %100, i32 %101
  %103 = bitcast %union.jvalue* %102 to i64*
  store i64 %99, i64* %103, align 8
  br label %144

104:                                              ; preds = %33
  %105 = bitcast %struct.__va_list* %5 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load %union.jvalue*, %union.jvalue** %9, align 4
  %115 = load i32, i32* %12, align 4
  %116 = getelementptr inbounds %union.jvalue, %union.jvalue* %114, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %113, double* %117, align 8
  br label %144

118:                                              ; preds = %33
  %119 = bitcast %struct.__va_list* %5 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = ptrtoint i8* %120 to i32
  %122 = add i32 %121, 7
  %123 = and i32 %122, -8
  %124 = inttoptr i32 %123 to i8*
  %125 = getelementptr inbounds i8, i8* %124, i32 8
  store i8* %125, i8** %119, align 4
  %126 = bitcast i8* %124 to double*
  %127 = load double, double* %126, align 8
  %128 = fptrunc double %127 to float
  %129 = load %union.jvalue*, %union.jvalue** %9, align 4
  %130 = load i32, i32* %12, align 4
  %131 = getelementptr inbounds %union.jvalue, %union.jvalue* %129, i32 %130
  %132 = bitcast %union.jvalue* %131 to float*
  store float %128, float* %132, align 8
  br label %144

133:                                              ; preds = %33
  %134 = bitcast %struct.__va_list* %5 to i8**
  %135 = load i8*, i8** %134, align 4
  %136 = getelementptr inbounds i8, i8* %135, i32 4
  store i8* %136, i8** %134, align 4
  %137 = bitcast i8* %135 to i8**
  %138 = load i8*, i8** %137, align 4
  %139 = bitcast i8* %138 to %struct._jobject*
  %140 = load %union.jvalue*, %union.jvalue** %9, align 4
  %141 = load i32, i32* %12, align 4
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %140, i32 %141
  %143 = bitcast %union.jvalue* %142 to %struct._jobject**
  store %struct._jobject* %139, %struct._jobject** %143, align 8
  br label %144

144:                                              ; preds = %33, %133, %118, %104, %93, %83, %71, %60, %49, %38
  br label %145

145:                                              ; preds = %144
  %146 = load i32, i32* %12, align 4
  %147 = add nsw i32 %146, 1
  store i32 %147, i32* %12, align 4
  br label %29, !llvm.loop !41

148:                                              ; preds = %29
  br label %149

149:                                              ; preds = %148
  %150 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %151 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %150, align 4
  %152 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %151, i32 0, i32 143
  %153 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %152, align 4
  %154 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %155 = load %struct._jobject*, %struct._jobject** %7, align 4
  %156 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %157 = load %union.jvalue*, %union.jvalue** %9, align 4
  call void %153(%struct.JNINativeInterface_** noundef %154, %struct._jobject* noundef %155, %struct._jmethodID* noundef %156, %union.jvalue* noundef %157)
  ret void
}

; Function Attrs: nofree nosync nounwind willreturn
declare i8* @llvm.stacksave() #1

; Function Attrs: nofree nosync nounwind willreturn
declare void @llvm.stackrestore(i8*) #1

attributes #0 = { alwaysinline nounwind "frame-pointer"="all" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+dsp,+fp64,+vfp2,+vfp2sp,+vfp3d16,+vfp3d16sp,-aes,-d32,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-neon,-sha2,-thumb-mode,-vfp3,-vfp3sp,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }
attributes #1 = { nofree nosync nounwind willreturn }
attributes #2 = { nounwind }

!llvm.module.flags = !{!0, !1, !2, !3, !4, !5, !6, !7, !8}
!llvm.ident = !{!9}

!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 1, !"min_enum_size", i32 4}
!2 = !{i32 1, !"branch-target-enforcement", i32 0}
!3 = !{i32 1, !"sign-return-address", i32 0}
!4 = !{i32 1, !"sign-return-address-all", i32 0}
!5 = !{i32 1, !"sign-return-address-with-bkey", i32 0}
!6 = !{i32 7, !"PIC Level", i32 2}
!7 = !{i32 7, !"PIE Level", i32 2}
!8 = !{i32 7, !"frame-pointer", i32 2}
!9 = !{!"Ubuntu clang version 14.0.0-1ubuntu1"}
!10 = distinct !{!10, !11}
!11 = !{!"llvm.loop.mustprogress"}
!12 = distinct !{!12, !11}
!13 = distinct !{!13, !11}
!14 = distinct !{!14, !11}
!15 = distinct !{!15, !11}
!16 = distinct !{!16, !11}
!17 = distinct !{!17, !11}
!18 = distinct !{!18, !11}
!19 = distinct !{!19, !11}
!20 = distinct !{!20, !11}
!21 = distinct !{!21, !11}
!22 = distinct !{!22, !11}
!23 = distinct !{!23, !11}
!24 = distinct !{!24, !11}
!25 = distinct !{!25, !11}
!26 = distinct !{!26, !11}
!27 = distinct !{!27, !11}
!28 = distinct !{!28, !11}
!29 = distinct !{!29, !11}
!30 = distinct !{!30, !11}
!31 = distinct !{!31, !11}
!32 = distinct !{!32, !11}
!33 = distinct !{!33, !11}
!34 = distinct !{!34, !11}
!35 = distinct !{!35, !11}
!36 = distinct !{!36, !11}
!37 = distinct !{!37, !11}
!38 = distinct !{!38, !11}
!39 = distinct !{!39, !11}
!40 = distinct !{!40, !11}
!41 = distinct !{!41, !11}
