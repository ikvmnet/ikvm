; ModuleID = 'jni.c'
source_filename = "jni.c"
target datalayout = "e-m:e-i8:8:32-i16:16:32-i64:64-i128:128-n32:64-S128"
target triple = "aarch64-pc-linux-gnueabihf"

%struct.JNINativeInterface_ = type { i8*, i8*, i8*, i8*, i32 (%struct.JNINativeInterface_**)*, %struct._jobject* (%struct.JNINativeInterface_**, i8*, %struct._jobject*, i8*, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i8*)*, %struct._jmethodID* (%struct.JNINativeInterface_**, %struct._jobject*)*, %struct._jfieldID* (%struct.JNINativeInterface_**, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, i8)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i8)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, %struct._jobject* (%struct.JNINativeInterface_**)*, void (%struct.JNINativeInterface_**)*, void (%struct.JNINativeInterface_**)*, void (%struct.JNINativeInterface_**, i8*)*, i32 (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, void (%struct.JNINativeInterface_**, %struct._jobject*)*, void (%struct.JNINativeInterface_**, %struct._jobject*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, {}*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*)*, %struct._jmethodID* (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i8*)*, {}*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, ...)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jfieldID* (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i8*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, %struct._jobject*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i8)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i8)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i16)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i16)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i64)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, float)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, double)*, %struct._jmethodID* (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i8*)*, {}*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, ...)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jfieldID* (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i8*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, %struct._jobject*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i8)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i8)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i16)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i16)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, i64)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, float)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jfieldID*, double)*, %struct._jobject* (%struct.JNINativeInterface_**, i16*, i32)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i16* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i16*)*, %struct._jobject* (%struct.JNINativeInterface_**, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i8* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, i32, %struct._jobject*, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, %struct._jobject*)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, %struct._jobject* (%struct.JNINativeInterface_**, i32)*, i8* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i8* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i16* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i16* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i32* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, i64* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, float* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, double* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i16*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i16*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i64*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, float*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, double*, i32)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i16*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i16*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i32*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i64*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, float*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, double*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i16*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i16*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i32*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i64*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, float*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, double*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct.JNINativeMethod*, i32)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, %struct.JNIInvokeInterface_***)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i16*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i32, i32, i8*)*, i8* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i8*, i32)*, i16* (%struct.JNINativeInterface_**, %struct._jobject*, i8*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, i16*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*)*, void (%struct.JNINativeInterface_**, %struct._jobject*)*, i8 (%struct.JNINativeInterface_**)*, %struct._jobject* (%struct.JNINativeInterface_**, i8*, i64)*, i8* (%struct.JNINativeInterface_**, %struct._jobject*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*)* }
%struct._jfieldID = type opaque
%"struct.std::__va_list" = type { i8*, i8*, i8*, i32, i32 }
%union.jvalue = type { i64 }
%struct.JNINativeMethod = type { i8*, i8*, i8* }
%struct.JNIInvokeInterface_ = type { i8*, i8*, i8*, i32 (%struct.JNIInvokeInterface_**)*, i32 (%struct.JNIInvokeInterface_**, i8**, i8*)*, i32 (%struct.JNIInvokeInterface_**)*, i32 (%struct.JNIInvokeInterface_**, i8**, i32)*, i32 (%struct.JNIInvokeInterface_**, i8**, i8*)* }
%struct._jobject = type opaque
%struct._jmethodID = type opaque

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_CallObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !24

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 36
  %219 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !26
  %220 = call %struct._jobject* %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret %struct._jobject* %220
}

; Function Attrs: argmemonly mustprogress nofree nosync nounwind willreturn
declare void @llvm.lifetime.start.p0i8(i64 immarg, i8* nocapture) #1

; Function Attrs: mustprogress nofree nosync nounwind willreturn
declare void @llvm.va_start(i8*) #2

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_CallObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !24

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 36
  %206 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !26
  %207 = call %struct._jobject* %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret %struct._jobject* %207
}

; Function Attrs: argmemonly mustprogress nofree nosync nounwind willreturn
declare void @llvm.lifetime.end.p0i8(i64 immarg, i8* nocapture) #1

; Function Attrs: mustprogress nofree nosync nounwind willreturn
declare void @llvm.va_end(i8*) #2

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_CallNonvirtualObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca %"struct.std::__va_list", align 8
  %7 = bitcast %"struct.std::__va_list"* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 0
  %9 = load i8*, i8** %8, align 8, !tbaa.struct !10
  %10 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 1
  %11 = load i8*, i8** %10, align 8, !tbaa.struct !17
  %12 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 2
  %13 = load i8*, i8** %12, align 8, !tbaa.struct !18
  %14 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 3
  %15 = load i32, i32* %14, align 8, !tbaa.struct !19
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 4
  %17 = load i32, i32* %16, align 4, !tbaa.struct !20
  %18 = call i8* @llvm.stacksave()
  %19 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %19) #4
  %20 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %20, align 8, !tbaa !11
  %22 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %21, align 8, !tbaa !21
  %23 = call i32 %22(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %19) #4
  %24 = sext i32 %23 to i64
  %25 = alloca %union.jvalue, i64 %24, align 16
  %26 = icmp sgt i32 %23, 0
  br i1 %26, label %27, label %217

27:                                               ; preds = %4
  %28 = zext i32 %23 to i64
  br label %29

29:                                               ; preds = %211, %27
  %30 = phi i32 [ %15, %27 ], [ %212, %211 ]
  %31 = phi i32 [ %17, %27 ], [ %213, %211 ]
  %32 = phi i8* [ %9, %27 ], [ %214, %211 ]
  %33 = phi i64 [ 0, %27 ], [ %215, %211 ]
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1, !tbaa !23
  switch i8 %35, label %211 [
    i8 90, label %36
    i8 66, label %56
    i8 83, label %76
    i8 67, label %96
    i8 73, label %116
    i8 74, label %135
    i8 68, label %153
    i8 70, label %172
    i8 76, label %192
  ]

36:                                               ; preds = %29
  %37 = icmp sgt i32 %30, -1
  br i1 %37, label %44, label %38

38:                                               ; preds = %36
  %39 = add nsw i32 %30, 8
  %40 = icmp ult i32 %30, -7
  br i1 %40, label %41, label %44

41:                                               ; preds = %38
  %42 = sext i32 %30 to i64
  %43 = getelementptr inbounds i8, i8* %11, i64 %42
  br label %47

44:                                               ; preds = %38, %36
  %45 = phi i32 [ %30, %36 ], [ %39, %38 ]
  %46 = getelementptr inbounds i8, i8* %32, i64 8
  br label %47

47:                                               ; preds = %44, %41
  %48 = phi i32 [ %45, %44 ], [ %39, %41 ]
  %49 = phi i8* [ %46, %44 ], [ %32, %41 ]
  %50 = phi i8* [ %32, %44 ], [ %43, %41 ]
  %51 = bitcast i8* %50 to i32*
  %52 = load i32, i32* %51, align 8
  %53 = trunc i32 %52 to i8
  %54 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %55 = bitcast %union.jvalue* %54 to i8*
  store i8 %53, i8* %55, align 8, !tbaa !23
  br label %211

56:                                               ; preds = %29
  %57 = icmp sgt i32 %30, -1
  br i1 %57, label %64, label %58

58:                                               ; preds = %56
  %59 = add nsw i32 %30, 8
  %60 = icmp ult i32 %30, -7
  br i1 %60, label %61, label %64

61:                                               ; preds = %58
  %62 = sext i32 %30 to i64
  %63 = getelementptr inbounds i8, i8* %11, i64 %62
  br label %67

64:                                               ; preds = %58, %56
  %65 = phi i32 [ %30, %56 ], [ %59, %58 ]
  %66 = getelementptr inbounds i8, i8* %32, i64 8
  br label %67

67:                                               ; preds = %64, %61
  %68 = phi i32 [ %65, %64 ], [ %59, %61 ]
  %69 = phi i8* [ %66, %64 ], [ %32, %61 ]
  %70 = phi i8* [ %32, %64 ], [ %63, %61 ]
  %71 = bitcast i8* %70 to i32*
  %72 = load i32, i32* %71, align 8
  %73 = trunc i32 %72 to i8
  %74 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %75 = bitcast %union.jvalue* %74 to i8*
  store i8 %73, i8* %75, align 8, !tbaa !23
  br label %211

76:                                               ; preds = %29
  %77 = icmp sgt i32 %30, -1
  br i1 %77, label %84, label %78

78:                                               ; preds = %76
  %79 = add nsw i32 %30, 8
  %80 = icmp ult i32 %30, -7
  br i1 %80, label %81, label %84

81:                                               ; preds = %78
  %82 = sext i32 %30 to i64
  %83 = getelementptr inbounds i8, i8* %11, i64 %82
  br label %87

84:                                               ; preds = %78, %76
  %85 = phi i32 [ %30, %76 ], [ %79, %78 ]
  %86 = getelementptr inbounds i8, i8* %32, i64 8
  br label %87

87:                                               ; preds = %84, %81
  %88 = phi i32 [ %85, %84 ], [ %79, %81 ]
  %89 = phi i8* [ %86, %84 ], [ %32, %81 ]
  %90 = phi i8* [ %32, %84 ], [ %83, %81 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 8
  %93 = trunc i32 %92 to i16
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %93, i16* %95, align 8, !tbaa !23
  br label %211

96:                                               ; preds = %29
  %97 = icmp sgt i32 %30, -1
  br i1 %97, label %104, label %98

98:                                               ; preds = %96
  %99 = add nsw i32 %30, 8
  %100 = icmp ult i32 %30, -7
  br i1 %100, label %101, label %104

101:                                              ; preds = %98
  %102 = sext i32 %30 to i64
  %103 = getelementptr inbounds i8, i8* %11, i64 %102
  br label %107

104:                                              ; preds = %98, %96
  %105 = phi i32 [ %30, %96 ], [ %99, %98 ]
  %106 = getelementptr inbounds i8, i8* %32, i64 8
  br label %107

107:                                              ; preds = %104, %101
  %108 = phi i32 [ %105, %104 ], [ %99, %101 ]
  %109 = phi i8* [ %106, %104 ], [ %32, %101 ]
  %110 = phi i8* [ %32, %104 ], [ %103, %101 ]
  %111 = bitcast i8* %110 to i32*
  %112 = load i32, i32* %111, align 8
  %113 = and i32 %112, 65535
  %114 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %115 = bitcast %union.jvalue* %114 to i32*
  store i32 %113, i32* %115, align 8, !tbaa !23
  br label %211

116:                                              ; preds = %29
  %117 = icmp sgt i32 %30, -1
  br i1 %117, label %124, label %118

118:                                              ; preds = %116
  %119 = add nsw i32 %30, 8
  %120 = icmp ult i32 %30, -7
  br i1 %120, label %121, label %124

121:                                              ; preds = %118
  %122 = sext i32 %30 to i64
  %123 = getelementptr inbounds i8, i8* %11, i64 %122
  br label %127

124:                                              ; preds = %118, %116
  %125 = phi i32 [ %30, %116 ], [ %119, %118 ]
  %126 = getelementptr inbounds i8, i8* %32, i64 8
  br label %127

127:                                              ; preds = %124, %121
  %128 = phi i32 [ %125, %124 ], [ %119, %121 ]
  %129 = phi i8* [ %126, %124 ], [ %32, %121 ]
  %130 = phi i8* [ %32, %124 ], [ %123, %121 ]
  %131 = bitcast i8* %130 to i32*
  %132 = load i32, i32* %131, align 8
  %133 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %134 = bitcast %union.jvalue* %133 to i32*
  store i32 %132, i32* %134, align 8, !tbaa !23
  br label %211

135:                                              ; preds = %29
  %136 = icmp sgt i32 %30, -1
  br i1 %136, label %143, label %137

137:                                              ; preds = %135
  %138 = add nsw i32 %30, 8
  %139 = icmp ult i32 %30, -7
  br i1 %139, label %140, label %143

140:                                              ; preds = %137
  %141 = sext i32 %30 to i64
  %142 = getelementptr inbounds i8, i8* %11, i64 %141
  br label %146

143:                                              ; preds = %137, %135
  %144 = phi i32 [ %30, %135 ], [ %138, %137 ]
  %145 = getelementptr inbounds i8, i8* %32, i64 8
  br label %146

146:                                              ; preds = %143, %140
  %147 = phi i32 [ %144, %143 ], [ %138, %140 ]
  %148 = phi i8* [ %145, %143 ], [ %32, %140 ]
  %149 = phi i8* [ %32, %143 ], [ %142, %140 ]
  %150 = bitcast i8* %149 to i64*
  %151 = load i64, i64* %150, align 8
  %152 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33, i32 0
  store i64 %151, i64* %152, align 8, !tbaa !23
  br label %211

153:                                              ; preds = %29
  %154 = icmp sgt i32 %31, -1
  br i1 %154, label %161, label %155

155:                                              ; preds = %153
  %156 = add nsw i32 %31, 16
  %157 = icmp ult i32 %31, -15
  br i1 %157, label %158, label %161

158:                                              ; preds = %155
  %159 = sext i32 %31 to i64
  %160 = getelementptr inbounds i8, i8* %13, i64 %159
  br label %164

161:                                              ; preds = %155, %153
  %162 = phi i32 [ %31, %153 ], [ %156, %155 ]
  %163 = getelementptr inbounds i8, i8* %32, i64 8
  br label %164

164:                                              ; preds = %161, %158
  %165 = phi i32 [ %162, %161 ], [ %156, %158 ]
  %166 = phi i8* [ %163, %161 ], [ %32, %158 ]
  %167 = phi i8* [ %32, %161 ], [ %160, %158 ]
  %168 = bitcast i8* %167 to double*
  %169 = load double, double* %168, align 8
  %170 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %171 = bitcast %union.jvalue* %170 to double*
  store double %169, double* %171, align 8, !tbaa !23
  br label %211

172:                                              ; preds = %29
  %173 = icmp sgt i32 %31, -1
  br i1 %173, label %180, label %174

174:                                              ; preds = %172
  %175 = add nsw i32 %31, 16
  %176 = icmp ult i32 %31, -15
  br i1 %176, label %177, label %180

177:                                              ; preds = %174
  %178 = sext i32 %31 to i64
  %179 = getelementptr inbounds i8, i8* %13, i64 %178
  br label %183

180:                                              ; preds = %174, %172
  %181 = phi i32 [ %31, %172 ], [ %175, %174 ]
  %182 = getelementptr inbounds i8, i8* %32, i64 8
  br label %183

183:                                              ; preds = %180, %177
  %184 = phi i32 [ %181, %180 ], [ %175, %177 ]
  %185 = phi i8* [ %182, %180 ], [ %32, %177 ]
  %186 = phi i8* [ %32, %180 ], [ %179, %177 ]
  %187 = bitcast i8* %186 to double*
  %188 = load double, double* %187, align 8
  %189 = fptrunc double %188 to float
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %191 = bitcast %union.jvalue* %190 to float*
  store float %189, float* %191, align 8, !tbaa !23
  br label %211

192:                                              ; preds = %29
  %193 = icmp sgt i32 %30, -1
  br i1 %193, label %200, label %194

194:                                              ; preds = %192
  %195 = add nsw i32 %30, 8
  %196 = icmp ult i32 %30, -7
  br i1 %196, label %197, label %200

197:                                              ; preds = %194
  %198 = sext i32 %30 to i64
  %199 = getelementptr inbounds i8, i8* %11, i64 %198
  br label %203

200:                                              ; preds = %194, %192
  %201 = phi i32 [ %30, %192 ], [ %195, %194 ]
  %202 = getelementptr inbounds i8, i8* %32, i64 8
  br label %203

203:                                              ; preds = %200, %197
  %204 = phi i32 [ %201, %200 ], [ %195, %197 ]
  %205 = phi i8* [ %202, %200 ], [ %32, %197 ]
  %206 = phi i8* [ %32, %200 ], [ %199, %197 ]
  %207 = bitcast i8* %206 to %struct._jobject**
  %208 = load %struct._jobject*, %struct._jobject** %207, align 8
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %210 = bitcast %union.jvalue* %209 to %struct._jobject**
  store %struct._jobject* %208, %struct._jobject** %210, align 8, !tbaa !23
  br label %211

211:                                              ; preds = %203, %183, %164, %146, %127, %107, %87, %67, %47, %29
  %212 = phi i32 [ %30, %29 ], [ %204, %203 ], [ %30, %183 ], [ %30, %164 ], [ %147, %146 ], [ %128, %127 ], [ %108, %107 ], [ %88, %87 ], [ %68, %67 ], [ %48, %47 ]
  %213 = phi i32 [ %31, %29 ], [ %31, %203 ], [ %184, %183 ], [ %165, %164 ], [ %31, %146 ], [ %31, %127 ], [ %31, %107 ], [ %31, %87 ], [ %31, %67 ], [ %31, %47 ]
  %214 = phi i8* [ %32, %29 ], [ %205, %203 ], [ %185, %183 ], [ %166, %164 ], [ %148, %146 ], [ %129, %127 ], [ %109, %107 ], [ %89, %87 ], [ %69, %67 ], [ %49, %47 ]
  %215 = add nuw nsw i64 %33, 1
  %216 = icmp eq i64 %215, %28
  br i1 %216, label %217, label %29, !llvm.loop !27

217:                                              ; preds = %211, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %19) #4
  %218 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %219 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %218, i64 0, i32 66
  %220 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %219, align 8, !tbaa !28
  %221 = call %struct._jobject* %220(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %25) #4
  call void @llvm.stackrestore(i8* %18)
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %7) #4
  ret %struct._jobject* %221
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_CallNonvirtualObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !11
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !21
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #4
  %12 = sext i32 %11 to i64
  %13 = alloca %union.jvalue, i64 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %204

15:                                               ; preds = %5
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 3
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 1
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 0
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 4
  %20 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 2
  %21 = zext i32 %11 to i64
  br label %22

22:                                               ; preds = %15, %201
  %23 = phi i64 [ 0, %15 ], [ %202, %201 ]
  %24 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 %23
  %25 = load i8, i8* %24, align 1, !tbaa !23
  switch i8 %25, label %201 [
    i8 90, label %26
    i8 66, label %46
    i8 83, label %66
    i8 67, label %86
    i8 73, label %106
    i8 74, label %125
    i8 68, label %143
    i8 70, label %162
    i8 76, label %182
  ]

26:                                               ; preds = %22
  %27 = load i32, i32* %16, align 8
  %28 = icmp sgt i32 %27, -1
  br i1 %28, label %36, label %29

29:                                               ; preds = %26
  %30 = add nsw i32 %27, 8
  store i32 %30, i32* %16, align 8
  %31 = icmp ult i32 %27, -7
  br i1 %31, label %32, label %36

32:                                               ; preds = %29
  %33 = load i8*, i8** %17, align 8
  %34 = sext i32 %27 to i64
  %35 = getelementptr inbounds i8, i8* %33, i64 %34
  br label %39

36:                                               ; preds = %29, %26
  %37 = load i8*, i8** %18, align 8
  %38 = getelementptr inbounds i8, i8* %37, i64 8
  store i8* %38, i8** %18, align 8
  br label %39

39:                                               ; preds = %36, %32
  %40 = phi i8* [ %35, %32 ], [ %37, %36 ]
  %41 = bitcast i8* %40 to i32*
  %42 = load i32, i32* %41, align 8
  %43 = trunc i32 %42 to i8
  %44 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %43, i8* %45, align 8, !tbaa !23
  br label %201

46:                                               ; preds = %22
  %47 = load i32, i32* %16, align 8
  %48 = icmp sgt i32 %47, -1
  br i1 %48, label %56, label %49

49:                                               ; preds = %46
  %50 = add nsw i32 %47, 8
  store i32 %50, i32* %16, align 8
  %51 = icmp ult i32 %47, -7
  br i1 %51, label %52, label %56

52:                                               ; preds = %49
  %53 = load i8*, i8** %17, align 8
  %54 = sext i32 %47 to i64
  %55 = getelementptr inbounds i8, i8* %53, i64 %54
  br label %59

56:                                               ; preds = %49, %46
  %57 = load i8*, i8** %18, align 8
  %58 = getelementptr inbounds i8, i8* %57, i64 8
  store i8* %58, i8** %18, align 8
  br label %59

59:                                               ; preds = %56, %52
  %60 = phi i8* [ %55, %52 ], [ %57, %56 ]
  %61 = bitcast i8* %60 to i32*
  %62 = load i32, i32* %61, align 8
  %63 = trunc i32 %62 to i8
  %64 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %63, i8* %65, align 8, !tbaa !23
  br label %201

66:                                               ; preds = %22
  %67 = load i32, i32* %16, align 8
  %68 = icmp sgt i32 %67, -1
  br i1 %68, label %76, label %69

69:                                               ; preds = %66
  %70 = add nsw i32 %67, 8
  store i32 %70, i32* %16, align 8
  %71 = icmp ult i32 %67, -7
  br i1 %71, label %72, label %76

72:                                               ; preds = %69
  %73 = load i8*, i8** %17, align 8
  %74 = sext i32 %67 to i64
  %75 = getelementptr inbounds i8, i8* %73, i64 %74
  br label %79

76:                                               ; preds = %69, %66
  %77 = load i8*, i8** %18, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %18, align 8
  br label %79

79:                                               ; preds = %76, %72
  %80 = phi i8* [ %75, %72 ], [ %77, %76 ]
  %81 = bitcast i8* %80 to i32*
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i16
  %84 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %85 = bitcast %union.jvalue* %84 to i16*
  store i16 %83, i16* %85, align 8, !tbaa !23
  br label %201

86:                                               ; preds = %22
  %87 = load i32, i32* %16, align 8
  %88 = icmp sgt i32 %87, -1
  br i1 %88, label %96, label %89

89:                                               ; preds = %86
  %90 = add nsw i32 %87, 8
  store i32 %90, i32* %16, align 8
  %91 = icmp ult i32 %87, -7
  br i1 %91, label %92, label %96

92:                                               ; preds = %89
  %93 = load i8*, i8** %17, align 8
  %94 = sext i32 %87 to i64
  %95 = getelementptr inbounds i8, i8* %93, i64 %94
  br label %99

96:                                               ; preds = %89, %86
  %97 = load i8*, i8** %18, align 8
  %98 = getelementptr inbounds i8, i8* %97, i64 8
  store i8* %98, i8** %18, align 8
  br label %99

99:                                               ; preds = %96, %92
  %100 = phi i8* [ %95, %92 ], [ %97, %96 ]
  %101 = bitcast i8* %100 to i32*
  %102 = load i32, i32* %101, align 8
  %103 = and i32 %102, 65535
  %104 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %103, i32* %105, align 8, !tbaa !23
  br label %201

106:                                              ; preds = %22
  %107 = load i32, i32* %16, align 8
  %108 = icmp sgt i32 %107, -1
  br i1 %108, label %116, label %109

109:                                              ; preds = %106
  %110 = add nsw i32 %107, 8
  store i32 %110, i32* %16, align 8
  %111 = icmp ult i32 %107, -7
  br i1 %111, label %112, label %116

112:                                              ; preds = %109
  %113 = load i8*, i8** %17, align 8
  %114 = sext i32 %107 to i64
  %115 = getelementptr inbounds i8, i8* %113, i64 %114
  br label %119

116:                                              ; preds = %109, %106
  %117 = load i8*, i8** %18, align 8
  %118 = getelementptr inbounds i8, i8* %117, i64 8
  store i8* %118, i8** %18, align 8
  br label %119

119:                                              ; preds = %116, %112
  %120 = phi i8* [ %115, %112 ], [ %117, %116 ]
  %121 = bitcast i8* %120 to i32*
  %122 = load i32, i32* %121, align 8
  %123 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %124 = bitcast %union.jvalue* %123 to i32*
  store i32 %122, i32* %124, align 8, !tbaa !23
  br label %201

125:                                              ; preds = %22
  %126 = load i32, i32* %16, align 8
  %127 = icmp sgt i32 %126, -1
  br i1 %127, label %135, label %128

128:                                              ; preds = %125
  %129 = add nsw i32 %126, 8
  store i32 %129, i32* %16, align 8
  %130 = icmp ult i32 %126, -7
  br i1 %130, label %131, label %135

131:                                              ; preds = %128
  %132 = load i8*, i8** %17, align 8
  %133 = sext i32 %126 to i64
  %134 = getelementptr inbounds i8, i8* %132, i64 %133
  br label %138

135:                                              ; preds = %128, %125
  %136 = load i8*, i8** %18, align 8
  %137 = getelementptr inbounds i8, i8* %136, i64 8
  store i8* %137, i8** %18, align 8
  br label %138

138:                                              ; preds = %135, %131
  %139 = phi i8* [ %134, %131 ], [ %136, %135 ]
  %140 = bitcast i8* %139 to i64*
  %141 = load i64, i64* %140, align 8
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23, i32 0
  store i64 %141, i64* %142, align 8, !tbaa !23
  br label %201

143:                                              ; preds = %22
  %144 = load i32, i32* %19, align 4
  %145 = icmp sgt i32 %144, -1
  br i1 %145, label %153, label %146

146:                                              ; preds = %143
  %147 = add nsw i32 %144, 16
  store i32 %147, i32* %19, align 4
  %148 = icmp ult i32 %144, -15
  br i1 %148, label %149, label %153

149:                                              ; preds = %146
  %150 = load i8*, i8** %20, align 8
  %151 = sext i32 %144 to i64
  %152 = getelementptr inbounds i8, i8* %150, i64 %151
  br label %156

153:                                              ; preds = %146, %143
  %154 = load i8*, i8** %18, align 8
  %155 = getelementptr inbounds i8, i8* %154, i64 8
  store i8* %155, i8** %18, align 8
  br label %156

156:                                              ; preds = %153, %149
  %157 = phi i8* [ %152, %149 ], [ %154, %153 ]
  %158 = bitcast i8* %157 to double*
  %159 = load double, double* %158, align 8
  %160 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %161 = bitcast %union.jvalue* %160 to double*
  store double %159, double* %161, align 8, !tbaa !23
  br label %201

162:                                              ; preds = %22
  %163 = load i32, i32* %19, align 4
  %164 = icmp sgt i32 %163, -1
  br i1 %164, label %172, label %165

165:                                              ; preds = %162
  %166 = add nsw i32 %163, 16
  store i32 %166, i32* %19, align 4
  %167 = icmp ult i32 %163, -15
  br i1 %167, label %168, label %172

168:                                              ; preds = %165
  %169 = load i8*, i8** %20, align 8
  %170 = sext i32 %163 to i64
  %171 = getelementptr inbounds i8, i8* %169, i64 %170
  br label %175

172:                                              ; preds = %165, %162
  %173 = load i8*, i8** %18, align 8
  %174 = getelementptr inbounds i8, i8* %173, i64 8
  store i8* %174, i8** %18, align 8
  br label %175

175:                                              ; preds = %172, %168
  %176 = phi i8* [ %171, %168 ], [ %173, %172 ]
  %177 = bitcast i8* %176 to double*
  %178 = load double, double* %177, align 8
  %179 = fptrunc double %178 to float
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %181 = bitcast %union.jvalue* %180 to float*
  store float %179, float* %181, align 8, !tbaa !23
  br label %201

182:                                              ; preds = %22
  %183 = load i32, i32* %16, align 8
  %184 = icmp sgt i32 %183, -1
  br i1 %184, label %192, label %185

185:                                              ; preds = %182
  %186 = add nsw i32 %183, 8
  store i32 %186, i32* %16, align 8
  %187 = icmp ult i32 %183, -7
  br i1 %187, label %188, label %192

188:                                              ; preds = %185
  %189 = load i8*, i8** %17, align 8
  %190 = sext i32 %183 to i64
  %191 = getelementptr inbounds i8, i8* %189, i64 %190
  br label %195

192:                                              ; preds = %185, %182
  %193 = load i8*, i8** %18, align 8
  %194 = getelementptr inbounds i8, i8* %193, i64 8
  store i8* %194, i8** %18, align 8
  br label %195

195:                                              ; preds = %192, %188
  %196 = phi i8* [ %191, %188 ], [ %193, %192 ]
  %197 = bitcast i8* %196 to %struct._jobject**
  %198 = load %struct._jobject*, %struct._jobject** %197, align 8
  %199 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %200 = bitcast %union.jvalue* %199 to %struct._jobject**
  store %struct._jobject* %198, %struct._jobject** %200, align 8, !tbaa !23
  br label %201

201:                                              ; preds = %39, %59, %79, %99, %119, %138, %156, %175, %195, %22
  %202 = add nuw nsw i64 %23, 1
  %203 = icmp eq i64 %202, %21
  br i1 %203, label %204, label %22, !llvm.loop !27

204:                                              ; preds = %201, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %205 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %206 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %205, i64 0, i32 66
  %207 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %206, align 8, !tbaa !28
  %208 = call %struct._jobject* %207(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret %struct._jobject* %208
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_CallStaticObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !29

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 116
  %219 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !30
  %220 = call %struct._jobject* %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret %struct._jobject* %220
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_CallStaticObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !29

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 116
  %206 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !30
  %207 = call %struct._jobject* %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret %struct._jobject* %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !31

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 39
  %219 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !32
  %220 = call i8 %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i8 %220
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !31

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 39
  %206 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !32
  %207 = call i8 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i8 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallNonvirtualBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca %"struct.std::__va_list", align 8
  %7 = bitcast %"struct.std::__va_list"* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 0
  %9 = load i8*, i8** %8, align 8, !tbaa.struct !10
  %10 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 1
  %11 = load i8*, i8** %10, align 8, !tbaa.struct !17
  %12 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 2
  %13 = load i8*, i8** %12, align 8, !tbaa.struct !18
  %14 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 3
  %15 = load i32, i32* %14, align 8, !tbaa.struct !19
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 4
  %17 = load i32, i32* %16, align 4, !tbaa.struct !20
  %18 = call i8* @llvm.stacksave()
  %19 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %19) #4
  %20 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %20, align 8, !tbaa !11
  %22 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %21, align 8, !tbaa !21
  %23 = call i32 %22(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %19) #4
  %24 = sext i32 %23 to i64
  %25 = alloca %union.jvalue, i64 %24, align 16
  %26 = icmp sgt i32 %23, 0
  br i1 %26, label %27, label %217

27:                                               ; preds = %4
  %28 = zext i32 %23 to i64
  br label %29

29:                                               ; preds = %211, %27
  %30 = phi i32 [ %15, %27 ], [ %212, %211 ]
  %31 = phi i32 [ %17, %27 ], [ %213, %211 ]
  %32 = phi i8* [ %9, %27 ], [ %214, %211 ]
  %33 = phi i64 [ 0, %27 ], [ %215, %211 ]
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1, !tbaa !23
  switch i8 %35, label %211 [
    i8 90, label %36
    i8 66, label %56
    i8 83, label %76
    i8 67, label %96
    i8 73, label %116
    i8 74, label %135
    i8 68, label %153
    i8 70, label %172
    i8 76, label %192
  ]

36:                                               ; preds = %29
  %37 = icmp sgt i32 %30, -1
  br i1 %37, label %44, label %38

38:                                               ; preds = %36
  %39 = add nsw i32 %30, 8
  %40 = icmp ult i32 %30, -7
  br i1 %40, label %41, label %44

41:                                               ; preds = %38
  %42 = sext i32 %30 to i64
  %43 = getelementptr inbounds i8, i8* %11, i64 %42
  br label %47

44:                                               ; preds = %38, %36
  %45 = phi i32 [ %30, %36 ], [ %39, %38 ]
  %46 = getelementptr inbounds i8, i8* %32, i64 8
  br label %47

47:                                               ; preds = %44, %41
  %48 = phi i32 [ %45, %44 ], [ %39, %41 ]
  %49 = phi i8* [ %46, %44 ], [ %32, %41 ]
  %50 = phi i8* [ %32, %44 ], [ %43, %41 ]
  %51 = bitcast i8* %50 to i32*
  %52 = load i32, i32* %51, align 8
  %53 = trunc i32 %52 to i8
  %54 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %55 = bitcast %union.jvalue* %54 to i8*
  store i8 %53, i8* %55, align 8, !tbaa !23
  br label %211

56:                                               ; preds = %29
  %57 = icmp sgt i32 %30, -1
  br i1 %57, label %64, label %58

58:                                               ; preds = %56
  %59 = add nsw i32 %30, 8
  %60 = icmp ult i32 %30, -7
  br i1 %60, label %61, label %64

61:                                               ; preds = %58
  %62 = sext i32 %30 to i64
  %63 = getelementptr inbounds i8, i8* %11, i64 %62
  br label %67

64:                                               ; preds = %58, %56
  %65 = phi i32 [ %30, %56 ], [ %59, %58 ]
  %66 = getelementptr inbounds i8, i8* %32, i64 8
  br label %67

67:                                               ; preds = %64, %61
  %68 = phi i32 [ %65, %64 ], [ %59, %61 ]
  %69 = phi i8* [ %66, %64 ], [ %32, %61 ]
  %70 = phi i8* [ %32, %64 ], [ %63, %61 ]
  %71 = bitcast i8* %70 to i32*
  %72 = load i32, i32* %71, align 8
  %73 = trunc i32 %72 to i8
  %74 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %75 = bitcast %union.jvalue* %74 to i8*
  store i8 %73, i8* %75, align 8, !tbaa !23
  br label %211

76:                                               ; preds = %29
  %77 = icmp sgt i32 %30, -1
  br i1 %77, label %84, label %78

78:                                               ; preds = %76
  %79 = add nsw i32 %30, 8
  %80 = icmp ult i32 %30, -7
  br i1 %80, label %81, label %84

81:                                               ; preds = %78
  %82 = sext i32 %30 to i64
  %83 = getelementptr inbounds i8, i8* %11, i64 %82
  br label %87

84:                                               ; preds = %78, %76
  %85 = phi i32 [ %30, %76 ], [ %79, %78 ]
  %86 = getelementptr inbounds i8, i8* %32, i64 8
  br label %87

87:                                               ; preds = %84, %81
  %88 = phi i32 [ %85, %84 ], [ %79, %81 ]
  %89 = phi i8* [ %86, %84 ], [ %32, %81 ]
  %90 = phi i8* [ %32, %84 ], [ %83, %81 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 8
  %93 = trunc i32 %92 to i16
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %93, i16* %95, align 8, !tbaa !23
  br label %211

96:                                               ; preds = %29
  %97 = icmp sgt i32 %30, -1
  br i1 %97, label %104, label %98

98:                                               ; preds = %96
  %99 = add nsw i32 %30, 8
  %100 = icmp ult i32 %30, -7
  br i1 %100, label %101, label %104

101:                                              ; preds = %98
  %102 = sext i32 %30 to i64
  %103 = getelementptr inbounds i8, i8* %11, i64 %102
  br label %107

104:                                              ; preds = %98, %96
  %105 = phi i32 [ %30, %96 ], [ %99, %98 ]
  %106 = getelementptr inbounds i8, i8* %32, i64 8
  br label %107

107:                                              ; preds = %104, %101
  %108 = phi i32 [ %105, %104 ], [ %99, %101 ]
  %109 = phi i8* [ %106, %104 ], [ %32, %101 ]
  %110 = phi i8* [ %32, %104 ], [ %103, %101 ]
  %111 = bitcast i8* %110 to i32*
  %112 = load i32, i32* %111, align 8
  %113 = and i32 %112, 65535
  %114 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %115 = bitcast %union.jvalue* %114 to i32*
  store i32 %113, i32* %115, align 8, !tbaa !23
  br label %211

116:                                              ; preds = %29
  %117 = icmp sgt i32 %30, -1
  br i1 %117, label %124, label %118

118:                                              ; preds = %116
  %119 = add nsw i32 %30, 8
  %120 = icmp ult i32 %30, -7
  br i1 %120, label %121, label %124

121:                                              ; preds = %118
  %122 = sext i32 %30 to i64
  %123 = getelementptr inbounds i8, i8* %11, i64 %122
  br label %127

124:                                              ; preds = %118, %116
  %125 = phi i32 [ %30, %116 ], [ %119, %118 ]
  %126 = getelementptr inbounds i8, i8* %32, i64 8
  br label %127

127:                                              ; preds = %124, %121
  %128 = phi i32 [ %125, %124 ], [ %119, %121 ]
  %129 = phi i8* [ %126, %124 ], [ %32, %121 ]
  %130 = phi i8* [ %32, %124 ], [ %123, %121 ]
  %131 = bitcast i8* %130 to i32*
  %132 = load i32, i32* %131, align 8
  %133 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %134 = bitcast %union.jvalue* %133 to i32*
  store i32 %132, i32* %134, align 8, !tbaa !23
  br label %211

135:                                              ; preds = %29
  %136 = icmp sgt i32 %30, -1
  br i1 %136, label %143, label %137

137:                                              ; preds = %135
  %138 = add nsw i32 %30, 8
  %139 = icmp ult i32 %30, -7
  br i1 %139, label %140, label %143

140:                                              ; preds = %137
  %141 = sext i32 %30 to i64
  %142 = getelementptr inbounds i8, i8* %11, i64 %141
  br label %146

143:                                              ; preds = %137, %135
  %144 = phi i32 [ %30, %135 ], [ %138, %137 ]
  %145 = getelementptr inbounds i8, i8* %32, i64 8
  br label %146

146:                                              ; preds = %143, %140
  %147 = phi i32 [ %144, %143 ], [ %138, %140 ]
  %148 = phi i8* [ %145, %143 ], [ %32, %140 ]
  %149 = phi i8* [ %32, %143 ], [ %142, %140 ]
  %150 = bitcast i8* %149 to i64*
  %151 = load i64, i64* %150, align 8
  %152 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33, i32 0
  store i64 %151, i64* %152, align 8, !tbaa !23
  br label %211

153:                                              ; preds = %29
  %154 = icmp sgt i32 %31, -1
  br i1 %154, label %161, label %155

155:                                              ; preds = %153
  %156 = add nsw i32 %31, 16
  %157 = icmp ult i32 %31, -15
  br i1 %157, label %158, label %161

158:                                              ; preds = %155
  %159 = sext i32 %31 to i64
  %160 = getelementptr inbounds i8, i8* %13, i64 %159
  br label %164

161:                                              ; preds = %155, %153
  %162 = phi i32 [ %31, %153 ], [ %156, %155 ]
  %163 = getelementptr inbounds i8, i8* %32, i64 8
  br label %164

164:                                              ; preds = %161, %158
  %165 = phi i32 [ %162, %161 ], [ %156, %158 ]
  %166 = phi i8* [ %163, %161 ], [ %32, %158 ]
  %167 = phi i8* [ %32, %161 ], [ %160, %158 ]
  %168 = bitcast i8* %167 to double*
  %169 = load double, double* %168, align 8
  %170 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %171 = bitcast %union.jvalue* %170 to double*
  store double %169, double* %171, align 8, !tbaa !23
  br label %211

172:                                              ; preds = %29
  %173 = icmp sgt i32 %31, -1
  br i1 %173, label %180, label %174

174:                                              ; preds = %172
  %175 = add nsw i32 %31, 16
  %176 = icmp ult i32 %31, -15
  br i1 %176, label %177, label %180

177:                                              ; preds = %174
  %178 = sext i32 %31 to i64
  %179 = getelementptr inbounds i8, i8* %13, i64 %178
  br label %183

180:                                              ; preds = %174, %172
  %181 = phi i32 [ %31, %172 ], [ %175, %174 ]
  %182 = getelementptr inbounds i8, i8* %32, i64 8
  br label %183

183:                                              ; preds = %180, %177
  %184 = phi i32 [ %181, %180 ], [ %175, %177 ]
  %185 = phi i8* [ %182, %180 ], [ %32, %177 ]
  %186 = phi i8* [ %32, %180 ], [ %179, %177 ]
  %187 = bitcast i8* %186 to double*
  %188 = load double, double* %187, align 8
  %189 = fptrunc double %188 to float
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %191 = bitcast %union.jvalue* %190 to float*
  store float %189, float* %191, align 8, !tbaa !23
  br label %211

192:                                              ; preds = %29
  %193 = icmp sgt i32 %30, -1
  br i1 %193, label %200, label %194

194:                                              ; preds = %192
  %195 = add nsw i32 %30, 8
  %196 = icmp ult i32 %30, -7
  br i1 %196, label %197, label %200

197:                                              ; preds = %194
  %198 = sext i32 %30 to i64
  %199 = getelementptr inbounds i8, i8* %11, i64 %198
  br label %203

200:                                              ; preds = %194, %192
  %201 = phi i32 [ %30, %192 ], [ %195, %194 ]
  %202 = getelementptr inbounds i8, i8* %32, i64 8
  br label %203

203:                                              ; preds = %200, %197
  %204 = phi i32 [ %201, %200 ], [ %195, %197 ]
  %205 = phi i8* [ %202, %200 ], [ %32, %197 ]
  %206 = phi i8* [ %32, %200 ], [ %199, %197 ]
  %207 = bitcast i8* %206 to %struct._jobject**
  %208 = load %struct._jobject*, %struct._jobject** %207, align 8
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %210 = bitcast %union.jvalue* %209 to %struct._jobject**
  store %struct._jobject* %208, %struct._jobject** %210, align 8, !tbaa !23
  br label %211

211:                                              ; preds = %203, %183, %164, %146, %127, %107, %87, %67, %47, %29
  %212 = phi i32 [ %30, %29 ], [ %204, %203 ], [ %30, %183 ], [ %30, %164 ], [ %147, %146 ], [ %128, %127 ], [ %108, %107 ], [ %88, %87 ], [ %68, %67 ], [ %48, %47 ]
  %213 = phi i32 [ %31, %29 ], [ %31, %203 ], [ %184, %183 ], [ %165, %164 ], [ %31, %146 ], [ %31, %127 ], [ %31, %107 ], [ %31, %87 ], [ %31, %67 ], [ %31, %47 ]
  %214 = phi i8* [ %32, %29 ], [ %205, %203 ], [ %185, %183 ], [ %166, %164 ], [ %148, %146 ], [ %129, %127 ], [ %109, %107 ], [ %89, %87 ], [ %69, %67 ], [ %49, %47 ]
  %215 = add nuw nsw i64 %33, 1
  %216 = icmp eq i64 %215, %28
  br i1 %216, label %217, label %29, !llvm.loop !33

217:                                              ; preds = %211, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %19) #4
  %218 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %219 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %218, i64 0, i32 69
  %220 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %219, align 8, !tbaa !34
  %221 = call i8 %220(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %25) #4
  call void @llvm.stackrestore(i8* %18)
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %7) #4
  ret i8 %221
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallNonvirtualBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !11
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !21
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #4
  %12 = sext i32 %11 to i64
  %13 = alloca %union.jvalue, i64 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %204

15:                                               ; preds = %5
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 3
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 1
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 0
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 4
  %20 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 2
  %21 = zext i32 %11 to i64
  br label %22

22:                                               ; preds = %15, %201
  %23 = phi i64 [ 0, %15 ], [ %202, %201 ]
  %24 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 %23
  %25 = load i8, i8* %24, align 1, !tbaa !23
  switch i8 %25, label %201 [
    i8 90, label %26
    i8 66, label %46
    i8 83, label %66
    i8 67, label %86
    i8 73, label %106
    i8 74, label %125
    i8 68, label %143
    i8 70, label %162
    i8 76, label %182
  ]

26:                                               ; preds = %22
  %27 = load i32, i32* %16, align 8
  %28 = icmp sgt i32 %27, -1
  br i1 %28, label %36, label %29

29:                                               ; preds = %26
  %30 = add nsw i32 %27, 8
  store i32 %30, i32* %16, align 8
  %31 = icmp ult i32 %27, -7
  br i1 %31, label %32, label %36

32:                                               ; preds = %29
  %33 = load i8*, i8** %17, align 8
  %34 = sext i32 %27 to i64
  %35 = getelementptr inbounds i8, i8* %33, i64 %34
  br label %39

36:                                               ; preds = %29, %26
  %37 = load i8*, i8** %18, align 8
  %38 = getelementptr inbounds i8, i8* %37, i64 8
  store i8* %38, i8** %18, align 8
  br label %39

39:                                               ; preds = %36, %32
  %40 = phi i8* [ %35, %32 ], [ %37, %36 ]
  %41 = bitcast i8* %40 to i32*
  %42 = load i32, i32* %41, align 8
  %43 = trunc i32 %42 to i8
  %44 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %43, i8* %45, align 8, !tbaa !23
  br label %201

46:                                               ; preds = %22
  %47 = load i32, i32* %16, align 8
  %48 = icmp sgt i32 %47, -1
  br i1 %48, label %56, label %49

49:                                               ; preds = %46
  %50 = add nsw i32 %47, 8
  store i32 %50, i32* %16, align 8
  %51 = icmp ult i32 %47, -7
  br i1 %51, label %52, label %56

52:                                               ; preds = %49
  %53 = load i8*, i8** %17, align 8
  %54 = sext i32 %47 to i64
  %55 = getelementptr inbounds i8, i8* %53, i64 %54
  br label %59

56:                                               ; preds = %49, %46
  %57 = load i8*, i8** %18, align 8
  %58 = getelementptr inbounds i8, i8* %57, i64 8
  store i8* %58, i8** %18, align 8
  br label %59

59:                                               ; preds = %56, %52
  %60 = phi i8* [ %55, %52 ], [ %57, %56 ]
  %61 = bitcast i8* %60 to i32*
  %62 = load i32, i32* %61, align 8
  %63 = trunc i32 %62 to i8
  %64 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %63, i8* %65, align 8, !tbaa !23
  br label %201

66:                                               ; preds = %22
  %67 = load i32, i32* %16, align 8
  %68 = icmp sgt i32 %67, -1
  br i1 %68, label %76, label %69

69:                                               ; preds = %66
  %70 = add nsw i32 %67, 8
  store i32 %70, i32* %16, align 8
  %71 = icmp ult i32 %67, -7
  br i1 %71, label %72, label %76

72:                                               ; preds = %69
  %73 = load i8*, i8** %17, align 8
  %74 = sext i32 %67 to i64
  %75 = getelementptr inbounds i8, i8* %73, i64 %74
  br label %79

76:                                               ; preds = %69, %66
  %77 = load i8*, i8** %18, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %18, align 8
  br label %79

79:                                               ; preds = %76, %72
  %80 = phi i8* [ %75, %72 ], [ %77, %76 ]
  %81 = bitcast i8* %80 to i32*
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i16
  %84 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %85 = bitcast %union.jvalue* %84 to i16*
  store i16 %83, i16* %85, align 8, !tbaa !23
  br label %201

86:                                               ; preds = %22
  %87 = load i32, i32* %16, align 8
  %88 = icmp sgt i32 %87, -1
  br i1 %88, label %96, label %89

89:                                               ; preds = %86
  %90 = add nsw i32 %87, 8
  store i32 %90, i32* %16, align 8
  %91 = icmp ult i32 %87, -7
  br i1 %91, label %92, label %96

92:                                               ; preds = %89
  %93 = load i8*, i8** %17, align 8
  %94 = sext i32 %87 to i64
  %95 = getelementptr inbounds i8, i8* %93, i64 %94
  br label %99

96:                                               ; preds = %89, %86
  %97 = load i8*, i8** %18, align 8
  %98 = getelementptr inbounds i8, i8* %97, i64 8
  store i8* %98, i8** %18, align 8
  br label %99

99:                                               ; preds = %96, %92
  %100 = phi i8* [ %95, %92 ], [ %97, %96 ]
  %101 = bitcast i8* %100 to i32*
  %102 = load i32, i32* %101, align 8
  %103 = and i32 %102, 65535
  %104 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %103, i32* %105, align 8, !tbaa !23
  br label %201

106:                                              ; preds = %22
  %107 = load i32, i32* %16, align 8
  %108 = icmp sgt i32 %107, -1
  br i1 %108, label %116, label %109

109:                                              ; preds = %106
  %110 = add nsw i32 %107, 8
  store i32 %110, i32* %16, align 8
  %111 = icmp ult i32 %107, -7
  br i1 %111, label %112, label %116

112:                                              ; preds = %109
  %113 = load i8*, i8** %17, align 8
  %114 = sext i32 %107 to i64
  %115 = getelementptr inbounds i8, i8* %113, i64 %114
  br label %119

116:                                              ; preds = %109, %106
  %117 = load i8*, i8** %18, align 8
  %118 = getelementptr inbounds i8, i8* %117, i64 8
  store i8* %118, i8** %18, align 8
  br label %119

119:                                              ; preds = %116, %112
  %120 = phi i8* [ %115, %112 ], [ %117, %116 ]
  %121 = bitcast i8* %120 to i32*
  %122 = load i32, i32* %121, align 8
  %123 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %124 = bitcast %union.jvalue* %123 to i32*
  store i32 %122, i32* %124, align 8, !tbaa !23
  br label %201

125:                                              ; preds = %22
  %126 = load i32, i32* %16, align 8
  %127 = icmp sgt i32 %126, -1
  br i1 %127, label %135, label %128

128:                                              ; preds = %125
  %129 = add nsw i32 %126, 8
  store i32 %129, i32* %16, align 8
  %130 = icmp ult i32 %126, -7
  br i1 %130, label %131, label %135

131:                                              ; preds = %128
  %132 = load i8*, i8** %17, align 8
  %133 = sext i32 %126 to i64
  %134 = getelementptr inbounds i8, i8* %132, i64 %133
  br label %138

135:                                              ; preds = %128, %125
  %136 = load i8*, i8** %18, align 8
  %137 = getelementptr inbounds i8, i8* %136, i64 8
  store i8* %137, i8** %18, align 8
  br label %138

138:                                              ; preds = %135, %131
  %139 = phi i8* [ %134, %131 ], [ %136, %135 ]
  %140 = bitcast i8* %139 to i64*
  %141 = load i64, i64* %140, align 8
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23, i32 0
  store i64 %141, i64* %142, align 8, !tbaa !23
  br label %201

143:                                              ; preds = %22
  %144 = load i32, i32* %19, align 4
  %145 = icmp sgt i32 %144, -1
  br i1 %145, label %153, label %146

146:                                              ; preds = %143
  %147 = add nsw i32 %144, 16
  store i32 %147, i32* %19, align 4
  %148 = icmp ult i32 %144, -15
  br i1 %148, label %149, label %153

149:                                              ; preds = %146
  %150 = load i8*, i8** %20, align 8
  %151 = sext i32 %144 to i64
  %152 = getelementptr inbounds i8, i8* %150, i64 %151
  br label %156

153:                                              ; preds = %146, %143
  %154 = load i8*, i8** %18, align 8
  %155 = getelementptr inbounds i8, i8* %154, i64 8
  store i8* %155, i8** %18, align 8
  br label %156

156:                                              ; preds = %153, %149
  %157 = phi i8* [ %152, %149 ], [ %154, %153 ]
  %158 = bitcast i8* %157 to double*
  %159 = load double, double* %158, align 8
  %160 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %161 = bitcast %union.jvalue* %160 to double*
  store double %159, double* %161, align 8, !tbaa !23
  br label %201

162:                                              ; preds = %22
  %163 = load i32, i32* %19, align 4
  %164 = icmp sgt i32 %163, -1
  br i1 %164, label %172, label %165

165:                                              ; preds = %162
  %166 = add nsw i32 %163, 16
  store i32 %166, i32* %19, align 4
  %167 = icmp ult i32 %163, -15
  br i1 %167, label %168, label %172

168:                                              ; preds = %165
  %169 = load i8*, i8** %20, align 8
  %170 = sext i32 %163 to i64
  %171 = getelementptr inbounds i8, i8* %169, i64 %170
  br label %175

172:                                              ; preds = %165, %162
  %173 = load i8*, i8** %18, align 8
  %174 = getelementptr inbounds i8, i8* %173, i64 8
  store i8* %174, i8** %18, align 8
  br label %175

175:                                              ; preds = %172, %168
  %176 = phi i8* [ %171, %168 ], [ %173, %172 ]
  %177 = bitcast i8* %176 to double*
  %178 = load double, double* %177, align 8
  %179 = fptrunc double %178 to float
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %181 = bitcast %union.jvalue* %180 to float*
  store float %179, float* %181, align 8, !tbaa !23
  br label %201

182:                                              ; preds = %22
  %183 = load i32, i32* %16, align 8
  %184 = icmp sgt i32 %183, -1
  br i1 %184, label %192, label %185

185:                                              ; preds = %182
  %186 = add nsw i32 %183, 8
  store i32 %186, i32* %16, align 8
  %187 = icmp ult i32 %183, -7
  br i1 %187, label %188, label %192

188:                                              ; preds = %185
  %189 = load i8*, i8** %17, align 8
  %190 = sext i32 %183 to i64
  %191 = getelementptr inbounds i8, i8* %189, i64 %190
  br label %195

192:                                              ; preds = %185, %182
  %193 = load i8*, i8** %18, align 8
  %194 = getelementptr inbounds i8, i8* %193, i64 8
  store i8* %194, i8** %18, align 8
  br label %195

195:                                              ; preds = %192, %188
  %196 = phi i8* [ %191, %188 ], [ %193, %192 ]
  %197 = bitcast i8* %196 to %struct._jobject**
  %198 = load %struct._jobject*, %struct._jobject** %197, align 8
  %199 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %200 = bitcast %union.jvalue* %199 to %struct._jobject**
  store %struct._jobject* %198, %struct._jobject** %200, align 8, !tbaa !23
  br label %201

201:                                              ; preds = %39, %59, %79, %99, %119, %138, %156, %175, %195, %22
  %202 = add nuw nsw i64 %23, 1
  %203 = icmp eq i64 %202, %21
  br i1 %203, label %204, label %22, !llvm.loop !33

204:                                              ; preds = %201, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %205 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %206 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %205, i64 0, i32 69
  %207 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %206, align 8, !tbaa !34
  %208 = call i8 %207(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret i8 %208
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallStaticBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !35

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 119
  %219 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !36
  %220 = call i8 %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i8 %220
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallStaticBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !35

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 119
  %206 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !36
  %207 = call i8 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i8 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !37

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 42
  %219 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !38
  %220 = call i8 %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i8 %220
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !37

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 42
  %206 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !38
  %207 = call i8 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i8 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallNonvirtualByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca %"struct.std::__va_list", align 8
  %7 = bitcast %"struct.std::__va_list"* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 0
  %9 = load i8*, i8** %8, align 8, !tbaa.struct !10
  %10 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 1
  %11 = load i8*, i8** %10, align 8, !tbaa.struct !17
  %12 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 2
  %13 = load i8*, i8** %12, align 8, !tbaa.struct !18
  %14 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 3
  %15 = load i32, i32* %14, align 8, !tbaa.struct !19
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 4
  %17 = load i32, i32* %16, align 4, !tbaa.struct !20
  %18 = call i8* @llvm.stacksave()
  %19 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %19) #4
  %20 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %20, align 8, !tbaa !11
  %22 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %21, align 8, !tbaa !21
  %23 = call i32 %22(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %19) #4
  %24 = sext i32 %23 to i64
  %25 = alloca %union.jvalue, i64 %24, align 16
  %26 = icmp sgt i32 %23, 0
  br i1 %26, label %27, label %217

27:                                               ; preds = %4
  %28 = zext i32 %23 to i64
  br label %29

29:                                               ; preds = %211, %27
  %30 = phi i32 [ %15, %27 ], [ %212, %211 ]
  %31 = phi i32 [ %17, %27 ], [ %213, %211 ]
  %32 = phi i8* [ %9, %27 ], [ %214, %211 ]
  %33 = phi i64 [ 0, %27 ], [ %215, %211 ]
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1, !tbaa !23
  switch i8 %35, label %211 [
    i8 90, label %36
    i8 66, label %56
    i8 83, label %76
    i8 67, label %96
    i8 73, label %116
    i8 74, label %135
    i8 68, label %153
    i8 70, label %172
    i8 76, label %192
  ]

36:                                               ; preds = %29
  %37 = icmp sgt i32 %30, -1
  br i1 %37, label %44, label %38

38:                                               ; preds = %36
  %39 = add nsw i32 %30, 8
  %40 = icmp ult i32 %30, -7
  br i1 %40, label %41, label %44

41:                                               ; preds = %38
  %42 = sext i32 %30 to i64
  %43 = getelementptr inbounds i8, i8* %11, i64 %42
  br label %47

44:                                               ; preds = %38, %36
  %45 = phi i32 [ %30, %36 ], [ %39, %38 ]
  %46 = getelementptr inbounds i8, i8* %32, i64 8
  br label %47

47:                                               ; preds = %44, %41
  %48 = phi i32 [ %45, %44 ], [ %39, %41 ]
  %49 = phi i8* [ %46, %44 ], [ %32, %41 ]
  %50 = phi i8* [ %32, %44 ], [ %43, %41 ]
  %51 = bitcast i8* %50 to i32*
  %52 = load i32, i32* %51, align 8
  %53 = trunc i32 %52 to i8
  %54 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %55 = bitcast %union.jvalue* %54 to i8*
  store i8 %53, i8* %55, align 8, !tbaa !23
  br label %211

56:                                               ; preds = %29
  %57 = icmp sgt i32 %30, -1
  br i1 %57, label %64, label %58

58:                                               ; preds = %56
  %59 = add nsw i32 %30, 8
  %60 = icmp ult i32 %30, -7
  br i1 %60, label %61, label %64

61:                                               ; preds = %58
  %62 = sext i32 %30 to i64
  %63 = getelementptr inbounds i8, i8* %11, i64 %62
  br label %67

64:                                               ; preds = %58, %56
  %65 = phi i32 [ %30, %56 ], [ %59, %58 ]
  %66 = getelementptr inbounds i8, i8* %32, i64 8
  br label %67

67:                                               ; preds = %64, %61
  %68 = phi i32 [ %65, %64 ], [ %59, %61 ]
  %69 = phi i8* [ %66, %64 ], [ %32, %61 ]
  %70 = phi i8* [ %32, %64 ], [ %63, %61 ]
  %71 = bitcast i8* %70 to i32*
  %72 = load i32, i32* %71, align 8
  %73 = trunc i32 %72 to i8
  %74 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %75 = bitcast %union.jvalue* %74 to i8*
  store i8 %73, i8* %75, align 8, !tbaa !23
  br label %211

76:                                               ; preds = %29
  %77 = icmp sgt i32 %30, -1
  br i1 %77, label %84, label %78

78:                                               ; preds = %76
  %79 = add nsw i32 %30, 8
  %80 = icmp ult i32 %30, -7
  br i1 %80, label %81, label %84

81:                                               ; preds = %78
  %82 = sext i32 %30 to i64
  %83 = getelementptr inbounds i8, i8* %11, i64 %82
  br label %87

84:                                               ; preds = %78, %76
  %85 = phi i32 [ %30, %76 ], [ %79, %78 ]
  %86 = getelementptr inbounds i8, i8* %32, i64 8
  br label %87

87:                                               ; preds = %84, %81
  %88 = phi i32 [ %85, %84 ], [ %79, %81 ]
  %89 = phi i8* [ %86, %84 ], [ %32, %81 ]
  %90 = phi i8* [ %32, %84 ], [ %83, %81 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 8
  %93 = trunc i32 %92 to i16
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %93, i16* %95, align 8, !tbaa !23
  br label %211

96:                                               ; preds = %29
  %97 = icmp sgt i32 %30, -1
  br i1 %97, label %104, label %98

98:                                               ; preds = %96
  %99 = add nsw i32 %30, 8
  %100 = icmp ult i32 %30, -7
  br i1 %100, label %101, label %104

101:                                              ; preds = %98
  %102 = sext i32 %30 to i64
  %103 = getelementptr inbounds i8, i8* %11, i64 %102
  br label %107

104:                                              ; preds = %98, %96
  %105 = phi i32 [ %30, %96 ], [ %99, %98 ]
  %106 = getelementptr inbounds i8, i8* %32, i64 8
  br label %107

107:                                              ; preds = %104, %101
  %108 = phi i32 [ %105, %104 ], [ %99, %101 ]
  %109 = phi i8* [ %106, %104 ], [ %32, %101 ]
  %110 = phi i8* [ %32, %104 ], [ %103, %101 ]
  %111 = bitcast i8* %110 to i32*
  %112 = load i32, i32* %111, align 8
  %113 = and i32 %112, 65535
  %114 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %115 = bitcast %union.jvalue* %114 to i32*
  store i32 %113, i32* %115, align 8, !tbaa !23
  br label %211

116:                                              ; preds = %29
  %117 = icmp sgt i32 %30, -1
  br i1 %117, label %124, label %118

118:                                              ; preds = %116
  %119 = add nsw i32 %30, 8
  %120 = icmp ult i32 %30, -7
  br i1 %120, label %121, label %124

121:                                              ; preds = %118
  %122 = sext i32 %30 to i64
  %123 = getelementptr inbounds i8, i8* %11, i64 %122
  br label %127

124:                                              ; preds = %118, %116
  %125 = phi i32 [ %30, %116 ], [ %119, %118 ]
  %126 = getelementptr inbounds i8, i8* %32, i64 8
  br label %127

127:                                              ; preds = %124, %121
  %128 = phi i32 [ %125, %124 ], [ %119, %121 ]
  %129 = phi i8* [ %126, %124 ], [ %32, %121 ]
  %130 = phi i8* [ %32, %124 ], [ %123, %121 ]
  %131 = bitcast i8* %130 to i32*
  %132 = load i32, i32* %131, align 8
  %133 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %134 = bitcast %union.jvalue* %133 to i32*
  store i32 %132, i32* %134, align 8, !tbaa !23
  br label %211

135:                                              ; preds = %29
  %136 = icmp sgt i32 %30, -1
  br i1 %136, label %143, label %137

137:                                              ; preds = %135
  %138 = add nsw i32 %30, 8
  %139 = icmp ult i32 %30, -7
  br i1 %139, label %140, label %143

140:                                              ; preds = %137
  %141 = sext i32 %30 to i64
  %142 = getelementptr inbounds i8, i8* %11, i64 %141
  br label %146

143:                                              ; preds = %137, %135
  %144 = phi i32 [ %30, %135 ], [ %138, %137 ]
  %145 = getelementptr inbounds i8, i8* %32, i64 8
  br label %146

146:                                              ; preds = %143, %140
  %147 = phi i32 [ %144, %143 ], [ %138, %140 ]
  %148 = phi i8* [ %145, %143 ], [ %32, %140 ]
  %149 = phi i8* [ %32, %143 ], [ %142, %140 ]
  %150 = bitcast i8* %149 to i64*
  %151 = load i64, i64* %150, align 8
  %152 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33, i32 0
  store i64 %151, i64* %152, align 8, !tbaa !23
  br label %211

153:                                              ; preds = %29
  %154 = icmp sgt i32 %31, -1
  br i1 %154, label %161, label %155

155:                                              ; preds = %153
  %156 = add nsw i32 %31, 16
  %157 = icmp ult i32 %31, -15
  br i1 %157, label %158, label %161

158:                                              ; preds = %155
  %159 = sext i32 %31 to i64
  %160 = getelementptr inbounds i8, i8* %13, i64 %159
  br label %164

161:                                              ; preds = %155, %153
  %162 = phi i32 [ %31, %153 ], [ %156, %155 ]
  %163 = getelementptr inbounds i8, i8* %32, i64 8
  br label %164

164:                                              ; preds = %161, %158
  %165 = phi i32 [ %162, %161 ], [ %156, %158 ]
  %166 = phi i8* [ %163, %161 ], [ %32, %158 ]
  %167 = phi i8* [ %32, %161 ], [ %160, %158 ]
  %168 = bitcast i8* %167 to double*
  %169 = load double, double* %168, align 8
  %170 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %171 = bitcast %union.jvalue* %170 to double*
  store double %169, double* %171, align 8, !tbaa !23
  br label %211

172:                                              ; preds = %29
  %173 = icmp sgt i32 %31, -1
  br i1 %173, label %180, label %174

174:                                              ; preds = %172
  %175 = add nsw i32 %31, 16
  %176 = icmp ult i32 %31, -15
  br i1 %176, label %177, label %180

177:                                              ; preds = %174
  %178 = sext i32 %31 to i64
  %179 = getelementptr inbounds i8, i8* %13, i64 %178
  br label %183

180:                                              ; preds = %174, %172
  %181 = phi i32 [ %31, %172 ], [ %175, %174 ]
  %182 = getelementptr inbounds i8, i8* %32, i64 8
  br label %183

183:                                              ; preds = %180, %177
  %184 = phi i32 [ %181, %180 ], [ %175, %177 ]
  %185 = phi i8* [ %182, %180 ], [ %32, %177 ]
  %186 = phi i8* [ %32, %180 ], [ %179, %177 ]
  %187 = bitcast i8* %186 to double*
  %188 = load double, double* %187, align 8
  %189 = fptrunc double %188 to float
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %191 = bitcast %union.jvalue* %190 to float*
  store float %189, float* %191, align 8, !tbaa !23
  br label %211

192:                                              ; preds = %29
  %193 = icmp sgt i32 %30, -1
  br i1 %193, label %200, label %194

194:                                              ; preds = %192
  %195 = add nsw i32 %30, 8
  %196 = icmp ult i32 %30, -7
  br i1 %196, label %197, label %200

197:                                              ; preds = %194
  %198 = sext i32 %30 to i64
  %199 = getelementptr inbounds i8, i8* %11, i64 %198
  br label %203

200:                                              ; preds = %194, %192
  %201 = phi i32 [ %30, %192 ], [ %195, %194 ]
  %202 = getelementptr inbounds i8, i8* %32, i64 8
  br label %203

203:                                              ; preds = %200, %197
  %204 = phi i32 [ %201, %200 ], [ %195, %197 ]
  %205 = phi i8* [ %202, %200 ], [ %32, %197 ]
  %206 = phi i8* [ %32, %200 ], [ %199, %197 ]
  %207 = bitcast i8* %206 to %struct._jobject**
  %208 = load %struct._jobject*, %struct._jobject** %207, align 8
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %210 = bitcast %union.jvalue* %209 to %struct._jobject**
  store %struct._jobject* %208, %struct._jobject** %210, align 8, !tbaa !23
  br label %211

211:                                              ; preds = %203, %183, %164, %146, %127, %107, %87, %67, %47, %29
  %212 = phi i32 [ %30, %29 ], [ %204, %203 ], [ %30, %183 ], [ %30, %164 ], [ %147, %146 ], [ %128, %127 ], [ %108, %107 ], [ %88, %87 ], [ %68, %67 ], [ %48, %47 ]
  %213 = phi i32 [ %31, %29 ], [ %31, %203 ], [ %184, %183 ], [ %165, %164 ], [ %31, %146 ], [ %31, %127 ], [ %31, %107 ], [ %31, %87 ], [ %31, %67 ], [ %31, %47 ]
  %214 = phi i8* [ %32, %29 ], [ %205, %203 ], [ %185, %183 ], [ %166, %164 ], [ %148, %146 ], [ %129, %127 ], [ %109, %107 ], [ %89, %87 ], [ %69, %67 ], [ %49, %47 ]
  %215 = add nuw nsw i64 %33, 1
  %216 = icmp eq i64 %215, %28
  br i1 %216, label %217, label %29, !llvm.loop !39

217:                                              ; preds = %211, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %19) #4
  %218 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %219 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %218, i64 0, i32 72
  %220 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %219, align 8, !tbaa !40
  %221 = call i8 %220(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %25) #4
  call void @llvm.stackrestore(i8* %18)
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %7) #4
  ret i8 %221
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallNonvirtualByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !11
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !21
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #4
  %12 = sext i32 %11 to i64
  %13 = alloca %union.jvalue, i64 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %204

15:                                               ; preds = %5
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 3
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 1
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 0
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 4
  %20 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 2
  %21 = zext i32 %11 to i64
  br label %22

22:                                               ; preds = %15, %201
  %23 = phi i64 [ 0, %15 ], [ %202, %201 ]
  %24 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 %23
  %25 = load i8, i8* %24, align 1, !tbaa !23
  switch i8 %25, label %201 [
    i8 90, label %26
    i8 66, label %46
    i8 83, label %66
    i8 67, label %86
    i8 73, label %106
    i8 74, label %125
    i8 68, label %143
    i8 70, label %162
    i8 76, label %182
  ]

26:                                               ; preds = %22
  %27 = load i32, i32* %16, align 8
  %28 = icmp sgt i32 %27, -1
  br i1 %28, label %36, label %29

29:                                               ; preds = %26
  %30 = add nsw i32 %27, 8
  store i32 %30, i32* %16, align 8
  %31 = icmp ult i32 %27, -7
  br i1 %31, label %32, label %36

32:                                               ; preds = %29
  %33 = load i8*, i8** %17, align 8
  %34 = sext i32 %27 to i64
  %35 = getelementptr inbounds i8, i8* %33, i64 %34
  br label %39

36:                                               ; preds = %29, %26
  %37 = load i8*, i8** %18, align 8
  %38 = getelementptr inbounds i8, i8* %37, i64 8
  store i8* %38, i8** %18, align 8
  br label %39

39:                                               ; preds = %36, %32
  %40 = phi i8* [ %35, %32 ], [ %37, %36 ]
  %41 = bitcast i8* %40 to i32*
  %42 = load i32, i32* %41, align 8
  %43 = trunc i32 %42 to i8
  %44 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %43, i8* %45, align 8, !tbaa !23
  br label %201

46:                                               ; preds = %22
  %47 = load i32, i32* %16, align 8
  %48 = icmp sgt i32 %47, -1
  br i1 %48, label %56, label %49

49:                                               ; preds = %46
  %50 = add nsw i32 %47, 8
  store i32 %50, i32* %16, align 8
  %51 = icmp ult i32 %47, -7
  br i1 %51, label %52, label %56

52:                                               ; preds = %49
  %53 = load i8*, i8** %17, align 8
  %54 = sext i32 %47 to i64
  %55 = getelementptr inbounds i8, i8* %53, i64 %54
  br label %59

56:                                               ; preds = %49, %46
  %57 = load i8*, i8** %18, align 8
  %58 = getelementptr inbounds i8, i8* %57, i64 8
  store i8* %58, i8** %18, align 8
  br label %59

59:                                               ; preds = %56, %52
  %60 = phi i8* [ %55, %52 ], [ %57, %56 ]
  %61 = bitcast i8* %60 to i32*
  %62 = load i32, i32* %61, align 8
  %63 = trunc i32 %62 to i8
  %64 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %63, i8* %65, align 8, !tbaa !23
  br label %201

66:                                               ; preds = %22
  %67 = load i32, i32* %16, align 8
  %68 = icmp sgt i32 %67, -1
  br i1 %68, label %76, label %69

69:                                               ; preds = %66
  %70 = add nsw i32 %67, 8
  store i32 %70, i32* %16, align 8
  %71 = icmp ult i32 %67, -7
  br i1 %71, label %72, label %76

72:                                               ; preds = %69
  %73 = load i8*, i8** %17, align 8
  %74 = sext i32 %67 to i64
  %75 = getelementptr inbounds i8, i8* %73, i64 %74
  br label %79

76:                                               ; preds = %69, %66
  %77 = load i8*, i8** %18, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %18, align 8
  br label %79

79:                                               ; preds = %76, %72
  %80 = phi i8* [ %75, %72 ], [ %77, %76 ]
  %81 = bitcast i8* %80 to i32*
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i16
  %84 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %85 = bitcast %union.jvalue* %84 to i16*
  store i16 %83, i16* %85, align 8, !tbaa !23
  br label %201

86:                                               ; preds = %22
  %87 = load i32, i32* %16, align 8
  %88 = icmp sgt i32 %87, -1
  br i1 %88, label %96, label %89

89:                                               ; preds = %86
  %90 = add nsw i32 %87, 8
  store i32 %90, i32* %16, align 8
  %91 = icmp ult i32 %87, -7
  br i1 %91, label %92, label %96

92:                                               ; preds = %89
  %93 = load i8*, i8** %17, align 8
  %94 = sext i32 %87 to i64
  %95 = getelementptr inbounds i8, i8* %93, i64 %94
  br label %99

96:                                               ; preds = %89, %86
  %97 = load i8*, i8** %18, align 8
  %98 = getelementptr inbounds i8, i8* %97, i64 8
  store i8* %98, i8** %18, align 8
  br label %99

99:                                               ; preds = %96, %92
  %100 = phi i8* [ %95, %92 ], [ %97, %96 ]
  %101 = bitcast i8* %100 to i32*
  %102 = load i32, i32* %101, align 8
  %103 = and i32 %102, 65535
  %104 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %103, i32* %105, align 8, !tbaa !23
  br label %201

106:                                              ; preds = %22
  %107 = load i32, i32* %16, align 8
  %108 = icmp sgt i32 %107, -1
  br i1 %108, label %116, label %109

109:                                              ; preds = %106
  %110 = add nsw i32 %107, 8
  store i32 %110, i32* %16, align 8
  %111 = icmp ult i32 %107, -7
  br i1 %111, label %112, label %116

112:                                              ; preds = %109
  %113 = load i8*, i8** %17, align 8
  %114 = sext i32 %107 to i64
  %115 = getelementptr inbounds i8, i8* %113, i64 %114
  br label %119

116:                                              ; preds = %109, %106
  %117 = load i8*, i8** %18, align 8
  %118 = getelementptr inbounds i8, i8* %117, i64 8
  store i8* %118, i8** %18, align 8
  br label %119

119:                                              ; preds = %116, %112
  %120 = phi i8* [ %115, %112 ], [ %117, %116 ]
  %121 = bitcast i8* %120 to i32*
  %122 = load i32, i32* %121, align 8
  %123 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %124 = bitcast %union.jvalue* %123 to i32*
  store i32 %122, i32* %124, align 8, !tbaa !23
  br label %201

125:                                              ; preds = %22
  %126 = load i32, i32* %16, align 8
  %127 = icmp sgt i32 %126, -1
  br i1 %127, label %135, label %128

128:                                              ; preds = %125
  %129 = add nsw i32 %126, 8
  store i32 %129, i32* %16, align 8
  %130 = icmp ult i32 %126, -7
  br i1 %130, label %131, label %135

131:                                              ; preds = %128
  %132 = load i8*, i8** %17, align 8
  %133 = sext i32 %126 to i64
  %134 = getelementptr inbounds i8, i8* %132, i64 %133
  br label %138

135:                                              ; preds = %128, %125
  %136 = load i8*, i8** %18, align 8
  %137 = getelementptr inbounds i8, i8* %136, i64 8
  store i8* %137, i8** %18, align 8
  br label %138

138:                                              ; preds = %135, %131
  %139 = phi i8* [ %134, %131 ], [ %136, %135 ]
  %140 = bitcast i8* %139 to i64*
  %141 = load i64, i64* %140, align 8
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23, i32 0
  store i64 %141, i64* %142, align 8, !tbaa !23
  br label %201

143:                                              ; preds = %22
  %144 = load i32, i32* %19, align 4
  %145 = icmp sgt i32 %144, -1
  br i1 %145, label %153, label %146

146:                                              ; preds = %143
  %147 = add nsw i32 %144, 16
  store i32 %147, i32* %19, align 4
  %148 = icmp ult i32 %144, -15
  br i1 %148, label %149, label %153

149:                                              ; preds = %146
  %150 = load i8*, i8** %20, align 8
  %151 = sext i32 %144 to i64
  %152 = getelementptr inbounds i8, i8* %150, i64 %151
  br label %156

153:                                              ; preds = %146, %143
  %154 = load i8*, i8** %18, align 8
  %155 = getelementptr inbounds i8, i8* %154, i64 8
  store i8* %155, i8** %18, align 8
  br label %156

156:                                              ; preds = %153, %149
  %157 = phi i8* [ %152, %149 ], [ %154, %153 ]
  %158 = bitcast i8* %157 to double*
  %159 = load double, double* %158, align 8
  %160 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %161 = bitcast %union.jvalue* %160 to double*
  store double %159, double* %161, align 8, !tbaa !23
  br label %201

162:                                              ; preds = %22
  %163 = load i32, i32* %19, align 4
  %164 = icmp sgt i32 %163, -1
  br i1 %164, label %172, label %165

165:                                              ; preds = %162
  %166 = add nsw i32 %163, 16
  store i32 %166, i32* %19, align 4
  %167 = icmp ult i32 %163, -15
  br i1 %167, label %168, label %172

168:                                              ; preds = %165
  %169 = load i8*, i8** %20, align 8
  %170 = sext i32 %163 to i64
  %171 = getelementptr inbounds i8, i8* %169, i64 %170
  br label %175

172:                                              ; preds = %165, %162
  %173 = load i8*, i8** %18, align 8
  %174 = getelementptr inbounds i8, i8* %173, i64 8
  store i8* %174, i8** %18, align 8
  br label %175

175:                                              ; preds = %172, %168
  %176 = phi i8* [ %171, %168 ], [ %173, %172 ]
  %177 = bitcast i8* %176 to double*
  %178 = load double, double* %177, align 8
  %179 = fptrunc double %178 to float
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %181 = bitcast %union.jvalue* %180 to float*
  store float %179, float* %181, align 8, !tbaa !23
  br label %201

182:                                              ; preds = %22
  %183 = load i32, i32* %16, align 8
  %184 = icmp sgt i32 %183, -1
  br i1 %184, label %192, label %185

185:                                              ; preds = %182
  %186 = add nsw i32 %183, 8
  store i32 %186, i32* %16, align 8
  %187 = icmp ult i32 %183, -7
  br i1 %187, label %188, label %192

188:                                              ; preds = %185
  %189 = load i8*, i8** %17, align 8
  %190 = sext i32 %183 to i64
  %191 = getelementptr inbounds i8, i8* %189, i64 %190
  br label %195

192:                                              ; preds = %185, %182
  %193 = load i8*, i8** %18, align 8
  %194 = getelementptr inbounds i8, i8* %193, i64 8
  store i8* %194, i8** %18, align 8
  br label %195

195:                                              ; preds = %192, %188
  %196 = phi i8* [ %191, %188 ], [ %193, %192 ]
  %197 = bitcast i8* %196 to %struct._jobject**
  %198 = load %struct._jobject*, %struct._jobject** %197, align 8
  %199 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %200 = bitcast %union.jvalue* %199 to %struct._jobject**
  store %struct._jobject* %198, %struct._jobject** %200, align 8, !tbaa !23
  br label %201

201:                                              ; preds = %39, %59, %79, %99, %119, %138, %156, %175, %195, %22
  %202 = add nuw nsw i64 %23, 1
  %203 = icmp eq i64 %202, %21
  br i1 %203, label %204, label %22, !llvm.loop !39

204:                                              ; preds = %201, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %205 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %206 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %205, i64 0, i32 72
  %207 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %206, align 8, !tbaa !40
  %208 = call i8 %207(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret i8 %208
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallStaticByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !41

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 122
  %219 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !42
  %220 = call i8 %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i8 %220
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallStaticByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !41

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 122
  %206 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !42
  %207 = call i8 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i8 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !43

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 45
  %219 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !44
  %220 = call i16 %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i16 %220
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !43

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 45
  %206 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !44
  %207 = call i16 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i16 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallNonvirtualCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca %"struct.std::__va_list", align 8
  %7 = bitcast %"struct.std::__va_list"* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 0
  %9 = load i8*, i8** %8, align 8, !tbaa.struct !10
  %10 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 1
  %11 = load i8*, i8** %10, align 8, !tbaa.struct !17
  %12 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 2
  %13 = load i8*, i8** %12, align 8, !tbaa.struct !18
  %14 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 3
  %15 = load i32, i32* %14, align 8, !tbaa.struct !19
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 4
  %17 = load i32, i32* %16, align 4, !tbaa.struct !20
  %18 = call i8* @llvm.stacksave()
  %19 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %19) #4
  %20 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %20, align 8, !tbaa !11
  %22 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %21, align 8, !tbaa !21
  %23 = call i32 %22(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %19) #4
  %24 = sext i32 %23 to i64
  %25 = alloca %union.jvalue, i64 %24, align 16
  %26 = icmp sgt i32 %23, 0
  br i1 %26, label %27, label %217

27:                                               ; preds = %4
  %28 = zext i32 %23 to i64
  br label %29

29:                                               ; preds = %211, %27
  %30 = phi i32 [ %15, %27 ], [ %212, %211 ]
  %31 = phi i32 [ %17, %27 ], [ %213, %211 ]
  %32 = phi i8* [ %9, %27 ], [ %214, %211 ]
  %33 = phi i64 [ 0, %27 ], [ %215, %211 ]
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1, !tbaa !23
  switch i8 %35, label %211 [
    i8 90, label %36
    i8 66, label %56
    i8 83, label %76
    i8 67, label %96
    i8 73, label %116
    i8 74, label %135
    i8 68, label %153
    i8 70, label %172
    i8 76, label %192
  ]

36:                                               ; preds = %29
  %37 = icmp sgt i32 %30, -1
  br i1 %37, label %44, label %38

38:                                               ; preds = %36
  %39 = add nsw i32 %30, 8
  %40 = icmp ult i32 %30, -7
  br i1 %40, label %41, label %44

41:                                               ; preds = %38
  %42 = sext i32 %30 to i64
  %43 = getelementptr inbounds i8, i8* %11, i64 %42
  br label %47

44:                                               ; preds = %38, %36
  %45 = phi i32 [ %30, %36 ], [ %39, %38 ]
  %46 = getelementptr inbounds i8, i8* %32, i64 8
  br label %47

47:                                               ; preds = %44, %41
  %48 = phi i32 [ %45, %44 ], [ %39, %41 ]
  %49 = phi i8* [ %46, %44 ], [ %32, %41 ]
  %50 = phi i8* [ %32, %44 ], [ %43, %41 ]
  %51 = bitcast i8* %50 to i32*
  %52 = load i32, i32* %51, align 8
  %53 = trunc i32 %52 to i8
  %54 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %55 = bitcast %union.jvalue* %54 to i8*
  store i8 %53, i8* %55, align 8, !tbaa !23
  br label %211

56:                                               ; preds = %29
  %57 = icmp sgt i32 %30, -1
  br i1 %57, label %64, label %58

58:                                               ; preds = %56
  %59 = add nsw i32 %30, 8
  %60 = icmp ult i32 %30, -7
  br i1 %60, label %61, label %64

61:                                               ; preds = %58
  %62 = sext i32 %30 to i64
  %63 = getelementptr inbounds i8, i8* %11, i64 %62
  br label %67

64:                                               ; preds = %58, %56
  %65 = phi i32 [ %30, %56 ], [ %59, %58 ]
  %66 = getelementptr inbounds i8, i8* %32, i64 8
  br label %67

67:                                               ; preds = %64, %61
  %68 = phi i32 [ %65, %64 ], [ %59, %61 ]
  %69 = phi i8* [ %66, %64 ], [ %32, %61 ]
  %70 = phi i8* [ %32, %64 ], [ %63, %61 ]
  %71 = bitcast i8* %70 to i32*
  %72 = load i32, i32* %71, align 8
  %73 = trunc i32 %72 to i8
  %74 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %75 = bitcast %union.jvalue* %74 to i8*
  store i8 %73, i8* %75, align 8, !tbaa !23
  br label %211

76:                                               ; preds = %29
  %77 = icmp sgt i32 %30, -1
  br i1 %77, label %84, label %78

78:                                               ; preds = %76
  %79 = add nsw i32 %30, 8
  %80 = icmp ult i32 %30, -7
  br i1 %80, label %81, label %84

81:                                               ; preds = %78
  %82 = sext i32 %30 to i64
  %83 = getelementptr inbounds i8, i8* %11, i64 %82
  br label %87

84:                                               ; preds = %78, %76
  %85 = phi i32 [ %30, %76 ], [ %79, %78 ]
  %86 = getelementptr inbounds i8, i8* %32, i64 8
  br label %87

87:                                               ; preds = %84, %81
  %88 = phi i32 [ %85, %84 ], [ %79, %81 ]
  %89 = phi i8* [ %86, %84 ], [ %32, %81 ]
  %90 = phi i8* [ %32, %84 ], [ %83, %81 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 8
  %93 = trunc i32 %92 to i16
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %93, i16* %95, align 8, !tbaa !23
  br label %211

96:                                               ; preds = %29
  %97 = icmp sgt i32 %30, -1
  br i1 %97, label %104, label %98

98:                                               ; preds = %96
  %99 = add nsw i32 %30, 8
  %100 = icmp ult i32 %30, -7
  br i1 %100, label %101, label %104

101:                                              ; preds = %98
  %102 = sext i32 %30 to i64
  %103 = getelementptr inbounds i8, i8* %11, i64 %102
  br label %107

104:                                              ; preds = %98, %96
  %105 = phi i32 [ %30, %96 ], [ %99, %98 ]
  %106 = getelementptr inbounds i8, i8* %32, i64 8
  br label %107

107:                                              ; preds = %104, %101
  %108 = phi i32 [ %105, %104 ], [ %99, %101 ]
  %109 = phi i8* [ %106, %104 ], [ %32, %101 ]
  %110 = phi i8* [ %32, %104 ], [ %103, %101 ]
  %111 = bitcast i8* %110 to i32*
  %112 = load i32, i32* %111, align 8
  %113 = and i32 %112, 65535
  %114 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %115 = bitcast %union.jvalue* %114 to i32*
  store i32 %113, i32* %115, align 8, !tbaa !23
  br label %211

116:                                              ; preds = %29
  %117 = icmp sgt i32 %30, -1
  br i1 %117, label %124, label %118

118:                                              ; preds = %116
  %119 = add nsw i32 %30, 8
  %120 = icmp ult i32 %30, -7
  br i1 %120, label %121, label %124

121:                                              ; preds = %118
  %122 = sext i32 %30 to i64
  %123 = getelementptr inbounds i8, i8* %11, i64 %122
  br label %127

124:                                              ; preds = %118, %116
  %125 = phi i32 [ %30, %116 ], [ %119, %118 ]
  %126 = getelementptr inbounds i8, i8* %32, i64 8
  br label %127

127:                                              ; preds = %124, %121
  %128 = phi i32 [ %125, %124 ], [ %119, %121 ]
  %129 = phi i8* [ %126, %124 ], [ %32, %121 ]
  %130 = phi i8* [ %32, %124 ], [ %123, %121 ]
  %131 = bitcast i8* %130 to i32*
  %132 = load i32, i32* %131, align 8
  %133 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %134 = bitcast %union.jvalue* %133 to i32*
  store i32 %132, i32* %134, align 8, !tbaa !23
  br label %211

135:                                              ; preds = %29
  %136 = icmp sgt i32 %30, -1
  br i1 %136, label %143, label %137

137:                                              ; preds = %135
  %138 = add nsw i32 %30, 8
  %139 = icmp ult i32 %30, -7
  br i1 %139, label %140, label %143

140:                                              ; preds = %137
  %141 = sext i32 %30 to i64
  %142 = getelementptr inbounds i8, i8* %11, i64 %141
  br label %146

143:                                              ; preds = %137, %135
  %144 = phi i32 [ %30, %135 ], [ %138, %137 ]
  %145 = getelementptr inbounds i8, i8* %32, i64 8
  br label %146

146:                                              ; preds = %143, %140
  %147 = phi i32 [ %144, %143 ], [ %138, %140 ]
  %148 = phi i8* [ %145, %143 ], [ %32, %140 ]
  %149 = phi i8* [ %32, %143 ], [ %142, %140 ]
  %150 = bitcast i8* %149 to i64*
  %151 = load i64, i64* %150, align 8
  %152 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33, i32 0
  store i64 %151, i64* %152, align 8, !tbaa !23
  br label %211

153:                                              ; preds = %29
  %154 = icmp sgt i32 %31, -1
  br i1 %154, label %161, label %155

155:                                              ; preds = %153
  %156 = add nsw i32 %31, 16
  %157 = icmp ult i32 %31, -15
  br i1 %157, label %158, label %161

158:                                              ; preds = %155
  %159 = sext i32 %31 to i64
  %160 = getelementptr inbounds i8, i8* %13, i64 %159
  br label %164

161:                                              ; preds = %155, %153
  %162 = phi i32 [ %31, %153 ], [ %156, %155 ]
  %163 = getelementptr inbounds i8, i8* %32, i64 8
  br label %164

164:                                              ; preds = %161, %158
  %165 = phi i32 [ %162, %161 ], [ %156, %158 ]
  %166 = phi i8* [ %163, %161 ], [ %32, %158 ]
  %167 = phi i8* [ %32, %161 ], [ %160, %158 ]
  %168 = bitcast i8* %167 to double*
  %169 = load double, double* %168, align 8
  %170 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %171 = bitcast %union.jvalue* %170 to double*
  store double %169, double* %171, align 8, !tbaa !23
  br label %211

172:                                              ; preds = %29
  %173 = icmp sgt i32 %31, -1
  br i1 %173, label %180, label %174

174:                                              ; preds = %172
  %175 = add nsw i32 %31, 16
  %176 = icmp ult i32 %31, -15
  br i1 %176, label %177, label %180

177:                                              ; preds = %174
  %178 = sext i32 %31 to i64
  %179 = getelementptr inbounds i8, i8* %13, i64 %178
  br label %183

180:                                              ; preds = %174, %172
  %181 = phi i32 [ %31, %172 ], [ %175, %174 ]
  %182 = getelementptr inbounds i8, i8* %32, i64 8
  br label %183

183:                                              ; preds = %180, %177
  %184 = phi i32 [ %181, %180 ], [ %175, %177 ]
  %185 = phi i8* [ %182, %180 ], [ %32, %177 ]
  %186 = phi i8* [ %32, %180 ], [ %179, %177 ]
  %187 = bitcast i8* %186 to double*
  %188 = load double, double* %187, align 8
  %189 = fptrunc double %188 to float
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %191 = bitcast %union.jvalue* %190 to float*
  store float %189, float* %191, align 8, !tbaa !23
  br label %211

192:                                              ; preds = %29
  %193 = icmp sgt i32 %30, -1
  br i1 %193, label %200, label %194

194:                                              ; preds = %192
  %195 = add nsw i32 %30, 8
  %196 = icmp ult i32 %30, -7
  br i1 %196, label %197, label %200

197:                                              ; preds = %194
  %198 = sext i32 %30 to i64
  %199 = getelementptr inbounds i8, i8* %11, i64 %198
  br label %203

200:                                              ; preds = %194, %192
  %201 = phi i32 [ %30, %192 ], [ %195, %194 ]
  %202 = getelementptr inbounds i8, i8* %32, i64 8
  br label %203

203:                                              ; preds = %200, %197
  %204 = phi i32 [ %201, %200 ], [ %195, %197 ]
  %205 = phi i8* [ %202, %200 ], [ %32, %197 ]
  %206 = phi i8* [ %32, %200 ], [ %199, %197 ]
  %207 = bitcast i8* %206 to %struct._jobject**
  %208 = load %struct._jobject*, %struct._jobject** %207, align 8
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %210 = bitcast %union.jvalue* %209 to %struct._jobject**
  store %struct._jobject* %208, %struct._jobject** %210, align 8, !tbaa !23
  br label %211

211:                                              ; preds = %203, %183, %164, %146, %127, %107, %87, %67, %47, %29
  %212 = phi i32 [ %30, %29 ], [ %204, %203 ], [ %30, %183 ], [ %30, %164 ], [ %147, %146 ], [ %128, %127 ], [ %108, %107 ], [ %88, %87 ], [ %68, %67 ], [ %48, %47 ]
  %213 = phi i32 [ %31, %29 ], [ %31, %203 ], [ %184, %183 ], [ %165, %164 ], [ %31, %146 ], [ %31, %127 ], [ %31, %107 ], [ %31, %87 ], [ %31, %67 ], [ %31, %47 ]
  %214 = phi i8* [ %32, %29 ], [ %205, %203 ], [ %185, %183 ], [ %166, %164 ], [ %148, %146 ], [ %129, %127 ], [ %109, %107 ], [ %89, %87 ], [ %69, %67 ], [ %49, %47 ]
  %215 = add nuw nsw i64 %33, 1
  %216 = icmp eq i64 %215, %28
  br i1 %216, label %217, label %29, !llvm.loop !45

217:                                              ; preds = %211, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %19) #4
  %218 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %219 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %218, i64 0, i32 75
  %220 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %219, align 8, !tbaa !46
  %221 = call i16 %220(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %25) #4
  call void @llvm.stackrestore(i8* %18)
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %7) #4
  ret i16 %221
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallNonvirtualCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !11
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !21
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #4
  %12 = sext i32 %11 to i64
  %13 = alloca %union.jvalue, i64 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %204

15:                                               ; preds = %5
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 3
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 1
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 0
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 4
  %20 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 2
  %21 = zext i32 %11 to i64
  br label %22

22:                                               ; preds = %15, %201
  %23 = phi i64 [ 0, %15 ], [ %202, %201 ]
  %24 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 %23
  %25 = load i8, i8* %24, align 1, !tbaa !23
  switch i8 %25, label %201 [
    i8 90, label %26
    i8 66, label %46
    i8 83, label %66
    i8 67, label %86
    i8 73, label %106
    i8 74, label %125
    i8 68, label %143
    i8 70, label %162
    i8 76, label %182
  ]

26:                                               ; preds = %22
  %27 = load i32, i32* %16, align 8
  %28 = icmp sgt i32 %27, -1
  br i1 %28, label %36, label %29

29:                                               ; preds = %26
  %30 = add nsw i32 %27, 8
  store i32 %30, i32* %16, align 8
  %31 = icmp ult i32 %27, -7
  br i1 %31, label %32, label %36

32:                                               ; preds = %29
  %33 = load i8*, i8** %17, align 8
  %34 = sext i32 %27 to i64
  %35 = getelementptr inbounds i8, i8* %33, i64 %34
  br label %39

36:                                               ; preds = %29, %26
  %37 = load i8*, i8** %18, align 8
  %38 = getelementptr inbounds i8, i8* %37, i64 8
  store i8* %38, i8** %18, align 8
  br label %39

39:                                               ; preds = %36, %32
  %40 = phi i8* [ %35, %32 ], [ %37, %36 ]
  %41 = bitcast i8* %40 to i32*
  %42 = load i32, i32* %41, align 8
  %43 = trunc i32 %42 to i8
  %44 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %43, i8* %45, align 8, !tbaa !23
  br label %201

46:                                               ; preds = %22
  %47 = load i32, i32* %16, align 8
  %48 = icmp sgt i32 %47, -1
  br i1 %48, label %56, label %49

49:                                               ; preds = %46
  %50 = add nsw i32 %47, 8
  store i32 %50, i32* %16, align 8
  %51 = icmp ult i32 %47, -7
  br i1 %51, label %52, label %56

52:                                               ; preds = %49
  %53 = load i8*, i8** %17, align 8
  %54 = sext i32 %47 to i64
  %55 = getelementptr inbounds i8, i8* %53, i64 %54
  br label %59

56:                                               ; preds = %49, %46
  %57 = load i8*, i8** %18, align 8
  %58 = getelementptr inbounds i8, i8* %57, i64 8
  store i8* %58, i8** %18, align 8
  br label %59

59:                                               ; preds = %56, %52
  %60 = phi i8* [ %55, %52 ], [ %57, %56 ]
  %61 = bitcast i8* %60 to i32*
  %62 = load i32, i32* %61, align 8
  %63 = trunc i32 %62 to i8
  %64 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %63, i8* %65, align 8, !tbaa !23
  br label %201

66:                                               ; preds = %22
  %67 = load i32, i32* %16, align 8
  %68 = icmp sgt i32 %67, -1
  br i1 %68, label %76, label %69

69:                                               ; preds = %66
  %70 = add nsw i32 %67, 8
  store i32 %70, i32* %16, align 8
  %71 = icmp ult i32 %67, -7
  br i1 %71, label %72, label %76

72:                                               ; preds = %69
  %73 = load i8*, i8** %17, align 8
  %74 = sext i32 %67 to i64
  %75 = getelementptr inbounds i8, i8* %73, i64 %74
  br label %79

76:                                               ; preds = %69, %66
  %77 = load i8*, i8** %18, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %18, align 8
  br label %79

79:                                               ; preds = %76, %72
  %80 = phi i8* [ %75, %72 ], [ %77, %76 ]
  %81 = bitcast i8* %80 to i32*
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i16
  %84 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %85 = bitcast %union.jvalue* %84 to i16*
  store i16 %83, i16* %85, align 8, !tbaa !23
  br label %201

86:                                               ; preds = %22
  %87 = load i32, i32* %16, align 8
  %88 = icmp sgt i32 %87, -1
  br i1 %88, label %96, label %89

89:                                               ; preds = %86
  %90 = add nsw i32 %87, 8
  store i32 %90, i32* %16, align 8
  %91 = icmp ult i32 %87, -7
  br i1 %91, label %92, label %96

92:                                               ; preds = %89
  %93 = load i8*, i8** %17, align 8
  %94 = sext i32 %87 to i64
  %95 = getelementptr inbounds i8, i8* %93, i64 %94
  br label %99

96:                                               ; preds = %89, %86
  %97 = load i8*, i8** %18, align 8
  %98 = getelementptr inbounds i8, i8* %97, i64 8
  store i8* %98, i8** %18, align 8
  br label %99

99:                                               ; preds = %96, %92
  %100 = phi i8* [ %95, %92 ], [ %97, %96 ]
  %101 = bitcast i8* %100 to i32*
  %102 = load i32, i32* %101, align 8
  %103 = and i32 %102, 65535
  %104 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %103, i32* %105, align 8, !tbaa !23
  br label %201

106:                                              ; preds = %22
  %107 = load i32, i32* %16, align 8
  %108 = icmp sgt i32 %107, -1
  br i1 %108, label %116, label %109

109:                                              ; preds = %106
  %110 = add nsw i32 %107, 8
  store i32 %110, i32* %16, align 8
  %111 = icmp ult i32 %107, -7
  br i1 %111, label %112, label %116

112:                                              ; preds = %109
  %113 = load i8*, i8** %17, align 8
  %114 = sext i32 %107 to i64
  %115 = getelementptr inbounds i8, i8* %113, i64 %114
  br label %119

116:                                              ; preds = %109, %106
  %117 = load i8*, i8** %18, align 8
  %118 = getelementptr inbounds i8, i8* %117, i64 8
  store i8* %118, i8** %18, align 8
  br label %119

119:                                              ; preds = %116, %112
  %120 = phi i8* [ %115, %112 ], [ %117, %116 ]
  %121 = bitcast i8* %120 to i32*
  %122 = load i32, i32* %121, align 8
  %123 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %124 = bitcast %union.jvalue* %123 to i32*
  store i32 %122, i32* %124, align 8, !tbaa !23
  br label %201

125:                                              ; preds = %22
  %126 = load i32, i32* %16, align 8
  %127 = icmp sgt i32 %126, -1
  br i1 %127, label %135, label %128

128:                                              ; preds = %125
  %129 = add nsw i32 %126, 8
  store i32 %129, i32* %16, align 8
  %130 = icmp ult i32 %126, -7
  br i1 %130, label %131, label %135

131:                                              ; preds = %128
  %132 = load i8*, i8** %17, align 8
  %133 = sext i32 %126 to i64
  %134 = getelementptr inbounds i8, i8* %132, i64 %133
  br label %138

135:                                              ; preds = %128, %125
  %136 = load i8*, i8** %18, align 8
  %137 = getelementptr inbounds i8, i8* %136, i64 8
  store i8* %137, i8** %18, align 8
  br label %138

138:                                              ; preds = %135, %131
  %139 = phi i8* [ %134, %131 ], [ %136, %135 ]
  %140 = bitcast i8* %139 to i64*
  %141 = load i64, i64* %140, align 8
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23, i32 0
  store i64 %141, i64* %142, align 8, !tbaa !23
  br label %201

143:                                              ; preds = %22
  %144 = load i32, i32* %19, align 4
  %145 = icmp sgt i32 %144, -1
  br i1 %145, label %153, label %146

146:                                              ; preds = %143
  %147 = add nsw i32 %144, 16
  store i32 %147, i32* %19, align 4
  %148 = icmp ult i32 %144, -15
  br i1 %148, label %149, label %153

149:                                              ; preds = %146
  %150 = load i8*, i8** %20, align 8
  %151 = sext i32 %144 to i64
  %152 = getelementptr inbounds i8, i8* %150, i64 %151
  br label %156

153:                                              ; preds = %146, %143
  %154 = load i8*, i8** %18, align 8
  %155 = getelementptr inbounds i8, i8* %154, i64 8
  store i8* %155, i8** %18, align 8
  br label %156

156:                                              ; preds = %153, %149
  %157 = phi i8* [ %152, %149 ], [ %154, %153 ]
  %158 = bitcast i8* %157 to double*
  %159 = load double, double* %158, align 8
  %160 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %161 = bitcast %union.jvalue* %160 to double*
  store double %159, double* %161, align 8, !tbaa !23
  br label %201

162:                                              ; preds = %22
  %163 = load i32, i32* %19, align 4
  %164 = icmp sgt i32 %163, -1
  br i1 %164, label %172, label %165

165:                                              ; preds = %162
  %166 = add nsw i32 %163, 16
  store i32 %166, i32* %19, align 4
  %167 = icmp ult i32 %163, -15
  br i1 %167, label %168, label %172

168:                                              ; preds = %165
  %169 = load i8*, i8** %20, align 8
  %170 = sext i32 %163 to i64
  %171 = getelementptr inbounds i8, i8* %169, i64 %170
  br label %175

172:                                              ; preds = %165, %162
  %173 = load i8*, i8** %18, align 8
  %174 = getelementptr inbounds i8, i8* %173, i64 8
  store i8* %174, i8** %18, align 8
  br label %175

175:                                              ; preds = %172, %168
  %176 = phi i8* [ %171, %168 ], [ %173, %172 ]
  %177 = bitcast i8* %176 to double*
  %178 = load double, double* %177, align 8
  %179 = fptrunc double %178 to float
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %181 = bitcast %union.jvalue* %180 to float*
  store float %179, float* %181, align 8, !tbaa !23
  br label %201

182:                                              ; preds = %22
  %183 = load i32, i32* %16, align 8
  %184 = icmp sgt i32 %183, -1
  br i1 %184, label %192, label %185

185:                                              ; preds = %182
  %186 = add nsw i32 %183, 8
  store i32 %186, i32* %16, align 8
  %187 = icmp ult i32 %183, -7
  br i1 %187, label %188, label %192

188:                                              ; preds = %185
  %189 = load i8*, i8** %17, align 8
  %190 = sext i32 %183 to i64
  %191 = getelementptr inbounds i8, i8* %189, i64 %190
  br label %195

192:                                              ; preds = %185, %182
  %193 = load i8*, i8** %18, align 8
  %194 = getelementptr inbounds i8, i8* %193, i64 8
  store i8* %194, i8** %18, align 8
  br label %195

195:                                              ; preds = %192, %188
  %196 = phi i8* [ %191, %188 ], [ %193, %192 ]
  %197 = bitcast i8* %196 to %struct._jobject**
  %198 = load %struct._jobject*, %struct._jobject** %197, align 8
  %199 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %200 = bitcast %union.jvalue* %199 to %struct._jobject**
  store %struct._jobject* %198, %struct._jobject** %200, align 8, !tbaa !23
  br label %201

201:                                              ; preds = %39, %59, %79, %99, %119, %138, %156, %175, %195, %22
  %202 = add nuw nsw i64 %23, 1
  %203 = icmp eq i64 %202, %21
  br i1 %203, label %204, label %22, !llvm.loop !45

204:                                              ; preds = %201, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %205 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %206 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %205, i64 0, i32 75
  %207 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %206, align 8, !tbaa !46
  %208 = call i16 %207(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret i16 %208
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallStaticCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !47

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 125
  %219 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !48
  %220 = call i16 %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i16 %220
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallStaticCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !47

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 125
  %206 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !48
  %207 = call i16 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i16 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !49

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 48
  %219 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !50
  %220 = call i16 %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i16 %220
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !49

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 48
  %206 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !50
  %207 = call i16 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i16 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallNonvirtualShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca %"struct.std::__va_list", align 8
  %7 = bitcast %"struct.std::__va_list"* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 0
  %9 = load i8*, i8** %8, align 8, !tbaa.struct !10
  %10 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 1
  %11 = load i8*, i8** %10, align 8, !tbaa.struct !17
  %12 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 2
  %13 = load i8*, i8** %12, align 8, !tbaa.struct !18
  %14 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 3
  %15 = load i32, i32* %14, align 8, !tbaa.struct !19
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 4
  %17 = load i32, i32* %16, align 4, !tbaa.struct !20
  %18 = call i8* @llvm.stacksave()
  %19 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %19) #4
  %20 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %20, align 8, !tbaa !11
  %22 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %21, align 8, !tbaa !21
  %23 = call i32 %22(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %19) #4
  %24 = sext i32 %23 to i64
  %25 = alloca %union.jvalue, i64 %24, align 16
  %26 = icmp sgt i32 %23, 0
  br i1 %26, label %27, label %217

27:                                               ; preds = %4
  %28 = zext i32 %23 to i64
  br label %29

29:                                               ; preds = %211, %27
  %30 = phi i32 [ %15, %27 ], [ %212, %211 ]
  %31 = phi i32 [ %17, %27 ], [ %213, %211 ]
  %32 = phi i8* [ %9, %27 ], [ %214, %211 ]
  %33 = phi i64 [ 0, %27 ], [ %215, %211 ]
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1, !tbaa !23
  switch i8 %35, label %211 [
    i8 90, label %36
    i8 66, label %56
    i8 83, label %76
    i8 67, label %96
    i8 73, label %116
    i8 74, label %135
    i8 68, label %153
    i8 70, label %172
    i8 76, label %192
  ]

36:                                               ; preds = %29
  %37 = icmp sgt i32 %30, -1
  br i1 %37, label %44, label %38

38:                                               ; preds = %36
  %39 = add nsw i32 %30, 8
  %40 = icmp ult i32 %30, -7
  br i1 %40, label %41, label %44

41:                                               ; preds = %38
  %42 = sext i32 %30 to i64
  %43 = getelementptr inbounds i8, i8* %11, i64 %42
  br label %47

44:                                               ; preds = %38, %36
  %45 = phi i32 [ %30, %36 ], [ %39, %38 ]
  %46 = getelementptr inbounds i8, i8* %32, i64 8
  br label %47

47:                                               ; preds = %44, %41
  %48 = phi i32 [ %45, %44 ], [ %39, %41 ]
  %49 = phi i8* [ %46, %44 ], [ %32, %41 ]
  %50 = phi i8* [ %32, %44 ], [ %43, %41 ]
  %51 = bitcast i8* %50 to i32*
  %52 = load i32, i32* %51, align 8
  %53 = trunc i32 %52 to i8
  %54 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %55 = bitcast %union.jvalue* %54 to i8*
  store i8 %53, i8* %55, align 8, !tbaa !23
  br label %211

56:                                               ; preds = %29
  %57 = icmp sgt i32 %30, -1
  br i1 %57, label %64, label %58

58:                                               ; preds = %56
  %59 = add nsw i32 %30, 8
  %60 = icmp ult i32 %30, -7
  br i1 %60, label %61, label %64

61:                                               ; preds = %58
  %62 = sext i32 %30 to i64
  %63 = getelementptr inbounds i8, i8* %11, i64 %62
  br label %67

64:                                               ; preds = %58, %56
  %65 = phi i32 [ %30, %56 ], [ %59, %58 ]
  %66 = getelementptr inbounds i8, i8* %32, i64 8
  br label %67

67:                                               ; preds = %64, %61
  %68 = phi i32 [ %65, %64 ], [ %59, %61 ]
  %69 = phi i8* [ %66, %64 ], [ %32, %61 ]
  %70 = phi i8* [ %32, %64 ], [ %63, %61 ]
  %71 = bitcast i8* %70 to i32*
  %72 = load i32, i32* %71, align 8
  %73 = trunc i32 %72 to i8
  %74 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %75 = bitcast %union.jvalue* %74 to i8*
  store i8 %73, i8* %75, align 8, !tbaa !23
  br label %211

76:                                               ; preds = %29
  %77 = icmp sgt i32 %30, -1
  br i1 %77, label %84, label %78

78:                                               ; preds = %76
  %79 = add nsw i32 %30, 8
  %80 = icmp ult i32 %30, -7
  br i1 %80, label %81, label %84

81:                                               ; preds = %78
  %82 = sext i32 %30 to i64
  %83 = getelementptr inbounds i8, i8* %11, i64 %82
  br label %87

84:                                               ; preds = %78, %76
  %85 = phi i32 [ %30, %76 ], [ %79, %78 ]
  %86 = getelementptr inbounds i8, i8* %32, i64 8
  br label %87

87:                                               ; preds = %84, %81
  %88 = phi i32 [ %85, %84 ], [ %79, %81 ]
  %89 = phi i8* [ %86, %84 ], [ %32, %81 ]
  %90 = phi i8* [ %32, %84 ], [ %83, %81 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 8
  %93 = trunc i32 %92 to i16
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %93, i16* %95, align 8, !tbaa !23
  br label %211

96:                                               ; preds = %29
  %97 = icmp sgt i32 %30, -1
  br i1 %97, label %104, label %98

98:                                               ; preds = %96
  %99 = add nsw i32 %30, 8
  %100 = icmp ult i32 %30, -7
  br i1 %100, label %101, label %104

101:                                              ; preds = %98
  %102 = sext i32 %30 to i64
  %103 = getelementptr inbounds i8, i8* %11, i64 %102
  br label %107

104:                                              ; preds = %98, %96
  %105 = phi i32 [ %30, %96 ], [ %99, %98 ]
  %106 = getelementptr inbounds i8, i8* %32, i64 8
  br label %107

107:                                              ; preds = %104, %101
  %108 = phi i32 [ %105, %104 ], [ %99, %101 ]
  %109 = phi i8* [ %106, %104 ], [ %32, %101 ]
  %110 = phi i8* [ %32, %104 ], [ %103, %101 ]
  %111 = bitcast i8* %110 to i32*
  %112 = load i32, i32* %111, align 8
  %113 = and i32 %112, 65535
  %114 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %115 = bitcast %union.jvalue* %114 to i32*
  store i32 %113, i32* %115, align 8, !tbaa !23
  br label %211

116:                                              ; preds = %29
  %117 = icmp sgt i32 %30, -1
  br i1 %117, label %124, label %118

118:                                              ; preds = %116
  %119 = add nsw i32 %30, 8
  %120 = icmp ult i32 %30, -7
  br i1 %120, label %121, label %124

121:                                              ; preds = %118
  %122 = sext i32 %30 to i64
  %123 = getelementptr inbounds i8, i8* %11, i64 %122
  br label %127

124:                                              ; preds = %118, %116
  %125 = phi i32 [ %30, %116 ], [ %119, %118 ]
  %126 = getelementptr inbounds i8, i8* %32, i64 8
  br label %127

127:                                              ; preds = %124, %121
  %128 = phi i32 [ %125, %124 ], [ %119, %121 ]
  %129 = phi i8* [ %126, %124 ], [ %32, %121 ]
  %130 = phi i8* [ %32, %124 ], [ %123, %121 ]
  %131 = bitcast i8* %130 to i32*
  %132 = load i32, i32* %131, align 8
  %133 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %134 = bitcast %union.jvalue* %133 to i32*
  store i32 %132, i32* %134, align 8, !tbaa !23
  br label %211

135:                                              ; preds = %29
  %136 = icmp sgt i32 %30, -1
  br i1 %136, label %143, label %137

137:                                              ; preds = %135
  %138 = add nsw i32 %30, 8
  %139 = icmp ult i32 %30, -7
  br i1 %139, label %140, label %143

140:                                              ; preds = %137
  %141 = sext i32 %30 to i64
  %142 = getelementptr inbounds i8, i8* %11, i64 %141
  br label %146

143:                                              ; preds = %137, %135
  %144 = phi i32 [ %30, %135 ], [ %138, %137 ]
  %145 = getelementptr inbounds i8, i8* %32, i64 8
  br label %146

146:                                              ; preds = %143, %140
  %147 = phi i32 [ %144, %143 ], [ %138, %140 ]
  %148 = phi i8* [ %145, %143 ], [ %32, %140 ]
  %149 = phi i8* [ %32, %143 ], [ %142, %140 ]
  %150 = bitcast i8* %149 to i64*
  %151 = load i64, i64* %150, align 8
  %152 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33, i32 0
  store i64 %151, i64* %152, align 8, !tbaa !23
  br label %211

153:                                              ; preds = %29
  %154 = icmp sgt i32 %31, -1
  br i1 %154, label %161, label %155

155:                                              ; preds = %153
  %156 = add nsw i32 %31, 16
  %157 = icmp ult i32 %31, -15
  br i1 %157, label %158, label %161

158:                                              ; preds = %155
  %159 = sext i32 %31 to i64
  %160 = getelementptr inbounds i8, i8* %13, i64 %159
  br label %164

161:                                              ; preds = %155, %153
  %162 = phi i32 [ %31, %153 ], [ %156, %155 ]
  %163 = getelementptr inbounds i8, i8* %32, i64 8
  br label %164

164:                                              ; preds = %161, %158
  %165 = phi i32 [ %162, %161 ], [ %156, %158 ]
  %166 = phi i8* [ %163, %161 ], [ %32, %158 ]
  %167 = phi i8* [ %32, %161 ], [ %160, %158 ]
  %168 = bitcast i8* %167 to double*
  %169 = load double, double* %168, align 8
  %170 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %171 = bitcast %union.jvalue* %170 to double*
  store double %169, double* %171, align 8, !tbaa !23
  br label %211

172:                                              ; preds = %29
  %173 = icmp sgt i32 %31, -1
  br i1 %173, label %180, label %174

174:                                              ; preds = %172
  %175 = add nsw i32 %31, 16
  %176 = icmp ult i32 %31, -15
  br i1 %176, label %177, label %180

177:                                              ; preds = %174
  %178 = sext i32 %31 to i64
  %179 = getelementptr inbounds i8, i8* %13, i64 %178
  br label %183

180:                                              ; preds = %174, %172
  %181 = phi i32 [ %31, %172 ], [ %175, %174 ]
  %182 = getelementptr inbounds i8, i8* %32, i64 8
  br label %183

183:                                              ; preds = %180, %177
  %184 = phi i32 [ %181, %180 ], [ %175, %177 ]
  %185 = phi i8* [ %182, %180 ], [ %32, %177 ]
  %186 = phi i8* [ %32, %180 ], [ %179, %177 ]
  %187 = bitcast i8* %186 to double*
  %188 = load double, double* %187, align 8
  %189 = fptrunc double %188 to float
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %191 = bitcast %union.jvalue* %190 to float*
  store float %189, float* %191, align 8, !tbaa !23
  br label %211

192:                                              ; preds = %29
  %193 = icmp sgt i32 %30, -1
  br i1 %193, label %200, label %194

194:                                              ; preds = %192
  %195 = add nsw i32 %30, 8
  %196 = icmp ult i32 %30, -7
  br i1 %196, label %197, label %200

197:                                              ; preds = %194
  %198 = sext i32 %30 to i64
  %199 = getelementptr inbounds i8, i8* %11, i64 %198
  br label %203

200:                                              ; preds = %194, %192
  %201 = phi i32 [ %30, %192 ], [ %195, %194 ]
  %202 = getelementptr inbounds i8, i8* %32, i64 8
  br label %203

203:                                              ; preds = %200, %197
  %204 = phi i32 [ %201, %200 ], [ %195, %197 ]
  %205 = phi i8* [ %202, %200 ], [ %32, %197 ]
  %206 = phi i8* [ %32, %200 ], [ %199, %197 ]
  %207 = bitcast i8* %206 to %struct._jobject**
  %208 = load %struct._jobject*, %struct._jobject** %207, align 8
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %210 = bitcast %union.jvalue* %209 to %struct._jobject**
  store %struct._jobject* %208, %struct._jobject** %210, align 8, !tbaa !23
  br label %211

211:                                              ; preds = %203, %183, %164, %146, %127, %107, %87, %67, %47, %29
  %212 = phi i32 [ %30, %29 ], [ %204, %203 ], [ %30, %183 ], [ %30, %164 ], [ %147, %146 ], [ %128, %127 ], [ %108, %107 ], [ %88, %87 ], [ %68, %67 ], [ %48, %47 ]
  %213 = phi i32 [ %31, %29 ], [ %31, %203 ], [ %184, %183 ], [ %165, %164 ], [ %31, %146 ], [ %31, %127 ], [ %31, %107 ], [ %31, %87 ], [ %31, %67 ], [ %31, %47 ]
  %214 = phi i8* [ %32, %29 ], [ %205, %203 ], [ %185, %183 ], [ %166, %164 ], [ %148, %146 ], [ %129, %127 ], [ %109, %107 ], [ %89, %87 ], [ %69, %67 ], [ %49, %47 ]
  %215 = add nuw nsw i64 %33, 1
  %216 = icmp eq i64 %215, %28
  br i1 %216, label %217, label %29, !llvm.loop !51

217:                                              ; preds = %211, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %19) #4
  %218 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %219 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %218, i64 0, i32 78
  %220 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %219, align 8, !tbaa !52
  %221 = call i16 %220(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %25) #4
  call void @llvm.stackrestore(i8* %18)
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %7) #4
  ret i16 %221
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallNonvirtualShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !11
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !21
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #4
  %12 = sext i32 %11 to i64
  %13 = alloca %union.jvalue, i64 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %204

15:                                               ; preds = %5
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 3
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 1
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 0
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 4
  %20 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 2
  %21 = zext i32 %11 to i64
  br label %22

22:                                               ; preds = %15, %201
  %23 = phi i64 [ 0, %15 ], [ %202, %201 ]
  %24 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 %23
  %25 = load i8, i8* %24, align 1, !tbaa !23
  switch i8 %25, label %201 [
    i8 90, label %26
    i8 66, label %46
    i8 83, label %66
    i8 67, label %86
    i8 73, label %106
    i8 74, label %125
    i8 68, label %143
    i8 70, label %162
    i8 76, label %182
  ]

26:                                               ; preds = %22
  %27 = load i32, i32* %16, align 8
  %28 = icmp sgt i32 %27, -1
  br i1 %28, label %36, label %29

29:                                               ; preds = %26
  %30 = add nsw i32 %27, 8
  store i32 %30, i32* %16, align 8
  %31 = icmp ult i32 %27, -7
  br i1 %31, label %32, label %36

32:                                               ; preds = %29
  %33 = load i8*, i8** %17, align 8
  %34 = sext i32 %27 to i64
  %35 = getelementptr inbounds i8, i8* %33, i64 %34
  br label %39

36:                                               ; preds = %29, %26
  %37 = load i8*, i8** %18, align 8
  %38 = getelementptr inbounds i8, i8* %37, i64 8
  store i8* %38, i8** %18, align 8
  br label %39

39:                                               ; preds = %36, %32
  %40 = phi i8* [ %35, %32 ], [ %37, %36 ]
  %41 = bitcast i8* %40 to i32*
  %42 = load i32, i32* %41, align 8
  %43 = trunc i32 %42 to i8
  %44 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %43, i8* %45, align 8, !tbaa !23
  br label %201

46:                                               ; preds = %22
  %47 = load i32, i32* %16, align 8
  %48 = icmp sgt i32 %47, -1
  br i1 %48, label %56, label %49

49:                                               ; preds = %46
  %50 = add nsw i32 %47, 8
  store i32 %50, i32* %16, align 8
  %51 = icmp ult i32 %47, -7
  br i1 %51, label %52, label %56

52:                                               ; preds = %49
  %53 = load i8*, i8** %17, align 8
  %54 = sext i32 %47 to i64
  %55 = getelementptr inbounds i8, i8* %53, i64 %54
  br label %59

56:                                               ; preds = %49, %46
  %57 = load i8*, i8** %18, align 8
  %58 = getelementptr inbounds i8, i8* %57, i64 8
  store i8* %58, i8** %18, align 8
  br label %59

59:                                               ; preds = %56, %52
  %60 = phi i8* [ %55, %52 ], [ %57, %56 ]
  %61 = bitcast i8* %60 to i32*
  %62 = load i32, i32* %61, align 8
  %63 = trunc i32 %62 to i8
  %64 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %63, i8* %65, align 8, !tbaa !23
  br label %201

66:                                               ; preds = %22
  %67 = load i32, i32* %16, align 8
  %68 = icmp sgt i32 %67, -1
  br i1 %68, label %76, label %69

69:                                               ; preds = %66
  %70 = add nsw i32 %67, 8
  store i32 %70, i32* %16, align 8
  %71 = icmp ult i32 %67, -7
  br i1 %71, label %72, label %76

72:                                               ; preds = %69
  %73 = load i8*, i8** %17, align 8
  %74 = sext i32 %67 to i64
  %75 = getelementptr inbounds i8, i8* %73, i64 %74
  br label %79

76:                                               ; preds = %69, %66
  %77 = load i8*, i8** %18, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %18, align 8
  br label %79

79:                                               ; preds = %76, %72
  %80 = phi i8* [ %75, %72 ], [ %77, %76 ]
  %81 = bitcast i8* %80 to i32*
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i16
  %84 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %85 = bitcast %union.jvalue* %84 to i16*
  store i16 %83, i16* %85, align 8, !tbaa !23
  br label %201

86:                                               ; preds = %22
  %87 = load i32, i32* %16, align 8
  %88 = icmp sgt i32 %87, -1
  br i1 %88, label %96, label %89

89:                                               ; preds = %86
  %90 = add nsw i32 %87, 8
  store i32 %90, i32* %16, align 8
  %91 = icmp ult i32 %87, -7
  br i1 %91, label %92, label %96

92:                                               ; preds = %89
  %93 = load i8*, i8** %17, align 8
  %94 = sext i32 %87 to i64
  %95 = getelementptr inbounds i8, i8* %93, i64 %94
  br label %99

96:                                               ; preds = %89, %86
  %97 = load i8*, i8** %18, align 8
  %98 = getelementptr inbounds i8, i8* %97, i64 8
  store i8* %98, i8** %18, align 8
  br label %99

99:                                               ; preds = %96, %92
  %100 = phi i8* [ %95, %92 ], [ %97, %96 ]
  %101 = bitcast i8* %100 to i32*
  %102 = load i32, i32* %101, align 8
  %103 = and i32 %102, 65535
  %104 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %103, i32* %105, align 8, !tbaa !23
  br label %201

106:                                              ; preds = %22
  %107 = load i32, i32* %16, align 8
  %108 = icmp sgt i32 %107, -1
  br i1 %108, label %116, label %109

109:                                              ; preds = %106
  %110 = add nsw i32 %107, 8
  store i32 %110, i32* %16, align 8
  %111 = icmp ult i32 %107, -7
  br i1 %111, label %112, label %116

112:                                              ; preds = %109
  %113 = load i8*, i8** %17, align 8
  %114 = sext i32 %107 to i64
  %115 = getelementptr inbounds i8, i8* %113, i64 %114
  br label %119

116:                                              ; preds = %109, %106
  %117 = load i8*, i8** %18, align 8
  %118 = getelementptr inbounds i8, i8* %117, i64 8
  store i8* %118, i8** %18, align 8
  br label %119

119:                                              ; preds = %116, %112
  %120 = phi i8* [ %115, %112 ], [ %117, %116 ]
  %121 = bitcast i8* %120 to i32*
  %122 = load i32, i32* %121, align 8
  %123 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %124 = bitcast %union.jvalue* %123 to i32*
  store i32 %122, i32* %124, align 8, !tbaa !23
  br label %201

125:                                              ; preds = %22
  %126 = load i32, i32* %16, align 8
  %127 = icmp sgt i32 %126, -1
  br i1 %127, label %135, label %128

128:                                              ; preds = %125
  %129 = add nsw i32 %126, 8
  store i32 %129, i32* %16, align 8
  %130 = icmp ult i32 %126, -7
  br i1 %130, label %131, label %135

131:                                              ; preds = %128
  %132 = load i8*, i8** %17, align 8
  %133 = sext i32 %126 to i64
  %134 = getelementptr inbounds i8, i8* %132, i64 %133
  br label %138

135:                                              ; preds = %128, %125
  %136 = load i8*, i8** %18, align 8
  %137 = getelementptr inbounds i8, i8* %136, i64 8
  store i8* %137, i8** %18, align 8
  br label %138

138:                                              ; preds = %135, %131
  %139 = phi i8* [ %134, %131 ], [ %136, %135 ]
  %140 = bitcast i8* %139 to i64*
  %141 = load i64, i64* %140, align 8
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23, i32 0
  store i64 %141, i64* %142, align 8, !tbaa !23
  br label %201

143:                                              ; preds = %22
  %144 = load i32, i32* %19, align 4
  %145 = icmp sgt i32 %144, -1
  br i1 %145, label %153, label %146

146:                                              ; preds = %143
  %147 = add nsw i32 %144, 16
  store i32 %147, i32* %19, align 4
  %148 = icmp ult i32 %144, -15
  br i1 %148, label %149, label %153

149:                                              ; preds = %146
  %150 = load i8*, i8** %20, align 8
  %151 = sext i32 %144 to i64
  %152 = getelementptr inbounds i8, i8* %150, i64 %151
  br label %156

153:                                              ; preds = %146, %143
  %154 = load i8*, i8** %18, align 8
  %155 = getelementptr inbounds i8, i8* %154, i64 8
  store i8* %155, i8** %18, align 8
  br label %156

156:                                              ; preds = %153, %149
  %157 = phi i8* [ %152, %149 ], [ %154, %153 ]
  %158 = bitcast i8* %157 to double*
  %159 = load double, double* %158, align 8
  %160 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %161 = bitcast %union.jvalue* %160 to double*
  store double %159, double* %161, align 8, !tbaa !23
  br label %201

162:                                              ; preds = %22
  %163 = load i32, i32* %19, align 4
  %164 = icmp sgt i32 %163, -1
  br i1 %164, label %172, label %165

165:                                              ; preds = %162
  %166 = add nsw i32 %163, 16
  store i32 %166, i32* %19, align 4
  %167 = icmp ult i32 %163, -15
  br i1 %167, label %168, label %172

168:                                              ; preds = %165
  %169 = load i8*, i8** %20, align 8
  %170 = sext i32 %163 to i64
  %171 = getelementptr inbounds i8, i8* %169, i64 %170
  br label %175

172:                                              ; preds = %165, %162
  %173 = load i8*, i8** %18, align 8
  %174 = getelementptr inbounds i8, i8* %173, i64 8
  store i8* %174, i8** %18, align 8
  br label %175

175:                                              ; preds = %172, %168
  %176 = phi i8* [ %171, %168 ], [ %173, %172 ]
  %177 = bitcast i8* %176 to double*
  %178 = load double, double* %177, align 8
  %179 = fptrunc double %178 to float
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %181 = bitcast %union.jvalue* %180 to float*
  store float %179, float* %181, align 8, !tbaa !23
  br label %201

182:                                              ; preds = %22
  %183 = load i32, i32* %16, align 8
  %184 = icmp sgt i32 %183, -1
  br i1 %184, label %192, label %185

185:                                              ; preds = %182
  %186 = add nsw i32 %183, 8
  store i32 %186, i32* %16, align 8
  %187 = icmp ult i32 %183, -7
  br i1 %187, label %188, label %192

188:                                              ; preds = %185
  %189 = load i8*, i8** %17, align 8
  %190 = sext i32 %183 to i64
  %191 = getelementptr inbounds i8, i8* %189, i64 %190
  br label %195

192:                                              ; preds = %185, %182
  %193 = load i8*, i8** %18, align 8
  %194 = getelementptr inbounds i8, i8* %193, i64 8
  store i8* %194, i8** %18, align 8
  br label %195

195:                                              ; preds = %192, %188
  %196 = phi i8* [ %191, %188 ], [ %193, %192 ]
  %197 = bitcast i8* %196 to %struct._jobject**
  %198 = load %struct._jobject*, %struct._jobject** %197, align 8
  %199 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %200 = bitcast %union.jvalue* %199 to %struct._jobject**
  store %struct._jobject* %198, %struct._jobject** %200, align 8, !tbaa !23
  br label %201

201:                                              ; preds = %39, %59, %79, %99, %119, %138, %156, %175, %195, %22
  %202 = add nuw nsw i64 %23, 1
  %203 = icmp eq i64 %202, %21
  br i1 %203, label %204, label %22, !llvm.loop !51

204:                                              ; preds = %201, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %205 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %206 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %205, i64 0, i32 78
  %207 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %206, align 8, !tbaa !52
  %208 = call i16 %207(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret i16 %208
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallStaticShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !53

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 128
  %219 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !54
  %220 = call i16 %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i16 %220
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallStaticShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !53

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 128
  %206 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !54
  %207 = call i16 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i16 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i32 @JNI_CallIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !55

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 51
  %219 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !56
  %220 = call i32 %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i32 %220
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i32 @JNI_CallIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !55

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 51
  %206 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !56
  %207 = call i32 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i32 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i32 @JNI_CallNonvirtualIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca %"struct.std::__va_list", align 8
  %7 = bitcast %"struct.std::__va_list"* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 0
  %9 = load i8*, i8** %8, align 8, !tbaa.struct !10
  %10 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 1
  %11 = load i8*, i8** %10, align 8, !tbaa.struct !17
  %12 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 2
  %13 = load i8*, i8** %12, align 8, !tbaa.struct !18
  %14 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 3
  %15 = load i32, i32* %14, align 8, !tbaa.struct !19
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 4
  %17 = load i32, i32* %16, align 4, !tbaa.struct !20
  %18 = call i8* @llvm.stacksave()
  %19 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %19) #4
  %20 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %20, align 8, !tbaa !11
  %22 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %21, align 8, !tbaa !21
  %23 = call i32 %22(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %19) #4
  %24 = sext i32 %23 to i64
  %25 = alloca %union.jvalue, i64 %24, align 16
  %26 = icmp sgt i32 %23, 0
  br i1 %26, label %27, label %217

27:                                               ; preds = %4
  %28 = zext i32 %23 to i64
  br label %29

29:                                               ; preds = %211, %27
  %30 = phi i32 [ %15, %27 ], [ %212, %211 ]
  %31 = phi i32 [ %17, %27 ], [ %213, %211 ]
  %32 = phi i8* [ %9, %27 ], [ %214, %211 ]
  %33 = phi i64 [ 0, %27 ], [ %215, %211 ]
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1, !tbaa !23
  switch i8 %35, label %211 [
    i8 90, label %36
    i8 66, label %56
    i8 83, label %76
    i8 67, label %96
    i8 73, label %116
    i8 74, label %135
    i8 68, label %153
    i8 70, label %172
    i8 76, label %192
  ]

36:                                               ; preds = %29
  %37 = icmp sgt i32 %30, -1
  br i1 %37, label %44, label %38

38:                                               ; preds = %36
  %39 = add nsw i32 %30, 8
  %40 = icmp ult i32 %30, -7
  br i1 %40, label %41, label %44

41:                                               ; preds = %38
  %42 = sext i32 %30 to i64
  %43 = getelementptr inbounds i8, i8* %11, i64 %42
  br label %47

44:                                               ; preds = %38, %36
  %45 = phi i32 [ %30, %36 ], [ %39, %38 ]
  %46 = getelementptr inbounds i8, i8* %32, i64 8
  br label %47

47:                                               ; preds = %44, %41
  %48 = phi i32 [ %45, %44 ], [ %39, %41 ]
  %49 = phi i8* [ %46, %44 ], [ %32, %41 ]
  %50 = phi i8* [ %32, %44 ], [ %43, %41 ]
  %51 = bitcast i8* %50 to i32*
  %52 = load i32, i32* %51, align 8
  %53 = trunc i32 %52 to i8
  %54 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %55 = bitcast %union.jvalue* %54 to i8*
  store i8 %53, i8* %55, align 8, !tbaa !23
  br label %211

56:                                               ; preds = %29
  %57 = icmp sgt i32 %30, -1
  br i1 %57, label %64, label %58

58:                                               ; preds = %56
  %59 = add nsw i32 %30, 8
  %60 = icmp ult i32 %30, -7
  br i1 %60, label %61, label %64

61:                                               ; preds = %58
  %62 = sext i32 %30 to i64
  %63 = getelementptr inbounds i8, i8* %11, i64 %62
  br label %67

64:                                               ; preds = %58, %56
  %65 = phi i32 [ %30, %56 ], [ %59, %58 ]
  %66 = getelementptr inbounds i8, i8* %32, i64 8
  br label %67

67:                                               ; preds = %64, %61
  %68 = phi i32 [ %65, %64 ], [ %59, %61 ]
  %69 = phi i8* [ %66, %64 ], [ %32, %61 ]
  %70 = phi i8* [ %32, %64 ], [ %63, %61 ]
  %71 = bitcast i8* %70 to i32*
  %72 = load i32, i32* %71, align 8
  %73 = trunc i32 %72 to i8
  %74 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %75 = bitcast %union.jvalue* %74 to i8*
  store i8 %73, i8* %75, align 8, !tbaa !23
  br label %211

76:                                               ; preds = %29
  %77 = icmp sgt i32 %30, -1
  br i1 %77, label %84, label %78

78:                                               ; preds = %76
  %79 = add nsw i32 %30, 8
  %80 = icmp ult i32 %30, -7
  br i1 %80, label %81, label %84

81:                                               ; preds = %78
  %82 = sext i32 %30 to i64
  %83 = getelementptr inbounds i8, i8* %11, i64 %82
  br label %87

84:                                               ; preds = %78, %76
  %85 = phi i32 [ %30, %76 ], [ %79, %78 ]
  %86 = getelementptr inbounds i8, i8* %32, i64 8
  br label %87

87:                                               ; preds = %84, %81
  %88 = phi i32 [ %85, %84 ], [ %79, %81 ]
  %89 = phi i8* [ %86, %84 ], [ %32, %81 ]
  %90 = phi i8* [ %32, %84 ], [ %83, %81 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 8
  %93 = trunc i32 %92 to i16
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %93, i16* %95, align 8, !tbaa !23
  br label %211

96:                                               ; preds = %29
  %97 = icmp sgt i32 %30, -1
  br i1 %97, label %104, label %98

98:                                               ; preds = %96
  %99 = add nsw i32 %30, 8
  %100 = icmp ult i32 %30, -7
  br i1 %100, label %101, label %104

101:                                              ; preds = %98
  %102 = sext i32 %30 to i64
  %103 = getelementptr inbounds i8, i8* %11, i64 %102
  br label %107

104:                                              ; preds = %98, %96
  %105 = phi i32 [ %30, %96 ], [ %99, %98 ]
  %106 = getelementptr inbounds i8, i8* %32, i64 8
  br label %107

107:                                              ; preds = %104, %101
  %108 = phi i32 [ %105, %104 ], [ %99, %101 ]
  %109 = phi i8* [ %106, %104 ], [ %32, %101 ]
  %110 = phi i8* [ %32, %104 ], [ %103, %101 ]
  %111 = bitcast i8* %110 to i32*
  %112 = load i32, i32* %111, align 8
  %113 = and i32 %112, 65535
  %114 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %115 = bitcast %union.jvalue* %114 to i32*
  store i32 %113, i32* %115, align 8, !tbaa !23
  br label %211

116:                                              ; preds = %29
  %117 = icmp sgt i32 %30, -1
  br i1 %117, label %124, label %118

118:                                              ; preds = %116
  %119 = add nsw i32 %30, 8
  %120 = icmp ult i32 %30, -7
  br i1 %120, label %121, label %124

121:                                              ; preds = %118
  %122 = sext i32 %30 to i64
  %123 = getelementptr inbounds i8, i8* %11, i64 %122
  br label %127

124:                                              ; preds = %118, %116
  %125 = phi i32 [ %30, %116 ], [ %119, %118 ]
  %126 = getelementptr inbounds i8, i8* %32, i64 8
  br label %127

127:                                              ; preds = %124, %121
  %128 = phi i32 [ %125, %124 ], [ %119, %121 ]
  %129 = phi i8* [ %126, %124 ], [ %32, %121 ]
  %130 = phi i8* [ %32, %124 ], [ %123, %121 ]
  %131 = bitcast i8* %130 to i32*
  %132 = load i32, i32* %131, align 8
  %133 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %134 = bitcast %union.jvalue* %133 to i32*
  store i32 %132, i32* %134, align 8, !tbaa !23
  br label %211

135:                                              ; preds = %29
  %136 = icmp sgt i32 %30, -1
  br i1 %136, label %143, label %137

137:                                              ; preds = %135
  %138 = add nsw i32 %30, 8
  %139 = icmp ult i32 %30, -7
  br i1 %139, label %140, label %143

140:                                              ; preds = %137
  %141 = sext i32 %30 to i64
  %142 = getelementptr inbounds i8, i8* %11, i64 %141
  br label %146

143:                                              ; preds = %137, %135
  %144 = phi i32 [ %30, %135 ], [ %138, %137 ]
  %145 = getelementptr inbounds i8, i8* %32, i64 8
  br label %146

146:                                              ; preds = %143, %140
  %147 = phi i32 [ %144, %143 ], [ %138, %140 ]
  %148 = phi i8* [ %145, %143 ], [ %32, %140 ]
  %149 = phi i8* [ %32, %143 ], [ %142, %140 ]
  %150 = bitcast i8* %149 to i64*
  %151 = load i64, i64* %150, align 8
  %152 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33, i32 0
  store i64 %151, i64* %152, align 8, !tbaa !23
  br label %211

153:                                              ; preds = %29
  %154 = icmp sgt i32 %31, -1
  br i1 %154, label %161, label %155

155:                                              ; preds = %153
  %156 = add nsw i32 %31, 16
  %157 = icmp ult i32 %31, -15
  br i1 %157, label %158, label %161

158:                                              ; preds = %155
  %159 = sext i32 %31 to i64
  %160 = getelementptr inbounds i8, i8* %13, i64 %159
  br label %164

161:                                              ; preds = %155, %153
  %162 = phi i32 [ %31, %153 ], [ %156, %155 ]
  %163 = getelementptr inbounds i8, i8* %32, i64 8
  br label %164

164:                                              ; preds = %161, %158
  %165 = phi i32 [ %162, %161 ], [ %156, %158 ]
  %166 = phi i8* [ %163, %161 ], [ %32, %158 ]
  %167 = phi i8* [ %32, %161 ], [ %160, %158 ]
  %168 = bitcast i8* %167 to double*
  %169 = load double, double* %168, align 8
  %170 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %171 = bitcast %union.jvalue* %170 to double*
  store double %169, double* %171, align 8, !tbaa !23
  br label %211

172:                                              ; preds = %29
  %173 = icmp sgt i32 %31, -1
  br i1 %173, label %180, label %174

174:                                              ; preds = %172
  %175 = add nsw i32 %31, 16
  %176 = icmp ult i32 %31, -15
  br i1 %176, label %177, label %180

177:                                              ; preds = %174
  %178 = sext i32 %31 to i64
  %179 = getelementptr inbounds i8, i8* %13, i64 %178
  br label %183

180:                                              ; preds = %174, %172
  %181 = phi i32 [ %31, %172 ], [ %175, %174 ]
  %182 = getelementptr inbounds i8, i8* %32, i64 8
  br label %183

183:                                              ; preds = %180, %177
  %184 = phi i32 [ %181, %180 ], [ %175, %177 ]
  %185 = phi i8* [ %182, %180 ], [ %32, %177 ]
  %186 = phi i8* [ %32, %180 ], [ %179, %177 ]
  %187 = bitcast i8* %186 to double*
  %188 = load double, double* %187, align 8
  %189 = fptrunc double %188 to float
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %191 = bitcast %union.jvalue* %190 to float*
  store float %189, float* %191, align 8, !tbaa !23
  br label %211

192:                                              ; preds = %29
  %193 = icmp sgt i32 %30, -1
  br i1 %193, label %200, label %194

194:                                              ; preds = %192
  %195 = add nsw i32 %30, 8
  %196 = icmp ult i32 %30, -7
  br i1 %196, label %197, label %200

197:                                              ; preds = %194
  %198 = sext i32 %30 to i64
  %199 = getelementptr inbounds i8, i8* %11, i64 %198
  br label %203

200:                                              ; preds = %194, %192
  %201 = phi i32 [ %30, %192 ], [ %195, %194 ]
  %202 = getelementptr inbounds i8, i8* %32, i64 8
  br label %203

203:                                              ; preds = %200, %197
  %204 = phi i32 [ %201, %200 ], [ %195, %197 ]
  %205 = phi i8* [ %202, %200 ], [ %32, %197 ]
  %206 = phi i8* [ %32, %200 ], [ %199, %197 ]
  %207 = bitcast i8* %206 to %struct._jobject**
  %208 = load %struct._jobject*, %struct._jobject** %207, align 8
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %210 = bitcast %union.jvalue* %209 to %struct._jobject**
  store %struct._jobject* %208, %struct._jobject** %210, align 8, !tbaa !23
  br label %211

211:                                              ; preds = %203, %183, %164, %146, %127, %107, %87, %67, %47, %29
  %212 = phi i32 [ %30, %29 ], [ %204, %203 ], [ %30, %183 ], [ %30, %164 ], [ %147, %146 ], [ %128, %127 ], [ %108, %107 ], [ %88, %87 ], [ %68, %67 ], [ %48, %47 ]
  %213 = phi i32 [ %31, %29 ], [ %31, %203 ], [ %184, %183 ], [ %165, %164 ], [ %31, %146 ], [ %31, %127 ], [ %31, %107 ], [ %31, %87 ], [ %31, %67 ], [ %31, %47 ]
  %214 = phi i8* [ %32, %29 ], [ %205, %203 ], [ %185, %183 ], [ %166, %164 ], [ %148, %146 ], [ %129, %127 ], [ %109, %107 ], [ %89, %87 ], [ %69, %67 ], [ %49, %47 ]
  %215 = add nuw nsw i64 %33, 1
  %216 = icmp eq i64 %215, %28
  br i1 %216, label %217, label %29, !llvm.loop !57

217:                                              ; preds = %211, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %19) #4
  %218 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %219 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %218, i64 0, i32 81
  %220 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %219, align 8, !tbaa !58
  %221 = call i32 %220(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %25) #4
  call void @llvm.stackrestore(i8* %18)
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %7) #4
  ret i32 %221
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i32 @JNI_CallNonvirtualIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !11
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !21
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #4
  %12 = sext i32 %11 to i64
  %13 = alloca %union.jvalue, i64 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %204

15:                                               ; preds = %5
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 3
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 1
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 0
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 4
  %20 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 2
  %21 = zext i32 %11 to i64
  br label %22

22:                                               ; preds = %15, %201
  %23 = phi i64 [ 0, %15 ], [ %202, %201 ]
  %24 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 %23
  %25 = load i8, i8* %24, align 1, !tbaa !23
  switch i8 %25, label %201 [
    i8 90, label %26
    i8 66, label %46
    i8 83, label %66
    i8 67, label %86
    i8 73, label %106
    i8 74, label %125
    i8 68, label %143
    i8 70, label %162
    i8 76, label %182
  ]

26:                                               ; preds = %22
  %27 = load i32, i32* %16, align 8
  %28 = icmp sgt i32 %27, -1
  br i1 %28, label %36, label %29

29:                                               ; preds = %26
  %30 = add nsw i32 %27, 8
  store i32 %30, i32* %16, align 8
  %31 = icmp ult i32 %27, -7
  br i1 %31, label %32, label %36

32:                                               ; preds = %29
  %33 = load i8*, i8** %17, align 8
  %34 = sext i32 %27 to i64
  %35 = getelementptr inbounds i8, i8* %33, i64 %34
  br label %39

36:                                               ; preds = %29, %26
  %37 = load i8*, i8** %18, align 8
  %38 = getelementptr inbounds i8, i8* %37, i64 8
  store i8* %38, i8** %18, align 8
  br label %39

39:                                               ; preds = %36, %32
  %40 = phi i8* [ %35, %32 ], [ %37, %36 ]
  %41 = bitcast i8* %40 to i32*
  %42 = load i32, i32* %41, align 8
  %43 = trunc i32 %42 to i8
  %44 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %43, i8* %45, align 8, !tbaa !23
  br label %201

46:                                               ; preds = %22
  %47 = load i32, i32* %16, align 8
  %48 = icmp sgt i32 %47, -1
  br i1 %48, label %56, label %49

49:                                               ; preds = %46
  %50 = add nsw i32 %47, 8
  store i32 %50, i32* %16, align 8
  %51 = icmp ult i32 %47, -7
  br i1 %51, label %52, label %56

52:                                               ; preds = %49
  %53 = load i8*, i8** %17, align 8
  %54 = sext i32 %47 to i64
  %55 = getelementptr inbounds i8, i8* %53, i64 %54
  br label %59

56:                                               ; preds = %49, %46
  %57 = load i8*, i8** %18, align 8
  %58 = getelementptr inbounds i8, i8* %57, i64 8
  store i8* %58, i8** %18, align 8
  br label %59

59:                                               ; preds = %56, %52
  %60 = phi i8* [ %55, %52 ], [ %57, %56 ]
  %61 = bitcast i8* %60 to i32*
  %62 = load i32, i32* %61, align 8
  %63 = trunc i32 %62 to i8
  %64 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %63, i8* %65, align 8, !tbaa !23
  br label %201

66:                                               ; preds = %22
  %67 = load i32, i32* %16, align 8
  %68 = icmp sgt i32 %67, -1
  br i1 %68, label %76, label %69

69:                                               ; preds = %66
  %70 = add nsw i32 %67, 8
  store i32 %70, i32* %16, align 8
  %71 = icmp ult i32 %67, -7
  br i1 %71, label %72, label %76

72:                                               ; preds = %69
  %73 = load i8*, i8** %17, align 8
  %74 = sext i32 %67 to i64
  %75 = getelementptr inbounds i8, i8* %73, i64 %74
  br label %79

76:                                               ; preds = %69, %66
  %77 = load i8*, i8** %18, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %18, align 8
  br label %79

79:                                               ; preds = %76, %72
  %80 = phi i8* [ %75, %72 ], [ %77, %76 ]
  %81 = bitcast i8* %80 to i32*
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i16
  %84 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %85 = bitcast %union.jvalue* %84 to i16*
  store i16 %83, i16* %85, align 8, !tbaa !23
  br label %201

86:                                               ; preds = %22
  %87 = load i32, i32* %16, align 8
  %88 = icmp sgt i32 %87, -1
  br i1 %88, label %96, label %89

89:                                               ; preds = %86
  %90 = add nsw i32 %87, 8
  store i32 %90, i32* %16, align 8
  %91 = icmp ult i32 %87, -7
  br i1 %91, label %92, label %96

92:                                               ; preds = %89
  %93 = load i8*, i8** %17, align 8
  %94 = sext i32 %87 to i64
  %95 = getelementptr inbounds i8, i8* %93, i64 %94
  br label %99

96:                                               ; preds = %89, %86
  %97 = load i8*, i8** %18, align 8
  %98 = getelementptr inbounds i8, i8* %97, i64 8
  store i8* %98, i8** %18, align 8
  br label %99

99:                                               ; preds = %96, %92
  %100 = phi i8* [ %95, %92 ], [ %97, %96 ]
  %101 = bitcast i8* %100 to i32*
  %102 = load i32, i32* %101, align 8
  %103 = and i32 %102, 65535
  %104 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %103, i32* %105, align 8, !tbaa !23
  br label %201

106:                                              ; preds = %22
  %107 = load i32, i32* %16, align 8
  %108 = icmp sgt i32 %107, -1
  br i1 %108, label %116, label %109

109:                                              ; preds = %106
  %110 = add nsw i32 %107, 8
  store i32 %110, i32* %16, align 8
  %111 = icmp ult i32 %107, -7
  br i1 %111, label %112, label %116

112:                                              ; preds = %109
  %113 = load i8*, i8** %17, align 8
  %114 = sext i32 %107 to i64
  %115 = getelementptr inbounds i8, i8* %113, i64 %114
  br label %119

116:                                              ; preds = %109, %106
  %117 = load i8*, i8** %18, align 8
  %118 = getelementptr inbounds i8, i8* %117, i64 8
  store i8* %118, i8** %18, align 8
  br label %119

119:                                              ; preds = %116, %112
  %120 = phi i8* [ %115, %112 ], [ %117, %116 ]
  %121 = bitcast i8* %120 to i32*
  %122 = load i32, i32* %121, align 8
  %123 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %124 = bitcast %union.jvalue* %123 to i32*
  store i32 %122, i32* %124, align 8, !tbaa !23
  br label %201

125:                                              ; preds = %22
  %126 = load i32, i32* %16, align 8
  %127 = icmp sgt i32 %126, -1
  br i1 %127, label %135, label %128

128:                                              ; preds = %125
  %129 = add nsw i32 %126, 8
  store i32 %129, i32* %16, align 8
  %130 = icmp ult i32 %126, -7
  br i1 %130, label %131, label %135

131:                                              ; preds = %128
  %132 = load i8*, i8** %17, align 8
  %133 = sext i32 %126 to i64
  %134 = getelementptr inbounds i8, i8* %132, i64 %133
  br label %138

135:                                              ; preds = %128, %125
  %136 = load i8*, i8** %18, align 8
  %137 = getelementptr inbounds i8, i8* %136, i64 8
  store i8* %137, i8** %18, align 8
  br label %138

138:                                              ; preds = %135, %131
  %139 = phi i8* [ %134, %131 ], [ %136, %135 ]
  %140 = bitcast i8* %139 to i64*
  %141 = load i64, i64* %140, align 8
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23, i32 0
  store i64 %141, i64* %142, align 8, !tbaa !23
  br label %201

143:                                              ; preds = %22
  %144 = load i32, i32* %19, align 4
  %145 = icmp sgt i32 %144, -1
  br i1 %145, label %153, label %146

146:                                              ; preds = %143
  %147 = add nsw i32 %144, 16
  store i32 %147, i32* %19, align 4
  %148 = icmp ult i32 %144, -15
  br i1 %148, label %149, label %153

149:                                              ; preds = %146
  %150 = load i8*, i8** %20, align 8
  %151 = sext i32 %144 to i64
  %152 = getelementptr inbounds i8, i8* %150, i64 %151
  br label %156

153:                                              ; preds = %146, %143
  %154 = load i8*, i8** %18, align 8
  %155 = getelementptr inbounds i8, i8* %154, i64 8
  store i8* %155, i8** %18, align 8
  br label %156

156:                                              ; preds = %153, %149
  %157 = phi i8* [ %152, %149 ], [ %154, %153 ]
  %158 = bitcast i8* %157 to double*
  %159 = load double, double* %158, align 8
  %160 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %161 = bitcast %union.jvalue* %160 to double*
  store double %159, double* %161, align 8, !tbaa !23
  br label %201

162:                                              ; preds = %22
  %163 = load i32, i32* %19, align 4
  %164 = icmp sgt i32 %163, -1
  br i1 %164, label %172, label %165

165:                                              ; preds = %162
  %166 = add nsw i32 %163, 16
  store i32 %166, i32* %19, align 4
  %167 = icmp ult i32 %163, -15
  br i1 %167, label %168, label %172

168:                                              ; preds = %165
  %169 = load i8*, i8** %20, align 8
  %170 = sext i32 %163 to i64
  %171 = getelementptr inbounds i8, i8* %169, i64 %170
  br label %175

172:                                              ; preds = %165, %162
  %173 = load i8*, i8** %18, align 8
  %174 = getelementptr inbounds i8, i8* %173, i64 8
  store i8* %174, i8** %18, align 8
  br label %175

175:                                              ; preds = %172, %168
  %176 = phi i8* [ %171, %168 ], [ %173, %172 ]
  %177 = bitcast i8* %176 to double*
  %178 = load double, double* %177, align 8
  %179 = fptrunc double %178 to float
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %181 = bitcast %union.jvalue* %180 to float*
  store float %179, float* %181, align 8, !tbaa !23
  br label %201

182:                                              ; preds = %22
  %183 = load i32, i32* %16, align 8
  %184 = icmp sgt i32 %183, -1
  br i1 %184, label %192, label %185

185:                                              ; preds = %182
  %186 = add nsw i32 %183, 8
  store i32 %186, i32* %16, align 8
  %187 = icmp ult i32 %183, -7
  br i1 %187, label %188, label %192

188:                                              ; preds = %185
  %189 = load i8*, i8** %17, align 8
  %190 = sext i32 %183 to i64
  %191 = getelementptr inbounds i8, i8* %189, i64 %190
  br label %195

192:                                              ; preds = %185, %182
  %193 = load i8*, i8** %18, align 8
  %194 = getelementptr inbounds i8, i8* %193, i64 8
  store i8* %194, i8** %18, align 8
  br label %195

195:                                              ; preds = %192, %188
  %196 = phi i8* [ %191, %188 ], [ %193, %192 ]
  %197 = bitcast i8* %196 to %struct._jobject**
  %198 = load %struct._jobject*, %struct._jobject** %197, align 8
  %199 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %200 = bitcast %union.jvalue* %199 to %struct._jobject**
  store %struct._jobject* %198, %struct._jobject** %200, align 8, !tbaa !23
  br label %201

201:                                              ; preds = %39, %59, %79, %99, %119, %138, %156, %175, %195, %22
  %202 = add nuw nsw i64 %23, 1
  %203 = icmp eq i64 %202, %21
  br i1 %203, label %204, label %22, !llvm.loop !57

204:                                              ; preds = %201, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %205 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %206 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %205, i64 0, i32 81
  %207 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %206, align 8, !tbaa !58
  %208 = call i32 %207(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret i32 %208
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i32 @JNI_CallStaticIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !59

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 131
  %219 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !60
  %220 = call i32 %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i32 %220
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i32 @JNI_CallStaticIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !59

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 131
  %206 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !60
  %207 = call i32 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i32 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i64 @JNI_CallLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !61

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 54
  %219 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !62
  %220 = call i64 %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i64 %220
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i64 @JNI_CallLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !61

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 54
  %206 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !62
  %207 = call i64 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i64 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i64 @JNI_CallNonvirtualLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca %"struct.std::__va_list", align 8
  %7 = bitcast %"struct.std::__va_list"* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 0
  %9 = load i8*, i8** %8, align 8, !tbaa.struct !10
  %10 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 1
  %11 = load i8*, i8** %10, align 8, !tbaa.struct !17
  %12 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 2
  %13 = load i8*, i8** %12, align 8, !tbaa.struct !18
  %14 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 3
  %15 = load i32, i32* %14, align 8, !tbaa.struct !19
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 4
  %17 = load i32, i32* %16, align 4, !tbaa.struct !20
  %18 = call i8* @llvm.stacksave()
  %19 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %19) #4
  %20 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %20, align 8, !tbaa !11
  %22 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %21, align 8, !tbaa !21
  %23 = call i32 %22(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %19) #4
  %24 = sext i32 %23 to i64
  %25 = alloca %union.jvalue, i64 %24, align 16
  %26 = icmp sgt i32 %23, 0
  br i1 %26, label %27, label %217

27:                                               ; preds = %4
  %28 = zext i32 %23 to i64
  br label %29

29:                                               ; preds = %211, %27
  %30 = phi i32 [ %15, %27 ], [ %212, %211 ]
  %31 = phi i32 [ %17, %27 ], [ %213, %211 ]
  %32 = phi i8* [ %9, %27 ], [ %214, %211 ]
  %33 = phi i64 [ 0, %27 ], [ %215, %211 ]
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1, !tbaa !23
  switch i8 %35, label %211 [
    i8 90, label %36
    i8 66, label %56
    i8 83, label %76
    i8 67, label %96
    i8 73, label %116
    i8 74, label %135
    i8 68, label %153
    i8 70, label %172
    i8 76, label %192
  ]

36:                                               ; preds = %29
  %37 = icmp sgt i32 %30, -1
  br i1 %37, label %44, label %38

38:                                               ; preds = %36
  %39 = add nsw i32 %30, 8
  %40 = icmp ult i32 %30, -7
  br i1 %40, label %41, label %44

41:                                               ; preds = %38
  %42 = sext i32 %30 to i64
  %43 = getelementptr inbounds i8, i8* %11, i64 %42
  br label %47

44:                                               ; preds = %38, %36
  %45 = phi i32 [ %30, %36 ], [ %39, %38 ]
  %46 = getelementptr inbounds i8, i8* %32, i64 8
  br label %47

47:                                               ; preds = %44, %41
  %48 = phi i32 [ %45, %44 ], [ %39, %41 ]
  %49 = phi i8* [ %46, %44 ], [ %32, %41 ]
  %50 = phi i8* [ %32, %44 ], [ %43, %41 ]
  %51 = bitcast i8* %50 to i32*
  %52 = load i32, i32* %51, align 8
  %53 = trunc i32 %52 to i8
  %54 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %55 = bitcast %union.jvalue* %54 to i8*
  store i8 %53, i8* %55, align 8, !tbaa !23
  br label %211

56:                                               ; preds = %29
  %57 = icmp sgt i32 %30, -1
  br i1 %57, label %64, label %58

58:                                               ; preds = %56
  %59 = add nsw i32 %30, 8
  %60 = icmp ult i32 %30, -7
  br i1 %60, label %61, label %64

61:                                               ; preds = %58
  %62 = sext i32 %30 to i64
  %63 = getelementptr inbounds i8, i8* %11, i64 %62
  br label %67

64:                                               ; preds = %58, %56
  %65 = phi i32 [ %30, %56 ], [ %59, %58 ]
  %66 = getelementptr inbounds i8, i8* %32, i64 8
  br label %67

67:                                               ; preds = %64, %61
  %68 = phi i32 [ %65, %64 ], [ %59, %61 ]
  %69 = phi i8* [ %66, %64 ], [ %32, %61 ]
  %70 = phi i8* [ %32, %64 ], [ %63, %61 ]
  %71 = bitcast i8* %70 to i32*
  %72 = load i32, i32* %71, align 8
  %73 = trunc i32 %72 to i8
  %74 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %75 = bitcast %union.jvalue* %74 to i8*
  store i8 %73, i8* %75, align 8, !tbaa !23
  br label %211

76:                                               ; preds = %29
  %77 = icmp sgt i32 %30, -1
  br i1 %77, label %84, label %78

78:                                               ; preds = %76
  %79 = add nsw i32 %30, 8
  %80 = icmp ult i32 %30, -7
  br i1 %80, label %81, label %84

81:                                               ; preds = %78
  %82 = sext i32 %30 to i64
  %83 = getelementptr inbounds i8, i8* %11, i64 %82
  br label %87

84:                                               ; preds = %78, %76
  %85 = phi i32 [ %30, %76 ], [ %79, %78 ]
  %86 = getelementptr inbounds i8, i8* %32, i64 8
  br label %87

87:                                               ; preds = %84, %81
  %88 = phi i32 [ %85, %84 ], [ %79, %81 ]
  %89 = phi i8* [ %86, %84 ], [ %32, %81 ]
  %90 = phi i8* [ %32, %84 ], [ %83, %81 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 8
  %93 = trunc i32 %92 to i16
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %93, i16* %95, align 8, !tbaa !23
  br label %211

96:                                               ; preds = %29
  %97 = icmp sgt i32 %30, -1
  br i1 %97, label %104, label %98

98:                                               ; preds = %96
  %99 = add nsw i32 %30, 8
  %100 = icmp ult i32 %30, -7
  br i1 %100, label %101, label %104

101:                                              ; preds = %98
  %102 = sext i32 %30 to i64
  %103 = getelementptr inbounds i8, i8* %11, i64 %102
  br label %107

104:                                              ; preds = %98, %96
  %105 = phi i32 [ %30, %96 ], [ %99, %98 ]
  %106 = getelementptr inbounds i8, i8* %32, i64 8
  br label %107

107:                                              ; preds = %104, %101
  %108 = phi i32 [ %105, %104 ], [ %99, %101 ]
  %109 = phi i8* [ %106, %104 ], [ %32, %101 ]
  %110 = phi i8* [ %32, %104 ], [ %103, %101 ]
  %111 = bitcast i8* %110 to i32*
  %112 = load i32, i32* %111, align 8
  %113 = and i32 %112, 65535
  %114 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %115 = bitcast %union.jvalue* %114 to i32*
  store i32 %113, i32* %115, align 8, !tbaa !23
  br label %211

116:                                              ; preds = %29
  %117 = icmp sgt i32 %30, -1
  br i1 %117, label %124, label %118

118:                                              ; preds = %116
  %119 = add nsw i32 %30, 8
  %120 = icmp ult i32 %30, -7
  br i1 %120, label %121, label %124

121:                                              ; preds = %118
  %122 = sext i32 %30 to i64
  %123 = getelementptr inbounds i8, i8* %11, i64 %122
  br label %127

124:                                              ; preds = %118, %116
  %125 = phi i32 [ %30, %116 ], [ %119, %118 ]
  %126 = getelementptr inbounds i8, i8* %32, i64 8
  br label %127

127:                                              ; preds = %124, %121
  %128 = phi i32 [ %125, %124 ], [ %119, %121 ]
  %129 = phi i8* [ %126, %124 ], [ %32, %121 ]
  %130 = phi i8* [ %32, %124 ], [ %123, %121 ]
  %131 = bitcast i8* %130 to i32*
  %132 = load i32, i32* %131, align 8
  %133 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %134 = bitcast %union.jvalue* %133 to i32*
  store i32 %132, i32* %134, align 8, !tbaa !23
  br label %211

135:                                              ; preds = %29
  %136 = icmp sgt i32 %30, -1
  br i1 %136, label %143, label %137

137:                                              ; preds = %135
  %138 = add nsw i32 %30, 8
  %139 = icmp ult i32 %30, -7
  br i1 %139, label %140, label %143

140:                                              ; preds = %137
  %141 = sext i32 %30 to i64
  %142 = getelementptr inbounds i8, i8* %11, i64 %141
  br label %146

143:                                              ; preds = %137, %135
  %144 = phi i32 [ %30, %135 ], [ %138, %137 ]
  %145 = getelementptr inbounds i8, i8* %32, i64 8
  br label %146

146:                                              ; preds = %143, %140
  %147 = phi i32 [ %144, %143 ], [ %138, %140 ]
  %148 = phi i8* [ %145, %143 ], [ %32, %140 ]
  %149 = phi i8* [ %32, %143 ], [ %142, %140 ]
  %150 = bitcast i8* %149 to i64*
  %151 = load i64, i64* %150, align 8
  %152 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33, i32 0
  store i64 %151, i64* %152, align 8, !tbaa !23
  br label %211

153:                                              ; preds = %29
  %154 = icmp sgt i32 %31, -1
  br i1 %154, label %161, label %155

155:                                              ; preds = %153
  %156 = add nsw i32 %31, 16
  %157 = icmp ult i32 %31, -15
  br i1 %157, label %158, label %161

158:                                              ; preds = %155
  %159 = sext i32 %31 to i64
  %160 = getelementptr inbounds i8, i8* %13, i64 %159
  br label %164

161:                                              ; preds = %155, %153
  %162 = phi i32 [ %31, %153 ], [ %156, %155 ]
  %163 = getelementptr inbounds i8, i8* %32, i64 8
  br label %164

164:                                              ; preds = %161, %158
  %165 = phi i32 [ %162, %161 ], [ %156, %158 ]
  %166 = phi i8* [ %163, %161 ], [ %32, %158 ]
  %167 = phi i8* [ %32, %161 ], [ %160, %158 ]
  %168 = bitcast i8* %167 to double*
  %169 = load double, double* %168, align 8
  %170 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %171 = bitcast %union.jvalue* %170 to double*
  store double %169, double* %171, align 8, !tbaa !23
  br label %211

172:                                              ; preds = %29
  %173 = icmp sgt i32 %31, -1
  br i1 %173, label %180, label %174

174:                                              ; preds = %172
  %175 = add nsw i32 %31, 16
  %176 = icmp ult i32 %31, -15
  br i1 %176, label %177, label %180

177:                                              ; preds = %174
  %178 = sext i32 %31 to i64
  %179 = getelementptr inbounds i8, i8* %13, i64 %178
  br label %183

180:                                              ; preds = %174, %172
  %181 = phi i32 [ %31, %172 ], [ %175, %174 ]
  %182 = getelementptr inbounds i8, i8* %32, i64 8
  br label %183

183:                                              ; preds = %180, %177
  %184 = phi i32 [ %181, %180 ], [ %175, %177 ]
  %185 = phi i8* [ %182, %180 ], [ %32, %177 ]
  %186 = phi i8* [ %32, %180 ], [ %179, %177 ]
  %187 = bitcast i8* %186 to double*
  %188 = load double, double* %187, align 8
  %189 = fptrunc double %188 to float
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %191 = bitcast %union.jvalue* %190 to float*
  store float %189, float* %191, align 8, !tbaa !23
  br label %211

192:                                              ; preds = %29
  %193 = icmp sgt i32 %30, -1
  br i1 %193, label %200, label %194

194:                                              ; preds = %192
  %195 = add nsw i32 %30, 8
  %196 = icmp ult i32 %30, -7
  br i1 %196, label %197, label %200

197:                                              ; preds = %194
  %198 = sext i32 %30 to i64
  %199 = getelementptr inbounds i8, i8* %11, i64 %198
  br label %203

200:                                              ; preds = %194, %192
  %201 = phi i32 [ %30, %192 ], [ %195, %194 ]
  %202 = getelementptr inbounds i8, i8* %32, i64 8
  br label %203

203:                                              ; preds = %200, %197
  %204 = phi i32 [ %201, %200 ], [ %195, %197 ]
  %205 = phi i8* [ %202, %200 ], [ %32, %197 ]
  %206 = phi i8* [ %32, %200 ], [ %199, %197 ]
  %207 = bitcast i8* %206 to %struct._jobject**
  %208 = load %struct._jobject*, %struct._jobject** %207, align 8
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %210 = bitcast %union.jvalue* %209 to %struct._jobject**
  store %struct._jobject* %208, %struct._jobject** %210, align 8, !tbaa !23
  br label %211

211:                                              ; preds = %203, %183, %164, %146, %127, %107, %87, %67, %47, %29
  %212 = phi i32 [ %30, %29 ], [ %204, %203 ], [ %30, %183 ], [ %30, %164 ], [ %147, %146 ], [ %128, %127 ], [ %108, %107 ], [ %88, %87 ], [ %68, %67 ], [ %48, %47 ]
  %213 = phi i32 [ %31, %29 ], [ %31, %203 ], [ %184, %183 ], [ %165, %164 ], [ %31, %146 ], [ %31, %127 ], [ %31, %107 ], [ %31, %87 ], [ %31, %67 ], [ %31, %47 ]
  %214 = phi i8* [ %32, %29 ], [ %205, %203 ], [ %185, %183 ], [ %166, %164 ], [ %148, %146 ], [ %129, %127 ], [ %109, %107 ], [ %89, %87 ], [ %69, %67 ], [ %49, %47 ]
  %215 = add nuw nsw i64 %33, 1
  %216 = icmp eq i64 %215, %28
  br i1 %216, label %217, label %29, !llvm.loop !63

217:                                              ; preds = %211, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %19) #4
  %218 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %219 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %218, i64 0, i32 84
  %220 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %219, align 8, !tbaa !64
  %221 = call i64 %220(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %25) #4
  call void @llvm.stackrestore(i8* %18)
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %7) #4
  ret i64 %221
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i64 @JNI_CallNonvirtualLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !11
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !21
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #4
  %12 = sext i32 %11 to i64
  %13 = alloca %union.jvalue, i64 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %204

15:                                               ; preds = %5
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 3
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 1
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 0
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 4
  %20 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 2
  %21 = zext i32 %11 to i64
  br label %22

22:                                               ; preds = %15, %201
  %23 = phi i64 [ 0, %15 ], [ %202, %201 ]
  %24 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 %23
  %25 = load i8, i8* %24, align 1, !tbaa !23
  switch i8 %25, label %201 [
    i8 90, label %26
    i8 66, label %46
    i8 83, label %66
    i8 67, label %86
    i8 73, label %106
    i8 74, label %125
    i8 68, label %143
    i8 70, label %162
    i8 76, label %182
  ]

26:                                               ; preds = %22
  %27 = load i32, i32* %16, align 8
  %28 = icmp sgt i32 %27, -1
  br i1 %28, label %36, label %29

29:                                               ; preds = %26
  %30 = add nsw i32 %27, 8
  store i32 %30, i32* %16, align 8
  %31 = icmp ult i32 %27, -7
  br i1 %31, label %32, label %36

32:                                               ; preds = %29
  %33 = load i8*, i8** %17, align 8
  %34 = sext i32 %27 to i64
  %35 = getelementptr inbounds i8, i8* %33, i64 %34
  br label %39

36:                                               ; preds = %29, %26
  %37 = load i8*, i8** %18, align 8
  %38 = getelementptr inbounds i8, i8* %37, i64 8
  store i8* %38, i8** %18, align 8
  br label %39

39:                                               ; preds = %36, %32
  %40 = phi i8* [ %35, %32 ], [ %37, %36 ]
  %41 = bitcast i8* %40 to i32*
  %42 = load i32, i32* %41, align 8
  %43 = trunc i32 %42 to i8
  %44 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %43, i8* %45, align 8, !tbaa !23
  br label %201

46:                                               ; preds = %22
  %47 = load i32, i32* %16, align 8
  %48 = icmp sgt i32 %47, -1
  br i1 %48, label %56, label %49

49:                                               ; preds = %46
  %50 = add nsw i32 %47, 8
  store i32 %50, i32* %16, align 8
  %51 = icmp ult i32 %47, -7
  br i1 %51, label %52, label %56

52:                                               ; preds = %49
  %53 = load i8*, i8** %17, align 8
  %54 = sext i32 %47 to i64
  %55 = getelementptr inbounds i8, i8* %53, i64 %54
  br label %59

56:                                               ; preds = %49, %46
  %57 = load i8*, i8** %18, align 8
  %58 = getelementptr inbounds i8, i8* %57, i64 8
  store i8* %58, i8** %18, align 8
  br label %59

59:                                               ; preds = %56, %52
  %60 = phi i8* [ %55, %52 ], [ %57, %56 ]
  %61 = bitcast i8* %60 to i32*
  %62 = load i32, i32* %61, align 8
  %63 = trunc i32 %62 to i8
  %64 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %63, i8* %65, align 8, !tbaa !23
  br label %201

66:                                               ; preds = %22
  %67 = load i32, i32* %16, align 8
  %68 = icmp sgt i32 %67, -1
  br i1 %68, label %76, label %69

69:                                               ; preds = %66
  %70 = add nsw i32 %67, 8
  store i32 %70, i32* %16, align 8
  %71 = icmp ult i32 %67, -7
  br i1 %71, label %72, label %76

72:                                               ; preds = %69
  %73 = load i8*, i8** %17, align 8
  %74 = sext i32 %67 to i64
  %75 = getelementptr inbounds i8, i8* %73, i64 %74
  br label %79

76:                                               ; preds = %69, %66
  %77 = load i8*, i8** %18, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %18, align 8
  br label %79

79:                                               ; preds = %76, %72
  %80 = phi i8* [ %75, %72 ], [ %77, %76 ]
  %81 = bitcast i8* %80 to i32*
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i16
  %84 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %85 = bitcast %union.jvalue* %84 to i16*
  store i16 %83, i16* %85, align 8, !tbaa !23
  br label %201

86:                                               ; preds = %22
  %87 = load i32, i32* %16, align 8
  %88 = icmp sgt i32 %87, -1
  br i1 %88, label %96, label %89

89:                                               ; preds = %86
  %90 = add nsw i32 %87, 8
  store i32 %90, i32* %16, align 8
  %91 = icmp ult i32 %87, -7
  br i1 %91, label %92, label %96

92:                                               ; preds = %89
  %93 = load i8*, i8** %17, align 8
  %94 = sext i32 %87 to i64
  %95 = getelementptr inbounds i8, i8* %93, i64 %94
  br label %99

96:                                               ; preds = %89, %86
  %97 = load i8*, i8** %18, align 8
  %98 = getelementptr inbounds i8, i8* %97, i64 8
  store i8* %98, i8** %18, align 8
  br label %99

99:                                               ; preds = %96, %92
  %100 = phi i8* [ %95, %92 ], [ %97, %96 ]
  %101 = bitcast i8* %100 to i32*
  %102 = load i32, i32* %101, align 8
  %103 = and i32 %102, 65535
  %104 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %103, i32* %105, align 8, !tbaa !23
  br label %201

106:                                              ; preds = %22
  %107 = load i32, i32* %16, align 8
  %108 = icmp sgt i32 %107, -1
  br i1 %108, label %116, label %109

109:                                              ; preds = %106
  %110 = add nsw i32 %107, 8
  store i32 %110, i32* %16, align 8
  %111 = icmp ult i32 %107, -7
  br i1 %111, label %112, label %116

112:                                              ; preds = %109
  %113 = load i8*, i8** %17, align 8
  %114 = sext i32 %107 to i64
  %115 = getelementptr inbounds i8, i8* %113, i64 %114
  br label %119

116:                                              ; preds = %109, %106
  %117 = load i8*, i8** %18, align 8
  %118 = getelementptr inbounds i8, i8* %117, i64 8
  store i8* %118, i8** %18, align 8
  br label %119

119:                                              ; preds = %116, %112
  %120 = phi i8* [ %115, %112 ], [ %117, %116 ]
  %121 = bitcast i8* %120 to i32*
  %122 = load i32, i32* %121, align 8
  %123 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %124 = bitcast %union.jvalue* %123 to i32*
  store i32 %122, i32* %124, align 8, !tbaa !23
  br label %201

125:                                              ; preds = %22
  %126 = load i32, i32* %16, align 8
  %127 = icmp sgt i32 %126, -1
  br i1 %127, label %135, label %128

128:                                              ; preds = %125
  %129 = add nsw i32 %126, 8
  store i32 %129, i32* %16, align 8
  %130 = icmp ult i32 %126, -7
  br i1 %130, label %131, label %135

131:                                              ; preds = %128
  %132 = load i8*, i8** %17, align 8
  %133 = sext i32 %126 to i64
  %134 = getelementptr inbounds i8, i8* %132, i64 %133
  br label %138

135:                                              ; preds = %128, %125
  %136 = load i8*, i8** %18, align 8
  %137 = getelementptr inbounds i8, i8* %136, i64 8
  store i8* %137, i8** %18, align 8
  br label %138

138:                                              ; preds = %135, %131
  %139 = phi i8* [ %134, %131 ], [ %136, %135 ]
  %140 = bitcast i8* %139 to i64*
  %141 = load i64, i64* %140, align 8
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23, i32 0
  store i64 %141, i64* %142, align 8, !tbaa !23
  br label %201

143:                                              ; preds = %22
  %144 = load i32, i32* %19, align 4
  %145 = icmp sgt i32 %144, -1
  br i1 %145, label %153, label %146

146:                                              ; preds = %143
  %147 = add nsw i32 %144, 16
  store i32 %147, i32* %19, align 4
  %148 = icmp ult i32 %144, -15
  br i1 %148, label %149, label %153

149:                                              ; preds = %146
  %150 = load i8*, i8** %20, align 8
  %151 = sext i32 %144 to i64
  %152 = getelementptr inbounds i8, i8* %150, i64 %151
  br label %156

153:                                              ; preds = %146, %143
  %154 = load i8*, i8** %18, align 8
  %155 = getelementptr inbounds i8, i8* %154, i64 8
  store i8* %155, i8** %18, align 8
  br label %156

156:                                              ; preds = %153, %149
  %157 = phi i8* [ %152, %149 ], [ %154, %153 ]
  %158 = bitcast i8* %157 to double*
  %159 = load double, double* %158, align 8
  %160 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %161 = bitcast %union.jvalue* %160 to double*
  store double %159, double* %161, align 8, !tbaa !23
  br label %201

162:                                              ; preds = %22
  %163 = load i32, i32* %19, align 4
  %164 = icmp sgt i32 %163, -1
  br i1 %164, label %172, label %165

165:                                              ; preds = %162
  %166 = add nsw i32 %163, 16
  store i32 %166, i32* %19, align 4
  %167 = icmp ult i32 %163, -15
  br i1 %167, label %168, label %172

168:                                              ; preds = %165
  %169 = load i8*, i8** %20, align 8
  %170 = sext i32 %163 to i64
  %171 = getelementptr inbounds i8, i8* %169, i64 %170
  br label %175

172:                                              ; preds = %165, %162
  %173 = load i8*, i8** %18, align 8
  %174 = getelementptr inbounds i8, i8* %173, i64 8
  store i8* %174, i8** %18, align 8
  br label %175

175:                                              ; preds = %172, %168
  %176 = phi i8* [ %171, %168 ], [ %173, %172 ]
  %177 = bitcast i8* %176 to double*
  %178 = load double, double* %177, align 8
  %179 = fptrunc double %178 to float
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %181 = bitcast %union.jvalue* %180 to float*
  store float %179, float* %181, align 8, !tbaa !23
  br label %201

182:                                              ; preds = %22
  %183 = load i32, i32* %16, align 8
  %184 = icmp sgt i32 %183, -1
  br i1 %184, label %192, label %185

185:                                              ; preds = %182
  %186 = add nsw i32 %183, 8
  store i32 %186, i32* %16, align 8
  %187 = icmp ult i32 %183, -7
  br i1 %187, label %188, label %192

188:                                              ; preds = %185
  %189 = load i8*, i8** %17, align 8
  %190 = sext i32 %183 to i64
  %191 = getelementptr inbounds i8, i8* %189, i64 %190
  br label %195

192:                                              ; preds = %185, %182
  %193 = load i8*, i8** %18, align 8
  %194 = getelementptr inbounds i8, i8* %193, i64 8
  store i8* %194, i8** %18, align 8
  br label %195

195:                                              ; preds = %192, %188
  %196 = phi i8* [ %191, %188 ], [ %193, %192 ]
  %197 = bitcast i8* %196 to %struct._jobject**
  %198 = load %struct._jobject*, %struct._jobject** %197, align 8
  %199 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %200 = bitcast %union.jvalue* %199 to %struct._jobject**
  store %struct._jobject* %198, %struct._jobject** %200, align 8, !tbaa !23
  br label %201

201:                                              ; preds = %39, %59, %79, %99, %119, %138, %156, %175, %195, %22
  %202 = add nuw nsw i64 %23, 1
  %203 = icmp eq i64 %202, %21
  br i1 %203, label %204, label %22, !llvm.loop !63

204:                                              ; preds = %201, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %205 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %206 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %205, i64 0, i32 84
  %207 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %206, align 8, !tbaa !64
  %208 = call i64 %207(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret i64 %208
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i64 @JNI_CallStaticLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !65

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 134
  %219 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !66
  %220 = call i64 %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i64 %220
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i64 @JNI_CallStaticLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !65

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 134
  %206 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !66
  %207 = call i64 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i64 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local float @JNI_CallFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !67

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 57
  %219 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !68
  %220 = call float %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret float %220
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local float @JNI_CallFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !67

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 57
  %206 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !68
  %207 = call float %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret float %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local float @JNI_CallNonvirtualFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca %"struct.std::__va_list", align 8
  %7 = bitcast %"struct.std::__va_list"* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 0
  %9 = load i8*, i8** %8, align 8, !tbaa.struct !10
  %10 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 1
  %11 = load i8*, i8** %10, align 8, !tbaa.struct !17
  %12 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 2
  %13 = load i8*, i8** %12, align 8, !tbaa.struct !18
  %14 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 3
  %15 = load i32, i32* %14, align 8, !tbaa.struct !19
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 4
  %17 = load i32, i32* %16, align 4, !tbaa.struct !20
  %18 = call i8* @llvm.stacksave()
  %19 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %19) #4
  %20 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %20, align 8, !tbaa !11
  %22 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %21, align 8, !tbaa !21
  %23 = call i32 %22(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %19) #4
  %24 = sext i32 %23 to i64
  %25 = alloca %union.jvalue, i64 %24, align 16
  %26 = icmp sgt i32 %23, 0
  br i1 %26, label %27, label %217

27:                                               ; preds = %4
  %28 = zext i32 %23 to i64
  br label %29

29:                                               ; preds = %211, %27
  %30 = phi i32 [ %15, %27 ], [ %212, %211 ]
  %31 = phi i32 [ %17, %27 ], [ %213, %211 ]
  %32 = phi i8* [ %9, %27 ], [ %214, %211 ]
  %33 = phi i64 [ 0, %27 ], [ %215, %211 ]
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1, !tbaa !23
  switch i8 %35, label %211 [
    i8 90, label %36
    i8 66, label %56
    i8 83, label %76
    i8 67, label %96
    i8 73, label %116
    i8 74, label %135
    i8 68, label %153
    i8 70, label %172
    i8 76, label %192
  ]

36:                                               ; preds = %29
  %37 = icmp sgt i32 %30, -1
  br i1 %37, label %44, label %38

38:                                               ; preds = %36
  %39 = add nsw i32 %30, 8
  %40 = icmp ult i32 %30, -7
  br i1 %40, label %41, label %44

41:                                               ; preds = %38
  %42 = sext i32 %30 to i64
  %43 = getelementptr inbounds i8, i8* %11, i64 %42
  br label %47

44:                                               ; preds = %38, %36
  %45 = phi i32 [ %30, %36 ], [ %39, %38 ]
  %46 = getelementptr inbounds i8, i8* %32, i64 8
  br label %47

47:                                               ; preds = %44, %41
  %48 = phi i32 [ %45, %44 ], [ %39, %41 ]
  %49 = phi i8* [ %46, %44 ], [ %32, %41 ]
  %50 = phi i8* [ %32, %44 ], [ %43, %41 ]
  %51 = bitcast i8* %50 to i32*
  %52 = load i32, i32* %51, align 8
  %53 = trunc i32 %52 to i8
  %54 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %55 = bitcast %union.jvalue* %54 to i8*
  store i8 %53, i8* %55, align 8, !tbaa !23
  br label %211

56:                                               ; preds = %29
  %57 = icmp sgt i32 %30, -1
  br i1 %57, label %64, label %58

58:                                               ; preds = %56
  %59 = add nsw i32 %30, 8
  %60 = icmp ult i32 %30, -7
  br i1 %60, label %61, label %64

61:                                               ; preds = %58
  %62 = sext i32 %30 to i64
  %63 = getelementptr inbounds i8, i8* %11, i64 %62
  br label %67

64:                                               ; preds = %58, %56
  %65 = phi i32 [ %30, %56 ], [ %59, %58 ]
  %66 = getelementptr inbounds i8, i8* %32, i64 8
  br label %67

67:                                               ; preds = %64, %61
  %68 = phi i32 [ %65, %64 ], [ %59, %61 ]
  %69 = phi i8* [ %66, %64 ], [ %32, %61 ]
  %70 = phi i8* [ %32, %64 ], [ %63, %61 ]
  %71 = bitcast i8* %70 to i32*
  %72 = load i32, i32* %71, align 8
  %73 = trunc i32 %72 to i8
  %74 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %75 = bitcast %union.jvalue* %74 to i8*
  store i8 %73, i8* %75, align 8, !tbaa !23
  br label %211

76:                                               ; preds = %29
  %77 = icmp sgt i32 %30, -1
  br i1 %77, label %84, label %78

78:                                               ; preds = %76
  %79 = add nsw i32 %30, 8
  %80 = icmp ult i32 %30, -7
  br i1 %80, label %81, label %84

81:                                               ; preds = %78
  %82 = sext i32 %30 to i64
  %83 = getelementptr inbounds i8, i8* %11, i64 %82
  br label %87

84:                                               ; preds = %78, %76
  %85 = phi i32 [ %30, %76 ], [ %79, %78 ]
  %86 = getelementptr inbounds i8, i8* %32, i64 8
  br label %87

87:                                               ; preds = %84, %81
  %88 = phi i32 [ %85, %84 ], [ %79, %81 ]
  %89 = phi i8* [ %86, %84 ], [ %32, %81 ]
  %90 = phi i8* [ %32, %84 ], [ %83, %81 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 8
  %93 = trunc i32 %92 to i16
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %93, i16* %95, align 8, !tbaa !23
  br label %211

96:                                               ; preds = %29
  %97 = icmp sgt i32 %30, -1
  br i1 %97, label %104, label %98

98:                                               ; preds = %96
  %99 = add nsw i32 %30, 8
  %100 = icmp ult i32 %30, -7
  br i1 %100, label %101, label %104

101:                                              ; preds = %98
  %102 = sext i32 %30 to i64
  %103 = getelementptr inbounds i8, i8* %11, i64 %102
  br label %107

104:                                              ; preds = %98, %96
  %105 = phi i32 [ %30, %96 ], [ %99, %98 ]
  %106 = getelementptr inbounds i8, i8* %32, i64 8
  br label %107

107:                                              ; preds = %104, %101
  %108 = phi i32 [ %105, %104 ], [ %99, %101 ]
  %109 = phi i8* [ %106, %104 ], [ %32, %101 ]
  %110 = phi i8* [ %32, %104 ], [ %103, %101 ]
  %111 = bitcast i8* %110 to i32*
  %112 = load i32, i32* %111, align 8
  %113 = and i32 %112, 65535
  %114 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %115 = bitcast %union.jvalue* %114 to i32*
  store i32 %113, i32* %115, align 8, !tbaa !23
  br label %211

116:                                              ; preds = %29
  %117 = icmp sgt i32 %30, -1
  br i1 %117, label %124, label %118

118:                                              ; preds = %116
  %119 = add nsw i32 %30, 8
  %120 = icmp ult i32 %30, -7
  br i1 %120, label %121, label %124

121:                                              ; preds = %118
  %122 = sext i32 %30 to i64
  %123 = getelementptr inbounds i8, i8* %11, i64 %122
  br label %127

124:                                              ; preds = %118, %116
  %125 = phi i32 [ %30, %116 ], [ %119, %118 ]
  %126 = getelementptr inbounds i8, i8* %32, i64 8
  br label %127

127:                                              ; preds = %124, %121
  %128 = phi i32 [ %125, %124 ], [ %119, %121 ]
  %129 = phi i8* [ %126, %124 ], [ %32, %121 ]
  %130 = phi i8* [ %32, %124 ], [ %123, %121 ]
  %131 = bitcast i8* %130 to i32*
  %132 = load i32, i32* %131, align 8
  %133 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %134 = bitcast %union.jvalue* %133 to i32*
  store i32 %132, i32* %134, align 8, !tbaa !23
  br label %211

135:                                              ; preds = %29
  %136 = icmp sgt i32 %30, -1
  br i1 %136, label %143, label %137

137:                                              ; preds = %135
  %138 = add nsw i32 %30, 8
  %139 = icmp ult i32 %30, -7
  br i1 %139, label %140, label %143

140:                                              ; preds = %137
  %141 = sext i32 %30 to i64
  %142 = getelementptr inbounds i8, i8* %11, i64 %141
  br label %146

143:                                              ; preds = %137, %135
  %144 = phi i32 [ %30, %135 ], [ %138, %137 ]
  %145 = getelementptr inbounds i8, i8* %32, i64 8
  br label %146

146:                                              ; preds = %143, %140
  %147 = phi i32 [ %144, %143 ], [ %138, %140 ]
  %148 = phi i8* [ %145, %143 ], [ %32, %140 ]
  %149 = phi i8* [ %32, %143 ], [ %142, %140 ]
  %150 = bitcast i8* %149 to i64*
  %151 = load i64, i64* %150, align 8
  %152 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33, i32 0
  store i64 %151, i64* %152, align 8, !tbaa !23
  br label %211

153:                                              ; preds = %29
  %154 = icmp sgt i32 %31, -1
  br i1 %154, label %161, label %155

155:                                              ; preds = %153
  %156 = add nsw i32 %31, 16
  %157 = icmp ult i32 %31, -15
  br i1 %157, label %158, label %161

158:                                              ; preds = %155
  %159 = sext i32 %31 to i64
  %160 = getelementptr inbounds i8, i8* %13, i64 %159
  br label %164

161:                                              ; preds = %155, %153
  %162 = phi i32 [ %31, %153 ], [ %156, %155 ]
  %163 = getelementptr inbounds i8, i8* %32, i64 8
  br label %164

164:                                              ; preds = %161, %158
  %165 = phi i32 [ %162, %161 ], [ %156, %158 ]
  %166 = phi i8* [ %163, %161 ], [ %32, %158 ]
  %167 = phi i8* [ %32, %161 ], [ %160, %158 ]
  %168 = bitcast i8* %167 to double*
  %169 = load double, double* %168, align 8
  %170 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %171 = bitcast %union.jvalue* %170 to double*
  store double %169, double* %171, align 8, !tbaa !23
  br label %211

172:                                              ; preds = %29
  %173 = icmp sgt i32 %31, -1
  br i1 %173, label %180, label %174

174:                                              ; preds = %172
  %175 = add nsw i32 %31, 16
  %176 = icmp ult i32 %31, -15
  br i1 %176, label %177, label %180

177:                                              ; preds = %174
  %178 = sext i32 %31 to i64
  %179 = getelementptr inbounds i8, i8* %13, i64 %178
  br label %183

180:                                              ; preds = %174, %172
  %181 = phi i32 [ %31, %172 ], [ %175, %174 ]
  %182 = getelementptr inbounds i8, i8* %32, i64 8
  br label %183

183:                                              ; preds = %180, %177
  %184 = phi i32 [ %181, %180 ], [ %175, %177 ]
  %185 = phi i8* [ %182, %180 ], [ %32, %177 ]
  %186 = phi i8* [ %32, %180 ], [ %179, %177 ]
  %187 = bitcast i8* %186 to double*
  %188 = load double, double* %187, align 8
  %189 = fptrunc double %188 to float
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %191 = bitcast %union.jvalue* %190 to float*
  store float %189, float* %191, align 8, !tbaa !23
  br label %211

192:                                              ; preds = %29
  %193 = icmp sgt i32 %30, -1
  br i1 %193, label %200, label %194

194:                                              ; preds = %192
  %195 = add nsw i32 %30, 8
  %196 = icmp ult i32 %30, -7
  br i1 %196, label %197, label %200

197:                                              ; preds = %194
  %198 = sext i32 %30 to i64
  %199 = getelementptr inbounds i8, i8* %11, i64 %198
  br label %203

200:                                              ; preds = %194, %192
  %201 = phi i32 [ %30, %192 ], [ %195, %194 ]
  %202 = getelementptr inbounds i8, i8* %32, i64 8
  br label %203

203:                                              ; preds = %200, %197
  %204 = phi i32 [ %201, %200 ], [ %195, %197 ]
  %205 = phi i8* [ %202, %200 ], [ %32, %197 ]
  %206 = phi i8* [ %32, %200 ], [ %199, %197 ]
  %207 = bitcast i8* %206 to %struct._jobject**
  %208 = load %struct._jobject*, %struct._jobject** %207, align 8
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %210 = bitcast %union.jvalue* %209 to %struct._jobject**
  store %struct._jobject* %208, %struct._jobject** %210, align 8, !tbaa !23
  br label %211

211:                                              ; preds = %203, %183, %164, %146, %127, %107, %87, %67, %47, %29
  %212 = phi i32 [ %30, %29 ], [ %204, %203 ], [ %30, %183 ], [ %30, %164 ], [ %147, %146 ], [ %128, %127 ], [ %108, %107 ], [ %88, %87 ], [ %68, %67 ], [ %48, %47 ]
  %213 = phi i32 [ %31, %29 ], [ %31, %203 ], [ %184, %183 ], [ %165, %164 ], [ %31, %146 ], [ %31, %127 ], [ %31, %107 ], [ %31, %87 ], [ %31, %67 ], [ %31, %47 ]
  %214 = phi i8* [ %32, %29 ], [ %205, %203 ], [ %185, %183 ], [ %166, %164 ], [ %148, %146 ], [ %129, %127 ], [ %109, %107 ], [ %89, %87 ], [ %69, %67 ], [ %49, %47 ]
  %215 = add nuw nsw i64 %33, 1
  %216 = icmp eq i64 %215, %28
  br i1 %216, label %217, label %29, !llvm.loop !69

217:                                              ; preds = %211, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %19) #4
  %218 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %219 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %218, i64 0, i32 87
  %220 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %219, align 8, !tbaa !70
  %221 = call float %220(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %25) #4
  call void @llvm.stackrestore(i8* %18)
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %7) #4
  ret float %221
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local float @JNI_CallNonvirtualFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !11
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !21
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #4
  %12 = sext i32 %11 to i64
  %13 = alloca %union.jvalue, i64 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %204

15:                                               ; preds = %5
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 3
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 1
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 0
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 4
  %20 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 2
  %21 = zext i32 %11 to i64
  br label %22

22:                                               ; preds = %15, %201
  %23 = phi i64 [ 0, %15 ], [ %202, %201 ]
  %24 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 %23
  %25 = load i8, i8* %24, align 1, !tbaa !23
  switch i8 %25, label %201 [
    i8 90, label %26
    i8 66, label %46
    i8 83, label %66
    i8 67, label %86
    i8 73, label %106
    i8 74, label %125
    i8 68, label %143
    i8 70, label %162
    i8 76, label %182
  ]

26:                                               ; preds = %22
  %27 = load i32, i32* %16, align 8
  %28 = icmp sgt i32 %27, -1
  br i1 %28, label %36, label %29

29:                                               ; preds = %26
  %30 = add nsw i32 %27, 8
  store i32 %30, i32* %16, align 8
  %31 = icmp ult i32 %27, -7
  br i1 %31, label %32, label %36

32:                                               ; preds = %29
  %33 = load i8*, i8** %17, align 8
  %34 = sext i32 %27 to i64
  %35 = getelementptr inbounds i8, i8* %33, i64 %34
  br label %39

36:                                               ; preds = %29, %26
  %37 = load i8*, i8** %18, align 8
  %38 = getelementptr inbounds i8, i8* %37, i64 8
  store i8* %38, i8** %18, align 8
  br label %39

39:                                               ; preds = %36, %32
  %40 = phi i8* [ %35, %32 ], [ %37, %36 ]
  %41 = bitcast i8* %40 to i32*
  %42 = load i32, i32* %41, align 8
  %43 = trunc i32 %42 to i8
  %44 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %43, i8* %45, align 8, !tbaa !23
  br label %201

46:                                               ; preds = %22
  %47 = load i32, i32* %16, align 8
  %48 = icmp sgt i32 %47, -1
  br i1 %48, label %56, label %49

49:                                               ; preds = %46
  %50 = add nsw i32 %47, 8
  store i32 %50, i32* %16, align 8
  %51 = icmp ult i32 %47, -7
  br i1 %51, label %52, label %56

52:                                               ; preds = %49
  %53 = load i8*, i8** %17, align 8
  %54 = sext i32 %47 to i64
  %55 = getelementptr inbounds i8, i8* %53, i64 %54
  br label %59

56:                                               ; preds = %49, %46
  %57 = load i8*, i8** %18, align 8
  %58 = getelementptr inbounds i8, i8* %57, i64 8
  store i8* %58, i8** %18, align 8
  br label %59

59:                                               ; preds = %56, %52
  %60 = phi i8* [ %55, %52 ], [ %57, %56 ]
  %61 = bitcast i8* %60 to i32*
  %62 = load i32, i32* %61, align 8
  %63 = trunc i32 %62 to i8
  %64 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %63, i8* %65, align 8, !tbaa !23
  br label %201

66:                                               ; preds = %22
  %67 = load i32, i32* %16, align 8
  %68 = icmp sgt i32 %67, -1
  br i1 %68, label %76, label %69

69:                                               ; preds = %66
  %70 = add nsw i32 %67, 8
  store i32 %70, i32* %16, align 8
  %71 = icmp ult i32 %67, -7
  br i1 %71, label %72, label %76

72:                                               ; preds = %69
  %73 = load i8*, i8** %17, align 8
  %74 = sext i32 %67 to i64
  %75 = getelementptr inbounds i8, i8* %73, i64 %74
  br label %79

76:                                               ; preds = %69, %66
  %77 = load i8*, i8** %18, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %18, align 8
  br label %79

79:                                               ; preds = %76, %72
  %80 = phi i8* [ %75, %72 ], [ %77, %76 ]
  %81 = bitcast i8* %80 to i32*
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i16
  %84 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %85 = bitcast %union.jvalue* %84 to i16*
  store i16 %83, i16* %85, align 8, !tbaa !23
  br label %201

86:                                               ; preds = %22
  %87 = load i32, i32* %16, align 8
  %88 = icmp sgt i32 %87, -1
  br i1 %88, label %96, label %89

89:                                               ; preds = %86
  %90 = add nsw i32 %87, 8
  store i32 %90, i32* %16, align 8
  %91 = icmp ult i32 %87, -7
  br i1 %91, label %92, label %96

92:                                               ; preds = %89
  %93 = load i8*, i8** %17, align 8
  %94 = sext i32 %87 to i64
  %95 = getelementptr inbounds i8, i8* %93, i64 %94
  br label %99

96:                                               ; preds = %89, %86
  %97 = load i8*, i8** %18, align 8
  %98 = getelementptr inbounds i8, i8* %97, i64 8
  store i8* %98, i8** %18, align 8
  br label %99

99:                                               ; preds = %96, %92
  %100 = phi i8* [ %95, %92 ], [ %97, %96 ]
  %101 = bitcast i8* %100 to i32*
  %102 = load i32, i32* %101, align 8
  %103 = and i32 %102, 65535
  %104 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %103, i32* %105, align 8, !tbaa !23
  br label %201

106:                                              ; preds = %22
  %107 = load i32, i32* %16, align 8
  %108 = icmp sgt i32 %107, -1
  br i1 %108, label %116, label %109

109:                                              ; preds = %106
  %110 = add nsw i32 %107, 8
  store i32 %110, i32* %16, align 8
  %111 = icmp ult i32 %107, -7
  br i1 %111, label %112, label %116

112:                                              ; preds = %109
  %113 = load i8*, i8** %17, align 8
  %114 = sext i32 %107 to i64
  %115 = getelementptr inbounds i8, i8* %113, i64 %114
  br label %119

116:                                              ; preds = %109, %106
  %117 = load i8*, i8** %18, align 8
  %118 = getelementptr inbounds i8, i8* %117, i64 8
  store i8* %118, i8** %18, align 8
  br label %119

119:                                              ; preds = %116, %112
  %120 = phi i8* [ %115, %112 ], [ %117, %116 ]
  %121 = bitcast i8* %120 to i32*
  %122 = load i32, i32* %121, align 8
  %123 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %124 = bitcast %union.jvalue* %123 to i32*
  store i32 %122, i32* %124, align 8, !tbaa !23
  br label %201

125:                                              ; preds = %22
  %126 = load i32, i32* %16, align 8
  %127 = icmp sgt i32 %126, -1
  br i1 %127, label %135, label %128

128:                                              ; preds = %125
  %129 = add nsw i32 %126, 8
  store i32 %129, i32* %16, align 8
  %130 = icmp ult i32 %126, -7
  br i1 %130, label %131, label %135

131:                                              ; preds = %128
  %132 = load i8*, i8** %17, align 8
  %133 = sext i32 %126 to i64
  %134 = getelementptr inbounds i8, i8* %132, i64 %133
  br label %138

135:                                              ; preds = %128, %125
  %136 = load i8*, i8** %18, align 8
  %137 = getelementptr inbounds i8, i8* %136, i64 8
  store i8* %137, i8** %18, align 8
  br label %138

138:                                              ; preds = %135, %131
  %139 = phi i8* [ %134, %131 ], [ %136, %135 ]
  %140 = bitcast i8* %139 to i64*
  %141 = load i64, i64* %140, align 8
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23, i32 0
  store i64 %141, i64* %142, align 8, !tbaa !23
  br label %201

143:                                              ; preds = %22
  %144 = load i32, i32* %19, align 4
  %145 = icmp sgt i32 %144, -1
  br i1 %145, label %153, label %146

146:                                              ; preds = %143
  %147 = add nsw i32 %144, 16
  store i32 %147, i32* %19, align 4
  %148 = icmp ult i32 %144, -15
  br i1 %148, label %149, label %153

149:                                              ; preds = %146
  %150 = load i8*, i8** %20, align 8
  %151 = sext i32 %144 to i64
  %152 = getelementptr inbounds i8, i8* %150, i64 %151
  br label %156

153:                                              ; preds = %146, %143
  %154 = load i8*, i8** %18, align 8
  %155 = getelementptr inbounds i8, i8* %154, i64 8
  store i8* %155, i8** %18, align 8
  br label %156

156:                                              ; preds = %153, %149
  %157 = phi i8* [ %152, %149 ], [ %154, %153 ]
  %158 = bitcast i8* %157 to double*
  %159 = load double, double* %158, align 8
  %160 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %161 = bitcast %union.jvalue* %160 to double*
  store double %159, double* %161, align 8, !tbaa !23
  br label %201

162:                                              ; preds = %22
  %163 = load i32, i32* %19, align 4
  %164 = icmp sgt i32 %163, -1
  br i1 %164, label %172, label %165

165:                                              ; preds = %162
  %166 = add nsw i32 %163, 16
  store i32 %166, i32* %19, align 4
  %167 = icmp ult i32 %163, -15
  br i1 %167, label %168, label %172

168:                                              ; preds = %165
  %169 = load i8*, i8** %20, align 8
  %170 = sext i32 %163 to i64
  %171 = getelementptr inbounds i8, i8* %169, i64 %170
  br label %175

172:                                              ; preds = %165, %162
  %173 = load i8*, i8** %18, align 8
  %174 = getelementptr inbounds i8, i8* %173, i64 8
  store i8* %174, i8** %18, align 8
  br label %175

175:                                              ; preds = %172, %168
  %176 = phi i8* [ %171, %168 ], [ %173, %172 ]
  %177 = bitcast i8* %176 to double*
  %178 = load double, double* %177, align 8
  %179 = fptrunc double %178 to float
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %181 = bitcast %union.jvalue* %180 to float*
  store float %179, float* %181, align 8, !tbaa !23
  br label %201

182:                                              ; preds = %22
  %183 = load i32, i32* %16, align 8
  %184 = icmp sgt i32 %183, -1
  br i1 %184, label %192, label %185

185:                                              ; preds = %182
  %186 = add nsw i32 %183, 8
  store i32 %186, i32* %16, align 8
  %187 = icmp ult i32 %183, -7
  br i1 %187, label %188, label %192

188:                                              ; preds = %185
  %189 = load i8*, i8** %17, align 8
  %190 = sext i32 %183 to i64
  %191 = getelementptr inbounds i8, i8* %189, i64 %190
  br label %195

192:                                              ; preds = %185, %182
  %193 = load i8*, i8** %18, align 8
  %194 = getelementptr inbounds i8, i8* %193, i64 8
  store i8* %194, i8** %18, align 8
  br label %195

195:                                              ; preds = %192, %188
  %196 = phi i8* [ %191, %188 ], [ %193, %192 ]
  %197 = bitcast i8* %196 to %struct._jobject**
  %198 = load %struct._jobject*, %struct._jobject** %197, align 8
  %199 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %200 = bitcast %union.jvalue* %199 to %struct._jobject**
  store %struct._jobject* %198, %struct._jobject** %200, align 8, !tbaa !23
  br label %201

201:                                              ; preds = %39, %59, %79, %99, %119, %138, %156, %175, %195, %22
  %202 = add nuw nsw i64 %23, 1
  %203 = icmp eq i64 %202, %21
  br i1 %203, label %204, label %22, !llvm.loop !69

204:                                              ; preds = %201, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %205 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %206 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %205, i64 0, i32 87
  %207 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %206, align 8, !tbaa !70
  %208 = call float %207(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret float %208
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local float @JNI_CallStaticFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !71

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 137
  %219 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !72
  %220 = call float %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret float %220
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local float @JNI_CallStaticFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !71

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 137
  %206 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !72
  %207 = call float %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret float %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local double @JNI_CallDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !73

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 60
  %219 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !74
  %220 = call double %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret double %220
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local double @JNI_CallDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !73

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 60
  %206 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !74
  %207 = call double %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret double %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local double @JNI_CallNonvirtualDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca %"struct.std::__va_list", align 8
  %7 = bitcast %"struct.std::__va_list"* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 0
  %9 = load i8*, i8** %8, align 8, !tbaa.struct !10
  %10 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 1
  %11 = load i8*, i8** %10, align 8, !tbaa.struct !17
  %12 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 2
  %13 = load i8*, i8** %12, align 8, !tbaa.struct !18
  %14 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 3
  %15 = load i32, i32* %14, align 8, !tbaa.struct !19
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 4
  %17 = load i32, i32* %16, align 4, !tbaa.struct !20
  %18 = call i8* @llvm.stacksave()
  %19 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %19) #4
  %20 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %20, align 8, !tbaa !11
  %22 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %21, align 8, !tbaa !21
  %23 = call i32 %22(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %19) #4
  %24 = sext i32 %23 to i64
  %25 = alloca %union.jvalue, i64 %24, align 16
  %26 = icmp sgt i32 %23, 0
  br i1 %26, label %27, label %217

27:                                               ; preds = %4
  %28 = zext i32 %23 to i64
  br label %29

29:                                               ; preds = %211, %27
  %30 = phi i32 [ %15, %27 ], [ %212, %211 ]
  %31 = phi i32 [ %17, %27 ], [ %213, %211 ]
  %32 = phi i8* [ %9, %27 ], [ %214, %211 ]
  %33 = phi i64 [ 0, %27 ], [ %215, %211 ]
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1, !tbaa !23
  switch i8 %35, label %211 [
    i8 90, label %36
    i8 66, label %56
    i8 83, label %76
    i8 67, label %96
    i8 73, label %116
    i8 74, label %135
    i8 68, label %153
    i8 70, label %172
    i8 76, label %192
  ]

36:                                               ; preds = %29
  %37 = icmp sgt i32 %30, -1
  br i1 %37, label %44, label %38

38:                                               ; preds = %36
  %39 = add nsw i32 %30, 8
  %40 = icmp ult i32 %30, -7
  br i1 %40, label %41, label %44

41:                                               ; preds = %38
  %42 = sext i32 %30 to i64
  %43 = getelementptr inbounds i8, i8* %11, i64 %42
  br label %47

44:                                               ; preds = %38, %36
  %45 = phi i32 [ %30, %36 ], [ %39, %38 ]
  %46 = getelementptr inbounds i8, i8* %32, i64 8
  br label %47

47:                                               ; preds = %44, %41
  %48 = phi i32 [ %45, %44 ], [ %39, %41 ]
  %49 = phi i8* [ %46, %44 ], [ %32, %41 ]
  %50 = phi i8* [ %32, %44 ], [ %43, %41 ]
  %51 = bitcast i8* %50 to i32*
  %52 = load i32, i32* %51, align 8
  %53 = trunc i32 %52 to i8
  %54 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %55 = bitcast %union.jvalue* %54 to i8*
  store i8 %53, i8* %55, align 8, !tbaa !23
  br label %211

56:                                               ; preds = %29
  %57 = icmp sgt i32 %30, -1
  br i1 %57, label %64, label %58

58:                                               ; preds = %56
  %59 = add nsw i32 %30, 8
  %60 = icmp ult i32 %30, -7
  br i1 %60, label %61, label %64

61:                                               ; preds = %58
  %62 = sext i32 %30 to i64
  %63 = getelementptr inbounds i8, i8* %11, i64 %62
  br label %67

64:                                               ; preds = %58, %56
  %65 = phi i32 [ %30, %56 ], [ %59, %58 ]
  %66 = getelementptr inbounds i8, i8* %32, i64 8
  br label %67

67:                                               ; preds = %64, %61
  %68 = phi i32 [ %65, %64 ], [ %59, %61 ]
  %69 = phi i8* [ %66, %64 ], [ %32, %61 ]
  %70 = phi i8* [ %32, %64 ], [ %63, %61 ]
  %71 = bitcast i8* %70 to i32*
  %72 = load i32, i32* %71, align 8
  %73 = trunc i32 %72 to i8
  %74 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %75 = bitcast %union.jvalue* %74 to i8*
  store i8 %73, i8* %75, align 8, !tbaa !23
  br label %211

76:                                               ; preds = %29
  %77 = icmp sgt i32 %30, -1
  br i1 %77, label %84, label %78

78:                                               ; preds = %76
  %79 = add nsw i32 %30, 8
  %80 = icmp ult i32 %30, -7
  br i1 %80, label %81, label %84

81:                                               ; preds = %78
  %82 = sext i32 %30 to i64
  %83 = getelementptr inbounds i8, i8* %11, i64 %82
  br label %87

84:                                               ; preds = %78, %76
  %85 = phi i32 [ %30, %76 ], [ %79, %78 ]
  %86 = getelementptr inbounds i8, i8* %32, i64 8
  br label %87

87:                                               ; preds = %84, %81
  %88 = phi i32 [ %85, %84 ], [ %79, %81 ]
  %89 = phi i8* [ %86, %84 ], [ %32, %81 ]
  %90 = phi i8* [ %32, %84 ], [ %83, %81 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 8
  %93 = trunc i32 %92 to i16
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %93, i16* %95, align 8, !tbaa !23
  br label %211

96:                                               ; preds = %29
  %97 = icmp sgt i32 %30, -1
  br i1 %97, label %104, label %98

98:                                               ; preds = %96
  %99 = add nsw i32 %30, 8
  %100 = icmp ult i32 %30, -7
  br i1 %100, label %101, label %104

101:                                              ; preds = %98
  %102 = sext i32 %30 to i64
  %103 = getelementptr inbounds i8, i8* %11, i64 %102
  br label %107

104:                                              ; preds = %98, %96
  %105 = phi i32 [ %30, %96 ], [ %99, %98 ]
  %106 = getelementptr inbounds i8, i8* %32, i64 8
  br label %107

107:                                              ; preds = %104, %101
  %108 = phi i32 [ %105, %104 ], [ %99, %101 ]
  %109 = phi i8* [ %106, %104 ], [ %32, %101 ]
  %110 = phi i8* [ %32, %104 ], [ %103, %101 ]
  %111 = bitcast i8* %110 to i32*
  %112 = load i32, i32* %111, align 8
  %113 = and i32 %112, 65535
  %114 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %115 = bitcast %union.jvalue* %114 to i32*
  store i32 %113, i32* %115, align 8, !tbaa !23
  br label %211

116:                                              ; preds = %29
  %117 = icmp sgt i32 %30, -1
  br i1 %117, label %124, label %118

118:                                              ; preds = %116
  %119 = add nsw i32 %30, 8
  %120 = icmp ult i32 %30, -7
  br i1 %120, label %121, label %124

121:                                              ; preds = %118
  %122 = sext i32 %30 to i64
  %123 = getelementptr inbounds i8, i8* %11, i64 %122
  br label %127

124:                                              ; preds = %118, %116
  %125 = phi i32 [ %30, %116 ], [ %119, %118 ]
  %126 = getelementptr inbounds i8, i8* %32, i64 8
  br label %127

127:                                              ; preds = %124, %121
  %128 = phi i32 [ %125, %124 ], [ %119, %121 ]
  %129 = phi i8* [ %126, %124 ], [ %32, %121 ]
  %130 = phi i8* [ %32, %124 ], [ %123, %121 ]
  %131 = bitcast i8* %130 to i32*
  %132 = load i32, i32* %131, align 8
  %133 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %134 = bitcast %union.jvalue* %133 to i32*
  store i32 %132, i32* %134, align 8, !tbaa !23
  br label %211

135:                                              ; preds = %29
  %136 = icmp sgt i32 %30, -1
  br i1 %136, label %143, label %137

137:                                              ; preds = %135
  %138 = add nsw i32 %30, 8
  %139 = icmp ult i32 %30, -7
  br i1 %139, label %140, label %143

140:                                              ; preds = %137
  %141 = sext i32 %30 to i64
  %142 = getelementptr inbounds i8, i8* %11, i64 %141
  br label %146

143:                                              ; preds = %137, %135
  %144 = phi i32 [ %30, %135 ], [ %138, %137 ]
  %145 = getelementptr inbounds i8, i8* %32, i64 8
  br label %146

146:                                              ; preds = %143, %140
  %147 = phi i32 [ %144, %143 ], [ %138, %140 ]
  %148 = phi i8* [ %145, %143 ], [ %32, %140 ]
  %149 = phi i8* [ %32, %143 ], [ %142, %140 ]
  %150 = bitcast i8* %149 to i64*
  %151 = load i64, i64* %150, align 8
  %152 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33, i32 0
  store i64 %151, i64* %152, align 8, !tbaa !23
  br label %211

153:                                              ; preds = %29
  %154 = icmp sgt i32 %31, -1
  br i1 %154, label %161, label %155

155:                                              ; preds = %153
  %156 = add nsw i32 %31, 16
  %157 = icmp ult i32 %31, -15
  br i1 %157, label %158, label %161

158:                                              ; preds = %155
  %159 = sext i32 %31 to i64
  %160 = getelementptr inbounds i8, i8* %13, i64 %159
  br label %164

161:                                              ; preds = %155, %153
  %162 = phi i32 [ %31, %153 ], [ %156, %155 ]
  %163 = getelementptr inbounds i8, i8* %32, i64 8
  br label %164

164:                                              ; preds = %161, %158
  %165 = phi i32 [ %162, %161 ], [ %156, %158 ]
  %166 = phi i8* [ %163, %161 ], [ %32, %158 ]
  %167 = phi i8* [ %32, %161 ], [ %160, %158 ]
  %168 = bitcast i8* %167 to double*
  %169 = load double, double* %168, align 8
  %170 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %171 = bitcast %union.jvalue* %170 to double*
  store double %169, double* %171, align 8, !tbaa !23
  br label %211

172:                                              ; preds = %29
  %173 = icmp sgt i32 %31, -1
  br i1 %173, label %180, label %174

174:                                              ; preds = %172
  %175 = add nsw i32 %31, 16
  %176 = icmp ult i32 %31, -15
  br i1 %176, label %177, label %180

177:                                              ; preds = %174
  %178 = sext i32 %31 to i64
  %179 = getelementptr inbounds i8, i8* %13, i64 %178
  br label %183

180:                                              ; preds = %174, %172
  %181 = phi i32 [ %31, %172 ], [ %175, %174 ]
  %182 = getelementptr inbounds i8, i8* %32, i64 8
  br label %183

183:                                              ; preds = %180, %177
  %184 = phi i32 [ %181, %180 ], [ %175, %177 ]
  %185 = phi i8* [ %182, %180 ], [ %32, %177 ]
  %186 = phi i8* [ %32, %180 ], [ %179, %177 ]
  %187 = bitcast i8* %186 to double*
  %188 = load double, double* %187, align 8
  %189 = fptrunc double %188 to float
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %191 = bitcast %union.jvalue* %190 to float*
  store float %189, float* %191, align 8, !tbaa !23
  br label %211

192:                                              ; preds = %29
  %193 = icmp sgt i32 %30, -1
  br i1 %193, label %200, label %194

194:                                              ; preds = %192
  %195 = add nsw i32 %30, 8
  %196 = icmp ult i32 %30, -7
  br i1 %196, label %197, label %200

197:                                              ; preds = %194
  %198 = sext i32 %30 to i64
  %199 = getelementptr inbounds i8, i8* %11, i64 %198
  br label %203

200:                                              ; preds = %194, %192
  %201 = phi i32 [ %30, %192 ], [ %195, %194 ]
  %202 = getelementptr inbounds i8, i8* %32, i64 8
  br label %203

203:                                              ; preds = %200, %197
  %204 = phi i32 [ %201, %200 ], [ %195, %197 ]
  %205 = phi i8* [ %202, %200 ], [ %32, %197 ]
  %206 = phi i8* [ %32, %200 ], [ %199, %197 ]
  %207 = bitcast i8* %206 to %struct._jobject**
  %208 = load %struct._jobject*, %struct._jobject** %207, align 8
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %210 = bitcast %union.jvalue* %209 to %struct._jobject**
  store %struct._jobject* %208, %struct._jobject** %210, align 8, !tbaa !23
  br label %211

211:                                              ; preds = %203, %183, %164, %146, %127, %107, %87, %67, %47, %29
  %212 = phi i32 [ %30, %29 ], [ %204, %203 ], [ %30, %183 ], [ %30, %164 ], [ %147, %146 ], [ %128, %127 ], [ %108, %107 ], [ %88, %87 ], [ %68, %67 ], [ %48, %47 ]
  %213 = phi i32 [ %31, %29 ], [ %31, %203 ], [ %184, %183 ], [ %165, %164 ], [ %31, %146 ], [ %31, %127 ], [ %31, %107 ], [ %31, %87 ], [ %31, %67 ], [ %31, %47 ]
  %214 = phi i8* [ %32, %29 ], [ %205, %203 ], [ %185, %183 ], [ %166, %164 ], [ %148, %146 ], [ %129, %127 ], [ %109, %107 ], [ %89, %87 ], [ %69, %67 ], [ %49, %47 ]
  %215 = add nuw nsw i64 %33, 1
  %216 = icmp eq i64 %215, %28
  br i1 %216, label %217, label %29, !llvm.loop !75

217:                                              ; preds = %211, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %19) #4
  %218 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %219 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %218, i64 0, i32 90
  %220 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %219, align 8, !tbaa !76
  %221 = call double %220(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %25) #4
  call void @llvm.stackrestore(i8* %18)
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %7) #4
  ret double %221
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local double @JNI_CallNonvirtualDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !11
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !21
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #4
  %12 = sext i32 %11 to i64
  %13 = alloca %union.jvalue, i64 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %204

15:                                               ; preds = %5
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 3
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 1
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 0
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 4
  %20 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 2
  %21 = zext i32 %11 to i64
  br label %22

22:                                               ; preds = %15, %201
  %23 = phi i64 [ 0, %15 ], [ %202, %201 ]
  %24 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 %23
  %25 = load i8, i8* %24, align 1, !tbaa !23
  switch i8 %25, label %201 [
    i8 90, label %26
    i8 66, label %46
    i8 83, label %66
    i8 67, label %86
    i8 73, label %106
    i8 74, label %125
    i8 68, label %143
    i8 70, label %162
    i8 76, label %182
  ]

26:                                               ; preds = %22
  %27 = load i32, i32* %16, align 8
  %28 = icmp sgt i32 %27, -1
  br i1 %28, label %36, label %29

29:                                               ; preds = %26
  %30 = add nsw i32 %27, 8
  store i32 %30, i32* %16, align 8
  %31 = icmp ult i32 %27, -7
  br i1 %31, label %32, label %36

32:                                               ; preds = %29
  %33 = load i8*, i8** %17, align 8
  %34 = sext i32 %27 to i64
  %35 = getelementptr inbounds i8, i8* %33, i64 %34
  br label %39

36:                                               ; preds = %29, %26
  %37 = load i8*, i8** %18, align 8
  %38 = getelementptr inbounds i8, i8* %37, i64 8
  store i8* %38, i8** %18, align 8
  br label %39

39:                                               ; preds = %36, %32
  %40 = phi i8* [ %35, %32 ], [ %37, %36 ]
  %41 = bitcast i8* %40 to i32*
  %42 = load i32, i32* %41, align 8
  %43 = trunc i32 %42 to i8
  %44 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %43, i8* %45, align 8, !tbaa !23
  br label %201

46:                                               ; preds = %22
  %47 = load i32, i32* %16, align 8
  %48 = icmp sgt i32 %47, -1
  br i1 %48, label %56, label %49

49:                                               ; preds = %46
  %50 = add nsw i32 %47, 8
  store i32 %50, i32* %16, align 8
  %51 = icmp ult i32 %47, -7
  br i1 %51, label %52, label %56

52:                                               ; preds = %49
  %53 = load i8*, i8** %17, align 8
  %54 = sext i32 %47 to i64
  %55 = getelementptr inbounds i8, i8* %53, i64 %54
  br label %59

56:                                               ; preds = %49, %46
  %57 = load i8*, i8** %18, align 8
  %58 = getelementptr inbounds i8, i8* %57, i64 8
  store i8* %58, i8** %18, align 8
  br label %59

59:                                               ; preds = %56, %52
  %60 = phi i8* [ %55, %52 ], [ %57, %56 ]
  %61 = bitcast i8* %60 to i32*
  %62 = load i32, i32* %61, align 8
  %63 = trunc i32 %62 to i8
  %64 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %63, i8* %65, align 8, !tbaa !23
  br label %201

66:                                               ; preds = %22
  %67 = load i32, i32* %16, align 8
  %68 = icmp sgt i32 %67, -1
  br i1 %68, label %76, label %69

69:                                               ; preds = %66
  %70 = add nsw i32 %67, 8
  store i32 %70, i32* %16, align 8
  %71 = icmp ult i32 %67, -7
  br i1 %71, label %72, label %76

72:                                               ; preds = %69
  %73 = load i8*, i8** %17, align 8
  %74 = sext i32 %67 to i64
  %75 = getelementptr inbounds i8, i8* %73, i64 %74
  br label %79

76:                                               ; preds = %69, %66
  %77 = load i8*, i8** %18, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %18, align 8
  br label %79

79:                                               ; preds = %76, %72
  %80 = phi i8* [ %75, %72 ], [ %77, %76 ]
  %81 = bitcast i8* %80 to i32*
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i16
  %84 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %85 = bitcast %union.jvalue* %84 to i16*
  store i16 %83, i16* %85, align 8, !tbaa !23
  br label %201

86:                                               ; preds = %22
  %87 = load i32, i32* %16, align 8
  %88 = icmp sgt i32 %87, -1
  br i1 %88, label %96, label %89

89:                                               ; preds = %86
  %90 = add nsw i32 %87, 8
  store i32 %90, i32* %16, align 8
  %91 = icmp ult i32 %87, -7
  br i1 %91, label %92, label %96

92:                                               ; preds = %89
  %93 = load i8*, i8** %17, align 8
  %94 = sext i32 %87 to i64
  %95 = getelementptr inbounds i8, i8* %93, i64 %94
  br label %99

96:                                               ; preds = %89, %86
  %97 = load i8*, i8** %18, align 8
  %98 = getelementptr inbounds i8, i8* %97, i64 8
  store i8* %98, i8** %18, align 8
  br label %99

99:                                               ; preds = %96, %92
  %100 = phi i8* [ %95, %92 ], [ %97, %96 ]
  %101 = bitcast i8* %100 to i32*
  %102 = load i32, i32* %101, align 8
  %103 = and i32 %102, 65535
  %104 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %103, i32* %105, align 8, !tbaa !23
  br label %201

106:                                              ; preds = %22
  %107 = load i32, i32* %16, align 8
  %108 = icmp sgt i32 %107, -1
  br i1 %108, label %116, label %109

109:                                              ; preds = %106
  %110 = add nsw i32 %107, 8
  store i32 %110, i32* %16, align 8
  %111 = icmp ult i32 %107, -7
  br i1 %111, label %112, label %116

112:                                              ; preds = %109
  %113 = load i8*, i8** %17, align 8
  %114 = sext i32 %107 to i64
  %115 = getelementptr inbounds i8, i8* %113, i64 %114
  br label %119

116:                                              ; preds = %109, %106
  %117 = load i8*, i8** %18, align 8
  %118 = getelementptr inbounds i8, i8* %117, i64 8
  store i8* %118, i8** %18, align 8
  br label %119

119:                                              ; preds = %116, %112
  %120 = phi i8* [ %115, %112 ], [ %117, %116 ]
  %121 = bitcast i8* %120 to i32*
  %122 = load i32, i32* %121, align 8
  %123 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %124 = bitcast %union.jvalue* %123 to i32*
  store i32 %122, i32* %124, align 8, !tbaa !23
  br label %201

125:                                              ; preds = %22
  %126 = load i32, i32* %16, align 8
  %127 = icmp sgt i32 %126, -1
  br i1 %127, label %135, label %128

128:                                              ; preds = %125
  %129 = add nsw i32 %126, 8
  store i32 %129, i32* %16, align 8
  %130 = icmp ult i32 %126, -7
  br i1 %130, label %131, label %135

131:                                              ; preds = %128
  %132 = load i8*, i8** %17, align 8
  %133 = sext i32 %126 to i64
  %134 = getelementptr inbounds i8, i8* %132, i64 %133
  br label %138

135:                                              ; preds = %128, %125
  %136 = load i8*, i8** %18, align 8
  %137 = getelementptr inbounds i8, i8* %136, i64 8
  store i8* %137, i8** %18, align 8
  br label %138

138:                                              ; preds = %135, %131
  %139 = phi i8* [ %134, %131 ], [ %136, %135 ]
  %140 = bitcast i8* %139 to i64*
  %141 = load i64, i64* %140, align 8
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23, i32 0
  store i64 %141, i64* %142, align 8, !tbaa !23
  br label %201

143:                                              ; preds = %22
  %144 = load i32, i32* %19, align 4
  %145 = icmp sgt i32 %144, -1
  br i1 %145, label %153, label %146

146:                                              ; preds = %143
  %147 = add nsw i32 %144, 16
  store i32 %147, i32* %19, align 4
  %148 = icmp ult i32 %144, -15
  br i1 %148, label %149, label %153

149:                                              ; preds = %146
  %150 = load i8*, i8** %20, align 8
  %151 = sext i32 %144 to i64
  %152 = getelementptr inbounds i8, i8* %150, i64 %151
  br label %156

153:                                              ; preds = %146, %143
  %154 = load i8*, i8** %18, align 8
  %155 = getelementptr inbounds i8, i8* %154, i64 8
  store i8* %155, i8** %18, align 8
  br label %156

156:                                              ; preds = %153, %149
  %157 = phi i8* [ %152, %149 ], [ %154, %153 ]
  %158 = bitcast i8* %157 to double*
  %159 = load double, double* %158, align 8
  %160 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %161 = bitcast %union.jvalue* %160 to double*
  store double %159, double* %161, align 8, !tbaa !23
  br label %201

162:                                              ; preds = %22
  %163 = load i32, i32* %19, align 4
  %164 = icmp sgt i32 %163, -1
  br i1 %164, label %172, label %165

165:                                              ; preds = %162
  %166 = add nsw i32 %163, 16
  store i32 %166, i32* %19, align 4
  %167 = icmp ult i32 %163, -15
  br i1 %167, label %168, label %172

168:                                              ; preds = %165
  %169 = load i8*, i8** %20, align 8
  %170 = sext i32 %163 to i64
  %171 = getelementptr inbounds i8, i8* %169, i64 %170
  br label %175

172:                                              ; preds = %165, %162
  %173 = load i8*, i8** %18, align 8
  %174 = getelementptr inbounds i8, i8* %173, i64 8
  store i8* %174, i8** %18, align 8
  br label %175

175:                                              ; preds = %172, %168
  %176 = phi i8* [ %171, %168 ], [ %173, %172 ]
  %177 = bitcast i8* %176 to double*
  %178 = load double, double* %177, align 8
  %179 = fptrunc double %178 to float
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %181 = bitcast %union.jvalue* %180 to float*
  store float %179, float* %181, align 8, !tbaa !23
  br label %201

182:                                              ; preds = %22
  %183 = load i32, i32* %16, align 8
  %184 = icmp sgt i32 %183, -1
  br i1 %184, label %192, label %185

185:                                              ; preds = %182
  %186 = add nsw i32 %183, 8
  store i32 %186, i32* %16, align 8
  %187 = icmp ult i32 %183, -7
  br i1 %187, label %188, label %192

188:                                              ; preds = %185
  %189 = load i8*, i8** %17, align 8
  %190 = sext i32 %183 to i64
  %191 = getelementptr inbounds i8, i8* %189, i64 %190
  br label %195

192:                                              ; preds = %185, %182
  %193 = load i8*, i8** %18, align 8
  %194 = getelementptr inbounds i8, i8* %193, i64 8
  store i8* %194, i8** %18, align 8
  br label %195

195:                                              ; preds = %192, %188
  %196 = phi i8* [ %191, %188 ], [ %193, %192 ]
  %197 = bitcast i8* %196 to %struct._jobject**
  %198 = load %struct._jobject*, %struct._jobject** %197, align 8
  %199 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %200 = bitcast %union.jvalue* %199 to %struct._jobject**
  store %struct._jobject* %198, %struct._jobject** %200, align 8, !tbaa !23
  br label %201

201:                                              ; preds = %39, %59, %79, %99, %119, %138, %156, %175, %195, %22
  %202 = add nuw nsw i64 %23, 1
  %203 = icmp eq i64 %202, %21
  br i1 %203, label %204, label %22, !llvm.loop !75

204:                                              ; preds = %201, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %205 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %206 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %205, i64 0, i32 90
  %207 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %206, align 8, !tbaa !76
  %208 = call double %207(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret double %208
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local double @JNI_CallStaticDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !77

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 140
  %219 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !78
  %220 = call double %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret double %220
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local double @JNI_CallStaticDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !77

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 140
  %206 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !78
  %207 = call double %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret double %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_NewObject(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !79

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 30
  %219 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !80
  %220 = call %struct._jobject* %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret %struct._jobject* %220
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_NewObjectV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !79

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 30
  %206 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !80
  %207 = call %struct._jobject* %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret %struct._jobject* %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local void @JNI_CallVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !81

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 63
  %219 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !82
  call void %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local void @JNI_CallVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !81

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 63
  %206 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !82
  call void %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local void @JNI_CallNonvirtualVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca %"struct.std::__va_list", align 8
  %7 = bitcast %"struct.std::__va_list"* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 0
  %9 = load i8*, i8** %8, align 8, !tbaa.struct !10
  %10 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 1
  %11 = load i8*, i8** %10, align 8, !tbaa.struct !17
  %12 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 2
  %13 = load i8*, i8** %12, align 8, !tbaa.struct !18
  %14 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 3
  %15 = load i32, i32* %14, align 8, !tbaa.struct !19
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %6, i64 0, i32 4
  %17 = load i32, i32* %16, align 4, !tbaa.struct !20
  %18 = call i8* @llvm.stacksave()
  %19 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %19) #4
  %20 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %20, align 8, !tbaa !11
  %22 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %21, align 8, !tbaa !21
  %23 = call i32 %22(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %19) #4
  %24 = sext i32 %23 to i64
  %25 = alloca %union.jvalue, i64 %24, align 16
  %26 = icmp sgt i32 %23, 0
  br i1 %26, label %27, label %217

27:                                               ; preds = %4
  %28 = zext i32 %23 to i64
  br label %29

29:                                               ; preds = %211, %27
  %30 = phi i32 [ %15, %27 ], [ %212, %211 ]
  %31 = phi i32 [ %17, %27 ], [ %213, %211 ]
  %32 = phi i8* [ %9, %27 ], [ %214, %211 ]
  %33 = phi i64 [ 0, %27 ], [ %215, %211 ]
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1, !tbaa !23
  switch i8 %35, label %211 [
    i8 90, label %36
    i8 66, label %56
    i8 83, label %76
    i8 67, label %96
    i8 73, label %116
    i8 74, label %135
    i8 68, label %153
    i8 70, label %172
    i8 76, label %192
  ]

36:                                               ; preds = %29
  %37 = icmp sgt i32 %30, -1
  br i1 %37, label %44, label %38

38:                                               ; preds = %36
  %39 = add nsw i32 %30, 8
  %40 = icmp ult i32 %30, -7
  br i1 %40, label %41, label %44

41:                                               ; preds = %38
  %42 = sext i32 %30 to i64
  %43 = getelementptr inbounds i8, i8* %11, i64 %42
  br label %47

44:                                               ; preds = %38, %36
  %45 = phi i32 [ %30, %36 ], [ %39, %38 ]
  %46 = getelementptr inbounds i8, i8* %32, i64 8
  br label %47

47:                                               ; preds = %44, %41
  %48 = phi i32 [ %45, %44 ], [ %39, %41 ]
  %49 = phi i8* [ %46, %44 ], [ %32, %41 ]
  %50 = phi i8* [ %32, %44 ], [ %43, %41 ]
  %51 = bitcast i8* %50 to i32*
  %52 = load i32, i32* %51, align 8
  %53 = trunc i32 %52 to i8
  %54 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %55 = bitcast %union.jvalue* %54 to i8*
  store i8 %53, i8* %55, align 8, !tbaa !23
  br label %211

56:                                               ; preds = %29
  %57 = icmp sgt i32 %30, -1
  br i1 %57, label %64, label %58

58:                                               ; preds = %56
  %59 = add nsw i32 %30, 8
  %60 = icmp ult i32 %30, -7
  br i1 %60, label %61, label %64

61:                                               ; preds = %58
  %62 = sext i32 %30 to i64
  %63 = getelementptr inbounds i8, i8* %11, i64 %62
  br label %67

64:                                               ; preds = %58, %56
  %65 = phi i32 [ %30, %56 ], [ %59, %58 ]
  %66 = getelementptr inbounds i8, i8* %32, i64 8
  br label %67

67:                                               ; preds = %64, %61
  %68 = phi i32 [ %65, %64 ], [ %59, %61 ]
  %69 = phi i8* [ %66, %64 ], [ %32, %61 ]
  %70 = phi i8* [ %32, %64 ], [ %63, %61 ]
  %71 = bitcast i8* %70 to i32*
  %72 = load i32, i32* %71, align 8
  %73 = trunc i32 %72 to i8
  %74 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %75 = bitcast %union.jvalue* %74 to i8*
  store i8 %73, i8* %75, align 8, !tbaa !23
  br label %211

76:                                               ; preds = %29
  %77 = icmp sgt i32 %30, -1
  br i1 %77, label %84, label %78

78:                                               ; preds = %76
  %79 = add nsw i32 %30, 8
  %80 = icmp ult i32 %30, -7
  br i1 %80, label %81, label %84

81:                                               ; preds = %78
  %82 = sext i32 %30 to i64
  %83 = getelementptr inbounds i8, i8* %11, i64 %82
  br label %87

84:                                               ; preds = %78, %76
  %85 = phi i32 [ %30, %76 ], [ %79, %78 ]
  %86 = getelementptr inbounds i8, i8* %32, i64 8
  br label %87

87:                                               ; preds = %84, %81
  %88 = phi i32 [ %85, %84 ], [ %79, %81 ]
  %89 = phi i8* [ %86, %84 ], [ %32, %81 ]
  %90 = phi i8* [ %32, %84 ], [ %83, %81 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 8
  %93 = trunc i32 %92 to i16
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %95 = bitcast %union.jvalue* %94 to i16*
  store i16 %93, i16* %95, align 8, !tbaa !23
  br label %211

96:                                               ; preds = %29
  %97 = icmp sgt i32 %30, -1
  br i1 %97, label %104, label %98

98:                                               ; preds = %96
  %99 = add nsw i32 %30, 8
  %100 = icmp ult i32 %30, -7
  br i1 %100, label %101, label %104

101:                                              ; preds = %98
  %102 = sext i32 %30 to i64
  %103 = getelementptr inbounds i8, i8* %11, i64 %102
  br label %107

104:                                              ; preds = %98, %96
  %105 = phi i32 [ %30, %96 ], [ %99, %98 ]
  %106 = getelementptr inbounds i8, i8* %32, i64 8
  br label %107

107:                                              ; preds = %104, %101
  %108 = phi i32 [ %105, %104 ], [ %99, %101 ]
  %109 = phi i8* [ %106, %104 ], [ %32, %101 ]
  %110 = phi i8* [ %32, %104 ], [ %103, %101 ]
  %111 = bitcast i8* %110 to i32*
  %112 = load i32, i32* %111, align 8
  %113 = and i32 %112, 65535
  %114 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %115 = bitcast %union.jvalue* %114 to i32*
  store i32 %113, i32* %115, align 8, !tbaa !23
  br label %211

116:                                              ; preds = %29
  %117 = icmp sgt i32 %30, -1
  br i1 %117, label %124, label %118

118:                                              ; preds = %116
  %119 = add nsw i32 %30, 8
  %120 = icmp ult i32 %30, -7
  br i1 %120, label %121, label %124

121:                                              ; preds = %118
  %122 = sext i32 %30 to i64
  %123 = getelementptr inbounds i8, i8* %11, i64 %122
  br label %127

124:                                              ; preds = %118, %116
  %125 = phi i32 [ %30, %116 ], [ %119, %118 ]
  %126 = getelementptr inbounds i8, i8* %32, i64 8
  br label %127

127:                                              ; preds = %124, %121
  %128 = phi i32 [ %125, %124 ], [ %119, %121 ]
  %129 = phi i8* [ %126, %124 ], [ %32, %121 ]
  %130 = phi i8* [ %32, %124 ], [ %123, %121 ]
  %131 = bitcast i8* %130 to i32*
  %132 = load i32, i32* %131, align 8
  %133 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %134 = bitcast %union.jvalue* %133 to i32*
  store i32 %132, i32* %134, align 8, !tbaa !23
  br label %211

135:                                              ; preds = %29
  %136 = icmp sgt i32 %30, -1
  br i1 %136, label %143, label %137

137:                                              ; preds = %135
  %138 = add nsw i32 %30, 8
  %139 = icmp ult i32 %30, -7
  br i1 %139, label %140, label %143

140:                                              ; preds = %137
  %141 = sext i32 %30 to i64
  %142 = getelementptr inbounds i8, i8* %11, i64 %141
  br label %146

143:                                              ; preds = %137, %135
  %144 = phi i32 [ %30, %135 ], [ %138, %137 ]
  %145 = getelementptr inbounds i8, i8* %32, i64 8
  br label %146

146:                                              ; preds = %143, %140
  %147 = phi i32 [ %144, %143 ], [ %138, %140 ]
  %148 = phi i8* [ %145, %143 ], [ %32, %140 ]
  %149 = phi i8* [ %32, %143 ], [ %142, %140 ]
  %150 = bitcast i8* %149 to i64*
  %151 = load i64, i64* %150, align 8
  %152 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33, i32 0
  store i64 %151, i64* %152, align 8, !tbaa !23
  br label %211

153:                                              ; preds = %29
  %154 = icmp sgt i32 %31, -1
  br i1 %154, label %161, label %155

155:                                              ; preds = %153
  %156 = add nsw i32 %31, 16
  %157 = icmp ult i32 %31, -15
  br i1 %157, label %158, label %161

158:                                              ; preds = %155
  %159 = sext i32 %31 to i64
  %160 = getelementptr inbounds i8, i8* %13, i64 %159
  br label %164

161:                                              ; preds = %155, %153
  %162 = phi i32 [ %31, %153 ], [ %156, %155 ]
  %163 = getelementptr inbounds i8, i8* %32, i64 8
  br label %164

164:                                              ; preds = %161, %158
  %165 = phi i32 [ %162, %161 ], [ %156, %158 ]
  %166 = phi i8* [ %163, %161 ], [ %32, %158 ]
  %167 = phi i8* [ %32, %161 ], [ %160, %158 ]
  %168 = bitcast i8* %167 to double*
  %169 = load double, double* %168, align 8
  %170 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %171 = bitcast %union.jvalue* %170 to double*
  store double %169, double* %171, align 8, !tbaa !23
  br label %211

172:                                              ; preds = %29
  %173 = icmp sgt i32 %31, -1
  br i1 %173, label %180, label %174

174:                                              ; preds = %172
  %175 = add nsw i32 %31, 16
  %176 = icmp ult i32 %31, -15
  br i1 %176, label %177, label %180

177:                                              ; preds = %174
  %178 = sext i32 %31 to i64
  %179 = getelementptr inbounds i8, i8* %13, i64 %178
  br label %183

180:                                              ; preds = %174, %172
  %181 = phi i32 [ %31, %172 ], [ %175, %174 ]
  %182 = getelementptr inbounds i8, i8* %32, i64 8
  br label %183

183:                                              ; preds = %180, %177
  %184 = phi i32 [ %181, %180 ], [ %175, %177 ]
  %185 = phi i8* [ %182, %180 ], [ %32, %177 ]
  %186 = phi i8* [ %32, %180 ], [ %179, %177 ]
  %187 = bitcast i8* %186 to double*
  %188 = load double, double* %187, align 8
  %189 = fptrunc double %188 to float
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %191 = bitcast %union.jvalue* %190 to float*
  store float %189, float* %191, align 8, !tbaa !23
  br label %211

192:                                              ; preds = %29
  %193 = icmp sgt i32 %30, -1
  br i1 %193, label %200, label %194

194:                                              ; preds = %192
  %195 = add nsw i32 %30, 8
  %196 = icmp ult i32 %30, -7
  br i1 %196, label %197, label %200

197:                                              ; preds = %194
  %198 = sext i32 %30 to i64
  %199 = getelementptr inbounds i8, i8* %11, i64 %198
  br label %203

200:                                              ; preds = %194, %192
  %201 = phi i32 [ %30, %192 ], [ %195, %194 ]
  %202 = getelementptr inbounds i8, i8* %32, i64 8
  br label %203

203:                                              ; preds = %200, %197
  %204 = phi i32 [ %201, %200 ], [ %195, %197 ]
  %205 = phi i8* [ %202, %200 ], [ %32, %197 ]
  %206 = phi i8* [ %32, %200 ], [ %199, %197 ]
  %207 = bitcast i8* %206 to %struct._jobject**
  %208 = load %struct._jobject*, %struct._jobject** %207, align 8
  %209 = getelementptr inbounds %union.jvalue, %union.jvalue* %25, i64 %33
  %210 = bitcast %union.jvalue* %209 to %struct._jobject**
  store %struct._jobject* %208, %struct._jobject** %210, align 8, !tbaa !23
  br label %211

211:                                              ; preds = %203, %183, %164, %146, %127, %107, %87, %67, %47, %29
  %212 = phi i32 [ %30, %29 ], [ %204, %203 ], [ %30, %183 ], [ %30, %164 ], [ %147, %146 ], [ %128, %127 ], [ %108, %107 ], [ %88, %87 ], [ %68, %67 ], [ %48, %47 ]
  %213 = phi i32 [ %31, %29 ], [ %31, %203 ], [ %184, %183 ], [ %165, %164 ], [ %31, %146 ], [ %31, %127 ], [ %31, %107 ], [ %31, %87 ], [ %31, %67 ], [ %31, %47 ]
  %214 = phi i8* [ %32, %29 ], [ %205, %203 ], [ %185, %183 ], [ %166, %164 ], [ %148, %146 ], [ %129, %127 ], [ %109, %107 ], [ %89, %87 ], [ %69, %67 ], [ %49, %47 ]
  %215 = add nuw nsw i64 %33, 1
  %216 = icmp eq i64 %215, %28
  br i1 %216, label %217, label %29, !llvm.loop !83

217:                                              ; preds = %211, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %19) #4
  %218 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %219 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %218, i64 0, i32 93
  %220 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %219, align 8, !tbaa !84
  call void %220(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %25) #4
  call void @llvm.stackrestore(i8* %18)
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %7) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local void @JNI_CallNonvirtualVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !11
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !21
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #4
  %12 = sext i32 %11 to i64
  %13 = alloca %union.jvalue, i64 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %204

15:                                               ; preds = %5
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 3
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 1
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 0
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 4
  %20 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i64 0, i32 2
  %21 = zext i32 %11 to i64
  br label %22

22:                                               ; preds = %15, %201
  %23 = phi i64 [ 0, %15 ], [ %202, %201 ]
  %24 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 %23
  %25 = load i8, i8* %24, align 1, !tbaa !23
  switch i8 %25, label %201 [
    i8 90, label %26
    i8 66, label %46
    i8 83, label %66
    i8 67, label %86
    i8 73, label %106
    i8 74, label %125
    i8 68, label %143
    i8 70, label %162
    i8 76, label %182
  ]

26:                                               ; preds = %22
  %27 = load i32, i32* %16, align 8
  %28 = icmp sgt i32 %27, -1
  br i1 %28, label %36, label %29

29:                                               ; preds = %26
  %30 = add nsw i32 %27, 8
  store i32 %30, i32* %16, align 8
  %31 = icmp ult i32 %27, -7
  br i1 %31, label %32, label %36

32:                                               ; preds = %29
  %33 = load i8*, i8** %17, align 8
  %34 = sext i32 %27 to i64
  %35 = getelementptr inbounds i8, i8* %33, i64 %34
  br label %39

36:                                               ; preds = %29, %26
  %37 = load i8*, i8** %18, align 8
  %38 = getelementptr inbounds i8, i8* %37, i64 8
  store i8* %38, i8** %18, align 8
  br label %39

39:                                               ; preds = %36, %32
  %40 = phi i8* [ %35, %32 ], [ %37, %36 ]
  %41 = bitcast i8* %40 to i32*
  %42 = load i32, i32* %41, align 8
  %43 = trunc i32 %42 to i8
  %44 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %43, i8* %45, align 8, !tbaa !23
  br label %201

46:                                               ; preds = %22
  %47 = load i32, i32* %16, align 8
  %48 = icmp sgt i32 %47, -1
  br i1 %48, label %56, label %49

49:                                               ; preds = %46
  %50 = add nsw i32 %47, 8
  store i32 %50, i32* %16, align 8
  %51 = icmp ult i32 %47, -7
  br i1 %51, label %52, label %56

52:                                               ; preds = %49
  %53 = load i8*, i8** %17, align 8
  %54 = sext i32 %47 to i64
  %55 = getelementptr inbounds i8, i8* %53, i64 %54
  br label %59

56:                                               ; preds = %49, %46
  %57 = load i8*, i8** %18, align 8
  %58 = getelementptr inbounds i8, i8* %57, i64 8
  store i8* %58, i8** %18, align 8
  br label %59

59:                                               ; preds = %56, %52
  %60 = phi i8* [ %55, %52 ], [ %57, %56 ]
  %61 = bitcast i8* %60 to i32*
  %62 = load i32, i32* %61, align 8
  %63 = trunc i32 %62 to i8
  %64 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %65 = bitcast %union.jvalue* %64 to i8*
  store i8 %63, i8* %65, align 8, !tbaa !23
  br label %201

66:                                               ; preds = %22
  %67 = load i32, i32* %16, align 8
  %68 = icmp sgt i32 %67, -1
  br i1 %68, label %76, label %69

69:                                               ; preds = %66
  %70 = add nsw i32 %67, 8
  store i32 %70, i32* %16, align 8
  %71 = icmp ult i32 %67, -7
  br i1 %71, label %72, label %76

72:                                               ; preds = %69
  %73 = load i8*, i8** %17, align 8
  %74 = sext i32 %67 to i64
  %75 = getelementptr inbounds i8, i8* %73, i64 %74
  br label %79

76:                                               ; preds = %69, %66
  %77 = load i8*, i8** %18, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %18, align 8
  br label %79

79:                                               ; preds = %76, %72
  %80 = phi i8* [ %75, %72 ], [ %77, %76 ]
  %81 = bitcast i8* %80 to i32*
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i16
  %84 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %85 = bitcast %union.jvalue* %84 to i16*
  store i16 %83, i16* %85, align 8, !tbaa !23
  br label %201

86:                                               ; preds = %22
  %87 = load i32, i32* %16, align 8
  %88 = icmp sgt i32 %87, -1
  br i1 %88, label %96, label %89

89:                                               ; preds = %86
  %90 = add nsw i32 %87, 8
  store i32 %90, i32* %16, align 8
  %91 = icmp ult i32 %87, -7
  br i1 %91, label %92, label %96

92:                                               ; preds = %89
  %93 = load i8*, i8** %17, align 8
  %94 = sext i32 %87 to i64
  %95 = getelementptr inbounds i8, i8* %93, i64 %94
  br label %99

96:                                               ; preds = %89, %86
  %97 = load i8*, i8** %18, align 8
  %98 = getelementptr inbounds i8, i8* %97, i64 8
  store i8* %98, i8** %18, align 8
  br label %99

99:                                               ; preds = %96, %92
  %100 = phi i8* [ %95, %92 ], [ %97, %96 ]
  %101 = bitcast i8* %100 to i32*
  %102 = load i32, i32* %101, align 8
  %103 = and i32 %102, 65535
  %104 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %105 = bitcast %union.jvalue* %104 to i32*
  store i32 %103, i32* %105, align 8, !tbaa !23
  br label %201

106:                                              ; preds = %22
  %107 = load i32, i32* %16, align 8
  %108 = icmp sgt i32 %107, -1
  br i1 %108, label %116, label %109

109:                                              ; preds = %106
  %110 = add nsw i32 %107, 8
  store i32 %110, i32* %16, align 8
  %111 = icmp ult i32 %107, -7
  br i1 %111, label %112, label %116

112:                                              ; preds = %109
  %113 = load i8*, i8** %17, align 8
  %114 = sext i32 %107 to i64
  %115 = getelementptr inbounds i8, i8* %113, i64 %114
  br label %119

116:                                              ; preds = %109, %106
  %117 = load i8*, i8** %18, align 8
  %118 = getelementptr inbounds i8, i8* %117, i64 8
  store i8* %118, i8** %18, align 8
  br label %119

119:                                              ; preds = %116, %112
  %120 = phi i8* [ %115, %112 ], [ %117, %116 ]
  %121 = bitcast i8* %120 to i32*
  %122 = load i32, i32* %121, align 8
  %123 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %124 = bitcast %union.jvalue* %123 to i32*
  store i32 %122, i32* %124, align 8, !tbaa !23
  br label %201

125:                                              ; preds = %22
  %126 = load i32, i32* %16, align 8
  %127 = icmp sgt i32 %126, -1
  br i1 %127, label %135, label %128

128:                                              ; preds = %125
  %129 = add nsw i32 %126, 8
  store i32 %129, i32* %16, align 8
  %130 = icmp ult i32 %126, -7
  br i1 %130, label %131, label %135

131:                                              ; preds = %128
  %132 = load i8*, i8** %17, align 8
  %133 = sext i32 %126 to i64
  %134 = getelementptr inbounds i8, i8* %132, i64 %133
  br label %138

135:                                              ; preds = %128, %125
  %136 = load i8*, i8** %18, align 8
  %137 = getelementptr inbounds i8, i8* %136, i64 8
  store i8* %137, i8** %18, align 8
  br label %138

138:                                              ; preds = %135, %131
  %139 = phi i8* [ %134, %131 ], [ %136, %135 ]
  %140 = bitcast i8* %139 to i64*
  %141 = load i64, i64* %140, align 8
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23, i32 0
  store i64 %141, i64* %142, align 8, !tbaa !23
  br label %201

143:                                              ; preds = %22
  %144 = load i32, i32* %19, align 4
  %145 = icmp sgt i32 %144, -1
  br i1 %145, label %153, label %146

146:                                              ; preds = %143
  %147 = add nsw i32 %144, 16
  store i32 %147, i32* %19, align 4
  %148 = icmp ult i32 %144, -15
  br i1 %148, label %149, label %153

149:                                              ; preds = %146
  %150 = load i8*, i8** %20, align 8
  %151 = sext i32 %144 to i64
  %152 = getelementptr inbounds i8, i8* %150, i64 %151
  br label %156

153:                                              ; preds = %146, %143
  %154 = load i8*, i8** %18, align 8
  %155 = getelementptr inbounds i8, i8* %154, i64 8
  store i8* %155, i8** %18, align 8
  br label %156

156:                                              ; preds = %153, %149
  %157 = phi i8* [ %152, %149 ], [ %154, %153 ]
  %158 = bitcast i8* %157 to double*
  %159 = load double, double* %158, align 8
  %160 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %161 = bitcast %union.jvalue* %160 to double*
  store double %159, double* %161, align 8, !tbaa !23
  br label %201

162:                                              ; preds = %22
  %163 = load i32, i32* %19, align 4
  %164 = icmp sgt i32 %163, -1
  br i1 %164, label %172, label %165

165:                                              ; preds = %162
  %166 = add nsw i32 %163, 16
  store i32 %166, i32* %19, align 4
  %167 = icmp ult i32 %163, -15
  br i1 %167, label %168, label %172

168:                                              ; preds = %165
  %169 = load i8*, i8** %20, align 8
  %170 = sext i32 %163 to i64
  %171 = getelementptr inbounds i8, i8* %169, i64 %170
  br label %175

172:                                              ; preds = %165, %162
  %173 = load i8*, i8** %18, align 8
  %174 = getelementptr inbounds i8, i8* %173, i64 8
  store i8* %174, i8** %18, align 8
  br label %175

175:                                              ; preds = %172, %168
  %176 = phi i8* [ %171, %168 ], [ %173, %172 ]
  %177 = bitcast i8* %176 to double*
  %178 = load double, double* %177, align 8
  %179 = fptrunc double %178 to float
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %181 = bitcast %union.jvalue* %180 to float*
  store float %179, float* %181, align 8, !tbaa !23
  br label %201

182:                                              ; preds = %22
  %183 = load i32, i32* %16, align 8
  %184 = icmp sgt i32 %183, -1
  br i1 %184, label %192, label %185

185:                                              ; preds = %182
  %186 = add nsw i32 %183, 8
  store i32 %186, i32* %16, align 8
  %187 = icmp ult i32 %183, -7
  br i1 %187, label %188, label %192

188:                                              ; preds = %185
  %189 = load i8*, i8** %17, align 8
  %190 = sext i32 %183 to i64
  %191 = getelementptr inbounds i8, i8* %189, i64 %190
  br label %195

192:                                              ; preds = %185, %182
  %193 = load i8*, i8** %18, align 8
  %194 = getelementptr inbounds i8, i8* %193, i64 8
  store i8* %194, i8** %18, align 8
  br label %195

195:                                              ; preds = %192, %188
  %196 = phi i8* [ %191, %188 ], [ %193, %192 ]
  %197 = bitcast i8* %196 to %struct._jobject**
  %198 = load %struct._jobject*, %struct._jobject** %197, align 8
  %199 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %23
  %200 = bitcast %union.jvalue* %199 to %struct._jobject**
  store %struct._jobject* %198, %struct._jobject** %200, align 8, !tbaa !23
  br label %201

201:                                              ; preds = %39, %59, %79, %99, %119, %138, %156, %175, %195, %22
  %202 = add nuw nsw i64 %23, 1
  %203 = icmp eq i64 %202, %21
  br i1 %203, label %204, label %22, !llvm.loop !83

204:                                              ; preds = %201, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %205 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %206 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %205, i64 0, i32 93
  %207 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %206, align 8, !tbaa !84
  call void %207(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local void @JNI_CallStaticVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 0
  %8 = load i8*, i8** %7, align 8, !tbaa.struct !10
  %9 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 1
  %10 = load i8*, i8** %9, align 8, !tbaa.struct !17
  %11 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 2
  %12 = load i8*, i8** %11, align 8, !tbaa.struct !18
  %13 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 3
  %14 = load i32, i32* %13, align 8, !tbaa.struct !19
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %5, i64 0, i32 4
  %16 = load i32, i32* %15, align 4, !tbaa.struct !20
  %17 = call i8* @llvm.stacksave()
  %18 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %18) #4
  %19 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %20 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %19, align 8, !tbaa !11
  %21 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %20, align 8, !tbaa !21
  %22 = call i32 %21(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %18) #4
  %23 = sext i32 %22 to i64
  %24 = alloca %union.jvalue, i64 %23, align 16
  %25 = icmp sgt i32 %22, 0
  br i1 %25, label %26, label %216

26:                                               ; preds = %3
  %27 = zext i32 %22 to i64
  br label %28

28:                                               ; preds = %210, %26
  %29 = phi i32 [ %14, %26 ], [ %211, %210 ]
  %30 = phi i32 [ %16, %26 ], [ %212, %210 ]
  %31 = phi i8* [ %8, %26 ], [ %213, %210 ]
  %32 = phi i64 [ 0, %26 ], [ %214, %210 ]
  %33 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %32
  %34 = load i8, i8* %33, align 1, !tbaa !23
  switch i8 %34, label %210 [
    i8 90, label %35
    i8 66, label %55
    i8 83, label %75
    i8 67, label %95
    i8 73, label %115
    i8 74, label %134
    i8 68, label %152
    i8 70, label %171
    i8 76, label %191
  ]

35:                                               ; preds = %28
  %36 = icmp sgt i32 %29, -1
  br i1 %36, label %43, label %37

37:                                               ; preds = %35
  %38 = add nsw i32 %29, 8
  %39 = icmp ult i32 %29, -7
  br i1 %39, label %40, label %43

40:                                               ; preds = %37
  %41 = sext i32 %29 to i64
  %42 = getelementptr inbounds i8, i8* %10, i64 %41
  br label %46

43:                                               ; preds = %37, %35
  %44 = phi i32 [ %29, %35 ], [ %38, %37 ]
  %45 = getelementptr inbounds i8, i8* %31, i64 8
  br label %46

46:                                               ; preds = %43, %40
  %47 = phi i32 [ %44, %43 ], [ %38, %40 ]
  %48 = phi i8* [ %45, %43 ], [ %31, %40 ]
  %49 = phi i8* [ %31, %43 ], [ %42, %40 ]
  %50 = bitcast i8* %49 to i32*
  %51 = load i32, i32* %50, align 8
  %52 = trunc i32 %51 to i8
  %53 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %54 = bitcast %union.jvalue* %53 to i8*
  store i8 %52, i8* %54, align 8, !tbaa !23
  br label %210

55:                                               ; preds = %28
  %56 = icmp sgt i32 %29, -1
  br i1 %56, label %63, label %57

57:                                               ; preds = %55
  %58 = add nsw i32 %29, 8
  %59 = icmp ult i32 %29, -7
  br i1 %59, label %60, label %63

60:                                               ; preds = %57
  %61 = sext i32 %29 to i64
  %62 = getelementptr inbounds i8, i8* %10, i64 %61
  br label %66

63:                                               ; preds = %57, %55
  %64 = phi i32 [ %29, %55 ], [ %58, %57 ]
  %65 = getelementptr inbounds i8, i8* %31, i64 8
  br label %66

66:                                               ; preds = %63, %60
  %67 = phi i32 [ %64, %63 ], [ %58, %60 ]
  %68 = phi i8* [ %65, %63 ], [ %31, %60 ]
  %69 = phi i8* [ %31, %63 ], [ %62, %60 ]
  %70 = bitcast i8* %69 to i32*
  %71 = load i32, i32* %70, align 8
  %72 = trunc i32 %71 to i8
  %73 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %74 = bitcast %union.jvalue* %73 to i8*
  store i8 %72, i8* %74, align 8, !tbaa !23
  br label %210

75:                                               ; preds = %28
  %76 = icmp sgt i32 %29, -1
  br i1 %76, label %83, label %77

77:                                               ; preds = %75
  %78 = add nsw i32 %29, 8
  %79 = icmp ult i32 %29, -7
  br i1 %79, label %80, label %83

80:                                               ; preds = %77
  %81 = sext i32 %29 to i64
  %82 = getelementptr inbounds i8, i8* %10, i64 %81
  br label %86

83:                                               ; preds = %77, %75
  %84 = phi i32 [ %29, %75 ], [ %78, %77 ]
  %85 = getelementptr inbounds i8, i8* %31, i64 8
  br label %86

86:                                               ; preds = %83, %80
  %87 = phi i32 [ %84, %83 ], [ %78, %80 ]
  %88 = phi i8* [ %85, %83 ], [ %31, %80 ]
  %89 = phi i8* [ %31, %83 ], [ %82, %80 ]
  %90 = bitcast i8* %89 to i32*
  %91 = load i32, i32* %90, align 8
  %92 = trunc i32 %91 to i16
  %93 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %94 = bitcast %union.jvalue* %93 to i16*
  store i16 %92, i16* %94, align 8, !tbaa !23
  br label %210

95:                                               ; preds = %28
  %96 = icmp sgt i32 %29, -1
  br i1 %96, label %103, label %97

97:                                               ; preds = %95
  %98 = add nsw i32 %29, 8
  %99 = icmp ult i32 %29, -7
  br i1 %99, label %100, label %103

100:                                              ; preds = %97
  %101 = sext i32 %29 to i64
  %102 = getelementptr inbounds i8, i8* %10, i64 %101
  br label %106

103:                                              ; preds = %97, %95
  %104 = phi i32 [ %29, %95 ], [ %98, %97 ]
  %105 = getelementptr inbounds i8, i8* %31, i64 8
  br label %106

106:                                              ; preds = %103, %100
  %107 = phi i32 [ %104, %103 ], [ %98, %100 ]
  %108 = phi i8* [ %105, %103 ], [ %31, %100 ]
  %109 = phi i8* [ %31, %103 ], [ %102, %100 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 8
  %112 = and i32 %111, 65535
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !23
  br label %210

115:                                              ; preds = %28
  %116 = icmp sgt i32 %29, -1
  br i1 %116, label %123, label %117

117:                                              ; preds = %115
  %118 = add nsw i32 %29, 8
  %119 = icmp ult i32 %29, -7
  br i1 %119, label %120, label %123

120:                                              ; preds = %117
  %121 = sext i32 %29 to i64
  %122 = getelementptr inbounds i8, i8* %10, i64 %121
  br label %126

123:                                              ; preds = %117, %115
  %124 = phi i32 [ %29, %115 ], [ %118, %117 ]
  %125 = getelementptr inbounds i8, i8* %31, i64 8
  br label %126

126:                                              ; preds = %123, %120
  %127 = phi i32 [ %124, %123 ], [ %118, %120 ]
  %128 = phi i8* [ %125, %123 ], [ %31, %120 ]
  %129 = phi i8* [ %31, %123 ], [ %122, %120 ]
  %130 = bitcast i8* %129 to i32*
  %131 = load i32, i32* %130, align 8
  %132 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %133 = bitcast %union.jvalue* %132 to i32*
  store i32 %131, i32* %133, align 8, !tbaa !23
  br label %210

134:                                              ; preds = %28
  %135 = icmp sgt i32 %29, -1
  br i1 %135, label %142, label %136

136:                                              ; preds = %134
  %137 = add nsw i32 %29, 8
  %138 = icmp ult i32 %29, -7
  br i1 %138, label %139, label %142

139:                                              ; preds = %136
  %140 = sext i32 %29 to i64
  %141 = getelementptr inbounds i8, i8* %10, i64 %140
  br label %145

142:                                              ; preds = %136, %134
  %143 = phi i32 [ %29, %134 ], [ %137, %136 ]
  %144 = getelementptr inbounds i8, i8* %31, i64 8
  br label %145

145:                                              ; preds = %142, %139
  %146 = phi i32 [ %143, %142 ], [ %137, %139 ]
  %147 = phi i8* [ %144, %142 ], [ %31, %139 ]
  %148 = phi i8* [ %31, %142 ], [ %141, %139 ]
  %149 = bitcast i8* %148 to i64*
  %150 = load i64, i64* %149, align 8
  %151 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32, i32 0
  store i64 %150, i64* %151, align 8, !tbaa !23
  br label %210

152:                                              ; preds = %28
  %153 = icmp sgt i32 %30, -1
  br i1 %153, label %160, label %154

154:                                              ; preds = %152
  %155 = add nsw i32 %30, 16
  %156 = icmp ult i32 %30, -15
  br i1 %156, label %157, label %160

157:                                              ; preds = %154
  %158 = sext i32 %30 to i64
  %159 = getelementptr inbounds i8, i8* %12, i64 %158
  br label %163

160:                                              ; preds = %154, %152
  %161 = phi i32 [ %30, %152 ], [ %155, %154 ]
  %162 = getelementptr inbounds i8, i8* %31, i64 8
  br label %163

163:                                              ; preds = %160, %157
  %164 = phi i32 [ %161, %160 ], [ %155, %157 ]
  %165 = phi i8* [ %162, %160 ], [ %31, %157 ]
  %166 = phi i8* [ %31, %160 ], [ %159, %157 ]
  %167 = bitcast i8* %166 to double*
  %168 = load double, double* %167, align 8
  %169 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %170 = bitcast %union.jvalue* %169 to double*
  store double %168, double* %170, align 8, !tbaa !23
  br label %210

171:                                              ; preds = %28
  %172 = icmp sgt i32 %30, -1
  br i1 %172, label %179, label %173

173:                                              ; preds = %171
  %174 = add nsw i32 %30, 16
  %175 = icmp ult i32 %30, -15
  br i1 %175, label %176, label %179

176:                                              ; preds = %173
  %177 = sext i32 %30 to i64
  %178 = getelementptr inbounds i8, i8* %12, i64 %177
  br label %182

179:                                              ; preds = %173, %171
  %180 = phi i32 [ %30, %171 ], [ %174, %173 ]
  %181 = getelementptr inbounds i8, i8* %31, i64 8
  br label %182

182:                                              ; preds = %179, %176
  %183 = phi i32 [ %180, %179 ], [ %174, %176 ]
  %184 = phi i8* [ %181, %179 ], [ %31, %176 ]
  %185 = phi i8* [ %31, %179 ], [ %178, %176 ]
  %186 = bitcast i8* %185 to double*
  %187 = load double, double* %186, align 8
  %188 = fptrunc double %187 to float
  %189 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %190 = bitcast %union.jvalue* %189 to float*
  store float %188, float* %190, align 8, !tbaa !23
  br label %210

191:                                              ; preds = %28
  %192 = icmp sgt i32 %29, -1
  br i1 %192, label %199, label %193

193:                                              ; preds = %191
  %194 = add nsw i32 %29, 8
  %195 = icmp ult i32 %29, -7
  br i1 %195, label %196, label %199

196:                                              ; preds = %193
  %197 = sext i32 %29 to i64
  %198 = getelementptr inbounds i8, i8* %10, i64 %197
  br label %202

199:                                              ; preds = %193, %191
  %200 = phi i32 [ %29, %191 ], [ %194, %193 ]
  %201 = getelementptr inbounds i8, i8* %31, i64 8
  br label %202

202:                                              ; preds = %199, %196
  %203 = phi i32 [ %200, %199 ], [ %194, %196 ]
  %204 = phi i8* [ %201, %199 ], [ %31, %196 ]
  %205 = phi i8* [ %31, %199 ], [ %198, %196 ]
  %206 = bitcast i8* %205 to %struct._jobject**
  %207 = load %struct._jobject*, %struct._jobject** %206, align 8
  %208 = getelementptr inbounds %union.jvalue, %union.jvalue* %24, i64 %32
  %209 = bitcast %union.jvalue* %208 to %struct._jobject**
  store %struct._jobject* %207, %struct._jobject** %209, align 8, !tbaa !23
  br label %210

210:                                              ; preds = %202, %182, %163, %145, %126, %106, %86, %66, %46, %28
  %211 = phi i32 [ %29, %28 ], [ %203, %202 ], [ %29, %182 ], [ %29, %163 ], [ %146, %145 ], [ %127, %126 ], [ %107, %106 ], [ %87, %86 ], [ %67, %66 ], [ %47, %46 ]
  %212 = phi i32 [ %30, %28 ], [ %30, %202 ], [ %183, %182 ], [ %164, %163 ], [ %30, %145 ], [ %30, %126 ], [ %30, %106 ], [ %30, %86 ], [ %30, %66 ], [ %30, %46 ]
  %213 = phi i8* [ %31, %28 ], [ %204, %202 ], [ %184, %182 ], [ %165, %163 ], [ %147, %145 ], [ %128, %126 ], [ %108, %106 ], [ %88, %86 ], [ %68, %66 ], [ %48, %46 ]
  %214 = add nuw nsw i64 %32, 1
  %215 = icmp eq i64 %214, %27
  br i1 %215, label %216, label %28, !llvm.loop !85

216:                                              ; preds = %210, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %18) #4
  %217 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %218 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %217, i64 0, i32 143
  %219 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %218, align 8, !tbaa !86
  call void %219(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %24) #4
  call void @llvm.stackrestore(i8* %17)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local void @JNI_CallStaticVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !11
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !21
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %203

14:                                               ; preds = %4
  %15 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 3
  %16 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 1
  %17 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 0
  %18 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 4
  %19 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i64 0, i32 2
  %20 = zext i32 %10 to i64
  br label %21

21:                                               ; preds = %14, %200
  %22 = phi i64 [ 0, %14 ], [ %201, %200 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !23
  switch i8 %24, label %200 [
    i8 90, label %25
    i8 66, label %45
    i8 83, label %65
    i8 67, label %85
    i8 73, label %105
    i8 74, label %124
    i8 68, label %142
    i8 70, label %161
    i8 76, label %181
  ]

25:                                               ; preds = %21
  %26 = load i32, i32* %15, align 8
  %27 = icmp sgt i32 %26, -1
  br i1 %27, label %35, label %28

28:                                               ; preds = %25
  %29 = add nsw i32 %26, 8
  store i32 %29, i32* %15, align 8
  %30 = icmp ult i32 %26, -7
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = load i8*, i8** %16, align 8
  %33 = sext i32 %26 to i64
  %34 = getelementptr inbounds i8, i8* %32, i64 %33
  br label %38

35:                                               ; preds = %28, %25
  %36 = load i8*, i8** %17, align 8
  %37 = getelementptr inbounds i8, i8* %36, i64 8
  store i8* %37, i8** %17, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %34, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 8
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !23
  br label %200

45:                                               ; preds = %21
  %46 = load i32, i32* %15, align 8
  %47 = icmp sgt i32 %46, -1
  br i1 %47, label %55, label %48

48:                                               ; preds = %45
  %49 = add nsw i32 %46, 8
  store i32 %49, i32* %15, align 8
  %50 = icmp ult i32 %46, -7
  br i1 %50, label %51, label %55

51:                                               ; preds = %48
  %52 = load i8*, i8** %16, align 8
  %53 = sext i32 %46 to i64
  %54 = getelementptr inbounds i8, i8* %52, i64 %53
  br label %58

55:                                               ; preds = %48, %45
  %56 = load i8*, i8** %17, align 8
  %57 = getelementptr inbounds i8, i8* %56, i64 8
  store i8* %57, i8** %17, align 8
  br label %58

58:                                               ; preds = %55, %51
  %59 = phi i8* [ %54, %51 ], [ %56, %55 ]
  %60 = bitcast i8* %59 to i32*
  %61 = load i32, i32* %60, align 8
  %62 = trunc i32 %61 to i8
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %62, i8* %64, align 8, !tbaa !23
  br label %200

65:                                               ; preds = %21
  %66 = load i32, i32* %15, align 8
  %67 = icmp sgt i32 %66, -1
  br i1 %67, label %75, label %68

68:                                               ; preds = %65
  %69 = add nsw i32 %66, 8
  store i32 %69, i32* %15, align 8
  %70 = icmp ult i32 %66, -7
  br i1 %70, label %71, label %75

71:                                               ; preds = %68
  %72 = load i8*, i8** %16, align 8
  %73 = sext i32 %66 to i64
  %74 = getelementptr inbounds i8, i8* %72, i64 %73
  br label %78

75:                                               ; preds = %68, %65
  %76 = load i8*, i8** %17, align 8
  %77 = getelementptr inbounds i8, i8* %76, i64 8
  store i8* %77, i8** %17, align 8
  br label %78

78:                                               ; preds = %75, %71
  %79 = phi i8* [ %74, %71 ], [ %76, %75 ]
  %80 = bitcast i8* %79 to i32*
  %81 = load i32, i32* %80, align 8
  %82 = trunc i32 %81 to i16
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %84 = bitcast %union.jvalue* %83 to i16*
  store i16 %82, i16* %84, align 8, !tbaa !23
  br label %200

85:                                               ; preds = %21
  %86 = load i32, i32* %15, align 8
  %87 = icmp sgt i32 %86, -1
  br i1 %87, label %95, label %88

88:                                               ; preds = %85
  %89 = add nsw i32 %86, 8
  store i32 %89, i32* %15, align 8
  %90 = icmp ult i32 %86, -7
  br i1 %90, label %91, label %95

91:                                               ; preds = %88
  %92 = load i8*, i8** %16, align 8
  %93 = sext i32 %86 to i64
  %94 = getelementptr inbounds i8, i8* %92, i64 %93
  br label %98

95:                                               ; preds = %88, %85
  %96 = load i8*, i8** %17, align 8
  %97 = getelementptr inbounds i8, i8* %96, i64 8
  store i8* %97, i8** %17, align 8
  br label %98

98:                                               ; preds = %95, %91
  %99 = phi i8* [ %94, %91 ], [ %96, %95 ]
  %100 = bitcast i8* %99 to i32*
  %101 = load i32, i32* %100, align 8
  %102 = and i32 %101, 65535
  %103 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %104 = bitcast %union.jvalue* %103 to i32*
  store i32 %102, i32* %104, align 8, !tbaa !23
  br label %200

105:                                              ; preds = %21
  %106 = load i32, i32* %15, align 8
  %107 = icmp sgt i32 %106, -1
  br i1 %107, label %115, label %108

108:                                              ; preds = %105
  %109 = add nsw i32 %106, 8
  store i32 %109, i32* %15, align 8
  %110 = icmp ult i32 %106, -7
  br i1 %110, label %111, label %115

111:                                              ; preds = %108
  %112 = load i8*, i8** %16, align 8
  %113 = sext i32 %106 to i64
  %114 = getelementptr inbounds i8, i8* %112, i64 %113
  br label %118

115:                                              ; preds = %108, %105
  %116 = load i8*, i8** %17, align 8
  %117 = getelementptr inbounds i8, i8* %116, i64 8
  store i8* %117, i8** %17, align 8
  br label %118

118:                                              ; preds = %115, %111
  %119 = phi i8* [ %114, %111 ], [ %116, %115 ]
  %120 = bitcast i8* %119 to i32*
  %121 = load i32, i32* %120, align 8
  %122 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %123 = bitcast %union.jvalue* %122 to i32*
  store i32 %121, i32* %123, align 8, !tbaa !23
  br label %200

124:                                              ; preds = %21
  %125 = load i32, i32* %15, align 8
  %126 = icmp sgt i32 %125, -1
  br i1 %126, label %134, label %127

127:                                              ; preds = %124
  %128 = add nsw i32 %125, 8
  store i32 %128, i32* %15, align 8
  %129 = icmp ult i32 %125, -7
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = load i8*, i8** %16, align 8
  %132 = sext i32 %125 to i64
  %133 = getelementptr inbounds i8, i8* %131, i64 %132
  br label %137

134:                                              ; preds = %127, %124
  %135 = load i8*, i8** %17, align 8
  %136 = getelementptr inbounds i8, i8* %135, i64 8
  store i8* %136, i8** %17, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %133, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to i64*
  %140 = load i64, i64* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22, i32 0
  store i64 %140, i64* %141, align 8, !tbaa !23
  br label %200

142:                                              ; preds = %21
  %143 = load i32, i32* %18, align 4
  %144 = icmp sgt i32 %143, -1
  br i1 %144, label %152, label %145

145:                                              ; preds = %142
  %146 = add nsw i32 %143, 16
  store i32 %146, i32* %18, align 4
  %147 = icmp ult i32 %143, -15
  br i1 %147, label %148, label %152

148:                                              ; preds = %145
  %149 = load i8*, i8** %19, align 8
  %150 = sext i32 %143 to i64
  %151 = getelementptr inbounds i8, i8* %149, i64 %150
  br label %155

152:                                              ; preds = %145, %142
  %153 = load i8*, i8** %17, align 8
  %154 = getelementptr inbounds i8, i8* %153, i64 8
  store i8* %154, i8** %17, align 8
  br label %155

155:                                              ; preds = %152, %148
  %156 = phi i8* [ %151, %148 ], [ %153, %152 ]
  %157 = bitcast i8* %156 to double*
  %158 = load double, double* %157, align 8
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %160 = bitcast %union.jvalue* %159 to double*
  store double %158, double* %160, align 8, !tbaa !23
  br label %200

161:                                              ; preds = %21
  %162 = load i32, i32* %18, align 4
  %163 = icmp sgt i32 %162, -1
  br i1 %163, label %171, label %164

164:                                              ; preds = %161
  %165 = add nsw i32 %162, 16
  store i32 %165, i32* %18, align 4
  %166 = icmp ult i32 %162, -15
  br i1 %166, label %167, label %171

167:                                              ; preds = %164
  %168 = load i8*, i8** %19, align 8
  %169 = sext i32 %162 to i64
  %170 = getelementptr inbounds i8, i8* %168, i64 %169
  br label %174

171:                                              ; preds = %164, %161
  %172 = load i8*, i8** %17, align 8
  %173 = getelementptr inbounds i8, i8* %172, i64 8
  store i8* %173, i8** %17, align 8
  br label %174

174:                                              ; preds = %171, %167
  %175 = phi i8* [ %170, %167 ], [ %172, %171 ]
  %176 = bitcast i8* %175 to double*
  %177 = load double, double* %176, align 8
  %178 = fptrunc double %177 to float
  %179 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %180 = bitcast %union.jvalue* %179 to float*
  store float %178, float* %180, align 8, !tbaa !23
  br label %200

181:                                              ; preds = %21
  %182 = load i32, i32* %15, align 8
  %183 = icmp sgt i32 %182, -1
  br i1 %183, label %191, label %184

184:                                              ; preds = %181
  %185 = add nsw i32 %182, 8
  store i32 %185, i32* %15, align 8
  %186 = icmp ult i32 %182, -7
  br i1 %186, label %187, label %191

187:                                              ; preds = %184
  %188 = load i8*, i8** %16, align 8
  %189 = sext i32 %182 to i64
  %190 = getelementptr inbounds i8, i8* %188, i64 %189
  br label %194

191:                                              ; preds = %184, %181
  %192 = load i8*, i8** %17, align 8
  %193 = getelementptr inbounds i8, i8* %192, i64 8
  store i8* %193, i8** %17, align 8
  br label %194

194:                                              ; preds = %191, %187
  %195 = phi i8* [ %190, %187 ], [ %192, %191 ]
  %196 = bitcast i8* %195 to %struct._jobject**
  %197 = load %struct._jobject*, %struct._jobject** %196, align 8
  %198 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %22
  %199 = bitcast %union.jvalue* %198 to %struct._jobject**
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !23
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !85

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !11
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 143
  %206 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !86
  call void %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret void
}

; Function Attrs: nofree nosync nounwind willreturn
declare i8* @llvm.stacksave() #3

; Function Attrs: nofree nosync nounwind willreturn
declare void @llvm.stackrestore(i8*) #3

attributes #0 = { alwaysinline nounwind uwtable "frame-pointer"="non-leaf" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+neon,+outline-atomics,+v8a" }
attributes #1 = { argmemonly mustprogress nofree nosync nounwind willreturn }
attributes #2 = { mustprogress nofree nosync nounwind willreturn }
attributes #3 = { nofree nosync nounwind willreturn }
attributes #4 = { nounwind }

!llvm.module.flags = !{!0, !1, !2, !3, !4, !5, !6, !7, !8}
!llvm.ident = !{!9}

!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 1, !"branch-target-enforcement", i32 0}
!2 = !{i32 1, !"sign-return-address", i32 0}
!3 = !{i32 1, !"sign-return-address-all", i32 0}
!4 = !{i32 1, !"sign-return-address-with-bkey", i32 0}
!5 = !{i32 7, !"PIC Level", i32 2}
!6 = !{i32 7, !"PIE Level", i32 2}
!7 = !{i32 7, !"uwtable", i32 1}
!8 = !{i32 7, !"frame-pointer", i32 1}
!9 = !{!"Ubuntu clang version 14.0.0-1ubuntu1"}
!10 = !{i64 0, i64 8, !11, i64 8, i64 8, !11, i64 16, i64 8, !11, i64 24, i64 4, !15, i64 28, i64 4, !15}
!11 = !{!12, !12, i64 0}
!12 = !{!"any pointer", !13, i64 0}
!13 = !{!"omnipotent char", !14, i64 0}
!14 = !{!"Simple C/C++ TBAA"}
!15 = !{!16, !16, i64 0}
!16 = !{!"int", !13, i64 0}
!17 = !{i64 0, i64 8, !11, i64 8, i64 8, !11, i64 16, i64 4, !15, i64 20, i64 4, !15}
!18 = !{i64 0, i64 8, !11, i64 8, i64 4, !15, i64 12, i64 4, !15}
!19 = !{i64 0, i64 4, !15, i64 4, i64 4, !15}
!20 = !{i64 0, i64 4, !15}
!21 = !{!22, !12, i64 0}
!22 = !{!"JNINativeInterface_", !12, i64 0, !12, i64 8, !12, i64 16, !12, i64 24, !12, i64 32, !12, i64 40, !12, i64 48, !12, i64 56, !12, i64 64, !12, i64 72, !12, i64 80, !12, i64 88, !12, i64 96, !12, i64 104, !12, i64 112, !12, i64 120, !12, i64 128, !12, i64 136, !12, i64 144, !12, i64 152, !12, i64 160, !12, i64 168, !12, i64 176, !12, i64 184, !12, i64 192, !12, i64 200, !12, i64 208, !12, i64 216, !12, i64 224, !12, i64 232, !12, i64 240, !12, i64 248, !12, i64 256, !12, i64 264, !12, i64 272, !12, i64 280, !12, i64 288, !12, i64 296, !12, i64 304, !12, i64 312, !12, i64 320, !12, i64 328, !12, i64 336, !12, i64 344, !12, i64 352, !12, i64 360, !12, i64 368, !12, i64 376, !12, i64 384, !12, i64 392, !12, i64 400, !12, i64 408, !12, i64 416, !12, i64 424, !12, i64 432, !12, i64 440, !12, i64 448, !12, i64 456, !12, i64 464, !12, i64 472, !12, i64 480, !12, i64 488, !12, i64 496, !12, i64 504, !12, i64 512, !12, i64 520, !12, i64 528, !12, i64 536, !12, i64 544, !12, i64 552, !12, i64 560, !12, i64 568, !12, i64 576, !12, i64 584, !12, i64 592, !12, i64 600, !12, i64 608, !12, i64 616, !12, i64 624, !12, i64 632, !12, i64 640, !12, i64 648, !12, i64 656, !12, i64 664, !12, i64 672, !12, i64 680, !12, i64 688, !12, i64 696, !12, i64 704, !12, i64 712, !12, i64 720, !12, i64 728, !12, i64 736, !12, i64 744, !12, i64 752, !12, i64 760, !12, i64 768, !12, i64 776, !12, i64 784, !12, i64 792, !12, i64 800, !12, i64 808, !12, i64 816, !12, i64 824, !12, i64 832, !12, i64 840, !12, i64 848, !12, i64 856, !12, i64 864, !12, i64 872, !12, i64 880, !12, i64 888, !12, i64 896, !12, i64 904, !12, i64 912, !12, i64 920, !12, i64 928, !12, i64 936, !12, i64 944, !12, i64 952, !12, i64 960, !12, i64 968, !12, i64 976, !12, i64 984, !12, i64 992, !12, i64 1000, !12, i64 1008, !12, i64 1016, !12, i64 1024, !12, i64 1032, !12, i64 1040, !12, i64 1048, !12, i64 1056, !12, i64 1064, !12, i64 1072, !12, i64 1080, !12, i64 1088, !12, i64 1096, !12, i64 1104, !12, i64 1112, !12, i64 1120, !12, i64 1128, !12, i64 1136, !12, i64 1144, !12, i64 1152, !12, i64 1160, !12, i64 1168, !12, i64 1176, !12, i64 1184, !12, i64 1192, !12, i64 1200, !12, i64 1208, !12, i64 1216, !12, i64 1224, !12, i64 1232, !12, i64 1240, !12, i64 1248, !12, i64 1256, !12, i64 1264, !12, i64 1272, !12, i64 1280, !12, i64 1288, !12, i64 1296, !12, i64 1304, !12, i64 1312, !12, i64 1320, !12, i64 1328, !12, i64 1336, !12, i64 1344, !12, i64 1352, !12, i64 1360, !12, i64 1368, !12, i64 1376, !12, i64 1384, !12, i64 1392, !12, i64 1400, !12, i64 1408, !12, i64 1416, !12, i64 1424, !12, i64 1432, !12, i64 1440, !12, i64 1448, !12, i64 1456, !12, i64 1464, !12, i64 1472, !12, i64 1480, !12, i64 1488, !12, i64 1496, !12, i64 1504, !12, i64 1512, !12, i64 1520, !12, i64 1528, !12, i64 1536, !12, i64 1544, !12, i64 1552, !12, i64 1560, !12, i64 1568, !12, i64 1576, !12, i64 1584, !12, i64 1592, !12, i64 1600, !12, i64 1608, !12, i64 1616, !12, i64 1624, !12, i64 1632, !12, i64 1640, !12, i64 1648, !12, i64 1656, !12, i64 1664, !12, i64 1672, !12, i64 1680, !12, i64 1688, !12, i64 1696, !12, i64 1704, !12, i64 1712, !12, i64 1720, !12, i64 1728, !12, i64 1736, !12, i64 1744, !12, i64 1752, !12, i64 1760, !12, i64 1768, !12, i64 1776, !12, i64 1784, !12, i64 1792, !12, i64 1800, !12, i64 1808, !12, i64 1816, !12, i64 1824, !12, i64 1832, !12, i64 1840, !12, i64 1848, !12, i64 1856}
!23 = !{!13, !13, i64 0}
!24 = distinct !{!24, !25}
!25 = !{!"llvm.loop.mustprogress"}
!26 = !{!22, !12, i64 288}
!27 = distinct !{!27, !25}
!28 = !{!22, !12, i64 528}
!29 = distinct !{!29, !25}
!30 = !{!22, !12, i64 928}
!31 = distinct !{!31, !25}
!32 = !{!22, !12, i64 312}
!33 = distinct !{!33, !25}
!34 = !{!22, !12, i64 552}
!35 = distinct !{!35, !25}
!36 = !{!22, !12, i64 952}
!37 = distinct !{!37, !25}
!38 = !{!22, !12, i64 336}
!39 = distinct !{!39, !25}
!40 = !{!22, !12, i64 576}
!41 = distinct !{!41, !25}
!42 = !{!22, !12, i64 976}
!43 = distinct !{!43, !25}
!44 = !{!22, !12, i64 360}
!45 = distinct !{!45, !25}
!46 = !{!22, !12, i64 600}
!47 = distinct !{!47, !25}
!48 = !{!22, !12, i64 1000}
!49 = distinct !{!49, !25}
!50 = !{!22, !12, i64 384}
!51 = distinct !{!51, !25}
!52 = !{!22, !12, i64 624}
!53 = distinct !{!53, !25}
!54 = !{!22, !12, i64 1024}
!55 = distinct !{!55, !25}
!56 = !{!22, !12, i64 408}
!57 = distinct !{!57, !25}
!58 = !{!22, !12, i64 648}
!59 = distinct !{!59, !25}
!60 = !{!22, !12, i64 1048}
!61 = distinct !{!61, !25}
!62 = !{!22, !12, i64 432}
!63 = distinct !{!63, !25}
!64 = !{!22, !12, i64 672}
!65 = distinct !{!65, !25}
!66 = !{!22, !12, i64 1072}
!67 = distinct !{!67, !25}
!68 = !{!22, !12, i64 456}
!69 = distinct !{!69, !25}
!70 = !{!22, !12, i64 696}
!71 = distinct !{!71, !25}
!72 = !{!22, !12, i64 1096}
!73 = distinct !{!73, !25}
!74 = !{!22, !12, i64 480}
!75 = distinct !{!75, !25}
!76 = !{!22, !12, i64 720}
!77 = distinct !{!77, !25}
!78 = !{!22, !12, i64 1120}
!79 = distinct !{!79, !25}
!80 = !{!22, !12, i64 240}
!81 = distinct !{!81, !25}
!82 = !{!22, !12, i64 504}
!83 = distinct !{!83, !25}
!84 = !{!22, !12, i64 744}
!85 = distinct !{!85, !25}
!86 = !{!22, !12, i64 1144}
