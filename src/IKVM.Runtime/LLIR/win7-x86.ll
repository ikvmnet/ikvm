; ModuleID = 'jni.c'
source_filename = "jni.c"
target datalayout = "e-m:x-p:32:32-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32-a:0:32-S32"
target triple = "i686-pc-windows-msvc19.34.31933"

%struct.JNINativeInterface_ = type { ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr }
%union.jvalue = type { i64 }

$sprintf = comdat any

$vsprintf = comdat any

$_snprintf = comdat any

$_vsnprintf = comdat any

$_vsprintf_l = comdat any

$_vsnprintf_l = comdat any

$__local_stdio_printf_options = comdat any

@__local_stdio_printf_options._OptionsStorage = internal global i64 0, align 8

; Function Attrs: noinline nounwind optnone
define linkonce_odr dso_local i32 @sprintf(ptr noundef %0, ptr noundef %1, ...) #0 comdat {
  %3 = alloca ptr, align 4
  %4 = alloca ptr, align 4
  %5 = alloca i32, align 4
  %6 = alloca ptr, align 4
  store ptr %1, ptr %3, align 4
  store ptr %0, ptr %4, align 4
  call void @llvm.va_start(ptr %6)
  %7 = load ptr, ptr %6, align 4
  %8 = load ptr, ptr %3, align 4
  %9 = load ptr, ptr %4, align 4
  %10 = call i32 @_vsprintf_l(ptr noundef %9, ptr noundef %8, ptr noundef null, ptr noundef %7)
  store i32 %10, ptr %5, align 4
  call void @llvm.va_end(ptr %6)
  %11 = load i32, ptr %5, align 4
  ret i32 %11
}

; Function Attrs: noinline nounwind optnone
define linkonce_odr dso_local i32 @vsprintf(ptr noundef %0, ptr noundef %1, ptr noundef %2) #0 comdat {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  %7 = load ptr, ptr %4, align 4
  %8 = load ptr, ptr %5, align 4
  %9 = load ptr, ptr %6, align 4
  %10 = call i32 @_vsnprintf_l(ptr noundef %9, i32 noundef -1, ptr noundef %8, ptr noundef null, ptr noundef %7)
  ret i32 %10
}

; Function Attrs: noinline nounwind optnone
define linkonce_odr dso_local i32 @_snprintf(ptr noundef %0, i32 noundef %1, ptr noundef %2, ...) #0 comdat {
  %4 = alloca ptr, align 4
  %5 = alloca i32, align 4
  %6 = alloca ptr, align 4
  %7 = alloca i32, align 4
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store i32 %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %8, align 4
  %10 = load ptr, ptr %4, align 4
  %11 = load i32, ptr %5, align 4
  %12 = load ptr, ptr %6, align 4
  %13 = call i32 @_vsnprintf(ptr noundef %12, i32 noundef %11, ptr noundef %10, ptr noundef %9)
  store i32 %13, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %14 = load i32, ptr %7, align 4
  ret i32 %14
}

; Function Attrs: noinline nounwind optnone
define linkonce_odr dso_local i32 @_vsnprintf(ptr noundef %0, i32 noundef %1, ptr noundef %2, ptr noundef %3) #0 comdat {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca i32, align 4
  %8 = alloca ptr, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store i32 %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  %9 = load ptr, ptr %5, align 4
  %10 = load ptr, ptr %6, align 4
  %11 = load i32, ptr %7, align 4
  %12 = load ptr, ptr %8, align 4
  %13 = call i32 @_vsnprintf_l(ptr noundef %12, i32 noundef %11, ptr noundef %10, ptr noundef null, ptr noundef %9)
  ret i32 %13
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport ptr @JNI_CallObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 4
  %10 = load ptr, ptr %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 35
  %12 = load ptr, ptr %11, align 4
  %13 = load ptr, ptr %8, align 4
  %14 = load ptr, ptr %4, align 4
  %15 = load ptr, ptr %5, align 4
  %16 = load ptr, ptr %6, align 4
  %17 = call x86_stdcallcc ptr %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store ptr %17, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %18 = load ptr, ptr %7, align 4
  ret ptr %18
}

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.va_start(ptr) #1

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.va_end(ptr) #1

; Function Attrs: noinline nounwind optnone
define dso_local dllexport ptr @JNI_CallObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !4

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 36
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  %118 = call x86_stdcallcc ptr %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret ptr %118
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport ptr @JNI_CallNonvirtualObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  call void @llvm.va_start(ptr %10)
  %11 = load ptr, ptr %8, align 4
  %12 = load ptr, ptr %11, align 4
  %13 = getelementptr inbounds %struct.JNINativeInterface_, ptr %12, i32 0, i32 65
  %14 = load ptr, ptr %13, align 4
  %15 = load ptr, ptr %10, align 4
  %16 = load ptr, ptr %5, align 4
  %17 = load ptr, ptr %6, align 4
  %18 = load ptr, ptr %7, align 4
  %19 = load ptr, ptr %8, align 4
  %20 = call x86_stdcallcc ptr %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store ptr %20, ptr %9, align 4
  call void @llvm.va_end(ptr %10)
  %21 = load ptr, ptr %9, align 4
  ret ptr %21
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport ptr @JNI_CallNonvirtualObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  %11 = alloca ptr, align 4
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  store ptr %3, ptr %7, align 4
  store ptr %2, ptr %8, align 4
  store ptr %1, ptr %9, align 4
  store ptr %0, ptr %10, align 4
  br label %15

15:                                               ; preds = %5
  %16 = load ptr, ptr %10, align 4
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 4
  %20 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 0
  %21 = load ptr, ptr %7, align 4
  %22 = load ptr, ptr %10, align 4
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = mul i32 %24, 8
  %26 = alloca i8, i32 %25, align 16
  store ptr %26, ptr %11, align 4
  store i32 0, ptr %14, align 4
  br label %27

27:                                               ; preds = %107, %15
  %28 = load i32, ptr %14, align 4
  %29 = load i32, ptr %13, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %110

31:                                               ; preds = %27
  %32 = load i32, ptr %14, align 4
  %33 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %106 [
    i32 90, label %36
    i32 66, label %44
    i32 83, label %52
    i32 67, label %60
    i32 73, label %69
    i32 74, label %76
    i32 68, label %84
    i32 70, label %91
    i32 76, label %99
  ]

36:                                               ; preds = %31
  %37 = load ptr, ptr %6, align 4
  %38 = getelementptr inbounds i8, ptr %37, i32 4
  store ptr %38, ptr %6, align 4
  %39 = load i32, ptr %37, align 4
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %11, align 4
  %42 = load i32, ptr %14, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %41, i32 %42
  store i8 %40, ptr %43, align 8
  br label %106

44:                                               ; preds = %31
  %45 = load ptr, ptr %6, align 4
  %46 = getelementptr inbounds i8, ptr %45, i32 4
  store ptr %46, ptr %6, align 4
  %47 = load i32, ptr %45, align 4
  %48 = trunc i32 %47 to i8
  %49 = load ptr, ptr %11, align 4
  %50 = load i32, ptr %14, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %49, i32 %50
  store i8 %48, ptr %51, align 8
  br label %106

52:                                               ; preds = %31
  %53 = load ptr, ptr %6, align 4
  %54 = getelementptr inbounds i8, ptr %53, i32 4
  store ptr %54, ptr %6, align 4
  %55 = load i32, ptr %53, align 4
  %56 = trunc i32 %55 to i16
  %57 = load ptr, ptr %11, align 4
  %58 = load i32, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %57, i32 %58
  store i16 %56, ptr %59, align 8
  br label %106

60:                                               ; preds = %31
  %61 = load ptr, ptr %6, align 4
  %62 = getelementptr inbounds i8, ptr %61, i32 4
  store ptr %62, ptr %6, align 4
  %63 = load i32, ptr %61, align 4
  %64 = trunc i32 %63 to i16
  %65 = zext i16 %64 to i32
  %66 = load ptr, ptr %11, align 4
  %67 = load i32, ptr %14, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %66, i32 %67
  store i32 %65, ptr %68, align 8
  br label %106

69:                                               ; preds = %31
  %70 = load ptr, ptr %6, align 4
  %71 = getelementptr inbounds i8, ptr %70, i32 4
  store ptr %71, ptr %6, align 4
  %72 = load i32, ptr %70, align 4
  %73 = load ptr, ptr %11, align 4
  %74 = load i32, ptr %14, align 4
  %75 = getelementptr inbounds %union.jvalue, ptr %73, i32 %74
  store i32 %72, ptr %75, align 8
  br label %106

76:                                               ; preds = %31
  %77 = load ptr, ptr %6, align 4
  %78 = getelementptr inbounds i8, ptr %77, i32 4
  store ptr %78, ptr %6, align 4
  %79 = load i32, ptr %77, align 4
  %80 = sext i32 %79 to i64
  %81 = load ptr, ptr %11, align 4
  %82 = load i32, ptr %14, align 4
  %83 = getelementptr inbounds %union.jvalue, ptr %81, i32 %82
  store i64 %80, ptr %83, align 8
  br label %106

84:                                               ; preds = %31
  %85 = load ptr, ptr %6, align 4
  %86 = getelementptr inbounds i8, ptr %85, i32 8
  store ptr %86, ptr %6, align 4
  %87 = load double, ptr %85, align 4
  %88 = load ptr, ptr %11, align 4
  %89 = load i32, ptr %14, align 4
  %90 = getelementptr inbounds %union.jvalue, ptr %88, i32 %89
  store double %87, ptr %90, align 8
  br label %106

91:                                               ; preds = %31
  %92 = load ptr, ptr %6, align 4
  %93 = getelementptr inbounds i8, ptr %92, i32 8
  store ptr %93, ptr %6, align 4
  %94 = load double, ptr %92, align 4
  %95 = fptrunc double %94 to float
  %96 = load ptr, ptr %11, align 4
  %97 = load i32, ptr %14, align 4
  %98 = getelementptr inbounds %union.jvalue, ptr %96, i32 %97
  store float %95, ptr %98, align 8
  br label %106

99:                                               ; preds = %31
  %100 = load ptr, ptr %6, align 4
  %101 = getelementptr inbounds i8, ptr %100, i32 4
  store ptr %101, ptr %6, align 4
  %102 = load ptr, ptr %100, align 4
  %103 = load ptr, ptr %11, align 4
  %104 = load i32, ptr %14, align 4
  %105 = getelementptr inbounds %union.jvalue, ptr %103, i32 %104
  store ptr %102, ptr %105, align 8
  br label %106

106:                                              ; preds = %31, %99, %91, %84, %76, %69, %60, %52, %44, %36
  br label %107

107:                                              ; preds = %106
  %108 = load i32, ptr %14, align 4
  %109 = add nsw i32 %108, 1
  store i32 %109, ptr %14, align 4
  br label %27, !llvm.loop !6

110:                                              ; preds = %27
  br label %111

111:                                              ; preds = %110
  %112 = load ptr, ptr %10, align 4
  %113 = load ptr, ptr %112, align 4
  %114 = getelementptr inbounds %struct.JNINativeInterface_, ptr %113, i32 0, i32 66
  %115 = load ptr, ptr %114, align 4
  %116 = load ptr, ptr %11, align 4
  %117 = load ptr, ptr %7, align 4
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %9, align 4
  %120 = load ptr, ptr %10, align 4
  %121 = call x86_stdcallcc ptr %115(ptr noundef %120, ptr noundef %119, ptr noundef %118, ptr noundef %117, ptr noundef %116)
  ret ptr %121
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport ptr @JNI_CallStaticObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 4
  %10 = load ptr, ptr %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 115
  %12 = load ptr, ptr %11, align 4
  %13 = load ptr, ptr %8, align 4
  %14 = load ptr, ptr %4, align 4
  %15 = load ptr, ptr %5, align 4
  %16 = load ptr, ptr %6, align 4
  %17 = call x86_stdcallcc ptr %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store ptr %17, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %18 = load ptr, ptr %7, align 4
  ret ptr %18
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport ptr @JNI_CallStaticObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !7

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 116
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  %118 = call x86_stdcallcc ptr %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret ptr %118
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport zeroext i8 @JNI_CallBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca i8, align 1
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 4
  %10 = load ptr, ptr %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 38
  %12 = load ptr, ptr %11, align 4
  %13 = load ptr, ptr %8, align 4
  %14 = load ptr, ptr %4, align 4
  %15 = load ptr, ptr %5, align 4
  %16 = load ptr, ptr %6, align 4
  %17 = call x86_stdcallcc zeroext i8 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i8 %17, ptr %7, align 1
  call void @llvm.va_end(ptr %8)
  %18 = load i8, ptr %7, align 1
  ret i8 %18
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport zeroext i8 @JNI_CallBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !8

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 39
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  %118 = call x86_stdcallcc zeroext i8 %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret i8 %118
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport zeroext i8 @JNI_CallNonvirtualBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca i8, align 1
  %10 = alloca ptr, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  call void @llvm.va_start(ptr %10)
  %11 = load ptr, ptr %8, align 4
  %12 = load ptr, ptr %11, align 4
  %13 = getelementptr inbounds %struct.JNINativeInterface_, ptr %12, i32 0, i32 68
  %14 = load ptr, ptr %13, align 4
  %15 = load ptr, ptr %10, align 4
  %16 = load ptr, ptr %5, align 4
  %17 = load ptr, ptr %6, align 4
  %18 = load ptr, ptr %7, align 4
  %19 = load ptr, ptr %8, align 4
  %20 = call x86_stdcallcc zeroext i8 %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store i8 %20, ptr %9, align 1
  call void @llvm.va_end(ptr %10)
  %21 = load i8, ptr %9, align 1
  ret i8 %21
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport zeroext i8 @JNI_CallNonvirtualBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  %11 = alloca ptr, align 4
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  store ptr %3, ptr %7, align 4
  store ptr %2, ptr %8, align 4
  store ptr %1, ptr %9, align 4
  store ptr %0, ptr %10, align 4
  br label %15

15:                                               ; preds = %5
  %16 = load ptr, ptr %10, align 4
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 4
  %20 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 0
  %21 = load ptr, ptr %7, align 4
  %22 = load ptr, ptr %10, align 4
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = mul i32 %24, 8
  %26 = alloca i8, i32 %25, align 16
  store ptr %26, ptr %11, align 4
  store i32 0, ptr %14, align 4
  br label %27

27:                                               ; preds = %107, %15
  %28 = load i32, ptr %14, align 4
  %29 = load i32, ptr %13, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %110

31:                                               ; preds = %27
  %32 = load i32, ptr %14, align 4
  %33 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %106 [
    i32 90, label %36
    i32 66, label %44
    i32 83, label %52
    i32 67, label %60
    i32 73, label %69
    i32 74, label %76
    i32 68, label %84
    i32 70, label %91
    i32 76, label %99
  ]

36:                                               ; preds = %31
  %37 = load ptr, ptr %6, align 4
  %38 = getelementptr inbounds i8, ptr %37, i32 4
  store ptr %38, ptr %6, align 4
  %39 = load i32, ptr %37, align 4
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %11, align 4
  %42 = load i32, ptr %14, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %41, i32 %42
  store i8 %40, ptr %43, align 8
  br label %106

44:                                               ; preds = %31
  %45 = load ptr, ptr %6, align 4
  %46 = getelementptr inbounds i8, ptr %45, i32 4
  store ptr %46, ptr %6, align 4
  %47 = load i32, ptr %45, align 4
  %48 = trunc i32 %47 to i8
  %49 = load ptr, ptr %11, align 4
  %50 = load i32, ptr %14, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %49, i32 %50
  store i8 %48, ptr %51, align 8
  br label %106

52:                                               ; preds = %31
  %53 = load ptr, ptr %6, align 4
  %54 = getelementptr inbounds i8, ptr %53, i32 4
  store ptr %54, ptr %6, align 4
  %55 = load i32, ptr %53, align 4
  %56 = trunc i32 %55 to i16
  %57 = load ptr, ptr %11, align 4
  %58 = load i32, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %57, i32 %58
  store i16 %56, ptr %59, align 8
  br label %106

60:                                               ; preds = %31
  %61 = load ptr, ptr %6, align 4
  %62 = getelementptr inbounds i8, ptr %61, i32 4
  store ptr %62, ptr %6, align 4
  %63 = load i32, ptr %61, align 4
  %64 = trunc i32 %63 to i16
  %65 = zext i16 %64 to i32
  %66 = load ptr, ptr %11, align 4
  %67 = load i32, ptr %14, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %66, i32 %67
  store i32 %65, ptr %68, align 8
  br label %106

69:                                               ; preds = %31
  %70 = load ptr, ptr %6, align 4
  %71 = getelementptr inbounds i8, ptr %70, i32 4
  store ptr %71, ptr %6, align 4
  %72 = load i32, ptr %70, align 4
  %73 = load ptr, ptr %11, align 4
  %74 = load i32, ptr %14, align 4
  %75 = getelementptr inbounds %union.jvalue, ptr %73, i32 %74
  store i32 %72, ptr %75, align 8
  br label %106

76:                                               ; preds = %31
  %77 = load ptr, ptr %6, align 4
  %78 = getelementptr inbounds i8, ptr %77, i32 4
  store ptr %78, ptr %6, align 4
  %79 = load i32, ptr %77, align 4
  %80 = sext i32 %79 to i64
  %81 = load ptr, ptr %11, align 4
  %82 = load i32, ptr %14, align 4
  %83 = getelementptr inbounds %union.jvalue, ptr %81, i32 %82
  store i64 %80, ptr %83, align 8
  br label %106

84:                                               ; preds = %31
  %85 = load ptr, ptr %6, align 4
  %86 = getelementptr inbounds i8, ptr %85, i32 8
  store ptr %86, ptr %6, align 4
  %87 = load double, ptr %85, align 4
  %88 = load ptr, ptr %11, align 4
  %89 = load i32, ptr %14, align 4
  %90 = getelementptr inbounds %union.jvalue, ptr %88, i32 %89
  store double %87, ptr %90, align 8
  br label %106

91:                                               ; preds = %31
  %92 = load ptr, ptr %6, align 4
  %93 = getelementptr inbounds i8, ptr %92, i32 8
  store ptr %93, ptr %6, align 4
  %94 = load double, ptr %92, align 4
  %95 = fptrunc double %94 to float
  %96 = load ptr, ptr %11, align 4
  %97 = load i32, ptr %14, align 4
  %98 = getelementptr inbounds %union.jvalue, ptr %96, i32 %97
  store float %95, ptr %98, align 8
  br label %106

99:                                               ; preds = %31
  %100 = load ptr, ptr %6, align 4
  %101 = getelementptr inbounds i8, ptr %100, i32 4
  store ptr %101, ptr %6, align 4
  %102 = load ptr, ptr %100, align 4
  %103 = load ptr, ptr %11, align 4
  %104 = load i32, ptr %14, align 4
  %105 = getelementptr inbounds %union.jvalue, ptr %103, i32 %104
  store ptr %102, ptr %105, align 8
  br label %106

106:                                              ; preds = %31, %99, %91, %84, %76, %69, %60, %52, %44, %36
  br label %107

107:                                              ; preds = %106
  %108 = load i32, ptr %14, align 4
  %109 = add nsw i32 %108, 1
  store i32 %109, ptr %14, align 4
  br label %27, !llvm.loop !9

110:                                              ; preds = %27
  br label %111

111:                                              ; preds = %110
  %112 = load ptr, ptr %10, align 4
  %113 = load ptr, ptr %112, align 4
  %114 = getelementptr inbounds %struct.JNINativeInterface_, ptr %113, i32 0, i32 69
  %115 = load ptr, ptr %114, align 4
  %116 = load ptr, ptr %11, align 4
  %117 = load ptr, ptr %7, align 4
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %9, align 4
  %120 = load ptr, ptr %10, align 4
  %121 = call x86_stdcallcc zeroext i8 %115(ptr noundef %120, ptr noundef %119, ptr noundef %118, ptr noundef %117, ptr noundef %116)
  ret i8 %121
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport zeroext i8 @JNI_CallStaticBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca i8, align 1
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 4
  %10 = load ptr, ptr %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 118
  %12 = load ptr, ptr %11, align 4
  %13 = load ptr, ptr %8, align 4
  %14 = load ptr, ptr %4, align 4
  %15 = load ptr, ptr %5, align 4
  %16 = load ptr, ptr %6, align 4
  %17 = call x86_stdcallcc zeroext i8 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i8 %17, ptr %7, align 1
  call void @llvm.va_end(ptr %8)
  %18 = load i8, ptr %7, align 1
  ret i8 %18
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport zeroext i8 @JNI_CallStaticBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !10

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 119
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  %118 = call x86_stdcallcc zeroext i8 %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret i8 %118
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport signext i8 @JNI_CallByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca i8, align 1
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 4
  %10 = load ptr, ptr %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 41
  %12 = load ptr, ptr %11, align 4
  %13 = load ptr, ptr %8, align 4
  %14 = load ptr, ptr %4, align 4
  %15 = load ptr, ptr %5, align 4
  %16 = load ptr, ptr %6, align 4
  %17 = call x86_stdcallcc signext i8 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i8 %17, ptr %7, align 1
  call void @llvm.va_end(ptr %8)
  %18 = load i8, ptr %7, align 1
  ret i8 %18
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport signext i8 @JNI_CallByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !11

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 42
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  %118 = call x86_stdcallcc signext i8 %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret i8 %118
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport signext i8 @JNI_CallNonvirtualByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca i8, align 1
  %10 = alloca ptr, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  call void @llvm.va_start(ptr %10)
  %11 = load ptr, ptr %8, align 4
  %12 = load ptr, ptr %11, align 4
  %13 = getelementptr inbounds %struct.JNINativeInterface_, ptr %12, i32 0, i32 71
  %14 = load ptr, ptr %13, align 4
  %15 = load ptr, ptr %10, align 4
  %16 = load ptr, ptr %5, align 4
  %17 = load ptr, ptr %6, align 4
  %18 = load ptr, ptr %7, align 4
  %19 = load ptr, ptr %8, align 4
  %20 = call x86_stdcallcc signext i8 %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store i8 %20, ptr %9, align 1
  call void @llvm.va_end(ptr %10)
  %21 = load i8, ptr %9, align 1
  ret i8 %21
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport signext i8 @JNI_CallNonvirtualByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  %11 = alloca ptr, align 4
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  store ptr %3, ptr %7, align 4
  store ptr %2, ptr %8, align 4
  store ptr %1, ptr %9, align 4
  store ptr %0, ptr %10, align 4
  br label %15

15:                                               ; preds = %5
  %16 = load ptr, ptr %10, align 4
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 4
  %20 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 0
  %21 = load ptr, ptr %7, align 4
  %22 = load ptr, ptr %10, align 4
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = mul i32 %24, 8
  %26 = alloca i8, i32 %25, align 16
  store ptr %26, ptr %11, align 4
  store i32 0, ptr %14, align 4
  br label %27

27:                                               ; preds = %107, %15
  %28 = load i32, ptr %14, align 4
  %29 = load i32, ptr %13, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %110

31:                                               ; preds = %27
  %32 = load i32, ptr %14, align 4
  %33 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %106 [
    i32 90, label %36
    i32 66, label %44
    i32 83, label %52
    i32 67, label %60
    i32 73, label %69
    i32 74, label %76
    i32 68, label %84
    i32 70, label %91
    i32 76, label %99
  ]

36:                                               ; preds = %31
  %37 = load ptr, ptr %6, align 4
  %38 = getelementptr inbounds i8, ptr %37, i32 4
  store ptr %38, ptr %6, align 4
  %39 = load i32, ptr %37, align 4
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %11, align 4
  %42 = load i32, ptr %14, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %41, i32 %42
  store i8 %40, ptr %43, align 8
  br label %106

44:                                               ; preds = %31
  %45 = load ptr, ptr %6, align 4
  %46 = getelementptr inbounds i8, ptr %45, i32 4
  store ptr %46, ptr %6, align 4
  %47 = load i32, ptr %45, align 4
  %48 = trunc i32 %47 to i8
  %49 = load ptr, ptr %11, align 4
  %50 = load i32, ptr %14, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %49, i32 %50
  store i8 %48, ptr %51, align 8
  br label %106

52:                                               ; preds = %31
  %53 = load ptr, ptr %6, align 4
  %54 = getelementptr inbounds i8, ptr %53, i32 4
  store ptr %54, ptr %6, align 4
  %55 = load i32, ptr %53, align 4
  %56 = trunc i32 %55 to i16
  %57 = load ptr, ptr %11, align 4
  %58 = load i32, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %57, i32 %58
  store i16 %56, ptr %59, align 8
  br label %106

60:                                               ; preds = %31
  %61 = load ptr, ptr %6, align 4
  %62 = getelementptr inbounds i8, ptr %61, i32 4
  store ptr %62, ptr %6, align 4
  %63 = load i32, ptr %61, align 4
  %64 = trunc i32 %63 to i16
  %65 = zext i16 %64 to i32
  %66 = load ptr, ptr %11, align 4
  %67 = load i32, ptr %14, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %66, i32 %67
  store i32 %65, ptr %68, align 8
  br label %106

69:                                               ; preds = %31
  %70 = load ptr, ptr %6, align 4
  %71 = getelementptr inbounds i8, ptr %70, i32 4
  store ptr %71, ptr %6, align 4
  %72 = load i32, ptr %70, align 4
  %73 = load ptr, ptr %11, align 4
  %74 = load i32, ptr %14, align 4
  %75 = getelementptr inbounds %union.jvalue, ptr %73, i32 %74
  store i32 %72, ptr %75, align 8
  br label %106

76:                                               ; preds = %31
  %77 = load ptr, ptr %6, align 4
  %78 = getelementptr inbounds i8, ptr %77, i32 4
  store ptr %78, ptr %6, align 4
  %79 = load i32, ptr %77, align 4
  %80 = sext i32 %79 to i64
  %81 = load ptr, ptr %11, align 4
  %82 = load i32, ptr %14, align 4
  %83 = getelementptr inbounds %union.jvalue, ptr %81, i32 %82
  store i64 %80, ptr %83, align 8
  br label %106

84:                                               ; preds = %31
  %85 = load ptr, ptr %6, align 4
  %86 = getelementptr inbounds i8, ptr %85, i32 8
  store ptr %86, ptr %6, align 4
  %87 = load double, ptr %85, align 4
  %88 = load ptr, ptr %11, align 4
  %89 = load i32, ptr %14, align 4
  %90 = getelementptr inbounds %union.jvalue, ptr %88, i32 %89
  store double %87, ptr %90, align 8
  br label %106

91:                                               ; preds = %31
  %92 = load ptr, ptr %6, align 4
  %93 = getelementptr inbounds i8, ptr %92, i32 8
  store ptr %93, ptr %6, align 4
  %94 = load double, ptr %92, align 4
  %95 = fptrunc double %94 to float
  %96 = load ptr, ptr %11, align 4
  %97 = load i32, ptr %14, align 4
  %98 = getelementptr inbounds %union.jvalue, ptr %96, i32 %97
  store float %95, ptr %98, align 8
  br label %106

99:                                               ; preds = %31
  %100 = load ptr, ptr %6, align 4
  %101 = getelementptr inbounds i8, ptr %100, i32 4
  store ptr %101, ptr %6, align 4
  %102 = load ptr, ptr %100, align 4
  %103 = load ptr, ptr %11, align 4
  %104 = load i32, ptr %14, align 4
  %105 = getelementptr inbounds %union.jvalue, ptr %103, i32 %104
  store ptr %102, ptr %105, align 8
  br label %106

106:                                              ; preds = %31, %99, %91, %84, %76, %69, %60, %52, %44, %36
  br label %107

107:                                              ; preds = %106
  %108 = load i32, ptr %14, align 4
  %109 = add nsw i32 %108, 1
  store i32 %109, ptr %14, align 4
  br label %27, !llvm.loop !12

110:                                              ; preds = %27
  br label %111

111:                                              ; preds = %110
  %112 = load ptr, ptr %10, align 4
  %113 = load ptr, ptr %112, align 4
  %114 = getelementptr inbounds %struct.JNINativeInterface_, ptr %113, i32 0, i32 72
  %115 = load ptr, ptr %114, align 4
  %116 = load ptr, ptr %11, align 4
  %117 = load ptr, ptr %7, align 4
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %9, align 4
  %120 = load ptr, ptr %10, align 4
  %121 = call x86_stdcallcc signext i8 %115(ptr noundef %120, ptr noundef %119, ptr noundef %118, ptr noundef %117, ptr noundef %116)
  ret i8 %121
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport signext i8 @JNI_CallStaticByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca i8, align 1
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 4
  %10 = load ptr, ptr %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 121
  %12 = load ptr, ptr %11, align 4
  %13 = load ptr, ptr %8, align 4
  %14 = load ptr, ptr %4, align 4
  %15 = load ptr, ptr %5, align 4
  %16 = load ptr, ptr %6, align 4
  %17 = call x86_stdcallcc signext i8 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i8 %17, ptr %7, align 1
  call void @llvm.va_end(ptr %8)
  %18 = load i8, ptr %7, align 1
  ret i8 %18
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport signext i8 @JNI_CallStaticByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !13

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 122
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  %118 = call x86_stdcallcc signext i8 %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret i8 %118
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport zeroext i16 @JNI_CallCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca i16, align 2
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 4
  %10 = load ptr, ptr %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 44
  %12 = load ptr, ptr %11, align 4
  %13 = load ptr, ptr %8, align 4
  %14 = load ptr, ptr %4, align 4
  %15 = load ptr, ptr %5, align 4
  %16 = load ptr, ptr %6, align 4
  %17 = call x86_stdcallcc zeroext i16 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i16 %17, ptr %7, align 2
  call void @llvm.va_end(ptr %8)
  %18 = load i16, ptr %7, align 2
  ret i16 %18
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport zeroext i16 @JNI_CallCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !14

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 45
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  %118 = call x86_stdcallcc zeroext i16 %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret i16 %118
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport zeroext i16 @JNI_CallNonvirtualCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca i16, align 2
  %10 = alloca ptr, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  call void @llvm.va_start(ptr %10)
  %11 = load ptr, ptr %8, align 4
  %12 = load ptr, ptr %11, align 4
  %13 = getelementptr inbounds %struct.JNINativeInterface_, ptr %12, i32 0, i32 74
  %14 = load ptr, ptr %13, align 4
  %15 = load ptr, ptr %10, align 4
  %16 = load ptr, ptr %5, align 4
  %17 = load ptr, ptr %6, align 4
  %18 = load ptr, ptr %7, align 4
  %19 = load ptr, ptr %8, align 4
  %20 = call x86_stdcallcc zeroext i16 %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store i16 %20, ptr %9, align 2
  call void @llvm.va_end(ptr %10)
  %21 = load i16, ptr %9, align 2
  ret i16 %21
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport zeroext i16 @JNI_CallNonvirtualCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  %11 = alloca ptr, align 4
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  store ptr %3, ptr %7, align 4
  store ptr %2, ptr %8, align 4
  store ptr %1, ptr %9, align 4
  store ptr %0, ptr %10, align 4
  br label %15

15:                                               ; preds = %5
  %16 = load ptr, ptr %10, align 4
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 4
  %20 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 0
  %21 = load ptr, ptr %7, align 4
  %22 = load ptr, ptr %10, align 4
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = mul i32 %24, 8
  %26 = alloca i8, i32 %25, align 16
  store ptr %26, ptr %11, align 4
  store i32 0, ptr %14, align 4
  br label %27

27:                                               ; preds = %107, %15
  %28 = load i32, ptr %14, align 4
  %29 = load i32, ptr %13, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %110

31:                                               ; preds = %27
  %32 = load i32, ptr %14, align 4
  %33 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %106 [
    i32 90, label %36
    i32 66, label %44
    i32 83, label %52
    i32 67, label %60
    i32 73, label %69
    i32 74, label %76
    i32 68, label %84
    i32 70, label %91
    i32 76, label %99
  ]

36:                                               ; preds = %31
  %37 = load ptr, ptr %6, align 4
  %38 = getelementptr inbounds i8, ptr %37, i32 4
  store ptr %38, ptr %6, align 4
  %39 = load i32, ptr %37, align 4
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %11, align 4
  %42 = load i32, ptr %14, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %41, i32 %42
  store i8 %40, ptr %43, align 8
  br label %106

44:                                               ; preds = %31
  %45 = load ptr, ptr %6, align 4
  %46 = getelementptr inbounds i8, ptr %45, i32 4
  store ptr %46, ptr %6, align 4
  %47 = load i32, ptr %45, align 4
  %48 = trunc i32 %47 to i8
  %49 = load ptr, ptr %11, align 4
  %50 = load i32, ptr %14, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %49, i32 %50
  store i8 %48, ptr %51, align 8
  br label %106

52:                                               ; preds = %31
  %53 = load ptr, ptr %6, align 4
  %54 = getelementptr inbounds i8, ptr %53, i32 4
  store ptr %54, ptr %6, align 4
  %55 = load i32, ptr %53, align 4
  %56 = trunc i32 %55 to i16
  %57 = load ptr, ptr %11, align 4
  %58 = load i32, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %57, i32 %58
  store i16 %56, ptr %59, align 8
  br label %106

60:                                               ; preds = %31
  %61 = load ptr, ptr %6, align 4
  %62 = getelementptr inbounds i8, ptr %61, i32 4
  store ptr %62, ptr %6, align 4
  %63 = load i32, ptr %61, align 4
  %64 = trunc i32 %63 to i16
  %65 = zext i16 %64 to i32
  %66 = load ptr, ptr %11, align 4
  %67 = load i32, ptr %14, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %66, i32 %67
  store i32 %65, ptr %68, align 8
  br label %106

69:                                               ; preds = %31
  %70 = load ptr, ptr %6, align 4
  %71 = getelementptr inbounds i8, ptr %70, i32 4
  store ptr %71, ptr %6, align 4
  %72 = load i32, ptr %70, align 4
  %73 = load ptr, ptr %11, align 4
  %74 = load i32, ptr %14, align 4
  %75 = getelementptr inbounds %union.jvalue, ptr %73, i32 %74
  store i32 %72, ptr %75, align 8
  br label %106

76:                                               ; preds = %31
  %77 = load ptr, ptr %6, align 4
  %78 = getelementptr inbounds i8, ptr %77, i32 4
  store ptr %78, ptr %6, align 4
  %79 = load i32, ptr %77, align 4
  %80 = sext i32 %79 to i64
  %81 = load ptr, ptr %11, align 4
  %82 = load i32, ptr %14, align 4
  %83 = getelementptr inbounds %union.jvalue, ptr %81, i32 %82
  store i64 %80, ptr %83, align 8
  br label %106

84:                                               ; preds = %31
  %85 = load ptr, ptr %6, align 4
  %86 = getelementptr inbounds i8, ptr %85, i32 8
  store ptr %86, ptr %6, align 4
  %87 = load double, ptr %85, align 4
  %88 = load ptr, ptr %11, align 4
  %89 = load i32, ptr %14, align 4
  %90 = getelementptr inbounds %union.jvalue, ptr %88, i32 %89
  store double %87, ptr %90, align 8
  br label %106

91:                                               ; preds = %31
  %92 = load ptr, ptr %6, align 4
  %93 = getelementptr inbounds i8, ptr %92, i32 8
  store ptr %93, ptr %6, align 4
  %94 = load double, ptr %92, align 4
  %95 = fptrunc double %94 to float
  %96 = load ptr, ptr %11, align 4
  %97 = load i32, ptr %14, align 4
  %98 = getelementptr inbounds %union.jvalue, ptr %96, i32 %97
  store float %95, ptr %98, align 8
  br label %106

99:                                               ; preds = %31
  %100 = load ptr, ptr %6, align 4
  %101 = getelementptr inbounds i8, ptr %100, i32 4
  store ptr %101, ptr %6, align 4
  %102 = load ptr, ptr %100, align 4
  %103 = load ptr, ptr %11, align 4
  %104 = load i32, ptr %14, align 4
  %105 = getelementptr inbounds %union.jvalue, ptr %103, i32 %104
  store ptr %102, ptr %105, align 8
  br label %106

106:                                              ; preds = %31, %99, %91, %84, %76, %69, %60, %52, %44, %36
  br label %107

107:                                              ; preds = %106
  %108 = load i32, ptr %14, align 4
  %109 = add nsw i32 %108, 1
  store i32 %109, ptr %14, align 4
  br label %27, !llvm.loop !15

110:                                              ; preds = %27
  br label %111

111:                                              ; preds = %110
  %112 = load ptr, ptr %10, align 4
  %113 = load ptr, ptr %112, align 4
  %114 = getelementptr inbounds %struct.JNINativeInterface_, ptr %113, i32 0, i32 75
  %115 = load ptr, ptr %114, align 4
  %116 = load ptr, ptr %11, align 4
  %117 = load ptr, ptr %7, align 4
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %9, align 4
  %120 = load ptr, ptr %10, align 4
  %121 = call x86_stdcallcc zeroext i16 %115(ptr noundef %120, ptr noundef %119, ptr noundef %118, ptr noundef %117, ptr noundef %116)
  ret i16 %121
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport zeroext i16 @JNI_CallStaticCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca i16, align 2
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 4
  %10 = load ptr, ptr %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 124
  %12 = load ptr, ptr %11, align 4
  %13 = load ptr, ptr %8, align 4
  %14 = load ptr, ptr %4, align 4
  %15 = load ptr, ptr %5, align 4
  %16 = load ptr, ptr %6, align 4
  %17 = call x86_stdcallcc zeroext i16 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i16 %17, ptr %7, align 2
  call void @llvm.va_end(ptr %8)
  %18 = load i16, ptr %7, align 2
  ret i16 %18
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport zeroext i16 @JNI_CallStaticCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !16

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 125
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  %118 = call x86_stdcallcc zeroext i16 %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret i16 %118
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport signext i16 @JNI_CallShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca i16, align 2
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 4
  %10 = load ptr, ptr %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 47
  %12 = load ptr, ptr %11, align 4
  %13 = load ptr, ptr %8, align 4
  %14 = load ptr, ptr %4, align 4
  %15 = load ptr, ptr %5, align 4
  %16 = load ptr, ptr %6, align 4
  %17 = call x86_stdcallcc signext i16 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i16 %17, ptr %7, align 2
  call void @llvm.va_end(ptr %8)
  %18 = load i16, ptr %7, align 2
  ret i16 %18
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport signext i16 @JNI_CallShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !17

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 48
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  %118 = call x86_stdcallcc signext i16 %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret i16 %118
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport signext i16 @JNI_CallNonvirtualShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca i16, align 2
  %10 = alloca ptr, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  call void @llvm.va_start(ptr %10)
  %11 = load ptr, ptr %8, align 4
  %12 = load ptr, ptr %11, align 4
  %13 = getelementptr inbounds %struct.JNINativeInterface_, ptr %12, i32 0, i32 77
  %14 = load ptr, ptr %13, align 4
  %15 = load ptr, ptr %10, align 4
  %16 = load ptr, ptr %5, align 4
  %17 = load ptr, ptr %6, align 4
  %18 = load ptr, ptr %7, align 4
  %19 = load ptr, ptr %8, align 4
  %20 = call x86_stdcallcc signext i16 %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store i16 %20, ptr %9, align 2
  call void @llvm.va_end(ptr %10)
  %21 = load i16, ptr %9, align 2
  ret i16 %21
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport signext i16 @JNI_CallNonvirtualShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  %11 = alloca ptr, align 4
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  store ptr %3, ptr %7, align 4
  store ptr %2, ptr %8, align 4
  store ptr %1, ptr %9, align 4
  store ptr %0, ptr %10, align 4
  br label %15

15:                                               ; preds = %5
  %16 = load ptr, ptr %10, align 4
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 4
  %20 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 0
  %21 = load ptr, ptr %7, align 4
  %22 = load ptr, ptr %10, align 4
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = mul i32 %24, 8
  %26 = alloca i8, i32 %25, align 16
  store ptr %26, ptr %11, align 4
  store i32 0, ptr %14, align 4
  br label %27

27:                                               ; preds = %107, %15
  %28 = load i32, ptr %14, align 4
  %29 = load i32, ptr %13, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %110

31:                                               ; preds = %27
  %32 = load i32, ptr %14, align 4
  %33 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %106 [
    i32 90, label %36
    i32 66, label %44
    i32 83, label %52
    i32 67, label %60
    i32 73, label %69
    i32 74, label %76
    i32 68, label %84
    i32 70, label %91
    i32 76, label %99
  ]

36:                                               ; preds = %31
  %37 = load ptr, ptr %6, align 4
  %38 = getelementptr inbounds i8, ptr %37, i32 4
  store ptr %38, ptr %6, align 4
  %39 = load i32, ptr %37, align 4
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %11, align 4
  %42 = load i32, ptr %14, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %41, i32 %42
  store i8 %40, ptr %43, align 8
  br label %106

44:                                               ; preds = %31
  %45 = load ptr, ptr %6, align 4
  %46 = getelementptr inbounds i8, ptr %45, i32 4
  store ptr %46, ptr %6, align 4
  %47 = load i32, ptr %45, align 4
  %48 = trunc i32 %47 to i8
  %49 = load ptr, ptr %11, align 4
  %50 = load i32, ptr %14, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %49, i32 %50
  store i8 %48, ptr %51, align 8
  br label %106

52:                                               ; preds = %31
  %53 = load ptr, ptr %6, align 4
  %54 = getelementptr inbounds i8, ptr %53, i32 4
  store ptr %54, ptr %6, align 4
  %55 = load i32, ptr %53, align 4
  %56 = trunc i32 %55 to i16
  %57 = load ptr, ptr %11, align 4
  %58 = load i32, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %57, i32 %58
  store i16 %56, ptr %59, align 8
  br label %106

60:                                               ; preds = %31
  %61 = load ptr, ptr %6, align 4
  %62 = getelementptr inbounds i8, ptr %61, i32 4
  store ptr %62, ptr %6, align 4
  %63 = load i32, ptr %61, align 4
  %64 = trunc i32 %63 to i16
  %65 = zext i16 %64 to i32
  %66 = load ptr, ptr %11, align 4
  %67 = load i32, ptr %14, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %66, i32 %67
  store i32 %65, ptr %68, align 8
  br label %106

69:                                               ; preds = %31
  %70 = load ptr, ptr %6, align 4
  %71 = getelementptr inbounds i8, ptr %70, i32 4
  store ptr %71, ptr %6, align 4
  %72 = load i32, ptr %70, align 4
  %73 = load ptr, ptr %11, align 4
  %74 = load i32, ptr %14, align 4
  %75 = getelementptr inbounds %union.jvalue, ptr %73, i32 %74
  store i32 %72, ptr %75, align 8
  br label %106

76:                                               ; preds = %31
  %77 = load ptr, ptr %6, align 4
  %78 = getelementptr inbounds i8, ptr %77, i32 4
  store ptr %78, ptr %6, align 4
  %79 = load i32, ptr %77, align 4
  %80 = sext i32 %79 to i64
  %81 = load ptr, ptr %11, align 4
  %82 = load i32, ptr %14, align 4
  %83 = getelementptr inbounds %union.jvalue, ptr %81, i32 %82
  store i64 %80, ptr %83, align 8
  br label %106

84:                                               ; preds = %31
  %85 = load ptr, ptr %6, align 4
  %86 = getelementptr inbounds i8, ptr %85, i32 8
  store ptr %86, ptr %6, align 4
  %87 = load double, ptr %85, align 4
  %88 = load ptr, ptr %11, align 4
  %89 = load i32, ptr %14, align 4
  %90 = getelementptr inbounds %union.jvalue, ptr %88, i32 %89
  store double %87, ptr %90, align 8
  br label %106

91:                                               ; preds = %31
  %92 = load ptr, ptr %6, align 4
  %93 = getelementptr inbounds i8, ptr %92, i32 8
  store ptr %93, ptr %6, align 4
  %94 = load double, ptr %92, align 4
  %95 = fptrunc double %94 to float
  %96 = load ptr, ptr %11, align 4
  %97 = load i32, ptr %14, align 4
  %98 = getelementptr inbounds %union.jvalue, ptr %96, i32 %97
  store float %95, ptr %98, align 8
  br label %106

99:                                               ; preds = %31
  %100 = load ptr, ptr %6, align 4
  %101 = getelementptr inbounds i8, ptr %100, i32 4
  store ptr %101, ptr %6, align 4
  %102 = load ptr, ptr %100, align 4
  %103 = load ptr, ptr %11, align 4
  %104 = load i32, ptr %14, align 4
  %105 = getelementptr inbounds %union.jvalue, ptr %103, i32 %104
  store ptr %102, ptr %105, align 8
  br label %106

106:                                              ; preds = %31, %99, %91, %84, %76, %69, %60, %52, %44, %36
  br label %107

107:                                              ; preds = %106
  %108 = load i32, ptr %14, align 4
  %109 = add nsw i32 %108, 1
  store i32 %109, ptr %14, align 4
  br label %27, !llvm.loop !18

110:                                              ; preds = %27
  br label %111

111:                                              ; preds = %110
  %112 = load ptr, ptr %10, align 4
  %113 = load ptr, ptr %112, align 4
  %114 = getelementptr inbounds %struct.JNINativeInterface_, ptr %113, i32 0, i32 78
  %115 = load ptr, ptr %114, align 4
  %116 = load ptr, ptr %11, align 4
  %117 = load ptr, ptr %7, align 4
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %9, align 4
  %120 = load ptr, ptr %10, align 4
  %121 = call x86_stdcallcc signext i16 %115(ptr noundef %120, ptr noundef %119, ptr noundef %118, ptr noundef %117, ptr noundef %116)
  ret i16 %121
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport signext i16 @JNI_CallStaticShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca i16, align 2
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 4
  %10 = load ptr, ptr %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 127
  %12 = load ptr, ptr %11, align 4
  %13 = load ptr, ptr %8, align 4
  %14 = load ptr, ptr %4, align 4
  %15 = load ptr, ptr %5, align 4
  %16 = load ptr, ptr %6, align 4
  %17 = call x86_stdcallcc signext i16 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i16 %17, ptr %7, align 2
  call void @llvm.va_end(ptr %8)
  %18 = load i16, ptr %7, align 2
  ret i16 %18
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport signext i16 @JNI_CallStaticShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !19

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 128
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  %118 = call x86_stdcallcc signext i16 %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret i16 %118
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport i32 @JNI_CallIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca i32, align 4
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 4
  %10 = load ptr, ptr %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 50
  %12 = load ptr, ptr %11, align 4
  %13 = load ptr, ptr %8, align 4
  %14 = load ptr, ptr %4, align 4
  %15 = load ptr, ptr %5, align 4
  %16 = load ptr, ptr %6, align 4
  %17 = call x86_stdcallcc i32 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i32 %17, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %18 = load i32, ptr %7, align 4
  ret i32 %18
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport i32 @JNI_CallIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !20

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 51
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  %118 = call x86_stdcallcc i32 %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret i32 %118
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport i32 @JNI_CallNonvirtualIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca i32, align 4
  %10 = alloca ptr, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  call void @llvm.va_start(ptr %10)
  %11 = load ptr, ptr %8, align 4
  %12 = load ptr, ptr %11, align 4
  %13 = getelementptr inbounds %struct.JNINativeInterface_, ptr %12, i32 0, i32 80
  %14 = load ptr, ptr %13, align 4
  %15 = load ptr, ptr %10, align 4
  %16 = load ptr, ptr %5, align 4
  %17 = load ptr, ptr %6, align 4
  %18 = load ptr, ptr %7, align 4
  %19 = load ptr, ptr %8, align 4
  %20 = call x86_stdcallcc i32 %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store i32 %20, ptr %9, align 4
  call void @llvm.va_end(ptr %10)
  %21 = load i32, ptr %9, align 4
  ret i32 %21
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport i32 @JNI_CallNonvirtualIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  %11 = alloca ptr, align 4
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  store ptr %3, ptr %7, align 4
  store ptr %2, ptr %8, align 4
  store ptr %1, ptr %9, align 4
  store ptr %0, ptr %10, align 4
  br label %15

15:                                               ; preds = %5
  %16 = load ptr, ptr %10, align 4
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 4
  %20 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 0
  %21 = load ptr, ptr %7, align 4
  %22 = load ptr, ptr %10, align 4
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = mul i32 %24, 8
  %26 = alloca i8, i32 %25, align 16
  store ptr %26, ptr %11, align 4
  store i32 0, ptr %14, align 4
  br label %27

27:                                               ; preds = %107, %15
  %28 = load i32, ptr %14, align 4
  %29 = load i32, ptr %13, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %110

31:                                               ; preds = %27
  %32 = load i32, ptr %14, align 4
  %33 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %106 [
    i32 90, label %36
    i32 66, label %44
    i32 83, label %52
    i32 67, label %60
    i32 73, label %69
    i32 74, label %76
    i32 68, label %84
    i32 70, label %91
    i32 76, label %99
  ]

36:                                               ; preds = %31
  %37 = load ptr, ptr %6, align 4
  %38 = getelementptr inbounds i8, ptr %37, i32 4
  store ptr %38, ptr %6, align 4
  %39 = load i32, ptr %37, align 4
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %11, align 4
  %42 = load i32, ptr %14, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %41, i32 %42
  store i8 %40, ptr %43, align 8
  br label %106

44:                                               ; preds = %31
  %45 = load ptr, ptr %6, align 4
  %46 = getelementptr inbounds i8, ptr %45, i32 4
  store ptr %46, ptr %6, align 4
  %47 = load i32, ptr %45, align 4
  %48 = trunc i32 %47 to i8
  %49 = load ptr, ptr %11, align 4
  %50 = load i32, ptr %14, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %49, i32 %50
  store i8 %48, ptr %51, align 8
  br label %106

52:                                               ; preds = %31
  %53 = load ptr, ptr %6, align 4
  %54 = getelementptr inbounds i8, ptr %53, i32 4
  store ptr %54, ptr %6, align 4
  %55 = load i32, ptr %53, align 4
  %56 = trunc i32 %55 to i16
  %57 = load ptr, ptr %11, align 4
  %58 = load i32, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %57, i32 %58
  store i16 %56, ptr %59, align 8
  br label %106

60:                                               ; preds = %31
  %61 = load ptr, ptr %6, align 4
  %62 = getelementptr inbounds i8, ptr %61, i32 4
  store ptr %62, ptr %6, align 4
  %63 = load i32, ptr %61, align 4
  %64 = trunc i32 %63 to i16
  %65 = zext i16 %64 to i32
  %66 = load ptr, ptr %11, align 4
  %67 = load i32, ptr %14, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %66, i32 %67
  store i32 %65, ptr %68, align 8
  br label %106

69:                                               ; preds = %31
  %70 = load ptr, ptr %6, align 4
  %71 = getelementptr inbounds i8, ptr %70, i32 4
  store ptr %71, ptr %6, align 4
  %72 = load i32, ptr %70, align 4
  %73 = load ptr, ptr %11, align 4
  %74 = load i32, ptr %14, align 4
  %75 = getelementptr inbounds %union.jvalue, ptr %73, i32 %74
  store i32 %72, ptr %75, align 8
  br label %106

76:                                               ; preds = %31
  %77 = load ptr, ptr %6, align 4
  %78 = getelementptr inbounds i8, ptr %77, i32 4
  store ptr %78, ptr %6, align 4
  %79 = load i32, ptr %77, align 4
  %80 = sext i32 %79 to i64
  %81 = load ptr, ptr %11, align 4
  %82 = load i32, ptr %14, align 4
  %83 = getelementptr inbounds %union.jvalue, ptr %81, i32 %82
  store i64 %80, ptr %83, align 8
  br label %106

84:                                               ; preds = %31
  %85 = load ptr, ptr %6, align 4
  %86 = getelementptr inbounds i8, ptr %85, i32 8
  store ptr %86, ptr %6, align 4
  %87 = load double, ptr %85, align 4
  %88 = load ptr, ptr %11, align 4
  %89 = load i32, ptr %14, align 4
  %90 = getelementptr inbounds %union.jvalue, ptr %88, i32 %89
  store double %87, ptr %90, align 8
  br label %106

91:                                               ; preds = %31
  %92 = load ptr, ptr %6, align 4
  %93 = getelementptr inbounds i8, ptr %92, i32 8
  store ptr %93, ptr %6, align 4
  %94 = load double, ptr %92, align 4
  %95 = fptrunc double %94 to float
  %96 = load ptr, ptr %11, align 4
  %97 = load i32, ptr %14, align 4
  %98 = getelementptr inbounds %union.jvalue, ptr %96, i32 %97
  store float %95, ptr %98, align 8
  br label %106

99:                                               ; preds = %31
  %100 = load ptr, ptr %6, align 4
  %101 = getelementptr inbounds i8, ptr %100, i32 4
  store ptr %101, ptr %6, align 4
  %102 = load ptr, ptr %100, align 4
  %103 = load ptr, ptr %11, align 4
  %104 = load i32, ptr %14, align 4
  %105 = getelementptr inbounds %union.jvalue, ptr %103, i32 %104
  store ptr %102, ptr %105, align 8
  br label %106

106:                                              ; preds = %31, %99, %91, %84, %76, %69, %60, %52, %44, %36
  br label %107

107:                                              ; preds = %106
  %108 = load i32, ptr %14, align 4
  %109 = add nsw i32 %108, 1
  store i32 %109, ptr %14, align 4
  br label %27, !llvm.loop !21

110:                                              ; preds = %27
  br label %111

111:                                              ; preds = %110
  %112 = load ptr, ptr %10, align 4
  %113 = load ptr, ptr %112, align 4
  %114 = getelementptr inbounds %struct.JNINativeInterface_, ptr %113, i32 0, i32 81
  %115 = load ptr, ptr %114, align 4
  %116 = load ptr, ptr %11, align 4
  %117 = load ptr, ptr %7, align 4
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %9, align 4
  %120 = load ptr, ptr %10, align 4
  %121 = call x86_stdcallcc i32 %115(ptr noundef %120, ptr noundef %119, ptr noundef %118, ptr noundef %117, ptr noundef %116)
  ret i32 %121
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport i32 @JNI_CallStaticIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca i32, align 4
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 4
  %10 = load ptr, ptr %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 130
  %12 = load ptr, ptr %11, align 4
  %13 = load ptr, ptr %8, align 4
  %14 = load ptr, ptr %4, align 4
  %15 = load ptr, ptr %5, align 4
  %16 = load ptr, ptr %6, align 4
  %17 = call x86_stdcallcc i32 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i32 %17, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %18 = load i32, ptr %7, align 4
  ret i32 %18
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport i32 @JNI_CallStaticIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !22

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 131
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  %118 = call x86_stdcallcc i32 %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret i32 %118
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport i64 @JNI_CallLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca i64, align 8
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 4
  %10 = load ptr, ptr %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 53
  %12 = load ptr, ptr %11, align 4
  %13 = load ptr, ptr %8, align 4
  %14 = load ptr, ptr %4, align 4
  %15 = load ptr, ptr %5, align 4
  %16 = load ptr, ptr %6, align 4
  %17 = call x86_stdcallcc i64 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i64 %17, ptr %7, align 8
  call void @llvm.va_end(ptr %8)
  %18 = load i64, ptr %7, align 8
  ret i64 %18
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport i64 @JNI_CallLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !23

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 54
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  %118 = call x86_stdcallcc i64 %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret i64 %118
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport i64 @JNI_CallNonvirtualLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca i64, align 8
  %10 = alloca ptr, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  call void @llvm.va_start(ptr %10)
  %11 = load ptr, ptr %8, align 4
  %12 = load ptr, ptr %11, align 4
  %13 = getelementptr inbounds %struct.JNINativeInterface_, ptr %12, i32 0, i32 83
  %14 = load ptr, ptr %13, align 4
  %15 = load ptr, ptr %10, align 4
  %16 = load ptr, ptr %5, align 4
  %17 = load ptr, ptr %6, align 4
  %18 = load ptr, ptr %7, align 4
  %19 = load ptr, ptr %8, align 4
  %20 = call x86_stdcallcc i64 %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store i64 %20, ptr %9, align 8
  call void @llvm.va_end(ptr %10)
  %21 = load i64, ptr %9, align 8
  ret i64 %21
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport i64 @JNI_CallNonvirtualLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  %11 = alloca ptr, align 4
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  store ptr %3, ptr %7, align 4
  store ptr %2, ptr %8, align 4
  store ptr %1, ptr %9, align 4
  store ptr %0, ptr %10, align 4
  br label %15

15:                                               ; preds = %5
  %16 = load ptr, ptr %10, align 4
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 4
  %20 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 0
  %21 = load ptr, ptr %7, align 4
  %22 = load ptr, ptr %10, align 4
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = mul i32 %24, 8
  %26 = alloca i8, i32 %25, align 16
  store ptr %26, ptr %11, align 4
  store i32 0, ptr %14, align 4
  br label %27

27:                                               ; preds = %107, %15
  %28 = load i32, ptr %14, align 4
  %29 = load i32, ptr %13, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %110

31:                                               ; preds = %27
  %32 = load i32, ptr %14, align 4
  %33 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %106 [
    i32 90, label %36
    i32 66, label %44
    i32 83, label %52
    i32 67, label %60
    i32 73, label %69
    i32 74, label %76
    i32 68, label %84
    i32 70, label %91
    i32 76, label %99
  ]

36:                                               ; preds = %31
  %37 = load ptr, ptr %6, align 4
  %38 = getelementptr inbounds i8, ptr %37, i32 4
  store ptr %38, ptr %6, align 4
  %39 = load i32, ptr %37, align 4
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %11, align 4
  %42 = load i32, ptr %14, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %41, i32 %42
  store i8 %40, ptr %43, align 8
  br label %106

44:                                               ; preds = %31
  %45 = load ptr, ptr %6, align 4
  %46 = getelementptr inbounds i8, ptr %45, i32 4
  store ptr %46, ptr %6, align 4
  %47 = load i32, ptr %45, align 4
  %48 = trunc i32 %47 to i8
  %49 = load ptr, ptr %11, align 4
  %50 = load i32, ptr %14, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %49, i32 %50
  store i8 %48, ptr %51, align 8
  br label %106

52:                                               ; preds = %31
  %53 = load ptr, ptr %6, align 4
  %54 = getelementptr inbounds i8, ptr %53, i32 4
  store ptr %54, ptr %6, align 4
  %55 = load i32, ptr %53, align 4
  %56 = trunc i32 %55 to i16
  %57 = load ptr, ptr %11, align 4
  %58 = load i32, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %57, i32 %58
  store i16 %56, ptr %59, align 8
  br label %106

60:                                               ; preds = %31
  %61 = load ptr, ptr %6, align 4
  %62 = getelementptr inbounds i8, ptr %61, i32 4
  store ptr %62, ptr %6, align 4
  %63 = load i32, ptr %61, align 4
  %64 = trunc i32 %63 to i16
  %65 = zext i16 %64 to i32
  %66 = load ptr, ptr %11, align 4
  %67 = load i32, ptr %14, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %66, i32 %67
  store i32 %65, ptr %68, align 8
  br label %106

69:                                               ; preds = %31
  %70 = load ptr, ptr %6, align 4
  %71 = getelementptr inbounds i8, ptr %70, i32 4
  store ptr %71, ptr %6, align 4
  %72 = load i32, ptr %70, align 4
  %73 = load ptr, ptr %11, align 4
  %74 = load i32, ptr %14, align 4
  %75 = getelementptr inbounds %union.jvalue, ptr %73, i32 %74
  store i32 %72, ptr %75, align 8
  br label %106

76:                                               ; preds = %31
  %77 = load ptr, ptr %6, align 4
  %78 = getelementptr inbounds i8, ptr %77, i32 4
  store ptr %78, ptr %6, align 4
  %79 = load i32, ptr %77, align 4
  %80 = sext i32 %79 to i64
  %81 = load ptr, ptr %11, align 4
  %82 = load i32, ptr %14, align 4
  %83 = getelementptr inbounds %union.jvalue, ptr %81, i32 %82
  store i64 %80, ptr %83, align 8
  br label %106

84:                                               ; preds = %31
  %85 = load ptr, ptr %6, align 4
  %86 = getelementptr inbounds i8, ptr %85, i32 8
  store ptr %86, ptr %6, align 4
  %87 = load double, ptr %85, align 4
  %88 = load ptr, ptr %11, align 4
  %89 = load i32, ptr %14, align 4
  %90 = getelementptr inbounds %union.jvalue, ptr %88, i32 %89
  store double %87, ptr %90, align 8
  br label %106

91:                                               ; preds = %31
  %92 = load ptr, ptr %6, align 4
  %93 = getelementptr inbounds i8, ptr %92, i32 8
  store ptr %93, ptr %6, align 4
  %94 = load double, ptr %92, align 4
  %95 = fptrunc double %94 to float
  %96 = load ptr, ptr %11, align 4
  %97 = load i32, ptr %14, align 4
  %98 = getelementptr inbounds %union.jvalue, ptr %96, i32 %97
  store float %95, ptr %98, align 8
  br label %106

99:                                               ; preds = %31
  %100 = load ptr, ptr %6, align 4
  %101 = getelementptr inbounds i8, ptr %100, i32 4
  store ptr %101, ptr %6, align 4
  %102 = load ptr, ptr %100, align 4
  %103 = load ptr, ptr %11, align 4
  %104 = load i32, ptr %14, align 4
  %105 = getelementptr inbounds %union.jvalue, ptr %103, i32 %104
  store ptr %102, ptr %105, align 8
  br label %106

106:                                              ; preds = %31, %99, %91, %84, %76, %69, %60, %52, %44, %36
  br label %107

107:                                              ; preds = %106
  %108 = load i32, ptr %14, align 4
  %109 = add nsw i32 %108, 1
  store i32 %109, ptr %14, align 4
  br label %27, !llvm.loop !24

110:                                              ; preds = %27
  br label %111

111:                                              ; preds = %110
  %112 = load ptr, ptr %10, align 4
  %113 = load ptr, ptr %112, align 4
  %114 = getelementptr inbounds %struct.JNINativeInterface_, ptr %113, i32 0, i32 84
  %115 = load ptr, ptr %114, align 4
  %116 = load ptr, ptr %11, align 4
  %117 = load ptr, ptr %7, align 4
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %9, align 4
  %120 = load ptr, ptr %10, align 4
  %121 = call x86_stdcallcc i64 %115(ptr noundef %120, ptr noundef %119, ptr noundef %118, ptr noundef %117, ptr noundef %116)
  ret i64 %121
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport i64 @JNI_CallStaticLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca i64, align 8
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 4
  %10 = load ptr, ptr %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 133
  %12 = load ptr, ptr %11, align 4
  %13 = load ptr, ptr %8, align 4
  %14 = load ptr, ptr %4, align 4
  %15 = load ptr, ptr %5, align 4
  %16 = load ptr, ptr %6, align 4
  %17 = call x86_stdcallcc i64 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i64 %17, ptr %7, align 8
  call void @llvm.va_end(ptr %8)
  %18 = load i64, ptr %7, align 8
  ret i64 %18
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport i64 @JNI_CallStaticLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !25

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 134
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  %118 = call x86_stdcallcc i64 %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret i64 %118
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport float @JNI_CallFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca float, align 4
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 4
  %10 = load ptr, ptr %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 56
  %12 = load ptr, ptr %11, align 4
  %13 = load ptr, ptr %8, align 4
  %14 = load ptr, ptr %4, align 4
  %15 = load ptr, ptr %5, align 4
  %16 = load ptr, ptr %6, align 4
  %17 = call x86_stdcallcc float %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store float %17, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %18 = load float, ptr %7, align 4
  ret float %18
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport float @JNI_CallFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !26

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 57
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  %118 = call x86_stdcallcc float %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret float %118
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport float @JNI_CallNonvirtualFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca float, align 4
  %10 = alloca ptr, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  call void @llvm.va_start(ptr %10)
  %11 = load ptr, ptr %8, align 4
  %12 = load ptr, ptr %11, align 4
  %13 = getelementptr inbounds %struct.JNINativeInterface_, ptr %12, i32 0, i32 86
  %14 = load ptr, ptr %13, align 4
  %15 = load ptr, ptr %10, align 4
  %16 = load ptr, ptr %5, align 4
  %17 = load ptr, ptr %6, align 4
  %18 = load ptr, ptr %7, align 4
  %19 = load ptr, ptr %8, align 4
  %20 = call x86_stdcallcc float %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store float %20, ptr %9, align 4
  call void @llvm.va_end(ptr %10)
  %21 = load float, ptr %9, align 4
  ret float %21
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport float @JNI_CallNonvirtualFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  %11 = alloca ptr, align 4
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  store ptr %3, ptr %7, align 4
  store ptr %2, ptr %8, align 4
  store ptr %1, ptr %9, align 4
  store ptr %0, ptr %10, align 4
  br label %15

15:                                               ; preds = %5
  %16 = load ptr, ptr %10, align 4
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 4
  %20 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 0
  %21 = load ptr, ptr %7, align 4
  %22 = load ptr, ptr %10, align 4
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = mul i32 %24, 8
  %26 = alloca i8, i32 %25, align 16
  store ptr %26, ptr %11, align 4
  store i32 0, ptr %14, align 4
  br label %27

27:                                               ; preds = %107, %15
  %28 = load i32, ptr %14, align 4
  %29 = load i32, ptr %13, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %110

31:                                               ; preds = %27
  %32 = load i32, ptr %14, align 4
  %33 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %106 [
    i32 90, label %36
    i32 66, label %44
    i32 83, label %52
    i32 67, label %60
    i32 73, label %69
    i32 74, label %76
    i32 68, label %84
    i32 70, label %91
    i32 76, label %99
  ]

36:                                               ; preds = %31
  %37 = load ptr, ptr %6, align 4
  %38 = getelementptr inbounds i8, ptr %37, i32 4
  store ptr %38, ptr %6, align 4
  %39 = load i32, ptr %37, align 4
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %11, align 4
  %42 = load i32, ptr %14, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %41, i32 %42
  store i8 %40, ptr %43, align 8
  br label %106

44:                                               ; preds = %31
  %45 = load ptr, ptr %6, align 4
  %46 = getelementptr inbounds i8, ptr %45, i32 4
  store ptr %46, ptr %6, align 4
  %47 = load i32, ptr %45, align 4
  %48 = trunc i32 %47 to i8
  %49 = load ptr, ptr %11, align 4
  %50 = load i32, ptr %14, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %49, i32 %50
  store i8 %48, ptr %51, align 8
  br label %106

52:                                               ; preds = %31
  %53 = load ptr, ptr %6, align 4
  %54 = getelementptr inbounds i8, ptr %53, i32 4
  store ptr %54, ptr %6, align 4
  %55 = load i32, ptr %53, align 4
  %56 = trunc i32 %55 to i16
  %57 = load ptr, ptr %11, align 4
  %58 = load i32, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %57, i32 %58
  store i16 %56, ptr %59, align 8
  br label %106

60:                                               ; preds = %31
  %61 = load ptr, ptr %6, align 4
  %62 = getelementptr inbounds i8, ptr %61, i32 4
  store ptr %62, ptr %6, align 4
  %63 = load i32, ptr %61, align 4
  %64 = trunc i32 %63 to i16
  %65 = zext i16 %64 to i32
  %66 = load ptr, ptr %11, align 4
  %67 = load i32, ptr %14, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %66, i32 %67
  store i32 %65, ptr %68, align 8
  br label %106

69:                                               ; preds = %31
  %70 = load ptr, ptr %6, align 4
  %71 = getelementptr inbounds i8, ptr %70, i32 4
  store ptr %71, ptr %6, align 4
  %72 = load i32, ptr %70, align 4
  %73 = load ptr, ptr %11, align 4
  %74 = load i32, ptr %14, align 4
  %75 = getelementptr inbounds %union.jvalue, ptr %73, i32 %74
  store i32 %72, ptr %75, align 8
  br label %106

76:                                               ; preds = %31
  %77 = load ptr, ptr %6, align 4
  %78 = getelementptr inbounds i8, ptr %77, i32 4
  store ptr %78, ptr %6, align 4
  %79 = load i32, ptr %77, align 4
  %80 = sext i32 %79 to i64
  %81 = load ptr, ptr %11, align 4
  %82 = load i32, ptr %14, align 4
  %83 = getelementptr inbounds %union.jvalue, ptr %81, i32 %82
  store i64 %80, ptr %83, align 8
  br label %106

84:                                               ; preds = %31
  %85 = load ptr, ptr %6, align 4
  %86 = getelementptr inbounds i8, ptr %85, i32 8
  store ptr %86, ptr %6, align 4
  %87 = load double, ptr %85, align 4
  %88 = load ptr, ptr %11, align 4
  %89 = load i32, ptr %14, align 4
  %90 = getelementptr inbounds %union.jvalue, ptr %88, i32 %89
  store double %87, ptr %90, align 8
  br label %106

91:                                               ; preds = %31
  %92 = load ptr, ptr %6, align 4
  %93 = getelementptr inbounds i8, ptr %92, i32 8
  store ptr %93, ptr %6, align 4
  %94 = load double, ptr %92, align 4
  %95 = fptrunc double %94 to float
  %96 = load ptr, ptr %11, align 4
  %97 = load i32, ptr %14, align 4
  %98 = getelementptr inbounds %union.jvalue, ptr %96, i32 %97
  store float %95, ptr %98, align 8
  br label %106

99:                                               ; preds = %31
  %100 = load ptr, ptr %6, align 4
  %101 = getelementptr inbounds i8, ptr %100, i32 4
  store ptr %101, ptr %6, align 4
  %102 = load ptr, ptr %100, align 4
  %103 = load ptr, ptr %11, align 4
  %104 = load i32, ptr %14, align 4
  %105 = getelementptr inbounds %union.jvalue, ptr %103, i32 %104
  store ptr %102, ptr %105, align 8
  br label %106

106:                                              ; preds = %31, %99, %91, %84, %76, %69, %60, %52, %44, %36
  br label %107

107:                                              ; preds = %106
  %108 = load i32, ptr %14, align 4
  %109 = add nsw i32 %108, 1
  store i32 %109, ptr %14, align 4
  br label %27, !llvm.loop !27

110:                                              ; preds = %27
  br label %111

111:                                              ; preds = %110
  %112 = load ptr, ptr %10, align 4
  %113 = load ptr, ptr %112, align 4
  %114 = getelementptr inbounds %struct.JNINativeInterface_, ptr %113, i32 0, i32 87
  %115 = load ptr, ptr %114, align 4
  %116 = load ptr, ptr %11, align 4
  %117 = load ptr, ptr %7, align 4
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %9, align 4
  %120 = load ptr, ptr %10, align 4
  %121 = call x86_stdcallcc float %115(ptr noundef %120, ptr noundef %119, ptr noundef %118, ptr noundef %117, ptr noundef %116)
  ret float %121
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport float @JNI_CallStaticFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca float, align 4
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 4
  %10 = load ptr, ptr %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 136
  %12 = load ptr, ptr %11, align 4
  %13 = load ptr, ptr %8, align 4
  %14 = load ptr, ptr %4, align 4
  %15 = load ptr, ptr %5, align 4
  %16 = load ptr, ptr %6, align 4
  %17 = call x86_stdcallcc float %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store float %17, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %18 = load float, ptr %7, align 4
  ret float %18
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport float @JNI_CallStaticFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !28

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 137
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  %118 = call x86_stdcallcc float %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret float %118
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport double @JNI_CallDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca double, align 8
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 4
  %10 = load ptr, ptr %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 59
  %12 = load ptr, ptr %11, align 4
  %13 = load ptr, ptr %8, align 4
  %14 = load ptr, ptr %4, align 4
  %15 = load ptr, ptr %5, align 4
  %16 = load ptr, ptr %6, align 4
  %17 = call x86_stdcallcc double %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store double %17, ptr %7, align 8
  call void @llvm.va_end(ptr %8)
  %18 = load double, ptr %7, align 8
  ret double %18
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport double @JNI_CallDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !29

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 60
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  %118 = call x86_stdcallcc double %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret double %118
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport double @JNI_CallNonvirtualDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca double, align 8
  %10 = alloca ptr, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  call void @llvm.va_start(ptr %10)
  %11 = load ptr, ptr %8, align 4
  %12 = load ptr, ptr %11, align 4
  %13 = getelementptr inbounds %struct.JNINativeInterface_, ptr %12, i32 0, i32 89
  %14 = load ptr, ptr %13, align 4
  %15 = load ptr, ptr %10, align 4
  %16 = load ptr, ptr %5, align 4
  %17 = load ptr, ptr %6, align 4
  %18 = load ptr, ptr %7, align 4
  %19 = load ptr, ptr %8, align 4
  %20 = call x86_stdcallcc double %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store double %20, ptr %9, align 8
  call void @llvm.va_end(ptr %10)
  %21 = load double, ptr %9, align 8
  ret double %21
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport double @JNI_CallNonvirtualDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  %11 = alloca ptr, align 4
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  store ptr %3, ptr %7, align 4
  store ptr %2, ptr %8, align 4
  store ptr %1, ptr %9, align 4
  store ptr %0, ptr %10, align 4
  br label %15

15:                                               ; preds = %5
  %16 = load ptr, ptr %10, align 4
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 4
  %20 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 0
  %21 = load ptr, ptr %7, align 4
  %22 = load ptr, ptr %10, align 4
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = mul i32 %24, 8
  %26 = alloca i8, i32 %25, align 16
  store ptr %26, ptr %11, align 4
  store i32 0, ptr %14, align 4
  br label %27

27:                                               ; preds = %107, %15
  %28 = load i32, ptr %14, align 4
  %29 = load i32, ptr %13, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %110

31:                                               ; preds = %27
  %32 = load i32, ptr %14, align 4
  %33 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %106 [
    i32 90, label %36
    i32 66, label %44
    i32 83, label %52
    i32 67, label %60
    i32 73, label %69
    i32 74, label %76
    i32 68, label %84
    i32 70, label %91
    i32 76, label %99
  ]

36:                                               ; preds = %31
  %37 = load ptr, ptr %6, align 4
  %38 = getelementptr inbounds i8, ptr %37, i32 4
  store ptr %38, ptr %6, align 4
  %39 = load i32, ptr %37, align 4
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %11, align 4
  %42 = load i32, ptr %14, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %41, i32 %42
  store i8 %40, ptr %43, align 8
  br label %106

44:                                               ; preds = %31
  %45 = load ptr, ptr %6, align 4
  %46 = getelementptr inbounds i8, ptr %45, i32 4
  store ptr %46, ptr %6, align 4
  %47 = load i32, ptr %45, align 4
  %48 = trunc i32 %47 to i8
  %49 = load ptr, ptr %11, align 4
  %50 = load i32, ptr %14, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %49, i32 %50
  store i8 %48, ptr %51, align 8
  br label %106

52:                                               ; preds = %31
  %53 = load ptr, ptr %6, align 4
  %54 = getelementptr inbounds i8, ptr %53, i32 4
  store ptr %54, ptr %6, align 4
  %55 = load i32, ptr %53, align 4
  %56 = trunc i32 %55 to i16
  %57 = load ptr, ptr %11, align 4
  %58 = load i32, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %57, i32 %58
  store i16 %56, ptr %59, align 8
  br label %106

60:                                               ; preds = %31
  %61 = load ptr, ptr %6, align 4
  %62 = getelementptr inbounds i8, ptr %61, i32 4
  store ptr %62, ptr %6, align 4
  %63 = load i32, ptr %61, align 4
  %64 = trunc i32 %63 to i16
  %65 = zext i16 %64 to i32
  %66 = load ptr, ptr %11, align 4
  %67 = load i32, ptr %14, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %66, i32 %67
  store i32 %65, ptr %68, align 8
  br label %106

69:                                               ; preds = %31
  %70 = load ptr, ptr %6, align 4
  %71 = getelementptr inbounds i8, ptr %70, i32 4
  store ptr %71, ptr %6, align 4
  %72 = load i32, ptr %70, align 4
  %73 = load ptr, ptr %11, align 4
  %74 = load i32, ptr %14, align 4
  %75 = getelementptr inbounds %union.jvalue, ptr %73, i32 %74
  store i32 %72, ptr %75, align 8
  br label %106

76:                                               ; preds = %31
  %77 = load ptr, ptr %6, align 4
  %78 = getelementptr inbounds i8, ptr %77, i32 4
  store ptr %78, ptr %6, align 4
  %79 = load i32, ptr %77, align 4
  %80 = sext i32 %79 to i64
  %81 = load ptr, ptr %11, align 4
  %82 = load i32, ptr %14, align 4
  %83 = getelementptr inbounds %union.jvalue, ptr %81, i32 %82
  store i64 %80, ptr %83, align 8
  br label %106

84:                                               ; preds = %31
  %85 = load ptr, ptr %6, align 4
  %86 = getelementptr inbounds i8, ptr %85, i32 8
  store ptr %86, ptr %6, align 4
  %87 = load double, ptr %85, align 4
  %88 = load ptr, ptr %11, align 4
  %89 = load i32, ptr %14, align 4
  %90 = getelementptr inbounds %union.jvalue, ptr %88, i32 %89
  store double %87, ptr %90, align 8
  br label %106

91:                                               ; preds = %31
  %92 = load ptr, ptr %6, align 4
  %93 = getelementptr inbounds i8, ptr %92, i32 8
  store ptr %93, ptr %6, align 4
  %94 = load double, ptr %92, align 4
  %95 = fptrunc double %94 to float
  %96 = load ptr, ptr %11, align 4
  %97 = load i32, ptr %14, align 4
  %98 = getelementptr inbounds %union.jvalue, ptr %96, i32 %97
  store float %95, ptr %98, align 8
  br label %106

99:                                               ; preds = %31
  %100 = load ptr, ptr %6, align 4
  %101 = getelementptr inbounds i8, ptr %100, i32 4
  store ptr %101, ptr %6, align 4
  %102 = load ptr, ptr %100, align 4
  %103 = load ptr, ptr %11, align 4
  %104 = load i32, ptr %14, align 4
  %105 = getelementptr inbounds %union.jvalue, ptr %103, i32 %104
  store ptr %102, ptr %105, align 8
  br label %106

106:                                              ; preds = %31, %99, %91, %84, %76, %69, %60, %52, %44, %36
  br label %107

107:                                              ; preds = %106
  %108 = load i32, ptr %14, align 4
  %109 = add nsw i32 %108, 1
  store i32 %109, ptr %14, align 4
  br label %27, !llvm.loop !30

110:                                              ; preds = %27
  br label %111

111:                                              ; preds = %110
  %112 = load ptr, ptr %10, align 4
  %113 = load ptr, ptr %112, align 4
  %114 = getelementptr inbounds %struct.JNINativeInterface_, ptr %113, i32 0, i32 90
  %115 = load ptr, ptr %114, align 4
  %116 = load ptr, ptr %11, align 4
  %117 = load ptr, ptr %7, align 4
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %9, align 4
  %120 = load ptr, ptr %10, align 4
  %121 = call x86_stdcallcc double %115(ptr noundef %120, ptr noundef %119, ptr noundef %118, ptr noundef %117, ptr noundef %116)
  ret double %121
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport double @JNI_CallStaticDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca double, align 8
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 4
  %10 = load ptr, ptr %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 139
  %12 = load ptr, ptr %11, align 4
  %13 = load ptr, ptr %8, align 4
  %14 = load ptr, ptr %4, align 4
  %15 = load ptr, ptr %5, align 4
  %16 = load ptr, ptr %6, align 4
  %17 = call x86_stdcallcc double %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store double %17, ptr %7, align 8
  call void @llvm.va_end(ptr %8)
  %18 = load double, ptr %7, align 8
  ret double %18
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport double @JNI_CallStaticDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !31

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 140
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  %118 = call x86_stdcallcc double %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret double %118
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport ptr @JNI_NewObject(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 4
  %10 = load ptr, ptr %9, align 4
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 29
  %12 = load ptr, ptr %11, align 4
  %13 = load ptr, ptr %8, align 4
  %14 = load ptr, ptr %4, align 4
  %15 = load ptr, ptr %5, align 4
  %16 = load ptr, ptr %6, align 4
  %17 = call x86_stdcallcc ptr %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store ptr %17, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %18 = load ptr, ptr %7, align 4
  ret ptr %18
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport ptr @JNI_NewObjectV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !32

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 30
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  %118 = call x86_stdcallcc ptr %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret ptr %118
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport void @JNI_CallVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %7)
  %8 = load ptr, ptr %6, align 4
  %9 = load ptr, ptr %8, align 4
  %10 = getelementptr inbounds %struct.JNINativeInterface_, ptr %9, i32 0, i32 62
  %11 = load ptr, ptr %10, align 4
  %12 = load ptr, ptr %7, align 4
  %13 = load ptr, ptr %4, align 4
  %14 = load ptr, ptr %5, align 4
  %15 = load ptr, ptr %6, align 4
  call x86_stdcallcc void %11(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12)
  call void @llvm.va_end(ptr %7)
  ret void
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport void @JNI_CallVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !33

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 63
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  call x86_stdcallcc void %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret void
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport void @JNI_CallNonvirtualVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  call void @llvm.va_start(ptr %9)
  %10 = load ptr, ptr %8, align 4
  %11 = load ptr, ptr %10, align 4
  %12 = getelementptr inbounds %struct.JNINativeInterface_, ptr %11, i32 0, i32 92
  %13 = load ptr, ptr %12, align 4
  %14 = load ptr, ptr %9, align 4
  %15 = load ptr, ptr %5, align 4
  %16 = load ptr, ptr %6, align 4
  %17 = load ptr, ptr %7, align 4
  %18 = load ptr, ptr %8, align 4
  call x86_stdcallcc void %13(ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15, ptr noundef %14)
  call void @llvm.va_end(ptr %9)
  ret void
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport void @JNI_CallNonvirtualVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  %11 = alloca ptr, align 4
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  store ptr %3, ptr %7, align 4
  store ptr %2, ptr %8, align 4
  store ptr %1, ptr %9, align 4
  store ptr %0, ptr %10, align 4
  br label %15

15:                                               ; preds = %5
  %16 = load ptr, ptr %10, align 4
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 4
  %20 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 0
  %21 = load ptr, ptr %7, align 4
  %22 = load ptr, ptr %10, align 4
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = mul i32 %24, 8
  %26 = alloca i8, i32 %25, align 16
  store ptr %26, ptr %11, align 4
  store i32 0, ptr %14, align 4
  br label %27

27:                                               ; preds = %107, %15
  %28 = load i32, ptr %14, align 4
  %29 = load i32, ptr %13, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %110

31:                                               ; preds = %27
  %32 = load i32, ptr %14, align 4
  %33 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %106 [
    i32 90, label %36
    i32 66, label %44
    i32 83, label %52
    i32 67, label %60
    i32 73, label %69
    i32 74, label %76
    i32 68, label %84
    i32 70, label %91
    i32 76, label %99
  ]

36:                                               ; preds = %31
  %37 = load ptr, ptr %6, align 4
  %38 = getelementptr inbounds i8, ptr %37, i32 4
  store ptr %38, ptr %6, align 4
  %39 = load i32, ptr %37, align 4
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %11, align 4
  %42 = load i32, ptr %14, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %41, i32 %42
  store i8 %40, ptr %43, align 8
  br label %106

44:                                               ; preds = %31
  %45 = load ptr, ptr %6, align 4
  %46 = getelementptr inbounds i8, ptr %45, i32 4
  store ptr %46, ptr %6, align 4
  %47 = load i32, ptr %45, align 4
  %48 = trunc i32 %47 to i8
  %49 = load ptr, ptr %11, align 4
  %50 = load i32, ptr %14, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %49, i32 %50
  store i8 %48, ptr %51, align 8
  br label %106

52:                                               ; preds = %31
  %53 = load ptr, ptr %6, align 4
  %54 = getelementptr inbounds i8, ptr %53, i32 4
  store ptr %54, ptr %6, align 4
  %55 = load i32, ptr %53, align 4
  %56 = trunc i32 %55 to i16
  %57 = load ptr, ptr %11, align 4
  %58 = load i32, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %57, i32 %58
  store i16 %56, ptr %59, align 8
  br label %106

60:                                               ; preds = %31
  %61 = load ptr, ptr %6, align 4
  %62 = getelementptr inbounds i8, ptr %61, i32 4
  store ptr %62, ptr %6, align 4
  %63 = load i32, ptr %61, align 4
  %64 = trunc i32 %63 to i16
  %65 = zext i16 %64 to i32
  %66 = load ptr, ptr %11, align 4
  %67 = load i32, ptr %14, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %66, i32 %67
  store i32 %65, ptr %68, align 8
  br label %106

69:                                               ; preds = %31
  %70 = load ptr, ptr %6, align 4
  %71 = getelementptr inbounds i8, ptr %70, i32 4
  store ptr %71, ptr %6, align 4
  %72 = load i32, ptr %70, align 4
  %73 = load ptr, ptr %11, align 4
  %74 = load i32, ptr %14, align 4
  %75 = getelementptr inbounds %union.jvalue, ptr %73, i32 %74
  store i32 %72, ptr %75, align 8
  br label %106

76:                                               ; preds = %31
  %77 = load ptr, ptr %6, align 4
  %78 = getelementptr inbounds i8, ptr %77, i32 4
  store ptr %78, ptr %6, align 4
  %79 = load i32, ptr %77, align 4
  %80 = sext i32 %79 to i64
  %81 = load ptr, ptr %11, align 4
  %82 = load i32, ptr %14, align 4
  %83 = getelementptr inbounds %union.jvalue, ptr %81, i32 %82
  store i64 %80, ptr %83, align 8
  br label %106

84:                                               ; preds = %31
  %85 = load ptr, ptr %6, align 4
  %86 = getelementptr inbounds i8, ptr %85, i32 8
  store ptr %86, ptr %6, align 4
  %87 = load double, ptr %85, align 4
  %88 = load ptr, ptr %11, align 4
  %89 = load i32, ptr %14, align 4
  %90 = getelementptr inbounds %union.jvalue, ptr %88, i32 %89
  store double %87, ptr %90, align 8
  br label %106

91:                                               ; preds = %31
  %92 = load ptr, ptr %6, align 4
  %93 = getelementptr inbounds i8, ptr %92, i32 8
  store ptr %93, ptr %6, align 4
  %94 = load double, ptr %92, align 4
  %95 = fptrunc double %94 to float
  %96 = load ptr, ptr %11, align 4
  %97 = load i32, ptr %14, align 4
  %98 = getelementptr inbounds %union.jvalue, ptr %96, i32 %97
  store float %95, ptr %98, align 8
  br label %106

99:                                               ; preds = %31
  %100 = load ptr, ptr %6, align 4
  %101 = getelementptr inbounds i8, ptr %100, i32 4
  store ptr %101, ptr %6, align 4
  %102 = load ptr, ptr %100, align 4
  %103 = load ptr, ptr %11, align 4
  %104 = load i32, ptr %14, align 4
  %105 = getelementptr inbounds %union.jvalue, ptr %103, i32 %104
  store ptr %102, ptr %105, align 8
  br label %106

106:                                              ; preds = %31, %99, %91, %84, %76, %69, %60, %52, %44, %36
  br label %107

107:                                              ; preds = %106
  %108 = load i32, ptr %14, align 4
  %109 = add nsw i32 %108, 1
  store i32 %109, ptr %14, align 4
  br label %27, !llvm.loop !34

110:                                              ; preds = %27
  br label %111

111:                                              ; preds = %110
  %112 = load ptr, ptr %10, align 4
  %113 = load ptr, ptr %112, align 4
  %114 = getelementptr inbounds %struct.JNINativeInterface_, ptr %113, i32 0, i32 93
  %115 = load ptr, ptr %114, align 4
  %116 = load ptr, ptr %11, align 4
  %117 = load ptr, ptr %7, align 4
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %9, align 4
  %120 = load ptr, ptr %10, align 4
  call x86_stdcallcc void %115(ptr noundef %120, ptr noundef %119, ptr noundef %118, ptr noundef %117, ptr noundef %116)
  ret void
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport void @JNI_CallStaticVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  call void @llvm.va_start(ptr %7)
  %8 = load ptr, ptr %6, align 4
  %9 = load ptr, ptr %8, align 4
  %10 = getelementptr inbounds %struct.JNINativeInterface_, ptr %9, i32 0, i32 142
  %11 = load ptr, ptr %10, align 4
  %12 = load ptr, ptr %7, align 4
  %13 = load ptr, ptr %4, align 4
  %14 = load ptr, ptr %5, align 4
  %15 = load ptr, ptr %6, align 4
  call x86_stdcallcc void %11(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12)
  call void @llvm.va_end(ptr %7)
  ret void
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport void @JNI_CallStaticVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 4
  %15 = load ptr, ptr %14, align 4
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 4
  %18 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 0
  %19 = load ptr, ptr %6, align 4
  %20 = load ptr, ptr %8, align 4
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 16
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %105, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %108

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %104 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %89
    i32 76, label %97
  ]

34:                                               ; preds = %29
  %35 = load ptr, ptr %5, align 4
  %36 = getelementptr inbounds i8, ptr %35, i32 4
  store ptr %36, ptr %5, align 4
  %37 = load i32, ptr %35, align 4
  %38 = trunc i32 %37 to i8
  %39 = load ptr, ptr %9, align 4
  %40 = load i32, ptr %12, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %39, i32 %40
  store i8 %38, ptr %41, align 8
  br label %104

42:                                               ; preds = %29
  %43 = load ptr, ptr %5, align 4
  %44 = getelementptr inbounds i8, ptr %43, i32 4
  store ptr %44, ptr %5, align 4
  %45 = load i32, ptr %43, align 4
  %46 = trunc i32 %45 to i8
  %47 = load ptr, ptr %9, align 4
  %48 = load i32, ptr %12, align 4
  %49 = getelementptr inbounds %union.jvalue, ptr %47, i32 %48
  store i8 %46, ptr %49, align 8
  br label %104

50:                                               ; preds = %29
  %51 = load ptr, ptr %5, align 4
  %52 = getelementptr inbounds i8, ptr %51, i32 4
  store ptr %52, ptr %5, align 4
  %53 = load i32, ptr %51, align 4
  %54 = trunc i32 %53 to i16
  %55 = load ptr, ptr %9, align 4
  %56 = load i32, ptr %12, align 4
  %57 = getelementptr inbounds %union.jvalue, ptr %55, i32 %56
  store i16 %54, ptr %57, align 8
  br label %104

58:                                               ; preds = %29
  %59 = load ptr, ptr %5, align 4
  %60 = getelementptr inbounds i8, ptr %59, i32 4
  store ptr %60, ptr %5, align 4
  %61 = load i32, ptr %59, align 4
  %62 = trunc i32 %61 to i16
  %63 = zext i16 %62 to i32
  %64 = load ptr, ptr %9, align 4
  %65 = load i32, ptr %12, align 4
  %66 = getelementptr inbounds %union.jvalue, ptr %64, i32 %65
  store i32 %63, ptr %66, align 8
  br label %104

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %104

74:                                               ; preds = %29
  %75 = load ptr, ptr %5, align 4
  %76 = getelementptr inbounds i8, ptr %75, i32 4
  store ptr %76, ptr %5, align 4
  %77 = load i32, ptr %75, align 4
  %78 = sext i32 %77 to i64
  %79 = load ptr, ptr %9, align 4
  %80 = load i32, ptr %12, align 4
  %81 = getelementptr inbounds %union.jvalue, ptr %79, i32 %80
  store i64 %78, ptr %81, align 8
  br label %104

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = getelementptr inbounds i8, ptr %83, i32 8
  store ptr %84, ptr %5, align 4
  %85 = load double, ptr %83, align 4
  %86 = load ptr, ptr %9, align 4
  %87 = load i32, ptr %12, align 4
  %88 = getelementptr inbounds %union.jvalue, ptr %86, i32 %87
  store double %85, ptr %88, align 8
  br label %104

89:                                               ; preds = %29
  %90 = load ptr, ptr %5, align 4
  %91 = getelementptr inbounds i8, ptr %90, i32 8
  store ptr %91, ptr %5, align 4
  %92 = load double, ptr %90, align 4
  %93 = fptrunc double %92 to float
  %94 = load ptr, ptr %9, align 4
  %95 = load i32, ptr %12, align 4
  %96 = getelementptr inbounds %union.jvalue, ptr %94, i32 %95
  store float %93, ptr %96, align 8
  br label %104

97:                                               ; preds = %29
  %98 = load ptr, ptr %5, align 4
  %99 = getelementptr inbounds i8, ptr %98, i32 4
  store ptr %99, ptr %5, align 4
  %100 = load ptr, ptr %98, align 4
  %101 = load ptr, ptr %9, align 4
  %102 = load i32, ptr %12, align 4
  %103 = getelementptr inbounds %union.jvalue, ptr %101, i32 %102
  store ptr %100, ptr %103, align 8
  br label %104

104:                                              ; preds = %29, %97, %89, %82, %74, %67, %58, %50, %42, %34
  br label %105

105:                                              ; preds = %104
  %106 = load i32, ptr %12, align 4
  %107 = add nsw i32 %106, 1
  store i32 %107, ptr %12, align 4
  br label %25, !llvm.loop !35

108:                                              ; preds = %25
  br label %109

109:                                              ; preds = %108
  %110 = load ptr, ptr %8, align 4
  %111 = load ptr, ptr %110, align 4
  %112 = getelementptr inbounds %struct.JNINativeInterface_, ptr %111, i32 0, i32 143
  %113 = load ptr, ptr %112, align 4
  %114 = load ptr, ptr %9, align 4
  %115 = load ptr, ptr %6, align 4
  %116 = load ptr, ptr %7, align 4
  %117 = load ptr, ptr %8, align 4
  call x86_stdcallcc void %113(ptr noundef %117, ptr noundef %116, ptr noundef %115, ptr noundef %114)
  ret void
}

; Function Attrs: noinline nounwind optnone
define linkonce_odr dso_local i32 @_vsprintf_l(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 comdat {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  store ptr %3, ptr %5, align 4
  store ptr %2, ptr %6, align 4
  store ptr %1, ptr %7, align 4
  store ptr %0, ptr %8, align 4
  %9 = load ptr, ptr %5, align 4
  %10 = load ptr, ptr %6, align 4
  %11 = load ptr, ptr %7, align 4
  %12 = load ptr, ptr %8, align 4
  %13 = call i32 @_vsnprintf_l(ptr noundef %12, i32 noundef -1, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  ret i32 %13
}

; Function Attrs: noinline nounwind optnone
define linkonce_odr dso_local i32 @_vsnprintf_l(ptr noundef %0, i32 noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 comdat {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca i32, align 4
  %10 = alloca ptr, align 4
  %11 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  store ptr %3, ptr %7, align 4
  store ptr %2, ptr %8, align 4
  store i32 %1, ptr %9, align 4
  store ptr %0, ptr %10, align 4
  %12 = load ptr, ptr %6, align 4
  %13 = load ptr, ptr %7, align 4
  %14 = load ptr, ptr %8, align 4
  %15 = load i32, ptr %9, align 4
  %16 = load ptr, ptr %10, align 4
  %17 = call ptr @__local_stdio_printf_options()
  %18 = load i64, ptr %17, align 8
  %19 = or i64 %18, 1
  %20 = call i32 @__stdio_common_vsprintf(i64 noundef %19, ptr noundef %16, i32 noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12)
  store i32 %20, ptr %11, align 4
  %21 = load i32, ptr %11, align 4
  %22 = icmp slt i32 %21, 0
  br i1 %22, label %23, label %24

23:                                               ; preds = %5
  br label %26

24:                                               ; preds = %5
  %25 = load i32, ptr %11, align 4
  br label %26

26:                                               ; preds = %24, %23
  %27 = phi i32 [ -1, %23 ], [ %25, %24 ]
  ret i32 %27
}

declare dso_local i32 @__stdio_common_vsprintf(i64 noundef, ptr noundef, i32 noundef, ptr noundef, ptr noundef, ptr noundef) #2

; Function Attrs: noinline nounwind optnone
define linkonce_odr dso_local ptr @__local_stdio_printf_options() #0 comdat {
  ret ptr @__local_stdio_printf_options._OptionsStorage
}

attributes #0 = { noinline nounwind optnone "frame-pointer"="all" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="pentium4" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
attributes #1 = { nocallback nofree nosync nounwind willreturn }
attributes #2 = { "frame-pointer"="all" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="pentium4" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }

!llvm.module.flags = !{!0, !1, !2}
!llvm.ident = !{!3}

!0 = !{i32 1, !"NumRegisterParameters", i32 0}
!1 = !{i32 1, !"wchar_size", i32 2}
!2 = !{i32 7, !"frame-pointer", i32 2}
!3 = !{!"clang version 15.0.2"}
!4 = distinct !{!4, !5}
!5 = !{!"llvm.loop.mustprogress"}
!6 = distinct !{!6, !5}
!7 = distinct !{!7, !5}
!8 = distinct !{!8, !5}
!9 = distinct !{!9, !5}
!10 = distinct !{!10, !5}
!11 = distinct !{!11, !5}
!12 = distinct !{!12, !5}
!13 = distinct !{!13, !5}
!14 = distinct !{!14, !5}
!15 = distinct !{!15, !5}
!16 = distinct !{!16, !5}
!17 = distinct !{!17, !5}
!18 = distinct !{!18, !5}
!19 = distinct !{!19, !5}
!20 = distinct !{!20, !5}
!21 = distinct !{!21, !5}
!22 = distinct !{!22, !5}
!23 = distinct !{!23, !5}
!24 = distinct !{!24, !5}
!25 = distinct !{!25, !5}
!26 = distinct !{!26, !5}
!27 = distinct !{!27, !5}
!28 = distinct !{!28, !5}
!29 = distinct !{!29, !5}
!30 = distinct !{!30, !5}
!31 = distinct !{!31, !5}
!32 = distinct !{!32, !5}
!33 = distinct !{!33, !5}
!34 = distinct !{!34, !5}
!35 = distinct !{!35, !5}
