; ModuleID = 'jni.c'
source_filename = "jni.c"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-gnueabihf"

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
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %22, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %186, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %10, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %189

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  %32 = icmp eq i32 %31, 90
  br i1 %32, label %33, label %43

33:                                               ; preds = %27
  %34 = bitcast %struct.__va_list* %7 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %11, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %185

43:                                               ; preds = %27
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  %48 = icmp eq i32 %47, 66
  br i1 %48, label %49, label %59

49:                                               ; preds = %43
  %50 = bitcast %struct.__va_list* %7 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %55, i8* %58, align 8
  br label %184

59:                                               ; preds = %43
  %60 = load i32, i32* %11, align 4
  %61 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %60
  %62 = load i8, i8* %61, align 1
  %63 = zext i8 %62 to i32
  %64 = icmp eq i32 %63, 67
  br i1 %64, label %65, label %75

65:                                               ; preds = %59
  %66 = bitcast %struct.__va_list* %7 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %11, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %183

75:                                               ; preds = %59
  %76 = load i32, i32* %11, align 4
  %77 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %76
  %78 = load i8, i8* %77, align 1
  %79 = zext i8 %78 to i32
  %80 = icmp eq i32 %79, 83
  br i1 %80, label %81, label %91

81:                                               ; preds = %75
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i16*
  store i16 %87, i16* %90, align 8
  br label %182

91:                                               ; preds = %75
  %92 = load i32, i32* %11, align 4
  %93 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %92
  %94 = load i8, i8* %93, align 1
  %95 = zext i8 %94 to i32
  %96 = icmp eq i32 %95, 73
  br i1 %96, label %97, label %106

97:                                               ; preds = %91
  %98 = bitcast %struct.__va_list* %7 to i8**
  %99 = load i8*, i8** %98, align 4
  %100 = getelementptr inbounds i8, i8* %99, i32 4
  store i8* %100, i8** %98, align 4
  %101 = bitcast i8* %99 to i32*
  %102 = load i32, i32* %101, align 4
  %103 = load i32, i32* %11, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %102, i32* %105, align 8
  br label %181

106:                                              ; preds = %91
  %107 = load i32, i32* %11, align 4
  %108 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %107
  %109 = load i8, i8* %108, align 1
  %110 = zext i8 %109 to i32
  %111 = icmp eq i32 %110, 74
  br i1 %111, label %112, label %122

112:                                              ; preds = %106
  %113 = bitcast %struct.__va_list* %7 to i8**
  %114 = load i8*, i8** %113, align 4
  %115 = getelementptr inbounds i8, i8* %114, i32 4
  store i8* %115, i8** %113, align 4
  %116 = bitcast i8* %114 to i32*
  %117 = load i32, i32* %116, align 4
  %118 = sext i32 %117 to i64
  %119 = load i32, i32* %11, align 4
  %120 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %119
  %121 = bitcast %union.jvalue* %120 to i64*
  store i64 %118, i64* %121, align 8
  br label %180

122:                                              ; preds = %106
  %123 = load i32, i32* %11, align 4
  %124 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %123
  %125 = load i8, i8* %124, align 1
  %126 = zext i8 %125 to i32
  %127 = icmp eq i32 %126, 70
  br i1 %127, label %128, label %142

128:                                              ; preds = %122
  %129 = bitcast %struct.__va_list* %7 to i8**
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
  %139 = load i32, i32* %11, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %139
  %141 = bitcast %union.jvalue* %140 to float*
  store float %138, float* %141, align 8
  br label %179

142:                                              ; preds = %122
  %143 = load i32, i32* %11, align 4
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %143
  %145 = load i8, i8* %144, align 1
  %146 = zext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %161

148:                                              ; preds = %142
  %149 = bitcast %struct.__va_list* %7 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = ptrtoint i8* %150 to i32
  %152 = add i32 %151, 7
  %153 = and i32 %152, -8
  %154 = inttoptr i32 %153 to i8*
  %155 = getelementptr inbounds i8, i8* %154, i32 8
  store i8* %155, i8** %149, align 4
  %156 = bitcast i8* %154 to double*
  %157 = load double, double* %156, align 8
  %158 = load i32, i32* %11, align 4
  %159 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %158
  %160 = bitcast %union.jvalue* %159 to double*
  store double %157, double* %160, align 8
  br label %178

161:                                              ; preds = %142
  %162 = load i32, i32* %11, align 4
  %163 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %162
  %164 = load i8, i8* %163, align 1
  %165 = zext i8 %164 to i32
  %166 = icmp eq i32 %165, 76
  br i1 %166, label %167, label %177

167:                                              ; preds = %161
  %168 = bitcast %struct.__va_list* %7 to i8**
  %169 = load i8*, i8** %168, align 4
  %170 = getelementptr inbounds i8, i8* %169, i32 4
  store i8* %170, i8** %168, align 4
  %171 = bitcast i8* %169 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = bitcast i8* %172 to %struct._jobject*
  %174 = load i32, i32* %11, align 4
  %175 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %174
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %176, align 8
  br label %177

177:                                              ; preds = %167, %161
  br label %178

178:                                              ; preds = %177, %148
  br label %179

179:                                              ; preds = %178, %128
  br label %180

180:                                              ; preds = %179, %112
  br label %181

181:                                              ; preds = %180, %97
  br label %182

182:                                              ; preds = %181, %81
  br label %183

183:                                              ; preds = %182, %65
  br label %184

184:                                              ; preds = %183, %49
  br label %185

185:                                              ; preds = %184, %33
  br label %186

186:                                              ; preds = %185
  %187 = load i32, i32* %11, align 4
  %188 = add nsw i32 %187, 1
  store i32 %188, i32* %11, align 4
  br label %23, !llvm.loop !10

189:                                              ; preds = %23
  %190 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %191 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %190, align 4
  %192 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %191, i32 0, i32 36
  %193 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %192, align 4
  %194 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %195 = load %struct._jobject*, %struct._jobject** %5, align 4
  %196 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %197 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  %198 = call %struct._jobject* %193(%struct.JNINativeInterface_** noundef %194, %struct._jobject* noundef %195, %struct._jmethodID* noundef %196, %union.jvalue* noundef %197)
  store %struct._jobject* %198, %struct._jobject** %12, align 4
  %199 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %199)
  %200 = load %struct._jobject*, %struct._jobject** %12, align 4
  ret %struct._jobject* %200
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
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !12

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 36
  %194 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %199 = call %struct._jobject* %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
  ret %struct._jobject* %199
}

; Function Attrs: noinline nounwind optnone
define dso_local %struct._jobject* @JNI_CallNonvirtualObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct.__va_list, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
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
  store i32 %24, i32* %12, align 4
  store i32 0, i32* %13, align 4
  br label %25

25:                                               ; preds = %188, %4
  %26 = load i32, i32* %13, align 4
  %27 = load i32, i32* %12, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %191

29:                                               ; preds = %25
  %30 = load i32, i32* %13, align 4
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %30
  %32 = load i8, i8* %31, align 1
  %33 = zext i8 %32 to i32
  %34 = icmp eq i32 %33, 90
  br i1 %34, label %35, label %45

35:                                               ; preds = %29
  %36 = bitcast %struct.__va_list* %9 to i8**
  %37 = load i8*, i8** %36, align 4
  %38 = getelementptr inbounds i8, i8* %37, i32 4
  store i8* %38, i8** %36, align 4
  %39 = bitcast i8* %37 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = load i32, i32* %13, align 4
  %43 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %42
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %41, i8* %44, align 8
  br label %187

45:                                               ; preds = %29
  %46 = load i32, i32* %13, align 4
  %47 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %46
  %48 = load i8, i8* %47, align 1
  %49 = zext i8 %48 to i32
  %50 = icmp eq i32 %49, 66
  br i1 %50, label %51, label %61

51:                                               ; preds = %45
  %52 = bitcast %struct.__va_list* %9 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load i32, i32* %13, align 4
  %59 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %58
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %57, i8* %60, align 8
  br label %186

61:                                               ; preds = %45
  %62 = load i32, i32* %13, align 4
  %63 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %62
  %64 = load i8, i8* %63, align 1
  %65 = zext i8 %64 to i32
  %66 = icmp eq i32 %65, 67
  br i1 %66, label %67, label %77

67:                                               ; preds = %61
  %68 = bitcast %struct.__va_list* %9 to i8**
  %69 = load i8*, i8** %68, align 4
  %70 = getelementptr inbounds i8, i8* %69, i32 4
  store i8* %70, i8** %68, align 4
  %71 = bitcast i8* %69 to i32*
  %72 = load i32, i32* %71, align 4
  %73 = trunc i32 %72 to i16
  %74 = load i32, i32* %13, align 4
  %75 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %74
  %76 = bitcast %union.jvalue* %75 to i16*
  store i16 %73, i16* %76, align 8
  br label %185

77:                                               ; preds = %61
  %78 = load i32, i32* %13, align 4
  %79 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %78
  %80 = load i8, i8* %79, align 1
  %81 = zext i8 %80 to i32
  %82 = icmp eq i32 %81, 83
  br i1 %82, label %83, label %93

83:                                               ; preds = %77
  %84 = bitcast %struct.__va_list* %9 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = trunc i32 %88 to i16
  %90 = load i32, i32* %13, align 4
  %91 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %90
  %92 = bitcast %union.jvalue* %91 to i16*
  store i16 %89, i16* %92, align 8
  br label %184

93:                                               ; preds = %77
  %94 = load i32, i32* %13, align 4
  %95 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %94
  %96 = load i8, i8* %95, align 1
  %97 = zext i8 %96 to i32
  %98 = icmp eq i32 %97, 73
  br i1 %98, label %99, label %108

99:                                               ; preds = %93
  %100 = bitcast %struct.__va_list* %9 to i8**
  %101 = load i8*, i8** %100, align 4
  %102 = getelementptr inbounds i8, i8* %101, i32 4
  store i8* %102, i8** %100, align 4
  %103 = bitcast i8* %101 to i32*
  %104 = load i32, i32* %103, align 4
  %105 = load i32, i32* %13, align 4
  %106 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %105
  %107 = bitcast %union.jvalue* %106 to i32*
  store i32 %104, i32* %107, align 8
  br label %183

108:                                              ; preds = %93
  %109 = load i32, i32* %13, align 4
  %110 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %109
  %111 = load i8, i8* %110, align 1
  %112 = zext i8 %111 to i32
  %113 = icmp eq i32 %112, 74
  br i1 %113, label %114, label %124

114:                                              ; preds = %108
  %115 = bitcast %struct.__va_list* %9 to i8**
  %116 = load i8*, i8** %115, align 4
  %117 = getelementptr inbounds i8, i8* %116, i32 4
  store i8* %117, i8** %115, align 4
  %118 = bitcast i8* %116 to i32*
  %119 = load i32, i32* %118, align 4
  %120 = sext i32 %119 to i64
  %121 = load i32, i32* %13, align 4
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %120, i64* %123, align 8
  br label %182

124:                                              ; preds = %108
  %125 = load i32, i32* %13, align 4
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %125
  %127 = load i8, i8* %126, align 1
  %128 = zext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %144

130:                                              ; preds = %124
  %131 = bitcast %struct.__va_list* %9 to i8**
  %132 = load i8*, i8** %131, align 4
  %133 = ptrtoint i8* %132 to i32
  %134 = add i32 %133, 7
  %135 = and i32 %134, -8
  %136 = inttoptr i32 %135 to i8*
  %137 = getelementptr inbounds i8, i8* %136, i32 8
  store i8* %137, i8** %131, align 4
  %138 = bitcast i8* %136 to double*
  %139 = load double, double* %138, align 8
  %140 = fptrunc double %139 to float
  %141 = load i32, i32* %13, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %140, float* %143, align 8
  br label %181

144:                                              ; preds = %124
  %145 = load i32, i32* %13, align 4
  %146 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %145
  %147 = load i8, i8* %146, align 1
  %148 = zext i8 %147 to i32
  %149 = icmp eq i32 %148, 68
  br i1 %149, label %150, label %163

150:                                              ; preds = %144
  %151 = bitcast %struct.__va_list* %9 to i8**
  %152 = load i8*, i8** %151, align 4
  %153 = ptrtoint i8* %152 to i32
  %154 = add i32 %153, 7
  %155 = and i32 %154, -8
  %156 = inttoptr i32 %155 to i8*
  %157 = getelementptr inbounds i8, i8* %156, i32 8
  store i8* %157, i8** %151, align 4
  %158 = bitcast i8* %156 to double*
  %159 = load double, double* %158, align 8
  %160 = load i32, i32* %13, align 4
  %161 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %160
  %162 = bitcast %union.jvalue* %161 to double*
  store double %159, double* %162, align 8
  br label %180

163:                                              ; preds = %144
  %164 = load i32, i32* %13, align 4
  %165 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %164
  %166 = load i8, i8* %165, align 1
  %167 = zext i8 %166 to i32
  %168 = icmp eq i32 %167, 76
  br i1 %168, label %169, label %179

169:                                              ; preds = %163
  %170 = bitcast %struct.__va_list* %9 to i8**
  %171 = load i8*, i8** %170, align 4
  %172 = getelementptr inbounds i8, i8* %171, i32 4
  store i8* %172, i8** %170, align 4
  %173 = bitcast i8* %171 to i8**
  %174 = load i8*, i8** %173, align 4
  %175 = bitcast i8* %174 to %struct._jobject*
  %176 = load i32, i32* %13, align 4
  %177 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %176
  %178 = bitcast %union.jvalue* %177 to %struct._jobject**
  store %struct._jobject* %175, %struct._jobject** %178, align 8
  br label %179

179:                                              ; preds = %169, %163
  br label %180

180:                                              ; preds = %179, %150
  br label %181

181:                                              ; preds = %180, %130
  br label %182

182:                                              ; preds = %181, %114
  br label %183

183:                                              ; preds = %182, %99
  br label %184

184:                                              ; preds = %183, %83
  br label %185

185:                                              ; preds = %184, %67
  br label %186

186:                                              ; preds = %185, %51
  br label %187

187:                                              ; preds = %186, %35
  br label %188

188:                                              ; preds = %187
  %189 = load i32, i32* %13, align 4
  %190 = add nsw i32 %189, 1
  store i32 %190, i32* %13, align 4
  br label %25, !llvm.loop !13

191:                                              ; preds = %25
  %192 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %193 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %192, align 4
  %194 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %193, i32 0, i32 66
  %195 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %194, align 4
  %196 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %197 = load %struct._jobject*, %struct._jobject** %6, align 4
  %198 = load %struct._jobject*, %struct._jobject** %7, align 4
  %199 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %200 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %201 = call %struct._jobject* %195(%struct.JNINativeInterface_** noundef %196, %struct._jobject* noundef %197, %struct._jobject* noundef %198, %struct._jmethodID* noundef %199, %union.jvalue* noundef %200)
  store %struct._jobject* %201, %struct._jobject** %14, align 4
  %202 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %202)
  %203 = load %struct._jobject*, %struct._jobject** %14, align 4
  ret %struct._jobject* %203
}

; Function Attrs: noinline nounwind optnone
define dso_local %struct._jobject* @JNI_CallNonvirtualObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
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
  store i32 %25, i32* %13, align 4
  store i32 0, i32* %14, align 4
  br label %26

26:                                               ; preds = %189, %5
  %27 = load i32, i32* %14, align 4
  %28 = load i32, i32* %13, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %192

30:                                               ; preds = %26
  %31 = load i32, i32* %14, align 4
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %31
  %33 = load i8, i8* %32, align 1
  %34 = zext i8 %33 to i32
  %35 = icmp eq i32 %34, 90
  br i1 %35, label %36, label %46

36:                                               ; preds = %30
  %37 = bitcast %struct.__va_list* %6 to i8**
  %38 = load i8*, i8** %37, align 4
  %39 = getelementptr inbounds i8, i8* %38, i32 4
  store i8* %39, i8** %37, align 4
  %40 = bitcast i8* %38 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = load i32, i32* %14, align 4
  %44 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %43
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %42, i8* %45, align 8
  br label %188

46:                                               ; preds = %30
  %47 = load i32, i32* %14, align 4
  %48 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %47
  %49 = load i8, i8* %48, align 1
  %50 = zext i8 %49 to i32
  %51 = icmp eq i32 %50, 66
  br i1 %51, label %52, label %62

52:                                               ; preds = %46
  %53 = bitcast %struct.__va_list* %6 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = load i32, i32* %14, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %58, i8* %61, align 8
  br label %187

62:                                               ; preds = %46
  %63 = load i32, i32* %14, align 4
  %64 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %63
  %65 = load i8, i8* %64, align 1
  %66 = zext i8 %65 to i32
  %67 = icmp eq i32 %66, 67
  br i1 %67, label %68, label %78

68:                                               ; preds = %62
  %69 = bitcast %struct.__va_list* %6 to i8**
  %70 = load i8*, i8** %69, align 4
  %71 = getelementptr inbounds i8, i8* %70, i32 4
  store i8* %71, i8** %69, align 4
  %72 = bitcast i8* %70 to i32*
  %73 = load i32, i32* %72, align 4
  %74 = trunc i32 %73 to i16
  %75 = load i32, i32* %14, align 4
  %76 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %75
  %77 = bitcast %union.jvalue* %76 to i16*
  store i16 %74, i16* %77, align 8
  br label %186

78:                                               ; preds = %62
  %79 = load i32, i32* %14, align 4
  %80 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %79
  %81 = load i8, i8* %80, align 1
  %82 = zext i8 %81 to i32
  %83 = icmp eq i32 %82, 83
  br i1 %83, label %84, label %94

84:                                               ; preds = %78
  %85 = bitcast %struct.__va_list* %6 to i8**
  %86 = load i8*, i8** %85, align 4
  %87 = getelementptr inbounds i8, i8* %86, i32 4
  store i8* %87, i8** %85, align 4
  %88 = bitcast i8* %86 to i32*
  %89 = load i32, i32* %88, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %14, align 4
  %92 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %91
  %93 = bitcast %union.jvalue* %92 to i16*
  store i16 %90, i16* %93, align 8
  br label %185

94:                                               ; preds = %78
  %95 = load i32, i32* %14, align 4
  %96 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %95
  %97 = load i8, i8* %96, align 1
  %98 = zext i8 %97 to i32
  %99 = icmp eq i32 %98, 73
  br i1 %99, label %100, label %109

100:                                              ; preds = %94
  %101 = bitcast %struct.__va_list* %6 to i8**
  %102 = load i8*, i8** %101, align 4
  %103 = getelementptr inbounds i8, i8* %102, i32 4
  store i8* %103, i8** %101, align 4
  %104 = bitcast i8* %102 to i32*
  %105 = load i32, i32* %104, align 4
  %106 = load i32, i32* %14, align 4
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %105, i32* %108, align 8
  br label %184

109:                                              ; preds = %94
  %110 = load i32, i32* %14, align 4
  %111 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %110
  %112 = load i8, i8* %111, align 1
  %113 = zext i8 %112 to i32
  %114 = icmp eq i32 %113, 74
  br i1 %114, label %115, label %125

115:                                              ; preds = %109
  %116 = bitcast %struct.__va_list* %6 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = getelementptr inbounds i8, i8* %117, i32 4
  store i8* %118, i8** %116, align 4
  %119 = bitcast i8* %117 to i32*
  %120 = load i32, i32* %119, align 4
  %121 = sext i32 %120 to i64
  %122 = load i32, i32* %14, align 4
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %121, i64* %124, align 8
  br label %183

125:                                              ; preds = %109
  %126 = load i32, i32* %14, align 4
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %126
  %128 = load i8, i8* %127, align 1
  %129 = zext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %145

131:                                              ; preds = %125
  %132 = bitcast %struct.__va_list* %6 to i8**
  %133 = load i8*, i8** %132, align 4
  %134 = ptrtoint i8* %133 to i32
  %135 = add i32 %134, 7
  %136 = and i32 %135, -8
  %137 = inttoptr i32 %136 to i8*
  %138 = getelementptr inbounds i8, i8* %137, i32 8
  store i8* %138, i8** %132, align 4
  %139 = bitcast i8* %137 to double*
  %140 = load double, double* %139, align 8
  %141 = fptrunc double %140 to float
  %142 = load i32, i32* %14, align 4
  %143 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %142
  %144 = bitcast %union.jvalue* %143 to float*
  store float %141, float* %144, align 8
  br label %182

145:                                              ; preds = %125
  %146 = load i32, i32* %14, align 4
  %147 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %146
  %148 = load i8, i8* %147, align 1
  %149 = zext i8 %148 to i32
  %150 = icmp eq i32 %149, 68
  br i1 %150, label %151, label %164

151:                                              ; preds = %145
  %152 = bitcast %struct.__va_list* %6 to i8**
  %153 = load i8*, i8** %152, align 4
  %154 = ptrtoint i8* %153 to i32
  %155 = add i32 %154, 7
  %156 = and i32 %155, -8
  %157 = inttoptr i32 %156 to i8*
  %158 = getelementptr inbounds i8, i8* %157, i32 8
  store i8* %158, i8** %152, align 4
  %159 = bitcast i8* %157 to double*
  %160 = load double, double* %159, align 8
  %161 = load i32, i32* %14, align 4
  %162 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %161
  %163 = bitcast %union.jvalue* %162 to double*
  store double %160, double* %163, align 8
  br label %181

164:                                              ; preds = %145
  %165 = load i32, i32* %14, align 4
  %166 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %165
  %167 = load i8, i8* %166, align 1
  %168 = zext i8 %167 to i32
  %169 = icmp eq i32 %168, 76
  br i1 %169, label %170, label %180

170:                                              ; preds = %164
  %171 = bitcast %struct.__va_list* %6 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = getelementptr inbounds i8, i8* %172, i32 4
  store i8* %173, i8** %171, align 4
  %174 = bitcast i8* %172 to i8**
  %175 = load i8*, i8** %174, align 4
  %176 = bitcast i8* %175 to %struct._jobject*
  %177 = load i32, i32* %14, align 4
  %178 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %177
  %179 = bitcast %union.jvalue* %178 to %struct._jobject**
  store %struct._jobject* %176, %struct._jobject** %179, align 8
  br label %180

180:                                              ; preds = %170, %164
  br label %181

181:                                              ; preds = %180, %151
  br label %182

182:                                              ; preds = %181, %131
  br label %183

183:                                              ; preds = %182, %115
  br label %184

184:                                              ; preds = %183, %100
  br label %185

185:                                              ; preds = %184, %84
  br label %186

186:                                              ; preds = %185, %68
  br label %187

187:                                              ; preds = %186, %52
  br label %188

188:                                              ; preds = %187, %36
  br label %189

189:                                              ; preds = %188
  %190 = load i32, i32* %14, align 4
  %191 = add nsw i32 %190, 1
  store i32 %191, i32* %14, align 4
  br label %26, !llvm.loop !14

192:                                              ; preds = %26
  %193 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %194 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %193, align 4
  %195 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %194, i32 0, i32 66
  %196 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %195, align 4
  %197 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %198 = load %struct._jobject*, %struct._jobject** %8, align 4
  %199 = load %struct._jobject*, %struct._jobject** %9, align 4
  %200 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %201 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 0
  %202 = call %struct._jobject* %196(%struct.JNINativeInterface_** noundef %197, %struct._jobject* noundef %198, %struct._jobject* noundef %199, %struct._jmethodID* noundef %200, %union.jvalue* noundef %201)
  ret %struct._jobject* %202
}

; Function Attrs: noinline nounwind optnone
define dso_local %struct._jobject* @JNI_CallStaticObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %22, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %186, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %10, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %189

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  %32 = icmp eq i32 %31, 90
  br i1 %32, label %33, label %43

33:                                               ; preds = %27
  %34 = bitcast %struct.__va_list* %7 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %11, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %185

43:                                               ; preds = %27
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  %48 = icmp eq i32 %47, 66
  br i1 %48, label %49, label %59

49:                                               ; preds = %43
  %50 = bitcast %struct.__va_list* %7 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %55, i8* %58, align 8
  br label %184

59:                                               ; preds = %43
  %60 = load i32, i32* %11, align 4
  %61 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %60
  %62 = load i8, i8* %61, align 1
  %63 = zext i8 %62 to i32
  %64 = icmp eq i32 %63, 67
  br i1 %64, label %65, label %75

65:                                               ; preds = %59
  %66 = bitcast %struct.__va_list* %7 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %11, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %183

75:                                               ; preds = %59
  %76 = load i32, i32* %11, align 4
  %77 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %76
  %78 = load i8, i8* %77, align 1
  %79 = zext i8 %78 to i32
  %80 = icmp eq i32 %79, 83
  br i1 %80, label %81, label %91

81:                                               ; preds = %75
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i16*
  store i16 %87, i16* %90, align 8
  br label %182

91:                                               ; preds = %75
  %92 = load i32, i32* %11, align 4
  %93 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %92
  %94 = load i8, i8* %93, align 1
  %95 = zext i8 %94 to i32
  %96 = icmp eq i32 %95, 73
  br i1 %96, label %97, label %106

97:                                               ; preds = %91
  %98 = bitcast %struct.__va_list* %7 to i8**
  %99 = load i8*, i8** %98, align 4
  %100 = getelementptr inbounds i8, i8* %99, i32 4
  store i8* %100, i8** %98, align 4
  %101 = bitcast i8* %99 to i32*
  %102 = load i32, i32* %101, align 4
  %103 = load i32, i32* %11, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %102, i32* %105, align 8
  br label %181

106:                                              ; preds = %91
  %107 = load i32, i32* %11, align 4
  %108 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %107
  %109 = load i8, i8* %108, align 1
  %110 = zext i8 %109 to i32
  %111 = icmp eq i32 %110, 74
  br i1 %111, label %112, label %122

112:                                              ; preds = %106
  %113 = bitcast %struct.__va_list* %7 to i8**
  %114 = load i8*, i8** %113, align 4
  %115 = getelementptr inbounds i8, i8* %114, i32 4
  store i8* %115, i8** %113, align 4
  %116 = bitcast i8* %114 to i32*
  %117 = load i32, i32* %116, align 4
  %118 = sext i32 %117 to i64
  %119 = load i32, i32* %11, align 4
  %120 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %119
  %121 = bitcast %union.jvalue* %120 to i64*
  store i64 %118, i64* %121, align 8
  br label %180

122:                                              ; preds = %106
  %123 = load i32, i32* %11, align 4
  %124 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %123
  %125 = load i8, i8* %124, align 1
  %126 = zext i8 %125 to i32
  %127 = icmp eq i32 %126, 70
  br i1 %127, label %128, label %142

128:                                              ; preds = %122
  %129 = bitcast %struct.__va_list* %7 to i8**
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
  %139 = load i32, i32* %11, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %139
  %141 = bitcast %union.jvalue* %140 to float*
  store float %138, float* %141, align 8
  br label %179

142:                                              ; preds = %122
  %143 = load i32, i32* %11, align 4
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %143
  %145 = load i8, i8* %144, align 1
  %146 = zext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %161

148:                                              ; preds = %142
  %149 = bitcast %struct.__va_list* %7 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = ptrtoint i8* %150 to i32
  %152 = add i32 %151, 7
  %153 = and i32 %152, -8
  %154 = inttoptr i32 %153 to i8*
  %155 = getelementptr inbounds i8, i8* %154, i32 8
  store i8* %155, i8** %149, align 4
  %156 = bitcast i8* %154 to double*
  %157 = load double, double* %156, align 8
  %158 = load i32, i32* %11, align 4
  %159 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %158
  %160 = bitcast %union.jvalue* %159 to double*
  store double %157, double* %160, align 8
  br label %178

161:                                              ; preds = %142
  %162 = load i32, i32* %11, align 4
  %163 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %162
  %164 = load i8, i8* %163, align 1
  %165 = zext i8 %164 to i32
  %166 = icmp eq i32 %165, 76
  br i1 %166, label %167, label %177

167:                                              ; preds = %161
  %168 = bitcast %struct.__va_list* %7 to i8**
  %169 = load i8*, i8** %168, align 4
  %170 = getelementptr inbounds i8, i8* %169, i32 4
  store i8* %170, i8** %168, align 4
  %171 = bitcast i8* %169 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = bitcast i8* %172 to %struct._jobject*
  %174 = load i32, i32* %11, align 4
  %175 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %174
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %176, align 8
  br label %177

177:                                              ; preds = %167, %161
  br label %178

178:                                              ; preds = %177, %148
  br label %179

179:                                              ; preds = %178, %128
  br label %180

180:                                              ; preds = %179, %112
  br label %181

181:                                              ; preds = %180, %97
  br label %182

182:                                              ; preds = %181, %81
  br label %183

183:                                              ; preds = %182, %65
  br label %184

184:                                              ; preds = %183, %49
  br label %185

185:                                              ; preds = %184, %33
  br label %186

186:                                              ; preds = %185
  %187 = load i32, i32* %11, align 4
  %188 = add nsw i32 %187, 1
  store i32 %188, i32* %11, align 4
  br label %23, !llvm.loop !15

189:                                              ; preds = %23
  %190 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %191 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %190, align 4
  %192 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %191, i32 0, i32 116
  %193 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %192, align 4
  %194 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %195 = load %struct._jobject*, %struct._jobject** %5, align 4
  %196 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %197 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  %198 = call %struct._jobject* %193(%struct.JNINativeInterface_** noundef %194, %struct._jobject* noundef %195, %struct._jmethodID* noundef %196, %union.jvalue* noundef %197)
  store %struct._jobject* %198, %struct._jobject** %12, align 4
  %199 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %199)
  %200 = load %struct._jobject*, %struct._jobject** %12, align 4
  ret %struct._jobject* %200
}

; Function Attrs: noinline nounwind optnone
define dso_local %struct._jobject* @JNI_CallStaticObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !16

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 116
  %194 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %199 = call %struct._jobject* %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
  ret %struct._jobject* %199
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i8 @JNI_CallBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %22, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %186, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %10, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %189

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  %32 = icmp eq i32 %31, 90
  br i1 %32, label %33, label %43

33:                                               ; preds = %27
  %34 = bitcast %struct.__va_list* %7 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %11, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %185

43:                                               ; preds = %27
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  %48 = icmp eq i32 %47, 66
  br i1 %48, label %49, label %59

49:                                               ; preds = %43
  %50 = bitcast %struct.__va_list* %7 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %55, i8* %58, align 8
  br label %184

59:                                               ; preds = %43
  %60 = load i32, i32* %11, align 4
  %61 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %60
  %62 = load i8, i8* %61, align 1
  %63 = zext i8 %62 to i32
  %64 = icmp eq i32 %63, 67
  br i1 %64, label %65, label %75

65:                                               ; preds = %59
  %66 = bitcast %struct.__va_list* %7 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %11, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %183

75:                                               ; preds = %59
  %76 = load i32, i32* %11, align 4
  %77 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %76
  %78 = load i8, i8* %77, align 1
  %79 = zext i8 %78 to i32
  %80 = icmp eq i32 %79, 83
  br i1 %80, label %81, label %91

81:                                               ; preds = %75
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i16*
  store i16 %87, i16* %90, align 8
  br label %182

91:                                               ; preds = %75
  %92 = load i32, i32* %11, align 4
  %93 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %92
  %94 = load i8, i8* %93, align 1
  %95 = zext i8 %94 to i32
  %96 = icmp eq i32 %95, 73
  br i1 %96, label %97, label %106

97:                                               ; preds = %91
  %98 = bitcast %struct.__va_list* %7 to i8**
  %99 = load i8*, i8** %98, align 4
  %100 = getelementptr inbounds i8, i8* %99, i32 4
  store i8* %100, i8** %98, align 4
  %101 = bitcast i8* %99 to i32*
  %102 = load i32, i32* %101, align 4
  %103 = load i32, i32* %11, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %102, i32* %105, align 8
  br label %181

106:                                              ; preds = %91
  %107 = load i32, i32* %11, align 4
  %108 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %107
  %109 = load i8, i8* %108, align 1
  %110 = zext i8 %109 to i32
  %111 = icmp eq i32 %110, 74
  br i1 %111, label %112, label %122

112:                                              ; preds = %106
  %113 = bitcast %struct.__va_list* %7 to i8**
  %114 = load i8*, i8** %113, align 4
  %115 = getelementptr inbounds i8, i8* %114, i32 4
  store i8* %115, i8** %113, align 4
  %116 = bitcast i8* %114 to i32*
  %117 = load i32, i32* %116, align 4
  %118 = sext i32 %117 to i64
  %119 = load i32, i32* %11, align 4
  %120 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %119
  %121 = bitcast %union.jvalue* %120 to i64*
  store i64 %118, i64* %121, align 8
  br label %180

122:                                              ; preds = %106
  %123 = load i32, i32* %11, align 4
  %124 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %123
  %125 = load i8, i8* %124, align 1
  %126 = zext i8 %125 to i32
  %127 = icmp eq i32 %126, 70
  br i1 %127, label %128, label %142

128:                                              ; preds = %122
  %129 = bitcast %struct.__va_list* %7 to i8**
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
  %139 = load i32, i32* %11, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %139
  %141 = bitcast %union.jvalue* %140 to float*
  store float %138, float* %141, align 8
  br label %179

142:                                              ; preds = %122
  %143 = load i32, i32* %11, align 4
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %143
  %145 = load i8, i8* %144, align 1
  %146 = zext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %161

148:                                              ; preds = %142
  %149 = bitcast %struct.__va_list* %7 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = ptrtoint i8* %150 to i32
  %152 = add i32 %151, 7
  %153 = and i32 %152, -8
  %154 = inttoptr i32 %153 to i8*
  %155 = getelementptr inbounds i8, i8* %154, i32 8
  store i8* %155, i8** %149, align 4
  %156 = bitcast i8* %154 to double*
  %157 = load double, double* %156, align 8
  %158 = load i32, i32* %11, align 4
  %159 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %158
  %160 = bitcast %union.jvalue* %159 to double*
  store double %157, double* %160, align 8
  br label %178

161:                                              ; preds = %142
  %162 = load i32, i32* %11, align 4
  %163 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %162
  %164 = load i8, i8* %163, align 1
  %165 = zext i8 %164 to i32
  %166 = icmp eq i32 %165, 76
  br i1 %166, label %167, label %177

167:                                              ; preds = %161
  %168 = bitcast %struct.__va_list* %7 to i8**
  %169 = load i8*, i8** %168, align 4
  %170 = getelementptr inbounds i8, i8* %169, i32 4
  store i8* %170, i8** %168, align 4
  %171 = bitcast i8* %169 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = bitcast i8* %172 to %struct._jobject*
  %174 = load i32, i32* %11, align 4
  %175 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %174
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %176, align 8
  br label %177

177:                                              ; preds = %167, %161
  br label %178

178:                                              ; preds = %177, %148
  br label %179

179:                                              ; preds = %178, %128
  br label %180

180:                                              ; preds = %179, %112
  br label %181

181:                                              ; preds = %180, %97
  br label %182

182:                                              ; preds = %181, %81
  br label %183

183:                                              ; preds = %182, %65
  br label %184

184:                                              ; preds = %183, %49
  br label %185

185:                                              ; preds = %184, %33
  br label %186

186:                                              ; preds = %185
  %187 = load i32, i32* %11, align 4
  %188 = add nsw i32 %187, 1
  store i32 %188, i32* %11, align 4
  br label %23, !llvm.loop !17

189:                                              ; preds = %23
  %190 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %191 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %190, align 4
  %192 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %191, i32 0, i32 39
  %193 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %192, align 4
  %194 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %195 = load %struct._jobject*, %struct._jobject** %5, align 4
  %196 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %197 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  %198 = call zeroext i8 %193(%struct.JNINativeInterface_** noundef %194, %struct._jobject* noundef %195, %struct._jmethodID* noundef %196, %union.jvalue* noundef %197)
  store i8 %198, i8* %12, align 1
  %199 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %199)
  %200 = load i8, i8* %12, align 1
  ret i8 %200
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i8 @JNI_CallBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !18

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 39
  %194 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %199 = call zeroext i8 %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
  ret i8 %199
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i8 @JNI_CallNonvirtualBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct.__va_list, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
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
  store i32 %24, i32* %12, align 4
  store i32 0, i32* %13, align 4
  br label %25

25:                                               ; preds = %188, %4
  %26 = load i32, i32* %13, align 4
  %27 = load i32, i32* %12, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %191

29:                                               ; preds = %25
  %30 = load i32, i32* %13, align 4
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %30
  %32 = load i8, i8* %31, align 1
  %33 = zext i8 %32 to i32
  %34 = icmp eq i32 %33, 90
  br i1 %34, label %35, label %45

35:                                               ; preds = %29
  %36 = bitcast %struct.__va_list* %9 to i8**
  %37 = load i8*, i8** %36, align 4
  %38 = getelementptr inbounds i8, i8* %37, i32 4
  store i8* %38, i8** %36, align 4
  %39 = bitcast i8* %37 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = load i32, i32* %13, align 4
  %43 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %42
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %41, i8* %44, align 8
  br label %187

45:                                               ; preds = %29
  %46 = load i32, i32* %13, align 4
  %47 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %46
  %48 = load i8, i8* %47, align 1
  %49 = zext i8 %48 to i32
  %50 = icmp eq i32 %49, 66
  br i1 %50, label %51, label %61

51:                                               ; preds = %45
  %52 = bitcast %struct.__va_list* %9 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load i32, i32* %13, align 4
  %59 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %58
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %57, i8* %60, align 8
  br label %186

61:                                               ; preds = %45
  %62 = load i32, i32* %13, align 4
  %63 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %62
  %64 = load i8, i8* %63, align 1
  %65 = zext i8 %64 to i32
  %66 = icmp eq i32 %65, 67
  br i1 %66, label %67, label %77

67:                                               ; preds = %61
  %68 = bitcast %struct.__va_list* %9 to i8**
  %69 = load i8*, i8** %68, align 4
  %70 = getelementptr inbounds i8, i8* %69, i32 4
  store i8* %70, i8** %68, align 4
  %71 = bitcast i8* %69 to i32*
  %72 = load i32, i32* %71, align 4
  %73 = trunc i32 %72 to i16
  %74 = load i32, i32* %13, align 4
  %75 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %74
  %76 = bitcast %union.jvalue* %75 to i16*
  store i16 %73, i16* %76, align 8
  br label %185

77:                                               ; preds = %61
  %78 = load i32, i32* %13, align 4
  %79 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %78
  %80 = load i8, i8* %79, align 1
  %81 = zext i8 %80 to i32
  %82 = icmp eq i32 %81, 83
  br i1 %82, label %83, label %93

83:                                               ; preds = %77
  %84 = bitcast %struct.__va_list* %9 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = trunc i32 %88 to i16
  %90 = load i32, i32* %13, align 4
  %91 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %90
  %92 = bitcast %union.jvalue* %91 to i16*
  store i16 %89, i16* %92, align 8
  br label %184

93:                                               ; preds = %77
  %94 = load i32, i32* %13, align 4
  %95 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %94
  %96 = load i8, i8* %95, align 1
  %97 = zext i8 %96 to i32
  %98 = icmp eq i32 %97, 73
  br i1 %98, label %99, label %108

99:                                               ; preds = %93
  %100 = bitcast %struct.__va_list* %9 to i8**
  %101 = load i8*, i8** %100, align 4
  %102 = getelementptr inbounds i8, i8* %101, i32 4
  store i8* %102, i8** %100, align 4
  %103 = bitcast i8* %101 to i32*
  %104 = load i32, i32* %103, align 4
  %105 = load i32, i32* %13, align 4
  %106 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %105
  %107 = bitcast %union.jvalue* %106 to i32*
  store i32 %104, i32* %107, align 8
  br label %183

108:                                              ; preds = %93
  %109 = load i32, i32* %13, align 4
  %110 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %109
  %111 = load i8, i8* %110, align 1
  %112 = zext i8 %111 to i32
  %113 = icmp eq i32 %112, 74
  br i1 %113, label %114, label %124

114:                                              ; preds = %108
  %115 = bitcast %struct.__va_list* %9 to i8**
  %116 = load i8*, i8** %115, align 4
  %117 = getelementptr inbounds i8, i8* %116, i32 4
  store i8* %117, i8** %115, align 4
  %118 = bitcast i8* %116 to i32*
  %119 = load i32, i32* %118, align 4
  %120 = sext i32 %119 to i64
  %121 = load i32, i32* %13, align 4
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %120, i64* %123, align 8
  br label %182

124:                                              ; preds = %108
  %125 = load i32, i32* %13, align 4
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %125
  %127 = load i8, i8* %126, align 1
  %128 = zext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %144

130:                                              ; preds = %124
  %131 = bitcast %struct.__va_list* %9 to i8**
  %132 = load i8*, i8** %131, align 4
  %133 = ptrtoint i8* %132 to i32
  %134 = add i32 %133, 7
  %135 = and i32 %134, -8
  %136 = inttoptr i32 %135 to i8*
  %137 = getelementptr inbounds i8, i8* %136, i32 8
  store i8* %137, i8** %131, align 4
  %138 = bitcast i8* %136 to double*
  %139 = load double, double* %138, align 8
  %140 = fptrunc double %139 to float
  %141 = load i32, i32* %13, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %140, float* %143, align 8
  br label %181

144:                                              ; preds = %124
  %145 = load i32, i32* %13, align 4
  %146 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %145
  %147 = load i8, i8* %146, align 1
  %148 = zext i8 %147 to i32
  %149 = icmp eq i32 %148, 68
  br i1 %149, label %150, label %163

150:                                              ; preds = %144
  %151 = bitcast %struct.__va_list* %9 to i8**
  %152 = load i8*, i8** %151, align 4
  %153 = ptrtoint i8* %152 to i32
  %154 = add i32 %153, 7
  %155 = and i32 %154, -8
  %156 = inttoptr i32 %155 to i8*
  %157 = getelementptr inbounds i8, i8* %156, i32 8
  store i8* %157, i8** %151, align 4
  %158 = bitcast i8* %156 to double*
  %159 = load double, double* %158, align 8
  %160 = load i32, i32* %13, align 4
  %161 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %160
  %162 = bitcast %union.jvalue* %161 to double*
  store double %159, double* %162, align 8
  br label %180

163:                                              ; preds = %144
  %164 = load i32, i32* %13, align 4
  %165 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %164
  %166 = load i8, i8* %165, align 1
  %167 = zext i8 %166 to i32
  %168 = icmp eq i32 %167, 76
  br i1 %168, label %169, label %179

169:                                              ; preds = %163
  %170 = bitcast %struct.__va_list* %9 to i8**
  %171 = load i8*, i8** %170, align 4
  %172 = getelementptr inbounds i8, i8* %171, i32 4
  store i8* %172, i8** %170, align 4
  %173 = bitcast i8* %171 to i8**
  %174 = load i8*, i8** %173, align 4
  %175 = bitcast i8* %174 to %struct._jobject*
  %176 = load i32, i32* %13, align 4
  %177 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %176
  %178 = bitcast %union.jvalue* %177 to %struct._jobject**
  store %struct._jobject* %175, %struct._jobject** %178, align 8
  br label %179

179:                                              ; preds = %169, %163
  br label %180

180:                                              ; preds = %179, %150
  br label %181

181:                                              ; preds = %180, %130
  br label %182

182:                                              ; preds = %181, %114
  br label %183

183:                                              ; preds = %182, %99
  br label %184

184:                                              ; preds = %183, %83
  br label %185

185:                                              ; preds = %184, %67
  br label %186

186:                                              ; preds = %185, %51
  br label %187

187:                                              ; preds = %186, %35
  br label %188

188:                                              ; preds = %187
  %189 = load i32, i32* %13, align 4
  %190 = add nsw i32 %189, 1
  store i32 %190, i32* %13, align 4
  br label %25, !llvm.loop !19

191:                                              ; preds = %25
  %192 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %193 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %192, align 4
  %194 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %193, i32 0, i32 69
  %195 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %194, align 4
  %196 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %197 = load %struct._jobject*, %struct._jobject** %6, align 4
  %198 = load %struct._jobject*, %struct._jobject** %7, align 4
  %199 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %200 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %201 = call zeroext i8 %195(%struct.JNINativeInterface_** noundef %196, %struct._jobject* noundef %197, %struct._jobject* noundef %198, %struct._jmethodID* noundef %199, %union.jvalue* noundef %200)
  store i8 %201, i8* %14, align 1
  %202 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %202)
  %203 = load i8, i8* %14, align 1
  ret i8 %203
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i8 @JNI_CallNonvirtualBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
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
  store i32 %25, i32* %13, align 4
  store i32 0, i32* %14, align 4
  br label %26

26:                                               ; preds = %189, %5
  %27 = load i32, i32* %14, align 4
  %28 = load i32, i32* %13, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %192

30:                                               ; preds = %26
  %31 = load i32, i32* %14, align 4
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %31
  %33 = load i8, i8* %32, align 1
  %34 = zext i8 %33 to i32
  %35 = icmp eq i32 %34, 90
  br i1 %35, label %36, label %46

36:                                               ; preds = %30
  %37 = bitcast %struct.__va_list* %6 to i8**
  %38 = load i8*, i8** %37, align 4
  %39 = getelementptr inbounds i8, i8* %38, i32 4
  store i8* %39, i8** %37, align 4
  %40 = bitcast i8* %38 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = load i32, i32* %14, align 4
  %44 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %43
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %42, i8* %45, align 8
  br label %188

46:                                               ; preds = %30
  %47 = load i32, i32* %14, align 4
  %48 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %47
  %49 = load i8, i8* %48, align 1
  %50 = zext i8 %49 to i32
  %51 = icmp eq i32 %50, 66
  br i1 %51, label %52, label %62

52:                                               ; preds = %46
  %53 = bitcast %struct.__va_list* %6 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = load i32, i32* %14, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %58, i8* %61, align 8
  br label %187

62:                                               ; preds = %46
  %63 = load i32, i32* %14, align 4
  %64 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %63
  %65 = load i8, i8* %64, align 1
  %66 = zext i8 %65 to i32
  %67 = icmp eq i32 %66, 67
  br i1 %67, label %68, label %78

68:                                               ; preds = %62
  %69 = bitcast %struct.__va_list* %6 to i8**
  %70 = load i8*, i8** %69, align 4
  %71 = getelementptr inbounds i8, i8* %70, i32 4
  store i8* %71, i8** %69, align 4
  %72 = bitcast i8* %70 to i32*
  %73 = load i32, i32* %72, align 4
  %74 = trunc i32 %73 to i16
  %75 = load i32, i32* %14, align 4
  %76 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %75
  %77 = bitcast %union.jvalue* %76 to i16*
  store i16 %74, i16* %77, align 8
  br label %186

78:                                               ; preds = %62
  %79 = load i32, i32* %14, align 4
  %80 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %79
  %81 = load i8, i8* %80, align 1
  %82 = zext i8 %81 to i32
  %83 = icmp eq i32 %82, 83
  br i1 %83, label %84, label %94

84:                                               ; preds = %78
  %85 = bitcast %struct.__va_list* %6 to i8**
  %86 = load i8*, i8** %85, align 4
  %87 = getelementptr inbounds i8, i8* %86, i32 4
  store i8* %87, i8** %85, align 4
  %88 = bitcast i8* %86 to i32*
  %89 = load i32, i32* %88, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %14, align 4
  %92 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %91
  %93 = bitcast %union.jvalue* %92 to i16*
  store i16 %90, i16* %93, align 8
  br label %185

94:                                               ; preds = %78
  %95 = load i32, i32* %14, align 4
  %96 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %95
  %97 = load i8, i8* %96, align 1
  %98 = zext i8 %97 to i32
  %99 = icmp eq i32 %98, 73
  br i1 %99, label %100, label %109

100:                                              ; preds = %94
  %101 = bitcast %struct.__va_list* %6 to i8**
  %102 = load i8*, i8** %101, align 4
  %103 = getelementptr inbounds i8, i8* %102, i32 4
  store i8* %103, i8** %101, align 4
  %104 = bitcast i8* %102 to i32*
  %105 = load i32, i32* %104, align 4
  %106 = load i32, i32* %14, align 4
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %105, i32* %108, align 8
  br label %184

109:                                              ; preds = %94
  %110 = load i32, i32* %14, align 4
  %111 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %110
  %112 = load i8, i8* %111, align 1
  %113 = zext i8 %112 to i32
  %114 = icmp eq i32 %113, 74
  br i1 %114, label %115, label %125

115:                                              ; preds = %109
  %116 = bitcast %struct.__va_list* %6 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = getelementptr inbounds i8, i8* %117, i32 4
  store i8* %118, i8** %116, align 4
  %119 = bitcast i8* %117 to i32*
  %120 = load i32, i32* %119, align 4
  %121 = sext i32 %120 to i64
  %122 = load i32, i32* %14, align 4
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %121, i64* %124, align 8
  br label %183

125:                                              ; preds = %109
  %126 = load i32, i32* %14, align 4
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %126
  %128 = load i8, i8* %127, align 1
  %129 = zext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %145

131:                                              ; preds = %125
  %132 = bitcast %struct.__va_list* %6 to i8**
  %133 = load i8*, i8** %132, align 4
  %134 = ptrtoint i8* %133 to i32
  %135 = add i32 %134, 7
  %136 = and i32 %135, -8
  %137 = inttoptr i32 %136 to i8*
  %138 = getelementptr inbounds i8, i8* %137, i32 8
  store i8* %138, i8** %132, align 4
  %139 = bitcast i8* %137 to double*
  %140 = load double, double* %139, align 8
  %141 = fptrunc double %140 to float
  %142 = load i32, i32* %14, align 4
  %143 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %142
  %144 = bitcast %union.jvalue* %143 to float*
  store float %141, float* %144, align 8
  br label %182

145:                                              ; preds = %125
  %146 = load i32, i32* %14, align 4
  %147 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %146
  %148 = load i8, i8* %147, align 1
  %149 = zext i8 %148 to i32
  %150 = icmp eq i32 %149, 68
  br i1 %150, label %151, label %164

151:                                              ; preds = %145
  %152 = bitcast %struct.__va_list* %6 to i8**
  %153 = load i8*, i8** %152, align 4
  %154 = ptrtoint i8* %153 to i32
  %155 = add i32 %154, 7
  %156 = and i32 %155, -8
  %157 = inttoptr i32 %156 to i8*
  %158 = getelementptr inbounds i8, i8* %157, i32 8
  store i8* %158, i8** %152, align 4
  %159 = bitcast i8* %157 to double*
  %160 = load double, double* %159, align 8
  %161 = load i32, i32* %14, align 4
  %162 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %161
  %163 = bitcast %union.jvalue* %162 to double*
  store double %160, double* %163, align 8
  br label %181

164:                                              ; preds = %145
  %165 = load i32, i32* %14, align 4
  %166 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %165
  %167 = load i8, i8* %166, align 1
  %168 = zext i8 %167 to i32
  %169 = icmp eq i32 %168, 76
  br i1 %169, label %170, label %180

170:                                              ; preds = %164
  %171 = bitcast %struct.__va_list* %6 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = getelementptr inbounds i8, i8* %172, i32 4
  store i8* %173, i8** %171, align 4
  %174 = bitcast i8* %172 to i8**
  %175 = load i8*, i8** %174, align 4
  %176 = bitcast i8* %175 to %struct._jobject*
  %177 = load i32, i32* %14, align 4
  %178 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %177
  %179 = bitcast %union.jvalue* %178 to %struct._jobject**
  store %struct._jobject* %176, %struct._jobject** %179, align 8
  br label %180

180:                                              ; preds = %170, %164
  br label %181

181:                                              ; preds = %180, %151
  br label %182

182:                                              ; preds = %181, %131
  br label %183

183:                                              ; preds = %182, %115
  br label %184

184:                                              ; preds = %183, %100
  br label %185

185:                                              ; preds = %184, %84
  br label %186

186:                                              ; preds = %185, %68
  br label %187

187:                                              ; preds = %186, %52
  br label %188

188:                                              ; preds = %187, %36
  br label %189

189:                                              ; preds = %188
  %190 = load i32, i32* %14, align 4
  %191 = add nsw i32 %190, 1
  store i32 %191, i32* %14, align 4
  br label %26, !llvm.loop !20

192:                                              ; preds = %26
  %193 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %194 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %193, align 4
  %195 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %194, i32 0, i32 69
  %196 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %195, align 4
  %197 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %198 = load %struct._jobject*, %struct._jobject** %8, align 4
  %199 = load %struct._jobject*, %struct._jobject** %9, align 4
  %200 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %201 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 0
  %202 = call zeroext i8 %196(%struct.JNINativeInterface_** noundef %197, %struct._jobject* noundef %198, %struct._jobject* noundef %199, %struct._jmethodID* noundef %200, %union.jvalue* noundef %201)
  ret i8 %202
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i8 @JNI_CallStaticBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %22, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %186, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %10, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %189

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  %32 = icmp eq i32 %31, 90
  br i1 %32, label %33, label %43

33:                                               ; preds = %27
  %34 = bitcast %struct.__va_list* %7 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %11, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %185

43:                                               ; preds = %27
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  %48 = icmp eq i32 %47, 66
  br i1 %48, label %49, label %59

49:                                               ; preds = %43
  %50 = bitcast %struct.__va_list* %7 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %55, i8* %58, align 8
  br label %184

59:                                               ; preds = %43
  %60 = load i32, i32* %11, align 4
  %61 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %60
  %62 = load i8, i8* %61, align 1
  %63 = zext i8 %62 to i32
  %64 = icmp eq i32 %63, 67
  br i1 %64, label %65, label %75

65:                                               ; preds = %59
  %66 = bitcast %struct.__va_list* %7 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %11, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %183

75:                                               ; preds = %59
  %76 = load i32, i32* %11, align 4
  %77 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %76
  %78 = load i8, i8* %77, align 1
  %79 = zext i8 %78 to i32
  %80 = icmp eq i32 %79, 83
  br i1 %80, label %81, label %91

81:                                               ; preds = %75
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i16*
  store i16 %87, i16* %90, align 8
  br label %182

91:                                               ; preds = %75
  %92 = load i32, i32* %11, align 4
  %93 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %92
  %94 = load i8, i8* %93, align 1
  %95 = zext i8 %94 to i32
  %96 = icmp eq i32 %95, 73
  br i1 %96, label %97, label %106

97:                                               ; preds = %91
  %98 = bitcast %struct.__va_list* %7 to i8**
  %99 = load i8*, i8** %98, align 4
  %100 = getelementptr inbounds i8, i8* %99, i32 4
  store i8* %100, i8** %98, align 4
  %101 = bitcast i8* %99 to i32*
  %102 = load i32, i32* %101, align 4
  %103 = load i32, i32* %11, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %102, i32* %105, align 8
  br label %181

106:                                              ; preds = %91
  %107 = load i32, i32* %11, align 4
  %108 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %107
  %109 = load i8, i8* %108, align 1
  %110 = zext i8 %109 to i32
  %111 = icmp eq i32 %110, 74
  br i1 %111, label %112, label %122

112:                                              ; preds = %106
  %113 = bitcast %struct.__va_list* %7 to i8**
  %114 = load i8*, i8** %113, align 4
  %115 = getelementptr inbounds i8, i8* %114, i32 4
  store i8* %115, i8** %113, align 4
  %116 = bitcast i8* %114 to i32*
  %117 = load i32, i32* %116, align 4
  %118 = sext i32 %117 to i64
  %119 = load i32, i32* %11, align 4
  %120 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %119
  %121 = bitcast %union.jvalue* %120 to i64*
  store i64 %118, i64* %121, align 8
  br label %180

122:                                              ; preds = %106
  %123 = load i32, i32* %11, align 4
  %124 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %123
  %125 = load i8, i8* %124, align 1
  %126 = zext i8 %125 to i32
  %127 = icmp eq i32 %126, 70
  br i1 %127, label %128, label %142

128:                                              ; preds = %122
  %129 = bitcast %struct.__va_list* %7 to i8**
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
  %139 = load i32, i32* %11, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %139
  %141 = bitcast %union.jvalue* %140 to float*
  store float %138, float* %141, align 8
  br label %179

142:                                              ; preds = %122
  %143 = load i32, i32* %11, align 4
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %143
  %145 = load i8, i8* %144, align 1
  %146 = zext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %161

148:                                              ; preds = %142
  %149 = bitcast %struct.__va_list* %7 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = ptrtoint i8* %150 to i32
  %152 = add i32 %151, 7
  %153 = and i32 %152, -8
  %154 = inttoptr i32 %153 to i8*
  %155 = getelementptr inbounds i8, i8* %154, i32 8
  store i8* %155, i8** %149, align 4
  %156 = bitcast i8* %154 to double*
  %157 = load double, double* %156, align 8
  %158 = load i32, i32* %11, align 4
  %159 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %158
  %160 = bitcast %union.jvalue* %159 to double*
  store double %157, double* %160, align 8
  br label %178

161:                                              ; preds = %142
  %162 = load i32, i32* %11, align 4
  %163 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %162
  %164 = load i8, i8* %163, align 1
  %165 = zext i8 %164 to i32
  %166 = icmp eq i32 %165, 76
  br i1 %166, label %167, label %177

167:                                              ; preds = %161
  %168 = bitcast %struct.__va_list* %7 to i8**
  %169 = load i8*, i8** %168, align 4
  %170 = getelementptr inbounds i8, i8* %169, i32 4
  store i8* %170, i8** %168, align 4
  %171 = bitcast i8* %169 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = bitcast i8* %172 to %struct._jobject*
  %174 = load i32, i32* %11, align 4
  %175 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %174
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %176, align 8
  br label %177

177:                                              ; preds = %167, %161
  br label %178

178:                                              ; preds = %177, %148
  br label %179

179:                                              ; preds = %178, %128
  br label %180

180:                                              ; preds = %179, %112
  br label %181

181:                                              ; preds = %180, %97
  br label %182

182:                                              ; preds = %181, %81
  br label %183

183:                                              ; preds = %182, %65
  br label %184

184:                                              ; preds = %183, %49
  br label %185

185:                                              ; preds = %184, %33
  br label %186

186:                                              ; preds = %185
  %187 = load i32, i32* %11, align 4
  %188 = add nsw i32 %187, 1
  store i32 %188, i32* %11, align 4
  br label %23, !llvm.loop !21

189:                                              ; preds = %23
  %190 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %191 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %190, align 4
  %192 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %191, i32 0, i32 119
  %193 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %192, align 4
  %194 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %195 = load %struct._jobject*, %struct._jobject** %5, align 4
  %196 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %197 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  %198 = call zeroext i8 %193(%struct.JNINativeInterface_** noundef %194, %struct._jobject* noundef %195, %struct._jmethodID* noundef %196, %union.jvalue* noundef %197)
  store i8 %198, i8* %12, align 1
  %199 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %199)
  %200 = load i8, i8* %12, align 1
  ret i8 %200
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i8 @JNI_CallStaticBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !22

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 119
  %194 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %199 = call zeroext i8 %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
  ret i8 %199
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i8 @JNI_CallByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %22, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %186, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %10, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %189

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  %32 = icmp eq i32 %31, 90
  br i1 %32, label %33, label %43

33:                                               ; preds = %27
  %34 = bitcast %struct.__va_list* %7 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %11, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %185

43:                                               ; preds = %27
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  %48 = icmp eq i32 %47, 66
  br i1 %48, label %49, label %59

49:                                               ; preds = %43
  %50 = bitcast %struct.__va_list* %7 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %55, i8* %58, align 8
  br label %184

59:                                               ; preds = %43
  %60 = load i32, i32* %11, align 4
  %61 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %60
  %62 = load i8, i8* %61, align 1
  %63 = zext i8 %62 to i32
  %64 = icmp eq i32 %63, 67
  br i1 %64, label %65, label %75

65:                                               ; preds = %59
  %66 = bitcast %struct.__va_list* %7 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %11, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %183

75:                                               ; preds = %59
  %76 = load i32, i32* %11, align 4
  %77 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %76
  %78 = load i8, i8* %77, align 1
  %79 = zext i8 %78 to i32
  %80 = icmp eq i32 %79, 83
  br i1 %80, label %81, label %91

81:                                               ; preds = %75
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i16*
  store i16 %87, i16* %90, align 8
  br label %182

91:                                               ; preds = %75
  %92 = load i32, i32* %11, align 4
  %93 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %92
  %94 = load i8, i8* %93, align 1
  %95 = zext i8 %94 to i32
  %96 = icmp eq i32 %95, 73
  br i1 %96, label %97, label %106

97:                                               ; preds = %91
  %98 = bitcast %struct.__va_list* %7 to i8**
  %99 = load i8*, i8** %98, align 4
  %100 = getelementptr inbounds i8, i8* %99, i32 4
  store i8* %100, i8** %98, align 4
  %101 = bitcast i8* %99 to i32*
  %102 = load i32, i32* %101, align 4
  %103 = load i32, i32* %11, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %102, i32* %105, align 8
  br label %181

106:                                              ; preds = %91
  %107 = load i32, i32* %11, align 4
  %108 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %107
  %109 = load i8, i8* %108, align 1
  %110 = zext i8 %109 to i32
  %111 = icmp eq i32 %110, 74
  br i1 %111, label %112, label %122

112:                                              ; preds = %106
  %113 = bitcast %struct.__va_list* %7 to i8**
  %114 = load i8*, i8** %113, align 4
  %115 = getelementptr inbounds i8, i8* %114, i32 4
  store i8* %115, i8** %113, align 4
  %116 = bitcast i8* %114 to i32*
  %117 = load i32, i32* %116, align 4
  %118 = sext i32 %117 to i64
  %119 = load i32, i32* %11, align 4
  %120 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %119
  %121 = bitcast %union.jvalue* %120 to i64*
  store i64 %118, i64* %121, align 8
  br label %180

122:                                              ; preds = %106
  %123 = load i32, i32* %11, align 4
  %124 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %123
  %125 = load i8, i8* %124, align 1
  %126 = zext i8 %125 to i32
  %127 = icmp eq i32 %126, 70
  br i1 %127, label %128, label %142

128:                                              ; preds = %122
  %129 = bitcast %struct.__va_list* %7 to i8**
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
  %139 = load i32, i32* %11, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %139
  %141 = bitcast %union.jvalue* %140 to float*
  store float %138, float* %141, align 8
  br label %179

142:                                              ; preds = %122
  %143 = load i32, i32* %11, align 4
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %143
  %145 = load i8, i8* %144, align 1
  %146 = zext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %161

148:                                              ; preds = %142
  %149 = bitcast %struct.__va_list* %7 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = ptrtoint i8* %150 to i32
  %152 = add i32 %151, 7
  %153 = and i32 %152, -8
  %154 = inttoptr i32 %153 to i8*
  %155 = getelementptr inbounds i8, i8* %154, i32 8
  store i8* %155, i8** %149, align 4
  %156 = bitcast i8* %154 to double*
  %157 = load double, double* %156, align 8
  %158 = load i32, i32* %11, align 4
  %159 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %158
  %160 = bitcast %union.jvalue* %159 to double*
  store double %157, double* %160, align 8
  br label %178

161:                                              ; preds = %142
  %162 = load i32, i32* %11, align 4
  %163 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %162
  %164 = load i8, i8* %163, align 1
  %165 = zext i8 %164 to i32
  %166 = icmp eq i32 %165, 76
  br i1 %166, label %167, label %177

167:                                              ; preds = %161
  %168 = bitcast %struct.__va_list* %7 to i8**
  %169 = load i8*, i8** %168, align 4
  %170 = getelementptr inbounds i8, i8* %169, i32 4
  store i8* %170, i8** %168, align 4
  %171 = bitcast i8* %169 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = bitcast i8* %172 to %struct._jobject*
  %174 = load i32, i32* %11, align 4
  %175 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %174
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %176, align 8
  br label %177

177:                                              ; preds = %167, %161
  br label %178

178:                                              ; preds = %177, %148
  br label %179

179:                                              ; preds = %178, %128
  br label %180

180:                                              ; preds = %179, %112
  br label %181

181:                                              ; preds = %180, %97
  br label %182

182:                                              ; preds = %181, %81
  br label %183

183:                                              ; preds = %182, %65
  br label %184

184:                                              ; preds = %183, %49
  br label %185

185:                                              ; preds = %184, %33
  br label %186

186:                                              ; preds = %185
  %187 = load i32, i32* %11, align 4
  %188 = add nsw i32 %187, 1
  store i32 %188, i32* %11, align 4
  br label %23, !llvm.loop !23

189:                                              ; preds = %23
  %190 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %191 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %190, align 4
  %192 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %191, i32 0, i32 42
  %193 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %192, align 4
  %194 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %195 = load %struct._jobject*, %struct._jobject** %5, align 4
  %196 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %197 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  %198 = call signext i8 %193(%struct.JNINativeInterface_** noundef %194, %struct._jobject* noundef %195, %struct._jmethodID* noundef %196, %union.jvalue* noundef %197)
  store i8 %198, i8* %12, align 1
  %199 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %199)
  %200 = load i8, i8* %12, align 1
  ret i8 %200
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i8 @JNI_CallByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !24

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 42
  %194 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %199 = call signext i8 %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
  ret i8 %199
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i8 @JNI_CallNonvirtualByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct.__va_list, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
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
  store i32 %24, i32* %12, align 4
  store i32 0, i32* %13, align 4
  br label %25

25:                                               ; preds = %188, %4
  %26 = load i32, i32* %13, align 4
  %27 = load i32, i32* %12, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %191

29:                                               ; preds = %25
  %30 = load i32, i32* %13, align 4
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %30
  %32 = load i8, i8* %31, align 1
  %33 = zext i8 %32 to i32
  %34 = icmp eq i32 %33, 90
  br i1 %34, label %35, label %45

35:                                               ; preds = %29
  %36 = bitcast %struct.__va_list* %9 to i8**
  %37 = load i8*, i8** %36, align 4
  %38 = getelementptr inbounds i8, i8* %37, i32 4
  store i8* %38, i8** %36, align 4
  %39 = bitcast i8* %37 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = load i32, i32* %13, align 4
  %43 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %42
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %41, i8* %44, align 8
  br label %187

45:                                               ; preds = %29
  %46 = load i32, i32* %13, align 4
  %47 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %46
  %48 = load i8, i8* %47, align 1
  %49 = zext i8 %48 to i32
  %50 = icmp eq i32 %49, 66
  br i1 %50, label %51, label %61

51:                                               ; preds = %45
  %52 = bitcast %struct.__va_list* %9 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load i32, i32* %13, align 4
  %59 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %58
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %57, i8* %60, align 8
  br label %186

61:                                               ; preds = %45
  %62 = load i32, i32* %13, align 4
  %63 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %62
  %64 = load i8, i8* %63, align 1
  %65 = zext i8 %64 to i32
  %66 = icmp eq i32 %65, 67
  br i1 %66, label %67, label %77

67:                                               ; preds = %61
  %68 = bitcast %struct.__va_list* %9 to i8**
  %69 = load i8*, i8** %68, align 4
  %70 = getelementptr inbounds i8, i8* %69, i32 4
  store i8* %70, i8** %68, align 4
  %71 = bitcast i8* %69 to i32*
  %72 = load i32, i32* %71, align 4
  %73 = trunc i32 %72 to i16
  %74 = load i32, i32* %13, align 4
  %75 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %74
  %76 = bitcast %union.jvalue* %75 to i16*
  store i16 %73, i16* %76, align 8
  br label %185

77:                                               ; preds = %61
  %78 = load i32, i32* %13, align 4
  %79 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %78
  %80 = load i8, i8* %79, align 1
  %81 = zext i8 %80 to i32
  %82 = icmp eq i32 %81, 83
  br i1 %82, label %83, label %93

83:                                               ; preds = %77
  %84 = bitcast %struct.__va_list* %9 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = trunc i32 %88 to i16
  %90 = load i32, i32* %13, align 4
  %91 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %90
  %92 = bitcast %union.jvalue* %91 to i16*
  store i16 %89, i16* %92, align 8
  br label %184

93:                                               ; preds = %77
  %94 = load i32, i32* %13, align 4
  %95 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %94
  %96 = load i8, i8* %95, align 1
  %97 = zext i8 %96 to i32
  %98 = icmp eq i32 %97, 73
  br i1 %98, label %99, label %108

99:                                               ; preds = %93
  %100 = bitcast %struct.__va_list* %9 to i8**
  %101 = load i8*, i8** %100, align 4
  %102 = getelementptr inbounds i8, i8* %101, i32 4
  store i8* %102, i8** %100, align 4
  %103 = bitcast i8* %101 to i32*
  %104 = load i32, i32* %103, align 4
  %105 = load i32, i32* %13, align 4
  %106 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %105
  %107 = bitcast %union.jvalue* %106 to i32*
  store i32 %104, i32* %107, align 8
  br label %183

108:                                              ; preds = %93
  %109 = load i32, i32* %13, align 4
  %110 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %109
  %111 = load i8, i8* %110, align 1
  %112 = zext i8 %111 to i32
  %113 = icmp eq i32 %112, 74
  br i1 %113, label %114, label %124

114:                                              ; preds = %108
  %115 = bitcast %struct.__va_list* %9 to i8**
  %116 = load i8*, i8** %115, align 4
  %117 = getelementptr inbounds i8, i8* %116, i32 4
  store i8* %117, i8** %115, align 4
  %118 = bitcast i8* %116 to i32*
  %119 = load i32, i32* %118, align 4
  %120 = sext i32 %119 to i64
  %121 = load i32, i32* %13, align 4
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %120, i64* %123, align 8
  br label %182

124:                                              ; preds = %108
  %125 = load i32, i32* %13, align 4
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %125
  %127 = load i8, i8* %126, align 1
  %128 = zext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %144

130:                                              ; preds = %124
  %131 = bitcast %struct.__va_list* %9 to i8**
  %132 = load i8*, i8** %131, align 4
  %133 = ptrtoint i8* %132 to i32
  %134 = add i32 %133, 7
  %135 = and i32 %134, -8
  %136 = inttoptr i32 %135 to i8*
  %137 = getelementptr inbounds i8, i8* %136, i32 8
  store i8* %137, i8** %131, align 4
  %138 = bitcast i8* %136 to double*
  %139 = load double, double* %138, align 8
  %140 = fptrunc double %139 to float
  %141 = load i32, i32* %13, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %140, float* %143, align 8
  br label %181

144:                                              ; preds = %124
  %145 = load i32, i32* %13, align 4
  %146 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %145
  %147 = load i8, i8* %146, align 1
  %148 = zext i8 %147 to i32
  %149 = icmp eq i32 %148, 68
  br i1 %149, label %150, label %163

150:                                              ; preds = %144
  %151 = bitcast %struct.__va_list* %9 to i8**
  %152 = load i8*, i8** %151, align 4
  %153 = ptrtoint i8* %152 to i32
  %154 = add i32 %153, 7
  %155 = and i32 %154, -8
  %156 = inttoptr i32 %155 to i8*
  %157 = getelementptr inbounds i8, i8* %156, i32 8
  store i8* %157, i8** %151, align 4
  %158 = bitcast i8* %156 to double*
  %159 = load double, double* %158, align 8
  %160 = load i32, i32* %13, align 4
  %161 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %160
  %162 = bitcast %union.jvalue* %161 to double*
  store double %159, double* %162, align 8
  br label %180

163:                                              ; preds = %144
  %164 = load i32, i32* %13, align 4
  %165 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %164
  %166 = load i8, i8* %165, align 1
  %167 = zext i8 %166 to i32
  %168 = icmp eq i32 %167, 76
  br i1 %168, label %169, label %179

169:                                              ; preds = %163
  %170 = bitcast %struct.__va_list* %9 to i8**
  %171 = load i8*, i8** %170, align 4
  %172 = getelementptr inbounds i8, i8* %171, i32 4
  store i8* %172, i8** %170, align 4
  %173 = bitcast i8* %171 to i8**
  %174 = load i8*, i8** %173, align 4
  %175 = bitcast i8* %174 to %struct._jobject*
  %176 = load i32, i32* %13, align 4
  %177 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %176
  %178 = bitcast %union.jvalue* %177 to %struct._jobject**
  store %struct._jobject* %175, %struct._jobject** %178, align 8
  br label %179

179:                                              ; preds = %169, %163
  br label %180

180:                                              ; preds = %179, %150
  br label %181

181:                                              ; preds = %180, %130
  br label %182

182:                                              ; preds = %181, %114
  br label %183

183:                                              ; preds = %182, %99
  br label %184

184:                                              ; preds = %183, %83
  br label %185

185:                                              ; preds = %184, %67
  br label %186

186:                                              ; preds = %185, %51
  br label %187

187:                                              ; preds = %186, %35
  br label %188

188:                                              ; preds = %187
  %189 = load i32, i32* %13, align 4
  %190 = add nsw i32 %189, 1
  store i32 %190, i32* %13, align 4
  br label %25, !llvm.loop !25

191:                                              ; preds = %25
  %192 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %193 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %192, align 4
  %194 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %193, i32 0, i32 72
  %195 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %194, align 4
  %196 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %197 = load %struct._jobject*, %struct._jobject** %6, align 4
  %198 = load %struct._jobject*, %struct._jobject** %7, align 4
  %199 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %200 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %201 = call signext i8 %195(%struct.JNINativeInterface_** noundef %196, %struct._jobject* noundef %197, %struct._jobject* noundef %198, %struct._jmethodID* noundef %199, %union.jvalue* noundef %200)
  store i8 %201, i8* %14, align 1
  %202 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %202)
  %203 = load i8, i8* %14, align 1
  ret i8 %203
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i8 @JNI_CallNonvirtualByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
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
  store i32 %25, i32* %13, align 4
  store i32 0, i32* %14, align 4
  br label %26

26:                                               ; preds = %189, %5
  %27 = load i32, i32* %14, align 4
  %28 = load i32, i32* %13, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %192

30:                                               ; preds = %26
  %31 = load i32, i32* %14, align 4
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %31
  %33 = load i8, i8* %32, align 1
  %34 = zext i8 %33 to i32
  %35 = icmp eq i32 %34, 90
  br i1 %35, label %36, label %46

36:                                               ; preds = %30
  %37 = bitcast %struct.__va_list* %6 to i8**
  %38 = load i8*, i8** %37, align 4
  %39 = getelementptr inbounds i8, i8* %38, i32 4
  store i8* %39, i8** %37, align 4
  %40 = bitcast i8* %38 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = load i32, i32* %14, align 4
  %44 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %43
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %42, i8* %45, align 8
  br label %188

46:                                               ; preds = %30
  %47 = load i32, i32* %14, align 4
  %48 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %47
  %49 = load i8, i8* %48, align 1
  %50 = zext i8 %49 to i32
  %51 = icmp eq i32 %50, 66
  br i1 %51, label %52, label %62

52:                                               ; preds = %46
  %53 = bitcast %struct.__va_list* %6 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = load i32, i32* %14, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %58, i8* %61, align 8
  br label %187

62:                                               ; preds = %46
  %63 = load i32, i32* %14, align 4
  %64 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %63
  %65 = load i8, i8* %64, align 1
  %66 = zext i8 %65 to i32
  %67 = icmp eq i32 %66, 67
  br i1 %67, label %68, label %78

68:                                               ; preds = %62
  %69 = bitcast %struct.__va_list* %6 to i8**
  %70 = load i8*, i8** %69, align 4
  %71 = getelementptr inbounds i8, i8* %70, i32 4
  store i8* %71, i8** %69, align 4
  %72 = bitcast i8* %70 to i32*
  %73 = load i32, i32* %72, align 4
  %74 = trunc i32 %73 to i16
  %75 = load i32, i32* %14, align 4
  %76 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %75
  %77 = bitcast %union.jvalue* %76 to i16*
  store i16 %74, i16* %77, align 8
  br label %186

78:                                               ; preds = %62
  %79 = load i32, i32* %14, align 4
  %80 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %79
  %81 = load i8, i8* %80, align 1
  %82 = zext i8 %81 to i32
  %83 = icmp eq i32 %82, 83
  br i1 %83, label %84, label %94

84:                                               ; preds = %78
  %85 = bitcast %struct.__va_list* %6 to i8**
  %86 = load i8*, i8** %85, align 4
  %87 = getelementptr inbounds i8, i8* %86, i32 4
  store i8* %87, i8** %85, align 4
  %88 = bitcast i8* %86 to i32*
  %89 = load i32, i32* %88, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %14, align 4
  %92 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %91
  %93 = bitcast %union.jvalue* %92 to i16*
  store i16 %90, i16* %93, align 8
  br label %185

94:                                               ; preds = %78
  %95 = load i32, i32* %14, align 4
  %96 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %95
  %97 = load i8, i8* %96, align 1
  %98 = zext i8 %97 to i32
  %99 = icmp eq i32 %98, 73
  br i1 %99, label %100, label %109

100:                                              ; preds = %94
  %101 = bitcast %struct.__va_list* %6 to i8**
  %102 = load i8*, i8** %101, align 4
  %103 = getelementptr inbounds i8, i8* %102, i32 4
  store i8* %103, i8** %101, align 4
  %104 = bitcast i8* %102 to i32*
  %105 = load i32, i32* %104, align 4
  %106 = load i32, i32* %14, align 4
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %105, i32* %108, align 8
  br label %184

109:                                              ; preds = %94
  %110 = load i32, i32* %14, align 4
  %111 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %110
  %112 = load i8, i8* %111, align 1
  %113 = zext i8 %112 to i32
  %114 = icmp eq i32 %113, 74
  br i1 %114, label %115, label %125

115:                                              ; preds = %109
  %116 = bitcast %struct.__va_list* %6 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = getelementptr inbounds i8, i8* %117, i32 4
  store i8* %118, i8** %116, align 4
  %119 = bitcast i8* %117 to i32*
  %120 = load i32, i32* %119, align 4
  %121 = sext i32 %120 to i64
  %122 = load i32, i32* %14, align 4
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %121, i64* %124, align 8
  br label %183

125:                                              ; preds = %109
  %126 = load i32, i32* %14, align 4
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %126
  %128 = load i8, i8* %127, align 1
  %129 = zext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %145

131:                                              ; preds = %125
  %132 = bitcast %struct.__va_list* %6 to i8**
  %133 = load i8*, i8** %132, align 4
  %134 = ptrtoint i8* %133 to i32
  %135 = add i32 %134, 7
  %136 = and i32 %135, -8
  %137 = inttoptr i32 %136 to i8*
  %138 = getelementptr inbounds i8, i8* %137, i32 8
  store i8* %138, i8** %132, align 4
  %139 = bitcast i8* %137 to double*
  %140 = load double, double* %139, align 8
  %141 = fptrunc double %140 to float
  %142 = load i32, i32* %14, align 4
  %143 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %142
  %144 = bitcast %union.jvalue* %143 to float*
  store float %141, float* %144, align 8
  br label %182

145:                                              ; preds = %125
  %146 = load i32, i32* %14, align 4
  %147 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %146
  %148 = load i8, i8* %147, align 1
  %149 = zext i8 %148 to i32
  %150 = icmp eq i32 %149, 68
  br i1 %150, label %151, label %164

151:                                              ; preds = %145
  %152 = bitcast %struct.__va_list* %6 to i8**
  %153 = load i8*, i8** %152, align 4
  %154 = ptrtoint i8* %153 to i32
  %155 = add i32 %154, 7
  %156 = and i32 %155, -8
  %157 = inttoptr i32 %156 to i8*
  %158 = getelementptr inbounds i8, i8* %157, i32 8
  store i8* %158, i8** %152, align 4
  %159 = bitcast i8* %157 to double*
  %160 = load double, double* %159, align 8
  %161 = load i32, i32* %14, align 4
  %162 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %161
  %163 = bitcast %union.jvalue* %162 to double*
  store double %160, double* %163, align 8
  br label %181

164:                                              ; preds = %145
  %165 = load i32, i32* %14, align 4
  %166 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %165
  %167 = load i8, i8* %166, align 1
  %168 = zext i8 %167 to i32
  %169 = icmp eq i32 %168, 76
  br i1 %169, label %170, label %180

170:                                              ; preds = %164
  %171 = bitcast %struct.__va_list* %6 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = getelementptr inbounds i8, i8* %172, i32 4
  store i8* %173, i8** %171, align 4
  %174 = bitcast i8* %172 to i8**
  %175 = load i8*, i8** %174, align 4
  %176 = bitcast i8* %175 to %struct._jobject*
  %177 = load i32, i32* %14, align 4
  %178 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %177
  %179 = bitcast %union.jvalue* %178 to %struct._jobject**
  store %struct._jobject* %176, %struct._jobject** %179, align 8
  br label %180

180:                                              ; preds = %170, %164
  br label %181

181:                                              ; preds = %180, %151
  br label %182

182:                                              ; preds = %181, %131
  br label %183

183:                                              ; preds = %182, %115
  br label %184

184:                                              ; preds = %183, %100
  br label %185

185:                                              ; preds = %184, %84
  br label %186

186:                                              ; preds = %185, %68
  br label %187

187:                                              ; preds = %186, %52
  br label %188

188:                                              ; preds = %187, %36
  br label %189

189:                                              ; preds = %188
  %190 = load i32, i32* %14, align 4
  %191 = add nsw i32 %190, 1
  store i32 %191, i32* %14, align 4
  br label %26, !llvm.loop !26

192:                                              ; preds = %26
  %193 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %194 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %193, align 4
  %195 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %194, i32 0, i32 72
  %196 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %195, align 4
  %197 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %198 = load %struct._jobject*, %struct._jobject** %8, align 4
  %199 = load %struct._jobject*, %struct._jobject** %9, align 4
  %200 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %201 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 0
  %202 = call signext i8 %196(%struct.JNINativeInterface_** noundef %197, %struct._jobject* noundef %198, %struct._jobject* noundef %199, %struct._jmethodID* noundef %200, %union.jvalue* noundef %201)
  ret i8 %202
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i8 @JNI_CallStaticByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %22, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %186, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %10, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %189

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  %32 = icmp eq i32 %31, 90
  br i1 %32, label %33, label %43

33:                                               ; preds = %27
  %34 = bitcast %struct.__va_list* %7 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %11, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %185

43:                                               ; preds = %27
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  %48 = icmp eq i32 %47, 66
  br i1 %48, label %49, label %59

49:                                               ; preds = %43
  %50 = bitcast %struct.__va_list* %7 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %55, i8* %58, align 8
  br label %184

59:                                               ; preds = %43
  %60 = load i32, i32* %11, align 4
  %61 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %60
  %62 = load i8, i8* %61, align 1
  %63 = zext i8 %62 to i32
  %64 = icmp eq i32 %63, 67
  br i1 %64, label %65, label %75

65:                                               ; preds = %59
  %66 = bitcast %struct.__va_list* %7 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %11, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %183

75:                                               ; preds = %59
  %76 = load i32, i32* %11, align 4
  %77 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %76
  %78 = load i8, i8* %77, align 1
  %79 = zext i8 %78 to i32
  %80 = icmp eq i32 %79, 83
  br i1 %80, label %81, label %91

81:                                               ; preds = %75
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i16*
  store i16 %87, i16* %90, align 8
  br label %182

91:                                               ; preds = %75
  %92 = load i32, i32* %11, align 4
  %93 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %92
  %94 = load i8, i8* %93, align 1
  %95 = zext i8 %94 to i32
  %96 = icmp eq i32 %95, 73
  br i1 %96, label %97, label %106

97:                                               ; preds = %91
  %98 = bitcast %struct.__va_list* %7 to i8**
  %99 = load i8*, i8** %98, align 4
  %100 = getelementptr inbounds i8, i8* %99, i32 4
  store i8* %100, i8** %98, align 4
  %101 = bitcast i8* %99 to i32*
  %102 = load i32, i32* %101, align 4
  %103 = load i32, i32* %11, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %102, i32* %105, align 8
  br label %181

106:                                              ; preds = %91
  %107 = load i32, i32* %11, align 4
  %108 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %107
  %109 = load i8, i8* %108, align 1
  %110 = zext i8 %109 to i32
  %111 = icmp eq i32 %110, 74
  br i1 %111, label %112, label %122

112:                                              ; preds = %106
  %113 = bitcast %struct.__va_list* %7 to i8**
  %114 = load i8*, i8** %113, align 4
  %115 = getelementptr inbounds i8, i8* %114, i32 4
  store i8* %115, i8** %113, align 4
  %116 = bitcast i8* %114 to i32*
  %117 = load i32, i32* %116, align 4
  %118 = sext i32 %117 to i64
  %119 = load i32, i32* %11, align 4
  %120 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %119
  %121 = bitcast %union.jvalue* %120 to i64*
  store i64 %118, i64* %121, align 8
  br label %180

122:                                              ; preds = %106
  %123 = load i32, i32* %11, align 4
  %124 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %123
  %125 = load i8, i8* %124, align 1
  %126 = zext i8 %125 to i32
  %127 = icmp eq i32 %126, 70
  br i1 %127, label %128, label %142

128:                                              ; preds = %122
  %129 = bitcast %struct.__va_list* %7 to i8**
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
  %139 = load i32, i32* %11, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %139
  %141 = bitcast %union.jvalue* %140 to float*
  store float %138, float* %141, align 8
  br label %179

142:                                              ; preds = %122
  %143 = load i32, i32* %11, align 4
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %143
  %145 = load i8, i8* %144, align 1
  %146 = zext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %161

148:                                              ; preds = %142
  %149 = bitcast %struct.__va_list* %7 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = ptrtoint i8* %150 to i32
  %152 = add i32 %151, 7
  %153 = and i32 %152, -8
  %154 = inttoptr i32 %153 to i8*
  %155 = getelementptr inbounds i8, i8* %154, i32 8
  store i8* %155, i8** %149, align 4
  %156 = bitcast i8* %154 to double*
  %157 = load double, double* %156, align 8
  %158 = load i32, i32* %11, align 4
  %159 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %158
  %160 = bitcast %union.jvalue* %159 to double*
  store double %157, double* %160, align 8
  br label %178

161:                                              ; preds = %142
  %162 = load i32, i32* %11, align 4
  %163 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %162
  %164 = load i8, i8* %163, align 1
  %165 = zext i8 %164 to i32
  %166 = icmp eq i32 %165, 76
  br i1 %166, label %167, label %177

167:                                              ; preds = %161
  %168 = bitcast %struct.__va_list* %7 to i8**
  %169 = load i8*, i8** %168, align 4
  %170 = getelementptr inbounds i8, i8* %169, i32 4
  store i8* %170, i8** %168, align 4
  %171 = bitcast i8* %169 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = bitcast i8* %172 to %struct._jobject*
  %174 = load i32, i32* %11, align 4
  %175 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %174
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %176, align 8
  br label %177

177:                                              ; preds = %167, %161
  br label %178

178:                                              ; preds = %177, %148
  br label %179

179:                                              ; preds = %178, %128
  br label %180

180:                                              ; preds = %179, %112
  br label %181

181:                                              ; preds = %180, %97
  br label %182

182:                                              ; preds = %181, %81
  br label %183

183:                                              ; preds = %182, %65
  br label %184

184:                                              ; preds = %183, %49
  br label %185

185:                                              ; preds = %184, %33
  br label %186

186:                                              ; preds = %185
  %187 = load i32, i32* %11, align 4
  %188 = add nsw i32 %187, 1
  store i32 %188, i32* %11, align 4
  br label %23, !llvm.loop !27

189:                                              ; preds = %23
  %190 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %191 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %190, align 4
  %192 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %191, i32 0, i32 122
  %193 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %192, align 4
  %194 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %195 = load %struct._jobject*, %struct._jobject** %5, align 4
  %196 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %197 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  %198 = call signext i8 %193(%struct.JNINativeInterface_** noundef %194, %struct._jobject* noundef %195, %struct._jmethodID* noundef %196, %union.jvalue* noundef %197)
  store i8 %198, i8* %12, align 1
  %199 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %199)
  %200 = load i8, i8* %12, align 1
  ret i8 %200
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i8 @JNI_CallStaticByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !28

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 122
  %194 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %199 = call signext i8 %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
  ret i8 %199
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i16 @JNI_CallCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %22, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %186, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %10, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %189

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  %32 = icmp eq i32 %31, 90
  br i1 %32, label %33, label %43

33:                                               ; preds = %27
  %34 = bitcast %struct.__va_list* %7 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %11, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %185

43:                                               ; preds = %27
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  %48 = icmp eq i32 %47, 66
  br i1 %48, label %49, label %59

49:                                               ; preds = %43
  %50 = bitcast %struct.__va_list* %7 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %55, i8* %58, align 8
  br label %184

59:                                               ; preds = %43
  %60 = load i32, i32* %11, align 4
  %61 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %60
  %62 = load i8, i8* %61, align 1
  %63 = zext i8 %62 to i32
  %64 = icmp eq i32 %63, 67
  br i1 %64, label %65, label %75

65:                                               ; preds = %59
  %66 = bitcast %struct.__va_list* %7 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %11, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %183

75:                                               ; preds = %59
  %76 = load i32, i32* %11, align 4
  %77 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %76
  %78 = load i8, i8* %77, align 1
  %79 = zext i8 %78 to i32
  %80 = icmp eq i32 %79, 83
  br i1 %80, label %81, label %91

81:                                               ; preds = %75
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i16*
  store i16 %87, i16* %90, align 8
  br label %182

91:                                               ; preds = %75
  %92 = load i32, i32* %11, align 4
  %93 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %92
  %94 = load i8, i8* %93, align 1
  %95 = zext i8 %94 to i32
  %96 = icmp eq i32 %95, 73
  br i1 %96, label %97, label %106

97:                                               ; preds = %91
  %98 = bitcast %struct.__va_list* %7 to i8**
  %99 = load i8*, i8** %98, align 4
  %100 = getelementptr inbounds i8, i8* %99, i32 4
  store i8* %100, i8** %98, align 4
  %101 = bitcast i8* %99 to i32*
  %102 = load i32, i32* %101, align 4
  %103 = load i32, i32* %11, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %102, i32* %105, align 8
  br label %181

106:                                              ; preds = %91
  %107 = load i32, i32* %11, align 4
  %108 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %107
  %109 = load i8, i8* %108, align 1
  %110 = zext i8 %109 to i32
  %111 = icmp eq i32 %110, 74
  br i1 %111, label %112, label %122

112:                                              ; preds = %106
  %113 = bitcast %struct.__va_list* %7 to i8**
  %114 = load i8*, i8** %113, align 4
  %115 = getelementptr inbounds i8, i8* %114, i32 4
  store i8* %115, i8** %113, align 4
  %116 = bitcast i8* %114 to i32*
  %117 = load i32, i32* %116, align 4
  %118 = sext i32 %117 to i64
  %119 = load i32, i32* %11, align 4
  %120 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %119
  %121 = bitcast %union.jvalue* %120 to i64*
  store i64 %118, i64* %121, align 8
  br label %180

122:                                              ; preds = %106
  %123 = load i32, i32* %11, align 4
  %124 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %123
  %125 = load i8, i8* %124, align 1
  %126 = zext i8 %125 to i32
  %127 = icmp eq i32 %126, 70
  br i1 %127, label %128, label %142

128:                                              ; preds = %122
  %129 = bitcast %struct.__va_list* %7 to i8**
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
  %139 = load i32, i32* %11, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %139
  %141 = bitcast %union.jvalue* %140 to float*
  store float %138, float* %141, align 8
  br label %179

142:                                              ; preds = %122
  %143 = load i32, i32* %11, align 4
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %143
  %145 = load i8, i8* %144, align 1
  %146 = zext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %161

148:                                              ; preds = %142
  %149 = bitcast %struct.__va_list* %7 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = ptrtoint i8* %150 to i32
  %152 = add i32 %151, 7
  %153 = and i32 %152, -8
  %154 = inttoptr i32 %153 to i8*
  %155 = getelementptr inbounds i8, i8* %154, i32 8
  store i8* %155, i8** %149, align 4
  %156 = bitcast i8* %154 to double*
  %157 = load double, double* %156, align 8
  %158 = load i32, i32* %11, align 4
  %159 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %158
  %160 = bitcast %union.jvalue* %159 to double*
  store double %157, double* %160, align 8
  br label %178

161:                                              ; preds = %142
  %162 = load i32, i32* %11, align 4
  %163 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %162
  %164 = load i8, i8* %163, align 1
  %165 = zext i8 %164 to i32
  %166 = icmp eq i32 %165, 76
  br i1 %166, label %167, label %177

167:                                              ; preds = %161
  %168 = bitcast %struct.__va_list* %7 to i8**
  %169 = load i8*, i8** %168, align 4
  %170 = getelementptr inbounds i8, i8* %169, i32 4
  store i8* %170, i8** %168, align 4
  %171 = bitcast i8* %169 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = bitcast i8* %172 to %struct._jobject*
  %174 = load i32, i32* %11, align 4
  %175 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %174
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %176, align 8
  br label %177

177:                                              ; preds = %167, %161
  br label %178

178:                                              ; preds = %177, %148
  br label %179

179:                                              ; preds = %178, %128
  br label %180

180:                                              ; preds = %179, %112
  br label %181

181:                                              ; preds = %180, %97
  br label %182

182:                                              ; preds = %181, %81
  br label %183

183:                                              ; preds = %182, %65
  br label %184

184:                                              ; preds = %183, %49
  br label %185

185:                                              ; preds = %184, %33
  br label %186

186:                                              ; preds = %185
  %187 = load i32, i32* %11, align 4
  %188 = add nsw i32 %187, 1
  store i32 %188, i32* %11, align 4
  br label %23, !llvm.loop !29

189:                                              ; preds = %23
  %190 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %191 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %190, align 4
  %192 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %191, i32 0, i32 45
  %193 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %192, align 4
  %194 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %195 = load %struct._jobject*, %struct._jobject** %5, align 4
  %196 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %197 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  %198 = call zeroext i16 %193(%struct.JNINativeInterface_** noundef %194, %struct._jobject* noundef %195, %struct._jmethodID* noundef %196, %union.jvalue* noundef %197)
  store i16 %198, i16* %12, align 2
  %199 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %199)
  %200 = load i16, i16* %12, align 2
  ret i16 %200
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i16 @JNI_CallCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !30

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 45
  %194 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %199 = call zeroext i16 %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
  ret i16 %199
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i16 @JNI_CallNonvirtualCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct.__va_list, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
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
  store i32 %24, i32* %12, align 4
  store i32 0, i32* %13, align 4
  br label %25

25:                                               ; preds = %188, %4
  %26 = load i32, i32* %13, align 4
  %27 = load i32, i32* %12, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %191

29:                                               ; preds = %25
  %30 = load i32, i32* %13, align 4
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %30
  %32 = load i8, i8* %31, align 1
  %33 = zext i8 %32 to i32
  %34 = icmp eq i32 %33, 90
  br i1 %34, label %35, label %45

35:                                               ; preds = %29
  %36 = bitcast %struct.__va_list* %9 to i8**
  %37 = load i8*, i8** %36, align 4
  %38 = getelementptr inbounds i8, i8* %37, i32 4
  store i8* %38, i8** %36, align 4
  %39 = bitcast i8* %37 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = load i32, i32* %13, align 4
  %43 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %42
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %41, i8* %44, align 8
  br label %187

45:                                               ; preds = %29
  %46 = load i32, i32* %13, align 4
  %47 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %46
  %48 = load i8, i8* %47, align 1
  %49 = zext i8 %48 to i32
  %50 = icmp eq i32 %49, 66
  br i1 %50, label %51, label %61

51:                                               ; preds = %45
  %52 = bitcast %struct.__va_list* %9 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load i32, i32* %13, align 4
  %59 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %58
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %57, i8* %60, align 8
  br label %186

61:                                               ; preds = %45
  %62 = load i32, i32* %13, align 4
  %63 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %62
  %64 = load i8, i8* %63, align 1
  %65 = zext i8 %64 to i32
  %66 = icmp eq i32 %65, 67
  br i1 %66, label %67, label %77

67:                                               ; preds = %61
  %68 = bitcast %struct.__va_list* %9 to i8**
  %69 = load i8*, i8** %68, align 4
  %70 = getelementptr inbounds i8, i8* %69, i32 4
  store i8* %70, i8** %68, align 4
  %71 = bitcast i8* %69 to i32*
  %72 = load i32, i32* %71, align 4
  %73 = trunc i32 %72 to i16
  %74 = load i32, i32* %13, align 4
  %75 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %74
  %76 = bitcast %union.jvalue* %75 to i16*
  store i16 %73, i16* %76, align 8
  br label %185

77:                                               ; preds = %61
  %78 = load i32, i32* %13, align 4
  %79 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %78
  %80 = load i8, i8* %79, align 1
  %81 = zext i8 %80 to i32
  %82 = icmp eq i32 %81, 83
  br i1 %82, label %83, label %93

83:                                               ; preds = %77
  %84 = bitcast %struct.__va_list* %9 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = trunc i32 %88 to i16
  %90 = load i32, i32* %13, align 4
  %91 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %90
  %92 = bitcast %union.jvalue* %91 to i16*
  store i16 %89, i16* %92, align 8
  br label %184

93:                                               ; preds = %77
  %94 = load i32, i32* %13, align 4
  %95 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %94
  %96 = load i8, i8* %95, align 1
  %97 = zext i8 %96 to i32
  %98 = icmp eq i32 %97, 73
  br i1 %98, label %99, label %108

99:                                               ; preds = %93
  %100 = bitcast %struct.__va_list* %9 to i8**
  %101 = load i8*, i8** %100, align 4
  %102 = getelementptr inbounds i8, i8* %101, i32 4
  store i8* %102, i8** %100, align 4
  %103 = bitcast i8* %101 to i32*
  %104 = load i32, i32* %103, align 4
  %105 = load i32, i32* %13, align 4
  %106 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %105
  %107 = bitcast %union.jvalue* %106 to i32*
  store i32 %104, i32* %107, align 8
  br label %183

108:                                              ; preds = %93
  %109 = load i32, i32* %13, align 4
  %110 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %109
  %111 = load i8, i8* %110, align 1
  %112 = zext i8 %111 to i32
  %113 = icmp eq i32 %112, 74
  br i1 %113, label %114, label %124

114:                                              ; preds = %108
  %115 = bitcast %struct.__va_list* %9 to i8**
  %116 = load i8*, i8** %115, align 4
  %117 = getelementptr inbounds i8, i8* %116, i32 4
  store i8* %117, i8** %115, align 4
  %118 = bitcast i8* %116 to i32*
  %119 = load i32, i32* %118, align 4
  %120 = sext i32 %119 to i64
  %121 = load i32, i32* %13, align 4
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %120, i64* %123, align 8
  br label %182

124:                                              ; preds = %108
  %125 = load i32, i32* %13, align 4
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %125
  %127 = load i8, i8* %126, align 1
  %128 = zext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %144

130:                                              ; preds = %124
  %131 = bitcast %struct.__va_list* %9 to i8**
  %132 = load i8*, i8** %131, align 4
  %133 = ptrtoint i8* %132 to i32
  %134 = add i32 %133, 7
  %135 = and i32 %134, -8
  %136 = inttoptr i32 %135 to i8*
  %137 = getelementptr inbounds i8, i8* %136, i32 8
  store i8* %137, i8** %131, align 4
  %138 = bitcast i8* %136 to double*
  %139 = load double, double* %138, align 8
  %140 = fptrunc double %139 to float
  %141 = load i32, i32* %13, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %140, float* %143, align 8
  br label %181

144:                                              ; preds = %124
  %145 = load i32, i32* %13, align 4
  %146 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %145
  %147 = load i8, i8* %146, align 1
  %148 = zext i8 %147 to i32
  %149 = icmp eq i32 %148, 68
  br i1 %149, label %150, label %163

150:                                              ; preds = %144
  %151 = bitcast %struct.__va_list* %9 to i8**
  %152 = load i8*, i8** %151, align 4
  %153 = ptrtoint i8* %152 to i32
  %154 = add i32 %153, 7
  %155 = and i32 %154, -8
  %156 = inttoptr i32 %155 to i8*
  %157 = getelementptr inbounds i8, i8* %156, i32 8
  store i8* %157, i8** %151, align 4
  %158 = bitcast i8* %156 to double*
  %159 = load double, double* %158, align 8
  %160 = load i32, i32* %13, align 4
  %161 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %160
  %162 = bitcast %union.jvalue* %161 to double*
  store double %159, double* %162, align 8
  br label %180

163:                                              ; preds = %144
  %164 = load i32, i32* %13, align 4
  %165 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %164
  %166 = load i8, i8* %165, align 1
  %167 = zext i8 %166 to i32
  %168 = icmp eq i32 %167, 76
  br i1 %168, label %169, label %179

169:                                              ; preds = %163
  %170 = bitcast %struct.__va_list* %9 to i8**
  %171 = load i8*, i8** %170, align 4
  %172 = getelementptr inbounds i8, i8* %171, i32 4
  store i8* %172, i8** %170, align 4
  %173 = bitcast i8* %171 to i8**
  %174 = load i8*, i8** %173, align 4
  %175 = bitcast i8* %174 to %struct._jobject*
  %176 = load i32, i32* %13, align 4
  %177 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %176
  %178 = bitcast %union.jvalue* %177 to %struct._jobject**
  store %struct._jobject* %175, %struct._jobject** %178, align 8
  br label %179

179:                                              ; preds = %169, %163
  br label %180

180:                                              ; preds = %179, %150
  br label %181

181:                                              ; preds = %180, %130
  br label %182

182:                                              ; preds = %181, %114
  br label %183

183:                                              ; preds = %182, %99
  br label %184

184:                                              ; preds = %183, %83
  br label %185

185:                                              ; preds = %184, %67
  br label %186

186:                                              ; preds = %185, %51
  br label %187

187:                                              ; preds = %186, %35
  br label %188

188:                                              ; preds = %187
  %189 = load i32, i32* %13, align 4
  %190 = add nsw i32 %189, 1
  store i32 %190, i32* %13, align 4
  br label %25, !llvm.loop !31

191:                                              ; preds = %25
  %192 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %193 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %192, align 4
  %194 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %193, i32 0, i32 75
  %195 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %194, align 4
  %196 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %197 = load %struct._jobject*, %struct._jobject** %6, align 4
  %198 = load %struct._jobject*, %struct._jobject** %7, align 4
  %199 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %200 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %201 = call zeroext i16 %195(%struct.JNINativeInterface_** noundef %196, %struct._jobject* noundef %197, %struct._jobject* noundef %198, %struct._jmethodID* noundef %199, %union.jvalue* noundef %200)
  store i16 %201, i16* %14, align 2
  %202 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %202)
  %203 = load i16, i16* %14, align 2
  ret i16 %203
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i16 @JNI_CallNonvirtualCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
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
  store i32 %25, i32* %13, align 4
  store i32 0, i32* %14, align 4
  br label %26

26:                                               ; preds = %189, %5
  %27 = load i32, i32* %14, align 4
  %28 = load i32, i32* %13, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %192

30:                                               ; preds = %26
  %31 = load i32, i32* %14, align 4
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %31
  %33 = load i8, i8* %32, align 1
  %34 = zext i8 %33 to i32
  %35 = icmp eq i32 %34, 90
  br i1 %35, label %36, label %46

36:                                               ; preds = %30
  %37 = bitcast %struct.__va_list* %6 to i8**
  %38 = load i8*, i8** %37, align 4
  %39 = getelementptr inbounds i8, i8* %38, i32 4
  store i8* %39, i8** %37, align 4
  %40 = bitcast i8* %38 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = load i32, i32* %14, align 4
  %44 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %43
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %42, i8* %45, align 8
  br label %188

46:                                               ; preds = %30
  %47 = load i32, i32* %14, align 4
  %48 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %47
  %49 = load i8, i8* %48, align 1
  %50 = zext i8 %49 to i32
  %51 = icmp eq i32 %50, 66
  br i1 %51, label %52, label %62

52:                                               ; preds = %46
  %53 = bitcast %struct.__va_list* %6 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = load i32, i32* %14, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %58, i8* %61, align 8
  br label %187

62:                                               ; preds = %46
  %63 = load i32, i32* %14, align 4
  %64 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %63
  %65 = load i8, i8* %64, align 1
  %66 = zext i8 %65 to i32
  %67 = icmp eq i32 %66, 67
  br i1 %67, label %68, label %78

68:                                               ; preds = %62
  %69 = bitcast %struct.__va_list* %6 to i8**
  %70 = load i8*, i8** %69, align 4
  %71 = getelementptr inbounds i8, i8* %70, i32 4
  store i8* %71, i8** %69, align 4
  %72 = bitcast i8* %70 to i32*
  %73 = load i32, i32* %72, align 4
  %74 = trunc i32 %73 to i16
  %75 = load i32, i32* %14, align 4
  %76 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %75
  %77 = bitcast %union.jvalue* %76 to i16*
  store i16 %74, i16* %77, align 8
  br label %186

78:                                               ; preds = %62
  %79 = load i32, i32* %14, align 4
  %80 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %79
  %81 = load i8, i8* %80, align 1
  %82 = zext i8 %81 to i32
  %83 = icmp eq i32 %82, 83
  br i1 %83, label %84, label %94

84:                                               ; preds = %78
  %85 = bitcast %struct.__va_list* %6 to i8**
  %86 = load i8*, i8** %85, align 4
  %87 = getelementptr inbounds i8, i8* %86, i32 4
  store i8* %87, i8** %85, align 4
  %88 = bitcast i8* %86 to i32*
  %89 = load i32, i32* %88, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %14, align 4
  %92 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %91
  %93 = bitcast %union.jvalue* %92 to i16*
  store i16 %90, i16* %93, align 8
  br label %185

94:                                               ; preds = %78
  %95 = load i32, i32* %14, align 4
  %96 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %95
  %97 = load i8, i8* %96, align 1
  %98 = zext i8 %97 to i32
  %99 = icmp eq i32 %98, 73
  br i1 %99, label %100, label %109

100:                                              ; preds = %94
  %101 = bitcast %struct.__va_list* %6 to i8**
  %102 = load i8*, i8** %101, align 4
  %103 = getelementptr inbounds i8, i8* %102, i32 4
  store i8* %103, i8** %101, align 4
  %104 = bitcast i8* %102 to i32*
  %105 = load i32, i32* %104, align 4
  %106 = load i32, i32* %14, align 4
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %105, i32* %108, align 8
  br label %184

109:                                              ; preds = %94
  %110 = load i32, i32* %14, align 4
  %111 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %110
  %112 = load i8, i8* %111, align 1
  %113 = zext i8 %112 to i32
  %114 = icmp eq i32 %113, 74
  br i1 %114, label %115, label %125

115:                                              ; preds = %109
  %116 = bitcast %struct.__va_list* %6 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = getelementptr inbounds i8, i8* %117, i32 4
  store i8* %118, i8** %116, align 4
  %119 = bitcast i8* %117 to i32*
  %120 = load i32, i32* %119, align 4
  %121 = sext i32 %120 to i64
  %122 = load i32, i32* %14, align 4
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %121, i64* %124, align 8
  br label %183

125:                                              ; preds = %109
  %126 = load i32, i32* %14, align 4
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %126
  %128 = load i8, i8* %127, align 1
  %129 = zext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %145

131:                                              ; preds = %125
  %132 = bitcast %struct.__va_list* %6 to i8**
  %133 = load i8*, i8** %132, align 4
  %134 = ptrtoint i8* %133 to i32
  %135 = add i32 %134, 7
  %136 = and i32 %135, -8
  %137 = inttoptr i32 %136 to i8*
  %138 = getelementptr inbounds i8, i8* %137, i32 8
  store i8* %138, i8** %132, align 4
  %139 = bitcast i8* %137 to double*
  %140 = load double, double* %139, align 8
  %141 = fptrunc double %140 to float
  %142 = load i32, i32* %14, align 4
  %143 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %142
  %144 = bitcast %union.jvalue* %143 to float*
  store float %141, float* %144, align 8
  br label %182

145:                                              ; preds = %125
  %146 = load i32, i32* %14, align 4
  %147 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %146
  %148 = load i8, i8* %147, align 1
  %149 = zext i8 %148 to i32
  %150 = icmp eq i32 %149, 68
  br i1 %150, label %151, label %164

151:                                              ; preds = %145
  %152 = bitcast %struct.__va_list* %6 to i8**
  %153 = load i8*, i8** %152, align 4
  %154 = ptrtoint i8* %153 to i32
  %155 = add i32 %154, 7
  %156 = and i32 %155, -8
  %157 = inttoptr i32 %156 to i8*
  %158 = getelementptr inbounds i8, i8* %157, i32 8
  store i8* %158, i8** %152, align 4
  %159 = bitcast i8* %157 to double*
  %160 = load double, double* %159, align 8
  %161 = load i32, i32* %14, align 4
  %162 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %161
  %163 = bitcast %union.jvalue* %162 to double*
  store double %160, double* %163, align 8
  br label %181

164:                                              ; preds = %145
  %165 = load i32, i32* %14, align 4
  %166 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %165
  %167 = load i8, i8* %166, align 1
  %168 = zext i8 %167 to i32
  %169 = icmp eq i32 %168, 76
  br i1 %169, label %170, label %180

170:                                              ; preds = %164
  %171 = bitcast %struct.__va_list* %6 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = getelementptr inbounds i8, i8* %172, i32 4
  store i8* %173, i8** %171, align 4
  %174 = bitcast i8* %172 to i8**
  %175 = load i8*, i8** %174, align 4
  %176 = bitcast i8* %175 to %struct._jobject*
  %177 = load i32, i32* %14, align 4
  %178 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %177
  %179 = bitcast %union.jvalue* %178 to %struct._jobject**
  store %struct._jobject* %176, %struct._jobject** %179, align 8
  br label %180

180:                                              ; preds = %170, %164
  br label %181

181:                                              ; preds = %180, %151
  br label %182

182:                                              ; preds = %181, %131
  br label %183

183:                                              ; preds = %182, %115
  br label %184

184:                                              ; preds = %183, %100
  br label %185

185:                                              ; preds = %184, %84
  br label %186

186:                                              ; preds = %185, %68
  br label %187

187:                                              ; preds = %186, %52
  br label %188

188:                                              ; preds = %187, %36
  br label %189

189:                                              ; preds = %188
  %190 = load i32, i32* %14, align 4
  %191 = add nsw i32 %190, 1
  store i32 %191, i32* %14, align 4
  br label %26, !llvm.loop !32

192:                                              ; preds = %26
  %193 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %194 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %193, align 4
  %195 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %194, i32 0, i32 75
  %196 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %195, align 4
  %197 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %198 = load %struct._jobject*, %struct._jobject** %8, align 4
  %199 = load %struct._jobject*, %struct._jobject** %9, align 4
  %200 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %201 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 0
  %202 = call zeroext i16 %196(%struct.JNINativeInterface_** noundef %197, %struct._jobject* noundef %198, %struct._jobject* noundef %199, %struct._jmethodID* noundef %200, %union.jvalue* noundef %201)
  ret i16 %202
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i16 @JNI_CallStaticCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %22, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %186, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %10, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %189

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  %32 = icmp eq i32 %31, 90
  br i1 %32, label %33, label %43

33:                                               ; preds = %27
  %34 = bitcast %struct.__va_list* %7 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %11, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %185

43:                                               ; preds = %27
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  %48 = icmp eq i32 %47, 66
  br i1 %48, label %49, label %59

49:                                               ; preds = %43
  %50 = bitcast %struct.__va_list* %7 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %55, i8* %58, align 8
  br label %184

59:                                               ; preds = %43
  %60 = load i32, i32* %11, align 4
  %61 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %60
  %62 = load i8, i8* %61, align 1
  %63 = zext i8 %62 to i32
  %64 = icmp eq i32 %63, 67
  br i1 %64, label %65, label %75

65:                                               ; preds = %59
  %66 = bitcast %struct.__va_list* %7 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %11, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %183

75:                                               ; preds = %59
  %76 = load i32, i32* %11, align 4
  %77 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %76
  %78 = load i8, i8* %77, align 1
  %79 = zext i8 %78 to i32
  %80 = icmp eq i32 %79, 83
  br i1 %80, label %81, label %91

81:                                               ; preds = %75
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i16*
  store i16 %87, i16* %90, align 8
  br label %182

91:                                               ; preds = %75
  %92 = load i32, i32* %11, align 4
  %93 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %92
  %94 = load i8, i8* %93, align 1
  %95 = zext i8 %94 to i32
  %96 = icmp eq i32 %95, 73
  br i1 %96, label %97, label %106

97:                                               ; preds = %91
  %98 = bitcast %struct.__va_list* %7 to i8**
  %99 = load i8*, i8** %98, align 4
  %100 = getelementptr inbounds i8, i8* %99, i32 4
  store i8* %100, i8** %98, align 4
  %101 = bitcast i8* %99 to i32*
  %102 = load i32, i32* %101, align 4
  %103 = load i32, i32* %11, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %102, i32* %105, align 8
  br label %181

106:                                              ; preds = %91
  %107 = load i32, i32* %11, align 4
  %108 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %107
  %109 = load i8, i8* %108, align 1
  %110 = zext i8 %109 to i32
  %111 = icmp eq i32 %110, 74
  br i1 %111, label %112, label %122

112:                                              ; preds = %106
  %113 = bitcast %struct.__va_list* %7 to i8**
  %114 = load i8*, i8** %113, align 4
  %115 = getelementptr inbounds i8, i8* %114, i32 4
  store i8* %115, i8** %113, align 4
  %116 = bitcast i8* %114 to i32*
  %117 = load i32, i32* %116, align 4
  %118 = sext i32 %117 to i64
  %119 = load i32, i32* %11, align 4
  %120 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %119
  %121 = bitcast %union.jvalue* %120 to i64*
  store i64 %118, i64* %121, align 8
  br label %180

122:                                              ; preds = %106
  %123 = load i32, i32* %11, align 4
  %124 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %123
  %125 = load i8, i8* %124, align 1
  %126 = zext i8 %125 to i32
  %127 = icmp eq i32 %126, 70
  br i1 %127, label %128, label %142

128:                                              ; preds = %122
  %129 = bitcast %struct.__va_list* %7 to i8**
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
  %139 = load i32, i32* %11, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %139
  %141 = bitcast %union.jvalue* %140 to float*
  store float %138, float* %141, align 8
  br label %179

142:                                              ; preds = %122
  %143 = load i32, i32* %11, align 4
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %143
  %145 = load i8, i8* %144, align 1
  %146 = zext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %161

148:                                              ; preds = %142
  %149 = bitcast %struct.__va_list* %7 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = ptrtoint i8* %150 to i32
  %152 = add i32 %151, 7
  %153 = and i32 %152, -8
  %154 = inttoptr i32 %153 to i8*
  %155 = getelementptr inbounds i8, i8* %154, i32 8
  store i8* %155, i8** %149, align 4
  %156 = bitcast i8* %154 to double*
  %157 = load double, double* %156, align 8
  %158 = load i32, i32* %11, align 4
  %159 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %158
  %160 = bitcast %union.jvalue* %159 to double*
  store double %157, double* %160, align 8
  br label %178

161:                                              ; preds = %142
  %162 = load i32, i32* %11, align 4
  %163 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %162
  %164 = load i8, i8* %163, align 1
  %165 = zext i8 %164 to i32
  %166 = icmp eq i32 %165, 76
  br i1 %166, label %167, label %177

167:                                              ; preds = %161
  %168 = bitcast %struct.__va_list* %7 to i8**
  %169 = load i8*, i8** %168, align 4
  %170 = getelementptr inbounds i8, i8* %169, i32 4
  store i8* %170, i8** %168, align 4
  %171 = bitcast i8* %169 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = bitcast i8* %172 to %struct._jobject*
  %174 = load i32, i32* %11, align 4
  %175 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %174
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %176, align 8
  br label %177

177:                                              ; preds = %167, %161
  br label %178

178:                                              ; preds = %177, %148
  br label %179

179:                                              ; preds = %178, %128
  br label %180

180:                                              ; preds = %179, %112
  br label %181

181:                                              ; preds = %180, %97
  br label %182

182:                                              ; preds = %181, %81
  br label %183

183:                                              ; preds = %182, %65
  br label %184

184:                                              ; preds = %183, %49
  br label %185

185:                                              ; preds = %184, %33
  br label %186

186:                                              ; preds = %185
  %187 = load i32, i32* %11, align 4
  %188 = add nsw i32 %187, 1
  store i32 %188, i32* %11, align 4
  br label %23, !llvm.loop !33

189:                                              ; preds = %23
  %190 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %191 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %190, align 4
  %192 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %191, i32 0, i32 125
  %193 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %192, align 4
  %194 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %195 = load %struct._jobject*, %struct._jobject** %5, align 4
  %196 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %197 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  %198 = call zeroext i16 %193(%struct.JNINativeInterface_** noundef %194, %struct._jobject* noundef %195, %struct._jmethodID* noundef %196, %union.jvalue* noundef %197)
  store i16 %198, i16* %12, align 2
  %199 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %199)
  %200 = load i16, i16* %12, align 2
  ret i16 %200
}

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i16 @JNI_CallStaticCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !34

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 125
  %194 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %199 = call zeroext i16 %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
  ret i16 %199
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i16 @JNI_CallShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %22, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %186, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %10, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %189

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  %32 = icmp eq i32 %31, 90
  br i1 %32, label %33, label %43

33:                                               ; preds = %27
  %34 = bitcast %struct.__va_list* %7 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %11, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %185

43:                                               ; preds = %27
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  %48 = icmp eq i32 %47, 66
  br i1 %48, label %49, label %59

49:                                               ; preds = %43
  %50 = bitcast %struct.__va_list* %7 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %55, i8* %58, align 8
  br label %184

59:                                               ; preds = %43
  %60 = load i32, i32* %11, align 4
  %61 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %60
  %62 = load i8, i8* %61, align 1
  %63 = zext i8 %62 to i32
  %64 = icmp eq i32 %63, 67
  br i1 %64, label %65, label %75

65:                                               ; preds = %59
  %66 = bitcast %struct.__va_list* %7 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %11, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %183

75:                                               ; preds = %59
  %76 = load i32, i32* %11, align 4
  %77 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %76
  %78 = load i8, i8* %77, align 1
  %79 = zext i8 %78 to i32
  %80 = icmp eq i32 %79, 83
  br i1 %80, label %81, label %91

81:                                               ; preds = %75
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i16*
  store i16 %87, i16* %90, align 8
  br label %182

91:                                               ; preds = %75
  %92 = load i32, i32* %11, align 4
  %93 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %92
  %94 = load i8, i8* %93, align 1
  %95 = zext i8 %94 to i32
  %96 = icmp eq i32 %95, 73
  br i1 %96, label %97, label %106

97:                                               ; preds = %91
  %98 = bitcast %struct.__va_list* %7 to i8**
  %99 = load i8*, i8** %98, align 4
  %100 = getelementptr inbounds i8, i8* %99, i32 4
  store i8* %100, i8** %98, align 4
  %101 = bitcast i8* %99 to i32*
  %102 = load i32, i32* %101, align 4
  %103 = load i32, i32* %11, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %102, i32* %105, align 8
  br label %181

106:                                              ; preds = %91
  %107 = load i32, i32* %11, align 4
  %108 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %107
  %109 = load i8, i8* %108, align 1
  %110 = zext i8 %109 to i32
  %111 = icmp eq i32 %110, 74
  br i1 %111, label %112, label %122

112:                                              ; preds = %106
  %113 = bitcast %struct.__va_list* %7 to i8**
  %114 = load i8*, i8** %113, align 4
  %115 = getelementptr inbounds i8, i8* %114, i32 4
  store i8* %115, i8** %113, align 4
  %116 = bitcast i8* %114 to i32*
  %117 = load i32, i32* %116, align 4
  %118 = sext i32 %117 to i64
  %119 = load i32, i32* %11, align 4
  %120 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %119
  %121 = bitcast %union.jvalue* %120 to i64*
  store i64 %118, i64* %121, align 8
  br label %180

122:                                              ; preds = %106
  %123 = load i32, i32* %11, align 4
  %124 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %123
  %125 = load i8, i8* %124, align 1
  %126 = zext i8 %125 to i32
  %127 = icmp eq i32 %126, 70
  br i1 %127, label %128, label %142

128:                                              ; preds = %122
  %129 = bitcast %struct.__va_list* %7 to i8**
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
  %139 = load i32, i32* %11, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %139
  %141 = bitcast %union.jvalue* %140 to float*
  store float %138, float* %141, align 8
  br label %179

142:                                              ; preds = %122
  %143 = load i32, i32* %11, align 4
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %143
  %145 = load i8, i8* %144, align 1
  %146 = zext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %161

148:                                              ; preds = %142
  %149 = bitcast %struct.__va_list* %7 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = ptrtoint i8* %150 to i32
  %152 = add i32 %151, 7
  %153 = and i32 %152, -8
  %154 = inttoptr i32 %153 to i8*
  %155 = getelementptr inbounds i8, i8* %154, i32 8
  store i8* %155, i8** %149, align 4
  %156 = bitcast i8* %154 to double*
  %157 = load double, double* %156, align 8
  %158 = load i32, i32* %11, align 4
  %159 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %158
  %160 = bitcast %union.jvalue* %159 to double*
  store double %157, double* %160, align 8
  br label %178

161:                                              ; preds = %142
  %162 = load i32, i32* %11, align 4
  %163 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %162
  %164 = load i8, i8* %163, align 1
  %165 = zext i8 %164 to i32
  %166 = icmp eq i32 %165, 76
  br i1 %166, label %167, label %177

167:                                              ; preds = %161
  %168 = bitcast %struct.__va_list* %7 to i8**
  %169 = load i8*, i8** %168, align 4
  %170 = getelementptr inbounds i8, i8* %169, i32 4
  store i8* %170, i8** %168, align 4
  %171 = bitcast i8* %169 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = bitcast i8* %172 to %struct._jobject*
  %174 = load i32, i32* %11, align 4
  %175 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %174
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %176, align 8
  br label %177

177:                                              ; preds = %167, %161
  br label %178

178:                                              ; preds = %177, %148
  br label %179

179:                                              ; preds = %178, %128
  br label %180

180:                                              ; preds = %179, %112
  br label %181

181:                                              ; preds = %180, %97
  br label %182

182:                                              ; preds = %181, %81
  br label %183

183:                                              ; preds = %182, %65
  br label %184

184:                                              ; preds = %183, %49
  br label %185

185:                                              ; preds = %184, %33
  br label %186

186:                                              ; preds = %185
  %187 = load i32, i32* %11, align 4
  %188 = add nsw i32 %187, 1
  store i32 %188, i32* %11, align 4
  br label %23, !llvm.loop !35

189:                                              ; preds = %23
  %190 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %191 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %190, align 4
  %192 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %191, i32 0, i32 48
  %193 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %192, align 4
  %194 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %195 = load %struct._jobject*, %struct._jobject** %5, align 4
  %196 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %197 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  %198 = call signext i16 %193(%struct.JNINativeInterface_** noundef %194, %struct._jobject* noundef %195, %struct._jmethodID* noundef %196, %union.jvalue* noundef %197)
  store i16 %198, i16* %12, align 2
  %199 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %199)
  %200 = load i16, i16* %12, align 2
  ret i16 %200
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i16 @JNI_CallShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !36

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 48
  %194 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %199 = call signext i16 %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
  ret i16 %199
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i16 @JNI_CallNonvirtualShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct.__va_list, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
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
  store i32 %24, i32* %12, align 4
  store i32 0, i32* %13, align 4
  br label %25

25:                                               ; preds = %188, %4
  %26 = load i32, i32* %13, align 4
  %27 = load i32, i32* %12, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %191

29:                                               ; preds = %25
  %30 = load i32, i32* %13, align 4
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %30
  %32 = load i8, i8* %31, align 1
  %33 = zext i8 %32 to i32
  %34 = icmp eq i32 %33, 90
  br i1 %34, label %35, label %45

35:                                               ; preds = %29
  %36 = bitcast %struct.__va_list* %9 to i8**
  %37 = load i8*, i8** %36, align 4
  %38 = getelementptr inbounds i8, i8* %37, i32 4
  store i8* %38, i8** %36, align 4
  %39 = bitcast i8* %37 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = load i32, i32* %13, align 4
  %43 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %42
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %41, i8* %44, align 8
  br label %187

45:                                               ; preds = %29
  %46 = load i32, i32* %13, align 4
  %47 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %46
  %48 = load i8, i8* %47, align 1
  %49 = zext i8 %48 to i32
  %50 = icmp eq i32 %49, 66
  br i1 %50, label %51, label %61

51:                                               ; preds = %45
  %52 = bitcast %struct.__va_list* %9 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load i32, i32* %13, align 4
  %59 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %58
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %57, i8* %60, align 8
  br label %186

61:                                               ; preds = %45
  %62 = load i32, i32* %13, align 4
  %63 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %62
  %64 = load i8, i8* %63, align 1
  %65 = zext i8 %64 to i32
  %66 = icmp eq i32 %65, 67
  br i1 %66, label %67, label %77

67:                                               ; preds = %61
  %68 = bitcast %struct.__va_list* %9 to i8**
  %69 = load i8*, i8** %68, align 4
  %70 = getelementptr inbounds i8, i8* %69, i32 4
  store i8* %70, i8** %68, align 4
  %71 = bitcast i8* %69 to i32*
  %72 = load i32, i32* %71, align 4
  %73 = trunc i32 %72 to i16
  %74 = load i32, i32* %13, align 4
  %75 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %74
  %76 = bitcast %union.jvalue* %75 to i16*
  store i16 %73, i16* %76, align 8
  br label %185

77:                                               ; preds = %61
  %78 = load i32, i32* %13, align 4
  %79 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %78
  %80 = load i8, i8* %79, align 1
  %81 = zext i8 %80 to i32
  %82 = icmp eq i32 %81, 83
  br i1 %82, label %83, label %93

83:                                               ; preds = %77
  %84 = bitcast %struct.__va_list* %9 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = trunc i32 %88 to i16
  %90 = load i32, i32* %13, align 4
  %91 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %90
  %92 = bitcast %union.jvalue* %91 to i16*
  store i16 %89, i16* %92, align 8
  br label %184

93:                                               ; preds = %77
  %94 = load i32, i32* %13, align 4
  %95 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %94
  %96 = load i8, i8* %95, align 1
  %97 = zext i8 %96 to i32
  %98 = icmp eq i32 %97, 73
  br i1 %98, label %99, label %108

99:                                               ; preds = %93
  %100 = bitcast %struct.__va_list* %9 to i8**
  %101 = load i8*, i8** %100, align 4
  %102 = getelementptr inbounds i8, i8* %101, i32 4
  store i8* %102, i8** %100, align 4
  %103 = bitcast i8* %101 to i32*
  %104 = load i32, i32* %103, align 4
  %105 = load i32, i32* %13, align 4
  %106 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %105
  %107 = bitcast %union.jvalue* %106 to i32*
  store i32 %104, i32* %107, align 8
  br label %183

108:                                              ; preds = %93
  %109 = load i32, i32* %13, align 4
  %110 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %109
  %111 = load i8, i8* %110, align 1
  %112 = zext i8 %111 to i32
  %113 = icmp eq i32 %112, 74
  br i1 %113, label %114, label %124

114:                                              ; preds = %108
  %115 = bitcast %struct.__va_list* %9 to i8**
  %116 = load i8*, i8** %115, align 4
  %117 = getelementptr inbounds i8, i8* %116, i32 4
  store i8* %117, i8** %115, align 4
  %118 = bitcast i8* %116 to i32*
  %119 = load i32, i32* %118, align 4
  %120 = sext i32 %119 to i64
  %121 = load i32, i32* %13, align 4
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %120, i64* %123, align 8
  br label %182

124:                                              ; preds = %108
  %125 = load i32, i32* %13, align 4
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %125
  %127 = load i8, i8* %126, align 1
  %128 = zext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %144

130:                                              ; preds = %124
  %131 = bitcast %struct.__va_list* %9 to i8**
  %132 = load i8*, i8** %131, align 4
  %133 = ptrtoint i8* %132 to i32
  %134 = add i32 %133, 7
  %135 = and i32 %134, -8
  %136 = inttoptr i32 %135 to i8*
  %137 = getelementptr inbounds i8, i8* %136, i32 8
  store i8* %137, i8** %131, align 4
  %138 = bitcast i8* %136 to double*
  %139 = load double, double* %138, align 8
  %140 = fptrunc double %139 to float
  %141 = load i32, i32* %13, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %140, float* %143, align 8
  br label %181

144:                                              ; preds = %124
  %145 = load i32, i32* %13, align 4
  %146 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %145
  %147 = load i8, i8* %146, align 1
  %148 = zext i8 %147 to i32
  %149 = icmp eq i32 %148, 68
  br i1 %149, label %150, label %163

150:                                              ; preds = %144
  %151 = bitcast %struct.__va_list* %9 to i8**
  %152 = load i8*, i8** %151, align 4
  %153 = ptrtoint i8* %152 to i32
  %154 = add i32 %153, 7
  %155 = and i32 %154, -8
  %156 = inttoptr i32 %155 to i8*
  %157 = getelementptr inbounds i8, i8* %156, i32 8
  store i8* %157, i8** %151, align 4
  %158 = bitcast i8* %156 to double*
  %159 = load double, double* %158, align 8
  %160 = load i32, i32* %13, align 4
  %161 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %160
  %162 = bitcast %union.jvalue* %161 to double*
  store double %159, double* %162, align 8
  br label %180

163:                                              ; preds = %144
  %164 = load i32, i32* %13, align 4
  %165 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %164
  %166 = load i8, i8* %165, align 1
  %167 = zext i8 %166 to i32
  %168 = icmp eq i32 %167, 76
  br i1 %168, label %169, label %179

169:                                              ; preds = %163
  %170 = bitcast %struct.__va_list* %9 to i8**
  %171 = load i8*, i8** %170, align 4
  %172 = getelementptr inbounds i8, i8* %171, i32 4
  store i8* %172, i8** %170, align 4
  %173 = bitcast i8* %171 to i8**
  %174 = load i8*, i8** %173, align 4
  %175 = bitcast i8* %174 to %struct._jobject*
  %176 = load i32, i32* %13, align 4
  %177 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %176
  %178 = bitcast %union.jvalue* %177 to %struct._jobject**
  store %struct._jobject* %175, %struct._jobject** %178, align 8
  br label %179

179:                                              ; preds = %169, %163
  br label %180

180:                                              ; preds = %179, %150
  br label %181

181:                                              ; preds = %180, %130
  br label %182

182:                                              ; preds = %181, %114
  br label %183

183:                                              ; preds = %182, %99
  br label %184

184:                                              ; preds = %183, %83
  br label %185

185:                                              ; preds = %184, %67
  br label %186

186:                                              ; preds = %185, %51
  br label %187

187:                                              ; preds = %186, %35
  br label %188

188:                                              ; preds = %187
  %189 = load i32, i32* %13, align 4
  %190 = add nsw i32 %189, 1
  store i32 %190, i32* %13, align 4
  br label %25, !llvm.loop !37

191:                                              ; preds = %25
  %192 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %193 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %192, align 4
  %194 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %193, i32 0, i32 78
  %195 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %194, align 4
  %196 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %197 = load %struct._jobject*, %struct._jobject** %6, align 4
  %198 = load %struct._jobject*, %struct._jobject** %7, align 4
  %199 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %200 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %201 = call signext i16 %195(%struct.JNINativeInterface_** noundef %196, %struct._jobject* noundef %197, %struct._jobject* noundef %198, %struct._jmethodID* noundef %199, %union.jvalue* noundef %200)
  store i16 %201, i16* %14, align 2
  %202 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %202)
  %203 = load i16, i16* %14, align 2
  ret i16 %203
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i16 @JNI_CallNonvirtualShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
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
  store i32 %25, i32* %13, align 4
  store i32 0, i32* %14, align 4
  br label %26

26:                                               ; preds = %189, %5
  %27 = load i32, i32* %14, align 4
  %28 = load i32, i32* %13, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %192

30:                                               ; preds = %26
  %31 = load i32, i32* %14, align 4
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %31
  %33 = load i8, i8* %32, align 1
  %34 = zext i8 %33 to i32
  %35 = icmp eq i32 %34, 90
  br i1 %35, label %36, label %46

36:                                               ; preds = %30
  %37 = bitcast %struct.__va_list* %6 to i8**
  %38 = load i8*, i8** %37, align 4
  %39 = getelementptr inbounds i8, i8* %38, i32 4
  store i8* %39, i8** %37, align 4
  %40 = bitcast i8* %38 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = load i32, i32* %14, align 4
  %44 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %43
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %42, i8* %45, align 8
  br label %188

46:                                               ; preds = %30
  %47 = load i32, i32* %14, align 4
  %48 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %47
  %49 = load i8, i8* %48, align 1
  %50 = zext i8 %49 to i32
  %51 = icmp eq i32 %50, 66
  br i1 %51, label %52, label %62

52:                                               ; preds = %46
  %53 = bitcast %struct.__va_list* %6 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = load i32, i32* %14, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %58, i8* %61, align 8
  br label %187

62:                                               ; preds = %46
  %63 = load i32, i32* %14, align 4
  %64 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %63
  %65 = load i8, i8* %64, align 1
  %66 = zext i8 %65 to i32
  %67 = icmp eq i32 %66, 67
  br i1 %67, label %68, label %78

68:                                               ; preds = %62
  %69 = bitcast %struct.__va_list* %6 to i8**
  %70 = load i8*, i8** %69, align 4
  %71 = getelementptr inbounds i8, i8* %70, i32 4
  store i8* %71, i8** %69, align 4
  %72 = bitcast i8* %70 to i32*
  %73 = load i32, i32* %72, align 4
  %74 = trunc i32 %73 to i16
  %75 = load i32, i32* %14, align 4
  %76 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %75
  %77 = bitcast %union.jvalue* %76 to i16*
  store i16 %74, i16* %77, align 8
  br label %186

78:                                               ; preds = %62
  %79 = load i32, i32* %14, align 4
  %80 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %79
  %81 = load i8, i8* %80, align 1
  %82 = zext i8 %81 to i32
  %83 = icmp eq i32 %82, 83
  br i1 %83, label %84, label %94

84:                                               ; preds = %78
  %85 = bitcast %struct.__va_list* %6 to i8**
  %86 = load i8*, i8** %85, align 4
  %87 = getelementptr inbounds i8, i8* %86, i32 4
  store i8* %87, i8** %85, align 4
  %88 = bitcast i8* %86 to i32*
  %89 = load i32, i32* %88, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %14, align 4
  %92 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %91
  %93 = bitcast %union.jvalue* %92 to i16*
  store i16 %90, i16* %93, align 8
  br label %185

94:                                               ; preds = %78
  %95 = load i32, i32* %14, align 4
  %96 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %95
  %97 = load i8, i8* %96, align 1
  %98 = zext i8 %97 to i32
  %99 = icmp eq i32 %98, 73
  br i1 %99, label %100, label %109

100:                                              ; preds = %94
  %101 = bitcast %struct.__va_list* %6 to i8**
  %102 = load i8*, i8** %101, align 4
  %103 = getelementptr inbounds i8, i8* %102, i32 4
  store i8* %103, i8** %101, align 4
  %104 = bitcast i8* %102 to i32*
  %105 = load i32, i32* %104, align 4
  %106 = load i32, i32* %14, align 4
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %105, i32* %108, align 8
  br label %184

109:                                              ; preds = %94
  %110 = load i32, i32* %14, align 4
  %111 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %110
  %112 = load i8, i8* %111, align 1
  %113 = zext i8 %112 to i32
  %114 = icmp eq i32 %113, 74
  br i1 %114, label %115, label %125

115:                                              ; preds = %109
  %116 = bitcast %struct.__va_list* %6 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = getelementptr inbounds i8, i8* %117, i32 4
  store i8* %118, i8** %116, align 4
  %119 = bitcast i8* %117 to i32*
  %120 = load i32, i32* %119, align 4
  %121 = sext i32 %120 to i64
  %122 = load i32, i32* %14, align 4
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %121, i64* %124, align 8
  br label %183

125:                                              ; preds = %109
  %126 = load i32, i32* %14, align 4
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %126
  %128 = load i8, i8* %127, align 1
  %129 = zext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %145

131:                                              ; preds = %125
  %132 = bitcast %struct.__va_list* %6 to i8**
  %133 = load i8*, i8** %132, align 4
  %134 = ptrtoint i8* %133 to i32
  %135 = add i32 %134, 7
  %136 = and i32 %135, -8
  %137 = inttoptr i32 %136 to i8*
  %138 = getelementptr inbounds i8, i8* %137, i32 8
  store i8* %138, i8** %132, align 4
  %139 = bitcast i8* %137 to double*
  %140 = load double, double* %139, align 8
  %141 = fptrunc double %140 to float
  %142 = load i32, i32* %14, align 4
  %143 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %142
  %144 = bitcast %union.jvalue* %143 to float*
  store float %141, float* %144, align 8
  br label %182

145:                                              ; preds = %125
  %146 = load i32, i32* %14, align 4
  %147 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %146
  %148 = load i8, i8* %147, align 1
  %149 = zext i8 %148 to i32
  %150 = icmp eq i32 %149, 68
  br i1 %150, label %151, label %164

151:                                              ; preds = %145
  %152 = bitcast %struct.__va_list* %6 to i8**
  %153 = load i8*, i8** %152, align 4
  %154 = ptrtoint i8* %153 to i32
  %155 = add i32 %154, 7
  %156 = and i32 %155, -8
  %157 = inttoptr i32 %156 to i8*
  %158 = getelementptr inbounds i8, i8* %157, i32 8
  store i8* %158, i8** %152, align 4
  %159 = bitcast i8* %157 to double*
  %160 = load double, double* %159, align 8
  %161 = load i32, i32* %14, align 4
  %162 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %161
  %163 = bitcast %union.jvalue* %162 to double*
  store double %160, double* %163, align 8
  br label %181

164:                                              ; preds = %145
  %165 = load i32, i32* %14, align 4
  %166 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %165
  %167 = load i8, i8* %166, align 1
  %168 = zext i8 %167 to i32
  %169 = icmp eq i32 %168, 76
  br i1 %169, label %170, label %180

170:                                              ; preds = %164
  %171 = bitcast %struct.__va_list* %6 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = getelementptr inbounds i8, i8* %172, i32 4
  store i8* %173, i8** %171, align 4
  %174 = bitcast i8* %172 to i8**
  %175 = load i8*, i8** %174, align 4
  %176 = bitcast i8* %175 to %struct._jobject*
  %177 = load i32, i32* %14, align 4
  %178 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %177
  %179 = bitcast %union.jvalue* %178 to %struct._jobject**
  store %struct._jobject* %176, %struct._jobject** %179, align 8
  br label %180

180:                                              ; preds = %170, %164
  br label %181

181:                                              ; preds = %180, %151
  br label %182

182:                                              ; preds = %181, %131
  br label %183

183:                                              ; preds = %182, %115
  br label %184

184:                                              ; preds = %183, %100
  br label %185

185:                                              ; preds = %184, %84
  br label %186

186:                                              ; preds = %185, %68
  br label %187

187:                                              ; preds = %186, %52
  br label %188

188:                                              ; preds = %187, %36
  br label %189

189:                                              ; preds = %188
  %190 = load i32, i32* %14, align 4
  %191 = add nsw i32 %190, 1
  store i32 %191, i32* %14, align 4
  br label %26, !llvm.loop !38

192:                                              ; preds = %26
  %193 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %194 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %193, align 4
  %195 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %194, i32 0, i32 78
  %196 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %195, align 4
  %197 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %198 = load %struct._jobject*, %struct._jobject** %8, align 4
  %199 = load %struct._jobject*, %struct._jobject** %9, align 4
  %200 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %201 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 0
  %202 = call signext i16 %196(%struct.JNINativeInterface_** noundef %197, %struct._jobject* noundef %198, %struct._jobject* noundef %199, %struct._jmethodID* noundef %200, %union.jvalue* noundef %201)
  ret i16 %202
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i16 @JNI_CallStaticShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %22, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %186, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %10, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %189

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  %32 = icmp eq i32 %31, 90
  br i1 %32, label %33, label %43

33:                                               ; preds = %27
  %34 = bitcast %struct.__va_list* %7 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %11, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %185

43:                                               ; preds = %27
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  %48 = icmp eq i32 %47, 66
  br i1 %48, label %49, label %59

49:                                               ; preds = %43
  %50 = bitcast %struct.__va_list* %7 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %55, i8* %58, align 8
  br label %184

59:                                               ; preds = %43
  %60 = load i32, i32* %11, align 4
  %61 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %60
  %62 = load i8, i8* %61, align 1
  %63 = zext i8 %62 to i32
  %64 = icmp eq i32 %63, 67
  br i1 %64, label %65, label %75

65:                                               ; preds = %59
  %66 = bitcast %struct.__va_list* %7 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %11, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %183

75:                                               ; preds = %59
  %76 = load i32, i32* %11, align 4
  %77 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %76
  %78 = load i8, i8* %77, align 1
  %79 = zext i8 %78 to i32
  %80 = icmp eq i32 %79, 83
  br i1 %80, label %81, label %91

81:                                               ; preds = %75
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i16*
  store i16 %87, i16* %90, align 8
  br label %182

91:                                               ; preds = %75
  %92 = load i32, i32* %11, align 4
  %93 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %92
  %94 = load i8, i8* %93, align 1
  %95 = zext i8 %94 to i32
  %96 = icmp eq i32 %95, 73
  br i1 %96, label %97, label %106

97:                                               ; preds = %91
  %98 = bitcast %struct.__va_list* %7 to i8**
  %99 = load i8*, i8** %98, align 4
  %100 = getelementptr inbounds i8, i8* %99, i32 4
  store i8* %100, i8** %98, align 4
  %101 = bitcast i8* %99 to i32*
  %102 = load i32, i32* %101, align 4
  %103 = load i32, i32* %11, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %102, i32* %105, align 8
  br label %181

106:                                              ; preds = %91
  %107 = load i32, i32* %11, align 4
  %108 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %107
  %109 = load i8, i8* %108, align 1
  %110 = zext i8 %109 to i32
  %111 = icmp eq i32 %110, 74
  br i1 %111, label %112, label %122

112:                                              ; preds = %106
  %113 = bitcast %struct.__va_list* %7 to i8**
  %114 = load i8*, i8** %113, align 4
  %115 = getelementptr inbounds i8, i8* %114, i32 4
  store i8* %115, i8** %113, align 4
  %116 = bitcast i8* %114 to i32*
  %117 = load i32, i32* %116, align 4
  %118 = sext i32 %117 to i64
  %119 = load i32, i32* %11, align 4
  %120 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %119
  %121 = bitcast %union.jvalue* %120 to i64*
  store i64 %118, i64* %121, align 8
  br label %180

122:                                              ; preds = %106
  %123 = load i32, i32* %11, align 4
  %124 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %123
  %125 = load i8, i8* %124, align 1
  %126 = zext i8 %125 to i32
  %127 = icmp eq i32 %126, 70
  br i1 %127, label %128, label %142

128:                                              ; preds = %122
  %129 = bitcast %struct.__va_list* %7 to i8**
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
  %139 = load i32, i32* %11, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %139
  %141 = bitcast %union.jvalue* %140 to float*
  store float %138, float* %141, align 8
  br label %179

142:                                              ; preds = %122
  %143 = load i32, i32* %11, align 4
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %143
  %145 = load i8, i8* %144, align 1
  %146 = zext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %161

148:                                              ; preds = %142
  %149 = bitcast %struct.__va_list* %7 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = ptrtoint i8* %150 to i32
  %152 = add i32 %151, 7
  %153 = and i32 %152, -8
  %154 = inttoptr i32 %153 to i8*
  %155 = getelementptr inbounds i8, i8* %154, i32 8
  store i8* %155, i8** %149, align 4
  %156 = bitcast i8* %154 to double*
  %157 = load double, double* %156, align 8
  %158 = load i32, i32* %11, align 4
  %159 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %158
  %160 = bitcast %union.jvalue* %159 to double*
  store double %157, double* %160, align 8
  br label %178

161:                                              ; preds = %142
  %162 = load i32, i32* %11, align 4
  %163 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %162
  %164 = load i8, i8* %163, align 1
  %165 = zext i8 %164 to i32
  %166 = icmp eq i32 %165, 76
  br i1 %166, label %167, label %177

167:                                              ; preds = %161
  %168 = bitcast %struct.__va_list* %7 to i8**
  %169 = load i8*, i8** %168, align 4
  %170 = getelementptr inbounds i8, i8* %169, i32 4
  store i8* %170, i8** %168, align 4
  %171 = bitcast i8* %169 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = bitcast i8* %172 to %struct._jobject*
  %174 = load i32, i32* %11, align 4
  %175 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %174
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %176, align 8
  br label %177

177:                                              ; preds = %167, %161
  br label %178

178:                                              ; preds = %177, %148
  br label %179

179:                                              ; preds = %178, %128
  br label %180

180:                                              ; preds = %179, %112
  br label %181

181:                                              ; preds = %180, %97
  br label %182

182:                                              ; preds = %181, %81
  br label %183

183:                                              ; preds = %182, %65
  br label %184

184:                                              ; preds = %183, %49
  br label %185

185:                                              ; preds = %184, %33
  br label %186

186:                                              ; preds = %185
  %187 = load i32, i32* %11, align 4
  %188 = add nsw i32 %187, 1
  store i32 %188, i32* %11, align 4
  br label %23, !llvm.loop !39

189:                                              ; preds = %23
  %190 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %191 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %190, align 4
  %192 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %191, i32 0, i32 128
  %193 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %192, align 4
  %194 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %195 = load %struct._jobject*, %struct._jobject** %5, align 4
  %196 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %197 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  %198 = call signext i16 %193(%struct.JNINativeInterface_** noundef %194, %struct._jobject* noundef %195, %struct._jmethodID* noundef %196, %union.jvalue* noundef %197)
  store i16 %198, i16* %12, align 2
  %199 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %199)
  %200 = load i16, i16* %12, align 2
  ret i16 %200
}

; Function Attrs: noinline nounwind optnone
define dso_local signext i16 @JNI_CallStaticShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !40

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 128
  %194 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %199 = call signext i16 %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
  ret i16 %199
}

; Function Attrs: noinline nounwind optnone
define dso_local i32 @JNI_CallIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %22, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %186, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %10, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %189

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  %32 = icmp eq i32 %31, 90
  br i1 %32, label %33, label %43

33:                                               ; preds = %27
  %34 = bitcast %struct.__va_list* %7 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %11, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %185

43:                                               ; preds = %27
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  %48 = icmp eq i32 %47, 66
  br i1 %48, label %49, label %59

49:                                               ; preds = %43
  %50 = bitcast %struct.__va_list* %7 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %55, i8* %58, align 8
  br label %184

59:                                               ; preds = %43
  %60 = load i32, i32* %11, align 4
  %61 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %60
  %62 = load i8, i8* %61, align 1
  %63 = zext i8 %62 to i32
  %64 = icmp eq i32 %63, 67
  br i1 %64, label %65, label %75

65:                                               ; preds = %59
  %66 = bitcast %struct.__va_list* %7 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %11, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %183

75:                                               ; preds = %59
  %76 = load i32, i32* %11, align 4
  %77 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %76
  %78 = load i8, i8* %77, align 1
  %79 = zext i8 %78 to i32
  %80 = icmp eq i32 %79, 83
  br i1 %80, label %81, label %91

81:                                               ; preds = %75
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i16*
  store i16 %87, i16* %90, align 8
  br label %182

91:                                               ; preds = %75
  %92 = load i32, i32* %11, align 4
  %93 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %92
  %94 = load i8, i8* %93, align 1
  %95 = zext i8 %94 to i32
  %96 = icmp eq i32 %95, 73
  br i1 %96, label %97, label %106

97:                                               ; preds = %91
  %98 = bitcast %struct.__va_list* %7 to i8**
  %99 = load i8*, i8** %98, align 4
  %100 = getelementptr inbounds i8, i8* %99, i32 4
  store i8* %100, i8** %98, align 4
  %101 = bitcast i8* %99 to i32*
  %102 = load i32, i32* %101, align 4
  %103 = load i32, i32* %11, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %102, i32* %105, align 8
  br label %181

106:                                              ; preds = %91
  %107 = load i32, i32* %11, align 4
  %108 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %107
  %109 = load i8, i8* %108, align 1
  %110 = zext i8 %109 to i32
  %111 = icmp eq i32 %110, 74
  br i1 %111, label %112, label %122

112:                                              ; preds = %106
  %113 = bitcast %struct.__va_list* %7 to i8**
  %114 = load i8*, i8** %113, align 4
  %115 = getelementptr inbounds i8, i8* %114, i32 4
  store i8* %115, i8** %113, align 4
  %116 = bitcast i8* %114 to i32*
  %117 = load i32, i32* %116, align 4
  %118 = sext i32 %117 to i64
  %119 = load i32, i32* %11, align 4
  %120 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %119
  %121 = bitcast %union.jvalue* %120 to i64*
  store i64 %118, i64* %121, align 8
  br label %180

122:                                              ; preds = %106
  %123 = load i32, i32* %11, align 4
  %124 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %123
  %125 = load i8, i8* %124, align 1
  %126 = zext i8 %125 to i32
  %127 = icmp eq i32 %126, 70
  br i1 %127, label %128, label %142

128:                                              ; preds = %122
  %129 = bitcast %struct.__va_list* %7 to i8**
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
  %139 = load i32, i32* %11, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %139
  %141 = bitcast %union.jvalue* %140 to float*
  store float %138, float* %141, align 8
  br label %179

142:                                              ; preds = %122
  %143 = load i32, i32* %11, align 4
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %143
  %145 = load i8, i8* %144, align 1
  %146 = zext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %161

148:                                              ; preds = %142
  %149 = bitcast %struct.__va_list* %7 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = ptrtoint i8* %150 to i32
  %152 = add i32 %151, 7
  %153 = and i32 %152, -8
  %154 = inttoptr i32 %153 to i8*
  %155 = getelementptr inbounds i8, i8* %154, i32 8
  store i8* %155, i8** %149, align 4
  %156 = bitcast i8* %154 to double*
  %157 = load double, double* %156, align 8
  %158 = load i32, i32* %11, align 4
  %159 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %158
  %160 = bitcast %union.jvalue* %159 to double*
  store double %157, double* %160, align 8
  br label %178

161:                                              ; preds = %142
  %162 = load i32, i32* %11, align 4
  %163 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %162
  %164 = load i8, i8* %163, align 1
  %165 = zext i8 %164 to i32
  %166 = icmp eq i32 %165, 76
  br i1 %166, label %167, label %177

167:                                              ; preds = %161
  %168 = bitcast %struct.__va_list* %7 to i8**
  %169 = load i8*, i8** %168, align 4
  %170 = getelementptr inbounds i8, i8* %169, i32 4
  store i8* %170, i8** %168, align 4
  %171 = bitcast i8* %169 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = bitcast i8* %172 to %struct._jobject*
  %174 = load i32, i32* %11, align 4
  %175 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %174
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %176, align 8
  br label %177

177:                                              ; preds = %167, %161
  br label %178

178:                                              ; preds = %177, %148
  br label %179

179:                                              ; preds = %178, %128
  br label %180

180:                                              ; preds = %179, %112
  br label %181

181:                                              ; preds = %180, %97
  br label %182

182:                                              ; preds = %181, %81
  br label %183

183:                                              ; preds = %182, %65
  br label %184

184:                                              ; preds = %183, %49
  br label %185

185:                                              ; preds = %184, %33
  br label %186

186:                                              ; preds = %185
  %187 = load i32, i32* %11, align 4
  %188 = add nsw i32 %187, 1
  store i32 %188, i32* %11, align 4
  br label %23, !llvm.loop !41

189:                                              ; preds = %23
  %190 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %191 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %190, align 4
  %192 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %191, i32 0, i32 51
  %193 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %192, align 4
  %194 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %195 = load %struct._jobject*, %struct._jobject** %5, align 4
  %196 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %197 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  %198 = call i32 %193(%struct.JNINativeInterface_** noundef %194, %struct._jobject* noundef %195, %struct._jmethodID* noundef %196, %union.jvalue* noundef %197)
  store i32 %198, i32* %12, align 4
  %199 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %199)
  %200 = load i32, i32* %12, align 4
  ret i32 %200
}

; Function Attrs: noinline nounwind optnone
define dso_local i32 @JNI_CallIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !42

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 51
  %194 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %199 = call i32 %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
  ret i32 %199
}

; Function Attrs: noinline nounwind optnone
define dso_local i32 @JNI_CallNonvirtualIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct.__va_list, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
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
  store i32 %24, i32* %12, align 4
  store i32 0, i32* %13, align 4
  br label %25

25:                                               ; preds = %188, %4
  %26 = load i32, i32* %13, align 4
  %27 = load i32, i32* %12, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %191

29:                                               ; preds = %25
  %30 = load i32, i32* %13, align 4
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %30
  %32 = load i8, i8* %31, align 1
  %33 = zext i8 %32 to i32
  %34 = icmp eq i32 %33, 90
  br i1 %34, label %35, label %45

35:                                               ; preds = %29
  %36 = bitcast %struct.__va_list* %9 to i8**
  %37 = load i8*, i8** %36, align 4
  %38 = getelementptr inbounds i8, i8* %37, i32 4
  store i8* %38, i8** %36, align 4
  %39 = bitcast i8* %37 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = load i32, i32* %13, align 4
  %43 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %42
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %41, i8* %44, align 8
  br label %187

45:                                               ; preds = %29
  %46 = load i32, i32* %13, align 4
  %47 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %46
  %48 = load i8, i8* %47, align 1
  %49 = zext i8 %48 to i32
  %50 = icmp eq i32 %49, 66
  br i1 %50, label %51, label %61

51:                                               ; preds = %45
  %52 = bitcast %struct.__va_list* %9 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load i32, i32* %13, align 4
  %59 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %58
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %57, i8* %60, align 8
  br label %186

61:                                               ; preds = %45
  %62 = load i32, i32* %13, align 4
  %63 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %62
  %64 = load i8, i8* %63, align 1
  %65 = zext i8 %64 to i32
  %66 = icmp eq i32 %65, 67
  br i1 %66, label %67, label %77

67:                                               ; preds = %61
  %68 = bitcast %struct.__va_list* %9 to i8**
  %69 = load i8*, i8** %68, align 4
  %70 = getelementptr inbounds i8, i8* %69, i32 4
  store i8* %70, i8** %68, align 4
  %71 = bitcast i8* %69 to i32*
  %72 = load i32, i32* %71, align 4
  %73 = trunc i32 %72 to i16
  %74 = load i32, i32* %13, align 4
  %75 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %74
  %76 = bitcast %union.jvalue* %75 to i16*
  store i16 %73, i16* %76, align 8
  br label %185

77:                                               ; preds = %61
  %78 = load i32, i32* %13, align 4
  %79 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %78
  %80 = load i8, i8* %79, align 1
  %81 = zext i8 %80 to i32
  %82 = icmp eq i32 %81, 83
  br i1 %82, label %83, label %93

83:                                               ; preds = %77
  %84 = bitcast %struct.__va_list* %9 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = trunc i32 %88 to i16
  %90 = load i32, i32* %13, align 4
  %91 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %90
  %92 = bitcast %union.jvalue* %91 to i16*
  store i16 %89, i16* %92, align 8
  br label %184

93:                                               ; preds = %77
  %94 = load i32, i32* %13, align 4
  %95 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %94
  %96 = load i8, i8* %95, align 1
  %97 = zext i8 %96 to i32
  %98 = icmp eq i32 %97, 73
  br i1 %98, label %99, label %108

99:                                               ; preds = %93
  %100 = bitcast %struct.__va_list* %9 to i8**
  %101 = load i8*, i8** %100, align 4
  %102 = getelementptr inbounds i8, i8* %101, i32 4
  store i8* %102, i8** %100, align 4
  %103 = bitcast i8* %101 to i32*
  %104 = load i32, i32* %103, align 4
  %105 = load i32, i32* %13, align 4
  %106 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %105
  %107 = bitcast %union.jvalue* %106 to i32*
  store i32 %104, i32* %107, align 8
  br label %183

108:                                              ; preds = %93
  %109 = load i32, i32* %13, align 4
  %110 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %109
  %111 = load i8, i8* %110, align 1
  %112 = zext i8 %111 to i32
  %113 = icmp eq i32 %112, 74
  br i1 %113, label %114, label %124

114:                                              ; preds = %108
  %115 = bitcast %struct.__va_list* %9 to i8**
  %116 = load i8*, i8** %115, align 4
  %117 = getelementptr inbounds i8, i8* %116, i32 4
  store i8* %117, i8** %115, align 4
  %118 = bitcast i8* %116 to i32*
  %119 = load i32, i32* %118, align 4
  %120 = sext i32 %119 to i64
  %121 = load i32, i32* %13, align 4
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %120, i64* %123, align 8
  br label %182

124:                                              ; preds = %108
  %125 = load i32, i32* %13, align 4
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %125
  %127 = load i8, i8* %126, align 1
  %128 = zext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %144

130:                                              ; preds = %124
  %131 = bitcast %struct.__va_list* %9 to i8**
  %132 = load i8*, i8** %131, align 4
  %133 = ptrtoint i8* %132 to i32
  %134 = add i32 %133, 7
  %135 = and i32 %134, -8
  %136 = inttoptr i32 %135 to i8*
  %137 = getelementptr inbounds i8, i8* %136, i32 8
  store i8* %137, i8** %131, align 4
  %138 = bitcast i8* %136 to double*
  %139 = load double, double* %138, align 8
  %140 = fptrunc double %139 to float
  %141 = load i32, i32* %13, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %140, float* %143, align 8
  br label %181

144:                                              ; preds = %124
  %145 = load i32, i32* %13, align 4
  %146 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %145
  %147 = load i8, i8* %146, align 1
  %148 = zext i8 %147 to i32
  %149 = icmp eq i32 %148, 68
  br i1 %149, label %150, label %163

150:                                              ; preds = %144
  %151 = bitcast %struct.__va_list* %9 to i8**
  %152 = load i8*, i8** %151, align 4
  %153 = ptrtoint i8* %152 to i32
  %154 = add i32 %153, 7
  %155 = and i32 %154, -8
  %156 = inttoptr i32 %155 to i8*
  %157 = getelementptr inbounds i8, i8* %156, i32 8
  store i8* %157, i8** %151, align 4
  %158 = bitcast i8* %156 to double*
  %159 = load double, double* %158, align 8
  %160 = load i32, i32* %13, align 4
  %161 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %160
  %162 = bitcast %union.jvalue* %161 to double*
  store double %159, double* %162, align 8
  br label %180

163:                                              ; preds = %144
  %164 = load i32, i32* %13, align 4
  %165 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %164
  %166 = load i8, i8* %165, align 1
  %167 = zext i8 %166 to i32
  %168 = icmp eq i32 %167, 76
  br i1 %168, label %169, label %179

169:                                              ; preds = %163
  %170 = bitcast %struct.__va_list* %9 to i8**
  %171 = load i8*, i8** %170, align 4
  %172 = getelementptr inbounds i8, i8* %171, i32 4
  store i8* %172, i8** %170, align 4
  %173 = bitcast i8* %171 to i8**
  %174 = load i8*, i8** %173, align 4
  %175 = bitcast i8* %174 to %struct._jobject*
  %176 = load i32, i32* %13, align 4
  %177 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %176
  %178 = bitcast %union.jvalue* %177 to %struct._jobject**
  store %struct._jobject* %175, %struct._jobject** %178, align 8
  br label %179

179:                                              ; preds = %169, %163
  br label %180

180:                                              ; preds = %179, %150
  br label %181

181:                                              ; preds = %180, %130
  br label %182

182:                                              ; preds = %181, %114
  br label %183

183:                                              ; preds = %182, %99
  br label %184

184:                                              ; preds = %183, %83
  br label %185

185:                                              ; preds = %184, %67
  br label %186

186:                                              ; preds = %185, %51
  br label %187

187:                                              ; preds = %186, %35
  br label %188

188:                                              ; preds = %187
  %189 = load i32, i32* %13, align 4
  %190 = add nsw i32 %189, 1
  store i32 %190, i32* %13, align 4
  br label %25, !llvm.loop !43

191:                                              ; preds = %25
  %192 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %193 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %192, align 4
  %194 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %193, i32 0, i32 81
  %195 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %194, align 4
  %196 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %197 = load %struct._jobject*, %struct._jobject** %6, align 4
  %198 = load %struct._jobject*, %struct._jobject** %7, align 4
  %199 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %200 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %201 = call i32 %195(%struct.JNINativeInterface_** noundef %196, %struct._jobject* noundef %197, %struct._jobject* noundef %198, %struct._jmethodID* noundef %199, %union.jvalue* noundef %200)
  store i32 %201, i32* %14, align 4
  %202 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %202)
  %203 = load i32, i32* %14, align 4
  ret i32 %203
}

; Function Attrs: noinline nounwind optnone
define dso_local i32 @JNI_CallNonvirtualIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
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
  store i32 %25, i32* %13, align 4
  store i32 0, i32* %14, align 4
  br label %26

26:                                               ; preds = %189, %5
  %27 = load i32, i32* %14, align 4
  %28 = load i32, i32* %13, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %192

30:                                               ; preds = %26
  %31 = load i32, i32* %14, align 4
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %31
  %33 = load i8, i8* %32, align 1
  %34 = zext i8 %33 to i32
  %35 = icmp eq i32 %34, 90
  br i1 %35, label %36, label %46

36:                                               ; preds = %30
  %37 = bitcast %struct.__va_list* %6 to i8**
  %38 = load i8*, i8** %37, align 4
  %39 = getelementptr inbounds i8, i8* %38, i32 4
  store i8* %39, i8** %37, align 4
  %40 = bitcast i8* %38 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = load i32, i32* %14, align 4
  %44 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %43
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %42, i8* %45, align 8
  br label %188

46:                                               ; preds = %30
  %47 = load i32, i32* %14, align 4
  %48 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %47
  %49 = load i8, i8* %48, align 1
  %50 = zext i8 %49 to i32
  %51 = icmp eq i32 %50, 66
  br i1 %51, label %52, label %62

52:                                               ; preds = %46
  %53 = bitcast %struct.__va_list* %6 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = load i32, i32* %14, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %58, i8* %61, align 8
  br label %187

62:                                               ; preds = %46
  %63 = load i32, i32* %14, align 4
  %64 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %63
  %65 = load i8, i8* %64, align 1
  %66 = zext i8 %65 to i32
  %67 = icmp eq i32 %66, 67
  br i1 %67, label %68, label %78

68:                                               ; preds = %62
  %69 = bitcast %struct.__va_list* %6 to i8**
  %70 = load i8*, i8** %69, align 4
  %71 = getelementptr inbounds i8, i8* %70, i32 4
  store i8* %71, i8** %69, align 4
  %72 = bitcast i8* %70 to i32*
  %73 = load i32, i32* %72, align 4
  %74 = trunc i32 %73 to i16
  %75 = load i32, i32* %14, align 4
  %76 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %75
  %77 = bitcast %union.jvalue* %76 to i16*
  store i16 %74, i16* %77, align 8
  br label %186

78:                                               ; preds = %62
  %79 = load i32, i32* %14, align 4
  %80 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %79
  %81 = load i8, i8* %80, align 1
  %82 = zext i8 %81 to i32
  %83 = icmp eq i32 %82, 83
  br i1 %83, label %84, label %94

84:                                               ; preds = %78
  %85 = bitcast %struct.__va_list* %6 to i8**
  %86 = load i8*, i8** %85, align 4
  %87 = getelementptr inbounds i8, i8* %86, i32 4
  store i8* %87, i8** %85, align 4
  %88 = bitcast i8* %86 to i32*
  %89 = load i32, i32* %88, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %14, align 4
  %92 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %91
  %93 = bitcast %union.jvalue* %92 to i16*
  store i16 %90, i16* %93, align 8
  br label %185

94:                                               ; preds = %78
  %95 = load i32, i32* %14, align 4
  %96 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %95
  %97 = load i8, i8* %96, align 1
  %98 = zext i8 %97 to i32
  %99 = icmp eq i32 %98, 73
  br i1 %99, label %100, label %109

100:                                              ; preds = %94
  %101 = bitcast %struct.__va_list* %6 to i8**
  %102 = load i8*, i8** %101, align 4
  %103 = getelementptr inbounds i8, i8* %102, i32 4
  store i8* %103, i8** %101, align 4
  %104 = bitcast i8* %102 to i32*
  %105 = load i32, i32* %104, align 4
  %106 = load i32, i32* %14, align 4
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %105, i32* %108, align 8
  br label %184

109:                                              ; preds = %94
  %110 = load i32, i32* %14, align 4
  %111 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %110
  %112 = load i8, i8* %111, align 1
  %113 = zext i8 %112 to i32
  %114 = icmp eq i32 %113, 74
  br i1 %114, label %115, label %125

115:                                              ; preds = %109
  %116 = bitcast %struct.__va_list* %6 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = getelementptr inbounds i8, i8* %117, i32 4
  store i8* %118, i8** %116, align 4
  %119 = bitcast i8* %117 to i32*
  %120 = load i32, i32* %119, align 4
  %121 = sext i32 %120 to i64
  %122 = load i32, i32* %14, align 4
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %121, i64* %124, align 8
  br label %183

125:                                              ; preds = %109
  %126 = load i32, i32* %14, align 4
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %126
  %128 = load i8, i8* %127, align 1
  %129 = zext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %145

131:                                              ; preds = %125
  %132 = bitcast %struct.__va_list* %6 to i8**
  %133 = load i8*, i8** %132, align 4
  %134 = ptrtoint i8* %133 to i32
  %135 = add i32 %134, 7
  %136 = and i32 %135, -8
  %137 = inttoptr i32 %136 to i8*
  %138 = getelementptr inbounds i8, i8* %137, i32 8
  store i8* %138, i8** %132, align 4
  %139 = bitcast i8* %137 to double*
  %140 = load double, double* %139, align 8
  %141 = fptrunc double %140 to float
  %142 = load i32, i32* %14, align 4
  %143 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %142
  %144 = bitcast %union.jvalue* %143 to float*
  store float %141, float* %144, align 8
  br label %182

145:                                              ; preds = %125
  %146 = load i32, i32* %14, align 4
  %147 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %146
  %148 = load i8, i8* %147, align 1
  %149 = zext i8 %148 to i32
  %150 = icmp eq i32 %149, 68
  br i1 %150, label %151, label %164

151:                                              ; preds = %145
  %152 = bitcast %struct.__va_list* %6 to i8**
  %153 = load i8*, i8** %152, align 4
  %154 = ptrtoint i8* %153 to i32
  %155 = add i32 %154, 7
  %156 = and i32 %155, -8
  %157 = inttoptr i32 %156 to i8*
  %158 = getelementptr inbounds i8, i8* %157, i32 8
  store i8* %158, i8** %152, align 4
  %159 = bitcast i8* %157 to double*
  %160 = load double, double* %159, align 8
  %161 = load i32, i32* %14, align 4
  %162 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %161
  %163 = bitcast %union.jvalue* %162 to double*
  store double %160, double* %163, align 8
  br label %181

164:                                              ; preds = %145
  %165 = load i32, i32* %14, align 4
  %166 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %165
  %167 = load i8, i8* %166, align 1
  %168 = zext i8 %167 to i32
  %169 = icmp eq i32 %168, 76
  br i1 %169, label %170, label %180

170:                                              ; preds = %164
  %171 = bitcast %struct.__va_list* %6 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = getelementptr inbounds i8, i8* %172, i32 4
  store i8* %173, i8** %171, align 4
  %174 = bitcast i8* %172 to i8**
  %175 = load i8*, i8** %174, align 4
  %176 = bitcast i8* %175 to %struct._jobject*
  %177 = load i32, i32* %14, align 4
  %178 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %177
  %179 = bitcast %union.jvalue* %178 to %struct._jobject**
  store %struct._jobject* %176, %struct._jobject** %179, align 8
  br label %180

180:                                              ; preds = %170, %164
  br label %181

181:                                              ; preds = %180, %151
  br label %182

182:                                              ; preds = %181, %131
  br label %183

183:                                              ; preds = %182, %115
  br label %184

184:                                              ; preds = %183, %100
  br label %185

185:                                              ; preds = %184, %84
  br label %186

186:                                              ; preds = %185, %68
  br label %187

187:                                              ; preds = %186, %52
  br label %188

188:                                              ; preds = %187, %36
  br label %189

189:                                              ; preds = %188
  %190 = load i32, i32* %14, align 4
  %191 = add nsw i32 %190, 1
  store i32 %191, i32* %14, align 4
  br label %26, !llvm.loop !44

192:                                              ; preds = %26
  %193 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %194 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %193, align 4
  %195 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %194, i32 0, i32 81
  %196 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %195, align 4
  %197 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %198 = load %struct._jobject*, %struct._jobject** %8, align 4
  %199 = load %struct._jobject*, %struct._jobject** %9, align 4
  %200 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %201 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 0
  %202 = call i32 %196(%struct.JNINativeInterface_** noundef %197, %struct._jobject* noundef %198, %struct._jobject* noundef %199, %struct._jmethodID* noundef %200, %union.jvalue* noundef %201)
  ret i32 %202
}

; Function Attrs: noinline nounwind optnone
define dso_local i32 @JNI_CallStaticIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %22, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %186, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %10, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %189

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  %32 = icmp eq i32 %31, 90
  br i1 %32, label %33, label %43

33:                                               ; preds = %27
  %34 = bitcast %struct.__va_list* %7 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %11, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %185

43:                                               ; preds = %27
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  %48 = icmp eq i32 %47, 66
  br i1 %48, label %49, label %59

49:                                               ; preds = %43
  %50 = bitcast %struct.__va_list* %7 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %55, i8* %58, align 8
  br label %184

59:                                               ; preds = %43
  %60 = load i32, i32* %11, align 4
  %61 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %60
  %62 = load i8, i8* %61, align 1
  %63 = zext i8 %62 to i32
  %64 = icmp eq i32 %63, 67
  br i1 %64, label %65, label %75

65:                                               ; preds = %59
  %66 = bitcast %struct.__va_list* %7 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %11, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %183

75:                                               ; preds = %59
  %76 = load i32, i32* %11, align 4
  %77 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %76
  %78 = load i8, i8* %77, align 1
  %79 = zext i8 %78 to i32
  %80 = icmp eq i32 %79, 83
  br i1 %80, label %81, label %91

81:                                               ; preds = %75
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i16*
  store i16 %87, i16* %90, align 8
  br label %182

91:                                               ; preds = %75
  %92 = load i32, i32* %11, align 4
  %93 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %92
  %94 = load i8, i8* %93, align 1
  %95 = zext i8 %94 to i32
  %96 = icmp eq i32 %95, 73
  br i1 %96, label %97, label %106

97:                                               ; preds = %91
  %98 = bitcast %struct.__va_list* %7 to i8**
  %99 = load i8*, i8** %98, align 4
  %100 = getelementptr inbounds i8, i8* %99, i32 4
  store i8* %100, i8** %98, align 4
  %101 = bitcast i8* %99 to i32*
  %102 = load i32, i32* %101, align 4
  %103 = load i32, i32* %11, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %102, i32* %105, align 8
  br label %181

106:                                              ; preds = %91
  %107 = load i32, i32* %11, align 4
  %108 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %107
  %109 = load i8, i8* %108, align 1
  %110 = zext i8 %109 to i32
  %111 = icmp eq i32 %110, 74
  br i1 %111, label %112, label %122

112:                                              ; preds = %106
  %113 = bitcast %struct.__va_list* %7 to i8**
  %114 = load i8*, i8** %113, align 4
  %115 = getelementptr inbounds i8, i8* %114, i32 4
  store i8* %115, i8** %113, align 4
  %116 = bitcast i8* %114 to i32*
  %117 = load i32, i32* %116, align 4
  %118 = sext i32 %117 to i64
  %119 = load i32, i32* %11, align 4
  %120 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %119
  %121 = bitcast %union.jvalue* %120 to i64*
  store i64 %118, i64* %121, align 8
  br label %180

122:                                              ; preds = %106
  %123 = load i32, i32* %11, align 4
  %124 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %123
  %125 = load i8, i8* %124, align 1
  %126 = zext i8 %125 to i32
  %127 = icmp eq i32 %126, 70
  br i1 %127, label %128, label %142

128:                                              ; preds = %122
  %129 = bitcast %struct.__va_list* %7 to i8**
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
  %139 = load i32, i32* %11, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %139
  %141 = bitcast %union.jvalue* %140 to float*
  store float %138, float* %141, align 8
  br label %179

142:                                              ; preds = %122
  %143 = load i32, i32* %11, align 4
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %143
  %145 = load i8, i8* %144, align 1
  %146 = zext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %161

148:                                              ; preds = %142
  %149 = bitcast %struct.__va_list* %7 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = ptrtoint i8* %150 to i32
  %152 = add i32 %151, 7
  %153 = and i32 %152, -8
  %154 = inttoptr i32 %153 to i8*
  %155 = getelementptr inbounds i8, i8* %154, i32 8
  store i8* %155, i8** %149, align 4
  %156 = bitcast i8* %154 to double*
  %157 = load double, double* %156, align 8
  %158 = load i32, i32* %11, align 4
  %159 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %158
  %160 = bitcast %union.jvalue* %159 to double*
  store double %157, double* %160, align 8
  br label %178

161:                                              ; preds = %142
  %162 = load i32, i32* %11, align 4
  %163 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %162
  %164 = load i8, i8* %163, align 1
  %165 = zext i8 %164 to i32
  %166 = icmp eq i32 %165, 76
  br i1 %166, label %167, label %177

167:                                              ; preds = %161
  %168 = bitcast %struct.__va_list* %7 to i8**
  %169 = load i8*, i8** %168, align 4
  %170 = getelementptr inbounds i8, i8* %169, i32 4
  store i8* %170, i8** %168, align 4
  %171 = bitcast i8* %169 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = bitcast i8* %172 to %struct._jobject*
  %174 = load i32, i32* %11, align 4
  %175 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %174
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %176, align 8
  br label %177

177:                                              ; preds = %167, %161
  br label %178

178:                                              ; preds = %177, %148
  br label %179

179:                                              ; preds = %178, %128
  br label %180

180:                                              ; preds = %179, %112
  br label %181

181:                                              ; preds = %180, %97
  br label %182

182:                                              ; preds = %181, %81
  br label %183

183:                                              ; preds = %182, %65
  br label %184

184:                                              ; preds = %183, %49
  br label %185

185:                                              ; preds = %184, %33
  br label %186

186:                                              ; preds = %185
  %187 = load i32, i32* %11, align 4
  %188 = add nsw i32 %187, 1
  store i32 %188, i32* %11, align 4
  br label %23, !llvm.loop !45

189:                                              ; preds = %23
  %190 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %191 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %190, align 4
  %192 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %191, i32 0, i32 131
  %193 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %192, align 4
  %194 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %195 = load %struct._jobject*, %struct._jobject** %5, align 4
  %196 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %197 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  %198 = call i32 %193(%struct.JNINativeInterface_** noundef %194, %struct._jobject* noundef %195, %struct._jmethodID* noundef %196, %union.jvalue* noundef %197)
  store i32 %198, i32* %12, align 4
  %199 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %199)
  %200 = load i32, i32* %12, align 4
  ret i32 %200
}

; Function Attrs: noinline nounwind optnone
define dso_local i32 @JNI_CallStaticIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !46

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 131
  %194 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %199 = call i32 %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
  ret i32 %199
}

; Function Attrs: noinline nounwind optnone
define dso_local i64 @JNI_CallLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %22, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %186, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %10, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %189

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  %32 = icmp eq i32 %31, 90
  br i1 %32, label %33, label %43

33:                                               ; preds = %27
  %34 = bitcast %struct.__va_list* %7 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %11, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %185

43:                                               ; preds = %27
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  %48 = icmp eq i32 %47, 66
  br i1 %48, label %49, label %59

49:                                               ; preds = %43
  %50 = bitcast %struct.__va_list* %7 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %55, i8* %58, align 8
  br label %184

59:                                               ; preds = %43
  %60 = load i32, i32* %11, align 4
  %61 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %60
  %62 = load i8, i8* %61, align 1
  %63 = zext i8 %62 to i32
  %64 = icmp eq i32 %63, 67
  br i1 %64, label %65, label %75

65:                                               ; preds = %59
  %66 = bitcast %struct.__va_list* %7 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %11, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %183

75:                                               ; preds = %59
  %76 = load i32, i32* %11, align 4
  %77 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %76
  %78 = load i8, i8* %77, align 1
  %79 = zext i8 %78 to i32
  %80 = icmp eq i32 %79, 83
  br i1 %80, label %81, label %91

81:                                               ; preds = %75
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i16*
  store i16 %87, i16* %90, align 8
  br label %182

91:                                               ; preds = %75
  %92 = load i32, i32* %11, align 4
  %93 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %92
  %94 = load i8, i8* %93, align 1
  %95 = zext i8 %94 to i32
  %96 = icmp eq i32 %95, 73
  br i1 %96, label %97, label %106

97:                                               ; preds = %91
  %98 = bitcast %struct.__va_list* %7 to i8**
  %99 = load i8*, i8** %98, align 4
  %100 = getelementptr inbounds i8, i8* %99, i32 4
  store i8* %100, i8** %98, align 4
  %101 = bitcast i8* %99 to i32*
  %102 = load i32, i32* %101, align 4
  %103 = load i32, i32* %11, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %102, i32* %105, align 8
  br label %181

106:                                              ; preds = %91
  %107 = load i32, i32* %11, align 4
  %108 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %107
  %109 = load i8, i8* %108, align 1
  %110 = zext i8 %109 to i32
  %111 = icmp eq i32 %110, 74
  br i1 %111, label %112, label %122

112:                                              ; preds = %106
  %113 = bitcast %struct.__va_list* %7 to i8**
  %114 = load i8*, i8** %113, align 4
  %115 = getelementptr inbounds i8, i8* %114, i32 4
  store i8* %115, i8** %113, align 4
  %116 = bitcast i8* %114 to i32*
  %117 = load i32, i32* %116, align 4
  %118 = sext i32 %117 to i64
  %119 = load i32, i32* %11, align 4
  %120 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %119
  %121 = bitcast %union.jvalue* %120 to i64*
  store i64 %118, i64* %121, align 8
  br label %180

122:                                              ; preds = %106
  %123 = load i32, i32* %11, align 4
  %124 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %123
  %125 = load i8, i8* %124, align 1
  %126 = zext i8 %125 to i32
  %127 = icmp eq i32 %126, 70
  br i1 %127, label %128, label %142

128:                                              ; preds = %122
  %129 = bitcast %struct.__va_list* %7 to i8**
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
  %139 = load i32, i32* %11, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %139
  %141 = bitcast %union.jvalue* %140 to float*
  store float %138, float* %141, align 8
  br label %179

142:                                              ; preds = %122
  %143 = load i32, i32* %11, align 4
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %143
  %145 = load i8, i8* %144, align 1
  %146 = zext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %161

148:                                              ; preds = %142
  %149 = bitcast %struct.__va_list* %7 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = ptrtoint i8* %150 to i32
  %152 = add i32 %151, 7
  %153 = and i32 %152, -8
  %154 = inttoptr i32 %153 to i8*
  %155 = getelementptr inbounds i8, i8* %154, i32 8
  store i8* %155, i8** %149, align 4
  %156 = bitcast i8* %154 to double*
  %157 = load double, double* %156, align 8
  %158 = load i32, i32* %11, align 4
  %159 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %158
  %160 = bitcast %union.jvalue* %159 to double*
  store double %157, double* %160, align 8
  br label %178

161:                                              ; preds = %142
  %162 = load i32, i32* %11, align 4
  %163 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %162
  %164 = load i8, i8* %163, align 1
  %165 = zext i8 %164 to i32
  %166 = icmp eq i32 %165, 76
  br i1 %166, label %167, label %177

167:                                              ; preds = %161
  %168 = bitcast %struct.__va_list* %7 to i8**
  %169 = load i8*, i8** %168, align 4
  %170 = getelementptr inbounds i8, i8* %169, i32 4
  store i8* %170, i8** %168, align 4
  %171 = bitcast i8* %169 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = bitcast i8* %172 to %struct._jobject*
  %174 = load i32, i32* %11, align 4
  %175 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %174
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %176, align 8
  br label %177

177:                                              ; preds = %167, %161
  br label %178

178:                                              ; preds = %177, %148
  br label %179

179:                                              ; preds = %178, %128
  br label %180

180:                                              ; preds = %179, %112
  br label %181

181:                                              ; preds = %180, %97
  br label %182

182:                                              ; preds = %181, %81
  br label %183

183:                                              ; preds = %182, %65
  br label %184

184:                                              ; preds = %183, %49
  br label %185

185:                                              ; preds = %184, %33
  br label %186

186:                                              ; preds = %185
  %187 = load i32, i32* %11, align 4
  %188 = add nsw i32 %187, 1
  store i32 %188, i32* %11, align 4
  br label %23, !llvm.loop !47

189:                                              ; preds = %23
  %190 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %191 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %190, align 4
  %192 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %191, i32 0, i32 54
  %193 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %192, align 4
  %194 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %195 = load %struct._jobject*, %struct._jobject** %5, align 4
  %196 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %197 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  %198 = call i64 %193(%struct.JNINativeInterface_** noundef %194, %struct._jobject* noundef %195, %struct._jmethodID* noundef %196, %union.jvalue* noundef %197)
  store i64 %198, i64* %12, align 8
  %199 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %199)
  %200 = load i64, i64* %12, align 8
  ret i64 %200
}

; Function Attrs: noinline nounwind optnone
define dso_local i64 @JNI_CallLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !48

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 54
  %194 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %199 = call i64 %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
  ret i64 %199
}

; Function Attrs: noinline nounwind optnone
define dso_local i64 @JNI_CallNonvirtualLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct.__va_list, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
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
  store i32 %24, i32* %12, align 4
  store i32 0, i32* %13, align 4
  br label %25

25:                                               ; preds = %188, %4
  %26 = load i32, i32* %13, align 4
  %27 = load i32, i32* %12, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %191

29:                                               ; preds = %25
  %30 = load i32, i32* %13, align 4
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %30
  %32 = load i8, i8* %31, align 1
  %33 = zext i8 %32 to i32
  %34 = icmp eq i32 %33, 90
  br i1 %34, label %35, label %45

35:                                               ; preds = %29
  %36 = bitcast %struct.__va_list* %9 to i8**
  %37 = load i8*, i8** %36, align 4
  %38 = getelementptr inbounds i8, i8* %37, i32 4
  store i8* %38, i8** %36, align 4
  %39 = bitcast i8* %37 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = load i32, i32* %13, align 4
  %43 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %42
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %41, i8* %44, align 8
  br label %187

45:                                               ; preds = %29
  %46 = load i32, i32* %13, align 4
  %47 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %46
  %48 = load i8, i8* %47, align 1
  %49 = zext i8 %48 to i32
  %50 = icmp eq i32 %49, 66
  br i1 %50, label %51, label %61

51:                                               ; preds = %45
  %52 = bitcast %struct.__va_list* %9 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load i32, i32* %13, align 4
  %59 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %58
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %57, i8* %60, align 8
  br label %186

61:                                               ; preds = %45
  %62 = load i32, i32* %13, align 4
  %63 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %62
  %64 = load i8, i8* %63, align 1
  %65 = zext i8 %64 to i32
  %66 = icmp eq i32 %65, 67
  br i1 %66, label %67, label %77

67:                                               ; preds = %61
  %68 = bitcast %struct.__va_list* %9 to i8**
  %69 = load i8*, i8** %68, align 4
  %70 = getelementptr inbounds i8, i8* %69, i32 4
  store i8* %70, i8** %68, align 4
  %71 = bitcast i8* %69 to i32*
  %72 = load i32, i32* %71, align 4
  %73 = trunc i32 %72 to i16
  %74 = load i32, i32* %13, align 4
  %75 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %74
  %76 = bitcast %union.jvalue* %75 to i16*
  store i16 %73, i16* %76, align 8
  br label %185

77:                                               ; preds = %61
  %78 = load i32, i32* %13, align 4
  %79 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %78
  %80 = load i8, i8* %79, align 1
  %81 = zext i8 %80 to i32
  %82 = icmp eq i32 %81, 83
  br i1 %82, label %83, label %93

83:                                               ; preds = %77
  %84 = bitcast %struct.__va_list* %9 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = trunc i32 %88 to i16
  %90 = load i32, i32* %13, align 4
  %91 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %90
  %92 = bitcast %union.jvalue* %91 to i16*
  store i16 %89, i16* %92, align 8
  br label %184

93:                                               ; preds = %77
  %94 = load i32, i32* %13, align 4
  %95 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %94
  %96 = load i8, i8* %95, align 1
  %97 = zext i8 %96 to i32
  %98 = icmp eq i32 %97, 73
  br i1 %98, label %99, label %108

99:                                               ; preds = %93
  %100 = bitcast %struct.__va_list* %9 to i8**
  %101 = load i8*, i8** %100, align 4
  %102 = getelementptr inbounds i8, i8* %101, i32 4
  store i8* %102, i8** %100, align 4
  %103 = bitcast i8* %101 to i32*
  %104 = load i32, i32* %103, align 4
  %105 = load i32, i32* %13, align 4
  %106 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %105
  %107 = bitcast %union.jvalue* %106 to i32*
  store i32 %104, i32* %107, align 8
  br label %183

108:                                              ; preds = %93
  %109 = load i32, i32* %13, align 4
  %110 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %109
  %111 = load i8, i8* %110, align 1
  %112 = zext i8 %111 to i32
  %113 = icmp eq i32 %112, 74
  br i1 %113, label %114, label %124

114:                                              ; preds = %108
  %115 = bitcast %struct.__va_list* %9 to i8**
  %116 = load i8*, i8** %115, align 4
  %117 = getelementptr inbounds i8, i8* %116, i32 4
  store i8* %117, i8** %115, align 4
  %118 = bitcast i8* %116 to i32*
  %119 = load i32, i32* %118, align 4
  %120 = sext i32 %119 to i64
  %121 = load i32, i32* %13, align 4
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %120, i64* %123, align 8
  br label %182

124:                                              ; preds = %108
  %125 = load i32, i32* %13, align 4
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %125
  %127 = load i8, i8* %126, align 1
  %128 = zext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %144

130:                                              ; preds = %124
  %131 = bitcast %struct.__va_list* %9 to i8**
  %132 = load i8*, i8** %131, align 4
  %133 = ptrtoint i8* %132 to i32
  %134 = add i32 %133, 7
  %135 = and i32 %134, -8
  %136 = inttoptr i32 %135 to i8*
  %137 = getelementptr inbounds i8, i8* %136, i32 8
  store i8* %137, i8** %131, align 4
  %138 = bitcast i8* %136 to double*
  %139 = load double, double* %138, align 8
  %140 = fptrunc double %139 to float
  %141 = load i32, i32* %13, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %140, float* %143, align 8
  br label %181

144:                                              ; preds = %124
  %145 = load i32, i32* %13, align 4
  %146 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %145
  %147 = load i8, i8* %146, align 1
  %148 = zext i8 %147 to i32
  %149 = icmp eq i32 %148, 68
  br i1 %149, label %150, label %163

150:                                              ; preds = %144
  %151 = bitcast %struct.__va_list* %9 to i8**
  %152 = load i8*, i8** %151, align 4
  %153 = ptrtoint i8* %152 to i32
  %154 = add i32 %153, 7
  %155 = and i32 %154, -8
  %156 = inttoptr i32 %155 to i8*
  %157 = getelementptr inbounds i8, i8* %156, i32 8
  store i8* %157, i8** %151, align 4
  %158 = bitcast i8* %156 to double*
  %159 = load double, double* %158, align 8
  %160 = load i32, i32* %13, align 4
  %161 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %160
  %162 = bitcast %union.jvalue* %161 to double*
  store double %159, double* %162, align 8
  br label %180

163:                                              ; preds = %144
  %164 = load i32, i32* %13, align 4
  %165 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %164
  %166 = load i8, i8* %165, align 1
  %167 = zext i8 %166 to i32
  %168 = icmp eq i32 %167, 76
  br i1 %168, label %169, label %179

169:                                              ; preds = %163
  %170 = bitcast %struct.__va_list* %9 to i8**
  %171 = load i8*, i8** %170, align 4
  %172 = getelementptr inbounds i8, i8* %171, i32 4
  store i8* %172, i8** %170, align 4
  %173 = bitcast i8* %171 to i8**
  %174 = load i8*, i8** %173, align 4
  %175 = bitcast i8* %174 to %struct._jobject*
  %176 = load i32, i32* %13, align 4
  %177 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %176
  %178 = bitcast %union.jvalue* %177 to %struct._jobject**
  store %struct._jobject* %175, %struct._jobject** %178, align 8
  br label %179

179:                                              ; preds = %169, %163
  br label %180

180:                                              ; preds = %179, %150
  br label %181

181:                                              ; preds = %180, %130
  br label %182

182:                                              ; preds = %181, %114
  br label %183

183:                                              ; preds = %182, %99
  br label %184

184:                                              ; preds = %183, %83
  br label %185

185:                                              ; preds = %184, %67
  br label %186

186:                                              ; preds = %185, %51
  br label %187

187:                                              ; preds = %186, %35
  br label %188

188:                                              ; preds = %187
  %189 = load i32, i32* %13, align 4
  %190 = add nsw i32 %189, 1
  store i32 %190, i32* %13, align 4
  br label %25, !llvm.loop !49

191:                                              ; preds = %25
  %192 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %193 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %192, align 4
  %194 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %193, i32 0, i32 84
  %195 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %194, align 4
  %196 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %197 = load %struct._jobject*, %struct._jobject** %6, align 4
  %198 = load %struct._jobject*, %struct._jobject** %7, align 4
  %199 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %200 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %201 = call i64 %195(%struct.JNINativeInterface_** noundef %196, %struct._jobject* noundef %197, %struct._jobject* noundef %198, %struct._jmethodID* noundef %199, %union.jvalue* noundef %200)
  store i64 %201, i64* %14, align 8
  %202 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %202)
  %203 = load i64, i64* %14, align 8
  ret i64 %203
}

; Function Attrs: noinline nounwind optnone
define dso_local i64 @JNI_CallNonvirtualLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
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
  store i32 %25, i32* %13, align 4
  store i32 0, i32* %14, align 4
  br label %26

26:                                               ; preds = %189, %5
  %27 = load i32, i32* %14, align 4
  %28 = load i32, i32* %13, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %192

30:                                               ; preds = %26
  %31 = load i32, i32* %14, align 4
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %31
  %33 = load i8, i8* %32, align 1
  %34 = zext i8 %33 to i32
  %35 = icmp eq i32 %34, 90
  br i1 %35, label %36, label %46

36:                                               ; preds = %30
  %37 = bitcast %struct.__va_list* %6 to i8**
  %38 = load i8*, i8** %37, align 4
  %39 = getelementptr inbounds i8, i8* %38, i32 4
  store i8* %39, i8** %37, align 4
  %40 = bitcast i8* %38 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = load i32, i32* %14, align 4
  %44 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %43
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %42, i8* %45, align 8
  br label %188

46:                                               ; preds = %30
  %47 = load i32, i32* %14, align 4
  %48 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %47
  %49 = load i8, i8* %48, align 1
  %50 = zext i8 %49 to i32
  %51 = icmp eq i32 %50, 66
  br i1 %51, label %52, label %62

52:                                               ; preds = %46
  %53 = bitcast %struct.__va_list* %6 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = load i32, i32* %14, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %58, i8* %61, align 8
  br label %187

62:                                               ; preds = %46
  %63 = load i32, i32* %14, align 4
  %64 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %63
  %65 = load i8, i8* %64, align 1
  %66 = zext i8 %65 to i32
  %67 = icmp eq i32 %66, 67
  br i1 %67, label %68, label %78

68:                                               ; preds = %62
  %69 = bitcast %struct.__va_list* %6 to i8**
  %70 = load i8*, i8** %69, align 4
  %71 = getelementptr inbounds i8, i8* %70, i32 4
  store i8* %71, i8** %69, align 4
  %72 = bitcast i8* %70 to i32*
  %73 = load i32, i32* %72, align 4
  %74 = trunc i32 %73 to i16
  %75 = load i32, i32* %14, align 4
  %76 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %75
  %77 = bitcast %union.jvalue* %76 to i16*
  store i16 %74, i16* %77, align 8
  br label %186

78:                                               ; preds = %62
  %79 = load i32, i32* %14, align 4
  %80 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %79
  %81 = load i8, i8* %80, align 1
  %82 = zext i8 %81 to i32
  %83 = icmp eq i32 %82, 83
  br i1 %83, label %84, label %94

84:                                               ; preds = %78
  %85 = bitcast %struct.__va_list* %6 to i8**
  %86 = load i8*, i8** %85, align 4
  %87 = getelementptr inbounds i8, i8* %86, i32 4
  store i8* %87, i8** %85, align 4
  %88 = bitcast i8* %86 to i32*
  %89 = load i32, i32* %88, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %14, align 4
  %92 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %91
  %93 = bitcast %union.jvalue* %92 to i16*
  store i16 %90, i16* %93, align 8
  br label %185

94:                                               ; preds = %78
  %95 = load i32, i32* %14, align 4
  %96 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %95
  %97 = load i8, i8* %96, align 1
  %98 = zext i8 %97 to i32
  %99 = icmp eq i32 %98, 73
  br i1 %99, label %100, label %109

100:                                              ; preds = %94
  %101 = bitcast %struct.__va_list* %6 to i8**
  %102 = load i8*, i8** %101, align 4
  %103 = getelementptr inbounds i8, i8* %102, i32 4
  store i8* %103, i8** %101, align 4
  %104 = bitcast i8* %102 to i32*
  %105 = load i32, i32* %104, align 4
  %106 = load i32, i32* %14, align 4
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %105, i32* %108, align 8
  br label %184

109:                                              ; preds = %94
  %110 = load i32, i32* %14, align 4
  %111 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %110
  %112 = load i8, i8* %111, align 1
  %113 = zext i8 %112 to i32
  %114 = icmp eq i32 %113, 74
  br i1 %114, label %115, label %125

115:                                              ; preds = %109
  %116 = bitcast %struct.__va_list* %6 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = getelementptr inbounds i8, i8* %117, i32 4
  store i8* %118, i8** %116, align 4
  %119 = bitcast i8* %117 to i32*
  %120 = load i32, i32* %119, align 4
  %121 = sext i32 %120 to i64
  %122 = load i32, i32* %14, align 4
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %121, i64* %124, align 8
  br label %183

125:                                              ; preds = %109
  %126 = load i32, i32* %14, align 4
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %126
  %128 = load i8, i8* %127, align 1
  %129 = zext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %145

131:                                              ; preds = %125
  %132 = bitcast %struct.__va_list* %6 to i8**
  %133 = load i8*, i8** %132, align 4
  %134 = ptrtoint i8* %133 to i32
  %135 = add i32 %134, 7
  %136 = and i32 %135, -8
  %137 = inttoptr i32 %136 to i8*
  %138 = getelementptr inbounds i8, i8* %137, i32 8
  store i8* %138, i8** %132, align 4
  %139 = bitcast i8* %137 to double*
  %140 = load double, double* %139, align 8
  %141 = fptrunc double %140 to float
  %142 = load i32, i32* %14, align 4
  %143 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %142
  %144 = bitcast %union.jvalue* %143 to float*
  store float %141, float* %144, align 8
  br label %182

145:                                              ; preds = %125
  %146 = load i32, i32* %14, align 4
  %147 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %146
  %148 = load i8, i8* %147, align 1
  %149 = zext i8 %148 to i32
  %150 = icmp eq i32 %149, 68
  br i1 %150, label %151, label %164

151:                                              ; preds = %145
  %152 = bitcast %struct.__va_list* %6 to i8**
  %153 = load i8*, i8** %152, align 4
  %154 = ptrtoint i8* %153 to i32
  %155 = add i32 %154, 7
  %156 = and i32 %155, -8
  %157 = inttoptr i32 %156 to i8*
  %158 = getelementptr inbounds i8, i8* %157, i32 8
  store i8* %158, i8** %152, align 4
  %159 = bitcast i8* %157 to double*
  %160 = load double, double* %159, align 8
  %161 = load i32, i32* %14, align 4
  %162 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %161
  %163 = bitcast %union.jvalue* %162 to double*
  store double %160, double* %163, align 8
  br label %181

164:                                              ; preds = %145
  %165 = load i32, i32* %14, align 4
  %166 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %165
  %167 = load i8, i8* %166, align 1
  %168 = zext i8 %167 to i32
  %169 = icmp eq i32 %168, 76
  br i1 %169, label %170, label %180

170:                                              ; preds = %164
  %171 = bitcast %struct.__va_list* %6 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = getelementptr inbounds i8, i8* %172, i32 4
  store i8* %173, i8** %171, align 4
  %174 = bitcast i8* %172 to i8**
  %175 = load i8*, i8** %174, align 4
  %176 = bitcast i8* %175 to %struct._jobject*
  %177 = load i32, i32* %14, align 4
  %178 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %177
  %179 = bitcast %union.jvalue* %178 to %struct._jobject**
  store %struct._jobject* %176, %struct._jobject** %179, align 8
  br label %180

180:                                              ; preds = %170, %164
  br label %181

181:                                              ; preds = %180, %151
  br label %182

182:                                              ; preds = %181, %131
  br label %183

183:                                              ; preds = %182, %115
  br label %184

184:                                              ; preds = %183, %100
  br label %185

185:                                              ; preds = %184, %84
  br label %186

186:                                              ; preds = %185, %68
  br label %187

187:                                              ; preds = %186, %52
  br label %188

188:                                              ; preds = %187, %36
  br label %189

189:                                              ; preds = %188
  %190 = load i32, i32* %14, align 4
  %191 = add nsw i32 %190, 1
  store i32 %191, i32* %14, align 4
  br label %26, !llvm.loop !50

192:                                              ; preds = %26
  %193 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %194 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %193, align 4
  %195 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %194, i32 0, i32 84
  %196 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %195, align 4
  %197 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %198 = load %struct._jobject*, %struct._jobject** %8, align 4
  %199 = load %struct._jobject*, %struct._jobject** %9, align 4
  %200 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %201 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 0
  %202 = call i64 %196(%struct.JNINativeInterface_** noundef %197, %struct._jobject* noundef %198, %struct._jobject* noundef %199, %struct._jmethodID* noundef %200, %union.jvalue* noundef %201)
  ret i64 %202
}

; Function Attrs: noinline nounwind optnone
define dso_local i64 @JNI_CallStaticLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %22, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %186, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %10, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %189

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  %32 = icmp eq i32 %31, 90
  br i1 %32, label %33, label %43

33:                                               ; preds = %27
  %34 = bitcast %struct.__va_list* %7 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %11, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %185

43:                                               ; preds = %27
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  %48 = icmp eq i32 %47, 66
  br i1 %48, label %49, label %59

49:                                               ; preds = %43
  %50 = bitcast %struct.__va_list* %7 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %55, i8* %58, align 8
  br label %184

59:                                               ; preds = %43
  %60 = load i32, i32* %11, align 4
  %61 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %60
  %62 = load i8, i8* %61, align 1
  %63 = zext i8 %62 to i32
  %64 = icmp eq i32 %63, 67
  br i1 %64, label %65, label %75

65:                                               ; preds = %59
  %66 = bitcast %struct.__va_list* %7 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %11, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %183

75:                                               ; preds = %59
  %76 = load i32, i32* %11, align 4
  %77 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %76
  %78 = load i8, i8* %77, align 1
  %79 = zext i8 %78 to i32
  %80 = icmp eq i32 %79, 83
  br i1 %80, label %81, label %91

81:                                               ; preds = %75
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i16*
  store i16 %87, i16* %90, align 8
  br label %182

91:                                               ; preds = %75
  %92 = load i32, i32* %11, align 4
  %93 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %92
  %94 = load i8, i8* %93, align 1
  %95 = zext i8 %94 to i32
  %96 = icmp eq i32 %95, 73
  br i1 %96, label %97, label %106

97:                                               ; preds = %91
  %98 = bitcast %struct.__va_list* %7 to i8**
  %99 = load i8*, i8** %98, align 4
  %100 = getelementptr inbounds i8, i8* %99, i32 4
  store i8* %100, i8** %98, align 4
  %101 = bitcast i8* %99 to i32*
  %102 = load i32, i32* %101, align 4
  %103 = load i32, i32* %11, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %102, i32* %105, align 8
  br label %181

106:                                              ; preds = %91
  %107 = load i32, i32* %11, align 4
  %108 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %107
  %109 = load i8, i8* %108, align 1
  %110 = zext i8 %109 to i32
  %111 = icmp eq i32 %110, 74
  br i1 %111, label %112, label %122

112:                                              ; preds = %106
  %113 = bitcast %struct.__va_list* %7 to i8**
  %114 = load i8*, i8** %113, align 4
  %115 = getelementptr inbounds i8, i8* %114, i32 4
  store i8* %115, i8** %113, align 4
  %116 = bitcast i8* %114 to i32*
  %117 = load i32, i32* %116, align 4
  %118 = sext i32 %117 to i64
  %119 = load i32, i32* %11, align 4
  %120 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %119
  %121 = bitcast %union.jvalue* %120 to i64*
  store i64 %118, i64* %121, align 8
  br label %180

122:                                              ; preds = %106
  %123 = load i32, i32* %11, align 4
  %124 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %123
  %125 = load i8, i8* %124, align 1
  %126 = zext i8 %125 to i32
  %127 = icmp eq i32 %126, 70
  br i1 %127, label %128, label %142

128:                                              ; preds = %122
  %129 = bitcast %struct.__va_list* %7 to i8**
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
  %139 = load i32, i32* %11, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %139
  %141 = bitcast %union.jvalue* %140 to float*
  store float %138, float* %141, align 8
  br label %179

142:                                              ; preds = %122
  %143 = load i32, i32* %11, align 4
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %143
  %145 = load i8, i8* %144, align 1
  %146 = zext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %161

148:                                              ; preds = %142
  %149 = bitcast %struct.__va_list* %7 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = ptrtoint i8* %150 to i32
  %152 = add i32 %151, 7
  %153 = and i32 %152, -8
  %154 = inttoptr i32 %153 to i8*
  %155 = getelementptr inbounds i8, i8* %154, i32 8
  store i8* %155, i8** %149, align 4
  %156 = bitcast i8* %154 to double*
  %157 = load double, double* %156, align 8
  %158 = load i32, i32* %11, align 4
  %159 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %158
  %160 = bitcast %union.jvalue* %159 to double*
  store double %157, double* %160, align 8
  br label %178

161:                                              ; preds = %142
  %162 = load i32, i32* %11, align 4
  %163 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %162
  %164 = load i8, i8* %163, align 1
  %165 = zext i8 %164 to i32
  %166 = icmp eq i32 %165, 76
  br i1 %166, label %167, label %177

167:                                              ; preds = %161
  %168 = bitcast %struct.__va_list* %7 to i8**
  %169 = load i8*, i8** %168, align 4
  %170 = getelementptr inbounds i8, i8* %169, i32 4
  store i8* %170, i8** %168, align 4
  %171 = bitcast i8* %169 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = bitcast i8* %172 to %struct._jobject*
  %174 = load i32, i32* %11, align 4
  %175 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %174
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %176, align 8
  br label %177

177:                                              ; preds = %167, %161
  br label %178

178:                                              ; preds = %177, %148
  br label %179

179:                                              ; preds = %178, %128
  br label %180

180:                                              ; preds = %179, %112
  br label %181

181:                                              ; preds = %180, %97
  br label %182

182:                                              ; preds = %181, %81
  br label %183

183:                                              ; preds = %182, %65
  br label %184

184:                                              ; preds = %183, %49
  br label %185

185:                                              ; preds = %184, %33
  br label %186

186:                                              ; preds = %185
  %187 = load i32, i32* %11, align 4
  %188 = add nsw i32 %187, 1
  store i32 %188, i32* %11, align 4
  br label %23, !llvm.loop !51

189:                                              ; preds = %23
  %190 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %191 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %190, align 4
  %192 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %191, i32 0, i32 134
  %193 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %192, align 4
  %194 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %195 = load %struct._jobject*, %struct._jobject** %5, align 4
  %196 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %197 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  %198 = call i64 %193(%struct.JNINativeInterface_** noundef %194, %struct._jobject* noundef %195, %struct._jmethodID* noundef %196, %union.jvalue* noundef %197)
  store i64 %198, i64* %12, align 8
  %199 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %199)
  %200 = load i64, i64* %12, align 8
  ret i64 %200
}

; Function Attrs: noinline nounwind optnone
define dso_local i64 @JNI_CallStaticLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !52

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 134
  %194 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %199 = call i64 %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
  ret i64 %199
}

; Function Attrs: noinline nounwind optnone
define dso_local float @JNI_CallFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %22, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %186, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %10, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %189

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  %32 = icmp eq i32 %31, 90
  br i1 %32, label %33, label %43

33:                                               ; preds = %27
  %34 = bitcast %struct.__va_list* %7 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %11, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %185

43:                                               ; preds = %27
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  %48 = icmp eq i32 %47, 66
  br i1 %48, label %49, label %59

49:                                               ; preds = %43
  %50 = bitcast %struct.__va_list* %7 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %55, i8* %58, align 8
  br label %184

59:                                               ; preds = %43
  %60 = load i32, i32* %11, align 4
  %61 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %60
  %62 = load i8, i8* %61, align 1
  %63 = zext i8 %62 to i32
  %64 = icmp eq i32 %63, 67
  br i1 %64, label %65, label %75

65:                                               ; preds = %59
  %66 = bitcast %struct.__va_list* %7 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %11, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %183

75:                                               ; preds = %59
  %76 = load i32, i32* %11, align 4
  %77 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %76
  %78 = load i8, i8* %77, align 1
  %79 = zext i8 %78 to i32
  %80 = icmp eq i32 %79, 83
  br i1 %80, label %81, label %91

81:                                               ; preds = %75
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i16*
  store i16 %87, i16* %90, align 8
  br label %182

91:                                               ; preds = %75
  %92 = load i32, i32* %11, align 4
  %93 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %92
  %94 = load i8, i8* %93, align 1
  %95 = zext i8 %94 to i32
  %96 = icmp eq i32 %95, 73
  br i1 %96, label %97, label %106

97:                                               ; preds = %91
  %98 = bitcast %struct.__va_list* %7 to i8**
  %99 = load i8*, i8** %98, align 4
  %100 = getelementptr inbounds i8, i8* %99, i32 4
  store i8* %100, i8** %98, align 4
  %101 = bitcast i8* %99 to i32*
  %102 = load i32, i32* %101, align 4
  %103 = load i32, i32* %11, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %102, i32* %105, align 8
  br label %181

106:                                              ; preds = %91
  %107 = load i32, i32* %11, align 4
  %108 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %107
  %109 = load i8, i8* %108, align 1
  %110 = zext i8 %109 to i32
  %111 = icmp eq i32 %110, 74
  br i1 %111, label %112, label %122

112:                                              ; preds = %106
  %113 = bitcast %struct.__va_list* %7 to i8**
  %114 = load i8*, i8** %113, align 4
  %115 = getelementptr inbounds i8, i8* %114, i32 4
  store i8* %115, i8** %113, align 4
  %116 = bitcast i8* %114 to i32*
  %117 = load i32, i32* %116, align 4
  %118 = sext i32 %117 to i64
  %119 = load i32, i32* %11, align 4
  %120 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %119
  %121 = bitcast %union.jvalue* %120 to i64*
  store i64 %118, i64* %121, align 8
  br label %180

122:                                              ; preds = %106
  %123 = load i32, i32* %11, align 4
  %124 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %123
  %125 = load i8, i8* %124, align 1
  %126 = zext i8 %125 to i32
  %127 = icmp eq i32 %126, 70
  br i1 %127, label %128, label %142

128:                                              ; preds = %122
  %129 = bitcast %struct.__va_list* %7 to i8**
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
  %139 = load i32, i32* %11, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %139
  %141 = bitcast %union.jvalue* %140 to float*
  store float %138, float* %141, align 8
  br label %179

142:                                              ; preds = %122
  %143 = load i32, i32* %11, align 4
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %143
  %145 = load i8, i8* %144, align 1
  %146 = zext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %161

148:                                              ; preds = %142
  %149 = bitcast %struct.__va_list* %7 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = ptrtoint i8* %150 to i32
  %152 = add i32 %151, 7
  %153 = and i32 %152, -8
  %154 = inttoptr i32 %153 to i8*
  %155 = getelementptr inbounds i8, i8* %154, i32 8
  store i8* %155, i8** %149, align 4
  %156 = bitcast i8* %154 to double*
  %157 = load double, double* %156, align 8
  %158 = load i32, i32* %11, align 4
  %159 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %158
  %160 = bitcast %union.jvalue* %159 to double*
  store double %157, double* %160, align 8
  br label %178

161:                                              ; preds = %142
  %162 = load i32, i32* %11, align 4
  %163 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %162
  %164 = load i8, i8* %163, align 1
  %165 = zext i8 %164 to i32
  %166 = icmp eq i32 %165, 76
  br i1 %166, label %167, label %177

167:                                              ; preds = %161
  %168 = bitcast %struct.__va_list* %7 to i8**
  %169 = load i8*, i8** %168, align 4
  %170 = getelementptr inbounds i8, i8* %169, i32 4
  store i8* %170, i8** %168, align 4
  %171 = bitcast i8* %169 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = bitcast i8* %172 to %struct._jobject*
  %174 = load i32, i32* %11, align 4
  %175 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %174
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %176, align 8
  br label %177

177:                                              ; preds = %167, %161
  br label %178

178:                                              ; preds = %177, %148
  br label %179

179:                                              ; preds = %178, %128
  br label %180

180:                                              ; preds = %179, %112
  br label %181

181:                                              ; preds = %180, %97
  br label %182

182:                                              ; preds = %181, %81
  br label %183

183:                                              ; preds = %182, %65
  br label %184

184:                                              ; preds = %183, %49
  br label %185

185:                                              ; preds = %184, %33
  br label %186

186:                                              ; preds = %185
  %187 = load i32, i32* %11, align 4
  %188 = add nsw i32 %187, 1
  store i32 %188, i32* %11, align 4
  br label %23, !llvm.loop !53

189:                                              ; preds = %23
  %190 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %191 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %190, align 4
  %192 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %191, i32 0, i32 57
  %193 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %192, align 4
  %194 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %195 = load %struct._jobject*, %struct._jobject** %5, align 4
  %196 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %197 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  %198 = call float %193(%struct.JNINativeInterface_** noundef %194, %struct._jobject* noundef %195, %struct._jmethodID* noundef %196, %union.jvalue* noundef %197)
  store float %198, float* %12, align 4
  %199 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %199)
  %200 = load float, float* %12, align 4
  ret float %200
}

; Function Attrs: noinline nounwind optnone
define dso_local float @JNI_CallFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !54

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 57
  %194 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %199 = call float %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
  ret float %199
}

; Function Attrs: noinline nounwind optnone
define dso_local float @JNI_CallNonvirtualFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct.__va_list, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
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
  store i32 %24, i32* %12, align 4
  store i32 0, i32* %13, align 4
  br label %25

25:                                               ; preds = %188, %4
  %26 = load i32, i32* %13, align 4
  %27 = load i32, i32* %12, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %191

29:                                               ; preds = %25
  %30 = load i32, i32* %13, align 4
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %30
  %32 = load i8, i8* %31, align 1
  %33 = zext i8 %32 to i32
  %34 = icmp eq i32 %33, 90
  br i1 %34, label %35, label %45

35:                                               ; preds = %29
  %36 = bitcast %struct.__va_list* %9 to i8**
  %37 = load i8*, i8** %36, align 4
  %38 = getelementptr inbounds i8, i8* %37, i32 4
  store i8* %38, i8** %36, align 4
  %39 = bitcast i8* %37 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = load i32, i32* %13, align 4
  %43 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %42
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %41, i8* %44, align 8
  br label %187

45:                                               ; preds = %29
  %46 = load i32, i32* %13, align 4
  %47 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %46
  %48 = load i8, i8* %47, align 1
  %49 = zext i8 %48 to i32
  %50 = icmp eq i32 %49, 66
  br i1 %50, label %51, label %61

51:                                               ; preds = %45
  %52 = bitcast %struct.__va_list* %9 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load i32, i32* %13, align 4
  %59 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %58
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %57, i8* %60, align 8
  br label %186

61:                                               ; preds = %45
  %62 = load i32, i32* %13, align 4
  %63 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %62
  %64 = load i8, i8* %63, align 1
  %65 = zext i8 %64 to i32
  %66 = icmp eq i32 %65, 67
  br i1 %66, label %67, label %77

67:                                               ; preds = %61
  %68 = bitcast %struct.__va_list* %9 to i8**
  %69 = load i8*, i8** %68, align 4
  %70 = getelementptr inbounds i8, i8* %69, i32 4
  store i8* %70, i8** %68, align 4
  %71 = bitcast i8* %69 to i32*
  %72 = load i32, i32* %71, align 4
  %73 = trunc i32 %72 to i16
  %74 = load i32, i32* %13, align 4
  %75 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %74
  %76 = bitcast %union.jvalue* %75 to i16*
  store i16 %73, i16* %76, align 8
  br label %185

77:                                               ; preds = %61
  %78 = load i32, i32* %13, align 4
  %79 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %78
  %80 = load i8, i8* %79, align 1
  %81 = zext i8 %80 to i32
  %82 = icmp eq i32 %81, 83
  br i1 %82, label %83, label %93

83:                                               ; preds = %77
  %84 = bitcast %struct.__va_list* %9 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = trunc i32 %88 to i16
  %90 = load i32, i32* %13, align 4
  %91 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %90
  %92 = bitcast %union.jvalue* %91 to i16*
  store i16 %89, i16* %92, align 8
  br label %184

93:                                               ; preds = %77
  %94 = load i32, i32* %13, align 4
  %95 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %94
  %96 = load i8, i8* %95, align 1
  %97 = zext i8 %96 to i32
  %98 = icmp eq i32 %97, 73
  br i1 %98, label %99, label %108

99:                                               ; preds = %93
  %100 = bitcast %struct.__va_list* %9 to i8**
  %101 = load i8*, i8** %100, align 4
  %102 = getelementptr inbounds i8, i8* %101, i32 4
  store i8* %102, i8** %100, align 4
  %103 = bitcast i8* %101 to i32*
  %104 = load i32, i32* %103, align 4
  %105 = load i32, i32* %13, align 4
  %106 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %105
  %107 = bitcast %union.jvalue* %106 to i32*
  store i32 %104, i32* %107, align 8
  br label %183

108:                                              ; preds = %93
  %109 = load i32, i32* %13, align 4
  %110 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %109
  %111 = load i8, i8* %110, align 1
  %112 = zext i8 %111 to i32
  %113 = icmp eq i32 %112, 74
  br i1 %113, label %114, label %124

114:                                              ; preds = %108
  %115 = bitcast %struct.__va_list* %9 to i8**
  %116 = load i8*, i8** %115, align 4
  %117 = getelementptr inbounds i8, i8* %116, i32 4
  store i8* %117, i8** %115, align 4
  %118 = bitcast i8* %116 to i32*
  %119 = load i32, i32* %118, align 4
  %120 = sext i32 %119 to i64
  %121 = load i32, i32* %13, align 4
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %120, i64* %123, align 8
  br label %182

124:                                              ; preds = %108
  %125 = load i32, i32* %13, align 4
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %125
  %127 = load i8, i8* %126, align 1
  %128 = zext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %144

130:                                              ; preds = %124
  %131 = bitcast %struct.__va_list* %9 to i8**
  %132 = load i8*, i8** %131, align 4
  %133 = ptrtoint i8* %132 to i32
  %134 = add i32 %133, 7
  %135 = and i32 %134, -8
  %136 = inttoptr i32 %135 to i8*
  %137 = getelementptr inbounds i8, i8* %136, i32 8
  store i8* %137, i8** %131, align 4
  %138 = bitcast i8* %136 to double*
  %139 = load double, double* %138, align 8
  %140 = fptrunc double %139 to float
  %141 = load i32, i32* %13, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %140, float* %143, align 8
  br label %181

144:                                              ; preds = %124
  %145 = load i32, i32* %13, align 4
  %146 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %145
  %147 = load i8, i8* %146, align 1
  %148 = zext i8 %147 to i32
  %149 = icmp eq i32 %148, 68
  br i1 %149, label %150, label %163

150:                                              ; preds = %144
  %151 = bitcast %struct.__va_list* %9 to i8**
  %152 = load i8*, i8** %151, align 4
  %153 = ptrtoint i8* %152 to i32
  %154 = add i32 %153, 7
  %155 = and i32 %154, -8
  %156 = inttoptr i32 %155 to i8*
  %157 = getelementptr inbounds i8, i8* %156, i32 8
  store i8* %157, i8** %151, align 4
  %158 = bitcast i8* %156 to double*
  %159 = load double, double* %158, align 8
  %160 = load i32, i32* %13, align 4
  %161 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %160
  %162 = bitcast %union.jvalue* %161 to double*
  store double %159, double* %162, align 8
  br label %180

163:                                              ; preds = %144
  %164 = load i32, i32* %13, align 4
  %165 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %164
  %166 = load i8, i8* %165, align 1
  %167 = zext i8 %166 to i32
  %168 = icmp eq i32 %167, 76
  br i1 %168, label %169, label %179

169:                                              ; preds = %163
  %170 = bitcast %struct.__va_list* %9 to i8**
  %171 = load i8*, i8** %170, align 4
  %172 = getelementptr inbounds i8, i8* %171, i32 4
  store i8* %172, i8** %170, align 4
  %173 = bitcast i8* %171 to i8**
  %174 = load i8*, i8** %173, align 4
  %175 = bitcast i8* %174 to %struct._jobject*
  %176 = load i32, i32* %13, align 4
  %177 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %176
  %178 = bitcast %union.jvalue* %177 to %struct._jobject**
  store %struct._jobject* %175, %struct._jobject** %178, align 8
  br label %179

179:                                              ; preds = %169, %163
  br label %180

180:                                              ; preds = %179, %150
  br label %181

181:                                              ; preds = %180, %130
  br label %182

182:                                              ; preds = %181, %114
  br label %183

183:                                              ; preds = %182, %99
  br label %184

184:                                              ; preds = %183, %83
  br label %185

185:                                              ; preds = %184, %67
  br label %186

186:                                              ; preds = %185, %51
  br label %187

187:                                              ; preds = %186, %35
  br label %188

188:                                              ; preds = %187
  %189 = load i32, i32* %13, align 4
  %190 = add nsw i32 %189, 1
  store i32 %190, i32* %13, align 4
  br label %25, !llvm.loop !55

191:                                              ; preds = %25
  %192 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %193 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %192, align 4
  %194 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %193, i32 0, i32 87
  %195 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %194, align 4
  %196 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %197 = load %struct._jobject*, %struct._jobject** %6, align 4
  %198 = load %struct._jobject*, %struct._jobject** %7, align 4
  %199 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %200 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %201 = call float %195(%struct.JNINativeInterface_** noundef %196, %struct._jobject* noundef %197, %struct._jobject* noundef %198, %struct._jmethodID* noundef %199, %union.jvalue* noundef %200)
  store float %201, float* %14, align 4
  %202 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %202)
  %203 = load float, float* %14, align 4
  ret float %203
}

; Function Attrs: noinline nounwind optnone
define dso_local float @JNI_CallNonvirtualFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
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
  store i32 %25, i32* %13, align 4
  store i32 0, i32* %14, align 4
  br label %26

26:                                               ; preds = %189, %5
  %27 = load i32, i32* %14, align 4
  %28 = load i32, i32* %13, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %192

30:                                               ; preds = %26
  %31 = load i32, i32* %14, align 4
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %31
  %33 = load i8, i8* %32, align 1
  %34 = zext i8 %33 to i32
  %35 = icmp eq i32 %34, 90
  br i1 %35, label %36, label %46

36:                                               ; preds = %30
  %37 = bitcast %struct.__va_list* %6 to i8**
  %38 = load i8*, i8** %37, align 4
  %39 = getelementptr inbounds i8, i8* %38, i32 4
  store i8* %39, i8** %37, align 4
  %40 = bitcast i8* %38 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = load i32, i32* %14, align 4
  %44 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %43
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %42, i8* %45, align 8
  br label %188

46:                                               ; preds = %30
  %47 = load i32, i32* %14, align 4
  %48 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %47
  %49 = load i8, i8* %48, align 1
  %50 = zext i8 %49 to i32
  %51 = icmp eq i32 %50, 66
  br i1 %51, label %52, label %62

52:                                               ; preds = %46
  %53 = bitcast %struct.__va_list* %6 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = load i32, i32* %14, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %58, i8* %61, align 8
  br label %187

62:                                               ; preds = %46
  %63 = load i32, i32* %14, align 4
  %64 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %63
  %65 = load i8, i8* %64, align 1
  %66 = zext i8 %65 to i32
  %67 = icmp eq i32 %66, 67
  br i1 %67, label %68, label %78

68:                                               ; preds = %62
  %69 = bitcast %struct.__va_list* %6 to i8**
  %70 = load i8*, i8** %69, align 4
  %71 = getelementptr inbounds i8, i8* %70, i32 4
  store i8* %71, i8** %69, align 4
  %72 = bitcast i8* %70 to i32*
  %73 = load i32, i32* %72, align 4
  %74 = trunc i32 %73 to i16
  %75 = load i32, i32* %14, align 4
  %76 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %75
  %77 = bitcast %union.jvalue* %76 to i16*
  store i16 %74, i16* %77, align 8
  br label %186

78:                                               ; preds = %62
  %79 = load i32, i32* %14, align 4
  %80 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %79
  %81 = load i8, i8* %80, align 1
  %82 = zext i8 %81 to i32
  %83 = icmp eq i32 %82, 83
  br i1 %83, label %84, label %94

84:                                               ; preds = %78
  %85 = bitcast %struct.__va_list* %6 to i8**
  %86 = load i8*, i8** %85, align 4
  %87 = getelementptr inbounds i8, i8* %86, i32 4
  store i8* %87, i8** %85, align 4
  %88 = bitcast i8* %86 to i32*
  %89 = load i32, i32* %88, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %14, align 4
  %92 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %91
  %93 = bitcast %union.jvalue* %92 to i16*
  store i16 %90, i16* %93, align 8
  br label %185

94:                                               ; preds = %78
  %95 = load i32, i32* %14, align 4
  %96 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %95
  %97 = load i8, i8* %96, align 1
  %98 = zext i8 %97 to i32
  %99 = icmp eq i32 %98, 73
  br i1 %99, label %100, label %109

100:                                              ; preds = %94
  %101 = bitcast %struct.__va_list* %6 to i8**
  %102 = load i8*, i8** %101, align 4
  %103 = getelementptr inbounds i8, i8* %102, i32 4
  store i8* %103, i8** %101, align 4
  %104 = bitcast i8* %102 to i32*
  %105 = load i32, i32* %104, align 4
  %106 = load i32, i32* %14, align 4
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %105, i32* %108, align 8
  br label %184

109:                                              ; preds = %94
  %110 = load i32, i32* %14, align 4
  %111 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %110
  %112 = load i8, i8* %111, align 1
  %113 = zext i8 %112 to i32
  %114 = icmp eq i32 %113, 74
  br i1 %114, label %115, label %125

115:                                              ; preds = %109
  %116 = bitcast %struct.__va_list* %6 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = getelementptr inbounds i8, i8* %117, i32 4
  store i8* %118, i8** %116, align 4
  %119 = bitcast i8* %117 to i32*
  %120 = load i32, i32* %119, align 4
  %121 = sext i32 %120 to i64
  %122 = load i32, i32* %14, align 4
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %121, i64* %124, align 8
  br label %183

125:                                              ; preds = %109
  %126 = load i32, i32* %14, align 4
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %126
  %128 = load i8, i8* %127, align 1
  %129 = zext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %145

131:                                              ; preds = %125
  %132 = bitcast %struct.__va_list* %6 to i8**
  %133 = load i8*, i8** %132, align 4
  %134 = ptrtoint i8* %133 to i32
  %135 = add i32 %134, 7
  %136 = and i32 %135, -8
  %137 = inttoptr i32 %136 to i8*
  %138 = getelementptr inbounds i8, i8* %137, i32 8
  store i8* %138, i8** %132, align 4
  %139 = bitcast i8* %137 to double*
  %140 = load double, double* %139, align 8
  %141 = fptrunc double %140 to float
  %142 = load i32, i32* %14, align 4
  %143 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %142
  %144 = bitcast %union.jvalue* %143 to float*
  store float %141, float* %144, align 8
  br label %182

145:                                              ; preds = %125
  %146 = load i32, i32* %14, align 4
  %147 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %146
  %148 = load i8, i8* %147, align 1
  %149 = zext i8 %148 to i32
  %150 = icmp eq i32 %149, 68
  br i1 %150, label %151, label %164

151:                                              ; preds = %145
  %152 = bitcast %struct.__va_list* %6 to i8**
  %153 = load i8*, i8** %152, align 4
  %154 = ptrtoint i8* %153 to i32
  %155 = add i32 %154, 7
  %156 = and i32 %155, -8
  %157 = inttoptr i32 %156 to i8*
  %158 = getelementptr inbounds i8, i8* %157, i32 8
  store i8* %158, i8** %152, align 4
  %159 = bitcast i8* %157 to double*
  %160 = load double, double* %159, align 8
  %161 = load i32, i32* %14, align 4
  %162 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %161
  %163 = bitcast %union.jvalue* %162 to double*
  store double %160, double* %163, align 8
  br label %181

164:                                              ; preds = %145
  %165 = load i32, i32* %14, align 4
  %166 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %165
  %167 = load i8, i8* %166, align 1
  %168 = zext i8 %167 to i32
  %169 = icmp eq i32 %168, 76
  br i1 %169, label %170, label %180

170:                                              ; preds = %164
  %171 = bitcast %struct.__va_list* %6 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = getelementptr inbounds i8, i8* %172, i32 4
  store i8* %173, i8** %171, align 4
  %174 = bitcast i8* %172 to i8**
  %175 = load i8*, i8** %174, align 4
  %176 = bitcast i8* %175 to %struct._jobject*
  %177 = load i32, i32* %14, align 4
  %178 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %177
  %179 = bitcast %union.jvalue* %178 to %struct._jobject**
  store %struct._jobject* %176, %struct._jobject** %179, align 8
  br label %180

180:                                              ; preds = %170, %164
  br label %181

181:                                              ; preds = %180, %151
  br label %182

182:                                              ; preds = %181, %131
  br label %183

183:                                              ; preds = %182, %115
  br label %184

184:                                              ; preds = %183, %100
  br label %185

185:                                              ; preds = %184, %84
  br label %186

186:                                              ; preds = %185, %68
  br label %187

187:                                              ; preds = %186, %52
  br label %188

188:                                              ; preds = %187, %36
  br label %189

189:                                              ; preds = %188
  %190 = load i32, i32* %14, align 4
  %191 = add nsw i32 %190, 1
  store i32 %191, i32* %14, align 4
  br label %26, !llvm.loop !56

192:                                              ; preds = %26
  %193 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %194 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %193, align 4
  %195 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %194, i32 0, i32 87
  %196 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %195, align 4
  %197 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %198 = load %struct._jobject*, %struct._jobject** %8, align 4
  %199 = load %struct._jobject*, %struct._jobject** %9, align 4
  %200 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %201 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 0
  %202 = call float %196(%struct.JNINativeInterface_** noundef %197, %struct._jobject* noundef %198, %struct._jobject* noundef %199, %struct._jmethodID* noundef %200, %union.jvalue* noundef %201)
  ret float %202
}

; Function Attrs: noinline nounwind optnone
define dso_local float @JNI_CallStaticFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %22, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %186, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %10, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %189

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  %32 = icmp eq i32 %31, 90
  br i1 %32, label %33, label %43

33:                                               ; preds = %27
  %34 = bitcast %struct.__va_list* %7 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %11, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %185

43:                                               ; preds = %27
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  %48 = icmp eq i32 %47, 66
  br i1 %48, label %49, label %59

49:                                               ; preds = %43
  %50 = bitcast %struct.__va_list* %7 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %55, i8* %58, align 8
  br label %184

59:                                               ; preds = %43
  %60 = load i32, i32* %11, align 4
  %61 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %60
  %62 = load i8, i8* %61, align 1
  %63 = zext i8 %62 to i32
  %64 = icmp eq i32 %63, 67
  br i1 %64, label %65, label %75

65:                                               ; preds = %59
  %66 = bitcast %struct.__va_list* %7 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %11, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %183

75:                                               ; preds = %59
  %76 = load i32, i32* %11, align 4
  %77 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %76
  %78 = load i8, i8* %77, align 1
  %79 = zext i8 %78 to i32
  %80 = icmp eq i32 %79, 83
  br i1 %80, label %81, label %91

81:                                               ; preds = %75
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i16*
  store i16 %87, i16* %90, align 8
  br label %182

91:                                               ; preds = %75
  %92 = load i32, i32* %11, align 4
  %93 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %92
  %94 = load i8, i8* %93, align 1
  %95 = zext i8 %94 to i32
  %96 = icmp eq i32 %95, 73
  br i1 %96, label %97, label %106

97:                                               ; preds = %91
  %98 = bitcast %struct.__va_list* %7 to i8**
  %99 = load i8*, i8** %98, align 4
  %100 = getelementptr inbounds i8, i8* %99, i32 4
  store i8* %100, i8** %98, align 4
  %101 = bitcast i8* %99 to i32*
  %102 = load i32, i32* %101, align 4
  %103 = load i32, i32* %11, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %102, i32* %105, align 8
  br label %181

106:                                              ; preds = %91
  %107 = load i32, i32* %11, align 4
  %108 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %107
  %109 = load i8, i8* %108, align 1
  %110 = zext i8 %109 to i32
  %111 = icmp eq i32 %110, 74
  br i1 %111, label %112, label %122

112:                                              ; preds = %106
  %113 = bitcast %struct.__va_list* %7 to i8**
  %114 = load i8*, i8** %113, align 4
  %115 = getelementptr inbounds i8, i8* %114, i32 4
  store i8* %115, i8** %113, align 4
  %116 = bitcast i8* %114 to i32*
  %117 = load i32, i32* %116, align 4
  %118 = sext i32 %117 to i64
  %119 = load i32, i32* %11, align 4
  %120 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %119
  %121 = bitcast %union.jvalue* %120 to i64*
  store i64 %118, i64* %121, align 8
  br label %180

122:                                              ; preds = %106
  %123 = load i32, i32* %11, align 4
  %124 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %123
  %125 = load i8, i8* %124, align 1
  %126 = zext i8 %125 to i32
  %127 = icmp eq i32 %126, 70
  br i1 %127, label %128, label %142

128:                                              ; preds = %122
  %129 = bitcast %struct.__va_list* %7 to i8**
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
  %139 = load i32, i32* %11, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %139
  %141 = bitcast %union.jvalue* %140 to float*
  store float %138, float* %141, align 8
  br label %179

142:                                              ; preds = %122
  %143 = load i32, i32* %11, align 4
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %143
  %145 = load i8, i8* %144, align 1
  %146 = zext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %161

148:                                              ; preds = %142
  %149 = bitcast %struct.__va_list* %7 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = ptrtoint i8* %150 to i32
  %152 = add i32 %151, 7
  %153 = and i32 %152, -8
  %154 = inttoptr i32 %153 to i8*
  %155 = getelementptr inbounds i8, i8* %154, i32 8
  store i8* %155, i8** %149, align 4
  %156 = bitcast i8* %154 to double*
  %157 = load double, double* %156, align 8
  %158 = load i32, i32* %11, align 4
  %159 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %158
  %160 = bitcast %union.jvalue* %159 to double*
  store double %157, double* %160, align 8
  br label %178

161:                                              ; preds = %142
  %162 = load i32, i32* %11, align 4
  %163 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %162
  %164 = load i8, i8* %163, align 1
  %165 = zext i8 %164 to i32
  %166 = icmp eq i32 %165, 76
  br i1 %166, label %167, label %177

167:                                              ; preds = %161
  %168 = bitcast %struct.__va_list* %7 to i8**
  %169 = load i8*, i8** %168, align 4
  %170 = getelementptr inbounds i8, i8* %169, i32 4
  store i8* %170, i8** %168, align 4
  %171 = bitcast i8* %169 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = bitcast i8* %172 to %struct._jobject*
  %174 = load i32, i32* %11, align 4
  %175 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %174
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %176, align 8
  br label %177

177:                                              ; preds = %167, %161
  br label %178

178:                                              ; preds = %177, %148
  br label %179

179:                                              ; preds = %178, %128
  br label %180

180:                                              ; preds = %179, %112
  br label %181

181:                                              ; preds = %180, %97
  br label %182

182:                                              ; preds = %181, %81
  br label %183

183:                                              ; preds = %182, %65
  br label %184

184:                                              ; preds = %183, %49
  br label %185

185:                                              ; preds = %184, %33
  br label %186

186:                                              ; preds = %185
  %187 = load i32, i32* %11, align 4
  %188 = add nsw i32 %187, 1
  store i32 %188, i32* %11, align 4
  br label %23, !llvm.loop !57

189:                                              ; preds = %23
  %190 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %191 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %190, align 4
  %192 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %191, i32 0, i32 137
  %193 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %192, align 4
  %194 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %195 = load %struct._jobject*, %struct._jobject** %5, align 4
  %196 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %197 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  %198 = call float %193(%struct.JNINativeInterface_** noundef %194, %struct._jobject* noundef %195, %struct._jmethodID* noundef %196, %union.jvalue* noundef %197)
  store float %198, float* %12, align 4
  %199 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %199)
  %200 = load float, float* %12, align 4
  ret float %200
}

; Function Attrs: noinline nounwind optnone
define dso_local float @JNI_CallStaticFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !58

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 137
  %194 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %199 = call float %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
  ret float %199
}

; Function Attrs: noinline nounwind optnone
define dso_local double @JNI_CallDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %22, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %186, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %10, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %189

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  %32 = icmp eq i32 %31, 90
  br i1 %32, label %33, label %43

33:                                               ; preds = %27
  %34 = bitcast %struct.__va_list* %7 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %11, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %185

43:                                               ; preds = %27
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  %48 = icmp eq i32 %47, 66
  br i1 %48, label %49, label %59

49:                                               ; preds = %43
  %50 = bitcast %struct.__va_list* %7 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %55, i8* %58, align 8
  br label %184

59:                                               ; preds = %43
  %60 = load i32, i32* %11, align 4
  %61 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %60
  %62 = load i8, i8* %61, align 1
  %63 = zext i8 %62 to i32
  %64 = icmp eq i32 %63, 67
  br i1 %64, label %65, label %75

65:                                               ; preds = %59
  %66 = bitcast %struct.__va_list* %7 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %11, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %183

75:                                               ; preds = %59
  %76 = load i32, i32* %11, align 4
  %77 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %76
  %78 = load i8, i8* %77, align 1
  %79 = zext i8 %78 to i32
  %80 = icmp eq i32 %79, 83
  br i1 %80, label %81, label %91

81:                                               ; preds = %75
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i16*
  store i16 %87, i16* %90, align 8
  br label %182

91:                                               ; preds = %75
  %92 = load i32, i32* %11, align 4
  %93 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %92
  %94 = load i8, i8* %93, align 1
  %95 = zext i8 %94 to i32
  %96 = icmp eq i32 %95, 73
  br i1 %96, label %97, label %106

97:                                               ; preds = %91
  %98 = bitcast %struct.__va_list* %7 to i8**
  %99 = load i8*, i8** %98, align 4
  %100 = getelementptr inbounds i8, i8* %99, i32 4
  store i8* %100, i8** %98, align 4
  %101 = bitcast i8* %99 to i32*
  %102 = load i32, i32* %101, align 4
  %103 = load i32, i32* %11, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %102, i32* %105, align 8
  br label %181

106:                                              ; preds = %91
  %107 = load i32, i32* %11, align 4
  %108 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %107
  %109 = load i8, i8* %108, align 1
  %110 = zext i8 %109 to i32
  %111 = icmp eq i32 %110, 74
  br i1 %111, label %112, label %122

112:                                              ; preds = %106
  %113 = bitcast %struct.__va_list* %7 to i8**
  %114 = load i8*, i8** %113, align 4
  %115 = getelementptr inbounds i8, i8* %114, i32 4
  store i8* %115, i8** %113, align 4
  %116 = bitcast i8* %114 to i32*
  %117 = load i32, i32* %116, align 4
  %118 = sext i32 %117 to i64
  %119 = load i32, i32* %11, align 4
  %120 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %119
  %121 = bitcast %union.jvalue* %120 to i64*
  store i64 %118, i64* %121, align 8
  br label %180

122:                                              ; preds = %106
  %123 = load i32, i32* %11, align 4
  %124 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %123
  %125 = load i8, i8* %124, align 1
  %126 = zext i8 %125 to i32
  %127 = icmp eq i32 %126, 70
  br i1 %127, label %128, label %142

128:                                              ; preds = %122
  %129 = bitcast %struct.__va_list* %7 to i8**
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
  %139 = load i32, i32* %11, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %139
  %141 = bitcast %union.jvalue* %140 to float*
  store float %138, float* %141, align 8
  br label %179

142:                                              ; preds = %122
  %143 = load i32, i32* %11, align 4
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %143
  %145 = load i8, i8* %144, align 1
  %146 = zext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %161

148:                                              ; preds = %142
  %149 = bitcast %struct.__va_list* %7 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = ptrtoint i8* %150 to i32
  %152 = add i32 %151, 7
  %153 = and i32 %152, -8
  %154 = inttoptr i32 %153 to i8*
  %155 = getelementptr inbounds i8, i8* %154, i32 8
  store i8* %155, i8** %149, align 4
  %156 = bitcast i8* %154 to double*
  %157 = load double, double* %156, align 8
  %158 = load i32, i32* %11, align 4
  %159 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %158
  %160 = bitcast %union.jvalue* %159 to double*
  store double %157, double* %160, align 8
  br label %178

161:                                              ; preds = %142
  %162 = load i32, i32* %11, align 4
  %163 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %162
  %164 = load i8, i8* %163, align 1
  %165 = zext i8 %164 to i32
  %166 = icmp eq i32 %165, 76
  br i1 %166, label %167, label %177

167:                                              ; preds = %161
  %168 = bitcast %struct.__va_list* %7 to i8**
  %169 = load i8*, i8** %168, align 4
  %170 = getelementptr inbounds i8, i8* %169, i32 4
  store i8* %170, i8** %168, align 4
  %171 = bitcast i8* %169 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = bitcast i8* %172 to %struct._jobject*
  %174 = load i32, i32* %11, align 4
  %175 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %174
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %176, align 8
  br label %177

177:                                              ; preds = %167, %161
  br label %178

178:                                              ; preds = %177, %148
  br label %179

179:                                              ; preds = %178, %128
  br label %180

180:                                              ; preds = %179, %112
  br label %181

181:                                              ; preds = %180, %97
  br label %182

182:                                              ; preds = %181, %81
  br label %183

183:                                              ; preds = %182, %65
  br label %184

184:                                              ; preds = %183, %49
  br label %185

185:                                              ; preds = %184, %33
  br label %186

186:                                              ; preds = %185
  %187 = load i32, i32* %11, align 4
  %188 = add nsw i32 %187, 1
  store i32 %188, i32* %11, align 4
  br label %23, !llvm.loop !59

189:                                              ; preds = %23
  %190 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %191 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %190, align 4
  %192 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %191, i32 0, i32 60
  %193 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %192, align 4
  %194 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %195 = load %struct._jobject*, %struct._jobject** %5, align 4
  %196 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %197 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  %198 = call double %193(%struct.JNINativeInterface_** noundef %194, %struct._jobject* noundef %195, %struct._jmethodID* noundef %196, %union.jvalue* noundef %197)
  store double %198, double* %12, align 8
  %199 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %199)
  %200 = load double, double* %12, align 8
  ret double %200
}

; Function Attrs: noinline nounwind optnone
define dso_local double @JNI_CallDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !60

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 60
  %194 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %199 = call double %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
  ret double %199
}

; Function Attrs: noinline nounwind optnone
define dso_local double @JNI_CallNonvirtualDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct.__va_list, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
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
  store i32 %24, i32* %12, align 4
  store i32 0, i32* %13, align 4
  br label %25

25:                                               ; preds = %188, %4
  %26 = load i32, i32* %13, align 4
  %27 = load i32, i32* %12, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %191

29:                                               ; preds = %25
  %30 = load i32, i32* %13, align 4
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %30
  %32 = load i8, i8* %31, align 1
  %33 = zext i8 %32 to i32
  %34 = icmp eq i32 %33, 90
  br i1 %34, label %35, label %45

35:                                               ; preds = %29
  %36 = bitcast %struct.__va_list* %9 to i8**
  %37 = load i8*, i8** %36, align 4
  %38 = getelementptr inbounds i8, i8* %37, i32 4
  store i8* %38, i8** %36, align 4
  %39 = bitcast i8* %37 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = load i32, i32* %13, align 4
  %43 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %42
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %41, i8* %44, align 8
  br label %187

45:                                               ; preds = %29
  %46 = load i32, i32* %13, align 4
  %47 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %46
  %48 = load i8, i8* %47, align 1
  %49 = zext i8 %48 to i32
  %50 = icmp eq i32 %49, 66
  br i1 %50, label %51, label %61

51:                                               ; preds = %45
  %52 = bitcast %struct.__va_list* %9 to i8**
  %53 = load i8*, i8** %52, align 4
  %54 = getelementptr inbounds i8, i8* %53, i32 4
  store i8* %54, i8** %52, align 4
  %55 = bitcast i8* %53 to i32*
  %56 = load i32, i32* %55, align 4
  %57 = trunc i32 %56 to i8
  %58 = load i32, i32* %13, align 4
  %59 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %58
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %57, i8* %60, align 8
  br label %186

61:                                               ; preds = %45
  %62 = load i32, i32* %13, align 4
  %63 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %62
  %64 = load i8, i8* %63, align 1
  %65 = zext i8 %64 to i32
  %66 = icmp eq i32 %65, 67
  br i1 %66, label %67, label %77

67:                                               ; preds = %61
  %68 = bitcast %struct.__va_list* %9 to i8**
  %69 = load i8*, i8** %68, align 4
  %70 = getelementptr inbounds i8, i8* %69, i32 4
  store i8* %70, i8** %68, align 4
  %71 = bitcast i8* %69 to i32*
  %72 = load i32, i32* %71, align 4
  %73 = trunc i32 %72 to i16
  %74 = load i32, i32* %13, align 4
  %75 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %74
  %76 = bitcast %union.jvalue* %75 to i16*
  store i16 %73, i16* %76, align 8
  br label %185

77:                                               ; preds = %61
  %78 = load i32, i32* %13, align 4
  %79 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %78
  %80 = load i8, i8* %79, align 1
  %81 = zext i8 %80 to i32
  %82 = icmp eq i32 %81, 83
  br i1 %82, label %83, label %93

83:                                               ; preds = %77
  %84 = bitcast %struct.__va_list* %9 to i8**
  %85 = load i8*, i8** %84, align 4
  %86 = getelementptr inbounds i8, i8* %85, i32 4
  store i8* %86, i8** %84, align 4
  %87 = bitcast i8* %85 to i32*
  %88 = load i32, i32* %87, align 4
  %89 = trunc i32 %88 to i16
  %90 = load i32, i32* %13, align 4
  %91 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %90
  %92 = bitcast %union.jvalue* %91 to i16*
  store i16 %89, i16* %92, align 8
  br label %184

93:                                               ; preds = %77
  %94 = load i32, i32* %13, align 4
  %95 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %94
  %96 = load i8, i8* %95, align 1
  %97 = zext i8 %96 to i32
  %98 = icmp eq i32 %97, 73
  br i1 %98, label %99, label %108

99:                                               ; preds = %93
  %100 = bitcast %struct.__va_list* %9 to i8**
  %101 = load i8*, i8** %100, align 4
  %102 = getelementptr inbounds i8, i8* %101, i32 4
  store i8* %102, i8** %100, align 4
  %103 = bitcast i8* %101 to i32*
  %104 = load i32, i32* %103, align 4
  %105 = load i32, i32* %13, align 4
  %106 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %105
  %107 = bitcast %union.jvalue* %106 to i32*
  store i32 %104, i32* %107, align 8
  br label %183

108:                                              ; preds = %93
  %109 = load i32, i32* %13, align 4
  %110 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %109
  %111 = load i8, i8* %110, align 1
  %112 = zext i8 %111 to i32
  %113 = icmp eq i32 %112, 74
  br i1 %113, label %114, label %124

114:                                              ; preds = %108
  %115 = bitcast %struct.__va_list* %9 to i8**
  %116 = load i8*, i8** %115, align 4
  %117 = getelementptr inbounds i8, i8* %116, i32 4
  store i8* %117, i8** %115, align 4
  %118 = bitcast i8* %116 to i32*
  %119 = load i32, i32* %118, align 4
  %120 = sext i32 %119 to i64
  %121 = load i32, i32* %13, align 4
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %120, i64* %123, align 8
  br label %182

124:                                              ; preds = %108
  %125 = load i32, i32* %13, align 4
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %125
  %127 = load i8, i8* %126, align 1
  %128 = zext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %144

130:                                              ; preds = %124
  %131 = bitcast %struct.__va_list* %9 to i8**
  %132 = load i8*, i8** %131, align 4
  %133 = ptrtoint i8* %132 to i32
  %134 = add i32 %133, 7
  %135 = and i32 %134, -8
  %136 = inttoptr i32 %135 to i8*
  %137 = getelementptr inbounds i8, i8* %136, i32 8
  store i8* %137, i8** %131, align 4
  %138 = bitcast i8* %136 to double*
  %139 = load double, double* %138, align 8
  %140 = fptrunc double %139 to float
  %141 = load i32, i32* %13, align 4
  %142 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %141
  %143 = bitcast %union.jvalue* %142 to float*
  store float %140, float* %143, align 8
  br label %181

144:                                              ; preds = %124
  %145 = load i32, i32* %13, align 4
  %146 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %145
  %147 = load i8, i8* %146, align 1
  %148 = zext i8 %147 to i32
  %149 = icmp eq i32 %148, 68
  br i1 %149, label %150, label %163

150:                                              ; preds = %144
  %151 = bitcast %struct.__va_list* %9 to i8**
  %152 = load i8*, i8** %151, align 4
  %153 = ptrtoint i8* %152 to i32
  %154 = add i32 %153, 7
  %155 = and i32 %154, -8
  %156 = inttoptr i32 %155 to i8*
  %157 = getelementptr inbounds i8, i8* %156, i32 8
  store i8* %157, i8** %151, align 4
  %158 = bitcast i8* %156 to double*
  %159 = load double, double* %158, align 8
  %160 = load i32, i32* %13, align 4
  %161 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %160
  %162 = bitcast %union.jvalue* %161 to double*
  store double %159, double* %162, align 8
  br label %180

163:                                              ; preds = %144
  %164 = load i32, i32* %13, align 4
  %165 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %164
  %166 = load i8, i8* %165, align 1
  %167 = zext i8 %166 to i32
  %168 = icmp eq i32 %167, 76
  br i1 %168, label %169, label %179

169:                                              ; preds = %163
  %170 = bitcast %struct.__va_list* %9 to i8**
  %171 = load i8*, i8** %170, align 4
  %172 = getelementptr inbounds i8, i8* %171, i32 4
  store i8* %172, i8** %170, align 4
  %173 = bitcast i8* %171 to i8**
  %174 = load i8*, i8** %173, align 4
  %175 = bitcast i8* %174 to %struct._jobject*
  %176 = load i32, i32* %13, align 4
  %177 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %176
  %178 = bitcast %union.jvalue* %177 to %struct._jobject**
  store %struct._jobject* %175, %struct._jobject** %178, align 8
  br label %179

179:                                              ; preds = %169, %163
  br label %180

180:                                              ; preds = %179, %150
  br label %181

181:                                              ; preds = %180, %130
  br label %182

182:                                              ; preds = %181, %114
  br label %183

183:                                              ; preds = %182, %99
  br label %184

184:                                              ; preds = %183, %83
  br label %185

185:                                              ; preds = %184, %67
  br label %186

186:                                              ; preds = %185, %51
  br label %187

187:                                              ; preds = %186, %35
  br label %188

188:                                              ; preds = %187
  %189 = load i32, i32* %13, align 4
  %190 = add nsw i32 %189, 1
  store i32 %190, i32* %13, align 4
  br label %25, !llvm.loop !61

191:                                              ; preds = %25
  %192 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %193 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %192, align 4
  %194 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %193, i32 0, i32 90
  %195 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %194, align 4
  %196 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %197 = load %struct._jobject*, %struct._jobject** %6, align 4
  %198 = load %struct._jobject*, %struct._jobject** %7, align 4
  %199 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %200 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  %201 = call double %195(%struct.JNINativeInterface_** noundef %196, %struct._jobject* noundef %197, %struct._jobject* noundef %198, %struct._jmethodID* noundef %199, %union.jvalue* noundef %200)
  store double %201, double* %14, align 8
  %202 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %202)
  %203 = load double, double* %14, align 8
  ret double %203
}

; Function Attrs: noinline nounwind optnone
define dso_local double @JNI_CallNonvirtualDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) #0 {
  %6 = alloca %struct.__va_list, align 4
  %7 = alloca %struct.JNINativeInterface_**, align 4
  %8 = alloca %struct._jobject*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct._jmethodID*, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
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
  store i32 %25, i32* %13, align 4
  store i32 0, i32* %14, align 4
  br label %26

26:                                               ; preds = %189, %5
  %27 = load i32, i32* %14, align 4
  %28 = load i32, i32* %13, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %192

30:                                               ; preds = %26
  %31 = load i32, i32* %14, align 4
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %31
  %33 = load i8, i8* %32, align 1
  %34 = zext i8 %33 to i32
  %35 = icmp eq i32 %34, 90
  br i1 %35, label %36, label %46

36:                                               ; preds = %30
  %37 = bitcast %struct.__va_list* %6 to i8**
  %38 = load i8*, i8** %37, align 4
  %39 = getelementptr inbounds i8, i8* %38, i32 4
  store i8* %39, i8** %37, align 4
  %40 = bitcast i8* %38 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = load i32, i32* %14, align 4
  %44 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %43
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %42, i8* %45, align 8
  br label %188

46:                                               ; preds = %30
  %47 = load i32, i32* %14, align 4
  %48 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %47
  %49 = load i8, i8* %48, align 1
  %50 = zext i8 %49 to i32
  %51 = icmp eq i32 %50, 66
  br i1 %51, label %52, label %62

52:                                               ; preds = %46
  %53 = bitcast %struct.__va_list* %6 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = load i32, i32* %14, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %58, i8* %61, align 8
  br label %187

62:                                               ; preds = %46
  %63 = load i32, i32* %14, align 4
  %64 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %63
  %65 = load i8, i8* %64, align 1
  %66 = zext i8 %65 to i32
  %67 = icmp eq i32 %66, 67
  br i1 %67, label %68, label %78

68:                                               ; preds = %62
  %69 = bitcast %struct.__va_list* %6 to i8**
  %70 = load i8*, i8** %69, align 4
  %71 = getelementptr inbounds i8, i8* %70, i32 4
  store i8* %71, i8** %69, align 4
  %72 = bitcast i8* %70 to i32*
  %73 = load i32, i32* %72, align 4
  %74 = trunc i32 %73 to i16
  %75 = load i32, i32* %14, align 4
  %76 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %75
  %77 = bitcast %union.jvalue* %76 to i16*
  store i16 %74, i16* %77, align 8
  br label %186

78:                                               ; preds = %62
  %79 = load i32, i32* %14, align 4
  %80 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %79
  %81 = load i8, i8* %80, align 1
  %82 = zext i8 %81 to i32
  %83 = icmp eq i32 %82, 83
  br i1 %83, label %84, label %94

84:                                               ; preds = %78
  %85 = bitcast %struct.__va_list* %6 to i8**
  %86 = load i8*, i8** %85, align 4
  %87 = getelementptr inbounds i8, i8* %86, i32 4
  store i8* %87, i8** %85, align 4
  %88 = bitcast i8* %86 to i32*
  %89 = load i32, i32* %88, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %14, align 4
  %92 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %91
  %93 = bitcast %union.jvalue* %92 to i16*
  store i16 %90, i16* %93, align 8
  br label %185

94:                                               ; preds = %78
  %95 = load i32, i32* %14, align 4
  %96 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %95
  %97 = load i8, i8* %96, align 1
  %98 = zext i8 %97 to i32
  %99 = icmp eq i32 %98, 73
  br i1 %99, label %100, label %109

100:                                              ; preds = %94
  %101 = bitcast %struct.__va_list* %6 to i8**
  %102 = load i8*, i8** %101, align 4
  %103 = getelementptr inbounds i8, i8* %102, i32 4
  store i8* %103, i8** %101, align 4
  %104 = bitcast i8* %102 to i32*
  %105 = load i32, i32* %104, align 4
  %106 = load i32, i32* %14, align 4
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %105, i32* %108, align 8
  br label %184

109:                                              ; preds = %94
  %110 = load i32, i32* %14, align 4
  %111 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %110
  %112 = load i8, i8* %111, align 1
  %113 = zext i8 %112 to i32
  %114 = icmp eq i32 %113, 74
  br i1 %114, label %115, label %125

115:                                              ; preds = %109
  %116 = bitcast %struct.__va_list* %6 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = getelementptr inbounds i8, i8* %117, i32 4
  store i8* %118, i8** %116, align 4
  %119 = bitcast i8* %117 to i32*
  %120 = load i32, i32* %119, align 4
  %121 = sext i32 %120 to i64
  %122 = load i32, i32* %14, align 4
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %121, i64* %124, align 8
  br label %183

125:                                              ; preds = %109
  %126 = load i32, i32* %14, align 4
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %126
  %128 = load i8, i8* %127, align 1
  %129 = zext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %145

131:                                              ; preds = %125
  %132 = bitcast %struct.__va_list* %6 to i8**
  %133 = load i8*, i8** %132, align 4
  %134 = ptrtoint i8* %133 to i32
  %135 = add i32 %134, 7
  %136 = and i32 %135, -8
  %137 = inttoptr i32 %136 to i8*
  %138 = getelementptr inbounds i8, i8* %137, i32 8
  store i8* %138, i8** %132, align 4
  %139 = bitcast i8* %137 to double*
  %140 = load double, double* %139, align 8
  %141 = fptrunc double %140 to float
  %142 = load i32, i32* %14, align 4
  %143 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %142
  %144 = bitcast %union.jvalue* %143 to float*
  store float %141, float* %144, align 8
  br label %182

145:                                              ; preds = %125
  %146 = load i32, i32* %14, align 4
  %147 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %146
  %148 = load i8, i8* %147, align 1
  %149 = zext i8 %148 to i32
  %150 = icmp eq i32 %149, 68
  br i1 %150, label %151, label %164

151:                                              ; preds = %145
  %152 = bitcast %struct.__va_list* %6 to i8**
  %153 = load i8*, i8** %152, align 4
  %154 = ptrtoint i8* %153 to i32
  %155 = add i32 %154, 7
  %156 = and i32 %155, -8
  %157 = inttoptr i32 %156 to i8*
  %158 = getelementptr inbounds i8, i8* %157, i32 8
  store i8* %158, i8** %152, align 4
  %159 = bitcast i8* %157 to double*
  %160 = load double, double* %159, align 8
  %161 = load i32, i32* %14, align 4
  %162 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %161
  %163 = bitcast %union.jvalue* %162 to double*
  store double %160, double* %163, align 8
  br label %181

164:                                              ; preds = %145
  %165 = load i32, i32* %14, align 4
  %166 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %165
  %167 = load i8, i8* %166, align 1
  %168 = zext i8 %167 to i32
  %169 = icmp eq i32 %168, 76
  br i1 %169, label %170, label %180

170:                                              ; preds = %164
  %171 = bitcast %struct.__va_list* %6 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = getelementptr inbounds i8, i8* %172, i32 4
  store i8* %173, i8** %171, align 4
  %174 = bitcast i8* %172 to i8**
  %175 = load i8*, i8** %174, align 4
  %176 = bitcast i8* %175 to %struct._jobject*
  %177 = load i32, i32* %14, align 4
  %178 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %177
  %179 = bitcast %union.jvalue* %178 to %struct._jobject**
  store %struct._jobject* %176, %struct._jobject** %179, align 8
  br label %180

180:                                              ; preds = %170, %164
  br label %181

181:                                              ; preds = %180, %151
  br label %182

182:                                              ; preds = %181, %131
  br label %183

183:                                              ; preds = %182, %115
  br label %184

184:                                              ; preds = %183, %100
  br label %185

185:                                              ; preds = %184, %84
  br label %186

186:                                              ; preds = %185, %68
  br label %187

187:                                              ; preds = %186, %52
  br label %188

188:                                              ; preds = %187, %36
  br label %189

189:                                              ; preds = %188
  %190 = load i32, i32* %14, align 4
  %191 = add nsw i32 %190, 1
  store i32 %191, i32* %14, align 4
  br label %26, !llvm.loop !62

192:                                              ; preds = %26
  %193 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %194 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %193, align 4
  %195 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %194, i32 0, i32 90
  %196 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %195, align 4
  %197 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %198 = load %struct._jobject*, %struct._jobject** %8, align 4
  %199 = load %struct._jobject*, %struct._jobject** %9, align 4
  %200 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %201 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 0
  %202 = call double %196(%struct.JNINativeInterface_** noundef %197, %struct._jobject* noundef %198, %struct._jobject* noundef %199, %struct._jmethodID* noundef %200, %union.jvalue* noundef %201)
  ret double %202
}

; Function Attrs: noinline nounwind optnone
define dso_local double @JNI_CallStaticDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %22, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %186, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %10, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %189

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  %32 = icmp eq i32 %31, 90
  br i1 %32, label %33, label %43

33:                                               ; preds = %27
  %34 = bitcast %struct.__va_list* %7 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %11, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %185

43:                                               ; preds = %27
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  %48 = icmp eq i32 %47, 66
  br i1 %48, label %49, label %59

49:                                               ; preds = %43
  %50 = bitcast %struct.__va_list* %7 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %55, i8* %58, align 8
  br label %184

59:                                               ; preds = %43
  %60 = load i32, i32* %11, align 4
  %61 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %60
  %62 = load i8, i8* %61, align 1
  %63 = zext i8 %62 to i32
  %64 = icmp eq i32 %63, 67
  br i1 %64, label %65, label %75

65:                                               ; preds = %59
  %66 = bitcast %struct.__va_list* %7 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %11, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %183

75:                                               ; preds = %59
  %76 = load i32, i32* %11, align 4
  %77 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %76
  %78 = load i8, i8* %77, align 1
  %79 = zext i8 %78 to i32
  %80 = icmp eq i32 %79, 83
  br i1 %80, label %81, label %91

81:                                               ; preds = %75
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i16*
  store i16 %87, i16* %90, align 8
  br label %182

91:                                               ; preds = %75
  %92 = load i32, i32* %11, align 4
  %93 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %92
  %94 = load i8, i8* %93, align 1
  %95 = zext i8 %94 to i32
  %96 = icmp eq i32 %95, 73
  br i1 %96, label %97, label %106

97:                                               ; preds = %91
  %98 = bitcast %struct.__va_list* %7 to i8**
  %99 = load i8*, i8** %98, align 4
  %100 = getelementptr inbounds i8, i8* %99, i32 4
  store i8* %100, i8** %98, align 4
  %101 = bitcast i8* %99 to i32*
  %102 = load i32, i32* %101, align 4
  %103 = load i32, i32* %11, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %102, i32* %105, align 8
  br label %181

106:                                              ; preds = %91
  %107 = load i32, i32* %11, align 4
  %108 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %107
  %109 = load i8, i8* %108, align 1
  %110 = zext i8 %109 to i32
  %111 = icmp eq i32 %110, 74
  br i1 %111, label %112, label %122

112:                                              ; preds = %106
  %113 = bitcast %struct.__va_list* %7 to i8**
  %114 = load i8*, i8** %113, align 4
  %115 = getelementptr inbounds i8, i8* %114, i32 4
  store i8* %115, i8** %113, align 4
  %116 = bitcast i8* %114 to i32*
  %117 = load i32, i32* %116, align 4
  %118 = sext i32 %117 to i64
  %119 = load i32, i32* %11, align 4
  %120 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %119
  %121 = bitcast %union.jvalue* %120 to i64*
  store i64 %118, i64* %121, align 8
  br label %180

122:                                              ; preds = %106
  %123 = load i32, i32* %11, align 4
  %124 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %123
  %125 = load i8, i8* %124, align 1
  %126 = zext i8 %125 to i32
  %127 = icmp eq i32 %126, 70
  br i1 %127, label %128, label %142

128:                                              ; preds = %122
  %129 = bitcast %struct.__va_list* %7 to i8**
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
  %139 = load i32, i32* %11, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %139
  %141 = bitcast %union.jvalue* %140 to float*
  store float %138, float* %141, align 8
  br label %179

142:                                              ; preds = %122
  %143 = load i32, i32* %11, align 4
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %143
  %145 = load i8, i8* %144, align 1
  %146 = zext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %161

148:                                              ; preds = %142
  %149 = bitcast %struct.__va_list* %7 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = ptrtoint i8* %150 to i32
  %152 = add i32 %151, 7
  %153 = and i32 %152, -8
  %154 = inttoptr i32 %153 to i8*
  %155 = getelementptr inbounds i8, i8* %154, i32 8
  store i8* %155, i8** %149, align 4
  %156 = bitcast i8* %154 to double*
  %157 = load double, double* %156, align 8
  %158 = load i32, i32* %11, align 4
  %159 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %158
  %160 = bitcast %union.jvalue* %159 to double*
  store double %157, double* %160, align 8
  br label %178

161:                                              ; preds = %142
  %162 = load i32, i32* %11, align 4
  %163 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %162
  %164 = load i8, i8* %163, align 1
  %165 = zext i8 %164 to i32
  %166 = icmp eq i32 %165, 76
  br i1 %166, label %167, label %177

167:                                              ; preds = %161
  %168 = bitcast %struct.__va_list* %7 to i8**
  %169 = load i8*, i8** %168, align 4
  %170 = getelementptr inbounds i8, i8* %169, i32 4
  store i8* %170, i8** %168, align 4
  %171 = bitcast i8* %169 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = bitcast i8* %172 to %struct._jobject*
  %174 = load i32, i32* %11, align 4
  %175 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %174
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %176, align 8
  br label %177

177:                                              ; preds = %167, %161
  br label %178

178:                                              ; preds = %177, %148
  br label %179

179:                                              ; preds = %178, %128
  br label %180

180:                                              ; preds = %179, %112
  br label %181

181:                                              ; preds = %180, %97
  br label %182

182:                                              ; preds = %181, %81
  br label %183

183:                                              ; preds = %182, %65
  br label %184

184:                                              ; preds = %183, %49
  br label %185

185:                                              ; preds = %184, %33
  br label %186

186:                                              ; preds = %185
  %187 = load i32, i32* %11, align 4
  %188 = add nsw i32 %187, 1
  store i32 %188, i32* %11, align 4
  br label %23, !llvm.loop !63

189:                                              ; preds = %23
  %190 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %191 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %190, align 4
  %192 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %191, i32 0, i32 140
  %193 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %192, align 4
  %194 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %195 = load %struct._jobject*, %struct._jobject** %5, align 4
  %196 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %197 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  %198 = call double %193(%struct.JNINativeInterface_** noundef %194, %struct._jobject* noundef %195, %struct._jmethodID* noundef %196, %union.jvalue* noundef %197)
  store double %198, double* %12, align 8
  %199 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %199)
  %200 = load double, double* %12, align 8
  ret double %200
}

; Function Attrs: noinline nounwind optnone
define dso_local double @JNI_CallStaticDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !64

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 140
  %194 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %199 = call double %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
  ret double %199
}

; Function Attrs: noinline nounwind optnone
define dso_local %struct._jobject* @JNI_NewObject(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %22, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %23

23:                                               ; preds = %186, %3
  %24 = load i32, i32* %11, align 4
  %25 = load i32, i32* %10, align 4
  %26 = icmp slt i32 %24, %25
  br i1 %26, label %27, label %189

27:                                               ; preds = %23
  %28 = load i32, i32* %11, align 4
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %28
  %30 = load i8, i8* %29, align 1
  %31 = zext i8 %30 to i32
  %32 = icmp eq i32 %31, 90
  br i1 %32, label %33, label %43

33:                                               ; preds = %27
  %34 = bitcast %struct.__va_list* %7 to i8**
  %35 = load i8*, i8** %34, align 4
  %36 = getelementptr inbounds i8, i8* %35, i32 4
  store i8* %36, i8** %34, align 4
  %37 = bitcast i8* %35 to i32*
  %38 = load i32, i32* %37, align 4
  %39 = trunc i32 %38 to i8
  %40 = load i32, i32* %11, align 4
  %41 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %40
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %39, i8* %42, align 8
  br label %185

43:                                               ; preds = %27
  %44 = load i32, i32* %11, align 4
  %45 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %44
  %46 = load i8, i8* %45, align 1
  %47 = zext i8 %46 to i32
  %48 = icmp eq i32 %47, 66
  br i1 %48, label %49, label %59

49:                                               ; preds = %43
  %50 = bitcast %struct.__va_list* %7 to i8**
  %51 = load i8*, i8** %50, align 4
  %52 = getelementptr inbounds i8, i8* %51, i32 4
  store i8* %52, i8** %50, align 4
  %53 = bitcast i8* %51 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = trunc i32 %54 to i8
  %56 = load i32, i32* %11, align 4
  %57 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %56
  %58 = bitcast %union.jvalue* %57 to i8*
  store i8 %55, i8* %58, align 8
  br label %184

59:                                               ; preds = %43
  %60 = load i32, i32* %11, align 4
  %61 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %60
  %62 = load i8, i8* %61, align 1
  %63 = zext i8 %62 to i32
  %64 = icmp eq i32 %63, 67
  br i1 %64, label %65, label %75

65:                                               ; preds = %59
  %66 = bitcast %struct.__va_list* %7 to i8**
  %67 = load i8*, i8** %66, align 4
  %68 = getelementptr inbounds i8, i8* %67, i32 4
  store i8* %68, i8** %66, align 4
  %69 = bitcast i8* %67 to i32*
  %70 = load i32, i32* %69, align 4
  %71 = trunc i32 %70 to i16
  %72 = load i32, i32* %11, align 4
  %73 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %72
  %74 = bitcast %union.jvalue* %73 to i16*
  store i16 %71, i16* %74, align 8
  br label %183

75:                                               ; preds = %59
  %76 = load i32, i32* %11, align 4
  %77 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %76
  %78 = load i8, i8* %77, align 1
  %79 = zext i8 %78 to i32
  %80 = icmp eq i32 %79, 83
  br i1 %80, label %81, label %91

81:                                               ; preds = %75
  %82 = bitcast %struct.__va_list* %7 to i8**
  %83 = load i8*, i8** %82, align 4
  %84 = getelementptr inbounds i8, i8* %83, i32 4
  store i8* %84, i8** %82, align 4
  %85 = bitcast i8* %83 to i32*
  %86 = load i32, i32* %85, align 4
  %87 = trunc i32 %86 to i16
  %88 = load i32, i32* %11, align 4
  %89 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %88
  %90 = bitcast %union.jvalue* %89 to i16*
  store i16 %87, i16* %90, align 8
  br label %182

91:                                               ; preds = %75
  %92 = load i32, i32* %11, align 4
  %93 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %92
  %94 = load i8, i8* %93, align 1
  %95 = zext i8 %94 to i32
  %96 = icmp eq i32 %95, 73
  br i1 %96, label %97, label %106

97:                                               ; preds = %91
  %98 = bitcast %struct.__va_list* %7 to i8**
  %99 = load i8*, i8** %98, align 4
  %100 = getelementptr inbounds i8, i8* %99, i32 4
  store i8* %100, i8** %98, align 4
  %101 = bitcast i8* %99 to i32*
  %102 = load i32, i32* %101, align 4
  %103 = load i32, i32* %11, align 4
  %104 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %103
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %102, i32* %105, align 8
  br label %181

106:                                              ; preds = %91
  %107 = load i32, i32* %11, align 4
  %108 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %107
  %109 = load i8, i8* %108, align 1
  %110 = zext i8 %109 to i32
  %111 = icmp eq i32 %110, 74
  br i1 %111, label %112, label %122

112:                                              ; preds = %106
  %113 = bitcast %struct.__va_list* %7 to i8**
  %114 = load i8*, i8** %113, align 4
  %115 = getelementptr inbounds i8, i8* %114, i32 4
  store i8* %115, i8** %113, align 4
  %116 = bitcast i8* %114 to i32*
  %117 = load i32, i32* %116, align 4
  %118 = sext i32 %117 to i64
  %119 = load i32, i32* %11, align 4
  %120 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %119
  %121 = bitcast %union.jvalue* %120 to i64*
  store i64 %118, i64* %121, align 8
  br label %180

122:                                              ; preds = %106
  %123 = load i32, i32* %11, align 4
  %124 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %123
  %125 = load i8, i8* %124, align 1
  %126 = zext i8 %125 to i32
  %127 = icmp eq i32 %126, 70
  br i1 %127, label %128, label %142

128:                                              ; preds = %122
  %129 = bitcast %struct.__va_list* %7 to i8**
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
  %139 = load i32, i32* %11, align 4
  %140 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %139
  %141 = bitcast %union.jvalue* %140 to float*
  store float %138, float* %141, align 8
  br label %179

142:                                              ; preds = %122
  %143 = load i32, i32* %11, align 4
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %143
  %145 = load i8, i8* %144, align 1
  %146 = zext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %161

148:                                              ; preds = %142
  %149 = bitcast %struct.__va_list* %7 to i8**
  %150 = load i8*, i8** %149, align 4
  %151 = ptrtoint i8* %150 to i32
  %152 = add i32 %151, 7
  %153 = and i32 %152, -8
  %154 = inttoptr i32 %153 to i8*
  %155 = getelementptr inbounds i8, i8* %154, i32 8
  store i8* %155, i8** %149, align 4
  %156 = bitcast i8* %154 to double*
  %157 = load double, double* %156, align 8
  %158 = load i32, i32* %11, align 4
  %159 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %158
  %160 = bitcast %union.jvalue* %159 to double*
  store double %157, double* %160, align 8
  br label %178

161:                                              ; preds = %142
  %162 = load i32, i32* %11, align 4
  %163 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %162
  %164 = load i8, i8* %163, align 1
  %165 = zext i8 %164 to i32
  %166 = icmp eq i32 %165, 76
  br i1 %166, label %167, label %177

167:                                              ; preds = %161
  %168 = bitcast %struct.__va_list* %7 to i8**
  %169 = load i8*, i8** %168, align 4
  %170 = getelementptr inbounds i8, i8* %169, i32 4
  store i8* %170, i8** %168, align 4
  %171 = bitcast i8* %169 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = bitcast i8* %172 to %struct._jobject*
  %174 = load i32, i32* %11, align 4
  %175 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %174
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %176, align 8
  br label %177

177:                                              ; preds = %167, %161
  br label %178

178:                                              ; preds = %177, %148
  br label %179

179:                                              ; preds = %178, %128
  br label %180

180:                                              ; preds = %179, %112
  br label %181

181:                                              ; preds = %180, %97
  br label %182

182:                                              ; preds = %181, %81
  br label %183

183:                                              ; preds = %182, %65
  br label %184

184:                                              ; preds = %183, %49
  br label %185

185:                                              ; preds = %184, %33
  br label %186

186:                                              ; preds = %185
  %187 = load i32, i32* %11, align 4
  %188 = add nsw i32 %187, 1
  store i32 %188, i32* %11, align 4
  br label %23, !llvm.loop !65

189:                                              ; preds = %23
  %190 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %191 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %190, align 4
  %192 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %191, i32 0, i32 30
  %193 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %192, align 4
  %194 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %195 = load %struct._jobject*, %struct._jobject** %5, align 4
  %196 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %197 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  %198 = call %struct._jobject* %193(%struct.JNINativeInterface_** noundef %194, %struct._jobject* noundef %195, %struct._jmethodID* noundef %196, %union.jvalue* noundef %197)
  store %struct._jobject* %198, %struct._jobject** %12, align 4
  %199 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %199)
  %200 = load %struct._jobject*, %struct._jobject** %12, align 4
  ret %struct._jobject* %200
}

; Function Attrs: noinline nounwind optnone
define dso_local %struct._jobject* @JNI_NewObjectV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !66

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 30
  %194 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  %199 = call %struct._jobject* %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
  ret %struct._jobject* %199
}

; Function Attrs: noinline nounwind optnone
define dso_local void @JNI_CallVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %21, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %22

22:                                               ; preds = %185, %3
  %23 = load i32, i32* %11, align 4
  %24 = load i32, i32* %10, align 4
  %25 = icmp slt i32 %23, %24
  br i1 %25, label %26, label %188

26:                                               ; preds = %22
  %27 = load i32, i32* %11, align 4
  %28 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %27
  %29 = load i8, i8* %28, align 1
  %30 = zext i8 %29 to i32
  %31 = icmp eq i32 %30, 90
  br i1 %31, label %32, label %42

32:                                               ; preds = %26
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %184

42:                                               ; preds = %26
  %43 = load i32, i32* %11, align 4
  %44 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %43
  %45 = load i8, i8* %44, align 1
  %46 = zext i8 %45 to i32
  %47 = icmp eq i32 %46, 66
  br i1 %47, label %48, label %58

48:                                               ; preds = %42
  %49 = bitcast %struct.__va_list* %7 to i8**
  %50 = load i8*, i8** %49, align 4
  %51 = getelementptr inbounds i8, i8* %50, i32 4
  store i8* %51, i8** %49, align 4
  %52 = bitcast i8* %50 to i32*
  %53 = load i32, i32* %52, align 4
  %54 = trunc i32 %53 to i8
  %55 = load i32, i32* %11, align 4
  %56 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %55
  %57 = bitcast %union.jvalue* %56 to i8*
  store i8 %54, i8* %57, align 8
  br label %183

58:                                               ; preds = %42
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %59
  %61 = load i8, i8* %60, align 1
  %62 = zext i8 %61 to i32
  %63 = icmp eq i32 %62, 67
  br i1 %63, label %64, label %74

64:                                               ; preds = %58
  %65 = bitcast %struct.__va_list* %7 to i8**
  %66 = load i8*, i8** %65, align 4
  %67 = getelementptr inbounds i8, i8* %66, i32 4
  store i8* %67, i8** %65, align 4
  %68 = bitcast i8* %66 to i32*
  %69 = load i32, i32* %68, align 4
  %70 = trunc i32 %69 to i16
  %71 = load i32, i32* %11, align 4
  %72 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %71
  %73 = bitcast %union.jvalue* %72 to i16*
  store i16 %70, i16* %73, align 8
  br label %182

74:                                               ; preds = %58
  %75 = load i32, i32* %11, align 4
  %76 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %75
  %77 = load i8, i8* %76, align 1
  %78 = zext i8 %77 to i32
  %79 = icmp eq i32 %78, 83
  br i1 %79, label %80, label %90

80:                                               ; preds = %74
  %81 = bitcast %struct.__va_list* %7 to i8**
  %82 = load i8*, i8** %81, align 4
  %83 = getelementptr inbounds i8, i8* %82, i32 4
  store i8* %83, i8** %81, align 4
  %84 = bitcast i8* %82 to i32*
  %85 = load i32, i32* %84, align 4
  %86 = trunc i32 %85 to i16
  %87 = load i32, i32* %11, align 4
  %88 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %87
  %89 = bitcast %union.jvalue* %88 to i16*
  store i16 %86, i16* %89, align 8
  br label %181

90:                                               ; preds = %74
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %91
  %93 = load i8, i8* %92, align 1
  %94 = zext i8 %93 to i32
  %95 = icmp eq i32 %94, 73
  br i1 %95, label %96, label %105

96:                                               ; preds = %90
  %97 = bitcast %struct.__va_list* %7 to i8**
  %98 = load i8*, i8** %97, align 4
  %99 = getelementptr inbounds i8, i8* %98, i32 4
  store i8* %99, i8** %97, align 4
  %100 = bitcast i8* %98 to i32*
  %101 = load i32, i32* %100, align 4
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %101, i32* %104, align 8
  br label %180

105:                                              ; preds = %90
  %106 = load i32, i32* %11, align 4
  %107 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %106
  %108 = load i8, i8* %107, align 1
  %109 = zext i8 %108 to i32
  %110 = icmp eq i32 %109, 74
  br i1 %110, label %111, label %121

111:                                              ; preds = %105
  %112 = bitcast %struct.__va_list* %7 to i8**
  %113 = load i8*, i8** %112, align 4
  %114 = getelementptr inbounds i8, i8* %113, i32 4
  store i8* %114, i8** %112, align 4
  %115 = bitcast i8* %113 to i32*
  %116 = load i32, i32* %115, align 4
  %117 = sext i32 %116 to i64
  %118 = load i32, i32* %11, align 4
  %119 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %118
  %120 = bitcast %union.jvalue* %119 to i64*
  store i64 %117, i64* %120, align 8
  br label %179

121:                                              ; preds = %105
  %122 = load i32, i32* %11, align 4
  %123 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %122
  %124 = load i8, i8* %123, align 1
  %125 = zext i8 %124 to i32
  %126 = icmp eq i32 %125, 70
  br i1 %126, label %127, label %141

127:                                              ; preds = %121
  %128 = bitcast %struct.__va_list* %7 to i8**
  %129 = load i8*, i8** %128, align 4
  %130 = ptrtoint i8* %129 to i32
  %131 = add i32 %130, 7
  %132 = and i32 %131, -8
  %133 = inttoptr i32 %132 to i8*
  %134 = getelementptr inbounds i8, i8* %133, i32 8
  store i8* %134, i8** %128, align 4
  %135 = bitcast i8* %133 to double*
  %136 = load double, double* %135, align 8
  %137 = fptrunc double %136 to float
  %138 = load i32, i32* %11, align 4
  %139 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %138
  %140 = bitcast %union.jvalue* %139 to float*
  store float %137, float* %140, align 8
  br label %178

141:                                              ; preds = %121
  %142 = load i32, i32* %11, align 4
  %143 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %142
  %144 = load i8, i8* %143, align 1
  %145 = zext i8 %144 to i32
  %146 = icmp eq i32 %145, 68
  br i1 %146, label %147, label %160

147:                                              ; preds = %141
  %148 = bitcast %struct.__va_list* %7 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = ptrtoint i8* %149 to i32
  %151 = add i32 %150, 7
  %152 = and i32 %151, -8
  %153 = inttoptr i32 %152 to i8*
  %154 = getelementptr inbounds i8, i8* %153, i32 8
  store i8* %154, i8** %148, align 4
  %155 = bitcast i8* %153 to double*
  %156 = load double, double* %155, align 8
  %157 = load i32, i32* %11, align 4
  %158 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %157
  %159 = bitcast %union.jvalue* %158 to double*
  store double %156, double* %159, align 8
  br label %177

160:                                              ; preds = %141
  %161 = load i32, i32* %11, align 4
  %162 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %161
  %163 = load i8, i8* %162, align 1
  %164 = zext i8 %163 to i32
  %165 = icmp eq i32 %164, 76
  br i1 %165, label %166, label %176

166:                                              ; preds = %160
  %167 = bitcast %struct.__va_list* %7 to i8**
  %168 = load i8*, i8** %167, align 4
  %169 = getelementptr inbounds i8, i8* %168, i32 4
  store i8* %169, i8** %167, align 4
  %170 = bitcast i8* %168 to i8**
  %171 = load i8*, i8** %170, align 4
  %172 = bitcast i8* %171 to %struct._jobject*
  %173 = load i32, i32* %11, align 4
  %174 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %173
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %172, %struct._jobject** %175, align 8
  br label %176

176:                                              ; preds = %166, %160
  br label %177

177:                                              ; preds = %176, %147
  br label %178

178:                                              ; preds = %177, %127
  br label %179

179:                                              ; preds = %178, %111
  br label %180

180:                                              ; preds = %179, %96
  br label %181

181:                                              ; preds = %180, %80
  br label %182

182:                                              ; preds = %181, %64
  br label %183

183:                                              ; preds = %182, %48
  br label %184

184:                                              ; preds = %183, %32
  br label %185

185:                                              ; preds = %184
  %186 = load i32, i32* %11, align 4
  %187 = add nsw i32 %186, 1
  store i32 %187, i32* %11, align 4
  br label %22, !llvm.loop !67

188:                                              ; preds = %22
  %189 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %190 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %189, align 4
  %191 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %190, i32 0, i32 63
  %192 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %191, align 4
  %193 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %194 = load %struct._jobject*, %struct._jobject** %5, align 4
  %195 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %196 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  call void %192(%struct.JNINativeInterface_** noundef %193, %struct._jobject* noundef %194, %struct._jmethodID* noundef %195, %union.jvalue* noundef %196)
  %197 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %197)
  ret void
}

; Function Attrs: noinline nounwind optnone
define dso_local void @JNI_CallVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !68

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 63
  %194 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  call void %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
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
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
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
  store i32 %23, i32* %12, align 4
  store i32 0, i32* %13, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %13, align 4
  %26 = load i32, i32* %12, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %13, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %9 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %13, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %13, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %9 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %13, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %13, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %9 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %13, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %13, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %9 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %13, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %13, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %9 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %13, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %13, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %9 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %13, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %13, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %9 to i8**
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
  %140 = load i32, i32* %13, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %13, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %9 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %13, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %13, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %9 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %13, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %13, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %13, align 4
  br label %24, !llvm.loop !69

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 93
  %194 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %196 = load %struct._jobject*, %struct._jobject** %6, align 4
  %197 = load %struct._jobject*, %struct._jobject** %7, align 4
  %198 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %199 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i32 0, i32 0
  call void %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jobject* noundef %197, %struct._jmethodID* noundef %198, %union.jvalue* noundef %199)
  %200 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %200)
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
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
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
  store i32 %25, i32* %13, align 4
  store i32 0, i32* %14, align 4
  br label %26

26:                                               ; preds = %189, %5
  %27 = load i32, i32* %14, align 4
  %28 = load i32, i32* %13, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %192

30:                                               ; preds = %26
  %31 = load i32, i32* %14, align 4
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %31
  %33 = load i8, i8* %32, align 1
  %34 = zext i8 %33 to i32
  %35 = icmp eq i32 %34, 90
  br i1 %35, label %36, label %46

36:                                               ; preds = %30
  %37 = bitcast %struct.__va_list* %6 to i8**
  %38 = load i8*, i8** %37, align 4
  %39 = getelementptr inbounds i8, i8* %38, i32 4
  store i8* %39, i8** %37, align 4
  %40 = bitcast i8* %38 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = load i32, i32* %14, align 4
  %44 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %43
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %42, i8* %45, align 8
  br label %188

46:                                               ; preds = %30
  %47 = load i32, i32* %14, align 4
  %48 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %47
  %49 = load i8, i8* %48, align 1
  %50 = zext i8 %49 to i32
  %51 = icmp eq i32 %50, 66
  br i1 %51, label %52, label %62

52:                                               ; preds = %46
  %53 = bitcast %struct.__va_list* %6 to i8**
  %54 = load i8*, i8** %53, align 4
  %55 = getelementptr inbounds i8, i8* %54, i32 4
  store i8* %55, i8** %53, align 4
  %56 = bitcast i8* %54 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = load i32, i32* %14, align 4
  %60 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %59
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %58, i8* %61, align 8
  br label %187

62:                                               ; preds = %46
  %63 = load i32, i32* %14, align 4
  %64 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %63
  %65 = load i8, i8* %64, align 1
  %66 = zext i8 %65 to i32
  %67 = icmp eq i32 %66, 67
  br i1 %67, label %68, label %78

68:                                               ; preds = %62
  %69 = bitcast %struct.__va_list* %6 to i8**
  %70 = load i8*, i8** %69, align 4
  %71 = getelementptr inbounds i8, i8* %70, i32 4
  store i8* %71, i8** %69, align 4
  %72 = bitcast i8* %70 to i32*
  %73 = load i32, i32* %72, align 4
  %74 = trunc i32 %73 to i16
  %75 = load i32, i32* %14, align 4
  %76 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %75
  %77 = bitcast %union.jvalue* %76 to i16*
  store i16 %74, i16* %77, align 8
  br label %186

78:                                               ; preds = %62
  %79 = load i32, i32* %14, align 4
  %80 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %79
  %81 = load i8, i8* %80, align 1
  %82 = zext i8 %81 to i32
  %83 = icmp eq i32 %82, 83
  br i1 %83, label %84, label %94

84:                                               ; preds = %78
  %85 = bitcast %struct.__va_list* %6 to i8**
  %86 = load i8*, i8** %85, align 4
  %87 = getelementptr inbounds i8, i8* %86, i32 4
  store i8* %87, i8** %85, align 4
  %88 = bitcast i8* %86 to i32*
  %89 = load i32, i32* %88, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %14, align 4
  %92 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %91
  %93 = bitcast %union.jvalue* %92 to i16*
  store i16 %90, i16* %93, align 8
  br label %185

94:                                               ; preds = %78
  %95 = load i32, i32* %14, align 4
  %96 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %95
  %97 = load i8, i8* %96, align 1
  %98 = zext i8 %97 to i32
  %99 = icmp eq i32 %98, 73
  br i1 %99, label %100, label %109

100:                                              ; preds = %94
  %101 = bitcast %struct.__va_list* %6 to i8**
  %102 = load i8*, i8** %101, align 4
  %103 = getelementptr inbounds i8, i8* %102, i32 4
  store i8* %103, i8** %101, align 4
  %104 = bitcast i8* %102 to i32*
  %105 = load i32, i32* %104, align 4
  %106 = load i32, i32* %14, align 4
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %105, i32* %108, align 8
  br label %184

109:                                              ; preds = %94
  %110 = load i32, i32* %14, align 4
  %111 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %110
  %112 = load i8, i8* %111, align 1
  %113 = zext i8 %112 to i32
  %114 = icmp eq i32 %113, 74
  br i1 %114, label %115, label %125

115:                                              ; preds = %109
  %116 = bitcast %struct.__va_list* %6 to i8**
  %117 = load i8*, i8** %116, align 4
  %118 = getelementptr inbounds i8, i8* %117, i32 4
  store i8* %118, i8** %116, align 4
  %119 = bitcast i8* %117 to i32*
  %120 = load i32, i32* %119, align 4
  %121 = sext i32 %120 to i64
  %122 = load i32, i32* %14, align 4
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %121, i64* %124, align 8
  br label %183

125:                                              ; preds = %109
  %126 = load i32, i32* %14, align 4
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %126
  %128 = load i8, i8* %127, align 1
  %129 = zext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %145

131:                                              ; preds = %125
  %132 = bitcast %struct.__va_list* %6 to i8**
  %133 = load i8*, i8** %132, align 4
  %134 = ptrtoint i8* %133 to i32
  %135 = add i32 %134, 7
  %136 = and i32 %135, -8
  %137 = inttoptr i32 %136 to i8*
  %138 = getelementptr inbounds i8, i8* %137, i32 8
  store i8* %138, i8** %132, align 4
  %139 = bitcast i8* %137 to double*
  %140 = load double, double* %139, align 8
  %141 = fptrunc double %140 to float
  %142 = load i32, i32* %14, align 4
  %143 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %142
  %144 = bitcast %union.jvalue* %143 to float*
  store float %141, float* %144, align 8
  br label %182

145:                                              ; preds = %125
  %146 = load i32, i32* %14, align 4
  %147 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %146
  %148 = load i8, i8* %147, align 1
  %149 = zext i8 %148 to i32
  %150 = icmp eq i32 %149, 68
  br i1 %150, label %151, label %164

151:                                              ; preds = %145
  %152 = bitcast %struct.__va_list* %6 to i8**
  %153 = load i8*, i8** %152, align 4
  %154 = ptrtoint i8* %153 to i32
  %155 = add i32 %154, 7
  %156 = and i32 %155, -8
  %157 = inttoptr i32 %156 to i8*
  %158 = getelementptr inbounds i8, i8* %157, i32 8
  store i8* %158, i8** %152, align 4
  %159 = bitcast i8* %157 to double*
  %160 = load double, double* %159, align 8
  %161 = load i32, i32* %14, align 4
  %162 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %161
  %163 = bitcast %union.jvalue* %162 to double*
  store double %160, double* %163, align 8
  br label %181

164:                                              ; preds = %145
  %165 = load i32, i32* %14, align 4
  %166 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i32 0, i32 %165
  %167 = load i8, i8* %166, align 1
  %168 = zext i8 %167 to i32
  %169 = icmp eq i32 %168, 76
  br i1 %169, label %170, label %180

170:                                              ; preds = %164
  %171 = bitcast %struct.__va_list* %6 to i8**
  %172 = load i8*, i8** %171, align 4
  %173 = getelementptr inbounds i8, i8* %172, i32 4
  store i8* %173, i8** %171, align 4
  %174 = bitcast i8* %172 to i8**
  %175 = load i8*, i8** %174, align 4
  %176 = bitcast i8* %175 to %struct._jobject*
  %177 = load i32, i32* %14, align 4
  %178 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 %177
  %179 = bitcast %union.jvalue* %178 to %struct._jobject**
  store %struct._jobject* %176, %struct._jobject** %179, align 8
  br label %180

180:                                              ; preds = %170, %164
  br label %181

181:                                              ; preds = %180, %151
  br label %182

182:                                              ; preds = %181, %131
  br label %183

183:                                              ; preds = %182, %115
  br label %184

184:                                              ; preds = %183, %100
  br label %185

185:                                              ; preds = %184, %84
  br label %186

186:                                              ; preds = %185, %68
  br label %187

187:                                              ; preds = %186, %52
  br label %188

188:                                              ; preds = %187, %36
  br label %189

189:                                              ; preds = %188
  %190 = load i32, i32* %14, align 4
  %191 = add nsw i32 %190, 1
  store i32 %191, i32* %14, align 4
  br label %26, !llvm.loop !70

192:                                              ; preds = %26
  %193 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %194 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %193, align 4
  %195 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %194, i32 0, i32 93
  %196 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %195, align 4
  %197 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %7, align 4
  %198 = load %struct._jobject*, %struct._jobject** %8, align 4
  %199 = load %struct._jobject*, %struct._jobject** %9, align 4
  %200 = load %struct._jmethodID*, %struct._jmethodID** %10, align 4
  %201 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i32 0, i32 0
  call void %196(%struct.JNINativeInterface_** noundef %197, %struct._jobject* noundef %198, %struct._jobject* noundef %199, %struct._jmethodID* noundef %200, %union.jvalue* noundef %201)
  ret void
}

; Function Attrs: noinline nounwind optnone
define dso_local void @JNI_CallStaticVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
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
  store i32 %21, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %22

22:                                               ; preds = %185, %3
  %23 = load i32, i32* %11, align 4
  %24 = load i32, i32* %10, align 4
  %25 = icmp slt i32 %23, %24
  br i1 %25, label %26, label %188

26:                                               ; preds = %22
  %27 = load i32, i32* %11, align 4
  %28 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %27
  %29 = load i8, i8* %28, align 1
  %30 = zext i8 %29 to i32
  %31 = icmp eq i32 %30, 90
  br i1 %31, label %32, label %42

32:                                               ; preds = %26
  %33 = bitcast %struct.__va_list* %7 to i8**
  %34 = load i8*, i8** %33, align 4
  %35 = getelementptr inbounds i8, i8* %34, i32 4
  store i8* %35, i8** %33, align 4
  %36 = bitcast i8* %34 to i32*
  %37 = load i32, i32* %36, align 4
  %38 = trunc i32 %37 to i8
  %39 = load i32, i32* %11, align 4
  %40 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %39
  %41 = bitcast %union.jvalue* %40 to i8*
  store i8 %38, i8* %41, align 8
  br label %184

42:                                               ; preds = %26
  %43 = load i32, i32* %11, align 4
  %44 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %43
  %45 = load i8, i8* %44, align 1
  %46 = zext i8 %45 to i32
  %47 = icmp eq i32 %46, 66
  br i1 %47, label %48, label %58

48:                                               ; preds = %42
  %49 = bitcast %struct.__va_list* %7 to i8**
  %50 = load i8*, i8** %49, align 4
  %51 = getelementptr inbounds i8, i8* %50, i32 4
  store i8* %51, i8** %49, align 4
  %52 = bitcast i8* %50 to i32*
  %53 = load i32, i32* %52, align 4
  %54 = trunc i32 %53 to i8
  %55 = load i32, i32* %11, align 4
  %56 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %55
  %57 = bitcast %union.jvalue* %56 to i8*
  store i8 %54, i8* %57, align 8
  br label %183

58:                                               ; preds = %42
  %59 = load i32, i32* %11, align 4
  %60 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %59
  %61 = load i8, i8* %60, align 1
  %62 = zext i8 %61 to i32
  %63 = icmp eq i32 %62, 67
  br i1 %63, label %64, label %74

64:                                               ; preds = %58
  %65 = bitcast %struct.__va_list* %7 to i8**
  %66 = load i8*, i8** %65, align 4
  %67 = getelementptr inbounds i8, i8* %66, i32 4
  store i8* %67, i8** %65, align 4
  %68 = bitcast i8* %66 to i32*
  %69 = load i32, i32* %68, align 4
  %70 = trunc i32 %69 to i16
  %71 = load i32, i32* %11, align 4
  %72 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %71
  %73 = bitcast %union.jvalue* %72 to i16*
  store i16 %70, i16* %73, align 8
  br label %182

74:                                               ; preds = %58
  %75 = load i32, i32* %11, align 4
  %76 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %75
  %77 = load i8, i8* %76, align 1
  %78 = zext i8 %77 to i32
  %79 = icmp eq i32 %78, 83
  br i1 %79, label %80, label %90

80:                                               ; preds = %74
  %81 = bitcast %struct.__va_list* %7 to i8**
  %82 = load i8*, i8** %81, align 4
  %83 = getelementptr inbounds i8, i8* %82, i32 4
  store i8* %83, i8** %81, align 4
  %84 = bitcast i8* %82 to i32*
  %85 = load i32, i32* %84, align 4
  %86 = trunc i32 %85 to i16
  %87 = load i32, i32* %11, align 4
  %88 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %87
  %89 = bitcast %union.jvalue* %88 to i16*
  store i16 %86, i16* %89, align 8
  br label %181

90:                                               ; preds = %74
  %91 = load i32, i32* %11, align 4
  %92 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %91
  %93 = load i8, i8* %92, align 1
  %94 = zext i8 %93 to i32
  %95 = icmp eq i32 %94, 73
  br i1 %95, label %96, label %105

96:                                               ; preds = %90
  %97 = bitcast %struct.__va_list* %7 to i8**
  %98 = load i8*, i8** %97, align 4
  %99 = getelementptr inbounds i8, i8* %98, i32 4
  store i8* %99, i8** %97, align 4
  %100 = bitcast i8* %98 to i32*
  %101 = load i32, i32* %100, align 4
  %102 = load i32, i32* %11, align 4
  %103 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %102
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %101, i32* %104, align 8
  br label %180

105:                                              ; preds = %90
  %106 = load i32, i32* %11, align 4
  %107 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %106
  %108 = load i8, i8* %107, align 1
  %109 = zext i8 %108 to i32
  %110 = icmp eq i32 %109, 74
  br i1 %110, label %111, label %121

111:                                              ; preds = %105
  %112 = bitcast %struct.__va_list* %7 to i8**
  %113 = load i8*, i8** %112, align 4
  %114 = getelementptr inbounds i8, i8* %113, i32 4
  store i8* %114, i8** %112, align 4
  %115 = bitcast i8* %113 to i32*
  %116 = load i32, i32* %115, align 4
  %117 = sext i32 %116 to i64
  %118 = load i32, i32* %11, align 4
  %119 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %118
  %120 = bitcast %union.jvalue* %119 to i64*
  store i64 %117, i64* %120, align 8
  br label %179

121:                                              ; preds = %105
  %122 = load i32, i32* %11, align 4
  %123 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %122
  %124 = load i8, i8* %123, align 1
  %125 = zext i8 %124 to i32
  %126 = icmp eq i32 %125, 70
  br i1 %126, label %127, label %141

127:                                              ; preds = %121
  %128 = bitcast %struct.__va_list* %7 to i8**
  %129 = load i8*, i8** %128, align 4
  %130 = ptrtoint i8* %129 to i32
  %131 = add i32 %130, 7
  %132 = and i32 %131, -8
  %133 = inttoptr i32 %132 to i8*
  %134 = getelementptr inbounds i8, i8* %133, i32 8
  store i8* %134, i8** %128, align 4
  %135 = bitcast i8* %133 to double*
  %136 = load double, double* %135, align 8
  %137 = fptrunc double %136 to float
  %138 = load i32, i32* %11, align 4
  %139 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %138
  %140 = bitcast %union.jvalue* %139 to float*
  store float %137, float* %140, align 8
  br label %178

141:                                              ; preds = %121
  %142 = load i32, i32* %11, align 4
  %143 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %142
  %144 = load i8, i8* %143, align 1
  %145 = zext i8 %144 to i32
  %146 = icmp eq i32 %145, 68
  br i1 %146, label %147, label %160

147:                                              ; preds = %141
  %148 = bitcast %struct.__va_list* %7 to i8**
  %149 = load i8*, i8** %148, align 4
  %150 = ptrtoint i8* %149 to i32
  %151 = add i32 %150, 7
  %152 = and i32 %151, -8
  %153 = inttoptr i32 %152 to i8*
  %154 = getelementptr inbounds i8, i8* %153, i32 8
  store i8* %154, i8** %148, align 4
  %155 = bitcast i8* %153 to double*
  %156 = load double, double* %155, align 8
  %157 = load i32, i32* %11, align 4
  %158 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %157
  %159 = bitcast %union.jvalue* %158 to double*
  store double %156, double* %159, align 8
  br label %177

160:                                              ; preds = %141
  %161 = load i32, i32* %11, align 4
  %162 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i32 0, i32 %161
  %163 = load i8, i8* %162, align 1
  %164 = zext i8 %163 to i32
  %165 = icmp eq i32 %164, 76
  br i1 %165, label %166, label %176

166:                                              ; preds = %160
  %167 = bitcast %struct.__va_list* %7 to i8**
  %168 = load i8*, i8** %167, align 4
  %169 = getelementptr inbounds i8, i8* %168, i32 4
  store i8* %169, i8** %167, align 4
  %170 = bitcast i8* %168 to i8**
  %171 = load i8*, i8** %170, align 4
  %172 = bitcast i8* %171 to %struct._jobject*
  %173 = load i32, i32* %11, align 4
  %174 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 %173
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %172, %struct._jobject** %175, align 8
  br label %176

176:                                              ; preds = %166, %160
  br label %177

177:                                              ; preds = %176, %147
  br label %178

178:                                              ; preds = %177, %127
  br label %179

179:                                              ; preds = %178, %111
  br label %180

180:                                              ; preds = %179, %96
  br label %181

181:                                              ; preds = %180, %80
  br label %182

182:                                              ; preds = %181, %64
  br label %183

183:                                              ; preds = %182, %48
  br label %184

184:                                              ; preds = %183, %32
  br label %185

185:                                              ; preds = %184
  %186 = load i32, i32* %11, align 4
  %187 = add nsw i32 %186, 1
  store i32 %187, i32* %11, align 4
  br label %22, !llvm.loop !71

188:                                              ; preds = %22
  %189 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %190 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %189, align 4
  %191 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %190, i32 0, i32 143
  %192 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %191, align 4
  %193 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %194 = load %struct._jobject*, %struct._jobject** %5, align 4
  %195 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %196 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i32 0, i32 0
  call void %192(%struct.JNINativeInterface_** noundef %193, %struct._jobject* noundef %194, %struct._jmethodID* noundef %195, %union.jvalue* noundef %196)
  %197 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %197)
  ret void
}

; Function Attrs: noinline nounwind optnone
define dso_local void @JNI_CallStaticVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = alloca %struct.JNINativeInterface_**, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
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
  store i32 %23, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %24

24:                                               ; preds = %187, %4
  %25 = load i32, i32* %12, align 4
  %26 = load i32, i32* %11, align 4
  %27 = icmp slt i32 %25, %26
  br i1 %27, label %28, label %190

28:                                               ; preds = %24
  %29 = load i32, i32* %12, align 4
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %29
  %31 = load i8, i8* %30, align 1
  %32 = zext i8 %31 to i32
  %33 = icmp eq i32 %32, 90
  br i1 %33, label %34, label %44

34:                                               ; preds = %28
  %35 = bitcast %struct.__va_list* %5 to i8**
  %36 = load i8*, i8** %35, align 4
  %37 = getelementptr inbounds i8, i8* %36, i32 4
  store i8* %37, i8** %35, align 4
  %38 = bitcast i8* %36 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = load i32, i32* %12, align 4
  %42 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %41
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %40, i8* %43, align 8
  br label %186

44:                                               ; preds = %28
  %45 = load i32, i32* %12, align 4
  %46 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %45
  %47 = load i8, i8* %46, align 1
  %48 = zext i8 %47 to i32
  %49 = icmp eq i32 %48, 66
  br i1 %49, label %50, label %60

50:                                               ; preds = %44
  %51 = bitcast %struct.__va_list* %5 to i8**
  %52 = load i8*, i8** %51, align 4
  %53 = getelementptr inbounds i8, i8* %52, i32 4
  store i8* %53, i8** %51, align 4
  %54 = bitcast i8* %52 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = trunc i32 %55 to i8
  %57 = load i32, i32* %12, align 4
  %58 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %57
  %59 = bitcast %union.jvalue* %58 to i8*
  store i8 %56, i8* %59, align 8
  br label %185

60:                                               ; preds = %44
  %61 = load i32, i32* %12, align 4
  %62 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %61
  %63 = load i8, i8* %62, align 1
  %64 = zext i8 %63 to i32
  %65 = icmp eq i32 %64, 67
  br i1 %65, label %66, label %76

66:                                               ; preds = %60
  %67 = bitcast %struct.__va_list* %5 to i8**
  %68 = load i8*, i8** %67, align 4
  %69 = getelementptr inbounds i8, i8* %68, i32 4
  store i8* %69, i8** %67, align 4
  %70 = bitcast i8* %68 to i32*
  %71 = load i32, i32* %70, align 4
  %72 = trunc i32 %71 to i16
  %73 = load i32, i32* %12, align 4
  %74 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %73
  %75 = bitcast %union.jvalue* %74 to i16*
  store i16 %72, i16* %75, align 8
  br label %184

76:                                               ; preds = %60
  %77 = load i32, i32* %12, align 4
  %78 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %77
  %79 = load i8, i8* %78, align 1
  %80 = zext i8 %79 to i32
  %81 = icmp eq i32 %80, 83
  br i1 %81, label %82, label %92

82:                                               ; preds = %76
  %83 = bitcast %struct.__va_list* %5 to i8**
  %84 = load i8*, i8** %83, align 4
  %85 = getelementptr inbounds i8, i8* %84, i32 4
  store i8* %85, i8** %83, align 4
  %86 = bitcast i8* %84 to i32*
  %87 = load i32, i32* %86, align 4
  %88 = trunc i32 %87 to i16
  %89 = load i32, i32* %12, align 4
  %90 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %89
  %91 = bitcast %union.jvalue* %90 to i16*
  store i16 %88, i16* %91, align 8
  br label %183

92:                                               ; preds = %76
  %93 = load i32, i32* %12, align 4
  %94 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %93
  %95 = load i8, i8* %94, align 1
  %96 = zext i8 %95 to i32
  %97 = icmp eq i32 %96, 73
  br i1 %97, label %98, label %107

98:                                               ; preds = %92
  %99 = bitcast %struct.__va_list* %5 to i8**
  %100 = load i8*, i8** %99, align 4
  %101 = getelementptr inbounds i8, i8* %100, i32 4
  store i8* %101, i8** %99, align 4
  %102 = bitcast i8* %100 to i32*
  %103 = load i32, i32* %102, align 4
  %104 = load i32, i32* %12, align 4
  %105 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %104
  %106 = bitcast %union.jvalue* %105 to i32*
  store i32 %103, i32* %106, align 8
  br label %182

107:                                              ; preds = %92
  %108 = load i32, i32* %12, align 4
  %109 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %108
  %110 = load i8, i8* %109, align 1
  %111 = zext i8 %110 to i32
  %112 = icmp eq i32 %111, 74
  br i1 %112, label %113, label %123

113:                                              ; preds = %107
  %114 = bitcast %struct.__va_list* %5 to i8**
  %115 = load i8*, i8** %114, align 4
  %116 = getelementptr inbounds i8, i8* %115, i32 4
  store i8* %116, i8** %114, align 4
  %117 = bitcast i8* %115 to i32*
  %118 = load i32, i32* %117, align 4
  %119 = sext i32 %118 to i64
  %120 = load i32, i32* %12, align 4
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %119, i64* %122, align 8
  br label %181

123:                                              ; preds = %107
  %124 = load i32, i32* %12, align 4
  %125 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %124
  %126 = load i8, i8* %125, align 1
  %127 = zext i8 %126 to i32
  %128 = icmp eq i32 %127, 70
  br i1 %128, label %129, label %143

129:                                              ; preds = %123
  %130 = bitcast %struct.__va_list* %5 to i8**
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
  %140 = load i32, i32* %12, align 4
  %141 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %140
  %142 = bitcast %union.jvalue* %141 to float*
  store float %139, float* %142, align 8
  br label %180

143:                                              ; preds = %123
  %144 = load i32, i32* %12, align 4
  %145 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %144
  %146 = load i8, i8* %145, align 1
  %147 = zext i8 %146 to i32
  %148 = icmp eq i32 %147, 68
  br i1 %148, label %149, label %162

149:                                              ; preds = %143
  %150 = bitcast %struct.__va_list* %5 to i8**
  %151 = load i8*, i8** %150, align 4
  %152 = ptrtoint i8* %151 to i32
  %153 = add i32 %152, 7
  %154 = and i32 %153, -8
  %155 = inttoptr i32 %154 to i8*
  %156 = getelementptr inbounds i8, i8* %155, i32 8
  store i8* %156, i8** %150, align 4
  %157 = bitcast i8* %155 to double*
  %158 = load double, double* %157, align 8
  %159 = load i32, i32* %12, align 4
  %160 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %159
  %161 = bitcast %union.jvalue* %160 to double*
  store double %158, double* %161, align 8
  br label %179

162:                                              ; preds = %143
  %163 = load i32, i32* %12, align 4
  %164 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i32 0, i32 %163
  %165 = load i8, i8* %164, align 1
  %166 = zext i8 %165 to i32
  %167 = icmp eq i32 %166, 76
  br i1 %167, label %168, label %178

168:                                              ; preds = %162
  %169 = bitcast %struct.__va_list* %5 to i8**
  %170 = load i8*, i8** %169, align 4
  %171 = getelementptr inbounds i8, i8* %170, i32 4
  store i8* %171, i8** %169, align 4
  %172 = bitcast i8* %170 to i8**
  %173 = load i8*, i8** %172, align 4
  %174 = bitcast i8* %173 to %struct._jobject*
  %175 = load i32, i32* %12, align 4
  %176 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 %175
  %177 = bitcast %union.jvalue* %176 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %177, align 8
  br label %178

178:                                              ; preds = %168, %162
  br label %179

179:                                              ; preds = %178, %149
  br label %180

180:                                              ; preds = %179, %129
  br label %181

181:                                              ; preds = %180, %113
  br label %182

182:                                              ; preds = %181, %98
  br label %183

183:                                              ; preds = %182, %82
  br label %184

184:                                              ; preds = %183, %66
  br label %185

185:                                              ; preds = %184, %50
  br label %186

186:                                              ; preds = %185, %34
  br label %187

187:                                              ; preds = %186
  %188 = load i32, i32* %12, align 4
  %189 = add nsw i32 %188, 1
  store i32 %189, i32* %12, align 4
  br label %24, !llvm.loop !72

190:                                              ; preds = %24
  %191 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %192 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %191, align 4
  %193 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %192, i32 0, i32 143
  %194 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %193, align 4
  %195 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 4
  %196 = load %struct._jobject*, %struct._jobject** %7, align 4
  %197 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %198 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i32 0, i32 0
  call void %194(%struct.JNINativeInterface_** noundef %195, %struct._jobject* noundef %196, %struct._jmethodID* noundef %197, %union.jvalue* noundef %198)
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
