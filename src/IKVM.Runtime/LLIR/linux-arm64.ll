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

; Function Attrs: noinline nounwind optnone uwtable
define dso_local %struct._jobject* @JNI_CallObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %"struct.std::__va_list", align 8
  %9 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %10 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 35
  %14 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = bitcast %"struct.std::__va_list"* %9 to i8*
  %19 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %18, i8* align 8 %19, i64 32, i1 false)
  %20 = call %struct._jobject* %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %"struct.std::__va_list"* noundef %9)
  store %struct._jobject* %20, %struct._jobject** %7, align 8
  %21 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load %struct._jobject*, %struct._jobject** %7, align 8
  ret %struct._jobject* %22
}

; Function Attrs: nofree nosync nounwind willreturn
declare void @llvm.va_start(i8*) #1

; Function Attrs: argmemonly nofree nounwind willreturn
declare void @llvm.memcpy.p0i8.p0i8.i64(i8* noalias nocapture writeonly, i8* noalias nocapture readonly, i64, i1 immarg) #2

; Function Attrs: nofree nosync nounwind willreturn
declare void @llvm.va_end(i8*) #1

; Function Attrs: noinline nounwind optnone uwtable
define dso_local %struct._jobject* @JNI_CallObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !10

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 36
  %278 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  %283 = call %struct._jobject* %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret %struct._jobject* %283
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local %struct._jobject* @JNI_CallNonvirtualObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca %struct._jobject*, align 8
  %10 = alloca %"struct.std::__va_list", align 8
  %11 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %12 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.va_start(i8* %12)
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 65
  %16 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %15, align 8
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %18 = load %struct._jobject*, %struct._jobject** %6, align 8
  %19 = load %struct._jobject*, %struct._jobject** %7, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %21 = bitcast %"struct.std::__va_list"* %11 to i8*
  %22 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %21, i8* align 8 %22, i64 32, i1 false)
  %23 = call %struct._jobject* %16(%struct.JNINativeInterface_** noundef %17, %struct._jobject* noundef %18, %struct._jobject* noundef %19, %struct._jmethodID* noundef %20, %"struct.std::__va_list"* noundef %11)
  store %struct._jobject* %23, %struct._jobject** %9, align 8
  %24 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load %struct._jobject*, %struct._jobject** %9, align 8
  ret %struct._jobject* %25
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local %struct._jobject* @JNI_CallNonvirtualObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca %union.jvalue*, align 8
  %11 = alloca [257 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  br label %14

14:                                               ; preds = %5
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 8
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 8
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %21 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i64 0, i64 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %12, align 4
  %24 = load i32, i32* %12, align 4
  %25 = sext i32 %24 to i64
  %26 = mul i64 %25, 8
  %27 = alloca i8, i64 %26, align 16
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %10, align 8
  store i32 0, i32* %13, align 4
  br label %29

29:                                               ; preds = %272, %14
  %30 = load i32, i32* %13, align 4
  %31 = load i32, i32* %12, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %275

33:                                               ; preds = %29
  %34 = load i32, i32* %13, align 4
  %35 = sext i32 %34 to i64
  %36 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i64 0, i64 %35
  %37 = load i8, i8* %36, align 1
  %38 = zext i8 %37 to i32
  switch i32 %38, label %271 [
    i32 90, label %39
    i32 66, label %65
    i32 83, label %91
    i32 67, label %117
    i32 73, label %144
    i32 74, label %169
    i32 68, label %194
    i32 70, label %219
    i32 76, label %245
  ]

39:                                               ; preds = %33
  %40 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %41 = load i32, i32* %40, align 8
  %42 = icmp sge i32 %41, 0
  br i1 %42, label %51, label %43

43:                                               ; preds = %39
  %44 = add i32 %41, 8
  store i32 %44, i32* %40, align 8
  %45 = icmp sle i32 %44, 0
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %48 = load i8*, i8** %47, align 8
  %49 = getelementptr inbounds i8, i8* %48, i32 %41
  %50 = bitcast i8* %49 to i32*
  br label %56

51:                                               ; preds = %43, %39
  %52 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %53 = load i8*, i8** %52, align 8
  %54 = getelementptr inbounds i8, i8* %53, i64 8
  store i8* %54, i8** %52, align 8
  %55 = bitcast i8* %53 to i32*
  br label %56

56:                                               ; preds = %51, %46
  %57 = phi i32* [ %50, %46 ], [ %55, %51 ]
  %58 = load i32, i32* %57, align 8
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %10, align 8
  %61 = load i32, i32* %13, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %59, i8* %64, align 8
  br label %271

65:                                               ; preds = %33
  %66 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %67 = load i32, i32* %66, align 8
  %68 = icmp sge i32 %67, 0
  br i1 %68, label %77, label %69

69:                                               ; preds = %65
  %70 = add i32 %67, 8
  store i32 %70, i32* %66, align 8
  %71 = icmp sle i32 %70, 0
  br i1 %71, label %72, label %77

72:                                               ; preds = %69
  %73 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %74 = load i8*, i8** %73, align 8
  %75 = getelementptr inbounds i8, i8* %74, i32 %67
  %76 = bitcast i8* %75 to i32*
  br label %82

77:                                               ; preds = %69, %65
  %78 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %79 = load i8*, i8** %78, align 8
  %80 = getelementptr inbounds i8, i8* %79, i64 8
  store i8* %80, i8** %78, align 8
  %81 = bitcast i8* %79 to i32*
  br label %82

82:                                               ; preds = %77, %72
  %83 = phi i32* [ %76, %72 ], [ %81, %77 ]
  %84 = load i32, i32* %83, align 8
  %85 = trunc i32 %84 to i8
  %86 = load %union.jvalue*, %union.jvalue** %10, align 8
  %87 = load i32, i32* %13, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, %union.jvalue* %86, i64 %88
  %90 = bitcast %union.jvalue* %89 to i8*
  store i8 %85, i8* %90, align 8
  br label %271

91:                                               ; preds = %33
  %92 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %93 = load i32, i32* %92, align 8
  %94 = icmp sge i32 %93, 0
  br i1 %94, label %103, label %95

95:                                               ; preds = %91
  %96 = add i32 %93, 8
  store i32 %96, i32* %92, align 8
  %97 = icmp sle i32 %96, 0
  br i1 %97, label %98, label %103

98:                                               ; preds = %95
  %99 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %100 = load i8*, i8** %99, align 8
  %101 = getelementptr inbounds i8, i8* %100, i32 %93
  %102 = bitcast i8* %101 to i32*
  br label %108

103:                                              ; preds = %95, %91
  %104 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %105 = load i8*, i8** %104, align 8
  %106 = getelementptr inbounds i8, i8* %105, i64 8
  store i8* %106, i8** %104, align 8
  %107 = bitcast i8* %105 to i32*
  br label %108

108:                                              ; preds = %103, %98
  %109 = phi i32* [ %102, %98 ], [ %107, %103 ]
  %110 = load i32, i32* %109, align 8
  %111 = trunc i32 %110 to i16
  %112 = load %union.jvalue*, %union.jvalue** %10, align 8
  %113 = load i32, i32* %13, align 4
  %114 = sext i32 %113 to i64
  %115 = getelementptr inbounds %union.jvalue, %union.jvalue* %112, i64 %114
  %116 = bitcast %union.jvalue* %115 to i16*
  store i16 %111, i16* %116, align 8
  br label %271

117:                                              ; preds = %33
  %118 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %119 = load i32, i32* %118, align 8
  %120 = icmp sge i32 %119, 0
  br i1 %120, label %129, label %121

121:                                              ; preds = %117
  %122 = add i32 %119, 8
  store i32 %122, i32* %118, align 8
  %123 = icmp sle i32 %122, 0
  br i1 %123, label %124, label %129

124:                                              ; preds = %121
  %125 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %126 = load i8*, i8** %125, align 8
  %127 = getelementptr inbounds i8, i8* %126, i32 %119
  %128 = bitcast i8* %127 to i32*
  br label %134

129:                                              ; preds = %121, %117
  %130 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %131 = load i8*, i8** %130, align 8
  %132 = getelementptr inbounds i8, i8* %131, i64 8
  store i8* %132, i8** %130, align 8
  %133 = bitcast i8* %131 to i32*
  br label %134

134:                                              ; preds = %129, %124
  %135 = phi i32* [ %128, %124 ], [ %133, %129 ]
  %136 = load i32, i32* %135, align 8
  %137 = trunc i32 %136 to i16
  %138 = zext i16 %137 to i32
  %139 = load %union.jvalue*, %union.jvalue** %10, align 8
  %140 = load i32, i32* %13, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %139, i64 %141
  %143 = bitcast %union.jvalue* %142 to i32*
  store i32 %138, i32* %143, align 8
  br label %271

144:                                              ; preds = %33
  %145 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %146 = load i32, i32* %145, align 8
  %147 = icmp sge i32 %146, 0
  br i1 %147, label %156, label %148

148:                                              ; preds = %144
  %149 = add i32 %146, 8
  store i32 %149, i32* %145, align 8
  %150 = icmp sle i32 %149, 0
  br i1 %150, label %151, label %156

151:                                              ; preds = %148
  %152 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %153 = load i8*, i8** %152, align 8
  %154 = getelementptr inbounds i8, i8* %153, i32 %146
  %155 = bitcast i8* %154 to i32*
  br label %161

156:                                              ; preds = %148, %144
  %157 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %158 = load i8*, i8** %157, align 8
  %159 = getelementptr inbounds i8, i8* %158, i64 8
  store i8* %159, i8** %157, align 8
  %160 = bitcast i8* %158 to i32*
  br label %161

161:                                              ; preds = %156, %151
  %162 = phi i32* [ %155, %151 ], [ %160, %156 ]
  %163 = load i32, i32* %162, align 8
  %164 = load %union.jvalue*, %union.jvalue** %10, align 8
  %165 = load i32, i32* %13, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds %union.jvalue, %union.jvalue* %164, i64 %166
  %168 = bitcast %union.jvalue* %167 to i32*
  store i32 %163, i32* %168, align 8
  br label %271

169:                                              ; preds = %33
  %170 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %171 = load i32, i32* %170, align 8
  %172 = icmp sge i32 %171, 0
  br i1 %172, label %181, label %173

173:                                              ; preds = %169
  %174 = add i32 %171, 8
  store i32 %174, i32* %170, align 8
  %175 = icmp sle i32 %174, 0
  br i1 %175, label %176, label %181

176:                                              ; preds = %173
  %177 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %178 = load i8*, i8** %177, align 8
  %179 = getelementptr inbounds i8, i8* %178, i32 %171
  %180 = bitcast i8* %179 to i64*
  br label %186

181:                                              ; preds = %173, %169
  %182 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %183 = load i8*, i8** %182, align 8
  %184 = getelementptr inbounds i8, i8* %183, i64 8
  store i8* %184, i8** %182, align 8
  %185 = bitcast i8* %183 to i64*
  br label %186

186:                                              ; preds = %181, %176
  %187 = phi i64* [ %180, %176 ], [ %185, %181 ]
  %188 = load i64, i64* %187, align 8
  %189 = load %union.jvalue*, %union.jvalue** %10, align 8
  %190 = load i32, i32* %13, align 4
  %191 = sext i32 %190 to i64
  %192 = getelementptr inbounds %union.jvalue, %union.jvalue* %189, i64 %191
  %193 = bitcast %union.jvalue* %192 to i64*
  store i64 %188, i64* %193, align 8
  br label %271

194:                                              ; preds = %33
  %195 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 4
  %196 = load i32, i32* %195, align 4
  %197 = icmp sge i32 %196, 0
  br i1 %197, label %206, label %198

198:                                              ; preds = %194
  %199 = add i32 %196, 16
  store i32 %199, i32* %195, align 4
  %200 = icmp sle i32 %199, 0
  br i1 %200, label %201, label %206

201:                                              ; preds = %198
  %202 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 2
  %203 = load i8*, i8** %202, align 8
  %204 = getelementptr inbounds i8, i8* %203, i32 %196
  %205 = bitcast i8* %204 to double*
  br label %211

206:                                              ; preds = %198, %194
  %207 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %208 = load i8*, i8** %207, align 8
  %209 = getelementptr inbounds i8, i8* %208, i64 8
  store i8* %209, i8** %207, align 8
  %210 = bitcast i8* %208 to double*
  br label %211

211:                                              ; preds = %206, %201
  %212 = phi double* [ %205, %201 ], [ %210, %206 ]
  %213 = load double, double* %212, align 8
  %214 = load %union.jvalue*, %union.jvalue** %10, align 8
  %215 = load i32, i32* %13, align 4
  %216 = sext i32 %215 to i64
  %217 = getelementptr inbounds %union.jvalue, %union.jvalue* %214, i64 %216
  %218 = bitcast %union.jvalue* %217 to double*
  store double %213, double* %218, align 8
  br label %271

219:                                              ; preds = %33
  %220 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 4
  %221 = load i32, i32* %220, align 4
  %222 = icmp sge i32 %221, 0
  br i1 %222, label %231, label %223

223:                                              ; preds = %219
  %224 = add i32 %221, 16
  store i32 %224, i32* %220, align 4
  %225 = icmp sle i32 %224, 0
  br i1 %225, label %226, label %231

226:                                              ; preds = %223
  %227 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 2
  %228 = load i8*, i8** %227, align 8
  %229 = getelementptr inbounds i8, i8* %228, i32 %221
  %230 = bitcast i8* %229 to double*
  br label %236

231:                                              ; preds = %223, %219
  %232 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %233 = load i8*, i8** %232, align 8
  %234 = getelementptr inbounds i8, i8* %233, i64 8
  store i8* %234, i8** %232, align 8
  %235 = bitcast i8* %233 to double*
  br label %236

236:                                              ; preds = %231, %226
  %237 = phi double* [ %230, %226 ], [ %235, %231 ]
  %238 = load double, double* %237, align 8
  %239 = fptrunc double %238 to float
  %240 = load %union.jvalue*, %union.jvalue** %10, align 8
  %241 = load i32, i32* %13, align 4
  %242 = sext i32 %241 to i64
  %243 = getelementptr inbounds %union.jvalue, %union.jvalue* %240, i64 %242
  %244 = bitcast %union.jvalue* %243 to float*
  store float %239, float* %244, align 8
  br label %271

245:                                              ; preds = %33
  %246 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %247 = load i32, i32* %246, align 8
  %248 = icmp sge i32 %247, 0
  br i1 %248, label %257, label %249

249:                                              ; preds = %245
  %250 = add i32 %247, 8
  store i32 %250, i32* %246, align 8
  %251 = icmp sle i32 %250, 0
  br i1 %251, label %252, label %257

252:                                              ; preds = %249
  %253 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %254 = load i8*, i8** %253, align 8
  %255 = getelementptr inbounds i8, i8* %254, i32 %247
  %256 = bitcast i8* %255 to i8**
  br label %262

257:                                              ; preds = %249, %245
  %258 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %259 = load i8*, i8** %258, align 8
  %260 = getelementptr inbounds i8, i8* %259, i64 8
  store i8* %260, i8** %258, align 8
  %261 = bitcast i8* %259 to i8**
  br label %262

262:                                              ; preds = %257, %252
  %263 = phi i8** [ %256, %252 ], [ %261, %257 ]
  %264 = load i8*, i8** %263, align 8
  %265 = bitcast i8* %264 to %struct._jobject*
  %266 = load %union.jvalue*, %union.jvalue** %10, align 8
  %267 = load i32, i32* %13, align 4
  %268 = sext i32 %267 to i64
  %269 = getelementptr inbounds %union.jvalue, %union.jvalue* %266, i64 %268
  %270 = bitcast %union.jvalue* %269 to %struct._jobject**
  store %struct._jobject* %265, %struct._jobject** %270, align 8
  br label %271

271:                                              ; preds = %33, %262, %236, %211, %186, %161, %134, %108, %82, %56
  br label %272

272:                                              ; preds = %271
  %273 = load i32, i32* %13, align 4
  %274 = add nsw i32 %273, 1
  store i32 %274, i32* %13, align 4
  br label %29, !llvm.loop !12

275:                                              ; preds = %29
  br label %276

276:                                              ; preds = %275
  %277 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %278 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %277, align 8
  %279 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %278, i32 0, i32 66
  %280 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %279, align 8
  %281 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %282 = load %struct._jobject*, %struct._jobject** %7, align 8
  %283 = load %struct._jobject*, %struct._jobject** %8, align 8
  %284 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %285 = load %union.jvalue*, %union.jvalue** %10, align 8
  %286 = call %struct._jobject* %280(%struct.JNINativeInterface_** noundef %281, %struct._jobject* noundef %282, %struct._jobject* noundef %283, %struct._jmethodID* noundef %284, %union.jvalue* noundef %285)
  ret %struct._jobject* %286
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local %struct._jobject* @JNI_CallStaticObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %"struct.std::__va_list", align 8
  %9 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %10 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 115
  %14 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = bitcast %"struct.std::__va_list"* %9 to i8*
  %19 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %18, i8* align 8 %19, i64 32, i1 false)
  %20 = call %struct._jobject* %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %"struct.std::__va_list"* noundef %9)
  store %struct._jobject* %20, %struct._jobject** %7, align 8
  %21 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load %struct._jobject*, %struct._jobject** %7, align 8
  ret %struct._jobject* %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local %struct._jobject* @JNI_CallStaticObjectMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !13

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 116
  %278 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  %283 = call %struct._jobject* %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret %struct._jobject* %283
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i8 @JNI_CallBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8, align 1
  %8 = alloca %"struct.std::__va_list", align 8
  %9 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %10 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 38
  %14 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = bitcast %"struct.std::__va_list"* %9 to i8*
  %19 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %18, i8* align 8 %19, i64 32, i1 false)
  %20 = call i8 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %"struct.std::__va_list"* noundef %9)
  store i8 %20, i8* %7, align 1
  %21 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i8, i8* %7, align 1
  ret i8 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i8 @JNI_CallBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !14

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 39
  %278 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  %283 = call i8 %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret i8 %283
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i8 @JNI_CallNonvirtualBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i8, align 1
  %10 = alloca %"struct.std::__va_list", align 8
  %11 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %12 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.va_start(i8* %12)
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 68
  %16 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %15, align 8
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %18 = load %struct._jobject*, %struct._jobject** %6, align 8
  %19 = load %struct._jobject*, %struct._jobject** %7, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %21 = bitcast %"struct.std::__va_list"* %11 to i8*
  %22 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %21, i8* align 8 %22, i64 32, i1 false)
  %23 = call i8 %16(%struct.JNINativeInterface_** noundef %17, %struct._jobject* noundef %18, %struct._jobject* noundef %19, %struct._jmethodID* noundef %20, %"struct.std::__va_list"* noundef %11)
  store i8 %23, i8* %9, align 1
  %24 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load i8, i8* %9, align 1
  ret i8 %25
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i8 @JNI_CallNonvirtualBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca %union.jvalue*, align 8
  %11 = alloca [257 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  br label %14

14:                                               ; preds = %5
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 8
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 8
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %21 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i64 0, i64 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %12, align 4
  %24 = load i32, i32* %12, align 4
  %25 = sext i32 %24 to i64
  %26 = mul i64 %25, 8
  %27 = alloca i8, i64 %26, align 16
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %10, align 8
  store i32 0, i32* %13, align 4
  br label %29

29:                                               ; preds = %272, %14
  %30 = load i32, i32* %13, align 4
  %31 = load i32, i32* %12, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %275

33:                                               ; preds = %29
  %34 = load i32, i32* %13, align 4
  %35 = sext i32 %34 to i64
  %36 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i64 0, i64 %35
  %37 = load i8, i8* %36, align 1
  %38 = zext i8 %37 to i32
  switch i32 %38, label %271 [
    i32 90, label %39
    i32 66, label %65
    i32 83, label %91
    i32 67, label %117
    i32 73, label %144
    i32 74, label %169
    i32 68, label %194
    i32 70, label %219
    i32 76, label %245
  ]

39:                                               ; preds = %33
  %40 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %41 = load i32, i32* %40, align 8
  %42 = icmp sge i32 %41, 0
  br i1 %42, label %51, label %43

43:                                               ; preds = %39
  %44 = add i32 %41, 8
  store i32 %44, i32* %40, align 8
  %45 = icmp sle i32 %44, 0
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %48 = load i8*, i8** %47, align 8
  %49 = getelementptr inbounds i8, i8* %48, i32 %41
  %50 = bitcast i8* %49 to i32*
  br label %56

51:                                               ; preds = %43, %39
  %52 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %53 = load i8*, i8** %52, align 8
  %54 = getelementptr inbounds i8, i8* %53, i64 8
  store i8* %54, i8** %52, align 8
  %55 = bitcast i8* %53 to i32*
  br label %56

56:                                               ; preds = %51, %46
  %57 = phi i32* [ %50, %46 ], [ %55, %51 ]
  %58 = load i32, i32* %57, align 8
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %10, align 8
  %61 = load i32, i32* %13, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %59, i8* %64, align 8
  br label %271

65:                                               ; preds = %33
  %66 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %67 = load i32, i32* %66, align 8
  %68 = icmp sge i32 %67, 0
  br i1 %68, label %77, label %69

69:                                               ; preds = %65
  %70 = add i32 %67, 8
  store i32 %70, i32* %66, align 8
  %71 = icmp sle i32 %70, 0
  br i1 %71, label %72, label %77

72:                                               ; preds = %69
  %73 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %74 = load i8*, i8** %73, align 8
  %75 = getelementptr inbounds i8, i8* %74, i32 %67
  %76 = bitcast i8* %75 to i32*
  br label %82

77:                                               ; preds = %69, %65
  %78 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %79 = load i8*, i8** %78, align 8
  %80 = getelementptr inbounds i8, i8* %79, i64 8
  store i8* %80, i8** %78, align 8
  %81 = bitcast i8* %79 to i32*
  br label %82

82:                                               ; preds = %77, %72
  %83 = phi i32* [ %76, %72 ], [ %81, %77 ]
  %84 = load i32, i32* %83, align 8
  %85 = trunc i32 %84 to i8
  %86 = load %union.jvalue*, %union.jvalue** %10, align 8
  %87 = load i32, i32* %13, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, %union.jvalue* %86, i64 %88
  %90 = bitcast %union.jvalue* %89 to i8*
  store i8 %85, i8* %90, align 8
  br label %271

91:                                               ; preds = %33
  %92 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %93 = load i32, i32* %92, align 8
  %94 = icmp sge i32 %93, 0
  br i1 %94, label %103, label %95

95:                                               ; preds = %91
  %96 = add i32 %93, 8
  store i32 %96, i32* %92, align 8
  %97 = icmp sle i32 %96, 0
  br i1 %97, label %98, label %103

98:                                               ; preds = %95
  %99 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %100 = load i8*, i8** %99, align 8
  %101 = getelementptr inbounds i8, i8* %100, i32 %93
  %102 = bitcast i8* %101 to i32*
  br label %108

103:                                              ; preds = %95, %91
  %104 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %105 = load i8*, i8** %104, align 8
  %106 = getelementptr inbounds i8, i8* %105, i64 8
  store i8* %106, i8** %104, align 8
  %107 = bitcast i8* %105 to i32*
  br label %108

108:                                              ; preds = %103, %98
  %109 = phi i32* [ %102, %98 ], [ %107, %103 ]
  %110 = load i32, i32* %109, align 8
  %111 = trunc i32 %110 to i16
  %112 = load %union.jvalue*, %union.jvalue** %10, align 8
  %113 = load i32, i32* %13, align 4
  %114 = sext i32 %113 to i64
  %115 = getelementptr inbounds %union.jvalue, %union.jvalue* %112, i64 %114
  %116 = bitcast %union.jvalue* %115 to i16*
  store i16 %111, i16* %116, align 8
  br label %271

117:                                              ; preds = %33
  %118 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %119 = load i32, i32* %118, align 8
  %120 = icmp sge i32 %119, 0
  br i1 %120, label %129, label %121

121:                                              ; preds = %117
  %122 = add i32 %119, 8
  store i32 %122, i32* %118, align 8
  %123 = icmp sle i32 %122, 0
  br i1 %123, label %124, label %129

124:                                              ; preds = %121
  %125 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %126 = load i8*, i8** %125, align 8
  %127 = getelementptr inbounds i8, i8* %126, i32 %119
  %128 = bitcast i8* %127 to i32*
  br label %134

129:                                              ; preds = %121, %117
  %130 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %131 = load i8*, i8** %130, align 8
  %132 = getelementptr inbounds i8, i8* %131, i64 8
  store i8* %132, i8** %130, align 8
  %133 = bitcast i8* %131 to i32*
  br label %134

134:                                              ; preds = %129, %124
  %135 = phi i32* [ %128, %124 ], [ %133, %129 ]
  %136 = load i32, i32* %135, align 8
  %137 = trunc i32 %136 to i16
  %138 = zext i16 %137 to i32
  %139 = load %union.jvalue*, %union.jvalue** %10, align 8
  %140 = load i32, i32* %13, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %139, i64 %141
  %143 = bitcast %union.jvalue* %142 to i32*
  store i32 %138, i32* %143, align 8
  br label %271

144:                                              ; preds = %33
  %145 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %146 = load i32, i32* %145, align 8
  %147 = icmp sge i32 %146, 0
  br i1 %147, label %156, label %148

148:                                              ; preds = %144
  %149 = add i32 %146, 8
  store i32 %149, i32* %145, align 8
  %150 = icmp sle i32 %149, 0
  br i1 %150, label %151, label %156

151:                                              ; preds = %148
  %152 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %153 = load i8*, i8** %152, align 8
  %154 = getelementptr inbounds i8, i8* %153, i32 %146
  %155 = bitcast i8* %154 to i32*
  br label %161

156:                                              ; preds = %148, %144
  %157 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %158 = load i8*, i8** %157, align 8
  %159 = getelementptr inbounds i8, i8* %158, i64 8
  store i8* %159, i8** %157, align 8
  %160 = bitcast i8* %158 to i32*
  br label %161

161:                                              ; preds = %156, %151
  %162 = phi i32* [ %155, %151 ], [ %160, %156 ]
  %163 = load i32, i32* %162, align 8
  %164 = load %union.jvalue*, %union.jvalue** %10, align 8
  %165 = load i32, i32* %13, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds %union.jvalue, %union.jvalue* %164, i64 %166
  %168 = bitcast %union.jvalue* %167 to i32*
  store i32 %163, i32* %168, align 8
  br label %271

169:                                              ; preds = %33
  %170 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %171 = load i32, i32* %170, align 8
  %172 = icmp sge i32 %171, 0
  br i1 %172, label %181, label %173

173:                                              ; preds = %169
  %174 = add i32 %171, 8
  store i32 %174, i32* %170, align 8
  %175 = icmp sle i32 %174, 0
  br i1 %175, label %176, label %181

176:                                              ; preds = %173
  %177 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %178 = load i8*, i8** %177, align 8
  %179 = getelementptr inbounds i8, i8* %178, i32 %171
  %180 = bitcast i8* %179 to i64*
  br label %186

181:                                              ; preds = %173, %169
  %182 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %183 = load i8*, i8** %182, align 8
  %184 = getelementptr inbounds i8, i8* %183, i64 8
  store i8* %184, i8** %182, align 8
  %185 = bitcast i8* %183 to i64*
  br label %186

186:                                              ; preds = %181, %176
  %187 = phi i64* [ %180, %176 ], [ %185, %181 ]
  %188 = load i64, i64* %187, align 8
  %189 = load %union.jvalue*, %union.jvalue** %10, align 8
  %190 = load i32, i32* %13, align 4
  %191 = sext i32 %190 to i64
  %192 = getelementptr inbounds %union.jvalue, %union.jvalue* %189, i64 %191
  %193 = bitcast %union.jvalue* %192 to i64*
  store i64 %188, i64* %193, align 8
  br label %271

194:                                              ; preds = %33
  %195 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 4
  %196 = load i32, i32* %195, align 4
  %197 = icmp sge i32 %196, 0
  br i1 %197, label %206, label %198

198:                                              ; preds = %194
  %199 = add i32 %196, 16
  store i32 %199, i32* %195, align 4
  %200 = icmp sle i32 %199, 0
  br i1 %200, label %201, label %206

201:                                              ; preds = %198
  %202 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 2
  %203 = load i8*, i8** %202, align 8
  %204 = getelementptr inbounds i8, i8* %203, i32 %196
  %205 = bitcast i8* %204 to double*
  br label %211

206:                                              ; preds = %198, %194
  %207 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %208 = load i8*, i8** %207, align 8
  %209 = getelementptr inbounds i8, i8* %208, i64 8
  store i8* %209, i8** %207, align 8
  %210 = bitcast i8* %208 to double*
  br label %211

211:                                              ; preds = %206, %201
  %212 = phi double* [ %205, %201 ], [ %210, %206 ]
  %213 = load double, double* %212, align 8
  %214 = load %union.jvalue*, %union.jvalue** %10, align 8
  %215 = load i32, i32* %13, align 4
  %216 = sext i32 %215 to i64
  %217 = getelementptr inbounds %union.jvalue, %union.jvalue* %214, i64 %216
  %218 = bitcast %union.jvalue* %217 to double*
  store double %213, double* %218, align 8
  br label %271

219:                                              ; preds = %33
  %220 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 4
  %221 = load i32, i32* %220, align 4
  %222 = icmp sge i32 %221, 0
  br i1 %222, label %231, label %223

223:                                              ; preds = %219
  %224 = add i32 %221, 16
  store i32 %224, i32* %220, align 4
  %225 = icmp sle i32 %224, 0
  br i1 %225, label %226, label %231

226:                                              ; preds = %223
  %227 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 2
  %228 = load i8*, i8** %227, align 8
  %229 = getelementptr inbounds i8, i8* %228, i32 %221
  %230 = bitcast i8* %229 to double*
  br label %236

231:                                              ; preds = %223, %219
  %232 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %233 = load i8*, i8** %232, align 8
  %234 = getelementptr inbounds i8, i8* %233, i64 8
  store i8* %234, i8** %232, align 8
  %235 = bitcast i8* %233 to double*
  br label %236

236:                                              ; preds = %231, %226
  %237 = phi double* [ %230, %226 ], [ %235, %231 ]
  %238 = load double, double* %237, align 8
  %239 = fptrunc double %238 to float
  %240 = load %union.jvalue*, %union.jvalue** %10, align 8
  %241 = load i32, i32* %13, align 4
  %242 = sext i32 %241 to i64
  %243 = getelementptr inbounds %union.jvalue, %union.jvalue* %240, i64 %242
  %244 = bitcast %union.jvalue* %243 to float*
  store float %239, float* %244, align 8
  br label %271

245:                                              ; preds = %33
  %246 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %247 = load i32, i32* %246, align 8
  %248 = icmp sge i32 %247, 0
  br i1 %248, label %257, label %249

249:                                              ; preds = %245
  %250 = add i32 %247, 8
  store i32 %250, i32* %246, align 8
  %251 = icmp sle i32 %250, 0
  br i1 %251, label %252, label %257

252:                                              ; preds = %249
  %253 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %254 = load i8*, i8** %253, align 8
  %255 = getelementptr inbounds i8, i8* %254, i32 %247
  %256 = bitcast i8* %255 to i8**
  br label %262

257:                                              ; preds = %249, %245
  %258 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %259 = load i8*, i8** %258, align 8
  %260 = getelementptr inbounds i8, i8* %259, i64 8
  store i8* %260, i8** %258, align 8
  %261 = bitcast i8* %259 to i8**
  br label %262

262:                                              ; preds = %257, %252
  %263 = phi i8** [ %256, %252 ], [ %261, %257 ]
  %264 = load i8*, i8** %263, align 8
  %265 = bitcast i8* %264 to %struct._jobject*
  %266 = load %union.jvalue*, %union.jvalue** %10, align 8
  %267 = load i32, i32* %13, align 4
  %268 = sext i32 %267 to i64
  %269 = getelementptr inbounds %union.jvalue, %union.jvalue* %266, i64 %268
  %270 = bitcast %union.jvalue* %269 to %struct._jobject**
  store %struct._jobject* %265, %struct._jobject** %270, align 8
  br label %271

271:                                              ; preds = %33, %262, %236, %211, %186, %161, %134, %108, %82, %56
  br label %272

272:                                              ; preds = %271
  %273 = load i32, i32* %13, align 4
  %274 = add nsw i32 %273, 1
  store i32 %274, i32* %13, align 4
  br label %29, !llvm.loop !15

275:                                              ; preds = %29
  br label %276

276:                                              ; preds = %275
  %277 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %278 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %277, align 8
  %279 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %278, i32 0, i32 69
  %280 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %279, align 8
  %281 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %282 = load %struct._jobject*, %struct._jobject** %7, align 8
  %283 = load %struct._jobject*, %struct._jobject** %8, align 8
  %284 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %285 = load %union.jvalue*, %union.jvalue** %10, align 8
  %286 = call i8 %280(%struct.JNINativeInterface_** noundef %281, %struct._jobject* noundef %282, %struct._jobject* noundef %283, %struct._jmethodID* noundef %284, %union.jvalue* noundef %285)
  ret i8 %286
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i8 @JNI_CallStaticBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8, align 1
  %8 = alloca %"struct.std::__va_list", align 8
  %9 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %10 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 118
  %14 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = bitcast %"struct.std::__va_list"* %9 to i8*
  %19 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %18, i8* align 8 %19, i64 32, i1 false)
  %20 = call i8 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %"struct.std::__va_list"* noundef %9)
  store i8 %20, i8* %7, align 1
  %21 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i8, i8* %7, align 1
  ret i8 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i8 @JNI_CallStaticBooleanMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !16

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 119
  %278 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  %283 = call i8 %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret i8 %283
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i8 @JNI_CallByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8, align 1
  %8 = alloca %"struct.std::__va_list", align 8
  %9 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %10 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 41
  %14 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = bitcast %"struct.std::__va_list"* %9 to i8*
  %19 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %18, i8* align 8 %19, i64 32, i1 false)
  %20 = call i8 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %"struct.std::__va_list"* noundef %9)
  store i8 %20, i8* %7, align 1
  %21 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i8, i8* %7, align 1
  ret i8 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i8 @JNI_CallByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !17

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 42
  %278 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  %283 = call i8 %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret i8 %283
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i8 @JNI_CallNonvirtualByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i8, align 1
  %10 = alloca %"struct.std::__va_list", align 8
  %11 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %12 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.va_start(i8* %12)
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 71
  %16 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %15, align 8
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %18 = load %struct._jobject*, %struct._jobject** %6, align 8
  %19 = load %struct._jobject*, %struct._jobject** %7, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %21 = bitcast %"struct.std::__va_list"* %11 to i8*
  %22 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %21, i8* align 8 %22, i64 32, i1 false)
  %23 = call i8 %16(%struct.JNINativeInterface_** noundef %17, %struct._jobject* noundef %18, %struct._jobject* noundef %19, %struct._jmethodID* noundef %20, %"struct.std::__va_list"* noundef %11)
  store i8 %23, i8* %9, align 1
  %24 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load i8, i8* %9, align 1
  ret i8 %25
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i8 @JNI_CallNonvirtualByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca %union.jvalue*, align 8
  %11 = alloca [257 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  br label %14

14:                                               ; preds = %5
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 8
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 8
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %21 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i64 0, i64 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %12, align 4
  %24 = load i32, i32* %12, align 4
  %25 = sext i32 %24 to i64
  %26 = mul i64 %25, 8
  %27 = alloca i8, i64 %26, align 16
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %10, align 8
  store i32 0, i32* %13, align 4
  br label %29

29:                                               ; preds = %272, %14
  %30 = load i32, i32* %13, align 4
  %31 = load i32, i32* %12, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %275

33:                                               ; preds = %29
  %34 = load i32, i32* %13, align 4
  %35 = sext i32 %34 to i64
  %36 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i64 0, i64 %35
  %37 = load i8, i8* %36, align 1
  %38 = zext i8 %37 to i32
  switch i32 %38, label %271 [
    i32 90, label %39
    i32 66, label %65
    i32 83, label %91
    i32 67, label %117
    i32 73, label %144
    i32 74, label %169
    i32 68, label %194
    i32 70, label %219
    i32 76, label %245
  ]

39:                                               ; preds = %33
  %40 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %41 = load i32, i32* %40, align 8
  %42 = icmp sge i32 %41, 0
  br i1 %42, label %51, label %43

43:                                               ; preds = %39
  %44 = add i32 %41, 8
  store i32 %44, i32* %40, align 8
  %45 = icmp sle i32 %44, 0
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %48 = load i8*, i8** %47, align 8
  %49 = getelementptr inbounds i8, i8* %48, i32 %41
  %50 = bitcast i8* %49 to i32*
  br label %56

51:                                               ; preds = %43, %39
  %52 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %53 = load i8*, i8** %52, align 8
  %54 = getelementptr inbounds i8, i8* %53, i64 8
  store i8* %54, i8** %52, align 8
  %55 = bitcast i8* %53 to i32*
  br label %56

56:                                               ; preds = %51, %46
  %57 = phi i32* [ %50, %46 ], [ %55, %51 ]
  %58 = load i32, i32* %57, align 8
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %10, align 8
  %61 = load i32, i32* %13, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %59, i8* %64, align 8
  br label %271

65:                                               ; preds = %33
  %66 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %67 = load i32, i32* %66, align 8
  %68 = icmp sge i32 %67, 0
  br i1 %68, label %77, label %69

69:                                               ; preds = %65
  %70 = add i32 %67, 8
  store i32 %70, i32* %66, align 8
  %71 = icmp sle i32 %70, 0
  br i1 %71, label %72, label %77

72:                                               ; preds = %69
  %73 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %74 = load i8*, i8** %73, align 8
  %75 = getelementptr inbounds i8, i8* %74, i32 %67
  %76 = bitcast i8* %75 to i32*
  br label %82

77:                                               ; preds = %69, %65
  %78 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %79 = load i8*, i8** %78, align 8
  %80 = getelementptr inbounds i8, i8* %79, i64 8
  store i8* %80, i8** %78, align 8
  %81 = bitcast i8* %79 to i32*
  br label %82

82:                                               ; preds = %77, %72
  %83 = phi i32* [ %76, %72 ], [ %81, %77 ]
  %84 = load i32, i32* %83, align 8
  %85 = trunc i32 %84 to i8
  %86 = load %union.jvalue*, %union.jvalue** %10, align 8
  %87 = load i32, i32* %13, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, %union.jvalue* %86, i64 %88
  %90 = bitcast %union.jvalue* %89 to i8*
  store i8 %85, i8* %90, align 8
  br label %271

91:                                               ; preds = %33
  %92 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %93 = load i32, i32* %92, align 8
  %94 = icmp sge i32 %93, 0
  br i1 %94, label %103, label %95

95:                                               ; preds = %91
  %96 = add i32 %93, 8
  store i32 %96, i32* %92, align 8
  %97 = icmp sle i32 %96, 0
  br i1 %97, label %98, label %103

98:                                               ; preds = %95
  %99 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %100 = load i8*, i8** %99, align 8
  %101 = getelementptr inbounds i8, i8* %100, i32 %93
  %102 = bitcast i8* %101 to i32*
  br label %108

103:                                              ; preds = %95, %91
  %104 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %105 = load i8*, i8** %104, align 8
  %106 = getelementptr inbounds i8, i8* %105, i64 8
  store i8* %106, i8** %104, align 8
  %107 = bitcast i8* %105 to i32*
  br label %108

108:                                              ; preds = %103, %98
  %109 = phi i32* [ %102, %98 ], [ %107, %103 ]
  %110 = load i32, i32* %109, align 8
  %111 = trunc i32 %110 to i16
  %112 = load %union.jvalue*, %union.jvalue** %10, align 8
  %113 = load i32, i32* %13, align 4
  %114 = sext i32 %113 to i64
  %115 = getelementptr inbounds %union.jvalue, %union.jvalue* %112, i64 %114
  %116 = bitcast %union.jvalue* %115 to i16*
  store i16 %111, i16* %116, align 8
  br label %271

117:                                              ; preds = %33
  %118 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %119 = load i32, i32* %118, align 8
  %120 = icmp sge i32 %119, 0
  br i1 %120, label %129, label %121

121:                                              ; preds = %117
  %122 = add i32 %119, 8
  store i32 %122, i32* %118, align 8
  %123 = icmp sle i32 %122, 0
  br i1 %123, label %124, label %129

124:                                              ; preds = %121
  %125 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %126 = load i8*, i8** %125, align 8
  %127 = getelementptr inbounds i8, i8* %126, i32 %119
  %128 = bitcast i8* %127 to i32*
  br label %134

129:                                              ; preds = %121, %117
  %130 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %131 = load i8*, i8** %130, align 8
  %132 = getelementptr inbounds i8, i8* %131, i64 8
  store i8* %132, i8** %130, align 8
  %133 = bitcast i8* %131 to i32*
  br label %134

134:                                              ; preds = %129, %124
  %135 = phi i32* [ %128, %124 ], [ %133, %129 ]
  %136 = load i32, i32* %135, align 8
  %137 = trunc i32 %136 to i16
  %138 = zext i16 %137 to i32
  %139 = load %union.jvalue*, %union.jvalue** %10, align 8
  %140 = load i32, i32* %13, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %139, i64 %141
  %143 = bitcast %union.jvalue* %142 to i32*
  store i32 %138, i32* %143, align 8
  br label %271

144:                                              ; preds = %33
  %145 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %146 = load i32, i32* %145, align 8
  %147 = icmp sge i32 %146, 0
  br i1 %147, label %156, label %148

148:                                              ; preds = %144
  %149 = add i32 %146, 8
  store i32 %149, i32* %145, align 8
  %150 = icmp sle i32 %149, 0
  br i1 %150, label %151, label %156

151:                                              ; preds = %148
  %152 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %153 = load i8*, i8** %152, align 8
  %154 = getelementptr inbounds i8, i8* %153, i32 %146
  %155 = bitcast i8* %154 to i32*
  br label %161

156:                                              ; preds = %148, %144
  %157 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %158 = load i8*, i8** %157, align 8
  %159 = getelementptr inbounds i8, i8* %158, i64 8
  store i8* %159, i8** %157, align 8
  %160 = bitcast i8* %158 to i32*
  br label %161

161:                                              ; preds = %156, %151
  %162 = phi i32* [ %155, %151 ], [ %160, %156 ]
  %163 = load i32, i32* %162, align 8
  %164 = load %union.jvalue*, %union.jvalue** %10, align 8
  %165 = load i32, i32* %13, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds %union.jvalue, %union.jvalue* %164, i64 %166
  %168 = bitcast %union.jvalue* %167 to i32*
  store i32 %163, i32* %168, align 8
  br label %271

169:                                              ; preds = %33
  %170 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %171 = load i32, i32* %170, align 8
  %172 = icmp sge i32 %171, 0
  br i1 %172, label %181, label %173

173:                                              ; preds = %169
  %174 = add i32 %171, 8
  store i32 %174, i32* %170, align 8
  %175 = icmp sle i32 %174, 0
  br i1 %175, label %176, label %181

176:                                              ; preds = %173
  %177 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %178 = load i8*, i8** %177, align 8
  %179 = getelementptr inbounds i8, i8* %178, i32 %171
  %180 = bitcast i8* %179 to i64*
  br label %186

181:                                              ; preds = %173, %169
  %182 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %183 = load i8*, i8** %182, align 8
  %184 = getelementptr inbounds i8, i8* %183, i64 8
  store i8* %184, i8** %182, align 8
  %185 = bitcast i8* %183 to i64*
  br label %186

186:                                              ; preds = %181, %176
  %187 = phi i64* [ %180, %176 ], [ %185, %181 ]
  %188 = load i64, i64* %187, align 8
  %189 = load %union.jvalue*, %union.jvalue** %10, align 8
  %190 = load i32, i32* %13, align 4
  %191 = sext i32 %190 to i64
  %192 = getelementptr inbounds %union.jvalue, %union.jvalue* %189, i64 %191
  %193 = bitcast %union.jvalue* %192 to i64*
  store i64 %188, i64* %193, align 8
  br label %271

194:                                              ; preds = %33
  %195 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 4
  %196 = load i32, i32* %195, align 4
  %197 = icmp sge i32 %196, 0
  br i1 %197, label %206, label %198

198:                                              ; preds = %194
  %199 = add i32 %196, 16
  store i32 %199, i32* %195, align 4
  %200 = icmp sle i32 %199, 0
  br i1 %200, label %201, label %206

201:                                              ; preds = %198
  %202 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 2
  %203 = load i8*, i8** %202, align 8
  %204 = getelementptr inbounds i8, i8* %203, i32 %196
  %205 = bitcast i8* %204 to double*
  br label %211

206:                                              ; preds = %198, %194
  %207 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %208 = load i8*, i8** %207, align 8
  %209 = getelementptr inbounds i8, i8* %208, i64 8
  store i8* %209, i8** %207, align 8
  %210 = bitcast i8* %208 to double*
  br label %211

211:                                              ; preds = %206, %201
  %212 = phi double* [ %205, %201 ], [ %210, %206 ]
  %213 = load double, double* %212, align 8
  %214 = load %union.jvalue*, %union.jvalue** %10, align 8
  %215 = load i32, i32* %13, align 4
  %216 = sext i32 %215 to i64
  %217 = getelementptr inbounds %union.jvalue, %union.jvalue* %214, i64 %216
  %218 = bitcast %union.jvalue* %217 to double*
  store double %213, double* %218, align 8
  br label %271

219:                                              ; preds = %33
  %220 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 4
  %221 = load i32, i32* %220, align 4
  %222 = icmp sge i32 %221, 0
  br i1 %222, label %231, label %223

223:                                              ; preds = %219
  %224 = add i32 %221, 16
  store i32 %224, i32* %220, align 4
  %225 = icmp sle i32 %224, 0
  br i1 %225, label %226, label %231

226:                                              ; preds = %223
  %227 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 2
  %228 = load i8*, i8** %227, align 8
  %229 = getelementptr inbounds i8, i8* %228, i32 %221
  %230 = bitcast i8* %229 to double*
  br label %236

231:                                              ; preds = %223, %219
  %232 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %233 = load i8*, i8** %232, align 8
  %234 = getelementptr inbounds i8, i8* %233, i64 8
  store i8* %234, i8** %232, align 8
  %235 = bitcast i8* %233 to double*
  br label %236

236:                                              ; preds = %231, %226
  %237 = phi double* [ %230, %226 ], [ %235, %231 ]
  %238 = load double, double* %237, align 8
  %239 = fptrunc double %238 to float
  %240 = load %union.jvalue*, %union.jvalue** %10, align 8
  %241 = load i32, i32* %13, align 4
  %242 = sext i32 %241 to i64
  %243 = getelementptr inbounds %union.jvalue, %union.jvalue* %240, i64 %242
  %244 = bitcast %union.jvalue* %243 to float*
  store float %239, float* %244, align 8
  br label %271

245:                                              ; preds = %33
  %246 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %247 = load i32, i32* %246, align 8
  %248 = icmp sge i32 %247, 0
  br i1 %248, label %257, label %249

249:                                              ; preds = %245
  %250 = add i32 %247, 8
  store i32 %250, i32* %246, align 8
  %251 = icmp sle i32 %250, 0
  br i1 %251, label %252, label %257

252:                                              ; preds = %249
  %253 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %254 = load i8*, i8** %253, align 8
  %255 = getelementptr inbounds i8, i8* %254, i32 %247
  %256 = bitcast i8* %255 to i8**
  br label %262

257:                                              ; preds = %249, %245
  %258 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %259 = load i8*, i8** %258, align 8
  %260 = getelementptr inbounds i8, i8* %259, i64 8
  store i8* %260, i8** %258, align 8
  %261 = bitcast i8* %259 to i8**
  br label %262

262:                                              ; preds = %257, %252
  %263 = phi i8** [ %256, %252 ], [ %261, %257 ]
  %264 = load i8*, i8** %263, align 8
  %265 = bitcast i8* %264 to %struct._jobject*
  %266 = load %union.jvalue*, %union.jvalue** %10, align 8
  %267 = load i32, i32* %13, align 4
  %268 = sext i32 %267 to i64
  %269 = getelementptr inbounds %union.jvalue, %union.jvalue* %266, i64 %268
  %270 = bitcast %union.jvalue* %269 to %struct._jobject**
  store %struct._jobject* %265, %struct._jobject** %270, align 8
  br label %271

271:                                              ; preds = %33, %262, %236, %211, %186, %161, %134, %108, %82, %56
  br label %272

272:                                              ; preds = %271
  %273 = load i32, i32* %13, align 4
  %274 = add nsw i32 %273, 1
  store i32 %274, i32* %13, align 4
  br label %29, !llvm.loop !18

275:                                              ; preds = %29
  br label %276

276:                                              ; preds = %275
  %277 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %278 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %277, align 8
  %279 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %278, i32 0, i32 72
  %280 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %279, align 8
  %281 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %282 = load %struct._jobject*, %struct._jobject** %7, align 8
  %283 = load %struct._jobject*, %struct._jobject** %8, align 8
  %284 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %285 = load %union.jvalue*, %union.jvalue** %10, align 8
  %286 = call i8 %280(%struct.JNINativeInterface_** noundef %281, %struct._jobject* noundef %282, %struct._jobject* noundef %283, %struct._jmethodID* noundef %284, %union.jvalue* noundef %285)
  ret i8 %286
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i8 @JNI_CallStaticByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i8, align 1
  %8 = alloca %"struct.std::__va_list", align 8
  %9 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %10 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 121
  %14 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = bitcast %"struct.std::__va_list"* %9 to i8*
  %19 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %18, i8* align 8 %19, i64 32, i1 false)
  %20 = call i8 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %"struct.std::__va_list"* noundef %9)
  store i8 %20, i8* %7, align 1
  %21 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i8, i8* %7, align 1
  ret i8 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i8 @JNI_CallStaticByteMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !19

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 122
  %278 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  %283 = call i8 %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret i8 %283
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i16 @JNI_CallCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i16, align 2
  %8 = alloca %"struct.std::__va_list", align 8
  %9 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %10 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 44
  %14 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = bitcast %"struct.std::__va_list"* %9 to i8*
  %19 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %18, i8* align 8 %19, i64 32, i1 false)
  %20 = call i16 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %"struct.std::__va_list"* noundef %9)
  store i16 %20, i16* %7, align 2
  %21 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i16, i16* %7, align 2
  ret i16 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i16 @JNI_CallCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !20

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 45
  %278 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  %283 = call i16 %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret i16 %283
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i16 @JNI_CallNonvirtualCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i16, align 2
  %10 = alloca %"struct.std::__va_list", align 8
  %11 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %12 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.va_start(i8* %12)
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 74
  %16 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %15, align 8
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %18 = load %struct._jobject*, %struct._jobject** %6, align 8
  %19 = load %struct._jobject*, %struct._jobject** %7, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %21 = bitcast %"struct.std::__va_list"* %11 to i8*
  %22 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %21, i8* align 8 %22, i64 32, i1 false)
  %23 = call i16 %16(%struct.JNINativeInterface_** noundef %17, %struct._jobject* noundef %18, %struct._jobject* noundef %19, %struct._jmethodID* noundef %20, %"struct.std::__va_list"* noundef %11)
  store i16 %23, i16* %9, align 2
  %24 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load i16, i16* %9, align 2
  ret i16 %25
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i16 @JNI_CallNonvirtualCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca %union.jvalue*, align 8
  %11 = alloca [257 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  br label %14

14:                                               ; preds = %5
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 8
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 8
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %21 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i64 0, i64 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %12, align 4
  %24 = load i32, i32* %12, align 4
  %25 = sext i32 %24 to i64
  %26 = mul i64 %25, 8
  %27 = alloca i8, i64 %26, align 16
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %10, align 8
  store i32 0, i32* %13, align 4
  br label %29

29:                                               ; preds = %272, %14
  %30 = load i32, i32* %13, align 4
  %31 = load i32, i32* %12, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %275

33:                                               ; preds = %29
  %34 = load i32, i32* %13, align 4
  %35 = sext i32 %34 to i64
  %36 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i64 0, i64 %35
  %37 = load i8, i8* %36, align 1
  %38 = zext i8 %37 to i32
  switch i32 %38, label %271 [
    i32 90, label %39
    i32 66, label %65
    i32 83, label %91
    i32 67, label %117
    i32 73, label %144
    i32 74, label %169
    i32 68, label %194
    i32 70, label %219
    i32 76, label %245
  ]

39:                                               ; preds = %33
  %40 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %41 = load i32, i32* %40, align 8
  %42 = icmp sge i32 %41, 0
  br i1 %42, label %51, label %43

43:                                               ; preds = %39
  %44 = add i32 %41, 8
  store i32 %44, i32* %40, align 8
  %45 = icmp sle i32 %44, 0
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %48 = load i8*, i8** %47, align 8
  %49 = getelementptr inbounds i8, i8* %48, i32 %41
  %50 = bitcast i8* %49 to i32*
  br label %56

51:                                               ; preds = %43, %39
  %52 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %53 = load i8*, i8** %52, align 8
  %54 = getelementptr inbounds i8, i8* %53, i64 8
  store i8* %54, i8** %52, align 8
  %55 = bitcast i8* %53 to i32*
  br label %56

56:                                               ; preds = %51, %46
  %57 = phi i32* [ %50, %46 ], [ %55, %51 ]
  %58 = load i32, i32* %57, align 8
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %10, align 8
  %61 = load i32, i32* %13, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %59, i8* %64, align 8
  br label %271

65:                                               ; preds = %33
  %66 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %67 = load i32, i32* %66, align 8
  %68 = icmp sge i32 %67, 0
  br i1 %68, label %77, label %69

69:                                               ; preds = %65
  %70 = add i32 %67, 8
  store i32 %70, i32* %66, align 8
  %71 = icmp sle i32 %70, 0
  br i1 %71, label %72, label %77

72:                                               ; preds = %69
  %73 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %74 = load i8*, i8** %73, align 8
  %75 = getelementptr inbounds i8, i8* %74, i32 %67
  %76 = bitcast i8* %75 to i32*
  br label %82

77:                                               ; preds = %69, %65
  %78 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %79 = load i8*, i8** %78, align 8
  %80 = getelementptr inbounds i8, i8* %79, i64 8
  store i8* %80, i8** %78, align 8
  %81 = bitcast i8* %79 to i32*
  br label %82

82:                                               ; preds = %77, %72
  %83 = phi i32* [ %76, %72 ], [ %81, %77 ]
  %84 = load i32, i32* %83, align 8
  %85 = trunc i32 %84 to i8
  %86 = load %union.jvalue*, %union.jvalue** %10, align 8
  %87 = load i32, i32* %13, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, %union.jvalue* %86, i64 %88
  %90 = bitcast %union.jvalue* %89 to i8*
  store i8 %85, i8* %90, align 8
  br label %271

91:                                               ; preds = %33
  %92 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %93 = load i32, i32* %92, align 8
  %94 = icmp sge i32 %93, 0
  br i1 %94, label %103, label %95

95:                                               ; preds = %91
  %96 = add i32 %93, 8
  store i32 %96, i32* %92, align 8
  %97 = icmp sle i32 %96, 0
  br i1 %97, label %98, label %103

98:                                               ; preds = %95
  %99 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %100 = load i8*, i8** %99, align 8
  %101 = getelementptr inbounds i8, i8* %100, i32 %93
  %102 = bitcast i8* %101 to i32*
  br label %108

103:                                              ; preds = %95, %91
  %104 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %105 = load i8*, i8** %104, align 8
  %106 = getelementptr inbounds i8, i8* %105, i64 8
  store i8* %106, i8** %104, align 8
  %107 = bitcast i8* %105 to i32*
  br label %108

108:                                              ; preds = %103, %98
  %109 = phi i32* [ %102, %98 ], [ %107, %103 ]
  %110 = load i32, i32* %109, align 8
  %111 = trunc i32 %110 to i16
  %112 = load %union.jvalue*, %union.jvalue** %10, align 8
  %113 = load i32, i32* %13, align 4
  %114 = sext i32 %113 to i64
  %115 = getelementptr inbounds %union.jvalue, %union.jvalue* %112, i64 %114
  %116 = bitcast %union.jvalue* %115 to i16*
  store i16 %111, i16* %116, align 8
  br label %271

117:                                              ; preds = %33
  %118 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %119 = load i32, i32* %118, align 8
  %120 = icmp sge i32 %119, 0
  br i1 %120, label %129, label %121

121:                                              ; preds = %117
  %122 = add i32 %119, 8
  store i32 %122, i32* %118, align 8
  %123 = icmp sle i32 %122, 0
  br i1 %123, label %124, label %129

124:                                              ; preds = %121
  %125 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %126 = load i8*, i8** %125, align 8
  %127 = getelementptr inbounds i8, i8* %126, i32 %119
  %128 = bitcast i8* %127 to i32*
  br label %134

129:                                              ; preds = %121, %117
  %130 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %131 = load i8*, i8** %130, align 8
  %132 = getelementptr inbounds i8, i8* %131, i64 8
  store i8* %132, i8** %130, align 8
  %133 = bitcast i8* %131 to i32*
  br label %134

134:                                              ; preds = %129, %124
  %135 = phi i32* [ %128, %124 ], [ %133, %129 ]
  %136 = load i32, i32* %135, align 8
  %137 = trunc i32 %136 to i16
  %138 = zext i16 %137 to i32
  %139 = load %union.jvalue*, %union.jvalue** %10, align 8
  %140 = load i32, i32* %13, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %139, i64 %141
  %143 = bitcast %union.jvalue* %142 to i32*
  store i32 %138, i32* %143, align 8
  br label %271

144:                                              ; preds = %33
  %145 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %146 = load i32, i32* %145, align 8
  %147 = icmp sge i32 %146, 0
  br i1 %147, label %156, label %148

148:                                              ; preds = %144
  %149 = add i32 %146, 8
  store i32 %149, i32* %145, align 8
  %150 = icmp sle i32 %149, 0
  br i1 %150, label %151, label %156

151:                                              ; preds = %148
  %152 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %153 = load i8*, i8** %152, align 8
  %154 = getelementptr inbounds i8, i8* %153, i32 %146
  %155 = bitcast i8* %154 to i32*
  br label %161

156:                                              ; preds = %148, %144
  %157 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %158 = load i8*, i8** %157, align 8
  %159 = getelementptr inbounds i8, i8* %158, i64 8
  store i8* %159, i8** %157, align 8
  %160 = bitcast i8* %158 to i32*
  br label %161

161:                                              ; preds = %156, %151
  %162 = phi i32* [ %155, %151 ], [ %160, %156 ]
  %163 = load i32, i32* %162, align 8
  %164 = load %union.jvalue*, %union.jvalue** %10, align 8
  %165 = load i32, i32* %13, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds %union.jvalue, %union.jvalue* %164, i64 %166
  %168 = bitcast %union.jvalue* %167 to i32*
  store i32 %163, i32* %168, align 8
  br label %271

169:                                              ; preds = %33
  %170 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %171 = load i32, i32* %170, align 8
  %172 = icmp sge i32 %171, 0
  br i1 %172, label %181, label %173

173:                                              ; preds = %169
  %174 = add i32 %171, 8
  store i32 %174, i32* %170, align 8
  %175 = icmp sle i32 %174, 0
  br i1 %175, label %176, label %181

176:                                              ; preds = %173
  %177 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %178 = load i8*, i8** %177, align 8
  %179 = getelementptr inbounds i8, i8* %178, i32 %171
  %180 = bitcast i8* %179 to i64*
  br label %186

181:                                              ; preds = %173, %169
  %182 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %183 = load i8*, i8** %182, align 8
  %184 = getelementptr inbounds i8, i8* %183, i64 8
  store i8* %184, i8** %182, align 8
  %185 = bitcast i8* %183 to i64*
  br label %186

186:                                              ; preds = %181, %176
  %187 = phi i64* [ %180, %176 ], [ %185, %181 ]
  %188 = load i64, i64* %187, align 8
  %189 = load %union.jvalue*, %union.jvalue** %10, align 8
  %190 = load i32, i32* %13, align 4
  %191 = sext i32 %190 to i64
  %192 = getelementptr inbounds %union.jvalue, %union.jvalue* %189, i64 %191
  %193 = bitcast %union.jvalue* %192 to i64*
  store i64 %188, i64* %193, align 8
  br label %271

194:                                              ; preds = %33
  %195 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 4
  %196 = load i32, i32* %195, align 4
  %197 = icmp sge i32 %196, 0
  br i1 %197, label %206, label %198

198:                                              ; preds = %194
  %199 = add i32 %196, 16
  store i32 %199, i32* %195, align 4
  %200 = icmp sle i32 %199, 0
  br i1 %200, label %201, label %206

201:                                              ; preds = %198
  %202 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 2
  %203 = load i8*, i8** %202, align 8
  %204 = getelementptr inbounds i8, i8* %203, i32 %196
  %205 = bitcast i8* %204 to double*
  br label %211

206:                                              ; preds = %198, %194
  %207 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %208 = load i8*, i8** %207, align 8
  %209 = getelementptr inbounds i8, i8* %208, i64 8
  store i8* %209, i8** %207, align 8
  %210 = bitcast i8* %208 to double*
  br label %211

211:                                              ; preds = %206, %201
  %212 = phi double* [ %205, %201 ], [ %210, %206 ]
  %213 = load double, double* %212, align 8
  %214 = load %union.jvalue*, %union.jvalue** %10, align 8
  %215 = load i32, i32* %13, align 4
  %216 = sext i32 %215 to i64
  %217 = getelementptr inbounds %union.jvalue, %union.jvalue* %214, i64 %216
  %218 = bitcast %union.jvalue* %217 to double*
  store double %213, double* %218, align 8
  br label %271

219:                                              ; preds = %33
  %220 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 4
  %221 = load i32, i32* %220, align 4
  %222 = icmp sge i32 %221, 0
  br i1 %222, label %231, label %223

223:                                              ; preds = %219
  %224 = add i32 %221, 16
  store i32 %224, i32* %220, align 4
  %225 = icmp sle i32 %224, 0
  br i1 %225, label %226, label %231

226:                                              ; preds = %223
  %227 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 2
  %228 = load i8*, i8** %227, align 8
  %229 = getelementptr inbounds i8, i8* %228, i32 %221
  %230 = bitcast i8* %229 to double*
  br label %236

231:                                              ; preds = %223, %219
  %232 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %233 = load i8*, i8** %232, align 8
  %234 = getelementptr inbounds i8, i8* %233, i64 8
  store i8* %234, i8** %232, align 8
  %235 = bitcast i8* %233 to double*
  br label %236

236:                                              ; preds = %231, %226
  %237 = phi double* [ %230, %226 ], [ %235, %231 ]
  %238 = load double, double* %237, align 8
  %239 = fptrunc double %238 to float
  %240 = load %union.jvalue*, %union.jvalue** %10, align 8
  %241 = load i32, i32* %13, align 4
  %242 = sext i32 %241 to i64
  %243 = getelementptr inbounds %union.jvalue, %union.jvalue* %240, i64 %242
  %244 = bitcast %union.jvalue* %243 to float*
  store float %239, float* %244, align 8
  br label %271

245:                                              ; preds = %33
  %246 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %247 = load i32, i32* %246, align 8
  %248 = icmp sge i32 %247, 0
  br i1 %248, label %257, label %249

249:                                              ; preds = %245
  %250 = add i32 %247, 8
  store i32 %250, i32* %246, align 8
  %251 = icmp sle i32 %250, 0
  br i1 %251, label %252, label %257

252:                                              ; preds = %249
  %253 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %254 = load i8*, i8** %253, align 8
  %255 = getelementptr inbounds i8, i8* %254, i32 %247
  %256 = bitcast i8* %255 to i8**
  br label %262

257:                                              ; preds = %249, %245
  %258 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %259 = load i8*, i8** %258, align 8
  %260 = getelementptr inbounds i8, i8* %259, i64 8
  store i8* %260, i8** %258, align 8
  %261 = bitcast i8* %259 to i8**
  br label %262

262:                                              ; preds = %257, %252
  %263 = phi i8** [ %256, %252 ], [ %261, %257 ]
  %264 = load i8*, i8** %263, align 8
  %265 = bitcast i8* %264 to %struct._jobject*
  %266 = load %union.jvalue*, %union.jvalue** %10, align 8
  %267 = load i32, i32* %13, align 4
  %268 = sext i32 %267 to i64
  %269 = getelementptr inbounds %union.jvalue, %union.jvalue* %266, i64 %268
  %270 = bitcast %union.jvalue* %269 to %struct._jobject**
  store %struct._jobject* %265, %struct._jobject** %270, align 8
  br label %271

271:                                              ; preds = %33, %262, %236, %211, %186, %161, %134, %108, %82, %56
  br label %272

272:                                              ; preds = %271
  %273 = load i32, i32* %13, align 4
  %274 = add nsw i32 %273, 1
  store i32 %274, i32* %13, align 4
  br label %29, !llvm.loop !21

275:                                              ; preds = %29
  br label %276

276:                                              ; preds = %275
  %277 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %278 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %277, align 8
  %279 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %278, i32 0, i32 75
  %280 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %279, align 8
  %281 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %282 = load %struct._jobject*, %struct._jobject** %7, align 8
  %283 = load %struct._jobject*, %struct._jobject** %8, align 8
  %284 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %285 = load %union.jvalue*, %union.jvalue** %10, align 8
  %286 = call i16 %280(%struct.JNINativeInterface_** noundef %281, %struct._jobject* noundef %282, %struct._jobject* noundef %283, %struct._jmethodID* noundef %284, %union.jvalue* noundef %285)
  ret i16 %286
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i16 @JNI_CallStaticCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i16, align 2
  %8 = alloca %"struct.std::__va_list", align 8
  %9 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %10 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 124
  %14 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = bitcast %"struct.std::__va_list"* %9 to i8*
  %19 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %18, i8* align 8 %19, i64 32, i1 false)
  %20 = call i16 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %"struct.std::__va_list"* noundef %9)
  store i16 %20, i16* %7, align 2
  %21 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i16, i16* %7, align 2
  ret i16 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i16 @JNI_CallStaticCharMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !22

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 125
  %278 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  %283 = call i16 %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret i16 %283
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i16 @JNI_CallShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i16, align 2
  %8 = alloca %"struct.std::__va_list", align 8
  %9 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %10 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 47
  %14 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = bitcast %"struct.std::__va_list"* %9 to i8*
  %19 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %18, i8* align 8 %19, i64 32, i1 false)
  %20 = call i16 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %"struct.std::__va_list"* noundef %9)
  store i16 %20, i16* %7, align 2
  %21 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i16, i16* %7, align 2
  ret i16 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i16 @JNI_CallShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !23

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 48
  %278 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  %283 = call i16 %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret i16 %283
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i16 @JNI_CallNonvirtualShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i16, align 2
  %10 = alloca %"struct.std::__va_list", align 8
  %11 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %12 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.va_start(i8* %12)
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 77
  %16 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %15, align 8
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %18 = load %struct._jobject*, %struct._jobject** %6, align 8
  %19 = load %struct._jobject*, %struct._jobject** %7, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %21 = bitcast %"struct.std::__va_list"* %11 to i8*
  %22 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %21, i8* align 8 %22, i64 32, i1 false)
  %23 = call i16 %16(%struct.JNINativeInterface_** noundef %17, %struct._jobject* noundef %18, %struct._jobject* noundef %19, %struct._jmethodID* noundef %20, %"struct.std::__va_list"* noundef %11)
  store i16 %23, i16* %9, align 2
  %24 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load i16, i16* %9, align 2
  ret i16 %25
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i16 @JNI_CallNonvirtualShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca %union.jvalue*, align 8
  %11 = alloca [257 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  br label %14

14:                                               ; preds = %5
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 8
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 8
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %21 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i64 0, i64 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %12, align 4
  %24 = load i32, i32* %12, align 4
  %25 = sext i32 %24 to i64
  %26 = mul i64 %25, 8
  %27 = alloca i8, i64 %26, align 16
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %10, align 8
  store i32 0, i32* %13, align 4
  br label %29

29:                                               ; preds = %272, %14
  %30 = load i32, i32* %13, align 4
  %31 = load i32, i32* %12, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %275

33:                                               ; preds = %29
  %34 = load i32, i32* %13, align 4
  %35 = sext i32 %34 to i64
  %36 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i64 0, i64 %35
  %37 = load i8, i8* %36, align 1
  %38 = zext i8 %37 to i32
  switch i32 %38, label %271 [
    i32 90, label %39
    i32 66, label %65
    i32 83, label %91
    i32 67, label %117
    i32 73, label %144
    i32 74, label %169
    i32 68, label %194
    i32 70, label %219
    i32 76, label %245
  ]

39:                                               ; preds = %33
  %40 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %41 = load i32, i32* %40, align 8
  %42 = icmp sge i32 %41, 0
  br i1 %42, label %51, label %43

43:                                               ; preds = %39
  %44 = add i32 %41, 8
  store i32 %44, i32* %40, align 8
  %45 = icmp sle i32 %44, 0
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %48 = load i8*, i8** %47, align 8
  %49 = getelementptr inbounds i8, i8* %48, i32 %41
  %50 = bitcast i8* %49 to i32*
  br label %56

51:                                               ; preds = %43, %39
  %52 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %53 = load i8*, i8** %52, align 8
  %54 = getelementptr inbounds i8, i8* %53, i64 8
  store i8* %54, i8** %52, align 8
  %55 = bitcast i8* %53 to i32*
  br label %56

56:                                               ; preds = %51, %46
  %57 = phi i32* [ %50, %46 ], [ %55, %51 ]
  %58 = load i32, i32* %57, align 8
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %10, align 8
  %61 = load i32, i32* %13, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %59, i8* %64, align 8
  br label %271

65:                                               ; preds = %33
  %66 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %67 = load i32, i32* %66, align 8
  %68 = icmp sge i32 %67, 0
  br i1 %68, label %77, label %69

69:                                               ; preds = %65
  %70 = add i32 %67, 8
  store i32 %70, i32* %66, align 8
  %71 = icmp sle i32 %70, 0
  br i1 %71, label %72, label %77

72:                                               ; preds = %69
  %73 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %74 = load i8*, i8** %73, align 8
  %75 = getelementptr inbounds i8, i8* %74, i32 %67
  %76 = bitcast i8* %75 to i32*
  br label %82

77:                                               ; preds = %69, %65
  %78 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %79 = load i8*, i8** %78, align 8
  %80 = getelementptr inbounds i8, i8* %79, i64 8
  store i8* %80, i8** %78, align 8
  %81 = bitcast i8* %79 to i32*
  br label %82

82:                                               ; preds = %77, %72
  %83 = phi i32* [ %76, %72 ], [ %81, %77 ]
  %84 = load i32, i32* %83, align 8
  %85 = trunc i32 %84 to i8
  %86 = load %union.jvalue*, %union.jvalue** %10, align 8
  %87 = load i32, i32* %13, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, %union.jvalue* %86, i64 %88
  %90 = bitcast %union.jvalue* %89 to i8*
  store i8 %85, i8* %90, align 8
  br label %271

91:                                               ; preds = %33
  %92 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %93 = load i32, i32* %92, align 8
  %94 = icmp sge i32 %93, 0
  br i1 %94, label %103, label %95

95:                                               ; preds = %91
  %96 = add i32 %93, 8
  store i32 %96, i32* %92, align 8
  %97 = icmp sle i32 %96, 0
  br i1 %97, label %98, label %103

98:                                               ; preds = %95
  %99 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %100 = load i8*, i8** %99, align 8
  %101 = getelementptr inbounds i8, i8* %100, i32 %93
  %102 = bitcast i8* %101 to i32*
  br label %108

103:                                              ; preds = %95, %91
  %104 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %105 = load i8*, i8** %104, align 8
  %106 = getelementptr inbounds i8, i8* %105, i64 8
  store i8* %106, i8** %104, align 8
  %107 = bitcast i8* %105 to i32*
  br label %108

108:                                              ; preds = %103, %98
  %109 = phi i32* [ %102, %98 ], [ %107, %103 ]
  %110 = load i32, i32* %109, align 8
  %111 = trunc i32 %110 to i16
  %112 = load %union.jvalue*, %union.jvalue** %10, align 8
  %113 = load i32, i32* %13, align 4
  %114 = sext i32 %113 to i64
  %115 = getelementptr inbounds %union.jvalue, %union.jvalue* %112, i64 %114
  %116 = bitcast %union.jvalue* %115 to i16*
  store i16 %111, i16* %116, align 8
  br label %271

117:                                              ; preds = %33
  %118 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %119 = load i32, i32* %118, align 8
  %120 = icmp sge i32 %119, 0
  br i1 %120, label %129, label %121

121:                                              ; preds = %117
  %122 = add i32 %119, 8
  store i32 %122, i32* %118, align 8
  %123 = icmp sle i32 %122, 0
  br i1 %123, label %124, label %129

124:                                              ; preds = %121
  %125 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %126 = load i8*, i8** %125, align 8
  %127 = getelementptr inbounds i8, i8* %126, i32 %119
  %128 = bitcast i8* %127 to i32*
  br label %134

129:                                              ; preds = %121, %117
  %130 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %131 = load i8*, i8** %130, align 8
  %132 = getelementptr inbounds i8, i8* %131, i64 8
  store i8* %132, i8** %130, align 8
  %133 = bitcast i8* %131 to i32*
  br label %134

134:                                              ; preds = %129, %124
  %135 = phi i32* [ %128, %124 ], [ %133, %129 ]
  %136 = load i32, i32* %135, align 8
  %137 = trunc i32 %136 to i16
  %138 = zext i16 %137 to i32
  %139 = load %union.jvalue*, %union.jvalue** %10, align 8
  %140 = load i32, i32* %13, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %139, i64 %141
  %143 = bitcast %union.jvalue* %142 to i32*
  store i32 %138, i32* %143, align 8
  br label %271

144:                                              ; preds = %33
  %145 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %146 = load i32, i32* %145, align 8
  %147 = icmp sge i32 %146, 0
  br i1 %147, label %156, label %148

148:                                              ; preds = %144
  %149 = add i32 %146, 8
  store i32 %149, i32* %145, align 8
  %150 = icmp sle i32 %149, 0
  br i1 %150, label %151, label %156

151:                                              ; preds = %148
  %152 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %153 = load i8*, i8** %152, align 8
  %154 = getelementptr inbounds i8, i8* %153, i32 %146
  %155 = bitcast i8* %154 to i32*
  br label %161

156:                                              ; preds = %148, %144
  %157 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %158 = load i8*, i8** %157, align 8
  %159 = getelementptr inbounds i8, i8* %158, i64 8
  store i8* %159, i8** %157, align 8
  %160 = bitcast i8* %158 to i32*
  br label %161

161:                                              ; preds = %156, %151
  %162 = phi i32* [ %155, %151 ], [ %160, %156 ]
  %163 = load i32, i32* %162, align 8
  %164 = load %union.jvalue*, %union.jvalue** %10, align 8
  %165 = load i32, i32* %13, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds %union.jvalue, %union.jvalue* %164, i64 %166
  %168 = bitcast %union.jvalue* %167 to i32*
  store i32 %163, i32* %168, align 8
  br label %271

169:                                              ; preds = %33
  %170 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %171 = load i32, i32* %170, align 8
  %172 = icmp sge i32 %171, 0
  br i1 %172, label %181, label %173

173:                                              ; preds = %169
  %174 = add i32 %171, 8
  store i32 %174, i32* %170, align 8
  %175 = icmp sle i32 %174, 0
  br i1 %175, label %176, label %181

176:                                              ; preds = %173
  %177 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %178 = load i8*, i8** %177, align 8
  %179 = getelementptr inbounds i8, i8* %178, i32 %171
  %180 = bitcast i8* %179 to i64*
  br label %186

181:                                              ; preds = %173, %169
  %182 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %183 = load i8*, i8** %182, align 8
  %184 = getelementptr inbounds i8, i8* %183, i64 8
  store i8* %184, i8** %182, align 8
  %185 = bitcast i8* %183 to i64*
  br label %186

186:                                              ; preds = %181, %176
  %187 = phi i64* [ %180, %176 ], [ %185, %181 ]
  %188 = load i64, i64* %187, align 8
  %189 = load %union.jvalue*, %union.jvalue** %10, align 8
  %190 = load i32, i32* %13, align 4
  %191 = sext i32 %190 to i64
  %192 = getelementptr inbounds %union.jvalue, %union.jvalue* %189, i64 %191
  %193 = bitcast %union.jvalue* %192 to i64*
  store i64 %188, i64* %193, align 8
  br label %271

194:                                              ; preds = %33
  %195 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 4
  %196 = load i32, i32* %195, align 4
  %197 = icmp sge i32 %196, 0
  br i1 %197, label %206, label %198

198:                                              ; preds = %194
  %199 = add i32 %196, 16
  store i32 %199, i32* %195, align 4
  %200 = icmp sle i32 %199, 0
  br i1 %200, label %201, label %206

201:                                              ; preds = %198
  %202 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 2
  %203 = load i8*, i8** %202, align 8
  %204 = getelementptr inbounds i8, i8* %203, i32 %196
  %205 = bitcast i8* %204 to double*
  br label %211

206:                                              ; preds = %198, %194
  %207 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %208 = load i8*, i8** %207, align 8
  %209 = getelementptr inbounds i8, i8* %208, i64 8
  store i8* %209, i8** %207, align 8
  %210 = bitcast i8* %208 to double*
  br label %211

211:                                              ; preds = %206, %201
  %212 = phi double* [ %205, %201 ], [ %210, %206 ]
  %213 = load double, double* %212, align 8
  %214 = load %union.jvalue*, %union.jvalue** %10, align 8
  %215 = load i32, i32* %13, align 4
  %216 = sext i32 %215 to i64
  %217 = getelementptr inbounds %union.jvalue, %union.jvalue* %214, i64 %216
  %218 = bitcast %union.jvalue* %217 to double*
  store double %213, double* %218, align 8
  br label %271

219:                                              ; preds = %33
  %220 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 4
  %221 = load i32, i32* %220, align 4
  %222 = icmp sge i32 %221, 0
  br i1 %222, label %231, label %223

223:                                              ; preds = %219
  %224 = add i32 %221, 16
  store i32 %224, i32* %220, align 4
  %225 = icmp sle i32 %224, 0
  br i1 %225, label %226, label %231

226:                                              ; preds = %223
  %227 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 2
  %228 = load i8*, i8** %227, align 8
  %229 = getelementptr inbounds i8, i8* %228, i32 %221
  %230 = bitcast i8* %229 to double*
  br label %236

231:                                              ; preds = %223, %219
  %232 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %233 = load i8*, i8** %232, align 8
  %234 = getelementptr inbounds i8, i8* %233, i64 8
  store i8* %234, i8** %232, align 8
  %235 = bitcast i8* %233 to double*
  br label %236

236:                                              ; preds = %231, %226
  %237 = phi double* [ %230, %226 ], [ %235, %231 ]
  %238 = load double, double* %237, align 8
  %239 = fptrunc double %238 to float
  %240 = load %union.jvalue*, %union.jvalue** %10, align 8
  %241 = load i32, i32* %13, align 4
  %242 = sext i32 %241 to i64
  %243 = getelementptr inbounds %union.jvalue, %union.jvalue* %240, i64 %242
  %244 = bitcast %union.jvalue* %243 to float*
  store float %239, float* %244, align 8
  br label %271

245:                                              ; preds = %33
  %246 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %247 = load i32, i32* %246, align 8
  %248 = icmp sge i32 %247, 0
  br i1 %248, label %257, label %249

249:                                              ; preds = %245
  %250 = add i32 %247, 8
  store i32 %250, i32* %246, align 8
  %251 = icmp sle i32 %250, 0
  br i1 %251, label %252, label %257

252:                                              ; preds = %249
  %253 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %254 = load i8*, i8** %253, align 8
  %255 = getelementptr inbounds i8, i8* %254, i32 %247
  %256 = bitcast i8* %255 to i8**
  br label %262

257:                                              ; preds = %249, %245
  %258 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %259 = load i8*, i8** %258, align 8
  %260 = getelementptr inbounds i8, i8* %259, i64 8
  store i8* %260, i8** %258, align 8
  %261 = bitcast i8* %259 to i8**
  br label %262

262:                                              ; preds = %257, %252
  %263 = phi i8** [ %256, %252 ], [ %261, %257 ]
  %264 = load i8*, i8** %263, align 8
  %265 = bitcast i8* %264 to %struct._jobject*
  %266 = load %union.jvalue*, %union.jvalue** %10, align 8
  %267 = load i32, i32* %13, align 4
  %268 = sext i32 %267 to i64
  %269 = getelementptr inbounds %union.jvalue, %union.jvalue* %266, i64 %268
  %270 = bitcast %union.jvalue* %269 to %struct._jobject**
  store %struct._jobject* %265, %struct._jobject** %270, align 8
  br label %271

271:                                              ; preds = %33, %262, %236, %211, %186, %161, %134, %108, %82, %56
  br label %272

272:                                              ; preds = %271
  %273 = load i32, i32* %13, align 4
  %274 = add nsw i32 %273, 1
  store i32 %274, i32* %13, align 4
  br label %29, !llvm.loop !24

275:                                              ; preds = %29
  br label %276

276:                                              ; preds = %275
  %277 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %278 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %277, align 8
  %279 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %278, i32 0, i32 78
  %280 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %279, align 8
  %281 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %282 = load %struct._jobject*, %struct._jobject** %7, align 8
  %283 = load %struct._jobject*, %struct._jobject** %8, align 8
  %284 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %285 = load %union.jvalue*, %union.jvalue** %10, align 8
  %286 = call i16 %280(%struct.JNINativeInterface_** noundef %281, %struct._jobject* noundef %282, %struct._jobject* noundef %283, %struct._jmethodID* noundef %284, %union.jvalue* noundef %285)
  ret i16 %286
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i16 @JNI_CallStaticShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i16, align 2
  %8 = alloca %"struct.std::__va_list", align 8
  %9 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %10 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 127
  %14 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = bitcast %"struct.std::__va_list"* %9 to i8*
  %19 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %18, i8* align 8 %19, i64 32, i1 false)
  %20 = call i16 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %"struct.std::__va_list"* noundef %9)
  store i16 %20, i16* %7, align 2
  %21 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i16, i16* %7, align 2
  ret i16 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i16 @JNI_CallStaticShortMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !25

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 128
  %278 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  %283 = call i16 %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret i16 %283
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @JNI_CallIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i32, align 4
  %8 = alloca %"struct.std::__va_list", align 8
  %9 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %10 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 50
  %14 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = bitcast %"struct.std::__va_list"* %9 to i8*
  %19 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %18, i8* align 8 %19, i64 32, i1 false)
  %20 = call i32 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %"struct.std::__va_list"* noundef %9)
  store i32 %20, i32* %7, align 4
  %21 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i32, i32* %7, align 4
  ret i32 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @JNI_CallIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !26

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 51
  %278 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  %283 = call i32 %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret i32 %283
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @JNI_CallNonvirtualIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i32, align 4
  %10 = alloca %"struct.std::__va_list", align 8
  %11 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %12 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.va_start(i8* %12)
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 80
  %16 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %15, align 8
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %18 = load %struct._jobject*, %struct._jobject** %6, align 8
  %19 = load %struct._jobject*, %struct._jobject** %7, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %21 = bitcast %"struct.std::__va_list"* %11 to i8*
  %22 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %21, i8* align 8 %22, i64 32, i1 false)
  %23 = call i32 %16(%struct.JNINativeInterface_** noundef %17, %struct._jobject* noundef %18, %struct._jobject* noundef %19, %struct._jmethodID* noundef %20, %"struct.std::__va_list"* noundef %11)
  store i32 %23, i32* %9, align 4
  %24 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load i32, i32* %9, align 4
  ret i32 %25
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @JNI_CallNonvirtualIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca %union.jvalue*, align 8
  %11 = alloca [257 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  br label %14

14:                                               ; preds = %5
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 8
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 8
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %21 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i64 0, i64 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %12, align 4
  %24 = load i32, i32* %12, align 4
  %25 = sext i32 %24 to i64
  %26 = mul i64 %25, 8
  %27 = alloca i8, i64 %26, align 16
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %10, align 8
  store i32 0, i32* %13, align 4
  br label %29

29:                                               ; preds = %272, %14
  %30 = load i32, i32* %13, align 4
  %31 = load i32, i32* %12, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %275

33:                                               ; preds = %29
  %34 = load i32, i32* %13, align 4
  %35 = sext i32 %34 to i64
  %36 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i64 0, i64 %35
  %37 = load i8, i8* %36, align 1
  %38 = zext i8 %37 to i32
  switch i32 %38, label %271 [
    i32 90, label %39
    i32 66, label %65
    i32 83, label %91
    i32 67, label %117
    i32 73, label %144
    i32 74, label %169
    i32 68, label %194
    i32 70, label %219
    i32 76, label %245
  ]

39:                                               ; preds = %33
  %40 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %41 = load i32, i32* %40, align 8
  %42 = icmp sge i32 %41, 0
  br i1 %42, label %51, label %43

43:                                               ; preds = %39
  %44 = add i32 %41, 8
  store i32 %44, i32* %40, align 8
  %45 = icmp sle i32 %44, 0
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %48 = load i8*, i8** %47, align 8
  %49 = getelementptr inbounds i8, i8* %48, i32 %41
  %50 = bitcast i8* %49 to i32*
  br label %56

51:                                               ; preds = %43, %39
  %52 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %53 = load i8*, i8** %52, align 8
  %54 = getelementptr inbounds i8, i8* %53, i64 8
  store i8* %54, i8** %52, align 8
  %55 = bitcast i8* %53 to i32*
  br label %56

56:                                               ; preds = %51, %46
  %57 = phi i32* [ %50, %46 ], [ %55, %51 ]
  %58 = load i32, i32* %57, align 8
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %10, align 8
  %61 = load i32, i32* %13, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %59, i8* %64, align 8
  br label %271

65:                                               ; preds = %33
  %66 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %67 = load i32, i32* %66, align 8
  %68 = icmp sge i32 %67, 0
  br i1 %68, label %77, label %69

69:                                               ; preds = %65
  %70 = add i32 %67, 8
  store i32 %70, i32* %66, align 8
  %71 = icmp sle i32 %70, 0
  br i1 %71, label %72, label %77

72:                                               ; preds = %69
  %73 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %74 = load i8*, i8** %73, align 8
  %75 = getelementptr inbounds i8, i8* %74, i32 %67
  %76 = bitcast i8* %75 to i32*
  br label %82

77:                                               ; preds = %69, %65
  %78 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %79 = load i8*, i8** %78, align 8
  %80 = getelementptr inbounds i8, i8* %79, i64 8
  store i8* %80, i8** %78, align 8
  %81 = bitcast i8* %79 to i32*
  br label %82

82:                                               ; preds = %77, %72
  %83 = phi i32* [ %76, %72 ], [ %81, %77 ]
  %84 = load i32, i32* %83, align 8
  %85 = trunc i32 %84 to i8
  %86 = load %union.jvalue*, %union.jvalue** %10, align 8
  %87 = load i32, i32* %13, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, %union.jvalue* %86, i64 %88
  %90 = bitcast %union.jvalue* %89 to i8*
  store i8 %85, i8* %90, align 8
  br label %271

91:                                               ; preds = %33
  %92 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %93 = load i32, i32* %92, align 8
  %94 = icmp sge i32 %93, 0
  br i1 %94, label %103, label %95

95:                                               ; preds = %91
  %96 = add i32 %93, 8
  store i32 %96, i32* %92, align 8
  %97 = icmp sle i32 %96, 0
  br i1 %97, label %98, label %103

98:                                               ; preds = %95
  %99 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %100 = load i8*, i8** %99, align 8
  %101 = getelementptr inbounds i8, i8* %100, i32 %93
  %102 = bitcast i8* %101 to i32*
  br label %108

103:                                              ; preds = %95, %91
  %104 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %105 = load i8*, i8** %104, align 8
  %106 = getelementptr inbounds i8, i8* %105, i64 8
  store i8* %106, i8** %104, align 8
  %107 = bitcast i8* %105 to i32*
  br label %108

108:                                              ; preds = %103, %98
  %109 = phi i32* [ %102, %98 ], [ %107, %103 ]
  %110 = load i32, i32* %109, align 8
  %111 = trunc i32 %110 to i16
  %112 = load %union.jvalue*, %union.jvalue** %10, align 8
  %113 = load i32, i32* %13, align 4
  %114 = sext i32 %113 to i64
  %115 = getelementptr inbounds %union.jvalue, %union.jvalue* %112, i64 %114
  %116 = bitcast %union.jvalue* %115 to i16*
  store i16 %111, i16* %116, align 8
  br label %271

117:                                              ; preds = %33
  %118 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %119 = load i32, i32* %118, align 8
  %120 = icmp sge i32 %119, 0
  br i1 %120, label %129, label %121

121:                                              ; preds = %117
  %122 = add i32 %119, 8
  store i32 %122, i32* %118, align 8
  %123 = icmp sle i32 %122, 0
  br i1 %123, label %124, label %129

124:                                              ; preds = %121
  %125 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %126 = load i8*, i8** %125, align 8
  %127 = getelementptr inbounds i8, i8* %126, i32 %119
  %128 = bitcast i8* %127 to i32*
  br label %134

129:                                              ; preds = %121, %117
  %130 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %131 = load i8*, i8** %130, align 8
  %132 = getelementptr inbounds i8, i8* %131, i64 8
  store i8* %132, i8** %130, align 8
  %133 = bitcast i8* %131 to i32*
  br label %134

134:                                              ; preds = %129, %124
  %135 = phi i32* [ %128, %124 ], [ %133, %129 ]
  %136 = load i32, i32* %135, align 8
  %137 = trunc i32 %136 to i16
  %138 = zext i16 %137 to i32
  %139 = load %union.jvalue*, %union.jvalue** %10, align 8
  %140 = load i32, i32* %13, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %139, i64 %141
  %143 = bitcast %union.jvalue* %142 to i32*
  store i32 %138, i32* %143, align 8
  br label %271

144:                                              ; preds = %33
  %145 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %146 = load i32, i32* %145, align 8
  %147 = icmp sge i32 %146, 0
  br i1 %147, label %156, label %148

148:                                              ; preds = %144
  %149 = add i32 %146, 8
  store i32 %149, i32* %145, align 8
  %150 = icmp sle i32 %149, 0
  br i1 %150, label %151, label %156

151:                                              ; preds = %148
  %152 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %153 = load i8*, i8** %152, align 8
  %154 = getelementptr inbounds i8, i8* %153, i32 %146
  %155 = bitcast i8* %154 to i32*
  br label %161

156:                                              ; preds = %148, %144
  %157 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %158 = load i8*, i8** %157, align 8
  %159 = getelementptr inbounds i8, i8* %158, i64 8
  store i8* %159, i8** %157, align 8
  %160 = bitcast i8* %158 to i32*
  br label %161

161:                                              ; preds = %156, %151
  %162 = phi i32* [ %155, %151 ], [ %160, %156 ]
  %163 = load i32, i32* %162, align 8
  %164 = load %union.jvalue*, %union.jvalue** %10, align 8
  %165 = load i32, i32* %13, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds %union.jvalue, %union.jvalue* %164, i64 %166
  %168 = bitcast %union.jvalue* %167 to i32*
  store i32 %163, i32* %168, align 8
  br label %271

169:                                              ; preds = %33
  %170 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %171 = load i32, i32* %170, align 8
  %172 = icmp sge i32 %171, 0
  br i1 %172, label %181, label %173

173:                                              ; preds = %169
  %174 = add i32 %171, 8
  store i32 %174, i32* %170, align 8
  %175 = icmp sle i32 %174, 0
  br i1 %175, label %176, label %181

176:                                              ; preds = %173
  %177 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %178 = load i8*, i8** %177, align 8
  %179 = getelementptr inbounds i8, i8* %178, i32 %171
  %180 = bitcast i8* %179 to i64*
  br label %186

181:                                              ; preds = %173, %169
  %182 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %183 = load i8*, i8** %182, align 8
  %184 = getelementptr inbounds i8, i8* %183, i64 8
  store i8* %184, i8** %182, align 8
  %185 = bitcast i8* %183 to i64*
  br label %186

186:                                              ; preds = %181, %176
  %187 = phi i64* [ %180, %176 ], [ %185, %181 ]
  %188 = load i64, i64* %187, align 8
  %189 = load %union.jvalue*, %union.jvalue** %10, align 8
  %190 = load i32, i32* %13, align 4
  %191 = sext i32 %190 to i64
  %192 = getelementptr inbounds %union.jvalue, %union.jvalue* %189, i64 %191
  %193 = bitcast %union.jvalue* %192 to i64*
  store i64 %188, i64* %193, align 8
  br label %271

194:                                              ; preds = %33
  %195 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 4
  %196 = load i32, i32* %195, align 4
  %197 = icmp sge i32 %196, 0
  br i1 %197, label %206, label %198

198:                                              ; preds = %194
  %199 = add i32 %196, 16
  store i32 %199, i32* %195, align 4
  %200 = icmp sle i32 %199, 0
  br i1 %200, label %201, label %206

201:                                              ; preds = %198
  %202 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 2
  %203 = load i8*, i8** %202, align 8
  %204 = getelementptr inbounds i8, i8* %203, i32 %196
  %205 = bitcast i8* %204 to double*
  br label %211

206:                                              ; preds = %198, %194
  %207 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %208 = load i8*, i8** %207, align 8
  %209 = getelementptr inbounds i8, i8* %208, i64 8
  store i8* %209, i8** %207, align 8
  %210 = bitcast i8* %208 to double*
  br label %211

211:                                              ; preds = %206, %201
  %212 = phi double* [ %205, %201 ], [ %210, %206 ]
  %213 = load double, double* %212, align 8
  %214 = load %union.jvalue*, %union.jvalue** %10, align 8
  %215 = load i32, i32* %13, align 4
  %216 = sext i32 %215 to i64
  %217 = getelementptr inbounds %union.jvalue, %union.jvalue* %214, i64 %216
  %218 = bitcast %union.jvalue* %217 to double*
  store double %213, double* %218, align 8
  br label %271

219:                                              ; preds = %33
  %220 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 4
  %221 = load i32, i32* %220, align 4
  %222 = icmp sge i32 %221, 0
  br i1 %222, label %231, label %223

223:                                              ; preds = %219
  %224 = add i32 %221, 16
  store i32 %224, i32* %220, align 4
  %225 = icmp sle i32 %224, 0
  br i1 %225, label %226, label %231

226:                                              ; preds = %223
  %227 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 2
  %228 = load i8*, i8** %227, align 8
  %229 = getelementptr inbounds i8, i8* %228, i32 %221
  %230 = bitcast i8* %229 to double*
  br label %236

231:                                              ; preds = %223, %219
  %232 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %233 = load i8*, i8** %232, align 8
  %234 = getelementptr inbounds i8, i8* %233, i64 8
  store i8* %234, i8** %232, align 8
  %235 = bitcast i8* %233 to double*
  br label %236

236:                                              ; preds = %231, %226
  %237 = phi double* [ %230, %226 ], [ %235, %231 ]
  %238 = load double, double* %237, align 8
  %239 = fptrunc double %238 to float
  %240 = load %union.jvalue*, %union.jvalue** %10, align 8
  %241 = load i32, i32* %13, align 4
  %242 = sext i32 %241 to i64
  %243 = getelementptr inbounds %union.jvalue, %union.jvalue* %240, i64 %242
  %244 = bitcast %union.jvalue* %243 to float*
  store float %239, float* %244, align 8
  br label %271

245:                                              ; preds = %33
  %246 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %247 = load i32, i32* %246, align 8
  %248 = icmp sge i32 %247, 0
  br i1 %248, label %257, label %249

249:                                              ; preds = %245
  %250 = add i32 %247, 8
  store i32 %250, i32* %246, align 8
  %251 = icmp sle i32 %250, 0
  br i1 %251, label %252, label %257

252:                                              ; preds = %249
  %253 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %254 = load i8*, i8** %253, align 8
  %255 = getelementptr inbounds i8, i8* %254, i32 %247
  %256 = bitcast i8* %255 to i8**
  br label %262

257:                                              ; preds = %249, %245
  %258 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %259 = load i8*, i8** %258, align 8
  %260 = getelementptr inbounds i8, i8* %259, i64 8
  store i8* %260, i8** %258, align 8
  %261 = bitcast i8* %259 to i8**
  br label %262

262:                                              ; preds = %257, %252
  %263 = phi i8** [ %256, %252 ], [ %261, %257 ]
  %264 = load i8*, i8** %263, align 8
  %265 = bitcast i8* %264 to %struct._jobject*
  %266 = load %union.jvalue*, %union.jvalue** %10, align 8
  %267 = load i32, i32* %13, align 4
  %268 = sext i32 %267 to i64
  %269 = getelementptr inbounds %union.jvalue, %union.jvalue* %266, i64 %268
  %270 = bitcast %union.jvalue* %269 to %struct._jobject**
  store %struct._jobject* %265, %struct._jobject** %270, align 8
  br label %271

271:                                              ; preds = %33, %262, %236, %211, %186, %161, %134, %108, %82, %56
  br label %272

272:                                              ; preds = %271
  %273 = load i32, i32* %13, align 4
  %274 = add nsw i32 %273, 1
  store i32 %274, i32* %13, align 4
  br label %29, !llvm.loop !27

275:                                              ; preds = %29
  br label %276

276:                                              ; preds = %275
  %277 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %278 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %277, align 8
  %279 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %278, i32 0, i32 81
  %280 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %279, align 8
  %281 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %282 = load %struct._jobject*, %struct._jobject** %7, align 8
  %283 = load %struct._jobject*, %struct._jobject** %8, align 8
  %284 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %285 = load %union.jvalue*, %union.jvalue** %10, align 8
  %286 = call i32 %280(%struct.JNINativeInterface_** noundef %281, %struct._jobject* noundef %282, %struct._jobject* noundef %283, %struct._jmethodID* noundef %284, %union.jvalue* noundef %285)
  ret i32 %286
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @JNI_CallStaticIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i32, align 4
  %8 = alloca %"struct.std::__va_list", align 8
  %9 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %10 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 130
  %14 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = bitcast %"struct.std::__va_list"* %9 to i8*
  %19 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %18, i8* align 8 %19, i64 32, i1 false)
  %20 = call i32 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %"struct.std::__va_list"* noundef %9)
  store i32 %20, i32* %7, align 4
  %21 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i32, i32* %7, align 4
  ret i32 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @JNI_CallStaticIntMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !28

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 131
  %278 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  %283 = call i32 %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret i32 %283
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i64 @JNI_CallLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i64, align 8
  %8 = alloca %"struct.std::__va_list", align 8
  %9 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %10 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 53
  %14 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = bitcast %"struct.std::__va_list"* %9 to i8*
  %19 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %18, i8* align 8 %19, i64 32, i1 false)
  %20 = call i64 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %"struct.std::__va_list"* noundef %9)
  store i64 %20, i64* %7, align 8
  %21 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i64, i64* %7, align 8
  ret i64 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i64 @JNI_CallLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !29

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 54
  %278 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  %283 = call i64 %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret i64 %283
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i64 @JNI_CallNonvirtualLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca i64, align 8
  %10 = alloca %"struct.std::__va_list", align 8
  %11 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %12 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.va_start(i8* %12)
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 83
  %16 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %15, align 8
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %18 = load %struct._jobject*, %struct._jobject** %6, align 8
  %19 = load %struct._jobject*, %struct._jobject** %7, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %21 = bitcast %"struct.std::__va_list"* %11 to i8*
  %22 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %21, i8* align 8 %22, i64 32, i1 false)
  %23 = call i64 %16(%struct.JNINativeInterface_** noundef %17, %struct._jobject* noundef %18, %struct._jobject* noundef %19, %struct._jmethodID* noundef %20, %"struct.std::__va_list"* noundef %11)
  store i64 %23, i64* %9, align 8
  %24 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load i64, i64* %9, align 8
  ret i64 %25
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i64 @JNI_CallNonvirtualLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca %union.jvalue*, align 8
  %11 = alloca [257 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  br label %14

14:                                               ; preds = %5
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 8
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 8
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %21 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i64 0, i64 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %12, align 4
  %24 = load i32, i32* %12, align 4
  %25 = sext i32 %24 to i64
  %26 = mul i64 %25, 8
  %27 = alloca i8, i64 %26, align 16
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %10, align 8
  store i32 0, i32* %13, align 4
  br label %29

29:                                               ; preds = %272, %14
  %30 = load i32, i32* %13, align 4
  %31 = load i32, i32* %12, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %275

33:                                               ; preds = %29
  %34 = load i32, i32* %13, align 4
  %35 = sext i32 %34 to i64
  %36 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i64 0, i64 %35
  %37 = load i8, i8* %36, align 1
  %38 = zext i8 %37 to i32
  switch i32 %38, label %271 [
    i32 90, label %39
    i32 66, label %65
    i32 83, label %91
    i32 67, label %117
    i32 73, label %144
    i32 74, label %169
    i32 68, label %194
    i32 70, label %219
    i32 76, label %245
  ]

39:                                               ; preds = %33
  %40 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %41 = load i32, i32* %40, align 8
  %42 = icmp sge i32 %41, 0
  br i1 %42, label %51, label %43

43:                                               ; preds = %39
  %44 = add i32 %41, 8
  store i32 %44, i32* %40, align 8
  %45 = icmp sle i32 %44, 0
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %48 = load i8*, i8** %47, align 8
  %49 = getelementptr inbounds i8, i8* %48, i32 %41
  %50 = bitcast i8* %49 to i32*
  br label %56

51:                                               ; preds = %43, %39
  %52 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %53 = load i8*, i8** %52, align 8
  %54 = getelementptr inbounds i8, i8* %53, i64 8
  store i8* %54, i8** %52, align 8
  %55 = bitcast i8* %53 to i32*
  br label %56

56:                                               ; preds = %51, %46
  %57 = phi i32* [ %50, %46 ], [ %55, %51 ]
  %58 = load i32, i32* %57, align 8
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %10, align 8
  %61 = load i32, i32* %13, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %59, i8* %64, align 8
  br label %271

65:                                               ; preds = %33
  %66 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %67 = load i32, i32* %66, align 8
  %68 = icmp sge i32 %67, 0
  br i1 %68, label %77, label %69

69:                                               ; preds = %65
  %70 = add i32 %67, 8
  store i32 %70, i32* %66, align 8
  %71 = icmp sle i32 %70, 0
  br i1 %71, label %72, label %77

72:                                               ; preds = %69
  %73 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %74 = load i8*, i8** %73, align 8
  %75 = getelementptr inbounds i8, i8* %74, i32 %67
  %76 = bitcast i8* %75 to i32*
  br label %82

77:                                               ; preds = %69, %65
  %78 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %79 = load i8*, i8** %78, align 8
  %80 = getelementptr inbounds i8, i8* %79, i64 8
  store i8* %80, i8** %78, align 8
  %81 = bitcast i8* %79 to i32*
  br label %82

82:                                               ; preds = %77, %72
  %83 = phi i32* [ %76, %72 ], [ %81, %77 ]
  %84 = load i32, i32* %83, align 8
  %85 = trunc i32 %84 to i8
  %86 = load %union.jvalue*, %union.jvalue** %10, align 8
  %87 = load i32, i32* %13, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, %union.jvalue* %86, i64 %88
  %90 = bitcast %union.jvalue* %89 to i8*
  store i8 %85, i8* %90, align 8
  br label %271

91:                                               ; preds = %33
  %92 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %93 = load i32, i32* %92, align 8
  %94 = icmp sge i32 %93, 0
  br i1 %94, label %103, label %95

95:                                               ; preds = %91
  %96 = add i32 %93, 8
  store i32 %96, i32* %92, align 8
  %97 = icmp sle i32 %96, 0
  br i1 %97, label %98, label %103

98:                                               ; preds = %95
  %99 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %100 = load i8*, i8** %99, align 8
  %101 = getelementptr inbounds i8, i8* %100, i32 %93
  %102 = bitcast i8* %101 to i32*
  br label %108

103:                                              ; preds = %95, %91
  %104 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %105 = load i8*, i8** %104, align 8
  %106 = getelementptr inbounds i8, i8* %105, i64 8
  store i8* %106, i8** %104, align 8
  %107 = bitcast i8* %105 to i32*
  br label %108

108:                                              ; preds = %103, %98
  %109 = phi i32* [ %102, %98 ], [ %107, %103 ]
  %110 = load i32, i32* %109, align 8
  %111 = trunc i32 %110 to i16
  %112 = load %union.jvalue*, %union.jvalue** %10, align 8
  %113 = load i32, i32* %13, align 4
  %114 = sext i32 %113 to i64
  %115 = getelementptr inbounds %union.jvalue, %union.jvalue* %112, i64 %114
  %116 = bitcast %union.jvalue* %115 to i16*
  store i16 %111, i16* %116, align 8
  br label %271

117:                                              ; preds = %33
  %118 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %119 = load i32, i32* %118, align 8
  %120 = icmp sge i32 %119, 0
  br i1 %120, label %129, label %121

121:                                              ; preds = %117
  %122 = add i32 %119, 8
  store i32 %122, i32* %118, align 8
  %123 = icmp sle i32 %122, 0
  br i1 %123, label %124, label %129

124:                                              ; preds = %121
  %125 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %126 = load i8*, i8** %125, align 8
  %127 = getelementptr inbounds i8, i8* %126, i32 %119
  %128 = bitcast i8* %127 to i32*
  br label %134

129:                                              ; preds = %121, %117
  %130 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %131 = load i8*, i8** %130, align 8
  %132 = getelementptr inbounds i8, i8* %131, i64 8
  store i8* %132, i8** %130, align 8
  %133 = bitcast i8* %131 to i32*
  br label %134

134:                                              ; preds = %129, %124
  %135 = phi i32* [ %128, %124 ], [ %133, %129 ]
  %136 = load i32, i32* %135, align 8
  %137 = trunc i32 %136 to i16
  %138 = zext i16 %137 to i32
  %139 = load %union.jvalue*, %union.jvalue** %10, align 8
  %140 = load i32, i32* %13, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %139, i64 %141
  %143 = bitcast %union.jvalue* %142 to i32*
  store i32 %138, i32* %143, align 8
  br label %271

144:                                              ; preds = %33
  %145 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %146 = load i32, i32* %145, align 8
  %147 = icmp sge i32 %146, 0
  br i1 %147, label %156, label %148

148:                                              ; preds = %144
  %149 = add i32 %146, 8
  store i32 %149, i32* %145, align 8
  %150 = icmp sle i32 %149, 0
  br i1 %150, label %151, label %156

151:                                              ; preds = %148
  %152 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %153 = load i8*, i8** %152, align 8
  %154 = getelementptr inbounds i8, i8* %153, i32 %146
  %155 = bitcast i8* %154 to i32*
  br label %161

156:                                              ; preds = %148, %144
  %157 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %158 = load i8*, i8** %157, align 8
  %159 = getelementptr inbounds i8, i8* %158, i64 8
  store i8* %159, i8** %157, align 8
  %160 = bitcast i8* %158 to i32*
  br label %161

161:                                              ; preds = %156, %151
  %162 = phi i32* [ %155, %151 ], [ %160, %156 ]
  %163 = load i32, i32* %162, align 8
  %164 = load %union.jvalue*, %union.jvalue** %10, align 8
  %165 = load i32, i32* %13, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds %union.jvalue, %union.jvalue* %164, i64 %166
  %168 = bitcast %union.jvalue* %167 to i32*
  store i32 %163, i32* %168, align 8
  br label %271

169:                                              ; preds = %33
  %170 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %171 = load i32, i32* %170, align 8
  %172 = icmp sge i32 %171, 0
  br i1 %172, label %181, label %173

173:                                              ; preds = %169
  %174 = add i32 %171, 8
  store i32 %174, i32* %170, align 8
  %175 = icmp sle i32 %174, 0
  br i1 %175, label %176, label %181

176:                                              ; preds = %173
  %177 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %178 = load i8*, i8** %177, align 8
  %179 = getelementptr inbounds i8, i8* %178, i32 %171
  %180 = bitcast i8* %179 to i64*
  br label %186

181:                                              ; preds = %173, %169
  %182 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %183 = load i8*, i8** %182, align 8
  %184 = getelementptr inbounds i8, i8* %183, i64 8
  store i8* %184, i8** %182, align 8
  %185 = bitcast i8* %183 to i64*
  br label %186

186:                                              ; preds = %181, %176
  %187 = phi i64* [ %180, %176 ], [ %185, %181 ]
  %188 = load i64, i64* %187, align 8
  %189 = load %union.jvalue*, %union.jvalue** %10, align 8
  %190 = load i32, i32* %13, align 4
  %191 = sext i32 %190 to i64
  %192 = getelementptr inbounds %union.jvalue, %union.jvalue* %189, i64 %191
  %193 = bitcast %union.jvalue* %192 to i64*
  store i64 %188, i64* %193, align 8
  br label %271

194:                                              ; preds = %33
  %195 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 4
  %196 = load i32, i32* %195, align 4
  %197 = icmp sge i32 %196, 0
  br i1 %197, label %206, label %198

198:                                              ; preds = %194
  %199 = add i32 %196, 16
  store i32 %199, i32* %195, align 4
  %200 = icmp sle i32 %199, 0
  br i1 %200, label %201, label %206

201:                                              ; preds = %198
  %202 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 2
  %203 = load i8*, i8** %202, align 8
  %204 = getelementptr inbounds i8, i8* %203, i32 %196
  %205 = bitcast i8* %204 to double*
  br label %211

206:                                              ; preds = %198, %194
  %207 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %208 = load i8*, i8** %207, align 8
  %209 = getelementptr inbounds i8, i8* %208, i64 8
  store i8* %209, i8** %207, align 8
  %210 = bitcast i8* %208 to double*
  br label %211

211:                                              ; preds = %206, %201
  %212 = phi double* [ %205, %201 ], [ %210, %206 ]
  %213 = load double, double* %212, align 8
  %214 = load %union.jvalue*, %union.jvalue** %10, align 8
  %215 = load i32, i32* %13, align 4
  %216 = sext i32 %215 to i64
  %217 = getelementptr inbounds %union.jvalue, %union.jvalue* %214, i64 %216
  %218 = bitcast %union.jvalue* %217 to double*
  store double %213, double* %218, align 8
  br label %271

219:                                              ; preds = %33
  %220 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 4
  %221 = load i32, i32* %220, align 4
  %222 = icmp sge i32 %221, 0
  br i1 %222, label %231, label %223

223:                                              ; preds = %219
  %224 = add i32 %221, 16
  store i32 %224, i32* %220, align 4
  %225 = icmp sle i32 %224, 0
  br i1 %225, label %226, label %231

226:                                              ; preds = %223
  %227 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 2
  %228 = load i8*, i8** %227, align 8
  %229 = getelementptr inbounds i8, i8* %228, i32 %221
  %230 = bitcast i8* %229 to double*
  br label %236

231:                                              ; preds = %223, %219
  %232 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %233 = load i8*, i8** %232, align 8
  %234 = getelementptr inbounds i8, i8* %233, i64 8
  store i8* %234, i8** %232, align 8
  %235 = bitcast i8* %233 to double*
  br label %236

236:                                              ; preds = %231, %226
  %237 = phi double* [ %230, %226 ], [ %235, %231 ]
  %238 = load double, double* %237, align 8
  %239 = fptrunc double %238 to float
  %240 = load %union.jvalue*, %union.jvalue** %10, align 8
  %241 = load i32, i32* %13, align 4
  %242 = sext i32 %241 to i64
  %243 = getelementptr inbounds %union.jvalue, %union.jvalue* %240, i64 %242
  %244 = bitcast %union.jvalue* %243 to float*
  store float %239, float* %244, align 8
  br label %271

245:                                              ; preds = %33
  %246 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %247 = load i32, i32* %246, align 8
  %248 = icmp sge i32 %247, 0
  br i1 %248, label %257, label %249

249:                                              ; preds = %245
  %250 = add i32 %247, 8
  store i32 %250, i32* %246, align 8
  %251 = icmp sle i32 %250, 0
  br i1 %251, label %252, label %257

252:                                              ; preds = %249
  %253 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %254 = load i8*, i8** %253, align 8
  %255 = getelementptr inbounds i8, i8* %254, i32 %247
  %256 = bitcast i8* %255 to i8**
  br label %262

257:                                              ; preds = %249, %245
  %258 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %259 = load i8*, i8** %258, align 8
  %260 = getelementptr inbounds i8, i8* %259, i64 8
  store i8* %260, i8** %258, align 8
  %261 = bitcast i8* %259 to i8**
  br label %262

262:                                              ; preds = %257, %252
  %263 = phi i8** [ %256, %252 ], [ %261, %257 ]
  %264 = load i8*, i8** %263, align 8
  %265 = bitcast i8* %264 to %struct._jobject*
  %266 = load %union.jvalue*, %union.jvalue** %10, align 8
  %267 = load i32, i32* %13, align 4
  %268 = sext i32 %267 to i64
  %269 = getelementptr inbounds %union.jvalue, %union.jvalue* %266, i64 %268
  %270 = bitcast %union.jvalue* %269 to %struct._jobject**
  store %struct._jobject* %265, %struct._jobject** %270, align 8
  br label %271

271:                                              ; preds = %33, %262, %236, %211, %186, %161, %134, %108, %82, %56
  br label %272

272:                                              ; preds = %271
  %273 = load i32, i32* %13, align 4
  %274 = add nsw i32 %273, 1
  store i32 %274, i32* %13, align 4
  br label %29, !llvm.loop !30

275:                                              ; preds = %29
  br label %276

276:                                              ; preds = %275
  %277 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %278 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %277, align 8
  %279 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %278, i32 0, i32 84
  %280 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %279, align 8
  %281 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %282 = load %struct._jobject*, %struct._jobject** %7, align 8
  %283 = load %struct._jobject*, %struct._jobject** %8, align 8
  %284 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %285 = load %union.jvalue*, %union.jvalue** %10, align 8
  %286 = call i64 %280(%struct.JNINativeInterface_** noundef %281, %struct._jobject* noundef %282, %struct._jobject* noundef %283, %struct._jmethodID* noundef %284, %union.jvalue* noundef %285)
  ret i64 %286
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i64 @JNI_CallStaticLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca i64, align 8
  %8 = alloca %"struct.std::__va_list", align 8
  %9 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %10 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 133
  %14 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = bitcast %"struct.std::__va_list"* %9 to i8*
  %19 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %18, i8* align 8 %19, i64 32, i1 false)
  %20 = call i64 %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %"struct.std::__va_list"* noundef %9)
  store i64 %20, i64* %7, align 8
  %21 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i64, i64* %7, align 8
  ret i64 %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i64 @JNI_CallStaticLongMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !31

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 134
  %278 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  %283 = call i64 %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret i64 %283
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local float @JNI_CallFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca float, align 4
  %8 = alloca %"struct.std::__va_list", align 8
  %9 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %10 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 56
  %14 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = bitcast %"struct.std::__va_list"* %9 to i8*
  %19 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %18, i8* align 8 %19, i64 32, i1 false)
  %20 = call float %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %"struct.std::__va_list"* noundef %9)
  store float %20, float* %7, align 4
  %21 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load float, float* %7, align 4
  ret float %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local float @JNI_CallFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !32

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 57
  %278 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  %283 = call float %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret float %283
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local float @JNI_CallNonvirtualFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca float, align 4
  %10 = alloca %"struct.std::__va_list", align 8
  %11 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %12 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.va_start(i8* %12)
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 86
  %16 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %15, align 8
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %18 = load %struct._jobject*, %struct._jobject** %6, align 8
  %19 = load %struct._jobject*, %struct._jobject** %7, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %21 = bitcast %"struct.std::__va_list"* %11 to i8*
  %22 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %21, i8* align 8 %22, i64 32, i1 false)
  %23 = call float %16(%struct.JNINativeInterface_** noundef %17, %struct._jobject* noundef %18, %struct._jobject* noundef %19, %struct._jmethodID* noundef %20, %"struct.std::__va_list"* noundef %11)
  store float %23, float* %9, align 4
  %24 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load float, float* %9, align 4
  ret float %25
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local float @JNI_CallNonvirtualFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca %union.jvalue*, align 8
  %11 = alloca [257 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  br label %14

14:                                               ; preds = %5
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 8
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 8
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %21 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i64 0, i64 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %12, align 4
  %24 = load i32, i32* %12, align 4
  %25 = sext i32 %24 to i64
  %26 = mul i64 %25, 8
  %27 = alloca i8, i64 %26, align 16
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %10, align 8
  store i32 0, i32* %13, align 4
  br label %29

29:                                               ; preds = %272, %14
  %30 = load i32, i32* %13, align 4
  %31 = load i32, i32* %12, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %275

33:                                               ; preds = %29
  %34 = load i32, i32* %13, align 4
  %35 = sext i32 %34 to i64
  %36 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i64 0, i64 %35
  %37 = load i8, i8* %36, align 1
  %38 = zext i8 %37 to i32
  switch i32 %38, label %271 [
    i32 90, label %39
    i32 66, label %65
    i32 83, label %91
    i32 67, label %117
    i32 73, label %144
    i32 74, label %169
    i32 68, label %194
    i32 70, label %219
    i32 76, label %245
  ]

39:                                               ; preds = %33
  %40 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %41 = load i32, i32* %40, align 8
  %42 = icmp sge i32 %41, 0
  br i1 %42, label %51, label %43

43:                                               ; preds = %39
  %44 = add i32 %41, 8
  store i32 %44, i32* %40, align 8
  %45 = icmp sle i32 %44, 0
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %48 = load i8*, i8** %47, align 8
  %49 = getelementptr inbounds i8, i8* %48, i32 %41
  %50 = bitcast i8* %49 to i32*
  br label %56

51:                                               ; preds = %43, %39
  %52 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %53 = load i8*, i8** %52, align 8
  %54 = getelementptr inbounds i8, i8* %53, i64 8
  store i8* %54, i8** %52, align 8
  %55 = bitcast i8* %53 to i32*
  br label %56

56:                                               ; preds = %51, %46
  %57 = phi i32* [ %50, %46 ], [ %55, %51 ]
  %58 = load i32, i32* %57, align 8
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %10, align 8
  %61 = load i32, i32* %13, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %59, i8* %64, align 8
  br label %271

65:                                               ; preds = %33
  %66 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %67 = load i32, i32* %66, align 8
  %68 = icmp sge i32 %67, 0
  br i1 %68, label %77, label %69

69:                                               ; preds = %65
  %70 = add i32 %67, 8
  store i32 %70, i32* %66, align 8
  %71 = icmp sle i32 %70, 0
  br i1 %71, label %72, label %77

72:                                               ; preds = %69
  %73 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %74 = load i8*, i8** %73, align 8
  %75 = getelementptr inbounds i8, i8* %74, i32 %67
  %76 = bitcast i8* %75 to i32*
  br label %82

77:                                               ; preds = %69, %65
  %78 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %79 = load i8*, i8** %78, align 8
  %80 = getelementptr inbounds i8, i8* %79, i64 8
  store i8* %80, i8** %78, align 8
  %81 = bitcast i8* %79 to i32*
  br label %82

82:                                               ; preds = %77, %72
  %83 = phi i32* [ %76, %72 ], [ %81, %77 ]
  %84 = load i32, i32* %83, align 8
  %85 = trunc i32 %84 to i8
  %86 = load %union.jvalue*, %union.jvalue** %10, align 8
  %87 = load i32, i32* %13, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, %union.jvalue* %86, i64 %88
  %90 = bitcast %union.jvalue* %89 to i8*
  store i8 %85, i8* %90, align 8
  br label %271

91:                                               ; preds = %33
  %92 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %93 = load i32, i32* %92, align 8
  %94 = icmp sge i32 %93, 0
  br i1 %94, label %103, label %95

95:                                               ; preds = %91
  %96 = add i32 %93, 8
  store i32 %96, i32* %92, align 8
  %97 = icmp sle i32 %96, 0
  br i1 %97, label %98, label %103

98:                                               ; preds = %95
  %99 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %100 = load i8*, i8** %99, align 8
  %101 = getelementptr inbounds i8, i8* %100, i32 %93
  %102 = bitcast i8* %101 to i32*
  br label %108

103:                                              ; preds = %95, %91
  %104 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %105 = load i8*, i8** %104, align 8
  %106 = getelementptr inbounds i8, i8* %105, i64 8
  store i8* %106, i8** %104, align 8
  %107 = bitcast i8* %105 to i32*
  br label %108

108:                                              ; preds = %103, %98
  %109 = phi i32* [ %102, %98 ], [ %107, %103 ]
  %110 = load i32, i32* %109, align 8
  %111 = trunc i32 %110 to i16
  %112 = load %union.jvalue*, %union.jvalue** %10, align 8
  %113 = load i32, i32* %13, align 4
  %114 = sext i32 %113 to i64
  %115 = getelementptr inbounds %union.jvalue, %union.jvalue* %112, i64 %114
  %116 = bitcast %union.jvalue* %115 to i16*
  store i16 %111, i16* %116, align 8
  br label %271

117:                                              ; preds = %33
  %118 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %119 = load i32, i32* %118, align 8
  %120 = icmp sge i32 %119, 0
  br i1 %120, label %129, label %121

121:                                              ; preds = %117
  %122 = add i32 %119, 8
  store i32 %122, i32* %118, align 8
  %123 = icmp sle i32 %122, 0
  br i1 %123, label %124, label %129

124:                                              ; preds = %121
  %125 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %126 = load i8*, i8** %125, align 8
  %127 = getelementptr inbounds i8, i8* %126, i32 %119
  %128 = bitcast i8* %127 to i32*
  br label %134

129:                                              ; preds = %121, %117
  %130 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %131 = load i8*, i8** %130, align 8
  %132 = getelementptr inbounds i8, i8* %131, i64 8
  store i8* %132, i8** %130, align 8
  %133 = bitcast i8* %131 to i32*
  br label %134

134:                                              ; preds = %129, %124
  %135 = phi i32* [ %128, %124 ], [ %133, %129 ]
  %136 = load i32, i32* %135, align 8
  %137 = trunc i32 %136 to i16
  %138 = zext i16 %137 to i32
  %139 = load %union.jvalue*, %union.jvalue** %10, align 8
  %140 = load i32, i32* %13, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %139, i64 %141
  %143 = bitcast %union.jvalue* %142 to i32*
  store i32 %138, i32* %143, align 8
  br label %271

144:                                              ; preds = %33
  %145 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %146 = load i32, i32* %145, align 8
  %147 = icmp sge i32 %146, 0
  br i1 %147, label %156, label %148

148:                                              ; preds = %144
  %149 = add i32 %146, 8
  store i32 %149, i32* %145, align 8
  %150 = icmp sle i32 %149, 0
  br i1 %150, label %151, label %156

151:                                              ; preds = %148
  %152 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %153 = load i8*, i8** %152, align 8
  %154 = getelementptr inbounds i8, i8* %153, i32 %146
  %155 = bitcast i8* %154 to i32*
  br label %161

156:                                              ; preds = %148, %144
  %157 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %158 = load i8*, i8** %157, align 8
  %159 = getelementptr inbounds i8, i8* %158, i64 8
  store i8* %159, i8** %157, align 8
  %160 = bitcast i8* %158 to i32*
  br label %161

161:                                              ; preds = %156, %151
  %162 = phi i32* [ %155, %151 ], [ %160, %156 ]
  %163 = load i32, i32* %162, align 8
  %164 = load %union.jvalue*, %union.jvalue** %10, align 8
  %165 = load i32, i32* %13, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds %union.jvalue, %union.jvalue* %164, i64 %166
  %168 = bitcast %union.jvalue* %167 to i32*
  store i32 %163, i32* %168, align 8
  br label %271

169:                                              ; preds = %33
  %170 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %171 = load i32, i32* %170, align 8
  %172 = icmp sge i32 %171, 0
  br i1 %172, label %181, label %173

173:                                              ; preds = %169
  %174 = add i32 %171, 8
  store i32 %174, i32* %170, align 8
  %175 = icmp sle i32 %174, 0
  br i1 %175, label %176, label %181

176:                                              ; preds = %173
  %177 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %178 = load i8*, i8** %177, align 8
  %179 = getelementptr inbounds i8, i8* %178, i32 %171
  %180 = bitcast i8* %179 to i64*
  br label %186

181:                                              ; preds = %173, %169
  %182 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %183 = load i8*, i8** %182, align 8
  %184 = getelementptr inbounds i8, i8* %183, i64 8
  store i8* %184, i8** %182, align 8
  %185 = bitcast i8* %183 to i64*
  br label %186

186:                                              ; preds = %181, %176
  %187 = phi i64* [ %180, %176 ], [ %185, %181 ]
  %188 = load i64, i64* %187, align 8
  %189 = load %union.jvalue*, %union.jvalue** %10, align 8
  %190 = load i32, i32* %13, align 4
  %191 = sext i32 %190 to i64
  %192 = getelementptr inbounds %union.jvalue, %union.jvalue* %189, i64 %191
  %193 = bitcast %union.jvalue* %192 to i64*
  store i64 %188, i64* %193, align 8
  br label %271

194:                                              ; preds = %33
  %195 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 4
  %196 = load i32, i32* %195, align 4
  %197 = icmp sge i32 %196, 0
  br i1 %197, label %206, label %198

198:                                              ; preds = %194
  %199 = add i32 %196, 16
  store i32 %199, i32* %195, align 4
  %200 = icmp sle i32 %199, 0
  br i1 %200, label %201, label %206

201:                                              ; preds = %198
  %202 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 2
  %203 = load i8*, i8** %202, align 8
  %204 = getelementptr inbounds i8, i8* %203, i32 %196
  %205 = bitcast i8* %204 to double*
  br label %211

206:                                              ; preds = %198, %194
  %207 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %208 = load i8*, i8** %207, align 8
  %209 = getelementptr inbounds i8, i8* %208, i64 8
  store i8* %209, i8** %207, align 8
  %210 = bitcast i8* %208 to double*
  br label %211

211:                                              ; preds = %206, %201
  %212 = phi double* [ %205, %201 ], [ %210, %206 ]
  %213 = load double, double* %212, align 8
  %214 = load %union.jvalue*, %union.jvalue** %10, align 8
  %215 = load i32, i32* %13, align 4
  %216 = sext i32 %215 to i64
  %217 = getelementptr inbounds %union.jvalue, %union.jvalue* %214, i64 %216
  %218 = bitcast %union.jvalue* %217 to double*
  store double %213, double* %218, align 8
  br label %271

219:                                              ; preds = %33
  %220 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 4
  %221 = load i32, i32* %220, align 4
  %222 = icmp sge i32 %221, 0
  br i1 %222, label %231, label %223

223:                                              ; preds = %219
  %224 = add i32 %221, 16
  store i32 %224, i32* %220, align 4
  %225 = icmp sle i32 %224, 0
  br i1 %225, label %226, label %231

226:                                              ; preds = %223
  %227 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 2
  %228 = load i8*, i8** %227, align 8
  %229 = getelementptr inbounds i8, i8* %228, i32 %221
  %230 = bitcast i8* %229 to double*
  br label %236

231:                                              ; preds = %223, %219
  %232 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %233 = load i8*, i8** %232, align 8
  %234 = getelementptr inbounds i8, i8* %233, i64 8
  store i8* %234, i8** %232, align 8
  %235 = bitcast i8* %233 to double*
  br label %236

236:                                              ; preds = %231, %226
  %237 = phi double* [ %230, %226 ], [ %235, %231 ]
  %238 = load double, double* %237, align 8
  %239 = fptrunc double %238 to float
  %240 = load %union.jvalue*, %union.jvalue** %10, align 8
  %241 = load i32, i32* %13, align 4
  %242 = sext i32 %241 to i64
  %243 = getelementptr inbounds %union.jvalue, %union.jvalue* %240, i64 %242
  %244 = bitcast %union.jvalue* %243 to float*
  store float %239, float* %244, align 8
  br label %271

245:                                              ; preds = %33
  %246 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %247 = load i32, i32* %246, align 8
  %248 = icmp sge i32 %247, 0
  br i1 %248, label %257, label %249

249:                                              ; preds = %245
  %250 = add i32 %247, 8
  store i32 %250, i32* %246, align 8
  %251 = icmp sle i32 %250, 0
  br i1 %251, label %252, label %257

252:                                              ; preds = %249
  %253 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %254 = load i8*, i8** %253, align 8
  %255 = getelementptr inbounds i8, i8* %254, i32 %247
  %256 = bitcast i8* %255 to i8**
  br label %262

257:                                              ; preds = %249, %245
  %258 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %259 = load i8*, i8** %258, align 8
  %260 = getelementptr inbounds i8, i8* %259, i64 8
  store i8* %260, i8** %258, align 8
  %261 = bitcast i8* %259 to i8**
  br label %262

262:                                              ; preds = %257, %252
  %263 = phi i8** [ %256, %252 ], [ %261, %257 ]
  %264 = load i8*, i8** %263, align 8
  %265 = bitcast i8* %264 to %struct._jobject*
  %266 = load %union.jvalue*, %union.jvalue** %10, align 8
  %267 = load i32, i32* %13, align 4
  %268 = sext i32 %267 to i64
  %269 = getelementptr inbounds %union.jvalue, %union.jvalue* %266, i64 %268
  %270 = bitcast %union.jvalue* %269 to %struct._jobject**
  store %struct._jobject* %265, %struct._jobject** %270, align 8
  br label %271

271:                                              ; preds = %33, %262, %236, %211, %186, %161, %134, %108, %82, %56
  br label %272

272:                                              ; preds = %271
  %273 = load i32, i32* %13, align 4
  %274 = add nsw i32 %273, 1
  store i32 %274, i32* %13, align 4
  br label %29, !llvm.loop !33

275:                                              ; preds = %29
  br label %276

276:                                              ; preds = %275
  %277 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %278 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %277, align 8
  %279 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %278, i32 0, i32 87
  %280 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %279, align 8
  %281 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %282 = load %struct._jobject*, %struct._jobject** %7, align 8
  %283 = load %struct._jobject*, %struct._jobject** %8, align 8
  %284 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %285 = load %union.jvalue*, %union.jvalue** %10, align 8
  %286 = call float %280(%struct.JNINativeInterface_** noundef %281, %struct._jobject* noundef %282, %struct._jobject* noundef %283, %struct._jmethodID* noundef %284, %union.jvalue* noundef %285)
  ret float %286
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local float @JNI_CallStaticFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca float, align 4
  %8 = alloca %"struct.std::__va_list", align 8
  %9 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %10 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 136
  %14 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = bitcast %"struct.std::__va_list"* %9 to i8*
  %19 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %18, i8* align 8 %19, i64 32, i1 false)
  %20 = call float %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %"struct.std::__va_list"* noundef %9)
  store float %20, float* %7, align 4
  %21 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load float, float* %7, align 4
  ret float %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local float @JNI_CallStaticFloatMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !34

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 137
  %278 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  %283 = call float %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret float %283
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local double @JNI_CallDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca double, align 8
  %8 = alloca %"struct.std::__va_list", align 8
  %9 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %10 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 59
  %14 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = bitcast %"struct.std::__va_list"* %9 to i8*
  %19 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %18, i8* align 8 %19, i64 32, i1 false)
  %20 = call double %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %"struct.std::__va_list"* noundef %9)
  store double %20, double* %7, align 8
  %21 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load double, double* %7, align 8
  ret double %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local double @JNI_CallDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !35

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 60
  %278 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  %283 = call double %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret double %283
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local double @JNI_CallNonvirtualDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca double, align 8
  %10 = alloca %"struct.std::__va_list", align 8
  %11 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %12 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.va_start(i8* %12)
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 89
  %16 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %15, align 8
  %17 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %18 = load %struct._jobject*, %struct._jobject** %6, align 8
  %19 = load %struct._jobject*, %struct._jobject** %7, align 8
  %20 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %21 = bitcast %"struct.std::__va_list"* %11 to i8*
  %22 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %21, i8* align 8 %22, i64 32, i1 false)
  %23 = call double %16(%struct.JNINativeInterface_** noundef %17, %struct._jobject* noundef %18, %struct._jobject* noundef %19, %struct._jmethodID* noundef %20, %"struct.std::__va_list"* noundef %11)
  store double %23, double* %9, align 8
  %24 = bitcast %"struct.std::__va_list"* %10 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load double, double* %9, align 8
  ret double %25
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local double @JNI_CallNonvirtualDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca %union.jvalue*, align 8
  %11 = alloca [257 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  br label %14

14:                                               ; preds = %5
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 8
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 8
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %21 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i64 0, i64 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %12, align 4
  %24 = load i32, i32* %12, align 4
  %25 = sext i32 %24 to i64
  %26 = mul i64 %25, 8
  %27 = alloca i8, i64 %26, align 16
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %10, align 8
  store i32 0, i32* %13, align 4
  br label %29

29:                                               ; preds = %272, %14
  %30 = load i32, i32* %13, align 4
  %31 = load i32, i32* %12, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %275

33:                                               ; preds = %29
  %34 = load i32, i32* %13, align 4
  %35 = sext i32 %34 to i64
  %36 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i64 0, i64 %35
  %37 = load i8, i8* %36, align 1
  %38 = zext i8 %37 to i32
  switch i32 %38, label %271 [
    i32 90, label %39
    i32 66, label %65
    i32 83, label %91
    i32 67, label %117
    i32 73, label %144
    i32 74, label %169
    i32 68, label %194
    i32 70, label %219
    i32 76, label %245
  ]

39:                                               ; preds = %33
  %40 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %41 = load i32, i32* %40, align 8
  %42 = icmp sge i32 %41, 0
  br i1 %42, label %51, label %43

43:                                               ; preds = %39
  %44 = add i32 %41, 8
  store i32 %44, i32* %40, align 8
  %45 = icmp sle i32 %44, 0
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %48 = load i8*, i8** %47, align 8
  %49 = getelementptr inbounds i8, i8* %48, i32 %41
  %50 = bitcast i8* %49 to i32*
  br label %56

51:                                               ; preds = %43, %39
  %52 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %53 = load i8*, i8** %52, align 8
  %54 = getelementptr inbounds i8, i8* %53, i64 8
  store i8* %54, i8** %52, align 8
  %55 = bitcast i8* %53 to i32*
  br label %56

56:                                               ; preds = %51, %46
  %57 = phi i32* [ %50, %46 ], [ %55, %51 ]
  %58 = load i32, i32* %57, align 8
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %10, align 8
  %61 = load i32, i32* %13, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %59, i8* %64, align 8
  br label %271

65:                                               ; preds = %33
  %66 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %67 = load i32, i32* %66, align 8
  %68 = icmp sge i32 %67, 0
  br i1 %68, label %77, label %69

69:                                               ; preds = %65
  %70 = add i32 %67, 8
  store i32 %70, i32* %66, align 8
  %71 = icmp sle i32 %70, 0
  br i1 %71, label %72, label %77

72:                                               ; preds = %69
  %73 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %74 = load i8*, i8** %73, align 8
  %75 = getelementptr inbounds i8, i8* %74, i32 %67
  %76 = bitcast i8* %75 to i32*
  br label %82

77:                                               ; preds = %69, %65
  %78 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %79 = load i8*, i8** %78, align 8
  %80 = getelementptr inbounds i8, i8* %79, i64 8
  store i8* %80, i8** %78, align 8
  %81 = bitcast i8* %79 to i32*
  br label %82

82:                                               ; preds = %77, %72
  %83 = phi i32* [ %76, %72 ], [ %81, %77 ]
  %84 = load i32, i32* %83, align 8
  %85 = trunc i32 %84 to i8
  %86 = load %union.jvalue*, %union.jvalue** %10, align 8
  %87 = load i32, i32* %13, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, %union.jvalue* %86, i64 %88
  %90 = bitcast %union.jvalue* %89 to i8*
  store i8 %85, i8* %90, align 8
  br label %271

91:                                               ; preds = %33
  %92 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %93 = load i32, i32* %92, align 8
  %94 = icmp sge i32 %93, 0
  br i1 %94, label %103, label %95

95:                                               ; preds = %91
  %96 = add i32 %93, 8
  store i32 %96, i32* %92, align 8
  %97 = icmp sle i32 %96, 0
  br i1 %97, label %98, label %103

98:                                               ; preds = %95
  %99 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %100 = load i8*, i8** %99, align 8
  %101 = getelementptr inbounds i8, i8* %100, i32 %93
  %102 = bitcast i8* %101 to i32*
  br label %108

103:                                              ; preds = %95, %91
  %104 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %105 = load i8*, i8** %104, align 8
  %106 = getelementptr inbounds i8, i8* %105, i64 8
  store i8* %106, i8** %104, align 8
  %107 = bitcast i8* %105 to i32*
  br label %108

108:                                              ; preds = %103, %98
  %109 = phi i32* [ %102, %98 ], [ %107, %103 ]
  %110 = load i32, i32* %109, align 8
  %111 = trunc i32 %110 to i16
  %112 = load %union.jvalue*, %union.jvalue** %10, align 8
  %113 = load i32, i32* %13, align 4
  %114 = sext i32 %113 to i64
  %115 = getelementptr inbounds %union.jvalue, %union.jvalue* %112, i64 %114
  %116 = bitcast %union.jvalue* %115 to i16*
  store i16 %111, i16* %116, align 8
  br label %271

117:                                              ; preds = %33
  %118 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %119 = load i32, i32* %118, align 8
  %120 = icmp sge i32 %119, 0
  br i1 %120, label %129, label %121

121:                                              ; preds = %117
  %122 = add i32 %119, 8
  store i32 %122, i32* %118, align 8
  %123 = icmp sle i32 %122, 0
  br i1 %123, label %124, label %129

124:                                              ; preds = %121
  %125 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %126 = load i8*, i8** %125, align 8
  %127 = getelementptr inbounds i8, i8* %126, i32 %119
  %128 = bitcast i8* %127 to i32*
  br label %134

129:                                              ; preds = %121, %117
  %130 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %131 = load i8*, i8** %130, align 8
  %132 = getelementptr inbounds i8, i8* %131, i64 8
  store i8* %132, i8** %130, align 8
  %133 = bitcast i8* %131 to i32*
  br label %134

134:                                              ; preds = %129, %124
  %135 = phi i32* [ %128, %124 ], [ %133, %129 ]
  %136 = load i32, i32* %135, align 8
  %137 = trunc i32 %136 to i16
  %138 = zext i16 %137 to i32
  %139 = load %union.jvalue*, %union.jvalue** %10, align 8
  %140 = load i32, i32* %13, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %139, i64 %141
  %143 = bitcast %union.jvalue* %142 to i32*
  store i32 %138, i32* %143, align 8
  br label %271

144:                                              ; preds = %33
  %145 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %146 = load i32, i32* %145, align 8
  %147 = icmp sge i32 %146, 0
  br i1 %147, label %156, label %148

148:                                              ; preds = %144
  %149 = add i32 %146, 8
  store i32 %149, i32* %145, align 8
  %150 = icmp sle i32 %149, 0
  br i1 %150, label %151, label %156

151:                                              ; preds = %148
  %152 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %153 = load i8*, i8** %152, align 8
  %154 = getelementptr inbounds i8, i8* %153, i32 %146
  %155 = bitcast i8* %154 to i32*
  br label %161

156:                                              ; preds = %148, %144
  %157 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %158 = load i8*, i8** %157, align 8
  %159 = getelementptr inbounds i8, i8* %158, i64 8
  store i8* %159, i8** %157, align 8
  %160 = bitcast i8* %158 to i32*
  br label %161

161:                                              ; preds = %156, %151
  %162 = phi i32* [ %155, %151 ], [ %160, %156 ]
  %163 = load i32, i32* %162, align 8
  %164 = load %union.jvalue*, %union.jvalue** %10, align 8
  %165 = load i32, i32* %13, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds %union.jvalue, %union.jvalue* %164, i64 %166
  %168 = bitcast %union.jvalue* %167 to i32*
  store i32 %163, i32* %168, align 8
  br label %271

169:                                              ; preds = %33
  %170 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %171 = load i32, i32* %170, align 8
  %172 = icmp sge i32 %171, 0
  br i1 %172, label %181, label %173

173:                                              ; preds = %169
  %174 = add i32 %171, 8
  store i32 %174, i32* %170, align 8
  %175 = icmp sle i32 %174, 0
  br i1 %175, label %176, label %181

176:                                              ; preds = %173
  %177 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %178 = load i8*, i8** %177, align 8
  %179 = getelementptr inbounds i8, i8* %178, i32 %171
  %180 = bitcast i8* %179 to i64*
  br label %186

181:                                              ; preds = %173, %169
  %182 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %183 = load i8*, i8** %182, align 8
  %184 = getelementptr inbounds i8, i8* %183, i64 8
  store i8* %184, i8** %182, align 8
  %185 = bitcast i8* %183 to i64*
  br label %186

186:                                              ; preds = %181, %176
  %187 = phi i64* [ %180, %176 ], [ %185, %181 ]
  %188 = load i64, i64* %187, align 8
  %189 = load %union.jvalue*, %union.jvalue** %10, align 8
  %190 = load i32, i32* %13, align 4
  %191 = sext i32 %190 to i64
  %192 = getelementptr inbounds %union.jvalue, %union.jvalue* %189, i64 %191
  %193 = bitcast %union.jvalue* %192 to i64*
  store i64 %188, i64* %193, align 8
  br label %271

194:                                              ; preds = %33
  %195 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 4
  %196 = load i32, i32* %195, align 4
  %197 = icmp sge i32 %196, 0
  br i1 %197, label %206, label %198

198:                                              ; preds = %194
  %199 = add i32 %196, 16
  store i32 %199, i32* %195, align 4
  %200 = icmp sle i32 %199, 0
  br i1 %200, label %201, label %206

201:                                              ; preds = %198
  %202 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 2
  %203 = load i8*, i8** %202, align 8
  %204 = getelementptr inbounds i8, i8* %203, i32 %196
  %205 = bitcast i8* %204 to double*
  br label %211

206:                                              ; preds = %198, %194
  %207 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %208 = load i8*, i8** %207, align 8
  %209 = getelementptr inbounds i8, i8* %208, i64 8
  store i8* %209, i8** %207, align 8
  %210 = bitcast i8* %208 to double*
  br label %211

211:                                              ; preds = %206, %201
  %212 = phi double* [ %205, %201 ], [ %210, %206 ]
  %213 = load double, double* %212, align 8
  %214 = load %union.jvalue*, %union.jvalue** %10, align 8
  %215 = load i32, i32* %13, align 4
  %216 = sext i32 %215 to i64
  %217 = getelementptr inbounds %union.jvalue, %union.jvalue* %214, i64 %216
  %218 = bitcast %union.jvalue* %217 to double*
  store double %213, double* %218, align 8
  br label %271

219:                                              ; preds = %33
  %220 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 4
  %221 = load i32, i32* %220, align 4
  %222 = icmp sge i32 %221, 0
  br i1 %222, label %231, label %223

223:                                              ; preds = %219
  %224 = add i32 %221, 16
  store i32 %224, i32* %220, align 4
  %225 = icmp sle i32 %224, 0
  br i1 %225, label %226, label %231

226:                                              ; preds = %223
  %227 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 2
  %228 = load i8*, i8** %227, align 8
  %229 = getelementptr inbounds i8, i8* %228, i32 %221
  %230 = bitcast i8* %229 to double*
  br label %236

231:                                              ; preds = %223, %219
  %232 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %233 = load i8*, i8** %232, align 8
  %234 = getelementptr inbounds i8, i8* %233, i64 8
  store i8* %234, i8** %232, align 8
  %235 = bitcast i8* %233 to double*
  br label %236

236:                                              ; preds = %231, %226
  %237 = phi double* [ %230, %226 ], [ %235, %231 ]
  %238 = load double, double* %237, align 8
  %239 = fptrunc double %238 to float
  %240 = load %union.jvalue*, %union.jvalue** %10, align 8
  %241 = load i32, i32* %13, align 4
  %242 = sext i32 %241 to i64
  %243 = getelementptr inbounds %union.jvalue, %union.jvalue* %240, i64 %242
  %244 = bitcast %union.jvalue* %243 to float*
  store float %239, float* %244, align 8
  br label %271

245:                                              ; preds = %33
  %246 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %247 = load i32, i32* %246, align 8
  %248 = icmp sge i32 %247, 0
  br i1 %248, label %257, label %249

249:                                              ; preds = %245
  %250 = add i32 %247, 8
  store i32 %250, i32* %246, align 8
  %251 = icmp sle i32 %250, 0
  br i1 %251, label %252, label %257

252:                                              ; preds = %249
  %253 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %254 = load i8*, i8** %253, align 8
  %255 = getelementptr inbounds i8, i8* %254, i32 %247
  %256 = bitcast i8* %255 to i8**
  br label %262

257:                                              ; preds = %249, %245
  %258 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %259 = load i8*, i8** %258, align 8
  %260 = getelementptr inbounds i8, i8* %259, i64 8
  store i8* %260, i8** %258, align 8
  %261 = bitcast i8* %259 to i8**
  br label %262

262:                                              ; preds = %257, %252
  %263 = phi i8** [ %256, %252 ], [ %261, %257 ]
  %264 = load i8*, i8** %263, align 8
  %265 = bitcast i8* %264 to %struct._jobject*
  %266 = load %union.jvalue*, %union.jvalue** %10, align 8
  %267 = load i32, i32* %13, align 4
  %268 = sext i32 %267 to i64
  %269 = getelementptr inbounds %union.jvalue, %union.jvalue* %266, i64 %268
  %270 = bitcast %union.jvalue* %269 to %struct._jobject**
  store %struct._jobject* %265, %struct._jobject** %270, align 8
  br label %271

271:                                              ; preds = %33, %262, %236, %211, %186, %161, %134, %108, %82, %56
  br label %272

272:                                              ; preds = %271
  %273 = load i32, i32* %13, align 4
  %274 = add nsw i32 %273, 1
  store i32 %274, i32* %13, align 4
  br label %29, !llvm.loop !36

275:                                              ; preds = %29
  br label %276

276:                                              ; preds = %275
  %277 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %278 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %277, align 8
  %279 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %278, i32 0, i32 90
  %280 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %279, align 8
  %281 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %282 = load %struct._jobject*, %struct._jobject** %7, align 8
  %283 = load %struct._jobject*, %struct._jobject** %8, align 8
  %284 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %285 = load %union.jvalue*, %union.jvalue** %10, align 8
  %286 = call double %280(%struct.JNINativeInterface_** noundef %281, %struct._jobject* noundef %282, %struct._jobject* noundef %283, %struct._jmethodID* noundef %284, %union.jvalue* noundef %285)
  ret double %286
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local double @JNI_CallStaticDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca double, align 8
  %8 = alloca %"struct.std::__va_list", align 8
  %9 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %10 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 139
  %14 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = bitcast %"struct.std::__va_list"* %9 to i8*
  %19 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %18, i8* align 8 %19, i64 32, i1 false)
  %20 = call double %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %"struct.std::__va_list"* noundef %9)
  store double %20, double* %7, align 8
  %21 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load double, double* %7, align 8
  ret double %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local double @JNI_CallStaticDoubleMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !37

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 140
  %278 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  %283 = call double %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret double %283
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local %struct._jobject* @JNI_NewObject(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %"struct.std::__va_list", align 8
  %9 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %10 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 29
  %14 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %13, align 8
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %16 = load %struct._jobject*, %struct._jobject** %5, align 8
  %17 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %18 = bitcast %"struct.std::__va_list"* %9 to i8*
  %19 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %18, i8* align 8 %19, i64 32, i1 false)
  %20 = call %struct._jobject* %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jmethodID* noundef %17, %"struct.std::__va_list"* noundef %9)
  store %struct._jobject* %20, %struct._jobject** %7, align 8
  %21 = bitcast %"struct.std::__va_list"* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load %struct._jobject*, %struct._jobject** %7, align 8
  ret %struct._jobject* %22
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local %struct._jobject* @JNI_NewObjectV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !38

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 30
  %278 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  %283 = call %struct._jobject* %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret %struct._jobject* %283
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @JNI_CallVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca %"struct.std::__va_list", align 8
  %8 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = bitcast %"struct.std::__va_list"* %7 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 8
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 62
  %13 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %12, align 8
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %15 = load %struct._jobject*, %struct._jobject** %5, align 8
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %17 = bitcast %"struct.std::__va_list"* %8 to i8*
  %18 = bitcast %"struct.std::__va_list"* %7 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %17, i8* align 8 %18, i64 32, i1 false)
  call void %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, %"struct.std::__va_list"* noundef %8)
  %19 = bitcast %"struct.std::__va_list"* %7 to i8*
  call void @llvm.va_end(i8* %19)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @JNI_CallVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !39

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 63
  %278 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  call void %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @JNI_CallNonvirtualVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jmethodID*, align 8
  %9 = alloca %"struct.std::__va_list", align 8
  %10 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jobject* %2, %struct._jobject** %7, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 8
  %11 = bitcast %"struct.std::__va_list"* %9 to i8*
  call void @llvm.va_start(i8* %11)
  %12 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %13 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %12, align 8
  %14 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %13, i32 0, i32 92
  %15 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %14, align 8
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %17 = load %struct._jobject*, %struct._jobject** %6, align 8
  %18 = load %struct._jobject*, %struct._jobject** %7, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %8, align 8
  %20 = bitcast %"struct.std::__va_list"* %10 to i8*
  %21 = bitcast %"struct.std::__va_list"* %9 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %20, i8* align 8 %21, i64 32, i1 false)
  call void %15(%struct.JNINativeInterface_** noundef %16, %struct._jobject* noundef %17, %struct._jobject* noundef %18, %struct._jmethodID* noundef %19, %"struct.std::__va_list"* noundef %10)
  %22 = bitcast %"struct.std::__va_list"* %9 to i8*
  call void @llvm.va_end(i8* %22)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @JNI_CallNonvirtualVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, %"struct.std::__va_list"* noundef %4) #0 {
  %6 = alloca %struct.JNINativeInterface_**, align 8
  %7 = alloca %struct._jobject*, align 8
  %8 = alloca %struct._jobject*, align 8
  %9 = alloca %struct._jmethodID*, align 8
  %10 = alloca %union.jvalue*, align 8
  %11 = alloca [257 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %6, align 8
  store %struct._jobject* %1, %struct._jobject** %7, align 8
  store %struct._jobject* %2, %struct._jobject** %8, align 8
  store %struct._jmethodID* %3, %struct._jmethodID** %9, align 8
  br label %14

14:                                               ; preds = %5
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %16 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %15, align 8
  %17 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %16, i32 0, i32 0
  %18 = load i8*, i8** %17, align 8
  %19 = bitcast i8* %18 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %20 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %21 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %22 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i64 0, i64 0
  %23 = call i32 %19(%struct.JNINativeInterface_** noundef %20, %struct._jmethodID* noundef %21, i8* noundef %22)
  store i32 %23, i32* %12, align 4
  %24 = load i32, i32* %12, align 4
  %25 = sext i32 %24 to i64
  %26 = mul i64 %25, 8
  %27 = alloca i8, i64 %26, align 16
  %28 = bitcast i8* %27 to %union.jvalue*
  store %union.jvalue* %28, %union.jvalue** %10, align 8
  store i32 0, i32* %13, align 4
  br label %29

29:                                               ; preds = %272, %14
  %30 = load i32, i32* %13, align 4
  %31 = load i32, i32* %12, align 4
  %32 = icmp slt i32 %30, %31
  br i1 %32, label %33, label %275

33:                                               ; preds = %29
  %34 = load i32, i32* %13, align 4
  %35 = sext i32 %34 to i64
  %36 = getelementptr inbounds [257 x i8], [257 x i8]* %11, i64 0, i64 %35
  %37 = load i8, i8* %36, align 1
  %38 = zext i8 %37 to i32
  switch i32 %38, label %271 [
    i32 90, label %39
    i32 66, label %65
    i32 83, label %91
    i32 67, label %117
    i32 73, label %144
    i32 74, label %169
    i32 68, label %194
    i32 70, label %219
    i32 76, label %245
  ]

39:                                               ; preds = %33
  %40 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %41 = load i32, i32* %40, align 8
  %42 = icmp sge i32 %41, 0
  br i1 %42, label %51, label %43

43:                                               ; preds = %39
  %44 = add i32 %41, 8
  store i32 %44, i32* %40, align 8
  %45 = icmp sle i32 %44, 0
  br i1 %45, label %46, label %51

46:                                               ; preds = %43
  %47 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %48 = load i8*, i8** %47, align 8
  %49 = getelementptr inbounds i8, i8* %48, i32 %41
  %50 = bitcast i8* %49 to i32*
  br label %56

51:                                               ; preds = %43, %39
  %52 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %53 = load i8*, i8** %52, align 8
  %54 = getelementptr inbounds i8, i8* %53, i64 8
  store i8* %54, i8** %52, align 8
  %55 = bitcast i8* %53 to i32*
  br label %56

56:                                               ; preds = %51, %46
  %57 = phi i32* [ %50, %46 ], [ %55, %51 ]
  %58 = load i32, i32* %57, align 8
  %59 = trunc i32 %58 to i8
  %60 = load %union.jvalue*, %union.jvalue** %10, align 8
  %61 = load i32, i32* %13, align 4
  %62 = sext i32 %61 to i64
  %63 = getelementptr inbounds %union.jvalue, %union.jvalue* %60, i64 %62
  %64 = bitcast %union.jvalue* %63 to i8*
  store i8 %59, i8* %64, align 8
  br label %271

65:                                               ; preds = %33
  %66 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %67 = load i32, i32* %66, align 8
  %68 = icmp sge i32 %67, 0
  br i1 %68, label %77, label %69

69:                                               ; preds = %65
  %70 = add i32 %67, 8
  store i32 %70, i32* %66, align 8
  %71 = icmp sle i32 %70, 0
  br i1 %71, label %72, label %77

72:                                               ; preds = %69
  %73 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %74 = load i8*, i8** %73, align 8
  %75 = getelementptr inbounds i8, i8* %74, i32 %67
  %76 = bitcast i8* %75 to i32*
  br label %82

77:                                               ; preds = %69, %65
  %78 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %79 = load i8*, i8** %78, align 8
  %80 = getelementptr inbounds i8, i8* %79, i64 8
  store i8* %80, i8** %78, align 8
  %81 = bitcast i8* %79 to i32*
  br label %82

82:                                               ; preds = %77, %72
  %83 = phi i32* [ %76, %72 ], [ %81, %77 ]
  %84 = load i32, i32* %83, align 8
  %85 = trunc i32 %84 to i8
  %86 = load %union.jvalue*, %union.jvalue** %10, align 8
  %87 = load i32, i32* %13, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, %union.jvalue* %86, i64 %88
  %90 = bitcast %union.jvalue* %89 to i8*
  store i8 %85, i8* %90, align 8
  br label %271

91:                                               ; preds = %33
  %92 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %93 = load i32, i32* %92, align 8
  %94 = icmp sge i32 %93, 0
  br i1 %94, label %103, label %95

95:                                               ; preds = %91
  %96 = add i32 %93, 8
  store i32 %96, i32* %92, align 8
  %97 = icmp sle i32 %96, 0
  br i1 %97, label %98, label %103

98:                                               ; preds = %95
  %99 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %100 = load i8*, i8** %99, align 8
  %101 = getelementptr inbounds i8, i8* %100, i32 %93
  %102 = bitcast i8* %101 to i32*
  br label %108

103:                                              ; preds = %95, %91
  %104 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %105 = load i8*, i8** %104, align 8
  %106 = getelementptr inbounds i8, i8* %105, i64 8
  store i8* %106, i8** %104, align 8
  %107 = bitcast i8* %105 to i32*
  br label %108

108:                                              ; preds = %103, %98
  %109 = phi i32* [ %102, %98 ], [ %107, %103 ]
  %110 = load i32, i32* %109, align 8
  %111 = trunc i32 %110 to i16
  %112 = load %union.jvalue*, %union.jvalue** %10, align 8
  %113 = load i32, i32* %13, align 4
  %114 = sext i32 %113 to i64
  %115 = getelementptr inbounds %union.jvalue, %union.jvalue* %112, i64 %114
  %116 = bitcast %union.jvalue* %115 to i16*
  store i16 %111, i16* %116, align 8
  br label %271

117:                                              ; preds = %33
  %118 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %119 = load i32, i32* %118, align 8
  %120 = icmp sge i32 %119, 0
  br i1 %120, label %129, label %121

121:                                              ; preds = %117
  %122 = add i32 %119, 8
  store i32 %122, i32* %118, align 8
  %123 = icmp sle i32 %122, 0
  br i1 %123, label %124, label %129

124:                                              ; preds = %121
  %125 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %126 = load i8*, i8** %125, align 8
  %127 = getelementptr inbounds i8, i8* %126, i32 %119
  %128 = bitcast i8* %127 to i32*
  br label %134

129:                                              ; preds = %121, %117
  %130 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %131 = load i8*, i8** %130, align 8
  %132 = getelementptr inbounds i8, i8* %131, i64 8
  store i8* %132, i8** %130, align 8
  %133 = bitcast i8* %131 to i32*
  br label %134

134:                                              ; preds = %129, %124
  %135 = phi i32* [ %128, %124 ], [ %133, %129 ]
  %136 = load i32, i32* %135, align 8
  %137 = trunc i32 %136 to i16
  %138 = zext i16 %137 to i32
  %139 = load %union.jvalue*, %union.jvalue** %10, align 8
  %140 = load i32, i32* %13, align 4
  %141 = sext i32 %140 to i64
  %142 = getelementptr inbounds %union.jvalue, %union.jvalue* %139, i64 %141
  %143 = bitcast %union.jvalue* %142 to i32*
  store i32 %138, i32* %143, align 8
  br label %271

144:                                              ; preds = %33
  %145 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %146 = load i32, i32* %145, align 8
  %147 = icmp sge i32 %146, 0
  br i1 %147, label %156, label %148

148:                                              ; preds = %144
  %149 = add i32 %146, 8
  store i32 %149, i32* %145, align 8
  %150 = icmp sle i32 %149, 0
  br i1 %150, label %151, label %156

151:                                              ; preds = %148
  %152 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %153 = load i8*, i8** %152, align 8
  %154 = getelementptr inbounds i8, i8* %153, i32 %146
  %155 = bitcast i8* %154 to i32*
  br label %161

156:                                              ; preds = %148, %144
  %157 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %158 = load i8*, i8** %157, align 8
  %159 = getelementptr inbounds i8, i8* %158, i64 8
  store i8* %159, i8** %157, align 8
  %160 = bitcast i8* %158 to i32*
  br label %161

161:                                              ; preds = %156, %151
  %162 = phi i32* [ %155, %151 ], [ %160, %156 ]
  %163 = load i32, i32* %162, align 8
  %164 = load %union.jvalue*, %union.jvalue** %10, align 8
  %165 = load i32, i32* %13, align 4
  %166 = sext i32 %165 to i64
  %167 = getelementptr inbounds %union.jvalue, %union.jvalue* %164, i64 %166
  %168 = bitcast %union.jvalue* %167 to i32*
  store i32 %163, i32* %168, align 8
  br label %271

169:                                              ; preds = %33
  %170 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %171 = load i32, i32* %170, align 8
  %172 = icmp sge i32 %171, 0
  br i1 %172, label %181, label %173

173:                                              ; preds = %169
  %174 = add i32 %171, 8
  store i32 %174, i32* %170, align 8
  %175 = icmp sle i32 %174, 0
  br i1 %175, label %176, label %181

176:                                              ; preds = %173
  %177 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %178 = load i8*, i8** %177, align 8
  %179 = getelementptr inbounds i8, i8* %178, i32 %171
  %180 = bitcast i8* %179 to i64*
  br label %186

181:                                              ; preds = %173, %169
  %182 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %183 = load i8*, i8** %182, align 8
  %184 = getelementptr inbounds i8, i8* %183, i64 8
  store i8* %184, i8** %182, align 8
  %185 = bitcast i8* %183 to i64*
  br label %186

186:                                              ; preds = %181, %176
  %187 = phi i64* [ %180, %176 ], [ %185, %181 ]
  %188 = load i64, i64* %187, align 8
  %189 = load %union.jvalue*, %union.jvalue** %10, align 8
  %190 = load i32, i32* %13, align 4
  %191 = sext i32 %190 to i64
  %192 = getelementptr inbounds %union.jvalue, %union.jvalue* %189, i64 %191
  %193 = bitcast %union.jvalue* %192 to i64*
  store i64 %188, i64* %193, align 8
  br label %271

194:                                              ; preds = %33
  %195 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 4
  %196 = load i32, i32* %195, align 4
  %197 = icmp sge i32 %196, 0
  br i1 %197, label %206, label %198

198:                                              ; preds = %194
  %199 = add i32 %196, 16
  store i32 %199, i32* %195, align 4
  %200 = icmp sle i32 %199, 0
  br i1 %200, label %201, label %206

201:                                              ; preds = %198
  %202 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 2
  %203 = load i8*, i8** %202, align 8
  %204 = getelementptr inbounds i8, i8* %203, i32 %196
  %205 = bitcast i8* %204 to double*
  br label %211

206:                                              ; preds = %198, %194
  %207 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %208 = load i8*, i8** %207, align 8
  %209 = getelementptr inbounds i8, i8* %208, i64 8
  store i8* %209, i8** %207, align 8
  %210 = bitcast i8* %208 to double*
  br label %211

211:                                              ; preds = %206, %201
  %212 = phi double* [ %205, %201 ], [ %210, %206 ]
  %213 = load double, double* %212, align 8
  %214 = load %union.jvalue*, %union.jvalue** %10, align 8
  %215 = load i32, i32* %13, align 4
  %216 = sext i32 %215 to i64
  %217 = getelementptr inbounds %union.jvalue, %union.jvalue* %214, i64 %216
  %218 = bitcast %union.jvalue* %217 to double*
  store double %213, double* %218, align 8
  br label %271

219:                                              ; preds = %33
  %220 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 4
  %221 = load i32, i32* %220, align 4
  %222 = icmp sge i32 %221, 0
  br i1 %222, label %231, label %223

223:                                              ; preds = %219
  %224 = add i32 %221, 16
  store i32 %224, i32* %220, align 4
  %225 = icmp sle i32 %224, 0
  br i1 %225, label %226, label %231

226:                                              ; preds = %223
  %227 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 2
  %228 = load i8*, i8** %227, align 8
  %229 = getelementptr inbounds i8, i8* %228, i32 %221
  %230 = bitcast i8* %229 to double*
  br label %236

231:                                              ; preds = %223, %219
  %232 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %233 = load i8*, i8** %232, align 8
  %234 = getelementptr inbounds i8, i8* %233, i64 8
  store i8* %234, i8** %232, align 8
  %235 = bitcast i8* %233 to double*
  br label %236

236:                                              ; preds = %231, %226
  %237 = phi double* [ %230, %226 ], [ %235, %231 ]
  %238 = load double, double* %237, align 8
  %239 = fptrunc double %238 to float
  %240 = load %union.jvalue*, %union.jvalue** %10, align 8
  %241 = load i32, i32* %13, align 4
  %242 = sext i32 %241 to i64
  %243 = getelementptr inbounds %union.jvalue, %union.jvalue* %240, i64 %242
  %244 = bitcast %union.jvalue* %243 to float*
  store float %239, float* %244, align 8
  br label %271

245:                                              ; preds = %33
  %246 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 3
  %247 = load i32, i32* %246, align 8
  %248 = icmp sge i32 %247, 0
  br i1 %248, label %257, label %249

249:                                              ; preds = %245
  %250 = add i32 %247, 8
  store i32 %250, i32* %246, align 8
  %251 = icmp sle i32 %250, 0
  br i1 %251, label %252, label %257

252:                                              ; preds = %249
  %253 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 1
  %254 = load i8*, i8** %253, align 8
  %255 = getelementptr inbounds i8, i8* %254, i32 %247
  %256 = bitcast i8* %255 to i8**
  br label %262

257:                                              ; preds = %249, %245
  %258 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %4, i32 0, i32 0
  %259 = load i8*, i8** %258, align 8
  %260 = getelementptr inbounds i8, i8* %259, i64 8
  store i8* %260, i8** %258, align 8
  %261 = bitcast i8* %259 to i8**
  br label %262

262:                                              ; preds = %257, %252
  %263 = phi i8** [ %256, %252 ], [ %261, %257 ]
  %264 = load i8*, i8** %263, align 8
  %265 = bitcast i8* %264 to %struct._jobject*
  %266 = load %union.jvalue*, %union.jvalue** %10, align 8
  %267 = load i32, i32* %13, align 4
  %268 = sext i32 %267 to i64
  %269 = getelementptr inbounds %union.jvalue, %union.jvalue* %266, i64 %268
  %270 = bitcast %union.jvalue* %269 to %struct._jobject**
  store %struct._jobject* %265, %struct._jobject** %270, align 8
  br label %271

271:                                              ; preds = %33, %262, %236, %211, %186, %161, %134, %108, %82, %56
  br label %272

272:                                              ; preds = %271
  %273 = load i32, i32* %13, align 4
  %274 = add nsw i32 %273, 1
  store i32 %274, i32* %13, align 4
  br label %29, !llvm.loop !40

275:                                              ; preds = %29
  br label %276

276:                                              ; preds = %275
  %277 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %278 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %277, align 8
  %279 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %278, i32 0, i32 93
  %280 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %279, align 8
  %281 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %6, align 8
  %282 = load %struct._jobject*, %struct._jobject** %7, align 8
  %283 = load %struct._jobject*, %struct._jobject** %8, align 8
  %284 = load %struct._jmethodID*, %struct._jmethodID** %9, align 8
  %285 = load %union.jvalue*, %union.jvalue** %10, align 8
  call void %280(%struct.JNINativeInterface_** noundef %281, %struct._jobject* noundef %282, %struct._jobject* noundef %283, %struct._jmethodID* noundef %284, %union.jvalue* noundef %285)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @JNI_CallStaticVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 8
  %5 = alloca %struct._jobject*, align 8
  %6 = alloca %struct._jmethodID*, align 8
  %7 = alloca %"struct.std::__va_list", align 8
  %8 = alloca %"struct.std::__va_list", align 8
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 8
  store %struct._jobject* %1, %struct._jobject** %5, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 8
  %9 = bitcast %"struct.std::__va_list"* %7 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 8
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 142
  %13 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %"struct.std::__va_list"*)** %12, align 8
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 8
  %15 = load %struct._jobject*, %struct._jobject** %5, align 8
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 8
  %17 = bitcast %"struct.std::__va_list"* %8 to i8*
  %18 = bitcast %"struct.std::__va_list"* %7 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %17, i8* align 8 %18, i64 32, i1 false)
  call void %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, %"struct.std::__va_list"* noundef %8)
  %19 = bitcast %"struct.std::__va_list"* %7 to i8*
  call void @llvm.va_end(i8* %19)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @JNI_CallStaticVoidMethodV(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, %"struct.std::__va_list"* noundef %3) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 8
  %6 = alloca %struct._jobject*, align 8
  %7 = alloca %struct._jmethodID*, align 8
  %8 = alloca %union.jvalue*, align 8
  %9 = alloca [257 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca i32, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 8
  store %struct._jobject* %1, %struct._jobject** %6, align 8
  store %struct._jmethodID* %2, %struct._jmethodID** %7, align 8
  br label %12

12:                                               ; preds = %4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %14 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %13, align 8
  %15 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %14, i32 0, i32 0
  %16 = load i8*, i8** %15, align 8
  %17 = bitcast i8* %16 to i32 (%struct.JNINativeInterface_**, %struct._jmethodID*, i8*)*
  %18 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %19 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %20 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 0
  %21 = call i32 %17(%struct.JNINativeInterface_** noundef %18, %struct._jmethodID* noundef %19, i8* noundef %20)
  store i32 %21, i32* %10, align 4
  %22 = load i32, i32* %10, align 4
  %23 = sext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  %26 = bitcast i8* %25 to %union.jvalue*
  store %union.jvalue* %26, %union.jvalue** %8, align 8
  store i32 0, i32* %11, align 4
  br label %27

27:                                               ; preds = %270, %12
  %28 = load i32, i32* %11, align 4
  %29 = load i32, i32* %10, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %273

31:                                               ; preds = %27
  %32 = load i32, i32* %11, align 4
  %33 = sext i32 %32 to i64
  %34 = getelementptr inbounds [257 x i8], [257 x i8]* %9, i64 0, i64 %33
  %35 = load i8, i8* %34, align 1
  %36 = zext i8 %35 to i32
  switch i32 %36, label %269 [
    i32 90, label %37
    i32 66, label %63
    i32 83, label %89
    i32 67, label %115
    i32 73, label %142
    i32 74, label %167
    i32 68, label %192
    i32 70, label %217
    i32 76, label %243
  ]

37:                                               ; preds = %31
  %38 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %39 = load i32, i32* %38, align 8
  %40 = icmp sge i32 %39, 0
  br i1 %40, label %49, label %41

41:                                               ; preds = %37
  %42 = add i32 %39, 8
  store i32 %42, i32* %38, align 8
  %43 = icmp sle i32 %42, 0
  br i1 %43, label %44, label %49

44:                                               ; preds = %41
  %45 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %46 = load i8*, i8** %45, align 8
  %47 = getelementptr inbounds i8, i8* %46, i32 %39
  %48 = bitcast i8* %47 to i32*
  br label %54

49:                                               ; preds = %41, %37
  %50 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %51 = load i8*, i8** %50, align 8
  %52 = getelementptr inbounds i8, i8* %51, i64 8
  store i8* %52, i8** %50, align 8
  %53 = bitcast i8* %51 to i32*
  br label %54

54:                                               ; preds = %49, %44
  %55 = phi i32* [ %48, %44 ], [ %53, %49 ]
  %56 = load i32, i32* %55, align 8
  %57 = trunc i32 %56 to i8
  %58 = load %union.jvalue*, %union.jvalue** %8, align 8
  %59 = load i32, i32* %11, align 4
  %60 = sext i32 %59 to i64
  %61 = getelementptr inbounds %union.jvalue, %union.jvalue* %58, i64 %60
  %62 = bitcast %union.jvalue* %61 to i8*
  store i8 %57, i8* %62, align 8
  br label %269

63:                                               ; preds = %31
  %64 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %65 = load i32, i32* %64, align 8
  %66 = icmp sge i32 %65, 0
  br i1 %66, label %75, label %67

67:                                               ; preds = %63
  %68 = add i32 %65, 8
  store i32 %68, i32* %64, align 8
  %69 = icmp sle i32 %68, 0
  br i1 %69, label %70, label %75

70:                                               ; preds = %67
  %71 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %72 = load i8*, i8** %71, align 8
  %73 = getelementptr inbounds i8, i8* %72, i32 %65
  %74 = bitcast i8* %73 to i32*
  br label %80

75:                                               ; preds = %67, %63
  %76 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %77 = load i8*, i8** %76, align 8
  %78 = getelementptr inbounds i8, i8* %77, i64 8
  store i8* %78, i8** %76, align 8
  %79 = bitcast i8* %77 to i32*
  br label %80

80:                                               ; preds = %75, %70
  %81 = phi i32* [ %74, %70 ], [ %79, %75 ]
  %82 = load i32, i32* %81, align 8
  %83 = trunc i32 %82 to i8
  %84 = load %union.jvalue*, %union.jvalue** %8, align 8
  %85 = load i32, i32* %11, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, %union.jvalue* %84, i64 %86
  %88 = bitcast %union.jvalue* %87 to i8*
  store i8 %83, i8* %88, align 8
  br label %269

89:                                               ; preds = %31
  %90 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %91 = load i32, i32* %90, align 8
  %92 = icmp sge i32 %91, 0
  br i1 %92, label %101, label %93

93:                                               ; preds = %89
  %94 = add i32 %91, 8
  store i32 %94, i32* %90, align 8
  %95 = icmp sle i32 %94, 0
  br i1 %95, label %96, label %101

96:                                               ; preds = %93
  %97 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %98 = load i8*, i8** %97, align 8
  %99 = getelementptr inbounds i8, i8* %98, i32 %91
  %100 = bitcast i8* %99 to i32*
  br label %106

101:                                              ; preds = %93, %89
  %102 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %103 = load i8*, i8** %102, align 8
  %104 = getelementptr inbounds i8, i8* %103, i64 8
  store i8* %104, i8** %102, align 8
  %105 = bitcast i8* %103 to i32*
  br label %106

106:                                              ; preds = %101, %96
  %107 = phi i32* [ %100, %96 ], [ %105, %101 ]
  %108 = load i32, i32* %107, align 8
  %109 = trunc i32 %108 to i16
  %110 = load %union.jvalue*, %union.jvalue** %8, align 8
  %111 = load i32, i32* %11, align 4
  %112 = sext i32 %111 to i64
  %113 = getelementptr inbounds %union.jvalue, %union.jvalue* %110, i64 %112
  %114 = bitcast %union.jvalue* %113 to i16*
  store i16 %109, i16* %114, align 8
  br label %269

115:                                              ; preds = %31
  %116 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %117 = load i32, i32* %116, align 8
  %118 = icmp sge i32 %117, 0
  br i1 %118, label %127, label %119

119:                                              ; preds = %115
  %120 = add i32 %117, 8
  store i32 %120, i32* %116, align 8
  %121 = icmp sle i32 %120, 0
  br i1 %121, label %122, label %127

122:                                              ; preds = %119
  %123 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %124 = load i8*, i8** %123, align 8
  %125 = getelementptr inbounds i8, i8* %124, i32 %117
  %126 = bitcast i8* %125 to i32*
  br label %132

127:                                              ; preds = %119, %115
  %128 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %129 = load i8*, i8** %128, align 8
  %130 = getelementptr inbounds i8, i8* %129, i64 8
  store i8* %130, i8** %128, align 8
  %131 = bitcast i8* %129 to i32*
  br label %132

132:                                              ; preds = %127, %122
  %133 = phi i32* [ %126, %122 ], [ %131, %127 ]
  %134 = load i32, i32* %133, align 8
  %135 = trunc i32 %134 to i16
  %136 = zext i16 %135 to i32
  %137 = load %union.jvalue*, %union.jvalue** %8, align 8
  %138 = load i32, i32* %11, align 4
  %139 = sext i32 %138 to i64
  %140 = getelementptr inbounds %union.jvalue, %union.jvalue* %137, i64 %139
  %141 = bitcast %union.jvalue* %140 to i32*
  store i32 %136, i32* %141, align 8
  br label %269

142:                                              ; preds = %31
  %143 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %144 = load i32, i32* %143, align 8
  %145 = icmp sge i32 %144, 0
  br i1 %145, label %154, label %146

146:                                              ; preds = %142
  %147 = add i32 %144, 8
  store i32 %147, i32* %143, align 8
  %148 = icmp sle i32 %147, 0
  br i1 %148, label %149, label %154

149:                                              ; preds = %146
  %150 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %151 = load i8*, i8** %150, align 8
  %152 = getelementptr inbounds i8, i8* %151, i32 %144
  %153 = bitcast i8* %152 to i32*
  br label %159

154:                                              ; preds = %146, %142
  %155 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %156 = load i8*, i8** %155, align 8
  %157 = getelementptr inbounds i8, i8* %156, i64 8
  store i8* %157, i8** %155, align 8
  %158 = bitcast i8* %156 to i32*
  br label %159

159:                                              ; preds = %154, %149
  %160 = phi i32* [ %153, %149 ], [ %158, %154 ]
  %161 = load i32, i32* %160, align 8
  %162 = load %union.jvalue*, %union.jvalue** %8, align 8
  %163 = load i32, i32* %11, align 4
  %164 = sext i32 %163 to i64
  %165 = getelementptr inbounds %union.jvalue, %union.jvalue* %162, i64 %164
  %166 = bitcast %union.jvalue* %165 to i32*
  store i32 %161, i32* %166, align 8
  br label %269

167:                                              ; preds = %31
  %168 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %169 = load i32, i32* %168, align 8
  %170 = icmp sge i32 %169, 0
  br i1 %170, label %179, label %171

171:                                              ; preds = %167
  %172 = add i32 %169, 8
  store i32 %172, i32* %168, align 8
  %173 = icmp sle i32 %172, 0
  br i1 %173, label %174, label %179

174:                                              ; preds = %171
  %175 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %176 = load i8*, i8** %175, align 8
  %177 = getelementptr inbounds i8, i8* %176, i32 %169
  %178 = bitcast i8* %177 to i64*
  br label %184

179:                                              ; preds = %171, %167
  %180 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %181 = load i8*, i8** %180, align 8
  %182 = getelementptr inbounds i8, i8* %181, i64 8
  store i8* %182, i8** %180, align 8
  %183 = bitcast i8* %181 to i64*
  br label %184

184:                                              ; preds = %179, %174
  %185 = phi i64* [ %178, %174 ], [ %183, %179 ]
  %186 = load i64, i64* %185, align 8
  %187 = load %union.jvalue*, %union.jvalue** %8, align 8
  %188 = load i32, i32* %11, align 4
  %189 = sext i32 %188 to i64
  %190 = getelementptr inbounds %union.jvalue, %union.jvalue* %187, i64 %189
  %191 = bitcast %union.jvalue* %190 to i64*
  store i64 %186, i64* %191, align 8
  br label %269

192:                                              ; preds = %31
  %193 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %194 = load i32, i32* %193, align 4
  %195 = icmp sge i32 %194, 0
  br i1 %195, label %204, label %196

196:                                              ; preds = %192
  %197 = add i32 %194, 16
  store i32 %197, i32* %193, align 4
  %198 = icmp sle i32 %197, 0
  br i1 %198, label %199, label %204

199:                                              ; preds = %196
  %200 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %201 = load i8*, i8** %200, align 8
  %202 = getelementptr inbounds i8, i8* %201, i32 %194
  %203 = bitcast i8* %202 to double*
  br label %209

204:                                              ; preds = %196, %192
  %205 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %206 = load i8*, i8** %205, align 8
  %207 = getelementptr inbounds i8, i8* %206, i64 8
  store i8* %207, i8** %205, align 8
  %208 = bitcast i8* %206 to double*
  br label %209

209:                                              ; preds = %204, %199
  %210 = phi double* [ %203, %199 ], [ %208, %204 ]
  %211 = load double, double* %210, align 8
  %212 = load %union.jvalue*, %union.jvalue** %8, align 8
  %213 = load i32, i32* %11, align 4
  %214 = sext i32 %213 to i64
  %215 = getelementptr inbounds %union.jvalue, %union.jvalue* %212, i64 %214
  %216 = bitcast %union.jvalue* %215 to double*
  store double %211, double* %216, align 8
  br label %269

217:                                              ; preds = %31
  %218 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 4
  %219 = load i32, i32* %218, align 4
  %220 = icmp sge i32 %219, 0
  br i1 %220, label %229, label %221

221:                                              ; preds = %217
  %222 = add i32 %219, 16
  store i32 %222, i32* %218, align 4
  %223 = icmp sle i32 %222, 0
  br i1 %223, label %224, label %229

224:                                              ; preds = %221
  %225 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 2
  %226 = load i8*, i8** %225, align 8
  %227 = getelementptr inbounds i8, i8* %226, i32 %219
  %228 = bitcast i8* %227 to double*
  br label %234

229:                                              ; preds = %221, %217
  %230 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %231 = load i8*, i8** %230, align 8
  %232 = getelementptr inbounds i8, i8* %231, i64 8
  store i8* %232, i8** %230, align 8
  %233 = bitcast i8* %231 to double*
  br label %234

234:                                              ; preds = %229, %224
  %235 = phi double* [ %228, %224 ], [ %233, %229 ]
  %236 = load double, double* %235, align 8
  %237 = fptrunc double %236 to float
  %238 = load %union.jvalue*, %union.jvalue** %8, align 8
  %239 = load i32, i32* %11, align 4
  %240 = sext i32 %239 to i64
  %241 = getelementptr inbounds %union.jvalue, %union.jvalue* %238, i64 %240
  %242 = bitcast %union.jvalue* %241 to float*
  store float %237, float* %242, align 8
  br label %269

243:                                              ; preds = %31
  %244 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 3
  %245 = load i32, i32* %244, align 8
  %246 = icmp sge i32 %245, 0
  br i1 %246, label %255, label %247

247:                                              ; preds = %243
  %248 = add i32 %245, 8
  store i32 %248, i32* %244, align 8
  %249 = icmp sle i32 %248, 0
  br i1 %249, label %250, label %255

250:                                              ; preds = %247
  %251 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 1
  %252 = load i8*, i8** %251, align 8
  %253 = getelementptr inbounds i8, i8* %252, i32 %245
  %254 = bitcast i8* %253 to i8**
  br label %260

255:                                              ; preds = %247, %243
  %256 = getelementptr inbounds %"struct.std::__va_list", %"struct.std::__va_list"* %3, i32 0, i32 0
  %257 = load i8*, i8** %256, align 8
  %258 = getelementptr inbounds i8, i8* %257, i64 8
  store i8* %258, i8** %256, align 8
  %259 = bitcast i8* %257 to i8**
  br label %260

260:                                              ; preds = %255, %250
  %261 = phi i8** [ %254, %250 ], [ %259, %255 ]
  %262 = load i8*, i8** %261, align 8
  %263 = bitcast i8* %262 to %struct._jobject*
  %264 = load %union.jvalue*, %union.jvalue** %8, align 8
  %265 = load i32, i32* %11, align 4
  %266 = sext i32 %265 to i64
  %267 = getelementptr inbounds %union.jvalue, %union.jvalue* %264, i64 %266
  %268 = bitcast %union.jvalue* %267 to %struct._jobject**
  store %struct._jobject* %263, %struct._jobject** %268, align 8
  br label %269

269:                                              ; preds = %31, %260, %234, %209, %184, %159, %132, %106, %80, %54
  br label %270

270:                                              ; preds = %269
  %271 = load i32, i32* %11, align 4
  %272 = add nsw i32 %271, 1
  store i32 %272, i32* %11, align 4
  br label %27, !llvm.loop !41

273:                                              ; preds = %27
  br label %274

274:                                              ; preds = %273
  %275 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %276 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %275, align 8
  %277 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %276, i32 0, i32 143
  %278 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, %union.jvalue*)** %277, align 8
  %279 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 8
  %280 = load %struct._jobject*, %struct._jobject** %6, align 8
  %281 = load %struct._jmethodID*, %struct._jmethodID** %7, align 8
  %282 = load %union.jvalue*, %union.jvalue** %8, align 8
  call void %278(%struct.JNINativeInterface_** noundef %279, %struct._jobject* noundef %280, %struct._jmethodID* noundef %281, %union.jvalue* noundef %282)
  ret void
}

attributes #0 = { noinline nounwind optnone uwtable "frame-pointer"="non-leaf" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+neon,+outline-atomics,+v8a" }
attributes #1 = { nofree nosync nounwind willreturn }
attributes #2 = { argmemonly nofree nounwind willreturn }

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
