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
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 35
  %9 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !14
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  %11 = call %struct._jobject* %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret %struct._jobject* %11
}

; Function Attrs: argmemonly mustprogress nofree nosync nounwind willreturn
declare void @llvm.lifetime.start.p0i8(i64 immarg, i8* nocapture) #1

; Function Attrs: mustprogress nofree nosync nounwind willreturn
declare void @llvm.va_start(i8*) #2

; Function Attrs: argmemonly mustprogress nofree nounwind willreturn
declare void @llvm.memcpy.p0i8.p0i8.i64(i8* noalias nocapture writeonly, i8* noalias nocapture readonly, i64, i1 immarg) #3

; Function Attrs: argmemonly mustprogress nofree nosync nounwind willreturn
declare void @llvm.lifetime.end.p0i8(i64 immarg, i8* nocapture) #1

; Function Attrs: mustprogress nofree nosync nounwind willreturn
declare void @llvm.va_end(i8*) #2

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_CallObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !21

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 36
  %206 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !23
  %207 = call %struct._jobject* %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret %struct._jobject* %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_CallNonvirtualObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = alloca %"struct.std::__va_list", align 8
  %7 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %9 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %8, i64 0, i32 65
  %10 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %9, align 8, !tbaa !24
  %11 = bitcast %"struct.std::__va_list"* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %11) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %11, i8* noundef nonnull align 8 dereferenceable(32) %7, i64 32, i1 false), !tbaa.struct !16
  %12 = call %struct._jobject* %10(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef nonnull %6) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %11) #4
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %7) #4
  ret %struct._jobject* %12
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_CallNonvirtualObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !10
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !19
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
  %25 = load i8, i8* %24, align 1, !tbaa !20
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
  store i8 %43, i8* %45, align 8, !tbaa !20
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
  store i8 %63, i8* %65, align 8, !tbaa !20
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
  store i16 %83, i16* %85, align 8, !tbaa !20
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
  store i32 %103, i32* %105, align 8, !tbaa !20
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
  store i32 %122, i32* %124, align 8, !tbaa !20
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
  store i64 %141, i64* %142, align 8, !tbaa !20
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
  store double %159, double* %161, align 8, !tbaa !20
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
  store float %179, float* %181, align 8, !tbaa !20
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
  store %struct._jobject* %198, %struct._jobject** %200, align 8, !tbaa !20
  br label %201

201:                                              ; preds = %39, %59, %79, %99, %119, %138, %156, %175, %195, %22
  %202 = add nuw nsw i64 %23, 1
  %203 = icmp eq i64 %202, %21
  br i1 %203, label %204, label %22, !llvm.loop !25

204:                                              ; preds = %201, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %205 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %206 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %205, i64 0, i32 66
  %207 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %206, align 8, !tbaa !26
  %208 = call %struct._jobject* %207(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret %struct._jobject* %208
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_CallStaticObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 115
  %9 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !27
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  %11 = call %struct._jobject* %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret %struct._jobject* %11
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_CallStaticObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !28

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 116
  %206 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !29
  %207 = call %struct._jobject* %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret %struct._jobject* %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 38
  %9 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !30
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  %11 = call i8 %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i8 %11
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !31

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 39
  %206 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !32
  %207 = call i8 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i8 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallNonvirtualBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = alloca %"struct.std::__va_list", align 8
  %7 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %9 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %8, i64 0, i32 68
  %10 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %9, align 8, !tbaa !33
  %11 = bitcast %"struct.std::__va_list"* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %11) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %11, i8* noundef nonnull align 8 dereferenceable(32) %7, i64 32, i1 false), !tbaa.struct !16
  %12 = call i8 %10(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef nonnull %6) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %11) #4
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %7) #4
  ret i8 %12
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallNonvirtualBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !10
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !19
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
  %25 = load i8, i8* %24, align 1, !tbaa !20
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
  store i8 %43, i8* %45, align 8, !tbaa !20
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
  store i8 %63, i8* %65, align 8, !tbaa !20
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
  store i16 %83, i16* %85, align 8, !tbaa !20
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
  store i32 %103, i32* %105, align 8, !tbaa !20
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
  store i32 %122, i32* %124, align 8, !tbaa !20
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
  store i64 %141, i64* %142, align 8, !tbaa !20
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
  store double %159, double* %161, align 8, !tbaa !20
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
  store float %179, float* %181, align 8, !tbaa !20
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
  store %struct._jobject* %198, %struct._jobject** %200, align 8, !tbaa !20
  br label %201

201:                                              ; preds = %39, %59, %79, %99, %119, %138, %156, %175, %195, %22
  %202 = add nuw nsw i64 %23, 1
  %203 = icmp eq i64 %202, %21
  br i1 %203, label %204, label %22, !llvm.loop !34

204:                                              ; preds = %201, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %205 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %206 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %205, i64 0, i32 69
  %207 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %206, align 8, !tbaa !35
  %208 = call i8 %207(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret i8 %208
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallStaticBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 118
  %9 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !36
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  %11 = call i8 %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i8 %11
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallStaticBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !37

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 119
  %206 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !38
  %207 = call i8 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i8 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 41
  %9 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !39
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  %11 = call i8 %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i8 %11
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !40

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 42
  %206 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !41
  %207 = call i8 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i8 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallNonvirtualByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = alloca %"struct.std::__va_list", align 8
  %7 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %9 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %8, i64 0, i32 71
  %10 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %9, align 8, !tbaa !42
  %11 = bitcast %"struct.std::__va_list"* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %11) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %11, i8* noundef nonnull align 8 dereferenceable(32) %7, i64 32, i1 false), !tbaa.struct !16
  %12 = call i8 %10(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef nonnull %6) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %11) #4
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %7) #4
  ret i8 %12
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallNonvirtualByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !10
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !19
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
  %25 = load i8, i8* %24, align 1, !tbaa !20
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
  store i8 %43, i8* %45, align 8, !tbaa !20
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
  store i8 %63, i8* %65, align 8, !tbaa !20
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
  store i16 %83, i16* %85, align 8, !tbaa !20
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
  store i32 %103, i32* %105, align 8, !tbaa !20
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
  store i32 %122, i32* %124, align 8, !tbaa !20
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
  store i64 %141, i64* %142, align 8, !tbaa !20
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
  store double %159, double* %161, align 8, !tbaa !20
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
  store float %179, float* %181, align 8, !tbaa !20
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
  store %struct._jobject* %198, %struct._jobject** %200, align 8, !tbaa !20
  br label %201

201:                                              ; preds = %39, %59, %79, %99, %119, %138, %156, %175, %195, %22
  %202 = add nuw nsw i64 %23, 1
  %203 = icmp eq i64 %202, %21
  br i1 %203, label %204, label %22, !llvm.loop !43

204:                                              ; preds = %201, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %205 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %206 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %205, i64 0, i32 72
  %207 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %206, align 8, !tbaa !44
  %208 = call i8 %207(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret i8 %208
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallStaticByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 121
  %9 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !45
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  %11 = call i8 %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i8 %11
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i8 @JNI_CallStaticByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !46

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 122
  %206 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !47
  %207 = call i8 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i8 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 44
  %9 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !48
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  %11 = call i16 %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i16 %11
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !49

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 45
  %206 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !50
  %207 = call i16 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i16 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallNonvirtualCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = alloca %"struct.std::__va_list", align 8
  %7 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %9 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %8, i64 0, i32 74
  %10 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %9, align 8, !tbaa !51
  %11 = bitcast %"struct.std::__va_list"* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %11) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %11, i8* noundef nonnull align 8 dereferenceable(32) %7, i64 32, i1 false), !tbaa.struct !16
  %12 = call i16 %10(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef nonnull %6) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %11) #4
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %7) #4
  ret i16 %12
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallNonvirtualCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !10
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !19
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
  %25 = load i8, i8* %24, align 1, !tbaa !20
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
  store i8 %43, i8* %45, align 8, !tbaa !20
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
  store i8 %63, i8* %65, align 8, !tbaa !20
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
  store i16 %83, i16* %85, align 8, !tbaa !20
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
  store i32 %103, i32* %105, align 8, !tbaa !20
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
  store i32 %122, i32* %124, align 8, !tbaa !20
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
  store i64 %141, i64* %142, align 8, !tbaa !20
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
  store double %159, double* %161, align 8, !tbaa !20
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
  store float %179, float* %181, align 8, !tbaa !20
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
  store %struct._jobject* %198, %struct._jobject** %200, align 8, !tbaa !20
  br label %201

201:                                              ; preds = %39, %59, %79, %99, %119, %138, %156, %175, %195, %22
  %202 = add nuw nsw i64 %23, 1
  %203 = icmp eq i64 %202, %21
  br i1 %203, label %204, label %22, !llvm.loop !52

204:                                              ; preds = %201, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %205 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %206 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %205, i64 0, i32 75
  %207 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %206, align 8, !tbaa !53
  %208 = call i16 %207(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret i16 %208
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallStaticCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 124
  %9 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !54
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  %11 = call i16 %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i16 %11
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallStaticCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !55

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 125
  %206 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !56
  %207 = call i16 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i16 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 47
  %9 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !57
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  %11 = call i16 %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i16 %11
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !58

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 48
  %206 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !59
  %207 = call i16 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i16 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallNonvirtualShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = alloca %"struct.std::__va_list", align 8
  %7 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %9 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %8, i64 0, i32 77
  %10 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %9, align 8, !tbaa !60
  %11 = bitcast %"struct.std::__va_list"* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %11) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %11, i8* noundef nonnull align 8 dereferenceable(32) %7, i64 32, i1 false), !tbaa.struct !16
  %12 = call i16 %10(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef nonnull %6) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %11) #4
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %7) #4
  ret i16 %12
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallNonvirtualShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !10
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !19
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
  %25 = load i8, i8* %24, align 1, !tbaa !20
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
  store i8 %43, i8* %45, align 8, !tbaa !20
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
  store i8 %63, i8* %65, align 8, !tbaa !20
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
  store i16 %83, i16* %85, align 8, !tbaa !20
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
  store i32 %103, i32* %105, align 8, !tbaa !20
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
  store i32 %122, i32* %124, align 8, !tbaa !20
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
  store i64 %141, i64* %142, align 8, !tbaa !20
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
  store double %159, double* %161, align 8, !tbaa !20
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
  store float %179, float* %181, align 8, !tbaa !20
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
  store %struct._jobject* %198, %struct._jobject** %200, align 8, !tbaa !20
  br label %201

201:                                              ; preds = %39, %59, %79, %99, %119, %138, %156, %175, %195, %22
  %202 = add nuw nsw i64 %23, 1
  %203 = icmp eq i64 %202, %21
  br i1 %203, label %204, label %22, !llvm.loop !61

204:                                              ; preds = %201, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %205 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %206 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %205, i64 0, i32 78
  %207 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %206, align 8, !tbaa !62
  %208 = call i16 %207(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret i16 %208
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallStaticShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 127
  %9 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !63
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  %11 = call i16 %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i16 %11
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i16 @JNI_CallStaticShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !64

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 128
  %206 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !65
  %207 = call i16 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i16 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i32 @JNI_CallIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 50
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !66
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  %11 = call i32 %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i32 %11
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i32 @JNI_CallIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !67

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 51
  %206 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !68
  %207 = call i32 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i32 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i32 @JNI_CallNonvirtualIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = alloca %"struct.std::__va_list", align 8
  %7 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %9 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %8, i64 0, i32 80
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %9, align 8, !tbaa !69
  %11 = bitcast %"struct.std::__va_list"* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %11) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %11, i8* noundef nonnull align 8 dereferenceable(32) %7, i64 32, i1 false), !tbaa.struct !16
  %12 = call i32 %10(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef nonnull %6) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %11) #4
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %7) #4
  ret i32 %12
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i32 @JNI_CallNonvirtualIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !10
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !19
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
  %25 = load i8, i8* %24, align 1, !tbaa !20
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
  store i8 %43, i8* %45, align 8, !tbaa !20
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
  store i8 %63, i8* %65, align 8, !tbaa !20
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
  store i16 %83, i16* %85, align 8, !tbaa !20
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
  store i32 %103, i32* %105, align 8, !tbaa !20
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
  store i32 %122, i32* %124, align 8, !tbaa !20
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
  store i64 %141, i64* %142, align 8, !tbaa !20
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
  store double %159, double* %161, align 8, !tbaa !20
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
  store float %179, float* %181, align 8, !tbaa !20
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
  store %struct._jobject* %198, %struct._jobject** %200, align 8, !tbaa !20
  br label %201

201:                                              ; preds = %39, %59, %79, %99, %119, %138, %156, %175, %195, %22
  %202 = add nuw nsw i64 %23, 1
  %203 = icmp eq i64 %202, %21
  br i1 %203, label %204, label %22, !llvm.loop !70

204:                                              ; preds = %201, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %205 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %206 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %205, i64 0, i32 81
  %207 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %206, align 8, !tbaa !71
  %208 = call i32 %207(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret i32 %208
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i32 @JNI_CallStaticIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 130
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !72
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  %11 = call i32 %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i32 %11
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i32 @JNI_CallStaticIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !73

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 131
  %206 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !74
  %207 = call i32 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i32 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i64 @JNI_CallLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 53
  %9 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !75
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  %11 = call i64 %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i64 %11
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i64 @JNI_CallLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !76

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 54
  %206 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !77
  %207 = call i64 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i64 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i64 @JNI_CallNonvirtualLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = alloca %"struct.std::__va_list", align 8
  %7 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %9 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %8, i64 0, i32 83
  %10 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %9, align 8, !tbaa !78
  %11 = bitcast %"struct.std::__va_list"* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %11) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %11, i8* noundef nonnull align 8 dereferenceable(32) %7, i64 32, i1 false), !tbaa.struct !16
  %12 = call i64 %10(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef nonnull %6) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %11) #4
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %7) #4
  ret i64 %12
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i64 @JNI_CallNonvirtualLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !10
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !19
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
  %25 = load i8, i8* %24, align 1, !tbaa !20
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
  store i8 %43, i8* %45, align 8, !tbaa !20
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
  store i8 %63, i8* %65, align 8, !tbaa !20
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
  store i16 %83, i16* %85, align 8, !tbaa !20
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
  store i32 %103, i32* %105, align 8, !tbaa !20
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
  store i32 %122, i32* %124, align 8, !tbaa !20
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
  store i64 %141, i64* %142, align 8, !tbaa !20
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
  store double %159, double* %161, align 8, !tbaa !20
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
  store float %179, float* %181, align 8, !tbaa !20
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
  store %struct._jobject* %198, %struct._jobject** %200, align 8, !tbaa !20
  br label %201

201:                                              ; preds = %39, %59, %79, %99, %119, %138, %156, %175, %195, %22
  %202 = add nuw nsw i64 %23, 1
  %203 = icmp eq i64 %202, %21
  br i1 %203, label %204, label %22, !llvm.loop !79

204:                                              ; preds = %201, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %205 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %206 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %205, i64 0, i32 84
  %207 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %206, align 8, !tbaa !80
  %208 = call i64 %207(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret i64 %208
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i64 @JNI_CallStaticLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 133
  %9 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !81
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  %11 = call i64 %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret i64 %11
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i64 @JNI_CallStaticLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !82

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 134
  %206 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !83
  %207 = call i64 %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret i64 %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local float @JNI_CallFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 56
  %9 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !84
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  %11 = call float %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret float %11
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local float @JNI_CallFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !85

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 57
  %206 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !86
  %207 = call float %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret float %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local float @JNI_CallNonvirtualFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = alloca %"struct.std::__va_list", align 8
  %7 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %9 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %8, i64 0, i32 86
  %10 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %9, align 8, !tbaa !87
  %11 = bitcast %"struct.std::__va_list"* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %11) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %11, i8* noundef nonnull align 8 dereferenceable(32) %7, i64 32, i1 false), !tbaa.struct !16
  %12 = call float %10(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef nonnull %6) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %11) #4
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %7) #4
  ret float %12
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local float @JNI_CallNonvirtualFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !10
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !19
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
  %25 = load i8, i8* %24, align 1, !tbaa !20
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
  store i8 %43, i8* %45, align 8, !tbaa !20
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
  store i8 %63, i8* %65, align 8, !tbaa !20
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
  store i16 %83, i16* %85, align 8, !tbaa !20
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
  store i32 %103, i32* %105, align 8, !tbaa !20
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
  store i32 %122, i32* %124, align 8, !tbaa !20
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
  store i64 %141, i64* %142, align 8, !tbaa !20
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
  store double %159, double* %161, align 8, !tbaa !20
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
  store float %179, float* %181, align 8, !tbaa !20
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
  store %struct._jobject* %198, %struct._jobject** %200, align 8, !tbaa !20
  br label %201

201:                                              ; preds = %39, %59, %79, %99, %119, %138, %156, %175, %195, %22
  %202 = add nuw nsw i64 %23, 1
  %203 = icmp eq i64 %202, %21
  br i1 %203, label %204, label %22, !llvm.loop !88

204:                                              ; preds = %201, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %205 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %206 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %205, i64 0, i32 87
  %207 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %206, align 8, !tbaa !89
  %208 = call float %207(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret float %208
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local float @JNI_CallStaticFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 136
  %9 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !90
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  %11 = call float %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret float %11
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local float @JNI_CallStaticFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !91

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 137
  %206 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !92
  %207 = call float %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret float %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local double @JNI_CallDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 59
  %9 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !93
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  %11 = call double %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret double %11
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local double @JNI_CallDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !94

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 60
  %206 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !95
  %207 = call double %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret double %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local double @JNI_CallNonvirtualDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = alloca %"struct.std::__va_list", align 8
  %7 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %9 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %8, i64 0, i32 89
  %10 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %9, align 8, !tbaa !96
  %11 = bitcast %"struct.std::__va_list"* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %11) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %11, i8* noundef nonnull align 8 dereferenceable(32) %7, i64 32, i1 false), !tbaa.struct !16
  %12 = call double %10(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef nonnull %6) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %11) #4
  call void @llvm.va_end(i8* nonnull %7)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %7) #4
  ret double %12
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local double @JNI_CallNonvirtualDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  %7 = getelementptr inbounds [257 x i8], [257 x i8]* %6, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %7) #4
  %8 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !10
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !19
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
  %25 = load i8, i8* %24, align 1, !tbaa !20
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
  store i8 %43, i8* %45, align 8, !tbaa !20
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
  store i8 %63, i8* %65, align 8, !tbaa !20
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
  store i16 %83, i16* %85, align 8, !tbaa !20
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
  store i32 %103, i32* %105, align 8, !tbaa !20
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
  store i32 %122, i32* %124, align 8, !tbaa !20
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
  store i64 %141, i64* %142, align 8, !tbaa !20
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
  store double %159, double* %161, align 8, !tbaa !20
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
  store float %179, float* %181, align 8, !tbaa !20
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
  store %struct._jobject* %198, %struct._jobject** %200, align 8, !tbaa !20
  br label %201

201:                                              ; preds = %39, %59, %79, %99, %119, %138, %156, %175, %195, %22
  %202 = add nuw nsw i64 %23, 1
  %203 = icmp eq i64 %202, %21
  br i1 %203, label %204, label %22, !llvm.loop !97

204:                                              ; preds = %201, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %205 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %206 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %205, i64 0, i32 90
  %207 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %206, align 8, !tbaa !98
  %208 = call double %207(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret double %208
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local double @JNI_CallStaticDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 139
  %9 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !99
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  %11 = call double %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret double %11
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local double @JNI_CallStaticDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !100

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 140
  %206 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !101
  %207 = call double %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret double %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_NewObject(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 29
  %9 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !102
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  %11 = call %struct._jobject* %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.va_end(i8* nonnull %6)
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %6) #4
  ret %struct._jobject* %11
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local %struct._jobject* @JNI_NewObjectV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = getelementptr inbounds [257 x i8], [257 x i8]* %5, i64 0, i64 0
  call void @llvm.lifetime.start.p0i8(i64 257, i8* nonnull %6) #4
  %7 = bitcast %struct.JNINativeInterface_** %0 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)***
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !103

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 30
  %206 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !104
  %207 = call %struct._jobject* %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret %struct._jobject* %207
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local void @JNI_CallVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 62
  %9 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !105
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  call void %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
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
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !106

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 63
  %206 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !107
  call void %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local void @JNI_CallNonvirtualVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = alloca %"struct.std::__va_list", align 8
  %7 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %7) #4
  call void @llvm.va_start(i8* nonnull %7)
  %8 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %9 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %8, i64 0, i32 92
  %10 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %9, align 8, !tbaa !108
  %11 = bitcast %"struct.std::__va_list"* %6 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %11) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %11, i8* noundef nonnull align 8 dereferenceable(32) %7, i64 32, i1 false), !tbaa.struct !16
  call void %10(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef nonnull %6) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %11) #4
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
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %8, align 8, !tbaa !10
  %10 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %9, align 8, !tbaa !19
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
  %25 = load i8, i8* %24, align 1, !tbaa !20
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
  store i8 %43, i8* %45, align 8, !tbaa !20
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
  store i8 %63, i8* %65, align 8, !tbaa !20
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
  store i16 %83, i16* %85, align 8, !tbaa !20
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
  store i32 %103, i32* %105, align 8, !tbaa !20
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
  store i32 %122, i32* %124, align 8, !tbaa !20
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
  store i64 %141, i64* %142, align 8, !tbaa !20
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
  store double %159, double* %161, align 8, !tbaa !20
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
  store float %179, float* %181, align 8, !tbaa !20
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
  store %struct._jobject* %198, %struct._jobject** %200, align 8, !tbaa !20
  br label %201

201:                                              ; preds = %39, %59, %79, %99, %119, %138, %156, %175, %195, %22
  %202 = add nuw nsw i64 %23, 1
  %203 = icmp eq i64 %202, %21
  br i1 %203, label %204, label %22, !llvm.loop !109

204:                                              ; preds = %201, %5
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %7) #4
  %205 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %206 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %205, i64 0, i32 93
  %207 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %206, align 8, !tbaa !110
  call void %207(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %union.jvalue* noundef nonnull %13) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local void @JNI_CallStaticVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca %"struct.std::__va_list", align 8
  %5 = alloca %"struct.std::__va_list", align 8
  %6 = bitcast %"struct.std::__va_list"* %4 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %6) #4
  call void @llvm.va_start(i8* nonnull %6)
  %7 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %8 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %7, i64 0, i32 142
  %9 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %8, align 8, !tbaa !111
  %10 = bitcast %"struct.std::__va_list"* %5 to i8*
  call void @llvm.lifetime.start.p0i8(i64 32, i8* nonnull %10) #4
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* noundef nonnull align 8 dereferenceable(32) %10, i8* noundef nonnull align 8 dereferenceable(32) %6, i64 32, i1 false), !tbaa.struct !16
  call void %9(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef nonnull %5) #4
  call void @llvm.lifetime.end.p0i8(i64 32, i8* nonnull %10) #4
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
  %8 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)**, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*** %7, align 8, !tbaa !10
  %9 = load i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*, i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)** %8, align 8, !tbaa !19
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
  %24 = load i8, i8* %23, align 1, !tbaa !20
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
  store i8 %42, i8* %44, align 8, !tbaa !20
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
  store i8 %62, i8* %64, align 8, !tbaa !20
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
  store i16 %82, i16* %84, align 8, !tbaa !20
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
  store i32 %102, i32* %104, align 8, !tbaa !20
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
  store i32 %121, i32* %123, align 8, !tbaa !20
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
  store i64 %140, i64* %141, align 8, !tbaa !20
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
  store double %158, double* %160, align 8, !tbaa !20
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
  store float %178, float* %180, align 8, !tbaa !20
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
  store %struct._jobject* %197, %struct._jobject** %199, align 8, !tbaa !20
  br label %200

200:                                              ; preds = %38, %58, %78, %98, %118, %137, %155, %174, %194, %21
  %201 = add nuw nsw i64 %22, 1
  %202 = icmp eq i64 %201, %20
  br i1 %202, label %203, label %21, !llvm.loop !112

203:                                              ; preds = %200, %4
  call void @llvm.lifetime.end.p0i8(i64 257, i8* nonnull %6) #4
  %204 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %0, align 8, !tbaa !10
  %205 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %204, i64 0, i32 143
  %206 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %205, align 8, !tbaa !113
  call void %206(%struct.JNINativeInterface_** noundef nonnull %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %union.jvalue* noundef nonnull %12) #4
  ret void
}

attributes #0 = { alwaysinline nounwind uwtable "frame-pointer"="non-leaf" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+neon,+outline-atomics,+v8a" }
attributes #1 = { argmemonly mustprogress nofree nosync nounwind willreturn }
attributes #2 = { mustprogress nofree nosync nounwind willreturn }
attributes #3 = { argmemonly mustprogress nofree nounwind willreturn }
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
!10 = !{!11, !11, i64 0}
!11 = !{!"any pointer", !12, i64 0}
!12 = !{!"omnipotent char", !13, i64 0}
!13 = !{!"Simple C/C++ TBAA"}
!14 = !{!15, !11, i64 280}
!15 = !{!"JNINativeInterface_", !11, i64 0, !11, i64 8, !11, i64 16, !11, i64 24, !11, i64 32, !11, i64 40, !11, i64 48, !11, i64 56, !11, i64 64, !11, i64 72, !11, i64 80, !11, i64 88, !11, i64 96, !11, i64 104, !11, i64 112, !11, i64 120, !11, i64 128, !11, i64 136, !11, i64 144, !11, i64 152, !11, i64 160, !11, i64 168, !11, i64 176, !11, i64 184, !11, i64 192, !11, i64 200, !11, i64 208, !11, i64 216, !11, i64 224, !11, i64 232, !11, i64 240, !11, i64 248, !11, i64 256, !11, i64 264, !11, i64 272, !11, i64 280, !11, i64 288, !11, i64 296, !11, i64 304, !11, i64 312, !11, i64 320, !11, i64 328, !11, i64 336, !11, i64 344, !11, i64 352, !11, i64 360, !11, i64 368, !11, i64 376, !11, i64 384, !11, i64 392, !11, i64 400, !11, i64 408, !11, i64 416, !11, i64 424, !11, i64 432, !11, i64 440, !11, i64 448, !11, i64 456, !11, i64 464, !11, i64 472, !11, i64 480, !11, i64 488, !11, i64 496, !11, i64 504, !11, i64 512, !11, i64 520, !11, i64 528, !11, i64 536, !11, i64 544, !11, i64 552, !11, i64 560, !11, i64 568, !11, i64 576, !11, i64 584, !11, i64 592, !11, i64 600, !11, i64 608, !11, i64 616, !11, i64 624, !11, i64 632, !11, i64 640, !11, i64 648, !11, i64 656, !11, i64 664, !11, i64 672, !11, i64 680, !11, i64 688, !11, i64 696, !11, i64 704, !11, i64 712, !11, i64 720, !11, i64 728, !11, i64 736, !11, i64 744, !11, i64 752, !11, i64 760, !11, i64 768, !11, i64 776, !11, i64 784, !11, i64 792, !11, i64 800, !11, i64 808, !11, i64 816, !11, i64 824, !11, i64 832, !11, i64 840, !11, i64 848, !11, i64 856, !11, i64 864, !11, i64 872, !11, i64 880, !11, i64 888, !11, i64 896, !11, i64 904, !11, i64 912, !11, i64 920, !11, i64 928, !11, i64 936, !11, i64 944, !11, i64 952, !11, i64 960, !11, i64 968, !11, i64 976, !11, i64 984, !11, i64 992, !11, i64 1000, !11, i64 1008, !11, i64 1016, !11, i64 1024, !11, i64 1032, !11, i64 1040, !11, i64 1048, !11, i64 1056, !11, i64 1064, !11, i64 1072, !11, i64 1080, !11, i64 1088, !11, i64 1096, !11, i64 1104, !11, i64 1112, !11, i64 1120, !11, i64 1128, !11, i64 1136, !11, i64 1144, !11, i64 1152, !11, i64 1160, !11, i64 1168, !11, i64 1176, !11, i64 1184, !11, i64 1192, !11, i64 1200, !11, i64 1208, !11, i64 1216, !11, i64 1224, !11, i64 1232, !11, i64 1240, !11, i64 1248, !11, i64 1256, !11, i64 1264, !11, i64 1272, !11, i64 1280, !11, i64 1288, !11, i64 1296, !11, i64 1304, !11, i64 1312, !11, i64 1320, !11, i64 1328, !11, i64 1336, !11, i64 1344, !11, i64 1352, !11, i64 1360, !11, i64 1368, !11, i64 1376, !11, i64 1384, !11, i64 1392, !11, i64 1400, !11, i64 1408, !11, i64 1416, !11, i64 1424, !11, i64 1432, !11, i64 1440, !11, i64 1448, !11, i64 1456, !11, i64 1464, !11, i64 1472, !11, i64 1480, !11, i64 1488, !11, i64 1496, !11, i64 1504, !11, i64 1512, !11, i64 1520, !11, i64 1528, !11, i64 1536, !11, i64 1544, !11, i64 1552, !11, i64 1560, !11, i64 1568, !11, i64 1576, !11, i64 1584, !11, i64 1592, !11, i64 1600, !11, i64 1608, !11, i64 1616, !11, i64 1624, !11, i64 1632, !11, i64 1640, !11, i64 1648, !11, i64 1656, !11, i64 1664, !11, i64 1672, !11, i64 1680, !11, i64 1688, !11, i64 1696, !11, i64 1704, !11, i64 1712, !11, i64 1720, !11, i64 1728, !11, i64 1736, !11, i64 1744, !11, i64 1752, !11, i64 1760, !11, i64 1768, !11, i64 1776, !11, i64 1784, !11, i64 1792, !11, i64 1800, !11, i64 1808, !11, i64 1816, !11, i64 1824, !11, i64 1832, !11, i64 1840, !11, i64 1848, !11, i64 1856}
!16 = !{i64 0, i64 8, !10, i64 8, i64 8, !10, i64 16, i64 8, !10, i64 24, i64 4, !17, i64 28, i64 4, !17}
!17 = !{!18, !18, i64 0}
!18 = !{!"int", !12, i64 0}
!19 = !{!15, !11, i64 0}
!20 = !{!12, !12, i64 0}
!21 = distinct !{!21, !22}
!22 = !{!"llvm.loop.mustprogress"}
!23 = !{!15, !11, i64 288}
!24 = !{!15, !11, i64 520}
!25 = distinct !{!25, !22}
!26 = !{!15, !11, i64 528}
!27 = !{!15, !11, i64 920}
!28 = distinct !{!28, !22}
!29 = !{!15, !11, i64 928}
!30 = !{!15, !11, i64 304}
!31 = distinct !{!31, !22}
!32 = !{!15, !11, i64 312}
!33 = !{!15, !11, i64 544}
!34 = distinct !{!34, !22}
!35 = !{!15, !11, i64 552}
!36 = !{!15, !11, i64 944}
!37 = distinct !{!37, !22}
!38 = !{!15, !11, i64 952}
!39 = !{!15, !11, i64 328}
!40 = distinct !{!40, !22}
!41 = !{!15, !11, i64 336}
!42 = !{!15, !11, i64 568}
!43 = distinct !{!43, !22}
!44 = !{!15, !11, i64 576}
!45 = !{!15, !11, i64 968}
!46 = distinct !{!46, !22}
!47 = !{!15, !11, i64 976}
!48 = !{!15, !11, i64 352}
!49 = distinct !{!49, !22}
!50 = !{!15, !11, i64 360}
!51 = !{!15, !11, i64 592}
!52 = distinct !{!52, !22}
!53 = !{!15, !11, i64 600}
!54 = !{!15, !11, i64 992}
!55 = distinct !{!55, !22}
!56 = !{!15, !11, i64 1000}
!57 = !{!15, !11, i64 376}
!58 = distinct !{!58, !22}
!59 = !{!15, !11, i64 384}
!60 = !{!15, !11, i64 616}
!61 = distinct !{!61, !22}
!62 = !{!15, !11, i64 624}
!63 = !{!15, !11, i64 1016}
!64 = distinct !{!64, !22}
!65 = !{!15, !11, i64 1024}
!66 = !{!15, !11, i64 400}
!67 = distinct !{!67, !22}
!68 = !{!15, !11, i64 408}
!69 = !{!15, !11, i64 640}
!70 = distinct !{!70, !22}
!71 = !{!15, !11, i64 648}
!72 = !{!15, !11, i64 1040}
!73 = distinct !{!73, !22}
!74 = !{!15, !11, i64 1048}
!75 = !{!15, !11, i64 424}
!76 = distinct !{!76, !22}
!77 = !{!15, !11, i64 432}
!78 = !{!15, !11, i64 664}
!79 = distinct !{!79, !22}
!80 = !{!15, !11, i64 672}
!81 = !{!15, !11, i64 1064}
!82 = distinct !{!82, !22}
!83 = !{!15, !11, i64 1072}
!84 = !{!15, !11, i64 448}
!85 = distinct !{!85, !22}
!86 = !{!15, !11, i64 456}
!87 = !{!15, !11, i64 688}
!88 = distinct !{!88, !22}
!89 = !{!15, !11, i64 696}
!90 = !{!15, !11, i64 1088}
!91 = distinct !{!91, !22}
!92 = !{!15, !11, i64 1096}
!93 = !{!15, !11, i64 472}
!94 = distinct !{!94, !22}
!95 = !{!15, !11, i64 480}
!96 = !{!15, !11, i64 712}
!97 = distinct !{!97, !22}
!98 = !{!15, !11, i64 720}
!99 = !{!15, !11, i64 1112}
!100 = distinct !{!100, !22}
!101 = !{!15, !11, i64 1120}
!102 = !{!15, !11, i64 232}
!103 = distinct !{!103, !22}
!104 = !{!15, !11, i64 240}
!105 = !{!15, !11, i64 496}
!106 = distinct !{!106, !22}
!107 = !{!15, !11, i64 504}
!108 = !{!15, !11, i64 736}
!109 = distinct !{!109, !22}
!110 = !{!15, !11, i64 744}
!111 = !{!15, !11, i64 1136}
!112 = distinct !{!112, !22}
!113 = !{!15, !11, i64 1144}
