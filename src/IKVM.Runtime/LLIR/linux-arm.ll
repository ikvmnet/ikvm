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
define dso_local %struct._jobject* @JNI_CallObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 35
  %8 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !13
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  %12 = call %struct._jobject* %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret %struct._jobject* %12
}

; Function Attrs: argmemonly mustprogress nofree nosync nounwind willreturn
declare void @llvm.lifetime.start.p0i8(i64 immarg, i8* nocapture) #1

; Function Attrs: mustprogress nofree nosync nounwind willreturn
declare void @llvm.va_start(i8*) #2

; Function Attrs: mustprogress nofree nosync nounwind willreturn
declare void @llvm.va_end(i8*) #2

; Function Attrs: argmemonly mustprogress nofree nosync nounwind willreturn
declare void @llvm.lifetime.end.p0i8(i64 immarg, i8* nocapture) #1

; Function Attrs: alwaysinline nounwind
define dso_local %struct._jobject* @JNI_CallObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !17

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 36
  %97 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !19
  %98 = call %struct._jobject* %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret %struct._jobject* %98
}

; Function Attrs: alwaysinline nounwind
define dso_local %struct._jobject* @JNI_CallNonvirtualObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = bitcast %struct.__va_list* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %6) #3
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i32 0, i32 65
  %9 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %8, align 4, !tbaa !20
  %10 = bitcast %struct.__va_list* %5 to i32*
  %11 = load i32, i32* %10, align 4
  %12 = insertvalue [1 x i32] poison, i32 %11, 0
  %13 = call %struct._jobject* %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %12) #3
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %6) #3
  ret %struct._jobject* %13
}

; Function Attrs: alwaysinline nounwind
define dso_local %struct._jobject* @JNI_CallNonvirtualObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #3
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 4, !tbaa !9
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 4, !tbaa !15
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #3
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 8
  %14 = bitcast i8* %13 to %union.jvalue*
  %15 = icmp sgt i32 %11, 0
  br i1 %15, label %16, label %95

16:                                               ; preds = %5
  %17 = extractvalue [1 x i32] %4, 0
  %18 = inttoptr i32 %17 to i8*
  br label %19

19:                                               ; preds = %16, %91
  %20 = phi i32 [ %93, %91 ], [ 0, %16 ]
  %21 = phi i8* [ %92, %91 ], [ %18, %16 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i32 0, i32 %20
  %23 = load i8, i8* %22, align 1, !tbaa !16
  switch i8 %23, label %91 [
    i8 90, label %24
    i8 66, label %31
    i8 83, label %38
    i8 67, label %45
    i8 73, label %52
    i8 74, label %58
    i8 68, label %64
    i8 70, label %74
    i8 76, label %85
  ]

24:                                               ; preds = %19
  %25 = getelementptr inbounds i8, i8* %21, i32 4
  %26 = bitcast i8* %21 to i32*
  %27 = load i32, i32* %26, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %30 = bitcast %union.jvalue* %29 to i8*
  store i8 %28, i8* %30, align 8, !tbaa !16
  br label %91

31:                                               ; preds = %19
  %32 = getelementptr inbounds i8, i8* %21, i32 4
  %33 = bitcast i8* %21 to i32*
  %34 = load i32, i32* %33, align 4
  %35 = trunc i32 %34 to i8
  %36 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %37 = bitcast %union.jvalue* %36 to i8*
  store i8 %35, i8* %37, align 8, !tbaa !16
  br label %91

38:                                               ; preds = %19
  %39 = getelementptr inbounds i8, i8* %21, i32 4
  %40 = bitcast i8* %21 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i16
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %44 = bitcast %union.jvalue* %43 to i16*
  store i16 %42, i16* %44, align 8, !tbaa !16
  br label %91

45:                                               ; preds = %19
  %46 = getelementptr inbounds i8, i8* %21, i32 4
  %47 = bitcast i8* %21 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = and i32 %48, 65535
  %50 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %51 = bitcast %union.jvalue* %50 to i32*
  store i32 %49, i32* %51, align 8, !tbaa !16
  br label %91

52:                                               ; preds = %19
  %53 = getelementptr inbounds i8, i8* %21, i32 4
  %54 = bitcast i8* %21 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %57 = bitcast %union.jvalue* %56 to i32*
  store i32 %55, i32* %57, align 8, !tbaa !16
  br label %91

58:                                               ; preds = %19
  %59 = getelementptr inbounds i8, i8* %21, i32 4
  %60 = bitcast i8* %21 to i32*
  %61 = load i32, i32* %60, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20, i32 0
  store i64 %62, i64* %63, align 8, !tbaa !16
  br label %91

64:                                               ; preds = %19
  %65 = ptrtoint i8* %21 to i32
  %66 = add i32 %65, 7
  %67 = and i32 %66, -8
  %68 = inttoptr i32 %67 to i8*
  %69 = getelementptr inbounds i8, i8* %68, i32 8
  %70 = inttoptr i32 %67 to double*
  %71 = load double, double* %70, align 8
  %72 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %73 = bitcast %union.jvalue* %72 to double*
  store double %71, double* %73, align 8, !tbaa !16
  br label %91

74:                                               ; preds = %19
  %75 = ptrtoint i8* %21 to i32
  %76 = add i32 %75, 7
  %77 = and i32 %76, -8
  %78 = inttoptr i32 %77 to i8*
  %79 = getelementptr inbounds i8, i8* %78, i32 8
  %80 = inttoptr i32 %77 to double*
  %81 = load double, double* %80, align 8
  %82 = fptrunc double %81 to float
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %84 = bitcast %union.jvalue* %83 to float*
  store float %82, float* %84, align 8, !tbaa !16
  br label %91

85:                                               ; preds = %19
  %86 = getelementptr inbounds i8, i8* %21, i32 4
  %87 = bitcast i8* %21 to %struct._jobject**
  %88 = load %struct._jobject*, %struct._jobject** %87, align 4
  %89 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %90 = bitcast %union.jvalue* %89 to %struct._jobject**
  store %struct._jobject* %88, %struct._jobject** %90, align 8, !tbaa !16
  br label %91

91:                                               ; preds = %24, %31, %38, %45, %52, %58, %64, %74, %85, %19
  %92 = phi i8* [ %21, %19 ], [ %86, %85 ], [ %79, %74 ], [ %69, %64 ], [ %59, %58 ], [ %53, %52 ], [ %46, %45 ], [ %39, %38 ], [ %32, %31 ], [ %25, %24 ]
  %93 = add nuw nsw i32 %20, 1
  %94 = icmp eq i32 %93, %11
  br i1 %94, label %95, label %19, !llvm.loop !21

95:                                               ; preds = %91, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #3
  %96 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %97 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %96, i32 0, i32 66
  %98 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %97, align 4, !tbaa !22
  %99 = call %struct._jobject* %98(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %14) #3
  ret %struct._jobject* %99
}

; Function Attrs: alwaysinline nounwind
define dso_local %struct._jobject* @JNI_CallStaticObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 115
  %8 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !23
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  %12 = call %struct._jobject* %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret %struct._jobject* %12
}

; Function Attrs: alwaysinline nounwind
define dso_local %struct._jobject* @JNI_CallStaticObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !24

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 116
  %97 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !25
  %98 = call %struct._jobject* %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret %struct._jobject* %98
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i8 @JNI_CallBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 38
  %8 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !26
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  %12 = call zeroext i8 %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret i8 %12
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i8 @JNI_CallBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !27

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 39
  %97 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !28
  %98 = call zeroext i8 %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret i8 %98
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i8 @JNI_CallNonvirtualBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = bitcast %struct.__va_list* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %6) #3
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i32 0, i32 68
  %9 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %8, align 4, !tbaa !29
  %10 = bitcast %struct.__va_list* %5 to i32*
  %11 = load i32, i32* %10, align 4
  %12 = insertvalue [1 x i32] poison, i32 %11, 0
  %13 = call zeroext i8 %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %12) #3
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %6) #3
  ret i8 %13
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i8 @JNI_CallNonvirtualBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #3
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 4, !tbaa !9
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 4, !tbaa !15
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #3
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 8
  %14 = bitcast i8* %13 to %union.jvalue*
  %15 = icmp sgt i32 %11, 0
  br i1 %15, label %16, label %95

16:                                               ; preds = %5
  %17 = extractvalue [1 x i32] %4, 0
  %18 = inttoptr i32 %17 to i8*
  br label %19

19:                                               ; preds = %16, %91
  %20 = phi i32 [ %93, %91 ], [ 0, %16 ]
  %21 = phi i8* [ %92, %91 ], [ %18, %16 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i32 0, i32 %20
  %23 = load i8, i8* %22, align 1, !tbaa !16
  switch i8 %23, label %91 [
    i8 90, label %24
    i8 66, label %31
    i8 83, label %38
    i8 67, label %45
    i8 73, label %52
    i8 74, label %58
    i8 68, label %64
    i8 70, label %74
    i8 76, label %85
  ]

24:                                               ; preds = %19
  %25 = getelementptr inbounds i8, i8* %21, i32 4
  %26 = bitcast i8* %21 to i32*
  %27 = load i32, i32* %26, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %30 = bitcast %union.jvalue* %29 to i8*
  store i8 %28, i8* %30, align 8, !tbaa !16
  br label %91

31:                                               ; preds = %19
  %32 = getelementptr inbounds i8, i8* %21, i32 4
  %33 = bitcast i8* %21 to i32*
  %34 = load i32, i32* %33, align 4
  %35 = trunc i32 %34 to i8
  %36 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %37 = bitcast %union.jvalue* %36 to i8*
  store i8 %35, i8* %37, align 8, !tbaa !16
  br label %91

38:                                               ; preds = %19
  %39 = getelementptr inbounds i8, i8* %21, i32 4
  %40 = bitcast i8* %21 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i16
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %44 = bitcast %union.jvalue* %43 to i16*
  store i16 %42, i16* %44, align 8, !tbaa !16
  br label %91

45:                                               ; preds = %19
  %46 = getelementptr inbounds i8, i8* %21, i32 4
  %47 = bitcast i8* %21 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = and i32 %48, 65535
  %50 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %51 = bitcast %union.jvalue* %50 to i32*
  store i32 %49, i32* %51, align 8, !tbaa !16
  br label %91

52:                                               ; preds = %19
  %53 = getelementptr inbounds i8, i8* %21, i32 4
  %54 = bitcast i8* %21 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %57 = bitcast %union.jvalue* %56 to i32*
  store i32 %55, i32* %57, align 8, !tbaa !16
  br label %91

58:                                               ; preds = %19
  %59 = getelementptr inbounds i8, i8* %21, i32 4
  %60 = bitcast i8* %21 to i32*
  %61 = load i32, i32* %60, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20, i32 0
  store i64 %62, i64* %63, align 8, !tbaa !16
  br label %91

64:                                               ; preds = %19
  %65 = ptrtoint i8* %21 to i32
  %66 = add i32 %65, 7
  %67 = and i32 %66, -8
  %68 = inttoptr i32 %67 to i8*
  %69 = getelementptr inbounds i8, i8* %68, i32 8
  %70 = inttoptr i32 %67 to double*
  %71 = load double, double* %70, align 8
  %72 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %73 = bitcast %union.jvalue* %72 to double*
  store double %71, double* %73, align 8, !tbaa !16
  br label %91

74:                                               ; preds = %19
  %75 = ptrtoint i8* %21 to i32
  %76 = add i32 %75, 7
  %77 = and i32 %76, -8
  %78 = inttoptr i32 %77 to i8*
  %79 = getelementptr inbounds i8, i8* %78, i32 8
  %80 = inttoptr i32 %77 to double*
  %81 = load double, double* %80, align 8
  %82 = fptrunc double %81 to float
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %84 = bitcast %union.jvalue* %83 to float*
  store float %82, float* %84, align 8, !tbaa !16
  br label %91

85:                                               ; preds = %19
  %86 = getelementptr inbounds i8, i8* %21, i32 4
  %87 = bitcast i8* %21 to %struct._jobject**
  %88 = load %struct._jobject*, %struct._jobject** %87, align 4
  %89 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %90 = bitcast %union.jvalue* %89 to %struct._jobject**
  store %struct._jobject* %88, %struct._jobject** %90, align 8, !tbaa !16
  br label %91

91:                                               ; preds = %24, %31, %38, %45, %52, %58, %64, %74, %85, %19
  %92 = phi i8* [ %21, %19 ], [ %86, %85 ], [ %79, %74 ], [ %69, %64 ], [ %59, %58 ], [ %53, %52 ], [ %46, %45 ], [ %39, %38 ], [ %32, %31 ], [ %25, %24 ]
  %93 = add nuw nsw i32 %20, 1
  %94 = icmp eq i32 %93, %11
  br i1 %94, label %95, label %19, !llvm.loop !30

95:                                               ; preds = %91, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #3
  %96 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %97 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %96, i32 0, i32 69
  %98 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %97, align 4, !tbaa !31
  %99 = call zeroext i8 %98(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %14) #3
  ret i8 %99
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i8 @JNI_CallStaticBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 118
  %8 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !32
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  %12 = call zeroext i8 %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret i8 %12
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i8 @JNI_CallStaticBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !33

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 119
  %97 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !34
  %98 = call zeroext i8 %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret i8 %98
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i8 @JNI_CallByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 41
  %8 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !35
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  %12 = call signext i8 %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret i8 %12
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i8 @JNI_CallByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !36

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 42
  %97 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !37
  %98 = call signext i8 %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret i8 %98
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i8 @JNI_CallNonvirtualByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = bitcast %struct.__va_list* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %6) #3
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i32 0, i32 71
  %9 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %8, align 4, !tbaa !38
  %10 = bitcast %struct.__va_list* %5 to i32*
  %11 = load i32, i32* %10, align 4
  %12 = insertvalue [1 x i32] poison, i32 %11, 0
  %13 = call signext i8 %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %12) #3
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %6) #3
  ret i8 %13
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i8 @JNI_CallNonvirtualByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #3
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 4, !tbaa !9
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 4, !tbaa !15
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #3
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 8
  %14 = bitcast i8* %13 to %union.jvalue*
  %15 = icmp sgt i32 %11, 0
  br i1 %15, label %16, label %95

16:                                               ; preds = %5
  %17 = extractvalue [1 x i32] %4, 0
  %18 = inttoptr i32 %17 to i8*
  br label %19

19:                                               ; preds = %16, %91
  %20 = phi i32 [ %93, %91 ], [ 0, %16 ]
  %21 = phi i8* [ %92, %91 ], [ %18, %16 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i32 0, i32 %20
  %23 = load i8, i8* %22, align 1, !tbaa !16
  switch i8 %23, label %91 [
    i8 90, label %24
    i8 66, label %31
    i8 83, label %38
    i8 67, label %45
    i8 73, label %52
    i8 74, label %58
    i8 68, label %64
    i8 70, label %74
    i8 76, label %85
  ]

24:                                               ; preds = %19
  %25 = getelementptr inbounds i8, i8* %21, i32 4
  %26 = bitcast i8* %21 to i32*
  %27 = load i32, i32* %26, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %30 = bitcast %union.jvalue* %29 to i8*
  store i8 %28, i8* %30, align 8, !tbaa !16
  br label %91

31:                                               ; preds = %19
  %32 = getelementptr inbounds i8, i8* %21, i32 4
  %33 = bitcast i8* %21 to i32*
  %34 = load i32, i32* %33, align 4
  %35 = trunc i32 %34 to i8
  %36 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %37 = bitcast %union.jvalue* %36 to i8*
  store i8 %35, i8* %37, align 8, !tbaa !16
  br label %91

38:                                               ; preds = %19
  %39 = getelementptr inbounds i8, i8* %21, i32 4
  %40 = bitcast i8* %21 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i16
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %44 = bitcast %union.jvalue* %43 to i16*
  store i16 %42, i16* %44, align 8, !tbaa !16
  br label %91

45:                                               ; preds = %19
  %46 = getelementptr inbounds i8, i8* %21, i32 4
  %47 = bitcast i8* %21 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = and i32 %48, 65535
  %50 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %51 = bitcast %union.jvalue* %50 to i32*
  store i32 %49, i32* %51, align 8, !tbaa !16
  br label %91

52:                                               ; preds = %19
  %53 = getelementptr inbounds i8, i8* %21, i32 4
  %54 = bitcast i8* %21 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %57 = bitcast %union.jvalue* %56 to i32*
  store i32 %55, i32* %57, align 8, !tbaa !16
  br label %91

58:                                               ; preds = %19
  %59 = getelementptr inbounds i8, i8* %21, i32 4
  %60 = bitcast i8* %21 to i32*
  %61 = load i32, i32* %60, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20, i32 0
  store i64 %62, i64* %63, align 8, !tbaa !16
  br label %91

64:                                               ; preds = %19
  %65 = ptrtoint i8* %21 to i32
  %66 = add i32 %65, 7
  %67 = and i32 %66, -8
  %68 = inttoptr i32 %67 to i8*
  %69 = getelementptr inbounds i8, i8* %68, i32 8
  %70 = inttoptr i32 %67 to double*
  %71 = load double, double* %70, align 8
  %72 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %73 = bitcast %union.jvalue* %72 to double*
  store double %71, double* %73, align 8, !tbaa !16
  br label %91

74:                                               ; preds = %19
  %75 = ptrtoint i8* %21 to i32
  %76 = add i32 %75, 7
  %77 = and i32 %76, -8
  %78 = inttoptr i32 %77 to i8*
  %79 = getelementptr inbounds i8, i8* %78, i32 8
  %80 = inttoptr i32 %77 to double*
  %81 = load double, double* %80, align 8
  %82 = fptrunc double %81 to float
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %84 = bitcast %union.jvalue* %83 to float*
  store float %82, float* %84, align 8, !tbaa !16
  br label %91

85:                                               ; preds = %19
  %86 = getelementptr inbounds i8, i8* %21, i32 4
  %87 = bitcast i8* %21 to %struct._jobject**
  %88 = load %struct._jobject*, %struct._jobject** %87, align 4
  %89 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %90 = bitcast %union.jvalue* %89 to %struct._jobject**
  store %struct._jobject* %88, %struct._jobject** %90, align 8, !tbaa !16
  br label %91

91:                                               ; preds = %24, %31, %38, %45, %52, %58, %64, %74, %85, %19
  %92 = phi i8* [ %21, %19 ], [ %86, %85 ], [ %79, %74 ], [ %69, %64 ], [ %59, %58 ], [ %53, %52 ], [ %46, %45 ], [ %39, %38 ], [ %32, %31 ], [ %25, %24 ]
  %93 = add nuw nsw i32 %20, 1
  %94 = icmp eq i32 %93, %11
  br i1 %94, label %95, label %19, !llvm.loop !39

95:                                               ; preds = %91, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #3
  %96 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %97 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %96, i32 0, i32 72
  %98 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %97, align 4, !tbaa !40
  %99 = call signext i8 %98(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %14) #3
  ret i8 %99
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i8 @JNI_CallStaticByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 121
  %8 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !41
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  %12 = call signext i8 %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret i8 %12
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i8 @JNI_CallStaticByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !42

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 122
  %97 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !43
  %98 = call signext i8 %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret i8 %98
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i16 @JNI_CallCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 44
  %8 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !44
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  %12 = call zeroext i16 %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret i16 %12
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i16 @JNI_CallCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !45

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 45
  %97 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !46
  %98 = call zeroext i16 %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret i16 %98
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i16 @JNI_CallNonvirtualCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = bitcast %struct.__va_list* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %6) #3
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i32 0, i32 74
  %9 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %8, align 4, !tbaa !47
  %10 = bitcast %struct.__va_list* %5 to i32*
  %11 = load i32, i32* %10, align 4
  %12 = insertvalue [1 x i32] poison, i32 %11, 0
  %13 = call zeroext i16 %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %12) #3
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %6) #3
  ret i16 %13
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i16 @JNI_CallNonvirtualCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #3
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 4, !tbaa !9
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 4, !tbaa !15
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #3
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 8
  %14 = bitcast i8* %13 to %union.jvalue*
  %15 = icmp sgt i32 %11, 0
  br i1 %15, label %16, label %95

16:                                               ; preds = %5
  %17 = extractvalue [1 x i32] %4, 0
  %18 = inttoptr i32 %17 to i8*
  br label %19

19:                                               ; preds = %16, %91
  %20 = phi i32 [ %93, %91 ], [ 0, %16 ]
  %21 = phi i8* [ %92, %91 ], [ %18, %16 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i32 0, i32 %20
  %23 = load i8, i8* %22, align 1, !tbaa !16
  switch i8 %23, label %91 [
    i8 90, label %24
    i8 66, label %31
    i8 83, label %38
    i8 67, label %45
    i8 73, label %52
    i8 74, label %58
    i8 68, label %64
    i8 70, label %74
    i8 76, label %85
  ]

24:                                               ; preds = %19
  %25 = getelementptr inbounds i8, i8* %21, i32 4
  %26 = bitcast i8* %21 to i32*
  %27 = load i32, i32* %26, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %30 = bitcast %union.jvalue* %29 to i8*
  store i8 %28, i8* %30, align 8, !tbaa !16
  br label %91

31:                                               ; preds = %19
  %32 = getelementptr inbounds i8, i8* %21, i32 4
  %33 = bitcast i8* %21 to i32*
  %34 = load i32, i32* %33, align 4
  %35 = trunc i32 %34 to i8
  %36 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %37 = bitcast %union.jvalue* %36 to i8*
  store i8 %35, i8* %37, align 8, !tbaa !16
  br label %91

38:                                               ; preds = %19
  %39 = getelementptr inbounds i8, i8* %21, i32 4
  %40 = bitcast i8* %21 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i16
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %44 = bitcast %union.jvalue* %43 to i16*
  store i16 %42, i16* %44, align 8, !tbaa !16
  br label %91

45:                                               ; preds = %19
  %46 = getelementptr inbounds i8, i8* %21, i32 4
  %47 = bitcast i8* %21 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = and i32 %48, 65535
  %50 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %51 = bitcast %union.jvalue* %50 to i32*
  store i32 %49, i32* %51, align 8, !tbaa !16
  br label %91

52:                                               ; preds = %19
  %53 = getelementptr inbounds i8, i8* %21, i32 4
  %54 = bitcast i8* %21 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %57 = bitcast %union.jvalue* %56 to i32*
  store i32 %55, i32* %57, align 8, !tbaa !16
  br label %91

58:                                               ; preds = %19
  %59 = getelementptr inbounds i8, i8* %21, i32 4
  %60 = bitcast i8* %21 to i32*
  %61 = load i32, i32* %60, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20, i32 0
  store i64 %62, i64* %63, align 8, !tbaa !16
  br label %91

64:                                               ; preds = %19
  %65 = ptrtoint i8* %21 to i32
  %66 = add i32 %65, 7
  %67 = and i32 %66, -8
  %68 = inttoptr i32 %67 to i8*
  %69 = getelementptr inbounds i8, i8* %68, i32 8
  %70 = inttoptr i32 %67 to double*
  %71 = load double, double* %70, align 8
  %72 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %73 = bitcast %union.jvalue* %72 to double*
  store double %71, double* %73, align 8, !tbaa !16
  br label %91

74:                                               ; preds = %19
  %75 = ptrtoint i8* %21 to i32
  %76 = add i32 %75, 7
  %77 = and i32 %76, -8
  %78 = inttoptr i32 %77 to i8*
  %79 = getelementptr inbounds i8, i8* %78, i32 8
  %80 = inttoptr i32 %77 to double*
  %81 = load double, double* %80, align 8
  %82 = fptrunc double %81 to float
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %84 = bitcast %union.jvalue* %83 to float*
  store float %82, float* %84, align 8, !tbaa !16
  br label %91

85:                                               ; preds = %19
  %86 = getelementptr inbounds i8, i8* %21, i32 4
  %87 = bitcast i8* %21 to %struct._jobject**
  %88 = load %struct._jobject*, %struct._jobject** %87, align 4
  %89 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %90 = bitcast %union.jvalue* %89 to %struct._jobject**
  store %struct._jobject* %88, %struct._jobject** %90, align 8, !tbaa !16
  br label %91

91:                                               ; preds = %24, %31, %38, %45, %52, %58, %64, %74, %85, %19
  %92 = phi i8* [ %21, %19 ], [ %86, %85 ], [ %79, %74 ], [ %69, %64 ], [ %59, %58 ], [ %53, %52 ], [ %46, %45 ], [ %39, %38 ], [ %32, %31 ], [ %25, %24 ]
  %93 = add nuw nsw i32 %20, 1
  %94 = icmp eq i32 %93, %11
  br i1 %94, label %95, label %19, !llvm.loop !48

95:                                               ; preds = %91, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #3
  %96 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %97 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %96, i32 0, i32 75
  %98 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %97, align 4, !tbaa !49
  %99 = call zeroext i16 %98(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %14) #3
  ret i16 %99
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i16 @JNI_CallStaticCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 124
  %8 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !50
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  %12 = call zeroext i16 %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret i16 %12
}

; Function Attrs: alwaysinline nounwind
define dso_local zeroext i16 @JNI_CallStaticCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !51

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 125
  %97 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !52
  %98 = call zeroext i16 %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret i16 %98
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i16 @JNI_CallShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 47
  %8 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !53
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  %12 = call signext i16 %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret i16 %12
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i16 @JNI_CallShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !54

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 48
  %97 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !55
  %98 = call signext i16 %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret i16 %98
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i16 @JNI_CallNonvirtualShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = bitcast %struct.__va_list* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %6) #3
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i32 0, i32 77
  %9 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %8, align 4, !tbaa !56
  %10 = bitcast %struct.__va_list* %5 to i32*
  %11 = load i32, i32* %10, align 4
  %12 = insertvalue [1 x i32] poison, i32 %11, 0
  %13 = call signext i16 %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %12) #3
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %6) #3
  ret i16 %13
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i16 @JNI_CallNonvirtualShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #3
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 4, !tbaa !9
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 4, !tbaa !15
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #3
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 8
  %14 = bitcast i8* %13 to %union.jvalue*
  %15 = icmp sgt i32 %11, 0
  br i1 %15, label %16, label %95

16:                                               ; preds = %5
  %17 = extractvalue [1 x i32] %4, 0
  %18 = inttoptr i32 %17 to i8*
  br label %19

19:                                               ; preds = %16, %91
  %20 = phi i32 [ %93, %91 ], [ 0, %16 ]
  %21 = phi i8* [ %92, %91 ], [ %18, %16 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i32 0, i32 %20
  %23 = load i8, i8* %22, align 1, !tbaa !16
  switch i8 %23, label %91 [
    i8 90, label %24
    i8 66, label %31
    i8 83, label %38
    i8 67, label %45
    i8 73, label %52
    i8 74, label %58
    i8 68, label %64
    i8 70, label %74
    i8 76, label %85
  ]

24:                                               ; preds = %19
  %25 = getelementptr inbounds i8, i8* %21, i32 4
  %26 = bitcast i8* %21 to i32*
  %27 = load i32, i32* %26, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %30 = bitcast %union.jvalue* %29 to i8*
  store i8 %28, i8* %30, align 8, !tbaa !16
  br label %91

31:                                               ; preds = %19
  %32 = getelementptr inbounds i8, i8* %21, i32 4
  %33 = bitcast i8* %21 to i32*
  %34 = load i32, i32* %33, align 4
  %35 = trunc i32 %34 to i8
  %36 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %37 = bitcast %union.jvalue* %36 to i8*
  store i8 %35, i8* %37, align 8, !tbaa !16
  br label %91

38:                                               ; preds = %19
  %39 = getelementptr inbounds i8, i8* %21, i32 4
  %40 = bitcast i8* %21 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i16
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %44 = bitcast %union.jvalue* %43 to i16*
  store i16 %42, i16* %44, align 8, !tbaa !16
  br label %91

45:                                               ; preds = %19
  %46 = getelementptr inbounds i8, i8* %21, i32 4
  %47 = bitcast i8* %21 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = and i32 %48, 65535
  %50 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %51 = bitcast %union.jvalue* %50 to i32*
  store i32 %49, i32* %51, align 8, !tbaa !16
  br label %91

52:                                               ; preds = %19
  %53 = getelementptr inbounds i8, i8* %21, i32 4
  %54 = bitcast i8* %21 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %57 = bitcast %union.jvalue* %56 to i32*
  store i32 %55, i32* %57, align 8, !tbaa !16
  br label %91

58:                                               ; preds = %19
  %59 = getelementptr inbounds i8, i8* %21, i32 4
  %60 = bitcast i8* %21 to i32*
  %61 = load i32, i32* %60, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20, i32 0
  store i64 %62, i64* %63, align 8, !tbaa !16
  br label %91

64:                                               ; preds = %19
  %65 = ptrtoint i8* %21 to i32
  %66 = add i32 %65, 7
  %67 = and i32 %66, -8
  %68 = inttoptr i32 %67 to i8*
  %69 = getelementptr inbounds i8, i8* %68, i32 8
  %70 = inttoptr i32 %67 to double*
  %71 = load double, double* %70, align 8
  %72 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %73 = bitcast %union.jvalue* %72 to double*
  store double %71, double* %73, align 8, !tbaa !16
  br label %91

74:                                               ; preds = %19
  %75 = ptrtoint i8* %21 to i32
  %76 = add i32 %75, 7
  %77 = and i32 %76, -8
  %78 = inttoptr i32 %77 to i8*
  %79 = getelementptr inbounds i8, i8* %78, i32 8
  %80 = inttoptr i32 %77 to double*
  %81 = load double, double* %80, align 8
  %82 = fptrunc double %81 to float
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %84 = bitcast %union.jvalue* %83 to float*
  store float %82, float* %84, align 8, !tbaa !16
  br label %91

85:                                               ; preds = %19
  %86 = getelementptr inbounds i8, i8* %21, i32 4
  %87 = bitcast i8* %21 to %struct._jobject**
  %88 = load %struct._jobject*, %struct._jobject** %87, align 4
  %89 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %90 = bitcast %union.jvalue* %89 to %struct._jobject**
  store %struct._jobject* %88, %struct._jobject** %90, align 8, !tbaa !16
  br label %91

91:                                               ; preds = %24, %31, %38, %45, %52, %58, %64, %74, %85, %19
  %92 = phi i8* [ %21, %19 ], [ %86, %85 ], [ %79, %74 ], [ %69, %64 ], [ %59, %58 ], [ %53, %52 ], [ %46, %45 ], [ %39, %38 ], [ %32, %31 ], [ %25, %24 ]
  %93 = add nuw nsw i32 %20, 1
  %94 = icmp eq i32 %93, %11
  br i1 %94, label %95, label %19, !llvm.loop !57

95:                                               ; preds = %91, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #3
  %96 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %97 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %96, i32 0, i32 78
  %98 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %97, align 4, !tbaa !58
  %99 = call signext i16 %98(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %14) #3
  ret i16 %99
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i16 @JNI_CallStaticShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 127
  %8 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !59
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  %12 = call signext i16 %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret i16 %12
}

; Function Attrs: alwaysinline nounwind
define dso_local signext i16 @JNI_CallStaticShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !60

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 128
  %97 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !61
  %98 = call signext i16 %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret i16 %98
}

; Function Attrs: alwaysinline nounwind
define dso_local i32 @JNI_CallIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 50
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !62
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  %12 = call i32 %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret i32 %12
}

; Function Attrs: alwaysinline nounwind
define dso_local i32 @JNI_CallIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !63

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 51
  %97 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !64
  %98 = call i32 %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret i32 %98
}

; Function Attrs: alwaysinline nounwind
define dso_local i32 @JNI_CallNonvirtualIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = bitcast %struct.__va_list* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %6) #3
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i32 0, i32 80
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %8, align 4, !tbaa !65
  %10 = bitcast %struct.__va_list* %5 to i32*
  %11 = load i32, i32* %10, align 4
  %12 = insertvalue [1 x i32] poison, i32 %11, 0
  %13 = call i32 %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %12) #3
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %6) #3
  ret i32 %13
}

; Function Attrs: alwaysinline nounwind
define dso_local i32 @JNI_CallNonvirtualIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #3
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 4, !tbaa !9
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 4, !tbaa !15
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #3
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 8
  %14 = bitcast i8* %13 to %union.jvalue*
  %15 = icmp sgt i32 %11, 0
  br i1 %15, label %16, label %95

16:                                               ; preds = %5
  %17 = extractvalue [1 x i32] %4, 0
  %18 = inttoptr i32 %17 to i8*
  br label %19

19:                                               ; preds = %16, %91
  %20 = phi i32 [ %93, %91 ], [ 0, %16 ]
  %21 = phi i8* [ %92, %91 ], [ %18, %16 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i32 0, i32 %20
  %23 = load i8, i8* %22, align 1, !tbaa !16
  switch i8 %23, label %91 [
    i8 90, label %24
    i8 66, label %31
    i8 83, label %38
    i8 67, label %45
    i8 73, label %52
    i8 74, label %58
    i8 68, label %64
    i8 70, label %74
    i8 76, label %85
  ]

24:                                               ; preds = %19
  %25 = getelementptr inbounds i8, i8* %21, i32 4
  %26 = bitcast i8* %21 to i32*
  %27 = load i32, i32* %26, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %30 = bitcast %union.jvalue* %29 to i8*
  store i8 %28, i8* %30, align 8, !tbaa !16
  br label %91

31:                                               ; preds = %19
  %32 = getelementptr inbounds i8, i8* %21, i32 4
  %33 = bitcast i8* %21 to i32*
  %34 = load i32, i32* %33, align 4
  %35 = trunc i32 %34 to i8
  %36 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %37 = bitcast %union.jvalue* %36 to i8*
  store i8 %35, i8* %37, align 8, !tbaa !16
  br label %91

38:                                               ; preds = %19
  %39 = getelementptr inbounds i8, i8* %21, i32 4
  %40 = bitcast i8* %21 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i16
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %44 = bitcast %union.jvalue* %43 to i16*
  store i16 %42, i16* %44, align 8, !tbaa !16
  br label %91

45:                                               ; preds = %19
  %46 = getelementptr inbounds i8, i8* %21, i32 4
  %47 = bitcast i8* %21 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = and i32 %48, 65535
  %50 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %51 = bitcast %union.jvalue* %50 to i32*
  store i32 %49, i32* %51, align 8, !tbaa !16
  br label %91

52:                                               ; preds = %19
  %53 = getelementptr inbounds i8, i8* %21, i32 4
  %54 = bitcast i8* %21 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %57 = bitcast %union.jvalue* %56 to i32*
  store i32 %55, i32* %57, align 8, !tbaa !16
  br label %91

58:                                               ; preds = %19
  %59 = getelementptr inbounds i8, i8* %21, i32 4
  %60 = bitcast i8* %21 to i32*
  %61 = load i32, i32* %60, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20, i32 0
  store i64 %62, i64* %63, align 8, !tbaa !16
  br label %91

64:                                               ; preds = %19
  %65 = ptrtoint i8* %21 to i32
  %66 = add i32 %65, 7
  %67 = and i32 %66, -8
  %68 = inttoptr i32 %67 to i8*
  %69 = getelementptr inbounds i8, i8* %68, i32 8
  %70 = inttoptr i32 %67 to double*
  %71 = load double, double* %70, align 8
  %72 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %73 = bitcast %union.jvalue* %72 to double*
  store double %71, double* %73, align 8, !tbaa !16
  br label %91

74:                                               ; preds = %19
  %75 = ptrtoint i8* %21 to i32
  %76 = add i32 %75, 7
  %77 = and i32 %76, -8
  %78 = inttoptr i32 %77 to i8*
  %79 = getelementptr inbounds i8, i8* %78, i32 8
  %80 = inttoptr i32 %77 to double*
  %81 = load double, double* %80, align 8
  %82 = fptrunc double %81 to float
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %84 = bitcast %union.jvalue* %83 to float*
  store float %82, float* %84, align 8, !tbaa !16
  br label %91

85:                                               ; preds = %19
  %86 = getelementptr inbounds i8, i8* %21, i32 4
  %87 = bitcast i8* %21 to %struct._jobject**
  %88 = load %struct._jobject*, %struct._jobject** %87, align 4
  %89 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %90 = bitcast %union.jvalue* %89 to %struct._jobject**
  store %struct._jobject* %88, %struct._jobject** %90, align 8, !tbaa !16
  br label %91

91:                                               ; preds = %24, %31, %38, %45, %52, %58, %64, %74, %85, %19
  %92 = phi i8* [ %21, %19 ], [ %86, %85 ], [ %79, %74 ], [ %69, %64 ], [ %59, %58 ], [ %53, %52 ], [ %46, %45 ], [ %39, %38 ], [ %32, %31 ], [ %25, %24 ]
  %93 = add nuw nsw i32 %20, 1
  %94 = icmp eq i32 %93, %11
  br i1 %94, label %95, label %19, !llvm.loop !66

95:                                               ; preds = %91, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #3
  %96 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %97 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %96, i32 0, i32 81
  %98 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %97, align 4, !tbaa !67
  %99 = call i32 %98(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %14) #3
  ret i32 %99
}

; Function Attrs: alwaysinline nounwind
define dso_local i32 @JNI_CallStaticIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 130
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !68
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  %12 = call i32 %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret i32 %12
}

; Function Attrs: alwaysinline nounwind
define dso_local i32 @JNI_CallStaticIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !69

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 131
  %97 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !70
  %98 = call i32 %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret i32 %98
}

; Function Attrs: alwaysinline nounwind
define dso_local i64 @JNI_CallLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 53
  %8 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !71
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  %12 = call i64 %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret i64 %12
}

; Function Attrs: alwaysinline nounwind
define dso_local i64 @JNI_CallLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !72

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 54
  %97 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !73
  %98 = call i64 %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret i64 %98
}

; Function Attrs: alwaysinline nounwind
define dso_local i64 @JNI_CallNonvirtualLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = bitcast %struct.__va_list* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %6) #3
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i32 0, i32 83
  %9 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %8, align 4, !tbaa !74
  %10 = bitcast %struct.__va_list* %5 to i32*
  %11 = load i32, i32* %10, align 4
  %12 = insertvalue [1 x i32] poison, i32 %11, 0
  %13 = call i64 %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %12) #3
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %6) #3
  ret i64 %13
}

; Function Attrs: alwaysinline nounwind
define dso_local i64 @JNI_CallNonvirtualLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #3
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 4, !tbaa !9
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 4, !tbaa !15
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #3
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 8
  %14 = bitcast i8* %13 to %union.jvalue*
  %15 = icmp sgt i32 %11, 0
  br i1 %15, label %16, label %95

16:                                               ; preds = %5
  %17 = extractvalue [1 x i32] %4, 0
  %18 = inttoptr i32 %17 to i8*
  br label %19

19:                                               ; preds = %16, %91
  %20 = phi i32 [ %93, %91 ], [ 0, %16 ]
  %21 = phi i8* [ %92, %91 ], [ %18, %16 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i32 0, i32 %20
  %23 = load i8, i8* %22, align 1, !tbaa !16
  switch i8 %23, label %91 [
    i8 90, label %24
    i8 66, label %31
    i8 83, label %38
    i8 67, label %45
    i8 73, label %52
    i8 74, label %58
    i8 68, label %64
    i8 70, label %74
    i8 76, label %85
  ]

24:                                               ; preds = %19
  %25 = getelementptr inbounds i8, i8* %21, i32 4
  %26 = bitcast i8* %21 to i32*
  %27 = load i32, i32* %26, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %30 = bitcast %union.jvalue* %29 to i8*
  store i8 %28, i8* %30, align 8, !tbaa !16
  br label %91

31:                                               ; preds = %19
  %32 = getelementptr inbounds i8, i8* %21, i32 4
  %33 = bitcast i8* %21 to i32*
  %34 = load i32, i32* %33, align 4
  %35 = trunc i32 %34 to i8
  %36 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %37 = bitcast %union.jvalue* %36 to i8*
  store i8 %35, i8* %37, align 8, !tbaa !16
  br label %91

38:                                               ; preds = %19
  %39 = getelementptr inbounds i8, i8* %21, i32 4
  %40 = bitcast i8* %21 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i16
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %44 = bitcast %union.jvalue* %43 to i16*
  store i16 %42, i16* %44, align 8, !tbaa !16
  br label %91

45:                                               ; preds = %19
  %46 = getelementptr inbounds i8, i8* %21, i32 4
  %47 = bitcast i8* %21 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = and i32 %48, 65535
  %50 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %51 = bitcast %union.jvalue* %50 to i32*
  store i32 %49, i32* %51, align 8, !tbaa !16
  br label %91

52:                                               ; preds = %19
  %53 = getelementptr inbounds i8, i8* %21, i32 4
  %54 = bitcast i8* %21 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %57 = bitcast %union.jvalue* %56 to i32*
  store i32 %55, i32* %57, align 8, !tbaa !16
  br label %91

58:                                               ; preds = %19
  %59 = getelementptr inbounds i8, i8* %21, i32 4
  %60 = bitcast i8* %21 to i32*
  %61 = load i32, i32* %60, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20, i32 0
  store i64 %62, i64* %63, align 8, !tbaa !16
  br label %91

64:                                               ; preds = %19
  %65 = ptrtoint i8* %21 to i32
  %66 = add i32 %65, 7
  %67 = and i32 %66, -8
  %68 = inttoptr i32 %67 to i8*
  %69 = getelementptr inbounds i8, i8* %68, i32 8
  %70 = inttoptr i32 %67 to double*
  %71 = load double, double* %70, align 8
  %72 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %73 = bitcast %union.jvalue* %72 to double*
  store double %71, double* %73, align 8, !tbaa !16
  br label %91

74:                                               ; preds = %19
  %75 = ptrtoint i8* %21 to i32
  %76 = add i32 %75, 7
  %77 = and i32 %76, -8
  %78 = inttoptr i32 %77 to i8*
  %79 = getelementptr inbounds i8, i8* %78, i32 8
  %80 = inttoptr i32 %77 to double*
  %81 = load double, double* %80, align 8
  %82 = fptrunc double %81 to float
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %84 = bitcast %union.jvalue* %83 to float*
  store float %82, float* %84, align 8, !tbaa !16
  br label %91

85:                                               ; preds = %19
  %86 = getelementptr inbounds i8, i8* %21, i32 4
  %87 = bitcast i8* %21 to %struct._jobject**
  %88 = load %struct._jobject*, %struct._jobject** %87, align 4
  %89 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %90 = bitcast %union.jvalue* %89 to %struct._jobject**
  store %struct._jobject* %88, %struct._jobject** %90, align 8, !tbaa !16
  br label %91

91:                                               ; preds = %24, %31, %38, %45, %52, %58, %64, %74, %85, %19
  %92 = phi i8* [ %21, %19 ], [ %86, %85 ], [ %79, %74 ], [ %69, %64 ], [ %59, %58 ], [ %53, %52 ], [ %46, %45 ], [ %39, %38 ], [ %32, %31 ], [ %25, %24 ]
  %93 = add nuw nsw i32 %20, 1
  %94 = icmp eq i32 %93, %11
  br i1 %94, label %95, label %19, !llvm.loop !75

95:                                               ; preds = %91, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #3
  %96 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %97 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %96, i32 0, i32 84
  %98 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %97, align 4, !tbaa !76
  %99 = call i64 %98(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %14) #3
  ret i64 %99
}

; Function Attrs: alwaysinline nounwind
define dso_local i64 @JNI_CallStaticLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 133
  %8 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !77
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  %12 = call i64 %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret i64 %12
}

; Function Attrs: alwaysinline nounwind
define dso_local i64 @JNI_CallStaticLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !78

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 134
  %97 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !79
  %98 = call i64 %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret i64 %98
}

; Function Attrs: alwaysinline nounwind
define dso_local float @JNI_CallFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 56
  %8 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !80
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  %12 = call float %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret float %12
}

; Function Attrs: alwaysinline nounwind
define dso_local float @JNI_CallFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !81

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 57
  %97 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !82
  %98 = call float %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret float %98
}

; Function Attrs: alwaysinline nounwind
define dso_local float @JNI_CallNonvirtualFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = bitcast %struct.__va_list* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %6) #3
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i32 0, i32 86
  %9 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %8, align 4, !tbaa !83
  %10 = bitcast %struct.__va_list* %5 to i32*
  %11 = load i32, i32* %10, align 4
  %12 = insertvalue [1 x i32] poison, i32 %11, 0
  %13 = call float %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %12) #3
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %6) #3
  ret float %13
}

; Function Attrs: alwaysinline nounwind
define dso_local float @JNI_CallNonvirtualFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #3
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 4, !tbaa !9
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 4, !tbaa !15
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #3
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 8
  %14 = bitcast i8* %13 to %union.jvalue*
  %15 = icmp sgt i32 %11, 0
  br i1 %15, label %16, label %95

16:                                               ; preds = %5
  %17 = extractvalue [1 x i32] %4, 0
  %18 = inttoptr i32 %17 to i8*
  br label %19

19:                                               ; preds = %16, %91
  %20 = phi i32 [ %93, %91 ], [ 0, %16 ]
  %21 = phi i8* [ %92, %91 ], [ %18, %16 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i32 0, i32 %20
  %23 = load i8, i8* %22, align 1, !tbaa !16
  switch i8 %23, label %91 [
    i8 90, label %24
    i8 66, label %31
    i8 83, label %38
    i8 67, label %45
    i8 73, label %52
    i8 74, label %58
    i8 68, label %64
    i8 70, label %74
    i8 76, label %85
  ]

24:                                               ; preds = %19
  %25 = getelementptr inbounds i8, i8* %21, i32 4
  %26 = bitcast i8* %21 to i32*
  %27 = load i32, i32* %26, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %30 = bitcast %union.jvalue* %29 to i8*
  store i8 %28, i8* %30, align 8, !tbaa !16
  br label %91

31:                                               ; preds = %19
  %32 = getelementptr inbounds i8, i8* %21, i32 4
  %33 = bitcast i8* %21 to i32*
  %34 = load i32, i32* %33, align 4
  %35 = trunc i32 %34 to i8
  %36 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %37 = bitcast %union.jvalue* %36 to i8*
  store i8 %35, i8* %37, align 8, !tbaa !16
  br label %91

38:                                               ; preds = %19
  %39 = getelementptr inbounds i8, i8* %21, i32 4
  %40 = bitcast i8* %21 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i16
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %44 = bitcast %union.jvalue* %43 to i16*
  store i16 %42, i16* %44, align 8, !tbaa !16
  br label %91

45:                                               ; preds = %19
  %46 = getelementptr inbounds i8, i8* %21, i32 4
  %47 = bitcast i8* %21 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = and i32 %48, 65535
  %50 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %51 = bitcast %union.jvalue* %50 to i32*
  store i32 %49, i32* %51, align 8, !tbaa !16
  br label %91

52:                                               ; preds = %19
  %53 = getelementptr inbounds i8, i8* %21, i32 4
  %54 = bitcast i8* %21 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %57 = bitcast %union.jvalue* %56 to i32*
  store i32 %55, i32* %57, align 8, !tbaa !16
  br label %91

58:                                               ; preds = %19
  %59 = getelementptr inbounds i8, i8* %21, i32 4
  %60 = bitcast i8* %21 to i32*
  %61 = load i32, i32* %60, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20, i32 0
  store i64 %62, i64* %63, align 8, !tbaa !16
  br label %91

64:                                               ; preds = %19
  %65 = ptrtoint i8* %21 to i32
  %66 = add i32 %65, 7
  %67 = and i32 %66, -8
  %68 = inttoptr i32 %67 to i8*
  %69 = getelementptr inbounds i8, i8* %68, i32 8
  %70 = inttoptr i32 %67 to double*
  %71 = load double, double* %70, align 8
  %72 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %73 = bitcast %union.jvalue* %72 to double*
  store double %71, double* %73, align 8, !tbaa !16
  br label %91

74:                                               ; preds = %19
  %75 = ptrtoint i8* %21 to i32
  %76 = add i32 %75, 7
  %77 = and i32 %76, -8
  %78 = inttoptr i32 %77 to i8*
  %79 = getelementptr inbounds i8, i8* %78, i32 8
  %80 = inttoptr i32 %77 to double*
  %81 = load double, double* %80, align 8
  %82 = fptrunc double %81 to float
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %84 = bitcast %union.jvalue* %83 to float*
  store float %82, float* %84, align 8, !tbaa !16
  br label %91

85:                                               ; preds = %19
  %86 = getelementptr inbounds i8, i8* %21, i32 4
  %87 = bitcast i8* %21 to %struct._jobject**
  %88 = load %struct._jobject*, %struct._jobject** %87, align 4
  %89 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %90 = bitcast %union.jvalue* %89 to %struct._jobject**
  store %struct._jobject* %88, %struct._jobject** %90, align 8, !tbaa !16
  br label %91

91:                                               ; preds = %24, %31, %38, %45, %52, %58, %64, %74, %85, %19
  %92 = phi i8* [ %21, %19 ], [ %86, %85 ], [ %79, %74 ], [ %69, %64 ], [ %59, %58 ], [ %53, %52 ], [ %46, %45 ], [ %39, %38 ], [ %32, %31 ], [ %25, %24 ]
  %93 = add nuw nsw i32 %20, 1
  %94 = icmp eq i32 %93, %11
  br i1 %94, label %95, label %19, !llvm.loop !84

95:                                               ; preds = %91, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #3
  %96 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %97 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %96, i32 0, i32 87
  %98 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %97, align 4, !tbaa !85
  %99 = call float %98(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %14) #3
  ret float %99
}

; Function Attrs: alwaysinline nounwind
define dso_local float @JNI_CallStaticFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 136
  %8 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !86
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  %12 = call float %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret float %12
}

; Function Attrs: alwaysinline nounwind
define dso_local float @JNI_CallStaticFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !87

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 137
  %97 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !88
  %98 = call float %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret float %98
}

; Function Attrs: alwaysinline nounwind
define dso_local double @JNI_CallDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 59
  %8 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !89
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  %12 = call double %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret double %12
}

; Function Attrs: alwaysinline nounwind
define dso_local double @JNI_CallDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !90

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 60
  %97 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !91
  %98 = call double %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret double %98
}

; Function Attrs: alwaysinline nounwind
define dso_local double @JNI_CallNonvirtualDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = bitcast %struct.__va_list* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %6) #3
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i32 0, i32 89
  %9 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %8, align 4, !tbaa !92
  %10 = bitcast %struct.__va_list* %5 to i32*
  %11 = load i32, i32* %10, align 4
  %12 = insertvalue [1 x i32] poison, i32 %11, 0
  %13 = call double %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %12) #3
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %6) #3
  ret double %13
}

; Function Attrs: alwaysinline nounwind
define dso_local double @JNI_CallNonvirtualDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #3
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 4, !tbaa !9
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 4, !tbaa !15
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #3
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 8
  %14 = bitcast i8* %13 to %union.jvalue*
  %15 = icmp sgt i32 %11, 0
  br i1 %15, label %16, label %95

16:                                               ; preds = %5
  %17 = extractvalue [1 x i32] %4, 0
  %18 = inttoptr i32 %17 to i8*
  br label %19

19:                                               ; preds = %16, %91
  %20 = phi i32 [ %93, %91 ], [ 0, %16 ]
  %21 = phi i8* [ %92, %91 ], [ %18, %16 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i32 0, i32 %20
  %23 = load i8, i8* %22, align 1, !tbaa !16
  switch i8 %23, label %91 [
    i8 90, label %24
    i8 66, label %31
    i8 83, label %38
    i8 67, label %45
    i8 73, label %52
    i8 74, label %58
    i8 68, label %64
    i8 70, label %74
    i8 76, label %85
  ]

24:                                               ; preds = %19
  %25 = getelementptr inbounds i8, i8* %21, i32 4
  %26 = bitcast i8* %21 to i32*
  %27 = load i32, i32* %26, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %30 = bitcast %union.jvalue* %29 to i8*
  store i8 %28, i8* %30, align 8, !tbaa !16
  br label %91

31:                                               ; preds = %19
  %32 = getelementptr inbounds i8, i8* %21, i32 4
  %33 = bitcast i8* %21 to i32*
  %34 = load i32, i32* %33, align 4
  %35 = trunc i32 %34 to i8
  %36 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %37 = bitcast %union.jvalue* %36 to i8*
  store i8 %35, i8* %37, align 8, !tbaa !16
  br label %91

38:                                               ; preds = %19
  %39 = getelementptr inbounds i8, i8* %21, i32 4
  %40 = bitcast i8* %21 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i16
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %44 = bitcast %union.jvalue* %43 to i16*
  store i16 %42, i16* %44, align 8, !tbaa !16
  br label %91

45:                                               ; preds = %19
  %46 = getelementptr inbounds i8, i8* %21, i32 4
  %47 = bitcast i8* %21 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = and i32 %48, 65535
  %50 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %51 = bitcast %union.jvalue* %50 to i32*
  store i32 %49, i32* %51, align 8, !tbaa !16
  br label %91

52:                                               ; preds = %19
  %53 = getelementptr inbounds i8, i8* %21, i32 4
  %54 = bitcast i8* %21 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %57 = bitcast %union.jvalue* %56 to i32*
  store i32 %55, i32* %57, align 8, !tbaa !16
  br label %91

58:                                               ; preds = %19
  %59 = getelementptr inbounds i8, i8* %21, i32 4
  %60 = bitcast i8* %21 to i32*
  %61 = load i32, i32* %60, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20, i32 0
  store i64 %62, i64* %63, align 8, !tbaa !16
  br label %91

64:                                               ; preds = %19
  %65 = ptrtoint i8* %21 to i32
  %66 = add i32 %65, 7
  %67 = and i32 %66, -8
  %68 = inttoptr i32 %67 to i8*
  %69 = getelementptr inbounds i8, i8* %68, i32 8
  %70 = inttoptr i32 %67 to double*
  %71 = load double, double* %70, align 8
  %72 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %73 = bitcast %union.jvalue* %72 to double*
  store double %71, double* %73, align 8, !tbaa !16
  br label %91

74:                                               ; preds = %19
  %75 = ptrtoint i8* %21 to i32
  %76 = add i32 %75, 7
  %77 = and i32 %76, -8
  %78 = inttoptr i32 %77 to i8*
  %79 = getelementptr inbounds i8, i8* %78, i32 8
  %80 = inttoptr i32 %77 to double*
  %81 = load double, double* %80, align 8
  %82 = fptrunc double %81 to float
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %84 = bitcast %union.jvalue* %83 to float*
  store float %82, float* %84, align 8, !tbaa !16
  br label %91

85:                                               ; preds = %19
  %86 = getelementptr inbounds i8, i8* %21, i32 4
  %87 = bitcast i8* %21 to %struct._jobject**
  %88 = load %struct._jobject*, %struct._jobject** %87, align 4
  %89 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %90 = bitcast %union.jvalue* %89 to %struct._jobject**
  store %struct._jobject* %88, %struct._jobject** %90, align 8, !tbaa !16
  br label %91

91:                                               ; preds = %24, %31, %38, %45, %52, %58, %64, %74, %85, %19
  %92 = phi i8* [ %21, %19 ], [ %86, %85 ], [ %79, %74 ], [ %69, %64 ], [ %59, %58 ], [ %53, %52 ], [ %46, %45 ], [ %39, %38 ], [ %32, %31 ], [ %25, %24 ]
  %93 = add nuw nsw i32 %20, 1
  %94 = icmp eq i32 %93, %11
  br i1 %94, label %95, label %19, !llvm.loop !93

95:                                               ; preds = %91, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #3
  %96 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %97 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %96, i32 0, i32 90
  %98 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %97, align 4, !tbaa !94
  %99 = call double %98(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %14) #3
  ret double %99
}

; Function Attrs: alwaysinline nounwind
define dso_local double @JNI_CallStaticDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 139
  %8 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !95
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  %12 = call double %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret double %12
}

; Function Attrs: alwaysinline nounwind
define dso_local double @JNI_CallStaticDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !96

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 140
  %97 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !97
  %98 = call double %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret double %98
}

; Function Attrs: alwaysinline nounwind
define dso_local %struct._jobject* @JNI_NewObject(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 29
  %8 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !98
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  %12 = call %struct._jobject* %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret %struct._jobject* %12
}

; Function Attrs: alwaysinline nounwind
define dso_local %struct._jobject* @JNI_NewObjectV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !99

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 30
  %97 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !100
  %98 = call %struct._jobject* %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret %struct._jobject* %98
}

; Function Attrs: alwaysinline nounwind
define dso_local void @JNI_CallVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 62
  %8 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !101
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  call void %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret void
}

; Function Attrs: alwaysinline nounwind
define dso_local void @JNI_CallVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !102

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 63
  %97 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !103
  call void %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret void
}

; Function Attrs: alwaysinline nounwind
define dso_local void @JNI_CallNonvirtualVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca %struct.__va_list, align 4
  %6 = bitcast %struct.__va_list* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %6) #3
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i32 0, i32 92
  %9 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %8, align 4, !tbaa !104
  %10 = bitcast %struct.__va_list* %5 to i32*
  %11 = load i32, i32* %10, align 4
  %12 = insertvalue [1 x i32] poison, i32 %11, 0
  call void %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %12) #3
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %6) #3
  ret void
}

; Function Attrs: alwaysinline nounwind
define dso_local void @JNI_CallNonvirtualVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, [1 x i32] %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #3
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 4, !tbaa !9
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 4, !tbaa !15
  %11 = call i32 %10(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %3, i8* noundef nonnull %7) #3
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 8
  %14 = bitcast i8* %13 to %union.jvalue*
  %15 = icmp sgt i32 %11, 0
  br i1 %15, label %16, label %95

16:                                               ; preds = %5
  %17 = extractvalue [1 x i32] %4, 0
  %18 = inttoptr i32 %17 to i8*
  br label %19

19:                                               ; preds = %16, %91
  %20 = phi i32 [ %93, %91 ], [ 0, %16 ]
  %21 = phi i8* [ %92, %91 ], [ %18, %16 ]
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i32 0, i32 %20
  %23 = load i8, i8* %22, align 1, !tbaa !16
  switch i8 %23, label %91 [
    i8 90, label %24
    i8 66, label %31
    i8 83, label %38
    i8 67, label %45
    i8 73, label %52
    i8 74, label %58
    i8 68, label %64
    i8 70, label %74
    i8 76, label %85
  ]

24:                                               ; preds = %19
  %25 = getelementptr inbounds i8, i8* %21, i32 4
  %26 = bitcast i8* %21 to i32*
  %27 = load i32, i32* %26, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %30 = bitcast %union.jvalue* %29 to i8*
  store i8 %28, i8* %30, align 8, !tbaa !16
  br label %91

31:                                               ; preds = %19
  %32 = getelementptr inbounds i8, i8* %21, i32 4
  %33 = bitcast i8* %21 to i32*
  %34 = load i32, i32* %33, align 4
  %35 = trunc i32 %34 to i8
  %36 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %37 = bitcast %union.jvalue* %36 to i8*
  store i8 %35, i8* %37, align 8, !tbaa !16
  br label %91

38:                                               ; preds = %19
  %39 = getelementptr inbounds i8, i8* %21, i32 4
  %40 = bitcast i8* %21 to i32*
  %41 = load i32, i32* %40, align 4
  %42 = trunc i32 %41 to i16
  %43 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %44 = bitcast %union.jvalue* %43 to i16*
  store i16 %42, i16* %44, align 8, !tbaa !16
  br label %91

45:                                               ; preds = %19
  %46 = getelementptr inbounds i8, i8* %21, i32 4
  %47 = bitcast i8* %21 to i32*
  %48 = load i32, i32* %47, align 4
  %49 = and i32 %48, 65535
  %50 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %51 = bitcast %union.jvalue* %50 to i32*
  store i32 %49, i32* %51, align 8, !tbaa !16
  br label %91

52:                                               ; preds = %19
  %53 = getelementptr inbounds i8, i8* %21, i32 4
  %54 = bitcast i8* %21 to i32*
  %55 = load i32, i32* %54, align 4
  %56 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %57 = bitcast %union.jvalue* %56 to i32*
  store i32 %55, i32* %57, align 8, !tbaa !16
  br label %91

58:                                               ; preds = %19
  %59 = getelementptr inbounds i8, i8* %21, i32 4
  %60 = bitcast i8* %21 to i32*
  %61 = load i32, i32* %60, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20, i32 0
  store i64 %62, i64* %63, align 8, !tbaa !16
  br label %91

64:                                               ; preds = %19
  %65 = ptrtoint i8* %21 to i32
  %66 = add i32 %65, 7
  %67 = and i32 %66, -8
  %68 = inttoptr i32 %67 to i8*
  %69 = getelementptr inbounds i8, i8* %68, i32 8
  %70 = inttoptr i32 %67 to double*
  %71 = load double, double* %70, align 8
  %72 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %73 = bitcast %union.jvalue* %72 to double*
  store double %71, double* %73, align 8, !tbaa !16
  br label %91

74:                                               ; preds = %19
  %75 = ptrtoint i8* %21 to i32
  %76 = add i32 %75, 7
  %77 = and i32 %76, -8
  %78 = inttoptr i32 %77 to i8*
  %79 = getelementptr inbounds i8, i8* %78, i32 8
  %80 = inttoptr i32 %77 to double*
  %81 = load double, double* %80, align 8
  %82 = fptrunc double %81 to float
  %83 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %84 = bitcast %union.jvalue* %83 to float*
  store float %82, float* %84, align 8, !tbaa !16
  br label %91

85:                                               ; preds = %19
  %86 = getelementptr inbounds i8, i8* %21, i32 4
  %87 = bitcast i8* %21 to %struct._jobject**
  %88 = load %struct._jobject*, %struct._jobject** %87, align 4
  %89 = getelementptr inbounds %union.jvalue, %union.jvalue* %14, i32 %20
  %90 = bitcast %union.jvalue* %89 to %struct._jobject**
  store %struct._jobject* %88, %struct._jobject** %90, align 8, !tbaa !16
  br label %91

91:                                               ; preds = %24, %31, %38, %45, %52, %58, %64, %74, %85, %19
  %92 = phi i8* [ %21, %19 ], [ %86, %85 ], [ %79, %74 ], [ %69, %64 ], [ %59, %58 ], [ %53, %52 ], [ %46, %45 ], [ %39, %38 ], [ %32, %31 ], [ %25, %24 ]
  %93 = add nuw nsw i32 %20, 1
  %94 = icmp eq i32 %93, %11
  br i1 %94, label %95, label %19, !llvm.loop !105

95:                                               ; preds = %91, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #3
  %96 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %97 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %96, i32 0, i32 93
  %98 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %97, align 4, !tbaa !106
  call void %98(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %14) #3
  ret void
}

; Function Attrs: alwaysinline nounwind
define dso_local void @JNI_CallStaticVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %struct.__va_list, align 4
  %5 = bitcast %struct.__va_list* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 4, i8* nonnull %5) #3
  call void @llvm.va_start(i8* nonnull %5)
  %6 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %7 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %6, i32 0, i32 142
  %8 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %7, align 4, !tbaa !107
  %9 = bitcast %struct.__va_list* %4 to i32*
  %10 = load i32, i32* %9, align 4
  %11 = insertvalue [1 x i32] poison, i32 %10, 0
  call void %8(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %11) #3
  call void @llvm.va_end(i8* nonnull %5)
  call void @llvm.lifetime.end.p0i8(i64 4, i8* nonnull %5) #3
  ret void
}

; Function Attrs: alwaysinline nounwind
define dso_local void @JNI_CallStaticVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, [1 x i32] %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #3
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 4, !tbaa !9
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 4, !tbaa !15
  %10 = call i32 %9(%struct.JNINativeInterface_** noundef %0, %struct._jmethodID* noundef %2, i8* noundef nonnull %6) #3
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = bitcast i8* %12 to %union.jvalue*
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %94

15:                                               ; preds = %4
  %16 = extractvalue [1 x i32] %3, 0
  %17 = inttoptr i32 %16 to i8*
  br label %18

18:                                               ; preds = %15, %90
  %19 = phi i32 [ %92, %90 ], [ 0, %15 ]
  %20 = phi i8* [ %91, %90 ], [ %17, %15 ]
  %21 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i32 0, i32 %19
  %22 = load i8, i8* %21, align 1, !tbaa !16
  switch i8 %22, label %90 [
    i8 90, label %23
    i8 66, label %30
    i8 83, label %37
    i8 67, label %44
    i8 73, label %51
    i8 74, label %57
    i8 68, label %63
    i8 70, label %73
    i8 76, label %84
  ]

23:                                               ; preds = %18
  %24 = getelementptr inbounds i8, i8* %20, i32 4
  %25 = bitcast i8* %20 to i32*
  %26 = load i32, i32* %25, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %29 = bitcast %union.jvalue* %28 to i8*
  store i8 %27, i8* %29, align 8, !tbaa !16
  br label %90

30:                                               ; preds = %18
  %31 = getelementptr inbounds i8, i8* %20, i32 4
  %32 = bitcast i8* %20 to i32*
  %33 = load i32, i32* %32, align 4
  %34 = trunc i32 %33 to i8
  %35 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %36 = bitcast %union.jvalue* %35 to i8*
  store i8 %34, i8* %36, align 8, !tbaa !16
  br label %90

37:                                               ; preds = %18
  %38 = getelementptr inbounds i8, i8* %20, i32 4
  %39 = bitcast i8* %20 to i32*
  %40 = load i32, i32* %39, align 4
  %41 = trunc i32 %40 to i16
  %42 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %43 = bitcast %union.jvalue* %42 to i16*
  store i16 %41, i16* %43, align 8, !tbaa !16
  br label %90

44:                                               ; preds = %18
  %45 = getelementptr inbounds i8, i8* %20, i32 4
  %46 = bitcast i8* %20 to i32*
  %47 = load i32, i32* %46, align 4
  %48 = and i32 %47, 65535
  %49 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %50 = bitcast %union.jvalue* %49 to i32*
  store i32 %48, i32* %50, align 8, !tbaa !16
  br label %90

51:                                               ; preds = %18
  %52 = getelementptr inbounds i8, i8* %20, i32 4
  %53 = bitcast i8* %20 to i32*
  %54 = load i32, i32* %53, align 4
  %55 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %56 = bitcast %union.jvalue* %55 to i32*
  store i32 %54, i32* %56, align 8, !tbaa !16
  br label %90

57:                                               ; preds = %18
  %58 = getelementptr inbounds i8, i8* %20, i32 4
  %59 = bitcast i8* %20 to i32*
  %60 = load i32, i32* %59, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19, i32 0
  store i64 %61, i64* %62, align 8, !tbaa !16
  br label %90

63:                                               ; preds = %18
  %64 = ptrtoint i8* %20 to i32
  %65 = add i32 %64, 7
  %66 = and i32 %65, -8
  %67 = inttoptr i32 %66 to i8*
  %68 = getelementptr inbounds i8, i8* %67, i32 8
  %69 = inttoptr i32 %66 to double*
  %70 = load double, double* %69, align 8
  %71 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %72 = bitcast %union.jvalue* %71 to double*
  store double %70, double* %72, align 8, !tbaa !16
  br label %90

73:                                               ; preds = %18
  %74 = ptrtoint i8* %20 to i32
  %75 = add i32 %74, 7
  %76 = and i32 %75, -8
  %77 = inttoptr i32 %76 to i8*
  %78 = getelementptr inbounds i8, i8* %77, i32 8
  %79 = inttoptr i32 %76 to double*
  %80 = load double, double* %79, align 8
  %81 = fptrunc double %80 to float
  %82 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %83 = bitcast %union.jvalue* %82 to float*
  store float %81, float* %83, align 8, !tbaa !16
  br label %90

84:                                               ; preds = %18
  %85 = getelementptr inbounds i8, i8* %20, i32 4
  %86 = bitcast i8* %20 to %struct._jobject**
  %87 = load %struct._jobject*, %struct._jobject** %86, align 4
  %88 = getelementptr inbounds %union.jvalue, %union.jvalue* %13, i32 %19
  %89 = bitcast %union.jvalue* %88 to %struct._jobject**
  store %struct._jobject* %87, %struct._jobject** %89, align 8, !tbaa !16
  br label %90

90:                                               ; preds = %23, %30, %37, %44, %51, %57, %63, %73, %84, %18
  %91 = phi i8* [ %20, %18 ], [ %85, %84 ], [ %78, %73 ], [ %68, %63 ], [ %58, %57 ], [ %52, %51 ], [ %45, %44 ], [ %38, %37 ], [ %31, %30 ], [ %24, %23 ]
  %92 = add nuw nsw i32 %19, 1
  %93 = icmp eq i32 %92, %10
  br i1 %93, label %94, label %18, !llvm.loop !108

94:                                               ; preds = %90, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #3
  %95 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 4, !tbaa !9
  %96 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %95, i32 0, i32 143
  %97 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %96, align 4, !tbaa !109
  call void %97(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %13) #3
  ret void
}

attributes #0 = { alwaysinline nounwind "frame-pointer"="none" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+dsp,+fp64,+vfp2,+vfp2sp,+vfp3d16,+vfp3d16sp,-aes,-d32,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-neon,-sha2,-thumb-mode,-vfp3,-vfp3sp,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }
attributes #1 = { argmemonly mustprogress nofree nosync nounwind willreturn }
attributes #2 = { mustprogress nofree nosync nounwind willreturn }
attributes #3 = { nounwind }

!llvm.module.flags = !{!0, !1, !2, !3, !4, !5, !6, !7}
!llvm.ident = !{!8}

!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 1, !"min_enum_size", i32 4}
!2 = !{i32 1, !"branch-target-enforcement", i32 0}
!3 = !{i32 1, !"sign-return-address", i32 0}
!4 = !{i32 1, !"sign-return-address-all", i32 0}
!5 = !{i32 1, !"sign-return-address-with-bkey", i32 0}
!6 = !{i32 7, !"PIC Level", i32 2}
!7 = !{i32 7, !"PIE Level", i32 2}
!8 = !{!"Ubuntu clang version 14.0.0-1ubuntu1"}
!9 = !{!10, !10, i64 0}
!10 = !{!"any pointer", !11, i64 0}
!11 = !{!"omnipotent char", !12, i64 0}
!12 = !{!"Simple C/C++ TBAA"}
!13 = !{!14, !10, i64 140}
!14 = !{!"JNINativeInterface_", !10, i64 0, !10, i64 4, !10, i64 8, !10, i64 12, !10, i64 16, !10, i64 20, !10, i64 24, !10, i64 28, !10, i64 32, !10, i64 36, !10, i64 40, !10, i64 44, !10, i64 48, !10, i64 52, !10, i64 56, !10, i64 60, !10, i64 64, !10, i64 68, !10, i64 72, !10, i64 76, !10, i64 80, !10, i64 84, !10, i64 88, !10, i64 92, !10, i64 96, !10, i64 100, !10, i64 104, !10, i64 108, !10, i64 112, !10, i64 116, !10, i64 120, !10, i64 124, !10, i64 128, !10, i64 132, !10, i64 136, !10, i64 140, !10, i64 144, !10, i64 148, !10, i64 152, !10, i64 156, !10, i64 160, !10, i64 164, !10, i64 168, !10, i64 172, !10, i64 176, !10, i64 180, !10, i64 184, !10, i64 188, !10, i64 192, !10, i64 196, !10, i64 200, !10, i64 204, !10, i64 208, !10, i64 212, !10, i64 216, !10, i64 220, !10, i64 224, !10, i64 228, !10, i64 232, !10, i64 236, !10, i64 240, !10, i64 244, !10, i64 248, !10, i64 252, !10, i64 256, !10, i64 260, !10, i64 264, !10, i64 268, !10, i64 272, !10, i64 276, !10, i64 280, !10, i64 284, !10, i64 288, !10, i64 292, !10, i64 296, !10, i64 300, !10, i64 304, !10, i64 308, !10, i64 312, !10, i64 316, !10, i64 320, !10, i64 324, !10, i64 328, !10, i64 332, !10, i64 336, !10, i64 340, !10, i64 344, !10, i64 348, !10, i64 352, !10, i64 356, !10, i64 360, !10, i64 364, !10, i64 368, !10, i64 372, !10, i64 376, !10, i64 380, !10, i64 384, !10, i64 388, !10, i64 392, !10, i64 396, !10, i64 400, !10, i64 404, !10, i64 408, !10, i64 412, !10, i64 416, !10, i64 420, !10, i64 424, !10, i64 428, !10, i64 432, !10, i64 436, !10, i64 440, !10, i64 444, !10, i64 448, !10, i64 452, !10, i64 456, !10, i64 460, !10, i64 464, !10, i64 468, !10, i64 472, !10, i64 476, !10, i64 480, !10, i64 484, !10, i64 488, !10, i64 492, !10, i64 496, !10, i64 500, !10, i64 504, !10, i64 508, !10, i64 512, !10, i64 516, !10, i64 520, !10, i64 524, !10, i64 528, !10, i64 532, !10, i64 536, !10, i64 540, !10, i64 544, !10, i64 548, !10, i64 552, !10, i64 556, !10, i64 560, !10, i64 564, !10, i64 568, !10, i64 572, !10, i64 576, !10, i64 580, !10, i64 584, !10, i64 588, !10, i64 592, !10, i64 596, !10, i64 600, !10, i64 604, !10, i64 608, !10, i64 612, !10, i64 616, !10, i64 620, !10, i64 624, !10, i64 628, !10, i64 632, !10, i64 636, !10, i64 640, !10, i64 644, !10, i64 648, !10, i64 652, !10, i64 656, !10, i64 660, !10, i64 664, !10, i64 668, !10, i64 672, !10, i64 676, !10, i64 680, !10, i64 684, !10, i64 688, !10, i64 692, !10, i64 696, !10, i64 700, !10, i64 704, !10, i64 708, !10, i64 712, !10, i64 716, !10, i64 720, !10, i64 724, !10, i64 728, !10, i64 732, !10, i64 736, !10, i64 740, !10, i64 744, !10, i64 748, !10, i64 752, !10, i64 756, !10, i64 760, !10, i64 764, !10, i64 768, !10, i64 772, !10, i64 776, !10, i64 780, !10, i64 784, !10, i64 788, !10, i64 792, !10, i64 796, !10, i64 800, !10, i64 804, !10, i64 808, !10, i64 812, !10, i64 816, !10, i64 820, !10, i64 824, !10, i64 828, !10, i64 832, !10, i64 836, !10, i64 840, !10, i64 844, !10, i64 848, !10, i64 852, !10, i64 856, !10, i64 860, !10, i64 864, !10, i64 868, !10, i64 872, !10, i64 876, !10, i64 880, !10, i64 884, !10, i64 888, !10, i64 892, !10, i64 896, !10, i64 900, !10, i64 904, !10, i64 908, !10, i64 912, !10, i64 916, !10, i64 920, !10, i64 924, !10, i64 928}
!15 = !{!14, !10, i64 0}
!16 = !{!11, !11, i64 0}
!17 = distinct !{!17, !18}
!18 = !{!"llvm.loop.mustprogress"}
!19 = !{!14, !10, i64 144}
!20 = !{!14, !10, i64 260}
!21 = distinct !{!21, !18}
!22 = !{!14, !10, i64 264}
!23 = !{!14, !10, i64 460}
!24 = distinct !{!24, !18}
!25 = !{!14, !10, i64 464}
!26 = !{!14, !10, i64 152}
!27 = distinct !{!27, !18}
!28 = !{!14, !10, i64 156}
!29 = !{!14, !10, i64 272}
!30 = distinct !{!30, !18}
!31 = !{!14, !10, i64 276}
!32 = !{!14, !10, i64 472}
!33 = distinct !{!33, !18}
!34 = !{!14, !10, i64 476}
!35 = !{!14, !10, i64 164}
!36 = distinct !{!36, !18}
!37 = !{!14, !10, i64 168}
!38 = !{!14, !10, i64 284}
!39 = distinct !{!39, !18}
!40 = !{!14, !10, i64 288}
!41 = !{!14, !10, i64 484}
!42 = distinct !{!42, !18}
!43 = !{!14, !10, i64 488}
!44 = !{!14, !10, i64 176}
!45 = distinct !{!45, !18}
!46 = !{!14, !10, i64 180}
!47 = !{!14, !10, i64 296}
!48 = distinct !{!48, !18}
!49 = !{!14, !10, i64 300}
!50 = !{!14, !10, i64 496}
!51 = distinct !{!51, !18}
!52 = !{!14, !10, i64 500}
!53 = !{!14, !10, i64 188}
!54 = distinct !{!54, !18}
!55 = !{!14, !10, i64 192}
!56 = !{!14, !10, i64 308}
!57 = distinct !{!57, !18}
!58 = !{!14, !10, i64 312}
!59 = !{!14, !10, i64 508}
!60 = distinct !{!60, !18}
!61 = !{!14, !10, i64 512}
!62 = !{!14, !10, i64 200}
!63 = distinct !{!63, !18}
!64 = !{!14, !10, i64 204}
!65 = !{!14, !10, i64 320}
!66 = distinct !{!66, !18}
!67 = !{!14, !10, i64 324}
!68 = !{!14, !10, i64 520}
!69 = distinct !{!69, !18}
!70 = !{!14, !10, i64 524}
!71 = !{!14, !10, i64 212}
!72 = distinct !{!72, !18}
!73 = !{!14, !10, i64 216}
!74 = !{!14, !10, i64 332}
!75 = distinct !{!75, !18}
!76 = !{!14, !10, i64 336}
!77 = !{!14, !10, i64 532}
!78 = distinct !{!78, !18}
!79 = !{!14, !10, i64 536}
!80 = !{!14, !10, i64 224}
!81 = distinct !{!81, !18}
!82 = !{!14, !10, i64 228}
!83 = !{!14, !10, i64 344}
!84 = distinct !{!84, !18}
!85 = !{!14, !10, i64 348}
!86 = !{!14, !10, i64 544}
!87 = distinct !{!87, !18}
!88 = !{!14, !10, i64 548}
!89 = !{!14, !10, i64 236}
!90 = distinct !{!90, !18}
!91 = !{!14, !10, i64 240}
!92 = !{!14, !10, i64 356}
!93 = distinct !{!93, !18}
!94 = !{!14, !10, i64 360}
!95 = !{!14, !10, i64 556}
!96 = distinct !{!96, !18}
!97 = !{!14, !10, i64 560}
!98 = !{!14, !10, i64 116}
!99 = distinct !{!99, !18}
!100 = !{!14, !10, i64 120}
!101 = !{!14, !10, i64 248}
!102 = distinct !{!102, !18}
!103 = !{!14, !10, i64 252}
!104 = !{!14, !10, i64 368}
!105 = distinct !{!105, !18}
!106 = !{!14, !10, i64 372}
!107 = !{!14, !10, i64 568}
!108 = distinct !{!108, !18}
!109 = !{!14, !10, i64 572}
