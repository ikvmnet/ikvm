; ModuleID = 'jni.c'
source_filename = "jni.c"
target datalayout = "e-m:o-i64:64-i128:128-n32:64-S128"
target triple = "arm64-apple-macosx10.4.0"

%struct.JNINativeInterface_ = type { i8*, i8*, i8*, i8*, i32 (%struct.JNINativeInterface_**)*, %struct._jobject* (%struct.JNINativeInterface_**, i8*, %struct._jobject*, i8*, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i8*)*, %struct._jmethodID* (%struct.JNINativeInterface_**, %struct._jobject*)*, %struct._jfieldID* (%struct.JNINativeInterface_**, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i8)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, %struct._jobject* (%struct.JNINativeInterface_**)*, void (%struct.JNINativeInterface_**)*, void (%struct.JNINativeInterface_**)*, void (%struct.JNINativeInterface_**, i8*)*, i32 (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, void (%struct.JNINativeInterface_**, %struct._jobject*)*, void (%struct.JNINativeInterface_**, %struct._jobject*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, {}*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*)*, %struct._jmethodID* (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i8*)*, {}*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, i8*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, i8*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, i8*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, i8*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, i8*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, i8*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, i8*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, i8*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jfieldID* (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i8*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, %struct._jobject*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i8)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i8)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i16)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i16)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i64)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, float)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, double)*, %struct._jmethodID* (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i8*)*, {}*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jfieldID* (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i8*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, %struct._jobject*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i8)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i8)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i16)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i16)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i64)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, float)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, double)*, %struct._jobject* (%struct.JNINativeInterface_**, i16*, i32)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i16* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i16*)*, %struct._jobject* (%struct.JNINativeInterface_**, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i8* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, i32, %struct._jobject*, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, i8* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i8* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i16* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i16* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i32* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i64* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, float* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, double* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i16*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i16*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i64*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, float*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, double*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i16*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i16*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i32*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i64*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, float*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, double*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i16*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i16*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i32*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i64*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, float*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, double*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct.JNINativeMethod*, i32)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, %struct.JNIInvokeInterface_***)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i16*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i8*)*, i8* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i32)*, i16* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i16*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, void (%struct.JNINativeInterface_**, %struct._jobject*)*, i8 (%struct.JNINativeInterface_**)*, %struct._jobject* (%struct.JNINativeInterface_**, i8*, i64)*, i8* (%struct.JNINativeInterface_**, %struct._jobject*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)* }
%struct._jfieldID = type opaque
%union.jvalue = type { i64 }
%struct.JNINativeMethod = type { i8*, i8*, i8* }
%struct.JNIInvokeInterface_ = type { i8*, i8*, i8*, i32 (%struct.JNIInvokeInterface_**)*, i32 (%struct.JNIInvokeInterface_**, i8**, i8*)*, i32 (%struct.JNIInvokeInterface_**)*, i32 (%struct.JNIInvokeInterface_**, i8**, i32)*, i32 (%struct.JNIInvokeInterface_**, i8**, i8*)* }
%struct._jobject = type opaque
%struct._jmethodID = type opaque

; Function Attrs: noinline nounwind optnone ssp uwtable
define %struct._jobject* @JNI_CallObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  %21 = alloca %struct._jobject*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %22 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %22)
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %24 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %23, align 8
  %25 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %24, i32 0, i32 0
  %26 = load i8*, i8** %25, align 8
  %27 = bitcast i8* %26 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %28 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %29 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %31 = call i32 %27(%struct.JNINativeInterface_** noundef %28, %struct._jmethodID* noundef %29, i8* noundef %30)
  store i32 %31, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %32

32:                                               ; preds = %177, %3
  %33 = load i32, i32* %11, align 4
  %34 = load i32, i32* %10, align 4
  %35 = icmp slt i32 %33, %34
  br i1 %35, label %36, label %180

36:                                               ; preds = %32
  %37 = load i32, i32* %11, align 4
  %38 = sext i32 %37 to i64
  %39 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %38
  %40 = load i8, i8* %39, align 1
  %41 = sext i8 %40 to i32
  %42 = icmp eq i32 %41, 90
  br i1 %42, label %43, label %51

43:                                               ; preds = %36
  %44 = va_arg i8** %7, i32
  store i32 %44, i32* %12, align 4
  %45 = load i32, i32* %12, align 4
  %46 = trunc i32 %45 to i8
  %47 = load i32, i32* %11, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %176

51:                                               ; preds = %36
  %52 = load i32, i32* %11, align 4
  %53 = sext i32 %52 to i64
  %54 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %53
  %55 = load i8, i8* %54, align 1
  %56 = sext i8 %55 to i32
  %57 = icmp eq i32 %56, 66
  br i1 %57, label %58, label %66

58:                                               ; preds = %51
  %59 = va_arg i8** %7, i32
  store i32 %59, i32* %13, align 4
  %60 = load i32, i32* %13, align 4
  %61 = trunc i32 %60 to i8
  %62 = load i32, i32* %11, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %63
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %61, i8* %65, align 8
  br label %175

66:                                               ; preds = %51
  %67 = load i32, i32* %11, align 4
  %68 = sext i32 %67 to i64
  %69 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %68
  %70 = load i8, i8* %69, align 1
  %71 = sext i8 %70 to i32
  %72 = icmp eq i32 %71, 67
  br i1 %72, label %73, label %81

73:                                               ; preds = %66
  %74 = va_arg i8** %7, i32
  store i32 %74, i32* %14, align 4
  %75 = load i32, i32* %14, align 4
  %76 = trunc i32 %75 to i16
  %77 = load i32, i32* %11, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %174

81:                                               ; preds = %66
  %82 = load i32, i32* %11, align 4
  %83 = sext i32 %82 to i64
  %84 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %83
  %85 = load i8, i8* %84, align 1
  %86 = sext i8 %85 to i32
  %87 = icmp eq i32 %86, 83
  br i1 %87, label %88, label %96

88:                                               ; preds = %81
  %89 = va_arg i8** %7, i32
  store i32 %89, i32* %15, align 4
  %90 = load i32, i32* %15, align 4
  %91 = trunc i32 %90 to i16
  %92 = load i32, i32* %11, align 4
  %93 = sext i32 %92 to i64
  %94 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %93
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %91, i16* %95, align 8
  br label %173

96:                                               ; preds = %81
  %97 = load i32, i32* %11, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %98
  %100 = load i8, i8* %99, align 1
  %101 = sext i8 %100 to i32
  %102 = icmp eq i32 %101, 73
  br i1 %102, label %103, label %110

103:                                              ; preds = %96
  %104 = va_arg i8** %7, i32
  store i32 %104, i32* %16, align 4
  %105 = load i32, i32* %16, align 4
  %106 = load i32, i32* %11, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %107
  %109 = bitcast %union.jvalue* %108 to i32*
  store i32 %105, i32* %109, align 8
  br label %172

110:                                              ; preds = %96
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %112
  %114 = load i8, i8* %113, align 1
  %115 = sext i8 %114 to i32
  %116 = icmp eq i32 %115, 74
  br i1 %116, label %117, label %124

117:                                              ; preds = %110
  %118 = va_arg i8** %7, i64
  store i64 %118, i64* %17, align 8
  %119 = load i64, i64* %17, align 8
  %120 = load i32, i32* %11, align 4
  %121 = sext i32 %120 to i64
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %119, i64* %123, align 8
  br label %171

124:                                              ; preds = %110
  %125 = load i32, i32* %11, align 4
  %126 = sext i32 %125 to i64
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %126
  %128 = load i8, i8* %127, align 1
  %129 = sext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %139

131:                                              ; preds = %124
  %132 = va_arg i8** %7, double
  store double %132, double* %18, align 8
  %133 = load double, double* %18, align 8
  %134 = fptrunc double %133 to float
  %135 = load i32, i32* %11, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %136
  %138 = bitcast %union.jvalue* %137 to float*
  store float %134, float* %138, align 8
  br label %170

139:                                              ; preds = %124
  %140 = load i32, i32* %11, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %141
  %143 = load i8, i8* %142, align 1
  %144 = sext i8 %143 to i32
  %145 = icmp eq i32 %144, 68
  br i1 %145, label %146, label %153

146:                                              ; preds = %139
  %147 = va_arg i8** %7, double
  store double %147, double* %19, align 8
  %148 = load double, double* %19, align 8
  %149 = load i32, i32* %11, align 4
  %150 = sext i32 %149 to i64
  %151 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %150
  %152 = bitcast %union.jvalue* %151 to double*
  store double %148, double* %152, align 8
  br label %169

153:                                              ; preds = %139
  %154 = load i32, i32* %11, align 4
  %155 = sext i32 %154 to i64
  %156 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %155
  %157 = load i8, i8* %156, align 1
  %158 = sext i8 %157 to i32
  %159 = icmp eq i32 %158, 76
  br i1 %159, label %160, label %168

160:                                              ; preds = %153
  %161 = va_arg i8** %7, i8*
  store i8* %161, i8** %20, align 8
  %162 = load i8*, i8** %20, align 8
  %163 = bitcast i8* %162 to %struct._jobject*
  %164 = load i32, i32* %11, align 4
  %165 = sext i32 %164 to i64
  %166 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %165
  %167 = bitcast %union.jvalue* %166 to %struct._jobject**
  store %struct._jobject* %163, %struct._jobject** %167, align 8
  br label %168

168:                                              ; preds = %160, %153
  br label %169

169:                                              ; preds = %168, %146
  br label %170

170:                                              ; preds = %169, %131
  br label %171

171:                                              ; preds = %170, %117
  br label %172

172:                                              ; preds = %171, %103
  br label %173

173:                                              ; preds = %172, %88
  br label %174

174:                                              ; preds = %173, %73
  br label %175

175:                                              ; preds = %174, %58
  br label %176

176:                                              ; preds = %175, %43
  br label %177

177:                                              ; preds = %176
  %178 = load i32, i32* %11, align 4
  %179 = add nsw i32 %178, 1
  store i32 %179, i32* %11, align 4
  br label %32, !llvm.loop !9

180:                                              ; preds = %32
  %181 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %182 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %181, align 8
  %183 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %182, i32 0, i32 36
  %184 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %183, align 8
  %185 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %186 = load %struct._jobject*, %struct._jobject** %5, align 8
  %187 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %188 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  %189 = call %struct._jobject* %184(%struct.JNINativeInterface_** noundef %185, %struct._jobject* noundef %186, %struct._jmethodID* noundef %187, %union.jvalue* noundef %188)
  store %struct._jobject* %189, %struct._jobject** %21, align 8
  %190 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %190)
  %191 = load %struct._jobject*, %struct._jobject** %21, align 8
  ret %struct._jobject* %191
}

; Function Attrs: nofree nosync nounwind willreturn
declare void @llvm.va_start(i8*) #1

; Function Attrs: nofree nosync nounwind willreturn
declare void @llvm.va_end(i8*) #1

; Function Attrs: noinline nounwind optnone ssp uwtable
define %struct._jobject* @JNI_CallObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !11

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 36
  %183 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  %188 = call %struct._jobject* %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret %struct._jobject* %188
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define %struct._jobject* @JNI_CallNonvirtualObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i8*, align 8
  %10 = alloca [256 x i8], align 1
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i32, align 4
  %19 = alloca i64, align 8
  %20 = alloca double, align 8
  %21 = alloca double, align 8
  %22 = alloca i8*, align 8
  %23 = alloca %struct._jobject*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %24 = bitcast i8** %9 to i8*
  call void @llvm.va_start(i8* %24)
  %25 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %26 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %25, align 8
  %27 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %26, i32 0, i32 0
  %28 = load i8*, i8** %27, align 8
  %29 = bitcast i8* %28 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %30 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %31 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 0
  %33 = call i32 %29(%struct.JNINativeInterface_** noundef %30, %struct._jmethodID* noundef %31, i8* noundef %32)
  store i32 %33, i32* %12, align 4
  store i32 0, i32* %13, align 4
  br label %34

34:                                               ; preds = %179, %4
  %35 = load i32, i32* %13, align 4
  %36 = load i32, i32* %12, align 4
  %37 = icmp slt i32 %35, %36
  br i1 %37, label %38, label %182

38:                                               ; preds = %34
  %39 = load i32, i32* %13, align 4
  %40 = sext i32 %39 to i64
  %41 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %40
  %42 = load i8, i8* %41, align 1
  %43 = sext i8 %42 to i32
  %44 = icmp eq i32 %43, 90
  br i1 %44, label %45, label %53

45:                                               ; preds = %38
  %46 = va_arg i8** %9, i32
  store i32 %46, i32* %14, align 4
  %47 = load i32, i32* %14, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %13, align 4
  %50 = sext i32 %49 to i64
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %48, i8* %52, align 8
  br label %178

53:                                               ; preds = %38
  %54 = load i32, i32* %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %55
  %57 = load i8, i8* %56, align 1
  %58 = sext i8 %57 to i32
  %59 = icmp eq i32 %58, 66
  br i1 %59, label %60, label %68

60:                                               ; preds = %53
  %61 = va_arg i8** %9, i32
  store i32 %61, i32* %15, align 4
  %62 = load i32, i32* %15, align 4
  %63 = trunc i32 %62 to i8
  %64 = load i32, i32* %13, align 4
  %65 = sext i32 %64 to i64
  %66 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %65
  %67 = bitcast %union.jvalue* %66 to i8*
  store i8 %63, i8* %67, align 8
  br label %177

68:                                               ; preds = %53
  %69 = load i32, i32* %13, align 4
  %70 = sext i32 %69 to i64
  %71 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %70
  %72 = load i8, i8* %71, align 1
  %73 = sext i8 %72 to i32
  %74 = icmp eq i32 %73, 67
  br i1 %74, label %75, label %83

75:                                               ; preds = %68
  %76 = va_arg i8** %9, i32
  store i32 %76, i32* %16, align 4
  %77 = load i32, i32* %16, align 4
  %78 = trunc i32 %77 to i16
  %79 = load i32, i32* %13, align 4
  %80 = sext i32 %79 to i64
  %81 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %80
  %82 = bitcast %union.jvalue* %81 to i16*
  store i16 %78, i16* %82, align 8
  br label %176

83:                                               ; preds = %68
  %84 = load i32, i32* %13, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %85
  %87 = load i8, i8* %86, align 1
  %88 = sext i8 %87 to i32
  %89 = icmp eq i32 %88, 83
  br i1 %89, label %90, label %98

90:                                               ; preds = %83
  %91 = va_arg i8** %9, i32
  store i32 %91, i32* %17, align 4
  %92 = load i32, i32* %17, align 4
  %93 = trunc i32 %92 to i16
  %94 = load i32, i32* %13, align 4
  %95 = sext i32 %94 to i64
  %96 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %95
  %97 = bitcast %union.jvalue* %96 to i16*
  store i16 %93, i16* %97, align 8
  br label %175

98:                                               ; preds = %83
  %99 = load i32, i32* %13, align 4
  %100 = sext i32 %99 to i64
  %101 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %100
  %102 = load i8, i8* %101, align 1
  %103 = sext i8 %102 to i32
  %104 = icmp eq i32 %103, 73
  br i1 %104, label %105, label %112

105:                                              ; preds = %98
  %106 = va_arg i8** %9, i32
  store i32 %106, i32* %18, align 4
  %107 = load i32, i32* %18, align 4
  %108 = load i32, i32* %13, align 4
  %109 = sext i32 %108 to i64
  %110 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %109
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %107, i32* %111, align 8
  br label %174

112:                                              ; preds = %98
  %113 = load i32, i32* %13, align 4
  %114 = sext i32 %113 to i64
  %115 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %114
  %116 = load i8, i8* %115, align 1
  %117 = sext i8 %116 to i32
  %118 = icmp eq i32 %117, 74
  br i1 %118, label %119, label %126

119:                                              ; preds = %112
  %120 = va_arg i8** %9, i64
  store i64 %120, i64* %19, align 8
  %121 = load i64, i64* %19, align 8
  %122 = load i32, i32* %13, align 4
  %123 = sext i32 %122 to i64
  %124 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %123
  %125 = bitcast %union.jvalue* %124 to i64*
  store i64 %121, i64* %125, align 8
  br label %173

126:                                              ; preds = %112
  %127 = load i32, i32* %13, align 4
  %128 = sext i32 %127 to i64
  %129 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %128
  %130 = load i8, i8* %129, align 1
  %131 = sext i8 %130 to i32
  %132 = icmp eq i32 %131, 70
  br i1 %132, label %133, label %141

133:                                              ; preds = %126
  %134 = va_arg i8** %9, double
  store double %134, double* %20, align 8
  %135 = load double, double* %20, align 8
  %136 = fptrunc double %135 to float
  %137 = load i32, i32* %13, align 4
  %138 = sext i32 %137 to i64
  %139 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %138
  %140 = bitcast %union.jvalue* %139 to float*
  store float %136, float* %140, align 8
  br label %172

141:                                              ; preds = %126
  %142 = load i32, i32* %13, align 4
  %143 = sext i32 %142 to i64
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %143
  %145 = load i8, i8* %144, align 1
  %146 = sext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %155

148:                                              ; preds = %141
  %149 = va_arg i8** %9, double
  store double %149, double* %21, align 8
  %150 = load double, double* %21, align 8
  %151 = load i32, i32* %13, align 4
  %152 = sext i32 %151 to i64
  %153 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %152
  %154 = bitcast %union.jvalue* %153 to double*
  store double %150, double* %154, align 8
  br label %171

155:                                              ; preds = %141
  %156 = load i32, i32* %13, align 4
  %157 = sext i32 %156 to i64
  %158 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %157
  %159 = load i8, i8* %158, align 1
  %160 = sext i8 %159 to i32
  %161 = icmp eq i32 %160, 76
  br i1 %161, label %162, label %170

162:                                              ; preds = %155
  %163 = va_arg i8** %9, i8*
  store i8* %163, i8** %22, align 8
  %164 = load i8*, i8** %22, align 8
  %165 = bitcast i8* %164 to %struct._jobject*
  %166 = load i32, i32* %13, align 4
  %167 = sext i32 %166 to i64
  %168 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %167
  %169 = bitcast %union.jvalue* %168 to %struct._jobject**
  store %struct._jobject* %165, %struct._jobject** %169, align 8
  br label %170

170:                                              ; preds = %162, %155
  br label %171

171:                                              ; preds = %170, %148
  br label %172

172:                                              ; preds = %171, %133
  br label %173

173:                                              ; preds = %172, %119
  br label %174

174:                                              ; preds = %173, %105
  br label %175

175:                                              ; preds = %174, %90
  br label %176

176:                                              ; preds = %175, %75
  br label %177

177:                                              ; preds = %176, %60
  br label %178

178:                                              ; preds = %177, %45
  br label %179

179:                                              ; preds = %178
  %180 = load i32, i32* %13, align 4
  %181 = add nsw i32 %180, 1
  store i32 %181, i32* %13, align 4
  br label %34, !llvm.loop !12

182:                                              ; preds = %34
  %183 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %184 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %183, align 8
  %185 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %184, i32 0, i32 66
  %186 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %185, align 8
  %187 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %188 = load %struct._jobject*, %struct._jobject** %6, align 8
  %189 = load %struct._jobject*, %struct._jobject** %7, align 8
  %190 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %191 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 0
  %192 = call %struct._jobject* %186(%struct.JNINativeInterface_** noundef %187, %struct._jobject* noundef %188, %struct._jobject* noundef %189, %struct._jmethodID* noundef %190, %union.jvalue* noundef %191)
  store %struct._jobject* %192, %struct._jobject** %23, align 8
  %193 = bitcast i8** %9 to i8*
  call void @llvm.va_end(i8* %193)
  %194 = load %struct._jobject*, %struct._jobject** %23, align 8
  ret %struct._jobject* %194
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define %struct._jobject* @JNI_CallNonvirtualObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, i8* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca i8*, align 8
  %11 = alloca [256 x i8], align 1
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i32, align 4
  %19 = alloca i32, align 4
  %20 = alloca i64, align 8
  %21 = alloca double, align 8
  %22 = alloca double, align 8
  %23 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  store i8* %4, i8** %10, align 8
  %24 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %25 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %25, i32 0, i32 0
  %27 = load i8*, i8** %26, align 8
  %28 = bitcast i8* %27 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %29 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %30 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 0
  %32 = call i32 %28(%struct.JNINativeInterface_** noundef %29, %struct._jmethodID* noundef %30, i8* noundef %31)
  store i32 %32, i32* %13, align 4
  store i32 0, i32* %14, align 4
  br label %33

33:                                               ; preds = %178, %5
  %34 = load i32, i32* %14, align 4
  %35 = load i32, i32* %13, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %181

37:                                               ; preds = %33
  %38 = load i32, i32* %14, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %39
  %41 = load i8, i8* %40, align 1
  %42 = sext i8 %41 to i32
  %43 = icmp eq i32 %42, 90
  br i1 %43, label %44, label %52

44:                                               ; preds = %37
  %45 = va_arg i8** %10, i32
  store i32 %45, i32* %15, align 4
  %46 = load i32, i32* %15, align 4
  %47 = trunc i32 %46 to i8
  %48 = load i32, i32* %14, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %47, i8* %51, align 8
  br label %177

52:                                               ; preds = %37
  %53 = load i32, i32* %14, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %54
  %56 = load i8, i8* %55, align 1
  %57 = sext i8 %56 to i32
  %58 = icmp eq i32 %57, 66
  br i1 %58, label %59, label %67

59:                                               ; preds = %52
  %60 = va_arg i8** %10, i32
  store i32 %60, i32* %16, align 4
  %61 = load i32, i32* %16, align 4
  %62 = trunc i32 %61 to i8
  %63 = load i32, i32* %14, align 4
  %64 = sext i32 %63 to i64
  %65 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %64
  %66 = bitcast %union.jvalue* %65 to i8*
  store i8 %62, i8* %66, align 8
  br label %176

67:                                               ; preds = %52
  %68 = load i32, i32* %14, align 4
  %69 = sext i32 %68 to i64
  %70 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %69
  %71 = load i8, i8* %70, align 1
  %72 = sext i8 %71 to i32
  %73 = icmp eq i32 %72, 67
  br i1 %73, label %74, label %82

74:                                               ; preds = %67
  %75 = va_arg i8** %10, i32
  store i32 %75, i32* %17, align 4
  %76 = load i32, i32* %17, align 4
  %77 = trunc i32 %76 to i16
  %78 = load i32, i32* %14, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %175

82:                                               ; preds = %67
  %83 = load i32, i32* %14, align 4
  %84 = sext i32 %83 to i64
  %85 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %84
  %86 = load i8, i8* %85, align 1
  %87 = sext i8 %86 to i32
  %88 = icmp eq i32 %87, 83
  br i1 %88, label %89, label %97

89:                                               ; preds = %82
  %90 = va_arg i8** %10, i32
  store i32 %90, i32* %18, align 4
  %91 = load i32, i32* %18, align 4
  %92 = trunc i32 %91 to i16
  %93 = load i32, i32* %14, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %94
  %96 = bitcast %union.jvalue* %95 to i16*
  store i16 %92, i16* %96, align 8
  br label %174

97:                                               ; preds = %82
  %98 = load i32, i32* %14, align 4
  %99 = sext i32 %98 to i64
  %100 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %99
  %101 = load i8, i8* %100, align 1
  %102 = sext i8 %101 to i32
  %103 = icmp eq i32 %102, 73
  br i1 %103, label %104, label %111

104:                                              ; preds = %97
  %105 = va_arg i8** %10, i32
  store i32 %105, i32* %19, align 4
  %106 = load i32, i32* %19, align 4
  %107 = load i32, i32* %14, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %108
  %110 = bitcast %union.jvalue* %109 to i32*
  store i32 %106, i32* %110, align 8
  br label %173

111:                                              ; preds = %97
  %112 = load i32, i32* %14, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %113
  %115 = load i8, i8* %114, align 1
  %116 = sext i8 %115 to i32
  %117 = icmp eq i32 %116, 74
  br i1 %117, label %118, label %125

118:                                              ; preds = %111
  %119 = va_arg i8** %10, i64
  store i64 %119, i64* %20, align 8
  %120 = load i64, i64* %20, align 8
  %121 = load i32, i32* %14, align 4
  %122 = sext i32 %121 to i64
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %120, i64* %124, align 8
  br label %172

125:                                              ; preds = %111
  %126 = load i32, i32* %14, align 4
  %127 = sext i32 %126 to i64
  %128 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %127
  %129 = load i8, i8* %128, align 1
  %130 = sext i8 %129 to i32
  %131 = icmp eq i32 %130, 70
  br i1 %131, label %132, label %140

132:                                              ; preds = %125
  %133 = va_arg i8** %10, double
  store double %133, double* %21, align 8
  %134 = load double, double* %21, align 8
  %135 = fptrunc double %134 to float
  %136 = load i32, i32* %14, align 4
  %137 = sext i32 %136 to i64
  %138 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %137
  %139 = bitcast %union.jvalue* %138 to float*
  store float %135, float* %139, align 8
  br label %171

140:                                              ; preds = %125
  %141 = load i32, i32* %14, align 4
  %142 = sext i32 %141 to i64
  %143 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %142
  %144 = load i8, i8* %143, align 1
  %145 = sext i8 %144 to i32
  %146 = icmp eq i32 %145, 68
  br i1 %146, label %147, label %154

147:                                              ; preds = %140
  %148 = va_arg i8** %10, double
  store double %148, double* %22, align 8
  %149 = load double, double* %22, align 8
  %150 = load i32, i32* %14, align 4
  %151 = sext i32 %150 to i64
  %152 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %151
  %153 = bitcast %union.jvalue* %152 to double*
  store double %149, double* %153, align 8
  br label %170

154:                                              ; preds = %140
  %155 = load i32, i32* %14, align 4
  %156 = sext i32 %155 to i64
  %157 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %156
  %158 = load i8, i8* %157, align 1
  %159 = sext i8 %158 to i32
  %160 = icmp eq i32 %159, 76
  br i1 %160, label %161, label %169

161:                                              ; preds = %154
  %162 = va_arg i8** %10, i8*
  store i8* %162, i8** %23, align 8
  %163 = load i8*, i8** %23, align 8
  %164 = bitcast i8* %163 to %struct._jobject*
  %165 = load i32, i32* %14, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %166
  %168 = bitcast %union.jvalue* %167 to %struct._jobject**
  store %struct._jobject* %164, %struct._jobject** %168, align 8
  br label %169

169:                                              ; preds = %161, %154
  br label %170

170:                                              ; preds = %169, %147
  br label %171

171:                                              ; preds = %170, %132
  br label %172

172:                                              ; preds = %171, %118
  br label %173

173:                                              ; preds = %172, %104
  br label %174

174:                                              ; preds = %173, %89
  br label %175

175:                                              ; preds = %174, %74
  br label %176

176:                                              ; preds = %175, %59
  br label %177

177:                                              ; preds = %176, %44
  br label %178

178:                                              ; preds = %177
  %179 = load i32, i32* %14, align 4
  %180 = add nsw i32 %179, 1
  store i32 %180, i32* %14, align 4
  br label %33, !llvm.loop !13

181:                                              ; preds = %33
  %182 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %183 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %182, align 8
  %184 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %183, i32 0, i32 66
  %185 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %184, align 8
  %186 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %187 = load %struct._jobject*, %struct._jobject** %7, align 8
  %188 = load %struct._jobject*, %struct._jobject** %8, align 8
  %189 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %190 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 0
  %191 = call %struct._jobject* %185(%struct.JNINativeInterface_** noundef %186, %struct._jobject* noundef %187, %struct._jobject* noundef %188, %struct._jmethodID* noundef %189, %union.jvalue* noundef %190)
  ret %struct._jobject* %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define %struct._jobject* @JNI_CallStaticObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  %21 = alloca %struct._jobject*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %22 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %22)
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %24 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %23, align 8
  %25 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %24, i32 0, i32 0
  %26 = load i8*, i8** %25, align 8
  %27 = bitcast i8* %26 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %28 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %29 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %31 = call i32 %27(%struct.JNINativeInterface_** noundef %28, %struct._jmethodID* noundef %29, i8* noundef %30)
  store i32 %31, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %32

32:                                               ; preds = %177, %3
  %33 = load i32, i32* %11, align 4
  %34 = load i32, i32* %10, align 4
  %35 = icmp slt i32 %33, %34
  br i1 %35, label %36, label %180

36:                                               ; preds = %32
  %37 = load i32, i32* %11, align 4
  %38 = sext i32 %37 to i64
  %39 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %38
  %40 = load i8, i8* %39, align 1
  %41 = sext i8 %40 to i32
  %42 = icmp eq i32 %41, 90
  br i1 %42, label %43, label %51

43:                                               ; preds = %36
  %44 = va_arg i8** %7, i32
  store i32 %44, i32* %12, align 4
  %45 = load i32, i32* %12, align 4
  %46 = trunc i32 %45 to i8
  %47 = load i32, i32* %11, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %176

51:                                               ; preds = %36
  %52 = load i32, i32* %11, align 4
  %53 = sext i32 %52 to i64
  %54 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %53
  %55 = load i8, i8* %54, align 1
  %56 = sext i8 %55 to i32
  %57 = icmp eq i32 %56, 66
  br i1 %57, label %58, label %66

58:                                               ; preds = %51
  %59 = va_arg i8** %7, i32
  store i32 %59, i32* %13, align 4
  %60 = load i32, i32* %13, align 4
  %61 = trunc i32 %60 to i8
  %62 = load i32, i32* %11, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %63
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %61, i8* %65, align 8
  br label %175

66:                                               ; preds = %51
  %67 = load i32, i32* %11, align 4
  %68 = sext i32 %67 to i64
  %69 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %68
  %70 = load i8, i8* %69, align 1
  %71 = sext i8 %70 to i32
  %72 = icmp eq i32 %71, 67
  br i1 %72, label %73, label %81

73:                                               ; preds = %66
  %74 = va_arg i8** %7, i32
  store i32 %74, i32* %14, align 4
  %75 = load i32, i32* %14, align 4
  %76 = trunc i32 %75 to i16
  %77 = load i32, i32* %11, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %174

81:                                               ; preds = %66
  %82 = load i32, i32* %11, align 4
  %83 = sext i32 %82 to i64
  %84 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %83
  %85 = load i8, i8* %84, align 1
  %86 = sext i8 %85 to i32
  %87 = icmp eq i32 %86, 83
  br i1 %87, label %88, label %96

88:                                               ; preds = %81
  %89 = va_arg i8** %7, i32
  store i32 %89, i32* %15, align 4
  %90 = load i32, i32* %15, align 4
  %91 = trunc i32 %90 to i16
  %92 = load i32, i32* %11, align 4
  %93 = sext i32 %92 to i64
  %94 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %93
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %91, i16* %95, align 8
  br label %173

96:                                               ; preds = %81
  %97 = load i32, i32* %11, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %98
  %100 = load i8, i8* %99, align 1
  %101 = sext i8 %100 to i32
  %102 = icmp eq i32 %101, 73
  br i1 %102, label %103, label %110

103:                                              ; preds = %96
  %104 = va_arg i8** %7, i32
  store i32 %104, i32* %16, align 4
  %105 = load i32, i32* %16, align 4
  %106 = load i32, i32* %11, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %107
  %109 = bitcast %union.jvalue* %108 to i32*
  store i32 %105, i32* %109, align 8
  br label %172

110:                                              ; preds = %96
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %112
  %114 = load i8, i8* %113, align 1
  %115 = sext i8 %114 to i32
  %116 = icmp eq i32 %115, 74
  br i1 %116, label %117, label %124

117:                                              ; preds = %110
  %118 = va_arg i8** %7, i64
  store i64 %118, i64* %17, align 8
  %119 = load i64, i64* %17, align 8
  %120 = load i32, i32* %11, align 4
  %121 = sext i32 %120 to i64
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %119, i64* %123, align 8
  br label %171

124:                                              ; preds = %110
  %125 = load i32, i32* %11, align 4
  %126 = sext i32 %125 to i64
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %126
  %128 = load i8, i8* %127, align 1
  %129 = sext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %139

131:                                              ; preds = %124
  %132 = va_arg i8** %7, double
  store double %132, double* %18, align 8
  %133 = load double, double* %18, align 8
  %134 = fptrunc double %133 to float
  %135 = load i32, i32* %11, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %136
  %138 = bitcast %union.jvalue* %137 to float*
  store float %134, float* %138, align 8
  br label %170

139:                                              ; preds = %124
  %140 = load i32, i32* %11, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %141
  %143 = load i8, i8* %142, align 1
  %144 = sext i8 %143 to i32
  %145 = icmp eq i32 %144, 68
  br i1 %145, label %146, label %153

146:                                              ; preds = %139
  %147 = va_arg i8** %7, double
  store double %147, double* %19, align 8
  %148 = load double, double* %19, align 8
  %149 = load i32, i32* %11, align 4
  %150 = sext i32 %149 to i64
  %151 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %150
  %152 = bitcast %union.jvalue* %151 to double*
  store double %148, double* %152, align 8
  br label %169

153:                                              ; preds = %139
  %154 = load i32, i32* %11, align 4
  %155 = sext i32 %154 to i64
  %156 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %155
  %157 = load i8, i8* %156, align 1
  %158 = sext i8 %157 to i32
  %159 = icmp eq i32 %158, 76
  br i1 %159, label %160, label %168

160:                                              ; preds = %153
  %161 = va_arg i8** %7, i8*
  store i8* %161, i8** %20, align 8
  %162 = load i8*, i8** %20, align 8
  %163 = bitcast i8* %162 to %struct._jobject*
  %164 = load i32, i32* %11, align 4
  %165 = sext i32 %164 to i64
  %166 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %165
  %167 = bitcast %union.jvalue* %166 to %struct._jobject**
  store %struct._jobject* %163, %struct._jobject** %167, align 8
  br label %168

168:                                              ; preds = %160, %153
  br label %169

169:                                              ; preds = %168, %146
  br label %170

170:                                              ; preds = %169, %131
  br label %171

171:                                              ; preds = %170, %117
  br label %172

172:                                              ; preds = %171, %103
  br label %173

173:                                              ; preds = %172, %88
  br label %174

174:                                              ; preds = %173, %73
  br label %175

175:                                              ; preds = %174, %58
  br label %176

176:                                              ; preds = %175, %43
  br label %177

177:                                              ; preds = %176
  %178 = load i32, i32* %11, align 4
  %179 = add nsw i32 %178, 1
  store i32 %179, i32* %11, align 4
  br label %32, !llvm.loop !14

180:                                              ; preds = %32
  %181 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %182 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %181, align 8
  %183 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %182, i32 0, i32 116
  %184 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %183, align 8
  %185 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %186 = load %struct._jobject*, %struct._jobject** %5, align 8
  %187 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %188 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  %189 = call %struct._jobject* %184(%struct.JNINativeInterface_** noundef %185, %struct._jobject* noundef %186, %struct._jmethodID* noundef %187, %union.jvalue* noundef %188)
  store %struct._jobject* %189, %struct._jobject** %21, align 8
  %190 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %190)
  %191 = load %struct._jobject*, %struct._jobject** %21, align 8
  ret %struct._jobject* %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define %struct._jobject* @JNI_CallStaticObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !15

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 116
  %183 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  %188 = call %struct._jobject* %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret %struct._jobject* %188
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define zeroext i8 @JNI_CallBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  %21 = alloca i8, align 1
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %22 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %22)
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %24 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %23, align 8
  %25 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %24, i32 0, i32 0
  %26 = load i8*, i8** %25, align 8
  %27 = bitcast i8* %26 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %28 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %29 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %31 = call i32 %27(%struct.JNINativeInterface_** noundef %28, %struct._jmethodID* noundef %29, i8* noundef %30)
  store i32 %31, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %32

32:                                               ; preds = %177, %3
  %33 = load i32, i32* %11, align 4
  %34 = load i32, i32* %10, align 4
  %35 = icmp slt i32 %33, %34
  br i1 %35, label %36, label %180

36:                                               ; preds = %32
  %37 = load i32, i32* %11, align 4
  %38 = sext i32 %37 to i64
  %39 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %38
  %40 = load i8, i8* %39, align 1
  %41 = sext i8 %40 to i32
  %42 = icmp eq i32 %41, 90
  br i1 %42, label %43, label %51

43:                                               ; preds = %36
  %44 = va_arg i8** %7, i32
  store i32 %44, i32* %12, align 4
  %45 = load i32, i32* %12, align 4
  %46 = trunc i32 %45 to i8
  %47 = load i32, i32* %11, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %176

51:                                               ; preds = %36
  %52 = load i32, i32* %11, align 4
  %53 = sext i32 %52 to i64
  %54 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %53
  %55 = load i8, i8* %54, align 1
  %56 = sext i8 %55 to i32
  %57 = icmp eq i32 %56, 66
  br i1 %57, label %58, label %66

58:                                               ; preds = %51
  %59 = va_arg i8** %7, i32
  store i32 %59, i32* %13, align 4
  %60 = load i32, i32* %13, align 4
  %61 = trunc i32 %60 to i8
  %62 = load i32, i32* %11, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %63
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %61, i8* %65, align 8
  br label %175

66:                                               ; preds = %51
  %67 = load i32, i32* %11, align 4
  %68 = sext i32 %67 to i64
  %69 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %68
  %70 = load i8, i8* %69, align 1
  %71 = sext i8 %70 to i32
  %72 = icmp eq i32 %71, 67
  br i1 %72, label %73, label %81

73:                                               ; preds = %66
  %74 = va_arg i8** %7, i32
  store i32 %74, i32* %14, align 4
  %75 = load i32, i32* %14, align 4
  %76 = trunc i32 %75 to i16
  %77 = load i32, i32* %11, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %174

81:                                               ; preds = %66
  %82 = load i32, i32* %11, align 4
  %83 = sext i32 %82 to i64
  %84 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %83
  %85 = load i8, i8* %84, align 1
  %86 = sext i8 %85 to i32
  %87 = icmp eq i32 %86, 83
  br i1 %87, label %88, label %96

88:                                               ; preds = %81
  %89 = va_arg i8** %7, i32
  store i32 %89, i32* %15, align 4
  %90 = load i32, i32* %15, align 4
  %91 = trunc i32 %90 to i16
  %92 = load i32, i32* %11, align 4
  %93 = sext i32 %92 to i64
  %94 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %93
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %91, i16* %95, align 8
  br label %173

96:                                               ; preds = %81
  %97 = load i32, i32* %11, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %98
  %100 = load i8, i8* %99, align 1
  %101 = sext i8 %100 to i32
  %102 = icmp eq i32 %101, 73
  br i1 %102, label %103, label %110

103:                                              ; preds = %96
  %104 = va_arg i8** %7, i32
  store i32 %104, i32* %16, align 4
  %105 = load i32, i32* %16, align 4
  %106 = load i32, i32* %11, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %107
  %109 = bitcast %union.jvalue* %108 to i32*
  store i32 %105, i32* %109, align 8
  br label %172

110:                                              ; preds = %96
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %112
  %114 = load i8, i8* %113, align 1
  %115 = sext i8 %114 to i32
  %116 = icmp eq i32 %115, 74
  br i1 %116, label %117, label %124

117:                                              ; preds = %110
  %118 = va_arg i8** %7, i64
  store i64 %118, i64* %17, align 8
  %119 = load i64, i64* %17, align 8
  %120 = load i32, i32* %11, align 4
  %121 = sext i32 %120 to i64
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %119, i64* %123, align 8
  br label %171

124:                                              ; preds = %110
  %125 = load i32, i32* %11, align 4
  %126 = sext i32 %125 to i64
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %126
  %128 = load i8, i8* %127, align 1
  %129 = sext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %139

131:                                              ; preds = %124
  %132 = va_arg i8** %7, double
  store double %132, double* %18, align 8
  %133 = load double, double* %18, align 8
  %134 = fptrunc double %133 to float
  %135 = load i32, i32* %11, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %136
  %138 = bitcast %union.jvalue* %137 to float*
  store float %134, float* %138, align 8
  br label %170

139:                                              ; preds = %124
  %140 = load i32, i32* %11, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %141
  %143 = load i8, i8* %142, align 1
  %144 = sext i8 %143 to i32
  %145 = icmp eq i32 %144, 68
  br i1 %145, label %146, label %153

146:                                              ; preds = %139
  %147 = va_arg i8** %7, double
  store double %147, double* %19, align 8
  %148 = load double, double* %19, align 8
  %149 = load i32, i32* %11, align 4
  %150 = sext i32 %149 to i64
  %151 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %150
  %152 = bitcast %union.jvalue* %151 to double*
  store double %148, double* %152, align 8
  br label %169

153:                                              ; preds = %139
  %154 = load i32, i32* %11, align 4
  %155 = sext i32 %154 to i64
  %156 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %155
  %157 = load i8, i8* %156, align 1
  %158 = sext i8 %157 to i32
  %159 = icmp eq i32 %158, 76
  br i1 %159, label %160, label %168

160:                                              ; preds = %153
  %161 = va_arg i8** %7, i8*
  store i8* %161, i8** %20, align 8
  %162 = load i8*, i8** %20, align 8
  %163 = bitcast i8* %162 to %struct._jobject*
  %164 = load i32, i32* %11, align 4
  %165 = sext i32 %164 to i64
  %166 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %165
  %167 = bitcast %union.jvalue* %166 to %struct._jobject**
  store %struct._jobject* %163, %struct._jobject** %167, align 8
  br label %168

168:                                              ; preds = %160, %153
  br label %169

169:                                              ; preds = %168, %146
  br label %170

170:                                              ; preds = %169, %131
  br label %171

171:                                              ; preds = %170, %117
  br label %172

172:                                              ; preds = %171, %103
  br label %173

173:                                              ; preds = %172, %88
  br label %174

174:                                              ; preds = %173, %73
  br label %175

175:                                              ; preds = %174, %58
  br label %176

176:                                              ; preds = %175, %43
  br label %177

177:                                              ; preds = %176
  %178 = load i32, i32* %11, align 4
  %179 = add nsw i32 %178, 1
  store i32 %179, i32* %11, align 4
  br label %32, !llvm.loop !16

180:                                              ; preds = %32
  %181 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %182 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %181, align 8
  %183 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %182, i32 0, i32 39
  %184 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %183, align 8
  %185 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %186 = load %struct._jobject*, %struct._jobject** %5, align 8
  %187 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %188 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  %189 = call zeroext i8 %184(%struct.JNINativeInterface_** noundef %185, %struct._jobject* noundef %186, %struct._jmethodID* noundef %187, %union.jvalue* noundef %188)
  store i8 %189, i8* %21, align 1
  %190 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %190)
  %191 = load i8, i8* %21, align 1
  ret i8 %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define zeroext i8 @JNI_CallBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !17

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 39
  %183 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  %188 = call zeroext i8 %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret i8 %188
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define zeroext i8 @JNI_CallNonvirtualBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i8*, align 8
  %10 = alloca [256 x i8], align 1
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i32, align 4
  %19 = alloca i64, align 8
  %20 = alloca double, align 8
  %21 = alloca double, align 8
  %22 = alloca i8*, align 8
  %23 = alloca i8, align 1
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %24 = bitcast i8** %9 to i8*
  call void @llvm.va_start(i8* %24)
  %25 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %26 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %25, align 8
  %27 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %26, i32 0, i32 0
  %28 = load i8*, i8** %27, align 8
  %29 = bitcast i8* %28 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %30 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %31 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 0
  %33 = call i32 %29(%struct.JNINativeInterface_** noundef %30, %struct._jmethodID* noundef %31, i8* noundef %32)
  store i32 %33, i32* %12, align 4
  store i32 0, i32* %13, align 4
  br label %34

34:                                               ; preds = %179, %4
  %35 = load i32, i32* %13, align 4
  %36 = load i32, i32* %12, align 4
  %37 = icmp slt i32 %35, %36
  br i1 %37, label %38, label %182

38:                                               ; preds = %34
  %39 = load i32, i32* %13, align 4
  %40 = sext i32 %39 to i64
  %41 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %40
  %42 = load i8, i8* %41, align 1
  %43 = sext i8 %42 to i32
  %44 = icmp eq i32 %43, 90
  br i1 %44, label %45, label %53

45:                                               ; preds = %38
  %46 = va_arg i8** %9, i32
  store i32 %46, i32* %14, align 4
  %47 = load i32, i32* %14, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %13, align 4
  %50 = sext i32 %49 to i64
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %48, i8* %52, align 8
  br label %178

53:                                               ; preds = %38
  %54 = load i32, i32* %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %55
  %57 = load i8, i8* %56, align 1
  %58 = sext i8 %57 to i32
  %59 = icmp eq i32 %58, 66
  br i1 %59, label %60, label %68

60:                                               ; preds = %53
  %61 = va_arg i8** %9, i32
  store i32 %61, i32* %15, align 4
  %62 = load i32, i32* %15, align 4
  %63 = trunc i32 %62 to i8
  %64 = load i32, i32* %13, align 4
  %65 = sext i32 %64 to i64
  %66 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %65
  %67 = bitcast %union.jvalue* %66 to i8*
  store i8 %63, i8* %67, align 8
  br label %177

68:                                               ; preds = %53
  %69 = load i32, i32* %13, align 4
  %70 = sext i32 %69 to i64
  %71 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %70
  %72 = load i8, i8* %71, align 1
  %73 = sext i8 %72 to i32
  %74 = icmp eq i32 %73, 67
  br i1 %74, label %75, label %83

75:                                               ; preds = %68
  %76 = va_arg i8** %9, i32
  store i32 %76, i32* %16, align 4
  %77 = load i32, i32* %16, align 4
  %78 = trunc i32 %77 to i16
  %79 = load i32, i32* %13, align 4
  %80 = sext i32 %79 to i64
  %81 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %80
  %82 = bitcast %union.jvalue* %81 to i16*
  store i16 %78, i16* %82, align 8
  br label %176

83:                                               ; preds = %68
  %84 = load i32, i32* %13, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %85
  %87 = load i8, i8* %86, align 1
  %88 = sext i8 %87 to i32
  %89 = icmp eq i32 %88, 83
  br i1 %89, label %90, label %98

90:                                               ; preds = %83
  %91 = va_arg i8** %9, i32
  store i32 %91, i32* %17, align 4
  %92 = load i32, i32* %17, align 4
  %93 = trunc i32 %92 to i16
  %94 = load i32, i32* %13, align 4
  %95 = sext i32 %94 to i64
  %96 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %95
  %97 = bitcast %union.jvalue* %96 to i16*
  store i16 %93, i16* %97, align 8
  br label %175

98:                                               ; preds = %83
  %99 = load i32, i32* %13, align 4
  %100 = sext i32 %99 to i64
  %101 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %100
  %102 = load i8, i8* %101, align 1
  %103 = sext i8 %102 to i32
  %104 = icmp eq i32 %103, 73
  br i1 %104, label %105, label %112

105:                                              ; preds = %98
  %106 = va_arg i8** %9, i32
  store i32 %106, i32* %18, align 4
  %107 = load i32, i32* %18, align 4
  %108 = load i32, i32* %13, align 4
  %109 = sext i32 %108 to i64
  %110 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %109
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %107, i32* %111, align 8
  br label %174

112:                                              ; preds = %98
  %113 = load i32, i32* %13, align 4
  %114 = sext i32 %113 to i64
  %115 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %114
  %116 = load i8, i8* %115, align 1
  %117 = sext i8 %116 to i32
  %118 = icmp eq i32 %117, 74
  br i1 %118, label %119, label %126

119:                                              ; preds = %112
  %120 = va_arg i8** %9, i64
  store i64 %120, i64* %19, align 8
  %121 = load i64, i64* %19, align 8
  %122 = load i32, i32* %13, align 4
  %123 = sext i32 %122 to i64
  %124 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %123
  %125 = bitcast %union.jvalue* %124 to i64*
  store i64 %121, i64* %125, align 8
  br label %173

126:                                              ; preds = %112
  %127 = load i32, i32* %13, align 4
  %128 = sext i32 %127 to i64
  %129 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %128
  %130 = load i8, i8* %129, align 1
  %131 = sext i8 %130 to i32
  %132 = icmp eq i32 %131, 70
  br i1 %132, label %133, label %141

133:                                              ; preds = %126
  %134 = va_arg i8** %9, double
  store double %134, double* %20, align 8
  %135 = load double, double* %20, align 8
  %136 = fptrunc double %135 to float
  %137 = load i32, i32* %13, align 4
  %138 = sext i32 %137 to i64
  %139 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %138
  %140 = bitcast %union.jvalue* %139 to float*
  store float %136, float* %140, align 8
  br label %172

141:                                              ; preds = %126
  %142 = load i32, i32* %13, align 4
  %143 = sext i32 %142 to i64
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %143
  %145 = load i8, i8* %144, align 1
  %146 = sext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %155

148:                                              ; preds = %141
  %149 = va_arg i8** %9, double
  store double %149, double* %21, align 8
  %150 = load double, double* %21, align 8
  %151 = load i32, i32* %13, align 4
  %152 = sext i32 %151 to i64
  %153 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %152
  %154 = bitcast %union.jvalue* %153 to double*
  store double %150, double* %154, align 8
  br label %171

155:                                              ; preds = %141
  %156 = load i32, i32* %13, align 4
  %157 = sext i32 %156 to i64
  %158 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %157
  %159 = load i8, i8* %158, align 1
  %160 = sext i8 %159 to i32
  %161 = icmp eq i32 %160, 76
  br i1 %161, label %162, label %170

162:                                              ; preds = %155
  %163 = va_arg i8** %9, i8*
  store i8* %163, i8** %22, align 8
  %164 = load i8*, i8** %22, align 8
  %165 = bitcast i8* %164 to %struct._jobject*
  %166 = load i32, i32* %13, align 4
  %167 = sext i32 %166 to i64
  %168 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %167
  %169 = bitcast %union.jvalue* %168 to %struct._jobject**
  store %struct._jobject* %165, %struct._jobject** %169, align 8
  br label %170

170:                                              ; preds = %162, %155
  br label %171

171:                                              ; preds = %170, %148
  br label %172

172:                                              ; preds = %171, %133
  br label %173

173:                                              ; preds = %172, %119
  br label %174

174:                                              ; preds = %173, %105
  br label %175

175:                                              ; preds = %174, %90
  br label %176

176:                                              ; preds = %175, %75
  br label %177

177:                                              ; preds = %176, %60
  br label %178

178:                                              ; preds = %177, %45
  br label %179

179:                                              ; preds = %178
  %180 = load i32, i32* %13, align 4
  %181 = add nsw i32 %180, 1
  store i32 %181, i32* %13, align 4
  br label %34, !llvm.loop !18

182:                                              ; preds = %34
  %183 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %184 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %183, align 8
  %185 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %184, i32 0, i32 69
  %186 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %185, align 8
  %187 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %188 = load %struct._jobject*, %struct._jobject** %6, align 8
  %189 = load %struct._jobject*, %struct._jobject** %7, align 8
  %190 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %191 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 0
  %192 = call zeroext i8 %186(%struct.JNINativeInterface_** noundef %187, %struct._jobject* noundef %188, %struct._jobject* noundef %189, %struct._jmethodID* noundef %190, %union.jvalue* noundef %191)
  store i8 %192, i8* %23, align 1
  %193 = bitcast i8** %9 to i8*
  call void @llvm.va_end(i8* %193)
  %194 = load i8, i8* %23, align 1
  ret i8 %194
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define zeroext i8 @JNI_CallNonvirtualBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, i8* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca i8*, align 8
  %11 = alloca [256 x i8], align 1
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i32, align 4
  %19 = alloca i32, align 4
  %20 = alloca i64, align 8
  %21 = alloca double, align 8
  %22 = alloca double, align 8
  %23 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  store i8* %4, i8** %10, align 8
  %24 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %25 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %25, i32 0, i32 0
  %27 = load i8*, i8** %26, align 8
  %28 = bitcast i8* %27 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %29 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %30 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 0
  %32 = call i32 %28(%struct.JNINativeInterface_** noundef %29, %struct._jmethodID* noundef %30, i8* noundef %31)
  store i32 %32, i32* %13, align 4
  store i32 0, i32* %14, align 4
  br label %33

33:                                               ; preds = %178, %5
  %34 = load i32, i32* %14, align 4
  %35 = load i32, i32* %13, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %181

37:                                               ; preds = %33
  %38 = load i32, i32* %14, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %39
  %41 = load i8, i8* %40, align 1
  %42 = sext i8 %41 to i32
  %43 = icmp eq i32 %42, 90
  br i1 %43, label %44, label %52

44:                                               ; preds = %37
  %45 = va_arg i8** %10, i32
  store i32 %45, i32* %15, align 4
  %46 = load i32, i32* %15, align 4
  %47 = trunc i32 %46 to i8
  %48 = load i32, i32* %14, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %47, i8* %51, align 8
  br label %177

52:                                               ; preds = %37
  %53 = load i32, i32* %14, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %54
  %56 = load i8, i8* %55, align 1
  %57 = sext i8 %56 to i32
  %58 = icmp eq i32 %57, 66
  br i1 %58, label %59, label %67

59:                                               ; preds = %52
  %60 = va_arg i8** %10, i32
  store i32 %60, i32* %16, align 4
  %61 = load i32, i32* %16, align 4
  %62 = trunc i32 %61 to i8
  %63 = load i32, i32* %14, align 4
  %64 = sext i32 %63 to i64
  %65 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %64
  %66 = bitcast %union.jvalue* %65 to i8*
  store i8 %62, i8* %66, align 8
  br label %176

67:                                               ; preds = %52
  %68 = load i32, i32* %14, align 4
  %69 = sext i32 %68 to i64
  %70 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %69
  %71 = load i8, i8* %70, align 1
  %72 = sext i8 %71 to i32
  %73 = icmp eq i32 %72, 67
  br i1 %73, label %74, label %82

74:                                               ; preds = %67
  %75 = va_arg i8** %10, i32
  store i32 %75, i32* %17, align 4
  %76 = load i32, i32* %17, align 4
  %77 = trunc i32 %76 to i16
  %78 = load i32, i32* %14, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %175

82:                                               ; preds = %67
  %83 = load i32, i32* %14, align 4
  %84 = sext i32 %83 to i64
  %85 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %84
  %86 = load i8, i8* %85, align 1
  %87 = sext i8 %86 to i32
  %88 = icmp eq i32 %87, 83
  br i1 %88, label %89, label %97

89:                                               ; preds = %82
  %90 = va_arg i8** %10, i32
  store i32 %90, i32* %18, align 4
  %91 = load i32, i32* %18, align 4
  %92 = trunc i32 %91 to i16
  %93 = load i32, i32* %14, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %94
  %96 = bitcast %union.jvalue* %95 to i16*
  store i16 %92, i16* %96, align 8
  br label %174

97:                                               ; preds = %82
  %98 = load i32, i32* %14, align 4
  %99 = sext i32 %98 to i64
  %100 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %99
  %101 = load i8, i8* %100, align 1
  %102 = sext i8 %101 to i32
  %103 = icmp eq i32 %102, 73
  br i1 %103, label %104, label %111

104:                                              ; preds = %97
  %105 = va_arg i8** %10, i32
  store i32 %105, i32* %19, align 4
  %106 = load i32, i32* %19, align 4
  %107 = load i32, i32* %14, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %108
  %110 = bitcast %union.jvalue* %109 to i32*
  store i32 %106, i32* %110, align 8
  br label %173

111:                                              ; preds = %97
  %112 = load i32, i32* %14, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %113
  %115 = load i8, i8* %114, align 1
  %116 = sext i8 %115 to i32
  %117 = icmp eq i32 %116, 74
  br i1 %117, label %118, label %125

118:                                              ; preds = %111
  %119 = va_arg i8** %10, i64
  store i64 %119, i64* %20, align 8
  %120 = load i64, i64* %20, align 8
  %121 = load i32, i32* %14, align 4
  %122 = sext i32 %121 to i64
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %120, i64* %124, align 8
  br label %172

125:                                              ; preds = %111
  %126 = load i32, i32* %14, align 4
  %127 = sext i32 %126 to i64
  %128 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %127
  %129 = load i8, i8* %128, align 1
  %130 = sext i8 %129 to i32
  %131 = icmp eq i32 %130, 70
  br i1 %131, label %132, label %140

132:                                              ; preds = %125
  %133 = va_arg i8** %10, double
  store double %133, double* %21, align 8
  %134 = load double, double* %21, align 8
  %135 = fptrunc double %134 to float
  %136 = load i32, i32* %14, align 4
  %137 = sext i32 %136 to i64
  %138 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %137
  %139 = bitcast %union.jvalue* %138 to float*
  store float %135, float* %139, align 8
  br label %171

140:                                              ; preds = %125
  %141 = load i32, i32* %14, align 4
  %142 = sext i32 %141 to i64
  %143 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %142
  %144 = load i8, i8* %143, align 1
  %145 = sext i8 %144 to i32
  %146 = icmp eq i32 %145, 68
  br i1 %146, label %147, label %154

147:                                              ; preds = %140
  %148 = va_arg i8** %10, double
  store double %148, double* %22, align 8
  %149 = load double, double* %22, align 8
  %150 = load i32, i32* %14, align 4
  %151 = sext i32 %150 to i64
  %152 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %151
  %153 = bitcast %union.jvalue* %152 to double*
  store double %149, double* %153, align 8
  br label %170

154:                                              ; preds = %140
  %155 = load i32, i32* %14, align 4
  %156 = sext i32 %155 to i64
  %157 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %156
  %158 = load i8, i8* %157, align 1
  %159 = sext i8 %158 to i32
  %160 = icmp eq i32 %159, 76
  br i1 %160, label %161, label %169

161:                                              ; preds = %154
  %162 = va_arg i8** %10, i8*
  store i8* %162, i8** %23, align 8
  %163 = load i8*, i8** %23, align 8
  %164 = bitcast i8* %163 to %struct._jobject*
  %165 = load i32, i32* %14, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %166
  %168 = bitcast %union.jvalue* %167 to %struct._jobject**
  store %struct._jobject* %164, %struct._jobject** %168, align 8
  br label %169

169:                                              ; preds = %161, %154
  br label %170

170:                                              ; preds = %169, %147
  br label %171

171:                                              ; preds = %170, %132
  br label %172

172:                                              ; preds = %171, %118
  br label %173

173:                                              ; preds = %172, %104
  br label %174

174:                                              ; preds = %173, %89
  br label %175

175:                                              ; preds = %174, %74
  br label %176

176:                                              ; preds = %175, %59
  br label %177

177:                                              ; preds = %176, %44
  br label %178

178:                                              ; preds = %177
  %179 = load i32, i32* %14, align 4
  %180 = add nsw i32 %179, 1
  store i32 %180, i32* %14, align 4
  br label %33, !llvm.loop !19

181:                                              ; preds = %33
  %182 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %183 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %182, align 8
  %184 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %183, i32 0, i32 69
  %185 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %184, align 8
  %186 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %187 = load %struct._jobject*, %struct._jobject** %7, align 8
  %188 = load %struct._jobject*, %struct._jobject** %8, align 8
  %189 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %190 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 0
  %191 = call zeroext i8 %185(%struct.JNINativeInterface_** noundef %186, %struct._jobject* noundef %187, %struct._jobject* noundef %188, %struct._jmethodID* noundef %189, %union.jvalue* noundef %190)
  ret i8 %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define zeroext i8 @JNI_CallStaticBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  %21 = alloca i8, align 1
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %22 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %22)
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %24 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %23, align 8
  %25 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %24, i32 0, i32 0
  %26 = load i8*, i8** %25, align 8
  %27 = bitcast i8* %26 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %28 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %29 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %31 = call i32 %27(%struct.JNINativeInterface_** noundef %28, %struct._jmethodID* noundef %29, i8* noundef %30)
  store i32 %31, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %32

32:                                               ; preds = %177, %3
  %33 = load i32, i32* %11, align 4
  %34 = load i32, i32* %10, align 4
  %35 = icmp slt i32 %33, %34
  br i1 %35, label %36, label %180

36:                                               ; preds = %32
  %37 = load i32, i32* %11, align 4
  %38 = sext i32 %37 to i64
  %39 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %38
  %40 = load i8, i8* %39, align 1
  %41 = sext i8 %40 to i32
  %42 = icmp eq i32 %41, 90
  br i1 %42, label %43, label %51

43:                                               ; preds = %36
  %44 = va_arg i8** %7, i32
  store i32 %44, i32* %12, align 4
  %45 = load i32, i32* %12, align 4
  %46 = trunc i32 %45 to i8
  %47 = load i32, i32* %11, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %176

51:                                               ; preds = %36
  %52 = load i32, i32* %11, align 4
  %53 = sext i32 %52 to i64
  %54 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %53
  %55 = load i8, i8* %54, align 1
  %56 = sext i8 %55 to i32
  %57 = icmp eq i32 %56, 66
  br i1 %57, label %58, label %66

58:                                               ; preds = %51
  %59 = va_arg i8** %7, i32
  store i32 %59, i32* %13, align 4
  %60 = load i32, i32* %13, align 4
  %61 = trunc i32 %60 to i8
  %62 = load i32, i32* %11, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %63
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %61, i8* %65, align 8
  br label %175

66:                                               ; preds = %51
  %67 = load i32, i32* %11, align 4
  %68 = sext i32 %67 to i64
  %69 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %68
  %70 = load i8, i8* %69, align 1
  %71 = sext i8 %70 to i32
  %72 = icmp eq i32 %71, 67
  br i1 %72, label %73, label %81

73:                                               ; preds = %66
  %74 = va_arg i8** %7, i32
  store i32 %74, i32* %14, align 4
  %75 = load i32, i32* %14, align 4
  %76 = trunc i32 %75 to i16
  %77 = load i32, i32* %11, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %174

81:                                               ; preds = %66
  %82 = load i32, i32* %11, align 4
  %83 = sext i32 %82 to i64
  %84 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %83
  %85 = load i8, i8* %84, align 1
  %86 = sext i8 %85 to i32
  %87 = icmp eq i32 %86, 83
  br i1 %87, label %88, label %96

88:                                               ; preds = %81
  %89 = va_arg i8** %7, i32
  store i32 %89, i32* %15, align 4
  %90 = load i32, i32* %15, align 4
  %91 = trunc i32 %90 to i16
  %92 = load i32, i32* %11, align 4
  %93 = sext i32 %92 to i64
  %94 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %93
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %91, i16* %95, align 8
  br label %173

96:                                               ; preds = %81
  %97 = load i32, i32* %11, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %98
  %100 = load i8, i8* %99, align 1
  %101 = sext i8 %100 to i32
  %102 = icmp eq i32 %101, 73
  br i1 %102, label %103, label %110

103:                                              ; preds = %96
  %104 = va_arg i8** %7, i32
  store i32 %104, i32* %16, align 4
  %105 = load i32, i32* %16, align 4
  %106 = load i32, i32* %11, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %107
  %109 = bitcast %union.jvalue* %108 to i32*
  store i32 %105, i32* %109, align 8
  br label %172

110:                                              ; preds = %96
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %112
  %114 = load i8, i8* %113, align 1
  %115 = sext i8 %114 to i32
  %116 = icmp eq i32 %115, 74
  br i1 %116, label %117, label %124

117:                                              ; preds = %110
  %118 = va_arg i8** %7, i64
  store i64 %118, i64* %17, align 8
  %119 = load i64, i64* %17, align 8
  %120 = load i32, i32* %11, align 4
  %121 = sext i32 %120 to i64
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %119, i64* %123, align 8
  br label %171

124:                                              ; preds = %110
  %125 = load i32, i32* %11, align 4
  %126 = sext i32 %125 to i64
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %126
  %128 = load i8, i8* %127, align 1
  %129 = sext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %139

131:                                              ; preds = %124
  %132 = va_arg i8** %7, double
  store double %132, double* %18, align 8
  %133 = load double, double* %18, align 8
  %134 = fptrunc double %133 to float
  %135 = load i32, i32* %11, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %136
  %138 = bitcast %union.jvalue* %137 to float*
  store float %134, float* %138, align 8
  br label %170

139:                                              ; preds = %124
  %140 = load i32, i32* %11, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %141
  %143 = load i8, i8* %142, align 1
  %144 = sext i8 %143 to i32
  %145 = icmp eq i32 %144, 68
  br i1 %145, label %146, label %153

146:                                              ; preds = %139
  %147 = va_arg i8** %7, double
  store double %147, double* %19, align 8
  %148 = load double, double* %19, align 8
  %149 = load i32, i32* %11, align 4
  %150 = sext i32 %149 to i64
  %151 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %150
  %152 = bitcast %union.jvalue* %151 to double*
  store double %148, double* %152, align 8
  br label %169

153:                                              ; preds = %139
  %154 = load i32, i32* %11, align 4
  %155 = sext i32 %154 to i64
  %156 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %155
  %157 = load i8, i8* %156, align 1
  %158 = sext i8 %157 to i32
  %159 = icmp eq i32 %158, 76
  br i1 %159, label %160, label %168

160:                                              ; preds = %153
  %161 = va_arg i8** %7, i8*
  store i8* %161, i8** %20, align 8
  %162 = load i8*, i8** %20, align 8
  %163 = bitcast i8* %162 to %struct._jobject*
  %164 = load i32, i32* %11, align 4
  %165 = sext i32 %164 to i64
  %166 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %165
  %167 = bitcast %union.jvalue* %166 to %struct._jobject**
  store %struct._jobject* %163, %struct._jobject** %167, align 8
  br label %168

168:                                              ; preds = %160, %153
  br label %169

169:                                              ; preds = %168, %146
  br label %170

170:                                              ; preds = %169, %131
  br label %171

171:                                              ; preds = %170, %117
  br label %172

172:                                              ; preds = %171, %103
  br label %173

173:                                              ; preds = %172, %88
  br label %174

174:                                              ; preds = %173, %73
  br label %175

175:                                              ; preds = %174, %58
  br label %176

176:                                              ; preds = %175, %43
  br label %177

177:                                              ; preds = %176
  %178 = load i32, i32* %11, align 4
  %179 = add nsw i32 %178, 1
  store i32 %179, i32* %11, align 4
  br label %32, !llvm.loop !20

180:                                              ; preds = %32
  %181 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %182 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %181, align 8
  %183 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %182, i32 0, i32 119
  %184 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %183, align 8
  %185 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %186 = load %struct._jobject*, %struct._jobject** %5, align 8
  %187 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %188 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  %189 = call zeroext i8 %184(%struct.JNINativeInterface_** noundef %185, %struct._jobject* noundef %186, %struct._jmethodID* noundef %187, %union.jvalue* noundef %188)
  store i8 %189, i8* %21, align 1
  %190 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %190)
  %191 = load i8, i8* %21, align 1
  ret i8 %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define zeroext i8 @JNI_CallStaticBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !21

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 119
  %183 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  %188 = call zeroext i8 %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret i8 %188
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define signext i8 @JNI_CallByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  %21 = alloca i8, align 1
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %22 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %22)
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %24 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %23, align 8
  %25 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %24, i32 0, i32 0
  %26 = load i8*, i8** %25, align 8
  %27 = bitcast i8* %26 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %28 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %29 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %31 = call i32 %27(%struct.JNINativeInterface_** noundef %28, %struct._jmethodID* noundef %29, i8* noundef %30)
  store i32 %31, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %32

32:                                               ; preds = %177, %3
  %33 = load i32, i32* %11, align 4
  %34 = load i32, i32* %10, align 4
  %35 = icmp slt i32 %33, %34
  br i1 %35, label %36, label %180

36:                                               ; preds = %32
  %37 = load i32, i32* %11, align 4
  %38 = sext i32 %37 to i64
  %39 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %38
  %40 = load i8, i8* %39, align 1
  %41 = sext i8 %40 to i32
  %42 = icmp eq i32 %41, 90
  br i1 %42, label %43, label %51

43:                                               ; preds = %36
  %44 = va_arg i8** %7, i32
  store i32 %44, i32* %12, align 4
  %45 = load i32, i32* %12, align 4
  %46 = trunc i32 %45 to i8
  %47 = load i32, i32* %11, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %176

51:                                               ; preds = %36
  %52 = load i32, i32* %11, align 4
  %53 = sext i32 %52 to i64
  %54 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %53
  %55 = load i8, i8* %54, align 1
  %56 = sext i8 %55 to i32
  %57 = icmp eq i32 %56, 66
  br i1 %57, label %58, label %66

58:                                               ; preds = %51
  %59 = va_arg i8** %7, i32
  store i32 %59, i32* %13, align 4
  %60 = load i32, i32* %13, align 4
  %61 = trunc i32 %60 to i8
  %62 = load i32, i32* %11, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %63
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %61, i8* %65, align 8
  br label %175

66:                                               ; preds = %51
  %67 = load i32, i32* %11, align 4
  %68 = sext i32 %67 to i64
  %69 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %68
  %70 = load i8, i8* %69, align 1
  %71 = sext i8 %70 to i32
  %72 = icmp eq i32 %71, 67
  br i1 %72, label %73, label %81

73:                                               ; preds = %66
  %74 = va_arg i8** %7, i32
  store i32 %74, i32* %14, align 4
  %75 = load i32, i32* %14, align 4
  %76 = trunc i32 %75 to i16
  %77 = load i32, i32* %11, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %174

81:                                               ; preds = %66
  %82 = load i32, i32* %11, align 4
  %83 = sext i32 %82 to i64
  %84 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %83
  %85 = load i8, i8* %84, align 1
  %86 = sext i8 %85 to i32
  %87 = icmp eq i32 %86, 83
  br i1 %87, label %88, label %96

88:                                               ; preds = %81
  %89 = va_arg i8** %7, i32
  store i32 %89, i32* %15, align 4
  %90 = load i32, i32* %15, align 4
  %91 = trunc i32 %90 to i16
  %92 = load i32, i32* %11, align 4
  %93 = sext i32 %92 to i64
  %94 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %93
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %91, i16* %95, align 8
  br label %173

96:                                               ; preds = %81
  %97 = load i32, i32* %11, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %98
  %100 = load i8, i8* %99, align 1
  %101 = sext i8 %100 to i32
  %102 = icmp eq i32 %101, 73
  br i1 %102, label %103, label %110

103:                                              ; preds = %96
  %104 = va_arg i8** %7, i32
  store i32 %104, i32* %16, align 4
  %105 = load i32, i32* %16, align 4
  %106 = load i32, i32* %11, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %107
  %109 = bitcast %union.jvalue* %108 to i32*
  store i32 %105, i32* %109, align 8
  br label %172

110:                                              ; preds = %96
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %112
  %114 = load i8, i8* %113, align 1
  %115 = sext i8 %114 to i32
  %116 = icmp eq i32 %115, 74
  br i1 %116, label %117, label %124

117:                                              ; preds = %110
  %118 = va_arg i8** %7, i64
  store i64 %118, i64* %17, align 8
  %119 = load i64, i64* %17, align 8
  %120 = load i32, i32* %11, align 4
  %121 = sext i32 %120 to i64
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %119, i64* %123, align 8
  br label %171

124:                                              ; preds = %110
  %125 = load i32, i32* %11, align 4
  %126 = sext i32 %125 to i64
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %126
  %128 = load i8, i8* %127, align 1
  %129 = sext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %139

131:                                              ; preds = %124
  %132 = va_arg i8** %7, double
  store double %132, double* %18, align 8
  %133 = load double, double* %18, align 8
  %134 = fptrunc double %133 to float
  %135 = load i32, i32* %11, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %136
  %138 = bitcast %union.jvalue* %137 to float*
  store float %134, float* %138, align 8
  br label %170

139:                                              ; preds = %124
  %140 = load i32, i32* %11, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %141
  %143 = load i8, i8* %142, align 1
  %144 = sext i8 %143 to i32
  %145 = icmp eq i32 %144, 68
  br i1 %145, label %146, label %153

146:                                              ; preds = %139
  %147 = va_arg i8** %7, double
  store double %147, double* %19, align 8
  %148 = load double, double* %19, align 8
  %149 = load i32, i32* %11, align 4
  %150 = sext i32 %149 to i64
  %151 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %150
  %152 = bitcast %union.jvalue* %151 to double*
  store double %148, double* %152, align 8
  br label %169

153:                                              ; preds = %139
  %154 = load i32, i32* %11, align 4
  %155 = sext i32 %154 to i64
  %156 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %155
  %157 = load i8, i8* %156, align 1
  %158 = sext i8 %157 to i32
  %159 = icmp eq i32 %158, 76
  br i1 %159, label %160, label %168

160:                                              ; preds = %153
  %161 = va_arg i8** %7, i8*
  store i8* %161, i8** %20, align 8
  %162 = load i8*, i8** %20, align 8
  %163 = bitcast i8* %162 to %struct._jobject*
  %164 = load i32, i32* %11, align 4
  %165 = sext i32 %164 to i64
  %166 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %165
  %167 = bitcast %union.jvalue* %166 to %struct._jobject**
  store %struct._jobject* %163, %struct._jobject** %167, align 8
  br label %168

168:                                              ; preds = %160, %153
  br label %169

169:                                              ; preds = %168, %146
  br label %170

170:                                              ; preds = %169, %131
  br label %171

171:                                              ; preds = %170, %117
  br label %172

172:                                              ; preds = %171, %103
  br label %173

173:                                              ; preds = %172, %88
  br label %174

174:                                              ; preds = %173, %73
  br label %175

175:                                              ; preds = %174, %58
  br label %176

176:                                              ; preds = %175, %43
  br label %177

177:                                              ; preds = %176
  %178 = load i32, i32* %11, align 4
  %179 = add nsw i32 %178, 1
  store i32 %179, i32* %11, align 4
  br label %32, !llvm.loop !22

180:                                              ; preds = %32
  %181 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %182 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %181, align 8
  %183 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %182, i32 0, i32 42
  %184 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %183, align 8
  %185 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %186 = load %struct._jobject*, %struct._jobject** %5, align 8
  %187 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %188 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  %189 = call signext i8 %184(%struct.JNINativeInterface_** noundef %185, %struct._jobject* noundef %186, %struct._jmethodID* noundef %187, %union.jvalue* noundef %188)
  store i8 %189, i8* %21, align 1
  %190 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %190)
  %191 = load i8, i8* %21, align 1
  ret i8 %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define signext i8 @JNI_CallByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !23

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 42
  %183 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  %188 = call signext i8 %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret i8 %188
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define signext i8 @JNI_CallNonvirtualByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i8*, align 8
  %10 = alloca [256 x i8], align 1
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i32, align 4
  %19 = alloca i64, align 8
  %20 = alloca double, align 8
  %21 = alloca double, align 8
  %22 = alloca i8*, align 8
  %23 = alloca i8, align 1
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %24 = bitcast i8** %9 to i8*
  call void @llvm.va_start(i8* %24)
  %25 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %26 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %25, align 8
  %27 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %26, i32 0, i32 0
  %28 = load i8*, i8** %27, align 8
  %29 = bitcast i8* %28 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %30 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %31 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 0
  %33 = call i32 %29(%struct.JNINativeInterface_** noundef %30, %struct._jmethodID* noundef %31, i8* noundef %32)
  store i32 %33, i32* %12, align 4
  store i32 0, i32* %13, align 4
  br label %34

34:                                               ; preds = %179, %4
  %35 = load i32, i32* %13, align 4
  %36 = load i32, i32* %12, align 4
  %37 = icmp slt i32 %35, %36
  br i1 %37, label %38, label %182

38:                                               ; preds = %34
  %39 = load i32, i32* %13, align 4
  %40 = sext i32 %39 to i64
  %41 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %40
  %42 = load i8, i8* %41, align 1
  %43 = sext i8 %42 to i32
  %44 = icmp eq i32 %43, 90
  br i1 %44, label %45, label %53

45:                                               ; preds = %38
  %46 = va_arg i8** %9, i32
  store i32 %46, i32* %14, align 4
  %47 = load i32, i32* %14, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %13, align 4
  %50 = sext i32 %49 to i64
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %48, i8* %52, align 8
  br label %178

53:                                               ; preds = %38
  %54 = load i32, i32* %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %55
  %57 = load i8, i8* %56, align 1
  %58 = sext i8 %57 to i32
  %59 = icmp eq i32 %58, 66
  br i1 %59, label %60, label %68

60:                                               ; preds = %53
  %61 = va_arg i8** %9, i32
  store i32 %61, i32* %15, align 4
  %62 = load i32, i32* %15, align 4
  %63 = trunc i32 %62 to i8
  %64 = load i32, i32* %13, align 4
  %65 = sext i32 %64 to i64
  %66 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %65
  %67 = bitcast %union.jvalue* %66 to i8*
  store i8 %63, i8* %67, align 8
  br label %177

68:                                               ; preds = %53
  %69 = load i32, i32* %13, align 4
  %70 = sext i32 %69 to i64
  %71 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %70
  %72 = load i8, i8* %71, align 1
  %73 = sext i8 %72 to i32
  %74 = icmp eq i32 %73, 67
  br i1 %74, label %75, label %83

75:                                               ; preds = %68
  %76 = va_arg i8** %9, i32
  store i32 %76, i32* %16, align 4
  %77 = load i32, i32* %16, align 4
  %78 = trunc i32 %77 to i16
  %79 = load i32, i32* %13, align 4
  %80 = sext i32 %79 to i64
  %81 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %80
  %82 = bitcast %union.jvalue* %81 to i16*
  store i16 %78, i16* %82, align 8
  br label %176

83:                                               ; preds = %68
  %84 = load i32, i32* %13, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %85
  %87 = load i8, i8* %86, align 1
  %88 = sext i8 %87 to i32
  %89 = icmp eq i32 %88, 83
  br i1 %89, label %90, label %98

90:                                               ; preds = %83
  %91 = va_arg i8** %9, i32
  store i32 %91, i32* %17, align 4
  %92 = load i32, i32* %17, align 4
  %93 = trunc i32 %92 to i16
  %94 = load i32, i32* %13, align 4
  %95 = sext i32 %94 to i64
  %96 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %95
  %97 = bitcast %union.jvalue* %96 to i16*
  store i16 %93, i16* %97, align 8
  br label %175

98:                                               ; preds = %83
  %99 = load i32, i32* %13, align 4
  %100 = sext i32 %99 to i64
  %101 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %100
  %102 = load i8, i8* %101, align 1
  %103 = sext i8 %102 to i32
  %104 = icmp eq i32 %103, 73
  br i1 %104, label %105, label %112

105:                                              ; preds = %98
  %106 = va_arg i8** %9, i32
  store i32 %106, i32* %18, align 4
  %107 = load i32, i32* %18, align 4
  %108 = load i32, i32* %13, align 4
  %109 = sext i32 %108 to i64
  %110 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %109
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %107, i32* %111, align 8
  br label %174

112:                                              ; preds = %98
  %113 = load i32, i32* %13, align 4
  %114 = sext i32 %113 to i64
  %115 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %114
  %116 = load i8, i8* %115, align 1
  %117 = sext i8 %116 to i32
  %118 = icmp eq i32 %117, 74
  br i1 %118, label %119, label %126

119:                                              ; preds = %112
  %120 = va_arg i8** %9, i64
  store i64 %120, i64* %19, align 8
  %121 = load i64, i64* %19, align 8
  %122 = load i32, i32* %13, align 4
  %123 = sext i32 %122 to i64
  %124 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %123
  %125 = bitcast %union.jvalue* %124 to i64*
  store i64 %121, i64* %125, align 8
  br label %173

126:                                              ; preds = %112
  %127 = load i32, i32* %13, align 4
  %128 = sext i32 %127 to i64
  %129 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %128
  %130 = load i8, i8* %129, align 1
  %131 = sext i8 %130 to i32
  %132 = icmp eq i32 %131, 70
  br i1 %132, label %133, label %141

133:                                              ; preds = %126
  %134 = va_arg i8** %9, double
  store double %134, double* %20, align 8
  %135 = load double, double* %20, align 8
  %136 = fptrunc double %135 to float
  %137 = load i32, i32* %13, align 4
  %138 = sext i32 %137 to i64
  %139 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %138
  %140 = bitcast %union.jvalue* %139 to float*
  store float %136, float* %140, align 8
  br label %172

141:                                              ; preds = %126
  %142 = load i32, i32* %13, align 4
  %143 = sext i32 %142 to i64
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %143
  %145 = load i8, i8* %144, align 1
  %146 = sext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %155

148:                                              ; preds = %141
  %149 = va_arg i8** %9, double
  store double %149, double* %21, align 8
  %150 = load double, double* %21, align 8
  %151 = load i32, i32* %13, align 4
  %152 = sext i32 %151 to i64
  %153 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %152
  %154 = bitcast %union.jvalue* %153 to double*
  store double %150, double* %154, align 8
  br label %171

155:                                              ; preds = %141
  %156 = load i32, i32* %13, align 4
  %157 = sext i32 %156 to i64
  %158 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %157
  %159 = load i8, i8* %158, align 1
  %160 = sext i8 %159 to i32
  %161 = icmp eq i32 %160, 76
  br i1 %161, label %162, label %170

162:                                              ; preds = %155
  %163 = va_arg i8** %9, i8*
  store i8* %163, i8** %22, align 8
  %164 = load i8*, i8** %22, align 8
  %165 = bitcast i8* %164 to %struct._jobject*
  %166 = load i32, i32* %13, align 4
  %167 = sext i32 %166 to i64
  %168 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %167
  %169 = bitcast %union.jvalue* %168 to %struct._jobject**
  store %struct._jobject* %165, %struct._jobject** %169, align 8
  br label %170

170:                                              ; preds = %162, %155
  br label %171

171:                                              ; preds = %170, %148
  br label %172

172:                                              ; preds = %171, %133
  br label %173

173:                                              ; preds = %172, %119
  br label %174

174:                                              ; preds = %173, %105
  br label %175

175:                                              ; preds = %174, %90
  br label %176

176:                                              ; preds = %175, %75
  br label %177

177:                                              ; preds = %176, %60
  br label %178

178:                                              ; preds = %177, %45
  br label %179

179:                                              ; preds = %178
  %180 = load i32, i32* %13, align 4
  %181 = add nsw i32 %180, 1
  store i32 %181, i32* %13, align 4
  br label %34, !llvm.loop !24

182:                                              ; preds = %34
  %183 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %184 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %183, align 8
  %185 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %184, i32 0, i32 72
  %186 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %185, align 8
  %187 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %188 = load %struct._jobject*, %struct._jobject** %6, align 8
  %189 = load %struct._jobject*, %struct._jobject** %7, align 8
  %190 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %191 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 0
  %192 = call signext i8 %186(%struct.JNINativeInterface_** noundef %187, %struct._jobject* noundef %188, %struct._jobject* noundef %189, %struct._jmethodID* noundef %190, %union.jvalue* noundef %191)
  store i8 %192, i8* %23, align 1
  %193 = bitcast i8** %9 to i8*
  call void @llvm.va_end(i8* %193)
  %194 = load i8, i8* %23, align 1
  ret i8 %194
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define signext i8 @JNI_CallNonvirtualByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, i8* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca i8*, align 8
  %11 = alloca [256 x i8], align 1
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i32, align 4
  %19 = alloca i32, align 4
  %20 = alloca i64, align 8
  %21 = alloca double, align 8
  %22 = alloca double, align 8
  %23 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  store i8* %4, i8** %10, align 8
  %24 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %25 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %25, i32 0, i32 0
  %27 = load i8*, i8** %26, align 8
  %28 = bitcast i8* %27 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %29 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %30 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 0
  %32 = call i32 %28(%struct.JNINativeInterface_** noundef %29, %struct._jmethodID* noundef %30, i8* noundef %31)
  store i32 %32, i32* %13, align 4
  store i32 0, i32* %14, align 4
  br label %33

33:                                               ; preds = %178, %5
  %34 = load i32, i32* %14, align 4
  %35 = load i32, i32* %13, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %181

37:                                               ; preds = %33
  %38 = load i32, i32* %14, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %39
  %41 = load i8, i8* %40, align 1
  %42 = sext i8 %41 to i32
  %43 = icmp eq i32 %42, 90
  br i1 %43, label %44, label %52

44:                                               ; preds = %37
  %45 = va_arg i8** %10, i32
  store i32 %45, i32* %15, align 4
  %46 = load i32, i32* %15, align 4
  %47 = trunc i32 %46 to i8
  %48 = load i32, i32* %14, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %47, i8* %51, align 8
  br label %177

52:                                               ; preds = %37
  %53 = load i32, i32* %14, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %54
  %56 = load i8, i8* %55, align 1
  %57 = sext i8 %56 to i32
  %58 = icmp eq i32 %57, 66
  br i1 %58, label %59, label %67

59:                                               ; preds = %52
  %60 = va_arg i8** %10, i32
  store i32 %60, i32* %16, align 4
  %61 = load i32, i32* %16, align 4
  %62 = trunc i32 %61 to i8
  %63 = load i32, i32* %14, align 4
  %64 = sext i32 %63 to i64
  %65 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %64
  %66 = bitcast %union.jvalue* %65 to i8*
  store i8 %62, i8* %66, align 8
  br label %176

67:                                               ; preds = %52
  %68 = load i32, i32* %14, align 4
  %69 = sext i32 %68 to i64
  %70 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %69
  %71 = load i8, i8* %70, align 1
  %72 = sext i8 %71 to i32
  %73 = icmp eq i32 %72, 67
  br i1 %73, label %74, label %82

74:                                               ; preds = %67
  %75 = va_arg i8** %10, i32
  store i32 %75, i32* %17, align 4
  %76 = load i32, i32* %17, align 4
  %77 = trunc i32 %76 to i16
  %78 = load i32, i32* %14, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %175

82:                                               ; preds = %67
  %83 = load i32, i32* %14, align 4
  %84 = sext i32 %83 to i64
  %85 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %84
  %86 = load i8, i8* %85, align 1
  %87 = sext i8 %86 to i32
  %88 = icmp eq i32 %87, 83
  br i1 %88, label %89, label %97

89:                                               ; preds = %82
  %90 = va_arg i8** %10, i32
  store i32 %90, i32* %18, align 4
  %91 = load i32, i32* %18, align 4
  %92 = trunc i32 %91 to i16
  %93 = load i32, i32* %14, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %94
  %96 = bitcast %union.jvalue* %95 to i16*
  store i16 %92, i16* %96, align 8
  br label %174

97:                                               ; preds = %82
  %98 = load i32, i32* %14, align 4
  %99 = sext i32 %98 to i64
  %100 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %99
  %101 = load i8, i8* %100, align 1
  %102 = sext i8 %101 to i32
  %103 = icmp eq i32 %102, 73
  br i1 %103, label %104, label %111

104:                                              ; preds = %97
  %105 = va_arg i8** %10, i32
  store i32 %105, i32* %19, align 4
  %106 = load i32, i32* %19, align 4
  %107 = load i32, i32* %14, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %108
  %110 = bitcast %union.jvalue* %109 to i32*
  store i32 %106, i32* %110, align 8
  br label %173

111:                                              ; preds = %97
  %112 = load i32, i32* %14, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %113
  %115 = load i8, i8* %114, align 1
  %116 = sext i8 %115 to i32
  %117 = icmp eq i32 %116, 74
  br i1 %117, label %118, label %125

118:                                              ; preds = %111
  %119 = va_arg i8** %10, i64
  store i64 %119, i64* %20, align 8
  %120 = load i64, i64* %20, align 8
  %121 = load i32, i32* %14, align 4
  %122 = sext i32 %121 to i64
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %120, i64* %124, align 8
  br label %172

125:                                              ; preds = %111
  %126 = load i32, i32* %14, align 4
  %127 = sext i32 %126 to i64
  %128 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %127
  %129 = load i8, i8* %128, align 1
  %130 = sext i8 %129 to i32
  %131 = icmp eq i32 %130, 70
  br i1 %131, label %132, label %140

132:                                              ; preds = %125
  %133 = va_arg i8** %10, double
  store double %133, double* %21, align 8
  %134 = load double, double* %21, align 8
  %135 = fptrunc double %134 to float
  %136 = load i32, i32* %14, align 4
  %137 = sext i32 %136 to i64
  %138 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %137
  %139 = bitcast %union.jvalue* %138 to float*
  store float %135, float* %139, align 8
  br label %171

140:                                              ; preds = %125
  %141 = load i32, i32* %14, align 4
  %142 = sext i32 %141 to i64
  %143 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %142
  %144 = load i8, i8* %143, align 1
  %145 = sext i8 %144 to i32
  %146 = icmp eq i32 %145, 68
  br i1 %146, label %147, label %154

147:                                              ; preds = %140
  %148 = va_arg i8** %10, double
  store double %148, double* %22, align 8
  %149 = load double, double* %22, align 8
  %150 = load i32, i32* %14, align 4
  %151 = sext i32 %150 to i64
  %152 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %151
  %153 = bitcast %union.jvalue* %152 to double*
  store double %149, double* %153, align 8
  br label %170

154:                                              ; preds = %140
  %155 = load i32, i32* %14, align 4
  %156 = sext i32 %155 to i64
  %157 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %156
  %158 = load i8, i8* %157, align 1
  %159 = sext i8 %158 to i32
  %160 = icmp eq i32 %159, 76
  br i1 %160, label %161, label %169

161:                                              ; preds = %154
  %162 = va_arg i8** %10, i8*
  store i8* %162, i8** %23, align 8
  %163 = load i8*, i8** %23, align 8
  %164 = bitcast i8* %163 to %struct._jobject*
  %165 = load i32, i32* %14, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %166
  %168 = bitcast %union.jvalue* %167 to %struct._jobject**
  store %struct._jobject* %164, %struct._jobject** %168, align 8
  br label %169

169:                                              ; preds = %161, %154
  br label %170

170:                                              ; preds = %169, %147
  br label %171

171:                                              ; preds = %170, %132
  br label %172

172:                                              ; preds = %171, %118
  br label %173

173:                                              ; preds = %172, %104
  br label %174

174:                                              ; preds = %173, %89
  br label %175

175:                                              ; preds = %174, %74
  br label %176

176:                                              ; preds = %175, %59
  br label %177

177:                                              ; preds = %176, %44
  br label %178

178:                                              ; preds = %177
  %179 = load i32, i32* %14, align 4
  %180 = add nsw i32 %179, 1
  store i32 %180, i32* %14, align 4
  br label %33, !llvm.loop !25

181:                                              ; preds = %33
  %182 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %183 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %182, align 8
  %184 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %183, i32 0, i32 72
  %185 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %184, align 8
  %186 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %187 = load %struct._jobject*, %struct._jobject** %7, align 8
  %188 = load %struct._jobject*, %struct._jobject** %8, align 8
  %189 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %190 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 0
  %191 = call signext i8 %185(%struct.JNINativeInterface_** noundef %186, %struct._jobject* noundef %187, %struct._jobject* noundef %188, %struct._jmethodID* noundef %189, %union.jvalue* noundef %190)
  ret i8 %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define signext i8 @JNI_CallStaticByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  %21 = alloca i8, align 1
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %22 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %22)
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %24 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %23, align 8
  %25 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %24, i32 0, i32 0
  %26 = load i8*, i8** %25, align 8
  %27 = bitcast i8* %26 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %28 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %29 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %31 = call i32 %27(%struct.JNINativeInterface_** noundef %28, %struct._jmethodID* noundef %29, i8* noundef %30)
  store i32 %31, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %32

32:                                               ; preds = %177, %3
  %33 = load i32, i32* %11, align 4
  %34 = load i32, i32* %10, align 4
  %35 = icmp slt i32 %33, %34
  br i1 %35, label %36, label %180

36:                                               ; preds = %32
  %37 = load i32, i32* %11, align 4
  %38 = sext i32 %37 to i64
  %39 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %38
  %40 = load i8, i8* %39, align 1
  %41 = sext i8 %40 to i32
  %42 = icmp eq i32 %41, 90
  br i1 %42, label %43, label %51

43:                                               ; preds = %36
  %44 = va_arg i8** %7, i32
  store i32 %44, i32* %12, align 4
  %45 = load i32, i32* %12, align 4
  %46 = trunc i32 %45 to i8
  %47 = load i32, i32* %11, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %176

51:                                               ; preds = %36
  %52 = load i32, i32* %11, align 4
  %53 = sext i32 %52 to i64
  %54 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %53
  %55 = load i8, i8* %54, align 1
  %56 = sext i8 %55 to i32
  %57 = icmp eq i32 %56, 66
  br i1 %57, label %58, label %66

58:                                               ; preds = %51
  %59 = va_arg i8** %7, i32
  store i32 %59, i32* %13, align 4
  %60 = load i32, i32* %13, align 4
  %61 = trunc i32 %60 to i8
  %62 = load i32, i32* %11, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %63
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %61, i8* %65, align 8
  br label %175

66:                                               ; preds = %51
  %67 = load i32, i32* %11, align 4
  %68 = sext i32 %67 to i64
  %69 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %68
  %70 = load i8, i8* %69, align 1
  %71 = sext i8 %70 to i32
  %72 = icmp eq i32 %71, 67
  br i1 %72, label %73, label %81

73:                                               ; preds = %66
  %74 = va_arg i8** %7, i32
  store i32 %74, i32* %14, align 4
  %75 = load i32, i32* %14, align 4
  %76 = trunc i32 %75 to i16
  %77 = load i32, i32* %11, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %174

81:                                               ; preds = %66
  %82 = load i32, i32* %11, align 4
  %83 = sext i32 %82 to i64
  %84 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %83
  %85 = load i8, i8* %84, align 1
  %86 = sext i8 %85 to i32
  %87 = icmp eq i32 %86, 83
  br i1 %87, label %88, label %96

88:                                               ; preds = %81
  %89 = va_arg i8** %7, i32
  store i32 %89, i32* %15, align 4
  %90 = load i32, i32* %15, align 4
  %91 = trunc i32 %90 to i16
  %92 = load i32, i32* %11, align 4
  %93 = sext i32 %92 to i64
  %94 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %93
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %91, i16* %95, align 8
  br label %173

96:                                               ; preds = %81
  %97 = load i32, i32* %11, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %98
  %100 = load i8, i8* %99, align 1
  %101 = sext i8 %100 to i32
  %102 = icmp eq i32 %101, 73
  br i1 %102, label %103, label %110

103:                                              ; preds = %96
  %104 = va_arg i8** %7, i32
  store i32 %104, i32* %16, align 4
  %105 = load i32, i32* %16, align 4
  %106 = load i32, i32* %11, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %107
  %109 = bitcast %union.jvalue* %108 to i32*
  store i32 %105, i32* %109, align 8
  br label %172

110:                                              ; preds = %96
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %112
  %114 = load i8, i8* %113, align 1
  %115 = sext i8 %114 to i32
  %116 = icmp eq i32 %115, 74
  br i1 %116, label %117, label %124

117:                                              ; preds = %110
  %118 = va_arg i8** %7, i64
  store i64 %118, i64* %17, align 8
  %119 = load i64, i64* %17, align 8
  %120 = load i32, i32* %11, align 4
  %121 = sext i32 %120 to i64
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %119, i64* %123, align 8
  br label %171

124:                                              ; preds = %110
  %125 = load i32, i32* %11, align 4
  %126 = sext i32 %125 to i64
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %126
  %128 = load i8, i8* %127, align 1
  %129 = sext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %139

131:                                              ; preds = %124
  %132 = va_arg i8** %7, double
  store double %132, double* %18, align 8
  %133 = load double, double* %18, align 8
  %134 = fptrunc double %133 to float
  %135 = load i32, i32* %11, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %136
  %138 = bitcast %union.jvalue* %137 to float*
  store float %134, float* %138, align 8
  br label %170

139:                                              ; preds = %124
  %140 = load i32, i32* %11, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %141
  %143 = load i8, i8* %142, align 1
  %144 = sext i8 %143 to i32
  %145 = icmp eq i32 %144, 68
  br i1 %145, label %146, label %153

146:                                              ; preds = %139
  %147 = va_arg i8** %7, double
  store double %147, double* %19, align 8
  %148 = load double, double* %19, align 8
  %149 = load i32, i32* %11, align 4
  %150 = sext i32 %149 to i64
  %151 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %150
  %152 = bitcast %union.jvalue* %151 to double*
  store double %148, double* %152, align 8
  br label %169

153:                                              ; preds = %139
  %154 = load i32, i32* %11, align 4
  %155 = sext i32 %154 to i64
  %156 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %155
  %157 = load i8, i8* %156, align 1
  %158 = sext i8 %157 to i32
  %159 = icmp eq i32 %158, 76
  br i1 %159, label %160, label %168

160:                                              ; preds = %153
  %161 = va_arg i8** %7, i8*
  store i8* %161, i8** %20, align 8
  %162 = load i8*, i8** %20, align 8
  %163 = bitcast i8* %162 to %struct._jobject*
  %164 = load i32, i32* %11, align 4
  %165 = sext i32 %164 to i64
  %166 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %165
  %167 = bitcast %union.jvalue* %166 to %struct._jobject**
  store %struct._jobject* %163, %struct._jobject** %167, align 8
  br label %168

168:                                              ; preds = %160, %153
  br label %169

169:                                              ; preds = %168, %146
  br label %170

170:                                              ; preds = %169, %131
  br label %171

171:                                              ; preds = %170, %117
  br label %172

172:                                              ; preds = %171, %103
  br label %173

173:                                              ; preds = %172, %88
  br label %174

174:                                              ; preds = %173, %73
  br label %175

175:                                              ; preds = %174, %58
  br label %176

176:                                              ; preds = %175, %43
  br label %177

177:                                              ; preds = %176
  %178 = load i32, i32* %11, align 4
  %179 = add nsw i32 %178, 1
  store i32 %179, i32* %11, align 4
  br label %32, !llvm.loop !26

180:                                              ; preds = %32
  %181 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %182 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %181, align 8
  %183 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %182, i32 0, i32 122
  %184 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %183, align 8
  %185 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %186 = load %struct._jobject*, %struct._jobject** %5, align 8
  %187 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %188 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  %189 = call signext i8 %184(%struct.JNINativeInterface_** noundef %185, %struct._jobject* noundef %186, %struct._jmethodID* noundef %187, %union.jvalue* noundef %188)
  store i8 %189, i8* %21, align 1
  %190 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %190)
  %191 = load i8, i8* %21, align 1
  ret i8 %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define signext i8 @JNI_CallStaticByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !27

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 122
  %183 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  %188 = call signext i8 %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret i8 %188
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define zeroext i16 @JNI_CallCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  %21 = alloca i16, align 2
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %22 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %22)
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %24 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %23, align 8
  %25 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %24, i32 0, i32 0
  %26 = load i8*, i8** %25, align 8
  %27 = bitcast i8* %26 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %28 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %29 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %31 = call i32 %27(%struct.JNINativeInterface_** noundef %28, %struct._jmethodID* noundef %29, i8* noundef %30)
  store i32 %31, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %32

32:                                               ; preds = %177, %3
  %33 = load i32, i32* %11, align 4
  %34 = load i32, i32* %10, align 4
  %35 = icmp slt i32 %33, %34
  br i1 %35, label %36, label %180

36:                                               ; preds = %32
  %37 = load i32, i32* %11, align 4
  %38 = sext i32 %37 to i64
  %39 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %38
  %40 = load i8, i8* %39, align 1
  %41 = sext i8 %40 to i32
  %42 = icmp eq i32 %41, 90
  br i1 %42, label %43, label %51

43:                                               ; preds = %36
  %44 = va_arg i8** %7, i32
  store i32 %44, i32* %12, align 4
  %45 = load i32, i32* %12, align 4
  %46 = trunc i32 %45 to i8
  %47 = load i32, i32* %11, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %176

51:                                               ; preds = %36
  %52 = load i32, i32* %11, align 4
  %53 = sext i32 %52 to i64
  %54 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %53
  %55 = load i8, i8* %54, align 1
  %56 = sext i8 %55 to i32
  %57 = icmp eq i32 %56, 66
  br i1 %57, label %58, label %66

58:                                               ; preds = %51
  %59 = va_arg i8** %7, i32
  store i32 %59, i32* %13, align 4
  %60 = load i32, i32* %13, align 4
  %61 = trunc i32 %60 to i8
  %62 = load i32, i32* %11, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %63
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %61, i8* %65, align 8
  br label %175

66:                                               ; preds = %51
  %67 = load i32, i32* %11, align 4
  %68 = sext i32 %67 to i64
  %69 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %68
  %70 = load i8, i8* %69, align 1
  %71 = sext i8 %70 to i32
  %72 = icmp eq i32 %71, 67
  br i1 %72, label %73, label %81

73:                                               ; preds = %66
  %74 = va_arg i8** %7, i32
  store i32 %74, i32* %14, align 4
  %75 = load i32, i32* %14, align 4
  %76 = trunc i32 %75 to i16
  %77 = load i32, i32* %11, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %174

81:                                               ; preds = %66
  %82 = load i32, i32* %11, align 4
  %83 = sext i32 %82 to i64
  %84 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %83
  %85 = load i8, i8* %84, align 1
  %86 = sext i8 %85 to i32
  %87 = icmp eq i32 %86, 83
  br i1 %87, label %88, label %96

88:                                               ; preds = %81
  %89 = va_arg i8** %7, i32
  store i32 %89, i32* %15, align 4
  %90 = load i32, i32* %15, align 4
  %91 = trunc i32 %90 to i16
  %92 = load i32, i32* %11, align 4
  %93 = sext i32 %92 to i64
  %94 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %93
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %91, i16* %95, align 8
  br label %173

96:                                               ; preds = %81
  %97 = load i32, i32* %11, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %98
  %100 = load i8, i8* %99, align 1
  %101 = sext i8 %100 to i32
  %102 = icmp eq i32 %101, 73
  br i1 %102, label %103, label %110

103:                                              ; preds = %96
  %104 = va_arg i8** %7, i32
  store i32 %104, i32* %16, align 4
  %105 = load i32, i32* %16, align 4
  %106 = load i32, i32* %11, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %107
  %109 = bitcast %union.jvalue* %108 to i32*
  store i32 %105, i32* %109, align 8
  br label %172

110:                                              ; preds = %96
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %112
  %114 = load i8, i8* %113, align 1
  %115 = sext i8 %114 to i32
  %116 = icmp eq i32 %115, 74
  br i1 %116, label %117, label %124

117:                                              ; preds = %110
  %118 = va_arg i8** %7, i64
  store i64 %118, i64* %17, align 8
  %119 = load i64, i64* %17, align 8
  %120 = load i32, i32* %11, align 4
  %121 = sext i32 %120 to i64
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %119, i64* %123, align 8
  br label %171

124:                                              ; preds = %110
  %125 = load i32, i32* %11, align 4
  %126 = sext i32 %125 to i64
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %126
  %128 = load i8, i8* %127, align 1
  %129 = sext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %139

131:                                              ; preds = %124
  %132 = va_arg i8** %7, double
  store double %132, double* %18, align 8
  %133 = load double, double* %18, align 8
  %134 = fptrunc double %133 to float
  %135 = load i32, i32* %11, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %136
  %138 = bitcast %union.jvalue* %137 to float*
  store float %134, float* %138, align 8
  br label %170

139:                                              ; preds = %124
  %140 = load i32, i32* %11, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %141
  %143 = load i8, i8* %142, align 1
  %144 = sext i8 %143 to i32
  %145 = icmp eq i32 %144, 68
  br i1 %145, label %146, label %153

146:                                              ; preds = %139
  %147 = va_arg i8** %7, double
  store double %147, double* %19, align 8
  %148 = load double, double* %19, align 8
  %149 = load i32, i32* %11, align 4
  %150 = sext i32 %149 to i64
  %151 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %150
  %152 = bitcast %union.jvalue* %151 to double*
  store double %148, double* %152, align 8
  br label %169

153:                                              ; preds = %139
  %154 = load i32, i32* %11, align 4
  %155 = sext i32 %154 to i64
  %156 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %155
  %157 = load i8, i8* %156, align 1
  %158 = sext i8 %157 to i32
  %159 = icmp eq i32 %158, 76
  br i1 %159, label %160, label %168

160:                                              ; preds = %153
  %161 = va_arg i8** %7, i8*
  store i8* %161, i8** %20, align 8
  %162 = load i8*, i8** %20, align 8
  %163 = bitcast i8* %162 to %struct._jobject*
  %164 = load i32, i32* %11, align 4
  %165 = sext i32 %164 to i64
  %166 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %165
  %167 = bitcast %union.jvalue* %166 to %struct._jobject**
  store %struct._jobject* %163, %struct._jobject** %167, align 8
  br label %168

168:                                              ; preds = %160, %153
  br label %169

169:                                              ; preds = %168, %146
  br label %170

170:                                              ; preds = %169, %131
  br label %171

171:                                              ; preds = %170, %117
  br label %172

172:                                              ; preds = %171, %103
  br label %173

173:                                              ; preds = %172, %88
  br label %174

174:                                              ; preds = %173, %73
  br label %175

175:                                              ; preds = %174, %58
  br label %176

176:                                              ; preds = %175, %43
  br label %177

177:                                              ; preds = %176
  %178 = load i32, i32* %11, align 4
  %179 = add nsw i32 %178, 1
  store i32 %179, i32* %11, align 4
  br label %32, !llvm.loop !28

180:                                              ; preds = %32
  %181 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %182 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %181, align 8
  %183 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %182, i32 0, i32 45
  %184 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %183, align 8
  %185 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %186 = load %struct._jobject*, %struct._jobject** %5, align 8
  %187 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %188 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  %189 = call zeroext i16 %184(%struct.JNINativeInterface_** noundef %185, %struct._jobject* noundef %186, %struct._jmethodID* noundef %187, %union.jvalue* noundef %188)
  store i16 %189, i16* %21, align 2
  %190 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %190)
  %191 = load i16, i16* %21, align 2
  ret i16 %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define zeroext i16 @JNI_CallCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !29

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 45
  %183 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  %188 = call zeroext i16 %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret i16 %188
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define zeroext i16 @JNI_CallNonvirtualCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i8*, align 8
  %10 = alloca [256 x i8], align 1
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i32, align 4
  %19 = alloca i64, align 8
  %20 = alloca double, align 8
  %21 = alloca double, align 8
  %22 = alloca i8*, align 8
  %23 = alloca i16, align 2
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %24 = bitcast i8** %9 to i8*
  call void @llvm.va_start(i8* %24)
  %25 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %26 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %25, align 8
  %27 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %26, i32 0, i32 0
  %28 = load i8*, i8** %27, align 8
  %29 = bitcast i8* %28 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %30 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %31 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 0
  %33 = call i32 %29(%struct.JNINativeInterface_** noundef %30, %struct._jmethodID* noundef %31, i8* noundef %32)
  store i32 %33, i32* %12, align 4
  store i32 0, i32* %13, align 4
  br label %34

34:                                               ; preds = %179, %4
  %35 = load i32, i32* %13, align 4
  %36 = load i32, i32* %12, align 4
  %37 = icmp slt i32 %35, %36
  br i1 %37, label %38, label %182

38:                                               ; preds = %34
  %39 = load i32, i32* %13, align 4
  %40 = sext i32 %39 to i64
  %41 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %40
  %42 = load i8, i8* %41, align 1
  %43 = sext i8 %42 to i32
  %44 = icmp eq i32 %43, 90
  br i1 %44, label %45, label %53

45:                                               ; preds = %38
  %46 = va_arg i8** %9, i32
  store i32 %46, i32* %14, align 4
  %47 = load i32, i32* %14, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %13, align 4
  %50 = sext i32 %49 to i64
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %48, i8* %52, align 8
  br label %178

53:                                               ; preds = %38
  %54 = load i32, i32* %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %55
  %57 = load i8, i8* %56, align 1
  %58 = sext i8 %57 to i32
  %59 = icmp eq i32 %58, 66
  br i1 %59, label %60, label %68

60:                                               ; preds = %53
  %61 = va_arg i8** %9, i32
  store i32 %61, i32* %15, align 4
  %62 = load i32, i32* %15, align 4
  %63 = trunc i32 %62 to i8
  %64 = load i32, i32* %13, align 4
  %65 = sext i32 %64 to i64
  %66 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %65
  %67 = bitcast %union.jvalue* %66 to i8*
  store i8 %63, i8* %67, align 8
  br label %177

68:                                               ; preds = %53
  %69 = load i32, i32* %13, align 4
  %70 = sext i32 %69 to i64
  %71 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %70
  %72 = load i8, i8* %71, align 1
  %73 = sext i8 %72 to i32
  %74 = icmp eq i32 %73, 67
  br i1 %74, label %75, label %83

75:                                               ; preds = %68
  %76 = va_arg i8** %9, i32
  store i32 %76, i32* %16, align 4
  %77 = load i32, i32* %16, align 4
  %78 = trunc i32 %77 to i16
  %79 = load i32, i32* %13, align 4
  %80 = sext i32 %79 to i64
  %81 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %80
  %82 = bitcast %union.jvalue* %81 to i16*
  store i16 %78, i16* %82, align 8
  br label %176

83:                                               ; preds = %68
  %84 = load i32, i32* %13, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %85
  %87 = load i8, i8* %86, align 1
  %88 = sext i8 %87 to i32
  %89 = icmp eq i32 %88, 83
  br i1 %89, label %90, label %98

90:                                               ; preds = %83
  %91 = va_arg i8** %9, i32
  store i32 %91, i32* %17, align 4
  %92 = load i32, i32* %17, align 4
  %93 = trunc i32 %92 to i16
  %94 = load i32, i32* %13, align 4
  %95 = sext i32 %94 to i64
  %96 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %95
  %97 = bitcast %union.jvalue* %96 to i16*
  store i16 %93, i16* %97, align 8
  br label %175

98:                                               ; preds = %83
  %99 = load i32, i32* %13, align 4
  %100 = sext i32 %99 to i64
  %101 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %100
  %102 = load i8, i8* %101, align 1
  %103 = sext i8 %102 to i32
  %104 = icmp eq i32 %103, 73
  br i1 %104, label %105, label %112

105:                                              ; preds = %98
  %106 = va_arg i8** %9, i32
  store i32 %106, i32* %18, align 4
  %107 = load i32, i32* %18, align 4
  %108 = load i32, i32* %13, align 4
  %109 = sext i32 %108 to i64
  %110 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %109
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %107, i32* %111, align 8
  br label %174

112:                                              ; preds = %98
  %113 = load i32, i32* %13, align 4
  %114 = sext i32 %113 to i64
  %115 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %114
  %116 = load i8, i8* %115, align 1
  %117 = sext i8 %116 to i32
  %118 = icmp eq i32 %117, 74
  br i1 %118, label %119, label %126

119:                                              ; preds = %112
  %120 = va_arg i8** %9, i64
  store i64 %120, i64* %19, align 8
  %121 = load i64, i64* %19, align 8
  %122 = load i32, i32* %13, align 4
  %123 = sext i32 %122 to i64
  %124 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %123
  %125 = bitcast %union.jvalue* %124 to i64*
  store i64 %121, i64* %125, align 8
  br label %173

126:                                              ; preds = %112
  %127 = load i32, i32* %13, align 4
  %128 = sext i32 %127 to i64
  %129 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %128
  %130 = load i8, i8* %129, align 1
  %131 = sext i8 %130 to i32
  %132 = icmp eq i32 %131, 70
  br i1 %132, label %133, label %141

133:                                              ; preds = %126
  %134 = va_arg i8** %9, double
  store double %134, double* %20, align 8
  %135 = load double, double* %20, align 8
  %136 = fptrunc double %135 to float
  %137 = load i32, i32* %13, align 4
  %138 = sext i32 %137 to i64
  %139 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %138
  %140 = bitcast %union.jvalue* %139 to float*
  store float %136, float* %140, align 8
  br label %172

141:                                              ; preds = %126
  %142 = load i32, i32* %13, align 4
  %143 = sext i32 %142 to i64
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %143
  %145 = load i8, i8* %144, align 1
  %146 = sext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %155

148:                                              ; preds = %141
  %149 = va_arg i8** %9, double
  store double %149, double* %21, align 8
  %150 = load double, double* %21, align 8
  %151 = load i32, i32* %13, align 4
  %152 = sext i32 %151 to i64
  %153 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %152
  %154 = bitcast %union.jvalue* %153 to double*
  store double %150, double* %154, align 8
  br label %171

155:                                              ; preds = %141
  %156 = load i32, i32* %13, align 4
  %157 = sext i32 %156 to i64
  %158 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %157
  %159 = load i8, i8* %158, align 1
  %160 = sext i8 %159 to i32
  %161 = icmp eq i32 %160, 76
  br i1 %161, label %162, label %170

162:                                              ; preds = %155
  %163 = va_arg i8** %9, i8*
  store i8* %163, i8** %22, align 8
  %164 = load i8*, i8** %22, align 8
  %165 = bitcast i8* %164 to %struct._jobject*
  %166 = load i32, i32* %13, align 4
  %167 = sext i32 %166 to i64
  %168 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %167
  %169 = bitcast %union.jvalue* %168 to %struct._jobject**
  store %struct._jobject* %165, %struct._jobject** %169, align 8
  br label %170

170:                                              ; preds = %162, %155
  br label %171

171:                                              ; preds = %170, %148
  br label %172

172:                                              ; preds = %171, %133
  br label %173

173:                                              ; preds = %172, %119
  br label %174

174:                                              ; preds = %173, %105
  br label %175

175:                                              ; preds = %174, %90
  br label %176

176:                                              ; preds = %175, %75
  br label %177

177:                                              ; preds = %176, %60
  br label %178

178:                                              ; preds = %177, %45
  br label %179

179:                                              ; preds = %178
  %180 = load i32, i32* %13, align 4
  %181 = add nsw i32 %180, 1
  store i32 %181, i32* %13, align 4
  br label %34, !llvm.loop !30

182:                                              ; preds = %34
  %183 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %184 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %183, align 8
  %185 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %184, i32 0, i32 75
  %186 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %185, align 8
  %187 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %188 = load %struct._jobject*, %struct._jobject** %6, align 8
  %189 = load %struct._jobject*, %struct._jobject** %7, align 8
  %190 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %191 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 0
  %192 = call zeroext i16 %186(%struct.JNINativeInterface_** noundef %187, %struct._jobject* noundef %188, %struct._jobject* noundef %189, %struct._jmethodID* noundef %190, %union.jvalue* noundef %191)
  store i16 %192, i16* %23, align 2
  %193 = bitcast i8** %9 to i8*
  call void @llvm.va_end(i8* %193)
  %194 = load i16, i16* %23, align 2
  ret i16 %194
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define zeroext i16 @JNI_CallNonvirtualCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, i8* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca i8*, align 8
  %11 = alloca [256 x i8], align 1
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i32, align 4
  %19 = alloca i32, align 4
  %20 = alloca i64, align 8
  %21 = alloca double, align 8
  %22 = alloca double, align 8
  %23 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  store i8* %4, i8** %10, align 8
  %24 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %25 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %25, i32 0, i32 0
  %27 = load i8*, i8** %26, align 8
  %28 = bitcast i8* %27 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %29 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %30 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 0
  %32 = call i32 %28(%struct.JNINativeInterface_** noundef %29, %struct._jmethodID* noundef %30, i8* noundef %31)
  store i32 %32, i32* %13, align 4
  store i32 0, i32* %14, align 4
  br label %33

33:                                               ; preds = %178, %5
  %34 = load i32, i32* %14, align 4
  %35 = load i32, i32* %13, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %181

37:                                               ; preds = %33
  %38 = load i32, i32* %14, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %39
  %41 = load i8, i8* %40, align 1
  %42 = sext i8 %41 to i32
  %43 = icmp eq i32 %42, 90
  br i1 %43, label %44, label %52

44:                                               ; preds = %37
  %45 = va_arg i8** %10, i32
  store i32 %45, i32* %15, align 4
  %46 = load i32, i32* %15, align 4
  %47 = trunc i32 %46 to i8
  %48 = load i32, i32* %14, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %47, i8* %51, align 8
  br label %177

52:                                               ; preds = %37
  %53 = load i32, i32* %14, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %54
  %56 = load i8, i8* %55, align 1
  %57 = sext i8 %56 to i32
  %58 = icmp eq i32 %57, 66
  br i1 %58, label %59, label %67

59:                                               ; preds = %52
  %60 = va_arg i8** %10, i32
  store i32 %60, i32* %16, align 4
  %61 = load i32, i32* %16, align 4
  %62 = trunc i32 %61 to i8
  %63 = load i32, i32* %14, align 4
  %64 = sext i32 %63 to i64
  %65 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %64
  %66 = bitcast %union.jvalue* %65 to i8*
  store i8 %62, i8* %66, align 8
  br label %176

67:                                               ; preds = %52
  %68 = load i32, i32* %14, align 4
  %69 = sext i32 %68 to i64
  %70 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %69
  %71 = load i8, i8* %70, align 1
  %72 = sext i8 %71 to i32
  %73 = icmp eq i32 %72, 67
  br i1 %73, label %74, label %82

74:                                               ; preds = %67
  %75 = va_arg i8** %10, i32
  store i32 %75, i32* %17, align 4
  %76 = load i32, i32* %17, align 4
  %77 = trunc i32 %76 to i16
  %78 = load i32, i32* %14, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %175

82:                                               ; preds = %67
  %83 = load i32, i32* %14, align 4
  %84 = sext i32 %83 to i64
  %85 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %84
  %86 = load i8, i8* %85, align 1
  %87 = sext i8 %86 to i32
  %88 = icmp eq i32 %87, 83
  br i1 %88, label %89, label %97

89:                                               ; preds = %82
  %90 = va_arg i8** %10, i32
  store i32 %90, i32* %18, align 4
  %91 = load i32, i32* %18, align 4
  %92 = trunc i32 %91 to i16
  %93 = load i32, i32* %14, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %94
  %96 = bitcast %union.jvalue* %95 to i16*
  store i16 %92, i16* %96, align 8
  br label %174

97:                                               ; preds = %82
  %98 = load i32, i32* %14, align 4
  %99 = sext i32 %98 to i64
  %100 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %99
  %101 = load i8, i8* %100, align 1
  %102 = sext i8 %101 to i32
  %103 = icmp eq i32 %102, 73
  br i1 %103, label %104, label %111

104:                                              ; preds = %97
  %105 = va_arg i8** %10, i32
  store i32 %105, i32* %19, align 4
  %106 = load i32, i32* %19, align 4
  %107 = load i32, i32* %14, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %108
  %110 = bitcast %union.jvalue* %109 to i32*
  store i32 %106, i32* %110, align 8
  br label %173

111:                                              ; preds = %97
  %112 = load i32, i32* %14, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %113
  %115 = load i8, i8* %114, align 1
  %116 = sext i8 %115 to i32
  %117 = icmp eq i32 %116, 74
  br i1 %117, label %118, label %125

118:                                              ; preds = %111
  %119 = va_arg i8** %10, i64
  store i64 %119, i64* %20, align 8
  %120 = load i64, i64* %20, align 8
  %121 = load i32, i32* %14, align 4
  %122 = sext i32 %121 to i64
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %120, i64* %124, align 8
  br label %172

125:                                              ; preds = %111
  %126 = load i32, i32* %14, align 4
  %127 = sext i32 %126 to i64
  %128 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %127
  %129 = load i8, i8* %128, align 1
  %130 = sext i8 %129 to i32
  %131 = icmp eq i32 %130, 70
  br i1 %131, label %132, label %140

132:                                              ; preds = %125
  %133 = va_arg i8** %10, double
  store double %133, double* %21, align 8
  %134 = load double, double* %21, align 8
  %135 = fptrunc double %134 to float
  %136 = load i32, i32* %14, align 4
  %137 = sext i32 %136 to i64
  %138 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %137
  %139 = bitcast %union.jvalue* %138 to float*
  store float %135, float* %139, align 8
  br label %171

140:                                              ; preds = %125
  %141 = load i32, i32* %14, align 4
  %142 = sext i32 %141 to i64
  %143 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %142
  %144 = load i8, i8* %143, align 1
  %145 = sext i8 %144 to i32
  %146 = icmp eq i32 %145, 68
  br i1 %146, label %147, label %154

147:                                              ; preds = %140
  %148 = va_arg i8** %10, double
  store double %148, double* %22, align 8
  %149 = load double, double* %22, align 8
  %150 = load i32, i32* %14, align 4
  %151 = sext i32 %150 to i64
  %152 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %151
  %153 = bitcast %union.jvalue* %152 to double*
  store double %149, double* %153, align 8
  br label %170

154:                                              ; preds = %140
  %155 = load i32, i32* %14, align 4
  %156 = sext i32 %155 to i64
  %157 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %156
  %158 = load i8, i8* %157, align 1
  %159 = sext i8 %158 to i32
  %160 = icmp eq i32 %159, 76
  br i1 %160, label %161, label %169

161:                                              ; preds = %154
  %162 = va_arg i8** %10, i8*
  store i8* %162, i8** %23, align 8
  %163 = load i8*, i8** %23, align 8
  %164 = bitcast i8* %163 to %struct._jobject*
  %165 = load i32, i32* %14, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %166
  %168 = bitcast %union.jvalue* %167 to %struct._jobject**
  store %struct._jobject* %164, %struct._jobject** %168, align 8
  br label %169

169:                                              ; preds = %161, %154
  br label %170

170:                                              ; preds = %169, %147
  br label %171

171:                                              ; preds = %170, %132
  br label %172

172:                                              ; preds = %171, %118
  br label %173

173:                                              ; preds = %172, %104
  br label %174

174:                                              ; preds = %173, %89
  br label %175

175:                                              ; preds = %174, %74
  br label %176

176:                                              ; preds = %175, %59
  br label %177

177:                                              ; preds = %176, %44
  br label %178

178:                                              ; preds = %177
  %179 = load i32, i32* %14, align 4
  %180 = add nsw i32 %179, 1
  store i32 %180, i32* %14, align 4
  br label %33, !llvm.loop !31

181:                                              ; preds = %33
  %182 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %183 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %182, align 8
  %184 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %183, i32 0, i32 75
  %185 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %184, align 8
  %186 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %187 = load %struct._jobject*, %struct._jobject** %7, align 8
  %188 = load %struct._jobject*, %struct._jobject** %8, align 8
  %189 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %190 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 0
  %191 = call zeroext i16 %185(%struct.JNINativeInterface_** noundef %186, %struct._jobject* noundef %187, %struct._jobject* noundef %188, %struct._jmethodID* noundef %189, %union.jvalue* noundef %190)
  ret i16 %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define zeroext i16 @JNI_CallStaticCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  %21 = alloca i16, align 2
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %22 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %22)
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %24 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %23, align 8
  %25 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %24, i32 0, i32 0
  %26 = load i8*, i8** %25, align 8
  %27 = bitcast i8* %26 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %28 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %29 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %31 = call i32 %27(%struct.JNINativeInterface_** noundef %28, %struct._jmethodID* noundef %29, i8* noundef %30)
  store i32 %31, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %32

32:                                               ; preds = %177, %3
  %33 = load i32, i32* %11, align 4
  %34 = load i32, i32* %10, align 4
  %35 = icmp slt i32 %33, %34
  br i1 %35, label %36, label %180

36:                                               ; preds = %32
  %37 = load i32, i32* %11, align 4
  %38 = sext i32 %37 to i64
  %39 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %38
  %40 = load i8, i8* %39, align 1
  %41 = sext i8 %40 to i32
  %42 = icmp eq i32 %41, 90
  br i1 %42, label %43, label %51

43:                                               ; preds = %36
  %44 = va_arg i8** %7, i32
  store i32 %44, i32* %12, align 4
  %45 = load i32, i32* %12, align 4
  %46 = trunc i32 %45 to i8
  %47 = load i32, i32* %11, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %176

51:                                               ; preds = %36
  %52 = load i32, i32* %11, align 4
  %53 = sext i32 %52 to i64
  %54 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %53
  %55 = load i8, i8* %54, align 1
  %56 = sext i8 %55 to i32
  %57 = icmp eq i32 %56, 66
  br i1 %57, label %58, label %66

58:                                               ; preds = %51
  %59 = va_arg i8** %7, i32
  store i32 %59, i32* %13, align 4
  %60 = load i32, i32* %13, align 4
  %61 = trunc i32 %60 to i8
  %62 = load i32, i32* %11, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %63
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %61, i8* %65, align 8
  br label %175

66:                                               ; preds = %51
  %67 = load i32, i32* %11, align 4
  %68 = sext i32 %67 to i64
  %69 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %68
  %70 = load i8, i8* %69, align 1
  %71 = sext i8 %70 to i32
  %72 = icmp eq i32 %71, 67
  br i1 %72, label %73, label %81

73:                                               ; preds = %66
  %74 = va_arg i8** %7, i32
  store i32 %74, i32* %14, align 4
  %75 = load i32, i32* %14, align 4
  %76 = trunc i32 %75 to i16
  %77 = load i32, i32* %11, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %174

81:                                               ; preds = %66
  %82 = load i32, i32* %11, align 4
  %83 = sext i32 %82 to i64
  %84 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %83
  %85 = load i8, i8* %84, align 1
  %86 = sext i8 %85 to i32
  %87 = icmp eq i32 %86, 83
  br i1 %87, label %88, label %96

88:                                               ; preds = %81
  %89 = va_arg i8** %7, i32
  store i32 %89, i32* %15, align 4
  %90 = load i32, i32* %15, align 4
  %91 = trunc i32 %90 to i16
  %92 = load i32, i32* %11, align 4
  %93 = sext i32 %92 to i64
  %94 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %93
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %91, i16* %95, align 8
  br label %173

96:                                               ; preds = %81
  %97 = load i32, i32* %11, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %98
  %100 = load i8, i8* %99, align 1
  %101 = sext i8 %100 to i32
  %102 = icmp eq i32 %101, 73
  br i1 %102, label %103, label %110

103:                                              ; preds = %96
  %104 = va_arg i8** %7, i32
  store i32 %104, i32* %16, align 4
  %105 = load i32, i32* %16, align 4
  %106 = load i32, i32* %11, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %107
  %109 = bitcast %union.jvalue* %108 to i32*
  store i32 %105, i32* %109, align 8
  br label %172

110:                                              ; preds = %96
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %112
  %114 = load i8, i8* %113, align 1
  %115 = sext i8 %114 to i32
  %116 = icmp eq i32 %115, 74
  br i1 %116, label %117, label %124

117:                                              ; preds = %110
  %118 = va_arg i8** %7, i64
  store i64 %118, i64* %17, align 8
  %119 = load i64, i64* %17, align 8
  %120 = load i32, i32* %11, align 4
  %121 = sext i32 %120 to i64
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %119, i64* %123, align 8
  br label %171

124:                                              ; preds = %110
  %125 = load i32, i32* %11, align 4
  %126 = sext i32 %125 to i64
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %126
  %128 = load i8, i8* %127, align 1
  %129 = sext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %139

131:                                              ; preds = %124
  %132 = va_arg i8** %7, double
  store double %132, double* %18, align 8
  %133 = load double, double* %18, align 8
  %134 = fptrunc double %133 to float
  %135 = load i32, i32* %11, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %136
  %138 = bitcast %union.jvalue* %137 to float*
  store float %134, float* %138, align 8
  br label %170

139:                                              ; preds = %124
  %140 = load i32, i32* %11, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %141
  %143 = load i8, i8* %142, align 1
  %144 = sext i8 %143 to i32
  %145 = icmp eq i32 %144, 68
  br i1 %145, label %146, label %153

146:                                              ; preds = %139
  %147 = va_arg i8** %7, double
  store double %147, double* %19, align 8
  %148 = load double, double* %19, align 8
  %149 = load i32, i32* %11, align 4
  %150 = sext i32 %149 to i64
  %151 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %150
  %152 = bitcast %union.jvalue* %151 to double*
  store double %148, double* %152, align 8
  br label %169

153:                                              ; preds = %139
  %154 = load i32, i32* %11, align 4
  %155 = sext i32 %154 to i64
  %156 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %155
  %157 = load i8, i8* %156, align 1
  %158 = sext i8 %157 to i32
  %159 = icmp eq i32 %158, 76
  br i1 %159, label %160, label %168

160:                                              ; preds = %153
  %161 = va_arg i8** %7, i8*
  store i8* %161, i8** %20, align 8
  %162 = load i8*, i8** %20, align 8
  %163 = bitcast i8* %162 to %struct._jobject*
  %164 = load i32, i32* %11, align 4
  %165 = sext i32 %164 to i64
  %166 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %165
  %167 = bitcast %union.jvalue* %166 to %struct._jobject**
  store %struct._jobject* %163, %struct._jobject** %167, align 8
  br label %168

168:                                              ; preds = %160, %153
  br label %169

169:                                              ; preds = %168, %146
  br label %170

170:                                              ; preds = %169, %131
  br label %171

171:                                              ; preds = %170, %117
  br label %172

172:                                              ; preds = %171, %103
  br label %173

173:                                              ; preds = %172, %88
  br label %174

174:                                              ; preds = %173, %73
  br label %175

175:                                              ; preds = %174, %58
  br label %176

176:                                              ; preds = %175, %43
  br label %177

177:                                              ; preds = %176
  %178 = load i32, i32* %11, align 4
  %179 = add nsw i32 %178, 1
  store i32 %179, i32* %11, align 4
  br label %32, !llvm.loop !32

180:                                              ; preds = %32
  %181 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %182 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %181, align 8
  %183 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %182, i32 0, i32 125
  %184 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %183, align 8
  %185 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %186 = load %struct._jobject*, %struct._jobject** %5, align 8
  %187 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %188 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  %189 = call zeroext i16 %184(%struct.JNINativeInterface_** noundef %185, %struct._jobject* noundef %186, %struct._jmethodID* noundef %187, %union.jvalue* noundef %188)
  store i16 %189, i16* %21, align 2
  %190 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %190)
  %191 = load i16, i16* %21, align 2
  ret i16 %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define zeroext i16 @JNI_CallStaticCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !33

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 125
  %183 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  %188 = call zeroext i16 %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret i16 %188
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define signext i16 @JNI_CallShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  %21 = alloca i16, align 2
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %22 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %22)
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %24 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %23, align 8
  %25 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %24, i32 0, i32 0
  %26 = load i8*, i8** %25, align 8
  %27 = bitcast i8* %26 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %28 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %29 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %31 = call i32 %27(%struct.JNINativeInterface_** noundef %28, %struct._jmethodID* noundef %29, i8* noundef %30)
  store i32 %31, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %32

32:                                               ; preds = %177, %3
  %33 = load i32, i32* %11, align 4
  %34 = load i32, i32* %10, align 4
  %35 = icmp slt i32 %33, %34
  br i1 %35, label %36, label %180

36:                                               ; preds = %32
  %37 = load i32, i32* %11, align 4
  %38 = sext i32 %37 to i64
  %39 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %38
  %40 = load i8, i8* %39, align 1
  %41 = sext i8 %40 to i32
  %42 = icmp eq i32 %41, 90
  br i1 %42, label %43, label %51

43:                                               ; preds = %36
  %44 = va_arg i8** %7, i32
  store i32 %44, i32* %12, align 4
  %45 = load i32, i32* %12, align 4
  %46 = trunc i32 %45 to i8
  %47 = load i32, i32* %11, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %176

51:                                               ; preds = %36
  %52 = load i32, i32* %11, align 4
  %53 = sext i32 %52 to i64
  %54 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %53
  %55 = load i8, i8* %54, align 1
  %56 = sext i8 %55 to i32
  %57 = icmp eq i32 %56, 66
  br i1 %57, label %58, label %66

58:                                               ; preds = %51
  %59 = va_arg i8** %7, i32
  store i32 %59, i32* %13, align 4
  %60 = load i32, i32* %13, align 4
  %61 = trunc i32 %60 to i8
  %62 = load i32, i32* %11, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %63
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %61, i8* %65, align 8
  br label %175

66:                                               ; preds = %51
  %67 = load i32, i32* %11, align 4
  %68 = sext i32 %67 to i64
  %69 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %68
  %70 = load i8, i8* %69, align 1
  %71 = sext i8 %70 to i32
  %72 = icmp eq i32 %71, 67
  br i1 %72, label %73, label %81

73:                                               ; preds = %66
  %74 = va_arg i8** %7, i32
  store i32 %74, i32* %14, align 4
  %75 = load i32, i32* %14, align 4
  %76 = trunc i32 %75 to i16
  %77 = load i32, i32* %11, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %174

81:                                               ; preds = %66
  %82 = load i32, i32* %11, align 4
  %83 = sext i32 %82 to i64
  %84 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %83
  %85 = load i8, i8* %84, align 1
  %86 = sext i8 %85 to i32
  %87 = icmp eq i32 %86, 83
  br i1 %87, label %88, label %96

88:                                               ; preds = %81
  %89 = va_arg i8** %7, i32
  store i32 %89, i32* %15, align 4
  %90 = load i32, i32* %15, align 4
  %91 = trunc i32 %90 to i16
  %92 = load i32, i32* %11, align 4
  %93 = sext i32 %92 to i64
  %94 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %93
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %91, i16* %95, align 8
  br label %173

96:                                               ; preds = %81
  %97 = load i32, i32* %11, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %98
  %100 = load i8, i8* %99, align 1
  %101 = sext i8 %100 to i32
  %102 = icmp eq i32 %101, 73
  br i1 %102, label %103, label %110

103:                                              ; preds = %96
  %104 = va_arg i8** %7, i32
  store i32 %104, i32* %16, align 4
  %105 = load i32, i32* %16, align 4
  %106 = load i32, i32* %11, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %107
  %109 = bitcast %union.jvalue* %108 to i32*
  store i32 %105, i32* %109, align 8
  br label %172

110:                                              ; preds = %96
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %112
  %114 = load i8, i8* %113, align 1
  %115 = sext i8 %114 to i32
  %116 = icmp eq i32 %115, 74
  br i1 %116, label %117, label %124

117:                                              ; preds = %110
  %118 = va_arg i8** %7, i64
  store i64 %118, i64* %17, align 8
  %119 = load i64, i64* %17, align 8
  %120 = load i32, i32* %11, align 4
  %121 = sext i32 %120 to i64
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %119, i64* %123, align 8
  br label %171

124:                                              ; preds = %110
  %125 = load i32, i32* %11, align 4
  %126 = sext i32 %125 to i64
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %126
  %128 = load i8, i8* %127, align 1
  %129 = sext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %139

131:                                              ; preds = %124
  %132 = va_arg i8** %7, double
  store double %132, double* %18, align 8
  %133 = load double, double* %18, align 8
  %134 = fptrunc double %133 to float
  %135 = load i32, i32* %11, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %136
  %138 = bitcast %union.jvalue* %137 to float*
  store float %134, float* %138, align 8
  br label %170

139:                                              ; preds = %124
  %140 = load i32, i32* %11, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %141
  %143 = load i8, i8* %142, align 1
  %144 = sext i8 %143 to i32
  %145 = icmp eq i32 %144, 68
  br i1 %145, label %146, label %153

146:                                              ; preds = %139
  %147 = va_arg i8** %7, double
  store double %147, double* %19, align 8
  %148 = load double, double* %19, align 8
  %149 = load i32, i32* %11, align 4
  %150 = sext i32 %149 to i64
  %151 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %150
  %152 = bitcast %union.jvalue* %151 to double*
  store double %148, double* %152, align 8
  br label %169

153:                                              ; preds = %139
  %154 = load i32, i32* %11, align 4
  %155 = sext i32 %154 to i64
  %156 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %155
  %157 = load i8, i8* %156, align 1
  %158 = sext i8 %157 to i32
  %159 = icmp eq i32 %158, 76
  br i1 %159, label %160, label %168

160:                                              ; preds = %153
  %161 = va_arg i8** %7, i8*
  store i8* %161, i8** %20, align 8
  %162 = load i8*, i8** %20, align 8
  %163 = bitcast i8* %162 to %struct._jobject*
  %164 = load i32, i32* %11, align 4
  %165 = sext i32 %164 to i64
  %166 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %165
  %167 = bitcast %union.jvalue* %166 to %struct._jobject**
  store %struct._jobject* %163, %struct._jobject** %167, align 8
  br label %168

168:                                              ; preds = %160, %153
  br label %169

169:                                              ; preds = %168, %146
  br label %170

170:                                              ; preds = %169, %131
  br label %171

171:                                              ; preds = %170, %117
  br label %172

172:                                              ; preds = %171, %103
  br label %173

173:                                              ; preds = %172, %88
  br label %174

174:                                              ; preds = %173, %73
  br label %175

175:                                              ; preds = %174, %58
  br label %176

176:                                              ; preds = %175, %43
  br label %177

177:                                              ; preds = %176
  %178 = load i32, i32* %11, align 4
  %179 = add nsw i32 %178, 1
  store i32 %179, i32* %11, align 4
  br label %32, !llvm.loop !34

180:                                              ; preds = %32
  %181 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %182 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %181, align 8
  %183 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %182, i32 0, i32 48
  %184 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %183, align 8
  %185 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %186 = load %struct._jobject*, %struct._jobject** %5, align 8
  %187 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %188 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  %189 = call signext i16 %184(%struct.JNINativeInterface_** noundef %185, %struct._jobject* noundef %186, %struct._jmethodID* noundef %187, %union.jvalue* noundef %188)
  store i16 %189, i16* %21, align 2
  %190 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %190)
  %191 = load i16, i16* %21, align 2
  ret i16 %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define signext i16 @JNI_CallShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !35

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 48
  %183 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  %188 = call signext i16 %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret i16 %188
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define signext i16 @JNI_CallNonvirtualShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i8*, align 8
  %10 = alloca [256 x i8], align 1
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i32, align 4
  %19 = alloca i64, align 8
  %20 = alloca double, align 8
  %21 = alloca double, align 8
  %22 = alloca i8*, align 8
  %23 = alloca i16, align 2
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %24 = bitcast i8** %9 to i8*
  call void @llvm.va_start(i8* %24)
  %25 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %26 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %25, align 8
  %27 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %26, i32 0, i32 0
  %28 = load i8*, i8** %27, align 8
  %29 = bitcast i8* %28 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %30 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %31 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 0
  %33 = call i32 %29(%struct.JNINativeInterface_** noundef %30, %struct._jmethodID* noundef %31, i8* noundef %32)
  store i32 %33, i32* %12, align 4
  store i32 0, i32* %13, align 4
  br label %34

34:                                               ; preds = %179, %4
  %35 = load i32, i32* %13, align 4
  %36 = load i32, i32* %12, align 4
  %37 = icmp slt i32 %35, %36
  br i1 %37, label %38, label %182

38:                                               ; preds = %34
  %39 = load i32, i32* %13, align 4
  %40 = sext i32 %39 to i64
  %41 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %40
  %42 = load i8, i8* %41, align 1
  %43 = sext i8 %42 to i32
  %44 = icmp eq i32 %43, 90
  br i1 %44, label %45, label %53

45:                                               ; preds = %38
  %46 = va_arg i8** %9, i32
  store i32 %46, i32* %14, align 4
  %47 = load i32, i32* %14, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %13, align 4
  %50 = sext i32 %49 to i64
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %48, i8* %52, align 8
  br label %178

53:                                               ; preds = %38
  %54 = load i32, i32* %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %55
  %57 = load i8, i8* %56, align 1
  %58 = sext i8 %57 to i32
  %59 = icmp eq i32 %58, 66
  br i1 %59, label %60, label %68

60:                                               ; preds = %53
  %61 = va_arg i8** %9, i32
  store i32 %61, i32* %15, align 4
  %62 = load i32, i32* %15, align 4
  %63 = trunc i32 %62 to i8
  %64 = load i32, i32* %13, align 4
  %65 = sext i32 %64 to i64
  %66 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %65
  %67 = bitcast %union.jvalue* %66 to i8*
  store i8 %63, i8* %67, align 8
  br label %177

68:                                               ; preds = %53
  %69 = load i32, i32* %13, align 4
  %70 = sext i32 %69 to i64
  %71 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %70
  %72 = load i8, i8* %71, align 1
  %73 = sext i8 %72 to i32
  %74 = icmp eq i32 %73, 67
  br i1 %74, label %75, label %83

75:                                               ; preds = %68
  %76 = va_arg i8** %9, i32
  store i32 %76, i32* %16, align 4
  %77 = load i32, i32* %16, align 4
  %78 = trunc i32 %77 to i16
  %79 = load i32, i32* %13, align 4
  %80 = sext i32 %79 to i64
  %81 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %80
  %82 = bitcast %union.jvalue* %81 to i16*
  store i16 %78, i16* %82, align 8
  br label %176

83:                                               ; preds = %68
  %84 = load i32, i32* %13, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %85
  %87 = load i8, i8* %86, align 1
  %88 = sext i8 %87 to i32
  %89 = icmp eq i32 %88, 83
  br i1 %89, label %90, label %98

90:                                               ; preds = %83
  %91 = va_arg i8** %9, i32
  store i32 %91, i32* %17, align 4
  %92 = load i32, i32* %17, align 4
  %93 = trunc i32 %92 to i16
  %94 = load i32, i32* %13, align 4
  %95 = sext i32 %94 to i64
  %96 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %95
  %97 = bitcast %union.jvalue* %96 to i16*
  store i16 %93, i16* %97, align 8
  br label %175

98:                                               ; preds = %83
  %99 = load i32, i32* %13, align 4
  %100 = sext i32 %99 to i64
  %101 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %100
  %102 = load i8, i8* %101, align 1
  %103 = sext i8 %102 to i32
  %104 = icmp eq i32 %103, 73
  br i1 %104, label %105, label %112

105:                                              ; preds = %98
  %106 = va_arg i8** %9, i32
  store i32 %106, i32* %18, align 4
  %107 = load i32, i32* %18, align 4
  %108 = load i32, i32* %13, align 4
  %109 = sext i32 %108 to i64
  %110 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %109
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %107, i32* %111, align 8
  br label %174

112:                                              ; preds = %98
  %113 = load i32, i32* %13, align 4
  %114 = sext i32 %113 to i64
  %115 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %114
  %116 = load i8, i8* %115, align 1
  %117 = sext i8 %116 to i32
  %118 = icmp eq i32 %117, 74
  br i1 %118, label %119, label %126

119:                                              ; preds = %112
  %120 = va_arg i8** %9, i64
  store i64 %120, i64* %19, align 8
  %121 = load i64, i64* %19, align 8
  %122 = load i32, i32* %13, align 4
  %123 = sext i32 %122 to i64
  %124 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %123
  %125 = bitcast %union.jvalue* %124 to i64*
  store i64 %121, i64* %125, align 8
  br label %173

126:                                              ; preds = %112
  %127 = load i32, i32* %13, align 4
  %128 = sext i32 %127 to i64
  %129 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %128
  %130 = load i8, i8* %129, align 1
  %131 = sext i8 %130 to i32
  %132 = icmp eq i32 %131, 70
  br i1 %132, label %133, label %141

133:                                              ; preds = %126
  %134 = va_arg i8** %9, double
  store double %134, double* %20, align 8
  %135 = load double, double* %20, align 8
  %136 = fptrunc double %135 to float
  %137 = load i32, i32* %13, align 4
  %138 = sext i32 %137 to i64
  %139 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %138
  %140 = bitcast %union.jvalue* %139 to float*
  store float %136, float* %140, align 8
  br label %172

141:                                              ; preds = %126
  %142 = load i32, i32* %13, align 4
  %143 = sext i32 %142 to i64
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %143
  %145 = load i8, i8* %144, align 1
  %146 = sext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %155

148:                                              ; preds = %141
  %149 = va_arg i8** %9, double
  store double %149, double* %21, align 8
  %150 = load double, double* %21, align 8
  %151 = load i32, i32* %13, align 4
  %152 = sext i32 %151 to i64
  %153 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %152
  %154 = bitcast %union.jvalue* %153 to double*
  store double %150, double* %154, align 8
  br label %171

155:                                              ; preds = %141
  %156 = load i32, i32* %13, align 4
  %157 = sext i32 %156 to i64
  %158 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %157
  %159 = load i8, i8* %158, align 1
  %160 = sext i8 %159 to i32
  %161 = icmp eq i32 %160, 76
  br i1 %161, label %162, label %170

162:                                              ; preds = %155
  %163 = va_arg i8** %9, i8*
  store i8* %163, i8** %22, align 8
  %164 = load i8*, i8** %22, align 8
  %165 = bitcast i8* %164 to %struct._jobject*
  %166 = load i32, i32* %13, align 4
  %167 = sext i32 %166 to i64
  %168 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %167
  %169 = bitcast %union.jvalue* %168 to %struct._jobject**
  store %struct._jobject* %165, %struct._jobject** %169, align 8
  br label %170

170:                                              ; preds = %162, %155
  br label %171

171:                                              ; preds = %170, %148
  br label %172

172:                                              ; preds = %171, %133
  br label %173

173:                                              ; preds = %172, %119
  br label %174

174:                                              ; preds = %173, %105
  br label %175

175:                                              ; preds = %174, %90
  br label %176

176:                                              ; preds = %175, %75
  br label %177

177:                                              ; preds = %176, %60
  br label %178

178:                                              ; preds = %177, %45
  br label %179

179:                                              ; preds = %178
  %180 = load i32, i32* %13, align 4
  %181 = add nsw i32 %180, 1
  store i32 %181, i32* %13, align 4
  br label %34, !llvm.loop !36

182:                                              ; preds = %34
  %183 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %184 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %183, align 8
  %185 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %184, i32 0, i32 78
  %186 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %185, align 8
  %187 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %188 = load %struct._jobject*, %struct._jobject** %6, align 8
  %189 = load %struct._jobject*, %struct._jobject** %7, align 8
  %190 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %191 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 0
  %192 = call signext i16 %186(%struct.JNINativeInterface_** noundef %187, %struct._jobject* noundef %188, %struct._jobject* noundef %189, %struct._jmethodID* noundef %190, %union.jvalue* noundef %191)
  store i16 %192, i16* %23, align 2
  %193 = bitcast i8** %9 to i8*
  call void @llvm.va_end(i8* %193)
  %194 = load i16, i16* %23, align 2
  ret i16 %194
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define signext i16 @JNI_CallNonvirtualShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, i8* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca i8*, align 8
  %11 = alloca [256 x i8], align 1
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i32, align 4
  %19 = alloca i32, align 4
  %20 = alloca i64, align 8
  %21 = alloca double, align 8
  %22 = alloca double, align 8
  %23 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  store i8* %4, i8** %10, align 8
  %24 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %25 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %25, i32 0, i32 0
  %27 = load i8*, i8** %26, align 8
  %28 = bitcast i8* %27 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %29 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %30 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 0
  %32 = call i32 %28(%struct.JNINativeInterface_** noundef %29, %struct._jmethodID* noundef %30, i8* noundef %31)
  store i32 %32, i32* %13, align 4
  store i32 0, i32* %14, align 4
  br label %33

33:                                               ; preds = %178, %5
  %34 = load i32, i32* %14, align 4
  %35 = load i32, i32* %13, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %181

37:                                               ; preds = %33
  %38 = load i32, i32* %14, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %39
  %41 = load i8, i8* %40, align 1
  %42 = sext i8 %41 to i32
  %43 = icmp eq i32 %42, 90
  br i1 %43, label %44, label %52

44:                                               ; preds = %37
  %45 = va_arg i8** %10, i32
  store i32 %45, i32* %15, align 4
  %46 = load i32, i32* %15, align 4
  %47 = trunc i32 %46 to i8
  %48 = load i32, i32* %14, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %47, i8* %51, align 8
  br label %177

52:                                               ; preds = %37
  %53 = load i32, i32* %14, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %54
  %56 = load i8, i8* %55, align 1
  %57 = sext i8 %56 to i32
  %58 = icmp eq i32 %57, 66
  br i1 %58, label %59, label %67

59:                                               ; preds = %52
  %60 = va_arg i8** %10, i32
  store i32 %60, i32* %16, align 4
  %61 = load i32, i32* %16, align 4
  %62 = trunc i32 %61 to i8
  %63 = load i32, i32* %14, align 4
  %64 = sext i32 %63 to i64
  %65 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %64
  %66 = bitcast %union.jvalue* %65 to i8*
  store i8 %62, i8* %66, align 8
  br label %176

67:                                               ; preds = %52
  %68 = load i32, i32* %14, align 4
  %69 = sext i32 %68 to i64
  %70 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %69
  %71 = load i8, i8* %70, align 1
  %72 = sext i8 %71 to i32
  %73 = icmp eq i32 %72, 67
  br i1 %73, label %74, label %82

74:                                               ; preds = %67
  %75 = va_arg i8** %10, i32
  store i32 %75, i32* %17, align 4
  %76 = load i32, i32* %17, align 4
  %77 = trunc i32 %76 to i16
  %78 = load i32, i32* %14, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %175

82:                                               ; preds = %67
  %83 = load i32, i32* %14, align 4
  %84 = sext i32 %83 to i64
  %85 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %84
  %86 = load i8, i8* %85, align 1
  %87 = sext i8 %86 to i32
  %88 = icmp eq i32 %87, 83
  br i1 %88, label %89, label %97

89:                                               ; preds = %82
  %90 = va_arg i8** %10, i32
  store i32 %90, i32* %18, align 4
  %91 = load i32, i32* %18, align 4
  %92 = trunc i32 %91 to i16
  %93 = load i32, i32* %14, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %94
  %96 = bitcast %union.jvalue* %95 to i16*
  store i16 %92, i16* %96, align 8
  br label %174

97:                                               ; preds = %82
  %98 = load i32, i32* %14, align 4
  %99 = sext i32 %98 to i64
  %100 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %99
  %101 = load i8, i8* %100, align 1
  %102 = sext i8 %101 to i32
  %103 = icmp eq i32 %102, 73
  br i1 %103, label %104, label %111

104:                                              ; preds = %97
  %105 = va_arg i8** %10, i32
  store i32 %105, i32* %19, align 4
  %106 = load i32, i32* %19, align 4
  %107 = load i32, i32* %14, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %108
  %110 = bitcast %union.jvalue* %109 to i32*
  store i32 %106, i32* %110, align 8
  br label %173

111:                                              ; preds = %97
  %112 = load i32, i32* %14, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %113
  %115 = load i8, i8* %114, align 1
  %116 = sext i8 %115 to i32
  %117 = icmp eq i32 %116, 74
  br i1 %117, label %118, label %125

118:                                              ; preds = %111
  %119 = va_arg i8** %10, i64
  store i64 %119, i64* %20, align 8
  %120 = load i64, i64* %20, align 8
  %121 = load i32, i32* %14, align 4
  %122 = sext i32 %121 to i64
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %120, i64* %124, align 8
  br label %172

125:                                              ; preds = %111
  %126 = load i32, i32* %14, align 4
  %127 = sext i32 %126 to i64
  %128 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %127
  %129 = load i8, i8* %128, align 1
  %130 = sext i8 %129 to i32
  %131 = icmp eq i32 %130, 70
  br i1 %131, label %132, label %140

132:                                              ; preds = %125
  %133 = va_arg i8** %10, double
  store double %133, double* %21, align 8
  %134 = load double, double* %21, align 8
  %135 = fptrunc double %134 to float
  %136 = load i32, i32* %14, align 4
  %137 = sext i32 %136 to i64
  %138 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %137
  %139 = bitcast %union.jvalue* %138 to float*
  store float %135, float* %139, align 8
  br label %171

140:                                              ; preds = %125
  %141 = load i32, i32* %14, align 4
  %142 = sext i32 %141 to i64
  %143 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %142
  %144 = load i8, i8* %143, align 1
  %145 = sext i8 %144 to i32
  %146 = icmp eq i32 %145, 68
  br i1 %146, label %147, label %154

147:                                              ; preds = %140
  %148 = va_arg i8** %10, double
  store double %148, double* %22, align 8
  %149 = load double, double* %22, align 8
  %150 = load i32, i32* %14, align 4
  %151 = sext i32 %150 to i64
  %152 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %151
  %153 = bitcast %union.jvalue* %152 to double*
  store double %149, double* %153, align 8
  br label %170

154:                                              ; preds = %140
  %155 = load i32, i32* %14, align 4
  %156 = sext i32 %155 to i64
  %157 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %156
  %158 = load i8, i8* %157, align 1
  %159 = sext i8 %158 to i32
  %160 = icmp eq i32 %159, 76
  br i1 %160, label %161, label %169

161:                                              ; preds = %154
  %162 = va_arg i8** %10, i8*
  store i8* %162, i8** %23, align 8
  %163 = load i8*, i8** %23, align 8
  %164 = bitcast i8* %163 to %struct._jobject*
  %165 = load i32, i32* %14, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %166
  %168 = bitcast %union.jvalue* %167 to %struct._jobject**
  store %struct._jobject* %164, %struct._jobject** %168, align 8
  br label %169

169:                                              ; preds = %161, %154
  br label %170

170:                                              ; preds = %169, %147
  br label %171

171:                                              ; preds = %170, %132
  br label %172

172:                                              ; preds = %171, %118
  br label %173

173:                                              ; preds = %172, %104
  br label %174

174:                                              ; preds = %173, %89
  br label %175

175:                                              ; preds = %174, %74
  br label %176

176:                                              ; preds = %175, %59
  br label %177

177:                                              ; preds = %176, %44
  br label %178

178:                                              ; preds = %177
  %179 = load i32, i32* %14, align 4
  %180 = add nsw i32 %179, 1
  store i32 %180, i32* %14, align 4
  br label %33, !llvm.loop !37

181:                                              ; preds = %33
  %182 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %183 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %182, align 8
  %184 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %183, i32 0, i32 78
  %185 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %184, align 8
  %186 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %187 = load %struct._jobject*, %struct._jobject** %7, align 8
  %188 = load %struct._jobject*, %struct._jobject** %8, align 8
  %189 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %190 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 0
  %191 = call signext i16 %185(%struct.JNINativeInterface_** noundef %186, %struct._jobject* noundef %187, %struct._jobject* noundef %188, %struct._jmethodID* noundef %189, %union.jvalue* noundef %190)
  ret i16 %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define signext i16 @JNI_CallStaticShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  %21 = alloca i16, align 2
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %22 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %22)
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %24 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %23, align 8
  %25 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %24, i32 0, i32 0
  %26 = load i8*, i8** %25, align 8
  %27 = bitcast i8* %26 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %28 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %29 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %31 = call i32 %27(%struct.JNINativeInterface_** noundef %28, %struct._jmethodID* noundef %29, i8* noundef %30)
  store i32 %31, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %32

32:                                               ; preds = %177, %3
  %33 = load i32, i32* %11, align 4
  %34 = load i32, i32* %10, align 4
  %35 = icmp slt i32 %33, %34
  br i1 %35, label %36, label %180

36:                                               ; preds = %32
  %37 = load i32, i32* %11, align 4
  %38 = sext i32 %37 to i64
  %39 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %38
  %40 = load i8, i8* %39, align 1
  %41 = sext i8 %40 to i32
  %42 = icmp eq i32 %41, 90
  br i1 %42, label %43, label %51

43:                                               ; preds = %36
  %44 = va_arg i8** %7, i32
  store i32 %44, i32* %12, align 4
  %45 = load i32, i32* %12, align 4
  %46 = trunc i32 %45 to i8
  %47 = load i32, i32* %11, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %176

51:                                               ; preds = %36
  %52 = load i32, i32* %11, align 4
  %53 = sext i32 %52 to i64
  %54 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %53
  %55 = load i8, i8* %54, align 1
  %56 = sext i8 %55 to i32
  %57 = icmp eq i32 %56, 66
  br i1 %57, label %58, label %66

58:                                               ; preds = %51
  %59 = va_arg i8** %7, i32
  store i32 %59, i32* %13, align 4
  %60 = load i32, i32* %13, align 4
  %61 = trunc i32 %60 to i8
  %62 = load i32, i32* %11, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %63
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %61, i8* %65, align 8
  br label %175

66:                                               ; preds = %51
  %67 = load i32, i32* %11, align 4
  %68 = sext i32 %67 to i64
  %69 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %68
  %70 = load i8, i8* %69, align 1
  %71 = sext i8 %70 to i32
  %72 = icmp eq i32 %71, 67
  br i1 %72, label %73, label %81

73:                                               ; preds = %66
  %74 = va_arg i8** %7, i32
  store i32 %74, i32* %14, align 4
  %75 = load i32, i32* %14, align 4
  %76 = trunc i32 %75 to i16
  %77 = load i32, i32* %11, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %174

81:                                               ; preds = %66
  %82 = load i32, i32* %11, align 4
  %83 = sext i32 %82 to i64
  %84 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %83
  %85 = load i8, i8* %84, align 1
  %86 = sext i8 %85 to i32
  %87 = icmp eq i32 %86, 83
  br i1 %87, label %88, label %96

88:                                               ; preds = %81
  %89 = va_arg i8** %7, i32
  store i32 %89, i32* %15, align 4
  %90 = load i32, i32* %15, align 4
  %91 = trunc i32 %90 to i16
  %92 = load i32, i32* %11, align 4
  %93 = sext i32 %92 to i64
  %94 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %93
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %91, i16* %95, align 8
  br label %173

96:                                               ; preds = %81
  %97 = load i32, i32* %11, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %98
  %100 = load i8, i8* %99, align 1
  %101 = sext i8 %100 to i32
  %102 = icmp eq i32 %101, 73
  br i1 %102, label %103, label %110

103:                                              ; preds = %96
  %104 = va_arg i8** %7, i32
  store i32 %104, i32* %16, align 4
  %105 = load i32, i32* %16, align 4
  %106 = load i32, i32* %11, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %107
  %109 = bitcast %union.jvalue* %108 to i32*
  store i32 %105, i32* %109, align 8
  br label %172

110:                                              ; preds = %96
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %112
  %114 = load i8, i8* %113, align 1
  %115 = sext i8 %114 to i32
  %116 = icmp eq i32 %115, 74
  br i1 %116, label %117, label %124

117:                                              ; preds = %110
  %118 = va_arg i8** %7, i64
  store i64 %118, i64* %17, align 8
  %119 = load i64, i64* %17, align 8
  %120 = load i32, i32* %11, align 4
  %121 = sext i32 %120 to i64
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %119, i64* %123, align 8
  br label %171

124:                                              ; preds = %110
  %125 = load i32, i32* %11, align 4
  %126 = sext i32 %125 to i64
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %126
  %128 = load i8, i8* %127, align 1
  %129 = sext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %139

131:                                              ; preds = %124
  %132 = va_arg i8** %7, double
  store double %132, double* %18, align 8
  %133 = load double, double* %18, align 8
  %134 = fptrunc double %133 to float
  %135 = load i32, i32* %11, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %136
  %138 = bitcast %union.jvalue* %137 to float*
  store float %134, float* %138, align 8
  br label %170

139:                                              ; preds = %124
  %140 = load i32, i32* %11, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %141
  %143 = load i8, i8* %142, align 1
  %144 = sext i8 %143 to i32
  %145 = icmp eq i32 %144, 68
  br i1 %145, label %146, label %153

146:                                              ; preds = %139
  %147 = va_arg i8** %7, double
  store double %147, double* %19, align 8
  %148 = load double, double* %19, align 8
  %149 = load i32, i32* %11, align 4
  %150 = sext i32 %149 to i64
  %151 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %150
  %152 = bitcast %union.jvalue* %151 to double*
  store double %148, double* %152, align 8
  br label %169

153:                                              ; preds = %139
  %154 = load i32, i32* %11, align 4
  %155 = sext i32 %154 to i64
  %156 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %155
  %157 = load i8, i8* %156, align 1
  %158 = sext i8 %157 to i32
  %159 = icmp eq i32 %158, 76
  br i1 %159, label %160, label %168

160:                                              ; preds = %153
  %161 = va_arg i8** %7, i8*
  store i8* %161, i8** %20, align 8
  %162 = load i8*, i8** %20, align 8
  %163 = bitcast i8* %162 to %struct._jobject*
  %164 = load i32, i32* %11, align 4
  %165 = sext i32 %164 to i64
  %166 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %165
  %167 = bitcast %union.jvalue* %166 to %struct._jobject**
  store %struct._jobject* %163, %struct._jobject** %167, align 8
  br label %168

168:                                              ; preds = %160, %153
  br label %169

169:                                              ; preds = %168, %146
  br label %170

170:                                              ; preds = %169, %131
  br label %171

171:                                              ; preds = %170, %117
  br label %172

172:                                              ; preds = %171, %103
  br label %173

173:                                              ; preds = %172, %88
  br label %174

174:                                              ; preds = %173, %73
  br label %175

175:                                              ; preds = %174, %58
  br label %176

176:                                              ; preds = %175, %43
  br label %177

177:                                              ; preds = %176
  %178 = load i32, i32* %11, align 4
  %179 = add nsw i32 %178, 1
  store i32 %179, i32* %11, align 4
  br label %32, !llvm.loop !38

180:                                              ; preds = %32
  %181 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %182 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %181, align 8
  %183 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %182, i32 0, i32 128
  %184 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %183, align 8
  %185 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %186 = load %struct._jobject*, %struct._jobject** %5, align 8
  %187 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %188 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  %189 = call signext i16 %184(%struct.JNINativeInterface_** noundef %185, %struct._jobject* noundef %186, %struct._jmethodID* noundef %187, %union.jvalue* noundef %188)
  store i16 %189, i16* %21, align 2
  %190 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %190)
  %191 = load i16, i16* %21, align 2
  ret i16 %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define signext i16 @JNI_CallStaticShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !39

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 128
  %183 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  %188 = call signext i16 %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret i16 %188
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define i32 @JNI_CallIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  %21 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %22 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %22)
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %24 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %23, align 8
  %25 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %24, i32 0, i32 0
  %26 = load i8*, i8** %25, align 8
  %27 = bitcast i8* %26 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %28 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %29 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %31 = call i32 %27(%struct.JNINativeInterface_** noundef %28, %struct._jmethodID* noundef %29, i8* noundef %30)
  store i32 %31, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %32

32:                                               ; preds = %177, %3
  %33 = load i32, i32* %11, align 4
  %34 = load i32, i32* %10, align 4
  %35 = icmp slt i32 %33, %34
  br i1 %35, label %36, label %180

36:                                               ; preds = %32
  %37 = load i32, i32* %11, align 4
  %38 = sext i32 %37 to i64
  %39 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %38
  %40 = load i8, i8* %39, align 1
  %41 = sext i8 %40 to i32
  %42 = icmp eq i32 %41, 90
  br i1 %42, label %43, label %51

43:                                               ; preds = %36
  %44 = va_arg i8** %7, i32
  store i32 %44, i32* %12, align 4
  %45 = load i32, i32* %12, align 4
  %46 = trunc i32 %45 to i8
  %47 = load i32, i32* %11, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %176

51:                                               ; preds = %36
  %52 = load i32, i32* %11, align 4
  %53 = sext i32 %52 to i64
  %54 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %53
  %55 = load i8, i8* %54, align 1
  %56 = sext i8 %55 to i32
  %57 = icmp eq i32 %56, 66
  br i1 %57, label %58, label %66

58:                                               ; preds = %51
  %59 = va_arg i8** %7, i32
  store i32 %59, i32* %13, align 4
  %60 = load i32, i32* %13, align 4
  %61 = trunc i32 %60 to i8
  %62 = load i32, i32* %11, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %63
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %61, i8* %65, align 8
  br label %175

66:                                               ; preds = %51
  %67 = load i32, i32* %11, align 4
  %68 = sext i32 %67 to i64
  %69 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %68
  %70 = load i8, i8* %69, align 1
  %71 = sext i8 %70 to i32
  %72 = icmp eq i32 %71, 67
  br i1 %72, label %73, label %81

73:                                               ; preds = %66
  %74 = va_arg i8** %7, i32
  store i32 %74, i32* %14, align 4
  %75 = load i32, i32* %14, align 4
  %76 = trunc i32 %75 to i16
  %77 = load i32, i32* %11, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %174

81:                                               ; preds = %66
  %82 = load i32, i32* %11, align 4
  %83 = sext i32 %82 to i64
  %84 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %83
  %85 = load i8, i8* %84, align 1
  %86 = sext i8 %85 to i32
  %87 = icmp eq i32 %86, 83
  br i1 %87, label %88, label %96

88:                                               ; preds = %81
  %89 = va_arg i8** %7, i32
  store i32 %89, i32* %15, align 4
  %90 = load i32, i32* %15, align 4
  %91 = trunc i32 %90 to i16
  %92 = load i32, i32* %11, align 4
  %93 = sext i32 %92 to i64
  %94 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %93
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %91, i16* %95, align 8
  br label %173

96:                                               ; preds = %81
  %97 = load i32, i32* %11, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %98
  %100 = load i8, i8* %99, align 1
  %101 = sext i8 %100 to i32
  %102 = icmp eq i32 %101, 73
  br i1 %102, label %103, label %110

103:                                              ; preds = %96
  %104 = va_arg i8** %7, i32
  store i32 %104, i32* %16, align 4
  %105 = load i32, i32* %16, align 4
  %106 = load i32, i32* %11, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %107
  %109 = bitcast %union.jvalue* %108 to i32*
  store i32 %105, i32* %109, align 8
  br label %172

110:                                              ; preds = %96
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %112
  %114 = load i8, i8* %113, align 1
  %115 = sext i8 %114 to i32
  %116 = icmp eq i32 %115, 74
  br i1 %116, label %117, label %124

117:                                              ; preds = %110
  %118 = va_arg i8** %7, i64
  store i64 %118, i64* %17, align 8
  %119 = load i64, i64* %17, align 8
  %120 = load i32, i32* %11, align 4
  %121 = sext i32 %120 to i64
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %119, i64* %123, align 8
  br label %171

124:                                              ; preds = %110
  %125 = load i32, i32* %11, align 4
  %126 = sext i32 %125 to i64
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %126
  %128 = load i8, i8* %127, align 1
  %129 = sext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %139

131:                                              ; preds = %124
  %132 = va_arg i8** %7, double
  store double %132, double* %18, align 8
  %133 = load double, double* %18, align 8
  %134 = fptrunc double %133 to float
  %135 = load i32, i32* %11, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %136
  %138 = bitcast %union.jvalue* %137 to float*
  store float %134, float* %138, align 8
  br label %170

139:                                              ; preds = %124
  %140 = load i32, i32* %11, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %141
  %143 = load i8, i8* %142, align 1
  %144 = sext i8 %143 to i32
  %145 = icmp eq i32 %144, 68
  br i1 %145, label %146, label %153

146:                                              ; preds = %139
  %147 = va_arg i8** %7, double
  store double %147, double* %19, align 8
  %148 = load double, double* %19, align 8
  %149 = load i32, i32* %11, align 4
  %150 = sext i32 %149 to i64
  %151 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %150
  %152 = bitcast %union.jvalue* %151 to double*
  store double %148, double* %152, align 8
  br label %169

153:                                              ; preds = %139
  %154 = load i32, i32* %11, align 4
  %155 = sext i32 %154 to i64
  %156 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %155
  %157 = load i8, i8* %156, align 1
  %158 = sext i8 %157 to i32
  %159 = icmp eq i32 %158, 76
  br i1 %159, label %160, label %168

160:                                              ; preds = %153
  %161 = va_arg i8** %7, i8*
  store i8* %161, i8** %20, align 8
  %162 = load i8*, i8** %20, align 8
  %163 = bitcast i8* %162 to %struct._jobject*
  %164 = load i32, i32* %11, align 4
  %165 = sext i32 %164 to i64
  %166 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %165
  %167 = bitcast %union.jvalue* %166 to %struct._jobject**
  store %struct._jobject* %163, %struct._jobject** %167, align 8
  br label %168

168:                                              ; preds = %160, %153
  br label %169

169:                                              ; preds = %168, %146
  br label %170

170:                                              ; preds = %169, %131
  br label %171

171:                                              ; preds = %170, %117
  br label %172

172:                                              ; preds = %171, %103
  br label %173

173:                                              ; preds = %172, %88
  br label %174

174:                                              ; preds = %173, %73
  br label %175

175:                                              ; preds = %174, %58
  br label %176

176:                                              ; preds = %175, %43
  br label %177

177:                                              ; preds = %176
  %178 = load i32, i32* %11, align 4
  %179 = add nsw i32 %178, 1
  store i32 %179, i32* %11, align 4
  br label %32, !llvm.loop !40

180:                                              ; preds = %32
  %181 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %182 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %181, align 8
  %183 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %182, i32 0, i32 51
  %184 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %183, align 8
  %185 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %186 = load %struct._jobject*, %struct._jobject** %5, align 8
  %187 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %188 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  %189 = call i32 %184(%struct.JNINativeInterface_** noundef %185, %struct._jobject* noundef %186, %struct._jmethodID* noundef %187, %union.jvalue* noundef %188)
  store i32 %189, i32* %21, align 4
  %190 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %190)
  %191 = load i32, i32* %21, align 4
  ret i32 %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define i32 @JNI_CallIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !41

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 51
  %183 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  %188 = call i32 %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret i32 %188
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define i32 @JNI_CallNonvirtualIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i8*, align 8
  %10 = alloca [256 x i8], align 1
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i32, align 4
  %19 = alloca i64, align 8
  %20 = alloca double, align 8
  %21 = alloca double, align 8
  %22 = alloca i8*, align 8
  %23 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %24 = bitcast i8** %9 to i8*
  call void @llvm.va_start(i8* %24)
  %25 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %26 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %25, align 8
  %27 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %26, i32 0, i32 0
  %28 = load i8*, i8** %27, align 8
  %29 = bitcast i8* %28 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %30 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %31 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 0
  %33 = call i32 %29(%struct.JNINativeInterface_** noundef %30, %struct._jmethodID* noundef %31, i8* noundef %32)
  store i32 %33, i32* %12, align 4
  store i32 0, i32* %13, align 4
  br label %34

34:                                               ; preds = %179, %4
  %35 = load i32, i32* %13, align 4
  %36 = load i32, i32* %12, align 4
  %37 = icmp slt i32 %35, %36
  br i1 %37, label %38, label %182

38:                                               ; preds = %34
  %39 = load i32, i32* %13, align 4
  %40 = sext i32 %39 to i64
  %41 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %40
  %42 = load i8, i8* %41, align 1
  %43 = sext i8 %42 to i32
  %44 = icmp eq i32 %43, 90
  br i1 %44, label %45, label %53

45:                                               ; preds = %38
  %46 = va_arg i8** %9, i32
  store i32 %46, i32* %14, align 4
  %47 = load i32, i32* %14, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %13, align 4
  %50 = sext i32 %49 to i64
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %48, i8* %52, align 8
  br label %178

53:                                               ; preds = %38
  %54 = load i32, i32* %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %55
  %57 = load i8, i8* %56, align 1
  %58 = sext i8 %57 to i32
  %59 = icmp eq i32 %58, 66
  br i1 %59, label %60, label %68

60:                                               ; preds = %53
  %61 = va_arg i8** %9, i32
  store i32 %61, i32* %15, align 4
  %62 = load i32, i32* %15, align 4
  %63 = trunc i32 %62 to i8
  %64 = load i32, i32* %13, align 4
  %65 = sext i32 %64 to i64
  %66 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %65
  %67 = bitcast %union.jvalue* %66 to i8*
  store i8 %63, i8* %67, align 8
  br label %177

68:                                               ; preds = %53
  %69 = load i32, i32* %13, align 4
  %70 = sext i32 %69 to i64
  %71 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %70
  %72 = load i8, i8* %71, align 1
  %73 = sext i8 %72 to i32
  %74 = icmp eq i32 %73, 67
  br i1 %74, label %75, label %83

75:                                               ; preds = %68
  %76 = va_arg i8** %9, i32
  store i32 %76, i32* %16, align 4
  %77 = load i32, i32* %16, align 4
  %78 = trunc i32 %77 to i16
  %79 = load i32, i32* %13, align 4
  %80 = sext i32 %79 to i64
  %81 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %80
  %82 = bitcast %union.jvalue* %81 to i16*
  store i16 %78, i16* %82, align 8
  br label %176

83:                                               ; preds = %68
  %84 = load i32, i32* %13, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %85
  %87 = load i8, i8* %86, align 1
  %88 = sext i8 %87 to i32
  %89 = icmp eq i32 %88, 83
  br i1 %89, label %90, label %98

90:                                               ; preds = %83
  %91 = va_arg i8** %9, i32
  store i32 %91, i32* %17, align 4
  %92 = load i32, i32* %17, align 4
  %93 = trunc i32 %92 to i16
  %94 = load i32, i32* %13, align 4
  %95 = sext i32 %94 to i64
  %96 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %95
  %97 = bitcast %union.jvalue* %96 to i16*
  store i16 %93, i16* %97, align 8
  br label %175

98:                                               ; preds = %83
  %99 = load i32, i32* %13, align 4
  %100 = sext i32 %99 to i64
  %101 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %100
  %102 = load i8, i8* %101, align 1
  %103 = sext i8 %102 to i32
  %104 = icmp eq i32 %103, 73
  br i1 %104, label %105, label %112

105:                                              ; preds = %98
  %106 = va_arg i8** %9, i32
  store i32 %106, i32* %18, align 4
  %107 = load i32, i32* %18, align 4
  %108 = load i32, i32* %13, align 4
  %109 = sext i32 %108 to i64
  %110 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %109
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %107, i32* %111, align 8
  br label %174

112:                                              ; preds = %98
  %113 = load i32, i32* %13, align 4
  %114 = sext i32 %113 to i64
  %115 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %114
  %116 = load i8, i8* %115, align 1
  %117 = sext i8 %116 to i32
  %118 = icmp eq i32 %117, 74
  br i1 %118, label %119, label %126

119:                                              ; preds = %112
  %120 = va_arg i8** %9, i64
  store i64 %120, i64* %19, align 8
  %121 = load i64, i64* %19, align 8
  %122 = load i32, i32* %13, align 4
  %123 = sext i32 %122 to i64
  %124 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %123
  %125 = bitcast %union.jvalue* %124 to i64*
  store i64 %121, i64* %125, align 8
  br label %173

126:                                              ; preds = %112
  %127 = load i32, i32* %13, align 4
  %128 = sext i32 %127 to i64
  %129 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %128
  %130 = load i8, i8* %129, align 1
  %131 = sext i8 %130 to i32
  %132 = icmp eq i32 %131, 70
  br i1 %132, label %133, label %141

133:                                              ; preds = %126
  %134 = va_arg i8** %9, double
  store double %134, double* %20, align 8
  %135 = load double, double* %20, align 8
  %136 = fptrunc double %135 to float
  %137 = load i32, i32* %13, align 4
  %138 = sext i32 %137 to i64
  %139 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %138
  %140 = bitcast %union.jvalue* %139 to float*
  store float %136, float* %140, align 8
  br label %172

141:                                              ; preds = %126
  %142 = load i32, i32* %13, align 4
  %143 = sext i32 %142 to i64
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %143
  %145 = load i8, i8* %144, align 1
  %146 = sext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %155

148:                                              ; preds = %141
  %149 = va_arg i8** %9, double
  store double %149, double* %21, align 8
  %150 = load double, double* %21, align 8
  %151 = load i32, i32* %13, align 4
  %152 = sext i32 %151 to i64
  %153 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %152
  %154 = bitcast %union.jvalue* %153 to double*
  store double %150, double* %154, align 8
  br label %171

155:                                              ; preds = %141
  %156 = load i32, i32* %13, align 4
  %157 = sext i32 %156 to i64
  %158 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %157
  %159 = load i8, i8* %158, align 1
  %160 = sext i8 %159 to i32
  %161 = icmp eq i32 %160, 76
  br i1 %161, label %162, label %170

162:                                              ; preds = %155
  %163 = va_arg i8** %9, i8*
  store i8* %163, i8** %22, align 8
  %164 = load i8*, i8** %22, align 8
  %165 = bitcast i8* %164 to %struct._jobject*
  %166 = load i32, i32* %13, align 4
  %167 = sext i32 %166 to i64
  %168 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %167
  %169 = bitcast %union.jvalue* %168 to %struct._jobject**
  store %struct._jobject* %165, %struct._jobject** %169, align 8
  br label %170

170:                                              ; preds = %162, %155
  br label %171

171:                                              ; preds = %170, %148
  br label %172

172:                                              ; preds = %171, %133
  br label %173

173:                                              ; preds = %172, %119
  br label %174

174:                                              ; preds = %173, %105
  br label %175

175:                                              ; preds = %174, %90
  br label %176

176:                                              ; preds = %175, %75
  br label %177

177:                                              ; preds = %176, %60
  br label %178

178:                                              ; preds = %177, %45
  br label %179

179:                                              ; preds = %178
  %180 = load i32, i32* %13, align 4
  %181 = add nsw i32 %180, 1
  store i32 %181, i32* %13, align 4
  br label %34, !llvm.loop !42

182:                                              ; preds = %34
  %183 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %184 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %183, align 8
  %185 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %184, i32 0, i32 81
  %186 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %185, align 8
  %187 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %188 = load %struct._jobject*, %struct._jobject** %6, align 8
  %189 = load %struct._jobject*, %struct._jobject** %7, align 8
  %190 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %191 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 0
  %192 = call i32 %186(%struct.JNINativeInterface_** noundef %187, %struct._jobject* noundef %188, %struct._jobject* noundef %189, %struct._jmethodID* noundef %190, %union.jvalue* noundef %191)
  store i32 %192, i32* %23, align 4
  %193 = bitcast i8** %9 to i8*
  call void @llvm.va_end(i8* %193)
  %194 = load i32, i32* %23, align 4
  ret i32 %194
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define i32 @JNI_CallNonvirtualIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, i8* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca i8*, align 8
  %11 = alloca [256 x i8], align 1
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i32, align 4
  %19 = alloca i32, align 4
  %20 = alloca i64, align 8
  %21 = alloca double, align 8
  %22 = alloca double, align 8
  %23 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  store i8* %4, i8** %10, align 8
  %24 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %25 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %25, i32 0, i32 0
  %27 = load i8*, i8** %26, align 8
  %28 = bitcast i8* %27 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %29 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %30 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 0
  %32 = call i32 %28(%struct.JNINativeInterface_** noundef %29, %struct._jmethodID* noundef %30, i8* noundef %31)
  store i32 %32, i32* %13, align 4
  store i32 0, i32* %14, align 4
  br label %33

33:                                               ; preds = %178, %5
  %34 = load i32, i32* %14, align 4
  %35 = load i32, i32* %13, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %181

37:                                               ; preds = %33
  %38 = load i32, i32* %14, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %39
  %41 = load i8, i8* %40, align 1
  %42 = sext i8 %41 to i32
  %43 = icmp eq i32 %42, 90
  br i1 %43, label %44, label %52

44:                                               ; preds = %37
  %45 = va_arg i8** %10, i32
  store i32 %45, i32* %15, align 4
  %46 = load i32, i32* %15, align 4
  %47 = trunc i32 %46 to i8
  %48 = load i32, i32* %14, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %47, i8* %51, align 8
  br label %177

52:                                               ; preds = %37
  %53 = load i32, i32* %14, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %54
  %56 = load i8, i8* %55, align 1
  %57 = sext i8 %56 to i32
  %58 = icmp eq i32 %57, 66
  br i1 %58, label %59, label %67

59:                                               ; preds = %52
  %60 = va_arg i8** %10, i32
  store i32 %60, i32* %16, align 4
  %61 = load i32, i32* %16, align 4
  %62 = trunc i32 %61 to i8
  %63 = load i32, i32* %14, align 4
  %64 = sext i32 %63 to i64
  %65 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %64
  %66 = bitcast %union.jvalue* %65 to i8*
  store i8 %62, i8* %66, align 8
  br label %176

67:                                               ; preds = %52
  %68 = load i32, i32* %14, align 4
  %69 = sext i32 %68 to i64
  %70 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %69
  %71 = load i8, i8* %70, align 1
  %72 = sext i8 %71 to i32
  %73 = icmp eq i32 %72, 67
  br i1 %73, label %74, label %82

74:                                               ; preds = %67
  %75 = va_arg i8** %10, i32
  store i32 %75, i32* %17, align 4
  %76 = load i32, i32* %17, align 4
  %77 = trunc i32 %76 to i16
  %78 = load i32, i32* %14, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %175

82:                                               ; preds = %67
  %83 = load i32, i32* %14, align 4
  %84 = sext i32 %83 to i64
  %85 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %84
  %86 = load i8, i8* %85, align 1
  %87 = sext i8 %86 to i32
  %88 = icmp eq i32 %87, 83
  br i1 %88, label %89, label %97

89:                                               ; preds = %82
  %90 = va_arg i8** %10, i32
  store i32 %90, i32* %18, align 4
  %91 = load i32, i32* %18, align 4
  %92 = trunc i32 %91 to i16
  %93 = load i32, i32* %14, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %94
  %96 = bitcast %union.jvalue* %95 to i16*
  store i16 %92, i16* %96, align 8
  br label %174

97:                                               ; preds = %82
  %98 = load i32, i32* %14, align 4
  %99 = sext i32 %98 to i64
  %100 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %99
  %101 = load i8, i8* %100, align 1
  %102 = sext i8 %101 to i32
  %103 = icmp eq i32 %102, 73
  br i1 %103, label %104, label %111

104:                                              ; preds = %97
  %105 = va_arg i8** %10, i32
  store i32 %105, i32* %19, align 4
  %106 = load i32, i32* %19, align 4
  %107 = load i32, i32* %14, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %108
  %110 = bitcast %union.jvalue* %109 to i32*
  store i32 %106, i32* %110, align 8
  br label %173

111:                                              ; preds = %97
  %112 = load i32, i32* %14, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %113
  %115 = load i8, i8* %114, align 1
  %116 = sext i8 %115 to i32
  %117 = icmp eq i32 %116, 74
  br i1 %117, label %118, label %125

118:                                              ; preds = %111
  %119 = va_arg i8** %10, i64
  store i64 %119, i64* %20, align 8
  %120 = load i64, i64* %20, align 8
  %121 = load i32, i32* %14, align 4
  %122 = sext i32 %121 to i64
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %120, i64* %124, align 8
  br label %172

125:                                              ; preds = %111
  %126 = load i32, i32* %14, align 4
  %127 = sext i32 %126 to i64
  %128 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %127
  %129 = load i8, i8* %128, align 1
  %130 = sext i8 %129 to i32
  %131 = icmp eq i32 %130, 70
  br i1 %131, label %132, label %140

132:                                              ; preds = %125
  %133 = va_arg i8** %10, double
  store double %133, double* %21, align 8
  %134 = load double, double* %21, align 8
  %135 = fptrunc double %134 to float
  %136 = load i32, i32* %14, align 4
  %137 = sext i32 %136 to i64
  %138 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %137
  %139 = bitcast %union.jvalue* %138 to float*
  store float %135, float* %139, align 8
  br label %171

140:                                              ; preds = %125
  %141 = load i32, i32* %14, align 4
  %142 = sext i32 %141 to i64
  %143 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %142
  %144 = load i8, i8* %143, align 1
  %145 = sext i8 %144 to i32
  %146 = icmp eq i32 %145, 68
  br i1 %146, label %147, label %154

147:                                              ; preds = %140
  %148 = va_arg i8** %10, double
  store double %148, double* %22, align 8
  %149 = load double, double* %22, align 8
  %150 = load i32, i32* %14, align 4
  %151 = sext i32 %150 to i64
  %152 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %151
  %153 = bitcast %union.jvalue* %152 to double*
  store double %149, double* %153, align 8
  br label %170

154:                                              ; preds = %140
  %155 = load i32, i32* %14, align 4
  %156 = sext i32 %155 to i64
  %157 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %156
  %158 = load i8, i8* %157, align 1
  %159 = sext i8 %158 to i32
  %160 = icmp eq i32 %159, 76
  br i1 %160, label %161, label %169

161:                                              ; preds = %154
  %162 = va_arg i8** %10, i8*
  store i8* %162, i8** %23, align 8
  %163 = load i8*, i8** %23, align 8
  %164 = bitcast i8* %163 to %struct._jobject*
  %165 = load i32, i32* %14, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %166
  %168 = bitcast %union.jvalue* %167 to %struct._jobject**
  store %struct._jobject* %164, %struct._jobject** %168, align 8
  br label %169

169:                                              ; preds = %161, %154
  br label %170

170:                                              ; preds = %169, %147
  br label %171

171:                                              ; preds = %170, %132
  br label %172

172:                                              ; preds = %171, %118
  br label %173

173:                                              ; preds = %172, %104
  br label %174

174:                                              ; preds = %173, %89
  br label %175

175:                                              ; preds = %174, %74
  br label %176

176:                                              ; preds = %175, %59
  br label %177

177:                                              ; preds = %176, %44
  br label %178

178:                                              ; preds = %177
  %179 = load i32, i32* %14, align 4
  %180 = add nsw i32 %179, 1
  store i32 %180, i32* %14, align 4
  br label %33, !llvm.loop !43

181:                                              ; preds = %33
  %182 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %183 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %182, align 8
  %184 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %183, i32 0, i32 81
  %185 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %184, align 8
  %186 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %187 = load %struct._jobject*, %struct._jobject** %7, align 8
  %188 = load %struct._jobject*, %struct._jobject** %8, align 8
  %189 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %190 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 0
  %191 = call i32 %185(%struct.JNINativeInterface_** noundef %186, %struct._jobject* noundef %187, %struct._jobject* noundef %188, %struct._jmethodID* noundef %189, %union.jvalue* noundef %190)
  ret i32 %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define i32 @JNI_CallStaticIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  %21 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %22 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %22)
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %24 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %23, align 8
  %25 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %24, i32 0, i32 0
  %26 = load i8*, i8** %25, align 8
  %27 = bitcast i8* %26 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %28 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %29 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %31 = call i32 %27(%struct.JNINativeInterface_** noundef %28, %struct._jmethodID* noundef %29, i8* noundef %30)
  store i32 %31, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %32

32:                                               ; preds = %177, %3
  %33 = load i32, i32* %11, align 4
  %34 = load i32, i32* %10, align 4
  %35 = icmp slt i32 %33, %34
  br i1 %35, label %36, label %180

36:                                               ; preds = %32
  %37 = load i32, i32* %11, align 4
  %38 = sext i32 %37 to i64
  %39 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %38
  %40 = load i8, i8* %39, align 1
  %41 = sext i8 %40 to i32
  %42 = icmp eq i32 %41, 90
  br i1 %42, label %43, label %51

43:                                               ; preds = %36
  %44 = va_arg i8** %7, i32
  store i32 %44, i32* %12, align 4
  %45 = load i32, i32* %12, align 4
  %46 = trunc i32 %45 to i8
  %47 = load i32, i32* %11, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %176

51:                                               ; preds = %36
  %52 = load i32, i32* %11, align 4
  %53 = sext i32 %52 to i64
  %54 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %53
  %55 = load i8, i8* %54, align 1
  %56 = sext i8 %55 to i32
  %57 = icmp eq i32 %56, 66
  br i1 %57, label %58, label %66

58:                                               ; preds = %51
  %59 = va_arg i8** %7, i32
  store i32 %59, i32* %13, align 4
  %60 = load i32, i32* %13, align 4
  %61 = trunc i32 %60 to i8
  %62 = load i32, i32* %11, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %63
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %61, i8* %65, align 8
  br label %175

66:                                               ; preds = %51
  %67 = load i32, i32* %11, align 4
  %68 = sext i32 %67 to i64
  %69 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %68
  %70 = load i8, i8* %69, align 1
  %71 = sext i8 %70 to i32
  %72 = icmp eq i32 %71, 67
  br i1 %72, label %73, label %81

73:                                               ; preds = %66
  %74 = va_arg i8** %7, i32
  store i32 %74, i32* %14, align 4
  %75 = load i32, i32* %14, align 4
  %76 = trunc i32 %75 to i16
  %77 = load i32, i32* %11, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %174

81:                                               ; preds = %66
  %82 = load i32, i32* %11, align 4
  %83 = sext i32 %82 to i64
  %84 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %83
  %85 = load i8, i8* %84, align 1
  %86 = sext i8 %85 to i32
  %87 = icmp eq i32 %86, 83
  br i1 %87, label %88, label %96

88:                                               ; preds = %81
  %89 = va_arg i8** %7, i32
  store i32 %89, i32* %15, align 4
  %90 = load i32, i32* %15, align 4
  %91 = trunc i32 %90 to i16
  %92 = load i32, i32* %11, align 4
  %93 = sext i32 %92 to i64
  %94 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %93
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %91, i16* %95, align 8
  br label %173

96:                                               ; preds = %81
  %97 = load i32, i32* %11, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %98
  %100 = load i8, i8* %99, align 1
  %101 = sext i8 %100 to i32
  %102 = icmp eq i32 %101, 73
  br i1 %102, label %103, label %110

103:                                              ; preds = %96
  %104 = va_arg i8** %7, i32
  store i32 %104, i32* %16, align 4
  %105 = load i32, i32* %16, align 4
  %106 = load i32, i32* %11, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %107
  %109 = bitcast %union.jvalue* %108 to i32*
  store i32 %105, i32* %109, align 8
  br label %172

110:                                              ; preds = %96
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %112
  %114 = load i8, i8* %113, align 1
  %115 = sext i8 %114 to i32
  %116 = icmp eq i32 %115, 74
  br i1 %116, label %117, label %124

117:                                              ; preds = %110
  %118 = va_arg i8** %7, i64
  store i64 %118, i64* %17, align 8
  %119 = load i64, i64* %17, align 8
  %120 = load i32, i32* %11, align 4
  %121 = sext i32 %120 to i64
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %119, i64* %123, align 8
  br label %171

124:                                              ; preds = %110
  %125 = load i32, i32* %11, align 4
  %126 = sext i32 %125 to i64
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %126
  %128 = load i8, i8* %127, align 1
  %129 = sext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %139

131:                                              ; preds = %124
  %132 = va_arg i8** %7, double
  store double %132, double* %18, align 8
  %133 = load double, double* %18, align 8
  %134 = fptrunc double %133 to float
  %135 = load i32, i32* %11, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %136
  %138 = bitcast %union.jvalue* %137 to float*
  store float %134, float* %138, align 8
  br label %170

139:                                              ; preds = %124
  %140 = load i32, i32* %11, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %141
  %143 = load i8, i8* %142, align 1
  %144 = sext i8 %143 to i32
  %145 = icmp eq i32 %144, 68
  br i1 %145, label %146, label %153

146:                                              ; preds = %139
  %147 = va_arg i8** %7, double
  store double %147, double* %19, align 8
  %148 = load double, double* %19, align 8
  %149 = load i32, i32* %11, align 4
  %150 = sext i32 %149 to i64
  %151 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %150
  %152 = bitcast %union.jvalue* %151 to double*
  store double %148, double* %152, align 8
  br label %169

153:                                              ; preds = %139
  %154 = load i32, i32* %11, align 4
  %155 = sext i32 %154 to i64
  %156 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %155
  %157 = load i8, i8* %156, align 1
  %158 = sext i8 %157 to i32
  %159 = icmp eq i32 %158, 76
  br i1 %159, label %160, label %168

160:                                              ; preds = %153
  %161 = va_arg i8** %7, i8*
  store i8* %161, i8** %20, align 8
  %162 = load i8*, i8** %20, align 8
  %163 = bitcast i8* %162 to %struct._jobject*
  %164 = load i32, i32* %11, align 4
  %165 = sext i32 %164 to i64
  %166 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %165
  %167 = bitcast %union.jvalue* %166 to %struct._jobject**
  store %struct._jobject* %163, %struct._jobject** %167, align 8
  br label %168

168:                                              ; preds = %160, %153
  br label %169

169:                                              ; preds = %168, %146
  br label %170

170:                                              ; preds = %169, %131
  br label %171

171:                                              ; preds = %170, %117
  br label %172

172:                                              ; preds = %171, %103
  br label %173

173:                                              ; preds = %172, %88
  br label %174

174:                                              ; preds = %173, %73
  br label %175

175:                                              ; preds = %174, %58
  br label %176

176:                                              ; preds = %175, %43
  br label %177

177:                                              ; preds = %176
  %178 = load i32, i32* %11, align 4
  %179 = add nsw i32 %178, 1
  store i32 %179, i32* %11, align 4
  br label %32, !llvm.loop !44

180:                                              ; preds = %32
  %181 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %182 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %181, align 8
  %183 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %182, i32 0, i32 131
  %184 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %183, align 8
  %185 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %186 = load %struct._jobject*, %struct._jobject** %5, align 8
  %187 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %188 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  %189 = call i32 %184(%struct.JNINativeInterface_** noundef %185, %struct._jobject* noundef %186, %struct._jmethodID* noundef %187, %union.jvalue* noundef %188)
  store i32 %189, i32* %21, align 4
  %190 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %190)
  %191 = load i32, i32* %21, align 4
  ret i32 %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define i32 @JNI_CallStaticIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !45

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 131
  %183 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  %188 = call i32 %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret i32 %188
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define i64 @JNI_CallLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  %21 = alloca i64, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %22 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %22)
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %24 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %23, align 8
  %25 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %24, i32 0, i32 0
  %26 = load i8*, i8** %25, align 8
  %27 = bitcast i8* %26 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %28 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %29 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %31 = call i32 %27(%struct.JNINativeInterface_** noundef %28, %struct._jmethodID* noundef %29, i8* noundef %30)
  store i32 %31, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %32

32:                                               ; preds = %177, %3
  %33 = load i32, i32* %11, align 4
  %34 = load i32, i32* %10, align 4
  %35 = icmp slt i32 %33, %34
  br i1 %35, label %36, label %180

36:                                               ; preds = %32
  %37 = load i32, i32* %11, align 4
  %38 = sext i32 %37 to i64
  %39 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %38
  %40 = load i8, i8* %39, align 1
  %41 = sext i8 %40 to i32
  %42 = icmp eq i32 %41, 90
  br i1 %42, label %43, label %51

43:                                               ; preds = %36
  %44 = va_arg i8** %7, i32
  store i32 %44, i32* %12, align 4
  %45 = load i32, i32* %12, align 4
  %46 = trunc i32 %45 to i8
  %47 = load i32, i32* %11, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %176

51:                                               ; preds = %36
  %52 = load i32, i32* %11, align 4
  %53 = sext i32 %52 to i64
  %54 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %53
  %55 = load i8, i8* %54, align 1
  %56 = sext i8 %55 to i32
  %57 = icmp eq i32 %56, 66
  br i1 %57, label %58, label %66

58:                                               ; preds = %51
  %59 = va_arg i8** %7, i32
  store i32 %59, i32* %13, align 4
  %60 = load i32, i32* %13, align 4
  %61 = trunc i32 %60 to i8
  %62 = load i32, i32* %11, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %63
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %61, i8* %65, align 8
  br label %175

66:                                               ; preds = %51
  %67 = load i32, i32* %11, align 4
  %68 = sext i32 %67 to i64
  %69 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %68
  %70 = load i8, i8* %69, align 1
  %71 = sext i8 %70 to i32
  %72 = icmp eq i32 %71, 67
  br i1 %72, label %73, label %81

73:                                               ; preds = %66
  %74 = va_arg i8** %7, i32
  store i32 %74, i32* %14, align 4
  %75 = load i32, i32* %14, align 4
  %76 = trunc i32 %75 to i16
  %77 = load i32, i32* %11, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %174

81:                                               ; preds = %66
  %82 = load i32, i32* %11, align 4
  %83 = sext i32 %82 to i64
  %84 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %83
  %85 = load i8, i8* %84, align 1
  %86 = sext i8 %85 to i32
  %87 = icmp eq i32 %86, 83
  br i1 %87, label %88, label %96

88:                                               ; preds = %81
  %89 = va_arg i8** %7, i32
  store i32 %89, i32* %15, align 4
  %90 = load i32, i32* %15, align 4
  %91 = trunc i32 %90 to i16
  %92 = load i32, i32* %11, align 4
  %93 = sext i32 %92 to i64
  %94 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %93
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %91, i16* %95, align 8
  br label %173

96:                                               ; preds = %81
  %97 = load i32, i32* %11, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %98
  %100 = load i8, i8* %99, align 1
  %101 = sext i8 %100 to i32
  %102 = icmp eq i32 %101, 73
  br i1 %102, label %103, label %110

103:                                              ; preds = %96
  %104 = va_arg i8** %7, i32
  store i32 %104, i32* %16, align 4
  %105 = load i32, i32* %16, align 4
  %106 = load i32, i32* %11, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %107
  %109 = bitcast %union.jvalue* %108 to i32*
  store i32 %105, i32* %109, align 8
  br label %172

110:                                              ; preds = %96
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %112
  %114 = load i8, i8* %113, align 1
  %115 = sext i8 %114 to i32
  %116 = icmp eq i32 %115, 74
  br i1 %116, label %117, label %124

117:                                              ; preds = %110
  %118 = va_arg i8** %7, i64
  store i64 %118, i64* %17, align 8
  %119 = load i64, i64* %17, align 8
  %120 = load i32, i32* %11, align 4
  %121 = sext i32 %120 to i64
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %119, i64* %123, align 8
  br label %171

124:                                              ; preds = %110
  %125 = load i32, i32* %11, align 4
  %126 = sext i32 %125 to i64
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %126
  %128 = load i8, i8* %127, align 1
  %129 = sext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %139

131:                                              ; preds = %124
  %132 = va_arg i8** %7, double
  store double %132, double* %18, align 8
  %133 = load double, double* %18, align 8
  %134 = fptrunc double %133 to float
  %135 = load i32, i32* %11, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %136
  %138 = bitcast %union.jvalue* %137 to float*
  store float %134, float* %138, align 8
  br label %170

139:                                              ; preds = %124
  %140 = load i32, i32* %11, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %141
  %143 = load i8, i8* %142, align 1
  %144 = sext i8 %143 to i32
  %145 = icmp eq i32 %144, 68
  br i1 %145, label %146, label %153

146:                                              ; preds = %139
  %147 = va_arg i8** %7, double
  store double %147, double* %19, align 8
  %148 = load double, double* %19, align 8
  %149 = load i32, i32* %11, align 4
  %150 = sext i32 %149 to i64
  %151 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %150
  %152 = bitcast %union.jvalue* %151 to double*
  store double %148, double* %152, align 8
  br label %169

153:                                              ; preds = %139
  %154 = load i32, i32* %11, align 4
  %155 = sext i32 %154 to i64
  %156 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %155
  %157 = load i8, i8* %156, align 1
  %158 = sext i8 %157 to i32
  %159 = icmp eq i32 %158, 76
  br i1 %159, label %160, label %168

160:                                              ; preds = %153
  %161 = va_arg i8** %7, i8*
  store i8* %161, i8** %20, align 8
  %162 = load i8*, i8** %20, align 8
  %163 = bitcast i8* %162 to %struct._jobject*
  %164 = load i32, i32* %11, align 4
  %165 = sext i32 %164 to i64
  %166 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %165
  %167 = bitcast %union.jvalue* %166 to %struct._jobject**
  store %struct._jobject* %163, %struct._jobject** %167, align 8
  br label %168

168:                                              ; preds = %160, %153
  br label %169

169:                                              ; preds = %168, %146
  br label %170

170:                                              ; preds = %169, %131
  br label %171

171:                                              ; preds = %170, %117
  br label %172

172:                                              ; preds = %171, %103
  br label %173

173:                                              ; preds = %172, %88
  br label %174

174:                                              ; preds = %173, %73
  br label %175

175:                                              ; preds = %174, %58
  br label %176

176:                                              ; preds = %175, %43
  br label %177

177:                                              ; preds = %176
  %178 = load i32, i32* %11, align 4
  %179 = add nsw i32 %178, 1
  store i32 %179, i32* %11, align 4
  br label %32, !llvm.loop !46

180:                                              ; preds = %32
  %181 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %182 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %181, align 8
  %183 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %182, i32 0, i32 54
  %184 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %183, align 8
  %185 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %186 = load %struct._jobject*, %struct._jobject** %5, align 8
  %187 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %188 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  %189 = call i64 %184(%struct.JNINativeInterface_** noundef %185, %struct._jobject* noundef %186, %struct._jmethodID* noundef %187, %union.jvalue* noundef %188)
  store i64 %189, i64* %21, align 8
  %190 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %190)
  %191 = load i64, i64* %21, align 8
  ret i64 %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define i64 @JNI_CallLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !47

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 54
  %183 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  %188 = call i64 %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret i64 %188
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define i64 @JNI_CallNonvirtualLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i8*, align 8
  %10 = alloca [256 x i8], align 1
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i32, align 4
  %19 = alloca i64, align 8
  %20 = alloca double, align 8
  %21 = alloca double, align 8
  %22 = alloca i8*, align 8
  %23 = alloca i64, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %24 = bitcast i8** %9 to i8*
  call void @llvm.va_start(i8* %24)
  %25 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %26 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %25, align 8
  %27 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %26, i32 0, i32 0
  %28 = load i8*, i8** %27, align 8
  %29 = bitcast i8* %28 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %30 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %31 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 0
  %33 = call i32 %29(%struct.JNINativeInterface_** noundef %30, %struct._jmethodID* noundef %31, i8* noundef %32)
  store i32 %33, i32* %12, align 4
  store i32 0, i32* %13, align 4
  br label %34

34:                                               ; preds = %179, %4
  %35 = load i32, i32* %13, align 4
  %36 = load i32, i32* %12, align 4
  %37 = icmp slt i32 %35, %36
  br i1 %37, label %38, label %182

38:                                               ; preds = %34
  %39 = load i32, i32* %13, align 4
  %40 = sext i32 %39 to i64
  %41 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %40
  %42 = load i8, i8* %41, align 1
  %43 = sext i8 %42 to i32
  %44 = icmp eq i32 %43, 90
  br i1 %44, label %45, label %53

45:                                               ; preds = %38
  %46 = va_arg i8** %9, i32
  store i32 %46, i32* %14, align 4
  %47 = load i32, i32* %14, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %13, align 4
  %50 = sext i32 %49 to i64
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %48, i8* %52, align 8
  br label %178

53:                                               ; preds = %38
  %54 = load i32, i32* %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %55
  %57 = load i8, i8* %56, align 1
  %58 = sext i8 %57 to i32
  %59 = icmp eq i32 %58, 66
  br i1 %59, label %60, label %68

60:                                               ; preds = %53
  %61 = va_arg i8** %9, i32
  store i32 %61, i32* %15, align 4
  %62 = load i32, i32* %15, align 4
  %63 = trunc i32 %62 to i8
  %64 = load i32, i32* %13, align 4
  %65 = sext i32 %64 to i64
  %66 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %65
  %67 = bitcast %union.jvalue* %66 to i8*
  store i8 %63, i8* %67, align 8
  br label %177

68:                                               ; preds = %53
  %69 = load i32, i32* %13, align 4
  %70 = sext i32 %69 to i64
  %71 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %70
  %72 = load i8, i8* %71, align 1
  %73 = sext i8 %72 to i32
  %74 = icmp eq i32 %73, 67
  br i1 %74, label %75, label %83

75:                                               ; preds = %68
  %76 = va_arg i8** %9, i32
  store i32 %76, i32* %16, align 4
  %77 = load i32, i32* %16, align 4
  %78 = trunc i32 %77 to i16
  %79 = load i32, i32* %13, align 4
  %80 = sext i32 %79 to i64
  %81 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %80
  %82 = bitcast %union.jvalue* %81 to i16*
  store i16 %78, i16* %82, align 8
  br label %176

83:                                               ; preds = %68
  %84 = load i32, i32* %13, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %85
  %87 = load i8, i8* %86, align 1
  %88 = sext i8 %87 to i32
  %89 = icmp eq i32 %88, 83
  br i1 %89, label %90, label %98

90:                                               ; preds = %83
  %91 = va_arg i8** %9, i32
  store i32 %91, i32* %17, align 4
  %92 = load i32, i32* %17, align 4
  %93 = trunc i32 %92 to i16
  %94 = load i32, i32* %13, align 4
  %95 = sext i32 %94 to i64
  %96 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %95
  %97 = bitcast %union.jvalue* %96 to i16*
  store i16 %93, i16* %97, align 8
  br label %175

98:                                               ; preds = %83
  %99 = load i32, i32* %13, align 4
  %100 = sext i32 %99 to i64
  %101 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %100
  %102 = load i8, i8* %101, align 1
  %103 = sext i8 %102 to i32
  %104 = icmp eq i32 %103, 73
  br i1 %104, label %105, label %112

105:                                              ; preds = %98
  %106 = va_arg i8** %9, i32
  store i32 %106, i32* %18, align 4
  %107 = load i32, i32* %18, align 4
  %108 = load i32, i32* %13, align 4
  %109 = sext i32 %108 to i64
  %110 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %109
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %107, i32* %111, align 8
  br label %174

112:                                              ; preds = %98
  %113 = load i32, i32* %13, align 4
  %114 = sext i32 %113 to i64
  %115 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %114
  %116 = load i8, i8* %115, align 1
  %117 = sext i8 %116 to i32
  %118 = icmp eq i32 %117, 74
  br i1 %118, label %119, label %126

119:                                              ; preds = %112
  %120 = va_arg i8** %9, i64
  store i64 %120, i64* %19, align 8
  %121 = load i64, i64* %19, align 8
  %122 = load i32, i32* %13, align 4
  %123 = sext i32 %122 to i64
  %124 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %123
  %125 = bitcast %union.jvalue* %124 to i64*
  store i64 %121, i64* %125, align 8
  br label %173

126:                                              ; preds = %112
  %127 = load i32, i32* %13, align 4
  %128 = sext i32 %127 to i64
  %129 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %128
  %130 = load i8, i8* %129, align 1
  %131 = sext i8 %130 to i32
  %132 = icmp eq i32 %131, 70
  br i1 %132, label %133, label %141

133:                                              ; preds = %126
  %134 = va_arg i8** %9, double
  store double %134, double* %20, align 8
  %135 = load double, double* %20, align 8
  %136 = fptrunc double %135 to float
  %137 = load i32, i32* %13, align 4
  %138 = sext i32 %137 to i64
  %139 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %138
  %140 = bitcast %union.jvalue* %139 to float*
  store float %136, float* %140, align 8
  br label %172

141:                                              ; preds = %126
  %142 = load i32, i32* %13, align 4
  %143 = sext i32 %142 to i64
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %143
  %145 = load i8, i8* %144, align 1
  %146 = sext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %155

148:                                              ; preds = %141
  %149 = va_arg i8** %9, double
  store double %149, double* %21, align 8
  %150 = load double, double* %21, align 8
  %151 = load i32, i32* %13, align 4
  %152 = sext i32 %151 to i64
  %153 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %152
  %154 = bitcast %union.jvalue* %153 to double*
  store double %150, double* %154, align 8
  br label %171

155:                                              ; preds = %141
  %156 = load i32, i32* %13, align 4
  %157 = sext i32 %156 to i64
  %158 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %157
  %159 = load i8, i8* %158, align 1
  %160 = sext i8 %159 to i32
  %161 = icmp eq i32 %160, 76
  br i1 %161, label %162, label %170

162:                                              ; preds = %155
  %163 = va_arg i8** %9, i8*
  store i8* %163, i8** %22, align 8
  %164 = load i8*, i8** %22, align 8
  %165 = bitcast i8* %164 to %struct._jobject*
  %166 = load i32, i32* %13, align 4
  %167 = sext i32 %166 to i64
  %168 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %167
  %169 = bitcast %union.jvalue* %168 to %struct._jobject**
  store %struct._jobject* %165, %struct._jobject** %169, align 8
  br label %170

170:                                              ; preds = %162, %155
  br label %171

171:                                              ; preds = %170, %148
  br label %172

172:                                              ; preds = %171, %133
  br label %173

173:                                              ; preds = %172, %119
  br label %174

174:                                              ; preds = %173, %105
  br label %175

175:                                              ; preds = %174, %90
  br label %176

176:                                              ; preds = %175, %75
  br label %177

177:                                              ; preds = %176, %60
  br label %178

178:                                              ; preds = %177, %45
  br label %179

179:                                              ; preds = %178
  %180 = load i32, i32* %13, align 4
  %181 = add nsw i32 %180, 1
  store i32 %181, i32* %13, align 4
  br label %34, !llvm.loop !48

182:                                              ; preds = %34
  %183 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %184 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %183, align 8
  %185 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %184, i32 0, i32 84
  %186 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %185, align 8
  %187 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %188 = load %struct._jobject*, %struct._jobject** %6, align 8
  %189 = load %struct._jobject*, %struct._jobject** %7, align 8
  %190 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %191 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 0
  %192 = call i64 %186(%struct.JNINativeInterface_** noundef %187, %struct._jobject* noundef %188, %struct._jobject* noundef %189, %struct._jmethodID* noundef %190, %union.jvalue* noundef %191)
  store i64 %192, i64* %23, align 8
  %193 = bitcast i8** %9 to i8*
  call void @llvm.va_end(i8* %193)
  %194 = load i64, i64* %23, align 8
  ret i64 %194
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define i64 @JNI_CallNonvirtualLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, i8* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca i8*, align 8
  %11 = alloca [256 x i8], align 1
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i32, align 4
  %19 = alloca i32, align 4
  %20 = alloca i64, align 8
  %21 = alloca double, align 8
  %22 = alloca double, align 8
  %23 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  store i8* %4, i8** %10, align 8
  %24 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %25 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %25, i32 0, i32 0
  %27 = load i8*, i8** %26, align 8
  %28 = bitcast i8* %27 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %29 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %30 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 0
  %32 = call i32 %28(%struct.JNINativeInterface_** noundef %29, %struct._jmethodID* noundef %30, i8* noundef %31)
  store i32 %32, i32* %13, align 4
  store i32 0, i32* %14, align 4
  br label %33

33:                                               ; preds = %178, %5
  %34 = load i32, i32* %14, align 4
  %35 = load i32, i32* %13, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %181

37:                                               ; preds = %33
  %38 = load i32, i32* %14, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %39
  %41 = load i8, i8* %40, align 1
  %42 = sext i8 %41 to i32
  %43 = icmp eq i32 %42, 90
  br i1 %43, label %44, label %52

44:                                               ; preds = %37
  %45 = va_arg i8** %10, i32
  store i32 %45, i32* %15, align 4
  %46 = load i32, i32* %15, align 4
  %47 = trunc i32 %46 to i8
  %48 = load i32, i32* %14, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %47, i8* %51, align 8
  br label %177

52:                                               ; preds = %37
  %53 = load i32, i32* %14, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %54
  %56 = load i8, i8* %55, align 1
  %57 = sext i8 %56 to i32
  %58 = icmp eq i32 %57, 66
  br i1 %58, label %59, label %67

59:                                               ; preds = %52
  %60 = va_arg i8** %10, i32
  store i32 %60, i32* %16, align 4
  %61 = load i32, i32* %16, align 4
  %62 = trunc i32 %61 to i8
  %63 = load i32, i32* %14, align 4
  %64 = sext i32 %63 to i64
  %65 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %64
  %66 = bitcast %union.jvalue* %65 to i8*
  store i8 %62, i8* %66, align 8
  br label %176

67:                                               ; preds = %52
  %68 = load i32, i32* %14, align 4
  %69 = sext i32 %68 to i64
  %70 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %69
  %71 = load i8, i8* %70, align 1
  %72 = sext i8 %71 to i32
  %73 = icmp eq i32 %72, 67
  br i1 %73, label %74, label %82

74:                                               ; preds = %67
  %75 = va_arg i8** %10, i32
  store i32 %75, i32* %17, align 4
  %76 = load i32, i32* %17, align 4
  %77 = trunc i32 %76 to i16
  %78 = load i32, i32* %14, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %175

82:                                               ; preds = %67
  %83 = load i32, i32* %14, align 4
  %84 = sext i32 %83 to i64
  %85 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %84
  %86 = load i8, i8* %85, align 1
  %87 = sext i8 %86 to i32
  %88 = icmp eq i32 %87, 83
  br i1 %88, label %89, label %97

89:                                               ; preds = %82
  %90 = va_arg i8** %10, i32
  store i32 %90, i32* %18, align 4
  %91 = load i32, i32* %18, align 4
  %92 = trunc i32 %91 to i16
  %93 = load i32, i32* %14, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %94
  %96 = bitcast %union.jvalue* %95 to i16*
  store i16 %92, i16* %96, align 8
  br label %174

97:                                               ; preds = %82
  %98 = load i32, i32* %14, align 4
  %99 = sext i32 %98 to i64
  %100 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %99
  %101 = load i8, i8* %100, align 1
  %102 = sext i8 %101 to i32
  %103 = icmp eq i32 %102, 73
  br i1 %103, label %104, label %111

104:                                              ; preds = %97
  %105 = va_arg i8** %10, i32
  store i32 %105, i32* %19, align 4
  %106 = load i32, i32* %19, align 4
  %107 = load i32, i32* %14, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %108
  %110 = bitcast %union.jvalue* %109 to i32*
  store i32 %106, i32* %110, align 8
  br label %173

111:                                              ; preds = %97
  %112 = load i32, i32* %14, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %113
  %115 = load i8, i8* %114, align 1
  %116 = sext i8 %115 to i32
  %117 = icmp eq i32 %116, 74
  br i1 %117, label %118, label %125

118:                                              ; preds = %111
  %119 = va_arg i8** %10, i64
  store i64 %119, i64* %20, align 8
  %120 = load i64, i64* %20, align 8
  %121 = load i32, i32* %14, align 4
  %122 = sext i32 %121 to i64
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %120, i64* %124, align 8
  br label %172

125:                                              ; preds = %111
  %126 = load i32, i32* %14, align 4
  %127 = sext i32 %126 to i64
  %128 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %127
  %129 = load i8, i8* %128, align 1
  %130 = sext i8 %129 to i32
  %131 = icmp eq i32 %130, 70
  br i1 %131, label %132, label %140

132:                                              ; preds = %125
  %133 = va_arg i8** %10, double
  store double %133, double* %21, align 8
  %134 = load double, double* %21, align 8
  %135 = fptrunc double %134 to float
  %136 = load i32, i32* %14, align 4
  %137 = sext i32 %136 to i64
  %138 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %137
  %139 = bitcast %union.jvalue* %138 to float*
  store float %135, float* %139, align 8
  br label %171

140:                                              ; preds = %125
  %141 = load i32, i32* %14, align 4
  %142 = sext i32 %141 to i64
  %143 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %142
  %144 = load i8, i8* %143, align 1
  %145 = sext i8 %144 to i32
  %146 = icmp eq i32 %145, 68
  br i1 %146, label %147, label %154

147:                                              ; preds = %140
  %148 = va_arg i8** %10, double
  store double %148, double* %22, align 8
  %149 = load double, double* %22, align 8
  %150 = load i32, i32* %14, align 4
  %151 = sext i32 %150 to i64
  %152 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %151
  %153 = bitcast %union.jvalue* %152 to double*
  store double %149, double* %153, align 8
  br label %170

154:                                              ; preds = %140
  %155 = load i32, i32* %14, align 4
  %156 = sext i32 %155 to i64
  %157 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %156
  %158 = load i8, i8* %157, align 1
  %159 = sext i8 %158 to i32
  %160 = icmp eq i32 %159, 76
  br i1 %160, label %161, label %169

161:                                              ; preds = %154
  %162 = va_arg i8** %10, i8*
  store i8* %162, i8** %23, align 8
  %163 = load i8*, i8** %23, align 8
  %164 = bitcast i8* %163 to %struct._jobject*
  %165 = load i32, i32* %14, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %166
  %168 = bitcast %union.jvalue* %167 to %struct._jobject**
  store %struct._jobject* %164, %struct._jobject** %168, align 8
  br label %169

169:                                              ; preds = %161, %154
  br label %170

170:                                              ; preds = %169, %147
  br label %171

171:                                              ; preds = %170, %132
  br label %172

172:                                              ; preds = %171, %118
  br label %173

173:                                              ; preds = %172, %104
  br label %174

174:                                              ; preds = %173, %89
  br label %175

175:                                              ; preds = %174, %74
  br label %176

176:                                              ; preds = %175, %59
  br label %177

177:                                              ; preds = %176, %44
  br label %178

178:                                              ; preds = %177
  %179 = load i32, i32* %14, align 4
  %180 = add nsw i32 %179, 1
  store i32 %180, i32* %14, align 4
  br label %33, !llvm.loop !49

181:                                              ; preds = %33
  %182 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %183 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %182, align 8
  %184 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %183, i32 0, i32 84
  %185 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %184, align 8
  %186 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %187 = load %struct._jobject*, %struct._jobject** %7, align 8
  %188 = load %struct._jobject*, %struct._jobject** %8, align 8
  %189 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %190 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 0
  %191 = call i64 %185(%struct.JNINativeInterface_** noundef %186, %struct._jobject* noundef %187, %struct._jobject* noundef %188, %struct._jmethodID* noundef %189, %union.jvalue* noundef %190)
  ret i64 %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define i64 @JNI_CallStaticLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  %21 = alloca i64, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %22 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %22)
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %24 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %23, align 8
  %25 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %24, i32 0, i32 0
  %26 = load i8*, i8** %25, align 8
  %27 = bitcast i8* %26 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %28 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %29 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %31 = call i32 %27(%struct.JNINativeInterface_** noundef %28, %struct._jmethodID* noundef %29, i8* noundef %30)
  store i32 %31, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %32

32:                                               ; preds = %177, %3
  %33 = load i32, i32* %11, align 4
  %34 = load i32, i32* %10, align 4
  %35 = icmp slt i32 %33, %34
  br i1 %35, label %36, label %180

36:                                               ; preds = %32
  %37 = load i32, i32* %11, align 4
  %38 = sext i32 %37 to i64
  %39 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %38
  %40 = load i8, i8* %39, align 1
  %41 = sext i8 %40 to i32
  %42 = icmp eq i32 %41, 90
  br i1 %42, label %43, label %51

43:                                               ; preds = %36
  %44 = va_arg i8** %7, i32
  store i32 %44, i32* %12, align 4
  %45 = load i32, i32* %12, align 4
  %46 = trunc i32 %45 to i8
  %47 = load i32, i32* %11, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %176

51:                                               ; preds = %36
  %52 = load i32, i32* %11, align 4
  %53 = sext i32 %52 to i64
  %54 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %53
  %55 = load i8, i8* %54, align 1
  %56 = sext i8 %55 to i32
  %57 = icmp eq i32 %56, 66
  br i1 %57, label %58, label %66

58:                                               ; preds = %51
  %59 = va_arg i8** %7, i32
  store i32 %59, i32* %13, align 4
  %60 = load i32, i32* %13, align 4
  %61 = trunc i32 %60 to i8
  %62 = load i32, i32* %11, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %63
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %61, i8* %65, align 8
  br label %175

66:                                               ; preds = %51
  %67 = load i32, i32* %11, align 4
  %68 = sext i32 %67 to i64
  %69 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %68
  %70 = load i8, i8* %69, align 1
  %71 = sext i8 %70 to i32
  %72 = icmp eq i32 %71, 67
  br i1 %72, label %73, label %81

73:                                               ; preds = %66
  %74 = va_arg i8** %7, i32
  store i32 %74, i32* %14, align 4
  %75 = load i32, i32* %14, align 4
  %76 = trunc i32 %75 to i16
  %77 = load i32, i32* %11, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %174

81:                                               ; preds = %66
  %82 = load i32, i32* %11, align 4
  %83 = sext i32 %82 to i64
  %84 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %83
  %85 = load i8, i8* %84, align 1
  %86 = sext i8 %85 to i32
  %87 = icmp eq i32 %86, 83
  br i1 %87, label %88, label %96

88:                                               ; preds = %81
  %89 = va_arg i8** %7, i32
  store i32 %89, i32* %15, align 4
  %90 = load i32, i32* %15, align 4
  %91 = trunc i32 %90 to i16
  %92 = load i32, i32* %11, align 4
  %93 = sext i32 %92 to i64
  %94 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %93
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %91, i16* %95, align 8
  br label %173

96:                                               ; preds = %81
  %97 = load i32, i32* %11, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %98
  %100 = load i8, i8* %99, align 1
  %101 = sext i8 %100 to i32
  %102 = icmp eq i32 %101, 73
  br i1 %102, label %103, label %110

103:                                              ; preds = %96
  %104 = va_arg i8** %7, i32
  store i32 %104, i32* %16, align 4
  %105 = load i32, i32* %16, align 4
  %106 = load i32, i32* %11, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %107
  %109 = bitcast %union.jvalue* %108 to i32*
  store i32 %105, i32* %109, align 8
  br label %172

110:                                              ; preds = %96
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %112
  %114 = load i8, i8* %113, align 1
  %115 = sext i8 %114 to i32
  %116 = icmp eq i32 %115, 74
  br i1 %116, label %117, label %124

117:                                              ; preds = %110
  %118 = va_arg i8** %7, i64
  store i64 %118, i64* %17, align 8
  %119 = load i64, i64* %17, align 8
  %120 = load i32, i32* %11, align 4
  %121 = sext i32 %120 to i64
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %119, i64* %123, align 8
  br label %171

124:                                              ; preds = %110
  %125 = load i32, i32* %11, align 4
  %126 = sext i32 %125 to i64
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %126
  %128 = load i8, i8* %127, align 1
  %129 = sext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %139

131:                                              ; preds = %124
  %132 = va_arg i8** %7, double
  store double %132, double* %18, align 8
  %133 = load double, double* %18, align 8
  %134 = fptrunc double %133 to float
  %135 = load i32, i32* %11, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %136
  %138 = bitcast %union.jvalue* %137 to float*
  store float %134, float* %138, align 8
  br label %170

139:                                              ; preds = %124
  %140 = load i32, i32* %11, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %141
  %143 = load i8, i8* %142, align 1
  %144 = sext i8 %143 to i32
  %145 = icmp eq i32 %144, 68
  br i1 %145, label %146, label %153

146:                                              ; preds = %139
  %147 = va_arg i8** %7, double
  store double %147, double* %19, align 8
  %148 = load double, double* %19, align 8
  %149 = load i32, i32* %11, align 4
  %150 = sext i32 %149 to i64
  %151 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %150
  %152 = bitcast %union.jvalue* %151 to double*
  store double %148, double* %152, align 8
  br label %169

153:                                              ; preds = %139
  %154 = load i32, i32* %11, align 4
  %155 = sext i32 %154 to i64
  %156 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %155
  %157 = load i8, i8* %156, align 1
  %158 = sext i8 %157 to i32
  %159 = icmp eq i32 %158, 76
  br i1 %159, label %160, label %168

160:                                              ; preds = %153
  %161 = va_arg i8** %7, i8*
  store i8* %161, i8** %20, align 8
  %162 = load i8*, i8** %20, align 8
  %163 = bitcast i8* %162 to %struct._jobject*
  %164 = load i32, i32* %11, align 4
  %165 = sext i32 %164 to i64
  %166 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %165
  %167 = bitcast %union.jvalue* %166 to %struct._jobject**
  store %struct._jobject* %163, %struct._jobject** %167, align 8
  br label %168

168:                                              ; preds = %160, %153
  br label %169

169:                                              ; preds = %168, %146
  br label %170

170:                                              ; preds = %169, %131
  br label %171

171:                                              ; preds = %170, %117
  br label %172

172:                                              ; preds = %171, %103
  br label %173

173:                                              ; preds = %172, %88
  br label %174

174:                                              ; preds = %173, %73
  br label %175

175:                                              ; preds = %174, %58
  br label %176

176:                                              ; preds = %175, %43
  br label %177

177:                                              ; preds = %176
  %178 = load i32, i32* %11, align 4
  %179 = add nsw i32 %178, 1
  store i32 %179, i32* %11, align 4
  br label %32, !llvm.loop !50

180:                                              ; preds = %32
  %181 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %182 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %181, align 8
  %183 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %182, i32 0, i32 134
  %184 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %183, align 8
  %185 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %186 = load %struct._jobject*, %struct._jobject** %5, align 8
  %187 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %188 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  %189 = call i64 %184(%struct.JNINativeInterface_** noundef %185, %struct._jobject* noundef %186, %struct._jmethodID* noundef %187, %union.jvalue* noundef %188)
  store i64 %189, i64* %21, align 8
  %190 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %190)
  %191 = load i64, i64* %21, align 8
  ret i64 %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define i64 @JNI_CallStaticLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !51

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 134
  %183 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  %188 = call i64 %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret i64 %188
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define float @JNI_CallFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  %21 = alloca float, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %22 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %22)
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %24 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %23, align 8
  %25 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %24, i32 0, i32 0
  %26 = load i8*, i8** %25, align 8
  %27 = bitcast i8* %26 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %28 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %29 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %31 = call i32 %27(%struct.JNINativeInterface_** noundef %28, %struct._jmethodID* noundef %29, i8* noundef %30)
  store i32 %31, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %32

32:                                               ; preds = %177, %3
  %33 = load i32, i32* %11, align 4
  %34 = load i32, i32* %10, align 4
  %35 = icmp slt i32 %33, %34
  br i1 %35, label %36, label %180

36:                                               ; preds = %32
  %37 = load i32, i32* %11, align 4
  %38 = sext i32 %37 to i64
  %39 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %38
  %40 = load i8, i8* %39, align 1
  %41 = sext i8 %40 to i32
  %42 = icmp eq i32 %41, 90
  br i1 %42, label %43, label %51

43:                                               ; preds = %36
  %44 = va_arg i8** %7, i32
  store i32 %44, i32* %12, align 4
  %45 = load i32, i32* %12, align 4
  %46 = trunc i32 %45 to i8
  %47 = load i32, i32* %11, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %176

51:                                               ; preds = %36
  %52 = load i32, i32* %11, align 4
  %53 = sext i32 %52 to i64
  %54 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %53
  %55 = load i8, i8* %54, align 1
  %56 = sext i8 %55 to i32
  %57 = icmp eq i32 %56, 66
  br i1 %57, label %58, label %66

58:                                               ; preds = %51
  %59 = va_arg i8** %7, i32
  store i32 %59, i32* %13, align 4
  %60 = load i32, i32* %13, align 4
  %61 = trunc i32 %60 to i8
  %62 = load i32, i32* %11, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %63
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %61, i8* %65, align 8
  br label %175

66:                                               ; preds = %51
  %67 = load i32, i32* %11, align 4
  %68 = sext i32 %67 to i64
  %69 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %68
  %70 = load i8, i8* %69, align 1
  %71 = sext i8 %70 to i32
  %72 = icmp eq i32 %71, 67
  br i1 %72, label %73, label %81

73:                                               ; preds = %66
  %74 = va_arg i8** %7, i32
  store i32 %74, i32* %14, align 4
  %75 = load i32, i32* %14, align 4
  %76 = trunc i32 %75 to i16
  %77 = load i32, i32* %11, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %174

81:                                               ; preds = %66
  %82 = load i32, i32* %11, align 4
  %83 = sext i32 %82 to i64
  %84 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %83
  %85 = load i8, i8* %84, align 1
  %86 = sext i8 %85 to i32
  %87 = icmp eq i32 %86, 83
  br i1 %87, label %88, label %96

88:                                               ; preds = %81
  %89 = va_arg i8** %7, i32
  store i32 %89, i32* %15, align 4
  %90 = load i32, i32* %15, align 4
  %91 = trunc i32 %90 to i16
  %92 = load i32, i32* %11, align 4
  %93 = sext i32 %92 to i64
  %94 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %93
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %91, i16* %95, align 8
  br label %173

96:                                               ; preds = %81
  %97 = load i32, i32* %11, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %98
  %100 = load i8, i8* %99, align 1
  %101 = sext i8 %100 to i32
  %102 = icmp eq i32 %101, 73
  br i1 %102, label %103, label %110

103:                                              ; preds = %96
  %104 = va_arg i8** %7, i32
  store i32 %104, i32* %16, align 4
  %105 = load i32, i32* %16, align 4
  %106 = load i32, i32* %11, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %107
  %109 = bitcast %union.jvalue* %108 to i32*
  store i32 %105, i32* %109, align 8
  br label %172

110:                                              ; preds = %96
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %112
  %114 = load i8, i8* %113, align 1
  %115 = sext i8 %114 to i32
  %116 = icmp eq i32 %115, 74
  br i1 %116, label %117, label %124

117:                                              ; preds = %110
  %118 = va_arg i8** %7, i64
  store i64 %118, i64* %17, align 8
  %119 = load i64, i64* %17, align 8
  %120 = load i32, i32* %11, align 4
  %121 = sext i32 %120 to i64
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %119, i64* %123, align 8
  br label %171

124:                                              ; preds = %110
  %125 = load i32, i32* %11, align 4
  %126 = sext i32 %125 to i64
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %126
  %128 = load i8, i8* %127, align 1
  %129 = sext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %139

131:                                              ; preds = %124
  %132 = va_arg i8** %7, double
  store double %132, double* %18, align 8
  %133 = load double, double* %18, align 8
  %134 = fptrunc double %133 to float
  %135 = load i32, i32* %11, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %136
  %138 = bitcast %union.jvalue* %137 to float*
  store float %134, float* %138, align 8
  br label %170

139:                                              ; preds = %124
  %140 = load i32, i32* %11, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %141
  %143 = load i8, i8* %142, align 1
  %144 = sext i8 %143 to i32
  %145 = icmp eq i32 %144, 68
  br i1 %145, label %146, label %153

146:                                              ; preds = %139
  %147 = va_arg i8** %7, double
  store double %147, double* %19, align 8
  %148 = load double, double* %19, align 8
  %149 = load i32, i32* %11, align 4
  %150 = sext i32 %149 to i64
  %151 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %150
  %152 = bitcast %union.jvalue* %151 to double*
  store double %148, double* %152, align 8
  br label %169

153:                                              ; preds = %139
  %154 = load i32, i32* %11, align 4
  %155 = sext i32 %154 to i64
  %156 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %155
  %157 = load i8, i8* %156, align 1
  %158 = sext i8 %157 to i32
  %159 = icmp eq i32 %158, 76
  br i1 %159, label %160, label %168

160:                                              ; preds = %153
  %161 = va_arg i8** %7, i8*
  store i8* %161, i8** %20, align 8
  %162 = load i8*, i8** %20, align 8
  %163 = bitcast i8* %162 to %struct._jobject*
  %164 = load i32, i32* %11, align 4
  %165 = sext i32 %164 to i64
  %166 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %165
  %167 = bitcast %union.jvalue* %166 to %struct._jobject**
  store %struct._jobject* %163, %struct._jobject** %167, align 8
  br label %168

168:                                              ; preds = %160, %153
  br label %169

169:                                              ; preds = %168, %146
  br label %170

170:                                              ; preds = %169, %131
  br label %171

171:                                              ; preds = %170, %117
  br label %172

172:                                              ; preds = %171, %103
  br label %173

173:                                              ; preds = %172, %88
  br label %174

174:                                              ; preds = %173, %73
  br label %175

175:                                              ; preds = %174, %58
  br label %176

176:                                              ; preds = %175, %43
  br label %177

177:                                              ; preds = %176
  %178 = load i32, i32* %11, align 4
  %179 = add nsw i32 %178, 1
  store i32 %179, i32* %11, align 4
  br label %32, !llvm.loop !52

180:                                              ; preds = %32
  %181 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %182 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %181, align 8
  %183 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %182, i32 0, i32 57
  %184 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %183, align 8
  %185 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %186 = load %struct._jobject*, %struct._jobject** %5, align 8
  %187 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %188 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  %189 = call float %184(%struct.JNINativeInterface_** noundef %185, %struct._jobject* noundef %186, %struct._jmethodID* noundef %187, %union.jvalue* noundef %188)
  store float %189, float* %21, align 4
  %190 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %190)
  %191 = load float, float* %21, align 4
  ret float %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define float @JNI_CallFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !53

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 57
  %183 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  %188 = call float %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret float %188
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define float @JNI_CallNonvirtualFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i8*, align 8
  %10 = alloca [256 x i8], align 1
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i32, align 4
  %19 = alloca i64, align 8
  %20 = alloca double, align 8
  %21 = alloca double, align 8
  %22 = alloca i8*, align 8
  %23 = alloca float, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %24 = bitcast i8** %9 to i8*
  call void @llvm.va_start(i8* %24)
  %25 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %26 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %25, align 8
  %27 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %26, i32 0, i32 0
  %28 = load i8*, i8** %27, align 8
  %29 = bitcast i8* %28 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %30 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %31 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 0
  %33 = call i32 %29(%struct.JNINativeInterface_** noundef %30, %struct._jmethodID* noundef %31, i8* noundef %32)
  store i32 %33, i32* %12, align 4
  store i32 0, i32* %13, align 4
  br label %34

34:                                               ; preds = %179, %4
  %35 = load i32, i32* %13, align 4
  %36 = load i32, i32* %12, align 4
  %37 = icmp slt i32 %35, %36
  br i1 %37, label %38, label %182

38:                                               ; preds = %34
  %39 = load i32, i32* %13, align 4
  %40 = sext i32 %39 to i64
  %41 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %40
  %42 = load i8, i8* %41, align 1
  %43 = sext i8 %42 to i32
  %44 = icmp eq i32 %43, 90
  br i1 %44, label %45, label %53

45:                                               ; preds = %38
  %46 = va_arg i8** %9, i32
  store i32 %46, i32* %14, align 4
  %47 = load i32, i32* %14, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %13, align 4
  %50 = sext i32 %49 to i64
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %48, i8* %52, align 8
  br label %178

53:                                               ; preds = %38
  %54 = load i32, i32* %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %55
  %57 = load i8, i8* %56, align 1
  %58 = sext i8 %57 to i32
  %59 = icmp eq i32 %58, 66
  br i1 %59, label %60, label %68

60:                                               ; preds = %53
  %61 = va_arg i8** %9, i32
  store i32 %61, i32* %15, align 4
  %62 = load i32, i32* %15, align 4
  %63 = trunc i32 %62 to i8
  %64 = load i32, i32* %13, align 4
  %65 = sext i32 %64 to i64
  %66 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %65
  %67 = bitcast %union.jvalue* %66 to i8*
  store i8 %63, i8* %67, align 8
  br label %177

68:                                               ; preds = %53
  %69 = load i32, i32* %13, align 4
  %70 = sext i32 %69 to i64
  %71 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %70
  %72 = load i8, i8* %71, align 1
  %73 = sext i8 %72 to i32
  %74 = icmp eq i32 %73, 67
  br i1 %74, label %75, label %83

75:                                               ; preds = %68
  %76 = va_arg i8** %9, i32
  store i32 %76, i32* %16, align 4
  %77 = load i32, i32* %16, align 4
  %78 = trunc i32 %77 to i16
  %79 = load i32, i32* %13, align 4
  %80 = sext i32 %79 to i64
  %81 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %80
  %82 = bitcast %union.jvalue* %81 to i16*
  store i16 %78, i16* %82, align 8
  br label %176

83:                                               ; preds = %68
  %84 = load i32, i32* %13, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %85
  %87 = load i8, i8* %86, align 1
  %88 = sext i8 %87 to i32
  %89 = icmp eq i32 %88, 83
  br i1 %89, label %90, label %98

90:                                               ; preds = %83
  %91 = va_arg i8** %9, i32
  store i32 %91, i32* %17, align 4
  %92 = load i32, i32* %17, align 4
  %93 = trunc i32 %92 to i16
  %94 = load i32, i32* %13, align 4
  %95 = sext i32 %94 to i64
  %96 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %95
  %97 = bitcast %union.jvalue* %96 to i16*
  store i16 %93, i16* %97, align 8
  br label %175

98:                                               ; preds = %83
  %99 = load i32, i32* %13, align 4
  %100 = sext i32 %99 to i64
  %101 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %100
  %102 = load i8, i8* %101, align 1
  %103 = sext i8 %102 to i32
  %104 = icmp eq i32 %103, 73
  br i1 %104, label %105, label %112

105:                                              ; preds = %98
  %106 = va_arg i8** %9, i32
  store i32 %106, i32* %18, align 4
  %107 = load i32, i32* %18, align 4
  %108 = load i32, i32* %13, align 4
  %109 = sext i32 %108 to i64
  %110 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %109
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %107, i32* %111, align 8
  br label %174

112:                                              ; preds = %98
  %113 = load i32, i32* %13, align 4
  %114 = sext i32 %113 to i64
  %115 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %114
  %116 = load i8, i8* %115, align 1
  %117 = sext i8 %116 to i32
  %118 = icmp eq i32 %117, 74
  br i1 %118, label %119, label %126

119:                                              ; preds = %112
  %120 = va_arg i8** %9, i64
  store i64 %120, i64* %19, align 8
  %121 = load i64, i64* %19, align 8
  %122 = load i32, i32* %13, align 4
  %123 = sext i32 %122 to i64
  %124 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %123
  %125 = bitcast %union.jvalue* %124 to i64*
  store i64 %121, i64* %125, align 8
  br label %173

126:                                              ; preds = %112
  %127 = load i32, i32* %13, align 4
  %128 = sext i32 %127 to i64
  %129 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %128
  %130 = load i8, i8* %129, align 1
  %131 = sext i8 %130 to i32
  %132 = icmp eq i32 %131, 70
  br i1 %132, label %133, label %141

133:                                              ; preds = %126
  %134 = va_arg i8** %9, double
  store double %134, double* %20, align 8
  %135 = load double, double* %20, align 8
  %136 = fptrunc double %135 to float
  %137 = load i32, i32* %13, align 4
  %138 = sext i32 %137 to i64
  %139 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %138
  %140 = bitcast %union.jvalue* %139 to float*
  store float %136, float* %140, align 8
  br label %172

141:                                              ; preds = %126
  %142 = load i32, i32* %13, align 4
  %143 = sext i32 %142 to i64
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %143
  %145 = load i8, i8* %144, align 1
  %146 = sext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %155

148:                                              ; preds = %141
  %149 = va_arg i8** %9, double
  store double %149, double* %21, align 8
  %150 = load double, double* %21, align 8
  %151 = load i32, i32* %13, align 4
  %152 = sext i32 %151 to i64
  %153 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %152
  %154 = bitcast %union.jvalue* %153 to double*
  store double %150, double* %154, align 8
  br label %171

155:                                              ; preds = %141
  %156 = load i32, i32* %13, align 4
  %157 = sext i32 %156 to i64
  %158 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %157
  %159 = load i8, i8* %158, align 1
  %160 = sext i8 %159 to i32
  %161 = icmp eq i32 %160, 76
  br i1 %161, label %162, label %170

162:                                              ; preds = %155
  %163 = va_arg i8** %9, i8*
  store i8* %163, i8** %22, align 8
  %164 = load i8*, i8** %22, align 8
  %165 = bitcast i8* %164 to %struct._jobject*
  %166 = load i32, i32* %13, align 4
  %167 = sext i32 %166 to i64
  %168 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %167
  %169 = bitcast %union.jvalue* %168 to %struct._jobject**
  store %struct._jobject* %165, %struct._jobject** %169, align 8
  br label %170

170:                                              ; preds = %162, %155
  br label %171

171:                                              ; preds = %170, %148
  br label %172

172:                                              ; preds = %171, %133
  br label %173

173:                                              ; preds = %172, %119
  br label %174

174:                                              ; preds = %173, %105
  br label %175

175:                                              ; preds = %174, %90
  br label %176

176:                                              ; preds = %175, %75
  br label %177

177:                                              ; preds = %176, %60
  br label %178

178:                                              ; preds = %177, %45
  br label %179

179:                                              ; preds = %178
  %180 = load i32, i32* %13, align 4
  %181 = add nsw i32 %180, 1
  store i32 %181, i32* %13, align 4
  br label %34, !llvm.loop !54

182:                                              ; preds = %34
  %183 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %184 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %183, align 8
  %185 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %184, i32 0, i32 87
  %186 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %185, align 8
  %187 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %188 = load %struct._jobject*, %struct._jobject** %6, align 8
  %189 = load %struct._jobject*, %struct._jobject** %7, align 8
  %190 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %191 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 0
  %192 = call float %186(%struct.JNINativeInterface_** noundef %187, %struct._jobject* noundef %188, %struct._jobject* noundef %189, %struct._jmethodID* noundef %190, %union.jvalue* noundef %191)
  store float %192, float* %23, align 4
  %193 = bitcast i8** %9 to i8*
  call void @llvm.va_end(i8* %193)
  %194 = load float, float* %23, align 4
  ret float %194
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define float @JNI_CallNonvirtualFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, i8* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca i8*, align 8
  %11 = alloca [256 x i8], align 1
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i32, align 4
  %19 = alloca i32, align 4
  %20 = alloca i64, align 8
  %21 = alloca double, align 8
  %22 = alloca double, align 8
  %23 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  store i8* %4, i8** %10, align 8
  %24 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %25 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %25, i32 0, i32 0
  %27 = load i8*, i8** %26, align 8
  %28 = bitcast i8* %27 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %29 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %30 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 0
  %32 = call i32 %28(%struct.JNINativeInterface_** noundef %29, %struct._jmethodID* noundef %30, i8* noundef %31)
  store i32 %32, i32* %13, align 4
  store i32 0, i32* %14, align 4
  br label %33

33:                                               ; preds = %178, %5
  %34 = load i32, i32* %14, align 4
  %35 = load i32, i32* %13, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %181

37:                                               ; preds = %33
  %38 = load i32, i32* %14, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %39
  %41 = load i8, i8* %40, align 1
  %42 = sext i8 %41 to i32
  %43 = icmp eq i32 %42, 90
  br i1 %43, label %44, label %52

44:                                               ; preds = %37
  %45 = va_arg i8** %10, i32
  store i32 %45, i32* %15, align 4
  %46 = load i32, i32* %15, align 4
  %47 = trunc i32 %46 to i8
  %48 = load i32, i32* %14, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %47, i8* %51, align 8
  br label %177

52:                                               ; preds = %37
  %53 = load i32, i32* %14, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %54
  %56 = load i8, i8* %55, align 1
  %57 = sext i8 %56 to i32
  %58 = icmp eq i32 %57, 66
  br i1 %58, label %59, label %67

59:                                               ; preds = %52
  %60 = va_arg i8** %10, i32
  store i32 %60, i32* %16, align 4
  %61 = load i32, i32* %16, align 4
  %62 = trunc i32 %61 to i8
  %63 = load i32, i32* %14, align 4
  %64 = sext i32 %63 to i64
  %65 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %64
  %66 = bitcast %union.jvalue* %65 to i8*
  store i8 %62, i8* %66, align 8
  br label %176

67:                                               ; preds = %52
  %68 = load i32, i32* %14, align 4
  %69 = sext i32 %68 to i64
  %70 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %69
  %71 = load i8, i8* %70, align 1
  %72 = sext i8 %71 to i32
  %73 = icmp eq i32 %72, 67
  br i1 %73, label %74, label %82

74:                                               ; preds = %67
  %75 = va_arg i8** %10, i32
  store i32 %75, i32* %17, align 4
  %76 = load i32, i32* %17, align 4
  %77 = trunc i32 %76 to i16
  %78 = load i32, i32* %14, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %175

82:                                               ; preds = %67
  %83 = load i32, i32* %14, align 4
  %84 = sext i32 %83 to i64
  %85 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %84
  %86 = load i8, i8* %85, align 1
  %87 = sext i8 %86 to i32
  %88 = icmp eq i32 %87, 83
  br i1 %88, label %89, label %97

89:                                               ; preds = %82
  %90 = va_arg i8** %10, i32
  store i32 %90, i32* %18, align 4
  %91 = load i32, i32* %18, align 4
  %92 = trunc i32 %91 to i16
  %93 = load i32, i32* %14, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %94
  %96 = bitcast %union.jvalue* %95 to i16*
  store i16 %92, i16* %96, align 8
  br label %174

97:                                               ; preds = %82
  %98 = load i32, i32* %14, align 4
  %99 = sext i32 %98 to i64
  %100 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %99
  %101 = load i8, i8* %100, align 1
  %102 = sext i8 %101 to i32
  %103 = icmp eq i32 %102, 73
  br i1 %103, label %104, label %111

104:                                              ; preds = %97
  %105 = va_arg i8** %10, i32
  store i32 %105, i32* %19, align 4
  %106 = load i32, i32* %19, align 4
  %107 = load i32, i32* %14, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %108
  %110 = bitcast %union.jvalue* %109 to i32*
  store i32 %106, i32* %110, align 8
  br label %173

111:                                              ; preds = %97
  %112 = load i32, i32* %14, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %113
  %115 = load i8, i8* %114, align 1
  %116 = sext i8 %115 to i32
  %117 = icmp eq i32 %116, 74
  br i1 %117, label %118, label %125

118:                                              ; preds = %111
  %119 = va_arg i8** %10, i64
  store i64 %119, i64* %20, align 8
  %120 = load i64, i64* %20, align 8
  %121 = load i32, i32* %14, align 4
  %122 = sext i32 %121 to i64
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %120, i64* %124, align 8
  br label %172

125:                                              ; preds = %111
  %126 = load i32, i32* %14, align 4
  %127 = sext i32 %126 to i64
  %128 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %127
  %129 = load i8, i8* %128, align 1
  %130 = sext i8 %129 to i32
  %131 = icmp eq i32 %130, 70
  br i1 %131, label %132, label %140

132:                                              ; preds = %125
  %133 = va_arg i8** %10, double
  store double %133, double* %21, align 8
  %134 = load double, double* %21, align 8
  %135 = fptrunc double %134 to float
  %136 = load i32, i32* %14, align 4
  %137 = sext i32 %136 to i64
  %138 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %137
  %139 = bitcast %union.jvalue* %138 to float*
  store float %135, float* %139, align 8
  br label %171

140:                                              ; preds = %125
  %141 = load i32, i32* %14, align 4
  %142 = sext i32 %141 to i64
  %143 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %142
  %144 = load i8, i8* %143, align 1
  %145 = sext i8 %144 to i32
  %146 = icmp eq i32 %145, 68
  br i1 %146, label %147, label %154

147:                                              ; preds = %140
  %148 = va_arg i8** %10, double
  store double %148, double* %22, align 8
  %149 = load double, double* %22, align 8
  %150 = load i32, i32* %14, align 4
  %151 = sext i32 %150 to i64
  %152 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %151
  %153 = bitcast %union.jvalue* %152 to double*
  store double %149, double* %153, align 8
  br label %170

154:                                              ; preds = %140
  %155 = load i32, i32* %14, align 4
  %156 = sext i32 %155 to i64
  %157 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %156
  %158 = load i8, i8* %157, align 1
  %159 = sext i8 %158 to i32
  %160 = icmp eq i32 %159, 76
  br i1 %160, label %161, label %169

161:                                              ; preds = %154
  %162 = va_arg i8** %10, i8*
  store i8* %162, i8** %23, align 8
  %163 = load i8*, i8** %23, align 8
  %164 = bitcast i8* %163 to %struct._jobject*
  %165 = load i32, i32* %14, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %166
  %168 = bitcast %union.jvalue* %167 to %struct._jobject**
  store %struct._jobject* %164, %struct._jobject** %168, align 8
  br label %169

169:                                              ; preds = %161, %154
  br label %170

170:                                              ; preds = %169, %147
  br label %171

171:                                              ; preds = %170, %132
  br label %172

172:                                              ; preds = %171, %118
  br label %173

173:                                              ; preds = %172, %104
  br label %174

174:                                              ; preds = %173, %89
  br label %175

175:                                              ; preds = %174, %74
  br label %176

176:                                              ; preds = %175, %59
  br label %177

177:                                              ; preds = %176, %44
  br label %178

178:                                              ; preds = %177
  %179 = load i32, i32* %14, align 4
  %180 = add nsw i32 %179, 1
  store i32 %180, i32* %14, align 4
  br label %33, !llvm.loop !55

181:                                              ; preds = %33
  %182 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %183 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %182, align 8
  %184 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %183, i32 0, i32 87
  %185 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %184, align 8
  %186 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %187 = load %struct._jobject*, %struct._jobject** %7, align 8
  %188 = load %struct._jobject*, %struct._jobject** %8, align 8
  %189 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %190 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 0
  %191 = call float %185(%struct.JNINativeInterface_** noundef %186, %struct._jobject* noundef %187, %struct._jobject* noundef %188, %struct._jmethodID* noundef %189, %union.jvalue* noundef %190)
  ret float %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define float @JNI_CallStaticFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  %21 = alloca float, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %22 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %22)
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %24 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %23, align 8
  %25 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %24, i32 0, i32 0
  %26 = load i8*, i8** %25, align 8
  %27 = bitcast i8* %26 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %28 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %29 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %31 = call i32 %27(%struct.JNINativeInterface_** noundef %28, %struct._jmethodID* noundef %29, i8* noundef %30)
  store i32 %31, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %32

32:                                               ; preds = %177, %3
  %33 = load i32, i32* %11, align 4
  %34 = load i32, i32* %10, align 4
  %35 = icmp slt i32 %33, %34
  br i1 %35, label %36, label %180

36:                                               ; preds = %32
  %37 = load i32, i32* %11, align 4
  %38 = sext i32 %37 to i64
  %39 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %38
  %40 = load i8, i8* %39, align 1
  %41 = sext i8 %40 to i32
  %42 = icmp eq i32 %41, 90
  br i1 %42, label %43, label %51

43:                                               ; preds = %36
  %44 = va_arg i8** %7, i32
  store i32 %44, i32* %12, align 4
  %45 = load i32, i32* %12, align 4
  %46 = trunc i32 %45 to i8
  %47 = load i32, i32* %11, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %176

51:                                               ; preds = %36
  %52 = load i32, i32* %11, align 4
  %53 = sext i32 %52 to i64
  %54 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %53
  %55 = load i8, i8* %54, align 1
  %56 = sext i8 %55 to i32
  %57 = icmp eq i32 %56, 66
  br i1 %57, label %58, label %66

58:                                               ; preds = %51
  %59 = va_arg i8** %7, i32
  store i32 %59, i32* %13, align 4
  %60 = load i32, i32* %13, align 4
  %61 = trunc i32 %60 to i8
  %62 = load i32, i32* %11, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %63
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %61, i8* %65, align 8
  br label %175

66:                                               ; preds = %51
  %67 = load i32, i32* %11, align 4
  %68 = sext i32 %67 to i64
  %69 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %68
  %70 = load i8, i8* %69, align 1
  %71 = sext i8 %70 to i32
  %72 = icmp eq i32 %71, 67
  br i1 %72, label %73, label %81

73:                                               ; preds = %66
  %74 = va_arg i8** %7, i32
  store i32 %74, i32* %14, align 4
  %75 = load i32, i32* %14, align 4
  %76 = trunc i32 %75 to i16
  %77 = load i32, i32* %11, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %174

81:                                               ; preds = %66
  %82 = load i32, i32* %11, align 4
  %83 = sext i32 %82 to i64
  %84 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %83
  %85 = load i8, i8* %84, align 1
  %86 = sext i8 %85 to i32
  %87 = icmp eq i32 %86, 83
  br i1 %87, label %88, label %96

88:                                               ; preds = %81
  %89 = va_arg i8** %7, i32
  store i32 %89, i32* %15, align 4
  %90 = load i32, i32* %15, align 4
  %91 = trunc i32 %90 to i16
  %92 = load i32, i32* %11, align 4
  %93 = sext i32 %92 to i64
  %94 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %93
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %91, i16* %95, align 8
  br label %173

96:                                               ; preds = %81
  %97 = load i32, i32* %11, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %98
  %100 = load i8, i8* %99, align 1
  %101 = sext i8 %100 to i32
  %102 = icmp eq i32 %101, 73
  br i1 %102, label %103, label %110

103:                                              ; preds = %96
  %104 = va_arg i8** %7, i32
  store i32 %104, i32* %16, align 4
  %105 = load i32, i32* %16, align 4
  %106 = load i32, i32* %11, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %107
  %109 = bitcast %union.jvalue* %108 to i32*
  store i32 %105, i32* %109, align 8
  br label %172

110:                                              ; preds = %96
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %112
  %114 = load i8, i8* %113, align 1
  %115 = sext i8 %114 to i32
  %116 = icmp eq i32 %115, 74
  br i1 %116, label %117, label %124

117:                                              ; preds = %110
  %118 = va_arg i8** %7, i64
  store i64 %118, i64* %17, align 8
  %119 = load i64, i64* %17, align 8
  %120 = load i32, i32* %11, align 4
  %121 = sext i32 %120 to i64
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %119, i64* %123, align 8
  br label %171

124:                                              ; preds = %110
  %125 = load i32, i32* %11, align 4
  %126 = sext i32 %125 to i64
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %126
  %128 = load i8, i8* %127, align 1
  %129 = sext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %139

131:                                              ; preds = %124
  %132 = va_arg i8** %7, double
  store double %132, double* %18, align 8
  %133 = load double, double* %18, align 8
  %134 = fptrunc double %133 to float
  %135 = load i32, i32* %11, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %136
  %138 = bitcast %union.jvalue* %137 to float*
  store float %134, float* %138, align 8
  br label %170

139:                                              ; preds = %124
  %140 = load i32, i32* %11, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %141
  %143 = load i8, i8* %142, align 1
  %144 = sext i8 %143 to i32
  %145 = icmp eq i32 %144, 68
  br i1 %145, label %146, label %153

146:                                              ; preds = %139
  %147 = va_arg i8** %7, double
  store double %147, double* %19, align 8
  %148 = load double, double* %19, align 8
  %149 = load i32, i32* %11, align 4
  %150 = sext i32 %149 to i64
  %151 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %150
  %152 = bitcast %union.jvalue* %151 to double*
  store double %148, double* %152, align 8
  br label %169

153:                                              ; preds = %139
  %154 = load i32, i32* %11, align 4
  %155 = sext i32 %154 to i64
  %156 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %155
  %157 = load i8, i8* %156, align 1
  %158 = sext i8 %157 to i32
  %159 = icmp eq i32 %158, 76
  br i1 %159, label %160, label %168

160:                                              ; preds = %153
  %161 = va_arg i8** %7, i8*
  store i8* %161, i8** %20, align 8
  %162 = load i8*, i8** %20, align 8
  %163 = bitcast i8* %162 to %struct._jobject*
  %164 = load i32, i32* %11, align 4
  %165 = sext i32 %164 to i64
  %166 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %165
  %167 = bitcast %union.jvalue* %166 to %struct._jobject**
  store %struct._jobject* %163, %struct._jobject** %167, align 8
  br label %168

168:                                              ; preds = %160, %153
  br label %169

169:                                              ; preds = %168, %146
  br label %170

170:                                              ; preds = %169, %131
  br label %171

171:                                              ; preds = %170, %117
  br label %172

172:                                              ; preds = %171, %103
  br label %173

173:                                              ; preds = %172, %88
  br label %174

174:                                              ; preds = %173, %73
  br label %175

175:                                              ; preds = %174, %58
  br label %176

176:                                              ; preds = %175, %43
  br label %177

177:                                              ; preds = %176
  %178 = load i32, i32* %11, align 4
  %179 = add nsw i32 %178, 1
  store i32 %179, i32* %11, align 4
  br label %32, !llvm.loop !56

180:                                              ; preds = %32
  %181 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %182 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %181, align 8
  %183 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %182, i32 0, i32 137
  %184 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %183, align 8
  %185 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %186 = load %struct._jobject*, %struct._jobject** %5, align 8
  %187 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %188 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  %189 = call float %184(%struct.JNINativeInterface_** noundef %185, %struct._jobject* noundef %186, %struct._jmethodID* noundef %187, %union.jvalue* noundef %188)
  store float %189, float* %21, align 4
  %190 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %190)
  %191 = load float, float* %21, align 4
  ret float %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define float @JNI_CallStaticFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !57

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 137
  %183 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  %188 = call float %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret float %188
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define double @JNI_CallDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  %21 = alloca double, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %22 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %22)
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %24 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %23, align 8
  %25 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %24, i32 0, i32 0
  %26 = load i8*, i8** %25, align 8
  %27 = bitcast i8* %26 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %28 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %29 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %31 = call i32 %27(%struct.JNINativeInterface_** noundef %28, %struct._jmethodID* noundef %29, i8* noundef %30)
  store i32 %31, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %32

32:                                               ; preds = %177, %3
  %33 = load i32, i32* %11, align 4
  %34 = load i32, i32* %10, align 4
  %35 = icmp slt i32 %33, %34
  br i1 %35, label %36, label %180

36:                                               ; preds = %32
  %37 = load i32, i32* %11, align 4
  %38 = sext i32 %37 to i64
  %39 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %38
  %40 = load i8, i8* %39, align 1
  %41 = sext i8 %40 to i32
  %42 = icmp eq i32 %41, 90
  br i1 %42, label %43, label %51

43:                                               ; preds = %36
  %44 = va_arg i8** %7, i32
  store i32 %44, i32* %12, align 4
  %45 = load i32, i32* %12, align 4
  %46 = trunc i32 %45 to i8
  %47 = load i32, i32* %11, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %176

51:                                               ; preds = %36
  %52 = load i32, i32* %11, align 4
  %53 = sext i32 %52 to i64
  %54 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %53
  %55 = load i8, i8* %54, align 1
  %56 = sext i8 %55 to i32
  %57 = icmp eq i32 %56, 66
  br i1 %57, label %58, label %66

58:                                               ; preds = %51
  %59 = va_arg i8** %7, i32
  store i32 %59, i32* %13, align 4
  %60 = load i32, i32* %13, align 4
  %61 = trunc i32 %60 to i8
  %62 = load i32, i32* %11, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %63
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %61, i8* %65, align 8
  br label %175

66:                                               ; preds = %51
  %67 = load i32, i32* %11, align 4
  %68 = sext i32 %67 to i64
  %69 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %68
  %70 = load i8, i8* %69, align 1
  %71 = sext i8 %70 to i32
  %72 = icmp eq i32 %71, 67
  br i1 %72, label %73, label %81

73:                                               ; preds = %66
  %74 = va_arg i8** %7, i32
  store i32 %74, i32* %14, align 4
  %75 = load i32, i32* %14, align 4
  %76 = trunc i32 %75 to i16
  %77 = load i32, i32* %11, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %174

81:                                               ; preds = %66
  %82 = load i32, i32* %11, align 4
  %83 = sext i32 %82 to i64
  %84 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %83
  %85 = load i8, i8* %84, align 1
  %86 = sext i8 %85 to i32
  %87 = icmp eq i32 %86, 83
  br i1 %87, label %88, label %96

88:                                               ; preds = %81
  %89 = va_arg i8** %7, i32
  store i32 %89, i32* %15, align 4
  %90 = load i32, i32* %15, align 4
  %91 = trunc i32 %90 to i16
  %92 = load i32, i32* %11, align 4
  %93 = sext i32 %92 to i64
  %94 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %93
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %91, i16* %95, align 8
  br label %173

96:                                               ; preds = %81
  %97 = load i32, i32* %11, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %98
  %100 = load i8, i8* %99, align 1
  %101 = sext i8 %100 to i32
  %102 = icmp eq i32 %101, 73
  br i1 %102, label %103, label %110

103:                                              ; preds = %96
  %104 = va_arg i8** %7, i32
  store i32 %104, i32* %16, align 4
  %105 = load i32, i32* %16, align 4
  %106 = load i32, i32* %11, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %107
  %109 = bitcast %union.jvalue* %108 to i32*
  store i32 %105, i32* %109, align 8
  br label %172

110:                                              ; preds = %96
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %112
  %114 = load i8, i8* %113, align 1
  %115 = sext i8 %114 to i32
  %116 = icmp eq i32 %115, 74
  br i1 %116, label %117, label %124

117:                                              ; preds = %110
  %118 = va_arg i8** %7, i64
  store i64 %118, i64* %17, align 8
  %119 = load i64, i64* %17, align 8
  %120 = load i32, i32* %11, align 4
  %121 = sext i32 %120 to i64
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %119, i64* %123, align 8
  br label %171

124:                                              ; preds = %110
  %125 = load i32, i32* %11, align 4
  %126 = sext i32 %125 to i64
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %126
  %128 = load i8, i8* %127, align 1
  %129 = sext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %139

131:                                              ; preds = %124
  %132 = va_arg i8** %7, double
  store double %132, double* %18, align 8
  %133 = load double, double* %18, align 8
  %134 = fptrunc double %133 to float
  %135 = load i32, i32* %11, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %136
  %138 = bitcast %union.jvalue* %137 to float*
  store float %134, float* %138, align 8
  br label %170

139:                                              ; preds = %124
  %140 = load i32, i32* %11, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %141
  %143 = load i8, i8* %142, align 1
  %144 = sext i8 %143 to i32
  %145 = icmp eq i32 %144, 68
  br i1 %145, label %146, label %153

146:                                              ; preds = %139
  %147 = va_arg i8** %7, double
  store double %147, double* %19, align 8
  %148 = load double, double* %19, align 8
  %149 = load i32, i32* %11, align 4
  %150 = sext i32 %149 to i64
  %151 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %150
  %152 = bitcast %union.jvalue* %151 to double*
  store double %148, double* %152, align 8
  br label %169

153:                                              ; preds = %139
  %154 = load i32, i32* %11, align 4
  %155 = sext i32 %154 to i64
  %156 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %155
  %157 = load i8, i8* %156, align 1
  %158 = sext i8 %157 to i32
  %159 = icmp eq i32 %158, 76
  br i1 %159, label %160, label %168

160:                                              ; preds = %153
  %161 = va_arg i8** %7, i8*
  store i8* %161, i8** %20, align 8
  %162 = load i8*, i8** %20, align 8
  %163 = bitcast i8* %162 to %struct._jobject*
  %164 = load i32, i32* %11, align 4
  %165 = sext i32 %164 to i64
  %166 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %165
  %167 = bitcast %union.jvalue* %166 to %struct._jobject**
  store %struct._jobject* %163, %struct._jobject** %167, align 8
  br label %168

168:                                              ; preds = %160, %153
  br label %169

169:                                              ; preds = %168, %146
  br label %170

170:                                              ; preds = %169, %131
  br label %171

171:                                              ; preds = %170, %117
  br label %172

172:                                              ; preds = %171, %103
  br label %173

173:                                              ; preds = %172, %88
  br label %174

174:                                              ; preds = %173, %73
  br label %175

175:                                              ; preds = %174, %58
  br label %176

176:                                              ; preds = %175, %43
  br label %177

177:                                              ; preds = %176
  %178 = load i32, i32* %11, align 4
  %179 = add nsw i32 %178, 1
  store i32 %179, i32* %11, align 4
  br label %32, !llvm.loop !58

180:                                              ; preds = %32
  %181 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %182 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %181, align 8
  %183 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %182, i32 0, i32 60
  %184 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %183, align 8
  %185 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %186 = load %struct._jobject*, %struct._jobject** %5, align 8
  %187 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %188 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  %189 = call double %184(%struct.JNINativeInterface_** noundef %185, %struct._jobject* noundef %186, %struct._jmethodID* noundef %187, %union.jvalue* noundef %188)
  store double %189, double* %21, align 8
  %190 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %190)
  %191 = load double, double* %21, align 8
  ret double %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define double @JNI_CallDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !59

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 60
  %183 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  %188 = call double %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret double %188
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define double @JNI_CallNonvirtualDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i8*, align 8
  %10 = alloca [256 x i8], align 1
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i32, align 4
  %19 = alloca i64, align 8
  %20 = alloca double, align 8
  %21 = alloca double, align 8
  %22 = alloca i8*, align 8
  %23 = alloca double, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %24 = bitcast i8** %9 to i8*
  call void @llvm.va_start(i8* %24)
  %25 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %26 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %25, align 8
  %27 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %26, i32 0, i32 0
  %28 = load i8*, i8** %27, align 8
  %29 = bitcast i8* %28 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %30 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %31 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %32 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 0
  %33 = call i32 %29(%struct.JNINativeInterface_** noundef %30, %struct._jmethodID* noundef %31, i8* noundef %32)
  store i32 %33, i32* %12, align 4
  store i32 0, i32* %13, align 4
  br label %34

34:                                               ; preds = %179, %4
  %35 = load i32, i32* %13, align 4
  %36 = load i32, i32* %12, align 4
  %37 = icmp slt i32 %35, %36
  br i1 %37, label %38, label %182

38:                                               ; preds = %34
  %39 = load i32, i32* %13, align 4
  %40 = sext i32 %39 to i64
  %41 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %40
  %42 = load i8, i8* %41, align 1
  %43 = sext i8 %42 to i32
  %44 = icmp eq i32 %43, 90
  br i1 %44, label %45, label %53

45:                                               ; preds = %38
  %46 = va_arg i8** %9, i32
  store i32 %46, i32* %14, align 4
  %47 = load i32, i32* %14, align 4
  %48 = trunc i32 %47 to i8
  %49 = load i32, i32* %13, align 4
  %50 = sext i32 %49 to i64
  %51 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %50
  %52 = bitcast %union.jvalue* %51 to i8*
  store i8 %48, i8* %52, align 8
  br label %178

53:                                               ; preds = %38
  %54 = load i32, i32* %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %55
  %57 = load i8, i8* %56, align 1
  %58 = sext i8 %57 to i32
  %59 = icmp eq i32 %58, 66
  br i1 %59, label %60, label %68

60:                                               ; preds = %53
  %61 = va_arg i8** %9, i32
  store i32 %61, i32* %15, align 4
  %62 = load i32, i32* %15, align 4
  %63 = trunc i32 %62 to i8
  %64 = load i32, i32* %13, align 4
  %65 = sext i32 %64 to i64
  %66 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %65
  %67 = bitcast %union.jvalue* %66 to i8*
  store i8 %63, i8* %67, align 8
  br label %177

68:                                               ; preds = %53
  %69 = load i32, i32* %13, align 4
  %70 = sext i32 %69 to i64
  %71 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %70
  %72 = load i8, i8* %71, align 1
  %73 = sext i8 %72 to i32
  %74 = icmp eq i32 %73, 67
  br i1 %74, label %75, label %83

75:                                               ; preds = %68
  %76 = va_arg i8** %9, i32
  store i32 %76, i32* %16, align 4
  %77 = load i32, i32* %16, align 4
  %78 = trunc i32 %77 to i16
  %79 = load i32, i32* %13, align 4
  %80 = sext i32 %79 to i64
  %81 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %80
  %82 = bitcast %union.jvalue* %81 to i16*
  store i16 %78, i16* %82, align 8
  br label %176

83:                                               ; preds = %68
  %84 = load i32, i32* %13, align 4
  %85 = sext i32 %84 to i64
  %86 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %85
  %87 = load i8, i8* %86, align 1
  %88 = sext i8 %87 to i32
  %89 = icmp eq i32 %88, 83
  br i1 %89, label %90, label %98

90:                                               ; preds = %83
  %91 = va_arg i8** %9, i32
  store i32 %91, i32* %17, align 4
  %92 = load i32, i32* %17, align 4
  %93 = trunc i32 %92 to i16
  %94 = load i32, i32* %13, align 4
  %95 = sext i32 %94 to i64
  %96 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %95
  %97 = bitcast %union.jvalue* %96 to i16*
  store i16 %93, i16* %97, align 8
  br label %175

98:                                               ; preds = %83
  %99 = load i32, i32* %13, align 4
  %100 = sext i32 %99 to i64
  %101 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %100
  %102 = load i8, i8* %101, align 1
  %103 = sext i8 %102 to i32
  %104 = icmp eq i32 %103, 73
  br i1 %104, label %105, label %112

105:                                              ; preds = %98
  %106 = va_arg i8** %9, i32
  store i32 %106, i32* %18, align 4
  %107 = load i32, i32* %18, align 4
  %108 = load i32, i32* %13, align 4
  %109 = sext i32 %108 to i64
  %110 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %109
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %107, i32* %111, align 8
  br label %174

112:                                              ; preds = %98
  %113 = load i32, i32* %13, align 4
  %114 = sext i32 %113 to i64
  %115 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %114
  %116 = load i8, i8* %115, align 1
  %117 = sext i8 %116 to i32
  %118 = icmp eq i32 %117, 74
  br i1 %118, label %119, label %126

119:                                              ; preds = %112
  %120 = va_arg i8** %9, i64
  store i64 %120, i64* %19, align 8
  %121 = load i64, i64* %19, align 8
  %122 = load i32, i32* %13, align 4
  %123 = sext i32 %122 to i64
  %124 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %123
  %125 = bitcast %union.jvalue* %124 to i64*
  store i64 %121, i64* %125, align 8
  br label %173

126:                                              ; preds = %112
  %127 = load i32, i32* %13, align 4
  %128 = sext i32 %127 to i64
  %129 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %128
  %130 = load i8, i8* %129, align 1
  %131 = sext i8 %130 to i32
  %132 = icmp eq i32 %131, 70
  br i1 %132, label %133, label %141

133:                                              ; preds = %126
  %134 = va_arg i8** %9, double
  store double %134, double* %20, align 8
  %135 = load double, double* %20, align 8
  %136 = fptrunc double %135 to float
  %137 = load i32, i32* %13, align 4
  %138 = sext i32 %137 to i64
  %139 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %138
  %140 = bitcast %union.jvalue* %139 to float*
  store float %136, float* %140, align 8
  br label %172

141:                                              ; preds = %126
  %142 = load i32, i32* %13, align 4
  %143 = sext i32 %142 to i64
  %144 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %143
  %145 = load i8, i8* %144, align 1
  %146 = sext i8 %145 to i32
  %147 = icmp eq i32 %146, 68
  br i1 %147, label %148, label %155

148:                                              ; preds = %141
  %149 = va_arg i8** %9, double
  store double %149, double* %21, align 8
  %150 = load double, double* %21, align 8
  %151 = load i32, i32* %13, align 4
  %152 = sext i32 %151 to i64
  %153 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %152
  %154 = bitcast %union.jvalue* %153 to double*
  store double %150, double* %154, align 8
  br label %171

155:                                              ; preds = %141
  %156 = load i32, i32* %13, align 4
  %157 = sext i32 %156 to i64
  %158 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %157
  %159 = load i8, i8* %158, align 1
  %160 = sext i8 %159 to i32
  %161 = icmp eq i32 %160, 76
  br i1 %161, label %162, label %170

162:                                              ; preds = %155
  %163 = va_arg i8** %9, i8*
  store i8* %163, i8** %22, align 8
  %164 = load i8*, i8** %22, align 8
  %165 = bitcast i8* %164 to %struct._jobject*
  %166 = load i32, i32* %13, align 4
  %167 = sext i32 %166 to i64
  %168 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %167
  %169 = bitcast %union.jvalue* %168 to %struct._jobject**
  store %struct._jobject* %165, %struct._jobject** %169, align 8
  br label %170

170:                                              ; preds = %162, %155
  br label %171

171:                                              ; preds = %170, %148
  br label %172

172:                                              ; preds = %171, %133
  br label %173

173:                                              ; preds = %172, %119
  br label %174

174:                                              ; preds = %173, %105
  br label %175

175:                                              ; preds = %174, %90
  br label %176

176:                                              ; preds = %175, %75
  br label %177

177:                                              ; preds = %176, %60
  br label %178

178:                                              ; preds = %177, %45
  br label %179

179:                                              ; preds = %178
  %180 = load i32, i32* %13, align 4
  %181 = add nsw i32 %180, 1
  store i32 %181, i32* %13, align 4
  br label %34, !llvm.loop !60

182:                                              ; preds = %34
  %183 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %184 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %183, align 8
  %185 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %184, i32 0, i32 90
  %186 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %185, align 8
  %187 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %188 = load %struct._jobject*, %struct._jobject** %6, align 8
  %189 = load %struct._jobject*, %struct._jobject** %7, align 8
  %190 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %191 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 0
  %192 = call double %186(%struct.JNINativeInterface_** noundef %187, %struct._jobject* noundef %188, %struct._jobject* noundef %189, %struct._jmethodID* noundef %190, %union.jvalue* noundef %191)
  store double %192, double* %23, align 8
  %193 = bitcast i8** %9 to i8*
  call void @llvm.va_end(i8* %193)
  %194 = load double, double* %23, align 8
  ret double %194
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define double @JNI_CallNonvirtualDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, i8* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca i8*, align 8
  %11 = alloca [256 x i8], align 1
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i32, align 4
  %19 = alloca i32, align 4
  %20 = alloca i64, align 8
  %21 = alloca double, align 8
  %22 = alloca double, align 8
  %23 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  store i8* %4, i8** %10, align 8
  %24 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %25 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %25, i32 0, i32 0
  %27 = load i8*, i8** %26, align 8
  %28 = bitcast i8* %27 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %29 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %30 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 0
  %32 = call i32 %28(%struct.JNINativeInterface_** noundef %29, %struct._jmethodID* noundef %30, i8* noundef %31)
  store i32 %32, i32* %13, align 4
  store i32 0, i32* %14, align 4
  br label %33

33:                                               ; preds = %178, %5
  %34 = load i32, i32* %14, align 4
  %35 = load i32, i32* %13, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %181

37:                                               ; preds = %33
  %38 = load i32, i32* %14, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %39
  %41 = load i8, i8* %40, align 1
  %42 = sext i8 %41 to i32
  %43 = icmp eq i32 %42, 90
  br i1 %43, label %44, label %52

44:                                               ; preds = %37
  %45 = va_arg i8** %10, i32
  store i32 %45, i32* %15, align 4
  %46 = load i32, i32* %15, align 4
  %47 = trunc i32 %46 to i8
  %48 = load i32, i32* %14, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %47, i8* %51, align 8
  br label %177

52:                                               ; preds = %37
  %53 = load i32, i32* %14, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %54
  %56 = load i8, i8* %55, align 1
  %57 = sext i8 %56 to i32
  %58 = icmp eq i32 %57, 66
  br i1 %58, label %59, label %67

59:                                               ; preds = %52
  %60 = va_arg i8** %10, i32
  store i32 %60, i32* %16, align 4
  %61 = load i32, i32* %16, align 4
  %62 = trunc i32 %61 to i8
  %63 = load i32, i32* %14, align 4
  %64 = sext i32 %63 to i64
  %65 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %64
  %66 = bitcast %union.jvalue* %65 to i8*
  store i8 %62, i8* %66, align 8
  br label %176

67:                                               ; preds = %52
  %68 = load i32, i32* %14, align 4
  %69 = sext i32 %68 to i64
  %70 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %69
  %71 = load i8, i8* %70, align 1
  %72 = sext i8 %71 to i32
  %73 = icmp eq i32 %72, 67
  br i1 %73, label %74, label %82

74:                                               ; preds = %67
  %75 = va_arg i8** %10, i32
  store i32 %75, i32* %17, align 4
  %76 = load i32, i32* %17, align 4
  %77 = trunc i32 %76 to i16
  %78 = load i32, i32* %14, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %175

82:                                               ; preds = %67
  %83 = load i32, i32* %14, align 4
  %84 = sext i32 %83 to i64
  %85 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %84
  %86 = load i8, i8* %85, align 1
  %87 = sext i8 %86 to i32
  %88 = icmp eq i32 %87, 83
  br i1 %88, label %89, label %97

89:                                               ; preds = %82
  %90 = va_arg i8** %10, i32
  store i32 %90, i32* %18, align 4
  %91 = load i32, i32* %18, align 4
  %92 = trunc i32 %91 to i16
  %93 = load i32, i32* %14, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %94
  %96 = bitcast %union.jvalue* %95 to i16*
  store i16 %92, i16* %96, align 8
  br label %174

97:                                               ; preds = %82
  %98 = load i32, i32* %14, align 4
  %99 = sext i32 %98 to i64
  %100 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %99
  %101 = load i8, i8* %100, align 1
  %102 = sext i8 %101 to i32
  %103 = icmp eq i32 %102, 73
  br i1 %103, label %104, label %111

104:                                              ; preds = %97
  %105 = va_arg i8** %10, i32
  store i32 %105, i32* %19, align 4
  %106 = load i32, i32* %19, align 4
  %107 = load i32, i32* %14, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %108
  %110 = bitcast %union.jvalue* %109 to i32*
  store i32 %106, i32* %110, align 8
  br label %173

111:                                              ; preds = %97
  %112 = load i32, i32* %14, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %113
  %115 = load i8, i8* %114, align 1
  %116 = sext i8 %115 to i32
  %117 = icmp eq i32 %116, 74
  br i1 %117, label %118, label %125

118:                                              ; preds = %111
  %119 = va_arg i8** %10, i64
  store i64 %119, i64* %20, align 8
  %120 = load i64, i64* %20, align 8
  %121 = load i32, i32* %14, align 4
  %122 = sext i32 %121 to i64
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %120, i64* %124, align 8
  br label %172

125:                                              ; preds = %111
  %126 = load i32, i32* %14, align 4
  %127 = sext i32 %126 to i64
  %128 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %127
  %129 = load i8, i8* %128, align 1
  %130 = sext i8 %129 to i32
  %131 = icmp eq i32 %130, 70
  br i1 %131, label %132, label %140

132:                                              ; preds = %125
  %133 = va_arg i8** %10, double
  store double %133, double* %21, align 8
  %134 = load double, double* %21, align 8
  %135 = fptrunc double %134 to float
  %136 = load i32, i32* %14, align 4
  %137 = sext i32 %136 to i64
  %138 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %137
  %139 = bitcast %union.jvalue* %138 to float*
  store float %135, float* %139, align 8
  br label %171

140:                                              ; preds = %125
  %141 = load i32, i32* %14, align 4
  %142 = sext i32 %141 to i64
  %143 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %142
  %144 = load i8, i8* %143, align 1
  %145 = sext i8 %144 to i32
  %146 = icmp eq i32 %145, 68
  br i1 %146, label %147, label %154

147:                                              ; preds = %140
  %148 = va_arg i8** %10, double
  store double %148, double* %22, align 8
  %149 = load double, double* %22, align 8
  %150 = load i32, i32* %14, align 4
  %151 = sext i32 %150 to i64
  %152 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %151
  %153 = bitcast %union.jvalue* %152 to double*
  store double %149, double* %153, align 8
  br label %170

154:                                              ; preds = %140
  %155 = load i32, i32* %14, align 4
  %156 = sext i32 %155 to i64
  %157 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %156
  %158 = load i8, i8* %157, align 1
  %159 = sext i8 %158 to i32
  %160 = icmp eq i32 %159, 76
  br i1 %160, label %161, label %169

161:                                              ; preds = %154
  %162 = va_arg i8** %10, i8*
  store i8* %162, i8** %23, align 8
  %163 = load i8*, i8** %23, align 8
  %164 = bitcast i8* %163 to %struct._jobject*
  %165 = load i32, i32* %14, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %166
  %168 = bitcast %union.jvalue* %167 to %struct._jobject**
  store %struct._jobject* %164, %struct._jobject** %168, align 8
  br label %169

169:                                              ; preds = %161, %154
  br label %170

170:                                              ; preds = %169, %147
  br label %171

171:                                              ; preds = %170, %132
  br label %172

172:                                              ; preds = %171, %118
  br label %173

173:                                              ; preds = %172, %104
  br label %174

174:                                              ; preds = %173, %89
  br label %175

175:                                              ; preds = %174, %74
  br label %176

176:                                              ; preds = %175, %59
  br label %177

177:                                              ; preds = %176, %44
  br label %178

178:                                              ; preds = %177
  %179 = load i32, i32* %14, align 4
  %180 = add nsw i32 %179, 1
  store i32 %180, i32* %14, align 4
  br label %33, !llvm.loop !61

181:                                              ; preds = %33
  %182 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %183 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %182, align 8
  %184 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %183, i32 0, i32 90
  %185 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %184, align 8
  %186 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %187 = load %struct._jobject*, %struct._jobject** %7, align 8
  %188 = load %struct._jobject*, %struct._jobject** %8, align 8
  %189 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %190 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 0
  %191 = call double %185(%struct.JNINativeInterface_** noundef %186, %struct._jobject* noundef %187, %struct._jobject* noundef %188, %struct._jmethodID* noundef %189, %union.jvalue* noundef %190)
  ret double %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define double @JNI_CallStaticDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  %21 = alloca double, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %22 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %22)
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %24 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %23, align 8
  %25 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %24, i32 0, i32 0
  %26 = load i8*, i8** %25, align 8
  %27 = bitcast i8* %26 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %28 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %29 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %31 = call i32 %27(%struct.JNINativeInterface_** noundef %28, %struct._jmethodID* noundef %29, i8* noundef %30)
  store i32 %31, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %32

32:                                               ; preds = %177, %3
  %33 = load i32, i32* %11, align 4
  %34 = load i32, i32* %10, align 4
  %35 = icmp slt i32 %33, %34
  br i1 %35, label %36, label %180

36:                                               ; preds = %32
  %37 = load i32, i32* %11, align 4
  %38 = sext i32 %37 to i64
  %39 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %38
  %40 = load i8, i8* %39, align 1
  %41 = sext i8 %40 to i32
  %42 = icmp eq i32 %41, 90
  br i1 %42, label %43, label %51

43:                                               ; preds = %36
  %44 = va_arg i8** %7, i32
  store i32 %44, i32* %12, align 4
  %45 = load i32, i32* %12, align 4
  %46 = trunc i32 %45 to i8
  %47 = load i32, i32* %11, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %176

51:                                               ; preds = %36
  %52 = load i32, i32* %11, align 4
  %53 = sext i32 %52 to i64
  %54 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %53
  %55 = load i8, i8* %54, align 1
  %56 = sext i8 %55 to i32
  %57 = icmp eq i32 %56, 66
  br i1 %57, label %58, label %66

58:                                               ; preds = %51
  %59 = va_arg i8** %7, i32
  store i32 %59, i32* %13, align 4
  %60 = load i32, i32* %13, align 4
  %61 = trunc i32 %60 to i8
  %62 = load i32, i32* %11, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %63
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %61, i8* %65, align 8
  br label %175

66:                                               ; preds = %51
  %67 = load i32, i32* %11, align 4
  %68 = sext i32 %67 to i64
  %69 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %68
  %70 = load i8, i8* %69, align 1
  %71 = sext i8 %70 to i32
  %72 = icmp eq i32 %71, 67
  br i1 %72, label %73, label %81

73:                                               ; preds = %66
  %74 = va_arg i8** %7, i32
  store i32 %74, i32* %14, align 4
  %75 = load i32, i32* %14, align 4
  %76 = trunc i32 %75 to i16
  %77 = load i32, i32* %11, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %174

81:                                               ; preds = %66
  %82 = load i32, i32* %11, align 4
  %83 = sext i32 %82 to i64
  %84 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %83
  %85 = load i8, i8* %84, align 1
  %86 = sext i8 %85 to i32
  %87 = icmp eq i32 %86, 83
  br i1 %87, label %88, label %96

88:                                               ; preds = %81
  %89 = va_arg i8** %7, i32
  store i32 %89, i32* %15, align 4
  %90 = load i32, i32* %15, align 4
  %91 = trunc i32 %90 to i16
  %92 = load i32, i32* %11, align 4
  %93 = sext i32 %92 to i64
  %94 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %93
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %91, i16* %95, align 8
  br label %173

96:                                               ; preds = %81
  %97 = load i32, i32* %11, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %98
  %100 = load i8, i8* %99, align 1
  %101 = sext i8 %100 to i32
  %102 = icmp eq i32 %101, 73
  br i1 %102, label %103, label %110

103:                                              ; preds = %96
  %104 = va_arg i8** %7, i32
  store i32 %104, i32* %16, align 4
  %105 = load i32, i32* %16, align 4
  %106 = load i32, i32* %11, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %107
  %109 = bitcast %union.jvalue* %108 to i32*
  store i32 %105, i32* %109, align 8
  br label %172

110:                                              ; preds = %96
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %112
  %114 = load i8, i8* %113, align 1
  %115 = sext i8 %114 to i32
  %116 = icmp eq i32 %115, 74
  br i1 %116, label %117, label %124

117:                                              ; preds = %110
  %118 = va_arg i8** %7, i64
  store i64 %118, i64* %17, align 8
  %119 = load i64, i64* %17, align 8
  %120 = load i32, i32* %11, align 4
  %121 = sext i32 %120 to i64
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %119, i64* %123, align 8
  br label %171

124:                                              ; preds = %110
  %125 = load i32, i32* %11, align 4
  %126 = sext i32 %125 to i64
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %126
  %128 = load i8, i8* %127, align 1
  %129 = sext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %139

131:                                              ; preds = %124
  %132 = va_arg i8** %7, double
  store double %132, double* %18, align 8
  %133 = load double, double* %18, align 8
  %134 = fptrunc double %133 to float
  %135 = load i32, i32* %11, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %136
  %138 = bitcast %union.jvalue* %137 to float*
  store float %134, float* %138, align 8
  br label %170

139:                                              ; preds = %124
  %140 = load i32, i32* %11, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %141
  %143 = load i8, i8* %142, align 1
  %144 = sext i8 %143 to i32
  %145 = icmp eq i32 %144, 68
  br i1 %145, label %146, label %153

146:                                              ; preds = %139
  %147 = va_arg i8** %7, double
  store double %147, double* %19, align 8
  %148 = load double, double* %19, align 8
  %149 = load i32, i32* %11, align 4
  %150 = sext i32 %149 to i64
  %151 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %150
  %152 = bitcast %union.jvalue* %151 to double*
  store double %148, double* %152, align 8
  br label %169

153:                                              ; preds = %139
  %154 = load i32, i32* %11, align 4
  %155 = sext i32 %154 to i64
  %156 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %155
  %157 = load i8, i8* %156, align 1
  %158 = sext i8 %157 to i32
  %159 = icmp eq i32 %158, 76
  br i1 %159, label %160, label %168

160:                                              ; preds = %153
  %161 = va_arg i8** %7, i8*
  store i8* %161, i8** %20, align 8
  %162 = load i8*, i8** %20, align 8
  %163 = bitcast i8* %162 to %struct._jobject*
  %164 = load i32, i32* %11, align 4
  %165 = sext i32 %164 to i64
  %166 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %165
  %167 = bitcast %union.jvalue* %166 to %struct._jobject**
  store %struct._jobject* %163, %struct._jobject** %167, align 8
  br label %168

168:                                              ; preds = %160, %153
  br label %169

169:                                              ; preds = %168, %146
  br label %170

170:                                              ; preds = %169, %131
  br label %171

171:                                              ; preds = %170, %117
  br label %172

172:                                              ; preds = %171, %103
  br label %173

173:                                              ; preds = %172, %88
  br label %174

174:                                              ; preds = %173, %73
  br label %175

175:                                              ; preds = %174, %58
  br label %176

176:                                              ; preds = %175, %43
  br label %177

177:                                              ; preds = %176
  %178 = load i32, i32* %11, align 4
  %179 = add nsw i32 %178, 1
  store i32 %179, i32* %11, align 4
  br label %32, !llvm.loop !62

180:                                              ; preds = %32
  %181 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %182 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %181, align 8
  %183 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %182, i32 0, i32 140
  %184 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %183, align 8
  %185 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %186 = load %struct._jobject*, %struct._jobject** %5, align 8
  %187 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %188 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  %189 = call double %184(%struct.JNINativeInterface_** noundef %185, %struct._jobject* noundef %186, %struct._jmethodID* noundef %187, %union.jvalue* noundef %188)
  store double %189, double* %21, align 8
  %190 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %190)
  %191 = load double, double* %21, align 8
  ret double %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define double @JNI_CallStaticDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !63

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 140
  %183 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  %188 = call double %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret double %188
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define %struct._jobject* @JNI_NewObject(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  %21 = alloca %struct._jobject*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %22 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %22)
  %23 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %24 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %23, align 8
  %25 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %24, i32 0, i32 0
  %26 = load i8*, i8** %25, align 8
  %27 = bitcast i8* %26 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %28 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %29 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %30 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %31 = call i32 %27(%struct.JNINativeInterface_** noundef %28, %struct._jmethodID* noundef %29, i8* noundef %30)
  store i32 %31, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %32

32:                                               ; preds = %177, %3
  %33 = load i32, i32* %11, align 4
  %34 = load i32, i32* %10, align 4
  %35 = icmp slt i32 %33, %34
  br i1 %35, label %36, label %180

36:                                               ; preds = %32
  %37 = load i32, i32* %11, align 4
  %38 = sext i32 %37 to i64
  %39 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %38
  %40 = load i8, i8* %39, align 1
  %41 = sext i8 %40 to i32
  %42 = icmp eq i32 %41, 90
  br i1 %42, label %43, label %51

43:                                               ; preds = %36
  %44 = va_arg i8** %7, i32
  store i32 %44, i32* %12, align 4
  %45 = load i32, i32* %12, align 4
  %46 = trunc i32 %45 to i8
  %47 = load i32, i32* %11, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %48
  %50 = bitcast %union.jvalue* %49 to i8*
  store i8 %46, i8* %50, align 8
  br label %176

51:                                               ; preds = %36
  %52 = load i32, i32* %11, align 4
  %53 = sext i32 %52 to i64
  %54 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %53
  %55 = load i8, i8* %54, align 1
  %56 = sext i8 %55 to i32
  %57 = icmp eq i32 %56, 66
  br i1 %57, label %58, label %66

58:                                               ; preds = %51
  %59 = va_arg i8** %7, i32
  store i32 %59, i32* %13, align 4
  %60 = load i32, i32* %13, align 4
  %61 = trunc i32 %60 to i8
  %62 = load i32, i32* %11, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %63
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %61, i8* %65, align 8
  br label %175

66:                                               ; preds = %51
  %67 = load i32, i32* %11, align 4
  %68 = sext i32 %67 to i64
  %69 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %68
  %70 = load i8, i8* %69, align 1
  %71 = sext i8 %70 to i32
  %72 = icmp eq i32 %71, 67
  br i1 %72, label %73, label %81

73:                                               ; preds = %66
  %74 = va_arg i8** %7, i32
  store i32 %74, i32* %14, align 4
  %75 = load i32, i32* %14, align 4
  %76 = trunc i32 %75 to i16
  %77 = load i32, i32* %11, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %78
  %80 = bitcast %union.jvalue* %79 to i16*
  store i16 %76, i16* %80, align 8
  br label %174

81:                                               ; preds = %66
  %82 = load i32, i32* %11, align 4
  %83 = sext i32 %82 to i64
  %84 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %83
  %85 = load i8, i8* %84, align 1
  %86 = sext i8 %85 to i32
  %87 = icmp eq i32 %86, 83
  br i1 %87, label %88, label %96

88:                                               ; preds = %81
  %89 = va_arg i8** %7, i32
  store i32 %89, i32* %15, align 4
  %90 = load i32, i32* %15, align 4
  %91 = trunc i32 %90 to i16
  %92 = load i32, i32* %11, align 4
  %93 = sext i32 %92 to i64
  %94 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %93
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %91, i16* %95, align 8
  br label %173

96:                                               ; preds = %81
  %97 = load i32, i32* %11, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %98
  %100 = load i8, i8* %99, align 1
  %101 = sext i8 %100 to i32
  %102 = icmp eq i32 %101, 73
  br i1 %102, label %103, label %110

103:                                              ; preds = %96
  %104 = va_arg i8** %7, i32
  store i32 %104, i32* %16, align 4
  %105 = load i32, i32* %16, align 4
  %106 = load i32, i32* %11, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %107
  %109 = bitcast %union.jvalue* %108 to i32*
  store i32 %105, i32* %109, align 8
  br label %172

110:                                              ; preds = %96
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %112
  %114 = load i8, i8* %113, align 1
  %115 = sext i8 %114 to i32
  %116 = icmp eq i32 %115, 74
  br i1 %116, label %117, label %124

117:                                              ; preds = %110
  %118 = va_arg i8** %7, i64
  store i64 %118, i64* %17, align 8
  %119 = load i64, i64* %17, align 8
  %120 = load i32, i32* %11, align 4
  %121 = sext i32 %120 to i64
  %122 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %121
  %123 = bitcast %union.jvalue* %122 to i64*
  store i64 %119, i64* %123, align 8
  br label %171

124:                                              ; preds = %110
  %125 = load i32, i32* %11, align 4
  %126 = sext i32 %125 to i64
  %127 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %126
  %128 = load i8, i8* %127, align 1
  %129 = sext i8 %128 to i32
  %130 = icmp eq i32 %129, 70
  br i1 %130, label %131, label %139

131:                                              ; preds = %124
  %132 = va_arg i8** %7, double
  store double %132, double* %18, align 8
  %133 = load double, double* %18, align 8
  %134 = fptrunc double %133 to float
  %135 = load i32, i32* %11, align 4
  %136 = sext i32 %135 to i64
  %137 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %136
  %138 = bitcast %union.jvalue* %137 to float*
  store float %134, float* %138, align 8
  br label %170

139:                                              ; preds = %124
  %140 = load i32, i32* %11, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %141
  %143 = load i8, i8* %142, align 1
  %144 = sext i8 %143 to i32
  %145 = icmp eq i32 %144, 68
  br i1 %145, label %146, label %153

146:                                              ; preds = %139
  %147 = va_arg i8** %7, double
  store double %147, double* %19, align 8
  %148 = load double, double* %19, align 8
  %149 = load i32, i32* %11, align 4
  %150 = sext i32 %149 to i64
  %151 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %150
  %152 = bitcast %union.jvalue* %151 to double*
  store double %148, double* %152, align 8
  br label %169

153:                                              ; preds = %139
  %154 = load i32, i32* %11, align 4
  %155 = sext i32 %154 to i64
  %156 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %155
  %157 = load i8, i8* %156, align 1
  %158 = sext i8 %157 to i32
  %159 = icmp eq i32 %158, 76
  br i1 %159, label %160, label %168

160:                                              ; preds = %153
  %161 = va_arg i8** %7, i8*
  store i8* %161, i8** %20, align 8
  %162 = load i8*, i8** %20, align 8
  %163 = bitcast i8* %162 to %struct._jobject*
  %164 = load i32, i32* %11, align 4
  %165 = sext i32 %164 to i64
  %166 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %165
  %167 = bitcast %union.jvalue* %166 to %struct._jobject**
  store %struct._jobject* %163, %struct._jobject** %167, align 8
  br label %168

168:                                              ; preds = %160, %153
  br label %169

169:                                              ; preds = %168, %146
  br label %170

170:                                              ; preds = %169, %131
  br label %171

171:                                              ; preds = %170, %117
  br label %172

172:                                              ; preds = %171, %103
  br label %173

173:                                              ; preds = %172, %88
  br label %174

174:                                              ; preds = %173, %73
  br label %175

175:                                              ; preds = %174, %58
  br label %176

176:                                              ; preds = %175, %43
  br label %177

177:                                              ; preds = %176
  %178 = load i32, i32* %11, align 4
  %179 = add nsw i32 %178, 1
  store i32 %179, i32* %11, align 4
  br label %32, !llvm.loop !64

180:                                              ; preds = %32
  %181 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %182 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %181, align 8
  %183 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %182, i32 0, i32 30
  %184 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %183, align 8
  %185 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %186 = load %struct._jobject*, %struct._jobject** %5, align 8
  %187 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %188 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  %189 = call %struct._jobject* %184(%struct.JNINativeInterface_** noundef %185, %struct._jobject* noundef %186, %struct._jmethodID* noundef %187, %union.jvalue* noundef %188)
  store %struct._jobject* %189, %struct._jobject** %21, align 8
  %190 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %190)
  %191 = load %struct._jobject*, %struct._jobject** %21, align 8
  ret %struct._jobject* %191
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define %struct._jobject* @JNI_NewObjectV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !65

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 30
  %183 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  %188 = call %struct._jobject* %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret %struct._jobject* %188
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define void @JNI_CallVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %21 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %21)
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %31

31:                                               ; preds = %176, %3
  %32 = load i32, i32* %11, align 4
  %33 = load i32, i32* %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %11, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %7, i32
  store i32 %43, i32* %12, align 4
  %44 = load i32, i32* %12, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %11, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %11, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %7, i32
  store i32 %58, i32* %13, align 4
  %59 = load i32, i32* %13, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %11, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %11, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %7, i32
  store i32 %73, i32* %14, align 4
  %74 = load i32, i32* %14, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %11, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %11, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %7, i32
  store i32 %88, i32* %15, align 4
  %89 = load i32, i32* %15, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %11, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %11, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %7, i32
  store i32 %103, i32* %16, align 4
  %104 = load i32, i32* %16, align 4
  %105 = load i32, i32* %11, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %11, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %7, i64
  store i64 %117, i64* %17, align 8
  %118 = load i64, i64* %17, align 8
  %119 = load i32, i32* %11, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %11, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %7, double
  store double %131, double* %18, align 8
  %132 = load double, double* %18, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %11, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %11, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %7, double
  store double %146, double* %19, align 8
  %147 = load double, double* %19, align 8
  %148 = load i32, i32* %11, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %11, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %7, i8*
  store i8* %160, i8** %20, align 8
  %161 = load i8*, i8** %20, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %11, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %11, align 4
  br label %31, !llvm.loop !66

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 63
  %183 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %185 = load %struct._jobject*, %struct._jobject** %5, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  call void %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  %188 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %188)
  ret void
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define void @JNI_CallVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !67

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 63
  %183 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  call void %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret void
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define void @JNI_CallNonvirtualVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i8*, align 8
  %10 = alloca [256 x i8], align 1
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i32, align 4
  %19 = alloca i64, align 8
  %20 = alloca double, align 8
  %21 = alloca double, align 8
  %22 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %23 = bitcast i8** %9 to i8*
  call void @llvm.va_start(i8* %23)
  %24 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %25 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %25, i32 0, i32 0
  %27 = load i8*, i8** %26, align 8
  %28 = bitcast i8* %27 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %29 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %30 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 0
  %32 = call i32 %28(%struct.JNINativeInterface_** noundef %29, %struct._jmethodID* noundef %30, i8* noundef %31)
  store i32 %32, i32* %12, align 4
  store i32 0, i32* %13, align 4
  br label %33

33:                                               ; preds = %178, %4
  %34 = load i32, i32* %13, align 4
  %35 = load i32, i32* %12, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %181

37:                                               ; preds = %33
  %38 = load i32, i32* %13, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %39
  %41 = load i8, i8* %40, align 1
  %42 = sext i8 %41 to i32
  %43 = icmp eq i32 %42, 90
  br i1 %43, label %44, label %52

44:                                               ; preds = %37
  %45 = va_arg i8** %9, i32
  store i32 %45, i32* %14, align 4
  %46 = load i32, i32* %14, align 4
  %47 = trunc i32 %46 to i8
  %48 = load i32, i32* %13, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %47, i8* %51, align 8
  br label %177

52:                                               ; preds = %37
  %53 = load i32, i32* %13, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %54
  %56 = load i8, i8* %55, align 1
  %57 = sext i8 %56 to i32
  %58 = icmp eq i32 %57, 66
  br i1 %58, label %59, label %67

59:                                               ; preds = %52
  %60 = va_arg i8** %9, i32
  store i32 %60, i32* %15, align 4
  %61 = load i32, i32* %15, align 4
  %62 = trunc i32 %61 to i8
  %63 = load i32, i32* %13, align 4
  %64 = sext i32 %63 to i64
  %65 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %64
  %66 = bitcast %union.jvalue* %65 to i8*
  store i8 %62, i8* %66, align 8
  br label %176

67:                                               ; preds = %52
  %68 = load i32, i32* %13, align 4
  %69 = sext i32 %68 to i64
  %70 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %69
  %71 = load i8, i8* %70, align 1
  %72 = sext i8 %71 to i32
  %73 = icmp eq i32 %72, 67
  br i1 %73, label %74, label %82

74:                                               ; preds = %67
  %75 = va_arg i8** %9, i32
  store i32 %75, i32* %16, align 4
  %76 = load i32, i32* %16, align 4
  %77 = trunc i32 %76 to i16
  %78 = load i32, i32* %13, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %175

82:                                               ; preds = %67
  %83 = load i32, i32* %13, align 4
  %84 = sext i32 %83 to i64
  %85 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %84
  %86 = load i8, i8* %85, align 1
  %87 = sext i8 %86 to i32
  %88 = icmp eq i32 %87, 83
  br i1 %88, label %89, label %97

89:                                               ; preds = %82
  %90 = va_arg i8** %9, i32
  store i32 %90, i32* %17, align 4
  %91 = load i32, i32* %17, align 4
  %92 = trunc i32 %91 to i16
  %93 = load i32, i32* %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %94
  %96 = bitcast %union.jvalue* %95 to i16*
  store i16 %92, i16* %96, align 8
  br label %174

97:                                               ; preds = %82
  %98 = load i32, i32* %13, align 4
  %99 = sext i32 %98 to i64
  %100 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %99
  %101 = load i8, i8* %100, align 1
  %102 = sext i8 %101 to i32
  %103 = icmp eq i32 %102, 73
  br i1 %103, label %104, label %111

104:                                              ; preds = %97
  %105 = va_arg i8** %9, i32
  store i32 %105, i32* %18, align 4
  %106 = load i32, i32* %18, align 4
  %107 = load i32, i32* %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %108
  %110 = bitcast %union.jvalue* %109 to i32*
  store i32 %106, i32* %110, align 8
  br label %173

111:                                              ; preds = %97
  %112 = load i32, i32* %13, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %113
  %115 = load i8, i8* %114, align 1
  %116 = sext i8 %115 to i32
  %117 = icmp eq i32 %116, 74
  br i1 %117, label %118, label %125

118:                                              ; preds = %111
  %119 = va_arg i8** %9, i64
  store i64 %119, i64* %19, align 8
  %120 = load i64, i64* %19, align 8
  %121 = load i32, i32* %13, align 4
  %122 = sext i32 %121 to i64
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %120, i64* %124, align 8
  br label %172

125:                                              ; preds = %111
  %126 = load i32, i32* %13, align 4
  %127 = sext i32 %126 to i64
  %128 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %127
  %129 = load i8, i8* %128, align 1
  %130 = sext i8 %129 to i32
  %131 = icmp eq i32 %130, 70
  br i1 %131, label %132, label %140

132:                                              ; preds = %125
  %133 = va_arg i8** %9, double
  store double %133, double* %20, align 8
  %134 = load double, double* %20, align 8
  %135 = fptrunc double %134 to float
  %136 = load i32, i32* %13, align 4
  %137 = sext i32 %136 to i64
  %138 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %137
  %139 = bitcast %union.jvalue* %138 to float*
  store float %135, float* %139, align 8
  br label %171

140:                                              ; preds = %125
  %141 = load i32, i32* %13, align 4
  %142 = sext i32 %141 to i64
  %143 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %142
  %144 = load i8, i8* %143, align 1
  %145 = sext i8 %144 to i32
  %146 = icmp eq i32 %145, 68
  br i1 %146, label %147, label %154

147:                                              ; preds = %140
  %148 = va_arg i8** %9, double
  store double %148, double* %21, align 8
  %149 = load double, double* %21, align 8
  %150 = load i32, i32* %13, align 4
  %151 = sext i32 %150 to i64
  %152 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %151
  %153 = bitcast %union.jvalue* %152 to double*
  store double %149, double* %153, align 8
  br label %170

154:                                              ; preds = %140
  %155 = load i32, i32* %13, align 4
  %156 = sext i32 %155 to i64
  %157 = getelementptr inbounds [256 x i8], [256 x i8]* %10, i64 0, i64 %156
  %158 = load i8, i8* %157, align 1
  %159 = sext i8 %158 to i32
  %160 = icmp eq i32 %159, 76
  br i1 %160, label %161, label %169

161:                                              ; preds = %154
  %162 = va_arg i8** %9, i8*
  store i8* %162, i8** %22, align 8
  %163 = load i8*, i8** %22, align 8
  %164 = bitcast i8* %163 to %struct._jobject*
  %165 = load i32, i32* %13, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 %166
  %168 = bitcast %union.jvalue* %167 to %struct._jobject**
  store %struct._jobject* %164, %struct._jobject** %168, align 8
  br label %169

169:                                              ; preds = %161, %154
  br label %170

170:                                              ; preds = %169, %147
  br label %171

171:                                              ; preds = %170, %132
  br label %172

172:                                              ; preds = %171, %118
  br label %173

173:                                              ; preds = %172, %104
  br label %174

174:                                              ; preds = %173, %89
  br label %175

175:                                              ; preds = %174, %74
  br label %176

176:                                              ; preds = %175, %59
  br label %177

177:                                              ; preds = %176, %44
  br label %178

178:                                              ; preds = %177
  %179 = load i32, i32* %13, align 4
  %180 = add nsw i32 %179, 1
  store i32 %180, i32* %13, align 4
  br label %33, !llvm.loop !68

181:                                              ; preds = %33
  %182 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %183 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %182, align 8
  %184 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %183, i32 0, i32 93
  %185 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %184, align 8
  %186 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %187 = load %struct._jobject*, %struct._jobject** %6, align 8
  %188 = load %struct._jobject*, %struct._jobject** %7, align 8
  %189 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %190 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %11, i64 0, i64 0
  call void %185(%struct.JNINativeInterface_** noundef %186, %struct._jobject* noundef %187, %struct._jobject* noundef %188, %struct._jmethodID* noundef %189, %union.jvalue* noundef %190)
  %191 = bitcast i8** %9 to i8*
  call void @llvm.va_end(i8* %191)
  ret void
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define void @JNI_CallNonvirtualVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, i8* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca i8*, align 8
  %11 = alloca [256 x i8], align 1
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i32, align 4
  %19 = alloca i32, align 4
  %20 = alloca i64, align 8
  %21 = alloca double, align 8
  %22 = alloca double, align 8
  %23 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  store i8* %4, i8** %10, align 8
  %24 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %25 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %25, i32 0, i32 0
  %27 = load i8*, i8** %26, align 8
  %28 = bitcast i8* %27 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %29 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %30 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %31 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 0
  %32 = call i32 %28(%struct.JNINativeInterface_** noundef %29, %struct._jmethodID* noundef %30, i8* noundef %31)
  store i32 %32, i32* %13, align 4
  store i32 0, i32* %14, align 4
  br label %33

33:                                               ; preds = %178, %5
  %34 = load i32, i32* %14, align 4
  %35 = load i32, i32* %13, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %181

37:                                               ; preds = %33
  %38 = load i32, i32* %14, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %39
  %41 = load i8, i8* %40, align 1
  %42 = sext i8 %41 to i32
  %43 = icmp eq i32 %42, 90
  br i1 %43, label %44, label %52

44:                                               ; preds = %37
  %45 = va_arg i8** %10, i32
  store i32 %45, i32* %15, align 4
  %46 = load i32, i32* %15, align 4
  %47 = trunc i32 %46 to i8
  %48 = load i32, i32* %14, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %49
  %51 = bitcast %union.jvalue* %50 to i8*
  store i8 %47, i8* %51, align 8
  br label %177

52:                                               ; preds = %37
  %53 = load i32, i32* %14, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %54
  %56 = load i8, i8* %55, align 1
  %57 = sext i8 %56 to i32
  %58 = icmp eq i32 %57, 66
  br i1 %58, label %59, label %67

59:                                               ; preds = %52
  %60 = va_arg i8** %10, i32
  store i32 %60, i32* %16, align 4
  %61 = load i32, i32* %16, align 4
  %62 = trunc i32 %61 to i8
  %63 = load i32, i32* %14, align 4
  %64 = sext i32 %63 to i64
  %65 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %64
  %66 = bitcast %union.jvalue* %65 to i8*
  store i8 %62, i8* %66, align 8
  br label %176

67:                                               ; preds = %52
  %68 = load i32, i32* %14, align 4
  %69 = sext i32 %68 to i64
  %70 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %69
  %71 = load i8, i8* %70, align 1
  %72 = sext i8 %71 to i32
  %73 = icmp eq i32 %72, 67
  br i1 %73, label %74, label %82

74:                                               ; preds = %67
  %75 = va_arg i8** %10, i32
  store i32 %75, i32* %17, align 4
  %76 = load i32, i32* %17, align 4
  %77 = trunc i32 %76 to i16
  %78 = load i32, i32* %14, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %79
  %81 = bitcast %union.jvalue* %80 to i16*
  store i16 %77, i16* %81, align 8
  br label %175

82:                                               ; preds = %67
  %83 = load i32, i32* %14, align 4
  %84 = sext i32 %83 to i64
  %85 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %84
  %86 = load i8, i8* %85, align 1
  %87 = sext i8 %86 to i32
  %88 = icmp eq i32 %87, 83
  br i1 %88, label %89, label %97

89:                                               ; preds = %82
  %90 = va_arg i8** %10, i32
  store i32 %90, i32* %18, align 4
  %91 = load i32, i32* %18, align 4
  %92 = trunc i32 %91 to i16
  %93 = load i32, i32* %14, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %94
  %96 = bitcast %union.jvalue* %95 to i16*
  store i16 %92, i16* %96, align 8
  br label %174

97:                                               ; preds = %82
  %98 = load i32, i32* %14, align 4
  %99 = sext i32 %98 to i64
  %100 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %99
  %101 = load i8, i8* %100, align 1
  %102 = sext i8 %101 to i32
  %103 = icmp eq i32 %102, 73
  br i1 %103, label %104, label %111

104:                                              ; preds = %97
  %105 = va_arg i8** %10, i32
  store i32 %105, i32* %19, align 4
  %106 = load i32, i32* %19, align 4
  %107 = load i32, i32* %14, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %108
  %110 = bitcast %union.jvalue* %109 to i32*
  store i32 %106, i32* %110, align 8
  br label %173

111:                                              ; preds = %97
  %112 = load i32, i32* %14, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %113
  %115 = load i8, i8* %114, align 1
  %116 = sext i8 %115 to i32
  %117 = icmp eq i32 %116, 74
  br i1 %117, label %118, label %125

118:                                              ; preds = %111
  %119 = va_arg i8** %10, i64
  store i64 %119, i64* %20, align 8
  %120 = load i64, i64* %20, align 8
  %121 = load i32, i32* %14, align 4
  %122 = sext i32 %121 to i64
  %123 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %122
  %124 = bitcast %union.jvalue* %123 to i64*
  store i64 %120, i64* %124, align 8
  br label %172

125:                                              ; preds = %111
  %126 = load i32, i32* %14, align 4
  %127 = sext i32 %126 to i64
  %128 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %127
  %129 = load i8, i8* %128, align 1
  %130 = sext i8 %129 to i32
  %131 = icmp eq i32 %130, 70
  br i1 %131, label %132, label %140

132:                                              ; preds = %125
  %133 = va_arg i8** %10, double
  store double %133, double* %21, align 8
  %134 = load double, double* %21, align 8
  %135 = fptrunc double %134 to float
  %136 = load i32, i32* %14, align 4
  %137 = sext i32 %136 to i64
  %138 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %137
  %139 = bitcast %union.jvalue* %138 to float*
  store float %135, float* %139, align 8
  br label %171

140:                                              ; preds = %125
  %141 = load i32, i32* %14, align 4
  %142 = sext i32 %141 to i64
  %143 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %142
  %144 = load i8, i8* %143, align 1
  %145 = sext i8 %144 to i32
  %146 = icmp eq i32 %145, 68
  br i1 %146, label %147, label %154

147:                                              ; preds = %140
  %148 = va_arg i8** %10, double
  store double %148, double* %22, align 8
  %149 = load double, double* %22, align 8
  %150 = load i32, i32* %14, align 4
  %151 = sext i32 %150 to i64
  %152 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %151
  %153 = bitcast %union.jvalue* %152 to double*
  store double %149, double* %153, align 8
  br label %170

154:                                              ; preds = %140
  %155 = load i32, i32* %14, align 4
  %156 = sext i32 %155 to i64
  %157 = getelementptr inbounds [256 x i8], [256 x i8]* %11, i64 0, i64 %156
  %158 = load i8, i8* %157, align 1
  %159 = sext i8 %158 to i32
  %160 = icmp eq i32 %159, 76
  br i1 %160, label %161, label %169

161:                                              ; preds = %154
  %162 = va_arg i8** %10, i8*
  store i8* %162, i8** %23, align 8
  %163 = load i8*, i8** %23, align 8
  %164 = bitcast i8* %163 to %struct._jobject*
  %165 = load i32, i32* %14, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 %166
  %168 = bitcast %union.jvalue* %167 to %struct._jobject**
  store %struct._jobject* %164, %struct._jobject** %168, align 8
  br label %169

169:                                              ; preds = %161, %154
  br label %170

170:                                              ; preds = %169, %147
  br label %171

171:                                              ; preds = %170, %132
  br label %172

172:                                              ; preds = %171, %118
  br label %173

173:                                              ; preds = %172, %104
  br label %174

174:                                              ; preds = %173, %89
  br label %175

175:                                              ; preds = %174, %74
  br label %176

176:                                              ; preds = %175, %59
  br label %177

177:                                              ; preds = %176, %44
  br label %178

178:                                              ; preds = %177
  %179 = load i32, i32* %14, align 4
  %180 = add nsw i32 %179, 1
  store i32 %180, i32* %14, align 4
  br label %33, !llvm.loop !69

181:                                              ; preds = %33
  %182 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %183 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %182, align 8
  %184 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %183, i32 0, i32 93
  %185 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %184, align 8
  %186 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %187 = load %struct._jobject*, %struct._jobject** %7, align 8
  %188 = load %struct._jobject*, %struct._jobject** %8, align 8
  %189 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %190 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %12, i64 0, i64 0
  call void %185(%struct.JNINativeInterface_** noundef %186, %struct._jobject* noundef %187, %struct._jobject* noundef %188, %struct._jmethodID* noundef %189, %union.jvalue* noundef %190)
  ret void
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define void @JNI_CallStaticVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8*, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca [256 x %union.jvalue], align 8
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i64, align 8
  %18 = alloca double, align 8
  %19 = alloca double, align 8
  %20 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %21 = bitcast i8** %7 to i8*
  call void @llvm.va_start(i8* %21)
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %10, align 4
  store i32 0, i32* %11, align 4
  br label %31

31:                                               ; preds = %176, %3
  %32 = load i32, i32* %11, align 4
  %33 = load i32, i32* %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %11, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %7, i32
  store i32 %43, i32* %12, align 4
  %44 = load i32, i32* %12, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %11, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %11, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %7, i32
  store i32 %58, i32* %13, align 4
  %59 = load i32, i32* %13, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %11, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %11, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %7, i32
  store i32 %73, i32* %14, align 4
  %74 = load i32, i32* %14, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %11, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %11, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %7, i32
  store i32 %88, i32* %15, align 4
  %89 = load i32, i32* %15, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %11, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %11, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %7, i32
  store i32 %103, i32* %16, align 4
  %104 = load i32, i32* %16, align 4
  %105 = load i32, i32* %11, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %11, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %7, i64
  store i64 %117, i64* %17, align 8
  %118 = load i64, i64* %17, align 8
  %119 = load i32, i32* %11, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %11, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %7, double
  store double %131, double* %18, align 8
  %132 = load double, double* %18, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %11, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %11, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %7, double
  store double %146, double* %19, align 8
  %147 = load double, double* %19, align 8
  %148 = load i32, i32* %11, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %11, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %8, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %7, i8*
  store i8* %160, i8** %20, align 8
  %161 = load i8*, i8** %20, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %11, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %11, align 4
  br label %31, !llvm.loop !70

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 143
  %183 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %185 = load %struct._jobject*, %struct._jobject** %5, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %9, i64 0, i64 0
  call void %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  %188 = bitcast i8** %7 to i8*
  call void @llvm.va_end(i8* %188)
  ret void
}

; Function Attrs: noinline nounwind optnone ssp uwtable
define void @JNI_CallStaticVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, i8* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  %15 = alloca i32, align 4
  %16 = alloca i32, align 4
  %17 = alloca i32, align 4
  %18 = alloca i64, align 8
  %19 = alloca double, align 8
  %20 = alloca double, align 8
  %21 = alloca i8*, align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  store i8* %3, i8** %8, align 8
  %22 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %23 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %23, i32 0, i32 0
  %25 = load i8*, i8** %24, align 8
  %26 = bitcast i8* %25 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %27 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %28 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %29 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 0
  %30 = call i32 %26(%struct.JNINativeInterface_** noundef %27, %struct._jmethodID* noundef %28, i8* noundef %29)
  store i32 %30, i32* %11, align 4
  store i32 0, i32* %12, align 4
  br label %31

31:                                               ; preds = %176, %4
  %32 = load i32, i32* %12, align 4
  %33 = load i32, i32* %11, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %179

35:                                               ; preds = %31
  %36 = load i32, i32* %12, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %37
  %39 = load i8, i8* %38, align 1
  %40 = sext i8 %39 to i32
  %41 = icmp eq i32 %40, 90
  br i1 %41, label %42, label %50

42:                                               ; preds = %35
  %43 = va_arg i8** %8, i32
  store i32 %43, i32* %13, align 4
  %44 = load i32, i32* %13, align 4
  %45 = trunc i32 %44 to i8
  %46 = load i32, i32* %12, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %47
  %49 = bitcast %union.jvalue* %48 to i8*
  store i8 %45, i8* %49, align 8
  br label %175

50:                                               ; preds = %35
  %51 = load i32, i32* %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %52
  %54 = load i8, i8* %53, align 1
  %55 = sext i8 %54 to i32
  %56 = icmp eq i32 %55, 66
  br i1 %56, label %57, label %65

57:                                               ; preds = %50
  %58 = va_arg i8** %8, i32
  store i32 %58, i32* %14, align 4
  %59 = load i32, i32* %14, align 4
  %60 = trunc i32 %59 to i8
  %61 = load i32, i32* %12, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %60, i8* %64, align 8
  br label %174

65:                                               ; preds = %50
  %66 = load i32, i32* %12, align 4
  %67 = sext i32 %66 to i64
  %68 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %67
  %69 = load i8, i8* %68, align 1
  %70 = sext i8 %69 to i32
  %71 = icmp eq i32 %70, 67
  br i1 %71, label %72, label %80

72:                                               ; preds = %65
  %73 = va_arg i8** %8, i32
  store i32 %73, i32* %15, align 4
  %74 = load i32, i32* %15, align 4
  %75 = trunc i32 %74 to i16
  %76 = load i32, i32* %12, align 4
  %77 = sext i32 %76 to i64
  %78 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %77
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %75, i16* %79, align 8
  br label %173

80:                                               ; preds = %65
  %81 = load i32, i32* %12, align 4
  %82 = sext i32 %81 to i64
  %83 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %82
  %84 = load i8, i8* %83, align 1
  %85 = sext i8 %84 to i32
  %86 = icmp eq i32 %85, 83
  br i1 %86, label %87, label %95

87:                                               ; preds = %80
  %88 = va_arg i8** %8, i32
  store i32 %88, i32* %16, align 4
  %89 = load i32, i32* %16, align 4
  %90 = trunc i32 %89 to i16
  %91 = load i32, i32* %12, align 4
  %92 = sext i32 %91 to i64
  %93 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %92
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %90, i16* %94, align 8
  br label %172

95:                                               ; preds = %80
  %96 = load i32, i32* %12, align 4
  %97 = sext i32 %96 to i64
  %98 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %97
  %99 = load i8, i8* %98, align 1
  %100 = sext i8 %99 to i32
  %101 = icmp eq i32 %100, 73
  br i1 %101, label %102, label %109

102:                                              ; preds = %95
  %103 = va_arg i8** %8, i32
  store i32 %103, i32* %17, align 4
  %104 = load i32, i32* %17, align 4
  %105 = load i32, i32* %12, align 4
  %106 = sext i32 %105 to i64
  %107 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %106
  %108 = bitcast %union.jvalue* %107 to i32*
  store i32 %104, i32* %108, align 8
  br label %171

109:                                              ; preds = %95
  %110 = load i32, i32* %12, align 4
  %111 = sext i32 %110 to i64
  %112 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %111
  %113 = load i8, i8* %112, align 1
  %114 = sext i8 %113 to i32
  %115 = icmp eq i32 %114, 74
  br i1 %115, label %116, label %123

116:                                              ; preds = %109
  %117 = va_arg i8** %8, i64
  store i64 %117, i64* %18, align 8
  %118 = load i64, i64* %18, align 8
  %119 = load i32, i32* %12, align 4
  %120 = sext i32 %119 to i64
  %121 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %120
  %122 = bitcast %union.jvalue* %121 to i64*
  store i64 %118, i64* %122, align 8
  br label %170

123:                                              ; preds = %109
  %124 = load i32, i32* %12, align 4
  %125 = sext i32 %124 to i64
  %126 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %125
  %127 = load i8, i8* %126, align 1
  %128 = sext i8 %127 to i32
  %129 = icmp eq i32 %128, 70
  br i1 %129, label %130, label %138

130:                                              ; preds = %123
  %131 = va_arg i8** %8, double
  store double %131, double* %19, align 8
  %132 = load double, double* %19, align 8
  %133 = fptrunc double %132 to float
  %134 = load i32, i32* %12, align 4
  %135 = sext i32 %134 to i64
  %136 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %135
  %137 = bitcast %union.jvalue* %136 to float*
  store float %133, float* %137, align 8
  br label %169

138:                                              ; preds = %123
  %139 = load i32, i32* %12, align 4
  %140 = sext i32 %139 to i64
  %141 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %140
  %142 = load i8, i8* %141, align 1
  %143 = sext i8 %142 to i32
  %144 = icmp eq i32 %143, 68
  br i1 %144, label %145, label %152

145:                                              ; preds = %138
  %146 = va_arg i8** %8, double
  store double %146, double* %20, align 8
  %147 = load double, double* %20, align 8
  %148 = load i32, i32* %12, align 4
  %149 = sext i32 %148 to i64
  %150 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %149
  %151 = bitcast %union.jvalue* %150 to double*
  store double %147, double* %151, align 8
  br label %168

152:                                              ; preds = %138
  %153 = load i32, i32* %12, align 4
  %154 = sext i32 %153 to i64
  %155 = getelementptr inbounds [256 x i8], [256 x i8]* %9, i64 0, i64 %154
  %156 = load i8, i8* %155, align 1
  %157 = sext i8 %156 to i32
  %158 = icmp eq i32 %157, 76
  br i1 %158, label %159, label %167

159:                                              ; preds = %152
  %160 = va_arg i8** %8, i8*
  store i8* %160, i8** %21, align 8
  %161 = load i8*, i8** %21, align 8
  %162 = bitcast i8* %161 to %struct._jobject*
  %163 = load i32, i32* %12, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 %164
  %166 = bitcast %union.jvalue* %165 to %struct._jobject**
  store %struct._jobject* %162, %struct._jobject** %166, align 8
  br label %167

167:                                              ; preds = %159, %152
  br label %168

168:                                              ; preds = %167, %145
  br label %169

169:                                              ; preds = %168, %130
  br label %170

170:                                              ; preds = %169, %116
  br label %171

171:                                              ; preds = %170, %102
  br label %172

172:                                              ; preds = %171, %87
  br label %173

173:                                              ; preds = %172, %72
  br label %174

174:                                              ; preds = %173, %57
  br label %175

175:                                              ; preds = %174, %42
  br label %176

176:                                              ; preds = %175
  %177 = load i32, i32* %12, align 4
  %178 = add nsw i32 %177, 1
  store i32 %178, i32* %12, align 4
  br label %31, !llvm.loop !71

179:                                              ; preds = %31
  %180 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %180, align 8
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i32 0, i32 143
  %183 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8
  %184 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %185 = load %struct._jobject*, %struct._jobject** %6, align 8
  %186 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %187 = getelementptr inbounds [256 x %union.jvalue], [256 x %union.jvalue]* %10, i64 0, i64 0
  call void %183(%struct.JNINativeInterface_** noundef %184, %struct._jobject* noundef %185, %struct._jmethodID* noundef %186, %union.jvalue* noundef %187)
  ret void
}

attributes #0 = { noinline nounwind optnone ssp uwtable "frame-pointer"="non-leaf" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="apple-m1" "target-features"="+aes,+crc,+crypto,+dotprod,+fp-armv8,+fp16fml,+fullfp16,+lse,+neon,+ras,+rcpc,+rdm,+sha2,+v8.5a,+zcm,+zcz" }
attributes #1 = { nofree nosync nounwind willreturn }

!llvm.module.flags = !{!0, !1, !2, !3, !4, !5, !6, !7}
!llvm.ident = !{!8}

!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 1, !"branch-target-enforcement", i32 0}
!2 = !{i32 1, !"sign-return-address", i32 0}
!3 = !{i32 1, !"sign-return-address-all", i32 0}
!4 = !{i32 1, !"sign-return-address-with-bkey", i32 0}
!5 = !{i32 7, !"PIC Level", i32 2}
!6 = !{i32 7, !"uwtable", i32 1}
!7 = !{i32 7, !"frame-pointer", i32 1}
!8 = !{!"Ubuntu clang version 14.0.0-1ubuntu1"}
!9 = distinct !{!9, !10}
!10 = !{!"llvm.loop.mustprogress"}
!11 = distinct !{!11, !10}
!12 = distinct !{!12, !10}
!13 = distinct !{!13, !10}
!14 = distinct !{!14, !10}
!15 = distinct !{!15, !10}
!16 = distinct !{!16, !10}
!17 = distinct !{!17, !10}
!18 = distinct !{!18, !10}
!19 = distinct !{!19, !10}
!20 = distinct !{!20, !10}
!21 = distinct !{!21, !10}
!22 = distinct !{!22, !10}
!23 = distinct !{!23, !10}
!24 = distinct !{!24, !10}
!25 = distinct !{!25, !10}
!26 = distinct !{!26, !10}
!27 = distinct !{!27, !10}
!28 = distinct !{!28, !10}
!29 = distinct !{!29, !10}
!30 = distinct !{!30, !10}
!31 = distinct !{!31, !10}
!32 = distinct !{!32, !10}
!33 = distinct !{!33, !10}
!34 = distinct !{!34, !10}
!35 = distinct !{!35, !10}
!36 = distinct !{!36, !10}
!37 = distinct !{!37, !10}
!38 = distinct !{!38, !10}
!39 = distinct !{!39, !10}
!40 = distinct !{!40, !10}
!41 = distinct !{!41, !10}
!42 = distinct !{!42, !10}
!43 = distinct !{!43, !10}
!44 = distinct !{!44, !10}
!45 = distinct !{!45, !10}
!46 = distinct !{!46, !10}
!47 = distinct !{!47, !10}
!48 = distinct !{!48, !10}
!49 = distinct !{!49, !10}
!50 = distinct !{!50, !10}
!51 = distinct !{!51, !10}
!52 = distinct !{!52, !10}
!53 = distinct !{!53, !10}
!54 = distinct !{!54, !10}
!55 = distinct !{!55, !10}
!56 = distinct !{!56, !10}
!57 = distinct !{!57, !10}
!58 = distinct !{!58, !10}
!59 = distinct !{!59, !10}
!60 = distinct !{!60, !10}
!61 = distinct !{!61, !10}
!62 = distinct !{!62, !10}
!63 = distinct !{!63, !10}
!64 = distinct !{!64, !10}
!65 = distinct !{!65, !10}
!66 = distinct !{!66, !10}
!67 = distinct !{!67, !10}
!68 = distinct !{!68, !10}
!69 = distinct !{!69, !10}
!70 = distinct !{!70, !10}
!71 = distinct !{!71, !10}
