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

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_CallObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !12

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 36
  %182 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !14
  %183 = call %struct._jobject* %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret %struct._jobject* %183
}

; Function Attrs: argmemonly mustprogress nofree nosync nounwind willreturn
declare void @llvm.lifetime.start.p0i8(i64 immarg, i8* nocapture) #1

; Function Attrs: mustprogress nofree nosync nounwind willreturn
declare void @llvm.va_start(i8*) #2

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_CallObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !12

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 36
  %188 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !14
  %189 = call %struct._jobject* %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret %struct._jobject* %189
}

; Function Attrs: mustprogress nofree nosync nounwind willreturn
declare void @llvm.va_end(i8*) #2

; Function Attrs: argmemonly mustprogress nofree nosync nounwind willreturn
declare void @llvm.lifetime.end.p0i8(i64 immarg, i8* nocapture) #1

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_CallNonvirtualObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = alloca [1 x %struct.__va_list_tag], align 16
  %7 = bitcast [1 x %struct.__va_list_tag]* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = call i8* @llvm.stacksave()
  %9 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %9) #4
  %10 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %10, align 8, !tbaa !5
  %12 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %11, align 8, !tbaa !9
  %13 = call i32 %12(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %9) #4
  %14 = sext i32 %13 to i64
  %15 = alloca %union.jvalue, i64 %14, align 16
  %16 = icmp sgt i32 %13, 0
  br i1 %16, label %17, label %180

17:                                               ; preds = %4
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 0
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 2
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 3
  %21 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 1
  %22 = zext i32 %13 to i64
  %23 = load i8*, i8** %20, align 16
  br label %24

24:                                               ; preds = %177, %17
  %25 = phi i64 [ 0, %17 ], [ %178, %177 ]
  %26 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %25
  %27 = load i8, i8* %26, align 1, !tbaa !11
  %28 = sext i8 %27 to i32
  switch i32 %28, label %177 [
    i32 90, label %29
    i32 66, label %46
    i32 83, label %63
    i32 67, label %80
    i32 73, label %97
    i32 74, label %113
    i32 68, label %128
    i32 70, label %144
    i32 76, label %161
  ]

29:                                               ; preds = %24
  %30 = load i32, i32* %18, align 16
  %31 = icmp ult i32 %30, 41
  br i1 %31, label %32, label %36

32:                                               ; preds = %29
  %33 = zext i32 %30 to i64
  %34 = getelementptr i8, i8* %23, i64 %33
  %35 = add nuw nsw i32 %30, 8
  store i32 %35, i32* %18, align 16
  br label %39

36:                                               ; preds = %29
  %37 = load i8*, i8** %19, align 8
  %38 = getelementptr i8, i8* %37, i64 8
  store i8* %38, i8** %19, align 8
  br label %39

39:                                               ; preds = %36, %32
  %40 = phi i8* [ %34, %32 ], [ %37, %36 ]
  %41 = bitcast i8* %40 to i32*
  %42 = load i32, i32* %41, align 4
  %43 = trunc i32 %42 to i8
  %44 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %43, i8* %45, align 8, !tbaa !11
  br label %177

46:                                               ; preds = %24
  %47 = load i32, i32* %18, align 16
  %48 = icmp ult i32 %47, 41
  br i1 %48, label %49, label %53

49:                                               ; preds = %46
  %50 = zext i32 %47 to i64
  %51 = getelementptr i8, i8* %23, i64 %50
  %52 = add nuw nsw i32 %47, 8
  store i32 %52, i32* %18, align 16
  br label %56

53:                                               ; preds = %46
  %54 = load i8*, i8** %19, align 8
  %55 = getelementptr i8, i8* %54, i64 8
  store i8* %55, i8** %19, align 8
  br label %56

56:                                               ; preds = %53, %49
  %57 = phi i8* [ %51, %49 ], [ %54, %53 ]
  %58 = bitcast i8* %57 to i32*
  %59 = load i32, i32* %58, align 4
  %60 = trunc i32 %59 to i8
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %60, i8* %62, align 8, !tbaa !11
  br label %177

63:                                               ; preds = %24
  %64 = load i32, i32* %18, align 16
  %65 = icmp ult i32 %64, 41
  br i1 %65, label %66, label %70

66:                                               ; preds = %63
  %67 = zext i32 %64 to i64
  %68 = getelementptr i8, i8* %23, i64 %67
  %69 = add nuw nsw i32 %64, 8
  store i32 %69, i32* %18, align 16
  br label %73

70:                                               ; preds = %63
  %71 = load i8*, i8** %19, align 8
  %72 = getelementptr i8, i8* %71, i64 8
  store i8* %72, i8** %19, align 8
  br label %73

73:                                               ; preds = %70, %66
  %74 = phi i8* [ %68, %66 ], [ %71, %70 ]
  %75 = bitcast i8* %74 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %77, i16* %79, align 8, !tbaa !11
  br label %177

80:                                               ; preds = %24
  %81 = load i32, i32* %18, align 16
  %82 = icmp ult i32 %81, 41
  br i1 %82, label %83, label %87

83:                                               ; preds = %80
  %84 = zext i32 %81 to i64
  %85 = getelementptr i8, i8* %23, i64 %84
  %86 = add nuw nsw i32 %81, 8
  store i32 %86, i32* %18, align 16
  br label %90

87:                                               ; preds = %80
  %88 = load i8*, i8** %19, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %19, align 8
  br label %90

90:                                               ; preds = %87, %83
  %91 = phi i8* [ %85, %83 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %177

97:                                               ; preds = %24
  %98 = load i32, i32* %18, align 16
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %104

100:                                              ; preds = %97
  %101 = zext i32 %98 to i64
  %102 = getelementptr i8, i8* %23, i64 %101
  %103 = add nuw nsw i32 %98, 8
  store i32 %103, i32* %18, align 16
  br label %107

104:                                              ; preds = %97
  %105 = load i8*, i8** %19, align 8
  %106 = getelementptr i8, i8* %105, i64 8
  store i8* %106, i8** %19, align 8
  br label %107

107:                                              ; preds = %104, %100
  %108 = phi i8* [ %102, %100 ], [ %105, %104 ]
  %109 = bitcast i8* %108 to i32*
  %110 = load i32, i32* %109, align 4
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %112 = bitcast %union.jvalue* %111 to i32*
  store i32 %110, i32* %112, align 8, !tbaa !11
  br label %177

113:                                              ; preds = %24
  %114 = load i32, i32* %18, align 16
  %115 = icmp ult i32 %114, 41
  br i1 %115, label %116, label %120

116:                                              ; preds = %113
  %117 = zext i32 %114 to i64
  %118 = getelementptr i8, i8* %23, i64 %117
  %119 = add nuw nsw i32 %114, 8
  store i32 %119, i32* %18, align 16
  br label %123

120:                                              ; preds = %113
  %121 = load i8*, i8** %19, align 8
  %122 = getelementptr i8, i8* %121, i64 8
  store i8* %122, i8** %19, align 8
  br label %123

123:                                              ; preds = %120, %116
  %124 = phi i8* [ %118, %116 ], [ %121, %120 ]
  %125 = bitcast i8* %124 to i64*
  %126 = load i64, i64* %125, align 8
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25, i32 0
  store i64 %126, i64* %127, align 8, !tbaa !11
  br label %177

128:                                              ; preds = %24
  %129 = load i32, i32* %21, align 4
  %130 = icmp ult i32 %129, 161
  br i1 %130, label %131, label %135

131:                                              ; preds = %128
  %132 = zext i32 %129 to i64
  %133 = getelementptr i8, i8* %23, i64 %132
  %134 = add nuw nsw i32 %129, 16
  store i32 %134, i32* %21, align 4
  br label %138

135:                                              ; preds = %128
  %136 = load i8*, i8** %19, align 8
  %137 = getelementptr i8, i8* %136, i64 8
  store i8* %137, i8** %19, align 8
  br label %138

138:                                              ; preds = %135, %131
  %139 = phi i8* [ %133, %131 ], [ %136, %135 ]
  %140 = bitcast i8* %139 to double*
  %141 = load double, double* %140, align 8
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %143 = bitcast %union.jvalue* %142 to double*
  store double %141, double* %143, align 8, !tbaa !11
  br label %177

144:                                              ; preds = %24
  %145 = load i32, i32* %21, align 4
  %146 = icmp ult i32 %145, 161
  br i1 %146, label %147, label %151

147:                                              ; preds = %144
  %148 = zext i32 %145 to i64
  %149 = getelementptr i8, i8* %23, i64 %148
  %150 = add nuw nsw i32 %145, 16
  store i32 %150, i32* %21, align 4
  br label %154

151:                                              ; preds = %144
  %152 = load i8*, i8** %19, align 8
  %153 = getelementptr i8, i8* %152, i64 8
  store i8* %153, i8** %19, align 8
  br label %154

154:                                              ; preds = %151, %147
  %155 = phi i8* [ %149, %147 ], [ %152, %151 ]
  %156 = bitcast i8* %155 to double*
  %157 = load double, double* %156, align 8
  %158 = fptrunc double %157 to float
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %160 = bitcast %union.jvalue* %159 to float*
  store float %158, float* %160, align 8, !tbaa !11
  br label %177

161:                                              ; preds = %24
  %162 = load i32, i32* %18, align 16
  %163 = icmp ult i32 %162, 41
  br i1 %163, label %164, label %168

164:                                              ; preds = %161
  %165 = zext i32 %162 to i64
  %166 = getelementptr i8, i8* %23, i64 %165
  %167 = add nuw nsw i32 %162, 8
  store i32 %167, i32* %18, align 16
  br label %171

168:                                              ; preds = %161
  %169 = load i8*, i8** %19, align 8
  %170 = getelementptr i8, i8* %169, i64 8
  store i8* %170, i8** %19, align 8
  br label %171

171:                                              ; preds = %168, %164
  %172 = phi i8* [ %166, %164 ], [ %169, %168 ]
  %173 = bitcast i8* %172 to %struct._jobject**
  %174 = load %struct._jobject*, %struct._jobject** %173, align 8
  %175 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %176, align 8, !tbaa !11
  br label %177

177:                                              ; preds = %171, %154, %138, %123, %107, %90, %73, %56, %39, %24
  %178 = add nuw nsw i64 %25, 1
  %179 = icmp eq i64 %178, %22
  br i1 %179, label %180, label %24, !llvm.loop !15

180:                                              ; preds = %177, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %9) #4
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i64 0, i32 66
  %183 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8, !tbaa !16
  %184 = call %struct._jobject* %183(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %15) #4
  call void @llvm.stackrestore(i8* %8)
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %7) #4
  ret %struct._jobject* %184
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_CallNonvirtualObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %struct.__va_list_tag* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 16
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !5
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !9
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #4
  %12 = sext i32 %11 to i64
  %13 = alloca %union.jvalue, i64 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %186

15:                                               ; preds = %5
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 0
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 2
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 3
  %19 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 1
  %20 = zext i32 %11 to i64
  br label %21

21:                                               ; preds = %15, %183
  %22 = phi i64 [ 0, %15 ], [ %184, %183 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !11
  %25 = sext i8 %24 to i32
  switch i32 %25, label %183 [
    i32 90, label %26
    i32 66, label %44
    i32 83, label %62
    i32 67, label %80
    i32 73, label %98
    i32 74, label %115
    i32 68, label %131
    i32 70, label %148
    i32 76, label %166
  ]

26:                                               ; preds = %21
  %27 = load i32, i32* %16, align 8
  %28 = icmp ult i32 %27, 41
  br i1 %28, label %29, label %34

29:                                               ; preds = %26
  %30 = load i8*, i8** %18, align 8
  %31 = zext i32 %27 to i64
  %32 = getelementptr i8, i8* %30, i64 %31
  %33 = add nuw nsw i32 %27, 8
  store i32 %33, i32* %16, align 8
  br label %37

34:                                               ; preds = %26
  %35 = load i8*, i8** %17, align 8
  %36 = getelementptr i8, i8* %35, i64 8
  store i8* %36, i8** %17, align 8
  br label %37

37:                                               ; preds = %34, %29
  %38 = phi i8* [ %32, %29 ], [ %35, %34 ]
  %39 = bitcast i8* %38 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %41, i8* %43, align 8, !tbaa !11
  br label %183

44:                                               ; preds = %21
  %45 = load i32, i32* %16, align 8
  %46 = icmp ult i32 %45, 41
  br i1 %46, label %47, label %52

47:                                               ; preds = %44
  %48 = load i8*, i8** %18, align 8
  %49 = zext i32 %45 to i64
  %50 = getelementptr i8, i8* %48, i64 %49
  %51 = add nuw nsw i32 %45, 8
  store i32 %51, i32* %16, align 8
  br label %55

52:                                               ; preds = %44
  %53 = load i8*, i8** %17, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %17, align 8
  br label %55

55:                                               ; preds = %52, %47
  %56 = phi i8* [ %50, %47 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %183

62:                                               ; preds = %21
  %63 = load i32, i32* %16, align 8
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %70

65:                                               ; preds = %62
  %66 = load i8*, i8** %18, align 8
  %67 = zext i32 %63 to i64
  %68 = getelementptr i8, i8* %66, i64 %67
  %69 = add nuw nsw i32 %63, 8
  store i32 %69, i32* %16, align 8
  br label %73

70:                                               ; preds = %62
  %71 = load i8*, i8** %17, align 8
  %72 = getelementptr i8, i8* %71, i64 8
  store i8* %72, i8** %17, align 8
  br label %73

73:                                               ; preds = %70, %65
  %74 = phi i8* [ %68, %65 ], [ %71, %70 ]
  %75 = bitcast i8* %74 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %77, i16* %79, align 8, !tbaa !11
  br label %183

80:                                               ; preds = %21
  %81 = load i32, i32* %16, align 8
  %82 = icmp ult i32 %81, 41
  br i1 %82, label %83, label %88

83:                                               ; preds = %80
  %84 = load i8*, i8** %18, align 8
  %85 = zext i32 %81 to i64
  %86 = getelementptr i8, i8* %84, i64 %85
  %87 = add nuw nsw i32 %81, 8
  store i32 %87, i32* %16, align 8
  br label %91

88:                                               ; preds = %80
  %89 = load i8*, i8** %17, align 8
  %90 = getelementptr i8, i8* %89, i64 8
  store i8* %90, i8** %17, align 8
  br label %91

91:                                               ; preds = %88, %83
  %92 = phi i8* [ %86, %83 ], [ %89, %88 ]
  %93 = bitcast i8* %92 to i32*
  %94 = load i32, i32* %93, align 4
  %95 = and i32 %94, 65535
  %96 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %97 = bitcast %union.jvalue* %96 to i32*
  store i32 %95, i32* %97, align 8, !tbaa !11
  br label %183

98:                                               ; preds = %21
  %99 = load i32, i32* %16, align 8
  %100 = icmp ult i32 %99, 41
  br i1 %100, label %101, label %106

101:                                              ; preds = %98
  %102 = load i8*, i8** %18, align 8
  %103 = zext i32 %99 to i64
  %104 = getelementptr i8, i8* %102, i64 %103
  %105 = add nuw nsw i32 %99, 8
  store i32 %105, i32* %16, align 8
  br label %109

106:                                              ; preds = %98
  %107 = load i8*, i8** %17, align 8
  %108 = getelementptr i8, i8* %107, i64 8
  store i8* %108, i8** %17, align 8
  br label %109

109:                                              ; preds = %106, %101
  %110 = phi i8* [ %104, %101 ], [ %107, %106 ]
  %111 = bitcast i8* %110 to i32*
  %112 = load i32, i32* %111, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !11
  br label %183

115:                                              ; preds = %21
  %116 = load i32, i32* %16, align 8
  %117 = icmp ult i32 %116, 41
  br i1 %117, label %118, label %123

118:                                              ; preds = %115
  %119 = load i8*, i8** %18, align 8
  %120 = zext i32 %116 to i64
  %121 = getelementptr i8, i8* %119, i64 %120
  %122 = add nuw nsw i32 %116, 8
  store i32 %122, i32* %16, align 8
  br label %126

123:                                              ; preds = %115
  %124 = load i8*, i8** %17, align 8
  %125 = getelementptr i8, i8* %124, i64 8
  store i8* %125, i8** %17, align 8
  br label %126

126:                                              ; preds = %123, %118
  %127 = phi i8* [ %121, %118 ], [ %124, %123 ]
  %128 = bitcast i8* %127 to i64*
  %129 = load i64, i64* %128, align 8
  %130 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22, i32 0
  store i64 %129, i64* %130, align 8, !tbaa !11
  br label %183

131:                                              ; preds = %21
  %132 = load i32, i32* %19, align 4
  %133 = icmp ult i32 %132, 161
  br i1 %133, label %134, label %139

134:                                              ; preds = %131
  %135 = load i8*, i8** %18, align 8
  %136 = zext i32 %132 to i64
  %137 = getelementptr i8, i8* %135, i64 %136
  %138 = add nuw nsw i32 %132, 16
  store i32 %138, i32* %19, align 4
  br label %142

139:                                              ; preds = %131
  %140 = load i8*, i8** %17, align 8
  %141 = getelementptr i8, i8* %140, i64 8
  store i8* %141, i8** %17, align 8
  br label %142

142:                                              ; preds = %139, %134
  %143 = phi i8* [ %137, %134 ], [ %140, %139 ]
  %144 = bitcast i8* %143 to double*
  %145 = load double, double* %144, align 8
  %146 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %147 = bitcast %union.jvalue* %146 to double*
  store double %145, double* %147, align 8, !tbaa !11
  br label %183

148:                                              ; preds = %21
  %149 = load i32, i32* %19, align 4
  %150 = icmp ult i32 %149, 161
  br i1 %150, label %151, label %156

151:                                              ; preds = %148
  %152 = load i8*, i8** %18, align 8
  %153 = zext i32 %149 to i64
  %154 = getelementptr i8, i8* %152, i64 %153
  %155 = add nuw nsw i32 %149, 16
  store i32 %155, i32* %19, align 4
  br label %159

156:                                              ; preds = %148
  %157 = load i8*, i8** %17, align 8
  %158 = getelementptr i8, i8* %157, i64 8
  store i8* %158, i8** %17, align 8
  br label %159

159:                                              ; preds = %156, %151
  %160 = phi i8* [ %154, %151 ], [ %157, %156 ]
  %161 = bitcast i8* %160 to double*
  %162 = load double, double* %161, align 8
  %163 = fptrunc double %162 to float
  %164 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %165 = bitcast %union.jvalue* %164 to float*
  store float %163, float* %165, align 8, !tbaa !11
  br label %183

166:                                              ; preds = %21
  %167 = load i32, i32* %16, align 8
  %168 = icmp ult i32 %167, 41
  br i1 %168, label %169, label %174

169:                                              ; preds = %166
  %170 = load i8*, i8** %18, align 8
  %171 = zext i32 %167 to i64
  %172 = getelementptr i8, i8* %170, i64 %171
  %173 = add nuw nsw i32 %167, 8
  store i32 %173, i32* %16, align 8
  br label %177

174:                                              ; preds = %166
  %175 = load i8*, i8** %17, align 8
  %176 = getelementptr i8, i8* %175, i64 8
  store i8* %176, i8** %17, align 8
  br label %177

177:                                              ; preds = %174, %169
  %178 = phi i8* [ %172, %169 ], [ %175, %174 ]
  %179 = bitcast i8* %178 to %struct._jobject**
  %180 = load %struct._jobject*, %struct._jobject** %179, align 8
  %181 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %182 = bitcast %union.jvalue* %181 to %struct._jobject**
  store %struct._jobject* %180, %struct._jobject** %182, align 8, !tbaa !11
  br label %183

183:                                              ; preds = %37, %55, %73, %91, %109, %126, %142, %159, %177, %21
  %184 = add nuw nsw i64 %22, 1
  %185 = icmp eq i64 %184, %20
  br i1 %185, label %186, label %21, !llvm.loop !15

186:                                              ; preds = %183, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %187 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %188 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %187, i64 0, i32 66
  %189 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %188, align 8, !tbaa !16
  %190 = call %struct._jobject* %189(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret %struct._jobject* %190
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_CallStaticObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !17

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 116
  %182 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !18
  %183 = call %struct._jobject* %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret %struct._jobject* %183
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_CallStaticObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !17

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 116
  %188 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !18
  %189 = call %struct._jobject* %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret %struct._jobject* %189
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local zeroext i8 @JNI_CallBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !19

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 39
  %182 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !20
  %183 = call zeroext i8 %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret i8 %183
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local zeroext i8 @JNI_CallBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !19

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 39
  %188 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !20
  %189 = call zeroext i8 %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i8 %189
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local zeroext i8 @JNI_CallNonvirtualBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = alloca [1 x %struct.__va_list_tag], align 16
  %7 = bitcast [1 x %struct.__va_list_tag]* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = call i8* @llvm.stacksave()
  %9 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %9) #4
  %10 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %10, align 8, !tbaa !5
  %12 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %11, align 8, !tbaa !9
  %13 = call i32 %12(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %9) #4
  %14 = sext i32 %13 to i64
  %15 = alloca %union.jvalue, i64 %14, align 16
  %16 = icmp sgt i32 %13, 0
  br i1 %16, label %17, label %180

17:                                               ; preds = %4
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 0
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 2
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 3
  %21 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 1
  %22 = zext i32 %13 to i64
  %23 = load i8*, i8** %20, align 16
  br label %24

24:                                               ; preds = %177, %17
  %25 = phi i64 [ 0, %17 ], [ %178, %177 ]
  %26 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %25
  %27 = load i8, i8* %26, align 1, !tbaa !11
  %28 = sext i8 %27 to i32
  switch i32 %28, label %177 [
    i32 90, label %29
    i32 66, label %46
    i32 83, label %63
    i32 67, label %80
    i32 73, label %97
    i32 74, label %113
    i32 68, label %128
    i32 70, label %144
    i32 76, label %161
  ]

29:                                               ; preds = %24
  %30 = load i32, i32* %18, align 16
  %31 = icmp ult i32 %30, 41
  br i1 %31, label %32, label %36

32:                                               ; preds = %29
  %33 = zext i32 %30 to i64
  %34 = getelementptr i8, i8* %23, i64 %33
  %35 = add nuw nsw i32 %30, 8
  store i32 %35, i32* %18, align 16
  br label %39

36:                                               ; preds = %29
  %37 = load i8*, i8** %19, align 8
  %38 = getelementptr i8, i8* %37, i64 8
  store i8* %38, i8** %19, align 8
  br label %39

39:                                               ; preds = %36, %32
  %40 = phi i8* [ %34, %32 ], [ %37, %36 ]
  %41 = bitcast i8* %40 to i32*
  %42 = load i32, i32* %41, align 4
  %43 = trunc i32 %42 to i8
  %44 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %43, i8* %45, align 8, !tbaa !11
  br label %177

46:                                               ; preds = %24
  %47 = load i32, i32* %18, align 16
  %48 = icmp ult i32 %47, 41
  br i1 %48, label %49, label %53

49:                                               ; preds = %46
  %50 = zext i32 %47 to i64
  %51 = getelementptr i8, i8* %23, i64 %50
  %52 = add nuw nsw i32 %47, 8
  store i32 %52, i32* %18, align 16
  br label %56

53:                                               ; preds = %46
  %54 = load i8*, i8** %19, align 8
  %55 = getelementptr i8, i8* %54, i64 8
  store i8* %55, i8** %19, align 8
  br label %56

56:                                               ; preds = %53, %49
  %57 = phi i8* [ %51, %49 ], [ %54, %53 ]
  %58 = bitcast i8* %57 to i32*
  %59 = load i32, i32* %58, align 4
  %60 = trunc i32 %59 to i8
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %60, i8* %62, align 8, !tbaa !11
  br label %177

63:                                               ; preds = %24
  %64 = load i32, i32* %18, align 16
  %65 = icmp ult i32 %64, 41
  br i1 %65, label %66, label %70

66:                                               ; preds = %63
  %67 = zext i32 %64 to i64
  %68 = getelementptr i8, i8* %23, i64 %67
  %69 = add nuw nsw i32 %64, 8
  store i32 %69, i32* %18, align 16
  br label %73

70:                                               ; preds = %63
  %71 = load i8*, i8** %19, align 8
  %72 = getelementptr i8, i8* %71, i64 8
  store i8* %72, i8** %19, align 8
  br label %73

73:                                               ; preds = %70, %66
  %74 = phi i8* [ %68, %66 ], [ %71, %70 ]
  %75 = bitcast i8* %74 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %77, i16* %79, align 8, !tbaa !11
  br label %177

80:                                               ; preds = %24
  %81 = load i32, i32* %18, align 16
  %82 = icmp ult i32 %81, 41
  br i1 %82, label %83, label %87

83:                                               ; preds = %80
  %84 = zext i32 %81 to i64
  %85 = getelementptr i8, i8* %23, i64 %84
  %86 = add nuw nsw i32 %81, 8
  store i32 %86, i32* %18, align 16
  br label %90

87:                                               ; preds = %80
  %88 = load i8*, i8** %19, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %19, align 8
  br label %90

90:                                               ; preds = %87, %83
  %91 = phi i8* [ %85, %83 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %177

97:                                               ; preds = %24
  %98 = load i32, i32* %18, align 16
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %104

100:                                              ; preds = %97
  %101 = zext i32 %98 to i64
  %102 = getelementptr i8, i8* %23, i64 %101
  %103 = add nuw nsw i32 %98, 8
  store i32 %103, i32* %18, align 16
  br label %107

104:                                              ; preds = %97
  %105 = load i8*, i8** %19, align 8
  %106 = getelementptr i8, i8* %105, i64 8
  store i8* %106, i8** %19, align 8
  br label %107

107:                                              ; preds = %104, %100
  %108 = phi i8* [ %102, %100 ], [ %105, %104 ]
  %109 = bitcast i8* %108 to i32*
  %110 = load i32, i32* %109, align 4
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %112 = bitcast %union.jvalue* %111 to i32*
  store i32 %110, i32* %112, align 8, !tbaa !11
  br label %177

113:                                              ; preds = %24
  %114 = load i32, i32* %18, align 16
  %115 = icmp ult i32 %114, 41
  br i1 %115, label %116, label %120

116:                                              ; preds = %113
  %117 = zext i32 %114 to i64
  %118 = getelementptr i8, i8* %23, i64 %117
  %119 = add nuw nsw i32 %114, 8
  store i32 %119, i32* %18, align 16
  br label %123

120:                                              ; preds = %113
  %121 = load i8*, i8** %19, align 8
  %122 = getelementptr i8, i8* %121, i64 8
  store i8* %122, i8** %19, align 8
  br label %123

123:                                              ; preds = %120, %116
  %124 = phi i8* [ %118, %116 ], [ %121, %120 ]
  %125 = bitcast i8* %124 to i64*
  %126 = load i64, i64* %125, align 8
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25, i32 0
  store i64 %126, i64* %127, align 8, !tbaa !11
  br label %177

128:                                              ; preds = %24
  %129 = load i32, i32* %21, align 4
  %130 = icmp ult i32 %129, 161
  br i1 %130, label %131, label %135

131:                                              ; preds = %128
  %132 = zext i32 %129 to i64
  %133 = getelementptr i8, i8* %23, i64 %132
  %134 = add nuw nsw i32 %129, 16
  store i32 %134, i32* %21, align 4
  br label %138

135:                                              ; preds = %128
  %136 = load i8*, i8** %19, align 8
  %137 = getelementptr i8, i8* %136, i64 8
  store i8* %137, i8** %19, align 8
  br label %138

138:                                              ; preds = %135, %131
  %139 = phi i8* [ %133, %131 ], [ %136, %135 ]
  %140 = bitcast i8* %139 to double*
  %141 = load double, double* %140, align 8
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %143 = bitcast %union.jvalue* %142 to double*
  store double %141, double* %143, align 8, !tbaa !11
  br label %177

144:                                              ; preds = %24
  %145 = load i32, i32* %21, align 4
  %146 = icmp ult i32 %145, 161
  br i1 %146, label %147, label %151

147:                                              ; preds = %144
  %148 = zext i32 %145 to i64
  %149 = getelementptr i8, i8* %23, i64 %148
  %150 = add nuw nsw i32 %145, 16
  store i32 %150, i32* %21, align 4
  br label %154

151:                                              ; preds = %144
  %152 = load i8*, i8** %19, align 8
  %153 = getelementptr i8, i8* %152, i64 8
  store i8* %153, i8** %19, align 8
  br label %154

154:                                              ; preds = %151, %147
  %155 = phi i8* [ %149, %147 ], [ %152, %151 ]
  %156 = bitcast i8* %155 to double*
  %157 = load double, double* %156, align 8
  %158 = fptrunc double %157 to float
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %160 = bitcast %union.jvalue* %159 to float*
  store float %158, float* %160, align 8, !tbaa !11
  br label %177

161:                                              ; preds = %24
  %162 = load i32, i32* %18, align 16
  %163 = icmp ult i32 %162, 41
  br i1 %163, label %164, label %168

164:                                              ; preds = %161
  %165 = zext i32 %162 to i64
  %166 = getelementptr i8, i8* %23, i64 %165
  %167 = add nuw nsw i32 %162, 8
  store i32 %167, i32* %18, align 16
  br label %171

168:                                              ; preds = %161
  %169 = load i8*, i8** %19, align 8
  %170 = getelementptr i8, i8* %169, i64 8
  store i8* %170, i8** %19, align 8
  br label %171

171:                                              ; preds = %168, %164
  %172 = phi i8* [ %166, %164 ], [ %169, %168 ]
  %173 = bitcast i8* %172 to %struct._jobject**
  %174 = load %struct._jobject*, %struct._jobject** %173, align 8
  %175 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %176, align 8, !tbaa !11
  br label %177

177:                                              ; preds = %171, %154, %138, %123, %107, %90, %73, %56, %39, %24
  %178 = add nuw nsw i64 %25, 1
  %179 = icmp eq i64 %178, %22
  br i1 %179, label %180, label %24, !llvm.loop !21

180:                                              ; preds = %177, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %9) #4
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i64 0, i32 69
  %183 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8, !tbaa !22
  %184 = call zeroext i8 %183(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %15) #4
  call void @llvm.stackrestore(i8* %8)
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %7) #4
  ret i8 %184
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local zeroext i8 @JNI_CallNonvirtualBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %struct.__va_list_tag* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 16
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !5
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !9
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #4
  %12 = sext i32 %11 to i64
  %13 = alloca %union.jvalue, i64 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %186

15:                                               ; preds = %5
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 0
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 2
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 3
  %19 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 1
  %20 = zext i32 %11 to i64
  br label %21

21:                                               ; preds = %15, %183
  %22 = phi i64 [ 0, %15 ], [ %184, %183 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !11
  %25 = sext i8 %24 to i32
  switch i32 %25, label %183 [
    i32 90, label %26
    i32 66, label %44
    i32 83, label %62
    i32 67, label %80
    i32 73, label %98
    i32 74, label %115
    i32 68, label %131
    i32 70, label %148
    i32 76, label %166
  ]

26:                                               ; preds = %21
  %27 = load i32, i32* %16, align 8
  %28 = icmp ult i32 %27, 41
  br i1 %28, label %29, label %34

29:                                               ; preds = %26
  %30 = load i8*, i8** %18, align 8
  %31 = zext i32 %27 to i64
  %32 = getelementptr i8, i8* %30, i64 %31
  %33 = add nuw nsw i32 %27, 8
  store i32 %33, i32* %16, align 8
  br label %37

34:                                               ; preds = %26
  %35 = load i8*, i8** %17, align 8
  %36 = getelementptr i8, i8* %35, i64 8
  store i8* %36, i8** %17, align 8
  br label %37

37:                                               ; preds = %34, %29
  %38 = phi i8* [ %32, %29 ], [ %35, %34 ]
  %39 = bitcast i8* %38 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %41, i8* %43, align 8, !tbaa !11
  br label %183

44:                                               ; preds = %21
  %45 = load i32, i32* %16, align 8
  %46 = icmp ult i32 %45, 41
  br i1 %46, label %47, label %52

47:                                               ; preds = %44
  %48 = load i8*, i8** %18, align 8
  %49 = zext i32 %45 to i64
  %50 = getelementptr i8, i8* %48, i64 %49
  %51 = add nuw nsw i32 %45, 8
  store i32 %51, i32* %16, align 8
  br label %55

52:                                               ; preds = %44
  %53 = load i8*, i8** %17, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %17, align 8
  br label %55

55:                                               ; preds = %52, %47
  %56 = phi i8* [ %50, %47 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %183

62:                                               ; preds = %21
  %63 = load i32, i32* %16, align 8
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %70

65:                                               ; preds = %62
  %66 = load i8*, i8** %18, align 8
  %67 = zext i32 %63 to i64
  %68 = getelementptr i8, i8* %66, i64 %67
  %69 = add nuw nsw i32 %63, 8
  store i32 %69, i32* %16, align 8
  br label %73

70:                                               ; preds = %62
  %71 = load i8*, i8** %17, align 8
  %72 = getelementptr i8, i8* %71, i64 8
  store i8* %72, i8** %17, align 8
  br label %73

73:                                               ; preds = %70, %65
  %74 = phi i8* [ %68, %65 ], [ %71, %70 ]
  %75 = bitcast i8* %74 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %77, i16* %79, align 8, !tbaa !11
  br label %183

80:                                               ; preds = %21
  %81 = load i32, i32* %16, align 8
  %82 = icmp ult i32 %81, 41
  br i1 %82, label %83, label %88

83:                                               ; preds = %80
  %84 = load i8*, i8** %18, align 8
  %85 = zext i32 %81 to i64
  %86 = getelementptr i8, i8* %84, i64 %85
  %87 = add nuw nsw i32 %81, 8
  store i32 %87, i32* %16, align 8
  br label %91

88:                                               ; preds = %80
  %89 = load i8*, i8** %17, align 8
  %90 = getelementptr i8, i8* %89, i64 8
  store i8* %90, i8** %17, align 8
  br label %91

91:                                               ; preds = %88, %83
  %92 = phi i8* [ %86, %83 ], [ %89, %88 ]
  %93 = bitcast i8* %92 to i32*
  %94 = load i32, i32* %93, align 4
  %95 = and i32 %94, 65535
  %96 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %97 = bitcast %union.jvalue* %96 to i32*
  store i32 %95, i32* %97, align 8, !tbaa !11
  br label %183

98:                                               ; preds = %21
  %99 = load i32, i32* %16, align 8
  %100 = icmp ult i32 %99, 41
  br i1 %100, label %101, label %106

101:                                              ; preds = %98
  %102 = load i8*, i8** %18, align 8
  %103 = zext i32 %99 to i64
  %104 = getelementptr i8, i8* %102, i64 %103
  %105 = add nuw nsw i32 %99, 8
  store i32 %105, i32* %16, align 8
  br label %109

106:                                              ; preds = %98
  %107 = load i8*, i8** %17, align 8
  %108 = getelementptr i8, i8* %107, i64 8
  store i8* %108, i8** %17, align 8
  br label %109

109:                                              ; preds = %106, %101
  %110 = phi i8* [ %104, %101 ], [ %107, %106 ]
  %111 = bitcast i8* %110 to i32*
  %112 = load i32, i32* %111, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !11
  br label %183

115:                                              ; preds = %21
  %116 = load i32, i32* %16, align 8
  %117 = icmp ult i32 %116, 41
  br i1 %117, label %118, label %123

118:                                              ; preds = %115
  %119 = load i8*, i8** %18, align 8
  %120 = zext i32 %116 to i64
  %121 = getelementptr i8, i8* %119, i64 %120
  %122 = add nuw nsw i32 %116, 8
  store i32 %122, i32* %16, align 8
  br label %126

123:                                              ; preds = %115
  %124 = load i8*, i8** %17, align 8
  %125 = getelementptr i8, i8* %124, i64 8
  store i8* %125, i8** %17, align 8
  br label %126

126:                                              ; preds = %123, %118
  %127 = phi i8* [ %121, %118 ], [ %124, %123 ]
  %128 = bitcast i8* %127 to i64*
  %129 = load i64, i64* %128, align 8
  %130 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22, i32 0
  store i64 %129, i64* %130, align 8, !tbaa !11
  br label %183

131:                                              ; preds = %21
  %132 = load i32, i32* %19, align 4
  %133 = icmp ult i32 %132, 161
  br i1 %133, label %134, label %139

134:                                              ; preds = %131
  %135 = load i8*, i8** %18, align 8
  %136 = zext i32 %132 to i64
  %137 = getelementptr i8, i8* %135, i64 %136
  %138 = add nuw nsw i32 %132, 16
  store i32 %138, i32* %19, align 4
  br label %142

139:                                              ; preds = %131
  %140 = load i8*, i8** %17, align 8
  %141 = getelementptr i8, i8* %140, i64 8
  store i8* %141, i8** %17, align 8
  br label %142

142:                                              ; preds = %139, %134
  %143 = phi i8* [ %137, %134 ], [ %140, %139 ]
  %144 = bitcast i8* %143 to double*
  %145 = load double, double* %144, align 8
  %146 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %147 = bitcast %union.jvalue* %146 to double*
  store double %145, double* %147, align 8, !tbaa !11
  br label %183

148:                                              ; preds = %21
  %149 = load i32, i32* %19, align 4
  %150 = icmp ult i32 %149, 161
  br i1 %150, label %151, label %156

151:                                              ; preds = %148
  %152 = load i8*, i8** %18, align 8
  %153 = zext i32 %149 to i64
  %154 = getelementptr i8, i8* %152, i64 %153
  %155 = add nuw nsw i32 %149, 16
  store i32 %155, i32* %19, align 4
  br label %159

156:                                              ; preds = %148
  %157 = load i8*, i8** %17, align 8
  %158 = getelementptr i8, i8* %157, i64 8
  store i8* %158, i8** %17, align 8
  br label %159

159:                                              ; preds = %156, %151
  %160 = phi i8* [ %154, %151 ], [ %157, %156 ]
  %161 = bitcast i8* %160 to double*
  %162 = load double, double* %161, align 8
  %163 = fptrunc double %162 to float
  %164 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %165 = bitcast %union.jvalue* %164 to float*
  store float %163, float* %165, align 8, !tbaa !11
  br label %183

166:                                              ; preds = %21
  %167 = load i32, i32* %16, align 8
  %168 = icmp ult i32 %167, 41
  br i1 %168, label %169, label %174

169:                                              ; preds = %166
  %170 = load i8*, i8** %18, align 8
  %171 = zext i32 %167 to i64
  %172 = getelementptr i8, i8* %170, i64 %171
  %173 = add nuw nsw i32 %167, 8
  store i32 %173, i32* %16, align 8
  br label %177

174:                                              ; preds = %166
  %175 = load i8*, i8** %17, align 8
  %176 = getelementptr i8, i8* %175, i64 8
  store i8* %176, i8** %17, align 8
  br label %177

177:                                              ; preds = %174, %169
  %178 = phi i8* [ %172, %169 ], [ %175, %174 ]
  %179 = bitcast i8* %178 to %struct._jobject**
  %180 = load %struct._jobject*, %struct._jobject** %179, align 8
  %181 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %182 = bitcast %union.jvalue* %181 to %struct._jobject**
  store %struct._jobject* %180, %struct._jobject** %182, align 8, !tbaa !11
  br label %183

183:                                              ; preds = %37, %55, %73, %91, %109, %126, %142, %159, %177, %21
  %184 = add nuw nsw i64 %22, 1
  %185 = icmp eq i64 %184, %20
  br i1 %185, label %186, label %21, !llvm.loop !21

186:                                              ; preds = %183, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %187 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %188 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %187, i64 0, i32 69
  %189 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %188, align 8, !tbaa !22
  %190 = call zeroext i8 %189(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret i8 %190
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local zeroext i8 @JNI_CallStaticBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !23

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 119
  %182 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !24
  %183 = call zeroext i8 %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret i8 %183
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local zeroext i8 @JNI_CallStaticBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !23

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 119
  %188 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !24
  %189 = call zeroext i8 %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i8 %189
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local signext i8 @JNI_CallByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !25

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 42
  %182 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !26
  %183 = call signext i8 %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret i8 %183
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local signext i8 @JNI_CallByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !25

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 42
  %188 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !26
  %189 = call signext i8 %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i8 %189
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local signext i8 @JNI_CallNonvirtualByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = alloca [1 x %struct.__va_list_tag], align 16
  %7 = bitcast [1 x %struct.__va_list_tag]* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = call i8* @llvm.stacksave()
  %9 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %9) #4
  %10 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %10, align 8, !tbaa !5
  %12 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %11, align 8, !tbaa !9
  %13 = call i32 %12(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %9) #4
  %14 = sext i32 %13 to i64
  %15 = alloca %union.jvalue, i64 %14, align 16
  %16 = icmp sgt i32 %13, 0
  br i1 %16, label %17, label %180

17:                                               ; preds = %4
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 0
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 2
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 3
  %21 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 1
  %22 = zext i32 %13 to i64
  %23 = load i8*, i8** %20, align 16
  br label %24

24:                                               ; preds = %177, %17
  %25 = phi i64 [ 0, %17 ], [ %178, %177 ]
  %26 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %25
  %27 = load i8, i8* %26, align 1, !tbaa !11
  %28 = sext i8 %27 to i32
  switch i32 %28, label %177 [
    i32 90, label %29
    i32 66, label %46
    i32 83, label %63
    i32 67, label %80
    i32 73, label %97
    i32 74, label %113
    i32 68, label %128
    i32 70, label %144
    i32 76, label %161
  ]

29:                                               ; preds = %24
  %30 = load i32, i32* %18, align 16
  %31 = icmp ult i32 %30, 41
  br i1 %31, label %32, label %36

32:                                               ; preds = %29
  %33 = zext i32 %30 to i64
  %34 = getelementptr i8, i8* %23, i64 %33
  %35 = add nuw nsw i32 %30, 8
  store i32 %35, i32* %18, align 16
  br label %39

36:                                               ; preds = %29
  %37 = load i8*, i8** %19, align 8
  %38 = getelementptr i8, i8* %37, i64 8
  store i8* %38, i8** %19, align 8
  br label %39

39:                                               ; preds = %36, %32
  %40 = phi i8* [ %34, %32 ], [ %37, %36 ]
  %41 = bitcast i8* %40 to i32*
  %42 = load i32, i32* %41, align 4
  %43 = trunc i32 %42 to i8
  %44 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %43, i8* %45, align 8, !tbaa !11
  br label %177

46:                                               ; preds = %24
  %47 = load i32, i32* %18, align 16
  %48 = icmp ult i32 %47, 41
  br i1 %48, label %49, label %53

49:                                               ; preds = %46
  %50 = zext i32 %47 to i64
  %51 = getelementptr i8, i8* %23, i64 %50
  %52 = add nuw nsw i32 %47, 8
  store i32 %52, i32* %18, align 16
  br label %56

53:                                               ; preds = %46
  %54 = load i8*, i8** %19, align 8
  %55 = getelementptr i8, i8* %54, i64 8
  store i8* %55, i8** %19, align 8
  br label %56

56:                                               ; preds = %53, %49
  %57 = phi i8* [ %51, %49 ], [ %54, %53 ]
  %58 = bitcast i8* %57 to i32*
  %59 = load i32, i32* %58, align 4
  %60 = trunc i32 %59 to i8
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %60, i8* %62, align 8, !tbaa !11
  br label %177

63:                                               ; preds = %24
  %64 = load i32, i32* %18, align 16
  %65 = icmp ult i32 %64, 41
  br i1 %65, label %66, label %70

66:                                               ; preds = %63
  %67 = zext i32 %64 to i64
  %68 = getelementptr i8, i8* %23, i64 %67
  %69 = add nuw nsw i32 %64, 8
  store i32 %69, i32* %18, align 16
  br label %73

70:                                               ; preds = %63
  %71 = load i8*, i8** %19, align 8
  %72 = getelementptr i8, i8* %71, i64 8
  store i8* %72, i8** %19, align 8
  br label %73

73:                                               ; preds = %70, %66
  %74 = phi i8* [ %68, %66 ], [ %71, %70 ]
  %75 = bitcast i8* %74 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %77, i16* %79, align 8, !tbaa !11
  br label %177

80:                                               ; preds = %24
  %81 = load i32, i32* %18, align 16
  %82 = icmp ult i32 %81, 41
  br i1 %82, label %83, label %87

83:                                               ; preds = %80
  %84 = zext i32 %81 to i64
  %85 = getelementptr i8, i8* %23, i64 %84
  %86 = add nuw nsw i32 %81, 8
  store i32 %86, i32* %18, align 16
  br label %90

87:                                               ; preds = %80
  %88 = load i8*, i8** %19, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %19, align 8
  br label %90

90:                                               ; preds = %87, %83
  %91 = phi i8* [ %85, %83 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %177

97:                                               ; preds = %24
  %98 = load i32, i32* %18, align 16
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %104

100:                                              ; preds = %97
  %101 = zext i32 %98 to i64
  %102 = getelementptr i8, i8* %23, i64 %101
  %103 = add nuw nsw i32 %98, 8
  store i32 %103, i32* %18, align 16
  br label %107

104:                                              ; preds = %97
  %105 = load i8*, i8** %19, align 8
  %106 = getelementptr i8, i8* %105, i64 8
  store i8* %106, i8** %19, align 8
  br label %107

107:                                              ; preds = %104, %100
  %108 = phi i8* [ %102, %100 ], [ %105, %104 ]
  %109 = bitcast i8* %108 to i32*
  %110 = load i32, i32* %109, align 4
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %112 = bitcast %union.jvalue* %111 to i32*
  store i32 %110, i32* %112, align 8, !tbaa !11
  br label %177

113:                                              ; preds = %24
  %114 = load i32, i32* %18, align 16
  %115 = icmp ult i32 %114, 41
  br i1 %115, label %116, label %120

116:                                              ; preds = %113
  %117 = zext i32 %114 to i64
  %118 = getelementptr i8, i8* %23, i64 %117
  %119 = add nuw nsw i32 %114, 8
  store i32 %119, i32* %18, align 16
  br label %123

120:                                              ; preds = %113
  %121 = load i8*, i8** %19, align 8
  %122 = getelementptr i8, i8* %121, i64 8
  store i8* %122, i8** %19, align 8
  br label %123

123:                                              ; preds = %120, %116
  %124 = phi i8* [ %118, %116 ], [ %121, %120 ]
  %125 = bitcast i8* %124 to i64*
  %126 = load i64, i64* %125, align 8
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25, i32 0
  store i64 %126, i64* %127, align 8, !tbaa !11
  br label %177

128:                                              ; preds = %24
  %129 = load i32, i32* %21, align 4
  %130 = icmp ult i32 %129, 161
  br i1 %130, label %131, label %135

131:                                              ; preds = %128
  %132 = zext i32 %129 to i64
  %133 = getelementptr i8, i8* %23, i64 %132
  %134 = add nuw nsw i32 %129, 16
  store i32 %134, i32* %21, align 4
  br label %138

135:                                              ; preds = %128
  %136 = load i8*, i8** %19, align 8
  %137 = getelementptr i8, i8* %136, i64 8
  store i8* %137, i8** %19, align 8
  br label %138

138:                                              ; preds = %135, %131
  %139 = phi i8* [ %133, %131 ], [ %136, %135 ]
  %140 = bitcast i8* %139 to double*
  %141 = load double, double* %140, align 8
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %143 = bitcast %union.jvalue* %142 to double*
  store double %141, double* %143, align 8, !tbaa !11
  br label %177

144:                                              ; preds = %24
  %145 = load i32, i32* %21, align 4
  %146 = icmp ult i32 %145, 161
  br i1 %146, label %147, label %151

147:                                              ; preds = %144
  %148 = zext i32 %145 to i64
  %149 = getelementptr i8, i8* %23, i64 %148
  %150 = add nuw nsw i32 %145, 16
  store i32 %150, i32* %21, align 4
  br label %154

151:                                              ; preds = %144
  %152 = load i8*, i8** %19, align 8
  %153 = getelementptr i8, i8* %152, i64 8
  store i8* %153, i8** %19, align 8
  br label %154

154:                                              ; preds = %151, %147
  %155 = phi i8* [ %149, %147 ], [ %152, %151 ]
  %156 = bitcast i8* %155 to double*
  %157 = load double, double* %156, align 8
  %158 = fptrunc double %157 to float
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %160 = bitcast %union.jvalue* %159 to float*
  store float %158, float* %160, align 8, !tbaa !11
  br label %177

161:                                              ; preds = %24
  %162 = load i32, i32* %18, align 16
  %163 = icmp ult i32 %162, 41
  br i1 %163, label %164, label %168

164:                                              ; preds = %161
  %165 = zext i32 %162 to i64
  %166 = getelementptr i8, i8* %23, i64 %165
  %167 = add nuw nsw i32 %162, 8
  store i32 %167, i32* %18, align 16
  br label %171

168:                                              ; preds = %161
  %169 = load i8*, i8** %19, align 8
  %170 = getelementptr i8, i8* %169, i64 8
  store i8* %170, i8** %19, align 8
  br label %171

171:                                              ; preds = %168, %164
  %172 = phi i8* [ %166, %164 ], [ %169, %168 ]
  %173 = bitcast i8* %172 to %struct._jobject**
  %174 = load %struct._jobject*, %struct._jobject** %173, align 8
  %175 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %176, align 8, !tbaa !11
  br label %177

177:                                              ; preds = %171, %154, %138, %123, %107, %90, %73, %56, %39, %24
  %178 = add nuw nsw i64 %25, 1
  %179 = icmp eq i64 %178, %22
  br i1 %179, label %180, label %24, !llvm.loop !27

180:                                              ; preds = %177, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %9) #4
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i64 0, i32 72
  %183 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8, !tbaa !28
  %184 = call signext i8 %183(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %15) #4
  call void @llvm.stackrestore(i8* %8)
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %7) #4
  ret i8 %184
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local signext i8 @JNI_CallNonvirtualByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %struct.__va_list_tag* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 16
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !5
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !9
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #4
  %12 = sext i32 %11 to i64
  %13 = alloca %union.jvalue, i64 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %186

15:                                               ; preds = %5
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 0
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 2
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 3
  %19 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 1
  %20 = zext i32 %11 to i64
  br label %21

21:                                               ; preds = %15, %183
  %22 = phi i64 [ 0, %15 ], [ %184, %183 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !11
  %25 = sext i8 %24 to i32
  switch i32 %25, label %183 [
    i32 90, label %26
    i32 66, label %44
    i32 83, label %62
    i32 67, label %80
    i32 73, label %98
    i32 74, label %115
    i32 68, label %131
    i32 70, label %148
    i32 76, label %166
  ]

26:                                               ; preds = %21
  %27 = load i32, i32* %16, align 8
  %28 = icmp ult i32 %27, 41
  br i1 %28, label %29, label %34

29:                                               ; preds = %26
  %30 = load i8*, i8** %18, align 8
  %31 = zext i32 %27 to i64
  %32 = getelementptr i8, i8* %30, i64 %31
  %33 = add nuw nsw i32 %27, 8
  store i32 %33, i32* %16, align 8
  br label %37

34:                                               ; preds = %26
  %35 = load i8*, i8** %17, align 8
  %36 = getelementptr i8, i8* %35, i64 8
  store i8* %36, i8** %17, align 8
  br label %37

37:                                               ; preds = %34, %29
  %38 = phi i8* [ %32, %29 ], [ %35, %34 ]
  %39 = bitcast i8* %38 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %41, i8* %43, align 8, !tbaa !11
  br label %183

44:                                               ; preds = %21
  %45 = load i32, i32* %16, align 8
  %46 = icmp ult i32 %45, 41
  br i1 %46, label %47, label %52

47:                                               ; preds = %44
  %48 = load i8*, i8** %18, align 8
  %49 = zext i32 %45 to i64
  %50 = getelementptr i8, i8* %48, i64 %49
  %51 = add nuw nsw i32 %45, 8
  store i32 %51, i32* %16, align 8
  br label %55

52:                                               ; preds = %44
  %53 = load i8*, i8** %17, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %17, align 8
  br label %55

55:                                               ; preds = %52, %47
  %56 = phi i8* [ %50, %47 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %183

62:                                               ; preds = %21
  %63 = load i32, i32* %16, align 8
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %70

65:                                               ; preds = %62
  %66 = load i8*, i8** %18, align 8
  %67 = zext i32 %63 to i64
  %68 = getelementptr i8, i8* %66, i64 %67
  %69 = add nuw nsw i32 %63, 8
  store i32 %69, i32* %16, align 8
  br label %73

70:                                               ; preds = %62
  %71 = load i8*, i8** %17, align 8
  %72 = getelementptr i8, i8* %71, i64 8
  store i8* %72, i8** %17, align 8
  br label %73

73:                                               ; preds = %70, %65
  %74 = phi i8* [ %68, %65 ], [ %71, %70 ]
  %75 = bitcast i8* %74 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %77, i16* %79, align 8, !tbaa !11
  br label %183

80:                                               ; preds = %21
  %81 = load i32, i32* %16, align 8
  %82 = icmp ult i32 %81, 41
  br i1 %82, label %83, label %88

83:                                               ; preds = %80
  %84 = load i8*, i8** %18, align 8
  %85 = zext i32 %81 to i64
  %86 = getelementptr i8, i8* %84, i64 %85
  %87 = add nuw nsw i32 %81, 8
  store i32 %87, i32* %16, align 8
  br label %91

88:                                               ; preds = %80
  %89 = load i8*, i8** %17, align 8
  %90 = getelementptr i8, i8* %89, i64 8
  store i8* %90, i8** %17, align 8
  br label %91

91:                                               ; preds = %88, %83
  %92 = phi i8* [ %86, %83 ], [ %89, %88 ]
  %93 = bitcast i8* %92 to i32*
  %94 = load i32, i32* %93, align 4
  %95 = and i32 %94, 65535
  %96 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %97 = bitcast %union.jvalue* %96 to i32*
  store i32 %95, i32* %97, align 8, !tbaa !11
  br label %183

98:                                               ; preds = %21
  %99 = load i32, i32* %16, align 8
  %100 = icmp ult i32 %99, 41
  br i1 %100, label %101, label %106

101:                                              ; preds = %98
  %102 = load i8*, i8** %18, align 8
  %103 = zext i32 %99 to i64
  %104 = getelementptr i8, i8* %102, i64 %103
  %105 = add nuw nsw i32 %99, 8
  store i32 %105, i32* %16, align 8
  br label %109

106:                                              ; preds = %98
  %107 = load i8*, i8** %17, align 8
  %108 = getelementptr i8, i8* %107, i64 8
  store i8* %108, i8** %17, align 8
  br label %109

109:                                              ; preds = %106, %101
  %110 = phi i8* [ %104, %101 ], [ %107, %106 ]
  %111 = bitcast i8* %110 to i32*
  %112 = load i32, i32* %111, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !11
  br label %183

115:                                              ; preds = %21
  %116 = load i32, i32* %16, align 8
  %117 = icmp ult i32 %116, 41
  br i1 %117, label %118, label %123

118:                                              ; preds = %115
  %119 = load i8*, i8** %18, align 8
  %120 = zext i32 %116 to i64
  %121 = getelementptr i8, i8* %119, i64 %120
  %122 = add nuw nsw i32 %116, 8
  store i32 %122, i32* %16, align 8
  br label %126

123:                                              ; preds = %115
  %124 = load i8*, i8** %17, align 8
  %125 = getelementptr i8, i8* %124, i64 8
  store i8* %125, i8** %17, align 8
  br label %126

126:                                              ; preds = %123, %118
  %127 = phi i8* [ %121, %118 ], [ %124, %123 ]
  %128 = bitcast i8* %127 to i64*
  %129 = load i64, i64* %128, align 8
  %130 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22, i32 0
  store i64 %129, i64* %130, align 8, !tbaa !11
  br label %183

131:                                              ; preds = %21
  %132 = load i32, i32* %19, align 4
  %133 = icmp ult i32 %132, 161
  br i1 %133, label %134, label %139

134:                                              ; preds = %131
  %135 = load i8*, i8** %18, align 8
  %136 = zext i32 %132 to i64
  %137 = getelementptr i8, i8* %135, i64 %136
  %138 = add nuw nsw i32 %132, 16
  store i32 %138, i32* %19, align 4
  br label %142

139:                                              ; preds = %131
  %140 = load i8*, i8** %17, align 8
  %141 = getelementptr i8, i8* %140, i64 8
  store i8* %141, i8** %17, align 8
  br label %142

142:                                              ; preds = %139, %134
  %143 = phi i8* [ %137, %134 ], [ %140, %139 ]
  %144 = bitcast i8* %143 to double*
  %145 = load double, double* %144, align 8
  %146 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %147 = bitcast %union.jvalue* %146 to double*
  store double %145, double* %147, align 8, !tbaa !11
  br label %183

148:                                              ; preds = %21
  %149 = load i32, i32* %19, align 4
  %150 = icmp ult i32 %149, 161
  br i1 %150, label %151, label %156

151:                                              ; preds = %148
  %152 = load i8*, i8** %18, align 8
  %153 = zext i32 %149 to i64
  %154 = getelementptr i8, i8* %152, i64 %153
  %155 = add nuw nsw i32 %149, 16
  store i32 %155, i32* %19, align 4
  br label %159

156:                                              ; preds = %148
  %157 = load i8*, i8** %17, align 8
  %158 = getelementptr i8, i8* %157, i64 8
  store i8* %158, i8** %17, align 8
  br label %159

159:                                              ; preds = %156, %151
  %160 = phi i8* [ %154, %151 ], [ %157, %156 ]
  %161 = bitcast i8* %160 to double*
  %162 = load double, double* %161, align 8
  %163 = fptrunc double %162 to float
  %164 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %165 = bitcast %union.jvalue* %164 to float*
  store float %163, float* %165, align 8, !tbaa !11
  br label %183

166:                                              ; preds = %21
  %167 = load i32, i32* %16, align 8
  %168 = icmp ult i32 %167, 41
  br i1 %168, label %169, label %174

169:                                              ; preds = %166
  %170 = load i8*, i8** %18, align 8
  %171 = zext i32 %167 to i64
  %172 = getelementptr i8, i8* %170, i64 %171
  %173 = add nuw nsw i32 %167, 8
  store i32 %173, i32* %16, align 8
  br label %177

174:                                              ; preds = %166
  %175 = load i8*, i8** %17, align 8
  %176 = getelementptr i8, i8* %175, i64 8
  store i8* %176, i8** %17, align 8
  br label %177

177:                                              ; preds = %174, %169
  %178 = phi i8* [ %172, %169 ], [ %175, %174 ]
  %179 = bitcast i8* %178 to %struct._jobject**
  %180 = load %struct._jobject*, %struct._jobject** %179, align 8
  %181 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %182 = bitcast %union.jvalue* %181 to %struct._jobject**
  store %struct._jobject* %180, %struct._jobject** %182, align 8, !tbaa !11
  br label %183

183:                                              ; preds = %37, %55, %73, %91, %109, %126, %142, %159, %177, %21
  %184 = add nuw nsw i64 %22, 1
  %185 = icmp eq i64 %184, %20
  br i1 %185, label %186, label %21, !llvm.loop !27

186:                                              ; preds = %183, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %187 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %188 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %187, i64 0, i32 72
  %189 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %188, align 8, !tbaa !28
  %190 = call signext i8 %189(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret i8 %190
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local signext i8 @JNI_CallStaticByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !29

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 122
  %182 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !30
  %183 = call signext i8 %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret i8 %183
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local signext i8 @JNI_CallStaticByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !29

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 122
  %188 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !30
  %189 = call signext i8 %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i8 %189
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local zeroext i16 @JNI_CallCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !31

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 45
  %182 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !32
  %183 = call zeroext i16 %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret i16 %183
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local zeroext i16 @JNI_CallCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !31

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 45
  %188 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !32
  %189 = call zeroext i16 %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i16 %189
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local zeroext i16 @JNI_CallNonvirtualCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = alloca [1 x %struct.__va_list_tag], align 16
  %7 = bitcast [1 x %struct.__va_list_tag]* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = call i8* @llvm.stacksave()
  %9 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %9) #4
  %10 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %10, align 8, !tbaa !5
  %12 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %11, align 8, !tbaa !9
  %13 = call i32 %12(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %9) #4
  %14 = sext i32 %13 to i64
  %15 = alloca %union.jvalue, i64 %14, align 16
  %16 = icmp sgt i32 %13, 0
  br i1 %16, label %17, label %180

17:                                               ; preds = %4
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 0
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 2
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 3
  %21 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 1
  %22 = zext i32 %13 to i64
  %23 = load i8*, i8** %20, align 16
  br label %24

24:                                               ; preds = %177, %17
  %25 = phi i64 [ 0, %17 ], [ %178, %177 ]
  %26 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %25
  %27 = load i8, i8* %26, align 1, !tbaa !11
  %28 = sext i8 %27 to i32
  switch i32 %28, label %177 [
    i32 90, label %29
    i32 66, label %46
    i32 83, label %63
    i32 67, label %80
    i32 73, label %97
    i32 74, label %113
    i32 68, label %128
    i32 70, label %144
    i32 76, label %161
  ]

29:                                               ; preds = %24
  %30 = load i32, i32* %18, align 16
  %31 = icmp ult i32 %30, 41
  br i1 %31, label %32, label %36

32:                                               ; preds = %29
  %33 = zext i32 %30 to i64
  %34 = getelementptr i8, i8* %23, i64 %33
  %35 = add nuw nsw i32 %30, 8
  store i32 %35, i32* %18, align 16
  br label %39

36:                                               ; preds = %29
  %37 = load i8*, i8** %19, align 8
  %38 = getelementptr i8, i8* %37, i64 8
  store i8* %38, i8** %19, align 8
  br label %39

39:                                               ; preds = %36, %32
  %40 = phi i8* [ %34, %32 ], [ %37, %36 ]
  %41 = bitcast i8* %40 to i32*
  %42 = load i32, i32* %41, align 4
  %43 = trunc i32 %42 to i8
  %44 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %43, i8* %45, align 8, !tbaa !11
  br label %177

46:                                               ; preds = %24
  %47 = load i32, i32* %18, align 16
  %48 = icmp ult i32 %47, 41
  br i1 %48, label %49, label %53

49:                                               ; preds = %46
  %50 = zext i32 %47 to i64
  %51 = getelementptr i8, i8* %23, i64 %50
  %52 = add nuw nsw i32 %47, 8
  store i32 %52, i32* %18, align 16
  br label %56

53:                                               ; preds = %46
  %54 = load i8*, i8** %19, align 8
  %55 = getelementptr i8, i8* %54, i64 8
  store i8* %55, i8** %19, align 8
  br label %56

56:                                               ; preds = %53, %49
  %57 = phi i8* [ %51, %49 ], [ %54, %53 ]
  %58 = bitcast i8* %57 to i32*
  %59 = load i32, i32* %58, align 4
  %60 = trunc i32 %59 to i8
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %60, i8* %62, align 8, !tbaa !11
  br label %177

63:                                               ; preds = %24
  %64 = load i32, i32* %18, align 16
  %65 = icmp ult i32 %64, 41
  br i1 %65, label %66, label %70

66:                                               ; preds = %63
  %67 = zext i32 %64 to i64
  %68 = getelementptr i8, i8* %23, i64 %67
  %69 = add nuw nsw i32 %64, 8
  store i32 %69, i32* %18, align 16
  br label %73

70:                                               ; preds = %63
  %71 = load i8*, i8** %19, align 8
  %72 = getelementptr i8, i8* %71, i64 8
  store i8* %72, i8** %19, align 8
  br label %73

73:                                               ; preds = %70, %66
  %74 = phi i8* [ %68, %66 ], [ %71, %70 ]
  %75 = bitcast i8* %74 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %77, i16* %79, align 8, !tbaa !11
  br label %177

80:                                               ; preds = %24
  %81 = load i32, i32* %18, align 16
  %82 = icmp ult i32 %81, 41
  br i1 %82, label %83, label %87

83:                                               ; preds = %80
  %84 = zext i32 %81 to i64
  %85 = getelementptr i8, i8* %23, i64 %84
  %86 = add nuw nsw i32 %81, 8
  store i32 %86, i32* %18, align 16
  br label %90

87:                                               ; preds = %80
  %88 = load i8*, i8** %19, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %19, align 8
  br label %90

90:                                               ; preds = %87, %83
  %91 = phi i8* [ %85, %83 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %177

97:                                               ; preds = %24
  %98 = load i32, i32* %18, align 16
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %104

100:                                              ; preds = %97
  %101 = zext i32 %98 to i64
  %102 = getelementptr i8, i8* %23, i64 %101
  %103 = add nuw nsw i32 %98, 8
  store i32 %103, i32* %18, align 16
  br label %107

104:                                              ; preds = %97
  %105 = load i8*, i8** %19, align 8
  %106 = getelementptr i8, i8* %105, i64 8
  store i8* %106, i8** %19, align 8
  br label %107

107:                                              ; preds = %104, %100
  %108 = phi i8* [ %102, %100 ], [ %105, %104 ]
  %109 = bitcast i8* %108 to i32*
  %110 = load i32, i32* %109, align 4
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %112 = bitcast %union.jvalue* %111 to i32*
  store i32 %110, i32* %112, align 8, !tbaa !11
  br label %177

113:                                              ; preds = %24
  %114 = load i32, i32* %18, align 16
  %115 = icmp ult i32 %114, 41
  br i1 %115, label %116, label %120

116:                                              ; preds = %113
  %117 = zext i32 %114 to i64
  %118 = getelementptr i8, i8* %23, i64 %117
  %119 = add nuw nsw i32 %114, 8
  store i32 %119, i32* %18, align 16
  br label %123

120:                                              ; preds = %113
  %121 = load i8*, i8** %19, align 8
  %122 = getelementptr i8, i8* %121, i64 8
  store i8* %122, i8** %19, align 8
  br label %123

123:                                              ; preds = %120, %116
  %124 = phi i8* [ %118, %116 ], [ %121, %120 ]
  %125 = bitcast i8* %124 to i64*
  %126 = load i64, i64* %125, align 8
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25, i32 0
  store i64 %126, i64* %127, align 8, !tbaa !11
  br label %177

128:                                              ; preds = %24
  %129 = load i32, i32* %21, align 4
  %130 = icmp ult i32 %129, 161
  br i1 %130, label %131, label %135

131:                                              ; preds = %128
  %132 = zext i32 %129 to i64
  %133 = getelementptr i8, i8* %23, i64 %132
  %134 = add nuw nsw i32 %129, 16
  store i32 %134, i32* %21, align 4
  br label %138

135:                                              ; preds = %128
  %136 = load i8*, i8** %19, align 8
  %137 = getelementptr i8, i8* %136, i64 8
  store i8* %137, i8** %19, align 8
  br label %138

138:                                              ; preds = %135, %131
  %139 = phi i8* [ %133, %131 ], [ %136, %135 ]
  %140 = bitcast i8* %139 to double*
  %141 = load double, double* %140, align 8
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %143 = bitcast %union.jvalue* %142 to double*
  store double %141, double* %143, align 8, !tbaa !11
  br label %177

144:                                              ; preds = %24
  %145 = load i32, i32* %21, align 4
  %146 = icmp ult i32 %145, 161
  br i1 %146, label %147, label %151

147:                                              ; preds = %144
  %148 = zext i32 %145 to i64
  %149 = getelementptr i8, i8* %23, i64 %148
  %150 = add nuw nsw i32 %145, 16
  store i32 %150, i32* %21, align 4
  br label %154

151:                                              ; preds = %144
  %152 = load i8*, i8** %19, align 8
  %153 = getelementptr i8, i8* %152, i64 8
  store i8* %153, i8** %19, align 8
  br label %154

154:                                              ; preds = %151, %147
  %155 = phi i8* [ %149, %147 ], [ %152, %151 ]
  %156 = bitcast i8* %155 to double*
  %157 = load double, double* %156, align 8
  %158 = fptrunc double %157 to float
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %160 = bitcast %union.jvalue* %159 to float*
  store float %158, float* %160, align 8, !tbaa !11
  br label %177

161:                                              ; preds = %24
  %162 = load i32, i32* %18, align 16
  %163 = icmp ult i32 %162, 41
  br i1 %163, label %164, label %168

164:                                              ; preds = %161
  %165 = zext i32 %162 to i64
  %166 = getelementptr i8, i8* %23, i64 %165
  %167 = add nuw nsw i32 %162, 8
  store i32 %167, i32* %18, align 16
  br label %171

168:                                              ; preds = %161
  %169 = load i8*, i8** %19, align 8
  %170 = getelementptr i8, i8* %169, i64 8
  store i8* %170, i8** %19, align 8
  br label %171

171:                                              ; preds = %168, %164
  %172 = phi i8* [ %166, %164 ], [ %169, %168 ]
  %173 = bitcast i8* %172 to %struct._jobject**
  %174 = load %struct._jobject*, %struct._jobject** %173, align 8
  %175 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %176, align 8, !tbaa !11
  br label %177

177:                                              ; preds = %171, %154, %138, %123, %107, %90, %73, %56, %39, %24
  %178 = add nuw nsw i64 %25, 1
  %179 = icmp eq i64 %178, %22
  br i1 %179, label %180, label %24, !llvm.loop !33

180:                                              ; preds = %177, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %9) #4
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i64 0, i32 75
  %183 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8, !tbaa !34
  %184 = call zeroext i16 %183(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %15) #4
  call void @llvm.stackrestore(i8* %8)
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %7) #4
  ret i16 %184
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local zeroext i16 @JNI_CallNonvirtualCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %struct.__va_list_tag* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 16
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !5
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !9
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #4
  %12 = sext i32 %11 to i64
  %13 = alloca %union.jvalue, i64 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %186

15:                                               ; preds = %5
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 0
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 2
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 3
  %19 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 1
  %20 = zext i32 %11 to i64
  br label %21

21:                                               ; preds = %15, %183
  %22 = phi i64 [ 0, %15 ], [ %184, %183 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !11
  %25 = sext i8 %24 to i32
  switch i32 %25, label %183 [
    i32 90, label %26
    i32 66, label %44
    i32 83, label %62
    i32 67, label %80
    i32 73, label %98
    i32 74, label %115
    i32 68, label %131
    i32 70, label %148
    i32 76, label %166
  ]

26:                                               ; preds = %21
  %27 = load i32, i32* %16, align 8
  %28 = icmp ult i32 %27, 41
  br i1 %28, label %29, label %34

29:                                               ; preds = %26
  %30 = load i8*, i8** %18, align 8
  %31 = zext i32 %27 to i64
  %32 = getelementptr i8, i8* %30, i64 %31
  %33 = add nuw nsw i32 %27, 8
  store i32 %33, i32* %16, align 8
  br label %37

34:                                               ; preds = %26
  %35 = load i8*, i8** %17, align 8
  %36 = getelementptr i8, i8* %35, i64 8
  store i8* %36, i8** %17, align 8
  br label %37

37:                                               ; preds = %34, %29
  %38 = phi i8* [ %32, %29 ], [ %35, %34 ]
  %39 = bitcast i8* %38 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %41, i8* %43, align 8, !tbaa !11
  br label %183

44:                                               ; preds = %21
  %45 = load i32, i32* %16, align 8
  %46 = icmp ult i32 %45, 41
  br i1 %46, label %47, label %52

47:                                               ; preds = %44
  %48 = load i8*, i8** %18, align 8
  %49 = zext i32 %45 to i64
  %50 = getelementptr i8, i8* %48, i64 %49
  %51 = add nuw nsw i32 %45, 8
  store i32 %51, i32* %16, align 8
  br label %55

52:                                               ; preds = %44
  %53 = load i8*, i8** %17, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %17, align 8
  br label %55

55:                                               ; preds = %52, %47
  %56 = phi i8* [ %50, %47 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %183

62:                                               ; preds = %21
  %63 = load i32, i32* %16, align 8
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %70

65:                                               ; preds = %62
  %66 = load i8*, i8** %18, align 8
  %67 = zext i32 %63 to i64
  %68 = getelementptr i8, i8* %66, i64 %67
  %69 = add nuw nsw i32 %63, 8
  store i32 %69, i32* %16, align 8
  br label %73

70:                                               ; preds = %62
  %71 = load i8*, i8** %17, align 8
  %72 = getelementptr i8, i8* %71, i64 8
  store i8* %72, i8** %17, align 8
  br label %73

73:                                               ; preds = %70, %65
  %74 = phi i8* [ %68, %65 ], [ %71, %70 ]
  %75 = bitcast i8* %74 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %77, i16* %79, align 8, !tbaa !11
  br label %183

80:                                               ; preds = %21
  %81 = load i32, i32* %16, align 8
  %82 = icmp ult i32 %81, 41
  br i1 %82, label %83, label %88

83:                                               ; preds = %80
  %84 = load i8*, i8** %18, align 8
  %85 = zext i32 %81 to i64
  %86 = getelementptr i8, i8* %84, i64 %85
  %87 = add nuw nsw i32 %81, 8
  store i32 %87, i32* %16, align 8
  br label %91

88:                                               ; preds = %80
  %89 = load i8*, i8** %17, align 8
  %90 = getelementptr i8, i8* %89, i64 8
  store i8* %90, i8** %17, align 8
  br label %91

91:                                               ; preds = %88, %83
  %92 = phi i8* [ %86, %83 ], [ %89, %88 ]
  %93 = bitcast i8* %92 to i32*
  %94 = load i32, i32* %93, align 4
  %95 = and i32 %94, 65535
  %96 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %97 = bitcast %union.jvalue* %96 to i32*
  store i32 %95, i32* %97, align 8, !tbaa !11
  br label %183

98:                                               ; preds = %21
  %99 = load i32, i32* %16, align 8
  %100 = icmp ult i32 %99, 41
  br i1 %100, label %101, label %106

101:                                              ; preds = %98
  %102 = load i8*, i8** %18, align 8
  %103 = zext i32 %99 to i64
  %104 = getelementptr i8, i8* %102, i64 %103
  %105 = add nuw nsw i32 %99, 8
  store i32 %105, i32* %16, align 8
  br label %109

106:                                              ; preds = %98
  %107 = load i8*, i8** %17, align 8
  %108 = getelementptr i8, i8* %107, i64 8
  store i8* %108, i8** %17, align 8
  br label %109

109:                                              ; preds = %106, %101
  %110 = phi i8* [ %104, %101 ], [ %107, %106 ]
  %111 = bitcast i8* %110 to i32*
  %112 = load i32, i32* %111, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !11
  br label %183

115:                                              ; preds = %21
  %116 = load i32, i32* %16, align 8
  %117 = icmp ult i32 %116, 41
  br i1 %117, label %118, label %123

118:                                              ; preds = %115
  %119 = load i8*, i8** %18, align 8
  %120 = zext i32 %116 to i64
  %121 = getelementptr i8, i8* %119, i64 %120
  %122 = add nuw nsw i32 %116, 8
  store i32 %122, i32* %16, align 8
  br label %126

123:                                              ; preds = %115
  %124 = load i8*, i8** %17, align 8
  %125 = getelementptr i8, i8* %124, i64 8
  store i8* %125, i8** %17, align 8
  br label %126

126:                                              ; preds = %123, %118
  %127 = phi i8* [ %121, %118 ], [ %124, %123 ]
  %128 = bitcast i8* %127 to i64*
  %129 = load i64, i64* %128, align 8
  %130 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22, i32 0
  store i64 %129, i64* %130, align 8, !tbaa !11
  br label %183

131:                                              ; preds = %21
  %132 = load i32, i32* %19, align 4
  %133 = icmp ult i32 %132, 161
  br i1 %133, label %134, label %139

134:                                              ; preds = %131
  %135 = load i8*, i8** %18, align 8
  %136 = zext i32 %132 to i64
  %137 = getelementptr i8, i8* %135, i64 %136
  %138 = add nuw nsw i32 %132, 16
  store i32 %138, i32* %19, align 4
  br label %142

139:                                              ; preds = %131
  %140 = load i8*, i8** %17, align 8
  %141 = getelementptr i8, i8* %140, i64 8
  store i8* %141, i8** %17, align 8
  br label %142

142:                                              ; preds = %139, %134
  %143 = phi i8* [ %137, %134 ], [ %140, %139 ]
  %144 = bitcast i8* %143 to double*
  %145 = load double, double* %144, align 8
  %146 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %147 = bitcast %union.jvalue* %146 to double*
  store double %145, double* %147, align 8, !tbaa !11
  br label %183

148:                                              ; preds = %21
  %149 = load i32, i32* %19, align 4
  %150 = icmp ult i32 %149, 161
  br i1 %150, label %151, label %156

151:                                              ; preds = %148
  %152 = load i8*, i8** %18, align 8
  %153 = zext i32 %149 to i64
  %154 = getelementptr i8, i8* %152, i64 %153
  %155 = add nuw nsw i32 %149, 16
  store i32 %155, i32* %19, align 4
  br label %159

156:                                              ; preds = %148
  %157 = load i8*, i8** %17, align 8
  %158 = getelementptr i8, i8* %157, i64 8
  store i8* %158, i8** %17, align 8
  br label %159

159:                                              ; preds = %156, %151
  %160 = phi i8* [ %154, %151 ], [ %157, %156 ]
  %161 = bitcast i8* %160 to double*
  %162 = load double, double* %161, align 8
  %163 = fptrunc double %162 to float
  %164 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %165 = bitcast %union.jvalue* %164 to float*
  store float %163, float* %165, align 8, !tbaa !11
  br label %183

166:                                              ; preds = %21
  %167 = load i32, i32* %16, align 8
  %168 = icmp ult i32 %167, 41
  br i1 %168, label %169, label %174

169:                                              ; preds = %166
  %170 = load i8*, i8** %18, align 8
  %171 = zext i32 %167 to i64
  %172 = getelementptr i8, i8* %170, i64 %171
  %173 = add nuw nsw i32 %167, 8
  store i32 %173, i32* %16, align 8
  br label %177

174:                                              ; preds = %166
  %175 = load i8*, i8** %17, align 8
  %176 = getelementptr i8, i8* %175, i64 8
  store i8* %176, i8** %17, align 8
  br label %177

177:                                              ; preds = %174, %169
  %178 = phi i8* [ %172, %169 ], [ %175, %174 ]
  %179 = bitcast i8* %178 to %struct._jobject**
  %180 = load %struct._jobject*, %struct._jobject** %179, align 8
  %181 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %182 = bitcast %union.jvalue* %181 to %struct._jobject**
  store %struct._jobject* %180, %struct._jobject** %182, align 8, !tbaa !11
  br label %183

183:                                              ; preds = %37, %55, %73, %91, %109, %126, %142, %159, %177, %21
  %184 = add nuw nsw i64 %22, 1
  %185 = icmp eq i64 %184, %20
  br i1 %185, label %186, label %21, !llvm.loop !33

186:                                              ; preds = %183, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %187 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %188 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %187, i64 0, i32 75
  %189 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %188, align 8, !tbaa !34
  %190 = call zeroext i16 %189(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret i16 %190
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local zeroext i16 @JNI_CallStaticCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !35

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 125
  %182 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !36
  %183 = call zeroext i16 %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret i16 %183
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local zeroext i16 @JNI_CallStaticCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !35

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 125
  %188 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !36
  %189 = call zeroext i16 %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i16 %189
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local signext i16 @JNI_CallShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !37

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 48
  %182 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !38
  %183 = call signext i16 %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret i16 %183
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local signext i16 @JNI_CallShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !37

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 48
  %188 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !38
  %189 = call signext i16 %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i16 %189
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local signext i16 @JNI_CallNonvirtualShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = alloca [1 x %struct.__va_list_tag], align 16
  %7 = bitcast [1 x %struct.__va_list_tag]* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = call i8* @llvm.stacksave()
  %9 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %9) #4
  %10 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %10, align 8, !tbaa !5
  %12 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %11, align 8, !tbaa !9
  %13 = call i32 %12(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %9) #4
  %14 = sext i32 %13 to i64
  %15 = alloca %union.jvalue, i64 %14, align 16
  %16 = icmp sgt i32 %13, 0
  br i1 %16, label %17, label %180

17:                                               ; preds = %4
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 0
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 2
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 3
  %21 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 1
  %22 = zext i32 %13 to i64
  %23 = load i8*, i8** %20, align 16
  br label %24

24:                                               ; preds = %177, %17
  %25 = phi i64 [ 0, %17 ], [ %178, %177 ]
  %26 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %25
  %27 = load i8, i8* %26, align 1, !tbaa !11
  %28 = sext i8 %27 to i32
  switch i32 %28, label %177 [
    i32 90, label %29
    i32 66, label %46
    i32 83, label %63
    i32 67, label %80
    i32 73, label %97
    i32 74, label %113
    i32 68, label %128
    i32 70, label %144
    i32 76, label %161
  ]

29:                                               ; preds = %24
  %30 = load i32, i32* %18, align 16
  %31 = icmp ult i32 %30, 41
  br i1 %31, label %32, label %36

32:                                               ; preds = %29
  %33 = zext i32 %30 to i64
  %34 = getelementptr i8, i8* %23, i64 %33
  %35 = add nuw nsw i32 %30, 8
  store i32 %35, i32* %18, align 16
  br label %39

36:                                               ; preds = %29
  %37 = load i8*, i8** %19, align 8
  %38 = getelementptr i8, i8* %37, i64 8
  store i8* %38, i8** %19, align 8
  br label %39

39:                                               ; preds = %36, %32
  %40 = phi i8* [ %34, %32 ], [ %37, %36 ]
  %41 = bitcast i8* %40 to i32*
  %42 = load i32, i32* %41, align 4
  %43 = trunc i32 %42 to i8
  %44 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %43, i8* %45, align 8, !tbaa !11
  br label %177

46:                                               ; preds = %24
  %47 = load i32, i32* %18, align 16
  %48 = icmp ult i32 %47, 41
  br i1 %48, label %49, label %53

49:                                               ; preds = %46
  %50 = zext i32 %47 to i64
  %51 = getelementptr i8, i8* %23, i64 %50
  %52 = add nuw nsw i32 %47, 8
  store i32 %52, i32* %18, align 16
  br label %56

53:                                               ; preds = %46
  %54 = load i8*, i8** %19, align 8
  %55 = getelementptr i8, i8* %54, i64 8
  store i8* %55, i8** %19, align 8
  br label %56

56:                                               ; preds = %53, %49
  %57 = phi i8* [ %51, %49 ], [ %54, %53 ]
  %58 = bitcast i8* %57 to i32*
  %59 = load i32, i32* %58, align 4
  %60 = trunc i32 %59 to i8
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %60, i8* %62, align 8, !tbaa !11
  br label %177

63:                                               ; preds = %24
  %64 = load i32, i32* %18, align 16
  %65 = icmp ult i32 %64, 41
  br i1 %65, label %66, label %70

66:                                               ; preds = %63
  %67 = zext i32 %64 to i64
  %68 = getelementptr i8, i8* %23, i64 %67
  %69 = add nuw nsw i32 %64, 8
  store i32 %69, i32* %18, align 16
  br label %73

70:                                               ; preds = %63
  %71 = load i8*, i8** %19, align 8
  %72 = getelementptr i8, i8* %71, i64 8
  store i8* %72, i8** %19, align 8
  br label %73

73:                                               ; preds = %70, %66
  %74 = phi i8* [ %68, %66 ], [ %71, %70 ]
  %75 = bitcast i8* %74 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %77, i16* %79, align 8, !tbaa !11
  br label %177

80:                                               ; preds = %24
  %81 = load i32, i32* %18, align 16
  %82 = icmp ult i32 %81, 41
  br i1 %82, label %83, label %87

83:                                               ; preds = %80
  %84 = zext i32 %81 to i64
  %85 = getelementptr i8, i8* %23, i64 %84
  %86 = add nuw nsw i32 %81, 8
  store i32 %86, i32* %18, align 16
  br label %90

87:                                               ; preds = %80
  %88 = load i8*, i8** %19, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %19, align 8
  br label %90

90:                                               ; preds = %87, %83
  %91 = phi i8* [ %85, %83 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %177

97:                                               ; preds = %24
  %98 = load i32, i32* %18, align 16
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %104

100:                                              ; preds = %97
  %101 = zext i32 %98 to i64
  %102 = getelementptr i8, i8* %23, i64 %101
  %103 = add nuw nsw i32 %98, 8
  store i32 %103, i32* %18, align 16
  br label %107

104:                                              ; preds = %97
  %105 = load i8*, i8** %19, align 8
  %106 = getelementptr i8, i8* %105, i64 8
  store i8* %106, i8** %19, align 8
  br label %107

107:                                              ; preds = %104, %100
  %108 = phi i8* [ %102, %100 ], [ %105, %104 ]
  %109 = bitcast i8* %108 to i32*
  %110 = load i32, i32* %109, align 4
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %112 = bitcast %union.jvalue* %111 to i32*
  store i32 %110, i32* %112, align 8, !tbaa !11
  br label %177

113:                                              ; preds = %24
  %114 = load i32, i32* %18, align 16
  %115 = icmp ult i32 %114, 41
  br i1 %115, label %116, label %120

116:                                              ; preds = %113
  %117 = zext i32 %114 to i64
  %118 = getelementptr i8, i8* %23, i64 %117
  %119 = add nuw nsw i32 %114, 8
  store i32 %119, i32* %18, align 16
  br label %123

120:                                              ; preds = %113
  %121 = load i8*, i8** %19, align 8
  %122 = getelementptr i8, i8* %121, i64 8
  store i8* %122, i8** %19, align 8
  br label %123

123:                                              ; preds = %120, %116
  %124 = phi i8* [ %118, %116 ], [ %121, %120 ]
  %125 = bitcast i8* %124 to i64*
  %126 = load i64, i64* %125, align 8
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25, i32 0
  store i64 %126, i64* %127, align 8, !tbaa !11
  br label %177

128:                                              ; preds = %24
  %129 = load i32, i32* %21, align 4
  %130 = icmp ult i32 %129, 161
  br i1 %130, label %131, label %135

131:                                              ; preds = %128
  %132 = zext i32 %129 to i64
  %133 = getelementptr i8, i8* %23, i64 %132
  %134 = add nuw nsw i32 %129, 16
  store i32 %134, i32* %21, align 4
  br label %138

135:                                              ; preds = %128
  %136 = load i8*, i8** %19, align 8
  %137 = getelementptr i8, i8* %136, i64 8
  store i8* %137, i8** %19, align 8
  br label %138

138:                                              ; preds = %135, %131
  %139 = phi i8* [ %133, %131 ], [ %136, %135 ]
  %140 = bitcast i8* %139 to double*
  %141 = load double, double* %140, align 8
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %143 = bitcast %union.jvalue* %142 to double*
  store double %141, double* %143, align 8, !tbaa !11
  br label %177

144:                                              ; preds = %24
  %145 = load i32, i32* %21, align 4
  %146 = icmp ult i32 %145, 161
  br i1 %146, label %147, label %151

147:                                              ; preds = %144
  %148 = zext i32 %145 to i64
  %149 = getelementptr i8, i8* %23, i64 %148
  %150 = add nuw nsw i32 %145, 16
  store i32 %150, i32* %21, align 4
  br label %154

151:                                              ; preds = %144
  %152 = load i8*, i8** %19, align 8
  %153 = getelementptr i8, i8* %152, i64 8
  store i8* %153, i8** %19, align 8
  br label %154

154:                                              ; preds = %151, %147
  %155 = phi i8* [ %149, %147 ], [ %152, %151 ]
  %156 = bitcast i8* %155 to double*
  %157 = load double, double* %156, align 8
  %158 = fptrunc double %157 to float
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %160 = bitcast %union.jvalue* %159 to float*
  store float %158, float* %160, align 8, !tbaa !11
  br label %177

161:                                              ; preds = %24
  %162 = load i32, i32* %18, align 16
  %163 = icmp ult i32 %162, 41
  br i1 %163, label %164, label %168

164:                                              ; preds = %161
  %165 = zext i32 %162 to i64
  %166 = getelementptr i8, i8* %23, i64 %165
  %167 = add nuw nsw i32 %162, 8
  store i32 %167, i32* %18, align 16
  br label %171

168:                                              ; preds = %161
  %169 = load i8*, i8** %19, align 8
  %170 = getelementptr i8, i8* %169, i64 8
  store i8* %170, i8** %19, align 8
  br label %171

171:                                              ; preds = %168, %164
  %172 = phi i8* [ %166, %164 ], [ %169, %168 ]
  %173 = bitcast i8* %172 to %struct._jobject**
  %174 = load %struct._jobject*, %struct._jobject** %173, align 8
  %175 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %176, align 8, !tbaa !11
  br label %177

177:                                              ; preds = %171, %154, %138, %123, %107, %90, %73, %56, %39, %24
  %178 = add nuw nsw i64 %25, 1
  %179 = icmp eq i64 %178, %22
  br i1 %179, label %180, label %24, !llvm.loop !39

180:                                              ; preds = %177, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %9) #4
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i64 0, i32 78
  %183 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8, !tbaa !40
  %184 = call signext i16 %183(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %15) #4
  call void @llvm.stackrestore(i8* %8)
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %7) #4
  ret i16 %184
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local signext i16 @JNI_CallNonvirtualShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %struct.__va_list_tag* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 16
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !5
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !9
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #4
  %12 = sext i32 %11 to i64
  %13 = alloca %union.jvalue, i64 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %186

15:                                               ; preds = %5
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 0
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 2
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 3
  %19 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 1
  %20 = zext i32 %11 to i64
  br label %21

21:                                               ; preds = %15, %183
  %22 = phi i64 [ 0, %15 ], [ %184, %183 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !11
  %25 = sext i8 %24 to i32
  switch i32 %25, label %183 [
    i32 90, label %26
    i32 66, label %44
    i32 83, label %62
    i32 67, label %80
    i32 73, label %98
    i32 74, label %115
    i32 68, label %131
    i32 70, label %148
    i32 76, label %166
  ]

26:                                               ; preds = %21
  %27 = load i32, i32* %16, align 8
  %28 = icmp ult i32 %27, 41
  br i1 %28, label %29, label %34

29:                                               ; preds = %26
  %30 = load i8*, i8** %18, align 8
  %31 = zext i32 %27 to i64
  %32 = getelementptr i8, i8* %30, i64 %31
  %33 = add nuw nsw i32 %27, 8
  store i32 %33, i32* %16, align 8
  br label %37

34:                                               ; preds = %26
  %35 = load i8*, i8** %17, align 8
  %36 = getelementptr i8, i8* %35, i64 8
  store i8* %36, i8** %17, align 8
  br label %37

37:                                               ; preds = %34, %29
  %38 = phi i8* [ %32, %29 ], [ %35, %34 ]
  %39 = bitcast i8* %38 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %41, i8* %43, align 8, !tbaa !11
  br label %183

44:                                               ; preds = %21
  %45 = load i32, i32* %16, align 8
  %46 = icmp ult i32 %45, 41
  br i1 %46, label %47, label %52

47:                                               ; preds = %44
  %48 = load i8*, i8** %18, align 8
  %49 = zext i32 %45 to i64
  %50 = getelementptr i8, i8* %48, i64 %49
  %51 = add nuw nsw i32 %45, 8
  store i32 %51, i32* %16, align 8
  br label %55

52:                                               ; preds = %44
  %53 = load i8*, i8** %17, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %17, align 8
  br label %55

55:                                               ; preds = %52, %47
  %56 = phi i8* [ %50, %47 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %183

62:                                               ; preds = %21
  %63 = load i32, i32* %16, align 8
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %70

65:                                               ; preds = %62
  %66 = load i8*, i8** %18, align 8
  %67 = zext i32 %63 to i64
  %68 = getelementptr i8, i8* %66, i64 %67
  %69 = add nuw nsw i32 %63, 8
  store i32 %69, i32* %16, align 8
  br label %73

70:                                               ; preds = %62
  %71 = load i8*, i8** %17, align 8
  %72 = getelementptr i8, i8* %71, i64 8
  store i8* %72, i8** %17, align 8
  br label %73

73:                                               ; preds = %70, %65
  %74 = phi i8* [ %68, %65 ], [ %71, %70 ]
  %75 = bitcast i8* %74 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %77, i16* %79, align 8, !tbaa !11
  br label %183

80:                                               ; preds = %21
  %81 = load i32, i32* %16, align 8
  %82 = icmp ult i32 %81, 41
  br i1 %82, label %83, label %88

83:                                               ; preds = %80
  %84 = load i8*, i8** %18, align 8
  %85 = zext i32 %81 to i64
  %86 = getelementptr i8, i8* %84, i64 %85
  %87 = add nuw nsw i32 %81, 8
  store i32 %87, i32* %16, align 8
  br label %91

88:                                               ; preds = %80
  %89 = load i8*, i8** %17, align 8
  %90 = getelementptr i8, i8* %89, i64 8
  store i8* %90, i8** %17, align 8
  br label %91

91:                                               ; preds = %88, %83
  %92 = phi i8* [ %86, %83 ], [ %89, %88 ]
  %93 = bitcast i8* %92 to i32*
  %94 = load i32, i32* %93, align 4
  %95 = and i32 %94, 65535
  %96 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %97 = bitcast %union.jvalue* %96 to i32*
  store i32 %95, i32* %97, align 8, !tbaa !11
  br label %183

98:                                               ; preds = %21
  %99 = load i32, i32* %16, align 8
  %100 = icmp ult i32 %99, 41
  br i1 %100, label %101, label %106

101:                                              ; preds = %98
  %102 = load i8*, i8** %18, align 8
  %103 = zext i32 %99 to i64
  %104 = getelementptr i8, i8* %102, i64 %103
  %105 = add nuw nsw i32 %99, 8
  store i32 %105, i32* %16, align 8
  br label %109

106:                                              ; preds = %98
  %107 = load i8*, i8** %17, align 8
  %108 = getelementptr i8, i8* %107, i64 8
  store i8* %108, i8** %17, align 8
  br label %109

109:                                              ; preds = %106, %101
  %110 = phi i8* [ %104, %101 ], [ %107, %106 ]
  %111 = bitcast i8* %110 to i32*
  %112 = load i32, i32* %111, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !11
  br label %183

115:                                              ; preds = %21
  %116 = load i32, i32* %16, align 8
  %117 = icmp ult i32 %116, 41
  br i1 %117, label %118, label %123

118:                                              ; preds = %115
  %119 = load i8*, i8** %18, align 8
  %120 = zext i32 %116 to i64
  %121 = getelementptr i8, i8* %119, i64 %120
  %122 = add nuw nsw i32 %116, 8
  store i32 %122, i32* %16, align 8
  br label %126

123:                                              ; preds = %115
  %124 = load i8*, i8** %17, align 8
  %125 = getelementptr i8, i8* %124, i64 8
  store i8* %125, i8** %17, align 8
  br label %126

126:                                              ; preds = %123, %118
  %127 = phi i8* [ %121, %118 ], [ %124, %123 ]
  %128 = bitcast i8* %127 to i64*
  %129 = load i64, i64* %128, align 8
  %130 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22, i32 0
  store i64 %129, i64* %130, align 8, !tbaa !11
  br label %183

131:                                              ; preds = %21
  %132 = load i32, i32* %19, align 4
  %133 = icmp ult i32 %132, 161
  br i1 %133, label %134, label %139

134:                                              ; preds = %131
  %135 = load i8*, i8** %18, align 8
  %136 = zext i32 %132 to i64
  %137 = getelementptr i8, i8* %135, i64 %136
  %138 = add nuw nsw i32 %132, 16
  store i32 %138, i32* %19, align 4
  br label %142

139:                                              ; preds = %131
  %140 = load i8*, i8** %17, align 8
  %141 = getelementptr i8, i8* %140, i64 8
  store i8* %141, i8** %17, align 8
  br label %142

142:                                              ; preds = %139, %134
  %143 = phi i8* [ %137, %134 ], [ %140, %139 ]
  %144 = bitcast i8* %143 to double*
  %145 = load double, double* %144, align 8
  %146 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %147 = bitcast %union.jvalue* %146 to double*
  store double %145, double* %147, align 8, !tbaa !11
  br label %183

148:                                              ; preds = %21
  %149 = load i32, i32* %19, align 4
  %150 = icmp ult i32 %149, 161
  br i1 %150, label %151, label %156

151:                                              ; preds = %148
  %152 = load i8*, i8** %18, align 8
  %153 = zext i32 %149 to i64
  %154 = getelementptr i8, i8* %152, i64 %153
  %155 = add nuw nsw i32 %149, 16
  store i32 %155, i32* %19, align 4
  br label %159

156:                                              ; preds = %148
  %157 = load i8*, i8** %17, align 8
  %158 = getelementptr i8, i8* %157, i64 8
  store i8* %158, i8** %17, align 8
  br label %159

159:                                              ; preds = %156, %151
  %160 = phi i8* [ %154, %151 ], [ %157, %156 ]
  %161 = bitcast i8* %160 to double*
  %162 = load double, double* %161, align 8
  %163 = fptrunc double %162 to float
  %164 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %165 = bitcast %union.jvalue* %164 to float*
  store float %163, float* %165, align 8, !tbaa !11
  br label %183

166:                                              ; preds = %21
  %167 = load i32, i32* %16, align 8
  %168 = icmp ult i32 %167, 41
  br i1 %168, label %169, label %174

169:                                              ; preds = %166
  %170 = load i8*, i8** %18, align 8
  %171 = zext i32 %167 to i64
  %172 = getelementptr i8, i8* %170, i64 %171
  %173 = add nuw nsw i32 %167, 8
  store i32 %173, i32* %16, align 8
  br label %177

174:                                              ; preds = %166
  %175 = load i8*, i8** %17, align 8
  %176 = getelementptr i8, i8* %175, i64 8
  store i8* %176, i8** %17, align 8
  br label %177

177:                                              ; preds = %174, %169
  %178 = phi i8* [ %172, %169 ], [ %175, %174 ]
  %179 = bitcast i8* %178 to %struct._jobject**
  %180 = load %struct._jobject*, %struct._jobject** %179, align 8
  %181 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %182 = bitcast %union.jvalue* %181 to %struct._jobject**
  store %struct._jobject* %180, %struct._jobject** %182, align 8, !tbaa !11
  br label %183

183:                                              ; preds = %37, %55, %73, %91, %109, %126, %142, %159, %177, %21
  %184 = add nuw nsw i64 %22, 1
  %185 = icmp eq i64 %184, %20
  br i1 %185, label %186, label %21, !llvm.loop !39

186:                                              ; preds = %183, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %187 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %188 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %187, i64 0, i32 78
  %189 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %188, align 8, !tbaa !40
  %190 = call signext i16 %189(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret i16 %190
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local signext i16 @JNI_CallStaticShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !41

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 128
  %182 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !42
  %183 = call signext i16 %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret i16 %183
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local signext i16 @JNI_CallStaticShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !41

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 128
  %188 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !42
  %189 = call signext i16 %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i16 %189
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i32 @JNI_CallIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !43

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 51
  %182 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !44
  %183 = call i32 %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret i32 %183
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i32 @JNI_CallIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !43

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 51
  %188 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !44
  %189 = call i32 %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i32 %189
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i32 @JNI_CallNonvirtualIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = alloca [1 x %struct.__va_list_tag], align 16
  %7 = bitcast [1 x %struct.__va_list_tag]* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = call i8* @llvm.stacksave()
  %9 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %9) #4
  %10 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %10, align 8, !tbaa !5
  %12 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %11, align 8, !tbaa !9
  %13 = call i32 %12(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %9) #4
  %14 = sext i32 %13 to i64
  %15 = alloca %union.jvalue, i64 %14, align 16
  %16 = icmp sgt i32 %13, 0
  br i1 %16, label %17, label %180

17:                                               ; preds = %4
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 0
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 2
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 3
  %21 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 1
  %22 = zext i32 %13 to i64
  %23 = load i8*, i8** %20, align 16
  br label %24

24:                                               ; preds = %177, %17
  %25 = phi i64 [ 0, %17 ], [ %178, %177 ]
  %26 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %25
  %27 = load i8, i8* %26, align 1, !tbaa !11
  %28 = sext i8 %27 to i32
  switch i32 %28, label %177 [
    i32 90, label %29
    i32 66, label %46
    i32 83, label %63
    i32 67, label %80
    i32 73, label %97
    i32 74, label %113
    i32 68, label %128
    i32 70, label %144
    i32 76, label %161
  ]

29:                                               ; preds = %24
  %30 = load i32, i32* %18, align 16
  %31 = icmp ult i32 %30, 41
  br i1 %31, label %32, label %36

32:                                               ; preds = %29
  %33 = zext i32 %30 to i64
  %34 = getelementptr i8, i8* %23, i64 %33
  %35 = add nuw nsw i32 %30, 8
  store i32 %35, i32* %18, align 16
  br label %39

36:                                               ; preds = %29
  %37 = load i8*, i8** %19, align 8
  %38 = getelementptr i8, i8* %37, i64 8
  store i8* %38, i8** %19, align 8
  br label %39

39:                                               ; preds = %36, %32
  %40 = phi i8* [ %34, %32 ], [ %37, %36 ]
  %41 = bitcast i8* %40 to i32*
  %42 = load i32, i32* %41, align 4
  %43 = trunc i32 %42 to i8
  %44 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %43, i8* %45, align 8, !tbaa !11
  br label %177

46:                                               ; preds = %24
  %47 = load i32, i32* %18, align 16
  %48 = icmp ult i32 %47, 41
  br i1 %48, label %49, label %53

49:                                               ; preds = %46
  %50 = zext i32 %47 to i64
  %51 = getelementptr i8, i8* %23, i64 %50
  %52 = add nuw nsw i32 %47, 8
  store i32 %52, i32* %18, align 16
  br label %56

53:                                               ; preds = %46
  %54 = load i8*, i8** %19, align 8
  %55 = getelementptr i8, i8* %54, i64 8
  store i8* %55, i8** %19, align 8
  br label %56

56:                                               ; preds = %53, %49
  %57 = phi i8* [ %51, %49 ], [ %54, %53 ]
  %58 = bitcast i8* %57 to i32*
  %59 = load i32, i32* %58, align 4
  %60 = trunc i32 %59 to i8
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %60, i8* %62, align 8, !tbaa !11
  br label %177

63:                                               ; preds = %24
  %64 = load i32, i32* %18, align 16
  %65 = icmp ult i32 %64, 41
  br i1 %65, label %66, label %70

66:                                               ; preds = %63
  %67 = zext i32 %64 to i64
  %68 = getelementptr i8, i8* %23, i64 %67
  %69 = add nuw nsw i32 %64, 8
  store i32 %69, i32* %18, align 16
  br label %73

70:                                               ; preds = %63
  %71 = load i8*, i8** %19, align 8
  %72 = getelementptr i8, i8* %71, i64 8
  store i8* %72, i8** %19, align 8
  br label %73

73:                                               ; preds = %70, %66
  %74 = phi i8* [ %68, %66 ], [ %71, %70 ]
  %75 = bitcast i8* %74 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %77, i16* %79, align 8, !tbaa !11
  br label %177

80:                                               ; preds = %24
  %81 = load i32, i32* %18, align 16
  %82 = icmp ult i32 %81, 41
  br i1 %82, label %83, label %87

83:                                               ; preds = %80
  %84 = zext i32 %81 to i64
  %85 = getelementptr i8, i8* %23, i64 %84
  %86 = add nuw nsw i32 %81, 8
  store i32 %86, i32* %18, align 16
  br label %90

87:                                               ; preds = %80
  %88 = load i8*, i8** %19, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %19, align 8
  br label %90

90:                                               ; preds = %87, %83
  %91 = phi i8* [ %85, %83 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %177

97:                                               ; preds = %24
  %98 = load i32, i32* %18, align 16
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %104

100:                                              ; preds = %97
  %101 = zext i32 %98 to i64
  %102 = getelementptr i8, i8* %23, i64 %101
  %103 = add nuw nsw i32 %98, 8
  store i32 %103, i32* %18, align 16
  br label %107

104:                                              ; preds = %97
  %105 = load i8*, i8** %19, align 8
  %106 = getelementptr i8, i8* %105, i64 8
  store i8* %106, i8** %19, align 8
  br label %107

107:                                              ; preds = %104, %100
  %108 = phi i8* [ %102, %100 ], [ %105, %104 ]
  %109 = bitcast i8* %108 to i32*
  %110 = load i32, i32* %109, align 4
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %112 = bitcast %union.jvalue* %111 to i32*
  store i32 %110, i32* %112, align 8, !tbaa !11
  br label %177

113:                                              ; preds = %24
  %114 = load i32, i32* %18, align 16
  %115 = icmp ult i32 %114, 41
  br i1 %115, label %116, label %120

116:                                              ; preds = %113
  %117 = zext i32 %114 to i64
  %118 = getelementptr i8, i8* %23, i64 %117
  %119 = add nuw nsw i32 %114, 8
  store i32 %119, i32* %18, align 16
  br label %123

120:                                              ; preds = %113
  %121 = load i8*, i8** %19, align 8
  %122 = getelementptr i8, i8* %121, i64 8
  store i8* %122, i8** %19, align 8
  br label %123

123:                                              ; preds = %120, %116
  %124 = phi i8* [ %118, %116 ], [ %121, %120 ]
  %125 = bitcast i8* %124 to i64*
  %126 = load i64, i64* %125, align 8
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25, i32 0
  store i64 %126, i64* %127, align 8, !tbaa !11
  br label %177

128:                                              ; preds = %24
  %129 = load i32, i32* %21, align 4
  %130 = icmp ult i32 %129, 161
  br i1 %130, label %131, label %135

131:                                              ; preds = %128
  %132 = zext i32 %129 to i64
  %133 = getelementptr i8, i8* %23, i64 %132
  %134 = add nuw nsw i32 %129, 16
  store i32 %134, i32* %21, align 4
  br label %138

135:                                              ; preds = %128
  %136 = load i8*, i8** %19, align 8
  %137 = getelementptr i8, i8* %136, i64 8
  store i8* %137, i8** %19, align 8
  br label %138

138:                                              ; preds = %135, %131
  %139 = phi i8* [ %133, %131 ], [ %136, %135 ]
  %140 = bitcast i8* %139 to double*
  %141 = load double, double* %140, align 8
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %143 = bitcast %union.jvalue* %142 to double*
  store double %141, double* %143, align 8, !tbaa !11
  br label %177

144:                                              ; preds = %24
  %145 = load i32, i32* %21, align 4
  %146 = icmp ult i32 %145, 161
  br i1 %146, label %147, label %151

147:                                              ; preds = %144
  %148 = zext i32 %145 to i64
  %149 = getelementptr i8, i8* %23, i64 %148
  %150 = add nuw nsw i32 %145, 16
  store i32 %150, i32* %21, align 4
  br label %154

151:                                              ; preds = %144
  %152 = load i8*, i8** %19, align 8
  %153 = getelementptr i8, i8* %152, i64 8
  store i8* %153, i8** %19, align 8
  br label %154

154:                                              ; preds = %151, %147
  %155 = phi i8* [ %149, %147 ], [ %152, %151 ]
  %156 = bitcast i8* %155 to double*
  %157 = load double, double* %156, align 8
  %158 = fptrunc double %157 to float
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %160 = bitcast %union.jvalue* %159 to float*
  store float %158, float* %160, align 8, !tbaa !11
  br label %177

161:                                              ; preds = %24
  %162 = load i32, i32* %18, align 16
  %163 = icmp ult i32 %162, 41
  br i1 %163, label %164, label %168

164:                                              ; preds = %161
  %165 = zext i32 %162 to i64
  %166 = getelementptr i8, i8* %23, i64 %165
  %167 = add nuw nsw i32 %162, 8
  store i32 %167, i32* %18, align 16
  br label %171

168:                                              ; preds = %161
  %169 = load i8*, i8** %19, align 8
  %170 = getelementptr i8, i8* %169, i64 8
  store i8* %170, i8** %19, align 8
  br label %171

171:                                              ; preds = %168, %164
  %172 = phi i8* [ %166, %164 ], [ %169, %168 ]
  %173 = bitcast i8* %172 to %struct._jobject**
  %174 = load %struct._jobject*, %struct._jobject** %173, align 8
  %175 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %176, align 8, !tbaa !11
  br label %177

177:                                              ; preds = %171, %154, %138, %123, %107, %90, %73, %56, %39, %24
  %178 = add nuw nsw i64 %25, 1
  %179 = icmp eq i64 %178, %22
  br i1 %179, label %180, label %24, !llvm.loop !45

180:                                              ; preds = %177, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %9) #4
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i64 0, i32 81
  %183 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8, !tbaa !46
  %184 = call i32 %183(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %15) #4
  call void @llvm.stackrestore(i8* %8)
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %7) #4
  ret i32 %184
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i32 @JNI_CallNonvirtualIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %struct.__va_list_tag* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 16
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !5
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !9
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #4
  %12 = sext i32 %11 to i64
  %13 = alloca %union.jvalue, i64 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %186

15:                                               ; preds = %5
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 0
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 2
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 3
  %19 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 1
  %20 = zext i32 %11 to i64
  br label %21

21:                                               ; preds = %15, %183
  %22 = phi i64 [ 0, %15 ], [ %184, %183 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !11
  %25 = sext i8 %24 to i32
  switch i32 %25, label %183 [
    i32 90, label %26
    i32 66, label %44
    i32 83, label %62
    i32 67, label %80
    i32 73, label %98
    i32 74, label %115
    i32 68, label %131
    i32 70, label %148
    i32 76, label %166
  ]

26:                                               ; preds = %21
  %27 = load i32, i32* %16, align 8
  %28 = icmp ult i32 %27, 41
  br i1 %28, label %29, label %34

29:                                               ; preds = %26
  %30 = load i8*, i8** %18, align 8
  %31 = zext i32 %27 to i64
  %32 = getelementptr i8, i8* %30, i64 %31
  %33 = add nuw nsw i32 %27, 8
  store i32 %33, i32* %16, align 8
  br label %37

34:                                               ; preds = %26
  %35 = load i8*, i8** %17, align 8
  %36 = getelementptr i8, i8* %35, i64 8
  store i8* %36, i8** %17, align 8
  br label %37

37:                                               ; preds = %34, %29
  %38 = phi i8* [ %32, %29 ], [ %35, %34 ]
  %39 = bitcast i8* %38 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %41, i8* %43, align 8, !tbaa !11
  br label %183

44:                                               ; preds = %21
  %45 = load i32, i32* %16, align 8
  %46 = icmp ult i32 %45, 41
  br i1 %46, label %47, label %52

47:                                               ; preds = %44
  %48 = load i8*, i8** %18, align 8
  %49 = zext i32 %45 to i64
  %50 = getelementptr i8, i8* %48, i64 %49
  %51 = add nuw nsw i32 %45, 8
  store i32 %51, i32* %16, align 8
  br label %55

52:                                               ; preds = %44
  %53 = load i8*, i8** %17, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %17, align 8
  br label %55

55:                                               ; preds = %52, %47
  %56 = phi i8* [ %50, %47 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %183

62:                                               ; preds = %21
  %63 = load i32, i32* %16, align 8
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %70

65:                                               ; preds = %62
  %66 = load i8*, i8** %18, align 8
  %67 = zext i32 %63 to i64
  %68 = getelementptr i8, i8* %66, i64 %67
  %69 = add nuw nsw i32 %63, 8
  store i32 %69, i32* %16, align 8
  br label %73

70:                                               ; preds = %62
  %71 = load i8*, i8** %17, align 8
  %72 = getelementptr i8, i8* %71, i64 8
  store i8* %72, i8** %17, align 8
  br label %73

73:                                               ; preds = %70, %65
  %74 = phi i8* [ %68, %65 ], [ %71, %70 ]
  %75 = bitcast i8* %74 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %77, i16* %79, align 8, !tbaa !11
  br label %183

80:                                               ; preds = %21
  %81 = load i32, i32* %16, align 8
  %82 = icmp ult i32 %81, 41
  br i1 %82, label %83, label %88

83:                                               ; preds = %80
  %84 = load i8*, i8** %18, align 8
  %85 = zext i32 %81 to i64
  %86 = getelementptr i8, i8* %84, i64 %85
  %87 = add nuw nsw i32 %81, 8
  store i32 %87, i32* %16, align 8
  br label %91

88:                                               ; preds = %80
  %89 = load i8*, i8** %17, align 8
  %90 = getelementptr i8, i8* %89, i64 8
  store i8* %90, i8** %17, align 8
  br label %91

91:                                               ; preds = %88, %83
  %92 = phi i8* [ %86, %83 ], [ %89, %88 ]
  %93 = bitcast i8* %92 to i32*
  %94 = load i32, i32* %93, align 4
  %95 = and i32 %94, 65535
  %96 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %97 = bitcast %union.jvalue* %96 to i32*
  store i32 %95, i32* %97, align 8, !tbaa !11
  br label %183

98:                                               ; preds = %21
  %99 = load i32, i32* %16, align 8
  %100 = icmp ult i32 %99, 41
  br i1 %100, label %101, label %106

101:                                              ; preds = %98
  %102 = load i8*, i8** %18, align 8
  %103 = zext i32 %99 to i64
  %104 = getelementptr i8, i8* %102, i64 %103
  %105 = add nuw nsw i32 %99, 8
  store i32 %105, i32* %16, align 8
  br label %109

106:                                              ; preds = %98
  %107 = load i8*, i8** %17, align 8
  %108 = getelementptr i8, i8* %107, i64 8
  store i8* %108, i8** %17, align 8
  br label %109

109:                                              ; preds = %106, %101
  %110 = phi i8* [ %104, %101 ], [ %107, %106 ]
  %111 = bitcast i8* %110 to i32*
  %112 = load i32, i32* %111, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !11
  br label %183

115:                                              ; preds = %21
  %116 = load i32, i32* %16, align 8
  %117 = icmp ult i32 %116, 41
  br i1 %117, label %118, label %123

118:                                              ; preds = %115
  %119 = load i8*, i8** %18, align 8
  %120 = zext i32 %116 to i64
  %121 = getelementptr i8, i8* %119, i64 %120
  %122 = add nuw nsw i32 %116, 8
  store i32 %122, i32* %16, align 8
  br label %126

123:                                              ; preds = %115
  %124 = load i8*, i8** %17, align 8
  %125 = getelementptr i8, i8* %124, i64 8
  store i8* %125, i8** %17, align 8
  br label %126

126:                                              ; preds = %123, %118
  %127 = phi i8* [ %121, %118 ], [ %124, %123 ]
  %128 = bitcast i8* %127 to i64*
  %129 = load i64, i64* %128, align 8
  %130 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22, i32 0
  store i64 %129, i64* %130, align 8, !tbaa !11
  br label %183

131:                                              ; preds = %21
  %132 = load i32, i32* %19, align 4
  %133 = icmp ult i32 %132, 161
  br i1 %133, label %134, label %139

134:                                              ; preds = %131
  %135 = load i8*, i8** %18, align 8
  %136 = zext i32 %132 to i64
  %137 = getelementptr i8, i8* %135, i64 %136
  %138 = add nuw nsw i32 %132, 16
  store i32 %138, i32* %19, align 4
  br label %142

139:                                              ; preds = %131
  %140 = load i8*, i8** %17, align 8
  %141 = getelementptr i8, i8* %140, i64 8
  store i8* %141, i8** %17, align 8
  br label %142

142:                                              ; preds = %139, %134
  %143 = phi i8* [ %137, %134 ], [ %140, %139 ]
  %144 = bitcast i8* %143 to double*
  %145 = load double, double* %144, align 8
  %146 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %147 = bitcast %union.jvalue* %146 to double*
  store double %145, double* %147, align 8, !tbaa !11
  br label %183

148:                                              ; preds = %21
  %149 = load i32, i32* %19, align 4
  %150 = icmp ult i32 %149, 161
  br i1 %150, label %151, label %156

151:                                              ; preds = %148
  %152 = load i8*, i8** %18, align 8
  %153 = zext i32 %149 to i64
  %154 = getelementptr i8, i8* %152, i64 %153
  %155 = add nuw nsw i32 %149, 16
  store i32 %155, i32* %19, align 4
  br label %159

156:                                              ; preds = %148
  %157 = load i8*, i8** %17, align 8
  %158 = getelementptr i8, i8* %157, i64 8
  store i8* %158, i8** %17, align 8
  br label %159

159:                                              ; preds = %156, %151
  %160 = phi i8* [ %154, %151 ], [ %157, %156 ]
  %161 = bitcast i8* %160 to double*
  %162 = load double, double* %161, align 8
  %163 = fptrunc double %162 to float
  %164 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %165 = bitcast %union.jvalue* %164 to float*
  store float %163, float* %165, align 8, !tbaa !11
  br label %183

166:                                              ; preds = %21
  %167 = load i32, i32* %16, align 8
  %168 = icmp ult i32 %167, 41
  br i1 %168, label %169, label %174

169:                                              ; preds = %166
  %170 = load i8*, i8** %18, align 8
  %171 = zext i32 %167 to i64
  %172 = getelementptr i8, i8* %170, i64 %171
  %173 = add nuw nsw i32 %167, 8
  store i32 %173, i32* %16, align 8
  br label %177

174:                                              ; preds = %166
  %175 = load i8*, i8** %17, align 8
  %176 = getelementptr i8, i8* %175, i64 8
  store i8* %176, i8** %17, align 8
  br label %177

177:                                              ; preds = %174, %169
  %178 = phi i8* [ %172, %169 ], [ %175, %174 ]
  %179 = bitcast i8* %178 to %struct._jobject**
  %180 = load %struct._jobject*, %struct._jobject** %179, align 8
  %181 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %182 = bitcast %union.jvalue* %181 to %struct._jobject**
  store %struct._jobject* %180, %struct._jobject** %182, align 8, !tbaa !11
  br label %183

183:                                              ; preds = %37, %55, %73, %91, %109, %126, %142, %159, %177, %21
  %184 = add nuw nsw i64 %22, 1
  %185 = icmp eq i64 %184, %20
  br i1 %185, label %186, label %21, !llvm.loop !45

186:                                              ; preds = %183, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %187 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %188 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %187, i64 0, i32 81
  %189 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %188, align 8, !tbaa !46
  %190 = call i32 %189(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret i32 %190
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i32 @JNI_CallStaticIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !47

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 131
  %182 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !48
  %183 = call i32 %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret i32 %183
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i32 @JNI_CallStaticIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !47

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 131
  %188 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !48
  %189 = call i32 %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i32 %189
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i64 @JNI_CallLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !49

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 54
  %182 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !50
  %183 = call i64 %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret i64 %183
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i64 @JNI_CallLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !49

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 54
  %188 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !50
  %189 = call i64 %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i64 %189
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i64 @JNI_CallNonvirtualLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = alloca [1 x %struct.__va_list_tag], align 16
  %7 = bitcast [1 x %struct.__va_list_tag]* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = call i8* @llvm.stacksave()
  %9 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %9) #4
  %10 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %10, align 8, !tbaa !5
  %12 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %11, align 8, !tbaa !9
  %13 = call i32 %12(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %9) #4
  %14 = sext i32 %13 to i64
  %15 = alloca %union.jvalue, i64 %14, align 16
  %16 = icmp sgt i32 %13, 0
  br i1 %16, label %17, label %180

17:                                               ; preds = %4
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 0
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 2
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 3
  %21 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 1
  %22 = zext i32 %13 to i64
  %23 = load i8*, i8** %20, align 16
  br label %24

24:                                               ; preds = %177, %17
  %25 = phi i64 [ 0, %17 ], [ %178, %177 ]
  %26 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %25
  %27 = load i8, i8* %26, align 1, !tbaa !11
  %28 = sext i8 %27 to i32
  switch i32 %28, label %177 [
    i32 90, label %29
    i32 66, label %46
    i32 83, label %63
    i32 67, label %80
    i32 73, label %97
    i32 74, label %113
    i32 68, label %128
    i32 70, label %144
    i32 76, label %161
  ]

29:                                               ; preds = %24
  %30 = load i32, i32* %18, align 16
  %31 = icmp ult i32 %30, 41
  br i1 %31, label %32, label %36

32:                                               ; preds = %29
  %33 = zext i32 %30 to i64
  %34 = getelementptr i8, i8* %23, i64 %33
  %35 = add nuw nsw i32 %30, 8
  store i32 %35, i32* %18, align 16
  br label %39

36:                                               ; preds = %29
  %37 = load i8*, i8** %19, align 8
  %38 = getelementptr i8, i8* %37, i64 8
  store i8* %38, i8** %19, align 8
  br label %39

39:                                               ; preds = %36, %32
  %40 = phi i8* [ %34, %32 ], [ %37, %36 ]
  %41 = bitcast i8* %40 to i32*
  %42 = load i32, i32* %41, align 4
  %43 = trunc i32 %42 to i8
  %44 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %43, i8* %45, align 8, !tbaa !11
  br label %177

46:                                               ; preds = %24
  %47 = load i32, i32* %18, align 16
  %48 = icmp ult i32 %47, 41
  br i1 %48, label %49, label %53

49:                                               ; preds = %46
  %50 = zext i32 %47 to i64
  %51 = getelementptr i8, i8* %23, i64 %50
  %52 = add nuw nsw i32 %47, 8
  store i32 %52, i32* %18, align 16
  br label %56

53:                                               ; preds = %46
  %54 = load i8*, i8** %19, align 8
  %55 = getelementptr i8, i8* %54, i64 8
  store i8* %55, i8** %19, align 8
  br label %56

56:                                               ; preds = %53, %49
  %57 = phi i8* [ %51, %49 ], [ %54, %53 ]
  %58 = bitcast i8* %57 to i32*
  %59 = load i32, i32* %58, align 4
  %60 = trunc i32 %59 to i8
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %60, i8* %62, align 8, !tbaa !11
  br label %177

63:                                               ; preds = %24
  %64 = load i32, i32* %18, align 16
  %65 = icmp ult i32 %64, 41
  br i1 %65, label %66, label %70

66:                                               ; preds = %63
  %67 = zext i32 %64 to i64
  %68 = getelementptr i8, i8* %23, i64 %67
  %69 = add nuw nsw i32 %64, 8
  store i32 %69, i32* %18, align 16
  br label %73

70:                                               ; preds = %63
  %71 = load i8*, i8** %19, align 8
  %72 = getelementptr i8, i8* %71, i64 8
  store i8* %72, i8** %19, align 8
  br label %73

73:                                               ; preds = %70, %66
  %74 = phi i8* [ %68, %66 ], [ %71, %70 ]
  %75 = bitcast i8* %74 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %77, i16* %79, align 8, !tbaa !11
  br label %177

80:                                               ; preds = %24
  %81 = load i32, i32* %18, align 16
  %82 = icmp ult i32 %81, 41
  br i1 %82, label %83, label %87

83:                                               ; preds = %80
  %84 = zext i32 %81 to i64
  %85 = getelementptr i8, i8* %23, i64 %84
  %86 = add nuw nsw i32 %81, 8
  store i32 %86, i32* %18, align 16
  br label %90

87:                                               ; preds = %80
  %88 = load i8*, i8** %19, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %19, align 8
  br label %90

90:                                               ; preds = %87, %83
  %91 = phi i8* [ %85, %83 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %177

97:                                               ; preds = %24
  %98 = load i32, i32* %18, align 16
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %104

100:                                              ; preds = %97
  %101 = zext i32 %98 to i64
  %102 = getelementptr i8, i8* %23, i64 %101
  %103 = add nuw nsw i32 %98, 8
  store i32 %103, i32* %18, align 16
  br label %107

104:                                              ; preds = %97
  %105 = load i8*, i8** %19, align 8
  %106 = getelementptr i8, i8* %105, i64 8
  store i8* %106, i8** %19, align 8
  br label %107

107:                                              ; preds = %104, %100
  %108 = phi i8* [ %102, %100 ], [ %105, %104 ]
  %109 = bitcast i8* %108 to i32*
  %110 = load i32, i32* %109, align 4
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %112 = bitcast %union.jvalue* %111 to i32*
  store i32 %110, i32* %112, align 8, !tbaa !11
  br label %177

113:                                              ; preds = %24
  %114 = load i32, i32* %18, align 16
  %115 = icmp ult i32 %114, 41
  br i1 %115, label %116, label %120

116:                                              ; preds = %113
  %117 = zext i32 %114 to i64
  %118 = getelementptr i8, i8* %23, i64 %117
  %119 = add nuw nsw i32 %114, 8
  store i32 %119, i32* %18, align 16
  br label %123

120:                                              ; preds = %113
  %121 = load i8*, i8** %19, align 8
  %122 = getelementptr i8, i8* %121, i64 8
  store i8* %122, i8** %19, align 8
  br label %123

123:                                              ; preds = %120, %116
  %124 = phi i8* [ %118, %116 ], [ %121, %120 ]
  %125 = bitcast i8* %124 to i64*
  %126 = load i64, i64* %125, align 8
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25, i32 0
  store i64 %126, i64* %127, align 8, !tbaa !11
  br label %177

128:                                              ; preds = %24
  %129 = load i32, i32* %21, align 4
  %130 = icmp ult i32 %129, 161
  br i1 %130, label %131, label %135

131:                                              ; preds = %128
  %132 = zext i32 %129 to i64
  %133 = getelementptr i8, i8* %23, i64 %132
  %134 = add nuw nsw i32 %129, 16
  store i32 %134, i32* %21, align 4
  br label %138

135:                                              ; preds = %128
  %136 = load i8*, i8** %19, align 8
  %137 = getelementptr i8, i8* %136, i64 8
  store i8* %137, i8** %19, align 8
  br label %138

138:                                              ; preds = %135, %131
  %139 = phi i8* [ %133, %131 ], [ %136, %135 ]
  %140 = bitcast i8* %139 to double*
  %141 = load double, double* %140, align 8
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %143 = bitcast %union.jvalue* %142 to double*
  store double %141, double* %143, align 8, !tbaa !11
  br label %177

144:                                              ; preds = %24
  %145 = load i32, i32* %21, align 4
  %146 = icmp ult i32 %145, 161
  br i1 %146, label %147, label %151

147:                                              ; preds = %144
  %148 = zext i32 %145 to i64
  %149 = getelementptr i8, i8* %23, i64 %148
  %150 = add nuw nsw i32 %145, 16
  store i32 %150, i32* %21, align 4
  br label %154

151:                                              ; preds = %144
  %152 = load i8*, i8** %19, align 8
  %153 = getelementptr i8, i8* %152, i64 8
  store i8* %153, i8** %19, align 8
  br label %154

154:                                              ; preds = %151, %147
  %155 = phi i8* [ %149, %147 ], [ %152, %151 ]
  %156 = bitcast i8* %155 to double*
  %157 = load double, double* %156, align 8
  %158 = fptrunc double %157 to float
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %160 = bitcast %union.jvalue* %159 to float*
  store float %158, float* %160, align 8, !tbaa !11
  br label %177

161:                                              ; preds = %24
  %162 = load i32, i32* %18, align 16
  %163 = icmp ult i32 %162, 41
  br i1 %163, label %164, label %168

164:                                              ; preds = %161
  %165 = zext i32 %162 to i64
  %166 = getelementptr i8, i8* %23, i64 %165
  %167 = add nuw nsw i32 %162, 8
  store i32 %167, i32* %18, align 16
  br label %171

168:                                              ; preds = %161
  %169 = load i8*, i8** %19, align 8
  %170 = getelementptr i8, i8* %169, i64 8
  store i8* %170, i8** %19, align 8
  br label %171

171:                                              ; preds = %168, %164
  %172 = phi i8* [ %166, %164 ], [ %169, %168 ]
  %173 = bitcast i8* %172 to %struct._jobject**
  %174 = load %struct._jobject*, %struct._jobject** %173, align 8
  %175 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %176, align 8, !tbaa !11
  br label %177

177:                                              ; preds = %171, %154, %138, %123, %107, %90, %73, %56, %39, %24
  %178 = add nuw nsw i64 %25, 1
  %179 = icmp eq i64 %178, %22
  br i1 %179, label %180, label %24, !llvm.loop !51

180:                                              ; preds = %177, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %9) #4
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i64 0, i32 84
  %183 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8, !tbaa !52
  %184 = call i64 %183(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %15) #4
  call void @llvm.stackrestore(i8* %8)
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %7) #4
  ret i64 %184
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i64 @JNI_CallNonvirtualLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %struct.__va_list_tag* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 16
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !5
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !9
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #4
  %12 = sext i32 %11 to i64
  %13 = alloca %union.jvalue, i64 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %186

15:                                               ; preds = %5
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 0
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 2
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 3
  %19 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 1
  %20 = zext i32 %11 to i64
  br label %21

21:                                               ; preds = %15, %183
  %22 = phi i64 [ 0, %15 ], [ %184, %183 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !11
  %25 = sext i8 %24 to i32
  switch i32 %25, label %183 [
    i32 90, label %26
    i32 66, label %44
    i32 83, label %62
    i32 67, label %80
    i32 73, label %98
    i32 74, label %115
    i32 68, label %131
    i32 70, label %148
    i32 76, label %166
  ]

26:                                               ; preds = %21
  %27 = load i32, i32* %16, align 8
  %28 = icmp ult i32 %27, 41
  br i1 %28, label %29, label %34

29:                                               ; preds = %26
  %30 = load i8*, i8** %18, align 8
  %31 = zext i32 %27 to i64
  %32 = getelementptr i8, i8* %30, i64 %31
  %33 = add nuw nsw i32 %27, 8
  store i32 %33, i32* %16, align 8
  br label %37

34:                                               ; preds = %26
  %35 = load i8*, i8** %17, align 8
  %36 = getelementptr i8, i8* %35, i64 8
  store i8* %36, i8** %17, align 8
  br label %37

37:                                               ; preds = %34, %29
  %38 = phi i8* [ %32, %29 ], [ %35, %34 ]
  %39 = bitcast i8* %38 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %41, i8* %43, align 8, !tbaa !11
  br label %183

44:                                               ; preds = %21
  %45 = load i32, i32* %16, align 8
  %46 = icmp ult i32 %45, 41
  br i1 %46, label %47, label %52

47:                                               ; preds = %44
  %48 = load i8*, i8** %18, align 8
  %49 = zext i32 %45 to i64
  %50 = getelementptr i8, i8* %48, i64 %49
  %51 = add nuw nsw i32 %45, 8
  store i32 %51, i32* %16, align 8
  br label %55

52:                                               ; preds = %44
  %53 = load i8*, i8** %17, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %17, align 8
  br label %55

55:                                               ; preds = %52, %47
  %56 = phi i8* [ %50, %47 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %183

62:                                               ; preds = %21
  %63 = load i32, i32* %16, align 8
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %70

65:                                               ; preds = %62
  %66 = load i8*, i8** %18, align 8
  %67 = zext i32 %63 to i64
  %68 = getelementptr i8, i8* %66, i64 %67
  %69 = add nuw nsw i32 %63, 8
  store i32 %69, i32* %16, align 8
  br label %73

70:                                               ; preds = %62
  %71 = load i8*, i8** %17, align 8
  %72 = getelementptr i8, i8* %71, i64 8
  store i8* %72, i8** %17, align 8
  br label %73

73:                                               ; preds = %70, %65
  %74 = phi i8* [ %68, %65 ], [ %71, %70 ]
  %75 = bitcast i8* %74 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %77, i16* %79, align 8, !tbaa !11
  br label %183

80:                                               ; preds = %21
  %81 = load i32, i32* %16, align 8
  %82 = icmp ult i32 %81, 41
  br i1 %82, label %83, label %88

83:                                               ; preds = %80
  %84 = load i8*, i8** %18, align 8
  %85 = zext i32 %81 to i64
  %86 = getelementptr i8, i8* %84, i64 %85
  %87 = add nuw nsw i32 %81, 8
  store i32 %87, i32* %16, align 8
  br label %91

88:                                               ; preds = %80
  %89 = load i8*, i8** %17, align 8
  %90 = getelementptr i8, i8* %89, i64 8
  store i8* %90, i8** %17, align 8
  br label %91

91:                                               ; preds = %88, %83
  %92 = phi i8* [ %86, %83 ], [ %89, %88 ]
  %93 = bitcast i8* %92 to i32*
  %94 = load i32, i32* %93, align 4
  %95 = and i32 %94, 65535
  %96 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %97 = bitcast %union.jvalue* %96 to i32*
  store i32 %95, i32* %97, align 8, !tbaa !11
  br label %183

98:                                               ; preds = %21
  %99 = load i32, i32* %16, align 8
  %100 = icmp ult i32 %99, 41
  br i1 %100, label %101, label %106

101:                                              ; preds = %98
  %102 = load i8*, i8** %18, align 8
  %103 = zext i32 %99 to i64
  %104 = getelementptr i8, i8* %102, i64 %103
  %105 = add nuw nsw i32 %99, 8
  store i32 %105, i32* %16, align 8
  br label %109

106:                                              ; preds = %98
  %107 = load i8*, i8** %17, align 8
  %108 = getelementptr i8, i8* %107, i64 8
  store i8* %108, i8** %17, align 8
  br label %109

109:                                              ; preds = %106, %101
  %110 = phi i8* [ %104, %101 ], [ %107, %106 ]
  %111 = bitcast i8* %110 to i32*
  %112 = load i32, i32* %111, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !11
  br label %183

115:                                              ; preds = %21
  %116 = load i32, i32* %16, align 8
  %117 = icmp ult i32 %116, 41
  br i1 %117, label %118, label %123

118:                                              ; preds = %115
  %119 = load i8*, i8** %18, align 8
  %120 = zext i32 %116 to i64
  %121 = getelementptr i8, i8* %119, i64 %120
  %122 = add nuw nsw i32 %116, 8
  store i32 %122, i32* %16, align 8
  br label %126

123:                                              ; preds = %115
  %124 = load i8*, i8** %17, align 8
  %125 = getelementptr i8, i8* %124, i64 8
  store i8* %125, i8** %17, align 8
  br label %126

126:                                              ; preds = %123, %118
  %127 = phi i8* [ %121, %118 ], [ %124, %123 ]
  %128 = bitcast i8* %127 to i64*
  %129 = load i64, i64* %128, align 8
  %130 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22, i32 0
  store i64 %129, i64* %130, align 8, !tbaa !11
  br label %183

131:                                              ; preds = %21
  %132 = load i32, i32* %19, align 4
  %133 = icmp ult i32 %132, 161
  br i1 %133, label %134, label %139

134:                                              ; preds = %131
  %135 = load i8*, i8** %18, align 8
  %136 = zext i32 %132 to i64
  %137 = getelementptr i8, i8* %135, i64 %136
  %138 = add nuw nsw i32 %132, 16
  store i32 %138, i32* %19, align 4
  br label %142

139:                                              ; preds = %131
  %140 = load i8*, i8** %17, align 8
  %141 = getelementptr i8, i8* %140, i64 8
  store i8* %141, i8** %17, align 8
  br label %142

142:                                              ; preds = %139, %134
  %143 = phi i8* [ %137, %134 ], [ %140, %139 ]
  %144 = bitcast i8* %143 to double*
  %145 = load double, double* %144, align 8
  %146 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %147 = bitcast %union.jvalue* %146 to double*
  store double %145, double* %147, align 8, !tbaa !11
  br label %183

148:                                              ; preds = %21
  %149 = load i32, i32* %19, align 4
  %150 = icmp ult i32 %149, 161
  br i1 %150, label %151, label %156

151:                                              ; preds = %148
  %152 = load i8*, i8** %18, align 8
  %153 = zext i32 %149 to i64
  %154 = getelementptr i8, i8* %152, i64 %153
  %155 = add nuw nsw i32 %149, 16
  store i32 %155, i32* %19, align 4
  br label %159

156:                                              ; preds = %148
  %157 = load i8*, i8** %17, align 8
  %158 = getelementptr i8, i8* %157, i64 8
  store i8* %158, i8** %17, align 8
  br label %159

159:                                              ; preds = %156, %151
  %160 = phi i8* [ %154, %151 ], [ %157, %156 ]
  %161 = bitcast i8* %160 to double*
  %162 = load double, double* %161, align 8
  %163 = fptrunc double %162 to float
  %164 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %165 = bitcast %union.jvalue* %164 to float*
  store float %163, float* %165, align 8, !tbaa !11
  br label %183

166:                                              ; preds = %21
  %167 = load i32, i32* %16, align 8
  %168 = icmp ult i32 %167, 41
  br i1 %168, label %169, label %174

169:                                              ; preds = %166
  %170 = load i8*, i8** %18, align 8
  %171 = zext i32 %167 to i64
  %172 = getelementptr i8, i8* %170, i64 %171
  %173 = add nuw nsw i32 %167, 8
  store i32 %173, i32* %16, align 8
  br label %177

174:                                              ; preds = %166
  %175 = load i8*, i8** %17, align 8
  %176 = getelementptr i8, i8* %175, i64 8
  store i8* %176, i8** %17, align 8
  br label %177

177:                                              ; preds = %174, %169
  %178 = phi i8* [ %172, %169 ], [ %175, %174 ]
  %179 = bitcast i8* %178 to %struct._jobject**
  %180 = load %struct._jobject*, %struct._jobject** %179, align 8
  %181 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %182 = bitcast %union.jvalue* %181 to %struct._jobject**
  store %struct._jobject* %180, %struct._jobject** %182, align 8, !tbaa !11
  br label %183

183:                                              ; preds = %37, %55, %73, %91, %109, %126, %142, %159, %177, %21
  %184 = add nuw nsw i64 %22, 1
  %185 = icmp eq i64 %184, %20
  br i1 %185, label %186, label %21, !llvm.loop !51

186:                                              ; preds = %183, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %187 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %188 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %187, i64 0, i32 84
  %189 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %188, align 8, !tbaa !52
  %190 = call i64 %189(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret i64 %190
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i64 @JNI_CallStaticLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !53

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 134
  %182 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !54
  %183 = call i64 %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret i64 %183
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i64 @JNI_CallStaticLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !53

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 134
  %188 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !54
  %189 = call i64 %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i64 %189
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local float @JNI_CallFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !55

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 57
  %182 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !56
  %183 = call float %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret float %183
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local float @JNI_CallFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !55

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 57
  %188 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !56
  %189 = call float %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret float %189
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local float @JNI_CallNonvirtualFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = alloca [1 x %struct.__va_list_tag], align 16
  %7 = bitcast [1 x %struct.__va_list_tag]* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = call i8* @llvm.stacksave()
  %9 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %9) #4
  %10 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %10, align 8, !tbaa !5
  %12 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %11, align 8, !tbaa !9
  %13 = call i32 %12(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %9) #4
  %14 = sext i32 %13 to i64
  %15 = alloca %union.jvalue, i64 %14, align 16
  %16 = icmp sgt i32 %13, 0
  br i1 %16, label %17, label %180

17:                                               ; preds = %4
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 0
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 2
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 3
  %21 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 1
  %22 = zext i32 %13 to i64
  %23 = load i8*, i8** %20, align 16
  br label %24

24:                                               ; preds = %177, %17
  %25 = phi i64 [ 0, %17 ], [ %178, %177 ]
  %26 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %25
  %27 = load i8, i8* %26, align 1, !tbaa !11
  %28 = sext i8 %27 to i32
  switch i32 %28, label %177 [
    i32 90, label %29
    i32 66, label %46
    i32 83, label %63
    i32 67, label %80
    i32 73, label %97
    i32 74, label %113
    i32 68, label %128
    i32 70, label %144
    i32 76, label %161
  ]

29:                                               ; preds = %24
  %30 = load i32, i32* %18, align 16
  %31 = icmp ult i32 %30, 41
  br i1 %31, label %32, label %36

32:                                               ; preds = %29
  %33 = zext i32 %30 to i64
  %34 = getelementptr i8, i8* %23, i64 %33
  %35 = add nuw nsw i32 %30, 8
  store i32 %35, i32* %18, align 16
  br label %39

36:                                               ; preds = %29
  %37 = load i8*, i8** %19, align 8
  %38 = getelementptr i8, i8* %37, i64 8
  store i8* %38, i8** %19, align 8
  br label %39

39:                                               ; preds = %36, %32
  %40 = phi i8* [ %34, %32 ], [ %37, %36 ]
  %41 = bitcast i8* %40 to i32*
  %42 = load i32, i32* %41, align 4
  %43 = trunc i32 %42 to i8
  %44 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %43, i8* %45, align 8, !tbaa !11
  br label %177

46:                                               ; preds = %24
  %47 = load i32, i32* %18, align 16
  %48 = icmp ult i32 %47, 41
  br i1 %48, label %49, label %53

49:                                               ; preds = %46
  %50 = zext i32 %47 to i64
  %51 = getelementptr i8, i8* %23, i64 %50
  %52 = add nuw nsw i32 %47, 8
  store i32 %52, i32* %18, align 16
  br label %56

53:                                               ; preds = %46
  %54 = load i8*, i8** %19, align 8
  %55 = getelementptr i8, i8* %54, i64 8
  store i8* %55, i8** %19, align 8
  br label %56

56:                                               ; preds = %53, %49
  %57 = phi i8* [ %51, %49 ], [ %54, %53 ]
  %58 = bitcast i8* %57 to i32*
  %59 = load i32, i32* %58, align 4
  %60 = trunc i32 %59 to i8
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %60, i8* %62, align 8, !tbaa !11
  br label %177

63:                                               ; preds = %24
  %64 = load i32, i32* %18, align 16
  %65 = icmp ult i32 %64, 41
  br i1 %65, label %66, label %70

66:                                               ; preds = %63
  %67 = zext i32 %64 to i64
  %68 = getelementptr i8, i8* %23, i64 %67
  %69 = add nuw nsw i32 %64, 8
  store i32 %69, i32* %18, align 16
  br label %73

70:                                               ; preds = %63
  %71 = load i8*, i8** %19, align 8
  %72 = getelementptr i8, i8* %71, i64 8
  store i8* %72, i8** %19, align 8
  br label %73

73:                                               ; preds = %70, %66
  %74 = phi i8* [ %68, %66 ], [ %71, %70 ]
  %75 = bitcast i8* %74 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %77, i16* %79, align 8, !tbaa !11
  br label %177

80:                                               ; preds = %24
  %81 = load i32, i32* %18, align 16
  %82 = icmp ult i32 %81, 41
  br i1 %82, label %83, label %87

83:                                               ; preds = %80
  %84 = zext i32 %81 to i64
  %85 = getelementptr i8, i8* %23, i64 %84
  %86 = add nuw nsw i32 %81, 8
  store i32 %86, i32* %18, align 16
  br label %90

87:                                               ; preds = %80
  %88 = load i8*, i8** %19, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %19, align 8
  br label %90

90:                                               ; preds = %87, %83
  %91 = phi i8* [ %85, %83 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %177

97:                                               ; preds = %24
  %98 = load i32, i32* %18, align 16
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %104

100:                                              ; preds = %97
  %101 = zext i32 %98 to i64
  %102 = getelementptr i8, i8* %23, i64 %101
  %103 = add nuw nsw i32 %98, 8
  store i32 %103, i32* %18, align 16
  br label %107

104:                                              ; preds = %97
  %105 = load i8*, i8** %19, align 8
  %106 = getelementptr i8, i8* %105, i64 8
  store i8* %106, i8** %19, align 8
  br label %107

107:                                              ; preds = %104, %100
  %108 = phi i8* [ %102, %100 ], [ %105, %104 ]
  %109 = bitcast i8* %108 to i32*
  %110 = load i32, i32* %109, align 4
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %112 = bitcast %union.jvalue* %111 to i32*
  store i32 %110, i32* %112, align 8, !tbaa !11
  br label %177

113:                                              ; preds = %24
  %114 = load i32, i32* %18, align 16
  %115 = icmp ult i32 %114, 41
  br i1 %115, label %116, label %120

116:                                              ; preds = %113
  %117 = zext i32 %114 to i64
  %118 = getelementptr i8, i8* %23, i64 %117
  %119 = add nuw nsw i32 %114, 8
  store i32 %119, i32* %18, align 16
  br label %123

120:                                              ; preds = %113
  %121 = load i8*, i8** %19, align 8
  %122 = getelementptr i8, i8* %121, i64 8
  store i8* %122, i8** %19, align 8
  br label %123

123:                                              ; preds = %120, %116
  %124 = phi i8* [ %118, %116 ], [ %121, %120 ]
  %125 = bitcast i8* %124 to i64*
  %126 = load i64, i64* %125, align 8
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25, i32 0
  store i64 %126, i64* %127, align 8, !tbaa !11
  br label %177

128:                                              ; preds = %24
  %129 = load i32, i32* %21, align 4
  %130 = icmp ult i32 %129, 161
  br i1 %130, label %131, label %135

131:                                              ; preds = %128
  %132 = zext i32 %129 to i64
  %133 = getelementptr i8, i8* %23, i64 %132
  %134 = add nuw nsw i32 %129, 16
  store i32 %134, i32* %21, align 4
  br label %138

135:                                              ; preds = %128
  %136 = load i8*, i8** %19, align 8
  %137 = getelementptr i8, i8* %136, i64 8
  store i8* %137, i8** %19, align 8
  br label %138

138:                                              ; preds = %135, %131
  %139 = phi i8* [ %133, %131 ], [ %136, %135 ]
  %140 = bitcast i8* %139 to double*
  %141 = load double, double* %140, align 8
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %143 = bitcast %union.jvalue* %142 to double*
  store double %141, double* %143, align 8, !tbaa !11
  br label %177

144:                                              ; preds = %24
  %145 = load i32, i32* %21, align 4
  %146 = icmp ult i32 %145, 161
  br i1 %146, label %147, label %151

147:                                              ; preds = %144
  %148 = zext i32 %145 to i64
  %149 = getelementptr i8, i8* %23, i64 %148
  %150 = add nuw nsw i32 %145, 16
  store i32 %150, i32* %21, align 4
  br label %154

151:                                              ; preds = %144
  %152 = load i8*, i8** %19, align 8
  %153 = getelementptr i8, i8* %152, i64 8
  store i8* %153, i8** %19, align 8
  br label %154

154:                                              ; preds = %151, %147
  %155 = phi i8* [ %149, %147 ], [ %152, %151 ]
  %156 = bitcast i8* %155 to double*
  %157 = load double, double* %156, align 8
  %158 = fptrunc double %157 to float
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %160 = bitcast %union.jvalue* %159 to float*
  store float %158, float* %160, align 8, !tbaa !11
  br label %177

161:                                              ; preds = %24
  %162 = load i32, i32* %18, align 16
  %163 = icmp ult i32 %162, 41
  br i1 %163, label %164, label %168

164:                                              ; preds = %161
  %165 = zext i32 %162 to i64
  %166 = getelementptr i8, i8* %23, i64 %165
  %167 = add nuw nsw i32 %162, 8
  store i32 %167, i32* %18, align 16
  br label %171

168:                                              ; preds = %161
  %169 = load i8*, i8** %19, align 8
  %170 = getelementptr i8, i8* %169, i64 8
  store i8* %170, i8** %19, align 8
  br label %171

171:                                              ; preds = %168, %164
  %172 = phi i8* [ %166, %164 ], [ %169, %168 ]
  %173 = bitcast i8* %172 to %struct._jobject**
  %174 = load %struct._jobject*, %struct._jobject** %173, align 8
  %175 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %176, align 8, !tbaa !11
  br label %177

177:                                              ; preds = %171, %154, %138, %123, %107, %90, %73, %56, %39, %24
  %178 = add nuw nsw i64 %25, 1
  %179 = icmp eq i64 %178, %22
  br i1 %179, label %180, label %24, !llvm.loop !57

180:                                              ; preds = %177, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %9) #4
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i64 0, i32 87
  %183 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8, !tbaa !58
  %184 = call float %183(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %15) #4
  call void @llvm.stackrestore(i8* %8)
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %7) #4
  ret float %184
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local float @JNI_CallNonvirtualFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %struct.__va_list_tag* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 16
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !5
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !9
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #4
  %12 = sext i32 %11 to i64
  %13 = alloca %union.jvalue, i64 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %186

15:                                               ; preds = %5
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 0
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 2
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 3
  %19 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 1
  %20 = zext i32 %11 to i64
  br label %21

21:                                               ; preds = %15, %183
  %22 = phi i64 [ 0, %15 ], [ %184, %183 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !11
  %25 = sext i8 %24 to i32
  switch i32 %25, label %183 [
    i32 90, label %26
    i32 66, label %44
    i32 83, label %62
    i32 67, label %80
    i32 73, label %98
    i32 74, label %115
    i32 68, label %131
    i32 70, label %148
    i32 76, label %166
  ]

26:                                               ; preds = %21
  %27 = load i32, i32* %16, align 8
  %28 = icmp ult i32 %27, 41
  br i1 %28, label %29, label %34

29:                                               ; preds = %26
  %30 = load i8*, i8** %18, align 8
  %31 = zext i32 %27 to i64
  %32 = getelementptr i8, i8* %30, i64 %31
  %33 = add nuw nsw i32 %27, 8
  store i32 %33, i32* %16, align 8
  br label %37

34:                                               ; preds = %26
  %35 = load i8*, i8** %17, align 8
  %36 = getelementptr i8, i8* %35, i64 8
  store i8* %36, i8** %17, align 8
  br label %37

37:                                               ; preds = %34, %29
  %38 = phi i8* [ %32, %29 ], [ %35, %34 ]
  %39 = bitcast i8* %38 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %41, i8* %43, align 8, !tbaa !11
  br label %183

44:                                               ; preds = %21
  %45 = load i32, i32* %16, align 8
  %46 = icmp ult i32 %45, 41
  br i1 %46, label %47, label %52

47:                                               ; preds = %44
  %48 = load i8*, i8** %18, align 8
  %49 = zext i32 %45 to i64
  %50 = getelementptr i8, i8* %48, i64 %49
  %51 = add nuw nsw i32 %45, 8
  store i32 %51, i32* %16, align 8
  br label %55

52:                                               ; preds = %44
  %53 = load i8*, i8** %17, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %17, align 8
  br label %55

55:                                               ; preds = %52, %47
  %56 = phi i8* [ %50, %47 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %183

62:                                               ; preds = %21
  %63 = load i32, i32* %16, align 8
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %70

65:                                               ; preds = %62
  %66 = load i8*, i8** %18, align 8
  %67 = zext i32 %63 to i64
  %68 = getelementptr i8, i8* %66, i64 %67
  %69 = add nuw nsw i32 %63, 8
  store i32 %69, i32* %16, align 8
  br label %73

70:                                               ; preds = %62
  %71 = load i8*, i8** %17, align 8
  %72 = getelementptr i8, i8* %71, i64 8
  store i8* %72, i8** %17, align 8
  br label %73

73:                                               ; preds = %70, %65
  %74 = phi i8* [ %68, %65 ], [ %71, %70 ]
  %75 = bitcast i8* %74 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %77, i16* %79, align 8, !tbaa !11
  br label %183

80:                                               ; preds = %21
  %81 = load i32, i32* %16, align 8
  %82 = icmp ult i32 %81, 41
  br i1 %82, label %83, label %88

83:                                               ; preds = %80
  %84 = load i8*, i8** %18, align 8
  %85 = zext i32 %81 to i64
  %86 = getelementptr i8, i8* %84, i64 %85
  %87 = add nuw nsw i32 %81, 8
  store i32 %87, i32* %16, align 8
  br label %91

88:                                               ; preds = %80
  %89 = load i8*, i8** %17, align 8
  %90 = getelementptr i8, i8* %89, i64 8
  store i8* %90, i8** %17, align 8
  br label %91

91:                                               ; preds = %88, %83
  %92 = phi i8* [ %86, %83 ], [ %89, %88 ]
  %93 = bitcast i8* %92 to i32*
  %94 = load i32, i32* %93, align 4
  %95 = and i32 %94, 65535
  %96 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %97 = bitcast %union.jvalue* %96 to i32*
  store i32 %95, i32* %97, align 8, !tbaa !11
  br label %183

98:                                               ; preds = %21
  %99 = load i32, i32* %16, align 8
  %100 = icmp ult i32 %99, 41
  br i1 %100, label %101, label %106

101:                                              ; preds = %98
  %102 = load i8*, i8** %18, align 8
  %103 = zext i32 %99 to i64
  %104 = getelementptr i8, i8* %102, i64 %103
  %105 = add nuw nsw i32 %99, 8
  store i32 %105, i32* %16, align 8
  br label %109

106:                                              ; preds = %98
  %107 = load i8*, i8** %17, align 8
  %108 = getelementptr i8, i8* %107, i64 8
  store i8* %108, i8** %17, align 8
  br label %109

109:                                              ; preds = %106, %101
  %110 = phi i8* [ %104, %101 ], [ %107, %106 ]
  %111 = bitcast i8* %110 to i32*
  %112 = load i32, i32* %111, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !11
  br label %183

115:                                              ; preds = %21
  %116 = load i32, i32* %16, align 8
  %117 = icmp ult i32 %116, 41
  br i1 %117, label %118, label %123

118:                                              ; preds = %115
  %119 = load i8*, i8** %18, align 8
  %120 = zext i32 %116 to i64
  %121 = getelementptr i8, i8* %119, i64 %120
  %122 = add nuw nsw i32 %116, 8
  store i32 %122, i32* %16, align 8
  br label %126

123:                                              ; preds = %115
  %124 = load i8*, i8** %17, align 8
  %125 = getelementptr i8, i8* %124, i64 8
  store i8* %125, i8** %17, align 8
  br label %126

126:                                              ; preds = %123, %118
  %127 = phi i8* [ %121, %118 ], [ %124, %123 ]
  %128 = bitcast i8* %127 to i64*
  %129 = load i64, i64* %128, align 8
  %130 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22, i32 0
  store i64 %129, i64* %130, align 8, !tbaa !11
  br label %183

131:                                              ; preds = %21
  %132 = load i32, i32* %19, align 4
  %133 = icmp ult i32 %132, 161
  br i1 %133, label %134, label %139

134:                                              ; preds = %131
  %135 = load i8*, i8** %18, align 8
  %136 = zext i32 %132 to i64
  %137 = getelementptr i8, i8* %135, i64 %136
  %138 = add nuw nsw i32 %132, 16
  store i32 %138, i32* %19, align 4
  br label %142

139:                                              ; preds = %131
  %140 = load i8*, i8** %17, align 8
  %141 = getelementptr i8, i8* %140, i64 8
  store i8* %141, i8** %17, align 8
  br label %142

142:                                              ; preds = %139, %134
  %143 = phi i8* [ %137, %134 ], [ %140, %139 ]
  %144 = bitcast i8* %143 to double*
  %145 = load double, double* %144, align 8
  %146 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %147 = bitcast %union.jvalue* %146 to double*
  store double %145, double* %147, align 8, !tbaa !11
  br label %183

148:                                              ; preds = %21
  %149 = load i32, i32* %19, align 4
  %150 = icmp ult i32 %149, 161
  br i1 %150, label %151, label %156

151:                                              ; preds = %148
  %152 = load i8*, i8** %18, align 8
  %153 = zext i32 %149 to i64
  %154 = getelementptr i8, i8* %152, i64 %153
  %155 = add nuw nsw i32 %149, 16
  store i32 %155, i32* %19, align 4
  br label %159

156:                                              ; preds = %148
  %157 = load i8*, i8** %17, align 8
  %158 = getelementptr i8, i8* %157, i64 8
  store i8* %158, i8** %17, align 8
  br label %159

159:                                              ; preds = %156, %151
  %160 = phi i8* [ %154, %151 ], [ %157, %156 ]
  %161 = bitcast i8* %160 to double*
  %162 = load double, double* %161, align 8
  %163 = fptrunc double %162 to float
  %164 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %165 = bitcast %union.jvalue* %164 to float*
  store float %163, float* %165, align 8, !tbaa !11
  br label %183

166:                                              ; preds = %21
  %167 = load i32, i32* %16, align 8
  %168 = icmp ult i32 %167, 41
  br i1 %168, label %169, label %174

169:                                              ; preds = %166
  %170 = load i8*, i8** %18, align 8
  %171 = zext i32 %167 to i64
  %172 = getelementptr i8, i8* %170, i64 %171
  %173 = add nuw nsw i32 %167, 8
  store i32 %173, i32* %16, align 8
  br label %177

174:                                              ; preds = %166
  %175 = load i8*, i8** %17, align 8
  %176 = getelementptr i8, i8* %175, i64 8
  store i8* %176, i8** %17, align 8
  br label %177

177:                                              ; preds = %174, %169
  %178 = phi i8* [ %172, %169 ], [ %175, %174 ]
  %179 = bitcast i8* %178 to %struct._jobject**
  %180 = load %struct._jobject*, %struct._jobject** %179, align 8
  %181 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %182 = bitcast %union.jvalue* %181 to %struct._jobject**
  store %struct._jobject* %180, %struct._jobject** %182, align 8, !tbaa !11
  br label %183

183:                                              ; preds = %37, %55, %73, %91, %109, %126, %142, %159, %177, %21
  %184 = add nuw nsw i64 %22, 1
  %185 = icmp eq i64 %184, %20
  br i1 %185, label %186, label %21, !llvm.loop !57

186:                                              ; preds = %183, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %187 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %188 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %187, i64 0, i32 87
  %189 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %188, align 8, !tbaa !58
  %190 = call float %189(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret float %190
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local float @JNI_CallStaticFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !59

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 137
  %182 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !60
  %183 = call float %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret float %183
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local float @JNI_CallStaticFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !59

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 137
  %188 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !60
  %189 = call float %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret float %189
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local double @JNI_CallDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !61

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 60
  %182 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !62
  %183 = call double %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret double %183
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local double @JNI_CallDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !61

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 60
  %188 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !62
  %189 = call double %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret double %189
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local double @JNI_CallNonvirtualDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = alloca [1 x %struct.__va_list_tag], align 16
  %7 = bitcast [1 x %struct.__va_list_tag]* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = call i8* @llvm.stacksave()
  %9 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %9) #4
  %10 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %10, align 8, !tbaa !5
  %12 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %11, align 8, !tbaa !9
  %13 = call i32 %12(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %9) #4
  %14 = sext i32 %13 to i64
  %15 = alloca %union.jvalue, i64 %14, align 16
  %16 = icmp sgt i32 %13, 0
  br i1 %16, label %17, label %180

17:                                               ; preds = %4
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 0
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 2
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 3
  %21 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 1
  %22 = zext i32 %13 to i64
  %23 = load i8*, i8** %20, align 16
  br label %24

24:                                               ; preds = %177, %17
  %25 = phi i64 [ 0, %17 ], [ %178, %177 ]
  %26 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %25
  %27 = load i8, i8* %26, align 1, !tbaa !11
  %28 = sext i8 %27 to i32
  switch i32 %28, label %177 [
    i32 90, label %29
    i32 66, label %46
    i32 83, label %63
    i32 67, label %80
    i32 73, label %97
    i32 74, label %113
    i32 68, label %128
    i32 70, label %144
    i32 76, label %161
  ]

29:                                               ; preds = %24
  %30 = load i32, i32* %18, align 16
  %31 = icmp ult i32 %30, 41
  br i1 %31, label %32, label %36

32:                                               ; preds = %29
  %33 = zext i32 %30 to i64
  %34 = getelementptr i8, i8* %23, i64 %33
  %35 = add nuw nsw i32 %30, 8
  store i32 %35, i32* %18, align 16
  br label %39

36:                                               ; preds = %29
  %37 = load i8*, i8** %19, align 8
  %38 = getelementptr i8, i8* %37, i64 8
  store i8* %38, i8** %19, align 8
  br label %39

39:                                               ; preds = %36, %32
  %40 = phi i8* [ %34, %32 ], [ %37, %36 ]
  %41 = bitcast i8* %40 to i32*
  %42 = load i32, i32* %41, align 4
  %43 = trunc i32 %42 to i8
  %44 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %43, i8* %45, align 8, !tbaa !11
  br label %177

46:                                               ; preds = %24
  %47 = load i32, i32* %18, align 16
  %48 = icmp ult i32 %47, 41
  br i1 %48, label %49, label %53

49:                                               ; preds = %46
  %50 = zext i32 %47 to i64
  %51 = getelementptr i8, i8* %23, i64 %50
  %52 = add nuw nsw i32 %47, 8
  store i32 %52, i32* %18, align 16
  br label %56

53:                                               ; preds = %46
  %54 = load i8*, i8** %19, align 8
  %55 = getelementptr i8, i8* %54, i64 8
  store i8* %55, i8** %19, align 8
  br label %56

56:                                               ; preds = %53, %49
  %57 = phi i8* [ %51, %49 ], [ %54, %53 ]
  %58 = bitcast i8* %57 to i32*
  %59 = load i32, i32* %58, align 4
  %60 = trunc i32 %59 to i8
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %60, i8* %62, align 8, !tbaa !11
  br label %177

63:                                               ; preds = %24
  %64 = load i32, i32* %18, align 16
  %65 = icmp ult i32 %64, 41
  br i1 %65, label %66, label %70

66:                                               ; preds = %63
  %67 = zext i32 %64 to i64
  %68 = getelementptr i8, i8* %23, i64 %67
  %69 = add nuw nsw i32 %64, 8
  store i32 %69, i32* %18, align 16
  br label %73

70:                                               ; preds = %63
  %71 = load i8*, i8** %19, align 8
  %72 = getelementptr i8, i8* %71, i64 8
  store i8* %72, i8** %19, align 8
  br label %73

73:                                               ; preds = %70, %66
  %74 = phi i8* [ %68, %66 ], [ %71, %70 ]
  %75 = bitcast i8* %74 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %77, i16* %79, align 8, !tbaa !11
  br label %177

80:                                               ; preds = %24
  %81 = load i32, i32* %18, align 16
  %82 = icmp ult i32 %81, 41
  br i1 %82, label %83, label %87

83:                                               ; preds = %80
  %84 = zext i32 %81 to i64
  %85 = getelementptr i8, i8* %23, i64 %84
  %86 = add nuw nsw i32 %81, 8
  store i32 %86, i32* %18, align 16
  br label %90

87:                                               ; preds = %80
  %88 = load i8*, i8** %19, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %19, align 8
  br label %90

90:                                               ; preds = %87, %83
  %91 = phi i8* [ %85, %83 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %177

97:                                               ; preds = %24
  %98 = load i32, i32* %18, align 16
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %104

100:                                              ; preds = %97
  %101 = zext i32 %98 to i64
  %102 = getelementptr i8, i8* %23, i64 %101
  %103 = add nuw nsw i32 %98, 8
  store i32 %103, i32* %18, align 16
  br label %107

104:                                              ; preds = %97
  %105 = load i8*, i8** %19, align 8
  %106 = getelementptr i8, i8* %105, i64 8
  store i8* %106, i8** %19, align 8
  br label %107

107:                                              ; preds = %104, %100
  %108 = phi i8* [ %102, %100 ], [ %105, %104 ]
  %109 = bitcast i8* %108 to i32*
  %110 = load i32, i32* %109, align 4
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %112 = bitcast %union.jvalue* %111 to i32*
  store i32 %110, i32* %112, align 8, !tbaa !11
  br label %177

113:                                              ; preds = %24
  %114 = load i32, i32* %18, align 16
  %115 = icmp ult i32 %114, 41
  br i1 %115, label %116, label %120

116:                                              ; preds = %113
  %117 = zext i32 %114 to i64
  %118 = getelementptr i8, i8* %23, i64 %117
  %119 = add nuw nsw i32 %114, 8
  store i32 %119, i32* %18, align 16
  br label %123

120:                                              ; preds = %113
  %121 = load i8*, i8** %19, align 8
  %122 = getelementptr i8, i8* %121, i64 8
  store i8* %122, i8** %19, align 8
  br label %123

123:                                              ; preds = %120, %116
  %124 = phi i8* [ %118, %116 ], [ %121, %120 ]
  %125 = bitcast i8* %124 to i64*
  %126 = load i64, i64* %125, align 8
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25, i32 0
  store i64 %126, i64* %127, align 8, !tbaa !11
  br label %177

128:                                              ; preds = %24
  %129 = load i32, i32* %21, align 4
  %130 = icmp ult i32 %129, 161
  br i1 %130, label %131, label %135

131:                                              ; preds = %128
  %132 = zext i32 %129 to i64
  %133 = getelementptr i8, i8* %23, i64 %132
  %134 = add nuw nsw i32 %129, 16
  store i32 %134, i32* %21, align 4
  br label %138

135:                                              ; preds = %128
  %136 = load i8*, i8** %19, align 8
  %137 = getelementptr i8, i8* %136, i64 8
  store i8* %137, i8** %19, align 8
  br label %138

138:                                              ; preds = %135, %131
  %139 = phi i8* [ %133, %131 ], [ %136, %135 ]
  %140 = bitcast i8* %139 to double*
  %141 = load double, double* %140, align 8
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %143 = bitcast %union.jvalue* %142 to double*
  store double %141, double* %143, align 8, !tbaa !11
  br label %177

144:                                              ; preds = %24
  %145 = load i32, i32* %21, align 4
  %146 = icmp ult i32 %145, 161
  br i1 %146, label %147, label %151

147:                                              ; preds = %144
  %148 = zext i32 %145 to i64
  %149 = getelementptr i8, i8* %23, i64 %148
  %150 = add nuw nsw i32 %145, 16
  store i32 %150, i32* %21, align 4
  br label %154

151:                                              ; preds = %144
  %152 = load i8*, i8** %19, align 8
  %153 = getelementptr i8, i8* %152, i64 8
  store i8* %153, i8** %19, align 8
  br label %154

154:                                              ; preds = %151, %147
  %155 = phi i8* [ %149, %147 ], [ %152, %151 ]
  %156 = bitcast i8* %155 to double*
  %157 = load double, double* %156, align 8
  %158 = fptrunc double %157 to float
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %160 = bitcast %union.jvalue* %159 to float*
  store float %158, float* %160, align 8, !tbaa !11
  br label %177

161:                                              ; preds = %24
  %162 = load i32, i32* %18, align 16
  %163 = icmp ult i32 %162, 41
  br i1 %163, label %164, label %168

164:                                              ; preds = %161
  %165 = zext i32 %162 to i64
  %166 = getelementptr i8, i8* %23, i64 %165
  %167 = add nuw nsw i32 %162, 8
  store i32 %167, i32* %18, align 16
  br label %171

168:                                              ; preds = %161
  %169 = load i8*, i8** %19, align 8
  %170 = getelementptr i8, i8* %169, i64 8
  store i8* %170, i8** %19, align 8
  br label %171

171:                                              ; preds = %168, %164
  %172 = phi i8* [ %166, %164 ], [ %169, %168 ]
  %173 = bitcast i8* %172 to %struct._jobject**
  %174 = load %struct._jobject*, %struct._jobject** %173, align 8
  %175 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %176, align 8, !tbaa !11
  br label %177

177:                                              ; preds = %171, %154, %138, %123, %107, %90, %73, %56, %39, %24
  %178 = add nuw nsw i64 %25, 1
  %179 = icmp eq i64 %178, %22
  br i1 %179, label %180, label %24, !llvm.loop !63

180:                                              ; preds = %177, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %9) #4
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i64 0, i32 90
  %183 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8, !tbaa !64
  %184 = call double %183(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %15) #4
  call void @llvm.stackrestore(i8* %8)
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %7) #4
  ret double %184
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local double @JNI_CallNonvirtualDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %struct.__va_list_tag* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 16
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !5
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !9
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #4
  %12 = sext i32 %11 to i64
  %13 = alloca %union.jvalue, i64 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %186

15:                                               ; preds = %5
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 0
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 2
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 3
  %19 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 1
  %20 = zext i32 %11 to i64
  br label %21

21:                                               ; preds = %15, %183
  %22 = phi i64 [ 0, %15 ], [ %184, %183 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !11
  %25 = sext i8 %24 to i32
  switch i32 %25, label %183 [
    i32 90, label %26
    i32 66, label %44
    i32 83, label %62
    i32 67, label %80
    i32 73, label %98
    i32 74, label %115
    i32 68, label %131
    i32 70, label %148
    i32 76, label %166
  ]

26:                                               ; preds = %21
  %27 = load i32, i32* %16, align 8
  %28 = icmp ult i32 %27, 41
  br i1 %28, label %29, label %34

29:                                               ; preds = %26
  %30 = load i8*, i8** %18, align 8
  %31 = zext i32 %27 to i64
  %32 = getelementptr i8, i8* %30, i64 %31
  %33 = add nuw nsw i32 %27, 8
  store i32 %33, i32* %16, align 8
  br label %37

34:                                               ; preds = %26
  %35 = load i8*, i8** %17, align 8
  %36 = getelementptr i8, i8* %35, i64 8
  store i8* %36, i8** %17, align 8
  br label %37

37:                                               ; preds = %34, %29
  %38 = phi i8* [ %32, %29 ], [ %35, %34 ]
  %39 = bitcast i8* %38 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %41, i8* %43, align 8, !tbaa !11
  br label %183

44:                                               ; preds = %21
  %45 = load i32, i32* %16, align 8
  %46 = icmp ult i32 %45, 41
  br i1 %46, label %47, label %52

47:                                               ; preds = %44
  %48 = load i8*, i8** %18, align 8
  %49 = zext i32 %45 to i64
  %50 = getelementptr i8, i8* %48, i64 %49
  %51 = add nuw nsw i32 %45, 8
  store i32 %51, i32* %16, align 8
  br label %55

52:                                               ; preds = %44
  %53 = load i8*, i8** %17, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %17, align 8
  br label %55

55:                                               ; preds = %52, %47
  %56 = phi i8* [ %50, %47 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %183

62:                                               ; preds = %21
  %63 = load i32, i32* %16, align 8
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %70

65:                                               ; preds = %62
  %66 = load i8*, i8** %18, align 8
  %67 = zext i32 %63 to i64
  %68 = getelementptr i8, i8* %66, i64 %67
  %69 = add nuw nsw i32 %63, 8
  store i32 %69, i32* %16, align 8
  br label %73

70:                                               ; preds = %62
  %71 = load i8*, i8** %17, align 8
  %72 = getelementptr i8, i8* %71, i64 8
  store i8* %72, i8** %17, align 8
  br label %73

73:                                               ; preds = %70, %65
  %74 = phi i8* [ %68, %65 ], [ %71, %70 ]
  %75 = bitcast i8* %74 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %77, i16* %79, align 8, !tbaa !11
  br label %183

80:                                               ; preds = %21
  %81 = load i32, i32* %16, align 8
  %82 = icmp ult i32 %81, 41
  br i1 %82, label %83, label %88

83:                                               ; preds = %80
  %84 = load i8*, i8** %18, align 8
  %85 = zext i32 %81 to i64
  %86 = getelementptr i8, i8* %84, i64 %85
  %87 = add nuw nsw i32 %81, 8
  store i32 %87, i32* %16, align 8
  br label %91

88:                                               ; preds = %80
  %89 = load i8*, i8** %17, align 8
  %90 = getelementptr i8, i8* %89, i64 8
  store i8* %90, i8** %17, align 8
  br label %91

91:                                               ; preds = %88, %83
  %92 = phi i8* [ %86, %83 ], [ %89, %88 ]
  %93 = bitcast i8* %92 to i32*
  %94 = load i32, i32* %93, align 4
  %95 = and i32 %94, 65535
  %96 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %97 = bitcast %union.jvalue* %96 to i32*
  store i32 %95, i32* %97, align 8, !tbaa !11
  br label %183

98:                                               ; preds = %21
  %99 = load i32, i32* %16, align 8
  %100 = icmp ult i32 %99, 41
  br i1 %100, label %101, label %106

101:                                              ; preds = %98
  %102 = load i8*, i8** %18, align 8
  %103 = zext i32 %99 to i64
  %104 = getelementptr i8, i8* %102, i64 %103
  %105 = add nuw nsw i32 %99, 8
  store i32 %105, i32* %16, align 8
  br label %109

106:                                              ; preds = %98
  %107 = load i8*, i8** %17, align 8
  %108 = getelementptr i8, i8* %107, i64 8
  store i8* %108, i8** %17, align 8
  br label %109

109:                                              ; preds = %106, %101
  %110 = phi i8* [ %104, %101 ], [ %107, %106 ]
  %111 = bitcast i8* %110 to i32*
  %112 = load i32, i32* %111, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !11
  br label %183

115:                                              ; preds = %21
  %116 = load i32, i32* %16, align 8
  %117 = icmp ult i32 %116, 41
  br i1 %117, label %118, label %123

118:                                              ; preds = %115
  %119 = load i8*, i8** %18, align 8
  %120 = zext i32 %116 to i64
  %121 = getelementptr i8, i8* %119, i64 %120
  %122 = add nuw nsw i32 %116, 8
  store i32 %122, i32* %16, align 8
  br label %126

123:                                              ; preds = %115
  %124 = load i8*, i8** %17, align 8
  %125 = getelementptr i8, i8* %124, i64 8
  store i8* %125, i8** %17, align 8
  br label %126

126:                                              ; preds = %123, %118
  %127 = phi i8* [ %121, %118 ], [ %124, %123 ]
  %128 = bitcast i8* %127 to i64*
  %129 = load i64, i64* %128, align 8
  %130 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22, i32 0
  store i64 %129, i64* %130, align 8, !tbaa !11
  br label %183

131:                                              ; preds = %21
  %132 = load i32, i32* %19, align 4
  %133 = icmp ult i32 %132, 161
  br i1 %133, label %134, label %139

134:                                              ; preds = %131
  %135 = load i8*, i8** %18, align 8
  %136 = zext i32 %132 to i64
  %137 = getelementptr i8, i8* %135, i64 %136
  %138 = add nuw nsw i32 %132, 16
  store i32 %138, i32* %19, align 4
  br label %142

139:                                              ; preds = %131
  %140 = load i8*, i8** %17, align 8
  %141 = getelementptr i8, i8* %140, i64 8
  store i8* %141, i8** %17, align 8
  br label %142

142:                                              ; preds = %139, %134
  %143 = phi i8* [ %137, %134 ], [ %140, %139 ]
  %144 = bitcast i8* %143 to double*
  %145 = load double, double* %144, align 8
  %146 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %147 = bitcast %union.jvalue* %146 to double*
  store double %145, double* %147, align 8, !tbaa !11
  br label %183

148:                                              ; preds = %21
  %149 = load i32, i32* %19, align 4
  %150 = icmp ult i32 %149, 161
  br i1 %150, label %151, label %156

151:                                              ; preds = %148
  %152 = load i8*, i8** %18, align 8
  %153 = zext i32 %149 to i64
  %154 = getelementptr i8, i8* %152, i64 %153
  %155 = add nuw nsw i32 %149, 16
  store i32 %155, i32* %19, align 4
  br label %159

156:                                              ; preds = %148
  %157 = load i8*, i8** %17, align 8
  %158 = getelementptr i8, i8* %157, i64 8
  store i8* %158, i8** %17, align 8
  br label %159

159:                                              ; preds = %156, %151
  %160 = phi i8* [ %154, %151 ], [ %157, %156 ]
  %161 = bitcast i8* %160 to double*
  %162 = load double, double* %161, align 8
  %163 = fptrunc double %162 to float
  %164 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %165 = bitcast %union.jvalue* %164 to float*
  store float %163, float* %165, align 8, !tbaa !11
  br label %183

166:                                              ; preds = %21
  %167 = load i32, i32* %16, align 8
  %168 = icmp ult i32 %167, 41
  br i1 %168, label %169, label %174

169:                                              ; preds = %166
  %170 = load i8*, i8** %18, align 8
  %171 = zext i32 %167 to i64
  %172 = getelementptr i8, i8* %170, i64 %171
  %173 = add nuw nsw i32 %167, 8
  store i32 %173, i32* %16, align 8
  br label %177

174:                                              ; preds = %166
  %175 = load i8*, i8** %17, align 8
  %176 = getelementptr i8, i8* %175, i64 8
  store i8* %176, i8** %17, align 8
  br label %177

177:                                              ; preds = %174, %169
  %178 = phi i8* [ %172, %169 ], [ %175, %174 ]
  %179 = bitcast i8* %178 to %struct._jobject**
  %180 = load %struct._jobject*, %struct._jobject** %179, align 8
  %181 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %182 = bitcast %union.jvalue* %181 to %struct._jobject**
  store %struct._jobject* %180, %struct._jobject** %182, align 8, !tbaa !11
  br label %183

183:                                              ; preds = %37, %55, %73, %91, %109, %126, %142, %159, %177, %21
  %184 = add nuw nsw i64 %22, 1
  %185 = icmp eq i64 %184, %20
  br i1 %185, label %186, label %21, !llvm.loop !63

186:                                              ; preds = %183, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %187 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %188 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %187, i64 0, i32 90
  %189 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %188, align 8, !tbaa !64
  %190 = call double %189(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret double %190
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local double @JNI_CallStaticDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !65

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 140
  %182 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !66
  %183 = call double %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret double %183
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local double @JNI_CallStaticDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !65

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 140
  %188 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !66
  %189 = call double %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret double %189
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_NewObject(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !67

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 30
  %182 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !68
  %183 = call %struct._jobject* %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret %struct._jobject* %183
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_NewObjectV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !67

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 30
  %188 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !68
  %189 = call %struct._jobject* %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret %struct._jobject* %189
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local void @JNI_CallVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !69

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 63
  %182 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !70
  call void %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local void @JNI_CallVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !69

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 63
  %188 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !70
  call void %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local void @JNI_CallNonvirtualVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = alloca [1 x %struct.__va_list_tag], align 16
  %7 = bitcast [1 x %struct.__va_list_tag]* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = call i8* @llvm.stacksave()
  %9 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %9) #4
  %10 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %10, align 8, !tbaa !5
  %12 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %11, align 8, !tbaa !9
  %13 = call i32 %12(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %9) #4
  %14 = sext i32 %13 to i64
  %15 = alloca %union.jvalue, i64 %14, align 16
  %16 = icmp sgt i32 %13, 0
  br i1 %16, label %17, label %180

17:                                               ; preds = %4
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 0
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 2
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 3
  %21 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %6, i64 0, i64 0, i32 1
  %22 = zext i32 %13 to i64
  %23 = load i8*, i8** %20, align 16
  br label %24

24:                                               ; preds = %177, %17
  %25 = phi i64 [ 0, %17 ], [ %178, %177 ]
  %26 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %25
  %27 = load i8, i8* %26, align 1, !tbaa !11
  %28 = sext i8 %27 to i32
  switch i32 %28, label %177 [
    i32 90, label %29
    i32 66, label %46
    i32 83, label %63
    i32 67, label %80
    i32 73, label %97
    i32 74, label %113
    i32 68, label %128
    i32 70, label %144
    i32 76, label %161
  ]

29:                                               ; preds = %24
  %30 = load i32, i32* %18, align 16
  %31 = icmp ult i32 %30, 41
  br i1 %31, label %32, label %36

32:                                               ; preds = %29
  %33 = zext i32 %30 to i64
  %34 = getelementptr i8, i8* %23, i64 %33
  %35 = add nuw nsw i32 %30, 8
  store i32 %35, i32* %18, align 16
  br label %39

36:                                               ; preds = %29
  %37 = load i8*, i8** %19, align 8
  %38 = getelementptr i8, i8* %37, i64 8
  store i8* %38, i8** %19, align 8
  br label %39

39:                                               ; preds = %36, %32
  %40 = phi i8* [ %34, %32 ], [ %37, %36 ]
  %41 = bitcast i8* %40 to i32*
  %42 = load i32, i32* %41, align 4
  %43 = trunc i32 %42 to i8
  %44 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %45 = bitcast %union.jvalue* %44 to i8*
  store i8 %43, i8* %45, align 8, !tbaa !11
  br label %177

46:                                               ; preds = %24
  %47 = load i32, i32* %18, align 16
  %48 = icmp ult i32 %47, 41
  br i1 %48, label %49, label %53

49:                                               ; preds = %46
  %50 = zext i32 %47 to i64
  %51 = getelementptr i8, i8* %23, i64 %50
  %52 = add nuw nsw i32 %47, 8
  store i32 %52, i32* %18, align 16
  br label %56

53:                                               ; preds = %46
  %54 = load i8*, i8** %19, align 8
  %55 = getelementptr i8, i8* %54, i64 8
  store i8* %55, i8** %19, align 8
  br label %56

56:                                               ; preds = %53, %49
  %57 = phi i8* [ %51, %49 ], [ %54, %53 ]
  %58 = bitcast i8* %57 to i32*
  %59 = load i32, i32* %58, align 4
  %60 = trunc i32 %59 to i8
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %60, i8* %62, align 8, !tbaa !11
  br label %177

63:                                               ; preds = %24
  %64 = load i32, i32* %18, align 16
  %65 = icmp ult i32 %64, 41
  br i1 %65, label %66, label %70

66:                                               ; preds = %63
  %67 = zext i32 %64 to i64
  %68 = getelementptr i8, i8* %23, i64 %67
  %69 = add nuw nsw i32 %64, 8
  store i32 %69, i32* %18, align 16
  br label %73

70:                                               ; preds = %63
  %71 = load i8*, i8** %19, align 8
  %72 = getelementptr i8, i8* %71, i64 8
  store i8* %72, i8** %19, align 8
  br label %73

73:                                               ; preds = %70, %66
  %74 = phi i8* [ %68, %66 ], [ %71, %70 ]
  %75 = bitcast i8* %74 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %77, i16* %79, align 8, !tbaa !11
  br label %177

80:                                               ; preds = %24
  %81 = load i32, i32* %18, align 16
  %82 = icmp ult i32 %81, 41
  br i1 %82, label %83, label %87

83:                                               ; preds = %80
  %84 = zext i32 %81 to i64
  %85 = getelementptr i8, i8* %23, i64 %84
  %86 = add nuw nsw i32 %81, 8
  store i32 %86, i32* %18, align 16
  br label %90

87:                                               ; preds = %80
  %88 = load i8*, i8** %19, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %19, align 8
  br label %90

90:                                               ; preds = %87, %83
  %91 = phi i8* [ %85, %83 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %177

97:                                               ; preds = %24
  %98 = load i32, i32* %18, align 16
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %104

100:                                              ; preds = %97
  %101 = zext i32 %98 to i64
  %102 = getelementptr i8, i8* %23, i64 %101
  %103 = add nuw nsw i32 %98, 8
  store i32 %103, i32* %18, align 16
  br label %107

104:                                              ; preds = %97
  %105 = load i8*, i8** %19, align 8
  %106 = getelementptr i8, i8* %105, i64 8
  store i8* %106, i8** %19, align 8
  br label %107

107:                                              ; preds = %104, %100
  %108 = phi i8* [ %102, %100 ], [ %105, %104 ]
  %109 = bitcast i8* %108 to i32*
  %110 = load i32, i32* %109, align 4
  %111 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %112 = bitcast %union.jvalue* %111 to i32*
  store i32 %110, i32* %112, align 8, !tbaa !11
  br label %177

113:                                              ; preds = %24
  %114 = load i32, i32* %18, align 16
  %115 = icmp ult i32 %114, 41
  br i1 %115, label %116, label %120

116:                                              ; preds = %113
  %117 = zext i32 %114 to i64
  %118 = getelementptr i8, i8* %23, i64 %117
  %119 = add nuw nsw i32 %114, 8
  store i32 %119, i32* %18, align 16
  br label %123

120:                                              ; preds = %113
  %121 = load i8*, i8** %19, align 8
  %122 = getelementptr i8, i8* %121, i64 8
  store i8* %122, i8** %19, align 8
  br label %123

123:                                              ; preds = %120, %116
  %124 = phi i8* [ %118, %116 ], [ %121, %120 ]
  %125 = bitcast i8* %124 to i64*
  %126 = load i64, i64* %125, align 8
  %127 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25, i32 0
  store i64 %126, i64* %127, align 8, !tbaa !11
  br label %177

128:                                              ; preds = %24
  %129 = load i32, i32* %21, align 4
  %130 = icmp ult i32 %129, 161
  br i1 %130, label %131, label %135

131:                                              ; preds = %128
  %132 = zext i32 %129 to i64
  %133 = getelementptr i8, i8* %23, i64 %132
  %134 = add nuw nsw i32 %129, 16
  store i32 %134, i32* %21, align 4
  br label %138

135:                                              ; preds = %128
  %136 = load i8*, i8** %19, align 8
  %137 = getelementptr i8, i8* %136, i64 8
  store i8* %137, i8** %19, align 8
  br label %138

138:                                              ; preds = %135, %131
  %139 = phi i8* [ %133, %131 ], [ %136, %135 ]
  %140 = bitcast i8* %139 to double*
  %141 = load double, double* %140, align 8
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %143 = bitcast %union.jvalue* %142 to double*
  store double %141, double* %143, align 8, !tbaa !11
  br label %177

144:                                              ; preds = %24
  %145 = load i32, i32* %21, align 4
  %146 = icmp ult i32 %145, 161
  br i1 %146, label %147, label %151

147:                                              ; preds = %144
  %148 = zext i32 %145 to i64
  %149 = getelementptr i8, i8* %23, i64 %148
  %150 = add nuw nsw i32 %145, 16
  store i32 %150, i32* %21, align 4
  br label %154

151:                                              ; preds = %144
  %152 = load i8*, i8** %19, align 8
  %153 = getelementptr i8, i8* %152, i64 8
  store i8* %153, i8** %19, align 8
  br label %154

154:                                              ; preds = %151, %147
  %155 = phi i8* [ %149, %147 ], [ %152, %151 ]
  %156 = bitcast i8* %155 to double*
  %157 = load double, double* %156, align 8
  %158 = fptrunc double %157 to float
  %159 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %160 = bitcast %union.jvalue* %159 to float*
  store float %158, float* %160, align 8, !tbaa !11
  br label %177

161:                                              ; preds = %24
  %162 = load i32, i32* %18, align 16
  %163 = icmp ult i32 %162, 41
  br i1 %163, label %164, label %168

164:                                              ; preds = %161
  %165 = zext i32 %162 to i64
  %166 = getelementptr i8, i8* %23, i64 %165
  %167 = add nuw nsw i32 %162, 8
  store i32 %167, i32* %18, align 16
  br label %171

168:                                              ; preds = %161
  %169 = load i8*, i8** %19, align 8
  %170 = getelementptr i8, i8* %169, i64 8
  store i8* %170, i8** %19, align 8
  br label %171

171:                                              ; preds = %168, %164
  %172 = phi i8* [ %166, %164 ], [ %169, %168 ]
  %173 = bitcast i8* %172 to %struct._jobject**
  %174 = load %struct._jobject*, %struct._jobject** %173, align 8
  %175 = getelementptr inbounds %union.jvalue, %union.jvalue* %15, i64 %25
  %176 = bitcast %union.jvalue* %175 to %struct._jobject**
  store %struct._jobject* %174, %struct._jobject** %176, align 8, !tbaa !11
  br label %177

177:                                              ; preds = %171, %154, %138, %123, %107, %90, %73, %56, %39, %24
  %178 = add nuw nsw i64 %25, 1
  %179 = icmp eq i64 %178, %22
  br i1 %179, label %180, label %24, !llvm.loop !71

180:                                              ; preds = %177, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %9) #4
  %181 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %182 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %181, i64 0, i32 93
  %183 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %182, align 8, !tbaa !72
  call void %183(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %15) #4
  call void @llvm.stackrestore(i8* %8)
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %7) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local void @JNI_CallNonvirtualVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %struct.__va_list_tag* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 16
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !5
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !9
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #4
  %12 = sext i32 %11 to i64
  %13 = alloca %union.jvalue, i64 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %186

15:                                               ; preds = %5
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 0
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 2
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 3
  %19 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %4, i64 0, i32 1
  %20 = zext i32 %11 to i64
  br label %21

21:                                               ; preds = %15, %183
  %22 = phi i64 [ 0, %15 ], [ %184, %183 ]
  %23 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 %22
  %24 = load i8, i8* %23, align 1, !tbaa !11
  %25 = sext i8 %24 to i32
  switch i32 %25, label %183 [
    i32 90, label %26
    i32 66, label %44
    i32 83, label %62
    i32 67, label %80
    i32 73, label %98
    i32 74, label %115
    i32 68, label %131
    i32 70, label %148
    i32 76, label %166
  ]

26:                                               ; preds = %21
  %27 = load i32, i32* %16, align 8
  %28 = icmp ult i32 %27, 41
  br i1 %28, label %29, label %34

29:                                               ; preds = %26
  %30 = load i8*, i8** %18, align 8
  %31 = zext i32 %27 to i64
  %32 = getelementptr i8, i8* %30, i64 %31
  %33 = add nuw nsw i32 %27, 8
  store i32 %33, i32* %16, align 8
  br label %37

34:                                               ; preds = %26
  %35 = load i8*, i8** %17, align 8
  %36 = getelementptr i8, i8* %35, i64 8
  store i8* %36, i8** %17, align 8
  br label %37

37:                                               ; preds = %34, %29
  %38 = phi i8* [ %32, %29 ], [ %35, %34 ]
  %39 = bitcast i8* %38 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i8
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %43 = bitcast %union.jvalue* %42 to i8*
  store i8 %41, i8* %43, align 8, !tbaa !11
  br label %183

44:                                               ; preds = %21
  %45 = load i32, i32* %16, align 8
  %46 = icmp ult i32 %45, 41
  br i1 %46, label %47, label %52

47:                                               ; preds = %44
  %48 = load i8*, i8** %18, align 8
  %49 = zext i32 %45 to i64
  %50 = getelementptr i8, i8* %48, i64 %49
  %51 = add nuw nsw i32 %45, 8
  store i32 %51, i32* %16, align 8
  br label %55

52:                                               ; preds = %44
  %53 = load i8*, i8** %17, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %17, align 8
  br label %55

55:                                               ; preds = %52, %47
  %56 = phi i8* [ %50, %47 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %183

62:                                               ; preds = %21
  %63 = load i32, i32* %16, align 8
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %70

65:                                               ; preds = %62
  %66 = load i8*, i8** %18, align 8
  %67 = zext i32 %63 to i64
  %68 = getelementptr i8, i8* %66, i64 %67
  %69 = add nuw nsw i32 %63, 8
  store i32 %69, i32* %16, align 8
  br label %73

70:                                               ; preds = %62
  %71 = load i8*, i8** %17, align 8
  %72 = getelementptr i8, i8* %71, i64 8
  store i8* %72, i8** %17, align 8
  br label %73

73:                                               ; preds = %70, %65
  %74 = phi i8* [ %68, %65 ], [ %71, %70 ]
  %75 = bitcast i8* %74 to i32*
  %76 = load i32, i32* %75, align 4
  %77 = trunc i32 %76 to i16
  %78 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %79 = bitcast %union.jvalue* %78 to i16*
  store i16 %77, i16* %79, align 8, !tbaa !11
  br label %183

80:                                               ; preds = %21
  %81 = load i32, i32* %16, align 8
  %82 = icmp ult i32 %81, 41
  br i1 %82, label %83, label %88

83:                                               ; preds = %80
  %84 = load i8*, i8** %18, align 8
  %85 = zext i32 %81 to i64
  %86 = getelementptr i8, i8* %84, i64 %85
  %87 = add nuw nsw i32 %81, 8
  store i32 %87, i32* %16, align 8
  br label %91

88:                                               ; preds = %80
  %89 = load i8*, i8** %17, align 8
  %90 = getelementptr i8, i8* %89, i64 8
  store i8* %90, i8** %17, align 8
  br label %91

91:                                               ; preds = %88, %83
  %92 = phi i8* [ %86, %83 ], [ %89, %88 ]
  %93 = bitcast i8* %92 to i32*
  %94 = load i32, i32* %93, align 4
  %95 = and i32 %94, 65535
  %96 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %97 = bitcast %union.jvalue* %96 to i32*
  store i32 %95, i32* %97, align 8, !tbaa !11
  br label %183

98:                                               ; preds = %21
  %99 = load i32, i32* %16, align 8
  %100 = icmp ult i32 %99, 41
  br i1 %100, label %101, label %106

101:                                              ; preds = %98
  %102 = load i8*, i8** %18, align 8
  %103 = zext i32 %99 to i64
  %104 = getelementptr i8, i8* %102, i64 %103
  %105 = add nuw nsw i32 %99, 8
  store i32 %105, i32* %16, align 8
  br label %109

106:                                              ; preds = %98
  %107 = load i8*, i8** %17, align 8
  %108 = getelementptr i8, i8* %107, i64 8
  store i8* %108, i8** %17, align 8
  br label %109

109:                                              ; preds = %106, %101
  %110 = phi i8* [ %104, %101 ], [ %107, %106 ]
  %111 = bitcast i8* %110 to i32*
  %112 = load i32, i32* %111, align 4
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %114 = bitcast %union.jvalue* %113 to i32*
  store i32 %112, i32* %114, align 8, !tbaa !11
  br label %183

115:                                              ; preds = %21
  %116 = load i32, i32* %16, align 8
  %117 = icmp ult i32 %116, 41
  br i1 %117, label %118, label %123

118:                                              ; preds = %115
  %119 = load i8*, i8** %18, align 8
  %120 = zext i32 %116 to i64
  %121 = getelementptr i8, i8* %119, i64 %120
  %122 = add nuw nsw i32 %116, 8
  store i32 %122, i32* %16, align 8
  br label %126

123:                                              ; preds = %115
  %124 = load i8*, i8** %17, align 8
  %125 = getelementptr i8, i8* %124, i64 8
  store i8* %125, i8** %17, align 8
  br label %126

126:                                              ; preds = %123, %118
  %127 = phi i8* [ %121, %118 ], [ %124, %123 ]
  %128 = bitcast i8* %127 to i64*
  %129 = load i64, i64* %128, align 8
  %130 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22, i32 0
  store i64 %129, i64* %130, align 8, !tbaa !11
  br label %183

131:                                              ; preds = %21
  %132 = load i32, i32* %19, align 4
  %133 = icmp ult i32 %132, 161
  br i1 %133, label %134, label %139

134:                                              ; preds = %131
  %135 = load i8*, i8** %18, align 8
  %136 = zext i32 %132 to i64
  %137 = getelementptr i8, i8* %135, i64 %136
  %138 = add nuw nsw i32 %132, 16
  store i32 %138, i32* %19, align 4
  br label %142

139:                                              ; preds = %131
  %140 = load i8*, i8** %17, align 8
  %141 = getelementptr i8, i8* %140, i64 8
  store i8* %141, i8** %17, align 8
  br label %142

142:                                              ; preds = %139, %134
  %143 = phi i8* [ %137, %134 ], [ %140, %139 ]
  %144 = bitcast i8* %143 to double*
  %145 = load double, double* %144, align 8
  %146 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %147 = bitcast %union.jvalue* %146 to double*
  store double %145, double* %147, align 8, !tbaa !11
  br label %183

148:                                              ; preds = %21
  %149 = load i32, i32* %19, align 4
  %150 = icmp ult i32 %149, 161
  br i1 %150, label %151, label %156

151:                                              ; preds = %148
  %152 = load i8*, i8** %18, align 8
  %153 = zext i32 %149 to i64
  %154 = getelementptr i8, i8* %152, i64 %153
  %155 = add nuw nsw i32 %149, 16
  store i32 %155, i32* %19, align 4
  br label %159

156:                                              ; preds = %148
  %157 = load i8*, i8** %17, align 8
  %158 = getelementptr i8, i8* %157, i64 8
  store i8* %158, i8** %17, align 8
  br label %159

159:                                              ; preds = %156, %151
  %160 = phi i8* [ %154, %151 ], [ %157, %156 ]
  %161 = bitcast i8* %160 to double*
  %162 = load double, double* %161, align 8
  %163 = fptrunc double %162 to float
  %164 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %165 = bitcast %union.jvalue* %164 to float*
  store float %163, float* %165, align 8, !tbaa !11
  br label %183

166:                                              ; preds = %21
  %167 = load i32, i32* %16, align 8
  %168 = icmp ult i32 %167, 41
  br i1 %168, label %169, label %174

169:                                              ; preds = %166
  %170 = load i8*, i8** %18, align 8
  %171 = zext i32 %167 to i64
  %172 = getelementptr i8, i8* %170, i64 %171
  %173 = add nuw nsw i32 %167, 8
  store i32 %173, i32* %16, align 8
  br label %177

174:                                              ; preds = %166
  %175 = load i8*, i8** %17, align 8
  %176 = getelementptr i8, i8* %175, i64 8
  store i8* %176, i8** %17, align 8
  br label %177

177:                                              ; preds = %174, %169
  %178 = phi i8* [ %172, %169 ], [ %175, %174 ]
  %179 = bitcast i8* %178 to %struct._jobject**
  %180 = load %struct._jobject*, %struct._jobject** %179, align 8
  %181 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i64 %22
  %182 = bitcast %union.jvalue* %181 to %struct._jobject**
  store %struct._jobject* %180, %struct._jobject** %182, align 8, !tbaa !11
  br label %183

183:                                              ; preds = %37, %55, %73, %91, %109, %126, %142, %159, %177, %21
  %184 = add nuw nsw i64 %22, 1
  %185 = icmp eq i64 %184, %20
  br i1 %185, label %186, label %21, !llvm.loop !71

186:                                              ; preds = %183, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %187 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %188 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %187, i64 0, i32 93
  %189 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %188, align 8, !tbaa !72
  call void %189(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local void @JNI_CallStaticVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca [1 x %struct.__va_list_tag], align 16
  %6 = bitcast [1 x %struct.__va_list_tag]* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 24, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = call i8* @llvm.stacksave()
  %8 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %8) #4
  %9 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %9, align 8, !tbaa !5
  %11 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %10, align 8, !tbaa !9
  %12 = call i32 %11(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %8) #4
  %13 = sext i32 %12 to i64
  %14 = alloca %union.jvalue, i64 %13, align 16
  %15 = icmp sgt i32 %12, 0
  br i1 %15, label %16, label %179

16:                                               ; preds = %3
  %17 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 0
  %18 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 2
  %19 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 3
  %20 = getelementptr inbounds [1 x %struct.__va_list_tag], [1 x %struct.__va_list_tag]* %5, i64 0, i64 0, i32 1
  %21 = zext i32 %12 to i64
  %22 = load i8*, i8** %19, align 16
  br label %23

23:                                               ; preds = %176, %16
  %24 = phi i64 [ 0, %16 ], [ %177, %176 ]
  %25 = getelementptr inbounds [257 x i8], [257 x i8]* %4, i64 0, i64 %24
  %26 = load i8, i8* %25, align 1, !tbaa !11
  %27 = sext i8 %26 to i32
  switch i32 %27, label %176 [
    i32 90, label %28
    i32 66, label %45
    i32 83, label %62
    i32 67, label %79
    i32 73, label %96
    i32 74, label %112
    i32 68, label %127
    i32 70, label %143
    i32 76, label %160
  ]

28:                                               ; preds = %23
  %29 = load i32, i32* %17, align 16
  %30 = icmp ult i32 %29, 41
  br i1 %30, label %31, label %35

31:                                               ; preds = %28
  %32 = zext i32 %29 to i64
  %33 = getelementptr i8, i8* %22, i64 %32
  %34 = add nuw nsw i32 %29, 8
  store i32 %34, i32* %17, align 16
  br label %38

35:                                               ; preds = %28
  %36 = load i8*, i8** %18, align 8
  %37 = getelementptr i8, i8* %36, i64 8
  store i8* %37, i8** %18, align 8
  br label %38

38:                                               ; preds = %35, %31
  %39 = phi i8* [ %33, %31 ], [ %36, %35 ]
  %40 = bitcast i8* %39 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i8
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %44 = bitcast %union.jvalue* %43 to i8*
  store i8 %42, i8* %44, align 8, !tbaa !11
  br label %176

45:                                               ; preds = %23
  %46 = load i32, i32* %17, align 16
  %47 = icmp ult i32 %46, 41
  br i1 %47, label %48, label %52

48:                                               ; preds = %45
  %49 = zext i32 %46 to i64
  %50 = getelementptr i8, i8* %22, i64 %49
  %51 = add nuw nsw i32 %46, 8
  store i32 %51, i32* %17, align 16
  br label %55

52:                                               ; preds = %45
  %53 = load i8*, i8** %18, align 8
  %54 = getelementptr i8, i8* %53, i64 8
  store i8* %54, i8** %18, align 8
  br label %55

55:                                               ; preds = %52, %48
  %56 = phi i8* [ %50, %48 ], [ %53, %52 ]
  %57 = bitcast i8* %56 to i32*
  %58 = load i32, i32* %57, align 4
  %59 = trunc i32 %58 to i8
  %60 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %61 = bitcast %union.jvalue* %60 to i8*
  store i8 %59, i8* %61, align 8, !tbaa !11
  br label %176

62:                                               ; preds = %23
  %63 = load i32, i32* %17, align 16
  %64 = icmp ult i32 %63, 41
  br i1 %64, label %65, label %69

65:                                               ; preds = %62
  %66 = zext i32 %63 to i64
  %67 = getelementptr i8, i8* %22, i64 %66
  %68 = add nuw nsw i32 %63, 8
  store i32 %68, i32* %17, align 16
  br label %72

69:                                               ; preds = %62
  %70 = load i8*, i8** %18, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %18, align 8
  br label %72

72:                                               ; preds = %69, %65
  %73 = phi i8* [ %67, %65 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %176

79:                                               ; preds = %23
  %80 = load i32, i32* %17, align 16
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %86

82:                                               ; preds = %79
  %83 = zext i32 %80 to i64
  %84 = getelementptr i8, i8* %22, i64 %83
  %85 = add nuw nsw i32 %80, 8
  store i32 %85, i32* %17, align 16
  br label %89

86:                                               ; preds = %79
  %87 = load i8*, i8** %18, align 8
  %88 = getelementptr i8, i8* %87, i64 8
  store i8* %88, i8** %18, align 8
  br label %89

89:                                               ; preds = %86, %82
  %90 = phi i8* [ %84, %82 ], [ %87, %86 ]
  %91 = bitcast i8* %90 to i32*
  %92 = load i32, i32* %91, align 4
  %93 = and i32 %92, 65535
  %94 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %95 = bitcast %union.jvalue* %94 to i32*
  store i32 %93, i32* %95, align 8, !tbaa !11
  br label %176

96:                                               ; preds = %23
  %97 = load i32, i32* %17, align 16
  %98 = icmp ult i32 %97, 41
  br i1 %98, label %99, label %103

99:                                               ; preds = %96
  %100 = zext i32 %97 to i64
  %101 = getelementptr i8, i8* %22, i64 %100
  %102 = add nuw nsw i32 %97, 8
  store i32 %102, i32* %17, align 16
  br label %106

103:                                              ; preds = %96
  %104 = load i8*, i8** %18, align 8
  %105 = getelementptr i8, i8* %104, i64 8
  store i8* %105, i8** %18, align 8
  br label %106

106:                                              ; preds = %103, %99
  %107 = phi i8* [ %101, %99 ], [ %104, %103 ]
  %108 = bitcast i8* %107 to i32*
  %109 = load i32, i32* %108, align 4
  %110 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %111 = bitcast %union.jvalue* %110 to i32*
  store i32 %109, i32* %111, align 8, !tbaa !11
  br label %176

112:                                              ; preds = %23
  %113 = load i32, i32* %17, align 16
  %114 = icmp ult i32 %113, 41
  br i1 %114, label %115, label %119

115:                                              ; preds = %112
  %116 = zext i32 %113 to i64
  %117 = getelementptr i8, i8* %22, i64 %116
  %118 = add nuw nsw i32 %113, 8
  store i32 %118, i32* %17, align 16
  br label %122

119:                                              ; preds = %112
  %120 = load i8*, i8** %18, align 8
  %121 = getelementptr i8, i8* %120, i64 8
  store i8* %121, i8** %18, align 8
  br label %122

122:                                              ; preds = %119, %115
  %123 = phi i8* [ %117, %115 ], [ %120, %119 ]
  %124 = bitcast i8* %123 to i64*
  %125 = load i64, i64* %124, align 8
  %126 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24, i32 0
  store i64 %125, i64* %126, align 8, !tbaa !11
  br label %176

127:                                              ; preds = %23
  %128 = load i32, i32* %20, align 4
  %129 = icmp ult i32 %128, 161
  br i1 %129, label %130, label %134

130:                                              ; preds = %127
  %131 = zext i32 %128 to i64
  %132 = getelementptr i8, i8* %22, i64 %131
  %133 = add nuw nsw i32 %128, 16
  store i32 %133, i32* %20, align 4
  br label %137

134:                                              ; preds = %127
  %135 = load i8*, i8** %18, align 8
  %136 = getelementptr i8, i8* %135, i64 8
  store i8* %136, i8** %18, align 8
  br label %137

137:                                              ; preds = %134, %130
  %138 = phi i8* [ %132, %130 ], [ %135, %134 ]
  %139 = bitcast i8* %138 to double*
  %140 = load double, double* %139, align 8
  %141 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %142 = bitcast %union.jvalue* %141 to double*
  store double %140, double* %142, align 8, !tbaa !11
  br label %176

143:                                              ; preds = %23
  %144 = load i32, i32* %20, align 4
  %145 = icmp ult i32 %144, 161
  br i1 %145, label %146, label %150

146:                                              ; preds = %143
  %147 = zext i32 %144 to i64
  %148 = getelementptr i8, i8* %22, i64 %147
  %149 = add nuw nsw i32 %144, 16
  store i32 %149, i32* %20, align 4
  br label %153

150:                                              ; preds = %143
  %151 = load i8*, i8** %18, align 8
  %152 = getelementptr i8, i8* %151, i64 8
  store i8* %152, i8** %18, align 8
  br label %153

153:                                              ; preds = %150, %146
  %154 = phi i8* [ %148, %146 ], [ %151, %150 ]
  %155 = bitcast i8* %154 to double*
  %156 = load double, double* %155, align 8
  %157 = fptrunc double %156 to float
  %158 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %159 = bitcast %union.jvalue* %158 to float*
  store float %157, float* %159, align 8, !tbaa !11
  br label %176

160:                                              ; preds = %23
  %161 = load i32, i32* %17, align 16
  %162 = icmp ult i32 %161, 41
  br i1 %162, label %163, label %167

163:                                              ; preds = %160
  %164 = zext i32 %161 to i64
  %165 = getelementptr i8, i8* %22, i64 %164
  %166 = add nuw nsw i32 %161, 8
  store i32 %166, i32* %17, align 16
  br label %170

167:                                              ; preds = %160
  %168 = load i8*, i8** %18, align 8
  %169 = getelementptr i8, i8* %168, i64 8
  store i8* %169, i8** %18, align 8
  br label %170

170:                                              ; preds = %167, %163
  %171 = phi i8* [ %165, %163 ], [ %168, %167 ]
  %172 = bitcast i8* %171 to %struct._jobject**
  %173 = load %struct._jobject*, %struct._jobject** %172, align 8
  %174 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i64 %24
  %175 = bitcast %union.jvalue* %174 to %struct._jobject**
  store %struct._jobject* %173, %struct._jobject** %175, align 8, !tbaa !11
  br label %176

176:                                              ; preds = %170, %153, %137, %122, %106, %89, %72, %55, %38, %23
  %177 = add nuw nsw i64 %24, 1
  %178 = icmp eq i64 %177, %21
  br i1 %178, label %179, label %23, !llvm.loop !73

179:                                              ; preds = %176, %3
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %8) #4
  %180 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %181 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %180, i64 0, i32 143
  %182 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %181, align 8, !tbaa !74
  call void %182(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %14) #4
  call void @llvm.stackrestore(i8* %7)
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 24, i8* nonnull %6) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local void @JNI_CallStaticVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %struct.__va_list_tag* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !5
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !9
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #4
  %11 = sext i32 %10 to i64
  %12 = alloca %union.jvalue, i64 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %185

14:                                               ; preds = %4
  %15 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 0
  %16 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 2
  %17 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 3
  %18 = getelementptr inbounds %struct.__va_list_tag, %struct.__va_list_tag* %3, i64 0, i32 1
  %19 = zext i32 %10 to i64
  br label %20

20:                                               ; preds = %14, %182
  %21 = phi i64 [ 0, %14 ], [ %183, %182 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 %21
  %23 = load i8, i8* %22, align 1, !tbaa !11
  %24 = sext i8 %23 to i32
  switch i32 %24, label %182 [
    i32 90, label %25
    i32 66, label %43
    i32 83, label %61
    i32 67, label %79
    i32 73, label %97
    i32 74, label %114
    i32 68, label %130
    i32 70, label %147
    i32 76, label %165
  ]

25:                                               ; preds = %20
  %26 = load i32, i32* %15, align 8
  %27 = icmp ult i32 %26, 41
  br i1 %27, label %28, label %33

28:                                               ; preds = %25
  %29 = load i8*, i8** %17, align 8
  %30 = zext i32 %26 to i64
  %31 = getelementptr i8, i8* %29, i64 %30
  %32 = add nuw nsw i32 %26, 8
  store i32 %32, i32* %15, align 8
  br label %36

33:                                               ; preds = %25
  %34 = load i8*, i8** %16, align 8
  %35 = getelementptr i8, i8* %34, i64 8
  store i8* %35, i8** %16, align 8
  br label %36

36:                                               ; preds = %33, %28
  %37 = phi i8* [ %31, %28 ], [ %34, %33 ]
  %38 = bitcast i8* %37 to i32*
  %39 = load i32, i32* %38, align 4
  %40 = trunc i32 %39 to i8
  %41 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %42 = bitcast %union.jvalue* %41 to i8*
  store i8 %40, i8* %42, align 8, !tbaa !11
  br label %182

43:                                               ; preds = %20
  %44 = load i32, i32* %15, align 8
  %45 = icmp ult i32 %44, 41
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = load i8*, i8** %17, align 8
  %48 = zext i32 %44 to i64
  %49 = getelementptr i8, i8* %47, i64 %48
  %50 = add nuw nsw i32 %44, 8
  store i32 %50, i32* %15, align 8
  br label %54

51:                                               ; preds = %43
  %52 = load i8*, i8** %16, align 8
  %53 = getelementptr i8, i8* %52, i64 8
  store i8* %53, i8** %16, align 8
  br label %54

54:                                               ; preds = %51, %46
  %55 = phi i8* [ %49, %46 ], [ %52, %51 ]
  %56 = bitcast i8* %55 to i32*
  %57 = load i32, i32* %56, align 4
  %58 = trunc i32 %57 to i8
  %59 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %60 = bitcast %union.jvalue* %59 to i8*
  store i8 %58, i8* %60, align 8, !tbaa !11
  br label %182

61:                                               ; preds = %20
  %62 = load i32, i32* %15, align 8
  %63 = icmp ult i32 %62, 41
  br i1 %63, label %64, label %69

64:                                               ; preds = %61
  %65 = load i8*, i8** %17, align 8
  %66 = zext i32 %62 to i64
  %67 = getelementptr i8, i8* %65, i64 %66
  %68 = add nuw nsw i32 %62, 8
  store i32 %68, i32* %15, align 8
  br label %72

69:                                               ; preds = %61
  %70 = load i8*, i8** %16, align 8
  %71 = getelementptr i8, i8* %70, i64 8
  store i8* %71, i8** %16, align 8
  br label %72

72:                                               ; preds = %69, %64
  %73 = phi i8* [ %67, %64 ], [ %70, %69 ]
  %74 = bitcast i8* %73 to i32*
  %75 = load i32, i32* %74, align 4
  %76 = trunc i32 %75 to i16
  %77 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %78 = bitcast %union.jvalue* %77 to i16*
  store i16 %76, i16* %78, align 8, !tbaa !11
  br label %182

79:                                               ; preds = %20
  %80 = load i32, i32* %15, align 8
  %81 = icmp ult i32 %80, 41
  br i1 %81, label %82, label %87

82:                                               ; preds = %79
  %83 = load i8*, i8** %17, align 8
  %84 = zext i32 %80 to i64
  %85 = getelementptr i8, i8* %83, i64 %84
  %86 = add nuw nsw i32 %80, 8
  store i32 %86, i32* %15, align 8
  br label %90

87:                                               ; preds = %79
  %88 = load i8*, i8** %16, align 8
  %89 = getelementptr i8, i8* %88, i64 8
  store i8* %89, i8** %16, align 8
  br label %90

90:                                               ; preds = %87, %82
  %91 = phi i8* [ %85, %82 ], [ %88, %87 ]
  %92 = bitcast i8* %91 to i32*
  %93 = load i32, i32* %92, align 4
  %94 = and i32 %93, 65535
  %95 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %96 = bitcast %union.jvalue* %95 to i32*
  store i32 %94, i32* %96, align 8, !tbaa !11
  br label %182

97:                                               ; preds = %20
  %98 = load i32, i32* %15, align 8
  %99 = icmp ult i32 %98, 41
  br i1 %99, label %100, label %105

100:                                              ; preds = %97
  %101 = load i8*, i8** %17, align 8
  %102 = zext i32 %98 to i64
  %103 = getelementptr i8, i8* %101, i64 %102
  %104 = add nuw nsw i32 %98, 8
  store i32 %104, i32* %15, align 8
  br label %108

105:                                              ; preds = %97
  %106 = load i8*, i8** %16, align 8
  %107 = getelementptr i8, i8* %106, i64 8
  store i8* %107, i8** %16, align 8
  br label %108

108:                                              ; preds = %105, %100
  %109 = phi i8* [ %103, %100 ], [ %106, %105 ]
  %110 = bitcast i8* %109 to i32*
  %111 = load i32, i32* %110, align 4
  %112 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %113 = bitcast %union.jvalue* %112 to i32*
  store i32 %111, i32* %113, align 8, !tbaa !11
  br label %182

114:                                              ; preds = %20
  %115 = load i32, i32* %15, align 8
  %116 = icmp ult i32 %115, 41
  br i1 %116, label %117, label %122

117:                                              ; preds = %114
  %118 = load i8*, i8** %17, align 8
  %119 = zext i32 %115 to i64
  %120 = getelementptr i8, i8* %118, i64 %119
  %121 = add nuw nsw i32 %115, 8
  store i32 %121, i32* %15, align 8
  br label %125

122:                                              ; preds = %114
  %123 = load i8*, i8** %16, align 8
  %124 = getelementptr i8, i8* %123, i64 8
  store i8* %124, i8** %16, align 8
  br label %125

125:                                              ; preds = %122, %117
  %126 = phi i8* [ %120, %117 ], [ %123, %122 ]
  %127 = bitcast i8* %126 to i64*
  %128 = load i64, i64* %127, align 8
  %129 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21, i32 0
  store i64 %128, i64* %129, align 8, !tbaa !11
  br label %182

130:                                              ; preds = %20
  %131 = load i32, i32* %18, align 4
  %132 = icmp ult i32 %131, 161
  br i1 %132, label %133, label %138

133:                                              ; preds = %130
  %134 = load i8*, i8** %17, align 8
  %135 = zext i32 %131 to i64
  %136 = getelementptr i8, i8* %134, i64 %135
  %137 = add nuw nsw i32 %131, 16
  store i32 %137, i32* %18, align 4
  br label %141

138:                                              ; preds = %130
  %139 = load i8*, i8** %16, align 8
  %140 = getelementptr i8, i8* %139, i64 8
  store i8* %140, i8** %16, align 8
  br label %141

141:                                              ; preds = %138, %133
  %142 = phi i8* [ %136, %133 ], [ %139, %138 ]
  %143 = bitcast i8* %142 to double*
  %144 = load double, double* %143, align 8
  %145 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %146 = bitcast %union.jvalue* %145 to double*
  store double %144, double* %146, align 8, !tbaa !11
  br label %182

147:                                              ; preds = %20
  %148 = load i32, i32* %18, align 4
  %149 = icmp ult i32 %148, 161
  br i1 %149, label %150, label %155

150:                                              ; preds = %147
  %151 = load i8*, i8** %17, align 8
  %152 = zext i32 %148 to i64
  %153 = getelementptr i8, i8* %151, i64 %152
  %154 = add nuw nsw i32 %148, 16
  store i32 %154, i32* %18, align 4
  br label %158

155:                                              ; preds = %147
  %156 = load i8*, i8** %16, align 8
  %157 = getelementptr i8, i8* %156, i64 8
  store i8* %157, i8** %16, align 8
  br label %158

158:                                              ; preds = %155, %150
  %159 = phi i8* [ %153, %150 ], [ %156, %155 ]
  %160 = bitcast i8* %159 to double*
  %161 = load double, double* %160, align 8
  %162 = fptrunc double %161 to float
  %163 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %164 = bitcast %union.jvalue* %163 to float*
  store float %162, float* %164, align 8, !tbaa !11
  br label %182

165:                                              ; preds = %20
  %166 = load i32, i32* %15, align 8
  %167 = icmp ult i32 %166, 41
  br i1 %167, label %168, label %173

168:                                              ; preds = %165
  %169 = load i8*, i8** %17, align 8
  %170 = zext i32 %166 to i64
  %171 = getelementptr i8, i8* %169, i64 %170
  %172 = add nuw nsw i32 %166, 8
  store i32 %172, i32* %15, align 8
  br label %176

173:                                              ; preds = %165
  %174 = load i8*, i8** %16, align 8
  %175 = getelementptr i8, i8* %174, i64 8
  store i8* %175, i8** %16, align 8
  br label %176

176:                                              ; preds = %173, %168
  %177 = phi i8* [ %171, %168 ], [ %174, %173 ]
  %178 = bitcast i8* %177 to %struct._jobject**
  %179 = load %struct._jobject*, %struct._jobject** %178, align 8
  %180 = getelementptr inbounds %union.jvalue, %union.jvalue* %12, i64 %21
  %181 = bitcast %union.jvalue* %180 to %struct._jobject**
  store %struct._jobject* %179, %struct._jobject** %181, align 8, !tbaa !11
  br label %182

182:                                              ; preds = %36, %54, %72, %90, %108, %125, %141, %158, %176, %20
  %183 = add nuw nsw i64 %21, 1
  %184 = icmp eq i64 %183, %19
  br i1 %184, label %185, label %20, !llvm.loop !73

185:                                              ; preds = %182, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %186 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !5
  %187 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %186, i64 0, i32 143
  %188 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %187, align 8, !tbaa !74
  call void %188(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret void
}

; Function Attrs: nofree nosync nounwind willreturn
declare i8* @llvm.stacksave() #3

; Function Attrs: nofree nosync nounwind willreturn
declare void @llvm.stackrestore(i8*) #3

attributes #0 = { alwaysinline nounwind uwtable "frame-pointer"="none" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
attributes #1 = { argmemonly mustprogress nofree nosync nounwind willreturn }
attributes #2 = { mustprogress nofree nosync nounwind willreturn }
attributes #3 = { nofree nosync nounwind willreturn }
attributes #4 = { nounwind }

!llvm.module.flags = !{!0, !1, !2, !3}
!llvm.ident = !{!4}

!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 7, !"PIE Level", i32 2}
!3 = !{i32 7, !"uwtable", i32 1}
!4 = !{!"Ubuntu clang version 14.0.0-1ubuntu1"}
!5 = !{!6, !6, i64 0}
!6 = !{!"any pointer", !7, i64 0}
!7 = !{!"omnipotent char", !8, i64 0}
!8 = !{!"Simple C/C++ TBAA"}
!9 = !{!10, !6, i64 0}
!10 = !{!"JNINativeInterface_", !6, i64 0, !6, i64 8, !6, i64 16, !6, i64 24, !6, i64 32, !6, i64 40, !6, i64 48, !6, i64 56, !6, i64 64, !6, i64 72, !6, i64 80, !6, i64 88, !6, i64 96, !6, i64 104, !6, i64 112, !6, i64 120, !6, i64 128, !6, i64 136, !6, i64 144, !6, i64 152, !6, i64 160, !6, i64 168, !6, i64 176, !6, i64 184, !6, i64 192, !6, i64 200, !6, i64 208, !6, i64 216, !6, i64 224, !6, i64 232, !6, i64 240, !6, i64 248, !6, i64 256, !6, i64 264, !6, i64 272, !6, i64 280, !6, i64 288, !6, i64 296, !6, i64 304, !6, i64 312, !6, i64 320, !6, i64 328, !6, i64 336, !6, i64 344, !6, i64 352, !6, i64 360, !6, i64 368, !6, i64 376, !6, i64 384, !6, i64 392, !6, i64 400, !6, i64 408, !6, i64 416, !6, i64 424, !6, i64 432, !6, i64 440, !6, i64 448, !6, i64 456, !6, i64 464, !6, i64 472, !6, i64 480, !6, i64 488, !6, i64 496, !6, i64 504, !6, i64 512, !6, i64 520, !6, i64 528, !6, i64 536, !6, i64 544, !6, i64 552, !6, i64 560, !6, i64 568, !6, i64 576, !6, i64 584, !6, i64 592, !6, i64 600, !6, i64 608, !6, i64 616, !6, i64 624, !6, i64 632, !6, i64 640, !6, i64 648, !6, i64 656, !6, i64 664, !6, i64 672, !6, i64 680, !6, i64 688, !6, i64 696, !6, i64 704, !6, i64 712, !6, i64 720, !6, i64 728, !6, i64 736, !6, i64 744, !6, i64 752, !6, i64 760, !6, i64 768, !6, i64 776, !6, i64 784, !6, i64 792, !6, i64 800, !6, i64 808, !6, i64 816, !6, i64 824, !6, i64 832, !6, i64 840, !6, i64 848, !6, i64 856, !6, i64 864, !6, i64 872, !6, i64 880, !6, i64 888, !6, i64 896, !6, i64 904, !6, i64 912, !6, i64 920, !6, i64 928, !6, i64 936, !6, i64 944, !6, i64 952, !6, i64 960, !6, i64 968, !6, i64 976, !6, i64 984, !6, i64 992, !6, i64 1000, !6, i64 1008, !6, i64 1016, !6, i64 1024, !6, i64 1032, !6, i64 1040, !6, i64 1048, !6, i64 1056, !6, i64 1064, !6, i64 1072, !6, i64 1080, !6, i64 1088, !6, i64 1096, !6, i64 1104, !6, i64 1112, !6, i64 1120, !6, i64 1128, !6, i64 1136, !6, i64 1144, !6, i64 1152, !6, i64 1160, !6, i64 1168, !6, i64 1176, !6, i64 1184, !6, i64 1192, !6, i64 1200, !6, i64 1208, !6, i64 1216, !6, i64 1224, !6, i64 1232, !6, i64 1240, !6, i64 1248, !6, i64 1256, !6, i64 1264, !6, i64 1272, !6, i64 1280, !6, i64 1288, !6, i64 1296, !6, i64 1304, !6, i64 1312, !6, i64 1320, !6, i64 1328, !6, i64 1336, !6, i64 1344, !6, i64 1352, !6, i64 1360, !6, i64 1368, !6, i64 1376, !6, i64 1384, !6, i64 1392, !6, i64 1400, !6, i64 1408, !6, i64 1416, !6, i64 1424, !6, i64 1432, !6, i64 1440, !6, i64 1448, !6, i64 1456, !6, i64 1464, !6, i64 1472, !6, i64 1480, !6, i64 1488, !6, i64 1496, !6, i64 1504, !6, i64 1512, !6, i64 1520, !6, i64 1528, !6, i64 1536, !6, i64 1544, !6, i64 1552, !6, i64 1560, !6, i64 1568, !6, i64 1576, !6, i64 1584, !6, i64 1592, !6, i64 1600, !6, i64 1608, !6, i64 1616, !6, i64 1624, !6, i64 1632, !6, i64 1640, !6, i64 1648, !6, i64 1656, !6, i64 1664, !6, i64 1672, !6, i64 1680, !6, i64 1688, !6, i64 1696, !6, i64 1704, !6, i64 1712, !6, i64 1720, !6, i64 1728, !6, i64 1736, !6, i64 1744, !6, i64 1752, !6, i64 1760, !6, i64 1768, !6, i64 1776, !6, i64 1784, !6, i64 1792, !6, i64 1800, !6, i64 1808, !6, i64 1816, !6, i64 1824, !6, i64 1832, !6, i64 1840, !6, i64 1848, !6, i64 1856}
!11 = !{!7, !7, i64 0}
!12 = distinct !{!12, !13}
!13 = !{!"llvm.loop.mustprogress"}
!14 = !{!10, !6, i64 288}
!15 = distinct !{!15, !13}
!16 = !{!10, !6, i64 528}
!17 = distinct !{!17, !13}
!18 = !{!10, !6, i64 928}
!19 = distinct !{!19, !13}
!20 = !{!10, !6, i64 312}
!21 = distinct !{!21, !13}
!22 = !{!10, !6, i64 552}
!23 = distinct !{!23, !13}
!24 = !{!10, !6, i64 952}
!25 = distinct !{!25, !13}
!26 = !{!10, !6, i64 336}
!27 = distinct !{!27, !13}
!28 = !{!10, !6, i64 576}
!29 = distinct !{!29, !13}
!30 = !{!10, !6, i64 976}
!31 = distinct !{!31, !13}
!32 = !{!10, !6, i64 360}
!33 = distinct !{!33, !13}
!34 = !{!10, !6, i64 600}
!35 = distinct !{!35, !13}
!36 = !{!10, !6, i64 1000}
!37 = distinct !{!37, !13}
!38 = !{!10, !6, i64 384}
!39 = distinct !{!39, !13}
!40 = !{!10, !6, i64 624}
!41 = distinct !{!41, !13}
!42 = !{!10, !6, i64 1024}
!43 = distinct !{!43, !13}
!44 = !{!10, !6, i64 408}
!45 = distinct !{!45, !13}
!46 = !{!10, !6, i64 648}
!47 = distinct !{!47, !13}
!48 = !{!10, !6, i64 1048}
!49 = distinct !{!49, !13}
!50 = !{!10, !6, i64 432}
!51 = distinct !{!51, !13}
!52 = !{!10, !6, i64 672}
!53 = distinct !{!53, !13}
!54 = !{!10, !6, i64 1072}
!55 = distinct !{!55, !13}
!56 = !{!10, !6, i64 456}
!57 = distinct !{!57, !13}
!58 = !{!10, !6, i64 696}
!59 = distinct !{!59, !13}
!60 = !{!10, !6, i64 1096}
!61 = distinct !{!61, !13}
!62 = !{!10, !6, i64 480}
!63 = distinct !{!63, !13}
!64 = !{!10, !6, i64 720}
!65 = distinct !{!65, !13}
!66 = !{!10, !6, i64 1120}
!67 = distinct !{!67, !13}
!68 = !{!10, !6, i64 240}
!69 = distinct !{!69, !13}
!70 = !{!10, !6, i64 504}
!71 = distinct !{!71, !13}
!72 = !{!10, !6, i64 744}
!73 = distinct !{!73, !13}
!74 = !{!10, !6, i64 1144}
