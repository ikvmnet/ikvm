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
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %9 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 4
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 35
  %13 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %12, align 4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct._jobject*, %struct._jobject** %5, align 4
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %17 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %8, i32 0, i32 0
  %18 = bitcast i8** %17 to [1 x i32]*
  %19 = load [1 x i32], [1 x i32]* %18, align 4
  %20 = call %struct._jobject* %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, [1 x i32] %19)
  store %struct._jobject* %20, %struct._jobject** %7, align 4
  %21 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load %struct._jobject*, %struct._jobject** %7, align 4
  ret %struct._jobject* %22
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

; Function Attrs: noinline nounwind optnone
define dso_local %struct._jobject* @JNI_CallNonvirtualObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct._jobject*, align 4
  %10 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %1, %struct._jobject** %6, align 4
  store %struct._jobject* %2, %struct._jobject** %7, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 4
  %11 = bitcast %struct.__va_list* %10 to i8*
  call void @llvm.va_start(i8* %11)
  %12 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %13 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %12, align 4
  %14 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %13, i32 0, i32 65
  %15 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %14, align 4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %17 = load %struct._jobject*, %struct._jobject** %6, align 4
  %18 = load %struct._jobject*, %struct._jobject** %7, align 4
  %19 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %20 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %10, i32 0, i32 0
  %21 = bitcast i8** %20 to [1 x i32]*
  %22 = load [1 x i32], [1 x i32]* %21, align 4
  %23 = call %struct._jobject* %15(%struct.JNINativeInterface_** noundef %16, %struct._jobject* noundef %17, %struct._jobject* noundef %18, %struct._jmethodID* noundef %19, [1 x i32] %22)
  store %struct._jobject* %23, %struct._jobject** %9, align 4
  %24 = bitcast %struct.__va_list* %10 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load %struct._jobject*, %struct._jobject** %9, align 4
  ret %struct._jobject* %25
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local %struct._jobject* @JNI_CallStaticObjectMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %9 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 4
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 115
  %13 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %12, align 4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct._jobject*, %struct._jobject** %5, align 4
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %17 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %8, i32 0, i32 0
  %18 = bitcast i8** %17 to [1 x i32]*
  %19 = load [1 x i32], [1 x i32]* %18, align 4
  %20 = call %struct._jobject* %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, [1 x i32] %19)
  store %struct._jobject* %20, %struct._jobject** %7, align 4
  %21 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load %struct._jobject*, %struct._jobject** %7, align 4
  ret %struct._jobject* %22
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i8 @JNI_CallBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca i8, align 1
  %8 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %9 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 4
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 38
  %13 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %12, align 4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct._jobject*, %struct._jobject** %5, align 4
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %17 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %8, i32 0, i32 0
  %18 = bitcast i8** %17 to [1 x i32]*
  %19 = load [1 x i32], [1 x i32]* %18, align 4
  %20 = call zeroext i8 %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, [1 x i32] %19)
  store i8 %20, i8* %7, align 1
  %21 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i8, i8* %7, align 1
  ret i8 %22
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i8 @JNI_CallNonvirtualBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca i8, align 1
  %10 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %1, %struct._jobject** %6, align 4
  store %struct._jobject* %2, %struct._jobject** %7, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 4
  %11 = bitcast %struct.__va_list* %10 to i8*
  call void @llvm.va_start(i8* %11)
  %12 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %13 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %12, align 4
  %14 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %13, i32 0, i32 68
  %15 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %14, align 4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %17 = load %struct._jobject*, %struct._jobject** %6, align 4
  %18 = load %struct._jobject*, %struct._jobject** %7, align 4
  %19 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %20 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %10, i32 0, i32 0
  %21 = bitcast i8** %20 to [1 x i32]*
  %22 = load [1 x i32], [1 x i32]* %21, align 4
  %23 = call zeroext i8 %15(%struct.JNINativeInterface_** noundef %16, %struct._jobject* noundef %17, %struct._jobject* noundef %18, %struct._jmethodID* noundef %19, [1 x i32] %22)
  store i8 %23, i8* %9, align 1
  %24 = bitcast %struct.__va_list* %10 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load i8, i8* %9, align 1
  ret i8 %25
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i8 @JNI_CallStaticBooleanMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca i8, align 1
  %8 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %9 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 4
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 118
  %13 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %12, align 4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct._jobject*, %struct._jobject** %5, align 4
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %17 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %8, i32 0, i32 0
  %18 = bitcast i8** %17 to [1 x i32]*
  %19 = load [1 x i32], [1 x i32]* %18, align 4
  %20 = call zeroext i8 %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, [1 x i32] %19)
  store i8 %20, i8* %7, align 1
  %21 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i8, i8* %7, align 1
  ret i8 %22
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local signext i8 @JNI_CallByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca i8, align 1
  %8 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %9 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 4
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 41
  %13 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %12, align 4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct._jobject*, %struct._jobject** %5, align 4
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %17 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %8, i32 0, i32 0
  %18 = bitcast i8** %17 to [1 x i32]*
  %19 = load [1 x i32], [1 x i32]* %18, align 4
  %20 = call signext i8 %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, [1 x i32] %19)
  store i8 %20, i8* %7, align 1
  %21 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i8, i8* %7, align 1
  ret i8 %22
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local signext i8 @JNI_CallNonvirtualByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca i8, align 1
  %10 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %1, %struct._jobject** %6, align 4
  store %struct._jobject* %2, %struct._jobject** %7, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 4
  %11 = bitcast %struct.__va_list* %10 to i8*
  call void @llvm.va_start(i8* %11)
  %12 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %13 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %12, align 4
  %14 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %13, i32 0, i32 71
  %15 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %14, align 4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %17 = load %struct._jobject*, %struct._jobject** %6, align 4
  %18 = load %struct._jobject*, %struct._jobject** %7, align 4
  %19 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %20 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %10, i32 0, i32 0
  %21 = bitcast i8** %20 to [1 x i32]*
  %22 = load [1 x i32], [1 x i32]* %21, align 4
  %23 = call signext i8 %15(%struct.JNINativeInterface_** noundef %16, %struct._jobject* noundef %17, %struct._jobject* noundef %18, %struct._jmethodID* noundef %19, [1 x i32] %22)
  store i8 %23, i8* %9, align 1
  %24 = bitcast %struct.__va_list* %10 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load i8, i8* %9, align 1
  ret i8 %25
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local signext i8 @JNI_CallStaticByteMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca i8, align 1
  %8 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %9 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 4
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 121
  %13 = load i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i8 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %12, align 4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct._jobject*, %struct._jobject** %5, align 4
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %17 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %8, i32 0, i32 0
  %18 = bitcast i8** %17 to [1 x i32]*
  %19 = load [1 x i32], [1 x i32]* %18, align 4
  %20 = call signext i8 %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, [1 x i32] %19)
  store i8 %20, i8* %7, align 1
  %21 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i8, i8* %7, align 1
  ret i8 %22
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i16 @JNI_CallCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca i16, align 2
  %8 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %9 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 4
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 44
  %13 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %12, align 4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct._jobject*, %struct._jobject** %5, align 4
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %17 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %8, i32 0, i32 0
  %18 = bitcast i8** %17 to [1 x i32]*
  %19 = load [1 x i32], [1 x i32]* %18, align 4
  %20 = call zeroext i16 %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, [1 x i32] %19)
  store i16 %20, i16* %7, align 2
  %21 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i16, i16* %7, align 2
  ret i16 %22
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i16 @JNI_CallNonvirtualCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca i16, align 2
  %10 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %1, %struct._jobject** %6, align 4
  store %struct._jobject* %2, %struct._jobject** %7, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 4
  %11 = bitcast %struct.__va_list* %10 to i8*
  call void @llvm.va_start(i8* %11)
  %12 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %13 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %12, align 4
  %14 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %13, i32 0, i32 74
  %15 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %14, align 4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %17 = load %struct._jobject*, %struct._jobject** %6, align 4
  %18 = load %struct._jobject*, %struct._jobject** %7, align 4
  %19 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %20 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %10, i32 0, i32 0
  %21 = bitcast i8** %20 to [1 x i32]*
  %22 = load [1 x i32], [1 x i32]* %21, align 4
  %23 = call zeroext i16 %15(%struct.JNINativeInterface_** noundef %16, %struct._jobject* noundef %17, %struct._jobject* noundef %18, %struct._jmethodID* noundef %19, [1 x i32] %22)
  store i16 %23, i16* %9, align 2
  %24 = bitcast %struct.__va_list* %10 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load i16, i16* %9, align 2
  ret i16 %25
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local zeroext i16 @JNI_CallStaticCharMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca i16, align 2
  %8 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %9 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 4
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 124
  %13 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %12, align 4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct._jobject*, %struct._jobject** %5, align 4
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %17 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %8, i32 0, i32 0
  %18 = bitcast i8** %17 to [1 x i32]*
  %19 = load [1 x i32], [1 x i32]* %18, align 4
  %20 = call zeroext i16 %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, [1 x i32] %19)
  store i16 %20, i16* %7, align 2
  %21 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i16, i16* %7, align 2
  ret i16 %22
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local signext i16 @JNI_CallShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca i16, align 2
  %8 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %9 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 4
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 47
  %13 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %12, align 4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct._jobject*, %struct._jobject** %5, align 4
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %17 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %8, i32 0, i32 0
  %18 = bitcast i8** %17 to [1 x i32]*
  %19 = load [1 x i32], [1 x i32]* %18, align 4
  %20 = call signext i16 %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, [1 x i32] %19)
  store i16 %20, i16* %7, align 2
  %21 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i16, i16* %7, align 2
  ret i16 %22
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local signext i16 @JNI_CallNonvirtualShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca i16, align 2
  %10 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %1, %struct._jobject** %6, align 4
  store %struct._jobject* %2, %struct._jobject** %7, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 4
  %11 = bitcast %struct.__va_list* %10 to i8*
  call void @llvm.va_start(i8* %11)
  %12 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %13 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %12, align 4
  %14 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %13, i32 0, i32 77
  %15 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %14, align 4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %17 = load %struct._jobject*, %struct._jobject** %6, align 4
  %18 = load %struct._jobject*, %struct._jobject** %7, align 4
  %19 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %20 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %10, i32 0, i32 0
  %21 = bitcast i8** %20 to [1 x i32]*
  %22 = load [1 x i32], [1 x i32]* %21, align 4
  %23 = call signext i16 %15(%struct.JNINativeInterface_** noundef %16, %struct._jobject* noundef %17, %struct._jobject* noundef %18, %struct._jmethodID* noundef %19, [1 x i32] %22)
  store i16 %23, i16* %9, align 2
  %24 = bitcast %struct.__va_list* %10 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load i16, i16* %9, align 2
  ret i16 %25
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local signext i16 @JNI_CallStaticShortMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca i16, align 2
  %8 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %9 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 4
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 127
  %13 = load i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i16 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %12, align 4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct._jobject*, %struct._jobject** %5, align 4
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %17 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %8, i32 0, i32 0
  %18 = bitcast i8** %17 to [1 x i32]*
  %19 = load [1 x i32], [1 x i32]* %18, align 4
  %20 = call signext i16 %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, [1 x i32] %19)
  store i16 %20, i16* %7, align 2
  %21 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i16, i16* %7, align 2
  ret i16 %22
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local i32 @JNI_CallIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca i32, align 4
  %8 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %9 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 4
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 50
  %13 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %12, align 4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct._jobject*, %struct._jobject** %5, align 4
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %17 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %8, i32 0, i32 0
  %18 = bitcast i8** %17 to [1 x i32]*
  %19 = load [1 x i32], [1 x i32]* %18, align 4
  %20 = call i32 %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, [1 x i32] %19)
  store i32 %20, i32* %7, align 4
  %21 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i32, i32* %7, align 4
  ret i32 %22
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local i32 @JNI_CallNonvirtualIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca i32, align 4
  %10 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %1, %struct._jobject** %6, align 4
  store %struct._jobject* %2, %struct._jobject** %7, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 4
  %11 = bitcast %struct.__va_list* %10 to i8*
  call void @llvm.va_start(i8* %11)
  %12 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %13 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %12, align 4
  %14 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %13, i32 0, i32 80
  %15 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %14, align 4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %17 = load %struct._jobject*, %struct._jobject** %6, align 4
  %18 = load %struct._jobject*, %struct._jobject** %7, align 4
  %19 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %20 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %10, i32 0, i32 0
  %21 = bitcast i8** %20 to [1 x i32]*
  %22 = load [1 x i32], [1 x i32]* %21, align 4
  %23 = call i32 %15(%struct.JNINativeInterface_** noundef %16, %struct._jobject* noundef %17, %struct._jobject* noundef %18, %struct._jmethodID* noundef %19, [1 x i32] %22)
  store i32 %23, i32* %9, align 4
  %24 = bitcast %struct.__va_list* %10 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load i32, i32* %9, align 4
  ret i32 %25
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local i32 @JNI_CallStaticIntMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca i32, align 4
  %8 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %9 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 4
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 130
  %13 = load i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i32 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %12, align 4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct._jobject*, %struct._jobject** %5, align 4
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %17 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %8, i32 0, i32 0
  %18 = bitcast i8** %17 to [1 x i32]*
  %19 = load [1 x i32], [1 x i32]* %18, align 4
  %20 = call i32 %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, [1 x i32] %19)
  store i32 %20, i32* %7, align 4
  %21 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i32, i32* %7, align 4
  ret i32 %22
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local i64 @JNI_CallLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca i64, align 8
  %8 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %9 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 4
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 53
  %13 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %12, align 4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct._jobject*, %struct._jobject** %5, align 4
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %17 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %8, i32 0, i32 0
  %18 = bitcast i8** %17 to [1 x i32]*
  %19 = load [1 x i32], [1 x i32]* %18, align 4
  %20 = call i64 %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, [1 x i32] %19)
  store i64 %20, i64* %7, align 8
  %21 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i64, i64* %7, align 8
  ret i64 %22
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local i64 @JNI_CallNonvirtualLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca i64, align 8
  %10 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %1, %struct._jobject** %6, align 4
  store %struct._jobject* %2, %struct._jobject** %7, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 4
  %11 = bitcast %struct.__va_list* %10 to i8*
  call void @llvm.va_start(i8* %11)
  %12 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %13 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %12, align 4
  %14 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %13, i32 0, i32 83
  %15 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %14, align 4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %17 = load %struct._jobject*, %struct._jobject** %6, align 4
  %18 = load %struct._jobject*, %struct._jobject** %7, align 4
  %19 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %20 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %10, i32 0, i32 0
  %21 = bitcast i8** %20 to [1 x i32]*
  %22 = load [1 x i32], [1 x i32]* %21, align 4
  %23 = call i64 %15(%struct.JNINativeInterface_** noundef %16, %struct._jobject* noundef %17, %struct._jobject* noundef %18, %struct._jmethodID* noundef %19, [1 x i32] %22)
  store i64 %23, i64* %9, align 8
  %24 = bitcast %struct.__va_list* %10 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load i64, i64* %9, align 8
  ret i64 %25
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local i64 @JNI_CallStaticLongMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca i64, align 8
  %8 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %9 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 4
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 133
  %13 = load i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, i64 (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %12, align 4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct._jobject*, %struct._jobject** %5, align 4
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %17 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %8, i32 0, i32 0
  %18 = bitcast i8** %17 to [1 x i32]*
  %19 = load [1 x i32], [1 x i32]* %18, align 4
  %20 = call i64 %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, [1 x i32] %19)
  store i64 %20, i64* %7, align 8
  %21 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load i64, i64* %7, align 8
  ret i64 %22
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local float @JNI_CallFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca float, align 4
  %8 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %9 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 4
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 56
  %13 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %12, align 4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct._jobject*, %struct._jobject** %5, align 4
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %17 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %8, i32 0, i32 0
  %18 = bitcast i8** %17 to [1 x i32]*
  %19 = load [1 x i32], [1 x i32]* %18, align 4
  %20 = call float %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, [1 x i32] %19)
  store float %20, float* %7, align 4
  %21 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load float, float* %7, align 4
  ret float %22
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local float @JNI_CallNonvirtualFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca float, align 4
  %10 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %1, %struct._jobject** %6, align 4
  store %struct._jobject* %2, %struct._jobject** %7, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 4
  %11 = bitcast %struct.__va_list* %10 to i8*
  call void @llvm.va_start(i8* %11)
  %12 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %13 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %12, align 4
  %14 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %13, i32 0, i32 86
  %15 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %14, align 4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %17 = load %struct._jobject*, %struct._jobject** %6, align 4
  %18 = load %struct._jobject*, %struct._jobject** %7, align 4
  %19 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %20 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %10, i32 0, i32 0
  %21 = bitcast i8** %20 to [1 x i32]*
  %22 = load [1 x i32], [1 x i32]* %21, align 4
  %23 = call float %15(%struct.JNINativeInterface_** noundef %16, %struct._jobject* noundef %17, %struct._jobject* noundef %18, %struct._jmethodID* noundef %19, [1 x i32] %22)
  store float %23, float* %9, align 4
  %24 = bitcast %struct.__va_list* %10 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load float, float* %9, align 4
  ret float %25
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local float @JNI_CallStaticFloatMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca float, align 4
  %8 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %9 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 4
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 136
  %13 = load float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, float (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %12, align 4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct._jobject*, %struct._jobject** %5, align 4
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %17 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %8, i32 0, i32 0
  %18 = bitcast i8** %17 to [1 x i32]*
  %19 = load [1 x i32], [1 x i32]* %18, align 4
  %20 = call float %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, [1 x i32] %19)
  store float %20, float* %7, align 4
  %21 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load float, float* %7, align 4
  ret float %22
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local double @JNI_CallDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca double, align 8
  %8 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %9 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 4
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 59
  %13 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %12, align 4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct._jobject*, %struct._jobject** %5, align 4
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %17 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %8, i32 0, i32 0
  %18 = bitcast i8** %17 to [1 x i32]*
  %19 = load [1 x i32], [1 x i32]* %18, align 4
  %20 = call double %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, [1 x i32] %19)
  store double %20, double* %7, align 8
  %21 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load double, double* %7, align 8
  ret double %22
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local double @JNI_CallNonvirtualDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca double, align 8
  %10 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %1, %struct._jobject** %6, align 4
  store %struct._jobject* %2, %struct._jobject** %7, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 4
  %11 = bitcast %struct.__va_list* %10 to i8*
  call void @llvm.va_start(i8* %11)
  %12 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %13 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %12, align 4
  %14 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %13, i32 0, i32 89
  %15 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %14, align 4
  %16 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %17 = load %struct._jobject*, %struct._jobject** %6, align 4
  %18 = load %struct._jobject*, %struct._jobject** %7, align 4
  %19 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %20 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %10, i32 0, i32 0
  %21 = bitcast i8** %20 to [1 x i32]*
  %22 = load [1 x i32], [1 x i32]* %21, align 4
  %23 = call double %15(%struct.JNINativeInterface_** noundef %16, %struct._jobject* noundef %17, %struct._jobject* noundef %18, %struct._jmethodID* noundef %19, [1 x i32] %22)
  store double %23, double* %9, align 8
  %24 = bitcast %struct.__va_list* %10 to i8*
  call void @llvm.va_end(i8* %24)
  %25 = load double, double* %9, align 8
  ret double %25
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local double @JNI_CallStaticDoubleMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca double, align 8
  %8 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %9 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 4
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 139
  %13 = load double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, double (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %12, align 4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct._jobject*, %struct._jobject** %5, align 4
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %17 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %8, i32 0, i32 0
  %18 = bitcast i8** %17 to [1 x i32]*
  %19 = load [1 x i32], [1 x i32]* %18, align 4
  %20 = call double %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, [1 x i32] %19)
  store double %20, double* %7, align 8
  %21 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load double, double* %7, align 8
  ret double %22
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local %struct._jobject* @JNI_NewObject(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %9 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_start(i8* %9)
  %10 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %11 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %10, align 4
  %12 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %11, i32 0, i32 29
  %13 = load %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, %struct._jobject* (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %12, align 4
  %14 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %15 = load %struct._jobject*, %struct._jobject** %5, align 4
  %16 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %17 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %8, i32 0, i32 0
  %18 = bitcast i8** %17 to [1 x i32]*
  %19 = load [1 x i32], [1 x i32]* %18, align 4
  %20 = call %struct._jobject* %13(%struct.JNINativeInterface_** noundef %14, %struct._jobject* noundef %15, %struct._jmethodID* noundef %16, [1 x i32] %19)
  store %struct._jobject* %20, %struct._jobject** %7, align 4
  %21 = bitcast %struct.__va_list* %8 to i8*
  call void @llvm.va_end(i8* %21)
  %22 = load %struct._jobject*, %struct._jobject** %7, align 4
  ret %struct._jobject* %22
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local void @JNI_CallVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %8 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %8)
  %9 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %10 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %10, i32 0, i32 62
  %12 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %11, align 4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %14 = load %struct._jobject*, %struct._jobject** %5, align 4
  %15 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %16 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %7, i32 0, i32 0
  %17 = bitcast i8** %16 to [1 x i32]*
  %18 = load [1 x i32], [1 x i32]* %17, align 4
  call void %12(%struct.JNINativeInterface_** noundef %13, %struct._jobject* noundef %14, %struct._jmethodID* noundef %15, [1 x i32] %18)
  %19 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %19)
  ret void
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local void @JNI_CallNonvirtualVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jobject* noundef %2, %struct._jmethodID* noundef %3, ...) #0 {
  %5 = alloca %struct.JNINativeInterface_**, align 4
  %6 = alloca %struct._jobject*, align 4
  %7 = alloca %struct._jobject*, align 4
  %8 = alloca %struct._jmethodID*, align 4
  %9 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %5, align 4
  store %struct._jobject* %1, %struct._jobject** %6, align 4
  store %struct._jobject* %2, %struct._jobject** %7, align 4
  store %struct._jmethodID* %3, %struct._jmethodID** %8, align 4
  %10 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_start(i8* %10)
  %11 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %12 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %11, align 4
  %13 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %12, i32 0, i32 92
  %14 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %13, align 4
  %15 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %5, align 4
  %16 = load %struct._jobject*, %struct._jobject** %6, align 4
  %17 = load %struct._jobject*, %struct._jobject** %7, align 4
  %18 = load %struct._jmethodID*, %struct._jmethodID** %8, align 4
  %19 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %9, i32 0, i32 0
  %20 = bitcast i8** %19 to [1 x i32]*
  %21 = load [1 x i32], [1 x i32]* %20, align 4
  call void %14(%struct.JNINativeInterface_** noundef %15, %struct._jobject* noundef %16, %struct._jobject* noundef %17, %struct._jmethodID* noundef %18, [1 x i32] %21)
  %22 = bitcast %struct.__va_list* %9 to i8*
  call void @llvm.va_end(i8* %22)
  ret void
}

; Function Attrs: noinline nounwind optnone
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

; Function Attrs: noinline nounwind optnone
define dso_local void @JNI_CallStaticVoidMethod(%struct.JNINativeInterface_** noundef %0, %struct._jobject* noundef %1, %struct._jmethodID* noundef %2, ...) #0 {
  %4 = alloca %struct.JNINativeInterface_**, align 4
  %5 = alloca %struct._jobject*, align 4
  %6 = alloca %struct._jmethodID*, align 4
  %7 = alloca %struct.__va_list, align 4
  store %struct.JNINativeInterface_** %0, %struct.JNINativeInterface_*** %4, align 4
  store %struct._jobject* %1, %struct._jobject** %5, align 4
  store %struct._jmethodID* %2, %struct._jmethodID** %6, align 4
  %8 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_start(i8* %8)
  %9 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %10 = load %struct.JNINativeInterface_*, %struct.JNINativeInterface_** %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, %struct.JNINativeInterface_* %10, i32 0, i32 142
  %12 = load void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])*, void (%struct.JNINativeInterface_**, %struct._jobject*, %struct._jmethodID*, [1 x i32])** %11, align 4
  %13 = load %struct.JNINativeInterface_**, %struct.JNINativeInterface_*** %4, align 4
  %14 = load %struct._jobject*, %struct._jobject** %5, align 4
  %15 = load %struct._jmethodID*, %struct._jmethodID** %6, align 4
  %16 = getelementptr inbounds %struct.__va_list, %struct.__va_list* %7, i32 0, i32 0
  %17 = bitcast i8** %16 to [1 x i32]*
  %18 = load [1 x i32], [1 x i32]* %17, align 4
  call void %12(%struct.JNINativeInterface_** noundef %13, %struct._jobject* noundef %14, %struct._jmethodID* noundef %15, [1 x i32] %18)
  %19 = bitcast %struct.__va_list* %7 to i8*
  call void @llvm.va_end(i8* %19)
  ret void
}

; Function Attrs: noinline nounwind optnone
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
