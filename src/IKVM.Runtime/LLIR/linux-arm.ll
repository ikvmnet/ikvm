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

; Function Attrs: noinline nounwind optnone
define dso_local %struct._jobject* @JNI_CallObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca %struct._jobject*, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %13 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %13)
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 4
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %21 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %130, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %9, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %133

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  switch i32 %31, label %128 [
    i32 90, label %32
    i32 66, label %42
    i32 67, label %52
    i32 83, label %62
    i32 73, label %72
    i32 74, label %81
    i32 70, label %91
    i32 68, label %105
    i32 76, label %118
  ]

32:                                               ; preds = %27
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %129

42:                                               ; preds = %27
  %43 = bitcast %struct.__va_list* %7 to i8**
  %44 = load i8*, i8** %43, align 4
  %45 = getelementptr inbounds i8, i8* %44, i32 4
  store i8* %45, i8** %43, align 4
  %46 = bitcast i8* %44 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %11, align 4
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %48, i8* %51, align 8
  br label %129

52:                                               ; preds = %27
  %53 = bitcast %struct.__va_list* %7 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i16
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i16*
  store i16 %58, i16* %61, align 8
  br label %129

62:                                               ; preds = %27
  %63 = bitcast %struct.__va_list* %7 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load i32, i32* %11, align 4
  %70 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %69
  %71 = bitcast %union.jvalue* %70 to i16*
  store i16 %68, i16* %71, align 8
  br label %129

72:                                               ; preds = %27
  %73 = bitcast %struct.__va_list* %7 to i8**
  %74 = load i8*, i8** %73, align 4
  %75 = getelementptr inbounds i8, i8* %74, i32 4
  store i8* %75, i8** %73, align 4
  %76 = bitcast i8* %74 to i32*
  %77 = load i32, i32* %76, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %78
  %80 = bitcast %union.jvalue* %79 to i32*
  store i32 %77, i32* %80, align 8
  br label %129

81:                                               ; preds = %27
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = sext i32 %86 to i64
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i64*
  store i64 %87, i64* %90, align 8
  br label %129

91:                                               ; preds = %27
  %92 = bitcast %struct.__va_list* %7 to i8**
  %93 = load i8*, i8** %92, align 4
  %94 = ptrtoint i8* %93 to i32
  %95 = add i32 %94, 7
  %96 = and i32 %95, -8
  %97 = inttoptr i32 %96 to i8*
  %98 = getelementptr inbounds i8, i8* %97, i32 8
  store i8* %98, i8** %92, align 4
  %99 = bitcast i8* %97 to double*
  %100 = load double, double* %99, align 8
  %101 = fptrunc double %100 to float
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to float*
  store float %101, float* %104, align 8
  br label %129

105:                                              ; preds = %27
  %106 = bitcast %struct.__va_list* %7 to i8**
  %107 = load i8*, i8** %106, align 4
  %108 = ptrtoint i8* %107 to i32
  %109 = add i32 %108, 7
  %110 = and i32 %109, -8
  %111 = inttoptr i32 %110 to i8*
  %112 = getelementptr inbounds i8, i8* %111, i32 8
  store i8* %112, i8** %106, align 4
  %113 = bitcast i8* %111 to double*
  %114 = load double, double* %113, align 8
  %115 = load i32, i32* %11, align 4
  %116 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %114, double* %117, align 8
  br label %129

118:                                              ; preds = %27
  %119 = bitcast %struct.__va_list* %7 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = getelementptr inbounds i8, i8* %120, i32 4
  store i8* %121, i8** %119, align 4
  %122 = bitcast i8* %120 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = bitcast i8* %123 to %struct._jobject*
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %125
  %127 = bitcast %union.jvalue* %126 to %struct._jobject**
  store %struct._jobject* %124, %struct._jobject** %127, align 8
  br label %129

128:                                              ; preds = %27
  br label %129

129:                                              ; preds = %128, %118, %105, %91, %81, %72, %62, %52, %42, %32
  br label %130

130:                                              ; preds = %129
  %131 = load i32, i32* %11, align 4
  %132 = add nsw i32 %131, 1
  store i32 %132, i32* %11, align 4
  br label %23, !llvm.loop !10

133:                                              ; preds = %23
  %134 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %135 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %134, align 4
  %136 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %135, i32 0, i32 36
  %137 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %136, align 4
  %138 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %139 = load %struct._jobject*, %struct._jobject** %5, align 4
  %140 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %142 = call %struct._jobject* %137(%struct.JNINativeInterface_** noundef %138, %struct._jobject* noundef %139, %struct._jmethodID* noundef %140, %union.jvalue* noundef %141)
  store %struct._jobject* %142, %struct._jobject** %12, align 4
  %143 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %143)
  %144 = load %struct._jobject*, %struct._jobject** %12, align 4
  ret %struct._jobject* %144
}

; Function Attrs: nofree nosync nounwind willreturn
declare void @llvm.va_start(i8*) #1

; Function Attrs: nofree nosync nounwind willreturn
declare void @llvm.va_end(i8*) #1

; Function Attrs: noinline nounwind optnone
define dso_local %struct._jobject* @JNI_CallObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !12

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 36
  %138 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %143 = call %struct._jobject* %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret %struct._jobject* %143
}

; Function Attrs: noinline nounwind optnone
define dso_local %struct._jobject* @JNI_CallNonvirtualObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct.__va_list, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca %struct._jobject*, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %1, %struct._jobject** %6, align 4
  store %struct._jobject* %2, %struct._jobject** %7, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 4
  %15 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_start(i8* %15)
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  store i32 0, i32* %13, align 4
  br label %25

25:                                               ; preds = %132, %4
  %26 = load i32, i32* %13, align 4
  %27 = load i32, i32* %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %135

29:                                               ; preds = %25
  %30 = load i32, i32* %13, align 4
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %30
  %32 = load i8, i8* %31, align 1
  %33 = zext i8 %32 to i32
  switch i32 %33, label %130 [
    i32 90, label %34
    i32 66, label %44
    i32 67, label %54
    i32 83, label %64
    i32 73, label %74
    i32 74, label %83
    i32 70, label %93
    i32 68, label %107
    i32 76, label %120
  ]

34:                                               ; preds = %29
  %35 = bitcast %struct.__va_list* %9 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %13, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %131

44:                                               ; preds = %29
  %45 = bitcast %struct.__va_list* %9 to i8**
  %46 = load i8*, i8** %45, align 4
  %47 = getelementptr inbounds i8, i8* %46, i32 4
  store i8* %47, i8** %45, align 4
  %48 = bitcast i8* %46 to i32*
  %49 = load i32, i32* %48, align 4
  %50 = trunc i32 %49 to i8
  %51 = load i32, i32* %13, align 4
  %52 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %51
  %53 = bitcast %union.jvalue* %52 to i8*
  store i8 %50, i8* %53, align 8
  br label %131

54:                                               ; preds = %29
  %55 = bitcast %struct.__va_list* %9 to i8**
  %56 = load i8*, i8** %55, align 4
  %57 = getelementptr inbounds i8, i8* %56, i32 4
  store i8* %57, i8** %55, align 4
  %58 = bitcast i8* %56 to i32*
  %59 = load i32, i32* %58, align 4
  %60 = trunc i32 %59 to i16
  %61 = load i32, i32* %13, align 4
  %62 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %61
  %63 = bitcast %union.jvalue* %62 to i16*
  store i16 %60, i16* %63, align 8
  br label %131

64:                                               ; preds = %29
  %65 = bitcast %struct.__va_list* %9 to i8**
  %66 = load i8*, i8** %65, align 4
  %67 = getelementptr inbounds i8, i8* %66, i32 4
  store i8* %67, i8** %65, align 4
  %68 = bitcast i8* %66 to i32*
  %69 = load i32, i32* %68, align 4
  %70 = trunc i32 %69 to i16
  %71 = load i32, i32* %13, align 4
  %72 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %71
  %73 = bitcast %union.jvalue* %72 to i16*
  store i16 %70, i16* %73, align 8
  br label %131

74:                                               ; preds = %29
  %75 = bitcast %struct.__va_list* %9 to i8**
  %76 = load i8*, i8** %75, align 4
  %77 = getelementptr inbounds i8, i8* %76, i32 4
  store i8* %77, i8** %75, align 4
  %78 = bitcast i8* %76 to i32*
  %79 = load i32, i32* %78, align 4
  %80 = load i32, i32* %13, align 4
  %81 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %79, i32* %82, align 8
  br label %131

83:                                               ; preds = %29
  %84 = bitcast %struct.__va_list* %9 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = sext i32 %88 to i64
  %90 = load i32, i32* %13, align 4
  %91 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %90
  %92 = bitcast %union.jvalue* %91 to i64*
  store i64 %89, i64* %92, align 8
  br label %131

93:                                               ; preds = %29
  %94 = bitcast %struct.__va_list* %9 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = ptrtoint i8* %95 to i32
  %97 = add i32 %96, 7
  %98 = and i32 %97, -8
  %99 = inttoptr i32 %98 to i8*
  %100 = getelementptr inbounds i8, i8* %99, i32 8
  store i8* %100, i8** %94, align 4
  %101 = bitcast i8* %99 to double*
  %102 = load double, double* %101, align 8
  %103 = fptrunc double %102 to float
  %104 = load i32, i32* %13, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to float*
  store float %103, float* %106, align 8
  br label %131

107:                                              ; preds = %29
  %108 = bitcast %struct.__va_list* %9 to i8**
  %109 = load i8*, i8** %108, align 4
  %110 = ptrtoint i8* %109 to i32
  %111 = add i32 %110, 7
  %112 = and i32 %111, -8
  %113 = inttoptr i32 %112 to i8*
  %114 = getelementptr inbounds i8, i8* %113, i32 8
  store i8* %114, i8** %108, align 4
  %115 = bitcast i8* %113 to double*
  %116 = load double, double* %115, align 8
  %117 = load i32, i32* %13, align 4
  %118 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %117
  %119 = bitcast %union.jvalue* %118 to double*
  store double %116, double* %119, align 8
  br label %131

120:                                              ; preds = %29
  %121 = bitcast %struct.__va_list* %9 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = getelementptr inbounds i8, i8* %122, i32 4
  store i8* %123, i8** %121, align 4
  %124 = bitcast i8* %122 to i8**
  %125 = load i8*, i8** %124, align 4
  %126 = bitcast i8* %125 to %struct._jobject*
  %127 = load i32, i32* %13, align 4
  %128 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %127
  %129 = bitcast %union.jvalue* %128 to %struct._jobject**
  store %struct._jobject* %126, %struct._jobject** %129, align 8
  br label %131

130:                                              ; preds = %29
  br label %131

131:                                              ; preds = %130, %120, %107, %93, %83, %74, %64, %54, %44, %34
  br label %132

132:                                              ; preds = %131
  %133 = load i32, i32* %13, align 4
  %134 = add nsw i32 %133, 1
  store i32 %134, i32* %13, align 4
  br label %25, !llvm.loop !13

135:                                              ; preds = %25
  %136 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %137 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %136, align 4
  %138 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %137, i32 0, i32 66
  %139 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %138, align 4
  %140 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %141 = load %struct._jobject*, %struct._jobject** %6, align 4
  %142 = load %struct._jobject*, %struct._jobject** %7, align 4
  %143 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %144 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 0
  %145 = call %struct._jobject* %139(%struct.JNINativeInterface_** noundef %140, %struct._jobject* noundef %141, %struct._jobject* noundef %142, %struct._jmethodID* noundef %143, %union.jvalue* noundef %144)
  store %struct._jobject* %145, %struct._jobject** %14, align 4
  %146 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %146)
  %147 = load %struct._jobject*, %struct._jobject** %14, align 4
  ret %struct._jobject* %147
}

; Function Attrs: noinline nounwind optnone
define dso_local %struct._jobject* @JNI_CallNonvirtualObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  %15 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %6, i32 0, i32 0
  %16 = bitcast i8** %15 to [1 x i32]*
  store [1 x i32] %4, [1 x i32]* %16, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %7, align 4
  store %struct._jobject* %1, %struct._jobject** %8, align 4
  store %struct._jobject* %2, %struct._jobject** %9, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %10, align 4
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %18 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %17, align 4
  %19 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %18, i32 0, i32 0
  %20 = load i8*, i8** %19, align 4
  %21 = bitcast i8* %20 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %23 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %24 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 0
  %25 = call i32 %21(%struct.JNINativeInterface_** noundef %22, %struct._jmethodID* noundef %23, i8* noundef %24)
  store i32 %25, i32* %12, align 4
  store i32 0, i32* %14, align 4
  br label %26

26:                                               ; preds = %133, %5
  %27 = load i32, i32* %14, align 4
  %28 = load i32, i32* %12, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %136

30:                                               ; preds = %26
  %31 = load i32, i32* %14, align 4
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %31
  %33 = load i8, i8* %32, align 1
  %34 = zext i8 %33 to i32
  switch i32 %34, label %131 [
    i32 90, label %35
    i32 66, label %45
    i32 67, label %55
    i32 83, label %65
    i32 73, label %75
    i32 74, label %84
    i32 70, label %94
    i32 68, label %108
    i32 76, label %121
  ]

35:                                               ; preds = %30
  %36 = bitcast %struct.__va_list* %6 to i8**
  %37 = load i8*, i8** %36, align 4
  %38 = getelementptr inbounds i8, i8* %37, i32 4
  store i8* %38, i8** %36, align 4
  %39 = bitcast i8* %37 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = load i32, i32* %14, align 4
  %43 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %42
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %41, i8* %44, align 8
  br label %132

45:                                               ; preds = %30
  %46 = bitcast %struct.__va_list* %6 to i8**
  %47 = load i8*, i8** %46, align 4
  %48 = getelementptr inbounds i8, i8* %47, i32 4
  store i8* %48, i8** %46, align 4
  %49 = bitcast i8* %47 to i32*
  %50 = load i32, i32* %49, align 4
  %51 = trunc i32 %50 to i8
  %52 = load i32, i32* %14, align 4
  %53 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %52
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %51, i8* %54, align 8
  br label %132

55:                                               ; preds = %30
  %56 = bitcast %struct.__va_list* %6 to i8**
  %57 = load i8*, i8** %56, align 4
  %58 = getelementptr inbounds i8, i8* %57, i32 4
  store i8* %58, i8** %56, align 4
  %59 = bitcast i8* %57 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = trunc i32 %60 to i16
  %62 = load i32, i32* %14, align 4
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %62
  %64 = bitcast %union.jvalue* %63 to i16*
  store i16 %61, i16* %64, align 8
  br label %132

65:                                               ; preds = %30
  %66 = bitcast %struct.__va_list* %6 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %14, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %132

75:                                               ; preds = %30
  %76 = bitcast %struct.__va_list* %6 to i8**
  %77 = load i8*, i8** %76, align 4
  %78 = getelementptr inbounds i8, i8* %77, i32 4
  store i8* %78, i8** %76, align 4
  %79 = bitcast i8* %77 to i32*
  %80 = load i32, i32* %79, align 4
  %81 = load i32, i32* %14, align 4
  %82 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %81
  %83 = bitcast %union.jvalue* %82 to i32*
  store i32 %80, i32* %83, align 8
  br label %132

84:                                               ; preds = %30
  %85 = bitcast %struct.__va_list* %6 to i8**
  %86 = load i8*, i8** %85, align 4
  %87 = getelementptr inbounds i8, i8* %86, i32 4
  store i8* %87, i8** %85, align 4
  %88 = bitcast i8* %86 to i32*
  %89 = load i32, i32* %88, align 4
  %90 = sext i32 %89 to i64
  %91 = load i32, i32* %14, align 4
  %92 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %91
  %93 = bitcast %union.jvalue* %92 to i64*
  store i64 %90, i64* %93, align 8
  br label %132

94:                                               ; preds = %30
  %95 = bitcast %struct.__va_list* %6 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = ptrtoint i8* %96 to i32
  %98 = add i32 %97, 7
  %99 = and i32 %98, -8
  %100 = inttoptr i32 %99 to i8*
  %101 = getelementptr inbounds i8, i8* %100, i32 8
  store i8* %101, i8** %95, align 4
  %102 = bitcast i8* %100 to double*
  %103 = load double, double* %102, align 8
  %104 = fptrunc double %103 to float
  %105 = load i32, i32* %14, align 4
  %106 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %105
  %107 = bitcast %union.jvalue* %106 to float*
  store float %104, float* %107, align 8
  br label %132

108:                                              ; preds = %30
  %109 = bitcast %struct.__va_list* %6 to i8**
  %110 = load i8*, i8** %109, align 4
  %111 = ptrtoint i8* %110 to i32
  %112 = add i32 %111, 7
  %113 = and i32 %112, -8
  %114 = inttoptr i32 %113 to i8*
  %115 = getelementptr inbounds i8, i8* %114, i32 8
  store i8* %115, i8** %109, align 4
  %116 = bitcast i8* %114 to double*
  %117 = load double, double* %116, align 8
  %118 = load i32, i32* %14, align 4
  %119 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %118
  %120 = bitcast %union.jvalue* %119 to double*
  store double %117, double* %120, align 8
  br label %132

121:                                              ; preds = %30
  %122 = bitcast %struct.__va_list* %6 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = getelementptr inbounds i8, i8* %123, i32 4
  store i8* %124, i8** %122, align 4
  %125 = bitcast i8* %123 to i8**
  %126 = load i8*, i8** %125, align 4
  %127 = bitcast i8* %126 to %struct._jobject*
  %128 = load i32, i32* %14, align 4
  %129 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %128
  %130 = bitcast %union.jvalue* %129 to %struct._jobject**
  store %struct._jobject* %127, %struct._jobject** %130, align 8
  br label %132

131:                                              ; preds = %30
  br label %132

132:                                              ; preds = %131, %121, %108, %94, %84, %75, %65, %55, %45, %35
  br label %133

133:                                              ; preds = %132
  %134 = load i32, i32* %14, align 4
  %135 = add nsw i32 %134, 1
  store i32 %135, i32* %14, align 4
  br label %26, !llvm.loop !14

136:                                              ; preds = %26
  %137 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %138 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %137, align 4
  %139 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %138, i32 0, i32 66
  %140 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %139, align 4
  %141 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %142 = load %struct._jobject*, %struct._jobject** %8, align 4
  %143 = load %struct._jobject*, %struct._jobject** %9, align 4
  %144 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %145 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 0
  %146 = call %struct._jobject* %140(%struct.JNINativeInterface_** noundef %141, %struct._jobject* noundef %142, %struct._jobject* noundef %143, %struct._jmethodID* noundef %144, %union.jvalue* noundef %145)
  ret %struct._jobject* %146
}

; Function Attrs: noinline nounwind optnone
define dso_local %struct._jobject* @JNI_CallStaticObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca %struct._jobject*, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %13 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %13)
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 4
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %21 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %130, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %9, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %133

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  switch i32 %31, label %128 [
    i32 90, label %32
    i32 66, label %42
    i32 67, label %52
    i32 83, label %62
    i32 73, label %72
    i32 74, label %81
    i32 70, label %91
    i32 68, label %105
    i32 76, label %118
  ]

32:                                               ; preds = %27
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %129

42:                                               ; preds = %27
  %43 = bitcast %struct.__va_list* %7 to i8**
  %44 = load i8*, i8** %43, align 4
  %45 = getelementptr inbounds i8, i8* %44, i32 4
  store i8* %45, i8** %43, align 4
  %46 = bitcast i8* %44 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %11, align 4
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %48, i8* %51, align 8
  br label %129

52:                                               ; preds = %27
  %53 = bitcast %struct.__va_list* %7 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i16
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i16*
  store i16 %58, i16* %61, align 8
  br label %129

62:                                               ; preds = %27
  %63 = bitcast %struct.__va_list* %7 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load i32, i32* %11, align 4
  %70 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %69
  %71 = bitcast %union.jvalue* %70 to i16*
  store i16 %68, i16* %71, align 8
  br label %129

72:                                               ; preds = %27
  %73 = bitcast %struct.__va_list* %7 to i8**
  %74 = load i8*, i8** %73, align 4
  %75 = getelementptr inbounds i8, i8* %74, i32 4
  store i8* %75, i8** %73, align 4
  %76 = bitcast i8* %74 to i32*
  %77 = load i32, i32* %76, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %78
  %80 = bitcast %union.jvalue* %79 to i32*
  store i32 %77, i32* %80, align 8
  br label %129

81:                                               ; preds = %27
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = sext i32 %86 to i64
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i64*
  store i64 %87, i64* %90, align 8
  br label %129

91:                                               ; preds = %27
  %92 = bitcast %struct.__va_list* %7 to i8**
  %93 = load i8*, i8** %92, align 4
  %94 = ptrtoint i8* %93 to i32
  %95 = add i32 %94, 7
  %96 = and i32 %95, -8
  %97 = inttoptr i32 %96 to i8*
  %98 = getelementptr inbounds i8, i8* %97, i32 8
  store i8* %98, i8** %92, align 4
  %99 = bitcast i8* %97 to double*
  %100 = load double, double* %99, align 8
  %101 = fptrunc double %100 to float
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to float*
  store float %101, float* %104, align 8
  br label %129

105:                                              ; preds = %27
  %106 = bitcast %struct.__va_list* %7 to i8**
  %107 = load i8*, i8** %106, align 4
  %108 = ptrtoint i8* %107 to i32
  %109 = add i32 %108, 7
  %110 = and i32 %109, -8
  %111 = inttoptr i32 %110 to i8*
  %112 = getelementptr inbounds i8, i8* %111, i32 8
  store i8* %112, i8** %106, align 4
  %113 = bitcast i8* %111 to double*
  %114 = load double, double* %113, align 8
  %115 = load i32, i32* %11, align 4
  %116 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %114, double* %117, align 8
  br label %129

118:                                              ; preds = %27
  %119 = bitcast %struct.__va_list* %7 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = getelementptr inbounds i8, i8* %120, i32 4
  store i8* %121, i8** %119, align 4
  %122 = bitcast i8* %120 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = bitcast i8* %123 to %struct._jobject*
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %125
  %127 = bitcast %union.jvalue* %126 to %struct._jobject**
  store %struct._jobject* %124, %struct._jobject** %127, align 8
  br label %129

128:                                              ; preds = %27
  br label %129

129:                                              ; preds = %128, %118, %105, %91, %81, %72, %62, %52, %42, %32
  br label %130

130:                                              ; preds = %129
  %131 = load i32, i32* %11, align 4
  %132 = add nsw i32 %131, 1
  store i32 %132, i32* %11, align 4
  br label %23, !llvm.loop !15

133:                                              ; preds = %23
  %134 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %135 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %134, align 4
  %136 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %135, i32 0, i32 116
  %137 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %136, align 4
  %138 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %139 = load %struct._jobject*, %struct._jobject** %5, align 4
  %140 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %142 = call %struct._jobject* %137(%struct.JNINativeInterface_** noundef %138, %struct._jobject* noundef %139, %struct._jmethodID* noundef %140, %union.jvalue* noundef %141)
  store %struct._jobject* %142, %struct._jobject** %12, align 4
  %143 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %143)
  %144 = load %struct._jobject*, %struct._jobject** %12, align 4
  ret %struct._jobject* %144
}

; Function Attrs: noinline nounwind optnone
define dso_local %struct._jobject* @JNI_CallStaticObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !16

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 116
  %138 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %143 = call %struct._jobject* %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret %struct._jobject* %143
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i8 @JNI_CallBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i8, align 1
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %13 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %13)
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 4
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %21 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %130, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %9, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %133

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  switch i32 %31, label %128 [
    i32 90, label %32
    i32 66, label %42
    i32 67, label %52
    i32 83, label %62
    i32 73, label %72
    i32 74, label %81
    i32 70, label %91
    i32 68, label %105
    i32 76, label %118
  ]

32:                                               ; preds = %27
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %129

42:                                               ; preds = %27
  %43 = bitcast %struct.__va_list* %7 to i8**
  %44 = load i8*, i8** %43, align 4
  %45 = getelementptr inbounds i8, i8* %44, i32 4
  store i8* %45, i8** %43, align 4
  %46 = bitcast i8* %44 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %11, align 4
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %48, i8* %51, align 8
  br label %129

52:                                               ; preds = %27
  %53 = bitcast %struct.__va_list* %7 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i16
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i16*
  store i16 %58, i16* %61, align 8
  br label %129

62:                                               ; preds = %27
  %63 = bitcast %struct.__va_list* %7 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load i32, i32* %11, align 4
  %70 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %69
  %71 = bitcast %union.jvalue* %70 to i16*
  store i16 %68, i16* %71, align 8
  br label %129

72:                                               ; preds = %27
  %73 = bitcast %struct.__va_list* %7 to i8**
  %74 = load i8*, i8** %73, align 4
  %75 = getelementptr inbounds i8, i8* %74, i32 4
  store i8* %75, i8** %73, align 4
  %76 = bitcast i8* %74 to i32*
  %77 = load i32, i32* %76, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %78
  %80 = bitcast %union.jvalue* %79 to i32*
  store i32 %77, i32* %80, align 8
  br label %129

81:                                               ; preds = %27
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = sext i32 %86 to i64
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i64*
  store i64 %87, i64* %90, align 8
  br label %129

91:                                               ; preds = %27
  %92 = bitcast %struct.__va_list* %7 to i8**
  %93 = load i8*, i8** %92, align 4
  %94 = ptrtoint i8* %93 to i32
  %95 = add i32 %94, 7
  %96 = and i32 %95, -8
  %97 = inttoptr i32 %96 to i8*
  %98 = getelementptr inbounds i8, i8* %97, i32 8
  store i8* %98, i8** %92, align 4
  %99 = bitcast i8* %97 to double*
  %100 = load double, double* %99, align 8
  %101 = fptrunc double %100 to float
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to float*
  store float %101, float* %104, align 8
  br label %129

105:                                              ; preds = %27
  %106 = bitcast %struct.__va_list* %7 to i8**
  %107 = load i8*, i8** %106, align 4
  %108 = ptrtoint i8* %107 to i32
  %109 = add i32 %108, 7
  %110 = and i32 %109, -8
  %111 = inttoptr i32 %110 to i8*
  %112 = getelementptr inbounds i8, i8* %111, i32 8
  store i8* %112, i8** %106, align 4
  %113 = bitcast i8* %111 to double*
  %114 = load double, double* %113, align 8
  %115 = load i32, i32* %11, align 4
  %116 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %114, double* %117, align 8
  br label %129

118:                                              ; preds = %27
  %119 = bitcast %struct.__va_list* %7 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = getelementptr inbounds i8, i8* %120, i32 4
  store i8* %121, i8** %119, align 4
  %122 = bitcast i8* %120 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = bitcast i8* %123 to %struct._jobject*
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %125
  %127 = bitcast %union.jvalue* %126 to %struct._jobject**
  store %struct._jobject* %124, %struct._jobject** %127, align 8
  br label %129

128:                                              ; preds = %27
  br label %129

129:                                              ; preds = %128, %118, %105, %91, %81, %72, %62, %52, %42, %32
  br label %130

130:                                              ; preds = %129
  %131 = load i32, i32* %11, align 4
  %132 = add nsw i32 %131, 1
  store i32 %132, i32* %11, align 4
  br label %23, !llvm.loop !17

133:                                              ; preds = %23
  %134 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %135 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %134, align 4
  %136 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %135, i32 0, i32 39
  %137 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %136, align 4
  %138 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %139 = load %struct._jobject*, %struct._jobject** %5, align 4
  %140 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %142 = call zeroext i8 %137(%struct.JNINativeInterface_** noundef %138, %struct._jobject* noundef %139, %struct._jmethodID* noundef %140, %union.jvalue* noundef %141)
  store i8 %142, i8* %12, align 1
  %143 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %143)
  %144 = load i8, i8* %12, align 1
  ret i8 %144
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i8 @JNI_CallBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !18

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 39
  %138 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %143 = call zeroext i8 %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret i8 %143
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i8 @JNI_CallNonvirtualBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct.__va_list, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i8, align 1
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %1, %struct._jobject** %6, align 4
  store %struct._jobject* %2, %struct._jobject** %7, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 4
  %15 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_start(i8* %15)
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  store i32 0, i32* %13, align 4
  br label %25

25:                                               ; preds = %132, %4
  %26 = load i32, i32* %13, align 4
  %27 = load i32, i32* %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %135

29:                                               ; preds = %25
  %30 = load i32, i32* %13, align 4
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %30
  %32 = load i8, i8* %31, align 1
  %33 = zext i8 %32 to i32
  switch i32 %33, label %130 [
    i32 90, label %34
    i32 66, label %44
    i32 67, label %54
    i32 83, label %64
    i32 73, label %74
    i32 74, label %83
    i32 70, label %93
    i32 68, label %107
    i32 76, label %120
  ]

34:                                               ; preds = %29
  %35 = bitcast %struct.__va_list* %9 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %13, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %131

44:                                               ; preds = %29
  %45 = bitcast %struct.__va_list* %9 to i8**
  %46 = load i8*, i8** %45, align 4
  %47 = getelementptr inbounds i8, i8* %46, i32 4
  store i8* %47, i8** %45, align 4
  %48 = bitcast i8* %46 to i32*
  %49 = load i32, i32* %48, align 4
  %50 = trunc i32 %49 to i8
  %51 = load i32, i32* %13, align 4
  %52 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %51
  %53 = bitcast %union.jvalue* %52 to i8*
  store i8 %50, i8* %53, align 8
  br label %131

54:                                               ; preds = %29
  %55 = bitcast %struct.__va_list* %9 to i8**
  %56 = load i8*, i8** %55, align 4
  %57 = getelementptr inbounds i8, i8* %56, i32 4
  store i8* %57, i8** %55, align 4
  %58 = bitcast i8* %56 to i32*
  %59 = load i32, i32* %58, align 4
  %60 = trunc i32 %59 to i16
  %61 = load i32, i32* %13, align 4
  %62 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %61
  %63 = bitcast %union.jvalue* %62 to i16*
  store i16 %60, i16* %63, align 8
  br label %131

64:                                               ; preds = %29
  %65 = bitcast %struct.__va_list* %9 to i8**
  %66 = load i8*, i8** %65, align 4
  %67 = getelementptr inbounds i8, i8* %66, i32 4
  store i8* %67, i8** %65, align 4
  %68 = bitcast i8* %66 to i32*
  %69 = load i32, i32* %68, align 4
  %70 = trunc i32 %69 to i16
  %71 = load i32, i32* %13, align 4
  %72 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %71
  %73 = bitcast %union.jvalue* %72 to i16*
  store i16 %70, i16* %73, align 8
  br label %131

74:                                               ; preds = %29
  %75 = bitcast %struct.__va_list* %9 to i8**
  %76 = load i8*, i8** %75, align 4
  %77 = getelementptr inbounds i8, i8* %76, i32 4
  store i8* %77, i8** %75, align 4
  %78 = bitcast i8* %76 to i32*
  %79 = load i32, i32* %78, align 4
  %80 = load i32, i32* %13, align 4
  %81 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %79, i32* %82, align 8
  br label %131

83:                                               ; preds = %29
  %84 = bitcast %struct.__va_list* %9 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = sext i32 %88 to i64
  %90 = load i32, i32* %13, align 4
  %91 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %90
  %92 = bitcast %union.jvalue* %91 to i64*
  store i64 %89, i64* %92, align 8
  br label %131

93:                                               ; preds = %29
  %94 = bitcast %struct.__va_list* %9 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = ptrtoint i8* %95 to i32
  %97 = add i32 %96, 7
  %98 = and i32 %97, -8
  %99 = inttoptr i32 %98 to i8*
  %100 = getelementptr inbounds i8, i8* %99, i32 8
  store i8* %100, i8** %94, align 4
  %101 = bitcast i8* %99 to double*
  %102 = load double, double* %101, align 8
  %103 = fptrunc double %102 to float
  %104 = load i32, i32* %13, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to float*
  store float %103, float* %106, align 8
  br label %131

107:                                              ; preds = %29
  %108 = bitcast %struct.__va_list* %9 to i8**
  %109 = load i8*, i8** %108, align 4
  %110 = ptrtoint i8* %109 to i32
  %111 = add i32 %110, 7
  %112 = and i32 %111, -8
  %113 = inttoptr i32 %112 to i8*
  %114 = getelementptr inbounds i8, i8* %113, i32 8
  store i8* %114, i8** %108, align 4
  %115 = bitcast i8* %113 to double*
  %116 = load double, double* %115, align 8
  %117 = load i32, i32* %13, align 4
  %118 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %117
  %119 = bitcast %union.jvalue* %118 to double*
  store double %116, double* %119, align 8
  br label %131

120:                                              ; preds = %29
  %121 = bitcast %struct.__va_list* %9 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = getelementptr inbounds i8, i8* %122, i32 4
  store i8* %123, i8** %121, align 4
  %124 = bitcast i8* %122 to i8**
  %125 = load i8*, i8** %124, align 4
  %126 = bitcast i8* %125 to %struct._jobject*
  %127 = load i32, i32* %13, align 4
  %128 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %127
  %129 = bitcast %union.jvalue* %128 to %struct._jobject**
  store %struct._jobject* %126, %struct._jobject** %129, align 8
  br label %131

130:                                              ; preds = %29
  br label %131

131:                                              ; preds = %130, %120, %107, %93, %83, %74, %64, %54, %44, %34
  br label %132

132:                                              ; preds = %131
  %133 = load i32, i32* %13, align 4
  %134 = add nsw i32 %133, 1
  store i32 %134, i32* %13, align 4
  br label %25, !llvm.loop !19

135:                                              ; preds = %25
  %136 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %137 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %136, align 4
  %138 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %137, i32 0, i32 69
  %139 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %138, align 4
  %140 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %141 = load %struct._jobject*, %struct._jobject** %6, align 4
  %142 = load %struct._jobject*, %struct._jobject** %7, align 4
  %143 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %144 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 0
  %145 = call zeroext i8 %139(%struct.JNINativeInterface_** noundef %140, %struct._jobject* noundef %141, %struct._jobject* noundef %142, %struct._jmethodID* noundef %143, %union.jvalue* noundef %144)
  store i8 %145, i8* %14, align 1
  %146 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %146)
  %147 = load i8, i8* %14, align 1
  ret i8 %147
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i8 @JNI_CallNonvirtualBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  %15 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %6, i32 0, i32 0
  %16 = bitcast i8** %15 to [1 x i32]*
  store [1 x i32] %4, [1 x i32]* %16, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %7, align 4
  store %struct._jobject* %1, %struct._jobject** %8, align 4
  store %struct._jobject* %2, %struct._jobject** %9, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %10, align 4
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %18 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %17, align 4
  %19 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %18, i32 0, i32 0
  %20 = load i8*, i8** %19, align 4
  %21 = bitcast i8* %20 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %23 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %24 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 0
  %25 = call i32 %21(%struct.JNINativeInterface_** noundef %22, %struct._jmethodID* noundef %23, i8* noundef %24)
  store i32 %25, i32* %12, align 4
  store i32 0, i32* %14, align 4
  br label %26

26:                                               ; preds = %133, %5
  %27 = load i32, i32* %14, align 4
  %28 = load i32, i32* %12, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %136

30:                                               ; preds = %26
  %31 = load i32, i32* %14, align 4
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %31
  %33 = load i8, i8* %32, align 1
  %34 = zext i8 %33 to i32
  switch i32 %34, label %131 [
    i32 90, label %35
    i32 66, label %45
    i32 67, label %55
    i32 83, label %65
    i32 73, label %75
    i32 74, label %84
    i32 70, label %94
    i32 68, label %108
    i32 76, label %121
  ]

35:                                               ; preds = %30
  %36 = bitcast %struct.__va_list* %6 to i8**
  %37 = load i8*, i8** %36, align 4
  %38 = getelementptr inbounds i8, i8* %37, i32 4
  store i8* %38, i8** %36, align 4
  %39 = bitcast i8* %37 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = load i32, i32* %14, align 4
  %43 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %42
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %41, i8* %44, align 8
  br label %132

45:                                               ; preds = %30
  %46 = bitcast %struct.__va_list* %6 to i8**
  %47 = load i8*, i8** %46, align 4
  %48 = getelementptr inbounds i8, i8* %47, i32 4
  store i8* %48, i8** %46, align 4
  %49 = bitcast i8* %47 to i32*
  %50 = load i32, i32* %49, align 4
  %51 = trunc i32 %50 to i8
  %52 = load i32, i32* %14, align 4
  %53 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %52
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %51, i8* %54, align 8
  br label %132

55:                                               ; preds = %30
  %56 = bitcast %struct.__va_list* %6 to i8**
  %57 = load i8*, i8** %56, align 4
  %58 = getelementptr inbounds i8, i8* %57, i32 4
  store i8* %58, i8** %56, align 4
  %59 = bitcast i8* %57 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = trunc i32 %60 to i16
  %62 = load i32, i32* %14, align 4
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %62
  %64 = bitcast %union.jvalue* %63 to i16*
  store i16 %61, i16* %64, align 8
  br label %132

65:                                               ; preds = %30
  %66 = bitcast %struct.__va_list* %6 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %14, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %132

75:                                               ; preds = %30
  %76 = bitcast %struct.__va_list* %6 to i8**
  %77 = load i8*, i8** %76, align 4
  %78 = getelementptr inbounds i8, i8* %77, i32 4
  store i8* %78, i8** %76, align 4
  %79 = bitcast i8* %77 to i32*
  %80 = load i32, i32* %79, align 4
  %81 = load i32, i32* %14, align 4
  %82 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %81
  %83 = bitcast %union.jvalue* %82 to i32*
  store i32 %80, i32* %83, align 8
  br label %132

84:                                               ; preds = %30
  %85 = bitcast %struct.__va_list* %6 to i8**
  %86 = load i8*, i8** %85, align 4
  %87 = getelementptr inbounds i8, i8* %86, i32 4
  store i8* %87, i8** %85, align 4
  %88 = bitcast i8* %86 to i32*
  %89 = load i32, i32* %88, align 4
  %90 = sext i32 %89 to i64
  %91 = load i32, i32* %14, align 4
  %92 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %91
  %93 = bitcast %union.jvalue* %92 to i64*
  store i64 %90, i64* %93, align 8
  br label %132

94:                                               ; preds = %30
  %95 = bitcast %struct.__va_list* %6 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = ptrtoint i8* %96 to i32
  %98 = add i32 %97, 7
  %99 = and i32 %98, -8
  %100 = inttoptr i32 %99 to i8*
  %101 = getelementptr inbounds i8, i8* %100, i32 8
  store i8* %101, i8** %95, align 4
  %102 = bitcast i8* %100 to double*
  %103 = load double, double* %102, align 8
  %104 = fptrunc double %103 to float
  %105 = load i32, i32* %14, align 4
  %106 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %105
  %107 = bitcast %union.jvalue* %106 to float*
  store float %104, float* %107, align 8
  br label %132

108:                                              ; preds = %30
  %109 = bitcast %struct.__va_list* %6 to i8**
  %110 = load i8*, i8** %109, align 4
  %111 = ptrtoint i8* %110 to i32
  %112 = add i32 %111, 7
  %113 = and i32 %112, -8
  %114 = inttoptr i32 %113 to i8*
  %115 = getelementptr inbounds i8, i8* %114, i32 8
  store i8* %115, i8** %109, align 4
  %116 = bitcast i8* %114 to double*
  %117 = load double, double* %116, align 8
  %118 = load i32, i32* %14, align 4
  %119 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %118
  %120 = bitcast %union.jvalue* %119 to double*
  store double %117, double* %120, align 8
  br label %132

121:                                              ; preds = %30
  %122 = bitcast %struct.__va_list* %6 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = getelementptr inbounds i8, i8* %123, i32 4
  store i8* %124, i8** %122, align 4
  %125 = bitcast i8* %123 to i8**
  %126 = load i8*, i8** %125, align 4
  %127 = bitcast i8* %126 to %struct._jobject*
  %128 = load i32, i32* %14, align 4
  %129 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %128
  %130 = bitcast %union.jvalue* %129 to %struct._jobject**
  store %struct._jobject* %127, %struct._jobject** %130, align 8
  br label %132

131:                                              ; preds = %30
  br label %132

132:                                              ; preds = %131, %121, %108, %94, %84, %75, %65, %55, %45, %35
  br label %133

133:                                              ; preds = %132
  %134 = load i32, i32* %14, align 4
  %135 = add nsw i32 %134, 1
  store i32 %135, i32* %14, align 4
  br label %26, !llvm.loop !20

136:                                              ; preds = %26
  %137 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %138 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %137, align 4
  %139 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %138, i32 0, i32 69
  %140 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %139, align 4
  %141 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %142 = load %struct._jobject*, %struct._jobject** %8, align 4
  %143 = load %struct._jobject*, %struct._jobject** %9, align 4
  %144 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %145 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 0
  %146 = call zeroext i8 %140(%struct.JNINativeInterface_** noundef %141, %struct._jobject* noundef %142, %struct._jobject* noundef %143, %struct._jmethodID* noundef %144, %union.jvalue* noundef %145)
  ret i8 %146
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i8 @JNI_CallStaticBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i8, align 1
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %13 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %13)
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 4
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %21 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %130, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %9, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %133

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  switch i32 %31, label %128 [
    i32 90, label %32
    i32 66, label %42
    i32 67, label %52
    i32 83, label %62
    i32 73, label %72
    i32 74, label %81
    i32 70, label %91
    i32 68, label %105
    i32 76, label %118
  ]

32:                                               ; preds = %27
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %129

42:                                               ; preds = %27
  %43 = bitcast %struct.__va_list* %7 to i8**
  %44 = load i8*, i8** %43, align 4
  %45 = getelementptr inbounds i8, i8* %44, i32 4
  store i8* %45, i8** %43, align 4
  %46 = bitcast i8* %44 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %11, align 4
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %48, i8* %51, align 8
  br label %129

52:                                               ; preds = %27
  %53 = bitcast %struct.__va_list* %7 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i16
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i16*
  store i16 %58, i16* %61, align 8
  br label %129

62:                                               ; preds = %27
  %63 = bitcast %struct.__va_list* %7 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load i32, i32* %11, align 4
  %70 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %69
  %71 = bitcast %union.jvalue* %70 to i16*
  store i16 %68, i16* %71, align 8
  br label %129

72:                                               ; preds = %27
  %73 = bitcast %struct.__va_list* %7 to i8**
  %74 = load i8*, i8** %73, align 4
  %75 = getelementptr inbounds i8, i8* %74, i32 4
  store i8* %75, i8** %73, align 4
  %76 = bitcast i8* %74 to i32*
  %77 = load i32, i32* %76, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %78
  %80 = bitcast %union.jvalue* %79 to i32*
  store i32 %77, i32* %80, align 8
  br label %129

81:                                               ; preds = %27
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = sext i32 %86 to i64
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i64*
  store i64 %87, i64* %90, align 8
  br label %129

91:                                               ; preds = %27
  %92 = bitcast %struct.__va_list* %7 to i8**
  %93 = load i8*, i8** %92, align 4
  %94 = ptrtoint i8* %93 to i32
  %95 = add i32 %94, 7
  %96 = and i32 %95, -8
  %97 = inttoptr i32 %96 to i8*
  %98 = getelementptr inbounds i8, i8* %97, i32 8
  store i8* %98, i8** %92, align 4
  %99 = bitcast i8* %97 to double*
  %100 = load double, double* %99, align 8
  %101 = fptrunc double %100 to float
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to float*
  store float %101, float* %104, align 8
  br label %129

105:                                              ; preds = %27
  %106 = bitcast %struct.__va_list* %7 to i8**
  %107 = load i8*, i8** %106, align 4
  %108 = ptrtoint i8* %107 to i32
  %109 = add i32 %108, 7
  %110 = and i32 %109, -8
  %111 = inttoptr i32 %110 to i8*
  %112 = getelementptr inbounds i8, i8* %111, i32 8
  store i8* %112, i8** %106, align 4
  %113 = bitcast i8* %111 to double*
  %114 = load double, double* %113, align 8
  %115 = load i32, i32* %11, align 4
  %116 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %114, double* %117, align 8
  br label %129

118:                                              ; preds = %27
  %119 = bitcast %struct.__va_list* %7 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = getelementptr inbounds i8, i8* %120, i32 4
  store i8* %121, i8** %119, align 4
  %122 = bitcast i8* %120 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = bitcast i8* %123 to %struct._jobject*
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %125
  %127 = bitcast %union.jvalue* %126 to %struct._jobject**
  store %struct._jobject* %124, %struct._jobject** %127, align 8
  br label %129

128:                                              ; preds = %27
  br label %129

129:                                              ; preds = %128, %118, %105, %91, %81, %72, %62, %52, %42, %32
  br label %130

130:                                              ; preds = %129
  %131 = load i32, i32* %11, align 4
  %132 = add nsw i32 %131, 1
  store i32 %132, i32* %11, align 4
  br label %23, !llvm.loop !21

133:                                              ; preds = %23
  %134 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %135 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %134, align 4
  %136 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %135, i32 0, i32 119
  %137 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %136, align 4
  %138 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %139 = load %struct._jobject*, %struct._jobject** %5, align 4
  %140 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %142 = call zeroext i8 %137(%struct.JNINativeInterface_** noundef %138, %struct._jobject* noundef %139, %struct._jmethodID* noundef %140, %union.jvalue* noundef %141)
  store i8 %142, i8* %12, align 1
  %143 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %143)
  %144 = load i8, i8* %12, align 1
  ret i8 %144
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i8 @JNI_CallStaticBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !22

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 119
  %138 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %143 = call zeroext i8 %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret i8 %143
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i8 @JNI_CallByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i8, align 1
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %13 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %13)
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 4
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %21 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %130, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %9, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %133

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  switch i32 %31, label %128 [
    i32 90, label %32
    i32 66, label %42
    i32 67, label %52
    i32 83, label %62
    i32 73, label %72
    i32 74, label %81
    i32 70, label %91
    i32 68, label %105
    i32 76, label %118
  ]

32:                                               ; preds = %27
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %129

42:                                               ; preds = %27
  %43 = bitcast %struct.__va_list* %7 to i8**
  %44 = load i8*, i8** %43, align 4
  %45 = getelementptr inbounds i8, i8* %44, i32 4
  store i8* %45, i8** %43, align 4
  %46 = bitcast i8* %44 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %11, align 4
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %48, i8* %51, align 8
  br label %129

52:                                               ; preds = %27
  %53 = bitcast %struct.__va_list* %7 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i16
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i16*
  store i16 %58, i16* %61, align 8
  br label %129

62:                                               ; preds = %27
  %63 = bitcast %struct.__va_list* %7 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load i32, i32* %11, align 4
  %70 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %69
  %71 = bitcast %union.jvalue* %70 to i16*
  store i16 %68, i16* %71, align 8
  br label %129

72:                                               ; preds = %27
  %73 = bitcast %struct.__va_list* %7 to i8**
  %74 = load i8*, i8** %73, align 4
  %75 = getelementptr inbounds i8, i8* %74, i32 4
  store i8* %75, i8** %73, align 4
  %76 = bitcast i8* %74 to i32*
  %77 = load i32, i32* %76, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %78
  %80 = bitcast %union.jvalue* %79 to i32*
  store i32 %77, i32* %80, align 8
  br label %129

81:                                               ; preds = %27
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = sext i32 %86 to i64
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i64*
  store i64 %87, i64* %90, align 8
  br label %129

91:                                               ; preds = %27
  %92 = bitcast %struct.__va_list* %7 to i8**
  %93 = load i8*, i8** %92, align 4
  %94 = ptrtoint i8* %93 to i32
  %95 = add i32 %94, 7
  %96 = and i32 %95, -8
  %97 = inttoptr i32 %96 to i8*
  %98 = getelementptr inbounds i8, i8* %97, i32 8
  store i8* %98, i8** %92, align 4
  %99 = bitcast i8* %97 to double*
  %100 = load double, double* %99, align 8
  %101 = fptrunc double %100 to float
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to float*
  store float %101, float* %104, align 8
  br label %129

105:                                              ; preds = %27
  %106 = bitcast %struct.__va_list* %7 to i8**
  %107 = load i8*, i8** %106, align 4
  %108 = ptrtoint i8* %107 to i32
  %109 = add i32 %108, 7
  %110 = and i32 %109, -8
  %111 = inttoptr i32 %110 to i8*
  %112 = getelementptr inbounds i8, i8* %111, i32 8
  store i8* %112, i8** %106, align 4
  %113 = bitcast i8* %111 to double*
  %114 = load double, double* %113, align 8
  %115 = load i32, i32* %11, align 4
  %116 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %114, double* %117, align 8
  br label %129

118:                                              ; preds = %27
  %119 = bitcast %struct.__va_list* %7 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = getelementptr inbounds i8, i8* %120, i32 4
  store i8* %121, i8** %119, align 4
  %122 = bitcast i8* %120 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = bitcast i8* %123 to %struct._jobject*
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %125
  %127 = bitcast %union.jvalue* %126 to %struct._jobject**
  store %struct._jobject* %124, %struct._jobject** %127, align 8
  br label %129

128:                                              ; preds = %27
  br label %129

129:                                              ; preds = %128, %118, %105, %91, %81, %72, %62, %52, %42, %32
  br label %130

130:                                              ; preds = %129
  %131 = load i32, i32* %11, align 4
  %132 = add nsw i32 %131, 1
  store i32 %132, i32* %11, align 4
  br label %23, !llvm.loop !23

133:                                              ; preds = %23
  %134 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %135 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %134, align 4
  %136 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %135, i32 0, i32 42
  %137 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %136, align 4
  %138 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %139 = load %struct._jobject*, %struct._jobject** %5, align 4
  %140 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %142 = call signext i8 %137(%struct.JNINativeInterface_** noundef %138, %struct._jobject* noundef %139, %struct._jmethodID* noundef %140, %union.jvalue* noundef %141)
  store i8 %142, i8* %12, align 1
  %143 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %143)
  %144 = load i8, i8* %12, align 1
  ret i8 %144
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i8 @JNI_CallByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !24

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 42
  %138 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %143 = call signext i8 %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret i8 %143
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i8 @JNI_CallNonvirtualByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct.__va_list, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i8, align 1
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %1, %struct._jobject** %6, align 4
  store %struct._jobject* %2, %struct._jobject** %7, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 4
  %15 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_start(i8* %15)
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  store i32 0, i32* %13, align 4
  br label %25

25:                                               ; preds = %132, %4
  %26 = load i32, i32* %13, align 4
  %27 = load i32, i32* %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %135

29:                                               ; preds = %25
  %30 = load i32, i32* %13, align 4
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %30
  %32 = load i8, i8* %31, align 1
  %33 = zext i8 %32 to i32
  switch i32 %33, label %130 [
    i32 90, label %34
    i32 66, label %44
    i32 67, label %54
    i32 83, label %64
    i32 73, label %74
    i32 74, label %83
    i32 70, label %93
    i32 68, label %107
    i32 76, label %120
  ]

34:                                               ; preds = %29
  %35 = bitcast %struct.__va_list* %9 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %13, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %131

44:                                               ; preds = %29
  %45 = bitcast %struct.__va_list* %9 to i8**
  %46 = load i8*, i8** %45, align 4
  %47 = getelementptr inbounds i8, i8* %46, i32 4
  store i8* %47, i8** %45, align 4
  %48 = bitcast i8* %46 to i32*
  %49 = load i32, i32* %48, align 4
  %50 = trunc i32 %49 to i8
  %51 = load i32, i32* %13, align 4
  %52 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %51
  %53 = bitcast %union.jvalue* %52 to i8*
  store i8 %50, i8* %53, align 8
  br label %131

54:                                               ; preds = %29
  %55 = bitcast %struct.__va_list* %9 to i8**
  %56 = load i8*, i8** %55, align 4
  %57 = getelementptr inbounds i8, i8* %56, i32 4
  store i8* %57, i8** %55, align 4
  %58 = bitcast i8* %56 to i32*
  %59 = load i32, i32* %58, align 4
  %60 = trunc i32 %59 to i16
  %61 = load i32, i32* %13, align 4
  %62 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %61
  %63 = bitcast %union.jvalue* %62 to i16*
  store i16 %60, i16* %63, align 8
  br label %131

64:                                               ; preds = %29
  %65 = bitcast %struct.__va_list* %9 to i8**
  %66 = load i8*, i8** %65, align 4
  %67 = getelementptr inbounds i8, i8* %66, i32 4
  store i8* %67, i8** %65, align 4
  %68 = bitcast i8* %66 to i32*
  %69 = load i32, i32* %68, align 4
  %70 = trunc i32 %69 to i16
  %71 = load i32, i32* %13, align 4
  %72 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %71
  %73 = bitcast %union.jvalue* %72 to i16*
  store i16 %70, i16* %73, align 8
  br label %131

74:                                               ; preds = %29
  %75 = bitcast %struct.__va_list* %9 to i8**
  %76 = load i8*, i8** %75, align 4
  %77 = getelementptr inbounds i8, i8* %76, i32 4
  store i8* %77, i8** %75, align 4
  %78 = bitcast i8* %76 to i32*
  %79 = load i32, i32* %78, align 4
  %80 = load i32, i32* %13, align 4
  %81 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %79, i32* %82, align 8
  br label %131

83:                                               ; preds = %29
  %84 = bitcast %struct.__va_list* %9 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = sext i32 %88 to i64
  %90 = load i32, i32* %13, align 4
  %91 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %90
  %92 = bitcast %union.jvalue* %91 to i64*
  store i64 %89, i64* %92, align 8
  br label %131

93:                                               ; preds = %29
  %94 = bitcast %struct.__va_list* %9 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = ptrtoint i8* %95 to i32
  %97 = add i32 %96, 7
  %98 = and i32 %97, -8
  %99 = inttoptr i32 %98 to i8*
  %100 = getelementptr inbounds i8, i8* %99, i32 8
  store i8* %100, i8** %94, align 4
  %101 = bitcast i8* %99 to double*
  %102 = load double, double* %101, align 8
  %103 = fptrunc double %102 to float
  %104 = load i32, i32* %13, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to float*
  store float %103, float* %106, align 8
  br label %131

107:                                              ; preds = %29
  %108 = bitcast %struct.__va_list* %9 to i8**
  %109 = load i8*, i8** %108, align 4
  %110 = ptrtoint i8* %109 to i32
  %111 = add i32 %110, 7
  %112 = and i32 %111, -8
  %113 = inttoptr i32 %112 to i8*
  %114 = getelementptr inbounds i8, i8* %113, i32 8
  store i8* %114, i8** %108, align 4
  %115 = bitcast i8* %113 to double*
  %116 = load double, double* %115, align 8
  %117 = load i32, i32* %13, align 4
  %118 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %117
  %119 = bitcast %union.jvalue* %118 to double*
  store double %116, double* %119, align 8
  br label %131

120:                                              ; preds = %29
  %121 = bitcast %struct.__va_list* %9 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = getelementptr inbounds i8, i8* %122, i32 4
  store i8* %123, i8** %121, align 4
  %124 = bitcast i8* %122 to i8**
  %125 = load i8*, i8** %124, align 4
  %126 = bitcast i8* %125 to %struct._jobject*
  %127 = load i32, i32* %13, align 4
  %128 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %127
  %129 = bitcast %union.jvalue* %128 to %struct._jobject**
  store %struct._jobject* %126, %struct._jobject** %129, align 8
  br label %131

130:                                              ; preds = %29
  br label %131

131:                                              ; preds = %130, %120, %107, %93, %83, %74, %64, %54, %44, %34
  br label %132

132:                                              ; preds = %131
  %133 = load i32, i32* %13, align 4
  %134 = add nsw i32 %133, 1
  store i32 %134, i32* %13, align 4
  br label %25, !llvm.loop !25

135:                                              ; preds = %25
  %136 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %137 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %136, align 4
  %138 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %137, i32 0, i32 72
  %139 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %138, align 4
  %140 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %141 = load %struct._jobject*, %struct._jobject** %6, align 4
  %142 = load %struct._jobject*, %struct._jobject** %7, align 4
  %143 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %144 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 0
  %145 = call signext i8 %139(%struct.JNINativeInterface_** noundef %140, %struct._jobject* noundef %141, %struct._jobject* noundef %142, %struct._jmethodID* noundef %143, %union.jvalue* noundef %144)
  store i8 %145, i8* %14, align 1
  %146 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %146)
  %147 = load i8, i8* %14, align 1
  ret i8 %147
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i8 @JNI_CallNonvirtualByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  %15 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %6, i32 0, i32 0
  %16 = bitcast i8** %15 to [1 x i32]*
  store [1 x i32] %4, [1 x i32]* %16, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %7, align 4
  store %struct._jobject* %1, %struct._jobject** %8, align 4
  store %struct._jobject* %2, %struct._jobject** %9, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %10, align 4
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %18 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %17, align 4
  %19 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %18, i32 0, i32 0
  %20 = load i8*, i8** %19, align 4
  %21 = bitcast i8* %20 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %23 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %24 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 0
  %25 = call i32 %21(%struct.JNINativeInterface_** noundef %22, %struct._jmethodID* noundef %23, i8* noundef %24)
  store i32 %25, i32* %12, align 4
  store i32 0, i32* %14, align 4
  br label %26

26:                                               ; preds = %133, %5
  %27 = load i32, i32* %14, align 4
  %28 = load i32, i32* %12, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %136

30:                                               ; preds = %26
  %31 = load i32, i32* %14, align 4
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %31
  %33 = load i8, i8* %32, align 1
  %34 = zext i8 %33 to i32
  switch i32 %34, label %131 [
    i32 90, label %35
    i32 66, label %45
    i32 67, label %55
    i32 83, label %65
    i32 73, label %75
    i32 74, label %84
    i32 70, label %94
    i32 68, label %108
    i32 76, label %121
  ]

35:                                               ; preds = %30
  %36 = bitcast %struct.__va_list* %6 to i8**
  %37 = load i8*, i8** %36, align 4
  %38 = getelementptr inbounds i8, i8* %37, i32 4
  store i8* %38, i8** %36, align 4
  %39 = bitcast i8* %37 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = load i32, i32* %14, align 4
  %43 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %42
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %41, i8* %44, align 8
  br label %132

45:                                               ; preds = %30
  %46 = bitcast %struct.__va_list* %6 to i8**
  %47 = load i8*, i8** %46, align 4
  %48 = getelementptr inbounds i8, i8* %47, i32 4
  store i8* %48, i8** %46, align 4
  %49 = bitcast i8* %47 to i32*
  %50 = load i32, i32* %49, align 4
  %51 = trunc i32 %50 to i8
  %52 = load i32, i32* %14, align 4
  %53 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %52
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %51, i8* %54, align 8
  br label %132

55:                                               ; preds = %30
  %56 = bitcast %struct.__va_list* %6 to i8**
  %57 = load i8*, i8** %56, align 4
  %58 = getelementptr inbounds i8, i8* %57, i32 4
  store i8* %58, i8** %56, align 4
  %59 = bitcast i8* %57 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = trunc i32 %60 to i16
  %62 = load i32, i32* %14, align 4
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %62
  %64 = bitcast %union.jvalue* %63 to i16*
  store i16 %61, i16* %64, align 8
  br label %132

65:                                               ; preds = %30
  %66 = bitcast %struct.__va_list* %6 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %14, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %132

75:                                               ; preds = %30
  %76 = bitcast %struct.__va_list* %6 to i8**
  %77 = load i8*, i8** %76, align 4
  %78 = getelementptr inbounds i8, i8* %77, i32 4
  store i8* %78, i8** %76, align 4
  %79 = bitcast i8* %77 to i32*
  %80 = load i32, i32* %79, align 4
  %81 = load i32, i32* %14, align 4
  %82 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %81
  %83 = bitcast %union.jvalue* %82 to i32*
  store i32 %80, i32* %83, align 8
  br label %132

84:                                               ; preds = %30
  %85 = bitcast %struct.__va_list* %6 to i8**
  %86 = load i8*, i8** %85, align 4
  %87 = getelementptr inbounds i8, i8* %86, i32 4
  store i8* %87, i8** %85, align 4
  %88 = bitcast i8* %86 to i32*
  %89 = load i32, i32* %88, align 4
  %90 = sext i32 %89 to i64
  %91 = load i32, i32* %14, align 4
  %92 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %91
  %93 = bitcast %union.jvalue* %92 to i64*
  store i64 %90, i64* %93, align 8
  br label %132

94:                                               ; preds = %30
  %95 = bitcast %struct.__va_list* %6 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = ptrtoint i8* %96 to i32
  %98 = add i32 %97, 7
  %99 = and i32 %98, -8
  %100 = inttoptr i32 %99 to i8*
  %101 = getelementptr inbounds i8, i8* %100, i32 8
  store i8* %101, i8** %95, align 4
  %102 = bitcast i8* %100 to double*
  %103 = load double, double* %102, align 8
  %104 = fptrunc double %103 to float
  %105 = load i32, i32* %14, align 4
  %106 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %105
  %107 = bitcast %union.jvalue* %106 to float*
  store float %104, float* %107, align 8
  br label %132

108:                                              ; preds = %30
  %109 = bitcast %struct.__va_list* %6 to i8**
  %110 = load i8*, i8** %109, align 4
  %111 = ptrtoint i8* %110 to i32
  %112 = add i32 %111, 7
  %113 = and i32 %112, -8
  %114 = inttoptr i32 %113 to i8*
  %115 = getelementptr inbounds i8, i8* %114, i32 8
  store i8* %115, i8** %109, align 4
  %116 = bitcast i8* %114 to double*
  %117 = load double, double* %116, align 8
  %118 = load i32, i32* %14, align 4
  %119 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %118
  %120 = bitcast %union.jvalue* %119 to double*
  store double %117, double* %120, align 8
  br label %132

121:                                              ; preds = %30
  %122 = bitcast %struct.__va_list* %6 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = getelementptr inbounds i8, i8* %123, i32 4
  store i8* %124, i8** %122, align 4
  %125 = bitcast i8* %123 to i8**
  %126 = load i8*, i8** %125, align 4
  %127 = bitcast i8* %126 to %struct._jobject*
  %128 = load i32, i32* %14, align 4
  %129 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %128
  %130 = bitcast %union.jvalue* %129 to %struct._jobject**
  store %struct._jobject* %127, %struct._jobject** %130, align 8
  br label %132

131:                                              ; preds = %30
  br label %132

132:                                              ; preds = %131, %121, %108, %94, %84, %75, %65, %55, %45, %35
  br label %133

133:                                              ; preds = %132
  %134 = load i32, i32* %14, align 4
  %135 = add nsw i32 %134, 1
  store i32 %135, i32* %14, align 4
  br label %26, !llvm.loop !26

136:                                              ; preds = %26
  %137 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %138 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %137, align 4
  %139 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %138, i32 0, i32 72
  %140 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %139, align 4
  %141 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %142 = load %struct._jobject*, %struct._jobject** %8, align 4
  %143 = load %struct._jobject*, %struct._jobject** %9, align 4
  %144 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %145 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 0
  %146 = call signext i8 %140(%struct.JNINativeInterface_** noundef %141, %struct._jobject* noundef %142, %struct._jobject* noundef %143, %struct._jmethodID* noundef %144, %union.jvalue* noundef %145)
  ret i8 %146
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i8 @JNI_CallStaticByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i8, align 1
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %13 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %13)
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 4
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %21 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %130, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %9, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %133

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  switch i32 %31, label %128 [
    i32 90, label %32
    i32 66, label %42
    i32 67, label %52
    i32 83, label %62
    i32 73, label %72
    i32 74, label %81
    i32 70, label %91
    i32 68, label %105
    i32 76, label %118
  ]

32:                                               ; preds = %27
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %129

42:                                               ; preds = %27
  %43 = bitcast %struct.__va_list* %7 to i8**
  %44 = load i8*, i8** %43, align 4
  %45 = getelementptr inbounds i8, i8* %44, i32 4
  store i8* %45, i8** %43, align 4
  %46 = bitcast i8* %44 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %11, align 4
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %48, i8* %51, align 8
  br label %129

52:                                               ; preds = %27
  %53 = bitcast %struct.__va_list* %7 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i16
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i16*
  store i16 %58, i16* %61, align 8
  br label %129

62:                                               ; preds = %27
  %63 = bitcast %struct.__va_list* %7 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load i32, i32* %11, align 4
  %70 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %69
  %71 = bitcast %union.jvalue* %70 to i16*
  store i16 %68, i16* %71, align 8
  br label %129

72:                                               ; preds = %27
  %73 = bitcast %struct.__va_list* %7 to i8**
  %74 = load i8*, i8** %73, align 4
  %75 = getelementptr inbounds i8, i8* %74, i32 4
  store i8* %75, i8** %73, align 4
  %76 = bitcast i8* %74 to i32*
  %77 = load i32, i32* %76, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %78
  %80 = bitcast %union.jvalue* %79 to i32*
  store i32 %77, i32* %80, align 8
  br label %129

81:                                               ; preds = %27
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = sext i32 %86 to i64
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i64*
  store i64 %87, i64* %90, align 8
  br label %129

91:                                               ; preds = %27
  %92 = bitcast %struct.__va_list* %7 to i8**
  %93 = load i8*, i8** %92, align 4
  %94 = ptrtoint i8* %93 to i32
  %95 = add i32 %94, 7
  %96 = and i32 %95, -8
  %97 = inttoptr i32 %96 to i8*
  %98 = getelementptr inbounds i8, i8* %97, i32 8
  store i8* %98, i8** %92, align 4
  %99 = bitcast i8* %97 to double*
  %100 = load double, double* %99, align 8
  %101 = fptrunc double %100 to float
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to float*
  store float %101, float* %104, align 8
  br label %129

105:                                              ; preds = %27
  %106 = bitcast %struct.__va_list* %7 to i8**
  %107 = load i8*, i8** %106, align 4
  %108 = ptrtoint i8* %107 to i32
  %109 = add i32 %108, 7
  %110 = and i32 %109, -8
  %111 = inttoptr i32 %110 to i8*
  %112 = getelementptr inbounds i8, i8* %111, i32 8
  store i8* %112, i8** %106, align 4
  %113 = bitcast i8* %111 to double*
  %114 = load double, double* %113, align 8
  %115 = load i32, i32* %11, align 4
  %116 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %114, double* %117, align 8
  br label %129

118:                                              ; preds = %27
  %119 = bitcast %struct.__va_list* %7 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = getelementptr inbounds i8, i8* %120, i32 4
  store i8* %121, i8** %119, align 4
  %122 = bitcast i8* %120 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = bitcast i8* %123 to %struct._jobject*
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %125
  %127 = bitcast %union.jvalue* %126 to %struct._jobject**
  store %struct._jobject* %124, %struct._jobject** %127, align 8
  br label %129

128:                                              ; preds = %27
  br label %129

129:                                              ; preds = %128, %118, %105, %91, %81, %72, %62, %52, %42, %32
  br label %130

130:                                              ; preds = %129
  %131 = load i32, i32* %11, align 4
  %132 = add nsw i32 %131, 1
  store i32 %132, i32* %11, align 4
  br label %23, !llvm.loop !27

133:                                              ; preds = %23
  %134 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %135 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %134, align 4
  %136 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %135, i32 0, i32 122
  %137 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %136, align 4
  %138 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %139 = load %struct._jobject*, %struct._jobject** %5, align 4
  %140 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %142 = call signext i8 %137(%struct.JNINativeInterface_** noundef %138, %struct._jobject* noundef %139, %struct._jmethodID* noundef %140, %union.jvalue* noundef %141)
  store i8 %142, i8* %12, align 1
  %143 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %143)
  %144 = load i8, i8* %12, align 1
  ret i8 %144
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i8 @JNI_CallStaticByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !28

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 122
  %138 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %143 = call signext i8 %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret i8 %143
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i16 @JNI_CallCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i16, align 2
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %13 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %13)
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 4
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %21 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %130, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %9, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %133

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  switch i32 %31, label %128 [
    i32 90, label %32
    i32 66, label %42
    i32 67, label %52
    i32 83, label %62
    i32 73, label %72
    i32 74, label %81
    i32 70, label %91
    i32 68, label %105
    i32 76, label %118
  ]

32:                                               ; preds = %27
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %129

42:                                               ; preds = %27
  %43 = bitcast %struct.__va_list* %7 to i8**
  %44 = load i8*, i8** %43, align 4
  %45 = getelementptr inbounds i8, i8* %44, i32 4
  store i8* %45, i8** %43, align 4
  %46 = bitcast i8* %44 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %11, align 4
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %48, i8* %51, align 8
  br label %129

52:                                               ; preds = %27
  %53 = bitcast %struct.__va_list* %7 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i16
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i16*
  store i16 %58, i16* %61, align 8
  br label %129

62:                                               ; preds = %27
  %63 = bitcast %struct.__va_list* %7 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load i32, i32* %11, align 4
  %70 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %69
  %71 = bitcast %union.jvalue* %70 to i16*
  store i16 %68, i16* %71, align 8
  br label %129

72:                                               ; preds = %27
  %73 = bitcast %struct.__va_list* %7 to i8**
  %74 = load i8*, i8** %73, align 4
  %75 = getelementptr inbounds i8, i8* %74, i32 4
  store i8* %75, i8** %73, align 4
  %76 = bitcast i8* %74 to i32*
  %77 = load i32, i32* %76, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %78
  %80 = bitcast %union.jvalue* %79 to i32*
  store i32 %77, i32* %80, align 8
  br label %129

81:                                               ; preds = %27
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = sext i32 %86 to i64
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i64*
  store i64 %87, i64* %90, align 8
  br label %129

91:                                               ; preds = %27
  %92 = bitcast %struct.__va_list* %7 to i8**
  %93 = load i8*, i8** %92, align 4
  %94 = ptrtoint i8* %93 to i32
  %95 = add i32 %94, 7
  %96 = and i32 %95, -8
  %97 = inttoptr i32 %96 to i8*
  %98 = getelementptr inbounds i8, i8* %97, i32 8
  store i8* %98, i8** %92, align 4
  %99 = bitcast i8* %97 to double*
  %100 = load double, double* %99, align 8
  %101 = fptrunc double %100 to float
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to float*
  store float %101, float* %104, align 8
  br label %129

105:                                              ; preds = %27
  %106 = bitcast %struct.__va_list* %7 to i8**
  %107 = load i8*, i8** %106, align 4
  %108 = ptrtoint i8* %107 to i32
  %109 = add i32 %108, 7
  %110 = and i32 %109, -8
  %111 = inttoptr i32 %110 to i8*
  %112 = getelementptr inbounds i8, i8* %111, i32 8
  store i8* %112, i8** %106, align 4
  %113 = bitcast i8* %111 to double*
  %114 = load double, double* %113, align 8
  %115 = load i32, i32* %11, align 4
  %116 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %114, double* %117, align 8
  br label %129

118:                                              ; preds = %27
  %119 = bitcast %struct.__va_list* %7 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = getelementptr inbounds i8, i8* %120, i32 4
  store i8* %121, i8** %119, align 4
  %122 = bitcast i8* %120 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = bitcast i8* %123 to %struct._jobject*
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %125
  %127 = bitcast %union.jvalue* %126 to %struct._jobject**
  store %struct._jobject* %124, %struct._jobject** %127, align 8
  br label %129

128:                                              ; preds = %27
  br label %129

129:                                              ; preds = %128, %118, %105, %91, %81, %72, %62, %52, %42, %32
  br label %130

130:                                              ; preds = %129
  %131 = load i32, i32* %11, align 4
  %132 = add nsw i32 %131, 1
  store i32 %132, i32* %11, align 4
  br label %23, !llvm.loop !29

133:                                              ; preds = %23
  %134 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %135 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %134, align 4
  %136 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %135, i32 0, i32 45
  %137 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %136, align 4
  %138 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %139 = load %struct._jobject*, %struct._jobject** %5, align 4
  %140 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %142 = call zeroext i16 %137(%struct.JNINativeInterface_** noundef %138, %struct._jobject* noundef %139, %struct._jmethodID* noundef %140, %union.jvalue* noundef %141)
  store i16 %142, i16* %12, align 2
  %143 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %143)
  %144 = load i16, i16* %12, align 2
  ret i16 %144
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i16 @JNI_CallCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !30

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 45
  %138 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %143 = call zeroext i16 %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret i16 %143
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i16 @JNI_CallNonvirtualCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct.__va_list, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i16, align 2
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %1, %struct._jobject** %6, align 4
  store %struct._jobject* %2, %struct._jobject** %7, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 4
  %15 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_start(i8* %15)
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  store i32 0, i32* %13, align 4
  br label %25

25:                                               ; preds = %132, %4
  %26 = load i32, i32* %13, align 4
  %27 = load i32, i32* %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %135

29:                                               ; preds = %25
  %30 = load i32, i32* %13, align 4
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %30
  %32 = load i8, i8* %31, align 1
  %33 = zext i8 %32 to i32
  switch i32 %33, label %130 [
    i32 90, label %34
    i32 66, label %44
    i32 67, label %54
    i32 83, label %64
    i32 73, label %74
    i32 74, label %83
    i32 70, label %93
    i32 68, label %107
    i32 76, label %120
  ]

34:                                               ; preds = %29
  %35 = bitcast %struct.__va_list* %9 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %13, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %131

44:                                               ; preds = %29
  %45 = bitcast %struct.__va_list* %9 to i8**
  %46 = load i8*, i8** %45, align 4
  %47 = getelementptr inbounds i8, i8* %46, i32 4
  store i8* %47, i8** %45, align 4
  %48 = bitcast i8* %46 to i32*
  %49 = load i32, i32* %48, align 4
  %50 = trunc i32 %49 to i8
  %51 = load i32, i32* %13, align 4
  %52 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %51
  %53 = bitcast %union.jvalue* %52 to i8*
  store i8 %50, i8* %53, align 8
  br label %131

54:                                               ; preds = %29
  %55 = bitcast %struct.__va_list* %9 to i8**
  %56 = load i8*, i8** %55, align 4
  %57 = getelementptr inbounds i8, i8* %56, i32 4
  store i8* %57, i8** %55, align 4
  %58 = bitcast i8* %56 to i32*
  %59 = load i32, i32* %58, align 4
  %60 = trunc i32 %59 to i16
  %61 = load i32, i32* %13, align 4
  %62 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %61
  %63 = bitcast %union.jvalue* %62 to i16*
  store i16 %60, i16* %63, align 8
  br label %131

64:                                               ; preds = %29
  %65 = bitcast %struct.__va_list* %9 to i8**
  %66 = load i8*, i8** %65, align 4
  %67 = getelementptr inbounds i8, i8* %66, i32 4
  store i8* %67, i8** %65, align 4
  %68 = bitcast i8* %66 to i32*
  %69 = load i32, i32* %68, align 4
  %70 = trunc i32 %69 to i16
  %71 = load i32, i32* %13, align 4
  %72 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %71
  %73 = bitcast %union.jvalue* %72 to i16*
  store i16 %70, i16* %73, align 8
  br label %131

74:                                               ; preds = %29
  %75 = bitcast %struct.__va_list* %9 to i8**
  %76 = load i8*, i8** %75, align 4
  %77 = getelementptr inbounds i8, i8* %76, i32 4
  store i8* %77, i8** %75, align 4
  %78 = bitcast i8* %76 to i32*
  %79 = load i32, i32* %78, align 4
  %80 = load i32, i32* %13, align 4
  %81 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %79, i32* %82, align 8
  br label %131

83:                                               ; preds = %29
  %84 = bitcast %struct.__va_list* %9 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = sext i32 %88 to i64
  %90 = load i32, i32* %13, align 4
  %91 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %90
  %92 = bitcast %union.jvalue* %91 to i64*
  store i64 %89, i64* %92, align 8
  br label %131

93:                                               ; preds = %29
  %94 = bitcast %struct.__va_list* %9 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = ptrtoint i8* %95 to i32
  %97 = add i32 %96, 7
  %98 = and i32 %97, -8
  %99 = inttoptr i32 %98 to i8*
  %100 = getelementptr inbounds i8, i8* %99, i32 8
  store i8* %100, i8** %94, align 4
  %101 = bitcast i8* %99 to double*
  %102 = load double, double* %101, align 8
  %103 = fptrunc double %102 to float
  %104 = load i32, i32* %13, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to float*
  store float %103, float* %106, align 8
  br label %131

107:                                              ; preds = %29
  %108 = bitcast %struct.__va_list* %9 to i8**
  %109 = load i8*, i8** %108, align 4
  %110 = ptrtoint i8* %109 to i32
  %111 = add i32 %110, 7
  %112 = and i32 %111, -8
  %113 = inttoptr i32 %112 to i8*
  %114 = getelementptr inbounds i8, i8* %113, i32 8
  store i8* %114, i8** %108, align 4
  %115 = bitcast i8* %113 to double*
  %116 = load double, double* %115, align 8
  %117 = load i32, i32* %13, align 4
  %118 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %117
  %119 = bitcast %union.jvalue* %118 to double*
  store double %116, double* %119, align 8
  br label %131

120:                                              ; preds = %29
  %121 = bitcast %struct.__va_list* %9 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = getelementptr inbounds i8, i8* %122, i32 4
  store i8* %123, i8** %121, align 4
  %124 = bitcast i8* %122 to i8**
  %125 = load i8*, i8** %124, align 4
  %126 = bitcast i8* %125 to %struct._jobject*
  %127 = load i32, i32* %13, align 4
  %128 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %127
  %129 = bitcast %union.jvalue* %128 to %struct._jobject**
  store %struct._jobject* %126, %struct._jobject** %129, align 8
  br label %131

130:                                              ; preds = %29
  br label %131

131:                                              ; preds = %130, %120, %107, %93, %83, %74, %64, %54, %44, %34
  br label %132

132:                                              ; preds = %131
  %133 = load i32, i32* %13, align 4
  %134 = add nsw i32 %133, 1
  store i32 %134, i32* %13, align 4
  br label %25, !llvm.loop !31

135:                                              ; preds = %25
  %136 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %137 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %136, align 4
  %138 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %137, i32 0, i32 75
  %139 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %138, align 4
  %140 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %141 = load %struct._jobject*, %struct._jobject** %6, align 4
  %142 = load %struct._jobject*, %struct._jobject** %7, align 4
  %143 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %144 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 0
  %145 = call zeroext i16 %139(%struct.JNINativeInterface_** noundef %140, %struct._jobject* noundef %141, %struct._jobject* noundef %142, %struct._jmethodID* noundef %143, %union.jvalue* noundef %144)
  store i16 %145, i16* %14, align 2
  %146 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %146)
  %147 = load i16, i16* %14, align 2
  ret i16 %147
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i16 @JNI_CallNonvirtualCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  %15 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %6, i32 0, i32 0
  %16 = bitcast i8** %15 to [1 x i32]*
  store [1 x i32] %4, [1 x i32]* %16, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %7, align 4
  store %struct._jobject* %1, %struct._jobject** %8, align 4
  store %struct._jobject* %2, %struct._jobject** %9, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %10, align 4
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %18 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %17, align 4
  %19 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %18, i32 0, i32 0
  %20 = load i8*, i8** %19, align 4
  %21 = bitcast i8* %20 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %23 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %24 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 0
  %25 = call i32 %21(%struct.JNINativeInterface_** noundef %22, %struct._jmethodID* noundef %23, i8* noundef %24)
  store i32 %25, i32* %12, align 4
  store i32 0, i32* %14, align 4
  br label %26

26:                                               ; preds = %133, %5
  %27 = load i32, i32* %14, align 4
  %28 = load i32, i32* %12, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %136

30:                                               ; preds = %26
  %31 = load i32, i32* %14, align 4
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %31
  %33 = load i8, i8* %32, align 1
  %34 = zext i8 %33 to i32
  switch i32 %34, label %131 [
    i32 90, label %35
    i32 66, label %45
    i32 67, label %55
    i32 83, label %65
    i32 73, label %75
    i32 74, label %84
    i32 70, label %94
    i32 68, label %108
    i32 76, label %121
  ]

35:                                               ; preds = %30
  %36 = bitcast %struct.__va_list* %6 to i8**
  %37 = load i8*, i8** %36, align 4
  %38 = getelementptr inbounds i8, i8* %37, i32 4
  store i8* %38, i8** %36, align 4
  %39 = bitcast i8* %37 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = load i32, i32* %14, align 4
  %43 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %42
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %41, i8* %44, align 8
  br label %132

45:                                               ; preds = %30
  %46 = bitcast %struct.__va_list* %6 to i8**
  %47 = load i8*, i8** %46, align 4
  %48 = getelementptr inbounds i8, i8* %47, i32 4
  store i8* %48, i8** %46, align 4
  %49 = bitcast i8* %47 to i32*
  %50 = load i32, i32* %49, align 4
  %51 = trunc i32 %50 to i8
  %52 = load i32, i32* %14, align 4
  %53 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %52
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %51, i8* %54, align 8
  br label %132

55:                                               ; preds = %30
  %56 = bitcast %struct.__va_list* %6 to i8**
  %57 = load i8*, i8** %56, align 4
  %58 = getelementptr inbounds i8, i8* %57, i32 4
  store i8* %58, i8** %56, align 4
  %59 = bitcast i8* %57 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = trunc i32 %60 to i16
  %62 = load i32, i32* %14, align 4
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %62
  %64 = bitcast %union.jvalue* %63 to i16*
  store i16 %61, i16* %64, align 8
  br label %132

65:                                               ; preds = %30
  %66 = bitcast %struct.__va_list* %6 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %14, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %132

75:                                               ; preds = %30
  %76 = bitcast %struct.__va_list* %6 to i8**
  %77 = load i8*, i8** %76, align 4
  %78 = getelementptr inbounds i8, i8* %77, i32 4
  store i8* %78, i8** %76, align 4
  %79 = bitcast i8* %77 to i32*
  %80 = load i32, i32* %79, align 4
  %81 = load i32, i32* %14, align 4
  %82 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %81
  %83 = bitcast %union.jvalue* %82 to i32*
  store i32 %80, i32* %83, align 8
  br label %132

84:                                               ; preds = %30
  %85 = bitcast %struct.__va_list* %6 to i8**
  %86 = load i8*, i8** %85, align 4
  %87 = getelementptr inbounds i8, i8* %86, i32 4
  store i8* %87, i8** %85, align 4
  %88 = bitcast i8* %86 to i32*
  %89 = load i32, i32* %88, align 4
  %90 = sext i32 %89 to i64
  %91 = load i32, i32* %14, align 4
  %92 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %91
  %93 = bitcast %union.jvalue* %92 to i64*
  store i64 %90, i64* %93, align 8
  br label %132

94:                                               ; preds = %30
  %95 = bitcast %struct.__va_list* %6 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = ptrtoint i8* %96 to i32
  %98 = add i32 %97, 7
  %99 = and i32 %98, -8
  %100 = inttoptr i32 %99 to i8*
  %101 = getelementptr inbounds i8, i8* %100, i32 8
  store i8* %101, i8** %95, align 4
  %102 = bitcast i8* %100 to double*
  %103 = load double, double* %102, align 8
  %104 = fptrunc double %103 to float
  %105 = load i32, i32* %14, align 4
  %106 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %105
  %107 = bitcast %union.jvalue* %106 to float*
  store float %104, float* %107, align 8
  br label %132

108:                                              ; preds = %30
  %109 = bitcast %struct.__va_list* %6 to i8**
  %110 = load i8*, i8** %109, align 4
  %111 = ptrtoint i8* %110 to i32
  %112 = add i32 %111, 7
  %113 = and i32 %112, -8
  %114 = inttoptr i32 %113 to i8*
  %115 = getelementptr inbounds i8, i8* %114, i32 8
  store i8* %115, i8** %109, align 4
  %116 = bitcast i8* %114 to double*
  %117 = load double, double* %116, align 8
  %118 = load i32, i32* %14, align 4
  %119 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %118
  %120 = bitcast %union.jvalue* %119 to double*
  store double %117, double* %120, align 8
  br label %132

121:                                              ; preds = %30
  %122 = bitcast %struct.__va_list* %6 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = getelementptr inbounds i8, i8* %123, i32 4
  store i8* %124, i8** %122, align 4
  %125 = bitcast i8* %123 to i8**
  %126 = load i8*, i8** %125, align 4
  %127 = bitcast i8* %126 to %struct._jobject*
  %128 = load i32, i32* %14, align 4
  %129 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %128
  %130 = bitcast %union.jvalue* %129 to %struct._jobject**
  store %struct._jobject* %127, %struct._jobject** %130, align 8
  br label %132

131:                                              ; preds = %30
  br label %132

132:                                              ; preds = %131, %121, %108, %94, %84, %75, %65, %55, %45, %35
  br label %133

133:                                              ; preds = %132
  %134 = load i32, i32* %14, align 4
  %135 = add nsw i32 %134, 1
  store i32 %135, i32* %14, align 4
  br label %26, !llvm.loop !32

136:                                              ; preds = %26
  %137 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %138 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %137, align 4
  %139 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %138, i32 0, i32 75
  %140 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %139, align 4
  %141 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %142 = load %struct._jobject*, %struct._jobject** %8, align 4
  %143 = load %struct._jobject*, %struct._jobject** %9, align 4
  %144 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %145 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 0
  %146 = call zeroext i16 %140(%struct.JNINativeInterface_** noundef %141, %struct._jobject* noundef %142, %struct._jobject* noundef %143, %struct._jmethodID* noundef %144, %union.jvalue* noundef %145)
  ret i16 %146
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i16 @JNI_CallStaticCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i16, align 2
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %13 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %13)
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 4
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %21 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %130, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %9, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %133

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  switch i32 %31, label %128 [
    i32 90, label %32
    i32 66, label %42
    i32 67, label %52
    i32 83, label %62
    i32 73, label %72
    i32 74, label %81
    i32 70, label %91
    i32 68, label %105
    i32 76, label %118
  ]

32:                                               ; preds = %27
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %129

42:                                               ; preds = %27
  %43 = bitcast %struct.__va_list* %7 to i8**
  %44 = load i8*, i8** %43, align 4
  %45 = getelementptr inbounds i8, i8* %44, i32 4
  store i8* %45, i8** %43, align 4
  %46 = bitcast i8* %44 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %11, align 4
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %48, i8* %51, align 8
  br label %129

52:                                               ; preds = %27
  %53 = bitcast %struct.__va_list* %7 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i16
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i16*
  store i16 %58, i16* %61, align 8
  br label %129

62:                                               ; preds = %27
  %63 = bitcast %struct.__va_list* %7 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load i32, i32* %11, align 4
  %70 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %69
  %71 = bitcast %union.jvalue* %70 to i16*
  store i16 %68, i16* %71, align 8
  br label %129

72:                                               ; preds = %27
  %73 = bitcast %struct.__va_list* %7 to i8**
  %74 = load i8*, i8** %73, align 4
  %75 = getelementptr inbounds i8, i8* %74, i32 4
  store i8* %75, i8** %73, align 4
  %76 = bitcast i8* %74 to i32*
  %77 = load i32, i32* %76, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %78
  %80 = bitcast %union.jvalue* %79 to i32*
  store i32 %77, i32* %80, align 8
  br label %129

81:                                               ; preds = %27
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = sext i32 %86 to i64
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i64*
  store i64 %87, i64* %90, align 8
  br label %129

91:                                               ; preds = %27
  %92 = bitcast %struct.__va_list* %7 to i8**
  %93 = load i8*, i8** %92, align 4
  %94 = ptrtoint i8* %93 to i32
  %95 = add i32 %94, 7
  %96 = and i32 %95, -8
  %97 = inttoptr i32 %96 to i8*
  %98 = getelementptr inbounds i8, i8* %97, i32 8
  store i8* %98, i8** %92, align 4
  %99 = bitcast i8* %97 to double*
  %100 = load double, double* %99, align 8
  %101 = fptrunc double %100 to float
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to float*
  store float %101, float* %104, align 8
  br label %129

105:                                              ; preds = %27
  %106 = bitcast %struct.__va_list* %7 to i8**
  %107 = load i8*, i8** %106, align 4
  %108 = ptrtoint i8* %107 to i32
  %109 = add i32 %108, 7
  %110 = and i32 %109, -8
  %111 = inttoptr i32 %110 to i8*
  %112 = getelementptr inbounds i8, i8* %111, i32 8
  store i8* %112, i8** %106, align 4
  %113 = bitcast i8* %111 to double*
  %114 = load double, double* %113, align 8
  %115 = load i32, i32* %11, align 4
  %116 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %114, double* %117, align 8
  br label %129

118:                                              ; preds = %27
  %119 = bitcast %struct.__va_list* %7 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = getelementptr inbounds i8, i8* %120, i32 4
  store i8* %121, i8** %119, align 4
  %122 = bitcast i8* %120 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = bitcast i8* %123 to %struct._jobject*
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %125
  %127 = bitcast %union.jvalue* %126 to %struct._jobject**
  store %struct._jobject* %124, %struct._jobject** %127, align 8
  br label %129

128:                                              ; preds = %27
  br label %129

129:                                              ; preds = %128, %118, %105, %91, %81, %72, %62, %52, %42, %32
  br label %130

130:                                              ; preds = %129
  %131 = load i32, i32* %11, align 4
  %132 = add nsw i32 %131, 1
  store i32 %132, i32* %11, align 4
  br label %23, !llvm.loop !33

133:                                              ; preds = %23
  %134 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %135 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %134, align 4
  %136 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %135, i32 0, i32 125
  %137 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %136, align 4
  %138 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %139 = load %struct._jobject*, %struct._jobject** %5, align 4
  %140 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %142 = call zeroext i16 %137(%struct.JNINativeInterface_** noundef %138, %struct._jobject* noundef %139, %struct._jmethodID* noundef %140, %union.jvalue* noundef %141)
  store i16 %142, i16* %12, align 2
  %143 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %143)
  %144 = load i16, i16* %12, align 2
  ret i16 %144
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i16 @JNI_CallStaticCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !34

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 125
  %138 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %143 = call zeroext i16 %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret i16 %143
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i16 @JNI_CallShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i16, align 2
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %13 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %13)
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 4
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %21 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %130, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %9, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %133

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  switch i32 %31, label %128 [
    i32 90, label %32
    i32 66, label %42
    i32 67, label %52
    i32 83, label %62
    i32 73, label %72
    i32 74, label %81
    i32 70, label %91
    i32 68, label %105
    i32 76, label %118
  ]

32:                                               ; preds = %27
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %129

42:                                               ; preds = %27
  %43 = bitcast %struct.__va_list* %7 to i8**
  %44 = load i8*, i8** %43, align 4
  %45 = getelementptr inbounds i8, i8* %44, i32 4
  store i8* %45, i8** %43, align 4
  %46 = bitcast i8* %44 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %11, align 4
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %48, i8* %51, align 8
  br label %129

52:                                               ; preds = %27
  %53 = bitcast %struct.__va_list* %7 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i16
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i16*
  store i16 %58, i16* %61, align 8
  br label %129

62:                                               ; preds = %27
  %63 = bitcast %struct.__va_list* %7 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load i32, i32* %11, align 4
  %70 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %69
  %71 = bitcast %union.jvalue* %70 to i16*
  store i16 %68, i16* %71, align 8
  br label %129

72:                                               ; preds = %27
  %73 = bitcast %struct.__va_list* %7 to i8**
  %74 = load i8*, i8** %73, align 4
  %75 = getelementptr inbounds i8, i8* %74, i32 4
  store i8* %75, i8** %73, align 4
  %76 = bitcast i8* %74 to i32*
  %77 = load i32, i32* %76, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %78
  %80 = bitcast %union.jvalue* %79 to i32*
  store i32 %77, i32* %80, align 8
  br label %129

81:                                               ; preds = %27
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = sext i32 %86 to i64
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i64*
  store i64 %87, i64* %90, align 8
  br label %129

91:                                               ; preds = %27
  %92 = bitcast %struct.__va_list* %7 to i8**
  %93 = load i8*, i8** %92, align 4
  %94 = ptrtoint i8* %93 to i32
  %95 = add i32 %94, 7
  %96 = and i32 %95, -8
  %97 = inttoptr i32 %96 to i8*
  %98 = getelementptr inbounds i8, i8* %97, i32 8
  store i8* %98, i8** %92, align 4
  %99 = bitcast i8* %97 to double*
  %100 = load double, double* %99, align 8
  %101 = fptrunc double %100 to float
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to float*
  store float %101, float* %104, align 8
  br label %129

105:                                              ; preds = %27
  %106 = bitcast %struct.__va_list* %7 to i8**
  %107 = load i8*, i8** %106, align 4
  %108 = ptrtoint i8* %107 to i32
  %109 = add i32 %108, 7
  %110 = and i32 %109, -8
  %111 = inttoptr i32 %110 to i8*
  %112 = getelementptr inbounds i8, i8* %111, i32 8
  store i8* %112, i8** %106, align 4
  %113 = bitcast i8* %111 to double*
  %114 = load double, double* %113, align 8
  %115 = load i32, i32* %11, align 4
  %116 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %114, double* %117, align 8
  br label %129

118:                                              ; preds = %27
  %119 = bitcast %struct.__va_list* %7 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = getelementptr inbounds i8, i8* %120, i32 4
  store i8* %121, i8** %119, align 4
  %122 = bitcast i8* %120 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = bitcast i8* %123 to %struct._jobject*
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %125
  %127 = bitcast %union.jvalue* %126 to %struct._jobject**
  store %struct._jobject* %124, %struct._jobject** %127, align 8
  br label %129

128:                                              ; preds = %27
  br label %129

129:                                              ; preds = %128, %118, %105, %91, %81, %72, %62, %52, %42, %32
  br label %130

130:                                              ; preds = %129
  %131 = load i32, i32* %11, align 4
  %132 = add nsw i32 %131, 1
  store i32 %132, i32* %11, align 4
  br label %23, !llvm.loop !35

133:                                              ; preds = %23
  %134 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %135 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %134, align 4
  %136 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %135, i32 0, i32 48
  %137 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %136, align 4
  %138 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %139 = load %struct._jobject*, %struct._jobject** %5, align 4
  %140 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %142 = call signext i16 %137(%struct.JNINativeInterface_** noundef %138, %struct._jobject* noundef %139, %struct._jmethodID* noundef %140, %union.jvalue* noundef %141)
  store i16 %142, i16* %12, align 2
  %143 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %143)
  %144 = load i16, i16* %12, align 2
  ret i16 %144
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i16 @JNI_CallShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !36

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 48
  %138 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %143 = call signext i16 %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret i16 %143
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i16 @JNI_CallNonvirtualShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct.__va_list, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i16, align 2
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %1, %struct._jobject** %6, align 4
  store %struct._jobject* %2, %struct._jobject** %7, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 4
  %15 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_start(i8* %15)
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  store i32 0, i32* %13, align 4
  br label %25

25:                                               ; preds = %132, %4
  %26 = load i32, i32* %13, align 4
  %27 = load i32, i32* %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %135

29:                                               ; preds = %25
  %30 = load i32, i32* %13, align 4
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %30
  %32 = load i8, i8* %31, align 1
  %33 = zext i8 %32 to i32
  switch i32 %33, label %130 [
    i32 90, label %34
    i32 66, label %44
    i32 67, label %54
    i32 83, label %64
    i32 73, label %74
    i32 74, label %83
    i32 70, label %93
    i32 68, label %107
    i32 76, label %120
  ]

34:                                               ; preds = %29
  %35 = bitcast %struct.__va_list* %9 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %13, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %131

44:                                               ; preds = %29
  %45 = bitcast %struct.__va_list* %9 to i8**
  %46 = load i8*, i8** %45, align 4
  %47 = getelementptr inbounds i8, i8* %46, i32 4
  store i8* %47, i8** %45, align 4
  %48 = bitcast i8* %46 to i32*
  %49 = load i32, i32* %48, align 4
  %50 = trunc i32 %49 to i8
  %51 = load i32, i32* %13, align 4
  %52 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %51
  %53 = bitcast %union.jvalue* %52 to i8*
  store i8 %50, i8* %53, align 8
  br label %131

54:                                               ; preds = %29
  %55 = bitcast %struct.__va_list* %9 to i8**
  %56 = load i8*, i8** %55, align 4
  %57 = getelementptr inbounds i8, i8* %56, i32 4
  store i8* %57, i8** %55, align 4
  %58 = bitcast i8* %56 to i32*
  %59 = load i32, i32* %58, align 4
  %60 = trunc i32 %59 to i16
  %61 = load i32, i32* %13, align 4
  %62 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %61
  %63 = bitcast %union.jvalue* %62 to i16*
  store i16 %60, i16* %63, align 8
  br label %131

64:                                               ; preds = %29
  %65 = bitcast %struct.__va_list* %9 to i8**
  %66 = load i8*, i8** %65, align 4
  %67 = getelementptr inbounds i8, i8* %66, i32 4
  store i8* %67, i8** %65, align 4
  %68 = bitcast i8* %66 to i32*
  %69 = load i32, i32* %68, align 4
  %70 = trunc i32 %69 to i16
  %71 = load i32, i32* %13, align 4
  %72 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %71
  %73 = bitcast %union.jvalue* %72 to i16*
  store i16 %70, i16* %73, align 8
  br label %131

74:                                               ; preds = %29
  %75 = bitcast %struct.__va_list* %9 to i8**
  %76 = load i8*, i8** %75, align 4
  %77 = getelementptr inbounds i8, i8* %76, i32 4
  store i8* %77, i8** %75, align 4
  %78 = bitcast i8* %76 to i32*
  %79 = load i32, i32* %78, align 4
  %80 = load i32, i32* %13, align 4
  %81 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %79, i32* %82, align 8
  br label %131

83:                                               ; preds = %29
  %84 = bitcast %struct.__va_list* %9 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = sext i32 %88 to i64
  %90 = load i32, i32* %13, align 4
  %91 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %90
  %92 = bitcast %union.jvalue* %91 to i64*
  store i64 %89, i64* %92, align 8
  br label %131

93:                                               ; preds = %29
  %94 = bitcast %struct.__va_list* %9 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = ptrtoint i8* %95 to i32
  %97 = add i32 %96, 7
  %98 = and i32 %97, -8
  %99 = inttoptr i32 %98 to i8*
  %100 = getelementptr inbounds i8, i8* %99, i32 8
  store i8* %100, i8** %94, align 4
  %101 = bitcast i8* %99 to double*
  %102 = load double, double* %101, align 8
  %103 = fptrunc double %102 to float
  %104 = load i32, i32* %13, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to float*
  store float %103, float* %106, align 8
  br label %131

107:                                              ; preds = %29
  %108 = bitcast %struct.__va_list* %9 to i8**
  %109 = load i8*, i8** %108, align 4
  %110 = ptrtoint i8* %109 to i32
  %111 = add i32 %110, 7
  %112 = and i32 %111, -8
  %113 = inttoptr i32 %112 to i8*
  %114 = getelementptr inbounds i8, i8* %113, i32 8
  store i8* %114, i8** %108, align 4
  %115 = bitcast i8* %113 to double*
  %116 = load double, double* %115, align 8
  %117 = load i32, i32* %13, align 4
  %118 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %117
  %119 = bitcast %union.jvalue* %118 to double*
  store double %116, double* %119, align 8
  br label %131

120:                                              ; preds = %29
  %121 = bitcast %struct.__va_list* %9 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = getelementptr inbounds i8, i8* %122, i32 4
  store i8* %123, i8** %121, align 4
  %124 = bitcast i8* %122 to i8**
  %125 = load i8*, i8** %124, align 4
  %126 = bitcast i8* %125 to %struct._jobject*
  %127 = load i32, i32* %13, align 4
  %128 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %127
  %129 = bitcast %union.jvalue* %128 to %struct._jobject**
  store %struct._jobject* %126, %struct._jobject** %129, align 8
  br label %131

130:                                              ; preds = %29
  br label %131

131:                                              ; preds = %130, %120, %107, %93, %83, %74, %64, %54, %44, %34
  br label %132

132:                                              ; preds = %131
  %133 = load i32, i32* %13, align 4
  %134 = add nsw i32 %133, 1
  store i32 %134, i32* %13, align 4
  br label %25, !llvm.loop !37

135:                                              ; preds = %25
  %136 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %137 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %136, align 4
  %138 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %137, i32 0, i32 78
  %139 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %138, align 4
  %140 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %141 = load %struct._jobject*, %struct._jobject** %6, align 4
  %142 = load %struct._jobject*, %struct._jobject** %7, align 4
  %143 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %144 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 0
  %145 = call signext i16 %139(%struct.JNINativeInterface_** noundef %140, %struct._jobject* noundef %141, %struct._jobject* noundef %142, %struct._jmethodID* noundef %143, %union.jvalue* noundef %144)
  store i16 %145, i16* %14, align 2
  %146 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %146)
  %147 = load i16, i16* %14, align 2
  ret i16 %147
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i16 @JNI_CallNonvirtualShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  %15 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %6, i32 0, i32 0
  %16 = bitcast i8** %15 to [1 x i32]*
  store [1 x i32] %4, [1 x i32]* %16, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %7, align 4
  store %struct._jobject* %1, %struct._jobject** %8, align 4
  store %struct._jobject* %2, %struct._jobject** %9, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %10, align 4
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %18 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %17, align 4
  %19 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %18, i32 0, i32 0
  %20 = load i8*, i8** %19, align 4
  %21 = bitcast i8* %20 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %23 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %24 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 0
  %25 = call i32 %21(%struct.JNINativeInterface_** noundef %22, %struct._jmethodID* noundef %23, i8* noundef %24)
  store i32 %25, i32* %12, align 4
  store i32 0, i32* %14, align 4
  br label %26

26:                                               ; preds = %133, %5
  %27 = load i32, i32* %14, align 4
  %28 = load i32, i32* %12, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %136

30:                                               ; preds = %26
  %31 = load i32, i32* %14, align 4
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %31
  %33 = load i8, i8* %32, align 1
  %34 = zext i8 %33 to i32
  switch i32 %34, label %131 [
    i32 90, label %35
    i32 66, label %45
    i32 67, label %55
    i32 83, label %65
    i32 73, label %75
    i32 74, label %84
    i32 70, label %94
    i32 68, label %108
    i32 76, label %121
  ]

35:                                               ; preds = %30
  %36 = bitcast %struct.__va_list* %6 to i8**
  %37 = load i8*, i8** %36, align 4
  %38 = getelementptr inbounds i8, i8* %37, i32 4
  store i8* %38, i8** %36, align 4
  %39 = bitcast i8* %37 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = load i32, i32* %14, align 4
  %43 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %42
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %41, i8* %44, align 8
  br label %132

45:                                               ; preds = %30
  %46 = bitcast %struct.__va_list* %6 to i8**
  %47 = load i8*, i8** %46, align 4
  %48 = getelementptr inbounds i8, i8* %47, i32 4
  store i8* %48, i8** %46, align 4
  %49 = bitcast i8* %47 to i32*
  %50 = load i32, i32* %49, align 4
  %51 = trunc i32 %50 to i8
  %52 = load i32, i32* %14, align 4
  %53 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %52
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %51, i8* %54, align 8
  br label %132

55:                                               ; preds = %30
  %56 = bitcast %struct.__va_list* %6 to i8**
  %57 = load i8*, i8** %56, align 4
  %58 = getelementptr inbounds i8, i8* %57, i32 4
  store i8* %58, i8** %56, align 4
  %59 = bitcast i8* %57 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = trunc i32 %60 to i16
  %62 = load i32, i32* %14, align 4
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %62
  %64 = bitcast %union.jvalue* %63 to i16*
  store i16 %61, i16* %64, align 8
  br label %132

65:                                               ; preds = %30
  %66 = bitcast %struct.__va_list* %6 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %14, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %132

75:                                               ; preds = %30
  %76 = bitcast %struct.__va_list* %6 to i8**
  %77 = load i8*, i8** %76, align 4
  %78 = getelementptr inbounds i8, i8* %77, i32 4
  store i8* %78, i8** %76, align 4
  %79 = bitcast i8* %77 to i32*
  %80 = load i32, i32* %79, align 4
  %81 = load i32, i32* %14, align 4
  %82 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %81
  %83 = bitcast %union.jvalue* %82 to i32*
  store i32 %80, i32* %83, align 8
  br label %132

84:                                               ; preds = %30
  %85 = bitcast %struct.__va_list* %6 to i8**
  %86 = load i8*, i8** %85, align 4
  %87 = getelementptr inbounds i8, i8* %86, i32 4
  store i8* %87, i8** %85, align 4
  %88 = bitcast i8* %86 to i32*
  %89 = load i32, i32* %88, align 4
  %90 = sext i32 %89 to i64
  %91 = load i32, i32* %14, align 4
  %92 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %91
  %93 = bitcast %union.jvalue* %92 to i64*
  store i64 %90, i64* %93, align 8
  br label %132

94:                                               ; preds = %30
  %95 = bitcast %struct.__va_list* %6 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = ptrtoint i8* %96 to i32
  %98 = add i32 %97, 7
  %99 = and i32 %98, -8
  %100 = inttoptr i32 %99 to i8*
  %101 = getelementptr inbounds i8, i8* %100, i32 8
  store i8* %101, i8** %95, align 4
  %102 = bitcast i8* %100 to double*
  %103 = load double, double* %102, align 8
  %104 = fptrunc double %103 to float
  %105 = load i32, i32* %14, align 4
  %106 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %105
  %107 = bitcast %union.jvalue* %106 to float*
  store float %104, float* %107, align 8
  br label %132

108:                                              ; preds = %30
  %109 = bitcast %struct.__va_list* %6 to i8**
  %110 = load i8*, i8** %109, align 4
  %111 = ptrtoint i8* %110 to i32
  %112 = add i32 %111, 7
  %113 = and i32 %112, -8
  %114 = inttoptr i32 %113 to i8*
  %115 = getelementptr inbounds i8, i8* %114, i32 8
  store i8* %115, i8** %109, align 4
  %116 = bitcast i8* %114 to double*
  %117 = load double, double* %116, align 8
  %118 = load i32, i32* %14, align 4
  %119 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %118
  %120 = bitcast %union.jvalue* %119 to double*
  store double %117, double* %120, align 8
  br label %132

121:                                              ; preds = %30
  %122 = bitcast %struct.__va_list* %6 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = getelementptr inbounds i8, i8* %123, i32 4
  store i8* %124, i8** %122, align 4
  %125 = bitcast i8* %123 to i8**
  %126 = load i8*, i8** %125, align 4
  %127 = bitcast i8* %126 to %struct._jobject*
  %128 = load i32, i32* %14, align 4
  %129 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %128
  %130 = bitcast %union.jvalue* %129 to %struct._jobject**
  store %struct._jobject* %127, %struct._jobject** %130, align 8
  br label %132

131:                                              ; preds = %30
  br label %132

132:                                              ; preds = %131, %121, %108, %94, %84, %75, %65, %55, %45, %35
  br label %133

133:                                              ; preds = %132
  %134 = load i32, i32* %14, align 4
  %135 = add nsw i32 %134, 1
  store i32 %135, i32* %14, align 4
  br label %26, !llvm.loop !38

136:                                              ; preds = %26
  %137 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %138 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %137, align 4
  %139 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %138, i32 0, i32 78
  %140 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %139, align 4
  %141 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %142 = load %struct._jobject*, %struct._jobject** %8, align 4
  %143 = load %struct._jobject*, %struct._jobject** %9, align 4
  %144 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %145 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 0
  %146 = call signext i16 %140(%struct.JNINativeInterface_** noundef %141, %struct._jobject* noundef %142, %struct._jobject* noundef %143, %struct._jmethodID* noundef %144, %union.jvalue* noundef %145)
  ret i16 %146
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i16 @JNI_CallStaticShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i16, align 2
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %13 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %13)
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 4
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %21 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %130, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %9, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %133

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  switch i32 %31, label %128 [
    i32 90, label %32
    i32 66, label %42
    i32 67, label %52
    i32 83, label %62
    i32 73, label %72
    i32 74, label %81
    i32 70, label %91
    i32 68, label %105
    i32 76, label %118
  ]

32:                                               ; preds = %27
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %129

42:                                               ; preds = %27
  %43 = bitcast %struct.__va_list* %7 to i8**
  %44 = load i8*, i8** %43, align 4
  %45 = getelementptr inbounds i8, i8* %44, i32 4
  store i8* %45, i8** %43, align 4
  %46 = bitcast i8* %44 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %11, align 4
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %48, i8* %51, align 8
  br label %129

52:                                               ; preds = %27
  %53 = bitcast %struct.__va_list* %7 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i16
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i16*
  store i16 %58, i16* %61, align 8
  br label %129

62:                                               ; preds = %27
  %63 = bitcast %struct.__va_list* %7 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load i32, i32* %11, align 4
  %70 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %69
  %71 = bitcast %union.jvalue* %70 to i16*
  store i16 %68, i16* %71, align 8
  br label %129

72:                                               ; preds = %27
  %73 = bitcast %struct.__va_list* %7 to i8**
  %74 = load i8*, i8** %73, align 4
  %75 = getelementptr inbounds i8, i8* %74, i32 4
  store i8* %75, i8** %73, align 4
  %76 = bitcast i8* %74 to i32*
  %77 = load i32, i32* %76, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %78
  %80 = bitcast %union.jvalue* %79 to i32*
  store i32 %77, i32* %80, align 8
  br label %129

81:                                               ; preds = %27
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = sext i32 %86 to i64
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i64*
  store i64 %87, i64* %90, align 8
  br label %129

91:                                               ; preds = %27
  %92 = bitcast %struct.__va_list* %7 to i8**
  %93 = load i8*, i8** %92, align 4
  %94 = ptrtoint i8* %93 to i32
  %95 = add i32 %94, 7
  %96 = and i32 %95, -8
  %97 = inttoptr i32 %96 to i8*
  %98 = getelementptr inbounds i8, i8* %97, i32 8
  store i8* %98, i8** %92, align 4
  %99 = bitcast i8* %97 to double*
  %100 = load double, double* %99, align 8
  %101 = fptrunc double %100 to float
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to float*
  store float %101, float* %104, align 8
  br label %129

105:                                              ; preds = %27
  %106 = bitcast %struct.__va_list* %7 to i8**
  %107 = load i8*, i8** %106, align 4
  %108 = ptrtoint i8* %107 to i32
  %109 = add i32 %108, 7
  %110 = and i32 %109, -8
  %111 = inttoptr i32 %110 to i8*
  %112 = getelementptr inbounds i8, i8* %111, i32 8
  store i8* %112, i8** %106, align 4
  %113 = bitcast i8* %111 to double*
  %114 = load double, double* %113, align 8
  %115 = load i32, i32* %11, align 4
  %116 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %114, double* %117, align 8
  br label %129

118:                                              ; preds = %27
  %119 = bitcast %struct.__va_list* %7 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = getelementptr inbounds i8, i8* %120, i32 4
  store i8* %121, i8** %119, align 4
  %122 = bitcast i8* %120 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = bitcast i8* %123 to %struct._jobject*
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %125
  %127 = bitcast %union.jvalue* %126 to %struct._jobject**
  store %struct._jobject* %124, %struct._jobject** %127, align 8
  br label %129

128:                                              ; preds = %27
  br label %129

129:                                              ; preds = %128, %118, %105, %91, %81, %72, %62, %52, %42, %32
  br label %130

130:                                              ; preds = %129
  %131 = load i32, i32* %11, align 4
  %132 = add nsw i32 %131, 1
  store i32 %132, i32* %11, align 4
  br label %23, !llvm.loop !39

133:                                              ; preds = %23
  %134 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %135 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %134, align 4
  %136 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %135, i32 0, i32 128
  %137 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %136, align 4
  %138 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %139 = load %struct._jobject*, %struct._jobject** %5, align 4
  %140 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %142 = call signext i16 %137(%struct.JNINativeInterface_** noundef %138, %struct._jobject* noundef %139, %struct._jmethodID* noundef %140, %union.jvalue* noundef %141)
  store i16 %142, i16* %12, align 2
  %143 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %143)
  %144 = load i16, i16* %12, align 2
  ret i16 %144
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i16 @JNI_CallStaticShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !40

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 128
  %138 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %143 = call signext i16 %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret i16 %143
}

; Function Attrs: noinline nounwind optnone
define dso_local i32 @JNI_CallIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %13 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %13)
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 4
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %21 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %130, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %9, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %133

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  switch i32 %31, label %128 [
    i32 90, label %32
    i32 66, label %42
    i32 67, label %52
    i32 83, label %62
    i32 73, label %72
    i32 74, label %81
    i32 70, label %91
    i32 68, label %105
    i32 76, label %118
  ]

32:                                               ; preds = %27
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %129

42:                                               ; preds = %27
  %43 = bitcast %struct.__va_list* %7 to i8**
  %44 = load i8*, i8** %43, align 4
  %45 = getelementptr inbounds i8, i8* %44, i32 4
  store i8* %45, i8** %43, align 4
  %46 = bitcast i8* %44 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %11, align 4
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %48, i8* %51, align 8
  br label %129

52:                                               ; preds = %27
  %53 = bitcast %struct.__va_list* %7 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i16
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i16*
  store i16 %58, i16* %61, align 8
  br label %129

62:                                               ; preds = %27
  %63 = bitcast %struct.__va_list* %7 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load i32, i32* %11, align 4
  %70 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %69
  %71 = bitcast %union.jvalue* %70 to i16*
  store i16 %68, i16* %71, align 8
  br label %129

72:                                               ; preds = %27
  %73 = bitcast %struct.__va_list* %7 to i8**
  %74 = load i8*, i8** %73, align 4
  %75 = getelementptr inbounds i8, i8* %74, i32 4
  store i8* %75, i8** %73, align 4
  %76 = bitcast i8* %74 to i32*
  %77 = load i32, i32* %76, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %78
  %80 = bitcast %union.jvalue* %79 to i32*
  store i32 %77, i32* %80, align 8
  br label %129

81:                                               ; preds = %27
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = sext i32 %86 to i64
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i64*
  store i64 %87, i64* %90, align 8
  br label %129

91:                                               ; preds = %27
  %92 = bitcast %struct.__va_list* %7 to i8**
  %93 = load i8*, i8** %92, align 4
  %94 = ptrtoint i8* %93 to i32
  %95 = add i32 %94, 7
  %96 = and i32 %95, -8
  %97 = inttoptr i32 %96 to i8*
  %98 = getelementptr inbounds i8, i8* %97, i32 8
  store i8* %98, i8** %92, align 4
  %99 = bitcast i8* %97 to double*
  %100 = load double, double* %99, align 8
  %101 = fptrunc double %100 to float
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to float*
  store float %101, float* %104, align 8
  br label %129

105:                                              ; preds = %27
  %106 = bitcast %struct.__va_list* %7 to i8**
  %107 = load i8*, i8** %106, align 4
  %108 = ptrtoint i8* %107 to i32
  %109 = add i32 %108, 7
  %110 = and i32 %109, -8
  %111 = inttoptr i32 %110 to i8*
  %112 = getelementptr inbounds i8, i8* %111, i32 8
  store i8* %112, i8** %106, align 4
  %113 = bitcast i8* %111 to double*
  %114 = load double, double* %113, align 8
  %115 = load i32, i32* %11, align 4
  %116 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %114, double* %117, align 8
  br label %129

118:                                              ; preds = %27
  %119 = bitcast %struct.__va_list* %7 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = getelementptr inbounds i8, i8* %120, i32 4
  store i8* %121, i8** %119, align 4
  %122 = bitcast i8* %120 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = bitcast i8* %123 to %struct._jobject*
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %125
  %127 = bitcast %union.jvalue* %126 to %struct._jobject**
  store %struct._jobject* %124, %struct._jobject** %127, align 8
  br label %129

128:                                              ; preds = %27
  br label %129

129:                                              ; preds = %128, %118, %105, %91, %81, %72, %62, %52, %42, %32
  br label %130

130:                                              ; preds = %129
  %131 = load i32, i32* %11, align 4
  %132 = add nsw i32 %131, 1
  store i32 %132, i32* %11, align 4
  br label %23, !llvm.loop !41

133:                                              ; preds = %23
  %134 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %135 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %134, align 4
  %136 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %135, i32 0, i32 51
  %137 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %136, align 4
  %138 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %139 = load %struct._jobject*, %struct._jobject** %5, align 4
  %140 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %142 = call i32 %137(%struct.JNINativeInterface_** noundef %138, %struct._jobject* noundef %139, %struct._jmethodID* noundef %140, %union.jvalue* noundef %141)
  store i32 %142, i32* %12, align 4
  %143 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %143)
  %144 = load i32, i32* %12, align 4
  ret i32 %144
}

; Function Attrs: noinline nounwind optnone
define dso_local i32 @JNI_CallIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !42

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 51
  %138 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %143 = call i32 %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret i32 %143
}

; Function Attrs: noinline nounwind optnone
define dso_local i32 @JNI_CallNonvirtualIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct.__va_list, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %1, %struct._jobject** %6, align 4
  store %struct._jobject* %2, %struct._jobject** %7, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 4
  %15 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_start(i8* %15)
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  store i32 0, i32* %13, align 4
  br label %25

25:                                               ; preds = %132, %4
  %26 = load i32, i32* %13, align 4
  %27 = load i32, i32* %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %135

29:                                               ; preds = %25
  %30 = load i32, i32* %13, align 4
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %30
  %32 = load i8, i8* %31, align 1
  %33 = zext i8 %32 to i32
  switch i32 %33, label %130 [
    i32 90, label %34
    i32 66, label %44
    i32 67, label %54
    i32 83, label %64
    i32 73, label %74
    i32 74, label %83
    i32 70, label %93
    i32 68, label %107
    i32 76, label %120
  ]

34:                                               ; preds = %29
  %35 = bitcast %struct.__va_list* %9 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %13, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %131

44:                                               ; preds = %29
  %45 = bitcast %struct.__va_list* %9 to i8**
  %46 = load i8*, i8** %45, align 4
  %47 = getelementptr inbounds i8, i8* %46, i32 4
  store i8* %47, i8** %45, align 4
  %48 = bitcast i8* %46 to i32*
  %49 = load i32, i32* %48, align 4
  %50 = trunc i32 %49 to i8
  %51 = load i32, i32* %13, align 4
  %52 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %51
  %53 = bitcast %union.jvalue* %52 to i8*
  store i8 %50, i8* %53, align 8
  br label %131

54:                                               ; preds = %29
  %55 = bitcast %struct.__va_list* %9 to i8**
  %56 = load i8*, i8** %55, align 4
  %57 = getelementptr inbounds i8, i8* %56, i32 4
  store i8* %57, i8** %55, align 4
  %58 = bitcast i8* %56 to i32*
  %59 = load i32, i32* %58, align 4
  %60 = trunc i32 %59 to i16
  %61 = load i32, i32* %13, align 4
  %62 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %61
  %63 = bitcast %union.jvalue* %62 to i16*
  store i16 %60, i16* %63, align 8
  br label %131

64:                                               ; preds = %29
  %65 = bitcast %struct.__va_list* %9 to i8**
  %66 = load i8*, i8** %65, align 4
  %67 = getelementptr inbounds i8, i8* %66, i32 4
  store i8* %67, i8** %65, align 4
  %68 = bitcast i8* %66 to i32*
  %69 = load i32, i32* %68, align 4
  %70 = trunc i32 %69 to i16
  %71 = load i32, i32* %13, align 4
  %72 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %71
  %73 = bitcast %union.jvalue* %72 to i16*
  store i16 %70, i16* %73, align 8
  br label %131

74:                                               ; preds = %29
  %75 = bitcast %struct.__va_list* %9 to i8**
  %76 = load i8*, i8** %75, align 4
  %77 = getelementptr inbounds i8, i8* %76, i32 4
  store i8* %77, i8** %75, align 4
  %78 = bitcast i8* %76 to i32*
  %79 = load i32, i32* %78, align 4
  %80 = load i32, i32* %13, align 4
  %81 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %79, i32* %82, align 8
  br label %131

83:                                               ; preds = %29
  %84 = bitcast %struct.__va_list* %9 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = sext i32 %88 to i64
  %90 = load i32, i32* %13, align 4
  %91 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %90
  %92 = bitcast %union.jvalue* %91 to i64*
  store i64 %89, i64* %92, align 8
  br label %131

93:                                               ; preds = %29
  %94 = bitcast %struct.__va_list* %9 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = ptrtoint i8* %95 to i32
  %97 = add i32 %96, 7
  %98 = and i32 %97, -8
  %99 = inttoptr i32 %98 to i8*
  %100 = getelementptr inbounds i8, i8* %99, i32 8
  store i8* %100, i8** %94, align 4
  %101 = bitcast i8* %99 to double*
  %102 = load double, double* %101, align 8
  %103 = fptrunc double %102 to float
  %104 = load i32, i32* %13, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to float*
  store float %103, float* %106, align 8
  br label %131

107:                                              ; preds = %29
  %108 = bitcast %struct.__va_list* %9 to i8**
  %109 = load i8*, i8** %108, align 4
  %110 = ptrtoint i8* %109 to i32
  %111 = add i32 %110, 7
  %112 = and i32 %111, -8
  %113 = inttoptr i32 %112 to i8*
  %114 = getelementptr inbounds i8, i8* %113, i32 8
  store i8* %114, i8** %108, align 4
  %115 = bitcast i8* %113 to double*
  %116 = load double, double* %115, align 8
  %117 = load i32, i32* %13, align 4
  %118 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %117
  %119 = bitcast %union.jvalue* %118 to double*
  store double %116, double* %119, align 8
  br label %131

120:                                              ; preds = %29
  %121 = bitcast %struct.__va_list* %9 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = getelementptr inbounds i8, i8* %122, i32 4
  store i8* %123, i8** %121, align 4
  %124 = bitcast i8* %122 to i8**
  %125 = load i8*, i8** %124, align 4
  %126 = bitcast i8* %125 to %struct._jobject*
  %127 = load i32, i32* %13, align 4
  %128 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %127
  %129 = bitcast %union.jvalue* %128 to %struct._jobject**
  store %struct._jobject* %126, %struct._jobject** %129, align 8
  br label %131

130:                                              ; preds = %29
  br label %131

131:                                              ; preds = %130, %120, %107, %93, %83, %74, %64, %54, %44, %34
  br label %132

132:                                              ; preds = %131
  %133 = load i32, i32* %13, align 4
  %134 = add nsw i32 %133, 1
  store i32 %134, i32* %13, align 4
  br label %25, !llvm.loop !43

135:                                              ; preds = %25
  %136 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %137 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %136, align 4
  %138 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %137, i32 0, i32 81
  %139 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %138, align 4
  %140 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %141 = load %struct._jobject*, %struct._jobject** %6, align 4
  %142 = load %struct._jobject*, %struct._jobject** %7, align 4
  %143 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %144 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 0
  %145 = call i32 %139(%struct.JNINativeInterface_** noundef %140, %struct._jobject* noundef %141, %struct._jobject* noundef %142, %struct._jmethodID* noundef %143, %union.jvalue* noundef %144)
  store i32 %145, i32* %14, align 4
  %146 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %146)
  %147 = load i32, i32* %14, align 4
  ret i32 %147
}

; Function Attrs: noinline nounwind optnone
define dso_local i32 @JNI_CallNonvirtualIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  %15 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %6, i32 0, i32 0
  %16 = bitcast i8** %15 to [1 x i32]*
  store [1 x i32] %4, [1 x i32]* %16, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %7, align 4
  store %struct._jobject* %1, %struct._jobject** %8, align 4
  store %struct._jobject* %2, %struct._jobject** %9, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %10, align 4
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %18 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %17, align 4
  %19 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %18, i32 0, i32 0
  %20 = load i8*, i8** %19, align 4
  %21 = bitcast i8* %20 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %23 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %24 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 0
  %25 = call i32 %21(%struct.JNINativeInterface_** noundef %22, %struct._jmethodID* noundef %23, i8* noundef %24)
  store i32 %25, i32* %12, align 4
  store i32 0, i32* %14, align 4
  br label %26

26:                                               ; preds = %133, %5
  %27 = load i32, i32* %14, align 4
  %28 = load i32, i32* %12, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %136

30:                                               ; preds = %26
  %31 = load i32, i32* %14, align 4
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %31
  %33 = load i8, i8* %32, align 1
  %34 = zext i8 %33 to i32
  switch i32 %34, label %131 [
    i32 90, label %35
    i32 66, label %45
    i32 67, label %55
    i32 83, label %65
    i32 73, label %75
    i32 74, label %84
    i32 70, label %94
    i32 68, label %108
    i32 76, label %121
  ]

35:                                               ; preds = %30
  %36 = bitcast %struct.__va_list* %6 to i8**
  %37 = load i8*, i8** %36, align 4
  %38 = getelementptr inbounds i8, i8* %37, i32 4
  store i8* %38, i8** %36, align 4
  %39 = bitcast i8* %37 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = load i32, i32* %14, align 4
  %43 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %42
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %41, i8* %44, align 8
  br label %132

45:                                               ; preds = %30
  %46 = bitcast %struct.__va_list* %6 to i8**
  %47 = load i8*, i8** %46, align 4
  %48 = getelementptr inbounds i8, i8* %47, i32 4
  store i8* %48, i8** %46, align 4
  %49 = bitcast i8* %47 to i32*
  %50 = load i32, i32* %49, align 4
  %51 = trunc i32 %50 to i8
  %52 = load i32, i32* %14, align 4
  %53 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %52
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %51, i8* %54, align 8
  br label %132

55:                                               ; preds = %30
  %56 = bitcast %struct.__va_list* %6 to i8**
  %57 = load i8*, i8** %56, align 4
  %58 = getelementptr inbounds i8, i8* %57, i32 4
  store i8* %58, i8** %56, align 4
  %59 = bitcast i8* %57 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = trunc i32 %60 to i16
  %62 = load i32, i32* %14, align 4
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %62
  %64 = bitcast %union.jvalue* %63 to i16*
  store i16 %61, i16* %64, align 8
  br label %132

65:                                               ; preds = %30
  %66 = bitcast %struct.__va_list* %6 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %14, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %132

75:                                               ; preds = %30
  %76 = bitcast %struct.__va_list* %6 to i8**
  %77 = load i8*, i8** %76, align 4
  %78 = getelementptr inbounds i8, i8* %77, i32 4
  store i8* %78, i8** %76, align 4
  %79 = bitcast i8* %77 to i32*
  %80 = load i32, i32* %79, align 4
  %81 = load i32, i32* %14, align 4
  %82 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %81
  %83 = bitcast %union.jvalue* %82 to i32*
  store i32 %80, i32* %83, align 8
  br label %132

84:                                               ; preds = %30
  %85 = bitcast %struct.__va_list* %6 to i8**
  %86 = load i8*, i8** %85, align 4
  %87 = getelementptr inbounds i8, i8* %86, i32 4
  store i8* %87, i8** %85, align 4
  %88 = bitcast i8* %86 to i32*
  %89 = load i32, i32* %88, align 4
  %90 = sext i32 %89 to i64
  %91 = load i32, i32* %14, align 4
  %92 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %91
  %93 = bitcast %union.jvalue* %92 to i64*
  store i64 %90, i64* %93, align 8
  br label %132

94:                                               ; preds = %30
  %95 = bitcast %struct.__va_list* %6 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = ptrtoint i8* %96 to i32
  %98 = add i32 %97, 7
  %99 = and i32 %98, -8
  %100 = inttoptr i32 %99 to i8*
  %101 = getelementptr inbounds i8, i8* %100, i32 8
  store i8* %101, i8** %95, align 4
  %102 = bitcast i8* %100 to double*
  %103 = load double, double* %102, align 8
  %104 = fptrunc double %103 to float
  %105 = load i32, i32* %14, align 4
  %106 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %105
  %107 = bitcast %union.jvalue* %106 to float*
  store float %104, float* %107, align 8
  br label %132

108:                                              ; preds = %30
  %109 = bitcast %struct.__va_list* %6 to i8**
  %110 = load i8*, i8** %109, align 4
  %111 = ptrtoint i8* %110 to i32
  %112 = add i32 %111, 7
  %113 = and i32 %112, -8
  %114 = inttoptr i32 %113 to i8*
  %115 = getelementptr inbounds i8, i8* %114, i32 8
  store i8* %115, i8** %109, align 4
  %116 = bitcast i8* %114 to double*
  %117 = load double, double* %116, align 8
  %118 = load i32, i32* %14, align 4
  %119 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %118
  %120 = bitcast %union.jvalue* %119 to double*
  store double %117, double* %120, align 8
  br label %132

121:                                              ; preds = %30
  %122 = bitcast %struct.__va_list* %6 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = getelementptr inbounds i8, i8* %123, i32 4
  store i8* %124, i8** %122, align 4
  %125 = bitcast i8* %123 to i8**
  %126 = load i8*, i8** %125, align 4
  %127 = bitcast i8* %126 to %struct._jobject*
  %128 = load i32, i32* %14, align 4
  %129 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %128
  %130 = bitcast %union.jvalue* %129 to %struct._jobject**
  store %struct._jobject* %127, %struct._jobject** %130, align 8
  br label %132

131:                                              ; preds = %30
  br label %132

132:                                              ; preds = %131, %121, %108, %94, %84, %75, %65, %55, %45, %35
  br label %133

133:                                              ; preds = %132
  %134 = load i32, i32* %14, align 4
  %135 = add nsw i32 %134, 1
  store i32 %135, i32* %14, align 4
  br label %26, !llvm.loop !44

136:                                              ; preds = %26
  %137 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %138 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %137, align 4
  %139 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %138, i32 0, i32 81
  %140 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %139, align 4
  %141 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %142 = load %struct._jobject*, %struct._jobject** %8, align 4
  %143 = load %struct._jobject*, %struct._jobject** %9, align 4
  %144 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %145 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 0
  %146 = call i32 %140(%struct.JNINativeInterface_** noundef %141, %struct._jobject* noundef %142, %struct._jobject* noundef %143, %struct._jmethodID* noundef %144, %union.jvalue* noundef %145)
  ret i32 %146
}

; Function Attrs: noinline nounwind optnone
define dso_local i32 @JNI_CallStaticIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %13 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %13)
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 4
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %21 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %130, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %9, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %133

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  switch i32 %31, label %128 [
    i32 90, label %32
    i32 66, label %42
    i32 67, label %52
    i32 83, label %62
    i32 73, label %72
    i32 74, label %81
    i32 70, label %91
    i32 68, label %105
    i32 76, label %118
  ]

32:                                               ; preds = %27
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %129

42:                                               ; preds = %27
  %43 = bitcast %struct.__va_list* %7 to i8**
  %44 = load i8*, i8** %43, align 4
  %45 = getelementptr inbounds i8, i8* %44, i32 4
  store i8* %45, i8** %43, align 4
  %46 = bitcast i8* %44 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %11, align 4
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %48, i8* %51, align 8
  br label %129

52:                                               ; preds = %27
  %53 = bitcast %struct.__va_list* %7 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i16
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i16*
  store i16 %58, i16* %61, align 8
  br label %129

62:                                               ; preds = %27
  %63 = bitcast %struct.__va_list* %7 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load i32, i32* %11, align 4
  %70 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %69
  %71 = bitcast %union.jvalue* %70 to i16*
  store i16 %68, i16* %71, align 8
  br label %129

72:                                               ; preds = %27
  %73 = bitcast %struct.__va_list* %7 to i8**
  %74 = load i8*, i8** %73, align 4
  %75 = getelementptr inbounds i8, i8* %74, i32 4
  store i8* %75, i8** %73, align 4
  %76 = bitcast i8* %74 to i32*
  %77 = load i32, i32* %76, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %78
  %80 = bitcast %union.jvalue* %79 to i32*
  store i32 %77, i32* %80, align 8
  br label %129

81:                                               ; preds = %27
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = sext i32 %86 to i64
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i64*
  store i64 %87, i64* %90, align 8
  br label %129

91:                                               ; preds = %27
  %92 = bitcast %struct.__va_list* %7 to i8**
  %93 = load i8*, i8** %92, align 4
  %94 = ptrtoint i8* %93 to i32
  %95 = add i32 %94, 7
  %96 = and i32 %95, -8
  %97 = inttoptr i32 %96 to i8*
  %98 = getelementptr inbounds i8, i8* %97, i32 8
  store i8* %98, i8** %92, align 4
  %99 = bitcast i8* %97 to double*
  %100 = load double, double* %99, align 8
  %101 = fptrunc double %100 to float
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to float*
  store float %101, float* %104, align 8
  br label %129

105:                                              ; preds = %27
  %106 = bitcast %struct.__va_list* %7 to i8**
  %107 = load i8*, i8** %106, align 4
  %108 = ptrtoint i8* %107 to i32
  %109 = add i32 %108, 7
  %110 = and i32 %109, -8
  %111 = inttoptr i32 %110 to i8*
  %112 = getelementptr inbounds i8, i8* %111, i32 8
  store i8* %112, i8** %106, align 4
  %113 = bitcast i8* %111 to double*
  %114 = load double, double* %113, align 8
  %115 = load i32, i32* %11, align 4
  %116 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %114, double* %117, align 8
  br label %129

118:                                              ; preds = %27
  %119 = bitcast %struct.__va_list* %7 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = getelementptr inbounds i8, i8* %120, i32 4
  store i8* %121, i8** %119, align 4
  %122 = bitcast i8* %120 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = bitcast i8* %123 to %struct._jobject*
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %125
  %127 = bitcast %union.jvalue* %126 to %struct._jobject**
  store %struct._jobject* %124, %struct._jobject** %127, align 8
  br label %129

128:                                              ; preds = %27
  br label %129

129:                                              ; preds = %128, %118, %105, %91, %81, %72, %62, %52, %42, %32
  br label %130

130:                                              ; preds = %129
  %131 = load i32, i32* %11, align 4
  %132 = add nsw i32 %131, 1
  store i32 %132, i32* %11, align 4
  br label %23, !llvm.loop !45

133:                                              ; preds = %23
  %134 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %135 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %134, align 4
  %136 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %135, i32 0, i32 131
  %137 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %136, align 4
  %138 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %139 = load %struct._jobject*, %struct._jobject** %5, align 4
  %140 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %142 = call i32 %137(%struct.JNINativeInterface_** noundef %138, %struct._jobject* noundef %139, %struct._jmethodID* noundef %140, %union.jvalue* noundef %141)
  store i32 %142, i32* %12, align 4
  %143 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %143)
  %144 = load i32, i32* %12, align 4
  ret i32 %144
}

; Function Attrs: noinline nounwind optnone
define dso_local i32 @JNI_CallStaticIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !46

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 131
  %138 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %143 = call i32 %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret i32 %143
}

; Function Attrs: noinline nounwind optnone
define dso_local i64 @JNI_CallLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i64, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %13 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %13)
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 4
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %21 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %130, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %9, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %133

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  switch i32 %31, label %128 [
    i32 90, label %32
    i32 66, label %42
    i32 67, label %52
    i32 83, label %62
    i32 73, label %72
    i32 74, label %81
    i32 70, label %91
    i32 68, label %105
    i32 76, label %118
  ]

32:                                               ; preds = %27
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %129

42:                                               ; preds = %27
  %43 = bitcast %struct.__va_list* %7 to i8**
  %44 = load i8*, i8** %43, align 4
  %45 = getelementptr inbounds i8, i8* %44, i32 4
  store i8* %45, i8** %43, align 4
  %46 = bitcast i8* %44 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %11, align 4
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %48, i8* %51, align 8
  br label %129

52:                                               ; preds = %27
  %53 = bitcast %struct.__va_list* %7 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i16
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i16*
  store i16 %58, i16* %61, align 8
  br label %129

62:                                               ; preds = %27
  %63 = bitcast %struct.__va_list* %7 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load i32, i32* %11, align 4
  %70 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %69
  %71 = bitcast %union.jvalue* %70 to i16*
  store i16 %68, i16* %71, align 8
  br label %129

72:                                               ; preds = %27
  %73 = bitcast %struct.__va_list* %7 to i8**
  %74 = load i8*, i8** %73, align 4
  %75 = getelementptr inbounds i8, i8* %74, i32 4
  store i8* %75, i8** %73, align 4
  %76 = bitcast i8* %74 to i32*
  %77 = load i32, i32* %76, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %78
  %80 = bitcast %union.jvalue* %79 to i32*
  store i32 %77, i32* %80, align 8
  br label %129

81:                                               ; preds = %27
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = sext i32 %86 to i64
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i64*
  store i64 %87, i64* %90, align 8
  br label %129

91:                                               ; preds = %27
  %92 = bitcast %struct.__va_list* %7 to i8**
  %93 = load i8*, i8** %92, align 4
  %94 = ptrtoint i8* %93 to i32
  %95 = add i32 %94, 7
  %96 = and i32 %95, -8
  %97 = inttoptr i32 %96 to i8*
  %98 = getelementptr inbounds i8, i8* %97, i32 8
  store i8* %98, i8** %92, align 4
  %99 = bitcast i8* %97 to double*
  %100 = load double, double* %99, align 8
  %101 = fptrunc double %100 to float
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to float*
  store float %101, float* %104, align 8
  br label %129

105:                                              ; preds = %27
  %106 = bitcast %struct.__va_list* %7 to i8**
  %107 = load i8*, i8** %106, align 4
  %108 = ptrtoint i8* %107 to i32
  %109 = add i32 %108, 7
  %110 = and i32 %109, -8
  %111 = inttoptr i32 %110 to i8*
  %112 = getelementptr inbounds i8, i8* %111, i32 8
  store i8* %112, i8** %106, align 4
  %113 = bitcast i8* %111 to double*
  %114 = load double, double* %113, align 8
  %115 = load i32, i32* %11, align 4
  %116 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %114, double* %117, align 8
  br label %129

118:                                              ; preds = %27
  %119 = bitcast %struct.__va_list* %7 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = getelementptr inbounds i8, i8* %120, i32 4
  store i8* %121, i8** %119, align 4
  %122 = bitcast i8* %120 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = bitcast i8* %123 to %struct._jobject*
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %125
  %127 = bitcast %union.jvalue* %126 to %struct._jobject**
  store %struct._jobject* %124, %struct._jobject** %127, align 8
  br label %129

128:                                              ; preds = %27
  br label %129

129:                                              ; preds = %128, %118, %105, %91, %81, %72, %62, %52, %42, %32
  br label %130

130:                                              ; preds = %129
  %131 = load i32, i32* %11, align 4
  %132 = add nsw i32 %131, 1
  store i32 %132, i32* %11, align 4
  br label %23, !llvm.loop !47

133:                                              ; preds = %23
  %134 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %135 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %134, align 4
  %136 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %135, i32 0, i32 54
  %137 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %136, align 4
  %138 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %139 = load %struct._jobject*, %struct._jobject** %5, align 4
  %140 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %142 = call i64 %137(%struct.JNINativeInterface_** noundef %138, %struct._jobject* noundef %139, %struct._jmethodID* noundef %140, %union.jvalue* noundef %141)
  store i64 %142, i64* %12, align 8
  %143 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %143)
  %144 = load i64, i64* %12, align 8
  ret i64 %144
}

; Function Attrs: noinline nounwind optnone
define dso_local i64 @JNI_CallLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !48

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 54
  %138 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %143 = call i64 %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret i64 %143
}

; Function Attrs: noinline nounwind optnone
define dso_local i64 @JNI_CallNonvirtualLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct.__va_list, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i64, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %1, %struct._jobject** %6, align 4
  store %struct._jobject* %2, %struct._jobject** %7, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 4
  %15 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_start(i8* %15)
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  store i32 0, i32* %13, align 4
  br label %25

25:                                               ; preds = %132, %4
  %26 = load i32, i32* %13, align 4
  %27 = load i32, i32* %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %135

29:                                               ; preds = %25
  %30 = load i32, i32* %13, align 4
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %30
  %32 = load i8, i8* %31, align 1
  %33 = zext i8 %32 to i32
  switch i32 %33, label %130 [
    i32 90, label %34
    i32 66, label %44
    i32 67, label %54
    i32 83, label %64
    i32 73, label %74
    i32 74, label %83
    i32 70, label %93
    i32 68, label %107
    i32 76, label %120
  ]

34:                                               ; preds = %29
  %35 = bitcast %struct.__va_list* %9 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %13, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %131

44:                                               ; preds = %29
  %45 = bitcast %struct.__va_list* %9 to i8**
  %46 = load i8*, i8** %45, align 4
  %47 = getelementptr inbounds i8, i8* %46, i32 4
  store i8* %47, i8** %45, align 4
  %48 = bitcast i8* %46 to i32*
  %49 = load i32, i32* %48, align 4
  %50 = trunc i32 %49 to i8
  %51 = load i32, i32* %13, align 4
  %52 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %51
  %53 = bitcast %union.jvalue* %52 to i8*
  store i8 %50, i8* %53, align 8
  br label %131

54:                                               ; preds = %29
  %55 = bitcast %struct.__va_list* %9 to i8**
  %56 = load i8*, i8** %55, align 4
  %57 = getelementptr inbounds i8, i8* %56, i32 4
  store i8* %57, i8** %55, align 4
  %58 = bitcast i8* %56 to i32*
  %59 = load i32, i32* %58, align 4
  %60 = trunc i32 %59 to i16
  %61 = load i32, i32* %13, align 4
  %62 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %61
  %63 = bitcast %union.jvalue* %62 to i16*
  store i16 %60, i16* %63, align 8
  br label %131

64:                                               ; preds = %29
  %65 = bitcast %struct.__va_list* %9 to i8**
  %66 = load i8*, i8** %65, align 4
  %67 = getelementptr inbounds i8, i8* %66, i32 4
  store i8* %67, i8** %65, align 4
  %68 = bitcast i8* %66 to i32*
  %69 = load i32, i32* %68, align 4
  %70 = trunc i32 %69 to i16
  %71 = load i32, i32* %13, align 4
  %72 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %71
  %73 = bitcast %union.jvalue* %72 to i16*
  store i16 %70, i16* %73, align 8
  br label %131

74:                                               ; preds = %29
  %75 = bitcast %struct.__va_list* %9 to i8**
  %76 = load i8*, i8** %75, align 4
  %77 = getelementptr inbounds i8, i8* %76, i32 4
  store i8* %77, i8** %75, align 4
  %78 = bitcast i8* %76 to i32*
  %79 = load i32, i32* %78, align 4
  %80 = load i32, i32* %13, align 4
  %81 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %79, i32* %82, align 8
  br label %131

83:                                               ; preds = %29
  %84 = bitcast %struct.__va_list* %9 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = sext i32 %88 to i64
  %90 = load i32, i32* %13, align 4
  %91 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %90
  %92 = bitcast %union.jvalue* %91 to i64*
  store i64 %89, i64* %92, align 8
  br label %131

93:                                               ; preds = %29
  %94 = bitcast %struct.__va_list* %9 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = ptrtoint i8* %95 to i32
  %97 = add i32 %96, 7
  %98 = and i32 %97, -8
  %99 = inttoptr i32 %98 to i8*
  %100 = getelementptr inbounds i8, i8* %99, i32 8
  store i8* %100, i8** %94, align 4
  %101 = bitcast i8* %99 to double*
  %102 = load double, double* %101, align 8
  %103 = fptrunc double %102 to float
  %104 = load i32, i32* %13, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to float*
  store float %103, float* %106, align 8
  br label %131

107:                                              ; preds = %29
  %108 = bitcast %struct.__va_list* %9 to i8**
  %109 = load i8*, i8** %108, align 4
  %110 = ptrtoint i8* %109 to i32
  %111 = add i32 %110, 7
  %112 = and i32 %111, -8
  %113 = inttoptr i32 %112 to i8*
  %114 = getelementptr inbounds i8, i8* %113, i32 8
  store i8* %114, i8** %108, align 4
  %115 = bitcast i8* %113 to double*
  %116 = load double, double* %115, align 8
  %117 = load i32, i32* %13, align 4
  %118 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %117
  %119 = bitcast %union.jvalue* %118 to double*
  store double %116, double* %119, align 8
  br label %131

120:                                              ; preds = %29
  %121 = bitcast %struct.__va_list* %9 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = getelementptr inbounds i8, i8* %122, i32 4
  store i8* %123, i8** %121, align 4
  %124 = bitcast i8* %122 to i8**
  %125 = load i8*, i8** %124, align 4
  %126 = bitcast i8* %125 to %struct._jobject*
  %127 = load i32, i32* %13, align 4
  %128 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %127
  %129 = bitcast %union.jvalue* %128 to %struct._jobject**
  store %struct._jobject* %126, %struct._jobject** %129, align 8
  br label %131

130:                                              ; preds = %29
  br label %131

131:                                              ; preds = %130, %120, %107, %93, %83, %74, %64, %54, %44, %34
  br label %132

132:                                              ; preds = %131
  %133 = load i32, i32* %13, align 4
  %134 = add nsw i32 %133, 1
  store i32 %134, i32* %13, align 4
  br label %25, !llvm.loop !49

135:                                              ; preds = %25
  %136 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %137 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %136, align 4
  %138 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %137, i32 0, i32 84
  %139 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %138, align 4
  %140 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %141 = load %struct._jobject*, %struct._jobject** %6, align 4
  %142 = load %struct._jobject*, %struct._jobject** %7, align 4
  %143 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %144 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 0
  %145 = call i64 %139(%struct.JNINativeInterface_** noundef %140, %struct._jobject* noundef %141, %struct._jobject* noundef %142, %struct._jmethodID* noundef %143, %union.jvalue* noundef %144)
  store i64 %145, i64* %14, align 8
  %146 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %146)
  %147 = load i64, i64* %14, align 8
  ret i64 %147
}

; Function Attrs: noinline nounwind optnone
define dso_local i64 @JNI_CallNonvirtualLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  %15 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %6, i32 0, i32 0
  %16 = bitcast i8** %15 to [1 x i32]*
  store [1 x i32] %4, [1 x i32]* %16, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %7, align 4
  store %struct._jobject* %1, %struct._jobject** %8, align 4
  store %struct._jobject* %2, %struct._jobject** %9, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %10, align 4
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %18 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %17, align 4
  %19 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %18, i32 0, i32 0
  %20 = load i8*, i8** %19, align 4
  %21 = bitcast i8* %20 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %23 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %24 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 0
  %25 = call i32 %21(%struct.JNINativeInterface_** noundef %22, %struct._jmethodID* noundef %23, i8* noundef %24)
  store i32 %25, i32* %12, align 4
  store i32 0, i32* %14, align 4
  br label %26

26:                                               ; preds = %133, %5
  %27 = load i32, i32* %14, align 4
  %28 = load i32, i32* %12, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %136

30:                                               ; preds = %26
  %31 = load i32, i32* %14, align 4
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %31
  %33 = load i8, i8* %32, align 1
  %34 = zext i8 %33 to i32
  switch i32 %34, label %131 [
    i32 90, label %35
    i32 66, label %45
    i32 67, label %55
    i32 83, label %65
    i32 73, label %75
    i32 74, label %84
    i32 70, label %94
    i32 68, label %108
    i32 76, label %121
  ]

35:                                               ; preds = %30
  %36 = bitcast %struct.__va_list* %6 to i8**
  %37 = load i8*, i8** %36, align 4
  %38 = getelementptr inbounds i8, i8* %37, i32 4
  store i8* %38, i8** %36, align 4
  %39 = bitcast i8* %37 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = load i32, i32* %14, align 4
  %43 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %42
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %41, i8* %44, align 8
  br label %132

45:                                               ; preds = %30
  %46 = bitcast %struct.__va_list* %6 to i8**
  %47 = load i8*, i8** %46, align 4
  %48 = getelementptr inbounds i8, i8* %47, i32 4
  store i8* %48, i8** %46, align 4
  %49 = bitcast i8* %47 to i32*
  %50 = load i32, i32* %49, align 4
  %51 = trunc i32 %50 to i8
  %52 = load i32, i32* %14, align 4
  %53 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %52
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %51, i8* %54, align 8
  br label %132

55:                                               ; preds = %30
  %56 = bitcast %struct.__va_list* %6 to i8**
  %57 = load i8*, i8** %56, align 4
  %58 = getelementptr inbounds i8, i8* %57, i32 4
  store i8* %58, i8** %56, align 4
  %59 = bitcast i8* %57 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = trunc i32 %60 to i16
  %62 = load i32, i32* %14, align 4
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %62
  %64 = bitcast %union.jvalue* %63 to i16*
  store i16 %61, i16* %64, align 8
  br label %132

65:                                               ; preds = %30
  %66 = bitcast %struct.__va_list* %6 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %14, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %132

75:                                               ; preds = %30
  %76 = bitcast %struct.__va_list* %6 to i8**
  %77 = load i8*, i8** %76, align 4
  %78 = getelementptr inbounds i8, i8* %77, i32 4
  store i8* %78, i8** %76, align 4
  %79 = bitcast i8* %77 to i32*
  %80 = load i32, i32* %79, align 4
  %81 = load i32, i32* %14, align 4
  %82 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %81
  %83 = bitcast %union.jvalue* %82 to i32*
  store i32 %80, i32* %83, align 8
  br label %132

84:                                               ; preds = %30
  %85 = bitcast %struct.__va_list* %6 to i8**
  %86 = load i8*, i8** %85, align 4
  %87 = getelementptr inbounds i8, i8* %86, i32 4
  store i8* %87, i8** %85, align 4
  %88 = bitcast i8* %86 to i32*
  %89 = load i32, i32* %88, align 4
  %90 = sext i32 %89 to i64
  %91 = load i32, i32* %14, align 4
  %92 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %91
  %93 = bitcast %union.jvalue* %92 to i64*
  store i64 %90, i64* %93, align 8
  br label %132

94:                                               ; preds = %30
  %95 = bitcast %struct.__va_list* %6 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = ptrtoint i8* %96 to i32
  %98 = add i32 %97, 7
  %99 = and i32 %98, -8
  %100 = inttoptr i32 %99 to i8*
  %101 = getelementptr inbounds i8, i8* %100, i32 8
  store i8* %101, i8** %95, align 4
  %102 = bitcast i8* %100 to double*
  %103 = load double, double* %102, align 8
  %104 = fptrunc double %103 to float
  %105 = load i32, i32* %14, align 4
  %106 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %105
  %107 = bitcast %union.jvalue* %106 to float*
  store float %104, float* %107, align 8
  br label %132

108:                                              ; preds = %30
  %109 = bitcast %struct.__va_list* %6 to i8**
  %110 = load i8*, i8** %109, align 4
  %111 = ptrtoint i8* %110 to i32
  %112 = add i32 %111, 7
  %113 = and i32 %112, -8
  %114 = inttoptr i32 %113 to i8*
  %115 = getelementptr inbounds i8, i8* %114, i32 8
  store i8* %115, i8** %109, align 4
  %116 = bitcast i8* %114 to double*
  %117 = load double, double* %116, align 8
  %118 = load i32, i32* %14, align 4
  %119 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %118
  %120 = bitcast %union.jvalue* %119 to double*
  store double %117, double* %120, align 8
  br label %132

121:                                              ; preds = %30
  %122 = bitcast %struct.__va_list* %6 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = getelementptr inbounds i8, i8* %123, i32 4
  store i8* %124, i8** %122, align 4
  %125 = bitcast i8* %123 to i8**
  %126 = load i8*, i8** %125, align 4
  %127 = bitcast i8* %126 to %struct._jobject*
  %128 = load i32, i32* %14, align 4
  %129 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %128
  %130 = bitcast %union.jvalue* %129 to %struct._jobject**
  store %struct._jobject* %127, %struct._jobject** %130, align 8
  br label %132

131:                                              ; preds = %30
  br label %132

132:                                              ; preds = %131, %121, %108, %94, %84, %75, %65, %55, %45, %35
  br label %133

133:                                              ; preds = %132
  %134 = load i32, i32* %14, align 4
  %135 = add nsw i32 %134, 1
  store i32 %135, i32* %14, align 4
  br label %26, !llvm.loop !50

136:                                              ; preds = %26
  %137 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %138 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %137, align 4
  %139 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %138, i32 0, i32 84
  %140 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %139, align 4
  %141 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %142 = load %struct._jobject*, %struct._jobject** %8, align 4
  %143 = load %struct._jobject*, %struct._jobject** %9, align 4
  %144 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %145 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 0
  %146 = call i64 %140(%struct.JNINativeInterface_** noundef %141, %struct._jobject* noundef %142, %struct._jobject* noundef %143, %struct._jmethodID* noundef %144, %union.jvalue* noundef %145)
  ret i64 %146
}

; Function Attrs: noinline nounwind optnone
define dso_local i64 @JNI_CallStaticLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i64, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %13 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %13)
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 4
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %21 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %130, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %9, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %133

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  switch i32 %31, label %128 [
    i32 90, label %32
    i32 66, label %42
    i32 67, label %52
    i32 83, label %62
    i32 73, label %72
    i32 74, label %81
    i32 70, label %91
    i32 68, label %105
    i32 76, label %118
  ]

32:                                               ; preds = %27
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %129

42:                                               ; preds = %27
  %43 = bitcast %struct.__va_list* %7 to i8**
  %44 = load i8*, i8** %43, align 4
  %45 = getelementptr inbounds i8, i8* %44, i32 4
  store i8* %45, i8** %43, align 4
  %46 = bitcast i8* %44 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %11, align 4
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %48, i8* %51, align 8
  br label %129

52:                                               ; preds = %27
  %53 = bitcast %struct.__va_list* %7 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i16
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i16*
  store i16 %58, i16* %61, align 8
  br label %129

62:                                               ; preds = %27
  %63 = bitcast %struct.__va_list* %7 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load i32, i32* %11, align 4
  %70 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %69
  %71 = bitcast %union.jvalue* %70 to i16*
  store i16 %68, i16* %71, align 8
  br label %129

72:                                               ; preds = %27
  %73 = bitcast %struct.__va_list* %7 to i8**
  %74 = load i8*, i8** %73, align 4
  %75 = getelementptr inbounds i8, i8* %74, i32 4
  store i8* %75, i8** %73, align 4
  %76 = bitcast i8* %74 to i32*
  %77 = load i32, i32* %76, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %78
  %80 = bitcast %union.jvalue* %79 to i32*
  store i32 %77, i32* %80, align 8
  br label %129

81:                                               ; preds = %27
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = sext i32 %86 to i64
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i64*
  store i64 %87, i64* %90, align 8
  br label %129

91:                                               ; preds = %27
  %92 = bitcast %struct.__va_list* %7 to i8**
  %93 = load i8*, i8** %92, align 4
  %94 = ptrtoint i8* %93 to i32
  %95 = add i32 %94, 7
  %96 = and i32 %95, -8
  %97 = inttoptr i32 %96 to i8*
  %98 = getelementptr inbounds i8, i8* %97, i32 8
  store i8* %98, i8** %92, align 4
  %99 = bitcast i8* %97 to double*
  %100 = load double, double* %99, align 8
  %101 = fptrunc double %100 to float
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to float*
  store float %101, float* %104, align 8
  br label %129

105:                                              ; preds = %27
  %106 = bitcast %struct.__va_list* %7 to i8**
  %107 = load i8*, i8** %106, align 4
  %108 = ptrtoint i8* %107 to i32
  %109 = add i32 %108, 7
  %110 = and i32 %109, -8
  %111 = inttoptr i32 %110 to i8*
  %112 = getelementptr inbounds i8, i8* %111, i32 8
  store i8* %112, i8** %106, align 4
  %113 = bitcast i8* %111 to double*
  %114 = load double, double* %113, align 8
  %115 = load i32, i32* %11, align 4
  %116 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %114, double* %117, align 8
  br label %129

118:                                              ; preds = %27
  %119 = bitcast %struct.__va_list* %7 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = getelementptr inbounds i8, i8* %120, i32 4
  store i8* %121, i8** %119, align 4
  %122 = bitcast i8* %120 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = bitcast i8* %123 to %struct._jobject*
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %125
  %127 = bitcast %union.jvalue* %126 to %struct._jobject**
  store %struct._jobject* %124, %struct._jobject** %127, align 8
  br label %129

128:                                              ; preds = %27
  br label %129

129:                                              ; preds = %128, %118, %105, %91, %81, %72, %62, %52, %42, %32
  br label %130

130:                                              ; preds = %129
  %131 = load i32, i32* %11, align 4
  %132 = add nsw i32 %131, 1
  store i32 %132, i32* %11, align 4
  br label %23, !llvm.loop !51

133:                                              ; preds = %23
  %134 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %135 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %134, align 4
  %136 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %135, i32 0, i32 134
  %137 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %136, align 4
  %138 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %139 = load %struct._jobject*, %struct._jobject** %5, align 4
  %140 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %142 = call i64 %137(%struct.JNINativeInterface_** noundef %138, %struct._jobject* noundef %139, %struct._jmethodID* noundef %140, %union.jvalue* noundef %141)
  store i64 %142, i64* %12, align 8
  %143 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %143)
  %144 = load i64, i64* %12, align 8
  ret i64 %144
}

; Function Attrs: noinline nounwind optnone
define dso_local i64 @JNI_CallStaticLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !52

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 134
  %138 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %143 = call i64 %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret i64 %143
}

; Function Attrs: noinline nounwind optnone
define dso_local float @JNI_CallFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca float, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %13 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %13)
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 4
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %21 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %130, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %9, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %133

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  switch i32 %31, label %128 [
    i32 90, label %32
    i32 66, label %42
    i32 67, label %52
    i32 83, label %62
    i32 73, label %72
    i32 74, label %81
    i32 70, label %91
    i32 68, label %105
    i32 76, label %118
  ]

32:                                               ; preds = %27
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %129

42:                                               ; preds = %27
  %43 = bitcast %struct.__va_list* %7 to i8**
  %44 = load i8*, i8** %43, align 4
  %45 = getelementptr inbounds i8, i8* %44, i32 4
  store i8* %45, i8** %43, align 4
  %46 = bitcast i8* %44 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %11, align 4
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %48, i8* %51, align 8
  br label %129

52:                                               ; preds = %27
  %53 = bitcast %struct.__va_list* %7 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i16
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i16*
  store i16 %58, i16* %61, align 8
  br label %129

62:                                               ; preds = %27
  %63 = bitcast %struct.__va_list* %7 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load i32, i32* %11, align 4
  %70 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %69
  %71 = bitcast %union.jvalue* %70 to i16*
  store i16 %68, i16* %71, align 8
  br label %129

72:                                               ; preds = %27
  %73 = bitcast %struct.__va_list* %7 to i8**
  %74 = load i8*, i8** %73, align 4
  %75 = getelementptr inbounds i8, i8* %74, i32 4
  store i8* %75, i8** %73, align 4
  %76 = bitcast i8* %74 to i32*
  %77 = load i32, i32* %76, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %78
  %80 = bitcast %union.jvalue* %79 to i32*
  store i32 %77, i32* %80, align 8
  br label %129

81:                                               ; preds = %27
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = sext i32 %86 to i64
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i64*
  store i64 %87, i64* %90, align 8
  br label %129

91:                                               ; preds = %27
  %92 = bitcast %struct.__va_list* %7 to i8**
  %93 = load i8*, i8** %92, align 4
  %94 = ptrtoint i8* %93 to i32
  %95 = add i32 %94, 7
  %96 = and i32 %95, -8
  %97 = inttoptr i32 %96 to i8*
  %98 = getelementptr inbounds i8, i8* %97, i32 8
  store i8* %98, i8** %92, align 4
  %99 = bitcast i8* %97 to double*
  %100 = load double, double* %99, align 8
  %101 = fptrunc double %100 to float
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to float*
  store float %101, float* %104, align 8
  br label %129

105:                                              ; preds = %27
  %106 = bitcast %struct.__va_list* %7 to i8**
  %107 = load i8*, i8** %106, align 4
  %108 = ptrtoint i8* %107 to i32
  %109 = add i32 %108, 7
  %110 = and i32 %109, -8
  %111 = inttoptr i32 %110 to i8*
  %112 = getelementptr inbounds i8, i8* %111, i32 8
  store i8* %112, i8** %106, align 4
  %113 = bitcast i8* %111 to double*
  %114 = load double, double* %113, align 8
  %115 = load i32, i32* %11, align 4
  %116 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %114, double* %117, align 8
  br label %129

118:                                              ; preds = %27
  %119 = bitcast %struct.__va_list* %7 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = getelementptr inbounds i8, i8* %120, i32 4
  store i8* %121, i8** %119, align 4
  %122 = bitcast i8* %120 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = bitcast i8* %123 to %struct._jobject*
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %125
  %127 = bitcast %union.jvalue* %126 to %struct._jobject**
  store %struct._jobject* %124, %struct._jobject** %127, align 8
  br label %129

128:                                              ; preds = %27
  br label %129

129:                                              ; preds = %128, %118, %105, %91, %81, %72, %62, %52, %42, %32
  br label %130

130:                                              ; preds = %129
  %131 = load i32, i32* %11, align 4
  %132 = add nsw i32 %131, 1
  store i32 %132, i32* %11, align 4
  br label %23, !llvm.loop !53

133:                                              ; preds = %23
  %134 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %135 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %134, align 4
  %136 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %135, i32 0, i32 57
  %137 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %136, align 4
  %138 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %139 = load %struct._jobject*, %struct._jobject** %5, align 4
  %140 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %142 = call float %137(%struct.JNINativeInterface_** noundef %138, %struct._jobject* noundef %139, %struct._jmethodID* noundef %140, %union.jvalue* noundef %141)
  store float %142, float* %12, align 4
  %143 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %143)
  %144 = load float, float* %12, align 4
  ret float %144
}

; Function Attrs: noinline nounwind optnone
define dso_local float @JNI_CallFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !54

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 57
  %138 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %143 = call float %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret float %143
}

; Function Attrs: noinline nounwind optnone
define dso_local float @JNI_CallNonvirtualFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct.__va_list, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca float, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %1, %struct._jobject** %6, align 4
  store %struct._jobject* %2, %struct._jobject** %7, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 4
  %15 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_start(i8* %15)
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  store i32 0, i32* %13, align 4
  br label %25

25:                                               ; preds = %132, %4
  %26 = load i32, i32* %13, align 4
  %27 = load i32, i32* %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %135

29:                                               ; preds = %25
  %30 = load i32, i32* %13, align 4
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %30
  %32 = load i8, i8* %31, align 1
  %33 = zext i8 %32 to i32
  switch i32 %33, label %130 [
    i32 90, label %34
    i32 66, label %44
    i32 67, label %54
    i32 83, label %64
    i32 73, label %74
    i32 74, label %83
    i32 70, label %93
    i32 68, label %107
    i32 76, label %120
  ]

34:                                               ; preds = %29
  %35 = bitcast %struct.__va_list* %9 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %13, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %131

44:                                               ; preds = %29
  %45 = bitcast %struct.__va_list* %9 to i8**
  %46 = load i8*, i8** %45, align 4
  %47 = getelementptr inbounds i8, i8* %46, i32 4
  store i8* %47, i8** %45, align 4
  %48 = bitcast i8* %46 to i32*
  %49 = load i32, i32* %48, align 4
  %50 = trunc i32 %49 to i8
  %51 = load i32, i32* %13, align 4
  %52 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %51
  %53 = bitcast %union.jvalue* %52 to i8*
  store i8 %50, i8* %53, align 8
  br label %131

54:                                               ; preds = %29
  %55 = bitcast %struct.__va_list* %9 to i8**
  %56 = load i8*, i8** %55, align 4
  %57 = getelementptr inbounds i8, i8* %56, i32 4
  store i8* %57, i8** %55, align 4
  %58 = bitcast i8* %56 to i32*
  %59 = load i32, i32* %58, align 4
  %60 = trunc i32 %59 to i16
  %61 = load i32, i32* %13, align 4
  %62 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %61
  %63 = bitcast %union.jvalue* %62 to i16*
  store i16 %60, i16* %63, align 8
  br label %131

64:                                               ; preds = %29
  %65 = bitcast %struct.__va_list* %9 to i8**
  %66 = load i8*, i8** %65, align 4
  %67 = getelementptr inbounds i8, i8* %66, i32 4
  store i8* %67, i8** %65, align 4
  %68 = bitcast i8* %66 to i32*
  %69 = load i32, i32* %68, align 4
  %70 = trunc i32 %69 to i16
  %71 = load i32, i32* %13, align 4
  %72 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %71
  %73 = bitcast %union.jvalue* %72 to i16*
  store i16 %70, i16* %73, align 8
  br label %131

74:                                               ; preds = %29
  %75 = bitcast %struct.__va_list* %9 to i8**
  %76 = load i8*, i8** %75, align 4
  %77 = getelementptr inbounds i8, i8* %76, i32 4
  store i8* %77, i8** %75, align 4
  %78 = bitcast i8* %76 to i32*
  %79 = load i32, i32* %78, align 4
  %80 = load i32, i32* %13, align 4
  %81 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %79, i32* %82, align 8
  br label %131

83:                                               ; preds = %29
  %84 = bitcast %struct.__va_list* %9 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = sext i32 %88 to i64
  %90 = load i32, i32* %13, align 4
  %91 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %90
  %92 = bitcast %union.jvalue* %91 to i64*
  store i64 %89, i64* %92, align 8
  br label %131

93:                                               ; preds = %29
  %94 = bitcast %struct.__va_list* %9 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = ptrtoint i8* %95 to i32
  %97 = add i32 %96, 7
  %98 = and i32 %97, -8
  %99 = inttoptr i32 %98 to i8*
  %100 = getelementptr inbounds i8, i8* %99, i32 8
  store i8* %100, i8** %94, align 4
  %101 = bitcast i8* %99 to double*
  %102 = load double, double* %101, align 8
  %103 = fptrunc double %102 to float
  %104 = load i32, i32* %13, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to float*
  store float %103, float* %106, align 8
  br label %131

107:                                              ; preds = %29
  %108 = bitcast %struct.__va_list* %9 to i8**
  %109 = load i8*, i8** %108, align 4
  %110 = ptrtoint i8* %109 to i32
  %111 = add i32 %110, 7
  %112 = and i32 %111, -8
  %113 = inttoptr i32 %112 to i8*
  %114 = getelementptr inbounds i8, i8* %113, i32 8
  store i8* %114, i8** %108, align 4
  %115 = bitcast i8* %113 to double*
  %116 = load double, double* %115, align 8
  %117 = load i32, i32* %13, align 4
  %118 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %117
  %119 = bitcast %union.jvalue* %118 to double*
  store double %116, double* %119, align 8
  br label %131

120:                                              ; preds = %29
  %121 = bitcast %struct.__va_list* %9 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = getelementptr inbounds i8, i8* %122, i32 4
  store i8* %123, i8** %121, align 4
  %124 = bitcast i8* %122 to i8**
  %125 = load i8*, i8** %124, align 4
  %126 = bitcast i8* %125 to %struct._jobject*
  %127 = load i32, i32* %13, align 4
  %128 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %127
  %129 = bitcast %union.jvalue* %128 to %struct._jobject**
  store %struct._jobject* %126, %struct._jobject** %129, align 8
  br label %131

130:                                              ; preds = %29
  br label %131

131:                                              ; preds = %130, %120, %107, %93, %83, %74, %64, %54, %44, %34
  br label %132

132:                                              ; preds = %131
  %133 = load i32, i32* %13, align 4
  %134 = add nsw i32 %133, 1
  store i32 %134, i32* %13, align 4
  br label %25, !llvm.loop !55

135:                                              ; preds = %25
  %136 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %137 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %136, align 4
  %138 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %137, i32 0, i32 87
  %139 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %138, align 4
  %140 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %141 = load %struct._jobject*, %struct._jobject** %6, align 4
  %142 = load %struct._jobject*, %struct._jobject** %7, align 4
  %143 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %144 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 0
  %145 = call float %139(%struct.JNINativeInterface_** noundef %140, %struct._jobject* noundef %141, %struct._jobject* noundef %142, %struct._jmethodID* noundef %143, %union.jvalue* noundef %144)
  store float %145, float* %14, align 4
  %146 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %146)
  %147 = load float, float* %14, align 4
  ret float %147
}

; Function Attrs: noinline nounwind optnone
define dso_local float @JNI_CallNonvirtualFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  %15 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %6, i32 0, i32 0
  %16 = bitcast i8** %15 to [1 x i32]*
  store [1 x i32] %4, [1 x i32]* %16, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %7, align 4
  store %struct._jobject* %1, %struct._jobject** %8, align 4
  store %struct._jobject* %2, %struct._jobject** %9, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %10, align 4
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %18 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %17, align 4
  %19 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %18, i32 0, i32 0
  %20 = load i8*, i8** %19, align 4
  %21 = bitcast i8* %20 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %23 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %24 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 0
  %25 = call i32 %21(%struct.JNINativeInterface_** noundef %22, %struct._jmethodID* noundef %23, i8* noundef %24)
  store i32 %25, i32* %12, align 4
  store i32 0, i32* %14, align 4
  br label %26

26:                                               ; preds = %133, %5
  %27 = load i32, i32* %14, align 4
  %28 = load i32, i32* %12, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %136

30:                                               ; preds = %26
  %31 = load i32, i32* %14, align 4
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %31
  %33 = load i8, i8* %32, align 1
  %34 = zext i8 %33 to i32
  switch i32 %34, label %131 [
    i32 90, label %35
    i32 66, label %45
    i32 67, label %55
    i32 83, label %65
    i32 73, label %75
    i32 74, label %84
    i32 70, label %94
    i32 68, label %108
    i32 76, label %121
  ]

35:                                               ; preds = %30
  %36 = bitcast %struct.__va_list* %6 to i8**
  %37 = load i8*, i8** %36, align 4
  %38 = getelementptr inbounds i8, i8* %37, i32 4
  store i8* %38, i8** %36, align 4
  %39 = bitcast i8* %37 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = load i32, i32* %14, align 4
  %43 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %42
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %41, i8* %44, align 8
  br label %132

45:                                               ; preds = %30
  %46 = bitcast %struct.__va_list* %6 to i8**
  %47 = load i8*, i8** %46, align 4
  %48 = getelementptr inbounds i8, i8* %47, i32 4
  store i8* %48, i8** %46, align 4
  %49 = bitcast i8* %47 to i32*
  %50 = load i32, i32* %49, align 4
  %51 = trunc i32 %50 to i8
  %52 = load i32, i32* %14, align 4
  %53 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %52
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %51, i8* %54, align 8
  br label %132

55:                                               ; preds = %30
  %56 = bitcast %struct.__va_list* %6 to i8**
  %57 = load i8*, i8** %56, align 4
  %58 = getelementptr inbounds i8, i8* %57, i32 4
  store i8* %58, i8** %56, align 4
  %59 = bitcast i8* %57 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = trunc i32 %60 to i16
  %62 = load i32, i32* %14, align 4
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %62
  %64 = bitcast %union.jvalue* %63 to i16*
  store i16 %61, i16* %64, align 8
  br label %132

65:                                               ; preds = %30
  %66 = bitcast %struct.__va_list* %6 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %14, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %132

75:                                               ; preds = %30
  %76 = bitcast %struct.__va_list* %6 to i8**
  %77 = load i8*, i8** %76, align 4
  %78 = getelementptr inbounds i8, i8* %77, i32 4
  store i8* %78, i8** %76, align 4
  %79 = bitcast i8* %77 to i32*
  %80 = load i32, i32* %79, align 4
  %81 = load i32, i32* %14, align 4
  %82 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %81
  %83 = bitcast %union.jvalue* %82 to i32*
  store i32 %80, i32* %83, align 8
  br label %132

84:                                               ; preds = %30
  %85 = bitcast %struct.__va_list* %6 to i8**
  %86 = load i8*, i8** %85, align 4
  %87 = getelementptr inbounds i8, i8* %86, i32 4
  store i8* %87, i8** %85, align 4
  %88 = bitcast i8* %86 to i32*
  %89 = load i32, i32* %88, align 4
  %90 = sext i32 %89 to i64
  %91 = load i32, i32* %14, align 4
  %92 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %91
  %93 = bitcast %union.jvalue* %92 to i64*
  store i64 %90, i64* %93, align 8
  br label %132

94:                                               ; preds = %30
  %95 = bitcast %struct.__va_list* %6 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = ptrtoint i8* %96 to i32
  %98 = add i32 %97, 7
  %99 = and i32 %98, -8
  %100 = inttoptr i32 %99 to i8*
  %101 = getelementptr inbounds i8, i8* %100, i32 8
  store i8* %101, i8** %95, align 4
  %102 = bitcast i8* %100 to double*
  %103 = load double, double* %102, align 8
  %104 = fptrunc double %103 to float
  %105 = load i32, i32* %14, align 4
  %106 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %105
  %107 = bitcast %union.jvalue* %106 to float*
  store float %104, float* %107, align 8
  br label %132

108:                                              ; preds = %30
  %109 = bitcast %struct.__va_list* %6 to i8**
  %110 = load i8*, i8** %109, align 4
  %111 = ptrtoint i8* %110 to i32
  %112 = add i32 %111, 7
  %113 = and i32 %112, -8
  %114 = inttoptr i32 %113 to i8*
  %115 = getelementptr inbounds i8, i8* %114, i32 8
  store i8* %115, i8** %109, align 4
  %116 = bitcast i8* %114 to double*
  %117 = load double, double* %116, align 8
  %118 = load i32, i32* %14, align 4
  %119 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %118
  %120 = bitcast %union.jvalue* %119 to double*
  store double %117, double* %120, align 8
  br label %132

121:                                              ; preds = %30
  %122 = bitcast %struct.__va_list* %6 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = getelementptr inbounds i8, i8* %123, i32 4
  store i8* %124, i8** %122, align 4
  %125 = bitcast i8* %123 to i8**
  %126 = load i8*, i8** %125, align 4
  %127 = bitcast i8* %126 to %struct._jobject*
  %128 = load i32, i32* %14, align 4
  %129 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %128
  %130 = bitcast %union.jvalue* %129 to %struct._jobject**
  store %struct._jobject* %127, %struct._jobject** %130, align 8
  br label %132

131:                                              ; preds = %30
  br label %132

132:                                              ; preds = %131, %121, %108, %94, %84, %75, %65, %55, %45, %35
  br label %133

133:                                              ; preds = %132
  %134 = load i32, i32* %14, align 4
  %135 = add nsw i32 %134, 1
  store i32 %135, i32* %14, align 4
  br label %26, !llvm.loop !56

136:                                              ; preds = %26
  %137 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %138 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %137, align 4
  %139 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %138, i32 0, i32 87
  %140 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %139, align 4
  %141 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %142 = load %struct._jobject*, %struct._jobject** %8, align 4
  %143 = load %struct._jobject*, %struct._jobject** %9, align 4
  %144 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %145 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 0
  %146 = call float %140(%struct.JNINativeInterface_** noundef %141, %struct._jobject* noundef %142, %struct._jobject* noundef %143, %struct._jmethodID* noundef %144, %union.jvalue* noundef %145)
  ret float %146
}

; Function Attrs: noinline nounwind optnone
define dso_local float @JNI_CallStaticFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca float, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %13 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %13)
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 4
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %21 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %130, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %9, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %133

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  switch i32 %31, label %128 [
    i32 90, label %32
    i32 66, label %42
    i32 67, label %52
    i32 83, label %62
    i32 73, label %72
    i32 74, label %81
    i32 70, label %91
    i32 68, label %105
    i32 76, label %118
  ]

32:                                               ; preds = %27
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %129

42:                                               ; preds = %27
  %43 = bitcast %struct.__va_list* %7 to i8**
  %44 = load i8*, i8** %43, align 4
  %45 = getelementptr inbounds i8, i8* %44, i32 4
  store i8* %45, i8** %43, align 4
  %46 = bitcast i8* %44 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %11, align 4
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %48, i8* %51, align 8
  br label %129

52:                                               ; preds = %27
  %53 = bitcast %struct.__va_list* %7 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i16
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i16*
  store i16 %58, i16* %61, align 8
  br label %129

62:                                               ; preds = %27
  %63 = bitcast %struct.__va_list* %7 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load i32, i32* %11, align 4
  %70 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %69
  %71 = bitcast %union.jvalue* %70 to i16*
  store i16 %68, i16* %71, align 8
  br label %129

72:                                               ; preds = %27
  %73 = bitcast %struct.__va_list* %7 to i8**
  %74 = load i8*, i8** %73, align 4
  %75 = getelementptr inbounds i8, i8* %74, i32 4
  store i8* %75, i8** %73, align 4
  %76 = bitcast i8* %74 to i32*
  %77 = load i32, i32* %76, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %78
  %80 = bitcast %union.jvalue* %79 to i32*
  store i32 %77, i32* %80, align 8
  br label %129

81:                                               ; preds = %27
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = sext i32 %86 to i64
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i64*
  store i64 %87, i64* %90, align 8
  br label %129

91:                                               ; preds = %27
  %92 = bitcast %struct.__va_list* %7 to i8**
  %93 = load i8*, i8** %92, align 4
  %94 = ptrtoint i8* %93 to i32
  %95 = add i32 %94, 7
  %96 = and i32 %95, -8
  %97 = inttoptr i32 %96 to i8*
  %98 = getelementptr inbounds i8, i8* %97, i32 8
  store i8* %98, i8** %92, align 4
  %99 = bitcast i8* %97 to double*
  %100 = load double, double* %99, align 8
  %101 = fptrunc double %100 to float
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to float*
  store float %101, float* %104, align 8
  br label %129

105:                                              ; preds = %27
  %106 = bitcast %struct.__va_list* %7 to i8**
  %107 = load i8*, i8** %106, align 4
  %108 = ptrtoint i8* %107 to i32
  %109 = add i32 %108, 7
  %110 = and i32 %109, -8
  %111 = inttoptr i32 %110 to i8*
  %112 = getelementptr inbounds i8, i8* %111, i32 8
  store i8* %112, i8** %106, align 4
  %113 = bitcast i8* %111 to double*
  %114 = load double, double* %113, align 8
  %115 = load i32, i32* %11, align 4
  %116 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %114, double* %117, align 8
  br label %129

118:                                              ; preds = %27
  %119 = bitcast %struct.__va_list* %7 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = getelementptr inbounds i8, i8* %120, i32 4
  store i8* %121, i8** %119, align 4
  %122 = bitcast i8* %120 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = bitcast i8* %123 to %struct._jobject*
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %125
  %127 = bitcast %union.jvalue* %126 to %struct._jobject**
  store %struct._jobject* %124, %struct._jobject** %127, align 8
  br label %129

128:                                              ; preds = %27
  br label %129

129:                                              ; preds = %128, %118, %105, %91, %81, %72, %62, %52, %42, %32
  br label %130

130:                                              ; preds = %129
  %131 = load i32, i32* %11, align 4
  %132 = add nsw i32 %131, 1
  store i32 %132, i32* %11, align 4
  br label %23, !llvm.loop !57

133:                                              ; preds = %23
  %134 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %135 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %134, align 4
  %136 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %135, i32 0, i32 137
  %137 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %136, align 4
  %138 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %139 = load %struct._jobject*, %struct._jobject** %5, align 4
  %140 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %142 = call float %137(%struct.JNINativeInterface_** noundef %138, %struct._jobject* noundef %139, %struct._jmethodID* noundef %140, %union.jvalue* noundef %141)
  store float %142, float* %12, align 4
  %143 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %143)
  %144 = load float, float* %12, align 4
  ret float %144
}

; Function Attrs: noinline nounwind optnone
define dso_local float @JNI_CallStaticFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !58

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 137
  %138 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %143 = call float %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret float %143
}

; Function Attrs: noinline nounwind optnone
define dso_local double @JNI_CallDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca double, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %13 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %13)
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 4
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %21 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %130, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %9, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %133

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  switch i32 %31, label %128 [
    i32 90, label %32
    i32 66, label %42
    i32 67, label %52
    i32 83, label %62
    i32 73, label %72
    i32 74, label %81
    i32 70, label %91
    i32 68, label %105
    i32 76, label %118
  ]

32:                                               ; preds = %27
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %129

42:                                               ; preds = %27
  %43 = bitcast %struct.__va_list* %7 to i8**
  %44 = load i8*, i8** %43, align 4
  %45 = getelementptr inbounds i8, i8* %44, i32 4
  store i8* %45, i8** %43, align 4
  %46 = bitcast i8* %44 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %11, align 4
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %48, i8* %51, align 8
  br label %129

52:                                               ; preds = %27
  %53 = bitcast %struct.__va_list* %7 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i16
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i16*
  store i16 %58, i16* %61, align 8
  br label %129

62:                                               ; preds = %27
  %63 = bitcast %struct.__va_list* %7 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load i32, i32* %11, align 4
  %70 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %69
  %71 = bitcast %union.jvalue* %70 to i16*
  store i16 %68, i16* %71, align 8
  br label %129

72:                                               ; preds = %27
  %73 = bitcast %struct.__va_list* %7 to i8**
  %74 = load i8*, i8** %73, align 4
  %75 = getelementptr inbounds i8, i8* %74, i32 4
  store i8* %75, i8** %73, align 4
  %76 = bitcast i8* %74 to i32*
  %77 = load i32, i32* %76, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %78
  %80 = bitcast %union.jvalue* %79 to i32*
  store i32 %77, i32* %80, align 8
  br label %129

81:                                               ; preds = %27
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = sext i32 %86 to i64
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i64*
  store i64 %87, i64* %90, align 8
  br label %129

91:                                               ; preds = %27
  %92 = bitcast %struct.__va_list* %7 to i8**
  %93 = load i8*, i8** %92, align 4
  %94 = ptrtoint i8* %93 to i32
  %95 = add i32 %94, 7
  %96 = and i32 %95, -8
  %97 = inttoptr i32 %96 to i8*
  %98 = getelementptr inbounds i8, i8* %97, i32 8
  store i8* %98, i8** %92, align 4
  %99 = bitcast i8* %97 to double*
  %100 = load double, double* %99, align 8
  %101 = fptrunc double %100 to float
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to float*
  store float %101, float* %104, align 8
  br label %129

105:                                              ; preds = %27
  %106 = bitcast %struct.__va_list* %7 to i8**
  %107 = load i8*, i8** %106, align 4
  %108 = ptrtoint i8* %107 to i32
  %109 = add i32 %108, 7
  %110 = and i32 %109, -8
  %111 = inttoptr i32 %110 to i8*
  %112 = getelementptr inbounds i8, i8* %111, i32 8
  store i8* %112, i8** %106, align 4
  %113 = bitcast i8* %111 to double*
  %114 = load double, double* %113, align 8
  %115 = load i32, i32* %11, align 4
  %116 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %114, double* %117, align 8
  br label %129

118:                                              ; preds = %27
  %119 = bitcast %struct.__va_list* %7 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = getelementptr inbounds i8, i8* %120, i32 4
  store i8* %121, i8** %119, align 4
  %122 = bitcast i8* %120 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = bitcast i8* %123 to %struct._jobject*
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %125
  %127 = bitcast %union.jvalue* %126 to %struct._jobject**
  store %struct._jobject* %124, %struct._jobject** %127, align 8
  br label %129

128:                                              ; preds = %27
  br label %129

129:                                              ; preds = %128, %118, %105, %91, %81, %72, %62, %52, %42, %32
  br label %130

130:                                              ; preds = %129
  %131 = load i32, i32* %11, align 4
  %132 = add nsw i32 %131, 1
  store i32 %132, i32* %11, align 4
  br label %23, !llvm.loop !59

133:                                              ; preds = %23
  %134 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %135 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %134, align 4
  %136 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %135, i32 0, i32 60
  %137 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %136, align 4
  %138 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %139 = load %struct._jobject*, %struct._jobject** %5, align 4
  %140 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %142 = call double %137(%struct.JNINativeInterface_** noundef %138, %struct._jobject* noundef %139, %struct._jmethodID* noundef %140, %union.jvalue* noundef %141)
  store double %142, double* %12, align 8
  %143 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %143)
  %144 = load double, double* %12, align 8
  ret double %144
}

; Function Attrs: noinline nounwind optnone
define dso_local double @JNI_CallDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !60

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 60
  %138 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %143 = call double %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret double %143
}

; Function Attrs: noinline nounwind optnone
define dso_local double @JNI_CallNonvirtualDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct.__va_list, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca double, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %1, %struct._jobject** %6, align 4
  store %struct._jobject* %2, %struct._jobject** %7, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 4
  %15 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_start(i8* %15)
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %17 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %17, i32 0, i32 0
  %19 = load i8*, i8** %18, align 4
  %20 = bitcast i8* %19 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %21 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %22 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %23 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 0
  %24 = call i32 %20(%struct.JNINativeInterface_** noundef %21, %struct._jmethodID* noundef %22, i8* noundef %23)
  store i32 %24, i32* %11, align 4
  store i32 0, i32* %13, align 4
  br label %25

25:                                               ; preds = %132, %4
  %26 = load i32, i32* %13, align 4
  %27 = load i32, i32* %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %135

29:                                               ; preds = %25
  %30 = load i32, i32* %13, align 4
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %30
  %32 = load i8, i8* %31, align 1
  %33 = zext i8 %32 to i32
  switch i32 %33, label %130 [
    i32 90, label %34
    i32 66, label %44
    i32 67, label %54
    i32 83, label %64
    i32 73, label %74
    i32 74, label %83
    i32 70, label %93
    i32 68, label %107
    i32 76, label %120
  ]

34:                                               ; preds = %29
  %35 = bitcast %struct.__va_list* %9 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %13, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %131

44:                                               ; preds = %29
  %45 = bitcast %struct.__va_list* %9 to i8**
  %46 = load i8*, i8** %45, align 4
  %47 = getelementptr inbounds i8, i8* %46, i32 4
  store i8* %47, i8** %45, align 4
  %48 = bitcast i8* %46 to i32*
  %49 = load i32, i32* %48, align 4
  %50 = trunc i32 %49 to i8
  %51 = load i32, i32* %13, align 4
  %52 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %51
  %53 = bitcast %union.jvalue* %52 to i8*
  store i8 %50, i8* %53, align 8
  br label %131

54:                                               ; preds = %29
  %55 = bitcast %struct.__va_list* %9 to i8**
  %56 = load i8*, i8** %55, align 4
  %57 = getelementptr inbounds i8, i8* %56, i32 4
  store i8* %57, i8** %55, align 4
  %58 = bitcast i8* %56 to i32*
  %59 = load i32, i32* %58, align 4
  %60 = trunc i32 %59 to i16
  %61 = load i32, i32* %13, align 4
  %62 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %61
  %63 = bitcast %union.jvalue* %62 to i16*
  store i16 %60, i16* %63, align 8
  br label %131

64:                                               ; preds = %29
  %65 = bitcast %struct.__va_list* %9 to i8**
  %66 = load i8*, i8** %65, align 4
  %67 = getelementptr inbounds i8, i8* %66, i32 4
  store i8* %67, i8** %65, align 4
  %68 = bitcast i8* %66 to i32*
  %69 = load i32, i32* %68, align 4
  %70 = trunc i32 %69 to i16
  %71 = load i32, i32* %13, align 4
  %72 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %71
  %73 = bitcast %union.jvalue* %72 to i16*
  store i16 %70, i16* %73, align 8
  br label %131

74:                                               ; preds = %29
  %75 = bitcast %struct.__va_list* %9 to i8**
  %76 = load i8*, i8** %75, align 4
  %77 = getelementptr inbounds i8, i8* %76, i32 4
  store i8* %77, i8** %75, align 4
  %78 = bitcast i8* %76 to i32*
  %79 = load i32, i32* %78, align 4
  %80 = load i32, i32* %13, align 4
  %81 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %80
  %82 = bitcast %union.jvalue* %81 to i32*
  store i32 %79, i32* %82, align 8
  br label %131

83:                                               ; preds = %29
  %84 = bitcast %struct.__va_list* %9 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = sext i32 %88 to i64
  %90 = load i32, i32* %13, align 4
  %91 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %90
  %92 = bitcast %union.jvalue* %91 to i64*
  store i64 %89, i64* %92, align 8
  br label %131

93:                                               ; preds = %29
  %94 = bitcast %struct.__va_list* %9 to i8**
  %95 = load i8*, i8** %94, align 4
  %96 = ptrtoint i8* %95 to i32
  %97 = add i32 %96, 7
  %98 = and i32 %97, -8
  %99 = inttoptr i32 %98 to i8*
  %100 = getelementptr inbounds i8, i8* %99, i32 8
  store i8* %100, i8** %94, align 4
  %101 = bitcast i8* %99 to double*
  %102 = load double, double* %101, align 8
  %103 = fptrunc double %102 to float
  %104 = load i32, i32* %13, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to float*
  store float %103, float* %106, align 8
  br label %131

107:                                              ; preds = %29
  %108 = bitcast %struct.__va_list* %9 to i8**
  %109 = load i8*, i8** %108, align 4
  %110 = ptrtoint i8* %109 to i32
  %111 = add i32 %110, 7
  %112 = and i32 %111, -8
  %113 = inttoptr i32 %112 to i8*
  %114 = getelementptr inbounds i8, i8* %113, i32 8
  store i8* %114, i8** %108, align 4
  %115 = bitcast i8* %113 to double*
  %116 = load double, double* %115, align 8
  %117 = load i32, i32* %13, align 4
  %118 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %117
  %119 = bitcast %union.jvalue* %118 to double*
  store double %116, double* %119, align 8
  br label %131

120:                                              ; preds = %29
  %121 = bitcast %struct.__va_list* %9 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = getelementptr inbounds i8, i8* %122, i32 4
  store i8* %123, i8** %121, align 4
  %124 = bitcast i8* %122 to i8**
  %125 = load i8*, i8** %124, align 4
  %126 = bitcast i8* %125 to %struct._jobject*
  %127 = load i32, i32* %13, align 4
  %128 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %127
  %129 = bitcast %union.jvalue* %128 to %struct._jobject**
  store %struct._jobject* %126, %struct._jobject** %129, align 8
  br label %131

130:                                              ; preds = %29
  br label %131

131:                                              ; preds = %130, %120, %107, %93, %83, %74, %64, %54, %44, %34
  br label %132

132:                                              ; preds = %131
  %133 = load i32, i32* %13, align 4
  %134 = add nsw i32 %133, 1
  store i32 %134, i32* %13, align 4
  br label %25, !llvm.loop !61

135:                                              ; preds = %25
  %136 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %137 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %136, align 4
  %138 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %137, i32 0, i32 90
  %139 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %138, align 4
  %140 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %141 = load %struct._jobject*, %struct._jobject** %6, align 4
  %142 = load %struct._jobject*, %struct._jobject** %7, align 4
  %143 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %144 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 0
  %145 = call double %139(%struct.JNINativeInterface_** noundef %140, %struct._jobject* noundef %141, %struct._jobject* noundef %142, %struct._jmethodID* noundef %143, %union.jvalue* noundef %144)
  store double %145, double* %14, align 8
  %146 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %146)
  %147 = load double, double* %14, align 8
  ret double %147
}

; Function Attrs: noinline nounwind optnone
define dso_local double @JNI_CallNonvirtualDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  %15 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %6, i32 0, i32 0
  %16 = bitcast i8** %15 to [1 x i32]*
  store [1 x i32] %4, [1 x i32]* %16, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %7, align 4
  store %struct._jobject* %1, %struct._jobject** %8, align 4
  store %struct._jobject* %2, %struct._jobject** %9, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %10, align 4
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %18 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %17, align 4
  %19 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %18, i32 0, i32 0
  %20 = load i8*, i8** %19, align 4
  %21 = bitcast i8* %20 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %23 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %24 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 0
  %25 = call i32 %21(%struct.JNINativeInterface_** noundef %22, %struct._jmethodID* noundef %23, i8* noundef %24)
  store i32 %25, i32* %12, align 4
  store i32 0, i32* %14, align 4
  br label %26

26:                                               ; preds = %133, %5
  %27 = load i32, i32* %14, align 4
  %28 = load i32, i32* %12, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %136

30:                                               ; preds = %26
  %31 = load i32, i32* %14, align 4
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %31
  %33 = load i8, i8* %32, align 1
  %34 = zext i8 %33 to i32
  switch i32 %34, label %131 [
    i32 90, label %35
    i32 66, label %45
    i32 67, label %55
    i32 83, label %65
    i32 73, label %75
    i32 74, label %84
    i32 70, label %94
    i32 68, label %108
    i32 76, label %121
  ]

35:                                               ; preds = %30
  %36 = bitcast %struct.__va_list* %6 to i8**
  %37 = load i8*, i8** %36, align 4
  %38 = getelementptr inbounds i8, i8* %37, i32 4
  store i8* %38, i8** %36, align 4
  %39 = bitcast i8* %37 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = load i32, i32* %14, align 4
  %43 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %42
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %41, i8* %44, align 8
  br label %132

45:                                               ; preds = %30
  %46 = bitcast %struct.__va_list* %6 to i8**
  %47 = load i8*, i8** %46, align 4
  %48 = getelementptr inbounds i8, i8* %47, i32 4
  store i8* %48, i8** %46, align 4
  %49 = bitcast i8* %47 to i32*
  %50 = load i32, i32* %49, align 4
  %51 = trunc i32 %50 to i8
  %52 = load i32, i32* %14, align 4
  %53 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %52
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %51, i8* %54, align 8
  br label %132

55:                                               ; preds = %30
  %56 = bitcast %struct.__va_list* %6 to i8**
  %57 = load i8*, i8** %56, align 4
  %58 = getelementptr inbounds i8, i8* %57, i32 4
  store i8* %58, i8** %56, align 4
  %59 = bitcast i8* %57 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = trunc i32 %60 to i16
  %62 = load i32, i32* %14, align 4
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %62
  %64 = bitcast %union.jvalue* %63 to i16*
  store i16 %61, i16* %64, align 8
  br label %132

65:                                               ; preds = %30
  %66 = bitcast %struct.__va_list* %6 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %14, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %132

75:                                               ; preds = %30
  %76 = bitcast %struct.__va_list* %6 to i8**
  %77 = load i8*, i8** %76, align 4
  %78 = getelementptr inbounds i8, i8* %77, i32 4
  store i8* %78, i8** %76, align 4
  %79 = bitcast i8* %77 to i32*
  %80 = load i32, i32* %79, align 4
  %81 = load i32, i32* %14, align 4
  %82 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %81
  %83 = bitcast %union.jvalue* %82 to i32*
  store i32 %80, i32* %83, align 8
  br label %132

84:                                               ; preds = %30
  %85 = bitcast %struct.__va_list* %6 to i8**
  %86 = load i8*, i8** %85, align 4
  %87 = getelementptr inbounds i8, i8* %86, i32 4
  store i8* %87, i8** %85, align 4
  %88 = bitcast i8* %86 to i32*
  %89 = load i32, i32* %88, align 4
  %90 = sext i32 %89 to i64
  %91 = load i32, i32* %14, align 4
  %92 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %91
  %93 = bitcast %union.jvalue* %92 to i64*
  store i64 %90, i64* %93, align 8
  br label %132

94:                                               ; preds = %30
  %95 = bitcast %struct.__va_list* %6 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = ptrtoint i8* %96 to i32
  %98 = add i32 %97, 7
  %99 = and i32 %98, -8
  %100 = inttoptr i32 %99 to i8*
  %101 = getelementptr inbounds i8, i8* %100, i32 8
  store i8* %101, i8** %95, align 4
  %102 = bitcast i8* %100 to double*
  %103 = load double, double* %102, align 8
  %104 = fptrunc double %103 to float
  %105 = load i32, i32* %14, align 4
  %106 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %105
  %107 = bitcast %union.jvalue* %106 to float*
  store float %104, float* %107, align 8
  br label %132

108:                                              ; preds = %30
  %109 = bitcast %struct.__va_list* %6 to i8**
  %110 = load i8*, i8** %109, align 4
  %111 = ptrtoint i8* %110 to i32
  %112 = add i32 %111, 7
  %113 = and i32 %112, -8
  %114 = inttoptr i32 %113 to i8*
  %115 = getelementptr inbounds i8, i8* %114, i32 8
  store i8* %115, i8** %109, align 4
  %116 = bitcast i8* %114 to double*
  %117 = load double, double* %116, align 8
  %118 = load i32, i32* %14, align 4
  %119 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %118
  %120 = bitcast %union.jvalue* %119 to double*
  store double %117, double* %120, align 8
  br label %132

121:                                              ; preds = %30
  %122 = bitcast %struct.__va_list* %6 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = getelementptr inbounds i8, i8* %123, i32 4
  store i8* %124, i8** %122, align 4
  %125 = bitcast i8* %123 to i8**
  %126 = load i8*, i8** %125, align 4
  %127 = bitcast i8* %126 to %struct._jobject*
  %128 = load i32, i32* %14, align 4
  %129 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %128
  %130 = bitcast %union.jvalue* %129 to %struct._jobject**
  store %struct._jobject* %127, %struct._jobject** %130, align 8
  br label %132

131:                                              ; preds = %30
  br label %132

132:                                              ; preds = %131, %121, %108, %94, %84, %75, %65, %55, %45, %35
  br label %133

133:                                              ; preds = %132
  %134 = load i32, i32* %14, align 4
  %135 = add nsw i32 %134, 1
  store i32 %135, i32* %14, align 4
  br label %26, !llvm.loop !62

136:                                              ; preds = %26
  %137 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %138 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %137, align 4
  %139 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %138, i32 0, i32 90
  %140 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %139, align 4
  %141 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %142 = load %struct._jobject*, %struct._jobject** %8, align 4
  %143 = load %struct._jobject*, %struct._jobject** %9, align 4
  %144 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %145 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 0
  %146 = call double %140(%struct.JNINativeInterface_** noundef %141, %struct._jobject* noundef %142, %struct._jobject* noundef %143, %struct._jmethodID* noundef %144, %union.jvalue* noundef %145)
  ret double %146
}

; Function Attrs: noinline nounwind optnone
define dso_local double @JNI_CallStaticDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca double, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %13 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %13)
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 4
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %21 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %130, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %9, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %133

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  switch i32 %31, label %128 [
    i32 90, label %32
    i32 66, label %42
    i32 67, label %52
    i32 83, label %62
    i32 73, label %72
    i32 74, label %81
    i32 70, label %91
    i32 68, label %105
    i32 76, label %118
  ]

32:                                               ; preds = %27
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %129

42:                                               ; preds = %27
  %43 = bitcast %struct.__va_list* %7 to i8**
  %44 = load i8*, i8** %43, align 4
  %45 = getelementptr inbounds i8, i8* %44, i32 4
  store i8* %45, i8** %43, align 4
  %46 = bitcast i8* %44 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %11, align 4
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %48, i8* %51, align 8
  br label %129

52:                                               ; preds = %27
  %53 = bitcast %struct.__va_list* %7 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i16
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i16*
  store i16 %58, i16* %61, align 8
  br label %129

62:                                               ; preds = %27
  %63 = bitcast %struct.__va_list* %7 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load i32, i32* %11, align 4
  %70 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %69
  %71 = bitcast %union.jvalue* %70 to i16*
  store i16 %68, i16* %71, align 8
  br label %129

72:                                               ; preds = %27
  %73 = bitcast %struct.__va_list* %7 to i8**
  %74 = load i8*, i8** %73, align 4
  %75 = getelementptr inbounds i8, i8* %74, i32 4
  store i8* %75, i8** %73, align 4
  %76 = bitcast i8* %74 to i32*
  %77 = load i32, i32* %76, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %78
  %80 = bitcast %union.jvalue* %79 to i32*
  store i32 %77, i32* %80, align 8
  br label %129

81:                                               ; preds = %27
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = sext i32 %86 to i64
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i64*
  store i64 %87, i64* %90, align 8
  br label %129

91:                                               ; preds = %27
  %92 = bitcast %struct.__va_list* %7 to i8**
  %93 = load i8*, i8** %92, align 4
  %94 = ptrtoint i8* %93 to i32
  %95 = add i32 %94, 7
  %96 = and i32 %95, -8
  %97 = inttoptr i32 %96 to i8*
  %98 = getelementptr inbounds i8, i8* %97, i32 8
  store i8* %98, i8** %92, align 4
  %99 = bitcast i8* %97 to double*
  %100 = load double, double* %99, align 8
  %101 = fptrunc double %100 to float
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to float*
  store float %101, float* %104, align 8
  br label %129

105:                                              ; preds = %27
  %106 = bitcast %struct.__va_list* %7 to i8**
  %107 = load i8*, i8** %106, align 4
  %108 = ptrtoint i8* %107 to i32
  %109 = add i32 %108, 7
  %110 = and i32 %109, -8
  %111 = inttoptr i32 %110 to i8*
  %112 = getelementptr inbounds i8, i8* %111, i32 8
  store i8* %112, i8** %106, align 4
  %113 = bitcast i8* %111 to double*
  %114 = load double, double* %113, align 8
  %115 = load i32, i32* %11, align 4
  %116 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %114, double* %117, align 8
  br label %129

118:                                              ; preds = %27
  %119 = bitcast %struct.__va_list* %7 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = getelementptr inbounds i8, i8* %120, i32 4
  store i8* %121, i8** %119, align 4
  %122 = bitcast i8* %120 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = bitcast i8* %123 to %struct._jobject*
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %125
  %127 = bitcast %union.jvalue* %126 to %struct._jobject**
  store %struct._jobject* %124, %struct._jobject** %127, align 8
  br label %129

128:                                              ; preds = %27
  br label %129

129:                                              ; preds = %128, %118, %105, %91, %81, %72, %62, %52, %42, %32
  br label %130

130:                                              ; preds = %129
  %131 = load i32, i32* %11, align 4
  %132 = add nsw i32 %131, 1
  store i32 %132, i32* %11, align 4
  br label %23, !llvm.loop !63

133:                                              ; preds = %23
  %134 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %135 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %134, align 4
  %136 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %135, i32 0, i32 140
  %137 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %136, align 4
  %138 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %139 = load %struct._jobject*, %struct._jobject** %5, align 4
  %140 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %142 = call double %137(%struct.JNINativeInterface_** noundef %138, %struct._jobject* noundef %139, %struct._jmethodID* noundef %140, %union.jvalue* noundef %141)
  store double %142, double* %12, align 8
  %143 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %143)
  %144 = load double, double* %12, align 8
  ret double %144
}

; Function Attrs: noinline nounwind optnone
define dso_local double @JNI_CallStaticDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !64

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 140
  %138 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %143 = call double %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret double %143
}

; Function Attrs: noinline nounwind optnone
define dso_local %struct._jobject* @JNI_NewObject(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca %struct._jobject*, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %13 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %13)
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %15, i32 0, i32 0
  %17 = load i8*, i8** %16, align 4
  %18 = bitcast i8* %17 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %19 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %20 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %21 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %22 = call i32 %18(%struct.JNINativeInterface_** noundef %19, %struct._jmethodID* noundef %20, i8* noundef %21)
  store i32 %22, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %130, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %9, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %133

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  switch i32 %31, label %128 [
    i32 90, label %32
    i32 66, label %42
    i32 67, label %52
    i32 83, label %62
    i32 73, label %72
    i32 74, label %81
    i32 70, label %91
    i32 68, label %105
    i32 76, label %118
  ]

32:                                               ; preds = %27
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %129

42:                                               ; preds = %27
  %43 = bitcast %struct.__va_list* %7 to i8**
  %44 = load i8*, i8** %43, align 4
  %45 = getelementptr inbounds i8, i8* %44, i32 4
  store i8* %45, i8** %43, align 4
  %46 = bitcast i8* %44 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %11, align 4
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %48, i8* %51, align 8
  br label %129

52:                                               ; preds = %27
  %53 = bitcast %struct.__va_list* %7 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i16
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i16*
  store i16 %58, i16* %61, align 8
  br label %129

62:                                               ; preds = %27
  %63 = bitcast %struct.__va_list* %7 to i8**
  %64 = load i8*, i8** %63, align 4
  %65 = getelementptr inbounds i8, i8* %64, i32 4
  store i8* %65, i8** %63, align 4
  %66 = bitcast i8* %64 to i32*
  %67 = load i32, i32* %66, align 4
  %68 = trunc i32 %67 to i16
  %69 = load i32, i32* %11, align 4
  %70 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %69
  %71 = bitcast %union.jvalue* %70 to i16*
  store i16 %68, i16* %71, align 8
  br label %129

72:                                               ; preds = %27
  %73 = bitcast %struct.__va_list* %7 to i8**
  %74 = load i8*, i8** %73, align 4
  %75 = getelementptr inbounds i8, i8* %74, i32 4
  store i8* %75, i8** %73, align 4
  %76 = bitcast i8* %74 to i32*
  %77 = load i32, i32* %76, align 4
  %78 = load i32, i32* %11, align 4
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %78
  %80 = bitcast %union.jvalue* %79 to i32*
  store i32 %77, i32* %80, align 8
  br label %129

81:                                               ; preds = %27
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = sext i32 %86 to i64
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i64*
  store i64 %87, i64* %90, align 8
  br label %129

91:                                               ; preds = %27
  %92 = bitcast %struct.__va_list* %7 to i8**
  %93 = load i8*, i8** %92, align 4
  %94 = ptrtoint i8* %93 to i32
  %95 = add i32 %94, 7
  %96 = and i32 %95, -8
  %97 = inttoptr i32 %96 to i8*
  %98 = getelementptr inbounds i8, i8* %97, i32 8
  store i8* %98, i8** %92, align 4
  %99 = bitcast i8* %97 to double*
  %100 = load double, double* %99, align 8
  %101 = fptrunc double %100 to float
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to float*
  store float %101, float* %104, align 8
  br label %129

105:                                              ; preds = %27
  %106 = bitcast %struct.__va_list* %7 to i8**
  %107 = load i8*, i8** %106, align 4
  %108 = ptrtoint i8* %107 to i32
  %109 = add i32 %108, 7
  %110 = and i32 %109, -8
  %111 = inttoptr i32 %110 to i8*
  %112 = getelementptr inbounds i8, i8* %111, i32 8
  store i8* %112, i8** %106, align 4
  %113 = bitcast i8* %111 to double*
  %114 = load double, double* %113, align 8
  %115 = load i32, i32* %11, align 4
  %116 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %115
  %117 = bitcast %union.jvalue* %116 to double*
  store double %114, double* %117, align 8
  br label %129

118:                                              ; preds = %27
  %119 = bitcast %struct.__va_list* %7 to i8**
  %120 = load i8*, i8** %119, align 4
  %121 = getelementptr inbounds i8, i8* %120, i32 4
  store i8* %121, i8** %119, align 4
  %122 = bitcast i8* %120 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = bitcast i8* %123 to %struct._jobject*
  %125 = load i32, i32* %11, align 4
  %126 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %125
  %127 = bitcast %union.jvalue* %126 to %struct._jobject**
  store %struct._jobject* %124, %struct._jobject** %127, align 8
  br label %129

128:                                              ; preds = %27
  br label %129

129:                                              ; preds = %128, %118, %105, %91, %81, %72, %62, %52, %42, %32
  br label %130

130:                                              ; preds = %129
  %131 = load i32, i32* %11, align 4
  %132 = add nsw i32 %131, 1
  store i32 %132, i32* %11, align 4
  br label %23, !llvm.loop !65

133:                                              ; preds = %23
  %134 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %135 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %134, align 4
  %136 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %135, i32 0, i32 30
  %137 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %136, align 4
  %138 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %139 = load %struct._jobject*, %struct._jobject** %5, align 4
  %140 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %142 = call %struct._jobject* %137(%struct.JNINativeInterface_** noundef %138, %struct._jobject* noundef %139, %struct._jmethodID* noundef %140, %union.jvalue* noundef %141)
  store %struct._jobject* %142, %struct._jobject** %12, align 4
  %143 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %143)
  %144 = load %struct._jobject*, %struct._jobject** %12, align 4
  ret %struct._jobject* %144
}

; Function Attrs: noinline nounwind optnone
define dso_local %struct._jobject* @JNI_NewObjectV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !66

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 30
  %138 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %143 = call %struct._jobject* %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret %struct._jobject* %143
}

; Function Attrs: noinline nounwind optnone
define dso_local void @JNI_CallVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %12 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %12)
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 4
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 4
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %19 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %20 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %22

22:                                               ; preds = %129, %3
  %23 = load i32, i32* %11, align 4
  %24 = load i32, i32* %9, align 4
  %25 = icmp slt i32 %23, %24
  br i1 %25, label %26, label %132

26:                                               ; preds = %22
  %27 = load i32, i32* %11, align 4
  %28 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %27
  %29 = load i8, i8* %28, align 1
  %30 = zext i8 %29 to i32
  switch i32 %30, label %127 [
    i32 90, label %31
    i32 66, label %41
    i32 67, label %51
    i32 83, label %61
    i32 73, label %71
    i32 74, label %80
    i32 70, label %90
    i32 68, label %104
    i32 76, label %117
  ]

31:                                               ; preds = %26
  %32 = bitcast %struct.__va_list* %7 to i8**
  %33 = load i8*, i8** %32, align 4
  %34 = getelementptr inbounds i8, i8* %33, i32 4
  store i8* %34, i8** %32, align 4
  %35 = bitcast i8* %33 to i32*
  %36 = load i32, i32* %35, align 4
  %37 = trunc i32 %36 to i8
  %38 = load i32, i32* %11, align 4
  %39 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %38
  %40 = bitcast %union.jvalue* %39 to i8*
  store i8 %37, i8* %40, align 8
  br label %128

41:                                               ; preds = %26
  %42 = bitcast %struct.__va_list* %7 to i8**
  %43 = load i8*, i8** %42, align 4
  %44 = getelementptr inbounds i8, i8* %43, i32 4
  store i8* %44, i8** %42, align 4
  %45 = bitcast i8* %43 to i32*
  %46 = load i32, i32* %45, align 4
  %47 = trunc i32 %46 to i8
  %48 = load i32, i32* %11, align 4
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %47, i8* %50, align 8
  br label %128

51:                                               ; preds = %26
  %52 = bitcast %struct.__va_list* %7 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i16
  %58 = load i32, i32* %11, align 4
  %59 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %58
  %60 = bitcast %union.jvalue* %59 to i16*
  store i16 %57, i16* %60, align 8
  br label %128

61:                                               ; preds = %26
  %62 = bitcast %struct.__va_list* %7 to i8**
  %63 = load i8*, i8** %62, align 4
  %64 = getelementptr inbounds i8, i8* %63, i32 4
  store i8* %64, i8** %62, align 4
  %65 = bitcast i8* %63 to i32*
  %66 = load i32, i32* %65, align 4
  %67 = trunc i32 %66 to i16
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %67, i16* %70, align 8
  br label %128

71:                                               ; preds = %26
  %72 = bitcast %struct.__va_list* %7 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = load i32, i32* %11, align 4
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %77
  %79 = bitcast %union.jvalue* %78 to i32*
  store i32 %76, i32* %79, align 8
  br label %128

80:                                               ; preds = %26
  %81 = bitcast %struct.__va_list* %7 to i8**
  %82 = load i8*, i8** %81, align 4
  %83 = getelementptr inbounds i8, i8* %82, i32 4
  store i8* %83, i8** %81, align 4
  %84 = bitcast i8* %82 to i32*
  %85 = load i32, i32* %84, align 4
  %86 = sext i32 %85 to i64
  %87 = load i32, i32* %11, align 4
  %88 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %87
  %89 = bitcast %union.jvalue* %88 to i64*
  store i64 %86, i64* %89, align 8
  br label %128

90:                                               ; preds = %26
  %91 = bitcast %struct.__va_list* %7 to i8**
  %92 = load i8*, i8** %91, align 4
  %93 = ptrtoint i8* %92 to i32
  %94 = add i32 %93, 7
  %95 = and i32 %94, -8
  %96 = inttoptr i32 %95 to i8*
  %97 = getelementptr inbounds i8, i8* %96, i32 8
  store i8* %97, i8** %91, align 4
  %98 = bitcast i8* %96 to double*
  %99 = load double, double* %98, align 8
  %100 = fptrunc double %99 to float
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %101
  %103 = bitcast %union.jvalue* %102 to float*
  store float %100, float* %103, align 8
  br label %128

104:                                              ; preds = %26
  %105 = bitcast %struct.__va_list* %7 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load i32, i32* %11, align 4
  %115 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %114
  %116 = bitcast %union.jvalue* %115 to double*
  store double %113, double* %116, align 8
  br label %128

117:                                              ; preds = %26
  %118 = bitcast %struct.__va_list* %7 to i8**
  %119 = load i8*, i8** %118, align 4
  %120 = getelementptr inbounds i8, i8* %119, i32 4
  store i8* %120, i8** %118, align 4
  %121 = bitcast i8* %119 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = bitcast i8* %122 to %struct._jobject*
  %124 = load i32, i32* %11, align 4
  %125 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %124
  %126 = bitcast %union.jvalue* %125 to %struct._jobject**
  store %struct._jobject* %123, %struct._jobject** %126, align 8
  br label %128

127:                                              ; preds = %26
  br label %128

128:                                              ; preds = %127, %117, %104, %90, %80, %71, %61, %51, %41, %31
  br label %129

129:                                              ; preds = %128
  %130 = load i32, i32* %11, align 4
  %131 = add nsw i32 %130, 1
  store i32 %131, i32* %11, align 4
  br label %22, !llvm.loop !67

132:                                              ; preds = %22
  %133 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %134 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %133, align 4
  %135 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %134, i32 0, i32 63
  %136 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %135, align 4
  %137 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %138 = load %struct._jobject*, %struct._jobject** %5, align 4
  %139 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  call void %136(%struct.JNINativeInterface_** noundef %137, %struct._jobject* noundef %138, %struct._jmethodID* noundef %139, %union.jvalue* noundef %140)
  %141 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %141)
  ret void
}

; Function Attrs: noinline nounwind optnone
define dso_local void @JNI_CallVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !68

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 63
  %138 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  call void %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret void
}

; Function Attrs: noinline nounwind optnone
define dso_local void @JNI_CallNonvirtualVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct.__va_list, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %1, %struct._jobject** %6, align 4
  store %struct._jobject* %2, %struct._jobject** %7, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 4
  %14 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_start(i8* %14)
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %13, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %13, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %13, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %9 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %13, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %9 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %13, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %9 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %13, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %9 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %13, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %9 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %13, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %9 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %13, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %9 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %13, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %9 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %13, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %9 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %13, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %13, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %13, align 4
  br label %24, !llvm.loop !69

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 93
  %138 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %140 = load %struct._jobject*, %struct._jobject** %6, align 4
  %141 = load %struct._jobject*, %struct._jobject** %7, align 4
  %142 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %143 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 0
  call void %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jobject* noundef %141, %struct._jmethodID* noundef %142, %union.jvalue* noundef %143)
  %144 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %144)
  ret void
}

; Function Attrs: noinline nounwind optnone
define dso_local void @JNI_CallNonvirtualVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  %15 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %6, i32 0, i32 0
  %16 = bitcast i8** %15 to [1 x i32]*
  store [1 x i32] %4, [1 x i32]* %16, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %7, align 4
  store %struct._jobject* %1, %struct._jobject** %8, align 4
  store %struct._jobject* %2, %struct._jobject** %9, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %10, align 4
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %18 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %17, align 4
  %19 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %18, i32 0, i32 0
  %20 = load i8*, i8** %19, align 4
  %21 = bitcast i8* %20 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %23 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %24 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 0
  %25 = call i32 %21(%struct.JNINativeInterface_** noundef %22, %struct._jmethodID* noundef %23, i8* noundef %24)
  store i32 %25, i32* %12, align 4
  store i32 0, i32* %14, align 4
  br label %26

26:                                               ; preds = %133, %5
  %27 = load i32, i32* %14, align 4
  %28 = load i32, i32* %12, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %136

30:                                               ; preds = %26
  %31 = load i32, i32* %14, align 4
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %31
  %33 = load i8, i8* %32, align 1
  %34 = zext i8 %33 to i32
  switch i32 %34, label %131 [
    i32 90, label %35
    i32 66, label %45
    i32 67, label %55
    i32 83, label %65
    i32 73, label %75
    i32 74, label %84
    i32 70, label %94
    i32 68, label %108
    i32 76, label %121
  ]

35:                                               ; preds = %30
  %36 = bitcast %struct.__va_list* %6 to i8**
  %37 = load i8*, i8** %36, align 4
  %38 = getelementptr inbounds i8, i8* %37, i32 4
  store i8* %38, i8** %36, align 4
  %39 = bitcast i8* %37 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = load i32, i32* %14, align 4
  %43 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %42
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %41, i8* %44, align 8
  br label %132

45:                                               ; preds = %30
  %46 = bitcast %struct.__va_list* %6 to i8**
  %47 = load i8*, i8** %46, align 4
  %48 = getelementptr inbounds i8, i8* %47, i32 4
  store i8* %48, i8** %46, align 4
  %49 = bitcast i8* %47 to i32*
  %50 = load i32, i32* %49, align 4
  %51 = trunc i32 %50 to i8
  %52 = load i32, i32* %14, align 4
  %53 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %52
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %51, i8* %54, align 8
  br label %132

55:                                               ; preds = %30
  %56 = bitcast %struct.__va_list* %6 to i8**
  %57 = load i8*, i8** %56, align 4
  %58 = getelementptr inbounds i8, i8* %57, i32 4
  store i8* %58, i8** %56, align 4
  %59 = bitcast i8* %57 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = trunc i32 %60 to i16
  %62 = load i32, i32* %14, align 4
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %62
  %64 = bitcast %union.jvalue* %63 to i16*
  store i16 %61, i16* %64, align 8
  br label %132

65:                                               ; preds = %30
  %66 = bitcast %struct.__va_list* %6 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %14, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %132

75:                                               ; preds = %30
  %76 = bitcast %struct.__va_list* %6 to i8**
  %77 = load i8*, i8** %76, align 4
  %78 = getelementptr inbounds i8, i8* %77, i32 4
  store i8* %78, i8** %76, align 4
  %79 = bitcast i8* %77 to i32*
  %80 = load i32, i32* %79, align 4
  %81 = load i32, i32* %14, align 4
  %82 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %81
  %83 = bitcast %union.jvalue* %82 to i32*
  store i32 %80, i32* %83, align 8
  br label %132

84:                                               ; preds = %30
  %85 = bitcast %struct.__va_list* %6 to i8**
  %86 = load i8*, i8** %85, align 4
  %87 = getelementptr inbounds i8, i8* %86, i32 4
  store i8* %87, i8** %85, align 4
  %88 = bitcast i8* %86 to i32*
  %89 = load i32, i32* %88, align 4
  %90 = sext i32 %89 to i64
  %91 = load i32, i32* %14, align 4
  %92 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %91
  %93 = bitcast %union.jvalue* %92 to i64*
  store i64 %90, i64* %93, align 8
  br label %132

94:                                               ; preds = %30
  %95 = bitcast %struct.__va_list* %6 to i8**
  %96 = load i8*, i8** %95, align 4
  %97 = ptrtoint i8* %96 to i32
  %98 = add i32 %97, 7
  %99 = and i32 %98, -8
  %100 = inttoptr i32 %99 to i8*
  %101 = getelementptr inbounds i8, i8* %100, i32 8
  store i8* %101, i8** %95, align 4
  %102 = bitcast i8* %100 to double*
  %103 = load double, double* %102, align 8
  %104 = fptrunc double %103 to float
  %105 = load i32, i32* %14, align 4
  %106 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %105
  %107 = bitcast %union.jvalue* %106 to float*
  store float %104, float* %107, align 8
  br label %132

108:                                              ; preds = %30
  %109 = bitcast %struct.__va_list* %6 to i8**
  %110 = load i8*, i8** %109, align 4
  %111 = ptrtoint i8* %110 to i32
  %112 = add i32 %111, 7
  %113 = and i32 %112, -8
  %114 = inttoptr i32 %113 to i8*
  %115 = getelementptr inbounds i8, i8* %114, i32 8
  store i8* %115, i8** %109, align 4
  %116 = bitcast i8* %114 to double*
  %117 = load double, double* %116, align 8
  %118 = load i32, i32* %14, align 4
  %119 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %118
  %120 = bitcast %union.jvalue* %119 to double*
  store double %117, double* %120, align 8
  br label %132

121:                                              ; preds = %30
  %122 = bitcast %struct.__va_list* %6 to i8**
  %123 = load i8*, i8** %122, align 4
  %124 = getelementptr inbounds i8, i8* %123, i32 4
  store i8* %124, i8** %122, align 4
  %125 = bitcast i8* %123 to i8**
  %126 = load i8*, i8** %125, align 4
  %127 = bitcast i8* %126 to %struct._jobject*
  %128 = load i32, i32* %14, align 4
  %129 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 %128
  %130 = bitcast %union.jvalue* %129 to %struct._jobject**
  store %struct._jobject* %127, %struct._jobject** %130, align 8
  br label %132

131:                                              ; preds = %30
  br label %132

132:                                              ; preds = %131, %121, %108, %94, %84, %75, %65, %55, %45, %35
  br label %133

133:                                              ; preds = %132
  %134 = load i32, i32* %14, align 4
  %135 = add nsw i32 %134, 1
  store i32 %135, i32* %14, align 4
  br label %26, !llvm.loop !70

136:                                              ; preds = %26
  %137 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %138 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %137, align 4
  %139 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %138, i32 0, i32 93
  %140 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %139, align 4
  %141 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %142 = load %struct._jobject*, %struct._jobject** %8, align 4
  %143 = load %struct._jobject*, %struct._jobject** %9, align 4
  %144 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %145 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %13, i32 0, i32 0
  call void %140(%struct.JNINativeInterface_** noundef %141, %struct._jobject* noundef %142, %struct._jobject* noundef %143, %struct._jmethodID* noundef %144, %union.jvalue* noundef %145)
  ret void
}

; Function Attrs: noinline nounwind optnone
define dso_local void @JNI_CallStaticVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %12 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %12)
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 4
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 4
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %19 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %20 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %9, align 4
  store i32 0, i32* %11, align 4
  br label %22

22:                                               ; preds = %129, %3
  %23 = load i32, i32* %11, align 4
  %24 = load i32, i32* %9, align 4
  %25 = icmp slt i32 %23, %24
  br i1 %25, label %26, label %132

26:                                               ; preds = %22
  %27 = load i32, i32* %11, align 4
  %28 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %27
  %29 = load i8, i8* %28, align 1
  %30 = zext i8 %29 to i32
  switch i32 %30, label %127 [
    i32 90, label %31
    i32 66, label %41
    i32 67, label %51
    i32 83, label %61
    i32 73, label %71
    i32 74, label %80
    i32 70, label %90
    i32 68, label %104
    i32 76, label %117
  ]

31:                                               ; preds = %26
  %32 = bitcast %struct.__va_list* %7 to i8**
  %33 = load i8*, i8** %32, align 4
  %34 = getelementptr inbounds i8, i8* %33, i32 4
  store i8* %34, i8** %32, align 4
  %35 = bitcast i8* %33 to i32*
  %36 = load i32, i32* %35, align 4
  %37 = trunc i32 %36 to i8
  %38 = load i32, i32* %11, align 4
  %39 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %38
  %40 = bitcast %union.jvalue* %39 to i8*
  store i8 %37, i8* %40, align 8
  br label %128

41:                                               ; preds = %26
  %42 = bitcast %struct.__va_list* %7 to i8**
  %43 = load i8*, i8** %42, align 4
  %44 = getelementptr inbounds i8, i8* %43, i32 4
  store i8* %44, i8** %42, align 4
  %45 = bitcast i8* %43 to i32*
  %46 = load i32, i32* %45, align 4
  %47 = trunc i32 %46 to i8
  %48 = load i32, i32* %11, align 4
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %47, i8* %50, align 8
  br label %128

51:                                               ; preds = %26
  %52 = bitcast %struct.__va_list* %7 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i16
  %58 = load i32, i32* %11, align 4
  %59 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %58
  %60 = bitcast %union.jvalue* %59 to i16*
  store i16 %57, i16* %60, align 8
  br label %128

61:                                               ; preds = %26
  %62 = bitcast %struct.__va_list* %7 to i8**
  %63 = load i8*, i8** %62, align 4
  %64 = getelementptr inbounds i8, i8* %63, i32 4
  store i8* %64, i8** %62, align 4
  %65 = bitcast i8* %63 to i32*
  %66 = load i32, i32* %65, align 4
  %67 = trunc i32 %66 to i16
  %68 = load i32, i32* %11, align 4
  %69 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %68
  %70 = bitcast %union.jvalue* %69 to i16*
  store i16 %67, i16* %70, align 8
  br label %128

71:                                               ; preds = %26
  %72 = bitcast %struct.__va_list* %7 to i8**
  %73 = load i8*, i8** %72, align 4
  %74 = getelementptr inbounds i8, i8* %73, i32 4
  store i8* %74, i8** %72, align 4
  %75 = bitcast i8* %73 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = load i32, i32* %11, align 4
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %77
  %79 = bitcast %union.jvalue* %78 to i32*
  store i32 %76, i32* %79, align 8
  br label %128

80:                                               ; preds = %26
  %81 = bitcast %struct.__va_list* %7 to i8**
  %82 = load i8*, i8** %81, align 4
  %83 = getelementptr inbounds i8, i8* %82, i32 4
  store i8* %83, i8** %81, align 4
  %84 = bitcast i8* %82 to i32*
  %85 = load i32, i32* %84, align 4
  %86 = sext i32 %85 to i64
  %87 = load i32, i32* %11, align 4
  %88 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %87
  %89 = bitcast %union.jvalue* %88 to i64*
  store i64 %86, i64* %89, align 8
  br label %128

90:                                               ; preds = %26
  %91 = bitcast %struct.__va_list* %7 to i8**
  %92 = load i8*, i8** %91, align 4
  %93 = ptrtoint i8* %92 to i32
  %94 = add i32 %93, 7
  %95 = and i32 %94, -8
  %96 = inttoptr i32 %95 to i8*
  %97 = getelementptr inbounds i8, i8* %96, i32 8
  store i8* %97, i8** %91, align 4
  %98 = bitcast i8* %96 to double*
  %99 = load double, double* %98, align 8
  %100 = fptrunc double %99 to float
  %101 = load i32, i32* %11, align 4
  %102 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %101
  %103 = bitcast %union.jvalue* %102 to float*
  store float %100, float* %103, align 8
  br label %128

104:                                              ; preds = %26
  %105 = bitcast %struct.__va_list* %7 to i8**
  %106 = load i8*, i8** %105, align 4
  %107 = ptrtoint i8* %106 to i32
  %108 = add i32 %107, 7
  %109 = and i32 %108, -8
  %110 = inttoptr i32 %109 to i8*
  %111 = getelementptr inbounds i8, i8* %110, i32 8
  store i8* %111, i8** %105, align 4
  %112 = bitcast i8* %110 to double*
  %113 = load double, double* %112, align 8
  %114 = load i32, i32* %11, align 4
  %115 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %114
  %116 = bitcast %union.jvalue* %115 to double*
  store double %113, double* %116, align 8
  br label %128

117:                                              ; preds = %26
  %118 = bitcast %struct.__va_list* %7 to i8**
  %119 = load i8*, i8** %118, align 4
  %120 = getelementptr inbounds i8, i8* %119, i32 4
  store i8* %120, i8** %118, align 4
  %121 = bitcast i8* %119 to i8**
  %122 = load i8*, i8** %121, align 4
  %123 = bitcast i8* %122 to %struct._jobject*
  %124 = load i32, i32* %11, align 4
  %125 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %124
  %126 = bitcast %union.jvalue* %125 to %struct._jobject**
  store %struct._jobject* %123, %struct._jobject** %126, align 8
  br label %128

127:                                              ; preds = %26
  br label %128

128:                                              ; preds = %127, %117, %104, %90, %80, %71, %61, %51, %41, %31
  br label %129

129:                                              ; preds = %128
  %130 = load i32, i32* %11, align 4
  %131 = add nsw i32 %130, 1
  store i32 %131, i32* %11, align 4
  br label %22, !llvm.loop !71

132:                                              ; preds = %22
  %133 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %134 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %133, align 4
  %135 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %134, i32 0, i32 143
  %136 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %135, align 4
  %137 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %138 = load %struct._jobject*, %struct._jobject** %5, align 4
  %139 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  call void %136(%struct.JNINativeInterface_** noundef %137, %struct._jobject* noundef %138, %struct._jmethodID* noundef %139, %union.jvalue* noundef %140)
  %141 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %141)
  ret void
}

; Function Attrs: noinline nounwind optnone
define dso_local void @JNI_CallStaticVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %5, i32 0, i32 0
  %14 = bitcast i8** %13 to [1 x i32]*
  store [1 x i32] %3, [1 x i32]* %14, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 4
  store %struct._jobject* %1, %struct._jobject** %7, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %8, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 4
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 4
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %21 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %22 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %10, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %131, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %10, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %134

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  switch i32 %32, label %129 [
    i32 90, label %33
    i32 66, label %43
    i32 67, label %53
    i32 83, label %63
    i32 73, label %73
    i32 74, label %82
    i32 70, label %92
    i32 68, label %106
    i32 76, label %119
  ]

33:                                               ; preds = %28
  %34 = bitcast %struct.__va_list* %5 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %12, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %130

43:                                               ; preds = %28
  %44 = bitcast %struct.__va_list* %5 to i8**
  %45 = load i8*, i8** %44, align 4
  %46 = getelementptr inbounds i8, i8* %45, i32 4
  store i8* %46, i8** %44, align 4
  %47 = bitcast i8* %45 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = trunc i32 %48 to i8
  %50 = load i32, i32* %12, align 4
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %49, i8* %52, align 8
  br label %130

53:                                               ; preds = %28
  %54 = bitcast %struct.__va_list* %5 to i8**
  %55 = load i8*, i8** %54, align 4
  %56 = getelementptr inbounds i8, i8* %55, i32 4
  store i8* %56, i8** %54, align 4
  %57 = bitcast i8* %55 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i16
  %60 = load i32, i32* %12, align 4
  %61 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %60
  %62 = bitcast %union.jvalue* %61 to i16*
  store i16 %59, i16* %62, align 8
  br label %130

63:                                               ; preds = %28
  %64 = bitcast %struct.__va_list* %5 to i8**
  %65 = load i8*, i8** %64, align 4
  %66 = getelementptr inbounds i8, i8* %65, i32 4
  store i8* %66, i8** %64, align 4
  %67 = bitcast i8* %65 to i32*
  %68 = load i32, i32* %67, align 4
  %69 = trunc i32 %68 to i16
  %70 = load i32, i32* %12, align 4
  %71 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %70
  %72 = bitcast %union.jvalue* %71 to i16*
  store i16 %69, i16* %72, align 8
  br label %130

73:                                               ; preds = %28
  %74 = bitcast %struct.__va_list* %5 to i8**
  %75 = load i8*, i8** %74, align 4
  %76 = getelementptr inbounds i8, i8* %75, i32 4
  store i8* %76, i8** %74, align 4
  %77 = bitcast i8* %75 to i32*
  %78 = load i32, i32* %77, align 4
  %79 = load i32, i32* %12, align 4
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %79
  %81 = bitcast %union.jvalue* %80 to i32*
  store i32 %78, i32* %81, align 8
  br label %130

82:                                               ; preds = %28
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = sext i32 %87 to i64
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i64*
  store i64 %88, i64* %91, align 8
  br label %130

92:                                               ; preds = %28
  %93 = bitcast %struct.__va_list* %5 to i8**
  %94 = load i8*, i8** %93, align 4
  %95 = ptrtoint i8* %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to i8*
  %99 = getelementptr inbounds i8, i8* %98, i32 8
  store i8* %99, i8** %93, align 4
  %100 = bitcast i8* %98 to double*
  %101 = load double, double* %100, align 8
  %102 = fptrunc double %101 to float
  %103 = load i32, i32* %12, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to float*
  store float %102, float* %105, align 8
  br label %130

106:                                              ; preds = %28
  %107 = bitcast %struct.__va_list* %5 to i8**
  %108 = load i8*, i8** %107, align 4
  %109 = ptrtoint i8* %108 to i32
  %110 = add i32 %109, 7
  %111 = and i32 %110, -8
  %112 = inttoptr i32 %111 to i8*
  %113 = getelementptr inbounds i8, i8* %112, i32 8
  store i8* %113, i8** %107, align 4
  %114 = bitcast i8* %112 to double*
  %115 = load double, double* %114, align 8
  %116 = load i32, i32* %12, align 4
  %117 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %116
  %118 = bitcast %union.jvalue* %117 to double*
  store double %115, double* %118, align 8
  br label %130

119:                                              ; preds = %28
  %120 = bitcast %struct.__va_list* %5 to i8**
  %121 = load i8*, i8** %120, align 4
  %122 = getelementptr inbounds i8, i8* %121, i32 4
  store i8* %122, i8** %120, align 4
  %123 = bitcast i8* %121 to i8**
  %124 = load i8*, i8** %123, align 4
  %125 = bitcast i8* %124 to %struct._jobject*
  %126 = load i32, i32* %12, align 4
  %127 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %126
  %128 = bitcast %union.jvalue* %127 to %struct._jobject**
  store %struct._jobject* %125, %struct._jobject** %128, align 8
  br label %130

129:                                              ; preds = %28
  br label %130

130:                                              ; preds = %129, %119, %106, %92, %82, %73, %63, %53, %43, %33
  br label %131

131:                                              ; preds = %130
  %132 = load i32, i32* %12, align 4
  %133 = add nsw i32 %132, 1
  store i32 %133, i32* %12, align 4
  br label %24, !llvm.loop !72

134:                                              ; preds = %24
  %135 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %136 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %135, align 4
  %137 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %136, i32 0, i32 143
  %138 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %137, align 4
  %139 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %140 = load %struct._jobject*, %struct._jobject** %7, align 4
  %141 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  call void %138(%struct.JNINativeInterface_** noundef %139, %struct._jobject* noundef %140, %struct._jmethodID* noundef %141, %union.jvalue* noundef %142)
  ret void
}

attributes #0 = { noinline nounwind optnone "frame-pointer"="all" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+dsp,+fp64,+vfp2,+vfp2sp,+vfp3d16,+vfp3d16sp,-aes,-d32,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-neon,-sha2,-thumb-mode,-vfp3,-vfp3sp,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }
attributes #1 = { nofree nosync nounwind willreturn }

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
!42 = distinct !{!42, !11}
!43 = distinct !{!43, !11}
!44 = distinct !{!44, !11}
!45 = distinct !{!45, !11}
!46 = distinct !{!46, !11}
!47 = distinct !{!47, !11}
!48 = distinct !{!48, !11}
!49 = distinct !{!49, !11}
!50 = distinct !{!50, !11}
!51 = distinct !{!51, !11}
!52 = distinct !{!52, !11}
!53 = distinct !{!53, !11}
!54 = distinct !{!54, !11}
!55 = distinct !{!55, !11}
!56 = distinct !{!56, !11}
!57 = distinct !{!57, !11}
!58 = distinct !{!58, !11}
!59 = distinct !{!59, !11}
!60 = distinct !{!60, !11}
!61 = distinct !{!61, !11}
!62 = distinct !{!62, !11}
!63 = distinct !{!63, !11}
!64 = distinct !{!64, !11}
!65 = distinct !{!65, !11}
!66 = distinct !{!66, !11}
!67 = distinct !{!67, !11}
!68 = distinct !{!68, !11}
!69 = distinct !{!69, !11}
!70 = distinct !{!70, !11}
!71 = distinct !{!71, !11}
!72 = distinct !{!72, !11}
